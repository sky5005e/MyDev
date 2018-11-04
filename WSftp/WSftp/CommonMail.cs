using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

/// <summary>
/// Summary description for CommonMail
/// </summary>
public class CommonMail
{
    public CommonMail()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    /// <summary>
    /// Simple Mail
    /// </summary>
    /// <param name="psFromAddress"></param>
    /// <param name="psToAddress"></param>
    /// <param name="psSubject"></param>
    /// <param name="psMessageBody"></param>
    /// <param name="Fromname"></param>
    /// <param name="SSL"></param>
    /// <param name="HTMLBody"></param>
    /// <returns></returns>
    public bool SendMail(string psFromAddress, string psToAddress, string psSubject, string psMessageBody, string Fromname
        , string Host, Int32 Port, bool SSL, bool HTMLBody)
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

            oMailMessage.To.Add(psToAddress);

            oMailMessage.From = FromAddress;
            oMailMessage.Subject = psSubject;
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
    /// This Method is for Send Mail with Attachment
    /// </summary>
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
    public bool SendMailWithAttachment(string psFromAddress, string psToAddress, string psSubject, string psMessageBody, string Fromname
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


            oMailMessage.To.Add(psToAddress);

            oMailMessage.From = FromAddress;
            oMailMessage.Subject = psSubject;
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

}