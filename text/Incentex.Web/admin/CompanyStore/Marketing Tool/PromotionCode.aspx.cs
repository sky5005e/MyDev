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
using Incentex.DAL;
using System.Collections.Generic;
using Incentex.DA;
public partial class admin_CompanyStore_Marketing_Tool_PromotionCode : PageBase
{
    #region Local Property
    Int64 CompanyStoreId
    {
        get
        {
            if (ViewState["CompanyStoreId"] == null)
            {
                ViewState["CompanyStoreId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {
            ViewState["CompanyStoreId"] = value;
        }
    }
    PromotionCodeDetail objPromotionCode = new PromotionCodeDetail();
    MarketingToolRepository objMarketingToolRepository = new MarketingToolRepository();
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            CheckLogin();
            if (!IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));
                    ((Label)Master.FindControl("lblPageHeading")).Text = "Promotion Code";
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/Marketing Tool/MarketingTool.aspx?id=" + this.CompanyStoreId;

                    BindValues();
                }

            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            objPromotionCode.StoreID = this.CompanyStoreId;
            if (ddlWorkgroup.SelectedIndex != 0)
                objPromotionCode.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);            
            if (ddlPriceLevel.SelectedIndex != 0)
                objPromotionCode.PriceLevel = Convert.ToInt64(ddlPriceLevel.SelectedValue);
            if (ddlItem.SelectedIndex!=0)
                objPromotionCode.ProductItemID = Convert.ToInt64(ddlItem.SelectedValue);
           
            objPromotionCode.PromotionTypeID = Convert.ToInt64(ddlPromotionType.SelectedValue);
            objPromotionCode.PromotionCode = txtPromotionCode.Text.Trim();
            objPromotionCode.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
            objPromotionCode.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
            objPromotionCode.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objPromotionCode.CreatedDate = DateTime.Now;
            if (Convert.ToDateTime(txtEndDate.Text.Trim()) >= Convert.ToDateTime(txtStartDate.Text.Trim()))
            {
                objMarketingToolRepository.Insert(objPromotionCode);
                objMarketingToolRepository.SubmitChanges();
                RestControls();
                lblMsg.Text = "Record Saved Successfully";
                // Bind Item List again
                BindItemList();
            }
            else
            {
                lblMsg.Text = "Start Date Should be lesser or equal to End Date";
            }

        }
        catch (Exception)
        {
            lblMsg.Text = "Error Saving";
        }
    }
    private void RestControls()
    {
        ddlWorkgroup.SelectedIndex = 0;
        ddlItem.SelectedIndex = 0;
        ddlPromotionType.SelectedIndex = 0;
        txtPromotionCode.Text = "";
        ddlPriceLevel.SelectedIndex = 0;
        txtStartDate.Text = "";
        txtEndDate.Text = "";
    }
    public void BindValues()
    {
        try
        {
            LookupRepository objLookRep = new LookupRepository();
            // For Workgroup
            ddlWorkgroup.DataSource = objLookRep.GetByLookup("Workgroup");
            ddlWorkgroup.DataValueField = "iLookupID";
            ddlWorkgroup.DataTextField = "sLookupName";
            ddlWorkgroup.DataBind();
            ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));

            // For price level
            ddlPriceLevel.DataSource = objLookRep.GetByLookup("PriceLevel");
            ddlPriceLevel.DataValueField = "iLookupID";
            ddlPriceLevel.DataTextField = "sLookupName";
            ddlPriceLevel.DataBind();
            ddlPriceLevel.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlPriceLevel.Items.RemoveAt(5);            
           
            //Select for Item
            ddlItem.Items.Insert(0, new ListItem("-Select-", "0"));

        }
        catch (Exception)
        {


        }
    }
    protected void ddlWorkgroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindItemList();
        }
        catch (Exception)
        {}
    }
    private void BindItemList()
    {
        try
        {
            StoreProductRepository objStoreProdRepository = new StoreProductRepository();
            List<MarketingToolRepository.GetItemListResult> lstitems = new List<MarketingToolRepository.GetItemListResult>();
            //lstStoreProduct = objStoreProdRepository.StoreProductDetails(Convert.ToInt32(this.CompanyStoreId), Convert.ToInt16(ddlWorkgroup.SelectedValue));

            lstitems = objMarketingToolRepository.GetItemList(Convert.ToInt64(ddlWorkgroup.SelectedValue), this.CompanyStoreId);

            ddlItem.DataSource = lstitems;
            ddlItem.DataValueField = "Value";
            ddlItem.DataTextField = "Text";
            ddlItem.DataBind();
            ddlItem.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception)
        {}
    }
}
