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
using System.Web.UI.DataVisualization.Charting;
public partial class AssetManagement_Reports_EquipmentStatusReport : PageBase
{
    #region Data Members
    AssetReportRepository objReportRepository = new AssetReportRepository();
    Int64 CompanyID
    {
        get
        {
            if (ViewState["CompanyID"] == null)
            {
                ViewState["CompanyID"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyID"]);
        }
        set
        {
            ViewState["CompanyID"] = value;
        }
    }
    Int64 BaseStationID
    {
        get
        {
            if (ViewState["BaseStationID"] == null)
            {
                ViewState["BaseStationID"] = 0;
            }
            return Convert.ToInt64(ViewState["BaseStationID"]);
        }
        set
        {
            ViewState["BaseStationID"] = value;
        }
    }
    Int64 EquipmentTypeID
    {
        get
        {
            if (ViewState["EquipmentTypeID"] == null)
            {
                ViewState["EquipmentTypeID"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentTypeID"]);
        }
        set
        {
            ViewState["EquipmentTypeID"] = value;
        }
    }
    String FDate
    {
        get
        {
            if (ViewState["FDate"] == null)
            {
                ViewState["FDate"] = "1/1/2012";
            }
            return Convert.ToString(ViewState["FDate"]);
        }
        set
        {
            ViewState["FDate"] = value;
        }
    }
    String TDate
    {
        get
        {
            if (ViewState["TDate"] == null)
            {
                ViewState["TDate"] = DateTime.Now.ToString();
            }
            return Convert.ToString(ViewState["TDate"]);
        }
        set
        {
            ViewState["TDate"] = value;
        }
    }
   
    #endregion
    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Equipment Status Report";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/Reports/ReportDashBoard.aspx";
           
            if (Request.QueryString["FromDate"] != "")
                FDate = Request.QueryString["FromDate"];
            if (Request.QueryString["ToDate"] != "")
                TDate = Request.QueryString["ToDate"];
            if (Request.QueryString["CompanyID"] != "0")
            {
                 this.CompanyID = Convert.ToInt64(Request.QueryString["CompanyID"]);
            }
            if (string.IsNullOrEmpty(Request.QueryString["BaseStationID"])==false)
            {
                this.BaseStationID = Convert.ToInt64(Request.QueryString["BaseStationID"]);
            }
            if (string.IsNullOrEmpty(Request.QueryString["EquipmentTypeID"]) == false)
            {
                this.EquipmentTypeID = Convert.ToInt64(Request.QueryString["EquipmentTypeID"]);
            }
           
            GenerateChart();
        }
    }
    protected void chrtStatusReport_Click(object sender, ImageMapEventArgs e)
    {

        try
        {
            AssetReportRepository objReportRepository = new AssetReportRepository();
            Int64 EquipmentStatusID = objReportRepository.GetEquiStatusIDByName(e.PostBackValue);
            if (EquipmentStatusID!=0)
            {
                Response.Redirect("~/AssetManagement/ManageEquipment.aspx?FrmPage=ESReport&EquipmentTypeID=" + this.EquipmentTypeID + "&BaseStationID=" + this.BaseStationID + "&EquipmentStatus=" + EquipmentStatusID + "&CompanyID=" + this.CompanyID);
            }           
        }
        catch (Exception)
        {}
       
    }
    #endregion

    #region Methods
    protected void GenerateChart()
    {
        try
        {           
            List<AssetReportRepository.GetEquipmentStatusResult> objResult = new List<AssetReportRepository.GetEquipmentStatusResult>();
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                objResult = objReportRepository.GetEquipmentStatusVE(Convert.ToDateTime(this.FDate), Convert.ToDateTime(this.TDate), this.CompanyID, this.EquipmentTypeID, this.BaseStationID, IncentexGlobal.CurrentMember.UserInfoID);
            else
                objResult = objReportRepository.GetEquipmentStatus(Convert.ToDateTime(this.FDate), Convert.ToDateTime(this.TDate), this.CompanyID, this.EquipmentTypeID, this.BaseStationID);
            chrtStatusReport.DataSource = objResult;
            chrtStatusReport.DataBind();
            dvReport.Visible = true;
        }
        catch (Exception)
        { }
    }
    #endregion
}
