using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_RecentlyUpdated : PageBase
{
    #region Page Properties

    private Int32 CurrentPage
    {
        get
        {
            if (Convert.ToString(ViewState["CurrentPage"]) == String.Empty)
                return 0;
            else
                return (Int32)this.ViewState["CurrentPage"];
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }

    private Int32 PagerSize
    {
        get
        {
            if (this.ViewState["PagerSize"] == null)
                return Convert.ToInt32(Application["GRIDPAGERSIZE"]);
            else
                return Convert.ToInt16(this.ViewState["PagerSize"]);
        }
    }

    private Int32 FrmPg
    {
        get
        {
            if (Convert.ToString(ViewState["FrmPg"]) == String.Empty)
                return 1;
            else
                return (Int32)this.ViewState["FrmPg"];
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }

    private Int32 ToPg
    {
        get
        {
            if (Convert.ToString(ViewState["ToPg"]) == String.Empty)
                return this.PagerSize;
            else
                return (Int32)this.ViewState["ToPg"];
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }

    private List<SelectServiceTicketHistoryPerEmployeeResult> lstTickets;

    private String PopupPreference
    {
        get
        {
            if (Convert.ToString(this.ViewState["PopupPreference"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["PopupPreference"]);
        }
        set
        {
            this.ViewState["PopupPreference"] = value;
        }
    }

    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        try
        {
            if (!IsPostBack)
            {
                ((Label)Master.FindControl("lblPageHeading")).Text = "Recently Updated Tickets";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/TrackingCenter.aspx";

                PopupPreference = new PreferenceRepository().GetUserPreferenceValue(IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.Usertype, "SerTicSumVieNotPop");
                BindGrid(false);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Control Events

    protected void gvServiceTickets_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            switch (this.ViewState["SortExp"].ToString())
            {
                case "TicketNumber":
                    PlaceHolder placeholderTicketNumber = (PlaceHolder)e.Row.FindControl("placeholderTicketNumber");
                    break;
                case "TicketName":
                    PlaceHolder placeholderTicketName = (PlaceHolder)e.Row.FindControl("placeholderTicketName");
                    break;
                case "ContactName":
                    PlaceHolder placeholderContactName = (PlaceHolder)e.Row.FindControl("placeholderContactName");
                    break;
                case "StartDateNTime":
                    PlaceHolder placeholderStartDateNTime = (PlaceHolder)e.Row.FindControl("placeholderStartDateNTime");
                    break;
                case "DatePromisedFormatted":
                    PlaceHolder placeholderPriority = (PlaceHolder)e.Row.FindControl("placeholderDatePromisedFormatted");
                    break;
                case "TicketStatus":
                    PlaceHolder placeholderTicketStatus = (PlaceHolder)e.Row.FindControl("placeholderTicketStatus");
                    break;
            }
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnGvStatusID = (HiddenField)e.Row.FindControl("hdnGvStatusID");
            HiddenField hdnID = (HiddenField)e.Row.FindControl("hdnID");
            Int64 ID = Convert.ToInt64(hdnID.Value);
            Label lblDatePromisedFormatted = (Label)e.Row.FindControl("lblDatePromisedFormatted");
            HiddenField hdnDueDate = (HiddenField)e.Row.FindControl("hdnDueDate");
            LinkButton lnkTicketStatus = (LinkButton)e.Row.FindControl("lnkTicketStatus");
            LinkButton lnkFlag = (LinkButton)e.Row.FindControl("lnkFlag");
            HiddenField hdnIsFlagged = (HiddenField)e.Row.FindControl("hdnIsFlagged");
            HiddenField hdnIsSupplierTicket = (HiddenField)e.Row.FindControl("hdnIsSupplierTicket");
            LinkButton lblTicketName = (LinkButton)e.Row.FindControl("lblTicketName");

            lnkFlag.OnClientClick = "javascript:FlagTicket(" + ID + ");";
            lnkFlag.ToolTip = hdnIsFlagged.Value == "1" ? "Remove Flag" : "Add Flag";

            if (hdnDueDate != null && !String.IsNullOrEmpty(Convert.ToString(hdnDueDate.Value)))
            {
                if (Convert.ToInt64(hdnGvStatusID.Value) != new LookupRepository().GetIdByLookupNameNLookUpCode("Closed", "ServiceTicketStatus"))
                {
                    if (Convert.ToDateTime(hdnDueDate.Value) == DateTime.Now.Date)
                    {
                        lblDatePromisedFormatted.Style.Add("background-color", "#EEAD0E");
                        lblDatePromisedFormatted.Style.Add("color", "Black");
                    }
                    else if (Convert.ToDateTime(hdnDueDate.Value) < DateTime.Now.Date)
                    {
                        lblDatePromisedFormatted.Style.Add("background-color", "#CD0000");
                        lblDatePromisedFormatted.Style.Add("color", "Black");
                    }
                }
            }

            Image imgNote = (Image)e.Row.FindControl("imgNote");
            String JavaScript = "NotePopup(" + ID + ", " + 1 + ", '" + lblTicketName.ToolTip + "');";

            imgNote.Attributes.Add(PopupPreference, JavaScript);
            imgNote.ImageUrl = "~/Images/edit_green.png";

            HtmlImage imgFlag = (HtmlImage)e.Row.FindControl("imgFlag");
            imgFlag.Src = hdnIsFlagged.Value == "1" ? "~/Images/flag_red.png" : "~/Images/flag_gray.png";
        }
    }

    protected void gvServiceTickets_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Sort"))
        {
            GridView gvServiceTickets = (GridView)sender;

            if (this.ViewState["SortExp"] == null)
            {
                this.ViewState["SortExp"] = e.CommandArgument.ToString();
                this.ViewState["SortOrder"] = "ASC";
            }
            else
            {
                if (this.ViewState["SortExp"].ToString() == e.CommandArgument.ToString())
                {
                    if (this.ViewState["SortOrder"].ToString() == "ASC")
                        this.ViewState["SortOrder"] = "DESC";
                    else
                        this.ViewState["SortOrder"] = "ASC";

                }
                else
                {
                    this.ViewState["SortOrder"] = "ASC";
                    this.ViewState["SortExp"] = e.CommandArgument.ToString();
                }
            }

            BindGrid(true);
        }
        else if (e.CommandName == "TicketDetail")
        {
            IncentexGlobal.SearchTicketURL = Request.Url.ToString();
            Response.Redirect("~/MyAccount/MyServiceTicketDetail.aspx?id=" + Convert.ToString(e.CommandArgument), false);
        }
        else if (e.CommandName == "FlagTicket")
        {
            ServiceTicketFlagDetail objFlag;
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();

            objFlag = objSerTicRep.GetTicketFlag(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);

            if (objFlag != null)
            {
                objSerTicRep.DeleteTicketFlag(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);
            }
            else
            {
                objFlag = new ServiceTicketFlagDetail();
                objFlag.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objFlag.CreatedDate = DateTime.Now;
                objFlag.ServiceTicketID = Convert.ToInt64(e.CommandArgument);
                objSerTicRep.Insert(objFlag);
                objSerTicRep.SubmitChanges();
            }

            BindGrid(true);
        }
    }

    protected void lnkButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (!(String.IsNullOrEmpty(txtNote.Text.Trim())))
            {
                String NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs);
                new ServiceTicketRepository().InsertTicketNote(Convert.ToInt64(hdnPopupTicketID.Value), IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtNote.Text.Trim()), NoteFor, null);
                SendEmailToCECAIE();
                txtNote.Text = "";
                lnkClose_Click(null, null);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 TicketID = Convert.ToInt64(hdnPopupTicketID.Value);
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            ServiceTicket objServiceTicket = objSerTicRep.GetByTicketID(TicketID);

            if (objServiceTicket != null)
            {
                objSerTicRep.UpdateNoteReadFlag(TicketID, IncentexGlobal.CurrentMember.UserInfoID, new ServiceTicketRepository().GetUnreadNotes(objServiceTicket.ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID).ToList());
            }

            BindGrid(true);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Methods

    private void BindGrid(Boolean IsFromPaging)
    {
        lstTickets = new ServiceTicketRepository().SelectRecentlyUpdatedServiceTicketForCACE(IncentexGlobal.CurrentMember.UserInfoID);
        lblRecords.Text = lstTickets.Count.ToString();

        DataTable dataTable = new DataTable();
        dataTable = ListToDataTable(lstTickets);

        DataView myDataView = new DataView();
        myDataView = dataTable.DefaultView;
        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }

        if (!IsFromPaging)
        {
            this.FrmPg = 1;
            this.ToPg = this.PagerSize;
            this.CurrentPage = 0;
        }

        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;

        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;

        lnkbtnNext.CssClass = pds.IsLastPage ? "nextb" : "nextb pagin";
        lnkbtnPrevious.CssClass = pds.IsFirstPage ? "prevb" : "prevb pagin";

        gvServiceTickets.DataSource = pds;
        gvServiceTickets.DataBind();

        dtlPaging.DataSource = doPaging(pds.CurrentPageIndex, pds.PageCount);
        dtlPaging.DataBind();

        pagingtable.Visible = gvServiceTickets.Rows.Count > 0 ? true : false;
    }

    private static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    [WebMethod]
    public static String GetExistingNotes(String TicketID, String NoteType)
    {
        String ExistingNotes = String.Empty;
        try
        {
            ServiceTicketRepository objRepo = new ServiceTicketRepository();
            ExistingNotes = objRepo.TrailingNotes(Convert.ToInt64(TicketID), Convert.ToInt32(NoteType), false, "\n");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        return ExistingNotes;
    }

    private void SendEmailToCECAIE()
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

            String TrailingNotesIE = new ServiceTicketRepository().TrailingNotes(objServiceTicket.ServiceTicketID, 2, false, "<br/>");
            String TrailingNotes = new ServiceTicketRepository().TrailingNotes(objServiceTicket.ServiceTicketID, 1, false, "<br/>");

            List<vw_ServiceTicketNoteRecipient> lstRecipients = new List<vw_ServiceTicketNoteRecipient>();
            lstRecipients = new ServiceTicketRepository().GetNoteRecipientsByTicketID(objServiceTicket.ServiceTicketID).Where(le => le.SubscriptionFlag == true).ToList();
            foreach (vw_ServiceTicketNoteRecipient recipient in lstRecipients)
            {
                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                {
                    String UserName = Convert.ToString(recipient.LastName) + " " + Convert.ToString(recipient.FirstName);
                    StringBuilder MessageBody = new StringBuilder(eMailTemplate);
                    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                    MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
                    MessageBody.Replace("{TicketNo}", Convert.ToString(objServiceTicket.ServiceTicketID));
                    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                    MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);
                    MessageBody.Replace("{Note}", recipient.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || recipient.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) ? TrailingNotesIE : TrailingNotes);
                    MessageBody.Replace("{CloseTicket}", String.Empty);

                    String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(objServiceTicket.ServiceTicketID) + "un" + Convert.ToString(recipient.UserInfoID) + "nt1en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                    new CommonMails().SendMailWithReplyTo(Convert.ToInt64(recipient.UserInfoID), "Support Ticket Center", Common.EmailFrom, Convert.ToString(recipient.Email), sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
                }
            }
        }
    }

    #endregion

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument);
            BindGrid(true);
        }
    }

    protected void dtlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPaging = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPaging.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPaging.Enabled = false;
            lnkbtnPaging.Font.Bold = true;
            lnkbtnPaging.CssClass = "";
        }
    }

    private DataTable doPaging(Int32 CurrentPageIndex, Int32 PageCount)
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            Int32 tCurrentPg = CurrentPageIndex + 1;
            Int32 tToPg = this.ToPg;
            Int32 tFrmPg = this.FrmPg;

            if (tCurrentPg > tToPg)
            {
                tFrmPg = tToPg + 1;
                tToPg += this.PagerSize;
            }
            else if (tCurrentPg < tFrmPg)
            {
                tToPg = tFrmPg - 1;
                tFrmPg -= this.PagerSize;
            }

            if (tToPg > PageCount)
                tToPg = PageCount;

            for (Int32 i = tFrmPg - 1; i < tToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr["PageIndex"] = i;
                dr["PageText"] = i + 1;
                dt.Rows.Add(dr);
            }

            this.ToPg = tToPg;
            this.FrmPg = tFrmPg;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return dt;
    }

    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindGrid(true);
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindGrid(true);
    }

    #endregion
}