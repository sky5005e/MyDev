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
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL;
using Incentex.DAL.Common;

public partial class MyAccount_MySettings_MyShipping : PageBase
{
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    UserInformationRepository objUsrInfoRep = new UserInformationRepository();
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Company;
            }
            return (CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType)ViewState["SortExp"];
        }
        set
        {
            ViewState["SortExp"] = value;
        }
    }
    Incentex.DAL.Common.DAEnums.SortOrderType SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"] = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
            }
            return (Incentex.DAL.Common.DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }
    long CompanyContactInfoID
    {
        get
        {
            if (ViewState["CompanyContactInfoID"] == null)
            {
                ViewState["CompanyContactInfoID"] = "0";
            }
            return Convert.ToInt64(ViewState["CompanyContactInfoID"]);
        }
        set
        {
            ViewState["CompanyContactInfoID"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((Label)Master.FindControl("lblPageHeading")).Text = "Shipping Information";
            if (IncentexGlobal.IsIEFromStore)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/MySettings/MySettingMenuOptions.aspx";
            }

            BindDDL(sender, e);
            DisplayData(sender, e);
            TxtShippingCompany.Focus();
        }
    }
    #region Functions
    void BindDDL(object sender, EventArgs e)
    {
        CountryRepository objCountryBillingRepo = new CountryRepository();
        List<INC_Country> objBillingCountryList = objCountryBillingRepo.GetAll();   

        CountryRepository objCountryShippingRepo = new CountryRepository();
        List<INC_Country> objShippingCountryList = objCountryBillingRepo.GetAll();
        Common.BindDDL(DrpShipingCoutry, objShippingCountryList, "sCountryName", "iCountryID", "-select country-");
        DrpShipingCoutry.SelectedValue = DrpShipingCoutry.Items.FindByText("United States").Value;
        DrpShipingCoutry_SelectedIndexChanged(sender, e);

    }

    /// <summary>
    /// Display Existing Record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DisplayData(object sender, EventArgs e)
    {
        //Shipping
        Company objCompany = new CompanyRepository().GetById(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
        TxtShippingCompany.Text = objCompany.CompanyName;
        bindgrid();

    }

    /// <summary>
    /// Clear Shipping Fields
    /// </summary>
    void ClearShippingFields(object sender, EventArgs e)
    {
        TxtShippingCompany.Text = "";
        TxtShippingName.Text = "";
        TxtShipingTitle.Text = "";
        //DrpShipingState.SelectedValue = "0";
        DrpShippingCity.SelectedValue = "0";
        TxtShippingZip.Text = "";
        TxtShppingTelephone.Text = "";
        //Fax is Last Name now
        TxtShipingFAX.Text = "";
        TxtShippingMobile.Text = "";
        //TxtShippingEmail.Text = "";
        TxtShipingAddress.Text = "";
        txtAddress2.Text = "";
        txtStreet.Text = "";
        //txtCounty.Text = "";
        txtAirport.Text = "";
        hdnDepartment.Value = "0";
        DrpShipingCoutry.SelectedValue = DrpShipingCoutry.Items.FindByText("United States").Value;
        DrpShipingCoutry_SelectedIndexChanged(sender, e);
    }

    #endregion
    #region Events
    protected void DrpShipingCoutry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(DrpShipingCoutry.SelectedValue));

        DrpShipingState.Items.Clear();
        DrpShipingState.ClearSelection();
        DrpShippingCity.Items.Clear();
        DrpShippingCity.Items.Insert(0, new ListItem("-select city-", "0"));
        Common.BindDDL(DrpShipingState, objStateList, "sStatename", "iStateID", "-select state-");
        if (DrpShipingCoutry.SelectedValue == "0" && DrpShipingState.SelectedValue == "0")
        {
            DrpShippingCity.Items.Remove(new ListItem("-Other-", "-Other-"));
        }
    }

    protected void DrpShipingState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(DrpShipingState.SelectedValue));
        DrpShippingCity.Items.Clear();
        Common.BindDDL(DrpShippingCity, objCityList, "sCityName", "iCityID", "-select city-");
        if (DrpShipingState.SelectedIndex > 0)
        {
            //DrpShippingCity.Items.Add(new ListItem("-Other-", "-1"));
            DrpShippingCity.Items.Insert(1, "-Other-");
        }
    }

    protected void dtlShipping_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblMsg.Text = "";

        Int64 CompanyContactInfoID = 0;
        CompanyEmployeeContactInfo objShipInfo = null;
        switch (e.CommandName)
        {
            case "EditRec":
                PnlCityOther.Visible = false;
                //display billing record to edit
                CompanyContactInfoID = Convert.ToInt64(e.CommandArgument);
                objShipInfo = objCmpEmpContRep.GetShippingDetailsByContactInfoID(CompanyContactInfoID);

                if (objShipInfo != null)
                {
                    this.CompanyContactInfoID = CompanyContactInfoID;
                    TxtShippingCompany.Text = objShipInfo.CompanyName;
                    //Name is First Name Now for the shipping
                    TxtShippingName.Text = objShipInfo.Name;
                    TxtShipingTitle.Text = objShipInfo.Title;
                    DrpShipingCoutry.SelectedValue = Common.GetLongString(objShipInfo.CountryID);
                    DrpShipingCoutry_SelectedIndexChanged(sender, e);

                    DrpShipingState.SelectedValue = Common.GetLongString(objShipInfo.StateID);
                    DrpShipingState_SelectedIndexChanged(sender, e);

                    DrpShippingCity.SelectedValue = Common.GetLongString(objShipInfo.CityID);

                    //add by mayur on 26-jan-2012
                    hdnDepartment.Value = objShipInfo.DepartmentID != null ? objShipInfo.DepartmentID.ToString() : "0";
                    txtAirport.Text = objShipInfo.Airport;
                    //end add by mayur on 26-jan-2012

                    TxtShippingZip.Text = objShipInfo.ZipCode;
                    TxtShppingTelephone.Text = objShipInfo.Telephone;
                    //Fax is Last name now for the shipping
                    TxtShipingFAX.Text = objShipInfo.Fax;
                    TxtShippingMobile.Text = objShipInfo.Mobile;
                    //TxtShippingEmail.Text = objShipInfo.Email;
                    TxtShipingAddress.Text = objShipInfo.Address;
                    txtAddress2.Text = objShipInfo.Address2;
                    txtStreet.Text = objShipInfo.Street;
                    //txtCounty.Text = objShipInfo.county;
                    TxtShippingCompany.Focus();
                   
                }

                break;
            case "DeleteRec":
                try
                {
                    CompanyContactInfoID = Convert.ToInt64(e.CommandArgument);
                    objShipInfo = objCmpEmpContRep.GetShippingDetailsByContactInfoID(CompanyContactInfoID);
                    objCmpEmpContRep.Delete(objShipInfo);
                    objCmpEmpContRep.SubmitChanges();
                    lblMsg.Text = "Record deleted successfully ...";
                    bindgrid();
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex)).Number == 547)
                    {
                        lblMsg.Text = "Unable to delete Shipping information as it is being used in orders!!";
                    }
                    else
                    {
                        ErrHandler.WriteError(ex);
                        lblMsg.Text = "Error in deleting record ...";
                    }

                }

                break;
            case "Sort":
                if (this.SortExp.ToString() == e.CommandArgument.ToString())
                {
                    if (this.SortOrder == Incentex.DAL.Common.DAEnums.SortOrderType.Asc)
                        this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Desc;
                    else
                        this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
                }
                else
                {
                    this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
                   // this.SortExp = (CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType)Enum.Parse(typeof(CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType), e.CommandArgument.ToString());
                }
                bindgrid();
                break;
        }
    }

    /// <summary>
    /// Insert or update shipping address
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkBtnAddAnotherShiping_Click(object sender, EventArgs e)
    {

        Int64 CityID = 0;
        //Add new city to INC_City Table
        if (PnlCityOther.Visible == true && DrpShippingCity.SelectedItem.Text == "-Other-")
        {
            CityRepository objCityRep = new CityRepository();
            Int64 countryID = Convert.ToInt64(DrpShipingCoutry.SelectedItem.Value);
            Int64 stateID = Convert.ToInt64(DrpShipingState.SelectedItem.Value);

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
        //End
        else
            CityID = Convert.ToInt64(DrpShippingCity.SelectedValue);
        //End

        lblMsg.Text = "";
        CompanyEmployeeContactInfo objShipInfo = new CompanyEmployeeContactInfo();

        string oldTitle = string.Empty; // Added by shehzad 5-jan-2011

        try
        {
            if (this.CompanyContactInfoID != 0)
            {
                objShipInfo = objCmpEmpContRep.GetShippingDetailsByContactInfoID(this.CompanyContactInfoID);
                oldTitle = objShipInfo.Title; // Added by shehzad 5-jan-2011
            }

            objShipInfo.CompanyName = TxtShippingCompany.Text.Trim();
            objShipInfo.Name = TxtShippingName.Text.Trim();
            objShipInfo.Title = TxtShipingTitle.Text.Trim();
            objShipInfo.CountryID = Common.GetLongNull(DrpShipingCoutry.SelectedValue, true);
            objShipInfo.StateID = Common.GetLongNull(DrpShipingState.SelectedValue, true);
            objShipInfo.CityID = CityID;
            objShipInfo.ZipCode = TxtShippingZip.Text.Trim();
            objShipInfo.Telephone = TxtShppingTelephone.Text.Trim();
            objShipInfo.Fax = TxtShipingFAX.Text.Trim();
            objShipInfo.Mobile = TxtShippingMobile.Text.Trim();
            //objShipInfo.Email = TxtShippingEmail.Text.Trim();
            objShipInfo.Address = TxtShipingAddress.Text.Trim();
            objShipInfo.Address2 = txtAddress2.Text.Trim();
            objShipInfo.Street = txtStreet.Text.Trim();
            objShipInfo.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
            //objShipInfo.county = txtCounty.Text.Trim();

            //start add by mayur on 26-jan-2012
            if(!string.IsNullOrEmpty(hdnDepartment.Value) && hdnDepartment.Value!="0")
                objShipInfo.DepartmentID = Convert.ToInt64(hdnDepartment.Value);
            else
                objShipInfo.DepartmentID = null;
            objShipInfo.Airport = txtAirport.Text.Trim();
            //end by mayur on 26-jan-2012

            if (this.CompanyContactInfoID == 0)
            {
                // shehzad start 5-jan-2011 => Check for duplicate title
                if (oldTitle.ToLower() == TxtShipingTitle.Text.ToLower())
                {
                    lblMsg.Text = "Title already exists please enter another Title could not save shipping address.";
                    return;
                }
                else // Shehzad End
                {
                    //insert
                    objShipInfo.ContactInfoType = DAEnums.CompanyEmployeeContactInfo.Shipping.ToString();
                    objShipInfo.OrderType = "MySetting";
                    objCmpEmpContRep.Insert(objShipInfo);
                }
            }

            objCmpEmpContRep.SubmitChanges();
            this.CompanyContactInfoID = 0;
            lblMsg.Text = "Record Saved Successfully ...";
            bindgrid();

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

            if (ex.ToString().Contains("Cannot insert duplicate key"))
                lblMsg.Text = "Title already exists please enter another Title could not save shipping address.";
            else
                lblMsg.Text = "Error in saving shipping address ...";
        }
        ClearShippingFields(sender, e);
    }
    /// <summary>
    /// Binds the Shipping address Grid
    /// </summary>
    public void bindgrid()
    {
        List<CompanyEmployeeContactInfo> ObjShipingDetailList = objCmpEmpContRep.GetSavedShippingAddressesForUser(IncentexGlobal.CurrentMember.UserInfoID, this.SortExp, this.SortOrder);
        dtlShipping.DataSource = ObjShipingDetailList;
        dtlShipping.DataBind();
    }
    #endregion
    #region Miscellaneous functions

    /// <summary>
    /// Function which converts List to the DataTable
    /// </summary>
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null);
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    #endregion

    protected void DrpShippingCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DrpShippingCity.SelectedValue == "-Other-")
        {
            PnlCityOther.Visible = true;

        }
        else
        {
            PnlCityOther.Visible = false;
        }
    }
}
