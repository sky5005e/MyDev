using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Web.Script.Services;
using System.Web.Services;

public partial class UserPages_MyAccountInfo : PageBase
{

    #region Data Members
    /// <summary>
    /// This will used to bind all company list and it will display in the txtCompany as autocomplete values
    /// </summary>
    static List<String> companyList
    {
        get { return (List<String>)HttpContext.Current.Session["companyList"]; }
        set { HttpContext.Current.Session["companyList"] = value; }
    }
    #endregion

    #region Page Event's
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MakeFieldReadOnly();
            BindDropDowns();
            DisplayData();
            // Set the CompanyList 
            companyList = new CompanyStoreRepository().GetCompanyStore().OrderBy(le => le.Company).Select(s => s.Company).ToList();
            SetActivetoLeftPanelNDiv(this.liBasicInfo,this.dvBasicInfo);
        }
    }
    protected void ddlMyAddressState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt64(ddlMyAddressState.SelectedValue) > 0)
            {
                acMyAddressCity.ContextKey = ddlMyAddressState.SelectedValue;
                if (sender != null)
                {
                    txtMyAddressCity.Focus();
                    SetActivetoLeftPanelNDiv(this.liMyAddressInfo,this.dvMyAddressInfo);
                }
            }
            else
            {
                acMyAddressCity.ContextKey = String.Empty;
                txtMyAddressCity.Text = String.Empty;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlNewState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt64(ddlNewState.SelectedValue) > 0)
            {
                acNewCity.ContextKey = ddlNewState.SelectedValue;
                if (sender != null)
                {
                    txtNewCity.Focus();
                    SetActivetoLeftPanelNDiv(this.liMyAddressInfo,this.dvMyAddressInfo);
                }
            }
            else
            {
                acNewCity.ContextKey = String.Empty;
                txtNewCity.Text = String.Empty;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void lnkNewAddressAdd_Click(object sender, EventArgs e)
    {
        SetActivetoLeftPanelNDiv(this.liMyAddressInfo, this.dvMyAddressInfo);
        try
        {
            if (Page.IsValid)
            {

                Int64 NewAddressCityID = 0;
                CityRepository objCityRep = new CityRepository();
                INC_City objCity = objCityRep.CheckIfExist(Convert.ToInt64(ddlNewAddressCountry.SelectedValue), Convert.ToInt64(ddlNewState.SelectedValue), txtNewCity.Text.Trim());
                if (objCity == null)
                {
                    INC_City objNewCity = new INC_City();
                    objNewCity.iCountryID = Convert.ToInt64(ddlNewAddressCountry.SelectedValue);
                    objNewCity.iStateID = Convert.ToInt64(ddlNewState.SelectedValue);
                    objNewCity.sCityName = txtNewCity.Text.Trim();
                    objCityRep.Insert(objNewCity);
                    objCityRep.SubmitChanges();
                    NewAddressCityID = objNewCity.iCityID;
                }
                else
                    NewAddressCityID = objCity.iCityID;

                CompanyEmployeeContactInfo objSaveShipping = new CompanyEmployeeContactInfo();
                CompanyContactInfoRepository objContactInfoRepo = new CompanyContactInfoRepository();
                objSaveShipping.Address = txtNewAddress.Text.Trim();
                objSaveShipping.Address2 = txtNewSuiteApt.Text.Trim();
                objSaveShipping.CityID = NewAddressCityID;
                objSaveShipping.BaseStationID = Convert.ToInt64(ddlNewBaseStation.SelectedValue);
                objSaveShipping.CompanyName = txtNewCompanyName.Text.Trim();
                objSaveShipping.ContactInfoType = Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString();
                objSaveShipping.CountryID = Convert.ToInt64(ddlNewAddressCountry.SelectedValue);
                objSaveShipping.county = ddlNewAddressCountry.SelectedItem.Text;
                objSaveShipping.Fax = txtNewLastName.Text.Trim();
                objSaveShipping.Name = txtNewFirstName.Text.Trim();
                objSaveShipping.OrderID = null;
                objSaveShipping.OrderType = "MySetting";
                objSaveShipping.StateID = Convert.ToInt64(ddlNewState.SelectedValue);
                objSaveShipping.Title = txtNewAddressSave.Text.Trim();
                objSaveShipping.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                objSaveShipping.ZipCode = txtNewZipCode.Text.Trim();

                objContactInfoRepo.Insert(objSaveShipping);
                objContactInfoRepo.SubmitChanges();

                FillAddressName();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkSaveNewMyAddress_Click(object sender, EventArgs e)
    {
        SetActivetoLeftPanelNDiv(this.liMyAddressInfo, this.dvMyAddressInfo);
        try
        {
            
            if (Convert.ToInt64(ddlMyAddressBookName.SelectedValue) > 0)
            {
                Int64 addressID = Convert.ToInt64(ddlMyAddressBookName.SelectedValue);
              
                Int64 MyAddressCityID = 0;
                CityRepository objCityRep = new CityRepository();
                INC_City objCity = objCityRep.CheckIfExist(Convert.ToInt64(ddlMyAddressCountry.SelectedValue), Convert.ToInt64(ddlMyAddressState.SelectedValue), txtMyAddressCity.Text.Trim());
                if (objCity == null)
                {
                    INC_City objNewCity = new INC_City();
                    objNewCity.iCountryID = Convert.ToInt64(ddlMyAddressCountry.SelectedValue);
                    objNewCity.iStateID = Convert.ToInt64(ddlMyAddressState.SelectedValue);
                    objNewCity.sCityName = txtMyAddressCity.Text.Trim();
                    objCityRep.Insert(objNewCity);
                    objCityRep.SubmitChanges();
                    MyAddressCityID = objNewCity.iCityID;
                }
                else
                    MyAddressCityID = objCity.iCityID;

                
                CompanyEmployeeContactInfoRepository objContactInfoRepo = new CompanyEmployeeContactInfoRepository();
                CompanyEmployeeContactInfo objSaveShipping = objContactInfoRepo.GetShippingDetailById(IncentexGlobal.CurrentMember.UserInfoID, addressID);
                
                objSaveShipping.Address = txtMyAddressAdress.Text.Trim();
                objSaveShipping.Address2 = txtMyAddressSuiteApt.Text.Trim();
                objSaveShipping.CityID = MyAddressCityID;
                objSaveShipping.CompanyName = txtMyAddressCompanyName.Text.Trim();
                objSaveShipping.ContactInfoType = Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString();
                objSaveShipping.CountryID = Convert.ToInt64(ddlMyAddressCountry.SelectedValue);
                objSaveShipping.county = ddlMyAddressCountry.SelectedItem.Text;
                objSaveShipping.Fax = txtMyAddressLastName.Text.Trim();
                objSaveShipping.Name = txtMyAddressFirstName.Text.Trim();
                objSaveShipping.OrderID = null;
                objSaveShipping.OrderType = "MySetting";
                objSaveShipping.StateID = Convert.ToInt64(ddlMyAddressState.SelectedValue);
                objSaveShipping.Title = txtMyAddressBook.Text.Trim();
                objSaveShipping.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                objSaveShipping.ZipCode = txtMyAddressZipCode.Text.Trim();
                objContactInfoRepo.SubmitChanges();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlMyAddressBookName_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetActivetoLeftPanelNDiv(this.liMyAddressInfo, this.dvMyAddressInfo);
        BindExistingAddress(Convert.ToInt64(ddlMyAddressBookName.SelectedValue));

       
    }

    protected void ddlMyAddressCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetActivetoLeftPanelNDiv(this.liMyAddressInfo, this.dvMyAddressInfo);
        BindMyAddressState(Convert.ToInt64(ddlMyAddressCountry.SelectedValue));

        if (ddlMyAddressCountry.SelectedItem.Text == "United States")
        {
            revMyAddressZipCode.ValidationExpression = @"^\d{5}([-\s]{0,3}\d{4})?$";
        }
        else if (ddlMyAddressCountry.SelectedItem.Text == "Canada")
            revMyAddressZipCode.ValidationExpression = @"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])[-\s]{0,3}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$";
        else
            revMyAddressZipCode.IsValid = true;
    }

    protected void ddlNewAddressCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetActivetoLeftPanelNDiv(this.liMyAddressInfo, this.dvMyAddressInfo);
        BindNewState(Convert.ToInt64(ddlNewAddressCountry.SelectedValue));

        if (ddlNewAddressCountry.SelectedItem.Text == "United States")
        {
            revNewZipCode.ValidationExpression = @"^\d{5}([-\s]{0,3}\d{4})?$";
        }
        else if (ddlNewAddressCountry.SelectedItem.Text == "Canada")
            revNewZipCode.ValidationExpression = @"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])[-\s]{0,3}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$";
        else
            revNewZipCode.IsValid = true;

    }

    protected void lnkSaveMainAccInfo_Click(object sender, EventArgs e)
    {
        SetActivetoLeftPanelNDiv(this.liBasicInfo, this.dvBasicInfo);
        try
        {
            UserInformationRepository objUsrInfoRep = new UserInformationRepository();
            //update User information
            UserInformation objUserInfo = objUsrInfoRep.GetById(IncentexGlobal.CurrentMember.UserInfoID);
            if (objUserInfo != null)
            {
                objUserInfo.FirstName = txtFirstName.Text.Trim();
                objUserInfo.LastName = txtLastName.Text.Trim();
                objUserInfo.Email = txtEmail.Text.Trim();
                objUsrInfoRep.SubmitChanges();
            }
            
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }

    protected void lnkDeleteAddress_Click(object sender, EventArgs e)
    {
        try
        {
            new CompanyEmployeeContactInfoRepository().DeleteAddressByID(Convert.ToInt64(ddlMyAddressBookName.SelectedValue));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void lnkSavePassword_Click(object sender, EventArgs e)
    {
        SetActivetoLeftPanelNDiv(this.liPasswordInfo, this.dvPasswordInfo);
        //VALIDATIONS 
        if (!string.IsNullOrEmpty(txtNewPassword.Text) && IncentexGlobal.CurrentMember.Password != txtOldPassword.Text.Trim())
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "GeneralAlertMsg('Please enter your correct Old Password !! ');", true);
            return;
        }
        else
        {
            try
            {
                #region User Information
                UserInformationRepository objUsrInfoRep = new UserInformationRepository();
                //update User information
                UserInformation objUserInfo = objUsrInfoRep.GetById(IncentexGlobal.CurrentMember.UserInfoID);
                if (!String.IsNullOrEmpty(txtNewPassword.Text) && !String.IsNullOrEmpty(txtNewConfirmPassword.Text))
                {
                    objUserInfo.Password = txtNewPassword.Text;
                    objUsrInfoRep.SubmitChanges();
                    IncentexGlobal.CurrentMember.Password = objUserInfo.Password;
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "GeneralAlertMsg('Password changed successfully !!');", true);

                }

                #endregion
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
        }
    }
    #endregion

    #region Page Method's
    [ScriptMethod()]
    [WebMethod]
    public static string[] GetCompanyList(String prefixText, Int32 count)
    {
        // Find All Matching company
        var list = from c in companyList
                   where c.ToLower().Contains(prefixText.ToLower())
                   select c;

        //Convert to Array as We need to return Array
        string[] prefixTextArray = list.ToArray<string>();

        //Return Selected company
        return prefixTextArray;
    }

    [ScriptMethod()]
    [WebMethod]
    public static string[] GetCityList(String prefixText, Int32 count, String contextKey)
    {

        // Get the Records Data Source. Change this method to use Database
        List<string> cityList = new CityRepository().GetByStateId(Convert.ToInt64(contextKey)).Where(le => le.sCityName.ToUpper().StartsWith(prefixText.ToUpper())).Select(le => le.sCityName).ToList<String>();

        //Convert to Array as We need to return Array
        String[] prefixTextArray = cityList.ToArray<string>();

        //Return Selected company
        return prefixTextArray;

    }

    private void BindExistingAddress(Int64 addressID)
    {
        try
        {
            CompanyEmployeeContactInfo objMyAddressInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailById(IncentexGlobal.CurrentMember.UserInfoID, addressID);

            if (objMyAddressInfo != null)
            {
                txtMyAddressCompanyName.Text = Convert.ToString(objMyAddressInfo.CompanyName);
                txtMyAddressAdress.Text = Convert.ToString(objMyAddressInfo.Address);
                txtMyAddressSuiteApt.Text = Convert.ToString(objMyAddressInfo.Address2);
                txtMyAddressFirstName.Text = Convert.ToString(objMyAddressInfo.Name);
                txtMyAddressLastName.Text = Convert.ToString(objMyAddressInfo.Fax);
                txtMyAddressSuiteApt.Text = Convert.ToString(objMyAddressInfo.Address2);
                if (objMyAddressInfo.BaseStationID != null)
                    ddlMyAddressBaseStation.SelectedValue = Convert.ToString(objMyAddressInfo.BaseStationID);
                ddlMyAddressCountry.SelectedValue = Convert.ToString(objMyAddressInfo.CountryID);

                ddlMyAddressCountry_SelectedIndexChanged(null, null);
                ddlMyAddressState.SelectedValue = Convert.ToString(objMyAddressInfo.StateID);
                ddlMyAddressState_SelectedIndexChanged(null, null);

                if (objMyAddressInfo.CityID != null)
                    txtMyAddressCity.Text = new CityRepository().GetById(Convert.ToInt64(objMyAddressInfo.CityID)).sCityName;

                txtMyAddressZipCode.Text = Convert.ToString(objMyAddressInfo.ZipCode);

                txtMyAddressBook.Text = Convert.ToString(objMyAddressInfo.Title);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void BindDropDowns()
    {
        CountryRepository objCountryRepo = new CountryRepository();
        List<INC_Country> objCountryList = objCountryRepo.GetAll();

        Common.BindDDL(ddlNewAddressCountry, objCountryList, "sCountryName", "iCountryID", "-select country-");
        ddlNewAddressCountry.SelectedValue = ddlNewAddressCountry.Items.FindByText("United States").Value;
        ddlNewAddressCountry_SelectedIndexChanged(null, null);

        Common.BindDDL(ddlMyAddressCountry, objCountryList, "sCountryName", "iCountryID", "-select country-");
        ddlMyAddressCountry.SelectedValue = ddlMyAddressCountry.Items.FindByText("United States").Value;
        ddlMyAddressCountry_SelectedIndexChanged(null, null);

        // For base satations
        List<INC_BasedStation> objbsList = new CompanyRepository().GetAllBaseStationResult().OrderBy(b => b.sBaseStation).ToList();
        Common.BindDDL(ddlMyAddressBaseStation, objbsList, "sBaseStation", "iBaseStationId", "-Select-");

        Common.BindDDL(ddlNewBaseStation, objbsList, "sBaseStation", "iBaseStationId", "-Select-");

        FillAddressName();
        BindNewState(Convert.ToInt64(ddlNewAddressCountry.SelectedValue));
        BindMyAddressState(Convert.ToInt64(ddlMyAddressCountry.SelectedValue));
        ddlNewState_SelectedIndexChanged(null, null);
        ddlMyAddressState_SelectedIndexChanged(null, null);
    }

    private void BindMyAddressState(Int64 countryID)
    {
        try
        {
            StateRepository objStateRepo = new StateRepository();
            ddlNewState.DataSource  = objStateRepo.GetByCountryId(countryID).OrderBy(le => le.sStatename).ToList();
            ddlNewState.DataTextField = "sStateName";
            ddlNewState.DataValueField = "iStateId";
            ddlNewState.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void BindNewState(Int64 countryID)
    {
        try
        {
            StateRepository objStateRepo = new StateRepository();
            List<INC_State> lstStates = objStateRepo.GetByCountryId(countryID).OrderBy(le => le.sStatename).ToList();
            ddlMyAddressState.DataSource = lstStates;
            ddlMyAddressState.DataTextField = "sStateName";
            ddlMyAddressState.DataValueField = "iStateId";
            ddlMyAddressState.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    private void FillAddressName()
    {
        try
        {
            CompanyEmployeeContactInfoRepository objCompEmpContactRepo = new CompanyEmployeeContactInfoRepository();
            List<CompanyEmployeeContactInfo> lstShippingAddresses = objCompEmpContactRepo.GetSavedShippingAddressesForUser(IncentexGlobal.CurrentMember.UserInfoID, CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Title, Incentex.DAL.Common.DAEnums.SortOrderType.Asc);
            ddlMyAddressBookName.DataSource = lstShippingAddresses;
            ddlMyAddressBookName.DataTextField = "Title";
            ddlMyAddressBookName.DataValueField = "CompanyContactInfoID";
            ddlMyAddressBookName.DataBind();
            ddlMyAddressBookName.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// Set Read Only
    /// </summary>
    private void MakeFieldReadOnly()
    {
        txtBaseStation.Attributes.Add("readonly", "readonly");
        txtCompany.Attributes.Add("readonly", "readonly");
        txtEmployeeID.Attributes.Add("readonly", "readonly");
        txtWorkGroup.Attributes.Add("readonly", "readonly");
    }
    /// <summary>
    /// Set Active to Left Panel
    /// </summary>
    /// <param name="liActive"></param>
    private void SetActivetoLeftPanelNDiv(HtmlControl _liActive, HtmlControl _dvActive)
    {
        liActive.Value = _liActive.ClientID;
        dvDisplay.Value = _dvActive.ClientID;
        /*
        this.liBasicInfo.Attributes.Remove("class");
        this.liMyAddressInfo.Attributes.Remove("class");
        this.liPasswordInfo.Attributes.Remove("class");
        this.dvBasicInfo.Attributes.Add("style", "display: none;");
        this.dvMyAddressInfo.Attributes.Add("style", "display: none;");
        this.dvPasswordInfo.Attributes.Add("style", "display: none;");

        _liActive.Attributes.Add("class", "active");
        _dvActive.Attributes.Add("style", "display: block;");
       */
    }

    /// <summary>
    /// Display Existing Record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DisplayData()
    {
        GetUserDetailsByUserInfoIDResult objUserInfo = new UserInformationRepository().GetUserDetailsByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);
        txtFirstName.Text = objUserInfo.FirstName;
        txtLastName.Text = objUserInfo.LastName;
        txtCompany.Text = objUserInfo.CompanyName;
        txtEmployeeID.Text = objUserInfo.EmployeeID;
        txtBaseStation.Text = objUserInfo.BaseStation;
        txtWorkGroup.Text = objUserInfo.WorkGroup;
        txtEmail.Text = objUserInfo.EmailAddress;
        txtFirstName.Focus();

    }


    #endregion

   
}
