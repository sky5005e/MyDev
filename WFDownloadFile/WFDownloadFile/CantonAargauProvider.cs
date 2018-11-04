using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

using System.Threading;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class CantonAargauProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"https://www.ag.ch/de/meta/jobs_personal_1/offene_stellen_2/verwaltung_1/kader/kader.jsp";

        public string ProviderName
        {
            get { return "cantonaargau"; }
        }

        public IList<JobAdDto> GetJobAds(String html)
        {
            IList<JobAdDto> jobAds = new List<JobAdDto>();
            ParseHtml(html, jobAds);

            // Do your Stuff;
            return jobAds;
        }

        public void RunBrowserThread()
        {
            Uri url = new Uri(_url);
            var th = new Thread(() =>
            {
                var br = new System.Windows.Forms.WebBrowser();
                br.DocumentCompleted += browser_DocumentCompleted;
                br.Navigate(url);
                Thread.Sleep(1000);
                System.Windows.Forms.Application.Run();
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        

        private void browser_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            var br = sender as System.Windows.Forms.WebBrowser;
            if (br.Url == e.Url)
            {
                GetJobAds(br.Document.Body.InnerHtml);
               System.Windows.Forms.Application.ExitThread();   // Stops the thread
            }
        }


       
        /// <summary>
        /// Parse Html 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="jobAds"></param>
        private void ParseHtml(string html, IList<JobAdDto> jobAds)
        {

            _htmlDoc.LoadHtml(html);
            var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//div[@class='inpagenav initialized']/ul/li/a");
            foreach (var item in parsedValues)
            {
                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;


                Link = item.GetAttributeValue("href", "").Trim();

                Description = item.InnerText.Trim();

                Region = String.Empty;

                JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("https://www.ag.ch{0}", Link)), Region = Region };
                jobAds.Add(jobAdDto);
            }
        }

    }

}

