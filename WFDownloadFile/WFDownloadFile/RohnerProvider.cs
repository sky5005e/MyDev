using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class RohnerProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://www.rohnerchem.ch/about_us/jobs.php";

        public string ProviderName
        {
            get { return "rohner"; }
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
            var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//div[@id='jobs']/p");

            foreach (var item in parsedValues)
            {
                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;

                var linkNode = item.SelectSingleNode(".//a");
                Link = linkNode.GetAttributeValue("href", "").Trim();

                Description = linkNode.InnerText.Trim();

                Region = "Pratteln, Switzerland";

                jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("http://www.rohnerchem.ch/{0}", Link)), Region = Region });
            }


        }

    }

}

