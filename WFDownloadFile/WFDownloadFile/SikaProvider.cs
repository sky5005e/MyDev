using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Collections.Specialized;
using System.Net;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class SikaProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://www20.i-grasp.com/fe/tpl_Actelion04.asp?KEY=86403995&C=588298355614&PAGESTAMP=sedtpzvjseximlxgad&nexts=INIT_JOBLISTSTART&nextss=&mode=1&newQuery=yes&searchlocation=5265&searchsublocation=0&searchjobgenerallist2id=0&searchtext=&formsubmit4=Search+and+apply";

        private string _nextPageURL = String.Empty;
        public string ProviderName
        {
            get { return "sika"; }
        }

        public List<JobAdDto> GetJobAds()
        {
            List<JobAdDto> jobAds = new List<JobAdDto>();

            _nextPageURL = _url;
            while (!String.IsNullOrEmpty(_nextPageURL))
            {
                jobAds.AddRange(GetJobAds(_nextPageURL));
            }



            return jobAds;
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
                var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//table[@id='searchresultslist']/tbody/tr").Where(s => s.InnerText.Trim().Contains("Switzerland")).ToList();
                foreach (var item in parsedValues)
                {
                    var tdlist = item.SelectNodes(".//td");
                    int tdcount = tdlist.Count;
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;

                    var linkNode = item.SelectSingleNode(".//td/a");
                    Link = linkNode.GetAttributeValue("href", "").Trim();
                    Description = linkNode.InnerText.Trim();
                    Region = item.SelectNodes(".//td")[2].InnerText.Trim() + ", " + item.SelectNodes(".//td")[1].InnerText.Trim();

                    jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("http://www20.i-grasp.com/fe/{0}", Link)), Region = Region });
                }

                // for Continue page
                var nextURL = _htmlDoc.DocumentNode.SelectSingleNode("//a[@class='nextbullet']");
                if (nextURL != null)
                {
                    Uri uri = new Uri(string.Format("http://www20.i-grasp.com/fe/{0}", nextURL.GetAttributeValue("href", "").Trim()));
                    _nextPageURL = uri.ToString();
                }
                else
                    _nextPageURL = String.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

    }

    
}


