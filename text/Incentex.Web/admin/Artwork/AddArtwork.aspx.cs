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

public partial class admin_Artwork_AddArtwork : PageBase
{
    #region Data Member's
    ArtWorkRepository objArtWorkRepo = new ArtWorkRepository();
    /// <summary>
    /// Set unique number
    /// </summary>
    Int64 uniqueNumber=0;
    
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
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Artwork/ListArts.aspx?type=Artwork";
            // to check unique number
            uniqueNumber = GenerateRandomNumber();

            while (objArtWorkRepo.HasRandomArtDesignNumber(uniqueNumber))
            {
                uniqueNumber = GenerateRandomNumber();    
            }
            lblArtworkDesignNumber.Text = GenerateRandomNumber().ToString();
            lblMsg.Visible = false;
            BindDropDowns();
        }

    }
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {

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

        ArtworkLibrary objArt = new ArtworkLibrary();

        objArt.ArtworkName = txtArtworkName.Text;
        objArt.ArtDesignNumber = Convert.ToInt64(lblArtworkDesignNumber.Text);
        objArt.ArtworkFor = Convert.ToInt64(ddlArtworkFor.SelectedValue);
        objArt.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
        objArt.SpecialNotes = txtSpecialNotes.Text;
        objArt.ArtworkFiles = Filename.ToString();

        objArtWorkRepo.Insert(objArt);
        objArtWorkRepo.SubmitChanges();

        ResetControls();
        Response.Redirect("ListArts.aspx?type=Artwork&req=1");
        //lblMsg.Visible = true;
        //lblMsg.Text = "Records sucessfully saved";
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
        CompanyRepository objRepo = new CompanyRepository();
        List<Company> objList = new List<Company>();
        objList = objRepo.GetAllCompany().OrderBy(c=>c.CompanyName).ToList();
        Common.BindDDL(ddlCompany, objList, "CompanyName", "CompanyID", "-Select-");

        LookupRepository objLookRep = new LookupRepository();

        ddlArtworkFor.DataSource = objLookRep.GetByLookup("ArtworkLibrary");
        ddlArtworkFor.DataValueField = "iLookupID";
        ddlArtworkFor.DataTextField = "sLookupName";
        ddlArtworkFor.DataBind();
        ddlArtworkFor.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    private void ResetControls()
    {
        ddlArtworkFor.SelectedIndex = 0;
        ddlCompany.SelectedIndex = 0;
        txtArtworkName.Text = "";
        txtSpecialNotes.Text = "";
    }
    #endregion

}
