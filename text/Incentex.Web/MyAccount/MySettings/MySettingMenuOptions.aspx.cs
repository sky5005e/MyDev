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
using Incentex.DAL;
using System.Collections.Generic;
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL.Common;
public partial class MyAccount_MySettings_MySettingMenuOptions : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();

            ((Label)Master.FindControl("lblPageHeading")).Text = "My Setting Menu Options";
            if (IncentexGlobal.IsIEFromStore)
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
         
            //bindReportDashboard();
        }
    }
    //public void bindReportDashboard()
    //{
    //    try
    //    {
    //        List<INC_Lookup> objlist = new List<INC_Lookup>();
    //        LookupRepository objLookRep = new LookupRepository();
    //        objlist = objLookRep.GetByLookup("MySettingOption");
    //        if (objlist.Count > 0)
    //        {
    //            dtIndex.DataSource = objlist;
    //            dtIndex.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ErrHandler.WriteError(ex);
    //    }


    //}
    //protected void dtIndex_ItemCommand(object source, DataListCommandEventArgs e)
    //{
    //    if (e.CommandName == "Redirect")
    //    {
    //        HiddenField hdnMenuPrevilidgedId = (HiddenField)e.Item.FindControl("hdnMenuAccess");
    //        if (e.CommandArgument.ToString() == "User Information")
    //        {
    //            Response.Redirect("MyUserInformation.aspx");
    //        }
    //        else if (e.CommandArgument.ToString() == "Billing Information")
    //        {
    //            Response.Redirect("MyBilling.aspx");
    //        }
    //        else if (e.CommandArgument.ToString() == "Shipping Information")
    //        {
    //            Response.Redirect("MyShipping.aspx");
    //        }
    //        else if (e.CommandArgument.ToString() == "Password Change")
    //        {
    //            Response.Redirect("MyPassworChanged.aspx");
    //        }
            

    //    }


    //}

    protected void lnkUserInformation_Click(object sender, EventArgs e)
    {
        Response.Redirect("MyUserInformation.aspx");
    }

    protected void lnkShippingInformation_Click(object sender, EventArgs e)
    {
        Response.Redirect("MyShipping.aspx");       
    }

    protected void lnkFlaggedAssets_Click(object sender, EventArgs e)
    {
        Response.Redirect("MyBilling.aspx");
    }

    protected void lnkPasswordChange_Click(object sender, EventArgs e)
    {
        Response.Redirect("MyPassworChanged.aspx");
    }

}
