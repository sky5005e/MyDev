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

public partial class ProductReturnManagement_ReturnProductIncenteEmployee : PageBase
{
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
    string Color
    {
        get
        {
            if (ViewState["Color"] == null)
            {
                ViewState["Color"] = 0;
            }
            return Convert.ToString(ViewState["Color"]);
        }
        set
        {
            ViewState["Color"] = value;
        }
    }
    string ToDate = null;
    string FromDate = null;
    string StoreId = null;
    string Email = null;
    string OrderNumber = null;
    string OrdeStatus = null;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Return View";
            // As per Amanda Post in Base Camp: Return Management "Go Back" Issue.
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/ProductReturnManagement/ProductReturnLookupSearch.aspx";
            //if (Session["ReturnMgtBack"] != null)
            //   ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = Convert.ToString(Session["ReturnMgtBack"]);
            //((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/ProductReturnManagement/ReturnProductIE.aspx?StoreId=" + StoreId + "&ToDate=" + ToDate + "&FromDate=" + FromDate + "&Email=" + Email + "&OrderNumber=" + OrderNumber + "&OrdeStatus=" + OrdeStatus;
            
            //((HtmlImage)Master.FindControl("imgShoppingCart")).Visible = true;
            if (Request.QueryString["OrderId"] != null)
            {
                this.OrderID = Convert.ToInt64(Request.QueryString["OrderId"]);
                BindRequestDropDownlist();
                BindGridview();
                DisplayNotes();
                DisplayNotesIE();
                BindAddress();
                BindOrderStatus();
                //Start 15-March-2012
                SetBillingShipping();
                //End 15-March-2012

            }
        }
    }
    private void BindGridview()
    {
        List<ReturnProductDetailsOnOrderIDResult> objList = new List<ReturnProductDetailsOnOrderIDResult>();
        objList = objPrdReturnRepos.GetProductReturnOnOrderID(this.OrderID);
        if (objList.Count > 0)
        {
            gvProductReturn.DataSource = objList;
            gvProductReturn.DataBind();
            lblOrderNo.Text = "RA " + objList[0].OrderNumber;
         
           txtPrdDescription.Text = objList[0].Reason;
         
        }
    }
    protected void gvProductReturn_RowDataBound(object sender, GridViewRowEventArgs e)
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
          //  DropDownList ddlRequest = (DropDownList)e.Row.FindControl("ddlRequesting");
            HiddenField hdnRequest = (HiddenField)e.Row.FindControl("hdnRequest");
            objNew = OrdShippOrder.GetDesCription(Convert.ToInt32(this.OrderID), Convert.ToInt32(hdnShoppingCartId.Value), hdnitemNo.Value);
            if (objNew.Count > 0)
            {
                this.Color = objNew[0].Color;
                lblSize.Text = objNew[0].Size;
                lblProductDescription.Text = objNew[0].ProductDescrption;
                ((Image)e.Row.FindControl("imgColor")).ImageUrl = "~/admin/Incentex_Used_Icons/" + new LookupRepository().GetIconByLookupName(Color);
                lblColor.Visible = false;


            }
          
            if (hdnRequest.Value != null)
            {
                ddlRequesting.SelectedIndex = ddlRequesting.Items.IndexOf(ddlRequesting.Items.FindByText(hdnRequest.Value));
            }
            else
            {
                ddlRequesting.SelectedIndex = 0;
            }
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
            objList = objRepo.GetNotesForIEPerOrderId(OrderID, Incentex.DAL.Common.DAEnums.NoteForType.OrderProductReturns,"IEInternalNotes",0);
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
    private void BindAddress()
    {
        try
        {
            List<SelectOrderAddressResult> obj = new List<SelectOrderAddressResult>();
            if (Request.QueryString["OrderID"] != null)
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
                lblOrderNo.Text = "RA "+obj[0].OrderNumber;
            }
            else
            {
                lblOrderNo.Text = "";
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
         if (Request.QueryString["OrderID"] != null)
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
                    INC_Lookup objLookup = new LookupRepository().GetById((long)objOrder.PaymentOption);
                    lblPaymentMethod.Text = objLookup.sLookupName;
                }
                else
                {
                    lblPaymentMethod.Text = "";
                }

               // lblOrderStatus.Text = objOrder.OrderStatus.ToString();
                if (Request.QueryString["Status"] != null)
                {
                    lblOrderStatus.Text = Request.QueryString["Status"].ToString();
                }

                if (objOrder.OrderFor == "IssuanceCart")
                {
                    //Comapny Pays
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((long)objOrder.UserId, objOrder.OrderID);
                    BindBillingAddress();

                    objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((long)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                    BindShippingAddress();

                }
                else
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((long)objOrder.UserId, objOrder.OrderID);
                    BindBillingAddress();

                    objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((long)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                    BindShippingAddress();

                }

                //End Nagmani Change 31-May-2011


            }
            else
            {
                lblOrderedDate.Text = "";
                lblOrderBy.Text = "";
                lblPaymentMethod.Text = "";

                lblOrderStatus.Text = "";
                //Billing Address
                lblBAddress.Text = "";
                lblBCity.Text = "";
                lblBCompany.Text = "";
                lblBCountry.Text = "";
                lblBName.Text = "";
                //Shipping Address
                lblSAddress.Text = "";
                lblSCity.Text = "";
                lblSCompany.Text = "";
                lblSCountry.Text = "";
                lblSName.Text = "";
            }



            //End Added



        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void BindBillingAddress()
    {
        lblBAddress.Text = objBillingInfo.Address;
        lblBCity.Text = objCity.GetById((long)objBillingInfo.CityID).sCityName + "," + objState.GetById((long)objBillingInfo.StateID).sStatename + "," + objBillingInfo.ZipCode;
        lblBCompany.Text = objBillingInfo.CompanyName;
        lblBCountry.Text = objCountry.GetById((long)objBillingInfo.CountryID).sCountryName;
        if (!string.IsNullOrEmpty(objBillingInfo.Manager))
        {
            lblBName.Text = objBillingInfo.BillingCO + " " + objBillingInfo.Manager;
        }
        else
        {
            lblBName.Text = objBillingInfo.BillingCO;
        }
        //Start 15-March-2012
        hfBillingInfoID.Value = objBillingInfo.CompanyContactInfoID.ToString();
        //End 15-March-2012

    }
    protected void BindShippingAddress()
    {
        lblSAddress.Text = objShippingInfo.Address;
        lblSCity.Text = objCity.GetById((long)objShippingInfo.CityID).sCityName + "," + objState.GetById((long)objShippingInfo.StateID).sStatename + "," + objShippingInfo.ZipCode;
        lblSCompany.Text = objShippingInfo.CompanyName;
        lblSCountry.Text = objCountry.GetById((long)objShippingInfo.CountryID).sCountryName;
        lblSName.Text = objShippingInfo.Name + " " + objShippingInfo.Fax;
        //Start 15-March-2012
        hfShippingInfoID.Value = objShippingInfo.CompanyContactInfoID.ToString();
        //End 15-March-2012
    }
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
    private void BindRequestDropDownlist()
    {
        try
        {
            string strRequest = "Request";
            ddlRequesting.DataSource = objLookRep.GetByLookup(strRequest);
            ddlRequesting.DataValueField = "iLookupID";
            ddlRequesting.DataTextField = "sLookupName";
            ddlRequesting.DataBind();
            ddlRequesting.Items.Insert(0, new ListItem("-Select-", "0"));

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    //Start 15-March-2012
    protected void btnEditBilling_Click(object sender, EventArgs e)
    {
        SetBillingAddress();
        modalEditBilling.Show();

    }
    protected void btnEditShipping_Click(object sender, EventArgs e)
    {
        SetShippingAddress();
        modalEditBilling.Show();
    }
    protected void SetBillingAddress()
    {

        btnSaveBilling.Text = "Edit Billing";
        OrderConfirmationRepository objOrderConfirEdit = new OrderConfirmationRepository();
        Order objOrderEdit = objOrderConfirEdit.GetByOrderID(this.OrderID);


        if (objOrderEdit.OrderFor == "IssuanceCart")
        {
            //Comapny Pays
            objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((long)objOrderEdit.UserId, objOrderEdit.OrderID);
        }
        else
        {
            objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((long)objOrderEdit.UserId, objOrderEdit.OrderID);
        }

        //Bind Country
        Common.BindCountry(drpCountry);
        drpCountry.Items.FindByValue(objBillingInfo.CountryID.ToString()).Selected = true;

        //Bind State
        Common.BindState(drpState, (long)objBillingInfo.CountryID);
        drpState.Items.FindByValue(objBillingInfo.StateID.ToString()).Selected = true;


        //bind City
        Common.BindCity(drpCity, (long)objBillingInfo.StateID);
        drpCity.Items.FindByValue(objBillingInfo.CityID.ToString()).Selected = true;





        txtZipcodeEdit.Text = objBillingInfo.ZipCode;


        txtBillingFnameEdit.Text = objBillingInfo.BillingCO;
        txtBillingLnameEdit.Text = objBillingInfo.Manager;
        txtBillingCompanyNameEdit.Text = objBillingInfo.CompanyName;
        txtBillingAddressEdit.Text = objBillingInfo.Address;
        txtEmailAdrressEdit.Text = objBillingInfo.Email;
        txtPhoneNumberEdit.Text = objBillingInfo.Telephone;


    }
    protected void SetShippingAddress()
    {

        btnSaveBilling.Text = "Edit Shipping";

        OrderConfirmationRepository objOrderConfirEdit = new OrderConfirmationRepository();
        Order objOrderEdit = objOrderConfirEdit.GetByOrderID(this.OrderID);


        if (objOrderEdit.OrderFor == "IssuanceCart")
        {
            objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((long)objOrderEdit.UserId, objOrderEdit.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
        }
        else
        {
            objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((long)objOrderEdit.UserId, objOrderEdit.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
        }

        //Bind Country
        Common.BindCountry(drpCountry);
        drpCountry.Items.FindByValue(objShippingInfo.CountryID.ToString()).Selected = true;

        //Bind State
        Common.BindState(drpState, (long)objShippingInfo.CountryID);
        drpState.Items.FindByValue(objShippingInfo.StateID.ToString()).Selected = true;


        //bind City
        Common.BindCity(drpCity, (long)objShippingInfo.StateID);
        drpCity.Items.FindByValue(objShippingInfo.CityID.ToString()).Selected = true;


        txtZipcodeEdit.Text = objShippingInfo.ZipCode;
        txtBillingFnameEdit.Text = objShippingInfo.Name;
        txtBillingLnameEdit.Text = objShippingInfo.Fax;
        txtBillingCompanyNameEdit.Text = objShippingInfo.CompanyName;
        txtBillingAddressEdit.Text = objShippingInfo.Address;
        txtEmailAdrressEdit.Text = objShippingInfo.Email;
        txtPhoneNumberEdit.Text = objShippingInfo.Telephone;
    }
    protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindState(drpState, Convert.ToInt64(drpCountry.SelectedValue));
        drpState_SelectedIndexChanged(sender, e);
        modalEditBilling.Show();

    }
    protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindCity(drpCity, Convert.ToInt64(drpState.SelectedValue));
        modalEditBilling.Show();
    }
    protected void btnSaveBilling_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSaveBilling.Text == "Edit Billing")
            {
                CompanyEmployeeContactInfoRepository objCmpEmpBilling = new CompanyEmployeeContactInfoRepository();
                objBillingInfo = objCmpEmpBilling.GetBillingDetailsByID((Convert.ToInt64(hfBillingInfoID.Value.ToString())));

                //Edit Billing Details here
                objBillingInfo.ZipCode = txtZipcodeEdit.Text;
                objBillingInfo.BillingCO = txtBillingFnameEdit.Text;
                objBillingInfo.Manager = txtBillingLnameEdit.Text;
                objBillingInfo.CompanyName = txtBillingCompanyNameEdit.Text;
                objBillingInfo.Address = txtBillingAddressEdit.Text;
                objBillingInfo.Email = txtEmailAdrressEdit.Text;
                objBillingInfo.Telephone = txtPhoneNumberEdit.Text;
                objBillingInfo.CountryID = Convert.ToInt64(drpCountry.SelectedItem.Value.ToString());
                objBillingInfo.StateID = Convert.ToInt64(drpState.SelectedItem.Value.ToString());
                objBillingInfo.CityID = Convert.ToInt64(drpCity.SelectedItem.Value);
                objCmpEmpBilling.SubmitChanges();
            }
            else
            {
                CompanyEmployeeContactInfoRepository objCmpEmpShipping = new CompanyEmployeeContactInfoRepository();
                objShippingInfo = objCmpEmpShipping.GetShippingDetailsByID((Convert.ToInt64(hfShippingInfoID.Value.ToString())));
                //Edit Shipping details here
                objShippingInfo.ZipCode = txtZipcodeEdit.Text;
                objShippingInfo.Name = txtBillingFnameEdit.Text;
                objShippingInfo.Fax = txtBillingLnameEdit.Text;
                objShippingInfo.CompanyName = txtBillingCompanyNameEdit.Text;
                objShippingInfo.Address = txtBillingAddressEdit.Text;
                objShippingInfo.Email = txtEmailAdrressEdit.Text;
                objShippingInfo.Telephone = txtPhoneNumberEdit.Text;
                objShippingInfo.CountryID = Convert.ToInt64(drpCountry.SelectedItem.Value.ToString());
                objShippingInfo.StateID = Convert.ToInt64(drpState.SelectedItem.Value.ToString());
                objShippingInfo.CityID = Convert.ToInt64(drpCity.SelectedItem.Value);
                objCmpEmpShipping.SubmitChanges();
            }
            //Clear the fields
            clearBillingDetails();
            //Bind Updated Details again
            BindOrderStatus();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private void clearBillingDetails()
    {
        txtZipcodeEdit.Text = string.Empty;
        txtBillingFnameEdit.Text = string.Empty;
        txtBillingLnameEdit.Text = string.Empty;
        txtBillingCompanyNameEdit.Text = string.Empty;
        txtBillingAddressEdit.Text = string.Empty;
        txtEmailAdrressEdit.Text = string.Empty;
        txtPhoneNumberEdit.Text = string.Empty;
        drpCountry.SelectedIndex = 0;
        drpState.SelectedIndex = 0;
        drpCity.SelectedIndex = 0;
    }
    private void SetBillingShipping()
    {

        if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
        {


            btnEditBilling.Visible = true;
            btnEditShipping.Visible = true;
        }

        else
        {
            btnEditBilling.Visible = false;
            btnEditShipping.Visible = false;
        }
    }
    //End Start 15-March-2012
}