using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.BE
{
     public class UserTrackingBE
    {
        #region Property

        public Int32 PageHistID { get; set; }
        public Int32 UserTrackingID { get; set; }
        public string PagesName { get; set; }
        public DateTime DateTimePage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #endregion
    }
}
