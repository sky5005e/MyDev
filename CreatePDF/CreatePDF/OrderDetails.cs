using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Data;

namespace CreatePDF
{
    public class InvoiceInfo
    {


        /// <summary>
        /// Get Ship To XML string
        /// </summary>
        /// <param name="objOrder">OrderDetails object</param>
        /// <returns>XML String </returns>
        public String GetShipToXMLstring(OrderDetails objOrder)
        {

            String xmlString = String.Empty;

            if (objOrder != null)
            {
                xmlString = @"<?xml version='1.0' encoding='UTF-8' ?>" +
                        @"<shiptos>" +
                                    @"<shipto customer_no='" + objOrder.INVCustomer + "' shipto_no='" + objOrder.INVOrderNumber + "'>" +
                                        @"<shipto_name>" + CommonCls.HtmlEncode(objOrder.ShipName) + "</shipto_name>" +
                                        @"<address>" +
                                                    @"<attention>" + CommonCls.HtmlEncode(objOrder.ShipName) + "</attention>" +
                                                    @"<address1>" + CommonCls.HtmlEncode(objOrder.ShipAddr1) + "</address1>" +
                                                    @"<address2>" + CommonCls.HtmlEncode(objOrder.ShipAddr2) + (objOrder.ShipAddr3 == "" ? "" : ", " + CommonCls.HtmlEncode(objOrder.ShipAddr3)) + (objOrder.ShipAddr4 == "" ? "" : ", " + CommonCls.HtmlEncode(objOrder.ShipAddr4)) + "  </address2>" +
                                                    @"<postal_zip>" + CommonCls.HtmlEncode(objOrder.ShipZip) + "</postal_zip>" +
                                                    @"<province_state>" + CommonCls.HtmlEncode(objOrder.ShipState) + "</province_state>" +
                                                    @"<country>" + CommonCls.HtmlEncode(objOrder.ShipCountry) + "</country>" +
                                                    @"<city>" + CommonCls.HtmlEncode(objOrder.ShipCity) + "</city>" +
                                                    @"<telephone>" + CommonCls.HtmlEncode(objOrder.ShipPhone) + "</telephone>" +
                                                    @"<fax>" + CommonCls.HtmlEncode(objOrder.ShipFax) + "</fax>" +
                                                    @"<email>" + CommonCls.HtmlEncode(objOrder.ShipEmail) + "</email>" +
                                            @"</address>" +
                                    @"</shipto>" +
                            @"</shiptos>";
            }

            return xmlString;
        }

        /// <summary>
        /// Get Customer XML string
        /// </summary>
        /// <param name="objOrder">OrderDetails object</param>
        /// <returns>XML String </returns>
        public String GetCustomerXMLstring(OrderDetails objOrder)
        {

            String xmlString = String.Empty;

            if (objOrder != null)
            {
                xmlString = @"<?xml version='1.0' encoding='UTF-8' ?>" +
                        @"<customers>" +
                                    @"<customer customer_no='" + objOrder.INVCustomer + "'>" +
                                        @"<customer_name>" + CommonCls.HtmlEncode(objOrder.BilName) + "</customer_name>" +
                                        @"<address>" +
                                                    @"<attention>" + CommonCls.HtmlEncode(objOrder.BilName) + "</attention>" +
                                                    @"<address1>" + CommonCls.HtmlEncode(objOrder.BilAddr1) + "</address1>" +
                                                    @"<address2>" + CommonCls.HtmlEncode(objOrder.BilAddr2) + (objOrder.BilAddr3 == "" ? "-" : ", " + CommonCls.HtmlEncode(objOrder.BilAddr3)) + ", " + (objOrder.BilAddr4 == "" ? "-" : ", " + CommonCls.HtmlEncode(objOrder.BilAddr4)) + "  </address2>" +
                                                    @"<postal_zip>" + CommonCls.HtmlEncode(objOrder.BilZip) + "</postal_zip>" +
                                                    @"<province_state>" + CommonCls.HtmlEncode(objOrder.BilState) + "</province_state>" +
                                                    @"<country>" + CommonCls.HtmlEncode(objOrder.BilCountry) + "</country>" +
                                                    @"<city>" + CommonCls.HtmlEncode(objOrder.BilCity) + "</city>" +
                                                    @"<telephone>" + CommonCls.HtmlEncode(objOrder.BilPhone) + "</telephone>" +
                                                    @"<fax>" + CommonCls.HtmlEncode(objOrder.BilFax) + "</fax>" +
                                                    @"<email>" + CommonCls.HtmlEncode(objOrder.BilEmail) + "</email>" +
                                            @"</address>" +
                                    @"</customer>" +
                            @"</customers>";
            }

            return xmlString;
        }

        /// <summary>
        /// Get Order XML string
        /// </summary>
        /// <param name="objOrder">OrderDetails object</param>
        /// <returns>XML String </returns>
        public String GetOrderXMLstring(OrderDetails objOrder)
        {

            String xmlString = String.Empty;

            if (objOrder != null)
            {
                xmlString = @"<?xml version='1.0' encoding='UTF-8' ?>" +
                        @"<orders>" +
                                    @"<order order_id='" + objOrder.INVOrderTrackingID + "'>" +
                                    @"<bill_to>" + objOrder.INVCustomer + "</bill_to>" +
                                    @"<ship_to>" + objOrder.INVOrderNumber + "</ship_to>";

                String orderItems = String.Empty;

                foreach (var items in objOrder.OrderItems)
                {
                    orderItems += @"<item part_no='" + CommonCls.HtmlEncode(items.ItemPartNo) + "'>" +
                                    @"<quantity>" + items.ItemShipped + "</quantity>" +
                                    @"<price>" + items.ItemInvMisc + "7</price>" +
                                    @"</item>";
                }



                xmlString = xmlString + orderItems +
                            @"<date_wanted>" + objOrder.INVShipDate + "</date_wanted>" +
                            @"<order_date>" + objOrder.INVOrderDate + "</order_date>" +
                            @"<order_notes>" + CommonCls.HtmlEncode(objOrder.INVDesc) + " " + CommonCls.HtmlEncode(objOrder.INVOrderComent) + " " + CommonCls.HtmlEncode(objOrder.INVDeliveryIns) + " </order_notes>" +
                          @"</order>" +
                    @"</orders>";
            }

            return xmlString;
        }

        /// <summary>
        /// Genearete Pdf Invoice
        /// </summary>
        /// <param name="objOrder">OrderDetails object</param>
        /// <returns>bytes </returns>
        public void GeneratePdfInvoice(String path, OrderDetails objOrder)
        {
            new InvoiceTemplates().GenerateInvoicePDF(path, objOrder);
        }

        public OrderDetails GetInvoiceDetails(String orderNumber)
        {
            //String filePath = MainDir + "file" + DateTime.Now.Ticks + ".xml";
            OrderDetails objOrder = new OrderDetails();
            try
            {
                DataAccess objDataAccess = new DataAccess();
                String selectQry = "SELECT * FROM Vw_OrderInvoice  WHERE INVOrderNumber = '" + orderNumber + "'";//ORD0011279'";
                DataSet ds = objDataAccess.GetTableDisconnect_Connection(selectQry);

                List<OrderItems> lstOrderItems = new List<OrderItems>();
                Int32 recordsCount = ds.Tables[0].Rows.Count;

                for (int i = 0; i < recordsCount; i++)
                {

                    OrderItems objOrderItem = new OrderItems();
                    objOrderItem.ExtendedPrice = Convert.ToString(ds.Tables[0].Rows[i]["ExtendedPrice"]).Trim();
                    objOrderItem.ItemAutoDate = Convert.ToString(ds.Tables[0].Rows[i]["ItemAutoDate"]).Trim();
                    objOrderItem.ItemInvMisc = Convert.ToString(ds.Tables[0].Rows[i]["ItemInvMisc"]).Trim();
                    objOrderItem.ItemPartNo = Convert.ToString(ds.Tables[0].Rows[i]["ItemPartNo"]).Trim();
                    objOrderItem.ItemDesc = Convert.ToString(ds.Tables[0].Rows[i]["ItemDesc"]).Trim();
                    objOrderItem.ItemPartNumberDesc = Convert.ToString(ds.Tables[0].Rows[i]["ItemPartNumberDesc"]).Trim();
                    objOrderItem.ItemQtyBackOrdered = Convert.ToString(ds.Tables[0].Rows[i]["ItemQtyBackOrdered"]).Trim();
                    objOrderItem.ItemQtyOrdered = Convert.ToString(ds.Tables[0].Rows[i]["ItemQtyOrdered"]).Trim();
                    objOrderItem.ItemShipped = Convert.ToString(ds.Tables[0].Rows[i]["ItemShipped"]).Trim();
                    objOrderItem.ItemUniq = Convert.ToString(ds.Tables[0].Rows[i]["ItemUniq"]).Trim();
                    objOrderItem.ItemUnitPrice = Convert.ToString(ds.Tables[0].Rows[i]["ItemUnitPrice"]).Trim();

                    lstOrderItems.Add(objOrderItem);

                }

                if (recordsCount > 0)
                {

                    objOrder.BilName = Convert.ToString(ds.Tables[0].Rows[0]["BilName"]).Trim();
                    objOrder.BilAddr1 = Convert.ToString(ds.Tables[0].Rows[0]["BilAddr1"]).Trim();
                    objOrder.BilAddr2 = Convert.ToString(ds.Tables[0].Rows[0]["BilAddr2"]).Trim();
                    objOrder.BilAddr3 = Convert.ToString(ds.Tables[0].Rows[0]["BilAddr3"]).Trim();
                    objOrder.BilAddr4 = Convert.ToString(ds.Tables[0].Rows[0]["BilAddr4"]).Trim();
                    objOrder.BilCity = Convert.ToString(ds.Tables[0].Rows[0]["BilCity"]).Trim();
                    objOrder.BilCountry = Convert.ToString(ds.Tables[0].Rows[0]["BilCountry"]).Trim();
                    objOrder.BilEmail = Convert.ToString(ds.Tables[0].Rows[0]["BilEmail"]).Trim();
                    objOrder.BilFax = Convert.ToString(ds.Tables[0].Rows[0]["BilFax"]).Trim();
                    objOrder.BilZip = Convert.ToString(ds.Tables[0].Rows[0]["BilZip"]).Trim();
                    objOrder.BilPhone = Convert.ToString(ds.Tables[0].Rows[0]["BilPhone"]).Trim();
                    objOrder.BilState = Convert.ToString(ds.Tables[0].Rows[0]["BilState"]).Trim();

                    objOrder.ShipName = Convert.ToString(ds.Tables[0].Rows[0]["ShipName"]).Trim();
                    objOrder.ShipAddr1 = Convert.ToString(ds.Tables[0].Rows[0]["ShipAddr1"]).Trim();
                    objOrder.ShipAddr2 = Convert.ToString(ds.Tables[0].Rows[0]["ShipAddr2"]).Trim();
                    objOrder.ShipAddr3 = Convert.ToString(ds.Tables[0].Rows[0]["ShipAddr3"]).Trim();
                    objOrder.ShipAddr4 = Convert.ToString(ds.Tables[0].Rows[0]["ShipAddr4"]).Trim();
                    objOrder.ShipCity = Convert.ToString(ds.Tables[0].Rows[0]["ShipCity"]).Trim();
                    objOrder.ShipCountry = Convert.ToString(ds.Tables[0].Rows[0]["ShipCountry"]).Trim();
                    objOrder.ShipEmail = Convert.ToString(ds.Tables[0].Rows[0]["ShipEmail"]).Trim();
                    objOrder.ShipFax = Convert.ToString(ds.Tables[0].Rows[0]["ShipFax"]).Trim();
                    objOrder.ShipZip = Convert.ToString(ds.Tables[0].Rows[0]["ShipZip"]).Trim();
                    objOrder.ShipPhone = Convert.ToString(ds.Tables[0].Rows[0]["ShipPhone"]).Trim();
                    objOrder.ShipState = Convert.ToString(ds.Tables[0].Rows[0]["ShipState"]).Trim();

                    objOrder.INVOrderTrackingID = Convert.ToString(ds.Tables[0].Rows[0]["INVOrderTrackingID"]).Trim();
                    objOrder.INVViaDesc = Convert.ToString(ds.Tables[0].Rows[0]["INVViaDesc"]).Trim();
                    objOrder.INVCustomer = Convert.ToString(ds.Tables[0].Rows[0]["INVCustomer"]).Trim();
                    objOrder.INVOrderNumber = Convert.ToString(ds.Tables[0].Rows[0]["INVOrderNumber"]).Trim();
                    objOrder.INVMisc = Convert.ToString(ds.Tables[0].Rows[0]["INVMisc"]).Trim();
                    objOrder.INVSubTotal = Convert.ToString(ds.Tables[0].Rows[0]["INVSubTotal"]).Trim();
                    objOrder.INVTotalAmount = Convert.ToString(ds.Tables[0].Rows[0]["INVTotalAmount"]).Trim();
                    objOrder.INVTaxTotal = Convert.ToString(ds.Tables[0].Rows[0]["INVTaxTotal"]).Trim();
                    objOrder.INVDiscAmount = Convert.ToString(ds.Tables[0].Rows[0]["INVDiscAmount"]).Trim();
                    objOrder.INVNetTax = Convert.ToString(ds.Tables[0].Rows[0]["INVNetTax"]).Trim();
                    objOrder.INVNumber = Convert.ToString(ds.Tables[0].Rows[0]["INVNumber"]).Trim();
                    objOrder.INVDate = Convert.ToString(ds.Tables[0].Rows[0]["INVDate"]).Trim();
                    objOrder.INVOrderDate = Convert.ToString(ds.Tables[0].Rows[0]["INVOrderDate"]).Trim();
                    objOrder.INVShipDate = Convert.ToString(ds.Tables[0].Rows[0]["INVShipDate"]).Trim();
                    objOrder.INVDesc = Convert.ToString(ds.Tables[0].Rows[0]["INVDesc"]).Trim();
                    objOrder.INVDeliveryIns = Convert.ToString(ds.Tables[0].Rows[0]["INVDeliveryIns"]).Trim();

                    // Add all the items list
                    objOrder.OrderItems = lstOrderItems;

                }

                //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(OrderDetails));
                //serializer.Serialize(System.IO.File.Create(@"D:\CRM\CRM\WWWRoot\CustomPages\OrderTracking\CatalystXML\" + "file" + DateTime.Now.Ticks + ".xml"), objOrder);
            }
            catch (Exception ex)
            {
                CommonCls.ErrorMessage(ex, @"D:\CRM\CRM\WWWRoot\CustomPages\OrderTracking\CatalystXML\" + "error" + DateTime.Now.Ticks + ".txt");
            }

            return objOrder;
        }



    }

    public class OrderDetails
    {

        public String BilAddr1 { get; set; }
        public String BilAddr2 { get; set; }
        public String BilAddr3 { get; set; }
        public String BilAddr4 { get; set; }
        public String BilCity { get; set; }
        public String BilCountry { get; set; }
        public String BilName { get; set; }
        public String BilEmail { get; set; }
        public String BilFax { get; set; }
        public String BilZip { get; set; }
        public String BilPhone { get; set; }
        public String BilState { get; set; }

        public String ShipName { get; set; }
        public String ShipAddr1 { get; set; }
        public String ShipAddr2 { get; set; }
        public String ShipAddr3 { get; set; }
        public String ShipAddr4 { get; set; }
        public String ShipCity { get; set; }
        public String ShipCountry { get; set; }
        public String ShipEmail { get; set; }
        public String ShipFax { get; set; }
        public String ShipZip { get; set; }
        public String ShipPhone { get; set; }
        public String ShipState { get; set; }

        public String INVOrderTrackingID { get; set; }
        public String INVViaDesc { get; set; }
        public String INVCustomer { get; set; }
        public String INVOrderNumber { get; set; }
        public String INVMisc { get; set; }
        public String INVSubTotal { get; set; }
        public String INVTotalAmount { get; set; }
        public String INVTaxTotal { get; set; }
        public String INVDiscAmount { get; set; }
        public String INVNetTax { get; set; }
        public String INVNumber { get; set; }
        public String INVDate { get; set; }
        public String INVOrderDate { get; set; }
        public String INVShipDate { get; set; }
        public String INVDesc { get; set; }
        public String INVOrderComent { get; set; }
        public String INVDeliveryIns { get; set; }

        public List<OrderItems> OrderItems { get; set; }

    }

    public class OrderItems
    {
        public String ItemUniq { get; set; }
        public String ItemQtyBackOrdered { get; set; }
        public String ItemQtyOrdered { get; set; }
        public String ItemShipped { get; set; }
        public String ItemInvMisc { get; set; }
        public String ItemPartNo { get; set; }
        public String ItemDesc { get; set; }
        public String ItemPartNumberDesc { get; set; }
        public String ItemUnitPrice { get; set; }
        public String ExtendedPrice { get; set; }
        public String ItemAutoDate { get; set; }
    }



}
