/* Project Name : Incentex
* Module Name : Store Management 
* Description : Displays all the stores exist thats are related to incentex
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_UserManagement_CompanyStoreView : PageBase
{
    #region Local Property
    PagedDataSource pds = new PagedDataSource();
    #endregion

    #region Page Load Event
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

            //Assign Page Header and return URL 
            ((Label)Master.FindControl("lblPageHeading")).Text = "View Company Store";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";

            //Bind grid function
            bindgrid();
        }
    }
    #endregion

    #region Miscellaneous functions

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

    #endregion

    #region GridView Events

    /// <summary>
    /// Function to bind all the company store which contains also logic for the Paging and sorting
    /// </summary>
    public void bindgrid()
    {
        DataView myDataView = new DataView();
        CompanyStoreRepository objStoreRep = new CompanyStoreRepository();
        List<Incentex.DAL.SqlRepository.CompanyStoreRepository.IECompanyListResults> objList = objStoreRep.GetCompanyStore();
        if (objList.Count == 0)
        {
            pagingtable.Visible = false;
            btnDelete.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
            btnDelete.Visible = true;
        }
        DataTable dataTable = ListToDataTable(objList);
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

        gvCompanyStore.DataSource = pds;
        gvCompanyStore.DataBind();
        doPaging();
    }

    protected void gvCompanyStore_RowCommand(object sender, GridViewCommandEventArgs e)
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
        }

        if (e.CommandName == "EditStore")
        {
            if (Session["ManageID"] == "6")
            {
                Session["ManageID"] = null;
            }
            Session["ManageID"] = 5;
            Response.Redirect("GeneralStoreSetup.aspx?Id=" + e.CommandArgument.ToString());
        }

        if (e.CommandName == "ViewProducts")
        {
            if (Session["ManageID"] == "5")
            {
                Session["ManageID"] = null;
            }
            Session["ManageID"] = 6;
            Session.Remove("ProductExpandedStatuses");
            Response.Redirect("~/admin/CompanyStore/Product/ViewStoreProduct.aspx?Id=" + e.CommandArgument.ToString());
        }
        
         if( e.CommandName == "EmpCradits")
         {
             string CompanyId = e.CommandArgument.ToString();
                    Response.Redirect("~/admin/CompanyStore/EmployeeCredits.aspx?Id=" + CompanyId );
         }
          

        bindgrid();
    }

    protected void gvCompanyStore_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            switch (this.ViewState["SortExp"].ToString())
            {
                case "Company":
                    PlaceHolder placeholderCompanyName = (PlaceHolder)e.Row.FindControl("placeholderCompanyName");
                    break;
                case "CountryName":
                    PlaceHolder placeholderCountry = (PlaceHolder)e.Row.FindControl("placeholderCountry");
                    break;
                case "StoreStatus":
                    PlaceHolder placeholderStatus = (PlaceHolder)e.Row.FindControl("placeholderStatus");
                    break;
              
            }

        }
    }

    #endregion

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindgrid();
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
        bindgrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        bindgrid();
    }
    #endregion

    #region Button Click events

    /// <summary>
    /// Redirect to the new company store page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddCompanyStroe_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/CompanyStore/GeneralStoreSetup.aspx?id=0");
    }

    /// <summary>
    /// Delete button click event
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

            bool IsChecked = false;
            string NotDeleted = "";

            foreach (GridViewRow dsrow in gvCompanyStore.Rows)
            {
                if (((CheckBox)dsrow.FindControl("chkStore")).Checked == true)
                {
                    //Check if any product exists in company store or not
                    StoreProductRepository objStoreProduct = new StoreProductRepository();
                    List<StoreProduct> objStoreProductList = objStoreProduct.GetAllStoreProduct(Convert.ToInt32(((Label)dsrow.FindControl("lblStoreID")).Text));

                    //assign bool
                    IsChecked = true;

                    if (objStoreProductList.Count > 0)
                    {
                        NotDeleted += ((HiddenField)dsrow.FindControl("hfComapnyName")).Value + ",";
                    }

                    else
                    {
                        //Delete from store document table and delete files from the folder too
                        CompanyStoreRepository objStoreRep = new CompanyStoreRepository();
                        Common objCommon = new Common();
                        CompanyStoreDocumentRepository obCompanyStoreDocument = new CompanyStoreDocumentRepository();
                        List<StoreDocument> objStoreList = new List<StoreDocument>();

                        objStoreList = obCompanyStoreDocument.GetAllDocumentsByStore(Convert.ToInt64(((Label)dsrow.FindControl("lblStoreID")).Text));

                        foreach (StoreDocument s in objStoreList)
                        {
                            //delete from store document table
                            obCompanyStoreDocument.DeleteCompanyStoreDocument(s.StoreDocumentID.ToString());

                            //Delete from folder
                            objCommon.DeleteImageFromFolder(s.DocumentName, IncentexGlobal.companystoredocuments);
                        }

                        //delete main company store record
                        objStoreRep.DeleteStore(Convert.ToInt64(((Label)dsrow.FindControl("lblStoreID")).Text));


                    }
                }

            }

            if (IsChecked)
            {
                if (string.IsNullOrEmpty(NotDeleted))
                {
                    lblmsg.Text = "Selected Records Deleted Successfully ...";
                }
                else
                {
                    NotDeleted = NotDeleted.Substring(0, NotDeleted.Length - 1);
                    lblmsg.Text = NotDeleted + " can not be deleted as they contains 1 or more products.";
                }
            }
            else
            {
                lblmsg.Text = "Please Select Record to delete ...";
            }

        }
        catch (Exception ex)
        {
            if (((System.Data.SqlClient.SqlException)(ex)).Number == 547)
            {
                lblmsg.Text = "Unable to delete store as this record is used in other detail table";
            }
            ErrHandler.WriteError(ex);
        }
        bindgrid();
    }

    protected void lnkPrdSearch_Click(object sender, EventArgs e)
    {
        Response.Redirect("CompanyStoreProductSearch.aspx");
    }

    #endregion
}
