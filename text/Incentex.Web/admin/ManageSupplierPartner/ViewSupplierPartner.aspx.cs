using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;

public partial class admin_ManageSupplierPartner_ViewSupplierPartner : PageBase
{
    #region Properties

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

    SupplierPartnerRepo.SupplierPartnerSortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = SupplierPartnerRepo.SupplierPartnerSortExpType.Name;
            }
            return (SupplierPartnerRepo.SupplierPartnerSortExpType)ViewState["SortExp"];
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
            if (ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"] = DAEnums.SortOrderType.Asc;
            }
            return (DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        base.MenuItem = "Manage Supplier Partner";
        base.ParentMenuID = 0;

        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
            base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
        else
            base.SetAccessRights(true, true, true, true);

        if (!base.CanView)
        {
            base.RedirectToUnauthorised();
        }

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Manage Supplier Partner";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";

            // bind grid here   
            BindGird();
        }
    }
    PagedDataSource pds = new PagedDataSource();

    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        Label lblID = e.Row.FindControl("lblID") as Label;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((HiddenField)e.Row.FindControl("hdnStatus")).Value == "True")
            {
                ((ImageButton)e.Row.FindControl("imgReject")).Visible = false;

            }
            else
            {
                ((ImageButton)e.Row.FindControl("imgApprove")).Visible = false;

            }
        }

        //Label lblNoofEmp = e.Row.FindControl("lblNoofEmp") as Label;
        //SupplierEmployeeRepository objRepo = new SupplierEmployeeRepository();
        //lblNoofEmp.Text = objRepo.GetCountSupplierId(Convert.ToInt32(lblID.Text)).ToString();

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
        BindGird();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        gv.PageIndex = CurrentPage;
        BindGird();
    }

    //protected void gv_DataBound(object sender, EventArgs e)


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
                    this.SortExp = (SupplierPartnerRepo.SupplierPartnerSortExpType)Enum.Parse(typeof(SupplierPartnerRepo.SupplierPartnerSortExpType), e.CommandArgument.ToString());
                }
                BindGird();
                break;



            case "Approve":
                {
                    if (!base.CanEdit)
                    {
                        base.RedirectToUnauthorised();
                    }

                    HiddenField hdnStatus;
                    ImageButton imgApprove;
                    GridViewRow row;
                    row = (GridViewRow)((ImageButton)e.CommandSource).Parent.Parent;
                    hdnStatus = (HiddenField)(gv.Rows[row.RowIndex].FindControl("hdnStatus"));
                    imgApprove = (ImageButton)(gv.Rows[row.RowIndex].FindControl("imgApprove"));
                    Label lblUserInfoID = (Label)(gv.Rows[row.RowIndex].FindControl("lblID"));

                    SupplierPartner ObjTbl = new SupplierPartner();
                    SupplierPartnerRepo objRepo = new SupplierPartnerRepo();
                    objRepo.UpdateStatusForSupplier(Convert.ToInt32(lblUserInfoID.Text), true);
                    objRepo.SubmitChanges();
                }
                BindGird();
                break;

            case "Reject":
                {
                    if (!base.CanEdit)
                    {
                        base.RedirectToUnauthorised();
                    }

                    HiddenField hdnStatus;
                    ImageButton imgReject;
                    GridViewRow row;
                    row = (GridViewRow)((ImageButton)e.CommandSource).Parent.Parent;
                    hdnStatus = (HiddenField)(gv.Rows[row.RowIndex].FindControl("hdnStatus"));
                    imgReject = (ImageButton)(gv.Rows[row.RowIndex].FindControl("imgReject"));
                    Label lblUserInfoID = (Label)(gv.Rows[row.RowIndex].FindControl("lblID"));

                    SupplierPartner ObjTbl = new SupplierPartner();
                    SupplierPartnerRepo objRepo = new SupplierPartnerRepo();
                    objRepo.UpdateStatusForSupplier(Convert.ToInt32(lblUserInfoID.Text), false);
                    objRepo.SubmitChanges();

                }
                BindGird();
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
        SupplierPartnerRepo objRepo = new SupplierPartnerRepo();

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

                    //SupplierPartnerRepo objRepo = new SupplierPartnerRepo();
                    int cnt = objRepo.GetCountSupplierId(Convert.ToInt64(lblID.Text));
                    IsAnySelected = true;

                    if (cnt > 0)
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
        catch (Exception ex)
        {
            lblMsg.Text = "Error in deleting record ...";
            ErrHandler.WriteError(ex);
        }

        objRepo.SubmitChanges();
        BindGird();
    }

    protected void lstPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGird();
            //bindGridView();
        }
    }
    public void BindGird()
    {
        SupplierPartnerRepo ObjRepo = new SupplierPartnerRepo();
        List<SupplierPartner> objList = ObjRepo.GetSupplierPartnerList(this.SortExp, this.SortOrder);
        pds.DataSource = objList;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gv.DataSource = pds;
        gv.DataBind();
        doPaging();

    }

    protected void gv_DataBound(object sender, EventArgs e)
    {
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
    }
}
