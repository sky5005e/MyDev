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

public partial class forgotpasswordconfirmation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Forgot Password?";
            ((HtmlImage)Master.FindControl("imgHomeBtn")).Visible = false;
            ((HtmlImage)Master.FindControl("imgSupportTickets")).Visible = false;
        }
    }
}
