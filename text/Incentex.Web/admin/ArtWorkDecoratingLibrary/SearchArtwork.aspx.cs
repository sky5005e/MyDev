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
using Incentex.DAL;
using Incentex.DA;
using Incentex.BE;

public partial class admin_ArtWorkDecoratingLibrary_SearchArtwork : PageBase
{
    CatogeryRepository objCatRepository = new CatogeryRepository();
    LookupRepository objLookupRepos = new LookupRepository();
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
            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Image Library";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/ArtWorkDecoratingLibrary/ArtworkIndex.aspx";
            BindValues();
          

        }
    }
    public void BindValues()
    {
        CompanyRepository objRepo = new CompanyRepository();
        List<Company> objList = new List<Company>();
        objList = objRepo.GetAllCompany();
        Common.BindDDL(ddlCompany, objList, "CompanyName", "CompanyID", "-select customer-");

       
    }
    protected void lnkBtnSearchInfo_Click(object sender, EventArgs e)
    {
        Response.Redirect("ListArtworkLibrary.aspx?compID=" + ddlCompany.SelectedValue + "&ArtName=" + txtFileName.Text + "");
    }
}
