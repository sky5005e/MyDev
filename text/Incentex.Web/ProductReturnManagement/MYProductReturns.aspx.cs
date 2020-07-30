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

public partial class ProductReturnManagement_MYProductReturns : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((HtmlImage)Master.FindControl("imgHomeBtn")).Visible = true;
            ((Label)Master.FindControl("lblPageHeading")).Text = "My Product Returns";
            if (IncentexGlobal.IsIEFromStore)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            }
            ShowShortReturnLink();
        }
    }


    private void ShowShortReturnLink()
    {
        CompanyEmployee objCmpnyInfo = new CompanyEmployee();
        CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
        objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

        // when User type = CE, StoreID = Spirit and Main Work Group = FlightAttendent Group 
        // Then only Short Return system Link is visible 
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee)
            && IncentexGlobal.CurrentMember.CompanyId == 3 && objCmpnyInfo.WorkgroupID == 51 && objCmpnyInfo.GenderID == 63 )// here 51 is work group
            dvShortReturn.Visible = true;
        else
            dvShortReturn.Visible = false;

    }

    protected void btnShortReturnSystem_Click(object sender, EventArgs e)
    {
        IncentexBEDataContext db = new IncentexBEDataContext();
        var ShortReturnCount = db.ReturnShortProducts.Where(s=> s.UserInfoID == IncentexGlobal.CurrentMember.UserInfoID).ToList();
        if (ShortReturnCount.Count > 0)
        {
            //string myStringVariable = string.Empty;
            //myStringVariable = "Shorts Product already exchanged";
            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
            Response.Redirect("ShortReturnThanks.aspx?Req=processing");
        }
        else
        {
            Response.Redirect("ShortReturns.aspx");
        }
    }
}
