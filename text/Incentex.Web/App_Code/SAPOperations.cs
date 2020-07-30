using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using SAP_API;
using SAP_WebService = SAP_API.ipostep_vP0010000106in_WCSX_comsapb1ivplatformruntime_INB_WS_CALL_SYNC_XPT_INB_WS_CALL_SYNC_XPTipo_proc_Service;

/// <summary>
/// Summary description for SAPOperations
/// </summary>
public class SAPOperations
{
    public SAPOperations()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private Boolean OrderSendingDone = true;
    //private Boolean UserSendingDone = true;

    private readonly Object SyncRootOrder = new Object();
    //private readonly Object SyncRootUser = new Object();

    public Boolean SubmitOrderToSAP(Int64 OrderID)
    {
        Boolean isSuccess = false;

        lock (SyncRootOrder)
        {
            OrderConfirmationRepository objRepo = new OrderConfirmationRepository();
            GetOrderDetailsForSAPResult objOrder = objRepo.GetOrderDetailsForSAP(OrderID);

            //Checking to stop duplicate transmission & MOAS Orders & Test Orders
            if (objOrder != null && !objOrder.OSentToSAP && !String.IsNullOrEmpty(objOrder.OOrderStatus) && Convert.ToString(objOrder.OOrderStatus).ToUpper() != "ORDER PENDING" && !String.IsNullOrEmpty(objOrder.OReferenceName) && !Convert.ToString(objOrder.OReferenceName).ToUpper().Contains("TEST"))
            {
                //Checking whether its not SAP Sync Downtime
                if (!Convert.ToBoolean(objOrder.IsDownTime))
                {
                    String WLRequestFilePath = String.Empty;
                    String SAPResponseFilePath = String.Empty;
                    String SAPResponseLogMessage = String.Empty;

                    Boolean HasFailed = false;
                    Boolean CanReTransmit = false;
                    Boolean IsAlreadyInSAP = false;
                    Boolean IsErrorFromSAP = false;
                    Boolean IsErrorFromWorldLink = true;
                    Boolean IsWorldLinkRequestSaved = false;
                    Boolean IsSAPResponseSaved = false;
                    Boolean IsNoResponseFromSAP = false;

                    try
                    {
                        if (!OrderSendingDone)
                            Monitor.Wait(SyncRootOrder);

                        OrderSendingDone = false;

                        #region Declaring & Initializing required objects

                        //Declaration & Initialization of the objects for Sales Order transmission to SAP
                        SalesOrderType objSalesOrder = new SalesOrderType();
                        SalesOrderTypeBO objBO = new SalesOrderTypeBO();

                        SalesOrderTypeBODocuments objDocuments = new SalesOrderTypeBODocuments();
                        SalesOrderTypeBODocumentsRow objRow = new SalesOrderTypeBODocumentsRow();

                        SalesOrderTypeBORow objLineRow;

                        SalesOrderTypeBOAddressExtension objAddressExtension = new SalesOrderTypeBOAddressExtension();
                        SalesOrderTypeBOAddressExtensionRow objAddressRow = new SalesOrderTypeBOAddressExtensionRow();

                        SalesOrderTypeBODocumentsAdditionalExpenses objDocumentsAdditionalExpenses = new SalesOrderTypeBODocumentsAdditionalExpenses();
                        SalesOrderTypeBODocumentsAdditionalExpensesRow objDocumentsAdditionalExpensesRow = new SalesOrderTypeBODocumentsAdditionalExpensesRow();

                        SalesOrderTypeBOContactEmployees objContactEmployee = new SalesOrderTypeBOContactEmployees();
                        SalesOrderTypeBOContactEmployeesRow objContactEmployeeRow = new SalesOrderTypeBOContactEmployeesRow();

                        #endregion

                        #region Header Section

                        //Setting Document Row for Sales Order Header part
                        objRow.CardCode = Convert.ToString(objOrder.SAPCompanyCode);
                        objRow.CntctCode = Convert.ToString(objOrder.SAPCompanyCode + "_" + objOrder.OUserID);
                        objRow.CreateDate = DateTime.Now.ToString("yyyyMMdd");
                        objRow.DiscSum = Convert.ToDecimal(objOrder.OCorporateDiscount).ToString();
                        objRow.DocDate = Convert.ToString(objOrder.OOrderDate.HasValue ? Convert.ToDateTime(objOrder.OOrderDate).ToString("yyyyMMdd") : "");
                        objRow.DocStatus = Convert.ToString(objOrder.OOrderStatus);

                        if (Convert.ToString(objOrder.OPaymentOptionName) != "MOAS")
                            objRow.DocTotal = Convert.ToString(Convert.ToDecimal(objOrder.OOrderAmount) + Convert.ToDecimal(objOrder.OShippingAmount) + Convert.ToDecimal(objOrder.OSalesTax) - Convert.ToDecimal(objOrder.OCorporateDiscount));
                        else
                            objRow.DocTotal = Convert.ToString(Convert.ToDecimal(objOrder.OMOASOrderAmount) + Convert.ToDecimal(objOrder.OShippingAmount) + Convert.ToDecimal(objOrder.OMOASSalesTax) - Convert.ToDecimal(objOrder.OCorporateDiscount));

                        objRow.NumAtCard = Convert.ToString(objOrder.OReferenceName);
                        objRow.U_AnnivCred = Convert.ToString(objOrder.OCreditUsed == "Anniversary" ? Convert.ToString(objOrder.OCreditAmt) : "0.0");
                        objRow.U_BillToContact = Convert.ToString(objOrder.SAPBillToCode);

                        if ((objOrder.OPaymentOptionName == "Cost-Center Code" || (objOrder.OPaymentOptionName == "MOAS" && Convert.ToBoolean(objOrder.IsMOASWithCostCenterCode))) && !String.IsNullOrEmpty(objOrder.OPaymentOptionCode))
                            objRow.U_BP_Cost_Ctr = Convert.ToString(objOrder.OPaymentOptionCode);
                        else
                            objRow.U_BP_Cost_Ctr = String.Empty;

                        if (objOrder.OPaymentOptionName == "Credit Card")
                            objRow.U_CredCard = Convert.ToString(Convert.ToDecimal(objRow.DocTotal) - Convert.ToDecimal(objRow.U_AnnivCred));
                        else
                            objRow.U_CredCard = "0.0";

                        objRow.U_EmployeeNo = Convert.ToString(objOrder.UEmployeeID);
                        objRow.U_IssuancePackage = Convert.ToString(objOrder.OOrderFor == "ShoppingCart" ? "N" : "Y");

                        if (objOrder.OPaymentOptionName == "Purchase Order" && !String.IsNullOrEmpty(objOrder.OPaymentOptionCode))
                            objRow.U_Payments_PO_No = Convert.ToString(objOrder.OPaymentOptionCode);
                        else
                            objRow.U_Payments_PO_No = String.Empty;

                        if (objOrder.OPaymentOptionName == "Employee Payroll Deduct")
                            objRow.U_PayrollDeduct = Convert.ToString(Convert.ToDecimal(objRow.DocTotal) - Convert.ToDecimal(objRow.U_AnnivCred));
                        else
                            objRow.U_PayrollDeduct = "0.0";

                        objRow.U_Station = Convert.ToString(objOrder.UBaseStation);
                        objRow.U_StoreID = Convert.ToString(objOrder.OStoreID);
                        objRow.U_U_WL_Tax = Convert.ToString(objOrder.OSalesTax);
                        objRow.U_WL_OrderNo = Convert.ToString(objOrder.OOrderNumber);
                        //objRow.U_Phone_No = Convert.ToString(objOrder.SPhoneNo);
                        //objRow.U_Email = Convert.ToString(objOrder.SEmail);

                        objRow.UpdateDate = DateTime.Now.ToString("yyyyMMdd");

                        objDocuments.row = objRow;
                        objBO.Documents = objDocuments;

                        #endregion

                        #region Line Items

                        //Getting Order Items for Sales Order Line items
                        List<SelectOrderItemsResult> lstItems = new OrderConfirmationRepository().GetOrderItems(objOrder.OOrderID);
                        objBO.Document_Lines = new SalesOrderTypeBORow[lstItems.Count];

                        Int16 iItemCount = 0;
                        foreach (SelectOrderItemsResult item in lstItems)
                        {
                            //Creating new LineRow & setting properties
                            objLineRow = new SalesOrderTypeBORow();

                            objLineRow.DelivrdQty = "0.0";
                            objLineRow.Dscription = Convert.ToString(item.ProductDescrption);
                            objLineRow.ItemCode = Convert.ToString(item.ItemNumber);
                            objLineRow.OpenQty = Convert.ToInt32(item.Quantity).ToString();

                            if (!Convert.ToBoolean(item.IsCustomerInventory))
                            {
                                if (Convert.ToString(objOrder.OPaymentOptionName) != "MOAS")
                                    objLineRow.Price = Convert.ToDecimal(item.Price).ToString();
                                else
                                    objLineRow.Price = Convert.ToDecimal(item.MOASUnitPrice).ToString();
                            }
                            else
                                objLineRow.Price = Convert.ToDecimal(0).ToString();

                            objLineRow.Quantity = Convert.ToInt32(item.Quantity).ToString();
                            objLineRow.U_BP_GL_Code = Convert.ToString(item.GLCode);
                            objLineRow.U_WorkgroupID = Convert.ToInt64(item.WorkgroupID).ToString();

                            //Adding the Order Items in Document_Lines
                            objBO.Document_Lines[iItemCount] = objLineRow;
                            iItemCount++;
                        }

                        #endregion

                        #region Shipping Address

                        //Setting the Addrees Row for Sales Order Shipping Address Part with Bill To Code
                        objAddressRow.Address = Convert.ToString(objOrder.SAddress1);
                        objAddressRow.Address2 = Convert.ToString(objOrder.SAddress2);
                        objAddressRow.CityS = Convert.ToString(objOrder.SCityName);
                        objAddressRow.CountyS = Convert.ToString(objOrder.SCounty);
                        objAddressRow.CountryS = Convert.ToString(objOrder.SCountryCode);
                        objAddressRow.StateS = Convert.ToString(objOrder.SStateCode);
                        objAddressRow.U_BillToCode = Convert.ToString(objOrder.SAPBillToCode);//"Ana Garcia"; //
                        objAddressRow.ShipToStreetNo = Convert.ToString(objOrder.SSuitApt);
                        objAddressRow.ShipToBuilding = Convert.ToString(objOrder.SFirstName) + " " + Convert.ToString(objOrder.SLastName);
                        objAddressRow.ZipCodeS = Convert.ToString(objOrder.SZipCode);

                        objAddressExtension.row = objAddressRow;
                        objBO.AddressExtension = objAddressExtension;

                        #endregion

                        #region Additional Expenses

                        objDocumentsAdditionalExpensesRow.ExpenseCode = "2";//HARD CODED FOR SAP FREIGHT TYPE (FREIGHT IN SHIPPPING)
                        objDocumentsAdditionalExpensesRow.LineTotal = Convert.ToString(objOrder.OShippingAmount);
                        objDocumentsAdditionalExpensesRow.Remarks = "";

                        objDocumentsAdditionalExpenses.row = objDocumentsAdditionalExpensesRow;
                        objBO.DocumentsAdditionalExpenses = objDocumentsAdditionalExpenses;

                        #endregion

                        #region Contact Employee

                        //ContactTypeBOContactEmployeesFirstName objFirstName = new ContactTypeBOContactEmployeesFirstName();
                        //ContactTypeBOContactEmployeesLastName objLastName = new ContactTypeBOContactEmployeesLastName();
                        //ContactTypeBOContactEmployeesMiddleName objMiddleName = new ContactTypeBOContactEmployeesMiddleName();

                        //objFirstName.nil = Convert.ToString(objOrder.UFirstName);
                        //objLastName.nil = Convert.ToString(objOrder.ULastName);
                        //objMiddleName.nil = Convert.ToString(objOrder.UMiddleName);

                        String Address2 = !String.IsNullOrEmpty(objOrder.UAddress2) ? Convert.ToString(objOrder.UAddress2 + ", ") : "";
                        String Extension = !String.IsNullOrEmpty(objOrder.UExtension) ? Convert.ToString("x" + objOrder.UExtension) : "";

                        //objEmployees.Active = "YES";
                        objContactEmployeeRow.Address = Convert.ToString(objOrder.UAddress1 + ", " + Address2 + objOrder.UCityName + ", " + objOrder.UZipCode);
                        objContactEmployeeRow.CardCode = Convert.ToString(objOrder.SAPCompanyCode);
                        objContactEmployeeRow.E_Mail = Convert.ToString(objOrder.ULoginEmail);
                        objContactEmployeeRow.Fax = Convert.ToString(objOrder.UFax);
                        objContactEmployeeRow.FirstName = Convert.ToString(objOrder.UFirstName);//objFirstName;
                        objContactEmployeeRow.Gender = Convert.ToString(objOrder.UGender).ToUpper() == "MALE" ? "gt_Male" : "gt_Female";
                        objContactEmployeeRow.LastName = Convert.ToString(objOrder.ULastName);//objLastName;
                        objContactEmployeeRow.MiddleName = Convert.ToString(objOrder.UMiddleName);//objMiddleName;
                        objContactEmployeeRow.MobilePhone = Convert.ToString(objOrder.UMobile);
                        objContactEmployeeRow.Name = Convert.ToString(objOrder.UFirstName + " " + objOrder.ULastName);
                        objContactEmployeeRow.Phone1 = Convert.ToString(objOrder.UTelephone + Extension);
                        objContactEmployeeRow.Title = Convert.ToString(objOrder.UTitle);
                        objContactEmployeeRow.U_BillToCode = Convert.ToString(objOrder.SAPBillToCode);//"Ana Garcia"; //
                        objContactEmployeeRow.U_WL_Contact_ID = Convert.ToString(objOrder.SAPCompanyCode + "_" + objOrder.UserInfoID);

                        objContactEmployee.row = objContactEmployeeRow;

                        objBO.ContactEmployees = objContactEmployee;

                        #endregion

                        objSalesOrder.BO = objBO;

                        #region Saving Request to Disk before transmission

                        try
                        {
                            WLRequestFilePath = Path.Combine(Common.SAPOrderFailedResponsePath, objOrder.OOrderNumber + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_WL_Request.xml");

                            //Saving request Object to disk before transmission
                            XmlSerializer RequestSerializer = new XmlSerializer(typeof(SalesOrderType));
                            TextWriter RequestWriter = new StreamWriter(WLRequestFilePath);
                            RequestSerializer.Serialize(RequestWriter, objSalesOrder);
                            RequestWriter.Close();

                            IsWorldLinkRequestSaved = true;
                        }
                        catch (Exception ex)
                        {
                            ErrHandler.WriteError(ex);
                        }

                        #endregion

                        //Calling SAP web service to submit Sales Order
                        SAP_WebService objService = new SAP_WebService();
                        IsErrorFromSAP = true;
                        OrderConfirmType objResponse = objService.ZaitWSSO2B1SOtest(objSalesOrder);
                        IsErrorFromSAP = false;

                        //Checking for response
                        if (objResponse != null && !String.IsNullOrEmpty(objResponse.LogStatus))
                        {
                            Order objUpdate = objRepo.GetByOrderID(objOrder.OOrderID);

                            if (objResponse.LogStatus.ToLower() == "success")
                            {
                                //Updating the flag & SAPOrderID to stop duplicate transmission on success
                                objUpdate.SentToSAP = true;
                                objUpdate.CanReTransmitToSAP = false;
                                objUpdate.SAPOrderID = objResponse.SAPOrderID;
                                isSuccess = true;
                            }
                            else
                            {
                                HasFailed = true;

                                if (!String.IsNullOrEmpty(objResponse.LogStatus) && objResponse.LogStatus.StartsWith("Skipped"))
                                {
                                    objUpdate.SentToSAP = true;
                                    objUpdate.CanReTransmitToSAP = false;
                                    objUpdate.SAPOrderID = objResponse.SAPOrderID;

                                    IsAlreadyInSAP = true;
                                }
                                else if (!String.IsNullOrEmpty(objResponse.LogMessage) && objResponse.LogMessage.StartsWith("Exception : Connect to Business One failed."))
                                {
                                    objUpdate.CanReTransmitToSAP = true;
                                    CanReTransmit = true;
                                }
                            }

                            SAPResponseLogMessage = objResponse.LogMessage;
                            objRepo.SubmitChanges();
                        }
                        else
                        {
                            HasFailed = true;
                            IsNoResponseFromSAP = true;
                        }

                        #region Saving response to disk in case of failure

                        if (!IsNoResponseFromSAP)
                        {
                            SAPResponseFilePath = Path.Combine(Common.SAPOrderFailedResponsePath, objOrder.OOrderNumber + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_SAP_Response.xml");

                            //Saving response Object to disk when the transmission fails
                            XmlSerializer ResponseSerializer = new XmlSerializer(typeof(OrderConfirmType));
                            TextWriter ResponserWriter = new StreamWriter(SAPResponseFilePath);
                            ResponseSerializer.Serialize(ResponserWriter, objResponse);
                            ResponserWriter.Close();

                            IsSAPResponseSaved = true;
                        }

                        IsErrorFromWorldLink = false;

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ErrHandler.WriteError(ex);
                        HasFailed = true;
                        IsErrorFromWorldLink = !IsErrorFromSAP;

                        #region Logic for Retransmission of Sales Orders to SAP

                        Order objUpdate = objRepo.GetByOrderID(OrderID);

                        if ((ex.Message == "Server Error" || ex.Message == "The operation has timed out") && ex.Source == "System.Web.Services")
                        {
                            objUpdate.CanReTransmitToSAP = true;
                            CanReTransmit = true;
                        }
                        else
                        {
                            objUpdate.CanReTransmitToSAP = false;
                            CanReTransmit = false;
                        }

                        objRepo.SubmitChanges();

                        #endregion
                    }
                    finally
                    {
                        #region Email Notification

                        try
                        {
                            StringBuilder emailBody = new StringBuilder();
                            String emailSubject = String.Empty;

                            if (!HasFailed)
                            {
                                emailSubject = "World-Link (" + (Common.Live ? "live" : "test") + ") : Sales order insertion to SAP succeeded - Order # " + objOrder.OOrderNumber;
                                emailBody.Append("Sales order insertion in SAP has been successful for World-Link (" + (Common.Live ? "live" : "test") + ") Order # " + objOrder.OOrderNumber + ".");
                            }
                            else
                            {
                                if (IsAlreadyInSAP)
                                {
                                    emailSubject = "World-Link (" + (Common.Live ? "live" : "test") + ") : Sales order already exists in SAP - Order # " + objOrder.OOrderNumber;
                                    emailBody.Append("World-Link (" + (Common.Live ? "live" : "test") + ") Order # " + objOrder.OOrderNumber + " already exists in SAP.");
                                    emailBody.Append("<br/><br/>Hence, sales order insertion was skipped in SAP.");
                                }
                                else
                                {
                                    emailSubject = "World-Link (" + (Common.Live ? "live" : "test") + ") : Sales order insertion to SAP failed - Order # " + objOrder.OOrderNumber;

                                    if (!IsErrorFromWorldLink && !IsErrorFromSAP)
                                    {
                                        if (!IsNoResponseFromSAP)
                                            emailBody.Append("Sales order insertion in SAP has failed for World-Link (" + (Common.Live ? "live" : "test") + ") Order # " + objOrder.OOrderNumber + ".");
                                        else
                                            emailBody.Append("Sales order insertion in SAP has failed for World-Link (" + (Common.Live ? "live" : "test") + ") Order # " + objOrder.OOrderNumber + ".");
                                    }
                                    else
                                    {
                                        if (IsErrorFromWorldLink)
                                        {
                                            emailBody.Append("An unknown error has occured on <b>World-Link</b> in Sales Order trasmission to SAP for World-Link (" + (Common.Live ? "live" : "test") + ") Order # " + objOrder.OOrderNumber + ".");
                                            emailBody.Append("<br/><br/>Please determine & fix the issue for smooth synchronization of Sales Orders between World-Link & SAP.");
                                        }
                                        else if (IsErrorFromSAP)
                                        {
                                            emailBody.Append("An unknown error has occured on <b>SAP</b> in Sales Order trasmission to SAP for World-Link (" + (Common.Live ? "live" : "test") + ") Order # " + objOrder.OOrderNumber + ".");
                                            emailBody.Append("<br/><br/>Please determine & fix the issue for smooth synchronization of Sales Orders between World-Link & SAP.");
                                        }
                                    }
                                }
                            }

                            emailBody.Append("<br/><br/>Log Message : " + SAPResponseLogMessage);

                            if (IsWorldLinkRequestSaved && IsSAPResponseSaved)
                                emailBody.Append("<br/><br/>Attached is the World-Link Sales Order request xml & SAP Response xml structures.");
                            else if (IsWorldLinkRequestSaved)
                                emailBody.Append("<br/><br/>Attached is the World-Link Sales Order request xml structure.");
                            else if (IsSAPResponseSaved)
                                emailBody.Append("<br/><br/>Attached is the SAP Response xml structure.");

                            emailBody.Append("<br/><br/>Thanks");
                            emailBody.Append("<br/>World-Link System");
                            emailBody.Append("<br/><br/><b>Note :</b> Do not reply to this email, it has been sent from an un-monitored email address.");

                            String emailNotificationList = String.Empty;
                            if (!CanReTransmit)
                            {
                                if (!HasFailed)
                                    emailNotificationList = Convert.ToString(ConfigurationManager.AppSettings["SAPOperationSucceedNotifyList"]);
                                else
                                    emailNotificationList = Convert.ToString(ConfigurationManager.AppSettings["SAPOperationFailedNotifyList"]);
                            }
                            else
                                emailNotificationList = Convert.ToString(ConfigurationManager.AppSettings["SAPOrderRetransmissionNotifyList"]);

                            List<UploadImage> lstAttachments = new List<UploadImage>();

                            if (IsWorldLinkRequestSaved)
                            {
                                UploadImage objUploadRequest = new UploadImage();
                                objUploadRequest.imageOnly = WLRequestFilePath.Substring(Common.SAPOrderFailedResponsePath.Length, WLRequestFilePath.Length - Common.SAPOrderFailedResponsePath.Length);
                                lstAttachments.Add(objUploadRequest);
                            }

                            if (IsSAPResponseSaved)
                            {
                                UploadImage objUploadResponse = new UploadImage();
                                objUploadResponse.imageOnly = SAPResponseFilePath.Substring(Common.SAPOrderFailedResponsePath.Length, SAPResponseFilePath.Length - Common.SAPOrderFailedResponsePath.Length);
                                lstAttachments.Add(objUploadResponse);
                            }

                            new CommonMails().SendMailWithMultiAttachment(0, "SAP Operation : Sales Order Transmission To SAP", CommonMails.EmailFrom, emailNotificationList, emailSubject, emailBody.ToString(), CommonMails.DisplyName, CommonMails.SSL, true, lstAttachments, Common.SAPOrderFailedResponsePath, CommonMails.SMTPHost, CommonMails.SMTPPort, CommonMails.UserName, CommonMails.Password, null, CommonMails.Live);
                        }
                        catch (Exception ex)
                        {
                            ErrHandler.WriteError(ex);
                        }

                        #endregion

                        OrderSendingDone = true;
                        Monitor.Pulse(SyncRootOrder);
                    }
                }
                else//Marking the order as re-transmittable in case its SAP Sync downtime, so that it can be transmitted at a later time
                {
                    if (!objOrder.CanReTransmitToSAP)
                    {
                        Order objDownTimeOrder = objRepo.GetByOrderID(OrderID);
                        objDownTimeOrder.CanReTransmitToSAP = true;
                        objRepo.SubmitChanges();
                    }
                }

            }//Checking for duplicate transmisstion & MOAS Order & Test Orders ends           

        }//lock ends

        return isSuccess;
    }

    //public void SubmitUserToSAP(Int64 UserInfoID, String UserEmail)
    //{
    //    lock (SyncRootUser)
    //    {
    //        UserInformationRepository objRepo = new UserInformationRepository();
    //        GetUserDetailsForSAPResult objUser = objRepo.GetUserDetailsForSAP(UserInfoID);

    //        if (objUser != null && !objUser.SentToSAP)
    //        {
    //            String WLRequestFilePath = String.Empty;
    //            String SAPResponseFilePath = String.Empty;

    //            //String ContactRequest = String.Empty;
    //            //String ContactResponse = String.Empty;

    //            Boolean IsException = false;
    //            Boolean HasFailed = false;

    //            try
    //            {
    //                if (!UserSendingDone)
    //                {
    //                    Monitor.Wait(SyncRootUser);
    //                }

    //                UserSendingDone = false;

    //                ContactType objContact = new ContactType();
    //                ContactTypeBO objBO = new ContactTypeBO();
    //                ContactTypeBOContactEmployees objEmployees = new ContactTypeBOContactEmployees();
    //                ContactTypeBOContactEmployeesFirstName objFirstName = new ContactTypeBOContactEmployeesFirstName();
    //                ContactTypeBOContactEmployeesLastName objLastName = new ContactTypeBOContactEmployeesLastName();
    //                //ContactTypeBOContactEmployeesMiddleName objMiddleName = new ContactTypeBOContactEmployeesMiddleName();

    //                String Address2 = !String.IsNullOrEmpty(objUser.Address2) ? Convert.ToString(objUser.Address2 + ", ") : "";
    //                String Extension = !String.IsNullOrEmpty(objUser.Extension) ? Convert.ToString("x" + objUser.Extension) : "";

    //                objFirstName.nil = Convert.ToString(objUser.FirstName);
    //                objLastName.nil = Convert.ToString(objUser.LastName);
    //                //objMiddleName.nil = Convert.ToString(objUser.MiddleName);

    //                //objEmployees.Active = "YES";
    //                objEmployees.Address = Convert.ToString(objUser.Address1 + ", " + Address2 + objUser.sCityName + ", " + objUser.ZipCode);
    //                objEmployees.CardCode = Convert.ToString(objUser.SAPCompanyCode);
    //                objEmployees.E_Mail = Convert.ToString(objUser.LoginEmail);
    //                //objEmployees.Fax = Convert.ToString(objUser.Fax);
    //                objEmployees.FirstName = objFirstName;
    //                objEmployees.Gender = Convert.ToString(objUser.Gender);
    //                objEmployees.LastName = objLastName;
    //                objEmployees.MiddleName = Convert.ToString(objUser.MiddleName);//objMiddleName;
    //                //objEmployees.MobilePhone = Convert.ToString(objUser.Mobile);
    //                objEmployees.Name = Convert.ToString(objUser.FirstName + " " + objUser.LastName);
    //                objEmployees.Phone1 = Convert.ToString(objUser.Telephone + Extension);
    //                //objEmployees.Title = Convert.ToString(objUser.Title);
    //                objEmployees.U_BillToCode = Convert.ToString(objUser.ManagerFirstName + " " + objUser.ManagerLastName);
    //                objEmployees.U_WL_Contact_ID = Convert.ToString(objUser.SAPCompanyCode + "_" + objUser.UserInfoID);

    //                objBO.ContactEmployees = objEmployees;

    //                objContact.BO = objBO;

    //                SAP_WebService objService = new SAP_WebService();
    //                ContactConfirmType objResponse = objService.ZaitWSUS2B1CD(objContact);

    //                if (objResponse.LogStatus.ToString().ToLower() == "success")
    //                {
    //                    UserInformation objSentUser = objRepo.GetById(objUser.UserInfoID);
    //                    objSentUser.SentToSAP = true;
    //                    objRepo.SubmitChanges();
    //                }
    //                else
    //                {
    //                    HasFailed = true;
    //                }

    //                #region Saving request & response to disk

    //                WLRequestFilePath = Path.Combine(Common.SAPUserFailedResponsePath, objUser.UserInfoID + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_WL_Request.xml");
    //                SAPResponseFilePath = Path.Combine(Common.SAPUserFailedResponsePath, objUser.UserInfoID + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_SAP_Response.xml");

    //                //ContactRequest = GetXMLFromObject(objContact);
    //                //ContactResponse = GetXMLFromObject(objResponse);

    //                XmlSerializer RequestSerializer = new XmlSerializer(typeof(ContactType));
    //                TextWriter RequestWriter = new StreamWriter(WLRequestFilePath);
    //                RequestSerializer.Serialize(RequestWriter, objContact);
    //                RequestWriter.Close();

    //                XmlSerializer ResponseSerializer = new XmlSerializer(typeof(ContactConfirmType));
    //                TextWriter ResponseWriter = new StreamWriter(SAPResponseFilePath);
    //                ResponseSerializer.Serialize(ResponseWriter, objResponse);
    //                ResponseWriter.Close();

    //                #endregion
    //            }
    //            catch (Exception ex)
    //            {
    //                ErrHandler.WriteError(ex);
    //                IsException = true;
    //                HasFailed = true;
    //            }
    //            finally
    //            {
    //                #region Email Notification

    //                try
    //                {
    //                    StringBuilder Body = new StringBuilder();
    //                    String Subject = String.Empty;

    //                    if (!HasFailed)
    //                    {
    //                        Subject = "World-Link (" + (Common.Live ? "live" : "test") + ") : Contact insertion to SAP succeeded - " + objUser.FirstName + " " + objUser.LastName + " (" + UserEmail + ")";
    //                        Body.Append("Contact insertion in SAP has been successful for World-Link (" + (Common.Live ? "live" : "test") + ") User " + objUser.FirstName + " " + objUser.LastName + " (" + UserEmail + ").");
    //                    }
    //                    else
    //                    {
    //                        Subject = "World-Link (" + (Common.Live ? "live" : "test") + ") : Contact insertion to SAP failed - " + objUser.FirstName + " " + objUser.LastName + " (" + UserEmail + ")";

    //                        if (!IsException)
    //                        {
    //                            Body.Append("Contact/User insertion in SAP has failed for World-Link (" + (Common.Live ? "live" : "test") + ") User " + objUser.FirstName + " " + objUser.LastName + " (" + UserEmail + ").");
    //                            Body.Append("<br/><br/>Attached are the World-Link User request xml & SAP Response xml structures.");
    //                        }
    //                        else
    //                        {
    //                            Body.Append("An unknown error has occured in Contact/User trasmission to SAP for World-Link (" + (Common.Live ? "live" : "test") + ") User " + objUser.FirstName + " " + objUser.LastName + " (" + UserEmail + ").");
    //                            Body.Append("<br/><br/>Please determine & fix the issue for smooth synchronization of Contacts between World-Link & SAP.");
    //                        }
    //                    }

    //                    //if (!String.IsNullOrEmpty(ContactRequest))
    //                    //{
    //                    //    Body.Append("<br/><br/><u>World-Link Request : </u>");
    //                    //    Body.Append("<br/><br/>" + ContactRequest);
    //                    //}

    //                    //if (!String.IsNullOrEmpty(ContactResponse))
    //                    //{
    //                    //    Body.Append("<br/><br/><u>SAP Response : </u>");
    //                    //    Body.Append("<br/><br/>" + ContactResponse);
    //                    //}

    //                    Body.Append("<br/><br/>Thanks");
    //                    Body.Append("<br/>World-Link System");
    //                    Body.Append("<br/><br/><b>Note :</b> Do not reply to this email, it has been sent from an un-monitored email address.");

    //                    using (System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage())
    //                    {
    //                        objEmail.Body = Body.ToString();
    //                        objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
    //                        objEmail.IsBodyHtml = true;
    //                        objEmail.Subject = Subject;

    //                        String EmailNotificationList = String.Empty;
    //                        if (!HasFailed)
    //                            EmailNotificationList = Convert.ToString(ConfigurationManager.AppSettings["SAPOperationSucceedNotifyList"]);
    //                        else
    //                            EmailNotificationList = Convert.ToString(ConfigurationManager.AppSettings["SAPOperationFailedNotifyList"]);

    //                        foreach (String objTo in EmailNotificationList.Split(','))
    //                        {
    //                            objEmail.To.Add(new MailAddress(objTo));
    //                        }

    //                        if (!IsException)
    //                        {
    //                            Attachment objWLRequest = new Attachment(WLRequestFilePath);
    //                            Attachment objSAPResponse = new Attachment(SAPResponseFilePath);

    //                            if (objWLRequest != null)
    //                                objEmail.Attachments.Add(objWLRequest);
    //                            if (objSAPResponse != null)
    //                                objEmail.Attachments.Add(objSAPResponse);
    //                        }

    //                        SmtpClient objSmtp = new SmtpClient();

    //                        objSmtp.EnableSsl = Common.SSL;
    //                        objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
    //                        objSmtp.Host = Common.SMTPHost;
    //                        objSmtp.Port = Common.SMTPPort;

    //                        objSmtp.Send(objEmail);
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    ErrHandler.WriteError(ex);
    //                }

    //                #endregion

    //                UserSendingDone = true;
    //                Monitor.Pulse(SyncRootUser);
    //            }

    //        }//Checking for duplicate transmisstion & Bill To Code ends

    //    }//lock ends
    //}

    private String GetXMLFromObject(Object o)
    {
        XmlSerializer XmlS = new XmlSerializer(o.GetType());

        StringWriter sw = new StringWriter();
        XmlTextWriter tw = new XmlTextWriter(sw);

        XmlS.Serialize(tw, o);
        return sw.ToString();
    }
}