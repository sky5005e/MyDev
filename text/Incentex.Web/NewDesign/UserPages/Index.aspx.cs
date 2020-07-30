using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;

public partial class UserPages_Index : PageBase
{
    #region Properties

    Int64 StatusActive
    {
        get
        {
            return Convert.ToInt64(ViewState["StatusActive"]);
        }
        set
        {
            ViewState["StatusActive"] = value;
        }
    }

    private Int32 PageCount
    {
        get
        {
            return Convert.ToInt32(ViewState["PageCount"]);
        }
        set
        {
            ViewState["PageCount"] = value;
        }
    }

    /// <summary>
    /// Used to display last column in repeater bind
    /// </summary>
    Int32 rowCountIndex
    {
        get
        {

            return Convert.ToInt32(ViewState["rowCountIndex"]);
        }
        set
        {
            ViewState["rowCountIndex"] = value;
        }
    }

    Int32 SubCategoryID
    {
        get
        {
            return Convert.ToInt32(ViewState["SubCategoryID"]);
        }
        set
        {
            ViewState["SubCategoryID"] = value;
        }
    }

    String SiteURL
    {
        get
        {
            return Convert.ToString(ConfigurationSettings.AppSettings["siteurl"]);
        }
    }

    Boolean IsFirstSubCatIDSet
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsFirstSubCatIDSet"]);
        }
        set
        {
            ViewState["IsFirstSubCatIDSet"] = value;
        }
    }
    /// <summary>
    /// Set this value when view all is clicked.
    /// </summary>
    private Boolean IsViewAllTotal
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsViewAllTotal"]);
        }
        set
        {
            ViewState["IsViewAllTotal"] = value;
        }
    }

    ProductItemDetailsRepository objProductItemRepo = new ProductItemDetailsRepository();

    #endregion

    #region Fields

    private List<GetUserCategoryAccessResult> lstUserCategories = new List<GetUserCategoryAccessResult>();

    #endregion

    #region Page Events

    protected void Page_Load(Object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                #region If Admin has landed here from the store listing then following condition will come true.
                if (Convert.ToString(Request.QueryString.Get("id")) == "true")
                {
                    IncentexGlobal.IsIEFromStore = true;
                    IncentexGlobal.IndexPageLink = Request.Url.ToString();
                    //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                    //((LinkButton)Master.FindControl("btnLogout")).Visible = false;

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
                    ((LinkButton)Master.FindControl("lnklogOut")).PostBackUrl = "~/NewDesign/UserPages/Index.aspx?IsFromStoreFront=1";

                    #endregion
                    //End

                    //Get Top menus for user based on the access given from admin panel
                }
                #endregion

                #region Change session for viewing WLS storefront

                //Check if admin is back from the store front this is done first because of redirection
                if (Request.QueryString["IsFromStoreFront"] != null)
                {
                    if (Request.QueryString.Get("IsFromStoreFront").ToString() == "1")
                    {
                        IncentexGlobal.IsIEFromStore = false;
                        IncentexGlobal.IsIEFromStoreTestMode = false;
                        IncentexGlobal.CurrentMember = null;
                        if (IncentexGlobal.AdminUser != null)
                        {
                            IncentexGlobal.CurrentMember = IncentexGlobal.AdminUser;
                            IncentexGlobal.AdminUser = null;
                            IncentexGlobal.IndexPageLink = null;
                        }
                        else
                        {
                            Response.Redirect("~/login.aspx");
                        }
                    }
                }
                #endregion

                Session.Remove("OrderCompleted");
                Session.Remove("OrderFailed");

                StatusActive = Convert.ToInt64(new LookupRepository().GetIdByLookupNameNLookUpCode("Active", "Status"));
                hdnUserType.Value = Incentex.DAL.Common.DAEnums.GetUserTypeName(IncentexGlobal.CurrentMember.Usertype);
                BindCategoryAccessRepeater();
                BindProductRepeater();
                BindIssuancePolicies();
                ShowPopupWhenLoginFirst();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }



    #endregion

    #region Methods

    private void BindProductRepeater()
    {
        try
        {
            DataView myDataView = new DataView();
            Int64? EmployeeTypeID = null;

            CompanyEmployee objCmpnyInfo = new CompanyEmployee();
            String GarmentType = String.Empty;

            CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
            objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

            if (objCmpnyInfo.EmployeeTypeID != null)
                EmployeeTypeID = objCmpnyInfo.EmployeeTypeID;

            INC_Lookup objLookup = new LookupRepository().GetById(objCmpnyInfo.GenderID);

            if (objLookup.sLookupName == "Male")
                GarmentType = "Male";
            else
                GarmentType = "Female";

            List<GetStoreProductsForUserResult> objProductList = objProductItemRepo.GetStoreProductsForUser(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), SubCategoryID, GarmentType, IncentexGlobal.CurrentMember.Usertype, EmployeeTypeID, IncentexGlobal.CurrentMember.ClimateSettingId).Where(p => p.IsCloseOut == false).ToList();

            DataTable dataTable = Common.ListToDataTable(objProductList);
            myDataView = dataTable.DefaultView;

            if (this.ViewState["SortExp"] != null)
                myDataView.Sort = Convert.ToString(this.ViewState["SortExp"]) + " " + Convert.ToString(this.ViewState["SortOrder"]);

            pds.DataSource = myDataView;
            pds.AllowPaging = true;

            if (IsViewAllTotal)
            {
                CurrentPage = 0;
                pds.PageSize = objProductList.Count;
            }
            else
                pds.PageSize = 9;// For Product Page
            pds.CurrentPageIndex = CurrentPage;
            lnkbtnNext.Enabled = !pds.IsLastPage;
            lnkbtnPrevious.Enabled = !pds.IsFirstPage;
            lnkbtnBottomNext.Enabled = !pds.IsLastPage;
            lnkbtnBottomPrevious.Enabled = !pds.IsFirstPage;
            rptProductList.DataSource = pds;
            rptProductList.DataBind();
            doPaging();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void BindCategoryAccessRepeater()
    {
        try
        {
            lstUserCategories = new CompanyEmployeeRepository().GetUserCategoryAccess(IncentexGlobal.CurrentMember.UserInfoID).OrderBy(le => le.CategoryName).ToList();
            rptCategories.DataSource = lstUserCategories.Select(le => new { CategoryID = le.CategoryID, CategoryName = le.CategoryName }).Distinct().OrderBy(le => le.CategoryID).ToList();
            rptCategories.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Bind Size drop down List
    /// </summary>
    /// <param name="ddlsize"></param>
    /// <param name="StoreProductid"></param>
    /// <param name="listProductItem"></param>
    private void BindddlSize(DropDownList ddlsize, IEnumerable<GetProductItemDetailsByStoreProductIDResult> listProductItem)
    {
        if (ddlsize != null)
        {
            try
            {
                ddlsize.DataSource = listProductItem;
                ddlsize.DataTextField = "ItemSize";
                ddlsize.DataValueField = "ItemSizeID";
                ddlsize.DataBind();

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
        }
    }

    /// <summary>
    /// Get Price Amount
    /// </summary>
    /// <param name="objprod"></param>
    /// <returns></returns>
    private String GetPriceAmount(GetProductItemDetailsByStoreProductIDResult objprod)
    {
        String Amount = String.Empty;
        // IF Product is Close out Product and Set True to IsCloseOutProduct 
        if ((Boolean)objprod.IsCloseOut)
            Amount = Convert.ToString(objprod.CloseOutPrice);
        else if (objprod.Level3PricingStatus != null && objprod.Level3PricingStatus == StatusActive && DateTime.Now.Date <= objprod.Level3PricingEndDate && DateTime.Now.Date >= objprod.Level3PricingStartDate)
            Amount = Convert.ToString(objprod.Level3PricingStatus);
        else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            Amount = Convert.ToString(objprod.Level1);
        else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
            Amount = Convert.ToString(objprod.Level2);
        else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
            Amount = Convert.ToString(objprod.Level4);

        return Amount;
    }

    private void BindIssuancePolicies()
    {
        try
        {
            List<GetIssuancePoliciesByUserInfoIDResult> lstPolicies = new UniformIssuancePolicyRepository().GetIssuancePolicyByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);

            repIssuancePolicies.DataSource = lstPolicies;
            repIssuancePolicies.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
   
    void ShowPopupWhenLoginFirst()
    {
       // if(IncentexGlobal.CurrentMember.Usertype)
        if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(DAEnums.UserTypes.SuperAdmin) && IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(DAEnums.UserTypes.IncentexAdmin))
        {
            WSUser objuser = new WSUser();
            MediaRepository objMedia = new MediaRepository();
            GetMediaHelpVideoOrDocResult ObjMyFirstVideo = objMedia.GetMediaHelpVideoOrDocResultWhenLoginFirst("login pop-up", "login pop-up", "When Login First", Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
            hfMediaVideoId.Value = ObjMyFirstVideo == null ? "0" : Convert.ToString(ObjMyFirstVideo.mediafileid);
            HfUrlOfMyFirstVideo.Value = ObjMyFirstVideo == null ? "" : ObjMyFirstVideo.Vimeourl;
            if (!objMedia.IsVideoViewedWhenLoginFirst(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(hfMediaVideoId.Value)) && ObjMyFirstVideo != null)
            {
                if (Session["MediaVideoViewStatus"] == null)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowPopupWhenLoginFirst();", true);
                    Session["MediaVideoViewStatus"] = "Viewed";
                }
            }
        }
    }
    #endregion

    #region Events

    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (cbDontShowAgain.Checked == true)
        {
            MediaRepository objMediaRepos = new MediaRepository();
            MediaViewed objMediaViewed = new MediaViewed();
            objMediaViewed.MediaId = Convert.ToInt64(hfMediaVideoId.Value);
            objMediaViewed.Isview = true;
            objMediaViewed.UserId = IncentexGlobal.CurrentMember.UserInfoID;
            objMediaViewed.ViewTime = System.DateTime.Now;
            //  objMediaRepos.ins
            objMediaRepos.Insert(objMediaViewed);
            objMediaRepos.SubmitChanges();

        }
    }

    protected void rptSubCategories_ItemCommand(Object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("display"))
        {
            this.CurrentPage = 0;
            this.FrmPg = 0;
            this.ToPg = 0;
            this.SubCategoryID = Convert.ToInt16(e.CommandArgument.ToString());
            HiddenField hdnCategoryName = (HiddenField)e.Item.FindControl("hdnCategoryName");
            LinkButton lnkSubCategory = (LinkButton)e.Item.FindControl("lnkSubCategory");
            hdnExpandedAccordion.Value = lnkSubCategory.ClientID;
            lblSelectionTitle.Text = lnkSubCategory.Text;

            BindProductRepeater();
            WSUser objWService = new WSUser();
            if (!string.IsNullOrEmpty(objWService.GetHelpVideoOrDocLink("document link", "" + hdnCategoryName.Value + "", "" + lnkSubCategory.Text + "")))
            {
                atagGuideline.Attributes.Add("onclick", "GetDocumentLink('" + hdnCategoryName.Value + "','" + lnkSubCategory.Text + "')");
                atagGuideline.Attributes["target"] = "_blank";
                //atagGuideline.Attributes.Add("ontouchstart", "GetDocumentLink('" + hdnCategoryName.Value + "','" + lnkSubCategory.Text + "')");
                DivGuideline.Visible = true;
            }
            else
            {
                DivGuideline.Visible = false;
            }
        }
    }
    protected void lbPlayPubVideo_Click(object sender, EventArgs e)
    {
        MediaRepository objmedia = new MediaRepository();
        MediaFile objfile = objmedia.GetFileById(Convert.ToInt64(hfMediaVideoId.Value));
        if (objfile != null)
        {
            objfile.View = objfile.View == null ? 1 : objfile.View + 1;
            objmedia.SubmitChanges();
        }

        iframepubvideo.Visible = true;
        iframepubvideo.Attributes.Add("src", "" + HfUrlOfMyFirstVideo.Value + "");
        //iframepubvideo.Attributes.Add("src", "http://player.vimeo.com/v2/video/79763545");
        lbPlayPubVideo.Visible = false;
        divvidimage.Visible = false;
       // OpenPopup("add-publish-block", "dmsvideo-content");
        divShowPubVideo.Visible = true;
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowPopupWhenLoginFirst();", true);
    }

    protected void rptSubCategories_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnSubCategoryID = (HiddenField)e.Item.FindControl("hdnSubCategoryID");
                LinkButton lnkSubCategory = (LinkButton)e.Item.FindControl("lnkSubCategory");
                HiddenField hdnCategoryName = (HiddenField)e.Item.FindControl("hdnCategoryName");
                if (!String.IsNullOrEmpty(hdnSubCategoryID.Value) && !IsFirstSubCatIDSet)
                {
                    this.SubCategoryID = Convert.ToInt32(hdnSubCategoryID.Value);
                    hdnExpandedAccordion.Value = lnkSubCategory.ClientID;
                    lblSelectionTitle.Text = lnkSubCategory.Text;
                    IsFirstSubCatIDSet = true;

                    WSUser objWService = new WSUser();
                    if (!string.IsNullOrEmpty(objWService.GetHelpVideoOrDoc("document link", "" + hdnCategoryName.Value + "", "" + lnkSubCategory.Text + "")))
                    {
                        atagGuideline.Attributes.Add("onclick", "GetDocumentLink('" + hdnCategoryName.Value + "','" + lnkSubCategory.Text + "')");
                        DivGuideline.Visible = true;
                    }
                    else
                    {
                        DivGuideline.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void rptCategories_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnCategoryID = (HiddenField)e.Item.FindControl("hdnCategoryID");
                Repeater rptSubCategories = (Repeater)e.Item.FindControl("rptSubCategories");
                rptSubCategories.DataSource = lstUserCategories.Where(le => le.CategoryID == Convert.ToInt64(hdnCategoryID.Value) && Convert.ToBoolean(le.IsActive)).OrderBy(le => le.SubCategoryName).ToList();
                rptSubCategories.DataBind();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void rptProductList_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnStoreProductID = (HiddenField)e.Item.FindControl("hdnStoreProductID");
                HiddenField hdnProductImage = (HiddenField)e.Item.FindControl("hdnProductImage");
                HiddenField hdnLargeProductImage = (HiddenField)e.Item.FindControl("hdnLargeProductImage");
                HtmlAnchor azoom = (HtmlAnchor)e.Item.FindControl("azoom");
                DropDownList ddlSize = (DropDownList)e.Item.FindControl("ddlSize");
                String ImageLargePath = String.Empty;
                HtmlImage imgProduct = (HtmlImage)e.Item.FindControl("imgProduct");
                List<GetProductItemDetailsByStoreProductIDResult> objList = new List<GetProductItemDetailsByStoreProductIDResult>();
                objList = objProductItemRepo.GetProductItemDetailsByStoreProductID(Convert.ToInt64(hdnStoreProductID.Value), IncentexGlobal.CurrentMember.UserInfoID).ToList();
                // START - to add last row class
                if (e.Item.ItemIndex == 0)
                    rowCountIndex = 1;
                else
                    rowCountIndex++;

                if (rowCountIndex % 3 == 0)
                {
                    HtmlControl liRepeater = (HtmlControl)e.Item.FindControl("liRepeater");
                    liRepeater.Attributes.Add("class", "last");
                }
                // END - to add last row class

                // START - Display Images
                if (!String.IsNullOrEmpty(hdnProductImage.Value))
                    imgProduct.Src = SiteURL + "UploadedImages/ProductImages/Thumbs/" + hdnProductImage.Value.ToString();
                else
                    imgProduct.Src = SiteURL + "UploadedImages/ProductImages/ProductDefault.jpg";

                if (!String.IsNullOrEmpty(hdnLargeProductImage.Value))
                    ImageLargePath = SiteURL + "UploadedImages/ProductImages/" + hdnLargeProductImage.Value.ToString();
                else
                    ImageLargePath = SiteURL + "UploadedImages/ProductImages/ProductDefault.jpg";
                // END - Display Images

                //Set here zoom
                // Now need this zoom on popup contents
                //azoom.HRef = ImageLargePath;
                //
                // To get Distinct value from Generic List using LINQ
                // Create an Equality Comprarer Intance
                IEqualityComparer<GetProductItemDetailsByStoreProductIDResult> customComparer = new Common.PropertyComparer<GetProductItemDetailsByStoreProductIDResult>("ItemSizeID");
                IEnumerable<GetProductItemDetailsByStoreProductIDResult> distinctSizeList = objList.Distinct(customComparer);
                // Bind Size 
                BindddlSize(ddlSize, distinctSizeList);

                //START- Display Price
                Label lblAmount = (Label)e.Item.FindControl("lblAmount");
                if (objList.Count > 0)
                {
                    GetProductItemDetailsByStoreProductIDResult objprod = objList.Where(q => q.ItemSizeID == Convert.ToInt64(ddlSize.SelectedValue)).FirstOrDefault();
                    String PriceAmount = GetPriceAmount(objprod);
                    if (!String.IsNullOrEmpty(PriceAmount))
                        lblAmount.Text = PriceAmount;
                    else
                        lblAmount.Text = "$0";

                    // Check Item is already Exist in cart
                    HtmlControl dvCartRemove = (HtmlControl)e.Item.FindControl("dvCartRemove");
                    HtmlControl dvCartCaption = (HtmlControl)e.Item.FindControl("dvCartCaption");
                    HtmlAnchor lnkAddtoCart = (HtmlAnchor)e.Item.FindControl("lnkAddtoCart");
                    // Show popup on dvCartCaption  click
                    String JavaScriptShow = "ShowQuickPopup('" + hdnStoreProductID.Value + "','" + dvCartCaption.ClientID + "','" + dvCartRemove.ClientID + "');";
                    dvCartCaption.Attributes.Add("onclick", JavaScriptShow);
                    if (Convert.ToBoolean(objprod.AlreadyInCart))
                    {
                        dvCartRemove.Attributes.Add("style", "display:block;");
                        dvCartCaption.Attributes.Add("style", "display:none;");
                        lnkAddtoCart.Attributes.Add("class", "cart-btn active");
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), DateTime.Now.ToString(), "SetSpanID('" + dvCartRemove.ClientID + "',  " + objprod.MyShoppingCartID + ");", true);
                    }
                    else
                    {
                        dvCartCaption.Attributes.Add("style", "display:block;");
                        dvCartRemove.Attributes.Add("style", "display:none;");
                        lnkAddtoCart.Attributes.Add("class", lnkAddtoCart.Attributes["class"].ToString().Replace("active", ""));
                    }
                }
                else
                    lblAmount.Text = "$0";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }

    protected void repIssuancePolicies_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetIssuancePoliciesByUserInfoIDResult objItem = (GetIssuancePoliciesByUserInfoIDResult)e.Item.DataItem;

                if (objItem != null)
                {
                    HyperLink hlIssuancePolicy = (HyperLink)e.Item.FindControl("hlIssuancePolicy");
                    Label lblPolicyName = (Label)e.Item.FindControl("lblPolicyName");
                    Image imgIsOrdered = (Image)e.Item.FindControl("imgIsOrdered");

                    lblPolicyName.Text = objItem.IssuanceProgramName;

                    if (objItem.IsOrdered.Substring(0, 1) == "1")
                    {
                        imgIsOrdered.ImageUrl = "../StaticContents/img/new-label.png";
                        imgIsOrdered.ToolTip = "Already Ordered";
                        hlIssuancePolicy.Attributes.Add("onclick", "GeneralAlertMsg('You have already placed an order using this policy.')");
                        hlIssuancePolicy.NavigateUrl = "";
                    }
                    //else if (objItem.IsOrdered.Substring(2, 1) == "1")
                    //{
                    //    imgIsOrdered.ImageUrl = "../NewDesign/img/new-label.png";
                    //    imgIsOrdered.ToolTip = "Already Ordered";
                    //    hlIssuancePolicy.Attributes.Add("onclick", "GeneralAlertMsg('You have already placed an order using this policy group.')");
                    //    hlIssuancePolicy.NavigateUrl = "";
                    //}
                    else
                    {
                        imgIsOrdered.ImageUrl = "../StaticContents/img/normal-label.png";
                        imgIsOrdered.ToolTip = "Not Ordered Yet";
                        hlIssuancePolicy.Attributes.Add("onclick", "ShowIssuancePopup('" + 120 + "')");
                        hlIssuancePolicy.NavigateUrl = "";

                        //hlIssuancePolicy.NavigateUrl = "IssuancePackage.aspx?pid=" + objItem.UniformIssuancePolicyID;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Paging

    PagedDataSource pds = new PagedDataSource();

    protected void dtlPaging_ItemCommand(Object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "lnkbtnPaging")
            {
                this.IsViewAllTotal = false;
                CurrentPage = Convert.ToInt16(e.CommandArgument);
                BindProductRepeater();
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
            LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");

            if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
            {
                lnkbtnPage.Enabled = false;
                lnkbtnPage.Font.Bold = true;
                lnkbtnPage.CssClass = String.Empty;
                lnkbtnPage.Font.Size = new FontUnit(12);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public Int32 CurrentPage
    {
        get
        {
            return Convert.ToInt32(ViewState["CurrentPage"]);
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }

    public Int32 FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt32(ViewState["FrmPg"]);
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }

    public Int32 ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return 3;
            else
                return Convert.ToInt32(ViewState["ToPg"]);
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }

    private void doPaging()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            Int32 CurrentPg = pds.CurrentPageIndex + 1;

            if (CurrentPg > ToPg)
            {
                FrmPg = ToPg + 1;
                ToPg = ToPg + 3;
            }
            else if (!IsViewAllTotal && CurrentPg == ToPg)// this will set if when user will click page index
            {
                ToPg = 3;
            }

            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - 3;
            }

            if (pds.PageCount < ToPg)
                ToPg = pds.PageCount;

            for (Int32 i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            // Top
            dtTopPaging.DataSource = dt;
            dtTopPaging.DataBind();
            // Bottom
            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();
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
            this.IsViewAllTotal = true;
            BindProductRepeater();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnPrevious_Click(Object sender, EventArgs e)
    {
        try
        {
            this.IsViewAllTotal = false;
            CurrentPage -= 1;
            BindProductRepeater();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnNext_Click(Object sender, EventArgs e)
    {
        try
        {
            this.IsViewAllTotal = false;
            CurrentPage += 1;
            BindProductRepeater();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}