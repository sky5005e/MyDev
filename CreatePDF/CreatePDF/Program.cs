using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace CreatePDF
{
    class Program
    {
        static String MainDir = @"D:\CRM\CRM\WWWRoot\CustomPages\OrderTracking\CatalystXML\pdfSetup\";
        static void Main(string[] args)
        {
            DeleteOldInvoiceFile();
            if (args[0] != null)
            {
                String pdfpath = GetPDFpath(Convert.ToString(args[0]));
                Console.WriteLine(pdfpath);
            }
        }

        public static String GetPDFpath(String orderNumber)
        {

            String PDFpath = String.Empty;
            InvoiceInfo objInvoiceInfo = new InvoiceInfo();

            OrderDetails objOrder = objInvoiceInfo.GetInvoiceDetails(orderNumber);


            // Write XML files
            if (objOrder != null)
            {
                PDFpath = MainDir + "invoice-" + orderNumber + ".pdf";
                new InvoiceInfo().GeneratePdfInvoice(PDFpath, objOrder);

            }

            return PDFpath;

        }

        public static OrderDetails FromXmlString(string xmlString)
        {
            var reader = new StringReader(xmlString);
            var serializer = new XmlSerializer(typeof(OrderDetails));
            var instance = (OrderDetails)serializer.Deserialize(reader);

            return instance;
        }

        public static void DeleteOldInvoiceFile()
        {
            DirectoryInfo di = new DirectoryInfo(MainDir);
            FileInfo[] files = di.GetFiles("invoice-*").Where(p => p.Extension == ".pdf").ToArray();
            foreach (var item in files)
            {
                LogHelper.Log($"File names : {item.Name}");
                if (item.CreationTime < DateTime.Now.AddHours(-1))
                    item.Delete();
            }
        }
    }
}
