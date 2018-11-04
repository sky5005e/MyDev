using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using System.Runtime.InteropServices;
using System.Net;
using System.Diagnostics;


using HtmlAgilityPack;


namespace WFDownloadFile
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
            timer1.Start(); // Then I start timer



        }
        String url = "http://www.chiropracticcanada.ca/en-us/find-a-chiropractor.aspx?searchType=2&province=ON&pageSize=0&page=1";

        public void SetComboItem(string id, string value)
        {
            HtmlElement ee = this.webBrowser1.Document.GetElementById(id);
            foreach (HtmlElement item in ee.Children)
            {
                if (item.OuterHtml.ToLower().IndexOf(value.ToLower()) >= 0)
                {
                    item.SetAttribute("selected", "selected");
                    item.InvokeMember("onChange");
                }
                else
                {
                    item.SetAttribute("selected", "");
                }
            }

            ee = this.webBrowser1.Document.GetElementById(id + "-input");
            ee.InnerText = value;
        }
        private void test()
        {
            //            string input = "super exemple of string key : text I want to keep - end of my string";
            //var match = Regex.Match(input, @"key : (.+?)-").Groups[1].Value;
            //String input1 = "window.open('http://direktlink.prospective.ch?view=F6220430-FC61-41F6-A3558A7783E7D755', '_blank', 'width=660px,height=710px,location=yes,scrollbars=yes,left=100,top=200,resizable=yes')";
            // String r = input1.Split(new string[] { "('" }, StringSplitOptions.None)[1]
            //.Split(',')[0]
            //.Trim();
            //            String s = Regex.Match("window.open('http://direktlink.prospective.ch?view=F6220430-FC61-41F6-A3558A7783E7D755', '_blank', 'width=660px,height=710px,location=yes,scrollbars=yes,left=100,top=200,resizable=yes')", @"('(.+?),").Groups[1].Value;
            //         
            this.SetComboItem("MySite.condition_s", "First");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            #region Comments

            //
            //test();
            //new Itstep.Jobmachine.Crawler.Core.Providers.DeloitteProvider().GetJobAds();
           // new AccentureProvider().GETWSDL();
           // new Itstep.Jobmachine.Crawler.Core.Providers.HelvetiaProvider().GetJobAds();
           // new Itstep.Jobmachine.Crawler.Core.Providers.EthProvider().GetJobAds();
            //new Itstep.Jobmachine.Crawler.Core.Providers.EmilFreyProvider().GetJobAds();
            //new Itstep.Jobmachine.Crawler.Core.Providers.TamediaProvider().GetJobAds();
            
           //new Itstep.Jobmachine.Crawler.Core.Providers.SyngentaProvider().doMainOpt();
           //new Itstep.Jobmachine.Crawler.Core.Providers.HelsanaProvider().GetJobAds();
            //new Itstep.Jobmachine.Crawler.Core.Providers.HolcimProvider().GetJobAds();
            //new Itstep.Jobmachine.Crawler.Core.Providers.AxaWinterthurProvider().GetJobAds();
            //new Itstep.Jobmachine.Crawler.Core.Providers.PsiProvider().GetJobAds();
            //new Itstep.Jobmachine.Crawler.Core.Providers.UnitStGallenProvider().GetJobAds();
            //webBrowser1.Url = new Uri("https://ssl1.peoplexs.com/Peoplexs22/CandidatesPortalNoLogin/Vacancies.cfm?PortalID=659&Match=18921&Trefwoord=&Sorteren=");


           // Itstep.Jobmachine.Crawler.Core.WebRequestResponse.PostMessageToURL("http://www2.unio.ch/cgi-bin/WebObjects/UNUserInterface.woa/wa/customPublicationMicroSite?unit=holcim&medium=holcim&skin=holcim", "3.5=WONoSelectionString&3.9=WONoSelectionString");

            //new Itstep.Jobmachine.Crawler.Core.Providers.ThyssenKruppProvider().GetJobAds();

           // new Itstep.Jobmachine.Crawler.Core.Providers.DsmProvider().GetJobAds();

            //new Itstep.Jobmachine.Crawler.Core.Providers.RohnerProvider().GetJobAds();
            //new Itstep.Jobmachine.Crawler.Core.Providers.CarbogenAmcisProvider().GetJobAds();
        
           // new Itstep.Jobmachine.Crawler.Core.Providers.SiegfriedProvider().GetJobAds();

            //new Itstep.Jobmachine.Crawler.Core.Providers.CantonLuzernProvider().GetJobAds();


            //Itstep.Jobmachine.Crawler.Core.WebRequestResponse.DoRequest(new Uri("https://glencorexstratajobs.nga.net.au/cp/index.cfm?CurATC=Search&CurBID=ea5bb92a-2b41-4cfc-b248-9db401357a6d"), null);
            #endregion

            //new Itstep.Jobmachine.Crawler.Core.Providers.GlencoreProvider().GetJobAds();

            new Itstep.Jobmachine.Crawler.Core.Providers.CityZurichProvider().GetJobAds();


        }
        private void WebBrowser1_ProgressChanged(Object sender, WebBrowserProgressChangedEventArgs e)
        {
            //progressBar1.Maximum = (int)e.MaximumProgress;
            //progressBar1.Value = (int)e.CurrentProgress;
        }
        public void ExportToExcel(List<ChiropractorCls> Chirop, String state)
        {
            // Load Excel application
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            // Create empty workbook
            excel.Workbooks.Add();

            // Create Worksheet from active sheet
            Microsoft.Office.Interop.Excel._Worksheet workSheet = excel.ActiveSheet;

            // I created Application and Worksheet objects before try/catch,
            // so that i can close them in finnaly block.
            // It's IMPORTANT to release these COM objects!!
            try
            {
                // ------------------------------------------------
                // Creation of header cells
                // ------------------------------------------------
                workSheet.Cells[1, "A"] = "Name";
                workSheet.Cells[1, "B"] = "Address1";
                workSheet.Cells[1, "C"] = "Address2";
                workSheet.Cells[1, "D"] = "City";
                workSheet.Cells[1, "E"] = "ZipCode";
                workSheet.Cells[1, "F"] = "State";
                workSheet.Cells[1, "G"] = "ContactNo";

                // ------------------------------------------------
                // Populate sheet with some real data from "cars" list
                // ------------------------------------------------
                int row = 2; // start row (in row 1 are header cells)
                foreach (ChiropractorCls cp in Chirop)
                {
                    workSheet.Cells[row, "A"] = cp.Name;
                    workSheet.Cells[row, "B"] = cp.Address1;
                    workSheet.Cells[row, "C"] = cp.Address2;
                    workSheet.Cells[row, "D"] = cp.City;
                    workSheet.Cells[row, "E"] = cp.ZipCode;
                    workSheet.Cells[row, "F"] = cp.State;
                    workSheet.Cells[row, "G"] = cp.ContactNo;//string.Format("{0} Phone", cp.MaximumSpeed);

                    row++;
                }

                // Apply some predefined styles for data to look nicely :)
                workSheet.Range["A1"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic1);

                // Define filename
                string fileName = string.Format(@"{0}\{1}_Excelchiropracticcanada.xlsx", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), state);

                // Save this data as a file
                workSheet.SaveAs(fileName);

                // Display SUCCESS message
               // MessageBox.Show(string.Format("The file '{0}' is saved successfully!", fileName));
            }
            catch (Exception exception)
            {
                MessageBox.Show("Exception",
                    "There was a PROBLEM saving Excel file!\n" + exception.Message,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Quit Excel application
                excel.Quit();

                // Release COM objects (very important!)
                if (excel != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

                if (workSheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);

                // Empty variables
                excel = null;
                workSheet = null;

                // Force garbage collector cleaning
                GC.Collect();
            }
        }
        private List<ChiropractorCls> DownloadHTMLData(String URL)
        {
            List<ChiropractorCls> lstChiroP = new List<ChiropractorCls>();
            //String htmlData = String.Empty;
            try
            {
                //WebClient client = new WebClient();
                // string bodyHtml = webBrowser1.Document.Body.InnerHtml; //client.DownloadString(URL);
                // HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();

                // htmlDocument.LoadHtml(bodyHtml);
                // string html1 = htmlDocument.DocumentNode.OuterHtml;

                HtmlWeb web = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument docMain = web.Load(URL);
                // HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();


                string html1 = docMain.DocumentNode.OuterHtml;
                // I don't see no jumbled data here
                //("//*[@class=\"rgMasterTable\"]"))
                // ("//table[@id='ContentPlaceHolder1_rgResult_ctl00']")
                //var divResult = htmlDocument.DocumentNode.SelectSingleNode(ChiropractorLocator);
                foreach (HtmlNode contactinfo in docMain.DocumentNode.SelectNodes("//div[@class=\"ContactInfo\"]"))
                {
                    //HtmlAttribute htmlatt = link.Attributes["id"];
                    String htmlData = contactinfo.OuterHtml;
                    HtmlAgilityPack.HtmlDocument docSub = new HtmlAgilityPack.HtmlDocument();
                    docSub.LoadHtml(htmlData);


                    String _name = docSub.DocumentNode.SelectSingleNode("//h2").InnerText;
                    String addinfo = contactinfo.InnerText.Replace(_name, "");
                    addinfo = addinfo.Replace("\n", "");
                    String[] arAddress = addinfo.Split('\r').Where(q => q.Trim().Length > 0).ToArray();
                    String _address1 = String.Empty;
                   
                    String _address2 = string.Empty;
                    String _city = String.Empty;
                    String _state = string.Empty;
                    String _Zip = String.Empty;
                    String _contactNum = String.Empty;
                    foreach (string add1 in arAddress)
                    {

                        if (add1.Contains("(") && add1.Contains(")"))
                            _contactNum = add1;
                        else if (add1.Contains(","))
                        {
                            String[] arAdd2 = add1.Split(',');
                            int Count = arAdd2.Count();
                            if (Count > 2)
                            {
                                _city = arAdd2[0].Trim();
                                _state = arAdd2[1].Trim();
                                _Zip = arAdd2[2].Trim();
                            }
                            else if (Count == 2)
                            {
                                _state = arAdd2[0].Trim();
                                _Zip = arAdd2[1].Trim();
                            }

                        }
                        else if (!add1.Contains("(") && !add1.Contains("#") && !add1.Contains("#"))
                        {
                            _address1 = add1.Trim();
                        }
                        else
                        {
                            _address2 = add1.Trim();
                        }
                    }
                    ChiropractorCls objChiro = new ChiropractorCls();
                    objChiro.Name = _name;
                    objChiro.Address1 = _address1;
                    objChiro.Address2 = _address2;

                    objChiro.City = _city;
                    objChiro.ZipCode = _Zip;
                    objChiro.State = _state;
                    objChiro.ContactNo = _contactNum;
                    //var brtags = docSub.DocumentNode.SelectNodes("//br");
                    //var _contactNo = brtags.LastOrDefault();
                    //string _address = string.Empty;
                    //for (int i = 0; i < brtags.Count - 1; i++)
                    //{
                    //   // _address = brtags[
                    //}

                    lstChiroP.Add(objChiro);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstChiroP;
        }
        private String DownloadHTMLData1(String URL)
        {
            String htmlData = String.Empty;
            try
            {
                //WebClient client = new WebClient();
                string bodyHtml = webBrowser1.Document.Body.InnerHtml; //client.DownloadString(URL);
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                //HtmlWeb web = new HtmlWeb();
                //HtmlAgilityPack.HtmlDocument doc1 = web.Load(URL);
                htmlDocument.LoadHtml(bodyHtml);
                string html1 = htmlDocument.DocumentNode.OuterHtml; // I don't see no jumbled data here
                //("//*[@class=\"rgMasterTable\"]"))

                foreach (HtmlNode link in htmlDocument.DocumentNode.SelectNodes("//table[@id='ContentPlaceHolder1_rgResult_ctl00']"))
                {
                    HtmlAttribute htmlatt = link.Attributes["id"];
                    htmlData = link.OuterHtml;
                }

                foreach (HtmlNode link in htmlDocument.DocumentNode.SelectNodes("//div[@class=\"rgNumPart\"]"))
                {
                    HtmlAttribute htmlatt = link.Attributes["id"];
                    htmlData = link.OuterHtml;
                }
                var current_activerpt = htmlDocument.DocumentNode.SelectNodes("//a[@class=\"rgCurrentPage\"]");
                
                var iddvnum = htmlDocument.DocumentNode.SelectNodes("//div[@class=\"rgWrap rgNumPart\"]");

                HtmlElementCollection links = webBrowser1.Document.GetElementsByTagName("A");

                foreach (HtmlElement link in links)
                {
                    if (link.InnerText.Equals("My Assigned"))
                        link.InvokeMember("Click");
                }

                if (!webBrowser1.Document.GetElementById("btnVerify").GetAttribute("value").Contains("Please"))
                {
                    webBrowser1.Document.GetElementById("btnVerify").InvokeMember("Click");
                    //return true;

                }

            }
            catch (Exception ex)
            {
                htmlData = "Error" + ex;
            }

            return htmlData;
        }

       

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

       
        private void clientDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("File downloaded");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var slcEle = webBrowser1.Document.GetElementsByTagName("select");
            //if (slcEle.Count == 1)
            //{
            //    //do something
            //    //new  Itstep.Jobmachine.Crawler.Core.Providers.SyngentaProvider().SetComboItem(slcEle[0] as System.Windows.Forms.HtmlElement, "2");
            //    var option = slcEle[0].Children.Cast<HtmlElement>().First(x => x.GetAttribute("value").Equals("12"));
            //    option.SetAttribute("selected", "selected");
            //    option.InvokeMember("onChange");
            //    //slcEle[0].SetAttribute("value", "2");
               

            //}
            var slcEle = webBrowser1.Document.GetElementsByTagName("select");
            if (slcEle.Count == 1)
            {
                HtmlElement alink = slcEle[0].Parent.Children[1];
                alink.InvokeMember("Click"); //++
            }
            //var links = webBrowser1.Document.GetElementsByTagName("table");
            //foreach (HtmlElement link in links)
            //{
            //    if (link.GetAttribute("class") == "browselist")// name Start1
            //    {
            //        var alinks = link.Children.GetElementsByName("a");
            //        foreach (HtmlElement al in alinks)
            //        {
            //            if (al.InnerText == ">>")
            //            {

            //            }
            //        }
            //    }
            //}

            //string[] arState = { "ON" };//{ "AB", "BC", "MB", "NB", "NL", "NT", "NS", "NU", "ON", "PE", "QC", "SK", "YT" };
            //foreach(string state in arState)
            //{
            //    string _url = String.Format("http://www.chiropracticcanada.ca/en-us/find-a-chiropractor.aspx?searchType=2&province={0}&pageSize=0&page=1", state); ;
            //    List<ChiropractorCls> lstCP = DownloadHTMLData(_url);
            //    ExportToExcel(lstCP, state);
            //}
            //DownloadHTMLData(webBrowser1.Url.ToString());
      
        }

        private void button2_Click(object sender, EventArgs e)
        {
             

            String fileName = String.Empty;
            String FilePath = String.Empty;
            try
            {

                String emailBody = "Hi,<br>An Order Number :  ORD00212121221 has some error.<br/>Please do needful.<br> Thank You<br>Admin";

                // Create a new Smpt Client using Google's servers
                // var mailclient = new SmtpClient();
                String smtpHost = "mail.adeeva.com";//ForGmail
                Int32 smtpPort = 587; //ForGmail

                Boolean EnableSsl = false;//true;//ForGmail
                // Send Email
                string[] arrEmail = { "sunil@adeeva.com", "y.surendar@yahoo.in" };
                foreach (String item in arrEmail)
                {
                    String toAddress = item;
                    string msg  = new CommonMail().SendMailWithAttachment("admin@adeeva.com", toAddress.Trim().ToString(), "FTP Auto alert-" + DateTime.Now.ToShortDateString(), emailBody, "Admin Team", EnableSsl, true, FilePath, smtpHost, smtpPort, "System", null, fileName, true);

                    MessageBox.Show("Email has been sucessfully sent " + msg.ToString()); 
                }
            }
            catch (Exception ex)
            {


                MessageBox.Show("error " + ex.ToString());
                //System.IO.File.WriteAllText(MainDir + "Error" + DateTime.Now.Ticks + ".txt", "Error on email  Service" + ex.Message);
            }

        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            //Form1 f1 = new Form1();
           // f1.Visible = false;
            f2.Show();

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

      
   
    }
}
