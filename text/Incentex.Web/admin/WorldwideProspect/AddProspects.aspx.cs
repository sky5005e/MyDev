using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;

public partial class admin_WorldwideProspect_AddProspects : PageBase
{
    #region
    LookupRepository objLookRep = new LookupRepository();
    WorldwideProspectsRepository objprospectRes = new WorldwideProspectsRepository();

    long ProspectID
    {
        get
        {
            if (ViewState["ProspectID"] != null)
                return Convert.ToInt64(ViewState["ProspectID"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["ProspectID"] = value;
        }
    }
    long Country
    {
        get
        {
            if (ViewState["Country"] != null)
                return Convert.ToInt64(ViewState["Country"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["Country"] = value;
        }
    }
    long BusinessType
    {
        get
        {
            if (ViewState["BusinessType"] != null)
                return Convert.ToInt64(ViewState["BusinessType"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["BusinessType"] = value;
        }
    }
    String CompanyName
    {
        get
        {
            if (ViewState["CompanyName"] != null)
                return ViewState["CompanyName"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["CompanyName"] = value;
        }
    }
    String ContactName
    {
        get
        {
            if (ViewState["ContactName"] != null)
                return ViewState["ContactName"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["ContactName"] = value;
        }
    }
    String Email
    {
        get
        {
            if (ViewState["Email"] != null)
                return ViewState["Email"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["Email"] = value;
        }
    }

    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Worldwide Prospects";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }


            ((Label)Master.FindControl("lblPageHeading")).Text = "Add Worldwide Prospects";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/WorldwideProspect/WorldwideProspects.aspx";


            this.BindDropDown();

            if (Request.QueryString["ProspectID"] != null)
            {
                this.ProspectID = Convert.ToInt64(Request.QueryString["ProspectID"]);
                if (!String.IsNullOrEmpty(Request.QueryString["Company"]))
                    this.CompanyName = Convert.ToString(Request.QueryString["Company"]);
                if (!String.IsNullOrEmpty(Request.QueryString["Contact"]))
                    this.ContactName = Convert.ToString(Request.QueryString["Contact"]);
                if (!String.IsNullOrEmpty(Request.QueryString["Email"]))
                    this.Email = Convert.ToString(Request.QueryString["Email"]);
                if (!string.IsNullOrEmpty(Request.QueryString["BusinessType"]))
                    this.BusinessType = Convert.ToInt64(Request.QueryString["BusinessType"]);
                if (!string.IsNullOrEmpty(Request.QueryString["Country"]))
                    this.Country = Convert.ToInt64(Request.QueryString["Country"]);

                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/WorldwideProspect/ListWorldwideProspects.aspx?Company=" + this.CompanyName + "&Contact=" + this.ContactName + "&Email=" + this.Email + "&BusinessType=" + this.BusinessType + "&Country=" + this.Country + "&State=" + "";
                this.Getwordwideprospectdetails();
            }
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        WorldWideProspect objprospect = new WorldWideProspect();

        objprospect.CompanyName = txtCompanyName.Text.Trim();
        objprospect.ContactName = txtContactName.Text.Trim();
        objprospect.Email = txtEmail.Text.Trim();
        objprospect.BusinessTypeID = Convert.ToInt64(ddlBusinessType.SelectedValue);
        objprospect.CountryID = Convert.ToInt64(ddlCountry.SelectedValue);
        objprospect.MailFlag = true; 

        //Add Mode
        if (this.ProspectID == 0)
        {
            objprospect.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objprospect.CreatedDate = System.DateTime.Now;
            objprospectRes.Insert(objprospect);
        }
        else
        {
            objprospect.ProspectID = this.ProspectID;
            objprospect.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objprospect.UpdatedDate = System.DateTime.Now;
            objprospectRes.updateprospect(objprospect);

            Response.Redirect("ListWorldwideProspects.aspx?Company=" + this.CompanyName + "&Contact=" + this.ContactName + "&Email=" + this.Email + "&BusinessType=" + this.BusinessType + "&Country=" + this.Country + "&State=" + "");
        }
        objprospectRes.SubmitChanges();

        Response.Redirect("WorldwideProspects.aspx?req=1");
    }
    #endregion

    #region Methods
    private void BindDropDown()
    {
        #region Country
        CountryRepository objCountryRepo = new CountryRepository();
        List<INC_Country> objCountryList = objCountryRepo.GetAll();

        Common.BindDDL(ddlCountry, objCountryList, "sCountryName", "iCountryID", "-select-");
        ddlCountry.SelectedValue = ddlCountry.Items.FindByText("United States").Value;
        #endregion

        #region Business Type
        ddlBusinessType.DataSource = objLookRep.GetByLookup("BusinessType");
        ddlBusinessType.DataValueField = "iLookupID";
        ddlBusinessType.DataTextField = "sLookupName";
        ddlBusinessType.DataBind();
        ddlBusinessType.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion
    }

    private void Getwordwideprospectdetails()
    {
        WorldWideProspect objprospect = objprospectRes.GetworldwideprospectByID(this.ProspectID);

        txtCompanyName.Text = objprospect.CompanyName;
        txtContactName.Text = objprospect.ContactName;
        txtEmail.Text = objprospect.Email;
        ddlBusinessType.SelectedValue = Convert.ToString(objprospect.BusinessTypeID);
        ddlCountry.SelectedValue = Convert.ToString(objprospect.CountryID);
    }

    #endregion
}
