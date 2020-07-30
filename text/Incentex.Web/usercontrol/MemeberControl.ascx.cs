using System;
using System.Collections;
using System.Collections.Generic;
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
using AjaxControlToolkit;
using Incentex.DAL;
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;

public partial class usercontrol_MemeberControl : System.Web.UI.UserControl
{
    String CompanyId
    {
        get
        {
            return Session["CompanyId"].ToString();
        }
        set
        {
            Session["CompanyId"] = value;
        }
    }
    String CompanyName
    {
        get
        {
            return Session["CompanyName"].ToString();
        }
        set
        {
            Session["CompanyName"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindCompanyStores();
            mpMemberStatus.Show();
        }
    }
    protected override void OnInit(EventArgs e)
    {
        Page.Init += delegate(object sender, EventArgs e_Init)
        {
            if (ToolkitScriptManager.GetCurrent(Page) == null && ScriptManager.GetCurrent(Page) == null)
            {
                ToolkitScriptManager sMgr = new ToolkitScriptManager();
                phScriptManager.Controls.AddAt(0, sMgr);
            }
        };
        base.OnInit(e);
    }

    public void bindCompanyStores()
    {
        //get company stores
        CompanyStoreRepository objRepo = new CompanyStoreRepository();
        List<CompanyStoreRepository.IECompanyListResults> lstStores = objRepo.GetCompanyStore().OrderBy(le => le.Company).ToList();
        Common.BindDDL(ddlCompany, lstStores, "Company", "CompanyId", "-select company-");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string CheckText = hdCheck.Value.ToString();

        try
        {
            if (ddlCompany.SelectedValue != "0")
                this.CompanyId = ddlCompany.SelectedValue.ToString();
            if (!String.IsNullOrEmpty(txtCompanyName.Text))
                this.CompanyName = txtCompanyName.Text;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        if (CheckText == "true")
        {
            Response.Redirect("~/signup.aspx");
        }
        else
        {
            Response.Redirect("~/Vendorsignup.aspx");
        }
    }
}