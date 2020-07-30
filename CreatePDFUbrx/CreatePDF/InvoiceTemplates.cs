using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;


namespace CreatePDF
{
    public class InvoiceTemplates
    {
        public InvoiceTemplates()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        /// <summary>
        /// Set Cell Properties
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="colspan"></param>
        /// <param name="align"></param>
        /// <param name="fontSize"></param>
        /// <param name="FontStyle"></param>
        /// <param name="border"></param>
        /// <param name="borderWidth"></param>
        /// <returns></returns>
        private PdfPCell SetCellProperties(String Text, Int32 colspan, Int32 align, float fontSize, Int32 FontStyle, Int32 border, float borderWidth)
        {
            PdfPCell cell = SetCellProperties(Text, colspan, align, fontSize, FontStyle);
            //cell.BorderColor = new BaseColor(System.Drawing.Color.Red);
            cell.Border = border; //iTextSharp.text.Rectangle.NO_BORDER; // | Rectangle.TOP_BORDER;
            cell.BorderWidthBottom = borderWidth;
            return cell;
        }

        private PdfPCell SetCellProperties(String Text, Int32 colspan, Int32 align, float fontSize, Int32 FontStyle, Int32 border, float borderWidth, System.Drawing.Color fontColor, System.Drawing.Color borderColor)
        {
            PdfPCell cell = SetCellProperties(Text, colspan, align, fontSize, FontStyle, fontColor);
            cell.BorderColor = new BaseColor(borderColor);
            cell.Border = border; //iTextSharp.text.Rectangle.NO_BORDER; // | Rectangle.TOP_BORDER;
            cell.BorderWidthBottom = borderWidth;
            return cell;
        }

        private PdfPCell SetCellProperties(String Text, Int32 colspan, Int32 rowspan, Int32 align, float fontSize, Int32 FontStyle)
        {
            PdfPCell cell = SetCellProperties(Text, colspan, align, fontSize, FontStyle);
            cell.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
            cell.Rowspan = rowspan;
            return cell;
        }

        private PdfPCell SetCellProperties(String Text, Int32 colspan, Int32 align, float fontSize, Int32 FontStyle, System.Drawing.Color color) 
        {
            var baseColor = new BaseColor(color); 

            Font fontArial = new Font(FontFactory.GetFont("Arial", fontSize, FontStyle, baseColor));


            //Font Style 0= Normal; 1=Bold
            //Align 0=Left, 1=Centre, 2=Right
            PdfPCell cell = new PdfPCell(new Phrase(Text, fontArial));
            //cell.Colspan = 3;
            cell.HorizontalAlignment = align;
            ////Style
            cell.Colspan = colspan;
            return cell;
        }

        private PdfPCell SetCellProperties(String Text, Int32 colspan, Int32 align, float fontSize, Int32 FontStyle)
        {

            Font fontArial = new Font(FontFactory.GetFont("Arial", fontSize, FontStyle));

            //Font Style 0= Normal; 1=Bold
            //Align 0=Left, 1=Centre, 2=Right
            PdfPCell cell = new PdfPCell(new Phrase(Text, fontArial));
            //cell.Colspan = 3;
            cell.HorizontalAlignment = align;
            ////Style
            cell.Colspan = colspan;
            return cell;
        }

        /// <summary>
        /// Set Color to the cell
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="colspan"></param>
        /// <param name="align"></param>
        /// <param name="fontSize"></param>
        /// <param name="FontStyle"></param>
        /// <param name="border"></param>
        /// <param name="borderWidth"></param>
        /// <returns></returns>
        private PdfPCell SetCellPropertiesWithColor(String Text, Int32 colspan, Int32 align, float fontSize, Int32 FontStyle, Int32 border, float borderWidth)
        {

            Font fontArial = new Font(FontFactory.GetFont("Arial", fontSize, FontStyle, new BaseColor(System.Drawing.Color.OrangeRed)));
            //Font Style 0= Normal; 1=Bold
            //Align 0=Left, 1=Centre, 2=Right
            PdfPCell cell = new PdfPCell(new Phrase(Text, fontArial));
            //cell.Colspan = 3;
            cell.HorizontalAlignment = align;
            ////Style
            cell.Colspan = colspan;
            //cell.BorderColor = new BaseColor(System.Drawing.Color.Red);
            cell.Border = border; //iTextSharp.text.Rectangle.NO_BORDER; // | Rectangle.TOP_BORDER;
            cell.BorderWidthBottom = borderWidth;
            return cell;
        }

        public void GenerateUberInvoicePDF(String filePath, OrderDetails objOrder)
        {
            FontFactory.RegisterDirectories();


            var pageSize = new iTextSharp.text.Rectangle(640f, 800F);
            Document doc = new Document(PageSize.A4, 65f, 65f, 40f, 40f);
            //byte[] pdfBytes;

            MemoryStream memStream = new MemoryStream();
            //PdfWriter writer = PdfWriter.GetInstance(doc, memStream);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            // calling PageEventHelper class to Include in document
            // PageEventHelper pageEventHelper = new PageEventHelper();
            //  writer.PageEvent = pageEventHelper;
            doc.Open();
            try
            {
                // 1 f = 1.71Pixel
                #region Header Section
                float[] widths = new float[] { 50, 50 };
                PdfPTable tabheader = new PdfPTable(widths);
                tabheader.WidthPercentage = 100.0f;
                tabheader.DefaultCell.Border = 0;// No Border
                                                 /*
                                                  //tabheader.
                                              iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(@"C:\Users\Sky\Desktop\All_Pics\SkyIncLogo.png");
                                              //Resize image depend upon your need
                                              jpg.ScaleToFit(40f, 40f);
                                              // Give space before image
                                              jpg.SpacingBefore = 9f;
                                              //Give some space after the image
                                              jpg.SpacingAfter = 1f;
                                              jpg.Alignment = Element.ALIGN_LEFT;
                                              */
                                                 // tabheader.AddCell(jpg);
                tabheader.AddCell(SetCellProperties("Uber", 1, 0, 20f, 1, 0, 0));
                tabheader.AddCell(SetCellProperties(objOrder.date, 1, 2, 9f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0.5f, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                //  tabheader.AddCell(jpg);
                tabheader.AddCell(SetCellProperties("Here's your receipt for your trip", 2, 0, 14f, 0, 0, 0));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("We hope you enjoyed your ride this " + objOrder.daytime, 2, 0, 9f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("Total", 1, 0, 10f, 0, 0, 0));
                tabheader.AddCell(SetCellProperties("MX$" + objOrder.totalAmount +".00", 1, 2, 10f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 5f, 0, 0, 0.5f));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("Trip Fare", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("MX$" + objOrder.TripFare +".00", 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                //tabheader.AddCell(SetCellProperties("Tarifa base", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("$" + objOrder.terifa, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                //tabheader.AddCell(SetCellProperties("Tiempo ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("$" + objOrder.tiempo, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                //tabheader.AddCell(SetCellProperties("Distancia ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("$" + objOrder.distancia, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 5f, 0, 0, 0.5f, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("Subtotal ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Black, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("MX$" + objOrder.subtotal + ".00", 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Black, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("Booking Fee", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("MX$" + objOrder.BookingFee + ".00", 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                //tabheader.AddCell(SetCellProperties("Cuota de solicitud ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("$" + objOrder.Cuotadesolicitud, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                //tabheader.AddCell(SetCellProperties("Redondeo hacia abajo ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("-$" + objOrder.ReAdjusto, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Green, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 5f, 0, 0, 0.5f, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("Amount Charged", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 1, 2, 10f, 0, 0, 0));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("Paid in cash ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("MX$" + objOrder.totalAmount + ".00", 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Black, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("Visit the trip page for more information, including invoices (where available)", 2, 0, 9f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 5f, 0, 0, 0.5f, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("You rode with " + objOrder.DriverFirstName, 2, 0, 12f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));
                tabheader.AddCell(SetCellProperties("Issued on behalf of " + objOrder.DriverFirstName + " " + objOrder.DriverLastName, 2, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));
                tabheader.AddCell(SetCellProperties("UberX  " + objOrder.distanceKM + " kilometres | " + objOrder.time + " min(s)", 2, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties(objOrder.timefrom + " | " + objOrder.FromAddress + " ", 2, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 8f, 1, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(objOrder.timeTo + " | " + objOrder.ToAddress + " ", 2, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));
                // add to doc
                doc.Add(tabheader);


                #endregion
                #region Footer Padding

                // add footer
                PdfPTable tabFooterSectoin = new PdfPTable(1);
                tabFooterSectoin.WidthPercentage = 100.0f;
                tabFooterSectoin.DefaultCell.Border = Rectangle.NO_BORDER;//Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                tabFooterSectoin.AddCell(SetCellProperties("Fare does not include fees that may be charged by your bank. Please contact your bank directly for inquiries.", 0, 0, 9f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                doc.Add(tabFooterSectoin);
                #endregion

                //pdfBytes = memStream.ToArray();
            }

            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                throw ex;
            }

            finally
            {
                writer.CloseStream = true;
                doc.Close();

            }

            //return pdfBytes;
        }
        
        public void GenerateUberInvoicePDFBogota(String filePath, OrderDetails objOrder)
        {
            FontFactory.RegisterDirectories();


            var pageSize = new iTextSharp.text.Rectangle(640f, 800F);
            Document doc = new Document(PageSize.A4, 65f, 65f, 40f, 40f);
            //byte[] pdfBytes;

            MemoryStream memStream = new MemoryStream();
            //PdfWriter writer = PdfWriter.GetInstance(doc, memStream);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            // calling PageEventHelper class to Include in document
            // PageEventHelper pageEventHelper = new PageEventHelper();
            //  writer.PageEvent = pageEventHelper;
            doc.Open();
            try
            {
                // 1 f = 1.71Pixel
                #region Header Section
                float[] widths = new float[] { 50, 50 };
                PdfPTable tabheader = new PdfPTable(widths);
                tabheader.WidthPercentage = 100.0f;
                tabheader.DefaultCell.Border = 0;// No Border
                                                 /*
                                                  //tabheader.
                                              iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(@"C:\Users\Sky\Desktop\All_Pics\SkyIncLogo.png");
                                              //Resize image depend upon your need
                                              jpg.ScaleToFit(40f, 40f);
                                              // Give space before image
                                              jpg.SpacingBefore = 9f;
                                              //Give some space after the image
                                              jpg.SpacingAfter = 1f;
                                              jpg.Alignment = Element.ALIGN_LEFT;
                                              */
                                                 // tabheader.AddCell(jpg);
                tabheader.AddCell(SetCellProperties("Uber", 1, 0, 20f, 1, 0, 0));
                tabheader.AddCell(SetCellProperties(objOrder.date, 1, 2, 9f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0.5f, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                //  tabheader.AddCell(jpg);
                tabheader.AddCell(SetCellProperties("Here's your receipt for your trip", 2, 0, 14f, 0, 0, 0));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("We hope you enjoyed your ride this " + objOrder.daytime, 2, 0, 9f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("Total", 1, 0, 10f, 0, 0, 0));
                tabheader.AddCell(SetCellProperties("$" + objOrder.totalAmount, 1, 2, 10f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 5f, 0, 0, 0.5f));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("Trip Fare", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("$" + objOrder.TripFare, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                //tabheader.AddCell(SetCellProperties("Tarifa base", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("$" + objOrder.terifa, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                //tabheader.AddCell(SetCellProperties("Tiempo ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("$" + objOrder.tiempo, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                //tabheader.AddCell(SetCellProperties("Distancia ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("$" + objOrder.distancia, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 5f, 0, 0, 0.5f, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("Subtotal ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Black, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("$" + objOrder.subtotal, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Black, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("Booking Fee", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("$" + objOrder.BookingFee, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                //tabheader.AddCell(SetCellProperties("Cuota de solicitud ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("$" + objOrder.Cuotadesolicitud, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                //tabheader.AddCell(SetCellProperties("Redondeo hacia abajo ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                //tabheader.AddCell(SetCellProperties("-$" + objOrder.ReAdjusto, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Green, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 5f, 0, 0, 0.5f, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("Amount Charged", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 1, 2, 10f, 0, 0, 0));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("Paid in cash ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("$" + objOrder.totalAmount, 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Black, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("Visit the trip page for more information, including invoices (where available)", 2, 0, 9f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 5f, 0, 0, 0.5f, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("You rode with " + objOrder.DriverFirstName, 2, 0, 12f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));
                tabheader.AddCell(SetCellProperties("Issued on behalf of " + objOrder.DriverFirstName + " " + objOrder.DriverLastName, 2, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));
                tabheader.AddCell(SetCellProperties("UberX  " + objOrder.distanceKM + " kilometres | " + objOrder.time + " min(s)", 2, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties(objOrder.timefrom + " | " + objOrder.FromAddress + ", Bogotá, Colombia", 2, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 8f, 1, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(objOrder.timeTo + " | " + objOrder.ToAddress + ", Bogotá, Colombia", 2, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));
                // add to doc
                doc.Add(tabheader);


                #endregion
                #region Footer Padding

                // add footer
                PdfPTable tabFooterSectoin = new PdfPTable(1);
                tabFooterSectoin.WidthPercentage = 100.0f;
                tabFooterSectoin.DefaultCell.Border = Rectangle.NO_BORDER;//Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                tabFooterSectoin.AddCell(SetCellProperties("Fare does not include fees that may be charged by your bank. Please contact your bank directly for inquiries.", 0, 0, 9f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                doc.Add(tabFooterSectoin);
                #endregion

                //pdfBytes = memStream.ToArray();
            }

            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                throw ex;
            }

            finally
            {
                writer.CloseStream = true;
                doc.Close();

            }

            //return pdfBytes;
        }

        public void GenerateInvoicePDF2(String filePath, OrderDetails objOrder)
        {
            FontFactory.RegisterDirectories();


            var pageSize = new iTextSharp.text.Rectangle(640f, 800F);
            Document doc = new Document(PageSize.A4, 65f, 65f, 40f, 40f);
            //byte[] pdfBytes;

            MemoryStream memStream = new MemoryStream();
            //PdfWriter writer = PdfWriter.GetInstance(doc, memStream);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            // calling PageEventHelper class to Include in document
           // PageEventHelper pageEventHelper = new PageEventHelper();
          //  writer.PageEvent = pageEventHelper;
            doc.Open();
            try
            {
                // 1 f = 1.71Pixel
                #region Header Section
                float[] widths = new float[] { 50, 50 };
                PdfPTable tabheader = new PdfPTable(widths);
                tabheader.WidthPercentage = 100.0f;
                tabheader.DefaultCell.Border = 0;// No Border
                   /*
                    //tabheader.
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(@"C:\Users\Sky\Desktop\All_Pics\SkyIncLogo.png");
                //Resize image depend upon your need
                jpg.ScaleToFit(40f, 40f);
                // Give space before image
                jpg.SpacingBefore = 9f;
                //Give some space after the image
                jpg.SpacingAfter = 1f;
                jpg.Alignment = Element.ALIGN_LEFT;
                */
               // tabheader.AddCell(jpg);
                tabheader.AddCell(SetCellProperties("Uber", 1, 0, 20f, 1, 0, 0));
                tabheader.AddCell(SetCellProperties("Sun, Sep 08, 2019", 1, 2, 9f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0.5f, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                //  tabheader.AddCell(jpg);
                tabheader.AddCell(SetCellProperties("Here's your receipt for your trip", 2, 0, 14f, 0, 0, 0));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("We hope you enjoyed your ride this [morning]", 2, 0, 9f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("Total", 1, 0, 10f, 0, 0, 0));
                tabheader.AddCell(SetCellProperties("$13,500", 1, 2, 10f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 5f, 0, 0, 0.5f));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("Tarifa base", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("$13,500", 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("Tiempo ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("$13,500", 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("Distancia ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("$13,500", 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 5f, 0, 0, 0.5f, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("Subtotal ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("$13,500", 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("Cuota de solicitud ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("$13,500", 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("Redondeo hacia abajo ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("$13,500", 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 5f, 0, 0, 0.5f, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("Amount Charged", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 1, 2, 10f, 0, 0, 0));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("Paid in cash ", 1, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("$13,500", 1, 2, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties("Visit the trip page for more information, including invoices (where available)", 2, 0, 9f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("", 2, 1, 5f, 0, 0, 0.5f, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("You rode with LEOVANI", 2, 0, 12f, 0, 0, 0));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));
                tabheader.AddCell(SetCellProperties("Issued on behalf of LEOVANI GARCIA OLIVAREZ", 2, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));
                tabheader.AddCell(SetCellProperties("UberX     12.90 kilometres | 22 min(s)", 2, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));

                tabheader.AddCell(SetCellProperties("9:16 | Cra. 18 #93a-83, Bogotá, Colombia", 2, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 8f, 1, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                tabheader.AddCell(SetCellProperties("9:40 | Cra. 103b #140a-82, Bogotá, Colombia", 2, 0, 10f, 0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                tabheader.AddCell(SetCellProperties(Environment.NewLine, 2, 0, 10f, 1, 0, 0));
                // add to doc
                doc.Add(tabheader);


                #endregion
                #region Footer Padding

                // add footer
                PdfPTable tabFooterSectoin = new PdfPTable(1);
                tabFooterSectoin.WidthPercentage = 100.0f;
                tabFooterSectoin.DefaultCell.Border = Rectangle.NO_BORDER;//Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                tabFooterSectoin.AddCell(SetCellProperties("Fare does not include fees that may be charged by your bank. Please contact your bank directly for inquiries.", 0, 0, 9f,0, 0, 0, System.Drawing.Color.Gray, System.Drawing.Color.Gray));
                doc.Add(tabFooterSectoin);
                #endregion

                //pdfBytes = memStream.ToArray();
            }

            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                throw ex;
            }

            finally
            {
                writer.CloseStream = true;
                doc.Close();

            }

            //return pdfBytes;
        }

    }

    public class PageEventHelper : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate template;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;
        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                //PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont();//BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                template = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
            }
            catch (System.IO.IOException ioe)
            {
            }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            int pageN = writer.PageNumber;
            String text =  pageN + "/";//"Page " + pageN + " of ";
            float len = bf.GetWidthPoint(text, 8);

            Rectangle pageSize = document.PageSize;

            cb.SetRGBColorFill(100, 100, 100);

            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(299, pageSize.GetBottom(30));//pageSize.GetLeft(40),pageSize.GetBottom(30));
            cb.ShowText(text);
            cb.EndText();

            cb.AddTemplate(template, 299 + len, pageSize.GetBottom(30));


        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            template.BeginText();
            template.SetFontAndSize(bf, 8);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber - 1));
            template.EndText();
        }


    }
}
