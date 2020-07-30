using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using commonlib.Common;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace Incentex.DA
{
    public class status
    {
        #region property
        public string SOperation { get; set; }
        public int statusid { get; set; }
        public string statusname { get; set; }
        #endregion

        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet ds = new DataSet();

        #region Methods
        public DataSet Status()
        {

            string sSQL = "status";

            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@sOperation", SOperation),
                new SqlParameter("@sStatusName", statusname),
				new SqlParameter("@iStatusid", statusid)
			};

            ds = dm.ExecuteDataSet(CommandType.StoredProcedure, sSQL, sqlp);
            return ds;
        }

        #endregion

    }
}
