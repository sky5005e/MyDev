using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Artwork_ViewLargerImage : PageBase
{
    #region Properties

    StoreProductImageRepository objRepos = new StoreProductImageRepository();

    Int64 ArtWorkID
    {
        get
        {
            if (ViewState["ArtWorkID"] != null)
                return Convert.ToInt64(ViewState["ArtWorkID"]);
            else
                return 0;
        }
        set
        {
            ViewState["ArtWorkID"] = value;
        }
    }

    Int64 StoreProductImageID
    {
        get
        {
            if (ViewState["StoreProductImageID"] != null)
                return Convert.ToInt64(ViewState["StoreProductImageID"]);
            else
                return 0;
        }
        set
        {
            ViewState["StoreProductImageID"] = value;
        }
    }

    String ImagePath
    {
        get
        {
            if (ViewState["ImagePath"] != null)
                return ViewState["ImagePath"].ToString();
            else
                return null;
        }
        set
        {
            ViewState["ImagePath"] = value;
        }
    }

    String ImageName
    {
        get
        {
            if (ViewState["ImageName"] != null)
                return ViewState["ImageName"].ToString();
            else
                return null;
        }
        set
        {
            ViewState["ImageName"] = value;
        }
    }

    String BackURL
    {
        get
        {
            if (ViewState["BackURL"] != null)
                return ViewState["BackURL"].ToString();
            else
                return null;
        }
        set
        {
            ViewState["BackURL"] = value;
        }
    }

    #endregion 

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {   
            ((Label)Master.FindControl("lblPageHeading")).Text = "Larger Image";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";

            if (Session["ArtImagrReturnURL"] != null)
            {
                this.BackURL = ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = Session["ArtImagrReturnURL"].ToString();
            }
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "0")
            {
                this.ArtWorkID = Convert.ToInt64(Request.QueryString.Get("id"));
                if (!String.IsNullOrEmpty(Request.QueryString["src"]))
                {
                    this.ImagePath ="~/UploadedImages/Artwork/Thumbs/"+ Request.QueryString["src"];
                    ImageName = new ArtWorkRepository().GetArtworkById(this.ArtWorkID).ArtworkName;
                }
                SetImageAttributes();
                lblDelete.Visible = true;
            }
            else
            {
                lblDelete.Visible = false;
                this.StoreProductImageID = Convert.ToInt64(Request.QueryString.Get("ProductId"));
                SetProductImageAttributes();
            }
        }
    }

    public void SetImageAttributes()
    {
        if (!String.IsNullOrEmpty(this.ImagePath))
            imgProduct.Src = "~/controller/createthumb.aspx?_ty=artImages&_path=" + Request.QueryString["src"] + "&_twidth=400&_theight=600";
        else
            imgProduct.Src = "~/controller/createthumb.aspx?_ty=artImagesLarge&_path=" + new ArtWorkRepository().GetById(this.ArtWorkID).LargerImageSName + "&_twidth=400&_theight=600";        
    }

    public void SetProductImageAttributes()
    {
        List<StoreProductImage> objList = objRepos.GetStoreProductImagesByStoreProductImageID(this.StoreProductImageID);
        if (objList.Count > 0)
        {
            ImagePath = "~/UploadedImages/ProductImages/" + objList[0].LargerProductImage;
            ImageName = objList[0].LargerProductImage;
            imgProduct.Src = ImagePath;
        }        
    }

    protected void lblDelete_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 artworkid = Convert.ToInt64(this.ArtWorkID);
            ArtWorkRepository rp = new ArtWorkRepository();
            CompanyArtwork cartwork = rp.GetById(artworkid);

            //Delete stored files
            if (cartwork.ThumbImageSName != String.Empty)
            {
                String ThumbName = Server.MapPath("~/UploadedImages/Artwork/Thumbs/") + cartwork.ThumbImageSName;
                Common.DeleteFile(ThumbName);
            }

            if (cartwork.LargerImageSName != String.Empty)
            {
                String LargeName = Server.MapPath("~/UploadedImages/Artwork/Large/") + cartwork.LargerImageSName;
                Common.DeleteFile(LargeName);
            }
            //End

            //Delete from table
            rp.Delete(cartwork);
            rp.SubmitChanges();

            Response.Redirect(this.BackURL, false);
        }
        catch (Exception ex)
        {
            ex = null;
            lblMsg.Text = "Enable to delete the art!";
        }
    }

    protected void DownloadFile(String filepath, String displayFileName)
    {
        System.IO.Stream iStream = null;
        String type = "";
        String ext = Path.GetExtension(filepath);

        // Buffer to read 10K bytes in chunk:
        Byte[] buffer = new Byte[10000];

        // Total bytes to read:
        Int64 dataToRead;

        // Identify the file name.
        String filename = System.IO.Path.GetFileName(filepath);
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
                    case ".pdf":
                        type = "Application/pdf";
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
                    case ".bmp":
                        type = "image/bmp";
                        break;
                    case ".ai":
                        type = "application/illustrator";
                        break;
                    case ".epf":
                        type = "application/postscript";
                        break;
                }
            }

            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + displayFileName + ext + "\"");
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

    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        if (ImagePath != null && ImagePath != "0" && ImageName != null && ImageName != "0")
        {
            String LargeImagePath = Server.MapPath(ImagePath);
            DownloadFile(LargeImagePath, ImageName);
        }
        else
        {
            Int64 artworkid = Convert.ToInt64(this.ArtWorkID);
            ArtWorkRepository rp = new ArtWorkRepository();
            CompanyArtwork cartwork = rp.GetById(artworkid);
            String LargeName = Server.MapPath("~/UploadedImages/Artwork/Large/") + cartwork.LargerImageSName;
            DownloadFile(LargeName, cartwork.LargerImageOName);
        }        
    }

    protected void lnkemail_Click(object sender, EventArgs e)
    {
        if (StoreProductImageID != 0)
            Response.Redirect("SendArtMail.aspx?ImgID=" + this.StoreProductImageID);
        else if (!String.IsNullOrEmpty(Request.QueryString["src"]))
        {
            Session["ArtworkFile"] = Request.QueryString["src"];
            Response.Redirect("SendArtMail.aspx?aid=" + this.ArtWorkID);
        }
        else
            Response.Redirect("SendArtMail.aspx?id=" + this.ArtWorkID);
    }
}