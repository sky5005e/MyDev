/// <summary>
/// Created by mayur on 28-dec-2011
/// For adding userwise scheduled event and this event is display on menu.aspx page when that IE is login in the system.
/// </summary>

using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Company_Employee_ScheduledEvent : PageBase
{
    #region local property
    CompanyEmployeeEventsRepository objCompanyEmployeeEventsRep = new CompanyEmployeeEventsRepository();
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    Common objcommm = new Common();
    Int64 CompanyId
    {
        get
        {
            if (ViewState["CompanyId"] == null)
            {
                ViewState["CompanyId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyId"]);
        }
        set
        {
            ViewState["CompanyId"] = value;
        }
    }

    Int64 EmployeeId
    {
        get
        {
            if (ViewState["EmployeeId"] == null)
            {
                ViewState["EmployeeId"] = 0;
            }
            return Convert.ToInt64(ViewState["EmployeeId"]);
        }
        set
        {
            ViewState["EmployeeId"] = value;
        }
    }

    Int64 EventID
    {
        get
        {
            if (ViewState["EventID"] == null)
            {
                ViewState["EventID"] = 0;
            }
            return Convert.ToInt64(ViewState["EventID"]);
        }
        set
        {
            ViewState["EventID"] = value;
        }
    }

    String AdditionalIEID
    {
        get
        {
            if (ViewState["AdditionalIEID"] == null)
            {
                ViewState["AdditionalIEID"] = "";
            }
            return ViewState["AdditionalIEID"].ToString();
        }
        set
        {
            ViewState["AdditionalIEID"] = value;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Employees";
            base.ParentMenuID = 11;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            this.CompanyId = Convert.ToInt64(Request.QueryString.Get("ID"));
            this.EmployeeId = Convert.ToInt64(Request.QueryString.Get("SubID"));
            ((Label)Master.FindControl("lblPageHeading")).Text = "Scheduled Event";

           ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Company/Employee/BasicInformation.aspx?Id=" + this.CompanyId + "&SubId=" + this.EmployeeId;

            //for add and edit scheduled event
            if (Request.QueryString["EventID"] != null)
            {
                this.EventID = Convert.ToInt64(Request.QueryString.Get("EventID"));
                FillDetail();
            }
            BindAdditionalIEGrid();
        }
    }

    protected void BindAdditionalIEGrid()
    {
        gvIncentexEmployee.DataSource = new IncentexEmployeeRepository().GetAllEmployee();
        gvIncentexEmployee.DataBind();
    }
    /// <summary>
    /// Fills the event detail when comming for edit event.
    /// </summary>
    protected void FillDetail()
    {
        CompanyEmployeeEvent objCompanyEmployeeEvent = new CompanyEmployeeEvent();
        objCompanyEmployeeEvent = objCompanyEmployeeEventsRep.GetById(this.EventID);

        if (objCompanyEmployeeEvent != null)
        {
            txtEventName.Text = objCompanyEmployeeEvent.EventName;
            txtEventDate.Text = DateTime.Parse(objCompanyEmployeeEvent.EventDate.ToString()).ToString("MM/dd/yyyy");
            txtEventTime.Text = DateTime.Parse(objCompanyEmployeeEvent.EventTime.ToString()).ToString("hh:mm");
            ddlEventReminder.SelectedValue = objCompanyEmployeeEvent.EventReminder.ToString();
            this.AdditionalIEID = objCompanyEmployeeEvent.AdditionalIEID;
        }

    }

    /// <summary>
    /// Used to Clears form control.
    /// </summary>
    protected void ClearControl()
    {
        txtEventName.Text = string.Empty;
        txtEventDate.Text = string.Empty;
        txtEventTime.Text = string.Empty;
        ddlEventReminder.SelectedValue = "0";
        this.AdditionalIEID = "";
        this.EventID = 0;
        foreach (GridViewRow gr in gvIncentexEmployee.Rows)
        {
            CheckBox chk = gr.FindControl("chk") as CheckBox;
            HtmlGenericControl dvChk = gr.FindControl("dvChk") as HtmlGenericControl;
            chk.Checked = true;
            dvChk.Attributes.Add("class", "custom-checkbox");
        }
    }

    protected void lnkBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            CompanyEmployeeEvent objCompanyEmployeeEvent = new CompanyEmployeeEvent();
            if (this.EventID != 0)
                objCompanyEmployeeEvent = objCompanyEmployeeEventsRep.GetById(this.EventID);

            objCompanyEmployeeEvent.CompanyEmployeeID = this.EmployeeId;
            objCompanyEmployeeEvent.UserInfoID = objCompanyEmployeeRepository.GetById(this.EmployeeId).UserInfoID;
            objCompanyEmployeeEvent.EventName = txtEventName.Text;
            objCompanyEmployeeEvent.EventDate = DateTime.Parse(txtEventDate.Text).Date;
            objCompanyEmployeeEvent.EventTime = TimeSpan.Parse(txtEventTime.Text);
            objCompanyEmployeeEvent.EventReminder = ddlEventReminder.SelectedValue;
            objCompanyEmployeeEvent.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objCompanyEmployeeEvent.UpdatedDate = DateTime.Now;
            objCompanyEmployeeEvent.AdditionalIEID = this.AdditionalIEID;
            lblMsg.Text = "Record updated successfully";
            if (this.EventID == 0)
            {
                lblMsg.Text = "Record Inserted successfully";
                objCompanyEmployeeEvent.IsActive = true;
                objCompanyEmployeeEvent.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objCompanyEmployeeEvent.CreatedDate = DateTime.Now;
                objCompanyEmployeeEventsRep.Insert(objCompanyEmployeeEvent);
            }
            objCompanyEmployeeEventsRep.SubmitChanges();
            ClearControl();
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcommm.Callvalidationmessage(Server.MapPath("../../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }

    }

    protected void lnkBtnlnkbtnAdditionalEmployee_Click(object sender, EventArgs e)
    {
        string[] Ids = this.AdditionalIEID.Split(',');
        foreach (GridViewRow gr in gvIncentexEmployee.Rows)
        {
            CheckBox chk = gr.FindControl("chk") as CheckBox;
            Label lblId = gr.FindControl("lblId") as Label;
            HtmlGenericControl dvChk = gr.FindControl("dvChk") as HtmlGenericControl;
            foreach (string i in Ids)
            {
                if (i.Equals(lblId.Text))
                {
                    chk.Checked = true;
                    dvChk.Attributes.Add("class", "custom-checkbox_checked");
                    break;
                }
            }
        }
        modal.Show();
    }

    protected void lnkbtnAddEmployee_Click(object sender, EventArgs e)
    {
        this.AdditionalIEID = "";
        foreach (GridViewRow gr in gvIncentexEmployee.Rows)
        {
            CheckBox chk = gr.FindControl("chk") as CheckBox;
            Label lblId = gr.FindControl("lblId") as Label;

            if (chk.Checked)
            {
                this.AdditionalIEID += lblId.Text + ",";
            }
        }
    }
}
