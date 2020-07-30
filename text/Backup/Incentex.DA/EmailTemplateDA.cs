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
    public class EmailTemplateDA
    {
        #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet dsEmailTemplate = new DataSet();
        EmailTemplateBE objEmailTemplate;
        
        string sSQL;    
        #endregion
        public DataSet EmailTemplate(EmailTemplateBE objEmailTemplate)
        {


            sSQL = "INC_SP_EmailTemplate";
           
            SqlParameter[] sqlp = new SqlParameter[]
			{
                
                new SqlParameter("@sOperation", objEmailTemplate.SOperation),
                         
                new SqlParameter("@sTemplateName", objEmailTemplate.STemplateName)
              
               
     
					
			};

            dsEmailTemplate = dm.ExecuteDataSet(CommandType.StoredProcedure, sSQL, sqlp);

            return dsEmailTemplate;
        }

    }
}
