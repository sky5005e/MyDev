using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_IncentexEmployee_Settings : PageBase
{
    #region Properties

    IncentexBEDataContext db;

    Int64 IncentexEmployeeID
    {
        get
        {
            return Convert.ToInt64(ViewState["IncentexEmployeeID"]);
        }
        set
        {
            ViewState["IncentexEmployeeID"] = value;
        }
    }

    Int64 EmployeeUserInfoID
    {
        get
        {   
            return Convert.ToInt64(ViewState["EmployeeUserInfoID"]);
        }
        set
        {
            ViewState["EmployeeUserInfoID"] = value;
        }
    }

    Int64 EmployeeUserType
    {
        get
        {
            return Convert.ToInt64(ViewState["EmployeeUserType"]);
        }
        set
        {
            ViewState["EmployeeUserType"] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Incentex Employee";
            base.ParentMenuID = 10;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Incentex Employee Settings";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/IncentexEmployee/ViewIncentexEmployee.aspx";

            this.IncentexEmployeeID = Convert.ToInt64(Request.QueryString.Get("Id"));
            if (this.IncentexEmployeeID == 0)
            {
                Response.Redirect("~/admin/IncentexEmployee/BasicInformation.aspx?Id=" + this.IncentexEmployeeID);
            }
            else
            {
                UserInformation objUserInfo = new UserInformationRepository().GetUserByIncentexEmployeeID(this.IncentexEmployeeID);
                if (objUserInfo != null)
                {
                    lblUserFullName.Text = objUserInfo.FirstName + " " + objUserInfo.LastName;
                    this.EmployeeUserInfoID = Convert.ToInt64(objUserInfo.UserInfoID);
                    this.EmployeeUserType = Convert.ToInt64(objUserInfo.Usertype);
                }
            }

            manuControl.PopulateMenu(6, 0, this.IncentexEmployeeID, 0, false);

            BindData();
        }
    }

    #endregion

    #region Control Events

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
                db = new IncentexBEDataContext();
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

    protected void dtManageEmail_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            CheckBox chk = (CheckBox)e.Item.FindControl("chkManageEmail");
            HiddenField hdnUserInfoID = (HiddenField)e.Item.FindControl("hdnUserInfoID");
            HtmlGenericControl dvChk = (HtmlGenericControl)e.Item.FindControl("ManageEmailspan");

            if (!String.IsNullOrEmpty(hdnUserInfoID.Value))
            {
                chk.Checked = true;
                dvChk.Attributes.Add("class", "custom-checkbox_checked");
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
            if (!base.CanEdit && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            //Saving Preferences
            db = new IncentexBEDataContext();
            List<UserPreference> lstPrevPref = db.UserPreferences.Where(le => le.UserInfoID == this.EmployeeUserInfoID).ToList();

            if (lstPrevPref != null && lstPrevPref.Count > 0)
            {
                db.UserPreferences.DeleteAllOnSubmit(lstPrevPref);
                db.SubmitChanges();
            }

            foreach (DataListItem repPref in dlPreferences.Items)
            {
                HiddenField hdnKeyID = (HiddenField)repPref.FindControl("hdnKeyID");
                HiddenField hdnValueID = (HiddenField)repPref.FindControl("hdnValueID");
                DropDownList ddlPreference = (DropDownList)repPref.FindControl("ddlPreference");

                if (!String.IsNullOrEmpty(hdnKeyID.Value) && !String.IsNullOrEmpty(hdnValueID.Value) && !String.IsNullOrEmpty(ddlPreference.SelectedValue))
                {
                    UserPreference objNewPref = new UserPreference();
                    objNewPref.PreferenceID = Convert.ToInt64(hdnKeyID.Value);
                    objNewPref.PreferenceValueID = Convert.ToInt64(ddlPreference.SelectedValue);
                    objNewPref.UserInfoID = this.EmployeeUserInfoID;
                    db.UserPreferences.InsertOnSubmit(objNewPref);
                }
            }

            db.SubmitChanges();
            
            //Saving Manage Email Settings
            IncentexEmpManagEmailRepository objManageEmailRepo = new IncentexEmpManagEmailRepository();
            List<IncEmpManageEmail> lstPrevSetttings = objManageEmailRepo.GetEmailRightsByUserInfoID(this.EmployeeUserInfoID);

            foreach (IncEmpManageEmail objPrevSetting in lstPrevSetttings)
            {
                objManageEmailRepo.Delete(objPrevSetting);
            }

            foreach (DataListItem dlItem in dtManageEmail.Items)
            {
                CheckBox chkManageEmail = (CheckBox)dlItem.FindControl("chkManageEmail");
                HiddenField hdnManageEmailID = (HiddenField)dlItem.FindControl("hdnManageEmailID");

                if (chkManageEmail != null && chkManageEmail.Checked)
                {
                    IncEmpManageEmail objNewSetting = new IncEmpManageEmail();
                    objNewSetting.UserInfoID = this.EmployeeUserInfoID;
                    objNewSetting.ManageEmailID = Convert.ToInt64(hdnManageEmailID.Value);

                    objManageEmailRepo.Insert(objNewSetting);
                }
            }

            objManageEmailRepo.SubmitChanges();

            lblMsg.Text = "Incentex Employee settings saved Successfully...";

            BindData();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Methods

    private void BindData()
    {   
        List<FUN_GetUserPreferenceResult> lstPref = new PreferenceRepository().GetUserPreferences(this.EmployeeUserInfoID, this.EmployeeUserType).ToList();

        if (lstPref != null && lstPref.Count > 0)
        {
            dlPreferences.DataSource = lstPref;
            dlPreferences.DataBind();
        }

        List<GetEmailSettingsForIncentexEmployeesByIncentexEmployeeIDResult> lstEmailSettings = new IncentexEmployeeRepository().GetEmailSettingsByEmployeeID(this.IncentexEmployeeID);

        if (lstEmailSettings != null && lstEmailSettings.Count > 0)
        {
            dtManageEmail.DataSource = lstEmailSettings;
            dtManageEmail.DataBind();
        }
    }

    #endregion
}