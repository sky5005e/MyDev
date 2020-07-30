using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using ShippingReference;
using Incentex.DAL.Common;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public partial class HeadsetRepairCenter_AddHeadsetRepairCenter : PageBase
{
    #region Data Member's

    CompanyRepository objcompany = new CompanyRepository();
    UserInformationRepository objcompanyEmployee = new UserInformationRepository();
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    LookupRepository objLookRep = new LookupRepository();
    HeadsetRepairCenterRepository objHeadset = new HeadsetRepairCenterRepository();
    StateRepository objState = new StateRepository();
    CityRepository objCity = new CityRepository();
    CountryRepository objCountry = new CountryRepository();
    String Message = String.Empty;

    long HeadsetRepairID
    {
        get
        {
            if (ViewState["HeadsetRepairID"] != null)
                return Convert.ToInt64(ViewState["HeadsetRepairID"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["HeadsetRepairID"] = value;
        }
    }

    long RepairNumberDetails
    {
        get
        {
            if (ViewState["RepairNumberDetails"] != null)
                return Convert.ToInt64(ViewState["RepairNumberDetails"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["RepairNumberDetails"] = value;
        }
    }

    String RepairNumber
    {
        get
        {
            if (ViewState["RepairNumber"] != null)
                return ViewState["RepairNumber"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["RepairNumber"] = value;
        }
    }

    long? Company
    {
        get
        {
            if (ViewState["Company"] != null)
                return Convert.ToInt64(ViewState["Company"].ToString());
            else
                return null;
        }
        set
        {
            ViewState["Company"] = value;
        }
    }

    long? Contact
    {
        get
        {
            if (ViewState["Contact"] != null)
                return Convert.ToInt64(ViewState["Contact"].ToString());
            else
                return null;
        }
        set
        {
            ViewState["Contact"] = value;
        }
    }

    long? Status
    {
        get
        {
            if (ViewState["Status"] != null)
                return Convert.ToInt64(ViewState["Status"].ToString());
            else
                return null;
        }
        set
        {
            ViewState["Status"] = value;
        }
    }

    #endregion

    #region Events
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
        if (!IsPostBack)
        {
            base.MenuItem = "Headset Repair Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            BindDropdown();

            ((Label)Master.FindControl("lblPageHeading")).Text = "Create Headset Repair Center";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/HeadsetRepairCenter/HeadsetRepairCenter.aspx";


            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) ||
                IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
            {
                trCompany.Visible = false;
                //trContact.Visible = false;

                this.BindContactDropdown(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
                ddlContact.SelectedValue = IncentexGlobal.CurrentMember.UserInfoID.ToString();

                //Bind ReturnHeadset Shipping Address info
                this.BindReturnHeadsetsDropdown(Convert.ToInt64(ddlContact.SelectedValue), 0);

                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";
            }

            if (Request.QueryString["HeadsetRepairID"] != null)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["RepairNumber"]))
                    this.RepairNumber = Convert.ToString(Request.QueryString["RepairNumber"]);
                if (!String.IsNullOrEmpty(Request.QueryString["Company"]))
                    this.Company = Convert.ToInt64(Request.QueryString["Company"]);
                if (!String.IsNullOrEmpty(Request.QueryString["Contact"]))
                    this.Contact = Convert.ToInt64(Request.QueryString["Contact"]);
                if (!string.IsNullOrEmpty(Request.QueryString["Status"]))
                    this.Status = Convert.ToInt64(Request.QueryString["Status"]);

                this.GetHeadsetRepairDetails(Request.QueryString["HeadsetRepairID"].ToString());
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/HeadsetRepairCenter/HeadsetRepairDetails.aspx?RepairNumberDetails=" + RepairNumberDetails + "&Contact=" + this.Contact + "&Company=" + this.Company + "&RepairNumber=" + RepairNumber + "&Status=" + Status;
            }

            //In edit mode status will be display
            trStatus.Visible = HeadsetRepairID > 0;
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            HeadsetRepairCenterMaster objHeadsetRepair = new HeadsetRepairCenterMaster();
            Message = string.Empty;

            objHeadsetRepair.HeadsetBrandID = Convert.ToInt64(ddlHeadsetBrand.SelectedValue);
            objHeadsetRepair.TotalHeadset = Convert.ToInt32(ddlTotalHeadsetstoRepair.SelectedItem.Text);
            objHeadsetRepair.RequestQuoteBeforeRepair = ddlRequestQuote.SelectedItem.Text == "Yes" ? true : false;
            objHeadsetRepair.ContactID = Convert.ToInt64(ddlContact.SelectedValue);
            objHeadsetRepair.Status = Convert.ToInt32(ddlStatus.SelectedValue);

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) ||
                  IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
            {
                objHeadsetRepair.CompanyID = IncentexGlobal.CurrentMember.CompanyId;
                //objHeadsetRepair.ContactID = IncentexGlobal.CurrentMember.UserInfoID;
            }
            else
            {
                objHeadsetRepair.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
                //objHeadsetRepair.ContactID = Convert.ToInt64(ddlContact.SelectedValue);
            }



            if (HeadsetRepairID == 0)
            {
                Int64? HeadsetRepairNo = objHeadset.GetMaxRepairNo();
                if (HeadsetRepairNo != null)
                    objHeadsetRepair.RepairNumber = Convert.ToInt64(HeadsetRepairNo + 1);
                else
                    objHeadsetRepair.RepairNumber = 1000001;

                //Add HeadsetRepair Return AddressTo
                if (ddlReturnHeadsets.SelectedIndex > 0)
                    objHeadsetRepair.ReturnHeadsetToID = Convert.ToInt64(ddlReturnHeadsets.SelectedValue);

                if (objHeadsetRepair.Status == 0)
                    objHeadsetRepair.Status = null;

                objHeadsetRepair.Date = System.DateTime.Now;
                objHeadsetRepair.CreateBy = IncentexGlobal.CurrentMember.UserInfoID;

                objHeadset.Insert(objHeadsetRepair);
                this.PrintPrepaidLabel(objHeadsetRepair);
                lblMsg.Text = "Records sucessfully Created";
                this.ClearControl();
            }
            else
            {
                objHeadsetRepair.UpdatedDate = System.DateTime.Now;
                objHeadsetRepair.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

                //Add HeadsetRepair Return AddressTo
                objHeadsetRepair.ReturnHeadsetToID = Convert.ToInt64(ddlReturnHeadsets.SelectedValue);
                objHeadsetRepair.HeadsetRepairID = this.HeadsetRepairID;
                objHeadset.UpdateHeaderRepairCenter(objHeadsetRepair);

                Response.Redirect("~/HeadsetRepairCenter/HeadsetRepairDetails.aspx?RepairNumberDetails=" + RepairNumberDetails + "&Contact=" + this.Contact + "&Company=" + this.Company + "&RepairNumber=" + RepairNumber + "&Status=" + Status);
            }

            objHeadset.SubmitChanges();
            //Response.Redirect("HeadsetRepairCenter.aspx?req=1");
        }
        catch (Exception ex)
        {
            //Response.Write(Message.ToString());
            Message = "Customer shipping address is not correct";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), System.DateTime.Now.ToString(), "javascript:alert('" + Message.ToString() + "', '', '');", true);
        }
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindContactDropdown(Convert.ToInt64(ddlCompany.SelectedValue));
    }

    protected void ddlContect_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindReturnHeadsetsDropdown(Convert.ToInt64(ddlContact.SelectedValue), 0);
    }

    protected void ddlretCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(ddlretCountry.SelectedValue));

        ddlretState.Items.Clear();
        ddlretState.ClearSelection();
        ddlretCity.Items.Clear();
        ddlretCity.Items.Insert(0, new ListItem("-select-", "0"));
        Common.BindDDL(ddlretState, objStateList, "sStatename", "iStateID", "-select-");

        modalPrint.PopupControlID = "pnlReturnHeadsetrepaircenter";
        modalPrint.CancelControlID = "cboxClose";
        modalPrint.Show();
    }

    protected void ddlretState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(ddlretState.SelectedValue));

        ddlretCity.Items.Clear();
        Common.BindDDL(ddlretCity, objCityList, "sCityName", "iCityID", "-select city-");

        modalPrint.PopupControlID = "pnlReturnHeadsetrepaircenter";
        modalPrint.CancelControlID = "cboxClose";
        modalPrint.Show();
    }

    protected void btnbtnreturnheadset_Click(object sender, EventArgs e)
    {
        #region Country
        CountryRepository objCountryRepo = new CountryRepository();
        List<INC_Country> objCountryList = objCountryRepo.GetAll();
        Common.BindDDL(ddlretCountry, objCountryList, "sCountryName", "iCountryID", "-select country-");
        ddlretCountry.SelectedValue = ddlretCountry.Items.FindByText("United States").Value;
        ddlretCountry_SelectedIndexChanged(null, null);
        #endregion

        modalPrint.PopupControlID = "pnlReturnHeadsetrepaircenter";
        modalPrint.CancelControlID = "cboxClose";
        modalPrint.Show();
    }

    protected void lnkbtnheadsetreturn_Click(object sender, EventArgs e)
    {
        CompanyEmployeeContactInfo objShipInfo = new CompanyEmployeeContactInfo();
        objShipInfo.CompanyName = txtretCompany.Text.Trim();
        objShipInfo.Name = txtretContact.Text.Trim();
        objShipInfo.Address = txtretaddress1.Text.Trim();
        objShipInfo.Address2 = txtretaddress2.Text.Trim();
        objShipInfo.Title = txtretAddressName.Text.Trim();  // AddressName 
        objShipInfo.CountryID = Convert.ToInt64(ddlretCountry.SelectedValue);
        objShipInfo.StateID = Convert.ToInt64(ddlretState.SelectedValue);
        objShipInfo.CityID = Convert.ToInt64(ddlretCity.SelectedValue);
        objShipInfo.ZipCode = txtretzip.Text.Trim();
        objShipInfo.Telephone = txtrettelephone.Text.Trim();
        objShipInfo.DepartmentID = null;
        objShipInfo.UserInfoID = Convert.ToInt64(ddlContact.SelectedValue);

        objShipInfo.ContactInfoType = DAEnums.CompanyEmployeeContactInfo.Shipping.ToString();
        objShipInfo.OrderType = "MySetting";


        objCmpEmpContRep.Insert(objShipInfo);
        objCmpEmpContRep.SubmitChanges();

        BindReturnHeadsetsDropdown(Convert.ToInt64(ddlContact.SelectedValue), objShipInfo.CompanyContactInfoID);
    }

    #endregion

    #region Methods

    protected void BindDropdown()
    {
        ddlContact.Items.Insert(0, new ListItem("-Select-", "0"));
        ddlReturnHeadsets.Items.Insert(0, new ListItem("-Select-", "0"));

        #region Company
        ddlCompany.DataSource = objcompany.GetAllCompany();
        ddlCompany.DataValueField = "CompanyID";
        ddlCompany.DataTextField = "CompanyName";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion

        #region HeadsetBrand
        ddlHeadsetBrand.DataSource = objLookRep.GetByLookup("HeadsetBrand");
        ddlHeadsetBrand.DataValueField = "iLookupID";
        ddlHeadsetBrand.DataTextField = "sLookupName";
        ddlHeadsetBrand.DataBind();
        ddlHeadsetBrand.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion

        #region Status
        ddlStatus.DataSource = objLookRep.GetByLookup("HeadsetStatus");
        ddlStatus.DataValueField = "iLookupID";
        ddlStatus.DataTextField = "sLookupName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion
    }

    private void BindContactDropdown(long CompanyID)
    {
        ddlContact.DataSource = objcompanyEmployee.GetAllUser(CompanyID);
        ddlContact.DataValueField = "UserInfoID";
        ddlContact.DataTextField = "UserName";
        ddlContact.DataBind();
        ddlContact.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    private void BindReturnHeadsetsDropdown(long UserinfoID, long SelectedValue)
    {
        ddlReturnHeadsets.DataSource = objCmpEmpContRep.GetSavedShippingAddressesForUser(UserinfoID, CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Company, DAEnums.SortOrderType.Asc);
        ddlReturnHeadsets.DataValueField = "companycontactinfoID";
        ddlReturnHeadsets.DataTextField = "companyname";
        ddlReturnHeadsets.DataBind();
        ddlReturnHeadsets.Items.Insert(0, new ListItem("-Select-", "0"));

        if (ddlContact.SelectedIndex > 0)
            ddlReturnHeadsets.Items.Insert(ddlReturnHeadsets.Items.Count, new ListItem("Other", "-1"));

        ddlReturnHeadsets.SelectedValue = Convert.ToString(SelectedValue);
    }

    protected void PrintPrepaidLabel(HeadsetRepairCenterMaster objHeadsetRepair)
    {
        try
        {
            ShipService shpSvc = new ShipService();
            ShipmentRequest shipmentRequest = new ShipmentRequest();
            UPSSecurity upss = new UPSSecurity();
            UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSSecurityServiceAccessToken();
            upssSvcAccessToken.AccessLicenseNumber = System.Configuration.ConfigurationSettings.AppSettings["UPSLicenceNumber"];
            upss.ServiceAccessToken = upssSvcAccessToken;
            UPSSecurityUsernameToken upssUsrNameToken = new UPSSecurityUsernameToken();
            upssUsrNameToken.Username = System.Configuration.ConfigurationSettings.AppSettings["UPSUserName"];
            upssUsrNameToken.Password = System.Configuration.ConfigurationSettings.AppSettings["UPSPassword"];
            upss.UsernameToken = upssUsrNameToken;
            shpSvc.UPSSecurityValue = upss;
            RequestType request = new RequestType();
            String[] requestOption = { "nonvalidate" };
            request.RequestOption = requestOption;
            shipmentRequest.Request = request;
            ShipmentType shipment = new ShipmentType();
            shipment.Description = "Ship webservice example";
            ShipperType shipper = new ShipperType();
            shipper.ShipperNumber = "9X91X1";
            PaymentInfoType paymentInfo = new PaymentInfoType();
            ShipmentChargeType shpmentCharge = new ShipmentChargeType();
            BillShipperType billShipper = new BillShipperType();
            billShipper.AccountNumber = "9X91X1";
            shpmentCharge.BillShipper = billShipper;
            shpmentCharge.Type = "01";
            ShipmentChargeType[] shpmentChargeArray = { shpmentCharge };
            paymentInfo.ShipmentCharge = shpmentChargeArray;
            shipment.PaymentInformation = paymentInfo;

            // Shipper Address ie Form Address 
            UserInformation objuserinfo = objcompanyEmployee.GetById(objHeadsetRepair.ContactID);
            Company objcom = objcompany.GetById(objHeadsetRepair.CompanyID.Value);

            ShippingReference.ShipAddressType shipperAddress = new ShippingReference.ShipAddressType();
            //String[] addressLine = { objShippingInfo.Address + ", " + objState.GetById((long)objShippingInfo.StateID).sStatename };
            String[] addressLine = { objuserinfo.Address1 + ", " + objuserinfo.Address2 }; //", " + objState.GetById((long)objuserinfo.StateId).sStatename 
            shipperAddress.AddressLine = addressLine;
            shipperAddress.City = objCity.GetById((long)objuserinfo.CityId).sCityName;
            shipperAddress.PostalCode = objuserinfo.ZipCode;
            shipperAddress.StateProvinceCode = objState.GetStateProvinceCodebyStateId((long)objuserinfo.StateId).ToString();
            shipperAddress.CountryCode = "US";
            shipper.Address = shipperAddress;
            shipper.Name = objuserinfo.FirstName + " " + objuserinfo.LastName;
            shipper.AttentionName = objcom.CompanyName;

            ShipPhoneType shipperPhone = new ShipPhoneType();
            if (objuserinfo.Telephone.Length >= 10)
                shipperPhone.Number = objuserinfo.Telephone;
            else
                shipperPhone.Number = "0123456789";

            shipper.Phone = shipperPhone;
            shipment.Shipper = shipper;

            // Ship To Address. Company Address
            ShipToType shipTo = new ShipToType();
            ShipToAddressType shipToAddress = new ShipToAddressType();
            String[] addressLine1 = { "400 Beach Road,228, Vero Beach" };
            shipToAddress.AddressLine = addressLine1;
            shipToAddress.City = "Florida";
            shipToAddress.PostalCode = "32963";
            shipToAddress.StateProvinceCode = "FL";
            shipToAddress.CountryCode = "US";
            shipTo.Address = shipToAddress;
            shipTo.AttentionName = "World-Link";
            shipTo.Name = "World-Link";
            ShipPhoneType shipToPhone = new ShipPhoneType();
            shipToPhone.Number = "17722317452";
            shipTo.Phone = shipToPhone;
            shipment.ShipTo = shipTo;

            // Other Service
            ServiceType service = new ServiceType();
            service.Code = "01";
            shipment.Service = service;
            PackageType package = new PackageType();
            PackageWeightType packageWeight = new PackageWeightType();
            packageWeight.Weight = "1";
            ShipUnitOfMeasurementType uom = new ShipUnitOfMeasurementType();
            uom.Code = "LBS";
            packageWeight.UnitOfMeasurement = uom;
            package.PackageWeight = packageWeight;
            PackagingType packType = new PackagingType();
            packType.Code = "02";
            package.Packaging = packType;
            PackageType[] pkgArray = { package };
            shipment.Package = pkgArray;
            LabelSpecificationType labelSpec = new LabelSpecificationType();
            LabelStockSizeType labelStockSize = new LabelStockSizeType();
            labelStockSize.Height = "6";
            labelStockSize.Width = "4";
            labelSpec.LabelStockSize = labelStockSize;
            LabelImageFormatType labelImageFormat = new LabelImageFormatType();
            labelImageFormat.Code = "GIF";//"SPL";
            labelSpec.LabelImageFormat = labelImageFormat;
            shipmentRequest.LabelSpecification = labelSpec;
            shipmentRequest.Shipment = shipment;
            System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicyForUPS();
            Message += shipmentRequest + "<br/>";
            ShipmentResponse shipmentResponse = shpSvc.ProcessShipment(shipmentRequest);
            Message += "The transaction was a " + shipmentResponse.Response.ResponseStatus.Description + "<br/>";
            Message += "The 1Z number of the new shipment is " + shipmentResponse.ShipmentResults.ShipmentIdentificationNumber + "<br/>";

            //Ser Shipping TrackingNumber
            //objHeadsetRepair.OrderTrackingNumber = shipmentResponse.ShipmentResults.ShipmentIdentificationNumber;

            objHeadset.SubmitChanges();

            // Convert image to graphic
            String base64String = shipmentResponse.ShipmentResults.PackageResults[0].ShippingLabel.GraphicImage;
            // Save Base 64 String to Image
            GraphicImageToImage(base64String, objHeadsetRepair.RepairNumber.Value.ToString());

            // Now Open then printable content in new popup window
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(700/2);window.open('PrintPreShippingLabel.aspx?Repairnumber=" + objHeadsetRepair.RepairNumber + "', null, 'height=700,width=760,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
        }
        catch (System.Web.Services.Protocols.SoapException ex)
        {

            Message += "---------Ship Web Service returns error----------------" + "<br/>";
            Message += "---------\"Hard\" is user error \"Transient\" is system error----------------" + "<br/>";
            Message += "SoapException Message= " + ex.Message + "<br/>";
            Message += "SoapException Category:Code:Message= " + ex.Detail.LastChild.InnerText + "<br/>";
            Message += "SoapException XML String for all= " + ex.Detail.LastChild.OuterXml + "<br/>";
            Message += "SoapException StackTrace= " + ex.StackTrace + "<br/>";
            Message += "-------------------------" + "<br/>";

            throw new ArgumentNullException(Message);
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            Message += "--------------------" + "<br/>";
            Message += "CommunicationException= " + ex.Message + "<br/>";
            Message += "CommunicationException-StackTrace= " + ex.StackTrace + "<br/>";
            Message += "-------------------------" + "<br/>";

            throw new ArgumentNullException(Message);
        }
        catch (Exception ex)
        {

            Message += "-------------------------" + "<br/>";
            Message += " Generaal Exception= " + ex.Message + "<br/>";
            Message += " Generaal Exception-StackTrace= " + ex.StackTrace + "<br/>";
            Message += "-------------------------" + "<br/>";

            throw new ArgumentNullException(Message);
        }
        finally
        {

        }
    }

    private void GraphicImageToImage(String base64String, string HeadsetrepairNumber)
    {
        System.Drawing.Image newImage;
        String strFileName = "HeadsetRepairShippingLabel.JPEG";
        String sFilePath = Server.MapPath("../UPSShippingReferences/ShippingLabelImage/") + strFileName;
        byte[] data = Convert.FromBase64String(base64String);
        using (System.IO.MemoryStream stream = new System.IO.MemoryStream(data, 0, data.Length))
        {
            HeadsetRepairCenterCustom objHeadsetDetails = objHeadset.GetHeadsetRepaircustomByRepairNumber(Convert.ToInt64(HeadsetrepairNumber));

            //TODO: do something with image
            newImage = System.Drawing.Image.FromStream(stream);

            Bitmap bitMapImage = new System.Drawing.Bitmap(newImage);

            Graphics graphicImage = Graphics.FromImage(bitMapImage);
            Graphics graphicCompanyColumn = Graphics.FromImage(bitMapImage);
            Graphics graphicRepairNumberColumn = Graphics.FromImage(bitMapImage);

            //Smooth graphics is nice.
            graphicImage.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(-600, 1020, 400, 600);
            graphicImage.RotateTransform(-90f);

            //Write your text.
            graphicImage.DrawString("Headset Repair Number : HR" + HeadsetrepairNumber, new Font("Arial", 12, FontStyle.Bold), SystemBrushes.WindowText, rect);



            Rectangle rectCompany = new Rectangle(-793, 1220, 400, 400);
            graphicCompanyColumn.RotateTransform(-90f);
            graphicCompanyColumn.DrawString("Company : " + objHeadsetDetails.CompanyName + "\n\nContact : " + objHeadsetDetails.ContactName + "\n\nRepair Quantity : " + Convert.ToString(objHeadsetDetails.TotalHeadset) + "\n\nHeadset Brand : " + objHeadsetDetails.HeadsetBrandName + "", new Font("Arial", 14, FontStyle.Bold), SystemBrushes.WindowText, rectCompany);

            Rectangle rectRepairnumber = new Rectangle(-400, 1220, 400, 200);
            graphicRepairNumberColumn.RotateTransform(-90f);
            graphicRepairNumberColumn.DrawString("Repair Number : HR" + objHeadsetDetails.RepairNumber + "\n\nSubmit Date : " + Convert.ToString(objHeadsetDetails.Date) + "\n\nRequest Quote : " + (objHeadsetDetails.Requestquotebeforerepair ? "Yes" : "No") + "", new Font("Arial", 14, FontStyle.Bold), SystemBrushes.WindowText, rectRepairnumber);

            bitMapImage.Save(sFilePath);
        }
    }

    private void ClearControl()
    {
        ddlCompany.SelectedIndex = 0;
        ddlContact.SelectedIndex = 0;
        ddlHeadsetBrand.SelectedIndex = 0;
        ddlRequestQuote.SelectedIndex = 0;
        ddlTotalHeadsetstoRepair.SelectedIndex = 0;
        ddlReturnHeadsets.SelectedIndex = 0;
    }

    private void GetHeadsetRepairDetails(string HeadsetRepairID)
    {
        HeadsetRepairCenterMaster objHeadsetDetails = objHeadset.GetHeadsetRepairCenterById(Convert.ToInt32(HeadsetRepairID));

        ddlCompany.SelectedValue = objHeadsetDetails.CompanyID.Value.ToString();
        this.BindContact();


        ddlContact.SelectedValue = objHeadsetDetails.ContactID.Value.ToString();
        ddlHeadsetBrand.SelectedValue = objHeadsetDetails.HeadsetBrandID.ToString();
        ddlTotalHeadsetstoRepair.SelectedValue = objHeadsetDetails.TotalHeadset.ToString();
        ddlRequestQuote.SelectedValue = Convert.ToBoolean(objHeadsetDetails.RequestQuoteBeforeRepair.ToString()) ? "1" : "2";
        ddlStatus.SelectedValue = objHeadsetDetails.Status.ToString();

        this.HeadsetRepairID = objHeadsetDetails.HeadsetRepairID;
        this.RepairNumberDetails = objHeadsetDetails.RepairNumber.Value;

        this.BindReturnHeadsetsDropdown(Convert.ToInt64(ddlContact.SelectedValue), 0);
        if (objHeadsetDetails.ReturnHeadsetToID.HasValue)
            ddlReturnHeadsets.SelectedValue = Convert.ToString(objHeadsetDetails.ReturnHeadsetToID);
    }

    private void BindContact()
    {
        ddlContact.DataSource = objcompanyEmployee.GetAllUser(Convert.ToInt64(ddlCompany.SelectedValue));
        ddlContact.DataValueField = "UserInfoID";
        ddlContact.DataTextField = "UserName";
        ddlContact.DataBind();
        ddlContact.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    #endregion
}
