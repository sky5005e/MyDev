using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class SchindlerProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"https://jobs.schindler.com/sap/bc/bsp/sap/y_quicksearch/search.htm?sap-language=DE&country=CH&FUNCAREA=&HIERLVL=";

        public string ProviderName
        {
            get { return "schindler"; }
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
                var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//table[@class='tablesorter quicksearch_result_tab']/tbody/tr");

                foreach (var item in parsedValues)
                {
                    var tdlist = item.SelectNodes(".//td");
                    int tdcount = tdlist.Count;
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;

                    var linkNode = item.SelectSingleNode(".//a");
                    Link = linkNode.GetAttributeValue("href", "").Trim();
                    Description = linkNode.InnerText.Trim();

                    Region = item.SelectNodes(".//td")[1].InnerText.Trim();

                    JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("{0}", Link)), Region = Region };
                    jobAds.Add(jobAdDto);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

    }

    
}

