using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class controller_SendOrderNotificationtoIE : System.Web.UI.Page
{
    UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    CompanyEmployeeContactInfo objShippingInfo;
    CompanyEmployeeContactInfo objBillingInfo;
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
    CompanyRepository objCmpRepo = new CompanyRepository();
    CompanyStoreRepository objCompanyStoreRepository = new CompanyStoreRepository();
    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    MyIssuanceCartRepository objMyIssCartRepo = new MyIssuanceCartRepository();
    OrderMOASSystemRepository objOrderMOASSystemRepository = new OrderMOASSystemRepository();
    List<SelectMyShoppingCartProductResult> objSelectMyShoppingCartProductResultList;
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    LookupRepository objLookupRepository = new LookupRepository();

    Boolean IsMOASWithCostCenterCode
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsMOASWithCostCenterCode"]);
        }
        set
        {
            ViewState["IsMOASWithCostCenterCode"] = value;
        }
    }

    int SendEmailCount
    {
        get
        {
            return Convert.ToInt32(ViewState["SendEmailCount"]);
        }
        set
        {
            ViewState["SendEmailCount"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //string strOrderNumber = "2013002648,2013002686,2013002649,2013002687";
            //string[] strArys = strOrderNumber.Split(',');
            //SendEmailCount = 0;
            //foreach (var item in strArys)
            //{
            //    Int64 OrderID = objRepos.GetOrderIDByOrderNo(item);
            //    if (OrderID != null && OrderID != 0)
            //        sendIEEmail("Approved", OrderID);
            //}
            //lblCount.Text = SendEmailCount + " Email have been send";
        }
    }
    protected void BtnSendNotification_Click(object sender, EventArgs e)
    {
        if (txtOrderNumber.Text.Trim() != "")
        {
            var objOrder = objRepos.GetByOrderNo(txtOrderNumber.Text.Trim());
            if (objOrder != null)
                sendIEEmail("Approved", objOrder.OrderID);
        }
    }
    /// <summary>
    /// Sends the email to incentex admin.
    /// </summary>
    private void sendIEEmail(String strStatus, Int64 OrderID)
    {
        List<UserInformation> objAdminList = new List<UserInformation>();
        objAdminList = new UserInformationRepository().GetEmailInformation();
        if (objAdminList.Count > 0)
        {
            for (Int32 i = 0; i < objAdminList.Count; i++)
            {
                //sendVerificationEmailAdmin(strStatus, OrderID, objAdminList[i].LoginEmail, objAdminList[i].FirstName, objAdminList[i].UserInfoID, false);
                //sendVerificationEmailAdmin(strStatus, OrderID, "prashanth.kankhara@indianic.com", objAdminList[i].FirstName, objAdminList[i].UserInfoID, false);
            }
        }
    }

    /// <summary>
    /// Sends the verification email to incentex admin.
    /// </summary>
    /// <param name="strStatus">New order status.</param>
    /// <param name="OrderID">The order id.</param>
    /// /// <param name="ToAdd">receiver email address.</param>
    /// /// /// <param name="ToAdd">full name of receiver.</param>
    private void sendVerificationEmailAdmin(String strStatus, Int64 OrderID, String ToAdd, String FullName, Int64 ToUserInfoID, Boolean IsTestOrder)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Placed";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                Order objOrder = objRepos.GetByOrderID(OrderID);
                this.IsMOASWithCostCenterCode = objCompanyEmployeeRepository.FUN_IsMOASWithCostCenterCode(Convert.ToInt64(objOrder.UserId));

                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID);

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                String sSubject = objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber.ToString();
                String sToadd = "";
                sToadd = ToAdd;
                //sToadd = "mayur.rathod@indianic.com";

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());

                messagebody.Replace("{firstnote}", "Thank you for your business you can check the status of order(s) any time by logging into your store <a href='http://www.world-link.us.com' title='Incentex'>here.</a>");
                messagebody.Replace("{FullName}", FullName);
                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName);
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());
                //Check here for CreditUsed
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
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
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
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
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

                #region PaymentOptionCode
                //For displaying payment option code
                if (IsMOASWithCostCenterCode)
                {
                    messagebody.Replace("{PaymentOptionCodeLabel}", "Cost Center Code :");
                    messagebody.Replace("{PaymentOptionCodeValue}", objOrder.PaymentOptionCode);
                }
                else
                {
                    messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
                    messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);
                }
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

                #region start
                String innermessage = "";
                Boolean GLCodeExists = false;

                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                    GLCodeExists = objSelectMyShoppingCartProductResultList.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                    {
                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objSelectMyShoppingCartProductResultList[i].NameToBeEngraved))
                        {
                            p = objSelectMyShoppingCartProductResultList[i].NameToBeEngraved.Split(',');
                        }
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
                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)).ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)) * Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].Quantity);
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
                }
                else
                {
                    List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                    GLCodeExists = objFinal.FirstOrDefault(le => le.GLCode != null) != null;

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

                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].GLCode;
                            innermessage = innermessage + "</td>";
                        }

                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)).ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)) * Convert.ToDecimal(objFinal[i].Qty);

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

                }

                if (GLCodeExists)
                    messagebody.Replace("<td id=\"tdPD\" width=\"48%\"", "<td id=\"tdPD\" width=\"38%\"");
                else
                    messagebody.Replace(@"<td id=""tdGC"" width=""10%"" style=""font-weight: bold; text-align: center;"">
                                                    GL Code
                                                </td>", "");
                #endregion

                messagebody.Replace("{Order Notes:}", "Order Notes :");
                messagebody.Replace("{ShippingCostView}", "Shipping :");
                messagebody.Replace("{SalesTaxView}", "Sales Tax :");
                messagebody.Replace("{OrderTotalView}", "Order Total :");
                messagebody.Replace("{innermesaageforsupplier}", "");
                String b = NameBars(objOrder.OrderID);
                if (b != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + b + (IsTestOrder ? @"<br/><span style=""color:red;font-weight:bold;"">THIS IS A TEST ORDER. PLEASE DO NOT PROCESS.</span>" : ""));
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + (IsTestOrder ? @"<br/><span style=""color:red;font-weight:bold;"">THIS IS A TEST ORDER. PLEASE DO NOT PROCESS.</span>" : ""));
                }

                messagebody.Replace("{ShippingCost}", objOrder.ShippingAmount.ToString());
                messagebody.Replace("{Saletax}", "$" + objOrder.SalesTax.ToString());
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + (objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount))).ToString());
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
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                if (new ManageEmailRepository().CheckEmailAuthentication(ToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.MOAS)) == true)
                {
                    if (HttpContext.Current.Request.IsLocal)
                        new CommonMails().SendEmail4Local(0, "MOAS From Email", sToadd, sSubject, messagebody.ToString(), "incentextest6@gmail.com", "test6incentex", true);
                    else
                        new CommonMails().SendMail(ToUserInfoID, "MOAS From Email", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                    SendEmailCount++;
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
}
