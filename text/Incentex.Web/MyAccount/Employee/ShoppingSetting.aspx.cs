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
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;

public partial class admin_Company_Employee_ShoppingSetting : PageBase
{
    #region Data Members
    LookupDA sEU = new LookupDA();
    LookupBE sEUBE = new LookupBE();
    string selecteduserstoreoption = "";
    string selectedworldlinkpayment = "";
    string selectedcheckoutinformation = "";
    List<string> MOASUserIds = new List<string>();
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
            if (Request.QueryString.Count > 0)
            {
                CheckLogin();

                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.EmployeeId = Convert.ToInt64(Request.QueryString.Get("SubId"));
                IncentexGlobal.ManageID = 7;
                
                if (this.EmployeeId == 0)
                {
                    Response.Redirect("~/MyAccount/Employee/BasicInformation.aspx?Id=" + this.CompanyId);
                }

                menucontrol.PopulateMenu(0, 4, this.CompanyId, this.EmployeeId, true);

                ((Label)Master.FindControl("lblPageHeading")).Text = "Storefront Settings";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/Employee/ViewEmployee.aspx?id=" + this.CompanyId;

                //set Search Page Return URL
                if (IncentexGlobal.SearchPageReturnURLFrontSide != null)
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.SearchPageReturnURLFrontSide;
                }
            }
            else
            {
                Response.Redirect("~/MyAccount/Employee/ViewEmployee.aspx");
            }
            bindShoppingSetting();
            DisplayData(sender, e);
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
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
            Boolean IsMOASSelect = false;
            Boolean IsReplacementUniforms = false;
            foreach (DataListItem dtpayoption in dtPaymentOptions.Items)
            {
                if (((CheckBox)dtpayoption.FindControl("chkPaymentOptions")).Checked == true)
                {
                    Label lbl = ((Label)dtpayoption.FindControl("lblPaymentOptions"));

                    if (lbl.Text.Contains("MOAS"))
                        IsMOASSelect = true;
                    else if (lbl.Text.ToUpper() == "REPLACEMENT UNIFORMS")
                        IsReplacementUniforms = true;

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
            if (IsMOASSelect)
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
            else if (IsReplacementUniforms)
            {
                objCompanyEmployee.MOASEmailAddresses = IncentexGlobal.CurrentMember.UserInfoID + "|1";
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

            Response.Redirect("ViewEmployee.aspx?ID=" + this.CompanyId);

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
        CheckBox chk;
        HiddenField lblId;
        if (this.EmployeeId != 0)
        {
            UserInformationRepository objUIRepository = new UserInformationRepository();
            CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetById(this.EmployeeId);

            if (objCompanyEmployee == null)
            {
                return;

            }

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
        List<UserInformation> lstCA = new CompanyEmployeeRepository().GetCAByCompanyID(this.CompanyId).Where(x => x.UserInfoID != objCompanyEmployee.UserInfoID).ToList();
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

    #endregion
}