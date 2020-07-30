using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using System.Data;
using Incentex.DAL;
using System.IO;
using System.Text;

public partial class admin_PurchaseOrderManagement_PostPurchaseOrder : PageBase
{
    #region Data Member's
    LookupRepository objLookRep = new LookupRepository();
    SupplierRepository objSupplier = new SupplierRepository();
    PurchaseOrderManagmentRepository objordermanagmentRep = new PurchaseOrderManagmentRepository();
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Purchase Order Management";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Purchase Order Management";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/PurchaseOrderManagement/PurchaseOrderManagement.aspx";

            this.BindDropdown();
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        PurchaseOrderMaster objOrder = new PurchaseOrderMaster();
        List<PurchaseOrderDetail> lstobjOrderDetails = new List<PurchaseOrderDetail>();

        objOrder.VendorID = Convert.ToInt64(ddlvendor.SelectedValue);
        objOrder.OrderFor = Convert.ToInt64(ddlOrderfor.SelectedValue);
        objOrder.OverseasVendor = Convert.ToInt64(ddlOverseasVendor.SelectedValue);
        objOrder.PurchaseOrderNumber = txtOrderNumber.Text.Trim();
        objOrder.OrderSentBy = Convert.ToInt64(ddlOrderSentby.SelectedValue);
        objOrder.MasterItemID = Convert.ToInt64(ddlMasterItem.SelectedValue);
        objOrder.IsDeleted = false;


        #region Saving Attachments
        String SavedFileName = string.Empty;
        decimal FileSize = 0;
        string extension = string.Empty;
        if (Request.Files.Count > 0)
        {
            HttpFileCollection Attachments = Request.Files;

            HttpPostedFile Attachment = Attachments[0];
            if (Attachment.ContentLength > 0 && !String.IsNullOrEmpty(Attachment.FileName))
            {
                SavedFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Attachment.FileName;
                SavedFileName = Common.MakeValidFileName(SavedFileName);
                String filePath = Server.MapPath("../../UploadedImages/PurchaseOrderManagement/") + SavedFileName;
                Attachment.SaveAs(filePath);
                FileSize = Attachment.ContentLength;
                extension = Path.GetExtension(Attachments[0].FileName).ToLower();
            }

            objOrder.FileName = SavedFileName;
            objOrder.OriginalFileName = Attachment.FileName;
            objOrder.extension = Path.GetExtension(Attachments[0].FileName).ToLower();
        }
        #endregion

        objOrder.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objOrder.CreatedDate = System.DateTime.Now;

        //Add OrderDetails 
        foreach (GridViewRow row in gvItemDetails.Rows)
        {
            TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
            TextBox txtPrice = (TextBox)row.FindControl("txtPrice");

            if (((CheckBox)(row.FindControl("chkItem"))).Checked && !string.IsNullOrEmpty(txtQuantity.Text.Trim()) && !string.IsNullOrEmpty(txtPrice.Text.Trim()))
            {
                PurchaseOrderDetail objorderdetails = new PurchaseOrderDetail();
                Label lblProductItemID = (Label)row.FindControl("lblProductItemID");

                objorderdetails.ProductItemID = Convert.ToInt64(lblProductItemID.Text.Trim());
                objorderdetails.Quantity = Convert.ToInt32(txtQuantity.Text.Trim());
                objorderdetails.Price = Convert.ToInt64(txtPrice.Text.Trim());

                lstobjOrderDetails.Add(objorderdetails);
            }
        }

        if (lstobjOrderDetails != null && lstobjOrderDetails.Count > 0)
        {
            objordermanagmentRep.Insert(objOrder);
            objordermanagmentRep.SubmitChanges();
            objordermanagmentRep.InsertPurchaseOrderDetails(lstobjOrderDetails, objOrder.PurchaseOrderID);

            Response.Redirect("PurchaseOrderManagement.aspx?req=1");
        }
        else
        {
            lblMsg.Text = "Please select at least one product";
        }
    }
    #endregion

    #region Method
    private void BindDropdown()
    {
        ddlMasterItem.Items.Insert(0, new ListItem("-Select-", "0"));

        #region Orderfor
        ddlOrderfor.DataSource = objLookRep.GetByLookup("Purchase Order for");
        ddlOrderfor.DataValueField = "iLookupID";
        ddlOrderfor.DataTextField = "sLookupName";
        ddlOrderfor.DataBind();
        ddlOrderfor.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion

        #region Overseas Vendor
        ddlOverseasVendor.DataSource = objLookRep.GetByLookup("Overseas Vendor");
        ddlOverseasVendor.DataValueField = "iLookupID";
        ddlOverseasVendor.DataTextField = "sLookupName";
        ddlOverseasVendor.DataBind();
        ddlOverseasVendor.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion

        #region PurchaseOrderSentby
        ddlOrderSentby.DataSource = objLookRep.GetByLookup("Purchase Order Sent by");
        ddlOrderSentby.DataValueField = "iLookupID";
        ddlOrderSentby.DataTextField = "sLookupName";
        ddlOrderSentby.DataBind();
        ddlOrderSentby.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion

        #region Vendor
        ddlvendor.DataSource = objSupplier.GetAllsupplier();
        ddlvendor.DataValueField = "SupplierID";
        ddlvendor.DataTextField = "FirstName";
        ddlvendor.DataBind();
        ddlvendor.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion
    }

    public void ddlvendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region Mastr Item
        List<StoreProductCustom> objlst = objordermanagmentRep.GetAllMasterItemBysupplierID(Convert.ToInt64(ddlvendor.SelectedValue));
        ddlMasterItem.DataSource = objlst;
        ddlMasterItem.DataValueField = "storeproductid";
        ddlMasterItem.DataTextField = "summary";
        ddlMasterItem.DataBind();
        ddlMasterItem.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion
    }

    public void ddlMasterItem_SelectedIndexchanged(object sender, EventArgs e)
    {
        string ImagePath = objordermanagmentRep.GetProductImages(Convert.ToInt64(ddlMasterItem.SelectedValue));

        if (string.IsNullOrEmpty(ImagePath))
            imgmasteritem.Src = "~/UploadedImages/ProductImages/ProductDefault.jpg";
        else
            imgmasteritem.Src = "~/UploadedImages/ProductImages/Thumbs/" + ImagePath;

        #region Listing of Item Sizes
        List<ProductItemDetailsRepository.ProductItemResult> oad = new ProductItemDetailsRepository().ProductItemDetails(Convert.ToInt32(ddlMasterItem.SelectedValue));

        DataView myDataView = new DataView();
        DataTable dataTable = Common.ListToDataTable(oad.Where(t => t.ItemNumberStatusID == 135).ToList());    //Status Active 
        myDataView = dataTable.DefaultView;

        if (this.ViewState["SortExp"] != null)
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();

        gvItemDetails.DataSource = myDataView;
        gvItemDetails.DataBind();
        #endregion
    }

    protected void gvItemDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    #endregion
}
