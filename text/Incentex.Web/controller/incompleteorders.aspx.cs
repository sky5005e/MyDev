using System;
using System.Collections.Generic;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class controller_incompleteorders : System.Web.UI.Page
{
    protected void Page_Load(Object sender, EventArgs e)
    {
        OrderConfirmationRepository ordConf = new OrderConfirmationRepository();
        List<Order> objOrderList = new List<Order>();
        objOrderList = ordConf.GetInCompleteOrders();

        foreach (Order ordDetails in objOrderList)
        {
            try
            {
                OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
                if (!String.IsNullOrEmpty(ordDetails.MyShoppingCartID))
                {
                    if (ordDetails.CreditUsed != null)
                    {
                        if (ordDetails.CreditUsed != "0")
                        {
                            CompanyEmployeeRepository objCmnyEmp = new CompanyEmployeeRepository();
                            CompanyEmployee cmpEmpl = objCmnyEmp.GetByUserInfoId((Int64)ordDetails.UserId);
                            if (ordDetails.CreditUsed == "Previous")
                            {
                                cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + ordDetails.CreditAmt;
                                cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + ordDetails.CreditAmt;
                                cmpEmpl.CreditAmtToExpired = cmpEmpl.CreditAmtToExpired + ordDetails.CreditAmt;
                                objCmnyEmp.SubmitChanges();
                            }
                            else if (ordDetails.CreditUsed == "Anniversary")
                            {
                                cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + ordDetails.CreditAmt;
                                cmpEmpl.CreditAmtToExpired = cmpEmpl.CreditAmtToExpired + ordDetails.CreditAmt;
                                objCmnyEmp.SubmitChanges();
                            }

                            #region EmployeeLedger
                            EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                            EmployeeLedger objEmplLedger = new EmployeeLedger();
                            EmployeeLedger transaction = new EmployeeLedger();

                            objEmplLedger.UserInfoId = cmpEmpl.UserInfoID;
                            objEmplLedger.CompanyEmployeeId = cmpEmpl.CompanyEmployeeID;
                            objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ORINAMT.ToString(); ;
                            objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.OrderInCompletedAmount.ToString();
                            objEmplLedger.TransactionAmount = ordDetails.CreditAmt;
                            objEmplLedger.AmountCreditDebit = "Credit";
                            objEmplLedger.OrderNumber = ordDetails.OrderNumber;
                            objEmplLedger.OrderId = ordDetails.OrderID;

                            //When there will be records then get the last transaction
                            transaction = objLedger.GetLastTransactionByEmplID(cmpEmpl.CompanyEmployeeID);
                            if (transaction != null)
                            {
                                objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                                objEmplLedger.CurrentBalance = transaction.CurrentBalance + objEmplLedger.TransactionAmount;
                            }
                            //When first time record is added for the CE
                            else
                            {
                                objEmplLedger.PreviousBalance = 0;
                                objEmplLedger.CurrentBalance = objEmplLedger.TransactionAmount;
                            }

                            objEmplLedger.TransactionDate = System.DateTime.Now;
                            objEmplLedger.UpdateById = 3;
                            objLedger.Insert(objEmplLedger);
                            objLedger.SubmitChanges();
                            #endregion
                        }
                    }

                    #region inventory

                    if (!String.IsNullOrEmpty(ordDetails.MyShoppingCartID))
                    {
                        String[] a;

                        a = ordDetails.MyShoppingCartID.ToString().Split(',');
                        if (ordDetails.OrderFor == "ShoppingCart")
                        {
                            //Shopping cart
                            MyShoppingCartRepository objShoppingCartRepos = new MyShoppingCartRepository();

                            foreach (String u in a)
                            {

                                MyShoppinCart objShoppingcart = new MyShoppinCart();
                                ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                                ProductItem objProductItem = new ProductItem();
                                objShoppingcart = objShoppingCartRepos.GetById(Convert.ToInt32(u), (Int64)ordDetails.UserId);

                                if (objShoppingcart != null)
                                {
                                    objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(objShoppingcart.StoreProductID), Convert.ToInt64(objShoppingcart.MasterItemNo), objShoppingcart.ItemNumber);
                                    //Update Inventory Here 
                                    String strProcess = "Shopping";
                                    String strMessage = objRepos.IncreaseDescreaseInventory(objProductItem.ProductItemID, -Convert.ToInt32(objShoppingcart.Quantity), strProcess);

                                    objShoppingcart.IsOrdered = false;
                                    objShoppingcart.OrderID = null;
                                }
                            }

                            objShoppingCartRepos.SubmitChanges();
                        }
                        else
                        {
                            //Issuance
                            MyIssuanceCartRepository objIssuanceRepos = new MyIssuanceCartRepository();

                            foreach (String u in a)
                            {
                                MyIssuanceCart objIssuance = new MyIssuanceCart();
                                ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                                //End 

                                objIssuance = objIssuanceRepos.GetById(Convert.ToInt64(u), (Int64)ordDetails.UserId);
                                if (objIssuance != null)
                                {
                                    List<SelectProductIDResult> objList = new List<SelectProductIDResult>();
                                    Int64 storeid = new CompanyStoreRepository().GetStoreIDByCompanyId(Convert.ToInt64((new UserInformationRepository().GetById(ordDetails.UserId).CompanyId)));
                                    objList = objProItemRepos.GetProductId(objIssuance.MasterItemID, Convert.ToInt64(u), Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(storeid));
                                    //Update Inventory Here 
                                    for (Int32 i = 0; i < objList.Count; i++)
                                    {
                                        String strProcess = "UniformIssuance";
                                        String strMessage = objRepos.IncreaseDescreaseInventory(Convert.ToInt64(objList[i].ProductItemID), -Convert.ToInt32(objIssuance.Qty), strProcess);
                                    }

                                    objIssuanceRepos.Delete(objIssuance);
                                }
                            }

                            objIssuanceRepos.SubmitChanges();
                        }
                    }
                    #endregion

                    #region  Update Order Status
                    objRepos.UpdateStatus(ordDetails.OrderID, "Deleted", null, DateTime.Now);
                    #endregion
                }
                else
                {
                    objRepos.RemoveOrderById(ordDetails.OrderID, ordDetails.WorkgroupId);
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
        }
    }
}