
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
    public class SauterProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://kombi.jobs.ch/basic_kombi/?kid=sauter_gruppe";// Child URL
        //http://www.sauter-controls.com/jobs-karriere-sauter/offene-stellen-sauter.html // Main URL

        public string ProviderName
        {
            get { return "sauter"; }
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

            var parsedValue = _htmlDoc.DocumentNode.SelectNodes("//div[@class='jobs jobs_ad_link']");

            foreach (var item in parsedValue)
            {

                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;
                var linkNode = item.SelectSingleNode(".//a[@class='jobs']");
                Link = linkNode.GetAttributeValue("href", "").Trim();
                Description = linkNode.InnerText.Trim();
                Region = "Basel";

                JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("{0}", Link)), Region = Region };
                jobAds.Add(jobAdDto);
            
            }


        }

    }

}

