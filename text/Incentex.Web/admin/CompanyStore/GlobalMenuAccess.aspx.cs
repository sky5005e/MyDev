using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Company_Employee_MenuAccess : PageBase
{
    #region Data Members

    String selectedmenuoption = "";
    String selectedemployeeuniform = "";
    String seelctedadditonalinfo = "";
    String selectedcompanystore = "";
    String selecteduniform = "";
    String selectedsupplies = "";

    Common objcomm = new Common();

    SubCatogeryRepository objSubCatogeryRepository = new SubCatogeryRepository();

    Int64 CompanyStoreId
    {
        get
        {
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {
            ViewState["CompanyStoreId"] = value;
        }
    }

    Int64 WorkGroupId
    {
        get
        {
            return Convert.ToInt64(ViewState["WorkGroupId"]);
        }
        set
        {
            ViewState["WorkGroupId"] = value;
        }
    }

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

            if (Request.QueryString.Count > 0)
            {
                this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));

                if (this.CompanyStoreId == 0)
                {
                    Response.Redirect("~/admin/CompanyStore/GeneralStoreSetup.aspx?Id=" + this.CompanyStoreId);
                }

                if (this.CompanyStoreId > 0 && !base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                this.WorkGroupId = Convert.ToInt64(Request.QueryString.Get("SubId"));

                ((Label)Master.FindControl("lblPageHeading")).Text = "Global Menu Access By Store Workgroup";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/ViewGlobalMenuSetting.aspx?ID=" + this.CompanyStoreId;

                Session["ManageID"] = 5;
                menuControl.PopulateMenu(11, 1, this.CompanyStoreId, this.WorkGroupId, true);
            }

            SetWorkGroupPreferences();
            BindData();
            DisplayData();
        }
    }

    protected void lnkBtnApplyAndOverride_Click(object sender, EventArgs e)
    {
        Boolean IsError = false;

        try
        {
            CompanyEmpMenuAccessRepository objCompanyEmpMenuAccessRepository = new CompanyEmpMenuAccessRepository();
            CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
            List<CompanyEmployee> lstCompanyemployee = objCompanyEmployeeRepository.GetByWorkgroupID(this.WorkGroupId, new CompanyStoreRepository().GetById(this.CompanyStoreId).CompanyID);

            GlobalMenuSetting objCompanyByWorkgroup = new GlobalMenuSetting();
            GlobalMenuSettingRepository objMenuRepo = new GlobalMenuSettingRepository();
            objCompanyByWorkgroup = objMenuRepo.GetById(this.WorkGroupId, this.CompanyStoreId);

            if (objCompanyByWorkgroup == null)
            {
                //Insert here first
                GlobalMenuSetting objGlobalInsert = new GlobalMenuSetting();
                objGlobalInsert.StoreId = this.CompanyStoreId;
                objGlobalInsert.WorkgroupId = this.WorkGroupId;
                objMenuRepo.Insert(objGlobalInsert);
                objMenuRepo.SubmitChanges();

                objCompanyByWorkgroup = objMenuRepo.GetById(this.WorkGroupId, this.CompanyStoreId);
            }

            #region Menu Access

            foreach (DataListItem dt in dtlMenus.Items)
            {
                if (((CheckBox)dt.FindControl("chkdtlMenus")).Checked == true)
                {
                    if (selectedmenuoption == "")
                        selectedmenuoption = ((HiddenField)dt.FindControl("hdnMenuAccess")).Value;
                    else
                        selectedmenuoption = selectedmenuoption + "," + ((HiddenField)dt.FindControl("hdnMenuAccess")).Value;
                }
            }
            objCompanyByWorkgroup.MainMenuAceess = selectedmenuoption;
            
            #endregion

            #region Employee Uniform

            foreach (DataListItem dtEmp in dtEmpUniform.Items)
            {
                if (((CheckBox)dtEmp.FindControl("chkEmpUniform")).Checked == true)
                {
                    if (selectedemployeeuniform == "")
                        selectedemployeeuniform = ((HiddenField)dtEmp.FindControl("hdnEmpUni")).Value;
                    else
                        selectedemployeeuniform = selectedemployeeuniform + "," + ((HiddenField)dtEmp.FindControl("hdnEmpUni")).Value;
                }
            }

            objCompanyByWorkgroup.EmployeeUniformAccess = selectedemployeeuniform;
            
            #endregion

            #region Additional Information

            foreach (DataListItem dt in dtAddiInfo.Items)
            {
                if (((CheckBox)dt.FindControl("chkAddiInfo")).Checked == true)
                {
                    if (seelctedadditonalinfo == "")
                        seelctedadditonalinfo = ((HiddenField)dt.FindControl("hdnAddiInfo")).Value;
                    else
                        seelctedadditonalinfo = seelctedadditonalinfo + "," + ((HiddenField)dt.FindControl("hdnAddiInfo")).Value;
                }
            }
            objCompanyByWorkgroup.AdditionInfoAccess = seelctedadditonalinfo;
            
            #endregion

            #region Company store

            foreach (DataListItem dt in dtCompanyStore.Items)
            {
                if (((CheckBox)dt.FindControl("chkCompanyStore")).Checked == true)
                {
                    if (selectedcompanystore == "")
                        selectedcompanystore = ((HiddenField)dt.FindControl("hdnCompanyStore")).Value;
                    else
                        selectedcompanystore = selectedcompanystore + "," + ((HiddenField)dt.FindControl("hdnCompanyStore")).Value;
                }
            }
            objCompanyByWorkgroup.CompanyStoreAccess = selectedcompanystore;
            
            #endregion

            #region Uniform Purchasing

            foreach (DataListItem dt in dtUniPurchasing.Items)
            {
                if (((CheckBox)dt.FindControl("chkUniPurchasing")).Checked == true)
                {
                    if (selecteduniform == "")
                        selecteduniform = ((HiddenField)dt.FindControl("hdnUniformPurchasing")).Value;
                    else
                        selecteduniform = selecteduniform + "," + ((HiddenField)dt.FindControl("hdnUniformPurchasing")).Value;
                }
            }
            objCompanyByWorkgroup.UniformPurchasingAccess = selecteduniform;
            
            #endregion

            #region Supplies

            foreach (DataListItem dts in dtSupplies.Items)
            {
                if (((CheckBox)dts.FindControl("chkSupplies")).Checked == true)
                {
                    if (selectedsupplies == "")
                        selectedsupplies = ((HiddenField)dts.FindControl("hdnSupplies")).Value;
                    else
                        selectedsupplies = selectedsupplies + "," + ((HiddenField)dts.FindControl("hdnSupplies")).Value;
                }
            }
            objCompanyByWorkgroup.SuppliesAccess = selectedsupplies;
            
            #endregion

            //Push the changes to the database
            objMenuRepo.SubmitChanges();

            #region Add to existing employee
            foreach (CompanyEmployee employee in lstCompanyemployee)
            {
                //Menu Access
                //First delete menu item for this user
                List<CompanyEmpMenuAccess> lst = objCompanyEmpMenuAccessRepository.getMenuByUserType(employee.CompanyEmployeeID, "Company Admin,Company Employee");
                foreach (CompanyEmpMenuAccess l in lst)
                {
                    objCompanyEmpMenuAccessRepository.Delete(l);
                    objCompanyEmpMenuAccessRepository.SubmitChanges();
                }
                //Insert all item to database for this user
                String[] strSelectedMenuOption = selectedmenuoption.Split(',');
                for (Int32 i = 0; i < strSelectedMenuOption.Length; i++)
                {
                    if (!String.IsNullOrEmpty(strSelectedMenuOption[i]))
                    {
                        CompanyEmpMenuAccess obj = new CompanyEmpMenuAccess();
                        obj.CompanyEmployeeID = employee.CompanyEmployeeID;
                        obj.MenuPrivilegeID = Convert.ToInt64(strSelectedMenuOption[i]);
                        objCompanyEmpMenuAccessRepository.Insert(obj);
                        objCompanyEmpMenuAccessRepository.SubmitChanges();
                    }
                }

                CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetById(employee.CompanyEmployeeID);
                objCompanyEmployee.EmployeeUniformAccess = selectedemployeeuniform;
                objCompanyEmployee.AdditionInfoAccess = seelctedadditonalinfo;
                objCompanyEmployee.CompanyStoreAccess = selectedcompanystore;
                objCompanyEmployee.UniformPurchasingAccess = selecteduniform;
                objCompanyEmployee.SuppliesAccess = selectedsupplies;
            }
            objCompanyEmployeeRepository.SubmitChanges();
            #endregion
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
            IsError = true;
        }

        if (!IsError)
            Response.Redirect("~/admin/CompanyStore/StoreFrontSetting.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.WorkGroupId);
    }

    protected void lnkbtnApplyAndAddition_Click(object sender, EventArgs e)
    {
        Boolean IsError = false;

        try
        {
            CompanyEmpMenuAccessRepository objCompanyEmpMenuAccessRepository = new CompanyEmpMenuAccessRepository();
            CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
            List<CompanyEmployee> lstCompanyemployee = objCompanyEmployeeRepository.GetByWorkgroupID(this.WorkGroupId, new CompanyStoreRepository().GetById(this.CompanyStoreId).CompanyID);

            GlobalMenuSetting objCompanyByWorkgroup = new GlobalMenuSetting();
            GlobalMenuSettingRepository objMenuRepo = new GlobalMenuSettingRepository();
            objCompanyByWorkgroup = objMenuRepo.GetById(this.WorkGroupId, this.CompanyStoreId);

            if (objCompanyByWorkgroup == null)
            {
                //Insert here first
                GlobalMenuSetting objGlobalInsert = new GlobalMenuSetting();
                objGlobalInsert.StoreId = this.CompanyStoreId;
                objGlobalInsert.WorkgroupId = this.WorkGroupId;
                objMenuRepo.Insert(objGlobalInsert);
                objMenuRepo.SubmitChanges();

                objCompanyByWorkgroup = objMenuRepo.GetById(this.WorkGroupId, this.CompanyStoreId);
            }

            #region Menu Access

            foreach (DataListItem dt in dtlMenus.Items)
            {
                if (((CheckBox)dt.FindControl("chkdtlMenus")).Checked == true)
                {
                    if (selectedmenuoption == "")
                        selectedmenuoption = ((HiddenField)dt.FindControl("hdnMenuAccess")).Value;
                    else
                        selectedmenuoption = selectedmenuoption + "," + ((HiddenField)dt.FindControl("hdnMenuAccess")).Value;
                }
            }
            objCompanyByWorkgroup.MainMenuAceess = selectedmenuoption;
            
            #endregion

            #region Employee Uniform

            foreach (DataListItem dtEmp in dtEmpUniform.Items)
            {
                if (((CheckBox)dtEmp.FindControl("chkEmpUniform")).Checked == true)
                {
                    if (selectedemployeeuniform == "")
                        selectedemployeeuniform = ((HiddenField)dtEmp.FindControl("hdnEmpUni")).Value;
                    else
                        selectedemployeeuniform = selectedemployeeuniform + "," + ((HiddenField)dtEmp.FindControl("hdnEmpUni")).Value;
                }
            }
            objCompanyByWorkgroup.EmployeeUniformAccess = selectedemployeeuniform;
            
            #endregion

            #region Additional Information

            foreach (DataListItem dt in dtAddiInfo.Items)
            {
                if (((CheckBox)dt.FindControl("chkAddiInfo")).Checked == true)
                {
                    if (seelctedadditonalinfo == "")
                        seelctedadditonalinfo = ((HiddenField)dt.FindControl("hdnAddiInfo")).Value;
                    else
                        seelctedadditonalinfo = seelctedadditonalinfo + "," + ((HiddenField)dt.FindControl("hdnAddiInfo")).Value;
                }
            }
            objCompanyByWorkgroup.AdditionInfoAccess = seelctedadditonalinfo;
            
            #endregion

            #region Company store

            foreach (DataListItem dt in dtCompanyStore.Items)
            {
                if (((CheckBox)dt.FindControl("chkCompanyStore")).Checked == true)
                {
                    if (selectedcompanystore == "")
                        selectedcompanystore = ((HiddenField)dt.FindControl("hdnCompanyStore")).Value;
                    else
                        selectedcompanystore = selectedcompanystore + "," + ((HiddenField)dt.FindControl("hdnCompanyStore")).Value;
                }
            }
            objCompanyByWorkgroup.CompanyStoreAccess = selectedcompanystore;
            
            #endregion

            #region Uniform Purchasing

            foreach (DataListItem dt in dtUniPurchasing.Items)
            {
                if (((CheckBox)dt.FindControl("chkUniPurchasing")).Checked == true)
                {
                    if (selecteduniform == "")
                        selecteduniform = ((HiddenField)dt.FindControl("hdnUniformPurchasing")).Value;
                    else
                        selecteduniform = selecteduniform + "," + ((HiddenField)dt.FindControl("hdnUniformPurchasing")).Value;
                }
            }
            objCompanyByWorkgroup.UniformPurchasingAccess = selecteduniform;
            
            #endregion

            #region Supplies

            foreach (DataListItem dts in dtSupplies.Items)
            {
                if (((CheckBox)dts.FindControl("chkSupplies")).Checked == true)
                {
                    if (selectedsupplies == "")
                        selectedsupplies = ((HiddenField)dts.FindControl("hdnSupplies")).Value;
                    else
                        selectedsupplies = selectedsupplies + "," + ((HiddenField)dts.FindControl("hdnSupplies")).Value;
                }
            }
            objCompanyByWorkgroup.SuppliesAccess = selectedsupplies;
            
            #endregion

            //Push the changes to the database
            objMenuRepo.SubmitChanges();

            #region Addition to existing employee
            foreach (CompanyEmployee employee in lstCompanyemployee)
            {
                //Menu Access
                List<CompanyEmpMenuAccess> objCompanyEmpMenuAccess = objCompanyEmpMenuAccessRepository.GetMenusByEmployeeId(employee.CompanyEmployeeID);
                if (selectedmenuoption.Length > 0)
                {
                    String[] strSelectedMenuOption = selectedmenuoption.Split(',');
                    for (Int32 i = 0; i < strSelectedMenuOption.Length; i++)
                    {
                        Boolean isExit = false;
                        for (Int32 j = 0; j < objCompanyEmpMenuAccess.Count; j++)
                        {
                            if (objCompanyEmpMenuAccess[j].MenuPrivilegeID == Convert.ToInt64(strSelectedMenuOption[i]))
                                isExit = true;
                        }
                        if (isExit == false)
                        {
                            if (!String.IsNullOrEmpty(strSelectedMenuOption[i]))
                            {
                                CompanyEmpMenuAccess obj = new CompanyEmpMenuAccess();
                                obj.CompanyEmployeeID = employee.CompanyEmployeeID;
                                obj.MenuPrivilegeID = Convert.ToInt64(strSelectedMenuOption[i]);
                                objCompanyEmpMenuAccessRepository.Insert(obj);
                                objCompanyEmpMenuAccessRepository.SubmitChanges();
                            }
                        }
                    }
                }

                //Employee Uniform
                List<String> lstCompanyEmployeeUniform = new List<String>((employee.EmployeeUniformAccess != null ? employee.EmployeeUniformAccess : "").Split(','));
                if (selectedemployeeuniform.Length > 0)
                {
                    String[] strSelectedEmployeeUniform = selectedemployeeuniform.Split(',');
                    for (Int32 i = 0; i < strSelectedEmployeeUniform.Length; i++)
                    {
                        Boolean isExit = false;
                        for (Int32 j = 0; j < lstCompanyEmployeeUniform.Count; j++)
                        {
                            if (lstCompanyEmployeeUniform[j] == strSelectedEmployeeUniform[i])
                                isExit = true;
                        }
                        if (isExit == false)
                            lstCompanyEmployeeUniform.Add(strSelectedEmployeeUniform[i]);
                    }
                }
                String strEUOption = String.Join(",", lstCompanyEmployeeUniform.ToArray());
                strEUOption = strEUOption.StartsWith(",") ? strEUOption.Substring(1) : strEUOption;
                strEUOption = strEUOption.EndsWith(",") ? strEUOption.Substring(0, strEUOption.Length - 1) : strEUOption;

                //Additional Information                
                List<String> lstCompanyEmployeeAdditionInfo = new List<String>((employee.AdditionInfoAccess != null ? employee.AdditionInfoAccess : "").Split(','));
                if (seelctedadditonalinfo.Length > 0)
                {
                    String[] strSeelctedAdditonalInfom = seelctedadditonalinfo.Split(',');
                    for (Int32 i = 0; i < strSeelctedAdditonalInfom.Length; i++)
                    {
                        Boolean isExit = false;
                        for (Int32 j = 0; j < lstCompanyEmployeeAdditionInfo.Count; j++)
                        {
                            if (lstCompanyEmployeeAdditionInfo[j] == strSeelctedAdditonalInfom[i])
                                isExit = true;
                        }
                        if (isExit == false)
                            lstCompanyEmployeeAdditionInfo.Add(strSeelctedAdditonalInfom[i]);
                    }
                }
                String strAIOption = String.Join(",", lstCompanyEmployeeAdditionInfo.ToArray());
                strAIOption = strAIOption.StartsWith(",") ? strAIOption.Substring(1) : strAIOption;
                strAIOption = strAIOption.EndsWith(",") ? strAIOption.Substring(0, strAIOption.Length - 1) : strAIOption;

                //Company Store
                List<String> lstCompanyEmployeeCompanystore = new List<String>((employee.CompanyStoreAccess != null ? employee.CompanyStoreAccess : "").Split(','));
                if (selectedcompanystore.Length > 0)
                {
                    String[] strselectedcompanystore = selectedcompanystore.Split(',');
                    for (Int32 i = 0; i < strselectedcompanystore.Length; i++)
                    {
                        Boolean isExit = false;
                        for (Int32 j = 0; j < lstCompanyEmployeeCompanystore.Count; j++)
                        {
                            if (lstCompanyEmployeeCompanystore[j] == strselectedcompanystore[i])
                                isExit = true;
                        }
                        if (isExit == false)
                            lstCompanyEmployeeCompanystore.Add(strselectedcompanystore[i]);
                    }
                }
                String strCSOption = String.Join(",", lstCompanyEmployeeCompanystore.ToArray());
                strCSOption = strCSOption.StartsWith(",") ? strCSOption.Substring(1) : strCSOption;
                strCSOption = strCSOption.EndsWith(",") ? strCSOption.Substring(0, strCSOption.Length - 1) : strCSOption;

                //Uniform Purchasing                
                List<String> lstCompanyEmployeeUniformPurchasing = new List<String>((employee.UniformPurchasingAccess != null ? employee.UniformPurchasingAccess : "").Split(','));
                if (selectedcompanystore.Length > 0)
                {
                    String[] strSelectedUniform = selecteduniform.Split(',');
                    for (Int32 i = 0; i < strSelectedUniform.Length; i++)
                    {
                        Boolean isExit = false;
                        for (Int32 j = 0; j < lstCompanyEmployeeUniformPurchasing.Count; j++)
                        {
                            if (lstCompanyEmployeeUniformPurchasing[j] == strSelectedUniform[i])
                                isExit = true;
                        }
                        if (isExit == false)
                            lstCompanyEmployeeUniformPurchasing.Add(strSelectedUniform[i]);
                    }
                }
                String strUPOption = String.Join(",", lstCompanyEmployeeUniformPurchasing.ToArray());
                strUPOption = strUPOption.StartsWith(",") ? strUPOption.Substring(1) : strUPOption;
                strUPOption = strUPOption.EndsWith(",") ? strUPOption.Substring(0, strUPOption.Length - 1) : strUPOption;


                //Uniform Purchasing
                List<String> lstCompanyEmployeeSuppliesAccess = new List<String>((employee.SuppliesAccess != null ? employee.SuppliesAccess : "").Split(','));
                if (selectedsupplies.Length > 0)
                {
                    String[] strSelectedSupplies = selectedsupplies.Split(',');
                    for (Int32 i = 0; i < strSelectedSupplies.Length; i++)
                    {
                        Boolean isExit = false;
                        for (Int32 j = 0; j < lstCompanyEmployeeSuppliesAccess.Count; j++)
                        {
                            if (lstCompanyEmployeeSuppliesAccess[j] == strSelectedSupplies[i])
                                isExit = true;
                        }
                        if (isExit == false)
                            lstCompanyEmployeeSuppliesAccess.Add(strSelectedSupplies[i]);
                    }
                }
                String strSAOption = String.Join(",", lstCompanyEmployeeSuppliesAccess.ToArray());
                strSAOption = strSAOption.StartsWith(",") ? strSAOption.Substring(1) : strSAOption;
                strSAOption = strSAOption.EndsWith(",") ? strSAOption.Substring(0, strSAOption.Length - 1) : strSAOption;

                CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetById(employee.CompanyEmployeeID);
                objCompanyEmployee.EmployeeUniformAccess = strEUOption;
                objCompanyEmployee.AdditionInfoAccess = strAIOption;
                objCompanyEmployee.CompanyStoreAccess = strCSOption;
                objCompanyEmployee.UniformPurchasingAccess = strUPOption;
                objCompanyEmployee.SuppliesAccess = strSAOption;
            }
            objCompanyEmployeeRepository.SubmitChanges();
            #endregion            
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
            IsError = true;
        }

        if (!IsError)
            Response.Redirect("~/admin/CompanyStore/StoreFrontSetting.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.WorkGroupId);
    }

    

    protected void ddlWorkGroupManager_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GlobalMenuSettingRepository objMenuRepo = new GlobalMenuSettingRepository();
            GlobalMenuSetting objCompanyByWorkgroup = objMenuRepo.GetById(this.WorkGroupId, this.CompanyStoreId);

            if (!String.IsNullOrEmpty(ddlWorkGroupManager.SelectedValue) && Convert.ToInt64(ddlWorkGroupManager.SelectedValue) > 0)
                objCompanyByWorkgroup.WorkGroupManagerID = Convert.ToInt64(ddlWorkGroupManager.SelectedValue);
            else
                objCompanyByWorkgroup.WorkGroupManagerID = null;

            objMenuRepo.SubmitChanges();

            lblMsg.Text = "Work Group Manager changed successfully...";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }

    protected void ddlPreference_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlPreference = (DropDownList)sender;
            HiddenField hdnKeyID = ((DropDownList)(sender)).Parent.FindControl("hdnKeyID") as HiddenField;
            PreferenceRepository objPreferenceRepo = new PreferenceRepository();
            WorkGroupPreference objWorkGroupPreference = objPreferenceRepo.GetWorkGroupPreferenceByWorkGroupID(this.WorkGroupId,this.CompanyStoreId,Convert.ToInt64(hdnKeyID.Value));
            if (objWorkGroupPreference == null)
            {
                objWorkGroupPreference = new WorkGroupPreference();
                objWorkGroupPreference.StoreID = this.CompanyStoreId;
                objWorkGroupPreference.WorkGroupID = this.WorkGroupId;
                objWorkGroupPreference.PreferenceID = Convert.ToInt64(hdnKeyID.Value);
                objWorkGroupPreference.PreferenceValueID = Convert.ToInt64(ddlPreference.SelectedValue);
                objWorkGroupPreference.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objWorkGroupPreference.CreatedDate = DateTime.Now;
                objPreferenceRepo.Insert(objWorkGroupPreference);
            }
            else
            {
                objWorkGroupPreference.StoreID = this.CompanyStoreId;
                objWorkGroupPreference.WorkGroupID = this.WorkGroupId;
                objWorkGroupPreference.PreferenceID = Convert.ToInt64(hdnKeyID.Value);
                objWorkGroupPreference.PreferenceValueID = Convert.ToInt64(ddlPreference.SelectedValue);
                objWorkGroupPreference.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objWorkGroupPreference.UpdatedDate = DateTime.Now;
               // objPreferenceRepo.Delete(objWorkGroupPreference);
            }
            objPreferenceRepo.SubmitChanges();
            lblMsg.Text = "Approver Level changed successfully...";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dlPreferences_ItemDataBound(object sender, DataListItemEventArgs e)
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
                PreferenceRepository objPreferenceRepos = new PreferenceRepository();
                List<PreferenceValue> lstPreference = objPreferenceRepos.GetPreferenceValuesByPreferenceID(PreferenceID);
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

    #region Methods

    void DisplayData()
    {   
        if (this.WorkGroupId != 0)
        {   
            lblWorkGroup.Text = new LookupRepository().GetById(this.WorkGroupId).sLookupName.ToString();
            GlobalMenuSetting objValuebyworkgroup = new GlobalMenuSettingRepository().GetById(this.WorkGroupId, this.CompanyStoreId);

            if (objValuebyworkgroup != null)
            {
                if (objValuebyworkgroup.WorkGroupManagerID != null)
                    ddlWorkGroupManager.SelectedValue = Convert.ToString(objValuebyworkgroup.WorkGroupManagerID);

                CheckBox chk;
                HiddenField lblId;

                //Get menu from empmenuaccess table
                if (objValuebyworkgroup.MainMenuAceess != null)
                {
                    String[] MainIds = objValuebyworkgroup.MainMenuAceess.Split(',');
                    foreach (String i in MainIds)
                    {
                        foreach (DataListItem dtM in dtlMenus.Items)
                        {
                            chk = dtM.FindControl("chkdtlMenus") as CheckBox;
                            lblId = dtM.FindControl("hdnMenuAccess") as HiddenField;
                            HtmlGenericControl dvChk = dtM.FindControl("menuspan") as HtmlGenericControl;

                            if (i.Equals(lblId.Value))
                            {
                                chk.Checked = true;
                                dvChk.Attributes.Add("class", "custom-checkbox_checked");
                                break;
                            }
                        }
                    }
                }

                //Employee Uniform
                if (objValuebyworkgroup.EmployeeUniformAccess != null)
                {
                    String[] Ids = objValuebyworkgroup.EmployeeUniformAccess.Split(',');
                    foreach (String i in Ids)
                    {
                        foreach (DataListItem dt in dtEmpUniform.Items)
                        {
                            chk = dt.FindControl("chkEmpUniform") as CheckBox;
                            lblId = dt.FindControl("hdnEmpUni") as HiddenField;
                            HtmlGenericControl dvChk = dt.FindControl("menuemployeeuni") as HtmlGenericControl;

                            if (i.Equals(lblId.Value))
                            {
                                chk.Checked = true;
                                dvChk.Attributes.Add("class", "custom-checkbox_checked");
                                break;
                            }
                        }
                    }

                }

                // Additional Information
                if (objValuebyworkgroup.AdditionInfoAccess != null)
                {
                    String[] additioninfoIds = objValuebyworkgroup.AdditionInfoAccess.Split(',');
                    foreach (String i in additioninfoIds)
                    {
                        foreach (DataListItem dtaddiinfo in dtAddiInfo.Items)
                        {
                            chk = dtaddiinfo.FindControl("chkAddiInfo") as CheckBox;
                            lblId = dtaddiinfo.FindControl("hdnAddiInfo") as HiddenField;
                            HtmlGenericControl dvChk = dtaddiinfo.FindControl("addinfospan") as HtmlGenericControl;

                            if (i.Equals(lblId.Value))
                            {
                                chk.Checked = true;
                                dvChk.Attributes.Add("class", "custom-checkbox_checked");
                                break;
                            }
                        }
                    }
                }

                // Company Store

                if (objValuebyworkgroup.CompanyStoreAccess != null)
                {
                    String[] companystoreIds = objValuebyworkgroup.CompanyStoreAccess.Split(',');
                    foreach (String i in companystoreIds)
                    {
                        foreach (DataListItem dtcmpnystore in dtCompanyStore.Items)
                        {
                            chk = dtcmpnystore.FindControl("chkCompanyStore") as CheckBox;
                            lblId = dtcmpnystore.FindControl("hdnCompanyStore") as HiddenField;
                            HtmlGenericControl dvChk = dtcmpnystore.FindControl("companystorespan") as HtmlGenericControl;

                            if (i.Equals(lblId.Value))
                            {
                                chk.Checked = true;
                                dvChk.Attributes.Add("class", "custom-checkbox_checked");
                                break;
                            }
                        }
                    }
                }

                // Uniform Purchasing
                if (objValuebyworkgroup.UniformPurchasingAccess != null)
                {
                    String[] UniformPurchasingIds = objValuebyworkgroup.UniformPurchasingAccess.Split(',');
                    foreach (String i in UniformPurchasingIds)
                    {
                        foreach (DataListItem dtuniformpurhcasing in dtUniPurchasing.Items)
                        {
                            chk = dtuniformpurhcasing.FindControl("chkUniPurchasing") as CheckBox;
                            lblId = dtuniformpurhcasing.FindControl("hdnUniformPurchasing") as HiddenField;
                            HtmlGenericControl dvChk = dtuniformpurhcasing.FindControl("uniformspan") as HtmlGenericControl;

                            if (i.Equals(lblId.Value))
                            {
                                chk.Checked = true;
                                dvChk.Attributes.Add("class", "custom-checkbox_checked");
                                break;
                            }
                        }
                    }
                }

                // Supplies
                if (objValuebyworkgroup.SuppliesAccess != null)
                {
                    String[] suppliesIds = objValuebyworkgroup.SuppliesAccess.Split(',');
                    foreach (String i in suppliesIds)
                    {
                        foreach (DataListItem dtsupplies in dtSupplies.Items)
                        {
                            chk = dtsupplies.FindControl("chkSupplies") as CheckBox;
                            lblId = dtsupplies.FindControl("hdnSupplies") as HiddenField;
                            HtmlGenericControl dvChk = dtsupplies.FindControl("supplliespan") as HtmlGenericControl;

                            if (i.Equals(lblId.Value))
                            {
                                chk.Checked = true;
                                dvChk.Attributes.Add("class", "custom-checkbox_checked");
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    public void BindData()
    {
        dtAddiInfo.DataSource = new LookupRepository().GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.AdditionalInfo);
        dtAddiInfo.DataBind();

        dtEmpUniform.DataSource = objSubCatogeryRepository.GetAllSubCategory(1);
        dtEmpUniform.DataBind();

        dtCompanyStore.DataSource = objSubCatogeryRepository.GetAllSubCategory(3);
        dtCompanyStore.DataBind();

        dtUniPurchasing.DataSource = objSubCatogeryRepository.GetAllSubCategory(4);
        dtUniPurchasing.DataBind();

        dtSupplies.DataSource = objSubCatogeryRepository.GetAllSubCategory(2);
        dtSupplies.DataBind();

        MenuPrivilegeRepository objMenu = new MenuPrivilegeRepository();
        List<INC_MenuPrivilege> objList = objMenu.GetFrontMenu(DAEnums.MenuTypes.FrontEnd.ToString(), "Company" + " " + DAEnums.CompanyEmployeeTypes.Admin.ToString() + "," + "Company" + " " + DAEnums.CompanyEmployeeTypes.Employee.ToString());
        dtlMenus.DataSource = objList;
        dtlMenus.DataBind();

        ddlWorkGroupManager.DataSource = new UserInformationRepository().GetCompanyEmployees(new CompanyStoreRepository().GetById(this.CompanyStoreId).CompanyID, this.WorkGroupId, null);
        ddlWorkGroupManager.DataTextField = "UserName";
        ddlWorkGroupManager.DataValueField = "UserInfoID";
        ddlWorkGroupManager.DataBind();
        ddlWorkGroupManager.Items.Insert(0, new ListItem("-Select Workgroup Manager-", "0"));
    }

    private void SetWorkGroupPreferences()
    {

        List<FUN_GetWorkGroupPreferenceResult> lstPref = new PreferenceRepository().GetWorkGroupPreferences(this.WorkGroupId,this.CompanyStoreId);

        if (lstPref != null && lstPref.Count > 0)
        {
            dlPreferences.DataSource = lstPref;
            dlPreferences.DataBind();
        }
    }

    #endregion
}