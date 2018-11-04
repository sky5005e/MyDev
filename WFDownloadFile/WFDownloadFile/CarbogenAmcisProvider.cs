using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class CarbogenAmcisProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://www.carbogen-amcis.com/";

        public string ProviderName
        {
            get { return "carbogenamcis"; }
        }

        public List<JobAdDto> GetJobAds()
        {
            List<JobAdDto> jobAds = new List<JobAdDto>();

            foreach (var item in GetUrlList())
            {
                jobAds.AddRange(GetJobAds(String.Format(_url+item.Value), item.Key));
            }
            return jobAds;
        }

        public IList<JobAdDto> GetJobAds(string url, string loc)
        {
            IList<JobAdDto> jobAds = new List<JobAdDto>();

            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument docMain = web.Load(url);
            string responseFromServer = docMain.DocumentNode.OuterHtml;


            ParseHtml(responseFromServer, jobAds,loc);
            return jobAds;
        }

        private Dictionary<string, string> GetUrlList()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Aarau", "Aarau-careers.asp");
            dic.Add("Bubendorf", "hq-careers.asp");
            dic.Add("Hunzenschwil", "neuland-careers.asp");
            return dic;

        }
       
        /// <summary>
        /// Parse Html 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="jobAds"></param>
        private void ParseHtml(string html, IList<JobAdDto> jobAds, string loc)
        {

            _htmlDoc.LoadHtml(html);
            var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//div[@class='innertube']/div/div");
            if (parsedValues.Count > 0 && !parsedValues[0].InnerText.Contains("No Current Positions"))
            {
                foreach (var item in parsedValues)
                {
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;

                    var linkNode = item.SelectSingleNode(".//a");
                    string _link = linkNode.GetAttributeValue("onclick", "").Trim();
                    Link = Regex.Match(_link, @"\(([^)]*)\)").Groups[1].Value;
                    Link = Link.Replace("'", "");

                    Description = linkNode.InnerText.Trim();

                    Region = loc;

                    jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("http://www.carbogen-amcis.com/{0}", Link)), Region = Region });
                }
            }


        }

    }

}

