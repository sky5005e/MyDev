using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Company_Employee_ShoppingSetting : PageBase
{
    #region Data Members
    LookupDA sEU = new LookupDA();
    LookupBE sEUBE = new LookupBE();
    string selecteduserstoreoption = "";
    string selectedworldlinkpayment = "";
    string selectedthirdpartysupplierpayment = "";
    string selectedcheckoutinformation = "";
    Common objcomm = new Common();
    Int64 CompanyStoreId
    {
        get
        {
            if (ViewState["CompanyStoreId"] == null)
            {
                ViewState["CompanyStoreId"] = 0;
            }
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
            if (ViewState["WorkGroupId"] == null)
            {
                ViewState["WorkGroupId"] = 0;
            }
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

                ((Label)Master.FindControl("lblPageHeading")).Text = "Storefront Settings by store workgroup";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/ViewGlobalMenuSetting.aspx?ID=" + this.CompanyStoreId;

                Session["ManageID"] = 5;
                menuControl.PopulateMenu(11, 2, this.CompanyStoreId, this.WorkGroupId, true);

                BindDDL(sender, e);
            }

            bindShoppingSetting();
            DisplayData(sender, e);
        }
    }

    void BindDDL(object sender, EventArgs e)
    {
        CountryRepository objCountryBillingRepo = new CountryRepository();
        List<INC_Country> objBillingCountryList = objCountryBillingRepo.GetAll();
        Common.BindDDL(DrpBillingCountry, objBillingCountryList, "sCountryName", "iCountryID", "-select country-");
        DrpBillingCountry.SelectedValue = DrpBillingCountry.Items.FindByText("United States").Value;
        DrpBillingCountry_SelectedIndexChanged(sender, e);
    }

    protected void DrpBillingCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(DrpBillingCountry.SelectedValue));

        DrpBillingState.Items.Clear();
        DrpBillingCity.Items.Clear();
        DrpBillingCity.Items.Insert(0, new ListItem("-select city-", "0"));
        Common.BindDDL(DrpBillingState, objStateList, "sStatename", "iStateID", "-select state-");
    }

    protected void DrpBillingState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(DrpBillingState.SelectedValue));
        DrpBillingCity.Items.Clear();
        Common.BindDDL(DrpBillingCity, objCityList, "sCityName", "iCityID", "-select city-");
    }

    protected void lnkBtnApplyAndOverride_Click(object sender, EventArgs e)
    {
        try
        {
            CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
            List<CompanyEmployee> lstCompanyemployee = objCompanyEmployeeRepository.GetByWorkgroupID(this.WorkGroupId, new CompanyStoreRepository().GetById(this.CompanyStoreId).CompanyID);

            GlobalMenuSetting objCompanyByWorkgroup = new GlobalMenuSetting();
            GlobalMenuSettingRepository objMenuRepo = new GlobalMenuSettingRepository();
            List<string> MOASUserIds = new List<string>();
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

            #region User Store Option
            foreach (DataListItem dtuserstore in dtUserStoreFront.Items)
            {
                if (((CheckBox)dtuserstore.FindControl("chkUserStoreFront")).Checked == true)
                {

                    if (selecteduserstoreoption == "")
                    {
                        selecteduserstoreoption = ((HiddenField)dtuserstore.FindControl("hdnStoreFront")).Value;

                    }
                    else
                    {
                        selecteduserstoreoption = selecteduserstoreoption + "," + ((HiddenField)dtuserstore.FindControl("hdnStoreFront")).Value;

                    }

                }
            }

            objCompanyByWorkgroup.Userstoreoption = selecteduserstoreoption;
            objMenuRepo.SubmitChanges();

            #endregion

            #region Payment Option
            Boolean IsMoasApply = false;
            Boolean IsEmployeePayrollDuductApply = false;
            string selectedworldlinkpaymentBeforeHireDate = "";
            string selectedworldlinkpaymentAfterHireDate = "";
            foreach (DataListItem dtpayoption in dtPaymentOptions.Items)
            {
                if (((CheckBox)dtpayoption.FindControl("chkPaymentOptions")).Checked == true)
                {
                    if (selectedworldlinkpayment == "")
                        selectedworldlinkpayment = ((HiddenField)dtpayoption.FindControl("hdnPaymentOption")).Value;
                    else
                        selectedworldlinkpayment = selectedworldlinkpayment + "," + ((HiddenField)dtpayoption.FindControl("hdnPaymentOption")).Value;

                    if (((Label)dtpayoption.FindControl("lblPaymentOptions")).Text == "Employee Payroll Deduct")
                        IsEmployeePayrollDuductApply = true;
                    if (((Label)dtpayoption.FindControl("lblPaymentOptions")).Text == "MOAS")
                        IsMoasApply = true;
                }
            }

            if (chkIsPaymentOptionByHireDate.Checked == true)
            {
                foreach (DataListItem dtpayoption in dtPaymentOptionsBeforeHireDate.Items)
                {
                    if (((CheckBox)dtpayoption.FindControl("chkPaymentOptionsBeforeHireDate")).Checked == true)
                    {
                        if (selectedworldlinkpaymentBeforeHireDate == "")
                            selectedworldlinkpaymentBeforeHireDate = ((HiddenField)dtpayoption.FindControl("hdnPaymentOptionsBeforeHireDate")).Value;
                        else
                            selectedworldlinkpaymentBeforeHireDate = selectedworldlinkpaymentBeforeHireDate + "," + ((HiddenField)dtpayoption.FindControl("hdnPaymentOptionsBeforeHireDate")).Value;

                        if (((Label)dtpayoption.FindControl("lblPaymentOptionsBeforeHireDate")).Text == "Employee Payroll Deduct")
                            IsEmployeePayrollDuductApply = true;
                        if (((Label)dtpayoption.FindControl("lblPaymentOptionsBeforeHireDate")).Text == "MOAS")
                            IsMoasApply = true;
                    }
                }
                foreach (DataListItem dtpayoption in dtPaymentOptionsAfterHireDate.Items)
                {
                    if (((CheckBox)dtpayoption.FindControl("chkPaymentOptionsAfterHireDate")).Checked == true)
                    {
                        if (selectedworldlinkpaymentAfterHireDate == "")
                            selectedworldlinkpaymentAfterHireDate = ((HiddenField)dtpayoption.FindControl("hdnPaymentOptionsAfterHireDate")).Value;
                        else
                            selectedworldlinkpaymentAfterHireDate = selectedworldlinkpaymentAfterHireDate + "," + ((HiddenField)dtpayoption.FindControl("hdnPaymentOptionsAfterHireDate")).Value;

                        if (((Label)dtpayoption.FindControl("lblPaymentOptionsAfterHireDate")).Text == "Employee Payroll Deduct")
                            IsEmployeePayrollDuductApply = true;
                        if (((Label)dtpayoption.FindControl("lblPaymentOptionsAfterHireDate")).Text == "MOAS")
                            IsMoasApply = true;
                    }
                }
            }

            objCompanyByWorkgroup.Paymentoption = selectedworldlinkpayment;
            objCompanyByWorkgroup.IsPaymentOptionOnHireDate = chkIsPaymentOptionByHireDate.Checked == true ? true : false;
            objCompanyByWorkgroup.PaymentOptionAfterHireDate = selectedworldlinkpaymentAfterHireDate;
            objCompanyByWorkgroup.PaymentOptionBeforeHireDate = selectedworldlinkpaymentBeforeHireDate;
            if (txtHireDate.Text.Trim() != string.Empty)
                objCompanyByWorkgroup.PaymentOptionHireDate = Convert.ToDateTime(txtHireDate.Text.Trim());
            else
                objCompanyByWorkgroup.PaymentOptionHireDate = null;
            objMenuRepo.SubmitChanges();

            if (IsEmployeePayrollDuductApply == true)
            {
                objCompanyByWorkgroup.IsGlobalBilling = true;
                objCompanyByWorkgroup.CompanyName = txtVendorCompany.Text;
                objCompanyByWorkgroup.FirstName = txtVendorFirstName.Text;
                objCompanyByWorkgroup.LastName = txtLastName.Text;
                objCompanyByWorkgroup.Address = txtVendorAddress.Text;
                objCompanyByWorkgroup.CountryId = Convert.ToInt64(DrpBillingCountry.SelectedItem.Value);
                objCompanyByWorkgroup.StateId = Convert.ToInt64(DrpBillingState.SelectedItem.Value);
                objCompanyByWorkgroup.CityId = Convert.ToInt64(DrpBillingCity.SelectedItem.Value);
                objCompanyByWorkgroup.Zip = txtVendorZip.Text;
                objCompanyByWorkgroup.Telephone = txtVendorTelephone.Text;
                objCompanyByWorkgroup.Email = txtVendorEmail.Text;
                objCompanyByWorkgroup.Moblie = TxtMobile.Text;
            }
            else
            {
                objCompanyByWorkgroup.IsGlobalBilling = false;
                objCompanyByWorkgroup.CompanyName = null;
                objCompanyByWorkgroup.FirstName = null;
                objCompanyByWorkgroup.LastName = null;
                objCompanyByWorkgroup.Address = null;
                objCompanyByWorkgroup.CountryId = null;
                objCompanyByWorkgroup.StateId = null;
                objCompanyByWorkgroup.CityId = null;
                objCompanyByWorkgroup.Zip = null;
                objCompanyByWorkgroup.Telephone = null;
                objCompanyByWorkgroup.Email = null;
                objCompanyByWorkgroup.Moblie = null;
            }
            if (IsMoasApply == true)
            {
                foreach (DataListItem dtCA in dtCompanyAdmin.Items)
                {
                    if (((CheckBox)dtCA.FindControl("chkCompanyAdmins")).Checked == true)
                    {
                        HiddenField hfCA = dtCA.FindControl("hdnCompanyAdmins") as HiddenField;
                        TextBox txtApproverPriority = dtCA.FindControl("txtApproverPriority") as TextBox;
                        MOASUserIds.Add(hfCA.Value + "|" + txtApproverPriority.Text);
                    }
                }
                if (MOASUserIds.Count > 0)
                    objCompanyByWorkgroup.MOASUserIDs = string.Join(",", MOASUserIds.ToArray());
                else
                    objCompanyByWorkgroup.MOASUserIDs = null;
            }
            else
                objCompanyByWorkgroup.MOASUserIDs = null;

            #endregion

            #region Third Party Supplier Payment Option
            foreach (DataListItem dtpayoption in dtThirdPartySupplierPaymentOptions.Items)
            {
                if (((CheckBox)dtpayoption.FindControl("chkPaymentOptions")).Checked == true)
                {
                    if (selectedthirdpartysupplierpayment == "")
                        selectedthirdpartysupplierpayment = ((HiddenField)dtpayoption.FindControl("hdnPaymentOption")).Value;
                    else
                        selectedthirdpartysupplierpayment = selectedthirdpartysupplierpayment + "," + ((HiddenField)dtpayoption.FindControl("hdnPaymentOption")).Value;
                }
            }

            objCompanyByWorkgroup.ThirdPartySupplierPaymentOption = selectedthirdpartysupplierpayment;
            objMenuRepo.SubmitChanges();
            #endregion

            #region Checkout infomration
            foreach (DataListItem dtcheckout in dtCheckOutInfo.Items)
            {
                if (((CheckBox)dtcheckout.FindControl("chkCheckOutInfo")).Checked == true)
                {

                    if (selectedcheckoutinformation == "")
                    {
                        selectedcheckoutinformation = ((HiddenField)dtcheckout.FindControl("hdnChecoutInfo")).Value;
                    }
                    else
                    {
                        selectedcheckoutinformation = selectedcheckoutinformation + "," + ((HiddenField)dtcheckout.FindControl("hdnChecoutInfo")).Value;
                    }
                }
            }
            objCompanyByWorkgroup.Checkoutinformation = selectedcheckoutinformation;
            #endregion

            #region Add to existing employee
            foreach (CompanyEmployee employee in lstCompanyemployee)
            {
                List<string> lstSelectedStoreOption = new List<string>(selecteduserstoreoption.Split(','));
                string BulkOrderID = new LookupRepository().GetIdByLookupNameNLookUpCode("Bulk Order", "UserStoreOptions").ToString();
                string NameToEngraveID = new LookupRepository().GetIdByLookupNameNLookUpCode("Name to Engrave", "UserStoreOptions").ToString();
                for (int i = 0; i < lstSelectedStoreOption.Count; i++)
                {
                    if ((BulkOrderID == lstSelectedStoreOption[i] || NameToEngraveID == lstSelectedStoreOption[i]) && employee.isCompanyAdmin == false)//this is for not apply bulk order and name to engrave option to CompanyEmployee
                    {
                        lstSelectedStoreOption.RemoveAt(i);
                        i--;
                    }
                }

                UserInformation objUserInformation = new UserInformationRepository().GetById(employee.UserInfoID);
                List<string> lstSelectedPaymentOptions = new List<string>((objUserInformation.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee)) ? selectedthirdpartysupplierpayment.Split(',') : (chkIsPaymentOptionByHireDate.Checked == true && employee.HirerdDate != null) ? employee.HirerdDate < Convert.ToDateTime(txtHireDate.Text.Trim()) ? selectedworldlinkpaymentBeforeHireDate.Split(',') : selectedworldlinkpaymentAfterHireDate.Split(',') : selectedworldlinkpayment.Split(','));
                List<string> employeeMOASUserIds = MOASUserIds;
                string MOASID = new LookupRepository().GetIdByLookupNameNLookUpCode("MOAS", "WLS Payment Option").ToString();
                for (int i = 0; i < lstSelectedPaymentOptions.Count; i++)
                {
                    if (MOASID == lstSelectedPaymentOptions[i] && employee.isCompanyAdmin == true && employee.IsMOASApprover == false)//this is for not apply MOAS option to CompanyAdmin
                    {
                        lstSelectedPaymentOptions.RemoveAt(i);
                        employeeMOASUserIds = null;
                    }
                }
                
                CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetById(employee.CompanyEmployeeID);
                objCompanyEmployee.Userstoreoption = string.Join(",", lstSelectedStoreOption.ToArray());
                objCompanyEmployee.Paymentoption = string.Join(",", lstSelectedPaymentOptions.ToArray());
                objCompanyEmployee.MOASEmailAddresses = employeeMOASUserIds!=null && employeeMOASUserIds.Count>0 ?string.Join(",", employeeMOASUserIds.ToArray()):null;
                objCompanyEmployee.Checkoutinformation = selectedcheckoutinformation;
            }
            objCompanyEmployeeRepository.SubmitChanges();
            #endregion

            if (chkIsHiredDateRequire.Checked)
            {
                objCompanyByWorkgroup.IsHiredDateTicked = true;
            }
            else
            {
                objCompanyByWorkgroup.IsHiredDateTicked = false;
            }

            objMenuRepo.SubmitChanges();

            Response.Redirect("ViewGlobalMenuSetting.aspx?ID=" + this.CompanyStoreId);

        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }

    }

    protected void lnkbtnApplyAndAddition_Click(object sender, EventArgs e)
    {
        try
        {
            CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
            List<CompanyEmployee> lstCompanyemployee = objCompanyEmployeeRepository.GetByWorkgroupID(this.WorkGroupId, new CompanyStoreRepository().GetById(this.CompanyStoreId).CompanyID);
            List<string> MOASUserIds = new List<string>();
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

            #region User Store Option
            foreach (DataListItem dtuserstore in dtUserStoreFront.Items)
            {
                if (((CheckBox)dtuserstore.FindControl("chkUserStoreFront")).Checked == true)
                {

                    if (selecteduserstoreoption == "")
                    {
                        selecteduserstoreoption = ((HiddenField)dtuserstore.FindControl("hdnStoreFront")).Value;

                    }
                    else
                    {
                        selecteduserstoreoption = selecteduserstoreoption + "," + ((HiddenField)dtuserstore.FindControl("hdnStoreFront")).Value;

                    }

                }
            }

            objCompanyByWorkgroup.Userstoreoption = selecteduserstoreoption;
            objMenuRepo.SubmitChanges();

            #endregion

            #region Payment Option
            Boolean IsMoasApply = false;
            Boolean IsEmployeePayrollDuductApply = false;
            string selectedworldlinkpaymentBeforeHireDate = "";
            string selectedworldlinkpaymentAfterHireDate = "";
            foreach (DataListItem dtpayoption in dtPaymentOptions.Items)
            {
                if (((CheckBox)dtpayoption.FindControl("chkPaymentOptions")).Checked == true)
                {
                    if (selectedworldlinkpayment == "")
                        selectedworldlinkpayment = ((HiddenField)dtpayoption.FindControl("hdnPaymentOption")).Value;
                    else
                        selectedworldlinkpayment = selectedworldlinkpayment + "," + ((HiddenField)dtpayoption.FindControl("hdnPaymentOption")).Value;

                    if (((Label)dtpayoption.FindControl("lblPaymentOptions")).Text == "Employee Payroll Deduct")
                        IsEmployeePayrollDuductApply = true;
                    if (((Label)dtpayoption.FindControl("lblPaymentOptions")).Text == "MOAS")
                        IsMoasApply = true;
                }
            }

            if (chkIsPaymentOptionByHireDate.Checked == true)
            {
                foreach (DataListItem dtpayoption in dtPaymentOptionsBeforeHireDate.Items)
                {
                    if (((CheckBox)dtpayoption.FindControl("chkPaymentOptionsBeforeHireDate")).Checked == true)
                    {
                        if (selectedworldlinkpaymentBeforeHireDate == "")
                            selectedworldlinkpaymentBeforeHireDate = ((HiddenField)dtpayoption.FindControl("hdnPaymentOptionsBeforeHireDate")).Value;
                        else
                            selectedworldlinkpaymentBeforeHireDate = selectedworldlinkpaymentBeforeHireDate + "," + ((HiddenField)dtpayoption.FindControl("hdnPaymentOptionsBeforeHireDate")).Value;

                        if (((Label)dtpayoption.FindControl("lblPaymentOptionsBeforeHireDate")).Text == "Employee Payroll Deduct")
                            IsEmployeePayrollDuductApply = true;
                        if (((Label)dtpayoption.FindControl("lblPaymentOptionsBeforeHireDate")).Text == "MOAS")
                            IsMoasApply = true;
                    }
                }
                foreach (DataListItem dtpayoption in dtPaymentOptionsAfterHireDate.Items)
                {
                    if (((CheckBox)dtpayoption.FindControl("chkPaymentOptionsAfterHireDate")).Checked == true)
                    {
                        if (selectedworldlinkpaymentAfterHireDate == "")
                            selectedworldlinkpaymentAfterHireDate = ((HiddenField)dtpayoption.FindControl("hdnPaymentOptionsAfterHireDate")).Value;
                        else
                            selectedworldlinkpaymentAfterHireDate = selectedworldlinkpaymentAfterHireDate + "," + ((HiddenField)dtpayoption.FindControl("hdnPaymentOptionsAfterHireDate")).Value;

                        if (((Label)dtpayoption.FindControl("lblPaymentOptionsAfterHireDate")).Text == "Employee Payroll Deduct")
                            IsEmployeePayrollDuductApply = true;
                        if (((Label)dtpayoption.FindControl("lblPaymentOptionsAfterHireDate")).Text == "MOAS")
                            IsMoasApply = true;
                    }
                }
            } 

            objCompanyByWorkgroup.Paymentoption = selectedworldlinkpayment;
            objCompanyByWorkgroup.IsPaymentOptionOnHireDate = chkIsPaymentOptionByHireDate.Checked == true ? true : false;
            objCompanyByWorkgroup.PaymentOptionAfterHireDate = selectedworldlinkpaymentAfterHireDate;
            objCompanyByWorkgroup.PaymentOptionBeforeHireDate = selectedworldlinkpaymentBeforeHireDate;
            if (txtHireDate.Text.Trim() != string.Empty)
                objCompanyByWorkgroup.PaymentOptionHireDate = Convert.ToDateTime(txtHireDate.Text.Trim());
            else
                objCompanyByWorkgroup.PaymentOptionHireDate = null;
            objMenuRepo.SubmitChanges();

            if (IsEmployeePayrollDuductApply == true)
            {
                objCompanyByWorkgroup.IsGlobalBilling = true;
                objCompanyByWorkgroup.CompanyName = txtVendorCompany.Text;
                objCompanyByWorkgroup.FirstName = txtVendorFirstName.Text;
                objCompanyByWorkgroup.LastName = txtLastName.Text;
                objCompanyByWorkgroup.Address = txtVendorAddress.Text;
                objCompanyByWorkgroup.CountryId = Convert.ToInt64(DrpBillingCountry.SelectedItem.Value);
                objCompanyByWorkgroup.StateId = Convert.ToInt64(DrpBillingState.SelectedItem.Value);
                objCompanyByWorkgroup.CityId = Convert.ToInt64(DrpBillingCity.SelectedItem.Value);
                objCompanyByWorkgroup.Zip = txtVendorZip.Text;
                objCompanyByWorkgroup.Telephone = txtVendorTelephone.Text;
                objCompanyByWorkgroup.Email = txtVendorEmail.Text;
                objCompanyByWorkgroup.Moblie = TxtMobile.Text;
            }
            else
            {
                objCompanyByWorkgroup.IsGlobalBilling = false;
                objCompanyByWorkgroup.CompanyName = null;
                objCompanyByWorkgroup.FirstName = null;
                objCompanyByWorkgroup.LastName = null;
                objCompanyByWorkgroup.Address = null;
                objCompanyByWorkgroup.CountryId = null;
                objCompanyByWorkgroup.StateId = null;
                objCompanyByWorkgroup.CityId = null;
                objCompanyByWorkgroup.Zip = null;
                objCompanyByWorkgroup.Telephone = null;
                objCompanyByWorkgroup.Email = null;
                objCompanyByWorkgroup.Moblie = null;
            }
            if (IsMoasApply == true)
            {
                foreach (DataListItem dtCA in dtCompanyAdmin.Items)
                {
                    if (((CheckBox)dtCA.FindControl("chkCompanyAdmins")).Checked == true)
                    {
                        HiddenField hfCA = dtCA.FindControl("hdnCompanyAdmins") as HiddenField;
                        TextBox txtApproverPriority = dtCA.FindControl("txtApproverPriority") as TextBox;
                        MOASUserIds.Add(hfCA.Value + "|" + txtApproverPriority.Text);
                    }
                }
                if (MOASUserIds.Count > 0)
                    objCompanyByWorkgroup.MOASUserIDs = string.Join(",", MOASUserIds.ToArray());
                else
                    objCompanyByWorkgroup.MOASUserIDs = null;
            }
            else
                objCompanyByWorkgroup.MOASUserIDs = null;

            #endregion

            #region Third Party Supplier Payment Option
            foreach (DataListItem dtpayoption in dtThirdPartySupplierPaymentOptions.Items)
            {
                if (((CheckBox)dtpayoption.FindControl("chkPaymentOptions")).Checked == true)
                {
                    if (selectedthirdpartysupplierpayment == "")
                        selectedthirdpartysupplierpayment = ((HiddenField)dtpayoption.FindControl("hdnPaymentOption")).Value;
                    else
                        selectedthirdpartysupplierpayment = selectedthirdpartysupplierpayment + "," + ((HiddenField)dtpayoption.FindControl("hdnPaymentOption")).Value;
                }
            }

            objCompanyByWorkgroup.ThirdPartySupplierPaymentOption = selectedthirdpartysupplierpayment;
            objMenuRepo.SubmitChanges();
            #endregion

            #region Checkout infomration
            foreach (DataListItem dtcheckout in dtCheckOutInfo.Items)
            {
                if (((CheckBox)dtcheckout.FindControl("chkCheckOutInfo")).Checked == true)
                {

                    if (selectedcheckoutinformation == "")
                    {
                        selectedcheckoutinformation = ((HiddenField)dtcheckout.FindControl("hdnChecoutInfo")).Value;
                    }
                    else
                    {
                        selectedcheckoutinformation = selectedcheckoutinformation + "," + ((HiddenField)dtcheckout.FindControl("hdnChecoutInfo")).Value;
                    }
                }
            }
            objCompanyByWorkgroup.Checkoutinformation = selectedcheckoutinformation;
            #endregion

            #region Addition to existing employee
            foreach (CompanyEmployee employee in lstCompanyemployee)
            {
                //Store option
                List<string> lstCompanyEmployeeStoreOption = new List<string>((employee.Userstoreoption != null ? employee.Userstoreoption : "").Split(','));
                string[] strSelectedStoreOption = selecteduserstoreoption.Split(',');
                string BulkOrderID = new LookupRepository().GetIdByLookupNameNLookUpCode("Bulk Order", "UserStoreOptions").ToString();
                string NameToEngraveID = new LookupRepository().GetIdByLookupNameNLookUpCode("Name to Engrave", "UserStoreOptions").ToString();
                for (int i = 0; i < strSelectedStoreOption.Length; i++)
                {
                    Boolean isExit = false;
                    if ((BulkOrderID == strSelectedStoreOption[i] || NameToEngraveID == strSelectedStoreOption[i]) && employee.isCompanyAdmin == false)//this is for not apply bulk order or Name to Engrave option to CompanyEmployee
                    {
                    }
                    else
                    {
                        for (int j = 0; j < lstCompanyEmployeeStoreOption.Count; j++)
                        {
                            if (lstCompanyEmployeeStoreOption[j] == strSelectedStoreOption[i])
                                isExit = true;
                        }
                        if (isExit == false)
                            lstCompanyEmployeeStoreOption.Add(strSelectedStoreOption[i]);
                    }
                }

                string strSOption = string.Join(",", lstCompanyEmployeeStoreOption.ToArray());
                strSOption = strSOption.StartsWith(",") ? strSOption.Substring(1) : strSOption;
                strSOption = strSOption.EndsWith(",") ? strSOption.Substring(0, strSOption.Length - 1) : strSOption;

                //payment options
                UserInformation objUserInformation = new UserInformationRepository().GetById(employee.UserInfoID);
                List<string> lstCompanyEmployeepaymentoption = new List<string>((employee.Paymentoption != null ? employee.Paymentoption : "").Split(','));
                string[] strSelectedPaymentOptions = (objUserInformation.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee)) ? selectedthirdpartysupplierpayment.Split(',') : (chkIsPaymentOptionByHireDate.Checked == true && employee.HirerdDate != null) ? employee.HirerdDate < Convert.ToDateTime(txtHireDate.Text.Trim()) ? selectedworldlinkpaymentBeforeHireDate.Split(',') : selectedworldlinkpaymentAfterHireDate.Split(',') : selectedworldlinkpayment.Split(',');
                List<string> employeeMOASUserIds = MOASUserIds;
                string MOASID = new LookupRepository().GetIdByLookupNameNLookUpCode("MOAS", "WLS Payment Option").ToString();
                for (int i = 0; i < strSelectedPaymentOptions.Length; i++)
                {
                    Boolean isExit = false;
                    if (MOASID == strSelectedPaymentOptions[i] && employee.isCompanyAdmin == true && employee.IsMOASApprover == false)//this is for not apply MOAS option to CompanyAdmin
                    {
                        employeeMOASUserIds = null;
                    }
                    else
                    {
                        for (int j = 0; j < lstCompanyEmployeepaymentoption.Count; j++)
                        {
                            if (lstCompanyEmployeepaymentoption[j] == strSelectedPaymentOptions[i])
                                isExit = true;
                        }
                        if (isExit == false)
                            lstCompanyEmployeepaymentoption.Add(strSelectedPaymentOptions[i]);
                    }
                }

                string strPOption = string.Join(",", lstCompanyEmployeepaymentoption.ToArray());
                strPOption = strPOption.StartsWith(",") ? strPOption.Substring(1) : strPOption;
                strPOption = strPOption.EndsWith(",") ? strPOption.Substring(0, strPOption.Length - 1) : strPOption;

                //Checkout info
                List<string> lstCompanyEmployeeCheckoutInfo = new List<string>((employee.Checkoutinformation != null ? employee.Checkoutinformation : "").Split(','));
                string[] strSelectedCheckoutInfo = selectedcheckoutinformation.Split(',');
                for (int i = 0; i < strSelectedCheckoutInfo.Length; i++)
                {
                    Boolean isExit = false;
                    if (lstCompanyEmployeeCheckoutInfo != null)
                    {
                        for (int j = 0; j < lstCompanyEmployeeCheckoutInfo.Count; j++)
                        {
                            if (lstCompanyEmployeeCheckoutInfo[j] == strSelectedCheckoutInfo[i])
                                isExit = true;
                        }
                    }
                    if (isExit == false)
                        lstCompanyEmployeeCheckoutInfo.Add(strSelectedCheckoutInfo[i]);
                }
                string strCOption = string.Join(",", lstCompanyEmployeeCheckoutInfo.ToArray());
                strCOption = strCOption.StartsWith(",") ? strCOption.Substring(1) : strCOption;
                strCOption = strCOption.EndsWith(",") ? strCOption.Substring(0, strCOption.Length - 1) : strCOption;


                CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetById(employee.CompanyEmployeeID);
                objCompanyEmployee.Checkoutinformation = strCOption;
                objCompanyEmployee.Paymentoption = strPOption;
                objCompanyEmployee.MOASEmailAddresses = employeeMOASUserIds != null && employeeMOASUserIds.Count > 0 ? string.Join(",", employeeMOASUserIds.ToArray()) : null;
                objCompanyEmployee.Userstoreoption = strSOption;
            }
            objCompanyEmployeeRepository.SubmitChanges();
            #endregion

            if (chkIsHiredDateRequire.Checked)
            {
                objCompanyByWorkgroup.IsHiredDateTicked = true;
            }
            else
            {
                objCompanyByWorkgroup.IsHiredDateTicked = false;
            }

            objMenuRepo.SubmitChanges();

            Response.Redirect("ViewGlobalMenuSetting.aspx?ID=" + this.CompanyStoreId);
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }
    }
    #endregion

    #region Methods
    
    void DisplayData(object sender, EventArgs e)
    {
        if (this.WorkGroupId != 0)
        {
            GlobalMenuSetting objValuebyworkgroup = new GlobalMenuSettingRepository().GetById(this.WorkGroupId, this.CompanyStoreId);
            lblWorkGroup.Text = new LookupRepository().GetById(this.WorkGroupId).sLookupName.ToString();
            if (objValuebyworkgroup == null)
            {
                return;

            }

            //Check If Hired Date Option
            if  (Convert.ToBoolean(objValuebyworkgroup.IsHiredDateTicked))
            {
                spanIsHiredRequire.Attributes.Add("class", "custom-checkbox_checked");
                chkIsHiredDateRequire.Checked = true;
            }
            else
            {
                chkIsHiredDateRequire.Checked = false;
            }
            //end


            //User Store
            if (objValuebyworkgroup.Userstoreoption != null)
            {
                string[] Ids = objValuebyworkgroup.Userstoreoption.Split(',');
                foreach (DataListItem dt in dtUserStoreFront.Items)
                {
                    CheckBox chk = dt.FindControl("chkUserStoreFront") as CheckBox;
                    HiddenField lblId = dt.FindControl("hdnStoreFront") as HiddenField;
                    HtmlGenericControl dvChk = dt.FindControl("storespan") as HtmlGenericControl;

                    foreach (string i in Ids)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }
                    }
                }
            }

            //Payment Option
            if (Convert.ToBoolean(objValuebyworkgroup.IsPaymentOptionOnHireDate))
            {
                //This is for hiredate payment option
                spnIsPaymentOptionByHireDate.Attributes.Add("class", "custom-checkbox_checked alignleft");
                chkIsPaymentOptionByHireDate.Checked = true;
                txtHireDate.Text = Convert.ToDateTime(objValuebyworkgroup.PaymentOptionHireDate).ToString("MM/dd/yyyy");
                dvHireDatePaymentOption.Style.Add("display", "block");

                //this is for after hiredate payment option
                if (objValuebyworkgroup.PaymentOptionAfterHireDate != null)
                {
                    string[] PaymentIds = objValuebyworkgroup.PaymentOptionAfterHireDate.Split(',');
                    foreach (DataListItem dtPaymentOp in dtPaymentOptionsAfterHireDate.Items)
                    {
                        CheckBox chk = dtPaymentOp.FindControl("chkPaymentOptionsAfterHireDate") as CheckBox;
                        HiddenField lblId = dtPaymentOp.FindControl("hdnPaymentOptionsAfterHireDate") as HiddenField;
                        Label lblName = dtPaymentOp.FindControl("lblPaymentOptionsAfterHireDate") as Label;
                        HtmlGenericControl dvChk = dtPaymentOp.FindControl("spnPaymentOptionsAfterHireDate") as HtmlGenericControl;

                        foreach (string i in PaymentIds)
                        {
                            if (i.Equals(lblId.Value))
                            {
                                chk.Checked = true;
                                dvChk.Attributes.Add("class", "custom-checkbox_checked");

                                /* Uncomment this line of Code*/
                                if (lblName.Text == "Employee Payroll Deduct")
                                {
                                    SetEmployeePayrollDeductBillingData(objValuebyworkgroup);
                                }
                                //start add by mayur for moas options
                                if (lblName.Text.Contains("MOAS"))
                                {
                                    SetMOASData(objValuebyworkgroup);
                                }
                                //end
                                break;
                            }
                        }
                    }
                }
                //this is for before hiredate payment option
                if (objValuebyworkgroup.PaymentOptionBeforeHireDate != null)
                {
                    string[] PaymentIds = objValuebyworkgroup.PaymentOptionBeforeHireDate.Split(',');
                    foreach (DataListItem dtPaymentOp in dtPaymentOptionsBeforeHireDate.Items)
                    {
                        CheckBox chk = dtPaymentOp.FindControl("chkPaymentOptionsBeforeHireDate") as CheckBox;
                        HiddenField lblId = dtPaymentOp.FindControl("hdnPaymentOptionsBeforeHireDate") as HiddenField;
                        Label lblName = dtPaymentOp.FindControl("lblPaymentOptionsBeforeHireDate") as Label;
                        HtmlGenericControl dvChk = dtPaymentOp.FindControl("spnPaymentOptionsBeforeHireDate") as HtmlGenericControl;

                        foreach (string i in PaymentIds)
                        {
                            if (i.Equals(lblId.Value))
                            {
                                chk.Checked = true;
                                dvChk.Attributes.Add("class", "custom-checkbox_checked");

                                /* Uncomment this line of Code*/
                                if (lblName.Text == "Employee Payroll Deduct")
                                {
                                    SetEmployeePayrollDeductBillingData(objValuebyworkgroup);
                                }
                                //start add by mayur for moas options
                                if (lblName.Text.Contains("MOAS"))
                                {
                                    SetMOASData(objValuebyworkgroup);
                                }
                                //end
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                //This is for normal payment option
                spnIsPaymentOptionByHireDate.Attributes.Add("class", "custom-checkbox alignleft");
                chkIsPaymentOptionByHireDate.Checked = false;
                txtHireDate.Text = string.Empty;
                dvHireDatePaymentOption.Style.Add("display", "none");
            }

            if (objValuebyworkgroup.Paymentoption != null)
            {
                string[] PaymentIds = objValuebyworkgroup.Paymentoption.Split(',');
                foreach (DataListItem dtPaymentOp in dtPaymentOptions.Items)
                {
                    CheckBox chk = dtPaymentOp.FindControl("chkPaymentOptions") as CheckBox;
                    HiddenField lblId = dtPaymentOp.FindControl("hdnPaymentOption") as HiddenField;
                    Label lblName = dtPaymentOp.FindControl("lblPaymentOptions") as Label;
                    HtmlGenericControl dvChk = dtPaymentOp.FindControl("paymentspan") as HtmlGenericControl;

                    foreach (string i in PaymentIds)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");

                            /* Uncomment this line of Code*/
                            if (lblName.Text == "Employee Payroll Deduct")
                            {
                                SetEmployeePayrollDeductBillingData(objValuebyworkgroup);
                            }
                            //start add by mayur for moas options
                            if (lblName.Text.Contains("MOAS"))
                            {
                                SetMOASData(objValuebyworkgroup);
                            }
                            //end
                            break;
                        }
                    }
                }
            }
            

            //Third party supplier Payment Option
            if (objValuebyworkgroup.ThirdPartySupplierPaymentOption != null)
            {
                string[] PaymentIds = objValuebyworkgroup.ThirdPartySupplierPaymentOption.Split(',');
                foreach (DataListItem dtPaymentOp in dtThirdPartySupplierPaymentOptions.Items)
                {
                    CheckBox chk = dtPaymentOp.FindControl("chkPaymentOptions") as CheckBox;
                    HiddenField lblId = dtPaymentOp.FindControl("hdnPaymentOption") as HiddenField;
                    HtmlGenericControl dvChk = dtPaymentOp.FindControl("paymentspan") as HtmlGenericControl;

                    foreach (string i in PaymentIds)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }
                    }
                }
            }

            //Checkout Info
            if (objValuebyworkgroup.Checkoutinformation != null)
            {
                string[] cheoutinfoIds = objValuebyworkgroup.Checkoutinformation.Split(',');
                foreach (DataListItem dtcheckout in dtCheckOutInfo.Items)
                {
                    CheckBox chk = dtcheckout.FindControl("chkCheckOutInfo") as CheckBox;
                    HiddenField lblId = dtcheckout.FindControl("hdnChecoutInfo") as HiddenField;
                    HtmlGenericControl dvChk = dtcheckout.FindControl("checkoutspan") as HtmlGenericControl;

                    foreach (string i in cheoutinfoIds)
                    {
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
    
    private void bindShoppingSetting()
    {
        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "UserStoreOptions";
        DataSet dsEU = sEU.LookUp(sEUBE);
        dtUserStoreFront.DataSource = dsEU;
        dtUserStoreFront.DataBind();

        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "WLS Payment Option";
        DataSet dsPO = sEU.LookUp(sEUBE);

        DataRow[] drReplacementUniforms = dsPO.Tables[0].Select("sLookupName = 'Replacement Uniforms'");
        if (drReplacementUniforms != null & drReplacementUniforms.Length > 0)
            dsPO.Tables[0].Rows.Remove(drReplacementUniforms[0]);

        dtPaymentOptions.DataSource = dsPO;
        dtPaymentOptions.DataBind();

        dtPaymentOptionsAfterHireDate.DataSource = dsPO;
        dtPaymentOptionsAfterHireDate.DataBind();

        dtPaymentOptionsBeforeHireDate.DataSource = dsPO;
        dtPaymentOptionsBeforeHireDate.DataBind();

        //this is for binding company admin based on company id
        List<UserInformation> lstCA = new CompanyEmployeeRepository().GetCAByCompanyID(new CompanyStoreRepository().GetById(this.CompanyStoreId).CompanyID);
        dtCompanyAdmin.DataSource = lstCA;
        dtCompanyAdmin.DataBind();

        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "Required Checkout Information";
        DataSet dsCE = sEU.LookUp(sEUBE);
        dtCheckOutInfo.DataSource = dsCE;
        dtCheckOutInfo.DataBind();

        //This payment options is for third party supplier employee add by mayur on 9-may-2012
        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "WLS Payment Option";
        DataSet dsTPSPO = sEU.LookUp(sEUBE);

        foreach (DataRow dr in dsTPSPO.Tables[0].Rows)
        {
            if (dr["sLookupName"].ToString().Contains("Employee Payroll Deduct") || dr["sLookupName"].ToString().Contains("Paid By Corporate") || dr["sLookupName"].ToString().Contains("MOAS"))
                dr.Delete();
        }
        dsTPSPO.Tables[0].AcceptChanges();

        dtThirdPartySupplierPaymentOptions.DataSource = dsTPSPO;
        dtThirdPartySupplierPaymentOptions.DataBind();

    }

    private void SetEmployeePayrollDeductBillingData(GlobalMenuSetting objValuebyworkgroup)
    {
        dvThirdParty.Attributes.Add("style", "display:inline");
        txtVendorCompany.Text = objValuebyworkgroup.CompanyName;
        txtVendorFirstName.Text = objValuebyworkgroup.FirstName;
        txtLastName.Text = objValuebyworkgroup.LastName;
        txtVendorAddress.Text = objValuebyworkgroup.Address;
        DrpBillingCountry.SelectedValue = objValuebyworkgroup.CountryId.ToString();
        DrpBillingCountry_SelectedIndexChanged(null, null);
        DrpBillingState.SelectedValue = objValuebyworkgroup.StateId.ToString();
        DrpBillingState_SelectedIndexChanged(null, null);
        DrpBillingCity.SelectedValue = objValuebyworkgroup.CityId.ToString();
        txtVendorZip.Text = objValuebyworkgroup.Zip;
        txtVendorTelephone.Text = objValuebyworkgroup.Telephone;
        txtVendorEmail.Text = objValuebyworkgroup.Email;
        TxtMobile.Text = objValuebyworkgroup.Moblie;
    }

    private void SetMOASData(GlobalMenuSetting objValuebyworkgroup)
    {
        dvMOASEmail.Style.Add("display", "block");
        dvReqChkoutInfo.Style.Add("display", "block");
        if (!string.IsNullOrEmpty(objValuebyworkgroup.MOASUserIDs))
        {
            string[] MOASUsers = objValuebyworkgroup.MOASUserIDs.Split(',');

            List<CompanyEmployeeRepository.MOASUserWithPriority> objListMOASUserWithPriority = new List<CompanyEmployeeRepository.MOASUserWithPriority>();
            for (int j = 0; j < MOASUsers.Length; j++)
            {
                try
                {
                    string[] MOASUsersWithPriority = MOASUsers[j].Split('|');
                    CompanyEmployeeRepository.MOASUserWithPriority objMOASUserWithPriority = new CompanyEmployeeRepository.MOASUserWithPriority();
                    objMOASUserWithPriority.UserInfoID = MOASUsersWithPriority[0];
                    objMOASUserWithPriority.Priority = MOASUsersWithPriority[1];
                    objListMOASUserWithPriority.Add(objMOASUserWithPriority);
                }
                catch { }
            }
            foreach (DataListItem dtCA in dtCompanyAdmin.Items)
            {
                CheckBox chkCA = dtCA.FindControl("chkCompanyAdmins") as CheckBox;
                HiddenField hfCA = dtCA.FindControl("hdnCompanyAdmins") as HiddenField;
                HtmlGenericControl dvChkCA = dtCA.FindControl("adminspan") as HtmlGenericControl;
                TextBox txtApproverPriority = dtCA.FindControl("txtApproverPriority") as TextBox;
                foreach (CompanyEmployeeRepository.MOASUserWithPriority obj in objListMOASUserWithPriority)
                {
                    if (obj.UserInfoID.Equals(hfCA.Value))
                    {
                        chkCA.Checked = true;
                        dvChkCA.Attributes.Add("class", "custom-checkbox_checked");
                        txtApproverPriority.Style.Add("display", "block");
                        txtApproverPriority.Text = obj.Priority;
                        break;
                    }
                }
            }
            //change end on 3-feb-2012 by mayur for setting priority
        }
    }
    #endregion
}