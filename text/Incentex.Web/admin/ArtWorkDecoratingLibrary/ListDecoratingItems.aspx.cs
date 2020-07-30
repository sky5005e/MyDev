using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
public partial class admin_ArtWorkDecoratingLibrary_ListDecoratingItems : PageBase
{
    #region Data Member's
    Int64 CompanyId
    {
        get
        {
            if (ViewState["CompanyId"] != null)
                return Convert.ToInt64(ViewState["CompanyId"]);
            else
                return 0;
        }
        set
        {
            ViewState["CompanyId"] = value;
        }
    }
    String JobName
    {
        get
        {
            if (ViewState["JobName"] != null)
                return ViewState["JobName"].ToString();
            else
                return null;
        }
        set
        {
            ViewState["JobName"] = value;
        }
    }

    DecoratingSpecsRepository objRepoDeco = new DecoratingSpecsRepository();
    ArtWorkRepository objArtRepo = new ArtWorkRepository();
    #endregion
    #region Event's
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Artwork Library";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

          
            ((Label)Master.FindControl("lblPageHeading")).Text = "Result Library";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/ArtWorkDecoratingLibrary/SearchDecoratingInst.aspx";

            if (!String.IsNullOrEmpty(Request.QueryString["compID"]))
                this.CompanyId = Convert.ToInt64(Request.QueryString["compID"]);

            if (!String.IsNullOrEmpty(Request.QueryString["JobName"]))
                this.JobName = Request.QueryString["JobName"];

            BindGridView();
        }
    }
    #endregion
    #region Method's

    private void BindGridView()
    {
        List<AllDecoratingSpecLibrary> list = objRepoDeco.GetAllDecoratingSpecLibrary(this.CompanyId, this.JobName);
        grdViewDecoSpec.DataSource = list;
        grdViewDecoSpec.DataBind();
    }

   
    #endregion
}

