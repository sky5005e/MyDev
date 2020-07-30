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
public partial class AssetManagement_vLookupRepairOrderStatus : PageBase
{
    #region Data Members
    AssetRepairMgtRepository objAssetRepairRepository = new AssetRepairMgtRepository();
    public Int64 StatusRepairID
    {
        get
        {
            if (this.ViewState["StatusRepairID"] == null)
                return 0;
            else
                return Convert.ToInt64(this.ViewState["StatusRepairID"].ToString());
        }
        set
        {
            this.ViewState["StatusRepairID"] = value;
        }
    }
    #endregion
    #region Event Handlers
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
        if (!IsPostBack)
        {
            CheckLogin();
            ((Label)Master.FindControl("lblPageHeading")).Text = "Repair Status Listing";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/DropDownMenu.aspx";
            BindDatlist();
        }
    }

    protected void dtlRepairStatus_ItemCommand(object source, DataListCommandEventArgs e)
    {

        if (e.CommandName == "deletevalue")
        {
            EquipmentLookup objLookup = objAssetRepairRepository.GetRepairStatusInfobyID(Convert.ToInt64(e.CommandArgument.ToString()));
            if (objLookup != null)
            {
                try
                {
                    objAssetRepairRepository.Delete(objLookup);
                    objAssetRepairRepository.SubmitChanges();
                    lblErrorMessage.Text = "Record deleted successfully";
                }
                catch { lblErrorMessage.Text = "Unable to delete record as this record exists in other detail table"; }
            }
        }

        else if (e.CommandName == "editvalue")
        {
            try
            {
                StatusRepairID = Convert.ToInt64(e.CommandArgument.ToString());
                EquipmentLookup objLookup = objAssetRepairRepository.GetRepairStatusInfobyID(Convert.ToInt64(e.CommandArgument.ToString()));
                if (objLookup != null)
                {
                    lblMessage.Text = string.Empty;
                    btnSubmit.Text = "Edit";
                    txtRepairStatusName.Text = objLookup.sLookupName;
                    modal.Show();
                }
            }
            catch
            {
            }
        }
        BindDatlist();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (btnSubmit.Text == "Add")
        {
            //Check dulication here when add new record.
            List<EquipmentLookup> objList = objAssetRepairRepository.CheckDuplication(txtRepairStatusName.Text.Trim());
            if (objList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                lblErrorMessage.Text = String.Empty;
                modal.Show();
            }
            else
            {
                EquipmentLookup obj = new EquipmentLookup();
                obj.sLookupName = txtRepairStatusName.Text.Trim();
                obj.iLookupCode = "RepairStatus";// For Repair Status 
                objAssetRepairRepository.Insert(obj);
                objAssetRepairRepository.SubmitChanges();
                lblErrorMessage.Text = "Record saved successfully!";
                lblMessage.Text = string.Empty;
                modal.Hide();
            }
        }
        else
        {
            //Check dulication here when add new record.
            List<EquipmentLookup> objList = objAssetRepairRepository.CheckDuplication(txtRepairStatusName.Text.Trim());
            if (objList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                lblErrorMessage.Text = String.Empty;
                modal.Show();
            }
            else
            {
                EquipmentLookup objEquipmentLookup = objAssetRepairRepository.GetRepairStatusInfobyID(StatusRepairID);
                if (objEquipmentLookup != null)
                {
                    objEquipmentLookup.sLookupName = txtRepairStatusName.Text.Trim();
                    objAssetRepairRepository.SubmitChanges();
                    lblErrorMessage.Text = "Record updated successfully!";
                    lblMessage.Text = string.Empty;
                    modal.Hide();
                }
            }
        }
        BindDatlist();
    }

    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        btnSubmit.Text = "Add";
        txtRepairStatusName.Text = string.Empty;
        lblMessage.Text = string.Empty;
        modal.Controls.Clear();
        modal.Show();
    }
    #endregion

    #region Methods
    public void BindDatlist()
    {
        try
        {
            List<EquipmentLookup> objJobCode = objAssetRepairRepository.GetRepairStatusInfo(0, "RepairStatus");// To get All result
            dtlRepairStatus.DataSource = objJobCode;
            dtlRepairStatus.DataBind();
        }
        catch { }
    }
    #endregion
}
