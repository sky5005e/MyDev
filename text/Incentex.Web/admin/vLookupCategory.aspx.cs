//Created by mayur for add/edot/delete category on category table
//Created on 8th-jun-2012

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_vLookupCategory : PageBase
{
    #region Data Members
    CatogeryRepository objCatogeryRepository = new CatogeryRepository();
    public Int64 CategoryID
    {
        get
        {
            if (this.ViewState["CategoryID"] == null)
                return 0;
            else
                return Convert.ToInt64(this.ViewState["CategoryID"].ToString());
        }
        set
        {
            this.ViewState["CategoryID"] = value;
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

            ((Label)Master.FindControl("lblPageHeading")).Text = "Category Listing";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/dropdownmenus.aspx";
            BindDatlist();
        }
    }

    protected void dtlCategory_ItemCommand(object source, DataListCommandEventArgs e)
    {

        if (e.CommandName == "deletevalue")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

             Category objCategory = objCatogeryRepository.GetCategoryByID(Convert.ToInt64(e.CommandArgument.ToString()));
             if (objCategory != null)
             {
                 try
                 {
                     objCatogeryRepository.Delete(objCategory);
                     objCatogeryRepository.SubmitChanges();
                     lblErrorMessage.Text = "Category deleted successfully";
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

                CategoryID = Convert.ToInt64(e.CommandArgument.ToString());
                Category objCategory = objCatogeryRepository.GetCategoryByID(Convert.ToInt64(e.CommandArgument.ToString()));
                if (objCategory != null)
                {
                    lblMessage.Text = string.Empty;
                    btnSubmit.Text = "Edit";
                    txtCategoryName.Text = objCategory.CategoryName;
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
            List<Category> objCategoryList = objCatogeryRepository.CheckDuplication(0, txtCategoryName.Text.Trim());
            if (objCategoryList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                Category objCategory = new Category();
                objCategory.CategoryName = txtCategoryName.Text.Trim();
                objCatogeryRepository.Insert(objCategory);
                objCatogeryRepository.SubmitChanges();
                lblErrorMessage.Text = "Record saved successfully!";
                lblMessage.Text = string.Empty;
                modal.Hide();
            }
        }
        else
        {

            //Check dulication here in edit mode
            List<Category> objCategoryList = objCatogeryRepository.CheckDuplication(CategoryID, txtCategoryName.Text.Trim());
            if (objCategoryList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                Category objCategory = objCatogeryRepository.GetCategoryByID(CategoryID);
                if (objCategory != null)
                {
                    objCategory.CategoryName = txtCategoryName.Text.Trim();
                    objCatogeryRepository.SubmitChanges();
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
        txtCategoryName.Text = string.Empty;
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
            List<Category> objCategory = objCatogeryRepository.GetAllCategory();
            dtlCategory.DataSource = objCategory;
            dtlCategory.DataBind();
        }
        catch { }
    }
    #endregion
}
