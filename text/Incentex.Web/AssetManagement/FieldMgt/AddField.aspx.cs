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
public partial class AssetManagement_FieldMgt_AddField : PageBase
{
    Int64 FieldMasterID
    {
        get
        {
            if (ViewState["FieldMasterID"] == null)
            {
                ViewState["FieldMasterID"] = 0;
            }
            return Convert.ToInt64(ViewState["FieldMasterID"]);
        }
        set
        {
            ViewState["FieldMasterID"] = value;
        }
    }
    AssetFieldRepository objFieldRepository = new AssetFieldRepository();
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
                base.MenuItem = "Field Management";
                base.ParentMenuID = 46;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Add Field";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/FieldMgt/FieldMgtIndex.aspx";

                lblMsg.Text = "";
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
                if (Request.QueryString.Count > 0)
                {
                    this.FieldMasterID = Convert.ToInt64(Request.QueryString.Get("Id"));
                    PopulateValues();
                }
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
            if (!base.CanAdd && !base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            EquipmentFieldMaster objFieldMaster = new EquipmentFieldMaster();
            if (this.FieldMasterID != 0)
            {
                objFieldMaster = objFieldRepository.GetFieldMasterById(this.FieldMasterID);
            }           
            objFieldMaster.EquipmentTypeID = Convert.ToInt64(ddlEquipmentType.SelectedValue);
            objFieldMaster.FieldMasterName = txtField.Text;
            objFieldMaster.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            if (this.FieldMasterID==0)
            {
                objFieldRepository.Insert(objFieldMaster);
            }           
            objFieldRepository.SubmitChanges();
            lblMsg.Text = "Record Saved Successfully";
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
            ResetControls();
            //get company
            CompanyRepository objCompanyRepo = new CompanyRepository();
            List<Company> objCompanyList = new List<Company>();
            objCompanyList = objCompanyRepo.GetAllCompany();
            Common.BindDDL(ddlCompany, objCompanyList, "CompanyName", "CompanyID", "-Select Company-");

        }
        catch (Exception)
        {


        }
    }

    public void PopulateValues()
    {
        try
        {

            EquipmentFieldMaster objFieldMaster = new EquipmentFieldMaster();
            objFieldMaster = objFieldRepository.GetFieldMasterById(this.FieldMasterID);
            if (objFieldMaster != null)
            {
                ddlCompany.SelectedValue = Convert.ToString(objFieldMaster.CompanyID);
                ddlEquipmentType.SelectedValue = Convert.ToString(objFieldMaster.EquipmentTypeID);
                txtField.Text = Convert.ToString(objFieldMaster.FieldMasterName);
            }
        }
        catch (Exception)
        {

        }
    }

    private void ResetControls()
    {
        ddlCompany.SelectedIndex = 0;
        ddlEquipmentType.SelectedIndex = 0;
        txtField.Text = "";
       
    }
   
    #endregion
}
