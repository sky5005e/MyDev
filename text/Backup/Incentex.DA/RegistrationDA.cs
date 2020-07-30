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
    public class RegistrationDA
    {

        #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet dsReg = new DataSet();
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the regBe class.
        /// </summary>
        public RegistrationDA()
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Saves a record to the registration table.
        /// </summary>
        public int Insert(RegistrationBE regBe)
        {
            SqlParameter[] parameters = new SqlParameter[]
			{				
                new SqlParameter("@iCompanyName",regBe.iCompanyName), 
				new SqlParameter("@sCompanyName", regBe.SCompanyName),
				new SqlParameter("@sFirstName", regBe.SFirstName),
				new SqlParameter("@sLastName", regBe.SLastName),
				new SqlParameter("@sAddress1", regBe.SAddress1),
				new SqlParameter("@sAddress2", regBe.SAddress2),
				new SqlParameter("@iCountryId", regBe.ICountryId),
				new SqlParameter("@iStateId", regBe.IStateId),
				new SqlParameter("@iCityId", regBe.ICityId),
				new SqlParameter("@sZipCode", regBe.SZipCode),
				new SqlParameter("@sEmailAddress", regBe.SEmailAddress),
				new SqlParameter("@sTelephoneNumber", regBe.STelephoneNumber),
				new SqlParameter("@sMobileNumber", regBe.SMobileNumber),
				new SqlParameter("@sEmployeeId", regBe.SEmployeeId),
				new SqlParameter("@iWorkgroupId", regBe.IWorkgroupId),
				new SqlParameter("@iBasestationId", regBe.IBasestationId),
				new SqlParameter("@iGender", regBe.IGender),                
                new SqlParameter("@sPhoto", regBe.SPhoto),
                new SqlParameter("@iDepartmentID", regBe.IDepartmentId),
                new SqlParameter("@Status", regBe.Status),
                new SqlParameter("@DateRequestSubmitted", regBe.DateRequestSubmitted),
                new SqlParameter("@DOH",regBe.DOH),
                new SqlParameter("@iEmployeeTypeID",regBe.iEmployeeTypeID)
			};

            int ret = dm.ExecuteNonQuery(CommandType.StoredProcedure, "INC_SP_Registration_Insert", parameters);
            //return "Record inserted successfully!";
            return ret;
        }
        public DataSet User(RegistrationBE regBe)
        {
            SqlParameter[] sqlp = new SqlParameter[]
            {
            	new SqlParameter("@sEmailAddress", regBe.SEmailAddress)	   		
            };
            dsReg = dm.ExecuteDataSet(CommandType.StoredProcedure, "INC_SP_CheckUserEmail", sqlp);
            return dsReg;
        }
        #endregion
       
    }
}
