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
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Collections.Generic;

public partial class admin_UpdateAllStoreStatus :PageBase
{
    LookupDA sEU = new LookupDA();
    LookupBE sEUBE = new LookupBE();
    Common objcomm = new Common();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Update All Store Status";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/GlobalSetting.aspx";
            CheckLogin();
            BindValues();
        }
    }
    public void BindValues()
    {
        //Get stor status from lookup table and bind it to the dropdown
        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "Store Status";
        DataSet dsEU = sEU.LookUp(sEUBE);
        ddlStoreStatus.DataTextField = "sLookupName";
        ddlStoreStatus.DataValueField = "iLookupID";
        ddlStoreStatus.DataSource = dsEU;
        ddlStoreStatus.DataBind();
        ddlStoreStatus.Items.Insert(0, new ListItem("-select store status-", "0"));


      

       

    }
    protected void ddlStoreStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStoreStatus.SelectedItem.Text == "Closed" || ddlStoreStatus.SelectedItem.Text == "Updating")
        {
            trStatusMessage.Attributes.Add("style", "display:block");

        }
        else
        {
            txtAddress.Value = string.Empty;
            trStatusMessage.Attributes.Add("style", "display:none");
        }
    }
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            CompanyStoreRepository objStoreRepository = new CompanyStoreRepository();
            List<CompanyStore> objList = objStoreRepository.GetAllCompanyStore();
            string statusmessage = string.Empty;
            if (ddlStoreStatus.SelectedItem.Text == "Open")
            {
                statusmessage = string.Empty;
            }
            foreach (CompanyStore objStore in objList)
            {
                objStore.StoreStatusID = Convert.ToInt64(ddlStoreStatus.SelectedItem.Value);
                if (statusmessage !=  "")
                {
                    objStore.StoreStausMessage = txtAddress.Value;
                }
                else
                {
                    objStore.StoreStausMessage = statusmessage;
                }
            }
            objStoreRepository.SubmitChanges();
            lblMsg.Text = "All Store's status updated successfully..";
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);

        }

    }
}
