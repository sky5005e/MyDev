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

public partial class NewDesign_DragDrop : System.Web.UI.Page
{

    public String FileExt
    {
        get
        {
            return Convert.ToString(ViewState["FileExt"]);
        }
        set
        {
            ViewState["FileExt"] = value;
        }
    }
    protected void UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
    {
        FileExt = "pdf,docx,doc,mpeg,mpg,mov,mp4,3gp,flv,avi,wmv,jpg,jpeg,png,bmp";
        MediaRepository objrepos = new MediaRepository();
        string path = "";
        string CurrentFileName = "";
        if (e.FileName.Contains('\\'))
        {
            CurrentFileName = e.FileName.Substring(e.FileName.LastIndexOf('\\') + 1);
        }
        else
        {
            CurrentFileName = e.FileName;
        }

        //path = path.Replace("\\\\", "\\");
        //string path = "~/NewDesign/DocmentStorageCenter/" + e.FileName;
        Session["MediaFileName"] = System.DateTime.Now.Ticks + "_" + CurrentFileName;
        path = Server.MapPath("~/NewDesign/DocmentStorageCenter/") + System.DateTime.Now.Ticks + "_" + CurrentFileName;
        
        string[] arr=FileExt.Split(',');
        string CurrFileExt = CurrentFileName.Substring(CurrentFileName.LastIndexOf('.')+1).ToLower();
        
            if (CurrFileExt == "pdf" )
            {
                Session["MediaFileType"] = "pdf";
                
            }
            else if (CurrFileExt == "doc" || CurrFileExt == "docx")
            {
                Session["MediaFileType"] = "document";
            }

            else if (CurrFileExt.ToLower() == "mpeg" || CurrFileExt == "mpg" || CurrFileExt == "mov" || CurrFileExt == "mp4" || CurrFileExt == "3gp" || CurrFileExt == "flv" || CurrFileExt == "avi" || CurrFileExt == "wmv")
            {
                Session["MediaFileType"] = "video";

            }
            else if (CurrFileExt.ToLower() == "jpg" ||   CurrFileExt=="jpeg" || CurrFileExt=="png" || CurrFileExt=="bmp")
            {
                Session["MediaFileType"] = "image";
               // Session["MediaFileName"] = e.FileName;
              //  path = Server.MapPath("~/NewDesign/DocmentStorageCenter/") + e.FileName;
            }
            else
            {
                Session["MediaFileType"] = "other";
            }

        string UploadPath = Server.MapPath("~/NewDesign/DocmentStorageCenter/");
        string filename = Session["MediaFileName"].ToString();
        
        
        string tempimage = Server.MapPath("~/NewDesign/DocmentStorageCenter/") + CurrentFileName;
       // AjaxFileUpload1.SaveAs(@"c:\\" + e.FileName);
        AjaxFileUpload1.SaveAs(path);
        if (Session["MediaFileType"] == "image")
        AjaxFileUpload1.SaveAs(tempimage);
      
        //string fullpath = System.IO.Path.Combine(UploadPath, filename);
        //AjaxFileUpload1.SaveAs();
        //Path.Combine(str_uploadpath, fileName);
    }

    
}



