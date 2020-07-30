using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_ProductStoreManagement_AddProductView : PageBase
{
    #region Property

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

    private Int32 PagerSize
    {
        get
        {
            if (this.ViewState["PagerSize"] == null)
                return Convert.ToInt32(Application["GRIDPAGERSIZE"]);
            else
                return Convert.ToInt16(this.ViewState["PagerSize"]);
        }
    }

    private Dictionary<Int64, Int32> CurrentPage
    {
        get
        {
            if (Convert.ToString(ViewState["CurrentPage"]) == String.Empty)
                return null;
            else
                return (Dictionary<Int64, Int32>)this.ViewState["CurrentPage"];
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }

    private Dictionary<Int64, Int32> FrmPg
    {
        get
        {
            if (Convert.ToString(ViewState["FrmPg"]) == String.Empty)
                return null;
            else
                return (Dictionary<Int64, Int32>)this.ViewState["FrmPg"];
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }

    private Dictionary<Int64, Int32> ToPg
    {
        get
        {
            if (Convert.ToString(ViewState["ToPg"]) == String.Empty)
                return null;
            else
                return (Dictionary<Int64, Int32>)this.ViewState["ToPg"];
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }

    Boolean IsChecked = false;

    private Dictionary<Int64, PagedDataSource> pds
    {
        get
        {
            if (Convert.ToString(Session["pds"]) == String.Empty)
                return null;
            else
                return (Dictionary<Int64, PagedDataSource>)Session["pds"];
        }
        set
        {
            Session["pds"] = value;
        }
    }

    StoreProduct objStoreProd = new StoreProduct();
    StoreProductRepository objStoreProdRepository = new StoreProductRepository();
    CompanyStoreRepository objcomstorrepos = new CompanyStoreRepository();
    CompanyStore objcomstore = new CompanyStore();
    LookupRepository objrepos = new LookupRepository();

    #endregion

    #region Event Handlers

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

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "List of all Products";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/ViewCompanyStore.aspx";

            if (Request.QueryString["Id"] != null)
            {
                this.iStoreID = Convert.ToInt32(Request.QueryString["Id"]);
                dvCompanyName.Visible = true;
                List<SelectCompanyNameCompanyIDResult> objlist = new CompanyStoreRepository().GetBYStoreId(Convert.ToInt32(iStoreID));
                lblCompanyName.Text = objlist[0].CompanyName.ToString();
            }
            else
            {
                dvCompanyName.Visible = false;
            }

            BindRepeater();
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
    protected void gvStoreProduct_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView gvStoreProduct = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            if (this.ViewState["SortOrder"].ToString() == "ASC")
                ImgSort.ImageUrl = "";
            else
                ImgSort.ImageUrl = "";

            switch (this.ViewState["SortExp"].ToString())
            {
                case "CategoryName":
                    PlaceHolder placeholderCategory = (PlaceHolder)e.Row.FindControl("placeholderCtegory");

                    break;
                case "WorkgroupName":
                    PlaceHolder placeholderWorkgroupName = (PlaceHolder)e.Row.FindControl("placeholderWorkgroupName");

                    break;
                case "SubCategoryName":
                    PlaceHolder placeholderSubCategoryName = (PlaceHolder)e.Row.FindControl("placeholderSubCategoryName");

                    break;
                case "Department":
                    PlaceHolder placeholderDepartment = (PlaceHolder)e.Row.FindControl("placeholderDepartment");

                    break;
                case "Workgroup":
                    PlaceHolder placeholderWorkgroup = (PlaceHolder)e.Row.FindControl("placeholderWorkgroup");

                    break;
                case "GarmentType":
                    PlaceHolder placeholderGarmentType = (PlaceHolder)e.Row.FindControl("placeholderGarmentType");

                    break;
                case "ProductStatus":
                    PlaceHolder placeholderProductStatus = (PlaceHolder)e.Row.FindControl("placeholderProductStatus");
                    break;
                case "ProductDescription":
                    PlaceHolder placeholderProductDescription = (PlaceHolder)e.Row.FindControl("placeholderProductDescription");
                    break;
                case "MasterItemNumber":
                    PlaceHolder placeholderMasterItemNumber = (PlaceHolder)e.Row.FindControl("placeholderMasterItemNumber");
                    break;
                case "EmployeeType":
                    PlaceHolder placeholderEmployeeType = (PlaceHolder)e.Row.FindControl("placeholderEmployeeType");
                    break;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String path = "../../../admin/Incentex_Used_Icons/" + ((HiddenField)e.Row.FindControl("hdnLookupIcon")).Value;
            ((HtmlImage)e.Row.FindControl("imgLookupIcon")).Src = path;
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
    protected void gvStoreProduct_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            GridView gvStoreProduct = (GridView)row.Parent.Parent;
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
                bindGridView(gvStoreProduct, true);
            }
            else if (e.CommandName == "Edit")  // or Edit
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                HiddenField hdnWorkgroupName = (HiddenField)row.Parent.Parent.Parent.FindControl("hdnWorkGroupName");
                Label lblProdStoreID = (Label)(row.FindControl("lblStoreProductID"));
                Label WorkgroupName = (Label)(row.FindControl("lblWorkgroup"));
                Session["WorkgroupName"] = WorkgroupName.Text;
                if (hdnWorkgroupName.Value != null)
                    Session["WorkGroupNameToDisplay"] = hdnWorkgroupName.Value;
                StoreProductExpandedStatuses();
                IncentexGlobal.ProductReturnURL = Request.Url.ToString();
                Response.Redirect("General.aspx?SubId=" + lblProdStoreID.Text + "&Id=" + iStoreID);
            }
            else if (e.CommandName == "StatusVhange")
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                Label lblProdStoreID = (Label)(row.FindControl("lblStoreProductID"));
                HiddenField hdnStatusID = (HiddenField)(row.FindControl("hdnStatusID"));
                objStoreProdRepository.UpdateStatus(Convert.ToInt32(lblProdStoreID.Text), Convert.ToInt32(hdnStatusID.Value));
                objStoreProdRepository.SubmitChanges();
                BindRepeater();
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Delete the record from
    /// the table when check box of griedview is
    /// checked true.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            //Create String Collection to store 
            //IDs of records to be deleted 
            Int64 iStroreprodID;
            //Loop through GridView rows to find checked rows 
            foreach (RepeaterItem repeaterItem in rptWorkgroup.Items)
            {
                GridView gvStoreProduct = (GridView)repeaterItem.FindControl("gvStoreProduct");
                Label lblmsg = (Label)repeaterItem.FindControl("lblmsg");
                for (Int32 i = 0; i < gvStoreProduct.Rows.Count; i++)
                {
                    CheckBox chkDelete = (CheckBox)gvStoreProduct.Rows[i].Cells[1].FindControl("CheckBox1");
                    if (chkDelete != null)
                    {
                        if (chkDelete.Checked)
                        {
                            Label lblStoreProductId = (Label)gvStoreProduct.Rows[i].Cells[0].FindControl("lblStoreProductID");
                            iStroreprodID = Convert.ToInt32(lblStoreProductId.Text);
                            objStoreProdRepository.DeleteStoreProduct(iStroreprodID, IncentexGlobal.CurrentMember.UserInfoID);
                            IsChecked = true;
                        }
                    }
                }
            }

            BindRepeater();
            // bindGridView();

            if (!IsChecked)
                lblMsgGlobal.Text = "Please Select Record to delete ...";
        }
        catch (Exception ex)
        {
            if (((System.Data.SqlClient.SqlException)(ex)).Number == 547)
            {
                //lblmsg.Text = "Unable to delete record as this record is used in other detail table";
            }
            ErrHandler.WriteError(ex);
        }
    }

    #endregion   

    #region Expander Change

    /// <summary>
    ///  BindRepeater()
    ///  26-jan-2012
    ///  Nagmani Kumar
    ///  Bind the Repeater [collasiable] with workgroupname
    /// </summary>
    public void BindRepeater()
    {
        StoreProductExpandedStatuses();

        this.pds = new Dictionary<Int64, PagedDataSource>();
        this.FrmPg = new Dictionary<Int64, Int32>();
        this.ToPg = new Dictionary<Int64, Int32>();
        this.CurrentPage = new Dictionary<Int64, Int32>();

        List<INC_Lookup> objlist = new List<INC_Lookup>();
        objlist = objrepos.GetByLookup("Workgroup ");
        rptWorkgroup.DataSource = objlist;
        rptWorkgroup.DataBind();
    }

    protected void rptWorkgroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnIamExpanded = (HiddenField)e.Item.FindControl("hdnIamExpanded");
                HiddenField hdnWorkgroupId = (HiddenField)e.Item.FindControl("hdnWorkgroupId");
                CheckBox chkActive = (CheckBox)e.Item.FindControl("chkActive");
                String[] ProductExpandedStatuses = Convert.ToString(Session["ProductExpandedStatuses"]).Split(',');

                if (ProductExpandedStatuses.Contains(hdnWorkgroupId.Value) || ProductExpandedStatuses.Contains(hdnWorkgroupId.Value + "|true"))
                    hdnIamExpanded.Value = "true";
                if (ProductExpandedStatuses.Contains(hdnWorkgroupId.Value + "|true"))
                    chkActive.Checked = true;

                GridView gvStoreproduct = (GridView)e.Item.FindControl("gvStoreproduct");
                bindGridView(gvStoreproduct, false);
            }
        }
        catch
        {
            
        }
    }

    protected void rptWorkgroup_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "AddNew")
            {
                if (Session["WorkgroupName"] != null)
                {
                    Session["WorkgroupName"] = null;
                }
                HiddenField hdnWorkgroupName = (HiddenField)e.Item.FindControl("hdnWorkgroupName");
                Session["WorkgroupName"] = hdnWorkgroupName.Value;
                Response.Redirect("General.aspx?SubId=" + 0 + "&Id=" + iStoreID, false);
            }
        }
        catch
        {
            
        }
    }

    public void bindGridView(GridView gvStoreProduct, Boolean FromPaging)
    {
        try
        {
            DataList DataList2 = (DataList)gvStoreProduct.Parent.Parent.FindControl("DataList2");
            HtmlControl pagingtable = (HtmlControl)gvStoreProduct.Parent.Parent.FindControl("pagingtable");
            LinkButton lnkbtnNext = (LinkButton)gvStoreProduct.Parent.Parent.FindControl("lnkbtnNext");
            LinkButton lnkbtnPrevious = (LinkButton)gvStoreProduct.Parent.Parent.FindControl("lnkbtnPrevious");
            HiddenField hdnWorkgroupId = (HiddenField)gvStoreProduct.Parent.Parent.FindControl("hdnWorkgroupId");
            HtmlControl dvCollapsible = (HtmlControl)gvStoreProduct.Parent.Parent.FindControl("dvCollapsible");
            Int64 StatusID = Convert.ToInt64(hdnWorkgroupId.Value);
            DataView myDataView = new DataView();

            if (iStoreID != 0)
            {
                List<Select_StoreProductDetailsResult> oad = new List<Select_StoreProductDetailsResult>();
                oad = objStoreProdRepository.StoreProductDetails(Convert.ToInt64(iStoreID), Convert.ToInt64(hdnWorkgroupId.Value));
                oad = oad.OrderBy(p => p.StatusID).ThenBy(p => p.StoreProductID).ToList();

                if (!FromPaging)
                {
                    this.FrmPg.Remove(StatusID);
                    this.FrmPg.Add(StatusID, 1);

                    this.ToPg.Remove(StatusID);
                    this.ToPg.Add(StatusID, this.PagerSize);

                    this.CurrentPage.Remove(StatusID);
                    this.CurrentPage.Add(StatusID, 0);
                }
                PagedDataSource tPds = new PagedDataSource();


                DataTable dataTable = Common.ListToDataTable(oad);
                myDataView = dataTable.DefaultView;
                if (this.ViewState["SortExp"] != null)
                {
                    myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
                }

                tPds.DataSource = myDataView;
                tPds.AllowPaging = true;
                tPds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
                tPds.CurrentPageIndex = CurrentPage.FirstOrDefault(le => le.Key == StatusID).Value;

                this.pds.Remove(StatusID);
                this.pds.Add(StatusID, tPds);

                lnkbtnNext.Enabled = !tPds.IsLastPage;
                lnkbtnPrevious.Enabled = !tPds.IsFirstPage;

                gvStoreProduct.DataSource = tPds;
                gvStoreProduct.DataBind();

                DataList2.DataSource = doPaging(StatusID);
                DataList2.DataBind();
                if (gvStoreProduct.Rows.Count == 0)
                {
                    pagingtable.Visible = false;
                }
                else
                {
                    pagingtable.Visible = true;
                }

                CheckBox chkActive = (CheckBox)gvStoreProduct.Parent.Parent.FindControl("chkActive");

                Int64? ActiveID = new LookupRepository().GetIdByLookupNameNLookUpCode("Active", "Status");
                String ProductCount = "0";
                if (chkActive.Checked == false)
                {
                    foreach (GridViewRow item in gvStoreProduct.Rows)
                    {
                        HiddenField hdnStatusID = (HiddenField)item.FindControl("hdnStatusID");
                        if (hdnStatusID.Value == "136")
                            gvStoreProduct.Rows[item.RowIndex].Visible = false;
                    }

                    ProductCount = Convert.ToString(oad.Where(le => le.StatusID == ActiveID).ToList().Count);
                    dvCollapsible.Attributes.Add("total", ProductCount);
                }
                else
                {
                    foreach (GridViewRow item in gvStoreProduct.Rows)
                    {
                        HiddenField hdnStatusID = (HiddenField)item.FindControl("hdnStatusID");
                        if (hdnStatusID.Value == "136")
                            gvStoreProduct.Rows[item.RowIndex].Visible = true;
                    }

                    ProductCount = Convert.ToString(oad.Count);
                    dvCollapsible.Attributes.Add("total", ProductCount);
                }
            }
            
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void chkActive_CheckedChanged(object sender, EventArgs e)
    {
        Int64? ActiveID = new LookupRepository().GetIdByLookupNameNLookUpCode("Active", "Status");
        String ProductCount = "0";

        CheckBox chkSender = (CheckBox)sender;

        foreach (RepeaterItem repeaterItem in rptWorkgroup.Items)
        {   
            CheckBox chkActive = (CheckBox)repeaterItem.FindControl("chkActive");

            if (chkSender.ClientID == chkActive.ClientID)
            {
                GridView gvStoreProduct = (GridView)repeaterItem.FindControl("gvStoreProduct");
                HtmlControl dvCollapsible = (HtmlControl)repeaterItem.FindControl("dvCollapsible");
                HiddenField hdnWorkgroupId = (HiddenField)gvStoreProduct.Parent.Parent.FindControl("hdnWorkgroupId");
                List<Select_StoreProductDetailsResult> oad = new List<Select_StoreProductDetailsResult>();
                oad = objStoreProdRepository.StoreProductDetails(Convert.ToInt64(iStoreID), Convert.ToInt64(hdnWorkgroupId.Value));
                if (chkActive.Checked == false)
                {
                    foreach (GridViewRow item in gvStoreProduct.Rows)
                    {
                        HiddenField hdnStatusID = (HiddenField)item.FindControl("hdnStatusID");
                        if (hdnStatusID.Value == "136")
                            gvStoreProduct.Rows[item.RowIndex].Visible = false;
                    }

                    ProductCount = Convert.ToString(oad.Where(le => le.StatusID == ActiveID).ToList().Count);
                    dvCollapsible.Attributes.Add("total", ProductCount);
                }
                else
                {
                    foreach (GridViewRow item in gvStoreProduct.Rows)
                    {
                        HiddenField hdnStatusID = (HiddenField)item.FindControl("hdnStatusID");
                        if (hdnStatusID.Value == "136")
                            gvStoreProduct.Rows[item.RowIndex].Visible = true;
                    }

                    ProductCount = Convert.ToString(oad.Count);
                    dvCollapsible.Attributes.Add("total", ProductCount);
                }
            }
        }
    }

    private void StoreProductExpandedStatuses()
    {
        if (rptWorkgroup.Items.Count > 0)
        {
            String ExpandedStatuses = String.Empty;
            foreach (RepeaterItem repItem in rptWorkgroup.Items)
            {
                HiddenField hdnWorkgroupId = (HiddenField)repItem.FindControl("hdnWorkgroupId");
                HiddenField hdnIamExpanded = (HiddenField)repItem.FindControl("hdnIamExpanded");
                CheckBox chkActive = (CheckBox)repItem.FindControl("chkActive");

                if (hdnIamExpanded != null && hdnWorkgroupId != null)
                {
                    if (Convert.ToBoolean(hdnIamExpanded.Value))
                    {
                        if (String.IsNullOrEmpty(ExpandedStatuses))
                            ExpandedStatuses = hdnWorkgroupId.Value;
                        else
                            ExpandedStatuses += "," + hdnWorkgroupId.Value;

                        if (chkActive.Checked == true)
                            ExpandedStatuses += "|true";
                    }
                }
            }

            Session["ProductExpandedStatuses"] = ExpandedStatuses;
        }
    }

    protected void btnProductUpload_Click(object sender, EventArgs e)
    {
        BulkProductUpload blkObj = new BulkProductUpload();
        String fileLoc = GetFileExcelPath(flproductUpload.Value, flproductUpload);
        try
        {
            blkObj.SetStoreProduct(Server.MapPath(fileLoc), this.iStoreID);
            lblStatus.Visible = true;
            lblStatus.Text = "Records successfully uploaded.";
            modalProduct.Show();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            lblStatus.Visible = true;
            lblStatus.Text = "Error occur while processing your request.";
            modalProduct.Show();
        }
        finally
        {
            String[] test = fileLoc.Split('/');
            for (Int32 i = 0; i < test.Length; i++)
            {
                if (i == test.Length - 1)
                {
                    String t = test[i].ToString();
                    fileLoc = fileLoc.Replace("/" + t, "");
                    System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Server.MapPath(fileLoc));
                    foreach (System.IO.FileInfo file in directory.GetFiles())
                    {
                        file.Delete();
                    }
                    System.IO.Directory.Delete(Server.MapPath(fileLoc));
                }
            }
            modalProduct.Show();
        }
    }

    private String GetFileExcelPath(String fileName, HtmlInputFile flupload)
    {
        String filePath = String.Empty;

        try
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                String ext = System.IO.Path.GetExtension(fileName);
                filePath = "~/UploadedImages/TempBulkProduct";
                // Create the subfolder
                if (!System.IO.Directory.Exists(Server.MapPath(filePath)))
                    System.IO.Directory.CreateDirectory(Server.MapPath(filePath));
                HttpPostedFile tempExcel = flupload.PostedFile;
                filePath += "/" + fileName;

                if (!String.IsNullOrEmpty(filePath))
                {
                    tempExcel.SaveAs(Server.MapPath(filePath));
                }
            }
        }
        catch
        {

        }

        return filePath;
    }

    protected void btnSubItemUpload_Click(object sender, EventArgs e)
    {
        BulkProductSubItemUpload blkSubItemObj = new BulkProductSubItemUpload();
        String fileLoc = GetFileExcelPath(flSubproductUpload.Value, flSubproductUpload);
        try
        {
            blkSubItemObj.SetProductSubitem(Server.MapPath(fileLoc));
            lblsubitemmsg.Visible = true;
            lblsubitemmsg.Text = "Records successfully uploaded.";
            modalSubItemProduct.Show();
        }
        catch (Exception ex)
        {
            lblsubitemmsg.Visible = true;
            lblsubitemmsg.Text = "Error occur while processing your request.";
            modalSubItemProduct.Show();
        }
        finally
        {
            String[] test = fileLoc.Split('/');
            for (Int32 i = 0; i < test.Length; i++)
            {
                if (i == test.Length - 1)
                {
                    String t = test[i].ToString();
                    fileLoc = fileLoc.Replace("/" + t, "");
                    System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Server.MapPath(fileLoc));
                    foreach (System.IO.FileInfo file in directory.GetFiles())
                    {
                        file.Delete();
                    }
                    System.IO.Directory.Delete(Server.MapPath(fileLoc));
                }
            }
            modalSubItemProduct.Show();
        }
    }

    protected void btnDownloadExcel_Click(object sender, EventArgs e)
    {
        BulkProductSubItemUpload blkSubItemObj = new BulkProductSubItemUpload();
        Int64 workgroupID = Convert.ToInt64(ddlWorkgruop.SelectedValue);
        try
        {
            var listitems = objStoreProdRepository.GetStoreIDforDataExcel(this.iStoreID, workgroupID);
            if (listitems.Count > 0)
            {
                lblsubitemmsg.Visible = false;
                DataTable dtable = blkSubItemObj.LINQToDataTable(listitems);
                ExportTableData(dtable);
            }
            else
            {
                lblsubitemmsg.Visible = true;
                lblsubitemmsg.Text = "There is no records associated with this workgroup";
            }
        }
        catch
        {
            lblsubitemmsg.Text = "Error occur while processing your request";
        }
    }

    public void ExportTableData(DataTable dtdata)
    {
        String attach = "attachment;filename=ProductDetails.xls";
        this.Response.ClearContent();
        this.Response.AddHeader("content-disposition", attach);
        this.Response.ContentType = "application/ms-excel";
        if (dtdata != null)
        {
            foreach (DataColumn dc in dtdata.Columns)
            {
                this.Response.Write(dc.ColumnName + "\t");
            }
            this.Response.Write(System.Environment.NewLine);
            foreach (DataRow dr in dtdata.Rows)
            {
                for (Int32 i = 0; i < dtdata.Columns.Count; i++)
                {
                    this.Response.Write(dr[i].ToString() + "\t");
                }
                this.Response.Write("\n");
            }
            this.Response.End();
        }
    }

    protected void lnkProductUpload_Click(object sender, EventArgs e)
    {
        lblStatus.Visible = false;
        modalProduct.Show();
    }

    protected void lnkSubItemBulkProduct_Click(object sender, EventArgs e)
    {
        lblsubitemmsg.Visible = false;
        // Bind ddl workgroup
        List<INC_Lookup> objlist = new List<INC_Lookup>();
        objlist = objrepos.GetByLookup("Workgroup ");
        var workList = (from i in objlist
                        select new { i.iLookupID, i.sLookupName }).Distinct().ToList();
        Common.BindDDL(ddlWorkgruop, workList, "sLookupName", "iLookupID", "-select-");
        lblsubitemmsg.Visible = false;
        modalSubItemProduct.Show();
    }

    #endregion

    #region Paging

    private DataTable doPaging(Int64 StatusID)
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            Int32 tCurrentPg = this.pds[StatusID].CurrentPageIndex + 1;
            Int32 tToPg = this.PagerSize;
            Int32 tFrmPg = this.FrmPg[StatusID];

            if (tCurrentPg > tToPg)
            {
                tFrmPg = tToPg + 1;
                tToPg += this.PagerSize;
            }

            if (tCurrentPg < tFrmPg)
            {
                tToPg = tFrmPg - 1;
                tFrmPg -= this.PagerSize;
            }

            tToPg = Convert.ToInt16(this.pds[StatusID].PageCount);

            for (Int32 i = tFrmPg - 1; i < tToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr["PageIndex"] = i;
                dr["PageText"] = i + 1;
                dt.Rows.Add(dr);
            }

            this.ToPg[StatusID] = tToPg;
            this.FrmPg[StatusID] = tFrmPg;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return dt;
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
        LinkButton lnkbtnPrevious = (LinkButton)sender;
        HiddenField hdnWorkgroupId = (HiddenField)lnkbtnPrevious.Parent.FindControl("hdnWorkgroupId");
        Int64 StatusID = Convert.ToInt64(hdnWorkgroupId.Value);
        CurrentPage[StatusID] -= 1;

        GridView gvStoreProduct = (GridView)lnkbtnPrevious.Parent.FindControl("gvStoreProduct");
        bindGridView(gvStoreProduct, true);
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
        LinkButton lnkbtnNext = (LinkButton)sender;
        HiddenField hdnWorkgroupId = (HiddenField)lnkbtnNext.Parent.FindControl("hdnWorkgroupId");
        Int64 StatusID = Convert.ToInt64(hdnWorkgroupId.Value);
        CurrentPage[StatusID] += 1;

        GridView gvStoreProduct = (GridView)lnkbtnNext.Parent.FindControl("gvStoreProduct");
        bindGridView(gvStoreProduct, true);
    }

    protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            GridView gvStoreProduct = (GridView)e.Item.Parent.Parent.FindControl("gvStoreProduct");
            HiddenField hdnWorkgroupId = (HiddenField)e.Item.Parent.Parent.FindControl("hdnWorkgroupId");
            Int64 StatusID = Convert.ToInt64(hdnWorkgroupId.Value);
            CurrentPage[StatusID] = Convert.ToInt16(e.CommandArgument);
            //BindRepeater();
            bindGridView(gvStoreProduct, true);
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
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage.FirstOrDefault().Value + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    #endregion
}