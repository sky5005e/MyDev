using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class HelblingProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://kombi.jobs.ch/basic_kombi/?kid=helbling";// sub URL
        //http://www.helbling.ch/hol/jobs/jobs-helbling";

        public string ProviderName
        {
            get { return "helbling"; }
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
                var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//div[@id='jobs_row']");

                foreach (var item in parsedValues)
                {
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;

                    var linkNode = item.SelectSingleNode(".//a[@class='jobs']");
                    Link = linkNode.GetAttributeValue("href", "").Trim();
                    Description = linkNode.InnerText.Trim();

                    Region = "Schweiz";

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

