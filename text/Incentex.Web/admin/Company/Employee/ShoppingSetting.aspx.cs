using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

    String selecteduserstoreoption = "";
    String selectedworldlinkpayment = "";
    String selectedcheckoutinformation = "";

    List<String> MOASUserIds = new List<String>();

    Common objcomm = new Common();

    Int64 CompanyId
    {
        get
        {
            if (ViewState["CompanyId"] == null)
            {
                ViewState["CompanyId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyId"]);
        }
        set
        {
            ViewState["CompanyId"] = value;
        }
    }

    Int64 EmployeeId
    {
        get
        {
            if (ViewState["EmployeeId"] == null)
            {
                ViewState["EmployeeId"] = 0;
            }
            return Convert.ToInt64(ViewState["EmployeeId"]);
        }
        set
        {
            ViewState["EmployeeId"] = value;
        }
    }

    #endregion

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Employees";
            base.ParentMenuID = 11;

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
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.EmployeeId = Convert.ToInt64(Request.QueryString.Get("SubId"));

                if (this.EmployeeId == 0)
                {
                    Response.Redirect("~/admin/Company/Employee/BasicInformation.aspx?Id=" + this.CompanyId);
                }

                menucontrol.PopulateMenu(2, 5, this.CompanyId, this.EmployeeId, true);

                ((Label)Master.FindControl("lblPageHeading")).Text = "Storefront Settings";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Company/Employee/ViewEmployee.aspx?id=" + this.CompanyId;

                //set Search Page Return URL
                if (IncentexGlobal.SearchPageReturnURL != null)
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.SearchPageReturnURL;
                }
            }
            else
            {
                Response.Redirect("~/admin/Company/Employee/ViewEmployee.aspx");
            }

            bindShoppingSetting();
            DisplayData();
            BindSupportTicketOwners();
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            CompanyEmployee objCompanyEmployee = new CompanyEmployee();
            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
            objCompanyEmployee = objEmpRepo.GetById(EmployeeId);

            //User Store Option
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

            objCompanyEmployee.Userstoreoption = selecteduserstoreoption;
            objEmpRepo.SubmitChanges();

            //Payment Option
            Boolean isMOASSelect = false;
            foreach (DataListItem dtpayoption in dtPaymentOptions.Items)
            {
                if (((CheckBox)dtpayoption.FindControl("chkPaymentOptions")).Checked == true)
                {
                    Label lbl = ((Label)dtpayoption.FindControl("lblPaymentOptions"));
                    if (lbl.Text.Contains("MOAS"))
                        isMOASSelect = true;

                    if (selectedworldlinkpayment == "")
                    {
                        selectedworldlinkpayment = ((HiddenField)dtpayoption.FindControl("hdnPaymentOption")).Value;

                    }
                    else
                    {
                        selectedworldlinkpayment = selectedworldlinkpayment + "," + ((HiddenField)dtpayoption.FindControl("hdnPaymentOption")).Value;

                    }

                }
            }

            objCompanyEmployee.Paymentoption = selectedworldlinkpayment;

            //MOAS users
            if (isMOASSelect)
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
                    objCompanyEmployee.MOASEmailAddresses = string.Join(",", MOASUserIds.ToArray());
                else
                    objCompanyEmployee.MOASEmailAddresses = null;
            }
            else
                objCompanyEmployee.MOASEmailAddresses = null;

            objEmpRepo.SubmitChanges();


            //Checkout infomration

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

            objCompanyEmployee.Checkoutinformation = selectedcheckoutinformation;
            if (chkThirdPartyBilling.Checked == true)
            {
                objCompanyEmployee.VendorCompany = txtVendorCompany.Text;
                objCompanyEmployee.VendorContact = txtVendorContact.Text;
                objCompanyEmployee.VendorAddress = txtVendorAddress.Text;
                objCompanyEmployee.VendorCity = txtVendorCity.Text;
                objCompanyEmployee.VendorState = txtVendorState.Text;
                objCompanyEmployee.VendorEmail = txtVendorEmail.Text;
                objCompanyEmployee.VendorTel = txtVendorTelephone.Text;
                objCompanyEmployee.VendorZip = txtVendorZip.Text;
            }
            else
            {
                objCompanyEmployee.VendorCompany = null;
                objCompanyEmployee.VendorContact = null;
                objCompanyEmployee.VendorAddress = null;
                objCompanyEmployee.VendorCity = null;
                objCompanyEmployee.VendorState = null;
                objCompanyEmployee.VendorEmail = null;
                objCompanyEmployee.VendorTel = null;
                objCompanyEmployee.VendorZip = null;
            }


            objEmpRepo.SubmitChanges();

            //Manage Email Start
            //Delete from CompanyEmpManageEmail
            CompanyEmpManageEmailRepository objCompanyEmpManageEmailRepo = new CompanyEmpManageEmailRepository();
            CompanyEmpManageEmail objCompanyEmpManageEmaildel = new CompanyEmpManageEmail();

            List<CompanyEmpManageEmail> lst = objCompanyEmpManageEmailRepo.GetEmailRightsByUserInfoID(objCompanyEmployee.UserInfoID);

            //foreach (CompanyEmpManageEmail l in lst)
            //{
            objCompanyEmpManageEmailRepo.DeleteAll(lst);
            //}

            objCompanyEmpManageEmailRepo.SubmitChanges();

            //Insert in CompanyEmpManageEmail
            foreach (DataListItem dt in dtManageEmail.Items)
            {
                if (((CheckBox)dt.FindControl("chkManageEmail")).Checked == true)
                {
                    CompanyEmpManageEmail objCompanyEmpManageEmailins = new CompanyEmpManageEmail();
                    objCompanyEmpManageEmailins.UserInfoID = objCompanyEmployee.UserInfoID;
                    objCompanyEmpManageEmailins.ManageEmailID = Convert.ToInt64(((HiddenField)dt.FindControl("hdnManageEmail")).Value);

                    objCompanyEmpManageEmailRepo.Insert(objCompanyEmpManageEmailins);
                }
            }
            objCompanyEmpManageEmailRepo.SubmitChanges();
            //Manage Email End

            StringBuilder SupportTicketOwners = new StringBuilder();

            foreach (DataListItem Item in dtSupportTicketOwners.Items)
            {
                CheckBox chkSupportTicketOwner = (CheckBox)Item.FindControl("chkSupportTicketOwner");
                HiddenField hdnSupportTicketOwner = (HiddenField)Item.FindControl("hdnSupportTicketOwner");
                if (chkSupportTicketOwner != null && hdnSupportTicketOwner != null && chkSupportTicketOwner.Checked)
                {
                    if (SupportTicketOwners.Length == 0)
                        SupportTicketOwners.Append(hdnSupportTicketOwner.Value);
                    else
                        SupportTicketOwners.Append("," + hdnSupportTicketOwner.Value);
                }
            }

            objCompanyEmployee.SupportTicketOwners = SupportTicketOwners.ToString();
            objEmpRepo.SubmitChanges();


            Response.Redirect("AdditionalWorkgroup.aspx?Id=" + this.CompanyId + "&SubId=" + this.EmployeeId);
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }
    }

    protected void dtSupportTicketOwners_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox chkSupportTicketOwner = (CheckBox)e.Item.FindControl("chkSupportTicketOwner");
            HiddenField hdnExisting = (HiddenField)e.Item.FindControl("hdnExisting");
            HtmlControl ownerspan = (HtmlControl)e.Item.FindControl("ownerspan");

            if (hdnExisting != null && chkSupportTicketOwner != null && ownerspan != null)
            {
                if (hdnExisting.Value == "1")
                {
                    chkSupportTicketOwner.Checked = true;
                    ownerspan.Attributes.Add("class", "custom-checkbox_checked");
                }
            }
        }
    }

    #endregion

    #region Methods

    void DisplayData()
    {
        CheckBox chk;
        HiddenField lblId;
        if (this.EmployeeId != 0)
        {
            UserInformationRepository objUIRepository = new UserInformationRepository();
            CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetById(this.EmployeeId);

            if (objCompanyEmployee == null)
                return;

            UserInformation objUserInfo = new UserInformationRepository().GetById(objCompanyEmployee.UserInfoID);
            lblUserFullName.Text = objUserInfo.FirstName + " " + objUserInfo.LastName;

            //User Store
            if (objCompanyEmployee.Userstoreoption != null)
            {
                string[] Ids = objCompanyEmployee.Userstoreoption.Split(',');
                foreach (DataListItem dt in dtUserStoreFront.Items)
                {
                    chk = dt.FindControl("chkUserStoreFront") as CheckBox;
                    lblId = dt.FindControl("hdnStoreFront") as HiddenField;
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
            if (objCompanyEmployee.Paymentoption != null)
            {
                string[] PaymentIds = objCompanyEmployee.Paymentoption.Split(',');
                foreach (DataListItem dtPaymentOp in dtPaymentOptions.Items)
                {
                    chk = dtPaymentOp.FindControl("chkPaymentOptions") as CheckBox;
                    Label lblName = dtPaymentOp.FindControl("lblPaymentOptions") as Label;
                    lblId = dtPaymentOp.FindControl("hdnPaymentOption") as HiddenField;
                    HtmlGenericControl dvChk = dtPaymentOp.FindControl("paymentspan") as HtmlGenericControl;

                    foreach (string i in PaymentIds)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            //start add by mayur for moas options
                            if (lblName.Text.Contains("MOAS"))
                            {
                                dvMOASEmail.Style.Add("display", "block");
                                dvReqChkoutInfo.Style.Add("display", "block");
                                if (!string.IsNullOrEmpty(objCompanyEmployee.MOASEmailAddresses))
                                {
                                    string[] MOASUsers = objCompanyEmployee.MOASEmailAddresses.Split(',');

                                    //change start on 3-feb-2012 by mayur for setting priority
                                    List<CompanyEmployeeRepository.MOASUserWithPriority> objListMOASUserWithPriority = new List<CompanyEmployeeRepository.MOASUserWithPriority>();
                                    for (int j = 0; j < MOASUsers.Count(); j++)
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
                            //end
                            break;
                        }
                    }
                }
            }

            //Checkout Info
            if (objCompanyEmployee.Checkoutinformation != null)
            {
                string[] cheoutinfoIds = objCompanyEmployee.Checkoutinformation.Split(',');
                foreach (DataListItem dtcheckout in dtCheckOutInfo.Items)
                {
                    chk = dtcheckout.FindControl("chkCheckOutInfo") as CheckBox;
                    lblId = dtcheckout.FindControl("hdnChecoutInfo") as HiddenField;
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
            //

            //Manage Email Start
            List<CompanyEmpManageEmail> lstCompanyEmpManageEmail = new CompanyEmpManageEmailRepository().GetEmailRightsByUserInfoID(objCompanyEmployee.UserInfoID);

            foreach (DataListItem dtM in dtManageEmail.Items)
            {
                chk = dtM.FindControl("chkManageEmail") as CheckBox;
                lblId = dtM.FindControl("hdnManageEmail") as HiddenField;
                HtmlGenericControl dvChk = dtM.FindControl("ManageEmailspan") as HtmlGenericControl;

                foreach (CompanyEmpManageEmail objMenu in lstCompanyEmpManageEmail)
                {

                    if (objMenu.ManageEmailID.ToString().Equals(lblId.Value))
                    {
                        chk.Checked = true;
                        dvChk.Attributes.Add("class", "custom-checkbox_checked");
                        break;
                    }

                }

            }

            //Manage Email End
            if (objCompanyEmployee.VendorCompany != null)
            {
                chkThirdPartyBilling.Checked = true;
                spanthirdpartybilling.Attributes.Add("class", "custom-checkbox_checked");
                dvThirdParty.Attributes.Add("style", "display:inline");
            }
            txtVendorCompany.Text = objCompanyEmployee.VendorCompany;
            txtVendorContact.Text = objCompanyEmployee.VendorContact;
            txtVendorAddress.Text = objCompanyEmployee.VendorAddress;
            txtVendorCity.Text = objCompanyEmployee.VendorCity;
            txtVendorState.Text = objCompanyEmployee.VendorState;
            txtVendorEmail.Text = objCompanyEmployee.VendorEmail;
            txtVendorTelephone.Text = objCompanyEmployee.VendorTel;
            txtVendorZip.Text = objCompanyEmployee.VendorZip;

        }

    }

    private void bindShoppingSetting()
    {
        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "UserStoreOptions";
        DataSet dsEU = sEU.LookUp(sEUBE);

        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "WLS Payment Option";
        DataSet dsPO = sEU.LookUp(sEUBE);

        DataRow[] drReplacementUniforms = dsPO.Tables[0].Select("sLookupName = 'Replacement Uniforms'");
        if (drReplacementUniforms != null & drReplacementUniforms.Length > 0)
            dsPO.Tables[0].Rows.Remove(drReplacementUniforms[0]);

        //Email Management System Start
        ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();
        dtManageEmail.DataSource = objManageEmailRepo.GetEmailControl();
        dtManageEmail.DataBind();

        //Email Management System End

        CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetById(this.EmployeeId);

        //this is for third party supplier employee not display other option
        if (new UserInformationRepository().GetById(objCompanyEmployee.UserInfoID).Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
        {
            foreach (DataRow dr in dsPO.Tables[0].Rows)
            {
                if (dr["sLookupName"].ToString().Contains("Employee Payroll Deduct") || dr["sLookupName"].ToString().Contains("Paid By Corporate") || dr["sLookupName"].ToString().Contains("MOAS"))
                    dr.Delete();
            }
            dsPO.Tables[0].AcceptChanges();
        }

        //start add by mayur rathod on 24-nov-2011 if CA and isMOASApprover is false then not display option for MOAS
        if (objCompanyEmployee.isCompanyAdmin && !objCompanyEmployee.IsMOASApprover)
        {
            foreach (DataRow dr in dsPO.Tables[0].Rows)
            {
                if (dr["sLookupName"].ToString().Contains("MOAS"))
                    dr.Delete();
            }
            dsPO.Tables[0].AcceptChanges();
        }
        else
        {
            //this is for transfer moas option to last index
            DataRow[] drnew = dsPO.Tables[0].Select(" sLookupName = 'MOAS'");
            if (drnew.Count() > 0 && drnew[0] != null)
            {
                dsPO.Tables[0].ImportRow(drnew[0]);
                dsPO.Tables[0].Rows.Remove(drnew[0]);
                dsPO.Tables[0].AcceptChanges();
            }

            if (!objCompanyEmployee.isCompanyAdmin)
            {
                foreach (DataRow dr in dsEU.Tables[0].Rows)
                {
                    if (dr["sLookupName"].ToString().Contains("Bulk Order") || dr["sLookupName"].ToString().Contains("Name to Engrave"))
                        dr.Delete();
                }
                dsEU.Tables[0].AcceptChanges();
            }
        }

        dtUserStoreFront.DataSource = dsEU;
        dtUserStoreFront.DataBind();

        dtPaymentOptions.DataSource = dsPO;
        dtPaymentOptions.DataBind();

        //this is for getting all the CA by companyid and same user can not become his own approver.
        CompanyEmployeeRepository objCompanyEmpRepos = new CompanyEmployeeRepository();
        
        List<UserInformation> lstCA = objCompanyEmpRepos.GetCAByCompanyID(this.CompanyId).ToList();
        dtCompanyAdmin.DataSource = lstCA;
        dtCompanyAdmin.DataBind();

        BindBillinginformation(new CompanyStoreRepository().GetByCompanyId(this.CompanyId).StoreID, objCompanyEmployee.WorkgroupID);

        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "Required Checkout Information";
        DataSet dsCE = sEU.LookUp(sEUBE);
        dtCheckOutInfo.DataSource = dsCE;
        dtCheckOutInfo.DataBind();
    }

    private void BindBillinginformation(Int64 StoreID, Int64 WorkgroupID)
    {
        try
        {
            MOASShoppingCartAddress objMOASShoppingCartAddress = new MOASShoppingCartAddressRepository().GetByStoreIDWorkgroupIDAndDepartmentID(StoreID, WorkgroupID);
            if (objMOASShoppingCartAddress != null)
            {
                lblBillingCompanyName.Text = objMOASShoppingCartAddress.BCompanyName;
                lblBillingFirstName.Text = objMOASShoppingCartAddress.FirstBName;
                lblBillingLastName.Text = objMOASShoppingCartAddress.LastBName;
                lblBillingAddress1.Text = objMOASShoppingCartAddress.BAddress1;
                lblBillingAddress2.Text = objMOASShoppingCartAddress.BAddress2;
                lblBillingCountry.Text = objMOASShoppingCartAddress.BCountryId != null ? new CountryRepository().GetById((long)objMOASShoppingCartAddress.BCountryId).sCountryName : "";
                lblBillingState.Text = objMOASShoppingCartAddress.BStateId != null ? new StateRepository().GetById((long)objMOASShoppingCartAddress.BStateId).sStatename : "";
                lblBillingCity.Text = objMOASShoppingCartAddress.BCityId != null ? new CityRepository().GetById((long)objMOASShoppingCartAddress.BCityId).sCityName : "";
                lblBillingZip.Text = objMOASShoppingCartAddress.BZipCode;
                lblBillingPhone.Text = objMOASShoppingCartAddress.BTelephone;
                lblBillingMobile.Text = objMOASShoppingCartAddress.BMobile;
                lblBillingEmail.Text = objMOASShoppingCartAddress.BEmail;
            }
            else
            {
                dvMOASBillingInfo.Visible = false;
                dvMOASBillingInfoError.Visible = true;
            }
        }
        catch { }
    }

    private void BindSupportTicketOwners()
    {
        ServiceTicketRepository objRepo = new ServiceTicketRepository();

        Int64 UserInfoID = objRepo.GetUserInfoIDByCompanyEmployeeID(this.EmployeeId);
        List<GetServiceTicketOwnersForCAResult> lstOwners = objRepo.GetServiceTicketOwnersForCA(UserInfoID).OrderBy(le => le.FirstName).ToList();

        //When the company employee is admin then count would be always greater then zero.
        if (lstOwners.Count > 0)
        {
            dtSupportTicketOwners.DataSource = lstOwners;
            dtSupportTicketOwners.DataBind();
        }
        //When compnay employee is not admin then hiding the support ticket owners section.
        else
        {
            tdSupportTicketOwners.Visible = false;
        }
    }

    #endregion
}
