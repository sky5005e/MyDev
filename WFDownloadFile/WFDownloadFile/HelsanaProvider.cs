using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class HelsanaProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"https://internet.refline.ch/418945/search.html?form.buttons.listAll=1&target=overview&lang=de";

        public string ProviderName
        {
            get { return "helsana"; }
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
                var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//table[@class='searchResult']/tbody/tr").Skip(1);

                foreach (var item in parsedValues)
                {
                    var tdlist = item.SelectNodes(".//td");
                    int tdcount = tdlist.Count;
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;

                    var linkNode = item.SelectSingleNode(".//td/a");
                    Link = linkNode.GetAttributeValue("href", "").Trim();
                    Description = item.SelectNodes(".//td")[0].InnerText.Trim();

                    Region = item.SelectNodes(".//td")[2].InnerText.Trim();

                    jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("{0}", Link)), Region = Region });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

    }

    
}

