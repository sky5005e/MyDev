using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Supplier_ViewAddSupplierEmployee : PageBase
{
    PagedDataSource pds = new PagedDataSource();
    SupplierEmployee objSupEmp = new SupplierEmployee();
    SupplierEmployeeRepository objSupCompRep = new SupplierEmployeeRepository();
    NotesHistoryRepository objNoticeRepos = new NotesHistoryRepository();
    List<SupplierEmployeeRepository.IEListResults> objIE = new List<SupplierEmployeeRepository.IEListResults>();

    int iSupplierId;
    bool IsChecked = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Employee";
            base.ParentMenuID = 18;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Supplier Employees";
            ((HtmlGenericControl)manuControl.FindControl("dvSubMenu")).Visible = false;
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Supplier/ViewSupplier.aspx";
            if (Request.QueryString["id"] != "0" || Request.QueryString["id"]!=null)
            {
                iSupplierId = Convert.ToInt32(Request.QueryString["id"].ToString());
                
                bindGridView();
            }
            else
            {
                Response.Redirect("ViewSupplier.aspx");
            }
            manuControl.PopulateMenu(3, 0, this.iSupplierId, 0, false);
        }
    }
    /// <summary>
    /// gvSupplierEmployee_RowCommand()
    /// Set the Ascending and Descending order of
    /// Sorting data on findin the currnet order of data
    /// if sorting order is Ascendig and when user click for 
    /// sorting then it set in descending order and vice versa.
    /// On Employee name click go to the Add supplier employee page
    /// with taking query string supplieremployeeid.
    /// Nagmani Kumar 15/09/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvSupplierEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
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

        if (e.CommandName == "Edit")  // or Edit
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            LinkButton lbtnAction;
            Label lblSupplierEmployeeID;
            GridViewRow row;
            row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            lbtnAction = (LinkButton)(gvSupplierEmployee.Rows[row.RowIndex].FindControl("hypEmployeeName"));
            lblSupplierEmployeeID = (Label)(gvSupplierEmployee.Rows[row.RowIndex].FindControl("lblSupplierEmpiD"));
            Response.Redirect("BasicSupplierInformation.aspx?id=" + Convert.ToInt32(Request.QueryString["id"].ToString()) + "&SubId=" + lblSupplierEmployeeID.Text);
        }
    }
    /// <summary>
    /// Sorting the Griedview.
    /// Nagmani 15/09/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvSupplierEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
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
                case "EmployeeName":
                    PlaceHolder placeholderEmployeeName = (PlaceHolder)e.Row.FindControl("placeholderEmployeeName");
                   // placeholderEmployeeName.Controls.Add(ImgSort);
                    break;
                case "Title":
                    PlaceHolder placeholderTitle = (PlaceHolder)e.Row.FindControl("placeholderTitle");
                   // placeholderTitle.Controls.Add(ImgSort);
                    break;
                case "Telephone":
                    PlaceHolder placeholderTel = (PlaceHolder)e.Row.FindControl("placeholderTelephone");
                   // placeholderTel.Controls.Add(ImgSort);
                    break;


            }

        }
    }
    /// <summary>
    /// Find the Command argument to search CurrentPage.
    /// Nagmani Kuamr 15/09/2010
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindGridView();
        }
    }
    /// <summary>
    /// This event is used to enable and disable the page
    /// in paging fucntion.
    /// Nagmani Kumar 15/09/2010
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
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        bindGridView();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        bindGridView();
    }
    protected void btnAddSuppEmp_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        Response.Redirect("BasicSupplierInformation.aspx?id=" + Convert.ToInt32(Request.QueryString["id"].ToString()));
    }
    /// <summary>
    /// Delete the Record on Passing the supplierEmployeeid
    /// from the table supplierEmployee table.
    /// NAgmani KUmar 15/09/2010
    /// </summary>
    /// <param name="supplierEmployeeid"></param>
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
            string iID = null;
            //Loop through GridView rows to find checked rows 
            for (int i = 0; i < gvSupplierEmployee.Rows.Count; i++)
            {
                CheckBox chkDelete = (CheckBox)
                 gvSupplierEmployee.Rows[i].Cells[1].FindControl("CheckBox1");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {                       
                        Label lblSupEmpId = (Label)gvSupplierEmployee.Rows[i].Cells[0].FindControl("lblSupplierEmpiD");
                        iID = lblSupEmpId.Text;
                        //Old Code
                        //objNoticeRepos.DeleteCompanyNotice(iID);  
                        //objSupCompRep.DeleteSupplierEmployee(iID);
                        //New
                        objSupCompRep.Delete(Convert.ToInt64(iID), IncentexGlobal.CurrentMember.UserInfoID);
                        IsChecked = true;
                    }
                }
            }
            
            bindGridView();


            if (IsChecked)
            {
                objSupCompRep.SubmitChanges();
                lblMsg.Text = "Selected Records Deleted Successfully ...";
            }
            else
            {
                lblMsg.Text = "Please Select Record to delete ...";
            }

        }
        catch (Exception ex)
        {
            if (((System.Data.SqlClient.SqlException)(ex)).Number == 547)
            {
                lblMsg.Text = "Unable to delete employee as this record is used in other detail table";
            }
            ErrHandler.WriteError(ex);
        }
    }
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);
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
    /// <summary>
    /// CurrentPage()
    /// Set and Get property for the Currnet page Record
    /// Nagmani Kuamr 15/09/2010
    /// </summary>
    public int CurrentPage
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
    public int PagerSize
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
    public int FrmPg
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
    public int ToPg
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
    /// Pageing fucntionality in gridview
    /// Nagmani Kuamr 15/09/2010
    /// </summary>
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

            DataList2.DataSource = dt;
            DataList2.DataBind();

        }
        catch (Exception)
        { }

    }
    /// <summary>
    /// bindGridView()
    /// Show the Record in Gridview from SupplierEmploye Table
    /// Nagmani Kuamr 15/09/2010
    /// </summary>
  
    public void bindGridView()
    {
        DataView myDataView = new DataView();
        objIE = objSupCompRep.GetAllSupplierEmployee(Convert.ToInt32(Request.QueryString["id"].ToString()));
        if (objIE.Count == 0)
        {
            pagingtable.Visible = false;
            btnDelete.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
            btnDelete.Visible = true;
        }
        DataTable dataTable = ListToDataTable(objIE);
        myDataView = dataTable.DefaultView;
        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gvSupplierEmployee.DataSource = pds;
        gvSupplierEmployee.DataBind();
        doPaging();
    }

}
