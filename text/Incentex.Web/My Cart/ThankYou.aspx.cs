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

public partial class My_Cart_ThankYou : PageBase
{
    UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
    UserInformation objUsrInfo = new UserInformation();
    OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
    CompanyRepository objCmpRepo = new CompanyRepository();
    CompanyEmployeeContactInfo objShippingInfo;
    CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfo();
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    List<SelectMyShoppingCartProductResult> obj;
    MyIssuanceCartRepository objMyIssCartRepo = new MyIssuanceCartRepository();
    List<MyIssuanceCart> objMyIssCart = new List<MyIssuanceCart>();
    String PaymentMethod = String.Empty;

    protected void Page_Load(Object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Thank You";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Visible = false;

            //Send Email
            // Session["NewOrderID"] = "2436";
            if (Session["NewOrderID"] != null)
            {
                GetMiscDetails(Session["NewOrderID"].ToString());
                //1.)Customer
                sendVerificationEmail(Session["NewOrderID"].ToString());

                //add by mayur on 22-nov-2011 for MOAS payment method
                //start
                if (PaymentMethod.Contains("MOAS"))
                {
                    //3.)MOAS Manager
                    sendEmailToMOASManager(Convert.ToInt64(Session["NewOrderID"].ToString()));
                }
                else
                {
                    //2.)IE Admin
                    sendIEEmail();
                    //3.)Supplier
                    sendEmailToSupplier(Session["NewOrderID"].ToString());
                }
                //end
                Session["NewOrderID"] = null;
                Session["OrderNumber"] = null;
                Session["ReferenceName"] = null;
                Session["OrderInstruction"] = null;
                // for set purchase flag in tracking center(parth)
                GetPurchaseUpdate();
            }
        }
    }

    private void GetMiscDetails(String OrderId)
    {
        Order objOrder = new Order();
        objOrder = objRepos.GetByOrderID(Convert.ToInt64(OrderId));

        //add by mayur on 22-nov-2011 for moas payment method
        //start
        if (objOrder.PaymentOption != null)
            PaymentMethod = new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName;
        //end

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
            objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
        }
        else
        {
            objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
        }


    }

    #region Send An Email To Supplier,IE and Customer Admin

    private void sendVerificationEmail(String OrderID)
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
            objEmailBE.STemplateName = "Order Placed";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                objUsrInfo = objUsrInfoRepo.GetById(IncentexGlobal.CurrentMember.UserInfoID);
                String sToadd = "";
                if (objUsrInfo != null)
                {
                    sToadd = objUsrInfo.LoginEmail;
                }



                //Find UserName who had order purchased
                String strUserName = null;
                Order objOrder = new Order();
                objOrder = objRepos.GetByOrderID(Convert.ToInt64(OrderID));
                Int64 intUserId = Convert.ToInt64(objOrder.UserId);
                objUsrInfo = objUsrInfoRepo.GetById(intUserId);
                if (objUsrInfo != null)
                    strUserName = (objUsrInfo.FirstName);
                //End


                //Start Subject
                // String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                Int64 intCompanyiD = Convert.ToInt64(objUsrInfo.CompanyId);
                List<Company> objComName = new List<Company>();
                objComName = objCmpRepo.GetByCompanyId(intCompanyiD);
                String sSubject = objComName[0].CompanyName + " - " + objOrder.OrderNumber.ToString();
                //End

                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();


                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());
                if (PaymentMethod.Contains("MOAS"))//start add by mayur on 24-nov-2011
                {
                    messagebody.Replace("{firstnote}", "Your requisition (" + objOrder.OrderNumber + ") has been submitted for review. Once the order is approved or if it is canceled we will notify you via email. You can also check the status of your requisition in your My Tracking Center.");
                }//end mayur
                else
                {
                    messagebody.Replace("{firstnote}", "Thank you for your business you can check the status of your order(s) any time by logging into your store <a href='http://www.world-link.us.com' title='Incentex'>here.</a>");
                }
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName);
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus); //change by mayur on 22-nov-2011


                //  messagebody.Replace("{ReferenceName}", lblPurchasedBy.Text);
                messagebody.Replace("{FullName}", strUserName);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());
                String cr;
                Boolean creditused = true;
                //Check here for CreditUsed
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
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt)).ToString());
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
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt)).ToString());
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
                // messagebody.Replace("{ShippingMethod}", "");

                //messagebody.Replace("{CreditType}", lblCreditType.Text);

                //Billing Address
                messagebody.Replace("{Billing Address}", "Billing Address");
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
                messagebody.Replace("{Address11}", objShippingInfo.Address);
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

                #region bind item detail
                String innermessage = "";

                if (objOrder.OrderFor == "ShoppingCart")
                {
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
                        //innermessage = innermessage + "<td width='6%' style='font-weight: bold; text-align: center;'>";
                        //innermessage = innermessage + obj[i].color;
                        //innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + obj[i].Size;
                        innermessage = innermessage + "</td>";
                        //innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + obj[i].ProductDescrption1;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(obj[i].UnitPrice);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(obj[i].UnitPrice) * Convert.ToDecimal(obj[i].Quantity);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td colspan='7'>";
                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                    }
                    messagebody.Replace("{innermessage}", innermessage);

                    messagebody.Replace("{ShippingCost}", objOrder.ShippingAmount.ToString());
                    messagebody.Replace("{Saletax}", "");
                    messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());

                }
                else
                {
                    Boolean showprice = checkissuancepolicy(objOrder);
                    List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                    for (Int32 i = 0; i < objFinal.Count; i++)
                    {
                        String[] p = new String[1];
                        if (objFinal != null && objFinal[i] != null)
                        {
                            if (objFinal[i].NameToBeEngraved != null && !String.IsNullOrEmpty(objFinal[i].NameToBeEngraved))
                            {
                                p = objFinal[i].NameToBeEngraved.Split(',');
                            }
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].Qty;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].item;
                        innermessage = innermessage + "</td>";
                        //innermessage = innermessage + "<td width='6%' style='font-weight: bold; text-align: center;'>";
                        //innermessage = innermessage + objFinal[i].color;
                        //innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].sLookupName;
                        innermessage = innermessage + "</td>";
                        //innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + objFinal[i].ProductDescrption;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        if (showprice)
                        {
                            innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate);
                        }
                        else
                        {
                            innermessage = innermessage + "---";
                        }
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        if (showprice)
                        {
                            innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate) * Convert.ToDecimal(objFinal[i].Qty);
                        }
                        else
                        {
                            innermessage = innermessage + "---";
                        }
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td colspan='7'>";
                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";



                    }
                    messagebody.Replace("{innermessage}", innermessage);


                    if (showprice)
                    {
                        messagebody.Replace("{ShippingCost}", objOrder.ShippingAmount.ToString());
                        messagebody.Replace("{Saletax}", "");
                        messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());
                    }
                    else
                    {
                        messagebody.Replace("{ShippingCost}", "---");
                        messagebody.Replace("{Saletax}", "");
                        messagebody.Replace("{OrderTotal}", "---");
                    }


                }
                #endregion

                messagebody.Replace("{innermesaageforsupplier}", "");
                messagebody.Replace("{Order Notes:}", "Order Notes :");
                messagebody.Replace("{ShippingCostView}", "Shipping :");
                //messagebody.Replace("{SalesTaxView}", "Sales Tax :");
                messagebody.Replace("{SalesTaxView}", "");
                messagebody.Replace("{OrderTotalView}", "Order Total :");
                String a = NameBars(objOrder.OrderID);
                if (a != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + a);
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                }


                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(objUsrInfo.UserInfoID, "Order Confirmation", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
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

    private Boolean checkissuancepolicy(Order objOrder)
    {
        Boolean returnvalue = false;
        String[] firstitem = objOrder.MyShoppingCartID.Split(',');
        MyIssuanceCart objMyIssuanceCart = new MyIssuanceCartRepository().GetByIssuanceCartId(Convert.ToInt64(firstitem[0]));
        UniformIssuancePolicyItem objUniformIssuancePolicyItem = new UniformIssuancePolicyItemRepository().GetById(objMyIssuanceCart.UniformIssuancePolicyItemID);

        UniformIssuancePolicy objPolicy = new UniformIssuancePolicy();
        UniformIssuancePolicyRepository objPolicyRepo = new UniformIssuancePolicyRepository();
        LookupRepository objLookupRepo = new LookupRepository();
        INC_Lookup objLook = new INC_Lookup();
        objPolicy = objPolicyRepo.GetById(Convert.ToInt64(objUniformIssuancePolicyItem.UniformIssuancePolicyID));
        if (objPolicy != null)
        {
            objLook = objLookupRepo.GetById(Convert.ToInt64(objPolicy.PRICEFOR));

            if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Both")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    returnvalue = true;
                }
                else
                {
                    returnvalue = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Employee")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                {
                    returnvalue = true;
                }
                else
                {
                    returnvalue = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Admin")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    returnvalue = true;
                }
                else
                {
                    returnvalue = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Admin")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                {
                    returnvalue = true;
                }
                else
                {
                    returnvalue = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Employee")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    returnvalue = true;
                }
                else
                {
                    returnvalue = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Both")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                {
                    returnvalue = false;
                }
                else
                {
                    returnvalue = true;
                }
            }
        }
        else
        {
            returnvalue = false;
        }

        return returnvalue;
    }
    private void sendIEEmail()
    {

        List<UserInformation> objAdminList = new List<UserInformation>();
        objAdminList = new UserInformationRepository().GetEmailInformation();
        if (objAdminList.Count > 0)
        {
            for (Int32 i = 0; i < objAdminList.Count; i++)
            {
                sendVerificationEmailAdmin(Convert.ToInt64(Session["NewOrderID"].ToString()), objAdminList[i].LoginEmail, objAdminList[i].FirstName);
            }
        }
        //End
    }
    private void sendVerificationEmailAdmin(Int64 OrderId, String IEemailAddress, String FullName)
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
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                objUsrInfo = objUsrInfoRepo.GetById(IncentexGlobal.CurrentMember.UserInfoID);
                String sToadd = "";
                sToadd = IEemailAddress;
                //sToadd = "nagmani.kumar@indianic.com";
                //Find UserName who had order purchased
                Order objOrder = new Order();
                objOrder = objRepos.GetByOrderID(OrderId);
                Int64 intUserId = Convert.ToInt64(objOrder.UserId);
                objUsrInfo = objUsrInfoRepo.GetById(intUserId);

                Int64 intCompanyiD = Convert.ToInt64(objUsrInfo.CompanyId);
                List<Company> objComName = new List<Company>();
                objComName = objCmpRepo.GetByCompanyId(intCompanyiD);
                //String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                // String sSubject = "An Order has been placed to the incentex";
                String sSubject = objComName[0].CompanyName + " - " + objOrder.OrderNumber.ToString();

                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();


                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());
                messagebody.Replace("{firstnote}", "Thank you for your business you can check the status of your order(s) any time by logging into your store <a href='http://www.world-link.us.com' title='Incentex'>here.</a>");
                messagebody.Replace("{FullName}", FullName);
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName);
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);// change by mayur on 22-nov-2011
                // messagebody.Replace("{ReferenceName}", lblPurchasedBy.Text);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());

                // Set SAP Company Contact ID and Full name
                String OrderPlacedBy = String.Empty;
                String WLContactID = String.Empty;
                GetSAPCompanyCodeIDResult objSAPCompanyResult = objUsrInfoRepo.GetSAPCompanyCodeID(objUsrInfo.UserInfoID);
                if (objSAPCompanyResult != null)
                {
                    OrderPlacedBy = String.Format("Order Placed By: {0}", objSAPCompanyResult.FullName);
                    WLContactID = String.Format("World-Link Contact ID : {0}", objSAPCompanyResult.SAPCompanyCodeID);
                }

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
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt)).ToString());
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
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt)).ToString());
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
                //Billing Address


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
                //shipping address

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
                messagebody.Replace("{Address11}", objShippingInfo.Address);
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

                #region start
                String innermessage = "";
                String innermessageSupplier = "";

                if (objOrder.OrderFor == "ShoppingCart")
                {
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
                        innermessage = innermessage + "$" + Convert.ToDecimal(obj[i].UnitPrice);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(obj[i].UnitPrice) * Convert.ToDecimal(obj[i].Quantity);
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
                        innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate) * Convert.ToDecimal(objFinal[i].Qty);
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


                messagebody.Replace("{Order Notes:}", "Order Notes :");
                messagebody.Replace("{ShippingCostView}", "Shipping :");
                //messagebody.Replace("{SalesTaxView}", "Sales Tax :");
                messagebody.Replace("{SalesTaxView}", "");
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
                //messagebody.Replace("{Saletax}", "$" + objOrder.SalesTax.ToString());
                messagebody.Replace("{Saletax}", "");
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());
                // messagebody.Replace("{PurchasesNotEligableforCreditUse}", lblNotEligibleforAnniversarycredit.Text);


                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(0, "Order Confirmation", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    private void sendEmailToSupplier(String orderid)
    {
        try
        {
            Order objOrder = new Order();
            OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
            List<SelectSupplierAddressResult> obj = new List<SelectSupplierAddressResult>();
            List<SelectSupplierAddressBySupplierIdResult> objSupplier = new List<SelectSupplierAddressBySupplierIdResult>();
            objOrder = OrderRepos.GetByOrderID(Convert.ToInt64(orderid));
            if (objOrder != null)
            {
                String[] MyShoppingcart;
                MyShoppingcart = objOrder.MyShoppingCartID.Split(',');
                obj = OrderRepos.GetSupplierAddress(objOrder.OrderFor, objOrder.OrderID);
            }
            List<Object> objRecord = new List<Object>();

            foreach (SelectSupplierAddressResult repeaterItem in obj)
            {
                //bind ORder Grid
                List<SelectShipToAddressByOrderIDResult> objShipToAddress = OrderRepos.GetShipToAddressByOrderId(objOrder.OrderID);
                objOrder = OrderRepos.GetByOrderID(objOrder.OrderID);
                if (objOrder != null)
                {
                    List<SelectOrderDetailsResult> obj1 = new List<SelectOrderDetailsResult>();
                    String[] MyShoppingcart;
                    MyShoppingcart = objOrder.MyShoppingCartID.Split(',');
                    sendVerificationEmailSupplier(Convert.ToInt64(orderid), objOrder.MyShoppingCartID, repeaterItem.SupplierID, repeaterItem.Name);

                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void sendVerificationEmailSupplier(Int64 OrderId, String ShoppingCartID, Int64 supplierId, String fullName)
    {
        try
        {
            //Get supplierinfo by id
            String supplieremail = new UserInformationRepository().GetById(new SupplierRepository().GetById(supplierId).UserInfoID).LoginEmail;
            //End

            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
            UserInformation objUsrInfo = new UserInformation();
            OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
            CompanyRepository objCmpRepo = new CompanyRepository();
            DataSet dsEmailTemplate;
            Common objcommm = new Common();
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Placed Supplier";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sToadd = "";
                sToadd = supplieremail;
                //sToadd = "mayur.rathod@indianic.com";


                //Find UserName who had order purchased
                Order objOrder = new Order();
                objOrder = objRepos.GetByOrderID(OrderId);
                Int64 intUserId = Convert.ToInt64(objOrder.UserId);
                objUsrInfo = objUsrInfoRepo.GetById(intUserId);

                //Start Subject
                //String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                Int64 intCompanyiD = Convert.ToInt64(objUsrInfo.CompanyId);
                List<Company> objComName = new List<Company>();
                objComName = objCmpRepo.GetByCompanyId(intCompanyiD);
                String sSubject = objComName[0].CompanyName + " - " + objOrder.OrderNumber;
                //End
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                //if (dsEmaiUser.Tables[0].Rows.Count > 0)
                //{
                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());
                messagebody.Replace("{firstnote}", "Please review the following order below if you have any questions please post them to the notes section of this order located within the order in the system.");
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName);
                messagebody.Replace("{OrderStatus}", "OPEN");
                //messagebody.Replace("{ReferenceName}", lblPurchasedBy.Text);
                messagebody.Replace("{FullName}", fullName);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());
                //Added on 20 Sep 2011
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
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits Paid by Corporate ";
                    }
                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
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
                    messagebody.Replace("{PaymentType}", new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                }
                else
                {
                    messagebody.Replace("{PaymentType}", "Paid By Corporate");
                }
                messagebody.Replace("{Payment Method :}", "Payment Method :");

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
                messagebody.Replace("{Address11}", objShippingInfo.Address);
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

                List<SelectSupplierAddressResult> objSupplier = new OrderConfirmationRepository().GetSupplierAddress(objOrder.OrderFor, objOrder.OrderID);

                #region Supplier



                //Get and Bind Supplier Details



                foreach (SelectSupplierAddressResult eachsupplier in objSupplier)
                {
                    if (supplierId == eachsupplier.SupplierID)
                    {
                        innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td colspan='9'>";
                        innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                        innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td align='center' colspan='2'  style='background-color:Gray;font-weight:bold;color:Black;'>";
                        innermessageSupplier = innermessageSupplier + eachsupplier.CompanyName;
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "</tr>";
                        /*innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td style='width:50%'>";
                        innermessageSupplier = innermessageSupplier + "<b>Vendor</b>:" + eachsupplier.CompanyName;
                        innermessageSupplier = innermessageSupplier + "<br />";
                        innermessageSupplier = innermessageSupplier + eachsupplier.CompanyAdddress;
                        innermessageSupplier = innermessageSupplier + "<br />";
                        innermessageSupplier = innermessageSupplier + eachsupplier.CityName + "," + eachsupplier.sStatename + " " + eachsupplier.Name + "</td>";
                        //innermessageSupplier = innermessageSupplier + "</tr>";
                        //innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td style='width: 50%'>";
                        innermessageSupplier = innermessageSupplier + "<b>Contact:</b>" + eachsupplier.Name;
                        innermessageSupplier = innermessageSupplier + "<br />";
                        innermessageSupplier = innermessageSupplier + "<b>Account #:</b>" + eachsupplier.BankAccountNumber;
                        innermessageSupplier = innermessageSupplier + "<br />";
                        innermessageSupplier = innermessageSupplier + "<b>Telephone:</b>" + eachsupplier.Telephone;
                        innermessageSupplier = innermessageSupplier + "<br />";
                        innermessageSupplier = innermessageSupplier + "<b>Email:</b>" + eachsupplier.ContactEmail;
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "</tr>";*/
                        innermessageSupplier = innermessageSupplier + "</table>";
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "</tr>";

                        //Header Row
                        //innermessageSupplier = innermessageSupplier + "<tr>";
                        //innermessageSupplier = innermessageSupplier + "<td colspan='7'>";
                        //innermessageSupplier = innermessageSupplier + "<hr />";
                        //innermessageSupplier = innermessageSupplier + "</td>";
                        //innermessageSupplier = innermessageSupplier + "</tr>";
                        innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td colspan='9'>";
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
                        innermessageSupplier = innermessageSupplier + "<td width='35%' style='font-weight: bold; text-align: center;'>";
                        innermessageSupplier = innermessageSupplier + "Description";
                        innermessageSupplier = innermessageSupplier + "</td>";
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
                        innermessageSupplier = innermessageSupplier + "<td colspan='9'>";
                        innermessageSupplier = innermessageSupplier + "<hr />";
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "</tr>";
                        //End

                        List<SelectOrderDetailsResult> obj1 = new List<SelectOrderDetailsResult>();
                        String[] MyShoppingcart;
                        MyShoppingcart = objOrder.MyShoppingCartID.Split(',');
                        for (Int32 i = 0; i < MyShoppingcart.Count(); i++)
                        {
                            obj1 = new OrderConfirmationRepository().GetDetailOrder(Convert.ToInt32(MyShoppingcart[i]), Convert.ToInt32(eachsupplier.SupplierID), objOrder.OrderFor);
                            if (obj1.Count > 0)
                            {

                                foreach (SelectOrderDetailsResult s in obj1)
                                {
                                    innermessageSupplier = innermessageSupplier + "<tr>";
                                    innermessageSupplier = innermessageSupplier + "<td colspan='9'>";
                                    innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                                    innermessageSupplier = innermessageSupplier + "<tr>";
                                    innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: left;'>";
                                    innermessageSupplier = innermessageSupplier + s.Quantity;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    innermessageSupplier = innermessageSupplier + "<td width='15%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + s.ItemNumber;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + s.Size;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + s.Color;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    innermessageSupplier = innermessageSupplier + "<td width='35%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + s.ProductDescrption;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    //Add nagmani 18-Jan-2012
                                    innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + "$" + Convert.ToDecimal(s.Price);
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + "$" + Convert.ToDecimal(s.Price) * Convert.ToDecimal(s.Quantity);
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    //End
                                    innermessageSupplier = innermessageSupplier + "</tr>";
                                    innermessageSupplier = innermessageSupplier + "</table>";
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    innermessageSupplier = innermessageSupplier + "</tr>";
                                }

                            }

                            //Update Nagmani 18-Jan-2012
                            messagebody.Replace("{ShippingCost}", "");
                            messagebody.Replace("{Saletax}", "");
                            // messagebody.Replace("{OrderTotal}", "");

                            //messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());
                            messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.SalesTax + objOrder.OrderAmount).ToString());

                            //End Nagmani
                            messagebody.Replace(" {Order Notes:}", "Order Notes :");
                            messagebody.Replace("{ShippingCostView}", "");
                            messagebody.Replace("{SalesTaxView}", "");
                            //Update Nagmani 18-Jan-2012
                            //  messagebody.Replace("{OrderTotalView}", "");
                            messagebody.Replace("{OrderTotalView}", "Order Total :");
                            //End Nagmani 
                        }

                        innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td colspan='9'>";
                        innermessageSupplier = innermessageSupplier + "<hr />";
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "</tr>";







                    }

                }

                messagebody.Replace("{innermesaageforsupplier}", innermessageSupplier);




                //End
                #endregion

                String c = NameBars(objOrder.OrderID);
                if (c != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + c);
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                }

                //  messagebody.Replace("{PurchasesNotEligableforCreditUse}", lblNotEligibleforAnniversarycredit.Text);


                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(0, "Order Confirmation", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }


    /// <summary>
    /// Sends the email to MOAS managers.
    /// Add By Mayur on 22-Nov-2011
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    private void sendEmailToMOASManager(Int64 orderid)
    {
        CompanyEmployee objCERepository = new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

        if (!String.IsNullOrEmpty(objCERepository.MOASEmailAddresses))
        {
            String[] MOASUsers = objCERepository.MOASEmailAddresses.Split(',');

            //change start on 3-feb-2012 by mayur for send email prioritywise
            List<CompanyEmployeeRepository.MOASUserWithPriority> objListMOASUserWithPriority = new List<CompanyEmployeeRepository.MOASUserWithPriority>();
            for (Int32 j = 0; j < MOASUsers.Count(); j++)
            {
                String[] MOASUsersWithPriority = MOASUsers[j].Split('|');
                CompanyEmployeeRepository.MOASUserWithPriority objMOASUserWithPriority = new CompanyEmployeeRepository.MOASUserWithPriority();
                objMOASUserWithPriority.UserInfoID = MOASUsersWithPriority[0];
                objMOASUserWithPriority.Priority = MOASUsersWithPriority[1];
                objListMOASUserWithPriority.Add(objMOASUserWithPriority);
            }
            //make order by priority and insert it into OrderMoasSystem table
            objListMOASUserWithPriority = objListMOASUserWithPriority.OrderBy(o => o.Priority).ToList();
            OrderMOASSystemRepository objOrderMOASSystemRepository = new OrderMOASSystemRepository();
            for (Int32 i = 0; i < objListMOASUserWithPriority.Count(); i++)
            {
                OrderMOASSystem objOrderMOASSystem = new OrderMOASSystem()
                {
                    OrderID = orderid,
                    ManagerUserInfoID = Convert.ToInt64(objListMOASUserWithPriority[i].UserInfoID),
                    Priority = Convert.ToInt64(objListMOASUserWithPriority[i].Priority),
                    Status = "Order Pending",
                    DateAffected = null
                };
                objOrderMOASSystemRepository.Insert(objOrderMOASSystem);
                objOrderMOASSystemRepository.SubmitChanges();

                //send email to first priority manager
                if (i == 0)
                {
                    UserInformation objUserInformation = new UserInformation();
                    objUserInformation = objUsrInfoRepo.GetById(Convert.ToInt64(objListMOASUserWithPriority[i].UserInfoID));
                    sendVerificationEmailMOASManager(orderid, objUserInformation.LoginEmail, objUserInformation.FirstName, objUserInformation.UserInfoID);
                }
            }
            //change end on 3-feb-2012 by mayur for send email priority wise
        }
    }

    /// <summary>
    /// Sends the verification email MOAS manager for approve order.
    /// Add By Mayur on 22-Nov-2011
    /// </summary>
    /// <param name="OrderNumber">The order number.</param>
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
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sToadd = "";
                sToadd = MOASEmailAddress;
                //sToadd = "mayur.rathod@indianic.com";

                String strUserName = null;
                Order objOrder = new Order();
                objOrder = objRepos.GetByOrderID(OrderId);
                Int64 intUserId = Convert.ToInt64(objOrder.UserId);
                objUsrInfo = objUsrInfoRepo.GetById(intUserId);
                if (objUsrInfo != null)
                    strUserName = (objUsrInfo.FirstName);

                Int64 intCompanyiD = Convert.ToInt64(objUsrInfo.CompanyId);
                List<Company> objComName = new List<Company>();
                objComName = objCmpRepo.GetByCompanyId(intCompanyiD);

                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString().Replace("{FullName}", strUserName + " " + objUsrInfo.LastName).Replace("{OrderNumber}", objOrder.OrderNumber);

                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();


                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());
                messagebody.Replace("{firstnote}", "Please review the following order below that requires your attention. You can Approve, Edit or Cancel this order by clicking the buttons on the bottom of this page.");
                messagebody.Replace("{FullName}", FullName);
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
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt)).ToString());
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
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt)).ToString());
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
                messagebody.Replace("{Address11}", objShippingInfo.Address);
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
                        innermessage = innermessage + "$" + Convert.ToDecimal(obj[i].UnitPrice);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(obj[i].UnitPrice) * Convert.ToDecimal(obj[i].Quantity);
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
                        innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate) * Convert.ToDecimal(objFinal[i].Qty);
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
                //messagebody.Replace("{SalesTaxView}", "Sales Tax :");
                messagebody.Replace("{SalesTaxView}", "");
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
                //messagebody.Replace("{Saletax}", "$" + objOrder.SalesTax.ToString());
                messagebody.Replace("{Saletax}", "");
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());
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

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(MOASUserId, "Order Confirmation", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
    #region
    // This is for the tracking center.
    // This is for  the get purchase update if thankyou then yes else no.
    UserTrackingRepo objtrackrepo = new UserTrackingRepo();
    UserTracking objtracktbl = new UserTracking();
    public void GetPurchaseUpdate()
    {
        objtrackrepo.SetPurchase(Convert.ToInt32(Session["trackID"]));
        //objtracktbl = objtrackrepo.GetById(IncentexGlobal.CurrentMember.UserInfoID);
        //objtracktbl.Isupdate = true;
        //objtrackrepo.SubmitChanges();
        //Int32 uid = Convert.ToInt32(objtracktbl);
        //objtrackrepo.SetPurchase(uid);


    }

    #endregion
}
