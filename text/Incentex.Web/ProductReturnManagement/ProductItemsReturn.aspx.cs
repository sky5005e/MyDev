using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Data;
using Incentex.DA;
using Incentex.BE;
using ShippingReference;
using AjaxControlToolkit;

public partial class ProductReturnManagement_ProductItemsReturn : PageBase
{
    #region Data Members
    Int64 OrderID
    {
        get
        {
            if (ViewState["OrderID"] == null)
            {
                ViewState["OrderID"] = 0;
            }
            return Convert.ToInt64(ViewState["OrderID"]);
        }
        set
        {
            ViewState["OrderID"] = value;
        }
    }
    String ReturnStatus
    {
        get
        {
            if (ViewState["ReturnStatus"] == null)
            {
                ViewState["ReturnStatus"] = null;
            }
            return Convert.ToString(ViewState["ReturnStatus"]);
        }
        set
        {
            ViewState["ReturnStatus"] = value;
        }
    }
    Int32 isExist
    {
        get
        {
            if (ViewState["isExist"] == null)
            {
                ViewState["isExist"] = 0;
            }
            return Convert.ToInt32(ViewState["isExist"]);
        }
        set
        {
            ViewState["isExist"] = value;
        }
    }
    Boolean IsAfterSave
    {
        get
        {
            if (ViewState["IsAfterSave"] == null)
            {
                ViewState["IsAfterSave"] = false;
            }
            return Convert.ToBoolean(ViewState["IsAfterSave"]);
        }
        set
        {
            ViewState["IsAfterSave"] = value;
        }
    }
    /// <summary>
    /// Set true when user is allow for free shipping return
    /// </summary>
    Boolean IsAllowShippingReturnLabel
    {
        get
        {
            if (ViewState["IsAllowShippingReturnLabel"] == null)
            {
                ViewState["IsAllowShippingReturnLabel"] = false;
            }
            return Convert.ToBoolean(ViewState["IsAllowShippingReturnLabel"]);
        }
        set
        {
            ViewState["IsAllowShippingReturnLabel"] = value;
        }
    }

    /// <summary>
    /// Set true when shipped quantity is more then return qty
    /// </summary>
    Boolean IsAllowToAddMoreRow
    {
        get
        {
            if (ViewState["IsAllowToAddMoreRow"] == null)
            {
                ViewState["IsAllowToAddMoreRow"] = false;
            }
            return Convert.ToBoolean(ViewState["IsAllowToAddMoreRow"]);
        }
        set
        {
            ViewState["IsAllowToAddMoreRow"] = value;
        }
    }
    /// <summary>
    /// Set the total return quantity
    /// </summary>
    Int32 TotalReturnQty
    {
        get
        {
            if (ViewState["TotalReturnQty"] == null)
            {
                ViewState["TotalReturnQty"] = 0;
            }
            return Convert.ToInt32(ViewState["TotalReturnQty"]);
        }
        set
        {
            ViewState["TotalReturnQty"] = value;
        }
    }
    /// <summary>
    /// This will set when the row command
    /// </summary>
    String rowCmdArgString
    {
        get
        {
            if (ViewState["rowCmdArgString"] == null)
            {
                ViewState["rowCmdArgString"] = null;
            }
            return ViewState["rowCmdArgString"].ToString();
        }
        set
        {
            ViewState["rowCmdArgString"] = value;
        }
    }
    OrderDetailHistoryRepository ObjOrderRepo = new OrderDetailHistoryRepository();
    ShipOrderRepository OrdShippOrder = new ShipOrderRepository();
    ProductReturnRepository ObjOrderReturnRepo = new ProductReturnRepository();
    CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfo();
    CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfo();
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
    LookupRepository objLookRep = new LookupRepository();
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    LookupDA sReasonCode = new LookupDA();
    LookupBE sReasonCodeBE = new LookupBE();

    
    #endregion

    #region Page Event's
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack|| IsAfterSave)
        {
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Order Detail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/MyTrackingCenter.aspx";
            if (!String.IsNullOrEmpty(Request.QueryString["orderid"]))
            {
                this.OrderID = Convert.ToInt64(Request.QueryString["orderid"]);
                hdORderID.Value = Request.QueryString["orderid"].ToString();
            }
            this.lblProcessDate.Text = "";
            this.lblStatus.Text = "";
            BindOrderSummaryGridView();
            tblReturnRequest.Visible = false;
            //BindGridView();
            bindRepeater();
            IsAfterSave = false;

            String FreeShippingValue = new PreferenceRepository().GetUserPreferenceValue(IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.Usertype, "FreeReturnShipping");

            if (FreeShippingValue == "1")
                IsAllowShippingReturnLabel = true;
        }
    }

    protected void lnkBtnSubmitReturn_Click(object sender, EventArgs e)
    {
        List<ReturnProduct> objReturnProducts = new List<ReturnProduct>(); 
        List<ShipingOrder> objList = new List<ShipingOrder>();
        objList = OrdShippOrder.GetAllShippOrder(OrderID);
        foreach (GridViewRow t in gvOrderDetail.Rows)
        {
            ReturnProduct objReturnProduct = new ReturnProduct();
            TextBox txtReturnQty = (TextBox)t.FindControl("txtReturnQty");
            DropDownList ddlReasonCode = (DropDownList)t.FindControl("ddlReasonCode");
            HiddenField hdnMyShoppingCartid = (HiddenField)t.FindControl("hdnMyShoppingCartiD");
            DropDownList ddlRequest = (DropDownList)t.FindControl("ddlRequesting");
            Label lblQtyOrder = (Label)t.FindControl("lblQtyOrder");
            //HiddenField hdnProductReturnId = (HiddenField)t.FindControl("hdnProductReturnId");
            //if (hdnProductReturnId.Value != null && hdnProductReturnId.Value != "0")
            //{
            //    objReturnProduct = ObjOrderReturnRepo.GetById(Convert.ToInt64(hdnProductReturnId.Value));
            //    objReturnProduct.ReasonId = Convert.ToInt64(ddlReasonCode.SelectedValue);
            //    if (!(string.IsNullOrEmpty(txtReturnQty.Text)))
            //        objReturnProduct.ReturnQty = Convert.ToInt64(txtReturnQty.Text);
            //    else
            //        objReturnProduct.ReturnQty = 0;
            //}
            //else
            //{
            if (txtReturnQty.Text == "" || Convert.ToInt32(txtReturnQty.Text) <= 0)
                continue;
            HiddenField hdnItemNumber = (HiddenField)t.FindControl("hdnitemNo");
            if (objList.Count > 0)
            {
                var itemShippDetail = objList.Where(m => m.ItemNumber == hdnItemNumber.Value).ToList();
                if (itemShippDetail != null && itemShippDetail.Count > 0)
                {
                    objReturnProduct.TrackingNumber = itemShippDetail[0].TrackingNo;
                    objReturnProduct.PackageID = itemShippDetail[0].PackageId;
                    objReturnProduct.ShippId = Convert.ToInt64(itemShippDetail[0].ShippID);
                }
                objReturnProduct.OrderId = Convert.ToInt64(OrderID);
                if (!(string.IsNullOrEmpty(txtReturnQty.Text)))
                    objReturnProduct.ReturnQty = Convert.ToInt64(txtReturnQty.Text);
                objReturnProduct.SubmitDate = Convert.ToDateTime(DateTime.Now.ToString());
                objReturnProduct.ItemNumber = hdnItemNumber.Value;
                objReturnProduct.MyShoppingCartID = Convert.ToInt64(hdnMyShoppingCartid.Value);
                objReturnProduct.ReasonId = Convert.ToInt64(ddlReasonCode.SelectedValue);
                objReturnProduct.Status = "Processing Return";
                objReturnProduct.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                ObjOrderReturnRepo.Insert(objReturnProduct);
                OrdShippOrder.UpDateShipOrderReturnStatus(Convert.ToInt32(itemShippDetail[0].ShippID), "1");
            }
        }
           // if (!(String.IsNullOrEmpty(txtReturnQty.Text)))
        ObjOrderReturnRepo.SubmitChanges();

        //}
        gvOrderDetail.DataSource = null;
        gvOrderDetail.DataBind();
        ViewState["CurrentTable"] = null;
        ViewState["PreviousNewTable"] = null;
        tblReturnRequest.Visible = false;

        dtlReturnRequest.Visible = true;
        divShowOrderDtls.Visible = true;
        bindRepeater();
        IsAfterSave = true;

        modalPrint.Show();

        if (IsAllowShippingReturnLabel)
            lnkPrintPrepaidLabel.Visible = true;
        else
            lnkPrintPrepaidLabel.Visible = false;
        //Page_Load(sender, e);


       
    }

    protected void LinPrint_Click(object sender, EventArgs e)
    {
        modalPrint.Show();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Int32 newRow = Convert.ToInt32(txtReturnQtyNo.Text);
        try
        {
            List<ProductReturnRepository.OrderReturnDetails> PreviousOrderList = GetCurrentGridViewList(rowCmdArgString.ToString(),';');

            List<ProductReturnRepository.OrderReturnDetails> OrderList = ObjOrderReturnRepo.GetAllShippingOrderDetaislByOrderID(this.OrderID);//(List<ProductReturnRepository.OrderReturnDetails>)ViewState["CurrentTable"];


            string[] ItemCollection = rowCmdArgString.ToString().Split(';');
            List<ProductReturnRepository.OrderReturnDetails> OrderItemNew = OrderList.Where(m => m.ItemNumber == ItemCollection[0] && m.ShippID == Convert.ToInt64(ItemCollection[1])).ToList();
            //string SelectedReasonString = "";

            List<ProductReturnRepository.OrderReturnDetails> AddNewRowList = new List<ProductReturnRepository.OrderReturnDetails>();
            for (Int32 i = 0; i < newRow; i++)
            {
                ProductReturnRepository.OrderReturnDetails OrderItem = new ProductReturnRepository.OrderReturnDetails();
                OrderItem.ItemNumber = OrderItemNew[0].ItemNumber;
                OrderItem.MyShoppingCartID = OrderItemNew[0].MyShoppingCartID;
                OrderItem.OrderID = OrderItemNew[0].OrderID;
                OrderItem.OrderStatus = ItemCollection[0] + "/" + i.ToString();
                OrderItem.ProductDescription = OrderItemNew[0].ProductDescription;
                OrderItem.Quantity = OrderItemNew[0].Quantity;
                OrderItem.ShippedQty = OrderItemNew[0].ShippedQty.ToString();
                OrderItem.StoreProductID = OrderItemNew[0].StoreProductID;
                OrderItem.UserInfoID = OrderItemNew[0].UserInfoID;
                OrderItem.ShippID = OrderItemNew[0].ShippID;
                OrderItem.ReasonCode = OrderItemNew[0].ReasonCode;
                AddNewRowList.Add(OrderItem);
            }

            PreviousOrderList.AddRange(AddNewRowList);
            ViewState["CurrentTable"] = PreviousOrderList;
            //AddGridViewRow(this.rowCmdArgString,i);
            gvOrderDetail.DataSource = PreviousOrderList;
            gvOrderDetail.DataBind();


            tblReturnRequest.Visible = true;

        }
        catch (Exception ex)
        {
        }
        finally
        {
            rowCmdArgString = null;
        }

    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            List<ProductReturnRepository.OrderReturnDetails> PreviousOrderList = GetCurrentGridViewList(rowCmdArgString.ToString(),';');
            
            List<ProductReturnRepository.OrderReturnDetails> OrderList = ObjOrderReturnRepo.GetAllShippingOrderDetaislByOrderID(this.OrderID);
            
            string[] ItemCollection = rowCmdArgString.ToString().Split(';');
            List<ProductReturnRepository.OrderReturnDetails> OrderItemNew = OrderList.Where(m => m.ItemNumber == ItemCollection[0] && m.ShippID == Convert.ToInt64(ItemCollection[1])).ToList();        

            List<ProductReturnRepository.OrderReturnDetails> AddNewRowList = new List<ProductReturnRepository.OrderReturnDetails>();
            ProductReturnRepository.OrderReturnDetails OrderItem = new ProductReturnRepository.OrderReturnDetails();
            OrderItem.ItemNumber = OrderItemNew[0].ItemNumber;
            OrderItem.MyShoppingCartID = OrderItemNew[0].MyShoppingCartID;
            OrderItem.OrderID = OrderItemNew[0].OrderID;
            OrderItem.OrderStatus = ItemCollection[0] + "/A";
            OrderItem.ProductDescription = OrderItemNew[0].ProductDescription;
            OrderItem.Quantity = OrderItemNew[0].Quantity;
            OrderItem.ShippedQty = OrderItemNew[0].ShippedQty.ToString();
            OrderItem.StoreProductID = OrderItemNew[0].StoreProductID;
            OrderItem.UserInfoID = OrderItemNew[0].UserInfoID;
            OrderItem.ShippID = OrderItemNew[0].ShippID;
            AddNewRowList.Add(OrderItem);

            PreviousOrderList.AddRange(AddNewRowList);
            ViewState["CurrentTable"] = PreviousOrderList;

            gvOrderDetail.DataSource = PreviousOrderList;
            gvOrderDetail.DataBind();

            tblReturnRequest.Visible = true;
        }
        catch (Exception ex)
        {
        }
        finally
        {
            rowCmdArgString = null;
        }

    }
    protected void lnkPrintPrepaidLabel_Click(object sender, EventArgs e)
    {
        
        String Message = String.Empty;
        try
        {
            ShipService shpSvc = new ShipService();
            ShipmentRequest shipmentRequest = new ShipmentRequest();
            UPSSecurity upss = new UPSSecurity();
            UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSSecurityServiceAccessToken();
            upssSvcAccessToken.AccessLicenseNumber = System.Configuration.ConfigurationSettings.AppSettings["UPSLicenceNumber"];
            upss.ServiceAccessToken = upssSvcAccessToken;
            UPSSecurityUsernameToken upssUsrNameToken = new UPSSecurityUsernameToken();
            upssUsrNameToken.Username = System.Configuration.ConfigurationSettings.AppSettings["UPSUserName"];
            upssUsrNameToken.Password = System.Configuration.ConfigurationSettings.AppSettings["UPSPassword"]; 
            upss.UsernameToken = upssUsrNameToken;
            shpSvc.UPSSecurityValue = upss;
            RequestType request = new RequestType();
            String[] requestOption = { "nonvalidate" };
            request.RequestOption = requestOption;
            shipmentRequest.Request = request;
            ShipmentType shipment = new ShipmentType();
            shipment.Description = "Ship webservice example";
            ShipperType shipper = new ShipperType();
            shipper.ShipperNumber = "9X91X1";
            PaymentInfoType paymentInfo = new PaymentInfoType();
            ShipmentChargeType shpmentCharge = new ShipmentChargeType();
            BillShipperType billShipper = new BillShipperType();
            billShipper.AccountNumber = "9X91X1";
            shpmentCharge.BillShipper = billShipper;
            shpmentCharge.Type = "01";
            ShipmentChargeType[] shpmentChargeArray = { shpmentCharge };
            paymentInfo.ShipmentCharge = shpmentChargeArray;
            shipment.PaymentInformation = paymentInfo;
           
            // Shipper Address ie Form Address 
            objShippingInfo = objCmpEmpContRep.GetShippingDetailByOrderId(this.OrderID);

            ShippingReference.ShipAddressType shipperAddress = new ShippingReference.ShipAddressType();
            String[] addressLine = { objShippingInfo.Address + ", " + objState.GetById((long)objShippingInfo.StateID).sStatename};
            shipperAddress.AddressLine = addressLine;
            shipperAddress.City = objCity.GetById((long)objShippingInfo.CityID).sCityName;
            shipperAddress.PostalCode = objShippingInfo.ZipCode;
            shipperAddress.StateProvinceCode = objState.GetStateProvinceCodebyStateId((long)objShippingInfo.StateID).ToString();
            shipperAddress.CountryCode = "US";
            shipperAddress.AddressLine = addressLine;
            shipper.Address = shipperAddress;
            shipper.Name = objShippingInfo.Name;
            shipper.AttentionName = objShippingInfo.CompanyName;
            ShipPhoneType shipperPhone = new ShipPhoneType();
            if (objShippingInfo.Telephone.Length >= 10)
                shipperPhone.Number = objShippingInfo.Telephone;
            else
                shipperPhone.Number = "0123456789";
            shipper.Phone = shipperPhone;
            shipment.Shipper = shipper;

            // Ship To Address. Company Address
            ShipToType shipTo = new ShipToType();
            ShipToAddressType shipToAddress = new ShipToAddressType();
            String[] addressLine1 = { "400 Beach Road,228, Vero Beach" };
            shipToAddress.AddressLine = addressLine1;
            shipToAddress.City = "Florida";
            shipToAddress.PostalCode = "32963";
            shipToAddress.StateProvinceCode = "FL";
            shipToAddress.CountryCode = "US";
            shipTo.Address = shipToAddress;
            shipTo.AttentionName = "World-Link";
            shipTo.Name = "World-Link";
            ShipPhoneType shipToPhone = new ShipPhoneType();
            shipToPhone.Number = "17722317452";
            shipTo.Phone = shipToPhone;
            shipment.ShipTo = shipTo;

            // Other Service
            ServiceType service = new ServiceType();
            service.Code = "01";
            shipment.Service = service;
            PackageType package = new PackageType();
            PackageWeightType packageWeight = new PackageWeightType();
            packageWeight.Weight = "1";
            ShipUnitOfMeasurementType uom = new ShipUnitOfMeasurementType();
            uom.Code = "LBS";
            packageWeight.UnitOfMeasurement = uom;
            package.PackageWeight = packageWeight;
            PackagingType packType = new PackagingType();
            packType.Code = "02";
            package.Packaging = packType;
            PackageType[] pkgArray = { package };
            shipment.Package = pkgArray;
            LabelSpecificationType labelSpec = new LabelSpecificationType();
            LabelStockSizeType labelStockSize = new LabelStockSizeType();
            labelStockSize.Height = "6";
            labelStockSize.Width = "4";
            labelSpec.LabelStockSize = labelStockSize;
            LabelImageFormatType labelImageFormat = new LabelImageFormatType();
            labelImageFormat.Code = "GIF";//"SPL";
            labelSpec.LabelImageFormat = labelImageFormat;
            shipmentRequest.LabelSpecification = labelSpec;
            shipmentRequest.Shipment = shipment;
            System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicyForUPS();
            Message += shipmentRequest + "<br/>";
            ShipmentResponse shipmentResponse = shpSvc.ProcessShipment(shipmentRequest);
            Message += "The transaction was a " + shipmentResponse.Response.ResponseStatus.Description + "<br/>";
            Message += "The 1Z number of the new shipment is " + shipmentResponse.ShipmentResults.ShipmentIdentificationNumber + "<br/>";

            // Convert image to graphic
            String base64String = shipmentResponse.ShipmentResults.PackageResults[0].ShippingLabel.GraphicImage;
            // Save Base 64 String to Image
            GraphicImageToImage(base64String);
            // Now Open then printable content in new popup window
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(700/2);window.open('PrintPreShippingLabel.aspx', null, 'height=700,width=760,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
            
        }
        catch (System.Web.Services.Protocols.SoapException ex)
        {

            Message += "---------Ship Web Service returns error----------------" + "<br/>";
            Message += "---------\"Hard\" is user error \"Transient\" is system error----------------" + "<br/>";
            Message += "SoapException Message= " + ex.Message + "<br/>";
            Message += "SoapException Category:Code:Message= " + ex.Detail.LastChild.InnerText + "<br/>";
            Message += "SoapException XML String for all= " + ex.Detail.LastChild.OuterXml + "<br/>";
            Message += "SoapException StackTrace= " + ex.StackTrace + "<br/>";
            Message += "-------------------------" + "<br/>";
            Response.Write(Message.ToString());

        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            Message += "--------------------" + "<br/>";
            Message += "CommunicationException= " + ex.Message + "<br/>";
            Message += "CommunicationException-StackTrace= " + ex.StackTrace + "<br/>";
            Message += "-------------------------" + "<br/>";
            Response.Write(Message.ToString());
        }
        catch (Exception ex)
        {

            Message += "-------------------------" + "<br/>";
            Message += " Generaal Exception= " + ex.Message + "<br/>";
            Message += " Generaal Exception-StackTrace= " + ex.StackTrace + "<br/>";
            Message += "-------------------------" + "<br/>";
            Response.Write(Message.ToString());
        }
        finally
        {
            
        }
        modalPrint.Show();

    }
   
   
    protected void dtlReturnRequest_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnIamExpanded = (HiddenField)e.Item.FindControl("hdnIamExpanded");
                HiddenField hdnOrderNumber = (HiddenField)e.Item.FindControl("hdnOrderNumber");
                HiddenField hdnSubmiDate = (HiddenField)e.Item.FindControl("hdnSubmiDate");
                string[] ProductExpandedStatuses = Convert.ToString(Session["ProductExpandedStatuses"]).Split(',');

                GridView grdViewReturnItems = (GridView)e.Item.FindControl("grdViewReturnItems");
                BindGridViewAfterSave(grdViewReturnItems, Convert.ToDateTime(hdnSubmiDate.Value));
            }
        }
        catch
        {
            //ErrHandler.WriteError(ex);
        }
    }

    protected void gvOrderSummary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnReturnStatusId = (HiddenField)e.Row.FindControl("hdnReturnStatusId");
            if (isExist != 1)
            {
                #region For Shipped
                ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
                Label lblQtyShipped = (Label)e.Row.FindControl("lblQtyShipped");
                Label lblProductionDescription = (Label)e.Row.FindControl("lblProductionDescription");
                HiddenField ItemNumber = (HiddenField)e.Row.FindControl("hdnitemNo");
                HiddenField hdnProductReturnId = (HiddenField)e.Row.FindControl("hdnProductReturnId");
                HiddenField MyShoppingCart = (HiddenField)e.Row.FindControl("hdnMyShoppingCartiD");

                List<ShippedReturnProductDescriptionResult> objNew = new List<ShippedReturnProductDescriptionResult>();
                objNew = OrdShippOrder.GetDesCription(Convert.ToInt32(OrderID), Convert.ToInt32(MyShoppingCart.Value), ItemNumber.Value);
                if (objNew.Count > 0)
                {
                    if (objNew[0].ProductDescrption != null)
                    {
                        String description = Convert.ToString(objNew[0].ProductDescrption.Length > 50 ? Convert.ToString(objNew[0].ProductDescrption).Substring(0, 50).Trim() + "..." : (Convert.ToString(objNew[0].ProductDescrption) + "&nbsp;"));
                        lblProductionDescription.Text = description;//objNew[0].ProductDescrption;
                        lblProductionDescription.ToolTip = objNew[0].ProductDescrption;
                    }
                    else
                        lblProductionDescription.Text = "&nbsp;";
                }
                #endregion

                #region For repeater  Bind
                bindRepeater();
                #endregion
            }
               
        }
    }
    #endregion

    #region Page Method's

    private void GraphicImageToImage(String base64String)
    {
        System.Drawing.Image newImage;
        String strFileName = "ShippingLabel.JPEG";
        String sFilePath = Server.MapPath("../UPSShippingReferences/ShippingLabelImage/") + strFileName;
        byte[] data = Convert.FromBase64String(base64String);
        using (System.IO.MemoryStream stream = new System.IO.MemoryStream(data, 0, data.Length))
        {
            //TODO: do something with image
            newImage = System.Drawing.Image.FromStream(stream);
            newImage.Save(sFilePath);

        }

    }

    private void BindOrderSummaryGridView()
    {
        List<ProductReturnRepository.OrderReturnDetails> orderReturnList = new List<ProductReturnRepository.OrderReturnDetails>();
        List<ShipingOrder> objList = new List<ShipingOrder>();

        objList = OrdShippOrder.CheckReturnStatus(OrderID);
        for (int i = 0; i < objList.Count; i++)
        {
            ProductReturnRepository.OrderReturnDetails orderReturn = new ProductReturnRepository.OrderReturnDetails();
            orderReturn.OrderStatus = objList[i].ItemNumber + "/" + (i + 1);
            orderReturn.ItemNumber = objList[i].ItemNumber;
            orderReturn.MyShoppingCartID = Convert.ToInt64(objList[i].MyShoppingCartiD);
            orderReturn.OrderID = Convert.ToInt64(objList[i].OrderID);
            orderReturn.ProductDescription = "";
            orderReturn.Quantity = objList[i].QtyOrder.ToString();
            orderReturn.ShippedQty = objList[i].ShipQuantity.ToString();
            orderReturn.ReasonCode = -1;
            orderReturn.Size = objList[i].ItemNumber;
            orderReturn.ShippID = objList[i].ShippID;
            orderReturnList.Add(orderReturn);
        }

        gvOrderSummary.DataSource = orderReturnList;
        gvOrderSummary.DataBind();
    }

    private void BindGridView()
    {
        List<ProductReturnRepository.OrderReturnDetails> orderReturnList = ObjOrderReturnRepo.GetAllReturnProductListByOrderID(this.OrderID);//new List<ProductReturnRepository.OrderReturnDetails>();
        gvOrderDetail.DataSource = orderReturnList;
        gvOrderDetail.DataBind();
        //List<ShipingOrder> objList = new List<ShipingOrder>();

        //objList = OrdShippOrder.CheckReturnStatus(OrderID);
        //if (objList.Count > 0)
        //{
        //    for (int i = 0; i < objList.Count; i++)
        //    {
        //        ReturnStatus = objList[i].ReturnStatus;
        //        if (!string.IsNullOrEmpty(ReturnStatus))
        //        {
        //            break;
        //        }
        //    }
        //    if (ReturnStatus == "1")
        //    {
        //        BindGridViewProductReturn();                
        //        //lnkPrint.Visible = true;
        //    }
        //    else
        //    {
        //        isExist = 0;
        //        for (int i = 0; i <= objList.Count - 1; i++)
        //        {
        //            if (objList[i].QuantityReceived.ToString() != "")
        //            {
        //                break;
        //            }
        //            isExist = 1;
        //        }
        //        if (isExist == 1)
        //        {
        //            for (int i = 0; i < objList.Count; i++)
        //            {
        //                ProductReturnRepository.OrderReturnDetails orderReturn = new ProductReturnRepository.OrderReturnDetails();
        //                orderReturn.OrderStatus = objList[i].ItemNumber + "/" + (i + 1);
        //                orderReturn.ItemNumber = objList[i].ItemNumber;
        //                orderReturn.MyShoppingCartID = Convert.ToInt64(objList[i].MyShoppingCartiD);
        //                orderReturn.OrderID = Convert.ToInt64(objList[i].OrderID);
        //                orderReturn.ProductDescription = "";
        //                orderReturn.Quantity = objList[i].QtyOrder.ToString();
        //                orderReturn.ShippedQty = objList[i].ShipQuantity.ToString();
        //                orderReturn.ReasonCode = -1;
        //                orderReturn.Size = objList[i].ItemNumber;
        //                orderReturn.ShippID = objList[i].ShippID;
        //                orderReturnList.Add(orderReturn);
        //            }
        //            ViewState["CurrentTable"] = orderReturnList;
        //            gvOrderDetail.DataSource = orderReturnList;
        //            isExist = 0;
        //            gvOrderDetail.DataBind();
        //        }
        //        isExist = 0;
        //        dtlReturnRequest.Visible = false;
        //        divShowOrderDtls.Visible = false;
        //    }
        //}
        //if (objList.Count == 0)
        //{
        //    lblProcessDate.Visible = false;
        //    lblProcessedOn.Visible = false;
        //    lblStatus.Visible = false;
        //    lblStatuslabel.Visible = false;
        //    lnkBtnSubmitReturn.Visible = false;
        //    lblMsg.Text = "There is no Shipped Item exist for a given order.";
        //}
    }

    private void BindGridViewAfterSave(GridView grdView, DateTime Date)
    {
        List<ProductReturnRepository.OrderReturnDetails> orderReturnListNew = new List<ProductReturnRepository.OrderReturnDetails>();
        List<OrderDetailHistoryRepository.OrderDetails> orderListNew = ObjOrderRepo.GetOrderDetailsByOrderID(this.OrderID);

        List<ReturnProduct> objListReturn = new List<ReturnProduct>();
        objListReturn = ObjOrderReturnRepo.GetOrdersGroupByOrderIdSubmiDate(OrderID, Date);

        List<ShipingOrder> objListShipping = new List<ShipingOrder>();
        objListShipping = OrdShippOrder.GetAllShippOrder(OrderID);
        for (int i = 0; i < objListReturn.Count; i++)
        {
            ProductReturnRepository.OrderReturnDetails orderReturn = new ProductReturnRepository.OrderReturnDetails();
            orderReturn.OrderStatus = objListReturn[i].Status;
            orderReturn.ItemNumber = objListReturn[i].ItemNumber;
            orderReturn.MyShoppingCartID = Convert.ToInt64(objListReturn[i].MyShoppingCartID);
            orderReturn.OrderID = Convert.ToInt64(objListReturn[i].OrderId);
            orderReturn.ProductDescription = "";
            orderReturn.ProductReturnId = Convert.ToInt64(objListReturn[i].ProductReturnId);

            var data = objListShipping.Where(m => m.ItemNumber == objListReturn[i].ItemNumber && m.ShippID == objListReturn[i].ShippId).ToList();
            if (data.Count > 0)
                orderReturn.Quantity = data[0].ShipQuantity.ToString();
            orderReturn.ReturnQty = objListReturn[i].ReturnQty.ToString();
            orderReturn.ReasonCode = Convert.ToInt64(objListReturn[i].ReasonId);
            orderReturnListNew.Add(orderReturn);
        }

        grdView.DataSource = orderReturnListNew;
        grdView.DataBind();

    }

    private void BindReasonCode(DropDownList ddlReasonCode)
    {
        if (ddlReasonCode != null)
        {
            try
            {
                ddlReasonCode.DataSource = objLookRep.GetByLookup("ReasonCode");
                ddlReasonCode.DataValueField = "iLookupID";
                ddlReasonCode.DataTextField = "sLookupName";
                ddlReasonCode.DataBind();
                ddlReasonCode.Items.Insert(0, new ListItem("-Select Reason Code-", "0"));
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
        }
    }

    private void BindGridViewProductReturn()
    {
        try
        {
            List<ProductReturnRepository.OrderReturnDetails> orderReturnListNew = new List<ProductReturnRepository.OrderReturnDetails>();
            List<OrderDetailHistoryRepository.OrderDetails> orderListNew = ObjOrderRepo.GetOrderDetailsByOrderID(this.OrderID);

            List<ReturnProduct> objReturnList = new List<ReturnProduct>();
            objReturnList = ObjOrderReturnRepo.GetByOrderID(OrderID);
            if (objReturnList.OrderBy(m => m.ProcessedDate).ToList().Count > 0 && objReturnList.OrderBy(m => m.SubmitDate).ToList()[0].ProcessedDate != null)
                lblProcessDate.Text = Convert.ToDateTime(objReturnList.OrderBy(m => m.SubmitDate).ToList()[0].ProcessedDate).ToString("MM/dd/yyyy");

            if (objReturnList.Where(m => m.ReturnStatusId != null).Count() == objReturnList.Count)
                   lblStatus.Text = "Completed";
            else if (objReturnList.Where(m => m.ReturnStatusId != null).Count() == 0)
                  lblStatus.Text = "Pending";
                else            
                    lblStatus.Text = "Partial Return Completed";

            List<ShipingOrder> objShipList = new List<ShipingOrder>();
            objShipList = OrdShippOrder.GetAllShippOrder(OrderID);
            for (int i = 0; i < objShipList.Count; i++)
            {
                var objItems = objReturnList.Where(s => s.ItemNumber == objShipList[i].ItemNumber && s.ShippId == objShipList[i].ShippID);
                ProductReturnRepository.OrderReturnDetails orderReturn = new ProductReturnRepository.OrderReturnDetails();
                if (objItems.Count() > 0) //If item is alredy Returned than go in if part other wise else part is executed.
                {
                    foreach (var data in objItems)
                    {
                        orderReturn = new ProductReturnRepository.OrderReturnDetails();
                        orderReturn.OrderStatus = data.Status;
                        orderReturn.ItemNumber = data.ItemNumber;
                        orderReturn.MyShoppingCartID = Convert.ToInt64(data.MyShoppingCartID);
                        orderReturn.OrderID = Convert.ToInt64(data.OrderId);
                        orderReturn.ProductDescription = "";
                        orderReturn.ReturnQty = data.ReturnQty.ToString();
                        orderReturn.ReasonCode = Convert.ToInt64(data.ReasonId);
                        orderReturn.ShippedQty = objShipList[i].ShipQuantity.ToString();
                        orderReturn.Quantity = objShipList[i].QtyOrder.ToString();
                        orderReturn.ShippID = objShipList[i].ShippID;
                        orderReturn.ProductReturnId = data.ProductReturnId;
                        orderReturn.ReturnStatusId =Convert.ToInt64( data.ReturnStatusId);
                        orderReturnListNew.Add(orderReturn);
                    }
                }
                else
                {

                    orderReturn.OrderStatus = objShipList[i].ItemNumber + "/" + (i + 1);
                    orderReturn.ItemNumber = objShipList[i].ItemNumber;
                    orderReturn.MyShoppingCartID = Convert.ToInt64(objShipList[i].MyShoppingCartiD);
                    orderReturn.OrderID = Convert.ToInt64(objShipList[i].OrderID);
                    orderReturn.ProductDescription = "";
                    orderReturn.Quantity = objShipList[i].QtyOrder.ToString();
                    orderReturn.ShippedQty = objShipList[i].ShipQuantity.ToString();
                    orderReturn.ReasonCode = -1;
                    orderReturn.Size = objShipList[i].ItemNumber;
                    orderReturn.ShippID = objShipList[i].ShippID;
                    orderReturnListNew.Add(orderReturn);
                }
            }
            ViewState["CurrentTable"] = orderReturnListNew;
            gvOrderDetail.DataSource = orderReturnListNew;
            gvOrderDetail.DataBind();

            int count = 0;
            foreach (GridViewRow t in gvOrderDetail.Rows)
            {
                ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
                Label lblQtyShipped = (Label)t.FindControl("lblQtyShipped");
                Label lblProductionDescription = (Label)t.FindControl("lblProductionDescription");
                HiddenField ItemNumber = (HiddenField)t.FindControl("hdnitemNo");
                DropDownList ddlReasonCode = (DropDownList)t.FindControl("ddlReasonCode");
                HiddenField MyShoppingCart = (HiddenField)t.FindControl("hdnMyShoppingCartiD");
                HiddenField hdnShippid = (HiddenField)t.FindControl("hdnShippid");
                HiddenField hdnProductReturnId = (HiddenField)t.FindControl("hdnProductReturnId");
                TextBox txtReturnQty = (TextBox)t.FindControl("txtReturnQty");
                

                List<ShippedReturnProductDescriptionResult> objNew = new List<ShippedReturnProductDescriptionResult>();
                objNew = OrdShippOrder.GetDesCription(Convert.ToInt32(OrderID), Convert.ToInt32(MyShoppingCart.Value), ItemNumber.Value);
                if (objNew.Count > 0)
                {
                    if (objNew[0].ProductDescrption != null)
                        lblProductionDescription.Text = objNew[0].ProductDescrption;
                    else
                        lblProductionDescription.Text = "&nbsp;";
                }
                BindReasonCode(ddlReasonCode);
                if (objReturnList.Count > 0 && count < objReturnList.Count)
                {
                    if (objReturnList.Where(m => m.ItemNumber == ItemNumber.Value && m.ShippId == Convert.ToInt64(hdnShippid.Value)).ToList().Count > 0)
                    {
                        ddlReasonCode.SelectedValue = objReturnList[count].ReasonId.ToString();
                        count++;
                    }
                }                
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void bindRepeater()
    {
        List<ProductReturnRepository.OrderReturnDetails> ObjProductReturn = ObjOrderReturnRepo.GetOrdersGroupByOrderId(OrderID);
        dtlReturnRequest.DataSource = ObjProductReturn;
        dtlReturnRequest.DataBind();
        if (ObjProductReturn.Count > 0)
            divShowOrderDtls.Visible = true;
        else
            divShowOrderDtls.Visible = false;

    }
    private void DeleteGridViewRow(string rowCmdArg)
    {
        List<ProductReturnRepository.OrderReturnDetails> OrderList = (List<ProductReturnRepository.OrderReturnDetails>)ViewState["CurrentTable"];
        ProductReturnRepository.OrderReturnDetails OrderItem = new ProductReturnRepository.OrderReturnDetails();

        string[] ItemCollection = rowCmdArg.ToString().Split('/');
        List<ProductReturnRepository.OrderReturnDetails> OrderItemNew = new List<ProductReturnRepository.OrderReturnDetails>();//OrderList.Where(m => m.ItemNumber == ItemCollection[0] && m.ShippID == Convert.ToInt64(ItemCollection[1])).ToList();

        String csname1 = "PopupScript";
        string SelectedReasonString = "";

        ClientScriptManager cs = Page.ClientScript;
        if (rowCmdArg == "Processing Return" || rowCmdArg == "Completed")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "You can not delete this row." + "');", true);
            return;
        }
        if (OrderList.Count > 1)
        {
            int count = 0;
            foreach (GridViewRow t in gvOrderDetail.Rows)
            {
                HiddenField hdnReturnQtyValue = (HiddenField)t.FindControl("hdnReturnQtyValue");
                TextBox txtReturnQty = (TextBox)t.FindControl("txtReturnQty");
                DropDownList ddlReasonCode = (DropDownList)t.FindControl("ddlReasonCode");
                if (!String.IsNullOrEmpty(txtReturnQty.Text))
                {
                    OrderList[count].ReturnQty = txtReturnQty.Text;
                    hdnReturnQtyValue.Value = txtReturnQty.Text;
                }
                else if (hdnReturnQtyValue.Value != null && hdnReturnQtyValue.Value != "")
                    OrderList[count].ReturnQty = hdnReturnQtyValue.Value;
                else
                    OrderList[count].ReturnQty = null;
                OrderList[count].ReasonCode = Convert.ToInt32(ddlReasonCode.SelectedValue);
                if (count < gvOrderDetail.Rows.Count)
                    SelectedReasonString += ddlReasonCode.SelectedValue + ",";
                count++;
            }

            OrderItem = OrderList.Where(m => m.OrderStatus == rowCmdArg.ToString()).FirstOrDefault();
            OrderList.Remove(OrderItem);
            ViewState["CurrentTable"] = OrderList;
            ViewState["PreviousNewTable"] = OrderList;
            count = 0;
            string[] Reasons = SelectedReasonString.Split(',');
            //Reasons.SetValue(" ", (Convert.ToInt32(rowCmdArg.ToString().Substring(rowCmdArg.ToString().IndexOf("/") + 1)) - 1));
            foreach (GridViewRow t in gvOrderDetail.Rows)
            {
                if (count < Reasons.Count())
                {
                    if (Reasons[count] == "")
                        count++;
                    DropDownList ddlReasonCode = (DropDownList)t.FindControl("ddlReasonCode");
                    ddlReasonCode.SelectedValue = Reasons[count];
                    count++;
                }
            }

            
            //here to hide previous records of that items i have filter it with new order status and return quantity is null
            //OrderList = OrderList.Where(r => r.ReturnQty == null && r.OrderStatus.Contains(ItemCollection[0])).ToList();
            gvOrderDetail.DataSource = OrderList;
            gvOrderDetail.DataBind();
        }
        else
        {
            cs.RegisterClientScriptBlock(this.GetType(), csname1, "alert('You can not delete this row.')", true);
            return;
        }
    }
    private void AddGridViewRow(string rowCmdArg,Int32 currentCount)
    {

        List<ProductReturnRepository.OrderReturnDetails> OrderList = (List<ProductReturnRepository.OrderReturnDetails>)ViewState["CurrentTable"];
        ProductReturnRepository.OrderReturnDetails OrderItem = new ProductReturnRepository.OrderReturnDetails();

        string[] ItemCollection = rowCmdArg.ToString().Split(';');
        List<ProductReturnRepository.OrderReturnDetails> OrderItemNew = OrderList.Where(m => m.ItemNumber == ItemCollection[0] && m.ShippID == Convert.ToInt64(ItemCollection[1])).ToList();

       
        string SelectedReasonString = "";
       
        OrderItem.ItemNumber = OrderItemNew[0].ItemNumber;
        OrderItem.MyShoppingCartID = OrderItemNew[0].MyShoppingCartID;
        OrderItem.OrderID = OrderItemNew[0].OrderID;
        OrderItem.OrderStatus = ItemCollection[0] + "/" + currentCount.ToString();
        OrderItem.ProductDescription = OrderItemNew[0].ProductDescription;
        OrderItem.Quantity = OrderItemNew[0].Quantity;
        OrderItem.ShippedQty = OrderItemNew[0].ShippedQty.ToString();
        OrderItem.StoreProductID = OrderItemNew[0].StoreProductID;
        OrderItem.UserInfoID = OrderItemNew[0].UserInfoID;
        OrderItem.ShippID = OrderItemNew[0].ShippID;
        OrderList.Add(OrderItem);
        int count = 0;
        foreach (GridViewRow t in gvOrderDetail.Rows)
        {
            HiddenField hdnReturnQtyValue = (HiddenField)t.FindControl("hdnReturnQtyValue");
            TextBox txtReturnQty = (TextBox)t.FindControl("txtReturnQty");
            DropDownList ddlReasonCode = (DropDownList)t.FindControl("ddlReasonCode");

            if (!String.IsNullOrEmpty(txtReturnQty.Text))
                OrderList[count].ReturnQty = txtReturnQty.Text;
            else if (hdnReturnQtyValue.Value != null && hdnReturnQtyValue.Value != "")
                OrderList[count].ReturnQty = hdnReturnQtyValue.Value;

            OrderList[count].ReasonCode = Convert.ToInt32(ddlReasonCode.SelectedValue);
            if (count < gvOrderDetail.Rows.Count)
                SelectedReasonString += ddlReasonCode.SelectedValue + ",";
            count++;
        }
        
        //

        //ViewState["CurrentTable"] = OrderList;
        //here to hide previous records of that items i have filter it
        OrderList = OrderList.Where(r => r.ReturnQty == null).ToList();
        ViewState["CurrentTable"] = OrderList;

        if (ViewState["PreviousNewTable"] != null && ((List<ProductReturnRepository.OrderReturnDetails>)ViewState["PreviousNewTable"]).Count > 0)
            OrderList.AddRange((List<ProductReturnRepository.OrderReturnDetails>)ViewState["PreviousNewTable"]);

        gvOrderDetail.DataSource = OrderList;
        gvOrderDetail.DataBind();
        count = 0;
        string[] Reasons = SelectedReasonString.Split(',');
        foreach (GridViewRow t in gvOrderDetail.Rows)
        {
            if (count < Reasons.Count())
            {
                DropDownList ddlReasonCode = (DropDownList)t.FindControl("ddlReasonCode");
                ddlReasonCode.SelectedValue = Reasons[count];
                count++;
            }
        }
    }

    private void CheckReturnQtyIsLessThenShippedQty(string rowCmdArg)
    {

        List<ProductReturnRepository.OrderReturnDetails> OrderReturnList = ObjOrderReturnRepo.GetReturnProductListByOrderID(this.OrderID);
        List<ShipingOrder> objshipList = OrdShippOrder.GetAllShippOrder(this.OrderID);
        // objList = OrdShippOrder.GetAllShippOrder(this.OrderID);
        string[] ItemCollection = rowCmdArg.ToString().Split(';');
        OrderReturnList = OrderReturnList.Where(m => m.ItemNumber == ItemCollection[0] && m.ShippID == Convert.ToInt64(ItemCollection[1])).ToList();
        String csname1 = "PopupScript";
        ClientScriptManager cs = Page.ClientScript;
        TotalReturnQty = OrderReturnList.Sum(r => Convert.ToInt32(r.ReturnQty == "" ? "0" : r.ReturnQty));

        objshipList = objshipList.Where(m => m.ItemNumber == ItemCollection[0] && m.ShippID == Convert.ToInt64(ItemCollection[1])).ToList();

        hdnMinimumRtnQty.Value = (Convert.ToInt32(objshipList[0].ShipQuantity) - OrderReturnList.Sum(r => Convert.ToInt32(r.ReturnQty == "" ? "0" : r.ReturnQty))).ToString();

        if (objshipList.Count > 0)
        {
            if (TotalReturnQty >= Convert.ToInt32(objshipList[0].ShipQuantity))
            {
                cs.RegisterClientScriptBlock(this.GetType(), csname1, "alert('You can not add rows more than Item qty .')", true);
                IsAllowToAddMoreRow = false;
                return;

            }
            else
                IsAllowToAddMoreRow = true;
        }
        else
            IsAllowToAddMoreRow = true;

    }

    #endregion

    #region Grid Events
    private List<ProductReturnRepository.OrderReturnDetails> GetCurrentGridViewList(String rowCmdArgument,char SplitString)
    {
        List<ProductReturnRepository.OrderReturnDetails> OrderList = new List<ProductReturnRepository.OrderReturnDetails>();
        string[] ItemCollection = rowCmdArgument.ToString().Split(SplitString);
        Int32 Count = 0;
        foreach (GridViewRow t in gvOrderDetail.Rows)
        {
            ProductReturnRepository.OrderReturnDetails OrderItem = new ProductReturnRepository.OrderReturnDetails();
            Label lblItemNumber = (Label)t.FindControl("lblItemNumber");
            Label lblProductionDescription = (Label)t.FindControl("lblProductionDescription");
            HiddenField hdnReturnQtyValue = (HiddenField)t.FindControl("hdnReturnQtyValue");
            HiddenField hdnShippid = (HiddenField)t.FindControl("hdnShippid");
            HiddenField hdnitemNo = (HiddenField)t.FindControl("hdnitemNo");
            HiddenField hdnMyShoppingCartiD = (HiddenField)t.FindControl("hdnMyShoppingCartiD");
            HiddenField hdnQuantity = (HiddenField)t.FindControl("hdnQuantity");
            Label lblQtyOrder = (Label)t.FindControl("lblQtyOrder");
            TextBox txtReturnQty = (TextBox)t.FindControl("txtReturnQty");
            Label lblQtyShipped = (Label)t.FindControl("lblQtyShipped");
            DropDownList ddlReasonCode = (DropDownList)t.FindControl("ddlReasonCode");

            OrderItem.ItemNumber = lblItemNumber.Text;
            OrderItem.MyShoppingCartID = Convert.ToInt64(hdnMyShoppingCartiD.Value);
            OrderItem.OrderID = this.OrderID;
            OrderItem.OrderStatus = ItemCollection[0] + "/" + Count.ToString();
            OrderItem.ProductDescription = lblProductionDescription.Text;
            OrderItem.Quantity = hdnQuantity.Value;
            OrderItem.ShippedQty = lblQtyShipped.Text;
            OrderItem.ReasonCode = Convert.ToInt64(ddlReasonCode.SelectedValue);
            OrderItem.ReturnQty = txtReturnQty.Text;
            OrderItem.ShippID = Convert.ToInt64(hdnShippid.Value);
            OrderList.Add(OrderItem);
            Count++;
        }
        ViewState["CurrentTable"] = OrderList;
        return OrderList;
        
    }

    protected void gvOrderDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "ItemDelete")
        {
            List<ProductReturnRepository.OrderReturnDetails> OrderList = GetCurrentGridViewList(e.CommandArgument.ToString(),'/');
            var OrderItemRemove = OrderList.Where(m => m.OrderStatus == e.CommandArgument.ToString()).FirstOrDefault();
            OrderList.Remove(OrderItemRemove);
            ViewState["CurrentTable"] = OrderList;
            gvOrderDetail.DataSource = OrderList;
            gvOrderDetail.DataBind();
            //DeleteGridViewRow(e.CommandArgument.ToString());


        }
    }
    protected void gvOrderSummary_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "ItemAdd")
        {
            rowCmdArgString = e.CommandArgument.ToString();
            CheckReturnQtyIsLessThenShippedQty(e.CommandArgument.ToString());
            if (IsAllowToAddMoreRow)
            {
                txtReturnQtyNo.Text = "";
                modalPopupConfirm.Show();
            }
        }
    }
    protected void grdViewReturnItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
            Label lblDescription = (Label)e.Row.FindControl("lblDescription");
            Label ItemNumber = (Label)e.Row.FindControl("lblItemNo");
            Label lblReason = (Label)e.Row.FindControl("lblReason");
            HiddenField hdnReasonCode = (HiddenField)e.Row.FindControl("hdnReasonCode");
            HiddenField MyShoppingCart = (HiddenField)e.Row.FindControl("hdnMyShoppingCartIdView");
            List<ShippedReturnProductDescriptionResult> objNew = new List<ShippedReturnProductDescriptionResult>();
            objNew = OrdShippOrder.GetDesCription(Convert.ToInt32(OrderID), Convert.ToInt32(MyShoppingCart.Value), ItemNumber.Text);
            if (objNew.Count > 0)
            {
                if (objNew[0].ProductDescrption != null)
                    lblDescription.Text = objNew[0].ProductDescrption;
                else
                    lblDescription.Text = "&nbsp;";
            }                       
            sReasonCodeBE.SOperation = "selectall";
            sReasonCodeBE.iLookupCode = "ReasonCode";
            DataTable dt = sReasonCode.LookUp(sReasonCodeBE).Tables[0];
            dt.DefaultView.RowFilter = "iLookupID='" + hdnReasonCode.Value + "'";
            if (dt.DefaultView.ToTable().Rows.Count>0)
               lblReason.Text= dt.DefaultView.ToTable().Rows[0]["sLookupName"].ToString();
        }
    }

    protected void gvOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlReasonCode = (DropDownList)e.Row.FindControl("ddlReasonCode");
            HiddenField hdnReasonCode = (HiddenField)e.Row.FindControl("hdnReasonCode");
            TextBox txtReturnQty = (TextBox)e.Row.FindControl("txtReturnQty");
            if (isExist != 1)
            {
                #region For Shipped
                ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
                Label lblQtyShipped = (Label)e.Row.FindControl("lblQtyShipped");
                Label lblProductionDescription = (Label)e.Row.FindControl("lblProductionDescription");
                HiddenField ItemNumber = (HiddenField)e.Row.FindControl("hdnitemNo");
                //HiddenField hdnProductReturnId = (HiddenField)e.Row.FindControl("hdnProductReturnId");
                HiddenField MyShoppingCart = (HiddenField)e.Row.FindControl("hdnMyShoppingCartiD");
                             
                List<ShippedReturnProductDescriptionResult> objNew = new List<ShippedReturnProductDescriptionResult>();
                objNew = OrdShippOrder.GetDesCription(Convert.ToInt32(OrderID), Convert.ToInt32(MyShoppingCart.Value), ItemNumber.Value);
                if (objNew.Count > 0)
                {
                    if (objNew[0].ProductDescrption != null)
                    {
                        String description = Convert.ToString(objNew[0].ProductDescrption.Length > 50 ? Convert.ToString(objNew[0].ProductDescrption).Substring(0, 50).Trim() + "..." : (Convert.ToString(objNew[0].ProductDescrption) + "&nbsp;"));
                        lblProductionDescription.Text = description;//objNew[0].ProductDescrption;
                        lblProductionDescription.ToolTip = objNew[0].ProductDescrption;
                    }
                    else
                        lblProductionDescription.Text = "&nbsp;";
                }               
                #endregion

                #region For Reason Code  Bind
                BindReasonCode(ddlReasonCode);
                if (!String.IsNullOrEmpty(hdnReasonCode.Value))
                    ddlReasonCode.SelectedValue = hdnReasonCode.Value;
                    //ddlReasonCode.Items.FindByValue(hdnReasonCode.Value).Selected = true;

                bindRepeater();
            }
            //if (hdnReturnStatusId.Value != null && hdnReturnStatusId.Value != "0")
            //{
            //    txtReturnQty.Enabled = false;
            //    ddlReasonCode.Enabled = false;
            //}
                #endregion
        }
    }
    #endregion
  
}
