using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Timers;

namespace WSOrderTraciking
{
    public partial class Service1 : ServiceBase
    {
        
        String errorMSG = String.Empty;
        //public  String subDir = DateTime.Now.ToString("yyyy-MM-dd");

        /// <summary>
        /// Get Dir path
        /// </summary>
        public String MainDir()
        {

            String newDirPath = CommonCls.rootDir + DateTime.Now.ToString("yyyy-MM-dd") + @"\";
            bool isExists = System.IO.Directory.Exists(newDirPath);

            if (!isExists)
                System.IO.Directory.CreateDirectory(newDirPath);

            return newDirPath;

        }
        public Service1()
        {

            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("FTPCrmAutoProcessSrc"))
            {

                System.Diagnostics.EventLog.CreateEventSource("FTPCrmAutoProcessSrc", "AutoFTPCrmXmlLog");

            }

            eventLog1.Source = "FTPCrmAutoProcessSrc";

            eventLog1.Log = "AutoFTPCrmXmlLog";


        }
        public String MessageBody()
        {
            String msg = @"<p>Hi,<br>

            <br>
            Scheduler2 has been sucessfully processed on FTP.<br>
            <br>
            From,<br>
            Adeeva<br>
            </p>";

            return msg;
        }
        private void SendEmailToMe(Boolean IsError)
        {

            try
            {

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

                String toAddress = "sunilpanchani@gmail.com";
                Boolean isMailSent = new CommonMail().SendMail("admin@adeeva.com", toAddress.Trim().ToString(), "FTP Auto alert-" + DateTime.Now.ToShortDateString(), emailBody, "Admin Team", smtpHost, smtpPort, EnableSsl, true);

                if (isMailSent)
                    eventLog1.WriteEntry("Email is sent successfully");
                else
                    eventLog1.WriteEntry("An errored occur while sending email : " + errorMSG);
            }
            catch (Exception ex)
            {

                eventLog1.WriteEntry("An error occured while sending email" + " " + ex.ToString());
                LogHelper.LogError(ex);
            }

        }
        private void SendEmailToOrder(String ordernum)
        {

            try
            {

                String emailBody = "Hi,<br>An Order Number : " + ordernum + " has some error.<br/>Please do needful.<br> Thank You<br>Admin";

                // Create a new Smpt Client using Google's servers
                // var mailclient = new SmtpClient();
                String smtpHost = "mail.adeeva.com";//ForGmail
                Int32 smtpPort = 587; //ForGmail

                Boolean EnableSsl = false;//true;//ForGmail
                // Send Email
                string[] arrEmail = { "orders@adeeva.com" };//"sunil@adeeva.com", "rajeev@adeeva.com", "tonda@adeeva.com", "jackie@adeeva.com" };
                foreach (String item in arrEmail)
                {
                    String toAddress = item;
                    Boolean isMailSent = new CommonMail().SendMail("admin@adeeva.com", toAddress.Trim().ToString(), "FTP Auto alert-" + DateTime.Now.ToShortDateString(), emailBody, "Admin Team",  smtpHost, smtpPort, EnableSsl, true);

                    if (isMailSent)
                        eventLog1.WriteEntry("Email is sent successfully");
                    else
                        eventLog1.WriteEntry("An errored occur while sending email");
                }
            }
            catch (Exception ex)
            {

                eventLog1.WriteEntry("An error occured while sending email" + " " + ex.ToString());
                LogHelper.LogError(ex);
            }

        }

        private void SendEmailUSAWareHouse(String orderNumber, String fobDesc, String Comments, DirFiles Files)
        {
            try
            {


                // var mailclient = new SmtpClient();
                String smtpHost = "mail.adeeva.com";//ForGmail
                Int32 smtpPort = 587; //ForGmail
                String msgBody = "<br>Special Delivery Instruction : " + fobDesc + "<br> Order Comments: " + Comments;
                Boolean EnableSsl = false;//true;//ForGmail
                // subject Order for dispatch #ORD0011265;
                String subject = "Order for dispatch #"+orderNumber;
                String toAddress = "aocchiuto@pdmny.com";
                Boolean isMailSent = new CommonMail().SendMailWithMultiAttachment("orders@adeeva.com", toAddress.Trim().ToString(), subject, msgBody, "Adeeva Nutritionals", EnableSsl, true, Files, smtpHost, smtpPort, "System", null, true);
               
                if (isMailSent)
                    eventLog1.WriteEntry("Email is sent successfully");
                else
                    eventLog1.WriteEntry("An errored occur while sending email");

            }
            catch (Exception ex)
            {

                eventLog1.WriteEntry("An error occured while sending email" + " " + ex.ToString());
                LogHelper.LogError(ex);
            }

        }
        public void ProcessMainOps()
        {
           
            try
            {

                DataAccess objDataAccess = new DataAccess();
                String selectQry = "SELECT ordt_ItemInvUniQ,ordt_ordernumber,ordt_order_country FROM CRM.dbo.ordertracking WHERE ordt_order_status = 'Pending' AND ordt_Deleted IS NULL";
                DataSet ds = objDataAccess.GetTableDisconnect_Connection(selectQry);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Boolean OrderHasError = false;
                    InvoiceInfo objInvoiceInfo = new InvoiceInfo();
                    String orderNumber = Convert.ToString(ds.Tables[0].Rows[i]["ordt_ordernumber"]).Trim();
                    String orderCountry = Convert.ToString(ds.Tables[0].Rows[i]["ordt_order_country"]).Trim();
                    String ItemInvUniQ = Convert.ToString(ds.Tables[0].Rows[i]["ordt_ItemInvUniQ"]).Trim();

                    OrderDetails objOrder = objInvoiceInfo.GetInvoiceDetails(orderNumber, ItemInvUniQ);
                    // Write XML files
                    if (objOrder != null)
                    {
                        String _mainDir = MainDir();
                        String cusmoterXMLpath = _mainDir + "customer-" + orderNumber + ".xml";
                        String orderXMLpath = _mainDir + "order-" + orderNumber + ".xml";
                        String shiptoXMLpath = _mainDir + "shipto-" + orderNumber + ".xml";
                        String invoicePDFpath = _mainDir + "invoice-" + orderNumber + ".pdf";

                        FTPClient ftpclient = new FTPClient();

                        DirFiles objDirFiles = new DirFiles();
                        objDirFiles.OrderNumber = orderNumber;
                        objDirFiles.CustomerXMLPath = cusmoterXMLpath;
                        objDirFiles.OrderXMLPath = orderXMLpath;
                        objDirFiles.ShipToXMLPath = shiptoXMLpath;
                        objDirFiles.InvoicePDFPath = invoicePDFpath;

                        if (orderCountry == "US") // create only pdf invoice
                        {
                            // Invoice PDF
                            objInvoiceInfo.GeneratePdfInvoice(invoicePDFpath, objOrder);
                            // send email to USA ware house
                            SendEmailUSAWareHouse(orderNumber, objOrder.INVDesc + " " + objOrder.INVDeliveryIns, objOrder.INVOrderComent, objDirFiles);
                        }
                        else if (orderCountry == "CA")
                        {
                            // Customer xml
                            File.WriteAllText(cusmoterXMLpath, objInvoiceInfo.GetCustomerXMLstring(objOrder));
                            // upload to FTP customer file
                            ftpclient.UploadFile(cusmoterXMLpath, Path.GetFileName(cusmoterXMLpath));

                            // Order XML
                            File.WriteAllText(orderXMLpath, objInvoiceInfo.GetOrderXMLstring(objOrder));
                            // upload to FTP Order file
                            ftpclient.UploadFile(orderXMLpath, Path.GetFileName(orderXMLpath));

                            // ShipTo XML
                            File.WriteAllText(shiptoXMLpath, objInvoiceInfo.GetShipToXMLstring(objOrder));
                            // upload to FTP ShipTo file
                            ftpclient.UploadFile(shiptoXMLpath, Path.GetFileName(shiptoXMLpath));

                            // Invoice PDF
                            objInvoiceInfo.GeneratePdfInvoice(invoicePDFpath, objOrder);
                            // upload to FTP Invoice PDF
                            ftpclient.UploadFile(invoicePDFpath, Path.GetFileName(invoicePDFpath));
                        }
                        else
                        {

                            SendEmailToOrder(orderNumber);
                            OrderHasError = true;

                        }

                        // Finally Update the OrderTracking Table
                        if (!OrderHasError)
                        {
                            String IscutoffTime = CutoffTime();
                            String updateQry = " UPDATE CRM.dbo.ordertracking SET ordt_order_status ='Processed', ordt_cutoff_time='" + IscutoffTime + "' WHERE ordt_ordernumber = '" + orderNumber + "' ";
                            objDataAccess.GetTableDisconnect_Connection(updateQry);
                        }
                        else
                        {
                            String updateQry = " UPDATE CRM.dbo.ordertracking SET ordt_order_status ='Error',ordt_message ='Invalid Country(Other than US or CA)' WHERE ordt_ordernumber = '" + orderNumber + "' ";
                            objDataAccess.GetTableDisconnect_Connection(updateQry);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }


        }

        private String CutoffTime()
        {
            String cutTime = String.Empty;
            if (DateTime.Now.TimeOfDay >= System.TimeSpan.Parse("14:00:00"))
                cutTime = "Yes";
            else
                cutTime = "No";

            return cutTime;
        }
        public void timer_elapsed(object sender, ElapsedEventArgs e)
        {
               
            eventLog1.WriteEntry("Mail Sending on test " + DateTime.Now.ToString());
            try
            {
                if (!CommonCls.IsWeekend() && CommonCls.IsWorkingHours())
                {
                    ProcessMainOps();
                    eventLog1.WriteEntry("Successfully Scheduler Run!");
                    //SendEmailToMe(false);
                }
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("An error occured while processing scheduler event" + " " + ex.ToString());
                errorMSG = ex.Message;
                SendEmailToMe(true);
            }


        }

        protected override void OnStart(string[] args)
        {
            
            try
            {
                // Put your OnStart code here

                eventLog1.WriteEntry("In OnStart --- Sending Mail to test" + DateTime.Now.ToString());

                Timer timer = new System.Timers.Timer();

                timer.Start();

                timer.Interval = 300000;

                timer.Elapsed += timer_elapsed;
                

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
           
        }
    }
}
