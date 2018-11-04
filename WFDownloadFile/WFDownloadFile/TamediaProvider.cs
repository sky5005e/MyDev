using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class TamediaProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://tamedia.prospective.ch/?sprcd=en";// This is the Acctual URL

        public string ProviderName
        {
            get { return "tamedia"; }
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
                var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//div[@id='itemlist']/div");// 

                foreach (var item in parsedValues)
                {
                    var pvl = item.SelectNodes(".//div[@id='col_right']/p");
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;

                    var linkNode = pvl[0].SelectSingleNode(".//a");
                    
                    Description = pvl[0].InnerText.Trim();
                    String linkHref = linkNode.GetAttributeValue("onclick", "").Trim();
                    String r = linkHref.Split(new string[] { "('" }, StringSplitOptions.None)[1].Split(',')[0].Trim();
                    Link = r.Replace("'","");
                    Region = Regex.Match(pvl[1].InnerText.Trim(), @"Location: (.+?)\n").Groups[1].Value.Trim();
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

