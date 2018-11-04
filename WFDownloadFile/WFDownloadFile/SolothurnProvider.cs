using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Collections.Specialized;
using System.Net;
using System.Text.RegularExpressions;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class SolothurnProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://www.so.ch/departemente/finanzen/personalamt/stellenangebote/index.html";
        
        public string ProviderName
        {
            get { return "solothurn"; }
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

       
        /// <summary>
        /// Parse Html 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="jobAds"></param>
        private void ParseHtml(string html, IList<JobAdDto> jobAds)
        {

            _htmlDoc.LoadHtml(html);
            var parsedValue = _htmlDoc.DocumentNode.SelectNodes("//ul[@class='uploads']/li");

            foreach (var item in parsedValue)
            {
                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;
                var alinkNode = item.SelectSingleNode(".//div[@class='uploads_link']/a");
                Link = alinkNode.GetAttributeValue("href", "").Trim();
                Description = alinkNode.InnerText.Trim();
                Region = "Solothurn";

                JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("http://www.helveting.com{0}", Link)), Region = Region };
                jobAds.Add(jobAdDto);
            }

        }

    }

}

