using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class ThyssenKruppProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://karriere.thyssenkrupp.com//de/karriere/stellenboerse.html?sort=ValidFromDate&dir=DESC&searchid=2626a3fa139466c8d6ea2fe0c62ac81e&search=1&prevperpage=20&perpage=100&page=3";

        public string ProviderName
        {
            get { return "thyssenkrupp"; }
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
            var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//table[@id='resulttable']/tbody/tr");

            foreach (var item in parsedValues)
            {
                var tdlist = item.SelectNodes(".//td");
                int tdcount = tdlist.Count;
                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;

                var linkNode = item.SelectSingleNode(".//td/a");
                Link = linkNode.GetAttributeValue("href", "").Trim();
                Description = item.SelectNodes(".//td")[1].InnerText.Trim();

                Region = item.SelectNodes(".//td")[3].InnerText.Trim(); ;

                jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("http://karriere.thyssenkrupp.com/{0}", Link)), Region = Region });
            }


        }

    }

}

