﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using Incentex.DAL;
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;

public partial class admin_Supplier_SupplierDocuments : PageBase
{
    #region Properties

    Int64 SupplierEmployeeID
    {
        get
        {
            if (ViewState["SupplierEmployeeID"] == null)
            {
                ViewState["SupplierEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierEmployeeID"]);
        }
        set
        {
            ViewState["SupplierEmployeeID"] = value;
        }
    }

    Int64 SupplierID
    {
        get
        {
            if (ViewState["SupplierID"] == null)
            {
                ViewState["SupplierID"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierID"]);
        }
        set
        {
            ViewState["SupplierID"] = value;
        }
    }

    Int64 SocialSecurityCardId  /*lookup table has same name for sLookupName column  */
    {
        get
        {
            if (ViewState["SocialSecurityCard"] == null)
            {
                ViewState["SocialSecurityCard"] = 0;
            }
            return Convert.ToInt64(ViewState["SocialSecurityCard"]);
        }
        set
        {
            ViewState["SocialSecurityCard"] = value;
        }
    }

    Int64 W4I9DocumentsId
    {
        get
        {
            if (ViewState["W4I9Documents"] == null)
            {
                ViewState["W4I9Documents"] = 0;
            }
            return Convert.ToInt64(ViewState["W4I9Documents"]);
        }
        set
        {
            ViewState["W4I9Documents"] = value;
        }
    }

    Int64 ValidPhotoIDId
    {
        get
        {
            if (ViewState["ValidPhotoID"] == null)
            {
                ViewState["ValidPhotoID"] = 0;
            }
            return Convert.ToInt64(ViewState["ValidPhotoID"]);
        }
        set
        {
            ViewState["ValidPhotoID"] = value;
        }
    }

    Int64 SignedNDAAgreementId
    {
        get
        {
            if (ViewState["SignedNDAAgreement"] == null)
            {
                ViewState["SignedNDAAgreement"] = 0;
            }
            return Convert.ToInt64(ViewState["SignedNDAAgreement"]);
        }
        set
        {
            ViewState["SignedNDAAgreement"] = value;
        }
    }

    Int64 AnnualReviewId
    {
        get
        {
            if (ViewState["AnnualReview"] == null)
            {
                ViewState["AnnualReview"] = 0;
            }
            return Convert.ToInt64(ViewState["AnnualReview"]);
        }
        set
        {
            ViewState["AnnualReview"] = value;
        }
    }

    Int64 OtherDocumentsId
    {
        get
        {
            if (ViewState["OtherDocuments"] == null)
            {
                ViewState["OtherDocuments"] = 0;
            }
            return Convert.ToInt64(ViewState["OtherDocuments"]);
        }
        set
        {
            ViewState["OtherDocuments"] = value;
        }
    }

    string SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = "FileType";
            }
            return ViewState["SortExp"].ToString();
        }
        set
        {
            ViewState["SortExp"] = value;
        }
    }

    string SortDir
    {
        get
        {
            if (ViewState["SortDir"] == null)
            {
                ViewState["SortDir"] = "ASC";
            }
            return ViewState["SortDir"].ToString();
        }
        set
        {
            ViewState["SortDir"] = value;
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
    public int CurrentPageAdditional
    {
        get
        {
            if (this.ViewState["CurrentPageAdditional"] == null)
                return 0;
            else
                return Convert.ToInt16(this.ViewState["CurrentPageAdditional"].ToString());
        }
        set
        {
            this.ViewState["CurrentPageAdditional"] = value;
        }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Employee";
            base.ParentMenuID = 18;

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
                if (Request.QueryString["SubId"] != null)
                {
                    this.SupplierEmployeeID = Convert.ToInt64(Request.QueryString.Get("SubId"));
                }
                if (Request.QueryString["id"] != "0")
                {
                    this.SupplierID = Convert.ToInt64(Request.QueryString.Get("id"));
                    // txtCompany.Text = Convert.ToString(objSupplierRepos.GetById(this.SupplierID).CompanyName);
                }
                ((Label)Master.FindControl("lblPageHeading")).Text = "Supplier Documents";
                //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to Employee listing</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Supplier/ViewAddSupplierEmployee.aspx?id=" + this.SupplierID;
                menuControl.PopulateMenu(3, 1, this.SupplierID, this.SupplierEmployeeID, true);

            }
            else
            {
                Response.Redirect("ViewSupplier.aspx");
            }
            SetDocumentTypeId();
            gvSetupDocuments.DataBind();
            gvAdditional.DataBind();
        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        Response.Redirect("SupplierPaymentInfo.aspx?id=" + Convert.ToInt32(this.SupplierID.ToString()) + "&SubId=" + Convert.ToInt32(this.SupplierEmployeeID.ToString()));
    }

    #region Setup Documents

    protected void gvSetupDocuments_DataBinding(object sender, EventArgs e)
    {
        //get documents 
        SupplierDocumentRepository objRepo = new SupplierDocumentRepository();
        List<SupplierDocument> objSocialSecurityCardList = objRepo.GetByDocumentTypeId(this.SocialSecurityCardId, this.SupplierEmployeeID);
        List<SupplierDocument> objW4I9List = objRepo.GetByDocumentTypeId(this.W4I9DocumentsId, this.SupplierEmployeeID);
        List<SupplierDocument> objValidPhotoList = objRepo.GetByDocumentTypeId(this.ValidPhotoIDId, this.SupplierEmployeeID);
        List<SupplierDocument> objSignedNDAAgreementList = objRepo.GetByDocumentTypeId(this.SignedNDAAgreementId, this.SupplierEmployeeID);

        List<SupplierDocument> objList = new List<SupplierDocument>();
        objList.AddRange(objSocialSecurityCardList);
        objList.AddRange(objW4I9List);
        objList.AddRange(objValidPhotoList);
        objList.AddRange(objSignedNDAAgreementList);



        DataTable dt = Common.ListToDataTable(objList);

        //add Document type column

        dt.Columns.Add("FileType");

        foreach (DataRow dr in dt.Rows)
        {
            LookupRepository objLookupRepository = new LookupRepository();
            INC_Lookup objLookup = objLookupRepository.GetById(Convert.ToInt64(dr["DocumentTypeID"]));

            dr["FileType"] = objLookup.sLookupName;
        }

        dt.DefaultView.Sort = this.SortExp + " " + this.SortDir;
        pds.DataSource = dt.DefaultView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;

        gvSetupDocuments.DataSource = pds;

        doPaging();
    }

    protected void gvSetupDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "del":
                try
                {
                    if (!base.CanDelete)
                    {
                        base.RedirectToUnauthorised();
                    }

                    HiddenField hdnFileName = ((HiddenField)((DataControlFieldCell)((ImageButton)e.CommandSource).Parent).FindControl("hdnFileName"));
                    string PathAndName = Common.SupplierEmployeeDocumentPath + hdnFileName.Value;

                    //delete file
                    Common.DeleteFile(PathAndName);

                    Int64 DocumentId = Convert.ToInt64(e.CommandArgument);
                    SupplierDocumentRepository objRepo = new SupplierDocumentRepository();
                    objRepo.Delete(DocumentId);
                    gvSetupDocuments.DataBind();
                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex);
                }

                break;
            case "Sort":
                if (this.SortExp == e.CommandArgument.ToString())
                {
                    if (this.SortDir == "ASC")
                        this.SortDir = "DESC";
                    else
                        this.SortDir = "ASC";

                }
                else
                {
                    this.SortDir = "ASC";
                    this.SortExp = e.CommandArgument.ToString();
                }
                gvSetupDocuments.DataBind();
                break;
        }
    }

    protected void gvSetupDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


        }
    }

    void SetDocumentTypeId()
    {
        LookupRepository objLookupRepository = new LookupRepository();

        // get DocumentTypeId from lookup list
        SocialSecurityCardId = objLookupRepository.GetIncentexEmployeeDocumentLookUpId(LookupRepository.IncentexEmployeeSetupDocumentType.SocialSecurityCard);
        W4I9DocumentsId = objLookupRepository.GetIncentexEmployeeDocumentLookUpId(LookupRepository.IncentexEmployeeSetupDocumentType.W4I9Documents);
        ValidPhotoIDId = objLookupRepository.GetIncentexEmployeeDocumentLookUpId(LookupRepository.IncentexEmployeeSetupDocumentType.ValidPhotoID);
        SignedNDAAgreementId = objLookupRepository.GetIncentexEmployeeDocumentLookUpId(LookupRepository.IncentexEmployeeSetupDocumentType.SignedNDAAgreement);

        //get for Additional Info
        AnnualReviewId = objLookupRepository.GetIncentexEmployeeDocumentLookUpId(LookupRepository.IncentexEmployeeAdditionalDocumentType.AnnualReview);
        OtherDocumentsId = objLookupRepository.GetIncentexEmployeeDocumentLookUpId(LookupRepository.IncentexEmployeeAdditionalDocumentType.OtherDocuments);

    }

    void OpenPopUp(Int64 DocTypeId)
    {
        modal.X = Convert.ToInt32(Convert.ToDecimal(hfX.Value)) - 200;
        modal.Y = Convert.ToInt32(Convert.ToDecimal(hfY.Value)) - 120;
        btnUpload.CommandArgument = DocTypeId.ToString();
        modal.Show();

    }

    protected void lnkSocialSecurityCard_Click(object sender, EventArgs e)
    {
        OpenPopUp(this.SocialSecurityCardId);
    }


    protected void lnkW4I9Documents_Click(object sender, EventArgs e)
    {
        OpenPopUp(this.W4I9DocumentsId);
    }


    protected void lnkValidPhotoID_Click(object sender, EventArgs e)
    {
        OpenPopUp(this.ValidPhotoIDId);
    }


    protected void lnkSignedNDAAgreement_Click(object sender, EventArgs e)
    {
        OpenPopUp(this.SignedNDAAgreementId);
    }

    PagedDataSource pds = new PagedDataSource();
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

            lstPaging.DataSource = dt;
            lstPaging.DataBind();

        }
        catch (Exception)
        { }
    }


    protected void lstPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            gvSetupDocuments.DataBind();
            //bindGridView();
        }
    }
    protected void lstPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        gvSetupDocuments.PageIndex = CurrentPage;
        gvSetupDocuments.DataBind();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        gvSetupDocuments.PageIndex = CurrentPage;
        gvSetupDocuments.DataBind();
    }

    protected void gvSetupDocuments_DataBound(object sender, EventArgs e)
    {
        if (gvSetupDocuments.Rows.Count == 0)
        {
            lnkbtnPrevious.Visible = false;
            lnkbtnNext.Visible = false;
            lstPaging.Visible = false;
        }
        else
        {
            lnkbtnPrevious.Visible = true;
            lnkbtnNext.Visible = true;
            lstPaging.Visible = true;
        }
    }

    #endregion


    #region Aditional Documents

    protected void lnkAnnualReview_Click(object sender, EventArgs e)
    {
        OpenPopUp(this.AnnualReviewId);
    }

    protected void lnkOtherDocuments_Click(object sender, EventArgs e)
    {
        OpenPopUp(this.OtherDocumentsId);
    }

    PagedDataSource pdsAdditional = new PagedDataSource();
    protected void gvAdditional_DataBinding(object sender, EventArgs e)
    {
        //get documents 
        SupplierDocumentRepository objRepo = new SupplierDocumentRepository();
        List<SupplierDocument> objAnnualReviewList = objRepo.GetByDocumentTypeId(this.AnnualReviewId, this.SupplierEmployeeID);
        List<SupplierDocument> objOtherDocumentsList = objRepo.GetByDocumentTypeId(this.OtherDocumentsId, this.SupplierEmployeeID);

        List<SupplierDocument> objList = new List<SupplierDocument>();
        objList.AddRange(objAnnualReviewList);
        objList.AddRange(objOtherDocumentsList);


        DataTable dt = Common.ListToDataTable(objList);

        //add Document type column

        dt.Columns.Add("FileType");

        foreach (DataRow dr in dt.Rows)
        {
            LookupRepository objLookupRepository = new LookupRepository();
            INC_Lookup objLookup = objLookupRepository.GetById(Convert.ToInt64(dr["DocumentTypeID"]));

            dr["FileType"] = objLookup.sLookupName;
        }

        dt.DefaultView.Sort = this.SortExp + " " + this.SortDir;

        pdsAdditional.DataSource = dt.DefaultView;
        pdsAdditional.AllowPaging = true;
        pdsAdditional.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pdsAdditional.CurrentPageIndex = CurrentPage;

        lnkbtnNextAdditional.Enabled = !pdsAdditional.IsLastPage;
        lnkbtnPreviousAdditional.Enabled = !pdsAdditional.IsFirstPage;

        gvAdditional.DataSource = pdsAdditional;

        doPagingAdditional();
    }

    protected void gvAdditional_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "del":
                try
                {
                    if (!base.CanDelete)
                    {
                        base.RedirectToUnauthorised();
                    }

                    HiddenField hdnFileName = ((HiddenField)((DataControlFieldCell)((ImageButton)e.CommandSource).Parent).FindControl("hdnFileName"));
                    string PathAndName = Common.SupplierEmployeeDocumentPath + hdnFileName.Value;

                    //delete file
                    Common.DeleteFile(PathAndName);

                    Int64 DocumentId = Convert.ToInt64(e.CommandArgument);
                    DocumentRepository objRepo = new DocumentRepository();
                    objRepo.Delete(DocumentId);
                    gvAdditional.DataBind();
                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex);
                }

                break;
            case "Sort":
                if (this.SortExp == e.CommandArgument.ToString())
                {
                    if (this.SortDir == "ASC")
                        this.SortDir = "DESC";
                    else
                        this.SortDir = "ASC";

                }
                else
                {
                    this.SortDir = "ASC";
                    this.SortExp = e.CommandArgument.ToString();
                }
                gvAdditional.DataBind();
                break;
        }
    }
    protected void gvAdditional_DataBound(object sender, EventArgs e)
    {
        if (gvAdditional.Rows.Count == 0)
        {
            lnkbtnNextAdditional.Visible = false;
            lnkbtnPreviousAdditional.Visible = false;
            lstAdditional.Visible = false;
        }
        else
        {
            lnkbtnNextAdditional.Visible = true;
            lnkbtnPreviousAdditional.Visible = true;
            lstAdditional.Visible = true;
        }
    }

    private void doPagingAdditional()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PageIndex");
        dt.Columns.Add("PageText");

        for (int i = 0; i < pdsAdditional.PageCount; i++)
        {
            DataRow dr = dt.NewRow();
            dr[0] = i;
            dr[1] = i + 1;
            dt.Rows.Add(dr);
        }

        lstAdditional.DataSource = dt;
        lstAdditional.DataBind();
    }

    protected void lnkbtnPreviousAdditional_Click(object sender, EventArgs e)
    {
        CurrentPageAdditional -= 1;
        gvAdditional.PageIndex = CurrentPageAdditional;
        gvAdditional.DataBind();
    }

    protected void lnkbtnNextAdditional_Click(object sender, EventArgs e)
    {
        CurrentPageAdditional += 1;
        gvAdditional.PageIndex = CurrentPage;
        gvAdditional.DataBind();
    }


    #endregion

    /// <summary>
    /// upload document
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        string UploadPathAndName = "";
        lblMsg.Text = "";
        Int64 DocumentTypeID = Convert.ToInt64(btnUpload.CommandArgument);

        if (flu.HasFile == true || flu.FileName != string.Empty)
        {
            try
            {
                SupplierDocumentRepository objRepo = new SupplierDocumentRepository();
                string OriginalFileName = "";
                string FileName = "";

                FileInfo objFileInfo = new FileInfo(flu.FileName);
                OriginalFileName = objFileInfo.Name;

                SupplierDocument objExist = objRepo.GetByTypeAndName(DocumentTypeID, OriginalFileName);
                if (objExist != null)
                {
                    lblMsg.Text = "Document already exist ...";
                    //modal.Show();
                    return;
                }

                FileName = OriginalFileName.Split('.')[0].Replace(" ", "_") + "_" + this.SupplierEmployeeID.ToString() + "_" + DocumentTypeID.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyhhss") + objFileInfo.Extension;

                SupplierDocument obj = new SupplierDocument();
                obj.CreatedDate = DateTime.Now;
                obj.DocumentFor = DAEnums.DocumentForType.SupplierEmployee.ToString();
                obj.DocumentTypeID = DocumentTypeID;
                obj.OriginalFileName = OriginalFileName;
                obj.FileName = FileName;
                obj.ForeignKey = this.SupplierEmployeeID;

                UploadPathAndName = Common.SupplierEmployeeDocumentPath + FileName;
                flu.SaveAs(UploadPathAndName);

                objRepo.Insert(obj);
                objRepo.SubmitChanges();

                gvSetupDocuments.DataBind();
                gvAdditional.DataBind();

            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error in uploading document ...";
                ErrHandler.WriteError(ex);
                Common.DeleteFile(UploadPathAndName);

            }
        }

    }


}
