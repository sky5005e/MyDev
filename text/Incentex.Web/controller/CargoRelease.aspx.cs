using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Xml;
//this is needed to navigate thru the XML
using System.Xml.XPath;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using MagayaService;

public partial class controller_CargoRelease : System.Web.UI.Page
{
    #region Data Member
    OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();
    // this is the general schema declaration at all xmls from magaya
    private const String xmlschema = "xmlns=\"http://www.magaya.com/XMLSchema/V1\"";
    // posible operation tracked by reading the transaction log
    private const Int32 LogItemCreation = 0x01;
    private const Int32 LogItemDeletion = 0x02;
    private const Int32 LogItemEdition = 0x04;
    private const Int32 flag = 0x20;
    DataSet ds = new DataSet();

    CSSoapService cSSoapService = new CSSoapService();
    Int32 AccKey;
    String user = ConfigurationSettings.AppSettings["MagayaUser"];
    String pass = ConfigurationSettings.AppSettings["MagayaPassword"];
    String TransType = "CR";
    String xmlTrans = String.Empty;
    api_session_error Result;
    String tXMl = String.Empty;
    #endregion

    #region Events
    protected void Page_Load(Object sender, EventArgs e)
    {
        try
        {
            ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
            //Get Supplier id of wtdc
            Supplier objsup = new SupplierRepository().GetSinglSupplierid("WTDC");
            Int32 supplierid = Convert.ToInt32(objsup.SupplierID);

            Result = cSSoapService.StartSession(user, pass, out AccKey);

            // you need to check if everythig is ok 
            Response.Write("Session start<br/>");
            if (Result == api_session_error.no_error)
            {

                //Result = cSSoapService.QueryLog(AccKey, "2012-03-11T00:13:22+05:30", "2012-03-21T00:13:22+05:30", LogItemCreation | LogItemEdition | LogItemDeletion, TransType, 0, out tXMl);
                Result = cSSoapService.QueryLog(AccKey, ConvertTimeToDatabaseFormat(DateTime.Now.AddMinutes(Convert.ToInt32(ConfigurationSettings.AppSettings["MagayStartMinute"]))), ConvertTimeToDatabaseFormat(DateTime.Now), LogItemCreation | LogItemEdition | LogItemDeletion, TransType, 0, out tXMl);
                Response.Write("Getting GUID<br/>");
                // this will create a navigator to traverse the XML
                XPathNavigator root = Create(tXMl);

                if (root != null)
                {
                    Response.Write("Root not null <br/>");

                    // this will give the list of GUIDs present at the XML result from QueryLog
                    List<String> GUIDs = GetTransactionList(root);

                    foreach (String guid in GUIDs)
                    {
                        Result = cSSoapService.GetTransaction(AccKey, TransType, 0, guid, out tXMl);

                        Response.Write("Get Transaction<br/>");

                        if (Result == api_session_error.no_error)
                        {
                            Response.Write("Get transaction with no error<br/>");

                            CargoReleaseClass CR = new CargoReleaseClass(tXMl);
                            if (CR.Items.Count != 0 && CR.ReleasedToType == "Client" && CR.ReleasedToName.ToString().Contains("Incentex") && CR.CarrierTrackingNumber != null && CR.CarrierTrackingNumber != "")
                            {
                                Response.Write("Object Created and client is incentex<br/>");
                                //for (Int32 i = 0; i <= CR.Items.Count - 1; i++)
                                for (Int32 i = 0; i <= CR.Items[0].ContainedItems.Count - 1; i++)
                                {
                                    try
                                    {
                                        DateTime CreatedOn = Convert.ToDateTime(CR.CreatedOn);
                                        String Number = Convert.ToString(CR.Number);
                                        String CarrierName = Convert.ToString(CR.CarrierName);
                                        String CarrierTrackingNumber = Convert.ToString(CR.CarrierTrackingNumber);

                                        String PieceQuantity = String.Empty;
                                        String CargoReleaseNumber = String.Empty;
                                        String PartNumber = String.Empty;
                                        String Pieces = String.Empty;
                                        String SalesOrderNumber = String.Empty;

                                        if (CR.Items[0].ContainedItems.Count > 0)
                                        {
                                            PieceQuantity = Convert.ToString(CR.Items[0].ContainedItems[i].PieceQuantity);
                                            CargoReleaseNumber = Convert.ToString(CR.Items[0].ContainedItems[i].CargoReleaseNumber);
                                            PartNumber = Convert.ToString(CR.Items[0].ContainedItems[i].PartNumber);
                                            Pieces = Convert.ToString(CR.Items[0].ContainedItems[i].Pieces);
                                            SalesOrderNumber = Convert.ToString(CR.Items[0].ContainedItems[i].SalesOrderNumber);
                                        }
                                        else
                                        {
                                            PieceQuantity = Convert.ToString(CR.Items[0].PieceQuantity);
                                            CargoReleaseNumber = Convert.ToString(CR.Items[0].CargoReleaseNumber);
                                            PartNumber = Convert.ToString(CR.Items[0].PartNumber);
                                            Pieces = Convert.ToString(CR.Items[0].Pieces);
                                            SalesOrderNumber = Convert.ToString(CR.Items[0].SalesOrderNumber);
                                        }

                                        Order objOrder = new OrderConfirmationRepository().GetByOrderNo(SalesOrderNumber);
                                        if (objOrder != null)
                                        {
                                            Response.Write("Order available on world-link " + objOrder.OrderNumber + "<br/>");
                                            if (objOrder != null && objOrder.OrderStatus.ToUpper() != "CLOSED" && objOrder.OrderStatus.ToUpper() != "DELETED")
                                            {
                                                Response.Write("Order available on world-link and status is not Closed and Deleted<br/>");
                                                String TrackingNumber = String.Empty;
                                                DateTime ShippingDate = DateTime.Now;
                                                Int64 ShipperService = 0;
                                                if (Number == CargoReleaseNumber)
                                                {
                                                    TrackingNumber = CarrierTrackingNumber;
                                                    ShippingDate = CreatedOn;
                                                    ShipperService = Convert.ToInt64(new LookupRepository().GetIdByLookupNameNLookUpCode(CarrierName, "Shipping Type"));
                                                }

                                                ShipingOrder objShiporder = new ShipingOrder();
                                                objShiporder.ShipingDate = ShippingDate;
                                                objShiporder.TrackingNo = TrackingNumber;
                                                objShiporder.PackageId = "Shipment_" + System.DateTime.Now.Ticks;
                                                objShiporder.ShipperService = ShipperService;
                                                //shipped qty by wtdc
                                                Decimal shippedQty = Convert.ToDecimal(PieceQuantity);
                                                objShiporder.ShipQuantity = Convert.ToInt64(shippedQty);
                                                objShiporder.NoOfBoxes = 1;

                                                objShiporder.IsShipped = true;
                                                objShiporder.SupplierId = supplierid;

                                                objShiporder.UpdateBy = objOrder.UserId;
                                                objShiporder.OrderID = objOrder.OrderID;
                                                Int64 strMyShoppingCartID = 0;
                                                Int64 TotalItemOrderQty = 0;
                                                Int64 TotalOrderQty = 0;
                                                Int64 RemainingOrderQty = 0;

                                                if (objOrder.OrderFor == "ShoppingCart")
                                                {
                                                    List<SelectMyShoppingCartProductResult> obj = new MyShoppingCartRepository().SelectCurrentOrderProducts(objOrder.OrderID);
                                                    for (Int32 k = 0; k < obj.Count; k++)
                                                    {
                                                        TotalOrderQty += Convert.ToInt64(obj[k].Quantity);
                                                        if (obj[k].item.Replace(" ", "") == PartNumber)
                                                        {
                                                            strMyShoppingCartID = obj[k].MyShoppingCartID;
                                                            TotalItemOrderQty = Convert.ToInt64(obj[k].Quantity);
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
                                                            if (objFinal[k].item.Replace(" ", "") == PartNumber)
                                                            {
                                                                strMyShoppingCartID = objFinal[k].MyIssuanceCartID;
                                                                TotalItemOrderQty = Convert.ToInt64(objFinal[k].Qty);
                                                            }
                                                        }
                                                    }
                                                }
                                                CountTotalShippedQuantiyResult objTotalItemShippedQty = new ShipOrderRepository().GetQtyShippedTotal(Convert.ToInt32(strMyShoppingCartID), PartNumber, objOrder.OrderID);
                                                //remaining order total qty
                                                RemainingOrderQty = TotalItemOrderQty - Convert.ToInt64(shippedQty) - (objTotalItemShippedQty.ShipQuantity != null ? Convert.ToInt64(objTotalItemShippedQty.ShipQuantity) : 0);
                                                objShiporder.QtyOrder = TotalItemOrderQty;
                                                objShiporder.RemaingQutOrder = RemainingOrderQty;
                                                objShiporder.MyShoppingCartiD = strMyShoppingCartID;
                                                objShiporder.ItemNumber = PartNumber;

                                                //change order item status based on remaining order item qty
                                                if (RemainingOrderQty == 0)
                                                    objShiporder.ShippingOrderStatus = "Shipped Complete";
                                                else
                                                    objShiporder.ShippingOrderStatus = "Partial Shipped";

                                                //Check if item already process with same tracking number,item and order
                                                if (strMyShoppingCartID != 0 && objShipOrderRepos.CheckIfCargoAlreadyProcess(objOrder.OrderID, strMyShoppingCartID, TrackingNumber).Count == 0)
                                                {
                                                    objShipOrderRepos.Insert(objShiporder);
                                                    objShipOrderRepos.SubmitChanges();


                                                    //Update order table status
                                                    Boolean IsOrderShippedComplete = new ShipOrderRepository().IsOrderShippedComplete(objOrder.OrderID);
                                                    if (IsOrderShippedComplete)
                                                        new OrderConfirmationRepository().UpdateStatus(objOrder.OrderID, "Closed");

                                                    Response.Write("Data inserted successfully.<br/>");
                                                    //update inventory of item
                                                    try
                                                    {
                                                        Int32 inventory = Convert.ToInt32(Pieces);
                                                        ProductItemDetailsRepository objProductItemDetailsRepository = new ProductItemDetailsRepository();
                                                        Int32 ProdItemId = Convert.ToInt32(objProductItemDetailsRepository.GetProductItemIdByItemNumber(objShiporder.ItemNumber, Convert.ToInt64(objOrder.StoreID), Convert.ToInt64(objOrder.WorkgroupId), supplierid));
                                                        objProductItemDetailsRepository.UpdateInventoryByMagaya(ProdItemId, inventory);
                                                        Response.Write("Inventory updated successfully.<br/>");
                                                    }
                                                    catch { }
                                                }
                                            }
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
            }
            Result = cSSoapService.EndSession(AccKey);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            Result = cSSoapService.EndSession(AccKey);
        }
    }
    #endregion

    #region Method

    private static List<String> GetTransactionList(XPathNavigator GUIDs)
    {
        XPathNodeIterator Items = GUIDs.Select("GUIDItems/GUIDItem");
        List<String> Transactions = new List<String>();

        while (Items.MoveNext())
        {
            XPathNavigator Item = Items.Current.Clone();
            XPathNavigator ItemGUID = Item.SelectSingleNode("GUID");

            if (ItemGUID != null)
            {
                if (!Transactions.Exists(delegate(String s) { return s == ItemGUID.Value; }))
                    Transactions.Add(ItemGUID.Value);
            }
        }

        return Transactions;
    }

    #region Convert XML To Dataset
    public DataSet ConvertXMLToDataSet(String xmlData)
    {
        StringReader stream = null;
        XmlTextReader reader = null;
        try
        {
            DataSet xmlDS = new DataSet();
            stream = new StringReader(xmlData.Replace("</Street><Street>", ""));
            // Load the XmlTextReader from the stream
            reader = new XmlTextReader(stream);

            xmlDS.ReadXml(reader);
            return xmlDS;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (reader != null) reader.Close();
        }
    }

    /// <summary>
    /// Creates an XPathNavigator from an XML String
    /// </summary>
    /// <param name="xml">XML String</param>
    /// <returns>XPathNavigator</returns>
    public static XPathNavigator Create(String xml)
    {
        if (String.IsNullOrEmpty(xml))
            return null;

        xml = xml.Replace(xmlschema, "");

        try
        {
            StringReader stringReader = new StringReader(xml);
            XmlTextReader text = new XmlTextReader(stringReader);
            XPathDocument reader = new XPathDocument(text);
            return reader.CreateNavigator();
        }
        catch
        {
            return null;
        }
    }
    #endregion

    public static String ConvertTimeToDatabaseFormat(DateTime datetime)
    {
        return datetime.ToString("yyyy-MM-dd") + "T" + datetime.ToString("HH:mm:sszzz");
    }

    #region Email Notification
    private void SendEmail()
    {
        using (MailMessage objEmail = new MailMessage())
        {
            objEmail.Body = "The cargo release scheduler has run successfully for the day.";
            objEmail.From = new MailAddress("notifications@world-link.us.com", "Incentex");
            objEmail.IsBodyHtml = true;
            objEmail.Subject = "Cargo release scheduler - Notification";
            objEmail.To.Add(new MailAddress("ware-house@world-link.us.com"));

            SmtpClient objSmtp = new SmtpClient();
            objSmtp.EnableSsl = false;
            objSmtp.Credentials = new NetworkCredential("notifications@world-link.us.com", "5rdxZSE$");
            objSmtp.Host = Convert.ToString(Application["SMTPHOST"]);
            objSmtp.Port = Convert.ToInt32(Application["SMTPPORT"]);

            #region Test Credentials
            //objSmtp.EnableSsl = true;
            //objSmtp.Credentials = new NetworkCredential("smtp.incentex@gmail.com", "smtp@incentex");
            //objSmtp.Host = "smtp.gmail.com";
            //objSmtp.Port = 25;
            #endregion

            objSmtp.Send(objEmail);
        }
    }
    #endregion
    #endregion
}
