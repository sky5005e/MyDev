<%@ Page Language="C#" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="commonlib.Common" %>
<%@ Import Namespace="Incentex.DAL" %>
<%@ Import Namespace="Incentex.DAL.SqlRepository" %>
<%@ Import Namespace="Incentex.DAL.Common" %>
<%
   
   string sRet = "false";
   string sFileName = "";
   string sFilePath = "";
   Common objcomm = new Common();
   
        if (Request.Files.Count > 0)
        {
            if (Path.GetExtension(Request.Files[0].FileName).ToLower() == ".xls" ||
                Path.GetExtension(Request.Files[0].FileName).ToLower() == ".xlsx")
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
                            sFileName = "Incentex.xlsx"; // + Path.GetExtension(Request.Files[0].FileName).ToLower();
                            sFilePath = Server.MapPath("../UploadedImages/employeePhoto/") + sFileName;
                            //Save file on the server
                            Request.Files[0].SaveAs(sFilePath);
                            sRet = sFileName;
                            
                            //Copy the same file on the database server too.
                            //local
                            //File.Delete(@"\\10.2.1.61\Archive\" + sFileName);
                            //File.Copy(sFilePath, @"\\10.2.1.61\Archive\" + sFileName);
                            //live
                            File.Delete(@"\\216.157.31.118\Shared\" + sFileName);
                            File.Copy(sFilePath, @"\\216.157.31.118\Shared\" + sFileName);
                            
                            //Call Sp here..
                            new CompanyRepository().BulkUploadCustomerEmployee();
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrHandler.WriteError(ex);
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
