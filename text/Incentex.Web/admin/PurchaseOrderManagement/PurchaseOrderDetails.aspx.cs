using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Data;
using System.Text;
using Incentex.BE;
using Incentex.DA;
using System.Configuration;
using System.Globalization;

public partial class PurchaseOrderDetails : PageBase
{
    #region Data Member's
    LookupRepository objLookRep = new LookupRepository();
    PurchaseOrderManagmentRepository objordermanagmentRep = new PurchaseOrderManagmentRepository();
    SupplierRepository objSupplierRepository = new SupplierRepository();
    OrderNoteRecipientDetailRepository objOrderRep = new OrderNoteRecipientDetailRepository();
    OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();
    NotesHistoryRepository objNotesHistoryRepository = new NotesHistoryRepository();
    UserInformationRepository objUserInformationRepository = new UserInformationRepository();

    long VendorID
    {
        get
        {
            if (ViewState["VendorID"] != null)
                return Convert.ToInt64(ViewState["VendorID"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["VendorID"] = value;
        }
    }
    long MasterItemID
    {
        get
        {
            if (ViewState["MasterItemID"] != null)
                return Convert.ToInt64(ViewState["MasterItemID"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["MasterItemID"] = value;
        }
    }
    /// <summary>
    /// Searching Parameter
    /// </summary>
    string OrderNumber
    {
        get
        {
            if (ViewState["OrderNumber"] != null)
                return ViewState["OrderNumber"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["OrderNumber"] = value;
        }
    }
    long PurchaseOrderID
    {
        get
        {
            if (ViewState["PurchaseOrderID"] != null)
                return Convert.ToInt64(ViewState["PurchaseOrderID"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["PurchaseOrderID"] = value;
        }
    }
    Boolean IsFillUpfollowup
    {
        get
        {
            if (ViewState["IsFillUpfollowup"] != null)
                return Convert.ToBoolean(ViewState["IsFillUpfollowup"].ToString());
            else
                return false;
        }
        set
        {
            ViewState["IsFillUpfollowup"] = value;
        }
    }
    Boolean IsTodayFollowup
    {
        get
        {
            if (ViewState["IsTodayFollowup"] != null)
                return Convert.ToBoolean(ViewState["IsTodayFollowup"].ToString());
            else
                return false;
        }
        set
        {
            ViewState["IsTodayFollowup"] = value;
        }
    }

    #endregion

    #region Event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Purchase Order Management";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }


            ((Label)Master.FindControl("lblPageHeading")).Text = "Purchase Order Management";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/PurchaseOrderManagement/PurchaseOrderList.aspx";


            if (!String.IsNullOrEmpty(Request.QueryString["VendorID"]))
                this.VendorID = Convert.ToInt64(Request.QueryString["VendorID"]);
            if (!String.IsNullOrEmpty(Request.QueryString["MasterItemID"]))
                this.MasterItemID = Convert.ToInt64(Request.QueryString["MasterItemID"]);
            if (!String.IsNullOrEmpty(Request.QueryString["OrderNumber"]))
                this.OrderNumber = Convert.ToString(Request.QueryString["OrderNumber"]);
            if (!String.IsNullOrEmpty(Request.QueryString["PurchaseOrderID"]))
                this.PurchaseOrderID = Convert.ToInt64(Request.QueryString["PurchaseOrderID"]);
            if (!String.IsNullOrEmpty(Request.QueryString["IsTodayFollowup"]))
                this.IsTodayFollowup = Convert.ToBoolean(Request.QueryString["IsTodayFollowup"]);
            if (!String.IsNullOrEmpty(Request.QueryString["IsFillUpfollowup"]))
                this.IsFillUpfollowup = Convert.ToBoolean(Request.QueryString["IsFillUpfollowup"]);


            this.BindData();

            if (IsFillUpfollowup)
            {
                //((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/PurchaseOrderManagement/PurchaseOrderList.aspx?IsTodayFollowup=" + IsTodayFollowup;
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/PurchaseOrderManagement/PurchaseOrderDetails.aspx?PurchaseOrderID=" + this.PurchaseOrderID + "&IsTodayFollowup=" + IsTodayFollowup + "&IsFillUpfollowup=true" + "&vendorID=" + VendorID + "&MasterItemID=" + MasterItemID + "&OrderNumber=" + this.OrderNumber;
                lnkbtnfollowup_Click(null, null);
                // IsFillUpfollowup = false;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/PurchaseOrderManagement/PurchaseOrderDetails.aspx?PurchaseOrderID=" + this.PurchaseOrderID + "&IsTodayFollowup=" + IsTodayFollowup + "&IsFillUpfollowup=true" + "&vendorID=" + VendorID + "&MasterItemID=" + MasterItemID + "&OrderNumber=" + this.OrderNumber;
                IsFillUpfollowup = true;
            }

            hdnPurchaseOrderID.Value = this.PurchaseOrderID.ToString();
            hdnIsTodayFollowup.Value = this.IsTodayFollowup.ToString();
            hdnIsFillUpfollowup.Value = this.IsFillUpfollowup.ToString();
            hdnSerVendorID.Value = this.VendorID.ToString();
            hdnSerMasterItemID.Value = this.MasterItemID.ToString();
            hdnSerOrderNumber.Value = this.OrderNumber.ToString();

            //if(IsTodayFollowup)
            //    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/PurchaseOrderManagement/PurchaseOrderList.aspx?IsTodayFollowup=true";


        }
        lblMsg.Text = string.Empty;
    }

    protected void lnkbtnVendorMessage_Click(object sender, EventArgs e)
    {
        hdnNoteType.Value = "Vendor Message";
        hdnOrderID.Value = "";
        dtIE.DataSource = new IncentexEmployeeRepository().GetAllEmployee();
        dtIE.DataBind();

        trIE.Visible = true;
        trIsEmailSend.Visible = true;

        chkIsEmailSend.Checked = false;
        spnIsEmailSend.Attributes.Add("class", "custom-checkbox");
        txtMessage.Text = string.Empty;
        trEmailAddress.Visible = true;

        lblEmailAddress.Text = objSupplierRepository.GetUserInformationBySupplierID(Convert.ToInt64(hdnVendorID.Value)).Email;

        mpeOrderNote.CancelControlID = "cboxClose";
        mpeOrderNote.BackgroundCssClass = "modalBackground";
        mpeOrderNote.PopupControlID = "pnlOrderNote";
        mpeOrderNote.Show();
    }

    protected void lnkbtnIncentexEmployeeMessage_Click(object sender, EventArgs e)
    {
        hdnNoteType.Value = "Incentex Employee Message";
        //hdnOrderID.Value = e.CommandArgument.ToString();
        dtIE.DataSource = new IncentexEmployeeRepository().GetAllEmployee();
        dtIE.DataBind();

        trIE.Visible = true;
        trIsEmailSend.Visible = true;

        txtMessage.Text = string.Empty;
        chkIsEmailSend.Checked = false;
        spnIsEmailSend.Attributes.Add("class", "custom-checkbox");

        trEmailAddress.Visible = false;
        lblEmailAddress.Text = string.Empty;

        mpeOrderNote.CancelControlID = "cboxClose";
        mpeOrderNote.BackgroundCssClass = "modalBackground";
        mpeOrderNote.PopupControlID = "pnlOrderNote";
        mpeOrderNote.Show();
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        objordermanagmentRep.UpdateOrderStatus(this.PurchaseOrderID, Convert.ToInt64(ddlStatus.SelectedValue));
        lblMsg.Text = "Current status changed successfully.";
    }

    protected void btnViewPurchaseOrder_Click(object sender, EventArgs e)
    {
        string filepath = Server.MapPath("~/UploadedImages/PurchaseOrderManagement/") + hdnFileName.Value;
        Common.DownloadFile(filepath, hdnOriginalFileName.Value);
    }

    protected void lnkbtnSubmitNote_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hdnNoteType.Value))
        {
            String NoteType = hdnNoteType.Value;
            if (NoteType == "Vendor Message" && txtMessage.Text.Trim() != "")
            {
                //NoteHistory for Supplier ORder
                String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.SupplierPurchaseOrder);

                NoteDetail objNoteDetail = new NoteDetail();
                objNoteDetail.Notecontents = txtMessage.Text.Trim();
                objNoteDetail.NoteFor = strNoteFor;
                objNoteDetail.ForeignKey = Convert.ToInt64(hdnVendorID.Value);
                objNoteDetail.SpecificNoteFor = this.PurchaseOrderID.ToString();
                objNoteDetail.CreateDate = System.DateTime.Now;
                objNoteDetail.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objNoteDetail.UpdateDate = System.DateTime.Now;
                objNoteDetail.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

                objNotesHistoryRepository.Insert(objNoteDetail);
                objNotesHistoryRepository.SubmitChanges();

                //Write code for sending email to selected IE
                if (chkIsEmailSend.Checked)
                {
                    foreach (DataListItem item in dtIE.Items)
                    {
                        if (((CheckBox)item.FindControl("chkUser")).Checked == true)
                        {
                            HiddenField hdnUserID = ((HiddenField)item.FindControl("hdnUserID"));
                            objOrderRep.InsertIEListIntoOrderNoteRecipient(this.PurchaseOrderID, objNoteDetail.NoteID, Convert.ToInt64(hdnUserID.Value), IncentexGlobal.CurrentMember.UserInfoID);
                            SendNotes(this.PurchaseOrderID, Convert.ToInt64(hdnUserID.Value), 1, objNoteDetail.NoteID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin), objSupplierRepository.GetById(Convert.ToInt64(hdnVendorID.Value)).UserInfoID);
                            //SendNotes(OrderID, 1092, 2);
                        }
                    }

                    SendNotes(this.PurchaseOrderID, objSupplierRepository.GetById(Convert.ToInt64(hdnVendorID.Value)).UserInfoID, 1, objNoteDetail.NoteID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier), IncentexGlobal.CurrentMember.UserInfoID);
                    //SendNotes(OrderID, 1092, 1);
                }
            }
            else if (NoteType == "Incentex Employee Message" && txtMessage.Text.Trim() != "")
            {
                string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.PurchaseOrderDetailsIEs);

                NoteDetail objNoteDetail = new NoteDetail();
                objNoteDetail.Notecontents = txtMessage.Text.Trim();
                objNoteDetail.NoteFor = strNoteFor;
                objNoteDetail.ForeignKey = this.PurchaseOrderID;
                objNoteDetail.SpecificNoteFor = "POIEInternalNotes";
                objNoteDetail.CreateDate = System.DateTime.Now;
                objNoteDetail.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objNoteDetail.UpdateDate = System.DateTime.Now;
                objNoteDetail.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

                objNotesHistoryRepository.Insert(objNoteDetail);
                objNotesHistoryRepository.SubmitChanges();

                //write code for sending email to IE based on checkmark selection
                if (chkIsEmailSend.Checked)
                {
                    foreach (DataListItem item in dtIE.Items)
                    {
                        if (((CheckBox)item.FindControl("chkUser")).Checked == true)
                        {
                            HiddenField hdnUserID = ((HiddenField)item.FindControl("hdnUserID"));
                            objOrderRep.InsertIEListIntoOrderNoteRecipient(this.PurchaseOrderID, objNoteDetail.NoteID, Convert.ToInt64(hdnUserID.Value), IncentexGlobal.CurrentMember.UserInfoID);
                            SendNotes(this.PurchaseOrderID, Convert.ToInt64(hdnUserID.Value), 2, objNoteDetail.NoteID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin), objNoteDetail.CreatedBy.Value);
                            //SendNotes(OrderID, 1092, 2);
                        }
                    }
                }
            }
        }
        BindNotes();
    }

    protected void txtDatePromised_TextChanged(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        PurchaseOrderMaster objpurchaseorder = objordermanagmentRep.GetPurchaseOrderByID(this.PurchaseOrderID);
        String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.PurchaseOrderDetailsIEs);

        if (objpurchaseorder != null)
        {
            if (!string.IsNullOrEmpty(txtDatePromised.Text.Trim()))
            {
                objpurchaseorder.DeliveryDate = DateTime.ParseExact(txtDatePromised.Text.Trim(),
                                                      Common.DateFormats,
                                                      CultureInfo.InvariantCulture,
                                                      DateTimeStyles.AssumeLocal);


                objpurchaseorder.UpdatedDate = System.DateTime.Now;
                objpurchaseorder.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objordermanagmentRep.SubmitChanges();



                Logaction("Changed Confirmed Delivery Date to : " + txtDatePromised.Text.Trim() + ".", "POIEInternalNotes", strNoteFor);
            }
            else
            {
                objpurchaseorder.DeliveryDate = null;
                objpurchaseorder.UpdatedDate = System.DateTime.Now;
                objpurchaseorder.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objordermanagmentRep.SubmitChanges();

                Logaction("Removed Confirmed Delivery Date.", "POIEInternalNotes", strNoteFor);
            }

            lblMsg.Text = "Confirmed delivery date changed successfully.";
            this.BindNotes();
        }
    }

    protected void lnkbtnfollowup_Click(object sender, EventArgs e)
    {
        txtconfirmdedate.Text = string.Empty;
        ddlorderstatus.SelectedIndex = 0;
        txtnextfollowupdate.Text = string.Empty;
        chkshoonTomorrow.Checked = false;

        mpeOrderNote.CancelControlID = "cboxpasswordClose";
        mpeOrderNote.BackgroundCssClass = "modalBackground";
        mpeOrderNote.PopupControlID = "pnlfollowup";
        mpeOrderNote.Show();
    }

    protected void lnkbtnSubmitfollowup_Click(object sender, EventArgs e)
    {
        PurchaseOrderFollowUp objfollowup = new PurchaseOrderFollowUp();
        PurchaseOrderMaster objpurchaseoreder = objordermanagmentRep.GetPurchaseOrderByID(this.PurchaseOrderID);

        objfollowup.PurchaseOrderID = this.PurchaseOrderID;
        objfollowup.LastFollowUpDate = DateTime.ParseExact(txtnextfollowupdate.Text.Trim(),
                                                      Common.DateFormats,
                                                      CultureInfo.InvariantCulture,
                                                      DateTimeStyles.AssumeLocal);
        objfollowup.PriorStatus = objordermanagmentRep.GetFollowupPriorStatusBy(this.PurchaseOrderID);
        objfollowup.CurrentStatus = Convert.ToInt64(ddlorderstatus.SelectedValue);
        objfollowup.ShowonTomorrows = chkshoonTomorrow.Checked;
        objfollowup.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objfollowup.CreatedDate = System.DateTime.Now;

        objOrderConfirmationRepository.Insert(objfollowup);
        objOrderConfirmationRepository.SubmitChanges();


        //Update Purchase Order Delivery and Status
        objpurchaseoreder.DeliveryDate = DateTime.ParseExact(txtconfirmdedate.Text.Trim(),
                                                      Common.DateFormats,
                                                      CultureInfo.InvariantCulture,
                                                      DateTimeStyles.AssumeLocal);
        objpurchaseoreder.StatusID = Convert.ToInt64(ddlorderstatus.SelectedValue);

        ddlStatus.SelectedValue = ddlorderstatus.SelectedValue;
        txtDatePromised.Text = objpurchaseoreder.DeliveryDate.Value.ToString("MM/dd/yyyy");
        objordermanagmentRep.SubmitChanges();

        lblMsg.Text = "FollowUp uploded successfully.";
        this.BindFollowUpTimeLine();

        ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/PurchaseOrderManagement/PurchaseOrderList.aspx?IsTodayFollowup=" + IsTodayFollowup + "&vendorID=" + VendorID + "&MasterItemID=" + MasterItemID + "&OrderNumber=" + this.OrderNumber;
        IsFillUpfollowup = false;
        hdnIsFillUpfollowup.Value = IsFillUpfollowup.ToString();
    }

    #endregion

    #region Method

    protected void BindData()
    {
        PurchaseOrderDetailCustom objorderdetails = objordermanagmentRep.GetPurchaseOrderDetails(this.PurchaseOrderID);

        lblvendorname.Text = objorderdetails.VendorName;
        lblvendorcompanyname.Text = objorderdetails.VendorCompany;

        if (objorderdetails.DeliveryDate.HasValue)
            txtDatePromised.Text = objorderdetails.DeliveryDate.Value.ToString("MM/dd/yyyy");

        lblOrderNumber.Text = objorderdetails.PurchaseOrderNumber;
        hdnFileName.Value = objorderdetails.FileName;
        hdnOriginalFileName.Value = objorderdetails.OriginalFileName;
        hdnVendorID.Value = Convert.ToString(objorderdetails.VendorID);

        if (string.IsNullOrEmpty(objorderdetails.FileName))
            btnViewPurchaseOrder.Visible = false;

        #region Status
        ddlStatus.DataSource = objLookRep.GetByLookup("Purchase Order Status");
        ddlStatus.DataValueField = "iLookupID";
        ddlStatus.DataTextField = "sLookupName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-Select-", "0"));

        ddlorderstatus.DataSource = objLookRep.GetByLookup("Purchase Order Status");
        ddlorderstatus.DataValueField = "iLookupID";
        ddlorderstatus.DataTextField = "sLookupName";
        ddlorderstatus.DataBind();
        ddlorderstatus.Items.Insert(0, new ListItem("-Select-", "0"));


        if (objorderdetails.Status.HasValue)
            ddlStatus.SelectedValue = Convert.ToString(objorderdetails.Status.Value);
        #endregion

        #region Listing of Item Sizes
        List<ProductItemCustom> oad = objordermanagmentRep.ProductItemDetails(objorderdetails.PurchaseOrderID);

        DataView myDataView = new DataView();
        DataTable dataTable = Common.ListToDataTable(oad.Where(t => t.ItemNumberStatusID == 135).ToList());    //Status Active 
        myDataView = dataTable.DefaultView;

        if (this.ViewState["SortExp"] != null)
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();

        gvItemDetails.DataSource = myDataView;
        gvItemDetails.DataBind();
        #endregion

        this.BindFollowUpTimeLine();
        this.BindNotes();
    }

    private void SendNotes(Int64 OrderID, Int64 ToUserInfoID, Int64 NoteType, Int64 NoteID, Int64 recipienttype, long ReceivedBY)
    {
        try
        {
            //Order objOrder = objOrderConfirmationRepository.GetByOrderID(OrderID);
            UserInformation objUserInformation = objUserInformationRepository.GetById(ToUserInfoID);

            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;

            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "IE NoteHistory";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();


                String sToadd = objUserInformation.Email;

                String sSubject = "Incentex Message - Purchase Order Number " + lblOrderNumber.Text;
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                //StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                //messagebody.Replace("{OrderNo}", lblOrderNumber.Text);
                //messagebody.Replace("{Commentssection}", txtMessage.Text.Trim());
                //messagebody.Replace("{fullname}", objUserInformation.FirstName);
                //messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                string messagebody = txtMessage.Text.Trim();

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                if (new ManageEmailRepository().CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderNotes)) == true)
                {
                    String sReplyToadd = CommonMails.ReplayTopurchaseordermanagement;
                    //QueryString flow = on__un__ad__rb__nt__ni_rtp__en
                    //on: order number
                    //un: user number(userid)
                    //ad: admin id(if applicable else 0)
                    //rb: Recevied by id 
                    //nt: note type
                    //ni: note id
                    //rtp: recipient type
                    //en: end of the string

                    String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+on" + this.PurchaseOrderID + "un" + objUserInformation.UserInfoID + "ad0" + "rb" + ReceivedBY + "nt" + NoteType + "ni" + NoteID + "rtp" + recipienttype + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                    new CommonMails().SendMailWithReplyTo(objUserInformation.UserInfoID, "Purchase Order Notes", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true, OrderID);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void BindNotes()
    {
        #region Display internal notes for IE
        try
        {
            // TextBox txtIEInternalNotes = ((TextBox)(repeaterItem.FindControl("txtIEInternalNotes")));
            List<NoteDetail> objList = objNotesHistoryRepository.GetNotesForIEPerOrderId(this.PurchaseOrderID, Incentex.DAL.Common.DAEnums.NoteForType.PurchaseOrderDetailsIEs, "POIEInternalNotes", 0);
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

            ////Append note who placed order with datetime as per ken discussed
            //for (int i = 0; i < orderReferenceResult.Count; i++)
            //{
            //    strNoteHistoryForIE.Append("Drop-Ship Orders Placed \n\n");
            //    strNoteHistoryForIE.Append(orderReferenceResult[i].IEName + "   ");
            //    strNoteHistoryForIE.Append(Convert.ToDateTime(orderReferenceResult[i].UpdatedDate).ToString("MM/dd/yyyy"));
            //    strNoteHistoryForIE.Append(" @ " + Convert.ToDateTime(orderReferenceResult[i].UpdatedDate).ToShortTimeString() + "\n");
            //    strNoteHistoryForIE.Append("___________________________________________________________________________________________________\n\n");
            //}

            txtIEInternalNotes.Text = strNoteHistoryForIE.ToString();
        }
        catch (Exception ex)
        { }
        #endregion

        #region Display notes sent by IE to Vendor
        try
        {
            List<NoteDetail> objList = objNotesHistoryRepository.GetNotesForSuppliersPerOrderId(Incentex.DAL.Common.DAEnums.NoteForType.SupplierPurchaseOrder, this.PurchaseOrderID.ToString(), 0);
            StringBuilder strNoteHistoryForSupplier = new StringBuilder();
            foreach (NoteDetail obj in objList)
            {
                strNoteHistoryForSupplier.Append(obj.Notecontents + "\n\n");
                UserInformation objUser = objUserInformationRepository.GetById(obj.CreatedBy);

                if (objUser != null)
                    strNoteHistoryForSupplier.Append(objUser.FirstName + " " + objUser.LastName + "   ");

                strNoteHistoryForSupplier.Append(Convert.ToDateTime(obj.CreateDate).ToString("MM/dd/yyyy"));
                strNoteHistoryForSupplier.Append(" @ " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n");
                strNoteHistoryForSupplier.Append("___________________________________________________________________________________________________\n\n");
            }
            txtNotesForVendor.Text = strNoteHistoryForSupplier.ToString();
        }
        catch { }
        #endregion
    }

    private void BindFollowUpTimeLine()
    {
        List<PurchaseOrderFollowUpCustom> objFollowUp = objordermanagmentRep.GetFollowUpTimeline(this.PurchaseOrderID);

        DataView myDataView = new DataView();
        DataTable dataTable = Common.ListToDataTable(objFollowUp);
        myDataView = dataTable.DefaultView;

        if (this.ViewState["SortExp"] != null)
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();

        gvFollowUpTimeline.DataSource = myDataView;
        gvFollowUpTimeline.DataBind();
    }

    private void Logaction(string Notecontents, string SpecificNoteFor, string strNoteFor)
    {
        NoteDetail objNoteDetail = new NoteDetail();


        objNoteDetail.Notecontents = Notecontents;
        objNoteDetail.NoteFor = strNoteFor;
        objNoteDetail.ForeignKey = this.PurchaseOrderID;
        objNoteDetail.SpecificNoteFor = SpecificNoteFor;
        objNoteDetail.CreateDate = System.DateTime.Now;
        objNoteDetail.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objNoteDetail.UpdateDate = System.DateTime.Now;
        objNoteDetail.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

        objNotesHistoryRepository.Insert(objNoteDetail);
        objNotesHistoryRepository.SubmitChanges();
    }

    #endregion

}
