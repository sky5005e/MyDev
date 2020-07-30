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
using System.Collections.Generic;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
public partial class AssetManagement_AccessReports : PageBase
{
    #region Data Member
    AssetReportRepository objReportRepository = new AssetReportRepository();
    UserInformationRepository objUserInformationRepository = new UserInformationRepository();
    LookupRepository objLookupRepos = new LookupRepository();    
    Int64 VendorID
    {
        get
        {
            if (ViewState["VendorID"] == null)
            {
                ViewState["VendorID"] = 0;
            }
            return Convert.ToInt64(ViewState["VendorID"]);
        }
        set
        {
            ViewState["VendorID"] = value;
        }
    }
    Int64 VendorEmployeeID
    {
        get
        {
            if (ViewState["VendorEmployeeID"] == null)
            {
                ViewState["VendorEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["VendorEmployeeID"]);
        }
        set
        {
            ViewState["VendorEmployeeID"] = value;
        }
    }
    #endregion
    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Employee";
            base.ParentMenuID = 50;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            if (Request.QueryString.Count > 0)
            {
                this.VendorID = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.VendorEmployeeID = Convert.ToInt64(Request.QueryString.Get("SubId"));
                ((Label)Master.FindControl("lblPageHeading")).Text = "Report Access Rights";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/VendorEmployeeAttachments.aspx?Id=" + this.VendorID + "&SubId=" + this.VendorEmployeeID;
                menucontrol.PopulateMenu(5, 0, this.VendorEmployeeID, this.VendorID, false);
                FillReportType();
            }
            else
            {
                Response.Redirect("EmployeeList.aspx");
            }

        }
    }
    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
       
        //Bind Equipment Type       
        gvEquipmentType.DataSource = objLookupRepos.GetByLookup("EquipmentType");
        gvEquipmentType.DataBind();

        //Bind Base station 
        //CompanyRepository objRepo = new CompanyRepository();
        //List<INC_BasedStation> objBaseStationList = new List<INC_BasedStation>();
        //gvBaseStation.DataSource = objRepo.GetAllBaseStationResult();
        //gvBaseStation.DataBind();
        //BindExistingData();
       
        AssetVendorRepository objVendorRepo = new AssetVendorRepository();
        Int64 CountryID = objVendorRepo.GetVendorEmpCountrybyID(this.VendorEmployeeID);//Get Country
        BaseStationDA sBaseStation = new BaseStationDA();
        BasedStationBE sBSBe = new BasedStationBE();
        sBSBe.SOperation = "getBaseStationbyCounty";
        sBSBe.iCountryID = CountryID;
        DataSet dsBaseStation = sBaseStation.GetBaseStaionByCountry(sBSBe);
        if (dsBaseStation.Tables.Count > 0 && dsBaseStation.Tables[0].Rows.Count > 0)
        {
            gvBaseStation.DataSource = dsBaseStation.Tables[0];
            gvBaseStation.DataBind();
        }
        dvEquipmentType.Visible = true;
        dvBaseStation.Visible = true;
        BindExistingData();
        }
        catch (Exception)
        {
        }
    }


    protected void lnkbtnSave_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        try
        {
            List<string> EquipmentTypeIDList = new List<string>();
            foreach (GridViewRow gr in gvEquipmentType.Rows)
            {
                CheckBox chk = gr.FindControl("ChkEquipmentType") as CheckBox;
                HiddenField lblId = gr.FindControl("hdnEquipmentTypeID") as HiddenField;

                if (chk.Checked)
                {
                    EquipmentTypeIDList.Add(lblId.Value);
                }
            }


            List<string> BaseStationIDList = new List<string>();
            foreach (GridViewRow gr in gvBaseStation.Rows)
            {
                CheckBox chk = gr.FindControl("ChkBaseStation") as CheckBox;
                HiddenField lblId = gr.FindControl("hdnBaseStationID") as HiddenField;

                if (chk.Checked)
                {
                    BaseStationIDList.Add(lblId.Value);
                }
            }

            //first delete previous entry for this parent report
            EquipmentReportAccessRight objReportAccRightDel = objReportRepository.GetByVEmpIDAndReportTypeID(this.VendorEmployeeID, Convert.ToInt64(ddlReportType.SelectedValue));
            if (objReportAccRightDel != null)
            {
                objReportRepository.Delete(objReportAccRightDel);
                objReportRepository.SubmitChanges();
            }

            //Add new record for this parent report
            EquipmentReportAccessRight objReportAccessRight = new EquipmentReportAccessRight();
            objReportAccessRight.VendorEmployeeID = this.VendorEmployeeID;
            objReportAccessRight.ReportTypeID = Convert.ToInt32(ddlReportType.SelectedValue);
            objReportAccessRight.EquipmentTypeID = EquipmentTypeIDList != null && EquipmentTypeIDList.Count > 0 ? string.Join(",", EquipmentTypeIDList.ToArray()) : null;
            objReportAccessRight.BaseStationID = BaseStationIDList != null && BaseStationIDList.Count > 0 ? string.Join(",", BaseStationIDList.ToArray()) : null;
          
            objReportRepository.Insert(objReportAccessRight);
            objReportRepository.SubmitChanges();

            lblMsg.Text = "Record Saved Successfully.";

            BindExistingData();
            Response.Redirect("VendorEmpUserSetting.aspx?SubId=" + this.VendorEmployeeID + "&Id=" + this.VendorID);

        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in saving record ...";
            ErrHandler.WriteError(ex);
        }
    }
    #endregion
    #region Methods
    public void FillReportType()
    {
        ddlReportType.DataSource = objReportRepository.GetAllReportType();
        ddlReportType.DataValueField = "ReportTypeID";
        ddlReportType.DataTextField = "ReportTypeName";
        ddlReportType.DataBind();
        ddlReportType.Items.Insert(0, new ListItem("-select-", "0"));
    }
   
    protected void BindExistingData()
    {
        //For Display data
        EquipmentReportAccessRight objReportAccessRight = objReportRepository.GetByVEmpIDAndReportTypeID(this.VendorEmployeeID, Convert.ToInt64(ddlReportType.SelectedValue));

        if (objReportAccessRight != null)
        {
            if (objReportAccessRight.EquipmentTypeID != null)
            {
                string[] Ids = objReportAccessRight.EquipmentTypeID.Split(',');
                foreach (GridViewRow gv in gvEquipmentType.Rows)
                {
                    CheckBox chk = gv.FindControl("ChkEquipmentType") as CheckBox;
                    HiddenField lblId = gv.FindControl("hdnEquipmentTypeID") as HiddenField;
                    HtmlGenericControl dvChk = gv.FindControl("dvChkEquipmentType") as HtmlGenericControl;

                    foreach (string i in Ids)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "wheather_checked");
                            break;
                        }
                        else
                        {
                            chk.Checked = false;
                            dvChk.Attributes.Add("class", "wheather_check");
                        }
                    }
                }
            }
            if (objReportAccessRight.BaseStationID != null)
            {
                string[] Ids = objReportAccessRight.BaseStationID.Split(',');
                foreach (GridViewRow gv in gvBaseStation.Rows)
                {
                    CheckBox chk = gv.FindControl("ChkBaseStation") as CheckBox;
                    HiddenField lblId = gv.FindControl("hdnBaseStationID") as HiddenField;
                    HtmlGenericControl dvChk = gv.FindControl("dvChkBaseStation") as HtmlGenericControl;

                    foreach (string i in Ids)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "wheather_checked");
                            break;
                        }
                        else
                        {
                            chk.Checked = false;
                            dvChk.Attributes.Add("class", "wheather_check");
                        }
                    }
                }
            }
        }
    }
    #endregion
}
