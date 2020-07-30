using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyStore_Product_AdditionalWorkgroupFroProduct : PageBase
{
    #region Data Members

    Int64 iStoreID
    {
        get
        {
            if (ViewState["iStoreID"] == null)
            {
                ViewState["iStoreID"] = 0;
            }
            return Convert.ToInt64(ViewState["iStoreID"]);
        }
        set
        {
            ViewState["iStoreID"] = value;
        }
    }

    Int64 ProductID
    {
        get
        {
            if (ViewState["ProductID"] == null)
            {
                ViewState["ProductID"] = 0;
            }
            return Convert.ToInt64(ViewState["ProductID"]);
        }
        set
        {
            ViewState["ProductID"] = value;
        }
    }

    /// <summary>
    /// To Display WorkgroupName
    /// </summary>
    String WorkGroupNameToDisplay
    {
        get
        {
            if (Session["WorkGroupNameToDisplay"] != null && Session["WorkGroupNameToDisplay"].ToString().Length > 0)
                ViewState["WorkGroupNameToDisplay"] = " - " + Session["WorkGroupNameToDisplay"].ToString();
            else
                ViewState["WorkGroupNameToDisplay"] = "";

            return ViewState["WorkGroupNameToDisplay"].ToString();
        }
    }

    LookupRepository objLookupRepos = new LookupRepository();
    StoreProductRepository objStoreProductRepos = new StoreProductRepository();
    SubCatogeryRepository objSubCatogeryRepository = new SubCatogeryRepository();

    #endregion

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Store Management Console";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            if (Request.QueryString["Id"] != null)
                iStoreID = Convert.ToInt64(Request.QueryString["Id"].ToString());

            if (Request.QueryString["SubId"] != null && Request.QueryString["SubId"] != "0" && Request.QueryString["SubId"] != "")
            {
                this.ProductID = Convert.ToInt64(Request.QueryString.Get("SubId"));
                menuControl.PopulateMenu(7, 0, Convert.ToInt64(this.ProductID), iStoreID, false);
                BindData();
                DisplayData();
            }
            else
                Response.Redirect("General.aspx?SubId=" + Request.QueryString["SubId"] + "&Id=" + Request.QueryString["Id"]);

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Additional" + WorkGroupNameToDisplay;
            ((HtmlGenericControl)menuControl.FindControl("dvSubMenu")).Visible = false;
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.ProductReturnURL;

        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        StringBuilder sbWorkgroup = new StringBuilder();
        for (Int32 i = 0; i < dtlWorkgroup.Items.Count; i++)
        {
            CheckBox chkdtlMenus = (CheckBox)dtlWorkgroup.Items[i].FindControl("chkdtlMenus");
            HiddenField hdnWorkgroupID = (HiddenField)dtlWorkgroup.Items[i].FindControl("hdnWorkgroupID");
            if (chkdtlMenus.Checked)
            {
                if (sbWorkgroup.ToString() == "")
                {
                    sbWorkgroup.Append(hdnWorkgroupID.Value);
                }
                else
                {
                    sbWorkgroup.Append("," + hdnWorkgroupID.Value);
                }
            }
        }

        StringBuilder sbSubCategory = new StringBuilder();
        for (Int32 i = 0; i < dtlSubCategory.Items.Count; i++)
        {
            CheckBox chkSubCategory = (CheckBox)dtlSubCategory.Items[i].FindControl("chkSubCategory");
            HiddenField hdnSubCategoryID = (HiddenField)dtlSubCategory.Items[i].FindControl("hdnSubCategoryID");
            if (chkSubCategory.Checked)
            {
                if (sbSubCategory.ToString() == "")
                    sbSubCategory.Append(hdnSubCategoryID.Value);
                else
                    sbSubCategory.Append("," + hdnSubCategoryID.Value);
            }
        }

        StoreProduct objStoreProduct = objStoreProductRepos.GetById(this.ProductID);
        objStoreProduct.AdditionalWorkgroupId = sbWorkgroup.ToString();
        objStoreProduct.AdditionalSubCategoryID = sbSubCategory.ToString();
        objStoreProductRepos.SubmitChanges();

        BindData();
        DisplayData();

    }

    #endregion

    #region Methods

    public void BindData()
    {
        try
        {
            if (Session["WorkgroupName"] != null)
            {
                Int64 iLookupWorkgroupId = Convert.ToInt64(objLookupRepos.GetIdByLookupNameNLookUpCode(Session["WorkgroupName"].ToString().Trim(), "Workgroup"));
                String strStatus = "Workgroup";
                dtlWorkgroup.DataSource = objLookupRepos.GetByLookupWorkgroupName(strStatus, Convert.ToInt32(iLookupWorkgroupId));
                dtlWorkgroup.DataBind();
            }

            //Bind Subcategory list with not exist main selected category
            StoreProduct objStoreProduct = objStoreProductRepos.GetById(this.ProductID);
            List<SubCategory> objSubCategoryList = objSubCatogeryRepository.GetAllSubCategory(Convert.ToInt32(objStoreProduct.CategoryID)).ToList();
            objSubCategoryList = objSubCategoryList.Where(s => s.SubCategoryID != objStoreProduct.SubCategoryID).ToList();
            dtlSubCategory.DataSource = objSubCategoryList;
            dtlSubCategory.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public void DisplayData()
    {
        CheckBox chk;
        HiddenField lblId;
        StoreProduct obj = new StoreProduct();
        obj = objStoreProductRepos.GetById(this.ProductID);

        //This is for workgroup binding
        String[] MyAdditionalWorkgroup = null;
        if (obj != null && obj.AdditionalWorkgroupId != null)
        {
            MyAdditionalWorkgroup = obj.AdditionalWorkgroupId.Split(',');
            foreach (DataListItem dtM in dtlWorkgroup.Items)
            {
                chk = dtM.FindControl("chkdtlMenus") as CheckBox;
                lblId = dtM.FindControl("hdnWorkgroupID") as HiddenField;
                HtmlGenericControl dvChk = dtM.FindControl("menuspan") as HtmlGenericControl;

                for (Int32 k = 0; k < MyAdditionalWorkgroup.Count(); ++k)
                {
                    if (MyAdditionalWorkgroup[k].ToString().Equals(lblId.Value))
                    {
                        chk.Checked = true;
                        dvChk.Attributes.Add("class", "custom-checkbox_checked");
                        break;
                    }
                }
            }
        }

        //This is for sub category binding
        String[] MyAdditionalSubCategory = null;
        if (obj != null && obj.AdditionalSubCategoryID != null)
        {
            MyAdditionalSubCategory = obj.AdditionalSubCategoryID.Split(',');
            foreach (DataListItem dtM in dtlSubCategory.Items)
            {
                chk = dtM.FindControl("chkSubCategory") as CheckBox;
                lblId = dtM.FindControl("hdnSubCategoryID") as HiddenField;
                HtmlGenericControl dvChk = dtM.FindControl("menuspan") as HtmlGenericControl;

                for (Int32 k = 0; k < MyAdditionalSubCategory.Count(); ++k)
                {
                    if (MyAdditionalSubCategory[k].ToString().Equals(lblId.Value))
                    {
                        chk.Checked = true;
                        dvChk.Attributes.Add("class", "custom-checkbox_checked");
                        break;
                    }
                }
            }
        }
    }

    #endregion
}