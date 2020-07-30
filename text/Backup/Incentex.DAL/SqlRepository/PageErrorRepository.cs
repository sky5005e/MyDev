using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class PageErrorRepository : RepositoryBase
    {
        public Boolean AddPageErrorLog(long UserInfoID, String strPageName, String strBrowserName, String strOS, String strResolution, DateTime dtDate, String strErrorDetails, String strComment)
        {
            db.AddPageErrors(UserInfoID, strPageName, strBrowserName, strOS, strResolution, dtDate, strErrorDetails, strComment);

            return true;
        }
    }
}
