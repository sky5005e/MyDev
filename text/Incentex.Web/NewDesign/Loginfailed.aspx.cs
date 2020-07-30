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

public partial class Loginfailed : System.Web.UI.Page
{
    string message = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Request.QueryString.Count > 0)
        //{
        //    if (Request.QueryString.Get("success") == "fail")
        //    {
        //        userInActive.Visible = true;
        //        LognFailed.Visible = false;

        //    }
        //    else if (Request.QueryString.Get("success") == "storeupdating")
        //    {

        //        message = new CompanyStoreRepository().GetById(new CompanyStoreRepository().GetStoreIDByCompanyId(Convert.ToInt64(Request.QueryString["uid"].ToString()))).StoreStausMessage.ToString();
        //        if (!string.IsNullOrEmpty(message))
        //        {
        //            lblUpdateMessage.Text = message.Replace(Environment.NewLine, "\n");
        //        }
        //        storeUpdating.Visible = true;
        //        LognFailed.Visible = false;

        //    }
        //    else
        //    {
        //        message = new CompanyStoreRepository().GetById(new CompanyStoreRepository().GetStoreIDByCompanyId(Convert.ToInt64(Request.QueryString["uid"].ToString()))).StoreStausMessage.ToString();
        //        if (!string.IsNullOrEmpty(message))
        //        {
        //            lblClosedMessage.Text = message.Replace(Environment.NewLine, "\n");
        //        }


        //        storeClosed.Visible = true;
        //        LognFailed.Visible = false;

        //    }

        //}
        //else
        //{
        //    userInActive.Visible = false;
        //    LognFailed.Visible = true;
        //}
    }
}
