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
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using System.Collections.Generic;

public partial class AssetManagement_BasicVendorEmpInformation : PageBase
{

    #region Properties
    Int64 NoteId
    {
        get
        {
            if (ViewState["NoteId"] == null)
            {
                ViewState["NoteId"] = 0;
            }
            return Convert.ToInt64(ViewState["NoteId"]);
        }
        set
        {
            ViewState["NoteId"] = value;
        }
    }
    Int64 VendorID
    {
        get
        {
            if (ViewState["VendorID"] == null)
            {
                ViewState["VendorID"] = 0;
            }
            return Convert.ToInt64(ViewState["VendorID"]);
        }
        set
        {
            ViewState["VendorID"] = value;
        }
    }
    Int64 VendorEmployeeID
    {
        get
        {
            if (ViewState["VendorEmployeeID"] == null)
            {
                ViewState["VendorEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["VendorEmployeeID"]);
        }
        set
        {
            ViewState["VendorEmployeeID"] = value;
        }
    }
    Int64 CompanyID
    {
        get
        {
            if (ViewState["CompanyID"] == null)
            {
                ViewState["CompanyID"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyID"]);
        }
        set
        {
            ViewState["CompanyID"] = value;
        }
    }
    Int64 UserInfoID
    {
        get
        {
            if (ViewState["UserInfoID"] == null)
            {
                ViewState["UserInfoID"] = 0;
            }
            return Convert.ToInt64(ViewState["UserInfoID"]);
        }
        set
        {
            ViewState["UserInfoID"] = value;
        }
    }
    Int64 iCityID
    {
        get
        {
            if (ViewState["iCityID"] == null)
            {
                ViewState["iCityID"] = 0;
            }
            return Convert.ToInt64(ViewState["iCityID"]);
        }
        set
        {
            ViewState["iCityID"] = value;
        }
    }
    
    #endregion
    CountryRepository obj = new CountryRepository();
    StateRepository objState = new StateRepository();
    CityRepository objCity = new CityRepository();
    SupplierRepository objSupplierRepos = new SupplierRepository();
    AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
    EquipmentVendorEmployee objEquipmentVendorEmployee = new EquipmentVendorEmployee();
    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || Incentex.DAL.Common.DAEnums.GetUserTypeFor(IncentexGlobal.CurrentMember.Usertype) == "GSEAssetManagement")
            {
                MasterPageFile = "~/admin/MasterPageAdmin.master";
            }
            else
            {
                MasterPageFile = "~/MasterPage.master";
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Employee";
            base.ParentMenuID = 50;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            Common.MenuModuleId = Incentex.DAL.Common.DAEnums.ManageID.EquipmentVendorEmployee;
            if (Request.QueryString.Count > 0)
            {
                this.VendorID = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.VendorEmployeeID = Convert.ToInt64(Request.QueryString.Get("SubId"));
                this.CompanyID = Convert.ToInt64(Request.QueryString.Get("CompanyID"));

                ((Label)Master.FindControl("lblPageHeading")).Text = "Vendor Employee Basic Information";
                


                // menuControl.PopulateMenu(3, 0, this.SupplierID, this.SupplierEmployeeID, true);
                if (IncentexGlobal.GSEMgtCurrentMember != null && (IncentexGlobal.GSEMgtCurrentMember.CurrentUserType == (long)Incentex.DAL.Common.DAEnums.UserTypes.CustomerEmployee || IncentexGlobal.GSEMgtCurrentMember.CurrentUserType == (long)Incentex.DAL.Common.DAEnums.UserTypes.VendorEmployee || IncentexGlobal.GSEMgtCurrentMember.VenoderEmployeeID == this.VendorEmployeeID))
                {
                    menuControl.Visible = false;
                    dvContact.Visible = false;
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx" ;
                }
                else
                {
                    if (this.VendorEmployeeID == 0)
                    {
                        ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/VendorList.aspx?Id=" + this.VendorID + "&SubId=" + this.VendorID; ;
                    }
                    else
                    {
                        ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/EmployeeList.aspx?Id=" + this.VendorID + "&SubId=" + this.VendorID; ;
                    }
                    menuControl.PopulateMenu(0, 0, this.VendorEmployeeID, this.VendorID, false);
                }

            }
            else
            {
                Response.Redirect("EmployeeList.aspx");
            }
            //BindUserType();
            BindDropDowns();
            BindDDl();
            BindBaseStation();
            DisplayData();

        }


    }

    public void BindDropDowns()
    {
        LookupRepository objLookRep = new LookupRepository();
        ddlStatus.DataSource = objLookRep.GetByLookup("status");
        ddlStatus.DataValueField = "iLookupID";
        ddlStatus.DataTextField = "sLookupName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-Select-", "0"));
    }
    /// <summary>
    /// Bind dropdown list
    /// </summary>
    void BindDDl()
    {
        FillCompCountry();
        //bind country dropdown
        Common.BindCountry(ddlCountry);

        //get Base Stations
        CompanyRepository objRepo = new CompanyRepository();
        List<INC_BasedStation> objBaseStationList = new List<INC_BasedStation>();
        //objBaseStationList = objRepo.GetAllBaseStationResult();
        //objBaseStationList = objBaseStationList.OrderBy(p => p.sBaseStation).ToList();
        //Common.BindDDL(ddlBaseStation, objBaseStationList, "sBaseStation", "iBaseStationId", "-Select All BaseSation-");
    }


    /// <summary>
    /// Display data to edit
    /// </summary>
    void DisplayData()
    {
        bool IsCustomer = false;
        UserInformation objUserInfo = new UserInformation();
        if (this.VendorID != 0)
        {
            EquipmentVendorMaster objEquipmentVendorMaster = objAssetVendorRepository.GetEquipmentVendorById(this.VendorID);
            if (objEquipmentVendorMaster != null)
            {
                IsCustomer = objEquipmentVendorMaster.IsCustomer;
            }
            txtCompany.Text = objEquipmentVendorMaster.EquipmentVendorName;
        }

        if (this.VendorEmployeeID != 0)
        {
            //SupplierEmployeeRepository objRepo = new SupplierEmployeeRepository();
            //SupplierEmployee obj = objRepo.GetById(this.SupplierEmployeeID);

            objEquipmentVendorEmployee = objAssetVendorRepository.GetVendorEmpById(this.VendorEmployeeID);

            if (objEquipmentVendorEmployee != null)
            {

                //data from userinformation table
                this.UserInfoID = Convert.ToInt64(objEquipmentVendorEmployee.UserInfoID);
                UserInformationRepository objUserInfoRepo = new UserInformationRepository();
                objUserInfo = objUserInfoRepo.GetById(this.UserInfoID);

                txtFirstName.Text = objUserInfo.FirstName;
                txtLastName.Text = objUserInfo.LastName;
                //txtTitle.Text = objUserInfo.Title;
                txtAddress.Text = objUserInfo.Address1;
                ddlCountry.SelectedValue = objUserInfo.CountryId.ToString();
                Common.BindState(ddlState, (objUserInfo.CountryId.Value == null ? -1 : objUserInfo.CountryId.Value));
                ddlState.SelectedValue = objUserInfo.StateId.ToString();
                Common.BindCity(ddlCity, (objUserInfo.StateId.Value == null ? -1 : objUserInfo.StateId.Value));
                ddlCity.SelectedValue = objUserInfo.CityId.ToString();
                ddlCity.Items.Insert(1, "-Other-");
                txtZip.Text = objUserInfo.ZipCode;
                txtTelephone.Text = objUserInfo.Telephone;
                //txtExtension.Text = objUserInfo.Extension;
                txtFax.Text = objUserInfo.Fax;
                txtMobile.Text = objUserInfo.Mobile;
                //txtEmail.Text = objUserInfo.Email;
                //txtSkypeName.Text = objUserInfo.SkypeName;
                txtLogInEmail.Text = objUserInfo.LoginEmail;
                txtPassword.Text = objUserInfo.Password;
                ddlUserRole.SelectedValue = Convert.ToString(objUserInfo.Usertype);
                if (objUserInfo.WLSStatusId != null)
                {
                    ddlStatus.SelectedValue = objUserInfo.WLSStatusId.ToString();
                }
                if (objEquipmentVendorEmployee.BaseStationID != null)
                {
                    CheckBox chk;
                    string lblId;
                    //Show Base Station


                    foreach (DataListItem dtM in dtBaseStation.Items)
                    {
                        chk = dtM.FindControl("chkBaseStation") as CheckBox;
                        lblId = objEquipmentVendorEmployee.BaseStationID;
                        HtmlGenericControl dvChk = dtM.FindControl("BaseStationspan") as HtmlGenericControl;
                        string[] BID = lblId.Split(',');
                        foreach (string i in BID)
                        {
                            if (((HiddenField)dtM.FindControl("hdnBaseStation")).Value.Equals(i))
                            {
                                chk.Checked = true;
                                dvChk.Attributes.Add("class", "custom-checkbox_checked");
                                break;
                            }
                        }
                    }
                }

            }
        }
        BindUserType(Convert.ToString(objUserInfo.Usertype), IsCustomer);
        
    }
    /// <summary>
    /// Bind Country
    /// Amit 20-Sep-2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindState(ddlState, Convert.ToInt64(ddlCountry.SelectedValue));
        ddlState_SelectedIndexChanged(sender, e);
        if (ddlCountry.SelectedValue == "0" && ddlState.SelectedValue == "0")
        {
            ddlCity.Items.Remove(new ListItem("-Other-", "-Other-"));

        }

    }
    /// <summary>
    /// Bind City
    /// Amit 20-Sep-2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindCity(ddlCity, Convert.ToInt64(ddlState.SelectedValue));
        if (ddlState.SelectedIndex > 0)
        {

            ddlCity.Items.Insert(1, "-Other-");
        }

    }
    /// <summary>
    /// Add \ Update record
    /// Amit 20-Sep-2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanAdd && !base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            lblMsg.Text = "";


            //Start Add City when Other Selection form city dropdownlist"
            if (PnlCityOther.Visible == true)
            {
                CityRepository objCityRep = new CityRepository();
                INC_City objCity = new INC_City();
                objCity.iCountryID = Convert.ToInt64(ddlCountry.SelectedItem.Value);
                objCity.iStateID = Convert.ToInt64(ddlState.SelectedItem.Value);
                if (this.iCityID != 0)
                {
                    objCity = objCityRep.GetById(this.iCityID);
                }
                objCity.sCityName = txtCity.Text;
                objCityRep.Insert(objCity);
                objCityRep.SubmitChanges();
                this.iCityID = Convert.ToInt32(objCity.iCityID);
                Session["this.iCityID"] = this.iCityID;
                txtCity.Text = string.Empty;
                PnlCityOther.Visible = false;
            }
            //End

            //save in User info
            UserInformationRepository objUserInfoRepo = new UserInformationRepository();

            if (objUserInfoRepo.CheckUniqueEmail(this.UserInfoID, txtLogInEmail.Text))
            {
                lblMsg.Text = "Login Email already exist ...";
                return;
            }

            UserInformation objUserInfo = new UserInformation();

            if (this.UserInfoID != 0)
            {
                objUserInfo = objUserInfoRepo.GetById(this.UserInfoID);
            }
            objUserInfo.FirstName = txtFirstName.Text;
            objUserInfo.LastName = txtLastName.Text;
            // objUserInfo.Title = txtTitle.Text;
            objUserInfo.Address1 = txtAddress.Text;
            objUserInfo.CountryId = Convert.ToInt64(ddlCountry.SelectedValue);
            objUserInfo.StateId = Convert.ToInt64(ddlState.SelectedValue);
            if (ddlCity.SelectedValue == "-Other-")
            {
                objUserInfo.CityId = Convert.ToInt64(Session["this.iCityID"]);
            }
            else
            {
                objUserInfo.CityId = Int64.Parse(ddlCity.SelectedItem.Value);
            }
            objUserInfo.ZipCode = txtZip.Text;
            objUserInfo.Telephone = txtTelephone.Text;
            //objUserInfo.Extension = txtExtension.Text;
            objUserInfo.Fax = txtFax.Text;
            objUserInfo.Mobile = txtMobile.Text;
            objUserInfo.Email = "knelson@incentex.com";
            //-----CompanyID
            EquipmentVendorMaster objEquipmentVendorMaster = new EquipmentVendorMaster();
            objEquipmentVendorMaster = objAssetVendorRepository.GetEquipmentVendorById(this.VendorID);
            objUserInfo.CompanyId = objEquipmentVendorMaster.CompanyID;
            //---------
            objUserInfo.Usertype = Convert.ToInt64(ddlUserRole.SelectedValue);
            objUserInfo.LoginEmail = txtLogInEmail.Text;
            objUserInfo.Password = txtPassword.Text;
            objUserInfo.WLSStatusId = Convert.ToInt64(ddlStatus.SelectedValue);

            if (this.UserInfoID == 0)
            {
                objUserInfo.CreatedDate = DateTime.Now;
                objUserInfo.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objUserInfoRepo.Insert(objUserInfo);
            }
            else
            {
                objUserInfo.UpdatedDate = DateTime.Now;
                objUserInfo.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            }
            objUserInfoRepo.SubmitChanges();
            this.UserInfoID = objUserInfo.UserInfoID;
            Session["this.iCityID"] = null;
            //save in Vendor Employee
            if (IncentexGlobal.GSEMgtCurrentMember == null || (IncentexGlobal.GSEMgtCurrentMember != null && IncentexGlobal.GSEMgtCurrentMember.CurrentUserType != (long)Incentex.DAL.Common.DAEnums.UserTypes.CustomerEmployee && IncentexGlobal.GSEMgtCurrentMember.CurrentUserType != (long)Incentex.DAL.Common.DAEnums.UserTypes.VendorEmployee && IncentexGlobal.GSEMgtCurrentMember.VenoderEmployeeID != this.VendorEmployeeID))
            {

                if (this.VendorEmployeeID != 0)
                {
                    objEquipmentVendorEmployee = objAssetVendorRepository.GetVendorEmpById(this.VendorEmployeeID);
                }
                objEquipmentVendorEmployee.VendorID = this.VendorID;
                objEquipmentVendorEmployee.UserInfoID = this.UserInfoID;
                //Base Station---------
                string BSID = string.Empty;

                foreach (DataListItem dt in dtBaseStation.Items)
                {
                    if (((CheckBox)dt.FindControl("chkBaseStation")).Checked == true)
                    {
                        if (BSID == string.Empty)
                        {
                            BSID = ((HiddenField)dt.FindControl("hdnBaseStation")).Value;
                        }
                        else
                        {
                            BSID = BSID + "," + ((HiddenField)dt.FindControl("hdnBaseStation")).Value;
                        }
                    }
                }
                objEquipmentVendorEmployee.BaseStationID = BSID;
                //---------------------

                if (this.VendorEmployeeID == 0)
                {
                    objAssetVendorRepository.Insert(objEquipmentVendorEmployee);
                }

                objAssetVendorRepository.SubmitChanges();
                this.VendorEmployeeID = objEquipmentVendorEmployee.VendorEmployeeID;
                Response.Redirect("ManageEmail.aspx?SubId=" + this.VendorEmployeeID + "&Id=" + this.VendorID);
            }
            lblMsg.Text = "Record Saved Sucessfully ...";
            //Response.Redirect("Documents.aspx?Id=" + this.SupplierEmployeeID);
            
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in saving record ...";
            ErrHandler.WriteError(ex);
        }
    }
    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCity.SelectedValue == "-Other-")
        {
            PnlCityOther.Visible = true;

        }
        else
        {
            PnlCityOther.Visible = false;
        }
    }

    public void FillCompCountry()
    {
        try
        {
            //Company
            ddlCountry.DataSource = obj.GetAll();
            ddlCountry.DataTextField = "sCountryName";
            ddlCountry.DataValueField = "iCountryID";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("-select country-", "0"));
            ddlCountry.SelectedValue = ddlCountry.Items.FindByText("United States").Value;


            ddlState.DataSource = objState.GetByCountryId(Convert.ToInt32(ddlCountry.SelectedItem.Value));
            ddlState.DataValueField = "iStateID";
            ddlState.DataTextField = "sStateName";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("-select state-", "0"));

            ddlCity.Items.Clear();
            ddlCity.Items.Add(new ListItem("-select city-", "0"));




        }
        catch (System.Threading.ThreadAbortException lException)
        {

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }

    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        try
        {


            var dt = new DataTable();
            foreach (var info in typeof(T).GetProperties())
            {
                //table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                dt.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);
            }
            foreach (var t in list)
            {
                var row = dt.NewRow();
                foreach (var info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        catch (Exception)
        {

            throw;
        }
    }



    /// <summary>
    /// Added by Prashant April 2013 to bind UserRole based on the Login User
    /// </summary>
    public void BindUserType(string SelectedValue, bool IsCustomer)
    {
        ddlUserRole.ClearSelection();
        ddlUserRole.Items.Clear();

        ddlUserRole.Items.Add("--Select--");
        ddlUserRole.Items[0].Value = "0";

        AssetMgtRepository objAssestMgtRepo = new AssetMgtRepository();
        var lstUserTypes = objAssestMgtRepo.GetGSEAssetManagemenUserType();
        bool IsSuperAdminDefine = objAssestMgtRepo.IsSuperAdminDefined(this.VendorID, IsCustomer);

        foreach (var item in lstUserTypes)
        {
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
            {
                //Bind all the UserTypes for IE,CA(Company Admin) or CE(Company Employee), and Super Admin from our Main System
                //Bind Roles for IE,CA,CE and Super Admin

                if (IsCustomer)
                {
                    if (!IsSuperAdminDefine && (item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CustomerSuperAdmin) || item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CustomerAdmin) || item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CustomerEmployee) || item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.FederalAviationAssociation)))
                    {
                        ddlUserRole.Items.Add(item.UserType1);
                        ddlUserRole.Items[ddlUserRole.Items.Count - 1].Value = item.UserTypeID.ToString();
                    }
                    else if (IsSuperAdminDefine && item.UserTypeID != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CustomerSuperAdmin) && (item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CustomerAdmin) || item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CustomerEmployee) || item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.FederalAviationAssociation)))
                    {
                        ddlUserRole.Items.Add(item.UserType1);
                        ddlUserRole.Items[ddlUserRole.Items.Count - 1].Value = item.UserTypeID.ToString();
                    }
                }
                else
                {
                    if (!IsSuperAdminDefine && (item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorSuperAdmin) || item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorAdmin) || item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorEmployee)))
                    {
                        ddlUserRole.Items.Add(item.UserType1);
                        ddlUserRole.Items[ddlUserRole.Items.Count - 1].Value = item.UserTypeID.ToString();
                    }
                    else if (IsSuperAdminDefine && item.UserTypeID != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorSuperAdmin) && (item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorAdmin) || item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorEmployee)))
                    {
                        ddlUserRole.Items.Add(item.UserType1);
                        ddlUserRole.Items[ddlUserRole.Items.Count - 1].Value = item.UserTypeID.ToString();
                    }
                }
            }
            else if (IncentexGlobal.GSEMgtCurrentMember.CurrentUserType == item.UserTypeID && SelectedValue != "0")
            {
                //Bind Role of the Logged In User when Editing
                ddlUserRole.Items.Add(item.UserType1);
                ddlUserRole.Items[ddlUserRole.Items.Count - 1].Value = item.UserTypeID.ToString();
            }
            else if (item.UserTypeID != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CustomerSuperAdmin) && item.UserTypeID != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorSuperAdmin))
            {

                if (IncentexGlobal.GSEMgtCurrentMember.CurrentUserType == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CustomerSuperAdmin))
                {
                    //Bind all the UserTypes except CSA and VSA
                    //Bind Roles for CSA
                    if (IsCustomer && item.UserTypeID != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorAdmin) && item.UserTypeID != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorEmployee))
                    {
                        ddlUserRole.Items.Add(item.UserType1);
                        ddlUserRole.Items[ddlUserRole.Items.Count - 1].Value = item.UserTypeID.ToString();
                    }
                    else if (!IsCustomer && item.UserTypeID != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CustomerAdmin) && item.UserTypeID != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CustomerEmployee))
                    {
                        ddlUserRole.Items.Add(item.UserType1);
                        ddlUserRole.Items[ddlUserRole.Items.Count - 1].Value = item.UserTypeID.ToString();
                    }
                }
                else if (item.UserTypeID != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.FederalAviationAssociation))
                {
                    if (IncentexGlobal.GSEMgtCurrentMember.CurrentUserType == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CustomerAdmin) && item.UserTypeID != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CustomerAdmin))
                    {
                        //Bind Roles for CA
                        if (IsCustomer && item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CustomerEmployee))
                        {
                            ddlUserRole.Items.Add(item.UserType1);
                            ddlUserRole.Items[ddlUserRole.Items.Count - 1].Value = item.UserTypeID.ToString();
                        }
                        else if (!IsCustomer && (item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorAdmin) || item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorEmployee)))
                        {
                            ddlUserRole.Items.Add(item.UserType1);
                            ddlUserRole.Items[ddlUserRole.Items.Count - 1].Value = item.UserTypeID.ToString();
                        }
                    }
                    else if (IncentexGlobal.GSEMgtCurrentMember.CurrentUserType == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorSuperAdmin) && (item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorAdmin) || item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorEmployee)))
                    {
                        //Bind Roles for VSA
                        ddlUserRole.Items.Add(item.UserType1);
                        ddlUserRole.Items[ddlUserRole.Items.Count - 1].Value = item.UserTypeID.ToString();
                    }
                    else if (IncentexGlobal.GSEMgtCurrentMember.CurrentUserType == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorAdmin) && item.UserTypeID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.VendorEmployee))
                    {
                        //Bind Roles for VA
                        ddlUserRole.Items.Add(item.UserType1);
                        ddlUserRole.Items[ddlUserRole.Items.Count - 1].Value = item.UserTypeID.ToString();
                    }
                }
            }
        }
        ddlUserRole.SelectedValue = SelectedValue;
    }

   
    /// <summary>
    /// added by Prashant April 2013 to bind BaseStation associated to the Login User
    /// </summary>
    public void BindBaseStation()
    {
        List<INC_BasedStation> objBaseStationList = new List<INC_BasedStation>();
        BaseStationRepository BaseStationRepos = new BaseStationRepository();
        if (IncentexGlobal.GSEMgtCurrentMember != null)
            objBaseStationList = BaseStationRepos.GetAssociateBaseStation(IncentexGlobal.GSEMgtCurrentMember.BaseStationIds).ToList();
        else
            objBaseStationList = BaseStationRepos.GetAssociateBaseStation("").ToList();
        if (objBaseStationList.Count > 0)
        {
            lblNoBaseSation.Visible = false;
            dtBaseStation.Visible = true;
            dtBaseStation.DataSource = objBaseStationList;
            dtBaseStation.DataBind();
        }
        else
        {
            lblNoBaseSation.Visible = true;
            dtBaseStation.Visible = false;
        }
    }

}

