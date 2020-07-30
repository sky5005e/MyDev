using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProductReturnManagement_ProductItemsReturnSearch : PageBase
{
    #region Data Members
    string RANumber = null;
    string FirstName = null;
    string LastName = null;
    #endregion

    #region Page Method's
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Return Lookup";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";
           
            // Here txt is hidded 
            txt.Visible = false;
            txtRANumber.Focus();
            txt.Attributes.Add("onkeypress", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('ctl00_ContentPlaceHolder1_lnkSubmitRequest').click();return false;}} else {return true}; "); 
        }
    }
    #endregion

    #region Page Method's
    protected void lnkSubmitRequest_Click(object sender, EventArgs e)
    {
        if (txtRANumber.Text != "")
            RANumber = txtRANumber.Text;
        if (txtFirstName.Text != "")
            FirstName = txtFirstName.Text;
        if (txtLastName.Text != "")
            LastName = txtLastName.Text;

        Int32 nday = 0;
        DateTime? FromDate = null;
        DateTime? ToDate = null;
        if (ddlDateRange.SelectedValue == "Range" && txtFromDate.Text != "" && txtToDate.Text != "")
        {
            FromDate = Convert.ToDateTime(txtFromDate.Text);
            ToDate = Convert.ToDateTime(txtToDate.Text);
        }
        else if (ddlDateRange.SelectedValue == "select")
        {
             FromDate = null;
             ToDate = null;
        }
        else
        {
            ToDate = DateTime.Now;
            nday = Convert.ToInt32(ddlDateRange.SelectedValue);
            FromDate = DateTime.Now.AddDays(-nday);
        }
        Response.Redirect("ReturnProductIEList.aspx?RANumber=" + RANumber + "&FirstName=" + FirstName + "&LastName=" + LastName + "&ToDate=" + ToDate + "&FromDate=" + FromDate);
    }

    protected void ddlDateRange_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDateRange.SelectedValue == "Range")//This is for date range
        {
            trFromDate.Visible = true;
            trToDate.Visible = true;
            trFromDate.Focus();
        }
        else
        {
            txtRANumber.Focus();
            txtFromDate.Text = null;
            txtToDate.Text = null;
            trFromDate.Visible = false;
            trToDate.Visible = false;
        }
    }
    #endregion
}
