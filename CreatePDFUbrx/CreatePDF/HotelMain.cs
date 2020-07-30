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
    public class HotelMain
    {
        public HotelMain()
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

            Font fontArial = new Font(FontFactory.GetFont("Book Antiqua", fontSize, FontStyle, baseColor));

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

            Font fontArial = new Font(FontFactory.GetFont("Book Antiqua", fontSize, FontStyle));

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

            Font fontArial = new Font(FontFactory.GetFont("Book Antiqua", fontSize, FontStyle, new BaseColor(System.Drawing.Color.OrangeRed)));
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

        public void GenerateInvoicePDF(String filePath, HotelsDetails objhotels)
        {
            // 

            // set headers

            PdfPTable tabMainheader = new PdfPTable(1);
            tabMainheader.WidthPercentage = 100.0f;
            tabMainheader.DefaultCell.Border = 0;// No Border

            // 1 f = 1.71Pixel
            #region Header Section
            float[] widths = new float[] { 5, 10, 20, 65 };
            PdfPTable tabheader = new PdfPTable(widths);
            tabheader.WidthPercentage = 100.0f;
            tabheader.DefaultCell.Border = 0;// No Border

            //tabheader.
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(@"C:\Users\g521753\Desktop\SKY-Drive\NewBills\hotel-logo.jpg");
            //Resize image depend upon your need
            jpg.ScaleToFit(50f, 50f);
            // Give space before image
            jpg.SpacingBefore = 9f;
            //Give some space after the image
            jpg.SpacingAfter = 1f;
            jpg.Alignment = Element.ALIGN_LEFT;
            PdfPCell imgCell1 = new PdfPCell(jpg);
            imgCell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tabheader.AddCell(SetCellProperties("", 1, 0, 16f, 1, 0, 0));
            tabheader.AddCell(imgCell1);
            tabheader.AddCell(SetCellProperties("", 1, 0, 16f, 1, 0, 0));
            tabheader.AddCell(SetCellProperties(Environment.NewLine + "GOSCAR S.A. DE C.V. ", 0, 0, 14f, 1, 0, 0));
            //doc.Add(tabheader);
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
            tabfooter.AddCell(SetCellProperties("IDEMIA IDENTITY & SECURITY FRANCE SAS", 0, 0, 9f, 0, 0, 0));
            tabfooter.AddCell(SetCellProperties("07/03/2020         08:40", 0, 0, 9f, 0, 0, 0));
            tabfooter.AddCell(SetCellProperties("PAGO EN UNA SOLA EXHIBICION", 0, 0, 9f, 0, 0, 0));
            tabfooter.AddCell(SetCellProperties("EFECTOS FISCALES AL PAGO", 0, 0, 9f, 0, 0, 0));

            tabMainFooter.AddCell(tabfooter);
            #endregion Footer Section

            FontFactory.RegisterDirectories();


            //var pageSize = new iTextSharp.text.Rectangle(640f, 800F);
            Document doc = new Document(PageSize.A4, 20f, 15f, 300f, 80f);
            //byte[] pdfBytes;

            MemoryStream memStream = new MemoryStream();
            //PdfWriter writer = PdfWriter.GetInstance(doc, memStream);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            // calling PageEventHelper class to Include in document
            HotelMainPageEventHelper pageEventHelper = new HotelMainPageEventHelper(tabMainheader, tabMainFooter);
            writer.PageEvent = pageEventHelper;
            doc.Open();
            try
            {

                //
                #region SubHeader3 Section


                float[] mainrowswidth = new float[] { 5, 10, 50, 7, 8, 10, 10 };
                PdfPTable mainrowTab = new PdfPTable(mainrowswidth);
                mainrowTab.WidthPercentage = 100.0f;
                mainrowTab.DefaultCell.Border = 0;// No Border
                DateTime date = new DateTime(2020, 2, 16);
                int dateIncr = 0;

                for (int i = 1; i < 45; i++)
                {

                    int col = i;
                    DateTime dt = date.AddDays(dateIncr);
                    string dates = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dateIncr = dateIncr + 1;
                    Console.WriteLine(dates);
                    // row main item 1
                    mainrowTab.AddCell(SetCellProperties(col.ToString(), 0, 0, 10f, 1, 0, 0));
                    mainrowTab.AddCell(SetCellProperties(dates, 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties("RENTA RESIDENCIA/RESIDENCE RENT", 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties(" 14818", 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties(" 5002", 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties("3,412.50", 0, 2, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties("", 0, 0, 10f, 0, 0, 0));
                    col = i + 1;
                    mainrowTab.AddCell(SetCellProperties(col.ToString(), 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties(dates, 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties("I.V.A", 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties(" 14818", 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties(" 5002", 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties("546.00", 0, 2, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties("", 0, 0, 10f, 0, 0, 0));
                    i = col;
                }
                /*
                for (int i = 1; i < 22; i++)
                {

                    int col = i;
                    DateTime dt = date.AddDays(dateIncr);
                    string dates = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dateIncr = dateIncr + 3;
                    Console.WriteLine(dates);
                    // row main item 1
                    mainrowTab.AddCell(SetCellProperties(col.ToString(), 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties(dates, 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties(" 4 piece Camisa, 3 Playera Ropa Lavenderia", 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties(" 14818", 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties(" 5002", 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties("3,412.50", 0, 2, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties("", 0, 0, 10f, 0, 0, 0));
                    col = i + 1;
                    mainrowTab.AddCell(SetCellProperties(col.ToString(), 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties(dates, 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties("I.V.A", 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties(" 14818", 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties(" 5002", 0, 0, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties("546.00", 0, 2, 9f, 0, 0, 0));
                    mainrowTab.AddCell(SetCellProperties("", 0, 0, 10f, 0, 0, 0));
                    i = col;
                }
                */
                doc.Add(mainrowTab);
                #endregion SubHeader3 Section


                #region Footer Padding


                float[] totalSummaryWidth = new float[] { 70, 15, 15 };
                PdfPTable totalSummaryTable = new PdfPTable(totalSummaryWidth);
                totalSummaryTable.WidthPercentage = 100.0f;
                totalSummaryTable.DefaultCell.Border = 0;// No Border

                totalSummaryTable.AddCell(SetCellProperties("**  SUB-TOTAL  **", 0, 1, 12f, 1, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("75,075.00", 0, 2, 12f, 1, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("", 0, 0, 9f, 0, 0, 0));

                totalSummaryTable.AddCell(SetCellProperties("I.V.A", 0, 1, 12f, 1, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("12,012.00", 0, 2, 12f, 1, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("", 0, 0, 9f, 0, 0, 0));

                totalSummaryTable.AddCell(SetCellProperties("** TOTAL **", 0, 1, 12f, 1, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("87,087.00", 0, 2, 12f, 1, 0, 0));
                totalSummaryTable.AddCell(SetCellProperties("", 0, 0, 9f, 0, 0, 0));

                totalSummaryTable.AddCell(SetCellProperties("[OCHENTA Y SIETE MIL OCHENTA Y SIETE PESOS 50/100 M.N ]", 0, 0, 9f, 0, 0, 0));


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

    public class HotelMainPageEventHelper : PdfPageEventHelper
    {
        PdfPTable _pdfPTableHeader;
        PdfPTable _pdfTableFooter;
        public HotelMainPageEventHelper(PdfPTable pdfPTableHeader, PdfPTable pdfTableFooter)
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
                headerTemplate = cb.CreateTemplate(100, 300);
                footerTemplate = cb.CreateTemplate(50, 50);
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



            ////Add paging to header
            //{
            //    cb.BeginText();
            //    cb.SetFontAndSize(bf, 12);
            //    cb.SetTextMatrix(document.PageSize.GetRight(200), document.PageSize.GetTop(45));
            //    cb.ShowText(text);
            //    cb.EndText();
            //    float len = bf.GetWidthPoint(text, 12);
            //    //Adds "12" in Page 1 of 12
            //    cb.AddTemplate(headerTemplate, document.PageSize.GetRight(200) + len, document.PageSize.GetTop(45));
            //}
            //Add paging to footer
            {
                String text = "Página " + pageN + " de ";//"Page " + pageN + " of ";
                float len = bf.GetWidthPoint(text, 8);

                Rectangle pageSize = document.PageSize;

                //cb.SetRGBColorFill(100, 100, 100);

                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(299, pageSize.GetBottom(30));//pageSize.GetLeft(40),pageSize.GetBottom(30));
                cb.ShowText(text);
                cb.EndText();
                cb.AddTemplate(footerTemplate, 299 + len, pageSize.GetBottom(30));
            }




            //pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.TotalWidth = document.PageSize.Width - 40f;
            pdfTab.WidthPercentage = 100;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;    

            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 15, document.PageSize.Height - 10, writer.DirectContent);
            //set pdfContent value

            // FOR FOOTER

            PdfPTable pdfTabFooter = new PdfPTable(1);
            pdfTabFooter = this._pdfTableFooter;
            pdfTabFooter.DefaultCell.Border = 0;
            pdfTabFooter.TotalWidth = document.PageSize.Width;
            pdfTabFooter.WidthPercentage = 100;


            pdfTabFooter.WriteSelectedRows(0, -1, 15, document.PageSize.GetBottom(65), writer.DirectContent);
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
