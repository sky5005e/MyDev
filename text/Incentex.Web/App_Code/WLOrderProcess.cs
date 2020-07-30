using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using com.strikeiron.ws;

/// <summary>
/// Summary description for WLLogin
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
[WebService(Namespace = "https://www.world-link.us.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WLOrderProcess : System.Web.Services.WebService
{
    #region Fields

    LookupRepository objLookupRepository = new LookupRepository();
    CompanyRepository objCompanyRepository = new CompanyRepository();
    CompanyEmployeeContactInfoRepository objCompanyEmployeeContactInfoRepository = new CompanyEmployeeContactInfoRepository();
    UserInformationRepository objUserInformationRepository = new UserInformationRepository();
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();
    List<SelectMyShoppingCartProductResult> objSelectMyShoppingCartProductResultList;
    Boolean IsTestOrder = false;
    #endregion

    #region Properties

    Decimal SalesTaxAmount
    {
        get;
        set;
    }

    Decimal StrikeIronTaxRate
    {
        get;
        set;
    }

    String County
    {
        get;
        set;
    }

    String StrikeIronResponseFileName
    {
        get;
        set;
    }

    String ShipToZipCodePassedToStrikeIron
    {
        get;
        set;
    }

    Boolean StrikeIronResponseFailed
    {
        get;
        set;
    }

    Boolean isFreeShipping
    {
        get;
        set;
    }

    #endregion

    public WLOrderProcess()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    #region Web Methods

    [WebMethod]
    public XmlDocument OrderProcess()
    {
        String xml = String.Empty;
        XmlDocument xmlDoc = new XmlDocument();
        String NetAmount = String.Empty;
        String MyShoppingCartID = String.Empty;
        String StoreID = String.Empty;
        String ShippingAddress = String.Empty;
        String BillingAddress = String.Empty;
        String CoupaOrderID = String.Empty;
        String ShipToName = String.Empty;
        try
        {
            // Initialize soap request XML
            XmlDocument xDoc = new XmlDocument();
            // Get raw request body
            Stream receiveStream = HttpContext.Current.Request.InputStream;
            // Move to begining of input stream and read
            receiveStream.Position = 0;
            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
            {
                // Load into XML document
                xDoc.Load(readStream);
            }

            XmlNode orderRequestHeader = xDoc.SelectSingleNode("/cXML/Request/OrderRequest/OrderRequestHeader");
            XmlNodeList lstItemOut = xDoc.SelectNodes("/cXML/Request/OrderRequest/ItemOut");

            if (orderRequestHeader != null && lstItemOut != null && lstItemOut.Count > 0)
            {
                CoupaOrderID = orderRequestHeader.Attributes["orderID"].Value;

                if (orderRequestHeader.Attributes["isTestOrder"] != null)
                    IsTestOrder = Convert.ToBoolean(orderRequestHeader.Attributes["isTestOrder"].Value);

                NetAmount = orderRequestHeader.SelectSingleNode("/cXML/Request/OrderRequest/OrderRequestHeader/Total/Money").InnerText;
                ShippingAddress = orderRequestHeader.SelectSingleNode("/cXML/Request/OrderRequest/OrderRequestHeader/ShipTo").OuterXml;
                BillingAddress = orderRequestHeader.SelectSingleNode("/cXML/Request/OrderRequest/OrderRequestHeader/BillTo").OuterXml;
                ShipToName = orderRequestHeader.SelectSingleNode("/cXML/Request/OrderRequest/OrderRequestHeader/Contact/Name").InnerText;

                xDoc.Save(Path.Combine(Common.CoupaOrderRequestPath, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_" + CoupaOrderID + ".xml"));

                StoreID = lstItemOut[0].SelectSingleNode("/cXML/Request/OrderRequest/ItemOut/ItemID/SupplierPartAuxiliaryID").InnerText;

                for (Int32 i = 0; i < lstItemOut.Count; i++)
                {
                    String shoppingCartID = lstItemOut[i].SelectSingleNode("/cXML/Request/OrderRequest/ItemOut/ItemID/SupplierPartID").InnerText;

                    if (!String.IsNullOrEmpty(MyShoppingCartID))
                        MyShoppingCartID += "," + shoppingCartID;
                    else
                        MyShoppingCartID = shoppingCartID;
                }

                //Check for NetAmount
                if (String.IsNullOrEmpty(NetAmount))
                    xml = "<Response><Status code=\"400\" text=\"unsuccess\"></Status></Response>";
                else
                {
                    if (!IsCoupaOrderIDExist(CoupaOrderID))
                    {
                        //Create String for response
                        xml = "<cXML xml:lang=\"en-US\" payloadID=" + "\"" + DateTime.Now.Ticks + "\"" + " timestamp=" + "\"" + DateTime.Now.ToString() + "\"" + @" >" +
                           "<Response><Status code=\"200\" text=\"OK\"/></Response></cXML>";

                        ProcessWLOrderRecords(MyShoppingCartID, Convert.ToInt64(StoreID), CoupaOrderID, Convert.ToDecimal(NetAmount), BillingAddress, ShippingAddress, ShipToName);
                    }
                    else
                    {
                        //Create String for response
                        xml = "<cXML xml:lang=\"en-US\" payloadID=" + "\"" + DateTime.Now.Ticks + "\"" + " timestamp=" + "\"" + DateTime.Now.ToString() + "\"" + @" >" +
                           "<Response><Status code=\"201\" text=\"OK\"/></Response></cXML>";
                    }
                }
                //Load String in xml doc and return
            }
            else
            {
                xml = "<Response><Status code=\"400\" text=\"unsuccess\"></Status></Response>";
            }

            xmlDoc.LoadXml(xml);
            return xmlDoc;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
            xml = "<Response><Status code=\"400\" text=\"unsuccess\">Internal Exception</Status></Response>";
            xmlDoc.LoadXml(xml);
            return xmlDoc;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Insert Data to Order Table
    /// </summary>
    private Boolean ProcessWLOrderRecords(String myCardIDs, Int64 storeID, String coupaOrderID, Decimal orderAmount, String billingAddress, String shippingAddress, String shipToName)
    {
        Boolean IsProcesSuccess = false;
        try
        {
            UserInformation objUserInfo = new UserInformation();
            if (HttpContext.Current.Request.IsLocal)
                objUserInfo = objUserInformationRepository.GetByLoginEmail("incentextest10@gmail.com");
            else
                objUserInfo = objUserInformationRepository.GetByLoginEmail("michael.wagner@asig.com");

            CompanyEmployee objCompanyEmp = new CompanyEmployee();
            objCompanyEmp = objCompanyEmployeeRepository.GetByUserInfoId(objUserInfo.UserInfoID);

            #region Order Number Generation

            String strOrderNumber = objOrderConfirmationRepository.GetMaxOrderNumber();

            // Check if OrderNumber for the workgroup is null or empty
            if (String.IsNullOrEmpty(strOrderNumber))
                strOrderNumber = DateTime.Now.Year.ToString() + "000001"; // Generate New Order Number
            else
            {
                // Check if new year?
                if (strOrderNumber.Substring(0, 4) == DateTime.Now.Year.ToString())
                    strOrderNumber = (Int64.Parse(strOrderNumber) + 1).ToString(); // Same year so Increment Order Number
                else // New year so Generate New Order Number
                    strOrderNumber = DateTime.Now.Year.ToString() + "000001";
            }

            #endregion

            #region Set Order Properties

            Order objOrder = new Order();
            objOrder.OrderNumber = strOrderNumber;
            objOrder.UserId = objUserInfo.UserInfoID;

            if (IsTestOrder)
                objOrder.ReferenceName = "Test Order : From Coupa, PO number :" + coupaOrderID;
            else
                objOrder.ReferenceName = "Order : From Coupa, PO number :" + coupaOrderID;

            objOrder.SalesTax = 0M;
            objOrder.OrderDate = DateTime.Now;
            objOrder.OrderAmount = orderAmount;
            objOrder.ShippingAmount = 0M;
            objOrder.StrikeIronTaxRate = 0M;
            objOrder.SpecialOrderInstruction = null;
            objOrder.OrderStatus = "Open";
            objOrder.CreatedDate = DateTime.Now;
            objOrder.UpdatedDate = null;
            objOrder.CreatedBy = objUserInfo.UserInfoID;
            objOrder.UpdatedBy = objUserInfo.UserInfoID;
            objOrder.WorkgroupId = objCompanyEmp.WorkgroupID;
            objOrder.MyShoppingCartID = Convert.ToString(myCardIDs);
            objOrder.StoreID = storeID;

            //Get Employee Type           
            if (objCompanyEmp.EmployeeTypeID != null)
                objOrder.EmployeeTypeID = objCompanyEmp.EmployeeTypeID;

            objOrder.IsPaid = true;
            objOrder.CreditUsed = null;
            objOrder.CreditAmt = 0M;
            objOrder.OrderFor = "ShoppingCart";
            objOrder.CoupaOrderID = coupaOrderID;

            #endregion

            #region Save Record

            objOrderConfirmationRepository.Insert(objOrder);
            objOrderConfirmationRepository.SubmitChanges();
            Int64 OrderIDNew = objOrder.OrderID;

            #endregion

            #region Calculating Shipping Amount

            Decimal shippingAmount = 0M;

            GetCheckoutDetailsByUserInfoIDResult objUserDetail = new UserInformationRepository().GetCheckoutDetailsByUserInfoID(objUserInfo.UserInfoID);

            if (objUserDetail != null)
            {
                FreeShipping objFreeShippingDetails = new FreeShipping();

                objFreeShippingDetails.IsFreeShippingActive = Convert.ToBoolean(objUserDetail.isFreeShippingActive);
                objFreeShippingDetails.IsSaleShipping = Convert.ToBoolean(objUserDetail.IsSaleShipping);
                objFreeShippingDetails.MinimumShippingAmount = Convert.ToDecimal(objUserDetail.MinimumShippingAmount);
                objFreeShippingDetails.ShippingPercentOfSale = Convert.ToDecimal(objUserDetail.ShippiingPercentOfSale);
                objFreeShippingDetails.ShippingProgramEndDate = Convert.ToDateTime(objUserDetail.ShippingProgramEndDate);
                objFreeShippingDetails.ShippingProgramFor = Convert.ToString(objUserDetail.shippingprogramfor);
                objFreeShippingDetails.ShippingProgramStartDate = Convert.ToDateTime(objUserDetail.ShippingProgramStartDate);
                objFreeShippingDetails.TotalSaleAbove = Convert.ToDecimal(objUserDetail.totalsaleabove);

                try
                {
                    if (objFreeShippingDetails != null)
                    {
                        Boolean isShippingAmountApplicable = false;

                        // Check if Free Shipping is active or not
                        if (objFreeShippingDetails.IsFreeShippingActive)
                        {
                            // Check if Free Shipping program is valid (in promotion)
                            if (objFreeShippingDetails.ShippingProgramStartDate < DateTime.Now && objFreeShippingDetails.ShippingProgramEndDate > DateTime.Now)
                            {
                                switch (objFreeShippingDetails.ShippingProgramFor.ToLower())
                                {
                                    case "admin": //For Admin
                                        if (objUserInfo.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                                            // Check if Sale Shipping is applicable
                                            isShippingAmountApplicable = objFreeShippingDetails.IsSaleShipping;
                                        break;

                                    case "employee": // For Employee
                                        if (objUserInfo.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                                            // Check if Sale Shipping is applicable
                                            isShippingAmountApplicable = objFreeShippingDetails.IsSaleShipping;
                                        break;

                                    case "both": // For Both
                                        // Check if Sale Shipping is applicable
                                        isShippingAmountApplicable = objFreeShippingDetails.IsSaleShipping;
                                        break;
                                    case "bothunticked":
                                        isShippingAmountApplicable = false;
                                        break;
                                    default:
                                        // Check if Sale Shipping is applicable                            
                                        isShippingAmountApplicable = objFreeShippingDetails.IsSaleShipping;
                                        break;
                                }
                            }
                            else
                                // Check if Sale Shipping is applicable
                                isShippingAmountApplicable = objFreeShippingDetails.IsSaleShipping;
                        }
                        else // Free Shipping Inactive so calculate Shipping charge
                            // Check if Sale Shipping is applicable
                            isShippingAmountApplicable = objFreeShippingDetails.IsSaleShipping;

                        if (isShippingAmountApplicable)
                        {
                            // Compare OrderAmount with TotalSalesAbove (Shipping)
                            if (orderAmount <= objFreeShippingDetails.TotalSaleAbove)
                                shippingAmount = objFreeShippingDetails.MinimumShippingAmount; // Shipping amount = only minimum shipping amount
                            else
                                shippingAmount = objFreeShippingDetails.MinimumShippingAmount + ((orderAmount - objFreeShippingDetails.TotalSaleAbove) * objFreeShippingDetails.ShippingPercentOfSale / 100.0M);
                        }
                        else
                        {
                            this.isFreeShipping = true;
                        }
                    }

                    shippingAmount = Decimal.Round(shippingAmount, 2);
                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex);
                }
            }

            #endregion

            // For Shipping Address
            UserAddress objShippingAddress = GetAddress(shippingAddress, "ShipTo", objUserInfo.UserInfoID, OrderIDNew, "Shipping");

            #region Tax Calculation

            //tax calculation
            //Don't go by name, names can be deceiving...!!! ;)
            if (objShippingAddress.Country.ToUpper() == "UNITED STATES" || objShippingAddress.Country.ToUpper() == "CANADA")
            {
                if (objShippingAddress.Country.ToUpper() == "UNITED STATES")
                {
                    //For the United States, method GetSalesTax is used not only to calculate sales tax but also for getting the county name from the zip code...
                    GetSalesTax(objShippingAddress.Country, objShippingAddress.State, objShippingAddress.ZipCode, Convert.ToDecimal(objOrder.OrderAmount), shippingAmount);
                }
                else if (objShippingAddress.Country.ToUpper() == "CANADA")
                {
                    //Taxes levied on all provinces of Canada, other than Alberta, Nunavut, Northwest Territories and Yukon Territory
                    if (objShippingAddress.State.ToUpper() != "ALBERTA" && objShippingAddress.State.ToUpper() != "NUNAVUT" && objShippingAddress.State.ToUpper() != "NORTHWEST TERRITORIES" && objShippingAddress.State.ToUpper() != "YUKON TERRITORY")
                        GetSalesTax(objShippingAddress.Country, objShippingAddress.State, objShippingAddress.ZipCode, Convert.ToDecimal(objOrder.OrderAmount), shippingAmount);
                }
            }

            OrderConfirmationRepository objOrderRepo = new OrderConfirmationRepository();
            Order objUpdateOrder = objOrderRepo.GetByOrderID(OrderIDNew);

            objUpdateOrder.SalesTax = this.SalesTaxAmount;
            objUpdateOrder.MOASSalesTax = this.SalesTaxAmount;
            objUpdateOrder.ShippingAmount = shippingAmount;
            objUpdateOrder.StrikeIronTaxRate = this.StrikeIronTaxRate;
            objUpdateOrder.StrikeIronResponseFileName = this.StrikeIronResponseFileName;

            objOrderRepo.SubmitChanges();

            #endregion

            #region Insert Shipping and Billing Address

            objShippingAddress.County = this.County;
            objShippingAddress.Name = shipToName;

            SetAddress(objShippingAddress);

            // For billing Address
            UserAddress objBillingAddress = GetAddress(billingAddress, "BillTo", objUserInfo.UserInfoID, OrderIDNew, "Billing");
            SetAddress(objBillingAddress);

            #endregion

            #region Update Shopping Cart

            foreach (var item in myCardIDs.Split(','))
            {
                MyShoppinCart objShopCart = objShoppingCartRepository.GetById(Convert.ToInt32(item), objUserInfo.UserInfoID);

                if (objShopCart != null)
                {
                    objShopCart.IsOrdered = true;
                    objShopCart.OrderID = Convert.ToInt64(OrderIDNew);
                    objShoppingCartRepository.SubmitChanges();
                }
            }

            #endregion

            // Send Mail 
            sendVerificationEmail(OrderIDNew, objBillingAddress, objShippingAddress);
            IsProcesSuccess = true;

            //Transmit to SAP
            new SAPOperations().SubmitOrderToSAP(OrderIDNew);

            if (this.StrikeIronResponseFailed)
            {
                String Subject = "World-Link (" + (CommonMails.Live ? "live" : "test") + ") : Stike-Iron Response failed for order # " + objOrder.OrderNumber;
                StringBuilder Body = new StringBuilder("Strike Iron Response has failed for World-Link (" + (CommonMails.Live ? "live" : "test") + ") order # " + objOrder.OrderNumber + ".");
                Body.Append("<br/><br/>The ship to zip code entered by the user was " + this.ShipToZipCodePassedToStrikeIron + ".");
                Body.Append("<br/><br/>Attached is the strike Iron response in XML format.");
                Body.Append("<br/><br/>Thanks");
                Body.Append("<br/>Wolrd-Link System");
                Body.Append("<br/><br/><b>Note :</b> Do not reply to this email, it has been sent from an un-monitored email address.");

                new CommonMails().SendMailWithAttachment(0, "Stike Iron Response Failed", CommonMails.EmailFrom, Convert.ToString(ConfigurationManager.AppSettings["StrikeIronFailedNotifyList"]), Subject, Body.ToString(), CommonMails.DisplyName, CommonMails.SSL, true, Path.Combine(Common.StrikeIronResponsePath, this.StrikeIronResponseFileName), CommonMails.SMTPHost, CommonMails.SMTPPort, CommonMails.UserName, CommonMails.Password, this.StrikeIronResponseFileName, CommonMails.Live);

                this.StrikeIronResponseFailed = false;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
        }
        return IsProcesSuccess;
    }

    /// <summary>
    /// Get Shipping and Billing Address
    /// </summary>
    /// <param name="_xmlString"></param>
    /// <param name="nodeType"></param>
    /// <param name="_userInfoID"></param>
    /// <param name="_orderID"></param>
    /// <param name="_contactInfoType"></param>
    /// <returns> UserAddress</returns>
    private UserAddress GetAddress(String _xmlString, String nodeType, Int64 _userInfoID, Int64 _orderID, String _contactInfoType)
    {
        UserAddress objUserAddress = new UserAddress();
        String _address = String.Empty;
        String _userName = String.Empty;
        String _emailAddress = String.Empty;
        String _companyName = "ASIG";//String.Empty;
        String _city = String.Empty;
        Int64? _cityID = null;
        String _state = String.Empty;
        String _stateCode = String.Empty;
        Int64? _stateID = null;
        String _country = String.Empty;
        String _countryCode = String.Empty;
        Int64? _countryID = null;
        String _zipCode = String.Empty;

        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(_xmlString);

        //Email
        XmlNodeList emailNode = xDoc.SelectNodes(nodeType + "/Address/Email");
        if (emailNode.Count > 0)
            _emailAddress = emailNode[0].InnerText;

        //Name
        XmlNodeList userNameNode = xDoc.SelectNodes(nodeType + "/Address/PostalAddress/DeliverTo");
        if (userNameNode.Count > 0)
            _userName = userNameNode[0].InnerText;

        //Address
        XmlNodeList AddressNode = xDoc.SelectNodes(nodeType + "/Address/PostalAddress/Street");
        if (AddressNode.Count > 0)
            for (Int32 i = 0; i < AddressNode.Count; i++)
            {
                _address += AddressNode[i].InnerText + ", ";
            }

        //ZipCode
        XmlNodeList zipCodeNode = xDoc.SelectNodes(nodeType + "/Address/PostalAddress/PostalCode");
        if (zipCodeNode.Count > 0)
            _zipCode = zipCodeNode[0].InnerText;

        //Country
        XmlNodeList countryNode = xDoc.SelectNodes(nodeType + "/Address/PostalAddress/Country");
        if (countryNode.Count > 0)
        {
            _country = countryNode[0].InnerText;

            INC_Country objCountry = new CountryRepository().GetByCountryName(_country);
            if (objCountry != null)
            {
                _countryID = objCountry.iCountryID;
                _countryCode = objCountry.sCode;
            }
        }

        //State
        XmlNodeList stateNode = xDoc.SelectNodes(nodeType + "/Address/PostalAddress/State");
        if (stateNode.Count > 0)
        {
            _state = stateNode[0].InnerText;

            if (_countryID != null)
            {
                INC_State objState = new StateRepository().GetByCountryIDAndStateCode(Convert.ToInt64(_countryID), _state);
                if (objState != null)
                {
                    _stateID = objState.iStateID;
                    _stateCode = objState.sCode;
                }
            }
        }

        //City
        XmlNodeList cityNode = xDoc.SelectNodes(nodeType + "/Address/PostalAddress/City");
        if (cityNode.Count > 0)
        {
            _city = cityNode[0].InnerText;

            if (_countryID != null && _stateID != null)
            {
                INC_City objCity = new CityRepository().CheckIfExist(Convert.ToInt64(_countryID), Convert.ToInt64(_stateID), _city);
                if (objCity != null)
                    _cityID = objCity.iCityID;
            }
        }

        // Set Address
        objUserAddress.UserInfoID = _userInfoID;
        objUserAddress.Name = _userName;
        objUserAddress.CompanyName = _companyName;
        objUserAddress.Address = _address;
        objUserAddress.City = _city;
        objUserAddress.CityID = _cityID;
        objUserAddress.State = _state;
        objUserAddress.StateCode = _stateCode;
        objUserAddress.StateID = _stateID;
        objUserAddress.Country = _country;
        objUserAddress.CountryCode = _countryCode;
        objUserAddress.CountryID = _countryID;
        objUserAddress.ZipCode = _zipCode;
        objUserAddress.EmailAddress = _emailAddress;
        objUserAddress.OrderID = _orderID;
        objUserAddress.ContactInfoType = _contactInfoType;
        objUserAddress.OrderType = "ShoppingCart";
        return objUserAddress;

    }

    /// <summary>
    /// To Insert Records Company Employee Contact Info 
    /// </summary>
    /// <param name="objAddress"></param>
    private void SetAddress(UserAddress objAddress)
    {
        CompanyEmployeeContactInfo objCEAddress = new CompanyEmployeeContactInfo()
        {
            UserInfoID = objAddress.UserInfoID,

            Name = objAddress.Name,
            CompanyName = objAddress.CompanyName,
            Address = objAddress.Address,
            Address2 = (objAddress.CityID == null ? objAddress.City : "") + (objAddress.StateID == null ? (", " + objAddress.State) : "") + (objAddress.CountryID == null ? (", " + objAddress.Country) : ""),
            CountryID = objAddress.CountryID,
            StateID = objAddress.StateID,
            CityID = objAddress.CityID,
            ZipCode = objAddress.ZipCode,
            Email = objAddress.EmailAddress,
            OrderID = objAddress.OrderID,
            ContactInfoType = objAddress.ContactInfoType,
            OrderType = objAddress.OrderType,
            county = objAddress.County
        };
        objCompanyEmployeeContactInfoRepository.Insert(objCEAddress);
        objCompanyEmployeeContactInfoRepository.SubmitChanges();
    }

    /// <summary>
    /// Check Coupa Order ID exist in Order table, then return false
    /// </summary>
    /// <param name="CoupaOrderID"></param>
    /// <returns>true/false</returns>
    private Boolean IsCoupaOrderIDExist(String coupaOrderID)
    {
        Boolean isIDExist = false;
        Order objOrder = objOrderConfirmationRepository.GetOrderDetailsByCoupaOrderID(coupaOrderID);
        if (objOrder != null)
            isIDExist = true;
        return isIDExist;
    }

    private void GetSalesTax(String countryName, String stateName, String zipCode, Decimal totalAmount, Decimal shippingAmount)
    {
        try
        {
            /// The web service definition for used in the Web Reference for this project is located at http://ws.strikeiron.com/taxdatabasic5?WSDL
            /// You can view more information about this web service here: https://strikeiron.com/ProductDetail.aspx?p=444

            /// Variables storing authentication values are declared below.  As a Registered StrikeIron user, you can authenticate
            /// to a StrikeIron web service with either a UserID/Password combination or a License Key.  If you wish to use a
            /// License Key, assign this value to the UserID field and set the Password field to null.
            /// 
            String userID = ConfigurationSettings.AppSettings["StrikeironUserID"];
            String password = ConfigurationSettings.AppSettings["StrikeironPassword"];

            /// To access the web service operations, you must declare a web service client Object.  This Object will contain
            /// all of the methods available in the web service and properties for each portion of the SOAP header.
            /// The class name for the web service client Object (assigned automatically by the Web Reference) is TaxDataBasic.
            /// 
            TaxDataBasic siService = new TaxDataBasic();

            /// StrikeIron web services accept user authentication credentials as part of the SOAP header.  .NET web service client
            /// objects represent SOAP header values as public fields.  The name of the field storing the authentication credentials
            /// is LicenseInfoValue (class type LicenseInfo).
            /// 
            LicenseInfo authHeader = new LicenseInfo();

            /// Registered StrikeIron users pass authentication credentials in the RegisteredUser section of the LicenseInfo Object.
            /// (property name: RegisteredUser; class name: RegisteredUser)
            /// 
            RegisteredUser regUser = new RegisteredUser();

            /// Assign credential values to this RegisteredUser Object
            /// 
            regUser.UserID = userID;
            regUser.Password = password;

            /// The populated RegisteredUser Object is now assigned to the LicenseInfo Object, which is then assigned to the web
            /// service client Object.
            /// 
            authHeader.RegisteredUser = regUser;
            siService.LicenseInfoValue = authHeader;

            /// Inputs for the GetTaxRateUS operation are declared below.
            ///             

            if (countryName.ToUpper() == "UNITED STATES")
            {
                zipCode = zipCode.Contains("-") ? zipCode.Substring(0, zipCode.IndexOf("-")) : zipCode.Length == 9 ? zipCode.Substring(0, 5) : zipCode;

                /// The GetTaxRateUS operation can now be called.  The output type for this operation is SIWSOutputOfTaxRateUSAData.
                /// Note that for simplicity, there is no error handling in this sample project.  In a production environment, any
                /// web service call should be encapsulated in a try-catch block.
                /// 
                SIWsOutputOfTaxRateUSAData wsOutput = siService.GetTaxRateUS(zipCode);

                this.StrikeIronResponseFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_Strike_Iron_Response.xml";
                Common.SaveXMLFromObject(wsOutput, Path.Combine(Common.StrikeIronResponsePath, this.StrikeIronResponseFileName));

                /// The output objects of this StrikeIron web service contains two sections: ServiceStatus, which stores data
                /// indicating the success/failure status of the the web service request; and ServiceResult, which contains the
                /// actual data returne as a result of the request.
                /// 
                /// ServiceStatus contains two elements - StatusNbr: a numeric status code, and StatusDescription: a String
                /// describing the status of the output Object.  As a standard, you can apply the following assumptions for the value of
                /// StatusNbr:
                ///   200-299: Successful web service call (data found, etc...)
                ///   300-399: Nonfatal error (No data found, etc...)
                ///   400-499: Error due to invalid input
                ///   500+: Unexpected internal error; contact support@strikeiron.com
                if (wsOutput.ServiceStatus.StatusNbr >= 300)
                {
                    this.StrikeIronResponseFailed = true;
                    this.ShipToZipCodePassedToStrikeIron = zipCode;
                    return;
                }

                if (stateName.ToUpper() == "FLORIDA" || stateName.ToUpper() == "CONNECTICUT")
                {
                    this.StrikeIronTaxRate = Convert.ToDecimal(wsOutput.ServiceResult.TotalSalesTax);
                    this.SalesTaxAmount = Decimal.Round((totalAmount + shippingAmount) * this.StrikeIronTaxRate, 2);
                }

                this.County = Convert.ToString(wsOutput.ServiceResult.County);
            }
            else if (countryName.ToUpper() == "CANADA")
            {
                if (stateName.ToUpper() != "ALBERTA" || stateName.ToUpper() != "NUNAVUT" || stateName.ToUpper() != "NORTHWEST TERRITORIES" || stateName.ToUpper() != "YUKON TERRITORY")
                {
                    /// Inputs for the GetTaxRateUS operation are declared below.

                    /// The GetTaxRateCanada operation can now be called.  The output type for this operation is SIWsOutputOfTaxRateCanadaData.
                    /// Note that for simplicity, there is no error handling in this sample project.  In a production environment, any
                    /// web service call should be encapsulated in a try-catch block.
                    /// 
                    SIWsOutputOfTaxRateCanadaData wsOutput = siService.GetTaxRateCanada(stateName);

                    this.StrikeIronResponseFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_Strike_Iron_Response.xml";
                    Common.SaveXMLFromObject(wsOutput, Path.Combine(Common.StrikeIronResponsePath, this.StrikeIronResponseFileName));

                    /// The output objects of this StrikeIron web service contains two sections: ServiceStatus, which stores data
                    /// indicating the success/failure status of the the web service request; and ServiceResult, which contains the
                    /// actual data returne as a result of the request.
                    /// 
                    /// ServiceStatus contains two elements - StatusNbr: a numeric status code, and StatusDescription: a String
                    /// describing the status of the output Object.  As a standard, you can apply the following assumptions for the value of
                    /// StatusNbr:
                    ///   200-299: Successful web service call (data found, etc...)
                    ///   300-399: Nonfatal error (No data found, etc...)
                    ///   400-499: Error due to invalid input
                    ///   500+: Unexpected internal error; contact support@strikeiron.com
                    if (wsOutput.ServiceStatus.StatusNbr >= 300)
                    {
                        this.StrikeIronResponseFailed = true;
                        this.ShipToZipCodePassedToStrikeIron = zipCode;
                        return;
                    }
                    else
                    {
                        this.StrikeIronTaxRate = Convert.ToDecimal(wsOutput.ServiceResult.GST);
                        this.SalesTaxAmount = Decimal.Round((totalAmount + shippingAmount) * this.StrikeIronTaxRate, 2);
                    }
                }

                //this.County = Convert.ToString(wsOutput.ServiceResult.Abbreviation);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendVerificationEmail(Int64 OrderID, UserAddress objBillingAddress, UserAddress objShippingAddress)
    {
        try
        {
            CityRepository objCity = new CityRepository();
            StateRepository objState = new StateRepository();
            CountryRepository objCountry = new CountryRepository();
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Coupa Order";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                Order objOrder = objOrderConfirmationRepository.GetByOrderID(OrderID);
                UserInformation objUserInformation = objUserInformationRepository.GetById(objOrder.UserId);
                CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID);

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                String sSubject = objCompanyRepository.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber.ToString();
                String sToadd = String.Empty;//"incentextest10@gmail.com"; //objUserInformation.LoginEmail;

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());


                messagebody.Replace("{firstnote}", "Thank you for your business you can check the status of your order(s) any time by logging into your store <a href='http://www.world-link.us.com' title='Incentex'>here.</a>");

                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName + "<br/>Workgroup : " + objLookupRepository.GetById(objCompanyEmployee.WorkgroupID).sLookupName);
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
                messagebody.Replace("{FullName}", objUserInformation.FirstName);
                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());



                messagebody.Replace("{PaymentType}", "Paid By Corporate");
                //Anniversary Credit Row
                messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                messagebody.Replace("{lblStCrAmntApplied}", String.Empty);

                //Payment Option Row
                messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);

                messagebody.Replace("{Payment Method :}", "Payment Method :");


                #region Billing Address
                messagebody.Replace("{BillingAddressName}", objBillingAddress.Name);
                messagebody.Replace("{BillingAddressCompanyName}", objBillingAddress.CompanyName);
                messagebody.Replace("{BillingAddressAddress}", objBillingAddress.Address + " " + objBillingAddress.City);
                messagebody.Replace("{BillingAddressState}", objBillingAddress.State.ToUpper());
                messagebody.Replace("{BillingAddressCountry}", objBillingAddress.Country);
                messagebody.Replace("{BillingAddressZipCode}", objBillingAddress.ZipCode);
                #endregion

                #region shipping address
                messagebody.Replace("{ShippingAddressName}", objShippingAddress.Name);
                messagebody.Replace("{ShippingAddressCompanyName}", objShippingAddress.CompanyName);
                messagebody.Replace("{ShippingAddressAddress}", objShippingAddress.Address + " " + objShippingAddress.City);
                messagebody.Replace("{ShippingAddressState}", objShippingAddress.State.ToUpper());
                messagebody.Replace("{ShippingAddressCountry}", objShippingAddress.Country);
                messagebody.Replace("{ShippingAddressZipCode}", objShippingAddress.ZipCode);
                messagebody.Replace("{ShippingAddressEmailAddress}", objShippingAddress.EmailAddress);

                #endregion

                #region bind item detail
                String innermessage = "";
                Boolean GLCodeExists = false;

                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                    GLCodeExists = objSelectMyShoppingCartProductResultList.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                    {
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Quantity;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].item;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Size;
                        innermessage = innermessage + "</td>";
                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].GLCode;
                            innermessage = innermessage + "</td>";
                        }
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice) * Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].Quantity);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";

                        if (GLCodeExists)
                            innermessage = innermessage + "<td colspan='8'>";
                        else
                            innermessage = innermessage + "<td colspan='7'>";

                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                    }
                    messagebody.Replace("{innermessage}", innermessage);

                    messagebody.Replace("{ShippingCost}", "");
                    messagebody.Replace("{Saletax}", objOrder.SalesTax.ToString());
                    messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());
                    messagebody.Replace("{CorporateDiscountView}", "");
                    messagebody.Replace("{CorporateDiscount}", "");
                }


                if (GLCodeExists)
                    messagebody.Replace("<td id=\"tdPD\" width=\"48%\"", "<td id=\"tdPD\" width=\"38%\"");
                else
                    messagebody.Replace(@"<td id=""tdGC"" width=""10%"" style=""font-weight: bold; text-align: center;"">
                                                    GL Code
                                                </td>", "");

                #endregion

                messagebody.Replace("{innermesaageforsupplier}", "");
                messagebody.Replace("{Order Notes:}", "Order Notes :");
                messagebody.Replace("{OrderTotalView}", "Order Total :");
                messagebody.Replace("{OrderNotes}", "Order Coupa");
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                String[] ArrayToAdd = { "orders@incentex.com", "incentexorders@worldfulfillment.com" }; //Ken given this email
                if (HttpContext.Current.Request.IsLocal || IsTestOrder)// Local test
                    new CommonMails().SendEmail4Local(1092, "Testing From Coupa", "surendar.yadav@indianic.com", sSubject, messagebody.ToString(), "incentextest6@gmail.com", "test6incentex", true);
                else
                {
                    foreach (var _itemToAdd in ArrayToAdd)
                    {
                        new CommonMails().SendMail(objUserInformation.UserInfoID, "Order Confirmation", sFrmadd, _itemToAdd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                    }
                }

            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
        }
    }

    #endregion

    #region Custom Classes

    public class UserAddress
    {
        public Int64 UserInfoID { get; set; }
        public String Name { get; set; }
        public String CompanyName { get; set; }
        public String Address { get; set; }
        public String Country { get; set; }
        public String CountryCode { get; set; }
        public Int64? CountryID { get; set; }
        public String State { get; set; }
        public String StateCode { get; set; }
        public Int64? StateID { get; set; }
        public String City { get; set; }
        public Int64? CityID { get; set; }
        public String ZipCode { get; set; }
        public String County { get; set; }
        public String EmailAddress { get; set; }
        public Int64 OrderID { get; set; }
        public String ContactInfoType { get; set; }
        public String OrderType { get; set; }
    }

    #endregion
}