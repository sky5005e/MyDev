using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL;
using Incentex.DAL.Common;

public partial class MyAccount_MySettings_MyBilling : PageBase
{
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    UserInformationRepository objUsrInfoRep = new UserInformationRepository();
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Billing Information";
            if (IncentexGlobal.IsIEFromStore)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/MySettings/MySettingMenuOptions.aspx";
            }

            BindDDL(sender, e);
            DisplayData(sender, e);
        }
    }
    #region Functions

    void BindDDL(object sender, EventArgs e)
    {

        
        CountryRepository objCountryBillingRepo = new CountryRepository();
        List<INC_Country> objBillingCountryList = objCountryBillingRepo.GetAll();
        Common.BindDDL(DrpBillingCountry, objBillingCountryList, "sCountryName", "iCountryID", "-select country-");
        DrpBillingCountry.SelectedValue = DrpBillingCountry.Items.FindByText("United States").Value;
        DrpBillingCountry_SelectedIndexChanged(sender, e);
       
    }

    /// <summary>
    /// Display Existing Record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DisplayData(object sender, EventArgs e)
    {
        
        //Billing
        CompanyEmployeeContactInfo objBillingInfo = objCmpEmpContRep.GetFirstBillingDetailByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);
        if (objBillingInfo != null)
        {
            TxtBillingCompanyName.Text = objBillingInfo.CompanyName;
            TxtCO.Text = objBillingInfo.BillingCO;
            TxtBillingManager.Text = objBillingInfo.Manager;

            DrpBillingCountry.SelectedValue = objBillingInfo.CountryID.ToString();
            DrpBillingCountry_SelectedIndexChanged(sender, e);

            DrpBillingState.SelectedValue = objBillingInfo.StateID.ToString();
            DrpBillingState_SelectedIndexChanged(sender, e);


            DrpBillingCity.SelectedValue = objBillingInfo.CityID.ToString();

            //TxtStation.Text = objBillingInfo.Station;
            TxtBillingAddressLine1.Text = objBillingInfo.Address;
            TxtBillingAddressLine2.Text = objBillingInfo.Address2;
            TxtBillingZip.Text = objBillingInfo.ZipCode;
            TxtMobile.Text = objBillingInfo.Mobile;
            TxtEmail.Text = objBillingInfo.Email;
            TxtTelephone.Text = objBillingInfo.Telephone;
            TxtBillingCompanyName.Focus();
        }

    }

    

    #endregion
    #region Events
    protected void DrpBillingCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(DrpBillingCountry.SelectedValue));

        DrpBillingState.Items.Clear();
        DrpBillingCity.Items.Clear();
        DrpBillingCity.Items.Insert(0, new ListItem("-select city-", "0"));
        Common.BindDDL(DrpBillingState, objStateList, "sStatename", "iStateID", "-select state-");
        if (DrpBillingCountry.SelectedValue == "0" && DrpBillingState.SelectedValue == "0")
        {
            DrpBillingCity.Items.Remove(new ListItem("-Other-", "-Other-"));
        }
    }

    protected void DrpBillingState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(DrpBillingState.SelectedValue));
        DrpBillingCity.Items.Clear();
        Common.BindDDL(DrpBillingCity, objCityList, "sCityName", "iCityID", "-select city-");
        if (DrpBillingState.SelectedIndex > 0)
        {
           // DrpBillingCity.Items.Add(new ListItem("-Other-", "-1"));
            DrpBillingCity.Items.Insert(1,"-Other-");
            
        }
    }
    
    protected void LnkAppyChanges_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            Int64 CityID = 0;
            //Add new city to INC_City Table
            if (PnlCityOther.Visible == true && DrpBillingCity.SelectedValue == "-Other-")
            {
                CityRepository objCityRep = new CityRepository();
                Int64 countryID = Convert.ToInt64(DrpBillingCountry.SelectedItem.Value);
                Int64 stateID = Convert.ToInt64(DrpBillingState.SelectedItem.Value);

                INC_City objCity = objCityRep.CheckIfExist(countryID, stateID, txtCity.Text.Trim());
                if (objCity == null)
                {
                    INC_City objCity1 = new INC_City();
                    objCity1.iCountryID = countryID;
                    objCity1.iStateID = stateID;
                    objCity1.sCityName = txtCity.Text.Trim();
                    objCityRep.Insert(objCity1);
                    objCityRep.SubmitChanges();
                    CityID = objCity1.iCityID;
                }
                else
                    CityID = objCity.iCityID;
            }
            //End
            else
                CityID = Convert.ToInt64(DrpBillingCity.SelectedValue);
            //End

            // insert or update Billing
            CompanyEmployeeContactInfo objBillingInfo = objCmpEmpContRep.GetFirstBillingDetailByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);

            if (objBillingInfo == null)
                objBillingInfo = new CompanyEmployeeContactInfo();

            objBillingInfo.CompanyName = TxtBillingCompanyName.Text.Trim();
            objBillingInfo.UserInfoID = (IncentexGlobal.CurrentMember.UserInfoID);
            //First name for the billing
            objBillingInfo.BillingCO = TxtCO.Text.Trim();
            //Last name for the Billing
            objBillingInfo.Manager = TxtBillingManager.Text.Trim();
            objBillingInfo.CountryID = Common.GetLongNull(DrpBillingCountry.SelectedValue, true);
            objBillingInfo.StateID = Common.GetLongNull(DrpBillingState.SelectedValue, true);
            objBillingInfo.CityID = Int64.Parse(DrpBillingCity.SelectedItem.Value);
            objBillingInfo.Address = TxtBillingAddressLine1.Text.Trim();
            objBillingInfo.Address2 = TxtBillingAddressLine2.Text.Trim();
            objBillingInfo.ZipCode = TxtBillingZip.Text.Trim();
            objBillingInfo.Name = TxtCO.Text.Trim() + " " + TxtBillingManager.Text.Trim();
            objBillingInfo.Email = TxtEmail.Text.Trim();
            objBillingInfo.Mobile = TxtMobile.Text.Trim();
            objBillingInfo.Telephone = TxtTelephone.Text.Trim();
            objBillingInfo.OrderType = "MySetting";
            objBillingInfo.ContactInfoType = DAEnums.CompanyEmployeeContactInfo.Billing.ToString();

            CompanyEmployeeContactInfo objBillingInfo1 = objCmpEmpContRep.GetFirstBillingDetailByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);

            if (objBillingInfo1 == null)
                objCmpEmpContRep.Insert(objBillingInfo);
            objCmpEmpContRep.SubmitChanges();
            BindDDL(sender, e);
            DisplayData(sender, e);

            lblMsg.Text = "Record Saved Successfully !!!";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

            if (ex.ToString().Contains("Cannot insert duplicate key"))
                lblMsg.Text = "Record already exists.";
            else
                lblMsg.Text = "Error in saving record ...";

        }
    }

    #endregion

    protected void DrpBillingCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DrpBillingCity.SelectedValue == "-Other-")
            PnlCityOther.Visible = true;
        else
            PnlCityOther.Visible = false;
    }
}
