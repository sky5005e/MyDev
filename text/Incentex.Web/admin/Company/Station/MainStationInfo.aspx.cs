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
using System.Xml.Linq;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DA;
using Incentex.BE;


public partial class admin_Company_Station_AddStation : PageBase
{

   
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

    Int64 StationId
    {
        get
        {
            if (ViewState["StationId"] == null)
            {
                ViewState["StationId"] = 0;
            }
            return Convert.ToInt64(ViewState["StationId"]);
        }
        set
        {
            ViewState["StationId"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Stations";
            base.ParentMenuID = 11;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Main Station Information";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to company listing</span>";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Company/ViewCompany.aspx";
           
            if (Request.QueryString.Count > 0)
            {
                
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.StationId = Convert.ToInt64(Request.QueryString.Get("SubId"));

                manuControl.PopulateMenu(3, 0, this.CompanyId, this.StationId,true);

                Company objCompany = new CompanyRepository().GetById(this.CompanyId);

                if(objCompany == null)
                {
                    Response.Redirect("~/admin/Company/Station/ViewStation.aspx");
                }
                lblCompany.Text = objCompany.CompanyName;
            }
            else
            {
                Response.Redirect("~/admin/Company/Station/ViewStation.aspx");
            }

            BindDDL(sender,e);
            DisplayData(sender,e);
        
        }
    }

    void DisplayData(object sender, EventArgs e)
    {
        if(this.StationId != 0)
        {
            CompanyStation objCompanyStation = new CompanyStationRepository().GetById(this.StationId);

            if (objCompanyStation == null)
            {
                return;
            }


            txtCode.Text = objCompanyStation.StationCode;
            txtAddress.Text = objCompanyStation.Address;
            ddlCountry.SelectedValue = objCompanyStation.CountryId.ToString();
            ddlCountry_SelectedIndexChanged(sender, e);

            ddlState.SelectedValue = objCompanyStation.StateId.ToString();
            ddlState_SelectedIndexChanged(sender, e);

            ddlCity.SelectedValue = objCompanyStation.CityId.ToString();

            txtTel.Text = objCompanyStation.Telephone;
            txtFax.Text = objCompanyStation.Fax;
            txtAirport.Text = objCompanyStation.AirportName;

            txtStationOperationingSince.Text =Common.GetDateString(objCompanyStation.OperatingSinceDate);
            txtSetupDate.Text = Common.GetDateString(objCompanyStation.StationSetupDate);
            txtCostCenter.Text = objCompanyStation.StationCostCenter;
            txtStationNumber.Text = objCompanyStation.StationNumber;

            txtStationId.Text = objCompanyStation.StationIdInput;
            txt3CompanyName.Text = objCompanyStation.ThirdPartyCompanyName;
            txt3OperatingSince.Text =Common.GetDateString(objCompanyStation.ThirdPartyOperartinSince);
            txt3CorporateContact.Text = objCompanyStation.ThirdPartyCorporateContact;
            txt3ContractTerm.Text = objCompanyStation.ThirdPartyContractTerms;
            if (objCompanyStation.Active != null)
            {
                ddlStatus.SelectedValue = objCompanyStation.Active.ToString();
            }
            if (objCompanyStation.Zip != null)
            {
                txtZip.Text = objCompanyStation.Zip;
            }
            else
            {
                txtZip.Text = "";
            }

        }
    }

    void BindDDL(object sender, EventArgs e)
    {

        CountryRepository objCountryRepo = new CountryRepository();
        List<INC_Country> objCountryList = objCountryRepo.GetAll();

        Common.BindDDL(ddlCountry, objCountryList, "sCountryName", "iCountryID", "-select country-");

        ddlCountry.SelectedValue = ddlCountry.Items.FindByText("United States").Value;

        ddlCountry_SelectedIndexChanged(sender, e);

        LookupDA sStatus = new LookupDA();
        LookupBE sStatusBE = new LookupBE();
        sStatusBE.SOperation = "selectall";
        sStatusBE.iLookupCode = "Status";
        ddlStatus.DataSource = sStatus.LookUp(sStatusBE);
        ddlStatus.DataValueField = "iLookupID";
        ddlStatus.DataTextField = "sLookupName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-select status-", "0"));
    }

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId( Convert.ToInt64(ddlCountry.SelectedValue));

        ddlState.Items.Clear();
        ddlCity.Items.Clear();
        ddlCity.Items.Insert(0, new ListItem("-select city-", "0")); 
        Common.BindDDL(ddlState, objStateList, "sStatename", "iStateID", "-select state-");

    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId( Convert.ToInt64(ddlState.SelectedValue));
        ddlCity.Items.Clear();
        Common.BindDDL(ddlCity,objCityList,"sCityName","iCityID","-select city-");
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        CompanyStation objCompanyStation = new CompanyStation();
        CompanyStationRepository objRepo = new CompanyStationRepository();


        if (this.StationId != 0)
        {
            objCompanyStation = objRepo.GetById(this.StationId);

            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }
        }
        else
        {
            if (!base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }
        }


        objCompanyStation.CompanyID = this.CompanyId;

        objCompanyStation.StationCode = txtCode.Text;
        objCompanyStation.Address = txtAddress.Text;
        objCompanyStation.CountryId = Convert.ToInt64(ddlCountry.SelectedValue);
        objCompanyStation.StateId = Convert.ToInt64(ddlState.SelectedValue);
        objCompanyStation.CityId = Convert.ToInt64(ddlCity.SelectedValue);

        objCompanyStation.Telephone = txtTel.Text;
        objCompanyStation.Fax = txtFax.Text;
        objCompanyStation.AirportName = txtAirport.Text;

        objCompanyStation.OperatingSinceDate = Common.GetDate(txtStationOperationingSince);
        objCompanyStation.StationSetupDate = Common.GetDate(txtSetupDate);
        objCompanyStation.StationCostCenter = txtCostCenter.Text;
        objCompanyStation.StationNumber = txtStationNumber.Text;

        objCompanyStation.StationIdInput = txtStationId.Text;
        objCompanyStation.ThirdPartyCompanyName = txt3CompanyName.Text;
        objCompanyStation.ThirdPartyOperartinSince = Common.GetDate(txt3OperatingSince);
        objCompanyStation.ThirdPartyCorporateContact = txt3CorporateContact.Text;
        objCompanyStation.ThirdPartyContractTerms = txt3ContractTerm.Text;
        objCompanyStation.Zip = txtZip.Text;
        objCompanyStation.Active = Convert.ToInt16(ddlStatus.SelectedValue);
        try
        {

            if (this.StationId == 0)
            {
                objRepo.Insert(objCompanyStation);
            }

            objRepo.SubmitChanges();
            this.StationId = objCompanyStation.StationID;

            lblMsg.Text = "Record Saved Successfully ...";
            Response.Redirect("ManagerInfo.aspx?Id=" + this.CompanyId + "&SubId=" + this.StationId);

            // ClearData();
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in saving record ...";
            ErrHandler.WriteError(ex);
        }


    }

    void ClearData()
    {
        txtCode.Text = "";
        txtAddress.Text = "";
        ddlCountry.SelectedValue = "0";
        ddlState.SelectedValue = "0";
        ddlCity.SelectedValue = "0";

        txtTel.Text = "";
        txtFax.Text = "";
        txtAirport.Text = "";

        txtStationOperationingSince.Text = "";
        txtSetupDate.Text = "";
        txtCostCenter.Text = "";
        txtStationNumber.Text = "";

        txtStationId.Text = "";
        txt3CompanyName.Text = "";
        txt3OperatingSince.Text = "";
        txt3CorporateContact.Text = "";
        txt3ContractTerm.Text = "";
        txtZip.Text = "";
        
    }

}//
