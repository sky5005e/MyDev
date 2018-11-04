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
    public class CembraProvider
    {
         private HtmlDocument _htmlDoc = new HtmlDocument();
         private string _url = @"https://recruitingapp-2755.umantis.com/Jobs/1?lang=ger";

        public string ProviderName
        {
            get { return "cembra"; }
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
                    var splist = item.SelectNodes(".//span");
                    int spcount = splist.Count;
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;

                    var linkNode = item.SelectSingleNode(".//span/a");
                    Link = linkNode.GetAttributeValue("href", "").Trim();
                    Description = linkNode.InnerText.Trim();
                    string _region = item.SelectNodes(".//span")[7].InnerText.Trim().Replace("|","");
                    _region = HtmlToPlainText(_region);
                    Region = _region.Trim();

                    JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("https://recruitingapp-2755.umantis.com{0}", Link)), Region = Region };
                    jobAds.Add(jobAdDto);
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

    }

    
}
