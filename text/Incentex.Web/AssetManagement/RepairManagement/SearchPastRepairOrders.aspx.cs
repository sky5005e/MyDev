using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Data;
public partial class AssetManagement_RepairManagement_SearchPastRepairOrders : PageBase
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
                base.MenuItem = "Repair Order Management";
                base.ParentMenuID = 46;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Search Repair Orders";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/RepairManagement/RepairManagementIndex.aspx";
                BindValues();

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
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void lnkBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {


            string EquipmentTypeID = ddlEquipmentType.SelectedValue != "0" ? ddlEquipmentType.SelectedValue : null;
            string RepairOrderId = txtRepairOrderId.Text != "" ? txtRepairOrderId.Text : null;
            string BaseStationID = ddlBaseStation.SelectedValue != "0" ? ddlBaseStation.SelectedValue : null;
            string EquipmentStatus = ddlStatus.SelectedValue != "0" ? ddlStatus.SelectedValue : null;
            string CompanyID = ddlCompany.SelectedValue != "0" ? ddlCompany.SelectedValue : null;
            string FromDate = txtFromDate.Text != "" ? txtFromDate.Text : null;
            string ToDate = txtToDate.Text != "" ? txtToDate.Text : null;

            Response.Redirect("~/AssetManagement/RepairManagement/ViewOpenRepairOrders.aspx?EquipmentTypeID=" + EquipmentTypeID + "&RepairOrderId=" + RepairOrderId + "&BaseStationID=" + BaseStationID + "&EquipmentStatus=" + EquipmentStatus + "&CompanyID=" + CompanyID + "&ToDate=" + ToDate + "&FromDate=" + FromDate + "&search=1" + "&IsBillingComplete=true");
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
            Common.BindDDL(ddlEquipmentType, objList, "sLookupName", "iLookupID", "-Select-");

            //get Status
            LookUpCode = "EquipmentStatus";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            objList = objList.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlStatus, objList, "sLookupName", "iLookupID", "-Select-");

            //get Base Stations
            CompanyRepository objRepo = new CompanyRepository();
            List<INC_BasedStation> objBaseStationList = new List<INC_BasedStation>();
            objBaseStationList = objRepo.GetAllBaseStationResult();
            objBaseStationList = objBaseStationList.OrderBy(p => p.sBaseStation).ToList();
            Common.BindDDL(ddlBaseStation, objBaseStationList, "sBaseStation", "iBaseStationId", "-Select-");

            //get company
            CompanyRepository objCompRep = new CompanyRepository();
            ddlCompany.DataSource = objCompRep.GetAllQuery().OrderBy(le => le.CompanyName);
            ddlCompany.DataValueField = "CompanyID";
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataBind();

        }
        catch (Exception)
        {


        }
    }

    #endregion
}
