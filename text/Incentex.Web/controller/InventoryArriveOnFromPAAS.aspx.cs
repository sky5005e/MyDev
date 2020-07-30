using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class controller_InventoryArriveOnFromPAAS : System.Web.UI.Page
{
    #region Data Member
    ProductItemDetailsRepository objProductItemDetailsRepository = new ProductItemDetailsRepository();
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime StartTime = DateTime.Now;
        Response.Write("Operation Started : " + StartTime + "<br/>");
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PASSConnection"].ConnectionString);
        try
        {
            ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();

            //Get Supplier id of Pilot Freight Services
            Supplier objsup = new SupplierRepository().GetSinglSupplierid("Pilot Freight Services");//new SupplierRepository().GetSinglSupplierid("WTDC");
            if (objsup != null)
            {
                int supplierid = Convert.ToInt32(objsup.SupplierID);

                StringBuilder strQuery = new StringBuilder();
                strQuery.Append("SELECT DISTINCT D.ProductID, O.PromisedDate FROM dbo.PO O INNER JOIN dbo.PODetail D ON O.GUIDPO = D.GUIDPO ");
                strQuery.Append("WHERE O.POStatus = 'I' AND O.PromisedDate IS NOT NULL AND O.PromisedDate >= GETDATE() ORDER BY D.ProductID, O.PromisedDate DESC ");

                SqlCommand command = new SqlCommand(strQuery.ToString(), connection);

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //This try and catch will handle error in item (if any) and continue with next item.
                            try
                            {
                                String ItemNumber = Convert.ToString(reader["ProductID"]);
                                String PromisedDate = Convert.ToString(reader["PromisedDate"]);
                                List<ProductItemInventory> objProductItemInventoryList = objProductItemDetailsRepository.GetProductItemInventoryByItemNumberAndSupplierID(ItemNumber, supplierid);
                                foreach (ProductItemInventory item in objProductItemInventoryList)
                                {
                                    item.ToArriveOn = Convert.ToDateTime(PromisedDate);
                                    Response.Write(ItemNumber + " -> " + PromisedDate + " Updated Successfully.<br/>");
                                }
                                objProductItemDetailsRepository.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                ErrHandler.WriteError(ex);
                            }
                        }
                    }
                    reader.Close();
                    connection.Close();
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            connection.Close();
        }
        DateTime CompletionTime = DateTime.Now;
        Response.Write("Operation Completed : " + CompletionTime);
        Response.Write("<br/>");
        Response.Write("The duration to complete the operation : " + (CompletionTime - StartTime));
    }
    #endregion
}