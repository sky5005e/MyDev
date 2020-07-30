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

public partial class RND_customvalidation : System.Web.UI.Page
{
    Common objcomm = new Common();
    protected void Page_Load(object sender, EventArgs e)
    {
      string message =  objcomm.Callvalidationmessage(Server.MapPath("Commonmsg.xml"),"Required","firstname");
      rdfFirstname.ErrorMessage = message;
    }
}
