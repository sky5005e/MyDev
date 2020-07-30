using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyStore_BillingAddressSetting : PageBase
{
    #region Page Properties

    Int64 StoreID
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

    Int64 WorkGroupID
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

    Int64 BillingAddressID
    {
        get
        {
            if (ViewState["BillingAddressID"] == null)
            {
                ViewState["BillingAddressID"] = 0;
            }
            return Convert.ToInt64(ViewState["BillingAddressID"]);
        }
        set
        {
            ViewState["BillingAddressID"] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(Object sender, EventArgs e)
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
                this.StoreID = Convert.ToInt64(Request.QueryString.Get("Id"));

                if (this.StoreID == 0)
                {
                    Response.Redirect("~/admin/CompanyStore/GeneralStoreSetup.aspx?Id=" + this.StoreID);
                }

                if (this.StoreID > 0 && !base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                this.WorkGroupID = Convert.ToInt64(Request.QueryString.Get("SubId"));

                ((Label)Master.FindControl("lblPageHeading")).Text = "Storefront Settings by store workgroup";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/ViewGlobalMenuSetting.aspx?ID=" + this.StoreID;

                Session["ManageID"] = 5;
                menuControl.PopulateMenu(11, 4, this.StoreID, this.WorkGroupID, true);

                BindDDL();
                lblStore.Text = new CompanyRepository().GetById(new CompanyStoreRepository().GetById(StoreID).CompanyID).CompanyName;
                lblWorkGroup.Text = new LookupRepository().GetById(this.WorkGroupID).sLookupName.ToString();

                GlobalBillingAddress objAddress = new GlobalBillingAddressRepository().GetByStoreAndWorkGroupID(this.StoreID, this.WorkGroupID);
                this.BillingAddressID = objAddress != null ? objAddress.BillingAddressID : 0;
                DisplayData();
            }
        }
    }

    #endregion

    #region Control Events

    protected void DrpBillingCountry_SelectedIndexChanged(Object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(DrpBillingCountry.SelectedValue));

        DrpBillingState.Items.Clear();
        DrpBillingCity.Items.Clear();
        DrpBillingCity.Items.Insert(0, new ListItem("-select city-", "0"));
        Common.BindDDL(DrpBillingState, objStateList, "sStatename", "iStateID", "-select state-");
    }

    protected void DrpBillingState_SelectedIndexChanged(Object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(DrpBillingState.SelectedValue));
        DrpBillingCity.Items.Clear();
        Common.BindDDL(DrpBillingCity, objCityList, "sCityName", "iCityID", "-select city-");
    }

    protected void lnkSave_Click(Object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(DrpBillingCountry.SelectedValue))
            {
                lblMsg.Text = "Please select country.";
                return;
            }

            if (String.IsNullOrEmpty(DrpBillingState.SelectedValue))
            {
                lblMsg.Text = "Please select state.";
                return;
            }

            if (String.IsNullOrEmpty(DrpBillingCity.SelectedValue))
            {
                lblMsg.Text = "Please select city.";
                return;
            }

            if (String.IsNullOrEmpty(txtZip.Text.Trim()))
            {
                lblMsg.Text = "Please enter zip code.";
                return;
            }
            else if (!Common.IsValidZipCode(DrpBillingCountry.SelectedItem.Text.Trim(), txtZip.Text.Trim()))
            {
                lblMsg.Text = "Please enter valid zip code.";
                return;
            }

            GlobalBillingAddressRepository objRepo = new GlobalBillingAddressRepository();
            GlobalBillingAddress objAddress;

            if (this.BillingAddressID == 0)
            {
                objAddress = new GlobalBillingAddress();
                objAddress.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objAddress.CreatedDate = DateTime.Now;
                objAddress.StoreID = this.StoreID;
                objAddress.WorkGroupID = this.WorkGroupID;
            }
            else
            {
                objAddress = objRepo.GetByID(this.BillingAddressID);
            }

            objAddress.AddressLine1 = txtAddressLine1.Text.Trim();
            objAddress.AddressLine2 = txtAddressLine2.Text.Trim();
            objAddress.CityID = Convert.ToInt64(DrpBillingCity.SelectedValue);
            objAddress.CompanyName = txtCompany.Text.Trim();
            objAddress.CountryID = Convert.ToInt64(DrpBillingCountry.SelectedValue);            
            objAddress.Email = txtEmail.Text.Trim();
            objAddress.FirstName = txtFirstName.Text.Trim();
            objAddress.LastName = txtLastName.Text.Trim();
            objAddress.Mobile = txtMobile.Text.Trim();
            objAddress.StateID = Convert.ToInt64(DrpBillingState.SelectedValue);
            objAddress.Telephone = txtTelephone.Text.Trim();
            objAddress.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objAddress.UpdatedDate = DateTime.Now;
            objAddress.ZipCode = txtZip.Text.Trim();

            if (this.BillingAddressID == 0)
                objRepo.Insert(objAddress);

            objRepo.SubmitChanges();

            lblMsg.Text = "Record saved successfully...";

            DisplayData();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Some error occured while saving the record...";
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Methods

    void BindDDL()
    {
        CountryRepository objCountryBillingRepo = new CountryRepository();
        List<INC_Country> objBillingCountryList = objCountryBillingRepo.GetAll();
        Common.BindDDL(DrpBillingCountry, objBillingCountryList, "sCountryName", "iCountryID", "-select country-");
        DrpBillingCountry.SelectedValue = DrpBillingCountry.Items.FindByText("United States").Value;
        DrpBillingCountry_SelectedIndexChanged(null, null);
    }

    private void DisplayData()
    {
        try
        {
            if (this.BillingAddressID > 0)
            {
                GlobalBillingAddress objAddress = new GlobalBillingAddressRepository().GetByID(this.BillingAddressID);

                DrpBillingCountry.SelectedValue = objAddress.CountryID != null ? Convert.ToString(objAddress.CountryID) : "0";
                DrpBillingCountry_SelectedIndexChanged(null, null);

                if (objAddress.CountryID != null)
                {
                    DrpBillingState.SelectedValue = objAddress.StateID != null ? Convert.ToString(objAddress.StateID) : "0";
                    DrpBillingState_SelectedIndexChanged(null, null);

                    if (objAddress.StateID != null)
                    {
                        DrpBillingCity.SelectedValue = objAddress.CityID != null ? Convert.ToString(objAddress.CityID) : "0";
                    }
                }

                txtAddressLine1.Text = Convert.ToString(objAddress.AddressLine1);
                txtAddressLine2.Text = Convert.ToString(objAddress.AddressLine2);
                txtCompany.Text = Convert.ToString(objAddress.CompanyName);
                txtEmail.Text = Convert.ToString(objAddress.Email);
                txtFirstName.Text = Convert.ToString(objAddress.FirstName);
                txtLastName.Text = Convert.ToString(objAddress.LastName);
                txtMobile.Text = Convert.ToString(objAddress.Mobile);
                txtTelephone.Text = Convert.ToString(objAddress.Telephone);
                txtZip.Text = Convert.ToString(objAddress.ZipCode);               
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }        
    }

    #endregion
}