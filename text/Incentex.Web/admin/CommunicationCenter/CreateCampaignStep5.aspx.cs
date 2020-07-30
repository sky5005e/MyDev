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
using System.IO;
using Incentex.BE;
using Incentex.DA;
using System.Data.SqlClient;
using System.Text;
using System.Web.Mail;
using System.Net;
using Incentex.DAL;
using Incentex.DAL.Common;
using System.Net.Mail;
using System.Collections.Generic;

public partial class admin_CommunicationCenter_CreateCampaignStep5 : PageBase
{
    CampignRepo ObjCamprerp = new CampignRepo();
    CompanyEmployeeRepository ObjCampRepo = new CompanyEmployeeRepository();
    Common objcommm = new Common();
    Campaign ObjTblCamp = new Campaign();
    CompanyEmployee ObjTblCmpEmp = new CompanyEmployee();
    IncentexEmployee objINC = new IncentexEmployee();
    int MailBouns1 = 0;
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    CampHistory ObjHistoryRepo = new CampHistory();
    IncentexEmployeeRepository objIncEmpRepo = new IncentexEmployeeRepository();

    //DataSet dsEmaiUser, dsEmailTemplate;  
    EmailTemplateBE objEmailBE = new EmailTemplateBE();
    EmailTemplateDA objEmailDA = new EmailTemplateDA();
    string FileName = string.Empty;
    string ext = string.Empty;
    String FromName
    {
        get
        {
            if (ViewState["FromName"] == null)
            {
                ViewState["FromName"] = null;
            }
            return ViewState["FromName"].ToString();
        }
        set
        {
            ViewState["FromName"] = value;
        }
    }
    Boolean IsMailBounced
    {
        get
        {
            if (ViewState["IsMailBounced"] == null)
            {
                ViewState["IsMailBounced"] = 0;
            }
            return (Boolean)ViewState["IsMailBounced"];
        }
        set
        {
            ViewState["IsMailBounced"] = value;
        }
    }
    String FromEmail
    {
        get
        {
            if (ViewState["FromEmail"] == null)
            {
                ViewState["FromEmail"] = null;
            }
            return ViewState["FromEmail"].ToString();
        }
        set
        {
            ViewState["FromEmail"] = value;
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
    CompanyEmployee objEachEmployee = new CompanyEmployee();
    List<CampignRepo.ExcluideCompanies> listExcluideCompanies = new List<CampignRepo.ExcluideCompanies>();
    CampignRepo.ExcluideCompanies company = new CampignRepo.ExcluideCompanies();
    protected void Page_Load(object sender, EventArgs e)
    {
        ((Label)Master.FindControl("lblPageHeading")).Text = "Final Step";
        ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";

        if (Request.QueryString["qck"] == "1")
        {
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CreateCampaignStep3.aspx";
        }
        else
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CreateCampaignStep4.aspx";

        CheckLogin();
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Session["FromName"].ToString()) || !String.IsNullOrEmpty(Session["FromEmail"].ToString()))
            {
                this.FromName = Session["FromName"].ToString();
                this.FromEmail = Session["FromEmail"].ToString();
            }

            if (Session["cid"].ToString() != "0" && !String.IsNullOrEmpty(Session["cid"].ToString()))
                this.CompanyID = Convert.ToInt64(Session["cid"].ToString());
            // check for the html and css templete in mail
            if (Convert.ToInt32(Session["Check"]) == 1)
            {
                WebClient wc = new WebClient();
                //Label1.Text = wc.DownloadString(Server.MapPath(Session["TempName"].ToString()));
                Label1.Text = wc.DownloadString(Server.MapPath("~/UploadedImages/EmailTempletes/") + Session["TempId"].ToString());
            }
            // check for the  text in mail
            if (Convert.ToInt32(Session["Check"]) == 2)
            {
                Label1.Text = Session["body"].ToString();
            }
            if (Session["ExcludeCompList"] != null)
            {
                string excludeCompanies = Session["ExcludeCompList"].ToString();
                String[] arrayList = excludeCompanies.Split(',').ToArray();
                for (int i = 0; i < arrayList.Length; i++)
                {
                    if (arrayList[i].ToString() != "")
                        company = new CampignRepo.ExcluideCompanies();
                    company.CompanyID = Convert.ToInt64(arrayList[i].ToString());
                    listExcluideCompanies.Add(company);
                }
            }
        }



    }
    /// <summary>
    /// Basically it will check two conditions check 1 and check 2 
    /// in first check (check -1) it will check mail is sending with the html or css templet
    /// in second check ( check-2) it will check the mail is sending with quick msg(simple text)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkSendMail_Click(object sender, EventArgs e)
    {
        List<CampignRepo.SearchCampUser> objList;
        if (this.CompanyID == 8)// For Incentex of Vero Beach
            objList = ObjCamprerp.GetIncentexEmp(Convert.ToInt64(Session["uid"].ToString()), Convert.ToInt64(Session["emptype"].ToString()));
        else if (Session["Isprospect"] != null)
            objList = ObjCamprerp.GetWordwideprospects();
        else
            objList = ObjCamprerp.GetCampUserFinal(Convert.ToInt64(Session["cid"]), Convert.ToInt64(Session["did"]), Convert.ToInt64(Session["wid"]), Convert.ToInt64(Session["gid"]), Convert.ToInt64(Session["sid"]), Convert.ToInt64(Session["countryID"].ToString()), Convert.ToInt64(Session["uid"].ToString()), Convert.ToInt64(Session["emptype"].ToString()), listExcluideCompanies);

        //uncomment for live
        var maillist = (from m in objList
                        where m.MailFlag == true
                        select m).ToList();
        CompanyEmployeeRepository objCmpEmpl = new CompanyEmployeeRepository();


        #region Loop
        foreach (CampignRepo.SearchCampUser user in maillist)
        {
            CampaignMailHistory ObjHistory = new CampaignMailHistory();
            try
            {
                Int64? CampaignId = null;
                IsMailBounced = false;
                if (this.CompanyID == 8)
                {
                    objINC = objIncEmpRepo.GetById((long)user.CompanyEmployeeID);
                    objINC.CampaignID = Convert.ToInt32(Session["CampID"]);
                    CampaignId = objINC.CampaignID;
                }
                else if (Session["Isprospect"] != null)
                {
                    CampaignId = Convert.ToInt32(Session["CampID"]);
                }
                else
                {
                    objEachEmployee = objCmpEmpl.GetById((long)user.CompanyEmployeeID);
                    objEachEmployee.CampaignID = Convert.ToInt32(Session["CampID"]);
                    CampaignId = objEachEmployee.CampaignID;
                }
                string sFrmadd = string.Empty;
                if (!String.IsNullOrEmpty(FromEmail))
                    sFrmadd = this.FromEmail;
                else
                    sFrmadd = "support@world-link.us.com";
                string sToadd = user.LoginEmail;
                string sFrmname = string.Empty;
                if (!String.IsNullOrEmpty(FromName))
                    sFrmname = this.FromName;
                else
                    sFrmname = "Incentex";

                string sSubject = Session["EmailSubject"].ToString();
                string smtphost = Application["SMTPHOST"].ToString();
                int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                string smtpUserID = Application["SMTPUSERID"].ToString();
                string smtppassword = Application["SMTPPASSWORD"].ToString();

                string htmlBody = null;
                // this string (strEmailTemp) variable for the maintain the status for mail where  0 -> not send , 1-> Mail sent, 2-> Open the mail,3-> Subscribe,4-> Unsubscribe(mail flag must be false)

                string strEmailTemp = null;
                strEmailTemp = "<table><tr><td>" +
                 "<img width='1' height='1' src='" + ConfigurationManager.AppSettings["siteurl"].ToString() + "admin/CommunicationCenter/Subscribe.aspx" + "?UserinfoID=" + user.UserInfoID + "&CampID=" + CampaignId + "'/>"
                 + "</td></tr></table>";

                string body = Label1.Text + "<br/>" + htmlBody + "<br/>" + strEmailTemp;
                if (Session["FileName"] == null && Session["ext"] == null)
                {
                    FileName = null;
                    ext = null;
                }

                else
                {
                    FileName = Session["FileName"].ToString();
                    ext = Session["ext"].ToString();
                }

                bool result = false;
                //Local Setting
                //result = SendMailWithMulipAttachment(sFrmadd, sToadd, sSubject, body, sFrmname, true, true, (List<UploadImage>)Session["ListValues"], "smtp.gmail.com", 25, "test.soft8@gmail.com", "wdfgh@$9", ext, false);
                List<UploadImage> objImage = new List<UploadImage>();
                if (Session["ListValues"] != null)
                {
                    objImage = (List<UploadImage>)Session["ListValues"];

                }
                String FilePath = Server.MapPath("../../UploadedImages/MultipleAttachments/");

                result = new CommonMails().SendMailWithMultiAttachment(user.UserInfoID, "Campiagn", sFrmadd, sToadd, sSubject, body, sFrmname, false, true, objImage, FilePath, smtphost, smtpport, smtpUserID, smtppassword, ext, true);

                //end for attachment
                // To insert record into CampaignNote
                InsertIntoCampaignNote(user.UserInfoID, body, Convert.ToInt16(Session["CampID"]));
            }
            catch (Exception ex)
            {
                //for the bouns email
                MailBouns1++;
                IsMailBounced = true;
                continue;
                throw;
            }
            finally
            {
                Session["Check"] = null;
                Session["TempId"] = null;

            }
            // insert the data in table of history
            ObjHistory.UserInfoID = Convert.ToInt16(user.UserInfoID);
            ObjHistory.CampID = Convert.ToInt16(Session["CampID"]);
            ObjHistory.ReadDate = DateTime.Now;
            if (IsMailBounced)
                ObjHistory.CampMailStatus = 0;
            else
                ObjHistory.CampMailStatus = 1;
            // making object of repo and insert in History table it.
            ObjHistoryRepo.Insert(ObjHistory);
            ObjHistoryRepo.SubmitChanges();

        }
        #endregion Loop
        try
        {
            objCmpEmpl.SubmitChanges();
            //for the set the total mail of Campaign table 
            CampignRepo ObjCampRepo = new CampignRepo();
            ObjCampRepo.SetMailCount(Convert.ToInt32(Session["CampID"]), Convert.ToInt32(Session["MailForCount"]));
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //finally
        //{
        //    String path = Server.MapPath("../../UploadedImages/MultipleAttachments/");
        //    if (Session["ListValues"] != null)
        //    {
        //        List<UploadImage> obj1 = (List<UploadImage>)Session["ListValues"];
        //        for (int i = 0; i < obj1.Count; i++)
        //            new Common().DeleteImageFromFolder(obj1[i].imageOnly, path);

        //    }
        //    Response.Redirect("DisplayMailingList.aspx");
        //}
        Response.Redirect("DisplayMailingList.aspx");



    }


    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["qck"] == "1")
            Response.Redirect("CreateCampaignStep3.aspx");
        else
            Response.Redirect("CreateCampaignStep4.aspx");
    }

    /// <summary>
    /// To Insert Records in Campaign Notes Table
    /// </summary>
    /// <param name="userID"></param>
    private void InsertIntoCampaignNote(Int64 userID, String body, int campID)
    {
        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        StringBuilder strb = new StringBuilder();
        strb.Append("Campaign Name: " + Session["CampName"].ToString() + System.Environment.NewLine);
        strb.Append("Date & Time: " + Convert.ToDateTime(Session["CampCreateDate"]) + System.Environment.NewLine);
        strb.Append("Email Subject: " + Session["EmailSubject"].ToString());

        CampaignNote objNote = new CampaignNote();
        objNote.Notecontents = strb.ToString();
        objNote.CampaignID = campID;
        objNote.UserInfoID = userID;
        objNote.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objNote.CreateDate = DateTime.Now;
        objNote.MessageBody = body;
        if (Request.QueryString["qck"] == "1")
            objNote.TempID = null;
        else
            objNote.TempID = Convert.ToInt32(Session["TemplateID"].ToString());
        objRepo.Insert(objNote);
        objRepo.SubmitChanges();
        strb.Remove(0, strb.Length);
    }

}

