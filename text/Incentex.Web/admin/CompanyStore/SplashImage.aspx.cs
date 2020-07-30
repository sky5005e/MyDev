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
using Incentex.BE;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;

public partial class admin_CompanyStore_SplashImage :PageBase
{
    #region Local Property
    
    CompanyStoreDocumentRepository objRep = new CompanyStoreDocumentRepository();
    List<StoreDocument> objStoreDocumentList = new List<StoreDocument>();
    StoreDocument objStoreDocument = new StoreDocument();
    Common objcommon = new Common();
    string sFilePath;
    string sFileName;
    Int64 CompanyStoreId
    {
        get
        {
            if (ViewState["CompanyStoreId"] == null)
            {
                ViewState["CompanyStoreId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {
            ViewState["CompanyStoreId"] = value;
        }
    }
    #endregion

    #region Page Load Event
    
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

            if (Request.QueryString.Count > 0)
            {   
                //Assign Page Header and return URL 
                this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));

                if (this.CompanyStoreId == 0)
                {
                    Response.Redirect("~/admin/CompanyStore/GeneralStoreSetup.aspx?Id=" + this.CompanyStoreId);
                }

                if (this.CompanyStoreId > 0 && !base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }                

                ((Label)Master.FindControl("lblPageHeading")).Text = "Splash Image";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/CompanyStore/ViewCompanyStore.aspx";
                Session["ManageID"] = 5;

                //Populate menu control from the menu control
                menuControl.PopulateMenu(1, 0, this.CompanyStoreId, 0, false);

                BindDropDowns();
            }
        }
    }
    #endregion

    #region Button Click events
    
    protected void lnkBtnUploadWorkgroup_Click(object sender, EventArgs e)
    {

        if (((float)Request.Files[0].ContentLength / 1048576) > 2)
        {
            lblMsg.Text = "The file you are uploading is more than 2MB.";
            return;
        }

        objStoreDocument.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
        objStoreDocument.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
        objStoreDocument.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objStoreDocument.CretedDate = System.DateTime.Now;
        objStoreDocument.DocuemntFor = Incentex.DAL.Common.DAEnums.CompanyStoreDocumentFor.SplashImage.ToString();
        sFileName = Incentex.DAL.Common.DAEnums.CompanyStoreDocumentFor.SplashImage.ToString() + "_" + this.CompanyStoreId.ToString() + "_" + Convert.ToInt64(ddlWorkgroup.SelectedValue) + "_" + System.DateTime.Now.Ticks + "_" + fpUpload. Value;
        sFilePath = Server.MapPath("~/UploadedImages/CompanyStoreDocuments/") + sFileName;
        Request.Files[0].SaveAs(sFilePath);
        if (fpUpload.Value != null)
            objStoreDocument.DocumentName = sFileName;
        else
            objStoreDocument.DocumentName = null;
        objStoreDocument.StoreId = this.CompanyStoreId;
        objRep.Insert(objStoreDocument);
        objRep.SubmitChanges();

        //
        getsplashImagesbyworkgroup();
    }
    protected void lnkBtnGetReocords_Click(object sender, EventArgs e)
    {
        getsplashImagesbyworkgroup();
    }
    #endregion

    #region Miscellaneous functions

     //bind dropdown for the workgroup, department
    public void BindDropDowns()
    {
        LookupRepository objLookRep = new LookupRepository();


        // For Department
        ddlDepartment.DataSource = objLookRep.GetByLookup("Department");
        ddlDepartment.DataValueField = "iLookupID";
        ddlDepartment.DataTextField = "sLookupName";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Workgroup
        ddlWorkgroup.DataSource = objLookRep.GetByLookup("Workgroup");
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));
    }


    /// <summary>
    /// Get Splash Images by specific workgroup
    /// </summary>
    public void getsplashImagesbyworkgroup()
    {

         
         objStoreDocumentList=objRep.GetSplashImagesdById(Convert.ToInt64(ddlWorkgroup.SelectedValue), this.CompanyStoreId);
         if (objStoreDocumentList.Count == 5)
         {
             lnkBtnUploadWorkgroup.Visible = false;
             tdUpload.Visible = false;
             trNote.Visible = true;
             lblMsg.Text = string.Empty;
         }
         else
         {
             if (objStoreDocumentList.Count == 0)
             {
                 lblMsg.Text = "No Splash image uploaded for selected workgroup..";
             }
             else
             {
                 lblMsg.Text = string.Empty;
             }
             lnkBtnUploadWorkgroup.Visible = true;
             tdUpload.Visible = true;
             trNote.Visible = false;
         }
         dtSplash.DataSource = objStoreDocumentList;
         dtSplash.DataBind();
    }
    #endregion

    #region DataList Events
    
    /// <summary>
    /// Datalst Item data bound command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dtSplash_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ((HtmlImage)e.Item.FindControl("imgSplashImage")).Src = "~/UploadedImages/CompanyStoreDocuments/"+ ((HiddenField)e.Item.FindControl("hdndocumentname")).Value;
            ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/CompanyStoreDocuments/" + ((HiddenField)e.Item.FindControl("hdndocumentname")).Value;
            //((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "../../controller/createthumb.aspx?_ty=CompanyStoreDocument&_path=" + ((HiddenField)e.Item.FindControl("hdndocumentname")).Value + "&_twidth=600&_theight=600";
            
             //"~/UploadedImages/CompanyStoreDocuments/" + ((HiddenField)e.Item.FindControl("hdndocumentname")).Value;

        }
    }

    /// <summary>
    /// Datalst Itemcommand event
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void dtSplash_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "DeleteSplashImage")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            objRep.DeleteCompanyStoreDocument(e.CommandArgument.ToString());
            objcommon.DeleteImageFromFolder(((HiddenField)e.Item.FindControl("hdndocumentname")).Value, IncentexGlobal.companystoredocuments);

            getsplashImagesbyworkgroup();
        }
    }
    #endregion
}
