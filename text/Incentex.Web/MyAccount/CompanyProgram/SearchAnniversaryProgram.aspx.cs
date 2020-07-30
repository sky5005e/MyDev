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
using System.Collections.Generic;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_CompanyProgram_SearchAnniversaryProgram : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            if (IncentexGlobal.IsIEFromStore)
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            else
               ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Company Anniversary Program";
            BindValues();
            FillBaseStation();
        }
    }
    private void FillBaseStation()
    {
        try
        {
            List<INC_BasedStation> objList = new List<INC_BasedStation>();
            BaseStationRepository objBaseRepos = new BaseStationRepository();
            objList = objBaseRepos.GetAllBaseStation();
            if (objList.Count > 0)
            {
                ddlStation.DataSource = objList;
                ddlStation.DataValueField = "iBaseStationId";
                ddlStation.DataTextField = "sBaseStation";
                ddlStation.DataBind();
                ddlStation.Items.Insert(0, new ListItem("-select-", "0"));
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
     //bind dropdown for the  
    public void BindValues()
    {

        LookupRepository objlist = new LookupRepository();
        ddlStatus.DataSource = objlist.GetByLookup("Status");
        ddlStatus.DataValueField = "iLookupID";
        ddlStatus.DataTextField = "sLookupName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
       Response.Redirect("~/MyAccount/CompanyProgram/ViewAnniversaryCreditPrograms.aspx?stCode="+ddlStation.SelectedItem.Value+"&employeeid="+txtEmployeeNumber.Text+"&empstatus="+ddlStatus.SelectedValue+"&FName="+txtFirstName.Text+"&Lname="+txtLastName.Text+"&crType="+ddlCreditType.SelectedItem.Value);
    }
    
}
