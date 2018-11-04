using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class EmilFreyProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://www.emilfrey.ch/de/ueber-uns/offene-stellen/";// This is the Acctual URL

        public string ProviderName
        {
            get { return "emilfrey"; }
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
            try
            {

                _htmlDoc.LoadHtml(html);
                var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//table[@class='ef-table tablesorter']/tbody/tr");// 

                foreach (var item in parsedValues)
                {
                    var tdlist = item.SelectNodes(".//td");
                    int tdcount = tdlist.Count;
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;

                    var linkNode = item.SelectSingleNode(".//td/a");
                    Link = linkNode.GetAttributeValue("href", "").Trim();
                    Description = item.SelectNodes(".//td")[2].InnerText.Trim();

                    Region = item.SelectNodes(".//td")[3].InnerText.Trim(); ;

                    jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("http://www.emilfrey.ch/{0}", Link)), Region = Region });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



    }

  
}
