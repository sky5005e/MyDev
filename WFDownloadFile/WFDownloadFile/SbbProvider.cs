using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Collections.Specialized;
using System.Net;
using System.Text.RegularExpressions;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class SbbProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://sbb2.prospective.ch/?sprCd=de";// 
        
        public string ProviderName
        {
            get { return "sbb"; }
        }

        public List<JobAdDto> GetJobAds()
        {
            List<JobAdDto> jobAds = new List<JobAdDto>();

            Int32 pgCount = GetPageCount(_url);

            for (int i = 1; i <= pgCount; i++)
            {
                String url = String.Format("http://sbb2.prospective.ch/index.cfm?sprCd=de&wlgo=1&wlsearch_SD_M2185_70=1&wlsearch_SD_M2185_80=1&seq={0}#up", i);
                jobAds.AddRange(GetJobAds(url));
            }

            return jobAds;
        }

        /// <summary>
        /// Get Total page with paging count
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public int GetPageCount(String url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument docMain = web.Load(url);
            string responseFromServer = docMain.DocumentNode.OuterHtml;
            _htmlDoc.LoadHtml(responseFromServer);
            var _parsedTotalcountLink = _htmlDoc.DocumentNode.SelectSingleNode("//td/a[@class='backfor-more-more']");
            string _lastUrl = _parsedTotalcountLink.GetAttributeValue("href", "").Trim();
            string tobesearched = "seq=";
            string lastNum = _lastUrl.Substring(_lastUrl.IndexOf(tobesearched) + tobesearched.Length);
            lastNum = lastNum.Replace("#up", "");
            int pgcount = Convert.ToInt32(lastNum);
            return pgcount;
        }

        private static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
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

            _htmlDoc.LoadHtml(html);

            var parsedValue = _htmlDoc.DocumentNode.SelectNodes("//div[@class='platform-item']");
            //div[@id='itemlist_jobtitle']");
            foreach (var item in parsedValue)
            {

                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;

                var linkNode = item.SelectSingleNode(".//div[@id='title']/a");

                String linkHref = linkNode.GetAttributeValue("onclick", "").Trim();
                String r = linkHref.Split(new string[] { "('" }, StringSplitOptions.None)[1].Split(',')[0].Trim();
                Link = r.Replace("'", "");

                Description = linkNode.InnerText.Trim();
                var _regionNode = item.SelectSingleNode(".//div[@id='col_2']");
                Region =HtmlToPlainText(_regionNode.InnerText.Trim());
                JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("{0}", Link)), Region = Region };
                jobAds.Add(jobAdDto);
            }


        }

    }

}


