using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using com.strikeiron.ws;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using LumiSoft.Net;
using LumiSoft.Net.IMAP;
using LumiSoft.Net.IMAP.Client;
using OpenPop.Mime;
using OpenPop.Pop3;
using SAP_API;
using ASP;

public partial class controller_TestSmtp : Page
{
    public delegate ISingleResult<TestProcedureResult> SqlInfoMessageEventHandler(Object sender, SqlInfoMessageEventArgs e);

    protected void Page_Load(Object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Value");
            dt.Columns.Add("Text");
            dt.Columns.Add("IsChecked");

            for (Int32 i = 0; i < 20; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Text"] = "This is text " + (i + 1);
                dr["Value"] = "This is value " + i;
                dr["IsChecked"] = i % 2 == 0;
                dt.Rows.Add(dr);
            }

            dt.AcceptChanges();

            msdTest1.DataSource = dt;
            msdTest1.DataValueField = "Value";
            msdTest1.DataTextField = "Text";
            msdTest1.DataCheckField = "IsChecked";
            msdTest1.DataSelectText = "- Select -";
            msdTest1.DataBind();

            msdTest2.DataSource = dt;
            msdTest2.DataValueField = "Value";
            msdTest2.DataTextField = "Text";
            msdTest2.DataCheckField = "IsChecked";
            msdTest2.DataSelectText = "- Select -";
            msdTest2.DataBind();

            #region Test Code
            //Response.Write("Page_Load <br/>");
            //try
            //{
            //    Response.Write("List<IMAP_Envelope> lstNewReplies = new List<IMAP_Envelope>() { }; <br/>");
            //    List<IMAP_Envelope> lstNewReplies = new List<IMAP_Envelope>() { };
            //    Response.Write("lstNewReplies = GetMessagesViaIMAP(); <br/>");
            //    lstNewReplies = GetMessagesViaIMAP();

            //    if (lstNewReplies.Count > 0)
            //    {
            //        //List<Message> lstPOP = FetchAllMessages("pop.gmail.com", 995, true, "smtp.incentex@gmail.com", "smtp@incentex");
            //        Response.Write("List<Message> lstPOP = FetchAllMessages(); <br/>");
            //        List<Message> lstPOP = FetchAllMessages("127.0.0.1", 110, false, "replyto@world-link.us.com", "7940bhoh");

            //        Response.Write("foreach (IMAP_Envelope objReply in lstNewReplies) <br/>");
            //        foreach (IMAP_Envelope objReply in lstNewReplies)
            //        {
            //            #region GetMessagesViaPOP
            //            String Reply = String.Empty;
            //            Boolean isReply;
            //            Response.Write("String ReplyTo = objReply.To[0].ToString(); <br/>");
            //            String ReplyTo = objReply.To[0].ToString();
            //            Response.Write(objReply.To[0].ToString() + "<br/>");

            //            Response.Write("if (ReplyTo.Contains(+tn) && ReplyTo.Contains(ni) && ReplyTo.Contains(un) && ReplyTo.Contains(en) && lstPOP.Count > 0) <br/>");
            //            if (ReplyTo.Contains("+tn") && ReplyTo.Contains("ni") && ReplyTo.Contains("un") && ReplyTo.Contains("en") && lstPOP.Count > 0)
            //            {
            //                Response.Write("foreach (Message msg in lstPOP) <br/>");
            //                foreach (Message msg in lstPOP)
            //                {
            //                    Response.Write("if (msg.Headers.MessageId == objReply.MessageID || msg.Headers.MessageId.Contains(objReply.MessageID) || objReply.MessageID.Contains(msg.Headers.MessageId)) <br/>");
            //                    if (msg.Headers.MessageId == objReply.MessageID || msg.Headers.MessageId.Contains(objReply.MessageID) || objReply.MessageID.Contains(msg.Headers.MessageId))
            //                    {
            //                        Response.Write("Reply = cleanMsgBody(msg.MessagePart.MessageParts[0].GetBodyAsText(), out isReply); <br/>");
            //                        Reply = cleanMsgBody(msg.MessagePart.MessageParts[0].GetBodyAsText(), out isReply);
            //                        Response.Write("if (isReply == true) <br/>");
            //                        try
            //                        {
            //                            //Response.Write("Int64 TicketNo = Convert.ToInt64(ReplyTo.Substring(ReplyTo.IndexOf(+tn) + 3, ReplyTo.IndexOf(ni) - ReplyTo.IndexOf(+tn) - 3)); <br/>");
            //                            Response.Write("ReplyTo.IndexOf(+tn) = " + ReplyTo.IndexOf("+tn") + "<br/>");
            //                            Response.Write("ReplyTo.IndexOf(ni) = " + ReplyTo.IndexOf("ni") + "<br/>");

            //                            Int64 TicketNo = Convert.ToInt64(ReplyTo.Substring(ReplyTo.IndexOf("+tn") + 3, ReplyTo.IndexOf("ni") - ReplyTo.IndexOf("+tn") - 3));

            //                            //Response.Write("Int64 OldNoteID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.IndexOf(ni) + 2, ReplyTo.IndexOf(un) - ReplyTo.IndexOf(ni) - 2)); <br/>");
            //                            Response.Write("ReplyTo.IndexOf(un) = " + ReplyTo.IndexOf("un") + "<br/>");

            //                            Int64 OldNoteID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.IndexOf("ni") + 2, ReplyTo.IndexOf("un") - ReplyTo.IndexOf("ni") - 2));

            //                            //Response.Write("Int64 UserInfoID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.IndexOf(un) + 2, ReplyTo.IndexOf(@) - ReplyTo.IndexOf(un) - 2)); <br/>");
            //                            Response.Write("ReplyTo.IndexOf(en) = " + ReplyTo.IndexOf("en@") + "<br/>");

            //                            Int64 UserInfoID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.IndexOf("un") + 2, ReplyTo.IndexOf("en@") - ReplyTo.IndexOf("un") - 2));
            //                            Response.Write("Int64 UserInfoID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.IndexOf(un) + 2, ReplyTo.IndexOf(en@) - ReplyTo.IndexOf(un) - 2));");

            //                            //String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs);
            //                            //NoteDetail objComNot = new NoteDetail();
            //                            //NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();
            //                            //objComNot.Notecontents = Reply;
            //                            //objComNot.NoteFor = strNoteFor;
            //                            //objComNot.ForeignKey = TicketNo;
            //                            //objComNot.CreateDate = System.DateTime.Now;
            //                            //objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            //                            //objComNot.UpdateDate = System.DateTime.Now;
            //                            //objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

            //                            //objCompNoteHistRepos.Insert(objComNot);
            //                            //objCompNoteHistRepos.SubmitChanges();
            //                            Response.Write("SendEMailForNotesCE(TicketNo, OldNoteID, objComNot.NoteID, UserInfoID, Reply) <br/>");
            //                            //SendEMailForNotesCE(TicketNo, OldNoteID, objComNot.NoteID, UserInfoID, Reply);
            //                        }
            //                        catch (Exception ex)
            //                        {
            //                            Response.Write(ex.Message + "<br/>");
            //                            ErrHandler.WriteError(ex);
            //                        }
            //                    }
            //                }
            //            }
            //            #endregion
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ErrHandler.WriteError(ex);
            //}
            #endregion

            if (!IsPostBack)
            {
                String EnumName1 = Enum.GetName(typeof(Incentex.DAL.Common.DAEnums.UserTypes), Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin));
                String EnumName2 = GetDescription(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin);
                //gvTemp.DataSource = getDataTable();
                //gvTemp.DataBind();            
            }
            //txtServerTime.Text = DateTime.Now.AddHours(1).ToString("MM/dd/yyyy hh:mm tt");
            //txtServerTime.Text = CultureInfo.CurrentCulture.Name;
            lblServerTime.Text = System.TimeZone.CurrentTimeZone.StandardName;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public String GetDescription(Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());

        DescriptionAttribute attribute
                = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                    as DescriptionAttribute;

        return attribute == null ? value.ToString() : attribute.Description;
    }

    private DataTable getDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        DataRow dr = dt.NewRow();
        dr["ID"] = 1;
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["ID"] = 2;
        dt.Rows.Add(dr);

        Int32 a = 0;
        Int32? b = null;
        a = b ?? 0;

        return dt;
    }

    protected void btnErrorCheck_Click(Object sender, EventArgs e)
    {
        try
        {
            String test = "";
            Int32 i = Convert.ToInt32(test);

        }
        catch
        {

        }
    }

    protected void gvOrderItems_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkTxnDelete = (LinkButton)e.Row.FindControl("lnkDeleteTxn");

                String recStatus = "Approved";
                if (recStatus == "Approved")
                {
                    lnkTxnDelete.Enabled = false;
                    lnkTxnDelete.Attributes.Add("onclick", "return false;");
                }
                else
                {
                    lnkTxnDelete.Enabled = true;
                    lnkTxnDelete.Attributes.Add("onclick", "return confirm('!!--WARNING--!! You are about to delete the transaction. Performing this action will permanently remove the transaction and all its details from the database. Proceed?')");
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void Button1_Click(Object sender, EventArgs e)
    {
        try
        {
            for (Int32 i = 0; i < 3; i++)
            {
                CustomDropDown def = (CustomDropDown)LoadControl("~/NewDesign/UserControl/CustomDropDown.ascx");
                BindDropDown(def);
                pnlCustomDropDownPanel.Controls.Add(def);
            }

            //txtZipCode.Text = intRemover(txtCountryCode.Text.Trim());

            //IncentexAPI objAPI = new IncentexAPI();

            ////Testing Code for test SAP Items Transaction
            //InsertItemType objItem = new InsertItemType();
            //objItem.ItemCode = txtItemNumber.Text;
            //objItem.ItemDescription = txtDescription.Text;
            //objItem.Status = "Add";
            //InsertItemResponse objItemResponse = objAPI.InsertItem(objItem);
            //ltrInsertResponse.Text = " <br /> Log Message : " + objItemResponse.LogMessage + " <br /> ";
            //ltrInsertResponse.Text += " Log Status : " + objItemResponse.LogStatus + " <br /> ";
            //ltrInsertResponse.Text += " Created On : " + objItemResponse.CreatedOn + " <br /> ";
            //ltrInsertResponse.Text += " World-Link ID : " + objItemResponse.WorldLinkItemID + " <br /> ";

            //Testing Code for test SAP Items Transaction
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://216.157.31.142:8080/B1iXcellerator/exec/soap/vP.0010000105.in_WCSX/com.sap.b1i.vplatform.runtime/INB_WS_CALL_SYNC_XPT/INB_WS_CALL_SYNC_XPT.ipo/proc?" + objBO.ToString());
            //request.ContentType = "text/xml;charset=\"utf-8\"";
            //request.ContentLength = 0;
            //request.Accept = "text/xml";
            //request.Method = "POST";
            //request.Credentials = new NetworkCredential("B1iadmin", "B1iadmin");

            //WebResponse response = request.GetResponse();
            //Stream responseStream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(responseStream);
            //Response.Write(reader.ReadToEnd());
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void Button2_Click(Object sender, EventArgs e)
    {
        try
        {
            String txtBody = @"All copyright, trademarks and other intellectual property rights in the Website content.Story and is protected by copyright and other laws and international treaty provisions. Story jewellery co.uk is good site .You accept that this Website is offered on an 'as is' and 'as available' basis. Story Jewellery Co. does not warrant that this Website will be uninterrupted, timely, secure, error-free, virus free or that defects will be corrected. Story is not responsible for the accuracy of the information displayed on the website or omitted from the website including but not limited to product descriptions, stockists information or any information published or displayed throughout the website. Mr. Anderson would be visiting the store most probabley by today evening.";
            //txtBody = Regex.Replace(txtBody, @"\[(.*?)\]", String.Empty);
            String[] sentences = Regex.Split(txtBody, @"(?<=[.!?])\s+", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);

            //String[] sentences = Regex.Split(txtBody, @"(?<=[\.!\?])\s+");

            foreach (String sentence in sentences)
            {
                Response.Write("<br/><br/>" + sentence);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void Button3_Click(Object sender, EventArgs e)
    {
        try
        {
            //General.SendMail("support@world-link.us.com", "devraj.gadhavi@indianic.com", "Test smtp Subject", "Test smtp Body", "localhost", 25, true, false, "Incentex");
            List<IMAP_Envelope> lstIMAP = GetMessagesViaIMAP();
            Response.Write("lstIMAP.Count = " + lstIMAP.Count + "<br/>");

            foreach (IMAP_Envelope objReply in lstIMAP)
            {
                String ReplyTo = objReply.To[0].ToString();
                if (ReplyTo.Contains("+tn") && ReplyTo.Contains("ni") && ReplyTo.Contains("un") && ReplyTo.Contains("en"))
                {
                    Response.Write("String ReplyTo = objReply.To[0].ToString(); <br/>");
                    Response.Write(objReply.To[0].ToString() + "<br/>");
                    try
                    {
                        Response.Write("ReplyTo.IndexOf(+tn) = " + ReplyTo.IndexOf("+tn") + "<br/>");
                        Response.Write("ReplyTo.IndexOf(ni) = " + ReplyTo.IndexOf("ni") + "<br/>");

                        Int64 TicketNo = Convert.ToInt64(ReplyTo.Substring(ReplyTo.IndexOf("+tn") + 3, ReplyTo.IndexOf("ni") - ReplyTo.IndexOf("+tn") - 3));

                        //Response.Write("Int64 OldNoteID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.IndexOf(ni) + 2, ReplyTo.IndexOf(un) - ReplyTo.IndexOf(ni) - 2)); <br/>");
                        Response.Write("ReplyTo.IndexOf(un) = " + ReplyTo.IndexOf("un") + "<br/>");

                        Int64 OldNoteID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.IndexOf("ni") + 2, ReplyTo.IndexOf("un") - ReplyTo.IndexOf("ni") - 2));

                        //Response.Write("Int64 UserInfoID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.IndexOf(un) + 2, ReplyTo.IndexOf(@) - ReplyTo.IndexOf(un) - 2)); <br/>");
                        Response.Write("ReplyTo.IndexOf(en) = " + ReplyTo.IndexOf("en@") + "<br/>");

                        Int64 UserInfoID = Convert.ToInt64(ReplyTo.Substring(ReplyTo.IndexOf("un") + 2, ReplyTo.IndexOf("en@") - ReplyTo.IndexOf("un") - 2));
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message + "<br/>");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            ErrHandler.WriteError(ex);
        }
    }

    protected void Button4_Click(Object sender, EventArgs e)
    {
        try
        {
            using (MailMessage objEmail = new MailMessage())
            {
                objEmail.Body = @"<SalesOrderType xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
	<BO>
		<Documents>
			<row />
		</Documents>
		<Document_Lines />
		<AddressExtension>
			<row />
		</AddressExtension>
		<DocumentsAdditionalExpenses>
			<row />
		</DocumentsAdditionalExpenses>
	</BO>
</SalesOrderType>";
                objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                objEmail.IsBodyHtml = false;
                objEmail.Subject = "Test smtp";
                objEmail.To.Add(new MailAddress("devraj.gadhavi@indianic.com"));

                SmtpClient objSmtp = new SmtpClient();

                objSmtp.EnableSsl = Common.SSL;
                objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
                objSmtp.Host = Common.SMTPHost;
                objSmtp.Port = Common.SMTPPort;

                objSmtp.Send(objEmail);
            }

            //SalesOrderType objSalesOrder = new SalesOrderType();
            //objSalesOrder.BO = new SalesOrderTypeBO();
            //objSalesOrder.BO.AddressExtension = new SalesOrderTypeBOAddressExtension();
            //objSalesOrder.BO.AddressExtension.row = new SalesOrderTypeBOAddressExtensionRow();

            //objSalesOrder.BO.Document_Lines = new SalesOrderTypeBORow[] { };
            //objSalesOrder.BO.Documents = new SalesOrderTypeBODocuments();
            //objSalesOrder.BO.Documents.row = new SalesOrderTypeBODocumentsRow();
            //objSalesOrder.BO.DocumentsAdditionalExpenses = new SalesOrderTypeBODocumentsAdditionalExpenses();
            //objSalesOrder.BO.DocumentsAdditionalExpenses.row = new SalesOrderTypeBODocumentsAdditionalExpensesRow();

            //XmlSerializer objSerializer = new XmlSerializer(typeof(SalesOrderType));
            //StringBuilder objSB = new StringBuilder();
            //XmlWriter objWriter = XmlWriter.Create(objSB);
            //objSerializer.Serialize(objWriter, objSalesOrder);

            //IncentexAPI objAPI = new IncentexAPI();
            //OrderConfirmType objRespone = objAPI.UpdateSalesOrder(objSB.ToString());

            ////XmlDocument objDoc = new XmlDocument();
            ////objDoc.LoadXml(@"<SalesOrderType><BO><Documents><row /></Documents><Document_Lines /><AddressExtension><row /></AddressExtension><DocumentsAdditionalExpenses><row /></DocumentsAdditionalExpenses></BO></SalesOrderType>");
            ////objDoc.Save("E:\\Devraj\\Projects\\incentex01\\UpdateOrderString.xml");

            //Response.Write(objRespone.LogMessage);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void Button5_Click(Object sender, EventArgs e)
    {
        try
        {
            Response.Write(Convert.ToBoolean("abc"));

            if (!Convert.ToBoolean(Application["testAppVar"]))
                Application["testAppVar"] = true;

            Response.Write(Convert.ToBoolean(Application["testAppVar"]));

            //List<Message> lstPOP = FetchAllMessages(Convert.ToString(host.Value), Convert.ToInt32(popport.Value), Convert.ToBoolean(ssl.Value), Convert.ToString(username.Value), Convert.ToString(password.Value));

            //SqlConnection con = new SqlConnection("data source=IND587; Integrated Security=SSPI; initial catalog=TestMVCdb;User ID=sa;password=indianic;");

            //Int32 TotalRecords = 0;

            //foreach (GridViewRow gvRow in gvTemp.Rows)
            //{
            //    Repeater repeater = (Repeater)gvRow.FindControl("repeater1");

            //    foreach (RepeaterItem repItem in repeater.Items)
            //    {
            //        RadioButtonList rbList = (RadioButtonList)repItem.FindControl("radiobuttonlist1");

            //        foreach (ListItem item in rbList.Items)
            //        {
            //            if (item.Selected)
            //            {
            //                //code for selected items goes here...
            //            }
            //            else
            //            {
            //                //code for not selected items goes here...
            //            }

            //            if (item.Value == "0")
            //            {
            //                //code for items with value == "0" goes here...
            //            }

            //            if (item.Value == "1")
            //            {
            //                //code for items with value == "1" goes here...
            //            }
            //        }
            //    }
            //    TextBox TextBox1 = (TextBox)gvRow.FindControl("TextBox1");

            //    String q = "INSERT INTO dbo.tblCountry ([Name], [Status]) VALUES('" + TextBox1.Text.Trim() + "', 1)";
            //    SqlCommand cmd = new SqlCommand(q, con);

            //    con.Open();
            //    TotalRecords += cmd.ExecuteNonQuery();
            //    con.Close();
            //}
        }
        catch (Exception ex)
        {
            ex.Source = "UserInfoID : " + IncentexGlobal.CurrentMember.UserInfoID + ", Buttn5_Click : " + ex.Source;
            Response.Write(ex + "<br/>");
            Response.Write(ex.Source);
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvTemp_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Repeater repeater1 = (Repeater)e.Row.FindControl("repeater1");
            repeater1.DataSource = getDataTable();
            repeater1.DataBind();

            CheckBoxList chkList = (CheckBoxList)e.Row.FindControl("chkList");
            chkList.DataSource = getDataTable();
            chkList.DataBind();

            foreach (ListItem item in chkList.Items)
            {
                item.Attributes.Add("class", "chkItem");
            }

            foreach (RepeaterItem item in repeater1.Items)
            {
                if (item.ItemType == ListItemType.Footer)
                {

                }
            }
        }
    }

    protected void gvTemp_RowDeleting(Object sender, GridViewDeleteEventArgs e)
    {
        //String abc = gvTemp.Rows[e.RowIndex].Cells[1].Text;
        //String Connection = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        //SqlDataSource pqr = new SqlDataSource();
        //pqr.DeleteCommand = "DELETE FROM dbo.INC_Lookup WHERE iLookupID = @iLookup_ID";
        //pqr.DeleteParameters.Add("iLookup_ID", "2359");
        //pqr.ConnectionString = Connection;
        //Int32 i = pqr.Delete();
    }

    protected void repeater1_ItemCommand(Object sender, RepeaterCommandEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            if (e.CommandName == "button1")
            {
                Button btnButton = (Button)e.Item.FindControl("btnButton");

            }
            else if (e.CommandName == "button2")
            {
                Button btnButton = (Button)e.Item.FindControl("btnButton2");
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
    public static List<Message> FetchAllMessages(String hostname, Int32 port, Boolean useSsl, String username, String password)
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

            List<MailMessage> lstMailMessages = new List<MailMessage>(messageCount);

            // Messages are numbered in the interval: [1, messageCount]
            // Ergo: message numbers are 1-based.
            for (Int32 i = 1; i <= messageCount; i++)
            {
                allMessages.Add(client.GetMessage(i));
                //client.DeleteMessage(i);
            }

            //client.DeleteAllMessages();
            client.Disconnect();
            client.Dispose();

            // Now return the fetched messages
            return allMessages;
        }
    }

    private List<IMAP_Envelope> GetMessagesViaIMAP()
    {
        List<IMAP_Envelope> lstMessages = new List<IMAP_Envelope>() { };

        try
        {
            using (IMAP_Client client = new IMAP_Client())
            {
                #region Gmail Test Credentials
                //client.Connect("imap.gmail.com", 993, true);
                //client.Login("smtp.incentex@gmail.com", "smtp@incentex");
                //client.SelectFolder("INBOX");
                #endregion

                #region Live Credentials
                client.Connect(Convert.ToString(host.Value), Convert.ToInt32(imapport.Value), Convert.ToBoolean(ssl.Value));
                client.Login(Convert.ToString(username.Value), Convert.ToString(password.Value));
                client.SelectFolder("INBOX");
                #endregion

                #region Credentials Through Hidden Fields
                //client.Connect(Convert.ToString(host.Value), Convert.ToInt32(port.Value), false);
                //client.Login(Convert.ToString(username.Value), Convert.ToString(password.Value));
                //client.SelectFolder(Convert.ToString(folder.Value));
                #endregion

                IMAP_SequenceSet sequence = new IMAP_SequenceSet();
                //sequence.Parse("*:1"); // from first to last

                IMAP_Client_FetchHandler fetchHandler = new IMAP_Client_FetchHandler();

                fetchHandler.NextMessage += new EventHandler(delegate(Object s, EventArgs e) { });

                fetchHandler.Envelope += new EventHandler<EventArgs<IMAP_Envelope>>(delegate(Object s, EventArgs<IMAP_Envelope> e)
                {
                    IMAP_Envelope envelope = e.Value;
                    lstMessages.Add(envelope);
                });

                // the best way to find unread emails is to perform server search
                //Int32[] unseen_ids = client.Search(false, "UTF-8", "unseen");
                Int32[] unseen_ids = client.Search(false, null, "unseen");
                //String charset = null;
                //if (!String.IsNullOrEmpty(txtCharSet.Text.Trim()))
                //    charset = txtCharSet.Text.Trim();                
                //Int32[] unseen_ids = client.Search(false, charset, Convert.ToString(criteria.Value));
                //Console.WriteLine("unseen count: " + unseen_ids.Count().ToString());

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

                    //uncomment this line to mark messages as read
                    //client.StoreMessageFlags(false, sequence, IMAP_Flags_SetType.Add, IMAP_MessageFlags.Seen);
                }

                client.Disconnect();
                client.Dispose();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return lstMessages;
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

    protected void hdnButton_Click(Object sender, EventArgs e)
    {
        String Value = hdnCurrent.Value;

        if (Value == "")
        {
            //
        }
        else
        {
            //
        }
    }

    protected void btnLoadOrderToUpdate_Click(Object sender, EventArgs e)
    {
        Label lblMultiSelect = (Label)msdTest1.FindControl("lblMultiSelect");
        Repeater repMultiSelect = (Repeater)msdTest1.FindControl("repMultiSelect");

        foreach (RepeaterItem row in repMultiSelect.Items)
        {
            CheckBox chkMultiSelect = (CheckBox)row.FindControl("chkMultiSelect");
            HiddenField hdnMultiSelectID = (HiddenField)row.FindControl("hdnMultiSelectID");
        }

        if (!String.IsNullOrEmpty(txtOrderNumberToUpdate.Text.Trim()))
        {
            Order objOrder = new OrderConfirmationRepository().GetByOrderNo(txtOrderNumberToUpdate.Text.Trim());

            if (objOrder != null)
            {
                if (objOrder.OrderFor == "ShoppingCart")
                {
                    List<MyShoppinCart> lstItems = new MyShoppingCartRepository().GetShoppingCartByOrderID(objOrder.OrderID);

                    if (lstItems.Count > 0)
                    {
                        gvOrderItems.DataSource = lstItems;
                        gvOrderItems.DataBind();

                        gvOrderItems.Visible = true;
                        btnUpdateFromSAP.Visible = true;
                    }
                    else
                    {
                        lblMessage.Text = "No items found for order # " + txtOrderNumberToUpdate.Text.Trim() + ".";
                    }
                }
                else
                {
                    List<MyIssuanceCartRepository.MyIssuanceCartDetails> lstItems = new MyIssuanceCartRepository().GetUniformIssuancePolicyDetailsByOrderID(objOrder.OrderID);

                    if (lstItems.Count > 0)
                    {
                        gvOrderItems.DataSource = lstItems;
                        gvOrderItems.DataBind();

                        gvOrderItems.Visible = true;
                        btnUpdateFromSAP.Visible = true;
                    }
                    else
                    {
                        lblMessage.Text = "No items found for order # " + txtOrderNumberToUpdate.Text.Trim() + ".";
                    }
                }
            }
            else
            {
                lblMessage.Text = "Order not found for order # " + txtOrderNumberToUpdate.Text.Trim() + ".";
            }
        }
        else
        {
            lblMessage.Text = "Please enter order # to load.";
        }
    }

    protected void btnUpdateFromSAP_Click(Object sender, EventArgs e)
    {
        SalesOrderType objSAPOrder = new SalesOrderType();
        objSAPOrder.BO = new SalesOrderTypeBO();
        objSAPOrder.BO.AddressExtension = new SalesOrderTypeBOAddressExtension();
        objSAPOrder.BO.AddressExtension.row = new SalesOrderTypeBOAddressExtensionRow();
        objSAPOrder.BO.AddressExtension.row.Address = "Address Line 1";
        objSAPOrder.BO.AddressExtension.row.Address2 = "Address Line 2";
        objSAPOrder.BO.AddressExtension.row.CityS = "Orange Park";
        objSAPOrder.BO.AddressExtension.row.CountryS = "US";
        objSAPOrder.BO.AddressExtension.row.CountyS = "BROWARD";
        objSAPOrder.BO.AddressExtension.row.StateS = "FL";
        objSAPOrder.BO.AddressExtension.row.U_BillToCode = "Ana Garcia";
        objSAPOrder.BO.AddressExtension.row.ZipCodeS = "33322";
        objSAPOrder.BO.AddressExtension.row.ShipToBuilding = "Gabrial Smith Anderson";
        objSAPOrder.BO.AddressExtension.row.ShipToStreetNo = "A 302";

        objSAPOrder.BO.Document_Lines = new SalesOrderTypeBORow[gvOrderItems.Rows.Count];

        foreach (GridViewRow gvRowItem in gvOrderItems.Rows)
        {
            Label lblItemNumber = (Label)gvRowItem.FindControl("lblItemNumber");
            TextBox txtQuantity = (TextBox)gvRowItem.FindControl("txtQuantity");
            HiddenField hdnWorkGroupID = (HiddenField)gvRowItem.FindControl("hdnWorkGroupID");
            HiddenField hdnDescription = (HiddenField)gvRowItem.FindControl("hdnDescription");

            SalesOrderTypeBORow objSAPItemRow = new SalesOrderTypeBORow();
            objSAPItemRow.Dscription = "";
            objSAPItemRow.ItemCode = lblItemNumber.Text.Trim();
            objSAPItemRow.Dscription = hdnDescription.Value.Trim();
            objSAPItemRow.Quantity = txtQuantity.Text.Trim();
            objSAPItemRow.U_WorkgroupID = hdnWorkGroupID.Value.Trim();

            objSAPOrder.BO.Document_Lines[gvRowItem.RowIndex] = objSAPItemRow;
        }

        objSAPOrder.BO.Documents = new SalesOrderTypeBODocuments();
        objSAPOrder.BO.Documents.row = new SalesOrderTypeBODocumentsRow();
        objSAPOrder.BO.Documents.row.CntctCode = "Kelly Scott";
        objSAPOrder.BO.Documents.row.CreateDate = DateTime.Now.ToString();
        objSAPOrder.BO.Documents.row.DiscSum = "14.86";
        objSAPOrder.BO.Documents.row.DocDate = DateTime.Now.ToString();
        objSAPOrder.BO.Documents.row.DocStatus = "Open";
        objSAPOrder.BO.Documents.row.DocTotal = "50.81";
        objSAPOrder.BO.Documents.row.U_AnnivCred = "15.39";
        objSAPOrder.BO.Documents.row.U_EmployeeNo = "123456";
        objSAPOrder.BO.Documents.row.U_IssuancePackage = "N";
        objSAPOrder.BO.Documents.row.U_StoreID = "4";
        objSAPOrder.BO.Documents.row.U_WL_OrderNo = txtOrderNumberToUpdate.Text.Trim();
        objSAPOrder.BO.Documents.row.U_U_WL_Tax = "";
        objSAPOrder.BO.Documents.row.UpdateDate = DateTime.Now.ToString();

        objSAPOrder.BO.DocumentsAdditionalExpenses = new SalesOrderTypeBODocumentsAdditionalExpenses();
        objSAPOrder.BO.DocumentsAdditionalExpenses.row = new SalesOrderTypeBODocumentsAdditionalExpensesRow();
        objSAPOrder.BO.DocumentsAdditionalExpenses.row.ExpenseCode = "2";
        objSAPOrder.BO.DocumentsAdditionalExpenses.row.LineTotal = "5.99";
        objSAPOrder.BO.DocumentsAdditionalExpenses.row.Remarks = "Trial for updating Sales Order in SAP.";

        IncentexAPI objAPI = new IncentexAPI();
        OrderConfirmType objResponse1 = objAPI.UpdateSalesOrderBySalesOrderType(objSAPOrder);

        ltrOrderResponse.Text = "<br/>Request Status : " + objResponse1.LogStatus;
        ltrOrderResponse.Text += "<br/>Message : " + objResponse1.LogMessage;
        //ltrOrderResponse.Text += "<br/>Order Status : " + objResponse1.OrderStatus;
        ltrOrderResponse.Text += "<br/>WL Order # : " + objResponse1.WordlinkOrderNumber;
        ltrOrderResponse.Text += "<br/>SAP OrderID : " + objResponse1.SAPOrderID;
        ltrOrderResponse.Text += "<br/>Created On : " + objResponse1.CreatedOn;

        //String strOrder = GetXMLFromObject(objSAPOrder);

        //OrderConfirmType objResponse2 = objAPI.UpdateSalesOrderByString(strOrder);

        //ltrOrderResponse.Text += "<br/><br/>Request Status : " + objResponse2.LogStatus;
        //ltrOrderResponse.Text += "<br/>Message : " + objResponse2.LogMessage;
        ////ltrOrderResponse.Text += "<br/>Order Status : " + objResponse2.OrderStatus;
        //ltrOrderResponse.Text += "<br/>WL Order # : " + objResponse2.WordlinkOrderNumber;
        //ltrOrderResponse.Text += "<br/>SAP OrderID : " + objResponse2.SAPOrderID;
        //ltrOrderResponse.Text += "<br/>Created On : " + objResponse2.CreatedOn;

        //XmlDocument xmlOrder = new XmlDocument();
        //xmlOrder.LoadXml(strOrder);

        //OrderConfirmType objResponse3 = objAPI.UpdateSalesOrderByXmlDocument(xmlOrder);

        //ltrOrderResponse.Text += "<br/><br/>Request Status : " + objResponse3.LogStatus;
        //ltrOrderResponse.Text += "<br/>Message : " + objResponse3.LogMessage;
        //ltrOrderResponse.Text += "<br/>Order Status : " + objResponse3.OrderStatus;
        //ltrOrderResponse.Text += "<br/>WL Order # : " + objResponse3.WordlinkOrderNumber;
        //ltrOrderResponse.Text += "<br/>SAP OrderID : " + objResponse3.SAPOrderID;
        //ltrOrderResponse.Text += "<br/>Created On : " + objResponse3.CreatedOn;
    }

    protected void btnUpdateShipment_Click(Object sender, EventArgs e)
    {
        OrderShipmentType objShipment = new OrderShipmentType();
        objShipment.BO = new OrderShipmentTypeBO();
        objShipment.BO.Documents = new OrderShipmentTypeBODocuments();
        objShipment.BO.Documents.row = new OrderShipmentTypeBODocumentsRow();
        objShipment.BO.Documents.row.U_WL_OrderNo = "2013003767";

        objShipment.BO.Document_Lines = new OrderShipmentTypeBORow[3];

        for (SByte i = 0; i < 3; i++)
        {
            OrderShipmentTypeBORow objLineItem = new OrderShipmentTypeBORow();
            objLineItem.CarrierName = "UPS";
            objLineItem.ItemCode = "SPA - 12" + i;
            objLineItem.NoOfBoxes = "1";
            objLineItem.ShippedDateTime = DateTime.Now.ToString("MMddyyyy HH:mm:ss");
            objLineItem.ShippedQuantity = Convert.ToString(i + 1);
            objLineItem.TrackingNumber = Guid.NewGuid().ToString();

            objShipment.BO.Document_Lines[i] = objLineItem;
        }

        IncentexAPI objAPI = new IncentexAPI();
        OrderShipmentResponse objResponse = objAPI.UpdateSalesOrderShipment(objShipment);

        ltrOrderResponse.Text = "<br/>Request Status : " + objResponse.LogStatus;
        ltrOrderResponse.Text += "<br/>Message : " + objResponse.LogMessage;
        //ltrOrderResponse.Text += "<br/>Order Status : " + objResponse1.OrderStatus;
        ltrOrderResponse.Text += "<br/>WL Order # : " + objResponse.WorldLinkOrderNumber;
        ltrOrderResponse.Text += "<br/>SAP OrderID : " + objResponse.SAPOrderID;
        ltrOrderResponse.Text += "<br/>Created On : " + objResponse.CreatedOn;
    }

    private String GetXMLFromObject(Object o)
    {
        XmlSerializer XmlS = new XmlSerializer(o.GetType());

        StringWriter sw = new StringWriter();
        XmlTextWriter tw = new XmlTextWriter(sw);

        XmlS.Serialize(tw, o);
        return sw.ToString();
    }

    protected void btnTestProcedure_Click(Object sender, EventArgs e)
    {
        SqlInfoMessageEventHandler sqlInfoDelegate = new SqlInfoMessageEventHandler(SqlInfoReceived);
        IAsyncResult objAsync = sqlInfoDelegate.BeginInvoke(null, null, null, null);

        //ISingleResult<TestProcedureResult> objResult;

        do
        {
            Response.Write("<br/>State : " + objAsync.AsyncState);
        }
        while (objAsync.IsCompleted);

        Response.Write("<br/>Completed : " + Convert.ToString(objAsync.CompletedSynchronously));

        //objResult = sqlInfoDelegate.EndInvoke(objAsync);

        //if (objResult != null)
        //{
        //    Response.Write("<br/>" + objResult.FirstOrDefault().Column1);
        //}
    }

    private ISingleResult<TestProcedureResult> SqlInfoReceived(Object sender, SqlInfoMessageEventArgs e)
    {
        return new IncentexBEDataContext().TestProcedure();
    }

    protected void btnGetStrikeIronResponseUS_Click(Object sender, EventArgs e)
    {
        /// This sample project demonstrates the use of the GetTaxRateUS operation within StrikeIron Sales & Use Tax Basic 5.0
        /// In order to run the application, you must have a Registered StrikeIron account.  If you do not have a Registered
        /// StrikeIron account, you can obtain one here: http://www.strikeiron.com/Register.aspx
        /// 
        /// The web service definition for used in the Web Reference for this project is located at http://ws.strikeiron.com/taxdatabasic5?WSDL
        /// You can view more information about this web service here: https://strikeiron.com/ProductDetail.aspx?p=444
        /// 
        /// Integrating a web service into a .NET application usually requires adding a Web Reference to the project using that
        /// web service's definition.  A Web Reference has already been added to this project; the reference was assign the
        /// namespace TaxBasicRef.  The namespace has been added to this codefile above.
        /// 

        /// Variables storing authentication values are declared below.  As a Registered StrikeIron user, you can authenticate
        /// to a StrikeIron web service with either a UserID/Password combination or a License Key.  If you wish to use a
        /// License Key, assign this value to the UserID field and set the Password field to null.
        /// 
        String userID = ConfigurationSettings.AppSettings["StrikeironUserID"];
        String password = ConfigurationSettings.AppSettings["StrikeironPassword"];

        /// Inputs for the GetTaxRateUS operation are declared below.
        /// 

        /// To access the web service operations, you must declare a web service client Object.  This Object will contain
        /// all of the methods available in the web service and properties for each portion of the SOAP header.
        /// The class name for the web service client Object (assigned automatically by the Web Reference) is TaxDataBasic.
        /// 
        TaxDataBasic siService = new TaxDataBasic();

        /// StrikeIron web services accept user authentication credentials as part of the SOAP header.  .NET web service client
        /// objects represent SOAP header values as public fields.  The name of the field storing the authentication credentials
        /// is LicenseInfoValue (class type LicenseInfo).
        /// 
        LicenseInfo authHeader = new LicenseInfo();

        /// Registered StrikeIron users pass authentication credentials in the RegisteredUser section of the LicenseInfo Object.
        /// (property name: RegisteredUser; class name: RegisteredUser)
        /// 
        RegisteredUser regUser = new RegisteredUser();

        /// Assign credential values to this RegisteredUser Object
        /// 
        regUser.UserID = userID;
        regUser.Password = password;

        /// The populated RegisteredUser Object is now assigned to the LicenseInfo Object, which is then assigned to the web
        /// service client Object.
        /// 
        authHeader.RegisteredUser = regUser;
        siService.LicenseInfoValue = authHeader;

        String zipCode = txtZipCodeUS.Text.Trim(); //this is the ZIP code to get for which the operation will return data
        zipCode = zipCode.Contains("-") ? zipCode.Substring(0, zipCode.IndexOf("-")) : zipCode.Length == 9 ? zipCode.Substring(0, 5) : zipCode;

        /// The GetTaxRateUS operation can now be called.  The output type for this operation is SIWSOutputOfTaxRateUSAData.
        /// Note that for simplicity, there is no error handling in this sample project.  In a production environment, any
        /// web service call should be encapsulated in a try-catch block.
        /// 
        SIWsOutputOfTaxRateUSAData usOutput = siService.GetTaxRateUS(zipCode);

        /// The output objects of this StrikeIron web service contains two sections: ServiceStatus, which stores data
        /// indicating the success/failure status of the the web service request; and ServiceResult, which contains the
        /// actual data returne as a result of the request.
        /// 
        /// ServiceStatus contains two elements - StatusNbr: a numeric status code, and StatusDescription: a String
        /// describing the status of the output Object.  As a standard, you can apply the following assumptions for the value of
        /// StatusNbr:
        ///   200-299: Successful web service call (data found, etc...)
        ///   300-399: Nonfatal error (No data found, etc...)
        ///   400-499: Error due to invalid input
        ///   500+: Unexpected internal error; contact support@strikeiron.com
        if (usOutput.ServiceStatus.StatusNbr >= 300)
            return;

        lblTaxRateUS.Text = Convert.ToDecimal(usOutput.ServiceResult.TotalSalesTax).ToString();
        lblCountyNameUS.Text = Convert.ToString(usOutput.ServiceResult.County);
    }

    protected void btnGetStrikeIronResponseCA_Click(Object sender, EventArgs e)
    {
        /// The web service definition for used in the Web Reference for this project is located at http://ws.strikeiron.com/taxdatabasic5?WSDL
        /// You can view more information about this web service here: https://strikeiron.com/ProductDetail.aspx?p=444

        /// Variables storing authentication values are declared below.  As a Registered StrikeIron user, you can authenticate
        /// to a StrikeIron web service with either a UserID/Password combination or a License Key.  If you wish to use a
        /// License Key, assign this value to the UserID field and set the Password field to null.
        /// 
        String userID = ConfigurationSettings.AppSettings["StrikeironUserID"];
        String password = ConfigurationSettings.AppSettings["StrikeironPassword"];

        /// To access the web service operations, you must declare a web service client Object.  This Object will contain
        /// all of the methods available in the web service and properties for each portion of the SOAP header.
        /// The class name for the web service client Object (assigned automatically by the Web Reference) is TaxDataBasic.
        /// 
        TaxDataBasic siService = new TaxDataBasic();

        /// StrikeIron web services accept user authentication credentials as part of the SOAP header.  .NET web service client
        /// objects represent SOAP header values as public fields.  The name of the field storing the authentication credentials
        /// is LicenseInfoValue (class type LicenseInfo).
        /// 
        LicenseInfo authHeader = new LicenseInfo();

        /// Registered StrikeIron users pass authentication credentials in the RegisteredUser section of the LicenseInfo Object.
        /// (property name: RegisteredUser; class name: RegisteredUser)
        /// 
        RegisteredUser regUser = new RegisteredUser();

        /// Assign credential values to this RegisteredUser Object
        /// 
        regUser.UserID = userID;
        regUser.Password = password;

        /// The populated RegisteredUser Object is now assigned to the LicenseInfo Object, which is then assigned to the web
        /// service client Object.
        /// 
        authHeader.RegisteredUser = regUser;
        siService.LicenseInfoValue = authHeader;

        /// Inputs for the GetTaxRateUS operation are declared below.
        /// 
        String province = txtProvinceCA.Text.Trim(); //this is the ZIP code to get for which the operation will return data

        /// The GetTaxRateCanada operation can now be called.  The output type for this operation is SIWsOutputOfTaxRateCanadaData.
        /// Note that for simplicity, there is no error handling in this sample project.  In a production environment, any
        /// web service call should be encapsulated in a try-catch block.
        /// 
        SIWsOutputOfTaxRateCanadaData caOutput = siService.GetTaxRateCanada(province);

        /// The output objects of this StrikeIron web service contains two sections: ServiceStatus, which stores data
        /// indicating the success/failure status of the the web service request; and ServiceResult, which contains the
        /// actual data returne as a result of the request.
        /// 
        /// ServiceStatus contains two elements - StatusNbr: a numeric status code, and StatusDescription: a String
        /// describing the status of the output Object.  As a standard, you can apply the following assumptions for the value of
        /// StatusNbr:
        ///   200-299: Successful web service call (data found, etc...)
        ///   300-399: Nonfatal error (No data found, etc...)
        ///   400-499: Error due to invalid input
        ///   500+: Unexpected internal error; contact support@strikeiron.com
        if (caOutput.ServiceStatus.StatusNbr >= 300)
            return;

        lblGSTCA.Text = Convert.ToDecimal(caOutput.ServiceResult.GST).ToString();
        lblPSTCA.Text = Convert.ToDecimal(caOutput.ServiceResult.PST).ToString();
        lblTotalCA.Text = Convert.ToDecimal(caOutput.ServiceResult.Total).ToString();
        lblAbbreviationCA.Text = Convert.ToString(caOutput.ServiceResult.Abbreviation) + ", " + Convert.ToString(caOutput.ServiceResult.TaxShippingHandling) + ", " + Convert.ToString(caOutput.ServiceResult.Province);
    }



    protected void btnUpdateSalesOrderFromXML_Click(object Sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(txtOrderUpdateXMLPath.Text.Trim()))
            {
                lblMessage.Text = "Please enter path for Order Update XML.";
                return;
            }
            else
            {
                if (File.Exists(txtOrderUpdateXMLPath.Text.Trim()))
                {
                    XmlReader objXmlReader = XmlReader.Create(txtOrderUpdateXMLPath.Text.Trim());
                    XmlSerializer objXmlSerializer = new XmlSerializer(typeof(SalesOrderType));

                    OrderConfirmType objResponse = new OrderConfirmType();
                    SalesOrderType objSalesOrder = new SalesOrderType();

                    objSalesOrder = (SalesOrderType)objXmlSerializer.Deserialize(objXmlReader);
                    objXmlReader.Close();
                    IncentexAPI objAPI = new IncentexAPI();

                    objResponse = objAPI.UpdateSalesOrderBySalesOrderType(objSalesOrder);

                    lblMessage.Text = objResponse.LogMessage;
                }
                else
                {
                    lblMessage.Text = "The file does not exists.";
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnUpdateShipmentFromXML_Click(Object Sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(txtShipmentUpdateXMLPath.Text.Trim()))
            {
                lblMessage.Text = "Please enter path for Order Update XML.";
                return;
            }
            else
            {
                if (File.Exists(txtShipmentUpdateXMLPath.Text.Trim()))
                {
                    XmlReader objXmlReader = XmlReader.Create(txtShipmentUpdateXMLPath.Text.Trim());
                    XmlSerializer objXmlSerializer = new XmlSerializer(typeof(OrderShipmentType));

                    OrderShipmentResponse objResponse = new OrderShipmentResponse();
                    OrderShipmentType objSalesOrderShipment = new OrderShipmentType();

                    objSalesOrderShipment = (OrderShipmentType)objXmlSerializer.Deserialize(objXmlReader);
                    objXmlReader.Close();
                    IncentexAPI objAPI = new IncentexAPI();

                    objResponse = objAPI.UpdateSalesOrderShipment(objSalesOrderShipment);

                    lblMessage.Text = objResponse.LogMessage;
                }
                else
                {
                    lblMessage.Text = "The file does not exists.";
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnTransmitToSAP_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(txtOrderNumberToTransmit.Text.Trim()))
        {
            Order objOrder = new OrderConfirmationRepository().GetByOrderNo(txtOrderNumberToTransmit.Text.Trim());
            if (objOrder != null)
            {
                new SAPOperations().SubmitOrderToSAP(objOrder.OrderID);
            }
            else
            {
                lblMessage.Text = "Could not find the order.";
            }
        }
        else
        {
            lblMessage.Text = "Please enter order number to Transmit to SAP.";
        }
    }

    private Boolean IsUsORCanadianZipCode(String zipCode)
    {
        String _usZipRegEx = @"^\d{5}(?:[-\s]*\d{4})?$";
        String _caZipRegEx = @"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])\ {0,1}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$";

        Boolean validZipCode = true;
        if ((!Regex.Match(zipCode, _usZipRegEx).Success) && (!Regex.Match(zipCode, _caZipRegEx).Success))
        {
            validZipCode = false;
        }
        return validZipCode;
    }

    public String intRemover(String input)
    {
        MatchCollection matches = Regex.Matches(input, @"[+-]?\d+(\.\d+)?");
        String abc = String.Empty;
        foreach (Match m in matches)
            abc += m.Value;

        return abc;
    }

    protected void btnSendEmail_Click(Object sender, EventArgs e)
    {
        try
        {
            IncentexBEDataContext db = new IncentexBEDataContext();
            List<TodaysEmailDetail> lstTodaysEmails = (from ted in db.TodaysEmailDetails
                                                       join o in db.Orders on ted.OrderID equals o.OrderID
                                                       where
                                                       o.OrderDate >= new DateTime(2013, 06, 15, 00, 00, 00)
                                                       && o.OrderDate <= new DateTime(2013, 06, 17, 00, 00, 00)
                                                       && ted.UserInfoID == 3224
                                                       select ted).ToList();

            if (lstTodaysEmails.Count > 0)
            {
                SmtpClient aClient = new SmtpClient();

                aClient.EnableSsl = Common.SSL;
                aClient.Credentials = new NetworkCredential(Common.UserName, Common.Password);
                aClient.Host = Common.SMTPHost;
                aClient.Port = Common.SMTPPort;

                foreach (TodaysEmailDetail objTodaysEmail in lstTodaysEmails)
                {
                    using (System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage())
                    {
                        //objEmail.Body = Convert.ToString(objTodaysEmail.MessageBody);
                        objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                        objEmail.IsBodyHtml = true;
                        objEmail.Subject = Convert.ToString(objTodaysEmail.EmailSubject);
                        //objEmail.To.Add(new MailAddress("orders@incentex.com"));
                        //objEmail.CC.Add(new MailAddress("lbowman@incentex.com"));
                        objEmail.Bcc.Add(new MailAddress("devraj.gadhavi@indianic.com"));

                        aClient.Send(objEmail);
                    }

                    break;
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void BindDropDown(CustomDropDown abc)
    {
        abc.DataSource = GetDataSource();
        abc.DataTextField = "Text";
        abc.DataValueField = "Value";
        abc.DataBind();

        abc.Value = "-1";
    }

    //protected void cddMyDropDown_SaveNewOptionAttempted(object sender, EventArgs e)
    //{
    //    cddMyDropDown.Items.Insert(cddMyDropDown.Items.Count - 1, new ListItem(Convert.ToString(cddMyDropDown.Text.Trim()), Convert.ToString(cddMyDropDown.Items.Count - 1)));
    //    cddMyDropDown.SelectedValue = cddMyDropDown.Items.FindByText(cddMyDropDown.Text.Trim()).Value;
    //    cddMyDropDown.Text = String.Empty;
    //}

    //protected void cddMyDropDown_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}

    private List<ListItem> GetDataSource()
    {
        List<ListItem> lstItems = new List<ListItem>();
        ListItem liSelect = new ListItem("- Select -", "0");
        lstItems.Add(liSelect);
        ListItem liIncentexInventory = new ListItem("Incentex Inventory", "1");
        lstItems.Add(liIncentexInventory);
        ListItem liCustomerInventory = new ListItem("Customer Inventory", "2");
        lstItems.Add(liCustomerInventory);
        ListItem li3rdPartyInventory = new ListItem("3rd Party Inventory", "3");
        lstItems.Add(li3rdPartyInventory);
        ListItem liAddNew = new ListItem("- Add New Option -", "-1");
        lstItems.Add(liAddNew);

        return lstItems;
    }
}