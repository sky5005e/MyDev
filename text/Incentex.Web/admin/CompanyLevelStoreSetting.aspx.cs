using System;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;

public partial class admin_CompanyLevelStoreSetting : PageBase
{
    LookupDA sEU = new LookupDA();
    LookupBE sEUBE = new LookupBE();
    string selecteduserstoreoption = "";
    string selectedworldlinkpayment = "";
    string selectedcheckoutinformation = "";
    Int64 WorkGroupId
    {
        get
        {
            if (ViewState["WorkGroupId"] == null)
            {
                ViewState["WorkGroupId"] = 0;
            }
            return Convert.ToInt64(ViewState["WorkGroupId"]);
        }
        set
        {
            ViewState["WorkGroupId"] = value;
        }
    }
    Int64 CompanyId
    {
        get
        {
            if (ViewState["CompanyId"] == null)
            {
                ViewState["CompanyId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyId"]);
        }
        set
        {
            ViewState["CompanyId"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack)
        {
            
            selectmenuaccess menuaccess = (selectmenuaccess)Session["MenuAccess"];
            this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
            this.WorkGroupId = Convert.ToInt64(Request.QueryString.Get("workgroupid"));
            ((Label)Master.FindControl("lblPageHeading")).Text = "Gloabal Setting For Store Front Setting";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyLevleMenuSetting.aspx?id=" + this.CompanyId + "&workgroupid=" + this.WorkGroupId;
            
            bindShoppingSetting();
        }
    }
    private void bindShoppingSetting()
    {


        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "UserStoreOptions";
        DataSet dsEU = sEU.LookUp(sEUBE);
        dtUserStoreFront.DataSource = dsEU;
        dtUserStoreFront.DataBind();

        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "WLS Payment Option";
        DataSet dsPO = sEU.LookUp(sEUBE);
        dtPaymentOptions.DataSource = dsPO;
        dtPaymentOptions.DataBind();

        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "Required Checkout Information";
        DataSet dsCE = sEU.LookUp(sEUBE);
        dtCheckOutInfo.DataSource = dsCE;
        dtCheckOutInfo.DataBind();



    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        selectstoresetting b = new selectstoresetting();
        //User Store Option
        foreach (DataListItem dtuserstore in dtUserStoreFront.Items)
        {
            if (((CheckBox)dtuserstore.FindControl("chkUserStoreFront")).Checked == true)
            {

                if (selecteduserstoreoption == "")
                {
                    selecteduserstoreoption = ((HiddenField)dtuserstore.FindControl("hdnStoreFront")).Value;

                }
                else
                {
                    selecteduserstoreoption = selecteduserstoreoption + "," + ((HiddenField)dtuserstore.FindControl("hdnStoreFront")).Value;

                }

            }
        }
        b.userstoreoption = selecteduserstoreoption;

        

        //Payment Option

        foreach (DataListItem dtpayoption in dtPaymentOptions.Items)
        {
            if (((CheckBox)dtpayoption.FindControl("chkPaymentOptions")).Checked == true)
            {

                if (selectedworldlinkpayment == "")
                {
                    selectedworldlinkpayment = ((HiddenField)dtpayoption.FindControl("hdnPaymentOption")).Value;

                }
                else
                {
                    selectedworldlinkpayment = selectedworldlinkpayment + "," + ((HiddenField)dtpayoption.FindControl("hdnPaymentOption")).Value;

                }

            }
        }
        b.paymentoption = selectedworldlinkpayment;

        //Checkout infomration
        foreach (DataListItem dtcheckout in dtCheckOutInfo.Items)
        {
            if (((CheckBox)dtcheckout.FindControl("chkCheckOutInfo")).Checked == true)
            {

                if (selectedcheckoutinformation == "")
                {
                    selectedcheckoutinformation = ((HiddenField)dtcheckout.FindControl("hdnChecoutInfo")).Value;

                }
                else
                {
                    selectedcheckoutinformation = selectedcheckoutinformation + "," + ((HiddenField)dtcheckout.FindControl("hdnChecoutInfo")).Value;

                }

            }
        }
        b.checkoutinformation = selectedcheckoutinformation;

        //Assign in the session..
        Session["selectstoresetting"] = b;

        //Response.Redirect("CompanyLevelFinalStep.aspx?id=" + this.CompanyId + "&workgroupid=" + this.WorkGroupId.ToString());
        Response.Redirect("CompanyLevelPrograms.aspx?id=" + this.CompanyId + "&workgroupid=" + this.WorkGroupId.ToString());
    }

    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        Response.Redirect("CompanyLevleMenuSetting.aspx?id=" + this.CompanyId + "&workgroupid=" + this.WorkGroupId);
    }
}
