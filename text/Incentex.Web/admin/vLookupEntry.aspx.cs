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
using Incentex.BE;
using Incentex.DA;
using System.Data.SqlClient;
using System.IO;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Data.Linq.SqlClient;
using System.Net.Mail;
using System.Net.Sockets;
using System.Net.Security;
using System.Xml;


public partial class admin_vLookupEntry : PageBase
{
    #region Variables and Properties
    /// <summary>
    ///  Object of Datacontext Class IncentexBE
    /// </summary>
    protected IncentexBEDataContext db = new IncentexBEDataContext();
    /// <summary>
    ///  Object of LookupRepository class
    /// </summary>
    LookupRepository objLookuprep = new LookupRepository();

    /// <summary>
    ///  To store lookupid in session for the delete and update operations
    /// </summary>
    public String lookupId
    {
        get
        {
            if ((HttpContext.Current.Session["lookupId"]) == null)
                return null;
            else
                return (String)HttpContext.Current.Session["lookupId"];
        }
        set 
        { 
            HttpContext.Current.Session["lookupId"] = value; 
        }
    }
    #endregion 

    #region Page Events
    /// <summary>
    /// Page Load Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Manage Lookup Informations";
            SetTreeviewNode();
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnSave.Visible = true;
            dvErrorMsg.Attributes.Add("style", "display: none; visibility: collapse;");
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
        }
    }


    /// <summary>
    /// To save records in INC_Lookup Table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        String saveImagePath = String.Empty;
        #region Upload Image and Set path
        try
        {
            if (!String.IsNullOrEmpty(FileUploadIcon.FileName))
            {
                String filePath = "~/admin/Incentex_Used_Icons/";
                HttpPostedFile Img = FileUploadIcon.PostedFile;
                String FileName = FileUploadIcon.FileName;
                String Extension = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName).Trim();
                saveImagePath = filePath + FileName + Extension;

                if (!String.IsNullOrEmpty(FileName))
                {
                    Img.SaveAs(Server.MapPath(saveImagePath));
                }
            }

        }
        catch (Exception ex)
        {

        }
        #endregion
        try
        {
            db.AddLookupRecords(null, txtLookupCat.Text, txtLookupSubCat.Text, FileUploadIcon.FileName, txtValue.Text);
            lblErrorMsg.Text = String.Format("{0} is added successfully", txtLookupSubCat.Text);
            dvErrorMsg.Attributes.Add("style", "display: block; visibility: visible;");
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = "Error occur while processing... :" + ex.ToString();
            dvErrorMsg.Attributes.Add("style", "display: block; visibility: visible;");
        }

    }

    protected void btnsendMail_Click(object sender, EventArgs e)
    {
        try
        {
            var fromAddress = "surendar.yadav@indianic.com";// your email address.
            // any address where the email will be sending
            var toAddress = "surendar.yadav@indianic.com";// To whom you want to send ur email
            //Password of your gmail address
            string fromPassword = "Surendar!2345";// ur email password
            // Passing the values and make a email formate to display
            string subject = "Test";
            string body = "From:   Surendar  <br/><br/>";
            body += "You are receiving this email because you filled out a form on our site indicating that you had forgotten your something";
            body += "You can follow using following link : <br/><br/>";

            //Set Conformation Button
            string buttonText = "<br/><br/>";
            buttonText += "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
            buttonText += "<tr>";
            buttonText += "<td>";
            buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "admin/CommunicationCenter/DelMyShoppingCart.aspx?DateTimeSpan=" + DateTime.Now + "&UserinfoID=" + 1092 + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/dlt_items_btn.png' alt='Delete items' border='0'/></a>";
            buttonText += "<td>";
            buttonText += "</tr>";
            buttonText += "</table>";
            body += buttonText;

            body += "<br/><br/> This link can be used for only once";
            body += "<br/><br/> This link will be expired after 1 hours";
            body += "<br/><br/> Thanks & Regards";

            SendEmail(fromAddress, fromPassword, toAddress, subject, body, true);
        }
        catch (Exception ex)
        {
            SetIsErrorOccur(true);
            ErrHandler.WriteError(ex);
        }

    }

    public static void SendEmail(String FromAddress, String Password, String ToAddress, string subject, string message, bool isHtml)
    {
       var mail = new MailMessage();
       var mailclient = new SmtpClient();
        

            //
            // Set the to and from addresses.
            // The from address must be your GMail account
            mail.From = new MailAddress(FromAddress);
            mail.To.Add(new MailAddress(ToAddress));
            mail.Headers.Add("Disposition-Notification-To", "<surendar.yadav@indianic.com>");
            mail.ReplyTo = new MailAddress("<test_te+surendar.yadav@indianic.com>");
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            mailclient.DeliveryMethod = SmtpDeliveryMethod.Network;

            mail.Headers.Add("Return-Path", "<surendar.yadav@indianic.com>");
            mail.Headers.Add("Disposition-Notification-To", "<skysurendar@gmail.com>");
            mail.Sender = new MailAddress("<surendar.yadav@indianic.com>", "TL");
            // Define the message
            mail.Subject = subject;
            mail.IsBodyHtml = isHtml;
            mail.Body = message;

            // Create a new Smpt Client using Google's servers
            // var mailclient = new SmtpClient();
            mailclient.Host = "smtp.gmail.com";//ForGmail
            mailclient.Port = 25; //ForGmail

            mailclient.EnableSsl = true;//ForGmail
            //mailclient.EnableSsl = false;
            mailclient.UseDefaultCredentials = true;

            //Attachment objAttach = new Attachment(HttpContext.Current.Server.MapPath("~/UploadedImages/Artwork/Thumbs/Artwork_20121001170607449_Penguins.jpg"));
            //System.Net.Mime.ContentDisposition obj = objAttach.ContentDisposition;
            //obj.FileName = "Artwork_20121001170607449_Penguins.jpg";
            //mail.Attachments.Add(objAttach);
            byte[] data = System.Text.ASCIIEncoding.Default.GetBytes("Surendar Yadav");

            MemoryStream ms = new MemoryStream(data);//File.ReadAllBytes(@"D:\Koala.jpg"));
            mail.Attachments.Add(new System.Net.Mail.Attachment(ms,"myFile.csv", "text/csv"));//, "Koala.jpg"));
            mailclient.Credentials = new System.Net.NetworkCredential(FromAddress, Password);//ForGmail

            try
            {
                //IMail em = mail.s;
                mailclient.Send(mail);
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
                    {
                        // Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        mailclient.Send(mail);
                    }
                    else
                    {
                        //  Console.WriteLine("Failed to deliver message to {0}", ex.InnerExceptions[i].FailedRecipient);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                //  Console.WriteLine("Exception caught in RetryIfBusy(): {0}",ex.ToString());
                ErrHandler.WriteError(ex);
            }
        

    }
    /// <summary>
    /// To Update records in INC_Lookup Table with Id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            db.UpdateLookupRecords(Convert.ToInt32(lookupId), txtLookupCat.Text, txtLookupSubCat.Text, FileUploadIcon.FileName, txtValue.Text);
            lblErrorMsg.Text = String.Format("{0} is updated successfully", txtLookupSubCat.Text);
            dvErrorMsg.Attributes.Add("style", "display: block; visibility: visible;");
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = "Error occur while updating records... :" + ex.ToString();
            dvErrorMsg.Attributes.Add("style", "display: block; visibility: visible;");
        }

    }
    /// <summary>
    /// Tree view Node checked event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void TreeViewMain_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {

    }
    /// <summary>
    /// Get the selected node of treeview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void TreeViewMain_SelectedNodeChanged(object sender, EventArgs e)
    {
        lookupId = TreeViewMain.SelectedValue;
        if (lookupId != null && lookupId.Length > 0)
        {
            txtLookupCat.ReadOnly = true;

            // if Parent node is selected
            if (IsparentSelected())
            {
                txtLookupCat.Text = TreeViewMain.SelectedValue;
                btnSave.Visible = true;
                imgIcon.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                txtLookupSubCat.Text = "";

            }// if childnode is selected then
            else
            {
                // Set Button update and Delete enable
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
                btnSave.Visible = false;
                var ilookdata = db.GetSLookupname().Where(id => id.iLookupID == Convert.ToInt32(lookupId)).ToList();
                foreach (var ilk in ilookdata)
                {
                    txtLookupCat.Text = ilk.iLookupCode.ToString();
                    txtLookupSubCat.Text = ilk.sLookupName.ToString();
                    if (ilk.Val1 != null && ilk.Val1.Length > 0)
                        txtValue.Text = ilk.Val1.ToString();
                    if (ilk.sLookupIcon != null && ilk.sLookupIcon.Length > 0)
                    {
                        imgIcon.Visible = true;
                        imgIcon.ImageUrl = "~/admin/Incentex_Used_Icons/" + ilk.sLookupIcon.ToString();
                    }
                    else
                    {
                        imgIcon.Visible = false;
                    }
                }
            }
        }
    }
    /// <summary>
    /// To cancel and reset the textbox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        lookupId = "";
        txtLookupCat.Text = "";
        txtLookupCat.ReadOnly = false;
        txtLookupSubCat.Text = "";
        //FileUploadIcon.Value = "";
        btnUpdate.Visible = false;
        btnDelete.Visible = false;
        btnSave.Visible = true;
        imgIcon.Visible = false;
        dvErrorMsg.Attributes.Add("style", "display: none; visibility: collapse;");
    }
    /// <summary>
    /// To delete the reocrds with Id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            db.DeleteLookupRecords(Convert.ToInt32(lookupId));
        }
        catch (SqlException sqlex)
        {
            lblErrorMsg.Text = "Error occur while deleting... :" + sqlex.ToString();
            dvErrorMsg.Attributes.Add("style", "display: block; visibility: visible;");
        }

    }

    #endregion 

    #region Funtions
    /// <summary>
    ///  Set Treeview Node 
    /// </summary>
    private void SetTreeviewNode()
    {
        TreeViewMain.Nodes.Clear();
        var lookdata = db.GetLookupCode().ToList();
        foreach (var c in lookdata)
        {
            TreeNode tnParent = new TreeNode();
            tnParent.Text = c.iLookupCode;
            tnParent.Value = c.iLookupCode;
            tnParent.PopulateOnDemand = false;
            tnParent.SelectAction = TreeNodeSelectAction.Select;
            tnParent.Expanded = false;
            tnParent.Selected = true;
            TreeViewMain.Nodes.Add(tnParent);
            FillChildNode(tnParent, tnParent.Value);
        }
    }

    /// <summary>
    ///  To set the child node of treeview 
    /// </summary>
    /// <param name="parent">parent node of treeview </param>
    /// <param name="parentValue"> parent node value</param>
    private void FillChildNode(TreeNode parent, string parentValue)
    {
        parent.ChildNodes.Clear();
        var lookdata = db.GetSLookupname().Where(slookname => slookname.iLookupCode == parentValue).ToList();
        foreach (var slookupcode in lookdata.ToList())
        {
            TreeNode child = new TreeNode();
            child.Text = slookupcode.sLookupName;
            child.Value = slookupcode.iLookupID.ToString();
            if (child.ChildNodes.Count == 0)
            {
                child.PopulateOnDemand = false;
            }
            child.SelectAction = TreeNodeSelectAction.SelectExpand;
            child.Expanded = false;
            child.Selected = true;
            child.ShowCheckBox = false;
            parent.ChildNodes.Add(child);
        }

    }
    /// <summary>
    /// To check whether parent is selected
    /// </summary>
    /// <returns></returns>
    private Boolean IsparentSelected()
    {
        Boolean _isparentSelected = false;
        if (TreeViewMain.SelectedNode.Parent != null)
            _isparentSelected = false;
        else
            _isparentSelected = true;

        return _isparentSelected;
    }
    /// <summary>
    /// To check Whether sLookupName Already Exist
    /// </summary>
    /// <returns></returns>
    private Boolean IsLookupNameAlreadyExist()
    {
        Boolean _isLookupNameAlreadyExist = false;
        var duplicate = db.GetSLookupname().Where(r => r.sLookupName == txtLookupSubCat.Text).ToList();
        if (duplicate != null && duplicate.Count > 0)
            _isLookupNameAlreadyExist = true;
        else
            _isLookupNameAlreadyExist = false;

        return _isLookupNameAlreadyExist;
    }
    #endregion 
   
}
