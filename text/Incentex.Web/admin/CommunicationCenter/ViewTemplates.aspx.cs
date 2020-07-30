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

using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;

public partial class admin_CommunicationCenter_ViewTemplates : System.Web.UI.Page
{
    #region Data Members
    EmailTemplete ObjTbl = new EmailTemplete();
    CampignRepo ObjRepo = new CampignRepo();
    Campaign ObjCampTable = new Campaign();
    CampaignNote objcampNote = new CampaignNote();
    NotesHistoryRepository objNoteRepo = new NotesHistoryRepository();
    TodayEmailsRepository objRepo = new TodayEmailsRepository();
    CampignRepo objCampRepo = new CampignRepo();
    private Int64 tempId
    {
        get
        {
            if (ViewState["tempId"] == null)
            {
                ViewState["tempId"] = 0;
            }
            return Convert.ToInt64(ViewState["tempId"]);
        }
        set
        {
            ViewState["tempId"] = value;
        }
    }
    private Int64 userID
    {
        get
        {
            if (ViewState["userID"] == null)
            {
                ViewState["userID"] = 0;
            }
            return Convert.ToInt64(ViewState["userID"]);
        }
        set
        {
            ViewState["userID"] = value;
        }
    }
    private Int64 campId
    {
        get
        {
            if (ViewState["campId"] == null)
            {
                ViewState["campId"] = 0;
            }
            return Convert.ToInt64(ViewState["campId"]);
        }
        set
        {
            ViewState["campId"] = value;
        }
    }
    private Int64 mailID
    {
        get
        {
            if (ViewState["mailID"] == null)
            {
                ViewState["mailID"] = 0;
            }
            return Convert.ToInt64(ViewState["mailID"]);
        }
        set
        {
            ViewState["mailID"] = value;
        }
    }
    private int mailStatus
    {
        get
        {
            if (ViewState["mailStatus"] == null)
            {
                ViewState["mailStatus"] = 0;
            }
            return Convert.ToInt32(ViewState["mailStatus"]);
        }
        set
        {
            ViewState["mailStatus"] = value;
        }
    }

    PagedDataSource pds = new PagedDataSource();
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
    /// <summary>
    /// This id is for Inc_EmailTemplates 
    /// </summary>
    private Int64 iTemplateID
    {
        get
        {
            if (ViewState["iTemplateID"] == null)
            {
                ViewState["iTemplateID"] = 0;
            }
            return Convert.ToInt64(ViewState["iTemplateID"]);
        }
        set
        {
            ViewState["iTemplateID"] = value;
        }
    }
    #endregion 
    #region Events

   

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((Label)Master.FindControl("lblPageHeading")).Text = "Templates";
            if (Request.QueryString["campId"] != "0" && !string.IsNullOrEmpty(Request.QueryString["campId"]))
            {
                this.campId = Convert.ToInt64(Request.QueryString["campId"]);
                this.CompanyID = Convert.ToInt64(Request.QueryString["compID"]);
                this.mailStatus = Convert.ToInt32(Request.QueryString["mailStatus"]);
                trscroll.Visible = false;
                trIframe.Visible = false;
                Bind_GridCampUser();
            }
            else if (Request.QueryString["templateID"] != "0" && !string.IsNullOrEmpty(Request.QueryString["templateID"]))
            {
                this.iTemplateID = Convert.ToInt64(Request.QueryString["templateID"]);
                trscroll.Visible = true;
                trIframe.Visible = false;
                truserDetails.Visible = false;
                GetEmailTemplates();
            }
            else if (Request.QueryString["mailID"] != "0" && !string.IsNullOrEmpty(Request.QueryString["mailID"]) && 
                Request.QueryString["rptop"] != "0" && !string.IsNullOrEmpty(Request.QueryString["rptop"]) 
                && Request.QueryString["rptop"] == "2" || Request.QueryString["rptop"] == "3" )
            {
                this.mailID = Convert.ToInt64(Request.QueryString["mailID"]);
                Int64 uid = Convert.ToInt64(Request.QueryString["uid"]);
                trscroll.Visible = true;
                trIframe.Visible = false;
                truserDetails.Visible = false;
                GetEmailsBodyofOpenedMail(mailID, uid);
            }
            else if (Request.QueryString["mailID"] != "0" && !string.IsNullOrEmpty(Request.QueryString["mailID"]))
            {
                this.mailID = Convert.ToInt64(Request.QueryString["mailID"]);
                trscroll.Visible = true;
                trIframe.Visible = false;
                truserDetails.Visible = false;
               // GetEmailsBodyByMailID(this.mailID);
            }
            else
            {
                truserDetails.Visible = false;
                #region  View Templates
                if (Request.QueryString["tid"] != "0" && !string.IsNullOrEmpty(Request.QueryString["tid"]))
                {
                    this.tempId = Convert.ToInt64(Request.QueryString["tid"]);
                    var temp = (from t in ObjRepo.GetAllTemp()
                                where t.TempID == tempId
                                select t).ToList();
                    foreach (var tm in temp)
                    {
                        string path = "../../UploadedImages/EmailTempletes/" + tm.TempFile;
                        iframe.Attributes.Add("src", path);
                        trIframe.Attributes.Add("style", "height: 650px;");
                    }
                    dvScroll.InnerHtml = String.Empty;
                    if (Request.QueryString["id"] != "0" && !string.IsNullOrEmpty(Request.QueryString["id"]))
                    {
                        this.userID = Convert.ToInt64(Request.QueryString["id"]);
                        var note = (from n in objNoteRepo.GetCampaignDetailsByEmpID(userID)
                                    select n).ToList();
                        string strNoteHistory = String.Empty;
                        foreach (var nc in note)
                        {
                            UserInformationRepository objUserRepo = new UserInformationRepository();
                            UserInformation objUser = objUserRepo.GetById(nc.CreatedBy);

                            if (objUser != null)
                            {
                                strNoteHistory += "Created By : " + objUser.FirstName + " " + objUser.LastName + "\n";
                            }
                            strNoteHistory += "Note : " + nc.Notecontents + "\n";
                            strNoteHistory += "Message : " + nc.MessageBody + "\n";
                            strNoteHistory += "---------------------------------------------------------------------------\n";

                            trIframe.Attributes.Add("style", "height: 350px;");
                            trscroll.Attributes.Add("style", "height: 300px;");
                            trscroll.Visible = true;
                        }
                        dvScroll.InnerHtml = strNoteHistory;
                    }
                    else
                    {
                        trIframe.Attributes.Add("style", "height: 650px;");
                        trscroll.Visible = false;
                    }
                }
                else if (Request.QueryString["id"] != "0" && !string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    this.userID = Convert.ToInt64(Request.QueryString["id"]);
                    var note = (from n in objNoteRepo.GetCampaignDetailsByEmpID(userID)
                                select n).ToList();
                    string strNoteHistory = String.Empty;
                    foreach (var nc in note)
                    {
                        UserInformationRepository objUserRepo = new UserInformationRepository();
                        UserInformation objUser = objUserRepo.GetById(nc.CreatedBy);

                        if (objUser != null)
                        {
                            strNoteHistory += "Created By : " + objUser.FirstName + " " + objUser.LastName + "\n";
                        }
                        strNoteHistory += "Note : " + nc.Notecontents + "\n";
                        strNoteHistory += "Message : " + nc.MessageBody + "\n";
                        strNoteHistory += "---------------------------------------------------------------------------\n";

                        trscroll.Attributes.Add("style", "height: 600px;");
                        trscroll.Visible = true;
                        trIframe.Visible = false;
                    }
                    dvScroll.InnerHtml = strNoteHistory;
                }
                else
                {
                    iframe.Attributes.Add("src", "");
                }

                #endregion Veiw Templates
            }
        }
    }

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

            HiddenField hdnMailFlag;
            ImageButton imgApprove;
            GridViewRow row;
            row = (GridViewRow)((ImageButton)e.CommandSource).Parent.Parent;
            hdnMailFlag = (HiddenField)(gvCampaignUser.Rows[row.RowIndex].FindControl("hdnMailFlag"));
            imgApprove = (ImageButton)(gvCampaignUser.Rows[row.RowIndex].FindControl("imgApprove"));
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
            HiddenField hdnMailFlag;
            ImageButton imgReject;
            GridViewRow row;
            row = (GridViewRow)((ImageButton)e.CommandSource).Parent.Parent;
            hdnMailFlag = (HiddenField)(gvCampaignUser.Rows[row.RowIndex].FindControl("hdnMailFlag"));
            imgReject = (ImageButton)(gvCampaignUser.Rows[row.RowIndex].FindControl("imgReject"));
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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnMailFlag =(HiddenField)e.Row.FindControl("hdnMailFlag");
            if (mailStatus == 0)
            {
                HiddenField hdnCompanyId = (HiddenField)e.Row.FindControl("hdnCompanyId");
                Label lblUserInfoID = (Label)e.Row.FindControl("lblUserInfoID");
                if (hdnCompanyId.Value == "8")// For Incentex of Vero Beach
                {
                    IncentexEmployee objInc = new IncentexEmployee();
                    IncentexEmployeeRepository ObjIncRepo = new IncentexEmployeeRepository();
                    objInc = ObjIncRepo.GetEmployeeByUserInfoId(Convert.ToInt32(lblUserInfoID.Text));
                    if (objInc != null)
                        objInc.MailFlag = false;
                    ObjIncRepo.SubmitChanges();
                    hdnMailFlag.Value = "False";
                }
                else
                {
                    CompanyEmployee ObjCe = new CompanyEmployee();
                    CompanyEmployeeRepository objRepo = new CompanyEmployeeRepository();
                    ObjCe = objRepo.GetByUserInfoId(Convert.ToInt32(lblUserInfoID.Text));
                    if (ObjCe != null)
                        ObjCe.MailFlag = false;
                    objRepo.SubmitChanges();
                    hdnMailFlag.Value = "False";
                }
            }

            if (hdnMailFlag.Value == "False")
            {
                ((ImageButton)e.Row.FindControl("imgApprove")).Visible = false;
            }
            else
            {
                ((ImageButton)e.Row.FindControl("imgReject")).Visible = false;
            }            
        }
    }
    #endregion
    #region Method 

    private void Bind_GridCampUser()
    {
        DataView myDataView = new DataView();
        CampaignMailHistory ObjHistoty = new CampaignMailHistory();
        List<CampignRepo.SearchCampUser> objList = new List<CampignRepo.SearchCampUser>();
        if (mailStatus == 0)
        {
            POP3Client popClient = new POP3Client();
            List<CampignRepo.SearchCampUser> objListDistinct = popClient.GetCampaignBouncedEmailDetails(this.CompanyID, 1, 20);        
            // To get Distinct value from Generic List using LINQ
            // Create an Equality Comprarer Intance
            IEqualityComparer<CampignRepo.SearchCampUser> customComparer = new Common.PropertyComparer<CampignRepo.SearchCampUser>("UserInfoID");
            IEnumerable<CampignRepo.SearchCampUser> distinctUser = objListDistinct.Distinct(customComparer);
            objList = distinctUser.ToList();
        }
        else
            objList = ObjRepo.GetCampMailHistoryUserID(this.campId, this.mailStatus, this.CompanyID);

        if (objList.Count == 0)
        {
            pagingtable.Visible = false;
            lblmsg.Text = "Sorry there is no records found with associate campaign";
        }
        else
        {
            pagingtable.Visible = true;
        }
        DataTable dataTable = Common.ListToDataTable(objList);
        myDataView = dataTable.DefaultView;
        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = 250;
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;

        gvCampaignUser.DataSource = pds;
        gvCampaignUser.DataBind();
        doPaging();
        if (this.mailStatus == 2)
        {
            this.gvCampaignUser.Columns[4].Visible = false;
            this.gvCampaignUser.Columns[5].Visible = false;
        }
        else
        {
            this.gvCampaignUser.Columns[3].Visible = false;
            this.gvCampaignUser.Columns[4].Visible = true;
            this.gvCampaignUser.Columns[5].Visible = true;
        }
       
       
    }

    private void GetEmailTemplates()
    {

        dvScroll.InnerHtml = objRepo.GetEmailTemplatesByID(this.iTemplateID).ToString();
    }

    /// <summary>
    /// To get the Message
    /// </summary>
    /// <param name="mailID"></param>
    /// <param name="UserInfoID"></param>
    private void GetEmailsBodyofOpenedMail(Int64 mailID, Int64 UserInfoID)
    {
        dvScroll.InnerHtml = objCampRepo.GetMessageContentsByID(mailID, UserInfoID).ToString();
    }
    #endregion
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

   
    
}
