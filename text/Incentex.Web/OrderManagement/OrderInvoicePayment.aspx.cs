using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class OrderManagement_OrderInvoicePayment : PageBase
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
    String OrderStatus
    {
        get
        {
            return Convert.ToString(ViewState["OrderStatus"]);
        }
        set
        {
            ViewState["OrderStatus"] = value;
        }
    }
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfo();
    CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfo();
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();

    OrderDocumentRepository objOrderDocRepos = new OrderDocumentRepository();
    Order objOrder = new Order();
    ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
    LookupRepository objLookRep = new LookupRepository();
    ShipOrderRepository OrdShippOrder = new ShipOrderRepository();
    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Order Management System";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Customer Invoice & Payment";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/OrderManagement/OrderDetailView.aspx";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.OrderReturnURL;
            if (Request.QueryString["Id"] != null)
            {
                this.OrderID = Convert.ToInt64(Request.QueryString["Id"]);
                setOrderStatus();
            }
            //Bind Menu
            menucontrol.PopulateMenu(2, 0, this.OrderID, 0, false);
            //End
            
            BindOrderStatus();
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
            {
                BindForSupplier();
            }
            else
            {
                BindForAdmin();
                BindSupplier();
            }
            bindfiletypes();


        }
    }
    private void setOrderStatus()
    {
        Order o = new OrderConfirmationRepository().GetByOrderID(this.OrderID);
        this.OrderStatus = o.OrderStatus;
        if (this.OrderStatus.ToUpper() == "CANCELED")
        {
            lnkbtnOrderPaper.Attributes.Add("style", "visibility:hidden");
            lnkbtnOrderPaper1.Attributes.Add("style", "visibility:hidden");
        }

    }
    private void BindSupplier()
    {
        OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
        List<SelectSupplierAddressResult> obj = new List<SelectSupplierAddressResult>();
        obj = OrderRepos.GetSupplierAddress(objOrder.OrderFor, this.OrderID);
        if (obj.Count > 0)
        {
            ddlVendor.DataSource = obj;
            ddlVendor.DataTextField = "Name";
            ddlVendor.DataValueField = "SupplierID";
            ddlVendor.DataBind();
        }
    }
    private void BindOrderStatus()
    {
        try
        {
            //Added on 24 Mar 11
            OrderConfirmationRepository objOrderConfir = new OrderConfirmationRepository();
            Order objOrder = objOrderConfir.GetByOrderID(this.OrderID);

            lblOrderedDate.Text = Convert.ToDateTime(objOrder.OrderDate).ToString("MM/dd/yyyy");
            lblOrderBy.Text = objOrder.ReferenceName.ToString();

            if (objOrder != null)
                lblOrderNo.Text = objOrder.OrderNumber;

            if (objOrder.PaymentOption != null)
            {
                INC_Lookup objLookup = new LookupRepository().GetById((Int64)objOrder.PaymentOption);
                lblPaymentMethod.Text = objLookup.sLookupName;
            }
            else
            {
                lblPaymentMethod.Text = "Paid By Corporate";
            }
            lblOrderStatus.Text = objOrder.OrderStatus.ToString();



            //Start Update Nagmani Change 31-May-2011



            //objBillingInfo = objCmpEmpContRep.GetBillingDetailById((Int64)objOrder.UserId);
            // BindBillingAddress();

            // objShippingInfo = objCmpEmpContRep.GetShippingDetailById((Int64)objOrder.UserId, objOrder.ShippingInfromationid);
            // BindShippingAddress();



            if (objOrder.OrderFor == "IssuanceCart")
            {
                //Comapny Pays
                objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
                BindBillingAddress();

                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                BindShippingAddress();

            }
            else
            {
                objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
                BindBillingAddress();

                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                BindShippingAddress();

            }

            //End Nagmani Change 31-May-2011




            //End Added

            //Added on 18 Arp By Ankit
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

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void BindBillingAddress()
    {
        lblBAddress1.Text = objBillingInfo.Address;
        lblBAddress2.Text = objBillingInfo.Address2;
        if (objBillingInfo.CityID != null && objBillingInfo.StateID != null)
            lblBCity.Text = objCity.GetById((Int64)objBillingInfo.CityID).sCityName + "," + objState.GetById((Int64)objBillingInfo.StateID).sStatename + " " + objBillingInfo.ZipCode;
        lblBCompany.Text = objBillingInfo.CompanyName;
        if (objBillingInfo.CountryID != null)
            lblBCountry.Text = objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName;
        //lblBEmail.Text = objBillingInfo.Email;
        if (!String.IsNullOrEmpty(objBillingInfo.BillingCO))
            lblBName.Text = objBillingInfo.BillingCO + " " + objBillingInfo.Manager;
        else
            lblBName.Text = objBillingInfo.Name;
        //lblBPhone.Text = objBillingInfo.Telephone;

    }
    protected void BindShippingAddress()
    {
        lblSAddress.Text = objShippingInfo.Address;
        lblSAddress2.Text = objShippingInfo.Address2;
        lblSStreet.Text = objShippingInfo.Street;
        if (objShippingInfo.CityID != null && objShippingInfo.StateID != null)
            lblSCity.Text = objCity.GetById((Int64)objShippingInfo.CityID).sCityName + "," + objState.GetById((Int64)objShippingInfo.StateID).sStatename + " " + objShippingInfo.ZipCode;
        lblSCompany.Text = objShippingInfo.CompanyName;
        if (objShippingInfo.CountryID != null)
            lblSCountry.Text = objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName;
        if (!String.IsNullOrEmpty(objBillingInfo.BillingCO))
            lblSName.Text = objBillingInfo.BillingCO + " " + objBillingInfo.Manager;
        else
            lblSName.Text = objBillingInfo.Name;
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
        if (e.CommandName == "deleteinvoicedocument")
        {
            OrderDocumentRepository objOrderDocRepos = new OrderDocumentRepository();
            objOrderDocRepos.DeleteOrderDocument(Convert.ToInt32(e.CommandArgument));
            objOrderDocRepos.SubmitChanges();
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
            {
                BindForSupplier();
            }
            else
            {
                BindForAdmin();
            }


        }
    }
    protected void gvSupplierPurchaseOrder_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            String[] docname = (((SelectDocumentOrderInvoiceResult)(e.Row.DataItem)).FileName).Split('_');
            ((Label)e.Row.FindControl("lnkFileName")).Text = docname[1].ToString();
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
            {
                if (IncentexGlobal.CurrentMember.UserInfoID != ((SelectDocumentOrderInvoiceResult)(e.Row.DataItem)).createdBy)
                {
                    ((LinkButton)e.Row.FindControl("lnkbtndeletePurchaseOrder")).Visible = false;
                }

            }
            if (this.OrderStatus.ToUpper() == "CANCELED")
            {
                ((LinkButton)e.Row.FindControl("lnkbtndeletePurchaseOrder")).Visible = false;
            }
        }
    }
    protected void lnkbtnDocument_Click(Object sender, EventArgs e)
    {
        try
        {


            List<Object> objRecord = new List<Object>();
            objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(this.OrderID));
            OrderDocumentRepository objOrderDocRepos = new OrderDocumentRepository();
            OrderDocument OrdDoc = new OrderDocument();
            String sFileName = null;
            OrdDoc.OrderId = objOrder.OrderID;
            if (ddlFileType.SelectedIndex > 0)
            {
                OrdDoc.FileFor = ddlFileType.SelectedItem.Text;
            }
            else
            {
                OrdDoc.FileFor = null;
            }

            OrdDoc.UploadedBy = IncentexGlobal.CurrentMember.UserInfoID;

            if (objOrder != null)
            {
                OrdDoc.OrderFor = objOrder.OrderFor;
            }
            if (filetypeDiv.Visible == true)
            {
                OrdDoc.Supplierid = Convert.ToInt64(ddlVendor.SelectedItem.Value);
            }
            else
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                {
                    Supplier objSupplier = new SupplierRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                    OrdDoc.Supplierid = objSupplier.SupplierID;
                }
                else
                {
                    OrdDoc.Supplierid = null;
                }
            }
            OrdDoc.InvoiceNumber = txtInvoiceNumber.Text;
            if (fpSupplierPurchadOrderUpload.Value != "")
            {
                OrdDoc.FileName = System.DateTime.Now.Ticks + "_" + fpSupplierPurchadOrderUpload.Value;
                sFileName = System.DateTime.Now.Ticks + "_" + fpSupplierPurchadOrderUpload.Value;
            }
            if (sFileName != null)
            {
                sFileName = Server.MapPath("~/UploadedImages/TailoringMeasurement/") + sFileName;
                Request.Files[0].SaveAs(sFileName);
            }
            objOrderDocRepos.Insert(OrdDoc);
            objOrderDocRepos.SubmitChanges();
            Int32 OrderDocumentID = (Int32)(OrdDoc.OrderDocumentID);

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
            {
                BindForSupplier();
            }
            else
            {
                BindForAdmin();
            }

            //Clear Fields
            txtInvoiceNumber.Text = String.Empty;
            ddlFileType.SelectedIndex = 0;
            ddlVendor.SelectedIndex = 0;
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
    private void bindfiletypes()
    {
        ddlFileType.Items.Insert(0, new ListItem("-select-"));
        ddlFileType.Items.Insert(1, new ListItem("Supplier Invoice"));
        ddlFileType.Items.Insert(2, new ListItem("Supplier Purchase Order"));
        if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
        {
            ddlFileType.Items.Insert(3, new ListItem("Customer Invoice"));
        }


    }

    private void BindForSupplier()
    {


        //Set Visibility of supplier
        filetypeDiv.Visible = false;
        //
        Supplier objSupplier = new SupplierRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(this.OrderID));
        List<SelectDocumentOrderInvoiceResult> OrdDocFile = new List<SelectDocumentOrderInvoiceResult>();
        OrdDocFile = objOrderDocRepos.GetDocumnet(Convert.ToInt32(objSupplier.SupplierID), Convert.ToInt32(this.OrderID), objOrder.OrderFor, "Supplier");//, Convert.ToInt32(hdnCartID.Value));
        gvSupplierPurchaseOrder.DataSource = OrdDocFile;
        gvSupplierPurchaseOrder.DataBind();


    }
    private void BindForAdmin()
    {
        try
        {


            //Set Visibility of 
            filetypeDiv.Visible = true;
            //
            List<SelectDocumentOrderInvoiceResult> OrdDocFile = new List<SelectDocumentOrderInvoiceResult>();
            objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(this.OrderID));
            OrdDocFile = objOrderDocRepos.GetDocumnet(0, Convert.ToInt32(this.OrderID), objOrder.OrderFor, "IEAdmin");//, Convert.ToInt32(hdnCartID.Value));
            if (OrdDocFile.Count > 0)
            {
                gvSupplierPurchaseOrder.DataSource = OrdDocFile;
                gvSupplierPurchaseOrder.DataBind();
            }



        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlFileType_Changed(Object sender, EventArgs e)
    {
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
        {
            filetypeDiv.Visible = false;
        }
        else
        {
            if (ddlFileType.SelectedIndex == 3)
            {
                filetypeDiv.Visible = false;

            }
            else
            {
                filetypeDiv.Visible = true;
            }
        }
        ModalPopupExtenderOrder.Show();
    }



}
