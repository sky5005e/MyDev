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

public partial class admin_IncentexEmployee_ManageEmails : PageBase
{
    Int64 IncentexEmployeeID
    {
        get
        {
            if (ViewState["IncentexEmployeeID"] == null)
            {
                ViewState["IncentexEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["IncentexEmployeeID"]);
        }
        set
        {
            ViewState["IncentexEmployeeID"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
             if (!IsPostBack)
        {
            CheckLogin();

            if (Request.QueryString.Count > 0)
            {
                this.IncentexEmployeeID = Convert.ToInt64(Request.QueryString.Get("Id"));

                if (this.IncentexEmployeeID == 0)
                {
                    Response.Redirect("~/admin/IncentexEmployee/BasicInformation.aspx?Id=" + this.IncentexEmployeeID);
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Employee Notes";
                //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to  Manage Incentex Employee</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/IncentexEmployee/ViewIncentexEmployee.aspx";
                
                menucontrol.PopulateMenu(5, 0, this.IncentexEmployeeID, 0,false);
                //populatesubmenu();
            }
            else
            {
                Response.Redirect("ViewIncentexEmployee.aspx");
            }

           
        }
        }
        catch (Exception)
        {
            
           
        }
    }
}
