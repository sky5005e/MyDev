using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class HolcimProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"http://www2.unio.ch/cgi-bin/WebObjects/UNUserInterface.woa/wa/customPublicationMicroSite?unit=holcim&medium=holcim&skin=holcim";

        private string _nextPageURL = String.Empty;
        public string ProviderName
        {
            get { return "holcim"; }
        }

        public List<JobAdDto> GetJobAds()
        {
            List<JobAdDto> jobAds = new List<JobAdDto>();

            _nextPageURL = string.Format("http://www2.unio.ch{0}", GetPostURL());
            
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
        public string GetPostURL()
        {
            string submiturl = String.Empty;
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument docMain = web.Load(_url);
                string responseFromServer = docMain.DocumentNode.OuterHtml;
                _htmlDoc.LoadHtml(responseFromServer);
                var _parsedform = _htmlDoc.DocumentNode.SelectNodes("//form[@name='holcim_search_form']");
                submiturl = _parsedform[0].GetAttributeValue("action", "").Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
                
            return submiturl;
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
                String _outerHtml = _htmlDoc.DocumentNode.SelectNodes("//td[@class='content']/table")[1].OuterHtml;

                //HtmlAttribute htmlatt = link.Attributes["id"];
                String htmlData = _outerHtml;
                HtmlAgilityPack.HtmlDocument docSubHtml = new HtmlAgilityPack.HtmlDocument();
                docSubHtml.LoadHtml(htmlData);

                var parsedValues = docSubHtml.DocumentNode.SelectNodes("//tr[@class='NewsText']");



                foreach (var item in parsedValues)
                {
                    var tdlist = item.SelectNodes(".//td");
                    int tdcount = tdlist.Count;
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;

                    String linkHref = item.GetAttributeValue("onclick", "").Trim();
                    String r = linkHref.Split(new string[] { "('" }, StringSplitOptions.None)[1].Split(',')[0].Trim();
                    Link = r.Replace("'", "");

                    Description = item.SelectNodes(".//td")[0].InnerText.Trim();

                    Region = item.SelectNodes(".//td")[tdcount-1].InnerText.Trim();

                    jobAds.Add(new JobAdDto() { Description = Description, Source = new Uri(string.Format("{0}", Link)), Region = Region });
                }

                // for Continue page
                var nextURL = _htmlDoc.DocumentNode.SelectNodes("//span[@class='arrowRightLink']/a");
               if (nextURL != null)
                   _nextPageURL = string.Format("http://www2.unio.ch{0}", nextURL[0].GetAttributeValue("href", "").Trim());
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


