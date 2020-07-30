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
using System.Xml.Linq;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using System.Collections.Generic;
using System.IO;

public partial class admin_Supplier_FactoryOrPersonalInfo : PageBase
{
    #region Properties

    Int64 SupplierId
    {
        get
        {
            if (ViewState["SupplierId"] == null)
            {
                ViewState["SupplierId"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierId"]);
        }
        set
        {
            ViewState["SupplierId"] = value;
        }
    }

    public Int64 DocTypeId_SupplierListVacationAndSupplierClosing
    {
        get
        {
            if (ViewState["DocTypeId_SupplierListVacationAndSupplierClosing"] == null)
            {
                ViewState["DocTypeId_SupplierListVacationAndSupplierClosing"] = 0;
            }
            return Convert.ToInt64(ViewState["DocTypeId_SupplierListVacationAndSupplierClosing"]);
        }
        set
        {
            ViewState["DocTypeId_SupplierListVacationAndSupplierClosing"] = value;
        }
    }


    public Int64 DocTypeId_SupplierMasterPriceOfferList
    {
        get
        {
            if (ViewState["DocTypeId_SupplierMasterPriceOfferList"] == null)
            {
                ViewState["DocTypeId_SupplierMasterPriceOfferList"] = 0;
            }
            return Convert.ToInt64(ViewState["DocTypeId_SupplierMasterPriceOfferList"]);
        }
        set
        {
            ViewState["DocTypeId_SupplierMasterPriceOfferList"] = value;
        }
    }


    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Factory/Personal Info";
            base.ParentMenuID = 18;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Factory / Personal Info";
           // ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to Supplier listing</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Supplier/ViewSupplier.aspx";
          
            if (Request.QueryString.Count > 0)
            {
                this.SupplierId = Convert.ToInt64(Request.QueryString.Get("Id"));
                manuControl.PopulateMenu(2, 0, this.SupplierId, 0, false);

            }
            else
            {
                Response.Redirect("~/admin/Supplier/ViewSupplier.aspx");
            }

            if (this.SupplierId == 0)
            {
                Response.Redirect("~/admin/Supplier/MainCompanyContact.aspx?Id=0");
            }
            SetDocumentTypeId();
            DisplayData();
            DisplayNotes();
            gvDocumnetVacations.DataBind();
            gvDocumnetMasterPrice.DataBind();
        }
    }

    void SetDocumentTypeId()
    {
        LookupRepository objLookupRepository = new LookupRepository();

        // get DocumentTypeId from lookup list
        DocTypeId_SupplierListVacationAndSupplierClosing = objLookupRepository.GetSupplierDocumentLookUpId(LookupRepository.SupplierDocumentsType.SupplierListVacationAndSupplierClosing);
        DocTypeId_SupplierMasterPriceOfferList = objLookupRepository.GetSupplierDocumentLookUpId(LookupRepository.SupplierDocumentsType.SupplierMasterPriceOfferList);


    }

    /// <summary>
    /// Display Supplier data
    /// </summary>
    void DisplayData()
    {
        SupplierRepository objSupplierRepository = new SupplierRepository();

        Supplier obj = objSupplierRepository.GetById(this.SupplierId);

        if (obj != null)
        {
            txtHoursOfOperation.Text = obj.FactoryHoursInformation;
            txtAnnualPriceOfferReviewDate.Text = Common.GetDateString(obj.AnnualPriceOfferReviewDate);
            txtSupplierAccountNumber.Text = obj.SupplierAccountNumber;
            txtOurAccountNumber.Text = obj.InternalAccountNumber;
            txtBirthday.Text = Common.GetDateString(obj.SupplierBirthDay);
            txtSpousesName.Text = obj.SpouseName;
            txtChildrensName.Text = obj.ChildrenName;
        }
    }
    
    /// <summary>
    /// Update Supplier 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            SupplierRepository objSupplierRepository = new SupplierRepository();

            Supplier obj = objSupplierRepository.GetById(this.SupplierId);

            obj.FactoryHoursInformation = txtHoursOfOperation.Text;
            obj.AnnualPriceOfferReviewDate = Common.GetDate(txtAnnualPriceOfferReviewDate);
            obj.SupplierAccountNumber = txtSupplierAccountNumber.Text;
            obj.InternalAccountNumber = txtOurAccountNumber.Text;
            obj.SupplierBirthDay = Common.GetDate(txtBirthday);
            obj.SpouseName = txtSpousesName.Text;
            obj.ChildrenName = txtChildrensName.Text;


            objSupplierRepository.SubmitChanges();

            lblMsg.Text = "Record Saved Successfully ...";
            Response.Redirect("ViewAddSupplierEmployee.aspx?Id=" + this.SupplierId, false);
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch(Exception ex)
        {
            lblMsg.Text = "Error in saving record ...";
            ErrHandler.WriteError(ex);
        }
    }

    
    /// <summary>
    /// Save Notes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkSaveNote_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtNote.Text))
        {
            return;
        }

        try
        {
            lblMsg.Text = "";
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            NoteDetail obj = new NoteDetail()
            {
                ForeignKey = this.SupplierId,
                Notecontents = txtNote.Text
                ,
                NoteFor = DAEnums.GetNoteForTypeName(DAEnums.NoteForType.Supplier)
                ,
                CreatedBy = IncentexGlobal.CurrentMember.UserInfoID 
                ,
                CreateDate = DateTime.Now
                ,
                UpdateDate = DateTime.Now
                ,
                UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID
            };

            objRepo.Insert(obj);
            objRepo.SubmitChanges();
            txtNote.Text = "";
            DisplayNotes();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in saving record ...";
            ErrHandler.WriteError(ex);
        }
    }


    /// <summary>
    /// Display Note history
    /// </summary>
    void DisplayNotes()
    {
        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        List<NoteDetail> objList = objRepo.GetByForeignKeyId(this.SupplierId , DAEnums.NoteForType.Supplier);
        txtNoteHistory.Text = "";
        foreach (NoteDetail obj in objList)
        {
            txtNoteHistory.Text += "Created Date : " + Convert.ToDateTime(obj.CreateDate).ToString("dd-MMM-yyyy") + "\n";
            txtNoteHistory.Text += "Created Time : " + Convert.ToDateTime(obj.CreateDate).ToString("hh:mm tt") + "\n";

            UserInformationRepository objUserRepo = new UserInformationRepository();
            UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);

            if (objUser != null)
            {
                txtNoteHistory.Text += "Created By : " + objUser.FirstName + " " + objUser.LastName + "\n";
            }

            txtNoteHistory.Text += "Note : " + obj.Notecontents + "\n";
            txtNoteHistory.Text += "-----------------------------------------------------------------------------------------------------------------------\n";

        }

    }

    /// <summary>
    /// Open pop up for note
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkAddNote_Click(object sender, EventArgs e)
    {  
        modalAddnotes.Show();
    }


    #region List Vacations & Supplier Closings

    /// <summary>
    /// Open pop up for List Vacations & Supplier Closings
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkVacations_Click(object sender, EventArgs e)
    {
        modalVacations.Show();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (fluDocument.HasFile == true || fluDocument.FileName != string.Empty)
        {

            try
            {

                DocumentRepository objRepo = new DocumentRepository();
                string OriginalFileName = "";
                string FileName = "";

                FileInfo objFileInfo = new FileInfo(fluDocument.FileName);
                OriginalFileName = objFileInfo.Name;

                FileName = OriginalFileName.Split('.')[0].Replace(" ", "_") + "_" + this.SupplierId.ToString() + "_" + this.DocTypeId_SupplierListVacationAndSupplierClosing.ToString() + "_" + LookupRepository.SupplierDocumentsType.SupplierListVacationAndSupplierClosing.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyhhss") + objFileInfo.Extension;

                Document obj = new Document();
                obj.CreatedDate = DateTime.Now;
                obj.DocumentFor = DAEnums.DocumentForType.Supplier.ToString();
                obj.DocumentTypeID = this.DocTypeId_SupplierListVacationAndSupplierClosing;
                obj.OriginalFileName = OriginalFileName;
                obj.FileName = FileName;
                obj.ForeignKey = this.SupplierId;

                string UploadPathAndName = Common.SupplierDocumentUploadPath + FileName;
                fluDocument.SaveAs(UploadPathAndName);

                objRepo.Insert(obj);
                objRepo.SubmitChanges();

                gvDocumnetVacations.DataBind();
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
        }
    }

    /// <summary>
    /// Event to bind data to grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDocumnetVacations_DataBinding(object sender, EventArgs e)
    {
        Int64 DocumentTypeId = this.DocTypeId_SupplierListVacationAndSupplierClosing;

        //get document List
        DocumentRepository objRepo = new DocumentRepository();

        List<Document> objList = objRepo.GetByDocumentTypeId(DocumentTypeId,this.SupplierId);
        gvDocumnetVacations.DataSource = objList;
    }


    /// <summary>
    /// Handle different events from grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDocumnetVacations_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "del":
                try
                {
                    HiddenField hdnFileName = ((HiddenField)((DataControlFieldCell)((ImageButton)e.CommandSource).Parent).FindControl("hdnFileName"));
                    string PathAndName = Common.SupplierDocumentUploadPath + hdnFileName.Value;

                    //delete file
                    Common.DeleteFile(PathAndName);

                    Int64 DocumentId = Convert.ToInt64(e.CommandArgument);
                    DocumentRepository objRepo = new DocumentRepository();
                    objRepo.Delete(DocumentId);
                    gvDocumnetVacations.DataBind();
                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex);
                }

                break;
         

        }
    }



    #endregion

    #region Upload Master Price Offer List

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkMasterPrice_Click(object sender, EventArgs e)
    {
       
        modalMasterPrice.Show();
    }

    protected void gvDocumnetMasterPrice_DataBinding(object sender, EventArgs e)
    {
        Int64 DocumentTypeId = this.DocTypeId_SupplierMasterPriceOfferList;

        //get document List
        DocumentRepository objRepo = new DocumentRepository();

        List<Document> objList = objRepo.GetByDocumentTypeId(DocumentTypeId, this.SupplierId);
        gvDocumnetMasterPrice.DataSource = objList;
  
    }

    protected void gvDocumnetMasterPrice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "del":
                try
                {
                    HiddenField hdnFileName = ((HiddenField)((DataControlFieldCell)((ImageButton)e.CommandSource).Parent).FindControl("hdnFileName"));
                    string PathAndName = Common.SupplierDocumentUploadPath + hdnFileName.Value;

                    //delete file
                    Common.DeleteFile(PathAndName);

                    Int64 DocumentId = Convert.ToInt64(e.CommandArgument);
                    DocumentRepository objRepo = new DocumentRepository();
                    objRepo.Delete(DocumentId);
                    gvDocumnetMasterPrice.DataBind();
                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex);
                }

                break;

        }
    }

    protected void btnUploadMasterPrice_Click(object sender, EventArgs e)
    {
        if (fluMasterPrice.HasFile == true || fluMasterPrice.FileName != string.Empty)
        {
            try
            {
                DocumentRepository objRepo = new DocumentRepository();
                string OriginalFileName = "";
                string FileName = "";

                FileInfo objFileInfo = new FileInfo(fluMasterPrice.FileName);
                OriginalFileName = objFileInfo.Name;

                FileName = OriginalFileName.Split('.')[0].Replace(" ", "_") + "_" + this.SupplierId.ToString() + "_" + this.DocTypeId_SupplierMasterPriceOfferList.ToString() + "_" + LookupRepository.SupplierDocumentsType.SupplierMasterPriceOfferList.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyhhss") + objFileInfo.Extension;

                Document obj = new Document();
                obj.CreatedDate = DateTime.Now;
                obj.DocumentFor = DAEnums.DocumentForType.Supplier.ToString();
                obj.DocumentTypeID = this.DocTypeId_SupplierMasterPriceOfferList;
                obj.OriginalFileName = OriginalFileName;
                obj.FileName = FileName;
                obj.ForeignKey = this.SupplierId;

                string UploadPathAndName = Common.SupplierDocumentUploadPath + FileName;
                fluMasterPrice.SaveAs(UploadPathAndName);

                objRepo.Insert(obj);
                objRepo.SubmitChanges();

                gvDocumnetMasterPrice.DataBind();
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
        }
    }

    #endregion


    
}//class
