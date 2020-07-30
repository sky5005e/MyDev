using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.BE;
using System.IO;
using Incentex.DA;
using System.Data;
using System.Configuration;

public partial class admin_Report_SummaryOrderCustomerViewReport : PageBase
{
    #region Data Members
    ReportRepository objReportRepository = new ReportRepository();
    LookupRepository objLookupRepos = new LookupRepository();
    ReportAccessRightsRepository objReportAccessRightsRepository = new ReportAccessRightsRepository();
    OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();
    NotesHistoryRepository objNotesHistoryRepository = new NotesHistoryRepository();
    UserInformationRepository objUserInformationRepository = new UserInformationRepository();
    SupplierRepository objSupplierRepository = new SupplierRepository();
    Boolean IsFromIndexPage
    {
        get
        {
            if (ViewState["IsFromIndexPage"] == null)
            {
                ViewState["IsFromIndexPage"] = false;
            }
            return Convert.ToBoolean(ViewState["IsFromIndexPage"]);
        }
        set
        {
            ViewState["IsFromIndexPage"] = value;
        }
    }
    static List<SelectOrderSummaryViewReportResult> objResult = new List<SelectOrderSummaryViewReportResult>();

    #endregion

    #region Properties
    public static string strStatusView = "0";
    #endregion

    #region Page Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Order Management System";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Order Management Dashboard";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Report/ReportDashBoard.aspx";
            IncentexGlobal.ManageID = 9;
            if (!string.IsNullOrEmpty(Request.QueryString["IsFromIndexPage"]) && Request.QueryString["IsFromIndexPage"] == "true")
            {
                IsFromIndexPage = true;
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Report/ReportDashBoard.aspx?IsFromIndexPage=true";
            }
            if (Request.QueryString["StatusView"] != null && Request.QueryString["StatusView"] != "")
            {
                strStatusView = Convert.ToString(Request.QueryString["StatusView"]);
            }
            GenerateChart();
        }
    }

    protected void GenerateChart()
    {
        DateTime? FromDate = null;
        DateTime? ToDate = null;
        Int64? StoreID = null;
        Int64? WorkgroupID = null;
        Int64? GenderID = null;
        Int64? BaseStationID = null;
        Int64? UserInfoID = null;
        String OrderNumber = null;
        String FirstName = null;
        String LastName = null;

        //if (txtFromDate.Text != "")
        //    FromDate = Convert.ToDateTime(txtFromDate.Text.Trim());
        //if (txtToDate.Text != "")
        //    ToDate = Convert.ToDateTime(txtToDate.Text.Trim());
        //if (ddlCompanyStore.SelectedIndex > 0)
        //    StoreID = Convert.ToInt64(ddlCompanyStore.SelectedValue);
        //if (ddlWorkgroup.SelectedValue != "0")
        //    WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
        //if (ddlBasestation.SelectedValue != "0")
        //    BaseStationID = Convert.ToInt64(ddlBasestation.SelectedValue);
        //if (ddlGender.SelectedValue != "0")
        //    GenderID = Convert.ToInt64(ddlGender.SelectedValue);
        //if (txtOrderNumber.Text != "")
        //    OrderNumber = txtOrderNumber.Text.Trim();
        //if (txtFirstName.Text != "")
        //    FirstName = txtFirstName.Text.Trim();
        //if (txtLastName.Text != "")
        //    LastName = txtLastName.Text.Trim();
        if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;

        objResult = objReportRepository.GetOrderSummaryViewReport(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, OrderNumber, FirstName, LastName, strStatusView);

        if (objResult != null && objResult.Count > 0)
        {
            objResult = objResult.OrderByDescending(x => x.OrderID).ToList();

            //Getting CustomerDetails 
            var customerResult = (from result in objResult
                                  select new
                                  {
                                      UserID = result.UserId,
                                      Name = result.FirstName + " " + result.LastName ?? string.Empty
                                  }).Distinct().ToList();

            rpCustomer.DataSource = customerResult;
            rpCustomer.DataBind();
        }
    }

    protected void BindCustomerOrder(Int64 CustomerID, int ItemIndex)
    {

        #region Summary Order View
        //Getting Order Detail only 

        var orderResult = (from result in objResult
                           where result.UserId == CustomerID
                           select new
                           {
                               OrderID = result.OrderID,
                               OrderNumber = result.OrderNumber,
                               CompanyName = result.CompanyName,
                               OrderDate = result.OrderDate,
                               Name = result.FirstName + " " + result.LastName ?? string.Empty,
                               OrderStatus = result.OrderStatus,
                               DaysOpened = result.DaysOpened,
                               WorkgroupID = result.WorkgroupId,
                               CompanyID = result.CompanyID,
                               UserID = result.UserId
                           }).ToList();


        Repeater rptSummaryOrderView = (Repeater)rpCustomer.Items[ItemIndex].FindControl("rptSummaryOrderView");
        rptSummaryOrderView.DataSource = orderResult;
        rptSummaryOrderView.DataBind();

        Label lblCustomerOrderCounter = (Label)rpCustomer.Items[ItemIndex].FindControl("lblCustomerOrderCounter");
        lblCustomerOrderCounter.Text = orderResult.Count.ToString();

        foreach (RepeaterItem repeaterItem in rptSummaryOrderView.Items)
        {
            HiddenField hdnOrderID = (HiddenField)repeaterItem.FindControl("hdnOrderID");

            //Get item detail based on order id
            var orderItemResult = (from result in objResult
                                   where result.OrderID == Convert.ToInt64(hdnOrderID.Value) && result.MyShoppingCartID != null
                                   select new
                                   {
                                       ItemNumber = result.ItemNumber,
                                       QuantityOrdered = result.QuantityOrdered,
                                       Description = result.ProductDescrption,
                                       UnitPrice = result.UnitPrice,
                                       QuantityShipped = result.QuantityShipped != null ? result.QuantityShipped : 0,
                                       Status = result.Status != null ? result.Status : false,
                                       IsDropShipItem = result.IsDropShipItem == 1 ? true : false,
                                       MyShoppingCartID = result.MyShoppingCartID,
                                       Inventory = result.Inventory != null ? result.Inventory : 0,
                                       BackOrderedUntil = result.BackOrderedUntil,
                                       IsShippedComplete = (((result.QuantityOrdered - (result.QuantityShipped != null ? result.QuantityShipped : 0)) > 0) ? false : true)
                                   }).OrderBy(x => x.QuantityShipped >= x.QuantityOrdered).ThenBy(x => x.QuantityShipped != 0).ThenBy(x => x.Inventory).ToList();

            GridView grdItemDetail = ((GridView)(repeaterItem.FindControl("grdItemDetail")));
            grdItemDetail.DataSource = orderItemResult;
            grdItemDetail.DataBind();

            foreach (GridViewRow item in grdItemDetail.Rows)
            {
                HiddenField hdnReference = (HiddenField)item.FindControl("hdnReference");
                HiddenField hdnIsDropShipItem = (HiddenField)item.FindControl("hdnIsDropShipItem");
                Label lblQtyOnHand = (Label)item.FindControl("lblQtyOnHand");
                Image imgStatus = (Image)item.FindControl("imgStatus");
                Label lblQtyShipped = (Label)item.FindControl("lblQtyShipped");
                Label lblQtyOrdered = (Label)item.FindControl("lblQtyOrdered");
                LinkButton lnkbtnDetail = (LinkButton)item.FindControl("lnkbtnDetail");
                if (lblQtyOrdered.Text != string.Empty && lblQtyShipped.Text != string.Empty && Convert.ToInt64(lblQtyOrdered.Text) <= Convert.ToInt64(lblQtyShipped.Text))//This is for shipped completed
                {
                    imgStatus.ImageUrl = "~/Images/green_checkmark.png";
                    imgStatus.ToolTip = "All Items Shipped";
                }
                else if (lblQtyOrdered.Text != string.Empty && lblQtyShipped.Text != string.Empty && Convert.ToInt64(lblQtyShipped.Text) != 0 && Convert.ToInt64(lblQtyOrdered.Text) > Convert.ToInt64(lblQtyShipped.Text)) //This is for partial ship
                {
                    imgStatus.ImageUrl = "~/Images/purple_checkmark.png";
                    imgStatus.ToolTip = "Partial Ship of Inventory Items";
                }
                else if (Convert.ToBoolean(hdnIsDropShipItem.Value) == true && Convert.ToBoolean(hdnReference.Value) == true) //This is for Drop-Shipped items have been ordered
                {
                    imgStatus.ImageUrl = "~/Images/lightblue_checkmark.png";
                    imgStatus.ToolTip = "Drop-Shipped Items have been Ordered";
                }
                else if (Convert.ToBoolean(hdnIsDropShipItem.Value) == true) //This is for Drop-Ship Item and needs to be ordered
                {
                    imgStatus.ImageUrl = "~/Images/yellow_checkmark.png";
                    imgStatus.ToolTip = "Drop-Ship Items needs to be ordered";
                }
                else if (lblQtyOrdered.Text != string.Empty && lblQtyShipped.Text != string.Empty && Convert.ToInt64(lblQtyShipped.Text) == 0 && Convert.ToInt64(lblQtyOnHand.Text) <= 0) // nothing has shipped and item does not show inventory this is for non drop ship items.
                {
                    imgStatus.ImageUrl = "~/Images/red_checkmark.png";
                    imgStatus.ToolTip = "No Items Shipped and Inventory not Available";
                }
                else if (lblQtyOrdered.Text != string.Empty && lblQtyShipped.Text != string.Empty && Convert.ToInt64(lblQtyShipped.Text) == 0 && Convert.ToInt64(lblQtyOnHand.Text) > 0) // nothing has shipped and item does not show inventory this is for non drop ship items.
                {
                    imgStatus.ImageUrl = "~/Images/brightblue_checkmark.png";
                    imgStatus.ToolTip = "No Items Shipped and Inventory is Available";
                }
            }

            //Show the IE and time any drop-ship orders have been placed
            var orderReferenceResult = (from result in objResult
                                        where result.OrderID == Convert.ToInt64(hdnOrderID.Value) && result.DropShipReportID != null
                                        select new
                                        {
                                            IEName = result.IEName,
                                            UpdatedDate = result.UpdatedDate
                                        }).Distinct().ToList();

            #region Show all emails sent to the customer related to this order.
            List<TodayEmailsRepository.GetUserforMails> objEmailList = new TodayEmailsRepository().GetEmailsbyOrderID(Convert.ToInt64(hdnOrderID.Value));
            GridView grdEmailList = ((GridView)(repeaterItem.FindControl("grdEmailList")));
            grdEmailList.DataSource = objEmailList;
            grdEmailList.DataBind();
            #endregion

            UserInformationRepository objUserInformationRepository = new UserInformationRepository();

            #region Display internal notes for IE
            try
            {
                TextBox txtIEInternalNotes = ((TextBox)(repeaterItem.FindControl("txtIEInternalNotes")));
                List<NoteDetail> objList = objNotesHistoryRepository.GetNotesForIEPerOrderId(Convert.ToInt64(hdnOrderID.Value), Incentex.DAL.Common.DAEnums.NoteForType.OrderDetailsIEs, "IEInternalNotes",1);
                StringBuilder strNoteHistoryForIE = new StringBuilder();
                foreach (NoteDetail obj in objList)
                {
                    strNoteHistoryForIE.Append(obj.Notecontents + "\n\n");
                    UserInformation objUser = objUserInformationRepository.GetById(obj.CreatedBy);
                    if (objUser != null)
                    {
                        strNoteHistoryForIE.Append(objUser.FirstName + " " + objUser.LastName + "   ");
                    }
                    strNoteHistoryForIE.Append(Convert.ToDateTime(obj.CreateDate).ToString("MM/dd/yyyy"));
                    strNoteHistoryForIE.Append(" @ " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n");
                    strNoteHistoryForIE.Append("___________________________________________________________________________________________________\n\n");
                }

                //Append note who placed order with datetime as per ken discussed
                for (int i = 0; i < orderReferenceResult.Count; i++)
                {
                    strNoteHistoryForIE.Append("Drop-Ship Orders Placed \n\n");
                    strNoteHistoryForIE.Append(orderReferenceResult[i].IEName + "   ");
                    strNoteHistoryForIE.Append(Convert.ToDateTime(orderReferenceResult[i].UpdatedDate).ToString("MM/dd/yyyy"));
                    strNoteHistoryForIE.Append(" @ " + Convert.ToDateTime(orderReferenceResult[i].UpdatedDate).ToShortTimeString() + "\n");
                    strNoteHistoryForIE.Append("___________________________________________________________________________________________________\n\n");
                }

                txtIEInternalNotes.Text = strNoteHistoryForIE.ToString();
            }
            catch { }
            #endregion

            #region Display notes sent by IE to Vendor
            try
            {
                TextBox txtNotesForVendor = (TextBox)repeaterItem.FindControl("txtNotesForVendor");
                List<NoteDetail> objList = objNotesHistoryRepository.GetNotesForSuppliersPerOrderId(Incentex.DAL.Common.DAEnums.NoteForType.SupplierOrder, hdnOrderID.Value,1);
                StringBuilder strNoteHistoryForSupplier = new StringBuilder();
                foreach (NoteDetail obj in objList)
                {
                    strNoteHistoryForSupplier.Append(obj.Notecontents + "\n\n");
                    UserInformation objUser = objUserInformationRepository.GetById(obj.CreatedBy);

                    if (objUser != null)
                        strNoteHistoryForSupplier.Append(objUser.FirstName + " " + objUser.LastName + "   ");

                    strNoteHistoryForSupplier.Append(Convert.ToDateTime(obj.CreateDate).ToString("MM/dd/yyyy"));
                    strNoteHistoryForSupplier.Append(" @ " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n");
                    strNoteHistoryForSupplier.Append("___________________________________________________________________________________________________\n\n");
                }
                txtNotesForVendor.Text = strNoteHistoryForSupplier.ToString();
            }
            catch { }
            #endregion

            #region Display notes sent to Customer
            try
            {
                TextBox txtCustomerNotes = (TextBox)repeaterItem.FindControl("txtCustomerNotes");
                HiddenField hfUserID = (HiddenField)repeaterItem.FindControl("hfUserID");
                List<NoteDetail> objList = objNotesHistoryRepository.GetNotesSendtoCustomer(Incentex.DAL.Common.DAEnums.NoteForType.CACE, Convert.ToInt64(hdnOrderID.Value), hfUserID.Value);
                StringBuilder strCustomerNoteHistory = new StringBuilder();
                foreach (NoteDetail obj in objList)
                {
                    strCustomerNoteHistory.Append(obj.Notecontents + "\n\n");
                    UserInformation objUser = objUserInformationRepository.GetById(obj.CreatedBy);

                    if (objUser != null)
                        strCustomerNoteHistory.Append(objUser.FirstName + " " + objUser.LastName + "   ");

                    strCustomerNoteHistory.Append(Convert.ToDateTime(obj.CreateDate).ToString("MM/dd/yyyy"));
                    strCustomerNoteHistory.Append(" @ " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n");
                    strCustomerNoteHistory.Append("___________________________________________________________________________________________________\n\n");
                }
                txtCustomerNotes.Text = strCustomerNoteHistory.ToString();
            }
            catch { }
            #endregion

            #region Bind Back Order Management Items
            var backorderresult = orderItemResult.Where(o => o.Inventory != null && o.Inventory <= 0 && !o.IsShippedComplete).ToList();
            GridView gvBackOrderManagement = ((GridView)(repeaterItem.FindControl("gvBackOrderManagement")));
            gvBackOrderManagement.DataSource = backorderresult;
            gvBackOrderManagement.DataBind();
            HtmlTable back_save_table = (HtmlTable)(repeaterItem.FindControl("back_save_table"));
            if (gvBackOrderManagement.Rows.Count == 0)
            {
                back_save_table.Visible = false;
            }
            else
            {
                back_save_table.Visible = true;
            }
            #endregion
        }
        #endregion
    }

    private void SendNotes(Int64 OrderID, Int64 ToUserInfoID, Int64 NoteType)
    {
        try
        {
            Order objOrder = objOrderConfirmationRepository.GetByOrderID(OrderID);
            UserInformation objUserInformation = objUserInformationRepository.GetById(ToUserInfoID);

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
                messagebody.Replace("{Commentssection}", txtMessage.Text.Trim());
                messagebody.Replace("{fullname}", objUserInformation.FirstName);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                if (new ManageEmailRepository().CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderNotes)) == true)
                {
                    //new CommonMails().SendMail(objUserInformation.UserInfoID, "Order Notes", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);

                    String sReplyToadd = CommonMails.ReplyToOrderSummary;//"ordernotes@world-link.us.com";
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

    private void SendNotesToCustomer(Int64 OrderID, Order objOrder, Int64 AdminUserID)
    {
        try
        {
            UserInformation objUserInformation = objUserInformationRepository.GetById(objOrder.UserId);

            //if (AdminUserID != 0)
            //    objUserInformation = objUserInformationRepository.GetById(AdminUserID);

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
                messagebody.Replace("{Commentssection}", txtCustomerNote.Text.Trim());
                messagebody.Replace("{fullname}", objUserInformation.FirstName);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                if (AdminUserID != 0)
                {
                    UserInformation objAdminUserInformation = objUserInformationRepository.GetById(AdminUserID);
                    if (new ManageEmailRepository().CheckEmailAuthentication(objAdminUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderNotes)) == true)
                    {
                        String sReplyToadd = CommonMails.ReplyToOrderSummary;//"ordernotes@world-link.us.com";
                        String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+on" + objOrder.OrderID + "un" + objUserInformation.UserInfoID + "cb" + IncentexGlobal.CurrentMember.UserInfoID + "ad" + AdminUserID + "nt3en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                        //if needs to insert in the todays emails send history than just pass adminuserid instead of 0 in the below line
                        new CommonMails().SendMailWithReplyTo(0, "Order Notes", sFrmadd, objAdminUserInformation.LoginEmail, sSubject, messagebody.ToString(), sFrmname, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true, OrderID);
                    }
                }

                if (new ManageEmailRepository().CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderNotes)) == true)
                {
                    String sReplyToadd = CommonMails.ReplyToOrderSummary;//"ordernotes@world-link.us.com";
                    String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+on" + objOrder.OrderID + "un" + objUserInformation.UserInfoID + "cb" + IncentexGlobal.CurrentMember.UserInfoID + "ad" + AdminUserID + "nt3en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                    new CommonMails().SendMailWithReplyTo(objUserInformation.UserInfoID, "Order Notes", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true, OrderID);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #region !----------------Send Back Order Mail Notification -----------------------------!
    private void SendBackOrderNotificationEmail(Int64 OrderID, String OrderNumber, String ReferenceName, DateTime? OrderDate, String OrderStatus, String OrderFor, Int64? UserInfoID)
    {
        List<SelectBackOrderedItemsByOrderIDResult> lstBackOrderedItems = new OrderConfirmationRepository().SelectBackOrderedItemsByOrderID(OrderID, OrderFor);
        if (lstBackOrderedItems.Count > 0)
        {
            UserInformation objUser = new UserInformationRepository().GetById(Convert.ToInt64(UserInfoID));
            if (objUser != null)
            {
                EmailTemplateBE objEmailBE = new EmailTemplateBE();
                EmailTemplateDA objEmailDA = new EmailTemplateDA();
                DataSet dsEmailTemplate;

                //Get Email Content
                objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
                objEmailBE.STemplateName = "BackOrder";
                dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

                if (dsEmailTemplate != null)
                {
                    String sFrmadd = Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"]);
                    String sToadd = objUser.LoginEmail;
                    //String sToadd = "devraj.gadhavi@indianic.com";

                    String sSubject = Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sSubject"]) + " - Order # " + OrderNumber;
                    String sFrmname = Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sFromName"]);

                    String eMailTemplate = String.Empty;

                    StreamReader _StreamReader;
                    _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/BackOrderedUntil.htm"));
                    eMailTemplate = _StreamReader.ReadToEnd();
                    _StreamReader.Close();
                    _StreamReader.Dispose();

                    StringBuilder messagebody = new StringBuilder(eMailTemplate);
                    messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                    messagebody.Replace("{FullName}", objUser.FirstName + " " + objUser.LastName);
                    messagebody.Replace("{OrderNumber}", OrderNumber);
                    messagebody.Replace("{referencename}", ReferenceName);
                    messagebody.Replace("{OrderDate}", OrderDate != null ? Convert.ToDateTime(OrderDate).ToString("MM/dd/yyyy") : "");
                    messagebody.Replace("{OrderStatus}", OrderStatus);

                    StringBuilder innermessage = new StringBuilder();

                    foreach (SelectBackOrderedItemsByOrderIDResult BackOrderedItem in lstBackOrderedItems)
                    {
                        innermessage.Append("<tr>");
                        innermessage.Append("<td width='20%' style='font-weight: bold; text-align: center;'>");
                        innermessage.Append(BackOrderedItem.RemainingQty);
                        innermessage.Append("</td>");
                        innermessage.Append("<td width='55%' style='font-weight: bold; text-align: center;'>");
                        innermessage.Append(BackOrderedItem.ProductDescrption);
                        innermessage.Append("</td>");
                        innermessage.Append("<td width='25%' style='font-weight: bold; text-align: center;'>");
                        innermessage.Append(BackOrderedItem.BackOrderedUntil != null ? Convert.ToDateTime(BackOrderedItem.BackOrderedUntil).ToString("MM/dd/yyyy") : "");
                        innermessage.Append("</td>");
                        innermessage.Append("</tr>");
                        innermessage.Append("<tr>");
                        innermessage.Append("<td colspan='7'>");
                        innermessage.Append("<hr />");
                        innermessage.Append("</td>");
                        innermessage.Append("</tr>");
                    }

                    messagebody.Replace("{innermessage}", innermessage.ToString());

                    String smtphost = Application["SMTPHOST"].ToString();
                    Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                    String smtpUserID = Application["SMTPUSERID"].ToString();
                    String smtppassword = Application["SMTPPASSWORD"].ToString();

                    new CommonMails().SendMail(objUser.UserInfoID, "Back Order Notification", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, OrderID);
                }
            }
        }
    }
    #endregion

    #endregion

    #region Event Handlers

    protected void rpCustomer_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HtmlGenericControl divHead = new HtmlGenericControl();
            divHead = (HtmlGenericControl)e.Item.FindControl("divHead");
            if (divHead != null)
            {
                //divHead.Attributes.Add("onclick", "__doPostBack('ctl00$ContentPlaceHolder1$rpCustomer$ctl0" + e.Item.ItemIndex + "$lnkSelect','');");
                divHead.Attributes.Add("onclick", "javascript: CallFunctionWhenClick(" + e.Item.ItemIndex + ",'" + divHead.ClientID + "');");
            }
        }
    }

    protected void lnkSelect_Click(object sender, EventArgs e)
    {
        RepeaterItem rpCust = ((LinkButton)sender).Parent.Parent as RepeaterItem;
        LinkButton lnkSelect = (LinkButton)sender;
        Int64 CustomerID = Convert.ToInt64(lnkSelect.CommandArgument);
        hfTempCustomerID.Value = Convert.ToString(CustomerID);
        BindCustomerOrder(CustomerID, rpCust.ItemIndex);
    }

    protected void lnkSubmitRequest_Click(object sender, EventArgs e)
    {
        GenerateChart();
    }

    protected void lnkbtnSubmitNote_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hdnNoteType.Value) && !string.IsNullOrEmpty(hdnOrderID.Value))
        {
            Int64 OrderID = Convert.ToInt64(hdnOrderID.Value);
            String NoteType = hdnNoteType.Value;
            if (NoteType == "Vendor Message" && txtMessage.Text.Trim() != "")
            {
                //NoteHistory for Supplier ORder
                String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.SupplierOrder);

                NoteDetail objNoteDetail = new NoteDetail();
                objNoteDetail.Notecontents = txtMessage.Text.Trim();
                objNoteDetail.NoteFor = strNoteFor;
                objNoteDetail.ForeignKey = Convert.ToInt64(ddlVendor.SelectedValue);
                objNoteDetail.SpecificNoteFor = OrderID.ToString();
                objNoteDetail.CreateDate = System.DateTime.Now;
                objNoteDetail.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objNoteDetail.UpdateDate = System.DateTime.Now;
                objNoteDetail.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

                objNotesHistoryRepository.Insert(objNoteDetail);
                objNotesHistoryRepository.SubmitChanges();

                //Write code for sending email to selected supplier
                if (chkIsEmailSend.Checked)
                {
                    SendNotes(OrderID, objSupplierRepository.GetById(Convert.ToInt64(ddlVendor.SelectedValue)).UserInfoID, 1);
                    //SendNotes(OrderID, 1092, 1);
                }
            }
            else if (NoteType == "Incentex Employee Message" && txtMessage.Text.Trim() != "")
            {
                string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.OrderDetailsIEs);

                NoteDetail objNoteDetail = new NoteDetail();
                objNoteDetail.Notecontents = txtMessage.Text.Trim();
                objNoteDetail.NoteFor = strNoteFor;
                objNoteDetail.ForeignKey = OrderID;
                objNoteDetail.SpecificNoteFor = "IEInternalNotes";
                objNoteDetail.CreateDate = System.DateTime.Now;
                objNoteDetail.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objNoteDetail.UpdateDate = System.DateTime.Now;
                objNoteDetail.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

                objNotesHistoryRepository.Insert(objNoteDetail);
                objNotesHistoryRepository.SubmitChanges();

                //write code for sending email to IE based on checkmark selection
                if (chkIsEmailSend.Checked)
                {
                    foreach (DataListItem item in dtIE.Items)
                    {
                        if (((CheckBox)item.FindControl("chkUser")).Checked == true)
                        {
                            HiddenField hdnUserID = ((HiddenField)item.FindControl("hdnUserID"));
                            SendNotes(OrderID, Convert.ToInt64(hdnUserID.Value), 2);
                            //SendNotes(OrderID, 1092, 2);
                        }
                    }
                }
            }
            else if (NoteType == "Send Customer Message" && txtCustomerNote.Text.Trim() != "")
            {
                string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.CACE);
                Order objOrder = objOrderConfirmationRepository.GetByOrderID(OrderID);

                //added by Prashant on 19th nov 2012
                NoteDetail objNoteDetail = new NoteDetail();
                objNoteDetail.Notecontents = txtCustomerNote.Text.Trim();
                objNoteDetail.NoteFor = strNoteFor;
                objNoteDetail.ForeignKey = OrderID;
                objNoteDetail.SpecificNoteFor = Convert.ToString(objOrder.UserId);
                if (ddlAdminForWorkgroup.SelectedValue != "0")
                {
                    objNoteDetail.ReceivedBy = Convert.ToString(ddlAdminForWorkgroup.SelectedValue);
                }
                objNoteDetail.CreateDate = System.DateTime.Now;
                objNoteDetail.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objNoteDetail.UpdateDate = System.DateTime.Now;
                objNoteDetail.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objNotesHistoryRepository.Insert(objNoteDetail);
                objNotesHistoryRepository.SubmitChanges();
                //added by Prashant on 19th nov 2012
                if (chkIsCustomerMailSend.Checked)
                {
                    SendNotesToCustomer(OrderID, objOrder, Convert.ToInt64(ddlAdminForWorkgroup.SelectedValue));
                }

                //If admin selected insert notedetail and send mail to admin as well 
                if (ddlAdminForWorkgroup.SelectedValue != "0")
                {
                    //objNoteDetail = new NoteDetail();
                    //objNoteDetail.Notecontents = txtCustomerNote.Text.Trim();
                    //objNoteDetail.NoteFor = strNoteFor;
                    //objNoteDetail.ForeignKey = OrderID;
                    //objNoteDetail.SpecificNoteFor = Convert.ToString(ddlAdminForWorkgroup.SelectedValue);
                    //objNoteDetail.CreateDate = System.DateTime.Now;
                    //objNoteDetail.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                    //objNoteDetail.UpdateDate = System.DateTime.Now;
                    //objNoteDetail.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                    //objNotesHistoryRepository.Insert(objNoteDetail);
                    //objNotesHistoryRepository.SubmitChanges();
                    //if (chkIsCustomerMailSend.Checked)
                    //{
                    //    SendNotesToCustomer(OrderID, objOrder, Convert.ToInt64(ddlAdminForWorkgroup.SelectedValue));
                    //}
                }
                //If admin selected insert notedetail and send mail to admin as well 
                //added by Prashant on 20th nov 2012
            }
        }
        GenerateChart();
        //BindCustomerOrder(Convert.ToInt64(hfTempCustomerID.Value));
    }

    protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblEmailAddress.Text = objSupplierRepository.GetUserInformationBySupplierID(Convert.ToInt64(ddlVendor.SelectedValue)).LoginEmail;
    }

    protected void rptSummaryOrderView_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Vendor Message")
            {
                hdnNoteType.Value = "Vendor Message";
                hdnOrderID.Value = e.CommandArgument.ToString();
                Order objOrder = objOrderConfirmationRepository.GetByOrderID(Convert.ToInt64(e.CommandArgument));
                List<SelectSupplierAddressResult> objSelectSupplierAddressResult = objOrderConfirmationRepository.GetSupplierAddress(objOrder.OrderFor, objOrder.OrderID);
                ddlVendor.DataSource = objSelectSupplierAddressResult;
                ddlVendor.DataTextField = "Name";
                ddlVendor.DataValueField = "SupplierID";
                ddlVendor.DataBind();
                trVendor.Visible = true;
                trIE.Visible = false;
                trIsEmailSend.Visible = true;
                chkIsEmailSend.Checked = false;
                spnIsEmailSend.Attributes.Add("class", "custom-checkbox");
                txtMessage.Text = string.Empty;
                trEmailAddress.Visible = true;
                lblEmailAddress.Text = objSupplierRepository.GetUserInformationBySupplierID(Convert.ToInt64(ddlVendor.SelectedValue)).LoginEmail;
                mpeOrderNote.CancelControlID = "cboxClose";
                mpeOrderNote.BackgroundCssClass = "modalBackground";
                mpeOrderNote.PopupControlID = "pnlOrderNote";
                mpeOrderNote.Show();

            }
            else if (e.CommandName == "Incentex Employee Message")
            {
                hdnNoteType.Value = "Incentex Employee Message";
                hdnOrderID.Value = e.CommandArgument.ToString();
                dtIE.DataSource = new IncentexEmployeeRepository().GetAllEmployee();
                dtIE.DataBind();
                trIE.Visible = true;
                trVendor.Visible = false;
                trAdminList.Visible = false;
                trIsEmailSend.Visible = true;
                txtMessage.Text = string.Empty;
                chkIsEmailSend.Checked = false;
                spnIsEmailSend.Attributes.Add("class", "custom-checkbox");
                trEmailAddress.Visible = false;
                lblEmailAddress.Text = string.Empty;
                mpeOrderNote.CancelControlID = "cboxClose";
                mpeOrderNote.BackgroundCssClass = "modalBackground";
                mpeOrderNote.PopupControlID = "pnlOrderNote";
                mpeOrderNote.Show();
            }
            else if (e.CommandName == "Send Customer Message")
            {
                //added by prashant dt-17-11-2012
                HiddenField hfWorkGroupID = new HiddenField();
                hfWorkGroupID = (HiddenField)e.Item.FindControl("hfWorkGroupID");
                HiddenField hfCompanyID = new HiddenField();
                hfCompanyID = (HiddenField)e.Item.FindControl("hfCompanyID");
                if (hfWorkGroupID != null && hfCompanyID != null)
                {
                    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
                    List<UserInformation> objAdminlist = objCompanyEmployeeRepository.GetCAByWorkgroupId(Convert.ToInt64(hfWorkGroupID.Value), Convert.ToInt64(hfCompanyID.Value)).ToList();
                    var UserResult = (from result in objAdminlist
                                      select new
                                      {
                                          Name = result.FirstName + " " + result.LastName,
                                          UserInfoID = result.UserInfoID
                                      }).ToList();
                    ddlAdminForWorkgroup.DataSource = UserResult;
                    ddlAdminForWorkgroup.DataTextField = "Name";
                    ddlAdminForWorkgroup.DataValueField = "UserInfoID";
                    ddlAdminForWorkgroup.DataBind();
                    ddlAdminForWorkgroup.Items.Insert(0, "-Select Admin-");
                    ddlAdminForWorkgroup.Items[0].Value = "0";
                }

                hdnNoteType.Value = "Send Customer Message";
                hdnOrderID.Value = e.CommandArgument.ToString();
                trAdminList.Visible = true;
                trVendor.Visible = false;
                trIE.Visible = false;
                trIsEmailSend.Visible = false;
                txtMessage.Text = string.Empty;
                chkIsEmailSend.Checked = false;
                spnIsEmailSend.Attributes.Add("class", "custom-checkbox");
                trEmailAddress.Visible = true;
                lblCustomerEmailAddress.Text = objOrderConfirmationRepository.GetUserInformationByOrderID(Convert.ToInt64(hdnOrderID.Value)).LoginEmail;
                mpeOrderNote.CancelControlID = "credboxClose";
                mpeOrderNote.PopupControlID = "pnlCustomerMessage";
                mpeOrderNote.Show();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), System.DateTime.Now.ToString(), "javascript:SetDropBackGround()", true);
            }
            else if (e.CommandName == "OrderDetails")
            {
                GridView gvBackOrderManagement = (GridView)e.Item.FindControl("gvBackOrderManagement");
                HiddenField hdnSupplierId = (HiddenField)e.Item.FindControl("hdnSupplierId");
                OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();

                Int64? MyShoppingCartID = null;
                ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
                Boolean IsBackOrderedChanged = false;
                hdnOrderID.Value = e.CommandArgument.ToString();

                Order objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(hdnOrderID.Value));

                foreach (GridViewRow t in gvBackOrderManagement.Rows)
                {
                    if (!base.CanEdit)
                    {
                        base.RedirectToUnauthorised();
                    }

                    Label lblItemNumber = (Label)t.FindControl("lblBackOrderItemNumber");
                    TextBox txtBackOrderDate = (TextBox)t.FindControl("txtBackOrderDate");

                    HiddenField hdnBackOrderedUntil = (HiddenField)t.FindControl("hdnBackOrderedUntil");
                    HiddenField hdnMyShoppingCartiD = (HiddenField)t.FindControl("hdnMyShoppingCartiD");

                    if (Convert.ToInt64(hdnMyShoppingCartiD.Value) > 0)
                        MyShoppingCartID = Convert.ToInt64(hdnMyShoppingCartiD.Value);

                    if (!String.IsNullOrEmpty(txtBackOrderDate.Text.Trim()) && txtBackOrderDate.Text.Trim() != Convert.ToString(hdnBackOrderedUntil.Value))
                    {
                        if (objOrder.OrderFor == "ShoppingCart")
                        {
                            MyShoppingCartRepository objCartRepo = new MyShoppingCartRepository();
                            MyShoppinCart objCart = objCartRepo.GetById(Convert.ToInt32(MyShoppingCartID), Convert.ToInt64(objOrder.UserId));
                            if (objCart != null)
                            {
                                objCart.BackOrderedUntil = Convert.ToDateTime(txtBackOrderDate.Text.Trim());
                                objCartRepo.SubmitChanges();
                            }
                        }
                        else
                        {
                            MyIssuanceCartRepository objCartRepo = new MyIssuanceCartRepository();
                            MyIssuanceCart objCart = objCartRepo.GetById(Convert.ToInt32(MyShoppingCartID), Convert.ToInt64(objOrder.UserId));
                            if (objCart != null)
                            {
                                objCart.BackOrderedUntil = Convert.ToDateTime(txtBackOrderDate.Text.Trim());
                                objCartRepo.SubmitChanges();
                            }
                        }

                        IsBackOrderedChanged = true;
                    }
                }

                if (IsBackOrderedChanged)
                    SendBackOrderNotificationEmail(objOrder.OrderID, objOrder.OrderNumber, objOrder.ReferenceName, objOrder.OrderDate, objOrder.OrderStatus, objOrder.OrderFor, objOrder.UserId);

                GenerateChart();
                RepeaterItem rpMainRepeaterItem = ((Repeater)source).Parent.Parent as RepeaterItem;
                BindCustomerOrder(Convert.ToInt64(hfTempCustomerID.Value), rpMainRepeaterItem.ItemIndex);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }


    #endregion
}
