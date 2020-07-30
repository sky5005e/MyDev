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
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL;
using Incentex.DA;
using Incentex.BE;

public partial class admin_Artwork_AddArtworkImage : PageBase
{
    string sFilePath, sFilePathLarge;
    string sFileName, sFileNameLarge,sOThumgImageName,sOLargeImageName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Image Library";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView || !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Add Image Library";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Artwork/ListArts.aspx";
            BindValues();
         
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void BindValues()
    {
        CompanyRepository objRepo = new CompanyRepository();
        List<Company> objList = new List<Company>();
        objList = objRepo.GetAllCompany();
        Common.BindDDL(ddlCompany, objList, "CompanyName", "CompanyID", "-select customer-");

        LookupDA sEU = new LookupDA();
        LookupBE sEUBE = new LookupBE();
        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "lnkFileCategory";

        DataSet dsSu = sEU.LookUp(sEUBE); 
      
        ddlStoreStatus.DataSource = dsSu;
        ddlStoreStatus.DataTextField = "sLookupName";
        ddlStoreStatus.DataValueField = "iLookupID";
       
        ddlStoreStatus.DataBind();  ddlStoreStatus.Items.Insert(0, new ListItem("-select file category-", "0"));
        


    }
    private Int64 GenerateRandomNumber()
    {
        Random random = new Random();
        return random.Next(1000000, 9999999);
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            CompanyArtwork ca = new CompanyArtwork();
            ArtWorkRepository caRep = new ArtWorkRepository();
            if (fpUpload.Value != "")
            {
                sFileName = "Thumb_" + System.DateTime.Now.Ticks + "_" + fpUpload.Value;
                string[] thumb = fpUpload.Value.Split('.');
                sOThumgImageName = thumb[0];
                sFilePath = Server.MapPath("../../UploadedImages/Artwork/Thumbs/") + sFileName;
                Request.Files[0].SaveAs(sFilePath);
            }
            if (fpUploadLargeImage.Value != "")
            {
                string[] large = fpUploadLargeImage.Value.Split('.');
                sOLargeImageName = large[0];
                sFileNameLarge = "Large_" + System.DateTime.Now.Ticks + "_" + fpUploadLargeImage.Value;
                sFilePathLarge = Server.MapPath("../../UploadedImages/Artwork/Large/") + sFileNameLarge;
                Request.Files[1].SaveAs(sFilePathLarge);
            }
            ca.CustomerId  = Convert.ToInt64(ddlCompany.SelectedItem.Value);
            ca.FileCategoryId = Convert.ToInt64(ddlStoreStatus.SelectedItem.Value);

            //Thumb Image..system name and original name
            ca.ThumbImageOName = sOThumgImageName;
            ca.ThumbImageSName = sFileName;

            //Larger Image..System name and original name
            ca.LargerImageOName = sOLargeImageName;
            ca.LargerImageSName = sFileNameLarge;
            

            caRep.Insert(ca);
            caRep.SubmitChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
