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
public partial class AssetManagement_Blog_BlogComments : PageBase
{
    #region Data Members
    List<EquipmentBlogComment> objLstComments = new List<EquipmentBlogComment>();
    EquipmentBlogComment objEquipmentBlogComment = new EquipmentBlogComment();
    AssetBlogRepository objAssetBlogRepository = new AssetBlogRepository();
    EquipmentBlogTitle objEquipmentBlogTitle = new EquipmentBlogTitle();
    Common objcommon = new Common();
    Int64 BlogTitleID
    {
        get
        {
            if (ViewState["BlogTitleID"] == null)
            {
                ViewState["BlogTitleID"] = 0;
            }
            return Convert.ToInt64(ViewState["BlogTitleID"]);
        }
        set
        {
            ViewState["BlogTitleID"] = value;
        }
    }
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
            ((Label)Master.FindControl("lblPageHeading")).Text = "Blog Center Comments";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/Blog/BlogDetail.aspx";

            if (Request.QueryString.Count > 0)
            {
                this.BlogTitleID = Convert.ToInt64(Request.QueryString.Get("Id"));
            }
            BindData();
        }
    }
    protected void rptComments_OnItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "DeleteBlog")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }
           bool Deleted= objAssetBlogRepository.DeleteComment(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);
           if (Deleted)
               lblMsg.Text = "Record Deleted Successfully";
           else
               lblMsg.Text = "Record Can't be deleted";
           BindData();
        }       
        
    }
    protected void rptComments_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        String PostedBy = string.Empty;
        String PostedOn = string.Empty;
        UserInformationRepository objUserRepo = new UserInformationRepository();
        UserInformation objUser = objUserRepo.GetById(((EquipmentBlogComment)e.Item.DataItem).CreatedBy);
        PostedBy = objUser.FirstName + " " + objUser.LastName;
        PostedOn = String.Format("{0:MM/dd/yyyy}", (((EquipmentBlogComment)e.Item.DataItem).CreatedDate));
        ((Label)e.Item.FindControl("lblPostedBy")).Text = PostedBy + " " + PostedOn;
    }
    protected void lnkAddBlog_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }
            objEquipmentBlogComment.BlogTitleID = this.BlogTitleID;
            objEquipmentBlogComment.BlogCommentDesc = txtComment.Text;            
            objEquipmentBlogComment.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objEquipmentBlogComment.CreatedDate = DateTime.Now;
            objAssetBlogRepository.Insert(objEquipmentBlogComment);
            objAssetBlogRepository.SubmitChanges();
            BindData();
            txtComment.Text = "";
        }
        catch (Exception)
        {}
    }
    #endregion
    #region Methods

    public void BindData()
    {
        try
        {

            objEquipmentBlogTitle = objAssetBlogRepository.GetByTitleID(this.BlogTitleID);
            lblTitle.Text = objEquipmentBlogTitle.BlogTitleName;
            lblDescription.Text = objEquipmentBlogTitle.TitleDescription;
            if (!string.IsNullOrEmpty(objEquipmentBlogTitle.FilePath))
                imgBlog.ImageUrl = "~/UploadedImages/EquipmentBlog/" + objEquipmentBlogTitle.FilePath;
            else
                imgBlog.ImageUrl = "~/UploadedImages/ProductImages/ProductDefault.jpg";

            objLstComments = objAssetBlogRepository.GetCommentsByTitleID(this.BlogTitleID);
            rptComments.DataSource = objLstComments;
            rptComments.DataBind();
        }
        catch { }
    }


    #endregion
  
}
