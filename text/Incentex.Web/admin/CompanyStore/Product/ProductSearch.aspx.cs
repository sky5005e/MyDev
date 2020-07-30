using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyStore_Product_ProductSearch : PageBase
{
    #region Data Members
    LookupRepository objLookupRepos = new LookupRepository();
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Product Search";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Search";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";
            BindValues();
            FillCompanyStore();
            FillMasterItemNumber();
        }
    }
    protected void ddlCompanyStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillMasterItemNumber();
    }

    protected void ddlMasterItemNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMasterItemNo.SelectedItem.Text != "-Enter Item Number-")
        {
            FillItemNumber(Convert.ToInt32(ddlMasterItemNo.SelectedValue));
            trMasterItemNumberKeyword.Visible = false;
        }
        else
        {
            trItemNumber.Visible = false;
            trMasterItemNumberKeyword.Visible = true;
        }

    }
    protected void lnkSubmitRequest_Click(object sender, EventArgs e)
    {
        string MasterItemNumber = null;
        string ItemNumber = null;
        string Price = null;
        string StoreId = null;
        string WorkgroupID = null;
        string Keyword = null;
        string MasterItemNumberKeyword = null;
        string ReportViewType = null;

        if (ddlCompanyStore.SelectedIndex > 0)
            StoreId = ddlCompanyStore.SelectedValue;
        if (ddlMasterItemNo.SelectedIndex > 0 && ddlMasterItemNo.SelectedItem.Text != "-Enter Item Number-")
            MasterItemNumber = ddlMasterItemNo.SelectedItem.Text;
        else if (ddlMasterItemNo.SelectedIndex > 0)
            MasterItemNumber = txtMasterItemNumberKeyword.Text.Trim();
        if (ddlItemNo.SelectedIndex > 0)
            ItemNumber = ddlItemNo.SelectedItem.Text;
        if (ddlReportViewType.SelectedIndex > 0)
            ReportViewType = ddlReportViewType.SelectedItem.Value;
        if (ddlWorkgroup.SelectedIndex > 0)
            WorkgroupID = ddlWorkgroup.SelectedValue;
        if (txtPrice.Text.Trim()!=string.Empty)
            Price = txtPrice.Text.Trim();
        if (txtKeyword.Text.Trim() != string.Empty)
            Keyword = txtKeyword.Text.Trim();


        Response.Redirect("ProductSearchResult.aspx?MasterNo=" + MasterItemNumber + "&ItemNumber=" + ItemNumber + "&ReportViewType=" + ReportViewType + "&Price=" + Price + "&StoreID=" + StoreId + "&WorkgroupID=" + WorkgroupID + "&Description=" + Keyword);
    }
    #endregion

    #region Methods
    private void BindValues()
    {
        ddlWorkgroup.DataSource = objLookupRepos.GetByLookup("Workgroup").OrderBy(x => x.sLookupName).ToList();
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));
    }
    private void FillCompanyStore()
    {
        OrderConfirmationRepository objLookRep = new OrderConfirmationRepository();
        ddlCompanyStore.DataSource = objLookRep.GetCompanyStoreName().OrderBy(le => le.CompanyName);
        ddlCompanyStore.DataValueField = "StoreID";
        ddlCompanyStore.DataTextField = "CompanyName";
        ddlCompanyStore.DataBind();
        ddlCompanyStore.Items.Insert(0, new ListItem("-Select-", "0"));
    }
    private void FillMasterItemNumber()
    {
        Int64? storeID = null;
        if (ddlCompanyStore.SelectedIndex != 0)
            storeID = Convert.ToInt64(ddlCompanyStore.SelectedValue);

        ddlMasterItemNo.Items.Clear();
        ddlMasterItemNo.DataSource = new StoreProductRepository().GetAllMasterItems(storeID).OrderBy(x => x.sLookupName);
        ddlMasterItemNo.DataValueField = "iLookupID";
        ddlMasterItemNo.DataTextField = "sLookupName";
        ddlMasterItemNo.DataBind();
        ddlMasterItemNo.Items.Insert(0, new ListItem("-select-", "0"));
        ddlMasterItemNo.Items.Insert(1, new ListItem("-Enter Item Number-", ""));
    }

    private void FillItemNumber(int iMasterItemNo)
    {
        try
        {
            ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
            List<ProductItem> objlist = objRepository.GetAllProductItem(iMasterItemNo);
            ddlItemNo.Items.Clear();
            if (objlist.Count > 0)
            {
                trItemNumber.Visible = true;
                ddlItemNo.DataSource = objlist.OrderBy(x => x.ItemNumber);
                ddlItemNo.DataValueField = "ItemNumber";
                ddlItemNo.DataTextField = "ItemNumber";
                ddlItemNo.DataBind();
                ddlItemNo.Items.Insert(0, new ListItem("-select-", "0"));
            }
            else
                trItemNumber.Visible = false;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion
}
