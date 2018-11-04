using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class DeloitteProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://careers.deloitte.com/jobs/eng-ch/results/c/Switzerland/n/100000";

        public string ProviderName
        {
            get { return "deloitte"; }
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
            var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//table[@class='datagrid']/tr").Skip(1);

            foreach (var item in parsedValues)
            {
                var tdlist = item.SelectNodes(".//td");
                int tdcount = tdlist.Count;
                // Here I have used static value for index ie 2
                Boolean _hasMultipleLocations = item.SelectNodes(".//td")[2].OuterHtml.Contains("Multiple Locations");
                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;

                var linkNode = item.SelectSingleNode(".//td/a");
                Link = linkNode.GetAttributeValue("href", "").Trim();
                Description = item.SelectNodes(".//td")[0].InnerText.Trim();

                if (_hasMultipleLocations)
                {
                    String htmlData = String.Empty;
                    var nodeList = item.SelectNodes(".//td/div/table/tr").ToList();
                    int countnode = nodeList.Count;
                    for (int i = 0; i < countnode; i++)
                    {
                        if (!String.IsNullOrEmpty(Region))
                            Region = Region + "," + nodeList[i].SelectNodes(".//td")[0].InnerText.Trim();
                        else
                            Region = nodeList[i].SelectNodes(".//td")[0].InnerText.Trim();
                    }
                }
                else
                    Region = item.SelectNodes(".//td")[2].InnerText.Trim(); ;

                jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("https://careers.deloitte.com/{0}", Link)), Region = Region });
            }


        }

    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public class JobAdDto
    //{
    //    public string Description {get; set;}
    //    public Uri Source { get ; set;}
    //    public string Region { get;set;} 
    //}
}
