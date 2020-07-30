using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

/// <summary>
/// Summary description for SAService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class SAService : System.Web.Services.WebService
{

    public SAService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    //[WebMethod]
    //public List<GetSystemAccessSubDetailsResult> GetSystemData(Int64 UserInfoID)
    //{
    //    SystemAccessRepository ObjRepo = new SystemAccessRepository();
    //    List<GetSystemAccessSubDetailsResult> ObjSADetails = ObjRepo.SearchSystemData(UserInfoID);
    //    return ObjSADetails;
    //}

}

