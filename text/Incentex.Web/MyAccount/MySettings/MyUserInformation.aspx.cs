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

public partial class MyAccount_MySettings_MyUserInformation : PageBase
{
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    UserInformationRepository objUsrInfoRep = new UserInformationRepository();
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((Label)Master.FindControl("lblPageHeading")).Text = "User Information Section";
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

        CountryRepository objCountryRepo = new CountryRepository();
        List<INC_Country> objCountryList = objCountryRepo.GetAll();
        Common.BindDDL(DrpCountry, objCountryList, "sCountryName", "iCountryID", "-select country-");
        DrpCountry.SelectedValue = DrpCountry.Items.FindByText("United States").Value;
        DrpCountry_SelectedIndexChanged(sender, e); 
    }

    /// <summary>
    /// Display Existing Record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DisplayData(object sender, EventArgs e)
    {
        UserInformation objUserInfo = objUsrInfoRep.GetById(IncentexGlobal.CurrentMember.UserInfoID);
        Company objCompany = new CompanyRepository().GetById((long)objUserInfo.CompanyId);
        TxtCompanyName.Text = objCompany.CompanyName;
        TxtFirstName.Text = objUserInfo.FirstName;
        TxtLastName.Text = objUserInfo.LastName;
        TxtZip.Text = objUserInfo.ZipCode;
        TxtTelephone.Text = objUserInfo.Telephone;
        TxtAddress2.Text = objUserInfo.Address2;
        TxtMobile.Text = objUserInfo.Mobile;
        TxtEmail.Text = objUserInfo.LoginEmail;
        TxtAddress.Text = objUserInfo.Address1;

        DrpCountry.SelectedValue = objUserInfo.CountryId.ToString();
        DrpCountry_SelectedIndexChanged(sender, e);

        DrpState.SelectedValue = objUserInfo.StateId.ToString();
        DrpState_SelectedIndexChanged(sender, e);


        DrpCity.SelectedValue = objUserInfo.CityId.ToString();
        //photo
        CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);       

        TxtCompanyName.Focus();
        
    }
    
    #endregion
    protected void DrpCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(DrpCountry.SelectedValue));

        DrpState.Items.Clear();
        DrpCity.Items.Clear();
        DrpCity.Items.Insert(0, new ListItem("-select city-", "0"));
       
        Common.BindDDL(DrpState, objStateList, "sStatename", "iStateID", "-select state-");
        if (DrpCountry.SelectedValue == "0" && DrpState.SelectedValue == "0")
        {
            DrpCity.Items.Remove(new ListItem("-Other-", "-Other-"));

        }
        
    }
    protected void DrpState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(DrpState.SelectedValue));
        DrpCity.Items.Clear();
        Common.BindDDL(DrpCity, objCityList, "sCityName", "iCityID", "-select city-");
        if (DrpState.SelectedIndex > 0)
        {
            //DrpCity.Items.Add(new ListItem("-Other-", "-1"));
            DrpCity.Items.Insert(1, "-Other-");
        }
        
    }
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {

            Int64 CityID = 0;
            //Add new city to INC_City Table
            if (PnlCityOther.Visible == true && DrpCity.SelectedValue == "-Other-")
            {
                CityRepository objCityRep = new CityRepository();
                Int64 countryID = Convert.ToInt64(DrpCountry.SelectedItem.Value);
                Int64 stateID = Convert.ToInt64(DrpState.SelectedItem.Value);

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
                CityID = Convert.ToInt64(DrpCity.SelectedValue);
            //End

            #region User Information

            //update User information
            UserInformation objUserInfo = objUsrInfoRep.GetById(IncentexGlobal.CurrentMember.UserInfoID);
            objUserInfo.FirstName = TxtFirstName.Text;
            objUserInfo.LastName = TxtLastName.Text;
            objUserInfo.ZipCode = TxtZip.Text;
            objUserInfo.Telephone = TxtTelephone.Text;
            objUserInfo.Address2 = TxtAddress2.Text;
            objUserInfo.Mobile = TxtMobile.Text;
            objUserInfo.LoginEmail = TxtEmail.Text;
            objUserInfo.Address1 = TxtAddress.Text;
            objUserInfo.CountryId = Convert.ToInt64(DrpCountry.SelectedValue);
            objUserInfo.StateId = Convert.ToInt64(DrpState.SelectedValue);
            objUserInfo.CityId = CityID;
            objUsrInfoRep.SubmitChanges();
           
            #endregion

            #region User Image

            //save photo in CompanyEmployee
            CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

           
            objCompanyEmployeeRepository.SubmitChanges();

            #endregion
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
    protected void DrpCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DrpCity.SelectedValue == "-Other-")
            PnlCityOther.Visible = true;
        else
            PnlCityOther.Visible = false;
    }
}
