using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Supplier_PastDownloads : PageBase
{
    #region Page Properties

    private Int32 CurrentPage
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

    private Int32 PagerSize
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

    private Int32 FrmPg
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

    private Int32 ToPg
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

    PagedDataSource pds = new PagedDataSource();

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "View Past Downloads";
            base.ParentMenuID = 60;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Supplier/GenerateSalesOrdersCSV.aspx";
            ((Label)Master.FindControl("lblPageHeading")).Text = "Past Downloads";

            gvDownloadedOrders.Visible = false;            
            lnkDelete.Visible = false;
            pagingTable.Visible = false;

            txtFromDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
        }
    }

    #endregion

    #region Control Events

    protected void lnkBtnSearchNow_Click(object sender, EventArgs e)
    {
        try
        {
            BindGrid(txtFromDate.Text.Trim(), txtToDate.Text.Trim());
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvDownloadedOrders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            switch (this.ViewState["SortExp"].ToString())
            {
                case "DisplayName":
                    PlaceHolder placeHolderDisplayName = (PlaceHolder)e.Row.FindControl("placeHolderDisplayName");
                    break;
                case "DownloadedBy":
                    PlaceHolder placeHolderDownloadedBy = (PlaceHolder)e.Row.FindControl("placeHolderDownloadedBy");
                    break;
                case "DownloadedDate":
                    PlaceHolder placeHolderDownloadedDate = (PlaceHolder)e.Row.FindControl("placeHolderDownloadedDate");
                    break;
            }
        }
    }

    protected void gvDownloadedOrders_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                GridView gvDownloadedOrders = sender as GridView;
                CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                CheckBox chkBxHeader = (CheckBox)gvDownloadedOrders.HeaderRow.FindControl("chkBxHeader");
                chkBxSelect.Attributes["onclick"] = String.Format("javascript:ChildClick(this,'{0}');", chkBxHeader.ClientID);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvDownloadedOrders_RowCommand(object sender, GridViewCommandEventArgs e)
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

            BindGrid(txtFromDate.Text.Trim(), txtToDate.Text.Trim());
        }
        else if (e.CommandName == "Download")
        {
            String[] Args = Convert.ToString(e.CommandArgument).Split('|');
            String filePath = Common.DownloadedPAASOrderPath + Args[1];
            DownloadFile(filePath, Args[0]);
        }
    }

    /// <summary>
    /// delete record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            Boolean IsChecked = false;

            foreach (GridViewRow gvRow in gvDownloadedOrders.Rows)
            {
                CheckBox chkBxSelect = (CheckBox)gvRow.FindControl("chkBxSelect");
                HiddenField hdnID = (HiddenField)gvRow.FindControl("hdnID");
                HiddenField hdnOriginalFile = (HiddenField)gvRow.FindControl("hdnOriginalFile");

                if (chkBxSelect.Checked)
                {
                    PAASRepository objRepo = new PAASRepository();
                    if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
                    {
                        if (File.Exists(Common.DownloadedPAASOrderPath + Convert.ToString(hdnOriginalFile.Value)))
                            File.Delete(Common.DownloadedPAASOrderPath + Convert.ToString(hdnOriginalFile.Value));
                        objRepo.DeleteDownloadedPAASOrder(Convert.ToInt64(hdnID.Value), true);
                    }
                    else
                    {
                        objRepo.DeleteDownloadedPAASOrder(Convert.ToInt64(hdnID.Value), false);
                    }                    
                    IsChecked = true;
                }
            }

            if (IsChecked)
            {
                lblMsgList.Text = "Selected records deleted successfully...";
            }
            else
            {
                lblMsgList.Text = "Please select record to delete...";
            }

            BindGrid(txtFromDate.Text.Trim(), txtToDate.Text.Trim());
        }
        catch (Exception ex)
        {
            lblMsgList.Text = "Error in deleting record ...";
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Methods

    private void BindGrid(String FromDate, String ToDate)
    {
        PAASRepository objRepo = new PAASRepository();
        List<SelectCSVFilesByDatesResult> lstCSVFiles = new List<SelectCSVFilesByDatesResult>();

        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
        {
            lstCSVFiles = objRepo.SelectCSVFilesByDate(FromDate, ToDate).OrderByDescending(le => le.DownloadedDate).ToList();
        }
        else
        {
            lstCSVFiles = objRepo.SelectCSVFilesByDate(FromDate, ToDate).Where(le => le.IsDeleted == false).OrderByDescending(le => le.DownloadedDate).ToList();
        }

        DataTable dataTable = new DataTable();
        dataTable = ListToDataTable(lstCSVFiles);

        DataView myDataView = new DataView();
        myDataView = dataTable.DefaultView;
        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkBtnNext.Enabled = !pds.IsLastPage;
        lnkBtnPrevious.Enabled = !pds.IsFirstPage;
        gvDownloadedOrders.DataSource = pds;
        gvDownloadedOrders.DataBind();
        doPaging();

        if (gvDownloadedOrders.Rows.Count == 0)
        {
            pagingTable.Visible = false;
            lnkDelete.Visible = false;
            gvDownloadedOrders.Visible = false;
            lblMsgList.Text = "No records found.";            
        }
        else
        {
            gvDownloadedOrders.Visible = true;
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
            {
                gvDownloadedOrders.Columns[0].Visible = true;
                lnkDelete.Visible = true;
            }
            else
            {
                gvDownloadedOrders.Columns[0].Visible = false;
                lnkDelete.Visible = false;
            }
            pagingTable.Visible = true;
            lblRecords.Text = lstCSVFiles.Count.ToString();
            dvTotal.Attributes.CssStyle.Remove("display");
        }
    }

    private void DownloadFile(String filePath, String displayName)
    {
        Stream iStream = null;

        try
        {
            // Open the file.
            iStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            // Total bytes to read:
            Byte[] data = new ASCIIEncoding().GetBytes(Convert.ToString(iStream));

            Response.AppendHeader("Content-Disposition", "attachment; filename=" + displayName + "");
            Response.ContentType = "application/zip";
            Response.OutputStream.Write(data, 0, data.Length);
            Response.WriteFile(filePath);
            Response.End();
        }
        catch (FileNotFoundException FNFEx)
        {
            lblMsgList.Text = "File not found.";
            ErrHandler.WriteError(FNFEx);
        }
        catch (Exception ex)
        {
            // Trap the error, if any.
            ErrHandler.WriteError(ex);
        }
        finally
        {
            if (iStream != null)
            {
                //Close the file.
                iStream.Close();
            }
        }
    }

    private static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    #endregion

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkBtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument);
            BindGrid(txtFromDate.Text.Trim(), txtToDate.Text.Trim());
        }
    }

    protected void dtlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkBtnPaging = (LinkButton)e.Item.FindControl("lnkBtnPaging");
        if (lnkBtnPaging.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkBtnPaging.Enabled = false;
            lnkBtnPaging.Font.Bold = true;
        }
    }

    private void doPaging()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            int CurrentPg = pds.CurrentPageIndex + 1;

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

            for (int i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();
        }
        catch (Exception)
        { }
    }

    protected void lnkBtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindGrid(txtFromDate.Text.Trim(), txtToDate.Text.Trim());
    }

    protected void lnkBtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindGrid(txtFromDate.Text.Trim(), txtToDate.Text.Trim());
    }

    #endregion
}
