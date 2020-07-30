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
public partial class AssetManagement_vLookupProductSubCategory : PageBase
{
    #region Data Members
    AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();
    public Int64 ProductSubCategoryID
    {
        get
        {
            if (this.ViewState["ProductSubCategory"] == null)
                return 0;
            else
                return Convert.ToInt64(this.ViewState["ProductSubCategory"].ToString());
        }
        set
        {
            this.ViewState["ProductSubCategory"] = value;
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

            ((Label)Master.FindControl("lblPageHeading")).Text = "Product Sub Category Listing";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/DropDownmenu.aspx";
            BindProductSubCategoryDropDown();
            BindDatlist();
        }
    }

    protected void rptProductCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            List<EquipmentProductCategoryLookup> objProductSubCategory = objInventoryRepository.GetAllProductSubCategoryDetail(Convert.ToInt64(((HiddenField)e.Item.FindControl("hdnProductCategoryID")).Value));
            if (objProductSubCategory.Count > 0)
            {
                ((DataList)e.Item.FindControl("dtlProductSubCategory")).DataSource = objProductSubCategory;
                ((DataList)e.Item.FindControl("dtlProductSubCategory")).DataBind();
            }
            else
            {
                ((Label)e.Item.FindControl("lblMsg")).Text = "No ProductSubCategory.";
            }
        }
    }

    protected void dtlProductSubCategory_ItemCommand(object source, DataListCommandEventArgs e)
    {

        if (e.CommandName == "deletevalue")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            EquipmentProductCategoryLookup objProductSubCategory = objInventoryRepository.GetProductSubCategoryByID(Convert.ToInt64(e.CommandArgument.ToString()));
            if (objProductSubCategory != null)
            {
                try
                {
                    objInventoryRepository.Delete(objProductSubCategory);
                    objInventoryRepository.SubmitChanges();
                    lblErrorMessage.Text = "ProductSubCategory deleted successfully";
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

                ProductSubCategoryID = Convert.ToInt64(e.CommandArgument.ToString());
                EquipmentProductCategoryLookup objProductSubCategory = objInventoryRepository.GetProductSubCategoryByID(ProductSubCategoryID);
                if (objProductSubCategory != null)
                {
                    lblMessage.Text = string.Empty;
                    btnSubmit.Text = "Edit";
                    ddlProductCategory.SelectedValue = objProductSubCategory.ParentProductCategoryID.ToString();
                    txtProductSubCategory.Text = objProductSubCategory.ProductCategoryName;
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
            List<EquipmentProductCategoryLookup> objProductSubCategoryList = objInventoryRepository.CheckDuplication(Convert.ToInt64(ddlProductCategory.SelectedValue), 0, txtProductSubCategory.Text.Trim());
            if (objProductSubCategoryList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                EquipmentProductCategoryLookup objProductSubCategory = new EquipmentProductCategoryLookup();
                objProductSubCategory.ParentProductCategoryID = Convert.ToInt64(ddlProductCategory.SelectedValue);
                objProductSubCategory.ProductCategoryName = txtProductSubCategory.Text.Trim();
                objInventoryRepository.Insert(objProductSubCategory);
                objInventoryRepository.SubmitChanges();
                lblErrorMessage.Text = "Record saved successfully!";
                lblMessage.Text = string.Empty;
                modal.Hide();
            }
        }
        else
        {

            //Check dulication here in edit mode
            List<EquipmentProductCategoryLookup> objProductSubCategoryList = objInventoryRepository.CheckDuplication(Convert.ToInt64(ddlProductCategory.SelectedValue), ProductSubCategoryID, txtProductSubCategory.Text.Trim());
            if (objProductSubCategoryList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                EquipmentProductCategoryLookup objProductSubCategory = objInventoryRepository.GetProductSubCategoryByID(ProductSubCategoryID);
                if (objProductSubCategory != null)
                {
                    objProductSubCategory.ParentProductCategoryID = Convert.ToInt64(ddlProductCategory.SelectedValue);
                    objProductSubCategory.ProductCategoryName = txtProductSubCategory.Text.Trim();
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
        txtProductSubCategory.Text = string.Empty;
        ddlProductCategory.ClearSelection();
        lblMessage.Text = string.Empty;
        modal.Controls.Clear();
        modal.Show();
    }
    #endregion

    #region Methods
    public void BindProductSubCategoryDropDown()
    {
        List<EquipmentProductCategoryLookup> objProductCategory = new AssetInventoryRepository().GetAllProductCategory();
        ddlProductCategory.DataSource = objProductCategory;
        ddlProductCategory.DataTextField = "ProductCategoryName";
        ddlProductCategory.DataValueField = "ProductCategoryID";
        ddlProductCategory.DataBind();
    }
    public void BindDatlist()
    {
        try
        {
            List<EquipmentProductCategoryLookup> objProductCategory = new AssetInventoryRepository().GetAllProductCategory();
            rptProductCategory.DataSource = objProductCategory;
            rptProductCategory.DataBind();
        }
        catch { }
    }
    #endregion
}
