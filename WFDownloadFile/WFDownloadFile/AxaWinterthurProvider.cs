using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class AxaWinterthurProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"https://www.axa-winterthur.ch/de/ueber-uns/jobs-karriere/Seiten/stellenangebote.aspx";
        //https://direktlink.prospective.ch/platform/2193/index.cfm?sprCd=de&wlgo=1&wlsearch_SD_M2193_70=1&seq=1

        public string ProviderName
        {
            get { return "axawinterthur"; }
        }

        public List<JobAdDto> GetJobAds()
        {
            List<JobAdDto> jobAds = new List<JobAdDto>();
            Int32 pgCount = GetPageCount("https://direktlink.prospective.ch/platform/2193/index.cfm?sprCd=de&wlgo=1&wlsearch_SD_M2193_70=1&seq=1");

            for (int i = 1; i <= pgCount; i++)
            {
                string url = String.Format("https://direktlink.prospective.ch/platform/2193/index.cfm?sprCd=de&wlgo=1&wlsearch_SD_M2193_70=1&seq={0}", i.ToString());
                jobAds.AddRange(GetJobAds(url));
            }

            return jobAds;
        }

        public int GetPageCount(String url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument docMain = web.Load(url);
            string responseFromServer = docMain.DocumentNode.OuterHtml;
            _htmlDoc.LoadHtml(responseFromServer);
            String _parsedTotalcount = _htmlDoc.DocumentNode.SelectNodes("//div[@id='recordcount']")[0].InnerText;
            string total = Regex.Match(_parsedTotalcount, @"\d+").Value;

            int pgcount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(total) / 5)));
            return pgcount;
        }
        public IList<JobAdDto> GetJobAds(string url)
        {
            IList<JobAdDto> jobAds = new List<JobAdDto>();

            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument docMain = web.Load(url);
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
                //here we will get two table  So selecting 1 index value
                String _outerHtml = _htmlDoc.DocumentNode.SelectNodes("//div[@id='itemlist']")[0].OuterHtml;

                String htmlData = _outerHtml;
                HtmlAgilityPack.HtmlDocument docSubHtml = new HtmlAgilityPack.HtmlDocument();
                docSubHtml.LoadHtml(htmlData);

                var parsedValues = docSubHtml.DocumentNode.SelectNodes("//tr[@class='divider']");
                foreach (var item in parsedValues)
                {
                    var tdlist = item.SelectNodes(".//td");
                    int tdcount = tdlist.Count;
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;
                    var linkNode = item.SelectNodes(".//td/a");
                    String linkHref = linkNode[0].GetAttributeValue("onclick", "").Trim();
                    String r = linkHref.Split(new string[] { "('" }, StringSplitOptions.None)[1].Split(',')[0].Trim();
                    Link = r.Replace("'", "");

                    String desc = item.SelectNodes(".//td")[0].InnerText.Trim();

                    if (desc.Contains("<!--"))
                        Description = desc.Remove(desc.IndexOf('<')).Trim();
                    else
                        Description = desc.Trim();

                    Region = item.SelectNodes(".//td")[tdcount-1].InnerText.Trim();

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



