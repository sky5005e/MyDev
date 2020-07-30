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

public partial class admin_ManageStore :PageBase
{
    int CompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Manage Store";
            ((HtmlGenericControl)manuControl.FindControl("dvSubMenu")).Visible = false;
            if (Request.QueryString["id"] != "0")
            {
                this.CompanyId = Convert.ToInt32(Request.QueryString["id"]);               
            }
            else
            {
                
            }
            manuControl.PopulateMenu(4, 0, Convert.ToInt64(Request.QueryString["id"]), 0, false);
        }
    }
}
