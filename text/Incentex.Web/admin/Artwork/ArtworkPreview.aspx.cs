using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class admin_Artwork_ArtworkPreview :PageBase
{
    #region Data Memeber's
    ArtWorkRepository objartRepo = new ArtWorkRepository();
    Int64 ArtworkID
    {
        get
        {
            if (ViewState["ArtworkID"] != null)
                return Convert.ToInt64(ViewState["ArtworkID"]);
            else
                return 0;
        }
        set
        {
            ViewState["ArtworkID"] = value;
        }
    }
    #endregion 
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

            if (!base.CanView || !base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Artwork Library";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Artwork/SearchArtworkLibrary.aspx";
            this.ArtworkID = Convert.ToInt64(Request.QueryString["Id"]);
            BindDatalist();
        }
    }
    private void BindDatalist()
    {
        List<ArtWorkList> list = objartRepo.GetAllArtWorkFiles(this.ArtworkID);
        dtArtList.DataSource = list;
        dtArtList.DataBind();

    }
    protected void dtArtList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HtmlImage img = e.Item.FindControl("img") as HtmlImage;
            HiddenField hdnFile = (HiddenField)e.Item.FindControl("hdnFile");
           

            Session["ArtImagrReturnURL"] = Request.Url.AbsoluteUri;
            if (hdnFile.Value.EndsWith(".pdf"))
            {
                img.Src = "~/UploadedImages/Artwork/Thumbs/" + ((HiddenField)e.Item.FindControl("hdnFile")).Value;
            }
            else
            {
                img.Src = "~/controller/createthumb.aspx?_ty=artImages&_path=" + hdnFile.Value + "&_twidth=145&_theight=198";
            }
            img.Alt = hdnFile.Value;

            // To display artwork Name, Artwork for
            Label lblDisplay = (Label)e.Item.FindControl("lblDisplay");
            lblDisplay.Text = String.Format("Artwork Name: {0} <br/> Artwork For: {1}", ((HiddenField)e.Item.FindControl("hdnArtworkName")).Value, ((HiddenField)e.Item.FindControl("hdnArtworkFor")).Value);

            // To display file type icon
            HtmlImage imgFiletype = (HtmlImage)e.Item.FindControl("imgFiletype");
            string ext = Path.GetExtension(hdnFile.Value);
            imgFiletype.Src = "~/Images/FileType/" + ext.Replace(".", "") + ".png";
                
        }

    }

    protected void dtArtList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        string ThumbName = string.Empty;
        string LargeName = string.Empty;
        HiddenField hdnFile = ((HiddenField)((DataListItem)((LinkButton)e.CommandSource).Parent).FindControl("hdnFile"));
        Session["ArtImagrReturnURL"] = Request.Url.AbsoluteUri;
        switch (e.CommandName)
        {

            case "download":
                ThumbName = Server.MapPath("~/UploadedImages/Artwork/Thumbs/") + hdnFile.Value;
                DownloadFile(ThumbName, hdnFile.Value);

                break;

            case "email":
                Session["ArtworkFile"] = hdnFile.Value;
                Response.Redirect("SendArtMail.aspx?aid=" + e.CommandArgument.ToString());
                break;
        }
       
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
                    //left:--rm

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
