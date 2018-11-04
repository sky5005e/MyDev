﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Collections.Specialized;
using System.Net;
using System.Text.RegularExpressions;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class AxpoProvider
    {
        
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"https://recruitingapp-2692.umantis.com/Jobs/1?DesignID=10002&lang=ger";
        //http://www.axpo.com/axpo/ch/de/axpo-jobs/offene-stellen.html";


        public string ProviderName
        {
            get { return "axpo"; }
        }

        public List<JobAdDto> GetJobAds()
        {
            List<JobAdDto> jobAds = new List<JobAdDto>();
            
            Int32 pgCount = GetPageCount(_url);

            for (int i = 1; i <= pgCount; i++)
            {
                String url = String.Format(@"https://recruitingapp-2692.umantis.com/Jobs/1?tc66856=p{0}&_search_token66856=1218702275#connectortable_66856&token=tk", i);

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
            var _parsedTotalcountLink = _htmlDoc.DocumentNode.SelectSingleNode("//td[@class='HSTableBottomNavigation']");
            string totallbl = HtmlToPlainText(_parsedTotalcountLink.InnerText.Trim());
            totallbl = totallbl.Trim();
            string tobesearched = "von";
            string lastNum = totallbl.Substring(totallbl.IndexOf(tobesearched) + tobesearched.Length).Trim();
            int total = Convert.ToInt32(lastNum);
            int pgcount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(total) / 10)));
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

            var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//div[@class='tableAsListCell']");
            foreach (var item in parsedValues)
            {

                var splist = item.SelectNodes(".//span");
                int spcount = splist.Count;
                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;

                var linkNode = item.SelectSingleNode(".//a[@class='HSTableLinkSubTitle']");
                Link = linkNode.GetAttributeValue("href", "").Trim();
                Description = linkNode.InnerText.Trim();

                Region = item.SelectNodes(".//span")[2].InnerText.Trim();

                JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("https://recruitingapp-2692.umantis.com{0}", Link)), Region = Region };
                jobAds.Add(jobAdDto);
               
            }


        }

    }

}

