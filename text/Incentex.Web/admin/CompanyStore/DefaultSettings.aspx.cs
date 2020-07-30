using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;


public partial class admin_CompanyStore_DefaultSettings : PageBase
{

    #region Data Member
    Int64 CompanyStoreId
    {
        get
        {
            if (ViewState["CompanyStoreId"] == null)
            {
                ViewState["CompanyStoreId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {
            ViewState["CompanyStoreId"] = value;
        }
    }

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

    GlobalMenuSetting objCompanyByWorkgroup = new GlobalMenuSetting();
    GlobalMenuSettingRepository objMenuRepo = new GlobalMenuSettingRepository();
    #endregion

    #region Methods
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack)
        {
            this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));
            this.WorkGroupId = Convert.ToInt64(Request.QueryString.Get("SubId"));

            ((Label)Master.FindControl("lblPageHeading")).Text = "Default Settings for Store Workgroup";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/ViewGlobalMenuSetting.aspx?ID=" + this.CompanyStoreId;

            Session["ManageID"] = 5;
            menuControl.PopulateMenu(11, 0, this.CompanyStoreId, this.WorkGroupId, true);
            if(this.WorkGroupId != 0)
                lblWorkGroup.Text = new LookupRepository().GetById(this.WorkGroupId).sLookupName.ToString();


            BindPricing();

        }

    }
    protected void BindPricing()
    {
        objCompanyByWorkgroup = objMenuRepo.GetById(this.WorkGroupId, this.CompanyStoreId);

        if (objCompanyByWorkgroup != null && objCompanyByWorkgroup.MOASPaymentPricing != null)
        {
            ddlPriceLevel.SelectedValue = objCompanyByWorkgroup.MOASPaymentPricing;
        }
    }
    #endregion

    #region Event Handlers
    protected void lnkbtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ddlPriceLevel.SelectedValue != "0")
            //{
                objCompanyByWorkgroup = objMenuRepo.GetById(this.WorkGroupId, this.CompanyStoreId);

                if (objCompanyByWorkgroup == null)
                {
                    //Insert here first
                    GlobalMenuSetting objGlobalInsert = new GlobalMenuSetting();
                    objGlobalInsert.StoreId = this.CompanyStoreId;
                    objGlobalInsert.WorkgroupId = this.WorkGroupId;
                    objMenuRepo.Insert(objGlobalInsert);
                    objMenuRepo.SubmitChanges();

                    objCompanyByWorkgroup = objMenuRepo.GetById(this.WorkGroupId, this.CompanyStoreId);
                }
                objCompanyByWorkgroup.MOASPaymentPricing = ddlPriceLevel.SelectedValue;
                objMenuRepo.SubmitChanges();
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
}
