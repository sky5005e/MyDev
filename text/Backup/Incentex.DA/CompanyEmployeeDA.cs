using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using commonlib.Common;
using System.Data;
using Incentex.BE;
using System.Data.SqlClient;
using System.Configuration;

namespace Incentex.DA
{
   public class CompanyEmployeeDA
    {
        #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet dsCompanyEmployee = new DataSet();
        
        #endregion

        #region Methods

        public DataSet CompanyEmployee(CompanyEmployeeBE objEmployee)
        {
            SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@Operation",objEmployee.Operations), 
                new SqlParameter("@CompanyID",objEmployee.CompanyId),
                new SqlParameter("@CompanyEmployeeID",objEmployee.CompanyEmployeeID),
                new SqlParameter("@Workgroup",objEmployee.WorkgroupID),
                new SqlParameter("@LoginEmail",objEmployee.LoginEmail)
			};
            dsCompanyEmployee = dm.ExecuteDataSet(CommandType.StoredProcedure, "sp_companyemployee", sqlp);
            return dsCompanyEmployee;
        }

        #endregion

    }
}
