using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using OfficeOpenXml;

namespace CreatePDF
{
    class Program
    {
        static String MainDir = @"C:\Users\g521753\Desktop\SKY-Drive\NewBills\uber\";
        static String Hotels = @"C:\Users\g521753\Desktop\SKY-Drive\NewBills\";
        static string excelpath = @"C:\Users\g521753\Desktop\SKY-Drive\NewBills\MXBLUX.xlsx";
        static void Main(string[] args)
        {
            
            String pdfpath = GenerateHotelBills();
            Console.WriteLine(pdfpath);
            Console.Read();
            
            /*
            String pdfpath = GetPDFpath("testSky" + Guid.NewGuid().ToString());
            Console.WriteLine(pdfpath);
            Console.Read();
            */
        }

        public static String GetPDFpath(String orderNumber)
        {
            String PDFpath = String.Empty;

            FileInfo existingFile = new FileInfo(excelpath);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                var dt = package.ToDataTable();
                List<OrderDetails> list = dt.DataTableToList<OrderDetails>();
                foreach (var item in list)
                {

                    PDFpath = MainDir + "receipt_" + Guid.NewGuid().ToString() + ".pdf";
                    new InvoiceInfo().GeneratePdfInvoice(PDFpath, item);
                }

            }

            return PDFpath;

        }

        public static string GenerateHotelBills()
        {
            HotelsDetails obj = new HotelsDetails();
            obj.name = "PARK NILO";
            obj.date = "2020-03-21";
            string PDFpath = Hotels + "receipt_" + Guid.NewGuid().ToString() + ".pdf";
            new InvoiceInfo().GenerateHotelsPdfInvoice(PDFpath, obj);
            return PDFpath;
        }
    }
}
