using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;

public partial class MyAccount_IssuancePackagePreorder : PageBase
{
    #region Properties
    long PolicyID
    {
        get
        {
            if (Request.QueryString["PID"] == null)
                ViewState["PID"] = 0;
            else
                ViewState["PID"] = Request.QueryString["PID"];

            return Convert.ToInt32(ViewState["PID"]);
        }
        set
        {
            ViewState["PID"] = value;
        }
    }
    long PolicyWorkgroupID
    {
        get
        {
            if (Request.QueryString["PolicyWorkgroupID"] == null)
                ViewState["PolicyWorkgroupID"] = 0;
            else
                ViewState["PolicyWorkgroupID"] = Request.QueryString["PolicyWorkgroupID"];

            return Convert.ToInt32(ViewState["PolicyWorkgroupID"]);
        }
        set
        {
            ViewState["PolicyWorkgroupID"] = value;
        }
    }
    //Created By Ankit for NameBars
    string NameFormat
    {
        get
        {
            if (ViewState["NameFormat"] == null)
            {
                ViewState["NameFormat"] = "";
            }
            return ViewState["NameFormat"].ToString();
        }
        set
        {
            ViewState["NameFormat"] = value;
        }
    }
    string FontFormat
    {
        get
        {
            if (ViewState["FontFormat"] == null)
            {
                ViewState["FontFormat"] = "";
            }
            return ViewState["FontFormat"].ToString();
        }
        set
        {
            ViewState["FontFormat"] = value;
        }
    }
    string FinalNameBarStyle
    {
        get
        {
            if (ViewState["FinalNameBarStyle"] == null)
            {
                ViewState["FinalNameBarStyle"] = "";
            }
            return ViewState["FinalNameBarStyle"].ToString();
        }
        set
        {
            ViewState["FinalNameBarStyle"] = value;
        }
    }
    string FinalNameBarStyleForGroupAssociation
    {
        get
        {
            if (ViewState["FinalNameBarStyleForGroupAssociation"] == null)
            {
                ViewState["FinalNameBarStyleForGroupAssociation"] = "";
            }
            return ViewState["FinalNameBarStyleForGroupAssociation"].ToString();
        }
        set
        {
            ViewState["FinalNameBarStyleForGroupAssociation"] = value;
        }
    }
    string FinalNameBarStyleForBudgetAssociation
    {
        get
        {
            if (ViewState["FinalNameBarStyleForBudgetAssociation"] == null)
            {
                ViewState["FinalNameBarStyleForBudgetAssociation"] = "";
            }
            return ViewState["FinalNameBarStyleForBudgetAssociation"].ToString();
        }
        set
        {
            ViewState["FinalNameBarStyleForBudgetAssociation"] = value;
        }
    }
    string priceleve
    {
        get
        {
            if (ViewState["priceleve"] == null)
            {
                ViewState["priceleve"] = "";
            }
            return ViewState["priceleve"].ToString();
        }
        set
        {
            ViewState["priceleve"] = value;
        }
    }
    string FName = string.Empty;
    string LName = string.Empty;
    Int64? EmployeeTypeID = null;
    Int64? WorkgroupID = null;
    //
    #endregion
    #region Objects/Variables
    UniformIssuancePolicy objPolicy = new UniformIssuancePolicy();
    UniformIssuancePolicyRepository objPolicyRepo = new UniformIssuancePolicyRepository();
    UniformIssuancePolicyItemRepository objPolicyItemRepo = new UniformIssuancePolicyItemRepository();
    UniformIssuancePolicyItemRepository.UniformIssuancePolicyItemResult objPolicyItem = new UniformIssuancePolicyItemRepository.UniformIssuancePolicyItemResult();
    List<UniformIssuancePolicyItem> objPolicyItemResult = new List<UniformIssuancePolicyItem>();
    //List<UniformIssuancePolicyItemRepository.UniformIssuancePolicyItemResult> objPolicyItemResult = new List<UniformIssuancePolicyItemRepository.UniformIssuancePolicyItemResult>();
    MyIssuanceCartRepository objMyIssuanceCartRepo = new MyIssuanceCartRepository();
    List<MyIssuanceCart> objMyIssuanceCart = new List<MyIssuanceCart>();
    LookupRepository objLookupRepo = new LookupRepository();
    INC_Lookup objLook = new INC_Lookup();
    ProductItemDetailsRepository objProdItemDetRepo = new ProductItemDetailsRepository();
    List<ProductItem> objProdItem = new List<ProductItem>();
    CompanyEmployeeRepository objCmpEmpRepo = new CompanyEmployeeRepository();
    CompanyEmployee objCmpEmp = new CompanyEmployee();
    OrderConfirmationRepository objOrderConfirmRepost = new OrderConfirmationRepository();
    List<Order> objOrderlist = new List<Order>();
    bool CompletePurchasedReqiured = false;
    long? IssuanceTypeID = 0;
    long MasterItemID = 0;
    decimal? TotalAmount = 0;
    bool boolMessage = true;
    //string priceleve;
    string Purchaserequired;
    bool boolCkeckPurchasedRequird = false;
    bool boolCkeckInventoryNotallowed = true;
    #endregion
    #region Page Load Event
    protected void Page_Load(object sender, EventArgs e)
   {
        CheckLogin();
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Issuance Package";
            ((Label)Master.FindControl("lblPageHeading")).CssClass="date";
            ((Label)Master.FindControl("lblPageHeading")).Font.Bold=true; 
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/MyIssuancePolicy.aspx";
            if (Request.QueryString["PID"] == null)
            {
                this.PolicyID = 0;
            }
            else
            {
                this.PolicyID = Convert.ToInt64(Request.QueryString["PID"]);
            }
            if (Request.QueryString["PID"]!= null)
            {
                DateTime? HiredDate = null;
                objCmpEmp = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                EmployeeTypeID = objCmpEmp.EmployeeTypeID;
                WorkgroupID = objCmpEmp.WorkgroupID;
                HiredDate = objCmpEmp.HirerdDate;
                objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
                if (objPolicy != null)
                {
                    this.priceleve = objPolicy.PricingLevel;
                    Purchaserequired = Convert.ToString(objPolicy.CompletePurchase);
                    if (objPolicy.CreditExpireDate == null)
                    {
                        lblEligable.Text = Convert.ToString(Convert.ToDateTime(HiredDate).AddMonths(Convert.ToInt32(objPolicy.NumberOfMonths)));
                        lblEligable.ForeColor = System.Drawing.Color.Green;
                        lblEligable.Font.Size = new FontUnit(11);
                        lblEligable.Font.Bold = true;
                        lblExpire.Text = Convert.ToString(Convert.ToDateTime(HiredDate).AddMonths(Convert.ToInt32(objPolicy.CreditExpireNumberOfMonths)));
                        lblExpire.ForeColor = System.Drawing.Color.Red;
                        lblExpire.Font.Size = new FontUnit(11);
                        lblExpire.Font.Bold = true;

                    }
                    else
                    {
                        lblEligable.Text = Convert.ToDateTime(objPolicy.EligibleDate).ToShortDateString();
                        lblExpire.Text = Convert.ToDateTime(objPolicy.CreditExpireDate).ToShortDateString();
                        lblEligable.ForeColor = System.Drawing.Color.Green;
                        lblEligable.Font.Size = new FontUnit(11);
                        lblExpire.ForeColor = System.Drawing.Color.Red;
                        lblExpire.Font.Size = new FontUnit(11);
                        lblExpire.Font.Bold = true;
                        lblEligable.Font.Bold = true;
                    }                   

                }

               

            }
         
            DisplayData();
           

        }
    }
    #endregion
    #region Misc Functions
    protected void DisplayData()
    {
        try
        {
            LoadSingleAssociationData();
            LoadGroupAssociationData1();
            LoadGroupBudgetAssociationData1();
            if (dvSingle.Visible == false && dvGroup.Visible == false && dvGroupBudget.Visible == false)
            {
                dvNoRecord.Visible = true;
                dvShipandCheckout.Visible = false;
            }
            else
            {
                dvNoRecord.Visible = false;
                dvShipandCheckout.Visible = true;

            }
            if (btnCheckOut.Visible == false)
            {
                if (divDateEligible.Visible == false)
                {
                    lblNoRecord.Text = "You have already purchased issuance package.";
                    dvNoRecord.Visible = true;
                }
            }
            else
            {
                lblNoRecord.Text = "";
                dvNoRecord.Visible = false;

                //Check Here For CA/CE Can See Other Workgroup Package.
                //But They can view other issuance packages for the other workgroups but not place orders for the secondary workgroups.
                if (Request.QueryString["PolicyWorkgroupID"] != null)
                {
                    PolicyWorkgroupID = Convert.ToInt64(Request.QueryString["PolicyWorkgroupID"]);

                    if (PolicyWorkgroupID == objCmpEmp.WorkgroupID)
                    {
                        btnCheckOut.Visible = true;
                        lblNoRecord.Text = "";
                        dvNoRecord.Visible = false;
                    }
                    else
                    {
                        btnCheckOut.Visible = false;
                        lblNoRecord.Text = "You can not place orders for the secondary workgroups packages.";
                        dvNoRecord.Visible = true;
                    }

                }



            }




        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
    #region Single Association Type
    protected void LoadSingleAssociationData()
    {
        try
        {
            // Update Chnage Nagmani 02-02-2012
            string[] MyWeatherlist = null;
            List<UniformIssuancePolicyItem> objMainList = new List<UniformIssuancePolicyItem>();
            List<SelectWeatherTypeIdResult> objWeatherlsit = new List<SelectWeatherTypeIdResult>();
            //Select Basestation and Employeetype Id from table companyemployee on login userid
            int intEmployeeType = 0;
            long logBaseStation = 0;
            string strweather = "0";
            objCmpEmp = objCmpEmpRepo.GetByUserInfoId(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
            if (objCmpEmp != null)
            {
                intEmployeeType = Convert.ToInt32(objCmpEmp.EmployeeTypeID);
                logBaseStation = Convert.ToInt64(objCmpEmp.BaseStation);
                IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Single Item Association");
                if (objCmpEmp.EmployeeTypeID != null && objCmpEmp.BaseStation != null)
                {

                    //Select Weather from InceBaseStation using basestation parameter
                    objWeatherlsit = objPolicyItemRepo.GetWeatherTypeid(Convert.ToInt32(logBaseStation));
                    if (objWeatherlsit.Count > 0)
                    {
                        if (objWeatherlsit[0].WEATHERTYPE != null)
                        {
                            MyWeatherlist = objWeatherlsit[0].WEATHERTYPE.Split(',');

                            for (int i = 0; i < MyWeatherlist.Count(); i++)
                            {
                                if (i == 0)
                                {

                                    strweather = "'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                }
                                else
                                {

                                    strweather += ",'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                }

                            }

                            objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();
                            //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0)).ToList();

                        }
                        else
                        {
                            MyWeatherlist = null;

                            //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();
                        }
                    }
                    else
                    {
                        MyWeatherlist = null;
                        //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();
                    }



                }

                else if (objCmpEmp.EmployeeTypeID == null && objCmpEmp.BaseStation != null)
                {
                    intEmployeeType = 0;
                    logBaseStation = Convert.ToInt64(objCmpEmp.BaseStation);

                    //Select Weather from InceBaseStation using basestation parameter
                    objWeatherlsit = objPolicyItemRepo.GetWeatherTypeid(Convert.ToInt32(logBaseStation));
                    if (objWeatherlsit.Count > 0)
                    {
                        if (objWeatherlsit[0].WEATHERTYPE != null)
                        {
                            MyWeatherlist = objWeatherlsit[0].WEATHERTYPE.Split(',');

                            for (int i = 0; i < MyWeatherlist.Count(); i++)
                            {
                                if (i == 0)
                                {

                                    strweather = "'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                }
                                else
                                {

                                    strweather += ",'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                }

                            }

                            objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == 0)).ToList();
                            // objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0)).ToList();

                        }
                        else
                        {
                            MyWeatherlist = null;

                            //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();
                        }
                    }
                    else
                    {
                        MyWeatherlist = null;
                        //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();
                    }

                }

            }
            else
            {
                // logEmployeeType = 0;
                logBaseStation = 0;
            }

            // IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Single Item Association");
            //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(i => i.AssociationIssuanceType == IssuanceTypeID).ToList();
            //End Nagmani 

            objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
            if (objPolicy != null)
                priceleve = (objPolicy.PricingLevel).Trim();
            if (objPolicyItemResult.Count > 0)
            {
                objPolicy = objPolicyRepo.GetById(this.PolicyID);

                if (objPolicy != null)
                {
                    // Calculate by Month
                    if (objPolicy.IsDateOfHiredTicked == true)
                    {
                        DateTime? dtHiredDate, dtEligbleDate, dtExpiryDate;

                        objCmpEmp = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                        dtHiredDate = objCmpEmp.HirerdDate;
                        dtEligbleDate = dtHiredDate.Value.AddMonths(int.Parse(objPolicy.NumberOfMonths.ToString()));
                        dtExpiryDate = dtEligbleDate.Value.AddMonths(int.Parse(objPolicy.CreditExpireNumberOfMonths.ToString()));

                        if ((dtEligbleDate <= DateTime.Now || dtEligbleDate > DateTime.Now) && dtExpiryDate >= DateTime.Now)
                        {
                            dvSingle.Visible = true;
                            pnlSingle.Visible = true;
                            for (int i = 0; i < objPolicyItemResult.Count; i++)
                            {
                                if (objPolicyItemResult[i].AssociationIssuancePolicyNote != "")
                                {
                                    lblSingleItemAssociation.Text = objPolicyItemResult[i].AssociationIssuancePolicyNote;
                                }
                            }

                            rptSingleAssociation.DataSource = objPolicyItemResult;
                            rptSingleAssociation.DataBind();

                            foreach (RepeaterItem item in rptSingleAssociation.Items)
                            {
                                #region Load Issuance and Balance
                                TextBox txtQty = (TextBox)item.FindControl("txtQty");
                                Label lblIssuance = new Label();
                                lblIssuance = (Label)item.FindControl("lblIssuance");
                                lblIssuance.Text = objPolicyItemResult[item.ItemIndex].Issuance.ToString();
                                HiddenField hdnStoreProductid = (HiddenField)item.FindControl("hdnStoreProductid");
                                Label lblBalance = new Label();
                                lblBalance = (Label)item.FindControl("lblBalance");
                                Label lblPurchase = new Label();
                                lblPurchase = (Label)item.FindControl("lblPurchase");
                                objMyIssuanceCart = objMyIssuanceCartRepo.GetByPolicyItemId(objPolicyItemResult[item.ItemIndex].UniformIssuancePolicyItemID, IncentexGlobal.CurrentMember.UserInfoID);

                                if (objMyIssuanceCart != null)
                                {
                                    int? Qty = 0;

                                    for (int i = 0; i < objMyIssuanceCart.Count; i++)
                                    {
                                        if (objMyIssuanceCart[i].OrderStatus == "Submitted")
                                        {
                                            Qty += objMyIssuanceCart[i].Qty;
                                        }
                                    }
                                    lblBalance.Text = (objPolicyItemResult[item.ItemIndex].Issuance - Qty).ToString();
                                    lblPurchase.Text = Convert.ToString((objPolicyItemResult[item.ItemIndex].Issuance - (Convert.ToInt32(lblBalance.Text))));
                                    if (int.Parse(lblIssuance.Text) <= int.Parse(lblPurchase.Text))
                                    {
                                        lblBalance.Text = "0";
                                    }
                                }
                                else
                                {
                                    lblBalance.Text = objPolicyItemResult[item.ItemIndex].Issuance.ToString();
                                    lblPurchase.Text = objPolicyItemResult[item.ItemIndex].Issuance.ToString();
                                }
                                //Saurabh--Check for Partial Issuance Status
                                CompanyEmployee objEmpType = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                                if (objEmpType.IssuancePolicyStatus == 'P')
                                {
                                    objOrderlist = objOrderConfirmRepost.CheckUserIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                                }
                                else
                                {
                                    objOrderlist = objOrderConfirmRepost.CheckUserIDWorkgroupIDEmpIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.EmployeeTypeID,this.WorkgroupID);
                                }
                                if (objOrderlist.Count > 0)
                                {
                                    if (Convert.ToDecimal(lblBalance.Text) == 0)
                                    {
                                        txtQty.Enabled = false;

                                    }
                                    else
                                    {
                                        txtQty.Enabled = true;

                                    }
                                }
                                else
                                {
                                    lblBalance.Text = lblIssuance.Text;
                                    lblPurchase.Text = "0";
                                    txtQty.Enabled = true;
                                }

                                //End Check Here If User Ordered Has Canceled 28-July-2011
                                #endregion
                                #region Size Dropdown Fill
                                MasterItemID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId);
                                objProdItem = objProdItemDetRepo.GetAllProductItem((int)MasterItemID);
                                #region Ankit Updated on 10 Feb For Name Bars
                                string Stylename = objLookupRepo.GetById(objProdItem[0].ProductStyleID).sLookupName.ToString();
                                string ProdcutStyleName = objLookupRepo.GetById(objProdItem[0].ItemSizeID).sLookupName.ToString();
                                if (Stylename == "Name Bars")
                                {
                                    if (ProdcutStyleName == "No Size")
                                    {

                                        this.NameFormat = objProdItem[0].NameFormatForNameBars;
                                        this.FontFormat = objProdItem[0].FontFormatForNameBars;
                                        this.FName = IncentexGlobal.CurrentMember.FirstName;
                                        this.LName = IncentexGlobal.CurrentMember.LastName;
                                        this.FinalNameBarStyle = SetNameFontFormat();

                                        /*Employee Title added on 16 Feb :( */
                                        CompanyEmployee objCompanyEmployee = new CompanyEmployee();
                                        CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
                                        LookupRepository objLookupRepoForTitle = new LookupRepository();
                                        objCompanyEmployee = objEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                                        if (objCompanyEmployee.EmployeeTitleId != null)
                                            this.FinalNameBarStyle = this.FinalNameBarStyle + "," + objLookupRepoForTitle.GetById((long)objCompanyEmployee.EmployeeTitleId).sLookupName.ToString();

                                        /*End*/

                                        Panel pnlNameBarsForSingleAssociation = new Panel();
                                        pnlNameBarsForSingleAssociation = (Panel)item.FindControl("pnlNameBarsForSingleAssociation");
                                        if (this.FinalNameBarStyle != string.Empty)
                                        {
                                            pnlNameBarsForSingleAssociation.Visible = true;
                                            if (this.FinalNameBarStyle.Contains(','))
                                            {

                                                string[] EmployeeTitle = (this.FinalNameBarStyle.Split(','));
                                                ((Label)item.FindControl("lblNameTobeEngravedForSingleAssociation")).Text = EmployeeTitle[0];
                                                ((Label)item.FindControl("lblEmplTitleForSingleAssociation")).Text = EmployeeTitle[1];
                                            }
                                            else
                                            {
                                                ((Label)item.FindControl("lblNameTobeEngravedForSingleAssociation")).Text = this.FinalNameBarStyle;
                                            }
                                        }
                                        else
                                        {
                                            pnlNameBarsForSingleAssociation.Visible = false;

                                        }

                                    }
                                }

                                #endregion

                                DropDownList ddlSize = new DropDownList();
                                ddlSize = (DropDownList)item.FindControl("ddlSize");
                                //Add Nagmani 16 Nov 2011
                                string ItemNumberSatus = null;
                                //End Nagmani 16 Nov 2011
                                var SizeList = (from s in objProdItem
                                                where s.ProductId == Convert.ToInt64(hdnStoreProductid.Value)
                                                select new { s.ItemSizeID, s.ItemNumberStatusID }).OrderBy(s => s.ItemSizeID).Distinct().ToList();
                                // foreach (var prod in objProdItem)
                                foreach (var prod in SizeList)
                                {

                                    //Add Nagmani 16 Nov 2011
                                    ItemNumberSatus = Convert.ToString(objLookupRepo.GetById(Convert.ToInt32(prod.ItemNumberStatusID)).sLookupName);
                                    //End Nagmani 16 Nov 2011
                                    if (ItemNumberSatus != "InActive")
                                    {
                                        ddlSize.Items.Add(new ListItem((objLookupRepo.GetById(prod.ItemSizeID).sLookupName).ToString(), (objLookupRepo.GetById(prod.ItemSizeID).iLookupID).ToString()));
                                    }


                                }
                                // Fill Master Item Nuber
                                MasterItemID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId);

                                LookupRepository oblLookRepos = new LookupRepository();
                                INC_Lookup objlook = new INC_Lookup();
                                objlook = oblLookRepos.GetById(MasterItemID);
                                Label lblItemNumber = (Label)item.FindControl("lblItemNumber");
                                lblItemNumber.Text = objlook.sLookupName;
                                //Add Nagmani ItemNumber 4 july 2011
                                Boolean boolTrue = false;
                                HiddenField hdnItemNumber = (HiddenField)item.FindControl("hdnItemNumber");
                                List<SELECTITEMNUMBERONSIZEMASTERNOResult> OBLISTITME = new List<SELECTITEMNUMBERONSIZEMASTERNOResult>();
                                UniformIssuancePolicyItemRepository OBJREPOSTITEM = new UniformIssuancePolicyItemRepository();
                                OBLISTITME = OBJREPOSTITEM.GETITEMNUMBER(Convert.ToInt32(MasterItemID), Convert.ToInt32(ddlSize.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));
                                if (OBLISTITME.Count > 0)
                                {
                                    for (int i = 0; i < OBLISTITME.Count; i++)
                                    {
                                        if (boolTrue != true)
                                        {
                                            hdnItemNumber.Value = OBLISTITME[i].ITEMNUMBER.ToString();
                                            boolTrue = true;
                                        }
                                    }
                                }

                                //End Nagmani ItemNumber 4 july 2011

                                //Added by Surendar Yadav 4 December 2012
                                HyperLink lnkSizingChart = (HyperLink)item.FindControl("lnkSizingChart");
                                TailoringMeasurementsRepository objRep = new TailoringMeasurementsRepository();
                                List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
                                objCount = objRep.GetTailoringMeasurementByStoreProductIDAndMasterItemID(Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId),Convert.ToInt64(objPolicyItemResult[item.ItemIndex].StoreProductid));
                                if (objCount.Count > 0)
                                {
                                    if (objCount[0].TailoringMeasurementChart != "")
                                    {
                                        lnkSizingChart.Visible = true;
                                        lnkSizingChart.NavigateUrl = "../UploadedImages/TailoringMeasurement/" + objCount[0].TailoringMeasurementChart;
                                    }
                                    else
                                    {
                                        lnkSizingChart.Visible = false;
                                        lnkSizingChart.NavigateUrl = "";
                                    }
                                }
                                else
                                {
                                    lnkSizingChart.Visible = false;
                                    lnkSizingChart.NavigateUrl = "";
                                }
                                //End by Surendar Yadav 4 December 2012


                                //Used method for Display Price
                                List<ProductItemPricing> objPricing = new List<ProductItemPricing>();

                                objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(MasterItemID, long.Parse(ddlSize.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                                if (objPricing.Count > 0)
                                {
                                    Label lblPrice = (Label)item.FindControl("lblPrice");
                                    if (priceleve == "1")
                                    {
                                        lblPrice.Text = Convert.ToString(objPricing[0].Level1);
                                    }
                                    else if (priceleve == "2")
                                    {
                                        lblPrice.Text = Convert.ToString(objPricing[0].Level2);
                                    }
                                    else
                                    {
                                        lblPrice.Text = Convert.ToString(objPricing[0].Level3);
                                    }

                                }



                                // CHECK TAILORING OPTION TRUE FALASE
                                MasterItemID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId);
                                Panel pnlTailoring = (Panel)item.FindControl("pnlTailoring");
                                List<SelectTailoringOptionActiveInActiveResult> objTailoring;
                                objTailoring = objPolicyItemRepo.GetTailoringActiveInActive(Convert.ToInt32(MasterItemID), int.Parse(ddlSize.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));

                                if (objTailoring.Count > 0)
                                {
                                    if (objTailoring[0].sLookupName == "Active")
                                    {
                                        if (!string.IsNullOrEmpty(objTailoring[0].TailoringRunCharge))
                                        {
                                            ((Label)item.FindControl("lblRunCharge")).Text = objTailoring[0].TailoringRunCharge.ToString();
                                        }
                                        pnlTailoring.Visible = true;
                                    }
                                    else
                                    {
                                        pnlTailoring.Visible = false;
                                    }
                                }


                                #endregion
                            }



                            //Start Modify Nagmani 06-May-2011
                            //Check Here This User Has Taken any Issuance Policy the He can not take other issuance policy
                            objOrderlist = objOrderConfirmRepost.CheckUserIDWorkgroupIDEmpIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.EmployeeTypeID,this.WorkgroupID);
                            if (objOrderlist.Count > 0)
                            {
                                btnCheckOut.Visible = false;
                            }
                            else
                            {
                                if (dtEligbleDate > DateTime.Now && dtExpiryDate >= DateTime.Now)
                                {
                                    btnCheckOut.Visible = false;
                                    divDateEligible.Visible = true;
                                    lblDateEligible.Text = "Your Policy Is Not Yet Eligible:" + " " + lblEligable.Text;
                                }
                                else
                                {
                                    btnCheckOut.Visible = true;
                                    lblDateEligible.Text = "";
                                    divDateEligible.Visible = false;
                                }
                            }

                        }
                        else
                        {
                            dvSingle.Visible = false;
                            pnlSingle.Visible = false;
                        }
                    }
                    // Calculate by Date
                    else
                    {
                        if (objPolicy.EligibleDate != null)
                        {
                            if ((objPolicy.EligibleDate <= DateTime.Now || objPolicy.EligibleDate > DateTime.Now) && objPolicy.CreditExpireDate >= DateTime.Now)
                            {
                                dvSingle.Visible = true;
                                pnlSingle.Visible = true;
                                for (int i = 0; i < objPolicyItemResult.Count; i++)
                                {
                                    if (objPolicyItemResult[i].AssociationIssuancePolicyNote != "")
                                    {
                                        lblSingleItemAssociation.Text = objPolicyItemResult[i].AssociationIssuancePolicyNote;
                                    }
                                }
                                rptSingleAssociation.DataSource = objPolicyItemResult;
                                rptSingleAssociation.DataBind();

                                foreach (RepeaterItem item in rptSingleAssociation.Items)
                                {
                                    #region Load Issuance and Balance
                                    HiddenField hdnStoreProductid = (HiddenField)item.FindControl("hdnStoreProductid");
                                    TextBox txtQty = (TextBox)item.FindControl("txtQty");
                                    Label lblIssuance = new Label();
                                    lblIssuance = (Label)item.FindControl("lblIssuance");
                                    lblIssuance.Text = objPolicyItemResult[item.ItemIndex].Issuance.ToString();

                                    Label lblBalance = new Label();
                                    lblBalance = (Label)item.FindControl("lblBalance");
                                    Label lblPurchase = new Label();
                                    lblPurchase = (Label)item.FindControl("lblPurchase");
                                    objMyIssuanceCart = objMyIssuanceCartRepo.GetByPolicyItemId(objPolicyItemResult[item.ItemIndex].UniformIssuancePolicyItemID, IncentexGlobal.CurrentMember.UserInfoID);

                                    if (objMyIssuanceCart != null)
                                    {
                                        int? Qty = 0;

                                        for (int i = 0; i < objMyIssuanceCart.Count; i++)
                                        {
                                            if (objMyIssuanceCart[i].OrderStatus == "Submitted")
                                            {
                                                Qty += objMyIssuanceCart[i].Qty;
                                            }
                                        }
                                        lblBalance.Text = (objPolicyItemResult[item.ItemIndex].Issuance - Qty).ToString();
                                        lblPurchase.Text = Convert.ToString((objPolicyItemResult[item.ItemIndex].Issuance - (Convert.ToInt32(lblBalance.Text))));
                                        if (int.Parse(lblIssuance.Text) <= int.Parse(lblPurchase.Text))
                                        {
                                            lblBalance.Text = "0";
                                        }
                                    }
                                    else
                                    {
                                        lblBalance.Text = objPolicyItemResult[item.ItemIndex].Issuance.ToString();
                                        lblPurchase.Text = objPolicyItemResult[item.ItemIndex].Issuance.ToString();
                                    }
                                    //Saurabh--Check for Partial Issuance Status
                                    CompanyEmployee objEmpType = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                                    if (objEmpType.IssuancePolicyStatus == 'P')
                                    {
                                        objOrderlist = objOrderConfirmRepost.CheckUserIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                                    }
                                    else
                                    {
                                        objOrderlist = objOrderConfirmRepost.CheckUserIDWorkgroupIDEmpIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.EmployeeTypeID,this.WorkgroupID);
                                    }
                                    if (objOrderlist.Count > 0)
                                    {
                                        if (Convert.ToDecimal(lblBalance.Text) == 0)
                                        {
                                            txtQty.Enabled = false;

                                        }
                                        else
                                        {
                                            txtQty.Enabled = true;

                                        }
                                    }
                                    else
                                    {
                                        lblBalance.Text = lblIssuance.Text;
                                        lblPurchase.Text = "0";
                                        txtQty.Enabled = true;
                                    }

                                    //End Check Here If User Ordered Has Canceled 28-July-2011





                                    #endregion

                                    #region Size Dropdown Fill

                                    MasterItemID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId);
                                    objProdItem = objProdItemDetRepo.GetAllProductItem((int)MasterItemID);

                                    #region Ankit Updated on 10 Feb For Name Bars

                                    string Stylename = objLookupRepo.GetById(objProdItem[0].ProductStyleID).sLookupName.ToString();

                                    string ProdcutStyleName = objLookupRepo.GetById(objProdItem[0].ItemSizeID).sLookupName.ToString();
                                    if (Stylename == "Name Bars")
                                    {
                                        if (ProdcutStyleName == "No Size")
                                        {
                                            this.NameFormat = objProdItem[0].NameFormatForNameBars;
                                            this.FontFormat = objProdItem[0].FontFormatForNameBars;
                                            this.FName = IncentexGlobal.CurrentMember.FirstName;
                                            this.LName = IncentexGlobal.CurrentMember.LastName;
                                            this.FinalNameBarStyle = SetNameFontFormat();

                                            /*Employee Title added on 16 Feb :( */
                                            CompanyEmployee objCompanyEmployee = new CompanyEmployee();
                                            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
                                            LookupRepository objLookupRepoForTitle = new LookupRepository();
                                            objCompanyEmployee = objEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                                            if (objCompanyEmployee.EmployeeTitleId != null)
                                                this.FinalNameBarStyle = this.FinalNameBarStyle + "," + objLookupRepoForTitle.GetById((long)objCompanyEmployee.EmployeeTitleId).sLookupName.ToString();

                                            /*End*/

                                            Panel pnlNameBarsForSingleAssociation = new Panel();
                                            pnlNameBarsForSingleAssociation = (Panel)item.FindControl("pnlNameBarsForSingleAssociation");
                                            if (this.FinalNameBarStyle != string.Empty)
                                            {
                                                pnlNameBarsForSingleAssociation.Visible = true;
                                                if (this.FinalNameBarStyle.Contains(','))
                                                {

                                                    string[] EmployeeTitle = (this.FinalNameBarStyle.Split(','));
                                                    ((Label)item.FindControl("lblNameTobeEngravedForSingleAssociation")).Text = EmployeeTitle[0];
                                                    ((Label)item.FindControl("lblEmplTitleForSingleAssociation")).Text = EmployeeTitle[1];
                                                }
                                                else
                                                {
                                                    ((Label)item.FindControl("lblNameTobeEngravedForSingleAssociation")).Text = this.FinalNameBarStyle;
                                                }
                                            }
                                            else
                                            {
                                                pnlNameBarsForSingleAssociation.Visible = false;

                                            }

                                        }
                                    }

                                    #endregion

                                    DropDownList ddlSize = new DropDownList();
                                    ddlSize = (DropDownList)item.FindControl("ddlSize");

                                    //Add Nagmani 16 Nov 2011
                                    string ItemNumberSatus = null;
                                    //End Nagmani 16 Nov 2011
                                    var SizeList = (from s in objProdItem
                                                    where s.ProductId == Convert.ToInt64(hdnStoreProductid.Value)
                                                    select new { s.ItemSizeID, s.ItemNumberStatusID }).OrderBy(s => s.ItemSizeID).Distinct().ToList();
                                    // foreach (var prod in objProdItem)
                                    foreach (var prod in SizeList)
                                    {

                                        //Add Nagmani 16 Nov 2011
                                        ItemNumberSatus = Convert.ToString(objLookupRepo.GetById(Convert.ToInt32(prod.ItemNumberStatusID)).sLookupName);
                                        //End Nagmani 16 Nov 2011
                                        if (ItemNumberSatus != "InActive")
                                        {
                                            ddlSize.Items.Add(new ListItem((objLookupRepo.GetById(prod.ItemSizeID).sLookupName).ToString(), (objLookupRepo.GetById(prod.ItemSizeID).iLookupID).ToString()));
                                        }
                                    }


                                    // Fill Master Item Nuber
                                    MasterItemID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId);
                                    LookupRepository oblLookRepos = new LookupRepository();
                                    INC_Lookup objlook = new INC_Lookup();
                                    objlook = oblLookRepos.GetById(MasterItemID);
                                    Label lblItemNumber = (Label)item.FindControl("lblItemNumber");
                                    lblItemNumber.Text = objlook.sLookupName;
                                    //Add Nagmani ItemNumber 4 july 2011
                                    Boolean boolTrue = false;
                                    HiddenField hdnItemNumber = (HiddenField)item.FindControl("hdnItemNumber");
                                    List<SELECTITEMNUMBERONSIZEMASTERNOResult> OBLISTITME = new List<SELECTITEMNUMBERONSIZEMASTERNOResult>();
                                    UniformIssuancePolicyItemRepository OBJREPOSTITEM = new UniformIssuancePolicyItemRepository();
                                    OBLISTITME = OBJREPOSTITEM.GETITEMNUMBER(Convert.ToInt32(MasterItemID), Convert.ToInt32(ddlSize.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));
                                    if (OBLISTITME.Count > 0)
                                    {
                                        for (int i = 0; i < OBLISTITME.Count; i++)
                                        {
                                            if (boolTrue != true)
                                            {
                                                hdnItemNumber.Value = OBLISTITME[i].ITEMNUMBER.ToString();
                                                boolTrue = true;
                                            }
                                        }
                                    }

                                    //End Nagmani ItemNumber 4 july 2011
                                    //Added by Surendar Yadav 4 December 2012
                                    HyperLink lnkSizingChart = (HyperLink)item.FindControl("lnkSizingChart");
                                    TailoringMeasurementsRepository objRep = new TailoringMeasurementsRepository();
                                    List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
                                    objCount = objRep.GetTailoringMeasurementByStoreProductIDAndMasterItemID(Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId), Convert.ToInt64(objPolicyItemResult[item.ItemIndex].StoreProductid));
                                    if (objCount.Count > 0)
                                    {
                                        if (objCount[0].TailoringMeasurementChart != "")
                                        {
                                            lnkSizingChart.Visible = true;
                                            lnkSizingChart.NavigateUrl = "../UploadedImages/TailoringMeasurement/" + objCount[0].TailoringMeasurementChart;
                                        }
                                        else
                                        {
                                            lnkSizingChart.Visible = false;
                                            lnkSizingChart.NavigateUrl = "";
                                        }
                                    }
                                    else
                                    {
                                        lnkSizingChart.Visible = false;
                                        lnkSizingChart.NavigateUrl = "";
                                    }
                                    //End by Surendar Yadav 4 December 2012
                                    //Used method for Display Price
                                    List<ProductItemPricing> objPricing = new List<ProductItemPricing>();

                                    objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(MasterItemID, long.Parse(ddlSize.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                                    if (objPricing.Count > 0)
                                    {
                                        Label lblPrice = (Label)item.FindControl("lblPrice");
                                        if (priceleve == "1")
                                        {
                                            lblPrice.Text = Convert.ToString(objPricing[0].Level1);
                                        }
                                        else if (priceleve == "2")
                                        {
                                            lblPrice.Text = Convert.ToString(objPricing[0].Level2);
                                        }
                                        else
                                        {
                                            lblPrice.Text = Convert.ToString(objPricing[0].Level3);
                                        }

                                    }

                                    // CHECK TAILORING OPTION TRUE FALSE
                                    MasterItemID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId);
                                    Panel pnlTailoring = (Panel)item.FindControl("pnlTailoring");
                                    List<SelectTailoringOptionActiveInActiveResult> objTailoring;
                                    objTailoring = objPolicyItemRepo.GetTailoringActiveInActive(Convert.ToInt32(MasterItemID), int.Parse(ddlSize.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));

                                    if (objTailoring.Count > 0)
                                    {
                                        if (objTailoring[0].sLookupName == "Active")
                                        {
                                            if (!string.IsNullOrEmpty(objTailoring[0].TailoringRunCharge))
                                            {
                                                ((Label)item.FindControl("lblRunCharge")).Text = objTailoring[0].TailoringRunCharge.ToString();
                                            }
                                            pnlTailoring.Visible = true;
                                        }
                                        else
                                        {
                                            pnlTailoring.Visible = false;
                                        }
                                    }

                                    #endregion
                                }



                                //Start Modify Nagmani 06-May-2011
                                //Check Here This User Has Taken any Issuance Policy the He can not take other issuance policy
                                objOrderlist = objOrderConfirmRepost.CheckUserIDWorkgroupIDEmpIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.EmployeeTypeID,this.WorkgroupID);
                                if (objOrderlist.Count > 0)
                                {
                                    btnCheckOut.Visible = false;
                                }
                                else
                                {
                                    if (objPolicy.EligibleDate > DateTime.Now && objPolicy.CreditExpireDate >= DateTime.Now)
                                    {
                                        btnCheckOut.Visible = false;
                                        divDateEligible.Visible = true;
                                        lblDateEligible.Text = "Your Policy Is Not Yet Eligible:" + " " + lblEligable.Text;
                                    }
                                    else
                                    {
                                        btnCheckOut.Visible = true;
                                        lblDateEligible.Text = "";
                                        divDateEligible.Visible = false;
                                    }
                                }


                            }
                            else
                            {
                                dvSingle.Visible = false;
                                pnlSingle.Visible = false;
                            }

                        }
                        else
                        {
                            dvSingle.Visible = false;
                            pnlSingle.Visible = false;
                        }
                    }
                }
                else
                {
                    dvSingle.Visible = false;
                    pnlSingle.Visible = false;
                }
            }
            else
            {
                dvSingle.Visible = false;
                pnlSingle.Visible = false;
            }


        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
    //Below function is created By Ankit on 16th Feb
    private string SetNameFontFormat()
    {
        string FinalNameFormat = string.Empty;
        if (this.NameFormat.ToString() != string.Empty)
        {
            if (this.NameFormat == "First Name")
            {
                if (this.FontFormat == "All Caps")
                {
                    FinalNameFormat = this.FName.ToUpper();
                }
                else if (this.FontFormat == "Upper and Lower Case")
                {
                    //txtExample.Text = ConvertToPascalCase(FName);
                    FinalNameFormat = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.FName.ToLower());
                }
                else
                {
                    FinalNameFormat = this.FName;
                }
            }
            else if (this.NameFormat == "Last Name")
            {
                if (this.FontFormat == "All Caps")
                {
                    FinalNameFormat = this.LName.ToUpper();
                }
                else if (this.FontFormat == "Upper and Lower Case")
                {
                    FinalNameFormat = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.LName.ToLower());
                }
                else
                {
                    FinalNameFormat = this.LName;
                }

            }
            else if (this.NameFormat == "First Initial.LastName")
            {
                if (this.FontFormat == "All Caps")
                {
                    FinalNameFormat = (this.FName.Substring(0, 1).ToString() + "." + this.LName.ToString()).ToUpper();
                }
                else if (this.FontFormat == "Upper and Lower Case")
                {
                    FinalNameFormat = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.FName.Substring(0, 1).ToString() + "." + LName.ToString().ToLower());
                }
                else
                {
                    FinalNameFormat = (this.FName.Substring(0, 1).ToString() + "." + this.LName.ToString()).ToUpper();
                }

            }
        }
        else
        {
            if (this.FontFormat == "All Caps")
            {
                FinalNameFormat = (this.FName + " " + this.LName).ToUpper();
            }
            else if (this.FontFormat == "Upper and Lower Case")
            {

                FinalNameFormat = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase((this.FName + " " + this.LName));
            }
            else
            {
                FinalNameFormat = (this.FName + " " + this.LName);
            }
        }
        return FinalNameFormat;
    }
    //End
    protected void ProcessSingleAssociation()
    {
        try
        {
            decimal? SingleTotal = 0;

            StringBuilder sbSingleCartIDs = new StringBuilder();
            sbSingleCartIDs.Append(string.Empty);

            if (dvSingle.Visible == true && pnlSingle.Visible == true)
            {
                decimal? rate = 0;

                IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Single Item Association");
               
                // Add New
                
               // objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(i => i.AssociationIssuanceType == IssuanceTypeID).ToList();
                // Update Chnage Nagmani 02-02-2012
                string[] MyWeatherlist = null;
                List<UniformIssuancePolicyItem> objMainList = new List<UniformIssuancePolicyItem>();
                List<SelectWeatherTypeIdResult> objWeatherlsit = new List<SelectWeatherTypeIdResult>();
                //Select Basestation and Employeetype Id from table companyemployee on login userid
                int intEmployeeType = 0;
                long logBaseStation = 0;
                string strweather = "0";
                objCmpEmp = objCmpEmpRepo.GetByUserInfoId(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                if (objCmpEmp != null)
                {
                    intEmployeeType = Convert.ToInt32(objCmpEmp.EmployeeTypeID);
                    logBaseStation = Convert.ToInt64(objCmpEmp.BaseStation);
                    IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Single Item Association");
                    if (objCmpEmp.EmployeeTypeID != null && objCmpEmp.BaseStation != null)
                    {

                        //Select Weather from InceBaseStation using basestation parameter
                        objWeatherlsit = objPolicyItemRepo.GetWeatherTypeid(Convert.ToInt32(logBaseStation));
                        if (objWeatherlsit.Count > 0)
                        {
                            if (objWeatherlsit[0].WEATHERTYPE != null)
                            {
                                MyWeatherlist = objWeatherlsit[0].WEATHERTYPE.Split(',');

                                for (int i = 0; i < MyWeatherlist.Count(); i++)
                                {
                                    if (i == 0)
                                    {

                                        strweather = "'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }
                                    else
                                    {

                                        strweather += ",'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }

                                }

                                objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();
                                //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0)).ToList();

                            }
                            else
                            {
                                MyWeatherlist = null;

                                //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();
                            }
                        }
                        else
                        {
                            MyWeatherlist = null;
                            //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();
                        }



                    }

                    else if (objCmpEmp.EmployeeTypeID == null && objCmpEmp.BaseStation != null)
                    {
                        intEmployeeType = 0;
                        logBaseStation = Convert.ToInt64(objCmpEmp.BaseStation);

                        //Select Weather from InceBaseStation using basestation parameter
                        objWeatherlsit = objPolicyItemRepo.GetWeatherTypeid(Convert.ToInt32(logBaseStation));
                        if (objWeatherlsit.Count > 0)
                        {
                            if (objWeatherlsit[0].WEATHERTYPE != null)
                            {
                                MyWeatherlist = objWeatherlsit[0].WEATHERTYPE.Split(',');

                                for (int i = 0; i < MyWeatherlist.Count(); i++)
                                {
                                    if (i == 0)
                                    {

                                        strweather = "'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }
                                    else
                                    {

                                        strweather += ",'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }

                                }

                                objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == 0)).ToList();
                                // objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0)).ToList();

                            }
                            else
                            {
                                MyWeatherlist = null;

                                //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();
                            }
                        }
                        else
                        {
                            MyWeatherlist = null;
                            //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();
                        }

                    }

                }
                else
                {
                    
                    logBaseStation = 0;
                }

                //End Nagmani 
                //End New 
                objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
                Purchaserequired = Convert.ToString(objPolicy.CompletePurchase);
                if (objPolicyItemResult.Count > 0)
                {
                    foreach (RepeaterItem item in rptSingleAssociation.Items)
                    {
                        HiddenField hdnStoreProductid = (HiddenField)item.FindControl("hdnStoreProductid");
                        string NameToBeEngravedForSingle = string.Empty;
                        Label lblIssuance = new Label();
                        lblIssuance = (Label)item.FindControl("lblIssuance");
                        Label lblPrice = (Label)item.FindControl("lblPrice");
                        Label lblBalance = new Label();
                        lblBalance = (Label)item.FindControl("lblBalance");
                        TextBox txtQty = new TextBox();
                        txtQty = (TextBox)item.FindControl("txtQty");
                        DropDownList ddlSize = new DropDownList();
                        ddlSize = (DropDownList)item.FindControl("ddlSize");
                        // Add Tailoring Length
                        TextBox txtTailoring = (TextBox)item.FindControl("txtSingleTailoringlenght");
                        Label lblItemNumber = (Label)item.FindControl("lblItemNumber");
                        //Add Nagmani ItemNumber 4 july 2011
                        HiddenField hdnItemNumber = (HiddenField)item.FindControl("hdnItemNumber");
                         //End Nagmani ItemNumber 4 july 2011
                        //Added by Surendar Yadav 4 December 2012
                        HyperLink lnkSizingChart = (HyperLink)item.FindControl("lnkSizingChart");
                        TailoringMeasurementsRepository objRep = new TailoringMeasurementsRepository();
                        List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
                        objCount = objRep.GetTailoringMeasurementByStoreProductIDAndMasterItemID(Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId), Convert.ToInt64(objPolicyItemResult[item.ItemIndex].StoreProductid));
                        if (objCount.Count > 0)
                        {
                            if (objCount[0].TailoringMeasurementChart != "")
                            {
                                lnkSizingChart.Visible = true;
                                lnkSizingChart.NavigateUrl = "../UploadedImages/TailoringMeasurement/" + objCount[0].TailoringMeasurementChart;
                            }
                            else
                            {
                                lnkSizingChart.Visible = false;
                                lnkSizingChart.NavigateUrl = "";
                            }
                        }
                        else
                        {
                            lnkSizingChart.Visible = false;
                            lnkSizingChart.NavigateUrl = "";
                        }
                        //End by Surendar Yadav 4 December 2012
                        //Created By Ankit For NameBars
                        if (Convert.ToString(((Label)item.FindControl("lblNameTobeEngravedForSingleAssociation")).Text) != string.Empty)
                        {
                            if (Convert.ToString(((Label)item.FindControl("lblEmplTitleForSingleAssociation")).Text) != string.Empty)
                            {
                                NameToBeEngravedForSingle = Convert.ToString(((Label)item.FindControl("lblNameTobeEngravedForSingleAssociation")).Text) + "," + Convert.ToString(((Label)item.FindControl("lblEmplTitleForSingleAssociation")).Text);
                            }
                            else
                            {
                                NameToBeEngravedForSingle = Convert.ToString(((Label)item.FindControl("lblNameTobeEngravedForSingleAssociation")).Text);
                            }
                        }
                        else
                        {
                            NameToBeEngravedForSingle = null;
                        }

                        //End

                        if (txtQty.Text != string.Empty && txtQty.Text != "0")
                        {
                            if (int.Parse(txtQty.Text) > 0)
                            {
                                //Check Here if Purchase complete required is True
                                if (Purchaserequired == "Y")
                                {
                                    if ((int.Parse(txtQty.Text) == int.Parse(lblBalance.Text)))
                                    {
                                       
                                        List<StoreProduct> backorder;

                                        backorder = objMyIssuanceCartRepo.GetBackorder(Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                        if (backorder.Count > 0)
                                        {
                                            
                                            int Inventory=0;
                                            List<ProductItemInventory> backorder1;

                                            backorder1 = objMyIssuanceCartRepo.GrtBackOrder1(Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId), long.Parse(ddlSize.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                                            if (backorder1.Count > 0)
                                            {
                                                Inventory = Convert.ToInt32(backorder1[0].Inventory);
                                            }
                                         
                                            objLook = objLookupRepo.GetById(Convert.ToInt16(backorder[0].AllowBackOrderID));
                                            if (objLook != null)
                                            {
                                                if (Inventory < int.Parse(lblBalance.Text) && objLook.sLookupName == "Yes")
                                                {
                                                    
                                                    lblSingleMsg.Visible = false;
                                                    MyIssuanceCart objCart = new MyIssuanceCart();
                                                    objCart.UniformIssuancePolicyItemID = objPolicyItemResult[item.ItemIndex].UniformIssuancePolicyItemID;
                                                    objCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                                                    objCart.UniformIssuanceType = (long)IssuanceTypeID;
                                                    objCart.MasterItemID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId);
                                                    objCart.ItemSizeID = Convert.ToInt64(ddlSize.SelectedValue);
                                                    objCart.Qty = int.Parse(txtQty.Text);
                                                    objCart.NameToBeEngraved = NameToBeEngravedForSingle;
                                                    
                                                    //Added on 9 Mar 11 By Ankit for RunNumber
                                                    if (((Label)item.FindControl("lblRunCharge")).Text != "")
                                                    {
                                                        objCart.RunCharge = ((Label)item.FindControl("lblRunCharge")).Text;
                                                    }
                                                    else
                                                    {
                                                        objCart.RunCharge = null;
                                                    }
                                                    //End

                                                    rate = objMyIssuanceCartRepo.GetPriceByMasterItemIdandItemSizeId(Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId), long.Parse(ddlSize.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                                                    objCart.Rate = decimal.Parse(lblPrice.Text);
                                                    if (txtTailoring.Text != "")
                                                    {
                                                        objCart.TailoringLength = txtTailoring.Text;
                                                    }
                                                    if (txtShippingDate.Text != string.Empty)
                                                        objCart.ShippingDate = Convert.ToDateTime(txtShippingDate.Text);
                                                    else
                                                        objCart.BackOrderStatus = "Y";
                                                    objCart.ShippingDate = null;
                                                    if (objPolicyItemResult[item.ItemIndex].StoreProductid != null)
                                                    {
                                                        objCart.StoreProductID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].StoreProductid);
                                                    }
                                                    objCart.PriceLevel = Convert.ToInt32(priceleve);
                                                    //Add New Add Item Number
                                                    if (hdnItemNumber.Value != "")
                                                    {
                                                        objCart.ItemNumber = hdnItemNumber.Value;
                                                    }
                                                    //objCart.ItemColor = objPolicyItemResult[item.ItemIndex];
                                                    //End
                                                    objMyIssuanceCartRepo.Insert(objCart);
                                                    objMyIssuanceCartRepo.SubmitChanges();

                                                    // Store the MyIssuanceCartIDs with comma separted values

                                                    sbSingleCartIDs.Append(objCart.MyIssuanceCartID + ",");


                                                    // Calculate the Total Amount for Single Association Items
                                                    // SingleTotal += objCart.Rate * objCart.Qty;
                                                    SingleTotal += decimal.Parse(lblPrice.Text) * objCart.Qty;

                                                    boolMessage = true;
                                                    CompletePurchasedReqiured = true;

                                                }
                                                else if (Inventory < int.Parse(lblBalance.Text) && objLook.sLookupName == "No")
                                                {
                                                    lblSingleMsg.Visible = true;
                                                    lblSingleMsg.Text = "Inventory Level Is Low.You Can Not Process Order.";
                                                    boolMessage = false;
                                                    CompletePurchasedReqiured = false;
                                                    boolCkeckInventoryNotallowed = false;
                                                }
                                                else
                                                {
                                                    lblSingleMsg.Visible = false;
                                                    MyIssuanceCart objCart = new MyIssuanceCart();
                                                    objCart.UniformIssuancePolicyItemID = objPolicyItemResult[item.ItemIndex].UniformIssuancePolicyItemID;
                                                    objCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                                                    objCart.UniformIssuanceType = (long)IssuanceTypeID;
                                                    objCart.MasterItemID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId);
                                                    objCart.ItemSizeID = Convert.ToInt64(ddlSize.SelectedValue);
                                                    objCart.Qty = int.Parse(txtQty.Text);
                                                    objCart.NameToBeEngraved = NameToBeEngravedForSingle;
                                                    //Added on 9 Mar 11 By Ankit for RunNumber
                                                    if (((Label)item.FindControl("lblRunCharge")).Text != "")
                                                    {
                                                        objCart.RunCharge = ((Label)item.FindControl("lblRunCharge")).Text;
                                                    }
                                                    else
                                                    {
                                                        objCart.RunCharge = null;
                                                    }
                                                    //End

                                                    rate = objMyIssuanceCartRepo.GetPriceByMasterItemIdandItemSizeId(Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                                    objCart.Rate = decimal.Parse(lblPrice.Text);
                                                    if (txtTailoring.Text != "")
                                                    {
                                                        objCart.TailoringLength = txtTailoring.Text;
                                                    }
                                                    if (txtShippingDate.Text != string.Empty)
                                                        objCart.ShippingDate = Convert.ToDateTime(txtShippingDate.Text);
                                                    else
                                                      objCart.ShippingDate = null;
                                                    if (objPolicyItemResult[item.ItemIndex].StoreProductid != null)
                                                    {
                                                        objCart.StoreProductID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].StoreProductid);
                                                    }
                                                    objCart.PriceLevel = Convert.ToInt32(priceleve);
                                                    //Add New Add Item Number
                                                    if (hdnItemNumber.Value != "")
                                                    {
                                                        objCart.ItemNumber = hdnItemNumber.Value;
                                                    }
                                                    //objCart.ItemColor = objPolicyItemResult[item.ItemIndex];
                                                    //End
                                                    objMyIssuanceCartRepo.Insert(objCart);
                                                    objMyIssuanceCartRepo.SubmitChanges();

                                                    // Store the MyIssuanceCartIDs with comma separted values

                                                    sbSingleCartIDs.Append(objCart.MyIssuanceCartID + ",");


                                                    // Calculate the Total Amount for Single Association Items
                                                    // SingleTotal += objCart.Rate * objCart.Qty;
                                                    SingleTotal += decimal.Parse(lblPrice.Text) * objCart.Qty;
                                                    boolMessage = true;
                                                    CompletePurchasedReqiured = true;
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        CompletePurchasedReqiured = false;
                                        lblSingleMsg.Visible = true;
                                        lblSingleMsg.Text = "Please Select Quantity equal to Total Balance left";
                                        boolMessage = false;
                                    }
                                }
                                else
                                {
                                    if (int.Parse(txtQty.Text) <= int.Parse(lblBalance.Text))
                                    {
                                        List<StoreProduct> backorder;
                                        backorder = objMyIssuanceCartRepo.GetBackorder(Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId), long.Parse(ddlSize.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                                        if (backorder.Count > 0)
                                        {
                                            int Inventory=0;
                                            List<ProductItemInventory> backorder1;

                                            backorder1 = objMyIssuanceCartRepo.GrtBackOrder1(Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId), long.Parse(ddlSize.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                                            if (backorder1.Count > 0)
                                            {
                                                Inventory = Convert.ToInt32(backorder1[0].Inventory);
                                            }
                                            objLook = objLookupRepo.GetById(Convert.ToInt16(backorder[0].AllowBackOrderID));
                                            if (objLook != null)
                                            {
                                                if (Inventory < int.Parse(lblBalance.Text) && objLook.sLookupName == "Yes")
                                                {
                                                    lblSingleMsg.Visible = false;
                                                    MyIssuanceCart objCart = new MyIssuanceCart();
                                                    objCart.UniformIssuancePolicyItemID = objPolicyItemResult[item.ItemIndex].UniformIssuancePolicyItemID;
                                                    objCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                                                    objCart.UniformIssuanceType = (long)IssuanceTypeID;
                                                    objCart.MasterItemID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId);
                                                    objCart.ItemSizeID = Convert.ToInt64(ddlSize.SelectedValue);
                                                    objCart.Qty = int.Parse(txtQty.Text);
                                                    objCart.NameToBeEngraved = NameToBeEngravedForSingle;
                                                    rate = objMyIssuanceCartRepo.GetPriceByMasterItemIdandItemSizeId(Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId), long.Parse(ddlSize.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                                                    // objCart.Rate = rate;
                                                    objCart.Rate = decimal.Parse(lblPrice.Text);
                                                    if (txtTailoring.Text != "")
                                                    {
                                                        objCart.TailoringLength = txtTailoring.Text;
                                                    }
                                                    if (txtShippingDate.Text != string.Empty)
                                                        objCart.ShippingDate = Convert.ToDateTime(txtShippingDate.Text);
                                                    else
                                                        objCart.BackOrderStatus = "Y";
                                                    objCart.ShippingDate = null;
                                                    if (objPolicyItemResult[item.ItemIndex].StoreProductid != null)
                                                    {
                                                        objCart.StoreProductID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].StoreProductid);
                                                    }
                                                    objCart.PriceLevel = Convert.ToInt32(priceleve);
                                                    //Add New Add Item Number
                                                    if (hdnItemNumber.Value != "")
                                                    {
                                                        objCart.ItemNumber = hdnItemNumber.Value;
                                                    }
                                                    //objCart.ItemColor = objPolicyItemResult[item.ItemIndex];
                                                    objMyIssuanceCartRepo.Insert(objCart);
                                                    objMyIssuanceCartRepo.SubmitChanges();

                                                    // Store the MyIssuanceCartIDs with comma separted values

                                                    sbSingleCartIDs.Append(objCart.MyIssuanceCartID + ",");

                                                    // Calculate the Total Amount for Single Association Items
                                                    //SingleTotal += objCart.Rate * objCart.Qty;
                                                    SingleTotal += decimal.Parse(lblPrice.Text) * objCart.Qty;
                                                    boolMessage = true;

                                                }
                                                else if (Inventory < int.Parse(lblBalance.Text) && objLook.sLookupName == "No")
                                                {
                                                    lblSingleMsg.Visible = true;
                                                    lblSingleMsg.Text = "Inventory Level Is Low.You Can Not Process Order.";
                                                    boolMessage = false;
                                                    boolCkeckInventoryNotallowed = false;
                                                   
                                                }
                                                else
                                                {
                                                    lblSingleMsg.Visible = false;
                                                    MyIssuanceCart objCart = new MyIssuanceCart();
                                                    objCart.UniformIssuancePolicyItemID = objPolicyItemResult[item.ItemIndex].UniformIssuancePolicyItemID;
                                                    objCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                                                    objCart.UniformIssuanceType = (long)IssuanceTypeID;
                                                    objCart.MasterItemID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId);
                                                    objCart.ItemSizeID = Convert.ToInt64(ddlSize.SelectedValue);
                                                    objCart.Qty = int.Parse(txtQty.Text);
                                                    objCart.NameToBeEngraved = NameToBeEngravedForSingle;
                                                    rate = objMyIssuanceCartRepo.GetPriceByMasterItemIdandItemSizeId(Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                                    objCart.Rate = decimal.Parse(lblPrice.Text);
                                                    if (txtTailoring.Text != "")
                                                    {
                                                        objCart.TailoringLength = txtTailoring.Text;
                                                    }
                                                    if (txtShippingDate.Text != string.Empty)
                                                        objCart.ShippingDate = Convert.ToDateTime(txtShippingDate.Text);
                                                    else
                                                      objCart.ShippingDate = null;
                                                    if (objPolicyItemResult[item.ItemIndex].StoreProductid != null)
                                                    {
                                                        objCart.StoreProductID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].StoreProductid);
                                                    }
                                                    objCart.PriceLevel = Convert.ToInt32(priceleve);
                                                    //Add New Add Item Number
                                                    if (hdnItemNumber.Value != "")
                                                    {
                                                        objCart.ItemNumber = hdnItemNumber.Value;
                                                    }
                                                    //objCart.ItemColor = objPolicyItemResult[item.ItemIndex];
                                                    //End
                                                    objMyIssuanceCartRepo.Insert(objCart);
                                                    objMyIssuanceCartRepo.SubmitChanges();

                                                    // Store the MyIssuanceCartIDs with comma separted values

                                                    sbSingleCartIDs.Append(objCart.MyIssuanceCartID + ",");


                                                    // Calculate the Total Amount for Single Association Items
                                                    //SingleTotal += objCart.Rate * objCart.Qty;
                                                    SingleTotal += decimal.Parse(lblPrice.Text) * objCart.Qty;
                                                    boolMessage = true;

                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        lblSingleMsg.Visible = true;
                                        lblSingleMsg.Text = "Quantity cannot be more than Balance Left";
                                        boolMessage = false;

                                    }
                                }

                            }
                        }

                    }
                }

            }

            // Remove the last Comma
            if (sbSingleCartIDs.ToString().Contains(","))
                sbSingleCartIDs.Remove(sbSingleCartIDs.ToString().LastIndexOf(","), 1);

            Session["SingleCartIDs"] = sbSingleCartIDs.ToString();
            Session["SingleTotal"] = SingleTotal;
            LoadSingleAssociationData();

        }

        catch (Exception ex)
        {
            Session["SingleTotal"] = 0;
            Session["SingleCartIDs"] = string.Empty;
            ex.Message.ToString();
        }
    }
    #endregion
    #region Group Association Type
    protected void LoadGroupAssociationData1()
    {
        objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
        List<UniformIssuancePolicyItem> totalCountlist = new List<UniformIssuancePolicyItem>();
        totalCountlist = objPolicyRepo.CoutGroupRecord(Convert.ToInt64(Request.QueryString["PID"].ToString()));

        if (totalCountlist.Count > 0)
        {
            rptGroupAssociation.DataSource = totalCountlist;
            rptGroupAssociation.DataBind();

            //List<object> objTotalRecord = new List<object>();

            //for (int i = 0; i < totalCountlist.Count; i++)
            //{
            //    objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, totalCountlist[i].NEWGROUP).Where(z => z.AssociationIssuanceType == IssuanceTypeID).ToList();
            //    for (int z = 0; z < objPolicyItemResult.Count; z++)
            //    {
            //        objTotalRecord.Add(objPolicyItemResult[z]);
            //    }
            //}
            //foreach (RepeaterItem item in rptGroupAssociation.Items)
            //{
            //    Repeater rptChildGroupAssociation = (Repeater)item.FindControl("rptChildGroupAssociation");
            //    if (objTotalRecord.Count > 0)
            //    {
            //        rptChildGroupAssociation.DataSource = objTotalRecord;
            //        rptChildGroupAssociation.DataBind();
            //    }
            //}
            dvGroup.Visible = true;
            pnlGroup.Visible = true;

        }

        else
        {
            dvGroup.Visible = false;
            pnlGroup.Visible = false;

        }


        foreach (RepeaterItem item in rptGroupAssociation.Items)
        {
            Repeater rptChildGroupAssociation = (Repeater)item.FindControl("rptChildGroupAssociation");
            
            Label lblGroupIssuance = (Label)item.FindControl("lblGroupIssuance");
            Label lblGroupBalanceLeft = (Label)item.FindControl("lblGroupBalanceLeft");
            Label lblPurchaseGroupBalance = (Label)item.FindControl("lblPurchaseGroupBalance");
            HiddenField hdnNEWGROUP = (HiddenField)item.FindControl("hdnNEWGROUP");
            Label lblGroupItemAssociationParent = (Label)item.FindControl("lblGroupItemAssociationParent");
            objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
            if (objPolicy != null)
                priceleve = (objPolicy.PricingLevel).Trim();
            IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Group Association");



            // Update Chnage Nagmani 02-02-2012
            string[] MyWeatherlist = null;
            List<UniformIssuancePolicyItem> objMainList = new List<UniformIssuancePolicyItem>();
            List<SelectWeatherTypeIdResult> objWeatherlsit = new List<SelectWeatherTypeIdResult>();
            //Select Basestation and Employeetype Id from table companyemployee on login userid
            int intEmployeeType = 0;
            long logBaseStation = 0;
            string strweather = "0";
            objCmpEmp = objCmpEmpRepo.GetByUserInfoId(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
            if (objCmpEmp != null)
            {
                intEmployeeType = Convert.ToInt32(objCmpEmp.EmployeeTypeID);
                logBaseStation = Convert.ToInt64(objCmpEmp.BaseStation);

                if (objCmpEmp.EmployeeTypeID != null && objCmpEmp.BaseStation != null)
                {

                    //Select Weather from InceBaseStation using basestation parameter
                    objWeatherlsit = objPolicyItemRepo.GetWeatherTypeid(Convert.ToInt32(logBaseStation));
                    if (objWeatherlsit.Count > 0)
                    {
                        if (objWeatherlsit[0].WEATHERTYPE != null)
                        {
                            MyWeatherlist = objWeatherlsit[0].WEATHERTYPE.Split(',');

                            for (int i = 0; i < MyWeatherlist.Count(); i++)
                            {
                                if (i == 0)
                                {

                                    strweather = "'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                }
                                else
                                {

                                    strweather += ",'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                }

                            }

                            objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();

                            //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0)).ToList();


                        }
                        else
                        {
                            MyWeatherlist = null;


                        }
                    }
                    else
                    {
                        MyWeatherlist = null;

                    }



                }

                else if (objCmpEmp.EmployeeTypeID == null && objCmpEmp.BaseStation != null)
                {
                    intEmployeeType = 0;
                    logBaseStation = Convert.ToInt64(objCmpEmp.BaseStation);

                    //Select Weather from InceBaseStation using basestation parameter
                    objWeatherlsit = objPolicyItemRepo.GetWeatherTypeid(Convert.ToInt32(logBaseStation));
                    if (objWeatherlsit.Count > 0)
                    {
                        if (objWeatherlsit[0].WEATHERTYPE != null)
                        {
                            MyWeatherlist = objWeatherlsit[0].WEATHERTYPE.Split(',');

                            for (int i = 0; i < MyWeatherlist.Count(); i++)
                            {
                                if (i == 0)
                                {

                                    strweather = "'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                }
                                else
                                {

                                    strweather += ",'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                }

                            }

                            objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == 0)).ToList();
                            //  objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0)).ToList();


                        }
                        else
                        {
                            MyWeatherlist = null;


                        }
                    }
                    else
                    {
                        MyWeatherlist = null;

                    }

                }

            }
            else
            {
                intEmployeeType = 0;
                logBaseStation = 0;
            }


            // objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(i => i.AssociationIssuanceType == IssuanceTypeID).ToList();

            //End Nagmani 






            if (objPolicyItemResult.Count > 0)
            {

                objPolicy = objPolicyRepo.GetById(this.PolicyID);

                if (objPolicy != null)
                {
                    // Calculate by Month
                    if (objPolicy.IsDateOfHiredTicked == true)
                    {
                        DateTime? dtHiredDate, dtEligbleDate, dtExpiryDate;
                        objCmpEmp = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                        dtHiredDate = objCmpEmp.HirerdDate;
                        dtEligbleDate = dtHiredDate.Value.AddMonths(int.Parse(objPolicy.NumberOfMonths.ToString()));
                        dtExpiryDate = dtEligbleDate.Value.AddMonths(int.Parse(objPolicy.CreditExpireNumberOfMonths.ToString()));
                        if ((dtEligbleDate <= DateTime.Now || dtEligbleDate > DateTime.Now) && dtExpiryDate >= DateTime.Now)
                        {
                            dvGroup.Visible = true;
                            pnlGroup.Visible = true;
                            for (int i = 0; i < objPolicyItemResult.Count; i++)
                            {
                                if (objPolicyItemResult[i].AssociationIssuancePolicyNote != "")
                                {

                                    lblGroupItemAssociation.Text = "Group Association Policy";
                                    lblGroupItemAssociationParent.Text = objPolicyItemResult[i].AssociationIssuancePolicyNote;
                                }
                            }
                            rptChildGroupAssociation.DataSource = objPolicyItemResult;
                            rptChildGroupAssociation.DataBind();

                            #region Load Issuance and Balance

                            int? Qty = 0;

                            foreach (RepeaterItem item1 in rptChildGroupAssociation.Items)
                            {
                                objMyIssuanceCart = objMyIssuanceCartRepo.GetByPolicyItemId(objPolicyItemResult[item1.ItemIndex].UniformIssuancePolicyItemID, IncentexGlobal.CurrentMember.UserInfoID);

                                for (int i = 0; i < objMyIssuanceCart.Count; i++)
                                {
                                    if (objMyIssuanceCart[i].OrderStatus == "Submitted")
                                    {
                                        Qty += objMyIssuanceCart[i].Qty;
                                    }
                                }
                            }

                            lblGroupIssuance.Text = objPolicyItemResult[0].Issuance.ToString();
                            lblGroupBalanceLeft.Text = (int.Parse(lblGroupIssuance.Text) - Qty).ToString();

                            lblPurchaseGroupBalance.Text = Convert.ToString((int.Parse(lblGroupIssuance.Text)) - (int.Parse(lblGroupBalanceLeft.Text)));
                            if (int.Parse(lblGroupIssuance.Text) <= int.Parse(lblPurchaseGroupBalance.Text))
                            {
                                lblGroupBalanceLeft.Text = "0";
                            }
                            #endregion

                            #region Size Dropdown Fill

                            foreach (RepeaterItem item1 in rptChildGroupAssociation.Items)
                            {
                                MasterItemID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId);
                                objProdItem = objProdItemDetRepo.GetAllProductItem((int)MasterItemID);

                                #region Ankit Updated on 10 Feb For Name Bars For Group Association
                                string Stylename = objLookupRepo.GetById(objProdItem[0].ProductStyleID).sLookupName.ToString();
                                string ProdcutStyleName = objLookupRepo.GetById(objProdItem[0].ItemSizeID).sLookupName.ToString();
                                if (Stylename == "Name Bars")
                                {

                                    if (ProdcutStyleName == "No Size")
                                    {
                                        this.NameFormat = objProdItem[0].NameFormatForNameBars;
                                        this.FontFormat = objProdItem[0].FontFormatForNameBars;
                                        this.FName = IncentexGlobal.CurrentMember.FirstName;
                                        this.LName = IncentexGlobal.CurrentMember.LastName;
                                        this.FinalNameBarStyleForGroupAssociation = SetNameFontFormat();

                                        /*Employee Title added on 16 Feb :( */
                                        CompanyEmployee objCompanyEmployee = new CompanyEmployee();
                                        CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
                                        LookupRepository objLookupRepoForTitle = new LookupRepository();
                                        objCompanyEmployee = objEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                                        if (objCompanyEmployee.EmployeeTitleId != null)
                                            this.FinalNameBarStyleForGroupAssociation = this.FinalNameBarStyleForGroupAssociation + "," + objLookupRepoForTitle.GetById((long)objCompanyEmployee.EmployeeTitleId).sLookupName.ToString();

                                        /*End*/

                                        Panel pnlNameBarsForGroupAssociation = new Panel();
                                        pnlNameBarsForGroupAssociation = (Panel)item1.FindControl("pnlNameBarsForGroupAssociation");
                                        if (this.FinalNameBarStyleForGroupAssociation != string.Empty)
                                        {
                                            pnlNameBarsForGroupAssociation.Visible = true;
                                            if (this.FinalNameBarStyleForGroupAssociation.Contains(','))
                                            {

                                                string[] EmployeeTitle = (this.FinalNameBarStyleForGroupAssociation.Split(','));
                                                ((Label)item1.FindControl("lblNameTobeEngravedForGroupAssociation")).Text = EmployeeTitle[0];
                                                ((Label)item1.FindControl("lblEmplTitleForGroupAssociation")).Text = EmployeeTitle[1];
                                            }
                                            else
                                            {
                                                ((Label)item.FindControl("lblNameTobeEngravedForGroupAssociation")).Text = this.FinalNameBarStyleForGroupAssociation;
                                            }
                                        }
                                        else
                                        {
                                            pnlNameBarsForGroupAssociation.Visible = false;

                                        }

                                    }
                                }

                                #endregion

                                DropDownList ddlSize = new DropDownList();
                                ddlSize = (DropDownList)item1.FindControl("ddlSize");
                                HiddenField hdnStoreProductid = (HiddenField)item1.FindControl("hdnStoreProductid");


                                //Add Nagmani 16 Nov 2011
                                string ItemNumberSatus = null;
                                //End Nagmani 16 Nov 2011
                                var SizeList = (from s in objProdItem
                                                where s.ProductId == Convert.ToInt64(hdnStoreProductid.Value)
                                                select new { s.ItemSizeID, s.ItemNumberStatusID }).OrderBy(s => s.ItemSizeID).Distinct().ToList();
                                // foreach (var prod in objProdItem)
                                foreach (var prod in SizeList)
                                {

                                    //Add Nagmani 16 Nov 2011
                                    ItemNumberSatus = Convert.ToString(objLookupRepo.GetById(Convert.ToInt32(prod.ItemNumberStatusID)).sLookupName);
                                    //End Nagmani 16 Nov 2011
                                    if (ItemNumberSatus != "InActive")
                                    {
                                        ddlSize.Items.Add(new ListItem((objLookupRepo.GetById(prod.ItemSizeID).sLookupName).ToString(), (objLookupRepo.GetById(prod.ItemSizeID).iLookupID).ToString()));
                                    }
                                }


                                // Fill Master Item Number
                                MasterItemID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId);
                                LookupRepository oblLookRepos = new LookupRepository();
                                INC_Lookup objlook = new INC_Lookup();
                                objlook = oblLookRepos.GetById(MasterItemID);
                                Label lblItemNumber = (Label)item1.FindControl("lblItemNumber");

                                lblItemNumber.Text = objlook.sLookupName;
                                //Added by Surendar Yadav 4 December 2012
                                HyperLink lnkGroupSizingChart = (HyperLink)item1.FindControl("lnkGroupSizingChart");
                                TailoringMeasurementsRepository objRep = new TailoringMeasurementsRepository();
                                List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
                                objCount = objRep.GetTailoringMeasurementByStoreProductIDAndMasterItemID(Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId), Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].StoreProductid));
                                if (objCount.Count > 0)
                                {
                                    if (objCount[0].TailoringMeasurementChart != "")
                                    {
                                        lnkGroupSizingChart.Visible = true;
                                        lnkGroupSizingChart.NavigateUrl = "../UploadedImages/TailoringMeasurement/" + objCount[0].TailoringMeasurementChart;
                                    }
                                    else
                                    {
                                        lnkGroupSizingChart.Visible = false;
                                        lnkGroupSizingChart.NavigateUrl = "";
                                    }
                                }
                                else
                                {
                                    lnkGroupSizingChart.Visible = false;
                                    lnkGroupSizingChart.NavigateUrl = "";
                                }
                                //End by Surendar Yadav 4 December 2012
                                //Add Nagmani ItemNumber 4 july 2011
                                Boolean boolTrue = false;
                                HiddenField hdnItemNumber = (HiddenField)item1.FindControl("hdnGroupItemNumber");
                                List<SELECTITEMNUMBERONSIZEMASTERNOResult> OBLISTITME = new List<SELECTITEMNUMBERONSIZEMASTERNOResult>();
                                UniformIssuancePolicyItemRepository OBJREPOSTITEM = new UniformIssuancePolicyItemRepository();
                                OBLISTITME = OBJREPOSTITEM.GETITEMNUMBER(Convert.ToInt32(MasterItemID), Convert.ToInt32(ddlSize.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));
                                if (OBLISTITME.Count > 0)
                                {
                                    for (int i = 0; i < OBLISTITME.Count; i++)
                                    {
                                        if (boolTrue != true)
                                        {
                                            hdnItemNumber.Value = OBLISTITME[i].ITEMNUMBER.ToString();
                                            boolTrue = true;
                                        }
                                    }
                                }




                                //End Nagmani ItemNumber 4 july 2011



                                //Used method for Display Price
                                List<ProductItemPricing> objPricing = new List<ProductItemPricing>();

                                objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(MasterItemID, long.Parse(ddlSize.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));

                                if (objPricing.Count > 0)
                                {
                                    Label lblPrice = (Label)item1.FindControl("lblGroupPrice");
                                    if (priceleve == "1")
                                    {
                                        lblPrice.Text = Convert.ToString(objPricing[0].Level1);
                                    }
                                    else if (priceleve == "2")
                                    {
                                        lblPrice.Text = Convert.ToString(objPricing[0].Level2);
                                    }
                                    else
                                    {
                                        lblPrice.Text = Convert.ToString(objPricing[0].Level3);
                                    }

                                }

                                //Check here for if balance is Zero then
                                //enabled teqty false or true

                                TextBox txtQty = (TextBox)item1.FindControl("txtQty");

                                //Start Check Here If User Ordered Has Canceled 28-July-2011
                                //Saurabh--Check for Partial Issuance Status
                                CompanyEmployee objEmpType = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                                if (objEmpType.IssuancePolicyStatus == 'P')
                                {
                                    objOrderlist = objOrderConfirmRepost.CheckUserIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                                }
                                else
                                {
                                    objOrderlist = objOrderConfirmRepost.CheckUserIDWorkgroupIDEmpIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.EmployeeTypeID,this.WorkgroupID);
                                }
                                if (objOrderlist.Count > 0)
                                {
                                    if (Convert.ToDecimal(lblGroupBalanceLeft.Text) == 0)
                                    {
                                        txtQty.Enabled = false;

                                    }
                                    else
                                    {
                                        txtQty.Enabled = true;

                                    }
                                }
                                else
                                {
                                    lblGroupBalanceLeft.Text = lblGroupIssuance.Text;
                                    lblPurchaseGroupBalance.Text = "0";
                                    txtQty.Enabled = true;
                                }

                                //End Check Here If User Ordered Has Canceled 28-July-2011

                                // CHECK TAILORING OPTION TRUE FALASE
                                MasterItemID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId);
                                Panel pnlTailoring = (Panel)item1.FindControl("pnlTailoringGroup");
                                List<SelectTailoringOptionActiveInActiveResult> objTailoring;
                                objTailoring = objPolicyItemRepo.GetTailoringActiveInActive(Convert.ToInt32(MasterItemID), int.Parse(ddlSize.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));

                                if (objTailoring.Count > 0)
                                {
                                    if (objTailoring[0].sLookupName == "Active")
                                    {
                                        pnlTailoring.Visible = true;
                                    }
                                    else
                                    {
                                        pnlTailoring.Visible = false;
                                    }
                                }

                            }

                            #endregion

                            //Start Modify Nagmani 06-May-2011
                            //Check Here This User Has Taken any Issuance Policy the He can not take other issuance policy
                            objOrderlist = objOrderConfirmRepost.CheckUserIDWorkgroupIDEmpIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.EmployeeTypeID,this.WorkgroupID);
                            if (objOrderlist.Count > 0)
                            {
                                btnCheckOut.Visible = false;
                                lblNoRecord.Text = "You have already purchased issuance package.";
                                dvNoRecord.Visible = true;
                            }
                            else
                            {
                                //Check Here for dtEligible > today then button of checkout will be 
                                // not show else show.
                                if (dtEligbleDate > DateTime.Now && dtExpiryDate >= DateTime.Now)
                                {
                                    btnCheckOut.Visible = false;
                                    divDateEligible.Visible = true;
                                    lblDateEligible.Text = "Your Policy Is Not Yet Eligible:" + " " + lblEligable.Text;
                                }
                                else
                                {
                                    btnCheckOut.Visible = true;
                                    lblDateEligible.Text = "";
                                    divDateEligible.Visible = false;
                                    lblNoRecord.Text = "";
                                    dvNoRecord.Visible = false;
                                }
                            }


                        }
                        else
                        {
                            dvGroup.Visible = false;
                            pnlGroup.Visible = false;
                        }
                    }
                    // Calculate by Date
                    else
                    {
                        if (objPolicy.EligibleDate != null)
                        {

                            if ((objPolicy.EligibleDate <= DateTime.Now || objPolicy.EligibleDate > DateTime.Now) && objPolicy.CreditExpireDate >= DateTime.Now)
                            {
                                dvGroup.Visible = true;
                                pnlGroup.Visible = true;
                                for (int i = 0; i < objPolicyItemResult.Count; i++)
                                {
                                    if (objPolicyItemResult[i].AssociationIssuancePolicyNote != "")
                                    {

                                        lblGroupItemAssociation.Text = "Group Association Policy";
                                        lblGroupItemAssociationParent.Text = objPolicyItemResult[i].AssociationIssuancePolicyNote;
                                    }
                                }
                                rptChildGroupAssociation.DataSource = objPolicyItemResult;
                                rptChildGroupAssociation.DataBind();

                                #region Load Issuance and Balance

                                int? Qty = 0;

                                foreach (RepeaterItem item1 in rptChildGroupAssociation.Items)
                                {
                                    objMyIssuanceCart = objMyIssuanceCartRepo.GetByPolicyItemId(objPolicyItemResult[item1.ItemIndex].UniformIssuancePolicyItemID, IncentexGlobal.CurrentMember.UserInfoID);

                                    for (int i = 0; i < objMyIssuanceCart.Count; i++)
                                    {
                                        if (objMyIssuanceCart[i].OrderStatus == "Submitted")
                                        {
                                            Qty += objMyIssuanceCart[i].Qty;
                                        }
                                    }
                                }

                                lblGroupIssuance.Text = objPolicyItemResult[0].Issuance.ToString();
                                lblGroupBalanceLeft.Text = (int.Parse(lblGroupIssuance.Text) - Qty).ToString();

                                lblPurchaseGroupBalance.Text = Convert.ToString((int.Parse(lblGroupIssuance.Text)) - (int.Parse(lblGroupBalanceLeft.Text)));
                                if (int.Parse(lblGroupIssuance.Text) <= int.Parse(lblPurchaseGroupBalance.Text))
                                {
                                    lblGroupBalanceLeft.Text = "0";
                                }
                                #endregion

                                #region Size Dropdown Fill

                                foreach (RepeaterItem item1 in rptChildGroupAssociation.Items)
                                {
                                    MasterItemID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId);
                                    objProdItem = objProdItemDetRepo.GetAllProductItem((int)MasterItemID);
                                    HiddenField hdnStoreProductid = (HiddenField)item1.FindControl("hdnStoreProductid");
                                    #region Ankit Updated on 10 Feb For Name Bars For Group Association
                                    string Stylename = objLookupRepo.GetById(objProdItem[0].ProductStyleID).sLookupName.ToString();
                                    string ProdcutStyleName = objLookupRepo.GetById(objProdItem[0].ItemSizeID).sLookupName.ToString();
                                    if (Stylename == "Name Bars")
                                    {
                                        if (ProdcutStyleName == "No Size")
                                        {
                                            this.NameFormat = objProdItem[0].NameFormatForNameBars;
                                            this.FontFormat = objProdItem[0].FontFormatForNameBars;
                                            this.FName = IncentexGlobal.CurrentMember.FirstName;
                                            this.LName = IncentexGlobal.CurrentMember.LastName;
                                            this.FinalNameBarStyleForGroupAssociation = SetNameFontFormat();

                                            /*Employee Title added on 16 Feb :( */
                                            CompanyEmployee objCompanyEmployee = new CompanyEmployee();
                                            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
                                            LookupRepository objLookupRepoForTitle = new LookupRepository();
                                            objCompanyEmployee = objEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                                            if (objCompanyEmployee.EmployeeTitleId != null)
                                                this.FinalNameBarStyleForGroupAssociation = this.FinalNameBarStyleForGroupAssociation + "," + objLookupRepoForTitle.GetById((long)objCompanyEmployee.EmployeeTitleId).sLookupName.ToString();

                                            /*End*/

                                            Panel pnlNameBarsForGroupAssociation = new Panel();
                                            pnlNameBarsForGroupAssociation = (Panel)item1.FindControl("pnlNameBarsForGroupAssociation");
                                            if (this.FinalNameBarStyleForGroupAssociation != string.Empty)
                                            {
                                                pnlNameBarsForGroupAssociation.Visible = true;
                                                if (this.FinalNameBarStyleForGroupAssociation.Contains(','))
                                                {

                                                    string[] EmployeeTitle = (this.FinalNameBarStyleForGroupAssociation.Split(','));
                                                    ((Label)item1.FindControl("lblNameTobeEngravedForGroupAssociation")).Text = EmployeeTitle[0];
                                                    ((Label)item1.FindControl("lblEmplTitleForGroupAssociation")).Text = EmployeeTitle[1];
                                                }
                                                else
                                                {
                                                    ((Label)item1.FindControl("lblNameTobeEngravedForGroupAssociation")).Text = this.FinalNameBarStyleForGroupAssociation;
                                                }
                                            }
                                            else
                                            {
                                                pnlNameBarsForGroupAssociation.Visible = false;

                                            }

                                        }
                                    }

                                    #endregion

                                    DropDownList ddlSize = new DropDownList();
                                    ddlSize = (DropDownList)item1.FindControl("ddlSize");


                                    //Add Nagmani 16 Nov 2011
                                    string ItemNumberSatus = null;
                                    //End Nagmani 16 Nov 2011
                                    var SizeList = (from s in objProdItem
                                                    where s.ProductId == Convert.ToInt64(hdnStoreProductid.Value)
                                                    select new { s.ItemSizeID, s.ItemNumberStatusID }).OrderBy(s => s.ItemSizeID).Distinct().ToList();
                                    // foreach (var prod in objProdItem)
                                    foreach (var prod in SizeList)
                                    {

                                        //Add Nagmani 16 Nov 2011
                                        ItemNumberSatus = Convert.ToString(objLookupRepo.GetById(Convert.ToInt32(prod.ItemNumberStatusID)).sLookupName);
                                        //End Nagmani 16 Nov 2011
                                        if (ItemNumberSatus != "InActive")
                                        {
                                            ddlSize.Items.Add(new ListItem((objLookupRepo.GetById(prod.ItemSizeID).sLookupName).ToString(), (objLookupRepo.GetById(prod.ItemSizeID).iLookupID).ToString()));
                                        }
                                    }
                                    // Fill Master Item Nuber
                                    MasterItemID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId);
                                    LookupRepository oblLookRepos = new LookupRepository();
                                    INC_Lookup objlook = new INC_Lookup();
                                    objlook = oblLookRepos.GetById(MasterItemID);
                                    Label lblItemNumber = (Label)item1.FindControl("lblItemNumber");

                                    lblItemNumber.Text = objlook.sLookupName;
                                    //Added by Surendar Yadav 4 December 2012
                                    HyperLink lnkGroupSizingChart = (HyperLink)item1.FindControl("lnkGroupSizingChart");
                                    TailoringMeasurementsRepository objRep = new TailoringMeasurementsRepository();
                                    List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
                                    objCount = objRep.GetTailoringMeasurementByStoreProductIDAndMasterItemID(Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId), Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].StoreProductid));
                                    if (objCount.Count > 0)
                                    {
                                        if (objCount[0].TailoringMeasurementChart != "")
                                        {
                                            lnkGroupSizingChart.Visible = true;
                                            lnkGroupSizingChart.NavigateUrl = "../UploadedImages/TailoringMeasurement/" + objCount[0].TailoringMeasurementChart;
                                        }
                                        else
                                        {
                                            lnkGroupSizingChart.Visible = false;
                                            lnkGroupSizingChart.NavigateUrl = "";
                                        }
                                    }
                                    else
                                    {
                                        lnkGroupSizingChart.Visible = false;
                                        lnkGroupSizingChart.NavigateUrl = "";
                                    }
                                    //End by Surendar Yadav 4 December 2012
                                    //Add Nagmani ItemNumber 4 july 2011
                                    Boolean boolTrue = false;
                                    HiddenField hdnItemNumber = (HiddenField)item1.FindControl("hdnGroupItemNumber");
                                    List<SELECTITEMNUMBERONSIZEMASTERNOResult> OBLISTITME = new List<SELECTITEMNUMBERONSIZEMASTERNOResult>();
                                    UniformIssuancePolicyItemRepository OBJREPOSTITEM = new UniformIssuancePolicyItemRepository();
                                    OBLISTITME = OBJREPOSTITEM.GETITEMNUMBER(Convert.ToInt32(MasterItemID), Convert.ToInt32(ddlSize.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));
                                    if (OBLISTITME.Count > 0)
                                    {
                                        for (int i = 0; i < OBLISTITME.Count; i++)
                                        {
                                            if (boolTrue != true)
                                            {
                                                hdnItemNumber.Value = OBLISTITME[i].ITEMNUMBER.ToString();
                                                boolTrue = true;
                                            }
                                        }
                                    }

                                    //End Nagmani ItemNumber 4 july 2011

                                    //Used method for Display Price
                                    List<ProductItemPricing> objPricing = new List<ProductItemPricing>();

                                    objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(MasterItemID, long.Parse(ddlSize.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                                    if (objPricing.Count > 0)
                                    {
                                        Label lblPrice = (Label)item1.FindControl("lblGroupPrice");
                                        if (priceleve == "1")
                                        {
                                            lblPrice.Text = Convert.ToString(objPricing[0].Level1);
                                        }
                                        else if (priceleve == "2")
                                        {
                                            lblPrice.Text = Convert.ToString(objPricing[0].Level2);
                                        }
                                        else
                                        {
                                            lblPrice.Text = Convert.ToString(objPricing[0].Level3);
                                        }

                                    }
                                    //Check here for if balance is Zero then
                                    //enabled teqty false or true
                                    TextBox txtQty = (TextBox)item1.FindControl("txtQty");
                                    //Saurabh--Check for Partial Issuance Status
                                    CompanyEmployee objEmpType = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                                    if (objEmpType.IssuancePolicyStatus == 'P')
                                    {
                                        objOrderlist = objOrderConfirmRepost.CheckUserIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                                    }
                                    else
                                    {
                                        objOrderlist = objOrderConfirmRepost.CheckUserIDWorkgroupIDEmpIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.EmployeeTypeID,this.WorkgroupID);
                                    }
                                    if (objOrderlist.Count > 0)
                                    {
                                        if (Convert.ToDecimal(lblGroupBalanceLeft.Text) == 0)
                                        {
                                            txtQty.Enabled = false;

                                        }
                                        else
                                        {
                                            txtQty.Enabled = true;

                                        }
                                    }
                                    else
                                    {
                                        lblGroupBalanceLeft.Text = lblGroupIssuance.Text;
                                        lblPurchaseGroupBalance.Text = "0";
                                        txtQty.Enabled = true;
                                    }

                                    //End Check Here If User Ordered Has Canceled 28-July-2011



                                    // CHECK TAILORING OPTION TRUE FALASE
                                    MasterItemID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId);
                                    Panel pnlTailoring = (Panel)item1.FindControl("pnlTailoringGroup");
                                    List<SelectTailoringOptionActiveInActiveResult> objTailoring;
                                    objTailoring = objPolicyItemRepo.GetTailoringActiveInActive(Convert.ToInt32(MasterItemID), int.Parse(ddlSize.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));

                                    if (objTailoring.Count > 0)
                                    {
                                        if (objTailoring[0].sLookupName == "Active")
                                        {
                                            pnlTailoring.Visible = true;
                                        }
                                        else
                                        {
                                            pnlTailoring.Visible = false;
                                        }
                                    }
                                }



                                #endregion

                                //Start Modify Nagmani 06-May-2011
                                //Check Here This User Has Taken any Issuance Policy the He can not take other issuance policy
                                objOrderlist = objOrderConfirmRepost.CheckUserIDWorkgroupIDEmpIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.EmployeeTypeID,this.WorkgroupID);
                                if (objOrderlist.Count > 0)
                                {
                                    btnCheckOut.Visible = false;
                                    lblNoRecord.Text = "You have already purchased issuance package.";
                                    dvNoRecord.Visible = true;
                                }
                                else
                                {
                                    //Check here for checkout button will show or Not on the Eligible date.
                                    if (objPolicy.EligibleDate > DateTime.Now && objPolicy.CreditExpireDate >= DateTime.Now)
                                    {
                                        btnCheckOut.Visible = false;
                                        divDateEligible.Visible = true;
                                        lblDateEligible.Text = "Your Policy Is Not Yet Eligible:" + " " + lblEligable.Text;
                                    }
                                    else
                                    {
                                        btnCheckOut.Visible = true;
                                        lblDateEligible.Text = "";
                                        divDateEligible.Visible = false;
                                        lblNoRecord.Text = "";
                                        dvNoRecord.Visible = false;
                                    }
                                }



                            }
                            else
                            {
                                dvGroup.Visible = false;
                                pnlGroup.Visible = false;
                            }
                        }
                        else
                        {
                            dvGroup.Visible = false;
                            pnlGroup.Visible = false;
                        }
                    }

                }
                else
                {
                    dvGroup.Visible = false;
                    pnlGroup.Visible = false;
                }
            }
            //else
            //{
            //    dvGroup.Visible = false;
            //    pnlGroup.Visible = false;
            //}

        }
    }
    protected void ProcessGroupAssociation()
    {
        try
        {
            string NameToBeEngravedForGroup = string.Empty;
            decimal? GroupTotal = 0;
            StringBuilder sbGroupCartIDs = new StringBuilder();
            sbGroupCartIDs.Append(string.Empty);
            foreach (RepeaterItem item in rptGroupAssociation.Items)
            {
                Repeater rptChildGroupAssociation = (Repeater)item.FindControl("rptChildGroupAssociation");
                Label lblGroupBalanceLeft = (Label)item.FindControl("lblGroupBalanceLeft");
                HiddenField hdnNEWGROUP = (HiddenField)item.FindControl("hdnNEWGROUP");

            if (dvGroup.Visible == true && pnlGroup.Visible == true)
            {
                decimal? rate = 0;
                int totalqty = 0;

                IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Group Association");

                // Update Chnage Nagmani 02-02-2012
               
                //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(i => i.AssociationIssuanceType == IssuanceTypeID).ToList();
                
                string[] MyWeatherlist = null;
                List<UniformIssuancePolicyItem> objMainList = new List<UniformIssuancePolicyItem>();
                List<SelectWeatherTypeIdResult> objWeatherlsit = new List<SelectWeatherTypeIdResult>();
                //Select Basestation and Employeetype Id from table companyemployee on login userid
                int intEmployeeType = 0;
                long logBaseStation = 0;
                string strweather = "0";
                objCmpEmp = objCmpEmpRepo.GetByUserInfoId(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                if (objCmpEmp != null)
                {
                    intEmployeeType = Convert.ToInt32(objCmpEmp.EmployeeTypeID);
                    logBaseStation = Convert.ToInt64(objCmpEmp.BaseStation);

                    if (objCmpEmp.EmployeeTypeID != null && objCmpEmp.BaseStation != null)
                    {

                        //Select Weather from InceBaseStation using basestation parameter
                        objWeatherlsit = objPolicyItemRepo.GetWeatherTypeid(Convert.ToInt32(logBaseStation));
                        if (objWeatherlsit.Count > 0)
                        {
                            if (objWeatherlsit[0].WEATHERTYPE != null)
                            {
                                MyWeatherlist = objWeatherlsit[0].WEATHERTYPE.Split(',');

                                for (int i = 0; i < MyWeatherlist.Count(); i++)
                                {
                                    if (i == 0)
                                    {

                                        strweather = "'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }
                                    else
                                    {

                                        strweather += ",'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }

                                }

                                objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();

                                //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0)).ToList();


                            }
                            else
                            {
                                MyWeatherlist = null;


                            }
                        }
                        else
                        {
                            MyWeatherlist = null;

                        }



                    }

                    else if (objCmpEmp.EmployeeTypeID == null && objCmpEmp.BaseStation != null)
                    {
                        intEmployeeType = 0;
                        logBaseStation = Convert.ToInt64(objCmpEmp.BaseStation);

                        //Select Weather from InceBaseStation using basestation parameter
                        objWeatherlsit = objPolicyItemRepo.GetWeatherTypeid(Convert.ToInt32(logBaseStation));
                        if (objWeatherlsit.Count > 0)
                        {
                            if (objWeatherlsit[0].WEATHERTYPE != null)
                            {
                                MyWeatherlist = objWeatherlsit[0].WEATHERTYPE.Split(',');

                                for (int i = 0; i < MyWeatherlist.Count(); i++)
                                {
                                    if (i == 0)
                                    {

                                        strweather = "'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }
                                    else
                                    {

                                        strweather += ",'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }

                                }

                                objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == 0)).ToList();
                                //  objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0)).ToList();


                            }
                            else
                            {
                                MyWeatherlist = null;


                            }
                        }
                        else
                        {
                            MyWeatherlist = null;

                        }

                    }

                }
                else
                {
                    intEmployeeType = 0;
                    logBaseStation = 0;
                }


                // objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(i => i.AssociationIssuanceType == IssuanceTypeID).ToList();

                //End Nagmani 
                
                
                objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
                Purchaserequired = Convert.ToString(objPolicy.CompletePurchase);
                if (objPolicyItemResult.Count > 0)
                {
                    foreach (RepeaterItem item1 in rptChildGroupAssociation.Items)
                    {
                        TextBox txtQty = new TextBox();
                        txtQty = (TextBox)item1.FindControl("txtQty");
                        if (txtQty.Text != string.Empty && txtQty.Text != "0")
                        {
                            if (int.Parse(txtQty.Text) > 0)
                                totalqty += int.Parse(txtQty.Text);
                        }
                        
                    }

                    //Check Here if Purchase complete required is True
                    if (Purchaserequired == "Y")
                    {
                        if (totalqty == int.Parse(lblGroupBalanceLeft.Text))
                        {
                            foreach (RepeaterItem item1 in rptChildGroupAssociation.Items)
                            {
                                Label lblItemNumber = (Label)item1.FindControl("lblItemNumber");
                                HiddenField hdnItemNumber = (HiddenField)item1.FindControl("hdnGroupItemNumber");
                                TextBox txtQty = new TextBox();
                                txtQty = (TextBox)item1.FindControl("txtQty");
                                Label lblPrice = (Label)item1.FindControl("lblGroupPrice");
                                DropDownList ddlSize = new DropDownList();
                                 HiddenField hdnStoreProductid=(HiddenField)item1.FindControl("hdnStoreProductid");
                                ddlSize = (DropDownList)item1.FindControl("ddlSize");

                                //Created By Ankit For NameBars
                                if (Convert.ToString(((Label)item1.FindControl("lblNameTobeEngravedForGroupAssociation")).Text) != string.Empty)
                                {
                                    if (Convert.ToString(((Label)item1.FindControl("lblEmplTitleForGroupAssociation")).Text) != string.Empty)
                                    {
                                        NameToBeEngravedForGroup = Convert.ToString(((Label)item1.FindControl("lblNameTobeEngravedForGroupAssociation")).Text) + "," + Convert.ToString(((Label)item1.FindControl("lblEmplTitleForGroupAssociation")).Text);
                                    }
                                    else
                                    {
                                        NameToBeEngravedForGroup = Convert.ToString(((Label)item1.FindControl("lblNameTobeEngravedForGroupAssociation")).Text);
                                    }
                                }
                                else
                                {
                                    NameToBeEngravedForGroup = null;
                                }

                                //End

                                if (txtQty.Text != string.Empty && txtQty.Text != "0")
                                {
                                    if (int.Parse(txtQty.Text) > 0)
                                    {
                                        lblGroupMsg.Visible = false;
                                        MyIssuanceCart objCart = new MyIssuanceCart();
                                        objCart.UniformIssuancePolicyItemID = objPolicyItemResult[item1.ItemIndex].UniformIssuancePolicyItemID;
                                        objCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                                        objCart.UniformIssuanceType = (long)IssuanceTypeID;
                                        objCart.MasterItemID = Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupMasterItemId")).Value);
                                        objCart.ItemSizeID = Convert.ToInt64(ddlSize.SelectedValue);
                                        objCart.Qty = int.Parse(txtQty.Text);
                                        objCart.NameToBeEngraved = NameToBeEngravedForGroup;
                                        rate = objMyIssuanceCartRepo.GetPriceByMasterItemIdandItemSizeId(Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupMasterItemId")).Value), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                        // objCart.Rate = rate;
                                        objCart.Rate = decimal.Parse(lblPrice.Text);
                                        if (txtShippingDate.Text != string.Empty)
                                            objCart.ShippingDate = Convert.ToDateTime(txtShippingDate.Text);
                                        else
                                            objCart.ShippingDate = null;
                                        if (objPolicyItemResult[item1.ItemIndex].StoreProductid != null)
                                        {
                                            objCart.StoreProductID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].StoreProductid);
                                        }
                                        objCart.PriceLevel = Convert.ToInt32(priceleve);
                                        //Add New Item Number
                                        if (hdnItemNumber.Value != "")
                                        {
                                            objCart.ItemNumber = hdnItemNumber.Value;
                                        }
                                        //End
                                        // Add Tailoring Length
                                        TextBox txtTailoring = (TextBox)item1.FindControl("txtGroupTailoringlenght");
                                        if (txtTailoring.Text != "")
                                        {
                                            objCart.TailoringLength = txtTailoring.Text;
                                        }

                                        List<StoreProduct> backorder;

                                        backorder = objMyIssuanceCartRepo.GetBackorder(Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupMasterItemId")).Value), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                        if (backorder.Count > 0)
                                        {
                                            int Inventory=0;
                                            List<ProductItemInventory> backorder1;
                                            backorder1 = objMyIssuanceCartRepo.GrtBackOrder1(Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupMasterItemId")).Value), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                            if (backorder1.Count > 0)
                                            {
                                                Inventory = Convert.ToInt32(backorder1[0].Inventory);
                                            }
                                            //End Modify Nagmani 05-May-2011
                                            objLook = objLookupRepo.GetById(Convert.ToInt16(backorder[0].AllowBackOrderID));
                                            if (objLook != null)
                                            {
                                                //Check here Backorder is yes
                                                if (Inventory < int.Parse(lblGroupBalanceLeft.Text) && objLook.sLookupName == "Yes")
                                                {
                                                    objCart.BackOrderStatus = "Y";
                                                    objMyIssuanceCartRepo.Insert(objCart);
                                                    objMyIssuanceCartRepo.SubmitChanges();

                                                    // Store the MyIssuanceCartIDs with comma separted values
                                                    sbGroupCartIDs.Append(objCart.MyIssuanceCartID + ",");


                                                    // Calculate Group Association Total Amount
                                                    // GroupTotal += objCart.Rate * objCart.Qty;
                                                    GroupTotal += decimal.Parse(lblPrice.Text) * objCart.Qty;

                                                    boolMessage = true;
                                                    CompletePurchasedReqiured = true;
                                                }
                                                else if (Inventory < int.Parse(lblGroupBalanceLeft.Text) && objLook.sLookupName == "No")
                                                {
                                                    lblGroupMsg.Visible = true;
                                                    lblGroupMsg.Text = "Inventory Level Is Low.You Can Not Process Order.";
                                                    boolMessage = false;
                                                    CompletePurchasedReqiured = false;
                                                   boolCkeckInventoryNotallowed = false;
                                                }
                                                else
                                                {

                                                    objMyIssuanceCartRepo.Insert(objCart);
                                                    objMyIssuanceCartRepo.SubmitChanges();

                                                    // Store the MyIssuanceCartIDs with comma separted values

                                                    sbGroupCartIDs.Append(objCart.MyIssuanceCartID + ",");


                                                    // Calculate Group Association Total Amount
                                                    //GroupTotal += objCart.Rate * objCart.Qty;
                                                    GroupTotal += decimal.Parse(lblPrice.Text) * objCart.Qty;
                                                    boolMessage = true;
                                                    CompletePurchasedReqiured = true;
                                                }
                                            }
                                        }


                                    }
                                }
                            }

                        }
                        else
                        {
                            CompletePurchasedReqiured = false;
                            lblGroupMsg.Visible = true;
                            lblGroupMsg.Text = "Please Select Quantity equal to Total Balance left";
                            boolMessage = false;
                        }
                    }
                    else
                    {
                        if (totalqty <= int.Parse(lblGroupBalanceLeft.Text))
                        {
                            foreach (RepeaterItem item1 in rptChildGroupAssociation.Items)
                            {
                                 HiddenField hdnStoreProductid=(HiddenField)item1.FindControl("hdnStoreProductid");
                                TextBox txtQty = new TextBox();
                                txtQty = (TextBox)item1.FindControl("txtQty");
                                Label lblPrice = (Label)item1.FindControl("lblGroupPrice");
                                DropDownList ddlSize = new DropDownList();
                                ddlSize = (DropDownList)item1.FindControl("ddlSize");
                                Label lblItemNumber = (Label)item1.FindControl("lblItemNumber");
                                //Add Nagmani ItemNumber 4 july 2011
                                HiddenField hdnItemNumber = (HiddenField)item1.FindControl("hdnGroupItemNumber");
                                //End Nagmani ItemNumber 4 july 2011
                                //Created By Ankit For NameBars
                                if (Convert.ToString(((Label)item1.FindControl("lblNameTobeEngravedForGroupAssociation")).Text) != string.Empty)
                                {
                                    if (Convert.ToString(((Label)item1.FindControl("lblEmplTitleForGroupAssociation")).Text) != string.Empty)
                                    {
                                        NameToBeEngravedForGroup = Convert.ToString(((Label)item1.FindControl("lblNameTobeEngravedForGroupAssociation")).Text) + "," + Convert.ToString(((Label)item1.FindControl("lblEmplTitleForGroupAssociation")).Text);
                                    }
                                    else
                                    {
                                        NameToBeEngravedForGroup = Convert.ToString(((Label)item1.FindControl("lblNameTobeEngravedForGroupAssociation")).Text);
                                    }
                                }
                                else
                                {
                                    NameToBeEngravedForGroup = null;
                                }

                                //End

                                if (txtQty.Text != string.Empty && txtQty.Text != "0")
                                {
                                    if (int.Parse(txtQty.Text) > 0)
                                    {
                                        lblGroupMsg.Visible = false;

                                        MyIssuanceCart objCart = new MyIssuanceCart();

                                        objCart.UniformIssuancePolicyItemID = objPolicyItemResult[item1.ItemIndex].UniformIssuancePolicyItemID;
                                        objCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                                        objCart.UniformIssuanceType = (long)IssuanceTypeID;
                                        objCart.MasterItemID = Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupMasterItemId")).Value);
                                        objCart.ItemSizeID = Convert.ToInt64(ddlSize.SelectedValue);
                                        objCart.Qty = int.Parse(txtQty.Text);
                                        objCart.NameToBeEngraved = NameToBeEngravedForGroup;
                                        rate = objMyIssuanceCartRepo.GetPriceByMasterItemIdandItemSizeId(Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupMasterItemId")).Value), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                       
                                        objCart.Rate = decimal.Parse(lblPrice.Text);
                                        if (txtShippingDate.Text != string.Empty)
                                            objCart.ShippingDate = Convert.ToDateTime(txtShippingDate.Text);
                                        else
                                            objCart.ShippingDate = null;
                                        if (objPolicyItemResult[item1.ItemIndex].StoreProductid != null)
                                        {
                                            objCart.StoreProductID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].StoreProductid);
                                        }
                                        objCart.PriceLevel = Convert.ToInt32(priceleve);
                                       
                                        if (hdnItemNumber.Value != "")
                                        {
                                            objCart.ItemNumber = hdnItemNumber.Value;
                                        }
                                      
                                        
                                        TextBox txtTailoring = (TextBox)item1.FindControl("txtGroupTailoringlenght");
                                        if (txtTailoring.Text != "")
                                        {
                                            objCart.TailoringLength = txtTailoring.Text;
                                        }
                                        


                                        List<StoreProduct> backorder;

                                        backorder = objMyIssuanceCartRepo.GetBackorder(Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupMasterItemId")).Value), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                        if (backorder.Count > 0)
                                        {
                                            objCart.StoreProductID = backorder[0].StoreProductID;
                                            int Inventory=0;
                                            List<ProductItemInventory> backorder1;

                                            backorder1 = objMyIssuanceCartRepo.GrtBackOrder1(Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupMasterItemId")).Value), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                            if (backorder1.Count > 0)
                                            {
                                                Inventory = Convert.ToInt32(backorder1[0].Inventory);
                                            }
                                            //End Modify Nagmani 05-May-2011
                                            objLook = objLookupRepo.GetById(Convert.ToInt16(backorder[0].AllowBackOrderID));
                                            if (objLook != null)
                                            {
                                                //Check here Backorder is yes
                                                if (Inventory < int.Parse(lblGroupBalanceLeft.Text) && objLook.sLookupName == "Yes")
                                                {
                                                    objCart.BackOrderStatus = "Y";
                                                    objMyIssuanceCartRepo.Insert(objCart);
                                                    objMyIssuanceCartRepo.SubmitChanges();
                                                    // Store the MyIssuanceCartIDs with comma separted values
                                                    sbGroupCartIDs.Append(objCart.MyIssuanceCartID + ",");
                                                    // Calculate Group Association Total Amount
                                                    //GroupTotal += objCart.Rate * objCart.Qty;
                                                    GroupTotal += decimal.Parse(lblPrice.Text) * objCart.Qty;
                                                    boolMessage = true;

                                                }
                                                else if (Inventory < int.Parse(lblGroupBalanceLeft.Text) && objLook.sLookupName == "No")
                                                {
                                                    lblGroupMsg.Visible = true;
                                                    lblGroupMsg.Text = "Inventory Level Is Low.You Can Not Process Order.";
                                                    boolMessage = false;
                                                    
                                                   
                                                }
                                                else
                                                {

                                                    objMyIssuanceCartRepo.Insert(objCart);
                                                    objMyIssuanceCartRepo.SubmitChanges();
                                                    // Store the MyIssuanceCartIDs with comma separted values
                                                    sbGroupCartIDs.Append(objCart.MyIssuanceCartID + ",");
                                                    // Calculate Group Association Total Amount
                                                    //GroupTotal += objCart.Rate * objCart.Qty;
                                                    GroupTotal += decimal.Parse(lblPrice.Text) * objCart.Qty;
                                                    boolMessage = true;
                                                }
                                            }
                                        }


                                    }
                                }
                            }
                        }
                        else
                        {
                            lblGroupMsg.Visible = true;
                            lblGroupMsg.Text = "Combined Quantity cannot be more than Balance Left";
                            boolMessage = false;
                        }
                    }

                   

                    Session["GroupCartIDs"] = sbGroupCartIDs.ToString();
                    Session["GroupTotal"] = GroupTotal;
                   
                }
            }
        }
            // Remove the last Comma
            if (sbGroupCartIDs.ToString().Contains(","))
                sbGroupCartIDs.Remove(sbGroupCartIDs.ToString().LastIndexOf(","), 1);
            LoadGroupAssociationData1();
        }
        catch (Exception ex)
        {
            Session["GroupTotal"] = 0;
            Session["GroupCartIDs"] = string.Empty;
            ex.Message.ToString();
        }
    }
    #endregion
    #region Group Budget Association Type
    protected void LoadGroupBudgetAssociationData1()
    {
        try
        {
            objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
           // List<UniformIssuancePolicyItem> totalCountlist = new List<UniformIssuancePolicyItem>();
            List<SelectTotaoIssuanceBudgetPolicyResult> totalCountlist = new List<SelectTotaoIssuanceBudgetPolicyResult>();
            totalCountlist = objPolicyRepo.CoutRecord1(Convert.ToInt64(Request.QueryString["PID"].ToString()));
            
            if (totalCountlist.Count > 0)
            {
                rptGroupBudgetAssociation.DataSource = totalCountlist;
                rptGroupBudgetAssociation.DataBind();
                dvGroupBudget.Visible = true;
                pnlGroupBudget.Visible = true;
            }
            else
            {
                dvGroupBudget.Visible = false;
                pnlGroupBudget.Visible = false;
            }
            //End nagmani

            foreach (RepeaterItem item in rptGroupBudgetAssociation.Items)
            {
                Repeater rptChildGroupBudgetAssociation = (Repeater)item.FindControl("rptChildGroupBudgetAssociation");

                Label lblGroupBudgetIssuance = (Label)item.FindControl("lblGroupBudgetIssuance");
                Label lblGroupBudgetBalanceLeft = (Label)item.FindControl("lblGroupBudgetBalanceLeft");
                Label lblPurchaseGroupBudget = (Label)item.FindControl("lblPurchaseGroupBudget");
                Label lblGroupBudgetAmount = (Label)item.FindControl("lblGroupBudgetAmount");
                Label lblGroupBudgetAmountLeft = (Label)item.FindControl("lblGroupBudgetAmountLeft");
                Label lblBudgetUsed = (Label)item.FindControl("lblBudgetUsed");
                HiddenField hdnNEWGROUP = (HiddenField)item.FindControl("hdnNEWGROUP");
                Label lblGroupItemAssociationBudgetParent = (Label)item.FindControl("lblGroupItemAssociationBudgetParent");
                if (objPolicy != null)
                    priceleve = (objPolicy.PricingLevel).Trim();
                IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Group Association with Budget");
              

                // Update Chnage Nagmani 02-02-2012
                string[] MyWeatherlist = null;
                List<UniformIssuancePolicyItem> objMainList = new List<UniformIssuancePolicyItem>();
                List<SelectWeatherTypeIdResult> objWeatherlsit = new List<SelectWeatherTypeIdResult>();
                //Select Basestation and Employeetype Id from table companyemployee on login userid
                int intEmployeeType = 0;
                long logBaseStation = 0;
                string strweather = "0";
                objCmpEmp = objCmpEmpRepo.GetByUserInfoId(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                if (objCmpEmp != null)
                {
                    intEmployeeType = Convert.ToInt32(objCmpEmp.EmployeeTypeID);
                    logBaseStation = Convert.ToInt64(objCmpEmp.BaseStation);

                    if (objCmpEmp.EmployeeTypeID != null && objCmpEmp.BaseStation != null)
                    {

                        //Select Weather from InceBaseStation using basestation parameter
                        objWeatherlsit = objPolicyItemRepo.GetWeatherTypeid(Convert.ToInt32(logBaseStation));
                        if (objWeatherlsit.Count > 0)
                        {
                            if (objWeatherlsit[0].WEATHERTYPE != null)
                            {
                                MyWeatherlist = objWeatherlsit[0].WEATHERTYPE.Split(',');

                                for (int i = 0; i < MyWeatherlist.Count(); i++)
                                {
                                    if (i == 0)
                                    {

                                        strweather = "'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }
                                    else
                                    {

                                        strweather += ",'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }

                                }
                                objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();

                                //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0)).ToList();
                            }

                        }

                    }

                    else if (objCmpEmp.EmployeeTypeID == null && objCmpEmp.BaseStation != null)
                    {
                        intEmployeeType = 0;
                        logBaseStation = Convert.ToInt64(objCmpEmp.BaseStation);

                        //Select Weather from InceBaseStation using basestation parameter
                        objWeatherlsit = objPolicyItemRepo.GetWeatherTypeid(Convert.ToInt32(logBaseStation));
                        if (objWeatherlsit.Count > 0)
                        {
                            if (objWeatherlsit[0].WEATHERTYPE != null)
                            {
                                MyWeatherlist = objWeatherlsit[0].WEATHERTYPE.Split(',');

                                for (int i = 0; i < MyWeatherlist.Count(); i++)
                                {
                                    if (i == 0)
                                    {

                                        strweather = "'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }
                                    else
                                    {

                                        strweather += ",'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }

                                }

                                objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == 0)).ToList();
                            }



                        }

                    }

                }
                else
                {
                    intEmployeeType = 0;
                    logBaseStation = 0;
                }



                // objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(z => z.AssociationIssuanceType == IssuanceTypeID).ToList();
                //End Nagmani 
                if (objPolicyItemResult.Count > 0)
                {

                    rptChildGroupBudgetAssociation.DataSource = objPolicyItemResult;
                    rptChildGroupBudgetAssociation.DataBind();
                    objPolicy = objPolicyRepo.GetById(this.PolicyID);

                    if (objPolicy != null)
                    {
                        // Calculate by Month
                        if (objPolicy.IsDateOfHiredTicked == true)
                        {
                            DateTime? dtHiredDate, dtEligbleDate, dtExpiryDate;

                            objCmpEmp = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                            dtHiredDate = objCmpEmp.HirerdDate;
                            dtEligbleDate = dtHiredDate.Value.AddMonths(int.Parse(objPolicy.NumberOfMonths.ToString()));
                            dtExpiryDate = dtEligbleDate.Value.AddMonths(int.Parse(objPolicy.CreditExpireNumberOfMonths.ToString()));

                            if ((dtEligbleDate <= DateTime.Now || dtEligbleDate > DateTime.Now) && dtExpiryDate >= DateTime.Now)
                            {
                                //dvSingle.Visible = true;
                                //pnlSingle.Visible = true;
                                dvGroupBudget.Visible = true;
                                pnlGroupBudget.Visible = true;
                                for (int l = 0; l < objPolicyItemResult.Count; l++)
                                {
                                    if (objPolicyItemResult[l].AssociationIssuancePolicyNote != "")
                                    {
                                        //lblGroupItemAssociationBudget.Text = objPolicyItemResult[l].AssociationIssuancePolicyNote;
                                        lblGroupItemAssociationBudget.Text = "Group With Budget Policy";
                                        lblGroupItemAssociationBudgetParent.Text = objPolicyItemResult[l].AssociationIssuancePolicyNote;
                                    }
                                }


                                #region Load Issuance and Balance

                                int? Qty = 0;
                                decimal? Amount = 0;

                                foreach (RepeaterItem item1 in rptChildGroupBudgetAssociation.Items)
                                {
                                    objMyIssuanceCart = objMyIssuanceCartRepo.GetByPolicyItemId(objPolicyItemResult[item1.ItemIndex].UniformIssuancePolicyItemID, IncentexGlobal.CurrentMember.UserInfoID);

                                    for (int p = 0; p < objMyIssuanceCart.Count; p++)
                                    {
                                        if (objMyIssuanceCart[p].OrderStatus == "Submitted")
                                        {
                                            Qty += objMyIssuanceCart[p].Qty;
                                        }
                                    }

                                }

                                lblGroupBudgetIssuance.Text = objPolicyItemResult[0].Issuance.ToString();
                                lblGroupBudgetBalanceLeft.Text = (int.Parse(lblGroupBudgetIssuance.Text) - Qty).ToString();

                                lblPurchaseGroupBudget.Text = Convert.ToString((int.Parse(lblGroupBudgetIssuance.Text)) - (int.Parse(lblGroupBudgetBalanceLeft.Text)));

                                if (int.Parse(lblGroupBudgetIssuance.Text) <= int.Parse(lblPurchaseGroupBudget.Text))
                                {
                                    lblGroupBudgetBalanceLeft.Text = "0";
                                }


                                foreach (RepeaterItem item1 in rptChildGroupBudgetAssociation.Items)
                                {
                                    objMyIssuanceCart = objMyIssuanceCartRepo.GetByPolicyItemId(objPolicyItemResult[item1.ItemIndex].UniformIssuancePolicyItemID, IncentexGlobal.CurrentMember.UserInfoID);

                                    for (int n = 0; n < objMyIssuanceCart.Count; n++)
                                    {
                                        if (objMyIssuanceCart[n].Rate != null)
                                        {
                                            if (objMyIssuanceCart[n].OrderStatus == "Submitted")
                                            {
                                                Amount += objMyIssuanceCart[n].Qty * objMyIssuanceCart[n].Rate;
                                            }
                                        }
                                    }
                                }
                                // Fill Master Item Nuber
                                MasterItemID = Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId);
                                LookupRepository oblLookRepos = new LookupRepository();
                                INC_Lookup objlook = new INC_Lookup();
                                objlook = oblLookRepos.GetById(MasterItemID);
                                Label lblItemNumber = (Label)item.FindControl("lblItemNumber");

                                lblItemNumber.Text = objlook.sLookupName;

                                //Added by Surendar Yadav 4 December 2012
                                HyperLink lnkGroupBudgetSizingChart = (HyperLink)item.FindControl("lnkGroupBudgetSizingChart");
                                TailoringMeasurementsRepository objRep = new TailoringMeasurementsRepository();
                                List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
                                objCount = objRep.GetTailoringMeasurementByStoreProductIDAndMasterItemID(Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId), Convert.ToInt64(objPolicyItemResult[item.ItemIndex].StoreProductid));
                                if (objCount.Count > 0)
                                {
                                    if (objCount[0].TailoringMeasurementChart != "")
                                    {
                                        lnkGroupBudgetSizingChart.Visible = true;
                                        lnkGroupBudgetSizingChart.NavigateUrl = "../UploadedImages/TailoringMeasurement/" + objCount[0].TailoringMeasurementChart;
                                    }
                                    else
                                    {
                                        lnkGroupBudgetSizingChart.Visible = false;
                                        lnkGroupBudgetSizingChart.NavigateUrl = "";
                                    }
                                }
                                else
                                {
                                    lnkGroupBudgetSizingChart.Visible = false;
                                    lnkGroupBudgetSizingChart.NavigateUrl = "";
                                }
                                //End by Surendar Yadav 4 December 2012


                                if (objPolicyItemResult[0].AssociationbudgetAmt != null)
                                {
                                    lblGroupBudgetAmount.Text = objPolicyItemResult[0].AssociationbudgetAmt.ToString();
                                    lblGroupBudgetAmountLeft.Text = (decimal.Parse(lblGroupBudgetAmount.Text) - Amount).ToString();

                                    lblBudgetUsed.Text = Convert.ToString(decimal.Parse(lblGroupBudgetAmount.Text) - decimal.Parse(lblGroupBudgetAmountLeft.Text));
                                    if (Convert.ToDecimal(lblBudgetUsed.Text) >= decimal.Parse(lblGroupBudgetAmount.Text))
                                    {
                                        lblGroupBudgetAmountLeft.Text = "0";
                                    }


                                }

                                #endregion

                                #region Size Dropdown Fill

                                foreach (RepeaterItem item1 in rptChildGroupBudgetAssociation.Items)
                                {
                                    MasterItemID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId);
                                    objProdItem = objProdItemDetRepo.GetAllProductItem((int)MasterItemID);

                                    #region Ankit Updated on 10 Feb For Name Bars For Group Budget Association
                                    string Stylename = objLookupRepo.GetById(objProdItem[0].ProductStyleID).sLookupName.ToString();

                                    string ProdcutStyleName = objLookupRepo.GetById(objProdItem[0].ItemSizeID).sLookupName.ToString();
                                    if (Stylename == "Name Bars")
                                    {
                                        if (ProdcutStyleName == "No Size")
                                        {
                                            this.NameFormat = objProdItem[0].NameFormatForNameBars;
                                            this.FontFormat = objProdItem[0].FontFormatForNameBars;
                                            this.FName = IncentexGlobal.CurrentMember.FirstName;
                                            this.LName = IncentexGlobal.CurrentMember.LastName;
                                            this.FinalNameBarStyleForBudgetAssociation = SetNameFontFormat();

                                            /*Employee Title added on 16 Feb :( */
                                            CompanyEmployee objCompanyEmployee = new CompanyEmployee();
                                            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
                                            LookupRepository objLookupRepoForTitle = new LookupRepository();
                                            objCompanyEmployee = objEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                                            if (objCompanyEmployee.EmployeeTitleId != null)
                                                this.FinalNameBarStyleForBudgetAssociation = this.FinalNameBarStyleForBudgetAssociation + "," + objLookupRepoForTitle.GetById((long)objCompanyEmployee.EmployeeTitleId).sLookupName.ToString();

                                            /*End*/

                                            Panel pnlNameBarsForBudgetAssociation = new Panel();
                                            pnlNameBarsForBudgetAssociation = (Panel)item1.FindControl("pnlNameBarsForBudgetAssociation");
                                            if (this.FinalNameBarStyleForBudgetAssociation != string.Empty)
                                            {
                                                pnlNameBarsForBudgetAssociation.Visible = true;
                                                if (this.FinalNameBarStyleForBudgetAssociation.Contains(','))
                                                {

                                                    string[] EmployeeTitle = (this.FinalNameBarStyleForBudgetAssociation.Split(','));
                                                    ((Label)item1.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text = EmployeeTitle[0];
                                                    ((Label)item1.FindControl("lblEmplTitleForBudgetAssociation")).Text = EmployeeTitle[1];
                                                }
                                                else
                                                {
                                                    ((Label)item1.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text = this.FinalNameBarStyleForBudgetAssociation;
                                                }
                                            }
                                            else
                                            {
                                                pnlNameBarsForBudgetAssociation.Visible = false;

                                            }

                                        }
                                    }
                                    #endregion

                                    DropDownList ddlSize = new DropDownList();
                                    ddlSize = (DropDownList)item.FindControl("ddlSize");
                                     HiddenField hdnStoreProductid=(HiddenField)item1.FindControl("hdnStoreProductid");

                                    //Add Nagmani 16 Nov 2011
                                    string ItemNumberSatus = null;
                                    //End Nagmani 16 Nov 2011
                                    var SizeList = (from s in objProdItem where s.ProductId==Convert.ToInt64(hdnStoreProductid.Value)
                                                    select new { s.ItemSizeID, s.ItemNumberStatusID }).OrderBy(s => s.ItemSizeID).Distinct().ToList();
                                    // foreach (var prod in objProdItem)
                                    foreach (var prod in SizeList)
                                    {

                                        //Add Nagmani 16 Nov 2011
                                        ItemNumberSatus = Convert.ToString(objLookupRepo.GetById(Convert.ToInt32(prod.ItemNumberStatusID)).sLookupName);
                                        //End Nagmani 16 Nov 2011
                                        if (ItemNumberSatus != "InActive")
                                        {
                                            ddlSize.Items.Add(new ListItem((objLookupRepo.GetById(prod.ItemSizeID).sLookupName).ToString(), (objLookupRepo.GetById(prod.ItemSizeID).iLookupID).ToString()));
                                        }
                                    }
                                    // Fill Master Item Nuber
                                    MasterItemID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId);
                                    LookupRepository oblLookRepos1 = new LookupRepository();
                                    INC_Lookup objlook1 = new INC_Lookup();
                                    objlook1 = oblLookRepos1.GetById(MasterItemID);
                                    Label lblItemNumber1 = (Label)item1.FindControl("lblItemNumber");

                                    lblItemNumber1.Text = objlook1.sLookupName;


                                    //Add Nagmani ItemNumber 4 july 2011
                                    Boolean boolTrue = false;
                                    HiddenField hdnItemNumber = (HiddenField)item1.FindControl("hdnBudgetItemNumber");
                                    List<SELECTITEMNUMBERONSIZEMASTERNOResult> OBLISTITME = new List<SELECTITEMNUMBERONSIZEMASTERNOResult>();
                                    UniformIssuancePolicyItemRepository OBJREPOSTITEM = new UniformIssuancePolicyItemRepository();
                                    OBLISTITME = OBJREPOSTITEM.GETITEMNUMBER(Convert.ToInt32(MasterItemID), Convert.ToInt32(ddlSize.SelectedValue),Convert.ToInt32(hdnStoreProductid.Value));
                                    if (OBLISTITME.Count > 0)
                                    {
                                        for (int i = 0; i < OBLISTITME.Count; i++)
                                        {
                                            if (boolTrue != true)
                                            {
                                                hdnItemNumber.Value = OBLISTITME[i].ITEMNUMBER.ToString();
                                                boolTrue = true;
                                            }
                                        }
                                    }

                                    //End Nagmani ItemNumber 4 july 2011 if(OBLISTITME1.Count>0)




                                    //Used method for Display Price
                                    List<ProductItemPricing> objPricing = new List<ProductItemPricing>();

                                    objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(MasterItemID, long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                    if (objPricing.Count > 0)
                                    {
                                        Label lblPrice = (Label)item.FindControl("lblGroupBudgetPrice");
                                        if (priceleve == "1")
                                        {
                                            lblPrice.Text = Convert.ToString(objPricing[0].Level1);
                                        }
                                        else if (priceleve == "2")
                                        {
                                            lblPrice.Text = Convert.ToString(objPricing[0].Level2);
                                        }
                                        else
                                        {
                                            lblPrice.Text = Convert.ToString(objPricing[0].Level3);
                                        }

                                    }


                                    //Start Check Here If User Ordered Has Canceled 28-July-2011
                                    //Check here for if balance is Zero then
                                    //enabled teqty false or true

                                    TextBox txtQty = (TextBox)item1.FindControl("txtQty");

                                    //if (Convert.ToDecimal(lblGroupBudgetAmountLeft.Text) == 0)
                                    //{
                                    //    txtQty.Enabled = false;
                                    //}
                                    //else
                                    //{
                                    //    txtQty.Enabled = true;
                                    //}

                                    //Saurabh--Check for Partial Issuance Status
                                    CompanyEmployee objEmpType = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                                    if (objEmpType.IssuancePolicyStatus == 'P')
                                    {
                                        objOrderlist = objOrderConfirmRepost.CheckUserIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                                    }
                                    else
                                    {
                                        objOrderlist = objOrderConfirmRepost.CheckUserIDWorkgroupIDEmpIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.EmployeeTypeID,this.WorkgroupID);
                                    }
                                        if (objOrderlist.Count > 0)
                                    {
                                        if (Convert.ToDecimal(lblGroupBudgetAmountLeft.Text) == 0)
                                        {
                                            txtQty.Enabled = false;

                                        }
                                        else
                                        {
                                            txtQty.Enabled = true;

                                        }
                                    }
                                    else
                                    {
                                        lblGroupBudgetAmountLeft.Text = lblGroupBudgetAmount.Text;
                                        lblBudgetUsed.Text = "0";
                                        txtQty.Enabled = true;
                                    }

                                    //End Check Here If User Ordered Has Canceled 28-July-2011



                                    // CHECK TAILORING OPTION TRUE FALASE
                                    MasterItemID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId);
                                    Panel pnlTailoring = (Panel)item1.FindControl("pnlTailoringGroupBudget");
                                    List<SelectTailoringOptionActiveInActiveResult> objTailoring;
                                    objTailoring = objPolicyItemRepo.GetTailoringActiveInActive(Convert.ToInt32(MasterItemID), int.Parse(ddlSize.SelectedValue),Convert.ToInt32(hdnStoreProductid.Value));

                                    if (objTailoring.Count > 0)
                                    {
                                        if (objTailoring[0].sLookupName == "Active")
                                        {
                                            pnlTailoring.Visible = true;
                                        }
                                        else
                                        {
                                            pnlTailoring.Visible = false;
                                        }
                                    }


                                }

                                #endregion

                                //Start Modify Nagmani 06-May-2011
                                //Check Here This User Has Taken any Issuance Policy the He can not take other issuance policy
                                objOrderlist = objOrderConfirmRepost.CheckUserIDWorkgroupIDEmpIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.EmployeeTypeID,this.WorkgroupID);
                                if (objOrderlist.Count > 0)
                                {
                                    lblNoRecord.Text = "You have already purchased issuance package.";
                                    dvNoRecord.Visible = true;
                                    btnCheckOut.Visible = false;
                                }
                                else
                                {
                                    if (dtEligbleDate > DateTime.Now && dtExpiryDate >= DateTime.Now)
                                    {
                                        btnCheckOut.Visible = false;
                                        divDateEligible.Visible = true;
                                        lblDateEligible.Text = "Your Policy Is Not Yet Eligible:" + " " + lblEligable.Text;

                                    }
                                    else
                                    {
                                        btnCheckOut.Visible = true;
                                        divDateEligible.Visible = false;
                                        lblDateEligible.Text = "";
                                        lblNoRecord.Text = "";
                                        dvNoRecord.Visible = false;

                                    }
                                }



                            }
                            else
                            {
                                dvGroupBudget.Visible = false;
                                pnlGroupBudget.Visible = false;
                            }
                        }
                        // Calculate by Date
                        else
                        {
                            if (objPolicy.EligibleDate != null)
                            {
                                if ((objPolicy.EligibleDate <= DateTime.Now || objPolicy.EligibleDate > DateTime.Now) && objPolicy.CreditExpireDate >= DateTime.Now)
                                {
                                    // dvSingle.Visible = true;
                                    //pnlSingle.Visible = true;
                                    dvGroupBudget.Visible = true;
                                    pnlGroupBudget.Visible = true;
                                    for (int u = 0; u < objPolicyItemResult.Count; u++)
                                    {
                                        if (objPolicyItemResult[u].AssociationIssuancePolicyNote != "")
                                        {
                                            //lblGroupItemAssociationBudget.Text = objPolicyItemResult[u].AssociationIssuancePolicyNote;
                                            lblGroupItemAssociationBudget.Text = "Group With Budget Policy";
                                            lblGroupItemAssociationBudgetParent.Text = objPolicyItemResult[u].AssociationIssuancePolicyNote;
                                        }
                                    }
                                    //rptChildGroupBudgetAssociation.DataSource = objPolicyItemResult;
                                    //rptChildGroupBudgetAssociation.DataBind();

                                    #region Load Issuance and Balance

                                    int? Qty = 0;
                                    decimal? Amount = 0;

                                    foreach (RepeaterItem item1 in rptChildGroupBudgetAssociation.Items)
                                    {
                                        objMyIssuanceCart = objMyIssuanceCartRepo.GetByPolicyItemId(objPolicyItemResult[item1.ItemIndex].UniformIssuancePolicyItemID, IncentexGlobal.CurrentMember.UserInfoID);

                                        for (int q = 0; q < objMyIssuanceCart.Count; q++)
                                        {
                                            if (objMyIssuanceCart[q].OrderStatus == "Submitted")
                                            {
                                                Qty += objMyIssuanceCart[q].Qty;
                                            }
                                        }



                                    }

                                    lblGroupBudgetIssuance.Text = objPolicyItemResult[0].Issuance.ToString();
                                    lblGroupBudgetBalanceLeft.Text = (int.Parse(lblGroupBudgetIssuance.Text) - Qty).ToString();

                                    foreach (RepeaterItem item1 in rptChildGroupBudgetAssociation.Items)
                                    {
                                        objMyIssuanceCart = objMyIssuanceCartRepo.GetByPolicyItemId(objPolicyItemResult[item1.ItemIndex].UniformIssuancePolicyItemID, IncentexGlobal.CurrentMember.UserInfoID);

                                        for (int k = 0; k < objMyIssuanceCart.Count; k++)
                                        {
                                            if (objMyIssuanceCart[k].Rate != null)
                                            {
                                                if (objMyIssuanceCart[k].OrderStatus == "Submitted")
                                                {
                                                    Amount += objMyIssuanceCart[k].Qty * objMyIssuanceCart[k].Rate;
                                                }
                                            }
                                        }
                                    }
                                    if (objPolicyItemResult[0].AssociationbudgetAmt != null)
                                    {
                                        lblGroupBudgetAmount.Text = objPolicyItemResult[0].AssociationbudgetAmt.ToString();
                                        lblGroupBudgetAmountLeft.Text = (decimal.Parse(lblGroupBudgetAmount.Text) - Amount).ToString();

                                        lblPurchaseGroupBudget.Text = Convert.ToString((int.Parse(lblGroupBudgetIssuance.Text)) - (int.Parse(lblGroupBudgetBalanceLeft.Text)));
                                        lblPurchaseGroupBudget.Text = Convert.ToString((int.Parse(lblGroupBudgetIssuance.Text)) - (int.Parse(lblGroupBudgetBalanceLeft.Text)));
                                        if (int.Parse(lblGroupBudgetIssuance.Text) <= int.Parse(lblPurchaseGroupBudget.Text))
                                        {
                                            lblGroupBudgetBalanceLeft.Text = "0";
                                        }

                                        lblBudgetUsed.Text = Convert.ToString(decimal.Parse(lblGroupBudgetAmount.Text) - decimal.Parse(lblGroupBudgetAmountLeft.Text));
                                        if (Convert.ToDecimal(lblBudgetUsed.Text) >= decimal.Parse(lblGroupBudgetAmount.Text))
                                        {
                                            lblGroupBudgetAmountLeft.Text = "0";
                                        }

                                    }

                                    #endregion

                                    #region Size Dropdown Fill

                                    foreach (RepeaterItem item1 in rptChildGroupBudgetAssociation.Items)
                                    {
                                        MasterItemID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId);
                                        objProdItem = objProdItemDetRepo.GetAllProductItem((int)MasterItemID);

                                        #region Ankit Updated on 10 Feb For Name Bars For Group Budget Association

                                        string Stylename = objLookupRepo.GetById(objProdItem[0].ProductStyleID).sLookupName.ToString();

                                        string ProdcutStyleName = objLookupRepo.GetById(objProdItem[0].ItemSizeID).sLookupName.ToString();
                                        if (Stylename == "Name Bars")
                                        {
                                            if (ProdcutStyleName == "No Size")
                                            {
                                                this.NameFormat = objProdItem[0].NameFormatForNameBars;
                                                this.FontFormat = objProdItem[0].FontFormatForNameBars;
                                                this.FName = IncentexGlobal.CurrentMember.FirstName;
                                                this.LName = IncentexGlobal.CurrentMember.LastName;
                                                this.FinalNameBarStyleForBudgetAssociation = SetNameFontFormat();

                                                /*Employee Title added on 16 Feb :( */
                                                CompanyEmployee objCompanyEmployee = new CompanyEmployee();
                                                CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
                                                LookupRepository objLookupRepoForTitle = new LookupRepository();
                                                objCompanyEmployee = objEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                                                if (objCompanyEmployee.EmployeeTitleId != null)
                                                    this.FinalNameBarStyleForBudgetAssociation = this.FinalNameBarStyleForBudgetAssociation + "," + objLookupRepoForTitle.GetById((long)objCompanyEmployee.EmployeeTitleId).sLookupName.ToString();

                                                /*End*/

                                                Panel pnlNameBarsForBudgetAssociation = new Panel();
                                                pnlNameBarsForBudgetAssociation = (Panel)item1.FindControl("pnlNameBarsForBudgetAssociation");
                                                if (this.FinalNameBarStyleForBudgetAssociation != string.Empty)
                                                {
                                                    pnlNameBarsForBudgetAssociation.Visible = true;
                                                    if (this.FinalNameBarStyleForBudgetAssociation.Contains(','))
                                                    {

                                                        string[] EmployeeTitle = (this.FinalNameBarStyleForBudgetAssociation.Split(','));
                                                        ((Label)item1.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text = EmployeeTitle[0];
                                                        ((Label)item1.FindControl("lblEmplTitleForBudgetAssociation")).Text = EmployeeTitle[1];
                                                    }
                                                    else
                                                    {
                                                        ((Label)item1.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text = this.FinalNameBarStyleForBudgetAssociation;
                                                    }
                                                }
                                                else
                                                {
                                                    pnlNameBarsForBudgetAssociation.Visible = false;

                                                }

                                            }
                                        }
                                        #endregion

                                        DropDownList ddlSize = new DropDownList();
                                        ddlSize = (DropDownList)item1.FindControl("ddlSize");

                                         HiddenField hdnStoreProductid=(HiddenField)item1.FindControl("hdnStoreProductid");
                                        //Add Nagmani 16 Nov 2011
                                        string ItemNumberSatus = null;
                                        //End Nagmani 16 Nov 2011
                                        var SizeList = (from s in objProdItem where s.ProductId==Convert.ToInt64(hdnStoreProductid.Value)
                                                        select new { s.ItemSizeID, s.ItemNumberStatusID }).OrderBy(s => s.ItemSizeID).Distinct().ToList();
                                        // foreach (var prod in objProdItem)
                                        foreach (var prod in SizeList)
                                        {

                                            //Add Nagmani 16 Nov 2011
                                            ItemNumberSatus = Convert.ToString(objLookupRepo.GetById(Convert.ToInt32(prod.ItemNumberStatusID)).sLookupName);
                                            //End Nagmani 16 Nov 2011
                                            if (ItemNumberSatus != "InActive")
                                            {
                                                ddlSize.Items.Add(new ListItem((objLookupRepo.GetById(prod.ItemSizeID).sLookupName).ToString(), (objLookupRepo.GetById(prod.ItemSizeID).iLookupID).ToString()));
                                            }
                                        }
                                        // Fill Master Item Nuber
                                        MasterItemID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId);
                                        LookupRepository oblLookRepos = new LookupRepository();
                                        INC_Lookup objlook = new INC_Lookup();
                                        objlook = oblLookRepos.GetById(MasterItemID);
                                        Label lblItemNumber = (Label)item1.FindControl("lblItemNumber");

                                        lblItemNumber.Text = objlook.sLookupName;

                                        //Added by Surendar Yadav 4 December 2012
                                        HyperLink lnkGroupBudgetSizingChart = (HyperLink)item1.FindControl("lnkGroupBudgetSizingChart");
                                        TailoringMeasurementsRepository objRep = new TailoringMeasurementsRepository();
                                        List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
                                        objCount = objRep.GetTailoringMeasurementByStoreProductIDAndMasterItemID(Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId), Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].StoreProductid));
                                        if (objCount.Count > 0)
                                        {
                                            if (objCount[0].TailoringMeasurementChart != "")
                                            {
                                                lnkGroupBudgetSizingChart.Visible = true;
                                                lnkGroupBudgetSizingChart.NavigateUrl = "../UploadedImages/TailoringMeasurement/" + objCount[0].TailoringMeasurementChart;
                                            }
                                            else
                                            {
                                                lnkGroupBudgetSizingChart.Visible = false;
                                                lnkGroupBudgetSizingChart.NavigateUrl = "";
                                            }
                                        }
                                        else
                                        {
                                            lnkGroupBudgetSizingChart.Visible = false;
                                            lnkGroupBudgetSizingChart.NavigateUrl = "";
                                        }
                                        //End by Surendar Yadav 4 December 2012

                                        //Add Nagmani ItemNumber 4 july 2011
                                        HiddenField hdnItemNumber = (HiddenField)item1.FindControl("hdnBudgetItemNumber");
                                        List<SELECTITEMNUMBERONSIZEMASTERNOResult> OBLISTITME = new List<SELECTITEMNUMBERONSIZEMASTERNOResult>();
                                        UniformIssuancePolicyItemRepository OBJREPOSTITEM = new UniformIssuancePolicyItemRepository();
                                        OBLISTITME = OBJREPOSTITEM.GETITEMNUMBER(Convert.ToInt32(MasterItemID), Convert.ToInt32(ddlSize.SelectedValue),Convert.ToInt32(hdnStoreProductid.Value));
                                        Boolean boolTrue = false;
                                        if (OBLISTITME.Count > 0)
                                        {
                                            for (int i = 0; i < OBLISTITME.Count; i++)
                                            {
                                                if (boolTrue != true)
                                                {
                                                    hdnItemNumber.Value = OBLISTITME[i].ITEMNUMBER.ToString();
                                                    boolTrue = true;
                                                }
                                            }
                                        }

                                        //End Nagmani ItemNumber 4 july 2011




                                        //Used method for Display Price
                                        List<ProductItemPricing> objPricing = new List<ProductItemPricing>();

                                        objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(MasterItemID, long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                        if (objPricing.Count > 0)
                                        {
                                            Label lblPrice = (Label)item1.FindControl("lblGroupBudgetPrice");
                                            if (priceleve == "1")
                                            {
                                                lblPrice.Text = Convert.ToString(objPricing[0].Level1);
                                            }
                                            else if (priceleve == "2")
                                            {
                                                lblPrice.Text = Convert.ToString(objPricing[0].Level2);
                                            }
                                            else
                                            {
                                                lblPrice.Text = Convert.ToString(objPricing[0].Level3);
                                            }

                                        }

                                        //Start Check Here If User Ordered Has Canceled 28-July-2011
                                        //Check here for if balance is Zero then
                                        //enabled teqty false or true

                                        TextBox txtQty = (TextBox)item1.FindControl("txtQty");

                                        //if (Convert.ToDecimal(lblGroupBudgetAmountLeft.Text) == 0)
                                        //{
                                        //    txtQty.Enabled = false;
                                        //}
                                        //else
                                        //{
                                        //    txtQty.Enabled = true;
                                        //}

                                        //Saurabh--Check for Partial Issuance Status
                                    CompanyEmployee objEmpType = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                                    if (objEmpType.IssuancePolicyStatus == 'P')
                                    {
                                        objOrderlist = objOrderConfirmRepost.CheckUserIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                                    }
                                    else
                                    {
                                        objOrderlist = objOrderConfirmRepost.CheckUserIDWorkgroupIDEmpIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.EmployeeTypeID,this.WorkgroupID);
                                    }
                                        if (objOrderlist.Count > 0)
                                        {
                                            if (Convert.ToDecimal(lblGroupBudgetAmountLeft.Text) == 0)
                                            {
                                                txtQty.Enabled = false;

                                            }
                                            else
                                            {
                                                txtQty.Enabled = true;

                                            }
                                        }
                                        else
                                        {
                                            lblGroupBudgetAmountLeft.Text = lblGroupBudgetAmount.Text;
                                            lblBudgetUsed.Text = "0";
                                            txtQty.Enabled = true;
                                        }

                                        //End Check Here If User Ordered Has Canceled 28-July-2011

                                        // CHECK TAILORING OPTION TRUE FALASE
                                        MasterItemID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId);
                                        Panel pnlTailoring = (Panel)item1.FindControl("pnlTailoringGroupBudget");
                                        List<SelectTailoringOptionActiveInActiveResult> objTailoring;
                                        objTailoring = objPolicyItemRepo.GetTailoringActiveInActive(Convert.ToInt32(MasterItemID), int.Parse(ddlSize.SelectedValue),Convert.ToInt32(hdnStoreProductid.Value));

                                        if (objTailoring.Count > 0)
                                        {
                                            if (objTailoring[0].sLookupName == "Active")
                                            {
                                                pnlTailoring.Visible = true;
                                            }
                                            else
                                            {
                                                pnlTailoring.Visible = false;
                                            }
                                        }
                                    }

                                    #endregion

                                    //Start Modify Nagmani 06-May-2011
                                    //Check Here This User Has Taken any Issuance Policy the He can not take other issuance policy
                                    objOrderlist = objOrderConfirmRepost.CheckUserIDWorkgroupIDEmpIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.EmployeeTypeID,this.WorkgroupID);
                                    if (objOrderlist.Count > 0)
                                    {
                                        btnCheckOut.Visible = false;
                                    }
                                    else
                                    {
                                        //Check here for checkout button will show or Not on the Eligible date.
                                        if (objPolicy.EligibleDate > DateTime.Now && objPolicy.CreditExpireDate >= DateTime.Now)
                                        {
                                            btnCheckOut.Visible = false;
                                            divDateEligible.Visible = true;
                                            lblDateEligible.Text = "Your Policy Is Not Yet Eligible:" + " " + lblEligable.Text;
                                        }
                                        else
                                        {
                                            btnCheckOut.Visible = true;
                                            divDateEligible.Visible = false;
                                            lblDateEligible.Text = "";
                                        }
                                    }

                                }
                                else
                                {
                                    dvGroupBudget.Visible = false;
                                    pnlGroupBudget.Visible = false;
                                }
                            }
                            else
                            {
                                dvGroupBudget.Visible = false;
                                pnlGroupBudget.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        dvGroupBudget.Visible = false;
                        pnlGroupBudget.Visible = false;
                    }
                }

                //else
                //{
                //    dvGroupBudget.Visible = false;
                //    pnlGroupBudget.Visible = false;
                //}

            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }   
    protected void ProcessGroupBudgetAssociation()
    {
        try
        {
            string NameToBeEngravedForBudget = string.Empty;
            decimal? GroupBudgetTotal = 0;
            StringBuilder sbGroupBudgetCartIDs = new StringBuilder();
            sbGroupBudgetCartIDs.Append(string.Empty);
            foreach (RepeaterItem item in rptGroupBudgetAssociation.Items)
            {
                Repeater rptChildGroupBudgetAssociation = (Repeater)item.FindControl("rptChildGroupBudgetAssociation");
                HiddenField hdnNEWGROUP = (HiddenField)item.FindControl("hdnNEWGROUP");
                Label lblGroupBudgetAmountLeft = (Label)item.FindControl("lblGroupBudgetAmountLeft");
                Label lblGroupBudgetBalanceLeft = (Label)item.FindControl("lblGroupBudgetBalanceLeft");
            if (dvGroupBudget.Visible == true && pnlGroupBudget.Visible == true)
            {
                decimal? rate = 0;
                int totalqty = 0;
                decimal? amount = 0;

                IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Group Association with Budget");



                // Update Chnage Nagmani 02-02-2012
                //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(z => z.AssociationIssuanceType == IssuanceTypeID).ToList();

               
                string[] MyWeatherlist = null;
                List<UniformIssuancePolicyItem> objMainList = new List<UniformIssuancePolicyItem>();
                List<SelectWeatherTypeIdResult> objWeatherlsit = new List<SelectWeatherTypeIdResult>();
                //Select Basestation and Employeetype Id from table companyemployee on login userid
                int intEmployeeType = 0;
                long logBaseStation = 0;
                string strweather = "0";
                objCmpEmp = objCmpEmpRepo.GetByUserInfoId(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                if (objCmpEmp != null)
                {
                    intEmployeeType = Convert.ToInt32(objCmpEmp.EmployeeTypeID);
                    logBaseStation = Convert.ToInt64(objCmpEmp.BaseStation);

                    if (objCmpEmp.EmployeeTypeID != null && objCmpEmp.BaseStation != null)
                    {

                        //Select Weather from InceBaseStation using basestation parameter
                        objWeatherlsit = objPolicyItemRepo.GetWeatherTypeid(Convert.ToInt32(logBaseStation));
                        if (objWeatherlsit.Count > 0)
                        {
                            if (objWeatherlsit[0].WEATHERTYPE != null)
                            {
                                MyWeatherlist = objWeatherlsit[0].WEATHERTYPE.Split(',');

                                for (int i = 0; i < MyWeatherlist.Count(); i++)
                                {
                                    if (i == 0)
                                    {

                                        strweather = "'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }
                                    else
                                    {

                                        strweather += ",'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }

                                }
                                objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == intEmployeeType || j.EmployeeTypeid == 0)).ToList();

                                //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0)).ToList();
                            }

                        }

                    }

                    else if (objCmpEmp.EmployeeTypeID == null && objCmpEmp.BaseStation != null)
                    {
                        intEmployeeType = 0;
                        logBaseStation = Convert.ToInt64(objCmpEmp.BaseStation);

                        //Select Weather from InceBaseStation using basestation parameter
                        objWeatherlsit = objPolicyItemRepo.GetWeatherTypeid(Convert.ToInt32(logBaseStation));
                        if (objWeatherlsit.Count > 0)
                        {
                            if (objWeatherlsit[0].WEATHERTYPE != null)
                            {
                                MyWeatherlist = objWeatherlsit[0].WEATHERTYPE.Split(',');

                                for (int i = 0; i < MyWeatherlist.Count(); i++)
                                {
                                    if (i == 0)
                                    {

                                        strweather = "'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }
                                    else
                                    {

                                        strweather += ",'" + Convert.ToString(MyWeatherlist[i]) + "'";
                                    }

                                }

                                objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (strweather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == 0)).ToList();
                            }



                        }

                    }

                }
                else
                {
                    intEmployeeType = 0;
                    logBaseStation = 0;
                }

                //End Nagmani 

                objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
                Purchaserequired = Convert.ToString(objPolicy.CompletePurchase);
                if (objPolicyItemResult.Count > 0)
                {
                    foreach (RepeaterItem item1 in rptChildGroupBudgetAssociation.Items)
                    {
                         HiddenField hdnStoreProductid=(HiddenField)item1.FindControl("hdnStoreProductid");
                        DropDownList ddlSize = new DropDownList();
                        ddlSize = (DropDownList)item1.FindControl("ddlSize");
                        Label lblPrice = (Label)item1.FindControl("lblGroupBudgetPrice");
                        TextBox txtQty = new TextBox();
                        txtQty = (TextBox)item1.FindControl("txtQty");
                        if (txtQty.Text != string.Empty && txtQty.Text != "0")
                        {
                            if (int.Parse(txtQty.Text) > 0)
                                totalqty += int.Parse(txtQty.Text);
                        }

                        rate = objMyIssuanceCartRepo.GetPriceByMasterItemIdandItemSizeId(Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupBudgetAssociationMasterItemId")).Value), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));

                        if (lblPrice.Text != "")
                        {
                            if (txtQty.Text != string.Empty && txtQty.Text != "0")
                                amount += int.Parse(txtQty.Text) * decimal.Parse(lblPrice.Text);
                        }



                    }

                    //Check Here if Purchase complete required is True
                    if (Purchaserequired == "Y")
                    {
                        //if(totalqty == int.Parse(lblGroupBudgetBalanceLeft.Text) && amount == decimal.Parse(lblGroupBudgetAmountLeft.Text))
                        if (amount == decimal.Parse(lblGroupBudgetAmountLeft.Text))
                        {
                            foreach (RepeaterItem item1 in rptChildGroupBudgetAssociation.Items)
                            {
                                HiddenField hdnStoreProductid=(HiddenField)item1.FindControl("hdnStoreProductid");
                                Label lblItemNumber = (Label)item1.FindControl("lblItemNumber");
                                TextBox txtQty = new TextBox();
                                txtQty = (TextBox)item1.FindControl("txtQty");
                                Label lblPrice = (Label)item1.FindControl("lblGroupBudgetPrice");
                                DropDownList ddlSize = new DropDownList();
                                ddlSize = (DropDownList)item1.FindControl("ddlSize");
                                TextBox txtTailoring = (TextBox)item1.FindControl("txtBudgetTailoringlenght");
                                //Add Nagmani ItemNumber 4 july 2011
                                HiddenField hdnItemNumber = (HiddenField)item1.FindControl("hdnBudgetItemNumber");
                               
                                //End Nagmani ItemNumber 4 july 2011
                                //Created By Ankit For NameBars
                                if (Convert.ToString(((Label)item1.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text) != string.Empty)
                                {
                                    if (Convert.ToString(((Label)item1.FindControl("lblEmplTitleForBudgetAssociation")).Text) != string.Empty)
                                    {
                                        NameToBeEngravedForBudget = Convert.ToString(((Label)item1.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text) + "," + Convert.ToString(((Label)item1.FindControl("lblEmplTitleForBudgetAssociation")).Text);
                                    }
                                    else
                                    {
                                        NameToBeEngravedForBudget = Convert.ToString(((Label)item1.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text);
                                    }
                                }
                                else
                                {
                                    NameToBeEngravedForBudget = null;
                                }

                                //End

                                if (txtQty.Text != string.Empty && txtQty.Text != "0")
                                {
                                    if (int.Parse(txtQty.Text) > 0)
                                    {
                                        lblGroupBudgetMsg.Visible = false;
                                        MyIssuanceCart objCart = new MyIssuanceCart();
                                        objCart.UniformIssuancePolicyItemID = objPolicyItemResult[item1.ItemIndex].UniformIssuancePolicyItemID;
                                        objCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                                        objCart.UniformIssuanceType = (long)IssuanceTypeID;
                                        objCart.MasterItemID = Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupBudgetAssociationMasterItemId")).Value);
                                        objCart.ItemSizeID = Convert.ToInt64(ddlSize.SelectedValue);
                                        objCart.Qty = int.Parse(txtQty.Text);
                                        objCart.NameToBeEngraved = NameToBeEngravedForBudget;
                                        rate = objMyIssuanceCartRepo.GetPriceByMasterItemIdandItemSizeId(Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupBudgetAssociationMasterItemId")).Value), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                        objCart.Rate = decimal.Parse(lblPrice.Text);
                                        if (txtShippingDate.Text != string.Empty)
                                            objCart.ShippingDate = Convert.ToDateTime(txtShippingDate.Text);
                                        else
                                            objCart.ShippingDate = null;
                                        if (txtTailoring.Text != "")
                                        {
                                            objCart.TailoringLength = txtTailoring.Text;
                                        }
                                        if (objPolicyItemResult[item1.ItemIndex].StoreProductid != null)
                                        {
                                            objCart.StoreProductID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].StoreProductid);
                                        }
                                         objCart.PriceLevel = Convert.ToInt32(priceleve);
                                       
                                        if (hdnItemNumber.Value != "")
                                        {
                                            objCart.ItemNumber = hdnItemNumber.Value;
                                        }
                                        List<StoreProduct> backorder;
                                        backorder = objMyIssuanceCartRepo.GetBackorder(Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupBudgetAssociationMasterItemId")).Value), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                        if (backorder.Count > 0)
                                        {
                                            int Inventory = 0;
                                            List<ProductItemInventory> backorder1;

                                            backorder1 = objMyIssuanceCartRepo.GrtBackOrder1(Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupBudgetAssociationMasterItemId")).Value), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                            if (backorder1.Count > 0)
                                            {
                                                Inventory = Convert.ToInt32(backorder1[0].Inventory);
                                            }
                                            //End Modify Nagmani 05-May-2011
                                            objLook = objLookupRepo.GetById(Convert.ToInt16(backorder[0].AllowBackOrderID));
                                            if (objLook != null)
                                            {
                                                //Check here Backorder is yes
                                                if (Inventory < int.Parse(lblGroupBudgetBalanceLeft.Text) && objLook.sLookupName == "Yes")
                                                {
                                                    objCart.BackOrderStatus = "Y";
                                                    objMyIssuanceCartRepo.Insert(objCart);
                                                    objMyIssuanceCartRepo.SubmitChanges();
                                                    // Store the MyIssuanceCartIDs with comma separted values
                                                     sbGroupBudgetCartIDs.Append(objCart.MyIssuanceCartID + ",");
                                                    // Calculate Group Association with budget Total Amount
                                                     GroupBudgetTotal += decimal.Parse(lblPrice.Text) * objCart.Qty;
                                                    boolMessage = true;
                                                    CompletePurchasedReqiured = true;
                                                }
                                                else if (Inventory < int.Parse(lblGroupBudgetBalanceLeft.Text) && objLook.sLookupName == "No")
                                                {
                                                    lblGroupBudgetMsg.Visible = true;
                                                    lblGroupBudgetMsg.Text = "Inventory Level Is Low.You Can Not Process Order.";
                                                    boolMessage = false;
                                                    CompletePurchasedReqiured = false;
                                                    boolCkeckInventoryNotallowed = false;
                                                }
                                                else
                                                {
                                                    objMyIssuanceCartRepo.Insert(objCart);
                                                    objMyIssuanceCartRepo.SubmitChanges();
                                                    // Store the MyIssuanceCartIDs with comma separted values
                                                    sbGroupBudgetCartIDs.Append(objCart.MyIssuanceCartID + ",");
                                                    // Calculate Group Association with budget Total Amount
                                                   
                                                    GroupBudgetTotal += decimal.Parse(lblPrice.Text) * objCart.Qty;
                                                    boolMessage = true;
                                                    CompletePurchasedReqiured = true;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            CompletePurchasedReqiured = false;
                            lblGroupBudgetMsg.Visible = true;
                            lblGroupBudgetMsg.Text = "Amount Must Be Equal To Budget Amount";
                            boolMessage = false;
                        }
                    }
                    else
                    {
                       
                        if (amount <= decimal.Parse(lblGroupBudgetAmountLeft.Text))
                        {
                            foreach (RepeaterItem item1 in rptChildGroupBudgetAssociation.Items)
                            {
                                HiddenField hdnStoreProductid=(HiddenField)item1.FindControl("hdnStoreProductid");
                                Label lblItemNumber = (Label)item1.FindControl("lblItemNumber");
                                HiddenField hdnItemNumber = (HiddenField)item1.FindControl("hdnBudgetItemNumber");
                                TextBox txtQty = new TextBox();
                                txtQty = (TextBox)item1.FindControl("txtQty");
                                Label lblPrice = (Label)item1.FindControl("lblGroupBudgetPrice");
                                DropDownList ddlSize = new DropDownList();
                                ddlSize = (DropDownList)item1.FindControl("ddlSize");
                                TextBox txtTailoring = (TextBox)item1.FindControl("txtBudgetTailoringlenght");
                                //Created By Ankit For NameBars
                                if (Convert.ToString(((Label)item1.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text) != string.Empty)
                                {
                                    if (Convert.ToString(((Label)item1.FindControl("lblEmplTitleForBudgetAssociation")).Text) != string.Empty)
                                    {
                                        NameToBeEngravedForBudget = Convert.ToString(((Label)item1.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text) + "," + Convert.ToString(((Label)item1.FindControl("lblEmplTitleForBudgetAssociation")).Text);
                                    }
                                    else
                                    {
                                        NameToBeEngravedForBudget = Convert.ToString(((Label)item1.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text);
                                    }
                                }
                                else
                                {
                                    NameToBeEngravedForBudget = null;
                                }

                                //End
                                if (txtQty.Text != string.Empty && txtQty.Text != "0")
                                {
                                    if (int.Parse(txtQty.Text) > 0)
                                    {
                                        // lblSingleMsg.Visible = false;
                                        lblGroupBudgetMsg.Visible = false;
                                        MyIssuanceCart objCart = new MyIssuanceCart();
                                        objCart.UniformIssuancePolicyItemID = objPolicyItemResult[item1.ItemIndex].UniformIssuancePolicyItemID;
                                        objCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                                        objCart.UniformIssuanceType = (long)IssuanceTypeID;
                                        objCart.MasterItemID = Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupBudgetAssociationMasterItemId")).Value);
                                        objCart.ItemSizeID = Convert.ToInt64(ddlSize.SelectedValue);
                                        objCart.Qty = int.Parse(txtQty.Text);
                                        objCart.NameToBeEngraved = NameToBeEngravedForBudget;
                                        rate = objMyIssuanceCartRepo.GetPriceByMasterItemIdandItemSizeId(Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupBudgetAssociationMasterItemId")).Value), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                        objCart.Rate = decimal.Parse(lblPrice.Text);
                                        if (txtShippingDate.Text != string.Empty)
                                            objCart.ShippingDate = Convert.ToDateTime(txtShippingDate.Text);
                                        else
                                            objCart.ShippingDate = null;
                                        if (txtTailoring.Text != "")
                                        {
                                            objCart.TailoringLength = txtTailoring.Text;
                                        }
                                        if (objPolicyItemResult[item1.ItemIndex].StoreProductid != null)
                                        {
                                            objCart.StoreProductID = Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].StoreProductid);
                                        }
                                        objCart.PriceLevel = Convert.ToInt32(priceleve);
                                       
                                        if (hdnItemNumber.Value != "")
                                        {
                                            objCart.ItemNumber = hdnItemNumber.Value;
                                        }
                                      
                                        //Start Modify Nagmani 05-May-2011
                                        //List<ProductItemInventory> backorder;
                                        List<StoreProduct> backorder;
                                        //End Modify Nagmani 05-May-2011
                                        backorder = objMyIssuanceCartRepo.GetBackorder(Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupBudgetAssociationMasterItemId")).Value), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                        if (backorder.Count > 0)
                                        {
                                            int Inventory = 0;
                                            List<ProductItemInventory> backorder1;
                                            backorder1 = objMyIssuanceCartRepo.GrtBackOrder1(Convert.ToInt64(((HiddenField)item1.FindControl("hdnChildGroupBudgetAssociationMasterItemId")).Value), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                                            if (backorder1.Count > 0)
                                            {
                                                Inventory = Convert.ToInt32(backorder1[0].Inventory);
                                            }
                                            objLook = objLookupRepo.GetById(Convert.ToInt16(backorder[0].AllowBackOrderID));
                                            if (objLook != null)
                                            {
                                                //Check here Backorder is yes
                                                if (Inventory < int.Parse(lblGroupBudgetBalanceLeft.Text) && objLook.sLookupName == "Yes")
                                                {
                                                    objCart.BackOrderStatus = "Y";
                                                    objMyIssuanceCartRepo.Insert(objCart);
                                                    objMyIssuanceCartRepo.SubmitChanges();

                                                    // Store the MyIssuanceCartIDs with comma separted values

                                                    sbGroupBudgetCartIDs.Append(objCart.MyIssuanceCartID + ",");

                                                    // Calculate Group Association with budget Total Amount
                                                    //  GroupBudgetTotal += objCart.Rate * objCart.Qty;
                                                    GroupBudgetTotal += decimal.Parse(lblPrice.Text) * objCart.Qty;
                                                    boolMessage = true;

                                                }
                                                else if (Inventory < int.Parse(lblGroupBudgetBalanceLeft.Text) && objLook.sLookupName == "No")
                                                {
                                                    lblGroupBudgetMsg.Visible = true;
                                                    lblGroupBudgetMsg.Text = "Inventory Level Is Low.You Can Not Process Order.";
                                                    boolMessage = false;
                                                   boolCkeckInventoryNotallowed = false;
                                                }
                                                else
                                                {

                                                    objMyIssuanceCartRepo.Insert(objCart);
                                                    objMyIssuanceCartRepo.SubmitChanges();

                                                    // Store the MyIssuanceCartIDs with comma separted values
                                                    sbGroupBudgetCartIDs.Append(objCart.MyIssuanceCartID + ",");
                                                    // Calculate Group Association with budget Total Amount
                                                    
                                                    GroupBudgetTotal += decimal.Parse(lblPrice.Text) * objCart.Qty;
                                                    boolMessage = true;

                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            lblGroupBudgetMsg.Visible = true;
                            lblGroupBudgetMsg.Text = "Amount Can Not Be More Than Budget Left Respectively";
                            boolMessage = false;
                        }
                    }


                    

                    Session["GroupBudgetCartIDs"] = sbGroupBudgetCartIDs.ToString();
                    Session["GroupBudgetTotal"] = GroupBudgetTotal;
                    
                }
            }
            }
            // Remove the last Comma
            if (sbGroupBudgetCartIDs.ToString().Contains(","))
                sbGroupBudgetCartIDs.Remove(sbGroupBudgetCartIDs.ToString().LastIndexOf(","), 1);
            LoadGroupBudgetAssociationData1();
        }
        catch (Exception ex)
        {
            Session["GroupBudgetTotal"] = 0;
            Session["GroupBudgetCartIDs"] = string.Empty;
            ex.Message.ToString();
        }
    }
    #endregion

    //private void CheckPartialIssuanceSingleAsso()
    //{
    //    try
    //    {

    //        foreach (RepeaterItem item in rptSingleAssociation.Items)
    //        {
    //            #region Load Issuance and Balance
    //            TextBox txtQty = (TextBox)item.FindControl("txtQty");
    //            Label lblIssuance = new Label();
    //            lblIssuance = (Label)item.FindControl("lblIssuance");
    //            lblIssuance.Text = objPolicyItemResult[item.ItemIndex].Issuance.ToString();
    //            HiddenField hdnStoreProductid = (HiddenField)item.FindControl("hdnStoreProductid");
    //            Label lblBalance = new Label();
    //            lblBalance = (Label)item.FindControl("lblBalance");
    //            Label lblPurchase = new Label();
    //            lblPurchase = (Label)item.FindControl("lblPurchase");
    //            objMyIssuanceCart = objMyIssuanceCartRepo.GetByPolicyItemId(objPolicyItemResult[item.ItemIndex].UniformIssuancePolicyItemID, IncentexGlobal.CurrentMember.UserInfoID);

    //            if (objMyIssuanceCart != null)
    //            {
    //                int? Qty = 0;

    //                for (int i = 0; i < objMyIssuanceCart.Count; i++)
    //                {
    //                    if (objMyIssuanceCart[i].OrderStatus == "Submitted")
    //                    {
    //                        Qty += objMyIssuanceCart[i].Qty;
    //                    }
    //                }
    //                lblBalance.Text = (objPolicyItemResult[item.ItemIndex].Issuance - Qty).ToString();
    //                lblPurchase.Text = Convert.ToString((objPolicyItemResult[item.ItemIndex].Issuance - (Convert.ToInt32(lblBalance.Text))));
    //                if (int.Parse(lblIssuance.Text) <= int.Parse(lblPurchase.Text))
    //                {
    //                    lblBalance.Text = "0";
    //                }
    //            }
    //            else
    //            {
    //                lblBalance.Text = objPolicyItemResult[item.ItemIndex].Issuance.ToString();
    //                lblPurchase.Text = objPolicyItemResult[item.ItemIndex].Issuance.ToString();
    //            }
    //            objOrderlist = objOrderConfirmRepost.CheckUserIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
    //            if (objOrderlist.Count > 0)
    //            {
    //                if (Convert.ToDecimal(lblBalance.Text) == 0)
    //                {
    //                    txtQty.Enabled = false;
    //                }
    //                else
    //                {
    //                    txtQty.Enabled = true;
    //                }
    //            }
    //            else
    //            {
    //                lblBalance.Text = lblIssuance.Text;
    //                lblPurchase.Text = "0";
    //                txtQty.Enabled = true;
    //            }

    //            //End Check Here If User Ordered Has Canceled 28-July-2011
    //            #endregion
    //        }

    //    }
    //    catch (Exception)
    //    { }
    //}
    //private void CheckPartialIssuanceGroupAsso()
    //{
    //    try
    //    {
    //        foreach (RepeaterItem item in rptGroupAssociation.Items)
    //        {
    //            Repeater rptChildGroupAssociation = (Repeater)item.FindControl("rptChildGroupAssociation");
    //            foreach (RepeaterItem item1 in rptChildGroupAssociation.Items)
    //            {
    //                Label lblGroupBalanceLeft = (Label)item.FindControl("lblGroupBalanceLeft");
    //                TextBox txtQty = (TextBox)item1.FindControl("txtQty");
    //                Label lblGroupIssuance = (Label)item.FindControl("lblGroupIssuance");
    //                Label lblPurchaseGroupBalance = (Label)item.FindControl("lblPurchaseGroupBalance");
    //                lblGroupIssuance.Text = objPolicyItemResult[0].Issuance.ToString();
    //                lblGroupBalanceLeft.Text = (int.Parse(lblGroupIssuance.Text) - Qty).ToString();
    //                objOrderlist = objOrderConfirmRepost.CheckUserIDEmpIDExit(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.EmployeeTypeID);
    //                if (objOrderlist.Count > 0)
    //                {
    //                    if (Convert.ToDecimal(lblGroupBalanceLeft.Text) == 0)
    //                    {
    //                        txtQty.Enabled = false;

    //                    }
    //                    else
    //                    {
    //                        txtQty.Enabled = true;

    //                    }
    //                }
    //                else
    //                {
    //                    lblGroupBalanceLeft.Text = lblGroupIssuance.Text;
    //                    lblPurchaseGroupBalance.Text = "0";
    //                    txtQty.Enabled = true;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception)
    //    {}
    //}
    #endregion
    #region Events
    protected void btnCheckOut_Click(object sender, EventArgs e)
    {
        try
        {

            CkeckPurchasedRequird();
            if (Session["SingleTotal"] != null)
                TotalAmount += decimal.Parse(Session["SingleTotal"].ToString());
            if (Session["GroupTotal"] != null)
                TotalAmount += decimal.Parse(Session["GroupTotal"].ToString());
            if (Session["GroupBudgetTotal"] != null)
                TotalAmount += decimal.Parse(Session["GroupBudgetTotal"].ToString());

            Session["TotalAmount"] = TotalAmount;
            Session["NotAnniversaryCreditAmount"] = null;
            objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID);
            if (objPolicyItemResult.Count > 0)
            {
                if (CompletePurchasedReqiured == true)
                {
                    dvNoRecord.Visible = false;
                    if (boolMessage == false)
                    {

                        return;
                    }
                    else if (boolCkeckInventoryNotallowed == false)
                    {
                        dvNoRecord.Visible = true;
                       lblNoRecord.Text= "Inventory Level Is Low And Allow Back Order is not Yes.";
                        return;
                    }
                    else
                    {
                        Response.Redirect("~/My Cart/CheckOutSteps.aspx?prog=ui");
                    }
                }
                else
                {
                    if (boolMessage == false)
                    {

                        return;
                    }
                    else if (boolCkeckInventoryNotallowed == false)
                    {
                        dvNoRecord.Visible = true;
                        lblNoRecord.Text = "Inventory Level Is Low And Allow Back Order is not Yes.";
                         return;
                    }
                    else
                    {
                        Response.Redirect("~/My Cart/CheckOutSteps.aspx?prog=ui");
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
    #endregion

    protected void rptSingleAssociation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        // Add Here Image in Griedview

        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                HiddenField hdnEmployeetype = (HiddenField)e.Item.FindControl("hdnEmployeetype");
                HiddenField hdnWeathertype = (HiddenField)e.Item.FindControl("hdnWeathertype");
                int intEmployeeType = 0;
                objCmpEmp = objCmpEmpRepo.GetByUserInfoId(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                if (objCmpEmp != null)
                {
                    intEmployeeType = Convert.ToInt32(objCmpEmp.EmployeeTypeID);
                }

                if (Convert.ToInt32(hdnEmployeetype.Value) != 0)
                {
                    if (hdnWeathertype.Value == "0" && (intEmployeeType != Convert.ToInt32(hdnEmployeetype.Value)))
                    {
                        e.Item.Visible = false;
                    }
                }



                HiddenField hdnLookupIcon = (HiddenField)e.Item.FindControl("hdnLookupIcon");
                HiddenField hdnProductImage = (HiddenField)e.Item.FindControl("hdnProductImage");
                HiddenField hdnlargerimagename = (HiddenField)e.Item.FindControl("hdnlargerimagename");
                Label lblPrice = (Label)e.Item.FindControl("lblPrice");


                IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Single Item Association");
                //UniformIssuancePolicyRepository objPolicyRepo = new UniformIssuancePolicyRepository();


                List<SelectUniformIssuanceImageResult> obj = new List<SelectUniformIssuanceImageResult>();

                obj = objPolicyRepo.GetIssuanceProductImage(Convert.ToInt16(IssuanceTypeID), Convert.ToInt64(((UniformIssuancePolicyItem)(e.Item.DataItem)).MasterItemId), Convert.ToInt32(PolicyID));
                if (obj.Count > 0)
                {


                    hdnLookupIcon.Value = Convert.ToString(obj[0].ProductImageID);
                    hdnlargerimagename.Value = obj[0].LargerProductImage;
                    hdnProductImage.Value = obj[0].ProductImage;

                    HtmlImage htnlImageSplash = ((HtmlImage)e.Item.FindControl("imgSplashImage"));
                    //htnlImageSplash.Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + hdnProductImage.Value + "&_twidth=145&_theight=198";
                    HtmlAnchor htnlAnchor = ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv"));
                    //htnlAnchor.HRef = "~/UploadedImages/ProductImages/" + hdnlargerimagename.Value;
                    htnlAnchor.HRef = "~/UploadedImages/ProductImages/" + hdnlargerimagename.Value;
                    htnlImageSplash.Src = "~/UploadedImages/ProductImages/Thumbs/" + hdnProductImage.Value;



                }

                objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
                if (objPolicy != null)
                {
                    objLook = objLookupRepo.GetById(Convert.ToInt64(objPolicy.PRICEFOR));
                    
                    if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName=="Both")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                            lblPrice.Visible = true;
                            tdSinglePrice.Visible = true;
                        }
                        else
                        {
                            lblPrice.Visible = false;
                            tdSinglePrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Employee")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                            lblPrice.Visible = true;
                            tdSinglePrice.Visible = true;
                        }
                        else
                        {
                            lblPrice.Visible = false;
                            tdSinglePrice.Visible = false;
                        } 
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Admin")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                            lblPrice.Visible = true;
                            tdSinglePrice.Visible = true;
                        }
                        else
                        {
                            lblPrice.Visible = false;
                            tdSinglePrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Admin")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                            lblPrice.Visible = true;
                            tdSinglePrice.Visible = true;
                        }
                        else
                        {
                            lblPrice.Visible = false;
                            tdSinglePrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Employee")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                            lblPrice.Visible = true;
                            tdSinglePrice.Visible = true;
                        }
                        else
                        {
                            lblPrice.Visible = false;
                            tdSinglePrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Both")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                            lblPrice.Visible = false;
                            tdSinglePrice.Visible = false;
                        }
                        else
                        {
                            lblPrice.Visible = true;
                            tdSinglePrice.Visible = true;
                        }
                    }
                  }


            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

    }
    protected void rptGroupAssociation_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
    protected void rptGroupAssociation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label tdGroupPrice = (Label)e.Item.FindControl("tdGroupPrice");
               
                objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
                if (objPolicy != null)
                {
                    objLook = objLookupRepo.GetById(Convert.ToInt64(objPolicy.PRICEFOR));

                    if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Both")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                           
                            tdGroupPrice.Visible = true;
                        }
                        else
                        {
                            
                            tdGroupPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Employee")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                          
                            tdGroupPrice.Visible = true;
                        }
                        else
                        {
                            
                            tdGroupPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Admin")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                            
                            tdGroupPrice.Visible = true;
                        }
                        else
                        {
                            
                            tdGroupPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Admin")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                           
                            tdGroupPrice.Visible = true;
                        }
                        else
                        {
                            
                            tdGroupPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Employee")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                          
                            tdGroupPrice.Visible = true;
                        }
                        else
                        {
                           
                            tdGroupPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Both")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                          
                            tdGroupPrice.Visible = false;
                        }
                        else
                        {
                          
                            tdGroupPrice.Visible = true;
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
    protected void rptChildGroupAssociation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        // Add Here Image in Griedview

        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                HiddenField hdnEmployeetype = (HiddenField)e.Item.FindControl("hdnEmployeetype");
                HiddenField hdnWeathertype = (HiddenField)e.Item.FindControl("hdnWeathertype");
                int intEmployeeType = 0;
                objCmpEmp = objCmpEmpRepo.GetByUserInfoId(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                if (objCmpEmp != null)
                {
                    intEmployeeType = Convert.ToInt32(objCmpEmp.EmployeeTypeID);
                }

                if (Convert.ToInt32(hdnEmployeetype.Value) != 0)
                {
                    if (hdnWeathertype.Value == "0" && (intEmployeeType != Convert.ToInt32(hdnEmployeetype.Value)))
                    {
                        e.Item.Visible = false;
                    }
                }



                HiddenField hdnLookupIcon = (HiddenField)e.Item.FindControl("hdnLookupIcon");
                HiddenField hdnProductImage = (HiddenField)e.Item.FindControl("hdnProductImage");
                HiddenField hdnlargerimagename = (HiddenField)e.Item.FindControl("hdnlargerimagename");
                Label lblPrice = (Label)e.Item.FindControl("lblGroupPrice");
                IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Group Association");
                //UniformIssuancePolicyRepository objPolicyRepo = new UniformIssuancePolicyRepository();

                List<SelectUniformIssuanceImageResult> obj = new List<SelectUniformIssuanceImageResult>();

                obj = objPolicyRepo.GetIssuanceProductImage(Convert.ToInt16(IssuanceTypeID), Convert.ToInt64(((UniformIssuancePolicyItem)(e.Item.DataItem)).MasterItemId), Convert.ToInt32(PolicyID));
                if (obj.Count > 0)
                {


                    hdnLookupIcon.Value = Convert.ToString(obj[0].ProductImageID);
                    hdnlargerimagename.Value = obj[0].LargerProductImage;
                    hdnProductImage.Value = obj[0].ProductImage;

                    HtmlImage htnlImageSplash = ((HtmlImage)e.Item.FindControl("imgSplashImage"));
                    //htnlImageSplash.Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + hdnProductImage.Value + "&_twidth=145&_theight=198";
                    HtmlAnchor htnlAnchor = ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv"));
                    //htnlAnchor.HRef = "~/UploadedImages/ProductImages/" + hdnlargerimagename.Value;
                    htnlAnchor.HRef = "~/UploadedImages/ProductImages/" + hdnlargerimagename.Value;
                    htnlImageSplash.Src = "~/UploadedImages/ProductImages/Thumbs/" + hdnProductImage.Value;



                }
               
                objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
                if (objPolicy != null)
                {
                    objLook = objLookupRepo.GetById(Convert.ToInt64(objPolicy.PRICEFOR));

                    if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Both")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                            lblPrice.Visible = true;
                           // tdGroupPrice.Visible = true;
                        }
                        else
                        {
                            lblPrice.Visible = false;
                          //  tdGroupPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Employee")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                            lblPrice.Visible = true;
                           // tdGroupPrice.Visible = true;
                        }
                        else
                        {
                            lblPrice.Visible = false;
                            //tdGroupPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Admin")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                            lblPrice.Visible = true;
                            //tdGroupPrice.Visible = true;
                        }
                        else
                        {
                            lblPrice.Visible = false;
                           // tdGroupPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Admin")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                            lblPrice.Visible = true;
                           // tdGroupPrice.Visible = true;
                        }
                        else
                        {
                            lblPrice.Visible = false;
                            //tdGroupPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Employee")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                            lblPrice.Visible = true;
                           // tdGroupPrice.Visible = true;
                        }
                        else
                        {
                            lblPrice.Visible = false;
                           // tdGroupPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Both")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                            lblPrice.Visible = false;
                           // tdGroupPrice.Visible = false;
                        }
                        else
                        {
                            lblPrice.Visible = true;
                            //tdGroupPrice.Visible = true;
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

    }
    protected void rptGroupBudgetAssociation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

       
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                        Label tdBudgetPrice = (Label)e.Item.FindControl("tdBudgetPrice");
                        
                       
                        objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
                        if (objPolicy != null)
                        {
                            objLook = objLookupRepo.GetById(Convert.ToInt64(objPolicy.PRICEFOR));

                            if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Both")
                            {
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                                {
                                   
                                    tdBudgetPrice.Visible = true;
                                }
                                else
                                {
                                   
                                    tdBudgetPrice.Visible = false;
                                }
                            }
                            else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Employee")
                            {
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                                {
                                   
                                    tdBudgetPrice.Visible = true;
                                }
                                else
                                {
                                  
                                    tdBudgetPrice.Visible = false;
                                }
                            }
                            else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Admin")
                            {
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                                {
                                   
                                    tdBudgetPrice.Visible = true;
                                }
                                else
                                {
                                    
                                    tdBudgetPrice.Visible = false;
                                }
                            }
                            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Admin")
                            {
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                                {
                                    
                                    tdBudgetPrice.Visible = true;
                                }
                                else
                                {
                                    
                                    tdBudgetPrice.Visible = false;
                                }
                            }
                            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Employee")
                            {
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                                {
                                    
                                    tdBudgetPrice.Visible = true;

                                }
                                else
                                {
                                   
                                    tdBudgetPrice.Visible = false;
                                }
                            }
                            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Both")
                            {
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                                {
                                   
                                    tdBudgetPrice.Visible = false;
                                }
                                else
                                {
                                   
                                    tdBudgetPrice.Visible = true;
                                }
                            }
                        }

                    }
              

    }
    protected void rptChildGroupBudgetAssociation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        //Start nagmani

        // Add Here Image in Griedview

        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnEmployeetype = (HiddenField)e.Item.FindControl("hdnEmployeetype");
                HiddenField hdnWeathertype = (HiddenField)e.Item.FindControl("hdnWeathertype");
                int intEmployeeType=0;
                 objCmpEmp = objCmpEmpRepo.GetByUserInfoId(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                 if (objCmpEmp != null)
                 {
                     intEmployeeType = Convert.ToInt32(objCmpEmp.EmployeeTypeID);
                 }

                 if (Convert.ToInt32(hdnEmployeetype.Value) != 0)
                {
                    if (hdnWeathertype.Value == "0" && (intEmployeeType != Convert.ToInt32(hdnEmployeetype.Value)))
                 {
                     e.Item.Visible = false;
                 }
                }


                            




                        HiddenField hdnLookupIcon = (HiddenField)e.Item.FindControl("hdnLookupIcon");
                        HiddenField hdnProductImage = (HiddenField)e.Item.FindControl("hdnProductImage");
                        HiddenField hdnlargerimagename = (HiddenField)e.Item.FindControl("hdnlargerimagename");
                        Label lblPrice = (Label)e.Item.FindControl("lblGroupBudgetPrice");
                        IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Group Association with Budget");
                        //UniformIssuancePolicyRepository objPolicyRepo = new UniformIssuancePolicyRepository();
                        List<SelectUniformIssuanceImageResult> obj = new List<SelectUniformIssuanceImageResult>();

                        obj = objPolicyRepo.GetIssuanceProductImage(Convert.ToInt16(IssuanceTypeID), Convert.ToInt64(((UniformIssuancePolicyItem)(e.Item.DataItem)).MasterItemId), Convert.ToInt32(PolicyID));
                        if (obj.Count > 0)
                        {

                            hdnLookupIcon.Value = Convert.ToString(obj[0].ProductImageID);
                            hdnlargerimagename.Value = obj[0].LargerProductImage;
                            hdnProductImage.Value = obj[0].ProductImage;
                            HtmlImage htnlImageSplash = ((HtmlImage)e.Item.FindControl("imgSplashImage"));
                            //htnlImageSplash.Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + hdnProductImage.Value + "&_twidth=145&_theight=198";
                            HtmlAnchor htnlAnchor = ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv"));
                            //htnlAnchor.HRef = "~/UploadedImages/ProductImages/" + hdnlargerimagename.Value;
                            htnlAnchor.HRef = "~/UploadedImages/ProductImages/" + hdnlargerimagename.Value;
                            htnlImageSplash.Src = "~/UploadedImages/ProductImages/Thumbs/" + hdnProductImage.Value;


                        }

                        objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
                        if (objPolicy != null)
                        {
                            objLook = objLookupRepo.GetById(Convert.ToInt64(objPolicy.PRICEFOR));

                            if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Both")
                            {
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                                {
                                    lblPrice.Visible = true;
                                   // tdBudgetPrice.Visible = true;
                                }
                                else
                                {
                                    lblPrice.Visible = false;
                                  //  tdBudgetPrice.Visible = false;
                                }
                            }
                            else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Employee")
                            {
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                                {
                                    lblPrice.Visible = true;
                                   // tdBudgetPrice.Visible = true;
                                }
                                else
                                {
                                    lblPrice.Visible = false;
                                    //tdBudgetPrice.Visible = false;
                                }
                            }
                            else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Admin")
                            {
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                                {
                                    lblPrice.Visible = true;
                                    //tdBudgetPrice.Visible = true;
                                }
                                else
                                {
                                    lblPrice.Visible = false;
                                   // tdBudgetPrice.Visible = false;
                                }
                            }
                            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Admin")
                            {
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                                {
                                    lblPrice.Visible = true;
                                  //  tdBudgetPrice.Visible = true;
                                }
                                else
                                {
                                    lblPrice.Visible = false;
                                   // tdBudgetPrice.Visible = false;
                                }
                            }
                            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Employee")
                            {
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                                {
                                    lblPrice.Visible = true;
                                   // tdBudgetPrice.Visible = true;

                                }
                                else
                                {
                                    lblPrice.Visible = false;
                                   // tdBudgetPrice.Visible = false;
                                }
                            }
                            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Both")
                            {
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                                {
                                    lblPrice.Visible = false;
                                    //tdBudgetPrice.Visible = false;
                                }
                                else
                                {
                                    lblPrice.Visible = true;
                                   // tdBudgetPrice.Visible = true;
                                }
                            }
                        }

                    }
               
            
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

        //End Nagmani

    }
    protected void rptSingleAssociation_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
    protected void ddlSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in rptSingleAssociation.Items)
        {
            HiddenField hdnStoreProductid=(HiddenField)item.FindControl("hdnStoreProductid");
            DropDownList ddlSizeList = (DropDownList)item.FindControl("ddlSize");
            Label lblPrice = (Label)item.FindControl("lblPrice");
            Label lblMasterItemNo = (Label)item.FindControl("lblItemNumber");
            long masterNo = Convert.ToInt64(objLookupRepo.GetIdByLookupName(lblMasterItemNo.Text));
            //Used method for Display Price
            List<ProductItemPricing> objPricing = new List<ProductItemPricing>();
            Panel pnlTailoring = (Panel)item.FindControl("pnlTailoring");
            objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
            if (objPolicy != null)
                priceleve = (objPolicy.PricingLevel).Trim();


            objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(masterNo, long.Parse(ddlSizeList.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));

            if (objPricing.Count > 0)
            {

                if (priceleve == "1")
                {
                    lblPrice.Text = Convert.ToString(objPricing[0].Level1);
                }
                else if (priceleve == "2")
                {
                    lblPrice.Text = Convert.ToString(objPricing[0].Level2);
                }
                else
                {
                    lblPrice.Text = Convert.ToString(objPricing[0].Level3);
                }

            }

            //Add Nagmani ItemNumber 4 july 2011
            Boolean boolTrue = false;
            HiddenField hdnItemNumber = (HiddenField)item.FindControl("hdnItemNumber");
            List<SELECTITEMNUMBERONSIZEMASTERNOResult> OBLISTITME = new List<SELECTITEMNUMBERONSIZEMASTERNOResult>();
            UniformIssuancePolicyItemRepository OBJREPOSTITEM = new UniformIssuancePolicyItemRepository();
            OBLISTITME = OBJREPOSTITEM.GETITEMNUMBER(Convert.ToInt32(masterNo), Convert.ToInt32(ddlSizeList.SelectedValue),Convert.ToInt32(hdnStoreProductid.Value));
            if (OBLISTITME.Count > 0)
            {
                for (int i = 0; i < OBLISTITME.Count; i++)
                {
                    if (boolTrue != true)
                    {
                        hdnItemNumber.Value = OBLISTITME[i].ITEMNUMBER.ToString();
                        boolTrue = true;
                    }
                }
            }

            //End Nagmani ItemNumber 4 july 2011

            //Added by Surendar Yadav 4 December 2012
            HyperLink lnkSizingChart = (HyperLink)item.FindControl("lnkSizingChart");
            TailoringMeasurementsRepository objRep = new TailoringMeasurementsRepository();
            List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
            //Added by Saurabh 15 December 2012
            if (objPolicyItemResult.Count > 0)
            objCount = objRep.GetTailoringMeasurementByStoreProductIDAndMasterItemID(Convert.ToInt64(objPolicyItemResult[item.ItemIndex].MasterItemId), Convert.ToInt64(objPolicyItemResult[item.ItemIndex].StoreProductid));
            if (objCount.Count > 0)
            {
                if (objCount[0].TailoringMeasurementChart != "")
                {
                    lnkSizingChart.Visible = true;
                    lnkSizingChart.NavigateUrl = "../UploadedImages/TailoringMeasurement/" + objCount[0].TailoringMeasurementChart;
                }
                else
                {
                    lnkSizingChart.Visible = false;
                    lnkSizingChart.NavigateUrl = "";
                }
            }
            else
            {
                lnkSizingChart.Visible = false;
                lnkSizingChart.NavigateUrl = "";
            }
            //End by Surendar Yadav 4 December 2012

            // CHECK TAILORING OPTION TRUE FALASE
            List<SelectTailoringOptionActiveInActiveResult> objTailoring;
            objTailoring = objPolicyItemRepo.GetTailoringActiveInActive(Convert.ToInt32(masterNo), int.Parse(ddlSizeList.SelectedValue),Convert.ToInt32(hdnStoreProductid.Value));

            if (objTailoring.Count > 0)
            {
                if (objTailoring[0].sLookupName == "Active")
                {
                    if (!string.IsNullOrEmpty(objTailoring[0].TailoringRunCharge))
                                        {
                    ((Label)item.FindControl("lblRunCharge")).Text = objTailoring[0].TailoringRunCharge.ToString();
                    }
                    pnlTailoring.Visible = true;
                }
                else
                {
                    pnlTailoring.Visible = false;
                }
            }

        }
    }
    protected void ddlSize1_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in rptGroupAssociation.Items)
        {
            Repeater rptChildGroupAssociation = (Repeater)item.FindControl("rptChildGroupAssociation");
        foreach (RepeaterItem item1 in rptChildGroupAssociation.Items)
        {
            HiddenField hdnStoreProductid=(HiddenField)item1.FindControl("hdnStoreProductid");
            DropDownList ddlSizeList = (DropDownList)item1.FindControl("ddlSize");
            Label lblPrice = (Label)item1.FindControl("lblGroupPrice");
            Label lblMasterItemNo = (Label)item1.FindControl("lblItemNumber");
            long masterNo = Convert.ToInt64(objLookupRepo.GetIdByLookupName(lblMasterItemNo.Text));
            Panel pnlTailoring = (Panel)item1.FindControl("pnlTailoringGroup");
            //Used method for Display Price
            List<ProductItemPricing> objPricing = new List<ProductItemPricing>();
            objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
            if (objPolicy != null)
                priceleve = (objPolicy.PricingLevel).Trim();
            objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(masterNo, long.Parse(ddlSizeList.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));

            if (objPricing.Count > 0)
            {

                if (priceleve == "1")
                {
                    lblPrice.Text = Convert.ToString(objPricing[0].Level1);
                }
                else if (priceleve == "2")
                {
                    lblPrice.Text = Convert.ToString(objPricing[0].Level2);
                }
                else
                {
                    lblPrice.Text = Convert.ToString(objPricing[0].Level3);
                }

            }

            //Add Nagmani ItemNumber 4 july 2011
            Boolean boolTrue = false;
            HiddenField hdnItemNumber = (HiddenField)item1.FindControl("hdnGroupItemNumber");
            List<SELECTITEMNUMBERONSIZEMASTERNOResult> OBLISTITME = new List<SELECTITEMNUMBERONSIZEMASTERNOResult>();
            UniformIssuancePolicyItemRepository OBJREPOSTITEM = new UniformIssuancePolicyItemRepository();
            OBLISTITME = OBJREPOSTITEM.GETITEMNUMBER(Convert.ToInt32(masterNo), Convert.ToInt32(ddlSizeList.SelectedValue),Convert.ToInt32(hdnStoreProductid.Value));
            if (OBLISTITME.Count > 0)
            {
                for (int i = 0; i < OBLISTITME.Count; i++)
                {
                    if (boolTrue != true)
                    {
                        hdnItemNumber.Value = OBLISTITME[i].ITEMNUMBER.ToString();
                        boolTrue = true;
                    }
                }
            }




            //End Nagmani ItemNumber 4 july 2011


            // CHECK TAILORING OPTION TRUE FALASE
            List<SelectTailoringOptionActiveInActiveResult> objTailoring;
            objTailoring = objPolicyItemRepo.GetTailoringActiveInActive(Convert.ToInt32(masterNo), int.Parse(ddlSizeList.SelectedValue),Convert.ToInt32(hdnStoreProductid.Value));

            if (objTailoring.Count > 0)
            {
                if (objTailoring[0].sLookupName == "Active")
                {
                    pnlTailoring.Visible = true;
                }
                else
                {
                    pnlTailoring.Visible = false;
                }
            }
        }
    }
    }
    protected void ddlSize2_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in rptGroupBudgetAssociation.Items)
        {
            Repeater rptChildGroupBudgetAssociation = (Repeater)item.FindControl("rptChildGroupBudgetAssociation");
            foreach (RepeaterItem item1 in rptChildGroupBudgetAssociation.Items)
            {
                DropDownList ddlSizeList = (DropDownList)item1.FindControl("ddlSize");
                Label lblPrice = (Label)item1.FindControl("lblGroupBudgetPrice");
                Label lblMasterItemNo = (Label)item1.FindControl("lblItemNumber");
                long masterNo = Convert.ToInt64(objLookupRepo.GetIdByLookupName(lblMasterItemNo.Text));
                Panel pnlTailoring = (Panel)item1.FindControl("pnlTailoringGroupBudget");
                HiddenField hdnStoreProductid = (HiddenField)item1.FindControl("hdnStoreProductid");
                //Used method for Display Price
                objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
                if (objPolicy != null)
                    priceleve = (objPolicy.PricingLevel).Trim();
                List<ProductItemPricing> objPricing = new List<ProductItemPricing>();

                objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(masterNo, long.Parse(ddlSizeList.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));
                if (objPricing.Count > 0)
                {

                    if (priceleve == "1")
                    {
                        lblPrice.Text = Convert.ToString(objPricing[0].Level1);
                    }
                    else if (priceleve == "2")
                    {
                        lblPrice.Text = Convert.ToString(objPricing[0].Level2);
                    }
                    else
                    {
                        lblPrice.Text = Convert.ToString(objPricing[0].Level3);
                    }

                }
                //Add Nagmani ItemNumber 4 july 2011
                Boolean boolTrue = false;
                HiddenField hdnItemNumber = (HiddenField)item1.FindControl("hdnBudgetItemNumber");
                List<SELECTITEMNUMBERONSIZEMASTERNOResult> OBLISTITME = new List<SELECTITEMNUMBERONSIZEMASTERNOResult>();
                UniformIssuancePolicyItemRepository OBJREPOSTITEM = new UniformIssuancePolicyItemRepository();
                OBLISTITME = OBJREPOSTITEM.GETITEMNUMBER(Convert.ToInt32(masterNo), Convert.ToInt32(ddlSizeList.SelectedValue),Convert.ToInt32(hdnStoreProductid.Value));
                if (OBLISTITME.Count > 0)
                {
                    for (int i = 0; i < OBLISTITME.Count; i++)
                    {
                        if (boolTrue != true)
                        {
                            hdnItemNumber.Value = OBLISTITME[i].ITEMNUMBER.ToString();
                            boolTrue = true;
                        }
                    }
                }

                //End Nagmani ItemNumber 4 july 2011 if(OBLISTITME1.Count>0)
                                
                

                // CHECK TAILORING OPTION TRUE FALASE
                List<SelectTailoringOptionActiveInActiveResult> objTailoring;
                objTailoring = objPolicyItemRepo.GetTailoringActiveInActive(Convert.ToInt32(masterNo), int.Parse(ddlSizeList.SelectedValue),Convert.ToInt32(hdnStoreProductid.Value));

                if (objTailoring.Count > 0)
                {
                    if (objTailoring[0].sLookupName == "Active")
                    {
                        pnlTailoring.Visible = true;
                    }
                    else
                    {
                        pnlTailoring.Visible = false;
                    }
                }
            }
        }
    }
    public void CkeckPurchasedRequird()
    {
        string strCheckSinglePurchased = null;
        string strCheckGroupPurchased = null;
        string strGroupBudgetPurchased = null;
        try
        {
            //Check Here if Purchase complete required is True
            objPolicy = objPolicyRepo.GetById(Convert.ToInt64(Request.QueryString["PID"].ToString()));
            Purchaserequired = Convert.ToString(objPolicy.CompletePurchase);
            if (Purchaserequired == "Y")
            {
                StringBuilder sbSingleCartIDs = new StringBuilder();
                sbSingleCartIDs.Append(string.Empty);

                if (dvSingle.Visible == true && pnlSingle.Visible == true)
                {
                    IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Single Item Association");
                    objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(i => i.AssociationIssuanceType == IssuanceTypeID).ToList();
                    Label lblBalance = new Label();
                    if (objPolicyItemResult.Count > 0)
                    {
                        int totalqty = 0;
                        int totalbalance = 0;
                        foreach (RepeaterItem item in rptSingleAssociation.Items)
                        {
                            Label lblIssuance = new Label();
                            lblIssuance = (Label)item.FindControl("lblIssuance");


                            lblBalance = (Label)item.FindControl("lblBalance");

                            TextBox txtQty = new TextBox();
                            txtQty = (TextBox)item.FindControl("txtQty");

                            DropDownList ddlSize = new DropDownList();
                            ddlSize = (DropDownList)item.FindControl("ddlSize");


                            if (txtQty.Text != string.Empty && txtQty.Text != "0")
                            {
                                if (int.Parse(txtQty.Text) > 0)
                                    totalqty += int.Parse(txtQty.Text);
                            }
                            totalbalance += int.Parse(lblBalance.Text);

                        }
                        //Single Check
                        if (totalqty == totalbalance)
                        {
                            strCheckSinglePurchased = "1";
                        }

                    }
                }
                //Group Check
                StringBuilder sbGroupCartIDs = new StringBuilder();
                sbGroupCartIDs.Append(string.Empty);
                if (dvGroup.Visible == true && pnlGroup.Visible == true)
                {

                    int totalqty = 0;

                    IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Group Association");
                    foreach (RepeaterItem item in rptGroupAssociation.Items)
                    {
                        Repeater rptChildGroupAssociation = (Repeater)item.FindControl("rptChildGroupAssociation");
                        HiddenField hdnNEWGROUP = (HiddenField)item.FindControl("hdnNEWGROUP");
                        Label lblGroupBalanceLeft = (Label)item.FindControl("lblGroupBalanceLeft");
                   
                   // objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(i => i.AssociationIssuanceType == IssuanceTypeID).ToList();
                        objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(i => i.AssociationIssuanceType == IssuanceTypeID).ToList();

                    if (objPolicyItemResult.Count > 0)
                    {
                        foreach (RepeaterItem item1 in rptChildGroupAssociation.Items)
                        {
                            TextBox txtQty = new TextBox();
                            txtQty = (TextBox)item1.FindControl("txtQty");

                            if (txtQty.Text != string.Empty && txtQty.Text != "0")
                            {
                                if (int.Parse(txtQty.Text) > 0)
                                    totalqty += int.Parse(txtQty.Text);
                            }

                        }
                        if (totalqty == int.Parse(lblGroupBalanceLeft.Text))
                        {
                            strCheckGroupPurchased = "1";
                            totalqty = 0;
                        }
                        else
                        {
                            strCheckGroupPurchased = "0";
                            break;
                        }


                    }
                }
                }
                //End Group Check     

                //Start GroupBudget Check


                StringBuilder sbGroupBudgetCartIDs = new StringBuilder();
                sbGroupBudgetCartIDs.Append(string.Empty);

                if (dvGroupBudget.Visible == true && pnlGroupBudget.Visible == true)
                {
                    decimal? rate = 0;
                    int totalqty = 0;
                    decimal? amount = 0;

                    IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Group Association with Budget");
                    foreach (RepeaterItem item in rptGroupBudgetAssociation.Items)
                    {
                        Repeater rptChildGroupBudgetAssociation = (Repeater)item.FindControl("rptChildGroupBudgetAssociation");
                        HiddenField hdnNEWGROUP = (HiddenField)item.FindControl("hdnNEWGROUP");
                        Label lblGroupBudgetAmountLeft = (Label)item.FindControl("lblGroupBudgetAmountLeft");
                    //objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(i => i.AssociationIssuanceType == IssuanceTypeID).ToList();
                        objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyIDAndNewGroup(this.PolicyID, hdnNEWGROUP.Value).Where(i => i.AssociationIssuanceType == IssuanceTypeID).ToList();

                    if (objPolicyItemResult.Count > 0)
                    {
                        foreach (RepeaterItem item1 in rptChildGroupBudgetAssociation.Items)
                        {
                            HiddenField hdnStoreProductid=(HiddenField)item1.FindControl("hdnStoreProductid");
                            DropDownList ddlSize = new DropDownList();
                            ddlSize = (DropDownList)item1.FindControl("ddlSize");
                            Label lblPrice = (Label)item1.FindControl("lblGroupBudgetPrice");
                            TextBox txtQty = new TextBox();
                            txtQty = (TextBox)item1.FindControl("txtQty");

                            if (txtQty.Text != string.Empty && txtQty.Text != "0")
                            {
                                if (int.Parse(txtQty.Text) > 0)
                                    totalqty += int.Parse(txtQty.Text);
                            }

                            rate = objMyIssuanceCartRepo.GetPriceByMasterItemIdandItemSizeId(Convert.ToInt64(objPolicyItemResult[item1.ItemIndex].MasterItemId), long.Parse(ddlSize.SelectedValue),Convert.ToInt64(hdnStoreProductid.Value));                            
                            if (lblPrice.Text != "")
                            {
                                if (txtQty.Text != string.Empty && txtQty.Text != "0")
                                    amount += int.Parse(txtQty.Text) * decimal.Parse(lblPrice.Text);
                            }

                        }
                        if (amount == decimal.Parse(lblGroupBudgetAmountLeft.Text))
                        {
                            strGroupBudgetPurchased = "1";
                            amount = 0;
                        }
                        else
                        {
                            strGroupBudgetPurchased = "0";
                            break;
                        }

                    }
                }
                }

                //End Group Budget
                if ((dvGroupBudget.Visible == true && pnlGroupBudget.Visible == true) && (dvGroup.Visible == true && pnlGroup.Visible == true) && (dvSingle.Visible == true && pnlSingle.Visible == true))
                {
                    if (strGroupBudgetPurchased == "1" && strCheckGroupPurchased == "1" && strCheckSinglePurchased == "1")
                    {
                       
                            ProcessSingleAssociation();
                            ProcessGroupAssociation();
                            ProcessGroupBudgetAssociation();
                            dvNoRecord.Visible = false;
                            lblNoRecord.Text = "";
                    }
                    else
                    {
                        boolCkeckPurchasedRequird = false;
                        boolMessage = false;
                        dvNoRecord.Visible = true;
                        lblNoRecord.Text = "Please Enter Complete Purchased Required Data";
                        return;
                    }
                }
                else if ((dvGroup.Visible == true && pnlGroup.Visible == true) && (dvSingle.Visible == true && pnlSingle.Visible == true))
                {
                    if (strCheckGroupPurchased == "1" && strCheckSinglePurchased == "1")
                    {
                       
                            ProcessSingleAssociation();
                            ProcessGroupAssociation();
                            dvNoRecord.Visible = false;
                            lblNoRecord.Text = "";
                       
                    }
                    else
                    {
                        boolCkeckPurchasedRequird = false;
                        boolMessage = false;
                        dvNoRecord.Visible = true;
                        lblNoRecord.Text = "Please Enter Complete Purchased Required Data";
                        return;
                    }
                }
                else if ((dvGroupBudget.Visible == true && pnlGroupBudget.Visible == true) && (dvGroup.Visible == true && pnlGroup.Visible == true))
                {
                    if (strGroupBudgetPurchased == "1" && strCheckGroupPurchased == "1")
                    {

                        ProcessGroupAssociation();
                        ProcessGroupBudgetAssociation();
                        dvNoRecord.Visible = false;
                        lblNoRecord.Text = "";
                    }
                    else
                    {
                        boolCkeckPurchasedRequird = false;
                        boolMessage = false;
                        dvNoRecord.Visible = true;
                        lblNoRecord.Text = "Please Enter Complete Purchased Required Data";
                        return;
                    }
                }
                else if ((dvGroupBudget.Visible == true && pnlGroupBudget.Visible == true) && (dvSingle.Visible == true && pnlSingle.Visible == true))
                {
                    if (strGroupBudgetPurchased == "1" && strCheckSinglePurchased == "1")
                    {
                        ProcessSingleAssociation();
                        ProcessGroupBudgetAssociation();
                        dvNoRecord.Visible = false;
                        lblNoRecord.Text = "";
                    }
                    else
                    {
                        boolCkeckPurchasedRequird = false;
                        boolMessage = false;
                        dvNoRecord.Visible = true;
                        lblNoRecord.Text = "Please Enter Complete Purchased Required Data";
                        return;
                    }
                }
                else if (dvGroupBudget.Visible == true && pnlGroupBudget.Visible == true)
                {
                    if (strGroupBudgetPurchased == "1")
                    {
                        ProcessGroupBudgetAssociation();
                        dvNoRecord.Visible = false;
                        lblNoRecord.Text = "";
                    }
                    else
                    {
                        boolCkeckPurchasedRequird = false;
                        boolMessage = false;
                        dvNoRecord.Visible = true;
                        lblNoRecord.Text = "Please Enter Complete Purchased Required Data";
                        return;
                    }
                }
                else if (dvGroup.Visible == true && pnlGroup.Visible == true)
                {
                    if (strCheckGroupPurchased == "1")
                    {
                        ProcessGroupAssociation();
                        dvNoRecord.Visible = false;
                        lblNoRecord.Text = "";
                    }
                    else
                    {
                        boolCkeckPurchasedRequird = false;
                        boolMessage = false;
                        dvNoRecord.Visible = true;
                        lblNoRecord.Text = "Please Enter Complete Purchased Required Data";
                        return;
                    }
                }
                else if (dvSingle.Visible == true && pnlSingle.Visible == true)
                {
                    if (strCheckSinglePurchased == "1")
                    {
                        ProcessSingleAssociation();
                        dvNoRecord.Visible = false;
                        lblNoRecord.Text = "";
                    }
                    else
                    {
                        boolCkeckPurchasedRequird = false;
                        boolMessage = false;
                        dvNoRecord.Visible = true;
                        lblNoRecord.Text = "Please Enter Complete Purchased Required Data";
                        return;
                    }
                }
            } // End Purchase Required
            else // Not Purchaed Required
            {
                ProcessSingleAssociation();
                ProcessGroupAssociation();
                ProcessGroupBudgetAssociation();

            }

        }
        catch (Exception ex)
        {

            ex.Message.ToString();
        }
    }

}
