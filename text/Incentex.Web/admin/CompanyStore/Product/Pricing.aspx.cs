using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_ProductStoreManagement_StorePricing : PageBase
{
    #region Data Members
    Int64 iStoreId
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
    Boolean bolflag = false;
    Int32 iProductId
    {
        get
        {
            if (ViewState["iProductId"] == null)
            {
                ViewState["iProductId"] = 0;
            }
            return Convert.ToInt32(ViewState["iProductId"]);
        }
        set
        {
            ViewState["iProductId"] = value;
        }
    }
    Int32 iProductItmePriceId;
    PagedDataSource pds = new PagedDataSource();
    ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
    ProductItem objprodItem = new ProductItem();
    LookupRepository objLookupRepos = new LookupRepository();
    ProductItemPriceRepository objPricingRepo = new ProductItemPriceRepository();
    ProductItemPricing objprice = new ProductItemPricing();
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
    #endregion

    #region Event Handlers
    
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

            pnlAddItem.Visible = false;

            if (Request.QueryString["Id"] != null)
                iStoreId = Convert.ToInt64(Request.QueryString["Id"].ToString());

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Pricing" + WorkGroupNameToDisplay;
            ((HtmlGenericControl)manuControl.FindControl("dvSubMenu")).Visible = false;
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.ProductReturnURL;
            if (Request.QueryString["SubId"] != null && Request.QueryString["SubId"] != "0" && Request.QueryString["SubId"] != "")
            {
                iProductId = Convert.ToInt32(Request.QueryString["SubId"]);
                manuControl.PopulateMenu(3, 0, Convert.ToInt64(iProductId), iStoreId, false);
                bindGridView();
            }
            else
            {
                Response.Redirect("General.aspx?SubId=" + Request.QueryString["SubId"] + "&Id=" + Request.QueryString["Id"]);
            }

            FillMasterItemNumber();
            FillLevel3StatusDropdown();
            FillLevel3OrderRuleDropdown();
            FillLevelThreeOnSaleFlagDropdown();
            FillLevelthreePreSeasonPurchaseDropdown();
            FillPricefor();
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
    protected void gvPricing_RowCommand(object sender, GridViewCommandEventArgs e)
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

                LinkButton lbtnAction;
                Label lblProductItemPricingID;
                Label lblProductItemId;
                GridViewRow row;
                row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
                lbtnAction = (LinkButton)(gvPricing.Rows[row.RowIndex].FindControl("hypStyle"));
                lblProductItemPricingID = (Label)(gvPricing.Rows[row.RowIndex].FindControl("lblProductItemPricingID"));
                lblProductItemId = (Label)(gvPricing.Rows[row.RowIndex].FindControl("lblProductItemID"));
                Session["iProductItmePriceId"] = lblProductItemPricingID.Text;
                Session["lblProductItemId"] = lblProductItemId.Text;

            }
            else if (e.CommandName == "del")
            {
                if (!base.CanDelete)
                {
                    base.RedirectToUnauthorised();
                }

                objPricingRepo.DeleteProductItemPricing(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);
                objPricingRepo.SubmitChanges();
                bindGridView();
            }
        }
        catch (SqlException ex)
        {
            ErrHandler.WriteError(ex);

            if (ex.Number == 547)
            {
                lblmsg.Text = "Unable to delete record as this record is used in other detail table";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);            
        }
    }
    
    protected void gvPricing_RowDataBound(object sender, GridViewRowEventArgs e)
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
                case "Level1":
                    PlaceHolder placeholderLevel1 = (PlaceHolder)e.Row.FindControl("placeholderLevel1");
                    break;

                case "ProductCost":
                    PlaceHolder placeholderProductCost = (PlaceHolder)e.Row.FindControl("placeholderProductCost");
                    break;

                case "Level2":
                    PlaceHolder placeholderLevel2 = (PlaceHolder)e.Row.FindControl("placeholderLevel2");
                    break;

                case "Level3":
                    PlaceHolder placeholderLevel3 = (PlaceHolder)e.Row.FindControl("placeholderLevel3");
                    break;

                case "Level4":
                    PlaceHolder placeholderLevel4 = (PlaceHolder)e.Row.FindControl("placeholderLevel4");
                    break;

                case "CloseOut":
                    PlaceHolder placeholderCloseOut = (PlaceHolder)e.Row.FindControl("placeholderCloseOut");
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

                case "MasterStyleName":
                    PlaceHolder placeholderMasterStyleName = (PlaceHolder)e.Row.FindControl("placeholderMasterStyleName");
                    break;

                case "ProductstyleName":
                    PlaceHolder placeholderStyle = (PlaceHolder)e.Row.FindControl("placeholderStyle");
                    break;
            }
        }
    }
    
    protected void gvPricing_RowEditing(object sender, GridViewEditEventArgs e)
    {
        PopulateControl(Convert.ToInt32(Session["iProductItmePriceId"]), Convert.ToInt32(Session["lblProductItemId"]));
        bindGridView();
    }
    
    protected void ddlMasterItemNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMasterItemNo.SelectedIndex > 0)
            FillItemNumber(Convert.ToInt32(ddlMasterItemNo.SelectedValue));
    }
    
    protected void lnkAddItem_Click(object sender, EventArgs e)
    {
        try
        {
            SaveData();
            bindGridView();

            if (bolflag)
                lblMessage.Text = "Record Saved Successfully!";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    
    protected void ddlItemNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ProductItemDetailsRepository.ProductItemResult oad = new ProductItemDetailsRepository().GetProductItemDetails(Convert.ToInt32(ddlMasterItemNo.SelectedValue), ddlItemNo.SelectedItem.Text);
            if (oad != null)
            {

                lblItemColor.Text = oad.ItemColor.ToString();
                lblSize.Text = oad.ItemSize.ToString();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    
    protected void lnkbtnGoNextTab_Click(object sender, EventArgs e)
    {
        Response.Redirect("TailoringAndMeasurement.aspx?SubId=" + Request.QueryString["SubId"] + "&Id=" + iStoreId);
    }
    
    protected void btnSaveStatus_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        foreach (GridViewRow t in gvPricing.Rows)
        {
            CheckBox chkDelete = (CheckBox)t.FindControl("CheckBox1");
            TextBox txtLevel1grid = (TextBox)t.FindControl("txtLevel1grid");
            TextBox txtLevel2grid = (TextBox)t.FindControl("txtLevel2grid");
            TextBox txtLevel3grid = (TextBox)t.FindControl("txtLevel3grid");
            TextBox txtLevel4grid = (TextBox)t.FindControl("txtLevel4grid");
            TextBox txtCloseOutPricegrid = (TextBox)t.FindControl("txtCloseOutPricegrid");
            Label lblProductItemPricingID = (Label)t.FindControl("lblProductItemPricingID");
            TextBox txtProductCost = (TextBox)t.FindControl("txtProductCost");
            Label lblProductItemID = (Label)t.FindControl("lblProductItemID");

            if (chkDelete != null)
            {
                if (chkDelete.Checked)
                {
                    if (lblProductItemPricingID.Text != "0")
                    {
                        objPricingRepo.UpdateTable(Convert.ToDecimal(txtLevel1grid.Text), Convert.ToDecimal(txtLevel2grid.Text), Convert.ToDecimal(txtLevel3grid.Text), Convert.ToDecimal(txtLevel4grid.Text), Convert.ToDecimal(txtCloseOutPricegrid.Text), Convert.ToDecimal(txtProductCost.Text), Convert.ToInt64(lblProductItemPricingID.Text));
                    }
                    else
                    {
                        objPricingRepo.InsertDate(Convert.ToDecimal(txtLevel1grid.Text), Convert.ToDecimal(txtLevel2grid.Text), Convert.ToDecimal(txtLevel3grid.Text), Convert.ToDecimal(txtLevel4grid.Text), Convert.ToDecimal(txtCloseOutPricegrid.Text), Convert.ToDecimal(txtProductCost.Text), Convert.ToInt64(lblProductItemID.Text));
                    }

                    chkDelete.Checked = false;
                }
            }
        }
        bindGridView();
        lblmsg.Text = "Record Saved Successfully!";
    }
    
    #endregion

    #region Methods
    
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

            if (iProductId != 0)
            {
                List<ProductItemPriceRepository.ProductItemPricingResult> oad = new ProductItemPriceRepository().ProductItemPricingDetails(iProductId);

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
                gvPricing.DataSource = pds;
                gvPricing.DataBind();
                doPaging();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// PopulateControl()
    /// This method is called to 
    /// Populate the control in edit mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ProductItemPriceId"></param>
    /// <param name="ProductItemID"></param>
    public void PopulateControl(Int32 ProductItemPriceId, Int32 ProductItemID)
    {
        try
        {
            //Reterieve Data from ProductItemInventory table here
            ProductItemPriceRepository.ProductItemPricingResult oad = new ProductItemPriceRepository().GetSingleProductItemPrice(ProductItemPriceId, ProductItemID);

            if (oad != null)
            {
                lblStyle.Text = oad.Style.ToString();
             
                pnlAddItem.Visible = true;
                txtPrdDescription.Text = objPricingRepo.GetProductDecription(iProductId);
                if (oad.Level1.ToString() == "0")
                    txtLevelOne.Text = "";
                else
                    txtLevelOne.Text = oad.Level1.ToString("0.00");

                if (oad.Level2.ToString() == "0")
                    txtLeveltwo.Text = "";
                else
                    txtLeveltwo.Text = oad.Level2.ToString("0.00");

                if (oad.Level3.ToString() == "0")
                    txtLevelthree.Text = "";
                else
                    txtLevelthree.Text = oad.Level3.ToString("0.00");

                if (oad.Level4.ToString() == "0")
                    txtLevelfour.Text = "";
                else
                    txtLevelfour.Text = oad.Level4.ToString("0.00");

                if (oad.CloseOutPrice.ToString() == "0")
                    txtCloseOutPrice.Text = "";
                else
                    txtCloseOutPrice.Text = oad.CloseOutPrice.ToString("0.00");

                FillLevel3OrderRuleDropdown();
                ddlLevethreeOrderRule.SelectedValue = oad.Level3OrderingRuleID.ToString();
                FillLevel3StatusDropdown();
                ddlLevelThreePricStatus.SelectedValue = oad.Level3PricingStatus.ToString();
                FillLevelThreeOnSaleFlagDropdown();
                ddlLevelthreeSaleFlag.SelectedValue = oad.Level3OnSaleFlagID.ToString();
                FillLevelthreePreSeasonPurchaseDropdown();
                ddlLevel3PreSeasonPurchase.SelectedValue = oad.Level3PreSeasonPurchase.ToString();

                if (oad.Level3PricingStartDate != null)
                    txtPriceStartDate.Text = Convert.ToString(oad.Level3PricingStartDate.ToShortDateString());

                if (oad.Level3PricingEndDate != null)
                    txtPricingEndDate.Text = Convert.ToString(oad.Level3PricingEndDate.ToShortDateString());

                if (oad.ProductCost.ToString() == "0")
                    txtProductCost.Text = "";
                else
                    txtProductCost.Text = oad.ProductCost.ToString("0.00");

                FillMasterItemNumber();
                ddlMasterItemNo.SelectedValue = oad.MasterStyleID.ToString();
                ddlMasterItemNo.Enabled = false;
                FillItemNumber(Convert.ToInt32(ddlMasterItemNo.SelectedValue));
                ddlItemNo.SelectedValue = oad.ItemNumber.ToString();
                lblItemColor.Text = oad.ItemColor.ToString();
                lblSize.Text = oad.ItemSize.ToString();
                FillPricefor();
                ddlPricefor.SelectedValue = oad.Pricefor.ToString();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    ///Save the Record.
    /// SaveData()
    /// Nagmani 07/10/2010
    /// </summary>
    public void SaveData()
    {
        try
        {
            //Insert into table ProductItemInventroy
            this.iProductItmePriceId = Convert.ToInt32(Session["iProductItmePriceId"]);

            if (this.iProductItmePriceId != 0)
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                objprice = objPricingRepo.GetById(this.iProductItmePriceId);
            }
            else
            {
                if (!base.CanAdd)
                {
                    base.RedirectToUnauthorised();
                }
            }

            objprice.ProductItemID = Convert.ToInt32(Session["lblProductItemId"].ToString());

            if (txtLevelOne.Text != "")
                objprice.Level1 = Convert.ToDecimal(txtLevelOne.Text);
            else
                objprice.Level1 = 0;

            if (txtLeveltwo.Text != "")
                objprice.Level2 = Convert.ToDecimal(txtLeveltwo.Text);
            else
                objprice.Level2 = 0;

            if (txtLevelthree.Text != "")
                objprice.Level3 = Convert.ToDecimal(txtLevelthree.Text);
            else
                objprice.Level3 = 0;

            if (txtLevelfour.Text != "")
                objprice.Level4 = Convert.ToDecimal(txtLevelfour.Text);
            else
                objprice.Level4 = 0;

            if (txtCloseOutPrice.Text != "")
                objprice.CloseOutPrice = Convert.ToDecimal(txtCloseOutPrice.Text);
            else
                objprice.CloseOutPrice = 0;

            if (txtPriceStartDate.Text != "")
                objprice.Level3PricingStartDate = Convert.ToDateTime(txtPriceStartDate.Text);
            else
                objprice.Level3PricingStartDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            if (txtPricingEndDate.Text != "")
                objprice.Level3PricingEndDate = Convert.ToDateTime(txtPricingEndDate.Text);
            else
                objprice.Level3PricingEndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            if (ddlLevelThreePricStatus.SelectedIndex > 0)
                objprice.Level3PricingStatus = Convert.ToInt32(ddlLevelThreePricStatus.SelectedValue);
            else
                objprice.Level3PricingStatus = null;

            if (ddlLevethreeOrderRule.SelectedIndex > 0)
                objprice.Level3OrderingRuleID = Convert.ToInt32(ddlLevethreeOrderRule.SelectedValue);
            else
                objprice.Level3OrderingRuleID = null;

            if (ddlLevelthreeSaleFlag.SelectedIndex > 0)
                objprice.Level3OnSaleFlagID = Convert.ToInt32(ddlLevelthreeSaleFlag.SelectedValue);
            else
                objprice.Level3OnSaleFlagID = null;

            if (ddlLevel3PreSeasonPurchase.SelectedIndex > 0)
                objprice.Level3PreSeasonPurchase = Convert.ToInt32(ddlLevel3PreSeasonPurchase.SelectedValue);
            else
                objprice.Level3PreSeasonPurchase = null;

            if (txtProductCost.Text != "")
                objprice.ProductCost = Convert.ToDecimal(txtProductCost.Text);
            else
                objprice.ProductCost = 0;

            if (ddlPricefor.SelectedIndex > 0)
                objprice.Pricefor = Convert.ToInt32(ddlPricefor.SelectedValue);
            else
                objprice.Pricefor = 0;

            if (this.iProductItmePriceId == 0 && (Session["iProductItmePriceId"].ToString()) == "0")
            {
                objPricingRepo.Insert(objprice);
                objPricingRepo.SubmitChanges();
                this.iProductItmePriceId = Convert.ToInt32(objprice.ProductItemPricingID);
                Session["iProductItmePriceId"] = this.iProductItmePriceId;
                bolflag = true;
            }
            else
            {
                
                objPricingRepo.SubmitChanges();
                this.iProductItmePriceId = Convert.ToInt32(objprice.ProductItemPricingID);
                Session["iProductItmePriceId"] = this.iProductItmePriceId;
                bolflag = true;

                //Start Nagmani 25-04-2011 
                objprodItem = objRepository.GetById(Convert.ToInt32(Session["lblProductItemId"].ToString()));
                objPricingRepo.UpdatePricelevel(objprodItem.ProductId, Convert.ToDecimal(txtLevelOne.Text), Convert.ToDecimal(txtLeveltwo.Text), Convert.ToDecimal(txtLevelthree.Text), Convert.ToDecimal(txtLevelfour.Text), Convert.ToDecimal(txtCloseOutPrice.Text));
                objPricingRepo.UpdatePricelevelShopping(objprodItem.ProductId, Convert.ToDecimal(txtLevelOne.Text), Convert.ToDecimal(txtLeveltwo.Text), Convert.ToDecimal(txtLevelthree.Text), Convert.ToDecimal(txtLevelfour.Text), Convert.ToDecimal(txtCloseOutPrice.Text));
                //End Nagmani 25-04-2011 
                
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    ///Fill the Master Item Number dropdownlist
    ///from lookup table
    ///FillMasterItemNumber()
    /// Nagmani 11/10/2010
    /// </summary>
    public void FillMasterItemNumber()
    {
        try
        {
            String strStatus = "MasterItemNumber";
            ddlMasterItemNo.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlMasterItemNo.DataValueField = "iLookupID";
            ddlMasterItemNo.DataTextField = "sLookupName";
            ddlMasterItemNo.DataBind();
            ddlMasterItemNo.Items.Insert(0, new ListItem("-select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    ///FillItemNumber()
    ///Fill the ItemNumber dropdownlist]
    ///from the table ProductItem
    /// Nagmani 11/10/2010
    /// </summary>
    private void FillItemNumber(Int32 iMasterItemNo)
    {
        try
        {
            List<ProductItem> objlist = new List<ProductItem>();
            objlist = objRepository.GetAllProductItem(iMasterItemNo);
            if (objlist != null)
            {
                ddlItemNo.DataSource = objlist;
                ddlItemNo.DataValueField = "ItemNumber";
                ddlItemNo.DataTextField = "ItemNumber";
                ddlItemNo.DataBind();
                ddlItemNo.Items.Insert(0, new ListItem("-select-", "0"));

            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    
    private void FillLevel3StatusDropdown()
    {
        try
        {
            String strStatus = "Status";
            ddlLevelThreePricStatus.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlLevelThreePricStatus.DataValueField = "iLookupID";
            ddlLevelThreePricStatus.DataTextField = "sLookupName";
            ddlLevelThreePricStatus.DataBind();
            ddlLevelThreePricStatus.Items.Insert(0, new ListItem("-select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    
    private void FillLevel3OrderRuleDropdown()
    {
        try
        {
            String strStatus = "Level Three Ordering Rule";
            ddlLevethreeOrderRule.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlLevethreeOrderRule.DataValueField = "iLookupID";
            ddlLevethreeOrderRule.DataTextField = "sLookupName";
            ddlLevethreeOrderRule.DataBind();
            ddlLevethreeOrderRule.Items.Insert(0, new ListItem("-select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    
    private void FillLevelThreeOnSaleFlagDropdown()
    {
        try
        {
            String strStatus = "ItemsToBePolybagged ";
            ddlLevelthreeSaleFlag.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlLevelthreeSaleFlag.DataValueField = "iLookupID";
            ddlLevelthreeSaleFlag.DataTextField = "sLookupName";
            ddlLevelthreeSaleFlag.DataBind();
            ddlLevelthreeSaleFlag.Items.Insert(0, new ListItem("-select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    
    private void FillLevelthreePreSeasonPurchaseDropdown()
    {
        try
        {
            String strStatus = "ItemsToBePolybagged ";
            ddlLevel3PreSeasonPurchase.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlLevel3PreSeasonPurchase.DataValueField = "iLookupID";
            ddlLevel3PreSeasonPurchase.DataTextField = "sLookupName";
            ddlLevel3PreSeasonPurchase.DataBind();
            ddlLevel3PreSeasonPurchase.Items.Insert(0, new ListItem("-select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    ///Fill the Pricefor dropdownlist
    ///from lookup table
    ///FillPricefor()
    /// Nagmani 11/10/2010
    /// </summary>
    public void FillPricefor()
    {
        try
        {
            String strStatus = "Pricefor";
            ddlPricefor.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlPricefor.DataValueField = "iLookupID";
            ddlPricefor.DataTextField = "sLookupName";
            ddlPricefor.DataBind();
            ddlPricefor.Items.Insert(0, new ListItem("-select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Pagging
    
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
    
    protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindGridView();
        }
    }
    
    #endregion
}