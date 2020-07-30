using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Incentex.DAL.SqlRepository;
using System.IO;

public partial class admin_PurchaseOrderManagement_PurchaseOrderList : PageBase
{
    #region Data Member's
    long VendorID
    {
        get
        {
            if (ViewState["VendorID"] != null)
                return Convert.ToInt64(ViewState["VendorID"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["VendorID"] = value;
        }
    }
    long MasterItemID
    {
        get
        {
            if (ViewState["MasterItemID"] != null)
                return Convert.ToInt64(ViewState["MasterItemID"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["MasterItemID"] = value;
        }
    }
    string OrderNumber
    {
        get
        {
            if (ViewState["OrderNumber"] != null)
                return ViewState["OrderNumber"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["OrderNumber"] = value;
        }
    }
    Boolean IsTodayFollowup
    {
        get
        {
            if (ViewState["IsTodayFollowup"] != null)
                return Convert.ToBoolean(ViewState["IsTodayFollowup"].ToString());
            else
                return false;
        }
        set
        {
            ViewState["IsTodayFollowup"] = value;
        }
    }

    PagedDataSource pds = new PagedDataSource();
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

    PurchaseOrderManagmentRepository objOrderRep = new PurchaseOrderManagmentRepository();
    #endregion

    #region Event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Purchase Order Management";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";


            if (!String.IsNullOrEmpty(Request.QueryString["VendorID"]))
                this.VendorID = Convert.ToInt64(Request.QueryString["VendorID"]);
            if (!String.IsNullOrEmpty(Request.QueryString["MasterItemID"]))
                this.MasterItemID = Convert.ToInt64(Request.QueryString["MasterItemID"]);
            if (!String.IsNullOrEmpty(Request.QueryString["OrderNumber"]))
                this.OrderNumber = Convert.ToString(Request.QueryString["OrderNumber"]);
            if (!string.IsNullOrEmpty(Request.QueryString["IsTodayFollowup"]))
                this.IsTodayFollowup = Convert.ToBoolean(Request.QueryString["IsTodayFollowup"]);

            if (!IsTodayFollowup)
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/PurchaseOrderManagement/SearchPurchaseOrders.aspx";
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/PurchaseOrderManagement/PurchaseOrderManagement.aspx";

            ((Label)Master.FindControl("lblPageHeading")).Text = IsTodayFollowup ? "Today’s Follow Up" : "Purchase Order Management";


            BindGridView(IsTodayFollowup);
        }
    }

    private void BindGridView(Boolean IsTodayFollowUp)
    {
        DataView myDataView = new DataView();
        List<SearchPurchaseOrder> list = new List<SearchPurchaseOrder>();

          

        if (IsTodayFollowup)
            list = objOrderRep.GetTodayFollowUpDetails(IncentexGlobal.CurrentMember.UserInfoID);
        else
            list = objOrderRep.GetPurchaseOrderDetailsBySearch(this.VendorID, this.MasterItemID, this.OrderNumber);

        if (list.Count == 0)
        {
            pagingtable.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
        }


        DataTable dataTable = Common.ListToDataTable(list);
        myDataView = dataTable.DefaultView;
        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }

        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = 50; //Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;

        grdView.DataSource = pds;
        grdView.DataBind();

        doPaging();
    }

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGridView(IsTodayFollowup);
        }
    }
    protected void dtlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
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
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindGridView(IsTodayFollowup);
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindGridView(IsTodayFollowup);
    }
    #endregion

    public void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Label lblFileSize = (Label)e.Row.FindControl("lblFileSize");
        HiddenField hdnextension = (HiddenField)e.Row.FindControl("hdnextension");
        System.Web.UI.HtmlControls.HtmlImage viewimg = (System.Web.UI.HtmlControls.HtmlImage)e.Row.FindControl("viewimg");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (hdnextension.Value.ToLower() == ".pdf")
                viewimg.Src = "~/Images/FileType/pdf.png";
            else if (hdnextension.Value.ToLower() == ".jpg" || hdnextension.Value.ToLower() == ".jpeg")
                viewimg.Src = "~/Images/FileType/jpg.png";
            else if (hdnextension.Value.ToLower() == ".doc" || hdnextension.Value.ToLower() == ".xls" || hdnextension.Value.ToLower() == ".xlsx")
                viewimg.Src = "~/Images/FileType/doc.png";
        }
    }

    public void grdView_RowCommand(object sender, GridViewCommandEventArgs e)
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

            BindGridView(IsTodayFollowup);
        }
        else if (e.CommandName == "Deleteorder")
        {
            objOrderRep.DeletePurchaseOrder(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);
            BindGridView(IsTodayFollowup);
        }
        else if (e.CommandName == "ViewOrder")
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            HiddenField hdnfilename = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnfilename"));
            HiddenField hdnOriginalFileName = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnOriginalFileName"));
            string filepath = Server.MapPath("~/UploadedImages/PurchaseOrderManagement/") + hdnfilename.Value;

            Common.DownloadFile(filepath, hdnOriginalFileName.Value);
        }
        else if (e.CommandName == "ViewOrderDetails")
        {
            Response.Redirect("PurchaseOrderDetails.aspx?PurchaseOrderID=" + Convert.ToInt64(e.CommandArgument) + "&IsTodayFollowup=" + IsTodayFollowup + "&vendorID=" + VendorID + "&MasterItemID=" + MasterItemID + "&OrderNumber=" + this.OrderNumber);
        }
    }
    #endregion
}
