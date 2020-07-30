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

public partial class admin_Artwork_SearchArtwork : PageBase
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
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Artwork/ListArts.aspx";
            BindValues();
            BindWorkGroup();
            GetUniqueFC();
            FillCategory();
            rbArtCategory.Checked = true;
            ShowControl();
            // Set Sub category
            ddlSubcategory.Items.Insert(0, new ListItem("-select-", "0"));

        }
    }
    public void BindValues()
    {
        CompanyRepository objRepo = new CompanyRepository();
        List<Company> objList = new List<Company>();
        objList = objRepo.GetAllCompany();
        Common.BindDDL(ddlCompany, objList, "CompanyName", "CompanyID", "-select customer-");

        LookupDA sEU = new LookupDA();
        LookupBE sEUBE = new LookupBE();
        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "lnkFileCategory";

        DataSet dsSu = sEU.LookUp(sEUBE);

        ddlStoreStatus.DataSource = dsSu;
        ddlStoreStatus.DataTextField = "sLookupName";
        ddlStoreStatus.DataValueField = "iLookupID";

        ddlStoreStatus.DataBind(); ddlStoreStatus.Items.Insert(0, new ListItem("-select file category-", "0"));



    }
    private void BindWorkGroup()
    {
        LookupDA sLookup = new LookupDA();
        LookupBE sLookupBE = new LookupBE();
        // For Workgroup
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "Workgroup";
        ddlWorkgroup.DataSource = sLookup.LookUp(sLookupBE);
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    public void FillCategory()
    {
        try
        {
            List<Category> objCatlist = new List<Category>();
            objCatlist = objCatRepository.GetAllCategory();
            if (objCatlist.Count != 0)
            {
                ddlProduct.DataSource = objCatlist;
                ddlProduct.DataValueField = "CategoryID";
                ddlProduct.DataTextField = "CategoryName";
                ddlProduct.DataBind();
                ddlProduct.Items.Insert(0, new ListItem("-select-", "0"));
                
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }

    private void ShowControl()
    {
        if (rbArtCategory.Checked)
        {
            trCompany.Visible = true;
            trStoreStatus.Visible = true;
            trFileName.Visible = true;
            trProduct.Visible = false;
            trSubCategory.Visible = false;
            trWorkgroup.Visible = false;
        }
        else
        {
            trProduct.Visible = true;
            trSubCategory.Visible = true;
            trWorkgroup.Visible = true;
            trCompany.Visible = false;
            trStoreStatus.Visible = false;
            trFileName.Visible = false;
        }
    }
    protected void lnkBtnSearchInfo_Click(object sender, EventArgs e)
    {
        if (rbProductCategory.Checked)
        {
            Response.Redirect("ViewSearchResults.aspx?Pid=" + ddlProduct.SelectedItem.Value + "&SubCatID=" + ddlSubcategory.SelectedItem.Value + "&WorkGroupID=" + ddlWorkgroup.SelectedValue + "");
        }
        else
        {
            Response.Redirect("ViewSearchResults.aspx?cid=" + ddlCompany.SelectedItem.Value + "&aid=" + ddlStoreStatus.SelectedItem.Value + "&fname=" + txtFileName.Text + "");
        }
    }
    public void GetUniqueFC()
    {
        ArtWorkRepository rp = new ArtWorkRepository();
        List<CompanyArtWorkResults> results = rp.getUniqueFileCategories();
    }
    protected void rbProductCategory_CheckedChanged(object sender, EventArgs e)
    {
        if (rbProductCategory.Checked)
        {
            trProduct.Visible = true;
            trSubCategory.Visible = true;
            trWorkgroup.Visible = true;
            trCompany.Visible = false;
            trStoreStatus.Visible = false;
            trFileName.Visible = false;
        }
        else
        {
            trCompany.Visible = true;
            trStoreStatus.Visible = true;
            trFileName.Visible = true;
            trProduct.Visible = false;
            trSubCategory.Visible = false;
            trWorkgroup.Visible = false;
        }
        
    }
    protected void rbArtCategory_CheckedChanged(object sender, EventArgs e)
    {
        if (rbArtCategory.Checked)
        {
            trCompany.Visible = true;
            trStoreStatus.Visible = true;
            trFileName.Visible = true;
            trProduct.Visible = false;
            trSubCategory.Visible = false;
            trWorkgroup.Visible = false;
        }
        else
        {
            trProduct.Visible = true;
            trSubCategory.Visible = true;
            trWorkgroup.Visible = true;
            trCompany.Visible = false;
            trStoreStatus.Visible = false;
            trFileName.Visible = false;
        }
        
    }
    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            List<SubCategory> objsubCatlist = new List<SubCategory>();
            objsubCatlist = objCatRepository.GetAllSubCategory(Convert.ToInt32(ddlProduct.SelectedValue));
            if (objsubCatlist.Count != 0)
            {
                ddlSubcategory.DataSource = objsubCatlist;
                ddlSubcategory.DataValueField = "SubCategoryID";
                ddlSubcategory.DataTextField = "SubCategoryName";
                ddlSubcategory.DataBind();
                ddlSubcategory.Items.Insert(0, new ListItem("-select-", "0"));
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}
