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

public partial class AssetManagement_vSubLookupDropdown : PageBase
{
    #region Data Members
    AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
    List<EquipmentLookup> objListLookup = new List<EquipmentLookup>();
    public Int64 SubCategoryID
    {
        get
        {
            if (this.ViewState["SubCategory"] == null)
                return 0;
            else
                return Convert.ToInt64(this.ViewState["SubCategory"].ToString());
        }
        set
        {
            this.ViewState["SubCategory"] = value;
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
            ((Label)Master.FindControl("lblPageHeading")).Text = "Sub Category Listing";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/DropDownmenu.aspx";
            if (Convert.ToString(Request.QueryString["strID"]) != null && Convert.ToString(Request.QueryString["strValue"]) != null)
            {
                string strText = Request.QueryString["strValue"].ToString();
                string strHeaderText = Common.Decryption(strText.Replace(" ", "+"));
                ((Label)Master.FindControl("lblPageHeading")).Text = strHeaderText;
                string strEncrept = Request.QueryString["strID"].ToString();
                Session["iLookupCode"] = strEncrept;
                BindSubCategoryDropDown(Convert.ToString(Session["iLookupCode"]));
                BindDatlist(Convert.ToString(Session["iLookupCode"]));
                //BindDatlist(strEncrept);

                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/DropdownMenu.aspx";
            }
            else
            {
                Response.Redirect("dropdownmenus.aspx");
            }

        }



    }

    protected void rptCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        String iLookupcode = Convert.ToString(Session["iLookupCode"]);
        Int64 iLookupID = Convert.ToInt64(((HiddenField)e.Item.FindControl("hdnCategoryID")).Value);
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            List<EquipmentLookup> objEquipmentLookup = objAssetMgtRepository.GetSubItemFrmEquipmentLookup(iLookupcode, iLookupID);
            if (objEquipmentLookup.Count > 0)
            {
                ((DataList)e.Item.FindControl("dtlSubCategory")).DataSource = objEquipmentLookup;
                ((DataList)e.Item.FindControl("dtlSubCategory")).DataBind();
            }
            else
            {
                ((Label)e.Item.FindControl("lblMsg")).Text = "No Record Found.";
            }
        }
    }

    protected void dtlSubCategory_ItemCommand(object source, DataListCommandEventArgs e)
    {

        if (e.CommandName == "deletevalue")
        {
            EquipmentLookup objSubCategory = objAssetMgtRepository.GetLookupByID(Convert.ToInt64(e.CommandArgument.ToString()));
            if (objSubCategory != null)
            {
                try
                {
                    objAssetMgtRepository.Delete(objSubCategory);
                    objAssetMgtRepository.SubmitChanges();
                    lblErrorMessage.Text = "Record deleted successfully";
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
                SubCategoryID = Convert.ToInt64(e.CommandArgument.ToString());
                EquipmentLookup objSubCategory = objAssetMgtRepository.GetLookupByID(SubCategoryID);
                if (objSubCategory != null)
                {
                    lblMessage.Text = string.Empty;
                    btnSubmit.Text = "Edit";
                    ddlCategory.SelectedValue = objSubCategory.ParentiLookupID.ToString();
                    txtSubCategory.Text = objSubCategory.sLookupName;
                    modal.Show();
                }
            }
            catch
            {
            }
        }
        BindDatlist(Convert.ToString(Session["iLookupCode"]));
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Add")
        {
            //Check dulication here when add new record.
            List<EquipmentLookup> objSubCategoryList = objAssetMgtRepository.CheckDuplicatLookup(txtSubCategory.Text.Trim(),Convert.ToString(Session["iLookupCode"]));
            if (objSubCategoryList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                EquipmentLookup objSubCategory = new EquipmentLookup();
                objSubCategory.ParentiLookupID = Convert.ToInt64(ddlCategory.SelectedValue);
                objSubCategory.sLookupName = txtSubCategory.Text.Trim();
                objSubCategory.iLookupCode=(Convert.ToString(Session["iLookupCode"]));
                objAssetMgtRepository.Insert(objSubCategory);
                objAssetMgtRepository.SubmitChanges();
                lblErrorMessage.Text = "Record saved successfully!";
                lblMessage.Text = string.Empty;
                modal.Hide();
            }
        }
        else
        {

            //Check dulication here in edit mode
            List<EquipmentLookup> objSubCategoryList = objAssetMgtRepository.CheckDuplicatLookup(txtSubCategory.Text.Trim(), Convert.ToString(Session["iLookupCode"]));
            if (objSubCategoryList.Count > 0)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                EquipmentLookup objSubCategory = objAssetMgtRepository.GetLookupByID(SubCategoryID);
                if (objSubCategory != null)
                {
                    objSubCategory.ParentiLookupID = Convert.ToInt64(ddlCategory.SelectedValue);
                    objSubCategory.sLookupName = txtSubCategory.Text.Trim();
                    objSubCategory.iLookupCode = (Convert.ToString(Session["iLookupCode"]));
                    objAssetMgtRepository.SubmitChanges();
                    lblErrorMessage.Text = "Record updated successfully!";
                    lblMessage.Text = string.Empty;
                    modal.Hide();
                }
            }
        }
        BindDatlist(Convert.ToString(Session["iLookupCode"]));
    }

    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        btnSubmit.Text = "Add";
        txtSubCategory.Text = string.Empty;
        ddlCategory.ClearSelection();
        lblMessage.Text = string.Empty;
        modal.Controls.Clear();
        modal.Show();
    }
    #endregion

    #region Methods
    public void BindSubCategoryDropDown(String iLookupCode)
    {       
        objListLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup(iLookupCode); ;
        ddlCategory.DataSource = objListLookup;
        ddlCategory.DataTextField = "sLookupName";
        ddlCategory.DataValueField = "iLookupID";
        ddlCategory.DataBind();
    }
    public void BindDatlist(String iLookupCode)
    {
        try
        {
            objListLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup(iLookupCode); ;
            rptCategory.DataSource = objListLookup;
            rptCategory.DataBind();
        }
        catch { }
    }
    #endregion
}
