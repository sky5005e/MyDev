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
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.IO;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
public partial class AssetManagement_Blog_BlogDetail : PageBase
{
    #region Data Members
    Int64 CompanyID
    {
        get
        {
            if (ViewState["CompanyID"] == null)
            {
                ViewState["CompanyID"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyID"]);
        }
        set
        {
            ViewState["CompanyID"] = value;
        }
    }
    //String SearchString
    //{
    //    get
    //    {
    //        if (ViewState["SearchString"] == null)
    //        {
    //            ViewState["SearchString"] = null;
    //        }
    //        return Convert.ToString(ViewState["SearchString"]);
    //    }
    //    set
    //    {
    //        ViewState["SearchString"] = value;
    //    }
    //}
    //String FromPage
    //{
    //    get
    //    {
    //        if (ViewState["FromPage"] == null)
    //        {
    //            ViewState["FromPage"] = null;
    //        }
    //        return Convert.ToString(ViewState["FromPage"]);
    //    }
    //    set
    //    {
    //        ViewState["FromPage"] = value;
    //    }
    //}
    List<EquipmentBlogTitle> objLstTitle = new List<EquipmentBlogTitle>();
    EquipmentBlogTitle objEquipmentBlogTitle = new EquipmentBlogTitle();
    AssetBlogRepository objAssetBlogRepository = new AssetBlogRepository();
    Common objcommon = new Common();
    #endregion

    #region Event Handlers
    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                MasterPageFile = "~/admin/MasterPageAdmin.master";
            }
            else
            {
                MasterPageFile = "~/MasterPage.master";
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Blog Center";
            base.ParentMenuID = 46;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            if (Request.QueryString.Count > 0)
            {
                Session["SearchString"] = Convert.ToString(Request.QueryString.Get("str"));
                Session["FromPage"] = Convert.ToString(Request.QueryString.Get("FromPage"));
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Blog Center";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            if (Convert.ToString(Session["FromPage"]) == "SearchBlog")
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/Blog/BlogSearch.aspx";
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/Blog/BlogIndex.aspx";

            
            BindData();

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                ddlCompany.SelectedValue = Convert.ToString(IncentexGlobal.CurrentMember.CompanyId);
                ddlCompany.Enabled = false;
            }
            else
            {               
                ddlCompany.Enabled = true;
            }
            
        }
    }

    protected void lnkPost_Click(object sender, EventArgs e)
    {
        string sFilePath = null;
        String filename = String.Empty;
        try
        {
            if (!base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            if (Request.Files.Count > 0)

            {
                if (((float)Request.Files[0].ContentLength / 1048576) > 2)
                {
                    lblMessage.Text = "The file you are uploading is more than 2MB.";
                    ModallnkAddBlog.Show();
                    return;
                }
                if (!String.IsNullOrEmpty(BlogImage.Value))
                {
                    filename = BlogImage.Value;
                    sFilePath = Server.MapPath("../../UploadedImages/EquipmentBlog/") + BlogImage.Value;
                    objcommon.DeleteImageFromFolder(BlogImage.Value, Server.MapPath("../../UploadedImages/InsurancePolicy/"));
                    Request.Files[0].SaveAs(sFilePath);
                }
            }
            if (ddlIsInternal.SelectedValue == "true" && ddlCompany.SelectedValue == "0")
            {
                lblMessage.Text = "Please Select a Company to Post Internal Blog";
                ModallnkAddBlog.Show();
                return;
            }
            if (ddlCompany.SelectedIndex > 0)
                objEquipmentBlogTitle.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);

            objEquipmentBlogTitle.FilePath = filename;
            objEquipmentBlogTitle.IsAnonymous = Convert.ToBoolean(ddlShowUser.SelectedValue);
            objEquipmentBlogTitle.IsInternal = Convert.ToBoolean(ddlIsInternal.SelectedValue);
            objEquipmentBlogTitle.BlogTitleName = txtTitle.Text.Trim();
            objEquipmentBlogTitle.TitleDescription = txtDescription.Text.Trim();
            objEquipmentBlogTitle.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objEquipmentBlogTitle.CreatedDate = DateTime.Now;
            objAssetBlogRepository.Insert(objEquipmentBlogTitle);
            objAssetBlogRepository.SubmitChanges();
            BindData();
        }
        catch (Exception)
        {


        }
    }

    protected void rptTitle_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {       
        //((Image)e.Item.FindControl("imgBlog")).ImageUrl = !String.IsNullOrEmpty(((EquipmentBlogTitle)e.Item.DataItem).FilePath) ? "~/UploadedImages/EquipmentBlog/" + ((EquipmentBlogTitle)e.Item.DataItem).FilePath : "~/UploadedImages/ProductImages/ProductDefault.jpg";
        String PostedBy = string.Empty;
        String PostedOn = string.Empty;
        UserInformationRepository objUserRepo = new UserInformationRepository();
        UserInformation objUser = objUserRepo.GetById(((EquipmentBlogTitle)e.Item.DataItem).CreatedBy);
        PostedBy = objUser.FirstName + " " + objUser.LastName;
        PostedOn = String.Format("{0:MM/dd/yyyy}", (((EquipmentBlogTitle)e.Item.DataItem).CreatedDate));
        if (((EquipmentBlogTitle)e.Item.DataItem).IsAnonymous)        
            ((Label)e.Item.FindControl("lblPostedBy")).Text = "Anonymous post";
        else
            ((Label)e.Item.FindControl("lblPostedBy")).Text = PostedBy + " " + PostedOn;

           
        
    }
    protected void rptTitle_OnItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName=="DeleteBlog")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }
         bool Deleted=objAssetBlogRepository.DeleteBlog(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);
         if (Deleted)
             lblMsg.Text = "Record Deleted";
         else
             lblMsg.Text = "Record Cann't be Deleted";
         BindData();
        }
        else if (e.CommandName == "Continue")
        {            
            Response.Redirect("BlogComments.aspx?id=" + e.CommandArgument);
        }
        else if (e.CommandName == "Image")
        {

            ModalImage.Show();
            //imgBlog.ImageUrl = !String.IsNullOrEmpty(e.CommandArgument) ? "~/UploadedImages/EquipmentBlog/" + ((EquipmentBlogTitle)e.Item.DataItem).FilePath : "~/UploadedImages/ProductImages/ProductDefault.jpg"; ;
            if (!String.IsNullOrEmpty(Convert.ToString(e.CommandArgument)))
              imgBlog.ImageUrl = "~/UploadedImages/EquipmentBlog/" + e.CommandArgument;
            else
                imgBlog.ImageUrl = "~/UploadedImages/ProductImages/ProductDefault.jpg";
           
        }
      
    }

    #endregion

    #region Methods

    //private void BindData()
    //{
    //    try
    //    {
    //        GetCompany();
    //        ResetControls();

    //        if (this.FromPage=="SearchBlog")
    //        {
    //            if (string.IsNullOrEmpty(this.SearchString))
    //                objLstTitle = objAssetBlogRepository.GetTitle(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
    //            else
    //                objLstTitle = objAssetBlogRepository.GetTitleByString(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), this.SearchString);
    //        }
    //        else 
    //        {
    //            if (this.FromPage=="PostBlog")
    //                objLstTitle = objAssetBlogRepository.GetTitleByString(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), this.SearchString);
    //            else
    //                objLstTitle = objAssetBlogRepository.GetTitleByString(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), this.SearchString).Where(x => SqlMethods.DateDiffDay(x.CreatedDate,DateTime.Now)==0).ToList();
    //        }
    //        if (objLstTitle.Count > 0)
    //        {
    //            rptTitle.DataSource = objLstTitle;
    //            rptTitle.DataBind();
    //        }
    //        else
    //            lblMsg.Text = "No Record Found";
    //    }
    //    catch { }
    //}

    private void BindData()
    {
        try
        {
            GetCompany();
            ResetControls();
            objLstTitle = objAssetBlogRepository.GetTitle(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
            lnkAddBlog.Visible = true;
            if (Convert.ToString(Session["FromPage"]) == "SearchBlog")
            {
                lnkAddBlog.Visible = false;
                if (!string.IsNullOrEmpty(Convert.ToString(Session["SearchString"])))
                {
                    objLstTitle = objLstTitle.Where(x => x.BlogTitleName.Contains(Convert.ToString(Session["SearchString"]))).ToList();
                }
            }
            else if (Convert.ToString(Session["FromPage"]) == "TodayBlog")
            {
                objLstTitle = objLstTitle.Where(x => SqlMethods.DateDiffDay(x.CreatedDate, DateTime.Now) == 0).ToList();
                lnkAddBlog.Visible = false;
            }
            if (objLstTitle.Count > 0)
            {
                rptTitle.DataSource = objLstTitle;
                rptTitle.DataBind();
            }
            else
                lblMsg.Text = "No Record Found";
        }
        catch { }
    }

    private void GetCompany()
    {
        try
        {             
            CompanyRepository objCompanyRepo = new CompanyRepository();
            List<Company> objCompanyList = new List<Company>();
            objCompanyList = objCompanyRepo.GetAllCompany();
            Common.BindDDL(ddlCompany, objCompanyList, "CompanyName", "CompanyID", "-Select Company-");
        }
        catch (Exception)
        {}
    }
    private void ResetControls()
    {
        ddlCompany.SelectedIndex = 0;
        ddlIsInternal.SelectedIndex = 0;
        ddlShowUser.SelectedIndex = 0;
        txtTitle.Text = "";
        txtDescription.Text = "";
        lblMsg.Text = "";
    }
    #endregion
}
