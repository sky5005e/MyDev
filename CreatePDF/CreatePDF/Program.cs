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
            
            String pdfpath = GetPDFpath(Convert.ToString(args[0]));
            Console.WriteLine(pdfpath);
            //localTest();
        }
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
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
        public static String GetPDFpath1(String orderNumber)
        {

            String PDFpath = String.Empty;
            InvoiceInfo objInvoiceInfo = new InvoiceInfo();
            String xmlString = String.Empty;
             XmlDocument xdoc = new XmlDocument();
            xdoc.Load(@"C:\Users\Sky\Desktop\file635310649415115404.xml");

            xmlString = xdoc.OuterXml;
            OrderDetails objOrder = FromXmlString(xmlString);
            //OrderDetails objOrder = objInvoiceInfo.GetInvoiceDetails(orderNumber);

            String MainDir = @"C:\Users\Sky\Desktop\ProFinal\FTP\";
            //String orderNumber = "ORD0011279";
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
    }
}
