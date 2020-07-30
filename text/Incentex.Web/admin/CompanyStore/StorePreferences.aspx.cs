using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyStore_StorePreferences : PageBase
{
    #region Properties

    Int64 CompanyStoreID
    {
        get
        {
            if (ViewState["CompanyStoreID"] == null)
            {
                ViewState["CompanyStoreID"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreID"]);
        }
        set
        {
            ViewState["CompanyStoreID"] = value;
        }
    }

    IncentexBEDataContext db;

    #endregion

    #region Page Events

    protected void Page_Load(Object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ((Label)Master.FindControl("lblPageHeading")).Text = "Store Preferences";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/CompanyStore/ViewCompanyStore.aspx";

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

                if (Request.QueryString.Count > 0)
                {
                    this.CompanyStoreID = Convert.ToInt64(Request.QueryString.Get("id"));

                    if (this.CompanyStoreID == 0)
                    {
                        Response.Redirect("~/admin/CompanyStore/GeneralStoreSetup.aspx?Id=" + this.CompanyStoreID);
                    }

                    if (this.CompanyStoreID > 0 && !base.CanEdit)
                    {
                        base.RedirectToUnauthorised();
                    }

                    lblStore.Text = new CompanyRepository().GetById(new CompanyStoreRepository().GetById(this.CompanyStoreID).CompanyID).CompanyName;

                    Session["ManageID"] = 5;
                    menuControl.PopulateMenu(10, 0, this.CompanyStoreID, 0, false);

                    BindWorkGroupDepartment();
                    SetPreferences();
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Control Events

    protected void dtlWorkGroup_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox chk = (CheckBox)e.Item.FindControl("chkWorkGroup");

                if (chk.Checked)
                {
                    HtmlGenericControl dvChk = (HtmlGenericControl)e.Item.FindControl("menuspan");
                    dvChk.Attributes.Add("class", "custom-checkbox_checked");
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dtlDepartment_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox chk = (CheckBox)e.Item.FindControl("chkDepartment");

                if (chk.Checked)
                {
                    HtmlGenericControl dvChk = (HtmlGenericControl)e.Item.FindControl("menuspan");
                    dvChk.Attributes.Add("class", "custom-checkbox_checked");
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkSave_Click(Object sender, EventArgs e)
    {
        try
        {
            CompanyStoreRepository objComStoRepo = new CompanyStoreRepository();

            foreach (DataListItem Item in dtlWorkGroup.Items)
            {
                CheckBox chkWorkGroup = (CheckBox)Item.FindControl("chkWorkGroup");
                HiddenField hdnWorkGroupID = (HiddenField)Item.FindControl("hdnWorkGroupID");

                StoreWorkGroup objStoreWorkGroup = objComStoRepo.GetStoreWorkGroupByStoreNWorkGroupID(this.CompanyStoreID, Convert.ToInt64(hdnWorkGroupID.Value));
                if (objStoreWorkGroup != null)
                {
                    objComStoRepo.Delete(objStoreWorkGroup);
                }

                if (chkWorkGroup.Checked)
                {
                    objStoreWorkGroup = new StoreWorkGroup();
                    objStoreWorkGroup.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                    objStoreWorkGroup.CreatedDate = DateTime.Now;
                    objStoreWorkGroup.StoreID = this.CompanyStoreID;
                    objStoreWorkGroup.WorkGroupID = Convert.ToInt64(hdnWorkGroupID.Value);
                    objComStoRepo.Insert(objStoreWorkGroup);
                }
            }

            objComStoRepo.SubmitChanges();
            objComStoRepo = new CompanyStoreRepository();

            foreach (DataListItem Item in dtlDepartment.Items)
            {
                CheckBox chkDepartment = (CheckBox)Item.FindControl("chkDepartment");
                HiddenField hdnDepartmentID = (HiddenField)Item.FindControl("hdnDepartmentID");

                StoreDepartment objStoreDepartment = objComStoRepo.GetStoreDepartmentByStoreNDepartmentID(this.CompanyStoreID, Convert.ToInt64(hdnDepartmentID.Value));
                if (objStoreDepartment != null)
                {
                    objComStoRepo.Delete(objStoreDepartment);
                }

                if (chkDepartment.Checked)
                {
                    objStoreDepartment = new StoreDepartment();
                    objStoreDepartment.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                    objStoreDepartment.CreatedDate = DateTime.Now;
                    objStoreDepartment.DepartmentID = Convert.ToInt64(hdnDepartmentID.Value);
                    objStoreDepartment.StoreID = this.CompanyStoreID;
                    objComStoRepo.Insert(objStoreDepartment);
                }
            }

            objComStoRepo.SubmitChanges();

            db = new IncentexBEDataContext();
            List<StorePreference> lstPrevPref = db.StorePreferences.Where(le => le.StoreID == this.CompanyStoreID).ToList();

            if (lstPrevPref != null && lstPrevPref.Count > 0)
            {
                db.StorePreferences.DeleteAllOnSubmit(lstPrevPref);
                db.SubmitChanges();
            }

            foreach (DataListItem repPref in dlPreferences.Items)
            {
                HiddenField hdnKeyID = (HiddenField)repPref.FindControl("hdnKeyID");
                HiddenField hdnValueID = (HiddenField)repPref.FindControl("hdnValueID");
                DropDownList ddlPreference = (DropDownList)repPref.FindControl("ddlPreference");

                if (!String.IsNullOrEmpty(hdnKeyID.Value) && !String.IsNullOrEmpty(hdnValueID.Value) && !String.IsNullOrEmpty(ddlPreference.SelectedValue))
                {
                    StorePreference objNewPref = new StorePreference();
                    objNewPref.PreferenceID = Convert.ToInt64(hdnKeyID.Value);
                    objNewPref.PreferenceValueID = Convert.ToInt64(ddlPreference.SelectedValue);
                    objNewPref.StoreID = this.CompanyStoreID;
                    db.StorePreferences.InsertOnSubmit(objNewPref);
                }
            }

            db.SubmitChanges();

            BindWorkGroupDepartment();

            lblMsg.Text = "Store preferences saved successfully.";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dlPreferences_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HiddenField hdnKeyID = (HiddenField)e.Item.FindControl("hdnKeyID");
            HiddenField hdnValueID = (HiddenField)e.Item.FindControl("hdnValueID");
            DropDownList ddlPreference = (DropDownList)e.Item.FindControl("ddlPreference");

            if (!String.IsNullOrEmpty(hdnKeyID.Value))
            {
                Int64 PreferenceID = Convert.ToInt64(hdnKeyID.Value);
                IncentexBEDataContext db = new IncentexBEDataContext();
                List<PreferenceValue> lstPreference = db.PreferenceValues.Where(le => le.PreferenceID == PreferenceID).ToList();
                ddlPreference.DataSource = lstPreference;
                ddlPreference.DataValueField = "PreferenceValueID";
                ddlPreference.DataTextField = "Display";
                ddlPreference.DataBind();

                if (!String.IsNullOrEmpty(hdnValueID.Value))
                    ddlPreference.SelectedValue = hdnValueID.Value;
            }
        }
    }

    #endregion

    #region Page Methods

    private void BindWorkGroupDepartment()
    {
        try
        {
            CompanyStoreRepository objComStoRepo = new CompanyStoreRepository();

            List<GetStoreWorkGroupsResult> lstWorkGroups = new List<GetStoreWorkGroupsResult>();
            lstWorkGroups = objComStoRepo.GetStoreWorkGroups(this.CompanyStoreID).OrderBy(le => le.WorkGroup).ToList();

            dtlWorkGroup.DataSource = lstWorkGroups;
            dtlWorkGroup.DataBind();

            List<GetStoreDepartmentsResult> lstDepartments = new List<GetStoreDepartmentsResult>();
            lstDepartments = objComStoRepo.GetStoreDepartments(this.CompanyStoreID).OrderBy(le => le.Department).ToList();

            dtlDepartment.DataSource = lstDepartments;
            dtlDepartment.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SetPreferences()
    {
        try
        {
            List<FUN_GetStorePreferenceResult> lstPref = new PreferenceRepository().GetStorePreferences(this.CompanyStoreID).ToList();

            if (lstPref != null && lstPref.Count > 0)
            {
                dlPreferences.DataSource = lstPref;
                dlPreferences.DataBind();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}