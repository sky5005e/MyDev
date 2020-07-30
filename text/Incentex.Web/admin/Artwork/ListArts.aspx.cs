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
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;

public partial class admin_Artwork_ListArts : PageBase
{
    /// <summary>
    /// set true when request type is Artwork
    /// </summary>
    private Boolean IsArtwork
    {
        get {
            if (ViewState["IsArtwork"] == null)
            {
                ViewState["IsArtwork"] = false;
            }
            return (Boolean)ViewState["IsArtwork"];
        }
        set {
            ViewState["IsArtwork"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["type"]) && Request.QueryString["type"] == "Artwork")
            {
                IsArtwork = true;
                base.MenuItem = "Artwork Library";
            }
            else
            {
                IsArtwork = false;
                base.MenuItem = "Image Library";
            }
            
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }
            
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";

            lnkAddArtwork.NavigateUrl = "~/admin/Artwork/AddArtwork.aspx";
            lnkSearchArtwork.NavigateUrl = "~/admin/Artwork/SearchArtworkLibrary.aspx";
            lnkAddImage.NavigateUrl = "~/admin/Artwork/AddArtworkImage.aspx";
            lnkSearchImage.NavigateUrl = "~/admin/Artwork/SearchArtwork.aspx";

            if (IsArtwork)
            {
                ((Label)Master.FindControl("lblPageHeading")).Text = "Artwork Library";
                dvArtWork.Visible = true;
                dvImage.Visible = false;
                if (!String.IsNullOrEmpty(Request.QueryString["req"]) && Request.QueryString["req"] == "1")
                    lblMsg.Text = "Records sucessfully added";
            }
            else
            {
                ((Label)Master.FindControl("lblPageHeading")).Text = "Image Library";
                dvArtWork.Visible = false;
                dvImage.Visible = true;
            }
        }
        
    }
    
}
