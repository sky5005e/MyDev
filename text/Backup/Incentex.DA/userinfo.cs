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
    public class userinfo
    {

        #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet ds = new DataSet();
        #endregion

        #region constructor

        public userinfo()
        {
        }
        

        #endregion

        #region Methods
        public DataSet User(UserBE uBe)
        {
            
            string sSQL = "userinfo";

            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@sOperation", uBe.SOperation),
                new SqlParameter("@sEmailAddress", uBe.sUserName),
				new SqlParameter("@sPassword", uBe.sPassword),
                new SqlParameter("@sStatus",uBe.sStatus)
			};

            ds = dm.ExecuteDataSet(CommandType.StoredProcedure, sSQL,sqlp);
            return ds;
        }

        public List<UserBE> GetAllUsers(string sOperation)
        {
            //Define Generic list
            List<UserBE> objUserBeList = new List<UserBE>();
            string sSQL = "userinfo";

            //Pass parameters here
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@sOperation", sOperation)
			};

            //Call Common Library
            ds = dm.ExecuteDataSet(CommandType.StoredProcedure, sSQL, sqlp);
            //Converting dataset into the Generic list
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    UserBE objUserBE = new UserBE();
                    objUserBE.iProductId = Convert.ToInt32(dr["iProductId"].ToString());
                    objUserBE.iProductName = dr["iProductName"].ToString();
                    objUserBE.dProductePrize = dr["dProductePrize"].ToString();
                    /*
                    objUserBE.sUserName = dr["sUserName"].ToString();
                    objUserBE.sPassword = dr["sPassword"].ToString();
                    objUserBE.sUserType = dr["sUserType"].ToString();
                    objUserBE.iUserId = Convert.ToInt32(dr["iUserId"].ToString());*/

                    objUserBeList.Add(objUserBE);
                }
                //Return Generic list
                
            }
            return objUserBeList;
        }

        #endregion
    }

}
