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
    public class AlpiqProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://www.alpiq.ch/menschen-karriere/jobs/open-jobs.jsp?page60932=1&searchfor=&search=Suchen&country=Schweiz&region=&=&category=&experience=";// 
        
        public string ProviderName
        {
            get { return "alpiq"; }
        }

        public List<JobAdDto> GetJobAds()
        {
            List<JobAdDto> jobAds = new List<JobAdDto>();

            Int32 pgCount = GetPageCount(_url);

            for (int i = 1; i <= pgCount; i++)
            {
                String url = String.Format("http://www.alpiq.ch/menschen-karriere/jobs/open-jobs.jsp?page60932={0}&searchfor=&search=Suchen&country=Schweiz&region=&=&category=&experience=", i);
                jobAds.AddRange(GetJobAds(url));
            }

            return jobAds;
        }

        /// <summary>
        /// Get Total page with paging count
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public int GetPageCount(String url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument docMain = web.Load(url);
            string responseFromServer = docMain.DocumentNode.OuterHtml;
            _htmlDoc.LoadHtml(responseFromServer);
            var _parsedTotalcountLink = _htmlDoc.DocumentNode.SelectSingleNode("//div[@class='scroll']/p");
            string lastNum = _parsedTotalcountLink.InnerText.Split('|').LastOrDefault();
            var result = Regex.Match(lastNum, @"\d+").Value;
            int pgcount = Convert.ToInt32(result);
            return pgcount;
        }

     
        public IList<JobAdDto> GetJobAds(string url)
        {
            IList<JobAdDto> jobAds = new List<JobAdDto>();

            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument docMain = web.Load(url);
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

            var parsedValue = _htmlDoc.DocumentNode.SelectNodes("//table[@class='news']/tbody/tr");
            
            foreach (var item in parsedValue)
            {
                var tdlist = item.SelectNodes(".//td");
                int tdCount = tdlist.Count;
                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;

                var linkNode = item.SelectSingleNode(".//td/p[@class='title']/a");

                Link = linkNode.GetAttributeValue("href", "").Trim();

                Description = linkNode.InnerText.Trim();


                Region = item.SelectNodes(".//td")[tdCount-1].InnerText.Trim();
                JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("http://www.alpiq.ch/menschen-karriere/jobs/open-jobs.jsp{0}", Link)), Region = Region };
                jobAds.Add(jobAdDto);
            }


        }

    }

}


