using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class My_Cart_OrderFailed : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Order Failed";
            ((HtmlImage)Master.FindControl("imgShoppingCart")).Visible = true;
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Visible = false;
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                UndoChanges();
        }
    }

    protected void UndoChanges()
    {
        try
        {
            Int64 OrderID = Convert.ToInt64(Request.QueryString["id"]);
            OrderConfirmationRepository objOdrCnfRep = new OrderConfirmationRepository();
            Order objFailedOrder = objOdrCnfRep.GetByOrderID(OrderID);

            if (objFailedOrder != null && objFailedOrder.OrderStatus != "Deleted")
            {
                CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();

                #region Undo Order Changes
                CompanyEmployee objCE = objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

                #region Undo Cart Changes

                if (!String.IsNullOrEmpty(objFailedOrder.MyShoppingCartID))
                {
                    String[] shoppingCartArray = objFailedOrder.MyShoppingCartID.Split(',');
                    Int32[] CartIDs = shoppingCartArray.Select(x => Int32.Parse(x)).ToArray();

                    if (objFailedOrder.OrderFor == "ShoppingCart")
                    {
                        //Shopping cart
                        MyShoppingCartRepository objShoppingCartRepos = new MyShoppingCartRepository();

                        foreach (Int32 CartID in CartIDs)
                        {
                            MyShoppinCart objShoppingcart = new MyShoppinCart();
                            ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                            ProductItem objProductItem = new ProductItem();
                            objShoppingcart = objShoppingCartRepos.GetById(CartID, Convert.ToInt64(objFailedOrder.UserId));

                            if (objShoppingcart != null)
                            {
                                objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(objShoppingcart.StoreProductID), Convert.ToInt64(objShoppingcart.MasterItemNo), objShoppingcart.ItemNumber);

                                //Update Inventory Here 
                                String strProcess = "Shopping";
                                String strMessage = objOdrCnfRep.IncreaseDescreaseInventory(objProductItem.ProductItemID, -Convert.ToInt32(objShoppingcart.Quantity), strProcess);

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

                        foreach (Int32 CartID in CartIDs)
                        {
                            MyIssuanceCart objIssuance = new MyIssuanceCart();
                            ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();

                            objIssuance = objIssuanceRepos.GetById(CartID, Convert.ToInt64(objFailedOrder.UserId));
                            if (objIssuance != null)
                            {
                                List<SelectProductIDResult> objList = new List<SelectProductIDResult>();
                                Int64 storeid = new CompanyStoreRepository().GetStoreIDByCompanyId(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
                                objList = objProItemRepos.GetProductId(objIssuance.MasterItemID, CartID, Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(storeid));
                                //Update Inventory Here
                                for (Int32 i = 0; i < objList.Count; i++)
                                {
                                    String strProcess = "UniformIssuance";
                                    String strMessage = objOdrCnfRep.IncreaseDescreaseInventory(Convert.ToInt64(objList[i].ProductItemID), -Convert.ToInt32(objIssuance.Qty), strProcess);
                                }

                                objIssuanceRepos.Delete(objIssuance);
                            }
                        }

                        objIssuanceRepos.SubmitChanges();
                    }
                }

                #endregion

                objOdrCnfRep.UpdateStatus(OrderID, "Deleted", null, DateTime.Now);

                #region Undo Credits Changes

                AnniversaryCreditProgram objACP = new AnniversaryCreditProgram();
                AnniversaryProgramRepository objAnnCreditProRep = new AnniversaryProgramRepository();

                // Undo Previous Credit Changes
                if (!String.IsNullOrEmpty(objFailedOrder.CreditUsed) && objFailedOrder.CreditUsed == "Previous" && objFailedOrder.CreditAmt > 0)
                {
                    if (objCE != null)
                    {
                        objCE.StratingCreditAmount = objCE.StratingCreditAmount + objFailedOrder.CreditAmt;
                        objCE.CreditAmtToApplied = objCE.CreditAmtToApplied + objFailedOrder.CreditAmt;
                        objCE.CreditAmtToExpired = objCE.CreditAmtToExpired + objFailedOrder.CreditAmt;
                        objCompanyEmployeeRepository.SubmitChanges();
                    }
                }

                // Undo Anniversary Credit Changes
                if (!String.IsNullOrEmpty(objFailedOrder.CreditUsed) && objFailedOrder.CreditUsed == "Anniversary" && objFailedOrder.CreditAmt > 0)
                {
                    if (objCE != null)
                    {
                        objCE.CreditAmtToApplied = objCE.CreditAmtToApplied + objFailedOrder.CreditAmt;
                        objCE.CreditAmtToExpired = objCE.CreditAmtToExpired + objFailedOrder.CreditAmt;
                        objCompanyEmployeeRepository.SubmitChanges();
                    }
                }

                #endregion

                #region EmployeeLedger

                if (objFailedOrder.CreditAmt > 0)
                {
                    ////Added for the company employee ledger
                    EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                    EmployeeLedger objEmplLedger = new EmployeeLedger();
                    EmployeeLedger transaction = new EmployeeLedger();

                    objEmplLedger.UserInfoId = objCE.UserInfoID;
                    objEmplLedger.CompanyEmployeeId = objCE.CompanyEmployeeID;
                    objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ORINAMT.ToString();
                    objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.OrderInCompletedAmount.ToString();
                    objEmplLedger.TransactionAmount = objFailedOrder.CreditAmt;
                    objEmplLedger.AmountCreditDebit = "Credit";
                    objEmplLedger.OrderNumber = objFailedOrder.OrderNumber;
                    objEmplLedger.OrderId = objFailedOrder.OrderID;

                    //When there will be records then get the last transaction
                    transaction = objLedger.GetLastTransactionByEmplID(objCE.CompanyEmployeeID);
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
                    objEmplLedger.UpdateById = IncentexGlobal.CurrentMember.UserInfoID;
                    objLedger.Insert(objEmplLedger);
                    objLedger.SubmitChanges();
                    ////Starting Credits Add
                }
                #endregion

                #endregion
            }

            using (MailMessage objEmail = new MailMessage())
            {
                objEmail.Body = "Order # " + Request.QueryString["no"] + " (" + Request.QueryString["id"] + ")" + " has failed on World-Link System.<br/><br/>Determine the reason & fix the bug/issue.";
                objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                objEmail.IsBodyHtml = true;
                objEmail.Subject = "Order Failed - " + Request.QueryString["no"] + " (" + Request.QueryString["id"] + ")";
                objEmail.To.Add(new MailAddress("devraj.gadhavi@indianic.com"));
                objEmail.To.Add(new MailAddress("krushna@indianic.com"));

                SmtpClient objSmtp = new SmtpClient();

                objSmtp.EnableSsl = Common.SSL;
                objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
                objSmtp.Host = Common.SMTPHost;
                objSmtp.Port = Common.SMTPPort;

                objSmtp.Send(objEmail);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}