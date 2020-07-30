using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_TrackServiceTicket : PageBase
{
    #region Page Properties

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

    private Boolean QuickView
    {
        get
        {
            return Convert.ToBoolean(this.ViewState["QuickView"]);
        }
        set
        {
            this.ViewState["QuickView"] = value;
        }
    }

    private List<SelectServiceTicketHistoryPerEmployeeResult> lstTickets
    {
        get
        {
            if (Convert.ToString(ViewState["lstTickets"]) == String.Empty)
                return null;
            else
                return (List<SelectServiceTicketHistoryPerEmployeeResult>)ViewState["lstTickets"];
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

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Track My Tickets";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/TrackingCenter.aspx";

            if (Session["CoupaID"] == null)
            {
                if (Request.QueryString["qv"] == "1")
                    this.QuickView = true;

                PopupPreference = new PreferenceRepository().GetUserPreferenceValue(IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.Usertype, "SerTicSumVieNotPop");
                BindRepeater();
            }
        }
    }

    #endregion

    #region Page Control Events

    protected void gvServiceTickets_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            switch (Convert.ToString(this.ViewState["SortExp"]))
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
            Label lblDatePromisedFormatted = (Label)e.Row.FindControl("lblDatePromisedFormatted");
            HiddenField hdnDueDate = (HiddenField)e.Row.FindControl("hdnDueDate");
            LinkButton lblTicketName = (LinkButton)e.Row.FindControl("lblTicketName");
            LinkButton lnkFlag = (LinkButton)e.Row.FindControl("lnkFlag");
            HiddenField hdnIsFlagged = (HiddenField)e.Row.FindControl("hdnIsFlagged");

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
            String JavaScript = "NotePopup(" + ID + ", " + 1 + ", " + StatusID + ", '" + lblTicketName.ToolTip + "');";
            imgNote.Attributes.Add(PopupPreference, JavaScript);
            imgNote.ImageUrl = Convert.ToInt64(hdnUnReadNotesExists.Value) > 0 ? "~/Images/edit_green.png" : "~/Images/edit_gray.png";

            HtmlImage imgFlag = (HtmlImage)e.Row.FindControl("imgFlag");
            imgFlag.Src = hdnIsFlagged.Value == "1" ? "~/Images/flag_red.png" : "~/Images/flag_gray.png";
        }
    }

    protected void gvServiceTickets_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Sort")
        {
            GridView gvServiceTickets = (GridView)sender;

            if (this.ViewState["SortExp"] == null)
            {
                this.ViewState["SortExp"] = Convert.ToString(e.CommandArgument);
                this.ViewState["SortOrder"] = "ASC";
            }
            else
            {
                if (Convert.ToString(this.ViewState["SortExp"]) == Convert.ToString(e.CommandArgument))
                {
                    if (Convert.ToString(this.ViewState["SortOrder"]) == "ASC")
                        this.ViewState["SortOrder"] = "DESC";
                    else
                        this.ViewState["SortOrder"] = "ASC";

                }
                else
                {
                    this.ViewState["SortOrder"] = "ASC";
                    this.ViewState["SortExp"] = Convert.ToString(e.CommandArgument);
                }
            }

            BindGrid(gvServiceTickets, true);
        }
        else if (e.CommandName == "TicketDetail")
        {
            StoreExpandedStatuses();
            IncentexGlobal.SearchTicketURL = Convert.ToString(Request.Url);
            Response.Redirect("~/MyAccount/MyServiceTicketDetail.aspx?id=" + Convert.ToString(e.CommandArgument), false);
        }
        else if (e.CommandName == "FlagTicket")
        {
            ServiceTicketFlagDetail objFlag;
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();

            objFlag = objSerTicRep.GetTicketFlag(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);

            if (objFlag != null)
                objSerTicRep.DeleteTicketFlag(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);
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

    protected void lnkButton_Click(Object sender, EventArgs e)
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

    protected void lnkClose_Click(Object sender, EventArgs e)
    {
        try
        {
            Int64 TicketID = Convert.ToInt64(hdnPopupTicketID.Value);
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            ServiceTicket objServiceTicket = objSerTicRep.GetByTicketID(TicketID);

            if (objServiceTicket != null)
                objSerTicRep.UpdateNoteReadFlag(TicketID, IncentexGlobal.CurrentMember.UserInfoID, new ServiceTicketRepository().GetUnreadNotes(objServiceTicket.ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID).ToList());

            BindRepeater();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void repServiceTickets_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HiddenField hdnIamExpanded = (HiddenField)e.Item.FindControl("hdnIamExpanded");
            HiddenField hdnRepStatusID = (HiddenField)e.Item.FindControl("hdnRepStatusID");

            if (Convert.ToString(Session["ExpandedStatuses"]).Split(',').Contains(hdnRepStatusID.Value))
                hdnIamExpanded.Value = "true";

            GridView gvServiceTickets = (GridView)e.Item.FindControl("gvServiceTickets");
            BindGrid(gvServiceTickets, false);
        }
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

            if (this.QuickView)
            {
                dt.DefaultView.RowFilter = " sLookupName = 'Open' ";
                dt = dt.DefaultView.ToTable();
                dt.Rows[0]["sLookupName"] = "All";
            }

            dt.DefaultView.Sort = "sLookupName";
            dt = dt.DefaultView.ToTable();

            DataRow dr = dt.NewRow();
            dr["iLookupID"] = "-2";
            dr["sLookupName"] = "Recently Updated";
            dt.Rows.InsertAt(dr, 0);

            dr = dt.NewRow();
            dr["iLookupID"] = "-1";
            dr["sLookupName"] = "Flagged";
            dt.Rows.InsertAt(dr, 1);

            if (!this.QuickView)
            {
                dr = dt.NewRow();
                dr["iLookupID"] = "0";
                dr["sLookupName"] = "All";
                dt.Rows.InsertAt(dr, 2);
            }

            this.pds = new Dictionary<Int64, PagedDataSource>();
            this.FrmPg = new Dictionary<Int64, Int32>();
            this.ToPg = new Dictionary<Int64, Int32>();
            this.CurrentPage = new Dictionary<Int64, Int32>();

            lstTickets = new ServiceTicketRepository().SelectServiceTicketHistoryByEmployee(IncentexGlobal.CurrentMember.UserInfoID, this.QuickView);

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
            dvCollapsible.Attributes.Add("total", Convert.ToString(lstTickets.Where(le => le.UnReadNotesExists == 1).ToList().Count));
            dataTable = ListToDataTable(lstTickets.Where(le => le.UnReadNotesExists == 1).ToList());
        }
        //Binding flagged tickets
        else if (StatusID == -1)
        {
            dvCollapsible.Attributes.Add("total", Convert.ToString(lstTickets.Where(le => le.IsFlagged == 1).ToList().Count));
            dataTable = ListToDataTable(lstTickets.Where(le => le.IsFlagged == 1).ToList());
        }
        //Binding all tickets
        else if (StatusID == 0)
        {
            dvCollapsible.Attributes.Add("total", Convert.ToString(lstTickets.Count));
            dataTable = ListToDataTable(lstTickets);
        }
        //Binding tickets status wise
        else
        {
            dvCollapsible.Attributes.Add("total", Convert.ToString(lstTickets.Where(le => le.TicketStatusID == StatusID).ToList().Count));
            dataTable = ListToDataTable(lstTickets.Where(le => le.TicketStatusID == StatusID).ToList());
        }

        DataView myDataView = new DataView();
        myDataView = dataTable.DefaultView;

        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = Convert.ToString(this.ViewState["SortExp"]) + " " + Convert.ToString(this.ViewState["SortOrder"]);
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

        pagingtable.Visible = gvServiceTickets.Rows.Count > 0 ? true : false;
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
                            ExpandedStatuses = hdnRepStatusID.Value;
                        else
                            ExpandedStatuses += "," + hdnRepStatusID.Value;
                    }
                }
            }

            Session["ExpandedStatuses"] = ExpandedStatuses;
        }
    }

    //Allow NULLABLE 
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        DataTable dt = new DataTable();

        foreach (PropertyInfo info in typeof(T).GetProperties())
            dt.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);

        foreach (T t in list)
        {
            DataRow row = dt.NewRow();

            foreach (PropertyInfo info in typeof(T).GetProperties())
                row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;

            dt.Rows.Add(row);
        }

        return dt;
    }

    [WebMethod]
    public static String GetExistingNotes(String TicketID, String UserID)
    {
        String ExistingNotes = String.Empty;
        try
        {
            ServiceTicketRepository objRepo = new ServiceTicketRepository();
            ExistingNotes = objRepo.TrailingNotes(Convert.ToInt64(TicketID), 1, false, "\n");
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

    protected void dtlPaging_ItemCommand(Object source, DataListCommandEventArgs e)
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

    protected void dtlPaging_ItemDataBound(Object sender, DataListItemEventArgs e)
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

            tToPg = Convert.ToInt16(this.pds[StatusID].PageCount);

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

    protected void lnkbtnPrevious_Click(Object sender, EventArgs e)
    {
        LinkButton lnkbtnPrevious = (LinkButton)sender;
        HiddenField hdnRepStatusID = (HiddenField)lnkbtnPrevious.Parent.FindControl("hdnRepStatusID");
        Int64 StatusID = Convert.ToInt64(hdnRepStatusID.Value);
        CurrentPage[StatusID] -= 1;

        GridView gvServiceTickets = (GridView)lnkbtnPrevious.Parent.FindControl("gvServiceTickets");
        BindGrid(gvServiceTickets, true);
    }

    protected void lnkbtnNext_Click(Object sender, EventArgs e)
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