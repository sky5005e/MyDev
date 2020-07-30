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
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL;
public partial class AssetManagement_Inventory_BasicInfo : PageBase
{
    Int64 EquipmentInventoryID
    {
        get
        {
            if (ViewState["EquipmentInventoryID"] == null)
            {
                ViewState["EquipmentInventoryID"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentInventoryID"]);
        }
        set
        {
            ViewState["EquipmentInventoryID"] = value;
        }
    }
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
                base.MenuItem = "Station Inventory";
                base.ParentMenuID = 46;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Basic Info";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/Inventory/InventoryIndex.aspx";

                Common.MenuModuleId = Incentex.DAL.Common.DAEnums.ManageID.EquipmentInventory;
                if (Request.QueryString.Count > 0)
                {
                    this.EquipmentInventoryID = Convert.ToInt64(Request.QueryString.Get("Id"));                    
                }
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    ddlCompany.SelectedValue = Convert.ToString(IncentexGlobal.CurrentMember.CompanyId);
                    ddlCompany.Enabled = false;
                }
                else
                {
                    ddlCompany.Items.Insert(0, new ListItem("-Select-", "0"));
                    ddlCompany.Enabled = true;
                }
                menuControl.PopulateMenu(0, 0, this.EquipmentInventoryID, 0, false);
                lblMsg.Text = "";
                BindValues();
                if (Request.QueryString.Count > 0)
                {
                    PopulateValues();
                }
            }
        }
        catch (Exception)
        {


        }
    }
    protected void lnkBtnNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanAdd && !base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();
            EquipmentInventoryMaster objInventoryMaster = new EquipmentInventoryMaster();

            if (this.EquipmentInventoryID != 0)
            {
                objInventoryMaster = objInventoryRepository.GetInventoryById(this.EquipmentInventoryID);
            }

            objInventoryMaster.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            objInventoryMaster.VendorID = Convert.ToInt64(ddlVendor.SelectedValue);
            if (ddlBaseStation.SelectedValue != "0")
            {
                objInventoryMaster.BaseStationID = Convert.ToInt64(ddlBaseStation.SelectedValue);
            }

            if (this.EquipmentInventoryID == 0)
            {
                objInventoryRepository.Insert(objInventoryMaster);                
            }

            objInventoryRepository.SubmitChanges();

            if (this.EquipmentInventoryID == 0)
            {
                this.EquipmentInventoryID = objInventoryRepository.GetMaxInventoryID();               
            }
            Response.Redirect("ItemInfo.aspx?Id=" + this.EquipmentInventoryID);

        }
        catch (Exception)
        {


        }
    }
    public void BindValues()
    {
        try
        {
           
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

            //get Vendors (IEs)
            AssetVendorRepository objVendorRep = new AssetVendorRepository();
            List<EquipmentVendorMaster> objVendorList = new List<EquipmentVendorMaster>();
            objVendorList = objVendorRep.GetAllEquipmentVendor();
            Common.BindDDL(ddlVendor, objVendorList, "EquipmentVendorName", "EquipmentVendorID", "-Select Vendor-");


        }
        catch (Exception)
        {


        }
    }
    public void PopulateValues()
    {
        try
        {
             AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();
            EquipmentInventoryMaster objInventoryMaster = new EquipmentInventoryMaster();            
            objInventoryMaster = objInventoryRepository.GetInventoryById(this.EquipmentInventoryID);
            if (objInventoryMaster!=null)
            {
                ddlCompany.SelectedValue = Convert.ToString(objInventoryMaster.CompanyID);
                ddlVendor.SelectedValue = Convert.ToString(objInventoryMaster.VendorID);
                ddlBaseStation.SelectedValue = Convert.ToString(objInventoryMaster.BaseStationID);
            }
        }
        catch (Exception)
        {           
            
        }
    }
}
