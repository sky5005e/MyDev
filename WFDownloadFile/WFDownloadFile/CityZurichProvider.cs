using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Collections.Specialized;
using System.Net;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class CityZurichProvider
    {
         private HtmlDocument _htmlDoc = new HtmlDocument();
         private string _url = @"https://e-gov.stadt-zuerich.ch/jobsuche/jobsuche.xhtml?filter=aus&url=";

        public string ProviderName
        {
            get { return "cityzurich"; }
        }

        public IList<JobAdDto> GetJobAds()
        {
            
            IList<JobAdDto> jobAds = new List<JobAdDto>();
            try
            {
                string responseFromServer = String.Empty;
                NameValueCollection reqparm = new NameValueCollection();
                reqparm.Add("j_idt22:ds_ds_3", "3");
                using (WebClient client = new WebClient())
                {
                    System.Net.ServicePointManager.Expect100Continue = false;
                    byte[] responsebytes = client.UploadValues(new Uri(_url), "POST", reqparm);
                    responseFromServer = Encoding.UTF8.GetString(responsebytes);

                }
                ParseHtml(responseFromServer, jobAds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                var parsedValues = _htmlDoc.DocumentNode.SelectNodes("//div[@class='search_results_collapsed']/a").Where(s => s.InnerText.Trim().Contains("Schweiz"));// 

                foreach (var item in parsedValues)
                {
                    string Region = String.Empty;
                    string Link = String.Empty;
                    string Description = String.Empty;

                    String _region = item.SelectSingleNode(".//span").InnerText.Trim();

                    Link = item.GetAttributeValue("href", "").Trim();
                    Description = item.InnerText.Trim().Replace(_region, "");

                    _region = _region.Split(',').FirstOrDefault();
                    Region = _region;

                    JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("http://www.stadlerrail.com{0}", Link)), Region = Region };
                    jobAds.Add(jobAdDto);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    
}
