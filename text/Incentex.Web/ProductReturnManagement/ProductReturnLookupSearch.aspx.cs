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
using System.Collections.Generic;
using Incentex.DAL;

public partial class ProductReturnManagement_ProductReturnLookupSearch : PageBase
{
    string ToDate = null;
    string FromDate = null;
    string StoreId = null;
    string Email = null;
    string OrderNumber = null;
    string OrdeStatus = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.Form.DefaultButton = lnkSubmitRequest.UniqueID;

            ddlCompanyStore.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            txtFromDate.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            txtToDate.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            ddlOrderStatus.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            txtOrderNumber.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            txtEmail.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
          
        if (!IsPostBack)
        {
            base.MenuItem = "Process Incoming Returns";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }
           
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Return Lookup";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";
            FillCompanyStore();
            FillStaus();
        }

    }
    private void FillStaus()
    {
        try
        {
            LookupRepository objLookRep = new LookupRepository();
            string strPayment = "ProductReturn";
            ddlOrderStatus.DataSource = objLookRep.GetByLookup(strPayment);
            ddlOrderStatus.DataValueField = "iLookupID";
            ddlOrderStatus.DataTextField = "sLookupName";
            ddlOrderStatus.DataBind();
            ddlOrderStatus.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    private void FillCompanyStore()
    {
        try
        {   
            OrderConfirmationRepository objLookRep = new OrderConfirmationRepository();
            ddlCompanyStore.DataSource = objLookRep.GetCompanyStoreName().OrderBy(le => le.CompanyName);
            ddlCompanyStore.DataValueField = "StoreID";
            ddlCompanyStore.DataTextField = "CompanyName";
            ddlCompanyStore.DataBind();
            ddlCompanyStore.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void lnkSubmitRequest_Click(object sender, EventArgs e)
    {
        if (ddlCompanyStore.SelectedIndex > 0)
        {
            StoreId = ddlCompanyStore.SelectedValue;
        }
        if (txtToDate.Text != "")
        {
            ToDate = txtToDate.Text;
        }
        if (txtFromDate.Text != "")
        {
            FromDate = txtFromDate.Text;
        }
        if (txtEmail.Text != "")
        {
            Email = txtEmail.Text;
        }
        if (txtOrderNumber.Text != "")
        {
            OrderNumber = txtOrderNumber.Text;
        }
        if (ddlOrderStatus.SelectedIndex > 0)
        {
            OrdeStatus = ddlOrderStatus.SelectedItem.Text;
        }

        Response.Redirect("ReturnProductIE.aspx?StoreId=" + StoreId + "&ToDate=" + ToDate + "&FromDate=" + FromDate + "&Email=" + Email + "&OrderNumber=" + OrderNumber + "&OrdeStatus=" + OrdeStatus);
    }
}
