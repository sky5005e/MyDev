using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class UserFrameItems_ApproveCancelPopup : System.Web.UI.Page
{
    #region Page Variable's
    String SiteURL
    {
        get
        {
            return Convert.ToString(ConfigurationSettings.AppSettings["NewDesignSiteurl"]);
        }
    }



    Int64 MainID
    {
        get
        {
            return Convert.ToInt64(ViewState["MainID"]);
        }
        set
        {
            ViewState["MainID"] = value;
        }
    }
    

    Int64 CartID
    {
        get
        {
            return Convert.ToInt64(ViewState["CartID"]);
        }
        set
        {
            ViewState["CartID"] = value;
        }
    }



    Boolean IsOrderApprove
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsOrderApprove"]);
        }
        set
        {
            ViewState["IsOrderApprove"] = value;
        }

    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["mainID"]))
            MainID = Convert.ToInt64(Request.QueryString["mainID"]);
        if (!String.IsNullOrEmpty(Request.QueryString["IsOrderApprove"]))
            IsOrderApprove = Convert.ToBoolean(Request.QueryString["IsOrderApprove"]);

        lblMsgTitle.Text = "Approve Order";

    }
}
