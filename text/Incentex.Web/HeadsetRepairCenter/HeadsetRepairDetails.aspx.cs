using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Text;
using Incentex.DAL.Common;
using System.Configuration;
using System.Data;
using Incentex.BE;
using Incentex.DA;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class HeadsetRepairCenter_HeadsetRepairDetails : PageBase
{
    #region Data Member's
    long RepairNumberDetails
    {
        get
        {
            if (ViewState["RepairNumberDetails"] != null)
                return Convert.ToInt64(ViewState["RepairNumberDetails"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["RepairNumberDetails"] = value;
        }
    }
    String RepairNumber
    {
        get
        {
            if (ViewState["RepairNumber"] != null)
                return ViewState["RepairNumber"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["RepairNumber"] = value;
        }
    }
    long? Company
    {
        get
        {
            if (ViewState["Company"] != null)
                return Convert.ToInt64(ViewState["Company"].ToString());
            else
                return null;
        }
        set
        {
            ViewState["Company"] = value;
        }
    }
    long? Contact
    {
        get
        {
            if (ViewState["Contact"] != null)
                return Convert.ToInt64(ViewState["Contact"].ToString());
            else
                return null;
        }
        set
        {
            ViewState["Contact"] = value;
        }
    }
    long? Status
    {
        get
        {
            if (ViewState["Status"] != null)
                return Convert.ToInt64(ViewState["Status"].ToString());
            else
                return null;
        }
        set
        {
            ViewState["Status"] = value;
        }
    }

    HeadsetRepairCenterRepository objHeadsetRepair = new HeadsetRepairCenterRepository();
    NotesHistoryRepository objGenralNote = new NotesHistoryRepository();
    UserInformationRepository objUserInformationRepository = new UserInformationRepository();
    IncentexEmployeeRepository objIncenteEmployee = new IncentexEmployeeRepository();
    LookupRepository objLookRep = new LookupRepository();
    #endregion

    #region Events

    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                MasterPageFile = "~/admin/MasterPageAdmin.master";
            }
            else
            {
                MasterPageFile = "~/MasterPage.master";
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Headset Repair Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            BindDropDown();

            if (!string.IsNullOrEmpty(Request.QueryString["RepairNumberDetails"])) //Edited RepairNumber
                this.RepairNumberDetails = Convert.ToInt64(Request.QueryString["RepairNumberDetails"].ToString());
            if (!String.IsNullOrEmpty(Request.QueryString["RepairNumber"]))
                this.RepairNumber = Convert.ToString(Request.QueryString["RepairNumber"]);
            if (!String.IsNullOrEmpty(Request.QueryString["Company"]))
                this.Company = Convert.ToInt64(Request.QueryString["Company"]);
            if (!String.IsNullOrEmpty(Request.QueryString["Contact"]))
                this.Contact = Convert.ToInt64(Request.QueryString["Contact"]);
            if (!string.IsNullOrEmpty(Request.QueryString["Status"]))
                this.Status = Convert.ToInt64(Request.QueryString["Status"]);


            ((Label)Master.FindControl("lblPageHeading")).Text = "Headset Repair Details";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/HeadsetRepairCenter/ListofHeadsetRepairCenter.aspx?Company=" + Company + "&Contact=" + Contact + "&RepairNumber=" + RepairNumber + "&Status=" + Status + "";




            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) ||
          IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
            {
                txtEnterOrderTrackingNumber.ReadOnly = true;
                txtrepairquoteamount.ReadOnly = true;
                txtestimatedleadtime.ReadOnly = true;
                dvHeadsetQuote.Style.Add("Height", "205px");
                lnkbtnSubmitQuote.Visible = false;

                lnkBtnTrackingNumber.Visible = false;
                ddlStatus.Enabled = false;

                dtIE.Visible = true;
                dvVendorInformation.Visible = false;
                dvVendorspace.Visible = false;

                modal.PopupControlID = "pnlGeneralIENote";
                modal.CancelControlID = "cboxpasswordClose";

                spnlogintype.InnerHtml = Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee.ToString();
            }
            else
            {
                spnlogintype.InnerHtml = Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin.ToString();
            }

            this.GetHeadsetRepairDetails();
        }
    }

    protected void btnSubmitNote_Click(object sender, EventArgs e)
    {
        NoteDetail objNotedetails = new NoteDetail();

        objNotedetails.NoteFor = "HeadsetRepairCenter";
        objNotedetails.ForeignKey = Convert.ToInt64(hdndeadsetRepairID.Value);
        objNotedetails.CreateDate = DateTime.Now;
        objNotedetails.UpdateDate = DateTime.Now;

        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) ||
            IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
        {
            objNotedetails.Notecontents = txtMessage.Text.Trim();
            objNotedetails.SpecificNoteFor = "CEActivity";         //Note post by IE or Superadmin
            objNotedetails.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;   //IE or Superadmin ID
            objNotedetails.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;  //IE or Superadmin ID
            objNotedetails.ReceivedBy = hdnContactID.Value;   //  CA/CE ID  
        }
        else
        {
            objNotedetails.Notecontents = txtIEMessage.Text.Trim();
            objNotedetails.SpecificNoteFor = "IEActivity";         //Note post by CE or CA
            objNotedetails.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;   // CA/CE ID
            objNotedetails.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;  // CA/CE ID
        }

        objGenralNote.Insert(objNotedetails);
        objGenralNote.SubmitChanges();

        List<long> lstIEUserID = new List<long>();

        //write code for sending email to IE based on checkmark selection
        if (chkIsEmailSendIE.Checked)
        {
            foreach (DataListItem item in dtIE.Items)
            {
                if (((CheckBox)item.FindControl("chkUser")).Checked == true)
                {
                    HiddenField hdnEmail = ((HiddenField)item.FindControl("hdnEmail"));
                    HiddenField hdnUserID = ((HiddenField)item.FindControl("hdnUserID"));

                    lstIEUserID.Add(Convert.ToInt64(hdnUserID.Value));
                    SendNotes(objNotedetails, hdnEmail.Value, "CEActivity", Convert.ToInt64(hdnUserID.Value));
                }
            }

            objHeadsetRepair.InsertHeadsetRepairCenterManageEmail(lstIEUserID, objNotedetails.NoteID, objNotedetails.ForeignKey.Value);
        }

        //Senting email to CA/CE
        if (chkIsEmailSend.Checked)
        {
            foreach (DataListItem item in dtlIE.Items)
            {
                if (((CheckBox)item.FindControl("chkUser")).Checked == true)
                {
                    HiddenField hdnEmail = ((HiddenField)item.FindControl("hdnEmail"));
                    HiddenField hdnUserID = ((HiddenField)item.FindControl("hdnUserID"));

                    lstIEUserID.Add(Convert.ToInt64(hdnUserID.Value));
                    SendNotes(objNotedetails, hdnEmail.Value, "CEActivity", Convert.ToInt64(hdnUserID.Value));
                }
            }

            objHeadsetRepair.InsertHeadsetRepairCenterManageEmail(lstIEUserID, objNotedetails.NoteID, objNotedetails.ForeignKey.Value);

            UserInformation objUserInformation = objUserInformationRepository.GetById(Convert.ToInt64(hdnContactID.Value));
            SendNotes(objNotedetails, objUserInformation.Email, "IEActivity", Convert.ToInt64(hdnContactID.Value));
        }

        this.GetGeneralNotes();
    }

    protected void lnkbtnGeneralNotes_Click(object sender, EventArgs e)
    {
        List<IEListResultsCustom> listIE = objHeadsetRepair.GetAllEmployeeCustome(this.RepairNumberDetails);

        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) ||
       IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
        {
            modal.PopupControlID = "pnlGeneralIENote";
            modal.CancelControlID = "cboxpasswordClose";
        }
        else
        {
            modal.PopupControlID = "pnlGeneralNote";
            modal.CancelControlID = "credboxClose";
        }


        //Bind IncetaxEmployee
        dtIE.DataSource = listIE;
        dtIE.DataBind();

        dtlIE.DataSource = listIE;
        dtlIE.DataBind();

        modal.Show();
    }

    protected void btntrakingordernumber_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        UserInformation objuser = objUserInformationRepository.GetById(Convert.ToInt64(hdnContactID.Value));
        //SentMailtoCustomer(objuser.Email, txtEnterOrderTrackingNumber.Text.Trim());
        objHeadsetRepair.UpdateOrderTrackingNumber(Convert.ToInt64(lblRepairNumber.Text.Substring(3, lblRepairNumber.Text.Length - 3)), txtEnterOrderTrackingNumber.Text.Trim());
    }

    protected void btnedit_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/HeadsetRepairCenter/AddHeadsetRepairCenter.aspx?HeadsetRepairID=" + hdndeadsetRepairID.Value + "&Contact=" + this.Contact + "&Company=" + this.Company + "&RepairNumber=" + RepairNumber + "&Status=" + this.Status);
        //Response.Redirect("~/HeadsetRepairCenter/AddHeadsetRepairCenter.aspx?HeadsetRepairID=" + hdndeadsetRepairID.Value);  
    }

    protected void lnkbtnSubmitQuote_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        objHeadsetRepair.InsertHeadsetQuote(Convert.ToInt64(hdndeadsetRepairID.Value), Convert.ToInt64(txtrepairquoteamount.Text.Trim()), txtestimatedleadtime.Text.Trim());

        //Add details in Notedetails
        NoteDetail objNotedetails = new NoteDetail();

        string Note = "your Headset Repair Quote as below \r\n\r\n";
        Note += "Repair Quote Amount : " + txtrepairquoteamount.Text.Trim() + "\r\n";
        Note += "Estimated Lead-Time : " + txtestimatedleadtime.Text.Trim() + "\r\n";

        objNotedetails.NoteFor = "HeadsetRepairCenter";
        objNotedetails.ForeignKey = Convert.ToInt64(hdndeadsetRepairID.Value);
        objNotedetails.CreateDate = DateTime.Now;
        objNotedetails.UpdateDate = DateTime.Now;
        objNotedetails.Notecontents = Note;
        objNotedetails.SpecificNoteFor = "CEActivity";         //Headset Quote by IE or Superadmin
        objNotedetails.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;   //IE or Superadmin ID
        objNotedetails.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;  //IE or Superadmin ID
        objNotedetails.ReceivedBy = hdnContactID.Value;   //CA/CE ID  

        objGenralNote.Insert(objNotedetails);
        objGenralNote.SubmitChanges();

        UserInformation objUserInformation = objUserInformationRepository.GetById(Convert.ToInt64(hdnContactID.Value));
        this.SentHeadsetQuote(objNotedetails, objUserInformation.Email, "IEActivity", Convert.ToInt64(hdnContactID.Value));

        this.GetHeadsetRepairDetails();
    }

    protected void lnkbtnVendorNotes_Click(object sender, EventArgs e)
    {
        modal.PopupControlID = "pnlVendorNote";
        modal.CancelControlID = "cboxVClose";

        modal.Show();
    }

    protected void lnkbtnVendorNotesSubmit_Click(object sender, EventArgs e)
    {
        NoteDetail objNotedetails = new NoteDetail();

        objNotedetails.NoteFor = "HeadsetRepairVendor";
        objNotedetails.ForeignKey = Convert.ToInt64(hdndeadsetRepairID.Value);
        objNotedetails.CreateDate = DateTime.Now;
        objNotedetails.UpdateDate = DateTime.Now;


        objNotedetails.Notecontents = txtVendorMessage.Text.Trim();
        objNotedetails.SpecificNoteFor = "IEToVendor";         //Note post by IE or Superadmin to Vendor
        objNotedetails.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;   //IE or Superadmin ID
        objNotedetails.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;  //IE or Superadmin ID
        objNotedetails.ReceivedBy = ddlVendor.SelectedValue;   // HeadsetRepair VendorID


        objGenralNote.Insert(objNotedetails);
        objGenralNote.SubmitChanges();

        List<long> lstIEUserID = new List<long>();

        //Senting email to HeadsetRepair Vendor
        if (chkVendorIsEmailSend.Checked && Convert.ToInt64(objNotedetails.ReceivedBy) > 0)
        {
            HeadsetRepairVendor objHeadsetvendorinformation = objHeadsetRepair.GetHeadsetRepairVendorByID(Convert.ToInt64(ddlVendor.SelectedValue));
            SendNotes(objNotedetails, objHeadsetvendorinformation.VendorEmail, "VendorToIE", Convert.ToInt64(ddlVendor.SelectedValue));
        }

        this.GetVedorNotes();
    }

    protected void lnkBtnTrackingNumber_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        modal.PopupControlID = "pnlTrackingNumber";
        modal.CancelControlID = "cboxClose";
        modal.Show();
    }

    protected void lnkbtntrackinginformation_Click(object sender, EventArgs e)
    {
        HeadsetRepairCenterMaster objheadset = new HeadsetRepairCenterMaster();

        objheadset.TrackingtoSupplierNumber = txttrackingtosupplier.Text.Trim();
        objheadset.TrackingfromSupplierNumber = txttrackingfromsupplier.Text.Trim();
        objheadset.TrackingtoClientNumber = txttrackingtoclient.Text.Trim();

        objheadset.IsUPSShippertoSupplier = chkUPStosupplier.Checked;
        objheadset.IsUPSShipperfromSupplier = chkUPSfromsupplier.Checked;
        objheadset.IsUPSShippertoClient = chkUPStoclient.Checked;

        objHeadsetRepair.InsertHeadsetTrackingInformation(Convert.ToInt64(hdndeadsetRepairID.Value), objheadset);

        this.GetHeadsetRepairDetails();
    }

    protected void ddlVendor_Selectedchanged(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        objHeadsetRepair.UpdateHeadsetRepairVenoderdetails(Convert.ToInt64(hdndeadsetRepairID.Value), Convert.ToInt64(ddlVendor.SelectedValue), Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
        this.GetHeadsetRepairDetails();
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        HeadsetRepairCenterMaster objHeadsetrepair = objHeadsetRepair.GetHeadsetRepairCenterById(Convert.ToInt64(hdndeadsetRepairID.Value));
        objHeadsetrepair.Status = Convert.ToInt64(ddlStatus.SelectedValue);
        objHeadsetRepair.UpdateHeaderRepairCenter(objHeadsetrepair);
    }

    #endregion

    #region Methods

    private void BindDropDown()
    {
        #region Bind Vendor Details
        ddlVendor.DataSource = objHeadsetRepair.GetAllHeadsetRepairVendor();
        ddlVendor.DataValueField = "VendorID";
        ddlVendor.DataTextField = "VendorContact";
        ddlVendor.DataBind();
        ddlVendor.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion

        #region Status
        ddlStatus.DataSource = objLookRep.GetByLookup("HeadsetStatus");
        ddlStatus.DataValueField = "iLookupID";
        ddlStatus.DataTextField = "sLookupName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion
    }

    protected void dtIE_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        CheckBox chk = (CheckBox)e.Item.FindControl("chkUser");
        HtmlGenericControl spnUser = (HtmlGenericControl)e.Item.FindControl("spnUser");

        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) ||
                IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
        {
            if (chk.Checked)
                spnUser.Attributes.Add("class", "custom-checkbox_checked alignleft");
            else
                spnUser.Attributes.Add("class", "custom-checkbox alignleft");
        }
        else
        {
            if (chk.Checked)
                spnUser.Attributes.Add("class", "custom-checkbox_checked redbox alignleft");
            else
                spnUser.Attributes.Add("class", "custom-checkbox redbox alignleft");
        }
    }

    private void GetHeadsetRepairDetails()
    {
        Incentex.DAL.SqlRepository.HeadsetRepairCenterCustom objHeadsetDetails = objHeadsetRepair.GetHeadsetRepaircustomByRepairNumber(this.RepairNumberDetails);

        lblCustomerInformation.Text = objHeadsetDetails.ContactName;
        lblRepairNumber.Text = "HR" + objHeadsetDetails.RepairNumber.ToString();
        lblDateSubmitted.Text = objHeadsetDetails.Date.ToString();
        lblNumberofSets.Text = objHeadsetDetails.TotalHeadset.ToString();
        lblBrand.Text = objHeadsetDetails.HeadsetBrandName;
        lblRequestRepairQuote.Text = objHeadsetDetails.Requestquotebeforerepair ? "Yes" : "No";
        ddlStatus.SelectedValue = !string.IsNullOrEmpty(objHeadsetDetails.Status) ? objHeadsetDetails.Status : "0";
        lblRequestquotebeforerepair.Text = objHeadsetDetails.Requestquotebeforerepair ? "Yes" : "No";
        txtEnterOrderTrackingNumber.Text = objHeadsetDetails.OrderTrackingNumber;
        lblrepairingsupplier.Text = objHeadsetDetails.Vendorcompany;

        lblretCompany.Text = objHeadsetDetails.retCompany;
        lblretContact.Text = objHeadsetDetails.retContactName;
        lblretAddress1.Text = objHeadsetDetails.retAddress1;
        lblretAddress2.Text = objHeadsetDetails.retAddress2;
        lblretCity.Text = objHeadsetDetails.retCity;
        lblretState.Text = objHeadsetDetails.retState;
        lblretZip.Text = objHeadsetDetails.retZip;
        lblretTel.Text = objHeadsetDetails.retTelephone;


        hypTrackingtosupplier.Text = objHeadsetDetails.trackingtosupplier;
        hypTrackingfromsupplier.Text = objHeadsetDetails.trackingfromsupplier;
        hypTrackingtoclient.Text = objHeadsetDetails.trackingtoclient;

        txttrackingtosupplier.Text = objHeadsetDetails.trackingtosupplier;
        txttrackingfromsupplier.Text = objHeadsetDetails.trackingfromsupplier;
        txttrackingtoclient.Text = objHeadsetDetails.trackingtoclient;

        if (!string.IsNullOrEmpty(objHeadsetDetails.IsUPSShippertoSupplier))
        {
            if (Convert.ToBoolean(objHeadsetDetails.IsUPSShippertoSupplier))
            {
                chkUPStosupplier.Checked = true;
                spnUPStosupplier.Attributes.Add("class", "checkout_checkbox_checked");
                hypTrackingtosupplier.NavigateUrl = "http://www.ups.com/tracking/tracking.html";
            }
            else
            {
                chkFedextosupplier.Checked = true;
                spnFedextosupplier.Attributes.Add("class", "checkout_checkbox_checked");
                hypTrackingtosupplier.NavigateUrl = "http://www.fedex.com/Tracking";
            }
        }
        if (!string.IsNullOrEmpty(objHeadsetDetails.IsUPSShipperfromSupplier))
        {
            if (Convert.ToBoolean(objHeadsetDetails.IsUPSShipperfromSupplier))
            {
                chkUPSfromsupplier.Checked = true;
                spnUPSfromsupplier.Attributes.Add("class", "checkout_checkbox_checked");
                hypTrackingfromsupplier.NavigateUrl = "http://www.ups.com/tracking/tracking.html";
            }
            else
            {
                chkFedexfromsupplier.Checked = true;
                spnFedexfromsupplier.Attributes.Add("class", "checkout_checkbox_checked");
                hypTrackingfromsupplier.NavigateUrl = "http://www.fedex.com/Tracking";
            }
        }
        if (!string.IsNullOrEmpty(objHeadsetDetails.IsUPSShippertoClient))
        {
            if (Convert.ToBoolean(objHeadsetDetails.IsUPSShippertoClient))
            {
                chkUPStoclient.Checked = true;
                spnUPStoclient.Attributes.Add("class", "checkout_checkbox_checked");
                hypTrackingtoclient.NavigateUrl = "http://www.ups.com/tracking/tracking.html";
            }
            else
            {
                chkFedextoclient.Checked = true;
                spnFedextoclient.Attributes.Add("class", "checkout_checkbox_checked");
                hypTrackingtoclient.NavigateUrl = "http://www.fedex.com/Tracking";
            }
        }


        if (!string.IsNullOrEmpty(objHeadsetDetails.IsUPSShipperfromSupplier))
        {
            chkUPSfromsupplier.Checked = Convert.ToBoolean(objHeadsetDetails.IsUPSShipperfromSupplier);
        }
        if (!string.IsNullOrEmpty(objHeadsetDetails.IsUPSShippertoClient))
        {
            chkUPStoclient.Checked = Convert.ToBoolean(objHeadsetDetails.IsUPSShippertoClient);
        }


        if (!string.IsNullOrEmpty(objHeadsetDetails.IsCustomerApprovedQuote))
        {
            //lblCustomerApprovedQuote.Text = objHeadsetDetails.IsCustomerApprovedQuote.ToLower() == "true" ? "Yes" : "No";
            imgapprovedquote.Style.Add("vertical-align", "middle");
            imgapprovedquote.ImageUrl = (objHeadsetDetails.IsCustomerApprovedQuote.ToLower() == "true") ? "~/admin/Incentex_Used_Icons/Yes.png" : "~/admin/Incentex_Used_Icons/cancel.png";
        }

        if (objHeadsetDetails.RepairQuoteAmount > 0)
        {
            txtrepairquoteamount.Text = Convert.ToString(objHeadsetDetails.RepairQuoteAmount);
            txtestimatedleadtime.Text = objHeadsetDetails.EstimatedLeadTime;
        }

        if (objHeadsetDetails.VendorID > 0)
            ddlVendor.SelectedValue = Convert.ToString(objHeadsetDetails.VendorID);

        hdndeadsetRepairID.Value = Convert.ToString(objHeadsetDetails.HeadsetRepairID);
        hdnContactID.Value = Convert.ToString(objHeadsetDetails.ContactID);

        this.GetGeneralNotes();
        this.GetVedorNotes();
    }

    private void GetGeneralNotes()
    {
        #region Display GeneralNotes
        try
        {
            List<NoteDetail> objList = objGenralNote.GetByForeignKeyId(Convert.ToInt64(hdndeadsetRepairID.Value), Incentex.DAL.Common.DAEnums.NoteForType.HeadsetRepairCenter);
            StringBuilder strNoteHistoryForIE = new StringBuilder();
            foreach (NoteDetail obj in objList)
            {
                strNoteHistoryForIE.Append(obj.Notecontents + "\n\n");
                UserInformation objUser = objUserInformationRepository.GetById(obj.CreatedBy);
                if (objUser != null)
                {
                    strNoteHistoryForIE.Append(objUser.FirstName + " " + objUser.LastName + "   ");
                }
                strNoteHistoryForIE.Append(Convert.ToDateTime(obj.CreateDate).ToString("MM/dd/yyyy"));
                strNoteHistoryForIE.Append(" @ " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n");
                strNoteHistoryForIE.Append("___________________________________________________________________________________________________\n\n");
            }

            txtCustomerNotes.Text = strNoteHistoryForIE.ToString();
        }
        catch { }
        #endregion
    }

    private void GetVedorNotes()
    {
        #region Display GeneralNotes
        try
        {
            List<NoteDetail> objList = objGenralNote.GetByForeignKeyId(Convert.ToInt64(hdndeadsetRepairID.Value), Incentex.DAL.Common.DAEnums.NoteForType.HeadsetRepairVendor);
            StringBuilder strNoteHistoryForIE = new StringBuilder();
            foreach (NoteDetail obj in objList)
            {
                strNoteHistoryForIE.Append(obj.Notecontents + "\n\n");
                UserInformation objUser = objUserInformationRepository.GetById(obj.CreatedBy);
                if (objUser != null)
                {
                    strNoteHistoryForIE.Append(objUser.FirstName + " " + objUser.LastName + "   ");
                }
                strNoteHistoryForIE.Append(Convert.ToDateTime(obj.CreateDate).ToString("MM/dd/yyyy"));
                strNoteHistoryForIE.Append(" @ " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n");
                strNoteHistoryForIE.Append("___________________________________________________________________________________________________\n\n");
            }

            txtVendorNote.Text = strNoteHistoryForIE.ToString();
        }
        catch { }
        #endregion
    }

    private void SendNotes(NoteDetail objNotedetails, string sentTo, string specificNoteFor, long UserID)
    {
        try
        {
            UserInformation objUserInformation = objUserInformationRepository.GetById(objNotedetails.CreatedBy);

            string sFrmadd = "support@world-link.us.com";
            String sToadd = sentTo;
            String sSubject = "Incentex Message - Headset Repair Number " + lblRepairNumber.Text;
            string sFrmname = IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName;


            String smtphost = Application["SMTPHOST"].ToString();
            Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            String smtpUserID = Application["SMTPUSERID"].ToString();
            String smtppassword = Application["SMTPPASSWORD"].ToString();

            String eMailTemplate = String.Empty;
            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/DocumentStorageCenter.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();


            string body = null;
            body = objNotedetails.Notecontents + "<br/><br/>";

            StringBuilder MessageBody = new StringBuilder(eMailTemplate);
            MessageBody.Replace("{MessageBody}", body);
            MessageBody.Replace("{Sender}", "Best Regards,<br/>" + sFrmname);

            //if (new ManageEmailRepository().CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderNotes)) == true)
            //{
            String sReplyToadd = CommonMails.ReplyToHeadserRepairCenter;//"ordernotes@world-link.us.com";
            String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+hn" + objNotedetails.ForeignKey + "un" + UserID + "snf" + specificNoteFor + "rb" + IncentexGlobal.CurrentMember.UserInfoID + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
            new CommonMails().SendMailWithReplyTo(objUserInformation.UserInfoID, "HeadsetRepairCenter", sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true, objNotedetails.ForeignKey);
            //}
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Sent Mail to Customer when Headsetrepaired and IE insert Tracking number
    /// </summary>
    /// <param name="EmailTo"></param>
    /// <param name="TrackingNumber"></param>
    private void SentMailtoCustomer(string EmailTo, string TrackingNumber)
    {
        try
        {
            string sFrmadd = "support@world-link.us.com";
            string sToadd = EmailTo;
            string sSubject = "Incentex Message - Headset Repair Number " + lblRepairNumber.Text;
            string sFrmname = IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName;


            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();

            String eMailTemplate = String.Empty;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/SimpleMailFormat.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            string body = null;

            body = "your most recent headset repairs have been completed and shipping to you. If you would like to track the package please click on the tracking number.<br/><br/>";
            body += "Tracking number = <a href='http://www.ups.com/tracking/tracking.html'>" + TrackingNumber + "</a><br/><br/>";
            StringBuilder MessageBody = new StringBuilder(eMailTemplate);

            MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            MessageBody.Replace("{FullName}", lblCustomerInformation.Text);
            MessageBody.Replace("{MessageBody}", body);
            MessageBody.Replace("{Sender}", "Best Regards,<br/>" + IncentexGlobal.CurrentMember.LastName);

            //Local
            //bool result = Common.SendMailWithAttachment(sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, true, true,null, "smtp.gmail.com", 587, "testsoft9@gmail.com", "tyrqm$78",null , false);

            //Live setting
            bool result = Common.SendMailWithAttachment(sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, false, true, null, smtphost, smtpport, smtpUserID, smtppassword, null, true);

        }
        catch (Exception ex)
        {
        }
    }

    private void SentHeadsetQuote(NoteDetail objNotedetails, string sentTo, string specificNoteFor, long UserID)
    {
        try
        {
            UserInformation objUserInformation = objUserInformationRepository.GetById(objNotedetails.CreatedBy);

            string sFrmadd = "support@world-link.us.com";
            String sToadd = sentTo;
            String sSubject = "Headset Repair Quote - Headset Repair Number - " + lblRepairNumber.Text;
            string sFrmname = IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName;


            String smtphost = Application["SMTPHOST"].ToString();
            Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            String smtpUserID = Application["SMTPUSERID"].ToString();
            String smtppassword = Application["SMTPPASSWORD"].ToString();

            String eMailTemplate = String.Empty;
            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/SimpleMailFormat.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();


            StringBuilder body = new StringBuilder();
            body.Append("The quote to repair all " + lblNumberofSets.Text + " headsets is " + txtrepairquoteamount.Text.Trim() + "$ and the lead-time will be " + txtestimatedleadtime.Text.Trim() + " please approve or reject the quote by clicking on the buttons below.");

            //Set Conformation Button
            String buttonText = "";
            buttonText += "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
            buttonText += "<tr>";
            buttonText += "<td width=\"100px;\">";
            buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "HeadsetRepairCenter/HeadsetQuoteStatus.aspx?UserId=" + UserID + "&HeadsetRepairID=" + objNotedetails.ForeignKey + "&Status=Approve" + "&rb=" + IncentexGlobal.CurrentMember.UserInfoID + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/approve_order_btn.png' alt='Approve Quote' border='0'/></a>";
            buttonText += "</td>";
            buttonText += "<td width=\"250px;\">";
            buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "HeadsetRepairCenter/HeadsetQuoteStatus.aspx?UserId=" + UserID + "&HeadsetRepairID=" + objNotedetails.ForeignKey + "&Status=Cancel" + "&rb=" + IncentexGlobal.CurrentMember.UserInfoID + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/cancel_order_btn.png' alt='Cancel Quote' border='0'/></a>";
            buttonText += "</td>";
            buttonText += "</tr>";
            buttonText += "</table>";

            //body.Append("<br/><input id=\"btn\" type=\"button\" style=\"padding:5px;\" value=\"Approve\" title=\"Approve\" />");
            //body.Append("&nbsp;&nbsp;<input id=\"btn\" type=\"button\" style=\"padding:5px;\" value=\"Cancel\" title=\"Cancel\" />");  
            body.Append("<br/><br/>");
            body.Append(buttonText);
            body.Append("<br/><br/>");
            StringBuilder MessageBody = new StringBuilder(eMailTemplate);

            MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            MessageBody.Replace("{FullName}", lblCustomerInformation.Text);
            MessageBody.Replace("{MessageBody}", body.ToString());
            MessageBody.Replace("{Sender}", "Best Regards,<br/>" + sFrmname);

            //if (new ManageEmailRepository().CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderNotes)) == true)
            //{
            String sReplyToadd = CommonMails.ReplyToHeadserRepairCenter;
            String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+hn" + objNotedetails.ForeignKey + "un" + UserID + "snf" + specificNoteFor + "rb" + IncentexGlobal.CurrentMember.UserInfoID + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
            new CommonMails().SendMailWithReplyTo(objUserInformation.UserInfoID, "HeadsetRepairCenter", sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true, objNotedetails.ForeignKey);
            //}
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

}
