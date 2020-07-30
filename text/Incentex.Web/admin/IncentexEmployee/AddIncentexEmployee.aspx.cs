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

public partial class admin_UserManagement_AddIncentexEmployee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
          // Visible False for upper div fo Master page.when Menu bind.
            Master.FindControl("spantitle").Visible = false;
            Master.FindControl("spanlogin").Visible = false;
            Master.FindControl("spandate").Visible = false;
            Master.FindControl("divheder").Visible = false;
            try
            {
                if (Request.QueryString["ManageID"] != null)
                {
                  
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
        }

    }
}
