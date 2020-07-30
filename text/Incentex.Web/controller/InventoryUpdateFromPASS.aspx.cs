using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class controller_InventoryUpdateFromPASS : System.Web.UI.Page
{
    #region Data Member
    ProductItemDetailsRepository objProductItemDetailsRepository = new ProductItemDetailsRepository();
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PASSConnection"].ConnectionString);
        try
        {
            ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();

            //Get Supplier id of Pilot Freight Services
            Supplier objsup = new SupplierRepository().GetSinglSupplierid("Pilot Freight Services");//new SupplierRepository().GetSinglSupplierid("WTDC");
            if (objsup != null)
            {
                int supplierid = Convert.ToInt32(objsup.SupplierID);

                String strQuery = "SELECT DISTINCT ProductID, QtyOnHand, Available FROM dbo.ProductWarehouseSummary WHERE ProductType = 'Inv' AND Deleted = 0";

                SqlCommand command = new SqlCommand(strQuery, connection);

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //This try and catch is due to skip error item and continue with next item.
                        try
                        {
                            string ItemNumber = Convert.ToString(reader["ProductID"]);
                            Int32 Inventory = Convert.ToInt32(reader["Available"]);
                            List<ProductItemInventory> objProductItemInventoryList = objProductItemDetailsRepository.GetProductItemInventoryByItemNumberAndSupplierID(ItemNumber, supplierid);
                            foreach (ProductItemInventory item in objProductItemInventoryList)
                            {
                                item.Inventory = Inventory;
                                Response.Write(ItemNumber + " -> " + Inventory + " Updated Successfully.<br/>");
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
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            connection.Close();
        }
    }
    #endregion
}