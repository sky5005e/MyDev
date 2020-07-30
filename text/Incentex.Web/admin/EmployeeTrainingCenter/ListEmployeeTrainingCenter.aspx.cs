using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using Incentex.DAL.SqlRepository;
using System.Configuration;
using System.Web.UI.HtmlControls;

public partial class admin_EmployeeTrainingCenter_ListEmployeeTrainingCenter : PageBase
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
    long UplodedBy
    {
        get
        {
            if (ViewState["UplodedBy"] != null)
                return Convert.ToInt64(ViewState["UplodedBy"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["UplodedBy"] = value;
        }
    }
    long? TrainingVideoType
    {
        get
        {
            if (ViewState["TrainingVideoType"] != null)
                return Convert.ToInt64(ViewState["TrainingVideoType"].ToString());
            else
                return null;
        }
        set
        {
            ViewState["TrainingVideoType"] = value;
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
    bool IsYouTubeVideoID
    {
        get
        {
            if (ViewState["IsYouTubeVideoID"] != null)
                return Convert.ToBoolean(ViewState["IsYouTubeVideoID"]);
            else
                return false;
        }
        set
        {
            ViewState["IsYouTubeVideoID"] = value;
        }
    }
    bool IsSearchByFolder
    {
        get
        {
            if (ViewState["IsSearchByFolder"] != null)
                return Convert.ToBoolean(ViewState["IsSearchByFolder"]);
            else
                return false;
        }
        set
        {
            ViewState["IsSearchByFolder"] = value;
        }
    }

    EmployeeTrainingCenterRepository objEmpTrainingRepo = new EmployeeTrainingCenterRepository();
    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Employee Training Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Employee Training Centre";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/EmployeeTrainingCenter/SearchEmployeeTrainingCenter.aspx";

            if (!String.IsNullOrEmpty(Request.QueryString["FileName"]))
                this.FileName = Convert.ToString(Request.QueryString["FileName"]);
            if (!String.IsNullOrEmpty(Request.QueryString["Searchkeyword"]))
                this.Searchkeyword = Convert.ToString(Request.QueryString["Searchkeyword"]);
            if (!String.IsNullOrEmpty(Request.QueryString["UplodedBy"]))
                this.UplodedBy = Convert.ToInt64(Request.QueryString["UplodedBy"]);
            if (!String.IsNullOrEmpty(Request.QueryString["DocumentType"]))
                this.TrainingVideoType = Convert.ToInt64(Request.QueryString["DocumentType"]);
            if (!String.IsNullOrEmpty(Request.QueryString["IsSearchByFolder"]))
                this.IsSearchByFolder = Convert.ToBoolean(Request.QueryString["IsSearchByFolder"]);

            if (string.IsNullOrEmpty(FileName) && string.IsNullOrEmpty(Searchkeyword) && UplodedBy == 0 && !this.IsSearchByFolder && (TrainingVideoType == null || TrainingVideoType == 0))
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

        if (TrainingVideoType != 0 && IsSearchByFolder)
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/EmployeeTrainingCenter/ListEmployeeTrainingCenter.aspx";
    }

    private void BindGridView()
    {
        dvfolder.Visible = false;
        dvList.Visible = true;

        DataView myDataView = new DataView();
        List<Incentex.DAL.SqlRepository.EmployeeTrainingCenterRepository.EmployeeTrainingCentreCustom> list = objEmpTrainingRepo.GetEmployeeTrainingcenterBySearch(this.FileName, this.UplodedBy, this.Searchkeyword, TrainingVideoType);

        DataTable dataTable = ListToDataTable(list);
        myDataView = dataTable.DefaultView;
        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }
        grdView.DataSource = myDataView;
        grdView.DataBind();
    }

    private void BindData()
    {
        LookupRepository objLookRep = new LookupRepository();
        dtDocumentFolder.DataSource = objLookRep.GetByLookup("EmployeeTrainingType");
        dtDocumentFolder.DataBind();
    }

    protected void dtDocumentFolder_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "SearchDocument")
        {
            this.TrainingVideoType = Convert.ToInt64(e.CommandArgument);
            DataListItem row = (DataListItem)(((LinkButton)e.CommandSource).NamingContainer);
            HiddenField hdfpassword = (HiddenField)(((System.Web.UI.WebControls.DataListItem)(row)).FindControl("hdfpassword"));

            if (hdfpassword != null && string.IsNullOrEmpty(hdfpassword.Value))
            {
                this.IsSearchByFolder = true;
                //dvfolder.Visible = false;
                //dvList.Visible = true;
                Response.Redirect("ListEmployeeTrainingCenter.aspx?DocumentType=" + this.TrainingVideoType + "&IsSearchByFolder=" + true + "");
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/EmployeeTrainingCenter/ListEmployeeTrainingCenter.aspx";
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
            HtmlImage imgyoutube = (HtmlImage)e.Row.FindControl("imgyoutube");

            if (lblFileSize != null)
            {
                lblFileSize.Text = Math.Round(Convert.ToDecimal(lblFileSize.Text.Trim()), 3).ToString();
            }

            if (lblFileSize.Text == "0.000")
            {
                lblFileSize.Visible = false;
                imgyoutube.Visible = true;
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
        else if (e.CommandName == "PlayVideo")
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            HiddenField hdnVideoName = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnOriginalFileName"));
            HiddenField lblVideoTitle = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnfilename"));
            HiddenField hdnYouTubeVideoID = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnYouTubeVideoID"));

            //Update Status
            objEmpTrainingRepo.UpdateEmployeeTrainingCenter(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(e.CommandArgument));
            usercontrol_TrainingVideo ur = LoadControl("~/usercontrol/TrainingVideo.ascx") as usercontrol_TrainingVideo;

            if (hdnVideoName.Value != "")
                ur.VideoName = Path.GetFileNameWithoutExtension(hdnVideoName.Value) + ".flv";
            else
                ur.VideoYouTubID = hdnYouTubeVideoID.Value;

            ur.VideoTitle = lblVideoTitle.Value;
            plhPlayVideo.Controls.Add(ur);
        }
        else if (e.CommandName == "SendEmail")
        {
            modal.Show();
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            HiddenField hdnfilename = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnfilename"));
            HiddenField hdnOriginalFileName = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnOriginalFileName"));
            HiddenField hdnYouTubeVideoID = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnYouTubeVideoID"));

            this.hdnfilename = hdnfilename.Value;

            IsYouTubeVideoID = hdnOriginalFileName.Value != "" ? false : true;
            this.hdnOriginalFileName = !IsYouTubeVideoID ? hdnOriginalFileName.Value : hdnYouTubeVideoID.Value;
        }
        else if (e.CommandName == "DeleteTraining")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            objEmpTrainingRepo.DeleteEmployeeTrainingCenter(Convert.ToInt64(e.CommandArgument));
            lblmsg.Text = "Record deleted successfully";
            BindGridView();
        }
    }

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
        this.IsSearchByFolder = true;
        //dvfolder.Visible = false;
        //dvList.Visible = true;
        Response.Redirect("ListEmployeeTrainingCenter.aspx?DocumentType=" + this.TrainingVideoType + "&IsSearchByFolder=" + true + "");
        ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/EmployeeTrainingCenter/ListEmployeeTrainingCenter.aspx";
        BindGridView();
    }

    /// <summary>
    /// Sent Mail with EmployeeTraining vidoes attachment
    /// </summary>
    /// <param name="Orignalfilename"></param>
    /// <param name="FileName"></param>
    private void SentMail(string Orignalfilename, string FileName, string EmailTo)
    {
        try
        {
            string sFrmadd = "support@world-link.us.com";
            string sToadd = EmailTo;
            string sSubject = "Incentex Employee Training Center";
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

            if (!IsYouTubeVideoID)
                body = "Attached is a vidoes for your review. If you have any questions please reply to this email.<br/><br/>";
            else
            {
                body = "Below is a vidoes Link for your review. If you have any questions please reply to this email.<br/><br/>";
                body += "http://www.youtube.com/v/" + Orignalfilename + "";
            }

            StringBuilder MessageBody = new StringBuilder(eMailTemplate);
            MessageBody.Replace("{MessageBody}", body);
            MessageBody.Replace("{Sender}", "Best Regards,<br/>Ken Nelson");

            //Local
            //bool result = Common.SendMailWithAttachment(sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, true, true, IsYouTubeVideoID ? null : Server.MapPath("~/UploadedImages/TrainingVideo/") + Orignalfilename, "smtp.gmail.com", 587, "testsoft9@gmail.com", "tyrqm$78", Orignalfilename, false);
            //Live setting
            bool result = Common.SendMailWithAttachment(sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, false, true, IsYouTubeVideoID ? null : Server.MapPath("~/UploadedImages/TrainingVideo/") + Orignalfilename, smtphost, smtpport, smtpUserID, smtppassword, Orignalfilename, true);


            if (!result)
                lblmsg.Text = "Error in sending Email";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void ClearControl()
    {
        txtEmailTo.Value = string.Empty;
        lblmsg.Text = string.Empty;
    }
    #endregion
}
