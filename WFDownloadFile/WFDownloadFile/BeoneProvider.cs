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
    public class BeoneProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://www.beone-group.com/stellenprofile.html";

        public string ProviderName
        {
            get { return "beone"; }
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

            var parseSecTag = _htmlDoc.DocumentNode.SelectNodes("//section[@class='ce_accordionStart ce_accordion block']").Where(s => s.InnerText.Contains("Schweiz")).ToList();
            
            String htmlData = parseSecTag[0].InnerHtml;
            HtmlAgilityPack.HtmlDocument docSubHtml = new HtmlAgilityPack.HtmlDocument();
            docSubHtml.LoadHtml(htmlData);
            var parsedDiv = docSubHtml.DocumentNode.SelectNodes(".//div/a");

            foreach (var item in parsedDiv)
            {

                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;

                Link = item.GetAttributeValue("href", "").Trim();
                Description = item.InnerText.Trim();
                Region = "Schweiz";

                jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("http://www.beone-group.com/{0}", Link)), Region = Region });
            }


        }

    }

}

