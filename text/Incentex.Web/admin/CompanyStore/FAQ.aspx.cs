using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyStore_FAQ : PageBase
{
    #region Local Property
    CompanyStoreDocumentRepository objRep = new CompanyStoreDocumentRepository();
    List<StoreDocument> objStoreDocumentList = new List<StoreDocument>();
    PagedDataSource pds = new PagedDataSource();
    StoreDocument objStoreDocument = new StoreDocument();
    Common objcommon = new Common();
    Int64 CompanyStoreId
    {
        get
        {
            if (ViewState["CompanyStoreId"] == null)
            {
                ViewState["CompanyStoreId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {
            ViewState["CompanyStoreId"] = value;
        }
    }

    Int64 CompanyStoreDocumentId
    {
        get
        {
            if (ViewState["CompanyStoreDocumentId"] == null)
            {
                ViewState["CompanyStoreDocumentId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreDocumentId"]);
        }
        set
        {
            ViewState["CompanyStoreDocumentId"] = value;
        }
    }
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

            if (Request.QueryString.Count > 0)
            {
                this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));

                if (this.CompanyStoreId == 0)
                {
                    Response.Redirect("~/admin/CompanyStore/GeneralStoreSetup.aspx?Id=" + this.CompanyStoreId);
                }

                if (this.CompanyStoreId > 0 && !base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                } 

                //Assign Page Header and return URL 
                ((Label)Master.FindControl("lblPageHeading")).Text = "FAQ";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/CompanyStore/ViewCompanyStore.aspx";
                Session["ManageID"] = 5;

                //Populate Menu function
                menuControl.PopulateMenu(6, 0, this.CompanyStoreId, 0, false);

                BindDropDowns();

                //Bind values function
                getFaqbystore();
            }
        }
    }
    #endregion

    #region Miscellaneous functions

    //bind dropdown for the workgroup, department
    public void BindDropDowns()
    {
        LookupRepository objLookRep = new LookupRepository();

        // For Department
        ddlDepartment.DataSource = objLookRep.GetByLookup("Department");
        ddlDepartment.DataValueField = "iLookupID";
        ddlDepartment.DataTextField = "sLookupName";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Workgroup
        ddlWorkgroup.DataSource = objLookRep.GetByLookup("Workgroup");
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    /// <summary>
    /// Get FAQ Images by specific workgroup
    /// </summary>
    public void getFaqbystore()
    {
        DataView myDataView = new DataView();
        List<CompanyStoreDocumentRepository.FAQExtension> objExtList = objRep.GetAllFAQsByStoreId(this.CompanyStoreId);
        if (objExtList.Count == 0)
        {
            pagingtable.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
        }


        DataTable dataTable = ListToDataTable(objExtList);
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
        dtlFaq.DataSource = pds;
        dtlFaq.DataBind();
        doPaging();
        //objStoreDocumentList = objRep.GetAllGuideLineManuals(this.CompanyStoreId);

    }
    /// <summary>
    /// Convert List to Datatable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
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
    /// Clear Controls
    /// </summary>
    private void ClearControls()
    {
        ddlDepartment.SelectedIndex = 0;
        ddlWorkgroup.SelectedIndex = 0;
        txtAnswer.Value = string.Empty;
        txtQuestion.Value = string.Empty;

    }
    #endregion

    #region DataList Events
    protected void dtlFaq_RowCommand(object sender, GridViewCommandEventArgs e)
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

        if (e.CommandName == "deletedocument")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            GridViewRow row;
            row = (GridViewRow)((ImageButton)e.CommandSource).Parent.Parent;
            objRep.DeleteCompanyStoreDocument(e.CommandArgument.ToString());
            lblmsg.Text = "Record deleted successfully";
        }
        if (e.CommandName == "EditFAQ")
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;

            objStoreDocument = objRep.GetById(Convert.ToInt64(e.CommandArgument.ToString()));
            this.CompanyStoreDocumentId = objStoreDocument.StoreDocumentID;
            ddlWorkgroup.SelectedValue = objStoreDocument.WorkgroupID.ToString();
            ddlDepartment.SelectedValue = objStoreDocument.DepartmentID.ToString();
            txtQuestion.Value = objStoreDocument.FaqQuestion;
            txtAnswer.Value = objStoreDocument.FaqAnswer;


            //e.CommandArgument.ToString();

        }
        getFaqbystore();
    }

    protected void dtlFaq_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            switch (this.ViewState["SortExp"].ToString())
            {
                case "Workgroup":
                    PlaceHolder placeholderWorkgroup = (PlaceHolder)e.Row.FindControl("placeholderWorkgroup");
                    break;
                case "Department":
                    PlaceHolder placeholderDepartment = (PlaceHolder)e.Row.FindControl("placeholderDepartment");
                    break;
                case "FaqQuestion":
                    PlaceHolder placeholderFaqQuestion = (PlaceHolder)e.Row.FindControl("placeholderFaqQuestion");
                    break;
                case "FaqAnswer":
                    PlaceHolder placeholderFaqAnswer = (PlaceHolder)e.Row.FindControl("placeholderFaqAnswer");
                    break;

            }

        }
    }
    #endregion

    #region Button Click events
    protected void lnkBtnUploadFaq_Click(object sender, EventArgs e)
    {
        try
        {
            CompanyEmployeeRepository objCmpEmpRepo = new CompanyEmployeeRepository();
            CompanyEmployee objCmpEmp = new CompanyEmployee();
            List<CompanyStoreDocumentRepository.FAQExtension> objFAQ = new List<CompanyStoreDocumentRepository.FAQExtension>();
            SelectEmployeeStoreIdResult objStoreId = new SelectEmployeeStoreIdResult();

            //objCmpEmp = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
            //objStoreId = objRep.GetEmpStoreId(IncentexGlobal.CurrentMember.UserInfoID, objCmpEmp.WorkgroupID);
            objFAQ = objRep.GetFAQsByWorkgroupId(this.CompanyStoreId, Convert.ToInt64(ddlWorkgroup.SelectedValue));

            if (this.CompanyStoreDocumentId != 0)
            {
                objStoreDocument = objRep.GetById(this.CompanyStoreDocumentId);
            }

            objStoreDocument.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            objStoreDocument.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
            objStoreDocument.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objStoreDocument.CretedDate = System.DateTime.Now;
            objStoreDocument.DocuemntFor = Incentex.DAL.Common.DAEnums.CompanyStoreDocumentFor.FAQ.ToString();
            objStoreDocument.FaqQuestion = txtQuestion.Value;
            objStoreDocument.FaqAnswer = txtAnswer.Value;
            objStoreDocument.StoreId = this.CompanyStoreId;

            if (objFAQ.Count < 20)
            {
                if (this.CompanyStoreDocumentId == 0)
                {
                    objRep.Insert(objStoreDocument);
                }

                objRep.SubmitChanges();


                ClearControls();

                if (this.CompanyStoreDocumentId == 0)
                    lblmsg.Text = "Record added successfully";
                else
                    lblmsg.Text = "Record updated successfully";

                getFaqbystore();
            }
            else
            {
                lblmsg.Text = "Maximum FAQs already added for this Workgroup";
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            //throw ex;
        }
    }

    protected void lnkAddFaq_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        this.CompanyStoreDocumentId = 0;
        ClearControls();
    }
    #endregion

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            getFaqbystore();
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
        getFaqbystore();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        getFaqbystore();
    }
    #endregion

}
