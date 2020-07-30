using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_MyOrderDetails : PageBase
{
    Int64 OrderID
    {
        get
        {
            return Convert.ToInt64(ViewState["OrderID"]);
        }
        set
        {
            ViewState["OrderID"] = value;
        }
    }

    public class TrackingNumber
    {
        public String trackingnuber { get; set; }
        public Int64 suppliernumber { get; set; }
        public Int64 ordernumber { get; set; }
    }

    OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
    OrderDocumentRepository objOrderDocRepos = new OrderDocumentRepository();
    Order objOrder = new Order();
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
    LookupRepository objLookRep = new LookupRepository();
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfo();
    CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfo();
    ShipOrderRepository OrdShippOrder = new ShipOrderRepository();

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Order Detail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/MyTrackingCenter.aspx";

            if (Request.QueryString["OrderId"] != null)
            {
                this.OrderID = Convert.ToInt64(Request.QueryString["OrderId"]);
            }

            BindAddress();
            BindOrderStatus();
            BindParentRepeaterSupplierAddress();
            DisplayNotes();
        }
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
        {
            lnkAddNew.Visible = false;
        }
        else
        {
            lnkAddNew.Visible = true;
        }
    }

    private void BindAddress()
    {
        try
        {
            List<SelectOrderAddressResult> obj = new List<SelectOrderAddressResult>();
            obj = OrderRepos.GetOrderAddress(Convert.ToInt32(this.OrderID));
            if (obj.Count > 0)
            {
                lblOrderNo.Text = obj[0].OrderNumber;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void DownloadFile(String filepath)
    {
        System.IO.Stream iStream = null;

        // Buffer to read 10K bytes in chunk:
        Byte[] buffer = new Byte[10000];

        // Length of the file:
        Int32 length;

        // Total bytes to read:
        Int64 dataToRead;

        // Identify the file name.
        String filename = System.IO.Path.GetFileName(filepath);

        try
        {
            // Open the file.
            iStream = new System.IO.FileStream(filepath, System.IO.FileMode.Open,
            System.IO.FileAccess.Read, System.IO.FileShare.Read);

            // Total bytes to read:
            dataToRead = iStream.Length;

            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "inline; filename=" + filename);

            // Read the bytes.
            while (dataToRead > 0)
            {
                // Verify that the client is connected.
                if (Response.IsClientConnected)
                {
                    // Read the data in buffer.
                    length = iStream.Read(buffer, 0, 10000);

                    // Write the data to the current output stream.
                    Response.OutputStream.Write(buffer, 0, length);

                    // Flush the data to the HTML output.
                    Response.Flush();

                    buffer = new Byte[10000];
                    dataToRead = dataToRead - length;
                }
                else
                {
                    //prevent infinite loop if user disconnects
                    dataToRead = -1;
                }
            }
        }
        catch (Exception ex)
        {
            // Trap the error, if any.
            Response.Write("Error : " + ex.Message);
        }
        finally
        {
            if (iStream != null)
            {
                //Close the file.
                iStream.Close();
            }
        }
    }

    public void DisplayNotes()
    {
        try
        {
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            List<NoteDetail> objList = new List<NoteDetail>();
            objList = objRepo.GetNotesForCACEPerOrderId(this.OrderID, Incentex.DAL.Common.DAEnums.NoteForType.CACE);
            txtOrderNotesForCECA.Text = String.Empty;

            foreach (NoteDetail obj in objList)
            {
                txtOrderNotesForCECA.Text += obj.Notecontents;
                txtOrderNotesForCECA.Text += "\n\n";
                UserInformationRepository objUserRepo = new UserInformationRepository();
                UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);
                if (objUser != null)
                {
                    txtOrderNotesForCECA.Text += objUser.FirstName + " " + objUser.LastName + "   ";
                }
                txtOrderNotesForCECA.Text += Convert.ToDateTime(obj.CreateDate).ToString("MM/dd/yyyy");
                txtOrderNotesForCECA.Text += " @ " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n";
                txtOrderNotesForCECA.Text += "______________________________________________________________________________";
                txtOrderNotesForCECA.Text += "\n\n";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkButton_Click(Object sender, EventArgs e)
    {
        try
        {
            String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.CACE);

            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();

            objComNot.Notecontents = txtNote.Text;
            objComNot.NoteFor = strNoteFor;
            objComNot.ForeignKey = this.OrderID;
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID; // 10; //Session["CurrentUser"].ToString();
            objComNot.UpdateDate = System.DateTime.Now;
            objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;// 10;

            if (!(String.IsNullOrEmpty(txtNote.Text)))
            {
                objCompNoteHistRepos.Insert(objComNot);
                objCompNoteHistRepos.SubmitChanges();
                Int32 NoteId = (Int32)(objComNot.NoteID);
            }

            List<UserInformation> objlist = new List<UserInformation>();
            UserInformationRepository objUInfoRepos = new UserInformationRepository();
            objlist = objUInfoRepos.GetEmailInformation();

            if (objlist.Count > 0)
            {
                for (Int32 i = 0; i < objlist.Count; i++)
                {
                    sendVerificationEmail(txtNote.Text, lblOrderNo.Text, objlist[i].LoginEmail, objlist[i].FirstName + " " + objlist[i].LastName, objlist[i].UserInfoID, this.OrderID);
                }
            }

            txtNote.Text = "";
            DisplayNotes();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void BindOrderStatus()
    {
        try
        {
            //Added on 24 Mar 11
            OrderConfirmationRepository objOrderConfir = new OrderConfirmationRepository();
            Order objOrder = objOrderConfir.GetByOrderID(this.OrderID);

            lblShipping.Text = Convert.ToDecimal(objOrder.ShippingAmount).ToString();
            if (IncentexGlobal.CurrentMember.Usertype != (Int64)Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee && objOrder.MOASOrderAmount != null)
            {
                lblOrderTotal.Text = Convert.ToString(Convert.ToDecimal(objOrder.MOASOrderAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + Convert.ToDecimal(objOrder.ShippingAmount));
                lblSalesTax.Text = (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)).ToString();
            }
            else
            {
                lblOrderTotal.Text = Convert.ToString(Convert.ToDecimal(objOrder.OrderAmount) + objOrder.SalesTax + Convert.ToDecimal(objOrder.ShippingAmount));
                lblSalesTax.Text = objOrder.SalesTax.ToString();
            }

            if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)
            {
                trCorporateDiscount.Visible = true;
                lblCorporateDiscount.Text = objOrder.CorporateDiscount.ToString();
            }

            lblOrderedDate.Text = Convert.ToDateTime(objOrder.OrderDate).ToString("MM/dd/yyyy");
            lblOrderBy.Text = objOrder.ReferenceName.ToString();
            txtSpecialOrderInstruction.Text = objOrder.SpecialOrderInstruction;

            if (objOrder.OrderStatus.ToUpper() == "CLOSED")
            {
                lnkRetunrExchangeDetails.HRef = "~/ProductReturnManagement/ProductItemsReturn.aspx?orderid=" + OrderID;
                lnkRetunrExchangeDetails.Visible = true;
            }
            else
                lnkRetunrExchangeDetails.Visible = false;

            if (objOrder.PaymentOption != null)
            {
                INC_Lookup objLookup = new LookupRepository().GetById((Int64)objOrder.PaymentOption);
                lblPaymentMethod.Text = objLookup.sLookupName + (!String.IsNullOrEmpty(objOrder.PaymentOptionCode) ? " : " + objOrder.PaymentOptionCode : "");
            }
            else
            {
                lblPaymentMethod.Text = "Paid By Corporate";
            }

            //Added on 18 Apr By Ankit
            if (objOrder.CreditUsed == "Previous")
            {
                lblCreditType.Text = "Starting Credits";
                trCreditType.Visible = true;
            }
            else if (objOrder.CreditUsed == "Anniversary")
            {
                lblCreditType.Text = "Anniversary Credits";
                trCreditType.Visible = true;
            }
            else
            {
                trCreditType.Visible = false;
            }

            //End Added
            lblOrderStatus.Text = objOrder.OrderStatus.ToString();

            if (objOrder.OrderFor == "IssuanceCart")
            {
                //Comapny Pays
                objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
                BindBillingAddress();

                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                BindShippingAddress();

                String[] firstitem = objOrder.MyShoppingCartID.Split(',');
                if (!String.IsNullOrEmpty(firstitem[0]))
                {
                    MyIssuanceCart objMyIssuanceCart = new MyIssuanceCartRepository().GetByIssuanceCartId(Convert.ToInt64(firstitem[0]));
                    UniformIssuancePolicyItem objUniformIssuancePolicyItem = new UniformIssuancePolicyItemRepository().GetById(objMyIssuanceCart.UniformIssuancePolicyItemID);
                    if (objUniformIssuancePolicyItem != null)
                        checkissuancepolicy(Convert.ToInt32(objUniformIssuancePolicyItem.UniformIssuancePolicyID));
                }
            }
            else
            {
                objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
                BindBillingAddress();

                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                BindShippingAddress();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void checkissuancepolicy(Int64 issuancepolocyid)
    {
        UniformIssuancePolicy objPolicy = new UniformIssuancePolicy();
        UniformIssuancePolicyRepository objPolicyRepo = new UniformIssuancePolicyRepository();
        LookupRepository objLookupRepo = new LookupRepository();
        INC_Lookup objLook = new INC_Lookup();
        objPolicy = objPolicyRepo.GetById(issuancepolocyid);

        if (objPolicy != null)
        {
            objLook = objLookupRepo.GetById(Convert.ToInt64(objPolicy.PRICEFOR));

            if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Both")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    trPrice.Visible = true;
                    gvOrderDetail.Columns[7].Visible = true;
                    gvOrderDetail.Columns[8].Visible = true;
                }
                else
                {
                    trPrice.Visible = false;
                    gvOrderDetail.Columns[7].Visible = false;
                    gvOrderDetail.Columns[8].Visible = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Employee")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                {
                    trPrice.Visible = true;
                    gvOrderDetail.Columns[7].Visible = true;
                    gvOrderDetail.Columns[8].Visible = true;
                }
                else
                {
                    trPrice.Visible = false;
                    gvOrderDetail.Columns[7].Visible = false;
                    gvOrderDetail.Columns[8].Visible = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Admin")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    trPrice.Visible = true;
                    gvOrderDetail.Columns[7].Visible = true;
                    gvOrderDetail.Columns[8].Visible = true;
                }
                else
                {
                    trPrice.Visible = false;
                    gvOrderDetail.Columns[7].Visible = false;
                    gvOrderDetail.Columns[8].Visible = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Admin")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                {
                    trPrice.Visible = true;
                    gvOrderDetail.Columns[7].Visible = true;
                    gvOrderDetail.Columns[8].Visible = true;
                }
                else
                {
                    trPrice.Visible = false;
                    gvOrderDetail.Columns[7].Visible = false;
                    gvOrderDetail.Columns[8].Visible = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Employee")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    trPrice.Visible = true;
                    gvOrderDetail.Columns[7].Visible = true;
                    gvOrderDetail.Columns[8].Visible = true;
                }
                else
                {
                    trPrice.Visible = false;
                    gvOrderDetail.Columns[7].Visible = false;
                    gvOrderDetail.Columns[8].Visible = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Both")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                {
                    trPrice.Visible = false;
                    gvOrderDetail.Columns[7].Visible = false;
                    gvOrderDetail.Columns[8].Visible = false;
                }
                else
                {
                    trPrice.Visible = true;
                    gvOrderDetail.Columns[7].Visible = true;
                    gvOrderDetail.Columns[8].Visible = true;
                }
            }
        }
    }

    protected void BindBillingAddress()
    {
        lblBAddressLine1.Text = objBillingInfo.Address;
        lblBAddressLine2.Text = objBillingInfo.Address2;
        lblBCity.Text = objCity.GetById((Int64)objBillingInfo.CityID).sCityName + "," + objState.GetById((Int64)objBillingInfo.StateID).sStatename + " " + objBillingInfo.ZipCode;
        lblBCompany.Text = objBillingInfo.CompanyName;
        lblBCountry.Text = objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName;
        lblBName.Text = objBillingInfo.BillingCO + " " + objBillingInfo.Manager;
    }

    protected void BindShippingAddress()
    {
        lblSAddress.Text = objShippingInfo.Address;
        lblSAddress2.Text = objShippingInfo.Address2;
        lblSStreet.Text = objShippingInfo.Street;
        lblSCity.Text = objCity.GetById((Int64)objShippingInfo.CityID).sCityName + "," + objState.GetById((Int64)objShippingInfo.StateID).sStatename + " " + objShippingInfo.ZipCode;
        lblSCompany.Text = objShippingInfo.CompanyName;
        lblSCountry.Text = objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName;
        lblSName.Text = objShippingInfo.Name + " " + objShippingInfo.Fax;

        if (objShippingInfo.OrderFor != null)
            lblSOrderFor.Text = objLookRep.GetById(Convert.ToInt64(objShippingInfo.OrderFor)).sLookupName;
    }

    private void BindParentRepeaterSupplierAddress()
    {
        try
        {
            List<SelectSupplierAddressResult> obj = new List<SelectSupplierAddressResult>();
            List<SelectSupplierAddressBySupplierIdResult> objSupplier = new List<SelectSupplierAddressBySupplierIdResult>();
            objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(this.OrderID));

            if (objOrder != null)
            {
                String[] MyShoppingcart;
                MyShoppingcart = objOrder.MyShoppingCartID.Split(',');
                obj = OrderRepos.GetSupplierAddress(objOrder.OrderFor, this.OrderID);
            }

            List<Object> objRecord = new List<Object>();
            List<SelectShippedQuantityResult> objOrderDoc = new List<SelectShippedQuantityResult>();
            List<SelectDocumentOrderInvoiceResult> OrdDocFile = new List<SelectDocumentOrderInvoiceResult>();
            List<SelectDocumentOrderInvoiceResult> OrdDocFileTemp = new List<SelectDocumentOrderInvoiceResult>();

            foreach (SelectSupplierAddressResult repeaterItem in obj)
            {
                //bind ORder Grid
                List<SelectShipToAddressByOrderIDResult> objShipToAddress = OrderRepos.GetShipToAddressByOrderId(this.OrderID);
                objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(this.OrderID));

                if (objOrder != null)
                {
                    List<SelectOrderDetailsResult> obj1 = new List<SelectOrderDetailsResult>();
                    String[] MyShoppingcart;
                    MyShoppingcart = objOrder.MyShoppingCartID.Split(',');

                    for (Int32 i = 0; i < MyShoppingcart.Count(); i++)
                    {
                        obj1 = OrderRepos.GetDetailOrder(Convert.ToInt32(MyShoppingcart[i]), Convert.ToInt32(repeaterItem.SupplierID), objOrder.OrderFor);

                        if (obj1.Count > 0)
                        {
                            for (Int32 z = 0; z < obj1.Count; z++)
                            {
                                objRecord.Add(obj1[z]);
                            }
                        }
                    }
                }

                List<SelectShippedQuantityResult> objOrderDocTemp = new List<SelectShippedQuantityResult>();

                //bind Shipment Grid
                ShipOrderRepository objShipOrder = new ShipOrderRepository();
                objOrderDocTemp = objShipOrder.GetShippedQuantity(this.OrderID);

                foreach (SelectShippedQuantityResult a in objOrderDocTemp)
                {
                    if (a.ShipingDate != null)
                    {
                        objOrderDoc.AddRange(objOrderDocTemp);
                        break;
                    }
                }
            }

            //Bind Document
            objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(this.OrderID));
            OrdDocFileTemp = objOrderDocRepos.GetDocumnet(0, Convert.ToInt32(this.OrderID), objOrder.OrderFor, "CACE");
            OrdDocFile.AddRange(OrdDocFileTemp);

            if (OrdDocFile.Count > 0)
            {
                gvSupplierPurchaseOrder.DataSource = OrdDocFile;
                gvSupplierPurchaseOrder.DataBind();
            }

            if (objOrderDoc.Count > 0)
            {
                gvShippedOrderDetail.DataSource = objOrderDoc;
                gvShippedOrderDetail.DataBind();
            }

            if (objRecord.Count > 0)
            {
                gvOrderDetail.DataSource = objRecord;
                gvOrderDetail.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvOrderDetail_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label txtQtyOrder = (Label)e.Row.FindControl("txtQtyOrder");
            //updated by prashant on 28th Feb 2013
            Label lblMOASPrice = (Label)e.Row.FindControl("lblMOASPrice");
            Label lblMOASExtendedPrice = (Label)e.Row.FindControl("lblMOASExtendedPrice");
            Label lblPrice = (Label)e.Row.FindControl("lblPrice");
            Label lblExtendedPrice = (Label)e.Row.FindControl("lblExtendedPrice");
            if (IncentexGlobal.CurrentMember.Usertype == (Int64)Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee || String.IsNullOrEmpty(lblMOASPrice.Text) || lblMOASPrice.Text == "$0.00")
            {
                lblPrice.Visible = true;
                lblExtendedPrice.Visible = true;
                lblMOASPrice.Visible = false;
                lblMOASExtendedPrice.Visible = false;
            }
            else
            {
                lblPrice.Visible = false;
                lblExtendedPrice.Visible = false;
                lblMOASPrice.Visible = true;
                lblMOASExtendedPrice.Visible = true;
            }
            //updated by prashant on 28th Feb 2013
            Label lblItemNumber = (Label)e.Row.FindControl("lblItemNumber");
            Label txtQtyOrderRemaining = (Label)e.Row.FindControl("txtQtyOrderRemaining");
            HiddenField MyShoppingCart = (HiddenField)e.Row.FindControl("hdnMyShoppingCartiD");
            CountTotalShippedQuantiyResult objTotalShipped = new CountTotalShippedQuantiyResult();
            Label txtQtyShipped = (Label)e.Row.FindControl("txtQtyShipped");
            objTotalShipped = objShipOrderRepos.GetQtyShippedTotal(Convert.ToInt32(MyShoppingCart.Value), lblItemNumber.Text, this.OrderID);

            if (!(String.IsNullOrEmpty(objTotalShipped.ToString())))
            {
                txtQtyOrderRemaining.Text = Convert.ToString((Convert.ToInt32(txtQtyOrder.Text)) - (Convert.ToInt32(objTotalShipped.ShipQuantity)));
            }

            ((Label)e.Row.FindControl("txtQtyShipped")).Text = Convert.ToInt64(objTotalShipped.ShipQuantity) == 0 ? "---" : objTotalShipped.ShipQuantity.ToString();
        }
    }

    protected void gvShippedOrderDetail_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((SelectShippedQuantityResult)(e.Row.DataItem)).TrackingNo != null)
            {
                List<TrackingNumber> obTrackNew = new List<TrackingNumber>();
                String[] TrackNo = ((SelectShippedQuantityResult)(e.Row.DataItem)).TrackingNo.Split(',');

                foreach (String str in TrackNo)
                {
                    TrackingNumber objNew = new TrackingNumber();
                    objNew.trackingnuber = str;
                    objNew.ordernumber = this.OrderID;
                    obTrackNew.Add(objNew);
                }

                ((GridView)e.Row.FindControl("gvTrackingNo")).DataSource = obTrackNew;
                ((GridView)e.Row.FindControl("gvTrackingNo")).DataBind();
            }

            if (Convert.ToInt64(((SelectShippedQuantityResult)(e.Row.DataItem)).ShipperService) > 0)
                ((Label)e.Row.FindControl("lblSelectedService")).Text = new LookupRepository().GetById(Convert.ToInt64(((SelectShippedQuantityResult)(e.Row.DataItem)).ShipperService)).sLookupName;
            else
                ((Label)e.Row.FindControl("lblSelectedService")).Text = "Not Found";

            ((HyperLink)e.Row.FindControl("hypShippment")).NavigateUrl = "~/OrderManagement/OrderQtyReceived.aspx?ShippID=" + ((HiddenField)e.Row.FindControl("hdnShippment")).Value.ToString() + "&OrderID=" + this.OrderID;
        }
    }

    protected void gvSupplierPurchaseOrder_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "viewPurchaseOrder")
        {
            GridViewRow row;
            row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            GridView gvSupplierPurchaseOrder = (GridView)row.FindControl("gvSupplierPurchaseOrder");
            String file = e.CommandArgument.ToString();
            String filePath = IncentexGlobal.OrderPurchaseInvoice;
            String strFullPath = filePath + file;
            DownloadFile(strFullPath);
        }
    }

    protected void gvSupplierPurchaseOrder_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String[] docname = (((SelectDocumentOrderInvoiceResult)(e.Row.DataItem)).FileName).Split('_');
            ((Label)e.Row.FindControl("lnkFileName")).Text = docname[1].ToString();
        }
    }

    private void sendVerificationEmail(String psNotes, String psOrderNo, String psEmailid, String psFullName, Int64 piUserInfoID, Int64? piOrderID)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "OrderRetunrNotes";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            String sToadd = null;

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                sToadd = psEmailid;
                String sSubject = "Order Notes - " + psOrderNo;

                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());
                messagebody.Replace("{OrderNumber}", psOrderNo);
                messagebody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                messagebody.Replace("{Note}", psNotes);
                messagebody.Replace("{FullName}", psFullName);

                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(piUserInfoID, "Add Order Notes", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, piOrderID);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvTrackingNo_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "TrackingNumber")
        {
            GridViewRow parentRow = ((LinkButton)e.CommandSource).NamingContainer.NamingContainer.NamingContainer as GridViewRow;

            if (parentRow != null)
            {
                Label lblCarrier = (Label)parentRow.FindControl("lblSelectedService");
                if (lblCarrier.Text.Contains("UPS"))
                {
                    usercontrol_UPSPackageTracking ups = LoadControl("~/usercontrol/UPSPackageTracking.ascx") as usercontrol_UPSPackageTracking;
                    ups.TrackingNumber = e.CommandArgument.ToString();
                    pnlUPS.Controls.Add(ups);
                }
            }
        }
    }
}