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
    public class GlencoreProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"https://glencorexstratajobs.nga.net.au/cp/index.cfm?lid=22106030001&CurATC=Search&CurBID=ea5bb92a-2b41-4cfc-b248-9db401357a6d";

        public string ProviderName
        {
            get { return "glencore"; }
        }

        public List<JobAdDto> GetJobAds()
        {
            List<JobAdDto> jobAds = new List<JobAdDto>();

            Int32 pgCount = GetPageCount(_url);

            for (int i = 1; i <= pgCount; i++)
            {
                jobAds.AddRange(GetJobAds(Convert.ToString(i)));
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
            var _parsedTotalcount = _htmlDoc.DocumentNode.SelectNodes("//div[@class='cp_pagination']/p/strong");
            int spcount = _parsedTotalcount.Count;
            string total = _parsedTotalcount[spcount - 1].InnerText; 

            int pgcount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(total) / 20)));
            return pgcount;
        }

        private IList<JobAdDto> GetJobAds(string currentPage)
        {
            IList<JobAdDto> jobAds = new List<JobAdDto>();
            string responseFromServer = String.Empty;
            NameValueCollection reqparm = new NameValueCollection();
            reqparm.Add("jobspage", currentPage);
            using (WebClient client = new WebClient())
            {
                
                byte[] responsebytes = client.UploadValues(new Uri(_url), "POST", reqparm);
                responseFromServer = Encoding.UTF8.GetString(responsebytes);

            }
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
            
            var parseLocTag = _htmlDoc.DocumentNode.SelectNodes("//td[@class='cp_jobDetails']").Where(s => s.InnerText.Contains("Switzerland")).ToList();
            foreach (var item in parseLocTag)
            {

                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;

                var linkNode = item.SelectSingleNode(".//a[@class='cp_jobListJobTitle']");
                Link = linkNode.GetAttributeValue("href", "").Trim();
                Description = linkNode.InnerText;
                var locNode = item.SelectNodes(".//ul/li").Where(l=>l.InnerText.Contains("Location")).FirstOrDefault();
                string _region = locNode.InnerText.Trim();
                Region = _region.Replace("Location -", "");

                jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("https://glencorexstratajobs.nga.net.au/cp/{0}", Link)), Region = Region });
            }


        }

    }

}

