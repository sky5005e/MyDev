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

public partial class admin_Supplier_SupplierMenuDataAccess : PageBase
{
    #region Properties
    SupplierEmployeeRepository objRepo = new SupplierEmployeeRepository();
    SupplierEmployee obj = new SupplierEmployee();
    Int64 SupplierID
    {
        get
        {
            if (ViewState["SupplierID"] == null)
            {
                ViewState["SupplierID"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierID"]);
        }
        set
        {
            ViewState["SupplierID"] = value;
        }
    }
    Int64 SupplierEmployeeID
    {
        get
        {
            if (ViewState["SupplierEmployeeID"] == null)
            {
                ViewState["SupplierEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierEmployeeID"]);
        }
        set
        {
            ViewState["SupplierEmployeeID"] = value;
        }
    }
    Int64 UserInfoID
    {
        get
        {
            if (ViewState["UserInfoID"] == null)
            {
                ViewState["UserInfoID"] = 0;
            }
            return Convert.ToInt64(ViewState["UserInfoID"]);
        }
        set
        {
            ViewState["UserInfoID"] = value;
        }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Employee";
            base.ParentMenuID = 18;

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
                if (Request.QueryString["SubId"] != null)
                {
                    this.SupplierEmployeeID = Convert.ToInt64(Request.QueryString.Get("SubId"));
                }
                if (Request.QueryString["id"] != "0")
                {
                    this.SupplierID = Convert.ToInt64(Request.QueryString.Get("id"));
                    // txtCompany.Text = Convert.ToString(objSupplierRepos.GetById(this.SupplierID).CompanyName);
                }
                if (this.SupplierEmployeeID == 0)
                {
                    Response.Redirect("~/admin/Supplier/BasicSupplierInformation.aspx?Id=" + this.SupplierID + "&SubId=" + this.SupplierEmployeeID);
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Supplier Employee Menu/Data Access";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Supplier/ViewAddSupplierEmployee.aspx?Id=" + this.SupplierID;
                menucontrol.PopulateMenu(3, 3, this.SupplierID, this.SupplierEmployeeID, true);
                
            }
            else
            {
                Response.Redirect("~/admin/Supplier/ViewAddSupplierEmployee.aspx");
            }


            bindMenus();
            DisplayData();
        }
    }
    void DisplayData()
    {
        CheckBox chk;
        HiddenField lblId;
        if (this.SupplierEmployeeID != 0)
        {
            objRepo = new SupplierEmployeeRepository();
            obj = objRepo.GetById(this.SupplierEmployeeID);

            if (obj != null)
            {
                //Get menu from empmenuaccess table
                List<SupplierEmpMenuAccess> lstMenuAccess = new SupplierMenuAccessRepository().GetMenusBySupplierEmployeeID(this.SupplierEmployeeID);

                foreach (DataListItem dtM in dtlMenus.Items)
                {
                    chk = dtM.FindControl("chkdtlMenus") as CheckBox;
                    lblId = dtM.FindControl("hdnMenuAccess") as HiddenField;
                    HtmlGenericControl dvChk = dtM.FindControl("menuspan") as HtmlGenericControl;

                    foreach (SupplierEmpMenuAccess objMenu in lstMenuAccess)
                    {

                        if (objMenu.MenuPrivilegeID.ToString().Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }

                    }

                }


                //Get Data from dataaccess


                List<SupplierEmpDataAccess> lstdataAccess = new SupplierDataAccessRepository().GetDataBySupplierEmpId(this.SupplierEmployeeID);

                foreach (DataListItem dtD in dtlDataAccess.Items)
                {
                    chk = dtD.FindControl("chkdtlDataAccess") as CheckBox;
                    lblId = dtD.FindControl("hdnDataBaseAccess") as HiddenField;
                    HtmlGenericControl dvChkData = dtD.FindControl("menuspanData") as HtmlGenericControl;

                    foreach (SupplierEmpDataAccess objMenu in lstdataAccess)
                    {

                        if (objMenu.DataPrivilegeID.ToString().Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChkData.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }

                    }

                }
            }
        }
    }
    public void bindMenus()
    {
        MenuPrivilegeRepository objMenu = new MenuPrivilegeRepository();
        List<INC_MenuPrivilege> objList = objMenu.GetFrontMenu(DAEnums.MenuTypes.BackEnd.ToString(), "Supplier Employee").OrderBy(le => le.MenuOrder).ToList();
        dtlMenus.DataSource = objList;
        dtlMenus.DataBind();
        DataAccessRepository objData = new DataAccessRepository();
        List<INC_DataPrivilege> objDataList = objData.GetDataAccess();
        dtlDataAccess.DataSource = objDataList;
        dtlDataAccess.DataBind();
    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            //Delete from SupplierEmpMenuAccess
            SupplierMenuAccessRepository objSupplierMenuAccessRepo = new SupplierMenuAccessRepository();
            SupplierEmpMenuAccess objSupplierEmpMenuAccessfordel = new SupplierEmpMenuAccess();

            List<SupplierEmpMenuAccess> lst = objSupplierMenuAccessRepo.GetMenusBySupplierEmployeeID(this.SupplierEmployeeID);

            foreach (SupplierEmpMenuAccess l in lst)
            {
                objSupplierMenuAccessRepo.Delete(l);
                objSupplierMenuAccessRepo.SubmitChanges();
            }

            //delete from supplierdataaccess

            SupplierDataAccessRepository objSupplierDataAccessRepos = new SupplierDataAccessRepository();
            SupplierEmpDataAccess objSupplierEmpDataAccessfordel = new SupplierEmpDataAccess();

            List<SupplierEmpDataAccess> lstData = objSupplierDataAccessRepos.GetDataBySupplierEmpId(this.SupplierEmployeeID);

            foreach (SupplierEmpDataAccess l in lstData)
            {
                objSupplierDataAccessRepos.Delete(l);
                objSupplierDataAccessRepos.SubmitChanges();
            }


            //Insert in Supplierempmenuaccess

            //Menu Access
            foreach (DataListItem dt in dtlMenus.Items)
            {
                if (((CheckBox)dt.FindControl("chkdtlMenus")).Checked == true)
                {
                    SupplierMenuAccessRepository objSupplierMenuAccessRep = new SupplierMenuAccessRepository();
                    SupplierEmpMenuAccess objSupplierEmpMenuAccess = new SupplierEmpMenuAccess();
                    objSupplierEmpMenuAccess.SupplierEmployeeID = this.SupplierEmployeeID;
                    objSupplierEmpMenuAccess.MenuPrivilegeID = Convert.ToInt64(((HiddenField)dt.FindControl("hdnMenuAccess")).Value);
                    objSupplierMenuAccessRep.Insert(objSupplierEmpMenuAccess);
                    objSupplierMenuAccessRep.SubmitChanges();
                }

            }

            //Insert in Supplierempdataaccess
            foreach (DataListItem dtData in dtlDataAccess.Items)
            {
                if (((CheckBox)dtData.FindControl("chkdtlDataAccess")).Checked == true)
                {
                    SupplierDataAccessRepository objSupplierDataAccessRep = new SupplierDataAccessRepository();
                    SupplierEmpDataAccess objSupplierEmpDataAccess = new SupplierEmpDataAccess();
                    objSupplierEmpDataAccess.SupplierEmployeeID = this.SupplierEmployeeID;
                    objSupplierEmpDataAccess.DataPrivilegeID = Convert.ToInt64(((HiddenField)dtData.FindControl("hdnDataBaseAccess")).Value);
                    objSupplierDataAccessRep.Insert(objSupplierEmpDataAccess);
                    objSupplierDataAccessRep.SubmitChanges();
                }

            }
            Response.Write("Saved Successfull");
            Response.Redirect("SupplierPersonalInformation.aspx?id=" + Convert.ToInt32(this.SupplierID.ToString()) + "&SubId=" + Convert.ToInt32(this.SupplierEmployeeID.ToString()));

        }
        catch (Exception ex)
        {

        }
    }
}
