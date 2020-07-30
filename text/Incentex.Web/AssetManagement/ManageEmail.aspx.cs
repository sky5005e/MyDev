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
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;

public partial class AssetManagement_ManageEmail : PageBase
{
    Int64 VendorID
    {
        get
        {
            if (ViewState["VendorID"] == null)
            {
                ViewState["VendorID"] = 0;
            }
            return Convert.ToInt64(ViewState["VendorID"]);
        }
        set
        {
            ViewState["VendorID"] = value;
        }
    }
    Int64 VendorEmployeeID
    {
        get
        {
            if (ViewState["VendorEmployeeID"] == null)
            {
                ViewState["VendorEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["VendorEmployeeID"]);
        }
        set
        {
            ViewState["VendorEmployeeID"] = value;
        }
    }
    Int64 UserinfoID
    {
        get
        {
            if (ViewState["UserinfoID"] == null)
            {
                ViewState["UserinfoID"] = 0;
            }
            return Convert.ToInt64(ViewState["UserinfoID"]);
        }
        set
        {
            ViewState["UserinfoID"] = value;
        }
    }
    AssetVendorRepository objAssetVendorRepo = new AssetVendorRepository();
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
        try
        {
            if (!IsPostBack)
            {
                base.MenuItem = "Manage Employee";
                base.ParentMenuID = 50;

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

                    this.VendorID = Convert.ToInt64(Request.QueryString.Get("Id"));
                    this.VendorEmployeeID = Convert.ToInt64(Request.QueryString.Get("SubId"));

                    if (this.VendorEmployeeID != 0)
                    {
                        EquipmentVendorEmployee objEquipmentVendorEmployee = new AssetVendorRepository().GetVendorEmpById(this.VendorEmployeeID);
                        this.UserinfoID = Convert.ToInt64(objEquipmentVendorEmployee.UserInfoID);
                    }

                    ((Label)Master.FindControl("lblPageHeading")).Text = "Email Management";
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/BasicVendorEmpInformation.aspx?SubId=" + this.VendorEmployeeID + "&Id=" + this.VendorID;

                    menucontrol.PopulateMenu(1, 0, this.VendorEmployeeID, this.VendorID, false);
                    BindData();
                    DisplayData();
                }
                else
                {
                    Response.Redirect("ViewIncentexEmployee.aspx");
                }


            }
        }
        catch (Exception)
        {


        }
    }

    private void BindData()
    {
        try
        {
            AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
            //Email Management System            
            dtManageEmail.DataSource = objAssetVendorRepository.GetEmailControl();
            dtManageEmail.DataBind();
            //Dropdown Management System 
            dtManageDropdown.DataSource = objAssetVendorRepository.GetDropDownControl();
            dtManageDropdown.DataBind();
        }
        catch (Exception)
        {


        }
    }

    private void DisplayData()
    {
        try
        {
            CheckBox chk;
            HiddenField lblId;
            //Manage Email
            List<EquipmentManageEmail> lstManageEmail = new AssetVendorRepository().GetEmailRightsByUserInfoID(this.UserinfoID);

            foreach (GridViewRow gv in dtManageEmail.Rows)
            {
                chk = gv.FindControl("chkManageEmail") as CheckBox;
                lblId = gv.FindControl("hdnManageEmail") as HiddenField;
                HtmlGenericControl dvChk = gv.FindControl("ManageEmailspan") as HtmlGenericControl;

                foreach (EquipmentManageEmail objMenu in lstManageEmail)
                {
                    if (objMenu.EquipmentEmailID.ToString().Equals(lblId.Value))
                    {
                        chk.Checked = true;
                        dvChk.Attributes.Add("class", "wheather_checked");
                        break;
                    }
                    else
                    {
                        chk.Checked = false;
                        dvChk.Attributes.Add("class", "wheather_check");
                    }
                }
            }
            //Manage Dropdown

            List<EquipmentManageDropDown> lstManageDropdown = new AssetVendorRepository().GetDropDownRightsByUserInfoID(this.UserinfoID);

            //foreach (DataListItem dtM in dtManageDropdown.Items)
            //{
            //    chk = dtM.FindControl("chkManageDropdown") as CheckBox;
            //    lblId = dtM.FindControl("hdnManageDropdown") as HiddenField;
            //    HtmlGenericControl dvChk = dtM.FindControl("ManageDropdownspan") as HtmlGenericControl;

            //    foreach (EquipmentManageDropDown objMenu in lstManageDropdown)
            //    {

            //        if (objMenu.EquipmentDropDownID.ToString().Equals(lblId.Value))
            //        {
            //            chk.Checked = true;
            //            dvChk.Attributes.Add("class", "custom-checkbox_checked");
            //            break;
            //        }

            //    }

            //}
            foreach (GridViewRow gv in dtManageDropdown.Rows)
            {
                chk = gv.FindControl("chkManageDropdown") as CheckBox;
                lblId = gv.FindControl("hdnManageDropdown") as HiddenField;
                HtmlGenericControl dvChk = gv.FindControl("ManageDropdownspan") as HtmlGenericControl;

                foreach (EquipmentManageDropDown objMenu in lstManageDropdown)
                {
                    if (objMenu.EquipmentDropDownID.ToString().Equals(lblId.Value))
                    {
                        chk.Checked = true;
                        dvChk.Attributes.Add("class", "wheather_checked");
                        break;
                    }
                    else
                    {
                        chk.Checked = false;
                        dvChk.Attributes.Add("class", "wheather_check");
                    }
                }
            }
        }
        catch (Exception)
        {


        }
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {

        Response.Redirect("MenuAccess.aspx?SubId=" + this.VendorEmployeeID + "&Id=" + this.VendorID);

    }

    protected void lnkSaveEmail_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            //Manage Email 
            //Delete from EquipmentManageEmail
            AssetVendorRepository objAssetVendorRepo = new AssetVendorRepository();
            EquipmentManageEmail objEquipmentManageEmaildel = new EquipmentManageEmail();
            List<EquipmentManageEmail> lst = objAssetVendorRepo.GetEmailRightsByUserInfoID(this.UserinfoID);
            foreach (EquipmentManageEmail l in lst)
            {
                objAssetVendorRepo.Delete(l);
                objAssetVendorRepo.SubmitChanges();
            }
            //Insert in EquipmentManageEmail
            foreach (GridViewRow gv in dtManageEmail.Rows)
            {
                if (((CheckBox)gv.FindControl("chkManageEmail")).Checked == true)
                {
                    EquipmentManageEmail objEquipmentManageEmailins = new EquipmentManageEmail();
                    objEquipmentManageEmailins.UserInfoID = this.UserinfoID;
                    objEquipmentManageEmailins.EquipmentEmailID = Convert.ToInt64(((HiddenField)gv.FindControl("hdnManageEmail")).Value);

                    objAssetVendorRepo.Insert(objEquipmentManageEmailins);
                    objAssetVendorRepo.SubmitChanges();
                }
            }
            DisplayData();
        }
        catch (Exception)
        {


        }
    }

    protected void lnkSaveDropDown_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            //Manage DropDown
            //Delete from EquipmentManageDropdown

            EquipmentManageDropDown objEquipmentManageDropDowndel = new EquipmentManageDropDown();
            List<EquipmentManageDropDown> lstddl = objAssetVendorRepo.GetDropDownRightsByUserInfoID(this.UserinfoID);
            foreach (EquipmentManageDropDown l in lstddl)
            {
                objAssetVendorRepo.Delete(l);
                objAssetVendorRepo.SubmitChanges();
            }
            //Insert in EquipmentManageDropDown
            foreach (GridViewRow gv in dtManageDropdown.Rows)
            {
                if (((CheckBox)gv.FindControl("chkManageDropDown")).Checked == true)
                {

                    EquipmentManageDropDown objEquipmentManageDropDownins = new EquipmentManageDropDown();
                    objEquipmentManageDropDownins.UserInfoID = this.UserinfoID;
                    objEquipmentManageDropDownins.EquipmentDropDownID = Convert.ToInt64(((HiddenField)gv.FindControl("hdnManageDropDown")).Value);

                    objAssetVendorRepo.Insert(objEquipmentManageDropDownins);
                    objAssetVendorRepo.SubmitChanges();
                }
            }
            DisplayData();
        }
        catch (Exception)
        {


        }
    }
}
