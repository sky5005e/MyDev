using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Text;
using System.Web.Services;

public partial class webchaptcha : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Set the Content type of the page to image.
        CaptchaMethod();
    }

    //[WebMethod]
    //public static void GetCaptchaImage()
    //{
    //    webchaptcha objwebcaptcha = new webchaptcha();
    //    objwebcaptcha.CaptchaMethod();
    //}

    private void CaptchaMethod()
    {
        System.Drawing.Image imgCaptcha;
        System.Drawing.Graphics graphics;
        System.IO.MemoryStream mStream = new System.IO.MemoryStream();
        FontStyle fnStyle = new FontStyle();
        fnStyle = FontStyle.Italic;

        Font objFont = new Font("Lucida Handwriting", 30, fnStyle);

        imgCaptcha = System.Drawing.Image.FromFile(Server.MapPath("~/Images/captcha1.jpg"));
        graphics = System.Drawing.Graphics.FromImage(imgCaptcha);        
        graphics.DrawString(Session["RandomCaptchaString"].ToString(), objFont, Brushes.SlateGray, 10, 10);
        imgCaptcha.Save(mStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        //byte[] imgArray = new Byte[ms.Length];
        Response.ContentType = "image/jpeg";
        Response.BinaryWrite(mStream.GetBuffer());
        Response.End();
    }
}
