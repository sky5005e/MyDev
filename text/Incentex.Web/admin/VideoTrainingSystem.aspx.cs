/// <summary>
/// Module Name : Video Training System
/// Description : This page dsiplay list of video for all store.
///               You can add/update and delete video from this page also.
///               Video is display to CA/CE as popup window based on storeid,url,workgroup,department,startdate and enddate.
/// Created : Mayur on 15-dec-2011
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Data;
using System.IO;
using Incentex.BE;
using Incentex.DA;

public partial class admin_VideoTrainingSystem : PageBase
{
    #region Data Members
    VideoTrainingRepository objRep = new VideoTrainingRepository();
    List<VideoTraining> objVideoTrainingList = new List<VideoTraining>();
    PagedDataSource pds = new PagedDataSource();
    VideoTraining objVideoTraining = new VideoTraining();
    Common objcommon = new Common();
    string sFilePath;
    string sFileName;
    Int64 VideoTrainingId
    {
        get
        {
            if (ViewState["VideoTrainingId"] == null)
            {
                ViewState["VideoTrainingId"] = 0;
            }
            return Convert.ToInt64(ViewState["VideoTrainingId"]);
        }
        set
        {
            ViewState["VideoTrainingId"] = value;
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
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Video Training System";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

           ((Label)Master.FindControl("lblPageHeading")).Text = "Video Training System";
           ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Index.aspx";

           FillCompanyStore();
           FillWorkgroup();
           FillVideoPlacement();
           BindGrid();
        }
    }

    #region Button Click events
    protected void lnkBtnUploadVideo_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkUploadToWL.Checked == false && chkUploadToYouTube.Checked == false)
            {
                lblMsg.Text = "Please select one of the video upload option.";
                return;
            }
            
            if (chkUploadToWL.Checked == true) //This is for upload video to world-link
            {
                if (((float)Request.Files[0].ContentLength / 1048576) > 150)
                {
                    lblMsg.Text = "The file you are uploading is more than 150MB.";
                    return;
                }
            }

            if (this.VideoTrainingId != 0)
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                objVideoTraining = objRep.GetById(this.VideoTrainingId);
            }

            if (ddlCompany.SelectedValue != "0")
                objVideoTraining.StoreID = Convert.ToInt64(ddlCompany.SelectedValue);
            else
                objVideoTraining.StoreID = null;

            if (ddlWorkgroup.SelectedValue != "0")
                objVideoTraining.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            else
                objVideoTraining.WorkgroupID = null;

            if (ddlUserType.SelectedValue != "0")
                objVideoTraining.UserTypeID = Convert.ToInt64(ddlUserType.SelectedValue);
            else
                objVideoTraining.UserTypeID = null;

            if (chkUploadToWL.Checked == true) //This is for upload video to world-link
            {
                if (this.VideoTrainingId == 0)
                {
                    sFileName = "Video_" + System.DateTime.Now.Ticks + "_" + fpUpload.Value;
                    sFilePath = Server.MapPath("~/UploadedImages/TrainingVideo/") + sFileName;
                    Request.Files[0].SaveAs(sFilePath);
                    if (fpUpload.Value != null)
                        objVideoTraining.VideoName = sFileName;
                    else
                        objVideoTraining.VideoName = "";

                }
                else
                {
                    if (fpUpload.Value != "")
                    {
                        sFileName = "Video_" + System.DateTime.Now.Ticks + "_" + fpUpload.Value;
                        sFilePath = Server.MapPath("~/UploadedImages/TrainingVideo/") + sFileName;
                        Request.Files[0].SaveAs(sFilePath);
                        //Delete Old File
                        objcommon.DeleteImageFromFolder(hdnEditVideoName.Value, Server.MapPath("~/UploadedImages/TrainingVideo/"));
                    }
                    else
                    {
                        sFileName = hdnEditVideoName.Value;
                    }

                    objVideoTraining.VideoName = sFileName;
                }
                objVideoTraining.YouTubeVideoID = null;
            }
            else if(chkUploadToYouTube.Checked == true)
            {
                objVideoTraining.YouTubeVideoID = txtYouTubeVideoID.Text.Trim();
                objVideoTraining.VideoName = null;
            }

            objVideoTraining.PlacementID = Convert.ToInt64(ddlPlacement.SelectedValue);
            objVideoTraining.VideoTitle = txtVideoTitle.Text;
            objVideoTraining.CreatedDate = System.DateTime.Now;
            objVideoTraining.StartDate = Convert.ToDateTime(txtStartDate.Text);
            objVideoTraining.ExpiredDate = Convert.ToDateTime(txtExpirationDate.Text);

            if (this.VideoTrainingId == 0)
            {
                objRep.Insert(objVideoTraining);
            }
            objRep.SubmitChanges();

            ClearControls();

            if (this.VideoTrainingId == 0)
                lblmsgList.Text = "Record added successfully";
            else
                lblmsgList.Text = "Record updated successfully";


            BindGrid();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    protected void lnkAddVideo_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        ClearControls();
    }
    #endregion

    #region Checkbox
    protected void chkUploadToWL_CheckedChanged(object sender, EventArgs e)
    {
        //Added on 15-sep-2011
        if (chkUploadToWL.Checked == true)
        {
            chkUploadToWL.Checked = true;
            spnUploadToWL.Attributes.Add("class", "custom-checkbox_checked alignleft");
            trUploadToWL.Visible = true;

            chkUploadToYouTube.Checked = false;
            spnUploadToYouTube.Attributes.Add("class", "custom-checkbox alignleft");
            trUploadToYoutube.Visible = false;
        }
        else
        {
            chkUploadToWL.Checked = false;
            spnUploadToWL.Attributes.Add("class", "custom-checkbox alignleft");
            trUploadToWL.Visible = false;
        }
    }

    protected void chkUploadToYouTube_CheckedChanged(object sender, EventArgs e)
    {
        //Added on 15-sep-2011
        if (chkUploadToYouTube.Checked == true)
        {
            chkUploadToYouTube.Checked = true;
            spnUploadToYouTube.Attributes.Add("class", "custom-checkbox_checked alignleft");
            trUploadToYoutube.Visible = true;

            chkUploadToWL.Checked = false;
            spnUploadToWL.Attributes.Add("class", "custom-checkbox alignleft");
            trUploadToWL.Visible = false;
        }
        else
        {
            chkUploadToYouTube.Checked = false;
            trUploadToYoutube.Visible = false;
            spnUploadToYouTube.Attributes.Add("class", "custom-checkbox alignleft");
        }

    }
    #endregion

    #region Datalist Events
    protected void dtlVideos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            switch (this.ViewState["SortExp"].ToString())
            {
                case "Company":
                    PlaceHolder placeholderCompany = (PlaceHolder)e.Row.FindControl("placeholderCompany");
                    break;
                case "Workgroup":
                    PlaceHolder placeholderWorkgroup = (PlaceHolder)e.Row.FindControl("placeholderWorkgroup");
                    break;
                case "UserType":
                    PlaceHolder placeholderUserType = (PlaceHolder)e.Row.FindControl("placeholderUserType");
                    break;
                case "Placement":
                    PlaceHolder placeholderPlacement = (PlaceHolder)e.Row.FindControl("placeholderPlacement");
                    break;
                case "VideoTitle":
                    PlaceHolder placeholderVideoTitle = (PlaceHolder)e.Row.FindControl("placeholderVideoTitle");
                    break;
                case "VideoName":
                    PlaceHolder placeholderVideoName = (PlaceHolder)e.Row.FindControl("placeholderVideoName");
                    break;
                case "StartDate":
                    PlaceHolder placeholderStartDate = (PlaceHolder)e.Row.FindControl("placeholderStartDate");
                    break;
                case "ExpiredDate":
                    PlaceHolder placeholderExpiredDate = (PlaceHolder)e.Row.FindControl("placeholderExpiredDate");
                    break;
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((LinkButton)e.Row.FindControl("lnkbtnItemCompany")).Text.ToString() == "")
            {
                ((LinkButton)e.Row.FindControl("lnkbtnItemCompany")).Text = "All Company";
            }
            if (((Label)e.Row.FindControl("lblWorkgroup")).Text.ToString() == "")
            {
                ((Label)e.Row.FindControl("lblWorkgroup")).Text = "All Workgroup";
            }
            if (((Label)e.Row.FindControl("lblUserType")).Text.ToString() == "")
            {
                ((Label)e.Row.FindControl("lblUserType")).Text = "Both";
            }
            if (((HiddenField)e.Row.FindControl("hdnVideoName")).Value.ToString() != "")
            {
                string[] filevalue = ((HiddenField)e.Row.FindControl("hdnVideoName")).Value.ToString().Split('_');
                ((LinkButton)e.Row.FindControl("lnkbtnItemVideoName")).Text = filevalue[2].ToString();
            }
            else
            {
                ((LinkButton)e.Row.FindControl("lnkbtnItemVideoName")).Text = ((HiddenField)e.Row.FindControl("hdnYouTubeVideoID")).Value;
                ((LinkButton)e.Row.FindControl("lnkbtnItemVideoName")).Enabled = false;
            }
        }
    }
    protected void dtlVideos_RowCommand(object sender, GridViewCommandEventArgs e)
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

        if (e.CommandName == "EditVideo")
        {
            ClearControls();
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;

            objVideoTraining = objRep.GetById(Convert.ToInt64(e.CommandArgument.ToString()));
            this.VideoTrainingId = objVideoTraining.VideoTrainingID;
            ddlWorkgroup.SelectedValue = objVideoTraining.WorkgroupID != null ? objVideoTraining.WorkgroupID.ToString() : "0";
            ddlCompany.SelectedValue = objVideoTraining.StoreID != null ? objVideoTraining.StoreID.ToString() : "0";
            ddlUserType.SelectedValue = objVideoTraining.UserTypeID != null ? objVideoTraining.UserTypeID.ToString() : "0";
            ddlPlacement.SelectedValue = objVideoTraining.PlacementID.ToString();
            txtVideoTitle.Text = objVideoTraining.VideoTitle;
            if (!string.IsNullOrEmpty(objVideoTraining.VideoName))
            {
                hdnEditVideoName.Value = objVideoTraining.VideoName;
                chkUploadToWL.Checked = true;
                spnUploadToWL.Attributes.Add("class", "custom-checkbox_checked alignleft");
                trUploadToWL.Visible = true;

                chkUploadToYouTube.Checked = false;
                spnUploadToYouTube.Attributes.Add("class", "custom-checkbox alignleft");
                trUploadToYoutube.Visible = false;
            }
            else
            {
                txtYouTubeVideoID.Text = objVideoTraining.YouTubeVideoID;
                chkUploadToYouTube.Checked = true;
                spnUploadToYouTube.Attributes.Add("class", "custom-checkbox_checked alignleft");
                trUploadToYoutube.Visible = true;

                chkUploadToWL.Checked = false;
                spnUploadToWL.Attributes.Add("class", "custom-checkbox alignleft");
                trUploadToWL.Visible = false;
            }
            txtStartDate.Text = Convert.ToDateTime(objVideoTraining.StartDate).ToShortDateString();
            txtExpirationDate.Text = Convert.ToDateTime(objVideoTraining.ExpiredDate).ToShortDateString();
            lblmsgList.Text = string.Empty;
        }


        if (e.CommandName == "DeleteVideo")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            GridViewRow row;
            row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            objRep.DeleteById(Convert.ToInt64(e.CommandArgument.ToString()));
            objcommon.DeleteImageFromFolder(((HiddenField)dtlVideos.Rows[row.RowIndex].FindControl("hdnVideoName")).Value, Server.MapPath("~/UploadedImages/TrainingVideo/"));
            lblmsgList.Text = "Record deleted successfully";
        }
        if (e.CommandName == "DownloadVideo")
        {
            HiddenField lnkFileName;
            GridViewRow row;
            row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            lnkFileName = ((HiddenField)dtlVideos.Rows[row.RowIndex].FindControl("hdnVideoName"));
            string filePath = Server.MapPath("~/UploadedImages/TrainingVideo/");
            string strFullPath = filePath + lnkFileName.Value;
            string[] filevalue = ((LinkButton)dtlVideos.Rows[row.RowIndex].FindControl("lnkbtnItemVideoName")).Text.ToString().Split('_');

            DownloadFile(strFullPath, filevalue[0].ToString());

        }
        if (e.CommandName == "PlayVideo")
        {
            GridViewRow row;
            row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            HiddenField hdnVideoName = ((HiddenField)dtlVideos.Rows[row.RowIndex].FindControl("hdnVideoName"));
            HiddenField hdnYouTubeVideoID = ((HiddenField)dtlVideos.Rows[row.RowIndex].FindControl("hdnYouTubeVideoID"));
            Label lblVideoTitle = ((Label)dtlVideos.Rows[row.RowIndex].FindControl("lblVideoTitle"));
            usercontrol_TrainingVideo ur = LoadControl("~/usercontrol/TrainingVideo.ascx") as usercontrol_TrainingVideo;
            if (hdnVideoName.Value != "")
                ur.VideoName = hdnVideoName.Value;
            else
                ur.VideoYouTubID = hdnYouTubeVideoID.Value;
            ur.VideoTitle = lblVideoTitle.Text;
            plhPlayVideo.Controls.Add(ur);
        }
        BindGrid();

    }
    #endregion
    #endregion

    #region Methods

    /// <summary>
    /// Fills the company store dropdownlist.
    /// </summary>
    private void FillCompanyStore()
    {
        try
        {   
            OrderConfirmationRepository objLookRep = new OrderConfirmationRepository();
            ddlCompany.DataSource = objLookRep.GetCompanyStoreName().OrderBy(le => le.CompanyName);
            ddlCompany.DataValueField = "StoreID";
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Fills the workgroup.
    /// </summary>
    private void FillWorkgroup()
    {
        LookupDA sWorkgroup = new LookupDA();
        LookupBE sWorkgroupBE = new LookupBE();
        sWorkgroupBE.SOperation = "selectall";
        sWorkgroupBE.iLookupCode = "Workgroup";
        ddlWorkgroup.DataSource = sWorkgroup.LookUp(sWorkgroupBE);
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    /// <summary>
    /// Fills the video placement dropdownlist.
    /// </summary>
    private void FillVideoPlacement()
    {
        LookupDA sVideoPlacement = new LookupDA();
        LookupBE sVideoPlacementBE = new LookupBE();
        sVideoPlacementBE.SOperation = "selectall";
        sVideoPlacementBE.iLookupCode = "VideoPlacement";

        DataSet dsVideoPlacement = sVideoPlacement.LookUp(sVideoPlacementBE);
        ddlPlacement.DataSource = dsVideoPlacement;
        ddlPlacement.DataValueField = "iLookupID";
        ddlPlacement.DataTextField = "sLookupName";
        ddlPlacement.DataBind();
        ddlPlacement.Items.Insert(0, new ListItem("-Select-", "0"));
    }
    
    public void BindGrid()
    {
        DataView myDataView = new DataView();
        List<SelectAllVideoTrainingResult> objVideoList = objRep.GetAllWithDetail();
        if (objVideoList.Count == 0)
        {
            pagingtable.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
        }


        DataTable dataTable = ListToDataTable(objVideoList);
        myDataView = dataTable.DefaultView;
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
        dtlVideos.DataSource = pds;
        dtlVideos.DataBind();
        doPaging();

    }

    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
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


            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + displayFileName + "\"");
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

    /// <summary>
    /// Clear Controls
    /// </summary>
    private void ClearControls()
    {
        lblmsgList.Text = string.Empty;
        lblMsg.Text = string.Empty;
        ddlCompany.SelectedIndex = 0;
        ddlUserType.SelectedIndex = 0;
        ddlPlacement.SelectedIndex = 0;
        ddlWorkgroup.SelectedIndex = 0;
        txtVideoTitle.Text = string.Empty;
        hdnEditVideoName.Value=string.Empty;
        txtStartDate.Text = string.Empty;
        txtExpirationDate.Text = string.Empty;
        txtYouTubeVideoID.Text = string.Empty;
        chkUploadToYouTube.Checked = false;
        spnUploadToYouTube.Attributes.Add("class", "custom-checkbox alignleft");
        trUploadToWL.Visible = false;

        chkUploadToWL.Checked = false;
        spnUploadToWL.Attributes.Add("class", "custom-checkbox alignleft");
        trUploadToYoutube.Visible = false;
    }

    #endregion

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGrid();
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
            {
                ToPg = pds.PageCount;
            }

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
        BindGrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindGrid();
    }
    #endregion
}
