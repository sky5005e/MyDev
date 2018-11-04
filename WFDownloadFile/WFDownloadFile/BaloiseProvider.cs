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
    public class BaloiseProvider
    {
        
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"https://jobs.baloise.com/de/search/result?hideBorders=true&hideFilter=false&hideTeaser=true&listView=false";
        //https://www.baloise.ch/de/unser-unternehmen/karriere/offene-stellen.html;


        public string ProviderName
        {
            get { return "baloise"; }
        }

        public List<JobAdDto> GetJobAds()
        {
            List<JobAdDto> jobAds = new List<JobAdDto>();
            
            Int32 pgCount = GetPageCount(_url);

            for (int i = 1; i <= pgCount; i++)
            {
                String url = String.Format(@"https://jobs.baloise.com/de/search/result/hideBorders/false/hideFilter/false/hideTeaser/true/listView/false/sort/date/direction/DESC/page/{0}/matchesPerPage/false/showAllVacanciesLink/false#currentJobs",i);

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
            string totlalbl = _htmlDoc.DocumentNode.SelectSingleNode("//span[@class='page']").InnerText.Trim();
            string tobesearched = "von ";
            string lastNum = totlalbl.Substring(totlalbl.IndexOf(tobesearched) + tobesearched.Length).Trim();
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

            var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//table[@class='standard']/tr").Skip(1);
            foreach (var item in parsedValues)
            {

                var tdlist = item.SelectNodes(".//td");
                int tdcount = tdlist.Count;
                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;

                var linkNode = item.SelectSingleNode(".//td/a[@class='jobLink']");
                Link = linkNode.GetAttributeValue("href", "").Trim();
                Description = linkNode.InnerText.Trim();

                Region = item.SelectNodes(".//td")[2].InnerText.Trim();


                jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("https://jobs.baloise.com/{0}", Link)), Region = Region });
            }


        }

    }

}

