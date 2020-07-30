using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_ServiceTicketCenter_ActivityReport : PageBase
{
    #region Page Properties

    private Int64? CompanyID
    {
        get
        {
            if (Convert.ToString(this.ViewState["CompanyID"]) == String.Empty)
                return null;
            else
                return Convert.ToInt64(this.ViewState["CompanyID"]);
        }
        set
        {
            this.ViewState["CompanyID"] = value;
        }
    }

    private Int64? ContactID
    {
        get
        {
            if (Convert.ToString(this.ViewState["ContactID"]) == String.Empty)
                return null;
            else
                return Convert.ToInt64(this.ViewState["ContactID"]);
        }
        set
        {
            this.ViewState["ContactID"] = value;
        }
    }

    private Int64? OpenedByID
    {
        get
        {
            if (Convert.ToString(this.ViewState["OpenedByID"]) == String.Empty)
                return null;
            else
                return Convert.ToInt64(this.ViewState["OpenedByID"]);
        }
        set
        {
            this.ViewState["OpenedByID"] = value;
        }
    }

    private Int64? StatusID
    {
        get
        {
            if (Convert.ToString(this.ViewState["StatusID"]) == String.Empty)
                return null;
            else
                return Convert.ToInt64(this.ViewState["StatusID"]);
        }
        set
        {
            this.ViewState["StatusID"] = value;
        }
    }

    private Int64? OwnerID
    {
        get
        {
            if (Convert.ToString(this.ViewState["OwnerID"]) == String.Empty)
                return null;
            else
                return Convert.ToInt64(this.ViewState["OwnerID"]);
        }
        set
        {
            this.ViewState["OwnerID"] = value;
        }
    }

    private Int64? SupplierID
    {
        get
        {
            if (Convert.ToString(this.ViewState["SupplierID"]) == String.Empty)
                return null;
            else
                return Convert.ToInt64(this.ViewState["SupplierID"]);
        }
        set
        {
            this.ViewState["SupplierID"] = value;
        }
    }

    private Int64? TypeOfRequestID
    {
        get
        {
            if (Convert.ToString(this.ViewState["TypeOfRequestID"]) == String.Empty)
                return null;
            else
                return Convert.ToInt64(this.ViewState["TypeOfRequestID"]);
        }
        set
        {
            this.ViewState["TypeOfRequestID"] = value;
        }
    }

    private Int64? SubOwnerID
    {
        get
        {
            if (Convert.ToString(this.ViewState["SubOwnerID"]) == String.Empty)
                return null;
            else
                return Convert.ToInt64(this.ViewState["SubOwnerID"]);
        }
        set
        {
            this.ViewState["SubOwnerID"] = value;
        }
    }

    private String TicketName
    {
        get
        {
            if (Convert.ToString(this.ViewState["TicketName"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["TicketName"]);
        }
        set
        {
            this.ViewState["TicketName"] = value;
        }
    }

    private String TicketNumber
    {
        get
        {
            if (Convert.ToString(this.ViewState["TicketNumber"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["TicketNumber"]);
        }
        set
        {
            this.ViewState["TicketNumber"] = value;
        }
    }

    private String DateNeeded
    {
        get
        {
            if (Convert.ToString(this.ViewState["DateNeeded"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["DateNeeded"]);
        }
        set
        {
            this.ViewState["DateNeeded"] = value;
        }
    }

    private String KeyWord
    {
        get
        {
            if (Convert.ToString(this.ViewState["KeyWord"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["KeyWord"]);
        }
        set
        {
            this.ViewState["KeyWord"] = value;
        }
    }

    private String FromDate
    {
        get
        {
            if (Convert.ToString(this.ViewState["FromDate"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["FromDate"]);
        }
        set
        {
            this.ViewState["FromDate"] = value;
        }
    }

    private String ToDate
    {
        get
        {
            if (Convert.ToString(this.ViewState["ToDate"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["ToDate"]);
        }
        set
        {
            this.ViewState["ToDate"] = value;
        }
    }

    private Boolean PostedNotesOnly
    {
        get
        {
            return Convert.ToBoolean(this.ViewState["PostedNotesOnly"]);
        }
        set
        {
            this.ViewState["PostedNotesOnly"] = value;
        }
    }

    //private List<FUN_GetServiceTicketNoteResult> lstServiceTicketNotes = new List<FUN_GetServiceTicketNoteResult>();

    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    OrderConfirmationRepository objOrderRepos = new OrderConfirmationRepository();
    NotesHistoryRepository objNotesRepos = new NotesHistoryRepository();
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CheckLogin();

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

                ((Label)Master.FindControl("lblPageHeading")).Text = "Support Ticket - Activity Report";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/ServiceTicketCenter/SearchServiceTicket.aspx";

                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString)))
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["com"]))
                    {
                        this.CompanyID = Convert.ToInt32(Request.QueryString["com"]);

                        if (!String.IsNullOrEmpty(Request.QueryString["con"]))
                            this.ContactID = Convert.ToInt32(Request.QueryString["con"]);
                        else
                            this.ContactID = null;
                    }
                    else
                    {
                        this.CompanyID = null;
                        this.ContactID = null;
                    }

                    if (!String.IsNullOrEmpty(Request.QueryString["ob"]))
                        this.OpenedByID = Convert.ToInt32(Request.QueryString["ob"]);
                    else
                        this.OpenedByID = null;

                    if (!String.IsNullOrEmpty(Request.QueryString["sta"]))
                        this.StatusID = Convert.ToInt32(Request.QueryString["sta"]);
                    else
                        this.StatusID = null;

                    if (!String.IsNullOrEmpty(Request.QueryString["own"]))
                        this.OwnerID = Convert.ToInt32(Request.QueryString["own"]);
                    else
                        this.OwnerID = null;

                    if (!String.IsNullOrEmpty(Request.QueryString["sp"]))
                        this.SupplierID = Convert.ToInt32(new SupplierRepository().GetByUserInfoId(Convert.ToInt64(Request.QueryString["sp"])).SupplierID);
                    else
                        this.SupplierID = null;

                    if (!String.IsNullOrEmpty(Request.QueryString["tor"]))
                        this.TypeOfRequestID = Convert.ToInt64(Request.QueryString["tor"]);
                    else
                        this.TypeOfRequestID = null;

                    if (!String.IsNullOrEmpty(Request.QueryString["so"]))
                        this.SubOwnerID = Convert.ToInt64(Request.QueryString["so"]);
                    else
                        this.SubOwnerID = null;

                    if (!String.IsNullOrEmpty(Request.QueryString["pn"]) && Request.QueryString["pn"] == "1")
                        this.PostedNotesOnly = true;
                    else
                        this.PostedNotesOnly = false;

                    this.TicketName = Request.QueryString["nam"];
                    this.TicketNumber = Request.QueryString["num"];
                    this.DateNeeded = Request.QueryString["dn"];
                    this.KeyWord = Request.QueryString["kw"];
                    this.FromDate = Request.QueryString["fd"];
                    this.ToDate = Request.QueryString["td"];
                }

                hdnScrollY.Value = !String.IsNullOrEmpty(Convert.ToString(Session["ActRepScrPos"])) ? Convert.ToString(Session["ActRepScrPos"]) : "0";

                BindData();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Control Events

    protected void dlTicketActivities_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnTicketID = (HiddenField)e.Item.FindControl("hdnTicketID");
                if (hdnTicketID != null)
                {
                    Int64 TicketID = Convert.ToInt64(hdnTicketID.Value);
                    DataList dlNotes = (DataList)e.Item.FindControl("dlNotes");
                    LinkButton lnkAssignSubOwners = (LinkButton)e.Item.FindControl("lnkAssignSubOwners");
                    lnkAssignSubOwners.PostBackUrl = "";

                    HtmlControl spanAddNote = (HtmlControl)e.Item.FindControl("spanAddNote");
                    spanAddNote.Attributes.Add("onclick", "javascript:NotePopup(" + TicketID + ")");


                    //HtmlControl spanAssociateWithOrder = (HtmlControl)e.Item.FindControl("spanAssociateWithOrder");
                    //spanAssociateWithOrder.Attributes.Add("onclick", "javascript:AssociatePopup(" + TicketID + ")");

                    HtmlGenericControl divAssociateOrder = (HtmlGenericControl)e.Item.FindControl("divAssociateOrder");

                    LinkButton lnkbtnAssociateWithOrder = (LinkButton)e.Item.FindControl("lnkbtnAssociateWithOrder");
                    Label lblAssociateOrder = (Label)e.Item.FindControl("lblAssociateOrder");
                    Label lblServiceTicketStatus = (Label)e.Item.FindControl("lblServiceTicketStatus");

                    var order = objOrderRepos.GetOrderByAssociateTicketID(TicketID);
                    //Order order = new Order();
                    //this.lstServiceTicketNotes = this.lstServiceTicketNotes.Where(le => le.ServiceTicketID == TicketID).ToList();


                    ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
                    IMultipleResults Results;
                    Results = objSerTicRep.GetServiceTicketActivitiesNotes("IEInternalNotes", IncentexGlobal.CurrentMember.UserInfoID, TicketID);
                    List<FUN_GetServiceTicketNoteResult> lstServiceTicketNotes = Results.GetResult<FUN_GetServiceTicketNoteResult>().ToList();


                    if (lblServiceTicketStatus != null && lblServiceTicketStatus.Text.ToLower() == "open")
                    {
                        if (order != null && lnkbtnAssociateWithOrder != null && divAssociateOrder != null && lblAssociateOrder != null)
                        {
                            lnkbtnAssociateWithOrder.Visible = false;
                            divAssociateOrder.Visible = true;
                            lblAssociateOrder.Text = order.OrderNumber;
                            List<FUN_GetServiceTicketNoteResult> objNoteDetail = new List<FUN_GetServiceTicketNoteResult>();
                            objNoteDetail = objNotesRepos.GetCustomerNotesByOrderID(order.OrderID, TicketID).ToList();
                            lstServiceTicketNotes.AddRange(objNoteDetail);
                        }
                        else
                        {
                            lnkbtnAssociateWithOrder.Visible = true;
                            divAssociateOrder.Visible = false;
                        }
                    }
                    else
                    {
                        lnkbtnAssociateWithOrder.Visible = false;
                        if (order != null && divAssociateOrder != null && lblAssociateOrder != null)
                        {
                            divAssociateOrder.Visible = true;
                            lblAssociateOrder.Text = order.OrderNumber;
                        }
                        else
                        {
                            divAssociateOrder.Visible = false;
                        }
                    }

                    //dlNotes.DataSource = this.lstServiceTicketNotes.Where(le => le.ServiceTicketID == TicketID).ToList().OrderByDescending(le => le.CreateDate).ToList();
                    //dlNotes.DataBind();

                    dlNotes.DataSource = lstServiceTicketNotes.ToList();
                    dlNotes.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dlTicketActivities_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "TicketDetail")
            {
                IncentexGlobal.SearchTicketURL = Request.Url.ToString();
                Session["ActRepScrPos"] = Convert.ToString(hdnScrollY.Value);
                Response.Redirect("~/admin/ServiceTicketCenter/ServiceTicketDetail.aspx?id=" + Convert.ToString(e.CommandArgument), false);
            }
            else if (e.CommandName == "SubOwners")
            {
                Session["ActRepScrPos"] = Convert.ToString(hdnScrollY.Value);
                Response.Redirect("~/admin/ServiceTicketCenter/AssignSubOwners.aspx?id=" + Convert.ToString(e.CommandArgument), false);
            }
            else if (e.CommandName == "AssociateOrder")
            {
                var orderList = objOrderRepos.GetUnAssociateOrderForTicket();
                if (orderList != null && orderList.Count > 0)
                {
                    ddlUnAssociateOrder.DataSource = orderList;
                    ddlUnAssociateOrder.DataTextField = "OrderNumber";
                    ddlUnAssociateOrder.DataValueField = "OrderID";
                    ddlUnAssociateOrder.DataBind();
                    ddlUnAssociateOrder.Items.Insert(0, "--Select Order--");
                    ddlUnAssociateOrder.Items[0].Value = "";
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AssociatePopup('" + Convert.ToString(e.CommandArgument) + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkNoteIE_Click(object sender, EventArgs e)
    {
        try
        {
            if (!(String.IsNullOrEmpty(txtNoteIE.Text.Trim())))
            {
                String NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);
                ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
                objSerTicRep.InsertTicketNote(Convert.ToInt64(hdnPopupTicketID.Value), IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtNoteIE.Text.Trim()), NoteFor, "IEInternalNotes");
                SendEMailForNotesIE();
                txtNoteIE.Text = "";
                BindData();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnAssociateOrder_Click(object sender, EventArgs e)
    {
        try
        {
            if (!(String.IsNullOrEmpty(ddlUnAssociateOrder.SelectedValue)))
            {
                objOrderRepos.UpdateOrderSetAssociateTicket(Convert.ToInt64(ddlUnAssociateOrder.SelectedValue), Convert.ToInt64(hdnAssociateTicketID.Value));
                ddlUnAssociateOrder.SelectedValue = "";
                BindData();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Methods

    public void BindData()
    {
        try
        {
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            IMultipleResults Results;

            if (!this.PostedNotesOnly)
            {
                Results = objSerTicRep.GetServiceTicketActivities(this.CompanyID, this.ContactID, this.OpenedByID, this.StatusID, this.OwnerID, this.TicketName, this.TicketNumber, this.DateNeeded, this.SupplierID, this.TypeOfRequestID, this.KeyWord, this.SubOwnerID, this.FromDate, this.ToDate, null, IncentexGlobal.CurrentMember.UserInfoID);
            }
            else
            {
                Results = objSerTicRep.GetServiceTicketActivities(this.CompanyID, this.ContactID, this.OpenedByID, this.StatusID, this.OwnerID, this.TicketName, this.TicketNumber, this.DateNeeded, this.SupplierID, this.TypeOfRequestID, this.KeyWord, this.SubOwnerID, this.FromDate, this.ToDate, "IEInternalNotes", IncentexGlobal.CurrentMember.UserInfoID);
            }

            List<GetServiceTicketActivitiesResult> lstServiceTickets = Results.GetResult<GetServiceTicketActivitiesResult>().ToList();
            //this.lstServiceTicketNotes = Results.GetResult<FUN_GetServiceTicketNoteResult>().ToList();

            lblRecords.Text = Convert.ToString(lstServiceTickets.Count);

            dlTicketActivities.DataSource = lstServiceTickets;
            dlTicketActivities.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SendEMailForNotesIE()
    {
        try
        {
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            vw_ServiceTicket objServiceTicket = objSerTicRep.GetFirstByID(Convert.ToInt64(hdnPopupTicketID.Value));

            if (objServiceTicket != null)
            {
                String eMailTemplate = String.Empty;
                String sSubject = objServiceTicket.ServiceTicketName + " - Support Ticket - " + objServiceTicket.ServiceTicketNumber;

                String sReplyToadd = Common.ReplyTo;

                StreamReader _StreamReader;
                _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/ServiceTicketNote.htm"));
                eMailTemplate = _StreamReader.ReadToEnd();
                _StreamReader.Close();
                _StreamReader.Dispose();

                List<vw_ServiceTicketNoteRecipient> lstRecipients = new List<vw_ServiceTicketNoteRecipient>();
                lstRecipients = objSerTicRep.GetNoteRecipientsByTicketID(Convert.ToInt64(hdnPopupTicketID.Value)).Where(le => (le.Usertype == 1 || le.Usertype == 2) && le.SubscriptionFlag == true).ToList();

                String TrailingNotes = objSerTicRep.TrailingNotes(Convert.ToInt64(hdnPopupTicketID.Value), 2, false, "<br/>");

                foreach (vw_ServiceTicketNoteRecipient recipient in lstRecipients)
                {
                    //Email Management
                    if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                    {
                        StringBuilder MessageBody = new StringBuilder(eMailTemplate);

                        MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                        MessageBody.Replace("{FullName}", recipient.FirstName + " " + recipient.LastName);
                        MessageBody.Replace("{TicketNo}", objServiceTicket.ServiceTicketNumber);
                        MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                        MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);
                        MessageBody.Replace("{Note}", "<br/>" + TrailingNotes);

                        String CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
                                    <a href='" + ConfigurationSettings.AppSettings["siteurl"]
                                        + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
                                        + objServiceTicket.ServiceTicketID + "&uid=" + recipient.UserInfoID
                                        + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";

                        MessageBody.Replace("{CloseTicket}", CloseTicket);

                        String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + objServiceTicket.ServiceTicketID + "un" + Convert.ToInt64(recipient.UserInfoID) + "nt2en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                        new CommonMails().SendMailWithReplyTo(Convert.ToInt64(recipient.UserInfoID), "Support Ticket Center", Common.EmailFrom, Convert.ToString(recipient.Email), sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}