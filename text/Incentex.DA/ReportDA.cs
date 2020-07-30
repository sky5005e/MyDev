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
    public class ReportDA
    {
        #region property
        DBManager dm = new DBManager(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataSet dsReportInventory = new DataSet();
        ReportBE objReport;
        #endregion

         public DataSet GetProdcutToOrderAssociation(Int64 MasterItemNumber, string ItemNumber, string OrderFor, string OrderStatus, Int64? WorkgroupId, Int64? BaseStationID)
         {
             SqlParameter[] sqlp = new SqlParameter[]
			{
         
                new SqlParameter("@masteritemid",MasterItemNumber),
                new SqlParameter("@itemnumber",ItemNumber),
                new SqlParameter("@orderfor",OrderFor),
                new SqlParameter("@orderstatus",OrderStatus),
                new SqlParameter("@workgroupid", WorkgroupId),
                new SqlParameter("@BaseStation",BaseStationID)
         
			};
             dsReportInventory = dm.ExecuteDataSet(CommandType.StoredProcedure, "producttoorderassociationreport", sqlp);
             return dsReportInventory;
         }

         public DataSet GetInventoryUsage(int MasterItemNumber, string ItemNumber, int Size)
         {
             SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@MasterItemNumber",MasterItemNumber),
                new SqlParameter("@ItemNumber",ItemNumber),
                new SqlParameter("@ItemSize",Size),
			};
             dsReportInventory = dm.ExecuteDataSet(CommandType.StoredProcedure, "SelectInventoryUsages", sqlp);
             return dsReportInventory;
         }
         public DataSet GetIncompleteOrderReport(DateTime? FromDate, DateTime? Todate, string StoreID, string ordefor)
         {
             SqlParameter[] sqlp = new SqlParameter[]
			{
                new SqlParameter("@FromDate",FromDate), 
                new SqlParameter("@Todate",Todate),
                new SqlParameter("@StoreID",StoreID),
                new SqlParameter("@orderfor",ordefor),
			};
             dsReportInventory = dm.ExecuteDataSet(CommandType.StoredProcedure, "SelectIncompleteOrder", sqlp);
             return dsReportInventory;
         }
    }
}