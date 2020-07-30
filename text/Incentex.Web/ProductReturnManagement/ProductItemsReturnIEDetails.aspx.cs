using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Collections.Generic;
using Incentex.BE;
using Incentex.DA;
using System.Text;
using commonlib.Common;
using com.strikeiron.ws;

public partial class ProductReturnManagement_ProductItemsReturnIEDetails : PageBase
{
    #region Data Members
    Int64 OrderID
    {
        get
        {
            if (ViewState["OrderId"] == null)
            {
                ViewState["OrderId"] = 0;
            }
            return Convert.ToInt64(ViewState["OrderId"]);
        }
        set
        {
            ViewState["OrderId"] = value;
        }
    }
    String RANumber
    {
        get
        {
            if (ViewState["RANumber"] == null)
            {
                ViewState["RANumber"] = null;
            }
            return ViewState["RANumber"].ToString();
        }
        set
        {
            ViewState["RANumber"] = value;
        }
    }
    String FirstName
    {
        get
        {
            if (ViewState["FirstName"] == null)
            {
                ViewState["FirstName"] = null;
            }
            return ViewState["FirstName"].ToString();
        }
        set
        {
            ViewState["FirstName"] = value;
        }
    }
    String LastName
    {
        get
        {
            if (ViewState["LastName"] == null)
            {
                ViewState["LastName"] = null;
            }
            return ViewState["LastName"].ToString();
        }
        set
        {
            ViewState["LastName"] = value;
        }
    }


    ProductReturnRepository objPrdReturnRepos = new ProductReturnRepository();
    IncentexEmpProductReturnReceivedRepository objIEProductReturnReceivedRepos = new IncentexEmpProductReturnReceivedRepository();
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
    EmailTemplateBE objEmailBE = new EmailTemplateBE();
    EmailTemplateDA objEmailDA = new EmailTemplateDA();
    DataSet dsEmailTemplate;
    OrderDetailHistoryRepository ObjOrdDetailHisRepo = new OrderDetailHistoryRepository();
    #endregion

    #region Page Event's
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Process Incoming Returns";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            if (Request.QueryString["OrderId"] != null || Request.QueryString["Id"] != null)
            {
                if (Request.QueryString["OrderId"] != null)
                {
                    this.OrderID = Convert.ToInt64(Request.QueryString["OrderId"]);
                    menucontrol.Visible = false;
                }
                else
                {
                    this.OrderID = Convert.ToInt64(Request.QueryString["Id"]);
                    menucontrol.PopulateMenu(4, 0, this.OrderID, 0, false);
                }

                if (Request.QueryString["RA"] != null)
                    this.RANumber = Request.QueryString["RA"];

                SetPageProperties();
                BindGridview();
                DisplayNotes();
                DisplayNotesIE();
                //BindAddress();
                //BindOrderStatus();                
                //SetBillingShipping();
            }
            if (Request.QueryString["OrderId"] == null)
            {
                lnkSaveOrderDetails.Visible = false;
                //btnEditShipping.Visible = false;
                //btnEditBilling.Visible = false;
                divAddNots.Visible = false;
                pnlNotesIE.Visible = false;
                pnlNotes.Visible = false;
                div1.Visible = false;
                div1.Visible = false;
                txtOrderNotesForCECA.Visible = false;
                dvInternalNotes.Visible = false;
                dvNotesHistory.Visible = false;
            }
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Return View";
            if (Session["ToFroURL"] != null)// To go back 
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = Session["ToFroURL"].ToString();
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/ProductReturnManagement/ReturnProductIEList.aspx?RANumber=" + RANumber + "&FirstName=" + FirstName + "&LastName=" + LastName;
        }
    }
    #endregion

    #region Button Events
    //protected void btnEditBilling_Click(object sender, EventArgs e)
    //{
    //    SetBillingAddress();
    //    modalEditBilling.Show();
    //}

    //protected void btnEditShipping_Click(object sender, EventArgs e)
    //{
    //    SetShippingAddress();
    //    modalEditBilling.Show();
    //}

    protected void lnkButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["OrderID"] != null)
            {
                OrderID = Convert.ToInt64(Request.QueryString["OrderID"].ToString());
            }
            else
            {
                OrderID = 0;
            }
            string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.OrderProductReturns);

            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();


            objComNot.Notecontents = txtNote.Text;
            objComNot.NoteFor = strNoteFor;
            objComNot.ForeignKey = OrderID;
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID; // 10; //Session["CurrentUser"].ToString();
            objComNot.UpdateDate = System.DateTime.Now;
            objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;// 10;

            if (!(string.IsNullOrEmpty(txtNote.Text)))
            {

                objCompNoteHistRepos.Insert(objComNot);
                objCompNoteHistRepos.SubmitChanges();
                int NoteId = (int)(objComNot.NoteID);
                sendVerificationMailForNotesToCE(Convert.ToInt64(Request.QueryString["OrderId"].ToString()));
                txtNote.Text = "";
            }
            else
            {

            }

            DisplayNotes();

        }

        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

        }
    }

    protected void lnkNoteHisForIE_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["OrderID"] != null)
            {
                OrderID = Convert.ToInt64(Request.QueryString["OrderID"].ToString());
            }
            else
            {
                OrderID = 0;
            }
            string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.OrderProductReturns);

            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();


            objComNot.Notecontents = txtNoteIE.Text;
            objComNot.NoteFor = strNoteFor;
            objComNot.SpecificNoteFor = "IEInternalNotes";
            objComNot.ForeignKey = OrderID;
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID; // 10; //Session["CurrentUser"].ToString();
            objComNot.UpdateDate = System.DateTime.Now;
            objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;// 10;

            if (!(string.IsNullOrEmpty(txtNoteIE.Text)))
            {

                objCompNoteHistRepos.Insert(objComNot);
                objCompNoteHistRepos.SubmitChanges();
                int NoteId = (int)(objComNot.NoteID);

                txtNoteIE.Text = "";
            }
            else
            {

            }

            DisplayNotesIE();

        }

        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

        }
    }

    protected void lnkSaveOrderDetails_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        decimal TotalCreditAmt = 0;
        decimal ItemQty = 0;
        List<GetReturnOrderDetailsResult> objList = new List<GetReturnOrderDetailsResult>();
        objList = objPrdReturnRepos.GetReturnOrderDetails(this.OrderID);
        Order ordDetails = new OrderConfirmationRepository().GetByOrderID(Convert.ToInt64(this.OrderID));
        OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
        StringBuilder objHtml = new StringBuilder();
        StringBuilder objHtmlRepackingFee = new StringBuilder();
        foreach (GridViewRow row in gvProductReturn.Rows)
        {
            TextBox txtReceivedQty = (TextBox)row.FindControl("txtReceivedQty");
            Label lblReturnQty = (Label)row.FindControl("lblReturnQty");
            HiddenField hdnitemNo = (HiddenField)row.FindControl("hdnitemNo");
            DropDownList ddlReturnStatus = (DropDownList)row.FindControl("ddlReturnStatus");
            HiddenField hdnProductReturnId = (HiddenField)row.FindControl("hdnProductReturnId");
            HiddenField hdnShoppingCartId = (HiddenField)row.FindControl("hdnShoppingCartId");
            Label lblProductionDescription = (Label)row.FindControl("lblProductionDescription");
            Label lblReason = (Label)row.FindControl("lblReason");

            if (txtReceivedQty.Text != "")
            {
                /*Start : Update a ReturnProduct table and insert row in IncentexEmpProductReturnReceived table */
                ReturnProduct ObjReturnProduct = objPrdReturnRepos.GetById(Convert.ToInt64(hdnProductReturnId.Value));
                ObjReturnProduct.Status = "Completed";
                ObjReturnProduct.ReturnStatusId = Convert.ToInt64(ddlReturnStatus.SelectedValue);
                if (objIEProductReturnReceivedRepos.GetByProductReturnID(Convert.ToInt64(hdnProductReturnId.Value)).Count == 0)
                {
                    ObjReturnProduct.ProcessedDate = DateTime.Now;
                    objPrdReturnRepos.SubmitChanges();

                    IncentexEmpProductReturnReceived objIncentexEmpProductReturnReceived = new IncentexEmpProductReturnReceived();
                    objIncentexEmpProductReturnReceived.ProductReturnId = ObjReturnProduct.ProductReturnId;
                    objIncentexEmpProductReturnReceived.ReceivedQty = Convert.ToInt64(txtReceivedQty.Text);
                    ItemQty = Convert.ToDecimal(txtReceivedQty.Text);
                    objIncentexEmpProductReturnReceived.ReceivedDate = DateTime.Now;
                    objIncentexEmpProductReturnReceived.OrderID = OrderID;
                    objIncentexEmpProductReturnReceived.NoOfReceived = 1;
                    objIEProductReturnReceivedRepos.Insert(objIncentexEmpProductReturnReceived);
                    objIEProductReturnReceivedRepos.SubmitChanges();
                }
                else
                {
                    objPrdReturnRepos.SubmitChanges();
                    IncentexEmpProductReturnReceived objIncentexEmpProductReturnReceived = objIEProductReturnReceivedRepos.GetByProductReturnID(Convert.ToInt64(hdnProductReturnId.Value)).Single();
                    ItemQty = Convert.ToInt64(txtReceivedQty.Text) - Convert.ToInt64(objIEProductReturnReceivedRepos.GetByProductReturnID(Convert.ToInt64(hdnProductReturnId.Value))[0].ReceivedQty);
                    objIncentexEmpProductReturnReceived.ReceivedQty = Convert.ToInt64(txtReceivedQty.Text);
                    objIEProductReturnReceivedRepos.SubmitChanges();
                }
                /*End : Update a ReturnProduct table and insert row in IncentexEmpProductReturnReceived table */

                //Create MessageBody using String Bulder for a Sending a Mail to CA/CE.
                objHtml.Append("<tr>");
                objHtml.Append("<td valign='top' text-align: center;>" + lblReturnQty.Text + "</td><td valign='top'>" + hdnitemNo.Value + "</td><td valign='top'>" + lblProductionDescription.Text + "</td><td valign='top'>" + txtReceivedQty.Text + "</td><td valign='top'>" + lblReason.Text + "</td><td valign='top'>" + ddlReturnStatus.SelectedItem.Text.ToString() + "</td>");
                objHtml.Append("</tr>");

                //Create MessageBody for Repacking fee using String Bulder for a Sending a Mail to IE (Accounts).
                if (ddlReturnStatus.SelectedItem.ToString() == Incentex.DAL.Common.ProductReturnStatusConsts.RetAccRepackFee.ToString())
                {
                    objHtmlRepackingFee.Append("<tr>");
                    objHtmlRepackingFee.Append("<td valign='top'>" + hdnitemNo.Value + "</td><td valign='top'>" + lblProductionDescription.Text + "</td><td valign='top' align='center' >" + txtReceivedQty.Text + "</td><td valign='top'>" + ddlReturnStatus.SelectedItem.Text.ToString() + "</td>");
                    objHtmlRepackingFee.Append("</tr>");
                }

                if (ddlReturnStatus.SelectedItem.ToString() == Incentex.DAL.Common.ProductReturnStatusConsts.RetAcc.ToString() || ddlReturnStatus.SelectedItem.ToString() == Incentex.DAL.Common.ProductReturnStatusConsts.RetAccManDef.ToString() || ddlReturnStatus.SelectedItem.ToString() == Incentex.DAL.Common.ProductReturnStatusConsts.RetAccRepackFee.ToString())
                {

                    /*If order gets returned then give back the credit amount he has used*/
                    ///Add a inventory for a product item
                    if (ordDetails.OrderFor == "ShoppingCart")
                    {
                        //Shopping cart
                        MyShoppingCartRepository objShoppingCartRepos = new MyShoppingCartRepository();
                        MyShoppinCart objShoppingcart = new MyShoppinCart();
                        ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                        ProductItem objProductItem = new ProductItem();
                        objShoppingcart = objShoppingCartRepos.GetById(Convert.ToInt32(hdnShoppingCartId.Value), (Int64)ordDetails.UserId);

                        TotalCreditAmt += Convert.ToDecimal(objShoppingcart.UnitPrice) * Convert.ToDecimal(ItemQty);
                        objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(objShoppingcart.StoreProductID), Convert.ToInt64(objShoppingcart.MasterItemNo), objShoppingcart.ItemNumber);
                        if (ItemQty != 0)
                        {
                            //Update Inventory Here , Call here upDate Procedure 
                            String strProcess = "Shopping";
                            String strMessage = objRepos.IncreaseDescreaseInventory(objProductItem.ProductItemID, -Convert.ToInt32(ItemQty), strProcess);
                        }
                    }
                    else
                    {
                        //Issuance
                        MyIssuanceCartRepository objIssuanceRepos = new MyIssuanceCartRepository();
                        MyIssuanceCart objIssuance = new MyIssuanceCart();
                        ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                        //End 

                        objIssuance = objIssuanceRepos.GetById(Convert.ToInt32(hdnShoppingCartId.Value), (Int64)ordDetails.UserId);
                        List<SelectProductIDResult> objList1 = new List<SelectProductIDResult>();
                        CompanyEmployeeRepository objcmpemprepo = new CompanyEmployeeRepository();
                        AnniversaryProgramRepository objacprepo = new AnniversaryProgramRepository();
                        Int64 storeid = objacprepo.GetEmpStoreId((Int64)ordDetails.UserId, objcmpemprepo.GetByUserInfoId((Int64)ordDetails.UserId).WorkgroupID).StoreID;
                        objList1 = objProItemRepos.GetProductId(objIssuance.MasterItemID, Convert.ToInt64(hdnShoppingCartId.Value), Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(storeid));
                        //Update Inventory Here Call here upDate Procedure
                        for (Int32 i = 0; i < objList1.Count; i++)
                        {
                            String strProcess = "UniformIssuance";
                            String strMessage = objRepos.IncreaseDescreaseInventory(Convert.ToInt64(objList1[i].ProductItemID), -Convert.ToInt32(objIssuance.Qty), strProcess);
                            //String strMessage = objRepos.IncreaseDescreaseInventory(Convert.ToInt64(objList1[i].ProductItemID), -Convert.ToInt32(objIssuance.Qty), strProcess);
                        }
                    }
                }
            }
        }
        if (TotalCreditAmt > 0)
        {
            string ZipCode = objCmpEmpContRep.GetShippingDetailAddress(Convert.ToInt64(ordDetails.UserId), OrderID, "Shipping", ordDetails.OrderFor).ZipCode.ToString();
            Double SalesTax = GetSalesTax(ZipCode);
            TotalCreditAmt = TotalCreditAmt + (TotalCreditAmt * Convert.ToDecimal(SalesTax));
        }
        /*Start : Update CreditAmount in CompanyEmployee table and insert note in EmployeeLedger table*/
        if (ordDetails.CreditUsed != null && TotalCreditAmt != 0)
        {
            CompanyEmployeeRepository objCmnyEmp = new CompanyEmployeeRepository();
            CompanyEmployee cmpEmpl = objCmnyEmp.GetByUserInfoId((Int64)ordDetails.UserId);

            if (ordDetails.CreditUsed == "Previous")
            {
                cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + TotalCreditAmt;
                cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + TotalCreditAmt;
                cmpEmpl.CreditAmtToExpired = cmpEmpl.CreditAmtToExpired + TotalCreditAmt;
                objCmnyEmp.SubmitChanges();
            }
            else if (ordDetails.CreditUsed == "Anniversary")
            {
                cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + TotalCreditAmt;
                cmpEmpl.CreditAmtToExpired = cmpEmpl.CreditAmtToExpired + TotalCreditAmt;
                objCmnyEmp.SubmitChanges();
            }
            #region EmployeeLedger
            EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
            EmployeeLedger objEmplLedger = new EmployeeLedger();
            EmployeeLedger transaction = new EmployeeLedger();

            objEmplLedger.UserInfoId = cmpEmpl.UserInfoID;
            objEmplLedger.CompanyEmployeeId = cmpEmpl.CompanyEmployeeID;
            objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ORRTN.ToString(); ;
            objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.OrderReturn.ToString();

            if (TotalCreditAmt < 0)
            {
                objEmplLedger.TransactionAmount = -TotalCreditAmt;
                objEmplLedger.AmountCreditDebit = "Debit";
            }
            else
            {
                objEmplLedger.AmountCreditDebit = "Credit";
                objEmplLedger.TransactionAmount = TotalCreditAmt;
            }
            objEmplLedger.OrderNumber = ordDetails.OrderNumber;
            objEmplLedger.OrderId = ordDetails.OrderID;
            transaction = objLedger.GetLastTransactionByEmplID(cmpEmpl.CompanyEmployeeID);
            if (transaction != null)
            {
                objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                objEmplLedger.CurrentBalance = transaction.CurrentBalance + objEmplLedger.TransactionAmount;
            }
            objEmplLedger.TransactionDate = System.DateTime.Now;
            objEmplLedger.UpdateById = IncentexGlobal.CurrentMember.UserInfoID;
            objLedger.Insert(objEmplLedger);
            objLedger.SubmitChanges();
            #endregion
        }
        /*End : Update CreditAmount in CompanyEmployee table and insert note in EmployeeLedger table*/

        //objPrdReturnRepos.UpdateStatus(OrderID, "Processed");
        //Send Email After Order Status Changed
        sendVerificationEmail("Processed", OrderID, objHtml.ToString());
        if (objHtmlRepackingFee.Length > 0)
            sendIEEmail("", "", objHtmlRepackingFee.ToString(), ordDetails.OrderNumber);
        lblmsg.Text = "Record Saved Successfully.";
        BindGridview();
    }
    #endregion

    #region Grid Events
    protected void gvProductReturn_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            List<ShippedReturnProductDescriptionResult> objNew = new List<ShippedReturnProductDescriptionResult>();
            HiddenField hdnOrderID = (HiddenField)e.Row.FindControl("hdnOrderID");
            HiddenField hdnitemNo = (HiddenField)e.Row.FindControl("hdnitemNo");
            HiddenField hdnShoppingCartId = (HiddenField)e.Row.FindControl("hdnShoppingCartId");
            Label lblProductDescription = (Label)e.Row.FindControl("lblProductionDescription");
            DropDownList ddlReturnStatus = (DropDownList)e.Row.FindControl("ddlReturnStatus");
            HiddenField hdnProductReturnId = (HiddenField)e.Row.FindControl("hdnProductReturnId");
            TextBox txtReceivedQty = (TextBox)e.Row.FindControl("txtReceivedQty");
            objNew = OrdShippOrder.GetDesCription(Convert.ToInt32(this.OrderID), Convert.ToInt32(hdnShoppingCartId.Value), hdnitemNo.Value);
            if (objNew.Count > 0)
                lblProductDescription.Text = objNew[0].ProductDescrption;


            BindReturnStatus(ddlReturnStatus);
            List<GetReturnOrderDetailsResult> objList = (List<GetReturnOrderDetailsResult>)gvProductReturn.DataSource;
            var data = objList.Where(m => m.ProductReturnId == Convert.ToInt64(hdnProductReturnId.Value)).Single();
            ddlReturnStatus.SelectedValue = data.ReturnStatusId.ToString();
            txtReceivedQty.Text = data.ReceivedQty.ToString();
            if (Request.QueryString["OrderId"] == null)
            {
                txtReceivedQty.Enabled = false;
                ddlReturnStatus.Enabled = false;
            }
        }
    }
    #endregion

    #region General Methods
    private void sendVerificationMailForNotesToCE(Int64 OrderId)
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
                string sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                objUsrInfo = objUsrInfoRepo.GetById((long)objOrder.UserId);
                string sToadd = "";
                Int64 sToUserInfoID = 0;
                if (objUsrInfo != null)
                {

                    sToadd = objUsrInfo.LoginEmail;
                    sToUserInfoID = objUsrInfo.UserInfoID;
                }
                string sSubject = "Return Authorization - " + lblOrderNo.Text;
                string sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();


                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                messagebody.Replace("{FullName}", objUsrInfo.FirstName + " " + objUsrInfo.LastName);
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                messagebody.Replace("{Note}", txtNote.Text);





                string smtphost = Application["SMTPHOST"].ToString();
                int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                string smtpUserID = Application["SMTPUSERID"].ToString();
                string smtppassword = Application["SMTPPASSWORD"].ToString();
                //bool htmlbdy = true;
                //bool ssl = false;

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ReturnNotes)) == true)
                {
                    new CommonMails().SendMail(sToUserInfoID, "Order Return Notes", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
                }
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    private void BindReturnStatus(DropDownList ddlReturnStatus)
    {
        try
        {
            LookupDA sReturnStatus = new LookupDA();
            LookupBE sReturnStatusBE = new LookupBE();
            sReturnStatusBE.SOperation = "selectall";
            sReturnStatusBE.iLookupCode = "Return Status";
            DataSet ds = new DataSet();
            ds = sReturnStatus.LookUp(sReturnStatusBE);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            dt.DefaultView.Sort = "sLookupName";

            ddlReturnStatus.DataSource = dt.DefaultView.ToTable();
            ddlReturnStatus.DataValueField = "iLookupID";
            ddlReturnStatus.DataTextField = "sLookupName";
            ddlReturnStatus.DataBind();
            ddlReturnStatus.Items.Insert(0, new ListItem("-Select Reason Code-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    public void DisplayNotes()
    {
        try
        {
            if (Request.QueryString["OrderID"] != null)
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
                txtOrderNotesForCECA.Text = string.Empty;
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

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    public void DisplayNotesIE()
    {
        try
        {
            if (Request.QueryString["OrderID"] != null)
            {
                OrderID = Convert.ToInt64(Request.QueryString["OrderID"].ToString());
            }
            else
            {
                OrderID = 0;
            }
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            List<NoteDetail> objList = new List<NoteDetail>();
            objList = objRepo.GetNotesForIEPerOrderId(OrderID, Incentex.DAL.Common.DAEnums.NoteForType.OrderProductReturns, "IEInternalNotes",0);
            if (objList.Count == 0)
            {
                txtOrderNotesForIE.Text = "";
            }
            else
            {
                txtOrderNotesForIE.Text = string.Empty;
                foreach (NoteDetail obj in objList)
                {
                    txtOrderNotesForIE.Text += obj.Notecontents;
                    txtOrderNotesForIE.Text += "\n\n";
                    UserInformationRepository objUserRepo = new UserInformationRepository();
                    UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);
                    if (objUser != null)
                    {
                        txtOrderNotesForIE.Text += objUser.FirstName + " " + objUser.LastName + "   ";
                    }
                    txtOrderNotesForIE.Text += Convert.ToDateTime(obj.CreateDate).ToString("MM/dd/yyyy");
                    txtOrderNotesForIE.Text += " @ " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n";
                    txtOrderNotesForIE.Text += "______________________________________________________________________________";
                    txtOrderNotesForIE.Text += "\n\n";



                }
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    private void SetPageProperties()
    {
        try
        {
            List<OrderDetailHistoryRepository.OrderUserInfo> objList = new List<OrderDetailHistoryRepository.OrderUserInfo>();
            objList = ObjOrdDetailHisRepo.GetOrderUserInfoByOrderID(this.OrderID);
            if(objList.Count > 0)
            {
                this.RANumber = objList[0].OrderNumber;
                this.FirstName = objList[0].FirstName;
                this.LastName = objList[0].LastName;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    private void BindGridview()
    {
        List<GetReturnOrderDetailsResult> objList = new List<GetReturnOrderDetailsResult>();
        objList = objPrdReturnRepos.GetReturnOrderDetails(this.OrderID);
        if (objList.Count > 0)
        {
            gvProductReturn.DataSource = objList;
            gvProductReturn.DataBind();
            lblOrderNo.Text = "RA " + RANumber;
            lblCustomer.Text = this.FirstName + " " + this.LastName;
            if (objList.Where(m => m.ReceivedQty != null).Count() == objList.Count)
                lblStatus.Text = "Completed";
            else if (objList.Where(m => m.ReceivedQty != null).Count() == 0)
                lblStatus.Text = "Pending";
            else
            {
                lblStatus.Text = "Partial Return Completed";
                //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Return Order is already Received.');", true);
            }
            lblSubmitDate.Text = Convert.ToDateTime(objList[0].SubmitDate).ToString("MM/dd/yyyy");

            if (objList.Where(m => m.ReceivedQty != null).Count() > 0)
                lblProcessDate.Text = Convert.ToDateTime(objList.Where(m => m.ReceivedQty != null).ToList()[0].ProcessedDate).ToString("MM/dd/yyyy");
            // txtPrdDescription.Text = objList[0].Reason;
        }
    }

    /// <summary>
    /// Below Method  is used to send Eamil for Return Procesing Note to CA/CE.
    /// </summary>
    /// <param name="strStatus"></param>
    /// <param name="OrderID"></param>
    /// <param name="itemDetail"></param>
    private void sendVerificationEmail(string strStatus, Int64 OrderID, String itemDetail)
    {
        try
        {
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            //objEmailBE.STemplateName = "OrderStatus";
            objEmailBE.STemplateName = "ReturnProcessed";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                //Find UserName who had order purchased
                Order objOrder = new OrderConfirmationRepository().GetByOrderID(OrderID);
                UserInformation objUserInformation = new UserInformationRepository().GetById(objOrder.UserId);

                string sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                string sToadd = objUserInformation.LoginEmail;
                string sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString().Replace("(Order #)", this.RANumber);
                string sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{OrderNo}", objOrder.OrderNumber);
                messagebody.Replace("{status}", strStatus);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                messagebody.Replace("{fullname}", objUserInformation.FirstName + " " + objUserInformation.LastName);
                messagebody.Replace("{ItemDetail}", itemDetail);
                messagebody.Replace("{ProcessedDate}", DateTime.Now.ToString("MM/dd/yyyy"));

                string smtphost = Application["SMTPHOST"].ToString();
                int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                string smtpUserID = Application["SMTPUSERID"].ToString();
                string smtppassword = Application["SMTPPASSWORD"].ToString();
                if (HttpContext.Current.Request.IsLocal)
                    new CommonMails().SendEmail4Local(objUserInformation.UserInfoID, "Order Status", "surendar.yadav@indianic.com", sSubject, messagebody.ToString(), "incentextest6@gmail.com", "test6incentex", true, OrderID);
                else
                    new CommonMails().SendMail(objUserInformation.UserInfoID, "Order Status", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);                
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Method is is used to send a Return Notification -Accounting Related to IE who have rights of Return Notification -Accounting Related.
    /// </summary>
    /// <param name="EmailAddressFor"></param>
    /// <param name="password"></param>
    /// <param name="itemDetail"></param>
    /// <param name="OrderNumber"></param>
    private void sendIEEmail(String EmailAddressFor, String password, String itemDetail, String OrderNumber)
    {
        List<UserInformation> objAdminList = new List<UserInformation>();
        objAdminList = new UserInformationRepository().GetEmailInformation();
        if (objAdminList.Count > 0)
        {
            for (Int32 i = 0; i < objAdminList.Count; i++)
            {
                sendApprovalEmailIE(EmailAddressFor, objAdminList[i].Email, password, objAdminList[i].FirstName + " " + objAdminList[i].LastName, objAdminList[i].UserInfoID, itemDetail, OrderNumber);
            }
        }
        //End
    }

    private void sendApprovalEmailIE(String EmailAddressFor, String ToEmailAddress, String password, String FullName, Int64 UserInfoID, String itemDetail, String OrderNumber)
    {
        try
        {
            //UserInformation objUserInformation = new UserInformationRepository().GetByLoginEmail(EmailAddressFor);

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "ReturnNotificationsAccountingRelated";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sToadd = ToEmailAddress;
                Int64 sToUserInfoID = UserInfoID;
                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{fullname}", FullName);
                messagebody.Replace("{OrderNo}", OrderNumber);
                messagebody.Replace("{ItemDetail}", itemDetail);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ReturnNotificationsAccountingRelated)) == true)
                {
                    if (HttpContext.Current.Request.IsLocal)
                        new CommonMails().SendEmail4Local(sToUserInfoID, "Return Notifications - Accounting Related", "surendar.yadav@indianic.com", sSubject, messagebody.ToString(), "incentextest6@gmail.com", "test6incentex", true, OrderID);
                    else
                        new CommonMails().SendMail(sToUserInfoID, "Return Notifications - Accounting Related", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, OrderID);
                }
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// Return TotalSales tax rate of that State by ZipCode
    /// </summary>
    /// <param name="ZipCode"></param>
    /// <returns></returns>
    private Double GetSalesTax(String ZipCode)
    {
        Double TotalSalesTax = 0D;
        try
        {
            /// This sample project demonstrates the use of the GetTaxRateUS operation within StrikeIron Sales & Use Tax Basic 5.0
            /// In order to run the application, you must have a Registered StrikeIron account.  If you do not have a Registered
            /// StrikeIron account, you can obtain one here: http://www.strikeiron.com/Register.aspx
            /// 
            /// The web service definition for used in the Web Reference for this project is located at http://ws.strikeiron.com/taxdatabasic5?WSDL
            /// You can view more information about this web service here: https://strikeiron.com/ProductDetail.aspx?p=444
            /// 
            /// Integrating a web service into a .NET application usually requires adding a Web Reference to the project using that
            /// web service's definition.  A Web Reference has already been added to this project; the reference was assign the
            /// namespace TaxBasicRef.  The namespace has been added to this codefile above.
            /// 

            /// Variables storing authentication values are declared below.  As a Registered StrikeIron user, you can authenticate
            /// to a StrikeIron web service with either a UserID/Password combination or a License Key.  If you wish to use a
            /// License Key, assign this value to the UserID field and set the Password field to null.
            /// 
            string userID = ConfigurationSettings.AppSettings["StrikeironUserID"];
            string password = ConfigurationSettings.AppSettings["StrikeironPassword"];

            /// Inputs for the GetTaxRateUS operation are declared below.
            /// 
            //string zipCode = txtSZip.Text.Trim(); //this is the ZIP code to get for which the operation will return data
            //zipCode = zipCode.Contains('-') ? zipCode.Substring(0, zipCode.IndexOf('-')) : zipCode;
            /// To access the web service operations, you must declare a web service client object.  This object will contain
            /// all of the methods available in the web service and properties for each portion of the SOAP header.
            /// The class name for the web service client object (assigned automatically by the Web Reference) is TaxDataBasic.
            /// 
            TaxDataBasic siService = new TaxDataBasic();

            /// StrikeIron web services accept user authentication credentials as part of the SOAP header.  .NET web service client
            /// objects represent SOAP header values as public fields.  The name of the field storing the authentication credentials
            /// is LicenseInfoValue (class type LicenseInfo).
            /// 
            LicenseInfo authHeader = new LicenseInfo();

            /// Registered StrikeIron users pass authentication credentials in the RegisteredUser section of the LicenseInfo object.
            /// (property name: RegisteredUser; class name: RegisteredUser)
            /// 
            RegisteredUser regUser = new RegisteredUser();

            /// Assign credential values to this RegisteredUser object
            /// 
            regUser.UserID = userID;
            regUser.Password = password;

            /// The populated RegisteredUser object is now assigned to the LicenseInfo object, which is then assigned to the web
            /// service client object.
            /// 
            authHeader.RegisteredUser = regUser;
            siService.LicenseInfoValue = authHeader;

            /// The GetTaxRateUS operation can now be called.  The output type for this operation is SIWSOutputOfTaxRateUSAData.
            /// Note that for simplicity, there is no error handling in this sample project.  In a production environment, any
            /// web service call should be encapsulated in a try-catch block.
            /// 
            SIWsOutputOfTaxRateUSAData wsOutput = siService.GetTaxRateUS(ZipCode);

            /// The output objects of this StrikeIron web service contains two sections: ServiceStatus, which stores data
            /// indicating the success/failure status of the the web service request; and ServiceResult, which contains the
            /// actual data returne as a result of the request.
            /// 
            /// ServiceStatus contains two elements - StatusNbr: a numeric status code, and StatusDescription: a string
            /// describing the status of the output object.  As a standard, you can apply the following assumptions for the value of
            /// StatusNbr:
            ///   200-299: Successful web service call (data found, etc...)
            ///   300-399: Nonfatal error (No data found, etc...)
            ///   400-499: Error due to invalid input
            ///   500+: Unexpected internal error; contact support@strikeiron.com
            if (wsOutput.ServiceStatus.StatusNbr >= 300)
                TotalSalesTax = 0D;
            else
                TotalSalesTax = wsOutput.ServiceResult.TotalSalesTax;


        }
        catch { }
        return TotalSalesTax;
    }
    #endregion
}
