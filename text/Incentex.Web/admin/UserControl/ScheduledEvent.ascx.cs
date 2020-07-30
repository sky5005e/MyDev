/// <summary>
/// Created by mayur on 2-Jan-2012
/// This popup will display when IE login in the system and any event of today is available.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_UserControl_ScheduledEvent : System.Web.UI.UserControl
{
    #region property
    List<Int64> RemindLatterEventID
    {
        get
        {
            if (ViewState["RemindLatterEventID"] == null)
            {
                ViewState["RemindLatterEventID"] = new List<Int64>();
            }
            return (List<Int64>)ViewState["RemindLatterEventID"];
        }
        set
        {
            ViewState["RemindLatterEventID"] = value;
        }
    }
    CompanyEmployeeEventsRepository objCompanyEmployeeEventsRepository = new CompanyEmployeeEventsRepository();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.UrlReferrer != null && Request.UrlReferrer.AbsoluteUri.ToLower().Contains("login.aspx") && IncentexGlobal.CurrentMember != null &&  (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin)))
            {
                BindDataList();
            }
        }
    }

    #region Datalist Events
    protected void dtScheduledEvent_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            TextBox txtDate = (TextBox)e.Item.FindControl("txtEventDate");
            TextBox txtTime = (TextBox)e.Item.FindControl("txtEventTime");
            txtDate.Text = DateTime.Parse(txtDate.Text).ToString("MM/dd/yyyy");
            txtTime.Text = DateTime.Parse(txtTime.Text).ToString("hh:mm");
        }
    }
    protected void dtScheduledEvent_ItemCommand(object source, DataListCommandEventArgs e)
    {
        lblErrorMsg.Text = string.Empty;
        if (e.CommandName == "Completed")
        {
            DataListItem item = (DataListItem)((LinkButton)e.CommandSource).Parent;
            ((HtmlGenericControl)dtScheduledEvent.Items[item.ItemIndex].FindControl("dvRescheduled")).Visible = false;
            ((HtmlGenericControl)dtScheduledEvent.Items[item.ItemIndex].FindControl("dvCompletedWithNote")).Visible = true;
        }
        if (e.CommandName == "CompletedWithNote")
        {
            DataListItem item = (DataListItem)((Button)e.CommandSource).Parent.Parent;

            //Change event status as inactive
            CompanyEmployeeEvent objEvent = objCompanyEmployeeEventsRepository.GetById(Convert.ToInt64(e.CommandArgument));
            objEvent.IsActive = !objEvent.IsActive;
            objCompanyEmployeeEventsRepository.SubmitChanges();

            //add entry to user not history
            string strNote = ((TextBox)dtScheduledEvent.Items[item.ItemIndex].FindControl("txtNote")).Text + Environment.NewLine;
            strNote += "Comment: Completed" + Environment.NewLine;
            strNote += "Event Name: " + objEvent.EventName + Environment.NewLine;
            strNote += "Event Date: " + Convert.ToDateTime(objEvent.EventDate).ToString("dd-MMM-yyyy") + Environment.NewLine;
            strNote += "Event Time: " + DateTime.Parse(objEvent.EventTime.ToString()).ToString("hh:mm tt") + Environment.NewLine;

            NoteDetail obj = new NoteDetail()
            {
                ForeignKey = objEvent.CompanyEmployeeID,
                Notecontents = strNote,
                NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.CompanyEmployee),
                CreatedBy = IncentexGlobal.CurrentMember.UserInfoID,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID
            };
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            objRepo.Insert(obj);
            objRepo.SubmitChanges();

            //bind datalist again
            BindDataList();
        }
        if (e.CommandName == "RemindMeLatter")
        {
            this.RemindLatterEventID.Add(Convert.ToInt64(e.CommandArgument));
            BindDataList();
        }
        if (e.CommandName == "Rescheduled")
        {
            DataListItem item = (DataListItem)((LinkButton)e.CommandSource).Parent;
            ((HtmlGenericControl)dtScheduledEvent.Items[item.ItemIndex].FindControl("dvCompletedWithNote")).Visible = false;
            ((HtmlGenericControl)dtScheduledEvent.Items[item.ItemIndex].FindControl("dvRescheduled")).Visible = true;
        }
        if (e.CommandName == "SubmitChange")
        {
            DataListItem item = (DataListItem)((Button)e.CommandSource).Parent.Parent;
            try
            {
                CompanyEmployeeEvent objEvent = objCompanyEmployeeEventsRepository.GetById(Convert.ToInt64(e.CommandArgument));
                objEvent.EventDate = DateTime.Parse(((TextBox)dtScheduledEvent.Items[item.ItemIndex].FindControl("txtEventDate")).Text).Date;
                objEvent.EventTime = TimeSpan.Parse(((TextBox)dtScheduledEvent.Items[item.ItemIndex].FindControl("txtEventTime")).Text);
                objEvent.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objEvent.UpdatedDate = DateTime.Now;
                objCompanyEmployeeEventsRepository.SubmitChanges();
                BindDataList();
            }
            catch
            {
                lblErrorMsg.Text = "Please Enter valid date and time.<br/>Time must be in 24 hour formate.";
            }
            
        }

    }
    #endregion

    /// <summary>
    /// Binds the data list control with today scheduled event.
    /// </summary>
    protected void BindDataList()
    {
        List<CompanyEmployeeEvent> objEventList = objCompanyEmployeeEventsRepository.GetTodayActiveEvents(IncentexGlobal.CurrentMember.UserInfoID);

        //here check that any event is set to reminder letter or not
        if (this.RemindLatterEventID != null && this.RemindLatterEventID.Count > 0)
        {
            foreach (CompanyEmployeeEvent objevent in objEventList.ToList())
            {
                for (int i = 0; i < this.RemindLatterEventID.Count; i++)
                {
                    if (objevent.EventID == this.RemindLatterEventID[i])
                    {
                        objEventList.Remove(objevent);
                    }
                }
            }
        }

        dtScheduledEvent.DataSource = objEventList;
        dtScheduledEvent.DataBind();

        if (dtScheduledEvent.Items.Count > 0)
        {
            //display popup window
            mpeScheduledEvent.Show();
        }
    }
    protected void ShowFooter_OnPreRender(object sender, EventArgs e)
    {
        DataList objTempDL = (DataList)sender;
        if (objTempDL.Items.Count == 0)
        {
            objTempDL.ShowFooter = true;
        }
    }
}
