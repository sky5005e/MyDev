using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_ProductStoreManagement_InventoryAndReOrder : PageBase
{
    #region Data Members

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
    
    Int32 iProductItmeIncentoryId;
    PagedDataSource pds = new PagedDataSource();
    ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
    LookupRepository objLookupRepos = new LookupRepository();
    ProductItemInventory objInventory = new ProductItemInventory();
    InventoryReorderRepository objInventoryRepos = new InventoryReorderRepository();
    List<UserInformation> objlist = new List<UserInformation>();
    UserInformationRepository objUInfoRepos = new UserInformationRepository();

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
                iStoreID = Convert.ToInt64(Request.QueryString["Id"].ToString());

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Inventory & Re-Order" + WorkGroupNameToDisplay;
            ((HtmlGenericControl)menuControl.FindControl("dvSubMenu")).Visible = false;
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.ProductReturnURL;

            FillMasterItemNumber();

            if (Request.QueryString["SubId"] != null && Request.QueryString["SubId"] != "0" && Request.QueryString["SubId"] != "")
            {
                iProductId = Convert.ToInt32(Request.QueryString["SubId"].ToString());
                menuControl.PopulateMenu(2, 0, Convert.ToInt64(iProductId), iStoreID, false);
                bindGridView();
            }
            else
                Response.Redirect("General.aspx?SubId=" + Request.QueryString["SubId"] + "&Id=" + Request.QueryString["Id"]);
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
    protected void gvInventroyOrder_RowCommand(object sender, GridViewCommandEventArgs e)
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
                Label lblProdItemInventoryID;
                Label lblProductItemId;
                GridViewRow row;
                row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
                lbtnAction = (LinkButton)(gvInventroyOrder.Rows[row.RowIndex].FindControl("hypStyle"));
                lblProdItemInventoryID = (Label)(gvInventroyOrder.Rows[row.RowIndex].FindControl("lblProductItemInventoryID"));
                lblProductItemId = (Label)(gvInventroyOrder.Rows[row.RowIndex].FindControl("lblProductItemID"));
                Session["iProductItmeIncentoryId"] = lblProdItemInventoryID.Text;
                Session["lblProductItemId"] = lblProductItemId.Text;
            }
            else if (e.CommandName == "del")
            {
                if (!base.CanDelete)
                {
                    base.RedirectToUnauthorised();
                }

                objInventoryRepos.DeleteProductItemInventory(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);
                objInventoryRepos.SubmitChanges();
                bindGridView();
            }
        }
        catch (SqlException ex)
        {
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

    /// <summary>
    /// This Gridveiw Event is used here for 
    /// Sorting the record on header click of
    /// griedview in asc and descding order.
    /// Nagmani 08/10/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvInventroyOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                DropDownList ddlItemNumber = (DropDownList)e.Row.FindControl("ddlItemNumber");
                HiddenField hdnMasterStyleID = (HiddenField)e.Row.FindControl("hdnMasterStyleID");
                Label lblColor = (Label)e.Row.FindControl("lblItemColor");
                List<ProductItem> objlist = new List<ProductItem>();
                objlist = objRepository.GetAllProductItem(Convert.ToInt32(hdnMasterStyleID.Value));

                if (objlist.Count != 0)
                {
                    ddlItemNumber.DataSource = objlist;
                    ddlItemNumber.DataValueField = "ItemNumber";
                    ddlItemNumber.DataTextField = "ItemNumber";
                    ddlItemNumber.DataBind();
                    ddlItemNumber.Items.Insert(0, new ListItem("-select-", "0"));
                }

                HiddenField hdnItemNumber = (HiddenField)e.Row.FindControl("hdnItemNumber");

                if (!(String.IsNullOrEmpty(hdnItemNumber.Value)))
                    ddlItemNumber.SelectedIndex = ddlItemNumber.Items.IndexOf(ddlItemNumber.Items.FindByText(hdnItemNumber.Value));
                else
                    ddlItemNumber.SelectedIndex = 0;

                ((Image)e.Row.FindControl("imgColor")).ImageUrl = "~/admin/Incentex_Used_Icons/" + new LookupRepository().GetIconByLookupName(lblColor.Text);

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
        }

        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            switch (this.ViewState["SortExp"].ToString())
            {
                case "Inventory":
                    PlaceHolder PlaceHolderInventory = (PlaceHolder)e.Row.FindControl("placeholderInventory");
                    break;

                case "ReOrderPoint":
                    PlaceHolder placeholderReOrderPoint = (PlaceHolder)e.Row.FindControl("placeholderReOrderPoint");
                    break;

                case "OnOrder":
                    PlaceHolder placeholderOnOrder = (PlaceHolder)e.Row.FindControl("placeholderOnOrder");
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

                case "InventoryArriveOn":
                    PlaceHolder InventoryArriveOn = (PlaceHolder)e.Row.FindControl("InventoryArriveOn");
                    break;
            }
        }
    }

    protected void gvInventroyOrder_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblmsg.Text = "";
        PopulateControl(Convert.ToInt32(Session["iProductItmeIncentoryId"]), Convert.ToInt32(Session["lblProductItemId"]));
        bindGridView();
    }

    protected void ddlItemNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ProductItemDetailsRepository.ProductItemResult oad = new ProductItemDetailsRepository().GetProductItemDetails(Convert.ToInt32(ddlMasterItemNo.SelectedValue), ddlItemNumber.SelectedItem.Text);
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

    protected void ddlMasterItemNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMasterItemNo.SelectedIndex > 0)
            FillItemNumber(Convert.ToInt32(ddlMasterItemNo.SelectedValue));
    }

    protected void ddlItemNumber_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow t in gvInventroyOrder.Rows)
            {
                HiddenField hdnMasterStyleID = (HiddenField)t.FindControl("hdnMasterStyleID");
                DropDownList ddlItemNumber = (DropDownList)t.FindControl("ddlItemNumber");
                Label lblItemColor = (Label)t.FindControl("lblItemColor");
                Label lblItemSize = (Label)t.FindControl("lblItemSize");
                ProductItemDetailsRepository.ProductItemResult oad = new ProductItemDetailsRepository().GetProductItemDetails(Convert.ToInt32(hdnMasterStyleID.Value), ddlItemNumber.SelectedItem.Text);

                if (oad != null)
                {
                    lblItemColor.Text = oad.ItemColor.ToString();
                    lblItemSize.Text = oad.ItemSize.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
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

    protected void lnkEmailNotification_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(txtInventory.Text) <= 10)
        {
            objlist = objUInfoRepos.GetEmailInformation();
            if (objlist.Count > 0)
                for (Int32 i = 0; i < objlist.Count; i++)
                    sendVerificationEmail(objlist[i].UserInfoID, objlist[i].LoginEmail);
        }
    }

    protected void lnkbtnGoNextTab_Click(object sender, EventArgs e)
    {
        Response.Redirect("Pricing.aspx?SubId=" + iProductId + "&Id=" + iStoreID);
    }

    protected void btnSaveStatus_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        foreach (GridViewRow t in gvInventroyOrder.Rows)
        {
            CheckBox chkDelete = (CheckBox)t.FindControl("CheckBox1");
            TextBox txtReOrderPointgrid = (TextBox)t.FindControl("txtReOrderPointgrid");
            TextBox txtOnOrdergrid = (TextBox)t.FindControl("txtOnOrderGrid");
            TextBox txtInventorygrid = (TextBox)t.FindControl("txtInventorygrid");
            Label lblProductItemInventoryID = (Label)t.FindControl("lblProductItemInventoryID");
            DropDownList ddlItemNumber = (DropDownList)t.FindControl("ddlItemNumber");
            Label lblItemColor = (Label)t.FindControl("lblItemColor");
            Label lblItemSize = (Label)t.FindControl("lblItemSize");
            Label lblProductItemID = (Label)t.FindControl("lblProductItemID");

            if (chkDelete != null)
            {
                if (chkDelete.Checked)
                {
                    if (lblProductItemInventoryID.Text != "0")
                        objInventoryRepos.UpdateTable(Convert.ToInt32(txtInventorygrid.Text), Convert.ToInt32(txtReOrderPointgrid.Text), Convert.ToInt64(lblProductItemInventoryID.Text), Convert.ToInt32(txtOnOrdergrid.Text));
                    else
                        objInventoryRepos.InsertTable(Convert.ToInt32(txtInventorygrid.Text), Convert.ToInt32(txtReOrderPointgrid.Text), Convert.ToInt64(lblProductItemID.Text), Convert.ToInt32(txtOnOrdergrid.Text));

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
            if (objlist.Count != 0)
            {
                ddlItemNumber.DataSource = objlist;
                ddlItemNumber.DataValueField = "ItemNumber";
                ddlItemNumber.DataTextField = "ItemNumber";
                ddlItemNumber.DataBind();
                ddlItemNumber.Items.Insert(0, new ListItem("-select-", "0"));
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
    /// <param name="iStoreProductID"></param>
    public void PopulateControl(Int32 ProductItemInventoryId, Int32 ProductItemID)
    {
        try
        {
            //Reterieve Data from ProductItemInventory table here
            InventoryReorderRepository.ProductItemInventoryResult oad = new InventoryReorderRepository().GetSingleProductItemInventory(ProductItemInventoryId, ProductItemID);
            if (oad != null)
            {
                pnlAddItem.Visible = true;
                lblStyle.Text = oad.Style.ToString();
                txtPrdDescription.Text = objInventoryRepos.GetProductDecription(this.iProductId);
                if (oad.Inventory.ToString() == "0")
                    txtInventory.Text = "";
                else
                    txtInventory.Text = oad.Inventory.ToString();
                if (oad.ReOrderPoint.ToString() == "0")
                    txtReOrderpoint.Text = "";
                else
                    txtReOrderpoint.Text = oad.ReOrderPoint.ToString();
                if (oad.OnOrder.ToString() == "0")
                    txtOnOrder.Text = "";
                else
                    txtOnOrder.Text = oad.OnOrder.ToString();
                FillMasterItemNumber();
                ddlMasterItemNo.SelectedValue = oad.MasterStyleID.ToString();
                ddlMasterItemNo.Enabled = false;
                FillItemNumber(Convert.ToInt32(ddlMasterItemNo.SelectedValue));
                ddlItemNumber.SelectedValue = oad.ItemNumber.ToString();
                lblItemColor.Text = oad.ItemColor.ToString();
                lblSize.Text = oad.ItemSize.ToString();
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
            this.iProductItmeIncentoryId = Convert.ToInt32(Session["iProductItmeIncentoryId"]);
            if (this.iProductItmeIncentoryId != 0)
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                objInventory = objInventoryRepos.GetById(this.iProductItmeIncentoryId);
            }
            else
            {
                if (!base.CanAdd)
                {
                    base.RedirectToUnauthorised();
                }
            }

            objInventory.ProductItemID = Convert.ToInt32(Session["lblProductItemId"]);

            if (txtReOrderpoint.Text != "")
                objInventory.ReOrderPoint = Convert.ToInt32(txtReOrderpoint.Text);
            else
                objInventory.ReOrderPoint = 0;

            if (txtInventory.Text != "")
                objInventory.Inventory = Convert.ToInt32(txtInventory.Text);
            else
                objInventory.Inventory = 0;

            if (txtOnOrder.Text != "")
                objInventory.OnOrder = Convert.ToInt32(txtOnOrder.Text);
            else
                objInventory.OnOrder = 0;

            if (this.iProductItmeIncentoryId == 0 && Session["iProductItmeIncentoryId"].ToString() == "0")
            {
                objInventoryRepos.Insert(objInventory);
                objInventoryRepos.SubmitChanges();
                this.iProductItmeIncentoryId = Convert.ToInt32(objInventory.ProductItemInventoryID);
                Session["iProductItmeIncentoryId"] = this.iProductItmeIncentoryId;
                bolflag = true;
            }
            else
            {   
                objInventoryRepos.SubmitChanges();
                this.iProductItmeIncentoryId = Convert.ToInt32(objInventory.ProductItemInventoryID);
                Session["iProductItmeIncentoryId"] = this.iProductItmeIncentoryId;
                bolflag = true;
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
            if (iProductId != 0)
            {
                List<InventoryReorderRepository.ProductItemInventoryResult> oad = new InventoryReorderRepository().ProductItemInventoryDetails(iProductId);

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
                gvInventroyOrder.DataSource = pds;
                gvInventroyOrder.DataBind();
                doPaging();
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

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

    private void sendVerificationEmail(Int64 piUserInfoID, String psLoginEmail)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Inventory Re-Order";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sToadd = psLoginEmail;
                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{itemNo}", ddlItemNumber.SelectedItem.Text);
                messagebody.Replace("{ProductDescription}", txtPrdDescription.Text);
                messagebody.Replace("{ReOrderPoint}", txtReOrderpoint.Text);
                messagebody.Replace("{CurrentInventory}", txtInventory.Text);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                //General.SendMail(sFrmadd, sToadd, sSubject, messagebody.ToString(), smtphost, smtpport, smtpUserID, smtppassword, sFrmname, true, true);
                new CommonMails().SendMail(piUserInfoID, "Inventory & Re-Order Product", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}