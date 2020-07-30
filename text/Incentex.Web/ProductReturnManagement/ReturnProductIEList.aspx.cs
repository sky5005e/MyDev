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
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Collections.Generic;
using commonlib.Common;
using System.Text;
using Incentex.BE;
using Incentex.DA;

public partial class ProductReturnManagement_ReturnProductIEList : PageBase
{
    #region Data Members
    string RANumber = null;
    string FirstName = null;
    string LastName = null;
    DateTime? FromDate = null;
    DateTime? ToDate = null;
    Int64 TotalCount = 0;
    PagedDataSource pds = new PagedDataSource();
    ProductReturnRepository objRepos = new ProductReturnRepository();
    #endregion

    #region Page Event's
    protected void Page_Load(object sender, EventArgs e)
    {   
        if (!IsPostBack)
        {
            base.MenuItem = "Process Incoming Returns";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Return View";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/ProductReturnManagement/ProductItemsReturnSearch.aspx";
            // Create session for Absolute URL 
            Session["ToFroURL"] = Request.Url.AbsoluteUri;
            if (Request.QueryString["RANumber"] != null && Request.QueryString["RANumber"] != "")
            {
                RANumber = Request.QueryString["RANumber"].ToString();
            }
            if (Request.QueryString["FirstName"] != "" && Request.QueryString["FirstName"] != null)
            {
                FirstName = Request.QueryString["FirstName"];
            }
            if (Request.QueryString["LastName"] != "" && Request.QueryString["LastName"] != null)
            {
                LastName = Request.QueryString["LastName"].ToString();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);

            if (!string.IsNullOrEmpty(Request.QueryString["FromDate"]))
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
                BindGridview();

            lblCount.Text = String.Format("Total Count : {0}", TotalCount.ToString());
        }
    }
    #endregion

    #region Page Method's
    private void BindGridview()
    {
        try
        {
            List<ProductReturnRepository.ReturnProductItemsDetails> objListDistinct = new List<ProductReturnRepository.ReturnProductItemsDetails>();
            objListDistinct = objRepos.GetReturnProductList(RANumber, FirstName, LastName, FromDate, ToDate);
            
            // To get Distinct value from Generic List using LINQ
            // Create an Equality Comprarer Intance
            IEqualityComparer<ProductReturnRepository.ReturnProductItemsDetails> customComparer = new Common.PropertyComparer<ProductReturnRepository.ReturnProductItemsDetails>("OrderNumber");
            IEnumerable<ProductReturnRepository.ReturnProductItemsDetails> objList = objListDistinct.Distinct(customComparer); 
            if (objList.Count()> 0)
            {

                TotalCount = objList.Count();

                gvProductReturn.DataSource = objList;
                gvProductReturn.DataBind();
                pds.DataSource = objList;
                pds.AllowPaging = true;
                pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
                pds.CurrentPageIndex = CurrentPage;
                lnkbtnNext.Enabled = !pds.IsLastPage;
                lnkbtnPrevious.Enabled = !pds.IsFirstPage;
                gvProductReturn.DataSource = pds;
                gvProductReturn.DataBind();

                if (gvProductReturn.Rows.Count == 0)
                {                    
                    pagingtable.Visible = false;                 
                    lblmsg.Text = "No records found for given criteria.";
                }
                else
                {
                    pagingtable.Visible = true;                    
                    doPaging();
                }
            }
            else
            {
                pagingtable.Visible = false;
                string myStringVariable = string.Empty;
                myStringVariable = "No Record Found!";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                //btnSaveStatus.Visible = false;
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

  
    #endregion

    #region Grid Events
    protected void gvProductReturn_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((HyperLink)e.Row.FindControl("hypOrderNumber")).NavigateUrl = "~/ProductReturnManagement/ProductItemsReturnIEDetails.aspx?OrderId=" + ((HiddenField)e.Row.FindControl("hdnOrderID")).Value.ToString() + "&Status=" + ((HiddenField)e.Row.FindControl("hdnStatus")).Value.ToString() + "&RA=" + ((HiddenField)e.Row.FindControl("hdnOrderNumber")).Value.ToString();
        }
    }
    #endregion

    #region Pagging Methods

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
        catch (Exception)
        { }

    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
       
            
        
            BindGridview();
      
    }

    /// <summary>
    /// Called Next record on the basic of
    /// No of paging.    
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;       
            BindGridview();
    }

    protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGridview();
        }
    }

    /// <summary>
    /// lnkbtnPaging of paging button is enable false     
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
    
    #endregion
}
