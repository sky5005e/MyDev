//Created by mayur for add/edot/delete sub category on sub category table
//Created on 8th-jun-2012

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_vLookupSubCategory : PageBase
{
    #region Data Members
    SubCatogeryRepository objSubCatogeryRepository = new SubCatogeryRepository();
    public Int64 SubCategoryID
    {
        get
        {
            if (this.ViewState["SubCategoryID"] == null)
                return 0;
            else
                return Convert.ToInt64(this.ViewState["SubCategoryID"].ToString());
        }
        set
        {
            this.ViewState["SubCategoryID"] = value;
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Drop-Down Management";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "SubCategory Listing";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/dropdownmenus.aspx";
            BindCategoryDropDown();
            BindDatlist();
        }
    }

    protected void rptCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            List<SubCategory> objSubCategory = objSubCatogeryRepository.GetAllSubCategory(Convert.ToInt32(((HiddenField)e.Item.FindControl("hdnCategoryID")).Value));
            if (objSubCategory.Count > 0)
            {
                ((DataList)e.Item.FindControl("dtlSubCategory")).DataSource = objSubCategory;
                ((DataList)e.Item.FindControl("dtlSubCategory")).DataBind();
            }
            else
            {
                ((Label)e.Item.FindControl("lblMsg")).Text = "No subcategory.";
            }
        }
    }

    protected void dtlSubCategory_ItemCommand(object source, DataListCommandEventArgs e)
    {

        if (e.CommandName == "deletevalue")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            SubCategory objSubCategory = objSubCatogeryRepository.GetSubCategoryByID(Convert.ToInt64(e.CommandArgument.ToString()));
            if (objSubCategory != null)
            {
                try
                {
                    objSubCatogeryRepository.Delete(objSubCategory);
                    objSubCatogeryRepository.SubmitChanges();
                    lblErrorMessage.Text = "SubCategory deleted successfully";
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

                SubCategoryID = Convert.ToInt64(e.CommandArgument.ToString());
                SubCategory objSubCategory = objSubCatogeryRepository.GetSubCategoryByID(SubCategoryID);
                if (objSubCategory != null)
                {
                    lblMessage.Text = string.Empty;
                    btnSubmit.Text = "Edit";
                    ddlCategory.SelectedValue = objSubCategory.CategoryID.ToString();
                    txtSubCategoryName.Text = objSubCategory.SubCategoryName;
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
            List<SubCategory> objSubCategoryList = objSubCatogeryRepository.CheckDuplication(Convert.ToInt64(ddlCategory.SelectedValue),0, txtSubCategoryName.Text.Trim());
            if (objSubCategoryList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                SubCategory objSubCategory = new SubCategory();
                objSubCategory.CategoryID = Convert.ToInt64(ddlCategory.SelectedValue);
                objSubCategory.SubCategoryName = txtSubCategoryName.Text.Trim();
                objSubCatogeryRepository.Insert(objSubCategory);
                objSubCatogeryRepository.SubmitChanges();
                lblErrorMessage.Text = "Record saved successfully!";
                lblMessage.Text = string.Empty;
                modal.Hide();
            }
        }
        else
        {
            //Check dulication here in edit mode
            List<SubCategory> objSubCategoryList = objSubCatogeryRepository.CheckDuplication(Convert.ToInt64(ddlCategory.SelectedValue),SubCategoryID, txtSubCategoryName.Text.Trim());
            if (objSubCategoryList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                SubCategory objSubCategory = objSubCatogeryRepository.GetSubCategoryByID(SubCategoryID);
                if (objSubCategory != null)
                {
                    objSubCategory.CategoryID = Convert.ToInt64(ddlCategory.SelectedValue);
                    objSubCategory.SubCategoryName = txtSubCategoryName.Text.Trim();
                    objSubCatogeryRepository.SubmitChanges();
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
        txtSubCategoryName.Text = string.Empty;
        ddlCategory.ClearSelection();
        lblMessage.Text = string.Empty;
        modal.Controls.Clear();
        modal.Show();
    }
    #endregion

    #region Methods
    public void BindCategoryDropDown()
    {
        List<Category> objCategory = new CatogeryRepository().GetAllCategory();
        ddlCategory.DataSource = objCategory;
        ddlCategory.DataTextField = "CategoryName";
        ddlCategory.DataValueField = "CategoryID";
        ddlCategory.DataBind();
    }
    public void BindDatlist()
    {
        try
        {
            List<Category> objCategory = new CatogeryRepository().GetAllCategory();
            rptCategory.DataSource = objCategory;
            rptCategory.DataBind();
        }
        catch { }
    }
    #endregion
}
