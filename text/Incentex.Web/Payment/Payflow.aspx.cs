using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using com.paypal.sdk.profiles;
using com.paypal.sdk.services;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using PayPal.Payments.Common.Utility;
using PayPal.Payments.DataObjects;
using PayPal.Payments.Transactions;

public partial class Payment_Payflow : PageBase
{
    #region Cross page postback vars
   
    public PaymentInfo objPaymentInfo { get; set; }    

    #endregion

    String PWD = ConfigurationSettings.AppSettings["PayFlowPWD"].ToString();
    String USER = ConfigurationSettings.AppSettings["PayFlowUSER"].ToString();
    String VENDOR = ConfigurationSettings.AppSettings["PayFlowVENDOR"].ToString();
    String PARTNER = ConfigurationSettings.AppSettings["PayFlowPARTNER"].ToString();

    
    protected void Page_Load(Object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            {
                //PayPal();
                try
                {
                    //Created On 1-Mar-2011 By Ankit
                    //RefundPaymentToHoder("V34A0A296F35","100.00");
                    //PayPal();

                    DoDirectPaymentCode();
                    
                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex);
                    ex = null;
                }
               
            }
        }

    }

    /// <summary>
    /// Transaction Using Api
    /// </summary>
    /// <param name="IsRecurring"></param>
    void  PayPal()
    {
        if (IncentexGlobal.PaymentDetails != null)
        {
            this.objPaymentInfo = IncentexGlobal.PaymentDetails;

            UserInfo ObjUserInfo = new UserInfo(USER, VENDOR, PARTNER, PWD);

            Invoice Inv = new Invoice();
            Currency Amt = new Currency(this.objPaymentInfo.OrderAmountToPay);

            Inv.Amt = Amt;

            Inv.Comment1 = "Incentex Payment Gateway";
            //Can Give here any comments here too.Additional Comment2.
            //Inv.Comment2 = "Payment for " + this.objPaymentInfo.SelectedPackage + " " + this.objPaymentInfo.SelectedmMonth.ToString() + " months";

            BillTo Bill = new BillTo();
            Bill.FirstName = this.objPaymentInfo.B_FirstName;
            Bill.LastName = this.objPaymentInfo.B_LastName;
            Bill.Street = this.objPaymentInfo.B_StreetAddress1;
            Bill.BillToStreet2 = this.objPaymentInfo.B_StreetAddress2;
            Bill.City = this.objPaymentInfo.B_City;
            Bill.State = this.objPaymentInfo.B_State;
            Bill.Zip = this.objPaymentInfo.B_Zipcode;
            Bill.PhoneNum = this.objPaymentInfo.B_PhoneNumber;
            Bill.Email = this.objPaymentInfo.B_Email;
            Bill.CompanyName = this.objPaymentInfo.B_CompanyName;
            Inv.BillTo = Bill;

            ShipTo Ship = new ShipTo();
            Ship.ShipToFirstName = this.objPaymentInfo.S_FirstName;
            Ship.ShipToLastName = this.objPaymentInfo.S_LastName;
            Ship.ShipToStreet = this.objPaymentInfo.S_StreetAddress1;
            Ship.ShipToStreet2 = this.objPaymentInfo.S_StreetAddress2;
            Ship.ShipToCity = this.objPaymentInfo.S_City;
            Ship.ShipToState = this.objPaymentInfo.S_State;
            Ship.ShipToZip = this.objPaymentInfo.S_Zipcode;
            Ship.ShipToPhone = this.objPaymentInfo.S_PhoneNumber;
            Ship.ShipToEmail = this.objPaymentInfo.S_Email;
            Inv.ShipTo = Ship;

            String[] a = this.objPaymentInfo.ExpiresOnYear.Split('/');
            String expirymonth = Convert.ToInt32(this.objPaymentInfo.ExpiresOnMonth).ToString("00");
            String expirydate = this.objPaymentInfo.ExpiresOnYear.Substring(2, 2);


            //CreditCard CC = new CreditCard("5105105105105100", "0109");
            // Note: Expiration date is in the format MMYY.
            CreditCard CC = new CreditCard(this.objPaymentInfo.CardNumber, expirymonth + expirydate);
            //CC.Cvv2 = "123";
            CC.Cvv2 = this.objPaymentInfo.CardVerification;
            CardTender Card = new CardTender(CC);

            //PayflowConnectionData ConnData = new PayflowConnectionData("pilot-payflowpro.paypal.com");
            PayflowConnectionData conn = new PayflowConnectionData(IncentexGlobal.Paypal_HOST_API, 443, 60);
            //PayflowConnectionData conn = new PayflowConnectionData("pilot-payflowpro.paypal.com", 443, 60);

            
            Response Resp = null;
            SaleTransaction STrans = new SaleTransaction(ObjUserInfo, conn, Inv, Card, PayflowUtility.RequestId);
            
            STrans.Verbosity = "MEDIUM";
            Resp = STrans.SubmitTransaction();
            

            if (Resp != null)
            {
                // Get the Transaction Response parameters.
                TransactionResponse TrxnResponse = Resp.TransactionResponse;

                // Refer to the Payflow Pro .NET API Reference Guide and the Payflow Pro Developer's Guide
                // for explanation of the items returned and for additional information and parameters available.
                if (TrxnResponse != null)
                {
                    //Response.Write(TrxnResponse.RespMsg);
                }

                FraudResponse FraudResp = Resp.FraudResponse;
                if (FraudResp != null)
                {

                }

                String DupMsg;
                if (TrxnResponse.Duplicate == "1")
                {
                    DupMsg = "Duplicate Transaction";
                }
                else
                {
                    DupMsg = "Not a Duplicate Transaction";
                }

               // Response.Write(("Duplicate Transaction (DUPLICATE) = " + DupMsg));

                String RespMsg;
                Boolean IsSuccess = false;

                // Evaluate Result Code
                RespMsg = GetResponseMsg(TrxnResponse.Result, TrxnResponse.AVSAddr, TrxnResponse.AVSZip, TrxnResponse.RespMsg);

               // Response.Write("<br/>Message : " + RespMsg);
                if (TrxnResponse.Result == 0)
                {
                    IsSuccess = true;
                }

                if (IsSuccess)
                {
                    //Need to Save the data with response message..
                    CompleteOrderProcess(TrxnResponse.Pnref);
                    
                }
                else
                {
                    String desc = RespMsg;
                    Response.Redirect("PaymentFail.aspx?msg=" + RespMsg.Replace("\n", "<br/>"), false);
                    //Remove Values after payment..
                    IncentexGlobal.PaymentDetails = null;
                }

            }
        }
        else
        {
            Response.Redirect("~/My Cart/PaymentDetails.aspx");
        }
        


    }

    String GetResponseMsg(Int32 Result, String AVSAddr, String AVSZip, String tRespMsg)
    {
        String RespMsg = "";
        if (Result < 0)
        {
            // Transaction failed.
            RespMsg = "There was an error processing your transaction." + Environment.NewLine + "Error: " + Result.ToString();
        }
        else if (Result == 1 || Result == 26)
        {
            // This is just checking for invalid login information.  You would not want to display this to your customers.
            RespMsg = "Account configuration issue. ";
        }
        else if (Result == 0)
        {
         
            RespMsg = "Your transaction was approved.";
        }
        else if (Result == 12)
        {
            // Hard decline from bank.
            RespMsg = "Your transaction was declined by the bank.";
        }
        else if (Result == 13)
        {
            // Voice authorization required.
            RespMsg = "Your Transaction is pending. Contact Customer Service to complete your order.";
        }
        else if (Result == 23 || Result == 24)
        {
            // Issue with credit card number or expiration date.
            RespMsg = "Invalid credit card information. Please re-enter.";
        }
        else if (Result == 125)
        {
            // 125, 126 and 127 are Fraud Responses.
            // Refer to the Payflow Pro Fraud Protection Services User's Guide or Website Payments Pro Payflow Edition - Fraud Protection Services User's Guide.
            //RespMsg = "Your Transactions has been declined. Contact Customer Service.";
            RespMsg = "Your Transactions has been declined. ";
        }
        else if (Result == 126)
        {
            // Decline transaction if AVS fails.
            if ((AVSAddr != "Y" | AVSZip != "Y"))
            {
                // Display message that transaction was not accepted.  At this time, you
                // could display message that information is incorrect and redirect user 
                // to re-enter STREET and ZIP information.  However, there should be some sort of
                // 3 strikes your out check.
                RespMsg = "Your billing information does not match. Please re-enter.";
                
                
            }
            else
            {
                RespMsg = "Your Transaction is Under Review. We will notify you via e-mail if accepted.";
            }
        }
        else if (Result == 127)
        {
            RespMsg = "Your Transaction is Under Review. We will notify you via e-mail if accepted.";
        }
        else
        {
            // Error occurred, display normalized message returned.
            RespMsg = tRespMsg;
        }

       //return "Code : " + Result + ". " + RespMsg;
       return RespMsg;
    }

    //Fuction That Updates the ORder Table's Detail and REdirect to the Thank You Page..
    private void CompleteOrderProcess(String TransactionIdFromGateway)
    {
        OrderConfirmationRepository objOdrCnfRep = new OrderConfirmationRepository();
        Order objOrder = new Order();

        CompanyEmployeeRepository objCompEmpRepo = new CompanyEmployeeRepository();
        CompanyEmployee objCompEmp = new CompanyEmployee();
        objCompEmp = objCompEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

        objOrder = objOdrCnfRep.GetRecordByOrderId(Convert.ToInt64(Session["NewOrderID"].ToString()), objCompEmp.WorkgroupID);
        objOrder.IsPaid = true; 
        objOrder.PaymentTranscationNumber = TransactionIdFromGateway;
        objOrder.UpdatedDate = System.DateTime.Now;
        objOrder.OrderStatus = "Open";
        objOdrCnfRep.SubmitChanges();

        //Decrease the Inventroy  Here if Order is confirmed
        if (Session["NewOrderID"] != null)
        {
            List<Order> obj = new List<Order>();
            OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
            MyShoppinCart objShoppingcart = new MyShoppinCart();
            MyShoppingCartRepository objShoppingCartRepos = new MyShoppingCartRepository();
            ProductItem objProductItem = new ProductItem();
            ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
            // Select shoppongcartid form   
            obj = objRepos.GetShoppingCartIdByOId(Convert.ToInt64(Session["NewOrderID"].ToString()));
            if (obj.Count > 0)
            {
                String[] a;
                a = obj[0].MyShoppingCartID.ToString().Split(',');
                foreach (String u in a)
                {
                    objShoppingcart = objShoppingCartRepos.GetById(Convert.ToInt32(u), IncentexGlobal.CurrentMember.UserInfoID);
                    if (objShoppingcart != null)
                    {
                        objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(objShoppingcart.StoreProductID), Convert.ToInt64(objShoppingcart.MasterItemNo), objShoppingcart.ItemNumber);
                        String strProcess = "Shopping";
                        String strMessage = objRepos.UpdateInventory(objShoppingcart.MyShoppingCartID, objProductItem.ProductItemID, strProcess);
                    }
                }
            }
        }
        //Remove Session Values
        //Session["OrderNumber"] = null;
        //Session["ReferenceName"] = null;
        //Session["OrderInstruction"] = null;
        //Remove Values after payment..
        IncentexGlobal.PaymentDetails = null;
        Response.Redirect("~/My Cart/ThankYou.aspx");
    }

    //void PayPal()
    //{
    //    //ICommonPostBack frm = PreviousPage as ICommonPostBack;
    //    this.objPaymentInfo = (PaymentInfo)base.GetSessionValue("objPaymentInfo");

    //    //this.objPaymentInfo = frm.objPaymentInfo;

    //    String[] a = this.objPaymentInfo.ExpiresOnYear.Split('/');
    //    //String expirymonth = (a[0].Trim().Length == 1 ? "0" + a[0].Trim() : a[0].Trim());
    //    //String expirydate = a[1].Trim().Substring(2, 2);
    //    String expirymonth = Convert.ToInt32(this.objPaymentInfo.ExpiresOnMonth).ToString("00");
    //    String expirydate = this.objPaymentInfo.ExpiresOnYear.Substring(2, 2);
    //    /*
    //   */


    //    //PayPal offers an SDK for .net that can be downloaded here: 

    //    //http://www.pdncommunity.com/pdn/board/message?board.id=payflow&message.id=569

    //    //this was created because the SDK can be somewhat confusing.



    //    //*************** Note ***********************

    //    //before using this code change the following line below: 

    //    //wrWebRequest.Headers.Add("X-VPS-VIT-Client-Certification-Id:14");

    //    //you need to change the ID to something beside 14 that will be unique



    //    //1. Set the url to send the transaction to

    //    //test

    //    //String postURL = "https://pilot-payflowpro.paypal.com";

    //    //live
    //    //String postURL ="https://payflowpro.paypal.com" ;

    //    String postURL = VinoXGlobal.Paypal_HOST;

    //    //2. Set your user info. This is case sensitive

    //    String PWD = ConfigurationSettings.AppSettings["PayFlowPWD"].ToString();
    //    String USER = ConfigurationSettings.AppSettings["PayFlowUSER"].ToString();
    //    String VENDOR = ConfigurationSettings.AppSettings["PayFlowVENDOR"].ToString();
    //    String PARTNER = ConfigurationSettings.AppSettings["PayFlowPARTNER"].ToString();


    //    //3. now create the "name value pair" String to send to the Payflow servers
    //    //more variables can be found in the docs that can be downloaded from your payflow manager
    //    StringBuilder postData = new StringBuilder();
    //    //***************add the user info***************
    //    postData.Append("PWD=" + PWD);
    //    postData.Append("&USER=" + USER);
    //    postData.Append("&VENDOR=" + VENDOR);
    //    postData.Append("&PARTNER=" + PARTNER);

    //    //***************add some required info for testing***************
    //    postData.Append("&CUSTIP=" + Request.UserHostAddress);
    //    //S for Sale. A for Auth. More in the docs
    //    postData.Append("&TRXTYPE=S");
    //    postData.Append("&AMT=" + this.objPaymentInfo.InterfaceAmountToPay.ToString("0.00"));
    //    //this is in the format MMYY
    //    postData.Append("&EXPDATE=" + expirymonth + expirydate);
    //    postData.Append("&ACCT=" + this.objPaymentInfo.CardNumber);
    //    //postData.Append("&ACCT=5105105105105100");
    //    postData.Append("&CVV2=" + this.objPaymentInfo.CardVerification);
    //    postData.Append("&FIRSTNAME=" + this.objPaymentInfo.FirstName);
    //    postData.Append("&LASTNAME=" + this.objPaymentInfo.LastName);
    //    postData.Append("&STREET=" + this.objPaymentInfo.StreetAddress1 + this.objPaymentInfo.StreetAddress2);
    //    postData.Append("&STATE=" + this.objPaymentInfo.State);
    //    postData.Append("&CITY=" + this.objPaymentInfo.City);
    //    postData.Append("&ZIP=" + this.objPaymentInfo.Zipcode);

    //    postData.Append("&COUNTRY=" + this.objPaymentInfo.Country);

    //    if (!String.IsNullOrEmpty(this.objPaymentInfo.CompanyName))
    //    {
    //        postData.Append("&COMPANYNAME=" + this.objPaymentInfo.CompanyName);
    //    }

    //    if (!String.IsNullOrEmpty(this.objPaymentInfo.Email))
    //    {
    //        postData.Append("&EMAIL=" + this.objPaymentInfo.Email);
    //    }


    //    //C is for credit card, P is for PayPal Express Checkout. More in the docs
    //    postData.Append("&TENDER=C");

    //    //*************add some optional feilds***************
    //    postData.Append("&COMMENT1=iVinoX Payment Testing");
    //    postData.Append("&COMMENT2=Payment for " + this.objPaymentInfo.SelectedPackage + " " + this.objPaymentInfo.SelectedmMonth.ToString() + " months");
    //    //make sure that the "@" is not url encoded or you will get an error
    //    //postData.Append("&email=bob@domain.com");
    //    //this is if you want to have PayPal send an IPN post
    //    //postData.Append("&NOTIFYURL=" + xxxx);
    //    //removed for now: INVNUM=12345678&
    //    //add the REQUEST_ID
    //    postData.Append("&REQUEST_ID=" + System.Guid.NewGuid().ToString());
    //    //write out the post data
    //    Response.Write("PostData:<br> " + postData + "<br><br>");

    //    Byte[] requestBytes = Encoding.UTF8.GetBytes(postData.ToString());
    //    HttpWebRequest wrWebRequest = (HttpWebRequest)WebRequest.Create(postURL);


    //    //Set WebRequest Properties
    //    wrWebRequest.Method = "POST";
    //    wrWebRequest.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
    //    //wrWebRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.7) Gecko/20060909 Firefox/1.5.0.7";
    //    wrWebRequest.UserAgent = Request.UserAgent;
    //    wrWebRequest.ContentType = "text/namevalue";
    //    wrWebRequest.ContentLength = requestBytes.Length;
    //    wrWebRequest.AllowAutoRedirect = false;

    //    //add the custom headers
    //    wrWebRequest.Headers.Add("X-VPS-Timeout:30");
    //    wrWebRequest.Headers.Add("X-VPS-VIT-Client-Architecture:x86");
    //    wrWebRequest.Headers.Add("X-VPS-VIT-Client-Certification-Id:" + System.Guid.NewGuid().ToString());
    //    wrWebRequest.Headers.Add("X-VPS-VIT-Client-Type:ASP.NET");
    //    wrWebRequest.Headers.Add("X-VPS-VIT-Client-Version:0.0.1");
    //    wrWebRequest.Headers.Add("X-VPS-VIT-Integration-Product:Homegrown");
    //    wrWebRequest.Headers.Add("X-VPS-VIT-Integration-Version:0.0.1");
    //    wrWebRequest.Headers.Add("X-VPS-VIT-OS-Name:windows");
    //    wrWebRequest.Headers.Add("X-VPS-VIT-OS-Version:2002_SP2");

    //    // write the form values into the request message
    //    Stream reqStream = wrWebRequest.GetRequestStream();
    //    reqStream.Write(requestBytes, 0, requestBytes.Length);
    //    // Get the response.
    //    HttpWebResponse hwrWebResponse = (HttpWebResponse)wrWebRequest.GetResponse();
    //    reqStream.Close();
    //    StreamReader responseReader = new StreamReader(wrWebRequest.GetResponse().GetResponseStream());
    //    String responseData = responseReader.ReadToEnd();
    //    responseReader.Close();
    //    Response.Write("Response Data:<br/>" + responseData);


    //    //Decode the response
    //    NVPCodec decoder = new NVPCodec();
    //    decoder.Decode(responseData);
    //    if (decoder["RESULT"].ToLower() == "0" && decoder["RESULT"].ToLower() != null)
    //    {

    //        //insert data in database

    //        //insert in mamber subscription

    //        SaveData(decoder["PNREF"]);
    //    }
    //    else
    //    {
    //        String error = decoder["RESULT"].ToLower();
    //        String desc = decoder["RESPMSG"].ToLower();

    //        //Response.Redirect("paymentfail.aspx?msg=" + decoder["RESPMSG"].ToLower());
    //        Response.Redirect("paymentfail.aspx?msg=" + GetResponseMsg(Convert.ToInt32(error), "", "", decoder["RESPMSG"]));
    //    }

    //}

    //Created On 1-Mar-2011 By Ankit
    void RefundPaymentToHoder(String transactionNumber,String AmountToRefund)
    {
        
        //PWD=incentex2011&USER=knelson&VENDOR=knelson&PARTNER=paypal&CUSTIP=127.0.0.1&TRXTYPE=C&TENDER=C&AMT=100&ORIGID=V34A0A296F35&COMMENT1=Refund Payment
        
        String postURL = IncentexGlobal.Paypal_HOST;
        
        //2. Set your user info. This is case sensitive

        String PWD = ConfigurationSettings.AppSettings["PayFlowPWD"].ToString();
        String USER = ConfigurationSettings.AppSettings["PayFlowUSER"].ToString();
        String VENDOR = ConfigurationSettings.AppSettings["PayFlowVENDOR"].ToString();
        String PARTNER = ConfigurationSettings.AppSettings["PayFlowPARTNER"].ToString();


        //3. now create the "name value pair" String to send to the Payflow servers
        //more variables can be found in the docs that can be downloaded from your payflow manager
        StringBuilder postData = new StringBuilder();
        //***************add the user info***************
        postData.Append("PWD=" + PWD);
        postData.Append("&USER=" + USER);
        postData.Append("&VENDOR=" + VENDOR);
        postData.Append("&PARTNER=" + PARTNER);

        //***************add some required info for testing***************
        postData.Append("&CUSTIP=" + Request.UserHostAddress);
        //S for Sale. A for Auth. More in the docs
        postData.Append("&TRXTYPE=C");
        postData.Append("&TENDER=C");
        postData.Append("&AMT=" + AmountToRefund);
        //this is in the format MMYY
        postData.Append("&ORIGID=" + transactionNumber);
        //*************add some optional feilds***************
        postData.Append("&COMMENT1=Refund Payment");
        //write out the post data
        //Response.Write("PostData:<br> " + postData + "<br><br>");

        Byte[] requestBytes = Encoding.UTF8.GetBytes(postData.ToString());
        HttpWebRequest wrWebRequest = (HttpWebRequest)WebRequest.Create(postURL);


        //Set WebRequest Properties
        wrWebRequest.Method = "POST";
        wrWebRequest.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
        //wrWebRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.7) Gecko/20060909 Firefox/1.5.0.7";
        wrWebRequest.UserAgent = Request.UserAgent;
        wrWebRequest.ContentType = "text/namevalue";
        wrWebRequest.ContentLength = requestBytes.Length;
        wrWebRequest.AllowAutoRedirect = false;

        //add the custom headers
        wrWebRequest.Headers.Add("X-VPS-Timeout:30");
        wrWebRequest.Headers.Add("X-VPS-VIT-Client-Architecture:x86");
        wrWebRequest.Headers.Add("X-VPS-VIT-Client-Certification-Id:" + System.Guid.NewGuid().ToString());
        wrWebRequest.Headers.Add("X-VPS-VIT-Client-Type:ASP.NET");
        wrWebRequest.Headers.Add("X-VPS-VIT-Client-Version:0.0.1");
        wrWebRequest.Headers.Add("X-VPS-VIT-Integration-Product:Homegrown");
        wrWebRequest.Headers.Add("X-VPS-VIT-Integration-Version:0.0.1");
        wrWebRequest.Headers.Add("X-VPS-VIT-OS-Name:windows");
        wrWebRequest.Headers.Add("X-VPS-VIT-OS-Version:2002_SP2");

        // write the form values into the request message
        Stream reqStream = wrWebRequest.GetRequestStream();
        reqStream.Write(requestBytes, 0, requestBytes.Length);
        // Get the response.
        HttpWebResponse hwrWebResponse = (HttpWebResponse)wrWebRequest.GetResponse();
        reqStream.Close();
        StreamReader responseReader = new StreamReader(wrWebRequest.GetResponse().GetResponseStream());
        String responseData = responseReader.ReadToEnd();
        responseReader.Close();
        //Response.Write("Response Data:<br/>" + responseData);


        //Decode the response
        NVPCodec decoder = new NVPCodec();
        decoder.Decode(responseData);
        if (decoder["RESULT"].ToLower() == "0" && decoder["RESULT"].ToLower() != null)
        {

            //insert data in database
            //insert in mamber subscription

            
        }
        else
        {
            String error = decoder["RESULT"].ToLower();
            String desc = decoder["RESPMSG"].ToLower();

            //Response.Redirect("paymentfail.aspx?msg=" + decoder["RESPMSG"].ToLower());
            //Response.Redirect("paymentfail.aspx?msg=" + GetResponseMsg(Convert.ToInt32(error), "", "", decoder["RESPMSG"]));
        }

    }

    public void DoDirectPaymentCode()
    {
        if (IncentexGlobal.PaymentDetails != null)
        {

            CityRepository objCity = new CityRepository();
            StateRepository objState = new StateRepository();
            CountryRepository objCountry = new CountryRepository();

            this.objPaymentInfo = IncentexGlobal.PaymentDetails;

            String[] a = this.objPaymentInfo.ExpiresOnYear.Split('/');
            String expirymonth = Convert.ToInt32(this.objPaymentInfo.ExpiresOnMonth).ToString("00");
            String expirydate = this.objPaymentInfo.ExpiresOnYear;

            //get ipaddress of user
            String ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!String.IsNullOrEmpty(ip))
            {
                String[] ipRange = ip.Split(',');
                ip = ipRange[0];
            }
            else
            {
                ip = Request.ServerVariables["REMOTE_ADDR"];
            }

            System.Guid uid = System.Guid.NewGuid();

            NVPCallerServices caller = new NVPCallerServices();
            IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();
            /*
             WARNING: Do not embed plaintext credentials in your application code.
             Doing so is insecure and against best practices.
             Your API credentials must be handled securely. Please consider
             encrypting them for use in any production environment, and ensure
             that only authorized individuals may view or modify them.
             */

            // Set up your API credentials, PayPal end point, API operation and version.
            /*Live Credential*/
            //profile.APIUsername = "lbowman_api1.incentex.com";
            //profile.APIPassword = "SY29D7VQEDERF8UN";
            //profile.APISignature = "AFcWxV21C7fd0v3bYYYRCpSSRl31A72PPxEc7HPA.AivfXoRdpPmyelW";
            //profile.Environment = "live";
            //profile.Subject = "";

            /*Sandbox Credential*/
            profile.APIUsername = "mayur._1329902388_biz_api1.indianic.com";
            profile.APIPassword = "XRVFGKHRTLQ2CG33";
            profile.APISignature = "ANKE9qAUIenRiCNEiwogMNiblbIaAHjwg450-G-55blbYB2HERgP5gxJ";
            profile.Environment = "sandbox";
            profile.Subject = "";
            
            caller.APIProfile = profile;

            NVPCodec encoder = new NVPCodec();
            encoder["VERSION"] = "64.0";
            encoder["METHOD"] = "DoDirectPayment";
            encoder["PAYMENTACTION"] = "Authorization";
            encoder["AMT"] = this.objPaymentInfo.OrderAmountToPay.ToString();
            encoder["CREDITCARDTYPE"] = this.objPaymentInfo.CardType;
            encoder["ACCT"] = this.objPaymentInfo.CardNumber;
            encoder["EXPDATE"] = expirymonth + expirydate;
            encoder["CVV2"] = this.objPaymentInfo.CardVerification;
            encoder["COUNTRYCODE"] = "US";
            encoder["CURRENCYCODE"] = "USD";
            encoder["IPADDRESS"] = ip;

            /*Billing Information*/
            encoder["FIRSTNAME"] = this.objPaymentInfo.B_FirstName;
            encoder["LASTNAME"] = this.objPaymentInfo.B_LastName;
            encoder["STREET"] = this.objPaymentInfo.B_StreetAddress1;
            encoder["STREET2"] = this.objPaymentInfo.B_StreetAddress2;
            encoder["CITY"] = this.objPaymentInfo.B_City;
            encoder["STATE"] = this.objPaymentInfo.B_State;
            encoder["ZIP"] = this.objPaymentInfo.B_Zipcode;
            encoder["SHIPTOPHONENUM"] = this.objPaymentInfo.B_PhoneNumber;
            encoder["EMAIL"] = this.objPaymentInfo.B_Email;

            /*Shipping Information*/
            encoder["SHIPTONAME"] = this.objPaymentInfo.S_FirstName + " " + this.objPaymentInfo.S_LastName;
            encoder["SHIPTOSTREET"] = this.objPaymentInfo.S_StreetAddress1;
            encoder["SHIPTOSTREET2"] = this.objPaymentInfo.S_StreetAddress2;
            encoder["SHIPTOCITY"] = this.objPaymentInfo.S_City;
            encoder["SHIPTOSTATE"] = this.objPaymentInfo.S_State;
            encoder["SHIPTOZIP"] = this.objPaymentInfo.S_Zipcode;
            encoder["SHIPTOPHONENUM"] = this.objPaymentInfo.S_PhoneNumber;


            // Execute the API operation and obtain the response.
            String pStrrequestforNvp = encoder.Encode();
            String pStresponsenvp = caller.Call(pStrrequestforNvp);


            NVPCodec decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);

            String RespMsg = decoder["ACK"];
            if (RespMsg != null && (RespMsg=="Success" || RespMsg == "SuccessWithWarning"))
            {
                //Need to Save the data with response message..
                CompleteOrderProcess(decoder["TRANSACTIONID"]);
            }
            else
            {
                Response.Redirect("PaymentFail.aspx?msg=" + decoder["L_LONGMESSAGE0"].Replace("\n", "<br/>"), false);
                //Remove Values after payment..
                IncentexGlobal.PaymentDetails = null;
            }
        }
        else
        {
            Response.Redirect("~/My Cart/PaymentDetails.aspx");
        }
    }

}//class

/*
Paypal Response Codes 
------------------------------------------------------------------
 *  refer :  PP_PayflowPro_Guide.pdf for more codes
------------------------------------------------------------------ 
0 Approved
NOTE: PayPal processor: Warning information may be returned that may be useful
to the request applicaton. See the PayPal API documentation on the PayPal
website for information on corrective actions.
Website Payments Pro Payflow Edition Developer’s Guide 59
Responses to Transaction Requests
RESULT Values and RESPMSG Text 6
1 User authentication failed. Error is caused by one or more of the following:
    􀁺 Login information is incorrect. Verify that USER, VENDOR, PARTNER, and
    PASSWORD have been entered correctly. VENDOR is your merchant ID and USER
    is the same as VENDOR unless you created a Payflow Pro user. All fields are case
    sensitive.
    􀁺 Invalid Processor information entered. Contact merchant bank to verify.
    􀁺 "Allowed IP Address" security feature implemented. The transaction is coming
    from an unknown IP address. See PayPal Manager online help for details on
    how to use Manager to update the allowed IP addresses.
    􀁺 You are using a test (not active) account to submit a transaction to the live
    PayPal servers. Change the host address from the test server URL to the live
    server URL
2 Invalid tender type. Your merchant bank account does not support the following
    credit card type that was submitted.
3 Invalid transaction type. Transaction type is not appropriate for this transaction.
    For example, you cannot credit an authorization-only transaction
4 Invalid amount format. Use the format: “#####.##” Do not include currency
    symbols or commas.
5 Invalid merchant information. Processor does not recognize your merchant
    account information. Contact your bank account acquirer to resolve this problem.
6 Invalid or unsupported currency code
7 Field format error. Invalid information entered. See RESPMSG
8 Not a transaction server
9 Too many parameters or invalid stream
10 Too many line items
11 Client time-out waiting for response
12 Declined. Check the credit card number, expiration date, and transaction
    information to make sure they were entered correctly. If this does not resolve the
    problem, have the customer call their card issuing bank to resolve.
13 Referral. Transaction cannot be approved electronically but can be approved with a
    verbal authorization. Contact your merchant bank to obtain an authorization and
    submit a manual Voice Authorization transaction.
19 Original transaction ID not found. The transaction ID you entered for this
    transaction is not valid. See RESPMSG
20 Cannot find the customer reference number
22 Invalid ABA number
------------------------------------------------------------------
 *  refer :  PP_PayflowPro_Guide.pdf for more codes
------------------------------------------------------------------ 

*/