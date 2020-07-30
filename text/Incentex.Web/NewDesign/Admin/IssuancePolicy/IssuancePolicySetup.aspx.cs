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

public partial class NewDesign_Admin_IssuancePolicy_IssuancePolicySetup : PageBase
{
    #region Page Variable's
    Int64 PolicyID
    {
        get { return Convert.ToInt64(ViewState["PolicyID"]); }
        set { this.ViewState["PolicyID"] = value; }
    }
    Int64 CompanyID
    {
        get { return Convert.ToInt64(ViewState["CompanyID"]); }
        set { this.ViewState["CompanyID"] = value; }
    }
    Int64 WorkgroupID
    {
        get { return Convert.ToInt64(ViewState["WorkgroupID"]); }
        set { this.ViewState["WorkgroupID"] = value; }
    }
    String CategoryType
    {
        get { return Convert.ToString(ViewState["CategoryType"]); }
        set { this.ViewState["CategoryType"] = value; }
    }
    String GarmentType
    {
        get { return Convert.ToString(ViewState["GarmentType"]); }
        set { this.ViewState["GarmentType"] = value; }
    }
    Int64 ProductStyleID
    {
        get { return Convert.ToInt64(ViewState["ProductStyleID"]); }
        set { this.ViewState["ProductStyleID"] = value; }
    }
    /// <summary>
    /// Used to display last column in repeater bind
    /// </summary>
    Int32 rowCountIndex
    {
        get
        {

            return Convert.ToInt32(ViewState["rowCountIndex"]);
        }
        set
        {
            ViewState["rowCountIndex"] = value;
        }
    }
    String SiteURL
    {
        get
        {
            return Convert.ToString(ConfigurationSettings.AppSettings["siteurl"]);
        }
    }

    List<IPItem> NewAddedItems
    {
        get { return ViewState["NewAddedItems"] as List<IPItem>; }
        set { ViewState["NewAddedItems"] = value; }
    }

    String[] arraySteps = { "step-one", "step-two", "step-three", "step-four", "step-five", "step-six", "step-seven", "step-eight" };
    #endregion

    #region Page Event's
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["pID"]))
            {
                PolicyID = Convert.ToInt64(Request.QueryString["pID"]);
            }
            hdnCurrentStep.Value = "step-one";
            BindDropdowns();
            BindRadioButtons();
            FillWorkGroupForStore(0);
            FillCountry();

            BindPolicyItems();
        }

    }
    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillWorkGroupForStore(Convert.ToInt64(ddlCompany.SelectedValue));
    }
    #region Steps Back button event
    protected void lnkBtnSetp2Back_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('step-one');", true);
    }
    protected void lnkBtnSetp3Back_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('step-two');", true);
    }
    protected void lnkBtnSetp4Back_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('step-three');", true);
    }
    protected void lnkBtnSetp5Back_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('step-four');", true);
    }
    protected void lnkBtnSetp6Back_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('step-five');", true);
    }
    protected void lnkBtnSetp7Back_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('step-six');", true);
    }
    #endregion
    #region Steps Next button event
    protected void lnkBtnStep1Next_Click(object sender, EventArgs e)
    {
        this.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
        this.WorkgroupID = Convert.ToInt64(ddlWorkGroup.SelectedValue);
        String category = String.Empty;
        foreach (ListItem item in rdbCategoryList.Items)
        {
            if (item.Selected)
                category = item.Text;
        }
        this.CategoryType = new LookupRepository().GetValueID(category);
        this.GarmentType = new LookupRepository().GetValueID(rdbGender.SelectedItem.Text);
        // Bind Items as per selected options       
        UniformIssuancePolicy objUP = new UniformIssuancePolicy();
        UniformIssuancePolicyRepository objUPRepo = new UniformIssuancePolicyRepository();
        if (PolicyID != 0)
            objUP = objUPRepo.GetByUniformIssuancePolicyID(this.PolicyID);

        objUP.StoreId = new CompanyStoreRepository().GetStoreIDByCompanyId(this.CompanyID);
        objUP.WorkgroupID = this.WorkgroupID;
        objUP.GenderType = Convert.ToInt32(rdbGender.SelectedValue);
        objUP.IssuanceCategory = category;
        objUP.IssuanceProgramName = Convert.ToString(txtIssuancePolicyName.Text.Trim());
        objUP.PolicyDate = DateTime.Now;

        if (PolicyID == 0)
            objUPRepo.Insert(objUP);
        objUPRepo.SubmitChanges();
        // set to PolicyID
        PolicyID = objUP.UniformIssuancePolicyID;
        ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('step-two');", true);
    }
    protected void lnkBtnStep2Next_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('step-three');", true);
    }
    protected void lnkBtnStep3Next_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('step-four');", true);
    }
    protected void lnkBtnStep4Next_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('step-five');", true);
    }
    protected void lnkBtnStep5Next_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('step-six');", true);
    }
    protected void lnkBtnStep6Next_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('step-seven');", true);
    }
    protected void lnkBtnStep7Next_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('step-eight');", true);
        ShowSteps(Convert.ToString(hdnCurrentStep.Value));
    }


    #endregion


    #region  Policy Gridview Events
    protected void gvPolicyItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvPolicyItem.EditIndex = -1;
            gvPolicyItem.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void gvPolicyItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            
            GridViewRow row = (GridViewRow)gvPolicyItem.Rows[e.RowIndex];
            int intUniformIssuancePolicyItemID = Int32.Parse(gvPolicyItem.DataKeys[e.RowIndex].Value.ToString());
            DropDownList ddlEmployeeType = (DropDownList)row.FindControl("ddlEmployeeType");
            DropDownList ddlWeatherType = (DropDownList)row.FindControl("ddlWeatherType");
            //TextBox tPageDesc = (TextBox)row.FindControl("txtPageDesc");
            UniformIssuancePolicyItemRepository objUniformIssuancePolicyStyleRepository = new UniformIssuancePolicyItemRepository();
            UniformIssuancePolicyItem objnewitem = objUniformIssuancePolicyStyleRepository.GetById(intUniformIssuancePolicyItemID);
            objnewitem.WeatherTypeid = Convert.ToInt32(ddlWeatherType.SelectedValue);
            objnewitem.EmployeeTypeid = Convert.ToInt32(ddlEmployeeType.SelectedValue);
            objUniformIssuancePolicyStyleRepository.SubmitChanges();
            
            // Refresh the data
            gvPolicyItem.EditIndex = -1;
            gvPolicyItem.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
#endregion
    #endregion
    #region Method's
    private void ShowSteps(String steps)
    {
        switch (steps)
        {
            case "step-one":
                int keyIndex = Array.FindIndex(arraySteps, row => row.Contains(steps));
                ClientScript.RegisterStartupScript(this.GetType(), "displaydiv", "displayDivSteps('"+steps+"');", true);
                break;
            case "step-two":
                break;
            case "step-three":
                break;
            case "step-four":
                break;
            case "step-five":
                break;
            case "step-six":
                break;
            case "step-seven":
                break;

        }
    }


    /// <summary>
    /// Fills the Workgroup.
    /// </summary>
    /// <param name="countryid">The CompanyID.</param>
    public void FillWorkGroupForStore(Int64 CompanyID)
    {
        ddlWorkGroup.Items.Clear();
        Int64 StoreID = 0;
        if (CompanyID > 0)
            StoreID = new CompanyStoreRepository().GetStoreIDByCompanyId(Convert.ToInt64(CompanyID));
        List<GetStoreWorkGroupsResult> lstWorkGroups = new CompanyStoreRepository().GetStoreWorkGroups(StoreID).OrderBy(le => le.WorkGroup).ToList();
        if (lstWorkGroups.Where(le => le.Existing == 1).ToList().Count > 0)
            ddlWorkGroup.DataSource = lstWorkGroups.Where(le => le.Existing == 1);
        else
            ddlWorkGroup.DataSource = lstWorkGroups;

        ddlWorkGroup.DataValueField = "WorkGroupID";
        ddlWorkGroup.DataTextField = "WorkGroup";
        ddlWorkGroup.DataBind();
        ddlWorkGroup.Items.Insert(0, new ListItem("-Workgroup-", "0"));
    }
    private void BindDropdowns()
    {
        // For Company 
        ddlCompany.DataSource = new CompanyStoreRepository().GetCompanyStore().OrderBy(le => le.Company).ToList();
        ddlCompany.DataValueField = "CompanyId";
        ddlCompany.DataTextField = "Company";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("-Select-", "0"));

        // For ShippingAddressCompany 
        ddlShippingAddressCompany.DataSource = new CompanyStoreRepository().GetCompanyStore().OrderBy(le => le.Company).ToList();
        ddlShippingAddressCompany.DataValueField = "CompanyId";
        ddlShippingAddressCompany.DataTextField = "Company";
        ddlShippingAddressCompany.DataBind();
        ddlShippingAddressCompany.Items.Insert(0, new ListItem("-Select-", "0"));

        // For department 
        ddlShippingAddressDepartment.DataSource = new LookupRepository().GetByLookup("Department").OrderBy(le => le.sLookupName).ToList();
        ddlShippingAddressDepartment.DataValueField = "iLookupID";
        ddlShippingAddressDepartment.DataTextField = "sLookupName";
        ddlShippingAddressDepartment.DataBind();
        ddlShippingAddressDepartment.Items.Insert(0, new ListItem("-Select-", "0"));
       
    }
    /// <summary>
    /// FillCountry()
    /// This method is used to fill the country dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void FillCountry()
    {
        try
        {
            List<INC_Country> lstCountry = new CountryRepository().GetAll();
            if (lstCountry.Count > 0)
            {
                ddlCountry.DataSource = lstCountry;
                ddlCountry.DataTextField = "sCountryName";
                ddlCountry.DataValueField = "iCountryID";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("-select country-", "0"));
                ddlCountry.SelectedValue = ddlCountry.Items.FindByText("United States").Value;

                ddlState.Enabled = true;
                List<INC_State> lstState = new StateRepository().GetByCountryId(Convert.ToInt32(ddlCountry.SelectedItem.Value));
              
                    if (lstState.Count > 0)
                    {
                        ddlState.DataSource = lstState;
                        ddlState.DataValueField = "iStateID";
                        ddlState.DataTextField = "sStateName";
                        ddlState.DataBind();
                        ddlState.Items.Insert(0, new ListItem("-select state-", "0"));
                    }
              

            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
          
        }
        
    }
    public void BindRadioButtons()
    {
        LookupRepository objLookupRepos = new LookupRepository();
        //For gender
        List<INC_Lookup> genderList = objLookupRepos.GetByLookup("Gender");
        rdbGender.DataSource = genderList.OrderBy(o => o.sLookupName).ToList();// for sorting the value of gender with unisex
        rdbGender.DataValueField = "iLookupID";
        rdbGender.DataTextField = "sLookupName";
        rdbGender.DataBind();
        rdbGender.Items.Insert(2, new ListItem("Unisex", "99"));
        // Price Level
        rdbPricingList.DataSource = objLookupRepos.GetByLookup("PriceLevel").Where(q => q.sLookupName != "L4" && q.sLookupName != "CloseOutPrice");
        rdbPricingList.DataValueField = "Val1";
        rdbPricingList.DataTextField = "sLookupName";
        rdbPricingList.SelectedIndex = 0;// for L1 price
        rdbPricingList.DataBind();
        // Show Pricing
        rdbShowPricingList.DataSource = objLookupRepos.GetByLookup("ItemsToBePolybagged");
        rdbShowPricingList.DataValueField = "iLookupID";
        rdbShowPricingList.DataTextField = "sLookupName";
        rdbShowPricingList.DataBind();
    }

    public void BindPolicyItems()
    {
        List<SelectUniformIssuancePolicyProgramResult> objList = new UniformIssuancePolicyItemRepository().GetCreditProgram(this.PolicyID); 
        gvPolicyItem.DataSource = objList;
        gvPolicyItem.DataBind();
    }
    #endregion
}
