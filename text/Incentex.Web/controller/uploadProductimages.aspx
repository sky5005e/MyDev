<%@ Page Language="C#" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="commonlib.Common" %>
<%
   
   string sRet = "false";
   string sImageName = "";
   string sFilePath = "";
   string sFilePathSupEmp = "";
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
                            sImageName = "empphoto_" +
                                DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() +
                                DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() +
                                DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() +
                                DateTime.Now.Millisecond.ToString() + Path.GetExtension(Request.Files[0].FileName);
                            
                                sFilePath = Server.MapPath("../UploadedImages/employeePhoto/") + sImageName;
                                objcomm.SaveImage(Request.Files[0], sFilePath, 600, 600);
                                sFilePathSupEmp = Server.MapPath("../UploadedImages/SupplierEmployeePhoto/") + sImageName;
                                objcomm.SaveImage(Request.Files[0], sFilePathSupEmp, 600, 600);                                                     
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
 
%>

