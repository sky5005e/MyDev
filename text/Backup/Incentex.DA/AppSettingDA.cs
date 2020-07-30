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
    
    public class AppSettingDA
    {
         #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet ds = new DataSet();
        AppSettingBE objAppSetBE = new AppSettingBE();
        string sSQL;
         #endregion

        #region Functions
        public DataSet AppSetting(AppSettingBE objAppSetBE)
        {
            sSQL = "INC_SP_AppSetting";

            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@sOperation", objAppSetBE.SOperation)
               
            };

            ds = dm.ExecuteDataSet(CommandType.StoredProcedure, sSQL, sqlp);

            return ds;
        }
        #endregion
    }
    
}
