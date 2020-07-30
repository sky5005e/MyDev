using System;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyStore_ViewGlobalMenuSetting : PageBase
{
    DataSet dsLookup;
    LookupDA objLookup = new LookupDA();
    LookupBE objBe = new LookupBE();
    string strMessgae = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Store Management Console";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            if(Request.QueryString["id"] != null)
            {
                ((Label)Master.FindControl("lblPageHeading")).Text = "View Store Front Setting By User workgroup";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/GeneralStoreSetup.aspx?Id=" + Request.QueryString["id"].ToString();
                Session["ManageID"] = 5;

                menuControl.PopulateMenu(11, 0, Convert.ToInt64(Request.QueryString["id"]), 0, false);

                BindDatlist("Workgroup");
            }
        }
    }
    public void BindDatlist(string strQurystring)
    {
        try
        {
            objBe.SOperation = "Selectall";
            objBe.iLookupCode = strQurystring;
            dsLookup = objLookup.LookUp(objBe);
            if (dsLookup.Tables.Count > 0 && dsLookup.Tables[0].Rows.Count > 0)
            {
                dtLstLookup.DataSource = dsLookup;
                dtLstLookup.DataBind();
            }
            else
            {
                dtLstLookup.DataSource = null;
                dtLstLookup.DataBind();
            }
        }
        catch (Exception ex)
        {
            strMessgae = ex.Message.ToString();
        }
        finally
        {

            objLookup = null;
            dsLookup.Dispose();

        }
    }
  
    protected void dtLstLookup_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            GlobalMenuSetting objVal = new GlobalMenuSettingRepository().GetById(Convert.ToInt64(((HiddenField)e.Item.FindControl("hdnLookupId")).Value), Convert.ToInt64(Request.QueryString["id"].ToString()));
            if (objVal == null)
                ((HyperLink)e.Item.FindControl("lnkEdit")).ToolTip = "Set up Menu access for this Workgroup";
            else
                ((HyperLink)e.Item.FindControl("lnkEdit")).ToolTip = "Edit Menu access for this workgroup";

            ((HyperLink)e.Item.FindControl("lnkEdit")).NavigateUrl = "GlobalMenuAccess.aspx?Id=" + Convert.ToInt64(Request.QueryString["id"].ToString()) + "&SubId=" + Convert.ToInt64(((HiddenField)e.Item.FindControl("hdnLookupId")).Value)+"";
            
        }
    }
}
