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

public partial class OrderManagement_OrderNotesHistory : PageBase
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
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfo();
    CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfo();
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    protected void Page_Load(object sender, EventArgs e)
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

            ((Label)Master.FindControl("lblPageHeading")).Text = "Notes/History";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/OrderManagement/OrderDetailView.aspx";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.OrderReturnURL;
            if (Request.QueryString["Id"] != null)
            {
                this.OrderID = Convert.ToInt64(Request.QueryString["Id"]);
            }
            //Bind Menu
            menucontrol.PopulateMenu(3, 0, this.OrderID, 0, false);
            //End
            
            BindOrderStatus();
            if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
            {
                DisplayNotes();
                DisplayNotesForIE();
            }
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            {
                lnkAddNewIE.Visible = false;
                pnlNotesForIE.Visible = false;
            }
            else
            {
                lnkAddNewIE.Visible = true;
                pnlNotesForIE.Visible = true;
            }
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
                INC_Lookup objLookup = new LookupRepository().GetById((long)objOrder.PaymentOption);
                lblPaymentMethod.Text = objLookup.sLookupName;
            }
            else
            {
                lblPaymentMethod.Text = "Paid By Corporate";
            }
            lblOrderStatus.Text = objOrder.OrderStatus.ToString();

            //Start Update Nagmani Change 31-May-2011

            //objBillingInfo = objCmpEmpContRep.GetBillingDetailById((long)objOrder.UserId);
            //BindBillingAddress();

            // objShippingInfo = objCmpEmpContRep.GetShippingDetailById((long)objOrder.UserId, objOrder.ShippingInfromationid);
            //BindShippingAddress();

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
            lblBCity.Text = objCity.GetById((long)objBillingInfo.CityID).sCityName + "," + objState.GetById((long)objBillingInfo.StateID).sStatename + " " + objBillingInfo.ZipCode;
        lblBCompany.Text = objBillingInfo.CompanyName;
        if (objBillingInfo.CountryID != null)
            lblBCountry.Text = objCountry.GetById((long)objBillingInfo.CountryID).sCountryName;
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
            lblSCity.Text = objCity.GetById((long)objShippingInfo.CityID).sCityName + "," + objState.GetById((long)objShippingInfo.StateID).sStatename + " " + objShippingInfo.ZipCode;
        lblSCompany.Text = objShippingInfo.CompanyName;
        if (objShippingInfo.CountryID != null)
            lblSCountry.Text = objCountry.GetById((long)objShippingInfo.CountryID).sCountryName;
        lblSName.Text = objShippingInfo.Name + " " + objShippingInfo.Fax;
    }
    protected void lnkButton_Click(object sender, EventArgs e)
    {
        try
        {

            string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.CACE);

            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();


            objComNot.Notecontents = txtNote.Text;
            objComNot.NoteFor = strNoteFor;
            objComNot.ForeignKey = this.OrderID;
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID; // 10; //Session["CurrentUser"].ToString();
            objComNot.UpdateDate = System.DateTime.Now;
            objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;// 10;

            if (!(string.IsNullOrEmpty(txtNote.Text)))
            {

                objCompNoteHistRepos.Insert(objComNot);
                objCompNoteHistRepos.SubmitChanges();
                int NoteId = (int)(objComNot.NoteID);
                sendVerificationMailForNotesToCE(this.OrderID);
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

    protected void linkbtnNotesForIE_Click(object sender, EventArgs e)
    {
        try
        {
            string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.OrderDetailsIEs);

            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();


            objComNot.Notecontents = txtNoteForIE.Text;
            objComNot.NoteFor = strNoteFor;
            objComNot.ForeignKey = this.OrderID;
            objComNot.SpecificNoteFor = "IEInternalNotes";
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID; // 10; //Session["CurrentUser"].ToString();
            objComNot.UpdateDate = System.DateTime.Now;
            objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;// 10;

            if (!(string.IsNullOrEmpty(txtNoteForIE.Text)))
            {

                objCompNoteHistRepos.Insert(objComNot);
                objCompNoteHistRepos.SubmitChanges();
                int NoteId = (int)(objComNot.NoteID);

                txtNoteForIE.Text = "";

            }
            else
            {

            }

            DisplayNotesForIE();
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

            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            List<NoteDetail> objList = new List<NoteDetail>();
            objList = objRepo.GetNotesForCACEPerOrderId(this.OrderID, Incentex.DAL.Common.DAEnums.NoteForType.CACE);
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
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public void DisplayNotesForIE()
    {
        try
        {

            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            List<NoteDetail> objList = new List<NoteDetail>();
            objList = objRepo.GetNotesForIEPerOrderId(OrderID, Incentex.DAL.Common.DAEnums.NoteForType.OrderDetailsIEs, "IEInternalNotes", 0);
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
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            modalAddnotes.Show();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void sendVerificationMailForNotesToCE(Int64 OrderId)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();

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
                string sSubject = "Order Notes - " + lblOrderNo.Text;
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

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderNotes)) == true)
                {
                    new CommonMails().SendMail(sToUserInfoID, "Order Notes", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}
