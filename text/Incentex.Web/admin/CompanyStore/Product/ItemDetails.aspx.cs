using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_ProductStoreManagement_ItemDetails : PageBase
{
    #region Page Properties

    /// <summary>
    /// To store previous Master  style
    /// </summary>
    Int32 OldProductStyleID
    {
        get
        {
            if (ViewState["OldProductStyleID"] == null)
            {
                ViewState["OldProductStyleID"] = 0;
            }
            return Convert.ToInt32(ViewState["OldProductStyleID"]);
        }
        set
        {
            ViewState["OldProductStyleID"] = value;
        }
    }

    Int64 StoreProductID
    {
        get
        {
            if (ViewState["StoreProductID"] == null)
            {
                ViewState["StoreProductID"] = 0;
            }
            return Convert.ToInt64(ViewState["StoreProductID"]);
        }
        set
        {
            ViewState["StoreProductID"] = value;
        }
    }

    Int64 iStoreID
    {
        get
        {
            if (ViewState["iStoreID"] == null)
            {
                ViewState["iStoreID"] = 0;
            }
            return Convert.ToInt64(ViewState["iStoreID"]);
        }
        set
        {
            ViewState["iStoreID"] = value;
        }
    }

    Int32 iDuplicate;
    Boolean bolflag = false;
    Boolean bolflagMaterItem = false;
    ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
    ProductItem objprodItem = new ProductItem();
    PagedDataSource pds = new PagedDataSource();
    LookupRepository objLookupRepos = new LookupRepository();
    Int32 iProductItmeID;

    /// <summary>
    /// Set the Currentpage no
    /// Nagmani 08/10/2010
    /// </summary>
    public Int32 CurrentPage
    {
        get
        {
            if (this.ViewState["CurrentPage"] == null)
                return 0;
            else
                return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
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
                return Convert.ToInt16(this.ViewState["PagerSize"].ToString());
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
                return Convert.ToInt16(this.ViewState["FrmPg"].ToString());
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
                return Convert.ToInt16(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }

    /// <summary>
    /// To Display WorkgroupName
    /// </summary>
    String WorkGroupNameToDisplay
    {
        get
        {
            if (Session["WorkGroupNameToDisplay"] != null && Session["WorkGroupNameToDisplay"].ToString().Length > 0)
                ViewState["WorkGroupNameToDisplay"] = " - " + Session["WorkGroupNameToDisplay"].ToString();
            else
                ViewState["WorkGroupNameToDisplay"] = "";

            return ViewState["WorkGroupNameToDisplay"].ToString();
        }
    }

    public Int64 AllowBackOrderID
    {
        get
        {
            if (this.ViewState["AllowBackOrderID"] == null)
                return 0;
            else
                return Convert.ToInt16(this.ViewState["AllowBackOrderID"].ToString());
        }
        set
        {
            this.ViewState["AllowBackOrderID"] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        Int32 ManageID = (Int32)Incentex.DAL.Common.DAEnums.ManageID.CompanyProduct;
        Session["ManageID"] = ManageID;

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

            BindValues();

            trNameFormat.Visible = false;
            trFontFormat.Visible = false;

            if (Request.QueryString["Id"] != null)
                iStoreID = Convert.ToInt64(Request.QueryString["Id"].ToString());

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Item Details" + WorkGroupNameToDisplay;
            ((HtmlGenericControl)menuControl.FindControl("dvSubMenu")).Visible = false;
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.ProductReturnURL;


            if (Request.QueryString["SubId"] != null && Request.QueryString["SubId"] != "0" && Request.QueryString["SubId"] != "")
            {
                StoreProductID = Convert.ToInt32(Request.QueryString["SubId"].ToString());
                menuControl.PopulateMenu(1, 0, StoreProductID, iStoreID, false);
                AllowBackOrderID = objRepository.GetProductBackOrderId(StoreProductID);
                INC_Lookup objLook = objLookupRepos.GetById(AllowBackOrderID);

                if (AllowBackOrderID > 0 && objLook.Val1 == "Backorder's set at item level")
                {
                    trallowforbackorder.Visible = true;
                    FillAllowbackOrders();
                }
                else
                {
                    trallowforbackorder.Visible = false;
                }
                ClearData();
                bindGridView();
            }
            else
            {
                Response.Redirect("General.aspx?SubId=" + Request.QueryString["SubId"] + "&Id=" + Request.QueryString["Id"]);
            }
        }
    }

    #endregion

    #region Page Control Events

    /// <summary>
    /// Save the record in product item
    /// table.
    /// lnkAddItem_Click()
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkAddItem_Click(object sender, EventArgs e)
    {
        try
        {
            SaveData();

            Session["iProductItmeId"] = null;
            ClearData();

            if (bolflagMaterItem == false)
            {
                if (bolflag)
                    txtItemNumber.Focus();
            }

            spanSaveitem.Visible = false;
            spanAddItem.Visible = true;

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    //bind dropdown 
    public void BindValues()
    {
        LookupDA sLookup = new LookupDA();
        LookupBE sLookupBE = new LookupBE();

        //For StyleNo
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "StyleNo";
        ddlStyle.DataSource = sLookup.LookUp(sLookupBE);
        ddlStyle.DataValueField = "iLookupID";
        ddlStyle.DataTextField = "sLookupName";
        ddlStyle.DataBind();
        ddlStyle.Items.Insert(0, new ListItem("-Select-", "0"));

        // For MasterItemNumber
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "MasterItemNumber";
        ddlMasterNumber.DataSource = sLookup.LookUp(sLookupBE);
        ddlMasterNumber.DataValueField = "iLookupID";
        ddlMasterNumber.DataTextField = "sLookupName";
        ddlMasterNumber.DataBind();
        ddlMasterNumber.Items.Insert(0, new ListItem("-Select-", "0"));

        // For ItemColor
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "ItemColor";
        ddlColor.DataSource = sLookup.LookUp(sLookupBE);
        ddlColor.DataValueField = "iLookupID";
        ddlColor.DataTextField = "sLookupName";
        ddlColor.DataBind();
        ddlColor.Items.Insert(0, new ListItem("-Select-", "0"));

        // For SoldBy
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "SoldBy";
        ddlSoldBy.DataSource = sLookup.LookUp(sLookupBE);
        ddlSoldBy.DataValueField = "iLookupID";
        ddlSoldBy.DataTextField = "sLookupName";
        ddlSoldBy.DataBind();
        ddlSoldBy.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Status
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "Status";
        ddlItemNumberStatus.DataSource = sLookup.LookUp(sLookupBE);
        ddlItemNumberStatus.DataValueField = "iLookupID";
        ddlItemNumberStatus.DataTextField = "sLookupName";
        ddlItemNumberStatus.DataBind();
        ddlItemNumberStatus.Items.Insert(0, new ListItem("-Select-", "0"));

        // For ItemSize
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "ItemSize";
        ddlItemsize.DataSource = sLookup.LookUp(sLookupBE);
        ddlItemsize.DataValueField = "iLookupID";
        ddlItemsize.DataTextField = "sLookupName";
        ddlItemsize.DataBind();
        ddlItemsize.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Material
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "Material";
        ddlMaterialStyle.DataSource = sLookup.LookUp(sLookupBE);
        ddlMaterialStyle.DataValueField = "iLookupID";
        ddlMaterialStyle.DataTextField = "sLookupName";
        ddlMaterialStyle.DataBind();
        ddlMaterialStyle.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    /// <summary>
    /// Called Previous record on the basic of
    /// No of paging.
    /// Nagmani 08/10/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        bindGridView();
    }

    /// <summary>
    /// Called Next record on the basic of
    /// No of paging.
    /// Nagmani 08/10/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        bindGridView();
    }

    protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindGridView();
        }
    }

    /// <summary>
    /// lnkbtnPaging of paging button is enable false 
    /// Nagmani 08/10/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    /// <summary>
    /// This Gridveiw Event is used here for 
    /// Sorting the record on header click of
    /// griedview in asc and descding order.
    /// Nagmani 08/10/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvItemDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            if (this.ViewState["SortOrder"].ToString() == "ASC")
                ImgSort.ImageUrl = "";
            else
                ImgSort.ImageUrl = "";

            switch (this.ViewState["SortExp"].ToString())
            {
                case "ProductStyle":
                    PlaceHolder placeholderStyle = (PlaceHolder)e.Row.FindControl("placeholderStyle");
                    break;

                case "ItemNumber":
                    PlaceHolder placeholderItemNo = (PlaceHolder)e.Row.FindControl("placeholderItemNumber");
                    break;

                case "ItemColor":
                    PlaceHolder placeholderColor = (PlaceHolder)e.Row.FindControl("placeholderItemColor");
                    break;

                case "ItemSize":
                    PlaceHolder placeholderSize = (PlaceHolder)e.Row.FindControl("placeholderItemSize");
                    break;

                case "SizePriority":
                    PlaceHolder placeholderSizePriority = (PlaceHolder)e.Row.FindControl("placeholderSizePriority");
                    break;

                case "ItemImage":
                    PlaceHolder placeholderItemImage = (PlaceHolder)e.Row.FindControl("placeholderItemImage");
                    break;

                case "MasterStyleName":
                    PlaceHolder placeholderMasterStyleName = (PlaceHolder)e.Row.FindControl("placeholderMasterStyleName");
                    break;

                case "ItemNoStatus":
                    PlaceHolder placeholderItemNoStatus = (PlaceHolder)e.Row.FindControl("placeholderItemNoStatus");
                    break;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String path = "../../../admin/Incentex_Used_Icons/" + ((HiddenField)e.Row.FindControl("hdnLookupIcon")).Value;
            ((HtmlImage)e.Row.FindControl("imgLookupIcon")).Src = path;

            String ItemImagePath = "~/UploadedImages/ProductImages/Thumbs/" + ((HiddenField)e.Row.FindControl("hdnItemImage")).Value;
            Image imgItemImage = (Image)e.Row.FindControl("imgItemImage");

            String JavaScriptShow = "ShowImagePopup('" + ((HiddenField)e.Row.FindControl("hdnItemImage")).Value + "');";
            imgItemImage.Attributes.Add("onmouseover", JavaScriptShow);
            String JavaScriptClose = "CloseImagePopup();";
            imgItemImage.Attributes.Add("onmouseout", JavaScriptClose);
            imgItemImage.ImageUrl = ItemImagePath;
        }
    }

    /// <summary>
    /// In this event we Store the viewstate of
    /// Sortexpression in asc and descending on the
    /// basics of Commandargument name.
    /// Nagmani 08/10/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvItemDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
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

                bindGridView();
            }
            else if (e.CommandName == "Edit")  // or Edit
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                if (lblMessage.Text != "")
                    lblMessage.Text = "";

                LinkButton lbtnAction;
                Label lblProdItemID;
                GridViewRow row;
                row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
                lbtnAction = (LinkButton)(gvItemDetails.Rows[row.RowIndex].FindControl("hypStyle"));
                lblProdItemID = (Label)(gvItemDetails.Rows[row.RowIndex].FindControl("lblProductItemID"));
                HiddenField hdnItemNumber = (HiddenField)(gvItemDetails.Rows[row.RowIndex].FindControl("hdnItemNumber"));
                Session["iProductItmeId"] = lblProdItemID.Text;

                spanSaveitem.Visible = true;
                spanAddItem.Visible = false;
                lnkAddItem.Enabled = true;
            }
            else if (e.CommandName == "del")
            {
                if (!base.CanDelete)
                {
                    base.RedirectToUnauthorised();
                }

                objRepository.DeleteProductItem(Convert.ToInt64(e.CommandArgument.ToString()), IncentexGlobal.CurrentMember.UserInfoID);
                objRepository.SubmitChanges();
                bindGridView();
            }
            else if (e.CommandName == "StatusVhange")
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                HiddenField hdnItemNoStatusID;
                Label lblProductItemID;
                GridViewRow row;
                row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;

                lblProductItemID = (Label)(gvItemDetails.Rows[row.RowIndex].FindControl("lblProductItemID"));

                hdnItemNoStatusID = (HiddenField)(gvItemDetails.Rows[row.RowIndex].FindControl("hdnStatusID"));
                objRepository.UpdateStatus(Convert.ToInt32(lblProductItemID.Text), Convert.ToInt32(hdnItemNoStatusID.Value));
                objRepository.SubmitChanges();
                bindGridView();
            }
        }
        catch (SqlException ex)
        {
            if (ex.Number == 547)
                lblMessage.Text = "Unable to delete record as this record is used in other detail table";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// gvItemDetails_RowEditing()
    /// At this event Populate control is called
    /// to fill the controls in edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvItemDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        PopulateControl(Convert.ToInt32(Session["iProductItmeId"]));
        bindGridView();
    }

    protected void lnkbtnGoNextTab_Click(object sender, EventArgs e)
    {
        Response.Redirect("InventoryAndReOrder.aspx?SubId=" + Request.QueryString["SubId"] + "&Id=" + iStoreID);
    }

    protected void ddlStyle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStyle.SelectedItem.Text == "Name Bars")
        {
            trFontFormat.Visible = true;
            trNameFormat.Visible = true;
        }
        else
        {
            trFontFormat.Visible = false;
            trNameFormat.Visible = false;
        }
    }

    protected void btnSavePriority_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        foreach (GridViewRow gvItem in gvItemDetails.Rows)
        {
            CheckBox chkItem = (CheckBox)gvItem.FindControl("chkItem");
            TextBox txtSizePriority = (TextBox)gvItem.FindControl("txtSizePriority");
            Label lblProductItemID = (Label)gvItem.FindControl("lblProductItemID");
            if (chkItem != null)
            {
                if (chkItem.Checked)
                {
                    objRepository.UpdateProductItemsSizeOrderPriority(Convert.ToInt32(lblProductItemID.Text), Convert.ToInt32(txtSizePriority.Text));
                    chkItem.Checked = false;
                }
            }
        }

        bindGridView();
        lblMessage.Text = "Record Saved Successfully!";
    }

    #endregion

    #region Page Methods

    /// <summary>
    ///Save the Record.
    /// SaveData()
    /// Nagmani 07/10/2010
    /// </summary>
    public void SaveData()
    {
        String saveImagePath = String.Empty;
        String ImageFileName = String.Empty;

        #region Upload Image and Set path

        try
        {
            if (!String.IsNullOrEmpty(FileUploadItemimage.FileName))
            {
                String filePath = "~/UploadedImages/ProductImages/Thumbs/";
                HttpPostedFile Img = FileUploadItemimage.PostedFile;
                ImageFileName = "Thumb_" + System.DateTime.Now.Ticks + "_" + FileUploadItemimage.FileName;
                saveImagePath = filePath + ImageFileName;

                if (!String.IsNullOrEmpty(saveImagePath))
                    Img.SaveAs(Server.MapPath(saveImagePath));
            }

        }
        catch
        {

        }

        #endregion

        Int32 intMasterItemNo = 0;
        Int32 intStyleNumber = 0;
        List<SelectMasterItemStyleDuplicateResult> objMasterDuplicate = new List<SelectMasterItemStyleDuplicateResult>();

        try
        {
            //Insert into table ProductItem
            this.iProductItmeID = Convert.ToInt32(Session["iProductItmeId"]);

            if (this.iProductItmeID != 0)
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                objprodItem = objRepository.GetById(this.iProductItmeID);
            }
            else
            {
                if (!base.CanAdd)
                {
                    base.RedirectToUnauthorised();
                }
            }

            objprodItem.ProductStyleID = Convert.ToInt32(ddlStyle.SelectedValue);
            objprodItem.MasterStyleID = Convert.ToInt32(ddlMasterNumber.SelectedValue);
            objprodItem.ItemColorID = Convert.ToInt32(ddlColor.SelectedValue);

            if (ddlItemsize.SelectedValue != "0")
                objprodItem.ItemSizeID = Convert.ToInt32(ddlItemsize.SelectedValue);

            if (txtItemNumber.Text != "")
                objprodItem.ItemNumber = txtItemNumber.Text;
            else
                objprodItem.ItemNumber = null;

            // Save Size Priority
            if (txtSizePriority.Text != "")
                objprodItem.SizePriority = Convert.ToInt64(txtSizePriority.Text);
            else
                objprodItem.SizePriority = 0;

            objprodItem.ItemNumberStatusID = Convert.ToInt64(ddlItemNumberStatus.SelectedValue);

            if (txtMinimumOrderAmt.Text != "")
                objprodItem.MiniumOrderAmount = txtMinimumOrderAmt.Text;
            else
                objprodItem.MiniumOrderAmount = null;

            if (ddlSoldBy.SelectedValue == "0")
                objprodItem.Soldby = null;
            else
                objprodItem.Soldby = Convert.ToInt32(ddlSoldBy.SelectedValue);

            if (ddlMaterialStyle.SelectedValue != "0")
                objprodItem.MaterialStyleID = Convert.ToInt32(ddlMaterialStyle.SelectedValue);
            else
                objprodItem.MaterialStyleID = null;

            objprodItem.ProductId = StoreProductID;
            objprodItem.VenderItemNo = txtVenderItemNumber.Text;

            //added on 3-2-11 by Ankit
            if (ddlNameFormat.SelectedValue != "-select-")
                objprodItem.NameFormatForNameBars = ddlNameFormat.SelectedValue;

            if (ddlFontFormat.SelectedValue != "-select-")
                objprodItem.FontFormatForNameBars = ddlFontFormat.SelectedValue;
            //End Added

            // For Item Description and item Images
            if (txtItemPrdDescription.Text != "")
                objprodItem.ItemDescription = txtItemPrdDescription.Text;
            else
                objprodItem.ItemDescription = null;

            if (!String.IsNullOrEmpty(ImageFileName))
                objprodItem.ItemImage = ImageFileName;
            else if (!String.IsNullOrEmpty(hdnSubItemImage.Value))
                objprodItem.ItemImage = hdnSubItemImage.Value.ToString();
            else
                objprodItem.ItemImage = null;// if no image then default image "ProductDefault.jpg";
            //

            //AllowBackOrderID
            if (trallowforbackorder.Visible && ddlAllowbackOrders.SelectedIndex > 0)
                objprodItem.AllowBackOrderID = Convert.ToInt64(ddlAllowbackOrders.SelectedValue);   
            else
                objprodItem.AllowBackOrderID = null;   

            if (this.iProductItmeID == 0)
            {
                String modeAdd = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.Add);

                objMasterDuplicate = objRepository.CheckDuplicateMasterItem(Convert.ToInt32(StoreProductID), Convert.ToInt32(ddlMasterNumber.SelectedValue), 0, modeAdd);

                if (objMasterDuplicate.Count != 0 && objMasterDuplicate[0].MasterStyleID != null)
                    intMasterItemNo = Convert.ToInt32(objMasterDuplicate[0].MasterStyleID);

                if (objMasterDuplicate.Count == 0 || objMasterDuplicate[0].MasterStyleID == null || intMasterItemNo == Convert.ToInt32(ddlMasterNumber.SelectedValue))
                {
                    if (objMasterDuplicate.Count != 0 && objMasterDuplicate[0].ProductStyleID != null)
                        intStyleNumber = Convert.ToInt32(objMasterDuplicate[0].ProductStyleID);

                    if (objMasterDuplicate.Count == 0 || objMasterDuplicate[0].ProductStyleID == null || intStyleNumber == Convert.ToInt32(ddlStyle.SelectedValue))
                    {
                        iDuplicate = objRepository.CheckDuplicate(txtItemNumber.Text, Convert.ToInt32(Session["iProductItmeId"]), Convert.ToInt32(StoreProductID), Convert.ToInt32(ddlMasterNumber.SelectedValue), Convert.ToInt32(ddlStyle.SelectedValue), modeAdd);

                        if (iDuplicate == 0)
                        {
                            objRepository.Insert(objprodItem);
                            objRepository.SubmitChanges();
                            this.iProductItmeID = Convert.ToInt32(objprodItem.ProductItemID);
                            Session["ProductItemID"] = this.iProductItmeID;
                            lblMessage.Text = "Record Saved Successfully!";
                        }
                        else
                        {
                            bolflag = true;
                            lblMessage.Text = "This type of Item number combination already exist!";

                            return;
                        }
                    }
                    else
                    {
                        bolflag = true;
                        lblMessage.Text = "Style number will be same for one product!";

                        return;
                    }
                }
                else
                {
                    bolflagMaterItem = true;
                    lblMessage.Text = "Master item number will be same for one product!";
                    return;
                }
            }
            else
            {
                String modeEdit = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.Edit);
                objMasterDuplicate = objRepository.CheckDuplicateMasterItem(Convert.ToInt32(StoreProductID), Convert.ToInt32(ddlMasterNumber.SelectedValue), this.iProductItmeID, modeEdit);

                if (objMasterDuplicate.Count != 0 && objMasterDuplicate[0].MasterStyleID != null)
                    intMasterItemNo = Convert.ToInt32(objMasterDuplicate[0].MasterStyleID);

                if (objMasterDuplicate.Count == 0 || objMasterDuplicate[0].MasterStyleID == null || intMasterItemNo == Convert.ToInt32(ddlMasterNumber.SelectedValue))
                {
                    if (objMasterDuplicate.Count != 0 && objMasterDuplicate[0].ProductStyleID != null)
                        intStyleNumber = Convert.ToInt32(objMasterDuplicate[0].ProductStyleID);

                    if (objMasterDuplicate.Count == 0 || objMasterDuplicate[0].ProductStyleID == null || intStyleNumber == Convert.ToInt32(ddlStyle.SelectedValue))
                    {
                        iDuplicate = objRepository.CheckDuplicate(txtItemNumber.Text, Convert.ToInt32(Session["iProductItmeId"]), Convert.ToInt32(StoreProductID), Convert.ToInt32(ddlMasterNumber.SelectedValue), Convert.ToInt32(ddlStyle.SelectedValue), modeEdit);

                        if (iDuplicate == 0)
                        {
                            objRepository.SubmitChanges();

                            this.iProductItmeID = Convert.ToInt32(objprodItem.ProductItemID);
                            Session["ProductItemID"] = this.iProductItmeID;
                            lblMessage.Text = "Record Updated Successfully!";
                        }
                        else
                        {
                            bolflag = true;
                            lblMessage.Text = "This type of Item number combination already exist!";

                            return;
                        }
                    }
                    else
                    {
                        bolflagMaterItem = true;
                        lblMessage.Text = "Style number will be same for one product!";
                        return;
                    }
                }
                else
                {
                    bolflagMaterItem = true;
                    lblMessage.Text = "Master item number will be same for one product!";
                    return;
                }
            }

            bindGridView();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// ClearData()
    /// Method id used for Clear the control in new mode
    /// </summary>
    public void ClearData()
    {
        txtMinimumOrderAmt.Text = "";
        txtItemPrdDescription.Text = "";
        txtItemNumber.Text = "";
        txtVenderItemNumber.Text = "";
        Session["iProductItmeId"] = null;
        ddlColor.SelectedValue = "0";
        ddlMasterNumber.SelectedValue = "0";
        ddlStyle.SelectedValue = "0";
        ddlSoldBy.SelectedValue = "0";
        ddlItemsize.SelectedValue = "0";
        ddlMaterialStyle.SelectedValue = "0";
        ddlItemNumberStatus.SelectedValue = "0";
        ddlAllowbackOrders.SelectedValue = "0"; 
        hdnSubItemImage.Value = null;
    }

    /// <summary>
    /// PopulateControl()
    /// This method is called to 
    /// Populate the control in edit mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="iStoreProductID"></param>
    public void PopulateControl(Int32 ProductItemId)
    {
        try
        {
            if (ProductItemId != 0)
            {
                //Reterieve Data
                objprodItem = objRepository.GetById(ProductItemId);
                ddlMasterNumber.SelectedValue = objprodItem.MasterStyleID.ToString();
                ddlItemNumberStatus.SelectedValue = objprodItem.ItemNumberStatusID.ToString();
                ddlColor.SelectedValue = objprodItem.ItemColorID.ToString();
                ddlItemsize.SelectedValue = objprodItem.ItemSizeID.ToString();
                // set Size priority
                txtSizePriority.Text = objprodItem.SizePriority.ToString();
                //

                if (!String.IsNullOrEmpty(objprodItem.Soldby.ToString()))
                    ddlSoldBy.SelectedValue = objprodItem.Soldby.ToString();
                else
                    ddlSoldBy.SelectedValue = "0";

                if (objprodItem.ProductStyleID.ToString() != null)
                {
                    ddlStyle.SelectedValue = objprodItem.ProductStyleID.ToString();
                    OldProductStyleID = Convert.ToInt32(objprodItem.ProductStyleID);
                }

                // For Material / Style
                if (!String.IsNullOrEmpty(objprodItem.MaterialStyleID.ToString()))
                    ddlMaterialStyle.SelectedValue = objprodItem.MaterialStyleID.ToString();
                else
                    ddlMaterialStyle.SelectedValue = "0";

                // to set image if update is perform
                if (objprodItem.ItemImage != null && objprodItem.ItemImage != "0")
                    hdnSubItemImage.Value = objprodItem.ItemImage.ToString();
                else
                    hdnSubItemImage.Value = null;

                if (objprodItem.MiniumOrderAmount != null)
                    txtMinimumOrderAmt.Text = objprodItem.MiniumOrderAmount.ToString();

                if (objprodItem.ItemNumber != null)
                    txtItemNumber.Text = objprodItem.ItemNumber.ToString();
                
                if (objprodItem.VenderItemNo != null)
                    txtVenderItemNumber.Text = objprodItem.VenderItemNo;

                if (objprodItem.NameFormatForNameBars != null)
                    ddlNameFormat.SelectedValue = objprodItem.NameFormatForNameBars.ToString();

                if (objprodItem.FontFormatForNameBars != null)                
                    ddlFontFormat.SelectedValue = objprodItem.FontFormatForNameBars.ToString();

                // For Item Descriptions and Item Images
                if (objprodItem.ItemDescription != null)
                    txtItemPrdDescription.Text = objprodItem.ItemDescription.ToString();

                if (objprodItem.NameFormatForNameBars != null)
                    ddlNameFormat.SelectedValue = objprodItem.NameFormatForNameBars.ToString();

                if (objprodItem.FontFormatForNameBars != null)
                    ddlFontFormat.SelectedValue = objprodItem.FontFormatForNameBars.ToString();

                ddlAllowbackOrders.SelectedValue = objprodItem.AllowBackOrderID.HasValue ? objprodItem.AllowBackOrderID.Value.ToString() : "0";   

                if (objprodItem.ProductStyleID.ToString() == "258")
                {
                    trFontFormat.Visible = true;
                    trNameFormat.Visible = true;
                }
                else
                {
                    trFontFormat.Visible = false;
                    trNameFormat.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// bindGridView()
    /// This method is used forn binding the
    /// geidview from tabel storproduct.
    /// Nagmani 08/10/2010
    /// </summary>
    public void bindGridView()
    {
        try
        {
            if (StoreProductID != 0)
            {
                List<ProductItemDetailsRepository.ProductItemResult> oad = new ProductItemDetailsRepository().ProductItemDetails(Convert.ToInt32(StoreProductID));

                if (oad.Count == 0)
                    pagingtable.Visible = false;
                else
                    pagingtable.Visible = true;

                DataView myDataView = new DataView();
                DataTable dataTable = ListToDataTable(oad);
                myDataView = dataTable.DefaultView;

                if (this.ViewState["SortExp"] != null)
                    myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();

                pds.DataSource = myDataView;
                pds.AllowPaging = true;
                pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
                pds.CurrentPageIndex = CurrentPage;
                lnkbtnNext.Enabled = !pds.IsLastPage;
                lnkbtnPrevious.Enabled = !pds.IsFirstPage;
                gvItemDetails.DataSource = pds;
                gvItemDetails.DataBind();
                doPaging();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();

        foreach (var info in typeof(T).GetProperties())
            dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));

        foreach (var t in list)
        {
            var row = dt.NewRow();

            foreach (var info in typeof(T).GetProperties())
                row[info.Name] = info.GetValue(t, null);

            dt.Rows.Add(row);
        }

        return dt;
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

            DataList2.DataSource = dt;
            DataList2.DataBind();
        }
        catch
        { }
    }

    /// <summary>
    ///Fill the Item Fill Allow back Order dropdownlist
    ///from lookup table
    /// FillAllowbackOrders()
    /// </summary>
    public void FillAllowbackOrders()
    {
        try
        {
            String strStatus = "ItemsToBePolybagged";
            ddlAllowbackOrders.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlAllowbackOrders.DataValueField = "iLookupID";
            ddlAllowbackOrders.DataTextField = "sLookupName";
            ddlAllowbackOrders.DataBind();
            ddlAllowbackOrders.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}