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
    public class InselSpitalBernProvider
    {
        
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://inselspital.prospective.ch/?sprCd=de";
        //http://www.insel.ch/de/jobs/inselstellen/";


        public string ProviderName
        {
            get { return "inselspitalbern"; }
        }

        public List<JobAdDto> GetJobAds()
        {
            List<JobAdDto> jobAds = new List<JobAdDto>();
            
            Int32 pgCount = GetPageCount(_url);

            for (int i = 1; i <= pgCount; i++)
            {
                String url = String.Format(@"http://inselspital.prospective.ch/index.cfm?sprCd=de&wlgo=1&seq={0}", i);

                jobAds.AddRange(GetJobAds(url));
            }

            return jobAds;
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
            int pgcount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(lastNum) / 10)));
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

            _htmlDoc.LoadHtml(html);

            var parsedValue = _htmlDoc.DocumentNode.SelectNodes("//div[@class='platform-item']");
            foreach (var item in parsedValue)
            {

                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;

                var linkNode = item.SelectSingleNode(".//div[@id='itemlist_jobtitle']/a");

                String linkHref = linkNode.GetAttributeValue("onclick", "").Trim();
                String r = linkHref.Split(new string[] { "('" }, StringSplitOptions.None)[1].Split(',')[0].Trim();
                Link = r.Replace("'", "");
                String _descp = linkNode.InnerText.Trim();
                Description =  _descp.Replace("\n","");
                Region = "Bern";


                JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("{0}", Link)), Region = Region };
                jobAds.Add(jobAdDto);
            }


        }


    }

}


