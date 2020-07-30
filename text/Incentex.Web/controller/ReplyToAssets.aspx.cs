using System;
using System.Collections;
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
using MailBee.ImapMail;
using MailBee.Mime;
using OpenPop.Mime;
using OpenPop.Pop3;
using System.Collections.Generic;

public partial class controller_ReplyToAssets : System.Web.UI.Page
{
    #region Properties

    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetIMAPEmails();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
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
            imap.Login(CommonMails.ReplyToAssetManagement, CommonMails.ReplyToAssetManagementPassword);
            //NOTE: Username and password are static as it was earlier in the OLD System. 

            // You can also use: LoginPLAIN, LoginCRAM, LoginDIGEST, LoginOAUTH methods,            
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

        String strFilePath = String.Empty;
        String strFileName = String.Empty;

        Response.Write("Attachments: " + "</br>");
        foreach (MailBee.Mime.Attachment attachment in email.Attachments)
        {
            Response.Write(attachment.Filename + "</br>");
            attachment.Save(@"c:\" + attachment.Filename, true);
            if (String.IsNullOrEmpty(strFilePath))
                strFilePath += ",";
            strFilePath += @"c:\" + attachment.Filename;

            if (String.IsNullOrEmpty(strFileName))
                strFileName += ",";
            strFileName += attachment.Filename;
        }

        try
        {
            String ReplyTo = email.To[0].ToString();

            //QueryString flow = an__un__rt__ad__cb__nf__snf__ni__fk__en
            //an: EquipmentMaster ID
            //un: user number(userid)
            //rt: recipient type: ax for annonymous user and admin for admin
            //cb: created by id 
            //nf: note for Options= 1: Basic,2:Accounting, 3: Specifications,4: Specifications
            //sf: note for Options= 1: LeaseAsset,2: PurchaseAsset, 3: AssetManuals, 4: AssetImagesVideos
            //ni: note id
            //fk: foreign key (Asset FileID)
            //en: end of the string
            //an128un1rtaxcb25nf2sf2nx35354fk20en
            if (ReplyTo.Contains("+an") && ReplyTo.Contains("un") && ReplyTo.Contains("rt") && ReplyTo.Contains("en") && ReplyTo.Contains("nf") && ReplyTo.Contains("cb") && ReplyTo.Contains("nx") && ReplyTo.Contains("fk"))
            {
                Int64 EquipmentMasterID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.LastIndexOf("+an") + 3, ReplyTo.LastIndexOf("un") - ReplyTo.LastIndexOf("+an") - 3));

                AssetMgtRepository AssetMgtRepos = new AssetMgtRepository();

                EquipmentMaster objEquipmentMaster = AssetMgtRepos.GetById(EquipmentMasterID);

                //If the OrderNotes Email is not deleted then only proceed, else skip to next email.
                if (objEquipmentMaster != null)
                {
                    String sUserInfoID = ReplyTo.Substring(ReplyTo.LastIndexOf("un") + 2, ReplyTo.LastIndexOf("rt") - ReplyTo.LastIndexOf("un") - 2);
                    String UserType = ReplyTo.Substring(ReplyTo.LastIndexOf("rt") + 2, ReplyTo.LastIndexOf("cb") - ReplyTo.LastIndexOf("rt") - 2);
                    Boolean IsUserFromAnonymousTable = false;
                    Int64 CreatedUserInfoId = 0;
                    Int64 NoteID = 0;
                    Int32 NoteFor = 0;
                    Int32 SpecificNoteFor = 0;
                    //Int64? UserInfoID = null;
                    Int64 ForeignKey = 0;


                    CreatedUserInfoId = Convert.ToInt64(ReplyTo.Substring(ReplyTo.LastIndexOf("cb") + 2, ReplyTo.LastIndexOf("nf") - ReplyTo.LastIndexOf("cb") - 2));
                    NoteFor = Convert.ToInt32(ReplyTo.Substring(ReplyTo.LastIndexOf("nf") + 2, ReplyTo.LastIndexOf("sf") - ReplyTo.LastIndexOf("nf") - 2));
                    SpecificNoteFor = Convert.ToInt32(ReplyTo.Substring(ReplyTo.LastIndexOf("sf") + 2, ReplyTo.LastIndexOf("nx") - ReplyTo.LastIndexOf("sf") - 2));
                    NoteID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.LastIndexOf("nx") + 2, ReplyTo.LastIndexOf("fk") - ReplyTo.LastIndexOf("nx") - 2));
                    ForeignKey = Convert.ToInt64(ReplyTo.Substring(ReplyTo.LastIndexOf("fk") + 2, ReplyTo.LastIndexOf("en@") - ReplyTo.LastIndexOf("fk") - 2));


                    String strNoteFor = String.Empty;
                    String strSpecificNoteFor = String.Empty;
                    if (NoteFor == 1)
                        strNoteFor = "Basic";
                    else if (NoteFor == 2)
                        strNoteFor = "Accounting";
                    else if (NoteFor == 3)
                        strNoteFor = "Warranty";
                    else if (NoteFor == 4)
                        strNoteFor = "Specifications";

                    if (SpecificNoteFor == 1)
                        strSpecificNoteFor = Convert.ToString(Incentex.DAL.Common.DAEnums.NoteForType.LeaseAsset);
                    else if (SpecificNoteFor == 2)
                        strSpecificNoteFor = Convert.ToString(Incentex.DAL.Common.DAEnums.NoteForType.PurchaseAsset);
                    else if (SpecificNoteFor == 3)
                        strSpecificNoteFor = Convert.ToString(Incentex.DAL.Common.DAEnums.NoteForType.AssetManuals);
                    else if (SpecificNoteFor == 4)
                        strSpecificNoteFor = Convert.ToString(Incentex.DAL.Common.DAEnums.NoteForType.AssetImagesVideos);

                    String Reply = String.Empty;
                    String TrailingNotesContent = String.Empty;
                    Reply = cleanMsgBody(Convert.ToString(email.BodyPlainText));

                    if (Reply.Length > 2500)
                    {
                        Reply = Reply.Substring(0, 2500);
                    }

                    #region !---------------Insert Reply Note Detail------------------!
                    NotesHistoryRepository objNotesHistoryRepository = new NotesHistoryRepository();

                    NoteDetail objNoteDetail = new NoteDetail();
                    objNoteDetail.ReceivedBy = Convert.ToString(EquipmentMasterID);
                    objNoteDetail.Notecontents = Reply;
                    objNoteDetail.NoteFor = strNoteFor;
                    objNoteDetail.SpecificNoteFor = strSpecificNoteFor;
                    objNoteDetail.ForeignKey = ForeignKey;
                    objNoteDetail.CreateDate = System.DateTime.Now;

                    objNotesHistoryRepository.Insert(objNoteDetail);
                    objNotesHistoryRepository.SubmitChanges();


                    AssetFilesNotesReceipientDetail objAssetFilesNotesReceipientDetails = new AssetFilesNotesReceipientDetail();
                    objAssetFilesNotesReceipientDetails.AssetID = EquipmentMasterID;
                    objAssetFilesNotesReceipientDetails.IsUserFromAnonymousUserTable = (UserType == "ax");
                    objAssetFilesNotesReceipientDetails.NoteID = objNoteDetail.NoteID;
                    objAssetFilesNotesReceipientDetails.RecipientID = CreatedUserInfoId;
                    objAssetFilesNotesReceipientDetails.CreatedBy = Convert.ToInt64(sUserInfoID);
                    objAssetFilesNotesReceipientDetails.CreatedDate = System.DateTime.Now;

                    AssetMgtRepos.Insert(objAssetFilesNotesReceipientDetails);
                    AssetMgtRepos.SubmitChanges();

                    TrailingNotesContent = AssetMgtRepos.TrailingNotes(Convert.ToString(EquipmentMasterID), strNoteFor, strSpecificNoteFor, "<br/>");
                    #endregion

                    //QueryString flow = on__un__ad__cb__nf__sf__nx__fk__en
                    //an: Asset ID
                    //un: user number(userid)
                    //cb: created by id 
                    //nf: note for Options= 1: Basic,2:Accounting, 3: Specifications,4: Specifications
                    //sf: note for Options= 1: LeaseAsset,2: PurchaseAsset, 3: AssetManuals, 4: AssetImagesVideos
                    //nx: note id
                    //fk: foreign key (Asset FileID)
                    //en: end of the string
                    //if (UserInfoID != CreatedUserInfoId)
                    //{

                    if (UserType == "ax")
                        UserType = "au";
                    else
                        UserType = "ax";
                    SendEMailRepliesForNotes(EquipmentMasterID, TrailingNotesContent, NoteFor, Convert.ToString(email.Subject), SpecificNoteFor, CreatedUserInfoId, Convert.ToInt64(sUserInfoID), NoteID, ForeignKey, strFilePath, strFileName, UserType);
                    //}
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

    private void SendEMailRepliesForNotes(Int64 EquipmentMasterID, String Reply, Int32 NoteFor, String sSubject, Int64 SpecificNoteFor, Int64 CreatedUserID, Int64 UserInfoID, Int64 NoteID, Int64 ForeignKey, String strFiles, String strFileNames, String UserType)
    {
        try
        {
            String eMailTemplate = String.Empty;
            String sReplyToadd = CommonMails.ReplyToAssetManagement;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/OrderNotes.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            AssetMgtRepository AssetRepos = new AssetMgtRepository();
            List<SelectRecipentsForReplyTo> lstRecipients = new List<SelectRecipentsForReplyTo>();
            lstRecipients = AssetRepos.GetRecipentsFromAssetID(CreatedUserID);


            foreach (SelectRecipentsForReplyTo recipient in lstRecipients)
            {
                String UserName = Convert.ToString(recipient.LastName) + " " + Convert.ToString(recipient.FirstName);

                string MessageBody = Reply;

                String sUserInfoID = recipient.UserInfoID == null ? "ax" : Convert.ToString(recipient.UserInfoID);
                //QueryString flow = an__un__ad__cb__nf__sf__nx__fk__en
                //an: Asset ID
                //un: user number(userid)
                //cb: created by id 
                //nf: note for Options= 1: Basic,2:Accounting, 3: Specifications,4: Specifications
                //snf: note for Options= 1: LeaseAsset,2: PurchaseAsset, 3: AssetManuals, 4: AssetImagesVideos
                //nx: note id
                //fk: foreign key (Asset FileID)
                //en: end of the string

                String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+an" + EquipmentMasterID + "un" + CreatedUserID + "rt" + UserType + "cb" + UserInfoID + "nf" + NoteFor + "sf" + SpecificNoteFor + "nx" + NoteID + "fk" + ForeignKey + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                CommonMails objCommonMails = new CommonMails();
                if (System.Web.HttpContext.Current.Request.IsLocal)
                    objCommonMails.SendMailWithReplyToANDAttachmentForAsset(Convert.ToInt64(sUserInfoID), "Asset Files", Common.EmailFrom, Convert.ToString(recipient.Email), sSubject, MessageBody.ToString(), Common.DisplyName, true, true, strFiles, Common.SMTPHost, Common.SMTPPort, Common.UserName, Common.Password, strFiles, false, ReplyToAddress);
                else
                    objCommonMails.SendMailWithReplyToANDAttachmentForAsset(Convert.ToInt64(sUserInfoID), "Asset Files", Common.EmailFrom, Convert.ToString(recipient.Email), sSubject, MessageBody.ToString(), Common.DisplyName, true, true, strFiles, Common.SMTPHost, Common.SMTPPort, Common.UserName, Common.Password, strFiles, false, ReplyToAddress);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}
