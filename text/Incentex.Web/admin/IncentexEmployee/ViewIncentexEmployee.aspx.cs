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
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;

public partial class admin_UserManagement_ViewIncentexEmployee : PageBase
{

    List<IncentexEmployeeRepository.IEListResults> objIE = new List<IncentexEmployeeRepository.IEListResults>();
    IncentexEmployeeRepository objIeRep = new IncentexEmployeeRepository();
    PagedDataSource pds = new PagedDataSource();
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Incentex Employee";
            base.ParentMenuID = 10;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Manage Incentex Employee";
           // ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to User Management</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/UserManagement.aspx";
             
            bindGrid();
          
        }
   
    }
    /// <summary>
    /// Bind Grid Function with Paging
    /// </summary>
    public void bindGrid()
    {
        DataView myDataView = new DataView();
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
        {
            objIE = objIeRep.GetEmployeeSAdmin();//including Super Admin
        }
        else
        {
            objIE = objIeRep.GetAllEmployee();//excluding Super Admin
        }
       
        if(objIE.Count == 0)
        {
            dvPaging.Visible = false;
            lnkBtnDelete.Visible = false;
        }
        else
        {
            dvPaging.Visible = true;
            lnkBtnDelete.Visible = true;
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
        gvIncentexEmployee.DataSource = pds;
        gvIncentexEmployee.DataBind();

        doPaging();
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

            dtlViewEmployee.DataSource = dt;
            dtlViewEmployee.DataBind();

        }
        catch (Exception)
        { }
    }
    /// <summary>
    /// COnverting from List to Datatables
    /// </summary>
    
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null);
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    /// <summary>
    /// Delete selected incentex employee
    /// </summary>
    
    protected void lnkBtnDelete_Click(object sender, EventArgs e)
    {
        Boolean IsChecked = false;

        try
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            IncentexEmployeeRepository objRepo = new IncentexEmployeeRepository();

            foreach (GridViewRow gr in gvIncentexEmployee.Rows)
            {
                CheckBox chkDelete = gr.FindControl("chkemployee") as CheckBox;
                Label lblID = gr.FindControl("lblCompanyEmployeeID") as Label;

                if (chkDelete.Checked)
                {
                    
                    objRepo.Delete(Convert.ToInt64(lblID.Text), IncentexGlobal.CurrentMember.UserInfoID);
                    IsChecked = true;
                }

            }
            
            if (IsChecked)
            {
                objRepo.SubmitChanges();
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
                lblMsg.Text = "Unable to delete employee as this record exists in other detail table";
                return;
            }
        }
        bindGrid();

    }

    /// <summary>
    /// Redirection  to the add new page
    /// </summary>
    protected void lnkBtnAddEmployee_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        Response.Redirect("BasicInformation.aspx?Id=0");
    }

    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        bindGrid();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        bindGrid();
    }
    protected void dtlViewEmployee_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
    protected void dtlViewEmployee_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindGrid();
        }
        
    }
    protected void gvIncentexEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvIncentexEmployee.PageIndex = e.NewPageIndex;
        bindGrid();
    }
    protected void gvIncentexEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
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
            bindGrid();
        }
        if (e.CommandName == "viewemployeeinfo")
        {
            Response.Redirect("BasicInformation.aspx?Id=" + e.CommandArgument.ToString());
        }
    }

    protected void gvIncentexEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    //placeholderEmployeeName.Controls.Add(ImgSort);
                    break;
                case "Country":
                    PlaceHolder placeholderCountry = (PlaceHolder)e.Row.FindControl("placeholderCountry");
                   // placeholderCountry.Controls.Add(ImgSort);
                    break;
                case "State":
                    PlaceHolder placeholderState = (PlaceHolder)e.Row.FindControl("placeholderState");
                    //placeholderState.Controls.Add(ImgSort);
                    break;
                case "Telephone":
                    PlaceHolder placeholderTelephone = (PlaceHolder)e.Row.FindControl("placeholderTelephone");
                   // placeholderTelephone.Controls.Add(ImgSort);
                    break;
                case "Mobile":
                    PlaceHolder placeholdercontactnumber = (PlaceHolder)e.Row.FindControl("placeholdercontactnumber");
                   // placeholdercontactnumber.Controls.Add(ImgSort);
                    break;
                case "IsDirectEmployee":
                    PlaceHolder placeholderemployeetype = (PlaceHolder)e.Row.FindControl("placeholderEmployeeType");
                    //placeholderemployeetype.Controls.Add(ImgSort);
                    break;
            }
        }
    }
}