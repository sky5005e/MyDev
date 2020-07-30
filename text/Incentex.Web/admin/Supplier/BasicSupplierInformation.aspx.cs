using System;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;
using Incentex.DA;
using Incentex.BE;

public partial class admin_Supplier_BasicSupplierInformation : PageBase
{

    #region Properties
    Int64 NoteId
    {
        get
        {
            if (ViewState["NoteId"] == null)
            {
                ViewState["NoteId"] = 0;
            }
            return Convert.ToInt64(ViewState["NoteId"]);
        }
        set
        {
            ViewState["NoteId"] = value;
        }
    }
    Int64 SupplierID
    {
        get
        {
            if (ViewState["SupplierID"] == null)
            {
                ViewState["SupplierID"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierID"]);
        }
        set
        {
            ViewState["SupplierID"] = value;
        }
    }
    Int64 SupplierEmployeeID
    {
        get
        {
            if (ViewState["SupplierEmployeeID"] == null)
            {
                ViewState["SupplierEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierEmployeeID"]);
        }
        set
        {
            ViewState["SupplierEmployeeID"] = value;
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
    SupplierRepository objSupplierRepos = new SupplierRepository();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Employee";
            base.ParentMenuID = 18;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            Common.MenuModuleId = Incentex.DAL.Common.DAEnums.ManageID.ManageSupplier;
            if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString["SubId"] != null)
                {
                    this.SupplierEmployeeID = Convert.ToInt64(Request.QueryString.Get("SubId"));
                }
                if (Request.QueryString["id"] != "0")
                {
                    this.SupplierID = Convert.ToInt64(Request.QueryString.Get("id"));
                    txtCompany.Text = Convert.ToString(objSupplierRepos.GetById(this.SupplierID).CompanyName);
                }
                ((Label)Master.FindControl("lblPageHeading")).Text = "Supplier Employee Basic Information";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Supplier/ViewAddSupplierEmployee.aspx?id=" + this.SupplierID;

                menuControl.PopulateMenu(3, 0, this.SupplierID, this.SupplierEmployeeID, true);
            } 
            else
            {
                Response.Redirect("ViewAddSupplierEmployee.aspx");
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
        if (this.SupplierEmployeeID != 0)
        {
            SupplierEmployeeRepository objRepo = new SupplierEmployeeRepository();
            SupplierEmployee obj = objRepo.GetById(this.SupplierEmployeeID);

            if (obj != null)
            {
                //Employee data
                if (obj.isDirectEmployee == 1)
                {
                   
                    ddlEmpType.SelectedValue = "1";
                }
                else
                {
                    ddlEmpType.SelectedValue = "0";
                }

                // user data
                UserInformationRepository objUserInfoRepo = new UserInformationRepository();
                UserInformation objUserInfo = objUserInfoRepo.GetById(obj.UserInfoID);
                this.UserInfoID = Convert.ToInt64(obj.UserInfoID);
                txtFirstName.Text = objUserInfo.FirstName;
                txtLastName.Text = objUserInfo.LastName;
                txtTitle.Text = objUserInfo.Title;
                txtAddress.Text = objUserInfo.Address1;
                ddlCountry.SelectedValue = objUserInfo.CountryId.ToString();
                Common.BindState(ddlState, (objUserInfo.CountryId.Value == null ? -1 :objUserInfo.CountryId.Value ));
                ddlState.SelectedValue = objUserInfo.StateId.ToString();
                Common.BindCity(ddlCity, (objUserInfo.StateId.Value == null ? -1 : objUserInfo.StateId.Value));
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
               
                if (objUserInfo.WLSStatusId != null)
                {
                    ddlStatus.SelectedValue = objUserInfo.WLSStatusId.ToString();
                }

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

            //save in User info
            UserInformationRepository objUserInfoRepo = new UserInformationRepository();

            if (objUserInfoRepo.CheckUniqueEmail(this.UserInfoID, txtLogInEmail.Text))
            {
                lblMsg.Text = "Login Email already exist ...";
                return;
            }

            UserInformation objUserInfo = new UserInformation();

            if (this.UserInfoID != 0)
            {
                objUserInfo = objUserInfoRepo.GetById(this.UserInfoID);
            }
            objUserInfo.FirstName = txtFirstName.Text;
            objUserInfo.LastName = txtLastName.Text;
            objUserInfo.Title = txtTitle.Text;
            objUserInfo.Address1 = txtAddress.Text;
            objUserInfo.CountryId = Convert.ToInt64(ddlCountry.SelectedValue);
            objUserInfo.StateId = Convert.ToInt64(ddlState.SelectedValue);
            objUserInfo.CityId = CityID;
            objUserInfo.ZipCode = txtZip.Text;
            objUserInfo.Telephone = txtTelephone.Text;
            objUserInfo.Extension = txtExtension.Text;
            objUserInfo.Fax = txtFax.Text;
            objUserInfo.Mobile = txtMobile.Text;
            objUserInfo.Email = txtEmail.Text;
            objUserInfo.SkypeName = txtSkypeName.Text;
            objUserInfo.Usertype = (Int64)Incentex.DAL.Common.DAEnums.UserTypes.SupplierEmployee;
            objUserInfo.LoginEmail = txtLogInEmail.Text;
            objUserInfo.Password = txtPassword.Text;
            objUserInfo.WLSStatusId = Convert.ToInt64(ddlStatus.SelectedValue);
            objUserInfo.CreatedDate = DateTime.Now;
            objUserInfo.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;

            if (this.UserInfoID == 0)
            {
                objUserInfoRepo.Insert(objUserInfo);
            }

            objUserInfoRepo.SubmitChanges();
            this.UserInfoID = objUserInfo.UserInfoID;
            
            //save in SupplierEmployee
            SupplierEmployee obj = new SupplierEmployee();
            SupplierEmployeeRepository objRepo = new SupplierEmployeeRepository();

            if (this.SupplierEmployeeID != 0)
            {
                obj = objRepo.GetById(this.SupplierEmployeeID);
            }
            obj.SupplierID = this.SupplierID;
            obj.UserInfoID = this.UserInfoID;
            obj.MemberRole = "Supplier Employee";
            //if (chkIsDirectEmployee.Checked)
            if (ddlEmpType.SelectedValue.Equals("1"))
            {
                obj.isDirectEmployee = 1;
            }
            else
            {
                obj.isDirectEmployee = 0;
            }

            if (this.SupplierEmployeeID == 0)
            {
                objRepo.Insert(obj);
            }

            objRepo.SubmitChanges();
            this.SupplierEmployeeID = obj.SupplierEmployeeID;

            lblMsg.Text = "Record Saved Sucessfully ...";
            //Response.Redirect("Documents.aspx?Id=" + this.SupplierEmployeeID);
            Response.Redirect("SupplierDocuments.aspx?id=" + Convert.ToInt32(this.SupplierID.ToString()) + "&SubId=" + Convert.ToInt32(this.SupplierEmployeeID.ToString()));
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
        {
            PnlCityOther.Visible = true;

        }
        else
        {
            PnlCityOther.Visible = false;
        }
    }
}
