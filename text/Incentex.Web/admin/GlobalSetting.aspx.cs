using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_GlobalSetting : PageBase
{
    INC_AppSetting objAppSetting;
    AppSettingRepository objAppSettingRep = new AppSettingRepository();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Global Settings";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Global Setting";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";
            DisplayData();
        }
    }

    void DisplayData()
    {
        objAppSetting = new INC_AppSetting();
        objAppSetting = objAppSettingRep.GetbyName("GRIDPAGING");
        txtPageSize.Text = objAppSetting.sSettingValue;

        //start add by mayur on 3-dec-2011
        objAppSetting = new INC_AppSetting();
        objAppSetting = objAppSettingRep.GetbyName("MOASREMINDERDAY");
        txtNoOfDays.Text = objAppSetting.sSettingValue;
        //end mayur on 3-dec-2011        

        BindSAPSettings();
    }

    private void BindSAPSettings()
    {
        dlSAPSettings.DataSource = new SAPRepository().GetSAPSettings();
        dlSAPSettings.DataBind();
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            //update appsetting value
            objAppSetting = new INC_AppSetting();
            objAppSetting = objAppSettingRep.GetbyName("GRIDPAGING");
            objAppSetting.sSettingValue = txtPageSize.Text;
            objAppSetting.dLastupdatedate = System.DateTime.Now;
            objAppSettingRep.SubmitChanges();
            //Assign updated value to the Applcation variable
            Application["GRIDPAGING"] = objAppSetting.sSettingValue;

            //start add by mayur on 3-dec-2011
            objAppSetting = new INC_AppSetting();
            objAppSetting = objAppSettingRep.GetbyName("MOASREMINDERDAY");
            objAppSetting.sSettingValue = txtNoOfDays.Text;
            objAppSetting.dLastupdatedate = System.DateTime.Now;
            objAppSettingRep.SubmitChanges();
            //end mayur on 3-dec-2011

            lblMsg.Text = "Global settings saved successfully...";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    protected void lnkUpdateAllStore_Click(object sender, EventArgs e)
    {
        Response.Redirect("UpdateAllStoreStatus.aspx");
    }

    protected void lnkBtnGetDataBaseValues_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/GetDBValues.aspx");
    }

    protected void lnkSaveSAPSettings_Click(object sender, EventArgs e)
    {
        try
        {
            SAPRepository objSAPRepo = new SAPRepository();

            foreach (DataListItem item in dlSAPSettings.Items)
            {
                HiddenField hdnSettingID = (HiddenField)item.FindControl("hdnSettingID");
                TextBox txtSettingValue = (TextBox)item.FindControl("txtSettingValue");

                SAPSetting objSAPSetting = objSAPRepo.GetSAPSettingByID(Convert.ToInt64(hdnSettingID.Value));
                objSAPSetting.SettingValue = txtSettingValue.Text.Trim();
            }

            objSAPRepo.SubmitChanges();
            BindSAPSettings();

            lblMsg.Text = "SAP settings saved successfully...";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}