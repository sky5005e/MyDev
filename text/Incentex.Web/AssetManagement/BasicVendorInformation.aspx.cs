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
using System.Web.UI.HtmlControls;

public partial class AssetManagement_BasicVendorInformation : PageBase
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
            if (!IsPostBack)
            {
                base.MenuItem = "Manage Company";
                base.ParentMenuID = 50;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Add Company";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/VendorList.aspx";

                Common.MenuModuleId = Incentex.DAL.Common.DAEnums.ManageID.EquipmentVendor;
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

                BindAssociateCustomer();
                menuControl.PopulateMenu(0, 0, this.EquipmentVendorID, 0, false);
                lblMsg.Text = "";
                BindValues();
            }
            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowAssociateCustomer('" + chkIsCustomer.Checked + "');", true);
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


            //Fill Controls
            GetGSECompanyByIDResult objEquipmentCompanyDetails = new GetGSECompanyByIDResult();
            AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
            objEquipmentCompanyDetails = objAssetVendorRepository.GetEquipmentVendorListById(this.EquipmentVendorID);
            if (objEquipmentCompanyDetails != null)
            {
                ddlCompany.SelectedValue = Convert.ToString(objEquipmentCompanyDetails.CompanyID);
                txtVendorName.Text = objEquipmentCompanyDetails.EquipmentVendorName;
                txtContact.Text = objEquipmentCompanyDetails.Contact;
                txtAddress.Text = objEquipmentCompanyDetails.Address1;
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
                chkIsCustomer.Checked = objEquipmentCompanyDetails.IsCustomer;
                //if (objEquipmentCompanyDetails.IsCustomer == true)
                //    dvIsCustomer.Attributes.Add("class", "wheather_checked");
                //else
                //    dvIsCustomer.Attributes.Add("class", "wheather_check");
                ddlBaseStation.SelectedValue = Convert.ToString(objEquipmentCompanyDetails.BaseStationID);
                
                if (!string.IsNullOrEmpty(objEquipmentCompanyDetails.AssociateCustomerID))
                {
                    OldeSelectedIDs.Value = objEquipmentCompanyDetails.AssociateCustomerID;
                    foreach (DataListItem dtM in dlAssociateCustomer.Items)
                    {
                        CheckBox chk = dtM.FindControl("chkCustomer") as CheckBox;
                        HtmlGenericControl dvChk = dtM.FindControl("AssiciateCustomerSpan") as HtmlGenericControl;
                        string[] BID = objEquipmentCompanyDetails.AssociateCustomerID.Split(',');

                        foreach (string i in BID)
                        {
                            if (((HiddenField)dtM.FindControl("hdnEquipmentVendorID")).Value.Equals(i))
                            {
                                chk.Checked = true;
                                dvChk.Attributes.Add("class", "custom-checkbox_checked");
                                break;
                            }
                        }
                    }
                }

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
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanAdd && !base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
            EquipmentVendorMaster objEquipmentVendorMaster = new EquipmentVendorMaster();

            if (this.EquipmentVendorID != 0)
            {
                objEquipmentVendorMaster = objAssetVendorRepository.GetEquipmentVendorById(this.EquipmentVendorID);
            }


            objEquipmentVendorMaster.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            objEquipmentVendorMaster.EquipmentVendorName = txtVendorName.Text;
            objEquipmentVendorMaster.Contact = txtContact.Text;
            objEquipmentVendorMaster.Address1 = txtAddress.Text;
            objEquipmentVendorMaster.Address2 = txtAddress.Text;
            if (ddlCountry.SelectedValue != "0")
            {
                objEquipmentVendorMaster.CountryID = Convert.ToInt64(ddlCountry.SelectedValue);
            }
            if (ddlState.SelectedValue != "0")
            {
                objEquipmentVendorMaster.StateID = Convert.ToInt64(ddlState.SelectedValue);
            }
            if (ddlCity.SelectedValue != "0")
            {
                objEquipmentVendorMaster.CityID = Convert.ToInt64(ddlCity.SelectedValue);
            }
            objEquipmentVendorMaster.Zip = txtZip.Text.Trim();
            objEquipmentVendorMaster.Telephone = txtTelephone.Text.Trim();
            objEquipmentVendorMaster.Fax = txtFax.Text.Trim();
            objEquipmentVendorMaster.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objEquipmentVendorMaster.CreatedDate = DateTime.Now;
            if (ddlBaseStation.SelectedIndex != 0)
            {
                objEquipmentVendorMaster.BaseStationID = Convert.ToInt64(ddlBaseStation.SelectedValue);
            }
            if (chkIsCustomer.Checked)
                objEquipmentVendorMaster.IsCustomer = true;
            else
                objEquipmentVendorMaster.IsCustomer = false;

            if (this.EquipmentVendorID == 0)
            {
                objAssetVendorRepository.Insert(objEquipmentVendorMaster);
            }

            objAssetVendorRepository.SubmitChanges();

            //Associate Customer---------
            string strIDs = string.Empty;

            if (!chkIsCustomer.Checked)
            {
                //delete records from the table if its in edit mode and its a vendor company
                if (this.EquipmentVendorID != 0 && OldeSelectedIDs.Value != selectedIDs.Value)
                {
                    List<EquipmentAssociateCustomerForVendor> lstAssociateCustomer = new List<EquipmentAssociateCustomerForVendor>();
                    lstAssociateCustomer = objAssetVendorRepository.GetAssociateCustomerByVendorID(this.EquipmentVendorID);
                    objAssetVendorRepository.DeleteAll(lstAssociateCustomer);
                    objAssetVendorRepository.SubmitChanges();
                }

                foreach (DataListItem dt in dlAssociateCustomer.Items)
                {
                    if (((CheckBox)dt.FindControl("chkCustomer")).Checked == true)
                    {
                        EquipmentAssociateCustomerForVendor objAssociateCustomer = new EquipmentAssociateCustomerForVendor();
                        objAssociateCustomer.EquipmentVendorID = this.EquipmentVendorID;
                        objAssociateCustomer.AssociateCustomerID = Convert.ToInt64(((HiddenField)dt.FindControl("hdnEquipmentVendorID")).Value);
                        objAssetVendorRepository.Insert(objAssociateCustomer);
                    }
                }

                objAssetVendorRepository.SubmitChanges();
                
            }
            //---------------------

            Response.Redirect("VendorNotes.aspx?Id=" + this.EquipmentVendorID);

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
    public void ResetControls()
    {
        try
        {
            ddlCompany.SelectedIndex = 0;
            txtVendorName.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
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
            ddlBaseStation.SelectedIndex = 0;
            chkIsCustomer.Checked = false;
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


    public void BindAssociateCustomer()
    {
        AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
        var lstAssociateCustomer = objAssetVendorRepository.GetCustomerCompany(this.EquipmentVendorID).ToList();
        if (lstAssociateCustomer.Count > 0)
        {
            lblEmptyMessage.Visible = false;
            dlAssociateCustomer.Visible = true;
            dlAssociateCustomer.DataSource = lstAssociateCustomer;
            dlAssociateCustomer.DataBind();
        }
        else
        {
            lblEmptyMessage.Visible = true;
            dlAssociateCustomer.Visible = false;
            lblEmptyMessage.Text = "No data found";
        }
    }
}
