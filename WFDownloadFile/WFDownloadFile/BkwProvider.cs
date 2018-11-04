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
    public class BkwProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://bkw.prospective.ch/index.cfm?sprCd=de";// 
        //http://www.bkw.ch/offene-stellen.html

        public string ProviderName
        {
            get { return "bkw"; }
        }

        public List<JobAdDto> GetJobAds()
        {
            List<JobAdDto> jobAds = new List<JobAdDto>();

            Int32 pgCount = GetPageCount(_url);

            for (int i = 1; i <= pgCount; i++)
            {
                String url = String.Format("http://bkw.prospective.ch/index.cfm?sprCd=de&wlgo=1&seq={0}", i);
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
            var _parsedTotalcountLink = _htmlDoc.DocumentNode.SelectSingleNode("//a[@class='backfor-more-more']");
            string _lastUrl = _parsedTotalcountLink.GetAttributeValue("href", "").Trim();
            string tobesearched = "seq=";
            string lastNum = _lastUrl.Substring(_lastUrl.IndexOf(tobesearched) + tobesearched.Length); 
            int total = Convert.ToInt32(lastNum);
            int pgcount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(total) / 10)));
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

            var parsedValue = _htmlDoc.DocumentNode.SelectNodes("//div[@class='platform-item']");
            //div[@id='itemlist_jobtitle']");
            foreach (var item in parsedValue)
            {

                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;

                var linkNode = item.SelectSingleNode(".//div[@id='itemlist_jobtitle']/a");

                String linkHref = linkNode.GetAttributeValue("onclick", "").Trim();
                String r = linkHref.Split(new string[] { "('" }, StringSplitOptions.None)[1].Split(',')[0].Trim();
                Link = r.Replace("'", "");
               
                Description = linkNode.InnerText;


                var regNode = item.SelectSingleNode(".//div[@id='itemlist_arbeitsort']");

                string _region = regNode.InnerText.Trim();
                Region = _region.Replace("Arbeitsort: ", "");

                jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("{0}", Link)), Region = Region });
            }


        }

    }

}

