using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;

public partial class admin_CompanyStore_InternationalShipping : PageBase
{
    #region Data Members
    InternationalShippingAddressRepository objInternationalShippingAddressRepository = new InternationalShippingAddressRepository();
    public Int64 StoreID
    {
        get
        {
            if (ViewState["StoreID"] == null)
            {
                ViewState["StoreID"] = 0;
            }
            return Convert.ToInt64(ViewState["StoreID"]);
        }
        set
        {
            ViewState["StoreID"] = value;
        }
    }
    public Int64 WorkGroupID
    {
        get
        {
            if (ViewState["WorkGroupID"] == null)
            {
                ViewState["WorkGroupID"] = 0;
            }
            return Convert.ToInt64(ViewState["WorkGroupID"]);
        }
        set
        {
            ViewState["WorkGroupID"] = value;
        }
    }
    public Int64 InternationalShippingID
    {
        get
        {
            if (ViewState["InternationalShippingID"] == null)
            {
                ViewState["InternationalShippingID"] = 0;
            }
            return Convert.ToInt64(ViewState["InternationalShippingID"]);
        }
        set
        {
            ViewState["InternationalShippingID"] = value;
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {            
            if (Request.QueryString["Id"] != null && Request.QueryString["SubId"]!=null)
            {
               
                this.StoreID= Convert.ToInt64(Request.QueryString["Id"]);
                this.WorkGroupID = Convert.ToInt64(Request.QueryString["SubId"]);

                ((Label)Master.FindControl("lblPageHeading")).Text = "International Shipping By Store Workgroup";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/ViewGlobalMenuSetting.aspx?ID=" + this.StoreID;
                lblWorkGroup.Text = new LookupRepository().GetById(this.WorkGroupID).sLookupName.ToString();
                Session["ManageID"] = 5;
                menuControl.PopulateMenu(11, 3, this.StoreID, this.WorkGroupID, true);
                BindCountry();
                BindDropDowns();
                DisplayData();
            }   
        }
    }

    protected void DrpShipingState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepository = new CityRepository();
        List<INC_City> objCityList = objCityRepository.GetByStateId(Convert.ToInt64(ddlState.SelectedValue));
        ddlCity.Items.Clear();
        Common.BindDDL(ddlCity, objCityList, "sCityName", "iCityID", "-select city-");
    }

    protected void lnkBtnSave_Click(object sender, EventArgs e)
    {
        InternationalShippingAddress objInternationalShippingAddress = new InternationalShippingAddress();

        try
        {
            if (this.InternationalShippingID != 0)
            {
                objInternationalShippingAddress = objInternationalShippingAddressRepository.GetByID(this.InternationalShippingID);
            }
            if (chkShoppingCart.Checked)
            {
                spnShoppingCart.Attributes.Add("class", "custom-checkbox_checked alignleft");
                objInternationalShippingAddress.IsForShoppingCart = true;
            }
            else
            {
                spnShoppingCart.Attributes.Add("class", "custom-checkbox alignleft");
                objInternationalShippingAddress.IsForShoppingCart = false;
            }
            if (chkIssuancePolicy.Checked)
            {
                spnIssuancePolicy.Attributes.Add("class", "custom-checkbox_checked alignleft");
                objInternationalShippingAddress.IsForIssuancePolicy = true;
            }
            else
            {
                spnIssuancePolicy.Attributes.Add("class", "custom-checkbox alignleft");
                objInternationalShippingAddress.IsForIssuancePolicy = false;
            }
            objInternationalShippingAddress.StoreID = this.StoreID;
            objInternationalShippingAddress.WorkgroupID = this.WorkGroupID;
            objInternationalShippingAddress.FirstName = TxtFirstName.Text.Trim();
            objInternationalShippingAddress.LastName = TxtLastName.Text.Trim();
            objInternationalShippingAddress.CompanyName = TxtCompany.Text.Trim();
            if (!string.IsNullOrEmpty(ddlDepartment.SelectedValue) && ddlDepartment.SelectedValue != "0")
                objInternationalShippingAddress.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
            else
                objInternationalShippingAddress.DepartmentID = null;
            objInternationalShippingAddress.Airport = txtAirport.Text.Trim();
            objInternationalShippingAddress.Address = TxtAddress.InnerText.Trim();
            objInternationalShippingAddress.CountryID = Common.GetLongNull(ddlCoutry.SelectedValue, true);
            objInternationalShippingAddress.StateID = Common.GetLongNull(ddlState.SelectedValue, true);
            objInternationalShippingAddress.CityID = Common.GetLongNull(ddlCity.SelectedItem.Value, true);
            objInternationalShippingAddress.County = txtCounty.Text.Trim();
            objInternationalShippingAddress.ZipCode = TxtZip.Text.Trim();
            objInternationalShippingAddress.Telephone = TxtTelephone.Text.Trim();

            objInternationalShippingAddress.Mobile = TxtMobile.Text.Trim();
            objInternationalShippingAddress.Email = TxtEmail.Text.Trim();

            if (this.InternationalShippingID == 0)
                objInternationalShippingAddressRepository.Insert(objInternationalShippingAddress);

            objInternationalShippingAddressRepository.SubmitChanges();
            this.InternationalShippingID = objInternationalShippingAddress.InternationalShippingID;
            lblMsg.Text = "Address Saved Successfully ...";

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            lblMsg.Text = "Error in saving shipping address ...";
        }
    }
    #endregion

    #region Methods
    //bind dropdown for the workgroup, department
    public void BindDropDowns()
    {
        CompanyStoreRepository objCompanyStoreRepository = new Incentex.DAL.SqlRepository.CompanyStoreRepository();
        List<GetStoreDepartmentsResult> lstDepartments = objCompanyStoreRepository.GetStoreDepartments(this.StoreID).Where(le => le.Existing == 1).OrderBy(le => le.Department).ToList();
        Common.BindDDL(ddlDepartment, lstDepartments, "Department", "DepartmentID", "-select department-");
      
    }
    protected void BindCountry()
    {
        CountryRepository objCountryRepository = new CountryRepository();
        List<INC_Country> objINC_CountryList = objCountryRepository.GetAll();

        ddlCoutry.DataSource = objINC_CountryList;
        ddlCoutry.DataValueField = "iCountryID";
        ddlCoutry.DataTextField = "sCountryName";
        ddlCoutry.DataBind();

        ddlCoutry.SelectedValue = ddlCoutry.Items.FindByText("United States").Value;
        ddlCoutry.Enabled = false;

        StateRepository objStateRepository = new StateRepository();
        List<INC_State> objINC_StateList = objStateRepository.GetByCountryId(Convert.ToInt64(ddlCoutry.SelectedValue));

        Common.BindDDL(ddlState, objINC_StateList, "sStatename", "iStateID", "-select state-");
        ddlCity.Items.Insert(0, new ListItem("-select city-", "0"));
    }
    protected void DisplayData()
    {
        InternationalShippingAddress objInternationalShippingAddress = objInternationalShippingAddressRepository.GetByStoreIDAndWorkgroupID(this.StoreID, this.WorkGroupID);
        if (objInternationalShippingAddress != null)
        {
            this.InternationalShippingID = objInternationalShippingAddress.InternationalShippingID;
            if (objInternationalShippingAddress.IsForShoppingCart)
            {
                spnShoppingCart.Attributes.Add("class", "custom-checkbox_checked alignleft");
                chkShoppingCart.Checked = true;
            }
            else
            {
                spnShoppingCart.Attributes.Add("class", "custom-checkbox alignleft");
                chkShoppingCart.Checked = false;
            }
            if (objInternationalShippingAddress.IsForIssuancePolicy)
            {
                spnIssuancePolicy.Attributes.Add("class", "custom-checkbox_checked alignleft");
                chkIssuancePolicy.Checked = true;
            }
            else
            {
                spnIssuancePolicy.Attributes.Add("class", "custom-checkbox alignleft");
                chkIssuancePolicy.Checked = false;
            }
            TxtFirstName.Text = objInternationalShippingAddress.FirstName;
            TxtLastName.Text = objInternationalShippingAddress.LastName;
            TxtCompany.Text = objInternationalShippingAddress.CompanyName;
            ddlDepartment.SelectedValue = objInternationalShippingAddress.DepartmentID!=null?objInternationalShippingAddress.DepartmentID.ToString():"0";
            txtAirport.Text = objInternationalShippingAddress.Airport;
            TxtAddress.InnerText = objInternationalShippingAddress.Address;
            ddlCoutry.SelectedValue = objInternationalShippingAddress.CountryID != null ? objInternationalShippingAddress.CountryID.ToString() : ddlCoutry.Items.FindByText("United States").Value;
            ddlState.SelectedValue = objInternationalShippingAddress.StateID != null ? objInternationalShippingAddress.StateID.ToString() : "0";
            DrpShipingState_SelectedIndexChanged(null,null);
            ddlCity.SelectedValue = objInternationalShippingAddress.CityID != null ? objInternationalShippingAddress.CityID.ToString() : "0";
            txtCounty.Text = objInternationalShippingAddress.County;
            TxtZip.Text = objInternationalShippingAddress.ZipCode;
            TxtTelephone.Text = objInternationalShippingAddress.Telephone;
            TxtMobile.Text = objInternationalShippingAddress.Mobile;
            TxtEmail.Text = objInternationalShippingAddress.Email;
        }
    }
    #endregion
}
