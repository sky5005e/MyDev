using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_index : PageBase
{
    Boolean ShowSupportTicketPopup = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["Usr"] = "";

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

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
            {
                Response.Redirect("~/index.aspx");
            }

            if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier) && IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SupplierEmployee) && Incentex.DAL.Common.DAEnums.GetUserTypeFor(IncentexGlobal.CurrentMember.Usertype) != "GSEAssetManagement")
            {
                //Binds Menu if USerType is Incentex Employee
                bindMenus();
                ((HtmlImage)Master.FindControl("imgHomeBtn")).Visible = false;
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Visible = false;
                //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                //((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Menu.aspx";
            }
            else if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SupplierEmployee) && Incentex.DAL.Common.DAEnums.GetUserTypeFor(IncentexGlobal.CurrentMember.Usertype) != "GSEAssetManagement")
            {
                //If Supplier Is Logged in then Bind below Menu..
                bindSupplierMenus();
                ((HtmlImage)Master.FindControl("imgHomeBtn")).Visible = false;
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "   ";
                //((HyperLink)Master.FindControl("lnkLoginUrl")).Visible = false;
            }
            //added by Prashant 8th April 2013
            else if (Incentex.DAL.Common.DAEnums.GetUserTypeFor(IncentexGlobal.CurrentMember.Usertype) == "GSEAssetManagement")
            {
                //If Equipment Vendor Employee Is Logged in then Bind below Menu..
                BindGSEAssetMgtUserMenus();
                ((HtmlImage)Master.FindControl("imgHomeBtn")).Visible = false;
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "   ";
                Session["Usr"] = Convert.ToString(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee);
                //((HyperLink)Master.FindControl("lnkLoginUrl")).Visible = false;
            }
            //added by Prashant 8th April 2013
            //if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
            //Changed by Saurabh 7 May 2012
            else
            {
                //Binds Menu if USerType is Supplier Employee
                bindSupplierEmployeeMenus();
                ((HtmlImage)Master.FindControl("imgHomeBtn")).Visible = false;
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "   ";
            }

            //setmenubasedonrights();

            ((Label)Master.FindControl("lblPageHeading")).Text = "World-Link System Menu";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Visible = false;
            //getDataRestictions();

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["st"])) && ShowSupportTicketPopup)
            {
                if (Convert.ToString(Request.QueryString["st"]) == "1")
                {
                    ostIEControl.Visible = true;
                    Session["ostIEControl"] = true;
                }
                else if (Convert.ToString(Request.QueryString["st"]) == "2")
                {
                    ostSPControl.Visible = true;
                    Session["ostSPControl"] = true;
                }
                ShowSupportTicketPopup = false;
            }

            HyperLink hlRecentlyUpdatedTickets = ((HyperLink)Master.FindControl("hlRecentlyUpdatedTickets"));
            hlRecentlyUpdatedTickets.Visible = true;
            Int32 UpdatedTicketCount = new ServiceTicketRepository().GetServiceTicketUpdatedCountIESA(IncentexGlobal.CurrentMember.UserInfoID);
            hlRecentlyUpdatedTickets.Text = "<span>" + UpdatedTicketCount + "</span>";
        }

    }

    public void setmenubasedonrights()
    {
        HiddenField lblId;
        IncentexEmployee objIncEmp = new IncentexEmployeeRepository().GetEmployeeByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        UserInformation objUserInfo = new UserInformationRepository().GetById(IncentexGlobal.CurrentMember.UserInfoID);
        List<IncEmpMenuAccess> lstMenuAccess = new IncentexEmpMenuAccessRepository().GetMenusByEmployeeId(objIncEmp.IncentexEmployeeID);

        foreach (DataListItem dtM in dtIndex.Items)
        {

            lblId = dtM.FindControl("hdnMenuAccess") as HiddenField;
            HtmlGenericControl dvChk = dtM.FindControl("menuspan") as HtmlGenericControl;

            foreach (IncEmpMenuAccess objMenu in lstMenuAccess)
            {

                if (objMenu.MenuPrivilegeID.ToString().Equals(lblId.Value))
                {
                    //dtM.FindControl("lnkMenuAccess").Visible = true;
                    break;
                }

            }

        }
    }

    public void bindMenus()
    {
        AccessRightRepository objRightRepo = new AccessRightRepository();
        List<GetMainMenuForIEByUserInfoIDResult> lstRights = new List<GetMainMenuForIEByUserInfoIDResult>();

        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
        {
            lstRights = objRightRepo.GetMainMenuForIE(IncentexGlobal.CurrentMember.UserInfoID);
        }
        else
        {
            var SuperAdminMenu = objRightRepo.GetAccessMenu().Where(le => le.ParentMenuID == 0);

            lstRights = (from r in SuperAdminMenu
                         select new GetMainMenuForIEByUserInfoIDResult
                         {
                             iMenuPrivilegeID = r.AccessMenuID,
                             sDescription = r.AccessMenuItem,
                             PageUrl = r.PageURL
                         }).ToList<GetMainMenuForIEByUserInfoIDResult>();
        }

        dtIndex.DataSource = lstRights;
        dtIndex.DataBind();
    }

    public void getDataRestictions()
    {

        string datarestrctions = "";
        //Get Employeeid Based on userID
        IncentexEmployee objIncEmp = new IncentexEmployeeRepository().GetEmployeeByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        IncentexEmpDataAccessRepository objCmpDataAccesRepos = new IncentexEmpDataAccessRepository();
        IncEmpDataAccess objCmpDataAccessfordel = new IncEmpDataAccess();

        //GetDataRestrction based on EmployeeId
        List<IncEmpDataAccess> lstData = objCmpDataAccesRepos.GetDataByEmployeeId(objIncEmp.IncentexEmployeeID);

        foreach (IncEmpDataAccess l in lstData)
        {
            if (datarestrctions == "")
            {
                datarestrctions = l.DataPrivilegeID.ToString();
            }
            else
            {
                datarestrctions = datarestrctions + "," + l.DataPrivilegeID.ToString();
            }
        }

        DataAccessRepository objData = new DataAccessRepository();
        List<INC_DataPrivilege> objDataList = objData.GetDataRestrictionByEmployeeID(datarestrctions);
        IncentexGlobal.IncentexEmployeeDataRestriction = objDataList;



        //

    }

    public void bindSupplierMenus()
    {

        SupplierRepository objSupRep = new SupplierRepository();
        Supplier objSupplier = new Supplier();
        MenuPrivilegeRepository objMenu = new MenuPrivilegeRepository();
        objSupplier = objSupRep.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        List<INC_MenuPrivilege> objList = new List<INC_MenuPrivilege>();
        if (objSupplier.AccessToEditOrMakeChangesToPurchaseOrders == 1)
        {
            INC_MenuPrivilege objMenuItem = objMenu.GetMenuByMenuType(Incentex.DA.DAEnums.MenuTypes.BackEnd.ToString(), "Supplier", "Purchase Orders");
            if (objMenuItem != null)
            {
                objList.Add(objMenuItem);
            }

        }

        if (objSupplier.AccessToWorldLinkTrackingCenter == 1)
        {
            INC_MenuPrivilege objMenuItem = objMenu.GetMenuByMenuType(Incentex.DA.DAEnums.MenuTypes.BackEnd.ToString(), "Supplier", "Order Management System");
            if (objMenuItem != null)
            {
                objList.Add(objMenuItem);
            }
        }
        if (objSupplier.AccessToWorldLinkTrackingCenter == 1)
        {
            INC_MenuPrivilege objMenuItem = objMenu.GetMenuByMenuType(Incentex.DA.DAEnums.MenuTypes.BackEnd.ToString(), "Supplier", "Return Management");
            if (objMenuItem != null)
            {
                objList.Add(objMenuItem);
            }
        }
        dtIndex.DataSource = objList;
        dtIndex.DataBind();

    }

    public void bindSupplierEmployeeMenus()
    {
        string SEmenupriviledge = "";
        SupplierEmployee objSuppEmp = new SupplierEmployeeRepository().GetEmployeeByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        UserInformation objUserInfo = new UserInformationRepository().GetById(IncentexGlobal.CurrentMember.UserInfoID);
        List<SupplierEmpMenuAccess> lstMenuAccess = new SupplierMenuAccessRepository().GetMenusBySupplierEmployeeID(objSuppEmp.SupplierEmployeeID);
        foreach (SupplierEmpMenuAccess objMenu1 in lstMenuAccess)
        {
            if (SEmenupriviledge == "")
            {
                SEmenupriviledge = objMenu1.MenuPrivilegeID.ToString();
            }
            else
            {
                SEmenupriviledge = SEmenupriviledge + "," + objMenu1.MenuPrivilegeID;
            }
        }
        MenuPrivilegeRepository objMenu = new MenuPrivilegeRepository();
        List<INC_MenuPrivilege> objList = objMenu.GetBackMenuByEmployeeID(Incentex.DA.DAEnums.MenuTypes.BackEnd.ToString(), "Supplier Employee", SEmenupriviledge).OrderBy(le => le.MenuOrder).ToList();
        IncentexGlobal.IncentexEmployeeMenuRights = objList;
        dtIndex.DataSource = objList;
        dtIndex.DataBind();



    }

    public void BindGSEAssetMgtUserMenus()
    {
        //string SVmenupriviledge = "";

        // Update By Prashant - 9th April  2013
        // Change Brief Note: Commented three Trips to the database and merge it to one trip

        //EquipmentVendorEmployee objEquiVendorEmployee = new AssetVendorRepository().GetVendorEmpByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);
        //// SupplierEmployee objSuppEmp = new SupplierEmployeeRepository().GetEmployeeByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        //UserInformation objUserInfo = new UserInformationRepository().GetById(IncentexGlobal.CurrentMember.UserInfoID);
        //List<EquipmentMenuAccess> lstMenuAccess = new AssetVendorRepository().GetMenusByVendorEmployeeID(objEquiVendorEmployee.VendorEmployeeID);
        ////List<SupplierEmpMenuAccess> lstMenuAccess = new SupplierMenuAccessRepository().GetMenusBySupplierEmployeeID(objSuppEmp.SupplierEmployeeID);


        //List<GSEUserDetails> objresult = new AssetVendorRepository().GetMenusByVendorEmployeeIDUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);
        //IncentexGlobal.GSEMgtUserDetails = objresult;
        //foreach (GSEUserDetails objMenu1 in objresult)
        //{  
        //    if (SVmenupriviledge == "")
        //    {
        //        SVmenupriviledge = objMenu1.MenuPrivilegeID.ToString();
        //    }
        //    else
        //    {
        //        SVmenupriviledge = SVmenupriviledge + "," + objMenu1.MenuPrivilegeID;
        //    }
        //}
        //MenuPrivilegeRepository objMenu = new MenuPrivilegeRepository();
        //List<INC_MenuPrivilege> objList = objMenu.GetBackMenuByEmployeeID(Incentex.DA.DAEnums.MenuTypes.FrontEnd.ToString(), "GSEAssetManagementUsers", SVmenupriviledge).OrderBy(le => le.MenuOrder).ToList();
        //IncentexGlobal.IncentexEmployeeMenuRights = objList;
        //dtIndex.DataSource = objList;
        //dtIndex.DataBind();


        // Update By Prashant - 9th April 2013

        AssetVendorRepository objAssetMenu = new AssetVendorRepository();
        List<INC_MenuPrivilege> objList = objAssetMenu.GetMenusByVendorEmployeeIDUserInfoID(IncentexGlobal.CurrentMember.UserInfoID, Incentex.DA.DAEnums.MenuTypes.FrontEnd.ToString(), "GSEAssetManagementUsers").OrderBy(le => le.MenuOrder).ToList();
        IncentexGlobal.IncentexEmployeeMenuRights = objList;
        dtIndex.DataSource = objList;
        dtIndex.DataBind();


    }

    protected void dtIndex_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ((HtmlImage)e.Item.FindControl("imgBtnPageURL")).Src = ((HiddenField)e.Item.FindControl("hdnMenuURL")).Value;
            if (((LinkButton)(e.Item.FindControl("lnkMenuAccess"))).ToolTip == "Reset Order Access")
            {
                ((LinkButton)e.Item.FindControl("lnkMenuAccess")).OnClientClick = "return OnClickForSync();";

            }
            if (((LinkButton)(e.Item.FindControl("lnkMenuAccess"))).ToolTip == "Server Management")
            {
                ((LinkButton)e.Item.FindControl("lnkMenuAccess")).OnClientClick = "window.open('https://mypeer1.com',null,'height=500, width=800')";

            }
            if (((LinkButton)(e.Item.FindControl("lnkMenuAccess"))).ToolTip == "Remote Support")
            {
                ((LinkButton)e.Item.FindControl("lnkMenuAccess")).OnClientClick = "window.open('http://showmypc.com/mac/java-client.html?ci=world-link','Remote-Support','height=350, width=550');return false;";

            }
            if (((LinkButton)(e.Item.FindControl("lnkMenuAccess"))).ToolTip == "View User Storefront")
            {
                ((LinkButton)e.Item.FindControl("lnkMenuAccess")).CommandName = "ViewStorefront";
            }


        }
    }
    protected void dtIndex_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "Redirect")
        {
            HiddenField hdnMenuPrevilidgedId = (HiddenField)e.Item.FindControl("hdnMenuAccess");
            if (e.CommandArgument.ToString() == "User Management")
            {
                Response.Redirect("UserManagement.aspx");
            }
            else if (e.CommandArgument.ToString() == "Drop-Down Menu Management")
            {
                Response.Redirect("dropdownmenus.aspx");
            }
            else if (e.CommandArgument.ToString() == "Store Management Console")
            {
                Response.Redirect("~/admin/CompanyStore/ViewCompanyStore.aspx?id=" + hdnMenuPrevilidgedId.Value);
            }
            else if (e.CommandArgument.ToString() == "Global Setting")
            {
                Response.Redirect("~/admin/GlobalSetting.aspx");
            }
            else if (e.CommandArgument.ToString() == "Order Management System")
            {
                IncentexGlobal.ManageID = 9;
                Response.Redirect("~/OrderManagement/OrderLookupSearch.aspx");

            }
            else if (e.CommandArgument.ToString() == "Tracking Center")
            {
                Response.Redirect("~/TrackingCenter/ReportSelection.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Return Management")
            {
                Response.Redirect("~/ProductReturnManagement/ProductReturnLookupSearch.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Executive Planning Reports")
            {
                Response.Redirect("~/admin/Report/ReportDashBoard.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Summary Order View")
            {
                Response.Redirect("~/admin/Report/ReportDashBoard.aspx?IsFromIndexPage=true");
            }
            else if (e.CommandArgument.ToString().Trim() == "Reset Order Access")
            {
                //Update All the users who has viewed the orders but havent got out from the orders
                //Will update the OrderDetailsHistory who are having wrong records and b4 1 hour of current date!
                new OrderConfirmationRepository().UpdateOrderWiseUsers();
                //end
            }
            else if (e.CommandArgument.ToString().Trim() == "Artwork Library")
            {
                Response.Redirect("~/admin/ArtWorkDecoratingLibrary/ArtworkIndex.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Image Library")
            {
                Response.Redirect("~/admin/Artwork/ListArts.aspx?type=Image");
            }
            else if (e.CommandArgument.ToString().Trim() == "Communications Center")
            {
                Response.Redirect("~/admin/CommunicationCenter/CampaignSelection.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Indianic Basecamp")
            {
                Response.Redirect("https://indianic.basecamphq.com/login");
            }
            else if (e.CommandArgument.ToString().Trim() == "Manage Supplier Partner")
            {
                Response.Redirect("~/admin/ManageSupplierPartner/ViewSupplierPartner.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Video Training System")
            {
                Response.Redirect("~/admin/VideoTrainingSystem.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Support Ticket Center")
            {
                Response.Redirect("~/admin/ServiceTicketCenter.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Product Search")
            {
                Response.Redirect("~/admin/CompanyStore/Product/ProductSearch.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "GSE Asset Management")
            {
                Response.Redirect("~/AssetManagement/AssetMgtIndex.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Add GSE Equipment")
            {
                Response.Redirect("~/AssetManagement/AddEquipment.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Search Equipment")
            {
                Response.Redirect("~/AssetManagement/SearchEquipment.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Flagged Assets")
            {
                Response.Redirect("~/AssetManagement/FlaggedAssets.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "GSE User Management")
            {
                if (IncentexGlobal.GSEMgtCurrentMember != null && (IncentexGlobal.GSEMgtCurrentMember.CurrentUserType == (long)Incentex.DAL.Common.DAEnums.UserTypes.CustomerEmployee || IncentexGlobal.GSEMgtCurrentMember.CurrentUserType == (long)Incentex.DAL.Common.DAEnums.UserTypes.VendorEmployee))
                    Response.Redirect("~/AssetManagement/BasicVendorEmpInformation.aspx?SubId=" + IncentexGlobal.GSEMgtCurrentMember.VenoderEmployeeID + "&Id=" + IncentexGlobal.GSEMgtCurrentMember.VendorID);
                else
                    Response.Redirect("~/AssetManagement/VendorList.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Drop Down Menu")
            {
                Response.Redirect("~/AssetManagement/DropDownMenu.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Generate Sales Orders (CSV)")
            {
                Response.Redirect("~/admin/Supplier/GenerateSalesOrdersCSV.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Pending Invoices")
            {
                Response.Redirect("~/AssetManagement/PendingInvoice.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Station Inventory")
            {
                Response.Redirect("~/AssetManagement/Inventory/InventoryIndex.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Blog Center")
            {
                Response.Redirect("~/AssetManagement/Blog/BlogDetail.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Sync Warehouse Data")
            {
                Response.Redirect("~/admin/Supplier/SyncInformation.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Field Management")
            {
                Response.Redirect("~/AssetManagement/FieldMgt/FieldMgtIndex.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Planning Reports")
            {
                Response.Redirect("~/AssetManagement/Reports/ReportDashBoard.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Repair Order Management")
            {
                Response.Redirect("~/AssetManagement/RepairManagement/RepairManagementIndex.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Process Incoming Returns")
            {
                Response.Redirect("~/ProductReturnManagement/ProductItemsReturnSearch.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Document Storage Center")
            {
                Response.Redirect("~/admin/DocumentStorageCentre/DocumentStorageCenter.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Employee Training Center")
            {
                Response.Redirect("~/admin/EmployeeTrainingCenter/EmployeeTrainingCenter.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Headset Repair Center")
            {
                Response.Redirect("~/HeadsetRepairCenter/HeadsetRepairCenter.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Supplier Partners")
            {
                Response.Redirect("~/admin/ManageSupplierPartner/ListSupplier.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Worldwide Prospects")
            {
                Response.Redirect("~/admin/WorldwideProspect/WorldwideProspects.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Purchase Order Management")
            {
                Response.Redirect("~/admin/PurchaseOrderManagement/PurchaseOrderManagement.aspx");
            }
            else if (e.CommandArgument.ToString().Trim() == "Incentex Order Management")
            {
                Response.Redirect("~/admin/PurchaseOrderManagement/VendorAccessIncOrderManegement.aspx");
            }
        }
        else if (e.CommandName == "ViewStorefront")
        {
            List<CompanyStoreRepository.IECompanyListResults> objCompanyList = new CompanyStoreRepository().GetCompanyStore();

            if (objCompanyList != null)
            {
                ddlStorefrontCompany.DataSource = objCompanyList.OrderBy(x => x.Company);
                ddlStorefrontCompany.DataValueField = "CompanyID";
                ddlStorefrontCompany.DataTextField = "Company";
                ddlStorefrontCompany.DataBind();
            }
            ddlStorefrontCompany.Items.Insert(0, new ListItem("-Select Company-", "0"));
            lblLogin.Text = string.Empty;
            lblPassword.Text = string.Empty;
            mpeViewUserStorefront.Show();
        }
    }
    protected void lnkbtnViewUserStoreFront_Click(object sender, EventArgs e)
    {
        if (ddlStorefrontCompany.SelectedIndex != 0 && ddlStorefrontUser.SelectedIndex != 0)
        {
            if (ddlStorefrontAccessType.SelectedItem.Value == "Test")
                IncentexGlobal.IsIEFromStoreTestMode = true;                    
            else if (ddlStorefrontAccessType.SelectedValue == "PEO")
                IncentexGlobal.IsPlaceExchangeOrder = true;

            Response.Redirect("~/index.aspx?id=true&subid=" + ddlStorefrontUser.SelectedItem.Value.ToString() + "&storeid=" + ddlStorefrontCompany.SelectedItem.Value);
        }
        else
        {
            lblMessage.Text = "Please select user";
            mpeViewUserStorefront.Show();
        }
    }
    protected void ddlStorefrontCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStorefrontCompany.SelectedIndex != 0)
        {
            ddlStorefrontUser.DataSource = new UserInformationRepository().GetCompanyEmployeeContactByCompanyID(Convert.ToInt64(ddlStorefrontCompany.SelectedValue), IncentexGlobal.CurrentMember.UserInfoID).Select(le => new { ContactName = le.LastName + " " + le.FirstName, UserInfoID = le.UserInfoID }).OrderBy(le => le.ContactName).ToList();
            ddlStorefrontUser.DataValueField = "UserInfoID";
            ddlStorefrontUser.DataTextField = "ContactName";
            ddlStorefrontUser.DataBind();
            ddlStorefrontUser.Items.Insert(0, new ListItem("-select user-", "0"));
        }
        else
        {
            lblMessage.Text = "Please select company";
        }
        mpeViewUserStorefront.Show();
    }
    protected void ddlStorefrontUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStorefrontUser.SelectedIndex != 0)
        {
            UserInformation objUserInformation = new UserInformationRepository().GetById(Convert.ToInt64(ddlStorefrontUser.SelectedValue));
            lblLogin.Text = objUserInformation.LoginEmail;
            lblPassword.Text = objUserInformation.Password;
        }
        mpeViewUserStorefront.Show();
    }

}
