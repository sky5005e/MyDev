using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;

public partial class admin_Supplier_ViewSupplier : PageBase
{

    #region Properties

    const int PageSize = 10;

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
      SupplierRepository.SupplierSortExpType   SortExp
      {
          get
          {
              if(ViewState["SortExp"] == null)
              {
                  ViewState["SortExp"]= SupplierRepository.SupplierSortExpType.FirstName;
              }
              return (SupplierRepository.SupplierSortExpType)ViewState["SortExp"];
          }
          set
          {
              ViewState["SortExp"] = value;
          }
      }

    DAEnums.SortOrderType SortOrder
    {
        get
        {
            if(ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"]= DAEnums.SortOrderType.Asc ;
            }
            return (DAEnums.SortOrderType) ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }


    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            base.MenuItem = "Manage Supplier";
            base.ParentMenuID = 10;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Supplier Listing";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to User Management</span>";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/UserManagement.aspx";
        
            gv.DataBind();

        }
    }

    PagedDataSource pds = new PagedDataSource();

    protected void gv_DataBinding(object sender, EventArgs e)
    {
        SupplierRepository objRepo = new SupplierRepository();

        IEnumerable objList = objRepo.GetSupplierList( this.SortExp, this.SortOrder   );

        pds.DataSource = objList;
        pds.AllowPaging = true;
        pds.PageSize = PageSize;
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gv.DataSource = pds;
        
        doPaging(); 
        //gv.DataSource = objList;
    }

    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowType == DataControlRowType.DataRow )
        {
            Label lblID = e.Row.FindControl("lblID") as Label;

            
            Label lblNoofEmp = e.Row.FindControl("lblNoofEmp") as Label;
            SupplierEmployeeRepository objRepo = new SupplierEmployeeRepository();
            lblNoofEmp.Text = objRepo.GetCountSupplierId( Convert.ToInt32(lblID.Text)).ToString();
            
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

            lstPaging.DataSource = dt;
            lstPaging.DataBind();

        }
        catch (Exception)
        { }

    }


    protected void lstPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            gv.DataBind();
            //bindGridView();
        }
    }
    protected void lstPaging_ItemDataBound(object sender, DataListItemEventArgs e)
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
        gv.PageIndex = CurrentPage;
        gv.DataBind();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        gv.PageIndex = CurrentPage;
        gv.DataBind();
    }

    protected void gv_DataBound(object sender, EventArgs e)
    {
        if (gv.Rows.Count == 0)
        {
            lnkDelete.Visible = false;
            lstPaging.Visible = false;
            lnkbtnNext.Visible = false;
            lnkbtnPrevious.Visible = false;
        }
        else
        {
            lnkDelete.Visible = true;
            lstPaging.Visible = true;
            lnkbtnNext.Visible = true;
            lnkbtnPrevious.Visible = true;
        }
    }


    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Sorting":

                if (this.SortExp.ToString() == e.CommandArgument.ToString())
                {
                    if (this.SortOrder == DAEnums.SortOrderType.Asc)
                        this.SortOrder = DAEnums.SortOrderType.Desc;
                    else
                        this.SortOrder = DAEnums.SortOrderType.Asc;
                }
                else
                {
                    this.SortOrder = DAEnums.SortOrderType.Asc;
                    this.SortExp = (SupplierRepository.SupplierSortExpType) Enum.Parse( typeof(SupplierRepository.SupplierSortExpType), e.CommandArgument.ToString());
                }
                gv.DataBind();
                break;

            case "AddEmp":
                if (!base.CanAdd)
                {
                    base.RedirectToUnauthorised();
                }

                Response.Redirect("BasicSupplierInformation.aspx?Id=" + e.CommandArgument);

                break;

        }
    }

    /// <summary>
    /// Delete Selected Records
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkDelete_Click(object sender, EventArgs e)
    {

        SupplierRepository objRepo = new SupplierRepository();

        try
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            string NotDeleted = "";

            bool IsAnySelected = false;
            foreach (GridViewRow gr in gv.Rows)
            {
                CheckBox chkDelete = gr.FindControl("chkDelete") as CheckBox;
                Label lblID = gr.FindControl("lblID") as Label;

                HyperLink lnkEditSupp = gr.FindControl("lnkEditSupp") as HyperLink;

                if (chkDelete.Checked)
                {
                    // check supplier has employee

                    SupplierEmployeeRepository objSupplierEmployeeRepository = new SupplierEmployeeRepository();
                    int cnt = objSupplierEmployeeRepository.GetCountSupplierId(Convert.ToInt64(lblID.Text));
                    IsAnySelected = true;

                    if (cnt == 0)
                    {
                        // delete emp
                        objRepo.Delete(Convert.ToInt64(lblID.Text));
                    }
                    else
                    {
                        NotDeleted += lnkEditSupp.Text + ","; 
                    }
                }
            }

            if (IsAnySelected)
            {
                if (string.IsNullOrEmpty(NotDeleted))
                {
                    lblMsg.Text = "Selected Records Deleted Successfully ...";
                }
                else
                {
                    NotDeleted = NotDeleted.Substring(0, NotDeleted.Length - 1);
                    lblMsg.Text = NotDeleted + " can not be deleted as they contains 1 or more employee.";
                }


            }
            else
            {
                lblMsg.Text = "Please Select Record to delete ...";
            }
            
        }
        catch(Exception ex)
        {
            lblMsg.Text = "Error in deleting record ...";
            ErrHandler.WriteError(ex);
        }

        objRepo.SubmitChanges();
        gv.DataBind();
    }


}
