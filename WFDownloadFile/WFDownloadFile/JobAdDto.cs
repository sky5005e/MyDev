using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Itstep.Jobmachine.Crawler.Core.Providers
{
    public class JobAdDto
    {
        /// <summary>
        /// The name of the provider (the crawled page)
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// The found region (village, canton etcetera)
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// The description of the job - the job ad itself
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Here further found information from the crawling
        /// process can be added. For example category information
        /// like "IT", "Temporaty" etcetera. It is not finally defined
        /// what all can be put into this token bag.
        /// </summary>
        public string TokenBag { get; set; }
        [XmlIgnore]
        public Uri Source { get; set; }
        [XmlElement("Uri")]
        public string UriString
        {
            get { return Source.ToString(); }
            set { Source = new Uri(value); }
        }
    }

}
