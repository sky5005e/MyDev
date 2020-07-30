using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL.Common;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text;
public partial class AssetManagement_PendingInvoice :PageBase
{
    #region DataMembers
    Int64 CompanyID
    {
        get
        {
            if (ViewState["CompanyID"] == null)
            {
                ViewState["CompanyID"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyID"]);
        }
        set
        {
            ViewState["CompanyID"] = value;
        }
    }

    
    AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
    CompanyEmployee objCmpnyInfo = new CompanyEmployee();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    UserInformation objuser = new UserInformation();
    UserInformationRepository objuserrepos = new UserInformationRepository();  
    PagedDataSource pds = new PagedDataSource();
    EquipmentMaintenanceCostDetail objEquipMaintenanceCostDtl = new EquipmentMaintenanceCostDetail();
    DataSet ds = new DataSet();
    AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
    #endregion
    #region Events
    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                MasterPageFile = "~/admin/MasterPageAdmin.master";
            }
            else
            {
                MasterPageFile = "~/MasterPage.master";
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                base.MenuItem = "Pending Invoices";
                base.ParentMenuID = 46;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Pending Invoices";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Index.aspx";
                }
                else
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/AssetMgtIndex.aspx";
                }

                             

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    CompanyID = Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId);
                }
               
                bindgrid();
            }
        }
        catch (Exception)
        {


        }
    }
    //protected void btnAddEquipment_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("AddEquipment.aspx");
    //}


    /// <summary>
    /// Row Command Event of Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEquipment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {


            if (e.CommandName.Equals("Sort"))
            {
                if (this.ViewState["SortExp"] == null)
                {
                    this.ViewState["SortExp"] = e.CommandArgument.ToString();
                    this.ViewState["SortOrder"] = "ASC";
                }
                else
                {
                    if (this.ViewState["SortExp"].ToString() == e.CommandArgument.ToString())
                    {
                        if (this.ViewState["SortOrder"].ToString() == "ASC")
                            this.ViewState["SortOrder"] = "DESC";
                        else
                            this.ViewState["SortOrder"] = "ASC";

                    }
                    else
                    {
                        this.ViewState["SortOrder"] = "ASC";
                        this.ViewState["SortExp"] = e.CommandArgument.ToString();
                    }
                }
            }

            if (e.CommandName == "PDF")
            {
                Int64 val = Convert.ToInt64(e.CommandArgument);
                objEquipMaintenanceCostDtl = objAssetMgtRepository.GetByEquipMaintenanceCostId(val);
                if (objEquipMaintenanceCostDtl.DocumentPath != null)
                {

                    string sFilePath = Server.MapPath("../UploadedImages/EquipmentInvoice/");
                    //DownloadFile(sFilePath, objEquipmentMaster.InsurancePolicy);

                    string strFullPath = sFilePath + objEquipMaintenanceCostDtl.DocumentPath;
                    DownloadFile(strFullPath);
                }
                else
                {
                    lblmsg.Text = "No Document Found..";
                }
            }
            bindgrid();
        }
        catch (Exception)
        {

        }
    }
    /// <summary>
    /// Row Databound event of a grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEquipment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
        try
        {


            if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
            {
                Image ImgSort = new Image();
                switch (this.ViewState["SortExp"].ToString())
                {
                    case "EquipmentVendorName":
                        PlaceHolder placeholderEquipmentVendorName = (PlaceHolder)e.Row.FindControl("placeholderEquipmentVendorName");
                        break;
                    case "Invoice":
                        PlaceHolder placeholderInvoice = (PlaceHolder)e.Row.FindControl("placeholderInvoice");
                        break;
                    case "EquipmentID":
                        PlaceHolder placeholderEquipmentID = (PlaceHolder)e.Row.FindControl("placeholderEquipmentID");
                        break;
                    case "AssetType":
                        PlaceHolder placeholderAssetType = (PlaceHolder)e.Row.FindControl("placeholderAssetType");
                        break;
                   
                }
               
            }




            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                //Change PDF & XL Icon Conditionally
                string FileName = string.Empty;
                string FileExt = string.Empty;
                ImageButton lnkbtnPDF = (ImageButton)e.Row.FindControl("lnkbtnPDF");
                Label lblEquipmentMaintinanceID = (Label)e.Row.FindControl("lblEquipmentMaintenanceCostID");
                Int64 val = lblEquipmentMaintinanceID.Text != null ? Convert.ToInt64(lblEquipmentMaintinanceID.Text) : 0;
                objEquipMaintenanceCostDtl = objAssetMgtRepository.GetByEquipMaintenanceCostId(val);
                if (objEquipMaintenanceCostDtl.DocumentPath != null)
                {
                    lnkbtnPDF.Enabled = true;
                    FileName = objEquipMaintenanceCostDtl.DocumentPath;
                    string[] parts = FileName.Split('.');
                    FileExt = parts[1];
                    if (FileExt == "pdf")
                    {
                        lnkbtnPDF.ImageUrl = string.Format("~/Images/pdf.png");
                    }
                    else if (FileExt == "xl" || FileExt == "xls")
                    {
                        lnkbtnPDF.ImageUrl = string.Format("~/Images/excel_small.png");
                    }
                    else
                    {
                        lnkbtnPDF.ImageUrl = string.Format("~/Images/Document.png");
                    }

                }
                else
                {
                    lnkbtnPDF.ImageUrl = string.Format("~/Images/spacer.gif");
                    lnkbtnPDF.Enabled = false;
                }

            }
      
        }
        catch (Exception)
        {


        }
    }

    /// <summary>
    /// Approve button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnApprove_Click(object sender, EventArgs e)
    {        
        try
        {

            List<GetGSEUsersResult> lstRecipients = new List<GetGSEUsersResult>();
            lstRecipients = new AssetMgtRepository().GetGSEUsers().ToList();
            String Mail = string.Empty;
            foreach (GetGSEUsersResult recipient in lstRecipients)
            {
                //Email Management
                if (objAssetVendorRepository.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.ApprovedInvoice)) == true)                
                {
                    if (Mail == string.Empty)
                    {
                        Mail = Convert.ToString(recipient.LoginEmail);
                    }
                    else
                    {
                        Mail = Mail + "," + Convert.ToString(recipient.LoginEmail);
                    }                    
                }
            }
            if (Mail==string.Empty)
            {
                lblInvoiceMail.Text = "You are about to approve and send these invoices for payment.Do you wish to proceed.";
            }
            else
            {
                lblInvoiceMail.Text = "You are about to approve and send these invoices to-</br>" + Mail + "</br> for payment.Do you wish to proceed.";
            }
            
            modal.Show();           

        }
        catch (Exception ex)
        {
            
        }

    }


    /// <summary>
    /// Delete button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();       
        try
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            string NotDeleted = "";

            bool IsAnySelected = false;
            foreach (GridViewRow gr in gvEquipment.Rows)
            {
                CheckBox chkDelete = gr.FindControl("chkDelete") as CheckBox;
                Label lblID = gr.FindControl("lblEquipmentMaintenanceCostID") as Label;

                if (chkDelete.Checked)
                {

                    EquipmentMaintenanceCostDetail objEquipmentMaintenanceCostDetail = new EquipmentMaintenanceCostDetail();
                    objEquipmentMaintenanceCostDetail = objAssetMgtRepository.GetByEquipMaintenanceCostId(Convert.ToInt64(lblID.Text));
                    objEquipmentMaintenanceCostDetail.IsDeleted = true;
                    IsAnySelected = true;                    
                }
            }

            if (IsAnySelected)
            {
                if (string.IsNullOrEmpty(NotDeleted))
                {
                    lblmsg.Text = "Selected Records Deleted Successfully ...";
                }

            }
            else
            {
                lblmsg.Text = "Please Select Record to Delete ...";
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = "Error in Deleting record ...";
            ErrHandler.WriteError(ex);
        }        
        objAssetMgtRepository.SubmitChanges();
        bindgrid();
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        modal.Hide();
        AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
        string invoice = string.Empty;
        string doc = string.Empty;
        try
        {
            string NotDeleted = "";
            bool IsAnySelected = false;        
           
                foreach (GridViewRow gr in gvEquipment.Rows)
                {
                    CheckBox chkDelete = gr.FindControl("chkDelete") as CheckBox;
                    Label lblID = gr.FindControl("lblEquipmentMaintenanceCostID") as Label;

                    if (chkDelete.Checked)
                    {

                        EquipmentMaintenanceCostDetail objEquipmentMaintenanceCostDetail = new EquipmentMaintenanceCostDetail();
                        objEquipmentMaintenanceCostDetail = objAssetMgtRepository.GetByEquipMaintenanceCostId(Convert.ToInt64(lblID.Text));
                        objEquipmentMaintenanceCostDetail.IsApproved = true;
                        IsAnySelected = true;
                        EditNotes("Maintenance Related Cost", objEquipmentMaintenanceCostDetail.Invoice, objEquipmentMaintenanceCostDetail.EquipmentMasterID);
                        if (invoice == string.Empty)
                        {
                            invoice = objEquipmentMaintenanceCostDetail.Invoice;
                            doc = objEquipmentMaintenanceCostDetail.DocumentPath;
                        }
                        else
                        {
                            invoice = invoice + "," + objEquipmentMaintenanceCostDetail.Invoice;
                            doc = doc + "," + objEquipmentMaintenanceCostDetail.DocumentPath;
                        }
                    }
                }

                if (IsAnySelected)
                {
                    if (string.IsNullOrEmpty(NotDeleted))
                    {
                        lblmsg.Text = "Selected Records Approved Successfully ...";
                        SendEmailNotes(invoice, doc, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.ApprovedInvoice));
                    }

                }
                else
                {
                    lblmsg.Text = "Please Select Record to Approve ...";
                }
            

        }
        catch (Exception ex)
        {
            lblmsg.Text = "Error in Approving record ...";
            ErrHandler.WriteError(ex);
        }

        objAssetMgtRepository.SubmitChanges();
        bindgrid();
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
        modal.Hide();
    }
    #endregion
    #region Methods
    public void bindgrid()
    {
        DataView myDataView = new DataView();

        DataTable dt = new DataTable();
        AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
        UserInformation objuser = new UserInformation();
        UserInformationRepository objuserrepos = new UserInformationRepository();
        try
        {
             bool IsAuthentic = objAssetVendorRepository.IsAuthentic(IncentexGlobal.CurrentMember.UserInfoID);
             if (IsAuthentic)
             {
                 dvBottom.Visible = true;
                 dt = ListToDataTable(objAssetMgtRepository.GetEquipPendingInvoiceByCmpID(this.CompanyID));
                 if (dt.Rows.Count == 0)
                 {
                     pagingtable.Visible = false;
                     btnApprove.Visible = false;
                 }
                 else
                 {
                     pagingtable.Visible = true;
                     btnApprove.Visible = true;
                 }
                 myDataView = dt.DefaultView;
                 if (this.ViewState["SortExp"] != null)
                 {
                     myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
                 }
                 pds.DataSource = myDataView;
                 pds.AllowPaging = true;
                 pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
                 pds.CurrentPageIndex = CurrentPage;
                 lnkbtnNext.Enabled = !pds.IsLastPage;
                 lnkbtnPrevious.Enabled = !pds.IsFirstPage;
                 gvEquipment.DataSource = pds;
                 gvEquipment.DataBind();
                 doPaging();
             }
            else
                 dvBottom.Visible = false;
        }
        catch (Exception)
        {


        }
    }
    /// <summary>
    /// Function which converts List to the DataTable
    /// </summary>
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            //table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            dt.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }
        return dt;
    }
    protected void DownloadFile(string filepath)
    {
        System.IO.Stream iStream = null;
        string type = "";
        string ext = Path.GetExtension(filepath);


        // Buffer to read 10K bytes in chunk:
        byte[] buffer = new Byte[10000];

        // Length of the file:
        int length;

        // Total bytes to read:
        long dataToRead;

        // Identify the file name.
        string filename = System.IO.Path.GetFileName(filepath);

        try
        {
            // Open the file.
            iStream = new System.IO.FileStream(filepath, System.IO.FileMode.Open,
            System.IO.FileAccess.Read, System.IO.FileShare.Read);


            // Total bytes to read:
            dataToRead = iStream.Length;
            if (ext != null)
            {
                switch (ext.ToLower())
                {
                    case ".htm":
                    case ".html":
                        type = "text/HTML";
                        break;
                    case ".txt":
                        type = "text/plain";
                        break;
                    case ".pdf":
                        type = "Application/pdf";
                        break;
                    case ".mp3":
                        type = "audio/mpeg";
                        break;
                    case ".wmi":
                        type = "audio/basic";
                        break;
                    case ".dat":
                    case ".mpeg":
                    case "mpg":
                        type = "video/mpeg";
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".gif":
                    case ".tiff":
                        type = "image/gif";
                        break;
                    case ".png":
                        type = "image/png";
                        break;
                    case ".doc":
                    case ".rtf":
                        type = "Application/msword";
                        break;
                    case ".wav":
                        type = "audio/x-wav";
                        break;
                    case "bmp":
                        type = "image/bmp";
                        break;
                    case "flv":
                        type = "video/x-flv";
                        break;
                    case ".docx":
                        type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case "ppt":
                        type = "application/vnd.ms-powerpoint";
                        break;
                    case "pptx":
                        type = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        break;
                    case "avi":
                        type = "video/x-msvideo";
                        break;
                    case "wmv":
                        type = "video/x-ms-wmv";
                        break;
                    case "wma":
                        type = "/audio/x-ms-wma";
                        break;

                    //left:--rm
                }
            }


            //Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + displayFileName + "\"");
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.ContentType = type;
            Response.WriteFile(filepath);
            Response.End();


        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (iStream != null)
            {
                //Close the file.
                iStream.Close();
            }
        }
    }
    public void EditNotes(string NoteContent, string Invoice, Int64 EquipmentMasterId)
    {
        try
        {
           
            string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement);

            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();


            objComNot.Notecontents = NoteContent + " " + Invoice;
            objComNot.NoteFor = strNoteFor;
            objComNot.ForeignKey = Convert.ToInt64(EquipmentMasterId);
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objComNot.UpdateDate = System.DateTime.Now;
            objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objCompNoteHistRepos.Insert(objComNot);
            objCompNoteHistRepos.SubmitChanges();
            int NoteId = (int)(objComNot.NoteID);

        }
        catch (Exception)
        {

        }
    }
    private void SendEmailNotes(string invoice,string doc, Int64 EmailFor)
    {
       
        if (EmailFor > 0)
        {
            
             
            String eMailTemplate = String.Empty;
            String sSubject = "Approved Invoices";
            
            // String sReplyToadd = Common.ReplyToGSE;
            String sReplyToadd = "replytoGSE@world-link.us.com";

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/GSEAssetMgtMail.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();


            List<GetGSEUsersResult> lstRecipients = new List<GetGSEUsersResult>();
            lstRecipients = new AssetMgtRepository().GetGSEUsers().ToList();
            foreach (GetGSEUsersResult recipient in lstRecipients)
            {
                //Email Management
                if (objAssetVendorRepository.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), EmailFor) == true)
                //if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                {
                    String UserName = Convert.ToString(recipient.LastName) + " " + Convert.ToString(recipient.FirstName);
                    StringBuilder MessageBody = new StringBuilder(eMailTemplate);
                    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                    MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
                    MessageBody.Replace("{EquipmentID}", invoice);
                    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                    MessageBody.Replace("{Note}", "<br/>" + "The following invoices have been approved for payment- " + invoice);
                    MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);

                    //using (MailMessage objEmail = new MailMessage())
                    //{
                    //    objEmail.Body = MessageBody.ToString();
                    //    objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                    //    objEmail.IsBodyHtml = true;
                    //    //objEmail.ReplyTo = new MailAddress(sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(this.EquipmentMasterId) + "un" + Convert.ToString(recipient.UserInfoID) + "nt" + Convert.ToString(EmailFor) + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@')));
                    //    objEmail.Subject = sSubject;
                    //    objEmail.To.Add(new MailAddress(Convert.ToString(recipient.LoginEmail)));

                        
                    //    string[] Attach = doc.Split(',');
                    //    foreach (string i in Attach)
                    //    {
                    //        string FP = Server.MapPath("../UploadedImages/EquipmentInvoice/") + i;
                    //        Attachment objAttach = new Attachment(FP);
                    //        objEmail.Attachments.Add(objAttach);
                    //    }
                       
                    //    SmtpClient objSmtp = new SmtpClient();

                    //    objSmtp.EnableSsl = Common.SSL;
                    //    objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
                    //    objSmtp.Host = Common.SMTPHost;
                    //    objSmtp.Port = Common.SMTPPort;

                    //    objSmtp.Send(objEmail);
                    //}
                    string smtpUserID = Application["SMTPUSERID"].ToString();
                    string[] Attach = doc.Split(',');
                    string FP=string.Empty;
                    foreach (string i in Attach)
                    {
                         FP = Server.MapPath("../UploadedImages/EquipmentInvoice/") + i;
                         new CommonMails().SendMailWithAttachment(Convert.ToInt64(recipient.UserInfoID), null, Common.EmailFrom, Convert.ToString(recipient.LoginEmail), sSubject, MessageBody.ToString(), Common.DisplyName, false, true, FP, Common.SMTPHost, Common.SMTPPort, smtpUserID, "smtppassword", i, true);
                    }

                    
                    //new CommonMails().SendMail(Convert.ToInt64(recipient.UserInfoID), null, Common.EmailFrom, Convert.ToString(recipient.LoginEmail), sSubject, MessageBody.ToString(), Common.DisplyName, Common.SMTPHost, Common.SMTPPort, false, true);
                }
            }
        }
    }
    #endregion
    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindgrid();
        }


    }
    protected void dtlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
    public int CurrentPage
    {
        get
        {
            if (this.ViewState["CurrentPage"] == null)
                return 0;
            else
                return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }

    public int FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt16(this.ViewState["FrmPg"].ToString());
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }
    public int ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return 5;
            else
                return Convert.ToInt16(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }
    private void doPaging()
    {
        try
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            int CurrentPg = pds.CurrentPageIndex + 1;

            if (CurrentPg > ToPg)
            {
                FrmPg = ToPg + 1;
                ToPg = ToPg + 5;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - 5;

            }

            if (pds.PageCount < ToPg)
                ToPg = pds.PageCount;

            for (int i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();

        }
        catch (Exception)
        { }

    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        bindgrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        bindgrid();
    }
    #endregion
}
