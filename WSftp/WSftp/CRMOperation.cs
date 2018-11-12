using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;

namespace WSftp
{
    public class CRMOperation
    {
        /// <summary>
        /// 
        /// </summary>
        public void CRMMainOperation()
        {


            String subDir = DateTime.Now.ToString("yyyy-MM-dd");
            String MainDir = @"D:\CRM\CRM\WWWRoot\CustomPages\OrderTracking\CatalystXML\Received\";
            String newDirPath = MainDir + subDir + @"\";
            bool isExists = System.IO.Directory.Exists(newDirPath);
            
            FTPClient ftpclient = new FTPClient();
            string[] DirInfo = ftpclient.ListDirectory();

            // Full Info of Dir
            //string[] DirDetails = ftpclient.ListDirectoryDetails();
            //
            
           
            
            foreach (String fileName in DirInfo)
            {
                if (!isExists)
                    System.IO.Directory.CreateDirectory(newDirPath);

                bool IsDirExist = ftpclient.CheckFTPUrlExist(subDir + "/");
                if (!IsDirExist)
                    ftpclient.MakeDirectory(subDir + "/");

                if (fileName.Contains(".xml"))
                {
                    // File Download  
                    ftpclient.DownloadFile(fileName, newDirPath + fileName);
                    if (fileName.Contains("shipment"))
                    {
                        //String SourceFile = newDirPath + fileName;
                        //String ext = Path.GetExtension(SourceFile); // returns .xml
                        //String newfilename = Path.GetFileNameWithoutExtension(SourceFile); // returns FileName
                        //newfilename = newfilename + "-" + subDir + "-" + DateTime.Now.ToString("HH_mm_ss") + ext;

                        //System.IO.File.Move(SourceFile, newDirPath + newfilename);

                        ParseShipmentXml(newDirPath, fileName, ftpclient, subDir);
                    }
                    else if (fileName.Contains("order-ORD"))
                    {
                        // 
                        ParseOrderXml(newDirPath, fileName, ftpclient, subDir);
                    }
                    else
                    {
                        ftpclient.UploadFile(newDirPath + fileName, subDir + "/" + fileName);
                        SendEmailToMe(fileName);
                    }

                    // Last delete file
                    ftpclient.DeleteFile(fileName);
                }

            }
             
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="fileName"></param>
        /// <param name="ftpClient"></param>
        /// <param name="CheckValidurl"></param>
        /// <param name="todayDatePath"></param>
        public void ParseOrderXml(String xmlPath, String fileName, FTPClient ftpClient,String todayDatePath)
        {
            String sourceFilePath = xmlPath + fileName;
            String msg = String.Empty;
            String status = String.Empty;
            String orderNumber = String.Empty;
            String ccm_order_id = String.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            if (!String.IsNullOrEmpty(xmlPath))
                xmlDoc.Load(sourceFilePath);
            XmlNodeList xnList = xmlDoc.SelectNodes("/order");
            if (xnList.Count > 0)
            {
                //status = (xmlDoc.SelectSingleNode("/order/status")).InnerText;
                orderNumber = xnList[0].Attributes["customer_po"].Value;
                foreach (XmlNode xn in xnList)
                {
                    if (xn["status"].InnerText == "success")
                        status = "Confirmed";
                    else
                        status = xn["status"].InnerText;

                    ccm_order_id = xn["ccm_order_id"].InnerText;
                    msg = xn["message"].InnerText;
                }
            }

            // UpDate Query
            DataAccess objDataAccess = new DataAccess();
            String selectQry = "SELECT Count(ordt_ordernumber) FROM CRM.dbo.ordertracking  WHERE ordt_ordernumber = '" + orderNumber + "' GROUP BY ordt_ordernumber";
            if (objDataAccess.IsOrderNumberExist(selectQry))
            {
                String updateQry = " UPDATE CRM.dbo.ordertracking SET ordt_order_status ='" + status + "', ordt_message ='" + msg + "' WHERE ordt_ordernumber = '" + orderNumber + "' ";
                objDataAccess.GetTableDisconnect_Connection(updateQry);
            }
            
            ftpClient.UploadFile(sourceFilePath, todayDatePath + "/" + fileName);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="fileName"></param>
        /// <param name="ftpClient"></param>
        /// <param name="todayDatePath"></param>
        public void ParseShipmentXml(String xmlPath, String fileName, FTPClient ftpClient,  String todayDatePath)
        {
            String sourceFilePath = xmlPath + fileName;
            String shipmentDate = String.Empty;
            String shipmentComplete = String.Empty;
            String carrierCode = String.Empty;
            String waybill = String.Empty;
            String orderNumber = String.Empty;
            String ccm_order_id = String.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            if (!String.IsNullOrEmpty(xmlPath))
                xmlDoc.Load(sourceFilePath);
            XmlNodeList xnList = xmlDoc.SelectNodes("/shipments/shipment");
            Int32 count = xnList.Count;

            for (int i = 0; i < count; i++)
            {

                shipmentDate = xnList[i].SelectSingleNode("shipment_date").InnerText;
                shipmentComplete = xnList[i].SelectSingleNode("shipment_complete").InnerText;
                carrierCode = xnList[i].SelectSingleNode("carrier_code").InnerText;
                waybill = xnList[i].SelectSingleNode("waybills/waybill").InnerText;
                orderNumber = xnList[i].SelectSingleNode("orders/order").Attributes["customer_order_id"].Value;
                ccm_order_id = xnList[i].SelectSingleNode("orders/order").Attributes["ccm_order_id"].Value;

                String trakingURL = "http://www.purolator.com/purolator/ship-track/tracking-details.page?pin=" + waybill;
          
                // UpDate Query
                DataAccess objDataAccess = new DataAccess();
                String selectQry = "SELECT Count(ordt_ordernumber) FROM CRM.dbo.ordertracking  WHERE ordt_ordernumber = '" + orderNumber + "' GROUP BY ordt_ordernumber";
                if (objDataAccess.IsOrderNumberExist(selectQry))
                {
                    String updateQry = " UPDATE CRM.dbo.ordertracking SET ordt_order_tracking_number = '" + waybill + "', ordt_carrier_name ='" + carrierCode + "',ordt_carrier_code ='" + carrierCode + "', ordt_order_status ='Shipped', ordt_shipped_date_text='" + shipmentDate + "', ordt_warehouse_order_id ='" + ccm_order_id + "',ordt_tracking_URL ='" + trakingURL + "'  WHERE ordt_ordernumber = '" + orderNumber + "' ";
                    objDataAccess.GetTableDisconnect_Connection(updateQry);
                }
            }
            //
            //String ext = Path.GetExtension(sourceFilePath); // returns .xml
            //String newfilename = Path.GetFileNameWithoutExtension(sourceFilePath); // returns File
            //newfilename = newfilename + "-" + todayDatePath + "-" + DateTime.Now.ToString("HH_mm_ss") + ext;
            ftpClient.UploadFile(sourceFilePath, todayDatePath + "/" + fileName);

        }

        private void SendEmailToMe(String filename)
        {

            String fileName = filename;
            String FilePath = String.Empty;
            try
            {
                //if (flFiles.HasFile)
                //{
                //  fileName = flFiles.FileName;
                FilePath = null;
                // flFiles.SaveAs(FilePath);

                //}
                String emailBody = "Hi, <br> A New file have been found" + filename + " <br> <br>Thanks <br> Adeeva";
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
              
                //}
            }
            catch (Exception ex)
            {

                LogHelper.LogError(ex);
                throw ex;
            }

        }

        public static Boolean IsWeekend()
        {
            Boolean _isWeekend = false;
            DateTime date = DateTime.Now;//DateTime.ParseExact("2014-03-30 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff",System.Globalization.CultureInfo.InvariantCulture);
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                _isWeekend = true;
            }
            return _isWeekend;
        }

        public static Boolean IsWorkingHours()
        {
            Boolean _isWorkingHours = false;
            DateTime date = DateTime.Now;
            if (date.Hour > 8 && date.Hour < 21)
            {
                _isWorkingHours = true;
            }
            return _isWorkingHours;
        }
    }
}
