using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Globalization;

namespace CreatePDF
{
    public class AirtelBills
    {
        public AirtelBills()
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

            Font fontArial = new Font(FontFactory.GetFont("Calibri Light", fontSize, FontStyle, baseColor));

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

            Font fontArial = new Font(FontFactory.GetFont("Calibri Light", fontSize, FontStyle));

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

            Font fontArial = new Font(FontFactory.GetFont("Calibri Light", fontSize, FontStyle, new BaseColor(System.Drawing.Color.OrangeRed)));
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


        public void GenerateInvoicePDF(String filePath)
        {
            // 

            // set headers

            PdfPTable tabMainheader = new PdfPTable(1);
            tabMainheader.WidthPercentage = 100.0f;
            tabMainheader.DefaultCell.Border = 0;// No Border

            // 1 f = 1.71Pixel
            #region Header Section
            float[] widths = new float[] { 50, 50 };
            PdfPTable tabheader = new PdfPTable(widths);
            tabheader.WidthPercentage = 100.0f;
            tabheader.DefaultCell.Border = 0;// No Border

            tabheader.AddCell(SetCellProperties("2/16/2020", 0, 0, 9f, 0, 0, 0));
            tabheader.AddCell(SetCellProperties("Airtel Online Prepaid Recharge, Online Mobile Recharge", 0, 0, 9f, 0, 0, 0));
            tabMainheader.AddCell(tabheader);

            #endregion Header Section


            // Main header table


            #region Footer Section

            PdfPTable tabMainFooter = new PdfPTable(1);
            tabMainFooter.WidthPercentage = 100.0f;
            tabMainFooter.DefaultCell.Border = 0;// No Border

            float[] subFooterwidths = new float[] { 100 };
            PdfPTable tabfooter = new PdfPTable(subFooterwidths);
            tabfooter.WidthPercentage = 100.0f;
            tabfooter.DefaultCell.Border = 0;// No Border
            // rows
            tabfooter.AddCell(SetCellProperties("https://www.airtel.in/prepaid-recharge/complete", 0, 0, 9f, 0, 0, 0));
            tabMainFooter.AddCell(tabfooter);
            #endregion Footer Section

            FontFactory.RegisterDirectories();


            //var pageSize = new iTextSharp.text.Rectangle(640f, 800F);
            Document doc = new Document(PageSize.A4, 20f, 10f, 30f, 30f);
            //byte[] pdfBytes;

            MemoryStream memStream = new MemoryStream();
            //PdfWriter writer = PdfWriter.GetInstance(doc, memStream);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            // calling PageEventHelper class to Include in document
            InvoiceAirtelPageEventHelper pageEventHelper = new InvoiceAirtelPageEventHelper(tabMainheader, tabMainFooter);
            writer.PageEvent = pageEventHelper;
            doc.Open();
            try
            {


                float[] newwidths = new float[] { 3,47, 45, 5 };
                PdfPTable newtabheader = new PdfPTable(newwidths);
                newtabheader.WidthPercentage = 100.0f;
                newtabheader.DefaultCell.Border = 0;// No Border

                //tabheader.
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(@"C:\Users\g521753\Desktop\SKY-Drive\localbills\airtel.png");
                //Resize image depend upon your need
                jpg.ScaleToFit(100f, 100f);
                // Give space before image
                jpg.SpacingBefore = 9f;
                //Give some space after the image
                jpg.SpacingAfter = 1f;
                jpg.Alignment = Element.ALIGN_LEFT;
                PdfPCell imgCell1 = new PdfPCell(jpg);
                newtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 4, 0, 16f, 1, 0, 0));
                
                imgCell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                newtabheader.AddCell(SetCellProperties("", 1, 0, 16f, 1, 0, 0));
                newtabheader.AddCell(imgCell1);
                newtabheader.AddCell(SetCellProperties("", 1, 0, 16f, 1, 0, 0));
                newtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 0, 0, 14f, 1, 0, 0));

                newtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 4, 0, 16f, 1, 0, 0));

                newtabheader.AddCell(SetCellProperties("", 1, 0, 16f, 0, 0, 0));
                newtabheader.AddCell(SetCellProperties("Recharge Details", 0, 0, 32f, 0, 0, 0));
                newtabheader.AddCell(SetCellProperties("", 1, 0, 16f, 1, 0, 0));
                newtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 0, 0, 14f, 1, 0, 0));

                newtabheader.AddCell(SetCellProperties("", 1, 0, 16f, 1, 0, 0));
                newtabheader.AddCell(SetCellProperties("16-Feb-2020", 0, 0, 36f, 0, 0, 0));
                newtabheader.AddCell(SetCellProperties("", 1, 0, 16f, 1, 0, 0));
                newtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 0, 0, 14f, 1, 0, 0));

                doc.Add(newtabheader);

                float[] amwidths = new float[] { 3,37, 60 };
                PdfPTable amtabheader = new PdfPTable(amwidths);
                amtabheader.WidthPercentage = 100.0f;
                amtabheader.DefaultCell.Border = 0;// No Border

                amtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 3, 0, 16f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties("", 1, 0, 8f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties("Mobile Number", 1, 0, 8f, 0, 0, 0));
                amtabheader.AddCell(SetCellProperties("Date", 1, 0, 8f, 0, 0, 0));

                amtabheader.AddCell(SetCellProperties("", 1, 0, 8f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties("9560160145", 1, 0, 14f, 0, 0, 0));
                amtabheader.AddCell(SetCellProperties("16-Feb-2020", 1, 0, 14f, 0, 0, 0));

                amtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 3, 0, 16f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties("", 1, 0, 8f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties("Transaction By ", 1, 0, 8f, 0, 0, 0));
                amtabheader.AddCell(SetCellProperties("TRANSACTION REFERENCE", 1, 0, 8f, 0, 0, 0));

                amtabheader.AddCell(SetCellProperties("", 1, 0, 8f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties("Credit Card", 1, 0, 14f, 0, 0, 0));
                amtabheader.AddCell(SetCellProperties("200216410709", 1, 0, 14f, 0, 0, 0));


                //iTextSharp.text.Image jpgline = iTextSharp.text.Image.GetInstance(@"C:\Users\g521753\Desktop\SKY-Drive\localbills\airtel.png");
                ////Resize image depend upon your need
                //jpgline.ScaleToFit(100f, 100f);
                //// Give space before image
                //jpgline.SpacingBefore = 9f;
                ////Give some space after the image
                //jpgline.SpacingAfter = 1f;
                //jpgline.Alignment = Element.ALIGN_LEFT;
                //PdfPCell jpglineCell1 = new PdfPCell(jpgline);
                //newtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 4, 0, 16f, 1, 0, 0));

                //imgCell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //newtabheader.AddCell(SetCellProperties("", 1, 0, 16f, 1, 0, 0));
                //newtabheader.AddCell(imgCell1,);
                amtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 3, 0, 16f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties("", 1, 0, 8f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties("", 2, 1, 2f, 0, 0, 1f, System.Drawing.Color.Gray, System.Drawing.Color.Gray));

                amtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 3, 0, 16f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties("", 1, 0, 8f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties("AMOUNT PAID ", 1, 0, 10f, 0, 0, 0));
                amtabheader.AddCell(SetCellProperties("", 1, 0, 8f, 1, 0, 0));

                amtabheader.AddCell(SetCellProperties("", 1, 0, 8f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties("Rs. 6999.0", 1, 0, 14f, 0, 0, 0));
                amtabheader.AddCell(SetCellProperties("", 1, 0, 16f, 1, 0, 0));

                amtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 3, 0, 16f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 3, 0, 16f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 3, 0, 16f, 1, 0, 0));
                amtabheader.AddCell(SetCellProperties(Environment.NewLine + "", 3, 0, 16f, 1, 0, 0));

                doc.Add(amtabheader);

                #region Footer Padding

                float[] totalSummaryWidth = new float[] { 3,3,94 };
                PdfPTable totalSummaryTable = new PdfPTable(totalSummaryWidth);
                totalSummaryTable.WidthPercentage = 100.0f;
                totalSummaryTable.DefaultCell.Border = 0;// No Border
                totalSummaryTable.AddCell(SetCellProperties("", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("1.", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("All payments made are subject to realization of the same.", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("2.", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("Customer is liable to pay surcharge levied for delayed payment at such rates as specified by Airtel from time to time.", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("3.", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("Customer is advised to make payment in full of the due amount along with delayed payment charges, if any.", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("4.", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("The payment made by the customer vide this receipt shall under no circumstances be deemed for full & final settlement.", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("5.", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("All claims subject to exclusive jurisdiction of Delhi courts only.", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("6.", 0, 0, 9f, 0, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("This is a system generated document and does not require signature. Any unauthorized use, disclosure, dissemination, or copying of this document is strictly prohibited and may be unlawful.", 0, 0, 9f, 0, 0, 0));
                doc.Add(totalSummaryTable);
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

    public class InvoiceAirtelPageEventHelper : PdfPageEventHelper
    {
        PdfPTable _pdfPTableHeader;
        PdfPTable _pdfTableFooter;
        public InvoiceAirtelPageEventHelper(PdfPTable pdfPTableHeader, PdfPTable pdfTableFooter)
        {
            _pdfPTableHeader = pdfPTableHeader;
            _pdfTableFooter = pdfTableFooter;
        }
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;



        #region Fields
        private string _header;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion
        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                //PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont();//BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 10);
                footerTemplate = cb.CreateTemplate(50, 10);
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
            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(1);
            pdfTab = this._pdfPTableHeader;
           
            {
                String text = pageN + "/";//"Page " + pageN + " of ";
                float len = bf.GetWidthPoint(text,8);

                Rectangle pageSize = document.PageSize;

                //cb.SetRGBColorFill(100, 100, 100);

                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(549, pageSize.GetBottom(10));//pageSize.GetLeft(40),pageSize.GetBottom(30));
                cb.ShowText(text);
                cb.EndText();
                cb.AddTemplate(footerTemplate, 549 + len, pageSize.GetBottom(10));
            }




            //pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 100;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;    

            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 20, document.PageSize.Height - 20, writer.DirectContent);
            //set pdfContent value

            // FOR FOOTER

            PdfPTable pdfTabFooter = new PdfPTable(1);
            pdfTabFooter = this._pdfTableFooter;
            pdfTabFooter.DefaultCell.Border = 0;
            pdfTabFooter.TotalWidth = document.PageSize.Width;
            pdfTabFooter.WidthPercentage = 100;


            pdfTabFooter.WriteSelectedRows(0, -1, 20, document.PageSize.GetBottom(20), writer.DirectContent);
            ////Move the pointer and draw line to separate header section from rest of page
            //cb.MoveTo(40, document.PageSize.Height - 100);
            //cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
            //cb.Stroke();

            //Move the pointer and draw line to separate footer section from rest of page
            //cb.MoveTo(40, document.PageSize.GetBottom(50));
            //cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            //cb.Stroke();


        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 8);
            headerTemplate.SetTextMatrix(0, 0);
            headerTemplate.ShowText("" + (writer.PageNumber - 1));
            headerTemplate.EndText();

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 8);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText("" + (writer.PageNumber - 1));
            footerTemplate.EndText();
        }


    }
}
