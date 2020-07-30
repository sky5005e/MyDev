using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using LumiSoft.Net;
using LumiSoft.Net.IMAP;
using LumiSoft.Net.IMAP.Client;
using LumiSoft.Net.Mail;
using MailBee.ImapMail;
using MailBee.Mime;
using OpenPop.Mime;
using OpenPop.Pop3;

public partial class controller_ServiceTicketNotesReplyFromEmail : System.Web.UI.Page
{
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write("Page Loading...</br>");
        try
        {
            //Response.Write("Getting IMAP emails...</br>");
            GetIMAPEmails();

            #region Lumisoft Code

            //List<IMAP_Envelope> lstNewReplies = new List<IMAP_Envelope>() { };

            //using (IMAP_Client client = new IMAP_Client())
            //{
            //    ////For reading emails from test account
            //    client.Connect("imap.gmail.com", 993, true);
            //    client.Login("smtp.incentex@gmail.com", "smtp@incentex");
            //    client.SelectFolder("INBOX");

            //    #region Credentials Through Hidden Fields
            //    //client.Connect(Convert.ToString(host.Value), Convert.ToInt32(port.Value), false);
            //    //client.Login(Convert.ToString(username.Value), Convert.ToString(password.Value));
            //    //client.SelectFolder(Convert.ToString(folder.Value));
            //    #endregion

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
            //            ////For reading emails from test account
            //            List<Message> lstPOP = FetchAllMessages("pop.gmail.com", 995, true, "smtp.incentex@gmail.com", "smtp@incentex");

            //            if (lstPOP.Count > 0)
            //            {
            //                foreach (IMAP_Envelope objReply in lstNewReplies)
            //                {
            //                    String ReplyTo = objReply.To[0].ToString();

            //                    if (ReplyTo.Contains("+tn") && ReplyTo.Contains("un") && ReplyTo.Contains("en") && ReplyTo.Contains("nt"))
            //                    {
            //                        foreach (Message objPopMsg in lstPOP)
            //                        {
            //                            if (objPopMsg.Headers.MessageId == objReply.MessageID || objPopMsg.Headers.MessageId.Contains(objReply.MessageID) || objReply.MessageID.Contains(objPopMsg.Headers.MessageId))
            //                            {
            //                                try
            //                                {
            //                                    Int64 TicketNo = Convert.ToInt64(ReplyTo.Substring(ReplyTo.LastIndexOf("+tn") + 3, ReplyTo.LastIndexOf("un") - ReplyTo.LastIndexOf("+tn") - 3));
            //                                    ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();

            //                                    vw_ServiceTicket objServiceTicket = objSerTicRep.GetFirstByID(TicketNo);

            //                                    //If the ticket is not deleted then only proceed, else skip to next email.
            //                                    if (objServiceTicket != null)
            //                                    {
            //                                        String sUserInfoID = ReplyTo.Substring(ReplyTo.LastIndexOf("un") + 2, ReplyTo.LastIndexOf("nt") - ReplyTo.LastIndexOf("un") - 2);

            //                                        Int64? UserInfoID = null;
            //                                        if (sUserInfoID != "annx")
            //                                        {
            //                                            UserInfoID = Convert.ToInt64(sUserInfoID);
            //                                        }

            //                                        Int32 NoteType = Convert.ToInt32(ReplyTo.Substring(ReplyTo.LastIndexOf("nt") + 2, ReplyTo.LastIndexOf("en@") - ReplyTo.LastIndexOf("nt") - 2));

            //                                        String strNoteFor = String.Empty;
            //                                        if (NoteType == 1)
            //                                            strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs);
            //                                        else if (NoteType == 2)
            //                                            strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);
            //                                        else if (NoteType == 3)
            //                                            strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketSupps);

            //                                        System.Net.Mail.MailMessage objMsg = objPopMsg.ToMailMessage();

            //                                        String Reply = String.Empty;
            //                                        Reply = cleanMsgBody(Convert.ToString(objMsg.Body));

            //                                        if (sUserInfoID == "annx")
            //                                        {
            //                                            String AnonymousEmail = Convert.ToString(objMsg.From);
            //                                            if (Reply.Length + AnonymousEmail.Length + 3 > 2500)
            //                                            {
            //                                                Reply = Reply.Substring(0, 2500 - AnonymousEmail.Length + 3) + " (" + AnonymousEmail + ")";
            //                                            }
            //                                            else
            //                                            {
            //                                                Reply = Reply + " (" + AnonymousEmail + ")";
            //                                            }
            //                                        }
            //                                        else if (Reply.Length > 2500)
            //                                        {
            //                                            Reply = Reply.Substring(0, 2500);
            //                                        }

            //                                        objSerTicRep.InsertTicketNote(TicketNo, UserInfoID, Reply, strNoteFor, NoteType == 2 ? "IEInternalNotes" : null);

            //                                        Reply = objSerTicRep.TrailingNotes(TicketNo, NoteType, false, "<br/>");
            //                                        SendEMailRepliesForNotes(TicketNo, Reply, NoteType, Convert.ToString(objMsg.Subject));
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
            //        if (Convert.ToBoolean(cleanmsg.Value) == true)
            //            client.StoreMessageFlags(false, sequence, IMAP_Flags_SetType.Add, IMAP_MessageFlags.Seen);

            //        //this line marks messages as deleted
            //        if (Convert.ToBoolean(deletemsg.Value) == true)
            //            client.StoreMessageFlags(false, sequence, IMAP_Flags_SetType.Add, IMAP_MessageFlags.Deleted);
            //    }

            //    //client.CloseFolder();
            //    client.Expunge();
            //    client.Disconnect();
            //    client.Dispose();
            //}

            #endregion

            #region RnD Stuff
            //------------------------------------------------------------------------------
            //------------------------------------------------------------------------------

            //using (Imap imap = new Imap())
            //{
            //    imap.ConnectSSL("imap.gmail.com", 993);       // or ConnectSSL for SSL
            //    imap.UseBestLogin("smtp.incentex@gmail.com", "smtp@incentex");

            //    imap.SelectInbox();
            //    List<long> uidList = imap.SearchFlag(Flag.Unseen);
            //    List<String> lstSubject = new List<String>();
            //    List<String> lstText = new List<String>();
            //    foreach (long uid in uidList)
            //    {
            //        IMail email = new MailBuilder()
            //            .CreateFromEml(imap.GetMessageByUID(uid));

            //        lstSubject.Add(email.Subject);
            //        lstText.Add(email.Text);                    
            //    }
            //    imap.Close();
            //}

            //------------------------------------------------------------------------------
            //------------------------------------------------------------------------------

            //// Connect to the IMAP server. The 'true' parameter specifies to use SSL
            //// which is important (for Gmail at least)
            //ImapClient ic = new ImapClient("imap.gmail.com", "name@gmail.com", "pass",
            //                ImapClient.AuthMethods.Login, 993, true);
            //// Select a mailbox. Case-insensitive
            //ic.SelectMailbox("INBOX");
            //Console.WriteLine(ic.GetMessageCount());
            //// Get the first *11* messages. 0 is the first message;
            //// and it also includes the 10th message, which is really the eleventh ;)
            //// MailMessage represents, well, a message in your mailbox
            //MailMessage[] mm = ic.GetMessages(0, 10);
            //foreach (MailMessage m in mm)
            //{
            //    Console.WriteLine(m.Subject);
            //}
            //// Probably wiser to use a using statement
            //ic.Dispose();

            //------------------------------------------------------------------------------
            //------------------------------------------------------------------------------

            //using (var client = new IMAP_Client())
            //{
            //    client.Connect("", 995, true);
            //    client.Login("", "");
            //    client.SelectFolder("INBOX");
            //    var sequence = new IMAP_SequenceSet();
            //    sequence.Parse("0:10");

            //    IMAP_Client_FetchHandler fetchHandler = new IMAP_Client_FetchHandler();

            //    fetchHandler.NextMessage += new EventHandler(delegate(object s, EventArgs e) { });

            //    fetchHandler.Envelope += new EventHandler<EventArgs<IMAP_Envelope>>(delegate(object s, EventArgs<IMAP_Envelope> e)
            //    {
            //        IMAP_Envelope envelope = e.Value;
            //    });

            //    var fetchItems = client.Fetch(false, sequence, new IMAP_Fetch_DataItem[] { new IMAP_Fetch_DataItem_Envelope() }, fetchHandler);
            //    foreach (var fetchItem in fetchItems)
            //    {
            //        Console.Out.WriteLine("message.UID = {0}", fetchItem.UID);
            //        Console.Out.WriteLine("message.Envelope.From = {0}", fetchItem.Envelope.From);
            //        Console.Out.WriteLine("message.Envelope.To = {0}", fetchItem.Envelope.To);
            //        Console.Out.WriteLine("message.Envelope.Subject = {0}", fetchItem.Envelope.Subject);
            //        Console.Out.WriteLine("message.Envelope.MessageID = {0}", fetchItem.Envelope.MessageID);
            //    }
            //    Console.Out.WriteLine("Fetching bodies");
            //    foreach (var fetchItem in client.Fetch(, sequence, IMAP_FetchItem_Flags.All, false, true)
            //    {             
            //        var email = LumiSoft.Net.Mail.Mail_Message.ParseFromByte(fetchItem.MessageData);             
            //        Console.Out.WriteLine("email.BodyText = {0}", email.BodyText);
            //    }
            //}

            //------------------------------------------------------------------------------
            //------------------------------------------------------------------------------

            #endregion
        }
        catch (Exception ex)
        {
            if (!Convert.ToBoolean(Application["ReplyToLocalInvalidDate"]))
            {
                if (ex.Message.StartsWith("Invalid Date: Invalid time zone. Input: \""))
                {
                    Application["ReplyToLocalInvalidDate"] = true;
                    #region Email Notification
                    using (System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage())
                    {
                        objEmail.Body = "The Reply-to-email has got some invalid email. <br /><br />Get rid of this email to get the function working again.";
                        objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                        objEmail.IsBodyHtml = true;
                        objEmail.Subject = "Reply-to-email function (local environment) not working.";
                        objEmail.To.Add(new MailAddress("devraj.gadhavi@indianic.com"));

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

            imap.Login(Common.ReplyTo, Common.ReplyToPassword);// You can also use: LoginPLAIN, LoginCRAM, LoginDIGEST, LoginOAUTH methods,
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

            String ReplyTo = email.To[0].ToString();

            if (ReplyTo.Contains("+tn") && ReplyTo.Contains("un") && ReplyTo.Contains("en") && ReplyTo.Contains("nt"))
            {
                Int64 TicketNo = Convert.ToInt64(ReplyTo.Substring(ReplyTo.LastIndexOf("+tn") + 3, ReplyTo.LastIndexOf("un") - ReplyTo.LastIndexOf("+tn") - 3));
                ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();

                vw_ServiceTicket objServiceTicket = objSerTicRep.GetFirstByID(TicketNo);

                //If the ticket is not deleted then only proceed, else skip to next email.
                if (objServiceTicket != null)
                {
                    String sUserInfoID = ReplyTo.Substring(ReplyTo.LastIndexOf("un") + 2, ReplyTo.LastIndexOf("nt") - ReplyTo.LastIndexOf("un") - 2);

                    Int64? UserInfoID = null;
                    if (sUserInfoID != "annx")
                    {
                        UserInfoID = Convert.ToInt64(sUserInfoID);
                    }

                    Int32 NoteType = Convert.ToInt32(ReplyTo.Substring(ReplyTo.LastIndexOf("nt") + 2, ReplyTo.LastIndexOf("en@") - ReplyTo.LastIndexOf("nt") - 2));

                    String strNoteFor = String.Empty;
                    if (NoteType == 1)
                        strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs);
                    else if (NoteType == 2)
                        strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);
                    else if (NoteType == 3)
                        strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketSupps);

                    String Reply = String.Empty;
                    Reply = cleanMsgBody(Convert.ToString(email.BodyPlainText));

                    if (sUserInfoID == "annx")
                    {
                        String AnonymousEmail = Convert.ToString(email.From);
                        if (Reply.Length + AnonymousEmail.Length + 3 > 2500)
                        {
                            Reply = Reply.Substring(0, 2500 - AnonymousEmail.Length + 3) + " (" + AnonymousEmail + ")";
                        }
                        else
                        {
                            Reply = Reply + " (" + AnonymousEmail + ")";
                        }
                    }
                    else if (Reply.Length > 2500)
                    {
                        Reply = Reply.Substring(0, 2500);
                    }

                    objSerTicRep.InsertTicketNote(TicketNo, UserInfoID, Reply, strNoteFor, NoteType == 2 ? "IEInternalNotes" : null);

                    Reply = objSerTicRep.TrailingNotes(TicketNo, NoteType, false, "<br/>");

                    UserInformation objSender = new UserInformationRepository().GetById(UserInfoID);
                    String sender = String.Empty;

                    if (objSender != null)
                        sender = objSender.FirstName + " " + objSender.LastName;

                    SendEMailRepliesForNotes(TicketNo, Reply, NoteType, Convert.ToString(email.Subject), sender);
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

    #region method m_pImap_Fetch_Message_UntaggedResponse

    /// <summary>
    /// This method is called when FETCH RFC822 untagged response is received.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event data.</param>
    private void m_pImap_Fetch_Message_UntaggedResponse(object sender, EventArgs<IMAP_r_u> e)
    {
        /* NOTE: All IMAP untagged responses may be raised from thread pool thread,
            so all UI operations must use Invoke.
             
           There may be other untagged responses than FETCH, because IMAP server
           may send any untagged response to any command.
        */

        try
        {
            if (e.Value is IMAP_r_u_Fetch)
            {
                IMAP_r_u_Fetch fetchResp = (IMAP_r_u_Fetch)e.Value;

                try
                {
                    fetchResp.Rfc822.Stream.Position = 0;
                    Mail_Message mime = Mail_Message.ParseFromStream(fetchResp.Rfc822.Stream);
                    if (mime != null)
                    {
                        //lstMessages.Add(mime);
                    }
                    fetchResp.Rfc822.Stream.Dispose();

                    //m_pTabPageMail_MessagesToolbar.Items["save"].Enabled = true;
                    //m_pTabPageMail_MessagesToolbar.Items["delete"].Enabled = true;

                    //m_pTabPageMail_MessageAttachments.Tag = mime;
                    //foreach (MIME_Entity entity in mime.Attachments)
                    //{
                    //    ListViewItem item = new ListViewItem(ListViewItemType.DataItem);
                    //    if (entity.ContentDisposition != null && entity.ContentDisposition.Param_FileName != null)
                    //    {
                    //        item.Text = entity.ContentDisposition.Param_FileName;
                    //    }
                    //    else
                    //    {
                    //        item.Text = "untitled";
                    //    }
                    //    item.ImageIndex = 0;
                    //    item.Tag = entity;
                    //    m_pTabPageMail_MessageAttachments.Items.Add(item);
                    //}

                    //if (mime.BodyText != null)
                    //{
                    //    m_pTabPageMail_MessageText.Text = mime.BodyText;
                    //    mime.
                    //}
                }
                catch (Exception x)
                {
                    ErrHandler.WriteError(x);
                }
            }
        }
        catch (Exception x)
        {
            ErrHandler.WriteError(x);
        }
    }

    #endregion

    protected void Button1_Click(object sender, EventArgs e)
    {

    }

    private void SendEMailRepliesForNotes(Int64 ticketID, String reply, Int32 noteType, String subject, String sender)
    {
        try
        {
            String eMailTemplate = String.Empty;
            String sReplyToadd = Common.ReplyTo;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/ServiceTicketNote.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            List<vw_ServiceTicketNoteRecipient> lstRecipients = new List<vw_ServiceTicketNoteRecipient>();

            if (noteType == 2)
            {
                lstRecipients = objSerTicRep.GetNoteRecipientsByTicketID(ticketID).Where(le => (le.Usertype == 1 || le.Usertype == 2) && le.SubscriptionFlag == true).ToList();
            }
            else
            {
                lstRecipients = objSerTicRep.GetNoteRecipientsByTicketID(ticketID).Where(le => le.SubscriptionFlag == true).ToList();
            }

            foreach (vw_ServiceTicketNoteRecipient recipient in lstRecipients)
            {
                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                {
                    String UserName = Convert.ToString(recipient.LastName) + " " + Convert.ToString(recipient.FirstName);

                    StringBuilder MessageBody = new StringBuilder(eMailTemplate);
                    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                    MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
                    MessageBody.Replace("{TicketNo}", Convert.ToString(ticketID));
                    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                    MessageBody.Replace("{Sender}", sender);
                    MessageBody.Replace("{Note}", reply);

                    if (recipient.Usertype == 1 || recipient.Usertype == 2 || recipient.Usertype == 5 || recipient.Usertype == 6)
                    {
                        String CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
                                    <a href='" + ConfigurationSettings.AppSettings["siteurl"]
                                        + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
                                        + ticketID + "&uid=" + recipient.UserInfoID
                                        + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";

                        MessageBody.Replace("{CloseTicket}", CloseTicket);
                    }
                    else
                    {
                        MessageBody.Replace("{CloseTicket}", String.Empty);
                    }

                    String sUserInfoID = recipient.UserInfoID == null ? "annx" : Convert.ToString(recipient.UserInfoID);

                    String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + ticketID + "un" + sUserInfoID + "nt" + noteType + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                    new CommonMails().SendMailWithReplyTo(Convert.ToInt64(sUserInfoID), "Support Ticket Center", Common.EmailFrom, Convert.ToString(recipient.Email), subject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}