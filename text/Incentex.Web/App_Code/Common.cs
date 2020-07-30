using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using System.Configuration;
using System.Data;
using System.Net;
using System.Xml.Linq;
using System.Xml;
using System.Web.UI;
using System.Reflection;
using System.ComponentModel;
using Incentex.BE;
using Incentex.DA;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for Common
/// </summary>
public class Common
{
    public Common()
    {
    }

    public static string GetString(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return "";
        }
        return s;
    }

    #region Properties

    public static string SupplierDocumentUploadPath
    {
        get
        {
            string path = ConfigurationManager.AppSettings["SupplierDocumentPath"];
            string UploadPath = HttpContext.Current.Server.MapPath(path);
            return UploadPath;
        }
    }

    public static string ServieTicketAttachmentPath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ServieTicketAttachementPath"]);
        }
    }

    public static string SAPOrderFailedResponsePath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["SAPOrderFailedResponsePath"]);
        }
    }

    public static string SAPUpdateOrderFailedRequestPath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["SAPUpdateOrderFailedRequestPath"]);
        }
    }

    public static string SAPUserFailedResponsePath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["SAPUserFailedResponsePath"]);
        }
    }

    public static string UserManagementTemplatePath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["UserManagementTemplates"]);
        }
    }

    public static string UserManagementTempPath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["UserManagementTemp"]);
        }
    }

    public static string StrikeIronResponsePath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["StrikeIronResponsePath"]);
        }
    }

    public static string CoupaOrderRequestPath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CoupaOrderRequestPath"]);
        }
    }

    public static string DownloadedPAASOrderPath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DownloadedPAASOrderPath"]);
        }
    }

    public static string IncentexEmployeeDocumentPath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["IncentexEmployeeDocumentPath"]);
        }
    }

    public static string SupplierEmployeeDocumentPath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["SupplierEmployeeDocumentPath"]);
        }
    }
    public static Incentex.DAL.Common.DAEnums.ManageID? MenuModuleId
    {
        get
        {
            return (Incentex.DAL.Common.DAEnums.ManageID)HttpContext.Current.Session["ManageID"];
        }
        set
        {
            if (value != null)
            {
                HttpContext.Current.Session["ManageID"] = (int)value;
            }
            else
            {
                HttpContext.Current.Session["ManageID"] = null;
            }
        }
    }

    #region Support Ticket Email Properties

    public static Boolean Live
    {
        get
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["SupportTicketEmailEnvironmentLive"]);
        }
    }

    public static Boolean SSL
    {
        get
        {
            return Live ? false : true;
        }
    }

    public static String UserName
    {
        get
        {
            return Live ? "notifications@world-link.us.com" : "notifications.incentex@gmail.com";
        }
    }

    public static String Password
    {
        get
        {
            return Live ? "5rdxZSE$" : "Notify-!@#";
        }
    }

    public static String SMTPHost
    {
        get
        {
            return Live ? "127.0.0.1" : "smtp.gmail.com";
        }
    }

    public static String IMAPHost
    {
        get
        {
            return Live ? "127.0.0.1" : "imap.gmail.com";
        }
    }

    public static String POPHost
    {
        get
        {
            return Live ? "127.0.0.1" : "pop.gmail.com";
        }
    }

    public static Int32 SMTPPort
    {
        get
        {
            return 25;
        }
    }

    public static Int32 IMAPPort
    {
        get
        {
            return Live ? 143 : 993;
        }
    }

    public static Int32 POPPort
    {
        get
        {
            return Live ? 110 : 995;
        }
    }

    public static String ReplyTo
    {
        get
        {
            return Live ? "replyto@world-link.us.com" : "smtp.incentex@gmail.com";
        }
    }

    public static String ReplyToPassword
    {
        get
        {
            return Live ? "7940bhoh" : "smtp@incentex";
        }
    }

    public static String OpenTicket
    {
        get
        {
            return Live ? "ticket@world-link.us.com" : "ticket.incentex@gmail.com";
        }
    }

    public static String OpenTicketPassword
    {
        get
        {
            return Live ? "sv4#EHTW" : "ticket@incentex";
        }
    }

    public static String EmailFrom
    {
        get
        {
            return Live ? "notifications@world-link.us.com" : "notifications.incentex@gmail.com";

        }
    }

    public static String DisplyName
    {
        get
        {
            return Live ? "Incentex" : "Incentex Test";
        }
    }

    #endregion

    public static readonly string Format1 = "MM/dd/yyyy";

    public static readonly string[] DateFormats = { Format1 };

    public static string SAPInsertItemFailedResponsePath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["SAPInsertItemFailedResponsePath"]);
        }
    }

    public static string SAPUpdateItemFailedResponsePath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["SAPUpdateItemFailedResponsePath"]);
        }
    }
    #endregion

    #region Send Mail

    public static bool SendMail(string psFromAddress, string psToAddress, string psSubject, string psMessageBody, string Fromname
        , bool SSL, bool HTMLBody)
    {

        bool Result = false;
        try
        {
            MailMessage oMailMessage = new MailMessage();
            SmtpClient smtpmail = new SmtpClient();
            MailAddress FromAddress = default(MailAddress);

            if (string.IsNullOrEmpty(Fromname))
            {
                FromAddress = new MailAddress(psFromAddress);
            }
            else
            {
                FromAddress = new MailAddress(psFromAddress, Fromname);
            }

            oMailMessage.To.Add("lbowman@incentex.com");
            //oMailMessage.To.Add(psToAddress);

            oMailMessage.From = FromAddress;
            oMailMessage.Subject = "Test Environment : " + psSubject;
            oMailMessage.IsBodyHtml = HTMLBody;
            oMailMessage.Body = psMessageBody;
            oMailMessage.Priority = MailPriority.High;

            smtpmail.EnableSsl = SSL;

            smtpmail.Send(oMailMessage);

            Result = true;

        }
        catch (Exception ex)
        {

            ex = null;

            Result = false;
        }


        return Result;
    }

    /// <summary>
    /// Function for sending email with reply to address different than from address.
    /// </summary>
    /// <param name="psFromAddress">
    /// email from address
    /// </param>
    /// <param name="psToAddress">
    /// email to address (separated by ',' or ';' when multiple recipients)
    /// </param>
    /// <param name="psSubject">
    /// email subject
    /// </param>
    /// <param name="psMessageBody">
    /// email body
    /// </param>
    /// <param name="Fromname">
    /// email from name
    /// </param>
    /// <param name="psReplyToAddress">
    /// email reply to address
    /// </param>
    /// <param name="psUserName">
    /// email credenital username
    /// </param>
    /// <param name="psPassword">
    /// email credenital password
    /// </param>
    /// <param name="psHost">
    /// email host
    /// </param>
    /// <param name="psPort">
    /// email port number
    /// </param>
    /// <param name="psPriority">
    /// email priority
    /// </param>
    /// <param name="SSL">
    /// email SSL (secure socket layer - true or false)
    /// </param>
    /// <param name="HTMLBody">
    /// email IsBodyHTML (true for html / false for plain text)
    /// </param>
    /// <returns>
    /// boolean success (true for success / false for failure)
    /// </returns>
    public Boolean SendMailWithReplyTo(String psFromAddress, String psToAddress, String psSubject, String psMessageBody, String Fromname,
        String psReplyToAddress, String psUserName, String psPassword, String psHost, String psPort, String psPriority, Boolean SSL, Boolean HTMLBody)
    {
        Boolean Result = false;
        try
        {
            MailMessage oMailMessage = new MailMessage();
            SmtpClient smtpmail = new SmtpClient();

            if (String.IsNullOrEmpty(Fromname))
            {
                oMailMessage.From = new MailAddress(psFromAddress);
            }
            else
            {
                oMailMessage.From = new MailAddress(psFromAddress, Fromname);
            }

            oMailMessage.To.Add("lbowman@incentex.com");

            //if (!String.IsNullOrEmpty(psToAddress))
            //{
            //    if (psToAddress.Contains(',') || psToAddress.Contains(';'))
            //    {
            //        if (psToAddress.Contains(','))
            //        {
            //            foreach (String ToAddress in psToAddress.Split(','))
            //            {
            //                oMailMessage.To.Add(ToAddress);
            //            }
            //        }
            //        else if (psToAddress.Contains(';'))
            //        {
            //            foreach (String ToAddress in psToAddress.Split(';'))
            //            {
            //                oMailMessage.To.Add(ToAddress);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        oMailMessage.To.Add(psToAddress);
            //    }
            //}

            if (!String.IsNullOrEmpty(psReplyToAddress))
            {
                oMailMessage.ReplyTo = new MailAddress(psReplyToAddress);
            }

            oMailMessage.Subject = "Test Environment : " + psSubject;
            oMailMessage.IsBodyHtml = HTMLBody;
            oMailMessage.Body = psMessageBody;

            switch (psPriority)
            {
                case "High":
                    oMailMessage.Priority = MailPriority.High;
                    break;
                case "Normal":
                    oMailMessage.Priority = MailPriority.Normal;
                    break;
                case "Low":
                    oMailMessage.Priority = MailPriority.Low;
                    break;
                default:
                    oMailMessage.Priority = MailPriority.Normal;
                    break;
            }

            smtpmail.Credentials = new NetworkCredential(psUserName, psPassword);
            smtpmail.Host = psHost;

            if (!String.IsNullOrEmpty(psPort))
            {
                smtpmail.Port = Convert.ToInt32(psPort);
            }
            else
            {
                smtpmail.Port = 25;
            }

            smtpmail.EnableSsl = SSL;

            smtpmail.Send(oMailMessage);

            Result = true;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return Result;
    }

    public Boolean SendServiceTicketReciept(Int64 TicketID, String UserInfoID, Int32 NoteType, String To, String Body, String Host, String Port, String Subject)
    {
        Boolean Success = false;
        #region Send Receipt
        try
        {
            Subject += " - Support Ticket - " + TicketID;

            if (String.IsNullOrEmpty(UserInfoID))
            {
                UserInfoID = "annx";
            }

            String OriginalReplyTo = Common.ReplyTo;

            String From = String.Empty;
            String FromName = String.Empty;
            String ReplyTo = String.Empty;
            String UserName = String.Empty;
            String Password = String.Empty;
            String Priority = String.Empty;
            Boolean SSL;
            Boolean IsHTML;

            From = EmailFrom;
            FromName = "Incentex";
            ReplyTo = OriginalReplyTo.Substring(0, OriginalReplyTo.IndexOf('@')) + "+tn" + TicketID + "un" + UserInfoID + "nt" + NoteType + "en" + OriginalReplyTo.Substring(OriginalReplyTo.IndexOf('@'), OriginalReplyTo.Length - OriginalReplyTo.IndexOf('@'));

            UserName = Common.UserName;
            Password = Common.Password;
            Priority = "Normal";
            SSL = Common.SSL;
            Host = Common.SMTPHost;
            Port = Convert.ToString(Common.SMTPPort);

            IsHTML = true;

            Common objcommon = new Common();
            Success = objcommon.SendMailWithReplyTo(From, To, Subject, Body, FromName, ReplyTo, UserName, Password, Host, Port, Priority, SSL, IsHTML);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        #endregion

        return Success;
    }

    public static bool SendMailWithAttachment(string psFromAddress, string psToAddress, string psSubject, string psMessageBody, string Fromname
       , bool SSL, bool HTMLBody, string FilePath, string Host, Int32 Port, string uName, string Pwd, string OFileName, bool IsForLive)
    {

        bool Result = false;
        try
        {
            MailMessage oMailMessage = new MailMessage();
            SmtpClient smtpmail = new SmtpClient();
            if (!IsForLive)
            {
                NetworkCredential objNet = new NetworkCredential(uName, Pwd);
                smtpmail.Credentials = objNet;
            }
            smtpmail.Host = Host;
            smtpmail.Port = Port;
            smtpmail.Timeout = 200000;


            MailAddress FromAddress = default(MailAddress);
            if (FilePath != null)
            {
                Attachment objAttach = new Attachment(FilePath);
                System.Net.Mime.ContentDisposition obj = objAttach.ContentDisposition;
                obj.FileName = OFileName;
                oMailMessage.Attachments.Add(objAttach);
            }



            if (string.IsNullOrEmpty(Fromname))
            {
                FromAddress = new MailAddress(psFromAddress);
            }
            else
            {
                FromAddress = new MailAddress(psFromAddress, Fromname);
            }

            oMailMessage.To.Add("lbowman@incentex.com");
            //oMailMessage.To.Add(psToAddress);

            oMailMessage.From = FromAddress;
            oMailMessage.Subject = "Test Environment : " + psSubject;
            oMailMessage.IsBodyHtml = HTMLBody;
            oMailMessage.Body = psMessageBody;
            oMailMessage.Priority = MailPriority.High;

            smtpmail.EnableSsl = SSL;

            smtpmail.Send(oMailMessage);

            Result = true;

        }
        catch (Exception ex)
        {
            ex = null;
            Result = false;
        }
        return Result;
    }

    #endregion

    #region Encryption,Decryption

    public static string Encryption(string psSource)
    {
        string original = null;
        string encrypted = null;
        string decrypted = null;
        string password = null;
        TripleDESCryptoServiceProvider des = default(TripleDESCryptoServiceProvider);
        MD5CryptoServiceProvider hashmd5 = default(MD5CryptoServiceProvider);
        byte[] pwdhash = null;
        byte[] buff = null;

        password = "secretpassword1!";

        original = psSource;


        hashmd5 = new MD5CryptoServiceProvider();
        pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
        hashmd5 = null;


        des = new TripleDESCryptoServiceProvider();


        des.Key = pwdhash;


        des.Mode = CipherMode.ECB;



        buff = ASCIIEncoding.ASCII.GetBytes(original);
        encrypted = Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
        return encrypted;
    }

    public static string Decryption(string pSource)
    {
        TripleDESCryptoServiceProvider des = default(TripleDESCryptoServiceProvider);
        des = new TripleDESCryptoServiceProvider();
        string original = null;
        string encrypted = null;
        string decrypted = null;
        string password = null;

        MD5CryptoServiceProvider hashmd5 = default(MD5CryptoServiceProvider);
        byte[] pwdhash = null;
        byte[] buff = null;

        password = "secretpassword1!";

        original = pSource;


        hashmd5 = new MD5CryptoServiceProvider();
        pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
        hashmd5 = null;


        des = new TripleDESCryptoServiceProvider();


        des.Key = pwdhash;


        des.Mode = CipherMode.ECB;


        buff = Convert.FromBase64String(pSource);
        decrypted = ASCIIEncoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));


        return decrypted;
    }
    #endregion

    #region Image

    public void SaveImage(HttpPostedFile pfFileInput, string psFilePathOut, int piTargetWidth, int piTargetHeight)
    {
        System.Drawing.Image original_image = null;
        System.Drawing.Bitmap final_image = null;
        System.Drawing.Graphics graphic = null;

        try
        {
            //code to upload images into created directory
            HttpPostedFile jpeg_image_upload = pfFileInput;

            // Retrieve the uploaded image
            //System.IO.FileStream fl = new FileStream("", FileMode.Create);
            //original_image = System.Drawing.Image.FromStream(fl);
            original_image = System.Drawing.Image.FromStream(jpeg_image_upload.InputStream);

            // Calculate the new width and height
            int width = original_image.Width;
            int height = original_image.Height;
            int target_width = piTargetWidth;
            int target_height = piTargetHeight;
            int new_width, new_height;

            if (height <= target_height && width <= target_width)
            {
                new_height = height;
                new_width = width;
            }
            else
            {
                float target_ratio = (float)target_width / (float)target_height;
                float image_ratio = (float)width / (float)height;

                if (target_ratio > image_ratio)
                {
                    new_height = target_height;
                    new_width = (int)Math.Floor(image_ratio * (float)target_height);
                }
                else
                {
                    new_height = (int)Math.Floor((float)target_width / image_ratio);
                    new_width = target_width;
                }

                new_width = new_width > target_width ? target_width : new_width;
                new_height = new_height > target_height ? target_height : new_height;
            }
            // Create the thumbnail


            final_image = new System.Drawing.Bitmap(new_width, new_height);
            graphic = System.Drawing.Graphics.FromImage(final_image);
            graphic.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Transparent), new System.Drawing.Rectangle(0, 0, target_width, target_height));
            int paste_x = (target_width - new_width) / 2;
            int paste_y = (target_height - new_height) / 2;
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; /* new way */
            //graphic.DrawImage(thumbnail_image, paste_x, paste_y, new_width, new_height);
            graphic.DrawImage(original_image, 0, 0, new_width, new_height);
            //saving image physically

            final_image.Save(psFilePathOut, GetImageFormat(pfFileInput.FileName));
        }
        catch (Exception ex)
        {

            throw ex;

        }

        finally
        {
            if (final_image != null) final_image.Dispose();
            if (graphic != null) graphic.Dispose();
            if (original_image != null) original_image.Dispose();
        }

    }




    public static System.Drawing.Imaging.ImageFormat GetImageFormat(string psFilePath)
    {
        switch (Path.GetExtension(psFilePath))
        {
            case ".gif":
                return System.Drawing.Imaging.ImageFormat.Gif;
            case ".jpg":
            case ".jpeg":
                return System.Drawing.Imaging.ImageFormat.Jpeg;
            default:
                return System.Drawing.Imaging.ImageFormat.Jpeg;
        }
    }

    public void DeleteImageFromFolder(string filename, string filepath)
    {
        //deleting thumb file physically

        if (File.Exists(filepath + filename))
        {
            File.Delete(filepath + filename);
        }
    }

    /// <summary>
    /// method for fetching the xml validation messages 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="id"></param>
    /// <param name="replacestring"></param>
    /// <returns></returns>
    public string Callvalidationmessage(string path, string id, string replacestring)
    {
        string strReturn = "";
        System.Data.DataSet dsMessages = new System.Data.DataSet();
        dsMessages.ReadXml(path);

        if (dsMessages != null)
        {
            if (dsMessages.Tables[0].Rows.Count > 0)
            {
                strReturn = dsMessages.Tables[0].Rows[0][id].ToString();
            }
        }

        switch (id)
        {
            case "Required":
                strReturn = strReturn.Replace("field", replacestring);
                break;
            case "NotInRecords":
                strReturn = strReturn.Replace("field", replacestring);
                break;
            case "ProcessError":
                break;
            default:
                strReturn = "";
                break;
        }

        return strReturn;
    }

    #endregion

    #region BindEnum
    public static Dictionary<int, string> GetEnumForBind(Type enumeration)
    {
        string[] names = Enum.GetNames(enumeration);
        Array values = Enum.GetValues(enumeration);
        Dictionary<int, string> ht = new Dictionary<int, string>();
        for (int i = 0; i < values.Length; i++)
        {
            ht.Add(Convert.ToInt32(values.GetValue(i)), names[i]);
        }

        return ht;
    }


    #endregion

    #region Bind Dropdown

    /// <summary>
    /// Bind dropdown and add onchange attribute
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ddl"></param>
    /// <param name="list"></param>
    /// <param name="DataTextField"></param>
    /// <param name="DataValueField"></param>
    /// <param name="FirstListItem"></param>
    public static void BindDDL<T>(DropDownList ddl, List<T> list, string DataTextField, string DataValueField,
        string FirstListItem) where T : class
    {
        ddl.Items.Clear();
        ddl.ClearSelection();
        if (list.Count > 0)
        {
            ddl.DataSource = list;
            ddl.DataTextField = DataTextField;
            ddl.DataValueField = DataValueField;
            ddl.DataBind();



            ddl.Attributes.Add("onchange", "pageLoad(this,value);");
        }

        if (!string.IsNullOrEmpty(FirstListItem))
        {
            ddl.Items.Insert(0, new ListItem(FirstListItem, "0"));
        }
    }

    public static void AddOnChangeAttribute(DropDownList ddl)
    {
        ddl.Attributes.Add("onchange", "pageLoad(this,value);");
    }



    /// <summary>
    /// To Insert Records in TodaysEmailDetails Table
    /// </summary>
    /// <param name="Messagebody">Message Contains or Body of Email</param>
    /// <param name="EmailSubject">Email Subject</param>
    /// <param name="SentFor">Mail type ie Support ticket or Order confirmations or Comm center etc . </param>
    /// <param name="UserInfoId">User Info id for which mail is send</param>
    /// <param name="CompanyId">and Company ID</param>

    public static void InsertIntoTodaysEmailDetails(String Messagebody, String EmailSubject, String SentFor, Int64 UserInfoId)
    {
        TodayEmailsRepository objRepo = new TodayEmailsRepository();
        TodaysEmailDetail objEmail = new TodaysEmailDetail();
        objEmail.FilePath = new Common().SaveFileAsHtml(Messagebody, EmailSubject);
        objEmail.EmailSubject = EmailSubject;
        objEmail.SentFor = SentFor;
        objEmail.UserInfoID = UserInfoId;
        objEmail.CreatedDate = DateTime.Now;
        objRepo.Insert(objEmail);
        objRepo.SubmitChanges();

    }


    public static void BindCountry(DropDownList ddl)
    {
        CountryRepository objRepo = new CountryRepository();
        List<INC_Country> objList = objRepo.GetAll();

        BindDDL(ddl, objList, "sCountryName", "iCountryID", "-select country-");

    }

    public static void BindState(DropDownList ddl, Int64 CountryId)
    {
        ddl.ClearSelection();
        ddl.Items.Clear();
        StateRepository objRepo = new StateRepository();
        List<INC_State> objList = objRepo.GetByCountryId(CountryId);

        Common.BindDDL(ddl, objList, "sStatename", "iStateID", "-select state-");
    }

    public static void BindCity(DropDownList ddl, Int64 StateId)
    {
        ddl.ClearSelection();
        ddl.Items.Clear();
        CityRepository objRepo = new CityRepository();
        List<INC_City> objList = objRepo.GetByStateId(StateId);

        Common.BindDDL(ddl, objList, "sCityName", "iCityID", "-select city-");
    }


    public static void BindActiveInactiveDDL(DropDownList ddl)
    {
        ddl.ClearSelection();
        ddl.Items.Clear();
        LookupRepository objRepo = new LookupRepository();
        List<INC_Lookup> objList = objRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Status);

        BindDDL(ddl, objList, "sLookupName", "iLookupID", null);
    }


    #endregion

    #region File

    public static void DeleteFile(string PathAndFileName)
    {
        if (File.Exists(PathAndFileName))
        {
            File.Delete(PathAndFileName);
        }
    }
    public static void DeleteDirectory(string PathAndDirectoryName)
    {
        if (Directory.Exists(PathAndDirectoryName))
        {
            Directory.Delete(PathAndDirectoryName,true);
        }
    }

    #endregion

    /// <summary>
    /// Get date value from textbox
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    public static DateTime? GetDate(TextBox txt)
    {
        if (string.IsNullOrEmpty(txt.Text))
        {
            return null;
        }

        return Convert.ToDateTime(txt.Text);
    }

    public static string GetDateString(DateTime? Date)
    {
        if (Date == null)
        {
            return "";
        }

        return Convert.ToDateTime(Date).ToString("MM/dd/yyyy");
    }

    /// <summary>
    /// return null if empty else decimalvalue
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static decimal? GetDecimal(string val)
    {
        if (string.IsNullOrEmpty(val))
        {
            return null;
        }

        return Convert.ToDecimal(val);

    }

    /// <summary>
    /// Return "0" if null
    /// </summary>
    /// <param name="Val"></param>
    /// <returns></returns>
    public static string GetIntString(int? Val)
    {
        if (Val == null)
        {
            Val = 0;
        }
        return Val.ToString();
    }

    /// <summary>
    /// Return "0" if null
    /// </summary>
    /// <returns></returns>
    public static string GetLongString(long? Val)
    {
        if (Val == null)
        {
            Val = 0;
        }
        return Val.ToString();

    }

    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        PropertyDescriptorCollection properties =
            TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        foreach (PropertyDescriptor prop in properties)
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        foreach (T item in list)
        {
            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);
        }
        return table;
    }

    /// <summary>
    /// return null value if Num is empty
    /// </summary>
    /// <param name="Num"></param>
    /// <returns></returns>
    public static long? GetLongNull(string Num, bool IsZeroNull)
    {
        long? NumVal = null;

        if (!string.IsNullOrEmpty(Num))
        {
            NumVal = Convert.ToInt64(Num);
        }

        if (IsZeroNull)
        {
            if (NumVal == 0)
            {
                NumVal = null;
            }
        }

        return NumVal;
    }

    ///// <summary>
    ///// Set session for porper menu
    ///// </summary>
    ///// <param name="Module"></param>
    //public static void SetModuleMenu(DAEnums.ManageID Module)
    //{
    //   HttpContext.Current.Session["ManageID"] = (int)Module;
    //}
    public static GeoDetailsByIP GetLatLong(string ipaddress)
    {
        GeoDetailsByIP objDetail = new GeoDetailsByIP();
        //double[] geo = new double[10];
        //http://api.ipinfodb.com/v3/ip-city/?key=f7f39d11a38b5e70ca84f0dd4730e313695e397916ad805172f585a9f3953111&ip=180.211.106.106&format=xml
        //http://api.ipinfodb.com/v3/ip-city/?key=f7f39d11a38b5e70ca84f0dd4730e313695e397916ad805172f585a9f3953111&ip=216.157.31.116&format=xml
        using (XmlReader reader = XmlReader.Create("http://api.ipinfodb.com/v3/ip-city/?key=f7f39d11a38b5e70ca84f0dd4730e313695e397916ad805172f585a9f3953111&ip=" + ipaddress + "&format=xml"))
        {
            reader.MoveToContent();
            // Parse the file and display each of the nodes.
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "statusCode")
                        {
                            XElement el = XElement.ReadFrom(reader) as XElement;
                            if (el != null)
                                objDetail.statusCode = el.Value;

                        }
                        if (reader.Name == "statusMessage")
                        {
                            XElement el = XElement.ReadFrom(reader) as XElement;
                            if (el != null)
                                objDetail.statusMessage = el.Value;

                        }
                        if (reader.Name == "countryCode")
                        {
                            XElement el = XElement.ReadFrom(reader) as XElement;
                            if (el != null)
                                objDetail.countryCode = el.Value;

                        }
                        if (reader.Name == "countryName")
                        {
                            XElement el = XElement.ReadFrom(reader) as XElement;
                            if (el != null)
                                objDetail.countryName = el.Value;

                        }
                        if (reader.Name == "regionName")
                        {
                            XElement el = XElement.ReadFrom(reader) as XElement;
                            if (el != null)
                                objDetail.regionName = el.Value;
                        }
                        if (reader.Name == "cityName")
                        {
                            XElement el = XElement.ReadFrom(reader) as XElement;
                            if (el != null)
                                objDetail.cityName = el.Value;

                        }
                        if (reader.Name == "zipCode")
                        {
                            XElement el = XElement.ReadFrom(reader) as XElement;
                            if (el != null)
                                objDetail.zipCode = el.Value;

                        }
                        if (reader.Name == "latitude")
                        {
                            XElement el = XElement.ReadFrom(reader) as XElement;
                            if (el != null)
                                objDetail.latitude = Double.Parse(el.Value);

                        }
                        else if (reader.Name == "longitude")
                        {
                            XElement el = XElement.ReadFrom(reader) as XElement;
                            if (el != null)
                                objDetail.longitude = Double.Parse(el.Value);
                        }
                        break;
                }
            }
        }
        return objDetail;
    }

    public static void ShowAlertMessage(String sErrorMessage, Page objPage)
    {
        if (objPage != null)
        {
            sErrorMessage = sErrorMessage.Replace("'", "\'");
            ScriptManager.RegisterStartupScript(objPage, objPage.GetType(), "err_msg", "alert('" + sErrorMessage + "');", true);
        }
    }

    #region For Property Comparer Class
    public class PropertyComparer<T> : IEqualityComparer<T>
    {
        private PropertyInfo _PropertyInfo;

        /// <summary>
        /// Creates a new instance of PropertyComparer.
        /// </summary>
        /// <param name="propertyName">The name of the property on type T 
        /// to perform the comparison on.</param>
        public PropertyComparer(string propertyName)
        {
            //store a reference to the property info object for use during the comparison
            _PropertyInfo = typeof(T).GetProperty(propertyName,
        BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            if (_PropertyInfo == null)
            {
                throw new ArgumentException(string.Format("{0} is not a property of type {1}.", propertyName, typeof(T)));
            }
        }

        #region IEqualityComparer<T> Members

        public bool Equals(T x, T y)
        {
            //get the current value of the comparison property of x and of y
            object xValue = _PropertyInfo.GetValue(x, null);
            object yValue = _PropertyInfo.GetValue(y, null);

            //if the xValue is null then we consider them equal if and only if yValue is null
            if (xValue == null)
                return yValue == null;

            //use the default comparer for whatever type the comparison property is.
            return xValue.Equals(yValue);
        }

        public int GetHashCode(T obj)
        {
            //get the value of the comparison property out of obj
            object propertyValue = _PropertyInfo.GetValue(obj, null);

            if (propertyValue == null)
                return 0;

            else
                return propertyValue.GetHashCode();
        }

        #endregion
    }
    #endregion

    #region Class Propeties of Mouse Flow API
    #region Properties use in mouse flow path

    /// <summary>
    /// This will set true to IsCartOrderComplete properity when Order  is completed in WL site.
    /// </summary>
    /// <param name="IsOrderComplete"></param>
    public void SetIsCartAbandonment(Boolean IsOrderComplete)
    {
        if (IsOrderComplete)
            IsCartOrderComplete = true;
        else
            IsCartOrderComplete = false;
    }
    /// <summary>
    /// This will true when user will not completed his cart payment
    /// </summary>
    public static Boolean IsCartOrderComplete
    {
        get
        {
            if ((HttpContext.Current.Session["IsCartOrderComplete"]) == null)
                return false;
            else
                return (Boolean)HttpContext.Current.Session["IsCartOrderComplete"];
        }
        set
        {
            HttpContext.Current.Session["IsCartOrderComplete"] = value;
        }

    }

    /// <summary>
    /// UserInfo ID
    /// </summary>
    public static Int64 UserID
    {
        get
        {
            if (IncentexGlobal.CurrentMember != null && IncentexGlobal.CurrentMember.UserInfoID != null && IncentexGlobal.CurrentMember.UserInfoID != 0)
                return IncentexGlobal.CurrentMember.UserInfoID;
            else
                return 0;
        }
    }    

    /// <summary>
    /// Company ID
    /// </summary>
    public static Int64? CompID
    {
        get
        {
            if (IncentexGlobal.CurrentMember != null && IncentexGlobal.CurrentMember.CompanyId != null && IncentexGlobal.CurrentMember.CompanyId != 0)
                return IncentexGlobal.CurrentMember.CompanyId;
            else
                return 0;
        }
    }
    #endregion

    public class WebsiteList
    {
        public string WsList { get; set; }
        public string Website { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public Int32 Recordings { get; set; }
        public string RecordingStatus { get; set; }
        public string Domains { get; set; }
        public string Alignment { get; set; }
        public string Width { get; set; }
        public string PageIdentifiers { get; set; }
        public string ExcludedIpAddresses { get; set; }
        public APIException Error { get; set; }
    }

    public class RecordingList
    {
        public string RcrdList { get; set; }
        public string Recording { get; set; }
        public string Id { get; set; }
        public string LastUpdated { get; set; }
        public string IP { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string RLLanguage { get; set; }
        public string EntryUrl { get; set; }
        public string PgCount { get; set; }
        public TimeSpan VisitLength { get; set; }
        public string Browser { get; set; }
        public string UserAgent { get; set; }
        public string OS { get; set; }
        public string Referrer { get; set; }

        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public String ImageSrc { get; set; }
        public String PopUrl { get; set; }
        public string URL { get; set; }
        public Int32 MaxScrolledPercentage { get; set; }
        public APIException Error { get; set; }
    }

    public class Pageviews
    {
        public string PageViewList { get; set; }
        public string PageView { get; set; }
        public string Id { get; set; }
        public string Session { get; set; }
        public string PageId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string PageTitle { get; set; }
        public string URL { get; set; }
        public string QueryString { get; set; }
        public string FormInteraction { get; set; }
        public Int32 MaxScrolledPercentage { get; set; }
        public string IP { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Gllanguage { get; set; }
        public string CntryCode { get; set; }
        public string VisitLength { get; set; }
        public string Browser { get; set; }
        public string UserAgent { get; set; }
        public string OS { get; set; }
        public string Referrer { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }

        public Boolean IsSetUserName { get; set; }
        public Boolean IsSetCompanyName { get; set; }
        public APIException Error { get; set; }
    }

    public class Pagelist
    {
        public string PgList { get; set; }
        public string Page { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Views30Days { get; set; }
        public string AvgVisitLengthSec { get; set; }
        public string AvgInteractionTimeSec { get; set; }
        public string BounceRate { get; set; }
        public string ClicksPerPageView { get; set; }
        public string LoadTimeMS { get; set; }
        public string GrabTimeMS { get; set; }
        public string AvgScrollPercentage { get; set; }
        public string PageHeight { get; set; }
        public string HtmlSizeKB { get; set; }

        public APIException Error { get; set; }
    }
    public class APIException
    {
        public int Code { get; set; }
        public string ErrorMessage { get; set; }
    }
    #endregion

    /// <summary>
    /// added by Prashant 10th January 2013
    /// To Bind the Drop Down for Period in Reports Module (Admin and My Account Section)
    /// </summary>
    /// <returns></returns>
    public static List<GetDatePeriodValueForSearchResult> BindPeriodDropDownItems()
    {
        List<GetDatePeriodValueForSearchResult> objItemList = new List<GetDatePeriodValueForSearchResult>();
        //objItemList.Add(new ListItem("Today", "0"));
        //objItemList.Add(new ListItem("Yesterday", "1"));
        //objItemList.Add(new ListItem("Last 3 Days", "3"));
        //objItemList.Add(new ListItem("Last 7 Days", "7"));
        //objItemList.Add(new ListItem("Last 30 Days", "30"));
        //objItemList.Add(new ListItem("Date Range", ""));
        //for (int i = 2012; i < DateTime.Now.Year; i++)
        //{
        //    objItemList.Add(new ListItem(i.ToString(), i.ToString()));
        //}
        ReportRepository objReportRepository = new ReportRepository();
        objItemList = objReportRepository.GetDatePeriodValues();
        return objItemList;
    }

    public static void DownloadFile(string filepath, string displayFileName)
    {
        System.IO.Stream iStream = null;
        string type = "";
        string ext = Path.GetExtension(filepath);
        if (displayFileName.IndexOf('.') > 0)
            displayFileName = Convert.ToString(displayFileName.Split('.')[0]);

        displayFileName = displayFileName.Replace(" ", "_");
        // Buffer to read 10K bytes in chunk:
        byte[] buffer = new Byte[10000];


        // Total bytes to read:
        long dataToRead;

        // Identify the file name.
        string filename = System.IO.Path.GetFileName(filepath);

        try
        {
            // Open the file.
            iStream = new System.IO.FileStream(filepath, System.IO.FileMode.Open,
            System.IO.FileAccess.Read, System.IO.FileShare.Read);


            // Total bytes to read:
            dataToRead = iStream.Length;
            if (ext != null)
            {
                switch (ext.ToLower())
                {
                    case ".htm":
                    case ".html":
                        type = "text/HTML";
                        break;
                    case ".txt":
                        type = "text/plain";
                        break;
                    case ".pdf":
                        type = "Application/pdf";
                        break;
                    case ".mp3":
                        type = "audio/mpeg";
                        break;
                    case ".wmi":
                        type = "audio/basic";
                        break;
                    case ".dat":
                    case ".mpeg":
                    case "mpg":
                        type = "video/mpeg";
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".gif":
                    case ".tiff":
                        type = "image/gif";
                        break;
                    case ".png":
                        type = "image/png";
                        break;
                    case ".doc":
                    case ".rtf":
                        type = "Application/msword";
                        break;
                    case ".xls":
                    case ".xlsx":
                        type = "application/x-excel";
                        break;
                    case ".wav":
                        type = "audio/x-wav";
                        break;
                    case "bmp":
                        type = "image/bmp";
                        break;
                    case "flv":
                        type = "video/x-flv";
                        break;
                    case "docx":
                        type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case "ppt":
                        type = "application/vnd.ms-powerpoint";
                        break;
                    case "pptx":
                        type = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        break;
                    case "avi":
                        type = "video/x-msvideo";
                        break;
                    case "wmv":
                        type = "video/x-ms-wmv";
                        break;
                    case "wma":
                        type = "/audio/x-ms-wma";
                        break;



                    //left:--rm
                }
            }


            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + displayFileName + ext + "\"");
            HttpContext.Current.Response.ContentType = type;
            HttpContext.Current.Response.WriteFile(filepath);
            HttpContext.Current.Response.End();


        }
        catch (Exception ex)
        {
            // Trap the error, if any.
            HttpContext.Current.Response.Write("Error : " + ex.Message);
        }
        finally
        {
            if (iStream != null)
            {
                //Close the file.
                iStream.Close();
            }
        }
    }

    public static void SaveXMLFromObject(Object o, String FilePath)
    {
        try
        {
            XmlSerializer XmlS = new XmlSerializer(o.GetType());
            StringWriter sw = new StringWriter();
            TextWriter textWriter = new StreamWriter(FilePath);
            XmlS.Serialize(textWriter, o);
            textWriter.Close();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #region To validate the Shipping Zipcode for different Expression based on the Selected Country

    public static Boolean IsValidZipCode(String Country, String ZipCode)
    {
        if (Country == "United States")
        {
            Regex USZIPCodeRegex = new Regex(@"^\d{5}([-\s]{0,3}\d{4})?$");
            return USZIPCodeRegex.IsMatch(ZipCode);
        }
        else if (Country == "Canada")
        {
            Regex CanadaPostalCodeRegex = new Regex(@"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])[-\s]{0,3}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$", RegexOptions.IgnoreCase);
            return CanadaPostalCodeRegex.IsMatch(ZipCode);
        }
        else
            return true;
    }

    #endregion

    #region MOAS Common function to approve,update and Send Notification to Appropriate Users

    //    /// <summary>
    //    /// Sends the verification email supplier.
    //    /// </summary>
    //    /// <param name="OrderID">The order id.</param>
    //    /// <param name="ShoppingCartID">The shopping cart ID.</param>
    //    /// <param name="supplierId">The supplier id.</param>
    //    /// <param name="fullName">The full name.</param>
    //    //public static EmailNotificationDetails GetEmailBodyForSupplier(Int64 OrderID, Boolean IsTestOrder)
    //    //{
    //    //    #region DataMembers
    //    //    UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
    //    //    OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
    //    //    LookupRepository objLookupRepository = new LookupRepository();
    //    //    CompanyRepository objCmpRepo = new CompanyRepository();
    //    //    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    //    //    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    //    //    CompanyStoreRepository objCompanyStoreRepository = new CompanyStoreRepository();
    //    //    CompanyEmployeeContactInfo objShippingInfo;
    //    //    CompanyEmployeeContactInfo objBillingInfo;
    //    //    CityRepository objCity = new CityRepository();
    //    //    StateRepository objState = new StateRepository();
    //    //    CountryRepository objCountry = new CountryRepository();
    //    //    MyIssuanceCartRepository objMyIssCartRepo = new MyIssuanceCartRepository();
    //    //    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    //    //    List<SelectMyShoppingCartProductResult> objSelectMyShoppingCartProductResultList;
    //    //    #endregion

    //    //    try
    //    //    {
    //    //        //Get supplierinfo by id
    //    //        UserInformation objUserInfo = objUsrInfoRepo.GetById(new SupplierRepository().GetById(supplierId).UserInfoID);

    //    //        if (objUserInfo != null)
    //    //        {

    //    //            EmailTemplateBE objEmailBE = new EmailTemplateBE();
    //    //            EmailTemplateDA objEmailDA = new EmailTemplateDA();
    //    //            DataSet dsEmailTemplate;
    //    //            String sFrmadd = String.Empty, sFrmname = String.Empty, sSubject = String.Empty;
    //    //            //Get Email Content
    //    //            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
    //    //            objEmailBE.STemplateName = "Order Placed Supplier";
    //    //            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
    //    //            if (dsEmailTemplate != null)
    //    //            {
    //    //                //Find UserName who had order purchased
    //    //                Order objOrder = objRepos.GetByOrderID(OrderID);
    //    //                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);

    //    //                sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
    //    //                sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
    //    //                sSubject = objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber;

    //    //                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
    //    //                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());
    //    //                messagebody.Replace("{firstnote}", "Please review the following order below if you have any questions please post them to the notes section of this order located within the order in the system.");
    //    //                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
    //    //                messagebody.Replace("{referencename}", objOrder.ReferenceName + "<br/>Workgroup : " + objLookupRepository.GetById((Int64)(objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID).WorkgroupID)).sLookupName); // change by mayur on 26-March-2012
    //    //                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
    //    //                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());

    //    //                String PaymentMethod = String.Empty;
    //    //                if (objOrder.PaymentOption != null)
    //    //                    PaymentMethod = objLookupRepository.GetById((Int64)objOrder.PaymentOption).sLookupName;

    //    //                //Added on 20 Sep 2011
    //    //                //Check here for CreditUsed
    //    //                String cr;
    //    //                Boolean creditused = true;
    //    //                if (objOrder.CreditUsed != "0")
    //    //                {
    //    //                    if (String.IsNullOrEmpty(objOrder.CreditUsed))
    //    //                    {
    //    //                        creditused = false;
    //    //                    }
    //    //                    else
    //    //                    {
    //    //                        creditused = true;
    //    //                    }
    //    //                }
    //    //                else
    //    //                {
    //    //                    creditused = false;
    //    //                }

    //    //                //Both Option have used
    //    //                if (objOrder.PaymentOption != null && creditused)
    //    //                {

    //    //                    if (objOrder.CreditUsed == "Previous")
    //    //                    {
    //    //                        cr = "Starting " + "Credits Paid by Corporate ";
    //    //                    }
    //    //                    else
    //    //                    {
    //    //                        cr = "Anniversary " + "Credits Paid by Corporate ";
    //    //                    }
    //    //                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + PaymentMethod);
    //    //                }
    //    //                //Only Starting or Anniversary
    //    //                else if (objOrder.PaymentOption == null && creditused)
    //    //                {
    //    //                    if (objOrder.CreditUsed == "Previous")
    //    //                    {
    //    //                        cr = "Starting " + "Credits - Paid by Corporate ";
    //    //                    }
    //    //                    else
    //    //                    {
    //    //                        cr = "Anniversary " + "Credits - Paid by Corporate ";
    //    //                    }
    //    //                    messagebody.Replace("{PaymentType}", cr);
    //    //                }
    //    //                //Only Payment Option
    //    //                else if (objOrder.PaymentOption != null && !creditused)
    //    //                {
    //    //                    messagebody.Replace("{PaymentType}", PaymentMethod);
    //    //                }
    //    //                else
    //    //                {
    //    //                    messagebody.Replace("{PaymentType}", "Paid By Corporate");
    //    //                }
    //    //                messagebody.Replace("{Payment Method :}", "Payment Method :");

    //    //                //For displaying payment option code
    //    //                messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
    //    //                messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);

    //    //                //End
    //    //                //messagebody.Replace("{CreditType}", "---");

    //    //                #region shipping address
    //    //                messagebody.Replace("{Shipping Address}", "Ship To:");
    //    //                messagebody.Replace("{AirportView1}", "Airport :");
    //    //                messagebody.Replace("{DepartmentView1}", "Department :");
    //    //                messagebody.Replace("{NameView1}", "Name :");
    //    //                messagebody.Replace("{CompanyNameView1}", "Company Name :");
    //    //                messagebody.Replace("{AddressView1}", "Address :");
    //    //                // messagebody.Replace("{Address2}", lblBadr.Text);
    //    //                messagebody.Replace("{CityView1}", "City :");
    //    //                messagebody.Replace("{CountyView1}", "County :");
    //    //                messagebody.Replace("{StateView1}", "State :");
    //    //                messagebody.Replace("{ZipView1}", "Zip :");
    //    //                messagebody.Replace("{CountryView1}", "Country :");
    //    //                messagebody.Replace("{PhoneView1}", "Phone :");
    //    //                messagebody.Replace("{EmailView1}", "Email :");

    //    //                messagebody.Replace("{Airport1}", objShippingInfo.Airport);
    //    //                if (objShippingInfo.DepartmentID != null)
    //    //                    messagebody.Replace("{Department1}", new LookupRepository().GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
    //    //                else
    //    //                    messagebody.Replace("{Department1}", "");
    //    //                messagebody.Replace("{Name1}", objShippingInfo.Name + " " + objShippingInfo.Fax);
    //    //                messagebody.Replace("{CompanyName1}", objShippingInfo.CompanyName);
    //    //                messagebody.Replace("{Address11}", objShippingInfo.Address + "<br/>" + objShippingInfo.Address2 + "<br/>" + objShippingInfo.Street);
    //    //                //messagebody.Replace("{Address22}", lbl.Text);
    //    //                messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
    //    //                messagebody.Replace("{County1}", objShippingInfo.county);
    //    //                messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
    //    //                messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
    //    //                messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
    //    //                messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
    //    //                messagebody.Replace("{Email1}", objShippingInfo.Email);
    //    //                #endregion

    //    //                String innermessageSupplier = "";
    //    //                Boolean GLCodeExists = false;

    //    //                #region Supplier

    //    //                List<SelectOrderDetailsForSupplierResult> lstSupplierItemsFromOrder = objRepos.GetOrderItemsForSupplier(supplierId, objOrder.OrderID);

    //    //                if (lstSupplierItemsFromOrder.Count > 0)
    //    //                {
    //    //                    GLCodeExists = lstSupplierItemsFromOrder.FirstOrDefault(le => le.GLCode != null && le.GLCode != "") != null;
    //    //                    Int16 ColSpan = 9;

    //    //                    if (GLCodeExists)
    //    //                        ColSpan = 10;

    //    //                    innermessageSupplier = innermessageSupplier + "<tr>";
    //    //                    innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
    //    //                    innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
    //    //                    innermessageSupplier = innermessageSupplier + "<tr>";
    //    //                    innermessageSupplier = innermessageSupplier + "<td align='center' colspan='2'  style='background-color:Gray;font-weight:bold;color:Black;'>";
    //    //                    innermessageSupplier = innermessageSupplier + SupplierCompanyName;
    //    //                    innermessageSupplier = innermessageSupplier + "</td>";
    //    //                    innermessageSupplier = innermessageSupplier + "</tr>";
    //    //                    innermessageSupplier = innermessageSupplier + "</table>";
    //    //                    innermessageSupplier = innermessageSupplier + "</td>";
    //    //                    innermessageSupplier = innermessageSupplier + "</tr>";
    //    //                    innermessageSupplier = innermessageSupplier + "<tr>";
    //    //                    innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
    //    //                    innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
    //    //                    innermessageSupplier = innermessageSupplier + "<tr>";
    //    //                    innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: left;'>";
    //    //                    innermessageSupplier = innermessageSupplier + "Ordered";
    //    //                    innermessageSupplier = innermessageSupplier + "</td>";
    //    //                    innermessageSupplier = innermessageSupplier + "<td width='15%' style='font-weight: bold; text-align: center;'>";
    //    //                    innermessageSupplier = innermessageSupplier + "Item#";
    //    //                    innermessageSupplier = innermessageSupplier + "</td>";
    //    //                    innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
    //    //                    innermessageSupplier = innermessageSupplier + "Size";
    //    //                    innermessageSupplier = innermessageSupplier + "</td>";
    //    //                    innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
    //    //                    innermessageSupplier = innermessageSupplier + "Color";
    //    //                    innermessageSupplier = innermessageSupplier + "</td>";

    //    //                    if (!GLCodeExists)
    //    //                    {
    //    //                        innermessageSupplier = innermessageSupplier + "<td width='35%' style='font-weight: bold; text-align: center;'>";
    //    //                        innermessageSupplier = innermessageSupplier + "Description";
    //    //                        innermessageSupplier = innermessageSupplier + "</td>";
    //    //                    }
    //    //                    else
    //    //                    {
    //    //                        innermessageSupplier = innermessageSupplier + "<td width='25%' style='font-weight: bold; text-align: center;'>";
    //    //                        innermessageSupplier = innermessageSupplier + "Description";
    //    //                        innermessageSupplier = innermessageSupplier + "</td>";
    //    //                        innermessageSupplier = innermessageSupplier + "<td width='15%' style='font-weight: bold; text-align: center;'>";
    //    //                        innermessageSupplier = innermessageSupplier + "GL Code";
    //    //                        innermessageSupplier = innermessageSupplier + "</td>";
    //    //                    }

    //    //                    //Add Nagmani 18-Jan-2012
    //    //                    innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
    //    //                    innermessageSupplier = innermessageSupplier + "Unit Price";
    //    //                    innermessageSupplier = innermessageSupplier + "</td>";
    //    //                    innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
    //    //                    innermessageSupplier = innermessageSupplier + "Extended Price";
    //    //                    innermessageSupplier = innermessageSupplier + "</td>";
    //    //                    //End

    //    //                    innermessageSupplier = innermessageSupplier + "</table>";
    //    //                    innermessageSupplier = innermessageSupplier + "</td>";
    //    //                    innermessageSupplier = innermessageSupplier + "</tr>";
    //    //                    innermessageSupplier = innermessageSupplier + "<tr>";
    //    //                    innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
    //    //                    innermessageSupplier = innermessageSupplier + "<hr />";
    //    //                    innermessageSupplier = innermessageSupplier + "</td>";
    //    //                    innermessageSupplier = innermessageSupplier + "</tr>";

    //    //                    foreach (SelectOrderDetailsForSupplierResult item in lstSupplierItemsFromOrder)
    //    //                    {
    //    //                        innermessageSupplier = innermessageSupplier + "<tr>";
    //    //                        innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
    //    //                        innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
    //    //                        innermessageSupplier = innermessageSupplier + "<tr>";
    //    //                        innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: left;'>";
    //    //                        innermessageSupplier = innermessageSupplier + item.Quantity;
    //    //                        innermessageSupplier = innermessageSupplier + "</td>";
    //    //                        innermessageSupplier = innermessageSupplier + "<td width='15%' height='50' style='font-weight: bold; text-align: center;'>";
    //    //                        innermessageSupplier = innermessageSupplier + item.ItemNumber;
    //    //                        innermessageSupplier = innermessageSupplier + "</td>";
    //    //                        innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
    //    //                        innermessageSupplier = innermessageSupplier + item.Size;
    //    //                        innermessageSupplier = innermessageSupplier + "</td>";
    //    //                        innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
    //    //                        innermessageSupplier = innermessageSupplier + item.Color;
    //    //                        innermessageSupplier = innermessageSupplier + "</td>";

    //    //                        if (!GLCodeExists)
    //    //                        {
    //    //                            innermessageSupplier = innermessageSupplier + "<td width='35%' height='50' style='font-weight: bold; text-align: center;'>";
    //    //                            innermessageSupplier = innermessageSupplier + item.ProductDescrption;
    //    //                            innermessageSupplier = innermessageSupplier + "</td>";
    //    //                        }
    //    //                        else
    //    //                        {
    //    //                            innermessageSupplier = innermessageSupplier + "<td width='25%' height='50' style='font-weight: bold; text-align: center;'>";
    //    //                            innermessageSupplier = innermessageSupplier + item.ProductDescrption;
    //    //                            innermessageSupplier = innermessageSupplier + "</td>";

    //    //                            innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
    //    //                            innermessageSupplier = innermessageSupplier + item.GLCode;
    //    //                            innermessageSupplier = innermessageSupplier + "</td>";
    //    //                        }

    //    //                        innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold;text-align: center;'>";
    //    //                        innermessageSupplier = innermessageSupplier + "$" + (item.MOASPrice != null ? Convert.ToDecimal(item.MOASPrice) : Convert.ToDecimal(item.Price)).ToString();
    //    //                        innermessageSupplier = innermessageSupplier + "</td>";
    //    //                        innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold;text-align: center;'>";
    //    //                        innermessageSupplier = innermessageSupplier + "$" + (item.MOASPrice != null ? Convert.ToDecimal(item.MOASPrice) : Convert.ToDecimal(item.Price)) * Convert.ToDecimal(item.Quantity);
    //    //                        innermessageSupplier = innermessageSupplier + "</td>";

    //    //                        innermessageSupplier = innermessageSupplier + "</tr>";
    //    //                        innermessageSupplier = innermessageSupplier + "</table>";
    //    //                        innermessageSupplier = innermessageSupplier + "</td>";
    //    //                        innermessageSupplier = innermessageSupplier + "</tr>";
    //    //                    }

    //    //                    innermessageSupplier = innermessageSupplier + "<tr>";
    //    //                    innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
    //    //                    innermessageSupplier = innermessageSupplier + "<hr />";
    //    //                    innermessageSupplier = innermessageSupplier + "</td>";
    //    //                    innermessageSupplier = innermessageSupplier + "</tr>";
    //    //                }

    //    //                messagebody.Replace("{innermesaageforsupplier}", innermessageSupplier);

    //    //                //Update Nagmani 18-Jan-2012
    //    //                messagebody.Replace("{ShippingCost}", "");
    //    //                messagebody.Replace("{Saletax}", "");
    //    //                // messagebody.Replace("{OrderTotal}", "");

    //    //                //messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());
    //    //                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.SalesTax + (objOrder.MOASOrderAmount != null ? objOrder.MOASOrderAmount : objOrder.OrderAmount)).ToString());

    //    //                //End Nagmani
    //    //                messagebody.Replace(" {Order Notes:}", "Order Notes :");
    //    //                messagebody.Replace("{ShippingCostView}", "");
    //    //                messagebody.Replace("{SalesTaxView}", "");
    //    //                //Update Nagmani 18-Jan-2012
    //    //                //  messagebody.Replace("{OrderTotalView}", "");
    //    //                messagebody.Replace("{OrderTotalView}", "Order Total :");
    //    //                //End Nagmani 

    //    //                //End
    //    //                #endregion

    //    //                String c = NameBars(objOrder.OrderID);
    //    //                if (c != null)
    //    //                {
    //    //                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + c);
    //    //                }
    //    //                else
    //    //                {
    //    //                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
    //    //                }

    //    //                //  messagebody.Replace("{PurchasesNotEligableforCreditUse}", lblNotEligibleforAnniversarycredit.Text);


    //    //                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);


    //    //                EmailNotificationDetails objEmailNotificationDetails = new EmailNotificationDetails();
    //    //                objEmailNotificationDetails.MessageBody = messagebody.ToString();
    //    //                objEmailNotificationDetails.sFromAddress = sFrmadd;
    //    //                objEmailNotificationDetails.sFromName = sFrmname;
    //    //                objEmailNotificationDetails.sSubject = sSubject;

    //    //                return objEmailNotificationDetails;
    //    //                //new CommonMails().SendMail(objUserInfo.UserInfoID, "Order Confirmation", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
    //    //            }

    //    //        }
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        ErrHandler.WriteError(ex);
    //    //        return null;
    //    //    }
    //    //}

    //    /// <summary>
    //    /// Function to generate the Email Body to send Order Notification to the IEs
    //    /// </summary>
    //    /// <param name="strStatus"></param>
    //    /// <param name="OrderID"></param>
    //    /// <param name="IsTestOrder"></param>
    //    /// <returns></returns>
    //    public static EmailNotificationDetails GetIEMailBody(Int64 OrderID, Boolean IsTestOrder)
    //    {
    //        #region DataMembers
    //        UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
    //        OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
    //        LookupRepository objLookupRepository = new LookupRepository();
    //        CompanyRepository objCmpRepo = new CompanyRepository();
    //        CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    //        CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    //        CompanyStoreRepository objCompanyStoreRepository = new CompanyStoreRepository();
    //        CompanyEmployeeContactInfo objShippingInfo;
    //        CompanyEmployeeContactInfo objBillingInfo;
    //        CityRepository objCity = new CityRepository();
    //        StateRepository objState = new StateRepository();
    //        CountryRepository objCountry = new CountryRepository();
    //        MyIssuanceCartRepository objMyIssCartRepo = new MyIssuanceCartRepository();
    //        MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    //        List<SelectMyShoppingCartProductResult> objSelectMyShoppingCartProductResultList;
    //        #endregion

    //        try
    //        {
    //            EmailTemplateBE objEmailBE = new EmailTemplateBE();
    //            EmailTemplateDA objEmailDA = new EmailTemplateDA();
    //            DataSet dsEmailTemplate;

    //            string sFrmadd = string.Empty, sFrmname = string.Empty, sSubject = string.Empty;

    //            //Get Email Content
    //            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
    //            objEmailBE.STemplateName = "Order Placed";
    //            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
    //            if (dsEmailTemplate != null)
    //            {
    //                Order objOrder = objRepos.GetByOrderID(OrderID);
    //                Boolean IsMOASWithCostCenterCode = objCompanyEmployeeRepository.FUN_IsMOASWithCostCenterCode(Convert.ToInt64(objOrder.UserId));

    //                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
    //                CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID);

    //                sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
    //                sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
    //                sSubject = objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber.ToString();

    //                //sToadd = "mayur.rathod@indianic.com";

    //                String PaymentMethod = String.Empty;
    //                if (objOrder.PaymentOption != null)
    //                    PaymentMethod = objLookupRepository.GetById((Int64)objOrder.PaymentOption).sLookupName;

    //                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
    //                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());

    //                messagebody.Replace("{firstnote}", "Thank you for your business you can check the status of order(s) any time by logging into your store <a href='http://www.world-link.us.com' title='Incentex'>here.</a>");
    //                //messagebody.Replace("{FullName}", FullName);
    //                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
    //                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
    //                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
    //                messagebody.Replace("{referencename}", objOrder.ReferenceName);
    //                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
    //                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());
    //                //Check here for CreditUsed
    //                String cr;
    //                Boolean creditused = true;
    //                if (objOrder.CreditUsed != "0")
    //                {
    //                    if (String.IsNullOrEmpty(objOrder.CreditUsed))
    //                    {
    //                        creditused = false;
    //                    }
    //                    else
    //                    {
    //                        creditused = true;
    //                    }
    //                }
    //                else
    //                {
    //                    creditused = false;
    //                }

    //                //Both Option have used
    //                if (objOrder.PaymentOption != null && creditused)
    //                {

    //                    if (objOrder.CreditUsed == "Previous")
    //                    {
    //                        cr = "Starting " + "Credits Paid by Corporate ";
    //                        /*Strating Credit Row*/
    //                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits :");
    //                        messagebody.Replace("{lblStCrAmntApplied}", objOrder.CreditAmt.ToString());
    //                        /*End*/
    //                    }
    //                    else
    //                    {
    //                        cr = "Anniversary " + "Credits Paid by Corporate ";
    //                        /*Anniversary Credit Row*/
    //                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits :");
    //                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
    //                        /*End*/

    //                    }
    //                    /*Payment Option Row*/
    //                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + PaymentMethod + " : ");
    //                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
    //                    /*End*/
    //                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + PaymentMethod);
    //                }
    //                //Only Starting or Anniversary
    //                else if (objOrder.PaymentOption == null && creditused)
    //                {
    //                    if (objOrder.CreditUsed == "Previous")
    //                    {
    //                        cr = "Starting " + "Credits - Paid by Corporate ";
    //                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits:");
    //                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
    //                    }
    //                    else
    //                    {
    //                        cr = "Anniversary " + "Credits - Paid by Corporate ";
    //                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits:");
    //                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
    //                    }
    //                    /*Payment Option Row*/
    //                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
    //                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
    //                    /*End*/
    //                    messagebody.Replace("{PaymentType}", cr);
    //                }
    //                //Only Payment Option
    //                else if (objOrder.PaymentOption != null && !creditused)
    //                {
    //                    messagebody.Replace("{PaymentType}", PaymentMethod);
    //                    /*Anniversary Credit Row*/
    //                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
    //                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
    //                    /*End*/
    //                    /*Payment Option Row*/
    //                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + PaymentMethod + " : ");
    //                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
    //                    /*End*/
    //                }
    //                else
    //                {
    //                    messagebody.Replace("{PaymentType}", "Paid By Corporate");
    //                    /*Anniversary Credit Row*/
    //                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
    //                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
    //                    /*End*/

    //                    /*Payment Option Row*/
    //                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
    //                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
    //                    /*End*/
    //                }
    //                messagebody.Replace("{Payment Method :}", "Payment Method :");

    //                #region PaymentOptionCode
    //                //For displaying payment option code
    //                if (IsMOASWithCostCenterCode)
    //                {
    //                    messagebody.Replace("{PaymentOptionCodeLabel}", "Cost Center Code :");
    //                    messagebody.Replace("{PaymentOptionCodeValue}", objOrder.PaymentOptionCode);
    //                }
    //                else
    //                {
    //                    messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
    //                    messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);
    //                }
    //                #endregion

    //                #region Billing Address
    //                if (objOrder.OrderFor == "ShoppingCart") // Company Pays
    //                {
    //                    objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
    //                }
    //                else
    //                {
    //                    objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
    //                }

    //                messagebody.Replace("{Billing Address}", "Bill To:");
    //                messagebody.Replace("{NameView}", "Name :");
    //                messagebody.Replace("{CompanyNameView}", "Company Name :");
    //                messagebody.Replace("{AddressView}", "Address :");
    //                messagebody.Replace("{CityView}", "City :");
    //                messagebody.Replace("{StateView}", "State :");
    //                messagebody.Replace("{ZipView}", "Zip :");
    //                messagebody.Replace("{CountryView}", "Country :");
    //                messagebody.Replace("{PhoneView}", "Phone :");
    //                messagebody.Replace("{EmailView}", "Email :");


    //                messagebody.Replace("{Name}", objBillingInfo.BillingCO + " " + objBillingInfo.Manager);
    //                messagebody.Replace("{CompanyName}", objBillingInfo.CompanyName);
    //                messagebody.Replace("{Address1}", objBillingInfo.Address);
    //                // messagebody.Replace("{Address2}", lblBadr.Text);
    //                messagebody.Replace("{City}", objCity.GetById((Int64)objBillingInfo.CityID).sCityName);
    //                messagebody.Replace("{State}", objState.GetById((Int64)objBillingInfo.StateID).sStatename);
    //                messagebody.Replace("{Zip}", objBillingInfo.ZipCode);
    //                messagebody.Replace("{Country}", objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName);
    //                messagebody.Replace("{Phone}", objBillingInfo.Telephone);
    //                messagebody.Replace("{Email}", objBillingInfo.Email);
    //                #endregion

    //                #region shipping address
    //                if (objOrder.OrderFor == "ShoppingCart")
    //                {
    //                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
    //                }
    //                else
    //                {
    //                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
    //                }
    //                messagebody.Replace("{Shipping Address}", "Ship To:");
    //                messagebody.Replace("{AirportView1}", "Airport :");
    //                messagebody.Replace("{DepartmentView1}", "Department :");
    //                messagebody.Replace("{NameView1}", "Name :");
    //                messagebody.Replace("{CompanyNameView1}", "Company Name :");
    //                messagebody.Replace("{AddressView1}", "Address :");
    //                messagebody.Replace("{CityView1}", "City :");
    //                messagebody.Replace("{CountyView1}", "County :");
    //                messagebody.Replace("{StateView1}", "State :");
    //                messagebody.Replace("{ZipView1}", "Zip :");
    //                messagebody.Replace("{CountryView1}", "Country :");
    //                messagebody.Replace("{PhoneView1}", "Phone :");
    //                messagebody.Replace("{EmailView1}", "Email :");


    //                messagebody.Replace("{Airport1}", objShippingInfo.Airport);
    //                if (objShippingInfo.DepartmentID != null)
    //                    messagebody.Replace("{Department1}", new LookupRepository().GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
    //                else
    //                    messagebody.Replace("{Department1}", "");
    //                messagebody.Replace("{Name1}", objShippingInfo.Name + " " + objShippingInfo.Fax);
    //                messagebody.Replace("{CompanyName1}", objShippingInfo.CompanyName);
    //                messagebody.Replace("{Address11}", objShippingInfo.Address + "<br/>" + objShippingInfo.Address2 + "<br/>" + objShippingInfo.Street);
    //                messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
    //                messagebody.Replace("{County1}", objShippingInfo.county);
    //                messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
    //                messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
    //                messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
    //                messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
    //                messagebody.Replace("{Email1}", objShippingInfo.Email);
    //                #endregion

    //                #region start
    //                String innermessage = "";
    //                Boolean GLCodeExists = false;

    //                if (objOrder.OrderFor == "ShoppingCart")
    //                {
    //                    objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.MyShoppingCartID);

    //                    GLCodeExists = objSelectMyShoppingCartProductResultList.FirstOrDefault(le => le.GLCode != null) != null;

    //                    for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
    //                    {
    //                        String[] p = new String[1];
    //                        if (!String.IsNullOrEmpty(objSelectMyShoppingCartProductResultList[i].NameToBeEngraved))
    //                        {
    //                            p = objSelectMyShoppingCartProductResultList[i].NameToBeEngraved.Split(',');
    //                        }

    //                        innermessage = innermessage + "<tr>";
    //                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
    //                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Quantity;
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
    //                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].item;
    //                        innermessage = innermessage + "</td>";

    //                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
    //                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Size;
    //                        innermessage = innermessage + "</td>";

    //                        if (!GLCodeExists)
    //                        {
    //                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
    //                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
    //                            innermessage = innermessage + "</td>";
    //                        }
    //                        else
    //                        {
    //                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
    //                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
    //                            innermessage = innermessage + "</td>";
    //                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
    //                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].GLCode;
    //                            innermessage = innermessage + "</td>";
    //                        }

    //                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
    //                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)).ToString();
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
    //                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)) * Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].Quantity);
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "</tr>";
    //                        innermessage = innermessage + "<tr>";

    //                        if (GLCodeExists)
    //                            innermessage = innermessage + "<td colspan='8'>";
    //                        else
    //                            innermessage = innermessage + "<td colspan='7'>";

    //                        innermessage = innermessage + "<hr />";
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "</tr>";

    //                    }
    //                    messagebody.Replace("{innermessage}", innermessage);
    //                }
    //                else
    //                {
    //                    Int64? storeid;
    //                    List<MyIssuanceCart> objMyIssCart = new List<MyIssuanceCart>();
    //                    storeid = objCompanyStoreRepository.GetStoreIDByCompanyId(Convert.ToInt64(objUserInformation.CompanyId));
    //                    List<SelectMyIssuanceProductItemsResult> objFinal;
    //                    objMyIssCart = objMyIssCartRepo.GetCurrentOrderProducts(objOrder.MyShoppingCartID);
    //                    objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objMyIssCart, storeid);

    //                    GLCodeExists = objFinal.FirstOrDefault(le => le.GLCode != null) != null;

    //                    for (Int32 i = 0; i < objFinal.Count; i++)
    //                    {
    //                        String[] p = new String[1];
    //                        if (!String.IsNullOrEmpty(objFinal[i].NameToBeEngraved))
    //                        {
    //                            p = objFinal[i].NameToBeEngraved.Split(',');
    //                        }

    //                        innermessage = innermessage + "<tr>";
    //                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
    //                        innermessage = innermessage + objFinal[i].Qty;
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
    //                        innermessage = innermessage + objFinal[i].item;
    //                        innermessage = innermessage + "</td>";

    //                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
    //                        innermessage = innermessage + objFinal[i].sLookupName;
    //                        innermessage = innermessage + "</td>";

    //                        if (!GLCodeExists)
    //                        {
    //                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
    //                            innermessage = innermessage + objFinal[i].ProductDescrption;
    //                            innermessage = innermessage + "</td>";
    //                        }
    //                        else
    //                        {
    //                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
    //                            innermessage = innermessage + objFinal[i].ProductDescrption;
    //                            innermessage = innermessage + "</td>";
    //                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
    //                            innermessage = innermessage + objFinal[i].GLCode;
    //                            innermessage = innermessage + "</td>";
    //                        }

    //                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
    //                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)).ToString();
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
    //                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)) * Convert.ToDecimal(objFinal[i].Qty);

    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "</tr>";
    //                        innermessage = innermessage + "<tr>";

    //                        if (GLCodeExists)
    //                            innermessage = innermessage + "<td colspan='8'>";
    //                        else
    //                            innermessage = innermessage + "<td colspan='7'>";

    //                        innermessage = innermessage + "<hr />";
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "</tr>";

    //                    }

    //                    messagebody.Replace("{innermessage}", innermessage);

    //                }

    //                if (GLCodeExists)
    //                    messagebody.Replace("<td id=\"tdPD\" width=\"48%\"", "<td id=\"tdPD\" width=\"38%\"");
    //                else
    //                    messagebody.Replace(@"<td id=""tdGC"" width=""10%"" style=""font-weight: bold; text-align: center;"">
    //                                                    GL Code
    //                                                </td>", "");
    //                #endregion

    //                messagebody.Replace("{Order Notes:}", "Order Notes :");
    //                messagebody.Replace("{ShippingCostView}", "Shipping :");
    //                messagebody.Replace("{SalesTaxView}", "Sales Tax :");
    //                messagebody.Replace("{OrderTotalView}", "Order Total :");
    //                messagebody.Replace("{innermesaageforsupplier}", "");
    //                String b = NameBars(objOrder.OrderID);
    //                if (b != null)
    //                {
    //                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + b + (IsTestOrder ? @"<br/><span style=""color:red;font-weight:bold;"">THIS IS A TEST ORDER. PLEASE DO NOT PROCESS.</span>" : ""));
    //                }
    //                else
    //                {
    //                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + (IsTestOrder ? @"<br/><span style=""color:red;font-weight:bold;"">THIS IS A TEST ORDER. PLEASE DO NOT PROCESS.</span>" : ""));
    //                }

    //                messagebody.Replace("{ShippingCost}", objOrder.ShippingAmount.ToString());
    //                messagebody.Replace("{Saletax}", "$" + objOrder.SalesTax.ToString());
    //                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + (objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount))).ToString());
    //                if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)//check that corporate discount is applied or not
    //                {
    //                    messagebody.Replace("{CorporateDiscountView}", "Corporate Discount");
    //                    messagebody.Replace("{CorporateDiscount}", objOrder.CorporateDiscount.ToString());
    //                }
    //                else
    //                {
    //                    messagebody.Replace("{CorporateDiscountView}", "");
    //                    messagebody.Replace("{CorporateDiscount}", "");
    //                }
    //                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

    //                EmailNotificationDetails objEmailNotificationDetails = new EmailNotificationDetails();
    //                objEmailNotificationDetails.MessageBody = messagebody.ToString();
    //                objEmailNotificationDetails.sFromAddress = sFrmadd;
    //                objEmailNotificationDetails.sFromName = sFrmname;
    //                objEmailNotificationDetails.sSubject = sSubject;

    //                return objEmailNotificationDetails;
    //            }
    //            else
    //                return null;

    //        }
    //        catch (Exception ex)
    //        {
    //            ErrHandler.WriteError(ex);
    //            return null;
    //        }
    //    }

    //    /// <summary>
    //    /// Sends the verification email MOAS manager for approve order.
    //    /// Add By Mayur on 6-feb-2012
    //    /// </summary>
    //    /// <param name="OrderNumber">The order number.</param>
    //    /// <param name="MOASEmailAddress">The MOAS email address.</param>
    //    /// <param name="FullName">The full name.</param>
    //    public static EmailNotificationDetails GetEmailMessageBodyForMOASManager(Int64 OrderId, Boolean IsTestOrder)
    //    {
    //        #region DataMembers
    //        UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
    //        OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
    //        LookupRepository objLookupRepository = new LookupRepository();
    //        CompanyRepository objCmpRepo = new CompanyRepository();
    //        CompanyEmployeeRepository objCERepo = new CompanyEmployeeRepository();
    //        CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    //        CompanyEmployeeContactInfo objShippingInfo;
    //        CompanyEmployeeContactInfo objBillingInfo;
    //        CityRepository objCity = new CityRepository();
    //        StateRepository objState = new StateRepository();
    //        CountryRepository objCountry = new CountryRepository();
    //        MyIssuanceCartRepository objMyIssCartRepo = new MyIssuanceCartRepository();
    //        MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    //        List<SelectMyShoppingCartProductResult> objSelectMyShoppingCartProductResultList;
    //        #endregion
    //        try
    //        {
    //            EmailTemplateBE objEmailBE = new EmailTemplateBE();
    //            EmailTemplateDA objEmailDA = new EmailTemplateDA();
    //            DataSet dsEmailTemplate;

    //            string sFrmadd = string.Empty, sFrmname = string.Empty, sSubject = string.Empty;

    //            //Get Email Content
    //            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
    //            objEmailBE.STemplateName = "Order Placed MOAS";
    //            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
    //            if (dsEmailTemplate != null)
    //            {
    //                Order objOrder = objRepos.GetByOrderID(OrderId);
    //                Boolean IsMOASWithCostCenterCode = objCERepo.FUN_IsMOASWithCostCenterCode(Convert.ToInt64(objOrder.UserId));

    //                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
    //                CompanyEmployee objCompanyEmployee = objCERepo.GetByUserInfoId(objUserInformation.UserInfoID);

    //                sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
    //                sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
    //                sSubject = objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber.ToString();

    //                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
    //                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());

    //                String PaymentMethod = String.Empty;
    //                if (objOrder.PaymentOption != null)
    //                    PaymentMethod = objLookupRepository.GetById((Int64)objOrder.PaymentOption).sLookupName;

    //                messagebody.Replace("{firstnote}", "Please review the following order below that requires your attention. You can Approve, Edit or Cancel this order by clicking the buttons on the bottom of this page.");
    //                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
    //                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
    //                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
    //                messagebody.Replace("{referencename}", objOrder.ReferenceName);
    //                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
    //                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());

    //                #region Check here for CreditUsed
    //                String cr;
    //                Boolean creditused = true;
    //                if (objOrder.CreditUsed != "0")
    //                {
    //                    if (String.IsNullOrEmpty(objOrder.CreditUsed))
    //                    {
    //                        creditused = false;
    //                    }
    //                    else
    //                    {
    //                        creditused = true;
    //                    }
    //                }
    //                else
    //                {
    //                    creditused = false;
    //                }
    //                #endregion

    //                #region PaymentType and PaymentMethod
    //                //Both Option have used
    //                if (objOrder.PaymentOption != null && creditused)
    //                {

    //                    if (objOrder.CreditUsed == "Previous")
    //                    {
    //                        cr = "Starting " + "Credits Paid by Corporate ";
    //                        /*Strating Credit Row*/
    //                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits :");
    //                        messagebody.Replace("{lblStCrAmntApplied}", objOrder.CreditAmt.ToString());
    //                        /*End*/
    //                    }
    //                    else
    //                    {
    //                        cr = "Anniversary " + "Credits Paid by Corporate ";
    //                        /*Anniversary Credit Row*/
    //                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits :");
    //                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
    //                        /*End*/

    //                    }
    //                    /*Payment Option Row*/
    //                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + PaymentMethod + " : ");
    //                    //updated by Prashant 27th feb 2013
    //                    //MOAS L1 Price Change for CA User
    //                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
    //                    //updated by Prashant 27th feb 2013
    //                    //MOAS L1 Price Change for CA User

    //                    /*End*/
    //                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + PaymentMethod);
    //                }
    //                //Only Starting or Anniversary
    //                else if (objOrder.PaymentOption == null && creditused)
    //                {
    //                    if (objOrder.CreditUsed == "Previous")
    //                    {
    //                        cr = "Starting " + "Credits - Paid by Corporate ";
    //                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits:");
    //                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
    //                    }
    //                    else
    //                    {
    //                        cr = "Anniversary " + "Credits - Paid by Corporate ";
    //                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits:");
    //                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
    //                    }
    //                    /*Payment Option Row*/
    //                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
    //                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
    //                    /*End*/
    //                    messagebody.Replace("{PaymentType}", cr);
    //                }
    //                //Only Payment Option
    //                else if (objOrder.PaymentOption != null && !creditused)
    //                {
    //                    messagebody.Replace("{PaymentType}", PaymentMethod);
    //                    /*Anniversary Credit Row*/
    //                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
    //                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
    //                    /*End*/
    //                    /*Payment Option Row*/
    //                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + PaymentMethod + " : ");
    //                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
    //                    /*End*/
    //                }
    //                else
    //                {
    //                    messagebody.Replace("{PaymentType}", "Paid By Corporate");
    //                    /*Anniversary Credit Row*/
    //                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
    //                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
    //                    /*End*/

    //                    /*Payment Option Row*/
    //                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
    //                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
    //                    /*End*/
    //                }
    //                messagebody.Replace("{Payment Method :}", "Payment Method :");
    //                #endregion

    //                #region PaymentOptionCode
    //                //For displaying payment option code
    //                if (IsMOASWithCostCenterCode)
    //                {
    //                    messagebody.Replace("{PaymentOptionCodeLabel}", "Cost Center Code :");
    //                    messagebody.Replace("{PaymentOptionCodeValue}", objOrder.PaymentOptionCode);
    //                }
    //                else
    //                {
    //                    messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
    //                    messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);
    //                }
    //                #endregion

    //                #region Billing Address
    //                if (objOrder.OrderFor == "ShoppingCart") // Company Pays
    //                {
    //                    objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
    //                }
    //                else
    //                {
    //                    objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
    //                }
    //                messagebody.Replace("{Billing Address}", "Bill To:");
    //                messagebody.Replace("{NameView}", "Name :");
    //                messagebody.Replace("{CompanyNameView}", "Company Name :");
    //                messagebody.Replace("{AddressView}", "Address :");
    //                // messagebody.Replace("{Address2}", lblBadr.Text);
    //                messagebody.Replace("{CityView}", "City :");
    //                messagebody.Replace("{StateView}", "State :");
    //                messagebody.Replace("{ZipView}", "Zip :");
    //                messagebody.Replace("{CountryView}", "Country :");
    //                messagebody.Replace("{PhoneView}", "Phone :");
    //                messagebody.Replace("{EmailView}", "Email :");

    //                messagebody.Replace("{Name}", objBillingInfo.BillingCO + " " + objBillingInfo.Manager);
    //                messagebody.Replace("{CompanyName}", objBillingInfo.CompanyName);
    //                messagebody.Replace("{Address1}", objBillingInfo.Address);
    //                // messagebody.Replace("{Address2}", lblBadr.Text);
    //                messagebody.Replace("{City}", objCity.GetById((Int64)objBillingInfo.CityID).sCityName);
    //                messagebody.Replace("{State}", objState.GetById((Int64)objBillingInfo.StateID).sStatename);
    //                messagebody.Replace("{Zip}", objBillingInfo.ZipCode);
    //                messagebody.Replace("{Country}", objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName);
    //                messagebody.Replace("{Phone}", objBillingInfo.Telephone);
    //                messagebody.Replace("{Email}", objBillingInfo.Email);
    //                #endregion

    //                #region shipping address
    //                if (objOrder.OrderFor == "ShoppingCart")
    //                {
    //                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
    //                }
    //                else
    //                {
    //                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
    //                }
    //                messagebody.Replace("{Shipping Address}", "Ship To:");
    //                messagebody.Replace("{AirportView1}", "Airport :");
    //                messagebody.Replace("{DepartmentView1}", "Department :");
    //                messagebody.Replace("{NameView1}", "Name :");
    //                messagebody.Replace("{CompanyNameView1}", "Company Name :");
    //                messagebody.Replace("{AddressView1}", "Address :");
    //                // messagebody.Replace("{Address2}", lblBadr.Text);
    //                messagebody.Replace("{CityView1}", "City :");
    //                messagebody.Replace("{CountyView1}", "County :");
    //                messagebody.Replace("{StateView1}", "State :");
    //                messagebody.Replace("{ZipView1}", "Zip :");
    //                messagebody.Replace("{CountryView1}", "Country :");
    //                messagebody.Replace("{PhoneView1}", "Phone :");
    //                messagebody.Replace("{EmailView1}", "Email :");

    //                messagebody.Replace("{Airport1}", objShippingInfo.Airport);
    //                if (objShippingInfo.DepartmentID != null)
    //                    messagebody.Replace("{Department1}", new LookupRepository().GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
    //                else
    //                    messagebody.Replace("{Department1}", "");
    //                messagebody.Replace("{Name1}", objShippingInfo.Name + " " + objShippingInfo.Fax);
    //                messagebody.Replace("{CompanyName1}", objShippingInfo.CompanyName);
    //                messagebody.Replace("{Address11}", objShippingInfo.Address + "<br/>" + objShippingInfo.Address2 + "<br/>" + objShippingInfo.Street);
    //                messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
    //                messagebody.Replace("{County1}", objShippingInfo.county);
    //                messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
    //                messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
    //                messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
    //                messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
    //                messagebody.Replace("{Email1}", objShippingInfo.Email);
    //                #endregion

    //                #region order product detail
    //                String innermessage = "";
    //                Boolean GLCodeExists = false;

    //                if (objOrder.OrderFor == "ShoppingCart")
    //                {
    //                    objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.MyShoppingCartID);

    //                    GLCodeExists = objSelectMyShoppingCartProductResultList.FirstOrDefault(le => le.GLCode != null) != null;

    //                    for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
    //                    {
    //                        String[] p = new String[1];
    //                        if (!String.IsNullOrEmpty(objSelectMyShoppingCartProductResultList[i].NameToBeEngraved))
    //                        {
    //                            p = objSelectMyShoppingCartProductResultList[i].NameToBeEngraved.Split(',');
    //                        }
    //                        innermessage = innermessage + "<tr>";
    //                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
    //                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Quantity;
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
    //                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].item;
    //                        innermessage = innermessage + "</td>";

    //                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
    //                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Size;
    //                        innermessage = innermessage + "</td>";

    //                        if (!GLCodeExists)
    //                        {
    //                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
    //                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
    //                            innermessage = innermessage + "</td>";
    //                        }
    //                        else
    //                        {
    //                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
    //                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
    //                            innermessage = innermessage + "</td>";
    //                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
    //                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].GLCode;
    //                            innermessage = innermessage + "</td>";
    //                        }

    //                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
    //                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)).ToString();
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
    //                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)) * Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].Quantity);
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "</tr>";
    //                        innermessage = innermessage + "<tr>";

    //                        if (GLCodeExists)
    //                            innermessage = innermessage + "<td colspan='8'>";
    //                        else
    //                            innermessage = innermessage + "<td colspan='7'>";

    //                        innermessage = innermessage + "<hr />";
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "</tr>";

    //                    }
    //                    messagebody.Replace("{innermessage}", innermessage);
    //                }
    //                else
    //                {
    //                    Int64? storeid;
    //                    storeid = new CompanyStoreRepository().GetStoreIDByCompanyId(Convert.ToInt64(objUserInformation.CompanyId));
    //                    List<SelectMyIssuanceProductItemsResult> objFinal;
    //                    List<MyIssuanceCart> objMyIssCart = new List<MyIssuanceCart>();
    //                    objMyIssCart = objMyIssCartRepo.GetCurrentOrderProducts(objOrder.MyShoppingCartID);
    //                    objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objMyIssCart, storeid);

    //                    GLCodeExists = objFinal.FirstOrDefault(le => le.GLCode != null) != null;

    //                    for (Int32 i = 0; i < objFinal.Count; i++)
    //                    {
    //                        String[] p = new String[1];
    //                        if (!String.IsNullOrEmpty(objFinal[i].NameToBeEngraved))
    //                        {
    //                            p = objFinal[i].NameToBeEngraved.Split(',');
    //                        }
    //                        innermessage = innermessage + "<tr>";
    //                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
    //                        innermessage = innermessage + objFinal[i].Qty;
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
    //                        innermessage = innermessage + objFinal[i].item;
    //                        innermessage = innermessage + "</td>";

    //                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
    //                        innermessage = innermessage + objFinal[i].sLookupName;
    //                        innermessage = innermessage + "</td>";

    //                        if (!GLCodeExists)
    //                        {
    //                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
    //                            innermessage = innermessage + objFinal[i].ProductDescrption;
    //                            innermessage = innermessage + "</td>";
    //                        }
    //                        else
    //                        {
    //                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
    //                            innermessage = innermessage + objFinal[i].ProductDescrption;
    //                            innermessage = innermessage + "</td>";
    //                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
    //                            innermessage = innermessage + objFinal[i].ProductDescrption;
    //                            innermessage = innermessage + "</td>";
    //                        }

    //                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
    //                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)).ToString();
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
    //                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)) * Convert.ToDecimal(objFinal[i].Qty);


    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "</tr>";
    //                        innermessage = innermessage + "<tr>";

    //                        if (GLCodeExists)
    //                            innermessage = innermessage + "<td colspan='8'>";
    //                        else
    //                            innermessage = innermessage + "<td colspan='7'>";

    //                        innermessage = innermessage + "<hr />";
    //                        innermessage = innermessage + "</td>";
    //                        innermessage = innermessage + "</tr>";

    //                    }

    //                    messagebody.Replace("{innermessage}", innermessage);

    //                }

    //                if (GLCodeExists)
    //                    messagebody.Replace("<td id=\"tdPD\" width=\"48%\"", "<td id=\"tdPD\" width=\"38%\"");
    //                else
    //                    messagebody.Replace(@"<td id=""tdGC"" width=""10%"" style=""font-weight: bold; text-align: center;"">
    //                                                    GL Code
    //                                                </td>", "");

    //                #endregion

    //                messagebody.Replace("{Order Notes:}", "Order Notes :");
    //                messagebody.Replace("{ShippingCostView}", "Shipping :");
    //                messagebody.Replace("{SalesTaxView}", "Sales Tax :");
    //                messagebody.Replace("{OrderTotalView}", "Order Total :");
    //                messagebody.Replace("{innermesaageforsupplier}", "");
    //                String b = NameBars(objOrder.OrderID);
    //                if (b != null)
    //                {
    //                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + b);
    //                }
    //                else
    //                {
    //                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
    //                }

    //                messagebody.Replace("{ShippingCost}", objOrder.ShippingAmount.ToString());
    //                messagebody.Replace("{Saletax}", "$" + objOrder.SalesTax.ToString());
    //                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + (objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount))).ToString());
    //                if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)//check that corporate discount is applied or not
    //                {
    //                    messagebody.Replace("{CorporateDiscountView}", "Corporate Discount");
    //                    messagebody.Replace("{CorporateDiscount}", objOrder.CorporateDiscount.ToString());
    //                }
    //                else
    //                {
    //                    messagebody.Replace("{CorporateDiscountView}", "");
    //                    messagebody.Replace("{CorporateDiscount}", "");
    //                }
    //                // messagebody.Replace("{PurchasesNotEligableforCreditUse}", lblNotEligibleforAnniversarycredit.Text);

    //                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

    //                //Set Conformation Button
    //                String buttonText = "";
    //                buttonText += "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
    //                buttonText += "<tr>";
    //                buttonText += "<td>";
    //                buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "MOASOrderStatus.aspx?UserId={MOASUserId}&OrderId=" + objOrder.OrderID + "&Status=Approve'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/approve_order_btn.png' alt='Approve Order' border='0'/></a>";
    //                buttonText += "<td>";
    //                buttonText += "<td>";
    //                buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "MOASOrderStatus.aspx?UserId={MOASUserId}&OrderId=" + objOrder.OrderID + "&Status=Cancel'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/cancel_order_btn.png' alt='Cancel Order' border='0'/></a>";
    //                buttonText += "<td>";
    //                buttonText += "<td>";
    //                buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "MyAccount/OrderManagement/EditOrderDetail.aspx?Id=" + objOrder.OrderID + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/edit_order_btn.png' alt='Edit Order' border='0'/></a>";
    //                buttonText += "<td>";
    //                buttonText += "</tr>";
    //                buttonText += "</table>";

    //                messagebody.Replace("{ConformationButton}", buttonText);

    //                EmailNotificationDetails objEmailNotificationDetails = new EmailNotificationDetails();
    //                objEmailNotificationDetails.MessageBody = messagebody.ToString();
    //                objEmailNotificationDetails.sFromAddress = sFrmadd;
    //                objEmailNotificationDetails.sFromName = sFrmname;
    //                objEmailNotificationDetails.sSubject = sSubject;

    //                return objEmailNotificationDetails;
    //            }
    //            else
    //                return null;

    //        }
    //        catch (Exception ex)
    //        {
    //            ErrHandler.WriteError(ex);
    //            return null;
    //        }
    //    }

    //    public static String NameBars(Int64 OrderId)
    //    {
    //        MyIssuanceCartRepository objMyIssCartRepo = new MyIssuanceCartRepository();
    //        MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    //        OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();
    //        String strNameBars = String.Empty;
    //        Order objOrder = objOrderConfirmationRepository.GetByOrderID(OrderId);
    //        if (objOrder.OrderFor == "IssuanceCart")
    //        {
    //            List<MyIssuanceCart> objIssList = new MyIssuanceCartRepository().GetCurrentOrderProducts(objOrder.MyShoppingCartID);
    //            foreach (MyIssuanceCart objItem in objIssList)
    //            {
    //                if (objItem.NameToBeEngraved != null && objItem.NameToBeEngraved != String.Empty)
    //                {
    //                    if (objItem.NameToBeEngraved.Contains(','))
    //                    {
    //                        String[] EmployeeTitle = objItem.NameToBeEngraved.Split(',');
    //                        strNameBars += "Name to be Engraved on Name Bar:" + EmployeeTitle[0] + "\n";
    //                        strNameBars += "Employee Title" + EmployeeTitle[1] + "\n";
    //                    }
    //                    else
    //                    {
    //                        strNameBars += "Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n";
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            List<SelectMyShoppingCartProductResult> objMyShpList = new MyShoppingCartRepository().SelectCurrentOrderProducts(objOrder.MyShoppingCartID);
    //            foreach (SelectMyShoppingCartProductResult objItem in objMyShpList)
    //            {
    //                if (objItem.NameToBeEngraved != null && objItem.NameToBeEngraved != String.Empty)
    //                {
    //                    if (objItem.NameToBeEngraved.Contains(','))
    //                    {
    //                        String[] EmployeeTitle = objItem.NameToBeEngraved.Split(',');
    //                        strNameBars += "Name to be Engraved on Name Bar:" + EmployeeTitle[0] + "\n";
    //                        strNameBars += "Employee Title:" + EmployeeTitle[1] + "\n";
    //                    }
    //                    else
    //                    {
    //                        strNameBars += "Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n";
    //                    }
    //                }
    //            }
    //        }

    //        return strNameBars.ToString();

    //    }

    //    public class EmailNotificationDetails
    //    {
    //        public string sFromAddress { get; set; }
    //        public string sFromName { get; set; }
    //        public string sSubject { get; set; }
    //        public string MessageBody { get; set; }
    //    }
    #endregion

    public static Boolean IsValidEmailAddress(String email)
    {
        Regex rexEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
        return rexEmail.IsMatch(email);
    }

    public static void SaveDocument(string strDirectoryPath, string FileName, Boolean IsToDeleteandSave, HttpPostedFile objPostedFile)
    {
        try
        {
            if (!Directory.Exists(strDirectoryPath))
            {
                Directory.CreateDirectory(strDirectoryPath);
            }
            if (IsToDeleteandSave && File.Exists(strDirectoryPath + @"\" + FileName))
            {
                File.Delete(strDirectoryPath + @"\" + FileName);
            }
            objPostedFile.SaveAs(strDirectoryPath + @"\" + FileName);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public static String MakeValidFileName(String fileName)
    {
        StringBuilder fileNameBuilder = new StringBuilder(fileName);

        Char[] invalidCharacters = System.IO.Path.GetInvalidFileNameChars();

        foreach (Char invalidCharacter in invalidCharacters)
        {
            fileNameBuilder.Replace(Convert.ToString(invalidCharacter), "");
        }

        return fileNameBuilder.ToString();
    }

    public static String GetFileType(string strContentType)
    {
        String strFileType = String.Empty;
        if (strContentType.ToLower().Contains("image/"))
        {
            strFileType = "Image";
        }
        else if (strContentType.ToLower().Contains("video/"))
        {
            strFileType = "Video";
        }
        else
        {
            strFileType = "Document";
        }
        return strFileType;
    }

    /// <summary>
    /// Bind dropdown and add onchange attribute
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ddl"></param>
    /// <param name="list"></param>
    /// <param name="DataTextField"></param>
    /// <param name="DataValueField"></param>
    /// <param name="FirstListItem"></param>
    public static void BindDropDown<T>(DropDownList ddl, List<T> list, string DataTextField, string DataValueField, string FirstListItem) where T : class
    {
        ddl.Items.Clear();
        ddl.ClearSelection();
        if (list.Count > 0)
        {
            ddl.DataSource = list;
            ddl.DataTextField = DataTextField;
            ddl.DataValueField = DataValueField;
            ddl.DataBind();
        }
        if (!string.IsNullOrEmpty(FirstListItem))
        {
            ddl.Items.Insert(0, new ListItem(FirstListItem, "0"));
            ddl.SelectedValue = "0";
        }
        
    }
    /// <summary>
    /// Save all email contents as HTML file  and Return File Name 
    /// </summary>
    /// <param name="emailContents"></param>
    /// <param name="Subject"></param>
    /// <returns></returns>
    public String SaveFileAsHtml(String emailContents, String Subject)
    {
        String path = "~/TodaysEmailFiles/" + DateTime.Today.ToString("dd-MMM-yy") + "/";
        String fileName = path + MakeValidFileName(Subject) + "_" + DateTime.Now.Ticks + ".html";
        Boolean folderExists = Directory.Exists(HttpContext.Current.Server.MapPath(path));
        if (!folderExists)
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));

        String filePath = HttpContext.Current.Server.MapPath(fileName);
        File.WriteAllText(filePath, emailContents);
        return fileName;
    }

    public static string GetMyHelpVideo(string VideoType,string ModuleName,string PageOrPopupTitle)
    {
        MediaRepository objmedia = new MediaRepository();
        String strVideoUrl = String.Empty;
        GetMediaHelpVideoOrDocResult ObjPageVideo = objmedia.GetMediaHelpVideoOrDoc(VideoType, ModuleName, PageOrPopupTitle, Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
        if (ObjPageVideo != null)
        {
            strVideoUrl = ObjPageVideo.Vimeourl;

            MediaFile objfile = objmedia.GetFileById(ObjPageVideo.mediafileid);
            if (objfile != null)
            {
                objfile.View = objfile.View == null ? 1 : objfile.View + 1;
                objmedia.SubmitChanges();
            }
        }
        return strVideoUrl;
    }

    public static string GetMediaDocLink(string Doctype, string ModuleName, string PageOrPopupTitle)
    {
        MediaRepository objmedia = new MediaRepository();
        String strVideoUrl = String.Empty;
        GetMediaHelpVideoOrDocResult ObjPageVideo = objmedia.GetMediaHelpVideoOrDoc(Doctype, ModuleName, PageOrPopupTitle, Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
        if (ObjPageVideo != null)
        {
            strVideoUrl = ObjPageVideo.OriginalFileName;
            MediaFile objfile = objmedia.GetFileById(ObjPageVideo.mediafileid);
            if (objfile != null)
            {
                objfile.View = objfile.View == null ? 1 : objfile.View + 1;
                objmedia.SubmitChanges();
            }
        }
        return strVideoUrl;
    }
}
