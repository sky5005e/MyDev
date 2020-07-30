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

public partial class MyAccount_MySetting : PageBase
{
    #region Members

    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    UserInformationRepository objUsrInfoRep = new UserInformationRepository();
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();

    CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Company;
            }
            return (CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType)ViewState["SortExp"];
        }
        set
        {
            ViewState["SortExp"] = value;
        }
    }

    Incentex.DAL.Common.DAEnums.SortOrderType SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"] = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
            }
            return (Incentex.DAL.Common.DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }

    long CompanyContactInfoID
    {
        get
        {
            if (ViewState["CompanyContactInfoID"] == null)
            {
                ViewState["CompanyContactInfoID"] = "0";
            }
            return Convert.ToInt64(ViewState["CompanyContactInfoID"]);
        }
        set
        {
            ViewState["CompanyContactInfoID"] = value;
        }
    }

    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((Label)Master.FindControl("lblPageHeading")).Text = "My Settings";
            if (IncentexGlobal.IsIEFromStore)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            }

            BindDDL(sender, e);
            DisplayData(sender, e);
        }
    }

    #endregion

    #region Functions

    void BindDDL(object sender, EventArgs e)
    {

        CountryRepository objCountryRepo = new CountryRepository();
        List<INC_Country> objCountryList = objCountryRepo.GetAll();
        Common.BindDDL(DrpCountry, objCountryList, "sCountryName", "iCountryID", "-select country-");
        DrpCountry.SelectedValue = DrpCountry.Items.FindByText("United States").Value;
        DrpCountry_SelectedIndexChanged(sender, e);

        CountryRepository objCountryBillingRepo = new CountryRepository();
        List<INC_Country> objBillingCountryList = objCountryBillingRepo.GetAll();
        Common.BindDDL(DrpBillingCountry, objBillingCountryList, "sCountryName", "iCountryID", "-select country-");
        DrpBillingCountry.SelectedValue = DrpBillingCountry.Items.FindByText("United States").Value;
        DrpBillingCountry_SelectedIndexChanged(sender, e);


        CountryRepository objCountryShippingRepo = new CountryRepository();
        List<INC_Country> objShippingCountryList = objCountryBillingRepo.GetAll();
        Common.BindDDL(DrpShipingCoutry, objShippingCountryList, "sCountryName", "iCountryID", "-select country-");
        DrpShipingCoutry.SelectedValue = DrpShipingCoutry.Items.FindByText("United States").Value;
        DrpShipingCoutry_SelectedIndexChanged(sender, e);

    }

    /// <summary>
    /// Display Existing Record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DisplayData(object sender, EventArgs e)
    {
        //UserInfo

        UserInformation objUserInfo = objUsrInfoRep.GetById(IncentexGlobal.CurrentMember.UserInfoID);
        Company objCompany = new CompanyRepository().GetById((long)objUserInfo.CompanyId);
        TxtCompanyName.Text = objCompany.CompanyName;
        TxtFirstName.Text = objUserInfo.FirstName;
        TxtLastName.Text = objUserInfo.LastName;
        TxtZip.Text = objUserInfo.ZipCode;
        TxtTelephone.Text = objUserInfo.Telephone;
        TxtAddress2.Text = objUserInfo.Address2;
        TxtMobile.Text = objUserInfo.Mobile;
        TxtEmail.Text = objUserInfo.Email;
        TxtAddress.Text = objUserInfo.Address1;

        DrpCountry.SelectedValue = objUserInfo.CountryId.ToString();
        DrpCountry_SelectedIndexChanged(sender, e);

        DrpState.SelectedValue = objUserInfo.StateId.ToString();
        DrpState_SelectedIndexChanged(sender, e);


        DrpCity.SelectedValue = objUserInfo.CityId.ToString();

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
            TxtBillingAddress.Text = objBillingInfo.Address;
            TxtBillingZip.Text = objBillingInfo.ZipCode;
        }


        //Shipping

        bindgrid();

        //photo
        CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        hdPriPhoto.Value = objCompanyEmployee.UploadImage;

    }

    /// <summary>
    /// Clear Shipping Fields
    /// </summary>
    void ClearShippingFields(object sender, EventArgs e)
    {

        TxtShippingCompany.Text = "";
        TxtShippingName.Text = "";
        TxtShipingTitle.Text = "";




        //DrpShipingState.SelectedValue = "0";



        DrpShippingCity.SelectedValue = "0";
        TxtShippingZip.Text = "";
        TxtShppingTelephone.Text = "";
        //Fax is Last Name now
        TxtShipingFAX.Text = "";
        TxtShippingMobile.Text = "";
        TxtShippingEmail.Text = "";
        TxtShipingAddress.Text = "";

        DrpShipingCoutry.SelectedValue = DrpShipingCoutry.Items.FindByText("United States").Value;
        DrpShipingCoutry_SelectedIndexChanged(sender, e);


    }

    #endregion

    #region Events

    protected void DrpCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(DrpCountry.SelectedValue));

        DrpState.Items.Clear();
        DrpCity.Items.Clear();
        DrpCity.Items.Insert(0, new ListItem("-select city-", "0"));
        Common.BindDDL(DrpState, objStateList, "sStatename", "iStateID", "-select state-");
    }

    protected void DrpState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(DrpState.SelectedValue));
        DrpCity.Items.Clear();
        Common.BindDDL(DrpCity, objCityList, "sCityName", "iCityID", "-select city-");
    }

    protected void DrpBillingCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(DrpBillingCountry.SelectedValue));

        DrpBillingState.Items.Clear();
        DrpBillingCity.Items.Clear();
        DrpBillingCity.Items.Insert(0, new ListItem("-select city-", "0"));
        Common.BindDDL(DrpBillingState, objStateList, "sStatename", "iStateID", "-select state-");
    }

    protected void DrpBillingState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(DrpBillingState.SelectedValue));
        DrpBillingCity.Items.Clear();
        Common.BindDDL(DrpBillingCity, objCityList, "sCityName", "iCityID", "-select city-");
    }

    protected void DrpShipingCoutry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(DrpShipingCoutry.SelectedValue));

        DrpShipingState.Items.Clear();
        DrpShipingState.ClearSelection();
        DrpShippingCity.Items.Clear();
        DrpShippingCity.Items.Insert(0, new ListItem("-select city-", "0"));
        Common.BindDDL(DrpShipingState, objStateList, "sStatename", "iStateID", "-select state-");
    }

    protected void DrpShipingState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(DrpShipingState.SelectedValue));
        DrpShippingCity.Items.Clear();
        Common.BindDDL(DrpShippingCity, objCityList, "sCityName", "iCityID", "-select city-");
    }

    protected void dtlShipping_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblMsg.Text = "";

        Int64 CompanyContactInfoID = 0;
        CompanyEmployeeContactInfo objShipInfo = null;
        switch (e.CommandName)
        {
            case "EditRec":
                //display billing record to edit
                CompanyContactInfoID = Convert.ToInt64(e.CommandArgument);
                objShipInfo = objCmpEmpContRep.GetShippingDetailsByContactInfoID(CompanyContactInfoID);

                if (objShipInfo != null)
                {
                    this.CompanyContactInfoID = CompanyContactInfoID;
                    TxtShippingCompany.Text = objShipInfo.CompanyName;
                    //Name is First Name Now for the shipping
                    TxtShippingName.Text = objShipInfo.Name;
                    TxtShipingTitle.Text = objShipInfo.Title;
                    DrpShipingCoutry.SelectedValue = Common.GetLongString(objShipInfo.CountryID);
                    DrpShipingCoutry_SelectedIndexChanged(sender, e);

                    DrpShipingState.SelectedValue = Common.GetLongString(objShipInfo.StateID);
                    DrpShipingState_SelectedIndexChanged(sender, e);

                    DrpShippingCity.SelectedValue = Common.GetLongString(objShipInfo.CityID);

                    TxtShippingZip.Text = objShipInfo.ZipCode;
                    TxtShppingTelephone.Text = objShipInfo.Telephone;
                    //Fax is Last name now for the shipping
                    TxtShipingFAX.Text = objShipInfo.Fax;
                    TxtShippingMobile.Text = objShipInfo.Mobile;
                    TxtShippingEmail.Text = objShipInfo.Email;
                    TxtShipingAddress.Text = objShipInfo.Address;

                }

                break;
            case "DeleteRec":
                try
                {
                    CompanyContactInfoID = Convert.ToInt64(e.CommandArgument);
                    objShipInfo = objCmpEmpContRep.GetShippingDetailsByContactInfoID(CompanyContactInfoID);
                    objCmpEmpContRep.Delete(objShipInfo);
                    objCmpEmpContRep.SubmitChanges();
                    lblMsg.Text = "Record deleted successfully ...";
                    bindgrid();
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex)).Number == 547)
                    {
                        lblMsg.Text = "Unable to delete Shipping information as it is being used in orders!!";
                    }
                    else
                    {
                        ErrHandler.WriteError(ex);
                        lblMsg.Text = "Error in deleting record ...";
                    }
                    
                }

                break;
            case "Sort":
                if (this.SortExp.ToString() == e.CommandArgument.ToString())
                {
                    if (this.SortOrder == Incentex.DAL.Common.DAEnums.SortOrderType.Asc)
                        this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Desc;
                    else
                        this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
                }
                else
                {
                    this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
                    this.SortExp = (CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType)Enum.Parse(typeof(CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType), e.CommandArgument.ToString());
                }
                bindgrid();
                break;
        }
    }

    /// <summary>
    /// Insert or update shipping address
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkBtnAddAnotherShiping_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        CompanyEmployeeContactInfo objShipInfo = new CompanyEmployeeContactInfo();

        string oldTitle = string.Empty; // Added by shehzad 5-jan-2011

        try
        {
            if (this.CompanyContactInfoID != 0)
            {
                objShipInfo = objCmpEmpContRep.GetShippingDetailsByContactInfoID(this.CompanyContactInfoID);
                oldTitle = objShipInfo.Title; // Added by shehzad 5-jan-2011
            }

            objShipInfo.CompanyName = TxtShippingCompany.Text;
            objShipInfo.Name = TxtShippingName.Text;
            objShipInfo.Title = TxtShipingTitle.Text;
            objShipInfo.CountryID = Common.GetLongNull(DrpShipingCoutry.SelectedValue, true);
            objShipInfo.StateID = Common.GetLongNull(DrpShipingState.SelectedValue, true);
            objShipInfo.CityID = Common.GetLongNull(DrpShippingCity.SelectedValue, true);
            objShipInfo.ZipCode = TxtShippingZip.Text;
            objShipInfo.Telephone = TxtShppingTelephone.Text;
            objShipInfo.Fax = TxtShipingFAX.Text;
            objShipInfo.Mobile = TxtShippingMobile.Text;
            objShipInfo.Email = TxtShippingEmail.Text;
            objShipInfo.Address = TxtShipingAddress.Text;
            objShipInfo.ContactInfoType = DAEnums.CompanyEmployeeContactInfo.Shipping.ToString();
            objShipInfo.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
            objShipInfo.OrderType = "MySetting";
            if (this.CompanyContactInfoID == 0)
            {
                // shehzad start 5-jan-2011 => Check for duplicate title
                if (oldTitle.ToLower() == TxtShipingTitle.Text.ToLower())
                {
                    lblMsg.Text = "Title already exists please enter another Title could not save shipping address.";
                    return;
                }
                else // Shehzad End
                {
                    //insert
                    objCmpEmpContRep.Insert(objShipInfo);
                }
            }

            objCmpEmpContRep.SubmitChanges();
            this.CompanyContactInfoID = 0;
            //lblMsg.Text = "Record Saved Successfully ...";
            bindgrid();

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

            if (ex.ToString().Contains("Cannot insert duplicate key"))
                lblMsg.Text = "Title already exists please enter another Title could not save shipping address.";
            else
                lblMsg.Text = "Error in saving shipping address ...";
        }
        ClearShippingFields(sender, e);
    }

    protected void lnkChangePassword_Click(object sender, EventArgs e)
    {
         lblMsg.Text = "";
        //VALIDATIONS 
         if (string.IsNullOrEmpty(TxtOldPassword.Text))
         {
             lblMsg.Text = "Please enter old password";
             return;
         }
         if (!string.IsNullOrEmpty(TxtNewPassword.Text))
         {
          
             if (IncentexGlobal.CurrentMember.Password != TxtOldPassword.Text.Trim())
             {
                 lblMsg.Text = "Please enter your correct Old Password !!";
                 return;
             }
             else
             {
                 lblMsg.Text = string.Empty;
             }
         }
        
        try
        {
            #region User Information

            //update User information
            UserInformation objUserInfo = objUsrInfoRep.GetById(IncentexGlobal.CurrentMember.UserInfoID);
            if (TxtNewPassword.Text != string.Empty || TxtConfirmNewPassword.Text != string.Empty)
            {
                objUserInfo.Password = TxtNewPassword.Text;
                objUsrInfoRep.SubmitChanges();
                IncentexGlobal.CurrentMember.Password = objUserInfo.Password;
                lblMsg.Text = "Password changed successfully";
                
            }
            else
            {
                lblMsg.Text = "Please enter old password or new password or confirm new password!!";
                return;
            }
            

            #endregion
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
    

    /// <summary>
    /// Update Record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LnkAppyChanges_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";

        #region Password Validation
        
        //VALIDATIONS 
        if (!string.IsNullOrEmpty(TxtNewPassword.Text))
        {
            if (IncentexGlobal.CurrentMember.Password != TxtOldPassword.Text.Trim())
            {
                lblMsg.Text = "Please enter your correct Old Password !!";
                return;
            }
            else
            {
                lblMsg.Text = string.Empty;
            }
        } 

        #endregion

        try
        {
            #region User Information

            //update User information
            UserInformation objUserInfo = objUsrInfoRep.GetById(IncentexGlobal.CurrentMember.UserInfoID);
            objUserInfo.FirstName = TxtFirstName.Text;
            objUserInfo.LastName = TxtLastName.Text;
            objUserInfo.ZipCode = TxtZip.Text;
            objUserInfo.Telephone = TxtTelephone.Text;
            objUserInfo.Address2 = TxtAddress2.Text;
            objUserInfo.Mobile = TxtMobile.Text;
            objUserInfo.Email = TxtEmail.Text;
            objUserInfo.Address1 = TxtAddress.Text;
            objUserInfo.CountryId = Convert.ToInt64(DrpCountry.SelectedValue);
            objUserInfo.StateId = Convert.ToInt64(DrpState.SelectedValue);
            objUserInfo.CityId = Convert.ToInt64(DrpCity.SelectedValue);

            if (TxtNewPassword.Text != string.Empty || TxtConfirmNewPassword.Text != string.Empty)
                objUserInfo.Password = TxtNewPassword.Text;

            objUsrInfoRep.SubmitChanges();

            #endregion

            #region User Image

            //save photo in CompanyEmployee
            CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

            if (objCompanyEmployee != null)
            {
                if (hdPriPhoto.Value != "")
                {
                    objCompanyEmployee.UploadImage = hdPriPhoto.Value;
                }
                else
                {
                    objCompanyEmployee.UploadImage = null;
                }
            }

            objCompanyEmployeeRepository.SubmitChanges();

            #endregion

            #region Billing Info

            // insert or update Billing
            CompanyEmployeeContactInfo objBillingInfo = objCmpEmpContRep.GetFirstBillingDetailByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);

            if (objBillingInfo == null)
                objBillingInfo = new CompanyEmployeeContactInfo();

            objBillingInfo.CompanyName = TxtBillingCompanyName.Text;
            objBillingInfo.UserInfoID = (IncentexGlobal.CurrentMember.UserInfoID);
            //First name for the billing
            objBillingInfo.BillingCO = TxtCO.Text;
            //Last name for the Billing
            objBillingInfo.Manager = TxtBillingManager.Text;
            objBillingInfo.CountryID = Common.GetLongNull(DrpBillingCountry.SelectedValue, true);
            objBillingInfo.StateID = Common.GetLongNull(DrpBillingState.SelectedValue, true);
            objBillingInfo.CityID = Common.GetLongNull(DrpBillingCity.SelectedValue, true);
            //objBillingInfo.Station = TxtStation.Text;
            objBillingInfo.Address = TxtBillingAddress.Text;
            objBillingInfo.ZipCode = TxtBillingZip.Text;
            
            // Shehzad Start 5-Jan-2011
            // Set the Name, Email, Mobile, Telephone from the User Information table
            //Full name
            objBillingInfo.Name = TxtCO.Text + " " + TxtBillingManager.Text;
            objBillingInfo.Email = TxtEmail.Text;
            objBillingInfo.Mobile = TxtMobile.Text;
            objBillingInfo.Telephone = TxtTelephone.Text;
            objBillingInfo.OrderType = "MySetting";
            // Shehzad End

            objBillingInfo.ContactInfoType = DAEnums.CompanyEmployeeContactInfo.Billing.ToString();

            CompanyEmployeeContactInfo objBillingInfo1 = objCmpEmpContRep.GetFirstBillingDetailByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);

            if (objBillingInfo1 == null)
                objCmpEmpContRep.Insert(objBillingInfo);

            objCmpEmpContRep.SubmitChanges();

            #endregion

            #region Shipping Info

            // Shehzad Start (5-Jan-2011) => Adds the Shipping Details on Apply Changes Click
            CompanyEmployeeContactInfo objShipInfo = new CompanyEmployeeContactInfo();

            if (this.CompanyContactInfoID != 0)

                objShipInfo = objCmpEmpContRep.GetShippingDetailsByContactInfoID(this.CompanyContactInfoID);

            objShipInfo.CompanyName = TxtShippingCompany.Text;
            objShipInfo.Name = TxtShippingName.Text;
            objShipInfo.Title = TxtShipingTitle.Text;
            objShipInfo.CountryID = Common.GetLongNull(DrpShipingCoutry.SelectedValue, true);
            objShipInfo.StateID = Common.GetLongNull(DrpShipingState.SelectedValue, true);
            objShipInfo.CityID = Common.GetLongNull(DrpShippingCity.SelectedValue, true);
            objShipInfo.ZipCode = TxtShippingZip.Text;
            objShipInfo.Telephone = TxtShppingTelephone.Text;
            objShipInfo.Fax = TxtShipingFAX.Text;
            objShipInfo.Mobile = TxtShippingMobile.Text;
            objShipInfo.Email = TxtShippingEmail.Text;
            objShipInfo.Address = TxtShipingAddress.Text;
            objShipInfo.ContactInfoType = DAEnums.CompanyEmployeeContactInfo.Shipping.ToString();
            objShipInfo.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
            objShipInfo.OrderType = "MySetting";
            if (this.CompanyContactInfoID == 0) //insert
                objCmpEmpContRep.Insert(objShipInfo);

            objCmpEmpContRep.SubmitChanges();

            this.CompanyContactInfoID = objShipInfo.CompanyContactInfoID;
            
            bindgrid();
            // Shehzad End

            #endregion

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

    /// <summary>
    /// Binds the Shipping address Grid
    /// </summary>
    public void bindgrid()
    {
        List<CompanyEmployeeContactInfo> ObjShipingDetailList = objCmpEmpContRep.GetSavedShippingAddressesForUser(IncentexGlobal.CurrentMember.UserInfoID, this.SortExp, this.SortOrder);

        dtlShipping.DataSource = ObjShipingDetailList;
        dtlShipping.DataBind();
    }

    #endregion

    #region Miscellaneous functions

    /// <summary>
    /// Function which converts List to the DataTable
    /// </summary>
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null);
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    #endregion
}
