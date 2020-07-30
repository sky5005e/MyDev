using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_ServiceTicketCenter_ServiceTicketDetail : PageBase
{
    #region Page Properties

    int sizeCount = 0;

    Int64 ServiceTicketID
    {
        get
        {
            if (ViewState["ServiceTicketID"] == null)
            {
                ViewState["ServiceTicketID"] = 0;
            }
            return Convert.ToInt64(ViewState["ServiceTicketID"]);
        }
        set
        {
            ViewState["ServiceTicketID"] = value;
        }
    }

    String SubOwner
    {
        get
        {
            return Convert.ToString(ViewState["SubOwner"]);
        }
        set
        {
            ViewState["SubOwner"] = value;
        }
    }

    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Support Ticket Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Support Ticket Detail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.SearchTicketURL != null ? IncentexGlobal.SearchTicketURL.Contains("ex=1") ? IncentexGlobal.SearchTicketURL : IncentexGlobal.SearchTicketURL.Contains("?") ? IncentexGlobal.SearchTicketURL + "&ex=1" : IncentexGlobal.SearchTicketURL + "?ex=1" : "~/admin/ServiceTicketcenter/SearchResult.aspx";

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])))
            {
                vw_ServiceTicket objServiceTicket = new vw_ServiceTicket();

                ServiceTicketID = Convert.ToInt64(Request.QueryString["id"]);
                objServiceTicket = new ServiceTicketRepository().GetFirstByID(ServiceTicketID);

                if (objServiceTicket != null)
                {
                    FillServiceTicketStatus();
                    FillTypeOfRequest();

                    FillTicketOwner(objServiceTicket.ServiceTicketOwnerID);

                    if (objServiceTicket.ContactID != null)
                    {
                        lblContact.Text = Convert.ToString(objServiceTicket.ContactName);
                        trContact.Visible = true;
                        lblCustomerEmail.Text = Convert.ToString(objServiceTicket.ContactEmail);
                        trCustomerEmail.Visible = true;
                        lblTelephone.Text = Convert.ToString(objServiceTicket.ContactTelephone);
                        trTelephone.Visible = true;
                    }
                    else if (objServiceTicket.SupplierID != null)
                    {
                        lblContact.Text = String.Empty;
                        trContact.Visible = false;
                        lblCustomerEmail.Text = Convert.ToString(objServiceTicket.SupplierEmail);
                        trCustomerEmail.Visible = true;
                        lblTelephone.Text = Convert.ToString(objServiceTicket.SupplierTelephone);
                        trTelephone.Visible = true;
                    }
                    else
                    {
                        lblContact.Text = String.Empty;
                        trContact.Visible = false;
                        lblCustomerEmail.Text = String.Empty;
                        trCustomerEmail.Visible = false;
                        lblTelephone.Text = String.Empty;
                        trTelephone.Visible = false;
                    }

                    if (objServiceTicket.CompanyID != null)
                    {
                        lblCustomer.Text = Convert.ToString(objServiceTicket.CompanyName);
                    }
                    else if (objServiceTicket.SupplierID != null)
                    {
                        lblCustSupp.InnerText = "Supplier :";
                        lblCustSuppEmail.InnerText = "Supplier Email :";
                        lblCustomer.Text = Convert.ToString(objServiceTicket.SupplierName);
                        spanCustSupp.InnerText = "Supplier";
                        spanNoteCustSupp.InnerHtml = @"
                                            <img src='../../Images/errorpage.png' height='25px' width='25px' alt='note:' />&nbsp;&nbsp;YOU
                                            ARE ABOUT TO SEND A NOTE TO A SUPPLIER ";
                        spanIENote.InnerHtml = @"
                                            <img src='../../Images/errorpage.png' height='25px' width='25px' alt='note:' />&nbsp;&nbsp;
                                            You are about to post an Incentex Internal Note.
                                            <br />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Supplier will not be
                                            able to view this note. ";
                        spanCustAdminsSuppEmp.InnerText = "Supplier Employees";
                        h4CASupp.InnerText = "Supplier Employees";
                    }
                    else
                    {
                        lblCustomer.Text = Convert.ToString(objServiceTicket.TicketEmail);
                    }

                    //if (!String.IsNullOrEmpty(objServiceTicket.ServiceTicketName))
                    //    lblServiceTicketName.Text = Convert.ToString(objServiceTicket.ServiceTicketName).Length > 25 ? Convert.ToString(objServiceTicket.ServiceTicketName).Substring(0, 25).Trim() + "..." : Convert.ToString(objServiceTicket.ServiceTicketName);
                    //lblServiceTicketName.ToolTip = Convert.ToString(objServiceTicket.ServiceTicketName);
                    txtServiceTicketName.Text = Convert.ToString(objServiceTicket.ServiceTicketName);
                    txtServiceTicketName.ToolTip = Convert.ToString(objServiceTicket.ServiceTicketName);
                    lblServiceTicketNumber.Text = Convert.ToString(objServiceTicket.ServiceTicketNumber);
                    lblServiceTicketOwner.Text = Convert.ToString(objServiceTicket.FirstName + " " + objServiceTicket.LastName);
                    lblServiceTicketReason.Text = Convert.ToString(objServiceTicket.Reason + " ");
                    lblStartDate.Text = Convert.ToString(objServiceTicket.StartDateNTime);
                    txtDatePromised.Text = Convert.ToString(objServiceTicket.DatePromisedFormatted);

                    ddlServiceTicketStatus.SelectedValue = Convert.ToString(objServiceTicket.TicketStatusID);

                    if (objServiceTicket.TypeOfRequestID != null)
                    {
                        ddlTypeOfRequest.SelectedValue = Convert.ToString(objServiceTicket.TypeOfRequestID);
                    }

                    ddlTicketOwner.SelectedValue = Convert.ToString(objServiceTicket.ServiceTicketOwnerID);
                    lblEndDate.Text = Convert.ToString(objServiceTicket.EndDateNTime);

                    imgFlag.Src = new ServiceTicketRepository().GetTicketFlag(objServiceTicket.ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID) != null ? "~/Images/flag_red.png" : "~/Images/flag_gray.png";
                    imgFlag.Alt = "flag";

                    imgAddNoteCustSupp.Src = objServiceTicket.SupplierID == null ? "~/Images/new_red_btn.png" : "~/Images/new_red_supp_btn.png";
                    imgAddNoteCustSupp.Alt = objServiceTicket.SupplierID == null ? "+ Add Customer View Note" : "+ Add Supplier View Note";

                    List<ServiceTicketAttachment> objAttachmentList = new List<ServiceTicketAttachment>() { };
                    objAttachmentList = new ServiceTicketAttachmentRepository().GetByServiceTicektId(this.ServiceTicketID);

                    List<UploadFile> objFileList = new List<UploadFile>();

                    foreach (ServiceTicketAttachment objAttachment in objAttachmentList)
                    {
                        UploadFile objFile = new UploadFile();
                        if (ViewState["ListValues"] != null)
                        {
                            objFileList = (List<UploadFile>)ViewState["ListValues"];
                            objFile.OnlyFileName = Convert.ToString(objAttachment.AttachmentFileName);
                            objFile.SavedFileName = Convert.ToString(objAttachment.SavedFileName);
                            objFile.AttachmentID = Convert.ToInt64(objAttachment.AttachmentID);
                            objFileList.Add(objFile);
                            ViewState["ListValues"] = objFileList;
                        }
                        else
                        {
                            objFile.OnlyFileName = Convert.ToString(objAttachment.AttachmentFileName);
                            objFile.SavedFileName = Convert.ToString(objAttachment.SavedFileName);
                            objFile.AttachmentID = Convert.ToInt64(objAttachment.AttachmentID);
                            objFileList.Add(objFile);
                            ViewState["ListValues"] = objFileList;
                        }
                    }

                    //For marking all unread notes as read
                    new ServiceTicketRepository().UpdateNoteReadFlag(objServiceTicket.ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, new ServiceTicketRepository().GetUnreadNotes(objServiceTicket.ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID).ToList());
                    DisplaySubOwners();

                    if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    {
                        hdnCompanyID.Value = Convert.ToString(objServiceTicket.CompanyID);
                        hdnCustomerID.Value = Convert.ToString(objServiceTicket.ContactID);
                        hdnSupplierID.Value = objServiceTicket.SupplierID != null ? Convert.ToString(new SupplierRepository().GetById(Convert.ToInt64(objServiceTicket.SupplierID)).UserInfoID) : "";
                        FillNotificationParties();

                        #region For note insertion

                        String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);

                        NoteDetail objComNot = new NoteDetail();
                        NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();

                        objComNot.Notecontents = "Viewed Support Ticket.";
                        objComNot.NoteFor = strNoteFor;
                        objComNot.SpecificNoteFor = "IEActivity";
                        objComNot.ForeignKey = objServiceTicket.ServiceTicketID;
                        objComNot.CreateDate = System.DateTime.Now;
                        objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                        objComNot.UpdateDate = System.DateTime.Now;
                        objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

                        objCompNoteHistRepos.Insert(objComNot);
                        objCompNoteHistRepos.SubmitChanges();

                        #endregion

                        DisplayNotes(objServiceTicket.SupplierID == null ? 1 : 3);

                        chkPostedNotes.Checked = true;
                        chkPostedNotes_CheckedChanged(null, null);
                    }
                    else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SupplierEmployee))
                    {
                        dvCustomerNote.Visible = false;
                        spanAddNote.InnerText = "+ Add Note";
                        spanNoteLabel.InnerText = "Notes/History :";
                        modalAddnotesIE.PopupControlID = pnlNoteSupp.ID;
                        modalAddnotesIE.CancelControlID = closeSuppPopup.ID;
                        tblPostedNotesOnly.Visible = false;
                        DisplaySupplierNotes();
                    }
                }
            }
        }
        BindFilesFromViewState();
        lblMsg.Text = "";
    }

    #endregion

    #region Page Control Events

    protected void lnkButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (!(String.IsNullOrEmpty(txtNote.Text.Trim())))
            {
                String NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(lblCustSupp.InnerText.Contains("Customer") ? Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs : Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketSupps);
                ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
                objSerTicRep.InsertTicketNote(this.ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtNote.Text.Trim()), NoteFor, null);
                SendEMailToAnnCECAIESuppSE();
                txtNote.Text = "";
                DisplayNotes(lblCustSupp.InnerText.Contains("Customer") ? 1 : 3);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkNoteSupp_Click(object sender, EventArgs e)
    {
        try
        {
            if (!(String.IsNullOrEmpty(txtNoteSupp.Text.Trim())))
            {
                String NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketSupps);
                ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
                objSerTicRep.InsertTicketNote(this.ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtNoteSupp.Text.Trim()), NoteFor, null);
                SendEmailToIESuppSE();
                txtNoteSupp.Text = "";
                DisplaySupplierNotes();
            }
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
            if (!(String.IsNullOrEmpty(txtNoteIE.Text.Trim())))
            {
                String NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);
                ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
                objSerTicRep.InsertTicketNote(this.ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtNoteIE.Text.Trim()), NoteFor, "IEInternalNotes");
                SendEMailToIE();
                txtNoteIE.Text = "";
                chkPostedNotes_CheckedChanged(null, null);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlServiceTicketStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        if (!String.IsNullOrEmpty(ddlServiceTicketStatus.SelectedValue))
        {
            LookupRepository objLookupRepo = new LookupRepository();

            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            ServiceTicket objServiceTicket = objSerTicRep.GetByTicketID(this.ServiceTicketID);

            if (objServiceTicket != null)
            {
                objServiceTicket.TicketStatusID = Convert.ToInt64(ddlServiceTicketStatus.SelectedValue);
                if (Convert.ToInt64(ddlServiceTicketStatus.SelectedValue) == objLookupRepo.GetIdByLookupNameNLookUpCode("Closed", "ServiceTicketStatus"))
                {
                    objServiceTicket.EndDate = DateTime.Now;
                    SendCloseTicketNotification();
                }
                else
                {
                    objServiceTicket.EndDate = null;
                }
                objServiceTicket.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objServiceTicket.UpdatedDate = DateTime.Now;
                objSerTicRep.SubmitChanges();

                vw_ServiceTicket objTemp = objSerTicRep.GetFirstByID(this.ServiceTicketID);

                if (objTemp != null)
                    lblEndDate.Text = objTemp.EndDateNTime;

                LogAction("Changed ticket status to : " + ddlServiceTicketStatus.SelectedItem.Text + ".", "IEActivity");
                lblMsg.Text = "Support ticket status changed successfully.";
            }
        }
    }

    protected void ddlTicketOwner_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        if (!String.IsNullOrEmpty(ddlTicketOwner.SelectedValue))
        {
            lnkOwnerToDo.Text = ddlTicketOwner.SelectedItem.Text;
            lblServiceTicketOwner.Text = ddlTicketOwner.SelectedItem.Text;
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            ServiceTicket objServiceTicket = objSerTicRep.GetByTicketID(ServiceTicketID);

            if (objServiceTicket != null)
            {
                if (objServiceTicket.ServiceTicketOwnerID == null)
                {
                    ddlTicketOwner.Items.RemoveAt(0);
                }

                objServiceTicket.ServiceTicketOwnerID = Convert.ToInt64(ddlTicketOwner.SelectedValue);
                objServiceTicket.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objServiceTicket.UpdatedDate = DateTime.Now;
                objSerTicRep.SubmitChanges();

                ddlTicketOwner.DataBind();

                LogAction("Changed ticket owner to : " + ddlTicketOwner.SelectedItem.Text + ".", "IEActivity");
                lblMsg.Text = "Support ticket owner changed successfully.";
            }
        }
    }

    protected void ddlTypeOfRequest_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        if (!String.IsNullOrEmpty(ddlTypeOfRequest.SelectedValue))
        {
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            ServiceTicket objServiceTicket = objSerTicRep.GetByTicketID(ServiceTicketID);

            if (objServiceTicket != null)
            {
                if (objServiceTicket.TypeOfRequestID == null)
                {
                    ddlTypeOfRequest.Items.RemoveAt(0);
                }

                objServiceTicket.TypeOfRequestID = Convert.ToInt64(ddlTypeOfRequest.SelectedValue);
                objServiceTicket.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objServiceTicket.UpdatedDate = DateTime.Now;
                objSerTicRep.SubmitChanges();

                ddlTypeOfRequest.DataBind();

                LogAction("Changed type of request to : " + ddlTypeOfRequest.SelectedItem.Text + ".", "IEActivity");
                lblMsg.Text = "Type of request changed successfully.";
            }
        }
    }

    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(fpAttachment.Value))
        {
            lblAttachmentMsg.Visible = true;
            lblAttachmentMsg.Text = "";
            if (Convert.ToInt64(ViewState["CountSize"]) > 18000)
            {
                lblAttachmentMsg.Text = "Attachment(s) must be less then 18 MB";
            }
            else
            {
                String SavedFileName = "serviceticket_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fpAttachment.Value;
                SavedFileName = Common.MakeValidFileName(SavedFileName);
                String filePath = Common.ServieTicketAttachmentPath + SavedFileName;

                sizeCount = sizeCount + (fpAttachment.PostedFile.ContentLength / 1024);
                ViewState["CountSize"] = Convert.ToInt32(ViewState["CountSize"]) + sizeCount;
                if (!String.IsNullOrEmpty(fpAttachment.Value))
                {
                    fpAttachment.PostedFile.SaveAs(filePath);
                }

                ServiceTicketAttachment objServiceTicketAttachemt = new ServiceTicketAttachment();
                objServiceTicketAttachemt.AttachmentFileName = fpAttachment.Value;
                objServiceTicketAttachemt.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objServiceTicketAttachemt.CreatedDate = DateTime.Now;
                objServiceTicketAttachemt.SavedFileName = SavedFileName;
                objServiceTicketAttachemt.ServiceTicketID = this.ServiceTicketID;
                ServiceTicketAttachmentRepository objSerTicAttRep = new ServiceTicketAttachmentRepository();
                objSerTicAttRep.Insert(objServiceTicketAttachemt);
                objSerTicAttRep.SubmitChanges();

                GenerateTableGrid(fpAttachment.Value, SavedFileName, objServiceTicketAttachemt.AttachmentID);
                LogAction("Uploaded an attachment : " + objServiceTicketAttachemt.AttachmentFileName, "IEActivity");
                lblAttachmentMsg.Text = "Attachment uploaded successfully.";
            }
        }

        modalAttachments.Show();
    }

    protected void grvAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow row;
        row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
        List<UploadFile> objFileList = new List<UploadFile>();

        if (ViewState["ListValues"] != null)
        {
            objFileList = (List<UploadFile>)ViewState["ListValues"];

            if (File.Exists(Common.ServieTicketAttachmentPath + objFileList[row.RowIndex].SavedFileName))
            {
                if (e.CommandName == "DeleteAttachment")
                {
                    //File.Delete(Common.ServieTicketAttachmentPath + objFileList[row.RowIndex].SavedFileName);

                    if (!base.CanDelete)
                    {
                        base.RedirectToUnauthorised();
                    }

                    ServiceTicketAttachmentRepository objSerTicAttRep = new ServiceTicketAttachmentRepository();
                    objSerTicAttRep.DeleteByAttachmentID(objFileList[row.RowIndex].AttachmentID, IncentexGlobal.CurrentMember.UserInfoID);
                    objSerTicAttRep.SubmitChanges();
                    LogAction("Deleted an attachment : " + objFileList[row.RowIndex].OnlyFileName, "IEActivity");
                    objFileList.RemoveAt(row.RowIndex);
                    ViewState["ListValues"] = objFileList;
                    BindFilesFromViewState();
                    modalAttachments.Show();
                }

                if (e.CommandName == "download")
                {
                    String filePath = Common.ServieTicketAttachmentPath + objFileList[row.RowIndex].SavedFileName;
                    DownloadFile(filePath, objFileList[row.RowIndex].OnlyFileName);
                }
            }
            else
            {
                BindFilesFromViewState();
                lblAttachmentMsg.Visible = true;
                lblAttachmentMsg.Text = "File not found.";
                modalAttachments.Show();
            }
        }
    }

    protected void dtlSubOwners_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "ToDo")
        {
            LinkButton lnkSubOwnerToDo = (LinkButton)e.Item.FindControl("lnkSubOwnerToDo");
            this.SubOwner = lnkSubOwnerToDo.Text;
            hdnTempID.Value = Convert.ToString(Convert.ToInt64(e.CommandArgument));
            BindTodoGrid();
            lblToDoMsg.Text = "";
            lblToDoMsg.Visible = false;
        }
    }

    protected void lnkOwnerToDo_Click(object sender, EventArgs e)
    {
        ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
        ServiceTicket objServiceTicket = objSerTicRep.GetByTicketID(this.ServiceTicketID);

        if (objServiceTicket != null)
        {
            hdnTempID.Value = Convert.ToString(objServiceTicket.ServiceTicketOwnerID);
            this.SubOwner = lnkOwnerToDo.Text;
            BindTodoGrid();
        }

        lblToDoMsg.Text = "";
        lblToDoMsg.Visible = false;
    }

    protected void chkPostedNotes_CheckedChanged(object sender, EventArgs e)
    {
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
        {
            if (chkPostedNotes.Checked)
            {
                DisplayNotesIE("IEInternalNotes");
                spanPostedNotes.Attributes.Add("class", "custom-checkbox_checked alignleft");
            }
            else
            {
                DisplayNotesIE("ALL");
                spanPostedNotes.Attributes.Add("class", "custom-checkbox alignleft");
            }
        }
    }

    protected void chkTodoDone_CheckedChanged(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        CheckBox chkTodoDone = (CheckBox)sender;

        ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
        ServiceTicketToDoDetail objToDo = objSerTicRep.GetToDoByToDoID(Convert.ToInt64(hdnTempTodoID.Value));
        objToDo.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objToDo.UpdatedDate = DateTime.Now;
        objToDo.Done = chkTodoDone.Checked;
        objSerTicRep.SubmitChanges();

        lblToDoMsg.Visible = true;
        lblToDoMsg.Text = "To-do status updated successfully.";
        LogAction("Changed a 'to-do' status for : " + this.SubOwner + ".\n\"" + objToDo.ToDo + "\" : " + Convert.ToString(chkTodoDone.Checked ? "Done" : "Not Done"), "IEActivity");
        BindTodoGrid();
    }

    protected void dlTodo_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox chkTodoDone = (CheckBox)e.Item.FindControl("chkTodoDone");
            HiddenField hdnToDoID = (HiddenField)e.Item.FindControl("hdnToDoID");
            HtmlControl menuspan = (HtmlControl)e.Item.FindControl("menuspan");
            chkTodoDone.Attributes.Add("onclick", "javascript:UpdateToDoStatus('" + hdnToDoID.Value + "');");
            if (chkTodoDone.Checked)
            {
                menuspan.Attributes.Add("class", "custom-checkbox_checked");
            }
        }
    }

    protected void dlTodo_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (!base.CanDelete)
        {
            base.RedirectToUnauthorised();
        }

        if (e.CommandName == "DeleteTodo")
        {
            if (e.CommandName == "DeleteTodo")
            {
                ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
                objSerTicRep.DeleteTodoByToDoID(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);
                objSerTicRep.SubmitChanges();
                lblToDoMsg.Text = "To-do deleted successfully.";
                Label lblToDo = (Label)e.Item.FindControl("lblToDo");
                LogAction("Deleted a 'to-do' for : " + this.SubOwner + ".\n\"" + lblToDo.ToolTip + "\"", "IEActivity");
                BindTodoGrid();
            }
        }
    }

    protected void btnAddToDo_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        if (!string.IsNullOrEmpty(txtTodo.Text.Trim()) && hdnTempID.Value != "")
        {
            ServiceTicketToDoDetail objToDo = new ServiceTicketToDoDetail();
            objToDo.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objToDo.CreatedDate = DateTime.Now;
            objToDo.Done = false;

            if (!String.IsNullOrEmpty(txtDueDate.Text.Trim()))
            {
                objToDo.DueDate = DateTime.Parse(Convert.ToString(txtDueDate.Text.Trim()), new System.Globalization.CultureInfo("en-US"));
            }

            objToDo.ServiceTicketID = this.ServiceTicketID;
            objToDo.ToDo = Convert.ToString(txtTodo.Text.Trim());
            objToDo.ToDoOwnerID = Convert.ToInt64(hdnTempID.Value);

            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            objSerTicRep.Insert(objToDo);
            objSerTicRep.SubmitChanges();

            lblToDoMsg.Visible = true;
            lblToDoMsg.Text = "To-do added successfully.";
            txtTodo.Text = "";
            txtDueDate.Text = "";

            LogAction("Assigned a 'to-do' to : " + this.SubOwner + ".\n\"" + objToDo.ToDo + "\"", "IEActivity");

            #region Send Alert For To-do

            ServiceTicket objServiceTicket = objSerTicRep.GetByTicketID(this.ServiceTicketID);

            if (objServiceTicket != null)
            {
                UserInformationRepository objUserRepo = new UserInformationRepository();
                UserInformation objToDoOwner = objUserRepo.GetById(Convert.ToInt64(hdnTempID.Value));

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(objToDoOwner.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                {
                    StreamReader _StreamReader;
                    _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/ReceiptForTicket.htm"));
                    String eMailTemplate = String.Empty;
                    eMailTemplate = _StreamReader.ReadToEnd();
                    _StreamReader.Close();
                    _StreamReader.Dispose();

                    String Message = "You have got a task \"To-do\" for this ticket.<br/><br/>\"" + objToDo.ToDo + "\"";

                    StringBuilder MessageBody = new StringBuilder(eMailTemplate);
                    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                    MessageBody.Replace("{FullName}", objToDoOwner.FirstName + " " + objToDoOwner.LastName);
                    MessageBody.Replace("{TicketNo}", Convert.ToString(objServiceTicket.ServiceTicketID));
                    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                    MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);
                    MessageBody.Replace("{Note}", Message);

                    Common objcommon = new Common();

                    objcommon.SendServiceTicketReciept(objServiceTicket.ServiceTicketID, Convert.ToString(objToDoOwner.UserInfoID), 2, objToDoOwner.Email, MessageBody.ToString(), Convert.ToString(Application["SMTPHOST"]), Convert.ToString(Application["SMTPPORT"]), Convert.ToString(objServiceTicket.ServiceTicketName));
                }

                BindTodoGrid();
            }
            #endregion
        }
    }

    protected void txtDatePromised_TextChanged(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
        ServiceTicket objServiceTicket = objSerTicRep.GetByTicketID(this.ServiceTicketID);

        if (objServiceTicket != null)
        {
            if (!String.IsNullOrEmpty(txtDatePromised.Text.Trim()))
            {
                objServiceTicket.DatePromised = DateTime.ParseExact(txtDatePromised.Text.Trim(),
                                                Common.DateFormats,
                                                CultureInfo.InvariantCulture,
                                                DateTimeStyles.AssumeLocal);

                LogAction("Changed date needed to : " + txtDatePromised.Text.Trim() + ".", "IEActivity");
            }
            else
            {
                objServiceTicket.DatePromised = null;
                LogAction("Removed date needed.", "IEActivity");
            }

            objServiceTicket.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objServiceTicket.UpdatedDate = DateTime.Now;
            objSerTicRep.SubmitChanges();

            lblMsg.Text = "Date needed changed successfully.";
        }
    }

    protected void imgBtnChangeTicketName_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        if (!String.IsNullOrEmpty(txtServiceTicketName.Text.Trim()))
        {
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            ServiceTicket objServiceTicket = objSerTicRep.GetByTicketID(this.ServiceTicketID);

            if (objServiceTicket != null)
            {

                objServiceTicket.ServiceTicketName = Convert.ToString(txtServiceTicketName.Text.Trim());
                objServiceTicket.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objServiceTicket.UpdatedDate = DateTime.Now;
                objSerTicRep.SubmitChanges();

                txtServiceTicketName.ToolTip = Convert.ToString(objServiceTicket.ServiceTicketName);

                LogAction("Changed ticket name to : \"" + txtServiceTicketName.Text.Trim() + "\".", "IEActivity");
                lblMsg.Text = "Ticket name changed successfully.";
            }
        }
    }

    protected void dtlCustNoteCAs_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox chkCustNoteCAs = (CheckBox)e.Item.FindControl("chkCustNoteCAs");
            HiddenField hdnCustNoteCAFlag = (HiddenField)e.Item.FindControl("hdnCustNoteCAFlag");
            HtmlControl menuspan = (HtmlControl)e.Item.FindControl("menuspan");

            if (Convert.ToBoolean(Convert.ToString(hdnCustNoteCAFlag.Value)))
            {
                menuspan.Attributes.Add("class", "custom-checkbox_checked alignleft");
                chkCustNoteCAs.Checked = true;
            }
            else
            {
                menuspan.Attributes.Add("class", "custom-checkbox alignleft");
                chkCustNoteCAs.Checked = false;
            }
        }
    }

    protected void dtlCustNoteIEs_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox chkCustNoteIEs = (CheckBox)e.Item.FindControl("chkCustNoteIEs");
            HiddenField hdnCustNoteIEFlag = (HiddenField)e.Item.FindControl("hdnCustNoteIEFlag");
            HtmlControl menuspan = (HtmlControl)e.Item.FindControl("menuspan");

            if (Convert.ToBoolean(Convert.ToString(hdnCustNoteIEFlag.Value)))
            {
                menuspan.Attributes.Add("class", "custom-checkbox_checked alignleft");
                chkCustNoteIEs.Checked = true;
            }
            else
            {
                menuspan.Attributes.Add("class", "custom-checkbox alignleft");
                chkCustNoteIEs.Checked = false;
            }
        }
    }

    protected void dtlIENoteIEs_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox chkIENoteIEs = (CheckBox)e.Item.FindControl("chkIENoteIEs");
            HiddenField hdnIENoteIEFlag = (HiddenField)e.Item.FindControl("hdnIENoteIEFlag");
            HtmlControl menuspan = (HtmlControl)e.Item.FindControl("menuspan");

            if (Convert.ToBoolean(Convert.ToString(hdnIENoteIEFlag.Value)))
            {
                menuspan.Attributes.Add("class", "custom-checkbox_checked alignleft");
                chkIENoteIEs.Checked = true;
            }
            else
            {
                menuspan.Attributes.Add("class", "custom-checkbox alignleft");
                chkIENoteIEs.Checked = false;
            }
        }
    }

    protected void lnkSaveRecipients_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        LinkButton lnkSaveRecipients = (LinkButton)sender;
        ServiceTicketNoteRecipientsDetail objRecipient;

        Int64? tUserInfoID = null;

        if (lnkSaveRecipients.ID == "lnkSaveCustNoteRecipients")
        {
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();

            if (!String.IsNullOrEmpty(hdnCustomerID.Value) || !String.IsNullOrEmpty(hdnSupplierID.Value))
            {
                tUserInfoID = lblCustSupp.InnerText.Contains("Customer") ? Convert.ToInt64(hdnCustomerID.Value) : Convert.ToInt64(hdnSupplierID.Value);
            }

            objRecipient = objSerTicRep.GetNoteRecipient(this.ServiceTicketID, tUserInfoID);

            objRecipient.SubscriptionFlag = chkCustomer.Checked;
            objRecipient.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objRecipient.UpdatedDate = DateTime.Now;

            objSerTicRep.SubmitChanges();
            objSerTicRep = new ServiceTicketRepository();

            foreach (DataListItem item in dtlCustNoteCAs.Items)
            {
                CheckBox chkCustNoteCAs = (CheckBox)item.FindControl("chkCustNoteCAs");
                HiddenField hdnCustNoteCARecipientID = (HiddenField)item.FindControl("hdnCustNoteCARecipientID");

                objRecipient = objSerTicRep.GetNoteRecipient(Convert.ToInt64(hdnCustNoteCARecipientID.Value));

                objRecipient.SubscriptionFlag = chkCustNoteCAs.Checked;
                objRecipient.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objRecipient.UpdatedDate = DateTime.Now;
            }

            objSerTicRep.SubmitChanges();
            objSerTicRep = new ServiceTicketRepository();

            foreach (DataListItem item in dtlCustNoteIEs.Items)
            {
                CheckBox chkCustNoteIEs = (CheckBox)item.FindControl("chkCustNoteIEs");
                HiddenField hdnCustNoteIERecipientID = (HiddenField)item.FindControl("hdnCustNoteIERecipientID");

                objRecipient = objSerTicRep.GetNoteRecipient(Convert.ToInt64(hdnCustNoteIERecipientID.Value));

                objRecipient.SubscriptionFlag = chkCustNoteIEs.Checked;
                objRecipient.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objRecipient.UpdatedDate = DateTime.Now;
            }

            objSerTicRep.SubmitChanges();
        }
        else
        {
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();

            foreach (DataListItem item in dtlIENoteIEs.Items)
            {
                CheckBox chkIENoteIEs = (CheckBox)item.FindControl("chkIENoteIEs");
                HiddenField hdnIENoteIERecipientID = (HiddenField)item.FindControl("hdnIENoteIERecipientID");

                objRecipient = objSerTicRep.GetNoteRecipient(Convert.ToInt64(hdnIENoteIERecipientID.Value));

                objRecipient.SubscriptionFlag = chkIENoteIEs.Checked;
                objRecipient.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objRecipient.UpdatedDate = DateTime.Now;
            }

            objSerTicRep.SubmitChanges();
        }

        FillNotificationParties();
    }

    protected void lnkFlag_Click(object sender, EventArgs e)
    {
        try
        {
            ServiceTicketFlagDetail objFlag;
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();

            objFlag = objSerTicRep.GetTicketFlag(this.ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID);

            if (objFlag != null)
            {
                objSerTicRep.DeleteTicketFlag(this.ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID);
                imgFlag.Src = "~/Images/flag_gray.png";
                lblMsg.Text = "Flag removed successfully.";
            }
            else
            {
                objFlag = new ServiceTicketFlagDetail();
                objFlag.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objFlag.CreatedDate = DateTime.Now;
                objFlag.ServiceTicketID = this.ServiceTicketID;
                objSerTicRep.Insert(objFlag);
                objSerTicRep.SubmitChanges();

                imgFlag.Src = "~/Images/flag_red.png";
                lblMsg.Text = "Flag added successfully.";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// For filling the support ticket status dropdown
    /// </summary>
    private void FillServiceTicketStatus()
    {
        try
        {
            LookupDA sServiceSupportStatus = new LookupDA();
            LookupBE sServiceSupportStatusBE = new LookupBE();
            sServiceSupportStatusBE.SOperation = "selectall";
            sServiceSupportStatusBE.iLookupCode = "ServiceTicketStatus";
            DataSet ds = new DataSet();
            ds = sServiceSupportStatus.LookUp(sServiceSupportStatusBE);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];

            dt.DefaultView.Sort = "sLookupName";
            ddlServiceTicketStatus.DataSource = dt.DefaultView.ToTable();
            ddlServiceTicketStatus.DataValueField = "iLookupID";
            ddlServiceTicketStatus.DataTextField = "sLookupName";
            ddlServiceTicketStatus.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// For filling the type of request dropdown
    /// </summary>
    private void FillTypeOfRequest()
    {
        try
        {
            LookupDA sServiceSupportStatus = new LookupDA();
            LookupBE sServiceSupportStatusBE = new LookupBE();
            sServiceSupportStatusBE.SOperation = "selectall";
            sServiceSupportStatusBE.iLookupCode = "TypeOfRequest";
            DataSet ds = new DataSet();
            ds = sServiceSupportStatus.LookUp(sServiceSupportStatusBE);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            dt.DefaultView.Sort = "sLookupName";
            ddlTypeOfRequest.DataSource = dt.DefaultView.ToTable();
            ddlTypeOfRequest.DataValueField = "iLookupID";
            ddlTypeOfRequest.DataTextField = "sLookupName";
            ddlTypeOfRequest.DataBind();
            ddlTypeOfRequest.Items.Insert(0, new ListItem("-Select Type Of Request-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// For filling the support ticket owner dropdown
    /// </summary>
    private void FillTicketOwner(Int64? OwnerID)
    {
        try
        {
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            ddlTicketOwner.DataSource = objSerTicRep.GetSubOwnersByTicketID(this.ServiceTicketID).Where(le => le.Existing == 0 && (le.Usertype == 1 || le.Usertype == 2)).Select(le => new { OwnerName = le.FirstName + " " + le.LastName, UserInfoID = le.SubOwnerID }).OrderBy(le => le.OwnerName).ToList();
            ddlTicketOwner.DataValueField = "UserInfoID";
            ddlTicketOwner.DataTextField = "OwnerName";
            ddlTicketOwner.DataBind();
            if (OwnerID == null)
                ddlTicketOwner.Items.Insert(0, new ListItem("-Select Support Ticket Owner-", "0"));

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void DisplayNotes(Int32 NotType)
    {
        try
        {
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            txtNotesForCECA.Text = objSerTicRep.TrailingNotes(this.ServiceTicketID, NotType, false, "\n");
            //Merge OrderView Note Here
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void DisplaySupplierNotes()
    {
        try
        {
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            txtNotesForIE.Text = objSerTicRep.TrailingNotes(this.ServiceTicketID, 3, false, "\n");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void DisplayNotesIE(String SpecificNoteFor)
    {
        try
        {
            Boolean? OnlyInternalNotes = null;

            if (SpecificNoteFor == "IEInternalNotes")
                OnlyInternalNotes = true;

            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            txtNotesForIE.Text = objSerTicRep.TrailingNotes(this.ServiceTicketID, 2, OnlyInternalNotes, "\n");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void GenerateTableGrid(String FileName, String SavedFileName, Int64 AttachmentID)
    {
        List<UploadFile> objFileList = new List<UploadFile>();
        UploadFile objFile = new UploadFile();

        if (ViewState["ListValues"] != null)
        {
            objFileList = (List<UploadFile>)ViewState["ListValues"];
            objFile.OnlyFileName = FileName;
            objFile.SavedFileName = SavedFileName;
            objFile.AttachmentID = AttachmentID;
            objFileList.Add(objFile);
            ViewState["ListValues"] = objFileList;
        }
        else
        {
            objFile.OnlyFileName = FileName;
            objFile.SavedFileName = SavedFileName;
            objFile.AttachmentID = AttachmentID;
            objFileList.Add(objFile);
            ViewState["ListValues"] = objFileList;
        }

        BindFilesFromViewState();
    }

    private void BindFilesFromViewState()
    {
        List<UploadFile> objFileList = new List<UploadFile>();
        if (ViewState["ListValues"] != null)
        {
            objFileList = (List<UploadFile>)ViewState["ListValues"];
            grvAttachment.DataSource = objFileList;
            grvAttachment.DataBind();
            atSpan.InnerText = "Attachment(s) (" + objFileList.Count + ")";
            lblAttachmentMsg.Visible = false;
        }
        else
        {
            lblAttachmentMsg.Visible = true;
            lblAttachmentMsg.Text = "No attachments.";
            atSpan.InnerText = "Attachment(s) (0)";
        }
    }

    private void DownloadFile(String filepath, String displayFileName)
    {
        System.IO.Stream iStream = null;
        string type = "";
        string ext = Path.GetExtension(filepath);

        // Buffer to read 10K bytes in chunk:
        byte[] buffer = new Byte[10000];

        // Total bytes to read:
        long dataToRead;

        // Identify the file name.
        string filename = System.IO.Path.GetFileName(filepath);

        try
        {
            // Open the file.
            iStream = new System.IO.FileStream(filepath, System.IO.FileMode.Open,
            System.IO.FileAccess.Read, System.IO.FileShare.Read);

            // Total bytes to read:
            dataToRead = iStream.Length;
            if (ext != null)
            {
                switch (ext.ToLower())
                {

                    case ".pdf":
                        type = "Application/pdf";
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".gif":
                    case ".tiff":
                        type = "image/gif";
                        break;
                    case ".png":
                        type = "image/png";
                        break;
                    case ".bmp":
                        type = "image/bmp";
                        break;
                    case ".ai":
                        type = "application/illustrator";
                        break;
                    case ".epf":
                        type = "application/postscript";
                        break;
                }
            }

            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + displayFileName + "\"");
            Response.ContentType = type;
            Response.WriteFile(filepath);
            Response.End();
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

    private void FillNotificationParties()
    {
        List<vw_ServiceTicketNoteRecipient> objRecipients = new List<vw_ServiceTicketNoteRecipient>();
        ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
        objRecipients = objSerTicRep.GetNoteRecipientsByTicketID(this.ServiceTicketID).ToList();

        Int64? ContactID = null;
        Int64? SupplierID = null;

        if (!String.IsNullOrEmpty(hdnCustomerID.Value))
        {
            ContactID = Convert.ToInt64(hdnCustomerID.Value);
        }

        if (!String.IsNullOrEmpty(hdnSupplierID.Value))
        {
            SupplierID = Convert.ToInt64(hdnSupplierID.Value);
        }

        if (!String.IsNullOrEmpty(hdnCompanyID.Value))
        {
            dvCustAdmins.Visible = true;

            if (ContactID != null)
            {
                dvCustomer.Visible = true;
                hdnCustomerEmail.Value = Convert.ToString(objRecipients.FirstOrDefault(le => le.UserInfoID == ContactID).Email);
                lblSendEmailToCustomer.Text = Convert.ToString(objRecipients.FirstOrDefault(le => le.UserInfoID == ContactID).FirstName) + " " + Convert.ToString(objRecipients.FirstOrDefault(le => le.UserInfoID == ContactID).LastName);

                if (Convert.ToBoolean(objRecipients.FirstOrDefault(le => le.UserInfoID == ContactID).SubscriptionFlag))
                {
                    chkCustomer.Checked = true;
                    spanCustomer.Attributes.Add("class", "custom-checkbox_checked alignleft");
                }
                else
                {
                    chkCustomer.Checked = false;
                    spanCustomer.Attributes.Add("class", "custom-checkbox alignleft");
                }

                dvCustNotefacebook.Style.Add("height", "550px");
                dvCustNotefacebook.Style.Add("top", "2%");
                dvCustNotepp_Content.Style.Add("height", "540px");
            }
            else
            {
                dvCustomer.Visible = false;

                dvCustNotefacebook.Style.Add("height", "500px");
                dvCustNotefacebook.Style.Add("top", "6%");
                dvCustNotepp_Content.Style.Add("height", "490px");
            }
        }
        else if (SupplierID != null)
        {
            hdnSupplierEmail.Value = Convert.ToString(objRecipients.FirstOrDefault(le => le.UserInfoID == SupplierID).Email);
            lblSendEmailToCustomer.Text = Convert.ToString(objRecipients.FirstOrDefault(le => le.UserInfoID == SupplierID).FirstName) + " " + Convert.ToString(objRecipients.FirstOrDefault(le => le.UserInfoID == SupplierID).LastName);

            if (Convert.ToBoolean(objRecipients.FirstOrDefault(le => le.UserInfoID == SupplierID).SubscriptionFlag))
            {
                chkCustomer.Checked = true;
                spanCustomer.Attributes.Add("class", "custom-checkbox_checked alignleft");
            }
            else
            {
                chkCustomer.Checked = false;
                spanCustomer.Attributes.Add("class", "custom-checkbox alignleft");
            }

            dvCustNotefacebook.Style.Add("height", "550px");
            dvCustNotefacebook.Style.Add("top", "2%");
            dvCustNotepp_Content.Style.Add("height", "540px");
        }
        //Condition for anonymous user
        else
        {
            dvCustAdmins.Visible = false;
            dvCustomer.Visible = true;

            if (Convert.ToBoolean(objRecipients.FirstOrDefault(le => le.UserInfoID.Equals(null)).SubscriptionFlag))
            {
                chkCustomer.Checked = true;
                spanCustomer.Attributes.Add("class", "custom-checkbox_checked alignleft");
            }
            else
            {
                chkCustomer.Checked = false;
                spanCustomer.Attributes.Add("class", "custom-checkbox alignleft");
            }

            hdnCustomerEmail.Value = Convert.ToString(objRecipients.FirstOrDefault(le => le.UserInfoID.Equals(null)).Email);
            lblSendEmailToCustomer.Text = Convert.ToString(objRecipients.FirstOrDefault(le => le.UserInfoID.Equals(null)).Email);

            dvCustNotefacebook.Style.Add("height", "435px");
            dvCustNotefacebook.Style.Add("top", "11%");
            dvCustNotepp_Content.Style.Add("height", "425px");
        }

        dtlCustNoteCAs.DataSource = objRecipients.Where(le => le.Usertype == (SupplierID == null ? 3 : 6) && le.UserInfoID != ContactID).OrderBy(le => le.FirstName).ToList();
        dtlCustNoteCAs.DataBind();
        dtlCustNoteIEs.DataSource = objRecipients.Where(le => (le.Usertype == 1 || le.Usertype == 2) && le.UserInfoID != ContactID).OrderBy(le => le.FirstName).ToList();
        dtlCustNoteIEs.DataBind();
        dtlIENoteIEs.DataSource = objRecipients.Where(le => (le.Usertype == 1 || le.Usertype == 2) && le.UserInfoID != ContactID).OrderBy(le => le.FirstName).ToList();
        dtlIENoteIEs.DataBind();
    }

    private void SendCloseTicketNotification()
    {
        try
        {
            String eMailTemplate = String.Empty;
            String sSubject = "Your Support Ticket Has Been Closed";

            String sReplyToadd = Common.ReplyTo;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/CloseTicket.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            Int64? tUserInfoID = null;
            Int32 NoteType = lblCustSupp.InnerText.Contains("Customer") ? 1 : 3;

            if (!String.IsNullOrEmpty(hdnCustomerID.Value) || !String.IsNullOrEmpty(hdnSupplierID.Value))
            {
                tUserInfoID = lblCustSupp.InnerText.Contains("Customer") ? Convert.ToInt64(hdnCustomerID.Value) : Convert.ToInt64(hdnSupplierID.Value);
            }

            String sUserInfoID = tUserInfoID != null ? Convert.ToString(tUserInfoID) : "annx";

            StringBuilder MessageBody = new StringBuilder(eMailTemplate);

            MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            MessageBody.Replace("{FullName}", lblSendEmailToCustomer.Text.Trim());
            MessageBody.Replace("{TicketNo}", lblServiceTicketNumber.Text);

            //using (MailMessage objEmail = new MailMessage())
            //{
            //    objEmail.Body = MessageBody.ToString();
            //    objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
            //    objEmail.IsBodyHtml = true;
            //    objEmail.ReplyTo = new MailAddress(sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + this.ServiceTicketID + "un" + sUserInfoID + "nt" + NoteType + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@')));
            //    objEmail.Subject = sSubject;
            //    objEmail.To.Add(new MailAddress(Convert.ToString(hdnCustomerEmail.Value)));

            //    SmtpClient objSmtp = new SmtpClient();

            //    objSmtp.EnableSsl = Common.SSL;
            //    objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
            //    objSmtp.Host = Common.SMTPHost;
            //    objSmtp.Port = Common.SMTPPort;

            //    objSmtp.Send(objEmail);
            //}

            String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + this.ServiceTicketID + "un" + sUserInfoID + "nt" + NoteType + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
            new CommonMails().SendMailWithReplyTo(Convert.ToInt64(sUserInfoID), "Support Ticket Center", Common.EmailFrom, Convert.ToString(hdnCustomerEmail.Value), sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SendEMailToAnnCECAIESuppSE()
    {
        try
        {
            String eMailTemplate = String.Empty;
            String sSubject = txtServiceTicketName.Text + " - Support Ticket - " + lblServiceTicketNumber.Text;

            String sReplyToadd = Common.ReplyTo;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/ServiceTicketNote.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            ServiceTicketNoteRecipientsDetail objRecipient;

            Int64? tUserInfoID = null;
            Int32 NoteType = lblCustSupp.InnerText.Contains("Customer") ? 1 : 3;

            String TrailingNotes = objSerTicRep.TrailingNotes(this.ServiceTicketID, NoteType, false, "<br/>");

            if (!String.IsNullOrEmpty(hdnCustomerID.Value) || !String.IsNullOrEmpty(hdnSupplierID.Value))
            {
                tUserInfoID = lblCustSupp.InnerText.Contains("Customer") ? Convert.ToInt64(hdnCustomerID.Value) : Convert.ToInt64(hdnSupplierID.Value);
            }

            objSerTicRep = new ServiceTicketRepository();
            objRecipient = objSerTicRep.GetNoteRecipient(this.ServiceTicketID, tUserInfoID);

            objRecipient.SubscriptionFlag = chkCustomer.Checked;
            objRecipient.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objRecipient.UpdatedDate = DateTime.Now;
            objSerTicRep.SubmitChanges();

            //Sending notification to the customer
            if (chkCustomer.Checked)
            {
                String sUserInfoID = String.Empty;
                Int64 sToUserInfoID = 0;
                if (tUserInfoID != null)
                {
                    sUserInfoID = Convert.ToString(tUserInfoID);
                    sToUserInfoID = Convert.ToInt64(tUserInfoID);
                }
                else
                {
                    sUserInfoID = "annx";
                }

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                {
                    StringBuilder MessageBody = new StringBuilder(eMailTemplate);

                    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                    MessageBody.Replace("{FullName}", lblSendEmailToCustomer.Text.Trim());
                    MessageBody.Replace("{TicketNo}", lblServiceTicketNumber.Text);
                    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                    MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);
                    MessageBody.Replace("{Note}", "<br/>" + TrailingNotes);

                    if (lblCustSupp.InnerText.Contains("Customer"))
                    {
                        MessageBody.Replace("{CloseTicket}", String.Empty);
                    }
                    else
                    {
                        String CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
                                    <a href='" + ConfigurationSettings.AppSettings["siteurl"]
                                        + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
                                        + this.ServiceTicketID + "&uid=" + tUserInfoID
                                        + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";

                        MessageBody.Replace("{CloseTicket}", CloseTicket);
                    }

                    String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + this.ServiceTicketID + "un" + sUserInfoID + "nt" + NoteType + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                    new CommonMails().SendMailWithReplyTo(Convert.ToInt64(sUserInfoID), "Support Ticket Center", Common.EmailFrom, Convert.ToString(hdnCustomerEmail.Value), sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
                }
            }

            //Sending notification to the CAs
            foreach (DataListItem item in dtlCustNoteCAs.Items)
            {
                CheckBox chkCustNoteCAs = (CheckBox)item.FindControl("chkCustNoteCAs");
                Label lblUserNameCA = (Label)item.FindControl("lblUserNameCA");
                HiddenField hdnCustNoteCAUserID = (HiddenField)item.FindControl("hdnCustNoteCAUserID");
                HiddenField hdnCustNoteCAEmail = (HiddenField)item.FindControl("hdnCustNoteCAEmail");
                HiddenField hdnCustNoteCARecipientID = (HiddenField)item.FindControl("hdnCustNoteCARecipientID");

                Int64 sToCAUserInfoID = !String.IsNullOrEmpty(hdnCustNoteCAUserID.Value) ? Convert.ToInt64(hdnCustNoteCAUserID.Value) : 0;
                objSerTicRep = new ServiceTicketRepository();

                objRecipient = objSerTicRep.GetNoteRecipient(Convert.ToInt64(hdnCustNoteCARecipientID.Value));

                objRecipient.SubscriptionFlag = chkCustNoteCAs.Checked;
                objRecipient.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objRecipient.UpdatedDate = DateTime.Now;
                objSerTicRep.SubmitChanges();

                if (chkCustNoteCAs.Checked)
                {
                    //Email Management
                    if (objManageEmailRepo.CheckEmailAuthentication(sToCAUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                    {
                        StringBuilder MessageBody = new StringBuilder(eMailTemplate);

                        MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                        MessageBody.Replace("{FullName}", lblUserNameCA.Text);
                        MessageBody.Replace("{TicketNo}", lblServiceTicketNumber.Text);
                        MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                        MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);
                        MessageBody.Replace("{Note}", "<br/>" + TrailingNotes);

                        if (lblCustSupp.InnerText.Contains("Customer"))
                        {
                            MessageBody.Replace("{CloseTicket}", String.Empty);
                        }
                        else
                        {
                            String CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
                                    <a href='" + ConfigurationSettings.AppSettings["siteurl"]
                                        + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
                                        + this.ServiceTicketID + "&uid=" + sToCAUserInfoID
                                        + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";

                            MessageBody.Replace("{CloseTicket}", CloseTicket);
                        }

                        String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + this.ServiceTicketID + "un" + sToCAUserInfoID + "nt" + NoteType + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                        new CommonMails().SendMailWithReplyTo(Convert.ToInt64(sToCAUserInfoID), "Support Ticket Center", Common.EmailFrom, Convert.ToString(hdnCustNoteCAEmail.Value), sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
                    }
                }
            }

            TrailingNotes = new ServiceTicketRepository().TrailingNotes(this.ServiceTicketID, 2, false, "<br/>");

            //Sending notification to the IEs
            foreach (DataListItem item in dtlCustNoteIEs.Items)
            {
                CheckBox chkCustNoteIEs = (CheckBox)item.FindControl("chkCustNoteIEs");
                Label lblUserNameIE = (Label)item.FindControl("lblUserNameIE");
                HiddenField hdnCustNoteIEUserID = (HiddenField)item.FindControl("hdnCustNoteIEUserID");
                HiddenField hdnCustNoteIEEmail = (HiddenField)item.FindControl("hdnCustNoteIEEmail");
                HiddenField hdnCustNoteIERecipientID = (HiddenField)item.FindControl("hdnCustNoteIERecipientID");

                Int64 sToIEUserInfoID = !String.IsNullOrEmpty(hdnCustNoteIEUserID.Value) ? Convert.ToInt64(hdnCustNoteIEUserID.Value) : 0;

                objSerTicRep = new ServiceTicketRepository();
                objRecipient = objSerTicRep.GetNoteRecipient(Convert.ToInt64(hdnCustNoteIERecipientID.Value));

                objRecipient.SubscriptionFlag = chkCustNoteIEs.Checked;
                objRecipient.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objRecipient.UpdatedDate = DateTime.Now;
                objSerTicRep.SubmitChanges();

                if (chkCustNoteIEs.Checked)
                {
                    //Email Management
                    if (objManageEmailRepo.CheckEmailAuthentication(sToIEUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                    {
                        StringBuilder MessageBody = new StringBuilder(eMailTemplate);

                        MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                        MessageBody.Replace("{FullName}", lblUserNameIE.Text);
                        MessageBody.Replace("{TicketNo}", lblServiceTicketNumber.Text);
                        MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                        MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);
                        MessageBody.Replace("{Note}", "<br/>" + TrailingNotes);

                        String CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
                                    <a href='" + ConfigurationSettings.AppSettings["siteurl"]
                                        + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
                                        + this.ServiceTicketID + "&uid=" + sToIEUserInfoID
                                        + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";

                        MessageBody.Replace("{CloseTicket}", CloseTicket);

                        String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + this.ServiceTicketID + "un" + sToIEUserInfoID + "nt" + NoteType + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                        new CommonMails().SendMailWithReplyTo(Convert.ToInt64(sToIEUserInfoID), "Support Ticket Center", Common.EmailFrom, Convert.ToString(hdnCustNoteIEEmail.Value), sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
                    }
                }
            }

            FillNotificationParties();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SendEMailToIE()
    {
        try
        {
            String eMailTemplate = String.Empty;
            String sSubject = txtServiceTicketName.Text + " - Support Ticket - " + lblServiceTicketNumber.Text;

            String sReplyToadd = Common.ReplyTo;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/ServiceTicketNote.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();

            String TrailingNotes = objSerTicRep.TrailingNotes(this.ServiceTicketID, 2, false, "<br/>");

            foreach (DataListItem item in dtlIENoteIEs.Items)
            {
                CheckBox chkIENoteIEs = (CheckBox)item.FindControl("chkIENoteIEs");
                Label lblUserNameIE = (Label)item.FindControl("lblUserNameIE");
                HiddenField hdnIENoteIEUserID = (HiddenField)item.FindControl("hdnIENoteIEUserID");
                HiddenField hdnIENoteIEEmail = (HiddenField)item.FindControl("hdnIENoteIEEmail");

                HiddenField hdnIENoteIERecipientID = (HiddenField)item.FindControl("hdnIENoteIERecipientID");
                Int64 sToUserInfoID = !String.IsNullOrEmpty(hdnIENoteIEUserID.Value) ? Convert.ToInt64(hdnIENoteIEUserID.Value) : 0;

                objSerTicRep = new ServiceTicketRepository();
                ServiceTicketNoteRecipientsDetail objRecipient = objSerTicRep.GetNoteRecipient(Convert.ToInt64(hdnIENoteIERecipientID.Value));

                objRecipient.SubscriptionFlag = chkIENoteIEs.Checked;
                objRecipient.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objRecipient.UpdatedDate = DateTime.Now;
                objSerTicRep.SubmitChanges();

                if (chkIENoteIEs.Checked)
                {
                    //Email Management
                    if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                    {
                        StringBuilder MessageBody = new StringBuilder(eMailTemplate);

                        MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                        MessageBody.Replace("{FullName}", lblUserNameIE.Text);
                        MessageBody.Replace("{TicketNo}", lblServiceTicketNumber.Text);
                        MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                        MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);
                        MessageBody.Replace("{Note}", "<br/>" + TrailingNotes);

                        String CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
                                    <a href='" + ConfigurationSettings.AppSettings["siteurl"]
                                        + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
                                        + this.ServiceTicketID + "&uid=" + sToUserInfoID
                                        + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";

                        MessageBody.Replace("{CloseTicket}", CloseTicket);

                        String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + this.ServiceTicketID + "un" + sToUserInfoID + "nt2en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                        new CommonMails().SendMailWithReplyTo(Convert.ToInt64(sToUserInfoID), "Support Ticket Center", Common.EmailFrom, Convert.ToString(hdnIENoteIEEmail.Value), sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
                    }
                }
            }

            FillNotificationParties();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SendEmailToIESuppSE()
    {
        String eMailTemplate = String.Empty;
        String sSubject = txtServiceTicketName.Text + " - Support Ticket - " + lblServiceTicketNumber.Text;

        String sReplyToadd = Common.ReplyTo;

        StreamReader _StreamReader;
        _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/ServiceTicketNote.htm"));
        eMailTemplate = _StreamReader.ReadToEnd();
        _StreamReader.Close();
        _StreamReader.Dispose();

        String TrailingNotes = new ServiceTicketRepository().TrailingNotes(this.ServiceTicketID, 3, false, "<br/>");

        List<vw_ServiceTicketNoteRecipient> lstRecipients = new List<vw_ServiceTicketNoteRecipient>();
        lstRecipients = new ServiceTicketRepository().GetNoteRecipientsByTicketID(this.ServiceTicketID).Where(le => le.SubscriptionFlag == true).ToList();
        foreach (vw_ServiceTicketNoteRecipient recipient in lstRecipients)
        {
            //Email Management
            if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
            {
                String UserName = Convert.ToString(recipient.LastName) + " " + Convert.ToString(recipient.FirstName);
                StringBuilder MessageBody = new StringBuilder(eMailTemplate);
                MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
                MessageBody.Replace("{TicketNo}", Convert.ToString(this.ServiceTicketID));
                MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);
                MessageBody.Replace("{Note}", TrailingNotes);

                String CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
                                    <a href='" + ConfigurationSettings.AppSettings["siteurl"]
                                + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
                                + this.ServiceTicketID + "&uid=" + recipient.UserInfoID
                                + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";

                MessageBody.Replace("{CloseTicket}", CloseTicket);

                String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(this.ServiceTicketID) + "un" + Convert.ToString(recipient.UserInfoID) + "nt3en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                new CommonMails().SendMailWithReplyTo(Convert.ToInt64(recipient.UserInfoID), "Support Ticket Center", Common.EmailFrom, Convert.ToString(recipient.Email), sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
            }
        }
    }

    private void DisplaySubOwners()
    {
        if (this.ServiceTicketID != 0)
        {
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            vw_ServiceTicket objSerTic = objSerTicRep.GetFirstByID(this.ServiceTicketID);

            if (objSerTic != null)
            {
                lnkOwnerToDo.Text = objSerTic.ServiceTicketOwnerID != null ? objSerTic.FirstName + " " + objSerTic.LastName : "N/A";

                //Get subowners
                List<GetServiceTicketSubOwnerDetailsResult> objSubOwnerList = new List<GetServiceTicketSubOwnerDetailsResult>() { };
                objSubOwnerList = objSerTicRep.GetSubOwnersByTicketID(Convert.ToInt32(this.ServiceTicketID)).Where(le => le.Existing == 1).OrderBy(le => le.FirstName).ToList();

                dtlIESubOwners.DataSource = objSubOwnerList.Where(le => le.Usertype == 1 || le.Usertype == 2).ToList();
                dtlIESubOwners.DataBind();

                dtlCASubOwners.DataSource = objSubOwnerList.Where(le => objSerTic.SupplierID == null ? le.Usertype == 3 : (le.Usertype == 5 || le.Usertype == 6)).ToList();
                dtlCASubOwners.DataBind();
            }
        }
    }

    private void BindTodoGrid()
    {
        ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
        dlTodo.DataSource = objSerTicRep.GetTodoListByTicketAndOwnerID(this.ServiceTicketID, Convert.ToInt64(hdnTempID.Value));
        dlTodo.DataBind();
        modalTodo.Show();
    }

    private void LogAction(String Content, String SpecificNoteFor)
    {
        String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);

        NoteDetail objComNot = new NoteDetail();
        NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();

        objComNot.Notecontents = Content;
        objComNot.NoteFor = strNoteFor;
        objComNot.SpecificNoteFor = SpecificNoteFor;
        objComNot.ForeignKey = ServiceTicketID;
        objComNot.CreateDate = System.DateTime.Now;
        objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objComNot.UpdateDate = System.DateTime.Now;
        objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

        objCompNoteHistRepos.Insert(objComNot);
        objCompNoteHistRepos.SubmitChanges();

        chkPostedNotes.Checked = false;
        chkPostedNotes_CheckedChanged(null, null);
    }    

    #endregion
}