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
using OpenPop.Mime;
using OpenPop.Pop3;
using Incentex.BE;
using Incentex.DA;
using System.Data;

public partial class controller_OrderNotesReplyFromEmail : System.Web.UI.Page
{
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            List<IMAP_Envelope> lstNewReplies = new List<IMAP_Envelope>() { };

            using (IMAP_Client client = new IMAP_Client())
            {
                ////For reading emails from test account
                client.Connect("imap.gmail.com", 993, true);
                client.Login("smtp.incentex@gmail.com", "smtp@incentex");//ordernotes@world-link.us.com,"Password"
                client.SelectFolder("INBOX");

                IMAP_SequenceSet sequence = new IMAP_SequenceSet();
                //sequence.Parse("*:1"); // from first to last

                IMAP_Client_FetchHandler fetchHandler = new IMAP_Client_FetchHandler();
                fetchHandler.NextMessage += new EventHandler(delegate(object s, EventArgs eNexMessage) { });
                fetchHandler.Envelope += new EventHandler<EventArgs<IMAP_Envelope>>(delegate(object s, EventArgs<IMAP_Envelope> eEnvelope)
                {
                    lstNewReplies.Add(eEnvelope.Value);
                });

                // the best way to find unread emails is to perform server search
                Int32[] unseen_ids = client.Search(false, null, "unseen");

                if (unseen_ids.Length > 0)
                {
                    System.Text.StringBuilder sbUnseenIDs = new System.Text.StringBuilder();
                    foreach (Int32 unseen_id in unseen_ids)
                    {
                        if (sbUnseenIDs.Length <= 0)
                        {
                            sbUnseenIDs.Append(Convert.ToString(unseen_id));
                        }
                        else
                        {
                            sbUnseenIDs.Append("," + Convert.ToString(unseen_id));
                        }
                    }

                    // now we need to initiate our sequence of messages to be fetched                    
                    sequence.Parse(sbUnseenIDs.ToString());

                    // fetch messages now                    
                    IMAP_Fetch_DataItem[] lstDataItem = new IMAP_Fetch_DataItem[] { new IMAP_Fetch_DataItem_Envelope() };
                    client.Fetch(false, sequence, lstDataItem, fetchHandler);

                    if (lstNewReplies.Count > 0)
                    {
                        ////For reading emails from test account
                        List<Message> lstPOP = FetchAllMessages("pop.gmail.com", 995, true, "smtp.incentex@gmail.com", "smtp@incentex");

                        if (lstPOP.Count > 0)
                        {
                            foreach (IMAP_Envelope objReply in lstNewReplies)
                            {
                                String ReplyTo = objReply.To[0].ToString();

                                if (ReplyTo.Contains("+on") && ReplyTo.Contains("un") && ReplyTo.Contains("en") && ReplyTo.Contains("nt"))
                                {
                                    foreach (Message objPopMsg in lstPOP)
                                    {
                                        if (objPopMsg.Headers.MessageId == objReply.MessageID || objPopMsg.Headers.MessageId.Contains(objReply.MessageID) || objReply.MessageID.Contains(objPopMsg.Headers.MessageId))
                                        {
                                            try
                                            {
                                                Int64 OrderID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.LastIndexOf("+on") + 3, ReplyTo.LastIndexOf("un") - ReplyTo.LastIndexOf("+tn") - 3));
                                                OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();

                                                Order objOrder = objOrderConfirmationRepository.GetByOrderID(OrderID);

                                                //If the ticket is not deleted then only proceed, else skip to next email.
                                                if (objOrder != null)
                                                {
                                                    Int64 UserInfoID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.LastIndexOf("un") + 2, ReplyTo.LastIndexOf("nt") - ReplyTo.LastIndexOf("un") - 2));

                                                    Int32 NoteType = Convert.ToInt32(ReplyTo.Substring(ReplyTo.LastIndexOf("nt") + 2, ReplyTo.LastIndexOf("en@") - ReplyTo.LastIndexOf("nt") - 2));

                                                    MailMessage objMsg = objPopMsg.ToMailMessage();

                                                    String Reply = String.Empty;
                                                    Reply = cleanMsgBody(Convert.ToString(objMsg.Body));

                                                    if (Reply.Length > 2500)
                                                        Reply = Reply.Substring(0, 2500);


                                                    NotesHistoryRepository objNotesHistoryRepository = new NotesHistoryRepository();
                                                    String strNoteFor = String.Empty;
                                                    if (NoteType == 1)
                                                    {
                                                        strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.SupplierOrder);

                                                        NoteDetail objNoteDetail = new NoteDetail();
                                                        objNoteDetail.Notecontents = Reply;
                                                        objNoteDetail.NoteFor = strNoteFor;
                                                        objNoteDetail.ForeignKey = new SupplierRepository().GetByUserInfoId(UserInfoID).SupplierID;
                                                        objNoteDetail.SpecificNoteFor = OrderID.ToString();
                                                        objNoteDetail.CreateDate = System.DateTime.Now;
                                                        objNoteDetail.CreatedBy = UserInfoID;
                                                        objNoteDetail.UpdateDate = System.DateTime.Now;
                                                        objNoteDetail.UpdatedBy = UserInfoID;
                                                        objNotesHistoryRepository.Insert(objNoteDetail);
                                                        objNotesHistoryRepository.SubmitChanges();
                                                    }
                                                    else if (NoteType == 2)
                                                    {
                                                        strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.OrderDetailsIEs);

                                                        NoteDetail objNoteDetail = new NoteDetail();
                                                        objNoteDetail.Notecontents = Reply;
                                                        objNoteDetail.NoteFor = strNoteFor;
                                                        objNoteDetail.ForeignKey = OrderID;
                                                        objNoteDetail.SpecificNoteFor = "IEInternalNotes";
                                                        objNoteDetail.CreateDate = System.DateTime.Now;
                                                        objNoteDetail.CreatedBy = UserInfoID;
                                                        objNoteDetail.UpdateDate = System.DateTime.Now;
                                                        objNoteDetail.UpdatedBy = UserInfoID;
                                                        objNotesHistoryRepository.Insert(objNoteDetail);
                                                        objNotesHistoryRepository.SubmitChanges();

                                                        //Send email to all the IE when reply from email as per Ken instruction
                                                        List<UserInformation> objAdminList = new UserInformationRepository().GetEmailInformation();
                                                        for (int i = 0; i < objAdminList.Count; i++)
                                                            SendNotes(OrderID, objAdminList[i].UserInfoID, NoteType, Reply);
                                                    }

                                                   
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ErrHandler.WriteError(ex);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //this line marks messages as read
                    if (Convert.ToBoolean(cleanmsg.Value) == true)
                        client.StoreMessageFlags(false, sequence, IMAP_Flags_SetType.Add, IMAP_MessageFlags.Seen);

                    //this line marks messages as deleted
                    if (Convert.ToBoolean(deletemsg.Value) == true)
                        client.StoreMessageFlags(false, sequence, IMAP_Flags_SetType.Add, IMAP_MessageFlags.Deleted);
                }

                //client.CloseFolder();
                client.Expunge();
                client.Disconnect();
                client.Dispose();
            }
        }
        catch (Exception ex)
        {
            if (!Convert.ToBoolean(Application["ReplyToLiveInvalidDateOrderNotes"]))
            {
                if (ex.Message.StartsWith("Invalid Date: Invalid time zone. Input: \""))
                {
                    Application["ReplyToLiveInvalidDateOrderNotes"] = true;
                    #region Email Notification
                    using (MailMessage objEmail = new MailMessage())
                    {
                        objEmail.Body = "The Reply-to-email has got some invalid email. <br /><br />Get rid of this email to get the function working again.";
                        objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                        objEmail.IsBodyHtml = true;
                        objEmail.Subject = "Reply-to-email function (local environment) not working.";
                        objEmail.To.Add(new MailAddress("prashanth.kankhara@indianic.com"));

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

        //Signatures
        Regex Thanks = new Regex("([\r\n\f]Thanks[\r\n\f]\\s*|[\r\n\f]Thanks,[\r\n\f]\\s*)", RegexOptions.IgnoreCase);
        Regex ThanksNRegards = new Regex("([\r\n\f]Thanks & Regards\\s*|[\r\n\f]Thanks and Regards\\s*)", RegexOptions.IgnoreCase);
        Regex Regards = new Regex("([\r\n\f]Regards\\s*)", RegexOptions.IgnoreCase);
        Regex WithRegards = new Regex("([\r\n\f]With Regards\\s*|[\r\n\f]With.*Regards\\s*)", RegexOptions.IgnoreCase);
        Regex WarmRegards = new Regex("([\r\n\f]Warm Regards\\s*)", RegexOptions.IgnoreCase);
        Regex BestRegards = new Regex("([\r\n\f]Best Regards\\s*)", RegexOptions.IgnoreCase);
        Regex Yours = new Regex("([\r\n\f]Yours\\s*|[\r\n\f]Yours,\\s*)", RegexOptions.IgnoreCase);

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

    private void SendNotes(Int64 OrderID, Int64 ToUserInfoID, Int64 NoteType,String Message)
    {
        try
        {
            Order objOrder = new OrderConfirmationRepository().GetByOrderID(OrderID);
            UserInformation objUserInformation = new UserInformationRepository().GetById(ToUserInfoID);

            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;

            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "IE NoteHistory";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();

                String sToadd = objUserInformation.LoginEmail;

                String sSubject = "Incentex Message - Order Number " + objOrder.OrderNumber;
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{OrderNo}", objOrder.OrderNumber);
                messagebody.Replace("{Commentssection}", Message);
                messagebody.Replace("{fullname}", objUserInformation.FirstName);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                if (new ManageEmailRepository().CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderNotes)) == true)
                {
                    //new CommonMails().SendMail(objUserInformation.UserInfoID, "Order Notes", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);

                    String sReplyToadd = "smtp.incentex@gmail.com";//"ordernotes@world-link.us.com";
                    String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+on" + objOrder.OrderID + "un" + objUserInformation.UserInfoID + "nt" + NoteType + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                    new CommonMails().SendMailWithReplyTo(objUserInformation.UserInfoID, "Order Notes", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true, OrderID);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}
