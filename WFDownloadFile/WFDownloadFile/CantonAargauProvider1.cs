using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class CantonAargauProvider1
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"https://www.ag.ch/";

        public string ProviderName
        {
            get { return "cantonaargau"; }
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

            ParseHtml(responseFromServer, jobAds, loc);
            return jobAds;
        }

        private Dictionary<string, string> GetUrlList()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Aarau", "de/meta/jobs_personal_1/offene_stellen_2/verwaltung_1/kader/kader.jsp");
            dic.Add("Bubendorf", "de/meta/jobs_personal_1/offene_stellen_2/verwaltung_1/administration___finanzen___personal___informatik/administration.jsp");
            dic.Add("Hunzenschwil", "de/meta/jobs_personal_1/offene_stellen_2/verwaltung_1/bau___verkehr___umwelt_/bau.jsp");
            dic.Add("Aarau", "de/meta/jobs_personal_1/offene_stellen_2/verwaltung_1/bildung___paedagogik___soziales___gesundheit/bildung.jsp");
            dic.Add("Bubendorf", "de/meta/jobs_personal_1/offene_stellen_2/verwaltung_1/handwerk___technik___hauswirtschaft/handwerk.jsp");
            dic.Add("Hunzenschwil", "de/meta/jobs_personal_1/offene_stellen_2/verwaltung_1/recht___justiz___sicherheit/recht.jsp");
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
            var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//span[@id='publicationsTable']/table/tbody/tr");
            foreach (var item in parsedValues)
            {
                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;

                var linkNode = item.SelectSingleNode(".//a[@class='ajaxlightbox']");
                Link = linkNode.GetAttributeValue("href", "").Trim();

                Description = linkNode.InnerText.Trim();

                Region = "";

                JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("https://www.ag.ch{0}", Link)), Region = Region };
                jobAds.Add(jobAdDto);
            }
        }

    }

}

