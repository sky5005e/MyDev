using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Supplier_SyncInformation : PageBase
{
    #region Data Member

    OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();
    ProductItemDetailsRepository objProductItemDetailsRepository = new ProductItemDetailsRepository();

    StringBuilder OrderIDs;
    StringBuilder ShippedInThisSync;

    #endregion

    #region Event Handlers

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Sync Warehouse Data";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";
            ((Label)Master.FindControl("lblPageHeading")).Text = "Sync Information";
            txtFromDate.Text = DateTime.Today.Date.ToString("MM/dd/yyyy");
            txtToDate.Text = DateTime.Today.Date.ToString("MM/dd/yyyy");
        }
    }

    protected void lnkSyncOrderData_Click(Object sender, EventArgs e)
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PASSConnection"].ConnectionString);
        try
        {
            //Get Supplier id of Pilot Freight Services
            Supplier objsup = new SupplierRepository().GetSinglSupplierid("Pilot Freight Services");//new SupplierRepository().GetSinglSupplierid("WTDC");
            if (objsup != null)
            {
                //This is for display data to page that which order is affected by this.                
                dvDetail.Visible = true;
                repUpdatedOrders.Visible = true;
                gvUpdatedItems.Visible = false;
                lblUpdateTitle.Text = "Updated Order List: ";

                Int32 supplierid = Convert.ToInt32(objsup.SupplierID);

                StringBuilder strQuery = new StringBuilder();
                strQuery.Append("SELECT DISTINCT O.WebOrderNumber, O.OrderNumber, ID.ProductID, S.ShipmentUpdatedDate, ID.QtyShipped, SS.NumberOfCartons, S.Carrier, ");
                strQuery.Append("S.TrackingNumber, O.PO CustomerPO, ROW_NUMBER() OVER (PARTITION BY O.WebOrderNumber ORDER by O.WebOrderNumber) RowNumber, ");
                strQuery.Append("SUM(1) OVER (PARTITION BY O.WebOrderNumber) TotalProducts ");

                strQuery.Append("FROM dbo.Shipment S ");
                strQuery.Append("INNER JOIN dbo.ShipmentOrder SO ON S.GUIDShipment = SO.GUIDShipment ");
                strQuery.Append("INNER JOIN dbo.Orders O ON SO.GUIDOrder = O.GUIDOrder ");
                strQuery.Append("INNER JOIN dbo.OrderDetail OD ON O.GUIDOrder = OD.GUIDOrder ");
                strQuery.Append("INNER JOIN dbo.ShipmentSummary SS ON S.GUIDShipment = SS.GUIDShipment ");
                strQuery.Append("INNER JOIN dbo.Invoice I ON SS.GUIDInvoice = I.GUIDInvoice ");
                strQuery.Append("INNER JOIN dbo.InvoiceDetail ID ON I.GUIDInvoice = ID.GUIDInvoice AND OD.GUIDOrderDetail = ID.GUIDOrderDetail ");
                strQuery.Append("AND OD.GUIDProduct = ID.GUIDProduct ");

                strQuery.Append("WHERE ID.QtyShipped > 0 And S.Carrier IS NOT NULL AND S.Carrier <> '' ");
                strQuery.Append("AND CONVERT(DATE, S.ShipmentUpdatedDate) >= '" + Convert.ToDateTime(txtFromDate.Text.Trim()).ToString("yyyy-MM-dd") + "' ");
                strQuery.Append("AND CONVERT(DATE, S.ShipmentUpdatedDate) <= '" + Convert.ToDateTime(txtToDate.Text.Trim()).ToString("yyyy-MM-dd") + "' ");
                strQuery.Append("ORDER BY O.WebOrderNumber, RowNumber ");

                SqlCommand command = new SqlCommand(strQuery.ToString(), connection);

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                StringBuilder objHtml = new StringBuilder();
                if (reader.HasRows)
                {
                    String OrderNumberOld = null;
                    String TrackingNumberOld = null;
                    String PackageIdOld = null;

                    OrderIDs = new StringBuilder();
                    ShippedInThisSync = new StringBuilder();

                    while (reader.Read())
                    {
                        //This try and catch is due to skip error item and continue with next item.
                        try
                        {
                            Int32 TotalProducts = Convert.ToInt32(reader["TotalProducts"]);
                            Int32 RowNumber = Convert.ToInt32(reader["RowNumber"]);
                            String CarrierName = Convert.ToString(reader["Carrier"]);
                            String TrackingNumber = Convert.ToString(reader["TrackingNumber"]);
                            DateTime ShippingDate = Convert.ToDateTime(reader["ShipmentUpdatedDate"]);
                            Int64 ShipQuantity = Convert.ToInt64(reader["QtyShipped"]);
                            String PartNumber = Convert.ToString(reader["ProductID"]);
                            String SalesOrderNumber = Convert.ToString(reader["WebOrderNumber"]);
                            Int32? NoOfBoxes = Convert.ToInt32(reader["NumberOfCartons"]);
                            Int64? ShipperService = new LookupRepository().GetIdByLookupNameNLookUpCode(CarrierName, "Shipping Type");
                            String PAASOrderNumber = Convert.ToString(reader["OrderNumber"]);
                            String CustomerPO = Convert.ToString(reader["CustomerPO"]);

                            SelectPAASOrderByWebOrderNoOrCustomerPOResult objOrder = new OrderConfirmationRepository().GetByWebOrderNoORCustomerPONo(SalesOrderNumber.Trim(), CustomerPO.Trim());
                            if (objOrder != null && ShipperService != null && objOrder.OrderStatus.ToUpper() != "CLOSED" && objOrder.OrderStatus.ToUpper() != "DELETED")
                            {
                                //This is for remove previous content of email when order number change
                                if (OrderNumberOld != objOrder.OrderNumber)
                                    objHtml.Remove(0, objHtml.Length);

                                Int64 strMyShoppingCartID = 0;
                                Int64 TotalItemOrderQty = 0;
                                Int64 TotalOrderQty = 0;
                                Int64 ActualRemainingQty = 0;
                                Int64 AlreadyShippedQty = 0;                                
                                String strProductDesc = "";
                                String strSize = "";

                                UserInformation objUserInformation = new UserInformationRepository().GetById(objOrder.UserId);
                                CompanyStoreRepository.IECompanyListResults objStore = new CompanyStoreRepository().GetCompanyStore((Int64)objUserInformation.CompanyId);                                

                                //This is for taking shoppingcartid, totalorderqty and totalitemorderqty
                                if (objOrder.OrderFor == "ShoppingCart")
                                {
                                    List<SelectMyShoppingCartProductResult> objMyShoppingCartList = new MyShoppingCartRepository().SelectCurrentOrderProducts(objOrder.OrderID);
                                    for (Int32 k = 0; k < objMyShoppingCartList.Count; k++)
                                    {
                                        TotalOrderQty += Convert.ToInt64(objMyShoppingCartList[k].Quantity);
                                        if (objMyShoppingCartList[k].item.Replace(" ", "").ToUpper() == PartNumber.ToUpper())
                                        {
                                            strMyShoppingCartID = objMyShoppingCartList[k].MyShoppingCartID;
                                            strProductDesc = objMyShoppingCartList[k].ProductDescrption1;
                                            strSize = objMyShoppingCartList[k].Size;
                                            TotalItemOrderQty = Convert.ToInt64(objMyShoppingCartList[k].Quantity);
                                        }
                                    }
                                }
                                else
                                {
                                    List<SelectMyIssuanceProductItemsResult> objFinal = new MyIssuanceCartRepository().GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);
                                    for (Int32 k = 0; k < objFinal.Count; k++)
                                    {
                                        if (objFinal != null && objFinal[k] != null)
                                        {
                                            TotalOrderQty += Convert.ToInt64(objFinal[k].Qty);
                                            if (objFinal[k].item.Replace(" ", "").ToUpper() == PartNumber.ToUpper())
                                            {
                                                strMyShoppingCartID = objFinal[k].MyIssuanceCartID;
                                                strProductDesc = objFinal[k].ProductDescrption;
                                                strSize = objFinal[k].sLookupName;
                                                TotalItemOrderQty = Convert.ToInt64(objFinal[k].Qty);
                                            }
                                        }
                                    }
                                }

                                //Check if item already process with same tracking number, item and order
                                if (strMyShoppingCartID != 0 && new ShipOrderRepository().CheckIfCargoAlreadyProcess(objOrder.OrderID, strMyShoppingCartID, TrackingNumber).Count == 0)
                                {
                                    //This is for taking total shipped qty for this item before this shipped
                                    CountTotalShippedQuantiyResult objTotalItemShippedQty = new ShipOrderRepository().GetQtyShippedTotal(Convert.ToInt32(strMyShoppingCartID), PartNumber, objOrder.OrderID);
                                    AlreadyShippedQty = (objTotalItemShippedQty != null ? Convert.ToInt64(objTotalItemShippedQty.ShipQuantity) : 0);

                                    ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();

                                    //remaining order total qty for current item
                                    ActualRemainingQty = TotalItemOrderQty - AlreadyShippedQty;

                                    //Insert Record to shiporder table
                                    ShipingOrder objShiporder = new ShipingOrder();
                                    objShiporder.ShipingDate = ShippingDate;
                                    objShiporder.TrackingNo = TrackingNumber;

                                    if (OrderNumberOld != null && OrderNumberOld == objOrder.OrderNumber && TrackingNumberOld != null && TrackingNumberOld == TrackingNumber)
                                        objShiporder.PackageId = PackageIdOld == null ? "Shipment_" + DateTime.Now.Ticks : PackageIdOld;
                                    else
                                        objShiporder.PackageId = "Shipment_" + DateTime.Now.Ticks;

                                    PackageIdOld = objShiporder.PackageId;
                                    OrderNumberOld = objOrder.OrderNumber;
                                    TrackingNumberOld = TrackingNumber;

                                    objShiporder.ShipperService = ShipperService;
                                    objShiporder.NoOfBoxes = NoOfBoxes;
                                    objShiporder.IsShipped = true;
                                    objShiporder.SupplierId = supplierid;
                                    objShiporder.UpdateBy = objOrder.UserId;
                                    objShiporder.OrderID = objOrder.OrderID;
                                    objShiporder.QtyOrder = TotalItemOrderQty;
                                    objShiporder.MyShoppingCartiD = strMyShoppingCartID;
                                    objShiporder.ItemNumber = PartNumber;

                                    objShiporder.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                                    objShiporder.CreatedDate = DateTime.Now;
                                    objShiporder.ProcessedFrom = "Sync Information : " + txtFromDate.Text.Trim() + " to " + txtToDate.Text.Trim();

                                    if (ActualRemainingQty > 0)//some quantity remaining
                                    {
                                        if (ActualRemainingQty >= ShipQuantity)//normal shipped qty case
                                        {
                                            objShiporder.IsExtra = false;
                                            objShiporder.ShipQuantity = ShipQuantity;
                                            objShiporder.RemaingQutOrder = ActualRemainingQty - ShipQuantity;

                                            //change order item status based on remaining order item qty
                                            if (objShiporder.RemaingQutOrder == 0)
                                                objShiporder.ShippingOrderStatus = "Shipped Complete";
                                            else
                                                objShiporder.ShippingOrderStatus = "Partial Shipped";

                                            objShipOrderRepos.Insert(objShiporder);
                                            objShipOrderRepos.SubmitChanges();

                                            objHtml.Append("<tr>");
                                            objHtml.Append("<td valign='top'>" + PartNumber + "</td><td valign='top'>" + TotalItemOrderQty + "</td><td valign='top'>" + ShipQuantity + "</td><td valign='top'></td><td valign='top'>" + strSize + "</td><td valign='top'>" + strProductDesc + "</td><td valign='top'>" + ShippingDate.ToString() + "</td><td valign='top'>" + CarrierName + "</td><td valign='top'><a href='http://wwwapps.ups.com/WebTracking/track?trackNums=" + TrackingNumber + "&track.x=Track'>" + TrackingNumber + "</a></td>");
                                            objHtml.Append("</tr>");

                                            if (ShippedInThisSync.Length == 0)
                                                ShippedInThisSync.Append(Convert.ToString(objOrder.OrderID) + ":" + strProductDesc + ":" + TotalItemOrderQty + ":" + ShipQuantity + ":" + AlreadyShippedQty);
                                            else
                                                ShippedInThisSync.Append("," + Convert.ToString(objOrder.OrderID) + ":" + strProductDesc + ":" + TotalItemOrderQty + ":" + ShipQuantity + ":" + AlreadyShippedQty);
                                        }
                                        else//more qty shipped than actual remaining
                                        {
                                            objShiporder.IsExtra = false;
                                            objShiporder.ShipQuantity = ActualRemainingQty;
                                            objShiporder.RemaingQutOrder = 0;
                                            objShiporder.ShippingOrderStatus = "Shipped Complete";

                                            objShipOrderRepos.Insert(objShiporder);
                                            objShipOrderRepos.SubmitChanges();

                                            objHtml.Append("<tr>");
                                            objHtml.Append("<td valign='top'>" + PartNumber + "</td><td valign='top'>" + TotalItemOrderQty + "</td><td valign='top'>" + ActualRemainingQty + "</td><td valign='top'></td><td valign='top'>" + strSize + "</td><td valign='top'>" + strProductDesc + "</td><td valign='top'>" + ShippingDate.ToString() + "</td><td valign='top'>" + CarrierName + "</td><td valign='top'><a href='http://wwwapps.ups.com/WebTracking/track?trackNums=" + TrackingNumber + "&track.x=Track'>" + TrackingNumber + "</a></td>");
                                            objHtml.Append("</tr>");

                                            if (ShippedInThisSync.Length == 0)
                                                ShippedInThisSync.Append(Convert.ToString(objOrder.OrderID) + ":" + strProductDesc + ":" + TotalItemOrderQty + ":" + ActualRemainingQty + ":" + AlreadyShippedQty);
                                            else
                                                ShippedInThisSync.Append("," + Convert.ToString(objOrder.OrderID) + ":" + strProductDesc + ":" + TotalItemOrderQty + ":" + ActualRemainingQty + ":" + AlreadyShippedQty);

                                            //inserting extra qty as another record
                                            ShipingOrder objExtra = new ShipingOrder();
                                            objExtra.ShipingDate = objShiporder.ShipingDate;
                                            objExtra.TrackingNo = objShiporder.TrackingNo;
                                            objExtra.PackageId = objShiporder.PackageId;

                                            objExtra.ShipperService = objShiporder.ShipperService;
                                            objExtra.NoOfBoxes = objShiporder.NoOfBoxes;
                                            objExtra.IsShipped = objShiporder.IsShipped;
                                            objExtra.SupplierId = objShiporder.SupplierId;
                                            objExtra.OrderID = objShiporder.OrderID;
                                            objExtra.QtyOrder = objShiporder.QtyOrder;
                                            objExtra.MyShoppingCartiD = objShiporder.MyShoppingCartiD;
                                            objExtra.ItemNumber = objShiporder.ItemNumber;
                                            objExtra.ShippingOrderStatus = "Extra";
                                            objExtra.IsExtra = true;
                                            objExtra.ShipQuantity = ShipQuantity - ActualRemainingQty;
                                            objExtra.RemaingQutOrder = 0;
                                            objExtra.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                                            objExtra.CreatedDate = objShiporder.CreatedDate;
                                            objExtra.ProcessedFrom = objShiporder.ProcessedFrom;

                                            objShipOrderRepos.Insert(objExtra);
                                            objShipOrderRepos.SubmitChanges();
                                        }

                                        if (RowNumber == TotalProducts)
                                        {
                                            sendShippingDetailEmail(objOrder.OrderID, objOrder.OrderNumber, objOrder.UserId, objHtml.ToString());
                                            //here remove the html if order number changed
                                            objHtml.Remove(0, objHtml.Length);

                                            if (OrderIDs.Length == 0)
                                                OrderIDs.Append(objOrder.OrderID + ":" + objOrder.OrderNumber + ":" + objStore.Company + ":" + objUserInformation.FirstName + ":" + objUserInformation.LastName);
                                            else
                                                OrderIDs.Append("," + objOrder.OrderID + ":" + objOrder.OrderNumber + ":" + objStore.Company + ":" + objUserInformation.FirstName + ":" + objUserInformation.LastName);
                                        }
                                    }
                                    else//no qty remaining, i.e. over shipped case
                                    {
                                        objShiporder.ShippingOrderStatus = "Extra";
                                        objShiporder.IsExtra = true;
                                        objShiporder.ShipQuantity = ShipQuantity;
                                        objShiporder.RemaingQutOrder = 0;

                                        objShipOrderRepos.Insert(objShiporder);
                                        objShipOrderRepos.SubmitChanges();
                                    }

                                    //Update order table status
                                    Boolean IsOrderShippedComplete = new ShipOrderRepository().IsOrderShippedComplete(objOrder.OrderID);
                                    if (IsOrderShippedComplete)
                                        new OrderConfirmationRepository().UpdatePAASOrder(objOrder.OrderID, "Closed", PAASOrderNumber);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            objHtml.Remove(0, objHtml.Length);
                            ErrHandler.WriteError(ex);
                        }
                    }
                }
                reader.Close();
                connection.Close();

                BindRepeater();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            connection.Close();
            lblMsg.Text = "Order Information Sync failed.";
            dvDetail.Visible = false;
        }
    }

    protected void lnkSyncInventoryData_Click(Object sender, EventArgs e)
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PASSConnection"].ConnectionString);
        try
        {
            ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();

            //Get Supplier id of Pilot Freight Services
            Supplier objsup = new SupplierRepository().GetSinglSupplierid("Pilot Freight Services");//new SupplierRepository().GetSinglSupplierid("WTDC");
            if (objsup != null)
            {
                //This is for display data to page that which order is affected by this.
                dvDetail.Visible = true;
                gvUpdatedItems.Visible = true;
                repUpdatedOrders.Visible = false;
                lblUpdateTitle.Text = "Updated Inventory List: ";

                Int32 supplierid = Convert.ToInt32(objsup.SupplierID);

                String strQuery = "SELECT DISTINCT ProductID, QtyOnHand, Available FROM dbo.ProductWarehouseSummary WHERE ProductType = 'Inv' AND Deleted = 0";

                SqlCommand command = new SqlCommand(strQuery, connection);

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                DataTable dtUpdatedItems = new DataTable();
                dtUpdatedItems.Columns.Add("ProdDesc");
                dtUpdatedItems.Columns.Add("UpdatedQty");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //This try and catch is due to skip error item and continue with next item.
                        try
                        {
                            String ItemNumber = Convert.ToString(reader["ProductID"]).Replace(" ", "");
                            Int32 Inventory = Convert.ToInt32(reader["Available"]);
                            List<ProductItemInventory> objProductItemInventoryList = objProductItemDetailsRepository.GetProductItemInventoryByItemNumberAndSupplierID(ItemNumber, supplierid);
                            foreach (ProductItemInventory item in objProductItemInventoryList)
                            {
                                item.Inventory = Inventory;
                            }
                            objProductItemDetailsRepository.SubmitChanges();
                            DataRow dr = dtUpdatedItems.NewRow();
                            dr["ProdDesc"] = ItemNumber;
                            dr["UpdatedQty"] = Inventory;
                            dtUpdatedItems.Rows.Add(dr);
                        }
                        catch (Exception ex)
                        {
                            ErrHandler.WriteError(ex);
                        }
                    }
                }
                reader.Close();
                connection.Close();

                gvUpdatedItems.DataSource = dtUpdatedItems;
                gvUpdatedItems.DataBind();

                lblRecords.Text = Convert.ToString(dtUpdatedItems.Rows.Count);

                lblMsg.Text = "Inventory Sync done Successfully.";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            connection.Close();
            lblMsg.Text = "Inventory Sync failed.";
            dvDetail.Visible = false;
        }
    }

    protected void repUpdatedOrders_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HiddenField hdnRepOrderID = (HiddenField)e.Item.FindControl("hdnRepOrderID");

            GridView gvUpdatedOrderItems = (GridView)e.Item.FindControl("gvUpdatedOrderItems");
            BindGrid(gvUpdatedOrderItems, hdnRepOrderID.Value);
        }
    }

    protected void repUpdatedOrders_ItemCommand(Object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "OrderDetail")
        {
            Response.Redirect("~/OrderManagement/OrderDetail.aspx?id=" + Convert.ToString(e.CommandArgument), false);
        }
    }

    #endregion

    #region Methods

    private void sendShippingDetailEmail(Int64 OrderID, String orderNumber, Int64? userInfoID, String itemDetail)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;

            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Item Shipped";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                UserInformation objUserInformation = new UserInformationRepository().GetById(userInfoID);
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sToadd = objUserInformation.LoginEmail;
                String sSubject = "Your Order Shipping Information - (" + orderNumber + ")";
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{fullname}", objUserInformation.FirstName + " " + objUserInformation.LastName);
                messagebody.Replace("{OrderNo}", orderNumber);
                messagebody.Replace("{ItemDetail}", itemDetail);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(objUserInformation.UserInfoID, "Sync Information From PASS", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, OrderID);
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void BindRepeater()
    {
        if (OrderIDs != null && OrderIDs.Length > 0)
        {
            String[] arrOrderID = OrderIDs.ToString().Split(',');

            DataTable dtOrders = new DataTable();
            dtOrders.Columns.Add("OrderID");
            dtOrders.Columns.Add("OrderNumber");
            dtOrders.Columns.Add("Company");
            dtOrders.Columns.Add("FirstName");
            dtOrders.Columns.Add("LastName");

            foreach (String strOrder in arrOrderID)
            {
                String[] arrOrderDetails = strOrder.Split(':');
                DataRow drOrder = dtOrders.NewRow();

                for (Int16 i = 0; i < arrOrderDetails.Length; i++)
                {
                    drOrder[i] = !String.IsNullOrEmpty(arrOrderDetails[i]) ? arrOrderDetails[i] : "";
                }

                dtOrders.Rows.Add(drOrder);
            }
            dtOrders.AcceptChanges();

            repUpdatedOrders.DataSource = dtOrders;
            repUpdatedOrders.DataBind();

            lblRecords.Text = Convert.ToString(dtOrders.Rows.Count);

            dtOrders.Dispose();

            IncentexGlobal.ManageID = 9;

            lblMsg.Text = "Order Information Sync done Successfully.";
        }
        else
        {
            dvDetail.Visible = false;
            lblMsg.Text = "No orders to sync in this date range.";
        }
    }

    private void BindGrid(GridView gvUpdatedOrderItems, String OrderID)
    {
        String[] arrOrders = ShippedInThisSync.ToString().Split(',');

        DataTable dtItems = new DataTable();
        dtItems.Columns.Add("OrderID");
        dtItems.Columns.Add("ProdDesc");
        dtItems.Columns.Add("OrderQty");
        dtItems.Columns.Add("ShippedQty");
        dtItems.Columns.Add("AlreadyShippedQty");

        List<String> lstItems = arrOrders.Where(le => le.StartsWith(OrderID + ":")).ToList();

        foreach (String strOrder in lstItems)
        {
            String[] Details = strOrder.Split(':');
            if (Details.Length == 5)
            {
                DataRow drItem = dtItems.NewRow();
                for (Int16 i = 0; i < Details.Length; i++)
                {
                    drItem[i] = !String.IsNullOrEmpty(Details[i]) ? Details[i] : "";
                }
                dtItems.Rows.Add(drItem);
            }
        }

        dtItems.AcceptChanges();

        gvUpdatedOrderItems.DataSource = dtItems;
        gvUpdatedOrderItems.DataBind();

        dtItems.Dispose();
    }

    #endregion
}