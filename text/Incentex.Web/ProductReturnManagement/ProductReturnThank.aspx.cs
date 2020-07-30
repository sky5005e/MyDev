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

public partial class ProductReturnManagement_ProductReturnThank : PageBase
{
    Int64 OrderID
    {
        get
        {
            if (ViewState["OrderID"] == null)
            {
                ViewState["OrderID"] = 0;
            }
            return Convert.ToInt64(ViewState["OrderID"]);
        }
        set
        {
            ViewState["OrderID"] = value;
        }
    }
    string Status
    {
        get
        {
            if (ViewState["Status"] == null)
            {
                ViewState["Status"] = null;
            }
            return Convert.ToString(ViewState["Status"]);
        }
        set
        {
            ViewState["Status"] = value;
        }
    }
    string ReturnType
    {
        get
        {
            if (ViewState["ReturnType"] == null)
            {
                ViewState["ReturnType"] = null;
            }
            return Convert.ToString(ViewState["ReturnType"]);
        }
        set
        {
            ViewState["ReturnType"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack)
        {
            if (Request.QueryString["OrderID"] != null)
            {
                OrderID = Convert.ToInt64(Request.QueryString["OrderID"]);
            }
            if (Request.QueryString["Status"] != null)
            {
                Status = Request.QueryString["Status"];
            }
            if (Request.QueryString["ReturnType"] != null)
            {
                ReturnType = Request.QueryString["ReturnType"];
            }
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Return";

            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/ProductReturnManagement/OrderProductReturns.aspx?OrderID=" + OrderID + "&Status=" + Status + "&ReturnType=" + ReturnType;
        }

    }
}
