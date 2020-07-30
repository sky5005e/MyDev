using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Data;

public partial class NewDesign_Admin_IssuancePolicy_IssuanceProgram : PageBase
{
    #region Properties

    public Int64? WorkGroupID
    {
        get
        {
            if (this.ViewState["WorkGroupID"] == null || Convert.ToString(this.ViewState["WorkGroupID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["WorkGroupID"]);
        }
        set
        {
            this.ViewState["WorkGroupID"] = value;
        }
    }

    public Int64? GenderID
    {
        get
        {
            if (this.ViewState["GenderID"] == null || Convert.ToString(this.ViewState["GenderID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["GenderID"]);
        }
        set
        {
            this.ViewState["GenderID"] = value;
        }
    }

    public String KeyWord
    {
        get
        {
            if (Convert.ToString(this.ViewState["KeyWord"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["KeyWord"]);
        }
        set
        {
            this.ViewState["KeyWord"] = value;
        }
    }

    public Int64? CompanyID
    {
        get
        {
            if (this.ViewState["CompanyID"] == null || Convert.ToString(this.ViewState["CompanyID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["CompanyID"]);
        }
        set
        {
            this.ViewState["CompanyID"] = value;
        }
    }

    public Int32 NoOfRecordsToDisplay
    {
        get
        {
            if (Convert.ToString(this.ViewState["NoOfRecordsToDisplay"]) == "")
                return Convert.ToInt32(Application["NEWPAGESIZE"]);
            else
                return Convert.ToInt32(this.ViewState["NoOfRecordsToDisplay"]);
        }
        set
        {
            this.ViewState["NoOfRecordsToDisplay"] = value;
        }
    }

    public Int32 PageIndex
    {
        get
        {
            if (Convert.ToString(this.ViewState["PageIndex"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["PageIndex"]);
        }
        set
        {
            this.ViewState["PageIndex"] = value;
        }
    }

    public String SortColumn
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortColumn"]) == String.Empty)
                return "EmployeeName";
            else
                return Convert.ToString(this.ViewState["SortColumn"]);
        }
        set
        {
            this.ViewState["SortColumn"] = value;
        }
    }

    public String SortDirection
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortDirection"]) == String.Empty)
                return "ASC";
            else
                return Convert.ToString(this.ViewState["SortDirection"]);
        }
        set
        {
            this.ViewState["SortDirection"] = value;
        }
    }

    public Int32 TotalPages
    {
        get
        {
            if (Convert.ToString(this.ViewState["TotalPages"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["TotalPages"]);
        }
        set
        {
            this.ViewState["TotalPages"] = value;
        }
    }

    public Int32 NoOfPagesToDisplay
    {
        get
        {
            if (Convert.ToString(this.ViewState["NoOfPagesToDisplay"]) == "")
                return Convert.ToInt32(Application["NEWPAGERSIZE"]);
            else
                return Convert.ToInt32(this.ViewState["NoOfPagesToDisplay"]);
        }
        set
        {
            this.ViewState["NoOfPagesToDisplay"] = value;
        }
    }

    public Int32 FromPage
    {
        get
        {
            if (Convert.ToString(this.ViewState["FromPage"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["FromPage"]);
        }
        set
        {
            this.ViewState["FromPage"] = value;
        }
    }

    public Int32 ToPage
    {
        get
        {
            if (Convert.ToString(this.ViewState["ToPage"]) == "")
                return this.NoOfPagesToDisplay;
            else
                return Convert.ToInt32(this.ViewState["ToPage"]);
        }
        set
        {
            this.ViewState["ToPage"] = value;
        }
    }

    public Int64 ActiveID
    {
        get
        {
            return Convert.ToInt64(this.ViewState["ActiveID"]);
        }
        set
        {
            this.ViewState["ActiveID"] = value;
        }
    }

    public Int64 InActiveID
    {
        get
        {
            return Convert.ToInt64(this.ViewState["InActiveID"]);
        }
        set
        {
            this.ViewState["InActiveID"] = value;
        }
    }

    public Int64? IssuanceStatusID
    {
        get
        {
            if (this.ViewState["IssuanceStatusID"] == null || Convert.ToString(this.ViewState["IssuanceStatusID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["IssuanceStatusID"]);
        }
        set
        {
            this.ViewState["IssuanceStatusID"] = value;
        }
    }
    #endregion

    #region Page Event's

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillDropDowns();
        }
    }
    protected void LnkbtnAddNewIssuance_Click(object sender, EventArgs e)
    {
        Response.Redirect("IssuancePolicySetup.aspx");
    }
    protected void btnSearchGrid_Click(object sender, EventArgs e)
    {
        try
        {
            this.KeyWord = txtSearchGrid.Text.Trim();
            BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void btnSearchIssuancePolicy_Click(object sender, EventArgs e)
    {
        try
        {
            this.CompanyID = Convert.ToInt64(ddlSearchCompany.SelectedValue);
            this.WorkGroupID = Convert.ToInt64(ddlSearchWorkGroup.SelectedValue);
            String _gender = ddlSearchGender.SelectedItem.Text == "UniSex" ? null : ddlSearchGender.SelectedValue;
            this.GenderID = Convert.ToInt64(_gender);
            this.IssuanceStatusID = Convert.ToInt64(ddlSearchStatus.SelectedValue);
            this.KeyWord = String.Empty;
            this.NoOfRecordsToDisplay = Convert.ToInt32(Application["NEWPAGESIZE"]);
            this.PageIndex = 1;

            btnSearchGrid.Enabled = true;
            btnSearchGrid.CssClass += " postback";
            txtSearchGrid.Text = String.Empty;

            //if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
            //this.CompanyID = Convert.ToInt64(3);
            BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvIssuancePolicy_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Sort")
            {
                if (this.SortColumn == null)
                {
                    this.SortColumn = Convert.ToString(e.CommandArgument);
                    this.SortDirection = "ASC";
                }
                else
                {
                    if (this.SortColumn == Convert.ToString(e.CommandArgument))
                    {
                        if (this.SortDirection == "ASC")
                            this.SortDirection = "DESC";
                        else
                            this.SortDirection = "ASC";
                    }
                    else
                    {
                        this.SortDirection = "ASC";
                        this.SortColumn = Convert.ToString(e.CommandArgument);
                    }
                }

                BindGrid(true);
            }
            else if (e.CommandName == "Detail")
            {
                //hdnBasicUserInfoID.Value = e.CommandArgument.ToString();
                this.ClientScript.RegisterStartupScript(this.GetType(), new Guid().ToString(), "IssuanceDetailsPopup('setup');", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    

    protected void chkStatus_CheckedChanged(Object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlSearchCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillWorkGroupForStore(Convert.ToInt64(ddlSearchCompany.SelectedValue));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion

    #region Paging Events

    protected void dtlPaging_ItemCommand(Object sender, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ChangePage")
            {
                this.PageIndex = Convert.ToInt32(e.CommandArgument);
                BindGrid(false);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dtlPaging_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            LinkButton lnkPaging = (LinkButton)e.Item.FindControl("lnkPaging");

            if (lnkPaging.Text == Convert.ToString(this.PageIndex))
            {
                lnkPaging.Enabled = false;
                lnkPaging.Font.Bold = true;
                lnkPaging.Attributes.Add("style", "cursor:pointer;");
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkPrevious_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex -= 1;
            BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkNext_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex += 1;
            BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkViewAll_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex = 1;
            this.NoOfRecordsToDisplay = 0;
            BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Methods

    private void GeneratePaging()
    {
        try
        {
            Int32 tCurrentPg = this.PageIndex;
            Int32 tToPg = this.ToPage;
            Int32 tFrmPg = this.FromPage;

            if (tCurrentPg > tToPg)
            {
                tFrmPg = tToPg + 1;
                tToPg += this.NoOfPagesToDisplay;
            }

            if (tCurrentPg < tFrmPg)
            {
                tToPg = tFrmPg - 1;
                tFrmPg -= this.NoOfPagesToDisplay;
            }

            if (tToPg > this.TotalPages)
                tToPg = this.TotalPages;

            this.ToPage = tToPg;
            this.FromPage = tFrmPg;

            DataTable dt = new DataTable();
            dt.Columns.Add("Index");

            for (Int32 i = tFrmPg; i <= tToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Index"] = i;
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

    /// <summary>
    /// Fills the Workgroup.
    /// </summary>
    /// <param name="countryid">The CompanyID.</param>
    public void FillWorkGroupForStore(Int64 CompanyID)
    {
        ddlSearchWorkGroup.Items.Clear();
        Int64 StoreID = 0;
        if (CompanyID > 0)
            StoreID = new CompanyStoreRepository().GetStoreIDByCompanyId(Convert.ToInt64(CompanyID));
        List<GetStoreWorkGroupsResult> lstWorkGroups = new CompanyStoreRepository().GetStoreWorkGroups(StoreID).OrderBy(le => le.WorkGroup).ToList();
        if (lstWorkGroups.Where(le => le.Existing == 1).ToList().Count > 0)
            ddlSearchWorkGroup.DataSource = lstWorkGroups.Where(le => le.Existing == 1);
        else
            ddlSearchWorkGroup.DataSource = lstWorkGroups;
        ddlSearchWorkGroup.DataValueField = "WorkGroupID";
        ddlSearchWorkGroup.DataTextField = "WorkGroup";
        ddlSearchWorkGroup.DataBind();
        ddlSearchWorkGroup.Items.Insert(0, new ListItem("-Workgroup-", "0"));
    }

    private void FillDropDowns()
    {
        try
        {
            ddlSearchCompany.DataSource = new CompanyRepository().GetAllCompany();
            ddlSearchCompany.DataValueField = "CompanyID";
            ddlSearchCompany.DataTextField = "CompanyName";
            ddlSearchCompany.DataBind();
            ddlSearchCompany.Items.Insert(0, new ListItem("-Company-", "0"));
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
            {
                ddlSearchCompany.SelectedValue = Convert.ToString(IncentexGlobal.CurrentMember.CompanyId);
                ddlSearchCompany.Enabled = false;
            }

            LookupRepository objLookRep = new LookupRepository();
            //For gender
            List<INC_Lookup> genderList = objLookRep.GetByLookup("Gender");
            INC_Lookup objGender = new INC_Lookup();
            objGender.iLookupID = 99;
            objGender.iLookupCode = "Gender";
            objGender.sLookupName = "Unisex";
            genderList.Add(objGender);
            ddlSearchGender.DataSource = genderList.OrderBy(o => o.sLookupName).ToList();// for sorting the value of gender with unisex
            ddlSearchGender.DataValueField = "iLookupID";
            ddlSearchGender.DataTextField = "sLookupName";
            ddlSearchGender.DataBind();
            ddlSearchGender.Items.Insert(0, new ListItem("-Gender-", "0"));
            // For Workgroup
            FillWorkGroupForStore(Convert.ToInt64(ddlSearchCompany.SelectedValue));

            // For Status
            ddlSearchStatus.DataSource = objLookRep.GetByLookup("Status").OrderBy(s => s.sLookupName).ToList();
            ddlSearchStatus.DataValueField = "iLookupID";
            ddlSearchStatus.DataTextField = "sLookupName";
            ddlSearchStatus.DataBind();
            ddlSearchStatus.Items.Insert(0, new ListItem("-Status-", "0"));

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

   

    private void BindGrid(Boolean isForSorting)
    {
        try
        {

            MyIssuanceCartRepository IssuanceCartRepo = new MyIssuanceCartRepository();
            List<GetIssuancePolicyBySearchCriteriaResult> listIssuancePolicy = IssuanceCartRepo.GetIssuancePolicyBySearchCriteria(this.CompanyID, this.WorkGroupID,GenderID,this.IssuanceStatusID, this.KeyWord, this.NoOfRecordsToDisplay, this.PageIndex, this.SortColumn, this.SortDirection);

            if (listIssuancePolicy != null && listIssuancePolicy.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(listIssuancePolicy[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;
            }

            gvIssuancePolicy.DataSource = listIssuancePolicy;
            gvIssuancePolicy.DataBind();

            if (!isForSorting)
            {
                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 1;
                lnkNext.Enabled = this.PageIndex < this.TotalPages;
            }

            if (gvIssuancePolicy.Rows.Count > 0)
            {
                totalcount_em.InnerText = listIssuancePolicy[0].TotalRecords + " results";
                pagingtable.Visible = true;
            }
            else
            {
                totalcount_em.InnerText = "no results";
                pagingtable.Visible = false;
            }

            totalcount_em.Visible = true;
        
            
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
             
    }
    #endregion
}
