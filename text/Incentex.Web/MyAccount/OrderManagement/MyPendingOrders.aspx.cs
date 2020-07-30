/// <summary>
/// Module Name : MOAS(Manager order approval system)
/// Description : This page is viewing my pending MOAS order and also include functionality for approve/cancel/edit order.
/// Created : Mayur on 24-nov-2011
/// </summary>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using MagayaService;

public partial class MyAccount_OrderManagement_MyPendingOrders : PageBase
{
    #region Data Members
    AnniversaryProgramRepository.CompanyAnniversarySortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = AnniversaryProgramRepository.CompanyAnniversarySortExpType.OrderID;
            }
            return (AnniversaryProgramRepository.CompanyAnniversarySortExpType)ViewState["SortExp"];
        }
        set
        {
            ViewState["SortExp"] = value;
        }
    }

    Incentex.DAL.Common.DAEnums.SortOrderType SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"] = Incentex.DAL.Common.DAEnums.SortOrderType.Desc;
            }
            return (Incentex.DAL.Common.DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }
    PagedDataSource pds = new PagedDataSource();
    UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
    OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    CompanyRepository objCmpRepo = new CompanyRepository();
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    CompanyEmployeeContactInfo objShippingInfo;
    CompanyEmployeeContactInfo objBillingInfo;
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    MyIssuanceCartRepository objMyIssCartRepo = new MyIssuanceCartRepository();
    OrderMOASSystemRepository objOrderMOASSystemRepository = new OrderMOASSystemRepository();
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    LookupRepository objLookupRepository = new LookupRepository();
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    //IncentexBEDataContext db = new IncentexBEDataContext();

    public Int32 CurrentPage
    {
        get
        {
            if (this.ViewState["CurrentPage"] == null)
                return 0;
            else
                return Convert.ToInt16(this.ViewState["CurrentPage"]);
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }
    public Int32 PagerSize
    {
        get
        {
            if (this.ViewState["PagerSize"] == null)
                return Convert.ToInt32(Application["GRIDPAGERSIZE"]);
            else
                return Convert.ToInt16(this.ViewState["PagerSize"]);
        }
        set
        {
            this.ViewState["PagerSize"] = value;
        }
    }
    public Int32 FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt16(this.ViewState["FrmPg"]);
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }
    public Int32 ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return PagerSize;
            else
                return Convert.ToInt16(this.ViewState["ToPg"]);
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }
    List<SelectMyShoppingCartProductResult> objSelectMyShoppingCartProductResultList;
    Int32 WTDCSupplierID
    {
        get
        {
            if (ViewState["WTDCSupplierID"] == null)
            {
                ViewState["WTDCSupplierID"] = new SupplierRepository().GetSinglSupplierid("WTDC").SupplierID;
            }
            return Convert.ToInt32(ViewState["WTDCSupplierID"]);
        }
        set
        {
            ViewState["WTDCSupplierID"] = value;
        }
    }
    /// <summary>
    /// Set true when user click from his/her email button view pending shopping cart.
    /// </summary>
    Boolean IsFormEmail
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsFormEmail"]);
        }
        set
        {
            ViewState["IsFormEmail"] = value;
        }
    }
    Int64 UserInfoID
    {
        get
        {
            return Convert.ToInt64(ViewState["UserInfoID"]);
        }
        set
        {
            ViewState["UserInfoID"] = value;
        }
    }

    //String ConnectionString
    //{
    //    get
    //    {
    //        return ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
    //    }
    //}
    #endregion

    #region Event Handlers
    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["UserInfoID"]) && !String.IsNullOrEmpty(Request.QueryString["IsFormEmail"]))
            {
                this.IsFormEmail = Convert.ToBoolean(Request.QueryString["IsFormEmail"].ToString());
                this.UserInfoID = Convert.ToInt64(Request.QueryString["UserInfoID"].ToString());
            }
            else
            {
                this.IsFormEmail = false;
                this.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "My Pending Orders";
            if (IncentexGlobal.IsIEFromStore)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            }
            if (Request.QueryString["action"] != null)
            {
                if (Request.QueryString["action"].ToLower() == "cancel")
                    lblmsg.Text = "Order has been cancelled successfully.";
                else if (Request.QueryString["action"].ToLower() == "approve")
                    lblmsg.Text = "Order has been approved successfully.";
            }
            BindPendingOrdersGrid();
        }
    }

    protected void gvEmployeePendingOrders_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Sorting":
                if (this.SortExp.ToString() == e.CommandArgument.ToString())
                {
                    if (this.SortOrder == Incentex.DAL.Common.DAEnums.SortOrderType.Asc)
                        this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Desc;
                    else
                        this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
                }
                else
                {
                    this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
                    this.SortExp = (AnniversaryProgramRepository.CompanyAnniversarySortExpType)Enum.Parse(typeof(AnniversaryProgramRepository.CompanyAnniversarySortExpType), e.CommandArgument.ToString());
                }
                BindPendingOrdersGrid();
                break;
            case "OrderDetail":
                Response.Redirect("~/MyAccount/OrderManagement/EditOrderDetail.aspx?Id=" + e.CommandArgument);
                break;
            case "ApproveOrder":
                //db = new IncentexBEDataContext(ConnectionString);

                Order objOrder = objRepos.GetByOrderID(Convert.ToInt64(e.CommandArgument));
                UserInformation objUser = objUsrInfoRepo.GetById(Convert.ToInt64(objOrder.UserId));

                //Approve Order, Update Price Level and Send Notification 
                //Updated by Prashant (move the whole logic to the Function)
                ApproveOrder(objOrder, objUser);

                Response.Redirect("~/MyAccount/OrderManagement/MyPendingOrders.aspx?action=approve&IsFormEmail=" + this.IsFormEmail + "&UserInfoID=" + this.UserInfoID + "");
                break;
            case "CancelOrder":
                hdnCancelOrderID.Value = e.CommandArgument.ToString();
                //GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                // ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("mpeCancelOrder");
                //mpe.Show();
                mpeCancelOrder.Show();
                break;
        }
    }

    protected void gvEmployeePendingOrders_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((Label)e.Row.FindControl("lblOrderSubmitedDate")).Text = Convert.ToDateTime(((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[3]).ToShortDateString();

            Label lblOrderAmountView = new Label();
            lblOrderAmountView = (Label)e.Row.FindControl("lblOrderAmountView");
            Label lblMOASOrderAmountView = new Label();
            lblMOASOrderAmountView = (Label)e.Row.FindControl("lblMOASOrderAmountView");
            if (lblOrderAmountView != null && lblMOASOrderAmountView != null)
            {
                if (IncentexGlobal.CurrentMember.Usertype == (Int64)Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee || String.IsNullOrEmpty(lblMOASOrderAmountView.Text) || lblMOASOrderAmountView.Text == "0.00")
                {
                    lblOrderAmountView.Visible = true;
                    lblMOASOrderAmountView.Visible = false;
                }
                else
                {
                    lblOrderAmountView.Visible = false;
                    lblMOASOrderAmountView.Visible = true;
                }
            }
            else if (lblOrderAmountView != null)
            {
                lblOrderAmountView.Visible = true;
                lblMOASOrderAmountView.Visible = false;
            }

        }

    }

    protected void lnkbtnCancelNow_Click(Object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(hdnCancelOrderID.Value))
            {
                Order objCancelOrder = objRepos.GetByOrderID(Convert.ToInt64(hdnCancelOrderID.Value));
                UserInformation objCancelUser = objUsrInfoRepo.GetById(Convert.ToInt64(objCancelOrder.UserId));

                objCancelOrder.OrderStatus = "Canceled";
                objCancelOrder.UpdatedDate = DateTime.Now;
                objRepos.SubmitChanges();

                //start changes added by mayur for maintain history of manager cancel order on 6-feb-2012
                OrderMOASSystem objOrderMOASSystem = objOrderMOASSystemRepository.GetByOrderIDAndManagerUserInfoID(objCancelOrder.OrderID, this.UserInfoID);
                objOrderMOASSystem.Status = "Canceled";
                objOrderMOASSystem.DateAffected = DateTime.Now;
                objOrderMOASSystemRepository.SubmitChanges();
                //end changes added by mayur for maintain history of manager cancel order on 6-feb-2012

                AddNoteHistory("Canceled", objCancelOrder.OrderID);

                #region Give back the credit amount he has used for this order
                if (objCancelOrder.CreditUsed != null)
                {
                    CompanyEmployeeRepository objCmnyEmp = new CompanyEmployeeRepository();
                    CompanyEmployee cmpEmpl = objCmnyEmp.GetByUserInfoId((Int64)objCancelOrder.UserId);
                    if (objCancelOrder.CreditUsed == "Previous")
                    {
                        cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + objCancelOrder.CreditAmt;
                        objCmnyEmp.SubmitChanges();
                    }
                    else if (objCancelOrder.CreditUsed == "Anniversary")
                    {
                        cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + objCancelOrder.CreditAmt;
                        objCmnyEmp.SubmitChanges();
                    }
                }
                #endregion

                #region update inventory and transfer order to shopping cart
                List<Order> obj = objRepos.GetShoppingCartId(objCancelOrder.OrderNumber);
                if (obj.Count > 0)
                {
                    String[] a;

                    a = obj[0].MyShoppingCartID.ToString().Split(',');

                    foreach (String u in a)
                    {
                        if (objCancelOrder.OrderFor == "ShoppingCart")
                        {
                            //Shopping cart
                            MyShoppinCart objShoppingcart = new MyShoppinCart();
                            ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                            ProductItem objProductItem = new ProductItem();
                            objShoppingcart = objShoppingCartRepository.GetById(Convert.ToInt32(u), (Int64)objCancelOrder.UserId);

                            objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(objShoppingcart.StoreProductID), Convert.ToInt64(objShoppingcart.MasterItemNo), objShoppingcart.ItemNumber);
                            //Update Inventory Here 
                            //Call here upDate Procedure
                            String strProcess = "Shopping";
                            String strMessage = objRepos.IncreaseDescreaseInventory(objProductItem.ProductItemID, -Convert.ToInt32(objShoppingcart.Quantity), strProcess);

                        }
                        else
                        {
                            //Issuance
                            MyIssuanceCartRepository objIssuanceRepos = new MyIssuanceCartRepository();
                            MyIssuanceCart objIssuance = new MyIssuanceCart();
                            ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                            //End 

                            objIssuance = objIssuanceRepos.GetById(Convert.ToInt32(u), (Int64)objCancelOrder.UserId);
                            List<SelectProductIDResult> objList = objProItemRepos.GetProductId(objIssuance.MasterItemID, Convert.ToInt64(u), Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(objCancelOrder.StoreID));
                            //Update Inventory Here 
                            //Call here upDate Procedure
                            for (Int32 i = 0; i < objList.Count; i++)
                            {
                                String strProcess = "UniformIssuance";
                                String strMessage = objRepos.IncreaseDescreaseInventory(Convert.ToInt64(objList[i].ProductItemID), -Convert.ToInt32(objIssuance.Qty), strProcess);
                                //String strMessage = objRepos.UpdateInventory(Convert.ToInt64(u), Convert.ToInt64(objList[i].ProductItemID), strProcess);
                            }
                        }
                    }
                }
                #endregion

                sendVerificationEmail("Canceled", objCancelOrder.OrderID, objCancelUser.LoginEmail, objCancelUser.FirstName, objCancelUser.UserInfoID, objCancelOrder); //send email to CE 

                if (!objCancelOrder.ReferenceName.ToLower().Contains("test") && !IncentexGlobal.IsIEFromStoreTestMode)
                {
                    sendIEEmail("Canceled", objCancelOrder.OrderID);//send email to IE
                }
                else
                {
                    sendTestOrderEmailNotification("Canceled", objCancelOrder.OrderID);
                }

                Response.Redirect("~/MyAccount/OrderManagement/MyPendingOrders.aspx?action=cancel");
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

        }
    }

    protected void lnkBtnApproveOrder_Click(Object sender, EventArgs e)
    {
        foreach (GridViewRow grv in gvEmployeePendingOrders.Rows)
        {
            if (((CheckBox)grv.FindControl("chkSelectOrder")).Checked == true)
            {
                Label lblOrderID = (Label)grv.FindControl("lblOrderID");
                Order objOrder = objRepos.GetByOrderID(Convert.ToInt64(lblOrderID.Text));
                UserInformation objUser = objUsrInfoRepo.GetById(Convert.ToInt64(objOrder.UserId));


                ApproveOrder(objOrder, objUser);
            }
        }
        Response.Redirect("~/MyAccount/OrderManagement/MyPendingOrders.aspx?action=approve");
    }
    #endregion

    #region Methods
    /// <summary>
    /// Binds the pending orders grid.
    /// Bind Paging datasource and set paging property
    /// </summary>
    private void BindPendingOrdersGrid()
    {
        DataView myDataView = new DataView();
        List<GetUserPendingOrdersToApproveResult> objExtList = new List<GetUserPendingOrdersToApproveResult>();
        objExtList = objRepos.GetMyPendingOrders(this.UserInfoID, this.SortExp, this.SortOrder);

        if (objExtList.Count == 0)
        {
            pagingtable.Visible = false;
            lnkBtnApproveOrder.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
            lnkBtnApproveOrder.Visible = true;
        }

        DataTable dataTable = Common.ListToDataTable(objExtList);
        myDataView = dataTable.DefaultView;
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gvEmployeePendingOrders.DataSource = pds;
        gvEmployeePendingOrders.DataBind();
        doPaging();
    }


    /// <summary>
    /// Sends the verification email.
    /// </summary>
    /// <param name="strStatus">New order status.</param>
    /// <param name="OrderID">The order id.</param>
    /// /// <param name="ToAdd">Email To address.</param>
    private void sendVerificationEmail(String strStatus, Int64 OrderID, String ToAdd, String FullName, Int64 UserInfoID, Order objOrder)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Placed";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                //Order objOrder = objRepos.GetByOrderID(OrderID);
                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                CompanyEmployeeRepository objCERepo = new CompanyEmployeeRepository();
                CompanyEmployee objCompanyEmployee = objCERepo.GetByUserInfoId(objUserInformation.UserInfoID);

                Boolean IsMOASWithCostCenterCode = objCERepo.FUN_IsMOASWithCostCenterCode(Convert.ToInt64(objOrder.UserId));

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                String sSubject = objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber.ToString();
                String sToadd = ToAdd;

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());

                String strFirstNote = "";
                if (strStatus == "Canceled")
                {
                    strFirstNote = "Your ordered has been review by your manager and at this time has been cancelled.";
                    strFirstNote += "<br/><br/><strong>Managers Reason for Cancelling: </strong>";
                    strFirstNote += txtCancelReason.Text;
                }
                else if (strStatus == "Approved")
                    strFirstNote = "You order has been reviewed by your manager and is approved. This order has been successfully submitted into our order processing system.";

                messagebody.Replace("{firstnote}", strFirstNote);
                messagebody.Replace("{FullName}", FullName);
                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName);
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());
                //Check here for CreditUsed
                String cr;
                Boolean creditused = true;
                if (objOrder.CreditUsed != "0")
                {
                    if (String.IsNullOrEmpty(objOrder.CreditUsed))
                    {
                        creditused = false;
                    }
                    else
                    {
                        creditused = true;
                    }
                }
                else
                {
                    creditused = false;
                }

                //Both Option have used
                if (objOrder.PaymentOption != null && creditused)
                {

                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits Paid by Corporate ";
                        /*Strating Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", objOrder.CreditAmt.ToString());
                        /*End*/
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits Paid by Corporate ";
                        /*Anniversary Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                        /*End*/

                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                }
                //Only Starting or Anniversary
                else if (objOrder.PaymentOption == null && creditused)
                {
                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr);
                }
                //Only Payment Option
                else if (objOrder.PaymentOption != null && !creditused)
                {
                    messagebody.Replace("{PaymentType}", new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                }
                else
                {
                    messagebody.Replace("{PaymentType}", "Paid By Corporate");
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/

                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                }
                messagebody.Replace("{Payment Method :}", "Payment Method :");

                #region PaymentOptionCode
                //For displaying payment option code
                if (IsMOASWithCostCenterCode)
                {
                    messagebody.Replace("{PaymentOptionCodeLabel}", "Cost Center Code :");
                    messagebody.Replace("{PaymentOptionCodeValue}", objOrder.PaymentOptionCode);
                }
                else
                {
                    messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
                    messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);
                }
                #endregion

                #region Billing Address
                if (objOrder.OrderFor == "ShoppingCart") // Company Pays
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                else
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }

                messagebody.Replace("{Billing Address}", "Bill To:");
                messagebody.Replace("{NameView}", "Name :");
                messagebody.Replace("{CompanyNameView}", "Company Name :");
                messagebody.Replace("{AddressView}", "Address :");
                messagebody.Replace("{CityView}", "City :");
                messagebody.Replace("{StateView}", "State :");
                messagebody.Replace("{ZipView}", "Zip :");
                messagebody.Replace("{CountryView}", "Country :");
                messagebody.Replace("{PhoneView}", "Phone :");
                messagebody.Replace("{EmailView}", "Email :");


                messagebody.Replace("{Name}", objBillingInfo.BillingCO + " " + objBillingInfo.Manager);
                messagebody.Replace("{CompanyName}", objBillingInfo.CompanyName);
                messagebody.Replace("{Address1}", objBillingInfo.Address + "<br/>" + objBillingInfo.Address2);
                // messagebody.Replace("{Address2}", lblBadr.Text);
                messagebody.Replace("{City}", objCity.GetById((Int64)objBillingInfo.CityID).sCityName);
                messagebody.Replace("{State}", objState.GetById((Int64)objBillingInfo.StateID).sStatename);
                messagebody.Replace("{Zip}", objBillingInfo.ZipCode);
                messagebody.Replace("{Country}", objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone}", objBillingInfo.Telephone);
                messagebody.Replace("{Email}", objBillingInfo.Email);
                #endregion

                #region shipping address
                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                }
                else
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                }
                messagebody.Replace("{Shipping Address}", "Ship To:");
                messagebody.Replace("{AirportView1}", "Airport :");
                messagebody.Replace("{DepartmentView1}", "Department :");
                messagebody.Replace("{NameView1}", "Name :");
                messagebody.Replace("{CompanyNameView1}", "Company Name :");
                messagebody.Replace("{AddressView1}", "Address :");
                messagebody.Replace("{CityView1}", "City :");
                messagebody.Replace("{CountyView1}", "County :");
                messagebody.Replace("{StateView1}", "State :");
                messagebody.Replace("{ZipView1}", "Zip :");
                messagebody.Replace("{CountryView1}", "Country :");
                messagebody.Replace("{PhoneView1}", "Phone :");
                messagebody.Replace("{EmailView1}", "Email :");

                messagebody.Replace("{Airport1}", objShippingInfo.Airport);
                if (objShippingInfo.DepartmentID != null)
                    messagebody.Replace("{Department1}", new LookupRepository().GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
                else
                    messagebody.Replace("{Department1}", "");
                messagebody.Replace("{Name1}", objShippingInfo.Name + " " + objShippingInfo.Fax);
                messagebody.Replace("{CompanyName1}", objShippingInfo.CompanyName);
                messagebody.Replace("{Address11}", objShippingInfo.Address + "<br/>" + objShippingInfo.Address2 + "<br/>" + objShippingInfo.Street);
                messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
                messagebody.Replace("{County1}", objShippingInfo.county);
                messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
                messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
                messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
                messagebody.Replace("{Email1}", objShippingInfo.Email);

                // WL ContactID Info
                messagebody.Replace("{OrderPlacedBy}", String.Empty);
                messagebody.Replace("{WLContactID}", String.Empty);
                #endregion

                #region start
                String innermessage = "";
                Boolean GLCodeExists = false;

                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                    GLCodeExists = objSelectMyShoppingCartProductResultList.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                    {
                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objSelectMyShoppingCartProductResultList[i].NameToBeEngraved))
                        {
                            p = objSelectMyShoppingCartProductResultList[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Quantity;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Size;
                        innermessage = innermessage + "</td>";

                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].GLCode;
                            innermessage = innermessage + "</td>";
                        }

                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice) * Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].Quantity);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";

                        if (GLCodeExists)
                            innermessage = innermessage + "<td colspan='8'>";
                        else
                            innermessage = innermessage + "<td colspan='7'>";

                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }
                    messagebody.Replace("{innermessage}", innermessage);
                }
                else
                {
                    List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                    GLCodeExists = objFinal.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objFinal.Count; i++)
                    {
                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objFinal[i].NameToBeEngraved))
                        {
                            p = objFinal[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].Qty;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].sLookupName;
                        innermessage = innermessage + "</td>";

                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].GLCode;
                            innermessage = innermessage + "</td>";
                        }

                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate) * Convert.ToDecimal(objFinal[i].Qty);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";

                        if (GLCodeExists)
                            innermessage = innermessage + "<td colspan='8'>";
                        else
                            innermessage = innermessage + "<td colspan='7'>";

                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }

                    messagebody.Replace("{innermessage}", innermessage);

                }

                if (GLCodeExists)
                    messagebody.Replace("<td id=\"tdPD\" width=\"48%\"", "<td id=\"tdPD\" width=\"38%\"");
                else
                    messagebody.Replace(@"<td id=""tdGC"" width=""10%"" style=""font-weight: bold; text-align: center;"">
                                                    GL Code
                                                </td>", "");

                #endregion

                messagebody.Replace("{Order Notes:}", "Order Notes :");
                messagebody.Replace("{ShippingCostView}", "Shipping :");
                messagebody.Replace("{SalesTaxView}", "Sales Tax :");
                messagebody.Replace("{OrderTotalView}", "Order Total :");
                messagebody.Replace("{innermesaageforsupplier}", "");
                String b = NameBars(objOrder.OrderID);
                if (b != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + b);
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                }

                messagebody.Replace("{ShippingCost}", objOrder.ShippingAmount.ToString());
                messagebody.Replace("{Saletax}", "$" + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)).ToString());
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + objOrder.OrderAmount).ToString());
                if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)//check that corporate discount is applied or not
                {
                    messagebody.Replace("{CorporateDiscountView}", "Corporate Discount");
                    messagebody.Replace("{CorporateDiscount}", objOrder.CorporateDiscount.ToString());
                }
                else
                {
                    messagebody.Replace("{CorporateDiscountView}", "");
                    messagebody.Replace("{CorporateDiscount}", "");
                }
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                //String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                //String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.MOAS)) == true)
                {
                    new CommonMails().SendMail(objUserInformation.UserInfoID, "MOAS From My Pending Orders", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Sends the email to incentex admin.
    /// </summary>
    private void sendIEEmail(String strStatus, Int64 OrderID)
    {
        List<UserInformation> objAdminList = new List<UserInformation>();
        objAdminList = new UserInformationRepository().GetEmailInformation();
        if (objAdminList.Count > 0)
        {
            //* ---------- Change for optimization By gaurang 16 MAR 2013 Start-----------------*/
            Order objOrder = objRepos.GetByOrderID(OrderID);
            UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
            CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID);
            //* ---------- Change for optimization By gaurang 16 MAR 2013 END-----------------*/

            for (Int32 i = 0; i < objAdminList.Count; i++)
            {
                sendVerificationEmailAdmin(strStatus, OrderID, objAdminList[i].LoginEmail, objAdminList[i].FirstName, objAdminList[i].UserInfoID, false, objUserInformation, objCompanyEmployee, objOrder);
            }
        }
    }

    /// <summary>
    /// Sends the email to incentex admin.
    /// </summary>
    private void sendTestOrderEmailNotification(String strStatus, Int64 OrderID)
    {
        List<FUN_GetTestOrderEmailReceiversResult> objAdminList = new List<FUN_GetTestOrderEmailReceiversResult>();
        objAdminList = new IncentexBEDataContext().FUN_GetTestOrderEmailReceivers().ToList();
        if (objAdminList.Count > 0)
        {
            //* ---------- Change for optimization By gaurang 16 MAR 2013 Start-----------------*/
            Order objOrder = objRepos.GetByOrderID(OrderID);
            UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
            CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID);
            //* ---------- Change for optimization By gaurang 16 MAR 2013 END-----------------*/

            foreach (FUN_GetTestOrderEmailReceiversResult receiver in objAdminList)
            {
                sendVerificationEmailAdmin(strStatus, OrderID, receiver.LoginEmail, receiver.FirstName, Convert.ToInt64(receiver.UserInfoID), true, objUserInformation, objCompanyEmployee, objOrder);
                //sendVerificationEmailAdmin(strStatus, OrderID, "prashanth.kankhara@indianic.com", receiver.FirstName, Convert.ToInt64(receiver.UserInfoID), true);

            }
        }
    }

    /// <summary>
    /// Sends the verification email to incentex admin.
    /// </summary>
    /// <param name="strStatus">New order status.</param>
    /// <param name="OrderID">The order id.</param>
    /// /// <param name="ToAdd">receiver email address.</param>
    /// /// /// <param name="ToAdd">full name of receiver.</param>
    private void sendVerificationEmailAdmin(String strStatus, Int64 OrderID, String ToAdd, String FullName, Int64 UserInfoID, Boolean IsTestOrder, UserInformation objUserInformation, CompanyEmployee objCompanyEmployee, Order objOrder)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Placed";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                //Order objOrder = objRepos.GetByOrderID(OrderID);
                //UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                //CompanyEmployee objCompanyEmployee = objCERepo.GetByUserInfoId(objUserInformation.UserInfoID);


                Boolean IsMOASWithCostCenterCode = objCompanyEmployeeRepository.FUN_IsMOASWithCostCenterCode(Convert.ToInt64(objOrder.UserId));

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                String sSubject = objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber.ToString();
                String sToadd = ToAdd;
                Int64 sToUserInfoID = UserInfoID;

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());

                messagebody.Replace("{firstnote}", "Thank you for your business you can check the status of order(s) any time by logging into your store <a href='http://www.world-link.us.com' title='Incentex'>here.</a>");
                messagebody.Replace("{FullName}", FullName);
                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName);
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());

                // Set SAP Company Contact ID and Full name
                String OrderPlacedBy = String.Empty;
                String WLContactID = String.Empty;
                GetSAPCompanyCodeIDResult objSAPCompanyResult = objUsrInfoRepo.GetSAPCompanyCodeID(objUserInformation.UserInfoID);
                if (objSAPCompanyResult != null)
                {
                    OrderPlacedBy = String.Format("Order Placed By: {0}", objSAPCompanyResult.FullName);
                    WLContactID = String.Format("World-Link Contact ID : {0}", objSAPCompanyResult.SAPCompanyCodeID);
                }
                //Check here for CreditUsed
                String cr;
                Boolean creditused = true;
                if (objOrder.CreditUsed != "0")
                {
                    if (String.IsNullOrEmpty(objOrder.CreditUsed))
                    {
                        creditused = false;
                    }
                    else
                    {
                        creditused = true;
                    }
                }
                else
                {
                    creditused = false;
                }

                //Both Option have used
                if (objOrder.PaymentOption != null && creditused)
                {

                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits Paid by Corporate ";
                        /*Strating Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", objOrder.CreditAmt.ToString());
                        /*End*/
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits Paid by Corporate ";
                        /*Anniversary Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                        /*End*/

                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                }
                //Only Starting or Anniversary
                else if (objOrder.PaymentOption == null && creditused)
                {
                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr);
                }
                //Only Payment Option
                else if (objOrder.PaymentOption != null && !creditused)
                {
                    messagebody.Replace("{PaymentType}", new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                }
                else
                {
                    messagebody.Replace("{PaymentType}", "Paid By Corporate");
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/

                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                }
                messagebody.Replace("{Payment Method :}", "Payment Method :");

                #region PaymentOptionCode
                //For displaying payment option code
                if (IsMOASWithCostCenterCode)
                {
                    messagebody.Replace("{PaymentOptionCodeLabel}", "Cost Center Code :");
                    messagebody.Replace("{PaymentOptionCodeValue}", objOrder.PaymentOptionCode);
                }
                else
                {
                    messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
                    messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);
                }
                #endregion

                #region Billing Address
                if (objOrder.OrderFor == "ShoppingCart") // Company Pays
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                else
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }

                messagebody.Replace("{Billing Address}", "Bill To:");
                messagebody.Replace("{NameView}", "Name :");
                messagebody.Replace("{CompanyNameView}", "Company Name :");
                messagebody.Replace("{AddressView}", "Address :");
                messagebody.Replace("{CityView}", "City :");
                messagebody.Replace("{StateView}", "State :");
                messagebody.Replace("{ZipView}", "Zip :");
                messagebody.Replace("{CountryView}", "Country :");
                messagebody.Replace("{PhoneView}", "Phone :");
                messagebody.Replace("{EmailView}", "Email :");


                messagebody.Replace("{Name}", objBillingInfo.BillingCO + " " + objBillingInfo.Manager);
                messagebody.Replace("{CompanyName}", objBillingInfo.CompanyName);
                messagebody.Replace("{Address1}", objBillingInfo.Address + "<br/>" + objBillingInfo.Address2);
                // messagebody.Replace("{Address2}", lblBadr.Text);
                messagebody.Replace("{City}", objCity.GetById((Int64)objBillingInfo.CityID).sCityName);
                messagebody.Replace("{State}", objState.GetById((Int64)objBillingInfo.StateID).sStatename);
                messagebody.Replace("{Zip}", objBillingInfo.ZipCode);
                messagebody.Replace("{Country}", objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone}", objBillingInfo.Telephone);
                messagebody.Replace("{Email}", objBillingInfo.Email);
                #endregion

                #region shipping address
                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                }
                else
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                }
                messagebody.Replace("{Shipping Address}", "Ship To:");
                messagebody.Replace("{AirportView1}", "Airport :");
                messagebody.Replace("{DepartmentView1}", "Department :");
                messagebody.Replace("{NameView1}", "Name :");
                messagebody.Replace("{CompanyNameView1}", "Company Name :");
                messagebody.Replace("{AddressView1}", "Address :");
                messagebody.Replace("{CityView1}", "City :");
                messagebody.Replace("{CountyView1}", "County :");
                messagebody.Replace("{StateView1}", "State :");
                messagebody.Replace("{ZipView1}", "Zip :");
                messagebody.Replace("{CountryView1}", "Country :");
                messagebody.Replace("{PhoneView1}", "Phone :");
                messagebody.Replace("{EmailView1}", "Email :");

                messagebody.Replace("{Airport1}", objShippingInfo.Airport);
                if (objShippingInfo.DepartmentID != null)
                    messagebody.Replace("{Department1}", new LookupRepository().GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
                else
                    messagebody.Replace("{Department1}", "");
                messagebody.Replace("{Name1}", objShippingInfo.Name + " " + objShippingInfo.Fax);
                messagebody.Replace("{CompanyName1}", objShippingInfo.CompanyName);
                messagebody.Replace("{Address11}", objShippingInfo.Address + "<br/>" + objShippingInfo.Address2 + "<br/>" + objShippingInfo.Street);
                messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
                messagebody.Replace("{County1}", objShippingInfo.county);
                messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
                messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
                messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
                messagebody.Replace("{Email1}", objShippingInfo.Email);

                // WL ContactID Info
                messagebody.Replace("{OrderPlacedBy}", OrderPlacedBy);
                messagebody.Replace("{WLContactID}", WLContactID);
                #endregion

                #region start
                String innermessage = "";
                Boolean GLCodeExists = false;

                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                    GLCodeExists = objSelectMyShoppingCartProductResultList.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                    {
                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objSelectMyShoppingCartProductResultList[i].NameToBeEngraved))
                        {
                            p = objSelectMyShoppingCartProductResultList[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Quantity;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Size;
                        innermessage = innermessage + "</td>";

                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].GLCode;
                            innermessage = innermessage + "</td>";
                        }

                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)).ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)) * Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].Quantity);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";

                        if (GLCodeExists)
                            innermessage = innermessage + "<td colspan='8'>";
                        else
                            innermessage = innermessage + "<td colspan='7'>";

                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }
                    messagebody.Replace("{innermessage}", innermessage);
                }
                else
                {
                    List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                    GLCodeExists = objFinal.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objFinal.Count; i++)
                    {
                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objFinal[i].NameToBeEngraved))
                        {
                            p = objFinal[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].Qty;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].sLookupName;
                        innermessage = innermessage + "</td>";

                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].GLCode;
                            innermessage = innermessage + "</td>";
                        }

                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)).ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)) * Convert.ToDecimal(objFinal[i].Qty);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";

                        if (GLCodeExists)
                            innermessage = innermessage + "<td colspan='8'>";
                        else
                            innermessage = innermessage + "<td colspan='7'>";

                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }

                    messagebody.Replace("{innermessage}", innermessage);

                }

                if (GLCodeExists)
                    messagebody.Replace("<td id=\"tdPD\" width=\"48%\"", "<td id=\"tdPD\" width=\"38%\"");
                else
                    messagebody.Replace(@"<td id=""tdGC"" width=""10%"" style=""font-weight: bold; text-align: center;"">
                                                    GL Code
                                                </td>", "");
                #endregion

                messagebody.Replace("{Order Notes:}", "Order Notes :");
                messagebody.Replace("{ShippingCostView}", "Shipping :");
                messagebody.Replace("{SalesTaxView}", "Sales Tax :");
                messagebody.Replace("{OrderTotalView}", "Order Total :");
                messagebody.Replace("{innermesaageforsupplier}", "");
                String b = NameBars(objOrder.OrderID);
                if (b != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + b + (IsTestOrder ? @"<br/><span style=""color:red;font-weight:bold;"">THIS IS A TEST ORDER. PLEASE DO NOT PROCESS.</span>" : ""));
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + (IsTestOrder ? @"<br/><span style=""color:red;font-weight:bold;"">THIS IS A TEST ORDER. PLEASE DO NOT PROCESS.</span>" : ""));
                }

                messagebody.Replace("{ShippingCost}", objOrder.ShippingAmount.ToString());
                messagebody.Replace("{Saletax}", "$" + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)).ToString());
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + (objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount))).ToString());
                if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)//check that corporate discount is applied or not
                {
                    messagebody.Replace("{CorporateDiscountView}", "Corporate Discount");
                    messagebody.Replace("{CorporateDiscount}", objOrder.CorporateDiscount.ToString());
                }
                else
                {
                    messagebody.Replace("{CorporateDiscountView}", "");
                    messagebody.Replace("{CorporateDiscount}", "");
                }
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                //String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                //String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.MOAS)) == true)
                {
                    new CommonMails().SendMail(sToUserInfoID, "MOAS From My Pending Orders", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                }
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Sends the email to supplier.
    /// </summary>
    /// <param name="OrderID">The order id.</param>
    private void sendEmailToSupplier(Int64 orderid, CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo)
    {
        try
        {
            Order objOrder = objRepos.GetByOrderID(orderid);
            if (objOrder != null)
            {
                List<SelectSupplierAddressResult> obj = objRepos.GetSupplierAddress(objOrder.OrderFor, objOrder.OrderID);

                //* ---------- Change for optimization By gaurang 16 MAR 2013 Start-----------------*/
                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                //* ---------- Change for optimization By gaurang 16 MAR 2013 END-----------------*/

                foreach (SelectSupplierAddressResult repeaterItem in obj)
                {
                    sendVerificationEmailSupplier(orderid, objOrder.MyShoppingCartID, repeaterItem.SupplierID, repeaterItem.Name, objShippingInfo, repeaterItem.CompanyName, objUserInformation, objOrder);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Sends the verification email supplier.
    /// </summary>
    /// <param name="OrderID">The order id.</param>
    /// <param name="ShoppingCartID">The shopping cart ID.</param>
    /// <param name="supplierId">The supplier id.</param>
    /// <param name="fullName">The full name.</param>
    private void sendVerificationEmailSupplier(Int64 OrderId, String ShoppingCartID, Int64 supplierId, String fullName, CompanyEmployeeContactInfo objShippingInfo, String SupplierCompanyName, UserInformation objUserInformation, Order objOrder)
    {
        try
        {
            //Get supplierinfo by id
            UserInformation objUserInfo = objUsrInfoRepo.GetById(new SupplierRepository().GetById(supplierId).UserInfoID);

            if (objUserInfo != null)
            {
                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(objUserInfo.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderConfirmations)) == true)
                {

                    CityRepository objCity = new CityRepository();
                    StateRepository objState = new StateRepository();
                    CountryRepository objCountry = new CountryRepository();
                    EmailTemplateBE objEmailBE = new EmailTemplateBE();
                    EmailTemplateDA objEmailDA = new EmailTemplateDA();
                    DataSet dsEmailTemplate;

                    //Get Email Content
                    objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
                    objEmailBE.STemplateName = "Order Placed Supplier";
                    dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
                    if (dsEmailTemplate != null)
                    {
                        //Find UserName who had order purchased
                        //Order objOrder = objRepos.GetByOrderID(OrderId);
                        //UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);

                        String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                        String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                        String sSubject = objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber;
                        String sToadd = objUserInfo.LoginEmail;

                        StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                        StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());
                        messagebody.Replace("{firstnote}", "Please review the following order below if you have any questions please post them to the notes section of this order located within the order in the system.");
                        messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                        messagebody.Replace("{referencename}", objOrder.ReferenceName + "<br/>Workgroup : " + objLookupRepository.GetById((Int64)(objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID).WorkgroupID)).sLookupName); // change by mayur on 26-March-2012
                        messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
                        //messagebody.Replace("{ReferenceName}", lblPurchasedBy.Text);
                        messagebody.Replace("{FullName}", fullName);
                        messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());

                        String PaymentMethod = String.Empty;
                        if (objOrder.PaymentOption != null)
                            PaymentMethod = objLookupRepository.GetById((Int64)objOrder.PaymentOption).sLookupName;

                        //Added on 20 Sep 2011
                        //Check here for CreditUsed
                        String cr;
                        Boolean creditused = true;
                        if (objOrder.CreditUsed != "0")
                        {
                            if (String.IsNullOrEmpty(objOrder.CreditUsed))
                            {
                                creditused = false;
                            }
                            else
                            {
                                creditused = true;
                            }
                        }
                        else
                        {
                            creditused = false;
                        }

                        //Both Option have used
                        if (objOrder.PaymentOption != null && creditused)
                        {

                            if (objOrder.CreditUsed == "Previous")
                            {
                                cr = "Starting " + "Credits Paid by Corporate ";
                            }
                            else
                            {
                                cr = "Anniversary " + "Credits Paid by Corporate ";
                            }
                            messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + PaymentMethod);
                        }
                        //Only Starting or Anniversary
                        else if (objOrder.PaymentOption == null && creditused)
                        {
                            if (objOrder.CreditUsed == "Previous")
                            {
                                cr = "Starting " + "Credits - Paid by Corporate ";
                            }
                            else
                            {
                                cr = "Anniversary " + "Credits - Paid by Corporate ";
                            }
                            messagebody.Replace("{PaymentType}", cr);
                        }
                        //Only Payment Option
                        else if (objOrder.PaymentOption != null && !creditused)
                        {
                            messagebody.Replace("{PaymentType}", PaymentMethod);
                        }
                        else
                        {
                            messagebody.Replace("{PaymentType}", "Paid By Corporate");
                        }
                        messagebody.Replace("{Payment Method :}", "Payment Method :");

                        //For displaying payment option code
                        messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
                        messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);

                        //End
                        //messagebody.Replace("{CreditType}", "---");

                        #region shipping address
                        messagebody.Replace("{Shipping Address}", "Ship To:");
                        messagebody.Replace("{AirportView1}", "Airport :");
                        messagebody.Replace("{DepartmentView1}", "Department :");
                        messagebody.Replace("{NameView1}", "Name :");
                        messagebody.Replace("{CompanyNameView1}", "Company Name :");
                        messagebody.Replace("{AddressView1}", "Address :");
                        // messagebody.Replace("{Address2}", lblBadr.Text);
                        messagebody.Replace("{CityView1}", "City :");
                        messagebody.Replace("{CountyView1}", "County :");
                        messagebody.Replace("{StateView1}", "State :");
                        messagebody.Replace("{ZipView1}", "Zip :");
                        messagebody.Replace("{CountryView1}", "Country :");
                        messagebody.Replace("{PhoneView1}", "Phone :");
                        messagebody.Replace("{EmailView1}", "Email :");

                        messagebody.Replace("{Airport1}", objShippingInfo.Airport);
                        if (objShippingInfo.DepartmentID != null)
                            messagebody.Replace("{Department1}", new LookupRepository().GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
                        else
                            messagebody.Replace("{Department1}", "");
                        messagebody.Replace("{Name1}", objShippingInfo.Name + " " + objShippingInfo.Fax);
                        messagebody.Replace("{CompanyName1}", objShippingInfo.CompanyName);
                        messagebody.Replace("{Address11}", objShippingInfo.Address + "<br/>" + objShippingInfo.Address2 + "<br/>" + objShippingInfo.Street);
                        //messagebody.Replace("{Address22}", lbl.Text);
                        messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
                        messagebody.Replace("{County1}", objShippingInfo.county);
                        messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
                        messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
                        messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
                        messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
                        messagebody.Replace("{Email1}", objShippingInfo.Email);
                        #endregion

                        String innermessageSupplier = "";
                        Boolean GLCodeExists = false;

                        #region Supplier

                        List<SelectOrderDetailsForSupplierResult> lstSupplierItemsFromOrder = objRepos.GetOrderItemsForSupplier(supplierId, objOrder.OrderID);

                        if (lstSupplierItemsFromOrder.Count > 0)
                        {
                            GLCodeExists = lstSupplierItemsFromOrder.FirstOrDefault(le => le.GLCode != null && le.GLCode != "") != null;
                            Int16 ColSpan = 9;

                            if (GLCodeExists)
                                ColSpan = 10;

                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                            innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td align='center' colspan='2'  style='background-color:Gray;font-weight:bold;color:Black;'>";
                            innermessageSupplier = innermessageSupplier + SupplierCompanyName;
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";
                            innermessageSupplier = innermessageSupplier + "</table>";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";
                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                            innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: left;'>";
                            innermessageSupplier = innermessageSupplier + "Ordered";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Item#";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Size";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Color";
                            innermessageSupplier = innermessageSupplier + "</td>";

                            if (!GLCodeExists)
                            {
                                innermessageSupplier = innermessageSupplier + "<td width='35%' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "Description";
                                innermessageSupplier = innermessageSupplier + "</td>";
                            }
                            else
                            {
                                innermessageSupplier = innermessageSupplier + "<td width='25%' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "Description";
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "GL Code";
                                innermessageSupplier = innermessageSupplier + "</td>";
                            }

                            //Add Nagmani 18-Jan-2012
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Unit Price";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Extended Price";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            //End

                            innermessageSupplier = innermessageSupplier + "</table>";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";
                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                            innermessageSupplier = innermessageSupplier + "<hr />";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";

                            foreach (SelectOrderDetailsForSupplierResult item in lstSupplierItemsFromOrder)
                            {
                                innermessageSupplier = innermessageSupplier + "<tr>";
                                innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                                innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                                innermessageSupplier = innermessageSupplier + "<tr>";
                                innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: left;'>";
                                innermessageSupplier = innermessageSupplier + item.Quantity;
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='15%' height='50' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + item.ItemNumber;
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + item.Size;
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + item.Color;
                                innermessageSupplier = innermessageSupplier + "</td>";

                                if (!GLCodeExists)
                                {
                                    innermessageSupplier = innermessageSupplier + "<td width='35%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + item.ProductDescrption;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                }
                                else
                                {
                                    innermessageSupplier = innermessageSupplier + "<td width='25%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + item.ProductDescrption;
                                    innermessageSupplier = innermessageSupplier + "</td>";

                                    innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + item.GLCode;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                }

                                innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "$" + (item.MOASPrice != null ? Convert.ToDecimal(item.MOASPrice) : Convert.ToDecimal(item.Price)).ToString();
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "$" + (item.MOASPrice != null ? Convert.ToDecimal(item.MOASPrice) : Convert.ToDecimal(item.Price)) * Convert.ToDecimal(item.Quantity);
                                innermessageSupplier = innermessageSupplier + "</td>";

                                innermessageSupplier = innermessageSupplier + "</tr>";
                                innermessageSupplier = innermessageSupplier + "</table>";
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "</tr>";
                            }

                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                            innermessageSupplier = innermessageSupplier + "<hr />";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";
                        }

                        messagebody.Replace("{innermesaageforsupplier}", innermessageSupplier);

                        //Update Nagmani 18-Jan-2012
                        messagebody.Replace("{ShippingCost}", "");
                        messagebody.Replace("{Saletax}", "");
                        // messagebody.Replace("{OrderTotal}", "");

                        //messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());
                        messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal((objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + (objOrder.MOASOrderAmount != null ? objOrder.MOASOrderAmount : objOrder.OrderAmount)).ToString());

                        //End Nagmani
                        messagebody.Replace(" {Order Notes:}", "Order Notes :");
                        messagebody.Replace("{ShippingCostView}", "");
                        messagebody.Replace("{SalesTaxView}", "");
                        //Update Nagmani 18-Jan-2012
                        //  messagebody.Replace("{OrderTotalView}", "");
                        messagebody.Replace("{OrderTotalView}", "Order Total :");
                        //End Nagmani 

                        //End
                        #endregion

                        String c = NameBars(objOrder.OrderID);
                        if (c != null)
                        {
                            messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + c);
                        }
                        else
                        {
                            messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                        }

                        //  messagebody.Replace("{PurchasesNotEligableforCreditUse}", lblNotEligibleforAnniversarycredit.Text);


                        messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                        String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                        Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                        //String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                        //String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                        new CommonMails().SendMail(objUserInfo.UserInfoID, "MOAS From My Pending Orders", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Sends the verification email MOAS manager for approve order.
    /// Add By Mayur on 6-feb-2012
    /// </summary>
    /// <param name="OrderNumber">The order number.</param>
    /// <param name="MOASEmailAddress">The MOAS email address.</param>
    /// <param name="FullName">The full name.</param>
    private void sendVerificationEmailMOASManager(Int64 OrderId, String MOASEmailAddress, String FullName, Int64 MOASUserId, Order objOrder)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Placed MOAS";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                //Order objOrder = objRepos.GetByOrderID(OrderId);
                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                CompanyEmployeeRepository objCERepo = new CompanyEmployeeRepository();
                CompanyEmployee objCompanyEmployee = objCERepo.GetByUserInfoId(objUserInformation.UserInfoID);

                Boolean IsMOASWithCostCenterCode = objCERepo.FUN_IsMOASWithCostCenterCode(Convert.ToInt64(objOrder.UserId));

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString().Replace("{FullName}", objUserInformation.FirstName + " " + objUserInformation.LastName).Replace("{OrderNumber}", objOrder.OrderNumber);
                String sToadd = MOASEmailAddress;

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());

                String PaymentMethod = String.Empty;
                if (objOrder.PaymentOption != null)
                    PaymentMethod = objLookupRepository.GetById((Int64)objOrder.PaymentOption).sLookupName;

                messagebody.Replace("{firstnote}", "Please review the following order below that requires your attention. You can Approve, Edit or Cancel this order by clicking the buttons on the bottom of this page.");
                messagebody.Replace("{FullName}", FullName);
                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName);
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());

                #region Check here for CreditUsed
                String cr;
                Boolean creditused = true;
                if (objOrder.CreditUsed != "0")
                {
                    if (String.IsNullOrEmpty(objOrder.CreditUsed))
                    {
                        creditused = false;
                    }
                    else
                    {
                        creditused = true;
                    }
                }
                else
                {
                    creditused = false;
                }
                #endregion

                #region PaymentType and PaymentMethod
                //Both Option have used
                if (objOrder.PaymentOption != null && creditused)
                {

                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits Paid by Corporate ";
                        /*Strating Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", objOrder.CreditAmt.ToString());
                        /*End*/
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits Paid by Corporate ";
                        /*Anniversary Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                        /*End*/

                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    //updated by Prashant 27th feb 2013
                    //MOAS L1 Price Change for CA User
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    //updated by Prashant 27th feb 2013
                    //MOAS L1 Price Change for CA User

                    /*End*/
                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                }
                //Only Starting or Anniversary
                else if (objOrder.PaymentOption == null && creditused)
                {
                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr);
                }
                //Only Payment Option
                else if (objOrder.PaymentOption != null && !creditused)
                {
                    messagebody.Replace("{PaymentType}", new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                }
                else
                {
                    messagebody.Replace("{PaymentType}", "Paid By Corporate");
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/

                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                }
                messagebody.Replace("{Payment Method :}", "Payment Method :");
                #endregion

                #region PaymentOptionCode
                //For displaying payment option code
                if (IsMOASWithCostCenterCode)
                {
                    messagebody.Replace("{PaymentOptionCodeLabel}", "Cost Center Code :");
                    messagebody.Replace("{PaymentOptionCodeValue}", objOrder.PaymentOptionCode);
                }
                else
                {
                    messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
                    messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);
                }
                #endregion

                #region Billing Address
                if (objOrder.OrderFor == "ShoppingCart") // Company Pays
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                else
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                messagebody.Replace("{Billing Address}", "Bill To:");
                messagebody.Replace("{NameView}", "Name :");
                messagebody.Replace("{CompanyNameView}", "Company Name :");
                messagebody.Replace("{AddressView}", "Address :");
                // messagebody.Replace("{Address2}", lblBadr.Text);
                messagebody.Replace("{CityView}", "City :");
                messagebody.Replace("{StateView}", "State :");
                messagebody.Replace("{ZipView}", "Zip :");
                messagebody.Replace("{CountryView}", "Country :");
                messagebody.Replace("{PhoneView}", "Phone :");
                messagebody.Replace("{EmailView}", "Email :");

                messagebody.Replace("{Name}", objBillingInfo.BillingCO + " " + objBillingInfo.Manager);
                messagebody.Replace("{CompanyName}", objBillingInfo.CompanyName);
                messagebody.Replace("{Address1}", objBillingInfo.Address + "<br/>" + objBillingInfo.Address2);
                // messagebody.Replace("{Address2}", lblBadr.Text);
                messagebody.Replace("{City}", objCity.GetById((Int64)objBillingInfo.CityID).sCityName);
                messagebody.Replace("{State}", objState.GetById((Int64)objBillingInfo.StateID).sStatename);
                messagebody.Replace("{Zip}", objBillingInfo.ZipCode);
                messagebody.Replace("{Country}", objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone}", objBillingInfo.Telephone);
                messagebody.Replace("{Email}", objBillingInfo.Email);
                #endregion

                #region shipping address
                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                }
                else
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                }
                messagebody.Replace("{Shipping Address}", "Ship To:");
                messagebody.Replace("{AirportView1}", "Airport :");
                messagebody.Replace("{DepartmentView1}", "Department :");
                messagebody.Replace("{NameView1}", "Name :");
                messagebody.Replace("{CompanyNameView1}", "Company Name :");
                messagebody.Replace("{AddressView1}", "Address :");
                // messagebody.Replace("{Address2}", lblBadr.Text);
                messagebody.Replace("{CityView1}", "City :");
                messagebody.Replace("{CountyView1}", "County :");
                messagebody.Replace("{StateView1}", "State :");
                messagebody.Replace("{ZipView1}", "Zip :");
                messagebody.Replace("{CountryView1}", "Country :");
                messagebody.Replace("{PhoneView1}", "Phone :");
                messagebody.Replace("{EmailView1}", "Email :");

                messagebody.Replace("{Airport1}", objShippingInfo.Airport);
                if (objShippingInfo.DepartmentID != null)
                    messagebody.Replace("{Department1}", new LookupRepository().GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
                else
                    messagebody.Replace("{Department1}", "");
                messagebody.Replace("{Name1}", objShippingInfo.Name + " " + objShippingInfo.Fax);
                messagebody.Replace("{CompanyName1}", objShippingInfo.CompanyName);
                messagebody.Replace("{Address11}", objShippingInfo.Address + "<br/>" + objShippingInfo.Address2 + "<br/>" + objShippingInfo.Street);
                messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
                messagebody.Replace("{County1}", objShippingInfo.county);
                messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
                messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
                messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
                messagebody.Replace("{Email1}", objShippingInfo.Email);
                #endregion

                #region order product detail
                String innermessage = "";
                Boolean GLCodeExists = false;

                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                    GLCodeExists = objSelectMyShoppingCartProductResultList.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                    {
                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objSelectMyShoppingCartProductResultList[i].NameToBeEngraved))
                        {
                            p = objSelectMyShoppingCartProductResultList[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Quantity;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Size;
                        innermessage = innermessage + "</td>";

                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].GLCode;
                            innermessage = innermessage + "</td>";
                        }

                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)).ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)) * Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].Quantity);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";

                        if (GLCodeExists)
                            innermessage = innermessage + "<td colspan='8'>";
                        else
                            innermessage = innermessage + "<td colspan='7'>";

                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }
                    messagebody.Replace("{innermessage}", innermessage);
                }
                else
                {
                    List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                    GLCodeExists = objFinal.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objFinal.Count; i++)
                    {
                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objFinal[i].NameToBeEngraved))
                        {
                            p = objFinal[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].Qty;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].sLookupName;
                        innermessage = innermessage + "</td>";

                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                        }

                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)).ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)) * Convert.ToDecimal(objFinal[i].Qty);


                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";

                        if (GLCodeExists)
                            innermessage = innermessage + "<td colspan='8'>";
                        else
                            innermessage = innermessage + "<td colspan='7'>";

                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }

                    messagebody.Replace("{innermessage}", innermessage);

                }

                if (GLCodeExists)
                    messagebody.Replace("<td id=\"tdPD\" width=\"48%\"", "<td id=\"tdPD\" width=\"38%\"");
                else
                    messagebody.Replace(@"<td id=""tdGC"" width=""10%"" style=""font-weight: bold; text-align: center;"">
                                                    GL Code
                                                </td>", "");

                #endregion

                messagebody.Replace("{Order Notes:}", "Order Notes :");
                messagebody.Replace("{ShippingCostView}", "Shipping :");
                messagebody.Replace("{SalesTaxView}", "Sales Tax :");
                messagebody.Replace("{OrderTotalView}", "Order Total :");
                messagebody.Replace("{innermesaageforsupplier}", "");
                String b = NameBars(objOrder.OrderID);
                if (b != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + b);
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                }

                messagebody.Replace("{ShippingCost}", objOrder.ShippingAmount.ToString());
                messagebody.Replace("{Saletax}", "$" + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)).ToString());
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + (objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount))).ToString());
                if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)//check that corporate discount is applied or not
                {
                    messagebody.Replace("{CorporateDiscountView}", "Corporate Discount");
                    messagebody.Replace("{CorporateDiscount}", objOrder.CorporateDiscount.ToString());
                }
                else
                {
                    messagebody.Replace("{CorporateDiscountView}", "");
                    messagebody.Replace("{CorporateDiscount}", "");
                }
                // messagebody.Replace("{PurchasesNotEligableforCreditUse}", lblNotEligibleforAnniversarycredit.Text);

                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                //Set Conformation Button
                String buttonText = "";
                buttonText += "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                buttonText += "<tr>";
                buttonText += "<td>";
                buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "MOASOrderStatus.aspx?UserId=" + MOASUserId + "&OrderId=" + objOrder.OrderID + "&Status=Approve'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/approve_order_btn.png' alt='Approve Order' border='0'/></a>";
                buttonText += "<td>";
                buttonText += "<td>";
                buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "MOASOrderStatus.aspx?UserId=" + MOASUserId + "&OrderId=" + objOrder.OrderID + "&Status=Cancel'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/cancel_order_btn.png' alt='Cancel Order' border='0'/></a>";
                buttonText += "<td>";
                buttonText += "<td>";
                buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "MyAccount/OrderManagement/EditOrderDetail.aspx?Id=" + objOrder.OrderID + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/edit_order_btn.png' alt='Edit Order' border='0'/></a>";
                buttonText += "<td>";
                buttonText += "</tr>";
                buttonText += "</table>";

                messagebody.Replace("{ConformationButton}", buttonText);

                String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                //String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                //String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                if (new ManageEmailRepository().CheckEmailAuthentication(MOASUserId, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.MOAS)) == true)
                {
                    new CommonMails().SendMail(MOASUserId, "MOAS From My Pending Orders", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                }
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private String NameBars(Int64 OrderId)
    {
        String strNameBars = String.Empty;
        Order objOrder = new OrderConfirmationRepository().GetByOrderID(OrderId);
        if (objOrder.OrderFor == "IssuanceCart")
        {
            List<MyIssuanceCart> objIssList = new MyIssuanceCartRepository().GetIssuanceCartByOrderID(objOrder.OrderID);
            foreach (MyIssuanceCart objItem in objIssList)
            {
                if (objItem.NameToBeEngraved != null && objItem.NameToBeEngraved != String.Empty)
                {
                    if (objItem.NameToBeEngraved.Contains(','))
                    {
                        String[] EmployeeTitle = objItem.NameToBeEngraved.Split(',');
                        strNameBars += "Name to be Engraved on Name Bar:" + EmployeeTitle[0] + "\n";
                        strNameBars += "Employee Title" + EmployeeTitle[1] + "\n";
                    }
                    else
                    {
                        strNameBars += "Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n";
                    }
                }
            }
        }
        else
        {
            List<SelectMyShoppingCartProductResult> objMyShpList = new MyShoppingCartRepository().SelectCurrentOrderProducts(objOrder.OrderID);
            foreach (SelectMyShoppingCartProductResult objItem in objMyShpList)
            {
                if (objItem.NameToBeEngraved != null && objItem.NameToBeEngraved != String.Empty)
                {
                    if (objItem.NameToBeEngraved.Contains(','))
                    {
                        String[] EmployeeTitle = objItem.NameToBeEngraved.Split(',');
                        strNameBars += "Name to be Engraved on Name Bar:" + EmployeeTitle[0] + "\n";
                        strNameBars += "Employee Title:" + EmployeeTitle[1] + "\n";
                    }
                    else
                    {
                        strNameBars += "Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n";
                    }
                }
            }
        }

        return strNameBars.ToString();

    }

    /// <summary>
    /// Add the note history of order.
    /// </summary>
    /// <param name="strStatus">The order status.</param>
    /// <param name="orderID">The order ID.</param>
    protected void AddNoteHistory(String strStatus, Int64 orderID)
    {
        String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.CACE);

        NoteDetail objComNot = new NoteDetail();
        NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();

        String strNoteContents = "";
        strNoteContents = "Action Taken: " + strStatus;

        if (strStatus == "Canceled")
        {
            strNoteContents += Environment.NewLine;
            strNoteContents += "Reason for Cancelling: " + txtCancelReason.Text;
        }

        objComNot.Notecontents = strNoteContents;
        objComNot.NoteFor = strNoteFor;
        objComNot.ForeignKey = orderID;
        objComNot.CreateDate = System.DateTime.Now;
        objComNot.CreatedBy = this.UserInfoID;
        objComNot.UpdateDate = System.DateTime.Now;
        objComNot.UpdatedBy = this.UserInfoID;

        objCompNoteHistRepos.Insert(objComNot);
        objCompNoteHistRepos.SubmitChanges();
    }

    #region Integration with Magaya

    private void SendDataToMagaya(Order objOrder)
    {
        api_session_error Result;
        List<MyIssuanceCart> objMyIssCart = new List<MyIssuanceCart>();
        String user = ConfigurationSettings.AppSettings["MagayaUser"];
        String pass = ConfigurationSettings.AppSettings["MagayaPassword"];
        Int32 AccKey;
        Int32 flag = 0x20;
        String Err_Desc;
        try
        {
            #region Check that supplier for WTDC only
            //Order objOrder1 = new Order();
            Int32 fispresnt = 0;
            if (objOrder.OrderFor == "ShoppingCart")
            {
                objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                {
                    if (objSelectMyShoppingCartProductResultList[i].SupplierID == this.WTDCSupplierID)
                    {
                        fispresnt = 1;
                        break;
                    }
                }
            }
            else
            {
                objMyIssCart = objMyIssCartRepo.GetIssuanceCartByOrderID(objOrder.OrderID);
                List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                for (Int32 i = 0; i < objFinal.Count; i++)
                {
                    if (objFinal[i].SupplierID == this.WTDCSupplierID)
                    {
                        fispresnt = 1;
                        break;
                    }
                }
            }
            #endregion

            #region If supplier is for WTDC then only this will process
            if (fispresnt == 1)
            {
                CSSoapService cSSoapService = new CSSoapService();
                String xmlTrans = beautify(createXmlDoc(objOrder.OrderID.ToString(), this.WTDCSupplierID).OuterXml);
                //API Functions
                Result = cSSoapService.StartSession(user, pass, out AccKey);
                //Result = cSSoapService.SetTransaction(AccKey, TransType, flag, xmlTrans, out Err_Desc);
                String szNumber = "";
                Result = cSSoapService.SubmitSalesOrder(AccKey, xmlTrans, flag, out szNumber, out Err_Desc);

                OrderConfirmationRepository objOrderConfirmationRepositoryMagaya = new OrderConfirmationRepository();
                Order ObjOrderMagaya = objOrderConfirmationRepositoryMagaya.GetByOrderID(objOrder.OrderID);
                if (Result == api_session_error.no_error)
                    ObjOrderMagaya.SendToMagaya = "Success";
                else
                    ObjOrderMagaya.SendToMagaya = "Fail";
                objOrderConfirmationRepositoryMagaya.SubmitChanges();

                Result = cSSoapService.EndSession(AccKey);
            }
            #endregion
        }
        catch (Exception ex)
        {
            if (ex.Message == "Unable to connect to the remote server")
            {
                OrderConfirmationRepository objOrderConfirmationRepositoryMagaya = new OrderConfirmationRepository();
                Order ObjOrderMagaya = objOrderConfirmationRepositoryMagaya.GetByOrderID(objOrder.OrderID);
                ObjOrderMagaya.SendToMagaya = "Fail";
                objOrderConfirmationRepositoryMagaya.SubmitChanges();
            }
        }
    }

    /// <summary>
    /// - Indent a XML 
    /// </summary>
    /// <param name="xml">Source XML String</param>
    /// <returns>Indented XML String</returns>
    public String beautify(String xml)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        MemoryStream MS = new MemoryStream();
        XmlTextWriter xmlWriter = new XmlTextWriter(MS, null);
        xmlWriter.Formatting = Formatting.Indented;
        xmlWriter.Indentation = 4;

        xmlDoc.Save(xmlWriter);

        return ByteArrayToStr(MS.ToArray());
    }

    /// <summary>
    ///  - Encoding  Byte's Array into String with UTF8 code
    /// </summary>
    /// <param name="bytes">Source Byte Array</param>
    /// <returns>Encoded String</returns>
    public String ByteArrayToStr(Byte[] bytes)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        return encoding.GetString(bytes);
    }

    /// <summary>
    /// - Create XML to send a Transaction
    /// </summary>
    /// <returns>DocXML with WareHouse</returns>
    public XmlDocument createXmlDoc(String OrderID, Int32 supplierid)
    {
        try
        {
            //Find UserName who had order purchased
            String strUserName = null;
            List<MyIssuanceCart> objMyIssCart = new List<MyIssuanceCart>();
            Order objOrder = new Order();
            objOrder = objRepos.GetByOrderID(Convert.ToInt64(OrderID));
            Int64 intUserId = Convert.ToInt64(objOrder.UserId);
            UserInformation objUsrInfo = objUsrInfoRepo.GetById(intUserId);
            if (objUsrInfo != null)
            {
                strUserName = objUsrInfo.FirstName + " " + objUsrInfo.MiddleName + " " + objUsrInfo.LastName;
            }
            //End

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\"?>" +
                             "<SalesOrder/> ");
            XmlElement root = xmlDoc.DocumentElement;
            XmlElement subElemt = null;
            XmlElement subsubElemt = null;
            XmlElement item;
            XmlElement elem = addChildNode(xmlDoc, root, "CreatedOn", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"), false);
            elem = addChildNode(xmlDoc, root, "CreatedByName", objUsrInfo.FirstName, false);
            elem = addChildNode(xmlDoc, root, "Number", objOrder.OrderNumber, false);

            //-----BuyerShippingAddress
            if (objOrder.OrderFor == "ShoppingCart")
            {
                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
            }
            else
            {
                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
            }
            elem = addChildNode(xmlDoc, root, "BuyerShippingAddress");
            //addChildNode(xmlDoc, elem, "ContactName", objShippingInfo.CompanyName.ToString(), false);
            subElemt = addChildNode(xmlDoc, elem, "Street", objShippingInfo.CompanyName + " " + objShippingInfo.Address + " " + objShippingInfo.Address2 + " " + objShippingInfo.Street, false);
            subElemt = addChildNode(xmlDoc, elem, "City", new CityRepository().GetById((Int64)objShippingInfo.CityID).sCityName, false);
            subElemt = addChildNode(xmlDoc, elem, "State", new StateRepository().GetById((Int64)objShippingInfo.StateID).sStatename, false);
            subElemt = addChildNode(xmlDoc, elem, "ZipCode", objShippingInfo.ZipCode, false);
            subElemt = addChildNode(xmlDoc, elem, "Country", new CountryRepository().GetById((Int64)objShippingInfo.CountryID).sCountryName, "Code", "US", false);
            //------

            //-------Seller Address
            elem = addChildNode(xmlDoc, root, "SellerName", "Incentex", false);
            elem = addChildNode(xmlDoc, root, "SellerAddress");
            addChildNode(xmlDoc, elem, "Street", "648 Ocean Road", false);
            subElemt = addChildNode(xmlDoc, elem, "City", "Vero Beach", false);
            subElemt = addChildNode(xmlDoc, elem, "State", "FL", false);
            subElemt = addChildNode(xmlDoc, elem, "ZipCode", "32963", false);
            subElemt = addChildNode(xmlDoc, elem, "Country", "UNITED STATES", "Code", "US", false);
            //------------

            elem = addChildNode(xmlDoc, root, "BuyerName", objShippingInfo.Name, false);

            //------BuyerBillingAddress
            //if (objOrder.OrderFor == "ShoppingCart") // Company Pays
            //{
            //    objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
            //}
            //else
            //{
            //    objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
            //}
            //elem = addChildNode(xmlDoc, root, "BuyerBillingAddress");
            //addChildNode(xmlDoc, elem, "Street", objBillingInfo.Address + " " + objBillingInfo.Address2, false);
            //subElemt = addChildNode(xmlDoc, elem, "City", new CityRepository().GetById((Int64)objBillingInfo.CityID).sCityName, false);
            //subElemt = addChildNode(xmlDoc, elem, "State", new StateRepository().GetById((Int64)objBillingInfo.StateID).sStatename, false);
            //subElemt = addChildNode(xmlDoc, elem, "ZipCode", objBillingInfo.ZipCode, false);
            //subElemt = addChildNode(xmlDoc, elem, "Country", new CountryRepository().GetById((Int64)objBillingInfo.CountryID).sCountryName, "Code", "US", false);
            //------

            elem = addChildNode(xmlDoc, root, "ModeOfTransportation", "<Description>Air</Description>" +
                                                       "<Method>Air</Method>",
                                                       "Code", "40", true);
            //----------Item

            if (objOrder.OrderFor == "ShoppingCart")
            {
                elem = addChildNode(xmlDoc, root, "Items");
                objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);
                for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                {
                    if (objSelectMyShoppingCartProductResultList[i].SupplierID == supplierid)
                    {
                        //Item node start
                        subElemt = addChildNode(xmlDoc, elem, "Item");
                        item = addChildNode(xmlDoc, subElemt, "PartNumber", objSelectMyShoppingCartProductResultList[i].item.Replace(" ", ""), false);
                        item = addChildNode(xmlDoc, subElemt, "Pieces", Convert.ToString(Convert.ToInt32(objSelectMyShoppingCartProductResultList[i].Quantity)), false);
                        item = addChildNode(xmlDoc, subElemt, "Description", objSelectMyShoppingCartProductResultList[i].ProductDescrption1, false);

                        //Item Defination Start
                        subsubElemt = addChildNode(xmlDoc, subElemt, "ItemDefinition");
                        item = addChildNode(xmlDoc, subsubElemt, "PartNumber", objSelectMyShoppingCartProductResultList[i].item.Replace(" ", ""), false);
                        item = addChildNode(xmlDoc, subsubElemt, "Description", objSelectMyShoppingCartProductResultList[i].ProductDescrption1, false);
                        subElemt.AppendChild(subsubElemt);

                        //----------Sales Charge Node Start
                        subsubElemt = addChildNode(xmlDoc, subElemt, "SalesCharge");
                        item = addChildNode(xmlDoc, subsubElemt, "Type", "Standard", false);
                        item = addChildNode(xmlDoc, subsubElemt, "ExchangeRate", "1.00", false);
                        item = addChildNode(xmlDoc, subsubElemt, "PriceInCurrency", objSelectMyShoppingCartProductResultList[i].UnitPrice, "Currency", "USD", false);
                        item = addChildNode(xmlDoc, subsubElemt, "Price", objSelectMyShoppingCartProductResultList[i].UnitPrice, "Currency", "USD", false);
                        item = addChildNode(xmlDoc, subsubElemt, "Quantity", Convert.ToString(Convert.ToInt32(objSelectMyShoppingCartProductResultList[i].Quantity)), false);
                        Double UnitPrice = Convert.ToDouble(objSelectMyShoppingCartProductResultList[i].UnitPrice);
                        Int32 qty = Convert.ToInt32(Convert.ToInt32(objSelectMyShoppingCartProductResultList[i].Quantity));
                        String Amt = Convert.ToString(UnitPrice * qty);
                        item = addChildNode(xmlDoc, subsubElemt, "AmountInCurrency", Amt, "Currency", "USD", false);
                        item = addChildNode(xmlDoc, subsubElemt, "Amount", Amt, "Currency", "USD", false);
                        item = addChildNode(xmlDoc, subsubElemt, "Currency", "<Name>United States Dollar</Name>" +
                                                        "<ExchangeRate>1.00</ExchangeRate>" + "<DecimalPlaces>2</DecimalPlaces>"
                                                        + "<IsHomeCurrency>true</IsHomeCurrency>",
                                                        "Code", "USD", true);
                        subElemt.AppendChild(subsubElemt);

                        item = addChildNode(xmlDoc, subElemt, "SalesOrderNumber", objOrder.OrderNumber, false);
                        elem.AppendChild(subElemt);
                    }
                }
            }
            else
            {
                elem = addChildNode(xmlDoc, root, "Items");
                List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                for (Int32 i = 0; i < objFinal.Count; i++)
                {
                    if (objFinal != null && objFinal[i] != null)
                    {
                        if (objFinal[i].SupplierID == supplierid)
                        {
                            //Item node start
                            subElemt = addChildNode(xmlDoc, elem, "Item");
                            item = addChildNode(xmlDoc, subElemt, "PartNumber", objFinal[i].item.Replace(" ", ""), false);
                            item = addChildNode(xmlDoc, subElemt, "Pieces", Convert.ToString(Convert.ToInt32(objFinal[i].Qty)), false);
                            item = addChildNode(xmlDoc, subElemt, "Description", objFinal[i].ProductDescrption, false);

                            //Item Defination Start
                            subsubElemt = addChildNode(xmlDoc, subElemt, "ItemDefinition");
                            item = addChildNode(xmlDoc, subsubElemt, "PartNumber", objFinal[i].item.Replace(" ", ""), false);
                            item = addChildNode(xmlDoc, subsubElemt, "Description", objFinal[i].ProductDescrption, false);
                            subElemt.AppendChild(subsubElemt);

                            //----------Sales Charge Node Start
                            subsubElemt = addChildNode(xmlDoc, subElemt, "SalesCharge");
                            item = addChildNode(xmlDoc, subsubElemt, "Type", "Standard", false);
                            item = addChildNode(xmlDoc, subsubElemt, "ExchangeRate", "1.00", false);
                            item = addChildNode(xmlDoc, subsubElemt, "PriceInCurrency", Convert.ToString(objFinal[i].Rate), "Currency", "USD", false);
                            item = addChildNode(xmlDoc, subsubElemt, "Price", Convert.ToString(objFinal[i].Rate), "Currency", "USD", false);
                            item = addChildNode(xmlDoc, subsubElemt, "Quantity", Convert.ToString(Convert.ToInt32(objFinal[i].Qty)), false);
                            Double UnitPrice = Convert.ToDouble(objFinal[i].Rate);
                            Int32 qty = Convert.ToInt32(Convert.ToInt32(objFinal[i].Qty));
                            String Amt = Convert.ToString(UnitPrice * qty);
                            item = addChildNode(xmlDoc, subsubElemt, "AmountInCurrency", Amt, "Currency", "USD", false);
                            item = addChildNode(xmlDoc, subsubElemt, "Amount", Amt, "Currency", "USD", false);
                            item = addChildNode(xmlDoc, subsubElemt, "Currency", "<Name>United States Dollar</Name>" +
                                                            "<ExchangeRate>1.00</ExchangeRate>" + "<DecimalPlaces>2</DecimalPlaces>"
                                                            + "<IsHomeCurrency>true</IsHomeCurrency>",
                                                            "Code", "USD", true);
                            subElemt.AppendChild(subsubElemt);

                            item = addChildNode(xmlDoc, subElemt, "SalesOrderNumber", objOrder.OrderNumber, false);
                            elem.AppendChild(subElemt);
                        }
                    }
                }
            }

            root.AppendChild(elem);
            String a = NameBars(objOrder.OrderID);
            if (a != null)
            {
                elem = addChildNode(xmlDoc, root, "Notes", objOrder.SpecialOrderInstruction.Replace(Environment.NewLine, "<br />") + a, false);
            }
            else
            {
                elem = addChildNode(xmlDoc, root, "Notes", objOrder.SpecialOrderInstruction.Replace(Environment.NewLine, "<br />"), false);
            }

            root.AppendChild(elem);

            root.SetAttribute("xmlns", "http://www.magaya.com/XMLSchema/V1");

            return xmlDoc;
        }
        catch (Exception)
        {

            throw;
        }
    }

    #region Add Child Node

    /// <summary>
    /// - Add empty Child node To XMLDocument into XmlNode   
    /// </summary>
    /// <param name="xmlDoc">XML Doucument</param>
    /// <param name="parentNode">Parent Node </param>
    /// <param name="tag">Node Tag</param>
    /// <returns>XmlElement</returns>
    public XmlElement addChildNode(XmlDocument xmlDoc, XmlNode parentNode, String tag)
    {

        XmlElement elem = xmlDoc.CreateElement(tag);
        parentNode.AppendChild(elem);
        return elem;

    }

    /// <summary>
    /// - Add Child node To XMLDocument into XmlNode with Value and Atribute
    /// </summary>
    /// <param name="xmlDoc">XML Doucumen</param>
    /// <param name="parentNode">Parent Node</param>
    /// <param name="tag">Node Tag</param>
    /// <param name="value">Node Value</param>
    /// <param name="attrTag">Attribute Tag</param>
    /// <param name="attrValue">Attribute Value</param>
    /// <param name="isXML">Body Kind (XML or Text)</param>
    /// <returns>XmlElement</returns>
    public XmlElement addChildNode(XmlDocument xmlDoc, XmlNode parentNode, String tag, String value, String attrTag, String attrValue, Boolean isXML)
    {
        XmlElement elem = addChildNode(xmlDoc, parentNode, tag, value, isXML);
        if ((elem != null) && (attrTag.Trim() != ""))
            elem.SetAttribute(attrTag, attrValue);
        return elem;

    }

    /// <summary>
    /// - Add Child node To XMLDocument into XmlNode with Value
    /// </summary>
    /// <param name="xmlDoc">XML Doucumen</param>
    /// <param name="parentNode">Parent Node</param>
    /// <param name="tag">Node Tag</param>
    /// <param name="value">Node Value</param>
    /// <param name="isXML">Body Kind (XML or Text)</param>
    /// <returns>XmlElement</returns>
    public XmlElement addChildNode(XmlDocument xmlDoc, XmlNode parentNode, String tag, String value, Boolean isXML)
    {
        if (value != null)
        {
            XmlElement elem = addChildNode(xmlDoc, parentNode, tag);
            if (isXML)
                elem.InnerXml = value;
            else
                elem.InnerText = value;
            return elem;
        } return null;
    }

    #endregion

    #endregion


    /// <summary>
    /// Added by Prashant April 2013,
    /// To approve order by the admin (Common for both Gridview Approve and the button outside GridView
    /// </summary>
    /// <param name="objOrder"></param>
    /// <param name="objUser"></param>
    protected void ApproveOrder(Order objOrder, UserInformation objUser)
    {

        OrderMOASSystem objOrderMOASSystem = objOrderMOASSystemRepository.GetByOrderIDAndManagerUserInfoID(objOrder.OrderID, this.UserInfoID);
        objOrderMOASSystem.Status = "Open";
        objOrderMOASSystem.DateAffected = DateTime.Now;
        objOrderMOASSystemRepository.SubmitChanges();


        //updated on 4th dec by prashant
        //updated on prashant april 2013
        PreferenceRepository objPreferenceRepository = new PreferenceRepository();
        String ApproverLevel = objPreferenceRepository.GetPreferenceValuesByPreferenceValueID(objOrder.MOASApproverLevelID);
        if (!String.IsNullOrEmpty(ApproverLevel) && ApproverLevel.ToLower() == "companylevel")
        {
            List<OrderMOASSystem> objOrderMOASSystemList = objOrderMOASSystemRepository.GetByOrderIDAndStatus(objOrder.OrderID, "Order Pending");

            if (objOrderMOASSystemList != null && objOrderMOASSystemList.Count > 0)
            {
                AddNoteHistory("Approved", objOrder.OrderID);

                UserInformation objUserInformation = new UserInformation();
                objUserInformation = objUsrInfoRepo.GetById(objOrderMOASSystemList[0].ManagerUserInfoID);
                sendVerificationEmailMOASManager(objOrder.OrderID, objUserInformation.LoginEmail, objUserInformation.FirstName, objUserInformation.UserInfoID, objOrder);
            }
            else
            {
                UpdateOrderAndSendConfirmation(objOrder, objUser);
            }
        }
        else
        {
            //Update the Received status for the order (If the MOAS Approver level is set at Station Level
            //Hence, Only one Approver needs to approve the order.    
            UpdateOrderAndSendConfirmation(objOrder, objUser);
        }
    }

    /// <summary>
    /// Added By Prashant April 2013,
    /// To Update the Order and Send the Order Approval and Received Notification to IE,Admin, Supplier
    /// </summary>
    /// <param name="objOrder"></param>
    /// <param name="objUser"></param>
    protected void UpdateOrderAndSendConfirmation(Order objOrder, UserInformation objUser)
    {

        //update price level if different from the global selection and send confirmation mail to the IE
        Int32 MOASDefaultPriceLevel = 0;

        GlobalMenuSettingRepository objGlobalMenuSettingsRepo = new GlobalMenuSettingRepository();
        var objDefaultPriceLevel = objGlobalMenuSettingsRepo.GetById(objOrder.WorkgroupId, Convert.ToInt64(objOrder.StoreID));
        if (objDefaultPriceLevel != null)
            MOASDefaultPriceLevel = Convert.ToInt32(objDefaultPriceLevel.MOASPaymentPricing);
        Decimal TotalOrderAmount = 0;


        //Update the Price Rate if Default Price Level for the workgroup is set

        if (objOrder.PaymentOption == (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS)
        {
            OrderDetailHistoryRepository objOrderDetailsRepo = new OrderDetailHistoryRepository();
            ProductItemPriceRepository objProductItemPriceRepo = new ProductItemPriceRepository();
            if (objOrder.OrderFor == Convert.ToString(Incentex.DAL.Common.DAEnums.OrderFor.ShoppingCart))
            {
                List<MyShoppinCart> objShoppingCart = objOrderDetailsRepo.GetShoppinCartDetailsByOrderID(objOrder.OrderID);
                foreach (var item in objShoppingCart)
                {
                    //Update the Price Rate if Default Price Level for the workgroup is different from the current pricelevel
                    if ((MOASDefaultPriceLevel != 0 && item.PriceLevel != MOASDefaultPriceLevel) || MOASDefaultPriceLevel == 0)
                    {

                        Incentex.DAL.SqlRepository.ProductItemPriceRepository.ProductItemPricingResult objProductPricing = objProductItemPriceRepo.GetSingleProductItemPrice(item.ItemNumber, (Int32)item.StoreProductID);

                        //db.MyShoppinCarts.Attach(item);

                        Decimal ItemPrice = objProductPricing.Level1;
                        if (MOASDefaultPriceLevel == 1)
                            ItemPrice = objProductPricing.Level1;
                        else if (MOASDefaultPriceLevel == 2)
                            ItemPrice = objProductPricing.Level2;
                        else if (MOASDefaultPriceLevel == 3)
                            ItemPrice = objProductPricing.Level3;
                        else if (MOASDefaultPriceLevel == 4)
                            ItemPrice = objProductPricing.Level4;

                        item.MOASPriceLevel = MOASDefaultPriceLevel == 0 ? 1 : MOASDefaultPriceLevel;
                        item.MOASUnitPrice = Convert.ToString(ItemPrice);
                        TotalOrderAmount += Decimal.Round((ItemPrice * Convert.ToInt32(item.Quantity)), 2);
                    }
                }
                objOrderDetailsRepo.SubmitChanges();
            }
            else if (objOrder.OrderFor == Convert.ToString(Incentex.DAL.Common.DAEnums.OrderFor.IssuanceCart))
            {
                List<MyIssuanceCart> objIssuanceCart = objOrderDetailsRepo.GetIssuanceCartDetailsByOrderID(objOrder.OrderID);
                foreach (var item in objIssuanceCart)
                {
                    if ((MOASDefaultPriceLevel != 0 && item.PriceLevel != MOASDefaultPriceLevel) || MOASDefaultPriceLevel == 0)
                    {
                        Incentex.DAL.SqlRepository.ProductItemPriceRepository.ProductItemPricingResult objProductPricing = objProductItemPriceRepo.GetSingleProductItemPrice(item.ItemNumber, (Int32)item.StoreProductID);

                        //db.MyIssuanceCarts.Attach(item);

                        Decimal ItemPrice = objProductPricing.Level1;
                        if (MOASDefaultPriceLevel == 1)
                            ItemPrice = objProductPricing.Level1;
                        else if (MOASDefaultPriceLevel == 2)
                            ItemPrice = objProductPricing.Level2;
                        else if (MOASDefaultPriceLevel == 3)
                            ItemPrice = objProductPricing.Level3;
                        else if (MOASDefaultPriceLevel == 4)
                            ItemPrice = objProductPricing.Level4;

                        item.MOASPriceLevel = MOASDefaultPriceLevel == 0 ? 1 : MOASDefaultPriceLevel;
                        item.MOASRate = ItemPrice;
                        TotalOrderAmount += Decimal.Round((ItemPrice * Convert.ToInt32(item.Qty)), 2);
                    }
                }
                objOrderDetailsRepo.SubmitChanges();

            }
        }

        if (TotalOrderAmount != 0)
        {
            objOrder.MOASOrderAmount = TotalOrderAmount;
            objOrder.MOASSalesTax = Decimal.Round((TotalOrderAmount + Convert.ToDecimal(objOrder.ShippingAmount)) * Convert.ToDecimal(objOrder.StrikeIronTaxRate), 2);
        }

        objOrder.OrderStatus = "Open";
        objOrder.UpdatedDate = DateTime.Now;
        objRepos.SubmitChanges();

        AddNoteHistory("Approved", objOrder.OrderID);

        sendVerificationEmail("Approved", objOrder.OrderID, objUser.LoginEmail, objUser.FirstName, objUser.UserInfoID, objOrder);//send email to CE


        if (!objOrder.ReferenceName.ToLower().Contains("test") && !IncentexGlobal.IsIEFromStoreTestMode)
        {
            //Billing
            if (objOrder.OrderFor == "ShoppingCart") // Company Pays
            {
                objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
            }
            else
            {
                objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
            }
            //Shipping
            if (objOrder.OrderFor == "ShoppingCart")
            {
                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
            }
            else
            {
                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
            }

            sendIEEmail("Approved", objOrder.OrderID);//send email to IE

            sendEmailToSupplier(objOrder.OrderID, objBillingInfo, objShippingInfo);//send order to supplier                   

            new SAPOperations().SubmitOrderToSAP(objOrder.OrderID);

            //add by Mayur on 29-April-2012 for Magaya Integration
            //SendDataToMagaya(objOrder);
        }
        else
        {
            sendTestOrderEmailNotification("Approved", objOrder.OrderID);
        }
    }

    #endregion

    #region Paging

    protected void dtlPaging_ItemCommand(Object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindPendingOrdersGrid();
        }
    }
    protected void dtlPaging_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    private void doPaging()
    {
        try
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            Int32 CurrentPg = pds.CurrentPageIndex + 1;

            if (CurrentPg > ToPg)
            {
                FrmPg = ToPg + 1;
                ToPg = ToPg + PagerSize;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - PagerSize;

            }

            if (pds.PageCount < ToPg)
                ToPg = pds.PageCount;

            for (Int32 i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();

        }
        catch (Exception)
        { }
    }
    protected void lnkbtnPrevious_Click(Object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindPendingOrdersGrid();
    }
    protected void lnkbtnNext_Click(Object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindPendingOrdersGrid();
    }

    #endregion
}