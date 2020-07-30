using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Company_ViewUploadDocument : PageBase
{
    Int64 DocumentId
    {
        get
        {
            if (ViewState["DocumentId"] == null)
            {
                ViewState["DocumentId"] = 0;
            }
            return Convert.ToInt64(ViewState["DocumentId"]);
        }
        set
        {
            ViewState["DocumentId"] = value;
        }
    }
    CompanyRepository objCompRepos = new CompanyRepository();
    CompanyDocumentRepository objComDocrepos = new CompanyDocumentRepository();
    Document objDoc = new Document();
    Common objcommon = new Common();
    PagedDataSource pds = new PagedDataSource();
    int iCompanyId ;
    bool isdelete = false;
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
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                base.MenuItem = "Document Management";
                base.ParentMenuID = 11;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                BindDocumentType();

                ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Document Management";
                ((HtmlGenericControl)manuControl.FindControl("dvSubMenu")).Visible = false;
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Company/ViewCompany.aspx";
                if (Request.QueryString["id"] !="0" )
                {
                    iCompanyId = Convert.ToInt32(Request.QueryString["id"].ToString());
                    bindGriedView(Convert.ToInt32(Request.QueryString["id"].ToString()));
                }
                else
                {
                    Response.Redirect("ViewCompany.aspx");
                }

                //populatesubmenu();
                manuControl.PopulateMenu(1, 0, Convert.ToInt32(Request.QueryString["id"].ToString()), 0, false); 
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    /// <summary>
    /// btnSubmit_Click()
    /// Save document file in database and
    /// in project folder.
    /// Nagmani Kumar 10/09/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string sFilePath = null;
        
                if (((float)Request.Files[0].ContentLength / 1048576) > 2)
                
                {
                    lblMessage.Text = "The file you are uploading is more than 2MB.";
                    modal.Show();
                    return;
                }

                //Check dulication here when add new record.
                int iduplicate = objCompRepos.CheckDuplicaetDocument(ViewState["DocumentTypeID"].ToString() + "_" + flFile.Value, Convert.ToInt32(Request.QueryString["id"].ToString()));
                if (iduplicate > 0)
                {
                    lblMessage.Text = "Record already exist!";
                     modal.Show();
                }
                else
                    {
                       
                    //Insert into Document tabel
            if (this.DocumentId != 0)
            {
                objDoc = objComDocrepos.GetById(this.DocumentId);
            }
            string strDocumentFor = Incentex.DAL.Common.DAEnums.GetLookupCodeName(Incentex.DAL.Common.DAEnums.LookupCodeType.DocumentFor);
            objDoc.DocumentFor = strDocumentFor;
            objDoc.DocumentTypeID = Convert.ToInt32(ViewState["DocumentTypeID"].ToString());
           if (flFile.Value != null)
                             objDoc.FileName = Convert.ToInt32(ViewState["DocumentTypeID"].ToString()) + "_" + flFile.Value;
                        else
                            objDoc.FileName = null;

           objDoc.ForeignKey = Convert.ToInt32(Request.QueryString["id"].ToString());
                            //sFilePath = Server.MapPath("../../../UploadedImages/CompanyDocument/") + Convert.ToInt32(ViewState["DocumentTypeID"].ToString()) + "_" + flFile.Value;
                            sFilePath = Server.MapPath("../../UploadedImages/CompanyDocument/") + Convert.ToInt32(ViewState["DocumentTypeID"].ToString()) + "_" + flFile.Value;
                        Request.Files[0].SaveAs(sFilePath);
                       if (this.DocumentId == 0)
                        {
                         objComDocrepos.Insert(objDoc);
                         objComDocrepos.SubmitChanges();
              
                            }
                       
                        this.DocumentId = objDoc.DocumentId;
                        this.DocumentId = 0;
                        //lblMessage.Text = "Record saved successfully!";
                        bindGriedView(Convert.ToInt32(Request.QueryString["id"].ToString()));
                        isdelete = true;
                    }
                if (isdelete == true)
                {
                    lblMsg.Text = "Recorsds upload successfully!";
                }
                
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        
    }
    /// <summary>
    /// lnkPGUpload_Click
    /// </summary>
    /// Nagmani Kumar 10/09/2010
    /// Open the Modal Dialog box for file upload.
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkPGUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            lblMsg.Text = string.Empty;
            icondiv.Visible = false;
            lblMessage.Text = string.Empty;
            ViewState["DocumentTypeID"] = hdnPorgramAgrement.Value;
            modal.Controls.Clear();
            modal.Show();
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        
    }
    /// <summary>
    /// This method is used to bind the document type in hiden field control
    /// for insert the documenttype id in table document when we save the 
    /// document file in table.
    /// Nagmani 10/09/2010
    /// BindDocumentType()
    /// <parameter>iLookupCode</parameter>
    /// </summary>
    public void BindDocumentType()
    {
        string striLookupCode = "Document";
        List<INC_Lookup> objlist = new List<INC_Lookup>();
        objlist = objCompRepos.GetLookupDocumentDetails(striLookupCode);
        if (objlist != null)
        {
            DataTable dataTable = ListToDataTable(objlist);
            if (dataTable.Rows[0].ItemArray[2].ToString() == "Program Agreement")
            {
                hdnPorgramAgrement.Value = dataTable.Rows[0].ItemArray[0].ToString();
            }
            if (dataTable.Rows[1].ItemArray[2].ToString() == "Guidelines Manuel")
            {
                hdnGuidelinesManuel.Value = dataTable.Rows[1].ItemArray[0].ToString();
            }
            if (dataTable.Rows[2].ItemArray[2].ToString() == "News Files")
            {
                hdnNewsFiles.Value = dataTable.Rows[2].ItemArray[0].ToString();
            }
            if (dataTable.Rows[3].ItemArray[2].ToString() == "NDA Agreement")
            {
                hdnNDAAgreement.Value = dataTable.Rows[3].ItemArray[0].ToString();
            }
            if (dataTable.Rows[4].ItemArray[2].ToString() == "Other Documents")
            {
                hdnOtherDocuments.Value = dataTable.Rows[4].ItemArray[0].ToString();
            }
        }
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
    /// <summary>
    /// BindDatlist()
    /// This method is used to bind the datalist
    /// on the basis of using parameter ILookupcode from which is coming
    /// from the dropdown menus page.
    /// </summary>
    /// <param name="strQurystring"></param>
    public void bindGriedView(Int32 CompanyID)
    {
        try
        {

            List<CompanyDocumentRepository.CompanyDocumentResult> oad = new CompanyDocumentRepository().GetDocumentDetails(Convert.ToInt32(Request.QueryString["id"].ToString()));

            if (oad.Count == 0)
            {
                pagingtable.Visible = false;
            }
            else
            {
                pagingtable.Visible = true;
            }
            if (oad != null)
            {
               
                DataView myDataView = new DataView();
                DataTable dataTable = ListToDataTable(oad);
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
                gvComDocumnet.DataSource = pds;
                gvComDocumnet.DataBind();
                doPaging();
                
            }
            //else
            //{

            //    gvComDocumnet.DataSource = null;
            //    gvComDocumnet.DataBind();

            //}
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
           ErrHandler.WriteError(ex);
        }
        
    }
    protected void gvComDocumnet_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "view")
            {
               
                string file = e.CommandArgument.ToString();
                LinkButton  lnkFileName;
                GridViewRow row;
                row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
                lnkFileName = (LinkButton)gvComDocumnet.Rows[row.RowIndex].FindControl("lnkFileName");
                string filePath = IncentexGlobal.documentpath;
                string strFullPath = filePath + lnkFileName.Text;
                DownloadFile(strFullPath);
               
            }
            if (e.CommandName == "del")  // or delete
            {
                if (!base.CanDelete)
                {
                    base.RedirectToUnauthorised();
                }

                LinkButton lbtnDelete;
                LinkButton lnkFileName;
                Label lblDocumentID;
                GridViewRow row;
                row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
                ViewState["row"] = row;
                lbtnDelete = (LinkButton)(gvComDocumnet.Rows[row.RowIndex].FindControl("lnkbtndelete"));
                lblDocumentID = (Label)(gvComDocumnet.Rows[row.RowIndex].FindControl("lblDocumentID"));
                lnkFileName = (LinkButton)gvComDocumnet.Rows[row.RowIndex].FindControl("lnkFileName");
               objComDocrepos.DeleteCompanyDocument(lblDocumentID.Text);
                objComDocrepos.SubmitChanges();
                objcommon.DeleteImageFromFolder(lnkFileName.Text, IncentexGlobal.documentpath);
              
                isdelete = true;
                if (isdelete == true)
                {
                    bindGriedView(Convert.ToInt32(Request.QueryString["id"].ToString()));
                    lblMessage.Text = "Selected Records Deleted Successfully ...";
                    Response.Redirect("ViewUploadDocument.aspx?id=" + Request.QueryString["id"]);

                }

            }
            
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

                bindGriedView(Convert.ToInt32(Request.QueryString["id"].ToString()));
            }
            

        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
           

        }
    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        bindGriedView(Convert.ToInt32(Request.QueryString["id"].ToString()));
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        bindGriedView(Convert.ToInt32(Request.QueryString["id"].ToString()));
    }
    protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
    protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindGriedView(Convert.ToInt32(Request.QueryString["id"].ToString()));
           
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



            DataList2.DataSource = dt;
            DataList2.DataBind();

        }
        catch (Exception)
        { }

    }
    protected void gvComDocumnet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            if (this.ViewState["SortOrder"].ToString() == "ASC")
                ImgSort.ImageUrl = "";
            else
                ImgSort.ImageUrl = "";


            switch (this.ViewState["SortExp"].ToString())
            {
                case "sLookupName":
                    PlaceHolder placeholderDocName = (PlaceHolder)e.Row.FindControl("placeholderDocName");
                    //placeholderDocName.Controls.Add(ImgSort);
                    break;
                case "FileName":
                    PlaceHolder placeholderFileName = (PlaceHolder)e.Row.FindControl("placeholderFileName");
                    //placeholderFileName.Controls.Add(ImgSort);
                    break;
               


            }

        }
    }
    /// <summary>
    /// lnkGMUpload_Click
    /// </summary>
    /// Nagmani Kumar 10/09/2010
    /// Open the Modal Dialog box for file upload.
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkGMUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            lblMsg.Text = string.Empty;
          
            icondiv.Visible = false;
            lblMessage.Text = string.Empty;
            ViewState["DocumentTypeID"] = hdnGuidelinesManuel.Value;
            modal.Controls.Clear();
            modal.Show();
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// lnkNewFiles_Click
    /// </summary>
    /// Nagmani Kumar 10/09/2010
    /// Open the Modal Dialog box for file upload.
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkNewFiles_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            lblMsg.Text = string.Empty;
            icondiv.Visible = false;
            lblMessage.Text = string.Empty;
            ViewState["DocumentTypeID"] = hdnNewsFiles.Value;
            modal.Controls.Clear();
            modal.Show();
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// lnkNDAAgreement_Click
    /// </summary>
    /// Nagmani Kumar 10/09/2010
    /// Open the Modal Dialog box for file upload.
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkNDAAgreement_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            lblMsg.Text = string.Empty;
           
            icondiv.Visible = false;
            lblMessage.Text = string.Empty;
            ViewState["DocumentTypeID"] = hdnNDAAgreement.Value;
            modal.Controls.Clear();
            modal.Show();
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// lnkOtherDocument_Click
    /// </summary>
    /// Nagmani Kumar 10/09/2010
    /// Open the Modal Dialog box for file upload.
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkOtherDocument_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            lblMsg.Text = string.Empty;
          
            icondiv.Visible = false;
            lblMessage.Text = string.Empty;
            ViewState["DocumentTypeID"] = hdnOtherDocuments.Value;
            modal.Controls.Clear();
            modal.Show();
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    protected void DownloadFile(string filepath)
    {
        System.IO.Stream iStream = null;

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

            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "inline; filename=" + filename);

            // Read the bytes.
            while (dataToRead > 0)
            {
                // Verify that the client is connected.
                if (Response.IsClientConnected)
                {
                    // Read the data in buffer.
                    length = iStream.Read(buffer, 0, 10000);

                    // Write the data to the current output stream.
                    Response.OutputStream.Write(buffer, 0, length);

                    // Flush the data to the HTML output.
                    Response.Flush();

                    buffer = new Byte[10000];
                    dataToRead = dataToRead - length;
                }
                else
                {
                    //prevent infinite loop if user disconnects
                    dataToRead = -1;
                }
            }
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
    /// populatesubmenu()
    ///This method is call when the Main menu item is Selected. 
    ///Nagmani Kumar
    /// </summary>
   
   
}
