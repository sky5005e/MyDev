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
using System.IO;
using Incentex.BE;
using Incentex.DA;
using System.Data.SqlClient;
using System.Text;
using System.Web.Mail;
using System.Net;
using Incentex.DAL;
using Incentex.DAL.Common;

public partial class admin_CommunicationCenter_Subscribe : PageBase
{
    CompanyEmployeeRepository obj = new CompanyEmployeeRepository();
    CompanyEmployee objEmpl = new CompanyEmployee();
    CampaignMailHistory ObjMailHistory = new CampaignMailHistory();
    CampHistory ObjCampHisRepo = new CampHistory();
    IncentexEmployee objIE = new IncentexEmployee();
    IncentexEmployeeRepository objIERepo = new IncentexEmployeeRepository();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            if (Request.QueryString["userinfoid"] != null)
            {
                objIE = objIERepo.GetEmployeeByUserInfoId(Convert.ToInt64(Request.QueryString["userinfoid"].ToString()));
                objEmpl = obj.GetByUserInfoId(Convert.ToInt64(Request.QueryString["userinfoid"].ToString()));

                if (Request.QueryString["id"].ToString() == "1")
                {
                    if (objIE != null)
                        objIE.MailFlag = true;
                    else
                        objEmpl.MailFlag = true;

                    dvSub.Visible = true;
                }
                else
                {
                    if (objIE != null)
                        objIE.MailFlag = false;
                    else
                        objEmpl.MailFlag = false;
                    dvNSub.Visible = true;
                }
                objIERepo.SubmitChanges();
                obj.SubmitChanges();
            }

        }
        if (Request.QueryString["CampID"] != null)
        {
            if (Request.QueryString["UserinfoID"] != null)
            {
                Int32 UserInfo = Convert.ToInt32(Request.QueryString["UserinfoID"]);
                Int32 Campid = Convert.ToInt32(Request.QueryString["CampID"]);
                ObjMailHistory = ObjCampHisRepo.GetByIdUserCamp(Convert.ToInt32(UserInfo), Convert.ToInt32(Campid));

                if (ObjMailHistory != null)
                {
                    ObjMailHistory.CampMailStatus = 2;
                    ObjMailHistory.ReadDate = DateTime.Now;
                    ObjCampHisRepo.SubmitChanges();
                }

                 
            
            }


        }
    }
}
