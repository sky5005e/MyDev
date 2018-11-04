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
    public class PwcProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://www.pwc.ch/de/dyn_output.html?content.void=41460&objects.job[page]=0&cache=refreshurl";// 
        
        public string ProviderName
        {
            get { return "pwc"; }
        }

        public List<JobAdDto> GetJobAds()
        {
            List<JobAdDto> jobAds = new List<JobAdDto>();

            Int32 pgCount = GetPageCount(_url);

            for (int i = 0; i < pgCount; i++)
            {
                String url = String.Format("http://www.pwc.ch/de/dyn_output.html?content.void=41460&objects.job[page]={0}&cache=refreshurl", i);
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
            var _parsedtab = _htmlDoc.DocumentNode.SelectNodes("//div[@id='center']/table");
            var  _parsedTotalcountLink =  _parsedtab[1].SelectNodes( ".//tr/td").FirstOrDefault();
            string totallbl = _parsedTotalcountLink.InnerText.Trim();
           
            string tobesearched = "von";
            string lastNum = totallbl.Substring(totallbl.IndexOf(tobesearched) + tobesearched.Length).Trim();
            int total = Convert.ToInt32(lastNum);
            int pgcount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(total) / 20)));
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

            var parsedValue = _htmlDoc.DocumentNode.SelectNodes("//div[@id='center']/table")[1].SelectNodes(".//tbody/tr").Skip(2);

            int trCount = parsedValue.Count();
            int curcount = 1;// Starting from 1
            foreach (var item in parsedValue)
            {
                if (curcount < trCount)
                {
                    var tdlist = item.SelectNodes(".//td");
                    int tdCount = tdlist.Count;
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;

                    var linkNode = item.SelectSingleNode(".//td[@class='vacancies']/a");

                    Link = linkNode.GetAttributeValue("href", "").Trim();

                    Description = linkNode.InnerText.Trim();


                    Region = item.SelectNodes(".//td")[3].InnerText.Trim();
                    JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("http://www.pwc.ch/de/{0}", Link)), Region = Region };
                    jobAds.Add(jobAdDto);
                    curcount = curcount + 1;
                }
            }


        }

    }

}


