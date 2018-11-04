
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class DsmProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://www.dsm.com/talentlink-portal-component/en_US/cworld/index.do?action=getFilteredRequisitions&continents=&countries=CH&areaOfInterest=&educationLevel=&jobType=&nextItem=0&prevItem=0&pageAction=next&noOfItems=10000&showPrev=no&showNext=no";

        public string ProviderName
        {
            get { return "dsm"; }
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
            var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//div[@id='jobresults']/div").Skip(1);

            foreach (var item in parsedValues)
            {
                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;

                var linkNode = item.SelectSingleNode(".//h3/a");
                Link = linkNode.GetAttributeValue("href", "").Trim();

                Description = linkNode.InnerText.Trim();

                Region = item.SelectNodes(".//p/span[@class='loc']")[0].InnerText.Trim(); 

                jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("http://dsm.com/{0}", Link)), Region = Region });
            }


        }

    }

}

