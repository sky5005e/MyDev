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
    public class LookupDA
    {
        #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet dsLookup = new DataSet();
        #endregion
        #region Methods
        /// <summary>
        /// GetAllrecordsByLookupCode()
        /// This method is used to get all records from Lookup tabel 
        /// on passing parameter iLookupCode.
        /// Nagmani kuamr 29-july-2010 
        /// </summary>
        /// <param name="iCountryID"></param>
        /// <returns></returns>
        public DataSet GetAllrecordsByLookupCode(string iLookupCode)
        {
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@pLookupCode",iLookupCode) 
			};
            dsLookup = dm.ExecuteDataSet(CommandType.StoredProcedure, "INC_SP_Lookup_SelectLookupRecord", sqlp);
            return dsLookup;
        }
        public string DeleteRecordByLookupID(int iLookupID)
        {
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@pLookupID",iLookupID) 
			};
            dm.ExecuteNonQuery(CommandType.StoredProcedure, "INC_SP_Lookup_DeleteLookupRecord", sqlp);
            return  "Record deleted successfully!";
        }
        public DataSet LookUp(LookupBE lBe)
        {

            string sSQL = "INC_SP_Lookup";

            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@sOperation", lBe.SOperation),
                new SqlParameter("@iLookupID", lBe.iLookupID),
				new SqlParameter("@iLookupCode", lBe.iLookupCode),
                new SqlParameter("@sLookupName",lBe.sLookupName),
                new SqlParameter("@sLookupIcon",lBe.sLookupIcon),
                new SqlParameter("@Val1",lBe.Val1),


			};

            dsLookup = dm.ExecuteDataSet(CommandType.StoredProcedure, sSQL, sqlp);
            return dsLookup;
        }

       
        public DataSet CheckDuplication(int iLookupID,string sLookupName,string sMode,string sLookCode)
        {
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@piLookupID",iLookupID) ,
                 new SqlParameter("@pLookupName",sLookupName) ,
                  new SqlParameter("@pMode",sMode),
                  new SqlParameter("@pLookupCode",sLookCode)
                  
			};
            dsLookup = dm.ExecuteDataSet(CommandType.StoredProcedure, "INC_SelectDuplicate", sqlp);
            return dsLookup;
        }

        public DataSet GetMasterItemNo(string Val1,int storeid, int workgroupid)
        {
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@Val1",Val1),
                new SqlParameter("@StoreID",storeid),
                new SqlParameter("@Workgroupid",workgroupid) 
			};
            dsLookup = dm.ExecuteDataSet(CommandType.StoredProcedure, "SelectMasterItemNoOnGender", sqlp);
            return dsLookup;
        }
        public DataSet GetGenderType(long UniformIssuancePolicyID)
        {
            SqlParameter[] sqlp = new SqlParameter[]
			{
                 new SqlParameter("@UniformIssuancePolicyID",UniformIssuancePolicyID),   
			};
            dsLookup = dm.ExecuteDataSet(CommandType.StoredProcedure, "SelectGenderType", sqlp);
            return dsLookup;
        }
        #endregion
    }
}
