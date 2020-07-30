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
using Incentex.BE;
using Incentex.DA;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.Mail;
using System.Net;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using System.Net.Mail;

public partial class admin_CommunicationCenter_QuickMsg : PageBase
{
    /* Change here start*/
    Int64 Count
    {
        get
        {
            if (ViewState["Count"] == null)
            {
                ViewState["Count"] = 0;
            }
            return Convert.ToInt64(ViewState["Count"]);
        }
        set
        {
            ViewState["Count"] = value;
        }
    }

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

    private static List<int> ListValues
    {
        get
        {

            if ((HttpContext.Current.Session["ListValues"]) == null)
                return new List<int>(50);
            else
                return ((List<int>)HttpContext.Current.Session["ListValues"]);

        }
        set
        {
            HttpContext.Current.Session["ListValues"] = value;
        }
    }

    public class TrackingNumber
    {
        public string AttachmentName { get; set; }
        public int userinfoid { get; set; }
        public int ordernumber { get; set; }
    }
    private static List<TrackingNumber> ListValuesTracking
    {
        get
        {
            if ((HttpContext.Current.Session["ListValuesTracking"]) == null)
                return new List<TrackingNumber>();
            else
                return (List<TrackingNumber>)HttpContext.Current.Session["ListValuesTracking"];
        }
        set
        {
            HttpContext.Current.Session["ListValuesTracking"] = value;
        }
    }
    /* Change here end*/
    Common objcommm = new Common();
    //DataSet dsEmaiUser, dsEmailTemplate;
    EmailTemplateBE objEmailBE = new EmailTemplateBE();
    EmailTemplateDA objEmailDA = new EmailTemplateDA();
    RegistrationBE objRegistrationBE = new RegistrationBE();
    RegistrationDA objRegistrationDA = new RegistrationDA();
    CampignRepo ObjRepo = new CampignRepo();
    int sizeCount = 0;
    string ext = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((Label)Master.FindControl("lblPageHeading")).Text = "Send Quick Message";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CreateCampaignStep3.aspx";
            if (!IsPostBack)
            {
                lblMessage.Visible = false;

                if (!String.IsNullOrEmpty(Session["FromName"].ToString()) || !String.IsNullOrEmpty(Session["FromEmail"].ToString()))
                {
                    this.FromName = Session["FromName"].ToString();
                    this.FromEmail = Session["FromEmail"].ToString();
                }
            }

            Session["ListValues"] = null;
            Session["Check"] = null;
        }

    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        if (Session["Check"] != null)
        {
            Response.Redirect("CreateCampaignStep5.aspx?qck=1");
        }
        else
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Please send and check a test mail first";
        }
    }
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateCampaignStep3.aspx");
    }

    protected void lnkSendMailForText_Click(object sender, EventArgs e)
    {
        try
        {
            int Textmail = 2;
            Session["Check"] = Textmail;
            //Check if Email is not entered.
            if (txtEmail.Text.Trim() == "")
            {
                lblMessage.Visible = true;
                lblMessage.Text = objcommm.Callvalidationmessage(Server.MapPath("../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "Required", "email address");
                return;
            }
            objRegistrationBE.SEmailAddress = txtEmail.Text.Trim();
            sendVerificationEmail();
        }
        catch (Exception)
        {

            throw;
        }
    }
    private void sendVerificationEmail()
    {

        try
        {


            string sFrmadd = string.Empty;
            if (!String.IsNullOrEmpty(FromEmail))
                sFrmadd = this.FromEmail;
            else
                sFrmadd = "support@world-link.us.com";

            string sToadd = txtEmail.Text.Trim();
            string sSubject = Session["EmailSubject"].ToString();

            string sFrmname = string.Empty;
            if (!String.IsNullOrEmpty(FromName))
                sFrmname = this.FromName;
            else
                sFrmname = "Incentex";
            string FileName = string.Empty;

            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();

            string body = TxtEmailText.Text;
            Session["body"] = TxtEmailText.Text;
            bool result = false;
            List<UploadImage> objImage = new List<UploadImage>();
            if (Session["ListValues"] != null)
            {
                objImage = (List<UploadImage>)Session["ListValues"];

            }
            String FilePath = Server.MapPath("../../UploadedImages/MultipleAttachments/");
           
            if (Session["ListValues"] != null)
            {
                result = new CommonMails().SendMailWithMultiAttachment(0, null, sFrmadd, sToadd, sSubject, body, sFrmname, false, true, objImage, FilePath, smtphost, smtpport, smtpUserID, "smtppassword", ext, true);
            }
            else
            {
                new CommonMails().SendMail(0, null, sFrmadd, sToadd, sSubject, body, sFrmname, smtphost, smtpport, false, true);
            }
            //end for attachment




            if (fpAttachment.Value == "")
            {
                
            }
            lblMessage.Visible = true;
            lblMessage.Text = "Text Mail Send Successfully ";

        }
        catch (Exception ex)
        {
        }

    }
    /* change here*/
    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        
        if (!string.IsNullOrEmpty(fpAttachment.Value))
        {
            lblMessage.Text = "";
            string GallaryPath = "~/UploadedImages/MultipleAttachments";
            string filePath = GallaryPath;

            if (!Directory.Exists(Server.MapPath(filePath)))
                Directory.CreateDirectory(Server.MapPath(filePath));

            HttpPostedFile dealImg = fpAttachment.PostedFile;

            string dealFileName = fpAttachment.Value;
            string dealExtension = Path.GetExtension(dealFileName);
            //dealFileName = Path.GetFileNameWithoutExtension(dealFileName).Trim();
            string savedealfilepath = filePath + "/" + dealFileName;
            // for count the length of content

            sizeCount = sizeCount + (fpAttachment.PostedFile.ContentLength / 1024);
            ViewState["CountSize"] = Convert.ToInt32(ViewState["CountSize"]) + sizeCount;
            if (!string.IsNullOrEmpty(dealFileName))
            {
                dealImg.SaveAs(Server.MapPath(savedealfilepath));
            }

            GenerateTableGrid(savedealfilepath.Replace("~", "."), dealFileName);

        }
        else
        {
            lblMessage.Text = "Please select attachments.";
            lblMessage.Visible = true;
        }
        if (Convert.ToInt64(ViewState["CountSize"]) > 18000)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Content must be less then 18 MB";
        }




    }

    private void GenerateTableGrid(string uploadImagePath, string dealFileName)
    {
        List<UploadImage> obj = new List<UploadImage>();
        UploadImage objnumber = new UploadImage();

        if (Session["ListValues"] != null)
        {
            obj = (List<UploadImage>)Session["ListValues"];
            objnumber.imageName = uploadImagePath;
            objnumber.imageOnly = dealFileName;
            obj.Add(objnumber);
            Session["ListValues"] = obj;
        }
        else
        {
            objnumber.imageName = uploadImagePath;
            objnumber.imageOnly = dealFileName;
            obj.Add(objnumber);
            Session["ListValues"] = obj;
        }

        BindImagesFromSession();
    }

    private void BindImagesFromSession()
    {

        List<UploadImage> obj = new List<UploadImage>();
        if (Session["ListValues"] != null)
        {
            obj = (List<UploadImage>)Session["ListValues"];

            grv.DataSource = obj;
            grv.DataBind();
        }
    }

    protected void grv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //ListValuesTracking.RemoveAt(e.RowIndex);
        //bindgrid();
        List<UploadImage> obj = new List<UploadImage>();
        if (Session["ListValues"] != null)
        {
            obj = (List<UploadImage>)Session["ListValues"];
            obj.RemoveAt(e.RowIndex);
            Session["ListValues"] = obj;

        }
        grv.DataSource = obj;
        grv.DataBind();

    }



}
