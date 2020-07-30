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

public partial class admin_ManageSupplierPartner_ListSupplier : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Supplier Partners";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Supplier Partner List";
            ((HtmlImage)Master.FindControl("imgHomeBtn")).Visible = false;
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";
            bindMenus();
        }
    }

    public void bindMenus()
    {
        String menupriviledge = "";
        SupplierPartnerRepo ObjRepo = new SupplierPartnerRepo();        
        
        List<SupplierPartner> lstMenuAccess = ObjRepo.GetAllSupplierActive().ToList();

        foreach (SupplierPartner objMenu1 in lstMenuAccess)
        {
            if (menupriviledge == "")
                menupriviledge = objMenu1.SupplierPartnerID.ToString();
            else
                menupriviledge = menupriviledge + "," + objMenu1.SupplierPartnerID;
        }
        
        dtSupIndex.DataSource = lstMenuAccess;
        dtSupIndex.DataBind();
    }

    public String GetTooltip(Object Lname, Object Pass)
    {
        return "Login name : " + Lname + "  ,  " + "Password :" + Pass;
    }
}