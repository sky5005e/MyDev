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

public partial class admin_ServiceTicketCenter_RecentlyUpdated : PageBase
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

    private List<ServiceTicketsSearchResult> lstTickets
    {
        get
        {
            if (Convert.ToString(ViewState["lstTickets"]) == String.Empty)
                return null;
            else
                return (List<ServiceTicketsSearchResult>)ViewState["lstTickets"];
        }
        set
        {
            ViewState["lstTickets"] = value;
        }
    }

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
        try
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

                ((Label)Master.FindControl("lblPageHeading")).Text = "Recently Updated Tickets";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/ServiceTicketCenter/SearchServiceTicket.aspx";

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SupplierEmployee))
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).Visible = false;
                }
                else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
                {
                    hdnIsSuperAdmin.Value = "1";
                }

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
            HtmlContainerControl lblDatePromisedFormatted = (HtmlContainerControl)e.Row.FindControl("lblDatePromisedFormatted");
            HiddenField hdnDueDate = (HiddenField)e.Row.FindControl("hdnDueDate");
            LinkButton lnkTicketStatus = (LinkButton)e.Row.FindControl("lnkTicketStatus");
            LinkButton lnkFlag = (LinkButton)e.Row.FindControl("lnkFlag");
            HiddenField hdnIsFlagged = (HiddenField)e.Row.FindControl("hdnIsFlagged");
            HiddenField hdnIsSupplierTicket = (HiddenField)e.Row.FindControl("hdnIsSupplierTicket");
            LinkButton lblTicketName = (LinkButton)e.Row.FindControl("lblTicketName");

            lnkTicketStatus.OnClientClick = "javascript:return CloseTicket(" + ID + ");";
            lnkFlag.OnClientClick = "javascript:FlagTicket(" + ID + ");";
            lnkFlag.ToolTip = hdnIsFlagged.Value == "1" ? "Remove Flag" : "Add Flag";

            String JavaScript = String.Empty;

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

                JavaScript = "DueDatePopup(" + ID + ", '" + Convert.ToDateTime(hdnDueDate.Value).ToString("MM/dd/yyyy") + "');";
            }
            else
            {
                JavaScript = "DueDatePopup(" + ID + ", '');";
            }

            lblDatePromisedFormatted.Attributes.Add("ondblclick", JavaScript);
            lblDatePromisedFormatted.Style.Add("cursor", "pointer");

            Image imgNote = (Image)e.Row.FindControl("imgNote");

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                JavaScript = "NotePopup(" + ID + ", " + 2 + ", '" + lblTicketName.ToolTip + "', " + hdnIsSupplierTicket.Value + ");";
            else
                JavaScript = "NotePopup(" + ID + ", " + 3 + ", '" + lblTicketName.ToolTip + "', " + hdnIsSupplierTicket.Value + ");";

            imgNote.Attributes.Add(PopupPreference, JavaScript);
            imgNote.ImageUrl = "~/Images/edit_green.png";

            HtmlImage imgFlag = (HtmlImage)e.Row.FindControl("imgFlag");
            imgFlag.Src = hdnIsFlagged.Value == "1" ? "~/Images/flag_red.png" : "~/Images/flag_gray.png";
        }
    }

    protected void gvServiceTickets_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                GridView gvServiceTickets = sender as GridView;
                CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                CheckBox chkBxHeader = (CheckBox)gvServiceTickets.HeaderRow.FindControl("chkBxHeader");
                chkBxSelect.Attributes["onclick"] = String.Format("javascript:ChildClick(this,'{0}');", chkBxHeader.ClientID);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
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
            Response.Redirect("~/admin/ServiceTicketCenter/ServiceTicketDetail.aspx?id=" + Convert.ToString(e.CommandArgument), false);
        }
        else if (e.CommandName == "CloseTicket")
        {
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            ServiceTicket objServiceTicket = objSerTicRep.GetByTicketID(Convert.ToInt64(e.CommandArgument));

            if (objServiceTicket != null)
            {
                objServiceTicket.TicketStatusID = new LookupRepository().GetIdByLookupNameNLookUpCode("Closed", "ServiceTicketStatus");
                objServiceTicket.EndDate = DateTime.Now;
                objServiceTicket.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objServiceTicket.UpdatedDate = DateTime.Now;
                objSerTicRep.SubmitChanges();

                vw_ServiceTicket vwServiceTicket = objSerTicRep.GetFirstByID(Convert.ToInt64(e.CommandArgument));

                if (vwServiceTicket != null)
                {
                    if (vwServiceTicket.ContactID != null)
                        SendCloseTicketNotification(vwServiceTicket.ServiceTicketID, vwServiceTicket.ContactID, 1, vwServiceTicket.ContactName, vwServiceTicket.ContactEmail, vwServiceTicket.ServiceTicketNumber);
                    else if (vwServiceTicket.SupplierID != null)
                        SendCloseTicketNotification(vwServiceTicket.ServiceTicketID, vwServiceTicket.SupplierID, 3, vwServiceTicket.SupplierName, vwServiceTicket.SupplierEmail, vwServiceTicket.ServiceTicketNumber);
                    else
                        SendCloseTicketNotification(vwServiceTicket.ServiceTicketID, null, 1, "User", vwServiceTicket.TicketEmail, vwServiceTicket.ServiceTicketNumber);
                }

                LogAction("Changed ticket status to : Closed.", "IEActivity");
            }
            BindGrid(true);
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

    /// <summary>
    /// delete record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean IsChecked = false;
            LinkButton lnkDelete = (LinkButton)sender;
            GridView gvServiceTickets = (GridView)lnkDelete.Parent.FindControl("gvServiceTickets");
            foreach (GridViewRow gvRow in gvServiceTickets.Rows)
            {
                CheckBox chkBxSelect = (CheckBox)gvRow.FindControl("chkBxSelect");
                HiddenField hdnID = (HiddenField)gvRow.FindControl("hdnID");

                if (chkBxSelect.Checked)
                {
                    DeleteTicket(Convert.ToInt64(hdnID.Value));
                    IsChecked = true;
                }
            }

            BindGrid(true);

            if (IsChecked)
            {
                lblMsgList.Text = "Selected record(s) deleted successfully.";
            }
            else
            {
                lblMsgList.Text = "Please select record(s) to delete.";
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsgList.Text = "Error in deleting record.";
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkNoteHisForIE_Click(object sender, EventArgs e)
    {
        try
        {
            if (!(String.IsNullOrEmpty(txtNoteIE.Text.Trim())))
            {
                String NoteFor = String.Empty;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                {
                    if (!chkCustomerNote.Checked)
                    {
                        NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);
                        new ServiceTicketRepository().InsertTicketNote(Convert.ToInt64(hdnPopupTicketID.Value), IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtNoteIE.Text.Trim()), NoteFor, "IEInternalNotes");
                        SendEMailToIE();
                    }
                    else
                    {
                        if (Convert.ToInt32(hdnPopupIsSupplierTicket.Value) == 0)
                        {
                            NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs);
                            new ServiceTicketRepository().InsertTicketNote(Convert.ToInt64(hdnPopupTicketID.Value), IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtNoteIE.Text.Trim()), NoteFor, null);
                            SendEmailToCECAIE();
                        }
                        else
                        {
                            NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketSupps);
                            new ServiceTicketRepository().InsertTicketNote(Convert.ToInt64(hdnPopupTicketID.Value), IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtNoteIE.Text.Trim()), NoteFor, null);
                            SendEmailToIESuppSE();
                        }
                    }
                }
                else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SupplierEmployee))
                {
                    NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketSupps);
                    new ServiceTicketRepository().InsertTicketNote(Convert.ToInt64(hdnPopupTicketID.Value), IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtNoteIE.Text.Trim()), NoteFor, null);
                    SendEmailToIESuppSE();
                }

                txtNoteIE.Text = "";
            }

            lnkClose_Click(null, null);
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

                if (chkCloseTicket.Checked || chkFlagTicket.Checked)
                {
                    objSerTicRep = new ServiceTicketRepository();

                    if (chkCloseTicket.Checked)
                    {
                        objServiceTicket.TicketStatusID = new LookupRepository().GetIdByLookupNameNLookUpCode("Closed", "ServiceTicketStatus");
                        objServiceTicket.EndDate = DateTime.Now;

                        vw_ServiceTicket vwServiceTicket = objSerTicRep.GetFirstByID(TicketID);

                        if (vwServiceTicket != null)
                        {
                            if (vwServiceTicket.ContactID != null)
                                SendCloseTicketNotification(vwServiceTicket.ServiceTicketID, vwServiceTicket.ContactID, 1, vwServiceTicket.ContactName, vwServiceTicket.ContactEmail, vwServiceTicket.ServiceTicketNumber);
                            else if (vwServiceTicket.SupplierID != null)
                                SendCloseTicketNotification(vwServiceTicket.ServiceTicketID, vwServiceTicket.SupplierID, 3, vwServiceTicket.SupplierName, vwServiceTicket.SupplierEmail, vwServiceTicket.ServiceTicketNumber);
                            else
                                SendCloseTicketNotification(vwServiceTicket.ServiceTicketID, null, 1, "User", vwServiceTicket.TicketEmail, vwServiceTicket.ServiceTicketNumber);
                        }

                        LogAction("Changed ticket status to : Closed.", "IEActivity");
                    }

                    if (chkFlagTicket.Checked)
                    {
                        ServiceTicketFlagDetail objFlag;

                        objFlag = objSerTicRep.GetTicketFlag(TicketID, IncentexGlobal.CurrentMember.UserInfoID);

                        if (objFlag != null)
                        {
                            objSerTicRep.DeleteTicketFlag(TicketID, IncentexGlobal.CurrentMember.UserInfoID);

                            LogAction("Removed Flag.", "IEActivity");
                        }
                        else
                        {
                            objFlag = new ServiceTicketFlagDetail();
                            objFlag.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                            objFlag.CreatedDate = DateTime.Now;
                            objFlag.ServiceTicketID = TicketID;
                            objSerTicRep.Insert(objFlag);
                            objSerTicRep.SubmitChanges();

                            LogAction("Added Flag.", "IEActivity");
                        }
                    }

                    objServiceTicket.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                    objServiceTicket.UpdatedDate = DateTime.Now;
                    objSerTicRep.SubmitChanges();
                }
            }

            BindGrid(true);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnChangeDueDate_Click(object sender, EventArgs e)
    {
        ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
        ServiceTicket objServiceTicket = objSerTicRep.GetByTicketID(Convert.ToInt64(hdnPopupTicketID.Value));

        if (objServiceTicket != null)
        {
            if (!String.IsNullOrEmpty(txtDueDate.Text.Trim()))
            {
                objServiceTicket.DatePromised = DateTime.ParseExact(txtDueDate.Text.Trim(),
                                                Common.DateFormats,
                                                CultureInfo.InvariantCulture,
                                                DateTimeStyles.AssumeLocal);

                LogAction("Changed date needed to : " + txtDueDate.Text.Trim() + ".", "IEActivity");
            }
            else
            {
                objServiceTicket.DatePromised = null;
                LogAction("Removed date needed.", "IEActivity");
            }

            objServiceTicket.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objServiceTicket.UpdatedDate = DateTime.Now;
            objSerTicRep.SubmitChanges();

            lblMsgList.Text = "Due date changed successfully...";
        }

        BindGrid(true);
    }

    #endregion

    #region Page Methods

    private void BindGrid(Boolean IsFromPaging)
    {
        lstTickets = new ServiceTicketRepository().SelectRecentlyUpdatedServiceTicket(IncentexGlobal.CurrentMember.UserInfoID);
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

        if (gvServiceTickets.Rows.Count == 0)
        {
            pagingtable.Visible = false;
            lnkDelete.Visible = false;
        }
        else
        {
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            {
                gvServiceTickets.Columns[0].Visible = true;
                lnkDelete.Visible = true;
            }
            else
            {
                gvServiceTickets.Columns[0].Visible = false;
                lnkDelete.Visible = false;
            }

            pagingtable.Visible = true;
        }
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

    private void DeleteTicket(Int64 TicketID)
    {
        new ServiceTicketRepository().DeleteById(Convert.ToInt64(TicketID), IncentexGlobal.CurrentMember.UserInfoID);
    }

    [WebMethod]
    public static String GetExistingNotes(String TicketID, String NoteType)
    {
        String ExistingNotes = String.Empty;
        try
        {
            ServiceTicketRepository objRepo = new ServiceTicketRepository();
            ExistingNotes = objRepo.TrailingNotes(Convert.ToInt64(TicketID), Convert.ToInt32(NoteType), false, "\n");
            //Merge OrderView Note Here
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        return ExistingNotes;
    }

    private void SendEMailToIE()
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

    private void SendEmailToIESuppSE()
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

            String TrailingNotes = new ServiceTicketRepository().TrailingNotes(objServiceTicket.ServiceTicketID, 3, false, "<br/>");

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
                    MessageBody.Replace("{Note}", TrailingNotes);

                    String CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
                                    <a href='" + ConfigurationSettings.AppSettings["siteurl"]
                                    + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
                                    + objServiceTicket.ServiceTicketID + "&uid=" + recipient.UserInfoID
                                    + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";

                    MessageBody.Replace("{CloseTicket}", CloseTicket);

                    String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(objServiceTicket.ServiceTicketID) + "un" + Convert.ToString(recipient.UserInfoID) + "nt3en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                    new CommonMails().SendMailWithReplyTo(Convert.ToInt64(recipient.UserInfoID), "Support Ticket Center", Common.EmailFrom, Convert.ToString(recipient.Email), sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
                }
            }
        }
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

    private void SendCloseTicketNotification(Int64? TicketID, Int64? tUserInfoID, Int32 NoteType, String FullName, String Email, String TicketNo)
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

            String sUserInfoID = tUserInfoID != null ? Convert.ToString(tUserInfoID) : "annx";

            StringBuilder MessageBody = new StringBuilder(eMailTemplate);

            MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            MessageBody.Replace("{FullName}", FullName);
            MessageBody.Replace("{TicketNo}", TicketNo);

            String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + TicketID + "un" + sUserInfoID + "nt" + NoteType + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
            new CommonMails().SendMailWithReplyTo(Convert.ToInt64(sUserInfoID), "Support Ticket Center", Common.EmailFrom, Email, sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void LogAction(String Content, String SpecificNoteFor)
    {
        String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);

        NoteDetail objComNot = new NoteDetail();
        NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();

        objComNot.Notecontents = Content;
        objComNot.NoteFor = strNoteFor;
        objComNot.SpecificNoteFor = SpecificNoteFor;
        objComNot.ForeignKey = Convert.ToInt64(hdnPopupTicketID.Value);
        objComNot.CreateDate = System.DateTime.Now;
        objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objComNot.UpdateDate = System.DateTime.Now;
        objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

        objCompNoteHistRepos.Insert(objComNot);
        objCompNoteHistRepos.SubmitChanges();
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