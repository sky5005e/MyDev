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
using Incentex.DAL;
using System.Collections.Generic;
using Incentex.DAL.SqlRepository;
using Incentex.DA;

public partial class admin_CompanyStore_CompanyPrograms_IssuanceCompanyAddress : PageBase
{
    Int64 workgroupid
    {
        get
        {
            if (ViewState["workgroupid"] == null)
            {
                ViewState["workgroupid"] = 0;
            }
            return Convert.ToInt64(ViewState["workgroupid"]);
        }
        set
        {
            ViewState["workgroupid"] = value;
        }
    }
    Int64 CompanyStoreId
    {
        get
        {
            if (ViewState["CompanyStoreId"] == null)
            {
                ViewState["CompanyStoreId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {

            ViewState["CompanyStoreId"] = value;
        }
    }
    Int64 UniformIssuancePolicyID
    {
        get
        {
            if (ViewState["UniformIssuancePolicyID"] == null)
            {
                ViewState["UniformIssuancePolicyID"] = 0;
            }
            return Convert.ToInt64(ViewState["UniformIssuancePolicyID"]);
        }
        set
        {
            ViewState["UniformIssuancePolicyID"] = value;
        }
    }
    Int64 IssuanceCompanyAddressId
    {
        get
        {
            if (ViewState["IssuanceCompanyAddressId"] == null)
            {
                ViewState["IssuanceCompanyAddressId"] = 0;
            }
            return Convert.ToInt64(ViewState["IssuanceCompanyAddressId"]);
        }
        set
        {
            ViewState["IssuanceCompanyAddressId"] = value;
        }
    }
    String PaymentType
    {
        get
        {
            if (ViewState["PaymentType"] == null)
            {
                ViewState["PaymentType"] = null;
            }
            return Convert.ToString(ViewState["PaymentType"]);
        }
        set
        {
            ViewState["PaymentType"] = value;
        }
    }
    String Programname
    {
        get
        {
            if (ViewState["Programname"] == null)
            {
                ViewState["Programname"] = null;
            }
            return Convert.ToString(ViewState["Programname"]);
        }
        set
        {
            ViewState["Programname"] = value;
        }
    }
    String CompletePR
    {
        get
        {
            if (ViewState["CompletePR"] == null)
            {
                ViewState["CompletePR"] = null;
            }
            return Convert.ToString(ViewState["CompletePR"]);
        }
        set
        {
            ViewState["CompletePR"] = value;
        }
    }
    String PriceLevel
    {
        get
        {
            if (ViewState["PriceLevel"] == null)
            {
                ViewState["PriceLevel"] = null;
            }
            return Convert.ToString(ViewState["PriceLevel"]);
        }
        set
        {
            ViewState["PriceLevel"] = value;
        }
    }
    String ShowPrice
    {
        get
        {
            if (ViewState["ShowPrice"] == null)
            {
                ViewState["ShowPrice"] = null;
            }
            return Convert.ToString(ViewState["ShowPrice"]);
        }
        set
        {
            ViewState["ShowPrice"] = value;
        }
    }
    String Pricefor
    {
        get
        {
            if (ViewState["Pricefor"] == null)
            {
                ViewState["Pricefor"] = null;
            }
            return Convert.ToString(ViewState["Pricefor"]);
        }
        set
        {
            ViewState["Pricefor"] = value;
        }
    }
    String Department
    {
        get
        {
            if (ViewState["Department"] == null)
            {
                ViewState["Department"] = null;
            }
            return Convert.ToString(ViewState["Department"]);
        }
        set
        {
            ViewState["Department"] = value;
        }
    }
    String Workgroup
    {
        get
        {
            if (ViewState["Workgroup"] == null)
            {
                ViewState["Workgroup"] = null;
            }
            return Convert.ToString(ViewState["Workgroup"]);
        }
        set
        {
            ViewState["Workgroup"] = value;
        }
    }
    String PolicyDate
    {
        get
        {
            if (ViewState["PolicyDate"] == null)
            {
                ViewState["PolicyDate"] = null;
            }
            return Convert.ToString(ViewState["PolicyDate"]);
        }
        set
        {
            ViewState["PolicyDate"] = value;
        }
    }
    String ShowBeforeAfter
    {
        get
        {
            if (ViewState["ShowBeforeAfter"] == null)
            {
                ViewState["ShowBeforeAfter"] = null;
            }
            return Convert.ToString(ViewState["ShowBeforeAfter"]);
        }
        set
        {
            ViewState["ShowBeforeAfter"] = value;
        }
    }
    String ShowShippingPayment
    {
        get
        {
            if (ViewState["ShowShippingPayment"] == null)
            {
                ViewState["ShowShippingPayment"] = null;
            }
            return Convert.ToString(ViewState["ShowShippingPayment"]);
        }
        set
        {
            ViewState["ShowShippingPayment"] = value;
        }
    }
    String Gender
    {
        get
        {
            if (ViewState["Gender"] == null)
            {
                ViewState["Gender"] = null;
            }
            return Convert.ToString(ViewState["Gender"]);
        }
        set
        {
            ViewState["Gender"] = value;
        }
    }
    DataSet ds;
    CountryDA objCountry = new CountryDA();
    StateDA objState = new StateDA();
    CityDA objCity = new CityDA();
    LookupDA objLookuoDA = new LookupDA();
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    UserInformationRepository objUsrInfoRep = new UserInformationRepository();
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    IssuanceCompanyAddressRepository objNewAddressRepos=new IssuanceCompanyAddressRepository();
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack)
        {

               this.Programname=Request.QueryString.Get("Programname");
               this.CompletePR=Request.QueryString.Get("CompletePR");
               this.PriceLevel=Request.QueryString.Get("PriceLevel");
               this.ShowPrice=Request.QueryString.Get("ShowPrice");
               this.Pricefor=Request.QueryString.Get("Pricefor");
               this.Department=Request.QueryString.Get("Department");
               this.Workgroup=Request.QueryString.Get("Workgroup");
               this.PolicyDate=Request.QueryString.Get("PolicyDate");
               this.ShowBeforeAfter=Request.QueryString.Get("ShowBeforeAfter");
               this.ShowShippingPayment = Request.QueryString.Get("ShowShippingPayment");
               this.Gender = Request.QueryString.Get("Gender");
               this.PaymentType = Request.QueryString.Get("PaymentType");
               this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));
               this.UniformIssuancePolicyID = Convert.ToInt64(Request.QueryString.Get("SubId"));
               ((Label)Master.FindControl("lblPageHeading")).Text = " Billing/Shipping Address";
               if (PaymentType == "CompanyPays")
               {
                   lblFor.Text = "Address For Company Pays";
               }
               else
               {
                   lblFor.Text = "Address For MOAS";
               }
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/CompanyPrograms/UniformIssuanceStep1.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType + "&IssuanceCompanyAddressId="+IssuanceCompanyAddressId;
            menuControl.PopulateMenu(4, 1, this.CompanyStoreId, this.UniformIssuancePolicyID, true);
            if (Request.QueryString["PaymentType"] == "CompanyPays" || Request.QueryString["PaymentType"] == "MOAS")
            {
                FillBCountry();
                FillSCountry();
                DisplayData(sender, e);
               
            }
            
        }

    }
    #region Display
    /// <summary>
    /// Display Existing Record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DisplayData(object sender, EventArgs e)
    {
        //Billing
        IssuanceCompanyAddress objUniformIssuancePolicyItem = new IssuanceCompanyAddress();

        IssuanceCompanyAddressRepository objRepos = new IssuanceCompanyAddressRepository();
        this.UniformIssuancePolicyID = Convert.ToInt64(Request.QueryString.Get("SubId"));
        objUniformIssuancePolicyItem = objRepos.GetByUniformIssuancePolicyId(UniformIssuancePolicyID);
        if (objUniformIssuancePolicyItem != null)
        {
            if (objUniformIssuancePolicyItem.CompanyId.ToString() != "")
            {

              
                //Billing
                TxtFirstName.Text = objUniformIssuancePolicyItem.FirstName;
                TxtLastName.Text = objUniformIssuancePolicyItem.LastName;
                TxtPhone.Text = objUniformIssuancePolicyItem.Telephone;
                TxtBillingAddress1.Text = objUniformIssuancePolicyItem.Address1;
                TxtBillingAddress2.Text = objUniformIssuancePolicyItem.Address2;
                TxtBillingCompanyName.Text = objUniformIssuancePolicyItem.BCompanyName;
                FillBCountry();
                if (objUniformIssuancePolicyItem.CountryId.ToString() != "")
                {
                    DrpBillingCountry.SelectedValue = objUniformIssuancePolicyItem.CountryId.ToString();
                }
                DrpBillingCountry_SelectedIndexChanged(sender, e);
                if (objUniformIssuancePolicyItem.StateId.ToString() != "")
                {
                    DrpBillingState.SelectedValue = objUniformIssuancePolicyItem.StateId.ToString();
                }
                DrpBillingState_SelectedIndexChanged(sender, e);
                if (objUniformIssuancePolicyItem.CityId.ToString() != "")
                {
                    DrpBillingCity.SelectedValue = objUniformIssuancePolicyItem.CityId.ToString();
                }
                TxtBillingZip.Text = objUniformIssuancePolicyItem.ZipCode;
                TxtMobile.Text = objUniformIssuancePolicyItem.Mobile;
                TxtEmail.Text = objUniformIssuancePolicyItem.Email;
                //Shipping
                txtSFName.Text = objUniformIssuancePolicyItem.FirstSName;
                txtSLName.Text = objUniformIssuancePolicyItem.LastSName;
                txtSTelephone.Text = objUniformIssuancePolicyItem.STelephone;
                txtSAddress1.Text = objUniformIssuancePolicyItem.SAddress1;
                txtSAddress2.Text = objUniformIssuancePolicyItem.SAddress2;
                txtSCompany.Text = objUniformIssuancePolicyItem.SCompanyName;
                FillSCountry();
                if (objUniformIssuancePolicyItem.SCountryId.ToString() != "")
                {
                    ddlSCountry.SelectedValue = objUniformIssuancePolicyItem.SCountryId.ToString();
                }
                ddlSCountry_SelectedIndexChanged(sender, e);
                if (objUniformIssuancePolicyItem.SStateId.ToString() != "")
                {
                    ddlSState.SelectedValue = objUniformIssuancePolicyItem.SStateId.ToString();
                }
                ddlSState_SelectedIndexChanged(sender, e);
                if (objUniformIssuancePolicyItem.SCityId.ToString() != "")
                {
                    ddlSCity.SelectedValue = objUniformIssuancePolicyItem.SCityId.ToString();
                }
                txtSZip.Text = objUniformIssuancePolicyItem.SZipCode;
                txtSMobile.Text = objUniformIssuancePolicyItem.SMobile;
                txtSEmail.Text = objUniformIssuancePolicyItem.SEmail;

            }
            
        }
        
    }
    #endregion
    private void ClrData()
    {
       
        TxtBillingAddress1.Text = "";
        TxtBillingAddress2.Text = "";
        //TxtBillingCompanyName.Text = "";
        TxtBillingZip.Text = "";
        TxtEmail.Text = "";
        TxtFirstName.Text = "";
        TxtLastName.Text = "";
        TxtMobile.Text = "";
        TxtPhone.Text = "";

        //Shipping

        txtSAddress1.Text = "";
        txtSAddress2.Text = "";
        txtSZip.Text = "";
        txtSEmail.Text = "";
        txtSFName.Text = "";
        txtSLName.Text = "";
        txtSMobile.Text = "";
        txtSTelephone.Text = "";

        

    }
    protected void lnkAddItem_Click(object sender, EventArgs e)
    {
       
        //end 
        lblMsg.Text = "";
        //Billing
        string strFName = null;
        string strLName = null;
        string strAddress1 = null;
        string strAddress2 = null;
        string strTelePhone = null;
        string strMobile = null;
        string strZipCode = null;
        string strEmail = null;
        long intCityId = 0;
        long intCountryId = 0;
        long intStateId = 0;
        long CompanyId = 0;
        string strBCompanyName = null;
        //Shipping
        string strSFName = null;
        string strSLName = null;
        string strSAddress1 = null;
        string strSAddress2 = null;
        string strSTelePhone = null;
        string strSMobile = null;
        string strSZipCode = null;
        string strSEmail = null;
        long intSCityId = 0;
        long intSCountryId = 0;
        long intSStateId = 0;
        long SCompanyId = 0;
        string strCompanyName = null;
        try
        {
            if (Request.QueryString["PaymentType"] == "CompanyPays" || Request.QueryString["PaymentType"] == "MOAS")
                {
                    //Billing
                    if (TxtFirstName.Text != "")
                    {
                        strFName = TxtFirstName.Text;
                    }
                    if (TxtLastName.Text != "")
                    {
                        strLName = TxtLastName.Text;
                    }
                    if (TxtBillingAddress1.Text != "")
                    {
                        strAddress1 = TxtBillingAddress1.Text;
                    }
                    if (TxtBillingAddress2.Text != "")
                    {
                        strAddress2 = TxtBillingAddress2.Text;
                    }
                    if (TxtBillingCompanyName.Text != null)
                    {

                        strBCompanyName = TxtBillingCompanyName.Text;

                    }
                    if (TxtBillingZip.Text != null)
                    {
                        strZipCode = TxtBillingZip.Text;
                    }
                    if (TxtEmail.Text != "")
                    {
                        strEmail = TxtEmail.Text;
                    }
                    if (TxtPhone.Text != "")
                    {
                        strTelePhone = TxtPhone.Text;
                    }
                    if (TxtMobile.Text != "")
                    {
                        strMobile = TxtMobile.Text;
                    }
                    if (DrpBillingCountry.SelectedIndex > 0)
                    {
                        intCountryId = Convert.ToInt64(DrpBillingCountry.SelectedValue);
                    }
                    if (DrpBillingState.SelectedIndex > 0)
                    {
                        intStateId = Convert.ToInt64(DrpBillingState.SelectedValue);
                    }
                    if (DrpBillingCity.SelectedIndex > 0)
                    {
                        intCityId = Convert.ToInt64(DrpBillingCity.SelectedValue);
                    }
                    //Shipping                    
                    if (txtSFName.Text != "")
                    {
                        strSFName = txtSFName.Text;
                    }
                    if (txtSLName.Text != "")
                    {
                        strSLName = txtSLName.Text;
                    }
                    if (txtSAddress1.Text != "")
                    {
                        strSAddress1 = txtSAddress1.Text;
                    }
                    if (txtSAddress2.Text != "")
                    {
                        strSAddress2 = txtSAddress2.Text;
                    }
                    if (txtSCompany.Text != null)
                    {

                        strCompanyName = txtSCompany.Text;
                    }
                    if (txtSZip.Text != null)
                    {
                        strSZipCode = txtSZip.Text;
                    }
                    if (txtSEmail.Text != "")
                    {
                        strSEmail = txtSEmail.Text;
                    }
                    if (txtSTelephone.Text != "")
                    {
                        strSTelePhone = txtSTelephone.Text;
                    }
                    if (txtSMobile.Text != "")
                    {
                        strSMobile = txtSMobile.Text;
                    }
                    if (ddlSCountry.SelectedIndex > 0)
                    {
                        intSCountryId = Convert.ToInt64(ddlSCountry.SelectedValue);
                    }
                    if (ddlSState.SelectedIndex > 0)
                    {
                        intSStateId = Convert.ToInt64(ddlSState.SelectedValue);
                    }
                    if (ddlSCity.SelectedIndex > 0)
                    {
                        intSCityId = Convert.ToInt64(ddlSCity.SelectedValue);
                    }
                }

            IssuanceCompanyAddress objnewaddress1 = new IssuanceCompanyAddress();
            IssuanceCompanyAddress objnewaddress = new IssuanceCompanyAddress();
            if (this.UniformIssuancePolicyID != 0)
            {
                objnewaddress = objNewAddressRepos.GetByIssuanceId(this.UniformIssuancePolicyID);
            }

            if (objnewaddress != null)
            {
                //Billing
                objnewaddress.UniformIssuancePolicyID = UniformIssuancePolicyID;
                objnewaddress.FirstName = strFName;
                objnewaddress.LastName = strLName;
                objnewaddress.Address1 = strAddress1;
                objnewaddress.Address2 = strAddress2;
                objnewaddress.CompanyId = CompanyId;
                objnewaddress.Email = strEmail;
                objnewaddress.Telephone = strTelePhone;
                objnewaddress.Mobile = strMobile;
                objnewaddress.CountryId = intCountryId;
                objnewaddress.StateId = intStateId;
                objnewaddress.CityId = intCityId;
                objnewaddress.ZipCode = strZipCode;

                //Shipping
                objnewaddress.FirstSName = strSFName;
                objnewaddress.LastSName = strSLName;
                objnewaddress.SAddress1 = strSAddress1;
                objnewaddress.SAddress2 = strSAddress2;
                objnewaddress.SCompanyId = SCompanyId;
                objnewaddress.SEmail = strSEmail;
                objnewaddress.STelephone = strSTelePhone;
                objnewaddress.SMobile = strSMobile;
                objnewaddress.SCountryId = intSCountryId;
                objnewaddress.SStateId = intSStateId;
                objnewaddress.SCityId = intSCityId;
                objnewaddress.SZipCode = strSZipCode;
                objnewaddress.SCompanyName = strCompanyName;
                objnewaddress.BCompanyName = strBCompanyName;  
            }
            else
            {

                
                //Billing
                objnewaddress1.UniformIssuancePolicyID = UniformIssuancePolicyID;
                objnewaddress1.FirstName = strFName;
                objnewaddress1.LastName = strLName;
                objnewaddress1.Address1 = strAddress1;
                objnewaddress1.Address2 = strAddress2;
                objnewaddress1.CompanyId = CompanyId;
                objnewaddress1.Email = strEmail;
                objnewaddress1.Telephone = strTelePhone;
                objnewaddress1.Mobile = strMobile;
                objnewaddress1.CountryId = intCountryId;
                objnewaddress1.StateId = intStateId;
                objnewaddress1.CityId = intCityId;
                objnewaddress1.ZipCode = strZipCode;

                //Shipping
                objnewaddress1.FirstSName = strSFName;
                objnewaddress1.LastSName = strSLName;
                objnewaddress1.SAddress1 = strSAddress1;
                objnewaddress1.SAddress2 = strSAddress2;
                objnewaddress1.SCompanyId = SCompanyId;
                objnewaddress1.SEmail = strSEmail;
                objnewaddress1.STelephone = strSTelePhone;
                objnewaddress1.SMobile = strSMobile;
                objnewaddress1.SCountryId = intSCountryId;
                objnewaddress1.SStateId = intSStateId;
                objnewaddress1.SCityId = intSCityId;
                objnewaddress1.SZipCode = strSZipCode;
                objnewaddress1.SCompanyName = strCompanyName;
                objnewaddress1.BCompanyName = strBCompanyName;



            }
            if (this.UniformIssuancePolicyID == 0)
            {
                if (objnewaddress == null)
                {
                    objNewAddressRepos.Insert(objnewaddress1);
                    objNewAddressRepos.SubmitChanges();
                    IssuanceCompanyAddressId = objnewaddress1.IssuanceCompanyAddressId;
                    lblMsg.Text = "Record saved successfully!";
                }
                else
                {
                    objNewAddressRepos.Insert(objnewaddress);
                    objNewAddressRepos.SubmitChanges();
                    IssuanceCompanyAddressId = objnewaddress.IssuanceCompanyAddressId;
                    lblMsg.Text = "Record saved successfully!";
                }
            }
            else
            {
                if (objnewaddress != null)
                {
                    objNewAddressRepos.SubmitChanges();
                    IssuanceCompanyAddressId = objnewaddress.IssuanceCompanyAddressId;
                    lblMsg.Text = "Record updated successfully!";
                }
                else
                {
                    objNewAddressRepos.Insert(objnewaddress1);
                    objNewAddressRepos.SubmitChanges();
                    IssuanceCompanyAddressId = objnewaddress1.IssuanceCompanyAddressId;
                    lblMsg.Text = "Record saved successfully!";
                }
                
            }
            DisplayData(sender, e);
           // ClrData();
           // Response.Redirect("UniformIssuanceStep2.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType + "&IssuanceCompanyAddressId=" + IssuanceCompanyAddressId + "&Programname=" + Programname + "&CompletePR=" + CompletePR + "&PriceLevel=" + PriceLevel + "&ShowPrice=" + ShowPrice + "&Pricefor=" + Pricefor + "&Department=" + Department + "&Workgroup=" + Workgroup + "&PolicyDate=" + PolicyDate + "&ShowBeforeAfter=" + ShowBeforeAfter + "&ShowShippingPayment=" + ShowShippingPayment + "&gender=" + Gender);
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in adding record in list.";
            ErrHandler.WriteError(ex);
        }

    }
    #region Drop downs
    public void FillBCountry()
    {
        try
        {
            ds = objCountry.GetCountry();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DrpBillingCountry.DataSource = ds;
                DrpBillingCountry.DataTextField = "sCountryName";
                DrpBillingCountry.DataValueField = "iCountryID";
                DrpBillingCountry.DataBind();
                DrpBillingCountry.Items.Insert(0, new ListItem("-Select-", "0"));
                // DrpBillingCountry.SelectedValue = DrpBillingCountry.Items.FindByText("United States").Value;
                // DrpBillingCountry.Enabled = true;
                ds = objState.GetStateByCountryID(Convert.ToInt32(DrpBillingCountry.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DrpBillingState.DataSource = ds;
                        DrpBillingState.DataValueField = "iStateID";
                        DrpBillingState.DataTextField = "sStateName";
                        DrpBillingState.DataBind();
                        DrpBillingState.Items.Insert(0, new ListItem("-Select-", "0"));

                        DrpBillingCity.Items.Clear();
                        DrpBillingCity.Items.Add(new ListItem("-Select-", "0"));
                    }
                    else
                    {
                        DrpBillingState.Items.Insert(0, new ListItem("-Select-", "0"));
                        DrpBillingCity.Items.Add(new ListItem("-Select-", "0"));
                    }
                }
                else
                {
                    DrpBillingState.Items.Insert(0, new ListItem("-Select-", "0"));
                    DrpBillingCity.Items.Add(new ListItem("-Select-", "0"));
                }


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
        finally
        {

        }
    }
    protected void DrpBillingCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(DrpBillingCountry.SelectedValue));

        DrpBillingState.Items.Clear();
        DrpBillingCity.Items.Clear();
        DrpBillingCity.Items.Insert(0, new ListItem("-Select-", "0"));
        Common.BindDDL(DrpBillingState, objStateList, "sStatename", "iStateID", "-Select-");
    }
    protected void DrpBillingState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(DrpBillingState.SelectedValue));
        DrpBillingCity.Items.Clear();
        Common.BindDDL(DrpBillingCity, objCityList, "sCityName", "iCityID", "-Select-");
    }
    public void FillSCountry()
    {
        try
        {
            ds = objCountry.GetCountry();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSCountry.DataSource = ds;
                ddlSCountry.DataTextField = "sCountryName";
                ddlSCountry.DataValueField = "iCountryID";
                ddlSCountry.DataBind();
                ddlSCountry.Items.Insert(0, new ListItem("-Select-", "0"));
                // ddlSCountry.SelectedValue = ddlSCountry.Items.FindByText("United States").Value;
                // ddlSCountry.Enabled = true;
                ds = objState.GetStateByCountryID(Convert.ToInt32(ddlSCountry.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlSState.DataSource = ds;
                        ddlSState.DataValueField = "iStateID";
                        ddlSState.DataTextField = "sStateName";
                        ddlSState.DataBind();
                        ddlSState.Items.Insert(0, new ListItem("-Select-", "0"));

                        ddlSCity.Items.Clear();
                        ddlSCity.Items.Add(new ListItem("-Select-", "0"));
                    }
                    else
                    {
                        ddlSState.Items.Insert(0, new ListItem("-Select-", "0"));
                        ddlSCity.Items.Add(new ListItem("-Select-", "0"));
                    }
                }
                else
                {
                    ddlSState.Items.Insert(0, new ListItem("-Select-", "0"));
                    ddlSCity.Items.Add(new ListItem("-Select-", "0"));
                }

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
        finally
        {

        }
    }
    protected void ddlSCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(ddlSCountry.SelectedValue));

        ddlSState.Items.Clear();
        ddlSCity.Items.Clear();
        ddlSCity.Items.Insert(0, new ListItem("-Select-", "0"));
        Common.BindDDL(ddlSState, objStateList, "sStatename", "iStateID", "-Select-");
    }
    protected void ddlSState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(ddlSState.SelectedValue));
        ddlSCity.Items.Clear();
        Common.BindDDL(ddlSCity, objCityList, "sCityName", "iCityID", "-Select-");
    }
    #endregion
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        Response.Redirect("UniformIssuanceStep2.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType);
    }
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        Response.Redirect("UniformIssuanceStep1.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID);
       
    }
}
