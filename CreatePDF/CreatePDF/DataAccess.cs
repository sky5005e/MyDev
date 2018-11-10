using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace CreatePDF
{
    public class DataAccess
    {
        const String DBConn = @"Data Source=APSVR;Initial Catalog=CRM;Integrated Security=True";
        public DataAccess()
        {
        }
        public void GetConnected_PerformOperations(string query_string)
        {

            SqlConnection sqlcon = new SqlConnection(DBConn);
            SqlCommand sqlcomm = new SqlCommand(query_string, sqlcon);
            sqlcon.Open();
            sqlcomm.ExecuteNonQuery();
            sqlcon.Close();
        }

        public DataSet GetTableDisconnect_Connection(string query_string)
        {
            SqlDataAdapter SqlDtAdapter = new SqlDataAdapter(query_string, DBConn);
            DataSet ds = new DataSet();
            SqlDtAdapter.Fill(ds);
            return ds;

        }

        public Boolean IsOrderNumberExist(string query_string)
        {

            SqlDataAdapter SqlDtAdapter = new SqlDataAdapter(query_string, DBConn);
            DataSet ds = new DataSet();
            SqlDtAdapter.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;

        }

        public void ExecuteNonParameterSP(String spName)
        {

            using (SqlConnection con = new SqlConnection(DBConn))
            {
                using (SqlCommand cmd = new SqlCommand(spName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@p1", SqlDbType.VarChar).Value = p1;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
