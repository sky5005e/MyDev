using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_ArtWorkDecoratingLibrary_ArtworkIndex : PageBase
{
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
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";
            ((Label)Master.FindControl("lblPageHeading")).Text = "Artwork & Decorating Library";

            lnkManageDecoratingPartners.NavigateUrl = "~/admin/ArtWorkDecoratingLibrary/MngDecoratingPartners.aspx";
            lnkAddArtwork.NavigateUrl = "~/admin/ArtWorkDecoratingLibrary/AddArtwork.aspx";
            lnkSearchArtwork.NavigateUrl = "~/admin/ArtWorkDecoratingLibrary/SearchArtwork.aspx";
            lnkAddDecoratingSpecifications.NavigateUrl = "~/admin/ArtWorkDecoratingLibrary/AddDecoratingSpecifications.aspx";
            lnkSearchDecoratedItems.NavigateUrl = "~/admin/ArtWorkDecoratingLibrary/SearchDecoratingInst.aspx";
        }
           
    }
}
