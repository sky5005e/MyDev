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

public partial class AssetManagement_vLookupJobCode : PageBase
{
    #region Data Members
    AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
    public Int64 JobCodeID
    {
        get
        {
            if (this.ViewState["JobCodeID"] == null)
                return 0;
            else
                return Convert.ToInt64(this.ViewState["JobCodeID"].ToString());
        }
        set
        {
            this.ViewState["JobCodeID"] = value;
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

            ((Label)Master.FindControl("lblPageHeading")).Text = "JobCode Listing";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/DropDownMenu.aspx";
            BindDatlist();
        }
    }

    protected void dtlJobCode_ItemCommand(object source, DataListCommandEventArgs e)
    {

        if (e.CommandName == "deletevalue")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            EquipmentJobCodeLookup objJobCodeLookup = objAssetMgtRepository.GetJobCodeByID(Convert.ToInt64(e.CommandArgument.ToString()));
            if (objJobCodeLookup != null)
            {
                try
                {
                    objAssetMgtRepository.Delete(objJobCodeLookup);
                    objAssetMgtRepository.SubmitChanges();
                    lblErrorMessage.Text = "JobCode deleted successfully";
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

                JobCodeID = Convert.ToInt64(e.CommandArgument.ToString());
                EquipmentJobCodeLookup objJobCodeLookup = objAssetMgtRepository.GetJobCodeByID(Convert.ToInt64(e.CommandArgument.ToString()));
                if (objJobCodeLookup != null)
                {
                    lblMessage.Text = string.Empty;
                    btnSubmit.Text = "Edit";
                    txtJobCodeName.Text = objJobCodeLookup.JobCodeName;
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
            List<EquipmentJobCodeLookup> objJobCodeList = objAssetMgtRepository.CheckDuplication(0, txtJobCodeName.Text.Trim());
            if (objJobCodeList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                EquipmentJobCodeLookup objJobCode = new EquipmentJobCodeLookup();
                objJobCode.JobCodeName = txtJobCodeName.Text.Trim();
                objAssetMgtRepository.Insert(objJobCode);
                objAssetMgtRepository.SubmitChanges();
                lblErrorMessage.Text = "Record saved successfully!";
                lblMessage.Text = string.Empty;
                modal.Hide();
            }
        }
        else
        {

            //Check dulication here in edit mode
            List<EquipmentJobCodeLookup> objJobCodeList = objAssetMgtRepository.CheckDuplication(JobCodeID, txtJobCodeName.Text.Trim());
            if (objJobCodeList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                EquipmentJobCodeLookup objEquipmentJobCodeLookup = objAssetMgtRepository.GetJobCodeByID(JobCodeID);
                if (objEquipmentJobCodeLookup != null)
                {
                    objEquipmentJobCodeLookup.JobCodeName = txtJobCodeName.Text.Trim();
                    objAssetMgtRepository.SubmitChanges();
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
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        btnSubmit.Text = "Add";
        txtJobCodeName.Text = string.Empty;
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
            List<EquipmentJobCodeLookup> objJobCode = objAssetMgtRepository.GetAllJobCode();
            dtlJobCode.DataSource = objJobCode;
            dtlJobCode.DataBind();
        }
        catch { }
    }
    #endregion
}
