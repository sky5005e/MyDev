//created by mayur for sending reminder email to moas manager related to order.
//this page is loading from window schedular
//Module Name : MOAS System

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class controller_MOASReminderSystem : System.Web.UI.Page
{
    #region DataMembers
    UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
    OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    CompanyRepository objCmpRepo = new CompanyRepository();
    CompanyEmployeeContactInfo objShippingInfo;
    CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfo();
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    List<SelectMyShoppingCartProductResult> obj;
    MyIssuanceCartRepository objMyIssCartRepo = new MyIssuanceCartRepository();
    List<MyIssuanceCart> objMyIssCart = new List<MyIssuanceCart>();
    AppSettingRepository objAppSettingRep = new AppSettingRepository();
    #endregion

    #region Event Handlers
    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            INC_AppSetting objAppSettingReminderDay = objAppSettingRep.GetbyName("MOASREMINDERDAY");
            INC_AppSetting objAppSettingLastReminderDate = objAppSettingRep.GetbyName("MOASLASTREMINDERDATE");
            if (Convert.ToDateTime(objAppSettingLastReminderDate.sSettingValue).AddDays(Convert.ToInt16(objAppSettingReminderDay.sSettingValue)).Date == DateTime.Now.Date)
            {
                List<Order> lstOrders = objRepos.GetOrderForMOASReminderSystem();
                if (lstOrders != null)
                {
                    foreach (Order objOrder in lstOrders)
                    {
                        sendEmailToMOASManager(objOrder.OrderID, Convert.ToInt64(objOrder.UserId));
                    }
                }
                INC_AppSetting objAppSetting = new INC_AppSetting();
                objAppSetting = objAppSettingRep.GetbyName("MOASLASTREMINDERDATE");
                objAppSetting.sSettingValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFF");
                objAppSetting.dLastupdatedate = System.DateTime.Now;
                objAppSettingRep.SubmitChanges();
            }
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Sends the email to MOAS managers.
    /// Add By Mayur on 22-Nov-2011
    /// </summary>
    /// <param name="OrderID">The order id.</param>
    private void sendEmailToMOASManager(Int64 OrderID, Int64 userInfoID)
    {
        List<OrderMOASSystem> objOrderMOASSystemList = new OrderMOASSystemRepository().GetByOrderIDAndStatus(OrderID, "Order Pending");

        if (objOrderMOASSystemList != null && objOrderMOASSystemList.Count > 0)
        {
            UserInformation objUserInformation = new UserInformation();
            objUserInformation = objUsrInfoRepo.GetById(objOrderMOASSystemList[0].ManagerUserInfoID);
            sendVerificationEmailMOASManager(OrderID, objUserInformation.LoginEmail, objUserInformation.FirstName, objUserInformation.UserInfoID);
        }
    }

    /// <summary>
    /// Sends the verification email MOAS manager for approve order.
    /// Add By Mayur on 22-Nov-2011
    /// </summary>
    /// <param name="OrderID">The order id.</param>
    /// <param name="MOASEmailAddress">The MOAS email address.</param>
    /// <param name="FullName">The full name.</param>
    private void sendVerificationEmailMOASManager(Int64 OrderId, String MOASEmailAddress, String FullName, Int64 MOASUserId)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Placed MOAS";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                Order objOrder = objRepos.GetByOrderID(OrderId);
                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetByUserInfoId(objUserInformation.UserInfoID);

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString().Replace("{FullName}", objUserInformation.FirstName + " " + objUserInformation.LastName).Replace("{OrderNumber}", objOrder.OrderNumber);
                String sToadd = MOASEmailAddress;

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());
                messagebody.Replace("{firstnote}", "Please review the following order below that requires your attention. You can Approve, Edit or Cancel this order by clicking the buttons on the bottom of this page.");
                messagebody.Replace("{FullName}", FullName);
                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName);
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());

                #region Check here for CreditUsed
                String cr;
                Boolean creditused = true;
                if (objOrder.CreditUsed != "0")
                {
                    if (String.IsNullOrEmpty(objOrder.CreditUsed))
                    {
                        creditused = false;
                    }
                    else
                    {
                        creditused = true;
                    }
                }
                else
                {
                    creditused = false;
                }
                #endregion

                #region PaymentType and PaymentMethod
                //Both Option have used
                if (objOrder.PaymentOption != null && creditused)
                {

                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits Paid by Corporate ";
                        /*Strating Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", objOrder.CreditAmt.ToString());
                        /*End*/
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits Paid by Corporate ";
                        /*Anniversary Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                        /*End*/

                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                }
                //Only Starting or Anniversary
                else if (objOrder.PaymentOption == null && creditused)
                {
                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr);
                }
                //Only Payment Option
                else if (objOrder.PaymentOption != null && !creditused)
                {
                    messagebody.Replace("{PaymentType}", new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                }
                else
                {
                    messagebody.Replace("{PaymentType}", "Paid By Corporate");
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/

                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                }
                messagebody.Replace("{Payment Method :}", "Payment Method :");
                #endregion

                #region Billing Address
                if (objOrder.OrderFor == "ShoppingCart") // Company Pays
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                else
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                messagebody.Replace("{Billing Address}", "Bill To:");
                messagebody.Replace("{NameView}", "Name :");
                messagebody.Replace("{CompanyNameView}", "Company Name :");
                messagebody.Replace("{AddressView}", "Address :");
                // messagebody.Replace("{Address2}", lblBadr.Text);
                messagebody.Replace("{CityView}", "City :");
                messagebody.Replace("{StateView}", "State :");
                messagebody.Replace("{ZipView}", "Zip :");
                messagebody.Replace("{CountryView}", "Country :");
                messagebody.Replace("{PhoneView}", "Phone :");
                messagebody.Replace("{EmailView}", "Email :");

                messagebody.Replace("{Name}", objBillingInfo.BillingCO + " " + objBillingInfo.Manager);
                messagebody.Replace("{CompanyName}", objBillingInfo.CompanyName);
                messagebody.Replace("{Address1}", objBillingInfo.Address);
                // messagebody.Replace("{Address2}", lblBadr.Text);
                messagebody.Replace("{City}", objCity.GetById((Int64)objBillingInfo.CityID).sCityName);
                messagebody.Replace("{State}", objState.GetById((Int64)objBillingInfo.StateID).sStatename);
                messagebody.Replace("{Zip}", objBillingInfo.ZipCode);
                messagebody.Replace("{Country}", objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone}", objBillingInfo.Telephone);
                messagebody.Replace("{Email}", objBillingInfo.Email);
                #endregion

                #region shipping address
                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                }
                else
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                }
                messagebody.Replace("{Shipping Address}", "Ship To:");
                messagebody.Replace("{AirportView1}", "Airport :");
                messagebody.Replace("{DepartmentView1}", "Department :");
                messagebody.Replace("{NameView1}", "Name :");
                messagebody.Replace("{CompanyNameView1}", "Company Name :");
                messagebody.Replace("{AddressView1}", "Address :");
                // messagebody.Replace("{Address2}", lblBadr.Text);
                messagebody.Replace("{CityView1}", "City :");
                messagebody.Replace("{CountyView1}", "County :");
                messagebody.Replace("{StateView1}", "State :");
                messagebody.Replace("{ZipView1}", "Zip :");
                messagebody.Replace("{CountryView1}", "Country :");
                messagebody.Replace("{PhoneView1}", "Phone :");
                messagebody.Replace("{EmailView1}", "Email :");

                messagebody.Replace("{Airport1}", objShippingInfo.Airport);
                if (objShippingInfo.DepartmentID != null)
                    messagebody.Replace("{Department1}", new LookupRepository().GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
                else
                    messagebody.Replace("{Department1}", "");
                messagebody.Replace("{Name1}", objShippingInfo.Name + " " + objShippingInfo.Fax);
                messagebody.Replace("{CompanyName1}", objShippingInfo.CompanyName);
                messagebody.Replace("{Address11}", objShippingInfo.Address + "<br/>" + objShippingInfo.Address2 + "<br/>" + objShippingInfo.Street);
                messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
                messagebody.Replace("{County1}", objShippingInfo.county);
                messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
                messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
                messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
                messagebody.Replace("{Email1}", objShippingInfo.Email);
                #endregion

                #region order product detail
                String innermessage = "";
                if (objOrder.OrderFor == "ShoppingCart")
                {
                    List<SelectMyShoppingCartProductResult> obj;
                    obj = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                    for (Int32 i = 0; i < obj.Count; i++)
                    {

                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(obj[i].NameToBeEngraved))
                        {
                            p = obj[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + obj[i].Quantity;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + obj[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + obj[i].Size;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + obj[i].ProductDescrption1;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (obj[i].MOASUnitPrice != null ? Convert.ToDecimal(obj[i].MOASUnitPrice) : Convert.ToDecimal(obj[i].UnitPrice)).ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (obj[i].MOASUnitPrice != null ? Convert.ToDecimal(obj[i].MOASUnitPrice) : Convert.ToDecimal(obj[i].UnitPrice)) * Convert.ToDecimal(obj[i].Quantity);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td colspan='7'>";
                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }
                    messagebody.Replace("{innermessage}", innermessage);
                }
                else
                {
                    List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);
                    for (Int32 i = 0; i < objFinal.Count; i++)
                    {

                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objFinal[i].NameToBeEngraved))
                        {
                            p = objFinal[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].Qty;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].sLookupName;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + objFinal[i].ProductDescrption;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)).ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)) * Convert.ToDecimal(objFinal[i].Qty);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td colspan='7'>";
                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }

                    messagebody.Replace("{innermessage}", innermessage);

                }
                #endregion

                messagebody.Replace("{Order Notes:}", "Order Notes :");
                messagebody.Replace("{ShippingCostView}", "Shipping :");
                messagebody.Replace("{SalesTaxView}", "Sales Tax :");
                messagebody.Replace("{OrderTotalView}", "Order Total :");
                messagebody.Replace("{innermesaageforsupplier}", "");
                String b = NameBars(objOrder.OrderID);
                if (b != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + b);
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                }

                messagebody.Replace("{ShippingCost}", objOrder.ShippingAmount.ToString());
                messagebody.Replace("{Saletax}", "$" + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)).ToString());
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + (objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount))).ToString());

                if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)//check that corporate discount is applied or not
                {
                    messagebody.Replace("{CorporateDiscountView}", "Corporate Discount");
                    messagebody.Replace("{CorporateDiscount}", objOrder.CorporateDiscount.ToString());
                }
                else
                {
                    messagebody.Replace("{CorporateDiscountView}", "");
                    messagebody.Replace("{CorporateDiscount}", "");
                }
                //messagebody.Replace("{PurchasesNotEligableforCreditUse}", lblNotEligibleforAnniversarycredit.Text);


                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                //Set Conformation Button
                String buttonText = "";
                buttonText += "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                buttonText += "<tr>";
                buttonText += "<td>";
                buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "MOASOrderStatus.aspx?UserId=" + MOASUserId + "&OrderId=" + objOrder.OrderID + "&Status=Approve'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/approve_order_btn.png' alt='Approve Order' border='0'/></a>";
                buttonText += "<td>";
                buttonText += "<td>";
                buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "MOASOrderStatus.aspx?UserId=" + MOASUserId + "&OrderId=" + objOrder.OrderID + "&Status=Cancel'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/cancel_order_btn.png' alt='Cancel Order' border='0'/></a>";
                buttonText += "<td>";
                buttonText += "<td>";
                buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "MyAccount/OrderManagement/EditOrderDetail.aspx?Id=" + objOrder.OrderID + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/edit_order_btn.png' alt='Edit Order' border='0'/></a>";
                buttonText += "<td>";
                buttonText += "</tr>";
                buttonText += "</table>";

                messagebody.Replace("{ConformationButton}", buttonText);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                if (new ManageEmailRepository().CheckEmailAuthentication(MOASUserId, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.MOAS)) == true)
                {
                    new CommonMails().SendMail(MOASUserId, "MOAS From My Account", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private String NameBars(Int64 OrderId)
    {
        String strNameBars = String.Empty;
        Order objOrder = new OrderConfirmationRepository().GetByOrderID(OrderId);
        if (objOrder.OrderFor == "IssuanceCart")
        {
            List<MyIssuanceCart> objIssList = new MyIssuanceCartRepository().GetIssuanceCartByOrderID(objOrder.OrderID);
            foreach (MyIssuanceCart objItem in objIssList)
            {
                if (objItem.NameToBeEngraved != null && objItem.NameToBeEngraved != String.Empty)
                {
                    if (objItem.NameToBeEngraved.Contains(','))
                    {
                        String[] EmployeeTitle = objItem.NameToBeEngraved.Split(',');
                        strNameBars += "Name to be Engraved on Name Bar:" + EmployeeTitle[0] + "\n";
                        strNameBars += "Employee Title" + EmployeeTitle[1] + "\n";
                    }
                    else
                    {
                        strNameBars += "Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n";
                    }
                }
            }
        }
        else
        {
            List<SelectMyShoppingCartProductResult> objMyShpList = new MyShoppingCartRepository().SelectCurrentOrderProducts(objOrder.OrderID);
            foreach (SelectMyShoppingCartProductResult objItem in objMyShpList)
            {
                if (objItem.NameToBeEngraved != null && objItem.NameToBeEngraved != String.Empty)
                {
                    if (objItem.NameToBeEngraved.Contains(','))
                    {
                        String[] EmployeeTitle = objItem.NameToBeEngraved.Split(',');
                        strNameBars += "Name to be Engraved on Name Bar:" + EmployeeTitle[0] + "\n";
                        strNameBars += "Employee Title:" + EmployeeTitle[1] + "\n";
                    }
                    else
                    {
                        strNameBars += "Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n";
                    }
                }
            }
        }

        return strNameBars.ToString();

    }
    #endregion
}
