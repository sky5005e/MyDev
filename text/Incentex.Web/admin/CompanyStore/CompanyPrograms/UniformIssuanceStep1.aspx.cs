/* Project Name : Incentex 
 * Module Name : Uniform Isuance Program 
 * Description : This page is for add\edit of step1 of Uniform Isuance Program
 * ----------------------------------------------------------------------------------------- 
 * DATE | ID/ISSUE| AUTHOR | REMARKS 
 * ----------------------------------------------------------------------------------------- 
 * 23-Oct-2010 | 1 | Amit Trivedi | Design and Coding
 * ----------------------------------------------------------------------------------------- */
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
using System.Collections.Generic;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;


public partial class admin_CompanyStore_CompanyPrograms_UniformIssuanceStep1 : PageBase
{
    #region Properties

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
    Int64 IssuanceCompanyAddressId
    {
        get
        {
            if (ViewState["IssuanceCompanyAddressId"] == null)
            {
                ViewState["IssuanceCompanyAddressId"] = 0;
            }
            return Convert.ToInt64(ViewState["IssuanceCompanyAddressId"]);
        }
        set
        {
            ViewState["IssuanceCompanyAddressId"] = value;
        }
    }
    Int64 UniformIssuancePolicyID
    {
        get
        {
            if (ViewState["UniformIssuancePolicyID"] == null)
            {
                ViewState["UniformIssuancePolicyID"] = 0;
            }
            return Convert.ToInt64(ViewState["UniformIssuancePolicyID"]);
        }
        set
        {
            ViewState["UniformIssuancePolicyID"] = value;
        }
    }
    String PaymentType
    {
        get
        {
            if (ViewState["PaymentType"] == null)
            {
                ViewState["PaymentType"] = null;
            }
            return Convert.ToString(ViewState["PaymentType"]);
        }
        set
        {
            ViewState["PaymentType"] = value;
        }
    }

    String PaymentAddress
    {
        get
        {
            if (ViewState["PaymentAddress"] == null)
            {
                ViewState["PaymentAddress"] = null;
            }
            return Convert.ToString(ViewState["PaymentAddress"]);
        }
        set
        {
            ViewState["PaymentAddress"] = value;
        }
    }
    UniformIssuancePolicyRepository objUniformIssuancePolicyRepository = new UniformIssuancePolicyRepository();
    IssuanceCompanyAddressRepository objNewRepos = new IssuanceCompanyAddressRepository();
    LookupRepository objLookupRepos = new LookupRepository();
    INC_Lookup objLookup = new INC_Lookup();
    
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Uniform Issuance Policy - Step 1";
            this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));
            this.UniformIssuancePolicyID = Convert.ToInt64(Request.QueryString.Get("SubId"));
            this.PaymentType = Convert.ToString(Request.QueryString.Get("PaymentType"));
            this.IssuanceCompanyAddressId = Convert.ToInt64(Request.QueryString.Get("IssuanceCompanyAddressId"));
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/CompanyPrograms/ViewIssuancePrograms.aspx?Id=" + this.CompanyStoreId;
            menuControl.PopulateMenu(4, 1, this.CompanyStoreId, this.UniformIssuancePolicyID, true);
            BindDropDowns();
            FillPurchaseRequired();
            FillddlPriceLevel();
            FillPricefor();
            FillShowPrice();
            FillPolicyBeforeAfter();
            FillShowPaymentOption();
            DisplayData();
           
           
        }
    }
    #region Events
    /// <summary>
    /// insert or update record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            UniformIssuancePolicyRepository objUniformIssuancePolicyRepository1 = new UniformIssuancePolicyRepository();
            //SEtUPDATEPAYMENTOPTION();
            UniformIssuancePolicy objUniformIssuancePolicy = new UniformIssuancePolicy();
            if (this.UniformIssuancePolicyID != 0)
            {
                objUniformIssuancePolicy = objUniformIssuancePolicyRepository1.GetById(this.UniformIssuancePolicyID);
            }

            //Add by mayur for MOAS payment option validation on 1-march-2012
            List<string> MOASUserIds = new List<string>();

            if (ddlPaymentOptionAddress.SelectedItem.Text == "MOAS" && pnlMOAS.Visible == true && !chkApplyStationLevelApproverFlow.Checked)
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
                if (MOASUserIds == null || MOASUserIds.Count == 0)
                {
                    lblMsg.Text = "Please select at least one MOAS user.";
                    return;
                }
                else
                {
                    //UniformIssuancePolicyRepository objUniformIssuancePolicyRepositoryMOAS = new UniformIssuancePolicyRepository();
                    // UniformIssuancePolicy objUniformIssuancePolicyMOAS = objUniformIssuancePolicyRepositoryMOAS.GetById(this.UniformIssuancePolicyID);
                    //objUniformIssuancePolicyMOAS.MOASUserIDs = string.Join(",", MOASUserIds.ToArray());
                    // objUniformIssuancePolicyRepositoryMOAS.SubmitChanges();
                    objUniformIssuancePolicy.MOASUserIDs = string.Join(",", MOASUserIds.ToArray());
                }
            }
            //End mayur on 1-march-2012

            objUniformIssuancePolicy.StoreId = this.CompanyStoreId;
            objUniformIssuancePolicy.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
            objUniformIssuancePolicy.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            objUniformIssuancePolicy.IssuanceProgramName = txtProgramName.Text;
            objUniformIssuancePolicy.GenderType = Convert.ToInt32(ddlGender.SelectedValue);
            // 
            if (ddlCorporateDiscount.SelectedValue == "True")
                objUniformIssuancePolicy.CorporateDiscountStatus = true;
            else
                objUniformIssuancePolicy.CorporateDiscountStatus = false;

            if (!string.IsNullOrEmpty(txtCorporateAmount.Text))
                objUniformIssuancePolicy.CorporateAmount = Convert.ToDecimal(txtCorporateAmount.Text);
            else
                objUniformIssuancePolicy.CorporateAmount = null;

            if (ddlBeforeAfter.SelectedIndex > 0)
            {
                objUniformIssuancePolicy.ShowBeforeAfter = Convert.ToInt32(ddlBeforeAfter.SelectedValue);
            }
            else
            {
                objUniformIssuancePolicy.ShowBeforeAfter = 0;
            }

            objUniformIssuancePolicy.PolicyDate = Common.GetDate(txtPolicyDate);

            if (ddlPurchaseRequired.SelectedIndex >= 0)
            {
                if (ddlPurchaseRequired.SelectedItem.Text == "Yes")
                {
                    objUniformIssuancePolicy.CompletePurchase = 'Y';
                }
                else if (ddlPurchaseRequired.SelectedItem.Text == "No")
                {
                    objUniformIssuancePolicy.CompletePurchase = 'N';
                }
                else
                {
                    objUniformIssuancePolicy.CompletePurchase = 'O';
                }
            }

            objUniformIssuancePolicy.PricingLevel = ddlPriceLevel.SelectedValue;
            if (ddlShowPrice.SelectedIndex >= 0)
            {
                if (ddlShowPrice.SelectedItem.Text == "Yes")
                {
                    objUniformIssuancePolicy.SHOWPRICE = 'Y';
                }
                else
                {
                    objUniformIssuancePolicy.SHOWPRICE = 'N';
                }
            }
            if (ddlPricefor.SelectedIndex > 0)
            {
                objUniformIssuancePolicy.PRICEFOR = Convert.ToInt32(ddlPricefor.SelectedValue);
            }
            else
            {
                objUniformIssuancePolicy.PRICEFOR = 0;
            }

            if (ddlPaymentOption.SelectedIndex >= 0)
            {
                if (ddlPaymentOption.SelectedItem.Text == "Yes")
                {
                    objUniformIssuancePolicy.ShowPayment = 'Y';
                }
                else
                {
                    objUniformIssuancePolicy.ShowPayment = 'N';
                }
            }

            objUniformIssuancePolicy.PaymentOption = Convert.ToInt32(ddlPaymentOptionAddress.SelectedValue);
            if (ddlPaymentOptionAddress.SelectedItem.Text == "MOAS")
                objUniformIssuancePolicy.ApplyStationLevelApproverFlow = chkApplyStationLevelApproverFlow.Checked;

            if (this.UniformIssuancePolicyID == 0)
            {
                //insert
                objUniformIssuancePolicyRepository1.Insert(objUniformIssuancePolicy);
                objUniformIssuancePolicyRepository1.SubmitChanges();
                this.UniformIssuancePolicyID = objUniformIssuancePolicy.UniformIssuancePolicyID;
                //set default status
                LookupRepository objLookupRepository = new LookupRepository();
                List<INC_Lookup> objList = objLookupRepository.GetByLookupCode(DAEnums.LookupCodeType.Status);
                objList.OrderBy(l => l.iLookupID);

                if (objList.Count > 0)
                {
                    objUniformIssuancePolicy.Status = objList[1].iLookupID;
                }

            }
            else
            {
                objUniformIssuancePolicyRepository1.SubmitChanges();
                this.UniformIssuancePolicyID = objUniformIssuancePolicy.UniformIssuancePolicyID;
            }
            if (ddlPaymentOptionAddress.SelectedItem.Text.Trim() == "Company Pays")
            {
                Response.Redirect("IssuanceCompanyAddress.aspx?SubId=" + UniformIssuancePolicyID + "&PaymentType=CompanyPays" + "&Id=" + CompanyStoreId + "&Programname=" + txtProgramName.Text + "&CompletePR=" + ddlPurchaseRequired.SelectedValue + "&PriceLevel=" + ddlPriceLevel.SelectedValue + "&ShowPrice=" + ddlShowPrice.SelectedValue + "&Pricefor=" + ddlPricefor.SelectedValue + "&Department=" + ddlDepartment.SelectedValue + "&Workgroup=" + ddlWorkgroup.SelectedValue + "&PolicyDate=" + txtPolicyDate.Text + "&ShowBeforeAfter=" + ddlBeforeAfter.SelectedValue + "&ShowShippingPayment=" + ddlPaymentOption.SelectedValue + "&Gender=" + ddlGender.SelectedValue);
            }
            else if (ddlPaymentOptionAddress.SelectedItem.Text.Trim() == "MOAS")
            {
                Response.Redirect("IssuanceCompanyAddress.aspx?SubId=" + UniformIssuancePolicyID + "&PaymentType=MOAS" + "&Id=" + CompanyStoreId + "&Programname=" + txtProgramName.Text + "&CompletePR=" + ddlPurchaseRequired.SelectedValue + "&PriceLevel=" + ddlPriceLevel.SelectedValue + "&ShowPrice=" + ddlShowPrice.SelectedValue + "&Pricefor=" + ddlPricefor.SelectedValue + "&Department=" + ddlDepartment.SelectedValue + "&Workgroup=" + ddlWorkgroup.SelectedValue + "&PolicyDate=" + txtPolicyDate.Text + "&ShowBeforeAfter=" + ddlBeforeAfter.SelectedValue + "&ShowShippingPayment=" + ddlPaymentOption.SelectedValue + "&Gender=" + ddlGender.SelectedValue);
            }
            else
            {
                Response.Redirect("UniformIssuanceStep2.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID);
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// redirect to previous listing page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewIssuancePrograms.aspx?Id=" + this.CompanyStoreId);
    }
    #endregion
    #region Methods
    public void FillPricefor()
    {
        try
        {
            string strStatus = "Pricefor";
            ddlPricefor.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlPricefor.DataValueField = "iLookupID";
            ddlPricefor.DataTextField = "sLookupName";
            ddlPricefor.DataBind();
            ddlPricefor.Items.Insert(0, new ListItem("-select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    public void FillPolicyBeforeAfter()
    {
        try
        {
            string strStatus = "BeforeAfter";
            ddlBeforeAfter.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlBeforeAfter.DataValueField = "iLookupID";
            ddlBeforeAfter.DataTextField = "sLookupName";
            ddlBeforeAfter.DataBind();
            ddlBeforeAfter.Items.Insert(0, new ListItem("-select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    public void FillShowPrice()
    {
        try
        {
            LookupRepository objLookupRepos = new LookupRepository();
            string strStatus = "ItemsToBePolybagged ";
            ddlShowPrice.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlShowPrice.DataValueField = "iLookupID";
            ddlShowPrice.DataTextField = "sLookupName";
            ddlShowPrice.DataBind();
            //  ddlPurchaseRequired.Items.Insert(0, new ListItem("-select-", "0"));
            ddlShowPrice.SelectedValue = ddlShowPrice.Items.FindByText("No").Value;
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    public void FillShowPaymentOption()
    {
        try
        {
            LookupRepository objLookupRepos = new LookupRepository();
            string strStatus = "ItemsToBePolybagged ";
            ddlPaymentOption.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlPaymentOption.DataValueField = "iLookupID";
            ddlPaymentOption.DataTextField = "sLookupName";
            ddlPaymentOption.DataBind();
            ddlPaymentOption.SelectedValue = ddlShowPrice.Items.FindByText("No").Value;
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    ///Fill the Item Fill Allow back Order dropdownlist
    ///from lookup table
    /// FillPurchaseRequired()
    /// Nagmani 05/02/2011
    /// </summary>
    public void FillPurchaseRequired()
    {
        try
        {
            LookupRepository objLookupRepos = new LookupRepository();
            ddlPurchaseRequired.DataSource = objLookupRepos.GetByLookup("IssuancePurchasedType");
            ddlPurchaseRequired.DataValueField = "iLookupID";
            ddlPurchaseRequired.DataTextField = "sLookupName";
            ddlPurchaseRequired.DataBind();
            ddlPurchaseRequired.SelectedValue = ddlPurchaseRequired.Items.FindByText("No").Value;
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    ///Fill the Item Fill PriceLevel
    ///from lookup table
    /// FillddlPriceLevel()
    /// Nagmani 05/02/2011
    /// </summary>
    public void FillddlPriceLevel()
    {
        try
        {
            LookupRepository objLookupRepos = new LookupRepository();
            string strStatus = "PriceLevel";
            ddlPriceLevel.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlPriceLevel.DataValueField = "Val1";
            ddlPriceLevel.DataTextField = "sLookupName";
            ddlPriceLevel.DataBind();
            ddlPriceLevel.Items.Insert(0, new ListItem("-select-", "0"));
            
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }  
    /// <summary>
    /// Display data if exists in database
    /// </summary>
    void DisplayData()
    {
        if (this.UniformIssuancePolicyID == 0)
        {
            BinData();
            if (Request.QueryString["PaymentType"] != null)
            {
                FillddlPriceLevel();
                FillPurchaseRequired();
                FillShowPrice();
                FillPolicyBeforeAfter();
                FillShowPaymentOption();
                if (Convert.ToInt64(Request.QueryString["IssuanceCompanyAddressId"]) != 0)
                {
                    txtProgramName.Text = Request.QueryString.Get("Programname");
                    ddlPurchaseRequired.SelectedValue = ddlPurchaseRequired.Items.FindByValue(Request.QueryString.Get("CompletePR")).Value;
                    ddlPriceLevel.SelectedValue = ddlPriceLevel.Items.FindByValue(Request.QueryString.Get("PriceLevel")).Value;
                    ddlShowPrice.SelectedValue = ddlShowPrice.Items.FindByValue(Request.QueryString.Get("ShowPrice")).Value;
                    ddlPricefor.SelectedValue = ddlPricefor.Items.FindByValue(Request.QueryString.Get("Pricefor")).Value;
                    ddlDepartment.SelectedValue = Request.QueryString.Get("Department");
                    ddlWorkgroup.SelectedValue = Request.QueryString.Get("Workgroup");
                    txtPolicyDate.Text = Request.QueryString.Get("PolicyDate");
                    ddlBeforeAfter.SelectedValue = ddlBeforeAfter.Items.FindByValue(Request.QueryString.Get("ShowBeforeAfter")).Value;
                    ddlPaymentOption.SelectedValue = ddlPaymentOption.Items.FindByValue(Request.QueryString.Get("ShowShippingPayment")).Value;
                    ddlGender.SelectedValue = Request.QueryString.Get("Gender");
                    if (this.PaymentType != "")
                    {
                        ddlPaymentOptionAddress.Items.FindByText("Company Pays").Selected = true;
                        ddlPaymentOptionAddress.SelectedItem.Value = ddlPaymentOptionAddress.Items.FindByText("Company Pays").Value;
                        pnlMOAS.Visible = false;
                    }
                }
            }
            return;
        }

        UniformIssuancePolicy objUniformIssuancePolicy = objUniformIssuancePolicyRepository.GetById(this.UniformIssuancePolicyID);

        if (objUniformIssuancePolicy != null)
        {
            ddlWorkgroup.SelectedValue = objUniformIssuancePolicy.WorkgroupID.ToString();
            ddlDepartment.SelectedValue = objUniformIssuancePolicy.DepartmentID.ToString();
            txtProgramName.Text = objUniformIssuancePolicy.IssuanceProgramName;
            ddlGender.SelectedValue = Convert.ToString(objUniformIssuancePolicy.GenderType);
            ddlCorporateDiscount.SelectedValue = Convert.ToString(objUniformIssuancePolicy.CorporateDiscountStatus);
            if (objUniformIssuancePolicy.CorporateAmount != null && objUniformIssuancePolicy.CorporateDiscountStatus)
            {
                txtCorporateAmount.Text = objUniformIssuancePolicy.CorporateAmount.ToString();
                dvCorporate.Visible = true;
            }
            else
            {
                dvCorporate.Visible = false;
            }

            FillddlPriceLevel();
            if (objUniformIssuancePolicy.PricingLevel != null)
            {
                ddlPriceLevel.SelectedValue = objUniformIssuancePolicy.PricingLevel.Trim();
            }
            //hdnPrice.Value =objUniformIssuancePolicy.PricingLevel;

            FillPurchaseRequired();
            if (objUniformIssuancePolicy.CompletePurchase == 'N')
            {
                ddlPurchaseRequired.SelectedValue = ddlPurchaseRequired.Items.FindByText("No").Value;
            }
            else if (objUniformIssuancePolicy.CompletePurchase == 'Y')
            {
                ddlPurchaseRequired.SelectedValue = ddlPurchaseRequired.Items.FindByText("Yes").Value;
            }
            else if (objUniformIssuancePolicy.CompletePurchase == 'O')
            {
                ddlPurchaseRequired.SelectedValue = ddlPurchaseRequired.Items.FindByText("Open Forum").Value;
            }
            FillShowPrice();
            if (objUniformIssuancePolicy.SHOWPRICE == 'N')
            {
                ddlShowPrice.SelectedValue = ddlShowPrice.Items.FindByText("No").Value;
            }
            else if (objUniformIssuancePolicy.SHOWPRICE == 'Y')
            {
                ddlShowPrice.SelectedValue = ddlShowPrice.Items.FindByText("Yes").Value;
            }

            FillPolicyBeforeAfter();
            if (objUniformIssuancePolicy.ShowBeforeAfter != null)
            {
                ddlBeforeAfter.SelectedValue = objUniformIssuancePolicy.ShowBeforeAfter.ToString();
            }
            FillPricefor();
            ddlPricefor.SelectedValue = objUniformIssuancePolicy.PRICEFOR.ToString();
            if (objUniformIssuancePolicy.PolicyDate != null)
            {
                txtPolicyDate.Text = Common.GetDateString(objUniformIssuancePolicy.PolicyDate);
            }

            FillShowPaymentOption();
            if (objUniformIssuancePolicy.ShowPayment == 'N')
            {
                ddlPaymentOption.SelectedValue = ddlPaymentOption.Items.FindByText("No").Value;
            }
            else if (objUniformIssuancePolicy.ShowPayment == 'Y')
            {
                ddlPaymentOption.SelectedValue = ddlPaymentOption.Items.FindByText("Yes").Value;
            }

            if (Convert.ToInt32(Request.QueryString["IssuanceCompanyAddressId"])!=0)
            {

                txtProgramName.Text = Request.QueryString.Get("Programname");
                ddlPurchaseRequired.SelectedValue = ddlPurchaseRequired.Items.FindByValue(Request.QueryString.Get("CompletePR")).Value;
                ddlPriceLevel.SelectedValue = ddlPriceLevel.Items.FindByValue(Request.QueryString.Get("PriceLevel")).Value;
                ddlShowPrice.SelectedValue = ddlShowPrice.Items.FindByValue(Request.QueryString.Get("ShowPrice")).Value;
                ddlPricefor.SelectedValue = ddlPricefor.Items.FindByValue(Request.QueryString.Get("Pricefor")).Value;
                ddlDepartment.SelectedValue = Request.QueryString.Get("Department");
                ddlWorkgroup.SelectedValue = Request.QueryString.Get("Workgroup");
                txtPolicyDate.Text = Request.QueryString.Get("PolicyDate");
                ddlBeforeAfter.SelectedValue = ddlBeforeAfter.Items.FindByValue(Request.QueryString.Get("ShowBeforeAfter")).Value;
                ddlPaymentOption.SelectedValue = ddlPaymentOption.Items.FindByValue(Request.QueryString.Get("ShowShippingPayment")).Value;
                ddlGender.SelectedValue = Request.QueryString.Get("Gender");

            }

            BinData();
            if (this.PaymentType == "CompanyPays")
            {
                ddlPaymentOptionAddress.SelectedValue = ddlPaymentOptionAddress.Items.FindByText("Company Pays").Value;
             
            }

            else if (this.PaymentType == "MOAS")
            {

                ddlPaymentOptionAddress.SelectedValue = ddlPaymentOptionAddress.Items.FindByText("MOAS").Value;
            }
            else if (objUniformIssuancePolicy.PaymentOption == 3)
            {
                ddlPaymentOptionAddress.SelectedValue = ddlPaymentOptionAddress.Items.FindByText("MOAS").Value;
                chkApplyStationLevelApproverFlow.Checked = Convert.ToBoolean(objUniformIssuancePolicy.ApplyStationLevelApproverFlow);
                DisplayMOASData();
            }
            else if (objUniformIssuancePolicy.PaymentOption == 1)
            {

                ddlPaymentOptionAddress.SelectedValue = ddlPaymentOptionAddress.Items.FindByText("Company Pays").Value;
            }
            else if (objUniformIssuancePolicy.PaymentOption == 2)
            {
                ddlPaymentOptionAddress.SelectedValue = ddlPaymentOptionAddress.Items.FindByText("Employee Pays - PD").Value;
            }
            
            this.PaymentAddress = ddlPaymentOptionAddress.SelectedItem.Text;
            
        }
       
        
    }

    //bind dropdown for the workgroup, department
    public void BindDropDowns()
    {
        LookupRepository objLookRep = new LookupRepository();
        
        ddlGender.DataSource = objLookRep.GetByLookup("Gender");
        ddlGender.DataValueField = "iLookupID";
        ddlGender.DataTextField = "sLookupName";
        ddlGender.DataBind();
        ddlGender.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Department
        ddlDepartment.DataSource = objLookRep.GetByLookup("Department");
        ddlDepartment.DataValueField = "iLookupID";
        ddlDepartment.DataTextField = "sLookupName";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Workgroup
        ddlWorkgroup.DataSource = objLookRep.GetByLookup("Workgroup");
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));


    }
    void BinData()
    {
        this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));

        //bind Payment Option
        ListItem LiCompany = new ListItem(Incentex.DAL.Common.DAEnums.GetUniformIssuancePaymentOptionName(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.CompanyPays),
             Convert.ToString((int)Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.CompanyPays));

        ListItem LiEmp = new ListItem(Incentex.DAL.Common.DAEnums.GetUniformIssuancePaymentOptionName(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.EmployeePays),
             Convert.ToString((int)Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.EmployeePays));

        //check that moas user is available for this workgroup or not 
        List<UserInformation> lstCA = new CompanyEmployeeRepository().GetCAByWorkgroupId(Convert.ToInt64(ddlWorkgroup.SelectedValue), new CompanyStoreRepository().GetById(this.CompanyStoreId).CompanyID);
        //Add by mayur got moas option 1-mar-2012
        ListItem LiMOAS = new ListItem(Incentex.DAL.Common.DAEnums.GetUniformIssuancePaymentOptionName(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.MOAS),
             Convert.ToString((int)Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.MOAS));
        ddlPaymentOptionAddress.Items.Insert(0, LiCompany);
        ddlPaymentOptionAddress.Items.Insert(0, LiMOAS);
        ddlPaymentOptionAddress.Items.Insert(0, LiEmp);
        Common.AddOnChangeAttribute(ddlPaymentOptionAddress);
       
       
    }

    protected void ddlCorporateDiscount_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCorporateDiscount.SelectedValue == "True")
            dvCorporate.Visible = true;
        else
            dvCorporate.Visible = false;
    }
    protected void ddlPaymentOptionAddress_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        if (ddlPaymentOptionAddress.SelectedItem.Text.Trim() == "MOAS")
        {
            pnlMOAS.Visible = true;
            DisplayMOASData();
        }
        else
        {
            lblMsg.Text = "";
            pnlMOAS.Visible = false;
        }

        this.PaymentAddress = ddlPaymentOptionAddress.SelectedItem.Text;
       
    }
    #endregion
    #region Display moas user of this issuance policy
    protected void DisplayMOASData()
    {
        //bind CA for workgroup selected for this policy
        List<UserInformation> lstCA = new CompanyEmployeeRepository().GetCAByCompanyID(new CompanyStoreRepository().GetById(this.CompanyStoreId).CompanyID);
        dtCompanyAdmin.DataSource = lstCA;
        dtCompanyAdmin.DataBind();

        pnlMOAS.Visible = true;
        UniformIssuancePolicyRepository objUniformIssuancePolicyRepositoryMOAS = new UniformIssuancePolicyRepository();
        UniformIssuancePolicy objUniformIssuancePolicyMOAS = objUniformIssuancePolicyRepositoryMOAS.GetById(this.UniformIssuancePolicyID);

        if (objUniformIssuancePolicyMOAS != null && !string.IsNullOrEmpty(objUniformIssuancePolicyMOAS.MOASUserIDs))
        {
            string[] MOASUsers = objUniformIssuancePolicyMOAS.MOASUserIDs.Split(',');

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
    /// <summary>
    /// Check if payment option in edit mode selected index changed to change one 
    /// Option to another option
    /// </summary>
    public void SEtUPDATEPAYMENTOPTION()
    {
        if (this.UniformIssuancePolicyID != 0)
        {
            string strAddreess = null;
            UniformIssuancePolicy objUniformIssuancePolicy = new UniformIssuancePolicy();
            if (this.UniformIssuancePolicyID != 0)
            {
                objUniformIssuancePolicy = objUniformIssuancePolicyRepository.GetById(this.UniformIssuancePolicyID);
            }

            if (objUniformIssuancePolicy.PaymentOption == 1)
            {
                strAddreess = "Company Pays";
            }
            else if (objUniformIssuancePolicy.PaymentOption == 3)
            {
                strAddreess = "MOAS";
            }
            else
            {
                strAddreess = "Employee Pays - PD";
            }
            IssuanceCompanyAddressRepository objAddressRepos = new IssuanceCompanyAddressRepository();
            if (this.PaymentAddress != strAddreess)
            {
                string steDelete = objAddressRepos.DeleteAddress(this.UniformIssuancePolicyID);
            }
            //string update = objUniformIssuancePolicyRepository.UpdateMOASUserid(this.UniformIssuancePolicyID);
        }
    }
    #endregion
    

}
