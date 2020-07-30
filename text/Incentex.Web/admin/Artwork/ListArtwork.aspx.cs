using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Artwork_ListArtwork : PageBase
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
    Int64 ArtworkForID
    {
        get
        {
            if (ViewState["ArtworkForID"] != null)
                return Convert.ToInt64(ViewState["ArtworkForID"]);
            else
                return 0;
        }
        set
        {
            ViewState["ArtworkForID"] = value;
        }
    }
    Int64 ArtworkDesignNumber
    {
        get
        {
            if (ViewState["ArtworkDesignNumber"] != null)
                return Convert.ToInt64(ViewState["ArtworkDesignNumber"]);
            else
                return 0;
        }
        set
        {
            ViewState["ArtworkDesignNumber"] = value;
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

            ((Label)Master.FindControl("lblPageHeading")).Text = "Artwork Library";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Artwork/SearchArtworkLibrary.aspx";

            this.CompanyId = Convert.ToInt64(Request.QueryString["compID"]);
            if (!String.IsNullOrEmpty(Request.QueryString["ArtDesign"]))
                this.ArtworkDesignNumber = Convert.ToInt64(Request.QueryString["ArtDesign"]);
            this.ArtworkName = Request.QueryString["ArtName"];
            if (!String.IsNullOrEmpty(Request.QueryString["ArtFor"]))
                this.ArtworkForID = Convert.ToInt64(Request.QueryString["ArtFor"]);

            BindGridView();
        }
    }
    #endregion
    #region Method's

    private void BindGridView()
    {
        List<AllArtWorkLibrary> list = objArtRepo.GetAllArtworkLibrary(this.CompanyId, this.ArtworkName, this.ArtworkForID, this.ArtworkDesignNumber);
        grdView.DataSource = list;
        grdView.DataBind();
    }

    protected void grdView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "del")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            objArtRepo.DeleteArtworkLibraryById(Convert.ToInt32(e.CommandArgument.ToString()));
            objArtRepo.SubmitChanges();
            lblMsgGlobal.Text = "Record deleted successfully";
            BindGridView();
        }
    }
    #endregion


}
