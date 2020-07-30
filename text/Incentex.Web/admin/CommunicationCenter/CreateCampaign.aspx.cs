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
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL.Common;
using System.Web.Services;

public partial class admin_CommunicationCenter_CreateCampaign : PageBase
{

    string EmailSubject = string.Empty;
    Campaign ObjCamp = new Campaign();
    CampignRepo ObjRepo = new CampignRepo();
    CompanyRepository objRepo = new CompanyRepository();

    /// <summary>
    /// To set true when editmode is selected else false for new insert records
    /// </summary>
    Boolean editFlag
    {
        get
        {
            if (ViewState["editFlag"] == null)
            {
                ViewState["editFlag"] = 0;
            }
            return Convert.ToBoolean(ViewState["editFlag"]);
        }
        set
        {
            ViewState["editFlag"] = value;
        }
    }
    /// <summary>
    ///  Campaign ID
    /// </summary>
    Int32 campID
    {
        get
        {
            if (ViewState["campID"] == null)
            {
                ViewState["campID"] = 0;
            }
            return Convert.ToInt32(ViewState["campID"]);
        }
        set
        {
            ViewState["campID"] = value;
        }
    }
    private Int64 EmployeeID
    {
        get
        {
            if (ViewState["EmployeeID"] == null)
            {
                ViewState["EmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["EmployeeID"]);
        }
        set
        {
            ViewState["EmployeeID"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["campID"]) && Request.QueryString["campID"] != "0")
                base.MenuItem = "View Campaign";
            else
                base.MenuItem = "Create Campaign";

            base.ParentMenuID = 29;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Create Email Campaign";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CampaignSelection.aspx";

            BindValues();
            FillCountryDDL();
            FillEmployeeType();
            LoadExcludingCompanies();
            ddlEmployee.Items.Insert(0, new ListItem("-Select-", "0"));

            if (!String.IsNullOrEmpty(Request.QueryString["campID"]) && Request.QueryString["campID"] != "0")
            {
                this.campID = Convert.ToInt32(Request.QueryString["campID"]);
                this.EmployeeID = Convert.ToInt64(Request.QueryString["uid"]);
                this.editFlag = true;
                PopulateData();
                ddlCompany_SelectedIndexChanged(sender, e);
            }

            Session["Isprospect"] = null;
            if (!string.IsNullOrEmpty(Request.QueryString["Isprospect"]))
            {
                this.SetWordwideprospectCampainData(); 
            }
        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd && !base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        Response.Redirect("CreateCampaignStep2.aspx");
    }

    //bind dropdown for the company, workgroup, department, gender, 
    public void BindValues()
    {
        //get company
        List<Company> objcomList = new List<Company>();
        objcomList = objRepo.GetAllCompany().OrderBy(c => c.CompanyName).ToList();
        Common.BindDDL(ddlCompany, objcomList, "CompanyName", "CompanyID", "-Select Company-");

        // For base stations
        List<INC_BasedStation> objbsList = new List<INC_BasedStation>();
        objbsList = objRepo.GetAllBaseStationResult().OrderBy(b => b.sBaseStation).ToList();
        Common.BindDDL(ddlBaseStation, objbsList, "sBaseStation", "iBaseStationId", "-Select All Base Station-");

        LookupRepository objLookRep = new LookupRepository();
        //For gender
        ddlGender.DataSource = objLookRep.GetByLookup("Gender");
        ddlGender.DataValueField = "iLookupID";
        ddlGender.DataTextField = "sLookupName";
        ddlGender.DataBind();
        ddlGender.Items.Insert(0, new ListItem("-Both-", "0"));
        // For Department
        ddlDepartment.DataSource = objLookRep.GetByLookup("Department");
        ddlDepartment.DataValueField = "iLookupID";
        ddlDepartment.DataTextField = "sLookupName";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("-Select All Department-", "0"));
        // For Workgroup
        ddlWorkgroup.DataSource = objLookRep.GetByLookup("Workgroup");
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-Select All Workgroup-", "0"));
    }


    /// <summary>
    /// Fills the country DDL.
    /// Created by mayur on 10-jan-2012
    /// </summary>
    private void FillCountryDDL()
    {
        ddlCountry.DataSource = new CountryRepository().GetAll();
        ddlCountry.DataTextField = "sCountryName";
        ddlCountry.DataValueField = "iCountryID";
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("-Select Country-", "0"));
    }

    private void FillEmployeeType()
    {
        ddlEmployeeType.DataSource = new UserTypeRepository().GetUserTypes();
        ddlEmployeeType.DataTextField = "UserType1";
        ddlEmployeeType.DataValueField = "UserTypeID";
        ddlEmployeeType.DataBind();
        ddlEmployeeType.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    /*
    [WebMethod()]
    public static List<CampignRepo.SearchCampUser> BindIncentexEmployee()
    {
        CampignRepo ObjRepo = new CampignRepo();
        List<CampignRepo.SearchCampUser> objList = ObjRepo.GetIncentexEmp(0).OrderBy(u => u.FullName).ToList();
        return objList;
    }

    [WebMethod()]
    public static List<CampignRepo.SearchCampUser> BindCompanyEmployee(string companyID)
    {
        CampignRepo ObjRepo = new CampignRepo();
        List<CampignRepo.SearchCampUser> objList = ObjRepo.GetCompanyEmp(Convert.ToInt64(companyID), 0).OrderBy(u => u.FullName).ToList();
        return objList;
    }
    */

    /// <summary>
    /// for insert the record into campaign table
    /// </summary>

    protected void lnkNext_Click1(object sender, EventArgs e)
    {

        if (this.campID != 0)
        {
            ObjCamp = ObjRepo.GetDetailFromCampID(this.campID);
        }
        ObjCamp.Name = Convert.ToString(txtCampaignName.Text.Trim());
        if (!String.IsNullOrEmpty(ObjCamp.Name))
            Session["CampName"] = ObjCamp.Name;
        ObjCamp.Subject = Convert.ToString(txtSubject.Text.Trim());
        if (!String.IsNullOrEmpty(ObjCamp.Subject))
            Session["EmailSubject"] = ObjCamp.Subject;
        ObjCamp.FromName = Convert.ToString(txtFromName.Text.Trim());
        if (!String.IsNullOrEmpty(ObjCamp.FromName))
            Session["FromName"] = ObjCamp.FromName;
        ObjCamp.FromEmail = Convert.ToString(txtFromEmail.Text.Trim());
        if (!String.IsNullOrEmpty(ObjCamp.FromEmail))
            Session["FromEmail"] = ObjCamp.FromEmail;

        ObjCamp.CompanyID = Convert.ToInt32(ddlCompany.SelectedValue);
        ObjCamp.WorkgroupID = Convert.ToInt32(ddlWorkgroup.SelectedValue);
        ObjCamp.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);
        ObjCamp.Gender = Convert.ToInt16(ddlGender.SelectedValue);
        ObjCamp.StationID = Convert.ToInt32(ddlBaseStation.SelectedValue);
        ObjCamp.CDate = Convert.ToDateTime(DateTime.Now);
        ObjCamp.CountryID = Convert.ToInt64(ddlCountry.SelectedValue);

        Session["CampCreateDate"] = ObjCamp.CDate;

        Session["cid"] = ddlCompany.SelectedValue.ToString();
        Session["wid"] = ddlWorkgroup.SelectedValue.ToString();
        Session["did"] = ddlDepartment.SelectedValue.ToString();
        Session["gid"] = ddlGender.SelectedValue.ToString();
        Session["sid"] = ddlBaseStation.SelectedValue.ToString();
        Session["emptype"] = ddlEmployeeType.SelectedValue.ToString();
        Session["countryID"] = ddlCountry.SelectedValue.ToString();
        if (ddlEmployee.SelectedValue == "")
            Session["uid"] = 0;
        else
            Session["uid"] = ddlEmployee.SelectedValue;


        if (this.campID == 0 && !this.editFlag)
        {
            ObjRepo.Insert(ObjCamp);
            ObjRepo.SubmitChanges();
            Session["CampID"] = ObjCamp.CampaignID;
        }
        else
        {
            Session["CampID"] = this.campID;
            ObjRepo.SubmitChanges();
        }

        if (!String.IsNullOrEmpty(hdnListofExcludedCompanies.Value))
            Session["ExcludeCompList"] = this.hdnListofExcludedCompanies.Value;
        else
            Session["ExcludeCompList"] = null;

        Response.Redirect("CreateCampaignStep2.aspx?cid=" + ddlCompany.SelectedValue + "&wid=" + ddlWorkgroup.SelectedValue + "&did=" + ddlDepartment.SelectedValue + "&gid=" + ddlGender.SelectedValue + "&sid=" + ddlBaseStation.SelectedValue + "&uid=" + ddlEmployee.SelectedValue);

    }

    private void PopulateData()
    {
        // Retrieve Data From campaign table 
        ObjCamp = ObjRepo.GetDetailFromCampID(this.campID);

        txtCampaignName.Text = ObjCamp.Name;
        txtSubject.Text = ObjCamp.Subject;
        txtFromEmail.Text = ObjCamp.FromEmail;
        txtFromName.Text = ObjCamp.FromName;

        ddlCompany.SelectedValue = ObjCamp.CompanyID.ToString();

        ddlDepartment.SelectedValue = ObjCamp.DepartmentID.ToString();
        ddlWorkgroup.SelectedValue = ObjCamp.WorkgroupID.ToString();
        ddlGender.SelectedValue = ObjCamp.Gender.ToString();

        ddlCountry.SelectedValue = ObjCamp.CountryID.ToString();
        ddlBaseStation.SelectedValue = ObjCamp.StationID.ToString();
        if (this.EmployeeID != 0)
            ddlEmployee.SelectedValue = this.EmployeeID.ToString();
        this.editFlag = true;

    }

    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        Response.Redirect("CampaignSelection.aspx");
    }
    private void LoadExcludingCompanies()
    {
        //get company
        List<Company> objcomList = new List<Company>();
        objcomList = objRepo.GetAllCompany().OrderBy(c => c.CompanyName).ToList();
        DataTable dt = Common.ListToDataTable(objcomList);
        this.drpMulti.DataSource = dt;
        this.drpMulti.DataTextField = "CompanyName";
        this.drpMulti.DataValueField = "CompanyID";
        this.drpMulti.DataBind();
    }

    protected void ddlEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlEmployeeType.SelectedValue != "0")
            {
                List<CampignRepo.SearchCampUser> objList = null;
                if (ddlCompany.SelectedValue == "8")// Here 8 for Company  Incentex of Vero Beach, LLC
                    objList = ObjRepo.GetIncentexEmp(0, Convert.ToInt64(ddlEmployeeType.SelectedValue)).OrderBy(u => u.FullName).ToList();
                else
                    objList = ObjRepo.GetCompanyEmp(Convert.ToInt64(ddlCompany.SelectedValue), 0, Convert.ToInt64(ddlEmployeeType.SelectedValue)).OrderBy(u => u.FullName).ToList();

                ddlEmployee.DataSource = objList;
                ddlEmployee.DataValueField = "UserInfoID";
                ddlEmployee.DataTextField = "FullName";
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("-Select All Employee-", "0"));
            }

        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            List<CampignRepo.SearchCampUser> objList = null;
            if (ddlCompany.SelectedValue == "8")// Here 8 for Company  Incentex of Vero Beach, LLC
                objList = ObjRepo.GetIncentexEmp(0, 0).OrderBy(u => u.FullName).ToList();
            else
                objList = ObjRepo.GetCompanyEmp(Convert.ToInt64(ddlCompany.SelectedValue), 0, 0).OrderBy(u => u.FullName).ToList();

            ddlEmployee.DataSource = objList;
            ddlEmployee.DataValueField = "UserInfoID";
            ddlEmployee.DataTextField = "FullName";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("-Select All Employee-", "0"));

            if (ddlCompany.SelectedValue != "0")
            {
                trExcludeCompanies.Visible = false;
                this.hdnListofExcludedCompanies.Value = null;
            }
            else
                trExcludeCompanies.Visible = true;
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void SetWordwideprospectCampainData()
    {
        trcompany.Visible = false;
        trExcludeCompanies.Visible = false;
        trdepartment.Visible = false;
        trworkgroup.Visible = false;
        tremployeetype.Visible = false;
        tremployee.Visible = false;
        trgender.Visible = false;
        trcountry.Visible = false;
        trbaseStation.Visible = false;
        Session["Isprospect"] = "true"; 
    }
}
