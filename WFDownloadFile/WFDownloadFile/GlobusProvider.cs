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
    public class GlobusProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://globus.prospective.ch/index.cfm?sprCd=de";// 
        
        public string ProviderName
        {
            get { return "globus"; }
        }

        public List<JobAdDto> GetJobAds()
        {
            List<JobAdDto> jobAds = new List<JobAdDto>();

            Int32 pgCount = GetPageCount(_url);

            for (int i = 1; i <= pgCount; i++)
            {
                String url = String.Format("http://globus.prospective.ch/index.cfm?sprCd=de&wlgo=1&seq={0}#gototop", i);
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
            var _parsedTotalcountLink = _htmlDoc.DocumentNode.SelectSingleNode("//td/a[@class='backfor-more-more']");
            string _lastUrl = _parsedTotalcountLink.GetAttributeValue("href", "").Trim();
            string tobesearched = "seq=";
            string lastNum = _lastUrl.Substring(_lastUrl.IndexOf(tobesearched) + tobesearched.Length);
            lastNum = lastNum.Replace("#gototop", "");
            int pgcount = Convert.ToInt32(lastNum);
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

                var linkNode = item.SelectSingleNode(".//div[@class='stellentitel']/a");

                String linkHref = linkNode.GetAttributeValue("onclick", "").Trim();
                String r = linkHref.Split(new string[] { "('" }, StringSplitOptions.None)[1].Split(',')[0].Trim();
                Link = r.Replace("'", "");

                Description = linkNode.InnerText.Trim();
                var _regionNode = item.SelectSingleNode(".//div[@class='arbeitsort']");
                Region = _regionNode.InnerText.Trim();
                JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("{0}", Link)), Region = Region };
                jobAds.Add(jobAdDto);
            }


        }

    }

}

