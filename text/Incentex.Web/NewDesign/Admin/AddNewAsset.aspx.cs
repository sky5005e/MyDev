/*
 * I have used Repeater for the Multiple Radiobutton as there is custom layout to use Radiobutton.
 * Kindly take a note of it and you can change it to Radiobutton if you have alternate solution to bind wrap the label and span with each radiobutton
 */

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
using System.Collections.Generic;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using ASP;

using System.Globalization;


public partial class Admin_AddNewAsset : PageBase
{

    #region Page Variables and Properties

    enum GridID
    {
        Invoice = 1,
        Warranty = 2,
        Claim = 3
    }

    Int64 EquipmentMasterID
    {
        get
        {
            return Convert.ToInt64(ViewState["EquipmentMasterID"]);
        }
        set
        {
            ViewState["EquipmentMasterID"] = value;
        }
    }

    Int64 CompanyID
    {
        get
        {
            return Convert.ToInt64(ViewState["CompanyID"]);
        }
        set
        {
            ViewState["CompanyID"] = value;
        }
    }

    Int64 EquipmentTypeID
    {
        get
        {
            return Convert.ToInt64(ViewState["EquipmentTypeID"]);
        }
        set
        {
            ViewState["EquipmentTypeID"] = value;
        }
    }

    Boolean IsFlagAsset
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsFlagAsset"]);
        }
        set
        {
            ViewState["IsFlagAsset"] = value;
        }
    }

    Decimal InvoiceTotalAmount
    {
        get
        {
            return Convert.ToDecimal(ViewState["InvoiceTotalAmount"]);
        }
        set
        {
            ViewState["InvoiceTotalAmount"] = value;
        }
    }

    public String InvoiceSortExp
    {
        get
        {
            return Convert.ToString(Session["InvoiceSortExp"]);
        }
        set
        {
            Session["InvoiceSortExp"] = value;
        }
    }

    public String InvoiceSortOrder
    {
        get
        {
            return Convert.ToString(Session["InvoiceSortOrder"]);
        }
        set
        {
            Session["InvoiceSortOrder"] = value;
        }
    }

    public String InvoicePagingStatus
    {
        get
        {
            if (Session["InvoicePagingStatus"] == null)
                return "VIEW ALL";
            return Convert.ToString(Session["InvoicePagingStatus"]);
        }
        set
        {
            Session["InvoicePagingStatus"] = value;
        }
    }

    public String WarrantyPagingStatus
    {
        get
        {
            if (Session["WarrantyPagingStatus"] == null)
                return "VIEW ALL";
            return Convert.ToString(Session["WarrantyPagingStatus"]);
        }
        set
        {
            Session["WarrantyPagingStatus"] = value;
        }
    }

    public String ClaimPagingStatus
    {
        get
        {
            if (Session["ClaimPagingStatus"] == null)
                return "VIEW ALL";
            return Convert.ToString(Session["ClaimPagingStatus"]);
        }
        set
        {
            Session["ClaimPagingStatus"] = value;
        }
    }

    string CurrModule = "Asset Management";
    string CurrSubMenu = "Assets";

    #endregion

    #region Event Handlers

    protected void lnkbtnDownload_Click(object sender, EventArgs e)
    {
        LinkButton lnkbtn = (LinkButton)sender;
        String filepath = Server.MapPath("~/" + lnkbtn.CommandArgument);
        String strFileName = "MyFile";
        if (!String.IsNullOrEmpty(lnkbtn.CommandName))
            strFileName = lnkbtn.CommandName;
        Common.DownloadFile(filepath, strFileName);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!String.IsNullOrEmpty(Convert.ToString(Session["Event"])))
            {
                //NOTE:
                //Here session is used to solve the issue of Custom Add Field in the view state
                if (Convert.ToString(Session["Event"]) == "Added")
                    this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Field added successfully.');", true);
                else if (Convert.ToString(Session["Event"]) == "Deleted")
                    this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Field Deleted Successfully.'); ", true);
            }

            if (String.IsNullOrEmpty(Convert.ToString(Session["Event"])))
                base.StartStopwatch();

            // Top - Link 
            base.SetTopLinkScript("admin-link");
            if (!String.IsNullOrEmpty(Request.QueryString["eqpID"]) && Request.QueryString["eqpID"] != "0")
            {
                flag_li.Visible = true;
                ul_basicfields.Visible = true;
                this.EquipmentMasterID = Convert.ToInt64(Request.QueryString["eqpID"]);
                //if (String.IsNullOrEmpty(Convert.ToString(Session["Event"])))
                // BindBasicDropDown();
                PopulateData(EquipmentMasterID);
                //else
                BindFieldTypes();
                //                BindBasicFields();
            }
            else
            {
                flag_li.Visible = false;
                ul_basicfields.Visible = false;
                BindFieldTypes();
            }

            txtAccountingYearlyDepreciation.Attributes.Add("readonly", "readonly");
            txtAccountingTotalMonthlyCost.Attributes.Add("readonly", "readonly");

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                this.CompanyID = Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId);

            lnkbtnBasic.Attributes.Add("onclick", "return GeneralConfirmationMsgForRecordSave('Do you want to save your changes?','" + lnkbtnBasic.ClientID + "',event)");
            lnkbtnAccounting.Attributes.Add("onclick", "return GeneralConfirmationMsgForRecordSave('Do you want to save your changes?','" + lnkbtnAccounting.ClientID + "',event)");
            lnkbtnWarranty.Attributes.Add("onclick", "return GeneralConfirmationMsgForRecordSave('Do you want to save your changes?','" + lnkbtnWarranty.ClientID + "',event)");
            lnkbtnSpecs.Attributes.Add("onclick", "return GeneralConfirmationMsgForRecordSave('Do you want to save your changes?','" + lnkbtnSpecs.ClientID + "',event)");
            lnkbtnRegistration.Attributes.Add("onclick", "return GeneralConfirmationMsgForRecordSave('Do you want to save your changes?','" + lnkbtnRegistration.ClientID + "',event)");
            lnkbtnHistory.Attributes.Add("onclick", "return GeneralConfirmationMsgForRecordSave('Do you want to save your changes?','" + lnkbtnHistory.ClientID + "',event)");

            if (String.IsNullOrEmpty(Convert.ToString(Session["Event"])))
                base.EndStopwatch("Page Load", CurrModule, CurrSubMenu);
            else
            {
                Session["Event"] = null;
                mvAddNewAsset.ActiveViewIndex = Convert.ToInt32(Session["ActiveViewIndex"]);
                Session["ActiveViewIndex"] = null;
            }

        }

        if ( (!String.IsNullOrEmpty(Request.QueryString["eqpID"]) && Request.QueryString["eqpID"] != "0") || this.EquipmentMasterID > 0)
        {
            if (mvAddNewAsset.ActiveViewIndex == 0 && ul_basicfields.Controls.Count <= 1 && Request.Form["__EventTarget"] != "ctl00$ContentPlaceHolder1$lnkbtnAccounting")
                BindCustomFields("Basic", true);
            else if (mvAddNewAsset.ActiveViewIndex == 1 && ul_AccountingFields.Controls.Count <= 1 && Request.Form["__EventTarget"] != "ctl00$ContentPlaceHolder1$lnkbtnWarranty")
                BindCustomFields("Accounting", true);
            else if (mvAddNewAsset.ActiveViewIndex == 3 && ul_SpecificationsFields.Controls.Count <= 1 && Request.Form["__EventTarget"] != "ctl00$ContentPlaceHolder1$lnkbtnRegistration")
                BindCustomFields("Specifications", true);
        }
    }

    /// <summary>
    /// Handles the Click event when the Tab is click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void lnkbtnChangeTab_Click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();
            if (EquipmentMasterID > 0)
            {
                LinkButton lnkbtn = (LinkButton)sender;
                SetActiveClassToLinkButtonTabs(lnkbtn, mvAddNewAsset);
                //Reset the change value
                string strActiveDiv = string.Empty;
                if (lnkbtn.Attributes["data-divID"] != null)
                    strActiveDiv = Convert.ToString(lnkbtn.Attributes["data-divID"]);
                ResetCtrlInfoChangedValue(strActiveDiv);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        finally { base.EndStopwatch("Change Menu parent Tab", CurrModule, CurrSubMenu); }
    }

    protected void lnkbtnHistoryChangeTab_Click(object sender, EventArgs e)
    {
        try
        {
            if (EquipmentMasterID > 0)
            {
                LinkButton lnkbtn = (LinkButton)sender;
                lnkbtnHistory.Attributes.Add("class", "active");
                SetActiveClassToLinkButtonTabs(lnkbtn, mvHistory);
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    protected void mvAddNewAsset_ActiveViewChanged(object sender, EventArgs e)
    {
        try
        {
            if ((mvAddNewAsset.ActiveViewIndex > 0 && EquipmentMasterID == 0) || mvAddNewAsset.ActiveViewIndex == 0)
            {
                //Basic Tab
                BindBasicDropDown();
            }
            HtmlInputHidden hdnSaveButtonToTrigger = (HtmlInputHidden)this.Master.FindControl("hdnSaveButtonToTrigger");
            if (hdnSaveButtonToTrigger != null)
            {
                if ((mvAddNewAsset.ActiveViewIndex > 0 && EquipmentMasterID == 0) || mvAddNewAsset.ActiveViewIndex == 0)
                    hdnSaveButtonToTrigger.Value = btnBasicSave.ClientID;
                else if (mvAddNewAsset.ActiveViewIndex == 1)
                    hdnSaveButtonToTrigger.Value = lnkbtnAccountingSave.ClientID;
                else if (mvAddNewAsset.ActiveViewIndex == 3)
                    hdnSaveButtonToTrigger.Value = lnkbtnSaveSpecifications.ClientID;
            }
            if (String.IsNullOrEmpty(Convert.ToString(Session["Event"])))
            {

                if ((mvAddNewAsset.ActiveViewIndex > 0 && EquipmentMasterID == 0) || mvAddNewAsset.ActiveViewIndex == 0)
                {
                    //Basic Tab
                    //BindBasicDropDown();
                    SetActiveClassToLinkButtonTabs(lnkbtnBasic, mvAddNewAsset);
                    if (this.EquipmentMasterID > 0 || !String.IsNullOrEmpty(Request.QueryString["eqpID"]) && Request.QueryString["eqpID"] != "0")
                    {
                        if (this.EquipmentMasterID == 0)
                            this.EquipmentMasterID = Convert.ToInt64(Request.QueryString["eqpID"]);
                        flag_li.Visible = true;
                        PopulateData(EquipmentMasterID);
                        if (ul_basicfields.Controls.Count <= 1)
                        {
                            BindCustomFields("Basic", false);
                        }
                    }
                    if (hdnSaveButtonToTrigger != null)
                    {
                        hdnSaveButtonToTrigger.Value = btnBasicSave.ClientID;
                    }
                    //btnBasicCancel.Attributes.Add("onclick", "RedirectURL('Assets.aspx?se=add','Do you want to save your changes',' ',true,event);");
                    //BindBasicTabNotes();
                }
                else if (mvAddNewAsset.ActiveViewIndex == 1)
                {
                    //Accouting Tab
                    BindAccountingTabControls();
                    SetActiveClassToLinkButtonTabs(lnkbtnAccounting);
                    if (hdnSaveButtonToTrigger != null)
                    {
                        hdnSaveButtonToTrigger.Value = lnkbtnAccountingSave.ClientID;
                    }
                    if (ul_AccountingFields.Controls.Count <= 1)
                    {
                        BindCustomFields("Accounting", false);
                    }
                    a_invoicepost.Attributes.Add("onclick", "ShowInvoicePopUp('Do you want to save your changes?','" + a_invoicepost.ClientID + "',event,false);");
                    a_invoicepost.Attributes.Add("ontouchend", "ShowInvoicePopUp('Do you want to save your changes?','" + a_invoicepost.ClientID + "',event,false);");
                    //Back Buttons
                    lnkbtnAccountingBack.Attributes.Add("onclick", "return GeneralConfirmationMsgForRecordSave('Do you want to save your changes?','" + lnkbtnAccountingBack.ClientID + "',event)");
                }
                else if (mvAddNewAsset.ActiveViewIndex == 2)
                {
                    //Warranty Tab
                    BindWarrantyTabControls();
                    SetActiveClassToLinkButtonTabs(lnkbtnWarranty);
                }
                else if (mvAddNewAsset.ActiveViewIndex == 3)
                {
                    //Specification Tab
                    SetActiveClassToLinkButtonTabs(lnkbtnSpecs);
                    if (ul_SpecificationsFields.Controls.Count <= 1)
                    {
                        BindCustomFields("Specifications", false);
                    }
                    BindFieldTypes();
                    BindSpecifiationFiles();
                }
                else if (mvAddNewAsset.ActiveViewIndex == 5)
                {
                    BindHistoryNotes();
                    SetActiveClassToLinkButtonTabs(lnkbtnNotes, mvHistory);
                }
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlLookUpDropDown_SaveNewOptionAttempted(object sender, EventArgs e)
    {
        //if (mvAddNewAsset.ActiveViewIndex == 3)
        //    BindSpecsFields();
        try
        {
            CustomDropDown ddlLookUpDropDown = (CustomDropDown)sender;

            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            if (cph != null)
            {
                HtmlGenericControl Parent_span = (HtmlGenericControl)cph.FindControl(ddlLookUpDropDown.Parent.ID);
                if (Parent_span != null)
                {
                    Parent_span.Attributes.Add("class", ddlLookUpDropDown.ParentSpanClassToRemove);
                    if (Parent_span.Attributes["class"].ToLower().Contains("popup"))
                        ShowPopUp(Parent_span);
                }
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "ScrollToTag('" + ddlLookUpDropDown.Parent.ClientID + "',false);", true);
            }
            if (ddlLookUpDropDown.Text.Trim() != "")
            {
                var objList = objAssetMgtRepository.InsertAndGetAllDropDownOption(ddlLookUpDropDown.GroupName, CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(ddlLookUpDropDown.Text), ddlLookUpDropDown.Module.ToLower());
                if (objList.Count > 0 && objList.FirstOrDefault(x => x.InsertTransactionStatus.ToLower() == "already exists") != null)
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Option already exists.');", true);
                BindCustomDropDown(ddlLookUpDropDown, objList.Where(x => x.DataValueField != "").OrderBy(x => x.DataValueField).ToList(), "DataTextField", "DataValueField", "- " + ddlLookUpDropDown.DefaultOptionText + " -", "-1");
                ddlLookUpDropDown.Items.FindByText(CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(ddlLookUpDropDown.Text)).Selected = true;
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlCommonDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (mvAddNewAsset.ActiveViewIndex == 3)
        //    BindSpecsFields();
        try
        {
            if (String.IsNullOrEmpty(Request.Form["__EventTarget"]) || Convert.ToString(Request.Form["__EventTarget"]).ToLower().Contains("ddldropdown"))
            {
                CustomDropDown ddlCommonDropDown = (CustomDropDown)sender;
                ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
                if (cph != null)
                {
                    HtmlGenericControl Parent_span = (HtmlGenericControl)cph.FindControl(ddlCommonDropDown.Parent.ID);
                    if (Parent_span != null && Parent_span.Attributes["class"].ToLower().Contains("popup"))
                        ShowPopUp(Parent_span);

                    if (Parent_span != null && !ddlCommonDropDown.IsEditing && ddlCommonDropDown.SelectedValue == "-1")
                    {
                        Parent_span.Attributes.Remove("class");
                        Parent_span.Attributes.Add("class", "popup");
                    }

                    //this.Page.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "ScrollToTag('" + ddlCommonDropDown.Parent.ClientID + "',false);", true);

                    //Code for this Page Only - Static Condition
                    if (ddlCommonDropDown.Parent.ID == "accounting_jobcode_span")
                        BindSubJobCode(Convert.ToInt64(ddlCommonDropDown.SelectedValue));
                }
            }

        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnBack_Click(object sender, EventArgs e)
    {
        if (mvAddNewAsset.ActiveViewIndex != 0)
            mvAddNewAsset.ActiveViewIndex = mvAddNewAsset.ActiveViewIndex - 1;
    }

    protected void lnkbtnSendMail_Click(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(hfAssetFiles.Value.Trim()) && !String.IsNullOrEmpty(hfFileID.Value.Trim()))
            {
                LinkButton lnkbtn = (LinkButton)sender;
                String strToAddress = txtToEmail.Text.Trim();
                String strBody = txtMailMessage.Value.Trim();
                String strFilePath = Server.MapPath("~/" + hfAssetFiles.Value.Trim());

                String smtphost = CommonMails.SMTPHost;
                Int32 smtpport = CommonMails.SMTPPort;
                String smtpUserID = CommonMails.UserName;
                String smtppassword = CommonMails.Password;
                // String sReplyToadd =IncentexGlobal.CurrentMember.LoginEmail;
                String ReplyToAddress = CommonMails.ReplyToAssetManagement; //"replytoGSE@world-link.us.com";
                //String ReplyToAddress = "prashanth.kankhara@indianic.com";
                String strNoteFor = String.Empty;
                if (mvAddNewAsset.ActiveViewIndex + 1 == 1)
                    strNoteFor = "Basic";
                else if (mvAddNewAsset.ActiveViewIndex + 1 == 2)
                    strNoteFor = "Accounting";
                else if (mvAddNewAsset.ActiveViewIndex + 1 == 3)
                    strNoteFor = "Warranty";
                else if (mvAddNewAsset.ActiveViewIndex + 1 == 4)
                    strNoteFor = "Specifications";


                AssetMgtRepository objAssetMgtRepos = new AssetMgtRepository();
                InsertNotesForSendMailResult objInsertNotesForSendMailResult = objAssetMgtRepos.InsertIntoNotesForSendEmail(strNoteFor, hfFileFor.Value.Trim(), this.EquipmentMasterID, hfFileID.Value.Trim(), strBody, strToAddress, "AssetManagement", IncentexGlobal.CurrentMember.UserInfoID);

                if (objInsertNotesForSendMailResult != null)
                {
                    String strSpecificNoteFor = String.Empty;
                    if (hfFileFor.Value.Trim() == Convert.ToString(Incentex.DAL.Common.DAEnums.NoteForType.LeaseAsset))
                        strSpecificNoteFor = "1";
                    else if (hfFileFor.Value.Trim() == Convert.ToString(Incentex.DAL.Common.DAEnums.NoteForType.PurchaseAsset))
                        strSpecificNoteFor = "2";
                    else if (hfFileFor.Value.Trim() == Convert.ToString(Incentex.DAL.Common.DAEnums.NoteForType.AssetManuals))
                        strSpecificNoteFor = "3";
                    else if (hfFileFor.Value.Trim() == Convert.ToString(Incentex.DAL.Common.DAEnums.NoteForType.AssetImagesVideos))
                        strSpecificNoteFor = "4";


                    CommonMails objCommonMails = new CommonMails();
                    //new CommonMails().SendMailWithReplyTo(objUserInformation.UserInfoID, "Order Notes", sFrmadd, sToadd, sSubject, Convert.ToString(messagebody), sFrmname, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Convert.ToString(Common.SMTPPort), "Normal", Common.SSL, true, OrderID);
                    //QueryString flow = an__un__rt__ad__cb__nf__sf__nx__fk__en
                    //an: Asset ID
                    //un: user number(userid)
                    //rt: recipient type: ax for annonymous user and au for admin
                    //cb: created by id 
                    //nf: note for Options= 1: Basic,2:Accounting, 3: Specifications,4: Specifications
                    //sf: note for Options= 1: LeaseAsset,2: PurchaseAsset, 3: AssetManuals, 4: AssetImagesVideos
                    //nx: note id
                    //fk: foreign key (Asset FileID)
                    //en: end of the string
                    //ReplyToAddress = ReplyToAddress.Substring(0, ReplyToAddress.IndexOf('@')) + "+an" + this.EquipmentMasterID + "un" + objUserInformation.UserInfoID + "cb" + IncentexGlobal.CurrentMember.UserInfoID + "nt2ni" + objComNot.NoteID + "en" + ReplyToAddress.Substring(ReplyToAddress.IndexOf('@'), ReplyToAddress.Length - ReplyToAddress.IndexOf('@'));

                    ReplyToAddress = ReplyToAddress.Substring(0, ReplyToAddress.IndexOf('@')) + "+an" + this.EquipmentMasterID + "un" + objInsertNotesForSendMailResult.RecipientID + "rtaxcb" + IncentexGlobal.CurrentMember.UserInfoID + "nf" + (mvAddNewAsset.ActiveViewIndex + 1) + "sf" + strSpecificNoteFor + "nx" + objInsertNotesForSendMailResult.NoteID + "fk" + hfFileID.Value.Trim() + "en" + ReplyToAddress.Substring(ReplyToAddress.IndexOf('@'), ReplyToAddress.Length - ReplyToAddress.IndexOf('@'));
                    if (HttpContext.Current.Request.IsLocal)
                        objCommonMails.SendMailWithReplyToANDAttachmentForAsset(IncentexGlobal.CurrentMember.UserInfoID, "Asset Files", IncentexGlobal.CurrentMember.LoginEmail, strToAddress, hfSubject.Value.Trim(), strBody, IncentexGlobal.CurrentMember.FirstName + ' ' + IncentexGlobal.CurrentMember.LastName, true, true, strFilePath, smtphost, smtpport, "incentextest6@gmail.com", "test6incentex", hfAssetFiles.Value.Substring(hfAssetFiles.Value.LastIndexOf('/')), false, ReplyToAddress);
                    else
                        objCommonMails.SendMailWithReplyToANDAttachmentForAsset(IncentexGlobal.CurrentMember.UserInfoID, "Asset Files", IncentexGlobal.CurrentMember.LoginEmail, strToAddress, hfSubject.Value.Trim(), strBody, IncentexGlobal.CurrentMember.FirstName + ' ' + IncentexGlobal.CurrentMember.LastName, true, true, strFilePath, smtphost, smtpport, Common.UserName, Common.Password, hfAssetFiles.Value.Substring(hfAssetFiles.Value.LastIndexOf('/')), false, ReplyToAddress);
                    this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Mail Send Successfully.'); $('#" + lnkbtn.ClientID + "').parents('.childdiv').prev('.parentdiv').click();", true);
                }
                else
                    this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('ERROR! Mail cannot be send.'); $('#" + lnkbtn.ClientID + "').parents('.childdiv').prev('.parentdiv').click();", true);
                txtMailMessage.Value = "";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnDeleteFile_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = (LinkButton)sender;
            Int64 EquipmentFileID = Convert.ToInt64(lnkbtn.CommandArgument);
            String strFilePath = Server.MapPath("~/" + Convert.ToString(lnkbtn.CommandName));
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            objAssetMgtRepository.DeleteEquipmentFile(EquipmentFileID);

            if (System.IO.File.Exists(strFilePath))
                System.IO.File.Delete(strFilePath);

            if (mvAddNewAsset.ActiveViewIndex == 3)
                BindSpecifiationFiles();
            else
                BindAcquisitionMethodFiles();

            this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('File Deleted Successfully.'); $('#" + lnkbtn.ClientID + "').parents('.childdiv').prev('.parentdiv').click();", true);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnClose_Click(object sender, EventArgs e)
    {
        try
        {
            string saveStatus = string.Empty;

            if (mvAddNewAsset.ActiveViewIndex == 0)
                saveStatus = SaveBasicTabRecords();
            else
                saveStatus = SaveAccountingRecords();

            if (saveStatus.ToLower() == "success" || saveStatus.ToLower().Contains("please"))
                Response.Redirect("Assets.aspx?se=add");
            else
                this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('" + saveStatus + "')", true);
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnFileUpload_Click(object sender, EventArgs e)
    {

        try
        {
            if (fuCommonFileUpload.HasFile)
            {
                AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
                EquipmentFile objEquipmentFile = new EquipmentFile();

                String strName = String.Empty;
                string strFileName = string.Empty;
                string strExtension = string.Empty;

                strExtension = System.IO.Path.GetExtension(fuCommonFileUpload.FileName);
                strFileName = txtFileName.Text.Trim();


                objEquipmentFile = new EquipmentFile();
                objEquipmentFile.EquipmentMasterID = this.EquipmentMasterID;
                objEquipmentFile.FileType = fuCommonFileUpload.PostedFile.ContentType;

                if (hdnFileFor.Value.Trim() != "")
                    objEquipmentFile.FileFor = hdnFileFor.Value.Trim();
                else
                {
                    if (mvAddNewAsset.ActiveViewIndex == 1)
                    {
                        if (ddlAccountingAcquisitionMethod.SelectedItem.Text.ToLower().Contains("lease"))
                            objEquipmentFile.FileFor = "Lease";
                        else
                            objEquipmentFile.FileFor = "Purchase";
                    }
                    else
                        objEquipmentFile.FileFor = "Specification";

                }
                objEquipmentFile.Name = strFileName + strExtension;
                objEquipmentFile.FileTitle = strFileName;
                objEquipmentFile.FileDate = DateTime.Now;
                objEquipmentFile.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objEquipmentFile.CreatedDate = DateTime.Now;

                objAssetMgtRepository.Insert(objEquipmentFile);

                //objEquipmentMaster.LeaseAggrement = strFileName;
                if (mvAddNewAsset.ActiveViewIndex == 1)
                    Common.SaveDocument(Server.MapPath("../../UploadedImages/AssetManagement/" + this.EquipmentMasterID + "/Accounting/" + objEquipmentFile.FileFor), (strFileName + strExtension), true, fuCommonFileUpload.PostedFile);
                else
                    Common.SaveDocument(Server.MapPath("../../UploadedImages/AssetManagement/" + this.EquipmentMasterID + "/" + objEquipmentFile.FileFor), (strFileName + strExtension), true, fuCommonFileUpload.PostedFile);
                objAssetMgtRepository.SubmitChanges();

                if (mvAddNewAsset.ActiveViewIndex == 1)
                    BindAcquisitionMethodFiles();
                else if (mvAddNewAsset.ActiveViewIndex == 3)
                    BindSpecifiationFiles();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnAddField_Click(object sender, EventArgs e)
    {
        try
        {
            if (EquipmentMasterID > 0)
            {
                AssetMgtRepository objAssetMgtReporitory = new AssetMgtRepository();
                String ControlType = "TEXTBOX";
                String strSectionName = hfSectionName.Value.Trim();
                foreach (RepeaterItem item in rpFieldType.Items)
                {
                    HiddenField hfValue = (HiddenField)item.FindControl("hfValue");
                    RadioButton rdbtnControlType = (RadioButton)item.FindControl("rdbtnControlType");
                    if (rdbtnControlType != null && hfValue != null && rdbtnControlType.Checked)
                    {
                        ControlType = hfValue.Value.ToUpper();
                        break;
                    }
                }
                string strInsertStatus = objAssetMgtReporitory.InsertIntoEquipmentFieldMaster(EquipmentMasterID, txtFieldName.Text.Trim(), EquipmentTypeID, this.CompanyID, ControlType, rdbtnAllAsset.Checked, strSectionName);

                //Reset the change value
                if (strInsertStatus != null && strInsertStatus.ToUpper() == "SUCCESS")
                {
                    //this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Field added successfully.');", true);
                    Session["Event"] = "Added";
                    Session["ActiveViewIndex"] = mvAddNewAsset.ActiveViewIndex;
                    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/AddNewAsset.aspx?eqpID=" + this.EquipmentMasterID, false);
                }
                else if (strInsertStatus != null && strInsertStatus.ToUpper() == "ALREADY EXISTS")
                    this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Field Already Exists.');", true);
                else
                    this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Some error while inserting records.');", true);
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Please save asset and create new fields for that asset.');", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
        }
    }

    protected void lnkRemoveControl_Click(object sender, EventArgs e)
    {
        try
        {

            EquipmentFieldDetail objEquipmentFieldDetail = new EquipmentFieldDetail();
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            String iLookUpCode = String.Empty;

            iLookUpCode = Convert.ToString(sender.GetType().GetProperty("GroupName").GetValue(sender, null));

            objAssetMgtRepository.DeleteFieldDetailFieldMasterByID(iLookUpCode);

            Session["Event"] = "Deleted";
            Session["ActiveViewIndex"] = mvAddNewAsset.ActiveViewIndex;
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/AddNewAsset.aspx?eqpID=" + this.EquipmentMasterID, false);

            //OLD Logic - 18th Dec 2013
            //Control objControl = (Control)sender;

            //if (mvAddNewAsset.ActiveViewIndex == 0)
            //{
            //    ul_basicfields.Controls.Clear();
            //    BindCustomFields("Basic", false);
            //}
            //else if (mvAddNewAsset.ActiveViewIndex == 1)
            //{
            //    //ul_AccountingFields.Controls.Clear();
            //    //BindCustomFields("Accounting", false);
            //    ul_AccountingFields.Controls.Remove(objControl);
            //}
            //else
            //{
            //    ul_SpecificationsFields.Controls.Clear();
            //    BindCustomFields("Specifications", false);
            //}
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lbHelpVideo_Click(object sender, EventArgs e)
    {
        string strVideoURL = Common.GetMyHelpVideo("Help Video", "Asset Management", "Add New Field");
        if (strVideoURL != "")
        {
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "ShowVideo('" + strVideoURL + "');", true);
        }
    }

    #endregion

    #region Methods

    private void BindFieldTypes()
    {
        try
        {
            PreferenceRepository objPreferenceRepository = new PreferenceRepository();
            //List<PreferenceValue> objFieldType = objPreferenceRepository.GetByPreferenceKey("FieldTypes");
            rpFieldType.DataSource = objPreferenceRepository.GetByPreferenceKey("FieldTypes");
            rpFieldType.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Bind dropdown and add onchange attribute
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ddl"></param>
    /// <param name="list"></param>
    /// <param name="DataTextField"></param>
    /// <param name="DataValueField"></param>
    /// <param name="FirstListItem"></param>
    public static void BindCustomDropDown<T>(CustomDropDown ddl, List<T> list, string DataTextField, string DataValueField,
        string FirstListItem, string AddNewOptionValue) where T : class
    {
        try
        {

            ddl.Items.Clear();
            if (list.Count > 0)
            {

                ddl.DataSource = list;
                ddl.DataTextField = DataTextField;
                ddl.DataValueField = DataValueField;
                ddl.DataBind();
            }
            ListItem liAddNew = new ListItem("- Add New Option -", AddNewOptionValue);
            ddl.Items.Insert((ddl.Items.Count), liAddNew);

            ddl.Value = AddNewOptionValue;

            if (!string.IsNullOrEmpty(FirstListItem))
                ddl.Items.Insert(0, new ListItem(FirstListItem, "0"));

        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    private void BindBasicDropDown()
    {
        try
        {
            LookupRepository objLookupRepository = new LookupRepository();
            List<INC_Lookup> objList = new List<INC_Lookup>();

            //Bind Equipment Types

            objList = objLookupRepository.GetByLookup("EquipmentType").OrderBy(p => p.sLookupName).ToList();
            BindCustomDropDown(ddlBasicAssetType, objList, "sLookupName", "iLookupID", "- Asset Type -", "-1");
            //CustomDrop

            //Bind Brands
            objList = objLookupRepository.GetByLookup("Brand").OrderBy(p => p.sLookupName).ToList();
            BindCustomDropDown(ddlBasicManufacturer, objList, "sLookupName", "iLookupID", "- Manufacturer -", "-1");

            //Bind Models
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            List<EquipmentLookup> objLookup = new List<EquipmentLookup>();
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup("EquipmentModel").OrderBy(p => p.sLookupName).ToList();
            BindCustomDropDown(ddlBasicModel, objLookup, "sLookupName", "iLookupID", "- Model -", "-1");

            //Bind Location/Base Stations
            CompanyRepository objRepo = new CompanyRepository();
            List<INC_BasedStation> objBaseStationList = new List<INC_BasedStation>();
            objBaseStationList = objRepo.GetAllBaseStationResult().OrderBy(p => p.sBaseStation).ToList();
            Common.BindDropDown(ddlBasicLocation, objBaseStationList, "sBaseStation", "iBaseStationId", "- Location -");


        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Function to remove the Active class from each of the Tab
    /// </summary>
    private void RemoveActiveClassFromTabs(LinkButton lnkbtn)
    {
        HtmlGenericControl ul_NewAsset = (HtmlGenericControl)lnkbtn.Parent;
        ControlCollection lnkbtnCollection = ul_NewAsset.Controls;

        foreach (Control lnkTabs in lnkbtnCollection)
        {
            if (lnkTabs.GetType().Name.ToLower() == lnkbtn.GetType().Name.ToLower())
            {
                LinkButton currlnkbtn = (LinkButton)lnkTabs;
                currlnkbtn.Attributes.Remove("class");
            }
        }
    }

    private void SetActiveClassToLinkButtonTabs(LinkButton lnkbtn)
    {
        RemoveActiveClassFromTabs(lnkbtn);
        lnkbtn.Attributes.Add("class", "active");
    }

    private void SetActiveClassToLinkButtonTabs(LinkButton lnkbtn, MultiView mv)
    {
        try
        {
            SetActiveClassToLinkButtonTabs(lnkbtn);
            mv.ActiveViewIndex = Convert.ToInt32(lnkbtn.CommandArgument);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void PopulateData(Int64 equipmentMasterID)
    {
        try
        {
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            EquipmentMaster objEquipmentMaster = new EquipmentMaster();
            if (equipmentMasterID > 0)
                objEquipmentMaster = objAssetMgtRepository.GetById(equipmentMasterID);
            if (objEquipmentMaster != null)
            {
                //Basic Tab Controls
                ddlBasicAssetType.SelectedValue = Convert.ToString(objEquipmentMaster.EquipmentTypeID);
                EquipmentTypeID = Convert.ToInt64(objEquipmentMaster.EquipmentTypeID);
                ddlBasicManufacturer.SelectedValue = Convert.ToString(objEquipmentMaster.BrandID);
                txtBasicSerialNumber.Text = Convert.ToString(objEquipmentMaster.SerialNumber);
                txtBasicAssetID.Text = Convert.ToString(objEquipmentMaster.EquipmentID);
                txtBasicPlate.Text = Convert.ToString(objEquipmentMaster.PlateNumber);
                ddlBasicModel.SelectedValue = objEquipmentMaster.EquipmentModel != null ? Convert.ToString(objEquipmentMaster.EquipmentModel) : "0";
                ddlBasicLocation.SelectedValue = Convert.ToString(objEquipmentMaster.BaseStationID);
                txtManufacturingDate.Text = objEquipmentMaster.ManufacturingDate != null ? Convert.ToDateTime(objEquipmentMaster.ManufacturingDate).ToString("MM/dd/yyyy") : "";
                //Bind Notes for this Equipment/Asset
                BindBasicTabNotes();
                BindFieldTypes();

                //set the Header
                title_span.InnerText = txtBasicAssetID.Text + " - " + ddlBasicAssetType.SelectedItem.Text;

                // Flag Asset
                if (objEquipmentMaster.Flagged)
                {
                    lnkFlagAsset.Attributes.Add("class", "asset-flag-on clientsidectrl");
                    lnkFlagAsset.Attributes.Add("onclick", "ShowAddNotePopup(this,'Do you want to save your changes?','asset-flag-on',event,true);");
                    lnkbtnAssetsUnflagged.Visible = true;
                    lnkViewNotes.Attributes.Add("onclick", "OpenViewNotes(this,'Do you want to save your changes?','view-note',event,true);");
                    lnkViewNotes.Visible = true;
                }
                else
                {
                    lnkFlagAsset.Attributes.Add("onclick", "ShowAddNotePopup(this,'Do you want to save your changes?','asset-flag-off',event,true);");
                    lnkbtnAssetsUnflagged.Visible = false;
                    lnkViewNotes.Visible = false;
                }
                IsFlagAsset = objEquipmentMaster.Flagged;


                //Bind Accounting Tab Invoice GridView Data
                //BindAccountingTabControls();

                //Bind Warranty Tab Warranty GridView Data
                //BindWarrantyGrid(false);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ShowPopUp(HtmlGenericControl Parent_span)
    {
        try
        {
            //        Label lblPopUpDiv = (Label)Parent_span.FindControl("lblClaimPopup");
            if (Parent_span.Attributes["data-content"] != null)
            {
                String strDivClasses = Convert.ToString(Parent_span.Attributes["data-content"]).ToLower();
                if (!String.IsNullOrEmpty(strDivClasses))
                {
                    string[] strDivIDs = strDivClasses.Split(';');
                    if (strDivIDs.Length == 2)
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "ShowPopUp('" + strDivIDs[0] + "','" + strDivIDs[1] + "','serverpopup');", true);
                    }
                }
            }

        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }

    }

    protected string GetPagingStatus(LinkButton lnkView)
    {
        try
        {
            String PagingStatus = "VIEW ALL";
            if (lnkView.Text.Trim().ToUpper() == "VIEW ALL")
                PagingStatus = "VIEW PAGING";
            else
                PagingStatus = "VIEW ALL";

            lnkView.Text = PagingStatus;
            return PagingStatus;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            return "Error";
        }
    }

    protected void SaveEquipmentFieldsValue(AssetMgtRepository objAssetMgtRepository, HtmlGenericControl ul_Custom_Panel)
    {
        try
        {

            EquipmentFieldDetail objEquipmentFieldDetail = new EquipmentFieldDetail();

            foreach (Control objControl in ul_Custom_Panel.Controls)
            {
                string strControlType = objControl.GetType().Name;
                if (strControlType.ToLower() == "textboxcontrol" || strControlType.ToLower() == "dropdowncontrol" || strControlType.ToLower() == "calendarcontrol")
                {
                    Int64 FieldDetailID = Convert.ToInt64(objControl.GetType().GetProperty("FieldDetailID").GetValue(objControl, null));
                    Int64 FieldMasterID = Convert.ToInt64(objControl.GetType().GetProperty("FieldMasterID").GetValue(objControl, null));
                    string strValue = string.Empty;


                    if (strControlType.ToLower() == "textboxcontrol")
                        strValue = Convert.ToString(objControl.GetType().GetProperty("Text").GetValue(objControl, null));
                    else
                        strValue = Convert.ToString(objControl.GetType().GetProperty("SelectedValue").GetValue(objControl, null));

                    if (FieldMasterID > 0)
                    {
                        if (FieldDetailID > 0)
                            objEquipmentFieldDetail = objAssetMgtRepository.GetFieldDetailById(FieldDetailID);
                        else
                            objEquipmentFieldDetail = new EquipmentFieldDetail();

                        objEquipmentFieldDetail.FieldMasterID = FieldMasterID;
                        objEquipmentFieldDetail.EquipmentMasterID = this.EquipmentMasterID;
                        objEquipmentFieldDetail.CompanyID = this.CompanyID;
                        objEquipmentFieldDetail.Description = strValue;

                        if (FieldDetailID == 0)
                            objAssetMgtRepository.Insert(objEquipmentFieldDetail);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ResetCtrlInfoChangedValue(string DivID)
    {
        HtmlInputHidden CtrlInfoChanged = (HtmlInputHidden)this.Master.FindControl("CtrlInfoChanged");
        if (CtrlInfoChanged != null && !String.IsNullOrEmpty(CtrlInfoChanged.Value.Trim()) && Convert.ToBoolean(CtrlInfoChanged.Value.Trim()))
            CtrlInfoChanged.Value = "false";

        HtmlInputHidden hdnMainDiv = (HtmlInputHidden)this.Master.FindControl("hdnMainDiv");
        if (hdnMainDiv != null && !String.IsNullOrEmpty(hdnMainDiv.Value.Trim()))
            hdnMainDiv.Value = DivID;
    }

    protected void FireCtrlEvent()
    {
        HtmlInputHidden CtrlInfoChanged = (HtmlInputHidden)this.Master.FindControl("CtrlInfoChanged");
        if (CtrlInfoChanged != null && !String.IsNullOrEmpty(CtrlInfoChanged.Value.Trim()) && Convert.ToBoolean(CtrlInfoChanged.Value.Trim()))
        {
            HtmlInputHidden CtrlThatFiresEvent = (HtmlInputHidden)this.Master.FindControl("CtrlThatFiresEvent");
            if (CtrlThatFiresEvent != null && !String.IsNullOrEmpty(CtrlThatFiresEvent.Value.Trim()))
            {
                string strClassName = CtrlThatFiresEvent.Attributes["class"];
                bool IsClientSideCtrl = false;
                if (!string.IsNullOrEmpty(strClassName) && strClassName.IndexOf("classname") > -1)
                    IsClientSideCtrl = true;
                //IsNormalFlow = false;
                this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "FireServerClickEvent('" + CtrlThatFiresEvent.Value.Trim() + "','" + IsClientSideCtrl + "');", true);
            }
            CtrlInfoChanged.Value = "false";
        }
    }
    #endregion

    #region Basic Tab

    protected void btnBasicSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                string saveStatus = SaveBasicTabRecords();
                if (saveStatus.ToLower() == "success")
                {
                    //Set Accounting Tab as Active
                    HtmlInputHidden CtrlInfoChanged = (HtmlInputHidden)this.Master.FindControl("CtrlInfoChanged");

                    Control MainCtrl = new Control();
                    ContentPlaceHolder cph = new ContentPlaceHolder();

                    bool IsNormalFlow = true;

                    if (CtrlInfoChanged != null && !String.IsNullOrEmpty(CtrlInfoChanged.Value.Trim()) && Convert.ToBoolean(CtrlInfoChanged.Value.Trim()))
                    {
                        HtmlInputHidden CtrlThatFiresEvent = (HtmlInputHidden)this.Master.FindControl("CtrlThatFiresEvent");
                        if (CtrlThatFiresEvent != null && !String.IsNullOrEmpty(CtrlThatFiresEvent.Value.Trim()))
                        {
                            string strClassName = CtrlThatFiresEvent.Attributes["class"];
                            bool IsClientSideCtrl = false;
                            if (!string.IsNullOrEmpty(strClassName) && strClassName.IndexOf("classname") > -1)
                                IsClientSideCtrl = true;
                            IsNormalFlow = false;
                            this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "FireServerClickEvent('" + CtrlThatFiresEvent.Value.Trim() + "','" + IsClientSideCtrl + "');", true);
                        }
                        CtrlInfoChanged.Value = "false";
                    }

                    if (IsNormalFlow)
                        SetActiveClassToLinkButtonTabs(lnkbtnAccounting, mvAddNewAsset);
                    //(Control)cph.FindControl("a_basic_addnewfield")
                    //String strvalue = String.Empty;
                    //if (MainCtrl != null)
                    //    strvalue = Convert.ToString(MainCtrl.GetType().GetProperty("class").GetValue(MainCtrl, null));


                }
                else
                    this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('" + saveStatus + "')", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void ResetBasicControls()
    {
        try
        {
            IsFlagAsset = false;
            ddlBasicAssetType.SelectedIndex = 0;
            ddlBasicManufacturer.SelectedIndex = 0;
            ddlBasicModel.SelectedIndex = 0;
            ddlBasicLocation.SelectedIndex = 0;
            txtBasicSerialNumber.Text = String.Empty;
            txtBasicAssetID.Text = String.Empty;
            txtBasicPlate.Text = String.Empty;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnSaveNote_Click(object sender, EventArgs e)
    {
        try
        {
            String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.FlagAssets);
            if (txtNoteDetails.Value.Trim() != "")
            {
                AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
                objAssetMgtRepository.FlagAssetsAndAddNote(this.EquipmentMasterID, IncentexGlobal.CurrentMember.UserInfoID, Incentex.DAL.Common.DAEnums.NoteForType.FlagAssets, txtNoteDetails.Value.Trim());

                lnkFlagAsset.Attributes.Remove("class");
                lnkFlagAsset.Attributes.Add("class", "asset-flag-on clientsidectrl");
                lnkFlagAsset.Attributes.Add("onclick", "ShowAddNotePopup(this,'Do you want to save your changes?','asset-flag-on',event,true);");
                lnkbtnAssetsUnflagged.Visible = true;
                lnkViewNotes.Attributes.Add("onclick", "OpenViewNotes(this,'Do you want to save your changes?','view-note',event,true);");
                lnkViewNotes.Visible = true;
                //Reset the change value
                FireCtrlEvent();
                this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Asset has been flagged successfully.');$('.close-btn').click();", true);
            }
            BindBasicTabNotes();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void BindBasicTabNotes()
    {
        try
        {
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();
            string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.FlagAssets);
            var objNoteDetailResult = objCompNoteHistRepos.GetFlaggedNotes(Convert.ToInt64(this.EquipmentMasterID), strNoteFor);
            if (objNoteDetailResult != null && objNoteDetailResult.Count > 0)
                nobasicnotes_li.Visible = false;
            else
                nobasicnotes_li.Visible = true;
            rpBasicViewNote.DataSource = objNoteDetailResult;
            rpBasicViewNote.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnAssetsNote_Click(object sender, EventArgs e)
    {
        try
        {
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            objAssetMgtRepository.FlagAssetsAndAddNote(this.EquipmentMasterID, IncentexGlobal.CurrentMember.UserInfoID, Incentex.DAL.Common.DAEnums.NoteForType.FlagAssets, txtFlaggedNoteDetails.Value.Trim());
            txtFlaggedNoteDetails.Value = "";
            BindBasicTabNotes();
            //Reset the change value
            FireCtrlEvent();

            this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Note added successfully.');$('.close-btn').click();", true);

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnAssetsUnflagged_Click(object sender, EventArgs e)
    {
        AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
        objAssetMgtRepository.UnflaggedAssets(this.EquipmentMasterID, IncentexGlobal.CurrentMember.UserInfoID, Incentex.DAL.Common.DAEnums.NoteForType.FlagAssets);
        lnkFlagAsset.Attributes.Remove("class");
        lnkFlagAsset.Attributes.Add("class", "asset-flag-off clientsidectrl");
        lnkFlagAsset.Attributes.Add("onclick", "ShowAddNotePopup(this,'Do you want to save your changes?','asset-flag-off',event,true);");
        lnkbtnAssetsUnflagged.Visible = false;
        lnkViewNotes.Visible = false;
        //Reset the change value
        FireCtrlEvent();
        this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Asset has been UnFlagged successfully.');$('.close-btn').click();", true);

    }

    protected void AddFlaggedNotes(string strNoteFor, string strNoteContents)
    {
        try
        {
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();
            NoteDetail objComNot = new NoteDetail();
            objComNot.Notecontents = strNoteContents;
            objComNot.NoteFor = strNoteFor;
            objComNot.SpecificNoteFor = "Flagged";
            objComNot.ForeignKey = Convert.ToInt64(this.EquipmentMasterID);
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objCompNoteHistRepos.Insert(objComNot);
            objCompNoteHistRepos.SubmitChanges();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected string SaveBasicTabRecords()
    {
        string message = string.Empty;
        try
        {
            if (ddlBasicAssetType.SelectedValue != "0" || ddlBasicManufacturer.SelectedValue != "0" || txtBasicSerialNumber.Text.Trim() != "" || txtBasicPlate.Text.Trim() != "" || txtBasicAssetID.Text.Trim() != "" || ddlBasicModel.SelectedValue != "0" || ddlBasicLocation.SelectedValue != "0" && txtManufacturingDate.Text.Trim() != "")
            {
                AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
                EquipmentMaster objEquipmentMaster = new EquipmentMaster();
                if (this.EquipmentMasterID > 0)
                    objEquipmentMaster = objAssetMgtRepository.GetById(this.EquipmentMasterID);
                //
                objEquipmentMaster.CompanyID = this.CompanyID;
                objEquipmentMaster.EquipmentTypeID = Convert.ToInt64(ddlBasicAssetType.SelectedValue);
                objEquipmentMaster.BrandID = Convert.ToInt64(ddlBasicManufacturer.SelectedValue);
                objEquipmentMaster.SerialNumber = Convert.ToString(txtBasicSerialNumber.Text.Trim());
                objEquipmentMaster.EquipmentID = Convert.ToString(txtBasicAssetID.Text.Trim());
                objEquipmentMaster.PlateNumber = Convert.ToString(txtBasicPlate.Text.Trim());
                objEquipmentMaster.EquipmentModel = Convert.ToInt64(ddlBasicModel.SelectedValue);
                objEquipmentMaster.BaseStationID = Convert.ToInt64(ddlBasicLocation.SelectedValue);
                if (txtManufacturingDate.Text.Trim() != "")
                    objEquipmentMaster.ManufacturingDate = Convert.ToDateTime(txtManufacturingDate.Text.Trim());

                // Flag Asset
                if (IsFlagAsset)
                    objEquipmentMaster.Flagged = true;
                else
                    objEquipmentMaster.Flagged = false;

#warning since there is no status is define
                objEquipmentMaster.Status = 135; // For Active 
                objEquipmentMaster.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objEquipmentMaster.CreatedDate = DateTime.Now;


                if (this.EquipmentMasterID == 0)
                    objAssetMgtRepository.Insert(objEquipmentMaster);

                //Save Custom Added Fields and Its Value
                SaveEquipmentFieldsValue(objAssetMgtRepository, ul_basicfields);

                objAssetMgtRepository.SubmitChanges();
                //Set Equipment Master ID to view State
                this.EquipmentMasterID = objEquipmentMaster.EquipmentMasterID;
                message = "success";

            }
            else
                message = "Please enter atleast single value for Assets.";

            return message;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            return ex.Message.Replace("'", "&quot;").Replace("\r\n", "");
        }
    }



    #endregion

    #region Accouting Tab

    protected void BindAccountingTabControls()
    {
        BindAccountingDropDowns();
        BindAccountingInvoiceGrid(false);
        BindAcquisitionMethodFiles();

        AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
        EquipmentMaster objEquipmentMaster = new EquipmentMaster();
        if (this.EquipmentMasterID > 0)
            objEquipmentMaster = objAssetMgtRepository.GetById(this.EquipmentMasterID);
        if (objEquipmentMaster != null)
        {
            //Accounting Tab Controls
            txtAccountingAcquisitonDate.Text = objEquipmentMaster.AcquisitionDate != null ? Convert.ToDateTime(objEquipmentMaster.AcquisitionDate).ToString("MM/dd/yyyy") : "";
            EquipmentTypeID = Convert.ToInt64(objEquipmentMaster.EquipmentTypeID);

            if (objEquipmentMaster.PurchasedFromID != null)
                ddlAccountingPurchasedFrom.SelectedValue = Convert.ToString(objEquipmentMaster.PurchasedFromID);
            if (objEquipmentMaster.NewOrRefurbished != null)
                ddlAccountingConditions.SelectedValue = Convert.ToString(objEquipmentMaster.NewOrRefurbished);
            if (objEquipmentMaster.EquipmentLife != null)
                ddlAccountingAssetLife.SelectedValue = Convert.ToString(objEquipmentMaster.EquipmentLife);
            if (objEquipmentMaster.GSEDepartmentID != null)
                ddlAccountingDepartments.SelectedValue = Convert.ToString(objEquipmentMaster.GSEDepartmentID);
            if (objEquipmentMaster.CostCenterCodeID != null)
                ddlAccountingCostCenter.SelectedValue = Convert.ToString(objEquipmentMaster.CostCenterCodeID);


            if (objEquipmentMaster.PurchaseMethod != null)
                ddlAccountingAcquisitionMethod.SelectedValue = Convert.ToString(objEquipmentMaster.PurchaseMethod);
            else
                ddlAccountingAcquisitionMethod.Items[1].Selected = true;

            if (ddlAccountingAcquisitionMethod.SelectedItem.Text.ToLower() == "lease asset")
            {
                lease_info_div.Attributes.Remove("style");
                purchase_info_div.Attributes.Add("style", "display:none;");
                hdnFileFor.Value = "Lease";

                if (objEquipmentMaster.OperatingLease != null)
                    ddlAccountingOperatingLease.SelectedValue = Convert.ToString(objEquipmentMaster.OperatingLease);
                if (objEquipmentMaster.LeaseTerms != null)
                    ddlAccountingLeaseTerms.SelectedValue = Convert.ToString(objEquipmentMaster.LeaseTerms);


                if (objEquipmentMaster.MonthlyBasePayment > 0)
                    txtAccountingMonthlyBasePayment.Text = Convert.ToString(objEquipmentMaster.MonthlyBasePayment);
                if (objEquipmentMaster.MaintenancePlanCost > 0)
                    txtAccountingMaintenancePlanCost.Text = Convert.ToString(objEquipmentMaster.MaintenancePlanCost);
                if (objEquipmentMaster.SalesTax > 0)
                    txtAccountingSalesTax.Text = Convert.ToString(objEquipmentMaster.SalesTax);
                if (objEquipmentMaster.TotalMonthlyCost > 0)
                    txtAccountingTotalMonthlyCost.Text = Convert.ToString(objEquipmentMaster.TotalMonthlyCost);
            }
            else
            {
                purchase_info_div.Attributes.Remove("style");
                lease_info_div.Attributes.Add("style", "display:none;");
                hdnFileFor.Value = "Purchase";

                //Check as Dadra doesn't want any value if not entered
                if (objEquipmentMaster.Depreciation > 0)
                    txtAccountingDepreciation.Text = Convert.ToString(objEquipmentMaster.Depreciation);
                if (objEquipmentMaster.PurchasePrice > 0)
                    txtAccountingPurchasePrice.Text = Convert.ToString(objEquipmentMaster.PurchasePrice);
                if (objEquipmentMaster.YearlyDepreciation > 0)
                    txtAccountingYearlyDepreciation.Text = Convert.ToString(objEquipmentMaster.YearlyDepreciation);
            }
        }
        else
        {
            purchase_info_div.Attributes.Remove("style");
            lease_info_div.Attributes.Add("style", "display:none;");
            hdnFileFor.Value = "Purchase";
        }
    }

    protected void BindSubJobCode(Int64 JobCodeID)
    {
        try
        {
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            var rsult = objAssetMgtRepository.GetAllSubJobCodeDetail(JobCodeID);
            cblJobSubCode.DataSource = rsult;
            cblJobSubCode.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindAccountingDropDowns()
    {
        try
        {
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            LookupRepository objLookupRepository = new LookupRepository();
            List<EquipmentJobCodeLookup> objList = new List<EquipmentJobCodeLookup>();

            //Bind Equipment Types

            //Bind Equipment Job Code
            objList = objAssetMgtRepository.GetAllJobCode().OrderBy(p => p.JobCodeName).ToList();
            BindCustomDropDown(ddlJobCode, objList, "JobCodeName", "JobCodeID", "- Job Code -", "-1");


            List<EquipmentLookup> objLookup = new List<EquipmentLookup>();
            //Bind Purchase Method RadioButton Repeater
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup("PurchaseMethod");
            Common.BindDropDown(ddlAccountingAcquisitionMethod, objLookup, "sLookupName", "iLookupID", "");

            //Bind Assets Life
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup("EquipmentLife").OrderBy(le => Convert.ToInt32(le.sLookupName.Substring(0, le.sLookupName.IndexOf("year")).Trim())).ToList();
            BindCustomDropDown(ddlAccountingAssetLife, objLookup, "sLookupName", "iLookupID", "- Asset Life -", "-1");

            List<INC_Lookup> objLookUpList = new List<INC_Lookup>();
            //Bind Departments
            objLookUpList = objLookupRepository.GetByLookup("GSEDepartment");
            BindCustomDropDown(ddlAccountingDepartments, objLookUpList, "sLookupName", "iLookupID", "- Department -", "-1");

            //Bind Condition/NewOrRefurbished
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup("NewOrRefurbished");
            BindCustomDropDown(ddlAccountingConditions, objLookup, "sLookupName", "iLookupID", "- Condition -", "-1");

            //Bind Purchased From
            objLookUpList = objLookupRepository.GetByLookup("PurchasedFrom");
            BindCustomDropDown(ddlAccountingPurchasedFrom, objLookUpList, "sLookupName", "iLookupID", "- Purchased From -", "-1");


            //Bind Operating Lease
            objLookUpList = objLookupRepository.GetByLookup("OperatingLease");
            BindCustomDropDown(ddlAccountingOperatingLease, objLookUpList, "sLookupName", "iLookupID", "- Operating Lease -", "-1");

            //Bind Lease Terms
            objLookUpList = objLookupRepository.GetByLookup("LeaseTerms");
            BindCustomDropDown(ddlAccountingLeaseTerms, objLookUpList, "sLookupName", "iLookupID", "- Lease Terms -", "-1");

            //Bind Accouting Tab --- Cost Center Dropdown
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup("CostCenterCode");
            BindCustomDropDown(ddlAccountingCostCenter, objLookup, "sLookupName", "iLookupID", "- Cost-Center -", "-1");

            //Bind Invoice Vendor DropDown
            AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
            List<GetEquipVendorDetailResult> objGetEquipVendorDetailResult = new List<GetEquipVendorDetailResult>();
            objGetEquipVendorDetailResult = objAssetVendorRepository.GetVendorDetailBySP(this.CompanyID);
            Common.BindDropDown(ddlVendor, objGetEquipVendorDetailResult, "EquipmentVendorName", "EquipmentVendorID", "- Vendor -");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindAccountingInvoiceGrid(Boolean FromPaging)
    {

        try
        {
            AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
            bool IsAuthentic = objAssetVendorRepository.IsAuthentic(IncentexGlobal.CurrentMember.UserInfoID);
            if (IsAuthentic)
            {
                DataView myDataView = new DataView();
                DataTable dt = new DataTable();
                AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();

                dt = Common.ListToDataTable(objAssetMgtRepository.GetEquipMaintenanceCostById(Convert.ToInt64(EquipmentMasterID)));
                if (dt.Rows.Count == 0)
                    pagingtable.Visible = false;
                else
                    pagingtable.Visible = true;

                myDataView = dt.DefaultView;
                if (!String.IsNullOrEmpty(this.InvoiceSortExp))
                    myDataView.Sort = Convert.ToString(this.InvoiceSortExp) + " " + Convert.ToString(this.InvoiceSortOrder);

                if (!FromPaging)
                {
                    if (this.pds == null)
                        this.pds = new Dictionary<Int64, PagedDataSource>();
                    if (this.FrmPg == null)
                        this.FrmPg = new Dictionary<Int64, Int32>();
                    if (this.ToPg == null)
                        this.ToPg = new Dictionary<Int64, Int32>();
                    if (this.CurrentPage == null)
                        this.CurrentPage = new Dictionary<Int64, Int32>();

                    this.FrmPg.Remove((int)GridID.Invoice);
                    this.FrmPg.Add((int)GridID.Invoice, 1);

                    this.ToPg.Remove((int)GridID.Invoice);
                    this.ToPg.Add((int)GridID.Invoice, this.PagerSize);

                    this.CurrentPage.Remove((int)GridID.Invoice);
                    this.CurrentPage.Add((int)GridID.Invoice, 0);
                }

                PagedDataSource tPds = new PagedDataSource();

                tPds.DataSource = myDataView;

                if (InvoicePagingStatus.ToUpper() == "VIEW ALL")
                    tPds.AllowPaging = true;
                else
                    tPds.AllowPaging = false;

                tPds.PageSize = this.PagerSize;
                tPds.CurrentPageIndex = CurrentPage.FirstOrDefault(le => le.Key == (int)GridID.Invoice).Value;

                this.pds.Remove((int)GridID.Invoice);
                this.pds.Add((int)GridID.Invoice, tPds);

                lnkbtnNext.Visible = !tPds.IsLastPage;
                lnkbtnPrevious.Visible = !tPds.IsFirstPage;

                gvEquipment.DataSource = tPds;
                gvEquipment.DataBind();
                dtlPaging.DataSource = doPaging((int)GridID.Invoice);
                dtlPaging.DataBind();

                if (gvEquipment.Rows.Count == 0)
                    pagingtable.Visible = false;
                else
                    pagingtable.Visible = true;

            }
            else
                pagingtable.Visible = false;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnAccountingSave_Click(object sender, EventArgs e)
    {
        try
        {
            string saveStatus = SaveAccountingRecords();
            if (saveStatus.ToLower() == "success")
            {
                HtmlInputHidden CtrlInfoChanged = (HtmlInputHidden)this.Master.FindControl("CtrlInfoChanged");

                Control MainCtrl = new Control();
                ContentPlaceHolder cph = new ContentPlaceHolder();

                bool IsNormalFlow = true;

                if (CtrlInfoChanged != null && !String.IsNullOrEmpty(CtrlInfoChanged.Value.Trim()) && Convert.ToBoolean(CtrlInfoChanged.Value.Trim()))
                {
                    HtmlInputHidden CtrlThatFiresEvent = (HtmlInputHidden)this.Master.FindControl("CtrlThatFiresEvent");
                    if (CtrlThatFiresEvent != null && !String.IsNullOrEmpty(CtrlThatFiresEvent.Value.Trim()))
                    {
                        string strClassName = CtrlThatFiresEvent.Attributes["class"];
                        bool IsClientSideCtrl = false;
                        if (!string.IsNullOrEmpty(strClassName) && strClassName.IndexOf("classname") > -1)
                            IsClientSideCtrl = true;
                        IsNormalFlow = false;
                        this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "FireServerClickEvent('" + CtrlThatFiresEvent.Value.Trim() + "','" + IsClientSideCtrl + "');", true);
                    }
                    CtrlInfoChanged.Value = "false";
                }

                if (IsNormalFlow)
                    SetActiveClassToLinkButtonTabs(lnkbtnWarranty, mvAddNewAsset); //Set Warranty Tab as Active
            }
            else
                this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('" + saveStatus + "')", true);

            //Reset the change value
            ResetCtrlInfoChangedValue(Convert.ToString(lnkbtnAccounting.Attributes["data-divID"]));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected string SaveAccountingRecords()
    {
        string message = string.Empty;
        try
        {
            if (EquipmentMasterID != 0)
            {
                AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
                EquipmentMaster objEquipmentMaster = objAssetMgtRepository.GetById(EquipmentMasterID);
                if (objEquipmentMaster != null)
                {
                    if (txtAccountingAcquisitonDate.Text.Trim() != "")
                        objEquipmentMaster.AcquisitionDate = Convert.ToDateTime(txtAccountingAcquisitonDate.Text.Trim());
                    objEquipmentMaster.PurchasedFromID = Convert.ToInt64(ddlAccountingPurchasedFrom.SelectedValue);
                    objEquipmentMaster.PurchaseMethod = Convert.ToInt64(ddlAccountingAcquisitionMethod.SelectedValue);
                    objEquipmentMaster.NewOrRefurbished = Convert.ToInt64(ddlAccountingConditions.SelectedValue);
                    objEquipmentMaster.EquipmentLife = Convert.ToInt64(ddlAccountingAssetLife.SelectedValue);
                    objEquipmentMaster.GSEDepartmentID = Convert.ToInt64(ddlAccountingDepartments.SelectedValue);
                    objEquipmentMaster.CostCenterCodeID = Convert.ToInt64(ddlAccountingCostCenter.SelectedValue);

                    //Earlier Both Purchase Information and Lease Information were required. hence there are seperate fields for both of them.

                    if (hdnFileFor.Value != "Lease")
                    {
                        objEquipmentMaster.Depreciation = txtAccountingDepreciation.Text.Trim() != "" ? Convert.ToInt32(txtAccountingDepreciation.Text.Trim()) : 0;
                        objEquipmentMaster.PurchasePrice = txtAccountingPurchasePrice.Text.Trim().Replace("$", "").Trim() != "" ? Convert.ToDouble(txtAccountingPurchasePrice.Text.Trim().Replace("$", "")) : 0;
                        if (objEquipmentMaster.PurchasePrice > 0 && objEquipmentMaster.Depreciation > 0)
                            objEquipmentMaster.YearlyDepreciation = Decimal.Round(Convert.ToDecimal(objEquipmentMaster.PurchasePrice / objEquipmentMaster.Depreciation), 2);
                    }
                    else
                    {
                        objEquipmentMaster.OperatingLease = Convert.ToInt64(ddlAccountingOperatingLease.SelectedValue);
                        objEquipmentMaster.LeaseTerms = Convert.ToInt64(ddlAccountingLeaseTerms.SelectedValue);
                        objEquipmentMaster.MonthlyBasePayment = txtAccountingMonthlyBasePayment.Text.Trim().Replace("$", "").Trim() != "" ? Convert.ToDecimal(txtAccountingMonthlyBasePayment.Text.Trim().Replace("$", "").Trim()) : 0;
                        objEquipmentMaster.MaintenancePlanCost = txtAccountingMaintenancePlanCost.Text.Trim().Replace("$", "").Trim() != "" ? Convert.ToDecimal(txtAccountingMaintenancePlanCost.Text.Trim().Replace("$", "").Trim()) : 0;
                        objEquipmentMaster.SalesTax = txtAccountingSalesTax.Text.Trim().Replace("$", "") != "" ? Convert.ToDecimal(txtAccountingSalesTax.Text.Trim().Replace("$", "")) : 0;
                        objEquipmentMaster.TotalMonthlyCost = (objEquipmentMaster.MonthlyBasePayment + objEquipmentMaster.MaintenancePlanCost) + ((objEquipmentMaster.MonthlyBasePayment + objEquipmentMaster.MaintenancePlanCost) * objEquipmentMaster.SalesTax / 100);
                    }

                    //Save Custom Added Fields and Its Value
                    SaveEquipmentFieldsValue(objAssetMgtRepository, ul_AccountingFields);

                    objAssetMgtRepository.SubmitChanges();
                    message = "success";
                }
            }
            else
            {
                //By Default set Basic Tab as Active
                SetActiveClassToLinkButtonTabs(lnkbtnBasic, mvAddNewAsset);
                message = "AssetID not found";
            }
            return message;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            return ex.Message.Replace("'", "&quot;").Replace("\r\n", "");
        }
    }

    protected void gvEquipment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
                InvoiceTotalAmount = 0;
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAmount = (Label)e.Row.FindControl("lblAmount");
                if (lblAmount != null && !String.IsNullOrEmpty(lblAmount.Text))
                {
                    Decimal Amount = Decimal.Parse(lblAmount.Text);
                    InvoiceTotalAmount += Amount;
                    lblAmount.Text = Amount.ToString("C2");
                }
                LinkButton lnkbtnDeleteInvoice = (LinkButton)e.Row.FindControl("lnkbtnDeleteInvoice");
                if (lnkbtnDeleteInvoice != null)
                {
                    lnkbtnDeleteInvoice.Attributes.Add("onclick", "return GeneralConfirmationMsgForDelete('Are you sure you want to delete this Invoice?','" + lnkbtnDeleteInvoice.ClientID + "',event);");
                }

                LinkButton lnkbtnViewInvoice = (LinkButton)e.Row.FindControl("lnkbtnViewInvoice");
                if (lnkbtnViewInvoice != null)
                {
                    lnkbtnViewInvoice.Attributes.Add("onclick", "ShowInvoicePopUp('Do you want to save your changes?','" + lnkbtnViewInvoice.ClientID + "',event,false);");
                    lnkbtnViewInvoice.Attributes.Add("ontouchend", "ShowInvoicePopUp('Do you want to save your changes?','" + lnkbtnViewInvoice.ClientID + "',event,false);");
                    
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalAmount = (Label)e.Row.FindControl("lblTotalAmount");
                if (lblTotalAmount != null)
                    lblTotalAmount.Text = Convert.ToString(Decimal.Round(InvoiceTotalAmount, 2));
                lblTotalAmount.Text = InvoiceTotalAmount.ToString("C2");
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvEquipment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.Equals("Sort"))
            {
                if (this.InvoiceSortExp == String.Empty)
                {
                    this.InvoiceSortExp = Convert.ToString(e.CommandArgument);
                    this.InvoiceSortOrder = "ASC";
                }
                else
                {
                    if (this.InvoiceSortExp == Convert.ToString(e.CommandArgument))
                    {
                        if (this.InvoiceSortOrder == "ASC")
                            this.InvoiceSortOrder = "DESC";
                        else
                            this.InvoiceSortOrder = "ASC";
                    }
                    else
                    {
                        this.InvoiceSortOrder = "ASC";
                        this.InvoiceSortExp = Convert.ToString(e.CommandArgument);
                    }
                }
                BindAccountingInvoiceGrid(false);
            }
            else if (e.CommandName == "View")
            {
                BindInvoiceDetails(Convert.ToInt64(e.CommandArgument));
            }
            else if (e.CommandName == "DownloadInvoice")
            {
                String filepath = Server.MapPath("~/" + e.CommandArgument);
                String strFileName = "MyFile";
                GridViewRow gvInvoiceGrid = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnInvoiceFileName = (HiddenField)gvInvoiceGrid.FindControl("hdnInvoiceFileName");
                if (hdnInvoiceFileName != null && !String.IsNullOrEmpty(hdnInvoiceFileName.Value))
                    strFileName = hdnInvoiceFileName.Value;
                this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "ShowLoader(false);", true);
                Common.DownloadFile(filepath, strFileName);

            }
            else if (e.CommandName == "DeleteInvoice")
            {
                AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
                Int64 MaintenanceCostDetailsID = Convert.ToInt64(e.CommandArgument);
                EquipmentMaintenanceCostDetail objEquipmentMaintenanceCostDetail = objAssetMgtRepository.GetByEquipMaintenanceCostId(MaintenanceCostDetailsID);
                if (objEquipmentMaintenanceCostDetail != null)
                {
                    if (!String.IsNullOrEmpty(objEquipmentMaintenanceCostDetail.DocumentPath))
                    {
                        string strFullPath = Server.MapPath("../../UploadedImages/AssetManagement/" + this.EquipmentMasterID + "/Accounting/Invoice/" + MaintenanceCostDetailsID + "/") + objEquipmentMaintenanceCostDetail.DocumentPath;
                        Common.DeleteFile(strFullPath);
                    }
                    objAssetMgtRepository.Delete(objEquipmentMaintenanceCostDetail);
                }
                objAssetMgtRepository.SubmitChanges();
                BindAccountingInvoiceGrid(false);
                this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Invoice Deleted Successfully.');", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnSaveInformation_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt64(ddlVendor.SelectedValue) > 0 || !String.IsNullOrEmpty(txtDescription.Value.Trim()) || !String.IsNullOrEmpty(txtInvoiceNumber.Text.Trim()) || Convert.ToInt64(ddlJobCode.SelectedValue) > 0 || !String.IsNullOrEmpty(txtPartsAmount.Text.Trim()) || !String.IsNullOrEmpty(txtLaborAmount.Text.Trim()) || !String.IsNullOrEmpty(txtDateOfService.Text.Trim()) || !String.IsNullOrEmpty(txtTotalInvoiceAmt.Text.Trim()))
            {
                AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
                EquipmentMaintenanceCostDetail objEquipmentMaintenanceCostDetail = new EquipmentMaintenanceCostDetail();
                Int64 InvoiceID = hfInvoiceID.Value.Trim() != "" ? Convert.ToInt64(hfInvoiceID.Value.Trim()) : 0;
                string Mode = "added";
                if (InvoiceID != 0)
                {
                    objEquipmentMaintenanceCostDetail = objAssetMgtRepository.GetByEquipMaintenanceCostId(InvoiceID);
                    Mode = "updated";
                }

                objEquipmentMaintenanceCostDetail.EquipmentMasterID = this.EquipmentMasterID;
                if (Convert.ToInt64(ddlVendor.SelectedValue) > 0)
                    objEquipmentMaintenanceCostDetail.Vendor = Convert.ToInt64(ddlVendor.SelectedValue);
                if (!String.IsNullOrEmpty(txtDateOfService.Text.Trim()))
                    objEquipmentMaintenanceCostDetail.DateofService = Convert.ToDateTime(txtDateOfService.Text.Trim());
                objEquipmentMaintenanceCostDetail.Invoice = txtInvoiceNumber.Text.Trim();
                objEquipmentMaintenanceCostDetail.Description = txtDescription.Value.Trim();

                if (!String.IsNullOrEmpty(txtTotalInvoiceAmt.Text.Trim().Replace("$", "").Trim()))
                    objEquipmentMaintenanceCostDetail.Amount = Convert.ToDecimal(txtTotalInvoiceAmt.Text.Trim().Replace("$", "").Trim());
                if (!String.IsNullOrEmpty(txtLaborAmount.Text.Trim().Replace("$", "").Trim()))
                    objEquipmentMaintenanceCostDetail.LaborAmount = Convert.ToDecimal(txtLaborAmount.Text.Trim().Replace("$", "").Trim());
                if (!String.IsNullOrEmpty(txtPartsAmount.Text.Trim().Replace("$", "").Trim()))
                    objEquipmentMaintenanceCostDetail.PartsAmount = Convert.ToDecimal(txtPartsAmount.Text.Trim().Replace("$", "").Trim());
                if (Convert.ToInt64(ddlJobCode.SelectedValue) > 0)
                    objEquipmentMaintenanceCostDetail.JobCode = Convert.ToInt64(ddlJobCode.SelectedValue);
                string JobSubCOde = string.Empty;
                foreach (ListItem item in cblJobSubCode.Items)
                {
                    if (item.Selected)
                    {
                        if (JobSubCOde != string.Empty)
                            JobSubCOde += ",";
                        JobSubCOde += item.Value;
                    }
                }
                if (JobSubCOde != string.Empty)
                    objEquipmentMaintenanceCostDetail.JobSubCode = JobSubCOde;

                string strFileName = System.DateTime.Now.Ticks + "_" + fuDocument.FileName;
                if (fuDocument.HasFile)
                    objEquipmentMaintenanceCostDetail.DocumentPath = strFileName;

                if (InvoiceID == 0)
                {
                    objAssetMgtRepository.Insert(objEquipmentMaintenanceCostDetail);
                }

                objAssetMgtRepository.SubmitChanges();

                if (fuDocument.HasFile)
                    Common.SaveDocument(Server.MapPath("../../UploadedImages/AssetManagement/" + this.EquipmentMasterID + "/Accounting/Invoice/" + objEquipmentMaintenanceCostDetail.EquipmentMaintenanceCostID), strFileName, true, fuDocument.PostedFile);

                cblJobSubCode.Items.Clear();
                FireCtrlEvent();
                this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Invoice " + Mode + " successfully.'); ResetAllFields('assetspendinginvoice-popup');", true);
                BindAccountingInvoiceGrid(false);
            }
            else
                this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Please enter atleast one value for Invoice.');", true);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindInvoiceDetails(Int64 MaintenanceCostDetailsID)
    {
        try
        {
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            EquipmentMaintenanceCostDetail objEquipmentMaintenanceCostDetail = objAssetMgtRepository.GetByEquipMaintenanceCostId(MaintenanceCostDetailsID);
            if (objEquipmentMaintenanceCostDetail != null)
            {
                //                invoiceHeader.InnerText = title_span.InnerText;
                hfInvoiceID.Value = Convert.ToString(MaintenanceCostDetailsID);
                if (objEquipmentMaintenanceCostDetail.Vendor != null)
                    ddlVendor.SelectedValue = Convert.ToString(objEquipmentMaintenanceCostDetail.Vendor);
                if (objEquipmentMaintenanceCostDetail.DateofService != null)
                    txtDateOfService.Text = objEquipmentMaintenanceCostDetail.DateofService.Value.ToString("MM/dd/yyyy");
                txtInvoiceNumber.Text = Convert.ToString(objEquipmentMaintenanceCostDetail.Invoice);
                txtDescription.Value = Convert.ToString(objEquipmentMaintenanceCostDetail.Description);
                if (objEquipmentMaintenanceCostDetail.Invoice != null)
                    txtTotalInvoiceAmt.Text = Convert.ToString(objEquipmentMaintenanceCostDetail.Amount);
                if (objEquipmentMaintenanceCostDetail.LaborAmount != null)
                    txtLaborAmount.Text = Convert.ToString(objEquipmentMaintenanceCostDetail.LaborAmount);
                if (objEquipmentMaintenanceCostDetail.PartsAmount != null)
                    txtPartsAmount.Text = Convert.ToString(objEquipmentMaintenanceCostDetail.PartsAmount);
                if (objEquipmentMaintenanceCostDetail.JobCode != null)
                    ddlJobCode.SelectedValue = Convert.ToString(objEquipmentMaintenanceCostDetail.JobCode);

                if (!String.IsNullOrEmpty(objEquipmentMaintenanceCostDetail.DocumentPath))
                {
                    lnkbtnDownloadInvoice.Visible = true;
                    lnkbtnDownloadInvoice.CommandArgument = "UploadedImages/AssetManagement/" + EquipmentMasterID + "/Accounting/Invoice/" + MaintenanceCostDetailsID + "/" + objEquipmentMaintenanceCostDetail.DocumentPath;
                    lnkbtnDownloadInvoice.CommandName = objEquipmentMaintenanceCostDetail.DocumentPath;
                }
                else
                {
                    lnkbtnDownloadInvoice.Visible = false;
                }

                BindSubJobCode(Convert.ToInt64(objEquipmentMaintenanceCostDetail.JobCode));
                //Bind SubCode Selection
                if (objEquipmentMaintenanceCostDetail.JobSubCode != null)
                {
                    String strJobSubCode = String.Empty;
                    foreach (ListItem item in cblJobSubCode.Items)
                    {
                        foreach (string selectedValue in objEquipmentMaintenanceCostDetail.JobSubCode.Split(','))
                        {
                            if (item.Value == selectedValue)
                            {
                                item.Selected = true;
                                if (strJobSubCode != String.Empty)
                                    strJobSubCode += "<br />";
                                strJobSubCode += item.Text;
                            }
                        }
                    }
                }

                this.ClientScript.RegisterStartupScript(this.GetType(), "ViewInvoice", "ShowPopUp('assetspendinginvoice-popup', 'assetspendinginvoice-popup .assetspoupup-content','dynamicpopup');", true);
                SetActiveClassToLinkButtonTabs(lnkbtnAccounting, mvAddNewAsset);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindAcquisitionMethodFiles()
    {
        try
        {
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();

            //Bind Lease Files
            List<EquipmentFile> objEquipmentFiles = objAssetMgtRepository.GetEquipmentFilesByEquipmentMasterID(this.EquipmentMasterID, "Lease");
            rpLeaseFiles.DataSource = objEquipmentFiles;
            rpLeaseFiles.DataBind();

            //Bind Purchase Files
            objEquipmentFiles = objAssetMgtRepository.GetEquipmentFilesByEquipmentMasterID(this.EquipmentMasterID, "Purchase");
            rpPurchaseAgreement.DataSource = objEquipmentFiles;
            rpPurchaseAgreement.DataBind();

        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkViewAllInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            InvoicePagingStatus = GetPagingStatus(lnkViewAllInvoice);
            BindAccountingInvoiceGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Bind Custom Fields for each Tabs
    /// strModuleName :- Basic, Accounting, Specifications
    /// </summary>
    /// <param name="strModuleName"></param>
    private void BindCustomFields(string strModuleName, bool IsFromPageLoad)
    {

        try
        {
            if (this.CompanyID > 0)
            {
                AssetMgtRepository objSpecAssetMgtRepository = new AssetMgtRepository();
                int itemCount = 0;
                //string strLiAlignment = "alignleft";
                List<GetAssetSpecificationsByAssetMasterIDCompanyIDResult> objResult = new List<GetAssetSpecificationsByAssetMasterIDCompanyIDResult>();
                if (this.EquipmentMasterID > 0)
                    objResult = objSpecAssetMgtRepository.GetAssetSpecificationsByAssetMasterIDCompanyID(this.EquipmentMasterID, this.CompanyID, strModuleName);

                if (objResult.Count > 0)
                {
                    List<EquipmentLookup> objLookup = new List<EquipmentLookup>();
                    this.EquipmentTypeID = Convert.ToInt64(objResult.FirstOrDefault().EquipmentTypeID);
                    foreach (GetAssetSpecificationsByAssetMasterIDCompanyIDResult objItem in objResult)
                    {
                        if (objItem.ControlType.ToUpper() == "TEXTBOX" || objItem.ControlType.ToUpper() == "CALENDAR")
                        {
                            TextBoxControl objCustomTextBox = (TextBoxControl)LoadControl("~/NewDesign/UserControl/TextBoxControl.ascx");
                            objCustomTextBox.TextBoxID = "txt" + strModuleName + (itemCount + 1);
                            if (objItem.ControlType.ToUpper() == "CALENDAR")
                                objCustomTextBox.TextBoxCssClass = "input-field-all setDatePicker";
                            //input-field-all setDatePicker
                            objCustomTextBox.FieldDetailID = Convert.ToInt64(objItem.FieldDetailID);
                            objCustomTextBox.FieldMasterID = objItem.FieldMasterID;
                            // objCustomTextBox.TextBoxValue = objItem.Description == null ? "" : objItem.Description;
                            objCustomTextBox.Text = objItem.Description == null ? "" : objItem.Description;
                            objCustomTextBox.LabelID = "lbl" + strModuleName + (itemCount + 1);
                            //objCustomTextBox.LiTagCssClassValue = strLiAlignment;
                            objCustomTextBox.Click += new TextBoxControl.ClickHandler(lnkRemoveControl_Click);

                            if (strModuleName.ToLower() == "basic")
                                ul_basicfields.Controls.Add(objCustomTextBox);
                            else if (strModuleName.ToLower() == "accounting")
                                ul_AccountingFields.Controls.Add(objCustomTextBox);
                            else if (strModuleName.ToLower() == "specifications")
                                ul_SpecificationsFields.Controls.Add(objCustomTextBox);

                            //OnClientClick="return confirm('Do you want to remove this field from all assets of this type?')"

                            objCustomTextBox.LabelText = objItem.FieldMasterName;
                            objCustomTextBox.GroupName = objItem.DropDownLookUpCode;
                            objCustomTextBox.ApplyToAll = Convert.ToString(objItem.ApplyToAll);
                            LinkButton lnkbtnRemoveClick = (LinkButton)objCustomTextBox.FindControl("lnkbtnRemoveClick");
                            if (lnkbtnRemoveClick != null && objItem.ApplyToAll)
                                lnkbtnRemoveClick.Attributes.Add("onclick", "return GeneralConfirmationMsgForDelete('Are you sure you want to delete this field from all assets of this type?','" + lnkbtnRemoveClick.ClientID + "',event)");
                            else
                                lnkbtnRemoveClick.Attributes.Add("onclick", "return GeneralConfirmationMsgForDelete('Are you sure you want to delete this field from this asset?','" + lnkbtnRemoveClick.ClientID + "',event)");

                            itemCount++;
                        }
                        else if (objItem.ControlType.ToUpper() == "DROPDOWN")
                        {
                            DropDownControl objDropDown = (DropDownControl)LoadControl("~/NewDesign/UserControl/DropDownControl.ascx");
                            //objDropDown.LiTagCssClassValue = strLiAlignment;
                            objDropDown.LabelID = "lbl" + strModuleName + (itemCount + 1);
                            objDropDown.ID = "ddl" + strModuleName + (itemCount + 1);
                            objDropDown.FieldDetailID = Convert.ToInt64(objItem.FieldDetailID);
                            objDropDown.FieldMasterID = objItem.FieldMasterID;
                            objDropDown.Click += new DropDownControl.ClickHandler(lnkRemoveControl_Click);

                            if (strModuleName.ToLower() == "basic")
                                ul_basicfields.Controls.Add(objDropDown);
                            else if (strModuleName.ToLower() == "accounting")
                                ul_AccountingFields.Controls.Add(objDropDown);
                            else if (strModuleName.ToLower() == "specifications")
                                ul_SpecificationsFields.Controls.Add(objDropDown);


                            CustomDropDown objCustDrop = (CustomDropDown)objDropDown.FindControl("ddlCustomDropDown");
                            objCustDrop.DropDownCssClass = "default";
                            objCustDrop.Module = "Equipmentlookup";
                            objCustDrop.TextBoxCssClass = "input-field-all";
                            objCustDrop.ParentSpanClassToRemove = "select-drop spec-drop";
                            objCustDrop.SaveNewOptionAttempted += new CustomDropDown.SaveNewOptionAttemptedEventHandler(ddlLookUpDropDown_SaveNewOptionAttempted);
                            objCustDrop.SelectedIndexChanged += new CustomDropDown.SelectedIndexChangedHandler(ddlCommonDropDown_SelectedIndexChanged);
                            objCustDrop.GroupName = objItem.DropDownLookUpCode;
                            objCustDrop.ApplyToAll = Convert.ToString(objItem.ApplyToAll);

                            LinkButton lnkbtnRemoveClick = (LinkButton)objDropDown.FindControl("lnkbtnRemoveClick");
                            if (lnkbtnRemoveClick != null && objItem.ApplyToAll)
                                lnkbtnRemoveClick.Attributes.Add("onclick", "return GeneralConfirmationMsgForDelete('Are you sure you want to delete this field from all assets of this type?','" + lnkbtnRemoveClick.ClientID + "',event)");
                            else
                                lnkbtnRemoveClick.Attributes.Add("onclick", "return GeneralConfirmationMsgForDelete('Are you sure you want to delete this field from this asset?','" + lnkbtnRemoveClick.ClientID + "',event)");

                            objLookup = new List<EquipmentLookup>();
                            objLookup = objSpecAssetMgtRepository.GetItemFrmEquipmentLookup(objItem.DropDownLookUpCode).OrderBy(p => p.sLookupName).ToList();
                            BindCustomDropDown(objCustDrop, objLookup, "sLookupName", "iLookupID", "- " + objItem.FieldMasterName + " -", "-1");
                            objDropDown.LabelText = objItem.FieldMasterName;
                            objDropDown.DefaultOptionText = objItem.FieldMasterName;
                            objCustDrop.SelectedValue = objItem.Description;
                            itemCount++;
                        }
                    }
                }

                HtmlGenericControl objLiAddNewField = new HtmlGenericControl("li");
                objLiAddNewField.Attributes.Add("class", "alignright");

                String strNamingContainerClientID = string.Empty;

                if (strModuleName.ToLower() == "basic")
                    strNamingContainerClientID = ul_basicfields.NamingContainer.ClientID;
                else if (strModuleName.ToLower() == "accounting")
                    strNamingContainerClientID = ul_AccountingFields.NamingContainer.ClientID;
                else if (strModuleName.ToLower() == "specifications")
                    strNamingContainerClientID = ul_SpecificationsFields.NamingContainer.ClientID;

                HtmlAnchor objAnchorAddNewField = new HtmlAnchor();
                //            objAnchorAddNewField.ID = "a_" + strModuleName.ToLower() + "_addnewfield";
                objAnchorAddNewField.HRef = "javascript:void(0);";
                objAnchorAddNewField.Title = "ADD NEW FIELD";
                objAnchorAddNewField.InnerText = "ADD NEW FIELD";
                objAnchorAddNewField.Attributes.Add("class", "add-field popup-openlink alignright clientsidectrl");
                objAnchorAddNewField.Attributes.Add("onclick", "ShowAddNewFieldPopUp(this,'Do you want to save your changes?','add-field',event,true);");
                objAnchorAddNewField.Attributes.Add("ontouchstart", "ShowAddNewFieldPopUp(this,'Do you want to save your changes?','add-field',event,true);");
                objAnchorAddNewField.Attributes.Add("data-section", strModuleName);


                objLiAddNewField.Controls.Add(objAnchorAddNewField);

                //Add Li Tag to Ul
                if (strModuleName.ToLower() == "basic")
                    ul_basicfields.Controls.Add(objLiAddNewField);
                else if (strModuleName.ToLower() == "accounting")
                    ul_AccountingFields.Controls.Add(objLiAddNewField);
                else if (strModuleName.ToLower() == "specifications")
                    ul_SpecificationsFields.Controls.Add(objLiAddNewField);

                itemCount++;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void rpAccountingFiles_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            LinkButton lnkbtnDelete = (LinkButton)e.Item.FindControl("lnkbtnDelete");
            if (lnkbtnDelete != null)
                lnkbtnDelete.Attributes.Add("onclick", "return GeneralConfirmationMsgForDelete('Are you sure you want to delete this file?','" + lnkbtnDelete.ClientID + "',event);");
        }
    }
    #endregion

    #region Warranty Tab

    protected void BindWarrantyTabControls()
    {
        try
        {
            BindWarrantyDropDowns();
            BindWarrantyGrid(false);
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    protected void BindWarrantyDropDowns()
    {
        try
        {
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            List<EquipmentLookup> objLookup = new List<EquipmentLookup>();

            //Bind Warranty By
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup("WarrantyBy").OrderBy(p => p.sLookupName).ToList();
            BindCustomDropDown(ddlWarrantyBy, objLookup, "sLookupName", "iLookupID", "- Warranty By -", "-1");

            //Bind Warranty Terms
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup("WarrantyTerms").OrderBy(p => p.sLookupName).ToList();
            BindCustomDropDown(ddlWarantyTerms, objLookup, "sLookupName", "iLookupID", "- Warranty Terms -", "-1");


            //Bind Warranty Type
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup("WarrantyType").OrderBy(p => p.sLookupName).ToList();
            BindCustomDropDown(ddlWarrantyType, objLookup, "sLookupName", "iLookupID", "- Warranty Type -", "-1");

            //Bind Warranty Purchase Conditions
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup("PurchaseConditions");
            BindCustomDropDown(ddlPurchaseCondition, objLookup, "sLookupName", "iLookupID", "- Purchase Conditions -", "-1");
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    protected void BindWarrantyGrid(Boolean FromPaging)
    {
        try
        {

            DataView myDataView = new DataView();
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            DataTable dt = Common.ListToDataTable(objAssetMgtRepository.GetAssetWarranty(EquipmentMasterID, null));
            if (dt.Rows.Count == 0)
            {
                pagingtable.Visible = false;
            }
            else
            {
                pagingtable.Visible = true;
            }

            myDataView = dt.DefaultView;
            if (this.ViewState["SortExp"] != null)
            {
                myDataView.Sort = Convert.ToString(this.ViewState["SortExp"]) + " " + Convert.ToString(this.ViewState["SortOrder"]);
            }

            if (!FromPaging)
            {
                if (this.pds == null)
                    this.pds = new Dictionary<Int64, PagedDataSource>();
                if (this.FrmPg == null)
                    this.FrmPg = new Dictionary<Int64, Int32>();
                if (this.ToPg == null)
                    this.ToPg = new Dictionary<Int64, Int32>();
                if (this.CurrentPage == null)
                    this.CurrentPage = new Dictionary<Int64, Int32>();

                this.FrmPg.Remove((int)GridID.Warranty);
                this.FrmPg.Add((int)GridID.Warranty, 1);

                this.ToPg.Remove((int)GridID.Warranty);
                this.ToPg.Add((int)GridID.Warranty, this.PagerSize);

                this.CurrentPage.Remove((int)GridID.Warranty);
                this.CurrentPage.Add((int)GridID.Warranty, 0);
            }

            PagedDataSource tPds = new PagedDataSource();

            tPds.DataSource = myDataView;

            if (WarrantyPagingStatus.ToUpper() == "VIEW ALL")
                tPds.AllowPaging = true;
            else
                tPds.AllowPaging = false;

            tPds.PageSize = this.PagerSize;
            tPds.CurrentPageIndex = CurrentPage.FirstOrDefault(le => le.Key == (int)GridID.Warranty).Value;

            this.pds.Remove((int)GridID.Warranty);
            this.pds.Add((int)GridID.Warranty, tPds);

            lnkbtnWarrantyNext.Visible = !tPds.IsLastPage;
            lnkbtnWarrantyPrevious.Visible = !tPds.IsFirstPage;

            gvWarranty.DataSource = tPds;
            gvWarranty.DataBind();
            dtlWarrantyPaging.DataSource = doPaging((int)GridID.Warranty);
            dtlWarrantyPaging.DataBind();

            if (gvWarranty.Rows.Count == 0)
                WarrantyPagingTable.Visible = false;
            else
                WarrantyPagingTable.Visible = true;

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindWarrantyClaimGrid(long WarrantyID, GridViewRow gvWarrantyGridRow, Boolean FromPaging)
    {
        try
        {
            DataView myDataView = new DataView();
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            DataTable dt = Common.ListToDataTable(objAssetMgtRepository.GetAssetWarrantyClaimByID(WarrantyID, null));

            GridView gvWarrantyClaim = (GridView)gvWarrantyGridRow.FindControl("gvWarrantyClaim");
            HtmlGenericControl ClaimPagingTable = (HtmlGenericControl)gvWarrantyGridRow.FindControl("ClaimPagingTable");
            DataList dtlClaimPaging = (DataList)gvWarrantyGridRow.FindControl("dtlClaimPaging");
            HtmlAnchor a_PostClaim = (HtmlAnchor)gvWarrantyGridRow.FindControl("a_PostClaim");
            if (a_PostClaim != null)
                a_PostClaim.Attributes.Add("onclick", "SetWarrantyIDToClaim(" + WarrantyID + "," + gvWarrantyGridRow.RowIndex + ");");

            myDataView = dt.DefaultView;
            if (this.ViewState["SortExp"] != null)
            {
                myDataView.Sort = Convert.ToString(this.ViewState["SortExp"]) + " " + Convert.ToString(this.ViewState["SortOrder"]);
            }

            if (!FromPaging)
            {
                if (this.pds == null)
                    this.pds = new Dictionary<Int64, PagedDataSource>();
                if (this.FrmPg == null)
                    this.FrmPg = new Dictionary<Int64, Int32>();
                if (this.ToPg == null)
                    this.ToPg = new Dictionary<Int64, Int32>();
                if (this.CurrentPage == null)
                    this.CurrentPage = new Dictionary<Int64, Int32>();

                this.FrmPg.Remove((int)GridID.Claim);
                this.FrmPg.Add((int)GridID.Claim, 1);

                this.ToPg.Remove((int)GridID.Claim);
                this.ToPg.Add((int)GridID.Claim, this.PagerSize);

                this.CurrentPage.Remove((int)GridID.Claim);
                this.CurrentPage.Add((int)GridID.Claim, 0);
            }

            PagedDataSource tPds = new PagedDataSource();

            tPds.DataSource = myDataView;

            if (ClaimPagingStatus.ToUpper() == "VIEW ALL")
                tPds.AllowPaging = true;
            else
                tPds.AllowPaging = false;

            tPds.PageSize = this.PagerSize;
            tPds.CurrentPageIndex = CurrentPage.FirstOrDefault(le => le.Key == (int)GridID.Claim).Value;

            this.pds.Remove((int)GridID.Claim);
            this.pds.Add((int)GridID.Claim, tPds);

            LinkButton lnkbtnClaimPrevious = (LinkButton)gvWarrantyGridRow.FindControl("lnkbtnClaimPrevious");
            LinkButton lnkbtnClaimNext = (LinkButton)gvWarrantyGridRow.FindControl("lnkbtnClaimNext");

            lnkbtnClaimNext.Visible = !tPds.IsLastPage;
            lnkbtnClaimPrevious.Visible = !tPds.IsFirstPage;

            gvWarrantyClaim.DataSource = tPds;
            gvWarrantyClaim.DataBind();
            dtlClaimPaging.DataSource = doPaging((int)GridID.Claim);
            dtlClaimPaging.DataBind();

            if (gvWarrantyClaim.Rows.Count == 0)
                ClaimPagingTable.Visible = false;
            else
                ClaimPagingTable.Visible = true;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected DateTime GetExpiryDateForWarranty(DateTime? StartDate)
    {
        try
        {
            DateTime objExpireDate = StartDate.HasValue ? Convert.ToDateTime(StartDate) : new DateTime();
            if (rdbtnYears.Checked)
                objExpireDate = objExpireDate.AddYears(txtWarrantyPeriod.Text.Trim() != "" ? Convert.ToInt32(txtWarrantyPeriod.Text.Trim()) : 0);
            else
                objExpireDate = objExpireDate.AddDays(txtWarrantyPeriod.Text.Trim() != "" ? Convert.ToInt32(txtWarrantyPeriod.Text.Trim()) : 0);

            return objExpireDate;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            return new DateTime();
        }
    }

    protected void lnkbtnAddNewWarranty_Click(object sender, EventArgs e)
    {
        try
        {
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            AssetWarranty objAssetWarranty = new AssetWarranty();
            objAssetWarranty.AssetID = EquipmentMasterID;
            objAssetWarranty.WarrantyBy = Convert.ToInt64(ddlWarrantyBy.SelectedValue);
            objAssetWarranty.WarrantyPurchaseCondition = Convert.ToInt64(ddlPurchaseCondition.SelectedValue);
            objAssetWarranty.WarrantyTerms = Convert.ToInt64(ddlWarantyTerms.SelectedValue);
            objAssetWarranty.WarrantyPeriod = txtWarrantyPeriod.Text.Trim() != "" ? Convert.ToInt32(txtWarrantyPeriod.Text.Trim()) : 0;
            objAssetWarranty.WarrantyPeriodType = rdbtnYears.Checked ? "Years" : "Days"; //if Period Type Years selected than Years else Days(By Default it should be Years)
            if (txtWarrantyStartDate.Text.Trim() != "")
            {
                objAssetWarranty.WarrantyStartDate = Convert.ToDateTime(txtWarrantyStartDate.Text.Trim());
                objAssetWarranty.WarrantyExpiryDate = GetExpiryDateForWarranty(objAssetWarranty.WarrantyStartDate);
                objAssetWarranty.DaysToExpire = (objAssetWarranty.WarrantyExpiryDate.Value - objAssetWarranty.WarrantyStartDate.Value).Days;
            }
            objAssetWarranty.WarrantyNotes = txtWarrantyNotes.Value.Trim();
            objAssetWarranty.WarrantyDocument = "";
            objAssetWarranty.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objAssetWarranty.CreatedDate = DateTime.Now;
            objAssetWarranty.StatusID = 135;

            string strFileName = string.Empty;
            if (fuAttachWarranty.Value != "")
            {
                strFileName = System.DateTime.Now.Ticks + "_" + fuAttachWarranty.Value;
                objAssetWarranty.WarrantyDocument = strFileName;
            }

            objAssetMgtRepository.Insert(objAssetWarranty);
            objAssetMgtRepository.SubmitChanges();

            if (fuAttachWarranty.Value != "")
            {
                Common.SaveDocument(Server.MapPath("../../UploadedImages/AssetManagement/Warranty/" + objAssetWarranty.WarrantyID), strFileName, true, fuAttachWarranty.PostedFile);
            }
            BindWarrantyGrid(false);

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnAddNewWarrantyClaim_Click(object sender, EventArgs e)
    {
        try
        {

            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            AssetWarrantyClaim objAssetWarrantyClaim = new AssetWarrantyClaim();
            objAssetWarrantyClaim.WarrantyID = Convert.ToInt64(hfWarrantyIDForClaim.Value);
            if (txtClaimDate.Text.Trim() != "")
                objAssetWarrantyClaim.ClaimDate = Convert.ToDateTime(txtClaimDate.Text.Trim());
            objAssetWarrantyClaim.ClaimEligibleAmount = Convert.ToDecimal(txtClaimEligibleAmount.Text.Trim());
            objAssetWarrantyClaim.ClaimAmount = Convert.ToDecimal(txtClaimAmount.Text.Trim());
            objAssetWarrantyClaim.ClaimNotes = txtClaimNotes.Value.Trim();
            objAssetWarrantyClaim.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objAssetWarrantyClaim.CreatedDate = DateTime.Now;
            objAssetWarrantyClaim.StatusID = 135;
            objAssetWarrantyClaim.WarrantyTypeID = ddlWarrantyType.SelectedValue != "" ? Convert.ToInt64(ddlWarrantyType.SelectedValue) : 0;

            string strFileName = string.Empty;
            if (fuClaim.Value != "")
            {
                strFileName = System.DateTime.Now.Ticks + "_" + fuClaim.Value;
                objAssetWarrantyClaim.ClaimDocument = strFileName;
            }

            objAssetMgtRepository.Insert(objAssetWarrantyClaim);
            objAssetMgtRepository.SubmitChanges();

            if (fuClaim.Value != "")
                Common.SaveDocument(Server.MapPath("../../UploadedImages/AssetManagement/Warranty/" + objAssetWarrantyClaim.WarrantyID + "/Claim/" + objAssetWarrantyClaim.ClaimID), strFileName, true, fuClaim.PostedFile);

            GridViewRow obj = gvWarranty.Rows[Convert.ToInt32(hfRowIndex.Value)];
            BindWarrantyClaimGrid(Convert.ToInt64(objAssetWarrantyClaim.WarrantyID), obj, false);

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvWarranty_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "VIEWWARRANTY")
            {
                AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
                GetAssetWarrantyListOrByIDResult objAssetWarranty = objAssetMgtRepository.GetAssetWarranty(EquipmentMasterID, null).FirstOrDefault();
                if (objAssetWarranty != null)
                {
                    ddlWarrantyBy.SelectedValue = Convert.ToString(objAssetWarranty.WarrantyByID);
                    ddlWarantyTerms.SelectedValue = Convert.ToString(objAssetWarranty.WarrantyTermsID);
                    txtWarrantyNotes.Value = objAssetWarranty.WarrantyNotes;
                    txtWarrantyPeriod.Text = Convert.ToString(objAssetWarranty.WarrantyPeriod);
                    if (objAssetWarranty.WarrantyPeriodType.ToUpper() == "YEARS")
                        rdbtnYears.Checked = true;
                    else
                        rdbtnDays.Checked = true;
                    ddlPurchaseCondition.SelectedValue = Convert.ToString(objAssetWarranty.WarrantyPurchaseConditionID);
                    txtWarrantyStartDate.Text = objAssetWarranty.WarrantyStartDate != null ? Convert.ToDateTime(objAssetWarranty.WarrantyStartDate).ToString("MM/dd/yyyy") : "";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "ShowPopUp('new-warranty-popup', 'new-warranty-popup .warranty-content','viewwarranty');", true);
                }
            }
            else if (e.CommandName.ToUpper() == "VIEW")
            {
                GridViewRow gvWarrantyGridRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HtmlGenericControl dvClaim = (HtmlGenericControl)gvWarrantyGridRow.FindControl("dvClaim");
                LinkButton lnkbtn = (LinkButton)e.CommandSource;
                dvClaim.Visible = !dvClaim.Visible;

                if (lnkbtn != null && dvClaim.Visible)
                {

                    lnkbtn.Text = "<span>HIDE</span>";
                    lnkbtn.CssClass = "gray-button";
                    if (gvWarrantyGridRow != null)
                        BindWarrantyClaimGrid(Convert.ToInt64(e.CommandArgument), gvWarrantyGridRow, false);
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "ScrollToTag('" + lnkbtn.ClientID + "',false);", true);
                }
                else if (lnkbtn != null)
                {
                    lnkbtn.Text = "<span>VIEW</span>";
                    lnkbtn.CssClass = "gray-button";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "ScrollToTag('" + lnkbtn.ClientID + "',false);", true);
                }
            }
            else if (e.CommandName.ToUpper() == "WARRANTYDELETE")
            {
                GridViewRow gvWarrantyGridRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                //HtmlGenericControl dvClaim = (HtmlGenericControl)gvWarrantyGridRow.FindControl("dvClaim");
                LinkButton lnkbtn = (LinkButton)e.CommandSource;
                Int64 WarrantyID = Convert.ToInt64(lnkbtn.CommandArgument);
                AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
                objAssetMgtRepository.DeleteWarrantyClaimByID(WarrantyID);
                Common.DeleteDirectory(Server.MapPath("../../UploadedImages/AssetManagement/Warranty/" + WarrantyID));
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnClaimAddNote_Click(object sender, EventArgs e)
    {
        try
        {

            string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.AssetWarrantyClaim);
            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();
            objComNot.Notecontents = txtClaimNoteDetails.Value.Trim(); ;
            objComNot.NoteFor = strNoteFor;
            objComNot.ForeignKey = Convert.ToInt64(hfWarrantyClaimID.Value);
            objComNot.SpecificNoteFor = Convert.ToString(this.EquipmentMasterID);
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objCompNoteHistRepos.Insert(objComNot);
            objCompNoteHistRepos.SubmitChanges();
            this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "GeneralAlertMsg('Claim notes added successfully.');", true);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnViewClaimNotes_Click(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        if (gvRow != null)
        {
            HiddenField hfGridWarrantyClaimID = (HiddenField)gvRow.FindControl("hfGridWarrantyClaimID");
            if (hfGridWarrantyClaimID != null && !String.IsNullOrEmpty(hfGridWarrantyClaimID.Value))
            {
                NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();
                string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.AssetWarrantyClaim);
                var objNoteDetailResult = objCompNoteHistRepos.GetByNoteForAndForeignKey(Convert.ToInt64(hfGridWarrantyClaimID.Value), strNoteFor);
                rpWarrantyClaimNotes.DataSource = objNoteDetailResult;
                rpWarrantyClaimNotes.DataBind();
                if (objNoteDetailResult.Count > 0)
                    li_EmptyClaimNotes.Visible = false;
                else
                    li_EmptyClaimNotes.Visible = true;
                this.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "ShowPopUp('claim-view-notes-popup', 'claim-view-notes-popup .warranty-content','viewclaimnotes');", true);
            }
        }
    }

    protected void gvWarrantyClaim_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "VIEWCLAIM")
            {
                AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
                GetAssetWarrantyClaimResult objAssetWarrantyClaim = objAssetMgtRepository.GetAssetWarrantyClaimByID(0, 0).FirstOrDefault();
                if (objAssetWarrantyClaim != null)
                {
                    //ddlWarrantyType.SelectedValue = Convert.ToString(objAssetWarrantyClaim.WarrantyT);
                    txtClaimDate.Text = objAssetWarrantyClaim.ClaimDate != null ? Convert.ToDateTime(objAssetWarrantyClaim.ClaimDate).ToString("MM/dd/yyyy") : "";
                    txtClaimAmount.Text = objAssetWarrantyClaim.ClaimAmount != null ? Convert.ToString(Decimal.Round(Convert.ToDecimal(objAssetWarrantyClaim.ClaimAmount), 2)) : "";
                    txtClaimEligibleAmount.Text = objAssetWarrantyClaim.ClaimEligibleAmount != null ? Convert.ToString(Decimal.Round(Convert.ToDecimal(objAssetWarrantyClaim.ClaimEligibleAmount), 2)) : "";
                    //txtWarrantyPeriod.Text = Convert.ToString(objAssetWarranty.WarrantyPeriod);
                    //if (objAssetWarranty.WarrantyPeriodType.ToUpper() == "YEARS")
                    //    rdbtnYears.Checked = true;
                    //else
                    //    rdbtnDays.Checked = true;
                    //ddlPurchaseCondition.SelectedValue = Convert.ToString(objAssetWarranty.WarrantyPurchaseConditionID);
                    //txtWarrantyStartDate.Text = objAssetWarranty.WarrantyStartDate != null ? Convert.ToDateTime(objAssetWarranty.WarrantyStartDate).ToString("MM/dd/yyyy") : "";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "ShowPopUp('new-warranty-popup', 'new-warranty-popup .warranty-content','viewclaim');", true);
                }
            }
            else if (e.CommandName.ToUpper() == "CLAIMDELETE")
            {
                //GridViewRow gvWarrantyGridRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                ////HtmlGenericControl dvClaim = (HtmlGenericControl)gvWarrantyGridRow.FindControl("dvClaim");
                //LinkButton lnkbtn = (LinkButton)e.CommandSource;
                //Int64 WarrantyID = Convert.ToInt64(lnkbtn.CommandArgument);
                //AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
                //objAssetMgtRepository.DeleteWarrantyClaimByID(WarrantyID);
                //Common.DeleteDirectory(Server.MapPath("../../UploadedImages/AssetManagement/Warranty/" + WarrantyID));
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkViewAllWarranty_Click(object sender, EventArgs e)
    {
        try
        {
            WarrantyPagingStatus = GetPagingStatus(lnkViewAllInvoice);
            BindWarrantyGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkViewAllClaim_Click(object sender, EventArgs e)
    {
        try
        {
            ClaimPagingStatus = GetPagingStatus(lnkViewAllInvoice);
            LinkButton lnkbtn = (LinkButton)sender;
            GridViewRow gvWarrantyGridRow = (GridViewRow)(lnkbtn.NamingContainer);
            if (gvWarrantyGridRow != null)
                BindWarrantyClaimGrid(Convert.ToInt64(lnkbtn.CommandArgument), gvWarrantyGridRow, false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region  Specs Tab

    private void BindSpecifiationFiles()
    {
        try
        {
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            List<EquipmentFile> objEquipmentFiles = objAssetMgtRepository.GetEquipmentFilesByEquipmentMasterID(this.EquipmentMasterID, "Specification");
            if (objEquipmentFiles != null && objEquipmentFiles.Count > 0)
            {
                List<EquipmentFile> objImagesAndVideos = objEquipmentFiles.Where(r => r.FileType.ToLower().Contains("image") || r.FileType.ToLower().Contains("video")).ToList();
                if (objImagesAndVideos != null && objImagesAndVideos.Count > 0)
                {
                    AssetImage_div.Visible = true;
                    rpAssetImage.DataSource = objImagesAndVideos;
                    rpAssetImage.DataBind();
                }
                else
                    AssetImage_div.Visible = false;

                List<EquipmentFile> objAssetManuals = objEquipmentFiles.Where(r => r.FileType.ToLower().Contains("document")).ToList();
                if (objAssetManuals != null && objAssetManuals.Count > 0)
                {
                    AssetManuals_div.Visible = true;
                    rpAssetManuals.DataSource = objAssetManuals;
                    rpAssetManuals.DataBind();
                }
                else
                    AssetManuals_div.Visible = false;
            }
            else
            {
                AssetImage_div.Visible = false;
                AssetManuals_div.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }

    protected void lnkbtnSaveSpecifications_Click(object sender, EventArgs e)
    {
        try
        {
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();

            //Save Custom Added Fields and Its Value
            SaveEquipmentFieldsValue(objAssetMgtRepository, ul_SpecificationsFields);

            String strFileName = String.Empty;
            String strFileType = String.Empty;
            HttpFileCollection hfc = Request.Files;
            EquipmentFile objEquipmentFile = new EquipmentFile();
            String strName = String.Empty;
            for (int i = 0; i < hfc.Count; i++)
            {
                HttpPostedFile hpf = hfc[i];
                if (hpf.ContentLength > 0)
                {
                    strFileType = Common.GetFileType(hpf.ContentType);
                    if (strFileType.ToLower() == "image")
                        strFileName = "Image Name";
                    else if (strFileType.ToLower() == "video")
                        strFileName = "Video Name";
                    else
                        strFileName = "Name";

                    strName = System.DateTime.Now.Ticks + "_" + hpf.FileName;

                    Common.SaveDocument(Server.MapPath("../../UploadedImages/AssetManagement/Specifications/"), strName, true, hpf);

                    objEquipmentFile = new EquipmentFile();
                    objEquipmentFile.EquipmentMasterID = this.EquipmentMasterID;
                    objEquipmentFile.FileType = strFileType;
                    objEquipmentFile.FileFor = "Specification";
                    objEquipmentFile.Name = strName;
                    objEquipmentFile.FileTitle = strFileName;
                    objEquipmentFile.FileDate = DateTime.Now;
                    objEquipmentFile.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                    objEquipmentFile.CreatedDate = DateTime.Now;
                    objAssetMgtRepository.Insert(objEquipmentFile);
                }
            }


            if (hfFieldIds.Value != String.Empty)
            {
                //  objAssetMgtRepository.DeleteFieldDetailFieldMasterByID(hfFieldIds.Value);
                hfFieldIds.Value = String.Empty;
            }

            objAssetMgtRepository.SubmitChanges();

            SetActiveClassToLinkButtonTabs(lnkbtnRegistration, mvAddNewAsset); //Set Warranty Tab as Active

            //Reset the change value
            ResetCtrlInfoChangedValue(Convert.ToString(lnkbtnSpecs.Attributes["data-divID"]));
            //ul_SpecificationsFields.Controls.Clear();
            //BindCustomFields("Specifications", false);
            //BindSpecifiationFiles();

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Paging in the Accounting Invoice GridView

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            string[] ArgumentValues = Convert.ToString(e.CommandArgument).Split(';');
            if (ArgumentValues.Length > 0)
            {
                int iGridID = Convert.ToInt32(ArgumentValues[1]);
                CurrentPage[iGridID] = Convert.ToInt16(Convert.ToString(ArgumentValues[0]));

                if (iGridID == (int)GridID.Invoice)
                    BindAccountingInvoiceGrid(true);
                else if (iGridID == (int)GridID.Warranty)
                    BindWarrantyGrid(true);
                //else
                //    BindWarrantyClaimGrid();
            }
        }


    }

    protected void dtlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        string[] ArgumentValues = Convert.ToString(lnkbtnPage.CommandArgument).Split(';');
        if (ArgumentValues.Length > 0)
        {
            int iGridID = Convert.ToInt32(ArgumentValues[1]);
            if (lnkbtnPage.Text == Convert.ToString(CurrentPage[iGridID] + 1))
            {
                lnkbtnPage.Enabled = false;
                lnkbtnPage.Font.Bold = true;
            }
        }
    }

    private Dictionary<Int64, PagedDataSource> pds
    {
        get
        {
            if (Convert.ToString(Session["pds"]) == String.Empty)
                return null;
            else
                return (Dictionary<Int64, PagedDataSource>)Session["pds"];
        }
        set
        {
            Session["pds"] = value;
        }
    }

    private Dictionary<Int64, Int32> CurrentPage
    {
        get
        {
            if (Convert.ToString(ViewState["CurrentPage"]) == String.Empty)
                return null;
            else
                return (Dictionary<Int64, Int32>)this.ViewState["CurrentPage"];
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }

    private Dictionary<Int64, Int32> FrmPg
    {
        get
        {
            if (Convert.ToString(ViewState["FrmPg"]) == String.Empty)
                return null;
            else
                return (Dictionary<Int64, Int32>)this.ViewState["FrmPg"];
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }

    private Dictionary<Int64, Int32> ToPg
    {
        get
        {
            if (Convert.ToString(ViewState["ToPg"]) == String.Empty)
                return null;
            else
                return (Dictionary<Int64, Int32>)this.ViewState["ToPg"];
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }

    private Int32 PagerSize
    {
        get
        {
            if (Convert.ToString(this.ViewState["PagerSize"]) == "")
                return Convert.ToInt32(Application["NEWPAGESIZE"]);
            else
                return Convert.ToInt32(this.ViewState["PagerSize"]);
        }
    }

    private DataTable doPaging(int GridStatusID)
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            Int32 tCurrentPg = this.pds[GridStatusID].CurrentPageIndex + 1;
            Int32 tToPg = this.PagerSize;
            Int32 tFrmPg = this.FrmPg[GridStatusID];

            if (tCurrentPg > tToPg)
            {
                tFrmPg = tToPg + 1;
                tToPg = this.PagerSize;
            }
            if (tCurrentPg < tFrmPg)
            {
                tToPg = tFrmPg - 1;
                tFrmPg = tFrmPg - 5;

            }

            if (tToPg > this.pds[GridStatusID].PageCount)
                tToPg = this.pds[GridStatusID].PageCount;

            for (int i = tFrmPg - 1; i < tToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            return dt;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            return new DataTable();
        }

    }

    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        LinkButton lnkbtn = (LinkButton)sender;
        if (lnkbtn != null && lnkbtn.ToolTip.ToLower() == "invoice")
        {
            this.CurrentPage[1] -= 1;
            BindAccountingInvoiceGrid(true);
        }
        else if (lnkbtn != null && lnkbtn.ToolTip.ToLower() == "warranty")
        {
            this.CurrentPage[2] -= 1;
            BindWarrantyGrid(true);
        }
        else
        {
            GridViewRow gvWarrantyGridRow = (GridViewRow)(lnkbtn.NamingContainer);
            if (gvWarrantyGridRow != null)
            {
                this.CurrentPage[3] -= 1;
                BindWarrantyClaimGrid(Convert.ToInt64(lnkbtn.CommandArgument), gvWarrantyGridRow, true);
            }
        }
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        LinkButton lnkbtn = (LinkButton)sender;
        if (lnkbtn != null && lnkbtn.ToolTip.ToLower() == "invoice")
        {
            this.CurrentPage[1] += 1;
            BindAccountingInvoiceGrid(true);
        }
        else if (lnkbtn != null && lnkbtn.ToolTip.ToLower() == "warranty")
        {
            this.CurrentPage[2] += 1;
            BindWarrantyGrid(true);
        }
        else
        {
            GridViewRow gvWarrantyGridRow = (GridViewRow)(lnkbtn.NamingContainer);
            if (gvWarrantyGridRow != null)
            {
                this.CurrentPage[3] += 1;
                BindWarrantyClaimGrid(Convert.ToInt64(lnkbtn.CommandArgument), gvWarrantyGridRow, true);
            }
        }
    }

    #endregion

    #region History Tab

    protected void BindHistoryNotes()
    {
        AssetMgtRepository objAssetMgtRepos = new AssetMgtRepository();
        // rpNotes.DataSource = objAssetMgtRepos.GetNotesForHistoryTab(); 
        //rpNotes.DataBind();
    }
    #endregion
}

