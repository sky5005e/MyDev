using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyStore_HelpVideo : PageBase
{
    #region Page Variable's
    Int64 iStoreid
    {
        get
        {
            return Convert.ToInt64(ViewState["iStoreid"]);
        }
        set
        {
            ViewState["iStoreid"] = value;
        }
    }
    Int64 HelpVideoID
    {
        get
        {
            return Convert.ToInt64(ViewState["HelpVideoID"]);
        }
        set
        {
            ViewState["HelpVideoID"] = value;
        }
    }
    #endregion 
    #region Page Event's
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Store Management Console";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Help Video";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/ViewCompanyStore.aspx";

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                this.iStoreid = Convert.ToInt64(Request.QueryString.Get("id"));
            

            BindDropDowns();
            BindGridView();
        }
    }
    protected void lnkSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            VideoTrainingRepository objVideoRepo = new VideoTrainingRepository();
            HelpVideo obj = new HelpVideo();

            if (HelpVideoID > 0)
                obj = objVideoRepo.GetHelpVideoByID(this.HelpVideoID);

            obj.CompanyID = new CompanyStoreRepository().GetById(this.iStoreid).CompanyID;
            obj.CreateDate = DateTime.Now;
            obj.VideoForID = Convert.ToInt64(ddlVideoFor.SelectedValue);
            obj.VideoName = Convert.ToString(txtVideoName.Text);
            obj.VideoPath = Convert.ToString(txtVideoPath.Text);

            if (HelpVideoID == 0)
                objVideoRepo.Insert(obj);

            objVideoRepo.SubmitChanges();
            ResetControl();
            BindGridView();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    protected void gvVideoTemplatesList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            if (this.ViewState["SortOrder"].ToString() == "ASC")
                ImgSort.ImageUrl = "";
            else
                ImgSort.ImageUrl = "";
            switch (this.ViewState["SortExp"].ToString())
            {

                case "ViewTemp":
                    PlaceHolder placeholderViewTemp = (PlaceHolder)e.Row.FindControl("placeholderViewTemp");
                    break;
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           
        }
    }
    protected void gvVideoTemplatesList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "del")
        {
            new VideoTrainingRepository().DeleteHelpVideo(Convert.ToInt64(e.CommandArgument));
            BindGridView();
        }
        if (e.CommandName == "updateData")
        {
            PopulateData(Convert.ToInt64(e.CommandArgument));
        }
    }

    #endregion 

    #region Page Method
    private void BindDropDowns()
    {
        LookupRepository objLookupRepos = new LookupRepository();
        //For Product Status 
        ddlVideoFor.DataSource = objLookupRepos.GetByLookup("HelpVideo");
        ddlVideoFor.DataValueField = "iLookupID";
        ddlVideoFor.DataTextField = "sLookupName";
        ddlVideoFor.DataBind();
        ddlVideoFor.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    private void BindGridView()
    {
        gvVideoTemplatesList.DataSource = new VideoTrainingRepository().GetAllHelpVideo(this.iStoreid).ToList();
        gvVideoTemplatesList.DataBind();
    }

    private void ResetControl()
    {
        this.txtVideoName.Text = String.Empty;
        this.txtVideoPath.Text = String.Empty;
        this.ddlVideoFor.SelectedIndex = 0;
        this.HelpVideoID = 0;
    }

    private void PopulateData(Int64 helpVideoID)
    {
        HelpVideo obj = new HelpVideo();
        obj = new VideoTrainingRepository().GetHelpVideoByID(helpVideoID);
        if (obj != null)
        {
            this.HelpVideoID = Convert.ToInt64(obj.HelpVideoID);
            this.txtVideoName.Text = Convert.ToString(obj.VideoName);
            this.txtVideoPath.Text = Convert.ToString(obj.VideoPath);
            this.ddlVideoFor.SelectedValue = Convert.ToString(obj.VideoForID);
        }
    }
    #endregion
}

