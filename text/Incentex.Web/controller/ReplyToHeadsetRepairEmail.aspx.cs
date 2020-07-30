using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
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

public partial class controller_ReplyToHeadsetRepairEmail : System.Web.UI.Page
{
    #region Properties
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    HeadsetRepairCenterRepository objHeadsetRepairCenterRepo = new HeadsetRepairCenterRepository();
    UserInformationRepository objUserInformationRepository = new UserInformationRepository();
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetIMAPEmails();

            #region Lumisoft Code

            //List<IMAP_Envelope> lstNewReplies = new List<IMAP_Envelope>() { };

            //using (IMAP_Client client = new IMAP_Client())
            //{
            //    client.Connect(CommonMails.IMAPHost, CommonMails.IMAPPort, CommonMails.SSL);
            //    client.Login(CommonMails.ReplyToHeadserRepairCenter, CommonMails.ReplyToHeadserRepairCenterPassword);
            //    client.SelectFolder("INBOX");

            //    IMAP_SequenceSet sequence = new IMAP_SequenceSet();
            //    //sequence.Parse("*:1"); // from first to last

            //    IMAP_Client_FetchHandler fetchHandler = new IMAP_Client_FetchHandler();
            //    fetchHandler.NextMessage += new EventHandler(delegate(object s, EventArgs eNexMessage) { });
            //    fetchHandler.Envelope += new EventHandler<EventArgs<IMAP_Envelope>>(delegate(object s, EventArgs<IMAP_Envelope> eEnvelope)
            //    {
            //        lstNewReplies.Add(eEnvelope.Value);
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

            //        if (lstNewReplies.Count > 0)
            //        {
            //            List<Message> lstPOP = FetchAllMessages(CommonMails.POPHost, CommonMails.POPPort, CommonMails.SSL, CommonMails.ReplyToHeadserRepairCenter, CommonMails.ReplyToHeadserRepairCenterPassword);

            //            if (lstPOP.Count > 0)
            //            {
            //                foreach (IMAP_Envelope objReply in lstNewReplies)
            //                {
            //                    String ReplyTo = objReply.To[0].ToString();

            //                    if (ReplyTo.Contains("+hn") && ReplyTo.Contains("un") && ReplyTo.Contains("en") && ReplyTo.Contains("snf") && ReplyTo.Contains("rb"))
            //                    {
            //                        foreach (Message objPopMsg in lstPOP)
            //                        {
            //                            if (objPopMsg.Headers.MessageId == objReply.MessageID || objPopMsg.Headers.MessageId.Contains(objReply.MessageID) || objReply.MessageID.Contains(objPopMsg.Headers.MessageId))
            //                            {
            //                                try
            //                                {
            //                                    Int64 HeadsetRepairID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.LastIndexOf("+hn") + 3, ReplyTo.LastIndexOf("un") - ReplyTo.LastIndexOf("+hn") - 3));

            //                                    HeadsetRepairCenterRepository HeadsetRepos = new HeadsetRepairCenterRepository();
            //                                    HeadsetRepairCenterMaster objHeadsetRepair = HeadsetRepos.GetHeadsetRepairCenterById(HeadsetRepairID);


            //                                    //If the objHeadsetRepair Email is not deleted then only proceed, else skip to next email.
            //                                    if (objHeadsetRepair != null)
            //                                    {
            //                                        String sUserInfoID = ReplyTo.Substring(ReplyTo.LastIndexOf("un") + 2, ReplyTo.LastIndexOf("snf") - ReplyTo.LastIndexOf("un") - 2);

            //                                        Int64 CreatedUserInfoId = 0;
            //                                        Int64? UserInfoID = null;

            //                                        if (sUserInfoID != "annx")
            //                                        {
            //                                            UserInfoID = Convert.ToInt64(sUserInfoID);
            //                                        }

            //                                        String strNoteFor = ReplyTo.Substring(ReplyTo.LastIndexOf("snf") + 3, ReplyTo.LastIndexOf("rb") - ReplyTo.LastIndexOf("snf") - 3);
            //                                        String NoteType = (strNoteFor != "IEToVendor" && strNoteFor != "VendorToIE") ? "HeadsetRepairCenter" : "HeadsetRepairVendor";

            //                                        //When Replay from CE/CA
            //                                        String ReceivedUserId = ReplyTo.Substring(ReplyTo.LastIndexOf("rb") + 2, ReplyTo.LastIndexOf("en@") - ReplyTo.LastIndexOf("rb") - 2);


            //                                        System.Net.Mail.MailMessage objMsg = objPopMsg.ToMailMessage();

            //                                        String Reply = String.Empty;
            //                                        String TrailingNotesContent = String.Empty;
            //                                        Reply = cleanMsgBody(Convert.ToString(objMsg.Body));

            //                                        if (Reply.Length > 2500)
            //                                        {
            //                                            Reply = Reply.Substring(0, 2500);
            //                                        }

            //                                        #region !---------------Insert Reply Note Detail------------------!
            //                                        NotesHistoryRepository objNotesHistoryRepository = new NotesHistoryRepository();
            //                                        SupplierRepository objSupplierRepository = new SupplierRepository();


            //                                        NoteDetail objNoteDetail = new NoteDetail();
            //                                        objNoteDetail.Notecontents = Reply;
            //                                        objNoteDetail.NoteFor = NoteType;
            //                                        objNoteDetail.ForeignKey = HeadsetRepairID;
            //                                        objNoteDetail.SpecificNoteFor = strNoteFor;
            //                                        objNoteDetail.CreateDate = System.DateTime.Now;
            //                                        objNoteDetail.CreatedBy = (strNoteFor != "VendorToIE") ? UserInfoID : null;
            //                                        objNoteDetail.UpdateDate = System.DateTime.Now;
            //                                        objNoteDetail.UpdatedBy = (strNoteFor != "VendorToIE") ? UserInfoID : null;

            //                                        objNotesHistoryRepository.Insert(objNoteDetail);
            //                                        objNotesHistoryRepository.SubmitChanges();

            //                                        TrailingNotesContent = HeadsetRepos.TrailingNotes(HeadsetRepairID, NoteType, strNoteFor, "<br/>");

            //                                        #endregion


            //                                        if (strNoteFor == "IEActivity")
            //                                        {
            //                                            //Send email to IE when reply from CA/CE email 
            //                                            List<IEListResultsCustom> objList = objHeadsetRepairCenterRepo.GetHeadsetRepairCenterManageEmail(objNoteDetail.ForeignKey.Value);

            //                                            if (objList != null && objList.Count > 0)
            //                                            {
            //                                                for (Int32 i = 0; i < objList.Count; i++) //objList[i].Email
            //                                                    SendNotes(objNoteDetail, objHeadsetRepair.RepairNumber.Value.ToString(), objList[i].Email, "HeadsetRepairCenter", "CEActivity", Convert.ToInt64(objList[i].UserInfoID), UserInfoID.Value);
            //                                            }
            //                                            else
            //                                            {
            //                                                UserInformation objuser = objUserInformationRepository.GetById(Convert.ToInt64(ReceivedUserId));
            //                                                SendNotes(objNoteDetail, objHeadsetRepair.RepairNumber.Value.ToString(), objuser.Email, "HeadsetRepairCenter", "CEActivity", Convert.ToInt64(ReceivedUserId), UserInfoID.Value);
            //                                            }
            //                                        }
            //                                        else if (strNoteFor == "IEToVendor" || strNoteFor == "VendorToIE")
            //                                        {
            //                                            if (strNoteFor == "IEToVendor")
            //                                            {
            //                                                HeadsetRepairVendor objHeadsetvendorinformation = objHeadsetRepairCenterRepo.GetHeadsetRepairVendorByID(Convert.ToInt64(ReceivedUserId));
            //                                                SendNotes(objNoteDetail, objHeadsetRepair.RepairNumber.Value.ToString(), objHeadsetvendorinformation.VendorEmail, "HeadsetRepairVendor", "VendorToIE", Convert.ToInt64(ReceivedUserId), UserInfoID.Value);
            //                                            }
            //                                            else if (strNoteFor == "VendorToIE")
            //                                            {
            //                                                UserInformation objuser = objUserInformationRepository.GetById(Convert.ToInt64(ReceivedUserId));
            //                                                SendNotes(objNoteDetail, objHeadsetRepair.RepairNumber.Value.ToString(), objuser.Email, "HeadsetRepairVendor", "IEToVendor", Convert.ToInt64(ReceivedUserId), UserInfoID.Value);
            //                                            }
            //                                        }
            //                                        else //sent email to CA/CE when replay from IE
            //                                        {
            //                                            //Send email to Other Selected IE 
            //                                            List<IEListResultsCustom> objList = objHeadsetRepairCenterRepo.GetHeadsetRepairCenterManageEmail(objNoteDetail.ForeignKey.Value);
            //                                            if (objList != null && objList.Count > 0)
            //                                            {
            //                                                for (Int32 i = 0; i < objList.Count; i++) //objList[i].Email
            //                                                {
            //                                                    if (objList[i].UserInfoID != objNoteDetail.CreatedBy)
            //                                                    {
            //                                                        SendNotes(objNoteDetail, objHeadsetRepair.RepairNumber.Value.ToString(), objList[i].Email, "HeadsetRepairCenter", "CEActivity", Convert.ToInt64(objList[i].UserInfoID), UserInfoID.Value);
            //                                                    }
            //                                                }
            //                                            }

            //                                            //sent email to CA/CE
            //                                            UserInformation objuser = objUserInformationRepository.GetById(objHeadsetRepair.ContactID.Value);
            //                                            SendNotes(objNoteDetail, objHeadsetRepair.RepairNumber.Value.ToString(), objuser.Email, "HeadsetRepairCenter", "IEActivity", objHeadsetRepair.ContactID.Value, UserInfoID.Value);
            //                                        }
            //                                    }
            //                                }
            //                                catch (Exception ex)
            //                                {
            //                                    ErrHandler.WriteError(ex);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }

            //        //this line marks messages as read
            //        client.StoreMessageFlags(false, sequence, IMAP_Flags_SetType.Add, IMAP_MessageFlags.Seen);

            //        //this line marks messages as deleted
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
            if (!Convert.ToBoolean(Application["ReplyToHeadsetRepairLiveInvalidDate"]))
            {
                if (ex.Message.StartsWith("Invalid Date: Invalid time zone. Input: \""))
                {
                    Application["ReplyToHeadsetRepairLiveInvalidDate"] = true;
                    #region Email Notification
                    using (System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage())
                    {
                        objEmail.Body = "The Reply-to-email has got some invalid email. <br /><br />Get rid of this email to get the function working again.";
                        objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                        objEmail.IsBodyHtml = true;
                        objEmail.Subject = "Reply-to-email function (live environment) not working.";
                        objEmail.To.Add(new MailAddress("gaurang.pathak@indianic.com"));

                        SmtpClient objSmtp = new SmtpClient();

                        objSmtp.EnableSsl = Common.SSL;
                        objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
                        objSmtp.Host = Common.SMTPHost;
                        objSmtp.Port = Common.SMTPPort;

                        objSmtp.Send(objEmail);
                    }
                    #endregion
                }
            }
            else
            {
                ErrHandler.WriteError(ex);
            }
        }
    }

    #endregion

    #region Page Methods

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
            for (Int32 i = 1; i <= messageCount; i++)
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
        Response.Write("Creating Imap object...</br>");
        using (Imap imap = new Imap())
        {
            Response.Write("Connecting imap host...</br>");
            if (!Common.SSL)
                imap.Connect(CommonMails.IMAPHost);//Without SSL
            else
                imap.Connect(CommonMails.IMAPHost, CommonMails.IMAPPort);//With SSL (i.e. SSL = true)

            Response.Write("Connected Sucessfully...</br>");
            Response.Write("Logging in imap host...</br>");
            imap.Login(CommonMails.ReplyToHeadserRepairCenter, CommonMails.ReplyToHeadserRepairCenterPassword);                       // You can also use: LoginPLAIN, LoginCRAM, LoginDIGEST, LoginOAUTH methods,            
            // or use UseBestLogin method if you want Mail.dll to choose for you.
            Response.Write("Looged in Sucessfully...</br>");

            Response.Write("Selecting Inbox...</br>");
            imap.SelectFolder("Inbox");                                 // You can select other folders, e.g. Sent folder: imap.Select("Sent");
            Response.Write("Selected Inbox Successfully...</br>");

            Response.Write("Searching for unseen emails...</br>");
            UidCollection uids = (UidCollection)imap.Search(true, "unseen", null);     // Find all unseen messages.
            Response.Write("Searching successful...</br>");

            Response.Write("Number of unseen messages is: " + uids.Count + "</br>");

            if (uids.Count > 0)
            {
                MailMessageCollection msgs = imap.DownloadEntireMessages(uids.ToString(), true);
                Response.Write("Looping through new emails...</br>");
                foreach (MailBee.Mime.MailMessage msg in msgs)
                {
                    Response.Write("Processing Email...</br></br>");
                    ProcessMessage(msg);                          // Display email data, save attachments.
                }

                Response.Write("Deleting read emails...</br></br>");
                imap.DeleteMessages(uids.ToString(), true);
                Response.Write("Deleted read emails Successfully...</br></br>");
            }            

            Response.Write("Closing imap...</br>");
            imap.Close();
            Response.Write("Closed Successfully...</br>");
            Response.Write("Disconnecting imap...</br>");
            imap.Disconnect();
            Response.Write("Disconnected Successfully...</br>");
        }
    }

    private void ProcessMessage(MailBee.Mime.MailMessage email)
    {
        Response.Write("Subject: " + email.Subject + "</br>");
        Response.Write("From: " + email.From + "</br>");
        Response.Write("To: " + email.To + "</br>");
        Response.Write("Cc: " + email.Cc + "</br>");
        Response.Write("Bcc: " + email.ReplyTo + "</br>");

        Response.Write("Text: " + email.BodyPlainText + "</br>");
        Response.Write("HTML: " + email.BodyHtmlText + "</br>");

        Response.Write("Attachments: " + "</br>");
        foreach (MailBee.Mime.Attachment attachment in email.Attachments)
        {
            Response.Write(attachment.Filename + "</br>");
            //attachment.Save(@"c:\" + attachment.Filename, true);
        }

        try
        {
            String ReplyTo = email.To[0].ToString();

            if (ReplyTo.Contains("+hn") && ReplyTo.Contains("un") && ReplyTo.Contains("en") && ReplyTo.Contains("snf") && ReplyTo.Contains("rb"))
            {
                Int64 HeadsetRepairID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.LastIndexOf("+hn") + 3, ReplyTo.LastIndexOf("un") - ReplyTo.LastIndexOf("+hn") - 3));

                HeadsetRepairCenterRepository HeadsetRepos = new HeadsetRepairCenterRepository();
                HeadsetRepairCenterMaster objHeadsetRepair = HeadsetRepos.GetHeadsetRepairCenterById(HeadsetRepairID);

                //If the objHeadsetRepair Email is not deleted then only proceed, else skip to next email.
                if (objHeadsetRepair != null)
                {
                    String sUserInfoID = ReplyTo.Substring(ReplyTo.LastIndexOf("un") + 2, ReplyTo.LastIndexOf("snf") - ReplyTo.LastIndexOf("un") - 2);
                    
                    Int64? UserInfoID = null;

                    if (sUserInfoID != "annx")
                    {
                        UserInfoID = Convert.ToInt64(sUserInfoID);
                    }

                    String strNoteFor = ReplyTo.Substring(ReplyTo.LastIndexOf("snf") + 3, ReplyTo.LastIndexOf("rb") - ReplyTo.LastIndexOf("snf") - 3);
                    String NoteType = (strNoteFor != "IEToVendor" && strNoteFor != "VendorToIE") ? "HeadsetRepairCenter" : "HeadsetRepairVendor";

                    //When Replay from CE/CA
                    String ReceivedUserId = ReplyTo.Substring(ReplyTo.LastIndexOf("rb") + 2, ReplyTo.LastIndexOf("en@") - ReplyTo.LastIndexOf("rb") - 2);

                    String Reply = String.Empty;
                    String TrailingNotesContent = String.Empty;
                    Reply = cleanMsgBody(Convert.ToString(email.BodyPlainText));

                    if (Reply.Length > 2500)
                    {
                        Reply = Reply.Substring(0, 2500);
                    }

                    #region !---------------Insert Reply Note Detail------------------!
                    NotesHistoryRepository objNotesHistoryRepository = new NotesHistoryRepository();
                    SupplierRepository objSupplierRepository = new SupplierRepository();


                    NoteDetail objNoteDetail = new NoteDetail();
                    objNoteDetail.Notecontents = Reply;
                    objNoteDetail.NoteFor = NoteType;
                    objNoteDetail.ForeignKey = HeadsetRepairID;
                    objNoteDetail.SpecificNoteFor = strNoteFor;
                    objNoteDetail.CreateDate = System.DateTime.Now;
                    objNoteDetail.CreatedBy = (strNoteFor != "VendorToIE") ? UserInfoID : null;
                    objNoteDetail.UpdateDate = System.DateTime.Now;
                    objNoteDetail.UpdatedBy = (strNoteFor != "VendorToIE") ? UserInfoID : null;

                    objNotesHistoryRepository.Insert(objNoteDetail);
                    objNotesHistoryRepository.SubmitChanges();

                    TrailingNotesContent = HeadsetRepos.TrailingNotes(HeadsetRepairID, NoteType, strNoteFor, "<br/>");

                    #endregion

                    if (strNoteFor == "IEActivity")
                    {
                        //Send email to IE when reply from CA/CE email 
                        List<IEListResultsCustom> objList = objHeadsetRepairCenterRepo.GetHeadsetRepairCenterManageEmail(objNoteDetail.ForeignKey.Value);

                        if (objList != null && objList.Count > 0)
                        {
                            for (Int32 i = 0; i < objList.Count; i++) //objList[i].Email
                                SendNotes(objNoteDetail, objHeadsetRepair.RepairNumber.Value.ToString(), objList[i].Email, "HeadsetRepairCenter", "CEActivity", Convert.ToInt64(objList[i].UserInfoID), UserInfoID.Value);
                        }
                        else
                        {
                            UserInformation objuser = objUserInformationRepository.GetById(Convert.ToInt64(ReceivedUserId));
                            SendNotes(objNoteDetail, objHeadsetRepair.RepairNumber.Value.ToString(), objuser.Email, "HeadsetRepairCenter", "CEActivity", Convert.ToInt64(ReceivedUserId), UserInfoID.Value);
                        }
                    }
                    else if (strNoteFor == "IEToVendor" || strNoteFor == "VendorToIE")
                    {
                        if (strNoteFor == "IEToVendor")
                        {
                            HeadsetRepairVendor objHeadsetvendorinformation = objHeadsetRepairCenterRepo.GetHeadsetRepairVendorByID(Convert.ToInt64(ReceivedUserId));
                            SendNotes(objNoteDetail, objHeadsetRepair.RepairNumber.Value.ToString(), objHeadsetvendorinformation.VendorEmail, "HeadsetRepairVendor", "VendorToIE", Convert.ToInt64(ReceivedUserId), UserInfoID.Value);
                        }
                        else if (strNoteFor == "VendorToIE")
                        {
                            UserInformation objuser = objUserInformationRepository.GetById(Convert.ToInt64(ReceivedUserId));
                            SendNotes(objNoteDetail, objHeadsetRepair.RepairNumber.Value.ToString(), objuser.Email, "HeadsetRepairVendor", "IEToVendor", Convert.ToInt64(ReceivedUserId), UserInfoID.Value);
                        }
                    }
                    else //sent email to CA/CE when replay from IE
                    {
                        //Send email to Other Selected IE 
                        List<IEListResultsCustom> objList = objHeadsetRepairCenterRepo.GetHeadsetRepairCenterManageEmail(objNoteDetail.ForeignKey.Value);
                        if (objList != null && objList.Count > 0)
                        {
                            for (Int32 i = 0; i < objList.Count; i++) //objList[i].Email
                            {
                                if (objList[i].UserInfoID != objNoteDetail.CreatedBy)
                                {
                                    SendNotes(objNoteDetail, objHeadsetRepair.RepairNumber.Value.ToString(), objList[i].Email, "HeadsetRepairCenter", "CEActivity", Convert.ToInt64(objList[i].UserInfoID), UserInfoID.Value);
                                }
                            }
                        }

                        //sent email to CA/CE
                        UserInformation objuser = objUserInformationRepository.GetById(objHeadsetRepair.ContactID.Value);
                        SendNotes(objNoteDetail, objHeadsetRepair.RepairNumber.Value.ToString(), objuser.Email, "HeadsetRepairCenter", "IEActivity", objHeadsetRepair.ContactID.Value, UserInfoID.Value);
                    }
                }
            }
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

    private void SendNotes(NoteDetail objNotedetails, String Headsetrepairnumber, String sentTo, String Notefore, String specificNoteFor, Int64 UserID, Int64 ReceivedUserID)
    {
        try
        {
            UserInformation objUserInformation = new UserInformation();
            HeadsetRepairVendor objHeadsetvendorinformation = new HeadsetRepairVendor();

            if (specificNoteFor == "IEToVendor")
                objHeadsetvendorinformation = objHeadsetRepairCenterRepo.GetHeadsetRepairVendorByID(ReceivedUserID);
            else
                objUserInformation = objUserInformationRepository.GetById(objNotedetails.CreatedBy);



            String sFrmadd = "support@world-link.us.com";
            String sToadd = sentTo;
            String sSubject = "Incentex Message - Headset Repair Number HR" + Headsetrepairnumber;
            String sFrmname = objUserInformation.UserInfoID > 0 ? (objUserInformation.FirstName + " " + objUserInformation.LastName) : objHeadsetvendorinformation.VendorContact;


            String smtphost = Application["SMTPHOST"].ToString();
            Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            String smtpUserID = Application["SMTPUSERID"].ToString();
            String smtppassword = Application["SMTPPASSWORD"].ToString();

            String eMailTemplate = String.Empty;
            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/DocumentStorageCenter.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();


            String body = null;
            body = objNotedetails.Notecontents + "<br/><br/>";

            StringBuilder MessageBody = new StringBuilder(eMailTemplate);
            MessageBody.Replace("{MessageBody}", body);
            MessageBody.Replace("{Sender}", "Best Regards,<br/>" + sFrmname);

            //if (new ManageEmailRepository().CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderNotes)) == true)
            //{
            String sReplyToadd = CommonMails.ReplyToHeadserRepairCenter;//"ordernotes@world-link.us.com";
            String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+hn" + objNotedetails.ForeignKey + "un" + UserID + "snf" + specificNoteFor + "rb" + ReceivedUserID + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
            new CommonMails().SendMailWithReplyTo((objUserInformation.UserInfoID > 0 ? objUserInformation.UserInfoID : objHeadsetvendorinformation.VendorID), Notefore, sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true, objNotedetails.ForeignKey);
            //}
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion
}