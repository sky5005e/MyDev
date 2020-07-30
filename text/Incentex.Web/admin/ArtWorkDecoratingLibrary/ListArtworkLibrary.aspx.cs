using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_ArtWorkDecoratingLibrary_ListArtworkLibrary : PageBase
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
    String ArtworkName
    {
        get
        {
            if (ViewState["ArtworkName"] != null)
                return ViewState["ArtworkName"].ToString();
            else
                return null;
        }
        set
        {
            ViewState["ArtworkName"] = value;
        }
    }
   

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

            if (Session["ArtImagrReturnURL"] != null)
            {
                Session["ArtImagrReturnURL"] = null;
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Artwork Library";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/ArtWorkDecoratingLibrary/SearchArtwork.aspx";

            if (!String.IsNullOrEmpty(Request.QueryString["compID"]))
                this.CompanyId = Convert.ToInt64(Request.QueryString["compID"]);

            if (!String.IsNullOrEmpty(Request.QueryString["ArtName"]))
                this.ArtworkName = Request.QueryString["ArtName"];          

            BindGridView();
        }
    }
    #endregion
    #region Method's

    private void BindGridView()
    {
        List<AllArtWorkLibrary> list = objArtRepo.GetAllArtworkLibrary(this.CompanyId, this.ArtworkName);
        grdView.DataSource = list;
        grdView.DataBind();
    }

    protected void grdView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "del")
        {
            objArtRepo.DeleteArtworkLibraryById(Convert.ToInt32(e.CommandArgument.ToString()));
            objArtRepo.SubmitChanges();
            lblMsgGlobal.Text = "Record deleted successfully";
            BindGridView();
        }
    }
    #endregion
}
