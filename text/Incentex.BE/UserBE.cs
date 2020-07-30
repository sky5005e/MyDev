using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.BE
{
    public class UserBE
    {
        #region Property

        public string SOperation { get; set; }
        //Test Entites for the userinfo
        public long iUserId { get; set; }
        public string sUserName { get; set; }
        public string sPassword { get; set; }
        public string sUserType { get; set; }
        public string sStatus { get; set; }
        //Test Enitities for the product
        public long iProductId { get; set; }
        public string iProductName { get; set; }
        public string dProductePrize { get; set; }

        #endregion
    }
}
