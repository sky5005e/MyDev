using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using System.Text;

public partial class admin_ArtWorkDecoratingLibrary_AddArtwork : PageBase
{
    #region Data Member's
    ArtWorkRepository objArtWorkRepo = new ArtWorkRepository();
    /// <summary>
    /// Set unique number
    /// </summary>
    String uniqueNumber= String.Empty;
    
    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            base.MenuItem = "Artwork Library";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView || !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Add Artwork Library";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/ArtWorkDecoratingLibrary/ArtworkIndex.aspx";
            
            lblMsg.Visible = false;
            BindDropDowns();
        }

    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCompany.SelectedValue != "0")
        {
            String Comp = ddlCompany.SelectedItem.Text.Substring(0, 3).ToUpper();
        // to check unique number
        checkAgain:
            uniqueNumber = Comp + "-" + GenerateRandomNumber().ToString();
            if (objArtWorkRepo.HasRandomArtworkNumber(uniqueNumber))//If true then generate new Random Number
            {
                goto checkAgain;
            }

            lblArtworkNumber.Text = uniqueNumber.ToString();
        }
    }
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {

        if (!CheckFileSize())
        {
            lblMsg.Visible = true;
            lblMsg.Text = "The file you are uploading is more than 25MB.";
            return;
        }
        // get all file name in this string builder;
        StringBuilder Filename = new StringBuilder();
        #region Saving Attachments
        if (Request.Files.Count > 0)
        {
            HttpFileCollection Attachments = Request.Files;
            for (int i = 0; i < Attachments.Count; i++)
            {
                HttpPostedFile Attachment = Attachments[i];
                if (Attachment.ContentLength > 0 && !String.IsNullOrEmpty(Attachment.FileName))
                {
                    String SavedFileName = "Artwork_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Attachment.FileName;
                    SavedFileName = Common.MakeValidFileName(SavedFileName);
                    String filePath = Server.MapPath("../../UploadedImages/Artwork/Thumbs/") + SavedFileName;
                    Attachment.SaveAs(filePath);

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
        }
        #endregion

        ArtworkDecoratingLibrary objArt = new ArtworkDecoratingLibrary();
        objArt.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
        objArt.ArtworkName = txtArtworkName.Text;
        objArt.ArtworkNumber = Convert.ToString(lblArtworkNumber.Text);
        objArt.SAPReferenceTag = txtSAPReferenceTag.Text;
        objArt.ArtworkCreatedBy = Convert.ToInt64(ddlArtworkCreatedBy.SelectedValue);
        objArt.GarmentSizeApply = Convert.ToInt64(ddlGarmentSizeApply.SelectedValue);
        // to save file as per ext. 
        String[] fileext = Filename.ToString().Split(',');
        foreach (var item in fileext)
        {
            if (item.Contains(".jpg") || item.Contains(".jpeg"))
                objArt.ArtworkFilesJPG = item.ToString();
            else if (item.Contains(".pdf"))
                objArt.ArtworkFilesPDF = item.ToString();
            else if (item.Contains(".emb"))
                objArt.ArtworkFilesEMB = item.ToString();
            else if (item.Contains(".dst"))
                objArt.ArtworkFilesDST = item.ToString();
            else if (item.Contains(".eps"))
                objArt.ArtworkFilesEPS = item.ToString();
        }

        objArtWorkRepo.Insert(objArt);
        objArtWorkRepo.SubmitChanges();

        ResetControls();
        //Response.Redirect("ListArts.aspx?type=Artwork&req=1");
        lblMsg.Visible = true;
        lblMsg.Text = "Records sucessfully saved";
    }
    #endregion

    #region Methods
    /// <summary>
    /// To generate Random of 5 Digit.
    /// </summary>
    /// <returns></returns>
    private Int64 GenerateRandomNumber()
    {
        Random random = new Random();
        return random.Next(10000, 99999);
    }
    /// <summary>
    /// To Bind Drop Downs
    /// </summary>
    private void BindDropDowns()
    {
        CompanyRepository objRepo = new CompanyRepository();
        List<Company> objList = new List<Company>();
        objList = objRepo.GetAllCompany().OrderBy(c=>c.CompanyName).ToList();
        Common.BindDDL(ddlCompany, objList, "CompanyName", "CompanyID", "-Select-");

        LookupRepository objLookRep = new LookupRepository();
        ddlArtworkCreatedBy.DataSource = objLookRep.GetByLookup("ArtworkCreatedBy");
        ddlArtworkCreatedBy.DataValueField = "iLookupID";
        ddlArtworkCreatedBy.DataTextField = "sLookupName";
        ddlArtworkCreatedBy.DataBind();
        ddlArtworkCreatedBy.Items.Insert(0, new ListItem("-Select-", "0"));

        ddlGarmentSizeApply.DataSource = objLookRep.GetByLookup("GarmentSizeApply");
        ddlGarmentSizeApply.DataValueField = "iLookupID";
        ddlGarmentSizeApply.DataTextField = "sLookupName";
        ddlGarmentSizeApply.DataBind();
        ddlGarmentSizeApply.Items.Insert(0, new ListItem("-Select-", "0"));

    }

    /// <summary>
    /// To check size of file 
    /// </summary>
    /// <returns></returns>
    private Boolean CheckFileSize()
    {
        Int64 size = 0;
         HttpFileCollection Attachments = Request.Files;
         for (int i = 0; i < Attachments.Count; i++)
         {
             HttpPostedFile Attachment = Attachments[i];
             size +=Attachment.ContentLength;
         }
         if (size / 1048576 > 25)
             return false;
         else
             return true;
    }
    private void ResetControls()
    {
        ddlArtworkCreatedBy.SelectedIndex = 0;
        ddlGarmentSizeApply.SelectedIndex = 0;
        ddlCompany.SelectedIndex = 0;
        txtArtworkName.Text = "";
        txtSAPReferenceTag.Text = "";
        lblArtworkNumber.Text = "";
    }
    #endregion

}
