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
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
public partial class AssetManagement_Reports_ReportDashBoard : PageBase
{
    #region Data Members
    LookupRepository objLookupRepository = new LookupRepository();
    #endregion
    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Planning Reports";
            base.ParentMenuID = 46;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>"; ;
            ((Label)Master.FindControl("lblPageHeading")).Text = "Management Reporting Filter Criteria";
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Index.aspx";
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/AssetMgtIndex.aspx";
            }
            FillDropdowns();
            txtFromDate.Text = "01/01/" + DateTime.Now.Year;
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

    protected void lnkSubmitRequest_Click(object sender, EventArgs e)
    {
        if (ddlReportType.SelectedItem.Text.Contains("Equipment Status"))
        {
            Response.Redirect("EquipmentStatusReport.aspx?CompanyID=" + ddlCompany.SelectedValue + "&BaseStationID=" + ddlBaseStation.SelectedValue + "&EquipmentTypeID=" + ddlEquipmentType.SelectedValue + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim());
        }
        else if (ddlReportType.SelectedItem.Text.Contains("Spend by Asset"))
        {
            Response.Redirect("SpendByAsset.aspx?CompanyID=" + ddlCompany.SelectedValue + "&BaseStationID=" + ddlBaseStation.SelectedValue + "&EquipmentTypeID=" + ddlEquipmentType.SelectedValue + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim());
        }
        else if (ddlReportType.SelectedItem.Text.Contains("Parts Purchase"))
        {
            Response.Redirect("PartsPurchasedReport.aspx?CompanyID=" + ddlCompany.SelectedValue + "&BaseStationID=" + ddlBaseStation.SelectedValue + "&EquipmentTypeID=" + ddlEquipmentType.SelectedValue + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim());
        }
       
    }
    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReportType.SelectedItem.Text.Contains("Equipment Status"))
        {
            dvFDate.Visible = false;
            dvTDate.Visible = false;
        }
        else if (ddlReportType.SelectedItem.Text.Contains("Spend by Asset"))
        {
            dvFDate.Visible = true;
            dvTDate.Visible = true;
        }
        else if (ddlReportType.SelectedItem.Text.Contains("Parts Purchase"))
        {
            dvFDate.Visible = true;
            dvTDate.Visible = true;
        }
    }
    #endregion
    #region Methods
    private void FillDropdowns()
    {
        try
        {
            //Company
            CompanyRepository objCompanyRepo = new CompanyRepository();
            List<Company> objCompanyList = new List<Company>();
            objCompanyList = objCompanyRepo.GetAllCompany();
            Common.BindDDL(ddlCompany, objCompanyList, "CompanyName", "CompanyID", "-Select Company-");

            //Report Type
            AssetReportRepository objReportRepository = new AssetReportRepository();
            ddlReportType.DataSource = objReportRepository.GetAllReportType();
            ddlReportType.DataValueField = "ReportTypeID";
            ddlReportType.DataTextField = "ReportTypeName";
            ddlReportType.DataBind();
            ddlReportType.Items.Insert(0, new ListItem("-select-", "0"));


            //get Equipment Type according to given rights 
            LookupRepository objLookupRepository = new LookupRepository();
            List<INC_Lookup> objList = new List<INC_Lookup>();
            String LookUpCode = string.Empty;
            LookUpCode = "EquipmentType";
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                objList = objReportRepository.GetEquiTypebyUserInfoID(IncentexGlobal.CurrentMember.UserInfoID, 1);
            }
            else
            {
                objList = objLookupRepository.GetByLookup(LookUpCode);
            }
            objList = objList.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlEquipmentType, objList, "sLookupName", "iLookupID", "-Select Equipment Type-");
            //get Base Stations according to given rights 
            CompanyRepository objRepo = new CompanyRepository();
            List<INC_BasedStation> objBaseStationList = new List<INC_BasedStation>();
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                objBaseStationList = objReportRepository.GetBaseStationbyUserInfoID(IncentexGlobal.CurrentMember.UserInfoID, 1);
            }
            else
            {
                objBaseStationList = objRepo.GetAllBaseStationResult();
            }
            objBaseStationList = objBaseStationList.OrderBy(p => p.sBaseStation).ToList();
            Common.BindDDL(ddlBaseStation, objBaseStationList, "sBaseStation", "iBaseStationId", "-Select All BaseSation-");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
   
    #endregion

   
}
