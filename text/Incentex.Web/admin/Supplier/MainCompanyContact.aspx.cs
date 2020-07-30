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
using Incentex.DAL.Common;
using System.Collections.Generic;


public partial class admin_Supplier_MainCompanyContact : PageBase
{
    #region Properties

    Int64 SupplierId
    {
        get
        {
            if (ViewState["SupplierId"] == null)
            {
                ViewState["SupplierId"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierId"]);
        }
        set
        {
            ViewState["SupplierId"] = value;
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
            base.MenuItem = "Main Company Contact";
            base.ParentMenuID = 18;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Main Company Contact";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to Supplier listing</span>";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Supplier/ViewSupplier.aspx";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = !String.IsNullOrEmpty(Convert.ToString(Request.UrlReferrer)) ? Convert.ToString(Request.UrlReferrer) : "~/Admin/index.aspx";
          
            if (Request.QueryString.Count > 0)
            {
                this.SupplierId = Convert.ToInt64(Request.QueryString.Get("Id"));
                manuControl.PopulateMenu(0, 0, this.SupplierId, 0,false);
            }
            else
            {
                Response.Redirect("~/admin/Supplier/ViewSupplier.aspx");
            }

            Common.BindCountry(ddlCountry);
            ddlCountry.SelectedValue = ddlCountry.Items.FindByText("United States").Value;

            ddlCountry_SelectedIndexChanged(sender, e);

            //lst.DataBind();
            //BindCompany();
            DisplayData();
            BindSupplierClassification();
        }
    }

    //void BindCompany()
    //{
    //    CompanyRepository objRepo = new CompanyRepository();
    //    List<Company> objList = objRepo.GetAllCompany();
    //    Common.BindDDL(ddlCompany, objList , "CompanyName", "CompanyID", "..-select company-..");

        
    //}

    void BindSupplierClassification()
    {
        LookupRepository objRepo = new LookupRepository();
        List<INC_Lookup> objList = objRepo.GetByLookupCode(DAEnums.LookupCodeType.SupplierClassifications);

        Common.BindDDL(ddlSupplierClassifications,objList, "sLookupName", "iLookupID", "-select a classification-");

    }

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindState(ddlState, Convert.ToInt64(ddlCountry.SelectedValue));
        ddlState_SelectedIndexChanged(sender, e);

    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindCity(ddlCity, Convert.ToInt64(ddlState.SelectedValue));
    }


    void DisplayData()
    {
        SupplierRepository objRepo = new SupplierRepository();
        Supplier objSupplier = objRepo.GetById(this.SupplierId);

        UserInformationRepository objUserInfoRepo = new UserInformationRepository();
          
    

        if(objSupplier != null)
        {
            
            //display supplier info
           // ddlCompany.SelectedValue = objSupplier.CompanyId.ToString();
            txtCompanyName.Text = objSupplier.CompanyName;
            txtCompanyWebsite.Text = objSupplier.CompanyWebsite;
            txtDepartment.Text = objSupplier.Department;
            txtSupplierSetupDate.Text = Common.GetDateString(objSupplier.SupplierSetupDate);
            ddlSupplierClassifications.SelectedValue = objSupplier.SupplierClassificationID.ToString();


            if(objSupplier.AccessToWorldLinkTrackingCenter == (int)DAEnums.YesNoType.Yes)
            {
                spchkAccessToWorldLinkTrackingCenter.Attributes.Add("class", "custom-checkbox_checked alignleft");
                chkAccessToWorldLinkTrackingCenter.Checked = true;
            }

            if (objSupplier.AccessToEditOrMakeChangesToPurchaseOrders == (int)DAEnums.YesNoType.Yes)
            {
                spchkAccessToPurchaseOrders.Attributes.Add("class","custom-checkbox_checked alignleft");
                chkAccessToPurchaseOrders.Checked = true;
            }


            //display user info
            UserInformation objUserInfo = objUserInfoRepo.GetById( objSupplier.UserInfoID);
            hdnUserInfoID.Value = Convert.ToString(objUserInfo.UserInfoID);

            ViewState["LoginEmail"] = objUserInfo.LoginEmail;

            this.UserInfoID = objUserInfo.UserInfoID;
            txtFirstName.Text = objUserInfo.FirstName;
            txtLastName.Text = objUserInfo.LastName;
            txtTitle.Text = objUserInfo.Title;
            txtAddress.Text = objUserInfo.Address1;
            ddlCountry.SelectedValue = objUserInfo.CountryId.ToString();

            Common.BindState(ddlState, (objUserInfo.CountryId.Value == null ? -1 : objUserInfo.CountryId.Value));
            ddlState.SelectedValue = objUserInfo.StateId.ToString();

            Common.BindCity(ddlCity, (objUserInfo.StateId.Value == null ? -1 : objUserInfo.StateId.Value));
            ddlCity.SelectedValue = objUserInfo.CityId.ToString(); ;

            txtZip.Text = objUserInfo.ZipCode;
            txtTelephone.Text = objUserInfo.Telephone;
            txtExtension.Text = objUserInfo.Extension;
            txtFax.Text = objUserInfo.Fax;
            txtMobile.Text = objUserInfo.Mobile;
            txtEmail.Text = objUserInfo.Email;
            txtSkypeName.Text = objUserInfo.SkypeName;

            txtUserName.Text = objUserInfo.LoginEmail;
            txtPassword.Text = objUserInfo.Password;
            //ddlCompany.SelectedValue = objUserInfo.CompanyId.ToString();
        }
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            //add or edit record

            // save in UserInformation table
            UserInformationRepository objUserInfoRepo = new UserInformationRepository();
            UserInformation objUserInfo = new UserInformation();
            if (Convert.ToString(ViewState["LoginEmail"]) != txtUserName.Text)
            {
                if (!objUserInfoRepo.CheckEmailExistence(txtUserName.Text, this.UserInfoID))
                {
                    lblMsg.Text = "Login Email already exist ...";
                    return;
                }
            }

            if(this.UserInfoID != 0)
            {
                objUserInfo = objUserInfoRepo.GetById(this.UserInfoID);
            }

            objUserInfo.FirstName = txtFirstName.Text;
            objUserInfo.LastName = txtLastName.Text;
            objUserInfo.Title = txtTitle.Text;
            objUserInfo.Address1 = txtAddress.Text;
            objUserInfo.CountryId = Convert.ToInt64(ddlCountry.SelectedValue);
            objUserInfo.StateId = Convert.ToInt64(ddlState.SelectedValue);
            objUserInfo.CityId = Convert.ToInt64(ddlCity.SelectedValue);
            objUserInfo.ZipCode = txtZip.Text;
            objUserInfo.Telephone = txtTelephone.Text;
            objUserInfo.Extension = txtExtension.Text;
            objUserInfo.Fax = txtFax.Text;
            objUserInfo.Mobile = txtMobile.Text;
            objUserInfo.Email = txtEmail.Text;
            objUserInfo.SkypeName = txtSkypeName.Text;
            objUserInfo.Usertype = (Int64)DAEnums.UserTypes.Supplier;
            objUserInfo.LoginEmail = txtUserName.Text;
            objUserInfo.Password = txtPassword.Text;
          //  objUserInfo.CompanyId = Convert.ToInt64(ddlCompany.SelectedValue);
            objUserInfo.CreatedDate = DateTime.Now;
            objUserInfo.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;

            if(this.UserInfoID == 0)
            {
                objUserInfoRepo.Insert(objUserInfo);
            }
            objUserInfoRepo.SubmitChanges();
            ViewState["LoginEmail"] = txtUserName.Text;
            this.UserInfoID = objUserInfo.UserInfoID;

            //save in supplier table

            SupplierRepository objSupplierRepo = new SupplierRepository(); 
            Supplier objSupplier = new Supplier(); 
            
            if(this.SupplierId != 0)
            {
                objSupplier = objSupplierRepo.GetById(this.SupplierId);
            }

            //objSupplier

            objSupplier.UserInfoID = objUserInfo.UserInfoID;
            objSupplier.CompanyName = txtCompanyName.Text;
            //objSupplier.CompanyID =  Convert.ToInt64(ddlCompany.SelectedValue);
            objSupplier.Department = txtDepartment.Text;
            objSupplier.CompanyWebsite = txtCompanyWebsite.Text;
            objSupplier.SupplierSetupDate = Common.GetDate(txtSupplierSetupDate);
            objSupplier.SupplierClassificationID = Convert.ToInt64(ddlSupplierClassifications.SelectedValue);


            if (chkAccessToPurchaseOrders.Checked)
            {
                objSupplier.AccessToEditOrMakeChangesToPurchaseOrders = (Int16)DAEnums.YesNoType.Yes;
            }
            else
            {
                objSupplier.AccessToEditOrMakeChangesToPurchaseOrders = (Int16)DAEnums.YesNoType.No;
            }

            if (chkAccessToWorldLinkTrackingCenter.Checked)
            {
                objSupplier.AccessToWorldLinkTrackingCenter = (Int16)DAEnums.YesNoType.Yes;
            }
            else
            {
                objSupplier.AccessToWorldLinkTrackingCenter = (Int16)DAEnums.YesNoType.No;
            }

            if(this.SupplierId == 0)
            {
                objSupplierRepo.Insert(objSupplier);
            }

            objSupplierRepo.SubmitChanges();
            this.SupplierId = objSupplier.SupplierID;


            lblMsg.Text = "Record saved successfully ...";
            Response.Redirect("BankOrGeneralQuality.aspx?Id=" + this.SupplierId,false);
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

    
    //protected void lst_DataBinding(object sender, EventArgs e)
    //{
    //    LookupRepository objRepo = new LookupRepository();
    //    List<INC_Lookup> objList = objRepo.GetByLookupCode(DAEnums.LookupCodeType.SupplierClassifications);
    //    lst.DataSource = objList;
    //}
}
