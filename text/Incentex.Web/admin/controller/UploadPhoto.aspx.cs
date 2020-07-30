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
using System.IO;


public partial class admin_controller_UploadPhoto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sRet = "false";
        string sImageName = "";
        string sFilePath = "";
        Common objcomm = new Common();

        if (Request.Files.Count > 0)
        {
            if (Path.GetExtension(Request.Files[0].FileName).ToLower() == ".gif" ||
                Path.GetExtension(Request.Files[0].FileName).ToLower() == ".jpg" ||
                Path.GetExtension(Request.Files[0].FileName).ToLower() == ".jpeg" ||
                Path.GetExtension(Request.Files[0].FileName).ToLower() == ".png" ||
                Path.GetExtension(Request.Files[0].FileName).ToLower() == ".bmp")
            {
                if (((float)Request.Files[0].ContentLength / 1048576) > 1)
                {
                    sRet = "SIZELIMIT";
                }
                else
                {
                    try
                    {
                        if (Request.Params["mode"] == "priphoto")
                        {
                            sImageName = "StationUser_" +
                                DateTime.Now.ToString("ddMMyyyyHHmmss") 
                                + DateTime.Now.Millisecond.ToString("00")
                                + Path.GetExtension(Request.Files[0].FileName);
                            sFilePath = Server.MapPath("~/UploadedImages/stationuserPhoto/");
                            sFilePath += sImageName;
                            objcomm.SaveImage(Request.Files[0], sFilePath, 174, 189);
                            sRet = sImageName;
                        }
                        if (Request.Params["mode"] == "storecontactphoto")
                        {
                            sImageName = "StoreContact_" +
                                DateTime.Now.ToString("ddMMyyyyHHmmss")
                                + DateTime.Now.Millisecond.ToString("00")
                                + Path.GetExtension(Request.Files[0].FileName);
                            sFilePath = Server.MapPath("~/UploadedImages/CompanyStoreDocuments/");
                            sFilePath += sImageName;
                            Request.Files[0].SaveAs(sFilePath);
                            sRet = sImageName;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex = null;
                        sRet = "false";
                    }
                }
            }
            else
            {
                sRet = "FILETYPE";
            }
        }
        else
        {
            sRet = "false";
        }
        Response.Write(sRet);


    }
}
