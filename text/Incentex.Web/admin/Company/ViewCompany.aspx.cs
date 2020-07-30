using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;

public partial class admin_Company_ViewCompany : PageBase
{
    bool IsChecked = false;
    PagedDataSource pds = new PagedDataSource();
    CompanyRepository objComRepos = new CompanyRepository();
    CompanyContactInfoRepository objCompContactRepos = new CompanyContactInfoRepository();
    NotesHistoryRepository objNoticeRepos = new NotesHistoryRepository();
    CompanyContactInfoRepository objBillingComRepos = new CompanyContactInfoRepository();
    decimal TotalAmount = 0;
    decimal TotalStation = 0;
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
                return 5;
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
            base.MenuItem = "Manage Company";
            base.ParentMenuID = 10;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text  = "Manage Company";
             ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/UserManagement.aspx";
             bindGridView();
        }
    }
    protected void btnAddCompany_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }
       // Session["ManageID"] = 1;
        Response.Redirect("AddCompany.aspx?id=0");
       
    }
    protected void gvCompany_RowDataBound(object sender, GridViewRowEventArgs e)
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
                case "CompanyName":
                    PlaceHolder placeholderCompany = (PlaceHolder)e.Row.FindControl("placeholderCompanyName");
                    //placeholderCompany.Controls.Add(ImgSort);
                    break;
                case "Country":
                    PlaceHolder placeholderCoutry = (PlaceHolder)e.Row.FindControl("placeholderCountry");
                    //placeholderCoutry.Controls.Add(ImgSort);
                    break;
                case "Telephone":
                    PlaceHolder placeholderTel = (PlaceHolder)e.Row.FindControl("placeholderTelephone");
                    //placeholderTel.Controls.Add(ImgSort);
                    break;
                case "NoOfEmp":
                    PlaceHolder placeholderNoEmployee = (PlaceHolder)e.Row.FindControl("placeholderNumberEmployees");
                    //placeholderTel.Controls.Add(ImgSort);
                    break;
                case "StationID":
                    PlaceHolder placeholderStation = (PlaceHolder)e.Row.FindControl("placeholderNoofStation");
                    //placeholderTel.Controls.Add(ImgSort);
                    break;
                
            }

        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TotalAmount = TotalAmount + Convert.ToDecimal(((Label)e.Row.FindControl("lblEmployeeno")).Text);
            hdnTotalValue.Value = TotalAmount.ToString();

            TotalStation = TotalStation + Convert.ToDecimal(((Label)e.Row.FindControl("lblStationID")).Text);
            hndTotalStation.Value = TotalStation.ToString();
           
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[4].Text = "Total of Employees:" + hdnTotalValue.Value;
            e.Row.Cells[4].Style.Add("style", "text-align:right");

            e.Row.Cells[5].Text = "Total of Stations:" + hndTotalStation.Value;
            e.Row.Cells[5].Style.Add("style", "text-align:right");

            e.Row.Font.Bold = true;

        }

        
    }
    protected void gvCompany_RowCommand(object sender, GridViewCommandEventArgs e)
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
           // Session["ManageID"] = 1;
            LinkButton lbtnAction;
            Label lblCompanyID;
            GridViewRow row;
             row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
              lbtnAction = (LinkButton)(gvCompany.Rows[row.RowIndex].FindControl("hypCompanyName"));
              lblCompanyID = (Label)(gvCompany.Rows[row.RowIndex].FindControl("lblCompanyID"));
             // Session["CompanyID"] = lblCompanyID.Text;
            Response.Redirect("AddCompany.aspx?id=" + lblCompanyID.Text);
        }
        if (e.CommandName == "Station")  // or go on Stattion page
        {
            // Session["ManageID"] = 1;
            LinkButton lbtnAction;
            Label lblCompanyID;
            GridViewRow row;
            row = (GridViewRow)((Button)e.CommandSource).Parent.Parent;
            lbtnAction = (LinkButton)(gvCompany.Rows[row.RowIndex].FindControl("hypCompanyName"));
            lblCompanyID = (Label)(gvCompany.Rows[row.RowIndex].FindControl("lblCompanyID"));
            // Session["CompanyID"] = lblCompanyID.Text;
            Response.Redirect("Station/MainStationInfo.aspx?id=" + lblCompanyID.Text + "&subId=0");
            //Response.Redirect("../../../admin/Company/Station/MainStationInfo.aspx?id=" + lblCompanyID.Text + "&subId=0");
        }
        if (e.CommandName == "Emp")  // or go on Employee page
        {
            // Session["ManageID"] = 1;
            LinkButton lbtnAction;
            Label lblCompanyID;
            GridViewRow row;
            row = (GridViewRow)((Button)e.CommandSource).Parent.Parent;
            lbtnAction = (LinkButton)(gvCompany.Rows[row.RowIndex].FindControl("hypCompanyName"));
            lblCompanyID = (Label)(gvCompany.Rows[row.RowIndex].FindControl("lblCompanyID"));
            // Session["CompanyID"] = lblCompanyID.Text;
            Response.Redirect("Employee/BasicInformation.aspx?id=" + lblCompanyID.Text + "&subId=0");
        }
        if (e.CommandName == "GlobalSetting")
        {
            Label lblCompanyID;
            GridViewRow row;
            row = (GridViewRow)((Button)e.CommandSource).Parent.Parent;
            lblCompanyID = (Label)(gvCompany.Rows[row.RowIndex].FindControl("lblCompanyID"));
            Response.Redirect("~/admin/CompanyLevelGlobalSetting.aspx?id=" + lblCompanyID.Text);
        }


    }
    /// <summary>
    /// btnDelete_Click()
    /// Delete the record from table Company,NoticeDetail and CompanyContactibfo
    /// table on the passing companyid.
    /// Nagmani kumar 08/09/2010
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
            string  iID = null;
            //Loop through GridView rows to find checked rows 
            for (int i = 0; i < gvCompany.Rows.Count; i++)
            {
                CheckBox chkDelete = (CheckBox)
                 gvCompany.Rows[i].Cells[1].FindControl("CheckBox1");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        Label lblCompanyId = (Label)gvCompany.Rows[i].Cells[0].FindControl("lblCompanyID");
                        //strID = gvCompany.Rows[i].Cells[0].Text;
                        iID = lblCompanyId.Text;
                        string strAccounttype = "AccountType";
                        string strBiling = "Billing";
                        objCompContactRepos.DeleteCompanyContact(iID, strAccounttype);
                        objBillingComRepos.DeleteCompanyContact(iID, strBiling);
                        objNoticeRepos.DeleteCompanyNotice(iID);
                        objComRepos.DeleteCompany(iID);
                        IsChecked = true;
                    }
                }
            }     
            bindGridView();    

            if (IsChecked)
            {
                objComRepos.SubmitChanges();
                lblmsg.Text = "Selected Records Deleted Successfully ...";
            }
            else
            {
                lblmsg.Text = "Please Select Record to delete ...";
            }
            
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            if (((System.Data.SqlClient.SqlException)(ex)).Number == 547)
            {
                lblmsg.Text = "Unable to delete employee as this record is used in other detail table";
            }
            ErrHandler.WriteError(ex);
    }
}
   public void bindGridView()
    {
        List<CompanyRepository.CompanyContactBillingResult> oad = new CompanyRepository().CompanyAndDetails();
       
        if (oad.Count == 0)
        {
            pagingtable.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
           // divStats.Visible = true;
          //  lblRecordCount.Text = oad.Count.ToString();
        }
        DataView myDataView = new DataView();
        DataTable dataTable = ListToDataTable(oad);
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
        gvCompany.DataSource = pds;
        gvCompany.DataBind();
        doPaging(); 
    } 
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
               ToPg = ToPg + 5;
           }
           if (CurrentPg < FrmPg)
           {
               ToPg = FrmPg - 1;
               FrmPg = FrmPg - 5;

           }

           if (pds.PageCount < ToPg)
           {
               ToPg = pds.PageCount;
           }

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
  
}
