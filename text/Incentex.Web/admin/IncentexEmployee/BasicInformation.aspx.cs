using System;
using System.Web.Services;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_IncentexEmployee_BasicInformation : PageBase
{
    #region Properties

    Int64 IncentexEmployeeID
    {
        get
        {
            if (ViewState["IncentexEmployeeID"] == null)
            {
                ViewState["IncentexEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["IncentexEmployeeID"]);
        }
        set
        {
            ViewState["IncentexEmployeeID"] = value;
        }
    }

    Int64 UserInfoID
    {
        get
        {
            if (ViewState["UserInfoID"] == null)
            {
                ViewState["UserInfoID"] = 0;
            }
            return Convert.ToInt64(ViewState["UserInfoID"]);
        }
        set
        {
            ViewState["UserInfoID"] = value;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Incentex Employee";
            base.ParentMenuID = 10;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Basic Information";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = !String.IsNullOrEmpty(Convert.ToString(Request.UrlReferrer)) ? Convert.ToString(Request.UrlReferrer) : "~/Admin/index.aspx";

            if (Request.QueryString.Count > 0)
            {
                this.IncentexEmployeeID = Convert.ToInt64(Request.QueryString.Get("Id"));
                manuControl.PopulateMenu(0, 0, this.IncentexEmployeeID, 0, false);
            }
            else
            {
                Response.Redirect("ViewIncentexEmployee.aspx");
            }
            BindDDl();
            DisplayData();
        }


    }

    /// <summary>
    /// Bind dropdown list
    /// </summary>
    void BindDDl()
    {
        //bind country dropdown
        Common.BindCountry(ddlCountry);

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

    /// <summary>
    /// Display data to edit
    /// </summary>
    void DisplayData()
    {
        if (this.IncentexEmployeeID != 0)
        {
            IncentexEmployeeRepository objRepo = new IncentexEmployeeRepository();
            IncentexEmployee obj = objRepo.GetById(this.IncentexEmployeeID);

            if (obj != null)
            {
                //Employee data
                if (obj.isDirectEmployee == 1)
                {
                    // chkIsDirectEmployee.Checked = true;
                    //  spChksDirectEmployee.Attributes.Add("class","custom-checkbox_checked alignleft");
                    ddlEmpType.SelectedValue = "1";
                }
                else
                {
                    ddlEmpType.SelectedValue = "0";
                }

                // user data
                UserInformationRepository objUserInfoRepo = new UserInformationRepository();
                UserInformation objUserInfo = objUserInfoRepo.GetById(obj.UserInfoID);
                hdnUserInfoID.Value = Convert.ToString(objUserInfo.UserInfoID);

                this.UserInfoID = obj.UserInfoID;

                txtFirstName.Text = objUserInfo.FirstName;
                txtLastName.Text = objUserInfo.LastName;
                txtTitle.Text = objUserInfo.Title;
                txtAddress.Text = objUserInfo.Address1;
                ddlCountry.SelectedValue = objUserInfo.CountryId.ToString();

                Common.BindState(ddlState, (objUserInfo.CountryId.Value == null ? -1 : objUserInfo.CountryId.Value));
                ddlState.SelectedValue = objUserInfo.StateId.ToString();

                Common.BindCity(ddlCity, (objUserInfo.StateId.Value == null ? -1 : objUserInfo.StateId.Value));//objUserInfo.StateId);
                ddlCity.SelectedValue = objUserInfo.CityId.ToString();
                ddlCity.Items.Insert(1, "-Other-");
                txtZip.Text = objUserInfo.ZipCode;
                txtTelephone.Text = objUserInfo.Telephone;
                txtExtension.Text = objUserInfo.Extension;
                txtFax.Text = objUserInfo.Fax;
                txtMobile.Text = objUserInfo.Mobile;
                txtEmail.Text = objUserInfo.Email;
                txtSkypeName.Text = objUserInfo.SkypeName;

                txtLogInEmail.Text = objUserInfo.LoginEmail;
                txtPassword.Text = objUserInfo.Password;
                //txtPassword.Attributes.Add("value", objUserInfo.Password);

                if (objUserInfo.WLSStatusId != null)
                    ddlStatus.SelectedValue = objUserInfo.WLSStatusId.ToString();

            }
        }
    }


    /// <summary>
    /// Bind Country
    /// Amit 20-Sep-2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindState(ddlState, Convert.ToInt64(ddlCountry.SelectedValue));
        ddlState_SelectedIndexChanged(sender, e);
        if (ddlCountry.SelectedValue == "0" && ddlState.SelectedValue == "0")
        {
            ddlCity.Items.Remove(new ListItem("-Other-", "-Other-"));

        }


    }

    /// <summary>
    /// Bind City
    /// Amit 20-Sep-2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindCity(ddlCity, Convert.ToInt64(ddlState.SelectedValue));
        if (ddlState.SelectedIndex > 0)
        {

            ddlCity.Items.Insert(1, "-Other-");
        }

    }

    /// <summary>
    /// Add \ Update record
    /// Amit 20-Sep-2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            lblMsg.Text = "";

            //save in User info
            UserInformationRepository objUserInfoRepo = new UserInformationRepository();

            if (objUserInfoRepo.CheckUniqueEmail(this.UserInfoID, txtLogInEmail.Text))
            {
                lblMsg.Text = "Login Email already exist ...";
                return;
            }

            UserInformation objUserInfo = new UserInformation();

            if (this.UserInfoID != 0)
                objUserInfo = objUserInfoRepo.GetById(this.UserInfoID);

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
            //End
            else
                CityID = Convert.ToInt64(ddlCity.SelectedValue);
            //End

            objUserInfo.FirstName = txtFirstName.Text.Trim();
            objUserInfo.LastName = txtLastName.Text.Trim();
            objUserInfo.Title = txtTitle.Text.Trim();
            objUserInfo.Address1 = txtAddress.Text.Trim();
            objUserInfo.CountryId = Convert.ToInt64(ddlCountry.SelectedValue);
            objUserInfo.StateId = Convert.ToInt64(ddlState.SelectedValue);
            objUserInfo.CityId = CityID;
            objUserInfo.ZipCode = txtZip.Text.Trim();
            objUserInfo.Telephone = txtTelephone.Text.Trim();
            objUserInfo.Extension = txtExtension.Text.Trim();
            objUserInfo.Fax = txtFax.Text.Trim();
            objUserInfo.Mobile = txtMobile.Text.Trim();
            objUserInfo.Email = txtEmail.Text.Trim();
            objUserInfo.SkypeName = txtSkypeName.Text.Trim();
            objUserInfo.Usertype = (Int64)Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin;
            objUserInfo.LoginEmail = txtLogInEmail.Text.Trim();
            objUserInfo.Password = txtPassword.Text.Trim();
            objUserInfo.WLSStatusId = Convert.ToInt64(ddlStatus.SelectedValue);
            objUserInfo.CreatedDate = DateTime.Now;
            objUserInfo.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;

            if (this.UserInfoID == 0)
            {
                objUserInfoRepo.Insert(objUserInfo);
            }

            objUserInfoRepo.SubmitChanges();
            this.UserInfoID = objUserInfo.UserInfoID;

            //save in IncentexEmployee
            IncentexEmployee obj = new IncentexEmployee();
            IncentexEmployeeRepository objRepo = new IncentexEmployeeRepository();

            if (this.IncentexEmployeeID != 0)
            {
                obj = objRepo.GetById(this.IncentexEmployeeID);
            }

            obj.UserInfoID = this.UserInfoID;
            obj.MemberRole = "Incentex Employee";
            //if (chkIsDirectEmployee.Checked)
            if (ddlEmpType.SelectedValue.Equals("1"))
            {
                obj.isDirectEmployee = 1;
            }
            else
            {
                obj.isDirectEmployee = 0;
            }

            if (this.IncentexEmployeeID == 0)
            {
                objRepo.Insert(obj);
            }

            objRepo.SubmitChanges();
            this.IncentexEmployeeID = obj.IncentexEmployeeID;

            lblMsg.Text = "Record Saved Sucessfully ...";
            Response.Redirect("Documents.aspx?Id=" + this.IncentexEmployeeID);
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

    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCity.SelectedValue == "-Other-")
            PnlCityOther.Visible = true;
        else
            PnlCityOther.Visible = false;
    }

    [WebMethod]
    public static String CheckDuplicateEmail(String email, String userInfoID)
    {
        String methodResponse = String.Empty;

        try
        {
            if (!String.IsNullOrEmpty(email))
            {
                if (new UserInformationRepository().CheckEmailExistence(email, !String.IsNullOrEmpty(userInfoID) ? Convert.ToInt64(userInfoID) : 0))
                {
                    if (new RegistrationRepository().CheckEmailExistence(email, 0))
                        methodResponse = "0";
                    else
                        methodResponse = "2";
                }
                else
                    methodResponse = "1";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return methodResponse;
    }
}