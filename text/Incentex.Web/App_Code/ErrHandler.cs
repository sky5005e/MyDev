using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Incentex.BE;
using Incentex.DA;

public class ErrHandler
{
    public ErrHandler()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// WriteError()
    /// Create file in Error folder when error comes in applcation page.
    /// </summary>
    public static void WriteError(Exception ex)
    {
        WriteError(ex, true);
        AddErrorIntoDB(ex);
    }

    private static void AddErrorIntoDB(Exception ex)
    {
        try
        {
            System.Web.HttpBrowserCapabilities browser = System.Web.HttpContext.Current.Request.Browser;
            string strPageName = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;

            Incentex.DAL.SqlRepository.PageErrorRepository objRepository = new Incentex.DAL.SqlRepository.PageErrorRepository();

            objRepository.AddPageErrorLog(Common.UserID, strPageName, browser.Browser, browser.Platform, (browser.ScreenCharactersWidth.ToString() + "x" + browser.ScreenPixelsHeight.ToString()), DateTime.Now, ex.Message, string.Empty);
        }
        catch (Exception EX)
        {
            WriteError(EX);
        }
    }

    /// <summary>
    /// WriteError()
    /// Create file in Error folder when error comes in applcation page.
    /// </summary>
    public static void WriteError(Exception ex, Boolean NotFromAPI)
    {
        try
        {
            String path = "~/Error/" + DateTime.Today.ToString("dd-MMM-yy") + ".txt";

            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();

            StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path));

            try
            {
                if (NotFromAPI)
                    PageBase.SetIsErrorOccur(true);

                StackTrace trace = new StackTrace(ex, true);

                StringBuilder err = new StringBuilder();
                err.Append("Log Entry : " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + Environment.NewLine);
                err.Append("Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() + Environment.NewLine);
                err.Append("Inner Exception : " + (ex.InnerException != null ? ex.InnerException.Message : "") + Environment.NewLine);
                err.Append("UserInfoID : " + (IncentexGlobal.CurrentMember != null ? Convert.ToString(IncentexGlobal.CurrentMember.UserInfoID) : "") + Environment.NewLine);
                err.Append("File Name : " + trace.GetFrame(0).GetFileName() + Environment.NewLine);
                err.Append("Line : " + trace.GetFrame(0).GetFileLineNumber() + Environment.NewLine);
                err.Append("Column: " + trace.GetFrame(0).GetFileColumnNumber() + Environment.NewLine);
                err.Append("Error Message: " + ex.Message + Environment.NewLine);
                err.Append("Source: " + ex.Source + Environment.NewLine);
                err.Append("StackTrace: " + ex.StackTrace + Environment.NewLine);
                err.Append("TargetSite:" + Convert.ToString(ex.TargetSite));

                w.WriteLine("\r\n" + err);

                w.WriteLine("__________________________");
                w.Flush();
                w.Close();

                EmailTemplateBE objEmailBE = new EmailTemplateBE();
                EmailTemplateDA objEmailDA = new EmailTemplateDA();
                DataSet dsEmailTemplate;

                objEmailBE.SOperation = "SELECTERROR";
                objEmailBE.STemplateName = "error";
                dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

                if (dsEmailTemplate != null)
                {
                    String sFrmadd = Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"]);
                    String sToadd = "devraj.gadhavi@indianic.com";
                    String sSubject = Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sSubject"]);
                    String sFrmname = Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sFromName"]);

                    StringBuilder messagebody = new StringBuilder(Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"])); // From Template table
                    messagebody.Replace("{siteurl}", Convert.ToString(ConfigurationSettings.AppSettings["siteurl"]));
                    messagebody.Replace("{error}", err.Replace(Environment.NewLine, "</br>").ToString());

                    String smtphost = Convert.ToString(ConfigurationSettings.AppSettings["SMTPHOST"]);
                    Int32 smtpport = Convert.ToInt32(ConfigurationSettings.AppSettings["SMTPPORT"]);
                    String smtpUserID = Convert.ToString(ConfigurationSettings.AppSettings["SMTPUSERID"]);
                    String smtppassword = Convert.ToString(ConfigurationSettings.AppSettings["SMTPPASSWORD"]);

                    messagebody.Replace(Environment.NewLine, "</br>");

                    new CommonMails().SendMail(0, "System Error", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
                }
            }
            catch (Exception EX)
            {
                WriteError(EX);
            }
            finally
            {
                w.Close();
            }
        }
        catch
        {
        }
    }
}