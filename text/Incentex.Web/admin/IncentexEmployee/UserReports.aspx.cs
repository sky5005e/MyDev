using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_IncentexEmployee_UserReports : PageBase
{
    #region Data Member
    IncentexEmployeeRepository objIncentexEmployeeRepository = new IncentexEmployeeRepository();
    LookupRepository objLookupRepos = new LookupRepository();
    ReportAccessRightsRepository objReportAccessRightsRepository = new ReportAccessRightsRepository();
    Int64 IncentexEmployeeID
    {
        get
        {
            if (ViewState["IncentexEmployeeID"] == null)
            {
                ViewState["IncentexEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["IncentexEmployeeID"]);
        }
        set
        {
            ViewState["IncentexEmployeeID"] = value;
        }
    }
    Int64 UserInfoID
    {
        get
        {
            if (ViewState["UserInfoID"] == null)
            {
                ViewState["UserInfoID"] = 0;
            }
            return Convert.ToInt64(ViewState["UserInfoID"]);
        }
        set
        {
            ViewState["UserInfoID"] = value;
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Incentex Employee";
            base.ParentMenuID = 10;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            if (Request.QueryString.Count > 0)
            {
                this.IncentexEmployeeID = Convert.ToInt64(Request.QueryString.Get("Id"));
                if (this.IncentexEmployeeID == 0)
                {
                    Response.Redirect("~/admin/IncentexEmployee/BasicInformation.aspx?Id=" + this.IncentexEmployeeID);
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Report Access Rights";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/IncentexEmployee/ViewIncentexEmployee.aspx";

                menucontrol.PopulateMenu(8, 0, this.IncentexEmployeeID, 0, false);
                DisplayData();
            }
            else
            {
                Response.Redirect("ViewIncentexEmployee.aspx");
            }
        }
    }
    protected void ddlParentReports_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        gvSubReport.DataSource = objLookupRepos.GetByLookup(ddlParentReports.SelectedItem.Text).OrderBy(x=>x.sLookupName);
        gvSubReport.DataBind();
        if (gvSubReport.Rows.Count > 0)
        {
            //store bind
            gvStore.DataSource = new OrderConfirmationRepository().GetCompanyStoreName().OrderBy(x => x.CompanyName);
            gvStore.DataBind();

            //workgroup bind
            gvWorkGroup.DataSource = objLookupRepos.GetByLookup("Workgroup").OrderBy(x => x.sLookupName);
            gvWorkGroup.DataBind();

            //Base station Bind
            gvBaseStation.DataSource = new BaseStationRepository().GetAllBaseStation().OrderBy(x => x.sBaseStation).ToList();
            gvBaseStation.DataBind();

            BindExistingData();

            dvSubReport.Visible = true;
            dvStore.Visible = true;
            dvWorkgroup.Visible = true;
            dvBaseStation.Visible = true;
        }
        else
        {
            gvStore.DataSource = null;
            gvStore.DataBind();
            gvWorkGroup.DataSource = null;
            gvWorkGroup.DataBind();
            gvBaseStation.DataSource = null;
            gvBaseStation.DataBind();

            dvSubReport.Visible = false;
            dvStore.Visible = false;
            dvWorkgroup.Visible = false;
            dvBaseStation.Visible = false;
        }
    }
    protected void lnkbtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            List<string> SubReportIDList = new List<string>();
            bool IsAnyChacked = false;
            foreach (GridViewRow gr in gvSubReport.Rows)
            {
                CheckBox chk = gr.FindControl("chkSubReport") as CheckBox;
                HiddenField hdnId = gr.FindControl("hdnSubReportID") as HiddenField;

                if (chk.Checked)
                {
                    SubReportIDList.Add(hdnId.Value);
                    IsAnyChacked = true;
                }
            }

            List<string> StoreIDList = new List<string>();
            foreach (GridViewRow gr in gvStore.Rows)
            {
                CheckBox chk = gr.FindControl("chkStore") as CheckBox;
                HiddenField hdnId = gr.FindControl("hdnStoreID") as HiddenField;

                if (chk.Checked)
                {
                    StoreIDList.Add(hdnId.Value);
                }
            }

            List<string> WorkgroupIDList = new List<string>();
            foreach (GridViewRow gr in gvWorkGroup.Rows)
            {
                CheckBox chk = gr.FindControl("chkWorkgroup") as CheckBox;
                HiddenField hdnId = gr.FindControl("hdnWorkgroupID") as HiddenField;

                if (chk.Checked)
                {
                    WorkgroupIDList.Add(hdnId.Value);
                }
            }

            List<string> BaseStationIDList = new List<string>();
            foreach (GridViewRow gr in gvBaseStation.Rows)
            {
                CheckBox chk = gr.FindControl("ChkBaseStation") as CheckBox;
                HiddenField hdnId = gr.FindControl("hdnBaseStationID") as HiddenField;

                if (chk.Checked)
                {
                    BaseStationIDList.Add(hdnId.Value);
                }
            }

            //first delete previous entry for this parent report
            ReportAccessRight objReportAccessRightForDelete = objReportAccessRightsRepository.GetByUserInfoIDAndParentReportID(this.UserInfoID, Convert.ToInt64(ddlParentReports.SelectedValue));
            if (objReportAccessRightForDelete != null)
            {
                objReportAccessRightsRepository.Delete(objReportAccessRightForDelete);
                objReportAccessRightsRepository.SubmitChanges();
            }

            if (IsAnyChacked)
            {
                //Add new record for this parent report
                ReportAccessRight objReportAccessRight = new ReportAccessRight();
                objReportAccessRight.UserInfoID = this.UserInfoID;
                objReportAccessRight.ParentReportID = Convert.ToInt32(ddlParentReports.SelectedValue);
                objReportAccessRight.ChildReportID = SubReportIDList != null && SubReportIDList.Count > 0 ? string.Join(",", SubReportIDList.ToArray()) : null;
                objReportAccessRight.StoreID = StoreIDList != null && StoreIDList.Count > 0 ? string.Join(",", StoreIDList.ToArray()) : null;
                objReportAccessRight.WorkgroupID = WorkgroupIDList != null && WorkgroupIDList.Count > 0 ? string.Join(",", WorkgroupIDList.ToArray()) : null;
                objReportAccessRight.BaseStationID = BaseStationIDList != null && BaseStationIDList.Count > 0 ? string.Join(",", BaseStationIDList.ToArray()) : null;
                objReportAccessRightsRepository.Insert(objReportAccessRight);
                objReportAccessRightsRepository.SubmitChanges();
                BindExistingData();
            }
            else
            {
                ddlParentReports_SelectedIndexChanged(null, null);
            }
            lblMsg.Text = "Record Saved Successfully.";
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in saving record ...";
            ErrHandler.WriteError(ex);
        }
    }
    #endregion

    #region Methods
    public void FillParentReports()
    {
        string strStatus = "ReportDashboard";
        ddlParentReports.DataSource = objLookupRepos.GetByLookup(strStatus).OrderBy(x=>x.sLookupName);
        ddlParentReports.DataValueField = "iLookupID";
        ddlParentReports.DataTextField = "sLookupName";
        ddlParentReports.DataBind();
        ddlParentReports.Items.Insert(0, new ListItem("-select-", "0"));
    }
    protected void DisplayData()
    {
        //As per ken post he want that only Super Admin can give rights to user for report on 19-Sept-2012
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
        {
            dvNoRights.Visible = false;
            IncentexEmployee objIncentexEmployee = objIncentexEmployeeRepository.GetById(this.IncentexEmployeeID);
            this.UserInfoID = objIncentexEmployee.UserInfoID;
            FillParentReports();
        }
        else
        {
            dvNoRights.Visible = true;
            dvRights.Visible = false;
        }

    }
    protected void BindExistingData()
    {
        //For Display data
        ReportAccessRight objReportAccessRight = objReportAccessRightsRepository.GetByUserInfoIDAndParentReportID(this.UserInfoID, Convert.ToInt64(ddlParentReports.SelectedValue));

        if (objReportAccessRight != null)
        {
            if (objReportAccessRight.ChildReportID != null)
            {
                string[] Ids = objReportAccessRight.ChildReportID.Split(',');
                foreach (GridViewRow gv in gvSubReport.Rows)
                {
                    CheckBox chk = gv.FindControl("chkSubReport") as CheckBox;
                    HiddenField hdnId = gv.FindControl("hdnSubReportID") as HiddenField;
                    HtmlGenericControl dvChk = gv.FindControl("dvChkSubReport") as HtmlGenericControl;

                    foreach (string i in Ids)
                    {
                        if (i.Equals(hdnId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "wheather_checked");
                            break;
                        }
                        else
                        {
                            chk.Checked = false;
                            dvChk.Attributes.Add("class", "wheather_check");
                        }
                    }
                }
            }

            if (objReportAccessRight.StoreID != null)
            {
                string[] Ids = objReportAccessRight.StoreID.Split(',');
                foreach (GridViewRow gv in gvStore.Rows)
                {
                    CheckBox chk = gv.FindControl("chkStore") as CheckBox;
                    HiddenField hdnId = gv.FindControl("hdnStoreID") as HiddenField;
                    HtmlGenericControl dvChk = gv.FindControl("dvChkStore") as HtmlGenericControl;

                    foreach (string i in Ids)
                    {
                        if (i.Equals(hdnId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "wheather_checked");
                            break;
                        }
                        else
                        {
                            chk.Checked = false;
                            dvChk.Attributes.Add("class", "wheather_check");
                        }
                    }
                }
            }

            if (objReportAccessRight.WorkgroupID != null)
            {
                string[] Ids = objReportAccessRight.WorkgroupID.Split(',');
                foreach (GridViewRow gv in gvWorkGroup.Rows)
                {
                    CheckBox chk = gv.FindControl("chkWorkgroup") as CheckBox;
                    HiddenField hdnId = gv.FindControl("hdnWorkgroupID") as HiddenField;
                    HtmlGenericControl dvChk = gv.FindControl("dvChkWorkgroup") as HtmlGenericControl;

                    foreach (string i in Ids)
                    {
                        if (i.Equals(hdnId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "wheather_checked");
                            break;
                        }
                        else
                        {
                            chk.Checked = false;
                            dvChk.Attributes.Add("class", "wheather_check");
                        }
                    }
                }
            }

            if (objReportAccessRight.BaseStationID != null)
            {
                string[] Ids = objReportAccessRight.BaseStationID.Split(',');
                foreach (GridViewRow gv in gvBaseStation.Rows)
                {
                    CheckBox chk = gv.FindControl("ChkBaseStation") as CheckBox;
                    HiddenField hdnId = gv.FindControl("hdnBaseStationID") as HiddenField;
                    HtmlGenericControl dvChk = gv.FindControl("dvChkBaseStation") as HtmlGenericControl;

                    foreach (string i in Ids)
                    {
                        if (i.Equals(hdnId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "wheather_checked");
                            break;
                        }
                        else
                        {
                            chk.Checked = false;
                            dvChk.Attributes.Add("class", "wheather_check");
                        }
                    }
                }
            }
        }
    }
    #endregion
}
