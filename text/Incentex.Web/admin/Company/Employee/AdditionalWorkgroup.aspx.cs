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
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using System;

public partial class admin_Company_Employee_AdditionalWorkgroup : PageBase
{
    Int64 AddID
    {
        get
        {
            if (ViewState["AddID"] == null)
            {
                ViewState["AddID"] = 0;
            }
            return Convert.ToInt64(ViewState["AddID"]);
        }
        set
        {
            ViewState["AddID"] = value;
        }
    }
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
    Common objcomm = new Common();
    LookupRepository objLookupRepos = new LookupRepository();
    INC_Lookup objLookup = new INC_Lookup();
    AdditionalWorkgroupRepository objRepo = new AdditionalWorkgroupRepository();
    
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
                menucontrol.PopulateMenu(2, 6, this.CompanyId, this.EmployeeId, true);
                ((Label)Master.FindControl("lblPageHeading")).Text = "Additional Workgroup";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Company/Employee/ViewEmployee.aspx?id=" + this.CompanyId;
                BindWorkgroupgrid();
                DisplayWorkgroupData(Convert.ToInt32(EmployeeId), Convert.ToInt32(CompanyId));
               
            }
            else
            {

                Response.Redirect("~/admin/Company/Employee/ViewEmployee.aspx");

            }
        }
      
      
    }
    public void BindWorkgroupgrid()
    {
        try
        {
            CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetById(this.EmployeeId);
            string strStatus = "Workgroup";
            dtlWorkgroup.DataSource = objLookupRepos.GetByLookupWorkgroupName(strStatus, Convert.ToInt32(objCompanyEmployee.WorkgroupID));
            dtlWorkgroup.DataBind();

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    void DisplayWorkgroupData(int UserID,int CompanyID)
    {
        CheckBox chk;
        HiddenField lblId;
        if (this.EmployeeId != 0)
        {
            CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetById(this.EmployeeId);
            if (objCompanyEmployee == null)
            {
                return;
            }
            UserInformation objUserInfo = new UserInformationRepository().GetById(objCompanyEmployee.UserInfoID);
            lblUserFullName.Text = objUserInfo.FirstName + " " + objUserInfo.LastName;
            //Get menu from empmenuaccess table
          
            List<AdditionalWorkgroup> objDashmanagelist = new List<AdditionalWorkgroup>();
            objDashmanagelist = objRepo.GetManageDetailName(Convert.ToInt32(EmployeeId), Convert.ToInt32(CompanyId));
            if (objDashmanagelist.Count == 0)
            {
                return;
            }

            foreach (DataListItem dtM in dtlWorkgroup.Items)
            {
                chk = dtM.FindControl("chkdtlMenus") as CheckBox;
                lblId = dtM.FindControl("hdnWorkgroupID") as HiddenField;
                HtmlGenericControl dvChk = dtM.FindControl("menuspan") as HtmlGenericControl;

                foreach (AdditionalWorkgroup objMenu in objDashmanagelist)
                {

                    if (objMenu.WorkgroupID.ToString().Equals(lblId.Value))
                    {
                        chk.Checked = true;
                        dvChk.Attributes.Add("class", "custom-checkbox_checked");
                        break;
                    }

                }

            }
        }

    }
    public void SaveWorkgroup()
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetById(this.EmployeeId);
            int ? LookUpIdList = null;
            bool IsAnyChacked = false;  
            List<AdditionalWorkgroup> objDashmanagelist = new List<AdditionalWorkgroup>();
            objDashmanagelist = objRepo.GetManageDetailName(Convert.ToInt32(EmployeeId), Convert.ToInt32(CompanyId));
            if (objDashmanagelist.Count != 0)
            {
                foreach (AdditionalWorkgroup l in objDashmanagelist)
                {
                    objRepo.Delete(l);
                    objRepo.SubmitChanges();
                }
            }
           
                foreach (DataListItem gr in dtlWorkgroup.Items)
                {
                    CheckBox chk = gr.FindControl("chkdtlMenus") as CheckBox;
                    HiddenField lblId = gr.FindControl("hdnWorkgroupID") as HiddenField;
                    AdditionalWorkgroup objWork = new AdditionalWorkgroup();
                    objRepo = new AdditionalWorkgroupRepository();
                    if (chk.Checked==true)
                    {
                        LookUpIdList = Convert.ToInt32(lblId.Value);
                        objWork.UserInfoID = Convert.ToInt32(objCompanyEmployee.UserInfoID);
                        objWork.CompanyEmployeeID = Convert.ToInt32(this.EmployeeId);
                        objWork.CompanyID = Convert.ToInt32(this.CompanyId);
                        objWork.WorkgroupID = LookUpIdList;
                        objRepo.Insert(objWork);
                        objRepo.SubmitChanges();
                        IsAnyChacked = true;
                    } 
                }
                if (!IsAnyChacked)
                {

                    BindWorkgroupgrid();
                    lblMsg.Text = "Please select any one workgroup name ... ";
                    return;
                }
                else
                {
                     lblMsg.Text = "Selected Records Save Successfully ...";
                }

        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    } 
    protected void lnkBtnSaveInfo_Click1(object sender, EventArgs e)
    {
        SaveWorkgroup();
        DisplayWorkgroupData(Convert.ToInt32(EmployeeId), Convert.ToInt32(CompanyId));
        Response.Redirect("ViewEmployee.aspx?ID=" + this.CompanyId);
    }
}
