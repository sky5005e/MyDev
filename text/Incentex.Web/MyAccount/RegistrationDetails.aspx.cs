using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_RegistrationDetails : PageBase
{
    #region Data Members

    LookupRepository objLookupRepos = new LookupRepository();

    Int64 RegistrationID
    {
        get
        {
            return Convert.ToInt64(ViewState["RegistrationID"]);
        }
        set
        {
            ViewState["RegistrationID"] = value;
        }
    }

    #endregion

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            if (Request.QueryString.Count > 0 && Convert.ToInt64(Request.QueryString.Get("ID")) > 0)
            {
                this.RegistrationID = Convert.ToInt64(Request.QueryString.Get("ID"));

                ((Label)Master.FindControl("lblPageHeading")).Text = "Registration Details";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/ViewPendingUsers.aspx";
            }
            else
            {
                Response.Redirect("~/MyAccount/ViewPendingUsers.aspx");
            }

            FillCountry();
            FillEmployeeType();
            FillGender();            
            DisplayData();
        }
    }

    #region Drop Down Events

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCountry.SelectedIndex <= 0)
            {
                ddlState.Enabled = false;
                ddlState.Items.Clear();
                ddlState.Items.Add(new ListItem("-select state-", "0"));

                ddlCity.Enabled = false;
                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-select city-", "0"));
            }
            else
            {
                FillState();
            }

            FillBasedStationOnCountry(Convert.ToInt64(ddlCountry.SelectedItem.Value));

            if (ddlCountry.SelectedValue == "0" && ddlState.SelectedValue == "0")
            {
                ddlCity.Items.Remove(new ListItem("-Other-", "-Other-"));
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlState.SelectedIndex <= 0)
            {
                ddlCity.Enabled = false;
                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-select city-", "0"));
            }
            else
            {
                FillCity();
            }

            if (ddlState.SelectedIndex > 0)
            {
                ddlCity.Items.Insert(1, "-Other-");
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCity.SelectedValue == "-Other-")
        {
            PnlCityOther.Visible = true;
        }
        else
        {
            PnlCityOther.Visible = false;
        }
    }    

    #endregion

    #region Link Button Events

    /// <summary>
    /// Insert signup details
    /// </summary>    
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        Boolean Success = true;
        try
        {
            RegistrationRepository objRepo = new RegistrationRepository();
            Inc_Registration objReg = objRepo.GetByRegistrationID(this.RegistrationID);

            Int64 CityID = 0;
            //Add new city to INC_City Table
            if (PnlCityOther.Visible == true && ddlCity.SelectedItem.Text == "-Other-")
            {
                CityRepository objCityRep = new CityRepository();
                Int64 countryID = Convert.ToInt64(ddlCountry.SelectedItem.Value);
                Int64 stateID = Convert.ToInt64(ddlState.SelectedItem.Value);

                INC_City objCity = objCityRep.CheckIfExist(countryID, stateID, txtCity.Text.Trim());
                if (objCity == null)
                {
                    INC_City objCity1 = new INC_City();
                    objCity1.iCountryID = countryID;
                    objCity1.iStateID = stateID;
                    objCity1.sCityName = txtCity.Text.Trim();
                    objCityRep.Insert(objCity1);
                    objCityRep.SubmitChanges();
                    CityID = objCity1.iCityID;
                }
                else
                    CityID = objCity.iCityID;
            }
            else
                CityID = Convert.ToInt64(ddlCity.SelectedValue);
            
            objReg.sFirstName = txtFirstName.Text.Trim();
            objReg.sLastName = txtLastName.Text.Trim();
            objReg.sAddress1 = txtAdress.Text.Trim();
            objReg.iCountryId = Convert.ToInt64(ddlCountry.SelectedItem.Value);
            objReg.iStateId = Convert.ToInt64(ddlState.SelectedItem.Value);
            objReg.iCityId = CityID;
            objReg.sZipCode = txtZip.Text.Trim();
            objReg.sEmailAddress = txtEmail.Text.Trim();
            objReg.sTelephoneNumber = txtTelephone.Text.Trim();
            objReg.sEmployeeId = txtEmployeeId.Text.Trim();
            objReg.iWorkgroupId = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            objReg.iBasestationId = Convert.ToInt64(ddlBasestation.SelectedValue);
            objReg.iGender = Convert.ToInt64(ddlGender.SelectedValue);
            objReg.status = "pending";
            objReg.sCompanyName = txtSupplierCompanyName.Text.Trim();

            if (!String.IsNullOrEmpty(ddlEmployeeType.SelectedValue) && Convert.ToInt64(ddlEmployeeType.SelectedValue) > 0)
                objReg.iEmployeeTypeID = Convert.ToInt64(ddlEmployeeType.SelectedValue);
            else
                objReg.iEmployeeTypeID = null;

            if (!String.IsNullOrEmpty(txtDateOfHired.Text.Trim()))
                objReg.DOH = Convert.ToDateTime(txtDateOfHired.Text.Trim());
            else
                objReg.DOH = DateTime.MinValue;

            objRepo.SubmitChanges();
        }
        catch (Exception ex)
        {
            Success = false;
            ErrHandler.WriteError(ex);
            lblMsg.Text = ex.Message;
            lblMsg.Visible = true;
        }

        if (Success)
        {
            lblMsg.Text = "Registration details saved successfully...";
            Response.Redirect("~/MyAccount/ViewPendingUsers.aspx");
        }
    }

    #endregion

    #endregion

    #region Methods

    #region Bind Dropdown list control

    private void FillEmployeeType()
    {
        ddlEmployeeType.DataSource = objLookupRepos.GetByLookup("EmployeeType ");
        ddlEmployeeType.DataValueField = "iLookupID";
        ddlEmployeeType.DataTextField = "sLookupName";
        ddlEmployeeType.DataBind();
        ddlEmployeeType.Items.Insert(0, new ListItem("-select employee type-", "0"));
    }

    private void FillGender()
    {
        ddlGender.DataSource = objLookupRepos.GetByLookup("Gender");
        ddlGender.DataValueField = "iLookupID";
        ddlGender.DataTextField = "sLookupName";
        ddlGender.DataBind();
        ddlGender.Items.Insert(0, new ListItem("-select gender-", "0"));
    }

    private void FillCountry()
    {
        try
        {
            DataSet ds = new CountryDA().GetCountry();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCountry.DataSource = ds;
                ddlCountry.DataTextField = "sCountryName";
                ddlCountry.DataValueField = "iCountryID";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("-select country-", "0"));
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillState()
    {
        ddlState.Enabled = true;
        DataSet ds = new StateDA().GetStateByCountryID(Convert.ToInt32(ddlCountry.SelectedItem.Value));
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlState.DataSource = ds;
                ddlState.DataValueField = "iStateID";
                ddlState.DataTextField = "sStateName";
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("-select state-", "0"));

                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-select city-", "0"));
            }
        }
    }

    private void FillCity()
    {
        ddlCity.Enabled = true;
        DataSet ds = new CityDA().GetCityByStateID(Convert.ToInt32(ddlState.SelectedItem.Value));
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCity.DataSource = ds;
                ddlCity.DataValueField = "iCityID";
                ddlCity.DataTextField = "sCityName";
                ddlCity.DataBind();
                ddlCity.Items.Insert(0, new ListItem("-select city-", "0"));
            }
        }
    }    

    /// <summary>
    /// Fills the Workgroup.
    /// </summary>
    /// <param name="countryid">The countryid.</param>
    public void FillWorkGroupNDepartmentForStore(Int64 CompanyID)
    {
        ddlWorkgroup.Items.Clear();
        if (CompanyID > 0)
        {
            Int64 StoreID = new CompanyStoreRepository().GetStoreIDByCompanyId(CompanyID);

            CompanyStoreRepository objComStoRepo = new CompanyStoreRepository();

            List<GetStoreWorkGroupsResult> lstWorkGroups = new List<GetStoreWorkGroupsResult>();
            lstWorkGroups = objComStoRepo.GetStoreWorkGroups(StoreID).Where(le => le.Existing == 1).OrderBy(le => le.WorkGroup).ToList();

            if (lstWorkGroups.Count > 0)
                Common.BindDDL(ddlWorkgroup, lstWorkGroups, "WorkGroup", "WorkGroupID", "-select work group-");
        }
        else
            ddlWorkgroup.Items.Insert(0, new ListItem("-no work group-", "0"));
    }

    private void FillBasedStationOnCountry(Int64 CountryID)
    {
        ddlBasestation.Items.Clear();

        BaseStationDA sBaseStation = new BaseStationDA();
        BasedStationBE sBSBe = new BasedStationBE();

        sBSBe.SOperation = "getBaseStationbyCounty";
        sBSBe.iCountryID = CountryID;
        String sOptions = String.Empty;
        DataSet dsBaseStation = sBaseStation.GetBaseStaionByCountry(sBSBe);
        if (dsBaseStation.Tables.Count > 0 && dsBaseStation.Tables[0].Rows.Count > 0)
        {
            ddlBasestation.DataSource = dsBaseStation.Tables[0];
            ddlBasestation.DataValueField = "iBaseStationId";
            ddlBasestation.DataTextField = "sBaseStation";
            ddlBasestation.DataBind();
            ddlBasestation.Items.Insert(0, new ListItem("-select basestation-", "0"));
        }
        else
            ddlBasestation.Items.Insert(0, new ListItem("-no basestation-", "0"));
    }

    #endregion

    private void DisplayData()
    {
        try
        {
            Inc_Registration objReg = new RegistrationRepository().GetByRegistrationID(this.RegistrationID);

            if (objReg == null)
            {
                lblMsg.Text = "No details found.";
                return;
            }

            txtFirstName.Text = objReg.sFirstName;
            txtLastName.Text = objReg.sLastName;
            txtAdress.Text = objReg.sAddress1;

            ddlCountry.SelectedValue = objReg.iCountryId.ToString();
            ddlCountry_SelectedIndexChanged(null, null);

            ddlState.SelectedValue = objReg.iStateId.ToString();
            ddlState_SelectedIndexChanged(null, null);
            ddlCity.SelectedValue = objReg.iCityId.ToString();

            if (objReg.iCompanyName != null)
            {
                FillWorkGroupNDepartmentForStore(Convert.ToInt64(objReg.iCompanyName));
            }

            if (!String.IsNullOrEmpty(objReg.sCompanyName))
            {
                trSupplierCompanyName.Visible = true;
                txtSupplierCompanyName.Text = objReg.sCompanyName;
            }

            if (objReg.iEmployeeTypeID != null)
            {
                ddlEmployeeType.SelectedValue = objReg.iEmployeeTypeID.ToString();
            }

            ddlGender.SelectedValue = objReg.iGender.ToString();
            ddlWorkgroup.SelectedValue = objReg.iWorkgroupId.ToString();
            ddlBasestation.SelectedValue = objReg.iBasestationId.ToString();

            txtZip.Text = objReg.sZipCode;
            txtTelephone.Text = objReg.sTelephoneNumber;
            txtEmail.Text = objReg.sEmailAddress;

            if (Convert.ToDateTime(objReg.DOH).ToString("MM/dd/yyyy") != "01/01/0001")
            {
                txtDateOfHired.Text = Convert.ToDateTime(objReg.DOH).ToString("MM/dd/yyyy");
            }
            else
            {
                txtDateOfHired.Text = "";
            }

            txtEmployeeId.Text = objReg.sEmployeeId;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}