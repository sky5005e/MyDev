using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using commonlib.Common;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Incentex.BE;

namespace Incentex.DA
{
   public class UserTrackingDA
    {
        #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet dsTracking = new DataSet();
        UserTrackingBE objUserTrackingBe;
        #endregion

        #region This is for Getting top10 pages viewResult
        public DataSet GetTop10PagesViewed1(DateTime StartDate, DateTime EndDate, int RecordNumber)
        {
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@StartDate",StartDate) ,
                new SqlParameter("@EndDate",EndDate),
                new SqlParameter("@RecordNumber",RecordNumber)
			};
            dsTracking = dm.ExecuteDataSet(CommandType.StoredProcedure, "GetTop10PagesViewed", sqlp);
            return dsTracking;
        }
        #endregion
        #region This Sp is for the getting the list of the User whose are accessing page 31/1/2012

        public DataSet GetUserAccessFromPageViewd(DateTime StartDate, DateTime EndDate, string Url)
        {
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@StartDate",StartDate) ,
                new SqlParameter("@EndDate",EndDate),
                new SqlParameter("@Url",Url)
			};
            dsTracking = dm.ExecuteDataSet(CommandType.StoredProcedure, "GetUserWiseAccessListFinal", sqlp);
            return dsTracking;
        }
        #endregion

    }
}
