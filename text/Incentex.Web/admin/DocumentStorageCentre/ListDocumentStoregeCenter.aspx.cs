using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Configuration;
using System.Data;
using System.Text;
using System.IO;

public partial class admin_DocumentStoregeCentre_ListDocumentStoregeCenter : PageBase
{
    #region Data Member's
    String FileName
    {
        get
        {
            if (ViewState["FileName"] != null)
                return ViewState["FileName"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["FileName"] = value;
        }
    }
    String Searchkeyword
    {
        get
        {
            if (ViewState["Searchkeyword"] != null)
                return ViewState["Searchkeyword"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["Searchkeyword"] = value;
        }
    }
    String UplodedBy
    {
        get
        {
            if (ViewState["UplodedBy"] != null)
                return ViewState["UplodedBy"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["UplodedBy"] = value;
        }
    }
    long? DocumentType
    {
        get
        {
            if (ViewState["DocumentType"] != null)
                return Convert.ToInt64(ViewState["DocumentType"].ToString());
            else
                return null;
        }
        set
        {
            ViewState["DocumentType"] = value;
        }
    }
    string hdnfilename
    {
        get
        {
            if (ViewState["hdnfilename"] != null)
                return ViewState["hdnfilename"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["hdnfilename"] = value;
        }
    }
    string hdnOriginalFileName
    {
        get
        {
            if (ViewState["hdnOriginalFileName"] != null)
                return ViewState["hdnOriginalFileName"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["hdnOriginalFileName"] = value;
        }
    }
    string hdnpassword
    {
        get
        {
            if (ViewState["hdnpassword"] != null)
                return ViewState["hdnpassword"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["hdnpassword"] = value;
        }
    }

    PagedDataSource pds = new PagedDataSource();
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
    public int PagerSize
    {
        get
        {
            if (this.ViewState["PagerSize"] == null)
                return Convert.ToInt32(Application["GRIDPAGERSIZE"]);
            else
                return Convert.ToInt16(this.ViewState["PagerSize"].ToString());
        }
        set
        {
            this.ViewState["PagerSize"] = value;
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
                return PagerSize;
            else
                return Convert.ToInt16(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }

    DocumentStoregeCenterRepository objDocumentRepo = new DocumentStoregeCenterRepository();
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Document Storage Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Document Storage Centre";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/DocumentStorageCentre/SearchDocumentStoregeCentre.aspx";

            if (!String.IsNullOrEmpty(Request.QueryString["FileName"]))
                this.FileName = Convert.ToString(Request.QueryString["FileName"]);
            if (!String.IsNullOrEmpty(Request.QueryString["Searchkeyword"]))
                this.Searchkeyword = Convert.ToString(Request.QueryString["Searchkeyword"]);
            if (!String.IsNullOrEmpty(Request.QueryString["UplodedBy"]))
                this.UplodedBy = Convert.ToString(Request.QueryString["UplodedBy"]);
            if (!String.IsNullOrEmpty(Request.QueryString["DocumentType"]))
                this.DocumentType = Convert.ToInt64(Request.QueryString["DocumentType"]);

            if (string.IsNullOrEmpty(FileName) && string.IsNullOrEmpty(Searchkeyword) && string.IsNullOrEmpty(UplodedBy) && DocumentType == null)
            {
                dvfolder.Visible = true;
                dvList.Visible = false;
                BindData();
            }
            else
            {
                dvfolder.Visible = false;
                dvList.Visible = true;
                BindGridView();
            }
        }
    }

    private void BindGridView()
    {
        DataView myDataView = new DataView();
        List<Incentex.DAL.SqlRepository.DocumentStoregeCenterRepository.DocumentStoregeCentreCustom> list = objDocumentRepo.GetDocumentStoregecenterBySearch(this.FileName, this.UplodedBy, this.Searchkeyword, DocumentType);

        if (list.Count == 0)
        {
            pagingtable.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
        }


        DataTable dataTable = ListToDataTable(list);
        myDataView = dataTable.DefaultView;
        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }

        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = 50; //Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;

        grdView.DataSource = pds;
        grdView.DataBind();

        doPaging();
    }

    private void BindData()
    {
        LookupRepository objLookRep = new LookupRepository();
        dtDocumentFolder.DataSource = objLookRep.GetByLookup("DocumentStorageType");
        dtDocumentFolder.DataBind();
    }

    protected void dtDocumentFolder_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "SearchDocument")
        {
            this.DocumentType = Convert.ToInt64(e.CommandArgument);
            DataListItem row = (DataListItem)(((LinkButton)e.CommandSource).NamingContainer);
            HiddenField hdfpassword = (HiddenField)(((System.Web.UI.WebControls.DataListItem)(row)).FindControl("hdfpassword"));

            if (hdfpassword != null && string.IsNullOrEmpty(hdfpassword.Value))
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/DocumentStorageCentre/ListDocumentStoregeCenter.aspx";
                dvfolder.Visible = false;
                dvList.Visible = true;
                BindGridView();
            }
            else
            {
                passwordmodal.Show();
            }
        }
    }

    protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFileSize = (Label)e.Row.FindControl("lblFileSize");
            HiddenField hdnextension = (HiddenField)e.Row.FindControl("hdnextension");
            System.Web.UI.HtmlControls.HtmlImage viewimg = (System.Web.UI.HtmlControls.HtmlImage)e.Row.FindControl("viewimg");

            if (lblFileSize != null)
            {
                lblFileSize.Text = Math.Round(Convert.ToDecimal(lblFileSize.Text.Trim()), 3).ToString();
            }

            if (viewimg != null)
            {
                if (hdnextension.Value.ToLower() == ".pdf")
                    viewimg.Src = "~/Images/FileType/pdf.png";
                else if (hdnextension.Value.ToLower() == ".jpg" || hdnextension.Value.ToLower() == ".jpeg")
                    viewimg.Src = "~/Images/FileType/jpg.png";
                else
                    viewimg.Src = "~/Images/FileType/doc.png";
            }
        }
    }

    protected void grdView_RowCommand(object sender, GridViewCommandEventArgs e)
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

            BindGridView();
        }
        else if (e.CommandName == "ViewDocument")
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            HiddenField hdnfilename = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnfilename"));
            HiddenField hdnOriginalFileName = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnOriginalFileName"));
            string filepath = Server.MapPath("~/UploadedImages/DocumentStorageCentre/") + hdnOriginalFileName.Value;
            //string filepath = ConfigurationSettings.AppSettings["siteurl"].ToString() + "UploadedImages/DocumentStorageCentre/" + hdnOriginalFileName.Value;
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), System.DateTime.Now.ToString(), "javascript:window.open('" + filepath + "', '', '');", true);

            objDocumentRepo.UpdateDocumentStoregeCenter(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(e.CommandArgument));
            BindGridView();

            this.DownloadFile(filepath, hdnfilename.Value);
        }
        else if (e.CommandName == "SendEmail")
        {
            modal.Show();
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            HiddenField hdnfilename = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnfilename"));
            HiddenField hdnOriginalFileName = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnOriginalFileName"));
            this.hdnfilename = hdnfilename.Value;
            this.hdnOriginalFileName = hdnOriginalFileName.Value;
        }
        else if (e.CommandName == "DeleteDocument")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            objDocumentRepo.DeleteDocumentStorageCenter(Convert.ToInt64(e.CommandArgument));
            lblmsg.Text = "Record deleted successfully";
            BindGridView();
        }
    }

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGridView();
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
                ToPg = ToPg + PagerSize;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - PagerSize;

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
        BindGridView();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindGridView();
    }
    #endregion

    /// <summary>
    /// Converting from List to Datatables
    /// </summary>
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {

        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null);
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    /// <summary>
    /// Sent Mail
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            this.SentMail(hdnOriginalFileName, hdnfilename, txtEmailTo.Value.Trim());
            this.ClearControl();
        }
        catch (Exception ex)
        {
            ex = null;
            modal.Hide();
            lblmsg.Text = "Error in sending Email";
        }
    }

    protected void btnSubmitpass_Click(object sender, EventArgs e)
    {
        ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/DocumentStorageCentre/ListDocumentStoregeCenter.aspx";
        dvfolder.Visible = false;
        dvList.Visible = true;
        BindGridView();
    }

    /// <summary>
    /// Sent Mail with Document attachment
    /// </summary>
    /// <param name="Orignalfilename"></param>
    /// <param name="FileName"></param>
    private void SentMail(string Orignalfilename, string FileName, string EmailTo)
    {
        try
        {
            string sFrmadd = "support@world-link.us.com";
            string sToadd = EmailTo;
            string sSubject = "Incentex Document Storage Center";
            string sFrmname = IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName;


            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();

            String eMailTemplate = String.Empty;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/DocumentStorageCenter.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            string body = null;

            body = "Attached is a file for your review. If you have any questions please reply to this email.<br/><br/>";


            StringBuilder MessageBody = new StringBuilder(eMailTemplate);
            MessageBody.Replace("{MessageBody}", body);
            MessageBody.Replace("{Sender}", "Best Regards,<br/>Ken Nelson");

            //Local
            //bool result = Common.SendMailWithAttachment(sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, true, true, Server.MapPath("~/UploadedImages/DocumentStorageCentre/") + Orignalfilename, "smtp.gmail.com", 587, "testsoft9@gmail.com", "tyrqm$78", Orignalfilename, false);
            //Live setting
            bool result = Common.SendMailWithAttachment(sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, false, true, Server.MapPath("~/UploadedImages/DocumentStorageCentre/") + Orignalfilename, smtphost, smtpport, smtpUserID, smtppassword, Orignalfilename, true);

        }
        catch (Exception ex)
        {
        }
    }

    protected void DownloadFile(string filepath, string displayFileName)
    {
        System.IO.Stream iStream = null;
        string type = "";
        string ext = Path.GetExtension(filepath);


        // Buffer to read 10K bytes in chunk:
        byte[] buffer = new Byte[10000];


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
                    case ".xls":
                    case ".xlsx":
                        type = "application/x-excel";
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
                    case "docx":
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


            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + displayFileName + ext + "\"");
            Response.ContentType = type;
            Response.WriteFile(filepath);
            Response.End();


        }
        catch (Exception ex)
        {
            // Trap the error, if any.
            Response.Write("Error : " + ex.Message);
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

    private void ClearControl()
    {
        txtEmailTo.Value = string.Empty;
        lblmsg.Text = string.Empty;
    }
    #endregion
}
