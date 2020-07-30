using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class ProductReturnManagement_ShortReturns : PageBase
{
    #region Properties
    /// <summary>
    /// 
    /// </summary>
    Int64 OrderReturnId
    {
        get
        {
            if (ViewState["OrderReturnId"] == null)
            {
                ViewState["OrderReturnId"] = 0;
            }
            return Convert.ToInt64(ViewState["OrderReturnId"]);
        }
        set
        {
            ViewState["OrderReturnId"] = value;
        }
    }
    String OrderNumber
    {
        get
        {
            return ViewState["OrderNumber"].ToString();
        }
        set
        {
            ViewState["OrderNumber"] = value;
        }
    }
    /// <summary>
    /// To check quantity and items quantity are differ 
    /// </summary>
    Boolean IsQuantityDiffer
    {
        get
        {
            if (ViewState["IsQuantityDiffer"] == null)
            {
                ViewState["IsQuantityDiffer"] = false;
            }
            return (Boolean)ViewState["IsQuantityDiffer"];
        }
        set
        {
            ViewState["IsQuantityDiffer"] = value;
        }
    }
    StoreProductRepository objStoreProductRepository = new StoreProductRepository();
    StoreProductImageRepository objStoreProductImageRepository = new StoreProductImageRepository();
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfo();
    UserInformationRepository objUserInformationRepository = new UserInformationRepository();
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    IncentexBEDataContext db = new IncentexBEDataContext();
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization

    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ShowShortReturnPage();
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Shorts Return System";

            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/ProductReturnManagement/MYProductReturns.aspx";
            BindDataINC3032();
            BindDataINC3034();
            //Bind Country
            Common.BindCountry(drpCountry);
            drpCountry.SelectedValue = "223"; // for USA
            //Bind State
            Common.BindState(drpState, 223);
            FillShippingLocations();
        }
    }

    #endregion

    #region Page Methods
    /// <summary>
    ///  Check user type and Is user already request for change.
    /// </summary>
    private void ShowShortReturnPage()
    {
        CompanyEmployee objCmpnyInfo = new CompanyEmployee();
        CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
        objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

        IncentexBEDataContext db = new IncentexBEDataContext();
        var ShortReturnCount = db.ReturnShortProducts.Where(s => s.UserInfoID == IncentexGlobal.CurrentMember.UserInfoID).ToList();
        // when User type = CE, StoreID = Spirit and Main Work Group = FlightAttendent Group 
        // Then only Short Return system Link is visible 
        if ((IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee)
            && IncentexGlobal.CurrentMember.CompanyId == 3 && objCmpnyInfo.WorkgroupID == 51 && objCmpnyInfo.GenderID == 63)
            && ShortReturnCount.Count == 0)// here 51 is work group
        {
            // Allow user to access this page.
        }
        else
        {
            Response.Redirect("~/index.aspx");
        }
    }

    /// <summary>
    /// To get All product details 
    /// </summary>
    /// <param name="masterItemNo"></param>
    /// <param name="StoreProductId"></param>
    /// <returns></returns>
    List<SP_GetStoreProductResult> GetProducts(Int64 masterItemNo, Int64 StoreProductId)
    {
        List<SP_GetStoreProductResult> objList = new List<SP_GetStoreProductResult>();
        var qry = objStoreProductRepository.GetStoreProductItems(3, 51, (Int64)IncentexGlobal.CurrentMember.UserInfoID);
        qry = qry.Where(a => a.StoreProductID == StoreProductId && a.MasterItemNo == masterItemNo).ToList();
        objList = qry;
        return objList;
    }
    /// <summary>
    /// Bind data to sub items INC3034
    /// </summary>
    private void BindDataINC3034()
    {
        List<StoreProductImage> objStoreProductImage = new List<StoreProductImage>();
        objStoreProductImage = objStoreProductImageRepository.GetStoreProductImagesdByMasterIDAndProductIamgeisActive(373, 39); // ( 606,156 )
        // Display Image
        if (objStoreProductImage.Count > 0 && !String.IsNullOrEmpty(objStoreProductImage[0].ProductImage))
            imgINC3034.Src = "../UploadedImages/ProductImages/Thumbs/" + objStoreProductImage[0].ProductImage;

        else
            imgINC3034.Src = "../UploadedImages/ProductImages/ProductDefault.jpg";
        // For LNK PDF
        //For Link PDF
        lnkINC3034.NavigateUrl = "../UploadedImages/TailoringMeasurement/Women Skirt.pdf";
        // Display Data
        List<SP_GetStoreProductResult> objListINC3034 = GetProducts(373, 39);
        if (objListINC3034.Count > 0)
        {
            lblProductDescriptionINC3034.Text = objListINC3034[0].ProductDescrption.ToString();
            var SizeListINC3034 = (from sINC3034 in objListINC3034
                                   where sINC3034.MasterItemNo == 373 && sINC3034.StoreProductID == 39
                                   select new { sINC3034.ItemSizeID, sINC3034.ItemSize }).Distinct().ToList();

            Common.BindDDL(ddlSizeINC3034, SizeListINC3034, "ItemSize", "ItemSizeID", "-select-");
        }
    }

    /// <summary>
    /// Bind data to sub items INC3034
    /// </summary>
    private void BindDataINC3032()
    {
        List<StoreProductImage> objStoreProductImage = new List<StoreProductImage>();
        objStoreProductImage = objStoreProductImageRepository.GetStoreProductImagesdByMasterIDAndProductIamgeisActive(371, 37);//(604, 154);
        // Display Image
        if (objStoreProductImage.Count > 0 && !String.IsNullOrEmpty(objStoreProductImage[0].ProductImage))
            imgINC3032.Src = "../UploadedImages/ProductImages/Thumbs/" + objStoreProductImage[0].ProductImage;
        else
            imgINC3032.Src = "../UploadedImages/ProductImages/ProductDefault.jpg";
        //For Link PDF
        lnkINC3032.NavigateUrl = "../UploadedImages/TailoringMeasurement/Women Pants.pdf";

        // 
        List<SP_GetStoreProductResult> objListINC3032 = GetProducts(371, 37);
        // Display Descriptions
        if (objListINC3032.Count > 0)
        {
            lblDescriptionINC3032.Text = objListINC3032[0].ProductDescrption.ToString();
            //bind size
            var SizeListINC3032 = (from sINC3032 in objListINC3032
                                   where sINC3032.MasterItemNo == 371 && sINC3032.StoreProductID == 37
                                   select new { sINC3032.ItemSizeID, sINC3032.ItemSize }).Distinct().ToList();
            Common.BindDDL(ddlSizeINC3032, SizeListINC3032, "ItemSize", "ItemSizeID", "-select-");
        }

    }

    /// <summary>
    /// If there is difference between quantity and items quantity then display this error message
    /// </summary>
    private void SetErrorMessage()
    {
        Int32 qntyINC3032 = 0;
        if (!String.IsNullOrEmpty(txtQuantityINC3032.Text))
            qntyINC3032 = Convert.ToInt32(txtQuantityINC3032.Text);
        Int32 qntyINC3034 = 0;
        if (!String.IsNullOrEmpty(txtQuantityINC3034.Text))
            qntyINC3034 = Convert.ToInt32(txtQuantityINC3034.Text);
        Int32 qnty = Convert.ToInt32(txtQnty.Text);
        Int32 sumQuantity = qntyINC3032 + qntyINC3034;

        // when both qntyINC3032 == 0 and qntyINC3034 == 0
        if (qntyINC3032 == 0 && qntyINC3034 == 0)
        {
            lblError.Text = "please enter sub items";
            dvError.Visible = true;
            lblError.Visible = true;
            IsQuantityDiffer = true;
        }
        else
        {
            lblError.Visible = false;
            IsQuantityDiffer = false;
            dvError.Visible = false;
        }
    }

    /// <summary>
    /// Fills the Title for Mutiple Shipping Addresses
    /// </summary>
    private void FillShippingLocations()
    {
        List<CompanyEmployeeContactInfo> obj = objCmpEmpContRep.GetSavedShippingAddressesForUser(IncentexGlobal.CurrentMember.UserInfoID, CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Company, Incentex.DAL.Common.DAEnums.SortOrderType.Asc);
        ddlShippingLocations.DataSource = obj;
        ddlShippingLocations.DataBind();
        ddlShippingLocations.Items.Insert(0, new ListItem("--select--", "0"));
        ddlShippingLocations.SelectedIndex = 1;
        SetShippingAddress();
    }

    /// <summary>
    ///  To get Shipping Address
    /// </summary>
    protected void SetShippingAddress()
    {
        objShippingInfo = objCmpEmpContRep.GetShippingDetailById(IncentexGlobal.CurrentMember.UserInfoID, Int64.Parse(ddlShippingLocations.SelectedValue.ToString()));

        #region Shipping information
        if (ddlShippingLocations.SelectedIndex >= 0)
        {
            if (objShippingInfo != null)
            {
                //Bind Country
                Common.BindCountry(drpCountry);
                drpCountry.Items.FindByValue(objShippingInfo.CountryID.ToString()).Selected = true;

                //Bind State
                Common.BindState(drpState, (Int64)objShippingInfo.CountryID);
                drpState.Items.FindByValue(objShippingInfo.StateID.ToString()).Selected = true;

                //Bind City
                Common.BindCity(drpCity, (Int64)objShippingInfo.StateID);
                drpCity.Items.FindByValue(objShippingInfo.CityID.ToString()).Selected = true;

                txtZipcodeEdit.Text = objShippingInfo.ZipCode;
                txtShippingFnameEdit.Text = objShippingInfo.Name;
                txtShippingLnameEdit.Text = objShippingInfo.Fax;
                txtShippingCompanyNameEdit.Text = objShippingInfo.CompanyName;
                txtShippingAddressEdit.Text = objShippingInfo.Address;
                txtEmailAdrressEdit.Text = objShippingInfo.Email;
                txtPhoneNumberEdit.Text = objShippingInfo.Telephone;
                hdnShippingInfoID.Value = objShippingInfo.CompanyContactInfoID.ToString();
            }
        }
        else
        {
            hdnShippingInfoID.Value = "0";
        }

        #endregion
    }
    /// <summary>
    /// Clear fields
    /// </summary>
    private void clearShippingDetails()
    {
        hdnShippingInfoID.Value = "0";
        txtZipcodeEdit.Text = String.Empty;
        txtShippingFnameEdit.Text = String.Empty;
        txtShippingLnameEdit.Text = String.Empty;
        txtShippingCompanyNameEdit.Text = String.Empty;
        txtShippingAddressEdit.Text = String.Empty;
        txtEmailAdrressEdit.Text = String.Empty;
        txtPhoneNumberEdit.Text = String.Empty;
        drpCountry.SelectedIndex = 0;
        drpState.SelectedIndex = 0;
        drpCity.SelectedIndex = 0;
    }

    #endregion

    #region Page Control Events

    protected void btnProcessNow_Click(object sender, EventArgs e)
    {
        SetErrorMessage();
        // Create Order Number
        OrderNumber = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + IncentexGlobal.CurrentMember.UserInfoID.ToString();
        OrderReturnId = Convert.ToInt64(OrderNumber);
        if (!IsQuantityDiffer)
        {
            // Insert Record in  ReturnShortProduct Table
            try
            {
                //Save shipping information
                SaveShippingInfo();
                // Save Shorts Return Info 
                SaveReturnShortsInfo();
                // To user who had made request.
                sendVerificationEmail(OrderNumber);
                // To IE 
                sendIEEmail();
                // To WTDC
                sendVerificationEmailtoIE(1261, "WTDC Warehouse", "incentex@wtdc.com");
                // then Redirect
                Response.Redirect("ShortReturnThanks.aspx");
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
    }

    protected void ddlShippingLocations_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlShippingLocations.SelectedValue != "0")
        {
            objShippingInfo = objCmpEmpContRep.GetShippingDetailById(IncentexGlobal.CurrentMember.UserInfoID, Int64.Parse(ddlShippingLocations.SelectedValue.ToString()));
            SetShippingAddress();
        }
        else
        {
            txtZipcodeEdit.Text = String.Empty;
            txtShippingFnameEdit.Text = String.Empty;
            txtShippingLnameEdit.Text = String.Empty;
            txtShippingCompanyNameEdit.Text = "Spirit Airlines";
            txtShippingAddressEdit.Text = String.Empty;
            txtEmailAdrressEdit.Text = String.Empty;
            txtPhoneNumberEdit.Text = String.Empty;
            hdnShippingInfoID.Value = "0";
            txtAddressTitle.Text = String.Empty;

            Common.BindCountry(drpCountry);
            drpCountry.SelectedValue = "223"; // for USA
            //Bind State
            Common.BindState(drpState, 223);
            //Bind City
            drpCity.ClearSelection();
            Common.BindCity(drpCity, 0);
        }
    }

    protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindState(drpState, Convert.ToInt64(drpCountry.SelectedValue));
        drpState_SelectedIndexChanged(sender, e);
    }

    protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindCity(drpCity, Convert.ToInt64(drpState.SelectedValue));
    }

    protected void SaveShippingInfo()
    {
        CompanyEmployeeContactInfoRepository objCmpEmpShipping = new CompanyEmployeeContactInfoRepository();
        if (hdnShippingInfoID.Value != "0" && hdnShippingInfoID.Value != "")
        {
            objShippingInfo = objCmpEmpShipping.GetShippingDetailsByID((Convert.ToInt64(hdnShippingInfoID.Value.ToString())));
            //Edit Shipping details here
            objShippingInfo.ZipCode = txtZipcodeEdit.Text;
            objShippingInfo.Name = txtShippingFnameEdit.Text;
            objShippingInfo.Fax = txtShippingLnameEdit.Text;
            objShippingInfo.CompanyName = txtShippingCompanyNameEdit.Text;
            objShippingInfo.Address = txtShippingAddressEdit.Text;
            objShippingInfo.Email = txtEmailAdrressEdit.Text;
            //objShippingInfo.Telephone = txtPhoneNumberEdit.Text;
            objShippingInfo.CountryID = Convert.ToInt64(drpCountry.SelectedItem.Value.ToString());
            objShippingInfo.StateID = Convert.ToInt64(drpState.SelectedItem.Value.ToString());
            objShippingInfo.CityID = Convert.ToInt64(drpCity.SelectedItem.Value);

            objCmpEmpShipping.SubmitChanges();
        }
        else
        {
            //Insert New Shipping details here
            objShippingInfo.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
            objShippingInfo.ContactInfoType = "Shipping";
            objShippingInfo.OrderType = "MySetting";

            if (!String.IsNullOrEmpty(txtAddressTitle.Text))
                objShippingInfo.Title = txtAddressTitle.Text;

            if (!String.IsNullOrEmpty(txtZipcodeEdit.Text))
                objShippingInfo.ZipCode = txtZipcodeEdit.Text;

            if (!String.IsNullOrEmpty(txtShippingFnameEdit.Text))
                objShippingInfo.Name = txtShippingFnameEdit.Text;

            if (!String.IsNullOrEmpty(txtShippingLnameEdit.Text))
                objShippingInfo.Fax = txtShippingLnameEdit.Text;

            if (!String.IsNullOrEmpty(txtShippingCompanyNameEdit.Text))
                objShippingInfo.CompanyName = txtShippingCompanyNameEdit.Text;

            if (!String.IsNullOrEmpty(txtShippingAddressEdit.Text))
                objShippingInfo.Address = txtShippingAddressEdit.Text;

            if (!String.IsNullOrEmpty(txtEmailAdrressEdit.Text))
                objShippingInfo.Email = txtEmailAdrressEdit.Text;

            if (!String.IsNullOrEmpty(txtPhoneNumberEdit.Text))
                objShippingInfo.Telephone = txtPhoneNumberEdit.Text;

            if (drpCountry.SelectedItem.Value != "0")
            {
                objShippingInfo.CountryID = Convert.ToInt64(drpCountry.SelectedItem.Value.ToString());
                objShippingInfo.county = drpCountry.SelectedItem.Text;
            }

            if (drpState.SelectedItem.Value != "0")
                objShippingInfo.StateID = Convert.ToInt64(drpState.SelectedItem.Value.ToString());

            if (drpCity.SelectedItem.Value != "0")
                objShippingInfo.CityID = Convert.ToInt64(drpCity.SelectedItem.Value);

            objCmpEmpShipping.Insert(objShippingInfo);
            objCmpEmpShipping.SubmitChanges();
        }
    }

    private void SaveReturnShortsInfo()
    {
        Int32 QuantityINC3034 = 0;
        String SizeINC3034 = null;
        Int32 QuantityINC3032 = 0;
        String SizeINC3032 = null;
        String Title = null;
        String phone = null;

        if (!String.IsNullOrEmpty(txtQuantityINC3034.Text))
            QuantityINC3034 = Convert.ToInt32(txtQuantityINC3034.Text);

        if (!String.IsNullOrEmpty(txtQuantityINC3032.Text))
            QuantityINC3032 = Convert.ToInt32(txtQuantityINC3032.Text);

        if (ddlSizeINC3032.SelectedValue != "0")
            SizeINC3032 = ddlSizeINC3032.SelectedItem.Text;

        if (ddlSizeINC3034.SelectedValue != "0")
            SizeINC3034 = ddlSizeINC3034.SelectedItem.Text;

        if (!String.IsNullOrEmpty(txtAddressTitle.Text))
            Title = txtAddressTitle.Text;

        if (!String.IsNullOrEmpty(txtPhoneNumberEdit.Text))
            phone = txtPhoneNumberEdit.Text;
        // to Insert records in Return Short Product
        db.SP_InsertReturnShortProductRecords(0, this.OrderReturnId, IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(txtQnty.Text), lblShortReturnDescriptions.Text, QuantityINC3032, QuantityINC3034,
            SizeINC3032, SizeINC3034, lblDescriptionINC3032.Text, lblProductDescriptionINC3034.Text, DateTime.Now.Date, txtShippingFnameEdit.Text, txtShippingLnameEdit.Text,
            txtShippingAddressEdit.Text, txtShippingCompanyNameEdit.Text, Title, drpCountry.SelectedItem.Text, drpState.SelectedItem.Text, drpCity.SelectedItem.Text, txtZipcodeEdit.Text, phone, txtEmailAdrressEdit.Text);
    }

    #endregion

    #region sending email to the user

    String messageBodyIE = String.Empty;

    private void sendVerificationEmail(String OrderNumber)
    {

        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            RegistrationBE objRegistrationBE = new RegistrationBE();
            RegistrationDA objRegistrationDA = new RegistrationDA();
            DataSet dsEmailTemplate;
            Common objcommm = new Common();
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Short Return";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
            UserInformation objUsrInfo = new UserInformation();
            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                objUsrInfo = objUsrInfoRepo.GetById(IncentexGlobal.CurrentMember.UserInfoID);
                String sToadd = "";
                Int64 sToUserInfoID = 0;
                if (objUsrInfo != null)
                {
                    //sToadd = "surendar.yadav@indianic.com";
                    sToadd = objUsrInfo.LoginEmail; //+";incentex@wtdc.com;";
                    sToUserInfoID = objUsrInfo.UserInfoID;
                }

                String sSubject = "Shorts Exchange " + OrderNumber;
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{Customername}", objUsrInfo.FirstName + " " + objUsrInfo.LastName);
                messagebody.Replace("{RA}", "RA" + OrderNumber);
                messagebody.Replace("{fullname}", objUsrInfo.FirstName + " " + objUsrInfo.LastName);
                messagebody.Replace("{ReferenceName}", OrderNumber);
                // For Quantity
                messagebody.Replace("{ReturnQuantity}", txtQnty.Text);
                messagebody.Replace("{SentOn}", System.DateTime.Now.ToString());
                messagebody.Replace("{Status}", "Shorts Exchange");
                //messagebody.Replace("{storename}", (new CompanyRepository().GetById((Int64)IncentexGlobal.CurrentMember.CompanyId).CompanyName).ToString());
                messagebody.Replace("{storename}", "Shorts Return System");
                //Start WTDC Shipp To Address

                ManagedShipAddressRepository objManageRepos = new ManagedShipAddressRepository();
                MangedShipAddress objManage = new MangedShipAddress();
                objManage = objManageRepos.GetAllRecord();
                if (objManage != null)
                {
                    messagebody.Replace("{CompanyName}", objManage.CompanyName);
                    messagebody.Replace("{Title}", objManage.Title);
                    messagebody.Replace("{Address}", objManage.Address);
                    messagebody.Replace("{CityStateZip}", new CityRepository().GetById(Convert.ToInt64(objManage.CityId)).sCityName + "  " + new StateRepository().GetById(Convert.ToInt64(objManage.StateId)).sStatename + "  " + objManage.Zipcode);
                    messagebody.Replace("{Tel}", objManage.Telephone);
                }

                //End
                // Start shipping Address

                if (!String.IsNullOrEmpty(txtShippingFnameEdit.Text))
                    messagebody.Replace("{Customername}", txtShippingFnameEdit.Text + " " + txtShippingLnameEdit.Text);

                if (!String.IsNullOrEmpty(txtShippingCompanyNameEdit.Text))
                    messagebody.Replace("{CustomerAddress}", txtShippingAddressEdit.Text);

                if (drpCity.SelectedItem.Value != "0")
                    messagebody.Replace("{CustomerCity}", drpCity.SelectedItem.Text);

                if (drpState.SelectedItem.Value != "0")
                    messagebody.Replace("{CustomerState}", drpState.SelectedItem.Text);

                if (drpCountry.SelectedItem.Value != "0")
                    messagebody.Replace("{CustomerCountry}", drpCountry.SelectedItem.Text);

                if (!String.IsNullOrEmpty(txtPhoneNumberEdit.Text))
                    messagebody.Replace("{Tel1}", txtPhoneNumberEdit.Text);

                if (!String.IsNullOrEmpty(txtEmailAdrressEdit.Text))
                    messagebody.Replace("{Email}", txtEmailAdrressEdit.Text);

                if (!String.IsNullOrEmpty(txtZipcodeEdit.Text))
                    messagebody.Replace("{Zip}", txtZipcodeEdit.Text);

                messagebody.Replace("{Customerstorename}", "Shorts Return System");
                // END
                #region start
                String innermessage = "";

                // For INC_3032 Items
                if (!String.IsNullOrEmpty(txtQuantityINC3032.Text))
                {

                    innermessage = innermessage + "<tr>";
                    innermessage = innermessage + "<td width='20%' style='font-weight: bold;'>";
                    innermessage = innermessage + "INC_3032_00";// 
                    innermessage = innermessage + "</td>";
                    innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align:center;'>";
                    innermessage = innermessage + txtQuantityINC3032.Text;
                    innermessage = innermessage + "</td>";
                    innermessage = innermessage + "<td width='20%' style='font-weight: bold;text-align:center;'>";
                    innermessage = innermessage + ddlSizeINC3032.SelectedItem.Text;
                    innermessage = innermessage + "</td>";
                    innermessage = innermessage + "<td width='35%' style='font-weight: bold;text-align:left;'>";
                    innermessage = innermessage + lblDescriptionINC3032.Text;
                    innermessage = innermessage + "</td>";
                    innermessage = innermessage + "</tr>";
                }
                // For INC_3034 Items
                if (!String.IsNullOrEmpty(txtQuantityINC3034.Text))
                {

                    innermessage = innermessage + "<tr>";
                    innermessage = innermessage + "<td width='20%' style='font-weight: bold;'>";
                    innermessage = innermessage + "INC_3034_00";// 
                    innermessage = innermessage + "</td>";
                    innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align:center;'>";
                    innermessage = innermessage + txtQuantityINC3034.Text;
                    innermessage = innermessage + "</td>";
                    innermessage = innermessage + "<td width='20%' style='font-weight: bold;text-align:center;'>";
                    innermessage = innermessage + ddlSizeINC3034.SelectedItem.Text;
                    innermessage = innermessage + "</td>";
                    innermessage = innermessage + "<td width='35%' style='font-weight: bold;text-align:left;'>";
                    innermessage = innermessage + lblProductDescriptionINC3034.Text;
                    innermessage = innermessage + "</td>";
                    innermessage = innermessage + "</tr>";
                }
                messagebody.Replace("{innermessage}", innermessage);


                #endregion
                messagebody.Replace("{reasonforreturn}", "Shorts Return");//+ lblShortReturnDescriptions.Text); 
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                // Set messageBody for Admin
                messageBodyIE = messagebody.ToString();

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(Convert.ToInt64(sToUserInfoID), "Short Returns", CommonMails.EmailFrom, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendVerificationEmailtoIE(Int32 sToUserInfoID, String FullName, String sToadd)
    {

        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            RegistrationBE objRegistrationBE = new RegistrationBE();
            RegistrationDA objRegistrationDA = new RegistrationDA();
            DataSet dsEmailTemplate;
            Common objcommm = new Common();
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Short Return IE";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            if (dsEmailTemplate != null)
            {
                String sSubject = "Shorts Exchange " + OrderNumber;
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{IEfullname}", FullName);

                // Set messageBody as per Request
                if (!String.IsNullOrEmpty(messageBodyIE))
                    messagebody.Replace("{innermessage}", messageBodyIE);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();
                new CommonMails().SendMail(Convert.ToInt64(sToUserInfoID), "Short Returns", CommonMails.EmailFrom, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendIEEmail()
    {
        List<UserInformation> objAdminList = new List<UserInformation>();
        objAdminList = objUserInformationRepository.GetEmailInformation();

        if (objAdminList.Count > 0)
            for (Int32 i = 0; i < objAdminList.Count; i++)
                sendVerificationEmailtoIE(Convert.ToInt32(objAdminList[i].UserInfoID), objAdminList[i].FirstName, objAdminList[i].LoginEmail.ToString());
    }

    #endregion
}