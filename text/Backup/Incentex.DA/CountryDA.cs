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
    public class CountryDA
    {
        #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet dsCountry = new DataSet();
        #endregion

        #region Methods
        public DataSet GetCountry()
        {
            dsCountry = dm.ExecuteDataSet(CommandType.StoredProcedure, "INC_SP_Country_SelectCountry", null);
            return dsCountry;
        }

        public DataSet GetAllCountry()
        {
            dsCountry = dm.ExecuteDataSet(CommandType.StoredProcedure, "selectallcountries", null);
            return dsCountry;
        }
        #endregion
    }
}
