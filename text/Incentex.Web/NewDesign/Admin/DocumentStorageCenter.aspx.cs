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
using System.Collections.Generic;


using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using ASP;
using System.IO;
using VimeoNET;


public partial class NewDesign_Admin_DocumentStorageCenter : PageBase
{

    #region Properties & Fields

    public string NewFolder
    {
        get
        {
            return Convert.ToString(ViewState["NewFolder"].ToString());
        }
        set
        {
            ViewState["NewFolder"] = value;
        }
    }


    public string NewFolderId
    {
        get
        {
            return Convert.ToString(ViewState["NewFolderId"].ToString());
        }
        set
        {
            ViewState["NewFolderId"] = value;
        }
    }





    public Int64 FolderId
    {
        get
        {
            if (Convert.ToString(Request.QueryString["folderid"]) == "")
                return 0;
            else
                return Convert.ToInt32(Request.QueryString["folderid"]);
        }
        set
        {
            this.ViewState["folderid"] = value;
        }
    }


    public Int64 Parentid
    {
        get
        {
            if (Convert.ToString(ViewState["Parentid"]) == "")
                return 0;
            else
                return Convert.ToInt32(ViewState["Parentid"]);
        }
        set
        {
            this.ViewState["Parentid"] = value;
        }
    }


    public Int64 subid
    {
        get
        {
            if (Convert.ToString(ViewState["subid"]) == "")
                return 0;
            else
                return Convert.ToInt32(ViewState["subid"]);
        }
        set
        {
            this.ViewState["subid"] = value;
        }
    }

    public Int32 PageIndex
    {
        get
        {
            if (Convert.ToString(this.ViewState["PageIndex"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["PageIndex"]);
        }
        set
        {
            this.ViewState["PageIndex"] = value;
        }
    }




    public Int32 TotalPages
    {
        get
        {
            if (Convert.ToString(this.ViewState["TotalPages"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["TotalPages"]);
        }
        set
        {
            this.ViewState["TotalPages"] = value;
        }
    }

    public Int32 NoOfPagesToDisplay
    {
        get
        {
            if (Convert.ToString(this.ViewState["NoOfPagesToDisplay"]) == "")
                return Convert.ToInt32(Application["NEWPAGERSIZE"]);
            else
                return Convert.ToInt32(this.ViewState["NoOfPagesToDisplay"]);
        }
        set
        {
            this.ViewState["NoOfPagesToDisplay"] = value;
        }
    }

    public Int32 FromPage
    {
        get
        {
            if (Convert.ToString(this.ViewState["FromPage"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["FromPage"]);
        }
        set
        {
            this.ViewState["FromPage"] = value;
        }
    }

    public Int32 ToPage
    {
        get
        {
            if (Convert.ToString(this.ViewState["ToPage"]) == "")
                return this.NoOfPagesToDisplay;
            else
                return Convert.ToInt32(this.ViewState["ToPage"]);
        }
        set
        {
            this.ViewState["ToPage"] = value;
        }
    }


    public Int32 NoOfRecordsToDisplay
    {
        get
        {
            if (Convert.ToString(this.ViewState["NoOfRecordsToDisplay"]) == "")
                return Convert.ToInt32(Application["NEWPAGESIZE"]) % 2 == 0 ? Convert.ToInt32(Application["NEWPAGESIZE"]) : Convert.ToInt32(Application["NEWPAGESIZE"]) - 1;
            //return 2;
            else
                return Convert.ToInt32(this.ViewState["NoOfRecordsToDisplay"]);
        }
        set
        {
            this.ViewState["NoOfRecordsToDisplay"] = value;
        }
    }

    public Int64 ActiveID
    {
        get
        {
            return Convert.ToInt64(this.ViewState["ActiveID"]);
        }
        set
        {
            this.ViewState["ActiveID"] = value;
        }
    }

    public Int64 InActiveID
    {
        get
        {
            return Convert.ToInt64(this.ViewState["InActiveID"]);
        }
        set
        {
            this.ViewState["InActiveID"] = value;
        }
    }


    String SiteURL
    {
        get
        {
            return Convert.ToString(ConfigurationSettings.AppSettings["siteurl"]);
        }
    }



    #endregion

    #region Events
    protected void lbPlayPubVideo_Click(object sender, EventArgs e)
    {
        iframepubvideo.Visible = true;
        iframepubvideo.Attributes.Add("src", "" + hfPubVideo.Value + "");
        //iframepubvideo.Attributes.Add("src", "http://player.vimeo.com/v2/video/79763545");

        lbPlayPubVideo.Visible = false;
        divvidimage.Visible = false;
        OpenPopup("add-publish-block", "dmsvideo-content");
        divShowPubVideo.Visible = true;
    }

    protected void lbContWithoutPublish_Click(object sender, EventArgs e)
    {
        try
        {
            //iframepubvideo.Attributes.Add("scr","");

            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "CloseHelpVideo('ctl00_ContentPlaceHolder1_iframepubvideo');", true);
            divvidimage.Visible = true;
            lbPlayPubVideo.Visible = true;
            divShowPubVideo.Visible = false;
            //string videourl = System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"].ToString() + "DocmentStorageCenter/" + item.originalfilename;
            //hfPubVideo.Value = videourl;
            lbContWithoutPublish.Visible = true;


            MediaRepository objrepos = new MediaRepository();
            MediaAssigned objassigned = objrepos.Getmediaassignedbyid(Convert.ToInt64(HfPubMediaid.Value));
            objassigned.IsPublished = false;
            if (!string.IsNullOrEmpty(Request.QueryString["folderid"]))
            {

                if (hfPubPassword.Value != txtPubPassword.Text)
                {
                    ShowAlertMessage("Can not publish. Folder password is incorrect.");
                    OpenPopup("add-publish-block", "dmsvideo-content");
                    return;
                }
            }

            if (HFPubType.Value == "pub-video")
            {
                objassigned.IsPublished = true;
                objassigned.Publishdate = System.DateTime.Now;
                //objassigned.Vimeoid = vimeoid;
                //objassigned.Vimeourl = "http://player.vimeo.com/v2/video/" + vimeoid;
                if (string.IsNullOrEmpty(objassigned.Vimeourl))
                {
                    objassigned.IsPublished = false;
                    ShowAlertMessage("This video can not be start at the available date range as it is not published to vimeo.");
                }
                objrepos.SubmitChanges();
            }
            else
            {
                objassigned.IsPublished = true;
                objrepos.SubmitChanges();
            }

            if (objassigned.IsPublished == true)
            {
                ShowAlertMessage("Your media file has been published and it is available from " + Convert.ToDateTime(objassigned.PubStartDate).ToShortDateString() + " to " + Convert.ToDateTime(objassigned.PubEndDate).ToShortDateString());
            }
        }
        catch (Exception ex)
        {

        }

    }

    protected void lbPublishvideo_Click1(object sender, EventArgs e)
    {
        try
        {


            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "CloseHelpVideo('ctl00_ContentPlaceHolder1_iframepubvideo');", true);
            divvidimage.Visible = true;
            lbPlayPubVideo.Visible = true;
            divShowPubVideo.Visible = false;
            //string videourl = System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"].ToString() + "DocmentStorageCenter/" + item.originalfilename;
            //hfPubVideo.Value = videourl;
            lbContWithoutPublish.Visible = true;

            MediaRepository objrepos = new MediaRepository();
            if (!string.IsNullOrEmpty(Request.QueryString["folderid"]))
            {

                if (hfPubPassword.Value != txtPubPassword.Text)
                {
                    ShowAlertMessage("Can not publish. Folder password is incorrect.");
                    OpenPopup("add-publish-block", "dmsvideo-content");
                    return;
                }
            }

            MediaAssigned objassigned = objrepos.Getmediaassignedbyid(Convert.ToInt64(HfPubMediaid.Value));
            objassigned.IsPublished = false;
            if (HFPubType.Value == "pub-video")
            {
                string vimeoid = UploadToVimeo(Convert.ToInt64(HfPubMediaid.Value)).ToString();
                if (vimeoid != "-1" && vimeoid != "0")
                {
                    objassigned.IsPublished = true;
                    objassigned.Publishdate = System.DateTime.Now;
                    objassigned.Vimeoid = vimeoid;
                    objassigned.Vimeourl = "https://player.vimeo.com/v2/video/" + vimeoid;
                    objrepos.SubmitChanges();
                }
                else
                {
                    ShowAlertMessage("Not able to upload video to vimeo. Please try again.");
                    OpenPopup("add-publish-block", "dmsvideo-content");
                    return;
                }
            }
            else
            {
                objassigned.IsPublished = true;
                objrepos.SubmitChanges();
            }

            if (objassigned.IsPublished == true)
            {
                ShowAlertMessage("Your media file has been published and it is availble from " + Convert.ToDateTime(objassigned.PubStartDate).ToShortDateString() + " to " + Convert.ToDateTime(objassigned.PubEndDate).ToShortDateString());
            }
        }
        catch (Exception ex)
        {

        }
        //string videourl = System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"].ToString() + "DocmentStorageCenter/" + item.originalfilename;
        //ltPubvideotag.Text = "<a title='Play' class='videoplay-icon'  onclick='ShowVideo(\"" + videourl + "\")' href='#' >Play</a>";
    }

    protected void BtnAssignToStore_Click1(object sender, EventArgs e)
    {
        // Todo:: Remove HFStoreId and hfstoreproductid 
        try
        {
            //if (Session["MediaFileType"] == null || Session["MediaFileName"] == null)
            //{
            //    ShowAlertMessage("Session has been expired please try again.");
            //    OpenPopup("add-assigntostoreproduct", "employess-content");
            //    return;
            //}


            LookupRepository ObjRepos = new LookupRepository();
            Int64? ID = ObjRepos.GetIdByLookupNameAndCode(txtMasterStyleName.Text);
            if (ID == 0)
            {
                ShowAlertMessage("Master item number does not exists in the database");
                OpenPopup("add-assigntostoreproduct", "employess-content");
                return;
            }
            




            MediaRepository ObjMedRepos = new MediaRepository();
            MediaFile ObjMediaFile = null;
            
            if (Convert.ToInt32(ddlFolderForAssignImage.SelectedValue) > 0 && HFStoreImageMode.Value != "Add" && ddlFolderForAssignImage.Enabled == false)
            {
                MediaFolder FolderObj = new MediaFolder();
                FolderObj = ObjMedRepos.GetFolderByID(Convert.ToInt32(ddlFolderForAssignImage.SelectedValue));
            }
            
            if (HFStoreImageMode.Value == "Add")
            {
                ObjMediaFile = new MediaFile();
                ObjMediaFile.OriginalFileName = "Thumb_" + Convert.ToString(Session["MediaFileName"]);
                ResizeImage(Server.MapPath("~/NewDesign/DocmentStorageCenter/"), Server.MapPath("~/NewDesign/DocmentStorageCenter/"), Session["MediaFileName"].ToString(), Common.MakeValidFileName("Large_" + Session["MediaFileName"].ToString()), 427, 570);
                ResizeImage(Server.MapPath("~/NewDesign/DocmentStorageCenter/"), Server.MapPath("~/NewDesign/DocmentStorageCenter/"), Session["MediaFileName"].ToString(), Common.MakeValidFileName("Thumb_" + Session["MediaFileName"].ToString()), 236, 315);
            }
            else
            {
                ObjMediaFile = ObjMedRepos.GetFileById(Convert.ToInt64(hfmediaid.Value));
            }
            ObjMediaFile.Name = txtMasterStyleName.Text;
            
            //ObjMediaFile.SearchTags = txtSearchTag.Text;
            ObjMediaFile.CreatedId = IncentexGlobal.CurrentMember.UserInfoID;
            ObjMediaFile.View = 0;
            //ObjMediaFile.IsStoreImage = false;
            ObjMediaFile.FileType = "image";
            ObjMediaFile.CreatedDate = System.DateTime.Now.Date;
            ObjMediaFile.Isdeleted = false;
            ObjMediaFile.productcategoryid = Convert.ToInt64(ddlProductCategory.SelectedValue);
            
            if (Convert.ToInt64(ddlFolderForAssignImage.SelectedValue) > 0)
                ObjMediaFile.MediaFolderId = Convert.ToInt64(ddlFolderForAssignImage.SelectedValue);

            if (HFStoreImageMode.Value == "Add")
                ObjMedRepos.Insert(ObjMediaFile);

            ObjMedRepos.SubmitChanges();

            MediaAssigned ObjAssign = new MediaAssigned();
            ObjAssign.MediaFileId = ObjMediaFile.MediaFileId;
            ObjAssign.SearchTags = txtMasterStyleName.Text;
            ObjAssign.Status = this.ActiveID;

            if (HFStoreImageMode.Value == "Add")
                ObjMedRepos.Insert(ObjAssign);

            ObjMedRepos.SubmitChanges();

            if (HFStoreImageMode.Value == "Edit")
            {
                ObjMedRepos.DeleteCompWork(Convert.ToInt64(hfmediaid.Value));
            }
            
            if (cbCompanyforassignimage.SelectedIndex == -1)
            {
                MediaCompWork objmediaC = new MediaCompWork();
                objmediaC.MediaId = ObjMediaFile.MediaFileId;
                objmediaC.CompWorkId = 0;
                objmediaC.Type = "Company";
                ObjMedRepos.Insert(objmediaC);
                ObjMedRepos.SubmitChanges();
            }
            else
            {
                for (int j = 0; j < cbCompanyforassignimage.Items.Count; j++)
                {

                    if (cbCompanyforassignimage.Items[j].Selected == true)
                    {
                        MediaCompWork objmediaC = new MediaCompWork();
                        objmediaC.CompWorkId = Convert.ToInt64(cbCompanyforassignimage.Items[j].Value);
                        objmediaC.Type = "Company";
                        objmediaC.MediaId = ObjMediaFile.MediaFileId;
                        ObjMedRepos.Insert(objmediaC);
                        ObjMedRepos.SubmitChanges();
                    }
                }
            }
            if (CBWorkgroupForAssignImage.SelectedIndex == -1)
            {
                MediaCompWork objmediaC = new MediaCompWork();
                objmediaC.MediaId = ObjMediaFile.MediaFileId;
                objmediaC.CompWorkId = 0;
                objmediaC.Type = "Workgroup";
                ObjMedRepos.Insert(objmediaC);
                ObjMedRepos.SubmitChanges();
            }
            else
            {
                for (int j = 0; j < CBWorkgroupForAssignImage.Items.Count; j++)
                {

                    if (CBWorkgroupForAssignImage.Items[j].Selected == true)
                    {
                        MediaCompWork objmediaW = new MediaCompWork();
                        objmediaW.CompWorkId = Convert.ToInt64(CBWorkgroupForAssignImage.Items[j].Value);
                        objmediaW.Type = "Workgroup";
                        objmediaW.MediaId = ObjMediaFile.MediaFileId;
                        ObjMedRepos.Insert(objmediaW);
                        ObjMedRepos.SubmitChanges();
                    }
                }
            }
            ObjMedRepos.SubmitChanges();
            ///
            BindMediaFiles();
            BindDropDownValues();
            
            ShowAlertMessage("Record has been updated successfully");
            
            txtMasterStyleName.Text = "";
            ddlFolderForAssignImage.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void CbModuleForDoc_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetMultiSelectDropDown();
        txtPlaceForDoc.Text = "- Place -";
        txtModuleForDoc.Text = "- Module -";
        lidocsub.Visible = true;
        string strModuleids = "";
        for (int i = 0; i < CbModuleForDoc.Items.Count; i++)
        {
            if (CbModuleForDoc.Items[i].Selected == true)
            {
                txtModuleForDoc.Text = "- Selected -";
                if (i == 0)
                    strModuleids = CbModuleForDoc.Items[i].Value;
                else
                    strModuleids = strModuleids + "," + CbModuleForDoc.Items[i].Value;
            }

        }
        MediaRepository ObjMedia = new MediaRepository();
        List<GetMediaPlacesResult> objplaces = new List<GetMediaPlacesResult>();
        objplaces = ObjMedia.GetMediaPlaces("Document Link", strModuleids);
        DataTable dtplaces = Common.ListToDataTable(objplaces);
        this.cbPlacefordoc.DataSource = objplaces;
        this.cbPlacefordoc.DataTextField = "ModuleName";
        this.cbPlacefordoc.DataValueField = "id";
        this.cbPlacefordoc.DataBind();
        for (int i = 0; i < cbPlacefordoc.Items.Count; i++)
        {
            cbPlacefordoc.Items[i].Attributes.Add("onclick", "ValidationForMultiSelection(this,'" + txtPlaceForDoc.ClientID + "','" + cbPlacefordoc.ClientID + "','- Place -')");
        }
        OpenPopup("add-media-block", "employess-content");

    }


    protected void CbModuleForHelp_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetMultiSelectDropDown();
        txtPlaceForHelp.Text = "- Place -";
        txtModuleForHelp.Text = "- Module -";
        liassetsub.Visible = true;
        string strModuleids = "";
        for (int i = 0; i < CbModuleForHelp.Items.Count; i++)
        {
            if (CbModuleForHelp.Items[i].Selected == true)
            {
                txtModuleForHelp.Text = "- Selected -";
                if (i == 0)
                    strModuleids = CbModuleForHelp.Items[i].Value;
                else
                    strModuleids = strModuleids + "," + CbModuleForHelp.Items[i].Value;
            }

        }
        MediaRepository ObjMedia = new MediaRepository();
        List<GetMediaPlacesResult> objplaces = new List<GetMediaPlacesResult>();
        objplaces = ObjMedia.GetMediaPlaces("Help Video", strModuleids);
        DataTable dtplaces = Common.ListToDataTable(objplaces);
        this.cbPlaceForHelp.DataSource = objplaces;
        this.cbPlaceForHelp.DataTextField = "ModuleName";
        this.cbPlaceForHelp.DataValueField = "id";
        this.cbPlaceForHelp.DataBind();

        for (int i = 0; i < cbPlaceForHelp.Items.Count; i++)
        {
            cbPlaceForHelp.Items[i].Attributes.Add("onclick", "ValidationForMultiSelection(this,'" + txtPlaceForHelp.ClientID + "','" + cbPlaceForHelp.ClientID + "','- Place -')");
        }


        OpenPopup("add-media-block", "employess-content");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) && IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            {
                Response.Redirect("unauthorised.aspx");
            }

            LookupRepository objLookupRepo = new LookupRepository();
            List<INC_Lookup> lstStatuses = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Status).OrderBy(le => le.sLookupName).ToList();
            this.ActiveID = lstStatuses.FirstOrDefault(le => le.sLookupName == "Active").iLookupID;
            this.InActiveID = lstStatuses.FirstOrDefault(le => le.sLookupName == "InActive").iLookupID;
            // Top - Link 
            //AtagHelpVid.Attributes.Add()
            NewFolderId = "0";
            Session["FileType"] = "Media";
            if (FolderId > 0)
            {
                btnBack.Visible = true;
                divshowfoldername.Visible = true;
                MediaRepository objmedia = new MediaRepository();

                MediaFolder objFolder = objmedia.GetFolderByID(FolderId);
                if (objFolder != null)
                {
                    ltShowFolderName.Text = "Files of " + objFolder.FolderName;
                    objFolder.View = objFolder.View == null ? 1 : objFolder.View + 1;
                    objmedia.SubmitChanges();
                }
            }
            else
            {
                btnBack.Visible = false;
                divshowfoldername.Visible = false;
            }
            BindMediaFiles();
            BindDropDownValues();
            //ResetAssignedMediaImage();
        }
    }


    protected void btnAssignedMedia_Click(object sender, EventArgs e)
    {
        lbAssignedMedia_Click(sender, e);
    }

    protected void btnMove_Click(object sender, EventArgs e)
    {
        MediaRepository ObjMedia = new MediaRepository();
        MediaFolder ObjSourceFolder = new MediaFolder();
        MediaFolder objDestinationFolder = new MediaFolder();

        ObjSourceFolder = ObjMedia.GetFolderByID(Convert.ToInt64(ddlMoveCurrentFolder.SelectedValue));
        objDestinationFolder = ObjMedia.GetFolderByID(Convert.ToInt64(ddlFolderToMove.SelectedValue));
        //if (txtMoveCurrentFolderPassword.Text != ObjSourceFolder.Password)
        //{
        //    ShowAlertMessage("Password for the source folder is incorrect.");
        //    OpenPopup("move-file", "employess-content");
        //    return;

        //}

        //if (txtMoveFolderPassword.Text != objDestinationFolder.Password)
        //{
        //    ShowAlertMessage("Password for destination folder is incorrect.");
        //    OpenPopup("move-file", "employess-content");
        //    return;

        //}

        MediaFile objFile = new MediaFile();
        objFile = ObjMedia.GetFileById(Convert.ToInt64(ddlMoveFile.SelectedValue));
        if (objFile != null)
        {
            objFile.MediaFolderId = Convert.ToInt64(ddlFolderToMove.SelectedValue);
            ObjMedia.SubmitChanges();
            ShowAlertMessage("File has been moved successfully.");

        }
        else
        {
            ShowAlertMessage("File Does Not Exists.");
            OpenPopup("add-media-block", "employess-content");

            return;
        }
    }

    protected void btnMoveImage_Click(object sender, EventArgs e)
    {
        MediaRepository ObjMedia = new MediaRepository();
        MediaFile objFile = new MediaFile();
        objFile = ObjMedia.GetFileById(Convert.ToInt64(ddlMoveImageFile.SelectedValue));
        if (objFile != null)
        {
            objFile.MediaFolderId = Convert.ToInt64(ddlFolderToMoveImage.SelectedValue);
            ObjMedia.SubmitChanges();
            ShowAlertMessage("File has been moved successfully.");

        }
        else
        {
            ShowAlertMessage("File Does Not Exists.");
            OpenPopup("add-media-block", "employess-content");
            return;
        }
    }


    protected void btnGo_Click(object sender, EventArgs e)
    {
        //string querystr = ""; 
        //querystr = querystr + "?companyid=" + ddlSearchcomp.SelectedValue.ToString();
        //querystr = querystr + "&workgroupid=" + ddlSearchWorkgroup.SelectedValue.ToString();
        //querystr = querystr + "&keyword=" + txtSearch.Text;

        Session["MediaCompanyid"] = ddlSearchcomp.SelectedValue.ToString();
        Session["MediaWorkGroupid"] = ddlSearchWorkgroup.SelectedValue.ToString();
        Session["MediaKeyword"] = txtSearch.Text;
        Response.Redirect("MediaSearch.aspx");

    }

    protected void ddlPlacement_SelectedIndexChanged(object sender, EventArgs e)
    {
        //rfvCompany.Visible = true;
        //rfvWorkGroup.Visible = true;

        lidocmain.Visible = false;

        lidocsub.Visible = false;
        liassetmain.Visible = false;
        liassetsub.Visible = false;
        liloginpopup.Visible = false;
        Parentid = 0;
        subid = 0;
        //ddldocmain.SelectedIndex = 0;
        //ddldocsub.SelectedIndex = 0;
        // ddlhelpmain.SelectedIndex = 0;
        // ddlVideoSub.SelectedIndex = 0;
        //  ddlLoginPopups.SelectedIndex = 0;

        txtPlaceForDoc.Text = "- Place -";
        txtModuleForDoc.Text = "- Module -";
        txtPlaceForHelp.Text = "- Place -";
        txtModuleForDoc.Text = "- Module -";
        txtPlaceforLogin.Text = "- Place -";

        ltThumbImage.Text = "";
        if (ddlPlacement.SelectedItem.Text == "Document Link")
        {
            lidocmain.Visible = true;
            lidocsub.Visible = true;
            if (cbPlacefordoc.SelectedIndex != -1)
            {
                txtPlaceForDoc.Text = "- Selected -";
            }
            if (CbModuleForDoc.SelectedIndex != -1)
            {
                txtModuleForDoc.Text = "- Selected -";
            }
        }
        if (ddlPlacement.SelectedItem.Text == "Help Video")
        {
            liassetmain.Visible = true;
            liassetsub.Visible = true;

            if (cbPlaceForHelp.SelectedIndex != -1)
            {
                txtPlaceForHelp.Text = "- Selected -";
            }
            if (CbModuleForHelp.SelectedIndex != -1)
            {
                txtModuleForHelp.Text = "- Selected -";
            }

        }
        if (ddlPlacement.SelectedItem.Text == "Login Pop-up")
        {
            liloginpopup.Visible = true;
            for (int i = 0; i < cbPlaceForLogin.Items.Count; i++)
            {
                cbPlaceForLogin.Items[i].Attributes.Add("onclick", "ValidationForMultiSelection(this,'" + txtPlaceforLogin.ClientID + "','" + cbPlaceForLogin.ClientID + "','- Place -')");
            }

            if (cbPlaceForLogin.SelectedIndex != -1)
            {
                txtPlaceforLogin.Text = "- Selected -";
            }
            //rfvCompany.Visible = false;
            //rfvWorkGroup.Visible = false;
        }
        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowPopUp('add-media-block','employess-content');", true);
        BindEventWithMultiDropDown();
        OpenPopup("add-media-block", "employess-content");


    }

    protected void ddlCommonDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (mvAddNewAsset.ActiveViewIndex == 3)
        //    BindSpecsFields();
        try
        {
            CustomDropDown ddlCommonDropDown = (CustomDropDown)sender;
            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            if (cph != null)
            {
                HtmlGenericControl Parent_span = (HtmlGenericControl)cph.FindControl(ddlCommonDropDown.Parent.ID);
                if (Parent_span != null && Parent_span.Attributes["class"].ToLower().Contains("popup"))
                    ShowPopUp(Parent_span);

                if (Parent_span != null && !ddlCommonDropDown.IsEditing && ddlCommonDropDown.SelectedValue == "-1")
                {
                    Parent_span.Attributes.Remove("class");
                    Parent_span.Attributes.Add("class", "popup");
                }
                //Code for this Page Only - Static Condition
                // rfvCompany.Visible = true;
                //rfvWorkGroup.Visible = true;

                if (ddlCommonDropDown.Module == "mediafolder")
                {
                    if (ddlCommonDropDown.SelectedValue == "-1")
                        FolderPassword.Visible = true;
                    else
                        FolderPassword.Visible = false;
                    NewFolderId = ddlCommonDropDown.SelectedValue;
                }
                MediaRepository objmedia = new MediaRepository();
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlLookUpDropDown_SaveNewOptionAttempted(object sender, EventArgs e)
    {

        try
        {
            CustomDropDown ddlLookUpDropDown = (CustomDropDown)sender;
            MediaRepository objMediaRepos = new MediaRepository();
            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            if (cph != null)
            {
                HtmlGenericControl Parent_span = (HtmlGenericControl)cph.FindControl(ddlLookUpDropDown.Parent.ID);
                if (Parent_span != null)
                {
                    Parent_span.Attributes.Add("class", ddlLookUpDropDown.ParentSpanClassToRemove);
                    if (Parent_span.Attributes["class"].ToLower().Contains("popup"))
                        ShowPopUp(Parent_span);
                }
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ScrollToTag('" + ddlLookUpDropDown.Parent.ClientID + "',false);", true);
            }
            if (ddlLookUpDropDown.Text.Trim() != "")
            {
                var objList = objMediaRepos.InsertAndGetAllMediaDropDown(ddlLookUpDropDown.GroupName, ddlLookUpDropDown.Text, ddlLookUpDropDown.Module.ToLower(), IncentexGlobal.CurrentMember.UserInfoID, Parentid);
                BindCustomDropDown(ddlLookUpDropDown, objList, "DataTextField", "DataValueField", "- Select -", "-1");
                if (objList.Take(1).FirstOrDefault().InsertTransactionStatus != "Already Exists")
                    ddlLookUpDropDown.Items.FindByText(ddlLookUpDropDown.Text).Selected = true;
                else
                    return;



                if (ddlLookUpDropDown.Module == "mediafolder")
                {
                    NewFolder = ddlLookUpDropDown.Text;
                    NewFolderId = ddlLookUpDropDown.SelectedValue;
                }
            }
            else
            {
                FolderPassword.Visible = false;
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {

                if (Session["MediaFileType"] == null || Session["MediaFileName"] == null)
                {
                    ShowAlertMessage("Session has been expired please try again.");
                    OpenPopup("add-file-block", "emp-content");
                    return;
                }
                if (Session["MediaFileType"] != "other" && Session["MediaFileType"] != null)
                {
                    MediaRepository ObjMedRepository = new MediaRepository();
                    MediaFile ObjMediaFile = new MediaFile();


                    if (ObjMedRepository.IsMediaFileExist(txtNameFile.Text) == false)
                    {

                        ObjMediaFile.Name = txtNameFile.Text;
                        ObjMediaFile.OriginalFileName = Convert.ToString(Session["MediaFileName"]);
                        //if (System.IO.File.Exists(Server.MapPath("~/NewDesign/DocmentStorageCenter/") + Convert.ToString(Session["MediaFileName"])))
                        //   getresizeimage(Server.MapPath("~/NewDesign/DocmentStorageCenter/"), Convert.ToString(Session["MediaFileName"]), 427, 570);

                        ObjMediaFile.SearchTags = txtSearchTag.Text;
                        ObjMediaFile.CreatedId = IncentexGlobal.CurrentMember.UserInfoID;
                        ObjMediaFile.View = 0;
                        //ObjMediaFile.IsStoreImage = false;
                        ObjMediaFile.FileType = Convert.ToString(Session["MediaFileType"]);
                        ObjMediaFile.CreatedDate = System.DateTime.Now.Date;
                        ObjMediaFile.Isdeleted = false;

                        if (Convert.ToInt64(ddlFoldeName.SelectedValue) > 0)
                            ObjMediaFile.MediaFolderId = Convert.ToInt64(ddlFoldeName.SelectedValue);

                        ObjMedRepository.Insert(ObjMediaFile);
                        ObjMedRepository.SubmitChanges();
                        if (string.IsNullOrEmpty(txtPassword.Text) == false && FolderPassword.Visible == true)
                        {
                            MediaFolder objfold = ObjMedRepository.GetFolderNameByName(NewFolder);
                            if (objfold != null)
                            {
                                objfold.Password = txtPassword.Text;
                                objfold.Isdeleted = false;

                                ObjMedRepository.SubmitChanges();
                            }
                        }
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "GeneralAlertMsg('Record has been updated successfully');", true);
                        txtNameFile.Text = "";
                        txtPassword.Text = "";
                        txtSearchTag.Text = "";
                        NewFolderId = "0";
                        BindMediaFiles();
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "GeneralAlertMsg('File name already exist.');", true);
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "$('#add-file-block').css('top', '0').show();$('.fade-layer').show();popupCenter();", true);
                    }
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "GeneralAlertMsg('Invalid file type. Only pdf,docx,doc,mpeg,mpg,mov,mp4, 3gp,flv,avi,wmv,jpg,jpeg,png,bmp files are allowed');", true);
                }
                txtNameFile.Text = "";
                txtSearchTag.Text = "";
                ddlFoldeName.SelectedIndex = 0;
                FolderPassword.Visible = false;

            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "$('#add-file-block').css('top', '0').show();$('.fade-layer').show();popupCenter();", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }


    protected void lbAssignedMedia_Click(object sender, EventArgs e)
    {
        try
        {
            MediaRepository objMedRepo = new MediaRepository();

            if (Convert.ToInt32(ddlMediaFolder.SelectedValue) > 0)
            {
                MediaFolder FolderObj = new MediaFolder();
                FolderObj = objMedRepo.GetFolderByID(Convert.ToInt32(ddlMediaFolder.SelectedValue));
                if (FolderObj.Password != txtMedoaPassword.Text && !string.IsNullOrEmpty(FolderObj.Password))
                {
                    ShowAlertMessage("Can not update. Folder password is incorrect");
                    OpenPopup("add-media-block", "employess-content");
                    ResetMultiSelectDropDown();

                    return;
                }
            }
            if (ValidateAssignMedia() == true)
            {

                MediaAssigned objMediaAssn = objMedRepo.Getmediaassignedbyid(Convert.ToInt64(hfmediaid.Value));
                if (objMediaAssn == null)
                {
                    objMediaAssn = new MediaAssigned();
                    objMediaAssn.Status = this.ActiveID;

                }
                //else
                objMediaAssn.IsPublished = false;

                objMediaAssn.MediaFileId = Convert.ToInt64(hfmediaid.Value);
                objMediaAssn.PlacementId = Convert.ToInt64(ddlPlacement.SelectedValue);
                objMediaAssn.SearchTags = txtMediaSearchTag.Text;



                objMediaAssn.PubStartDate = null;
                objMediaAssn.PubEndDate = null;

                if (txtPubStartDate.Text.Trim() != "")
                    objMediaAssn.PubStartDate = Convert.ToDateTime(txtPubStartDate.Text);

                if (txtPubEndDate.Text.Trim() != "")
                    objMediaAssn.PubEndDate = Convert.ToDateTime(txtPubEndDate.Text);

                if (hfMediaAssignedid.Value == "0") // to check add or  edit mode
                {
                    objMedRepo.Insert(objMediaAssn);
                }
                objMedRepo.SubmitChanges();

                objMedRepo.DeleteCompWork(Convert.ToInt64(hfmediaid.Value));
                objMedRepo.DeleteModuleAndPlaces(Convert.ToInt64(hfmediaid.Value));

                if (ddlPlacement.SelectedItem.Text.ToLower() == "document link")
                {
                    for (int i = 0; i < CbModuleForDoc.Items.Count; i++)
                    {
                        if (CbModuleForDoc.Items[i].Selected == true)
                        {
                            MediaModuleAllocation ObjModuleAllocate = new MediaModuleAllocation();
                            ObjModuleAllocate.MediaFileId = Convert.ToInt64(hfmediaid.Value);
                            ObjModuleAllocate.Moduleid = Convert.ToInt64(CbModuleForDoc.Items[i].Value);
                            ObjModuleAllocate.ModuleType = "Document Link";
                            ObjModuleAllocate.MediaAssignedid = objMediaAssn.MediaAssignedId;
                            objMedRepo.Insert(ObjModuleAllocate);
                            objMedRepo.SubmitChanges();

                        }
                    }


                    for (int i = 0; i < cbPlacefordoc.Items.Count; i++)
                    {
                        if (cbPlacefordoc.Items[i].Selected == true)
                        {
                            MediaPlaceAllocation ObjPlaceAllocate = new MediaPlaceAllocation();
                            ObjPlaceAllocate.MediaFileId = Convert.ToInt64(hfmediaid.Value);
                            ObjPlaceAllocate.PlaceId = Convert.ToInt64(cbPlacefordoc.Items[i].Value);
                            ObjPlaceAllocate.PlaceType = "Document Link";
                            ObjPlaceAllocate.MediaAssignedid = objMediaAssn.MediaAssignedId;
                            objMedRepo.Insert(ObjPlaceAllocate);
                            objMedRepo.SubmitChanges();

                        }
                    }
                }


                if (ddlPlacement.SelectedItem.Text.ToLower() == "help video")
                {
                    for (int i = 0; i < CbModuleForHelp.Items.Count; i++)
                    {
                        if (CbModuleForHelp.Items[i].Selected == true)
                        {
                            MediaModuleAllocation ObjModuleAllocate = new MediaModuleAllocation();
                            ObjModuleAllocate.MediaFileId = Convert.ToInt64(hfmediaid.Value);
                            ObjModuleAllocate.Moduleid = Convert.ToInt64(CbModuleForHelp.Items[i].Value);
                            ObjModuleAllocate.ModuleType = "Help Video";
                            ObjModuleAllocate.MediaAssignedid = objMediaAssn.MediaAssignedId;
                            objMedRepo.Insert(ObjModuleAllocate);
                            objMedRepo.SubmitChanges();

                        }
                    }

                    for (int i = 0; i < cbPlaceForHelp.Items.Count; i++)
                    {
                        if (cbPlaceForHelp.Items[i].Selected == true)
                        {
                            MediaPlaceAllocation ObjPlaceAllocate = new MediaPlaceAllocation();
                            ObjPlaceAllocate.MediaFileId = Convert.ToInt64(hfmediaid.Value);
                            ObjPlaceAllocate.PlaceId = Convert.ToInt64(cbPlaceForHelp.Items[i].Value);
                            ObjPlaceAllocate.PlaceType = "Help Video";
                            ObjPlaceAllocate.MediaAssignedid = objMediaAssn.MediaAssignedId;
                            objMedRepo.Insert(ObjPlaceAllocate);
                            objMedRepo.SubmitChanges();
                        }
                    }
                }

                if (ddlPlacement.SelectedItem.Text.ToLower() == "login pop-up")
                {
                    for (int i = 0; i < cbPlaceForLogin.Items.Count; i++)
                    {
                        if (cbPlaceForLogin.Items[i].Selected == true)
                        {
                            MediaPlaceAllocation ObjPlaceAllocate = new MediaPlaceAllocation();
                            ObjPlaceAllocate.MediaFileId = Convert.ToInt64(hfmediaid.Value);
                            ObjPlaceAllocate.PlaceId = Convert.ToInt64(cbPlaceForLogin.Items[i].Value);
                            ObjPlaceAllocate.PlaceType = "Login Pop-up";
                            ObjPlaceAllocate.MediaAssignedid = objMediaAssn.MediaAssignedId;
                            objMedRepo.Insert(ObjPlaceAllocate);
                            objMedRepo.SubmitChanges();

                        }
                    }
                    //objMediaAssn.MediaLoginpopupid = Convert.ToInt64(ddlLoginPopups.SelectedValue);
                    //SaveScreenFile(Convert.ToInt64(ddlLoginPopups.SelectedValue), "login pop-up");
                }

                //SaveScreenFile(ddlPlacement.SelectedItem.Text.ToLower());

                if (ddlMediaFolder.SelectedValue != "0")
                {
                    MediaFile objFile = objMedRepo.GetFileById(Convert.ToInt64(hfmediaid.Value));
                    objFile.MediaFolderId = Convert.ToInt64(ddlMediaFolder.SelectedValue);
                    objMedRepo.SubmitChanges();
                }



                if (cbCompany.SelectedIndex == -1)
                {
                    MediaCompWork objcomp = new MediaCompWork();
                    objcomp.MediaId = Convert.ToInt64(hfmediaid.Value);
                    objcomp.CompWorkId = Convert.ToInt64(0);
                    objcomp.Type = "Company";
                    objMedRepo.Insert(objcomp);
                    objMedRepo.SubmitChanges();
                }
                else
                {
                    for (int i = 0; i < cbCompany.Items.Count; i++)
                    {
                        if (cbCompany.Items[i].Selected == true)
                        {
                            MediaCompWork objcomp = new MediaCompWork();
                            objcomp.MediaId = Convert.ToInt64(hfmediaid.Value);
                            objcomp.CompWorkId = Convert.ToInt64(cbCompany.Items[i].Value);
                            objcomp.Type = "Company";
                            objMedRepo.Insert(objcomp);
                            objMedRepo.SubmitChanges();
                        }
                    }
                }
                if (cbWorkGroup.SelectedIndex == -1)
                {
                    MediaCompWork objwork = new MediaCompWork();
                    objwork.MediaId = Convert.ToInt64(hfmediaid.Value);
                    objwork.CompWorkId = Convert.ToInt64(0);
                    objwork.Type = "Workgroup";
                    objMedRepo.Insert(objwork);
                    objMedRepo.SubmitChanges();

                }
                else
                {

                    for (int i = 0; i < cbWorkGroup.Items.Count; i++)
                    {
                        if (cbWorkGroup.Items[i].Selected == true)
                        {
                            MediaCompWork objwork = new MediaCompWork();
                            objwork.MediaId = Convert.ToInt64(hfmediaid.Value);
                            objwork.CompWorkId = Convert.ToInt64(cbWorkGroup.Items[i].Value);
                            objwork.Type = "Workgroup";
                            objMedRepo.Insert(objwork);
                            objMedRepo.SubmitChanges();
                        }
                    }


                }

                //
                ShowAlertMessage("File/Video has been assigned successfully. You can publish it by clicking on publish.");
                BindMediaFiles();
            }
            else
            {
                OpenPopup("add-media-block", "employess-content");
                ResetMultiSelectDropDown();
            }

        }
        catch (Exception ex)
        {
        }
    }

    //public void SaveScreenFile(string type)
    //{
    //    if (FUScreenImage.HasFile)
    //    {
    //        MediaRepository objrepos = new MediaRepository();

    //        switch (type)
    //        {
    //            case "document link":
    //                MediadocLinkSub objdoclink = objrepos.GetLinkSubById(Convert.ToInt64(ddldocsub.SelectedValue));
    //                objdoclink.ThumbnailImage = Convert.ToInt64(ddldocsub.SelectedValue).ToString() + "_" + FUScreenImage.FileName;
    //                objrepos.SubmitChanges();
    //                FUScreenImage.SaveAs(Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/") + Convert.ToInt64(ddldocsub.SelectedValue).ToString() + "_" + FUScreenImage.FileName);
    //                getresizeimage(Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/"), (Convert.ToInt64(ddldocsub.SelectedValue).ToString() + "_" + FUScreenImage.FileName), 427, 570);
    //                break;
    //            case "help video":
    //                MediaHelpVidSub objHelpVid = objrepos.GetHelpSubById(Convert.ToInt64(ddlVideoSub.SelectedValue));
    //                objHelpVid.ThumbnailImage = Convert.ToInt64(ddlVideoSub.SelectedValue).ToString() + "_" + FUScreenImage.FileName;
    //                objrepos.SubmitChanges();
    //                FUScreenImage.SaveAs(Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/") + Convert.ToInt64(ddlVideoSub.SelectedValue) + "_" + FUScreenImage.FileName);
    //                getresizeimage(Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/"), (Convert.ToInt64(ddlVideoSub.SelectedValue).ToString() + "_" + FUScreenImage.FileName), 427, 570);
    //                break;
    //            case "login pop-up":
    //                MediaLoginpopup objPopup = objrepos.GetLoginPopupbyid(Convert.ToInt64(ddlLoginPopups.SelectedValue));
    //                objPopup.ThumbnailImage = Convert.ToInt64(ddlLoginPopups.SelectedValue).ToString() + "_" + FUScreenImage.FileName;
    //                objrepos.SubmitChanges();
    //                FUScreenImage.SaveAs(Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/") + Convert.ToInt64(ddlLoginPopups.SelectedValue).ToString() + "_" + FUScreenImage.FileName);
    //                getresizeimage(Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/"), (Convert.ToInt64(ddlLoginPopups.SelectedValue).ToString() + "_" + FUScreenImage.FileName), 427, 570);
    //                break;
    //        }
    //    }
    //}




    protected void btnRenameFileSave_Click(object sender, EventArgs e)
    {
        try
        {
            MediaRepository ObjMedia = new MediaRepository();
            if (hfRenfileType.Value == "folder")
            {
                if (hfRenamePassword.Value == txtRenamePassword.Text)
                {
                    if (ObjMedia.IsFileOrFolderDuplicate(txtRenameFileName.Text, "folder", Convert.ToInt64(hfRenfileid.Value)))
                    {
                        ShowAlertMessage("Folder name already exists");
                        return;
                    }
                    MediaFolder ObjFolder = ObjMedia.GetFolderByID(Convert.ToInt64(hfRenfileid.Value));
                    ObjFolder.FolderName = txtRenameFileName.Text;
                    ObjMedia.SubmitChanges();
                    BindDropDownValues();
                    ShowAlertMessage("Folder name has been updated successfully");
                }
                else
                {
                    ShowAlertMessage("Can not change folder name.Password is incorrect.");
                }
            }
            else if (hfRenfileType.Value == "file")
            {
                if (!string.IsNullOrEmpty(Request.QueryString["folderid"]))
                {
                    if (hfRenamePassword.Value != txtRenamePassword.Text)
                    {
                        ShowAlertMessage("Can not change file name.Password is incorrect.");
                        return;
                    }
                    if (ObjMedia.IsFileOrFolderDuplicate(txtRenameFileName.Text, "file", Convert.ToInt64(hfRenfileid.Value)))
                    {
                        ShowAlertMessage("File name already exists");
                        return;
                    }
                }
                MediaFile ObjFile = ObjMedia.GetFileById(Convert.ToInt64(hfRenfileid.Value));
                ObjFile.Name = txtRenameFileName.Text;
                ObjMedia.SubmitChanges();
                ShowAlertMessage("File name has been updated successfully.");
            }
            hfRenfileid.Value = "";
            hfRenfileType.Value = "";
            hfRenamePassword.Value = "";

            BindMediaFiles();
        }
        catch (Exception ex)
        {
            // ErrHandler.WriteError(ex);
        }
    }
    protected void btnRemoveSave_Click(object sender, EventArgs e)
    {
        try
        {



            string filetype = "";
            MediaRepository ObjMedia = new MediaRepository();
            MediaFolder ObjFolder = ObjMedia.GetFolderByID(Convert.ToInt64(hfremovefolderid.Value));
            if (txtremovePassword.Text == ObjFolder.Password || ObjFolder.Password == null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["folderid"]))
                {
                    filetype = "File";

                    MediaAssigned objAssigmed = ObjMedia.Getmediaassignedbyid(Convert.ToInt32(hfRemovefileid.Value));
                    ObjMedia.DeleteCompWork(Convert.ToInt32(hfRemovefileid.Value));
                    ObjMedia.DeleteModuleAndPlaces(Convert.ToInt64(hfRemovefileid.Value));
                    if (objAssigmed != null)
                    {
                        ObjMedia.Delete(objAssigmed);
                        ObjMedia.SubmitChanges();
                    }

                    MediaFile ObjFile = ObjMedia.GetFileById(Convert.ToInt32(hfRemovefileid.Value));

                    ObjMedia.Delete(ObjFile);
                    //ObjFile.Isdeleted = true;
                    ObjMedia.SubmitChanges();

                    if (System.IO.File.Exists(Server.MapPath("~/NewDesign/DocmentStorageCenter/") + ObjFile.OriginalFileName))
                        System.IO.File.Delete(Server.MapPath("~/NewDesign/DocmentStorageCenter/") + ObjFile.OriginalFileName);
                }
                else
                {
                    filetype = "Folder";
                    ObjMedia.Delete(ObjFolder);
                    //ObjFolder.Isdeleted = true;
                    ObjMedia.SubmitChanges();
                    var filelist = ObjMedia.GetFilesByfolderid(Convert.ToInt64(hfremovefolderid.Value));
                    foreach (var item in filelist)
                    {
                        MediaFile ObjFile = ObjMedia.GetFileById(item.MediaFileId);
                        //ObjFile.Isdeleted = true;
                        MediaAssigned objAssigmed = ObjMedia.Getmediaassignedbyid(item.MediaFileId);
                        ObjMedia.DeleteCompWork(Convert.ToInt32(ObjFile.MediaFileId));
                        ObjMedia.DeleteModuleAndPlaces(Convert.ToInt64(ObjFile.MediaFileId));
                        if (objAssigmed != null)
                        {
                            ObjMedia.Delete(objAssigmed);
                            ObjMedia.SubmitChanges();
                        }

                        ObjMedia.Delete(ObjFile);
                        ObjMedia.SubmitChanges();
                        if (System.IO.File.Exists(Server.MapPath("~/NewDesign/DocmentStorageCenter/") + ObjFile.OriginalFileName))
                            System.IO.File.Delete(Server.MapPath("~/NewDesign/DocmentStorageCenter/") + ObjFile.OriginalFileName);

                    }
                }
                ShowAlertMessage(filetype + " has been deleted successfully.");
                BindMediaFiles();

            }
            else
            {
                ShowAlertMessage("Can not remove " + hfRenfileType.Value + ". Password is incorrect.");
            }
        }
        catch (Exception ex)
        {
            // ErrHandler.WriteError(ex);
        }
    }
    protected void dlMediaFiles_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            MediaRepository ObjMedia = new MediaRepository();
            Int64 mediaid = Convert.ToInt64(e.CommandArgument);
            if (e.CommandName.ToString().Substring(0, 3) == "del")
            {
                if (e.CommandName.ToString() == "del-folder")
                {
                    hfremovefolderid.Value = mediaid.ToString();
                    OpenPopup("add-remove-block", "removefile-content");
                    hfRenfileType.Value = "folder";

                }
                else
                {
                    hfRenfileType.Value = "file";
                    if (!string.IsNullOrEmpty(Request.QueryString["folderid"]))
                    {
                        MediaFile ObjFile = ObjMedia.GetFileById(mediaid);
                        if (ObjFile.FileType.ToLower() == "image")
                        {
                            MediaAssigned objAssigmed = ObjMedia.Getmediaassignedbyid(mediaid);
                            ObjMedia.DeleteCompWork(mediaid);
                           // ObjMedia.DeleteModuleAndPlaces(mediaid);
                            if (objAssigmed != null)
                            {
                                ObjMedia.Delete(objAssigmed);
                                ObjMedia.SubmitChanges();
                            }

                            ObjMedia.Delete(ObjFile);
                            ObjMedia.SubmitChanges();
                            BindMediaFiles();
                        }
                        else
                        {
                            hfRemovefileid.Value = mediaid.ToString();
                            hfremovefolderid.Value = Request.QueryString["folderid"].ToString();
                            OpenPopup("add-remove-block", "removefile-content");
                        }


                    }
                    else
                    {
                        MediaFile ObjFile = ObjMedia.GetFileById(mediaid);
                        MediaAssigned objAssigmed = ObjMedia.Getmediaassignedbyid(mediaid);
                        ObjMedia.DeleteCompWork(mediaid);
                        ObjMedia.DeleteModuleAndPlaces(mediaid);
                        if (objAssigmed != null)
                        {
                            ObjMedia.Delete(objAssigmed);
                            ObjMedia.SubmitChanges();
                        }

                        ObjMedia.Delete(ObjFile);
                        ObjMedia.SubmitChanges();
                        BindMediaFiles();
                        if (System.IO.File.Exists(Server.MapPath("~/NewDesign/DocmentStorageCenter/") + ObjFile.OriginalFileName))
                            System.IO.File.Delete(Server.MapPath("~/NewDesign/DocmentStorageCenter/") + ObjFile.OriginalFileName);
                    }
                }
            }

            else if (e.CommandName.ToString().Substring(0, 3) == "ren")
            {
                hfRenfileid.Value = mediaid.ToString();
                if (e.CommandName.ToString() == "ren-folder")
                {
                    MediaFolder ObjFolder = ObjMedia.GetFolderByID(mediaid);
                    txtRenameFileName.Text = ObjFolder.FolderName;
                    ltRenameFile.Text = "Folder Name";
                    hfRenfileType.Value = "folder";
                    hfRenamePassword.Value = ObjFolder.Password == null ? "" : ObjFolder.Password;
                    liRenamePassord.Visible = true;
                    if (ObjFolder.FolderType.ToLower() == "system folder")
                        liRenamePassord.Visible = false;
                    //this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowPopUp('add-rename-block','renamefile-content');", true);
                }
                else
                {
                    MediaFile ObjFile = ObjMedia.GetFileById(mediaid);
                    txtRenameFileName.Text = ObjFile.Name;
                    ltRenameFile.Text = "File Name";
                    hfRenfileType.Value = "file";
                    liRenamePassord.Visible = false;
                    if (ObjFile.MediaFolderId != null)
                    {
                        if (ObjFile.MediaFolderId > 0)
                        {
                            liRenamePassord.Visible = true;

                            MediaFolder ObjFolder = ObjMedia.GetFolderByID(Convert.ToInt64(ObjFile.MediaFolderId));
                            hfRenamePassword.Value = ObjFolder.Password == null ? "" : ObjFolder.Password;
                        }
                    }

                }

                OpenPopup("add-rename-block", "renamefile-content");
            }

            else if (e.CommandName.ToString().Substring(0, 3) == "asg")
            {


                //ddlProductCategory.Enabled = false;

                MediaFile objmediafile = ObjMedia.GetFileById(mediaid);
                switch (e.CommandName.ToString())
                {
                    case "asg-video":
                        ResetAssignedMEdia();
                        hfmediaid.Value = mediaid.ToString();
                        FillAssignedMedia(Convert.ToInt64(hfmediaid.Value));

                        string videourl = System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"].ToString() + "DocmentStorageCenter/" + objmediafile.OriginalFileName;
                        ltmediaimage.Text = "<a  onclick='ShowVideo(\"" + videourl + "\")' href='#'  > <img src='../StaticContents/img/vidpopupimage.png' width='144px' height='144px' border='0' /></a>";
                        lbAssignedMedia.Text = "Assign Video";
                        btnAssignedMedia.Text = "Assign Video";
                        DateTime dt = Convert.ToDateTime(objmediafile.CreatedDate);
                        ltfilename.Text = objmediafile.Name;
                        ltmediapopupdate.Text = dt.ToString("MMM d, yyyy"); //dt.Date.Month.ToString() + " " + dt.Date.Day.ToString() + ", " + dt.Date.Year.ToString();
                        ltMediaPopupView.Text = objmediafile.View == null ? "0" : objmediafile.View.ToString();
                        hfMediaType.Value = objmediafile.FileType;

                        OpenPopup("add-media-block", "employess-content");
                        break;
                    case "asg-document":
                        ResetAssignedMEdia();
                        hfmediaid.Value = mediaid.ToString();
                        FillAssignedMedia(Convert.ToInt64(hfmediaid.Value));

                        ltmediaimage.Text = "<a target='_blank' href='../DocmentStorageCenter/" + objmediafile.OriginalFileName + "' title='" + objmediafile.Name + "'  > <img src='../StaticContents/img/dmspdf-img.jpg' width='144px' height='144px' border='0' /></a>";
                        lbAssignedMedia.Text = "Assign Document";
                        btnAssignedMedia.Text = "Assign Document";
                        dt = Convert.ToDateTime(objmediafile.CreatedDate);
                        ltfilename.Text = objmediafile.Name;
                        ltmediapopupdate.Text = dt.ToString("MMM d, yyyy"); //dt.Date.Month.ToString() + " " + dt.Date.Day.ToString() + ", " + dt.Date.Year.ToString();
                        ltMediaPopupView.Text = objmediafile.View == null ? "0" : objmediafile.View.ToString();
                        hfMediaType.Value = objmediafile.FileType;
                        OpenPopup("add-media-block", "employess-content");
                        break;
                    case "asg-pdf":
                        ResetAssignedMEdia();
                        hfmediaid.Value = mediaid.ToString();
                        FillAssignedMedia(Convert.ToInt64(hfmediaid.Value));

                        ltmediaimage.Text = "<a target='_blank' href='../DocmentStorageCenter/" + objmediafile.OriginalFileName + "' title='" + objmediafile.Name + "'  > <img src='../StaticContents/img/dmspdf-img.jpg' width='144px' height='144px' border='0' /></a>";
                        lbAssignedMedia.Text = "Assign Document";
                        btnAssignedMedia.Text = "Assign Document";
                        dt = Convert.ToDateTime(objmediafile.CreatedDate);
                        ltfilename.Text = objmediafile.Name;
                        ltmediapopupdate.Text = dt.ToString("MMM d, yyyy"); //dt.Date.Month.ToString() + " " + dt.Date.Day.ToString() + ", " + dt.Date.Year.ToString();
                        ltMediaPopupView.Text = objmediafile.View == null ? "0" : objmediafile.View.ToString();
                        hfMediaType.Value = objmediafile.FileType;
                        OpenPopup("add-media-block", "employess-content");
                        break;
                    case "asg-image":
                        //DivProductAssignPassword.Visible = false;
                        ResetAssignedMediaImage();
                        hfmediaid.Value = mediaid.ToString();
                        FillValueInAssignToStoreImagePopup(Convert.ToInt64(hfmediaid.Value));
                        ltMediaPopupViewforstore.Text = objmediafile.View == null ? "0" : objmediafile.View.ToString() + " Views";
                        dt = Convert.ToDateTime(objmediafile.CreatedDate);
                        ltmediapopupdateforstore.Text = dt.ToString("MMM d, yyyy");
                        OpenPopup("add-assigntostoreproduct", "employess-content");
                        imgProductImage.Src = "../DocmentStorageCenter/" + objmediafile.OriginalFileName;
                        HfStoreImage.Value = "../DocmentStorageCenter/" + objmediafile.OriginalFileName;
                        break;
                }
                //this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowPopUp('add-media-block','employess-content');", true);
            }
            else if (e.CommandName.ToString().Substring(0, 3) == "pub")
            {
                if (!string.IsNullOrEmpty(Request.QueryString["folderid"]))
                {
                    MediaFolder objFolder = new MediaFolder();
                    objFolder = ObjMedia.GetFolderByID(Convert.ToInt64(Request.QueryString["folderid"]));
                    hfPubPassword.Value = objFolder.Password == null ? "" : objFolder.Password;
                    ListItem NewItem = new ListItem(objFolder.FolderName, objFolder.MediaFolderId.ToString());
                    ddlPubFolder.Items.Add(NewItem);
                    ddlPubFolder.SelectedValue = objFolder.MediaFolderId.ToString();
                    ulPubfolder.Visible = true;
                    ddlPubFolder.Enabled = false;

                }
                else
                {
                    ulPubfolder.Visible = false;
                }



                lbPublishvideo.Enabled = true;
                lblPubInstruction.Text = "";
                MediaAssigned objassigned = new MediaAssigned();
                objassigned = ObjMedia.Getmediaassignedbyid(Convert.ToInt64(mediaid));


                if (objassigned == null)
                {
                    ShowAlertMessage("Can not publish file. File is not assigned.");
                    return;
                }
                else
                {
                    var placename = ObjMedia.GetPlacement().ToList().Where(x => x.MediaPlaceMentId == objassigned.PlacementId).SingleOrDefault();
                    var result = ObjMedia.GetMediaAssignedDetail(mediaid, placename.PlacementName).ToList();
                    //pubvideo
                    foreach (var item in result)
                    {
                        if (item.pubstartdate == null || item.pubenddate == null)
                        {
                            ShowAlertMessage("Date range is not specified for this media file.");
                            return;
                        }
                        if (item.IsPublished == true)
                        {
                            lbPublishvideo.Enabled = false;
                            lblPubInstruction.Text = "[Note:The media file is already published.]";
                        }

                        HfPubMediaid.Value = mediaid.ToString();
                        HFPubType.Value = e.CommandName.ToString();
                        ltPublishedFileName.Text = item.name;
                        ltpublishedCompany.Text = item.Company;
                        ltPublishWorkgroup.Text = item.Workgroup;
                        ltPublishdates.Text = item.startdate + " to " + item.enddate;
                        ltPublishPlacement.Text = item.PlacementName;
                        ltPublishlocation.Text = item.Module;
                        ltPublishPlace.Text = item.Place;
                        //ltPublishpage.Text = item.location2;

                        if (string.IsNullOrEmpty(item.Vimeourl))
                        {
                            divVimeoUrl.Visible = false;
                        }
                        else
                        {
                            ltVimeoUrl.Text = item.Vimeourl;
                            divVimeoUrl.Visible = true;
                        }
                        if (e.CommandName.ToString() == "pub-video")
                            lbPublishvideo.Text = "Publish to Vimeo";
                        else
                            lbPublishvideo.Text = "Publish Document";

                        if (e.CommandName.ToString() == "pub-video")
                        {
                            divvidimage.Visible = true;
                            lbPlayPubVideo.Visible = true;
                            divShowPubVideo.Visible = false;
                            string videourl = System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"].ToString() + "DocmentStorageCenter/" + item.originalfilename;
                            hfPubVideo.Value = videourl;
                            lbContWithoutPublish.Visible = true;
                        }
                        else
                        {
                            divvidimage.Visible = false;
                            lbPlayPubVideo.Visible = false;
                            divShowPubVideo.Visible = false;
                            divvidimage.Visible = false;
                            ltPubvideotag.Text = "";
                            lbContWithoutPublish.Visible = false;
                        }
                    }
                    OpenPopup("add-publish-block", "employess-content");
                }
            }

            else if (e.CommandName.ToString().Substring(0, 3) == "mov")
            {
                //MediaRepository ObjMedia = new MediaRepository();

                MediaFolder objFolder = new MediaFolder();
                objFolder = ObjMedia.GetFolderByID(mediaid);
                if (objFolder.FolderType.ToLower() == "folder")
                {
                    ListItem NewItem = new ListItem(objFolder.FolderName, objFolder.MediaFolderId.ToString());
                    ddlMoveCurrentFolder.Items.Add(NewItem);
                    ddlMoveCurrentFolder.SelectedValue = objFolder.MediaFolderId.ToString();
                    List<MediaFolder> objList = new List<MediaFolder>();
                    objList = ObjMedia.GetAllFolder().Where(x => x.MediaFolderId != objFolder.MediaFolderId).ToList();
                    List<MediaFile> ObjFileList = ObjMedia.GetFilesByfolderid(objFolder.MediaFolderId).OrderBy(x => x.Name).ToList();

                    BindDropDown(ddlFolderToMove, objList, "FolderName", "MediaFolderId", "Folder", "0");
                    BindDropDown(ddlMoveFile, ObjFileList.Where(x => x.CreatedId == IncentexGlobal.CurrentMember.UserInfoID), "Name", "MediaFileId", "File", "0");
                    OpenPopup("move-file", "employess-content");
                }
                else if (objFolder.FolderType.ToLower() == "system folder")
                {

                    ListItem NewItem = new ListItem(objFolder.FolderName, objFolder.MediaFolderId.ToString());
                    ddlMoveCurrentImageFolder.Items.Add(NewItem);
                    ddlMoveCurrentImageFolder.SelectedValue = objFolder.MediaFolderId.ToString();
                    List<MediaFolder> objList = new List<MediaFolder>();
                    objList = ObjMedia.GetSystemFolder().Where(x => x.MediaFolderId != objFolder.MediaFolderId).ToList();
                    List<MediaFile> ObjFileList = ObjMedia.GetFilesByfolderid(objFolder.MediaFolderId).OrderBy(x => x.Name).ToList();

                    BindDropDown(ddlFolderToMoveImage, objList, "FolderName", "MediaFolderId", "Folder", "0");
                    BindDropDown(ddlMoveImageFile, ObjFileList.Where(x => x.CreatedId == IncentexGlobal.CurrentMember.UserInfoID), "Name", "MediaFileId", "File", "0");
                    OpenPopup("move-image", "employess-content");
                }

            }
        }
        catch (Exception ex)
        {
            // ErrHandler.WriteError(ex);
        }
    }

    protected void dlMediaFiles_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Button btnRemove = (Button)e.Item.FindControl("btnRemove");
            if (btnRemove != null)
            {
                btnRemove.Attributes.Add("onclick", "return GeneralConfirmationMsgForDelete('Are you sure you want to delete the item?','" + btnRemove.ClientID + "',event)");
            }
        }
    }



    #endregion

    #region Methods
    public Int64 UploadToVimeo(Int64 mediaid)
    {
        try
        {

            MediaRepository objrepos = new MediaRepository();
            MediaFile objFile = objrepos.GetFileById(mediaid);
            if (objFile != null)
            {
                string strpath = Server.MapPath("~/NewDesign/DocmentStorageCenter/") + objFile.OriginalFileName;
                VideoUploadTicket videoTicket = VimeoNET.VimeoAPI.UploadVideo(strpath, objFile.Name, objFile.Name, objFile.SearchTags, false);
                if (videoTicket != null && videoTicket.VideoId != string.Empty)
                {
                    return Convert.ToInt64(videoTicket.VideoId);
                }
            }
            return -1;
        }
        catch (Exception ex)
        {
            return -1;
        }
    }


    public void getresizeimage(string path, string fileName, int height, int width)
    {
        //fileName = "indexpage.png";
        int NewWidth = height;
        int NewHeight = width;
        // FileStream fs = File.Open(, FileMode.Open);

        StreamReader sr = new StreamReader(path + fileName);
        System.Drawing.Bitmap bmpOut = new System.Drawing.Bitmap(NewWidth, NewHeight);
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmpOut);
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.FillRectangle(System.Drawing.Brushes.White, 0, 0, NewWidth, NewHeight);
        g.DrawImage(new System.Drawing.Bitmap(sr.BaseStream), 0, 0, NewWidth, NewHeight);
        MemoryStream stream = new MemoryStream();
        bmpOut.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        String saveImagePath = path + width.ToString() + "x" + height.ToString() + "_" + fileName.Substring(0, fileName.LastIndexOf('.')) + ".jpg";
        bmpOut.Save(saveImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        g.Dispose();
        bmpOut.Dispose();
        sr.Dispose();
    }


    public void ResizeImage(string path, string newfilepath, string fileName, string newfilename, int width, int height)
    {
        //fileName = "indexpage.png";
        int NewWidth = width;
        int NewHeight = height;
        // FileStream fs = File.Open(, FileMode.Open);

        StreamReader sr = new StreamReader(path + fileName);
        System.Drawing.Bitmap bmpOut = new System.Drawing.Bitmap(NewWidth, NewHeight);
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmpOut);
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.FillRectangle(System.Drawing.Brushes.White, 0, 0, NewWidth, NewHeight);
        g.DrawImage(new System.Drawing.Bitmap(sr.BaseStream), 0, 0, NewWidth, NewHeight);
        MemoryStream stream = new MemoryStream();
        bmpOut.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        String saveImagePath = newfilepath + newfilename;
        bmpOut.Save(saveImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        g.Dispose();
        bmpOut.Dispose();
        sr.Dispose();
    }



    protected void TextValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = ValidateAssignMedia();
        OpenPopup("add-media-block", "employess-content");
    }

    public bool ValidateAssignMedia()
    {
        bool result = true;
        bool cbcomp = false;
        bool cbwork = false;


        if (hfMediaType.Value == "video" && ddlPlacement.SelectedItem.Text == "Document Link")
        {
            ShowAlertMessage("Video can not be assigned for this place");
            return false;
        }

        if (hfMediaType.Value != "video" && ddlPlacement.SelectedItem.Text != "Document Link")
        {
            ShowAlertMessage("Document can not be assigned for this place");
            return false;
        }
        DateTime dt1;
        DateTime dt2;

        if (DateTime.TryParse(txtPubStartDate.Text, out dt1) && DateTime.TryParse(txtPubEndDate.Text, out dt2))
        {
            if (dt1 > dt2)
            {
                ShowAlertMessage("Start Date Should be lesser or equal to End Date");
                //OpenPopup("add-media-block", "employess-content");
                return false;
            }
        }
        return result;
    }

    public void FillAssignedMedia(Int64 mediaid)
    {

        // rfvCompany.Visible = true;
        //rfvWorkGroup.Visible = true;

        MediaRepository ObjMediaRepo = new MediaRepository();
        MediaAssigned objassigned = new MediaAssigned();
        objassigned = ObjMediaRepo.Getmediaassignedbyid(Convert.ToInt64(hfmediaid.Value));
        if (objassigned != null)
        {
            hfMediaAssignedid.Value = objassigned.MediaFileId.ToString();
            SetCheckBoxListvalue(ref cbCompany, ObjMediaRepo.GetMediaCompWorkBymediaid(Convert.ToInt64(hfmediaid.Value), "Company"), "Company");
            SetCheckBoxListvalue(ref cbWorkGroup, ObjMediaRepo.GetMediaCompWorkBymediaid(Convert.ToInt64(hfmediaid.Value), "Workgroup"), "Workgroup");
            txtMediaSearchTag.Text = objassigned.SearchTags;
            ddlPlacement.SelectedValue = objassigned.PlacementId.ToString();
            txtPubStartDate.Text = objassigned.PubStartDate == null ? "" : Convert.ToDateTime(objassigned.PubStartDate).ToString("MM/dd/yyyy");
            txtPubEndDate.Text = objassigned.PubEndDate == null ? "" : Convert.ToDateTime(objassigned.PubEndDate).ToString("MM/dd/yyyy");
            if (ddlPlacement.SelectedItem.Text.ToLower() == "document link")
            {
                lidocmain.Visible = true;
                lidocsub.Visible = true;

                SetCheckBoxListvalue(ref CbModuleForDoc, ObjMediaRepo.GetMediaModules(Convert.ToInt64(hfmediaid.Value), "Document Link"), "Document Link - Module");

                string strModuleids = "";
                for (int i = 0; i < CbModuleForDoc.Items.Count; i++)
                {
                    if (CbModuleForDoc.Items[i].Selected == true)
                    {
                        if (i == 0)
                            strModuleids = CbModuleForDoc.Items[i].Value;
                        else
                            strModuleids = strModuleids + "," + CbModuleForDoc.Items[i].Value;
                    }

                }
                MediaRepository ObjMedia = new MediaRepository();
                List<GetMediaPlacesResult> objplaces = new List<GetMediaPlacesResult>();
                objplaces = ObjMedia.GetMediaPlaces("Document Link", strModuleids);
                DataTable dtplaces = Common.ListToDataTable(objplaces);
                this.cbPlacefordoc.DataSource = objplaces;
                this.cbPlacefordoc.DataTextField = "ModuleName";
                this.cbPlacefordoc.DataValueField = "id";
                this.cbPlacefordoc.DataBind();

                for (int i = 0; i < cbPlacefordoc.Items.Count; i++)
                {
                    cbPlacefordoc.Items[i].Attributes.Add("onclick", "ValidationForMultiSelection(this,'" + txtPlaceForDoc.ClientID + "','" + cbPlacefordoc.ClientID + "','- Place -')");
                }
                SetCheckBoxListvalue(ref cbPlacefordoc, ObjMediaRepo.GetMediaPlaces(Convert.ToInt64(hfmediaid.Value), "Document Link"), "Document Link - Place");


            }
            if (ddlPlacement.SelectedItem.Text.ToLower() == "help video")
            {
                liassetsub.Visible = true;
                liassetmain.Visible = true;

                SetCheckBoxListvalue(ref CbModuleForHelp, ObjMediaRepo.GetMediaModules(Convert.ToInt64(hfmediaid.Value), "Help Video"), "Help Video - Module");

                string strModuleids = "";
                for (int i = 0; i < CbModuleForHelp.Items.Count; i++)
                {
                    if (CbModuleForHelp.Items[i].Selected == true)
                    {
                        if (i == 0)
                            strModuleids = CbModuleForHelp.Items[i].Value;
                        else
                            strModuleids = strModuleids + "," + CbModuleForHelp.Items[i].Value;
                    }

                }
                MediaRepository ObjMedia = new MediaRepository();
                List<GetMediaPlacesResult> objplaces = new List<GetMediaPlacesResult>();
                objplaces = ObjMedia.GetMediaPlaces("Help Video", strModuleids);
                DataTable dtplaces = Common.ListToDataTable(objplaces);
                this.cbPlaceForHelp.DataSource = objplaces;
                this.cbPlaceForHelp.DataTextField = "ModuleName";
                this.cbPlaceForHelp.DataValueField = "id";
                this.cbPlaceForHelp.DataBind();

                for (int i = 0; i < cbPlaceForHelp.Items.Count; i++)
                {
                    cbPlaceForHelp.Items[i].Attributes.Add("onclick", "ValidationForMultiSelection(this,'" + txtPlaceForHelp.ClientID + "','" + cbPlaceForHelp.ClientID + "','- Place -')");
                }

                SetCheckBoxListvalue(ref cbPlaceForHelp, ObjMediaRepo.GetMediaPlaces(Convert.ToInt64(hfmediaid.Value), "Help Video"), "Help Video - Place");
            }
            // rfvWorkGroup.Visible = true;
            //rfvCompany.Visible = true;
            if (ddlPlacement.SelectedItem.Text.ToLower() == "login pop-up")
            {
                //rfvWorkGroup.Visible = false;
                //rfvCompany.Visible = false;
                liloginpopup.Visible = true;
                SetCheckBoxListvalue(ref cbPlaceForLogin, ObjMediaRepo.GetMediaPlaces(Convert.ToInt64(hfmediaid.Value), "Login Pop-up"), "Login Pop-up - Place");
                for (int i = 0; i < cbPlaceForLogin.Items.Count; i++)
                {
                    cbPlaceForLogin.Items[i].Attributes.Add("onclick", "ValidationForMultiSelection(this,'" + txtPlaceforLogin.ClientID + "','" + cbPlaceForLogin.ClientID + "','- Place -')");
                }
                // ddlLoginPopups.SelectedValue = objassigned.MediaLoginpopupid.ToString();
                //liPageurl.Visible = true;
                // SetThumbNailImage(Convert.ToInt32(ddlPlacement.SelectedValue), "login pop-up");
                //if (ddlLoginPopups.SelectedItem.Text.ToLower() == "become a member")
                //    {
                //        rfvCompany.Visible = false;
                //        rfvWorkGroup.Visible = false;
                //    }
            }
        }

        MediaFile objfile = ObjMediaRepo.GetFileById(Convert.ToInt64(hfmediaid.Value));
        if (objfile.MediaFolderId != null)
        {
            ddlMediaFolder.SelectedValue = objfile.MediaFolderId.ToString();
            ddlMediaFolder.Enabled = false;
        }
    }


    public void SetThumbNailImage(int id, string type)
    {
        //MediaRepository objrepos = new MediaRepository();
        //switch (type)
        //{
        //    case "document link":
        //        MediadocLinkSub objdoclink = objrepos.GetLinkSubById(Convert.ToInt64(ddldocsub.SelectedValue));
        //        if (!string.IsNullOrEmpty(objdoclink.ThumbnailImage))
        //            ltThumbImage.Text = "<a class='ajax'  href='../StaticContents/img/ThumbImages/" + objdoclink.ThumbnailImage + "' target='blank'><img src='../StaticContents/img/ThumbImages/" + objdoclink.ThumbnailImage + "' width='144px' height='144px' border='0' /></a>";
        //        else
        //            ltThumbImage.Text = "";
        //        break;
        //    case "help video":
        //        MediaHelpVidSub objHelpVid = objrepos.GetHelpSubById(Convert.ToInt64(ddlVideoSub.SelectedValue));
        //        if (!string.IsNullOrEmpty(objHelpVid.ThumbnailImage))
        //            ltThumbImage.Text = "<a class='ajax'  href='../StaticContents/img/ThumbImages/" + objHelpVid.ThumbnailImage + "' target='blank'><img src='../StaticContents/img/ThumbImages/" + objHelpVid.ThumbnailImage + "' width='144px' height='144px' border='0' /></a>";
        //        else
        //            ltThumbImage.Text = "";
        //        break;
        //    case "login pop-up":
        //        MediaLoginpopup objPopup = objrepos.GetLoginPopupbyid(Convert.ToInt64(ddlLoginPopups.SelectedValue));
        //        if (!string.IsNullOrEmpty(objPopup.ThumbnailImage))
        //            ltThumbImage.Text = "<a class='ajax' href='../StaticContents/img/ThumbImages/" + objPopup.ThumbnailImage + "' target='blank'><img src='../StaticContents/img/ThumbImages/" + objPopup.ThumbnailImage + "' width='144px' height='144px' border='0' /></a>";
        //        else
        //            ltThumbImage.Text = "";
        //        break;
        //}

    }
    protected void SetCheckBoxListvalue(ref CheckBoxList cbl, string cblvalue, string type)
    {

        cblvalue = ";" + cblvalue;

        if (string.IsNullOrEmpty(cblvalue))
        {
            foreach (ListItem li in cbl.Items)
            {
                li.Selected = false;
            }
        }
        else
        {
            foreach (ListItem li in cbl.Items)
            {
                if (cblvalue.Contains(";" + li.Value + ";"))
                {
                    li.Selected = true;
                    if (type == "Company")
                    {
                        txtCompany.Text = "- Selected -";
                    }
                    if (type == "Workgroup")
                    {
                        txtWorkgroups.Text = "- Selected -";
                    }

                    if (type == "Document Link - Module")
                    {
                        txtModuleForDoc.Text = "- Selected -";

                    }
                    if (type == "Document Link - Place")
                    {
                        txtPlaceForDoc.Text = "- Selected -";
                    }

                    if (type == "Help Video - Module")
                    {
                        txtModuleForHelp.Text = "- Selected -";
                    }
                    if (type == "Help Video - Place")
                    {
                        txtPlaceForHelp.Text = "- Selected -";
                    }
                    if (type == "Login Pop-up - Place")
                    {
                        txtPlaceforLogin.Text = "- Selected -";
                    }
                }
                else
                {
                    li.Selected = false;
                }
            }
        }

    }

    protected void SetCheckBoxListvalueForstore(ref CheckBoxList cbl, string cblvalue)
    {

        cblvalue = "," + cblvalue + ",";
        if (string.IsNullOrEmpty(cblvalue))
        {
            foreach (ListItem li in cbl.Items)
            {
                li.Selected = false;
            }
        }
        else
        {
            foreach (ListItem li in cbl.Items)
            {
                if (cblvalue.Contains("," + li.Value + ","))
                {
                    li.Selected = true;
                }
                else
                {
                    li.Selected = false;
                }
            }
        }
    }


    private void ResetMultiSelectDropDown()
    {
        if (ddlPlacement.SelectedItem.Text == "Document Link")
        {
            for (int i = 0; i < cbPlacefordoc.Items.Count; i++)
            {
                cbPlacefordoc.Items[i].Attributes.Add("onclick", "ValidationForMultiSelection(this,'" + txtPlaceForDoc.ClientID + "','" + cbPlacefordoc.ClientID + "','- Place -')");
            }
            if (cbPlacefordoc.SelectedIndex != -1)
            {
                txtPlaceForDoc.Text = "- Selected -";
            }

        }


        if (ddlPlacement.SelectedItem.Text == "Help Video")
        {

            for (int i = 0; i < cbPlaceForHelp.Items.Count; i++)
            {
                cbPlaceForHelp.Items[i].Attributes.Add("onclick", "ValidationForMultiSelection(this,'" + txtPlaceForHelp.ClientID + "','" + cbPlaceForHelp.ClientID + "','- Place -')");
            }
            if (cbPlaceForHelp.SelectedIndex != -1)
            {
                txtPlaceForHelp.Text = "- Selected -";
            }

        }

        if (ddlPlacement.SelectedItem.Text == "Login Pop-up")
        {
            for (int i = 0; i < cbPlaceForLogin.Items.Count; i++)
            {
                cbPlaceForLogin.Items[i].Attributes.Add("onclick", "ValidationForMultiSelection(this,'" + txtPlaceforLogin.ClientID + "','" + cbPlaceForLogin.ClientID + "','- Place -')");
            }

            if (cbPlaceForLogin.SelectedIndex != -1)
            {
                txtPlaceforLogin.Text = "- Selected -";
            }
        }


        for (int i = 0; i < cbCompany.Items.Count; i++)
        {
            cbCompany.Items[i].Attributes.Add("onclick", "ValidationForComp(this)");



        }
        if (cbCompany.SelectedIndex != -1)
        {
            txtCompany.Text = "- Selected -";
        }
        else
        {
            txtCompany.Text = "- Company -";
        }



        for (int i = 0; i < cbWorkGroup.Items.Count; i++)
        {
            if (cbWorkGroup.Items[i].Selected == true)

                txtWorkgroups.Text = "- Selected -";
            cbWorkGroup.Items[i].Attributes.Add("onclick", "ValidationForWork(this)");


        }
        if (cbWorkGroup.SelectedIndex != -1)
        {
            txtWorkgroups.Text = "- Selected -";
        }
        else
        {
            txtWorkgroups.Text = "- Workgroup -";
        }
    }


    void ResetValueInAssignToStoreImagePopup()
    {
        imgProductImage.Src = "";
        ltfilenameforstore.Text = "";
        ltmediapopupdateforstore.Text = "";
        ltMediaPopupViewforstore.Text = "";

    }
    void FillValueInAssignToStoreImagePopup(Int64 mediaid)
    {
        MediaRepository ObjMediaRepo = new MediaRepository();
        MediaAssigned objassigned = new MediaAssigned();
        MediaFile ObjMediafile = ObjMediaRepo.GetFileById(mediaid);
        HFStoreImageMode.Value = "Edit";
        
        if (ObjMediafile != null)
        {

            SetCheckBoxListvalue(ref cbCompanyforassignimage, ObjMediaRepo.GetMediaCompWorkBymediaid(mediaid, "Company"), "Company");
            SetCheckBoxListvalue(ref CBWorkgroupForAssignImage, ObjMediaRepo.GetMediaCompWorkBymediaid(mediaid, "Workgroup"), "Workgroup");

            if (cbCompanyforassignimage.SelectedIndex == -1)
            {
                txtCompanyforassignimage.Text = "- Company -";
            }
            else
            {
                txtCompanyforassignimage.Text = "- Selected -";
            }

            if (CBWorkgroupForAssignImage.SelectedIndex == -1)
            {
                txtWorkgroupForAssignImage.Text = "- Workgroup -";
            }
            else
            {
                txtWorkgroupForAssignImage.Text = "- Selected -";
            }
            
            txtMasterStyleName.Text = ObjMediafile.Name;
            MediaRepository ObhRepos = new MediaRepository();
            ddlProductCategory.SelectedValue = ObjMediafile.productcategoryid == null ? "0" : ObjMediafile.productcategoryid.ToString();

            //Session["MediaFileName"] = ObjMediafile.OriginalFileName;
            //Session["MediaFileType"] = ObjMediafile.FileType;

        }

        if (ObjMediafile.MediaFolderId != null)
        {
            ddlFolderForAssignImage.SelectedValue = ObjMediafile.MediaFolderId.ToString();
        }
    }
    private void ShowAlertMessage(string message)
    {
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "GeneralAlertMsg('" + message + "');", true);
    }

    private void OpenPopup(string mainid, string contentid)
    {
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowPopUp('" + mainid + "','" + contentid + "');", true);
    }

    private void BindEventWithMultiDropDown()
    {
        bool comp = false;
        bool work = false;


        for (int i = 0; i < cbCompany.Items.Count; i++)
        {
            if (cbCompany.Items[i].Selected == true)
                comp = true;
            cbCompany.Items[i].Attributes.Add("onclick", "ValidationForComp(this)");
        }
        for (int i = 0; i < cbWorkGroup.Items.Count; i++)
        {
            if (cbWorkGroup.Items[i].Selected == true)
                work = true;
            txtWorkgroups.Text = "- Selected -";
            cbWorkGroup.Items[i].Attributes.Add("onclick", "ValidationForWork(this)");
        }

        if (comp == true)
            txtCompany.Text = "- Selected -";
        else
            txtCompany.Text = "- Company -";

        if (work == true)
            txtWorkgroups.Text = "- Selected -";
        else
            txtWorkgroups.Text = "- Workgroup -";

    }

    private void ResetAssignedMediaImage()
    {
        for (int i = 0; i < cbCompanyforassignimage.Items.Count; i++)
        {
            cbCompanyforassignimage.Items[i].Selected = false;
            cbCompanyforassignimage.Items[i].Attributes.Add("onclick", "ValidationForMultiSelection(this,'" + txtCompanyforassignimage.ClientID + "','" + cbCompanyforassignimage.ClientID + "','- Company -')");
        }
        for (int i = 0; i < CBWorkgroupForAssignImage.Items.Count; i++)
        {
            CBWorkgroupForAssignImage.Items[i].Selected = false;
            CBWorkgroupForAssignImage.Items[i].Attributes.Add("onclick", "ValidationForMultiSelection(this,'" + txtWorkgroupForAssignImage.ClientID + "','" + CBWorkgroupForAssignImage.ClientID + "','- Workgroup -')");
        }
    }


    private void ResetAssignedMEdia()
    {
        txtCompany.Text = "- Company -";
        txtWorkgroups.Text = "- Workgroup -";
        BindDropDownValues();

        ddlMediaFolder.Enabled = true;
        ddlMediaFolder.SelectedValue = "0";
        lidocmain.Visible = false;
        lidocsub.Visible = false;
        liassetmain.Visible = false;
        liassetsub.Visible = false;
        liloginpopup.Visible = false;
        //liPageurl.Visible = false;
        ltmediaimage.Text = "";
        ltmediapopupdate.Text = "";
        ltMediaPopupView.Text = "";
        ltThumbImage.Text = "";

        ddlPlacement.SelectedValue = "0";
        txtMediaSearchTag.Text = "";
        txtPubStartDate.Text = "";
        txtPubEndDate.Text = "";
        //        txtPageUrl.Text = "";
        txtMedoaPassword.Text = "";
        ddlFoldeName.SelectedValue = "0";
        hfMediaAssignedid.Value = "0";
        for (int i = 0; i < cbCompany.Items.Count; i++)
        {
            cbCompany.Items[i].Selected = false;
            cbCompany.Items[i].Attributes.Add("onclick", "ValidationForComp(this)");
        }
        for (int i = 0; i < cbWorkGroup.Items.Count; i++)
        {
            cbWorkGroup.Items[i].Selected = false;
            cbWorkGroup.Items[i].Attributes.Add("onclick", "ValidationForWork(this)");
        }
    }
    private void BindDropDownValues()
    {
        try
        {
            MediaRepository OBJ = new MediaRepository();
            List<MediaFolder> objList = new List<MediaFolder>();
            objList = OBJ.GetAllFolder();
            BindCustomDropDown(ddlFoldeName, objList, "FolderName", "MediaFolderId", "- Folder -", "-1");
            BindDropDown(ddlMediaFolder, objList, "FolderName", "MediaFolderId", "Folder", "0");
            objList = OBJ.GetSystemFolder();
            BindDropDown(ddlFolderForAssignImage, objList, "FolderName", "MediaFolderId", "Folder", "0");

            List<MediaHelpVidPrnt> lsthelpvid = new List<MediaHelpVidPrnt>();
            lsthelpvid = OBJ.GetAllHelpMainSection();
            DataTable dthelpmodule = Common.ListToDataTable(lsthelpvid);
            this.CbModuleForHelp.DataSource = dthelpmodule;
            this.CbModuleForHelp.DataTextField = "ModuleName";
            this.CbModuleForHelp.DataValueField = "MediaHelpVidPrntId";
            this.CbModuleForHelp.DataBind();




            List<MediaDocLinkParent> listdocmain = new List<MediaDocLinkParent>();
            listdocmain = OBJ.GetAllDocMainSection();

            DataTable dtdocMain = Common.ListToDataTable(listdocmain);
            this.CbModuleForDoc.DataSource = dtdocMain;
            this.CbModuleForDoc.DataTextField = "ModuleName";
            this.CbModuleForDoc.DataValueField = "MediaDocLinkParentId";
            this.CbModuleForDoc.DataBind();

            // List<MediadocLinkSub> lstDocSec = OBJ.GetAllDocMainSubSection(0);
            //BindCustomDropDown(ddldocsub, lstDocSec, "ModuleName", "MediadocLinkSubId", "- Place -", "-1");

            // List<MediaHelpVidSub> lstHelpvidSec = OBJ.GetAllHelpMainSubSection(0);
            // BindCustomDropDown(ddlVideoSub, lstHelpvidSec, "ModuleName", "MediaHelpVidSubId", "- Place -", "-1");

            List<MediaLoginpopup> lstloginpopup = new List<MediaLoginpopup>();
            lstloginpopup = OBJ.GetLoginpopuplist();
            DataTable dtloginpopup = Common.ListToDataTable(lstloginpopup);
            this.cbPlaceForLogin.DataSource = dtloginpopup;
            this.cbPlaceForLogin.DataTextField = "ModuleName";
            this.cbPlaceForLogin.DataValueField = "MediaLoginpopupid";
            this.cbPlaceForLogin.DataBind();

            // BindCustomDropDown(ddlLoginPopups, lstloginpopup, "ModuleName", "MediaLoginpopupid", "- Place -", "-1");


            CompanyRepository objCompanyRepo = new CompanyRepository();
            List<Company> lstCompanies = objCompanyRepo.GetAllCompany();
            DataTable dt = Common.ListToDataTable(lstCompanies);
            this.cbCompany.DataSource = dt;
            this.cbCompany.DataTextField = "CompanyName";
            this.cbCompany.DataValueField = "CompanyID";
            this.cbCompany.DataBind();


            this.cbCompanyforassignimage.DataSource = dt;
            this.cbCompanyforassignimage.DataTextField = "CompanyName";
            this.cbCompanyforassignimage.DataValueField = "CompanyID";
            this.cbCompanyforassignimage.DataBind();

            BindDropDown(ddlSearchcomp, lstCompanies, "CompanyName", "CompanyID", "Company", "0");






            LookupRepository objLookupRepo = new LookupRepository();
            List<INC_Lookup> lstWorkGroups = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Workgroup).OrderBy(le => le.sLookupName).ToList();

            DataTable dt1 = Common.ListToDataTable(lstWorkGroups);
            this.cbWorkGroup.DataSource = dt1;
            this.cbWorkGroup.DataTextField = "sLookupName";
            this.cbWorkGroup.DataValueField = "iLookupID";
            this.cbWorkGroup.DataBind();


            this.CBWorkgroupForAssignImage.DataSource = dt1;
            this.CBWorkgroupForAssignImage.DataTextField = "sLookupName";
            this.CBWorkgroupForAssignImage.DataValueField = "iLookupID";
            this.CBWorkgroupForAssignImage.DataBind();

            BindDropDown(ddlSearchWorkgroup, lstWorkGroups, "sLookupName", "iLookupID", "Workgroup", "0");

            List<MediaPlaceMent> lstplacement = OBJ.GetPlacement();
            BindDropDown(ddlPlacement, lstplacement, "PlacementName", "MediaPlaceMentId", "Placement", "0");

            CatogeryRepository objCatRepository = new CatogeryRepository();
            List<Category> objCatlist = new List<Category>();
            objCatlist = objCatRepository.GetAllCategory();
            if (objCatlist.Count != 0)
            {
                ddlProductCategory.DataSource = objCatlist;
                ddlProductCategory.DataValueField = "CategoryID";
                ddlProductCategory.DataTextField = "CategoryName";
                ddlProductCategory.DataBind();
                ddlProductCategory.Items.Insert(0, new ListItem("- Product Category -", "0"));
            }

        }
        catch (Exception EX)
        {
            ErrHandler.WriteError(EX);
        }
    }

    private void BindDropDown(DropDownList ddlDropDown, Object lstValues, String textField, String valueField, String selectLabel, String selectedValue)
    {
        try
        {
            ddlDropDown.DataSource = lstValues;
            ddlDropDown.DataTextField = textField;
            ddlDropDown.DataValueField = valueField;
            ddlDropDown.DataBind();

            ddlDropDown.Items.Insert(0, new ListItem("- " + selectLabel + " -", "0"));
            ddlDropDown.SelectedValue = ddlDropDown.Items.FindByValue(selectedValue) != null ? selectedValue : "0";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public static void BindCustomDropDown<T>(CustomDropDown ddl, List<T> list, string DataTextField, string DataValueField,
      string FirstListItem, string AddNewOptionValue) where T : class
    {
        try
        {

            ddl.Items.Clear();
            if (list.Count > 0)
            {

                ddl.DataSource = list;
                ddl.DataTextField = DataTextField;
                ddl.DataValueField = DataValueField;
                ddl.DataBind();
            }
            ListItem liAddNew = new ListItem("- Add New Option -", AddNewOptionValue);
            ddl.Items.Insert((ddl.Items.Count), liAddNew);
            ddl.Value = AddNewOptionValue;

            if (!string.IsNullOrEmpty(FirstListItem))
                ddl.Items.Insert(0, new ListItem(FirstListItem, "0"));
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    protected void ShowPopUp(HtmlGenericControl Parent_span)
    {
        try
        {
            if (Parent_span.Attributes["data-content"] != null)
            {
                String strDivClasses = Convert.ToString(Parent_span.Attributes["data-content"]).ToLower();
                if (!String.IsNullOrEmpty(strDivClasses))
                {
                    string[] strDivIDs = strDivClasses.Split(';');
                    if (strDivIDs.Length == 2)
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowPopUp('" + strDivIDs[0] + "','" + strDivIDs[1] + "');", true);
                        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "$('#"+ strDivIDs[0] +"').css('top', '0').show();$('.fade-layer').show();popupCenter();", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }

    }
    // code to save first popup first filename

    //list of mediafiles
    void BindMediaFiles()
    {


        //NoOfPagesToDisplay = Convert.ToInt32(Application["NEWPAGERSIZE"]);
        //NoOfRecordsToDisplay = Convert.ToInt32(Application["NEWPAGESIZE"]) % 2 == 0 ? Convert.ToInt32(Application["NEWPAGESIZE"]) : Convert.ToInt32(Application["NEWPAGESIZE"]) - 1;
        //TotalPages = 1;
        //FromPage = 1;
        //ToPage = NoOfPagesToDisplay;





        MediaRepository ObjMediaRepos = new MediaRepository();

        List<GetAllFileListResult> lstRepository = ObjMediaRepos.GetAllFileList(FolderId, this.NoOfRecordsToDisplay, this.PageIndex);
        if (lstRepository.Count == 0 && PageIndex > 1)
        {
            PageIndex = PageIndex - 1;
            lstRepository = ObjMediaRepos.GetAllFileList(FolderId, this.NoOfRecordsToDisplay, this.PageIndex);
        }




        if (lstRepository != null && lstRepository.Count > 0)
        {
            lblErrorMessage.Text = "";
            if (this.NoOfRecordsToDisplay > 0)
                this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lstRepository[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
            else
                this.TotalPages = 1;



            dlMediaFiles.DataSource = lstRepository;
            dlMediaFiles.DataBind();

            GeneratePaging();
            lnkPrevious.Enabled = this.PageIndex > 1;
            lnkNext.Enabled = this.PageIndex < this.TotalPages;

            if (dlMediaFiles.Items.Count > 0)
            {
                pagingtable.Visible = true;
            }
            else
            {
                //totalcount_em.InnerText = "no results";
                pagingtable.Visible = false;
            }
            //totalcount_em.Visible = true;

        }
        else
        {
            lblErrorMessage.Text = "No media files found in database";
            dlMediaFiles.DataSource = lstRepository;
            dlMediaFiles.DataBind();
            pagingtable.Visible = false;

        }
    }
    private void GeneratePaging()
    {
        try
        {
            Int32 tCurrentPg = this.PageIndex;
            Int32 tToPg = this.ToPage;
            Int32 tFrmPg = this.FromPage;

            if (tCurrentPg > tToPg)
            {
                tFrmPg = tToPg + 1;
                tToPg += this.NoOfPagesToDisplay;
            }

            if (tCurrentPg < tFrmPg)
            {
                tToPg = tFrmPg - 1;
                tFrmPg -= this.NoOfPagesToDisplay;
            }
            if (this.TotalPages - tToPg == 1 && (tToPg + 1) - tFrmPg <= (NoOfPagesToDisplay - 1))
            {
                tToPg = tToPg + 1;
            }

            if (tToPg > this.TotalPages)
                tToPg = this.TotalPages;

            this.ToPage = tToPg;
            this.FromPage = tFrmPg;

            DataTable dt = new DataTable();
            dt.Columns.Add("Index");

            for (Int32 i = tFrmPg; i <= tToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Index"] = i;
                dt.Rows.Add(dr);
            }

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }



    public string GetMediaImage(string filename, string filetype, string name, string mediaid)
    {
        //mm
        string img = "";
        switch (filetype)
        {
            case "video":
                string videourl = System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"].ToString() + "DocmentStorageCenter/" + filename;
                img = "<a  onclick='ShowVideo(\"" + videourl + "\")' href='#'  > <img src='../StaticContents/img/vidpopupimage.png' width='144px' height='144px' border='0' /></a>";
                break;
            case "document":
                img = "<a target='_blank' href='../DocmentStorageCenter/" + filename + "' title='" + name + "'  ><img src='../StaticContents/img/dmspdf-img.jpg' width='144px' height='144px' border='0' /></a>";
                break;
            case "pdf":
                img = "<a target='_blank' href='../DocmentStorageCenter/" + filename + "' title='" + name + "'  ><img src='../StaticContents/img/dmspdf-img.jpg' width='144px' height='144px' border='0' /></a>";
                break;
            case "folder":
                img = "<a  href='DocumentStorageCenter.aspx?folderid=" + mediaid + "' title='" + name + "'  ><img src='../StaticContents/img/foldericon.png' width='144px' height='144px' border='0' /></a>";
                break;
            case "image":
                string LasrgeName = "";

                if (filename.StartsWith("Thumb_"))
                    LasrgeName = "Large_" + filename.Substring(6);
                else
                    LasrgeName = filename;

                img = "<a class='ajax'  href='../DocmentStorageCenter/" + LasrgeName + "' target='blank'><img src='../DocmentStorageCenter/" + filename + "' width='144px' height='144px' border='0' /></a>";
                break;

        }
        return img;
    }

    public bool ShowAssigned(string filetype)
    {
        if (filetype == "video" || filetype == "document" || filetype == "pdf")
            return true;
        else
            return false;
    }

    public bool ShowAssignedToStore(string filetype, string buttontype)
    {

        if (filetype != "image" && buttontype != "assign to store")
            return true;

        if (filetype == "image" && buttontype == "rename")
            return false;

        if (filetype == "image")
            return true;
        else
            return false;
    }

    public bool CheckForSystemFolder(string filetype, string id)
    {
        if (filetype.ToLower() == "folder")
        {
            MediaRepository objMedia = new MediaRepository();
            MediaFolder ObjFolder = objMedia.GetFolderByID(Convert.ToInt64(id));
            if (ObjFolder.FolderType.ToLower() == "system folder")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }


    public bool ShowForFolder(string filetype)
    {
        if (filetype.ToLower() == "folder" || filetype.ToLower() == "system folder")
        {
            return true;
        }
        else
        {
            return false;
        }

    }



    public bool ShowRemoveButtonforSystemFolder(string filetype, string id)
    {
        if (filetype.ToLower() == "folder")
        {
            MediaRepository objMedia = new MediaRepository();
            MediaFolder ObjFolder = objMedia.GetFolderByID(Convert.ToInt64(id));
            if (ObjFolder.FolderType.ToLower() == "system folder")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }




    public bool ShowButtons(string isdeleted)
    {
        if (isdeleted.ToLower() == "true")
            return false;
        else
            return true;
    }

    #endregion

    #region Paging Events

    protected void dtlPaging_ItemCommand(Object sender, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ChangePage")
            {
                this.PageIndex = Convert.ToInt32(e.CommandArgument);
                BindMediaFiles();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }



    protected void dtlPaging_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            LinkButton lnkPaging = (LinkButton)e.Item.FindControl("lnkPaging");

            if (lnkPaging.Text == Convert.ToString(this.PageIndex))
            {
                lnkPaging.Enabled = false;
                lnkPaging.Font.Bold = true;
                lnkPaging.Attributes.Add("style", "cursor:pointer;");
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkPrevious_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex -= 1;
            BindMediaFiles();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkNext_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex += 1;
            BindMediaFiles();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkViewAll_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex = 1;
            this.NoOfRecordsToDisplay = 0;
            BindMediaFiles();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion









    //public void GetMyHelpVideo()
    //{
    //     MediaRepository objmedia=new MediaRepository();
    //     GetMediaHelpVideoOrDocResult ObjPageVideo = objmedia.GetMediaHelpVideoOrDoc("help video", "Document & Media Storage", "Document & Media Storage", Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
    //        if (ObjPageVideo != null)
    //        {
    //        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowVideo('" + ObjPageVideo.Vimeourl + "');", true);
    //        }
    //}

    //protected void lbHelpVideo_Click(object sender, EventArgs e)
    //{
    //    string strVideoURL = Common.GetMyHelpVideo("help video", "Document and Media Storage", "Document and Media Storage");
    //    if (strVideoURL != "")
    //    {
    //        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowVideo('" + strVideoURL + "');", true);
    //    }
    //}

}
