using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class EthProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"https://internet.refline.ch/845721/search.html?form.buttons.listAll=1";// This is the Acctual URL

        public string ProviderName
        {
            get { return "Eth"; }
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
                var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//table[@class='jquery-tablesorter']/tbody/tr");// 

                foreach (var item in parsedValues)
                {
                    var tdlist = item.SelectNodes(".//td");
                    int tdcount = tdlist.Count;
                    string Department = String.Empty;
                    string Link = String.Empty;
                    string Position = String.Empty;
                    string Workload = String.Empty;

                    var linkNode = item.SelectSingleNode(".//td/a");
                    Link = linkNode.GetAttributeValue("href", "").Trim();
                    Position = item.SelectNodes(".//td")[0].InnerText.Trim();
                    Workload = item.SelectNodes(".//td")[1].InnerText.Trim();

                    Department = item.SelectNodes(".//td")[2].InnerText.Trim(); ;

                    jobAds.Add(new JobAdDto() { Position = Position, Source = new Uri(string.Format("{0}", Link)), Department = Department, Workload = Workload });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        /// <summary>
        /// 
        /// </summary>
        public class JobAdDto
        {
            public string Position { get; set; }
            public Uri Source { get; set; }
            public string Workload { get; set; }
            public string Department { get; set; }
        }
    }
}

    