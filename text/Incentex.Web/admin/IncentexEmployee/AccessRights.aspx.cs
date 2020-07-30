using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Web.UI.HtmlControls;

public partial class admin_IncentexEmployee_AccessRights : PageBase
{
    #region Page Properties

    Int64 UserInfoID
    {
        get
        {
            return Convert.ToInt64(ViewState["UserInfoID"]);
        }
        set
        {
            ViewState["UserInfoID"] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
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

            ((Label)Master.FindControl("lblPageHeading")).Text = "Access Rights";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/IncentexEmployee/ViewIncentexEmployee.aspx";

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                IncentexEmployee objEmployee = new IncentexEmployeeRepository().GetById(Convert.ToInt64(Request.QueryString["id"]));

                if (objEmployee != null)
                {
                    this.UserInfoID = objEmployee.UserInfoID;
                    UserInformation objUserInfo = new UserInformationRepository().GetById(this.UserInfoID);
                    lblUserFullName.Text = objUserInfo.FirstName + " " + objUserInfo.LastName;
                    BindAccessRights();

                    menucontrol.PopulateMenu(3, 0, Convert.ToInt64(Request.QueryString["id"]), 0, false);
                }
                else
                {
                    Response.Redirect("~/admin/IncentexEmployee/BasicInformation.aspx?Id=" + Request.QueryString["id"]);
                }
            }
            else
            {
                Response.Redirect("ViewIncentexEmployee.aspx");
            }
        }
    }

    #endregion

    #region Control Events

    protected void dtlRights_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnHasChild = (HiddenField)e.Item.FindControl("hdnHasChild");
                HiddenField hdnAccessMenuID = (HiddenField)e.Item.FindControl("hdnAccessMenuID");
                HiddenField hdnParentMenuID = (HiddenField)e.Item.FindControl("hdnParentMenuID");
                HiddenField hdnDisplayLevel = (HiddenField)e.Item.FindControl("hdnDisplayLevel");
                HiddenField hdnOrderString = (HiddenField)e.Item.FindControl("hdnOrderString");
                HtmlControl tblMenus = (HtmlControl)e.Item.FindControl("tblMenus");

                if (!String.IsNullOrEmpty(hdnHasChild.Value) && Convert.ToBoolean(hdnHasChild.Value) && !String.IsNullOrEmpty(hdnParentMenuID.Value) && Convert.ToInt64(hdnParentMenuID.Value) > 0)
                    tblMenus.Attributes.Add("class", "parent" + hdnAccessMenuID.Value + " child" + hdnParentMenuID.Value);
                else
                {
                    if (!String.IsNullOrEmpty(hdnHasChild.Value) && Convert.ToBoolean(hdnHasChild.Value))
                        tblMenus.Attributes.Add("class", "parent" + hdnAccessMenuID.Value);

                    if (!String.IsNullOrEmpty(hdnParentMenuID.Value) && Convert.ToInt64(hdnParentMenuID.Value) > 0)
                        tblMenus.Attributes.Add("class", "child" + hdnParentMenuID.Value);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkBtnSaveAccessRights_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            AccessRightRepository objRightRepo = new AccessRightRepository();

            foreach (DataListItem item in dtlRights.Items)
            {
                HiddenField hdnAccessMenuID = (HiddenField)item.FindControl("hdnAccessMenuID");
                HiddenField hdnAccessRightID = (HiddenField)item.FindControl("hdnAccessRightID");

                CheckBox chkView = (CheckBox)item.FindControl("chkView");
                CheckBox chkAdd = (CheckBox)item.FindControl("chkAdd");
                CheckBox chkEdit = (CheckBox)item.FindControl("chkEdit");
                CheckBox chkDelete = (CheckBox)item.FindControl("chkDelete");

                if (!String.IsNullOrEmpty(hdnAccessRightID.Value))
                {
                    AccessRight objRight = objRightRepo.GetAccessRight(Convert.ToInt64(hdnAccessRightID.Value));

                    objRight.CanAdd = chkAdd.Checked;
                    objRight.CanDelete = chkDelete.Checked;
                    objRight.CanEdit = chkEdit.Checked;
                    objRight.CanView = chkView.Checked;

                    objRight.UpdateDate = DateTime.Now;
                    objRight.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                }
                else
                {
                    AccessRight objRight = new AccessRight();

                    objRight.AccessMenuID = Convert.ToInt64(hdnAccessMenuID.Value);

                    objRight.CanAdd = chkAdd.Checked;
                    objRight.CanDelete = chkDelete.Checked;
                    objRight.CanEdit = chkEdit.Checked;
                    objRight.CanView = chkView.Checked;

                    objRight.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                    objRight.CreatedDate = DateTime.Now;

                    objRight.UserInfoID = this.UserInfoID;

                    objRightRepo.Insert(objRight);
                }
            }

            objRightRepo.SubmitChanges();

            BindAccessRights();

            lblMsg.Text = "Access rights saved successfully...";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Methods

    private void BindAccessRights()
    {
        AccessRightRepository objRightRepo = new AccessRightRepository();
        List<GetAccessRightsByUserInfoIDResult> lstRights = objRightRepo.GetAccessRights(this.UserInfoID).ToList();
        dtlRights.DataSource = lstRights;
        dtlRights.DataBind();
    }

    #endregion
}