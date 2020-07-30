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
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL.Common;

public partial class AssetManagement_AddEquipment : PageBase
{
    #region DataMembers

    CompanyEmployee objCmpnyInfo = new CompanyEmployee();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    UserInformation objuser = new UserInformation();
    UserInformationRepository objuserrepos = new UserInformationRepository();
    AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
    Int64 EquipmentMasterId
    {
        get
        {
            if (ViewState["EquipmentMasterId"] == null)
            {
                ViewState["EquipmentMasterId"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentMasterId"]);
        }
        set
        {
            ViewState["EquipmentMasterId"] = value;
        }
    }
    #endregion

    #region Events
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
        try
        {   
            if (!IsPostBack)
            {
                base.MenuItem = "Add GSE Equipment";
                base.ParentMenuID = 46;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Add GSE Equipment";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";

                if (Session["Usr"] == Convert.ToString(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Index.aspx";
                }
                else
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/AssetMgtIndex.aspx";
                }

                lblMsg.Text = "";
                BindValues();

                if (Request.QueryString.Count > 0)
                {
                    this.EquipmentMasterId = Convert.ToInt64(Request.QueryString.Get("ID"));

                    if (this.EquipmentMasterId == 0 && !base.CanAdd)
                    {
                        base.RedirectToUnauthorised();
                    }
                }
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {

                    ddlCompany.SelectedValue = Convert.ToString(IncentexGlobal.CurrentMember.CompanyId);
                    ddlCompany.Enabled = false;


                }
                else
                {
                    ddlCompany.SelectedIndex = 0;
                    ddlCompany.Enabled = true;
                }


                //DisplayData();
            }
        }
        catch (Exception)
        {


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
           
            EquipmentMaster objEquipmentMaster = new EquipmentMaster();

           
                objEquipmentMaster.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
                objEquipmentMaster.GSEDepartmentID = Convert.ToInt64(ddlGSEDepartment.SelectedValue); ;
                objEquipmentMaster.BaseStationID = Convert.ToInt64(ddlBaseStation.SelectedValue);
                objEquipmentMaster.EquipmentID = txtEquipmentId.Text.Trim();
                objEquipmentMaster.EquipmentTypeID = Convert.ToInt64(ddlEquipmentType.SelectedValue);
                objEquipmentMaster.BrandID = Convert.ToInt64(ddlBrand.SelectedValue);
                objEquipmentMaster.ModelYear = txtModelYear.Text != "" ? Convert.ToInt32(txtModelYear.Text.Trim()) : 0;
                objEquipmentMaster.FuelTypeID = Convert.ToInt64(ddlFuelType.SelectedValue);
                objEquipmentMaster.SerialNumber = txtSerialNumber.Text.Trim();
                objEquipmentMaster.VinNumber = txtVinNumber.Text.Trim();
                objEquipmentMaster.PlateNumber = txtPlateNumber.Text.Trim();
                objEquipmentMaster.PurchasedFromID = Convert.ToInt64(ddlPurchasedFrom.SelectedValue);
                objEquipmentMaster.PurchasePrice = txtPurchasePrice.Text != "" ? Convert.ToDouble(txtPurchasePrice.Text.Trim()) : 0.00;
                objEquipmentMaster.BaseStationID = Convert.ToInt64(ddlBaseStation.SelectedValue);
                objEquipmentMaster.ChargeToID = Convert.ToInt64(ddlChargeTo.SelectedValue);
                objEquipmentMaster.Status = Convert.ToInt64(ddlStatus.SelectedValue);
                //--New Fields
                objEquipmentMaster.EquipmentModel = Convert.ToInt64(ddlEquipmentModel.SelectedValue);
                objEquipmentMaster.OwnedBy = Convert.ToInt64(ddlOwnedBy.SelectedValue);
                objEquipmentMaster.PowerSource = Convert.ToInt64(ddlPowerSource.SelectedValue);
                objEquipmentMaster.NewOrRefurbished = Convert.ToInt64(ddlNewRefurbished.SelectedValue);
                objEquipmentMaster.PurchaseMethod = Convert.ToInt64(ddlPurchaseMethod.SelectedValue);
                objEquipmentMaster.AircraftType = Convert.ToInt64(ddlAircraftType.SelectedValue);
                objEquipmentMaster.MaxAircraftWeight = Convert.ToInt64(ddlMaxAircraftWeight.SelectedValue);
                objEquipmentMaster.EquipmentLife = Convert.ToInt64(ddlEquipmentLife.SelectedValue);
                objEquipmentMaster.MaxWeight = txtMaxWeight.Text.Trim();
                objEquipmentMaster.WarrantyParts = txtWarrantyParts.Text.Trim();
                objEquipmentMaster.WarrantyLabor = txtWarrantyLabor.Text.Trim();
                if (!string.IsNullOrEmpty(txtPurchaseDate.Text))
                    objEquipmentMaster.PurchaseDate = Convert.ToDateTime(txtPurchaseDate.Text);
                //--

                objEquipmentMaster.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objEquipmentMaster.CreatedDate = DateTime.Now;

                objAssetMgtRepository.Insert(objEquipmentMaster);
                objAssetMgtRepository.SubmitChanges();
                lblMsg.Text = "Equipment Information Saved Successfully...";
           

            ResetControls();
        }
        catch (Exception)
        {


        }
    }
    #endregion

    #region Methods

    public void BindValues()
    {
        try
        {
            LookupRepository objLookupRepository = new LookupRepository();
            List<INC_Lookup> objList = new List<INC_Lookup>();
            String LookUpCode = string.Empty;
            //get Equipment Type
            LookUpCode = "EquipmentType";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            objList = objList.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlEquipmentType, objList, "sLookupName", "iLookupID", "-Select Equipment Type-");
            //get Brand
            LookUpCode = "Brand";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            objList = objList.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlBrand, objList, "sLookupName", "iLookupID", "-Select Brand-");
            //get Fuel Type
            LookUpCode = "FuelType";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            objList = objList.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlFuelType, objList, "sLookupName", "iLookupID", "-Select Fuel Type-");
            //get Purchased From
            LookUpCode = "PurchasedFrom";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            objList = objList.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlPurchasedFrom, objList, "sLookupName", "iLookupID", "-Select Purchased From-");           
            //get Charge To
            LookUpCode = "ChargeTo";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            objList = objList.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlChargeTo, objList, "sLookupName", "iLookupID", "-Select Charge To-");
            //get Status
            LookUpCode = "EquipmentStatus";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            objList = objList.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlStatus, objList, "sLookupName", "iLookupID", "-Select Status-");            
            //get GSE Department
            LookUpCode = "GSEDepartment";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            objList = objList.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlGSEDepartment, objList, "sLookupName", "iLookupID", "-Select GSE Department-");
            //get Base Stations
            CompanyRepository objRepo = new CompanyRepository();
            List<INC_BasedStation> objBaseStationList = new List<INC_BasedStation>();
            objBaseStationList = objRepo.GetAllBaseStationResult();
            objBaseStationList = objBaseStationList.OrderBy(p => p.sBaseStation).ToList();
            Common.BindDDL(ddlBaseStation, objBaseStationList, "sBaseStation", "iBaseStationId", "-Select All BaseSation-");
            //get company
            CompanyRepository objCompanyRepo = new CompanyRepository();
            List<Company> objCompanyList = new List<Company>();
            objCompanyList = objCompanyRepo.GetAllCompany();
            Common.BindDDL(ddlCompany, objCompanyList, "CompanyName", "CompanyID", "-Select Company-");
            #region NewFields
           
            List<EquipmentLookup> objLookup = new List<EquipmentLookup>();
            LookUpCode="EquipmentModel";
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup(LookUpCode);
            objLookup = objLookup.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlEquipmentModel, objLookup, "sLookupName", "iLookupID", "-Select All Equipment Model-");

            LookUpCode = "OwnedBy";
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup(LookUpCode);
            objLookup = objLookup.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlOwnedBy, objLookup, "sLookupName", "iLookupID", "-Select All Owned By-");

            LookUpCode = "PowerSource";
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup(LookUpCode);
            objLookup = objLookup.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlPowerSource, objLookup, "sLookupName", "iLookupID", "-Select All Power Source-");

            LookUpCode = "NewOrRefurbished";
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup(LookUpCode);
            objLookup = objLookup.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlNewRefurbished, objLookup, "sLookupName", "iLookupID", "-Select New or Refurbished-");

            LookUpCode = "PurchaseMethod";
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup(LookUpCode);
            objLookup = objLookup.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlPurchaseMethod, objLookup, "sLookupName", "iLookupID", "-Select All Purchase Method-");

            LookUpCode = "AircraftType";
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup(LookUpCode);
            objLookup = objLookup.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlAircraftType, objLookup, "sLookupName", "iLookupID", "-Select All Aircraft Type-");

            LookUpCode = "MaxAircraftWeight";
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup(LookUpCode);
            objLookup = objLookup.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlMaxAircraftWeight, objLookup, "sLookupName", "iLookupID", "-Select All Max Aircraft Weight-");

            LookUpCode = "EquipmentLife";
            objLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup(LookUpCode);
            objLookup = objLookup.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlEquipmentLife, objLookup, "sLookupName", "iLookupID", "-Select All Equipment Life-");

           
            #endregion
        }
        catch (Exception)
        {


        }
    }


    private void ResetControls()
    {
        txtEquipmentId.Text = "";
        ddlEquipmentType.SelectedIndex = 0;
        ddlBrand.SelectedIndex = 0;
        txtModelYear.Text = "";
        ddlFuelType.SelectedIndex = 0;
        txtSerialNumber.Text = "";
        txtVinNumber.Text = "";
        txtPlateNumber.Text = "";
        ddlPurchasedFrom.SelectedIndex = 0;
        txtPurchasePrice.Text = "";
        ddlBaseStation.SelectedIndex = 0;
        ddlChargeTo.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        ddlCompany.SelectedIndex = 0;
        ddlGSEDepartment.SelectedIndex = 0;
        ddlBaseStation.SelectedIndex = 0;
        ddlEquipmentModel.SelectedIndex = 0;
        ddlOwnedBy.SelectedIndex = 0;
        ddlNewRefurbished.SelectedIndex = 0;
        ddlPurchaseMethod.SelectedIndex = 0;
        ddlAircraftType.SelectedIndex = 0;
        ddlMaxAircraftWeight.SelectedIndex = 0;
        ddlEquipmentLife.SelectedIndex = 0;
        ddlPowerSource.SelectedIndex = 0;
        txtPurchasePrice.Text = "";
        txtPurchaseDate.Text = "";
        txtMaxWeight.Text = "";
        txtWarrantyParts.Text = "";
        txtWarrantyLabor.Text = "";
        txtPurchasePrice.Text = "";
    }
 
    #endregion
}
