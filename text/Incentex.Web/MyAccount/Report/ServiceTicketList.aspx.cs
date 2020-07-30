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

public partial class MyAccount_Report_ServiceTicketList : PageBase
{
    #region Page Properties

    private String ReturnURL
    {
        get
        {
            if (Convert.ToString(this.ViewState["ReturnURL"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["ReturnURL"]);
        }
        set
        {
            this.ViewState["ReturnURL"] = value;
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

    private Int64? WorkGroupID
    {
        get
        {
            if (Convert.ToString(this.ViewState["WorkGroupID"]) == String.Empty)
                return null;
            else
                return Convert.ToInt64(this.ViewState["WorkGroupID"]);
        }
        set
        {
            this.ViewState["WorkGroupID"] = value;
        }
    }

    private Int64? BaseStationID
    {
        get
        {
            if (Convert.ToString(this.ViewState["BaseStationID"]) == String.Empty)
                return null;
            else
                return Convert.ToInt64(this.ViewState["BaseStationID"]);
        }
        set
        {
            this.ViewState["BaseStationID"] = value;
        }
    }

    private Int64? GenderID
    {
        get
        {
            if (Convert.ToString(this.ViewState["GenderID"]) == String.Empty)
                return null;
            else
                return Convert.ToInt64(this.ViewState["GenderID"]);
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

    private Int64? IEID
    {
        get
        {
            if (Convert.ToString(this.ViewState["IEID"]) == String.Empty)
                return null;
            else
                return Convert.ToInt64(this.ViewState["IEID"]);
        }
        set
        {
            this.ViewState["IEID"] = value;
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

    private Boolean NoActivity
    {
        get
        {
            return Convert.ToBoolean(this.ViewState["NoActivity"]);
        }
        set
        {
            this.ViewState["NoActivity"] = value;
        }
    }

    private List<SelectServiceTicketsBySearchCriteriaForReportResult> lstTickets
    {
        get
        {
            if (Convert.ToString(ViewState["lstTickets"]) == String.Empty)
                return null;
            else
                return (List<SelectServiceTicketsBySearchCriteriaForReportResult>)ViewState["lstTickets"];
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
        CheckLogin();

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Support Tickets";

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SupplierEmployee))
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Visible = false;
            }
            else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            {
                hdnIsSuperAdmin.Value = "1";
            }

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString)))
            {
                if (String.IsNullOrEmpty(Request.QueryString["ie"]))
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["con"]))
                        this.ContactID = Convert.ToInt32(Request.QueryString["con"]);
                    else
                        this.ContactID = null;

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

                    //
                    if (!String.IsNullOrEmpty(Request.QueryString["bs"]))
                        this.BaseStationID = Convert.ToInt64(Request.QueryString["bs"]);
                    else
                        this.BaseStationID = null;

                    if (!String.IsNullOrEmpty(Request.QueryString["wg"]))
                        this.WorkGroupID = Convert.ToInt64(Request.QueryString["wg"]);
                    else
                        this.WorkGroupID = null;

                    if (!String.IsNullOrEmpty(Request.QueryString["gn"]))
                        this.GenderID = Convert.ToInt64(Request.QueryString["gn"]);
                    else
                        this.GenderID = null;
                    //

                    if (!String.IsNullOrEmpty(Request.QueryString["na"]) && Request.QueryString["na"] == "1")
                        this.NoActivity = true;
                    else
                        this.NoActivity = false;

                    this.TicketName = Request.QueryString["nam"];
                    this.TicketNumber = Request.QueryString["num"];
                    this.DateNeeded = Request.QueryString["dn"];
                    this.KeyWord = Request.QueryString["kw"];
                    this.FromDate = Request.QueryString["FromDate"];
                    this.ToDate = Request.QueryString["ToDate"];
                    if (Request.QueryString["ChartSubReport"] != null && Request.QueryString["ChartSubReport"] != "")
                    {
                        if (Convert.ToString(Request.QueryString["ChartSubReport"]) == "Help Tickets by Workgroup" || Convert.ToString(Request.QueryString["ChartSubReport"]) == "Help Tickets by Location" || Convert.ToString(Request.QueryString["ChartSubReport"]) == "Help Tickets by Type")
                            this.ReturnURL = "~/MyAccount/Report/ServiceLevelScorecardReport.aspx";
                    }

                    this.IEID = null;
                }
                else
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["ie"]))
                        this.IEID = Convert.ToInt64(Request.QueryString["ie"]);
                    else
                        this.IEID = null;
                }

                //Now combine all queryString with returnurl
                if (ReturnURL != "")
                {
                    for (Int32 i = 0; i < Request.QueryString.Count; i++)
                    {
                        if (!String.IsNullOrEmpty(Request.QueryString[i]))
                        {
                            if (i == 0)
                                this.ReturnURL += "?SubReport";
                            else
                                this.ReturnURL += "&" + Request.QueryString.AllKeys[i];

                            this.ReturnURL += "=" + Request.QueryString[i].ToString();
                        }
                    }
                }
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = this.ReturnURL;

                if (String.IsNullOrEmpty(Request.QueryString["ex"]))
                    Session.Remove("ExpandedStatuses");
            }
            else
            {
                Session.Remove("ExpandedStatuses");
            }

            PopupPreference = new PreferenceRepository().GetUserPreferenceValue(IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.Usertype, "SerTicSumVieNotPop");
            BindRepeater();
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
            GridView gvServiceTickets = (GridView)sender;
            HiddenField hdnRepStatusID = (HiddenField)gvServiceTickets.Parent.Parent.FindControl("hdnRepStatusID");
            HiddenField hdnGvStatusID = (HiddenField)e.Row.FindControl("hdnGvStatusID");
            Int64 StatusID = Convert.ToInt64(hdnRepStatusID.Value);
            HiddenField hdnID = (HiddenField)e.Row.FindControl("hdnID");
            HiddenField hdnUnReadNotesExists = (HiddenField)e.Row.FindControl("hdnUnReadNotesExists");
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
                JavaScript = "NotePopup(" + ID + ", " + 2 + ", " + StatusID + ", '" + lblTicketName.ToolTip + "', " + hdnIsSupplierTicket.Value + ");";
            else
                JavaScript = "NotePopup(" + ID + ", " + 3 + ", " + StatusID + ", '" + lblTicketName.ToolTip + "', " + hdnIsSupplierTicket.Value + ");";

            imgNote.Attributes.Add(PopupPreference, JavaScript);
            imgNote.ImageUrl = Convert.ToInt64(hdnUnReadNotesExists.Value) > 0 ? "~/Images/edit_green.png" : "~/Images/edit_gray.png";

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

            BindGrid(gvServiceTickets, true);
        }
        else if (e.CommandName == "TicketDetail")
        {
            StoreExpandedStatuses();
            IncentexGlobal.SearchTicketURL = Request.Url.ToString();
            Response.Redirect("~/MyAccount/MyServiceTicketDetail.aspx?id=" + Convert.ToString(e.CommandArgument), false);
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
            BindRepeater();
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

            BindRepeater();
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

            BindRepeater();

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
            Int64 StatusID = Convert.ToInt64(hdnPopupStatusID.Value);
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

            BindRepeater();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void repServiceTickets_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HiddenField hdnIamExpanded = (HiddenField)e.Item.FindControl("hdnIamExpanded");
            HiddenField hdnRepStatusID = (HiddenField)e.Item.FindControl("hdnRepStatusID");

            if (Convert.ToString(Session["ExpandedStatuses"]).Split(',').Contains(hdnRepStatusID.Value))
            {
                hdnIamExpanded.Value = "true";
            }

            GridView gvServiceTickets = (GridView)e.Item.FindControl("gvServiceTickets");
            BindGrid(gvServiceTickets, false);
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

        BindRepeater();
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// For filling the support ticket status dropdown
    /// </summary>
    private void BindRepeater()
    {
        try
        {
            StoreExpandedStatuses();
            LookupDA sServiceSupportStatus = new LookupDA();
            LookupBE sServiceSupportStatusBE = new LookupBE();
            sServiceSupportStatusBE.SOperation = "selectall";
            sServiceSupportStatusBE.iLookupCode = "ServiceTicketStatus";
            DataSet ds = new DataSet();
            ds = sServiceSupportStatus.LookUp(sServiceSupportStatusBE);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            DataRow dr;

            if (this.IEID != null)
            {
                dt.DefaultView.RowFilter = " sLookupName = 'Open' ";
                dt = dt.DefaultView.ToTable();
                dt.Rows[0]["sLookupName"] = "All";
            }

            if (this.StatusID != null)
            {
                dt.DefaultView.RowFilter = " iLookupID = " + this.StatusID + " ";
            }

            dt.DefaultView.Sort = "sLookupName";
            dt = dt.DefaultView.ToTable();

            Int32 InsertIndex = 0;

            if (!this.NoActivity)
            {
                dr = dt.NewRow();
                dr["iLookupID"] = "-2";
                dr["sLookupName"] = "Recently Updated";
                dt.Rows.InsertAt(dr, InsertIndex);
                InsertIndex += 1;
            }

            dr = dt.NewRow();
            dr["iLookupID"] = "-1";
            dr["sLookupName"] = "Flagged";
            dt.Rows.InsertAt(dr, InsertIndex);

            if (this.IEID == null)
            {
                dr = dt.NewRow();
                dr["iLookupID"] = "0";
                dr["sLookupName"] = "All";
                dt.Rows.InsertAt(dr, InsertIndex + 1);
            }

            this.pds = new Dictionary<Int64, PagedDataSource>();
            this.FrmPg = new Dictionary<Int64, Int32>();
            this.ToPg = new Dictionary<Int64, Int32>();
            this.CurrentPage = new Dictionary<Int64, Int32>();

            this.lstTickets = new List<SelectServiceTicketsBySearchCriteriaForReportResult>();

            // if (this.IEID == null)
            // {
            this.lstTickets = new ReportForEmployeeRepository().SelectServiceTicketsBySearchCriteriaForReport(IncentexGlobal.CurrentMember.CompanyId, this.ContactID, this.OpenedByID, this.StatusID, this.OwnerID, this.TicketName, this.TicketNumber, this.DateNeeded, this.SupplierID, this.TypeOfRequestID, this.KeyWord, this.SubOwnerID, this.FromDate, this.ToDate, IncentexGlobal.CurrentMember.UserInfoID, this.WorkGroupID, this.BaseStationID, this.NoActivity).ToList<SelectServiceTicketsBySearchCriteriaForReportResult>();
            // }
            // else
            //  {
            //     this.lstTickets = new ServiceTicketRepository().SelectServiceTicketAssociatedWithIEs(this.IEID);
            //  }

            repServiceTickets.DataSource = dt.DefaultView.ToTable();
            repServiceTickets.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void BindGrid(GridView gvServiceTickets, Boolean FromPaging)
    {
        DataList dtlPaging = (DataList)gvServiceTickets.Parent.Parent.FindControl("dtlPaging");
        HtmlControl pagingtable = (HtmlControl)gvServiceTickets.Parent.Parent.FindControl("pagingtable");
        LinkButton lnkDelete = (LinkButton)gvServiceTickets.Parent.Parent.FindControl("lnkDelete");
        LinkButton lnkbtnNext = (LinkButton)gvServiceTickets.Parent.Parent.FindControl("lnkbtnNext");
        LinkButton lnkbtnPrevious = (LinkButton)gvServiceTickets.Parent.Parent.FindControl("lnkbtnPrevious");
        HtmlControl dvCollapsible = (HtmlControl)gvServiceTickets.Parent.Parent.FindControl("dvCollapsible");

        HiddenField hdnRepStatusID = (HiddenField)gvServiceTickets.Parent.Parent.FindControl("hdnRepStatusID");
        Int64 StatusID = Convert.ToInt64(hdnRepStatusID.Value);

        DataTable dataTable = new DataTable();

        //Binding recently updated tickets
        if (StatusID == -2)
        {
            dvCollapsible.Attributes.Add("total", Convert.ToString(this.lstTickets.Where(le => le.UnReadNotesExists == 1).ToList().Count));
            dataTable = ListToDataTable(this.lstTickets.Where(le => le.UnReadNotesExists == 1).ToList());
        }
        //Binding flagged tickets
        else if (StatusID == -1)
        {
            dvCollapsible.Attributes.Add("total", Convert.ToString(this.lstTickets.Where(le => le.IsFlagged == 1).ToList().Count));
            dataTable = ListToDataTable(this.lstTickets.Where(le => le.IsFlagged == 1).ToList());
        }
        //Binding all tickets
        else if (StatusID == 0)
        {
            dvCollapsible.Attributes.Add("total", Convert.ToString(this.lstTickets.Count));
            dataTable = ListToDataTable(this.lstTickets);
        }
        //Binding tickets status wise
        else
        {
            dvCollapsible.Attributes.Add("total", Convert.ToString(this.lstTickets.Where(le => le.TicketStatusID == StatusID).ToList().Count));
            dataTable = ListToDataTable(this.lstTickets.Where(le => le.TicketStatusID == StatusID).ToList());
        }

        DataView myDataView = new DataView();
        myDataView = dataTable.DefaultView;
        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }

        if (!FromPaging)
        {
            this.FrmPg.Remove(StatusID);
            this.FrmPg.Add(StatusID, 1);

            this.ToPg.Remove(StatusID);
            this.ToPg.Add(StatusID, this.PagerSize);

            this.CurrentPage.Remove(StatusID);
            this.CurrentPage.Add(StatusID, 0);
        }

        PagedDataSource tPds = new PagedDataSource();

        tPds.DataSource = myDataView;
        tPds.AllowPaging = true;
        tPds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        tPds.CurrentPageIndex = CurrentPage.FirstOrDefault(le => le.Key == StatusID).Value;

        this.pds.Remove(StatusID);
        this.pds.Add(StatusID, tPds);

        lnkbtnNext.Enabled = !tPds.IsLastPage;
        lnkbtnPrevious.Enabled = !tPds.IsFirstPage;

        lnkbtnNext.CssClass = tPds.IsLastPage ? "nextb" : "nextb pagin";
        lnkbtnPrevious.CssClass = tPds.IsFirstPage ? "prevb" : "prevb pagin";

        gvServiceTickets.DataSource = tPds;
        gvServiceTickets.DataBind();

        dtlPaging.DataSource = doPaging(StatusID);
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
                    MessageBody.Replace("{Sender}", "");
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
                    MessageBody.Replace("{Sender}", "");
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

    private void StoreExpandedStatuses()
    {
        if (repServiceTickets.Items.Count > 0)
        {
            String ExpandedStatuses = String.Empty;
            foreach (RepeaterItem repItem in repServiceTickets.Items)
            {
                HiddenField hdnRepStatusID = (HiddenField)repItem.FindControl("hdnRepStatusID");
                HiddenField hdnIamExpanded = (HiddenField)repItem.FindControl("hdnIamExpanded");

                if (hdnIamExpanded != null && hdnRepStatusID != null)
                {
                    if (Convert.ToBoolean(hdnIamExpanded.Value))
                    {
                        if (String.IsNullOrEmpty(ExpandedStatuses))
                        {
                            ExpandedStatuses = hdnRepStatusID.Value;
                        }
                        else
                        {
                            ExpandedStatuses += "," + hdnRepStatusID.Value;
                        }
                    }
                }
            }

            Session["ExpandedStatuses"] = ExpandedStatuses;
        }
    }

    #endregion

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            GridView gvServiceTickets = (GridView)e.Item.Parent.Parent.FindControl("gvServiceTickets");
            HiddenField hdnRepStatusID = (HiddenField)e.Item.Parent.Parent.FindControl("hdnRepStatusID");
            Int64 StatusID = Convert.ToInt64(hdnRepStatusID.Value);
            CurrentPage[StatusID] = Convert.ToInt16(e.CommandArgument);
            BindGrid(gvServiceTickets, true);
        }
    }

    protected void dtlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        HiddenField hdnRepStatusID = (HiddenField)e.Item.Parent.Parent.FindControl("hdnRepStatusID");
        Int64 StatusID = Convert.ToInt64(hdnRepStatusID.Value);
        LinkButton lnkbtnPaging = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPaging.Text == Convert.ToString(CurrentPage.FirstOrDefault(le => le.Key == StatusID).Value + 1))
        {
            lnkbtnPaging.Enabled = false;
            lnkbtnPaging.Font.Bold = true;
            lnkbtnPaging.CssClass = "";
        }
    }

    private DataTable doPaging(Int64 StatusID)
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            Int32 tCurrentPg = this.pds[StatusID].CurrentPageIndex + 1;
            Int32 tToPg = this.PagerSize;
            Int32 tFrmPg = this.FrmPg[StatusID];

            if (tCurrentPg > tToPg)
            {
                tFrmPg = tToPg + 1;
                tToPg += this.PagerSize;
            }

            if (tCurrentPg < tFrmPg)
            {
                tToPg = tFrmPg - 1;
                tFrmPg -= this.PagerSize;
            }

            if (tToPg > this.pds[StatusID].PageCount)
                tToPg = this.pds[StatusID].PageCount;

            for (Int32 i = tFrmPg - 1; i < tToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr["PageIndex"] = i;
                dr["PageText"] = i + 1;
                dt.Rows.Add(dr);
            }

            this.ToPg[StatusID] = tToPg;
            this.FrmPg[StatusID] = tFrmPg;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return dt;
    }

    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        LinkButton lnkbtnPrevious = (LinkButton)sender;
        HiddenField hdnRepStatusID = (HiddenField)lnkbtnPrevious.Parent.FindControl("hdnRepStatusID");
        Int64 StatusID = Convert.ToInt64(hdnRepStatusID.Value);
        CurrentPage[StatusID] -= 1;

        GridView gvServiceTickets = (GridView)lnkbtnPrevious.Parent.FindControl("gvServiceTickets");
        BindGrid(gvServiceTickets, true);
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        LinkButton lnkbtnNext = (LinkButton)sender;
        HiddenField hdnRepStatusID = (HiddenField)lnkbtnNext.Parent.FindControl("hdnRepStatusID");
        Int64 StatusID = Convert.ToInt64(hdnRepStatusID.Value);
        CurrentPage[StatusID] += 1;

        GridView gvServiceTickets = (GridView)lnkbtnNext.Parent.FindControl("gvServiceTickets");
        BindGrid(gvServiceTickets, true);
    }

    #endregion
}
