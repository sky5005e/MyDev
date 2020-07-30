using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using commonlib.Common;

namespace Incentex.DA
{
    public class RepositoryBase
    {
        

        #region Properties

        

        #endregion


        public RepositoryBase()
        {
            DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
            //CheckConn();
            //string conn = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

            
        }

        bool CheckConn()
        {
            string conn = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            SqlConnection cn = new SqlConnection(conn);
            bool b = true;

            try
            {
                cn.Open();
            }
            catch (Exception ex)
            {
                b = false;
                throw ex;
            }
            finally
            {
                cn.Close();
            }

            return b;
        }

       

   

    }
}
