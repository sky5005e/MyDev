using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class SAPBillingAddressSetting : PageBase
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

    Int64 BillingAddressID
    {
        get
        {
            return Convert.ToInt64(ViewState["BillingAddressID"]);
        }
        set
        {
            ViewState["BillingAddressID"] = value;
        }
    }

    PagedDataSource pds = new PagedDataSource();

    public Int32 CurrentPage
    {
        get
        {
            return Convert.ToInt32(this.ViewState["CurrentPage"]);
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
                return Convert.ToInt32(this.ViewState["PagerSize"]);
        }
        set
        {
            this.ViewState["PagerSize"] = value;
        }
    }

    public Int32 FrmPg
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

    public Int32 ToPg
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

    SAPBillingAddressRepository objRepo = new SAPBillingAddressRepository();

    #endregion

    #region Page Events

    protected void Page_Load(Object sender, EventArgs e)
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
                menuControl.PopulateMenu(11, 5, this.StoreID, this.WorkGroupID, true);

                BindDDL();
                BindGridView();
                lblStore.Text = new CompanyRepository().GetById(new CompanyStoreRepository().GetById(StoreID).CompanyID).CompanyName;
                lblWorkGroup.Text = new LookupRepository().GetById(this.WorkGroupID).sLookupName.ToString();
            }
        }
    }

    #endregion

    #region Control Events

    protected void DrpBillingCountry_SelectedIndexChanged(Object sender, EventArgs e)
    {
        #region State
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(DrpBillingCountry.SelectedValue));

        DrpBillingState.Items.Clear();
        DrpBillingCity.Items.Clear();
        DrpBillingCity.Items.Insert(0, new ListItem("-select city-", "0"));
        Common.BindDDL(DrpBillingState, objStateList, "sStatename", "iStateID", "-select state-");
        #endregion

        #region BasedStation
        BaseStationRepository objBaseStation = new BaseStationRepository();
        List<INC_BasedStation> objbasestationlist = objBaseStation.GetAllBaseStationbyCountryID(Convert.ToInt64(DrpBillingCountry.SelectedValue));
        ddlBasedStation.Items.Clear();
        Common.BindDDL(ddlBasedStation, objbasestationlist, "sBaseStation", "iBaseStationId", "-select Basestation-");
        #endregion
    }

    protected void DrpBillingState_SelectedIndexChanged(Object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(DrpBillingState.SelectedValue));
        DrpBillingCity.Items.Clear();
        Common.BindDDL(DrpBillingCity, objCityList, "sCityName", "iCityID", "-select city-");
    }

    protected void ddlBasedStation_SelectedIndexChanged(Object sender, EventArgs e)
    {
        SAPBillingAddressRepository objRepo = new SAPBillingAddressRepository();
        if (ddlBasedStation.SelectedIndex > 0 && objRepo.CheckIsExist(this.StoreID, this.WorkGroupID, Convert.ToInt64(ddlBasedStation.SelectedValue), this.BillingAddressID))
            lblMsg.Text = "Record alredy exits for this Base Station";
        else
            lblMsg.Text = String.Empty;
    }

    protected void lnkSave_Click(Object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(txtSAPBillToCode.Text.Trim()))
            {
                lblMsg.Text = "Please enter SAP bill to code.";
                return;
            }

            if (String.IsNullOrEmpty(DrpBillingCountry.SelectedValue))
            {
                lblMsg.Text = "Please select country.";
                return;
            }

            if (String.IsNullOrEmpty(DrpBillingState.SelectedValue) || DrpBillingState.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select state.";
                return;
            }

            if (String.IsNullOrEmpty(DrpBillingCity.SelectedValue) || DrpBillingCity.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select city.";
                return;
            }

            if (String.IsNullOrEmpty(ddlBasedStation.SelectedValue) || ddlBasedStation.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Base Station.";
                return;
            }

            SAPBillingAddress objAddress;
            if (!objRepo.CheckIsExist(this.StoreID, this.WorkGroupID, Convert.ToInt64(ddlBasedStation.SelectedValue), this.BillingAddressID))
            {
                if (this.BillingAddressID == 0)
                {
                    objAddress = new SAPBillingAddress();
                    objAddress.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                    objAddress.CreatedDate = DateTime.Now;
                    objAddress.StoreID = this.StoreID;
                    objAddress.WorkGroupID = this.WorkGroupID;
                }
                else
                {
                    objAddress = objRepo.GetByID(this.BillingAddressID);
                }

                objAddress.AddressLine1 = txtAddressLine1.Text.Trim();
                objAddress.AddressLine2 = txtAddressLine2.Text.Trim();
                objAddress.CityID = Convert.ToInt64(DrpBillingCity.SelectedValue);
                objAddress.CompanyName = txtCompany.Text.Trim();
                objAddress.CountryID = Convert.ToInt64(DrpBillingCountry.SelectedValue);
                objAddress.BaseStationId = Convert.ToInt64(ddlBasedStation.SelectedValue);
                objAddress.Email = txtEmail.Text.Trim();
                objAddress.FirstName = txtFirstName.Text.Trim();
                objAddress.LastName = txtLastName.Text.Trim();
                objAddress.Mobile = txtMobile.Text.Trim();
                objAddress.StateID = Convert.ToInt64(DrpBillingState.SelectedValue);
                objAddress.SAPBillToCode = txtSAPBillToCode.Text.Trim();
                objAddress.Telephone = txtTelephone.Text.Trim();
                objAddress.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objAddress.UpdatedDate = DateTime.Now;
                objAddress.ZipCode = txtZip.Text.Trim();

                if (this.BillingAddressID == 0)
                    objRepo.Insert(objAddress);

                objRepo.SubmitChanges();

                lblMsg.Text = "Record saved successfully...";

                BindGridView();
                Clear();
            }
            else
            {
                lblMsg.Text = "Record alredy exits for this Basedstion";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Some error occured while saving the record...";
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvbillingaddress_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMsg.Text = "";

        switch (e.CommandName)
        {
            case "EditRec":
                Clear();
                this.BillingAddressID = Convert.ToInt64(e.CommandArgument);
                this.DisplayData();
                break;
            case "DeleteRec":
                try
                {
                    objRepo.DeleteByID(Convert.ToInt64(e.CommandArgument));
                    BindGridView();
                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex);
                    lblMsg.Text = "Error in deleting record ...";
                }

                break;
            case "Sort":
                if (this.ViewState["SortExp"] == null)
                {
                    this.ViewState["SortExp"] = Convert.ToString(e.CommandArgument);
                    this.ViewState["SortOrder"] = "ASC";
                }
                else
                {
                    if (Convert.ToString(this.ViewState["SortExp"]) == Convert.ToString(e.CommandArgument))
                    {
                        if (Convert.ToString(this.ViewState["SortOrder"]) == "ASC")
                            this.ViewState["SortOrder"] = "DESC";
                        else
                            this.ViewState["SortOrder"] = "ASC";

                    }
                    else
                    {
                        this.ViewState["SortOrder"] = "ASC";
                        this.ViewState["SortExp"] = Convert.ToString(e.CommandArgument);
                    }
                }

                BindGridView();
                break;
        }
    }

    #endregion

    #region Page Methods

    private void BindGridView()
    {
        DataView myDataView = new DataView();
        List<SAPBillingAddressCustom> list = objRepo.GetByStoreAndWorkGroupID(this.StoreID, this.WorkGroupID);

        if (list.Count == 0)
            pagingtable.Visible = false;
        else
            pagingtable.Visible = true;

        DataTable dataTable = Common.ListToDataTable(list);
        myDataView = dataTable.DefaultView;

        if (this.ViewState["SortExp"] != null)
            myDataView.Sort = Convert.ToString(this.ViewState["SortExp"]) + " " + Convert.ToString(this.ViewState["SortOrder"]);

        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;

        gvbillingaddress.DataSource = pds;
        gvbillingaddress.DataBind();

        doPaging();
    }

    private void BindDDL()
    {
        CountryRepository objCountryBillingRepo = new CountryRepository();
        List<INC_Country> objBillingCountryList = objCountryBillingRepo.GetAll();
        Common.BindDDL(DrpBillingCountry, objBillingCountryList, "sCountryName", "iCountryID", "-select country-");
        DrpBillingCountry.SelectedValue = DrpBillingCountry.Items.FindByText("United States").Value;
        DrpBillingCountry_SelectedIndexChanged(null, null);
    }

    private void DisplayData()
    {
        try
        {
            if (this.BillingAddressID > 0)
            {
                SAPBillingAddress objAddress = new SAPBillingAddressRepository().GetByID(this.BillingAddressID);

                DrpBillingCountry.SelectedValue = objAddress.CountryID != null ? Convert.ToString(objAddress.CountryID) : "0";
                DrpBillingCountry_SelectedIndexChanged(null, null);

                if (objAddress.CountryID != null)
                {
                    ddlBasedStation.SelectedValue = objAddress.BaseStationId.HasValue ? Convert.ToString(objAddress.BaseStationId) : "0";
                    DrpBillingState.SelectedValue = objAddress.StateID != null ? Convert.ToString(objAddress.StateID) : "0";
                    DrpBillingState_SelectedIndexChanged(null, null);

                    if (objAddress.StateID != null)
                    {
                        DrpBillingCity.SelectedValue = objAddress.CityID != null ? Convert.ToString(objAddress.CityID) : "0";
                    }
                }

                txtAddressLine1.Text = Convert.ToString(objAddress.AddressLine1);
                txtAddressLine2.Text = Convert.ToString(objAddress.AddressLine2);
                txtCompany.Text = Convert.ToString(objAddress.CompanyName);
                txtEmail.Text = Convert.ToString(objAddress.Email);
                txtFirstName.Text = Convert.ToString(objAddress.FirstName);
                txtLastName.Text = Convert.ToString(objAddress.LastName);
                txtMobile.Text = Convert.ToString(objAddress.Mobile).Trim();
                txtSAPBillToCode.Text = Convert.ToString(objAddress.SAPBillToCode);
                txtTelephone.Text = Convert.ToString(objAddress.Telephone);
                txtZip.Text = Convert.ToString(objAddress.ZipCode);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void Clear()
    {
        txtAddressLine1.Text = String.Empty;
        txtAddressLine2.Text = String.Empty;
        txtCompany.Text = String.Empty;
        txtEmail.Text = String.Empty;
        txtFirstName.Text = String.Empty;
        txtLastName.Text = String.Empty;
        txtMobile.Text = String.Empty;
        txtSAPBillToCode.Text = String.Empty;
        txtTelephone.Text = String.Empty;
        txtZip.Text = String.Empty;
        ddlBasedStation.SelectedIndex = 0;
        DrpBillingCountry.SelectedIndex = 0;
        DrpBillingState.SelectedIndex = 0;
        DrpBillingCity.SelectedIndex = 0;
        this.BillingAddressID = 0;
    }

    #region Paging

    protected void dtlPaging_ItemCommand(Object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGridView();
        }
    }

    protected void dtlPaging_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    private void doPaging()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            Int32 CurrentPg = pds.CurrentPageIndex + 1;

            if (CurrentPg > ToPg)
            {
                FrmPg = ToPg + 1;
                ToPg = ToPg + PagerSize;
            }

            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - PagerSize;
            }

            if (pds.PageCount < ToPg)
                ToPg = pds.PageCount;

            for (Int32 i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnPrevious_Click(Object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindGridView();
    }

    protected void lnkbtnNext_Click(Object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindGridView();
    }

    #endregion

    #endregion
}