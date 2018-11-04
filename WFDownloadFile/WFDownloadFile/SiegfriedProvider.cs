using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class SiegfriedProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://www.siegfried.ch/about-siegfried/jobscareer/jobs/";

        public string ProviderName
        {
            get { return "siegfried"; }
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
            // find the Switzerland jobs
            var pTag = _htmlDoc.DocumentNode.SelectNodes("//div[@class='csc-default']/p").Where(s => s.InnerText.Contains("Switzerland")).FirstOrDefault();
            String _innerHtml = pTag.ParentNode.InnerHtml;

            String htmlData = _innerHtml;
            HtmlAgilityPack.HtmlDocument docSubHtml = new HtmlAgilityPack.HtmlDocument();
            docSubHtml.LoadHtml(htmlData);

            var parsedValues = docSubHtml.DocumentNode.SelectNodes("//p[@class='withOutMargin']").Skip(1);

            foreach (var item in parsedValues)
            {
                var linkNode = item.SelectNodes(".//a");

                foreach (var aitem in linkNode)
                {
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;

                    Link = aitem.GetAttributeValue("href", "").Trim();

                    Description = aitem.InnerText.Trim().Replace("&gt;","");

                    Region = "Zofingen, Switzerland";

                    jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("http://www.siegfried.ch/{0}", Link)), Region = Region });
                }
            }


        }

    }

}

