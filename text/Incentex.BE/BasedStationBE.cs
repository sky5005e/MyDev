using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.BE
{
    public class BasedStationBE
    {
        #region Property
        public string SOperation { get; set; }

        public long iBaseStationId { get; set; }
        public string sBaseStation { get; set; }
        public string sBaseStationIcon { get; set; }
        public long iCountryID { get; set; }
        public string sCountry { get; set; }
        public string sWeatherType { get; set; }
        public string iLatitude { get; set; }
        public string iLongitude { get; set; }
        #endregion
    }
}
