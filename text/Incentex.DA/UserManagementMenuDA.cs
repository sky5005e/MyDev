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
   public class UserManagementMenuDA
    {
        #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet ds = new DataSet();
        string sSQL;
        #endregion
        #region Functions
        public DataSet getMenuData(UserManagementMenuBE objBE)
        {
            
                sSQL = "INC_SP_MenuSelect";
                SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@pManageID", objBE.IManageID) 
            };
                ds = dm.ExecuteDataSet(CommandType.StoredProcedure, sSQL, sqlp);
                return ds;       
        }

        public DataSet getMenuSubData(UserManagementMenuBE objBE)
        {
            sSQL = "INC_SP_MenuSubSelect";
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@piMenuId", objBE.IMenuID)
            };
            ds = dm.ExecuteDataSet(CommandType.StoredProcedure, sSQL, sqlp);
            return ds;
        }

        public DataSet getMenuSubID(UserManagementMenuBE objBE)
        {
            sSQL = "INC_SP_MenuSubIDSelect";
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@piMenuName", objBE.SMenuName),
                new SqlParameter("@iManageID", objBE.IManageID),

            };
            ds = dm.ExecuteDataSet(CommandType.StoredProcedure, sSQL, sqlp);
            return ds;
        }
        public DataSet getSubMenuUrl(UserManagementMenuBE objBE)
        {
            sSQL = "INC_SP_SubMenuURLSelect";
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@psSubMenuName", objBE.SMenuSubName)
            };
            ds = dm.ExecuteDataSet(CommandType.StoredProcedure, sSQL, sqlp);
            return ds;
        }
        #endregion
    }

}
