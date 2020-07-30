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
using Incentex.DA;
using Incentex.BE;
using System.Collections.Generic;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Company_Employee_UserReports : PageBase
{
    #region Data Member
    CompanyEmployeeRepository objCompanyEmployeeRepository =  new CompanyEmployeeRepository();
    UserInformationRepository objUserInformationRepository = new UserInformationRepository();
    LookupRepository objLookupRepos = new LookupRepository();
    ReportAccessRightsRepository objReportAccessRightsRepository = new ReportAccessRightsRepository();
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
    Int64 EmployeeCountryID
    {
        get
        {
            if (ViewState["EmployeeCountryID"] == null)
            {
                ViewState["EmployeeCountryID"] = 0;
            }
            return Convert.ToInt64(ViewState["EmployeeCountryID"]);
        }
        set
        {
            ViewState["EmployeeCountryID"] = value;
        }
    }
    #endregion

    #region Event Handlers
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

            if (Request.QueryString.Count > 0)
            {
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.EmployeeId = Convert.ToInt64(Request.QueryString.Get("SubId"));

                if (this.EmployeeId == 0)
                {
                    Response.Redirect("~/admin/Company/Employee/BasicInformation.aspx?Id=" + this.CompanyId);
                }
                ((Label)Master.FindControl("lblPageHeading")).Text = "User Reports";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Company/Employee/ViewEmployee.aspx?id=" + this.CompanyId;
                menucontrol.PopulateMenu(2, 4, this.CompanyId, this.EmployeeId, true);

                //set Search Page Return URL
                if (IncentexGlobal.SearchPageReturnURL != null)
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.SearchPageReturnURL;
                }
                DisplayData();
            }
            else
            {
                Response.Redirect("~/admin/Company/Employee/ViewEmployee.aspx");
            }
            
        }
    }
    protected void ddlParentReports_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        CompanyStoreRepository objCompanyStoreRepository = new CompanyStoreRepository();
        gvSubReport.DataSource = objLookupRepos.GetByLookup(ddlParentReports.SelectedItem.Text);
        gvSubReport.DataBind();
        if (gvSubReport.Rows.Count > 0)
        {
            //Price Level Bind
            dtPriceLevel.DataSource = objLookupRepos.GetByLookup("PriceLevel");
            dtPriceLevel.DataBind();

            //workgroup bind
            gvWorkGroup.DataSource = objCompanyStoreRepository.GetStoreWorkGroups(objCompanyStoreRepository.GetStoreIDByCompanyId(this.CompanyId)).Where(le => le.Existing == 1).OrderBy(le => le.WorkGroup).ToList();
            gvWorkGroup.DataBind();

            //Base station Bind
            BaseStationDA sBaseStation = new BaseStationDA();
            BasedStationBE sBSBe = new BasedStationBE();
            sBSBe.SOperation = "getBaseStationbyCounty";
            sBSBe.iCountryID = this.EmployeeCountryID;
            DataSet dsBaseStation = sBaseStation.GetBaseStaionByCountry(sBSBe);
            if (dsBaseStation.Tables.Count > 0 && dsBaseStation.Tables[0].Rows.Count > 0)
            {
                gvBaseStation.DataSource = dsBaseStation.Tables[0];
                gvBaseStation.DataBind();
            }

            BindExistingData();

            dvSubReport.Visible = true;
            dvPriceLevel.Visible = true;
            dvWorkgroup.Visible = true;
            dvBaseStation.Visible = true;
        }
        else
        {
            dtPriceLevel.DataSource = null;
            dtPriceLevel.DataBind();
            gvWorkGroup.DataSource = null;
            gvWorkGroup.DataBind();
            gvBaseStation.DataSource = null;
            gvBaseStation.DataBind();

            dvSubReport.Visible = false;
            dvPriceLevel.Visible = false;
            dvWorkgroup.Visible = false;
            dvBaseStation.Visible = false;
        }
    }
    protected void lnkbtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            List<string> SubReportIDList = new List<string>();
            bool IsAnyChacked = false;
            foreach (GridViewRow gr in gvSubReport.Rows)
            {
                CheckBox chk = gr.FindControl("chkSubReport") as CheckBox;
                HiddenField lblId = gr.FindControl("hdnSubReportID") as HiddenField;

                if (chk.Checked)
                {
                    SubReportIDList.Add(lblId.Value);
                    IsAnyChacked = true;
                }
            }

            List<string> PriceIDList = new List<string>();
            foreach (DataListItem item in dtPriceLevel.Items)
            {
                CheckBox chk = item.FindControl("chkPriceLevel") as CheckBox;
                HiddenField lblId = item.FindControl("hdnSPriceLevel") as HiddenField;

                if (chk.Checked)
                {
                    PriceIDList.Add(lblId.Value);
                }
            }

            List<string> WorkgroupIDList = new List<string>();
            foreach (GridViewRow gr in gvWorkGroup.Rows)
            {
                CheckBox chk = gr.FindControl("chkWorkgroup") as CheckBox;
                HiddenField lblId = gr.FindControl("hdnWorkgroupID") as HiddenField;

                if (chk.Checked)
                {
                    WorkgroupIDList.Add(lblId.Value);
                }
            }

            List<string> BaseStationIDList = new List<string>();
            foreach (GridViewRow gr in gvBaseStation.Rows)
            {
                CheckBox chk = gr.FindControl("ChkBaseStation") as CheckBox;
                HiddenField lblId = gr.FindControl("hdnBaseStationID") as HiddenField;

                if (chk.Checked)
                {
                    BaseStationIDList.Add(lblId.Value);
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
                objReportAccessRight.WorkgroupID = WorkgroupIDList != null && WorkgroupIDList.Count > 0 ? string.Join(",", WorkgroupIDList.ToArray()) : null;
                objReportAccessRight.BaseStationID = BaseStationIDList != null && BaseStationIDList.Count > 0 ? string.Join(",", BaseStationIDList.ToArray()) : null;
                objReportAccessRight.PriceLevel = PriceIDList != null && PriceIDList.Count > 0 ? string.Join(",", PriceIDList.ToArray()) : null;
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
        ddlParentReports.DataSource = objLookupRepos.GetByLookup(strStatus);
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
            dvRights.Visible = false;
            CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetById(this.EmployeeId);
            if (objCompanyEmployee.isCompanyAdmin)
            {
                dvCompanyEmployee.Visible = false;
                UserInformation objUserInformation = objUserInformationRepository.GetById(objCompanyEmployee.UserInfoID);
                lblUserFullName.Text = objUserInformation.FirstName + " " + objUserInformation.LastName;
                this.UserInfoID = objUserInformation.UserInfoID;
                this.EmployeeCountryID = Convert.ToInt64(objUserInformation.CountryId);
                FillParentReports();
            }
            else
                dvCompanyAdmin.Visible = false;
        }
        else
        {
            dvRights.Visible = true;
            dvCompanyEmployee.Visible = false;
            dvCompanyAdmin.Visible = false;
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
                        HiddenField lblId = gv.FindControl("hdnSubReportID") as HiddenField;
                        HtmlGenericControl dvChk = gv.FindControl("dvChkSubReport") as HtmlGenericControl;

                        foreach (string i in Ids)
                        {
                            if (i.Equals(lblId.Value))
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

                if (objReportAccessRight.PriceLevel != null)
                {
                    string[] Ids = objReportAccessRight.PriceLevel.Split(',');
                    foreach (DataListItem dt in dtPriceLevel.Items)
                    {
                        CheckBox chk = dt.FindControl("chkPriceLevel") as CheckBox;
                        HiddenField lblId = dt.FindControl("hdnSPriceLevel") as HiddenField;
                        HtmlGenericControl dvChk = dt.FindControl("spnPriceLevel") as HtmlGenericControl;

                        foreach (string i in Ids)
                        {
                            if (i.Equals(lblId.Value))
                            {
                                chk.Checked = true;
                                dvChk.Attributes.Add("class", "custom-checkbox_checked alignleft");
                                break;
                            }
                            else
                            {
                                chk.Checked = false;
                                dvChk.Attributes.Add("class", "custom-checkbox alignleft");
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataListItem dt in dtPriceLevel.Items)
                    {
                        HtmlGenericControl dvChk = dt.FindControl("spnPriceLevel") as HtmlGenericControl;
                        dvChk.Attributes.Add("class", "custom-checkbox alignleft");
                    }
                }

                if (objReportAccessRight.WorkgroupID != null)
                {
                    string[] Ids = objReportAccessRight.WorkgroupID.Split(',');
                    foreach (GridViewRow gv in gvWorkGroup.Rows)
                    {
                        CheckBox chk = gv.FindControl("chkWorkgroup") as CheckBox;
                        HiddenField lblId = gv.FindControl("hdnWorkgroupID") as HiddenField;
                        HtmlGenericControl dvChk = gv.FindControl("dvChkWorkgroup") as HtmlGenericControl;

                        foreach (string i in Ids)
                        {
                            if (i.Equals(lblId.Value))
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
                        HiddenField lblId = gv.FindControl("hdnBaseStationID") as HiddenField;
                        HtmlGenericControl dvChk = gv.FindControl("dvChkBaseStation") as HtmlGenericControl;

                        foreach (string i in Ids)
                        {
                            if (i.Equals(lblId.Value))
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
            else
            {

            }
    }
    #endregion
}
