using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Xml;
using System.ServiceModel.Syndication;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class MicrosoftProvider
    {
        private HtmlDocument _htmlDoc = new HtmlDocument();
        private string _url = @"https://careers.microsoft.com/Feed/Search.ashx?ss=&jc=all&pr=all&dv=all&ct=all&rg=CH&lang=en";

        public string ProviderName
        {
            get { return "microsoft"; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<JobAdDto> GetJobAds()
        {

            IList<JobAdDto> jobAds = new List<JobAdDto>();
            XmlReader reader = XmlReader.Create(_url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            foreach (SyndicationItem item in feed.Items)
            {
                string Region = String.Empty;
                string Link = String.Empty;
                string Description = String.Empty;
                Link = item.Links[0].Uri.AbsoluteUri;
                Description = item.Title.Text;
                Region = "Wall is ellen";

                JobAdDto jobAdDto = new JobAdDto() { Description = Description, Source = new Uri(string.Format("{0}", Link)), Region = Region };
                jobAds.Add(jobAdDto);


            }

            return jobAds;

        }

    }

    
}


