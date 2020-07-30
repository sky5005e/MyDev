using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_DocumentStoregeCentre_SearchDocumentStoregeCentre : PageBase
{
    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Document Storage Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Document Storage Centre";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/DocumentStorageCentre/DocumentStorageCenter.aspx";
        }
    }

    protected void lnkBtnSearch_Click(object sender, EventArgs e)
    {
        Response.Redirect("ListDocumentStoregeCenter.aspx?FileName=" + txtFileName.Text.Trim() + "&Searchkeyword=" + txtSearchKeyword.Text.Trim() + "&UplodedBy=" + txtuploadby.Text.Trim() + "");
    }
    #endregion
}
