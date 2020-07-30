using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class controller_AnniversaryCredit : Page
{
    protected void Page_Load(Object sender, EventArgs e)
    {
        Boolean isError = false;

        #region Expire/Apply Anniversary Credits

        try
        {
            if (!IsPostBack)
            {
                AnniversaryProgramRepository objProgRepository = new AnniversaryProgramRepository();

                List<GetUsersToSetAnniversaryNExpiryDatesResult> lstSetUsers = objProgRepository.GetUsersToSetAnniversaryNExpiryDates();

                foreach (GetUsersToSetAnniversaryNExpiryDatesResult objSetUser in lstSetUsers)
                {
                    CompanyEmployeeRepository objSetCERepo = new CompanyEmployeeRepository();
                    CompanyEmployee objSetCE = objSetCERepo.GetById(objSetUser.CompanyEmployeeID);
                    objSetCE.NextCreditAppliedOn = Convert.ToDateTime(objSetUser.NextCreditAppliedOn).ToString("MM/dd/yyyy");
                    objSetCE.CreditExpireOn = Convert.ToDateTime(objSetUser.CreditExpireOn).ToString("MM/dd/yyyy");
                    objSetCERepo.SubmitChanges();
                }

                Boolean hasExpiredCredits = false;
                StringBuilder sbCreditExpiredDetails = new StringBuilder();

                List<GetUsersToApplyAnniversaryCreditResult> lstApplyUsers = objProgRepository.GetUsersToApplyAnniversaryCredit();

                foreach (GetUsersToApplyAnniversaryCreditResult objApplyUser in lstApplyUsers)
                {
                    SelectAnniversaryCreditProgramPerEmployeeResult objCE = objProgRepository.GetCompanyEmployeeAnniversaryCreditDetails(objApplyUser.UserInfoID);

                    //Update value for the next year..
                    if (objCE != null)
                    {
                        CompanyEmployeeRepository objCERepForNextYear = new CompanyEmployeeRepository();
                        CompanyEmployee objCEForNextYear = objCERepForNextYear.GetById(objApplyUser.CompanyEmployeeID);

                        //Check if CreditAmountExpiry is there or not?IF its not there then 
                        if (Convert.ToDateTime(objCE.CreditExpireAfter).Year.ToString() == "9999")
                        {
                            //Update value
                            objCEForNextYear.CreditExpireOn = "---";
                            objCEForNextYear.CreditAmtToExpired = 0;
                            objCEForNextYear.NextCreditAppliedOn = Convert.ToDateTime(objCE.NextCreditAmountToApplyDate).AddMonths(Convert.ToInt32(objApplyUser.IssueCreditIn)).ToShortDateString();
                            objCEForNextYear.CreditAmtToApplied = objCEForNextYear.CreditAmtToApplied + objApplyUser.StandardCreditAmount;
                            objCERepForNextYear.SubmitChanges();

                            #region EmployeeLedger
                            EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                            EmployeeLedger objEmplLedger = new EmployeeLedger();
                            EmployeeLedger transaction = new EmployeeLedger();

                            objEmplLedger.UserInfoId = objCEForNextYear.UserInfoID;
                            objEmplLedger.CompanyEmployeeId = objCEForNextYear.CompanyEmployeeID;
                            objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ANCR.ToString(); ;
                            objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.AnniversaryCredits.ToString();

                            objEmplLedger.TransactionAmount = Convert.ToDecimal(objCEForNextYear.CreditAmtToApplied);
                            objEmplLedger.AmountCreditDebit = "Credit";

                            //When there will be records then get the last transaction
                            transaction = objLedger.GetLastTransactionByEmplID(objCEForNextYear.CompanyEmployeeID);
                            if (transaction != null)
                            {
                                objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                                objEmplLedger.CurrentBalance = transaction.CurrentBalance + objEmplLedger.TransactionAmount;
                            }
                            //When first time record is added for the CE
                            else
                            {
                                objEmplLedger.PreviousBalance = 0;
                                objEmplLedger.CurrentBalance = Convert.ToDecimal(objCEForNextYear.CreditAmtToApplied);
                            }

                            objEmplLedger.TransactionDate = System.DateTime.Now;
                            objEmplLedger.Notes = "Issued Anniversary Credit";
                            objLedger.Insert(objEmplLedger);
                            objLedger.SubmitChanges();

                            #endregion
                        }
                        else
                        {
                            //wether the expiry date has passed yesterday
                            if (Convert.ToDateTime(objCE.NextCreditAmountToExpireDate).AddDays(1).Date == System.DateTime.Now.Date)
                            {
                                //Set next expiry date.
                                objCEForNextYear.CreditExpireOn = Convert.ToDateTime(objCE.NextCreditAmountToExpireDate).AddMonths(Convert.ToInt32(objApplyUser.CreditExpiresIn)).ToShortDateString();

                                //Remove only expired amount from the total amount (if negative then set 0)
                                objCEForNextYear.CreditAmtToApplied = Convert.ToDecimal(objCEForNextYear.CreditAmtToApplied - objCEForNextYear.CreditAmtToExpired) > 0 ? Convert.ToDecimal(objCEForNextYear.CreditAmtToApplied - objCEForNextYear.CreditAmtToExpired) : 0;

                                objCERepForNextYear.SubmitChanges();

                                #region EmployeeLedger
                                EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                                EmployeeLedger objEmplLedger = new EmployeeLedger();
                                EmployeeLedger transaction = new EmployeeLedger();

                                objEmplLedger.UserInfoId = objCEForNextYear.UserInfoID;
                                objEmplLedger.CompanyEmployeeId = objCEForNextYear.CompanyEmployeeID;
                                objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ANCR.ToString(); ;
                                objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.AnniversaryCredits.ToString();

                                objEmplLedger.TransactionAmount = Convert.ToDecimal(objCEForNextYear.CreditAmtToExpired);
                                objEmplLedger.AmountCreditDebit = "Debit";

                                //When there will be records then get the last transaction
                                transaction = objLedger.GetLastTransactionByEmplID(objCEForNextYear.CompanyEmployeeID);
                                if (transaction != null)
                                {
                                    objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                                    objEmplLedger.CurrentBalance = transaction.CurrentBalance - objEmplLedger.TransactionAmount;
                                }
                                //When first time record is added for the CE
                                else
                                {
                                    objEmplLedger.PreviousBalance = 0;
                                    objEmplLedger.CurrentBalance = Convert.ToDecimal(objCEForNextYear.CreditAmtToExpired) * -1;
                                }

                                objEmplLedger.TransactionDate = System.DateTime.Now;
                                objEmplLedger.Notes = "Expired Anniversary Credit";
                                objLedger.Insert(objEmplLedger);
                                objLedger.SubmitChanges();

                                #endregion

                                sbCreditExpiredDetails.Append("<tr>");
                                sbCreditExpiredDetails.Append("<td style='width:35%;text-align:left'>" + objApplyUser.CompanyName + "</td>");
                                sbCreditExpiredDetails.Append("<td style='width:30%;text-align:left'>" + objApplyUser.WorkGroup + "</td>");
                                sbCreditExpiredDetails.Append("<td style='width:25%;text-align:left'>" + objApplyUser.FirstName + " " + objApplyUser.LastName + "</td>");
                                sbCreditExpiredDetails.Append("<td style='width:10%;text-align:right'>" + objApplyUser.CreditAmtToExpired + "</td>");
                                sbCreditExpiredDetails.Append("</tr>");

                                hasExpiredCredits = true;
                            }

                            //wether today is the anniversary date
                            if (Convert.ToDateTime(objCE.NextCreditAmountToApplyDate).Date == System.DateTime.Now.Date)
                            {
                                //Set next anniversary date
                                objCEForNextYear.NextCreditAppliedOn = Convert.ToDateTime(objCE.NextCreditAmountToApplyDate).AddMonths(Convert.ToInt32(objApplyUser.IssueCreditIn)).ToShortDateString();

                                //add credit amount
                                objCEForNextYear.CreditAmtToApplied += objApplyUser.StandardCreditAmount;

                                //Set amount to expire after expiry date has passed.
                                objCEForNextYear.CreditAmtToExpired = objCEForNextYear.CreditAmtToApplied;

                                objCERepForNextYear.SubmitChanges();

                                #region EmployeeLedger
                                EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                                EmployeeLedger objEmplLedger = new EmployeeLedger();
                                EmployeeLedger transaction = new EmployeeLedger();

                                objEmplLedger.UserInfoId = objCEForNextYear.UserInfoID;
                                objEmplLedger.CompanyEmployeeId = objCEForNextYear.CompanyEmployeeID;
                                objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ANCR.ToString(); ;
                                objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.AnniversaryCredits.ToString();

                                objEmplLedger.TransactionAmount = Convert.ToDecimal(objApplyUser.StandardCreditAmount);
                                objEmplLedger.AmountCreditDebit = "Credit";

                                //When there will be records then get the last transaction
                                transaction = objLedger.GetLastTransactionByEmplID(objCEForNextYear.CompanyEmployeeID);
                                if (transaction != null)
                                {
                                    objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                                    objEmplLedger.CurrentBalance = transaction.CurrentBalance + objEmplLedger.TransactionAmount;
                                }
                                //When first time record is added for the CE
                                else
                                {
                                    objEmplLedger.PreviousBalance = 0;
                                    objEmplLedger.CurrentBalance = Convert.ToDecimal(objCEForNextYear.CreditAmtToApplied);
                                }

                                objEmplLedger.TransactionDate = System.DateTime.Now;
                                objEmplLedger.Notes = "Issued Anniversary Credit";
                                objLedger.Insert(objEmplLedger);
                                objLedger.SubmitChanges();

                                #endregion
                            }
                        }
                    }
                }

                #region Expired Credits Email Notification

                if (hasExpiredCredits)
                {
                    StreamReader streamReader;
                    streamReader = File.OpenText(Server.MapPath("~/emailtemplate/AnniversaryCreditExpired.htm"));

                    StringBuilder eMailTemplate = new StringBuilder(streamReader.ReadToEnd());

                    streamReader.Close();
                    streamReader.Dispose();

                    eMailTemplate.Replace("{CreditExpiredDetails}", sbCreditExpiredDetails.ToString());

                    using (MailMessage objEmail = new MailMessage())
                    {
                        objEmail.Body = eMailTemplate.ToString();
                        objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                        objEmail.IsBodyHtml = true;
                        objEmail.Subject = "Annivesary Credit Scheduler - Notification for expired credits";
                        objEmail.To.Add(new MailAddress(ConfigurationManager.AppSettings["AnniversaryCreditExpiredNotificationEmailAddress"]));

                        SmtpClient objSmtp = new SmtpClient();

                        objSmtp.EnableSsl = Common.SSL;

                        if (!Common.Live)
                            objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);

                        objSmtp.Host = Common.SMTPHost;
                        objSmtp.Port = Common.SMTPPort;

                        objSmtp.Send(objEmail);
                    }
                }

                #endregion
            }
        }
        catch (Exception ex)
        {
            isError = true;
            ErrHandler.WriteError(ex);
        }
        finally
        {
            try
            {
                using (MailMessage objEmail = new MailMessage())
                {
                    if (!isError)
                    {
                        objEmail.Body = "The Annivesary Credit Scheduler has run successfully for the day.";
                        objEmail.Subject = "Annivesary Credit Scheduler - Succeeded";
                    }
                    else
                    {
                        objEmail.Body = "There was an error in Annivesary Credit Scheduler for the day.";
                        objEmail.Subject = "Annivesary Credit Scheduler - Failed";
                    }

                    objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                    objEmail.IsBodyHtml = true;

                    objEmail.To.Add(new MailAddress("a-credit@world-link.us.com"));

                    SmtpClient objSmtp = new SmtpClient();

                    objSmtp.EnableSsl = Common.SSL;

                    if (!Common.Live)
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

        #endregion

        #region Auditor Logic

        isError = false;

        try
        {
            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
            List<GetMismatchBetweenCreditBalanceAndLedgerBalanceResult> lstMismatchingAccounts = objEmpRepo.GetMismatchBetweenCreditBalanceAndLedgerBalance();

            if (lstMismatchingAccounts.Count > 0)
            {
                StringBuilder sbMismatchDetails = new StringBuilder();

                foreach (GetMismatchBetweenCreditBalanceAndLedgerBalanceResult objMismatch in lstMismatchingAccounts)
                {
                    sbMismatchDetails.Append("<tr>");
                    sbMismatchDetails.Append("<td style='width:45%;text-align:left'>" + objMismatch.FirstName + " " + objMismatch.LastName + "</td>");
                    //sbMismatchDetails.Append("<td style='width:25%;text-align:left'>" + objMismatch.LoginEmail + "</td>");
                    sbMismatchDetails.Append("<td style='width:35%;text-align:left'>" + objMismatch.CompanyName + "</td>");
                    sbMismatchDetails.Append("<td style='width:10%;text-align:right'>" + objMismatch.CreditAmtToApplied + "</td>");
                    sbMismatchDetails.Append("<td style='width:10%;text-align:right'>" + objMismatch.CurrentBalance + "</td>");
                    sbMismatchDetails.Append("</tr>");
                }

                StreamReader streamReader;
                streamReader = File.OpenText(Server.MapPath("~/emailtemplate/MismatchInLedger.htm"));

                StringBuilder eMailTemplate = new StringBuilder(streamReader.ReadToEnd());

                streamReader.Close();
                streamReader.Dispose();

                eMailTemplate.Replace("{MismatchDetails}", sbMismatchDetails.ToString());

                using (MailMessage objEmail = new MailMessage())
                {
                    objEmail.Body = eMailTemplate.ToString();
                    objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                    objEmail.IsBodyHtml = true;
                    objEmail.Subject = "Mismatch found in Credit Amount & Ledger Balance";
                    objEmail.To.Add(new MailAddress(ConfigurationManager.AppSettings["AuditorNotificationEmailAddress"]));

                    SmtpClient objSmtp = new SmtpClient();

                    objSmtp.EnableSsl = Common.SSL;

                    if (!Common.Live)
                        objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);

                    objSmtp.Host = Common.SMTPHost;
                    objSmtp.Port = Common.SMTPPort;

                    objSmtp.Send(objEmail);
                }
            }
        }
        catch (Exception ex)
        {
            isError = true;
            ErrHandler.WriteError(ex);
        }
        finally
        {
            try
            {
                using (MailMessage objEmail = new MailMessage())
                {
                    if (!isError)
                    {
                        objEmail.Body = "The Auditor has run successfully for the day.";
                        objEmail.Subject = "Audit succeeded";
                    }
                    else
                    {
                        objEmail.Body = "There was an error in Auditor for the day.";
                        objEmail.Subject = "Audit Failed";
                    }

                    objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                    objEmail.IsBodyHtml = true;

                    objEmail.To.Add(new MailAddress("a-credit@world-link.us.com"));

                    SmtpClient objSmtp = new SmtpClient();

                    objSmtp.EnableSsl = Common.SSL;

                    if (!Common.Live)
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

        #endregion
    }
}