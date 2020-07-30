using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class index : PageBase
{
    #region Local Property

    MenuPrivilegeRepository objMenuPriviledgeRep = new MenuPrivilegeRepository();

    //global object for the store repository
    CompanyStoreRepository objStoreRep = new CompanyStoreRepository();

    //global generic list for company employee repository
    CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
    Boolean ShowSupportTicketPopup = true;

    int totalTopNavigationItems = 0;
    int totalSecondTopNavigationItems = 0;
    int totalAddiInfo = 0;
    int totalRepeaterData = 0;
    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Remove("OrderCompleted");
            Session.Remove("OrderFailed");

            ((Label)Master.FindControl("lblPageHeading")).Text = "Home Page";
            ((HtmlImage)Master.FindControl("imgHomeBtn")).Visible = false;
            
            // Only Declare Session for ProductList page.  This is used when we go to my cart.
            Session["ProductListUrl"] = null;

            #region If Admin has landed here from the store listing then following condition will come true.
            if (Convert.ToString(Request.QueryString.Get("id")) == "true")
            {
                IncentexGlobal.IsIEFromStore = true;
                IncentexGlobal.IndexPageLink = Request.Url.ToString();
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((LinkButton)Master.FindControl("btnLogout")).Visible = false;

                #region Core Logic for Changing session and One Level Up functionality for the IE

                //I m changing the user session here(So admin can also view the same pages as customer employee or admin)..
                //Admin users session is null then n then only assigning values to it otherwise it will overrite the session..
                if (IncentexGlobal.AdminUser == null)
                {
                    IncentexGlobal.AdminUser = IncentexGlobal.CurrentMember;
                }
                UserInformationRepository objUIR = new UserInformationRepository();
                UserInformation objUI = new UserInformation();
                objUI = objUIR.GetById(Convert.ToInt64(Request.QueryString["subid"]));
                IncentexGlobal.CurrentMember = objUI;

                //Below line is for the CA View
                if (IncentexGlobal.AdminUser.Usertype == 3)
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/UserManagement.aspx?IsFromStoreFront=1";
                }
                //For the IE Viewing
                else
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Index.aspx?IsFromStoreFront=1";
                }

                #endregion
                //End

                //Get Top menus for user based on the access given from admin panel
                BindTopNavigationForCompanyAdmin();

                BindTopNavigationForCompanyEmployeeAndCompanyAdmin();

                BindCategoryRepeaterControl();

                BindSplashImage((long)IncentexGlobal.CurrentMember.CompanyId, IncentexGlobal.CurrentMember.UserInfoID);

                BindGuidelineManuals((long)IncentexGlobal.CurrentMember.CompanyId, IncentexGlobal.CurrentMember.UserInfoID);
            }
            #endregion

            #region If User has logged in them selves from login page then following condition will execute.
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "   ";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "";

                //Get Top menus for user based on the access given from admin panel
                BindTopNavigationForCompanyAdmin();

                BindTopNavigationForCompanyEmployeeAndCompanyAdmin();

                BindCategoryRepeaterControl();

                BindSplashImage((long)IncentexGlobal.CurrentMember.CompanyId, IncentexGlobal.CurrentMember.UserInfoID);

                BindGuidelineManuals((long)IncentexGlobal.CurrentMember.CompanyId, IncentexGlobal.CurrentMember.UserInfoID);

                //for display general video popup if available
                if (Request.UrlReferrer != null && Request.UrlReferrer.AbsoluteUri.ToLower().Contains("login.aspx") && objEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).IsGeneralVideoDisplay == false)
                {
                    VideoTraining objVideoTraining = new VideoTraining();
                    objVideoTraining = new VideoTrainingRepository().GetVideoByStoreIDWorkgroupIDUserTypeAndUrl(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.Usertype, Request.Url.ToString());
                    if (objVideoTraining != null)
                    {
                        VideoControlIndexPage.Visible = true;
                        VideoControlIndexPage.VideoTitle = objVideoTraining.VideoTitle;
                        if (!string.IsNullOrEmpty(objVideoTraining.VideoName))
                            VideoControlIndexPage.VideoName = objVideoTraining.VideoName;
                        else
                            VideoControlIndexPage.VideoYouTubID = objVideoTraining.YouTubeVideoID;

                        //update that general video is shown by user
                        CompanyEmployee objCompanyEmployee = new CompanyEmployee();
                        objCompanyEmployee = objEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                        objCompanyEmployee.IsGeneralVideoDisplay = true;
                        objEmpRepo.SubmitChanges();
                    }
                }

                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["st"])) && ShowSupportTicketPopup)
                {
                    if (Convert.ToString(Request.QueryString["st"]) == "1")
                    {
                        ostCAControl.Visible = true;
                        Session["ostCAControl"] = true;
                    }
                    else if (Convert.ToString(Request.QueryString["st"]) == "2")
                    {
                        ostCEControl.Visible = true;
                        Session["ostCEControl"] = true;
                    }
                    else if (Convert.ToString(Request.QueryString["st"]) == "3")
                    {
                        ostAnnControl.Visible = true;
                        Session["ostAnnControl"] = true;
                    }

                    ShowSupportTicketPopup = false;
                }
                else if (Convert.ToString(Request.QueryString["ru"]) == "1" && IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    ruCAControl.Visible = true;
                    Session["ruCAControl"] = true;
                }
            }
            #endregion
        }
    }

    #endregion

    #region DataList Events

    #region rpuGuideLineManuals

    protected void rpuGuideLineManuals_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string[] filevalue = ((Label)e.Item.FindControl("lblFileName")).Text.ToString().Split('_');
            string[] onlyname = filevalue[4].ToString().Split('.');
            ((LinkButton)e.Item.FindControl("lnkrpuGuideLineManuals")).Text = onlyname[0].ToString();
        }
    }

    protected void rpuGuideLineManuals_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "viewguidelinemanual")
        {

            string file = e.CommandArgument.ToString();
            string filePath = Server.MapPath("UploadedImages/CompanyStoreDocuments/");
            string strFullPath = filePath + ((Label)e.Item.FindControl("lblFileName")).Text;
            string[] filevalue = ((Label)e.Item.FindControl("lblFileName")).Text.ToString().Split('_');
            DownloadFile(strFullPath, filevalue[4].ToString());
        }

    }

    #endregion

    #region TopMenu

    protected void topNavigation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemIndex == totalTopNavigationItems - 1)
        {
            HtmlGenericControl lis = ((HtmlGenericControl)e.Item.FindControl("lis"));
            lis.Attributes.Add("class", "last");
        }
    }

    protected void topNavigation_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }

    protected void topSecondNavigation_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
    protected void topSecondNavigation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemIndex == totalSecondTopNavigationItems - 1)
        {
            HtmlGenericControl lis = ((HtmlGenericControl)e.Item.FindControl("lis"));
            lis.Attributes.Add("class", "last");
        }
    }

    #endregion

    #region Location Repeater Control Events

    protected void rptLocation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemIndex == totalRepeaterData - 1)
        {
            HtmlGenericControl lis = ((HtmlGenericControl)e.Item.FindControl("liLocation"));
            lis.Attributes.Add("class", "last");
        }
    }
    protected void rptLocation_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "ListProduct":
                Response.Redirect("~/Products/ProductList.aspx?SubCat=" + e.CommandArgument.ToString());
                break;
        }
    }

    #endregion

    #region rptAddiInfo

    protected void rptAddiInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemIndex == totalAddiInfo - 1)
        {
            HtmlGenericControl lis = ((HtmlGenericControl)e.Item.FindControl("AddiInfolis"));
            lis.Attributes.Add("class", "last");
        }
    }

    /// <summary>
    /// Handles the ItemCommand event of the rptAddiInfo control.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterCommandEventArgs"/> instance containing the event data.</param>
    protected void rptAddiInfo_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandArgument.ToString())
        {
            case "Recent News  ":
                Session["UserInfoID"] = IncentexGlobal.CurrentMember.UserInfoID.ToString();
                Session["CompanyID"] = IncentexGlobal.CurrentMember.CompanyId.ToString();
                Response.Redirect("~/CompanyStore/ViewNews.aspx"); //?u=" + Session["UserInfoID"] + "&c=" + Session["CompanyID"]);
                break;
            case "Contact Us":
                Session["UserInfoID"] = IncentexGlobal.CurrentMember.UserInfoID.ToString();
                Session["CompanyID"] = IncentexGlobal.CurrentMember.CompanyId.ToString();
                Response.Redirect("~/CompanyStore/ViewContact.aspx"); //?u=" + Session["UserInfoID"] + "&c=" + Session["CompanyID"]);
                break;
            case "FAQ's":
                Session["UserInfoID"] = IncentexGlobal.CurrentMember.UserInfoID.ToString();
                Session["CompanyID"] = IncentexGlobal.CurrentMember.CompanyId.ToString();
                Response.Redirect("~/CompanyStore/ViewFAQ.aspx"); //?u=" + Session["UserInfoID"] + "&c=" + Session["CompanyID"]);
                break;
            case "New Products  ":
                Response.Redirect("~/Products/NewProductList.aspx");
                break;
            case "Specials  ":
                Response.Redirect("~/Products/SaleProductList.aspx");
                break;
            case "Video Podcast":
                Response.Redirect("~/CompanyStore/VideoPodcast.aspx");
                break;
            case "Closeouts":
                Response.Redirect("~/Products/ProductList.aspx?IsCloseOut=1");
                break;
            case "Replacement Uniforms":
                Response.Redirect("~/index.aspx?ru=1");
                break;
            case "Employee Status Change":
                Response.Redirect("~/UserStatusChange/ChangeStatus.aspx?");
                break;
        }
    }

    #endregion

    #region lnkButton

    protected void lnkBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/CompanyStore/ViewCompanyStore.aspx");
    }
    protected void lnkBtnLogin_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/login.aspx");
    }

    #endregion

    #endregion

    #region Other Functions
    protected void BindTopNavigationForCompanyAdmin()
    {
        //Code for the second top naviagation
        List<SelectFrontSecondMenuByCompanyEmployeeResult> objSecondTopMenuList = new List<SelectFrontSecondMenuByCompanyEmployeeResult>();
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
        {
            objSecondTopMenuList = objMenuPriviledgeRep.GetSecondTopMenusByUserInformationId(IncentexGlobal.CurrentMember.UserInfoID);
            totalSecondTopNavigationItems = objSecondTopMenuList.Count;
            if (totalSecondTopNavigationItems == 0)
            {
                dvSecondTopNavigation.Visible = false;
            }
            else
            {
                dvSecondTopNavigation.Visible = true;
            }

            //Added by Devraj on 07th March, 2013 for hiding pending users button in ATS store, as requested by Kim Hurst.
            if (IncentexGlobal.CurrentMember.CompanyId != 13)//13 is the companyid of ATS in the live database.
            {
                //Updated by Prashant on May-June 2013 for display total pending user as per new logic
                CompanyEmployeeRepository objCompanyEmployeeRepos = new CompanyEmployeeRepository();
                Int64 pendingUserCount = objCompanyEmployeeRepos.GetPendingUsersList(IncentexGlobal.CurrentMember.UserInfoID).ToList().Count;
                ((HyperLink)Master.FindControl("lnkPendingUsers")).Text = "<span>" + pendingUserCount + "</span>";
            }
            else
                ((HyperLink)Master.FindControl("lnkPendingUsers")).Visible = false;
        }
        else
        {
            dvSecondTopNavigation.Visible = false;
        }
        topSecondNavigation.DataSource = objSecondTopMenuList;
        topSecondNavigation.DataBind();
    }

    protected void BindTopNavigationForCompanyEmployeeAndCompanyAdmin()
    {
        List<SelectFrontMenuByCompanyEmployeeResult> objTopMenuList = objMenuPriviledgeRep.GetMenusByUserInformationId(IncentexGlobal.CurrentMember.UserInfoID);

        //Function that will keep "My Cart" Last in the List..
        List<SelectFrontMenuByCompanyEmployeeResult> objTopMenuCustomizedList = SortList(objTopMenuList);
        totalTopNavigationItems = objTopMenuCustomizedList.Count;

        if (totalTopNavigationItems == 0)
            dvTopNavigation.Visible = false;
        else
            dvTopNavigation.Visible = true;
        topNavigation.DataSource = objTopMenuCustomizedList;
        topNavigation.DataBind();

        HyperLink hlRecentlyUpdatedTickets = ((HyperLink)Master.FindControl("hlRecentlyUpdatedTickets"));
        hlRecentlyUpdatedTickets.Visible = true;
        Int32 UpdatedTicketCount = new ServiceTicketRepository().GetServiceTicketUpdatedCountCACE(IncentexGlobal.CurrentMember.UserInfoID);
        hlRecentlyUpdatedTickets.Text = "<span>" + UpdatedTicketCount + "</span>";
    }

    protected void BindCategoryRepeaterControl()
    {
        //Get addition information for user based on the access given from admin panel
        List<SelectAddiInfoAccessByCompanyEmployeeResult> objAddiInfo = objMenuPriviledgeRep.GetAddiInfoByUserInformationId(IncentexGlobal.CurrentMember.UserInfoID);
        totalAddiInfo = objAddiInfo.Count;
        if (totalAddiInfo == 0)
            dvAdditionalInfo.Visible = false;
        else
        {
            dvAdditionalInfo.Visible = true;
            rptAddiInfo.DataSource = objAddiInfo;
            rptAddiInfo.DataBind();
        }

        List<StoreCategoryLocationRepository.StoreCategoryLocationResults> objStoreCategoryLocationResults = new StoreCategoryLocationRepository().GetLocationByStoreID(objStoreRep.GetStoreIDByCompanyId((Int32)IncentexGlobal.CurrentMember.CompanyId));
        foreach (StoreCategoryLocationRepository.StoreCategoryLocationResults item in objStoreCategoryLocationResults)
        {
            List<GetUserAssignedSubCategoryByCategoryIDAndUserInfoIDResult> objGetUserAssignedSubCategoryByCategoryIDAndUserInfoID = objMenuPriviledgeRep.GetUserAssignedSubCategoryByCategoryIDAndUserInfoID(IncentexGlobal.CurrentMember.UserInfoID, item.CategoryID);
            totalRepeaterData = objGetUserAssignedSubCategoryByCategoryIDAndUserInfoID.Count;
            if (item.Location == "A")
            {
                if (totalRepeaterData == 0)
                {
                    dvLocationAEmpty.Visible = true;
                    dvLocationA.Visible = false;
                }
                else
                {
                    dvLocationAEmpty.Visible = false;
                    dvLocationA.Visible = true;
                    spnLocationA.InnerText = item.CategoryName;
                    rptLocationA.DataSource = objGetUserAssignedSubCategoryByCategoryIDAndUserInfoID;
                    rptLocationA.DataBind();
                }
            }
            else if (item.Location == "B")
            {
                if (totalRepeaterData == 0)
                    dvLocationB.Visible = false;
                else
                {
                    dvLocationB.Visible = true;
                    spnLocationB.InnerText = item.CategoryName;
                    rptLocationB.DataSource = objGetUserAssignedSubCategoryByCategoryIDAndUserInfoID;
                    rptLocationB.DataBind();
                }
            }
            else if (item.Location == "C")
            {
                if (totalRepeaterData == 0)
                    dvLocationC.Visible = false;
                else
                {
                    dvLocationC.Visible = true;
                    spnLocationC.InnerText = item.CategoryName;
                    rptLocationC.DataSource = objGetUserAssignedSubCategoryByCategoryIDAndUserInfoID;
                    rptLocationC.DataBind();
                }
            }
            else if (item.Location == "D")
            {
                if (totalRepeaterData == 0)
                    dvLocationD.Visible = false;
                else
                {
                    dvLocationD.Visible = true;
                    spnLocationD.InnerText = item.CategoryName;
                    rptLocationD.DataSource = objGetUserAssignedSubCategoryByCategoryIDAndUserInfoID;
                    rptLocationD.DataBind();
                }
            }
        }
        //Visible false div if location is not assigned from admin panel
        if (rptLocationA.Items.Count == 0)
        {
            dvLocationAEmpty.Visible = true;
            dvLocationA.Visible = false;
        }
        if (rptLocationB.Items.Count == 0)
            dvLocationB.Visible = false;
        if (rptLocationC.Items.Count == 0)
            dvLocationC.Visible = false;
        if (rptLocationD.Items.Count == 0)
            dvLocationD.Visible = false;
    }

    /// <summary>
    /// //Get splash image by Users workgroup and generate 
    /// a string builder which will be assigning to the div so that image can be rotate one by one
    /// </summary>
    /// <param name="CompanyID">The company ID.</param>
    /// <param name="UserInfoID">The user info ID.</param>
    protected void BindSplashImage(Int64 CompanyID, Int64 UserInfoID)
    {
        StringBuilder sHtml = new StringBuilder("<ul>");
        List<SelectSplashImagesByCompanyEmployeeResult> objList = objStoreRep.GetSplashImagesByCompanyEmployee(CompanyID, UserInfoID);
        for (int i = 0; i < objList.Count; i++)
        {
            if (i == 0)
            {
                sHtml = new StringBuilder(sHtml + "<li class='show'><img src='UploadedImages/CompanyStoreDocuments/" + objList[i].DocumentName + "' height='520' width='500'  alt='splashimage" + i + "'/></li>");
            }
            else
            {
                sHtml = new StringBuilder(sHtml + "<li><img src='UploadedImages/CompanyStoreDocuments/" + objList[i].DocumentName + "' height='520' width='500' alt='splashimage" + i + "'/></li>");
            }
        }
        sHtml = new StringBuilder(sHtml + "</ul>");
        rotator.InnerHtml = sHtml.ToString();
    }

    /// <summary>
    /// Binds the guideline manuals.
    /// </summary>
    /// <param name="CompanyID">The company ID.</param>
    /// <param name="UserInfoID">The user info ID.</param>
    protected void BindGuidelineManuals(Int64 CompanyID, Int64 UserInfoID)
    {
        List<SelectGuidelineByCompanyEmployeeResult> objGuideLineList = objStoreRep.GetGuideLinesManualByCompanyEmployee((long)IncentexGlobal.CurrentMember.CompanyId, (long)IncentexGlobal.CurrentMember.UserInfoID);
        rpuGuideLineManuals.DataSource = objGuideLineList;
        rpuGuideLineManuals.DataBind();
    }
    protected void DownloadFile(string filepath, string displayFileName)
    {
        System.IO.Stream iStream = null;
        string type = "";
        string ext = Path.GetExtension(filepath);


        // Buffer to read 10K bytes in chunk:
        byte[] buffer = new Byte[10000];

        // Length of the file:
        int length;

        // Total bytes to read:
        long dataToRead;

        // Identify the file name.
        string filename = System.IO.Path.GetFileName(filepath);

        try
        {
            // Open the file.
            iStream = new System.IO.FileStream(filepath, System.IO.FileMode.Open,
            System.IO.FileAccess.Read, System.IO.FileShare.Read);


            // Total bytes to read:
            dataToRead = iStream.Length;
            if (ext != null)
            {
                switch (ext.ToLower())
                {
                    case ".htm":
                    case ".html":
                        type = "text/HTML";
                        break;
                    case ".txt":
                        type = "text/plain";
                        break;
                    case ".pdf":
                        type = "Application/pdf";
                        break;
                    case ".mp3":
                        type = "audio/mpeg";
                        break;
                    case ".wmi":
                        type = "audio/basic";
                        break;
                    case ".dat":
                    case ".mpeg":
                    case "mpg":
                        type = "video/mpeg";
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".gif":
                    case ".tiff":
                        type = "image/gif";
                        break;
                    case ".png":
                        type = "image/png";
                        break;
                    case ".doc":
                    case ".rtf":
                        type = "Application/msword";
                        break;
                    case ".wav":
                        type = "audio/x-wav";
                        break;
                    case "bmp":
                        type = "image/bmp";
                        break;
                    case "flv":
                        type = "video/x-flv";
                        break;
                    case "docx":
                        type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case "ppt":
                        type = "application/vnd.ms-powerpoint";
                        break;
                    case "pptx":
                        type = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        break;
                    case "avi":
                        type = "video/x-msvideo";
                        break;
                    case "wmv":
                        type = "video/x-ms-wmv";
                        break;
                    case "wma":
                        type = "/audio/x-ms-wma";
                        break;

                    //left:--rm
                }
            }


            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + displayFileName + "\"");
            Response.ContentType = type;
            Response.WriteFile(filepath);
            Response.End();


        }
        catch (Exception ex)
        {
            SetIsErrorOccur(true);
            // Trap the error, if any.
            Response.Write("Error : " + ex.Message);
        }
        finally
        {
            if (iStream != null)
            {
                //Close the file.
                iStream.Close();
            }
        }
    }

    //Below function is added to Keep the "My Cart" at the last place in the navigation bar
    public List<SelectFrontMenuByCompanyEmployeeResult> SortList(List<SelectFrontMenuByCompanyEmployeeResult> list)
    {

        List<SelectFrontMenuByCompanyEmployeeResult> a = new List<SelectFrontMenuByCompanyEmployeeResult>();
        List<SelectFrontMenuByCompanyEmployeeResult> b = new List<SelectFrontMenuByCompanyEmployeeResult>();
        foreach (SelectFrontMenuByCompanyEmployeeResult Main in list)
        {
            if (Main.sDescription == "My Cart")
            {
                a.Add(Main);
            }
            else
            {
                b.Add(Main);
            }
        }
        b.AddRange(a);

        return b;
    }
    #endregion
}
