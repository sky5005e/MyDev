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
using System.Collections.Generic;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class AssetManagement_vLookupJobSubCode : PageBase
{
    #region Data Members
    AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
    public Int64 SubJobCodeID
    {
        get
        {
            if (this.ViewState["SubJobCodeID"] == null)
                return 0;
            else
                return Convert.ToInt64(this.ViewState["SubJobCodeID"].ToString());
        }
        set
        {
            this.ViewState["SubJobCodeID"] = value;
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

            ((Label)Master.FindControl("lblPageHeading")).Text = "SubJobCode Listing";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/DropDownmenu.aspx";
            BindJobCodeDropDown();
            BindDatlist();
        }
    }

    protected void rptJobCode_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            List<EquipmentJobCodeLookup> objSubJobCode = objAssetMgtRepository.GetAllSubJobCodeDetail(Convert.ToInt64(((HiddenField)e.Item.FindControl("hdnJobCodeID")).Value));
            if (objSubJobCode.Count > 0)
            {
                ((DataList)e.Item.FindControl("dtlSubJobCode")).DataSource = objSubJobCode;
                ((DataList)e.Item.FindControl("dtlSubJobCode")).DataBind();
            }
            else
            {
                ((Label)e.Item.FindControl("lblMsg")).Text = "No subJobCode.";
            }
        }
    }

    protected void dtlSubJobCode_ItemCommand(object source, DataListCommandEventArgs e)
    {

        if (e.CommandName == "deletevalue")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            EquipmentJobCodeLookup objSubJobCode = objAssetMgtRepository.GetSubJobCodeByID(Convert.ToInt64(e.CommandArgument.ToString()));
            if (objSubJobCode != null)
            {
                try
                {
                    

                    objAssetMgtRepository.Delete(objSubJobCode);
                    objAssetMgtRepository.SubmitChanges();
                    lblErrorMessage.Text = "SubJobCode deleted successfully";
                }
                catch
                {
                    lblErrorMessage.Text = "Unable to delete record as this record exists in other detail table";
                }
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

                SubJobCodeID = Convert.ToInt64(e.CommandArgument.ToString());
                EquipmentJobCodeLookup objSubJobCode = objAssetMgtRepository.GetSubJobCodeByID(SubJobCodeID);
                if (objSubJobCode != null)
                {
                    lblMessage.Text = string.Empty;
                    btnSubmit.Text = "Edit";
                    ddlJobCode.SelectedValue = objSubJobCode.ParentJobCode.ToString();
                    txtSubJobCodeName.Text = objSubJobCode.JobCodeName;
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
            List<EquipmentJobCodeLookup> objSubJobCodeList = objAssetMgtRepository.CheckDuplication(Convert.ToInt64(ddlJobCode.SelectedValue), 0, txtSubJobCodeName.Text.Trim());
            if (objSubJobCodeList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                EquipmentJobCodeLookup objSubJobCode = new EquipmentJobCodeLookup();
                objSubJobCode.ParentJobCode = Convert.ToInt64(ddlJobCode.SelectedValue);
                objSubJobCode.JobCodeName = txtSubJobCodeName.Text.Trim();
                objAssetMgtRepository.Insert(objSubJobCode);
                objAssetMgtRepository.SubmitChanges();
                lblErrorMessage.Text = "Record saved successfully!";
                lblMessage.Text = string.Empty;
                modal.Hide();
            }
        }
        else
        {

            //Check dulication here in edit mode
            List<EquipmentJobCodeLookup> objSubJobCodeList = objAssetMgtRepository.CheckDuplication(Convert.ToInt64(ddlJobCode.SelectedValue), SubJobCodeID, txtSubJobCodeName.Text.Trim());
            if (objSubJobCodeList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                EquipmentJobCodeLookup objSubJobCode = objAssetMgtRepository.GetSubJobCodeByID(SubJobCodeID);
                if (objSubJobCode != null)
                {
                    objSubJobCode.ParentJobCode = Convert.ToInt64(ddlJobCode.SelectedValue);
                    objSubJobCode.JobCodeName = txtSubJobCodeName.Text.Trim();
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
        txtSubJobCodeName.Text = string.Empty;
        ddlJobCode.ClearSelection();
        lblMessage.Text = string.Empty;
        modal.Controls.Clear();
        modal.Show();
    }
    #endregion

    #region Methods
    public void BindJobCodeDropDown()
    {
        List<EquipmentJobCodeLookup> objJobCode = new AssetMgtRepository().GetAllJobCode();
        ddlJobCode.DataSource = objJobCode;
        ddlJobCode.DataTextField = "JobCodeName";
        ddlJobCode.DataValueField = "JobCodeID";
        ddlJobCode.DataBind();
    }
    public void BindDatlist()
    {
        try
        {
            List<EquipmentJobCodeLookup> objJobCode = new AssetMgtRepository().GetAllJobCode();
            rptJobCode.DataSource = objJobCode;
            rptJobCode.DataBind();
        }
        catch { }
    }
    #endregion
}
