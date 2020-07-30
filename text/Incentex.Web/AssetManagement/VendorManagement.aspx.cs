using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Data;
using Incentex.DAL.Common;

public partial class AssetManagement_VendorManagement : PageBase
{
    CountryRepository obj = new CountryRepository();
    StateRepository objState = new StateRepository();
    CityRepository objCity = new CityRepository();
    Int64 EquipmentVendorID
    {
        get
        {
            if (ViewState["EquipmentVendorID"] == null)
            {
                ViewState["EquipmentVendorID"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentVendorID"]);
        }
        set
        {
            ViewState["EquipmentVendorID"] = value;
        }
    }
    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                MasterPageFile = "~/admin/MasterPageAdmin.master";
            }
            else
            {
                MasterPageFile = "~/MasterPage.master";
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            ((Label)Master.FindControl("lblPageHeading")).Text = "Add Vendor";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/VendorList.aspx";
            if (!IsPostBack)
            {
                if (Request.QueryString.Count > 0)
                {
                    this.EquipmentVendorID = Convert.ToInt64(Request.QueryString.Get("Id"));
                    //btnAddEmployee.Visible = true;
                    //btnViewEmployee.Visible = true;
                }
                else
                {
                    //btnAddEmployee.Visible = false;
                    //btnViewEmployee.Visible = false;
                }
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {

                    ddlCompany.SelectedValue = Convert.ToString(IncentexGlobal.CurrentMember.CompanyId);
                    ddlCompany.Enabled = false;


                }
                else
                {
                    ddlCompany.Items.Insert(0, new ListItem("-Select-", "0"));
                    ddlCompany.Enabled = true;
                }
                lblMsg.Text = "";
                BindValues();             
            }
        }
        catch (Exception)
        {


        }
    }

    public void BindValues()
    {
        try
        {
            FillCompCountry();

            //get Base Stations
            CompanyRepository objRepo = new CompanyRepository();
            List<INC_BasedStation> objBaseStationList = new List<INC_BasedStation>();
            objBaseStationList = objRepo.GetAllBaseStationResult();
            objBaseStationList = objBaseStationList.OrderBy(p => p.sBaseStation).ToList();
            Common.BindDDL(ddlBaseStation, objBaseStationList, "sBaseStation", "iBaseStationId", "-Select All BaseSation-");

            //get company
            List<Company> objCompanyList = new List<Company>();
            objCompanyList = objRepo.GetAllCompany();
            Common.BindDDL(ddlCompany, objCompanyList, "CompanyName", "CompanyID", "-Select Company-");


            GetGSECompanyByIDResult objEquipmentCompanyDetails = new GetGSECompanyByIDResult();
            AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
            objEquipmentCompanyDetails = objAssetVendorRepository.GetEquipmentVendorListById(this.EquipmentVendorID);
            if (objEquipmentCompanyDetails != null)
            {
                ddlCompany.SelectedValue = Convert.ToString(objEquipmentCompanyDetails.CompanyID);
                txtVendorName.Text = objEquipmentCompanyDetails.EquipmentVendorName;
                txtContact.Text = objEquipmentCompanyDetails.Contact;
                txtAddress1.Text = objEquipmentCompanyDetails.Address1;
                txtAddress2.Text = objEquipmentCompanyDetails.Address2;
                ddlCountry.SelectedValue = Convert.ToString(objEquipmentCompanyDetails.CountryID);
                //bind state
                ddlState.Enabled = true;
                ddlState.DataSource = objState.GetByCountryId(Convert.ToInt32(ddlCountry.SelectedValue));
                ddlState.DataBind();
                ddlState.SelectedValue = Convert.ToString(objEquipmentCompanyDetails.StateID);
                //bind city
                ddlCity.Enabled = true;
                ddlCity.DataSource = objCity.GetByStateId(Convert.ToInt32(ddlState.SelectedItem.Value));
                ddlCity.DataValueField = "iCityID";
                ddlCity.DataTextField = "sCityName";
                ddlCity.DataBind();
                ddlCity.SelectedValue = Convert.ToString(objEquipmentCompanyDetails.CityID);
                txtZip.Text = objEquipmentCompanyDetails.Zip;
                txtTelephone.Text = objEquipmentCompanyDetails.Telephone;
                txtFax.Text = objEquipmentCompanyDetails.Fax;
                ddlBaseStation.SelectedValue = Convert.ToString(objEquipmentCompanyDetails.BaseStationID);

            }


        }
        catch (Exception)
        {


        }
    }
    public void FillCompCountry()
    {
        try
        {
            //Company
            ddlCountry.DataSource = obj.GetAll();
            ddlCountry.DataTextField = "sCountryName";
            ddlCountry.DataValueField = "iCountryID";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("-select country-", "0"));
            ddlCountry.SelectedValue = ddlCountry.Items.FindByText("United States").Value;


            ddlState.DataSource = objState.GetByCountryId(Convert.ToInt32(ddlCountry.SelectedItem.Value));
            ddlState.DataValueField = "iStateID";
            ddlState.DataTextField = "sStateName";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("-select state-", "0"));

            ddlCity.Items.Clear();
            ddlCity.Items.Add(new ListItem("-select city-", "0"));




        }
        catch (System.Threading.ThreadAbortException lException)
        {

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
            EquipmentVendorMaster objEquipmentVendorMaster = new EquipmentVendorMaster();

            if (this.EquipmentVendorID != 0)
            {
                objEquipmentVendorMaster = objAssetVendorRepository.GetEquipmentVendorById(this.EquipmentVendorID);
            }


            objEquipmentVendorMaster.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            objEquipmentVendorMaster.EquipmentVendorName = txtVendorName.Text;
            objEquipmentVendorMaster.Contact = txtContact.Text;
            objEquipmentVendorMaster.Address1 = txtAddress1.Text;
            objEquipmentVendorMaster.Address2 = txtAddress2.Text;
            if (ddlCountry.SelectedValue!="0")
            {
                objEquipmentVendorMaster.CountryID = Convert.ToInt64(ddlCountry.SelectedValue);
            }
            if (ddlState.SelectedValue!="0")
            {
                objEquipmentVendorMaster.StateID = Convert.ToInt64(ddlState.SelectedValue);
            }
            if (ddlCity.SelectedValue!="0")
            {
                objEquipmentVendorMaster.CityID = Convert.ToInt64(ddlCity.SelectedValue);
            }           
            objEquipmentVendorMaster.Zip = txtZip.Text.Trim();
            objEquipmentVendorMaster.Telephone = txtTelephone.Text.Trim();
            objEquipmentVendorMaster.Fax = txtFax.Text.Trim();
            objEquipmentVendorMaster.Email = txtEmail.Text.Trim();
            objEquipmentVendorMaster.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objEquipmentVendorMaster.CreatedDate = DateTime.Now;
            if (ddlBaseStation.SelectedIndex!=0)
            {
                objEquipmentVendorMaster.BaseStationID = Convert.ToInt64(ddlBaseStation.SelectedValue);
            }

            if (this.EquipmentVendorID == 0)
            {
                objAssetVendorRepository.Insert(objEquipmentVendorMaster);
            }
           
            objAssetVendorRepository.SubmitChanges();
            lblMsg.Text = "Vendor Information Saved Successfully...";
            ResetControls();

        }
        catch (Exception)
        {


        }
    }
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
                ddlState.Enabled = true;
                ddlState.DataSource = objState.GetByCountryId(Convert.ToInt32(ddlCountry.SelectedValue));
                ddlState.DataTextField = "sStateName";
                ddlState.DataValueField = "iStateID";
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("-select state-", "0"));

                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-select city-", "0"));


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
                ddlCity.Enabled = true;

                ddlCity.DataSource = objCity.GetByStateId(Convert.ToInt32(ddlState.SelectedItem.Value));
                ddlCity.DataValueField = "iCityID";
                ddlCity.DataTextField = "sCityName";
                ddlCity.DataBind();
                ddlCity.Items.Insert(0, new ListItem("-select city-", "0"));

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
    public void ResetControls()
    {
        try
        {
            ddlCompany.SelectedIndex = 0;
            txtVendorName.Text = "";
            txtContact.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            ddlCountry.SelectedIndex = 0;
            //bind state
            ddlState.DataSource = objState.GetByCountryId(Convert.ToInt32(ddlCountry.SelectedValue));
            ddlState.DataBind();
            //bind city

            ddlCity.DataSource = objCity.GetByStateId(0);
            ddlCity.DataValueField = "iCityID";
            ddlCity.DataTextField = "sCityName";
            ddlCity.DataBind();
            txtZip.Text = "";
            txtTelephone.Text = "";
            txtFax.Text = "";
            txtEmail.Text = "";
            ddlBaseStation.SelectedIndex = 0;
        }
        catch (Exception)
        {
            
            
        }
    }
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        try
        {


            var dt = new DataTable();
            foreach (var info in typeof(T).GetProperties())
            {
                //table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                dt.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);
            }
            foreach (var t in list)
            {
                var row = dt.NewRow();
                foreach (var info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //protected void btnAddEmployee_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("BasicVendorEmpInformation.aspx?Id=" + Convert.ToString(this.EquipmentVendorID));
    //}
    //protected void btnViewEmployee_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("EmployeeList.aspx?Id=" + Convert.ToString(this.EquipmentVendorID));
    //}
}
