using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class NewDesign_Admin_MediaSearch : PageBase
{

    #region Properties

    String SiteURL
    {
        get
        {
            return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["siteurl"].ToString());
        }
    }

    public Int64? WorkGroupID
    {
        get
        {
            if (this.ViewState["WorkGroupID"] == null || Convert.ToString(this.ViewState["WorkGroupID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["WorkGroupID"]);
        }
        set
        {
            this.ViewState["WorkGroupID"] = value;
        }
    }



    public String KeyWord
    {
        get
        {
            if (Convert.ToString(this.ViewState["KeyWord"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["KeyWord"]);
        }
        set
        {
            this.ViewState["KeyWord"] = value;
        }
    }

    public Int64? CompanyID
    {
        get
        {
            if (this.ViewState["CompanyID"] == null || Convert.ToString(this.ViewState["CompanyID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["CompanyID"]);
        }
        set
        {
            this.ViewState["CompanyID"] = value;
        }
    }

    public Int32 NoOfRecordsToDisplay
    {
        get
        {
            if (Convert.ToString(this.ViewState["NoOfRecordsToDisplay"]) == "")
                return Convert.ToInt32(Application["NEWPAGESIZE"]);
            else
                return Convert.ToInt32(this.ViewState["NoOfRecordsToDisplay"]);
        }
        set
        {
            this.ViewState["NoOfRecordsToDisplay"] = value;
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

    public String SortColumn
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortColumn"]) == String.Empty)
                return "EmployeeName";
            else
                return Convert.ToString(this.ViewState["SortColumn"]);
        }
        set
        {
            this.ViewState["SortColumn"] = value;
        }
    }

    public String SortDirection
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortDirection"]) == String.Empty)
                return "ASC";
            else
                return Convert.ToString(this.ViewState["SortDirection"]);
        }
        set
        {
            this.ViewState["SortDirection"] = value;
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



    public Int64? MediaStatusID
    {
        get
        {
            if (this.ViewState["MediaStatusID"] == null || Convert.ToString(this.ViewState["MediaStatusID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["MediaStatusID"]);
        }
        set
        {
            this.ViewState["MediaStatusID"] = value;
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


    #endregion

    #region Page Event's

    protected void ddlCommonDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (mvAddNewAsset.ActiveViewIndex == 3)
        //    BindSpecsFields();
        try
        {
            //rfvCompany.Visible = true;
            //rfvWorkGroup.Visible = true;
            
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
                ltThumbImage.Text = "";
                //txtPageUrl.Text = "";

               
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

            if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) && IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            {
                Response.Redirect("unauthorised.aspx");
            }
            FillDropDowns();
            
            if (Session["MediaCompanyid"]!=null)
            this.CompanyID = Convert.ToInt64(Session["MediaCompanyid"]);
            if (Session["MediaWorkGroupid"] != null)
            this.WorkGroupID = Convert.ToInt64(Session["MediaWorkGroupid"]);
            if (Session["MediaKeyword"] != null)
            this.KeyWord = Session["MediaKeyword"].ToString();
           // this.NoOfRecordsToDisplay = 2;
            this.PageIndex = 1;
            BindGrid(false);
        }
    }

    protected void btnAssignedMedia_Click(object sender, EventArgs e)
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

                if (ValidateAssignMedia() == false)
                {
                    OpenPopup("add-media-block", "employess-content");
                    ResetMultiSelectDropDown();
                    return;

                }
                

                
                MediaAssigned objMediaAssn = objMedRepo.Getmediaassignedbyid(Convert.ToInt64(hfmediaid.Value));

                if (objMediaAssn == null)
                {
                    ShowAlertMessage("Record not found.");
                    return;
                }
                     
                    


                    objMediaAssn.MediaFileId = Convert.ToInt64(hfmediaid.Value);
                    objMediaAssn.PlacementId = Convert.ToInt64(ddlPlacement.SelectedValue);
                    objMediaAssn.SearchTags = txtMediaSearchTag.Text;
                    objMediaAssn.IsPublished = false;
              
                   

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



                    ////SaveScreenFile(ddlPlacement.SelectedItem.Text.ToLower());

                    if (ddlMediaFolder.SelectedValue != "0")
                    {
                        MediaFile objFile = objMedRepo.GetFileById(Convert.ToInt64(hfmediaid.Value));
                        objFile.MediaFolderId = Convert.ToInt64(ddlMediaFolder.SelectedValue);
                        objMedRepo.SubmitChanges();
                    }

                    objMedRepo.DeleteCompWork(Convert.ToInt64(hfmediaid.Value));
                    
                    if (cbCompany.SelectedIndex == -1 )
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

                  if(cbWorkGroup.SelectedIndex == -1)
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
                    
                    ShowAlertMessage("File/Video has been assigned successfully. You can publish it by clicking on publish.");
                    BindGrid(false);
               
           
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnSearchGrid_Click(object sender, EventArgs e)
    {
        try
        {
            NoOfPagesToDisplay = Convert.ToInt32(Application["NEWPAGERSIZE"]);
            NoOfRecordsToDisplay = Convert.ToInt32(Application["NEWPAGESIZE"]);
            TotalPages = 1;
            FromPage = 1;
            ToPage = NoOfPagesToDisplay;

            this.KeyWord = txtSearchGrid.Text.Trim();
            BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void btnSearchMedia_Click(object sender, EventArgs e)
    {
        try
        {

            NoOfPagesToDisplay = Convert.ToInt32(Application["NEWPAGERSIZE"]);
            NoOfRecordsToDisplay = Convert.ToInt32(Application["NEWPAGESIZE"]);
            TotalPages = 1;
            FromPage = 1;
            ToPage = NoOfPagesToDisplay;


            this.CompanyID = Convert.ToInt64(ddlSearchCompany.SelectedValue);
            this.WorkGroupID = Convert.ToInt64(ddlSearchWorkGroup.SelectedValue);
          
          
            this.MediaStatusID = Convert.ToInt64(ddlSearchStatus.SelectedValue);
            this.KeyWord = String.Empty;
            //this.NoOfRecordsToDisplay = 2;
            this.PageIndex = 1;

            btnSearchGrid.Enabled = true;
            //btnSearchGrid.CssClass += " postback";
            txtSearchGrid.Text = String.Empty;

            //if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
            //this.CompanyID = Convert.ToInt64(3);
            BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvMediaSearch_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Sort")
            {
                if (this.SortColumn == null)
                {
                    this.SortColumn = Convert.ToString(e.CommandArgument);
                    this.SortDirection = "ASC";
                }
                else
                {
                    if (this.SortColumn == Convert.ToString(e.CommandArgument))
                    {
                        if (this.SortDirection == "ASC")
                            this.SortDirection = "DESC";
                        else
                            this.SortDirection = "ASC";
                    }
                    else
                    {
                        this.SortDirection = "ASC";
                        this.SortColumn = Convert.ToString(e.CommandArgument);
                    }
                }

                BindGrid(true);
            }
            
            if (e.CommandName.ToString().Substring(0, 3) == "asg")
            {
                 MediaRepository ObjMedia = new MediaRepository();
                int mediaid = Convert.ToInt32(e.CommandArgument);
               
                hfmediaid.Value = mediaid.ToString();
            

                MediaFile objmediafile = ObjMedia.GetFileById(mediaid);
                DateTime dt = Convert.ToDateTime(objmediafile.CreatedDate);
                switch (e.CommandName.ToString())
                {
                    case "asg-video":
                        ResetAssignedMEdia();
                        FillAssignedMedia(Convert.ToInt64(hfmediaid.Value));
                        string videourl = System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"].ToString() + "DocmentStorageCenter/" + objmediafile.OriginalFileName;
                        ltmediaimage.Text = "<a  onclick='ShowVideo(\"" + videourl + "\")' href='#'  > <img src='../StaticContents/img/vidpopupimage.png' width='144px' height='144px' border='0' /></a>";
                        btnAssignedMedia.Text = "Assign Video";
                        ltfilename.Text = objmediafile.Name;
                        ltmediapopupdate.Text = dt.ToString("MMM d, yyyy"); //dt.Date.Month.ToString() + " " + dt.Date.Day.ToString() + ", " + dt.Date.Year.ToString();
                        ltMediaPopupView.Text = objmediafile.View == null ? "0" : objmediafile.View.ToString();
                        hfMediaType.Value = objmediafile.FileType;
                        OpenPopup("add-media-block", "employess-content");
                        break;
                    case "asg-document":
                        ResetAssignedMEdia();
                        FillAssignedMedia(Convert.ToInt64(hfmediaid.Value));
                        ltmediaimage.Text = "<a target='_blank' href='../DocmentStorageCenter/" + objmediafile.OriginalFileName + "' title='" + objmediafile.Name + "'  > <img src='../StaticContents/img/dmspdf-img.jpg' width='144px' height='144px' border='0' /></a>";
                        btnAssignedMedia.Text = "Assign Document";
                        ltfilename.Text = objmediafile.Name;
                        ltmediapopupdate.Text = dt.ToString("MMM d, yyyy"); //dt.Date.Month.ToString() + " " + dt.Date.Day.ToString() + ", " + dt.Date.Year.ToString();
                        ltMediaPopupView.Text = objmediafile.View == null ? "0" : objmediafile.View.ToString();
                        hfMediaType.Value = objmediafile.FileType;
                        OpenPopup("add-media-block", "employess-content");
                        break;
                    case "asg-pdf":
                        ResetAssignedMEdia();
                        FillAssignedMedia(Convert.ToInt64(hfmediaid.Value));
                        ltmediaimage.Text = "<a target='_blank' href='../DocmentStorageCenter/" + objmediafile.OriginalFileName + "' title='" + objmediafile.Name + "'  > <img src='../StaticContents/img/dmspdf-img.jpg' width='144px' height='144px' border='0' /></a>";
                        btnAssignedMedia.Text = "Assign Document";
                        ltfilename.Text = objmediafile.Name;
                        ltmediapopupdate.Text = dt.ToString("MMM d, yyyy"); //dt.Date.Month.ToString() + " " + dt.Date.Day.ToString() + ", " + dt.Date.Year.ToString();
                        ltMediaPopupView.Text = objmediafile.View == null ? "0" : objmediafile.View.ToString();
                        hfMediaType.Value = objmediafile.FileType;
                        OpenPopup("add-media-block", "employess-content");
                        break;
                    case "asg-image":
                        //DivProductAssignPassword.Visible = false;
                        ResetAssignedMediaImage();
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

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void chkStatus_CheckedChanged(Object sender, EventArgs e)
    {
        try
        {

            CheckBox chkSender = (CheckBox)sender;
            Boolean IsStatusUpdated = false;

            foreach (GridViewRow employeeRow in gvMediaSearch.Rows)
            {
                CheckBox chkStatus = (CheckBox)employeeRow.FindControl("chkStatus");

                if (chkSender.ClientID == chkStatus.ClientID)
                {
                    HiddenField hfMediaAssigned = (HiddenField)employeeRow.FindControl("hfMediaAssigned");

                    if (hfMediaAssigned != null && Convert.ToInt64(hfMediaAssigned.Value) > 0)
                    {
                        MediaRepository ObjRepos = new MediaRepository();
                        MediaAssigned ObjAssgn = ObjRepos.GetAssigedMediaById(Convert.ToInt64(hfMediaAssigned.Value));
                        if (ObjAssgn != null)
                        {
                            if (ObjAssgn.Status == this.ActiveID)
                            {
                                ObjAssgn.Status = this.InActiveID;
                            }
                            else
                            {
                                ObjAssgn.Status = this.ActiveID;
                            }

                            ObjRepos.SubmitChanges();
                            IsStatusUpdated = true;
                            break;
                        }
                    }
                }
            }

            if (IsStatusUpdated)
                BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
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
   
        ltThumbImage.Text = "";

        txtPlaceForDoc.Text = "- Place -";
        txtModuleForDoc.Text = "- Module -";
        txtPlaceForHelp.Text = "- Place -";
        txtModuleForDoc.Text = "- Module -";
        txtPlaceforLogin.Text = "- Place -";


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
 
    protected void BtnAssignToStore_Click1(object sender, EventArgs e)
    {
        // Todo:: Remove HFStoreId and hfstoreproductid 
        try
        {
            


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

            BindGrid(false);
            ShowAlertMessage("Record has been updated successfully");

            txtMasterStyleName.Text = "";
            ddlFolderForAssignImage.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
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
                BindGrid(false);
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
            BindGrid(false);
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
            BindGrid(false);
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
            BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Methods
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
    public bool GetStatusValue(string FileType)
    {
        if (FileType == "image")
            return false;
        else
            return true;

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

    public void FillAssignedMedia(Int64 mediaid)
    {

        //rfvCompany.Visible = true;
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
            //rfvWorkGroup.Visible = true;
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


    protected void SetCheckBoxListvalue(ref CheckBoxList cbl, string cblvalue,string type)
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
     public void SetThumbNailImage(int id,string type)
    {
        //MediaRepository objrepos = new MediaRepository();
        //switch (type)
        //{
        //    case "document link":
        //        MediadocLinkSub objdoclink = objrepos.GetLinkSubById(Convert.ToInt64(ddldocsub.SelectedValue));
        //        if (!string.IsNullOrEmpty(objdoclink.ThumbnailImage))
        //            ltThumbImage.Text = "<a class='group2'  href='../StaticContents/img/ThumbImages/" + objdoclink.ThumbnailImage + "' target='blank'><img src='../StaticContents/img/ThumbImages/" + objdoclink.ThumbnailImage + "' width='144px' height='144px' border='0' /></a>";
        //        else
        //            ltThumbImage.Text = "";
        //        break;
        //    case "help video":
        //        MediaHelpVidSub objHelpVid = objrepos.GetHelpSubById(Convert.ToInt64(ddlVideoSub.SelectedValue));
        //        if (!string.IsNullOrEmpty(objHelpVid.ThumbnailImage))
        //            ltThumbImage.Text = "<a class='group2'  href='../StaticContents/img/ThumbImages/" + objHelpVid.ThumbnailImage + "' target='blank'><img src='../StaticContents/img/ThumbImages/" + objHelpVid.ThumbnailImage + "' width='144px' height='144px' border='0' /></a>";
        //        else
        //            ltThumbImage.Text = "";
        //            break;
        //    case "login pop-up":
        //        MediaLoginpopup objPopup = objrepos.GetLoginPopupbyid(Convert.ToInt64(ddlLoginPopups.SelectedValue));
        //        if (!string.IsNullOrEmpty(objPopup.ThumbnailImage))
        //            ltThumbImage.Text = "<a class='group2' href='../StaticContents/img/ThumbImages/" + objPopup.ThumbnailImage + "' target='blank'><img src='../StaticContents/img/ThumbImages/" + objPopup.ThumbnailImage + "' width='144px' height='144px' border='0' /></a>";
        //        else
        //            ltThumbImage.Text = "";
        //        break;
        //}

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
      // BindDropDownValues();


        ddlMediaFolder.Enabled = true;
        ddlMediaFolder.SelectedValue="0";
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

    private void FillDropDowns()
    {
        try
        {
            ddlSearchCompany.DataSource = new CompanyRepository().GetAllCompany();
            ddlSearchCompany.DataValueField = "CompanyID";
            ddlSearchCompany.DataTextField = "CompanyName";
            ddlSearchCompany.DataBind();
            ddlSearchCompany.Items.Insert(0, new ListItem("-Company-", "0"));
            //if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
           // {
               
                //ddlSearchCompany.Enabled = false;
           // }
            LookupRepository objLookRep = new LookupRepository();

            // For Workgroup
            ddlSearchWorkGroup.DataSource = objLookRep.GetByLookup("Workgroup").OrderBy(s => s.sLookupName).ToList();
            ddlSearchWorkGroup.DataValueField = "iLookupID";
            ddlSearchWorkGroup.DataTextField = "sLookupName";
            ddlSearchWorkGroup.DataBind();
            ddlSearchWorkGroup.Items.Insert(0, new ListItem("-Workgroup-", "0"));

            // For Status
            ddlSearchStatus.DataSource = objLookRep.GetByLookup("Status").OrderBy(s => s.sLookupName).ToList();
            ddlSearchStatus.DataValueField = "iLookupID";
            ddlSearchStatus.DataTextField = "sLookupName";
            ddlSearchStatus.DataBind();
            ddlSearchStatus.Items.Insert(0, new ListItem("-Status-", "0"));
            LookupRepository objLookupRepo = new LookupRepository();
            List<INC_Lookup> lstStatuses = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Status).OrderBy(le => le.sLookupName).ToList();
            this.ActiveID = lstStatuses.FirstOrDefault(le => le.sLookupName == "Active").iLookupID;
            this.InActiveID = lstStatuses.FirstOrDefault(le => le.sLookupName == "InActive").iLookupID;


            // Assigned media popup drop down

            MediaRepository OBJ = new MediaRepository();
            List<MediaFolder> objList = new List<MediaFolder>();
            objList = OBJ.GetAllFolder();
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

          

            List<MediaLoginpopup> lstloginpopup = new List<MediaLoginpopup>();
            lstloginpopup = OBJ.GetLoginpopuplist();
            DataTable dtloginpopup = Common.ListToDataTable(lstloginpopup);
            this.cbPlaceForLogin.DataSource = dtloginpopup;
            this.cbPlaceForLogin.DataTextField = "ModuleName";
            this.cbPlaceForLogin.DataValueField = "MediaLoginpopupid";
            this.cbPlaceForLogin.DataBind();
            
            List<MediaPlaceMent> lstplacement = OBJ.GetPlacement();
            BindDropDown(ddlPlacement, lstplacement, "PlacementName", "MediaPlaceMentId", "Placement", "0");


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




            
            List<INC_Lookup> lstWorkGroups = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Workgroup).OrderBy(le => le.sLookupName).ToList();
            //BindDropDown(ddlWorkGroup, lstWorkGroups, "sLookupName", "iLookupID", "Workgroup", "0");
            DataTable dt1 = Common.ListToDataTable(lstWorkGroups);
            this.cbWorkGroup.DataSource = dt1;
            this.cbWorkGroup.DataTextField = "sLookupName";
            this.cbWorkGroup.DataValueField = "iLookupID";
            this.cbWorkGroup.DataBind();

            this.CBWorkgroupForAssignImage.DataSource = dt1;
            this.CBWorkgroupForAssignImage.DataTextField = "sLookupName";
            this.CBWorkgroupForAssignImage.DataValueField = "iLookupID";
            this.CBWorkgroupForAssignImage.DataBind();



            CatogeryRepository objCatRepository = new CatogeryRepository();
            List<Category> objCatlist = new List<Category>();
            objCatlist = objCatRepository.GetAllCategory();
            if (objCatlist.Count != 0)
            {
                ddlProductCategory.DataSource = objCatlist;
                ddlProductCategory.DataValueField = "CategoryID";
                ddlProductCategory.DataTextField = "CategoryName";
                ddlProductCategory.DataBind();
                ddlProductCategory.Items.Insert(0, new ListItem("-Select-", "0"));
            }




        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
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



    private void BindGrid(Boolean isForSorting)
    {
        try
        {
            MediaRepository MediaRepo = new MediaRepository();
            List<GetMediaAssignedSearchResult> lstMediaAssgined = MediaRepo.GetMediaAssignedSearch(this.CompanyID, this.WorkGroupID, this.KeyWord, this.MediaStatusID, this.NoOfRecordsToDisplay, this.PageIndex, this.SortColumn, this.SortDirection);

            if (lstMediaAssgined != null && lstMediaAssgined.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lstMediaAssgined[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;
            }

            gvMediaSearch.DataSource = lstMediaAssgined;
            gvMediaSearch.DataBind();

            if (!isForSorting)
            {
                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 1;
                lnkNext.Enabled = this.PageIndex < this.TotalPages;
            }

            if (gvMediaSearch.Rows.Count > 0)
            {
                totalcount_em.InnerText = lstMediaAssgined[0].TotalRecords + " results";
                pagingtable.Visible = true;
            }
            else
            {
                totalcount_em.InnerText = "no results";
                pagingtable.Visible = false;
            }

            totalcount_em.Visible = true;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }

    public bool GetStatus(string statusid)
    {
        
        if (!string.IsNullOrEmpty(statusid))
        {
            if (Convert.ToInt64(statusid) == this.ActiveID)
                return true;
            else
                return false;
        }
        else
        {
            return false;
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


  

    #endregion
}
