using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace WSftp
{
    public partial class Service1 : ServiceBase
    {
        String errorMSG = String.Empty;
        public Service1()
        {

            InitializeComponent();

            if (!System.Diagnostics.EventLog.SourceExists("FTPScheduler"))
            {

                System.Diagnostics.EventLog.CreateEventSource("FTPScheduler", "AutoFTPLog");

            }

            eventLog1.Source = "FTPScheduler";

            eventLog1.Log = "AutoFTPLog";


        }
        public String MessageBody()
        {
            String msg = @"<p>Hi,<br>

            <br>
            Scheduler has been sucessfully processed on FTP.<br>
            <br>
            From,<br>
            Adeeva<br>
            </p>";

            return msg;
        }
        private void SendEmailToMe(Boolean IsError)
        {

            String fileName = String.Empty;
            String FilePath = String.Empty;
            try
            {
                //if (flFiles.HasFile)
                //{
                //  fileName = flFiles.FileName;
                FilePath = null;
                // flFiles.SaveAs(FilePath);

                //}
                String emailBody = String.Empty;
                if (!IsError)
                    emailBody = MessageBody();
                else
                    emailBody = "An Error occurred while processing your request on FTP<br/>" + errorMSG;
                // Create a new Smpt Client using Google's servers
                // var mailclient = new SmtpClient();
                String smtpHost = "mail.adeeva.com";//ForGmail
                Int32 smtpPort = 587; //ForGmail

                Boolean EnableSsl = false;//true;//ForGmail
                // Send Email
                //foreach (var item in Emaillist())
                //{
                String item = "sunilpanchani@gmail.com";
                Boolean isMailSent = new CommonMail().SendMailWithAttachment("admin@adeeva.com", item.Trim().ToString(), "FTP Auto alert-" + DateTime.Now.ToShortDateString(), emailBody, "Admin Team", EnableSsl, true, FilePath, smtpHost, smtpPort, "System", null, fileName, false);
                if (isMailSent)
                    eventLog1.WriteEntry("Email is sent successfully");
                else
                    eventLog1.WriteEntry("An errored occur while sending email");
                //}
            }
            catch (Exception ex)
            {

                LogHelper.LogError(ex);
                eventLog1.WriteEntry("An error occured while sending email" + " " + ex.ToString());

            }

        }
        public void time_elapsed(object sender, ElapsedEventArgs e)
        {

            eventLog1.WriteEntry("Mail Sending on " + DateTime.Now.ToString());
            try
            {
                if (!CRMOperation.IsWeekend() && CRMOperation.IsWorkingHours())
                {
                    new CRMOperation().CRMMainOperation();
                    eventLog1.WriteEntry("Successfully Scheduler Run!");
                    //SendEmailToMe(false);
                }
               
            }
            catch(Exception ex)
            {
                eventLog1.WriteEntry("An error occured while processing scheduler event" + " " + ex.ToString());
                LogHelper.LogError(ex);
                SendEmailToMe(true);
            }
           

        }

        protected override void OnStart(string[] args)
        {


            eventLog1.WriteEntry("In OnStart --- Sending Mail to" + DateTime.Now.ToString());

            System.Timers.Timer time = new System.Timers.Timer();

            time.Start();

            time.Interval = 300000;

            time.Elapsed += time_elapsed;
        }

        protected override void OnStop()
        {
        }
    }
}
