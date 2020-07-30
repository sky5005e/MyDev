using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class AssetManagement_vLookupReasonForRepair : PageBase
{
    #region Data Members
    AssetRepairMgtRepository objAssetRepairRepository = new AssetRepairMgtRepository();
    public Int64 ReasonRepairID
    {
        get
        {
            if (this.ViewState["ReasonRepairID"] == null)
                return 0;
            else
                return Convert.ToInt64(this.ViewState["ReasonRepairID"].ToString());
        }
        set
        {
            this.ViewState["ReasonRepairID"] = value;
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
            base.MenuItem = "Drop Down Menu";
            base.ParentMenuID = 46;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Repair Reason Listing";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/DropDownMenu.aspx";
            BindDatlist();
        }
    }

    protected void dtlRepairReason_ItemCommand(object source, DataListCommandEventArgs e)
    {

        if (e.CommandName == "deletevalue")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            EquipmentLookup objLookup = objAssetRepairRepository.GetRepairReasonInfobyID(Convert.ToInt64(e.CommandArgument.ToString()));
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
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                ReasonRepairID = Convert.ToInt64(e.CommandArgument.ToString());
                EquipmentLookup objLookup = objAssetRepairRepository.GetRepairReasonInfobyID(Convert.ToInt64(e.CommandArgument.ToString()));
                if (objLookup != null)
                {
                    lblMessage.Text = string.Empty;
                    btnSubmit.Text = "Edit";
                    txtRepairReasonName.Text = objLookup.sLookupName;
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
            List<EquipmentLookup> objList = objAssetRepairRepository.CheckDuplication(txtRepairReasonName.Text.Trim());
            if (objList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                lblErrorMessage.Text = String.Empty;
                modal.Show();
            }
            else
            {
                EquipmentLookup obj = new EquipmentLookup();
                obj.sLookupName = txtRepairReasonName.Text.Trim();
                obj.iLookupCode = "RepairReason";// For Repair Reason 
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
            List<EquipmentLookup> objList = objAssetRepairRepository.CheckDuplication(txtRepairReasonName.Text.Trim());
            if (objList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                lblErrorMessage.Text = String.Empty;
                modal.Show();
            }
            else
            {
                EquipmentLookup objEquipmentLookup = objAssetRepairRepository.GetRepairReasonInfobyID(ReasonRepairID);
                if (objEquipmentLookup != null)
                {
                    objEquipmentLookup.sLookupName = txtRepairReasonName.Text.Trim();
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
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        btnSubmit.Text = "Add";
        txtRepairReasonName.Text = string.Empty;
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
            List<EquipmentLookup> objJobCode = objAssetRepairRepository.GetRepairReasonInfo(0, "RepairReason");// To get All result
            dtlRepairReason.DataSource = objJobCode;
            dtlRepairReason.DataBind();
        }
        catch { }
    }
    #endregion
}
