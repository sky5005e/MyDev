using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Data;
using System.Text;


public partial class admin_CompanyStore_ShippingAddressSetting : PageBase
{

    #region Page Properties

    Int64 StoreID
    {
        get
        {
            return Convert.ToInt64(ViewState["StoreID"]);
        }
        set
        {
            ViewState["StoreID"] = value;
        }
    }

    Int64 WorkGroupID
    {
        get
        {
            return Convert.ToInt64(ViewState["WorkGroupID"]);
        }
        set
        {
            ViewState["WorkGroupID"] = value;
        }
    }

    Int64 ShippingAddressID
    {
        get
        {
            return Convert.ToInt64(ViewState["ShippingAddressID"]);
        }
        set
        {
            ViewState["ShippingAddressID"] = value;
        }
    }
    Int64 BaseStationID
    {
        get
        {
            return Convert.ToInt64(ViewState["BaseStationID"]);
        }
        set
        {
            ViewState["BaseStationID"] = value;
        }
    }
    Int64 PaymentOptionTypeID
    {
        get
        {
            return Convert.ToInt64(ViewState["PaymentOptionTypeID"]);
        }
        set
        {
            ViewState["PaymentOptionTypeID"] = value;
        }
    }

    GlobalShippingAddressRepository objShippingRepo = new GlobalShippingAddressRepository();
    LookupRepository objLookupRepo = new LookupRepository();

    PagedDataSource pds = new PagedDataSource();

    public int CurrentPage
    {
        get
        {
            return Convert.ToInt16(this.ViewState["CurrentPage"]);
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }

    public Int32 PagerSize
    {
        get
        {
            if (this.ViewState["PagerSize"] == null)
                return Convert.ToInt32(Application["GRIDPAGERSIZE"]);
            else
                return Convert.ToInt16(this.ViewState["PagerSize"]);
        }
        set
        {
            this.ViewState["PagerSize"] = value;
        }
    }

    public int FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt16(this.ViewState["FrmPg"]);
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }

    public int ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return PagerSize;
            else
                return Convert.ToInt16(this.ViewState["ToPg"]);
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }


    #endregion


    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Store Management Console";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            if (Request.QueryString.Count > 0)
            {
                this.StoreID = Convert.ToInt64(Request.QueryString.Get("Id"));

                if (this.StoreID == 0)
                {
                    Response.Redirect("~/admin/CompanyStore/GeneralStoreSetup.aspx?Id=" + this.StoreID);
                }

                if (this.StoreID > 0 && !base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                this.WorkGroupID = Convert.ToInt64(Request.QueryString.Get("SubId"));

                ((Label)Master.FindControl("lblPageHeading")).Text = "Storefront Settings by store workgroup";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/ViewGlobalMenuSetting.aspx?ID=" + this.StoreID;

                Session["ManageID"] = 5;
                menuControl.PopulateMenu(11, 6, this.StoreID, this.WorkGroupID, true);

                BindDDL();
                lblStore.Text = new CompanyRepository().GetById(new CompanyStoreRepository().GetById(StoreID).CompanyID).CompanyName;
                lblWorkGroup.Text = objLookupRepo.GetById(this.WorkGroupID).sLookupName.ToString();

                BindGridView();
                //modalNewShippingAddress.Show();
            }
        }
    }

    #endregion

    #region Control Events

    protected void DrpShippingCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(DrpShippingCountry.SelectedValue));

        DrpShippingState.Items.Clear();
        DrpShippingCity.Items.Clear();
        DrpShippingCity.Items.Insert(0, new ListItem("-select city-", "0"));
        Common.BindDDL(DrpShippingState, objStateList, "sStatename", "iStateID", "-select state-");
    }

    protected void DrpShippingState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(DrpShippingState.SelectedValue));
        DrpShippingCity.Items.Clear();
        Common.BindDDL(DrpShippingCity, objCityList, "sCityName", "iCityID", "-select city-");
    }


    protected void gvShippingAddress_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ResetControl();
        if (e.CommandName.Equals("Sort"))
        {
            if (this.ViewState["SortExp"] == null)
            {
                this.ViewState["SortExp"] = e.CommandArgument.ToString();
                this.ViewState["SortOrder"] = "ASC";
            }
            else
            {
                if (this.ViewState["SortExp"].ToString() == e.CommandArgument.ToString())
                {
                    if (this.ViewState["SortOrder"].ToString() == "ASC")
                        this.ViewState["SortOrder"] = "DESC";
                    else
                        this.ViewState["SortOrder"] = "ASC";

                }
                else
                {
                    this.ViewState["SortOrder"] = "ASC";
                    this.ViewState["SortExp"] = e.CommandArgument.ToString();
                }
            }

            BindGridView();
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ScrollToTag('orderreturn_box',true);", true);
        }
        else if (e.CommandName == "Modify")
        {
            this.ShippingAddressID = Convert.ToInt64(e.CommandArgument.ToString());
            DisplayData(this.ShippingAddressID);
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ScrollToTag('dvfixShipAdressfields',false);", true);
        }
        else if (e.CommandName == "ShipDelete")
        {
            GlobalShippingAddressRepository objGlobalShippingAddressRepository = new GlobalShippingAddressRepository();
            objGlobalShippingAddressRepository.DeleteShippingAddress(Convert.ToInt64(e.CommandArgument.ToString()), IncentexGlobal.CurrentMember.UserInfoID);
            lblMsg.Text = "Record deleted successfully.";
            dvMsg.Visible = true;
            BindGridView();
        }
    }



    protected void lnkReset_Click(object sender, EventArgs e)
    {
        ResetControl();
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder sbMessage = new StringBuilder();

            if (String.IsNullOrEmpty(txtAddress.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter address.");

            if (String.IsNullOrEmpty(DrpShippingCountry.SelectedValue))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please select country.");

            if (String.IsNullOrEmpty(DrpShippingState.SelectedValue))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please select state.");

            if (String.IsNullOrEmpty(DrpShippingCity.SelectedValue))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please select city.");

            if (String.IsNullOrEmpty(ddlStatus.SelectedValue))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please select status.");

            if (String.IsNullOrEmpty(txtZip.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter zip code.");
            else if (!Common.IsValidZipCode(DrpShippingCountry.SelectedItem.Text.Trim(), txtZip.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter valid zip code.");

            if (!String.IsNullOrEmpty(txtEmail.Text.Trim()) && !Common.IsValidEmailAddress(txtEmail.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter valid email.");

            if (sbMessage.Length > 0)
            {
                dvMsg.Visible = true;
                lblMsg.Text = sbMessage.ToString();
                return;
            }
            else
                dvMsg.Visible = false;

            string strPaymentOptionName = String.Empty;
            foreach (ListItem chkPaymentOption in chklstPaymentOptions.Items)
            {
                if (chkPaymentOption.Selected)
                {
                    if (strPaymentOptionName != "")
                        strPaymentOptionName += ",";
                    strPaymentOptionName += chkPaymentOption.Value;
                }
            }

            dvMsg.Visible = true;
            if (strPaymentOptionName == "")
            {
                lblMsg.Text = "Please select atleast one payment option...";
                return;
            }
            else
            {
                GlobalShippingAddressRepository objGlobalShippingAddressRepository = new GlobalShippingAddressRepository();
                string strInsertUpdateStatus = objGlobalShippingAddressRepository.InsertGlobalShippingAddressDetails(this.ShippingAddressID, this.StoreID, this.WorkGroupID, Convert.ToInt64(ddlBaseStation.SelectedValue), txtAddress.Text, Convert.ToInt64(DrpShippingCity.SelectedValue), Convert.ToInt64(DrpShippingState.SelectedValue), Convert.ToInt64(DrpShippingCountry.SelectedValue), txtCompany.Text.Trim(), txtEmail.Text.Trim(), txtMobile.Text.Trim(), txtTelephone.Text.Trim(), txtZip.Text.Trim(), ddlApplyToIssuancePolicy.SelectedValue, Convert.ToInt64(ddlStatus.SelectedValue), IncentexGlobal.CurrentMember.UserInfoID, strPaymentOptionName);

                if (strInsertUpdateStatus.ToUpper() == "SUCCESS")
                {
                    lblMsg.Text = "Record saved successfully...";
                    ResetControl();
                    BindGridView();
                    ddlStatus.SelectedIndex = 1;
                }
                else
                {
                    lblMsg.Text = "Address already exists.";
                }
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Some error occured while saving the record...";
            dvMsg.Visible = true;
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Methods

    private void ResetControl()
    {
        this.ShippingAddressID = 0;
        ddlBaseStation.SelectedIndex = 0;
        chklstPaymentOptions.ClearSelection();
        DrpShippingCity.SelectedIndex = 0;
        DrpShippingCountry.SelectedIndex = 0;
        DrpShippingState.SelectedIndex = 0;
        txtCompany.Text = String.Empty;
        txtAddress.Text = String.Empty;
        txtZip.Text = String.Empty;
        txtTelephone.Text = String.Empty;
        txtEmail.Text = String.Empty;
        txtMobile.Text = String.Empty;
        ddlApplyToIssuancePolicy.SelectedIndex = 0;
    }

    private void BindDDL()
    {
        CountryRepository objCountryShippingRepo = new CountryRepository();
        List<INC_Country> objShippingCountryList = objCountryShippingRepo.GetAll();
        Common.BindDDL(DrpShippingCountry, objShippingCountryList, "sCountryName", "iCountryID", "-select country-");
        DrpShippingCountry.SelectedValue = DrpShippingCountry.Items.FindByText("United States").Value;
        DrpShippingCountry_SelectedIndexChanged(null, null);

        // For Base stations
        List<INC_BasedStation> objbsList = new List<INC_BasedStation>();
        CompanyRepository objRepo = new CompanyRepository();
        objbsList = objRepo.GetAllBaseStationResult().OrderBy(b => b.sBaseStation).ToList();
        Common.BindDDL(ddlBaseStation, objbsList, "sBaseStation", "iBaseStationId", "-Select-");

        //For chklstPaymentOption
        chklstPaymentOptions.DataSource = objLookupRepo.GetByLookup("WLS Payment Option");
        chklstPaymentOptions.DataValueField = "iLookupID";
        chklstPaymentOptions.DataTextField = "sLookupName";
        chklstPaymentOptions.DataBind();
        //ddlPaymentOption.Items.Insert(0, new ListItem("-Select-", "0"));


        //Bind the Status Dropdown
        LookupRepository objLookupRepos = new LookupRepository();
        ddlStatus.DataSource = objLookupRepos.GetByLookup("Status");
        ddlStatus.DataValueField = "iLookupID";
        ddlStatus.DataTextField = "sLookupName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-select status-", "0"));

        ddlStatus.SelectedIndex = 1;

        PreferenceRepository objPreferenceRepos = new PreferenceRepository();
        List<PreferenceValue> lstPreference = objPreferenceRepos.GetByPreferenceKey("FixShippingAddress");
        ddlApplyToIssuancePolicy.DataSource = lstPreference;
        ddlApplyToIssuancePolicy.DataValueField = "Value";
        ddlApplyToIssuancePolicy.DataTextField = "Display";
        ddlApplyToIssuancePolicy.DataBind();
        ddlApplyToIssuancePolicy.Items.Insert(0, new ListItem("-select-", ""));
        ddlApplyToIssuancePolicy.SelectedValue = "";
    }

    private void BindGridView()
    {
        List<GetShippingAddressByStoreIDWorkGroupIDResult> objList = objShippingRepo.GetShippingAddressDetails(this.StoreID, this.WorkGroupID).ToList();
        DataTable dataTable = Common.ListToDataTable(objList);
        DataView myDataView = dataTable.DefaultView;
        if (objList.Count == 0)
        {
            CurrentPage = 0;
            dvPager.Visible = false;
        }
        else
            dvPager.Visible = true;

        if (this.ViewState["SortExp"] != null)
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();

        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;

        gvShippingAddress.DataSource = pds;
        gvShippingAddress.DataBind();

        doPaging();
    }

    private void DisplayData(Int64 ID)
    {
        try
        {
            if (ID > 0)
            {
                GetShippingAddressByStoreIDWorkGroupIDResult objAddress = objShippingRepo.GetByID(this.StoreID,this.WorkGroupID,ID);
                if (objAddress != null)
                {
                    ddlBaseStation.SelectedValue = Convert.ToString(objAddress.BaseStationID);
                    ddlStatus.SelectedValue = Convert.ToString(objAddress.StatusID);
                    ddlApplyToIssuancePolicy.SelectedValue = Convert.ToString(objAddress.IssuancePolicyTypeToApply);

                    DrpShippingCountry.SelectedValue = objAddress.CountryID != null ? Convert.ToString(objAddress.CountryID) : "0";
                    DrpShippingCountry_SelectedIndexChanged(null, null);

                    if (objAddress.CountryID != null)
                    {
                        DrpShippingState.SelectedValue = objAddress.StateID != null ? Convert.ToString(objAddress.StateID) : "0";
                        DrpShippingState_SelectedIndexChanged(null, null);

                        if (objAddress.StateID != null)
                        {
                            DrpShippingCity.SelectedValue = objAddress.CityID != null ? Convert.ToString(objAddress.CityID) : "0";
                        }
                    }

                    txtAddress.Text = Convert.ToString(objAddress.ShippingAddress).Trim();
                    txtCompany.Text = Convert.ToString(objAddress.CompanyName).Trim();
                    txtEmail.Text = Convert.ToString(objAddress.Email).Trim();
                    txtMobile.Text = Convert.ToString(objAddress.Mobile).Trim();
                    txtTelephone.Text = Convert.ToString(objAddress.Telephone).Trim();
                    txtZip.Text = Convert.ToString(objAddress.ZipCode).Trim();

                    foreach (ListItem chkPaymentOption in chklstPaymentOptions.Items)
                    {
                        string[] strPaymentOptionIds = objAddress.PaymentOption.Split(',');
                        foreach (string item in strPaymentOptionIds )
                        {
                            if (chkPaymentOption.Value == item)
                                chkPaymentOption.Selected = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Functions for Paging
    protected void dtlAddresses_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
    protected void dtlAddresses_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGridView();
        }
    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindGridView();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindGridView();
    }

    private void doPaging()
    {
        try
        {

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            int CurrentPg = pds.CurrentPageIndex + 1;

            if (CurrentPg > ToPg)
            {
                FrmPg = ToPg + 1;
                ToPg = ToPg + 5;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - 5;

            }

            if (pds.PageCount < ToPg)
            {
                ToPg = pds.PageCount;
            }

            for (int i = FrmPg - 1; i < ToPg; i++)
            {
                System.Data.DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }



            dtlAddresses.DataSource = dt;
            dtlAddresses.DataBind();

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion
}
