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

public partial class NewDesign_UserFrameItems_IPWizardSetup : System.Web.UI.Page
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
    #endregion

    #region Page Event's
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["pID"]))
            {
                PolicyID = Convert.ToInt64(Request.QueryString["pID"]);

                //this.CompanyID = 3;
                //this.WorkgroupID = 51;
                //String category = "Uniform Items";
                //this.CategoryType = new LookupRepository().GetValueID(category);
                //this.GarmentType = new LookupRepository().GetValueID("Unisex");
                //BindProductStyle();
                //DisplaytControl(pnlPart2Item);
                //DisplaytControl(pnlPart4Preview);
                //BindPreviewData(this.PolicyID);
                DisplaytControl(pnlPart1wizard);
                BindDropDown();
                BindRadioButtons();
                PopulateData(PolicyID);
            }
        }

    }
    
    protected void WizardIP_NextButtonClick(object sender, EventArgs e)
    {
        if (WizardIP.ActiveStep.Name == "Company")
            FillWorkGroupForStore(Convert.ToInt64(ddlCompany.SelectedValue));
    }

    protected void WizardIP_FinishButtonClick(object sender, EventArgs e)
    {
        DisplaytControl(pnlPart2Item);
        this.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
        this.WorkgroupID = Convert.ToInt64(ddlWorkGroup.SelectedValue);
        String category = String.Empty;
        foreach (ListItem item in rdbCategoryList.Items)
        {
            if (item.Selected)
                category = item.Text;
        }
        Int64 climateTypeID = 0;
        foreach (ListItem cl in rdbClimateList.Items)
        {
            if (cl.Selected)
                climateTypeID = Convert.ToInt64(cl.Value);
        }
        String priceLevel = String.Empty;
        foreach (ListItem pl in rdbPricingList.Items)
        {
            if (pl.Selected)
                priceLevel = pl.Text;
        }
        String showPrice = String.Empty;
        foreach (ListItem sp in rdbShowPricingList.Items)
        {
            if (sp.Selected)
                showPrice = sp.Text;
        }
        String completePurchase = String.Empty;
        if (rdbComplete.Checked)
            completePurchase = "Y";
        else if (rdbPartial.Checked)
            completePurchase = "N";
        else
            completePurchase = "O";
        // Step 8
        Boolean IsDateOfHired = false;
        Int32 noMonthsonDOH = 0;
        DateTime CreditEligibleDate = DateTime.Now;
        DateTime CreditExpireDate = DateTime.Now;
        if (rdbPolicyActivationDOH.Checked)
        {
            if (rdbPolicyActivationUOM.Checked)
                noMonthsonDOH = Convert.ToInt32(ddlmonth.SelectedValue);
            IsDateOfHired = true;
        }
        else if (rdbPolicyActivationDate.Checked)
        {
            CreditEligibleDate = Convert.ToDateTime(txtDateFrom.Text.Trim());
            CreditExpireDate = Convert.ToDateTime(txtDateTo.Text.Trim());
        }
        else if (rdbPolicyActivationPreviusIssuance.Checked)
        {
            // Implement later
            //if (rdbPolicyActivationMonth1.Checked)
            //{}
            //else if (rdbPolicyActivationMonth2.Checked)
            //{ }
            //else
            //{ } 
        }
        // Steps 9
        Int32 ExpireMonthNumber = Convert.ToInt32(ddlExpiremonthNumber.SelectedValue);
        Int32 PolicyReactivateMonthNo = 0;
        if (rdbPolicyExpireFinal.Checked)
        { }
        else if (rdbPolicyExpireReactivateBeforeTime.Checked)
        { }
        else if (rdbPolicyExpireReactivateBeforeMonth.Checked)
        {
            PolicyReactivateMonthNo = Convert.ToInt32(ddlPolicyReactivateMonthNo.SelectedValue);
        }
        this.CategoryType = new LookupRepository().GetValueID(category);
        this.GarmentType = new LookupRepository().GetValueID(ddlGender.SelectedItem.Text);

        // Bind Items as per selected options       
        UniformIssuancePolicy objUP = new UniformIssuancePolicy();
        UniformIssuancePolicyRepository objUPRepo = new UniformIssuancePolicyRepository();
        if (PolicyID != 0)
            objUP = objUPRepo.GetByUniformIssuancePolicyID(this.PolicyID);

        objUP.StoreId = new CompanyStoreRepository().GetStoreIDByCompanyId(this.CompanyID);
        objUP.WorkgroupID = this.WorkgroupID;
        objUP.GenderType = Convert.ToInt32(ddlGender.SelectedValue);
        objUP.IssuanceCategory = category;
        objUP.IssuanceProgramName = Convert.ToString(txtIssuancePolicyName.Text.Trim());
        objUP.ClimateType = climateTypeID;
        objUP.PricingLevel = GetValue(priceLevel);
        objUP.SHOWPRICE = Convert.ToChar(GetValue(showPrice));
        objUP.CompletePurchase = Convert.ToChar(completePurchase);
        if (IsDateOfHired)
        {
            objUP.IsDateOfHiredTicked = true;
            if (noMonthsonDOH > 0)
                objUP.NumberOfMonths = noMonthsonDOH;
        }
        else if (rdbPolicyActivationDate.Checked)
        {
            objUP.EligibleDate = CreditEligibleDate;
            objUP.CreditExpireDate = CreditExpireDate;
        }
        objUP.CreditExpireNumberOfMonths = ExpireMonthNumber;
        //objUP.ExpiresAfter = ExpireMonthNumber;
        objUP.PolicyDate = DateTime.Now;
        objUP.ShowPayment = 'N';//
        objUPRepo.Insert(objUP);
        objUPRepo.SubmitChanges();

        // set to PolicyID
        PolicyID = objUP.UniformIssuancePolicyID;

        BindProductStyle();
        lblIssuancePolicyName.Text = txtIssuancePolicyName.Text;

    }


    protected void ddlProductStyle_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ProductStyleID = Convert.ToInt64(ddlProductStyle.SelectedValue);
        BindProductItem();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            DisplaytControl(pnlPart3Preview);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void btnPeviewNext_Click(object sender, EventArgs e)
    {
        try
        {
            BindPreviewData(this.PolicyID);
            DisplaytControl(pnlPart4Preview);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void btnAddItemtoIP_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (RepeaterItem rptItem in rptIPItemList.Items)
            {
                CheckBox chkAddItem = (CheckBox)rptItem.FindControl("chkAddItem");
                HiddenField hdnStoreProductID = (HiddenField)rptItem.FindControl("hdnStoreProductID");
                HiddenField hdnMasterItemID = (HiddenField)rptItem.FindControl("hdnMasterItemID");
                if (chkAddItem != null && chkAddItem.Checked)
                {
                    
                     Int32 _storeProductID = Convert.ToInt32(hdnStoreProductID.Value);
                     Int64 _masterItemID = Convert.ToInt64(hdnMasterItemID.Value);
                    // Insert items and bind
                    AddItemtoIP(0, _storeProductID, 0, _masterItemID, 0, null, null, null, 0, 0);
                }
            }
            // Bind IP Item
            BindBackIPItem();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    
    protected void rptIPItemList_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnStoreProductID = (HiddenField)e.Item.FindControl("hdnStoreProductID");
                HiddenField hdnProductImage = (HiddenField)e.Item.FindControl("hdnProductImage");
               
                String ImageLargePath = String.Empty;
                HtmlImage imgProduct = (HtmlImage)e.Item.FindControl("imgProduct");
                // START - to add last row class
                if (e.Item.ItemIndex == 0)
                    rowCountIndex = 1;
                else
                    rowCountIndex++;

                if (rowCountIndex % 3 == 0)
                {
                    HtmlControl liRepeater = (HtmlControl)e.Item.FindControl("liRepeater");
                    liRepeater.Attributes.Add("class", "last");
                }
                // END - to add last row class

                // START - Display Images
                if (!String.IsNullOrEmpty(hdnProductImage.Value))
                    imgProduct.Src = SiteURL + "UploadedImages/ProductImages/Thumbs/" + hdnProductImage.Value.ToString();
                else
                    imgProduct.Src = SiteURL + "UploadedImages/ProductImages/ProductDefault.jpg";
                // END - Display Images
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    protected void rptPolicyPreviewItem_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            Int64 uniformPolicyitemID = Convert.ToInt64(e.CommandArgument);
            try
            {
                UniformIssuancePolicyItemRepository ObjUPItemRepo = new UniformIssuancePolicyItemRepository();
                UniformIssuancePolicyItem ObjUPItem = ObjUPItemRepo.GetById(uniformPolicyitemID);
                ObjUPItemRepo.Delete(ObjUPItem);
                ObjUPItemRepo.SubmitChanges();
                BindPreviewData(this.PolicyID);
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }

        }
    }
    protected void rptPolicyPreviewItem_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnItemStoreProductID = (HiddenField)e.Item.FindControl("hdnItemStoreProductID");
                HiddenField hdnItemProductImage = (HiddenField)e.Item.FindControl("hdnItemProductImage");
                String ImageLargePath = String.Empty;
                HtmlImage imgIPItem = (HtmlImage)e.Item.FindControl("imgIPItem");
               
                // START - Display Images
                if (!String.IsNullOrEmpty(hdnItemProductImage.Value))
                    imgIPItem.Src = SiteURL + "UploadedImages/ProductImages/Thumbs/" + hdnItemProductImage.Value.ToString();
                else
                    imgIPItem.Src = SiteURL + "UploadedImages/ProductImages/ProductDefault.jpg";
                // END - Display Images
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    #endregion

    #region Page Method's
    /// <summary>
    /// Get Value
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private String GetValue(String key)
    {
        String value = String.Empty;
        switch (key)
        {
            case "L1":
                value = "1";
                break;
            case "L2":
                value = "2";
                break;
            case "L3":
                value = "3";
                break;
            case "Yes":
                value = "Y";
                break;
            case "No":
                value = "N";
                break;
            default:
                value = String.Empty;
                break;
        }
        return value;
    }

    private void BindProductStyle()
    {
        try
        {
            NewAddedItems = new UniformIssuancePolicyItemRepository().GetIssuancePolicyItemDetails(this.CompanyID, this.WorkgroupID, this.CategoryType, this.GarmentType);
            // Bind Drop for Product Style
            ddlProductStyle.DataSource = NewAddedItems.Select(q => new { q.Style, q.StyleID }).Distinct().OrderBy(o => o.Style).ToList();
            ddlProductStyle.DataValueField = "StyleID";
            ddlProductStyle.DataTextField = "Style";
            ddlProductStyle.DataBind();
            ddlProductStyle.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
            
    }

    private void BindProductItem()
    {
        try
        {
            DisplaytControl(pnlPartItemSelect);
            rptIPItemList.DataSource = NewAddedItems.Where(q => q.StyleID == this.ProductStyleID).ToList();
            rptIPItemList.DataBind();
           
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    private void AddItemtoIP(Int64 uniformIssuancePolicyItemID, Int32 storeproductID, Int32 issuanceQty, Int64 masterItemID, Int64 AssociationType, String AssociationNote, String newGroupName, String isGrpAssociation, Int32 climateType, Int32 employeeType)
    {
        UniformIssuancePolicyItemRepository ObjUPItemRepo = new UniformIssuancePolicyItemRepository();
        UniformIssuancePolicyItem ObjUPItem = new UniformIssuancePolicyItem();
        if (uniformIssuancePolicyItemID > 0)
            ObjUPItem = ObjUPItemRepo.GetById(uniformIssuancePolicyItemID);
        ObjUPItem.UniformIssuancePolicyID = this.PolicyID;
        if (issuanceQty > 0)
            ObjUPItem.Issuance = issuanceQty;
        ObjUPItem.MasterItemId = masterItemID;
        //ObjUPItem.RankId = RankId;
        ObjUPItem.CreatedDate = DateTime.Now;
        if (AssociationType > 0)
            ObjUPItem.AssociationIssuanceType = AssociationType;
        //ObjUPItem.AssociationbudgetAmt = strBudgetAmount;
        if (!String.IsNullOrEmpty(AssociationNote))
            ObjUPItem.AssociationIssuancePolicyNote = AssociationNote;
        if (!String.IsNullOrEmpty(newGroupName))
            ObjUPItem.NEWGROUP = newGroupName;
        if (!String.IsNullOrEmpty(isGrpAssociation))
            ObjUPItem.ISGROUPASSOCIATION = Convert.ToChar(isGrpAssociation);
        if (climateType > 0)
            ObjUPItem.WeatherTypeid = climateType;
        if (employeeType > 0)
            ObjUPItem.EmployeeTypeid = employeeType;

        ObjUPItem.StoreProductid = storeproductID;

        if (uniformIssuancePolicyItemID == 0)
            ObjUPItemRepo.Insert(ObjUPItem);
        ObjUPItemRepo.SubmitChanges();


    }

    private void BindBackIPItem()
    {
        gvItemsInfo.DataSource = new UniformIssuancePolicyItemRepository().GetUniformIssuancePolicyItems(this.PolicyID);
        gvItemsInfo.DataBind();
        DisplaytControl(pnlPart2Item);
    }
    /// <summary>
    /// To set Control visibility
    /// </summary>
    /// <param name="pnl"></param>
    private void DisplaytControl(Panel pnl)
    {
        pnlPart1wizard.Visible = pnlPart2Item.Visible = pnlPart3Preview.Visible = pnlPartItemSelect.Visible = pnlPart4Preview.Visible =  false;
        pnl.Visible = true;
    }
    
    //bind dropdown
    public void BindDropDown()
    {
        // For Company 
        ddlCompany.DataSource = new CompanyStoreRepository().GetCompanyStore().OrderBy(le => le.Company).ToList();
        ddlCompany.DataValueField = "CompanyId";
        ddlCompany.DataTextField = "Company";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("-Select-", "0"));
        //

        LookupRepository objLookRep = new LookupRepository();
        //For gender
        List<INC_Lookup> genderList = objLookRep.GetByLookup("Gender");
        INC_Lookup objGender = new INC_Lookup();
        objGender.iLookupID = 99;
        objGender.iLookupCode = "Gender";
        objGender.sLookupName = "Unisex";
        genderList.Add(objGender);
        ddlGender.DataSource = genderList.OrderBy(o => o.sLookupName).ToList();// for sorting the value of gender with unisex
        ddlGender.DataValueField = "iLookupID";
        ddlGender.DataTextField = "sLookupName";
        ddlGender.DataBind();
        ddlGender.Items.Insert(0, new ListItem("-Gender-", "0"));

        // For base stations
        ddlItemBaseStation.DataSource = new CompanyRepository().GetAllBaseStationResult().OrderBy(b => b.sBaseStation).ToList();
        ddlItemBaseStation.DataValueField = "iBaseStationId";
        ddlItemBaseStation.DataTextField = "sBaseStation";
        ddlItemBaseStation.DataBind();
        ddlItemBaseStation.Items.Insert(0, new ListItem("-Base Station-", "0"));
      

    }
    public void BindRadioButtons()
    {
        LookupRepository objLookupRepos = new LookupRepository();
        rdbClimateList.DataSource = objLookupRepos.GetByLookup("StationAdditionalInfo").OrderBy(o => o.sLookupName).ToList();// for sorting the value of gender with unisex
        rdbClimateList.DataValueField = "iLookupID";
        rdbClimateList.DataTextField = "sLookupName";
        rdbClimateList.DataBind();
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
        // ExcludedBaseStation
        rdbExcludedBaseStation.DataSource = objLookupRepos.GetByLookup("ItemsToBePolybagged");
        rdbExcludedBaseStation.DataValueField = "iLookupID";
        rdbExcludedBaseStation.DataTextField = "sLookupName";
        rdbExcludedBaseStation.DataBind();
        // Employee Type
        rdbEmployeeType.DataSource = objLookupRepos.GetByLookup("EmployeeType ").OrderBy(o=>o.sLookupName).ToList();
        rdbEmployeeType.DataValueField = "iLookupID";
        rdbEmployeeType.DataTextField = "sLookupName";
        rdbEmployeeType.DataBind();

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

    private void PopulateData(Int64 policyID)
    {

    }

    private void BindPreviewData(Int64 policyID)
    {
        rptPolicyPreviewItem.DataSource = new UniformIssuancePolicyItemRepository().GetUniformIssuancePolicyItems(policyID);
        rptPolicyPreviewItem.DataBind();

    }

    #endregion 
   
}
