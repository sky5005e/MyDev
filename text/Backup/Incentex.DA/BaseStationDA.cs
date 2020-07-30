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
    public class BaseStationDA
    {
        #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet dsBaseStation = new DataSet();
        #endregion

        #region Methods
        public DataSet GetBaseStaionByCountry(BasedStationBE objBSBe)
        {
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@iCountryID",objBSBe.iCountryID),
                new SqlParameter("@sOperation",objBSBe.SOperation)

			};
            dsBaseStation = dm.ExecuteDataSet(CommandType.StoredProcedure, "INC_BaseStation", sqlp);
            return dsBaseStation;
        }

        public DataSet LookUpBaseStation(BasedStationBE lBe)
        {
            try
            {
                string sSQL = "INC_SP_LookupBaseStation";

                SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@sOperation", lBe.SOperation),
                new SqlParameter("@iBaseStationId", lBe.iBaseStationId),
				new SqlParameter("@iCountryCode", lBe.iCountryID),
                new SqlParameter("@sBaseStation",lBe.sBaseStation),
                new SqlParameter("@sBaseStationIcon",lBe.sBaseStationIcon),
                 new SqlParameter("@sWeatherType",lBe.sWeatherType),
                  new SqlParameter("@iLatitude",lBe.iLatitude),
                   new SqlParameter("@iLongitude",lBe.iLongitude),
			};
                return dm.ExecuteDataSet(CommandType.StoredProcedure, sSQL, sqlp);

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
        }
        public DataSet CheckBaseDuplication(int iBaseStationId, string sBaseStation, string sMode)
        {
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@piBaseStationId",iBaseStationId) ,
                 new SqlParameter("@psBaseStation",sBaseStation) ,
                  new SqlParameter("@pMode",sMode),
                  
                  
			};
            dsBaseStation = dm.ExecuteDataSet(CommandType.StoredProcedure, "INC_SelectBaseIconDuplicate", sqlp);
            return dsBaseStation;
        }
        #endregion
    }
}
