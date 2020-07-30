using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using LumiSoft.Net;
using LumiSoft.Net.IMAP;
using LumiSoft.Net.IMAP.Client;
using MailBee.ImapMail;
using MailBee.Mime;
using OpenPop.Mime;
using OpenPop.Pop3;

public partial class controller_OpenTicketViaEmail : System.Web.UI.Page
{
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetIMAPEmails();

            #region Lumisoft Code

            //List<IMAP_Envelope> lstRequests = new List<IMAP_Envelope>() { };

            //using (IMAP_Client client = new IMAP_Client())
            //{
            //    client.Connect(Common.IMAPHost, Common.IMAPPort, Common.SSL);
            //    client.Login(Common.OpenTicket, Common.OpenTicketPassword);
            //    client.SelectFolder("INBOX");

            //    IMAP_SequenceSet sequence = new IMAP_SequenceSet();
            //    //sequence.Parse("*:1"); // from first to last

            //    IMAP_Client_FetchHandler fetchHandler = new IMAP_Client_FetchHandler();
            //    fetchHandler.NextMessage += new EventHandler(delegate(object s, EventArgs eNexMessage) { });
            //    fetchHandler.Envelope += new EventHandler<EventArgs<IMAP_Envelope>>(delegate(object s, EventArgs<IMAP_Envelope> eEnvelope)
            //    {
            //        lstRequests.Add(eEnvelope.Value);
            //    });

            //    // the best way to find unread emails is to perform server search                
            //    Int32[] unseen_ids = client.Search(false, null, "unseen");

            //    if (unseen_ids.Length > 0)
            //    {
            //        System.Text.StringBuilder sbUnseenIDs = new System.Text.StringBuilder();
            //        foreach (Int32 unseen_id in unseen_ids)
            //        {
            //            if (sbUnseenIDs.Length <= 0)
            //            {
            //                sbUnseenIDs.Append(Convert.ToString(unseen_id));
            //            }
            //            else
            //            {
            //                sbUnseenIDs.Append("," + Convert.ToString(unseen_id));
            //            }
            //        }

            //        // now we need to initiate our sequence of messages to be fetched                    
            //        sequence.Parse(sbUnseenIDs.ToString());

            //        // fetch messages now                    
            //        IMAP_Fetch_DataItem[] lstDataItem = new IMAP_Fetch_DataItem[] { new IMAP_Fetch_DataItem_Envelope() };
            //        client.Fetch(false, sequence, lstDataItem, fetchHandler);

            //        if (lstRequests.Count > 0)
            //        {
            //            List<Message> lstPOP = FetchAllMessages(Common.POPHost, Common.POPPort, Common.SSL, Common.OpenTicket, Common.OpenTicketPassword);

            //            if (lstPOP.Count > 0)
            //            {
            //                foreach (IMAP_Envelope objRequest in lstRequests)
            //                {
            //                    String ToAddress = objRequest.To[0].ToString();

            //                    foreach (Message objPopMsg in lstPOP)
            //                    {
            //                        if (objPopMsg.Headers.MessageId == objRequest.MessageID || objPopMsg.Headers.MessageId.Contains(objRequest.MessageID) || objRequest.MessageID.Contains(objPopMsg.Headers.MessageId))
            //                        {
            //                            try
            //                            {
            //                                String sUserInfoID = String.Empty;

            //                                if (ToAddress.Contains("+un"))
            //                                {
            //                                    sUserInfoID = ToAddress.Substring(ToAddress.LastIndexOf("+un") + 3, ToAddress.LastIndexOf("@") - ToAddress.LastIndexOf("+un") - 3);
            //                                }
            //                                else
            //                                {
            //                                    UserInformation objUser = new UserInformationRepository().GetByEmail(objPopMsg.Headers.From.Address.ToString());
            //                                    sUserInfoID = objUser != null ? Convert.ToString(objUser.UserInfoID) : "annx";
            //                                }

            //                                String UserEmail = String.Empty;
            //                                String strNoteFor = String.Empty;
            //                                String UserName = "User";

            //                                Int64? UserInfoID = null;
            //                                Int64? CompanyID = null;
            //                                Int64? UserType = null;
            //                                Int64? OwnerID;
            //                                Int32 NoteType = 2;

            //                                System.Net.Mail.MailMessage objMsg = objPopMsg.ToMailMessage();

            //                                String Question = String.Empty;
            //                                Question = cleanMsgBody(Convert.ToString(objMsg.Body));

            //                                if (sUserInfoID != "annx")
            //                                {
            //                                    UserInfoID = Convert.ToInt64(sUserInfoID);
            //                                    UserInformation objUser = new UserInformationRepository().GetById(UserInfoID);
            //                                    if (objUser != null)
            //                                    {
            //                                        CompanyID = (objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) && objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin)) ? objUser.CompanyId : 8;
            //                                        UserType = objUser.Usertype;
            //                                        UserEmail = objUser.Email;
            //                                        NoteType = (objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) && objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin)) ? (objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee)) ? 1 : 3 : 2;
            //                                        strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName((objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) && objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin)) ? (objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee)) ? Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs : Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketSupps : Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);
            //                                        UserName = objUser.FirstName + " " + objUser.LastName;
            //                                    }

            //                                    if (Question.Length > 2500)
            //                                    {
            //                                        Question = Question.Substring(0, 2500);
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    String AnonymousEmail = Convert.ToString(objMsg.From);
            //                                    if (Question.Length + AnonymousEmail.Length + 3 > 2500)
            //                                    {
            //                                        Question = Question.Substring(0, 2500 - AnonymousEmail.Length + 3) + " (" + AnonymousEmail + ")";
            //                                    }
            //                                    else
            //                                    {
            //                                        Question = Question + " (" + AnonymousEmail + ")";
            //                                    }
            //                                }

            //                                ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            //                                Int64 TicketID = objSerTicRep.InsertTicket(UserInfoID, CompanyID, UserType, objMsg.Subject, Question, null, UserEmail, out OwnerID);
            //                                objSerTicRep.InsertTicketNote(TicketID, UserInfoID, Question, strNoteFor, NoteType == 2 ? "IEInternalNotes" : null);

            //                                //UserInformationRepository objUserRepo = new UserInformationRepository();
            //                                //UserInformation objOwner = objUserRepo.GetById(OwnerID);

            //                                //StreamReader _StreamReader;
            //                                //_StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/ServiceTicketNote.htm"));
            //                                //String eMailTemplate = String.Empty;
            //                                //eMailTemplate = _StreamReader.ReadToEnd();
            //                                //_StreamReader.Close();
            //                                //_StreamReader.Dispose();

            //                                //Common objcommon = new Common();
            //                                //StringBuilder MessageBody = new StringBuilder(eMailTemplate);

            //                                #region Send Receipt

            //                                //Email Management                
            //                                //if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
            //                                //{   
            //                                //    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            //                                //    MessageBody.Replace("{FullName}", UserName);
            //                                //    MessageBody.Replace("{TicketNo}", Convert.ToString(TicketID));
            //                                //    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
            //                                //    MessageBody.Replace("{Sender}", objOwner == null ? "World-Link Team" : objOwner.FirstName + " " + objOwner.LastName);
            //                                //    MessageBody.Replace("{Note}", "<br/>" + Convert.ToString(Question));
            //                                //    MessageBody.Replace("{CloseTicket}", String.Empty);

            //                                //    objcommon.SendServiceTicketReciept(TicketID, Convert.ToString(UserInfoID), NoteType, UserEmail, MessageBody.ToString(), Convert.ToString(Application["SMTPHOST"]), Convert.ToString(Application["SMTPPORT"]), Convert.ToString(objMsg.Subject).Trim());
            //                                //}

            //                                #endregion

            //                                #region Send Alert To Owner

            //                                //Email Management                
            //                                //if (objManageEmailRepo.CheckEmailAuthentication(objOwner.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
            //                                //{
            //                                //    MessageBody = new StringBuilder(eMailTemplate);
            //                                //    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            //                                //    MessageBody.Replace("{FullName}", objOwner.FirstName + " " + objOwner.LastName);
            //                                //    MessageBody.Replace("{TicketNo}", Convert.ToString(TicketID));
            //                                //    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
            //                                //    MessageBody.Replace("{Sender}", UserName);
            //                                //    MessageBody.Replace("{Note}", "<br/>" + Question);
            //                                //
            //                                //    String CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
            //                                //<a href='" + ConfigurationSettings.AppSettings["siteurl"]
            //                                //                        + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
            //                                //                        + TicketID + "&uid=" + objOwner.UserInfoID
            //                                //                        + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";
            //                                //
            //                                //    MessageBody.Replace("{CloseTicket}", CloseTicket);

            //                                //   objcommon.SendServiceTicketReciept(TicketID, Convert.ToString(objOwner.UserInfoID), NoteType, objOwner.Email, MessageBody.ToString(), Convert.ToString(Application["SMTPHOST"]), Convert.ToString(Application["SMTPPORT"]), Convert.ToString(objMsg.Subject).Trim());
            //                                //}

            //                                #endregion
            //                            }
            //                            catch (Exception ex)
            //                            {
            //                                ErrHandler.WriteError(ex);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }

            //        client.StoreMessageFlags(false, sequence, IMAP_Flags_SetType.Add, IMAP_MessageFlags.Seen);
            //        client.StoreMessageFlags(false, sequence, IMAP_Flags_SetType.Add, IMAP_MessageFlags.Deleted);
            //    }

            //    //client.CloseFolder();
            //    client.Expunge();
            //    client.Disconnect();
            //    client.Dispose();
            //}

            #endregion
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Example showing:
    ///  - how to fetch all messages from a POP3 server
    /// </summary>
    /// <param name="hostname">Hostname of the server. For example: pop3.live.com</param>
    /// <param name="port">Host port to connect to. Normally: 110 for plain POP3, 995 for SSL POP3</param>
    /// <param name="useSsl">Whether or not to use SSL to connect to server</param>
    /// <param name="username">Username of the user on the server</param>
    /// <param name="password">Password of the user on the server</param>
    /// <returns>All Messages on the POP3 server</returns>
    private List<Message> FetchAllMessages(String hostname, Int32 port, Boolean useSsl, String username, String password)
    {
        // The client disconnects from the server when being disposed        
        using (Pop3Client client = new Pop3Client())
        {
            // Connect to the server            
            client.Connect(hostname, port, useSsl);

            // Authenticate ourselves towards the server            
            client.Authenticate(username, password);

            // Get the number of messages in the inbox            
            Int32 messageCount = client.GetMessageCount();

            // We want to download all messages            
            List<Message> allMessages = new List<Message>(messageCount);

            // Messages are numbered in the interval: [1, messageCount]
            // Ergo: message numbers are 1-based.            
            for (int i = 1; i <= messageCount; i++)
            {
                allMessages.Add(client.GetMessage(i));
                //client.DeleteMessage(i);
            }

            client.Disconnect();
            client.Dispose();

            // Now return the fetched messages            
            return allMessages;
        }
    }

    private void GetIMAPEmails()
    {
        using (Imap imap = new Imap())
        {
            if (!Common.SSL)
                imap.Connect(Common.IMAPHost);//Without SSL
            else
                imap.Connect(Common.IMAPHost, Common.IMAPPort);//With SSL (i.e. SSL = true)

            imap.Login(Common.OpenTicket, Common.OpenTicketPassword);// You can also use: LoginPLAIN, LoginCRAM, LoginDIGEST, LoginOAUTH methods,
            // or use UseBestLogin method if you want Mail.dll to choose for you.

            imap.SelectFolder("Inbox");// You can select other folders, e.g. Sent folder: imap.Select("Sent");

            UidCollection uids = (UidCollection)imap.Search(true, "unseen", null);// Find all unseen messages.

            if (uids.Count > 0)
            {
                MailMessageCollection msgs = imap.DownloadEntireMessages(uids.ToString(), true);

                foreach (MailBee.Mime.MailMessage msg in msgs)
                {
                    ProcessMessage(msg);// Display email data, save attachments.
                }

                imap.DeleteMessages(uids.ToString(), true);
            }

            imap.Close();
            imap.Disconnect();
        }
    }

    private void ProcessMessage(MailBee.Mime.MailMessage email)
    {
        //foreach (MailBee.Mime.Attachment attachment in email.Attachments)
        //{
        //    Response.Write(attachment.Filename + "</br>");
        //    //attachment.Save(@"c:\" + attachment.Filename, true);
        //}

        try
        {
            String ToAddress = email.To[0].ToString();

            String sUserInfoID = String.Empty;

            if (ToAddress.Contains("+un"))
            {
                sUserInfoID = ToAddress.Substring(ToAddress.LastIndexOf("+un") + 3, ToAddress.LastIndexOf("@") - ToAddress.LastIndexOf("+un") - 3);
            }
            else
            {
                UserInformation objUser = new UserInformationRepository().GetByEmail(email.From.ToString());
                sUserInfoID = objUser != null ? Convert.ToString(objUser.UserInfoID) : "annx";
            }

            String UserEmail = String.Empty;
            String strNoteFor = String.Empty;
            String UserName = "User";

            Int64? UserInfoID = null;
            Int64? CompanyID = null;
            Int64? UserType = null;
            Int64? OwnerID;
            Int32 NoteType = 2;

            String Question = String.Empty;
            Question = cleanMsgBody(Convert.ToString(email.BodyPlainText));

            if (sUserInfoID != "annx")
            {
                UserInfoID = Convert.ToInt64(sUserInfoID);
                UserInformation objUser = new UserInformationRepository().GetById(UserInfoID);
                if (objUser != null)
                {
                    CompanyID = (objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) && objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin)) ? objUser.CompanyId : 8;
                    UserType = objUser.Usertype;
                    UserEmail = objUser.Email;
                    NoteType = (objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) && objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin)) ? (objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee)) ? 1 : 3 : 2;
                    strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName((objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) && objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin)) ? (objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee)) ? Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs : Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketSupps : Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);
                    UserName = objUser.FirstName + " " + objUser.LastName;
                }

                if (Question.Length > 2500)
                {
                    Question = Question.Substring(0, 2500);
                }
            }
            else
            {
                String AnonymousEmail = Convert.ToString(email.From);
                if (Question.Length + AnonymousEmail.Length + 3 > 2500)
                {
                    Question = Question.Substring(0, 2500 - AnonymousEmail.Length + 3) + " (" + AnonymousEmail + ")";
                }
                else
                {
                    Question = Question + " (" + AnonymousEmail + ")";
                }
            }

            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            Int64 TicketID = objSerTicRep.InsertTicket(UserInfoID, CompanyID, UserType, email.Subject, Question, null, UserEmail, out OwnerID);
            objSerTicRep.InsertTicketNote(TicketID, UserInfoID, Question, strNoteFor, NoteType == 2 ? "IEInternalNotes" : null);

            //UserInformationRepository objUserRepo = new UserInformationRepository();
            //UserInformation objOwner = objUserRepo.GetById(OwnerID);

            //StreamReader _StreamReader;
            //_StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/ServiceTicketNote.htm"));
            //String eMailTemplate = String.Empty;
            //eMailTemplate = _StreamReader.ReadToEnd();
            //_StreamReader.Close();
            //_StreamReader.Dispose();

            //Common objcommon = new Common();
            //StringBuilder MessageBody = new StringBuilder(eMailTemplate);

            #region Send Receipt

            //Email Management                
            //if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
            //{   
            //    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            //    MessageBody.Replace("{FullName}", UserName);
            //    MessageBody.Replace("{TicketNo}", Convert.ToString(TicketID));
            //    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
            //    MessageBody.Replace("{Sender}", "World-Link System");
            //    MessageBody.Replace("{Note}", "<br/>" + Convert.ToString(Question));
            //    MessageBody.Replace("{CloseTicket}", String.Empty);

            //    objcommon.SendServiceTicketReciept(TicketID, Convert.ToString(UserInfoID), NoteType, UserEmail, MessageBody.ToString(), Convert.ToString(Application["SMTPHOST"]), Convert.ToString(Application["SMTPPORT"]), Convert.ToString(objMsg.Subject).Trim());
            //}

            #endregion

            #region Send Alert To Owner

            //Email Management                
            //if (objManageEmailRepo.CheckEmailAuthentication(objOwner.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
            //{
            //    MessageBody = new StringBuilder(eMailTemplate);
            //    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            //    MessageBody.Replace("{FullName}", objOwner.FirstName + " " + objOwner.LastName);
            //    MessageBody.Replace("{TicketNo}", Convert.ToString(TicketID));
            //    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
            //    MessageBody.Replace("{Sender}", UserName);
            //    MessageBody.Replace("{Note}", "<br/>" + Question);
            //
            //    String CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
            //<a href='" + ConfigurationSettings.AppSettings["siteurl"]
            //                        + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
            //                        + TicketID + "&uid=" + objOwner.UserInfoID
            //                        + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";
            //
            //    MessageBody.Replace("{CloseTicket}", CloseTicket);

            //   objcommon.SendServiceTicketReciept(TicketID, Convert.ToString(objOwner.UserInfoID), NoteType, objOwner.Email, MessageBody.ToString(), Convert.ToString(Application["SMTPHOST"]), Convert.ToString(Application["SMTPPORT"]), Convert.ToString(objMsg.Subject).Trim());
            //}

            #endregion
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public String cleanMsgBody(String oBody)
    {
        Regex sentOn = new Regex("\\nSent from my.*(\\n|\\r)On.*(AM|PM), (Incentex|\"Incentex\")", RegexOptions.IgnoreCase);
        Regex rx1 = new Regex("\n-----");
        Regex rx2 = new Regex("\n([^\n]+):([ \t\r\n\v\f]+)>");
        Regex rx3 = new Regex("([0-9]+)/([0-9]+)/([0-9]+)([^\n]+)<([^\n]+)>");
        Regex rx4 = new Regex("(\nFrom:\\s*|\rFrom:\\s*|From:\\s*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex rx5 = new Regex("<" + Regex.Escape(oBody) + ">", RegexOptions.IgnoreCase);
        Regex rx6 = new Regex(Regex.Escape(oBody) + "\\s+wrote:", RegexOptions.IgnoreCase);
        Regex rx7 = new Regex("\\n.*On.*(\\r\\n)?wrote:\\r\\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex rx8 = new Regex("-+original\\s+message-+\\s*$", RegexOptions.IgnoreCase);
        Regex rx9 = new Regex("from:\\s*$", RegexOptions.IgnoreCase);
        Regex rx10 = new Regex("^>.*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        Regex rx11 = new Regex("^.*On.*(\n)?wrote:$", RegexOptions.IgnoreCase);
        Regex rx12 = new Regex("\\nOn.*(AM|PM), (Incentex|\"Incentex\")", RegexOptions.IgnoreCase);
        Regex rx13 = new Regex("\\r\\nOn.*(\\r\\n)?wrote:\\r\\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        //Signatures
        Regex Thanks = new Regex("([\r\n\f]Thanks[\r\n\f]\\s*|[\r\n\f]Thanks,[\r\n\f]\\s*|[\r\n\f]\\*Thanks[\r\n\f]\\s*|[\r\n\f]\\*Thanks,[\r\n\f]\\s*)", RegexOptions.IgnoreCase);
        Regex ThanksNRegards = new Regex("([\r\n\f]Thanks & Regards\\s*|[\r\n\f]Thanks and Regards\\s*|[\r\n\f]\\*Thanks & Regards\\s*|[\r\n\f]\\*Thanks and Regards\\s*)", RegexOptions.IgnoreCase);
        Regex Regards = new Regex("([\r\n\f]Regards\\s*|[\r\n\f]\\*Regards\\s*)", RegexOptions.IgnoreCase);
        Regex WithRegards = new Regex("([\r\n\f]With Regards\\s*|[\r\n\f]With.*Regards\\s*|[\r\n\f]\\*With Regards\\s*|[\r\n\f]\\*With.*Regards\\s*)", RegexOptions.IgnoreCase);
        Regex WarmRegards = new Regex("([\r\n\f]Warm Regards\\s*|[\r\n\f]\\*Warm Regards\\s*)", RegexOptions.IgnoreCase);
        Regex BestRegards = new Regex("([\r\n\f]Best Regards\\s*|[\r\n\f]\\*Best Regards\\s*)", RegexOptions.IgnoreCase);
        Regex Yours = new Regex("([\r\n\f]Yours\\s*|[\r\n\f]\\*Yours\\s*)", RegexOptions.IgnoreCase);

        String txtBody = oBody;

        if (sentOn.IsMatch(txtBody))
            txtBody = sentOn.Split(txtBody)[0]; // Maybe a loop through would be better
        if (rx1.IsMatch(txtBody))
            txtBody = rx1.Split(txtBody)[0]; // Maybe a loop through would be better
        if (rx2.IsMatch(txtBody))
            txtBody = rx2.Split(txtBody)[0]; // Maybe a loop through would be better
        if (rx3.IsMatch(txtBody))
            txtBody = rx3.Split(txtBody)[0]; // Maybe a loop through would be better
        if (rx4.IsMatch(txtBody))
            txtBody = rx4.Split(txtBody)[0]; // Maybe a loop through would be better
        if (rx5.IsMatch(txtBody))
            txtBody = rx5.Split(txtBody)[0]; // Maybe a loop through would be better
        if (rx6.IsMatch(txtBody))
            txtBody = rx6.Split(txtBody)[0]; // Maybe a loop through would be better
        if (rx7.IsMatch(txtBody))
            txtBody = rx7.Split(txtBody)[0]; // Maybe a loop through would be better
        if (rx8.IsMatch(txtBody))
            txtBody = rx8.Split(txtBody)[0]; // Maybe a loop through would be better
        if (rx9.IsMatch(txtBody))
            txtBody = rx9.Split(txtBody)[0]; // Maybe a loop through would be better
        if (rx10.IsMatch(txtBody))
            txtBody = rx10.Split(txtBody)[0]; // Maybe a loop through would be better
        if (rx11.IsMatch(txtBody))
            txtBody = rx11.Split(txtBody)[0]; // Maybe a loop through would be better        
        if (rx12.IsMatch(txtBody))
            txtBody = rx12.Split(txtBody)[0]; // Maybe a loop through would be better
        if (rx13.IsMatch(txtBody))
            txtBody = rx13.Split(txtBody)[0]; // Maybe a loop through would be better

        //replacing signature patterns
        if (Thanks.IsMatch(txtBody))
            txtBody = Thanks.Split(txtBody)[0]; // Maybe a loop through would be better
        if (ThanksNRegards.IsMatch(txtBody))
            txtBody = ThanksNRegards.Split(txtBody)[0]; // Maybe a loop through would be better
        if (Regards.IsMatch(txtBody))
            txtBody = Regards.Split(txtBody)[0]; // Maybe a loop through would be better        
        if (WithRegards.IsMatch(txtBody))
            txtBody = WithRegards.Split(txtBody)[0]; // Maybe a loop through would be better        
        if (WarmRegards.IsMatch(txtBody))
            txtBody = WarmRegards.Split(txtBody)[0]; // Maybe a loop through would be better        
        if (BestRegards.IsMatch(txtBody))
            txtBody = BestRegards.Split(txtBody)[0]; // Maybe a loop through would be better
        if (Yours.IsMatch(txtBody))
            txtBody = Yours.Split(txtBody)[0]; // Maybe a loop through would be better


        while (txtBody.Contains("\n\n")) txtBody = txtBody.Replace("\n\n", "\n");
        while (new Regex("\n ").IsMatch(txtBody)) txtBody = (new Regex("\n ")).Replace(txtBody, "\n");
        while (txtBody.Contains("  ")) txtBody = txtBody.Replace("  ", " ");

        if (txtBody.Contains("\r\n\r")) txtBody = txtBody.Replace("\r\n\r", "");

        if (txtBody.Contains("\r\n")) txtBody = txtBody.Replace("\r\n", "");

        return txtBody;
    }
}