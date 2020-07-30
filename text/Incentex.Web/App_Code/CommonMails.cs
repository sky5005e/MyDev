using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;

/// <summary>
/// Summary description for SendMails
/// </summary>
public class CommonMails
{
    public CommonMails()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Support Ticket Email Properties

    public static Boolean Live
    {
        get
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["SupportTicketEmailEnvironmentLive"]);
        }
    }

    private static Boolean OrderSummaryLive
    {
        get
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["OrderSummaryNotesEnvironmentLive"]);
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

    public static String ReplyToOrderSummary
    {
        get
        {
            return Live ? "ordernotes@world-link.us.com" : "report.incentex@gmail.com";
        }
    }

    public static String ReplyToOrderSummaryPassword
    {
        get
        {
            return Live ? "order@notes" : "report@incentex";
        }
    }

    public static String ReplyToHeadserRepairCenter
    {
        get
        {
            return Live ? "headsetrepairnotes@world-link.us.com" : "headsetrepaircenter@gmail.com";
        }
    }

    public static string ReplayTopurchaseordermanagement
    {
        get
        {
            return Live ? "purchaseorder@world-link.us.com" : "purchaseordermanagement10@gmail.com";
        }
    }

    public static string ReplyToAssetManagement
    {
        get {
            return Live ? "replytoGSE@world-link.us.com" : "replytonewasset@gmail.com";
        }
    }

    public static string ReplyToAssetManagementPassword
    {
        get
        {
            return Live ? "kA#6BMR8" : "reply@asset";
        }
    }

    public static string ReplayTopurchaseordermanagementPassword
    {
        get
        {
            return Live ? "E8$a3sth" : "order@incentex";
        }
    }

    public static String ReplyToHeadserRepairCenterPassword
    {
        get
        {
            return Live ? "uR$6bqHa" : "headsetrepair@incentex";
        }
    }

    public static String EmailFrom
    {
        get
        {
            return "notifications@world-link.us.com";
        }
    }

    public static String DisplyName
    {
        get
        {
            return "Incentex";
        }
    }

    #endregion

    #region Send Mail

    public void SendEmail4Local(Int64 UserInfoID, String SentFor, String ToAddress, String subject, String messageBody, String FromAddress, String Password, bool isHtml)
    {
        SendEmail4Local(UserInfoID, SentFor, ToAddress, subject, messageBody, FromAddress, Password, isHtml, null);
    }

    public void SendEmail4Local(Int64 UserInfoID, String SentFor, String ToAddress, String subject, String messageBody, String FromAddress, String Password, bool isHtml, String ModuleName, String SubMenu)
    {
        SendEmail4Local(UserInfoID, SentFor, ToAddress, subject, messageBody, FromAddress, Password, isHtml, null, ModuleName, SubMenu);
    }

    public void SendEmail4Local(Int64 UserInfoID, String SentFor, String ToAddress, String subject, String messageBody, String FromAddress, String Password, bool isHtml, Int64? OrderID)
    {
        MailMessage mail = new MailMessage();
        SmtpClient mailclient = new SmtpClient();
        //
        // Set the to and from addresses.
        // The from address must be your GMail account

        mail.From = new MailAddress(FromAddress);
        mail.To.Add(new MailAddress(ToAddress));
        //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

        //mailclient.DeliveryMethod = SmtpDeliveryMethod.Network;


        // Define the message
        mail.Subject = "Test Environment : " + subject;
        mail.IsBodyHtml = isHtml;
        mail.Body = messageBody;

        // Create a new Smpt Client using Google's servers
        // var mailclient = new SmtpClient();
        mailclient.Host = "smtp.gmail.com";//ForGmail
        mailclient.Port = 25; //ForGmail

        mailclient.EnableSsl = true;//ForGmail
        //mailclient.EnableSsl = false;
        //mailclient.UseDefaultCredentials = true;
        mailclient.Credentials = new System.Net.NetworkCredential(FromAddress, Password);//ForGmail

        try
        {
            //IMail em = mail.s;
            mailclient.Send(mail);
            //Insert Details into TodaysEmailDetails
            if (UserInfoID != 0)
                InsertIntoTodaysEmailDetails(messageBody, subject, SentFor, UserInfoID, OrderID);
        }
        catch (SmtpFailedRecipientsException ex)
        {
            for (int i = 0; i < ex.InnerExceptions.Length; i++)
            {
                SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
                {
                    // Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                    System.Threading.Thread.Sleep(5000);
                    mailclient.Send(mail);
                }
                else
                {
                    throw ex;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }

    public void SendEmail4Local(Int64 UserInfoID, String SentFor, String ToAddress, String subject, String messageBody, String FromAddress, String Password, bool isHtml, Int64? OrderID, String ModuleName, String SubMenu)
    {
        MailMessage mail = new MailMessage();
        SmtpClient mailclient = new SmtpClient();
        //
        // Set the to and from addresses.
        // The from address must be your GMail account

        mail.From = new MailAddress(FromAddress);
        mail.To.Add(new MailAddress(ToAddress));
        //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

        //mailclient.DeliveryMethod = SmtpDeliveryMethod.Network;


        // Define the message
        mail.Subject = "Test Environment : " + subject;
        mail.IsBodyHtml = isHtml;
        mail.Body = messageBody;

        // Create a new Smpt Client using Google's servers
        // var mailclient = new SmtpClient();
        mailclient.Host = "smtp.gmail.com";//ForGmail
        mailclient.Port = 25; //ForGmail

        mailclient.EnableSsl = true;//ForGmail
        //mailclient.EnableSsl = false;
        //mailclient.UseDefaultCredentials = true;
        mailclient.Credentials = new System.Net.NetworkCredential(FromAddress, Password);//ForGmail

        try
        {
            //IMail em = mail.s;
            mailclient.Send(mail);
            //Insert Details into TodaysEmailDetails
            if (UserInfoID != 0)
                InsertIntoTodaysEmailDetails(messageBody, subject, SentFor, UserInfoID, OrderID, ModuleName, SubMenu);
        }
        catch (SmtpFailedRecipientsException ex)
        {
            for (int i = 0; i < ex.InnerExceptions.Length; i++)
            {
                SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
                {
                    // Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                    System.Threading.Thread.Sleep(5000);
                    mailclient.Send(mail);
                }
                else
                {
                    throw ex;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }

    public bool SendMail(Int64 UserInfoID, String sentFor, string psFromAddress, string psToAddress, string psSubject, string psMessageBody, string Fromname
        , string Host, Int32 Port, bool SSL, bool HTMLBody)
    {
        return SendMail(UserInfoID, sentFor, psFromAddress, psToAddress, psSubject, psMessageBody, Fromname, Host, Port, SSL, HTMLBody, null);
    }

    public bool SendMail(Int64 UserInfoID, String sentFor, string psFromAddress, string psToAddress, string psSubject, string psMessageBody, string Fromname
        , string Host, Int32 Port, bool SSL, bool HTMLBody, Int64? OrderID)
    {
        return SendMail(UserInfoID, sentFor, psFromAddress, psToAddress, psSubject, psMessageBody, Fromname, Host, Port, String.Empty, String.Empty, SSL, HTMLBody, OrderID);
    }

    /// <summary>
    /// Simple Mail
    /// </summary>
    /// <param name="UserInfoID">UserInfoId</param>
    /// <param name="sentFor">Reason of Send Mail: Campaign/Service Ticket/ Product Order etc</param>
    /// <param name="psFromAddress"></param>
    /// <param name="psToAddress"></param>
    /// <param name="psSubject"></param>
    /// <param name="psMessageBody"></param>
    /// <param name="Fromname"></param>
    /// <param name="SSL"></param>
    /// <param name="HTMLBody"></param>
    /// <returns></returns>
    public bool SendMail(Int64 UserInfoID, String sentFor, string psFromAddress, string psToAddress, string psSubject, string psMessageBody, string Fromname
        , string Host, Int32 Port, String userName, String password, bool SSL, bool HTMLBody, Int64? OrderID)
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

            smtpmail.Host = Host;
            smtpmail.Port = Port;

            oMailMessage.To.Add(Convert.ToString(ConfigurationManager.AppSettings["testSystemInbox"]));
            //oMailMessage.To.Add(psToAddress);

            oMailMessage.From = FromAddress;
            oMailMessage.Subject = "Test Environment : " + psSubject;
            oMailMessage.IsBodyHtml = HTMLBody;
            oMailMessage.Body = psMessageBody;
            oMailMessage.Priority = MailPriority.High;

            // Send undeliver email
            // email : incentexbounce@gmail.com
            // Password : worldlink
            // To check whether any  fail message send to return path.
            oMailMessage.Headers.Add("Disposition-Notification-To", "<incentexbounce@gmail.com>");
            oMailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            oMailMessage.Headers.Add("Return-Path", "<incentexbounce@gmail.com>");
            oMailMessage.ReplyTo = new MailAddress("<incentexbounce@gmail.com>");
            oMailMessage.Sender = new MailAddress("<incentexbounce@gmail.com>");

            smtpmail.EnableSsl = SSL;

            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
                smtpmail.Credentials = new NetworkCredential(userName, password);

            smtpmail.Send(oMailMessage);
            //Insert Details into TodaysEmailDetails
            if (UserInfoID != 0)
                InsertIntoTodaysEmailDetails(psMessageBody, psSubject, sentFor, UserInfoID, OrderID);
            Result = true;
        }
        catch
        {
            Result = false;
        }

        return Result;
    }

    /// <summary>
    /// Function for sending email with reply to address different than from address.
    /// </summary>
    /// <param name="UserInfoID">UserInfoId</param>
    /// <param name="sentFor">Reason of Send Mail: Campaign/Service Ticket/ Product Order etc</param>
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
    public Boolean SendMailWithReplyTo(Int64 UserInfoID, String sentFor, String psFromAddress, String psToAddress, String psSubject, String psMessageBody, String Fromname,
        String psReplyToAddress, String psUserName, String psPassword, String psHost, String psPort, String psPriority, Boolean SSL, Boolean HTMLBody)
    {
        return SendMailWithReplyTo(UserInfoID, sentFor, psFromAddress, psToAddress, psSubject, psMessageBody, Fromname, psReplyToAddress, psUserName, psPassword, psHost, psPort, psPriority, SSL, HTMLBody, null);
    }

    public Boolean SendMailWithReplyTo(Int64 UserInfoID, String sentFor, String psFromAddress, String psToAddress, String psSubject, String psMessageBody, String Fromname,
        String psReplyToAddress, String psUserName, String psPassword, String psHost, String psPort, String psPriority, Boolean SSL, Boolean HTMLBody, Int64? OrderID)
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

            oMailMessage.To.Add(Convert.ToString(ConfigurationManager.AppSettings["testSystemInbox"]));

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
            //Insert Details into TodaysEmailDetails
            if (UserInfoID != 0)
                InsertIntoTodaysEmailDetails(psMessageBody, psSubject, sentFor, UserInfoID, OrderID);
            Result = true;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return Result;
    }
    /// <summary>
    /// This is to send mail of Service Ticket for Recipients
    /// </summary>
    /// <param name="TicketID"></param>
    /// <param name="UserInfoID"></param>
    /// <param name="SentFor"></param>
    /// <param name="NoteType"></param>
    /// <param name="To"></param>
    /// <param name="Body"></param>
    /// <param name="Host"></param>
    /// <param name="Port"></param>
    /// <param name="Subject"></param>
    /// <returns></returns>
    public Boolean SendServiceTicketReciept(Int64 TicketID, String UserInfoID, String SentFor, Int32 NoteType, String To, String Body, String Host, String Port, String Subject)
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

            String from = String.Empty;
            String fromName = String.Empty;
            String ReplyTo = String.Empty;
            String userName = String.Empty;
            String password = String.Empty;
            String priority = String.Empty;
            Boolean ssl;
            Boolean IsHTML;

            from = EmailFrom;
            fromName = "Incentex";
            ReplyTo = OriginalReplyTo.Substring(0, OriginalReplyTo.IndexOf('@')) + "+tn" + TicketID + "un" + UserInfoID + "nt" + NoteType + "en" + OriginalReplyTo.Substring(OriginalReplyTo.IndexOf('@'), OriginalReplyTo.Length - OriginalReplyTo.IndexOf('@'));

            userName = UserName;
            password = Password;
            priority = "Normal";
            ssl = SSL;
            Host = SMTPHost;
            Port = Convert.ToString(SMTPPort);

            IsHTML = true;

            Success = SendMailWithReplyTo(Convert.ToInt64(UserInfoID != "annx" ? UserInfoID : "0"), SentFor, from, To, Subject, Body, fromName, ReplyTo, userName, password, Host, Port, priority, ssl, IsHTML);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        #endregion

        return Success;
    }
    /// <summary>
    /// This Method is for Send Mail with Attachment
    /// </summary>
    /// <param name="UserInfoID">UserInfoId</param>
    /// <param name="sentFor">Reason of Send Mail: Campaign/Service Ticket/ Product Order etc</param>
    /// <param name="psFromAddress">From email Address</param>
    /// <param name="psToAddress">To eamil Address</param>
    /// <param name="psSubject">Subject of Mail</param>
    /// <param name="psMessageBody">Message Body</param>
    /// <param name="Fromname">From Name of Sender</param>
    /// <param name="SSL">Socket For Data Communications</param>
    /// <param name="HTMLBody">Html Body </param>
    /// <param name="FilePath">File Path from where it will collect file</param>
    /// <param name="Host">Host name </param>
    /// <param name="Port">Port Number</param>
    /// <param name="uName">User Name of smtp.configuration</param>
    /// <param name="Pwd">Password of Smtp</param>
    /// <param name="OFileName">File name</param>
    /// <param name="IsForLive">For Live/Localhost</param>
    /// <returns>if Successful then return true else false</returns>
    public bool SendMailWithAttachment(Int64 UserInfoID, String sentFor, string psFromAddress, string psToAddress, string psSubject, string psMessageBody, string Fromname
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


            oMailMessage.To.Add(Convert.ToString(ConfigurationManager.AppSettings["testSystemInbox"]));
            //oMailMessage.To.Add(psToAddress);

            oMailMessage.From = FromAddress;
            oMailMessage.Subject = "Test Environment : " + psSubject;
            oMailMessage.IsBodyHtml = HTMLBody;
            oMailMessage.Body = psMessageBody;
            oMailMessage.Priority = MailPriority.High;

            smtpmail.EnableSsl = SSL;

            smtpmail.Send(oMailMessage);

            //Insert Details into TodaysEmailDetails
            if (UserInfoID != 0)
                InsertIntoTodaysEmailDetails(psMessageBody, psSubject, sentFor, UserInfoID, null);

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
    /// This Method is for Send Mail with Multiple Attachment
    /// </summary>
    /// <param name="UserInfoID">UserInfoId</param>
    /// <param name="sentFor">Reason of Send Mail: Campaign/Service Ticket/ Product Order etc</param>
    /// <param name="psFromAddress">From email Address</param>
    /// <param name="psToAddress">To eamil Address</param>
    /// <param name="psSubject">Subject of Mail</param>
    /// <param name="psMessageBody">Message Body</param>
    /// <param name="Fromname">From Name of Sender</param>
    /// <param name="SSL">Socket For Data Communications</param>
    /// <param name="HTMLBody">Html Body </param>
    /// <param name="FileList">List of File</param>
    /// <param name="FilePath">File Path from where it will collect all file</param>
    /// <param name="Host">Host name </param>
    /// <param name="Port">Port Number</param>
    /// <param name="uName">User Name of smtp.configuration</param>
    /// <param name="Pwd">Password of Smtp</param>
    /// <param name="OFileName">File name</param>
    /// <param name="IsForLive">For Live/Localhost</param>
    /// <returns>if Successful then return true else false</returns>
    public bool SendMailWithMultiAttachment(Int64 UserInfoID, String sentFor, string psFromAddress, string psToAddress, string psSubject, string psMessageBody, string Fromname
       , bool SSL, bool HTMLBody, List<UploadImage> FileList, String FilePath, string Host, Int32 Port, string uName, string Pwd, string OFileName, bool IsForLive)
    {

        bool Result = false;
        try
        {
            System.Net.Mail.MailMessage oMailMessage = new System.Net.Mail.MailMessage();
            SmtpClient smtpmail = new SmtpClient();
            if (!IsForLive)
            {
                NetworkCredential objNet = new NetworkCredential(uName, Pwd);
                smtpmail.Credentials = objNet;
            }
            smtpmail.Host = Host;
            smtpmail.Port = Port;

            MailAddress FromAddress = default(MailAddress);
            if (FileList != null && FileList.Count > 0)
            {
                for (int i = 0; i < FileList.Count; i++)
                {
                    string FP = FilePath + FileList[i].imageOnly;
                    Attachment objAttach = new Attachment(FP);
                    if (objAttach != null)
                    {
                        System.Net.Mime.ContentDisposition obj = objAttach.ContentDisposition;
                        obj.FileName = FileList[i].imageOnly;
                        oMailMessage.Attachments.Add(objAttach);
                    }
                }
            }

            if (string.IsNullOrEmpty(Fromname))
            {
                FromAddress = new MailAddress(psFromAddress);
            }
            else
            {
                FromAddress = new MailAddress(psFromAddress, Fromname);
            }

            oMailMessage.To.Add(Convert.ToString(ConfigurationManager.AppSettings["testSystemInbox"]));
            //oMailMessage.To.Add(psToAddress);

            oMailMessage.From = FromAddress;
            oMailMessage.Subject = "Test Environment : " + psSubject;
            oMailMessage.IsBodyHtml = HTMLBody;
            oMailMessage.Body = psMessageBody;
            oMailMessage.Priority = System.Net.Mail.MailPriority.High;
            // Send undeliver email
            //email : incentexbounce@gmail.com
            // Password : worldlink
            // To check whether any  fail message send to return path.
            oMailMessage.Headers.Add("Disposition-Notification-To", "<incentexbounce@gmail.com>");
            oMailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            oMailMessage.Headers.Add("Return-Path", "<incentexbounce@gmail.com>");
            oMailMessage.ReplyTo = new MailAddress("<incentexbounce@gmail.com>");
            oMailMessage.Sender = new MailAddress("<incentexbounce@gmail.com>");

            smtpmail.EnableSsl = SSL;

            smtpmail.Send(oMailMessage);
            //Insert Details into TodaysEmailDetails
            if (UserInfoID != 0)
                InsertIntoTodaysEmailDetails(psMessageBody, psSubject, sentFor, UserInfoID, null);

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
    /// To Insert Records in TodaysEmailDetails Table
    /// </summary>
    /// <param name="Messagebody">Message Contains or Body of Email</param>
    /// <param name="EmailSubject">Email Subject</param>
    /// <param name="SentFor">Mail type ie Support ticket or Order confirmations or Comm center etc . </param>
    /// <param name="UserInfoId">User Info id for which mail is send</param>

    public void InsertIntoTodaysEmailDetails(String Messagebody, String EmailSubject, String SentFor, Int64 UserInfoId, Int64? OrderID)
    {

        TodayEmailsRepository objRepo = new TodayEmailsRepository();
        TodaysEmailDetail objEmail = new TodaysEmailDetail();
        objEmail.FilePath = new Common().SaveFileAsHtml(Messagebody, EmailSubject);
        objEmail.EmailSubject = EmailSubject;
        objEmail.SentFor = SentFor;
        objEmail.UserInfoID = UserInfoId;
        objEmail.CreatedDate = DateTime.Now;
        objEmail.OrderID = OrderID;
        objRepo.Insert(objEmail);
        objRepo.SubmitChanges();
    }

    public void InsertIntoTodaysEmailDetails(String Messagebody, String EmailSubject, String SentFor, Int64 UserInfoId, Int64? OrderID, String ModuleName, String SubMenu)
    {

        TodayEmailsRepository objRepo = new TodayEmailsRepository();
        TodaysEmailDetail objEmail = new TodaysEmailDetail();
        objEmail.FilePath = new Common().SaveFileAsHtml(Messagebody, EmailSubject);
        objEmail.EmailSubject = EmailSubject;
        objEmail.SentFor = SentFor;
        objEmail.UserInfoID = UserInfoId;
        objEmail.CreatedDate = DateTime.Now;
        objEmail.OrderID = OrderID;
        objEmail.ModuleName = ModuleName;
        objEmail.SubMenu = SubMenu;
        objRepo.Insert(objEmail);
        objRepo.SubmitChanges();
    }

    public bool SendMailWithReplyToANDAttachmentForAsset(Int64 UserInfoID, String sentFor, string psFromAddress, string psToAddress, string psSubject, string psMessageBody, string Fromname
       , bool SSL, bool HTMLBody, String FilePath, string Host, Int32 Port, string uName, string Pwd, String OFileName, bool IsForLive, string psReplyToAddress)
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


            MailAddress FromAddress = default(MailAddress);
            if (!String.IsNullOrEmpty(FilePath))
            {
                String[] AryFilePath = FilePath.Split(',');
                String[] AryFileNames = OFileName.Split(',');
                for (int i = 0; i < AryFilePath.Length; i++)
                {
                    Attachment objAttach = new Attachment(AryFilePath[i]);
                    System.Net.Mime.ContentDisposition obj = objAttach.ContentDisposition;
                    obj.FileName = AryFileNames[i];
                    oMailMessage.Attachments.Add(objAttach);
                }
            }



            if (string.IsNullOrEmpty(Fromname))
            {
                FromAddress = new MailAddress(psFromAddress);
            }
            else
            {
                FromAddress = new MailAddress(psFromAddress, Fromname);
            }


            //oMailMessage.To.Add(Convert.ToString(ConfigurationManager.AppSettings["testSystemInbox"]));
            oMailMessage.To.Add(psToAddress);

            if (!String.IsNullOrEmpty(psReplyToAddress))
            {
                oMailMessage.ReplyTo = new MailAddress(psReplyToAddress);
            }

            oMailMessage.From = FromAddress;
            oMailMessage.Subject = "Test Environment : " + psSubject;
            oMailMessage.IsBodyHtml = HTMLBody;
            oMailMessage.Body = psMessageBody;
            oMailMessage.Priority = MailPriority.High;

            smtpmail.EnableSsl = SSL;

            smtpmail.Send(oMailMessage);

            //Insert Details into TodaysEmailDetails
            if (UserInfoID != 0)
                InsertIntoTodaysEmailDetails(psMessageBody, psSubject, sentFor, UserInfoID, null);

            Result = true;

        }
        catch (Exception ex)
        {
            Result = false;
            throw ex;
        }
        return Result;
    }

    #endregion
}
