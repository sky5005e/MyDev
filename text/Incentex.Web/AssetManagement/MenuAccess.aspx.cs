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
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Collections.Generic;
using Incentex.DA;
using Incentex.BE;
public partial class AssetManagement_MenuAccess : PageBase
{
    #region Properties
    AssetVendorRepository objRepo = new AssetVendorRepository();
    EquipmentVendorEmployee obj = new EquipmentVendorEmployee();
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
    #endregion
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


                ((Label)Master.FindControl("lblPageHeading")).Text = "Vendor Employee Menu Access";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/ManageEmail.aspx?SubId=" + this.VendorEmployeeID + "&Id=" + this.VendorID;
                menucontrol.PopulateMenu(2, 0, this.VendorID, this.VendorEmployeeID, false);

            }
            else
            {
                Response.Redirect("~/admin/AssetManagement/ManageEmail.aspx?SubId=" + this.VendorEmployeeID + "&Id=" + this.VendorID);
            }


            bindMenus();
            DisplayData();
        }

    }

    public void bindMenus()
    {
        MenuPrivilegeRepository objMenu = new MenuPrivilegeRepository();
        List<INC_MenuPrivilege> objList = objMenu.GetFrontMenu(DAEnums.MenuTypes.FrontEnd.ToString(), "GSEAssetManagementUsers");
        dtlMenus.DataSource = objList;
        dtlMenus.DataBind();
    }

    void DisplayData()
    {
        CheckBox chk;
        HiddenField lblId;
        if (this.VendorEmployeeID != 0)
        {
            obj = objRepo.GetVendorEmpById(this.VendorEmployeeID);

            if (obj != null)
            {
                //Get menu from empmenuaccess table
                List<EquipmentMenuAccess> lstMenuAccess = new AssetVendorRepository().GetMenusByVendorEmployeeID(this.VendorEmployeeID);

                //foreach (GridView dtM in dtlMenus.Items)
                //{
                //    chk = dtM.FindControl("chkdtlMenus") as CheckBox;
                //    lblId = dtM.FindControl("hdnMenuAccess") as HiddenField;
                //    HtmlGenericControl dvChk = dtM.FindControl("menuspan") as HtmlGenericControl;

                //    foreach (EquipmentMenuAccess objMenu in lstMenuAccess)
                //    {

                //        if (objMenu.MenuPrivilegeID.ToString().Equals(lblId.Value))
                //        {
                //            chk.Checked = true;
                //            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                //            break;
                //        }

                //    }

                //}
                foreach (GridViewRow gv in dtlMenus.Rows)
                {
                    chk = gv.FindControl("chkdtlMenus") as CheckBox;
                    lblId = gv.FindControl("hdnMenuAccess") as HiddenField;
                    HtmlGenericControl dvChk = gv.FindControl("dvChkSubReport") as HtmlGenericControl;

                    foreach (EquipmentMenuAccess objMenu in lstMenuAccess)
                    {
                        if (objMenu.MenuPrivilegeID.ToString().Equals(lblId.Value))
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

        }
    }


    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            //Delete from EmpMenuAccess
            AssetVendorRepository objAssetVendorRepo = new AssetVendorRepository();
            EquipmentMenuAccess objEquipmentMenuAccessfordel = new EquipmentMenuAccess();

            List<EquipmentMenuAccess> lst = objAssetVendorRepo.GetMenusByVendorEmployeeID(this.VendorEmployeeID);

            foreach (EquipmentMenuAccess l in lst)
            {
                objAssetVendorRepo.Delete(l);
                objAssetVendorRepo.SubmitChanges();
            }


            //Insert in empmenuaccess

            //Menu Access
            foreach (GridViewRow gv in dtlMenus.Rows)
            {
                if (((CheckBox)gv.FindControl("chkdtlMenus")).Checked == true)
                {
                    AssetVendorRepository objAssetVendorRep = new AssetVendorRepository();
                    EquipmentMenuAccess objEquipmentMenuAccess = new EquipmentMenuAccess();
                    objEquipmentMenuAccess.VendorEmployeeID = this.VendorEmployeeID;
                    objEquipmentMenuAccess.MenuPrivilegeID = Convert.ToInt64(((HiddenField)gv.FindControl("hdnMenuAccess")).Value);
                    objAssetVendorRep.Insert(objEquipmentMenuAccess);
                    objAssetVendorRep.SubmitChanges();
                }

            }





            Response.Write("Saved Successfull");
            Response.Redirect("VendorEmployeeNotes.aspx?SubId=" + this.VendorEmployeeID + "&Id=" + this.VendorID);

        }
        catch (Exception ex)
        {

        }
    }
}
