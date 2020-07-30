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

public partial class AssetManagement_SearchEquipment : PageBase
{
    #region DataMembers
    CompanyEmployee objCmpnyInfo = new CompanyEmployee();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    UserInformation objuser = new UserInformation();
    UserInformationRepository objuserrepos = new UserInformationRepository();
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
                base.MenuItem = "Search GSE Equipment";
                base.ParentMenuID = 46;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Search Equipment Inventory";
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

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    ddlCompanyStore.SelectedValue = Convert.ToString(IncentexGlobal.CurrentMember.CompanyId);
                    ddlCompanyStore.Enabled = false;
                }
                else
                {
                    ddlCompanyStore.Items.Insert(0, new ListItem("-Select-", "0"));
                    ddlCompanyStore.Enabled = true;
                }
            }
        }
        catch (Exception)
        {

        }
    }

    protected void lnkBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {


            string EquipmentTypeID = ddlEquipmentType.SelectedValue != "0" ? ddlEquipmentType.SelectedValue : null;
            string EquipmentID = txtEquipmentId.Text != "" ? txtEquipmentId.Text : null;
            string BaseStationID = ddlBaseStation.SelectedValue != "0" ? ddlBaseStation.SelectedValue : null;
            string EquipmentStatus = ddlEquipmentStatus.SelectedValue != "0" ? ddlEquipmentStatus.SelectedValue : null;
            string CompanyID = ddlCompanyStore.SelectedValue != "0" ? ddlCompanyStore.SelectedValue : null;
            string GSEDepartment = ddlGSEDepartment.SelectedValue != "0" ? ddlGSEDepartment.SelectedValue : null;

            
            //Response.Redirect("~/AssetManagement/ManageEquipment.aspx?EquipmentTypeID=" + EquipmentTypeID + "&EquipmentID=" + EquipmentID + "&CurrentLocationID=" + CurrentLocationID + "&EquipmentStatus=" + EquipmentStatus + "&CompanyID=" + CompanyID + "&WorkgroupID=" + WorkgroupID);
            Response.Redirect("~/AssetManagement/ManageEquipment.aspx?EquipmentTypeID=" + EquipmentTypeID + "&EquipmentID=" + EquipmentID + "&BaseStationID=" + BaseStationID + "&EquipmentStatus=" + EquipmentStatus + "&CompanyID=" + CompanyID + "&GSEDepartmentID=" + GSEDepartment);
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

            //get Status
            LookUpCode = "EquipmentStatus";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            objList = objList.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlEquipmentStatus, objList, "sLookupName", "iLookupID", "-Select Status-");

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
            Common.BindDDL(ddlCompanyStore, objCompanyList, "CompanyName", "CompanyID", "-Select Company-");

        }
        catch (Exception)
        {


        }
    }
    
    #endregion
}
