using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Web;

public partial class admin_CompanyStore_Product_General : PageBase
{
    #region Data Members
    Int64 iStoreid
    {
        get
        {
            if (ViewState["iStoreid"] == null)
            {
                ViewState["iStoreid"] = 0;
            }
            return Convert.ToInt64(ViewState["iStoreid"]);
        }
        set
        {
            ViewState["iStoreid"] = value;
        }
    }
    CatogeryRepository objCatRepository = new CatogeryRepository();
    StoreProduct objStoreProd = new StoreProduct();
    StoreProductRepository objStoreProdRepository = new StoreProductRepository();
    SupplierRepository objSupRepos = new SupplierRepository();
    LookupRepository objLookupRepos = new LookupRepository();
    Boolean bolflag = false;
    Int64 iStoreProductID
    {
        get
        {
            if (ViewState["iStoreProductID"] == null)
            {
                ViewState["iStoreProductID"] = 0;
            }
            return Convert.ToInt64(ViewState["iStoreProductID"]);
        }
        set
        {
            ViewState["iStoreProductID"] = value;
        }
    }
    /// <summary>
    /// To Display WorkgroupName
    /// </summary>
    String WorkGroupNameToDisplay
    {
        get
        {
            if (Session["WorkGroupNameToDisplay"] != null && Session["WorkGroupNameToDisplay"].ToString().Length > 0)
                ViewState["WorkGroupNameToDisplay"] = " - " + Session["WorkGroupNameToDisplay"].ToString();
            else
                ViewState["WorkGroupNameToDisplay"] = "";

            return ViewState["WorkGroupNameToDisplay"].ToString();
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        Int32 ManageID = (Int32)Incentex.DAL.Common.DAEnums.ManageID.CompanyProduct;
        Session["ManageID"] = ManageID;
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

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "General" + WorkGroupNameToDisplay;
            ((HtmlGenericControl)menuControl.FindControl("dvSubMenu")).Visible = false;
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";

            BindValues();
            FillCategory();
            FillSupplier();
            FillInventoryStatus();
            FillShowInventoryLevelStore();
            FillAllowbackOrders();
            FillCreditMessage();
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                iStoreid = Convert.ToInt64(Request.QueryString["Id"]);
                lblEmployeeName.Text = new CompanyStoreRepository().GetBYStoreId(Convert.ToInt32(iStoreid)).FirstOrDefault().CompanyName;

                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.ProductReturnURL;
            }

            if (Session["WorkgroupName"] != null)
            {
                Int32 workgroupid = Convert.ToInt32(objLookupRepos.GetIdByLookupNameNLookUpCode(Session["WorkgroupName"].ToString().Trim(), "Workgroup"));
                ddlWorkgroup.SelectedValue = Convert.ToString(workgroupid);
            }

            if (Request.QueryString["SubId"] != null && Request.QueryString["SubId"] != "0")
            {
                iStoreProductID = Convert.ToInt64(Request.QueryString["SubId"]);
                menuControl.PopulateMenu(0, 0, Convert.ToInt64(iStoreProductID), iStoreid, false);
                PopulateControl();
            }
            else
            {
                menuControl.PopulateMenu(0, 0, 0, 0, false);
                ClearData();
            }
        }
    }

    /// <summary>
    ///Fill the SubCatogry dropdownlist
    ///from SubCatogery table.
    /// FillCategory()
    /// Nagmani 07/10/2010
    /// <param name="iCatogeryid"></param>
    /// </summary>
    protected void ddlPrdCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            List<SubCategory> objsubCatlist = new List<SubCategory>();
            objsubCatlist = objCatRepository.GetAllSubCategory(Convert.ToInt32(ddlPrdCategory.SelectedValue));
            if (objsubCatlist.Count != 0)
            {
                ddlSubCategory.DataSource = objsubCatlist;
                ddlSubCategory.DataValueField = "SubCategoryID";
                ddlSubCategory.DataTextField = "SubCategoryName";
                ddlSubCategory.DataBind();
                ddlSubCategory.Items.Insert(0, new ListItem("..-Select-..", "0"));
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            SaveData();

            if (bolflag == true)
                Response.Redirect("ItemDetails.aspx?SubId=" + objStoreProd.StoreProductID + "&Id=" + iStoreid);
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    #endregion

    #region Methods

    /// <summary>
    /// PopulateControl()
    /// This method is called to 
    /// Populate the control in edit mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="iStoreProductID"></param>
    public void PopulateControl()
    {
        try
        {
            String strDAteNewProductUntil = "1900-01-01";
            String strDAteArrive = "1900-01-01";

            if (this.iStoreProductID != 0)
            {
                //Reterieve Data from company table here
                objStoreProd = objStoreProdRepository.GetById(this.iStoreProductID);

                if (objStoreProd.ProductDescrption != null)
                    txtPrdDescription.Text = objStoreProd.ProductDescrption;

                if (objStoreProd.Summary != null)
                    txtSummary.Text = objStoreProd.Summary;

                if (objStoreProd.NewProductUntil != Convert.ToDateTime(strDAteNewProductUntil))
                    txtNewProdUntil.Text = Convert.ToDateTime(objStoreProd.NewProductUntil).ToString("MM/dd/yyyy");
                else
                    txtNewProdUntil.Text = String.Empty;

                //Fill all category and select
                FillCategory();
                if (Convert.ToInt64(objStoreProd.CategoryID) != 0)
                    ddlPrdCategory.SelectedValue = objStoreProd.CategoryID.ToString();

                //Fill inventory status and select
                FillInventoryStatus();
                if (objStoreProd.InventoryStatus != null)
                    ddlInventoryStatus.SelectedValue = objStoreProd.InventoryStatus.ToString();

                ddlPrdCategory_SelectedIndexChanged(null, null);
                ddlSubCategory.SelectedValue = objStoreProd.SubCategoryID.ToString();
                ddlDepartment.SelectedValue = objStoreProd.DepartmentID.ToString();
                ddlWorkgroup.SelectedValue = objStoreProd.WorkgroupID.ToString();
                ddlGender.SelectedValue = objStoreProd.GarmentTypeID.ToString();
                ddlStatus.SelectedValue = objStoreProd.StatusID.ToString();
                ddlCreditEligible.SelectedValue = objStoreProd.AnneversaryCreditEligibleID.ToString();
                ddlTailoringGuidelineStatus.SelectedValue = objStoreProd.TailoringOption.ToString();
                ddlTailoringMeasurementChart.SelectedValue = objStoreProd.TailoringMeasurementStatus.ToString();
                ddlInventoryNotificationSystem.SelectedValue = objStoreProd.NotificationSystemID.ToString();

                //Fill supplier and select
                FillSupplier();
                ddlSupplierName.SelectedValue = objStoreProd.SupplierId.ToString();

                //Fill inventory level and select
                FillShowInventoryLevelStore();
                if (!String.IsNullOrEmpty(objStoreProd.ShowInventoryLevelInStoreID.ToString()))
                    ddlInvtLevelStore.SelectedValue = objStoreProd.ShowInventoryLevelInStoreID.ToString();

                //Fill allow backorder and select
                FillAllowbackOrders();
                if (!String.IsNullOrEmpty(objStoreProd.AllowBackOrderID.ToString()))
                    ddlAllowbackOrders.SelectedValue = objStoreProd.AllowBackOrderID.ToString();

                if (objStoreProd.ToArriveOn != Convert.ToDateTime(strDAteArrive))
                    txtDate.Text = Convert.ToDateTime(objStoreProd.ToArriveOn).ToShortDateString();
                else
                    txtDate.Text = String.Empty;

                if (objStoreProd.TailoringRunCharge == null)
                    txtRailoringRunCharge.Text = "";
                else
                    txtRailoringRunCharge.Text = objStoreProd.TailoringRunCharge.ToString();

                if (objStoreProd.TailoringServicesLeadTime == null)
                    txtTailoringServicesLeadTime.Text = "";
                else
                    txtTailoringServicesLeadTime.Text = objStoreProd.TailoringServicesLeadTime.ToString();

                if (objStoreProd.TailoringOption.ToString() == "0")
                {
                    Int32 LookupId = Convert.ToInt32(objLookupRepos.GetIdByLookupName("InActive"));
                    ddlTailoringGuidelineStatus.SelectedValue = LookupId.ToString();
                }
                else
                    ddlTailoringGuidelineStatus.SelectedValue = objStoreProd.TailoringOption.ToString();

                if (objStoreProd.TailoringMeasurementStatus.ToString() == "0")
                {
                    Int32 tLookupId = Convert.ToInt32(objLookupRepos.GetIdByLookupName("InActive"));
                    ddlTailoringMeasurementChart.SelectedValue = tLookupId.ToString();
                }
                else
                    ddlTailoringMeasurementChart.SelectedValue = objStoreProd.TailoringMeasurementStatus.ToString();

                if (objStoreProd.NotificationSystemID.ToString() == "136")
                {
                    Int32 iLookupId = Convert.ToInt32(objLookupRepos.GetIdByLookupName("InActive"));
                    ddlInventoryNotificationSystem.SelectedValue = iLookupId.ToString();
                }
                else
                    ddlInventoryNotificationSystem.SelectedValue = objStoreProd.NotificationSystemID.ToString();

                //Fill credit message and select
                FillCreditMessage();
                if (!String.IsNullOrEmpty(objStoreProd.CreditMessage.ToString()))
                    ddlCreditMessage.SelectedValue = objStoreProd.CreditMessage.ToString();

                if (objStoreProd.EmployeeTypeid != 0 || objStoreProd.EmployeeTypeid != null)
                    ddlEmployeeType.SelectedValue = Convert.ToString(objStoreProd.EmployeeTypeid);

                ddlColor.SelectedValue = Convert.ToString(objStoreProd.ColorOff);
                ddlSize.SelectedValue = Convert.ToString(objStoreProd.SizeOff);
                ddlMaterial.SelectedValue = Convert.ToString(objStoreProd.MaterialStatus);
                ddlCloseOut.SelectedValue = Convert.ToString(objStoreProd.IsCloseOut);

                ddlShowSoldby.SelectedValue = Convert.ToString(objStoreProd.ShowSoldBy);
                ddlCertification.SelectedValue = Convert.ToString(objStoreProd.ShowCertification);
                ddlShowProductVideo.SelectedValue = Convert.ToString(objStoreProd.ShowProductVideo);
                txtProductVideoUrl.Text = Convert.ToString(objStoreProd.ProductVideoUrl);

                // to set Certification path
                if (!String.IsNullOrEmpty(objStoreProd.CertificationPath))
                    hdnCertPath.Value = Convert.ToString(objStoreProd.CertificationPath);
                else
                    hdnCertPath.Value = null;

                if (objStoreProd.ReportTag != null)
                    ddlReportTag.SelectedValue = objStoreProd.ReportTag.ToString();

                if (objStoreProd.ItemType != null)
                    ddlItemType.SelectedValue = Convert.ToString(objStoreProd.ItemType);

                if (objStoreProd.GLCodeID != null)
                    ddlProductGLCode.SelectedValue = Convert.ToString(objStoreProd.GLCodeID);

                if (objStoreProd.ClimateSettingId.HasValue)
                    ddlClimateSetting.SelectedValue = Convert.ToString(objStoreProd.ClimateSettingId);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// ClearData()
    /// This method is used to Clear the control
    /// in new mode
    /// FillCompCountry()
    /// Nagmani 07/10/2010
    /// </summary>
    public void ClearData()
    {
        ddlPrdCategory.SelectedIndex = 0;
        ddlSubCategory.SelectedIndex = 0;
        ddlCreditMessage.SelectedIndex = 0;
        txtNewProdUntil.Text = "";
        txtPrdDescription.Text = "";
        txtSummary.Text = "";
        ddlInventoryStatus.SelectedIndex = 0;
        ddlSupplierName.SelectedIndex = 0;
    }

    /// <summary>
    ///Save the Record.
    /// SaveData()
    /// Nagmani 07/10/2010
    /// </summary>
    public void SaveData()
    {
        try
        {
            //Insert into StoreProduct
            if (this.iStoreProductID != 0)
            {
                objStoreProd = objStoreProdRepository.GetById(Convert.ToInt32(this.iStoreProductID));
            }

            String strDAteNewProductUntil = "1900-01-01";
            objStoreProd.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
            objStoreProd.CategoryID = Convert.ToInt32(ddlPrdCategory.SelectedValue);
            objStoreProd.SubCategoryID = Convert.ToInt32(ddlSubCategory.SelectedValue);
            objStoreProd.AnneversaryCreditEligibleID = Convert.ToInt32(ddlCreditEligible.SelectedValue);
            objStoreProd.WorkgroupID = Convert.ToInt32(ddlWorkgroup.SelectedValue);
            objStoreProd.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);
            objStoreProd.GarmentTypeID = Convert.ToInt32(ddlGender.SelectedValue);

            if (txtNewProdUntil.Text != "")
                objStoreProd.NewProductUntil = Convert.ToDateTime(txtNewProdUntil.Text);
            else
                objStoreProd.NewProductUntil = Convert.ToDateTime(strDAteNewProductUntil);

            objStoreProd.ProductDescrption = txtPrdDescription.Text;
            objStoreProd.Summary = txtSummary.Text;
            objStoreProd.InventoryStatus = Convert.ToInt32(ddlInventoryStatus.SelectedValue);
            objStoreProd.SupplierId = Convert.ToInt32(ddlSupplierName.SelectedValue);
            objStoreProd.ShowInventoryLevelInStoreID = Convert.ToInt32(ddlInvtLevelStore.SelectedValue);

            //Change by prashant - 12th March 2013---Start
            if (objStoreProd.InventoryStatus == objLookupRepos.GetIdByLookupNameNLookUpCode("Customer Inventory", Incentex.DAL.Common.DAEnums.LookupCodeType.InventoryStatus.ToString()))
                objStoreProd.AllowBackOrderID = Convert.ToInt32(ddlAllowbackOrders.Items.FindByText("No").Value);
            else
                objStoreProd.AllowBackOrderID = Convert.ToInt32(ddlAllowbackOrders.SelectedValue);
            //Change by prashant - 12th March 2013---End

            if (txtDate.Text != "")
                objStoreProd.ToArriveOn = Convert.ToDateTime(txtDate.Text);
            else
                objStoreProd.ToArriveOn = Convert.ToDateTime("1900-01-01");

            if (ddlTailoringGuidelineStatus.SelectedValue != "")
                objStoreProd.TailoringOption = Convert.ToInt32(ddlTailoringGuidelineStatus.SelectedValue);
            else
                objStoreProd.TailoringOption = 0;

            //For tailoring guideline
            if (ddlTailoringMeasurementChart.SelectedValue != "")
                objStoreProd.TailoringMeasurementStatus = Convert.ToInt32(ddlTailoringMeasurementChart.SelectedValue);
            else
                objStoreProd.TailoringMeasurementStatus = 0;

            //
            //For Notification guideline
            if (ddlInventoryNotificationSystem.SelectedValue != "")
                objStoreProd.NotificationSystemID = Convert.ToInt32(ddlInventoryNotificationSystem.SelectedValue);
            else
                objStoreProd.NotificationSystemID = 136;// For InActive

            if (txtRailoringRunCharge.Text != "")
                objStoreProd.TailoringRunCharge = txtRailoringRunCharge.Text;
            else
                objStoreProd.TailoringRunCharge = null;

            if (txtTailoringServicesLeadTime.Text != "")
                objStoreProd.TailoringServicesLeadTime = txtTailoringServicesLeadTime.Text;
            else
                objStoreProd.TailoringServicesLeadTime = null;

            if (ddlCreditMessage.SelectedIndex > 0)
                objStoreProd.CreditMessage = Convert.ToInt32(ddlCreditMessage.SelectedValue);
            else
                objStoreProd.CreditMessage = 0;

            if (Convert.ToInt32(ddlEmployeeType.SelectedValue) != 0)
                objStoreProd.EmployeeTypeid = Convert.ToInt32(ddlEmployeeType.SelectedValue);
            else
                objStoreProd.EmployeeTypeid = 0;

            // For Color and Size
            if (ddlColor.SelectedValue == "True")
                objStoreProd.ColorOff = true;
            else
                objStoreProd.ColorOff = false;

            if (ddlSize.SelectedValue == "True")
                objStoreProd.SizeOff = true;
            else
                objStoreProd.SizeOff = false;

            // For Material
            if (ddlMaterial.SelectedValue == "True")
                objStoreProd.MaterialStatus = true;
            else
                objStoreProd.MaterialStatus = false;

            // For CloseOut Product
            if (ddlCloseOut.SelectedValue == "True")
                objStoreProd.IsCloseOut = true;
            else
                objStoreProd.IsCloseOut = false;
            //

            if (ddlReportTag.SelectedValue != "0")
                objStoreProd.ReportTag = Convert.ToInt64(ddlReportTag.SelectedValue);
            else
                objStoreProd.ReportTag = null;

            // For Production Item type
            if (ddlItemType.SelectedValue != "0")
                objStoreProd.ItemType = Convert.ToInt64(ddlItemType.SelectedValue);
            else
                objStoreProd.ItemType = null;

            // For Product GL-Code
            if (!String.IsNullOrEmpty(ddlProductGLCode.SelectedValue) && Convert.ToInt64(ddlProductGLCode.SelectedValue) > 0)
                objStoreProd.GLCodeID = Convert.ToInt64(ddlProductGLCode.SelectedValue);
            else
                objStoreProd.GLCodeID = null;


            //For Climate Setting
            if (ddlClimateSetting.SelectedIndex > 0)
                objStoreProd.ClimateSettingId = Convert.ToInt64(ddlClimateSetting.SelectedValue);
            else
                objStoreProd.ClimateSettingId = null;

            objStoreProd.Priority = 9999;

            // For Show vidoe 
            if (ddlShowSoldby.SelectedItem.Text == "Active")
                objStoreProd.ShowSoldBy = true;
            else
                objStoreProd.ShowSoldBy = false;

            // For Show vidoe 
            if (ddlShowProductVideo.SelectedItem.Text == "Active")
                objStoreProd.ShowProductVideo = true;
            else
                objStoreProd.ShowProductVideo = false;

            if (!String.IsNullOrEmpty(txtProductVideoUrl.Text))
                objStoreProd.ProductVideoUrl = txtProductVideoUrl.Text.Trim();

            // For Show Certification 
            if (ddlCertification.SelectedItem.Text == "Active")
                objStoreProd.ShowCertification = true;
            else
                objStoreProd.ShowCertification = false;

            
            #region Upload certification file and Set path
            String saveCertPath = String.Empty;
            String CertFileName = String.Empty;
            if (fpCertFile.HasFile)
            {
                String filePath = "~/UploadedImages/ProductCertification/";
                HttpPostedFile certFile = fpCertFile.PostedFile;
                CertFileName = "cert_" + System.DateTime.Now.Ticks + "_" + fpCertFile.FileName;
                saveCertPath = filePath + CertFileName;

                if (!String.IsNullOrEmpty(saveCertPath))
                    certFile.SaveAs(Server.MapPath(saveCertPath));

                if (!String.IsNullOrEmpty(fpCertFile.FileName))
                    objStoreProd.CertificationPath = CertFileName;
                else if (!String.IsNullOrEmpty(hdnCertPath.Value))
                    objStoreProd.CertificationPath = Convert.ToString(hdnCertPath.Value);
                else
                    objStoreProd.CertificationPath = null;//
            }

            #endregion

            if (this.iStoreProductID == 0)
            {
                objStoreProd.StoreId = this.iStoreid;
                objStoreProdRepository.Insert(objStoreProd);
                objStoreProdRepository.SubmitChanges();
                this.iStoreProductID = objStoreProd.StoreProductID;
                bolflag = true;
            }
            else
            {
                objStoreProdRepository.SubmitChanges();
                this.iStoreProductID = objStoreProd.StoreProductID;
                bolflag = true;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    ///Fill the Catogry dropdownlist
    ///from Catogery table.
    /// FillCategory()
    /// Nagmani 07/10/2010
    /// </summary>
    public void FillCategory()
    {
        try
        {
            List<Category> objCatlist = new List<Category>();
            objCatlist = objCatRepository.GetAllCategory();
            if (objCatlist.Count != 0)
            {
                ddlPrdCategory.DataSource = objCatlist;
                ddlPrdCategory.DataValueField = "CategoryID";
                ddlPrdCategory.DataTextField = "CategoryName";
                ddlPrdCategory.DataBind();
                ddlPrdCategory.Items.Insert(0, new ListItem("..-Select-..", "0"));
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }

    /// <summary>
    ///Fill the Supplier dropdownlist
    ///from Catogery table.
    /// FillSupplier()
    /// Nagmani 04/01/2011
    /// </summary>
    public void FillSupplier()
    {
        try
        {
            IEnumerable objList = objSupRepos.GetAllsupplier();
            if (objList != null)
            {
                ddlSupplierName.DataSource = objList;
                ddlSupplierName.DataValueField = "SupplierID";
                ddlSupplierName.DataTextField = "FirstName";
                ddlSupplierName.DataBind();
                ddlSupplierName.Items.Insert(0, new ListItem("..-Select-..", "0"));
            }
            else
            {
                ddlSupplierName.DataSource = null;
                ddlSupplierName.DataBind();
                ddlSupplierName.Items.Insert(0, new ListItem("..-Select-..", "0"));
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    //bind dropdown for the workgroup, department, gender, 
    public void BindValues()
    {
        //For Product Status 
        ddlStatus.DataSource = objLookupRepos.GetByLookup("Status");
        ddlStatus.DataValueField = "iLookupID";
        ddlStatus.DataTextField = "sLookupName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Department
        ddlDepartment.DataSource = objLookupRepos.GetByLookup("Department");
        ddlDepartment.DataValueField = "iLookupID";
        ddlDepartment.DataTextField = "sLookupName";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Gender
        ddlGender.DataSource = objLookupRepos.GetByLookup("GarmentType");
        ddlGender.DataValueField = "iLookupID";
        ddlGender.DataTextField = "sLookupName";
        ddlGender.DataBind();
        ddlGender.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Workgroup
        ddlWorkgroup.DataSource = objLookupRepos.GetByLookup("Workgroup");
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Credit Eligible
        ddlCreditEligible.DataSource = objLookupRepos.GetByLookup("Anniversary Credit Eligible");
        ddlCreditEligible.DataValueField = "iLookupID";
        ddlCreditEligible.DataTextField = "sLookupName";
        ddlCreditEligible.DataBind();
        ddlCreditEligible.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Inventory Notification System
        ddlInventoryNotificationSystem.DataSource = objLookupRepos.GetByLookup("Status");
        ddlInventoryNotificationSystem.DataValueField = "iLookupID";
        ddlInventoryNotificationSystem.DataTextField = "sLookupName";
        ddlInventoryNotificationSystem.DataBind();
        ddlInventoryNotificationSystem.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Tailoring Guideline Status
        ddlTailoringGuidelineStatus.DataSource = objLookupRepos.GetByLookup("Status");
        ddlTailoringGuidelineStatus.DataValueField = "iLookupID";
        ddlTailoringGuidelineStatus.DataTextField = "sLookupName";
        ddlTailoringGuidelineStatus.DataBind();
        ddlTailoringGuidelineStatus.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Tailoring Measurement Chart
        ddlTailoringMeasurementChart.DataSource = objLookupRepos.GetByLookup("Status");
        ddlTailoringMeasurementChart.DataValueField = "iLookupID";
        ddlTailoringMeasurementChart.DataTextField = "sLookupName";
        ddlTailoringMeasurementChart.DataBind();
        ddlTailoringMeasurementChart.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Credit Eligible
        ddlEmployeeType.DataSource = objLookupRepos.GetByLookup("EmployeeType");
        ddlEmployeeType.DataValueField = "iLookupID";
        ddlEmployeeType.DataTextField = "sLookupName";
        ddlEmployeeType.DataBind();
        ddlEmployeeType.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Report Tag
        ddlReportTag.DataSource = objLookupRepos.GetByLookup("ReportTag");
        ddlReportTag.DataValueField = "iLookupID";
        ddlReportTag.DataTextField = "sLookupName";
        ddlReportTag.DataBind();
        ddlReportTag.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Production item type 
        ddlItemType.DataSource = objLookupRepos.GetByLookup("ItemType");
        ddlItemType.DataValueField = "iLookupID";
        ddlItemType.DataTextField = "sLookupName";
        ddlItemType.DataBind();
        ddlItemType.Items.Insert(0, new ListItem("-Select-", "0"));

        //For Product GL-Code
        ddlProductGLCode.DataSource = objLookupRepos.GetByLookup("ProductGLCode");
        ddlProductGLCode.DataValueField = "iLookupID";
        ddlProductGLCode.DataTextField = "sLookupName";
        ddlProductGLCode.DataBind();
        ddlProductGLCode.Items.Insert(0, new ListItem("-Select-", "0"));

        //For ClimateSetting
        ddlClimateSetting.DataSource = objLookupRepos.GetByLookup("ClimateSetting");
        ddlClimateSetting.DataValueField = "iLookupID";
        ddlClimateSetting.DataTextField = "sLookupName";
        ddlClimateSetting.DataBind();
        ddlClimateSetting.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    /// <summary>
    /// FillInventoryStatus()
    ///Fill the inventory status dropdownlist
    ///from lookup table
    /// Nagmani 11/10/2010
    /// </summary>
    public void FillInventoryStatus()
    {
        try
        {
            String strStatus = "InventoryStatus";
            ddlInventoryStatus.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlInventoryStatus.DataValueField = "iLookupID";
            ddlInventoryStatus.DataTextField = "sLookupName";
            ddlInventoryStatus.DataBind();
            ddlInventoryStatus.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    ///Fill the Item Fill Allow back Order dropdownlist
    ///from lookup table
    /// FillAllowbackOrders()
    /// Nagmani 11/10/2010
    /// </summary>
    public void FillAllowbackOrders()
    {
        try
        {
            String strStatus = "BackOrderManagement";
            ddlAllowbackOrders.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlAllowbackOrders.DataValueField = "iLookupID";
            ddlAllowbackOrders.DataTextField = "sLookupName";
            ddlAllowbackOrders.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    ///Fill the Item Fillshow inventroy levels store dropdownlist
    ///from lookup table
    /// FillShowInventoryLevelStore()
    /// Nagmani 11/10/2010
    /// </summary>
    public void FillShowInventoryLevelStore()
    {
        try
        {
            String strStatus = "ItemsToBePolybagged";
            ddlInvtLevelStore.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlInvtLevelStore.DataValueField = "iLookupID";
            ddlInvtLevelStore.DataTextField = "sLookupName";
            ddlInvtLevelStore.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public void FillCreditMessage()
    {
        try
        {
            String strStatus = "CreditMessage";
            ddlCreditMessage.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlCreditMessage.DataValueField = "iLookupID";
            ddlCreditMessage.DataTextField = "sLookupName";
            ddlCreditMessage.DataBind();
            ddlCreditMessage.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}