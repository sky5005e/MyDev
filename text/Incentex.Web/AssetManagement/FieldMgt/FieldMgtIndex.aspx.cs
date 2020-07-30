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

public partial class AssetManagement_FieldMgt_FieldMgtIndex :PageBase
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                MasterPageFile = "~/admin/MasterPageAdmin.master";
            }
            else
            {
                MasterPageFile = "~/MasterPage.master";
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            CheckLogin();
            if (!IsPostBack)
            {

                ((Label)Master.FindControl("lblPageHeading")).Text = "Field Management";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                if (Session["Usr"] == Convert.ToString(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Index.aspx";
                }
                else
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/AssetMgtIndex.aspx";
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}
