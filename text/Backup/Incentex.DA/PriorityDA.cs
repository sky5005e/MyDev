using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using commonlib.Common;
using System.Data;
using System.Configuration;
using Incentex.BE;
using System.Data.SqlClient;

namespace Incentex.DA
{
    public class PriorityDA
    {

        #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet ds = new DataSet();
        #endregion

           #region constructor

        public PriorityDA()
        {
        }
        

        #endregion

        #region Methods
        public DataSet Priority(PriorityBE pBe)
        {

            string sSQL = "priority";

            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@sOperation", pBe.SOperation),
                new SqlParameter("@iPriorityId", pBe.iPriorityId),
				new SqlParameter("@sPriorityName", pBe.sPriorityName),
                new SqlParameter("@sPriorityIcon",pBe.sPriorityIcon)
			};

            ds = dm.ExecuteDataSet(CommandType.StoredProcedure, sSQL,sqlp);
            return ds;
        }

        public List<PriorityBE> GetAllPriority(string sOperation)
        {
            //Define Generic list
            List<PriorityBE> objPriorityBeList = new List<PriorityBE>();
            string sSQL = "priority";

            //Pass parameters here
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@sOperation", sOperation)
			};

            //Call Common Library
            ds = dm.ExecuteDataSet(CommandType.StoredProcedure, sSQL, sqlp);

            //Converting dataset into the Generic list
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                PriorityBE objPriorityBE = new PriorityBE();
                objPriorityBE.iPriorityId = Convert.ToInt32(dr["iProductId"].ToString());
                objPriorityBE.sPriorityName = dr["iProductName"].ToString();
                objPriorityBE.sPriorityIcon = dr["dProductePrize"].ToString();

                objPriorityBeList.Add(objPriorityBE);
            }
            //Return Generic list
            return objPriorityBeList;
        }

        #endregion
    }
}
