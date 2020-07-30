using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Artwork_ViewSearchResults : PageBase
{
    Int64 CustomerID
    {
        get
        {
            if (ViewState["CustomerID"] != null)
                return Convert.ToInt64(ViewState["CustomerID"]);
            else
                return 0;
        }
        set
        {
            ViewState["CustomerID"] = value;
        }
    }

    Int64 FileCategoryID
    {
        get
        {
            if (ViewState["FileCategoryID"] != null)
                return Convert.ToInt64(ViewState["FileCategoryID"]);
            else
                return 0;
        }
        set
        {
            ViewState["FileCategoryID"] = value;
        }
    }

    String FileName
    {
        get
        {
            if (ViewState["FileName"] != null)
                return ViewState["FileName"].ToString();
            else
                return null;
        }
        set
        {
            ViewState["FileName"] = value;
        }
    }

    Int64 ProdCateID
    {
        get
        {
            if (ViewState["ProdCateID"] != null)
                return Convert.ToInt64(ViewState["ProdCateID"]);
            else
                return 0;
        }
        set
        {
            ViewState["ProdCateID"] = value;
        }
    }

    Int64 SubCatID
    {
        get
        {
            if (ViewState["SubCatID"] != null)
                return Convert.ToInt64(ViewState["SubCatID"]);
            else
                return 0;
        }
        set
        {
            ViewState["SubCatID"] = value;
        }
    }

    Int64 WorkGroupID
    {
        get
        {
            if (ViewState["WorkGroupID"] != null)
                return Convert.ToInt64(ViewState["WorkGroupID"]);
            else
                return 0;
        }
        set
        {
            ViewState["WorkGroupID"] = value;
        }
    }

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

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            Session["ArtImagrReturnURL"] = Request.Url.AbsoluteUri;
            ((Label)Master.FindControl("lblPageHeading")).Text = "Image or Artwork Results";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Artwork/SearchArtwork.aspx";
            // Set this Properties when images get from Product items
            if (Request.QueryString["Pid"] != null)
                this.ProdCateID = Convert.ToInt64(Request.QueryString.Get("Pid"));
            if (Request.QueryString["SubCatID"] != null)
                this.SubCatID = Convert.ToInt64(Request.QueryString.Get("SubCatID"));
            if (Request.QueryString["WorkGroupID"] != null)
                this.WorkGroupID = Convert.ToInt64(Request.QueryString.Get("WorkGroupID"));

            // Set this properties when select product from Art image Library 
            if (Request.QueryString["cid"] != null)
                this.CustomerID = Convert.ToInt64(Request.QueryString.Get("cid"));
            if (Request.QueryString["aid"] != null)
                this.FileCategoryID = Convert.ToInt64(Request.QueryString.Get("aid"));
            if (Request.QueryString["fname"] != null)
                this.FileName = Request.QueryString.Get("fname");
            // Get All images from ArtImage Library 
            if (Request.QueryString["cid"] != null && Request.QueryString["aid"] != null)
                GetArtsForFC();
            // Get All images from Product Items
            if (Request.QueryString["Pid"] != null && Request.QueryString["SubCatID"] != null)
                GetProductImageList();
        }
    }

    private void GetProductImageList()
    {
        List<StoreProductImage> objStoreProductImage = new List<StoreProductImage>();
        StoreProductImageRepository objRepos = new StoreProductImageRepository();
        StoreProductRepository objStoreRep = new StoreProductRepository();
        var StoreList = objStoreRep.GetStoreProducts(this.ProdCateID, this.SubCatID, this.WorkGroupID).Select(s => new { s.StoreProductID }).ToList();

        foreach (var items in StoreList)
        {
            objStoreProductImage.AddRange(objRepos.GetStoreProductImagesByStoreProductId(Convert.ToInt64(items.StoreProductID)));
        }

        dtStoreProdcutImageList.DataSource = objStoreProductImage.ToList();
        dtStoreProdcutImageList.DataBind();
    }

    public void GetArtsForFC()
    {
        ArtWorkRepository rp = new ArtWorkRepository();
        List<CompanyArtWorkResults> results = rp.GetAllArts(this.CustomerID, this.FileCategoryID, this.FileName == null ? null : this.FileName);
        lstImageList.DataSource = results;
        lstImageList.DataBind();

        ((Label)Master.FindControl("lblPageHeading")).Text = new LookupRepository().GetById(this.FileCategoryID).sLookupName;
    }

    protected void lstImageList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CompanyArtWorkResults obj = e.Item.DataItem as CompanyArtWorkResults;
            if (obj.ThumbImageSName != null)
            {
                String[] th = obj.ThumbImageSName.Split('.');
                HtmlImage imgProduct = e.Item.FindControl("imgProduct") as HtmlImage;

                if (th[1] == "pdf" || th[1] == "eps" || th[1] == "DST" || th[1] == "EMB" || th[1] == "ai")
                {
                    imgProduct.Src = "../../UploadedImages/Artwork/download.png";
                    imgProduct.Alt = "0";
                }
                else
                {
                    imgProduct.Src = "~/controller/createthumb.aspx?_ty=artImages&_path=" + obj.ThumbImageSName + "&_twidth=145&_theight=198";
                    imgProduct.Alt = obj.ArtWorkId.ToString();
                }
            }
        }
    }

    protected void dtStoreProdcutImageList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HtmlImage imgStoreProduct = e.Item.FindControl("imgStoreProduct") as HtmlImage;
            imgStoreProduct.Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + ((HiddenField)e.Item.FindControl("hdnProductImage")).Value + "&_twidth=145&_theight=198";
            imgStoreProduct.Alt = ((HiddenField)e.Item.FindControl("hdnStoreProductImageId")).Value.ToString();
        }
    }

    protected void dtStoreProdcutImageList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        String ThumbName = String.Empty;
        String LargeName = String.Empty;
        HiddenField hdnProductImage = ((HiddenField)((DataListItem)((LinkButton)e.CommandSource).Parent).FindControl("hdnProductImage"));
        Session["ArtImagrReturnURL"] = Request.Url.AbsoluteUri;

        switch (e.CommandName)
        {
            case "download":
                ThumbName = Server.MapPath("~/UploadedImages/ProductImages/Thumbs/") + hdnProductImage.Value;
                DownloadFile(ThumbName, hdnProductImage.Value);
                break;

            case "email":
                Response.Redirect("SendArtMail.aspx?ImgID=" + e.CommandArgument.ToString());
                break;
        }

        GetProductImageList();
    }

    protected void lstImageList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        String ThumbName = String.Empty;
        String LargeName = String.Empty;
        HiddenField hfThumb = ((HiddenField)((DataListItem)((LinkButton)e.CommandSource).Parent).FindControl("hfThumb"));
        HiddenField hfLarge = ((HiddenField)((DataListItem)((LinkButton)e.CommandSource).Parent).FindControl("hfLarge"));
        Session["ArtImagrReturnURL"] = Request.Url.AbsoluteUri;

        switch (e.CommandName)
        {
            case "del":
                try
                {
                    if (!base.CanDelete)
                    {
                        base.RedirectToUnauthorised();
                    }

                    //Delete stored files                   
                    ThumbName = Server.MapPath("~/UploadedImages/Artwork/Thumbs/") + hfThumb.Value;
                    Common.DeleteFile(ThumbName);

                    if (hfLarge.Value != String.Empty)
                    {
                        LargeName = Server.MapPath("~/UploadedImages/Artwork/Large/") + hfLarge.Value;
                        Common.DeleteFile(LargeName);
                    }

                    //Delete from table
                    Int64 artworkid = Convert.ToInt64(e.CommandArgument.ToString());
                    ArtWorkRepository rp = new ArtWorkRepository();
                    CompanyArtwork cartwork = rp.GetById(artworkid);
                    rp.Delete(cartwork);
                    rp.SubmitChanges();

                }
                catch
                {
                    lblMsg.Text = "Unable to delete the art!";
                }
                break;

            case "download":
                ThumbName = Server.MapPath("~/UploadedImages/Artwork/Thumbs/") + hfThumb.Value;
                HiddenField hfThumbO = ((HiddenField)((DataListItem)((LinkButton)e.CommandSource).Parent).FindControl("hfThumbO"));
                DownloadFile(ThumbName, hfThumbO.Value);
                break;

            case "email":
                Response.Redirect("SendArtMail.aspx?id=" + e.CommandArgument.ToString());
                break;
        }

        GetArtsForFC();
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
}