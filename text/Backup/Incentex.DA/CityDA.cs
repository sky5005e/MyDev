///
/// This class does retieve the data from city table.
///
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
    public class CityDA
    {
        #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet dsCity = new DataSet();
        CityBE objCity;
        #endregion

        #region Methods
        /// <summary>
        /// GetCityByStateID()
        /// This method is used to get all city from tabel.
        /// Nagmani kuamr 28-july-2010 
        /// </summary>
        /// <param name="iCountryID"></param>
        /// <returns></returns>
        public DataSet GetCityByStateID(int iStateID)
        {
            
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@pStateID",iStateID) 
			};
            
            dsCity = dm.ExecuteDataSet(CommandType.StoredProcedure, "INC_SP_City_SelectCityByStateID", sqlp);
            return dsCity;
        }

        public DataSet GetCityAndIdByStateID(int iStateID)
        {
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@pStateID",iStateID) 
			};
            dsCity = dm.ExecuteDataSet(CommandType.StoredProcedure, "selectcitybystate", sqlp);
            return dsCity;
        }
        #endregion

    }
}
