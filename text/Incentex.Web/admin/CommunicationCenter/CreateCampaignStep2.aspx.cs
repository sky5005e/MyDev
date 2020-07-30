using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CommunicationCenter_CreateCampaignStep2 : PageBase
{
    CampignRepo ObjCampRepo = new CampignRepo();
    PagedDataSource pds = new PagedDataSource();
    List<CampignRepo.ExcluideCompanies> listExcluideCompanies = new List<CampignRepo.ExcluideCompanies>();
    CampignRepo.ExcluideCompanies company = new CampignRepo.ExcluideCompanies();
    //IncentexBEDataContext Objdbml;
    int EmailSending = 0;
    int EmailFlaged = 0;
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
    private Int64 CompanyID
    {
        get
        {
            if (ViewState["CompanyID"] == null)
            {
                ViewState["CompanyID"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyID"]);
        }
        set
        {
            ViewState["CompanyID"] = value;
        }
    }
    private Int64 EmployeeID
    {
        get
        {
            if (ViewState["EmployeeID"] == null)
            {
                ViewState["EmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["EmployeeID"]);
        }
        set
        {
            ViewState["EmployeeID"] = value;
        }
    }
    private Int64 UserTypeID
    {
        get
        {
            if (ViewState["UserTypeID"] == null)
            {
                ViewState["UserTypeID"] = 0;
            }
            return Convert.ToInt64(ViewState["UserTypeID"]);
        }
        set
        {
            ViewState["UserTypeID"] = value;
        }
    }
    private bool Isprospects
    {
        get
        {
            if (ViewState["Isprospects"] == null)
            {
                ViewState["Isprospects"] = 0;
            }
            return Convert.ToBoolean(ViewState["Isprospects"]);
        }
        set
        {
            ViewState["Isprospects"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        ((Label)Master.FindControl("lblPageHeading")).Text = "Email Lookup";
        ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
        //((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CreateCampaign.aspx";
        ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CreateCampaign.aspx?campID=" + Session["CampID"].ToString() + "&uid=" + Session["uid"].ToString() + "&Isprospect=" + Session["Isprospect"];
        if (!IsPostBack)
        {
            if (Request.QueryString["cid"] != "0" && !String.IsNullOrEmpty(Request.QueryString["cid"]))
                this.CompanyID = Convert.ToInt64(Request.QueryString["cid"]);


            // set EmpID
            this.EmployeeID = Convert.ToInt64(Session["uid"].ToString());
            // set UserTypeID
            this.UserTypeID = Convert.ToInt64(Session["emptype"].ToString());

            LblEmailSending.Text = Convert.ToString(EmailSending);
            // this is Count of total mail  MailForCount
            Session["MailForCount"] = LblEmailSending.Text;
            LblEmailFlaged.Text = Convert.ToString(EmailFlaged);
            Session["SendMail"] = LblEmailSending.Text.Trim();

            if (Session["ExcludeCompList"] != null)
            {
                string excludeCompanies = Session["ExcludeCompList"].ToString();
                String[] arrayList = excludeCompanies.Split(',').ToArray();
                for (int i = 0; i < arrayList.Length; i++)
                {
                    if (arrayList[i].ToString() != "")
                    {
                        company = new CampignRepo.ExcluideCompanies();
                        company.CompanyID = Convert.ToInt64(arrayList[i].ToString());
                        listExcluideCompanies.Add(company);
                    }
                }
            }
            Bind_GridCampUser();

        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        if (this.CompanyID == 8)
            Response.Redirect("CreateCampaignStep3.aspx?cid=" + this.CompanyID);
        else
            Response.Redirect("CreateCampaignStep3.aspx");
    }
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateCampaign.aspx?campID=" + Session["CampID"].ToString() + "&uid=" + this.EmployeeID.ToString() + "&Isprospect=" + Session["Isprospect"]);
    }

    public void Bind_GridCampUser()
    {

        DataView myDataView = new DataView();
        List<CampignRepo.SearchCampUser> objList;
        if (this.CompanyID == 8)// For Incentex of Vero Beach
            objList = ObjCampRepo.GetIncentexEmp(this.EmployeeID, this.UserTypeID).OrderBy(u => u.FullName).ToList();
        else if (Session["Isprospect"] != null)
            objList = ObjCampRepo.GetWordwideprospects();
        else
            objList = ObjCampRepo.GetCampUserFinal(Convert.ToInt64(Session["cid"]), Convert.ToInt64(Session["did"]), Convert.ToInt64(Session["wid"]), Convert.ToInt64(Session["gid"]), Convert.ToInt64(Session["sid"]), Convert.ToInt64(Session["countryID"].ToString()), Convert.ToInt64(Session["uid"].ToString()), this.UserTypeID, listExcluideCompanies);

        if (objList.Count == 0)
        {
            pagingtable.Visible = false;
            lnkNext.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
            lnkNext.Visible = true;
        }
        DataTable dataTable = Common.ListToDataTable(objList);
        myDataView = dataTable.DefaultView;
        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        // pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.PageSize = 250;
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;

        gvCampaignUser.DataSource = pds;
        gvCampaignUser.DataBind();
        doPaging();
        //Session["Uid"] = Convert.ToInt32(objList[0].UserInfoID);
        Session["EmailCount"] = Convert.ToInt32(objList.Count());
        for (int i = 0; i < objList.Count; i++)
        {
            if (objList[i].MailFlag == false)
            {
                EmailFlaged++;
            }
            else
            {
                EmailSending++;
            }

        }

        // Set label and Session 
        LblEmailSending.Text = Convert.ToString(EmailSending);
        // this is Count of total mail  MailForCount
        Session["MailForCount"] = LblEmailSending.Text;
        LblEmailFlaged.Text = Convert.ToString(EmailFlaged);
        Session["SendMail"] = LblEmailSending.Text.Trim();
        if (objList.Count != 0)
        {
            ObjCampRepo.CountMail(Convert.ToInt32(Session["MailForCount"]), Convert.ToInt32(Session["CampID"]));
        }
    }
    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            Bind_GridCampUser();
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



            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();

        }
        catch (Exception)
        {
        }
    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        Bind_GridCampUser();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        Bind_GridCampUser();
    }
    #endregion

    /// <summary>
    /// in the row command it will convert approve to reject and vise versa. by defalut it will take the values from the db.
    /// first time by defalut all will be true and subscribe campaign
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCampaignUser_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Approve")
        {

            HiddenField hdnMaliFlag;
            ImageButton imgApprove;
            GridViewRow row;
            row = (GridViewRow)((ImageButton)e.CommandSource).Parent.Parent;
            hdnMaliFlag = (HiddenField)(gvCampaignUser.Rows[row.RowIndex].FindControl("hdnMaliFlag"));
            imgApprove = (ImageButton)(gvCampaignUser.Rows[row.RowIndex].FindControl("imgApprove"));
            Label lblUserInfoID = (Label)(gvCampaignUser.Rows[row.RowIndex].FindControl("lblUserInfoID"));
            if (this.CompanyID == 8)// For Incentex of Vero Beach
            {
                IncentexEmployee objInc = new IncentexEmployee();
                IncentexEmployeeRepository ObjIncRepo = new IncentexEmployeeRepository();
                objInc = ObjIncRepo.GetEmployeeByUserInfoId(Convert.ToInt32(lblUserInfoID.Text));
                if (objInc != null)
                    objInc.MailFlag = false;
                ObjIncRepo.SubmitChanges();
            }
            else if ((Session["Isprospect"] != null))
            {
                WorldWideProspect objprospect = new WorldWideProspect();
                WorldwideProspectsRepository objprospectRep = new WorldwideProspectsRepository();
                objprospect = objprospectRep.GetworldwideprospectByID(Convert.ToInt64(lblUserInfoID.Text));

                if (objprospect != null)
                    objprospect.MailFlag = false;

                objprospectRep.SubmitChanges();
            }
            else
            {
                CompanyEmployee ObjCe = new CompanyEmployee();
                CompanyEmployeeRepository objRepo = new CompanyEmployeeRepository();

                objRepo.UpdateStatus(Convert.ToInt32(lblUserInfoID.Text), true);
                objRepo.SubmitChanges();
            }

        }
        if (e.CommandName == "Reject")
        {
            HiddenField hdnMaliFlag;
            ImageButton imgReject;
            GridViewRow row;
            row = (GridViewRow)((ImageButton)e.CommandSource).Parent.Parent;
            hdnMaliFlag = (HiddenField)(gvCampaignUser.Rows[row.RowIndex].FindControl("hdnMaliFlag"));
            imgReject = (ImageButton)(gvCampaignUser.Rows[row.RowIndex].FindControl("imgReject"));
            Label lblUserInfoID = (Label)(gvCampaignUser.Rows[row.RowIndex].FindControl("lblUserInfoID"));

            if (this.CompanyID == 8)// For Incentex of Vero Beach
            {
                IncentexEmployee objInc = new IncentexEmployee();
                IncentexEmployeeRepository ObjIncRepo = new IncentexEmployeeRepository();
                objInc = ObjIncRepo.GetEmployeeByUserInfoId(Convert.ToInt32(lblUserInfoID.Text));
                if (objInc != null)
                    objInc.MailFlag = true;
                ObjIncRepo.SubmitChanges();
            }
            else if ((Session["Isprospect"] != null))
            {
                WorldWideProspect objprospect = new WorldWideProspect();
                WorldwideProspectsRepository objprospectRep = new WorldwideProspectsRepository();

                objprospect = objprospectRep.GetworldwideprospectByID(Convert.ToInt64(lblUserInfoID.Text));
                if (objprospect != null)
                    objprospect.MailFlag = true;

                objprospectRep.SubmitChanges();
            }
            else
            {
                CompanyEmployee ObjCe = new CompanyEmployee();
                CompanyEmployeeRepository objRepo = new CompanyEmployeeRepository();

                objRepo.UpdateStatus(Convert.ToInt32(lblUserInfoID.Text), false);
                objRepo.SubmitChanges();
            }

        }
        Bind_GridCampUser();

    }
    /// <summary>
    /// in row data boubt it will take the value from db and accroding to approva and reject image will display
    /// if user subscribe then it shoes approve else reject
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCampaignUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Session["Isprospect"] != null)
            {
                LinkButton lnklastname = (LinkButton)e.Row.FindControl("lnkbtnLastName");
                if (lnklastname != null)
                    lnklastname.Text = "Company";
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((HiddenField)e.Row.FindControl("hdnMaliFlag")).Value == "False")
            {
                ((ImageButton)e.Row.FindControl("imgApprove")).Visible = false;
            }
            else
            {
                ((ImageButton)e.Row.FindControl("imgReject")).Visible = false;
            }
        }
    }

}
