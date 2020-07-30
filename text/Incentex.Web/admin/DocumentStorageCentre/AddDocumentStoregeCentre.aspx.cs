using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.IO;

public partial class admin_DocumentStoregeCentre_AddDocumentStoregeCentre : PageBase
{
    #region Data Member's
    DocumentRepository objDocumentRepo = new DocumentRepository();
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

            if (!base.CanView && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            BindDropDowns();

            ((Label)Master.FindControl("lblPageHeading")).Text = "Add Document Storage Centre";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/DocumentStorageCentre/DocumentStorageCenter.aspx";
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        // get all file name in this string builder;
        StringBuilder Filename = new StringBuilder();

        #region Saving Attachments
        String SavedFileName = string.Empty;
        decimal FileSize = 0;
        string extension = string.Empty;
        if (Request.Files.Count > 0)
        {
            HttpFileCollection Attachments = Request.Files;

            HttpPostedFile Attachment = Attachments[0];
            if (Attachment.ContentLength > 0 && !String.IsNullOrEmpty(Attachment.FileName))
            {
                SavedFileName = ddldocumentType.SelectedItem + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Attachment.FileName;
                SavedFileName = Common.MakeValidFileName(SavedFileName);
                String filePath = Server.MapPath("../../UploadedImages/DocumentStorageCentre/") + SavedFileName;
                Attachment.SaveAs(filePath);
                FileSize = Attachment.ContentLength;
                extension = Path.GetExtension(Attachments[0].FileName).ToLower();

                if (Filename.ToString() == "")
                {
                    Filename.Append(SavedFileName);
                }
                else
                {
                    Filename.Append("," + SavedFileName);
                }
            }
        }
        #endregion

        DocumentStorageCentre objdocument = new DocumentStorageCentre();

        objdocument.FileName = txtFileName.Text;
        objdocument.OriginalFileName = SavedFileName;
        objdocument.extension = extension;
        objdocument.FileSize = FileSize;
        objdocument.SearchKeyWord = txtSearchKeyword.Text;
        objdocument.DocumentType = Convert.ToInt64(ddldocumentType.SelectedValue);
        objdocument.UplodedDate = System.DateTime.Now;
        objdocument.UplodedBy = IncentexGlobal.CurrentMember.UserInfoID;

        objDocumentRepo.Insert(objdocument);
        objDocumentRepo.SubmitChanges();

        ResetControls();
        Response.Redirect("DocumentStorageCenter.aspx?req=1");
    }
    #endregion

    #region Methods
    /// <summary>
    /// To generate Random of 7 Digit.
    /// </summary>
    /// <returns></returns>
    private Int64 GenerateRandomNumber()
    {
        Random random = new Random();
        return random.Next(1000000, 9999999);
    }

    /// <summary>
    /// To Bind Drop Downs
    /// </summary>
    private void BindDropDowns()
    {
        LookupRepository objLookRep = new LookupRepository();
        ddldocumentType.DataSource = objLookRep.GetByLookup("DocumentStorageType");
        ddldocumentType.DataValueField = "iLookupID";
        ddldocumentType.DataTextField = "sLookupName";
        ddldocumentType.DataBind();
        ddldocumentType.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    private void ResetControls()
    {
        ddldocumentType.SelectedIndex = 0;
        txtFileName.Text = "";
        txtSearchKeyword.Text = "";
    }
    #endregion
}
