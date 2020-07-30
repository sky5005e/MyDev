/// <summary>
/// Cleanup code and remove delete user functionality from this page by mayur on 13-jan-2012
/// Remove delete user function on 13-jan-2012 by mayur
/// </summary>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using commonlib.Common;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Company_Employee_ViewEmployee : PageBase
{
    EmailTemplateBE objEmailBE = new EmailTemplateBE();
    EmailTemplateDA objEmailDA = new EmailTemplateDA();
    DataSet dsEmailTemplate;
    PagedDataSource pds = new PagedDataSource();
    DataSet ds;
    CompanyEmployeeBE objCE = new CompanyEmployeeBE();
    CompanyEmployeeDA objCeDa = new CompanyEmployeeDA();
    UserInformationRepository objUserRepos = new UserInformationRepository();
    UserInformation objUInfo = new UserInformation();
    INC_Lookup objLook = new INC_Lookup();
    LookupRepository objLookupRepos = new LookupRepository();
    CompanyRepository objCompanyRepos = new CompanyRepository();
    List<Company> objComplist = new List<Company>();
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

    public int CurrentPage
    {
        get
        {
            if (this.ViewState["CurrentPage"] == null)
                return 0;
            else
                return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }
    public int PagerSize
    {
        get
        {
            if (this.ViewState["PagerSize"] == null)
                return Convert.ToInt32(Application["GRIDPAGERSIZE"]);
            else
                return Convert.ToInt16(this.ViewState["PagerSize"].ToString());
        }
        set
        {
            this.ViewState["PagerSize"] = value;
        }
    }
    public int FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt16(this.ViewState["FrmPg"].ToString());
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }
    public int ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return PagerSize;
            else
                return Convert.ToInt16(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.Count > 0)
            {
                CheckLogin();
                //set Search Page Return URL
                IncentexGlobal.SearchPageReturnURLFrontSide = null;
                
                 
             
                this.CompanyId = Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId);
                Company objCompany = new CompanyRepository().GetById(this.CompanyId);
                if (objCompany == null)
                {
                    Response.Redirect("~/MyAccount/UserManagement.aspx");
                }
                //((HtmlGenericControl)menucontrol.FindControl("dvSubMenu")).Visible = false;
                //menucontrol.PopulateMenu(2, 0, this.CompanyId, 0, false);
               // ((Label)Master.FindControl("lblPageHeading")).Text = "Employee Listing";
                ((Label)Master.FindControl("lblPageHeading")).Text = objCompany.CompanyName + " Employees";
                //((Label)Master.FindControl("lblPageHeading")).Text = "Company Record ";
              
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/UserManagement.aspx";

                bindgrid();
            }
            else
            {
                Response.Redirect("~/index.aspx");
            }

        }
    }

    private void bindgrid()
    {
        try
        {
            //Get WorkGroup by Logged in User Id
            CompanyEmployee objCmpnyInfo = new CompanyEmployee();
            CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
            objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

            DataView myDataView = new DataView();
            objCE.Operations = "selectcompanyemployeebyworkgroup";
            objCE.CompanyId = this.CompanyId;
            objCE.WorkgroupID = Convert.ToInt64(objCmpnyInfo.ManagementControlForWorkgroup);
            ds = objCeDa.CompanyEmployee(objCE);
            if (ds.Tables[0].Rows.Count == 0)
            {
                dvPaging.Visible = false;
            }
            else
            {
                lblWorkgroupName.Text = "Workgroup: " + ds.Tables[0].Rows[0]["Workgroup"].ToString();
                hdnWorkGroupName.Value = ds.Tables[0].Rows[0]["WorkgroupID"].ToString();
                lblRecords.Text = ds.Tables[0].Rows.Count.ToString();
                dvPaging.Visible = true;
            }
            myDataView = ds.Tables[0].DefaultView;
            if (this.ViewState["SortExp"] != null)
            {
                myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
            }
            pds.DataSource = myDataView;
            pds.AllowPaging = true;
            pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
            pds.CurrentPageIndex = CurrentPage;
            lnkbtnNext.Enabled = !pds.IsLastPage;
            lnkbtnPrevious.Enabled = !pds.IsFirstPage;
            gvEmployee.DataSource = pds;
            gvEmployee.DataBind();

            doPaging();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        
    }

    private void doPaging()
    {
        try
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            int CurrentPg = pds.CurrentPageIndex + 1;

            if (CurrentPg > ToPg)
            {
                FrmPg = ToPg + 1;
                ToPg = ToPg + PagerSize;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - PagerSize;

            }

            if (pds.PageCount < ToPg)
                ToPg = pds.PageCount;

            for (int i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            dtlViewEmployee.DataSource = dt;
            dtlViewEmployee.DataBind();

        }
        catch (Exception)
        { }
    }

    protected void gvEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployee.PageIndex = e.NewPageIndex;
        bindgrid();
    }

    private string getSortDirectionString(SortDirection sortDireciton)
    {
        string newSortDirection = String.Empty;
        if (sortDireciton == SortDirection.Ascending)
        {
            newSortDirection = "ASC";
        }
        else
        {
            newSortDirection = "DESC";
        }

        return newSortDirection;
    }

    protected void gvEmployee_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dtSortTable = gvEmployee.DataSource as DataTable;
        if (dtSortTable != null)
        {
            DataView dvSortedView = new DataView(dtSortTable);
            dvSortedView.Sort = e.SortExpression + " " + getSortDirectionString(e.SortDirection);
            gvEmployee.DataSource = dvSortedView;
            gvEmployee.DataBind();
        }
    }
    protected void lnkBtnAddEmployee_Click(object sender, EventArgs e)
    {
        Response.Redirect("BasicInformation.aspx?Id=" + this.CompanyId + "&SubId=0&wg=" + hdnWorkGroupName.Value);
    }

    protected void gvEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName.Equals("Sort"))
        {
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

            bindgrid();
        }
        if (e.CommandName == "vieweditces")
        {
            Response.Redirect("BasicInformation.aspx?Id=" + this.CompanyId + "&SubId=" + e.CommandArgument.ToString(),false);
        }
        if (e.CommandName == "ChangeStatus")
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            Label lblCompanyEmployeeID = (Label)(gvEmployee.Rows[row.RowIndex].FindControl("lblCompanyEmployeeID"));

            HiddenField hdnStatusID = (HiddenField)(gvEmployee.Rows[row.RowIndex].FindControl("hdnStatusID"));
            if (lblCompanyEmployeeID.Text != "")
            {
                CompanyEmployeeRepository objCompanyEmpRepos = new CompanyEmployeeRepository();
                CompanyEmployee objEmpCop = new CompanyEmployee();
                objEmpCop = objCompanyEmpRepos.GetById(Convert.ToInt64(lblCompanyEmployeeID.Text));
                if (objEmpCop.UserInfoID != null)
                {
                    //Reterive Data of User from Userinformation table.
                    objUInfo = objUserRepos.GetById(Convert.ToInt32(objEmpCop.UserInfoID));

                    UserInformationRepository objUserInfoRepos = new UserInformationRepository();
                    objUserInfoRepos.UpdateStatus(Convert.ToInt32(objEmpCop.UserInfoID), Convert.ToInt32(hdnStatusID.Value));
                    objUserInfoRepos.SubmitChanges();

                    //Select LookupName to check Here Active Or Inactive
                    objLook=objLookupRepos.GetById(Convert.ToInt32(hdnStatusID.Value));
                    if (objLook.sLookupName == "Active")
                    {
                        //after Inactive mail sent to employee
                        sendInApprovalEmail(objUInfo.LoginEmail, objUInfo.FirstName + " " + objUInfo.LastName, objUInfo.UserInfoID);
                    }
                    else
                    {
                        //after Active mail sent to employee
                        sendApprovalEmail(objUInfo.LoginEmail, objUInfo.Password, objUInfo.FirstName + " " + objUInfo.LastName, objUInfo.UserInfoID);
                    }
                    bindgrid();
                }
            }

        }
    }

    protected void gvEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            
        switch (this.ViewState["SortExp"].ToString())
            {
                case "FullName":
                    PlaceHolder placeholderEmployeeName = (PlaceHolder)e.Row.FindControl("placeholderEmployeeName");
                    //placeholderEmployeeName.Controls.Add(ImgSort);
                    break;
                case "Country":
                    PlaceHolder placeholderCountry = (PlaceHolder)e.Row.FindControl("placeholderCountry");
                   // placeholderCountry.Controls.Add(ImgSort);
                    break;
                case "State":
                    PlaceHolder placeholderState = (PlaceHolder)e.Row.FindControl("placeholderState");
                    //placeholderState.Controls.Add(ImgSort);
                    break;
                case "Telephone":
                    PlaceHolder placeholdercontactnumber = (PlaceHolder)e.Row.FindControl("placeholdercontactnumber");
                    //placeholdercontactnumber.Controls.Add(ImgSort);
                    break;
                

            }
           
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CompanyEmpMenuAccessRepository objMenuReporsitory = new CompanyEmpMenuAccessRepository();
            List<CompanyEmpMenuAccess> objMenu = new List<CompanyEmpMenuAccess>();
            objMenu = objMenuReporsitory.GetMenusByEmployeeId(Convert.ToInt64(((Label)e.Row.FindControl("lblCompanyEmployeeID")).Text));
            if (objMenu.Count == 0)
            {
                e.Row.FindControl("imgNew").Visible = true;
            }
            else
            {
                e.Row.FindControl("imgNew").Visible = false;
            }
 
            string path = "~/admin/Incentex_Used_Icons/" + ((HiddenField)e.Row.FindControl("hdnLookupIcon")).Value;
            ((HtmlImage)e.Row.FindControl("imgLookupIcon")).Src = path;
            //((HtmlImage)e.Row.FindControl("imgLookupIconView")).Src = path;
        }

    }

    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        bindgrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        bindgrid();
    }

    protected void dtlViewEmployee_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    protected void dtlViewEmployee_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindgrid();
        }
    }
    private void sendInApprovalEmail(string psEmailAddress, string psUserName, Int64 piUserInfoID)
    {

        try
        {
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Employee Inactivation";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                string sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                string sToadd = psEmailAddress;
                string sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                string sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{fullname}", psUserName);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                string smtphost = Application["SMTPHOST"].ToString();
                int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                string smtpUserID = Application["SMTPUSERID"].ToString();
                string smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(piUserInfoID, "Employee Status", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    private void sendApprovalEmail(string psEmailAddress, string psPassword, string psUserName, Int64 piUserInfoID)
    {

        try
        {
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "employee activation";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                string sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                string sToadd = psEmailAddress;
                string sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                string sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{fullname}", psUserName);
                messagebody.Replace("{password}", psPassword);
                messagebody.Replace("{email}", psEmailAddress);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                objComplist = objCompanyRepos.GetByCompanyId(this.CompanyId);
                if (objComplist.Count > 0)
                {
                    messagebody.Replace("{storename}", objComplist[0].CompanyName);
                }

                string smtphost = Application["SMTPHOST"].ToString();
                int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                string smtpUserID = Application["SMTPUSERID"].ToString();
                string smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(piUserInfoID, "Employee Status", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}

