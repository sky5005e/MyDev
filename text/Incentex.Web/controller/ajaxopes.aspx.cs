using System;
using System.Data;
using System.IO;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class controller_ajaxopes : System.Web.UI.Page
{
    DataSet ds, dsOrderDetails;
    string sRet, sImageName;

    protected void Page_Load(object sender, EventArgs e)
    {

      try
        {
            if (Request.Params["mode"] != null)
            {
                sRet = "";
                switch (Request.Params["mode"])
                {
                    //Delete Primary Photo
                    case "DELPRIPHOTO":
                        sImageName = Request.Params["imgname"];
                        if (File.Exists(Server.MapPath("../UploadedImages/employeePhoto/" + sImageName)))
                            File.Delete(Server.MapPath("../UploadedImages/employeePhoto/" + sImageName));
                        break;
                    case   "DELSTATIONPHOTO":
                        sImageName = Request.Params["imgname"];
                        if (File.Exists(Server.MapPath("../UploadedImages/stationuserPhoto/" + sImageName)))
                            File.Delete(Server.MapPath("../UploadedImages/stationuserPhoto/" + sImageName));
                        break;
                    case "DELSUPPLIEREMPLOYEEPHOTO":
                        sImageName = Request.Params["imgname"];
                        if (File.Exists(Server.MapPath("../UploadedImages/SupplierEmployeePhoto/" + sImageName)))
                            File.Delete(Server.MapPath("../UploadedImages/SupplierEmployeePhoto/" + sImageName));
                        break;
                    case "DELETESTOREPHOTO":
                        sImageName = Request.Params["imgname"];
                        if (File.Exists(Server.MapPath("../UploadedImages/CompanyStoreDocuments/" + sImageName)))
                            File.Delete(Server.MapPath("../UploadedImages/CompanyStoreDocuments/" + sImageName));
                        break;
                    //Added on 28Sep2011
                    case "CHECKIFHIREDDATE":
                        sImageName = Request.Params["imgname"];
                        GlobalMenuSetting objValue = new GlobalMenuSettingRepository().GetById(Convert.ToInt64(Request.Params["imgname"]),new CompanyStoreRepository().GetStoreIDByCompanyId(Convert.ToInt64(Request.Params["compid"])));
                        if (objValue != null)
                        {
                            //Check if Paymnet option is ticked
                            if ((bool)objValue.IsHiredDateTicked)
                            {
                                sRet = "true";
                            }
                            else
                            {
                                sRet = "false";
                            }
                        }
                        else
                        {
                            sRet = "false";
                        }
                      break;
                      //End
                    default:
                        sRet = "false";
                        break;
                }

                if (sRet == "")
                {
                    sRet = "true";                    
                }
            }
        }
        catch (Exception ex)
        {
            ex = null;
            sRet = "false";
        }
        finally
        {
           Response.Clear();
            Response.Write(sRet);
        Response.End();
        }
    }

    private void deleteFiles(string psPath, string psSearchString)
    {
        Array.ForEach(Directory.GetFiles(psPath, psSearchString),
          delegate(string path)
          {
              if (File.Exists(path))
                  File.Delete(path);
          });
    }    
   
 }

   