using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Configuration;
using System.Text;
using System.Globalization;

public partial class UserFrameItems_BasicInfo : System.Web.UI.Page
{
    #region Page Variable's
    Int64 RegistrationID
    {
        get { return Convert.ToInt64(ViewState["RegistrationID"]); }
        set { this.ViewState["RegistrationID"] = value; }
    }
    #endregion 

    #region Page Event's
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["RegisID"]))
            {
                RegistrationID = Convert.ToInt64(Request.QueryString["RegisID"]);
                BindDropDown();
                PopulateData(RegistrationID);
            }
        }

    }

    protected void lnkSaveAndApprove_Click(object sender, EventArgs e)
    {
        String msg = String.Empty;
        try
        {
            SaveData();
            Boolean IsthirdParty = false;
            if (!String.IsNullOrEmpty(ddlCompany.SelectedValue) && ddlCompany.SelectedValue != "0")
                IsthirdParty = true;//chkThirdPartySupplier.Checked;
            msg = new WSUser().ApproveUser(Convert.ToString(RegistrationID));

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "window.parent.GeneralAlertMsg('" + msg + "');", true);
    }

    protected void lnkSaveRegistration_Click(object sender, EventArgs e)
    {
        try
        {
            SaveData();
            this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "CloseUserInfoPopup(true);", true);
            ResetControl();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "CloseUserInfoPopup(false);", true);
        }
    }
    #endregion 
    #region Page Method's
    /// <summary>
    /// Set User Info
    /// </summary>
    /// <param name="RegisID"></param>
    private void PopulateData(Int64 RegisID)
    {
        GetPendingUsersByRegistrationIDResult objUser = new RegistrationRepository().GetPendingUserByRegistrationID(RegisID);
        if (objUser != null)
        {
            txtFirstName.Text = objUser.FirstName;
            txtLastName.Text = objUser.LastName;
            txtCompany.Text = objUser.CompanyName;
            txtEmployeeID.Text = objUser.EmployeeID;
            ddlWorkGroup.SelectedValue = Convert.ToString(objUser.WorkGroupID);
            ddlBaseStation.SelectedValue = Convert.ToString(objUser.BaseStationID);
            ddlGender.SelectedValue = Convert.ToString(objUser.GenderID);
            ddlEmployeeTitle.SelectedValue = Convert.ToString(objUser.EmployeeTitleID);
            //txtBaseStation.Text = objUser.BaseStation;
            //txtWorkgroup.Text = objUser.WorkGroup;
            if (objUser.HireDate.HasValue)
            {
                DateTime doh = Convert.ToDateTime(objUser.HireDate);
                txtHireDate.Text = doh.ToShortDateString();
            }
            //txtGender.Text = objUser.Gender;
            txtEmailAddress.Text = objUser.EmailAddress;
            txtPwdRegistration.Text = objUser.UserPassword;
        }
    }
    /// <summary>
    /// Save Data 
    /// </summary>
    private void SaveData()
    {
        Int64 CompanyID = new CompanyStoreRepository().GetCompanyIDByName(Convert.ToString(txtCompany.Text));
        String DateHire = String.Empty;
        DateTime hireDate = Convert.ToDateTime(txtHireDate.Text);

        RegistrationRepository objRegRepo = new RegistrationRepository();
        Inc_Registration objRegis = objRegRepo.GetByRegistrationID(this.RegistrationID);
        objRegis.iCompanyName = Convert.ToInt64(CompanyID);
        objRegis.sFirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFirstName.Text.Trim().ToLower());
        objRegis.sLastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtLastName.Text.Trim().ToLower()); 
        objRegis.sEmailAddress = txtEmailAddress.Text.Trim();
        objRegis.iWorkgroupId = Int64.Parse(ddlWorkGroup.SelectedValue);
        objRegis.iBasestationId = Int64.Parse(ddlBaseStation.SelectedValue);
        objRegis.iGender = Int32.Parse(ddlGender.SelectedValue);
        Int64 empTitleID = Convert.ToInt64(ddlEmployeeTitle.SelectedValue);
        if (empTitleID > 0)
            objRegis.EmployeeTitleID = empTitleID;
        else
            objRegis.EmployeeTitleID = null;
        //objRegis.status = "pending";
        objRegis.DateRequestSubmitted = System.DateTime.Now;
        objRegis.DOH = hireDate;
        objRegis.sEmployeeId = Convert.ToString(txtEmployeeID.Text);
        objRegis.Password = Convert.ToString(txtPwdRegistration.Text);
        objRegRepo.SubmitChanges();
        
    }
    private void ResetControl()
    {
        txtCompany.Text = String.Empty;
        txtLastName.Text = String.Empty;
        txtFirstName.Text = String.Empty;
        ddlWorkGroup.SelectedIndex = 0;
        ddlBaseStation.SelectedIndex = 0;
        ddlGender.SelectedIndex = 0;
        ddlEmployeeTitle.SelectedIndex = 0;
        txtEmployeeID.Text = String.Empty;
        txtEmailAddress.Text = String.Empty;
        txtPwdRegistration.Text = String.Empty;

    }
    //bind dropdown
    public void BindDropDown()
    {
        // For base satations
        //CompanyRepository objRepo = new CompanyRepository();
        List<INC_BasedStation> objbsList = new CompanyRepository().GetAllBaseStationResult().OrderBy(b => b.sBaseStation).ToList();
        Common.BindDDL(ddlBaseStation, objbsList, "sBaseStation", "iBaseStationId", "-Select-");

        LookupRepository objLookRep = new LookupRepository();
        //For gender
        ddlGender.DataSource = objLookRep.GetByLookup("Gender");
        ddlGender.DataValueField = "iLookupID";
        ddlGender.DataTextField = "sLookupName";
        ddlGender.DataBind();
        ddlGender.Items.Insert(0, new ListItem("-Select-", "0"));
        // For Workgroup
        ddlWorkGroup.DataSource = objLookRep.GetByLookup("Workgroup");
        ddlWorkGroup.DataValueField = "iLookupID";
        ddlWorkGroup.DataTextField = "sLookupName";
        ddlWorkGroup.DataBind();
        ddlWorkGroup.Items.Insert(0, new ListItem("-Select-", "0"));

        //For Employee Title
        ddlEmployeeTitle.DataSource = objLookRep.GetByLookup("EmployeeTitle");
        ddlEmployeeTitle.DataValueField = "iLookupID";
        ddlEmployeeTitle.DataTextField = "sLookupName";
        ddlEmployeeTitle.DataBind();
        ddlEmployeeTitle.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Company 
        ddlCompany.DataSource = new CompanyStoreRepository().GetCompanyStore().OrderBy(le => le.Company).ToList();
        ddlCompany.DataValueField = "CompanyId";
        ddlCompany.DataTextField = "Company";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("-Select-", "0"));

    }
    #endregion 
}
