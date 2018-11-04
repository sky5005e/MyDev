using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class SwissTPHProvider
    {
         private HtmlDocument _htmlDoc = new HtmlDocument();
         private string _url = @"https://recruitingapp-2698.umantis.com/Jobs/2?CompanyID=1&Reset=G&click_token=158633";// This is the Acctual URL
       
        public string ProviderName
        {
            get { return "swisstph"; }
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
                var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//div[@class='tableaslist_cell']");// 

                foreach (var item in parsedValues)
                {
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;

                    var linkNode = item.SelectSingleNode(".//a[@class='HSTableLinkSubTitle']");
                    Link = linkNode.GetAttributeValue("href", "").Trim();
                    Description = linkNode.InnerText;
                    var xregion = item.InnerText.Trim().Split('|').Where(s => s.Contains("Place of work")).FirstOrDefault();
                    String match = Regex.Match(xregion.Trim(), @"work: (.+?)&nbsp").Groups[1].Value;
                    Region = match.Trim();

                    jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("https://recruitingapp-2698.umantis.com/{0}", Link)), Region = Region });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

    }

    
}
