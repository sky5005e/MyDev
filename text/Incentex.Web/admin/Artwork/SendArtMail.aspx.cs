using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Artwork_SendArtMail : PageBase
{
    StoreProductImageRepository objRepos = new StoreProductImageRepository();

    Int64 ArtWorkID
    {
        get
        {
            if (ViewState["ArtWorkID"] != null)
                return Convert.ToInt64(ViewState["ArtWorkID"]);
            else
                return 0;
        }
        set
        {
            ViewState["ArtWorkID"] = value;
        }
    }

    String ArtWorkFile
    {
        get
        {
            if (ViewState["ArtWorkFile"] != null)
                return ViewState["ArtWorkFile"].ToString();
            else
                return null;
        }
        set
        {
            ViewState["ArtWorkFile"] = value;
        }
    }

    Int64 StoreProductImageID
    {
        get
        {
            if (ViewState["StoreProductImageID"] != null)
                return Convert.ToInt64(ViewState["StoreProductImageID"]);
            else
                return 0;
        }
        set
        {
            ViewState["StoreProductImageID"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Send Art to mail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "0")
            {
                this.ArtWorkID = Convert.ToInt64(Request.QueryString.Get("id"));
            }
            else if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "0")
            {
                this.ArtWorkID = Convert.ToInt64(Request.QueryString.Get("aid"));
                this.ArtWorkFile = Session["ArtworkFile"].ToString();
            }
            else
            {
                this.StoreProductImageID = Convert.ToInt64(Request.QueryString.Get("ImgID"));
            }
            if (Session["ArtImagrReturnURL"] != null)
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = Session["ArtImagrReturnURL"].ToString();
        }
    }

    protected void lnkBtnSendNow_Click(object sender, EventArgs e)
    {
        if (txtEmailTo.Value != null)
        {
            if (this.StoreProductImageID != null && this.StoreProductImageID != 0)
                SendProductImageMail();
            else if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "0")
            {
                SendArtworkMail();
            }
            else
                SendMail();
        }
    }

    private void SendMail()
    {
        try
        {
            Int64 artworkid = Convert.ToInt64(this.ArtWorkID);
            ArtWorkRepository rp = new ArtWorkRepository();
            CompanyArtwork cartwork = rp.GetById(artworkid);
            //For Send mail
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "ArtWorkImage";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String[] listofaddress = new String[50];

                if (txtEmailTo.Value.Contains("\n"))
                {
                    listofaddress = txtEmailTo.Value.Split(new String[] { "\n" }, StringSplitOptions.None);
                }
                else
                {
                    listofaddress[0] = txtEmailTo.Value;
                }

                foreach (String a in listofaddress)
                {

                    if (!String.IsNullOrEmpty(a))
                    {
                        String sToadd = a;
                        String sSubject = txtSubject.Text;
                        String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                        StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                        messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                        // messagebody.Replace("{FullName}", "User");
                        messagebody.Replace("{Message}", txtAddress.Value.ToString().Replace(Environment.NewLine, "\n"));

                        String smtphost = Application["SMTPHOST"].ToString();
                        Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                        String smtpUserID = Application["SMTPUSERID"].ToString();
                        String smtppassword = Application["SMTPPASSWORD"].ToString();

                        String ext = System.IO.Path.GetExtension(Server.MapPath("~/UploadedImages/Artwork/Large/") + cartwork.LargerImageSName);
                        //Live setting
                        //Boolean result = Common.SendMailWithAttachment(sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, false, true, Server.MapPath("~/UploadedImages/Artwork/Large/") + cartwork.LargerImageSName, smtphost, smtpport, smtpUserID, smtppassword, cartwork.LargerImageOName + ext, true);
                        Boolean result = new CommonMails().SendMailWithAttachment(0, null, sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, false, true, Server.MapPath("~/UploadedImages/Artwork/Thumbs/") + this.ArtWorkFile, smtphost, smtpport, smtpUserID, smtppassword, this.ArtWorkFile, true);
                        if (result)
                        {
                            //success
                        }
                        else
                        {
                            //error
                        }
                    }
                }
                lblMsg.Text = "Mail sent successfully";

                txtAddress.Value = String.Empty;
                txtEmailTo.Value = String.Empty;
                txtSubject.Text = String.Empty;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
            ex = null;
        }
    }

    private void SendArtworkMail()
    {
        try
        {
            Int64 artworkid = Convert.ToInt64(this.ArtWorkID);

            //For Send mail
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "ArtWorkImage";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String[] listofaddress = new String[50];

                if (txtEmailTo.Value.Contains("\n"))
                {
                    listofaddress = txtEmailTo.Value.Split(new String[] { "\n" }, StringSplitOptions.None);
                }
                else
                {
                    listofaddress[0] = txtEmailTo.Value;
                }

                foreach (String a in listofaddress)
                {

                    if (!String.IsNullOrEmpty(a))
                    {
                        String sToadd = a;
                        String sSubject = txtSubject.Text;
                        String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                        StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                        messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                        // messagebody.Replace("{FullName}", "User");
                        messagebody.Replace("{Message}", txtAddress.Value.ToString().Replace(Environment.NewLine, "\n"));

                        String smtphost = Application["SMTPHOST"].ToString();
                        Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                        String smtpUserID = Application["SMTPUSERID"].ToString();
                        String smtppassword = Application["SMTPPASSWORD"].ToString();
                        Boolean result = new CommonMails().SendMailWithAttachment(0, null, sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, false, true, Server.MapPath("~/UploadedImages/Artwork/Thumbs/") + this.ArtWorkFile, smtphost, smtpport, smtpUserID, smtppassword, this.ArtWorkFile, true);
                    }
                }

                lblMsg.Text = "Mail sent successfully";

                txtAddress.Value = String.Empty;
                txtEmailTo.Value = String.Empty;
                txtSubject.Text = String.Empty;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
            ex = null;
        }
    }

    private void SendProductImageMail()
    {
        try
        {
            List<StoreProductImage> objList = objRepos.GetStoreProductImagesByStoreProductImageID(this.StoreProductImageID);
            //For Send mail
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "ArtWorkImage";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String[] listofaddress = new String[50];

                if (txtEmailTo.Value.Contains("\n"))
                {
                    listofaddress = txtEmailTo.Value.Split(new String[] { "\n" }, StringSplitOptions.None);
                }
                else
                {
                    listofaddress[0] = txtEmailTo.Value;
                }

                foreach (String a in listofaddress)
                {

                    if (!String.IsNullOrEmpty(a))
                    {
                        String sToadd = a;
                        String sSubject = txtSubject.Text;
                        String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                        StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                        messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                        // messagebody.Replace("{FullName}", "User");
                        messagebody.Replace("{Message}", txtAddress.Value.ToString().Replace(Environment.NewLine, "\n"));

                        String smtphost = Application["SMTPHOST"].ToString();
                        Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                        String smtpUserID = Application["SMTPUSERID"].ToString();
                        String smtppassword = Application["SMTPPASSWORD"].ToString();

                        //Live server Message

                        String ext = System.IO.Path.GetExtension(Server.MapPath("~/UploadedImages/ProductImages/") + objList[0].LargerProductImage);
                        //Live setting
                        Boolean result = new CommonMails().SendMailWithAttachment(0, null, sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, false, true, Server.MapPath("~/UploadedImages/ProductImages/") + objList[0].LargerProductImage, smtphost, smtpport, smtpUserID, smtppassword, objList[0].LargerProductImage + ext, true);
                    }
                }

                lblMsg.Text = "Mail sent successfully";

                txtAddress.Value = String.Empty;
                txtEmailTo.Value = String.Empty;
                txtSubject.Text = String.Empty;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
            ex = null;
        }
    }
}