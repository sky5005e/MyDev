using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyStore_GuideLineManuals : PageBase
{
    #region Local Property
    CompanyStoreDocumentRepository objRep = new CompanyStoreDocumentRepository();
    List<StoreDocument> objStoreDocumentList = new List<StoreDocument>();
    PagedDataSource pds = new PagedDataSource();
    StoreDocument objStoreDocument = new StoreDocument();
    Common objcommon = new Common();
    string sFilePath;
    string sFileName;

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
    #endregion

    #region Page Load Event
        protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Store Management Console";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            if (Request.QueryString.Count > 0)
            {   
                this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));

                if (this.CompanyStoreId == 0)
                {
                    Response.Redirect("~/admin/CompanyStore/GeneralStoreSetup.aspx?Id=" + this.CompanyStoreId);
                }
                
                if (this.CompanyStoreId > 0 && !base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                } 

                ((Label)Master.FindControl("lblPageHeading")).Text = "GuildeLine Manuals";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/CompanyStore/ViewCompanyStore.aspx";
                Session["ManageID"] = 5;
                menuControl.PopulateMenu(3, 0, this.CompanyStoreId, 0, false);

                BindDropDowns();
                //
                getguidemanualsbystore();
            }
        }
    }
    #endregion

    #region Button Click events
        protected void lnkBtnUploadWorkgroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (((float)Request.Files[0].ContentLength / 1048576) > 2)
                {
                    lblMsg.Text = "The file you are uploading is more than 2MB.";
                    return;
                }

                objStoreDocument.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
                objStoreDocument.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
                objStoreDocument.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objStoreDocument.CretedDate = System.DateTime.Now;
                objStoreDocument.DocuemntFor = Incentex.DAL.Common.DAEnums.CompanyStoreDocumentFor.GuideLineManuals.ToString();
                sFileName = Incentex.DAL.Common.DAEnums.CompanyStoreDocumentFor.GuideLineManuals.ToString() + "_" + this.CompanyStoreId.ToString() + "_" + Convert.ToInt64(ddlWorkgroup.SelectedValue) + "_" + System.DateTime.Now.Ticks + "_" + fpUpload.Value;
                sFilePath = Server.MapPath("~/UploadedImages/CompanyStoreDocuments/") + sFileName;
                Request.Files[0].SaveAs(sFilePath);
                if (fpUpload.Value != null)
                    objStoreDocument.DocumentName = sFileName;
                else
                    objStoreDocument.DocumentName = null;
                objStoreDocument.StoreId = this.CompanyStoreId;
                objRep.Insert(objStoreDocument);
                objRep.SubmitChanges();
                //
                lblMsg.Text = "Record added successfully";
                //
                getguidemanualsbystore();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

    #region Miscellaneous functions
        //bind dropdown for the workgroup, department
        public void BindDropDowns()
        {
            LookupRepository objLookRep = new LookupRepository();

            // For Department
            ddlDepartment.DataSource = objLookRep.GetByLookup("Department");
            ddlDepartment.DataValueField = "iLookupID";
            ddlDepartment.DataTextField = "sLookupName";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("-Select-", "0"));

            // For Workgroup
            ddlWorkgroup.DataSource = objLookRep.GetByLookup("Workgroup");
            ddlWorkgroup.DataValueField = "iLookupID";
            ddlWorkgroup.DataTextField = "sLookupName";
            ddlWorkgroup.DataBind();
            ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));
        } 

        public void getguidemanualsbystore()
        {
            DataView myDataView = new DataView();
            List<CompanyStoreDocumentRepository.StoreDocumentExtension> objExtList = objRep.GetAllGuideLineManuals(this.CompanyStoreId);
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
            dtlManuals.DataSource = pds;
            dtlManuals.DataBind();
            doPaging();
            //objStoreDocumentList = objRep.GetAllGuideLineManuals(this.CompanyStoreId);

        }
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
        #endregion

    #region Datalist Events
        protected void dtlManuals_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    case "DocumentName":
                        PlaceHolder placeholderFileName = (PlaceHolder)e.Row.FindControl("placeholderFileName");
                        break;
                }

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string[] filevalue = ((LinkButton)e.Row.FindControl("lblFileName")).Text.ToString().Split('_');
                ((LinkButton)e.Row.FindControl("lblFileName")).Text = filevalue[4].ToString().Split('.')[0].ToString();

                /*if (((Label)e.Row.FindControl("lblFileName")).Text.ToString().Length > 30)
                 {
                     ((Label)e.Row.FindControl("lblFileName")).Text = ((Label)e.Row.FindControl("lblFileName")).Text.ToString().Substring(0, 30) + "...";
                 }*/
            }
        }
        protected void dtlManuals_RowCommand(object sender, GridViewCommandEventArgs e)
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
                if (!base.CanDelete)
                {
                    base.RedirectToUnauthorised();
                }
                //lblMsg.Text = "";
                GridViewRow row;
                row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
                objRep.DeleteCompanyStoreDocument(e.CommandArgument.ToString());
                objcommon.DeleteImageFromFolder(((HiddenField)dtlManuals.Rows[row.RowIndex].FindControl("hfHiddenFile")).Value, IncentexGlobal.companystoredocuments);
                lblMsg.Text = "Record deleted successfully";
            }
            if (e.CommandName == "viewguidelinemanual")
            {

                string file = e.CommandArgument.ToString();
                HiddenField lnkFileName;
                GridViewRow row;
                row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
                lnkFileName = ((HiddenField)dtlManuals.Rows[row.RowIndex].FindControl("hfHiddenFile"));
                string filePath = IncentexGlobal.companystoredocuments;
                string strFullPath = filePath + lnkFileName.Value;
                string[] filevalue = ((LinkButton)dtlManuals.Rows[row.RowIndex].FindControl("lblFileName")).Text.ToString().Split('_');
                
                DownloadFile(strFullPath,filevalue[0].ToString());

            }
            getguidemanualsbystore();

        }
        #endregion

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            getguidemanualsbystore();
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
        getguidemanualsbystore();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        getguidemanualsbystore();
    }
    #endregion
}
