using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;

public partial class admin_Artwork_SearchArtworkLibrary : PageBase
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Artwork Library";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Artwork/ListArts.aspx?type=Artwork";
            BindDropDowns();
        }
    }
    protected void lnkBtnSearch_Click(object sender, EventArgs e)
    {
        Response.Redirect("ListArtwork.aspx?compID="+ddlCompany.SelectedValue+"&ArtName="+txtArtworkName.Text+"&ArtFor="+ddlArtworkFor.Text+"&ArtDesign="+txtArtworkDesignNumber.Text+"");
    }

    /// <summary>
    /// To Bind Drop Downs
    /// </summary>
    private void BindDropDowns()
    {
        CompanyRepository objRepo = new CompanyRepository();
        List<Company> objList = new List<Company>();
        objList = objRepo.GetAllCompany().OrderBy(c => c.CompanyName).ToList();
        Common.BindDDL(ddlCompany, objList, "CompanyName", "CompanyID", "-Select-");

        LookupRepository objLookRep = new LookupRepository();

        ddlArtworkFor.DataSource = objLookRep.GetByLookup("ArtworkLibrary");
        ddlArtworkFor.DataValueField = "iLookupID";
        ddlArtworkFor.DataTextField = "sLookupName";
        ddlArtworkFor.DataBind();
        ddlArtworkFor.Items.Insert(0, new ListItem("-Select-", "0"));
    }
}
