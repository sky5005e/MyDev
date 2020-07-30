using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class ProductReturnManagement_OrderProductReturns : PageBase
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

    Int64 ProductReturnId
    {
        get
        {
            return Convert.ToInt64(ViewState["ProductReturnId"]);
        }
        set
        {
            ViewState["ProductReturnId"] = value;
        }
    }

    String Color
    {
        get
        {
            return Convert.ToString(ViewState["Color"]);
        }
        set
        {
            ViewState["Color"] = value;
        }
    }

    String ReturnStatus
    {
        get
        {
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
            return Convert.ToInt32(ViewState["isExist"]);
        }
        set
        {
            ViewState["isExist"] = value;
        }
    }

    Int32 intbtnSave
    {
        get
        {
            return Convert.ToInt32(ViewState["intbtnSave"]);
        }
        set
        {
            ViewState["intbtnSave"] = value;
        }
    }

    ProductReturnRepository objPrdReturnRepos = new ProductReturnRepository();
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
    UserInformationRepository objUserInfoRepos = new UserInformationRepository();
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    Boolean boolSaved = false;

    protected void Page_Load(Object sender, EventArgs e)
    {
        //Check Here for CA/CE to Inserted Return Quantity        
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
        {
            lnkBtnSaveInfo.Visible = true;
        }
        else
        {
            lnkBtnSaveInfo.Visible = false;
        }

        if (!IsPostBack)
        {
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Order Return Details";

            if (Request.QueryString["OrderID"] != null)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/ProductReturnManagement/ProductReturnView.aspx";
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/ProductReturnManagement/MYProductReturns.aspx";
            }

            BindOrderClosed();
            BindRequestDropDownlist();

            if (Request.QueryString["OrderID"] != null)
            {
                this.OrderID = Convert.ToInt64(Request.QueryString["OrderID"]);
                this.ProductReturnId = Convert.ToInt64(Request.QueryString["ProductReturnId"]);
                PnlgvReturnProductUpdate.Visible = true;
                upnlOrderClosed.Visible = false;
                pnlReturnStatus.Visible = true;
                PnlgvOrderRetun.Visible = false;
                tblProductDescription.Visible = true;
                BidGridViewProductReturn();
                DisplayNotes();
                pnlMain.Visible = true;
                litMessage.Visible = false;
            }
            else
            {
                upnlOrderClosed.Visible = true;
                tblProductDescription.Visible = true;
                pnlMain.Visible = false;
            }

            BindAddress();
            BindOrderStatus();
        }
    }

    private void BindAddress()
    {
        try
        {
            List<SelectOrderAddressResult> obj = new List<SelectOrderAddressResult>();
            if (ddlOrderClosed.SelectedIndex > 0)
            {
                OrderID = Convert.ToInt64(ddlOrderClosed.SelectedValue);
            }
            else if (Request.QueryString["OrderID"] != null)
            {
                OrderID = Convert.ToInt64(Request.QueryString["OrderID"].ToString());
            }
            else
            {
                OrderID = 0;
            }
            obj = OrderRepos.GetOrderAddress(Convert.ToInt32(OrderID));
            if (obj.Count > 0)
            {
                if (Request.QueryString["OrderID"] != null || ReturnStatus == "1")
                {
                    trOrder.Visible = false;
                    trReturnOrder.Visible = true;
                    lblReturnOrderNumber.Text = "RA " + obj[0].OrderNumber;
                }
                else
                {
                    trOrder.Visible = true;
                    trReturnOrder.Visible = false;
                    lblOrderNo.Text = obj[0].OrderNumber;
                }
            }
            else
            {
                trOrder.Visible = false;
                trReturnOrder.Visible = false;
                lblOrderNo.Text = "";
                lblReturnOrderNumber.Text = "";
            }
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
            if (ddlOrderClosed.SelectedIndex > 0)
            {
                OrderID = Convert.ToInt64(ddlOrderClosed.SelectedValue);
            }
            else if (Request.QueryString["OrderID"] != null)
            {
                OrderID = Convert.ToInt64(Request.QueryString["OrderID"].ToString());
            }
            else
            {
                OrderID = 0;
            }

            OrderConfirmationRepository objOrderConfir = new OrderConfirmationRepository();
            Order objOrder = objOrderConfir.GetByOrderID(OrderID);
            if (objOrder != null)
            {
                lblOrderedDate.Text = Convert.ToDateTime(objOrder.OrderDate).ToString("MM/dd/yyyy");
                lblOrderBy.Text = objOrder.ReferenceName.ToString();
                if (objOrder.PaymentOption != null)
                {
                    INC_Lookup objLookup = new LookupRepository().GetById((Int64)objOrder.PaymentOption);
                    lblPaymentMethod.Text = objLookup.sLookupName + (!String.IsNullOrEmpty(objOrder.PaymentOptionCode) ? " : " + objOrder.PaymentOptionCode : "");
                }
                else
                {
                    lblPaymentMethod.Text = "";
                }

                if (Request.QueryString["OrderID"] != null)
                {
                    lblOrderStatus.Text = Request.QueryString["Status"].ToString();
                }
                else
                {
                    lblOrderStatus.Text = objOrder.OrderStatus.ToString();
                }

                if (objOrder.OrderFor == "IssuanceCart")
                {
                    //Comapny Pays
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
                    if (objBillingInfo != null)
                        BindBillingAddress();

                    objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                    if (objShippingInfo != null)
                        BindShippingAddress();
                }
                else
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
                    if (objBillingInfo != null)
                        BindBillingAddress();

                    objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                    if (objShippingInfo != null)
                        BindShippingAddress();
                }
            }
            else
            {
                lblOrderedDate.Text = "";
                lblOrderBy.Text = "";
                lblPaymentMethod.Text = "";
                lblOrderStatus.Text = "";
                lblBAddress.Text = "";
                lblBCity.Text = "";
                lblBCompany.Text = "";
                lblBCountry.Text = "";
                lblBName.Text = "";
                lblSAddress.Text = "";
                lblSCity.Text = "";
                lblSCompany.Text = "";
                lblSCountry.Text = "";
                lblSName.Text = "";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindBillingAddress()
    {
        lblBAddress.Text = objBillingInfo.Address;
        lblBCity.Text = objCity.GetById((Int64)objBillingInfo.CityID).sCityName + "," + objState.GetById((Int64)objBillingInfo.StateID).sStatename + "," + objBillingInfo.ZipCode;
        lblBCompany.Text = objBillingInfo.CompanyName;
        lblBCountry.Text = objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName;
        if (!String.IsNullOrEmpty(objBillingInfo.Manager))
        {
            lblBName.Text = objBillingInfo.BillingCO + " " + objBillingInfo.Manager;
        }
        else
        {
            lblBName.Text = objBillingInfo.BillingCO;
        }
    }

    protected void BindShippingAddress()
    {
        lblSAddress.Text = objShippingInfo.Address;
        lblSCity.Text = objCity.GetById((Int64)objShippingInfo.CityID).sCityName + "," + objState.GetById((Int64)objShippingInfo.StateID).sStatename + "," + objShippingInfo.ZipCode;
        lblSCompany.Text = objShippingInfo.CompanyName;
        lblSCountry.Text = objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName;
        lblSName.Text = objShippingInfo.Name + " " + objShippingInfo.Fax;
    }

    public void DisplayNotes()
    {
        try
        {
            if (ddlOrderClosed.SelectedIndex > 0)
            {
                OrderID = Convert.ToInt64(ddlOrderClosed.SelectedValue);
            }
            else if (Request.QueryString["OrderID"] != null)
            {
                OrderID = Convert.ToInt64(Request.QueryString["OrderID"].ToString());
            }
            else
            {
                OrderID = 0;
            }
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            List<NoteDetail> objList = new List<NoteDetail>();
            objList = objRepo.GetNotesForCACEPerOrderId(OrderID, Incentex.DAL.Common.DAEnums.NoteForType.OrderProductReturns);
            if (objList.Count == 0)
            {
                txtOrderNotesForCECA.Text = "";
            }
            else
            {
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
                    txtOrderNotesForCECA.Text += "__________________________________________________________";
                    txtOrderNotesForCECA.Text += "\n\n";
                }
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
            if (ddlOrderClosed.SelectedIndex > 0)
            {
                OrderID = Convert.ToInt64(ddlOrderClosed.SelectedValue);
            }
            else if (Request.QueryString["OrderID"] != null)
            {
                OrderID = Convert.ToInt64(Request.QueryString["OrderID"].ToString());
            }
            else
            {
                OrderID = 0;
            }
            String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.OrderProductReturns);

            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();

            objComNot.Notecontents = txtNote.Text;
            objComNot.NoteFor = strNoteFor;
            objComNot.ForeignKey = OrderID;
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID; // 10; //Session["CurrentUser"].ToString();
            objComNot.UpdateDate = System.DateTime.Now;
            objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;// 10;

            if (!(String.IsNullOrEmpty(txtNote.Text)))
            {
                objCompNoteHistRepos.Insert(objComNot);
                objCompNoteHistRepos.SubmitChanges();
                Int32 NoteId = (Int32)(objComNot.NoteID);
                sendIEEmailForNotes(OrderID);
                txtNote.Text = "";
            }

            DisplayNotes();

            if (intbtnSave == 1)
            {
                lnkBtnSaveInfo.Visible = false;
            }
            else
            {
                lnkBtnSaveInfo.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    ///  BidGridViewProductReturn()
    ///  This gridview is bind from table Product Return
    /// </summary>
    private void BidGridViewProductReturn()
    {
        try
        {
            List<ReturnProduct> objList = new List<ReturnProduct>();
            if (Request.QueryString["OrderID"] != null)
            {
                OrderID = Convert.ToInt64(Request.QueryString["OrderID"].ToString());
            }
            else if (ddlOrderClosed.SelectedIndex > 0)
            {
                OrderID = Convert.ToInt64(ddlOrderClosed.SelectedValue);
            }
            else
            {
                OrderID = 0;
            }

            objList = objPrdReturnRepos.GetByOrderID(OrderID);

            if (objList.Count > 0)
            {
                gvReturnProductUpdate.DataSource = objList;
                gvReturnProductUpdate.DataBind();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// BindGriedViewOrder Selection of Dropdownlist()
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BidGridView()
    {
        try
        {
            List<ShipingOrder> objList = new List<ShipingOrder>();

            if (ddlOrderClosed.SelectedIndex > 0)
            {
                OrderID = Convert.ToInt64(ddlOrderClosed.SelectedValue);
            }
            else if (Request.QueryString["OrderID"] != null)
            {
                OrderID = Convert.ToInt64(Request.QueryString["OrderID"].ToString());
            }
            else
            {
                OrderID = 0;
            }
            //Check here the OrderId Return Status

            objList = OrdShippOrder.CheckReturnStatus(OrderID);
            if (objList.Count > 0)
            {
                for (Int32 i = 0; i < objList.Count; i++)
                {
                    ReturnStatus = objList[i].ReturnStatus;
                    if (!String.IsNullOrEmpty(ReturnStatus))
                    {
                        ReturnStatus = "1";
                        intbtnSave = 1;
                        break;
                    }
                }

                if (ReturnStatus == "1")
                {
                    if (boolSaved == true)
                    {

                        String myStringVariable = String.Empty;
                        myStringVariable = "Record Saved Successfully!";
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                    }
                    else
                    {

                        String myStringVariable = String.Empty;
                        myStringVariable = "This Order Number is Already Return";
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                        lblmsg.Text = "";
                    }

                    BidGridViewProductReturn();
                    PnlgvReturnProductUpdate.Visible = true;
                    pnlReturnStatus.Visible = true;
                    pnlOrderSummary.Visible = true;
                    PnlgvOrderRetun.Visible = false;
                    lnkBtnSaveInfo.Visible = false;
                    lnkAddNew.Visible = true;
                    pnlNotes.Visible = true;
                    isExist = 1;
                }
                else
                {
                    List<ShipingOrder> objList1 = new List<ShipingOrder>();
                    objList1 = OrdShippOrder.GetAllShippOrder(OrderID);

                    for (Int32 i = 0; i <= objList1.Count - 1; i++)
                    {
                        if (objList1[i].QuantityReceived.ToString() != "")
                        {
                            isExist = 1;
                            break;
                        }
                        else
                        {
                            isExist = 1;
                        }
                    }

                    if (isExist == 1)
                    {
                        gvOrderReturn.DataSource = objList1;
                        gvOrderReturn.DataBind();
                    }

                    PnlgvReturnProductUpdate.Visible = false;
                    PnlgvOrderRetun.Visible = true;
                    pnlReturnStatus.Visible = true;
                    pnlOrderSummary.Visible = true;
                    lnkBtnSaveInfo.Visible = true;
                    lnkAddNew.Visible = true;
                    pnlNotes.Visible = true;

                    txtPrdDescription.Text = "";
                    intbtnSave = 0;
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Bind Order # dropdownlist 
    /// which order no has status is Closed"
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void BindOrderClosed()
    {
        List<GetOrdersToBeReturnedByUserInfoIDResult> objOrd = new List<GetOrdersToBeReturnedByUserInfoIDResult>();
        objOrd = OrderRepos.GetOrderNo(IncentexGlobal.CurrentMember.UserInfoID);

        if (objOrd.Count > 0)
        {
            ddlOrderClosed.DataSource = objOrd;
            ddlOrderClosed.DataTextField = "OrderNumber";
            ddlOrderClosed.DataValueField = "OrderID";
            ddlOrderClosed.DataBind();
            ddlOrderClosed.Items.Insert(0, new ListItem("-Select Order-", "0"));
        }

        if (ddlOrderClosed.Items.Count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('There are no orders that can be returned.');", true);
            pnlcolsedOrder.Visible = false;
        }
        else
        {
            pnlcolsedOrder.Visible = true;
        }
    }

    protected void ddlOrderClosed_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            BidGridView();
            BindAddress();
            BindOrderStatus();
            DisplayNotes();

            if (ddlOrderClosed.SelectedIndex > 0)
            {
                pnlMain.Visible = true;
                litMessage.Visible = false;
            }
            else
            {
                isExist = 1;
                pnlMain.Visible = false;
                litMessage.Visible = true;
            }

            if (isExist == 1)
            {
                pnlMain.Visible = true;

            }
            else
            {
                pnlMain.Visible = false;
            }

            pnlcolsedOrder.Visible = false;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvOrderReturn_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<ShippedReturnProductDescriptionResult> objNew = new List<ShippedReturnProductDescriptionResult>();
                HiddenField hdnOrderID = (HiddenField)e.Row.FindControl("hdnOrderID");
                HiddenField hdnitemNo = (HiddenField)e.Row.FindControl("hdnitemNo");
                HiddenField hdnShoppingCartId = (HiddenField)e.Row.FindControl("hdnShoppingCartId");
                Label lblColor = (Label)e.Row.FindControl("lblColor");
                Label lblSize = (Label)e.Row.FindControl("lblSize");
                Label lblProductDescription = (Label)e.Row.FindControl("lblProductionDescription");
                Label lblReceivedQty = (Label)e.Row.FindControl("lblReceivedQty");
                TextBox txtReturnQty = (TextBox)e.Row.FindControl("txtReturnQty");

                objNew = OrdShippOrder.GetDesCription(Convert.ToInt32(hdnOrderID.Value), Convert.ToInt32(hdnShoppingCartId.Value), hdnitemNo.Value);

                if (objNew.Count > 0)
                {
                    this.Color = objNew[0].Color;
                    if (objNew[0].Size != null)
                    {
                        lblSize.Text = objNew[0].Size;
                    }
                    else
                    {
                        lblSize.Text = "&nbsp;";
                    }
                    if (objNew[0].ProductDescrption != null)
                    {
                        lblProductDescription.Text = objNew[0].ProductDescrption;
                    }
                    else
                    {
                        lblProductDescription.Text = "&nbsp;";
                    }
                    ((Image)e.Row.FindControl("imgColor")).ImageUrl = "~/admin/Incentex_Used_Icons/" + new LookupRepository().GetIconByLookupName(this.Color);
                    lblColor.Text = objNew[0].Color;
                    lblColor.Visible = false;

                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Bind Color, Description,Size, Received Quantity And ShipDate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvReturnProductUpdate_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<ShippedReturnProductDescriptionResult> objNew = new List<ShippedReturnProductDescriptionResult>();
                List<ShipingOrder> objShipp = new List<ShipingOrder>();
                HiddenField hdnRequesting = (HiddenField)e.Row.FindControl("hdnRequesting");
                HiddenField hdnOrderID = (HiddenField)e.Row.FindControl("hdnOrderID");
                HiddenField hdnShipDate = (HiddenField)e.Row.FindControl("hdnShipDate");
                HiddenField hdnitemNo = (HiddenField)e.Row.FindControl("hdnitemNo");
                HiddenField hdniShippid = (HiddenField)e.Row.FindControl("hdnshippid");
                HiddenField hdnShoppingCartId = (HiddenField)e.Row.FindControl("hdnShoppingCartId");
                HiddenField hdnReason = (HiddenField)e.Row.FindControl("hdnReason");
                Label lblColor = (Label)e.Row.FindControl("lblColor");
                Label lblSize = (Label)e.Row.FindControl("lblSize");
                Label lblReceivedQty = (Label)e.Row.FindControl("lblReceivedQty");
                Label lblQtyOrder = (Label)e.Row.FindControl("lblQtyOrder");
                Label lblProductDescription = (Label)e.Row.FindControl("lblProductionDescription");

                txtPrdDescription.Text = hdnReason.Value;
                objNew = OrdShippOrder.GetDesCription(Convert.ToInt32(hdnOrderID.Value), Convert.ToInt32(hdnShoppingCartId.Value), hdnitemNo.Value);

                if (objNew.Count > 0)
                {
                    this.Color = objNew[0].Color;
                    ((Image)e.Row.FindControl("imgColor")).ImageUrl = "~/admin/Incentex_Used_Icons/" + new LookupRepository().GetIconByLookupName(this.Color);

                    if (objNew[0].Size != null)
                    {
                        lblSize.Text = objNew[0].Size;
                    }
                    else
                    {
                        lblSize.Text = "&nbsp;";
                    }

                    if (objNew[0].ProductDescrption != null)
                    {
                        lblProductDescription.Text = objNew[0].ProductDescrption;
                    }
                    else
                    {
                        lblProductDescription.Text = "&nbsp;";
                    }

                    lblColor.Visible = false;
                }

                objShipp = OrdShippOrder.GetShippAll(Convert.ToInt64(hdniShippid.Value));

                if (objShipp.Count > 0)
                {
                    if (objShipp[0].QuantityReceived != null)
                    {
                        lblReceivedQty.Text = objShipp[0].QuantityReceived.ToString();
                    }
                    else
                    {
                        lblReceivedQty.Text = "&nbsp;";
                    }

                    lblQtyOrder.Text = objShipp[0].QtyOrder.ToString().Trim();
                    hdnShipDate.Value = objShipp[0].ShipingDate.ToString();

                    BindRequestDropDownlist();
                    ddlRequesting.Items.FindByValue(hdnRequesting.Value).Selected = true;
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkBtnSaveInfo_Click(Object sender, EventArgs e)
    {
        //Make Return for reason compulsory field..
        if (txtPrdDescription.Text == String.Empty)
        {
            String reason = String.Empty;
            reason = "You must enter in the field named “Reason for Return” located on the prior page why you are placing this return. If this is for an exchange you must enter in the item(s) you wish to exchange for. Thank you.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + reason + "');", true);
            return;
        }
        if (ddlRequesting.SelectedIndex == 0)
        {
            String myStringVariable = String.Empty;
            myStringVariable = "Please Select Type Of Return.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
            return;
        }
        //End

        foreach (GridViewRow t in gvOrderReturn.Rows)
        {
            TextBox txtReturnQty = (TextBox)t.FindControl("txtReturnQty");
            //Kept for Email..As it emptying its value in below logic
            if (!(String.IsNullOrEmpty(txtReturnQty.Text)))
            {
                ((HiddenField)t.FindControl("hfReturnQty")).Value = txtReturnQty.Text;
            }
            //End
        }

        Boolean isSetReturnQty = false;

        foreach (GridViewRow t in gvOrderReturn.Rows)
        {
            HiddenField hdnTrackingNo = (HiddenField)t.FindControl("hdnTrackingNumber");
            HiddenField hdnOrderID = (HiddenField)t.FindControl("hdnOrderID");
            HiddenField hdnShippID = (HiddenField)t.FindControl("hdnshippid");
            HiddenField hdnPackageID = (HiddenField)t.FindControl("hdnPackageID");
            HiddenField hdnMyShoppingCartid = (HiddenField)t.FindControl("hdnShoppingCartId");
            HiddenField hdnItemNumber = (HiddenField)t.FindControl("hdnitemNo");
            TextBox txtReturnQty = (TextBox)t.FindControl("txtReturnQty");

            HiddenField hdnShipDate = (HiddenField)t.FindControl("hdnShipDate");
            Label lblItemReceived = (Label)t.FindControl("lblReceivedQty");
            Label lblQtyOrder = (Label)t.FindControl("lblQtyOrder");
            ReturnProduct objReturnProduct = new ReturnProduct();
            objReturnProduct.OrderId = Convert.ToInt64(hdnOrderID.Value);

            if (!(String.IsNullOrEmpty(txtReturnQty.Text)))
            {
                objReturnProduct.ReturnQty = Convert.ToInt64(txtReturnQty.Text);
            }

            objReturnProduct.SubmitDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            objReturnProduct.ShippId = Convert.ToInt64(hdnShippID.Value);
            objReturnProduct.Requesting = Convert.ToInt64(ddlRequesting.SelectedValue);
            objReturnProduct.Reason = txtPrdDescription.Text;
            objReturnProduct.ItemNumber = hdnItemNumber.Value;
            objReturnProduct.MyShoppingCartID = Convert.ToInt64(hdnMyShoppingCartid.Value);
            objReturnProduct.TrackingNumber = hdnTrackingNo.Value;
            objReturnProduct.PackageID = hdnPackageID.Value;
            objReturnProduct.Status = "Processing Return";
            objReturnProduct.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;

            //Check Here the ship date Should Not be Greater than 30 days for Exchange and Return
            Order objOrder = new Order();
            objOrder = OrderRepos.GetByOrderID(Convert.ToInt64(hdnOrderID.Value));

            if (!(String.IsNullOrEmpty(txtReturnQty.Text)))
            {
                isSetReturnQty = true;
                if (!(String.IsNullOrEmpty(txtReturnQty.Text)) && txtReturnQty.Text != "0")
                {
                    //Check Here Return Quantity Should Not be Greater Than item Received
                    // S Nagmani 6july2011                    
                    if ((Convert.ToInt32(txtReturnQty.Text)) <= (Convert.ToInt32(lblQtyOrder.Text.Trim())))
                    {
                        objPrdReturnRepos.Insert(objReturnProduct);
                        objPrdReturnRepos.SubmitChanges();
                        hdnReturnQuantity.Value = txtReturnQty.Text;
                        //Update ReturnStatus in shipporder table
                        OrdShippOrder.UpDateShipOrderReturnStatus(Convert.ToInt32(hdnShippID.Value), ddlRequesting.SelectedItem.Text);
                        boolSaved = true;

                        // S Nagmani 6july2011
                        if ((Convert.ToInt32(txtReturnQty.Text)) == (Convert.ToInt32(lblQtyOrder.Text.Trim())))
                        {
                            txtReturnQty.Text = "";
                            txtReturnQty.Enabled = false;
                            ddlRequesting.Enabled = false;
                        }
                        else
                        {
                            txtReturnQty.Text = "";
                        }
                    }
                    else
                    {
                        String myStringVariable = String.Empty;
                        myStringVariable = "Return Quantity Should Not Be Greater Than Total Quantity Order";
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                        return;
                    }
                }
                else
                {
                    String myStringVariable = String.Empty;
                    myStringVariable = "Return Quantity Should Not Be Zero";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                    return;
                }
            }
        }

        if (isSetReturnQty == false)
        {
            String myStringVariable = "Please enter return quantity.";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
            return;
        }

        if (hdnReturnQuantity.Value != "" && hdnReturnQuantity.Value != "0")
        {
            sendVerificationEmail(this.OrderID.ToString());
            sendIEReturnEmail(this.OrderID);
        }

        if (Request.QueryString["OrderID"] != null)
        {
            foreach (GridViewRow t in gvReturnProductUpdate.Rows)
            {
                ReturnProduct objReturnProduct = new ReturnProduct();
                TextBox txtReturnQty = (TextBox)t.FindControl("txtReturnQty");
                HiddenField hdnShippID = (HiddenField)t.FindControl("hdnshippid");

                Label lblQtyOrder = (Label)t.FindControl("lblQtyOrder");
                Label lblItemReceived = (Label)t.FindControl("lblReceivedQty");

                HiddenField hdnProductReturnId = (HiddenField)t.FindControl("hdnProductReturnId");

                ProductReturnId = Convert.ToInt64(hdnProductReturnId.Value);
                objReturnProduct.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

                if (this.ProductReturnId != 0)
                {
                    objReturnProduct = objPrdReturnRepos.GetById(this.ProductReturnId);
                }

                if (!(String.IsNullOrEmpty(txtReturnQty.Text)))
                {
                    objReturnProduct.ReturnQty = Convert.ToInt64(txtReturnQty.Text);
                }
                else
                {
                    String myStringVariable = String.Empty;
                    myStringVariable = "Please enter the Return Quantity.";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                    return;
                }

                objReturnProduct.Reason = txtPrdDescription.Text;
                objReturnProduct.Requesting = Convert.ToInt64(ddlRequesting.SelectedValue);

                if (this.ProductReturnId != 0)
                {
                    if (!(String.IsNullOrEmpty(txtReturnQty.Text)) && txtReturnQty.Text != "0")
                    {
                        //Check Here Return Quantity Should Not be Greater Than item Received
                        // S Nagmani 6july2011

                        if ((Convert.ToInt32(txtReturnQty.Text)) <= (Convert.ToInt32(lblQtyOrder.Text.Trim())))
                        {
                            objPrdReturnRepos.SubmitChanges();
                            this.ProductReturnId = objReturnProduct.ProductReturnId;
                            //Update ReturnStatus in shipporder table
                            OrdShippOrder.UpDateShipOrderReturnStatus(Convert.ToInt32(hdnShippID.Value), ddlRequesting.SelectedItem.Text);

                            sendVerificationEmail(this.OrderID.ToString());
                            sendIEReturnEmail(this.OrderID);
                        }
                        else
                        {
                            String myStringVariable = String.Empty;
                            myStringVariable = "Return Quantity Should Not Be Greater Than Total Quantity Order";
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                            return;
                        }
                    }
                }
            }

            BidGridViewProductReturn();
        }

        BidGridView();
        ddlOrderClosed.SelectedIndex = 0;
        Response.Redirect("ProductReturnThank.aspx?OrderID=" + OrderID + "&Status=" + lblOrderStatus.Text + "&ReturnType=" + ddlRequesting.SelectedItem.Text);
    }

    #region sending email to the user

    private void sendVerificationEmail(String OrderNumber)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            RegistrationBE objRegistrationBE = new RegistrationBE();
            RegistrationDA objRegistrationDA = new RegistrationDA();
            DataSet dsEmailTemplate;
            Common objcommm = new Common();

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Return";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
            UserInformation objUsrInfo = new UserInformation();

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                objUsrInfo = objUsrInfoRepo.GetById(IncentexGlobal.CurrentMember.UserInfoID);
                String sToadd = "";
                Int64 sToUserInfoID = 0;
                if (objUsrInfo != null)
                {

                    sToadd = objUsrInfo.LoginEmail;
                    sToUserInfoID = objUsrInfo.UserInfoID;
                }

                String sSubject = "Return Authorization - " + "RA" + lblOrderNo.Text;
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{Customername}", objUsrInfo.FirstName + " " + objUsrInfo.LastName);
                messagebody.Replace("{RA}", "RA" + lblOrderNo.Text);
                messagebody.Replace("{fullname}", objUsrInfo.FirstName + " " + objUsrInfo.LastName);
                messagebody.Replace("{OrderNumber}", lblOrderNo.Text);
                messagebody.Replace("{ReferenceName}", lblOrderBy.Text);
                messagebody.Replace("{OrderDate}", lblOrderedDate.Text);

                if (lblPaymentMethod.Text != "")
                {
                    messagebody.Replace("{PaymentType}", lblPaymentMethod.Text);
                }
                else
                {
                    messagebody.Replace("{PaymentType}", "");
                    messagebody.Replace("Payment Method :", "");
                }

                messagebody.Replace("{SentOn}", System.DateTime.Now.ToString());
                messagebody.Replace("{Status}", ddlRequesting.SelectedItem.Text);
                messagebody.Replace("{storename}", (new CompanyRepository().GetById((Int64)IncentexGlobal.CurrentMember.CompanyId).CompanyName).ToString());
                //Start WTDC Shipp To Address

                ManagedShipAddressRepository objManageRepos = new ManagedShipAddressRepository();
                MangedShipAddress objManage = new MangedShipAddress();
                objManage = objManageRepos.GetAllRecord();

                if (objManage != null)
                {
                    messagebody.Replace("{CompanyName}", objManage.CompanyName);
                    messagebody.Replace("{Title}", objManage.Title);
                    messagebody.Replace("{Address}", objManage.Address);
                    messagebody.Replace("{CityStateZip}", new CityRepository().GetById(Convert.ToInt64(objManage.CityId)).sCityName + "  " + new StateRepository().GetById(Convert.ToInt64(objManage.StateId)).sStatename + "  " + objManage.Zipcode);
                    messagebody.Replace("{Tel}", objManage.Telephone);
                }
                //End

                #region start

                String innermessage = "";

                foreach (GridViewRow grv in gvOrderReturn.Rows)
                {
                    if (((HiddenField)grv.FindControl("hfReturnQty")).Value != "" && (ddlRequesting.SelectedIndex != 0))
                    {
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='20%' style='font-weight: bold;'>";
                        innermessage = innermessage + ((HiddenField)grv.FindControl("hdnitemNo")).Value;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + ((HiddenField)grv.FindControl("hfReturnQty")).Value;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + ((Label)grv.FindControl("lblColor")).Text;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='20%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + ((Label)grv.FindControl("lblSize")).Text;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='35%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + ((Label)grv.FindControl("lblProductionDescription")).Text;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                    }
                }

                messagebody.Replace("{innermessage}", innermessage);

                #endregion

                messagebody.Replace("{reasonforreturn}", txtPrdDescription.Text);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ReturnConfirmations)) == true)
                {
                    new CommonMails().SendMail(sToUserInfoID, "Order Returns", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    private void BindRequestDropDownlist()
    {
        try
        {   
            ddlRequesting.DataSource = objLookRep.GetByLookup("Request");
            ddlRequesting.DataValueField = "iLookupID";
            ddlRequesting.DataTextField = "sLookupName";
            ddlRequesting.DataBind();
            ddlRequesting.Items.Insert(0, new ListItem("-Select-", "0"));

            if (Request.QueryString["ReturnType"] != null)
            {
                ddlRequesting.Items.FindByText(Request.QueryString["ReturnType"].ToString()).Selected = true;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #region Admin Part Email for the Notes

    private void sendIEEmailForNotes(Int64 OrderId)
    {
        List<UserInformation> objAdminList = new List<UserInformation>();
        objAdminList = new UserInformationRepository().GetEmailInformation();
        if (objAdminList.Count > 0)
        {
            for (Int32 i = 0; i < objAdminList.Count; i++)
            {
                sendVerificationMailForNotesToAdmin(OrderId, objAdminList[i].LoginEmail, objAdminList[i].FirstName, objAdminList[i].UserInfoID);
            }
        }
    }

    private void sendVerificationMailForNotesToAdmin(Int64 OrderId, String IEemailAddress, String FullName, Int64 UserInfoID)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            RegistrationBE objRegistrationBE = new RegistrationBE();
            RegistrationDA objRegistrationDA = new RegistrationDA();
            DataSet dsEmailTemplate;
            Common objcommm = new Common();

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "OrderRetunrNotes";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
            UserInformation objUsrInfo = new UserInformation();
            Order objOrder = new OrderConfirmationRepository().GetByOrderID(OrderID);

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sToadd = IEemailAddress;
                Int64 sToUserInfoID = UserInfoID;

                String sSubject = "Return Notes - " + lblReturnOrderNumber.Text;
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                messagebody.Replace("{FullName}", FullName);
                messagebody.Replace("{OrderNumber}", "RA" + objOrder.OrderNumber);
                messagebody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                messagebody.Replace("{Note}", txtNote.Text);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ReturnNotes)) == true)
                {
                    new CommonMails().SendMail(sToUserInfoID, "Order Returns", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Admin part email for the return products

    private void sendIEReturnEmail(Int64 OrderId)
    {
        List<UserInformation> objAdminList = new List<UserInformation>();
        objAdminList = new UserInformationRepository().GetEmailInformation();
        if (objAdminList.Count > 0)
        {
            for (Int32 i = 0; i < objAdminList.Count; i++)
            {
                sendVerificationReturnEmailToIE(OrderId, objAdminList[i].LoginEmail, objAdminList[i].FirstName, objAdminList[i].UserInfoID);
            }
        }
    }

    private void sendVerificationReturnEmailToIE(Int64 OrderId, String IEemailAddress, String FullName, Int64 UserInfoID)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            RegistrationBE objRegistrationBE = new RegistrationBE();
            RegistrationDA objRegistrationDA = new RegistrationDA();
            DataSet dsEmailTemplate;
            Common objcommm = new Common();

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Return";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
            UserInformation objUsrInfo = new UserInformation();

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                objUsrInfo = objUsrInfoRepo.GetById(IncentexGlobal.CurrentMember.UserInfoID);
                String sToadd = IEemailAddress;
                Int64 sToUserInfoID = UserInfoID;

                String sSubject = "Return Authorization - " + "RA" + lblOrderNo.Text;
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{Customername}", objUsrInfo.FirstName + " " + objUsrInfo.LastName);
                messagebody.Replace("{RA}", "RA" + lblOrderNo.Text);
                messagebody.Replace("{fullname}", FullName);
                messagebody.Replace("{OrderNumber}", lblOrderNo.Text);
                messagebody.Replace("{ReferenceName}", lblOrderBy.Text);
                messagebody.Replace("{OrderDate}", lblOrderedDate.Text);

                if (lblPaymentMethod.Text != "")
                {
                    messagebody.Replace("{PaymentType}", lblPaymentMethod.Text);
                }
                else
                {
                    messagebody.Replace("{PaymentType}", "");
                    messagebody.Replace("Payment Method :", "");
                }
                messagebody.Replace("{SentOn}", System.DateTime.Now.ToString());
                messagebody.Replace("{Status}", ddlRequesting.SelectedItem.Text);
                messagebody.Replace("{storename}", (new CompanyRepository().GetById((Int64)IncentexGlobal.CurrentMember.CompanyId).CompanyName).ToString());
                //Start WTDC Shipp To Address

                ManagedShipAddressRepository objManageRepos = new ManagedShipAddressRepository();
                MangedShipAddress objManage = new MangedShipAddress();
                objManage = objManageRepos.GetAllRecord();
                if (objManage != null)
                {
                    messagebody.Replace("{CompanyName}", objManage.CompanyName);
                    messagebody.Replace("{Title}", objManage.Title);
                    messagebody.Replace("{Address}", objManage.Address);
                    messagebody.Replace("{CityStateZip}", new CityRepository().GetById(Convert.ToInt64(objManage.CityId)).sCityName + "  " + new StateRepository().GetById(Convert.ToInt64(objManage.StateId)).sStatename + "  " + objManage.Zipcode);
                    messagebody.Replace("{Tel}", objManage.Telephone);
                }
                //End

                #region start
                String innermessage = "";

                foreach (GridViewRow grv in gvOrderReturn.Rows)
                {
                    if (((HiddenField)grv.FindControl("hfReturnQty")).Value != "" && (ddlRequesting.SelectedIndex != 0))
                    {
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='20%' style='font-weight: bold;'>";
                        innermessage = innermessage + ((HiddenField)grv.FindControl("hdnitemNo")).Value;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + ((HiddenField)grv.FindControl("hfReturnQty")).Value;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + ((Label)grv.FindControl("lblColor")).Text;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='20%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + ((Label)grv.FindControl("lblSize")).Text;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='35%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + ((Label)grv.FindControl("lblProductionDescription")).Text;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                    }
                }

                messagebody.Replace("{innermessage}", innermessage);

                #endregion

                messagebody.Replace("{reasonforreturn}", txtPrdDescription.Text);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ReturnConfirmations)) == true)
                {
                    new CommonMails().SendMail(sToUserInfoID, "Order Returns", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    //Add 30 Sep -2011 For One Particular User
    private void sendEmail(String OrderNumber)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            RegistrationBE objRegistrationBE = new RegistrationBE();
            RegistrationDA objRegistrationDA = new RegistrationDA();
            DataSet dsEmailTemplate;
            Common objcommm = new Common();

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Return";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
            UserInformation objUsrInfo = new UserInformation();

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                objUsrInfo = objUsrInfoRepo.GetById(IncentexGlobal.CurrentMember.UserInfoID);
                String sToadd = "";
                Int64 sToUserInfoID = 0;

                if (objUsrInfo != null)
                {
                    sToadd = "incentex@mdfsystems.com";
                    sToUserInfoID = objUsrInfo.UserInfoID;
                }

                String sSubject = "Return Authorization - " + "RA" + lblOrderNo.Text;
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{Customername}", objUsrInfo.FirstName + " " + objUsrInfo.LastName);
                messagebody.Replace("{RA}", "RA" + lblOrderNo.Text);
                messagebody.Replace("{fullname}", objUsrInfo.FirstName + " " + objUsrInfo.LastName);
                messagebody.Replace("{OrderNumber}", lblOrderNo.Text);
                messagebody.Replace("{ReferenceName}", lblOrderBy.Text);
                messagebody.Replace("{OrderDate}", lblOrderedDate.Text);

                if (lblPaymentMethod.Text != "")
                {
                    messagebody.Replace("{PaymentType}", lblPaymentMethod.Text);
                }
                else
                {
                    messagebody.Replace("{PaymentType}", "");
                    messagebody.Replace("Payment Method :", "");
                }

                messagebody.Replace("{SentOn}", System.DateTime.Now.ToString());
                messagebody.Replace("{Status}", ddlRequesting.SelectedItem.Text);
                messagebody.Replace("{storename}", (new CompanyRepository().GetById((Int64)IncentexGlobal.CurrentMember.CompanyId).CompanyName).ToString());

                //Start WTDC Shipp To Address

                ManagedShipAddressRepository objManageRepos = new ManagedShipAddressRepository();
                MangedShipAddress objManage = new MangedShipAddress();
                objManage = objManageRepos.GetAllRecord();

                if (objManage != null)
                {
                    messagebody.Replace("{CompanyName}", objManage.CompanyName);
                    messagebody.Replace("{Title}", objManage.Title);
                    messagebody.Replace("{Address}", objManage.Address);
                    messagebody.Replace("{CityStateZip}", new CityRepository().GetById(Convert.ToInt64(objManage.CityId)).sCityName + "  " + new StateRepository().GetById(Convert.ToInt64(objManage.StateId)).sStatename + "  " + objManage.Zipcode);
                    messagebody.Replace("{Tel}", objManage.Telephone);
                }
                //End

                #region start

                String innermessage = "";

                foreach (GridViewRow grv in gvOrderReturn.Rows)
                {
                    if (((HiddenField)grv.FindControl("hfReturnQty")).Value != "" && (ddlRequesting.SelectedIndex != 0))
                    {
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='20%' style='font-weight: bold;'>";
                        innermessage = innermessage + ((HiddenField)grv.FindControl("hdnitemNo")).Value;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + ((HiddenField)grv.FindControl("hfReturnQty")).Value;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + ((Label)grv.FindControl("lblColor")).Text;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='20%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + ((Label)grv.FindControl("lblSize")).Text;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='35%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + ((Label)grv.FindControl("lblProductionDescription")).Text;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "</tr>";
                    }
                }

                messagebody.Replace("{innermessage}", innermessage);

                #endregion

                messagebody.Replace("{reasonforreturn}", "");
                messagebody.Replace("Reason for Return:", "");
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ReturnConfirmations)) == true)
                {
                    new CommonMails().SendMail(sToUserInfoID, "Order Returns", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}