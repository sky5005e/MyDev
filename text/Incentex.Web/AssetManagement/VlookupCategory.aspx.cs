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
public partial class AssetManagement_vLookupCategory : PageBase
{
    #region Data Members
    AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();
    public Int64 ProductCategoryID
    {
        get
        {
            if (this.ViewState["ProductCategoryID"] == null)
                return 0;
            else
                return Convert.ToInt64(this.ViewState["ProductCategoryID"].ToString());
        }
        set
        {
            this.ViewState["ProductCategoryID"] = value;
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

            ((Label)Master.FindControl("lblPageHeading")).Text = "ProductCategory Listing";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/DropDownMenu.aspx";
            BindDatlist();
        }
    }

    protected void dtlProductCategory_ItemCommand(object source, DataListCommandEventArgs e)
    {

        if (e.CommandName == "deletevalue")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            EquipmentProductCategoryLookup objProductCategoryLookup = objInventoryRepository.GetProductCategoryByID(Convert.ToInt64(e.CommandArgument.ToString()));
            if (objProductCategoryLookup != null)
            {
                try
                {
                    objInventoryRepository.Delete(objProductCategoryLookup);
                    objInventoryRepository.SubmitChanges();
                    lblErrorMessage.Text = "Product Category deleted successfully";
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

                ProductCategoryID = Convert.ToInt64(e.CommandArgument.ToString());
                EquipmentProductCategoryLookup objProductCategoryLookup = objInventoryRepository.GetProductCategoryByID(Convert.ToInt64(e.CommandArgument.ToString()));
                if (objProductCategoryLookup != null)
                {
                    lblMessage.Text = string.Empty;
                    btnSubmit.Text = "Edit";
                    txtProductCategoryName.Text = objProductCategoryLookup.ProductCategoryName;
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
            List<EquipmentProductCategoryLookup> objProductCategoryList = objInventoryRepository.CheckDuplication(0, txtProductCategoryName.Text.Trim());
            if (objProductCategoryList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                EquipmentProductCategoryLookup objProductCategory = new EquipmentProductCategoryLookup();
                objProductCategory.ProductCategoryName = txtProductCategoryName.Text.Trim();
                objInventoryRepository.Insert(objProductCategory);
                objInventoryRepository.SubmitChanges();
                lblErrorMessage.Text = "Record saved successfully!";
                lblMessage.Text = string.Empty;
                modal.Hide();
            }
        }
        else
        {

            //Check dulication here in edit mode
            List<EquipmentProductCategoryLookup> objProductCategoryList = objInventoryRepository.CheckDuplication(ProductCategoryID, txtProductCategoryName.Text.Trim());
            if (objProductCategoryList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                EquipmentProductCategoryLookup objEquipmentProductCategoryLookup = objInventoryRepository.GetProductCategoryByID(ProductCategoryID);
                if (objEquipmentProductCategoryLookup != null)
                {
                    objEquipmentProductCategoryLookup.ProductCategoryName = txtProductCategoryName.Text.Trim();
                    objInventoryRepository.SubmitChanges();
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
        txtProductCategoryName.Text = string.Empty;
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
            List<EquipmentProductCategoryLookup> objProductCategory = objInventoryRepository.GetAllProductCategory();
            dtlProductCategory.DataSource = objProductCategory;
            dtlProductCategory.DataBind();
        }
        catch { }
    }
    #endregion
}
