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
using Incentex.DAL;
using System.Collections.Generic;
using Incentex.DAL.SqlRepository;
using System.IO;

public partial class admin_CompanyStore_News : PageBase
{

    #region Local Property

    CompanyStoreDocumentRepository objRep = new CompanyStoreDocumentRepository();
    List<StoreDocument> objStoreDocumentList = new List<StoreDocument>();
    PagedDataSource pds = new PagedDataSource();
    StoreDocument objStoreDocument = new StoreDocument();
    Common objcommon = new Common();
    string sFilePath;
    string sFileName;
    /// <summary>
    /// 
    /// </summary>
    Int64 CompanyStoreId
    {
        get
        {
            if (ViewState["CompanyStoreId"] == null)
            {
                ViewState["CompanyStoreId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {
            ViewState["CompanyStoreId"] = value;
        }
    }

    Int64 CompanyStoreDocumentId
    {
        get
        {
            if (ViewState["CompanyStoreDocumentId"] == null)
            {
                ViewState["CompanyStoreDocumentId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreDocumentId"]);
        }
        set
        {
            ViewState["CompanyStoreDocumentId"] = value;
        }
    }
    #endregion

    #region Page Load Event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
                CheckLogin();
                ((Label)Master.FindControl("lblPageHeading")).Text = "News";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/UserManagement.aspx";
                getnewsbystore();
            }
    }

    #endregion

    #region Miscellaneous functions
    public void getnewsbystore()
    {
        DataView myDataView = new DataView();
        this.CompanyStoreId = new CompanyStoreRepository().GetByCompanyId((long)IncentexGlobal.CurrentMember.CompanyId).StoreID;
        hdnWorkgroup.Value = new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).WorkgroupID.ToString();
        lblWG.Text = new LookupRepository().GetById(new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).WorkgroupID).sLookupName;
        List<CompanyStoreDocumentRepository.NewsExtension> objExtList = objRep.GetAllNewsByWorkgroupIdForCA(Convert.ToInt64(hdnWorkgroup.Value), (long)IncentexGlobal.CurrentMember.CompanyId).ToList();
        if (objExtList.Count == 0)
        {
            pagingtable.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
        }


        DataTable dataTable = ListToDataTable(objExtList);
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
        dtlNews.DataSource = pds;
        dtlNews.DataBind();
        doPaging();
        //objStoreDocumentList = objRep.GetAllGuideLineManuals(this.CompanyStoreId);

    }
    protected void DownloadFile(string filepath, string displayFileName)
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

    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
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

    /// <summary>
    /// Clear Controls
    /// </summary>
    private void ClearControls()
    {
        lblmsgList.Text = string.Empty;
        hdnDepartment.Value = "0";
        
        txtNewsTitle.Value = string.Empty;
        txtPostDate.Text = string.Empty;
        txtExpirationDate.Text = string.Empty;

    }
    #endregion

    #region Button Click events
    protected void lnkBtnUploadNews_Click(object sender, EventArgs e)
    {
        try
        {

            if (((float)Request.Files[0].ContentLength / 1048576) > 2)
            {
                lblMsg.Text = "The file you are uploading is more than 2MB.";
                return;
            }


            if (this.CompanyStoreDocumentId != 0)
            {
                objStoreDocument = objRep.GetById(this.CompanyStoreDocumentId);
            }

            objStoreDocument.WorkgroupID = Convert.ToInt64(hdnWorkgroup.Value);
            objStoreDocument.DepartmentID = Convert.ToInt64(hdnDepartment.Value);
            objStoreDocument.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objStoreDocument.CretedDate = System.DateTime.Now;
            objStoreDocument.NewsTitleDes = txtNewsTitle.Value;
            objStoreDocument.DocuemntFor = Incentex.DAL.Common.DAEnums.CompanyStoreDocumentFor.News.ToString();
            
            if (this.CompanyStoreDocumentId == 0)
            {
                sFileName = Incentex.DAL.Common.DAEnums.CompanyStoreDocumentFor.News.ToString() + "_" + this.CompanyStoreId.ToString() + "_" + Convert.ToInt64(hdnWorkgroup.Value) + "_" + System.DateTime.Now.Ticks + "_" + fpUpload.Value;
                sFilePath = Server.MapPath("~/UploadedImages/CompanyStoreDocuments/") + sFileName;
                Request.Files[0].SaveAs(sFilePath);
                if (fpUpload.Value != null)
                    objStoreDocument.NewsTitle = sFileName;
                else
                    objStoreDocument.NewsTitle = null;

            }
            else
            {
                if (fpUpload.Value != "")
                {
                    sFileName = Incentex.DAL.Common.DAEnums.CompanyStoreDocumentFor.News.ToString() + "_" + this.CompanyStoreId.ToString() + "_" + Convert.ToInt64(hdnWorkgroup.Value) + "_" + System.DateTime.Now.Ticks + "_" + fpUpload.Value;
                    sFilePath = Server.MapPath("~/UploadedImages/CompanyStoreDocuments/") + sFileName;
                    Request.Files[0].SaveAs(sFilePath);
                    //Delete Old File
                    objcommon.DeleteImageFromFolder(hdnEditNewsDocumentName.Value, IncentexGlobal.companystoredocuments);
                }
                else
                {
                    sFileName = hdnEditNewsDocumentName.Value;
                }

            
            }
            //objStoreDocument.NewsTitle = txtNewsTitle.Value;
            objStoreDocument.NewsPostDate = Convert.ToDateTime(txtPostDate.Text);
            objStoreDocument.NewsEndDate = Convert.ToDateTime(txtExpirationDate.Text);
            

            objStoreDocument.StoreId = this.CompanyStoreId;

            if (this.CompanyStoreDocumentId == 0)
            {
                objRep.Insert(objStoreDocument);
            }
            objRep.SubmitChanges();

            ClearControls();

            if (this.CompanyStoreDocumentId == 0)
                lblmsgList.Text = "Record added successfully";
            else
                lblmsgList.Text = "Record updated successfully";

            getnewsbystore();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void lnkAddNews_Click(object sender, EventArgs e)
    {
        this.CompanyStoreDocumentId = 0;
        ClearControls();
    }
    #endregion

    #region GridView Events
    protected void dtlNews_RowCommand(object sender, GridViewCommandEventArgs e)
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

        if (e.CommandName == "deletedocument")
        {
            lblmsgList.Text = "";
            GridViewRow row;
            row = (GridViewRow)((ImageButton)e.CommandSource).Parent.Parent;
            objRep.DeleteCompanyStoreDocument(e.CommandArgument.ToString());
            lblmsgList.Text = "Record deleted successfully";
        }

        if (e.CommandName == "EditNews")
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;

            objStoreDocument = objRep.GetById(Convert.ToInt64(e.CommandArgument.ToString()));
            this.CompanyStoreDocumentId = objStoreDocument.StoreDocumentID;
            hdnWorkgroup.Value = objStoreDocument.WorkgroupID.ToString();
            hdnDepartment.Value = objStoreDocument.DepartmentID.ToString();
            txtPostDate.Text = Convert.ToDateTime(objStoreDocument.NewsPostDate).ToShortDateString();
            hdnEditNewsDocumentName.Value = objStoreDocument.NewsTitle;
            //txtNewsTitle.Value = objStoreDocument.NewsTitle;
            txtExpirationDate.Text = Convert.ToDateTime(objStoreDocument.NewsEndDate).ToShortDateString();
            lblmsgList.Text = string.Empty;
            txtNewsTitle.Value = objStoreDocument.NewsTitleDes;
            //e.CommandArgument.ToString();

        }

        if (e.CommandName == "viewnewsdoc")
        {

            string file = e.CommandArgument.ToString();
            HiddenField lnkFileName;
            GridViewRow row;
            row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            lnkFileName = ((HiddenField)dtlNews.Rows[row.RowIndex].FindControl("hfHiddenFile"));
            string filePath = IncentexGlobal.companystoredocuments;
            string strFullPath = filePath + lnkFileName.Value;
            string[] filevalue = ((LinkButton)dtlNews.Rows[row.RowIndex].FindControl("lblNewsTitle")).ToolTip.ToString().Split('_');

            DownloadFile(strFullPath, filevalue[0].ToString());

        }


        getnewsbystore();

    }
    protected void dtlNews_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            switch (this.ViewState["SortExp"].ToString())
            {
                case "Workgroup":
                    PlaceHolder placeholderWorkgroup = (PlaceHolder)e.Row.FindControl("placeholderWorkgroup");
                    break;
                case "Department":
                    PlaceHolder placeholderDepartment = (PlaceHolder)e.Row.FindControl("placeholderDepartment");
                    break;
                case "NewsTitle":
                    PlaceHolder placeholderNewsTitle = (PlaceHolder)e.Row.FindControl("placeholderNewsTitle");
                    break;
                case "NewsPostDate":
                    PlaceHolder placeholderNewsPostDate = (PlaceHolder)e.Row.FindControl("placeholderNewsPostDate");
                    break;
                case "UploadNews":
                    PlaceHolder placeholderUploadNews = (PlaceHolder)e.Row.FindControl("placeholderUploadNews");
                    break;

            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //string[] filevalueTitleDes = ((HiddenField)e.Row.FindControl("hdnNewsTitleDes")).Value.ToString().Split('_');
            //if (filevalueTitleDes[0] !="")
            //{
            //    ((Label)e.Row.FindControl("lblNewsTitleDes")).Text = filevalueTitleDes[1].ToString();
            //}
            //else
            //{
            //    ((Label)e.Row.FindControl("lblNewsTitleDes")).Text = string.Empty;
            //}

            //if (((Label)e.Row.FindControl("lblNewsTitleDes")).Text.ToString().Length > 15)
            //{
            //    ((Label)e.Row.FindControl("lblNewsTitleDes")).ToolTip = ((Label)e.Row.FindControl("lblNewsTitleDes")).Text;
            //    ((Label)e.Row.FindControl("lblNewsTitleDes")).Text = ((Label)e.Row.FindControl("lblNewsTitleDes")).Text.ToString().Substring(0, 15) + "...";
            //}
            if (((HiddenField)e.Row.FindControl("hfHiddenFile")).Value.ToString() != "")
            {
                string[] filevalue = ((HiddenField)e.Row.FindControl("hfHiddenFile")).Value.ToString().Split('_');
                ((LinkButton)e.Row.FindControl("lblNewsTitle")).Text = filevalue[4].ToString();
            }

            if (((LinkButton)e.Row.FindControl("lblNewsTitle")).Text.ToString().Length > 15)
            {
                ((LinkButton)e.Row.FindControl("lblNewsTitle")).ToolTip = ((LinkButton)e.Row.FindControl("lblNewsTitle")).Text;
                ((LinkButton)e.Row.FindControl("lblNewsTitle")).Text = ((LinkButton)e.Row.FindControl("lblNewsTitle")).Text.ToString().Substring(0, 15) + "...";
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
            getnewsbystore();
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
        getnewsbystore();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        getnewsbystore();
    }
    #endregion

}
