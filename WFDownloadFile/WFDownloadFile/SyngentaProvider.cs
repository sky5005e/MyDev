using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class SyngentaProvider
    {
        private HtmlAgilityPack.HtmlDocument _htmlDoc = new HtmlAgilityPack.HtmlDocument();
        private string _url = @"https://ssl1.peoplexs.com/Peoplexs22/CandidatesPortalNoLogin/Vacancies.cfm?PortalID=659&Match=18921&Trefwoord=&Sorteren=";// This is the Acctual URL

        public string ProviderName
        {
            get { return "syngenta"; }
        }

        int firstRun = 0;
        int pgMax = 0;
        int curPage = 1;
        private static bool completed = false;
        private static WebBrowser wb;
        [STAThread]
        public void Main()
        {
            wb = new WebBrowser();
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);
            wb.Navigate(_url);
            while (!completed)
            {
                Application.DoEvents();
                Thread.Sleep(5000);
                completed = false;
                //this.SetComboItem("pagebrowser", "2", wb);
            }
           
        }

        public void doMainOpt()
        {
            
            runBrowserThread(new Uri(_url));
        }

        private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //Console.WriteLine(wb.Document.Body.InnerHtml);
            GetJobAds(wb.Document.Body.InnerHtml);

            completed = true;
        }

        private void runBrowserThread(Uri url)
        {
            var th = new Thread(() =>
            {
                var br = new WebBrowser();
                br.DocumentCompleted += browser_DocumentCompleted;
                br.Navigate(url);
                Thread.Sleep(5000);
                Application.Run();
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var br = sender as WebBrowser;
            if (br.Url == e.Url)
            {
               // Console.WriteLine("Natigated to {0}", e.Url);
                String navurl = String.Format("Natigated to {0}", e.Url);
                if (!br.Document.Body.InnerHtml.Contains("Error"))
                {
                    GetJobAds(br.Document.Body.InnerHtml);
                    System.IO.File.WriteAllText(@"C:\Users\Sky\Desktop\ProFinal\FTP\WriteLines" + DateTime.Now.Ticks.ToString() + ".html", br.Document.Body.InnerHtml);
                    Thread.Sleep(1000);
                    //GetOptionValue(br);

                    while (curPage <= pgMax)
                    {
                        curPage = curPage + 1;
                        NextPageEvent(br);
                    }
                }
                else
                {
                    Thread.Sleep(10000);
                    doMainOpt();
                }
                Application.ExitThread();   // Stops the thread
            }
        }

        private void NextPageEvent(WebBrowser wb)
        {
            var slcEle = wb.Document.GetElementsByTagName("select");
            if (slcEle.Count == 1)
            {
                HtmlElement alink = null;
                if (firstRun != 0 && slcEle[0].Parent.Children.Count > 2)
                {
                     alink = slcEle[0].Parent.Children[2];
                     firstRun = 1;
                }
                else
                {
                    alink = slcEle[0].Parent.Children[1];
                }
                    //alink.InvokeMember("Click"); //++
                    Thread.Sleep(5000);
                    runBrowserThread(new Uri(String.Format("{0}", alink.GetAttribute("href").Trim())));
                
                
                //wb.DocumentCompleted += wb_DocumentCompleted;
            }
        }
        private void GetOptionValue(WebBrowser wb)
        {
            var slcEle= wb.Document.GetElementsByTagName("select");
            //foreach (HtmlElement link in slcEle)
            //{
                //if (link.GetAttribute("class") == "pagebrowser")// name Start1
                if(slcEle.Count == 1)
                {
                    //do something
                    this.SetComboItem(slcEle[0] as System.Windows.Forms.HtmlElement, "2");
              
                }
            //}
        }
        public void SetComboItem(System.Windows.Forms.HtmlElement selectElement, string value)
        {
            System.Windows.Forms.HtmlElement ee = selectElement;
            foreach (HtmlElement item in ee.Children)
            {
                //if (item.OuterHtml.ToLower().IndexOf(value.ToLower()) >= 0)
                if (item.InnerText.Trim() == value)
                {
                    item.SetAttribute("selected", "");
                    item.InvokeMember("onChange");
                    break;
                }
                //else
                //{
                //    item.SetAttribute("selected", "");
                //}
            }

        }

        public IList<JobAdDto> GetJobAds(String html)
        {
            IList<JobAdDto> jobAds = new List<JobAdDto>();


            ParseHtml(html, jobAds);
            return jobAds;
        }

        public IList<JobAdDto> GetJobAds()
        {
            IList<JobAdDto> jobAds = new List<JobAdDto>();

            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument docMain = web.Load(_url);
            string responseFromServer = docMain.DocumentNode.OuterHtml;


            ParseHtml(responseFromServer, jobAds);
            return jobAds;
        }

        private void ParsePaging(HtmlAgilityPack.HtmlDocument _htmlDoc)
        {
            var slMaxValue = _htmlDoc.DocumentNode.SelectNodes("//select[@class='pagebrowser']");
            String _pgMax = slMaxValue[0].InnerText;
            pgMax = Convert.ToInt32(_pgMax.Split(' ').LastOrDefault());
            var pageList =  _htmlDoc.DocumentNode.SelectNodes("//select[@class='pagebrowser']//option");

            foreach (HtmlNode node in pageList)
            {

            }
        }
        /// <summary>
        /// Parse Html 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="jobAds"></param>
        private void ParseHtml(string html, IList<JobAdDto> jobAds)
        {
            try
            {

                _htmlDoc.LoadHtml(html);

                #region Parse Paging Logic
                ParsePaging(_htmlDoc);
                #endregion
                #region for Main Parse
                var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//table[@class='vacancies']/tbody/tr").Skip(1);


                foreach (var item in parsedValues)
                {
                    var tdlist = item.SelectNodes(".//td");
                    int tdcount = tdlist.Count;
                    if (tdcount >= 2)
                    {
                        string Description = String.Empty;
                        string Link = String.Empty;
                        string Region = String.Empty;

                        var linkNode = item.SelectSingleNode(".//td/a");
                        Link = linkNode.GetAttributeValue("href", "").Trim();
                        Description = item.SelectNodes(".//td")[0].InnerText.Trim();

                        Region = item.SelectNodes(".//td")[1].InnerText.Trim(); ;

                        jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("https://ssl1.peoplexs.com/Peoplexs22/{0}", Link)), Region = Region });
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}


