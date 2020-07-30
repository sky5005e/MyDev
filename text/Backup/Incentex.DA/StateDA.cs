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
    public class StateDA
    {
        #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet dsState = new DataSet();
        StateBE objState;
        #endregion

        #region Methods
        /// <summary>
        /// GetState()
        /// This method is used to getall state from tabel.
        /// Nagmani kuamr 26-july-2010 
        /// </summary>
        /// <returns></returns>
        public DataSet GetState()
        {
            dsState = dm.ExecuteDataSet(CommandType.StoredProcedure, "INC_SP_State_SelectState", null);
            return dsState;
        }
        /// <summary>
        /// GetStateByCountryID()
        /// This method is used to getall state from tabel.
        /// Nagmani kuamr 26-july-2010 
        /// </summary>
        /// <param name="iCountryID"></param>
        /// <returns></returns>
        public DataSet GetStateByCountryID(int iCountryID)
        {
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@pCountryID",iCountryID) 
			};
            dsState = dm.ExecuteDataSet(CommandType.StoredProcedure, "INC_SP_State_SelectStateByCountryID", sqlp);
            return dsState;
        }
        public DataSet GetStateAndIdByCountryID(int iCountryID)
        {
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@pCountryID",iCountryID) 
			};
            dsState = dm.ExecuteDataSet(CommandType.StoredProcedure, "selectstatebyid", sqlp);
            return dsState;
        }
        #endregion
    }
}
