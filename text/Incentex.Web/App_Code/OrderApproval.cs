using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Globalization;

/// <summary>
/// Summary description for OrderApproval
/// </summary>
public class OrderApproval
{
    #region Variable's
    UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
    OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    CompanyRepository objCmpRepo = new CompanyRepository();
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    CompanyEmployeeContactInfo objShippingInfo;
    CompanyEmployeeContactInfo objBillingInfo;
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    MyIssuanceCartRepository objMyIssCartRepo = new MyIssuanceCartRepository();
    OrderMOASSystemRepository objOrderMOASSystemRepository = new OrderMOASSystemRepository();
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    LookupRepository objLookupRepository = new LookupRepository();
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    List<SelectMyShoppingCartProductResult> objSelectMyShoppingCartProductResultList;
    #endregion
    public OrderApproval()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// Added by Prashant April 2013,
    /// To approve order by the admin (Common for both Gridview Approve and the button outside GridView
    /// </summary>
    /// <param name="objOrder"></param>
    /// <param name="objUser"></param>
    public String ApproveOrder(Int64 OrderID, Int64 ApproverID)
    {
        String userName = String.Empty;
        Order objOrder = objRepos.GetByOrderID(Convert.ToInt64(OrderID));
        UserInformation objUser = new UserInformationRepository().GetById(Convert.ToInt64(objOrder.UserId));
        if (!String.IsNullOrEmpty(objUser.FirstName) || !String.IsNullOrEmpty(objUser.LastName))
            userName = objUser.FirstName + " " + objUser.LastName;

        userName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(userName);

        OrderMOASSystem objOrderMOASSystem = objOrderMOASSystemRepository.GetByOrderIDAndManagerUserInfoID(objOrder.OrderID, ApproverID);
        objOrderMOASSystem.Status = "Open";
        objOrderMOASSystem.DateAffected = DateTime.Now;
        objOrderMOASSystemRepository.SubmitChanges();


        //updated on 4th dec by prashant
        //update price level if different from the global selection and send confirmation mail to the IE
        Int32 MOASDefaultPriceLevel = 0;

        GlobalMenuSettingRepository objGlobalMenuSettingsRepo = new GlobalMenuSettingRepository();
        var objDefaultPriceLevel = objGlobalMenuSettingsRepo.GetById(objOrder.WorkgroupId, Convert.ToInt64(objOrder.StoreID));
        if (objDefaultPriceLevel != null)
            MOASDefaultPriceLevel = Convert.ToInt32(objDefaultPriceLevel.MOASPaymentPricing);
        Decimal TotalOrderAmount = 0;



        //Update the Price Rate if Default Price Level for the workgroup is set

        if (MOASDefaultPriceLevel != 0 && objOrder.PaymentOption == (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS)
        {
            OrderDetailHistoryRepository objOrderDetailsRepo = new OrderDetailHistoryRepository();
            ProductItemPriceRepository objProductItemPriceRepo = new ProductItemPriceRepository();

            if (objOrder.OrderFor == Convert.ToString(Incentex.DAL.Common.DAEnums.OrderFor.ShoppingCart))
            {
                List<MyShoppinCart> objShoppingCart = objOrderDetailsRepo.GetShoppinCartDetailsByOrderID(objOrder.OrderID);
                foreach (var item in objShoppingCart)
                {
                    //Update the Price Rate if Default Price Level for the workgroup is different from the current pricelevel
                    if (item.PriceLevel != MOASDefaultPriceLevel)
                    {

                        Incentex.DAL.SqlRepository.ProductItemPriceRepository.ProductItemPricingResult objProductPricing = objProductItemPriceRepo.GetSingleProductItemPrice(item.ItemNumber, (Int32)item.StoreProductID);

                        Decimal ItemPrice = 0;
                        if (MOASDefaultPriceLevel == 1)
                            ItemPrice = objProductPricing.Level1;
                        else if (MOASDefaultPriceLevel == 2)
                            ItemPrice = objProductPricing.Level2;
                        else if (MOASDefaultPriceLevel == 3)
                            ItemPrice = objProductPricing.Level3;
                        else if (MOASDefaultPriceLevel == 4)
                            ItemPrice = objProductPricing.Level4;

                        item.MOASPriceLevel = MOASDefaultPriceLevel;
                        item.MOASUnitPrice = Convert.ToString(ItemPrice);
                        TotalOrderAmount += Decimal.Round((ItemPrice * Convert.ToInt32(item.Quantity)), 2);


                        objOrderDetailsRepo.SubmitChanges();
                    }
                }
            }
            else if (objOrder.OrderFor == Convert.ToString(Incentex.DAL.Common.DAEnums.OrderFor.IssuanceCart))
            {
                List<MyIssuanceCart> objIssuanceCart = objOrderDetailsRepo.GetIssuanceCartDetailsByOrderID(objOrder.OrderID);
                foreach (var item in objIssuanceCart)
                {
                    if (item.PriceLevel != MOASDefaultPriceLevel)
                    {
                        Incentex.DAL.SqlRepository.ProductItemPriceRepository.ProductItemPricingResult objProductPricing = objProductItemPriceRepo.GetSingleProductItemPrice(item.ItemNumber, (Int32)item.StoreProductID);

                        Decimal ItemPrice = 0;
                        if (MOASDefaultPriceLevel == 1)
                            ItemPrice = objProductPricing.Level1;
                        else if (MOASDefaultPriceLevel == 2)
                            ItemPrice = objProductPricing.Level2;
                        else if (MOASDefaultPriceLevel == 3)
                            ItemPrice = objProductPricing.Level3;
                        else if (MOASDefaultPriceLevel == 4)
                            ItemPrice = objProductPricing.Level4;

                        item.MOASPriceLevel = MOASDefaultPriceLevel;
                        item.MOASRate = ItemPrice;
                        TotalOrderAmount += Decimal.Round((ItemPrice * Convert.ToInt32(item.Qty)), 2);

                        objOrderDetailsRepo.SubmitChanges();
                    }
                }
            }
            if (TotalOrderAmount != 0)
            {
                objOrder.MOASOrderAmount = TotalOrderAmount;
                objOrder.MOASSalesTax = Decimal.Round((TotalOrderAmount + Convert.ToDecimal(objOrder.ShippingAmount)) * Convert.ToDecimal(objOrder.StrikeIronTaxRate), 2);
                objRepos.SubmitChanges();
            }
        }

        //updated on prashant april 2013
        String ApproverLevel = objCompanyEmployeeRepository.GetWorkGroupMOASApproverLevel(objOrder.WorkgroupId, objUser.CompanyId);
        if (!String.IsNullOrEmpty(ApproverLevel) && ApproverLevel.ToLower() == "companylevel")
        {
            List<OrderMOASSystem> objOrderMOASSystemList = objOrderMOASSystemRepository.GetByOrderIDAndStatus(objOrder.OrderID, "Order Pending");

            if (objOrderMOASSystemList != null && objOrderMOASSystemList.Count > 0)
            {
                AddNoteHistory("Approved", objOrder.OrderID, ApproverID, null);

                UserInformation objUserInformation = new UserInformation();
                objUserInformation = objUsrInfoRepo.GetById(objOrderMOASSystemList[0].ManagerUserInfoID);
                sendVerificationEmailMOASManager(objOrder.OrderID, objUserInformation.LoginEmail, objUserInformation.FirstName, objUserInformation.UserInfoID, objOrder);
            }
            else
            {
                UpdateOrderAndSendConfirmation(objOrder, objUser, ApproverID);
            }
        }
        else
        {
            //Update the Received status for the order (If the MOAS Approver level is set at Station Level
            //Hence, Only one Approver needs to approve the order.    
            UpdateOrderAndSendConfirmation(objOrder, objUser, ApproverID);
        }

        return userName;
    }

    /// <summary>
    /// Added By Prashant April 2013,
    /// To Update the Order and Send the Order Approval and Received Notification to IE,Admin, Supplier
    /// </summary>
    /// <param name="objOrder"></param>
    /// <param name="objUser"></param>
    private void UpdateOrderAndSendConfirmation(Order objOrder, UserInformation objUser, Int64 ApproverID)
    {

        objOrder.OrderStatus = "Open";
        objOrder.UpdatedDate = DateTime.Now;
        objRepos.SubmitChanges();

        AddNoteHistory("Approved", objOrder.OrderID, ApproverID, null);

        sendVerificationEmail("Approved", objOrder.OrderID, objUser.LoginEmail, objUser.FirstName, objUser.UserInfoID, objOrder, null);//send email to CE

        if (!objOrder.ReferenceName.ToLower().Contains("test") && !IncentexGlobal.IsIEFromStoreTestMode)
        {
            //Billing
            if (objOrder.OrderFor == "ShoppingCart") // Company Pays
            {
                objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
            }
            else
            {
                objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
            }
            //Shipping
            if (objOrder.OrderFor == "ShoppingCart")
            {
                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
            }
            else
            {
                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
            }

            sendIEEmail("Approved", objOrder.OrderID);//send email to IE

            sendEmailToSupplier(objOrder.OrderID, objBillingInfo, objShippingInfo);//send order to supplier                   

            new SAPOperations().SubmitOrderToSAP(objOrder.OrderID);

            //add by Mayur on 29-April-2012 for Magaya Integration
            //SendDataToMagaya(objOrder);
        }
        else
        {
            sendTestOrderEmailNotification("Approved", objOrder.OrderID);
        }
    }


    /// <summary>
    /// Sends the verification email.
    /// </summary>
    /// <param name="strStatus">New order status.</param>
    /// <param name="OrderID">The order id.</param>
    /// /// <param name="ToAdd">Email To address.</param>
    private void sendVerificationEmail(String strStatus, Int64 OrderID, String ToAdd, String FullName, Int64 UserInfoID, Order objOrder, String ReasonMsg)
    {
        try
        {
            //Get Email Content
            INC_EmailTemplate objEmail = new EmailRepository().GetEmailTemplatesByName("Order Placed");
            if (objEmail != null)
            {
                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                CompanyEmployeeRepository objCERepo = new CompanyEmployeeRepository();
                CompanyEmployee objCompanyEmployee = objCERepo.GetByUserInfoId(objUserInformation.UserInfoID);

                Boolean IsMOASWithCostCenterCode = objCERepo.FUN_IsMOASWithCostCenterCode(Convert.ToInt64(objOrder.UserId));

                String sFrmadd = objEmail.sFromAddress;
                String sFrmname = objEmail.sFromName;
                String sSubject = objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber.ToString();
                String sToadd = ToAdd;

                StringBuilder messagebody = new StringBuilder(objEmail.sTemplateContent); // From Template table

                String strFirstNote = "";
                if (strStatus == "Canceled")
                {
                    strFirstNote = "Your ordered has been review by your manager and at this time has been cancelled.";
                    strFirstNote += "<br/><br/><strong>Managers Reason for Cancelling: </strong>";
                    strFirstNote += ReasonMsg;
                }
                else if (strStatus == "Approved")
                    strFirstNote = "You order has been reviewed by your manager and is approved. This order has been successfully submitted into our order processing system.";

                messagebody.Replace("{firstnote}", strFirstNote);
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
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
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
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
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

                // WL ContactID Info
                messagebody.Replace("{OrderPlacedBy}", String.Empty);
                messagebody.Replace("{WLContactID}", String.Empty);
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
                        innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate) * Convert.ToDecimal(objFinal[i].Qty);
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
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + b);
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                }

                messagebody.Replace("{ShippingCost}", objOrder.ShippingAmount.ToString());
                messagebody.Replace("{Saletax}", "$" + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)).ToString());
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + objOrder.OrderAmount).ToString());
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

                String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                //String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                //String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.MOAS)) == true)
                {
                    new CommonMails().SendMail(objUserInformation.UserInfoID, "MOAS From My Pending Orders", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
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
            //* ---------- Change for optimization By gaurang 16 MAR 2013 Start-----------------*/
            Order objOrder = objRepos.GetByOrderID(OrderID);
            UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
            CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID);
            //* ---------- Change for optimization By gaurang 16 MAR 2013 END-----------------*/

            for (Int32 i = 0; i < objAdminList.Count; i++)
            {
                sendVerificationEmailAdmin(strStatus, OrderID, objAdminList[i].LoginEmail, objAdminList[i].FirstName, objAdminList[i].UserInfoID, false, objUserInformation, objCompanyEmployee, objOrder);
            }
        }
    }

    /// <summary>
    /// Sends the email to incentex admin.
    /// </summary>
    private void sendTestOrderEmailNotification(String strStatus, Int64 OrderID)
    {
        List<FUN_GetTestOrderEmailReceiversResult> objAdminList = new List<FUN_GetTestOrderEmailReceiversResult>();
        objAdminList = new IncentexBEDataContext().FUN_GetTestOrderEmailReceivers().ToList();
        if (objAdminList.Count > 0)
        {
            //* ---------- Change for optimization By gaurang 16 MAR 2013 Start-----------------*/
            Order objOrder = objRepos.GetByOrderID(OrderID);
            UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
            CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID);
            //* ---------- Change for optimization By gaurang 16 MAR 2013 END-----------------*/

            foreach (FUN_GetTestOrderEmailReceiversResult receiver in objAdminList)
            {
                sendVerificationEmailAdmin(strStatus, OrderID, receiver.LoginEmail, receiver.FirstName, Convert.ToInt64(receiver.UserInfoID), true, objUserInformation, objCompanyEmployee, objOrder);
            }
        }
    }

    public void GenerateEmail(Int64 orderID)
    {
        try
        {
            List<GetMOASPendingOrderDetailsResult> listMOASOrderDetails = new List<GetMOASPendingOrderDetailsResult>();

            listMOASOrderDetails = new OrderConfirmationRepository().GetMOASPendingOrderDetails(orderID).ToList();
            if (listMOASOrderDetails.Count > 0)
            {
                // Get Web URL from web Config
                String serverPath = HttpContext.Current.Server.MapPath("~/emailtemplate/");
                EmailBodyContents objEmailBody = new OrderApproval().GenerateMailBody(listMOASOrderDetails, serverPath);
                String eMailTemplate = String.Empty;
                String sSubject = "Order Placed";

                String sReplyToadd = Common.ReplyTo;

                StreamReader _StreamReader;
                _StreamReader = System.IO.File.OpenText(serverPath + "OrderCommonMail.htm");
                eMailTemplate = _StreamReader.ReadToEnd();
                _StreamReader.Close();
                _StreamReader.Dispose();



                StringBuilder MessageBody = new StringBuilder(eMailTemplate);

                MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                MessageBody.Replace("{FullName}", "Incentex");

                MessageBody.Replace("{firstnote}", "Please review the following order below if you have any questions please post them to the notes section of this order located within the order in the system.");

                MessageBody.Replace("{OrderNumber}", listMOASOrderDetails[0].OrderNumber);
                MessageBody.Replace("{referencename}", listMOASOrderDetails[0].ReferenceName + "<br/>Workgroup : " + listMOASOrderDetails[0].WorkGroupName); // change by mayur on 26-March-2012
                MessageBody.Replace("{OrderStatus}", listMOASOrderDetails[0].OrderStatus);
                MessageBody.Replace("{FullName}", listMOASOrderDetails[0].FullName);
                MessageBody.Replace("{OrderDate}", listMOASOrderDetails[0].OrderDate.ToString());

                // Replace Billing Info
                MessageBody.Replace("{BillingTable}", objEmailBody.BillingInfotbl);
                // Replace Shipping Info
                MessageBody.Replace("{ShippingTable}", objEmailBody.ShippingInfotbl);
                // Replace Items Details
                MessageBody.Replace("{OrderCartItemDetails}", objEmailBody.OrderCartItemDetails);



                new CommonMails().SendEmail4Local(1092, "Testing", "surendar.yadav@indianic.com", sSubject, MessageBody.ToString(), "incentextest6@gmail.com", "test6incentex", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);

        }
    }
    public EmailBodyContents GenerateMailBody(List<GetMOASPendingOrderDetailsResult> listMOASOrderDetails, String serverPath)
    {
        EmailBodyContents objmailBody = new EmailBodyContents();

        // DO YOUR STUFF   
        #region Billing Info
        StreamReader _srBilling;
        _srBilling = System.IO.File.OpenText(serverPath + "OrderBillingTbl.htm");
        StringBuilder strBilling = new StringBuilder(_srBilling.ReadToEnd());
        _srBilling.Close();
        _srBilling.Dispose();
        strBilling.Replace("{Name}", listMOASOrderDetails[0].BillingCO + " " + listMOASOrderDetails[0].BillingManager);
        strBilling.Replace("{CompanyName}", listMOASOrderDetails[0].BillingCompanyName);
        strBilling.Replace("{Address1}", listMOASOrderDetails[0].BillingAddress1);
        strBilling.Replace("{City}", listMOASOrderDetails[0].BillingsCityName);
        strBilling.Replace("{State}", listMOASOrderDetails[0].BillingsStatename);
        strBilling.Replace("{Zip}", listMOASOrderDetails[0].BillingZipCode);
        strBilling.Replace("{Country}", listMOASOrderDetails[0].BillingsCountryName);
        strBilling.Replace("{Phone}", listMOASOrderDetails[0].BillingTelephone);
        strBilling.Replace("{Email}", listMOASOrderDetails[0].BillingEmail);
        #endregion

        #region Shipping Info
        StreamReader _srShipping;
        _srShipping = System.IO.File.OpenText(serverPath + "OrderShippingTbl.htm");
        StringBuilder strShipping = new StringBuilder(_srShipping.ReadToEnd());
        _srShipping.Close();
        _srShipping.Dispose();

        strShipping.Replace("{Airport1}", listMOASOrderDetails[0].ShippingAirport);
        strShipping.Replace("{Department1}", listMOASOrderDetails[0].ShippingDepartment);
        strShipping.Replace("{Name1}", listMOASOrderDetails[0].ShippingFirstName + " " + listMOASOrderDetails[0].ShippingLastName);
        strShipping.Replace("{CompanyName1}", listMOASOrderDetails[0].ShippingCompanyName);
        strShipping.Replace("{Address11}", listMOASOrderDetails[0].ShippingAddress1 + "<br/>" + listMOASOrderDetails[0].ShippingStreet);
        strShipping.Replace("{City1}", listMOASOrderDetails[0].ShippingsCityName);
        strShipping.Replace("{County1}", listMOASOrderDetails[0].ShippingsCountryName);
        strShipping.Replace("{State1}", listMOASOrderDetails[0].ShippingsStatename);
        strShipping.Replace("{Zip1}", listMOASOrderDetails[0].ShippingZipCode);
        strShipping.Replace("{Phone1}", listMOASOrderDetails[0].ShippingTelephone);
        strShipping.Replace("{Email1}", listMOASOrderDetails[0].ShippingEmail);
        #endregion
        StringBuilder sbCartItems = new StringBuilder();
        #region Items Header
        // do code for Header for items
        //sbCartItems.Append(@"<table style=""font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px; color: #666 background-color: #DDDDDD;"" width=""100%"">");
        sbCartItems.Append(@"<table width=""100%"" cellspacing=""1"" cellpadding=""0"" style=""background-color:#dddddd;"" >");
        sbCartItems.Append(@"<tr>");
        sbCartItems.Append(@"<td width=""8%"" style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
        //sbCartItems.Append("@<td style="padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
        sbCartItems.Append(@"Ordered");
        sbCartItems.Append(@"</td>");
        sbCartItems.Append(@"<td width=""15%"" style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
        sbCartItems.Append(@"Item#");
        sbCartItems.Append(@"</td>");
        sbCartItems.Append(@"<td width=""13%"" style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
        sbCartItems.Append(@"Size");
        sbCartItems.Append(@"</td>");
        sbCartItems.Append(@"<td width=""15%"" style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
        sbCartItems.Append(@"Product Description");
        sbCartItems.Append(@"</td>");
        sbCartItems.Append(@"<td width=""8%"" style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
        sbCartItems.Append(@"GL Code");
        sbCartItems.Append(@"</td>");
        sbCartItems.Append(@"<td width=""8%"" style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
        sbCartItems.Append(@"Price");
        sbCartItems.Append(@"</td>");
        sbCartItems.Append(@"<td width=""8%"" style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
        sbCartItems.Append(@"Extended");
        sbCartItems.Append(@"</td>");
        sbCartItems.Append(@"</tr>");
        //sbCartItems.Append(@"<tr> <td colspan=""7""><hr />  </td></tr>");

        #endregion

        #region Cart items rows
        foreach (var item in listMOASOrderDetails)
        {


            sbCartItems.Append(@"<tr>");
            sbCartItems.Append(@"<td width='8%' style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
            sbCartItems.Append(item.Quantity);
            sbCartItems.Append(@"</td>");
            sbCartItems.Append(@"<td width='15%' style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
            sbCartItems.Append(item.ItemNumber);
            sbCartItems.Append(@"</td>");

            sbCartItems.Append(@"<td width='13%' style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
            sbCartItems.Append(item.Size);
            sbCartItems.Append(@"</td>");

            sbCartItems.Append(@"<td width='38%' style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
            sbCartItems.Append(item.ProductDescription);
            sbCartItems.Append(@"</td>");

            sbCartItems.Append(@"<td width='10%' style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
            if (!String.IsNullOrEmpty(item.GLCode))
                sbCartItems.Append(item.GLCode);
            else
                sbCartItems.Append("-");
            sbCartItems.Append(@"</td>");

            sbCartItems.Append(@"<td width='8%' style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
            sbCartItems.Append(@"$" + (item.MOASUnitPrice != null ? Convert.ToDecimal(item.MOASUnitPrice) : Convert.ToDecimal(item.UnitPrice)).ToString());
            sbCartItems.Append(@"</td>");
            sbCartItems.Append(@"<td width='8%' style=""padding:5px 10px;background-color:#ffffff;font-size:12px;font-family:Arial;"">");
            sbCartItems.Append(@"$" + (item.MOASUnitPrice != null ? Convert.ToDecimal(item.MOASUnitPrice) : Convert.ToDecimal(item.UnitPrice)) * Convert.ToDecimal(item.Quantity));
            sbCartItems.Append(@"</td>");
            sbCartItems.Append(@"</tr>");
            //sbCartItems.Append(@"<tr><td colspan='7'><hr /></td></tr>");
        }
        #endregion

        objmailBody.BillingInfotbl = strBilling.ToString();
        objmailBody.ShippingInfotbl = strShipping.ToString();
        objmailBody.OrderCartItemDetails = sbCartItems.ToString();
        return objmailBody;
    }

    /// <summary>
    /// Sends the verification email to incentex admin.
    /// </summary>
    /// <param name="strStatus">New order status.</param>
    /// <param name="OrderID">The order id.</param>
    /// <param name="ToAdd">receiver email address.</param>
    ///<param name="ToAdd">full name of receiver.</param>
    private void sendVerificationEmailAdmin(String strStatus, Int64 OrderID, String ToAdd, String FullName, Int64 UserInfoID, Boolean IsTestOrder, UserInformation objUserInformation, CompanyEmployee objCompanyEmployee, Order objOrder)
    {
        try
        {

            //Get Email Content
            INC_EmailTemplate objEmail = new EmailRepository().GetEmailTemplatesByName("Order Placed");
            if (objEmail != null)
            {

                Boolean IsMOASWithCostCenterCode = objCompanyEmployeeRepository.FUN_IsMOASWithCostCenterCode(Convert.ToInt64(objOrder.UserId));

                String sFrmadd = objEmail.sFromAddress;
                String sFrmname = objEmail.sFromName;
                String sSubject = objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber.ToString();
                String sToadd = ToAdd;
                Int64 sToUserInfoID = UserInfoID;

                StringBuilder messagebody = new StringBuilder(objEmail.sTemplateContent); // From Template table

                messagebody.Replace("{firstnote}", "Thank you for your business you can check the status of order(s) any time by logging into your store <a href='http://www.world-link.us.com' title='Incentex'>here.</a>");
                messagebody.Replace("{FullName}", FullName);
                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName);
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());

                // Set SAP Company Contact ID and Full name
                String OrderPlacedBy = String.Empty;
                String WLContactID = String.Empty;
                GetSAPCompanyCodeIDResult objSAPCompanyResult = objUsrInfoRepo.GetSAPCompanyCodeID(objUserInformation.UserInfoID);
                if (objSAPCompanyResult != null)
                {
                    OrderPlacedBy = String.Format("Order Placed By: {0}", objSAPCompanyResult.FullName);
                    WLContactID = String.Format("World-Link Contact ID : {0}", objSAPCompanyResult.SAPCompanyCodeID);
                }
                //Check here for CreditUsed
                String cr;
                Boolean creditused = true;
                if (!String.IsNullOrEmpty(objOrder.CreditUsed) && objOrder.CreditUsed != "0")
                    creditused = true;
                else
                    creditused = false;

                //Both Option have used
                if (objOrder.PaymentOption != null && creditused)
                {

                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting Credits Paid by Corporate ";
                        //Strating Credit Row
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", objOrder.CreditAmt.ToString());
                    }
                    else
                    {
                        cr = "Anniversary Credits Paid by Corporate ";
                        //Anniversary Credit Row
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());

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

                // WL ContactID Info
                messagebody.Replace("{OrderPlacedBy}", OrderPlacedBy);
                messagebody.Replace("{WLContactID}", WLContactID);
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
                //String b = NameBars(objOrder.OrderID);
                //if (b != null)
                //{
                messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + NameBars(objOrder.OrderID) + (IsTestOrder ? @"<br/><span style=""color:red;font-weight:bold;"">THIS IS A TEST ORDER. PLEASE DO NOT PROCESS.</span>" : ""));
                //}
                //else
                //{
                //    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + (IsTestOrder ? @"<br/><span style=""color:red;font-weight:bold;"">THIS IS A TEST ORDER. PLEASE DO NOT PROCESS.</span>" : ""));
                //}

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
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                //String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                //String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.MOAS)) == true)
                {
                    new CommonMails().SendMail(sToUserInfoID, "MOAS From My Pending Orders", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                }
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Sends the email to supplier.
    /// </summary>
    /// <param name="OrderID">The order id.</param>
    private void sendEmailToSupplier(Int64 orderid, CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo)
    {
        try
        {
            Order objOrder = objRepos.GetByOrderID(orderid);
            if (objOrder != null)
            {
                List<SelectSupplierAddressResult> obj = objRepos.GetSupplierAddress(objOrder.OrderFor, objOrder.OrderID);

                //* ---------- Change for optimization By gaurang 16 MAR 2013 Start-----------------*/
                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                //* ---------- Change for optimization By gaurang 16 MAR 2013 END-----------------*/

                foreach (SelectSupplierAddressResult repeaterItem in obj)
                {
                    sendVerificationEmailSupplier(orderid, objOrder.MyShoppingCartID, repeaterItem.SupplierID, repeaterItem.Name, objShippingInfo, repeaterItem.CompanyName, objUserInformation, objOrder);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Sends the verification email supplier.
    /// </summary>
    /// <param name="OrderID">The order id.</param>
    /// <param name="ShoppingCartID">The shopping cart ID.</param>
    /// <param name="supplierId">The supplier id.</param>
    /// <param name="fullName">The full name.</param>
    private void sendVerificationEmailSupplier(Int64 OrderId, String ShoppingCartID, Int64 supplierId, String fullName, CompanyEmployeeContactInfo objShippingInfo, String SupplierCompanyName, UserInformation objUserInformation, Order objOrder)
    {
        try
        {
            //Get supplierinfo by id
            UserInformation objUserInfo = objUsrInfoRepo.GetById(new SupplierRepository().GetById(supplierId).UserInfoID);

            if (objUserInfo != null)
            {
                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(objUserInfo.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderConfirmations)) == true)
                {

                    CityRepository objCity = new CityRepository();
                    StateRepository objState = new StateRepository();
                    CountryRepository objCountry = new CountryRepository();

                    //Get Email Content
                    INC_EmailTemplate objEmail = new EmailRepository().GetEmailTemplatesByName("Order Placed Supplier");
                    if (objEmail != null)
                    {

                        //Find UserName who had order purchased
                        //Order objOrder = objRepos.GetByOrderID(OrderId);
                        //UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                        String sFrmadd = objEmail.sFromAddress;
                        String sFrmname = objEmail.sFromName;
                        String sSubject = objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber.ToString();
                        String sToadd = objUserInfo.LoginEmail;

                        StringBuilder messagebody = new StringBuilder(objEmail.sTemplateContent); // From Template table

                        messagebody.Replace("{firstnote}", "Please review the following order below if you have any questions please post them to the notes section of this order located within the order in the system.");
                        messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                        messagebody.Replace("{referencename}", objOrder.ReferenceName + "<br/>Workgroup : " + objLookupRepository.GetById((Int64)(objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID).WorkgroupID)).sLookupName); // change by mayur on 26-March-2012
                        messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
                        //messagebody.Replace("{ReferenceName}", lblPurchasedBy.Text);
                        messagebody.Replace("{FullName}", fullName);
                        messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());

                        String PaymentMethod = String.Empty;
                        if (objOrder.PaymentOption != null)
                            PaymentMethod = objLookupRepository.GetById((Int64)objOrder.PaymentOption).sLookupName;

                        //Added on 20 Sep 2011
                        //Check here for CreditUsed
                        String cr;
                        Boolean creditused = true;
                        if (!String.IsNullOrEmpty(objOrder.CreditUsed) && objOrder.CreditUsed != "0")
                            creditused = true;
                        else
                            creditused = false;


                        //Both Option have used
                        if (objOrder.PaymentOption != null && creditused)
                        {

                            if (objOrder.CreditUsed == "Previous")
                            {
                                cr = "Starting " + "Credits Paid by Corporate ";
                            }
                            else
                            {
                                cr = "Anniversary " + "Credits Paid by Corporate ";
                            }
                            messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + PaymentMethod);
                        }
                        //Only Starting or Anniversary
                        else if (objOrder.PaymentOption == null && creditused)
                        {
                            if (objOrder.CreditUsed == "Previous")
                            {
                                cr = "Starting " + "Credits - Paid by Corporate ";
                            }
                            else
                            {
                                cr = "Anniversary " + "Credits - Paid by Corporate ";
                            }
                            messagebody.Replace("{PaymentType}", cr);
                        }
                        //Only Payment Option
                        else if (objOrder.PaymentOption != null && !creditused)
                        {
                            messagebody.Replace("{PaymentType}", PaymentMethod);
                        }
                        else
                        {
                            messagebody.Replace("{PaymentType}", "Paid By Corporate");
                        }
                        messagebody.Replace("{Payment Method :}", "Payment Method :");

                        //For displaying payment option code
                        messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
                        messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);

                        //End
                        //messagebody.Replace("{CreditType}", "---");

                        #region shipping address
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
                        //messagebody.Replace("{Address22}", lbl.Text);
                        messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
                        messagebody.Replace("{County1}", objShippingInfo.county);
                        messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
                        messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
                        messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
                        messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
                        messagebody.Replace("{Email1}", objShippingInfo.Email);
                        #endregion

                        String innermessageSupplier = "";
                        Boolean GLCodeExists = false;

                        #region Supplier

                        List<SelectOrderDetailsForSupplierResult> lstSupplierItemsFromOrder = objRepos.GetOrderItemsForSupplier(supplierId, objOrder.OrderID);

                        if (lstSupplierItemsFromOrder.Count > 0)
                        {
                            GLCodeExists = lstSupplierItemsFromOrder.FirstOrDefault(le => le.GLCode != null && le.GLCode != "") != null;
                            Int16 ColSpan = 9;

                            if (GLCodeExists)
                                ColSpan = 10;

                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                            innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td align='center' colspan='2'  style='background-color:Gray;font-weight:bold;color:Black;'>";
                            innermessageSupplier = innermessageSupplier + SupplierCompanyName;
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";
                            innermessageSupplier = innermessageSupplier + "</table>";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";
                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                            innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: left;'>";
                            innermessageSupplier = innermessageSupplier + "Ordered";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Item#";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Size";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Color";
                            innermessageSupplier = innermessageSupplier + "</td>";

                            if (!GLCodeExists)
                            {
                                innermessageSupplier = innermessageSupplier + "<td width='35%' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "Description";
                                innermessageSupplier = innermessageSupplier + "</td>";
                            }
                            else
                            {
                                innermessageSupplier = innermessageSupplier + "<td width='25%' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "Description";
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "GL Code";
                                innermessageSupplier = innermessageSupplier + "</td>";
                            }

                            //Add Nagmani 18-Jan-2012
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Unit Price";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Extended Price";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            //End

                            innermessageSupplier = innermessageSupplier + "</table>";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";
                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                            innermessageSupplier = innermessageSupplier + "<hr />";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";

                            foreach (SelectOrderDetailsForSupplierResult item in lstSupplierItemsFromOrder)
                            {
                                innermessageSupplier = innermessageSupplier + "<tr>";
                                innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                                innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                                innermessageSupplier = innermessageSupplier + "<tr>";
                                innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: left;'>";
                                innermessageSupplier = innermessageSupplier + item.Quantity;
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='15%' height='50' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + item.ItemNumber;
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + item.Size;
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + item.Color;
                                innermessageSupplier = innermessageSupplier + "</td>";

                                if (!GLCodeExists)
                                {
                                    innermessageSupplier = innermessageSupplier + "<td width='35%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + item.ProductDescrption;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                }
                                else
                                {
                                    innermessageSupplier = innermessageSupplier + "<td width='25%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + item.ProductDescrption;
                                    innermessageSupplier = innermessageSupplier + "</td>";

                                    innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + item.GLCode;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                }

                                innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "$" + (item.MOASPrice != null ? Convert.ToDecimal(item.MOASPrice) : Convert.ToDecimal(item.Price)).ToString();
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "$" + (item.MOASPrice != null ? Convert.ToDecimal(item.MOASPrice) : Convert.ToDecimal(item.Price)) * Convert.ToDecimal(item.Quantity);
                                innermessageSupplier = innermessageSupplier + "</td>";

                                innermessageSupplier = innermessageSupplier + "</tr>";
                                innermessageSupplier = innermessageSupplier + "</table>";
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "</tr>";
                            }

                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                            innermessageSupplier = innermessageSupplier + "<hr />";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";
                        }

                        messagebody.Replace("{innermesaageforsupplier}", innermessageSupplier);

                        //Update Nagmani 18-Jan-2012
                        messagebody.Replace("{ShippingCost}", "");
                        messagebody.Replace("{Saletax}", "");
                        // messagebody.Replace("{OrderTotal}", "");

                        //messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());
                        messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal((objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + (objOrder.MOASOrderAmount != null ? objOrder.MOASOrderAmount : objOrder.OrderAmount)).ToString());

                        //End Nagmani
                        messagebody.Replace(" {Order Notes:}", "Order Notes :");
                        messagebody.Replace("{ShippingCostView}", "");
                        messagebody.Replace("{SalesTaxView}", "");
                        //Update Nagmani 18-Jan-2012
                        //  messagebody.Replace("{OrderTotalView}", "");
                        messagebody.Replace("{OrderTotalView}", "Order Total :");
                        //End Nagmani 

                        //End
                        #endregion


                        messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + NameBars(objOrder.OrderID));

                        messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                        String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                        Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);

                        new CommonMails().SendMail(objUserInfo.UserInfoID, "MOAS From My Pending Orders", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Sends the verification email MOAS manager for approve order.
    /// Add By Mayur on 6-feb-2012
    /// </summary>
    /// <param name="OrderNumber">The order number.</param>
    /// <param name="MOASEmailAddress">The MOAS email address.</param>
    /// <param name="FullName">The full name.</param>
    private void sendVerificationEmailMOASManager(Int64 OrderId, String MOASEmailAddress, String FullName, Int64 MOASUserId, Order objOrder)
    {
        try
        {
            //Get Email Content
            INC_EmailTemplate objEmail = new EmailRepository().GetEmailTemplatesByName("Order Placed MOAS");
            if (objEmail != null)
            {

                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                CompanyEmployeeRepository objCERepo = new CompanyEmployeeRepository();
                CompanyEmployee objCompanyEmployee = objCERepo.GetByUserInfoId(objUserInformation.UserInfoID);

                Boolean IsMOASWithCostCenterCode = objCERepo.FUN_IsMOASWithCostCenterCode(Convert.ToInt64(objOrder.UserId));

                String sFrmadd = objEmail.sFromAddress;
                String sFrmname = objEmail.sFromName;
                String sSubject = objEmail.sSubject.Replace("{FullName}", objUserInformation.FirstName + " " + objUserInformation.LastName).Replace("{OrderNumber}", objOrder.OrderNumber);
                String sToadd = MOASEmailAddress;

                StringBuilder messagebody = new StringBuilder(objEmail.sTemplateContent); // From Template table
                String PaymentMethod = String.Empty;
                if (objOrder.PaymentOption != null)
                    PaymentMethod = objLookupRepository.GetById((Int64)objOrder.PaymentOption).sLookupName;

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
                    //updated by Prashant 27th feb 2013
                    //MOAS L1 Price Change for CA User
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    //updated by Prashant 27th feb 2013
                    //MOAS L1 Price Change for CA User

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
                            innermessage = innermessage + objFinal[i].ProductDescrption;
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

                messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + NameBars(objOrder.OrderID));

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
                // messagebody.Replace("{PurchasesNotEligableforCreditUse}", lblNotEligibleforAnniversarycredit.Text);

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

                String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                if (new ManageEmailRepository().CheckEmailAuthentication(MOASUserId, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.MOAS)) == true)
                {
                    new CommonMails().SendMail(MOASUserId, "MOAS From My Pending Orders", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
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
                if (!String.IsNullOrEmpty(objItem.NameToBeEngraved))
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
                if (!String.IsNullOrEmpty(objItem.NameToBeEngraved))
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

    /// <summary>
    /// Add the note history of order.
    /// </summary>
    /// <param name="strStatus">The order status.</param>
    /// <param name="orderID">The order ID.</param>
    public void AddNoteHistory(String strStatus, Int64 orderID, Int64 ApproverID, String ReasonMsg)
    {
        String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.CACE);
        NoteDetail objComNot = new NoteDetail();
        NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();

        String strNoteContents = "Action Taken: " + strStatus;
        if (strStatus == "Canceled")
            strNoteContents += Environment.NewLine + "Reason for Cancelling: " + ReasonMsg;

        objComNot.Notecontents = strNoteContents;
        objComNot.NoteFor = strNoteFor;
        objComNot.ForeignKey = orderID;
        objComNot.CreateDate = System.DateTime.Now;
        objComNot.CreatedBy = ApproverID;
        objComNot.UpdateDate = System.DateTime.Now;
        objComNot.UpdatedBy = ApproverID;
        objCompNoteHistRepos.Insert(objComNot);
        objCompNoteHistRepos.SubmitChanges();
    }
}
public class EmailBodyContents
{
    public String OrderCartItemDetails { get; set; }
    public String OrderSummaryTable { get; set; }
    public String OrderHeaderTable { get; set; }
    public String BillingInfotbl { get; set; }
    public String ShippingInfotbl { get; set; }
    public String SupplierMsgTbl { get; set; }

}
