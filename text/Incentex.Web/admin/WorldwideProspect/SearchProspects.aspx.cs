using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;

public partial class admin_WorldwideProspect_SearchProspects : PageBase
{

    #region
    LookupRepository objLookRep = new LookupRepository();
    WorldwideProspectsRepository objprospectRes = new WorldwideProspectsRepository();
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

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Worldwide Prospects";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/WorldwideProspect/WorldwideProspects.aspx";

            this.BindDropDown();
        }
    }

    protected void lnkBtnsearch_Click(object sender, EventArgs e)
    {
        Response.Redirect("ListWorldwideProspects.aspx?Company=" + txtCompanyName.Text.Trim() + "&Contact=" + txtContactName.Text.Trim() + "&Email=" + txtEmail.Text.Trim() + "&BusinessType=" + ddlBusinessType.SelectedValue + "&Country=" + ddlCountry.SelectedValue + "");
    }
    #endregion

    #region Methods
    private void BindDropDown()
    {
        #region Country
        CountryRepository objCountryRepo = new CountryRepository();
        List<INC_Country> objCountryList = objCountryRepo.GetAll();

        Common.BindDDL(ddlCountry, objCountryList, "sCountryName", "iCountryID", "-select-");
        #endregion

        #region Business Type
        ddlBusinessType.DataSource = objLookRep.GetByLookup("BusinessType");
        ddlBusinessType.DataValueField = "iLookupID";
        ddlBusinessType.DataTextField = "sLookupName";
        ddlBusinessType.DataBind();
        ddlBusinessType.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion
    }
    #endregion
}
