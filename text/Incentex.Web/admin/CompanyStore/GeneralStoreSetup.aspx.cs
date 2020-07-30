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
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;

public partial class admin_CompanyStore_GeneralStoreSetup : PageBase
{
    #region Local Property
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

    LookupDA sEU = new LookupDA();
    LookupBE sEUBE = new LookupBE();
    Common objcomm = new Common();
    #endregion

    #region Page Load Event
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

            if (Request.QueryString.Count > 0)
            {
                this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));                

                //Assign Page Header and return URL 
                ((Label)Master.FindControl("lblPageHeading")).Text = "General Store Information";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/CompanyStore/ViewCompanyStore.aspx";
                Session["ManageID"] = 5;

                menuControl.PopulateMenu(0, 0, this.CompanyStoreId, 0, false);

                ////Bind values function
                BindValues();

                //Function gets called when user comes in edit mode
                DisplayData(sender, e);
            }
        }
    }
    #endregion


    #region Miscellaneous functions
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


        //Get companyids from companystore table
        CompanyStoreRepository objStoreRepo = new CompanyStoreRepository();
        List<CompanyStore> objStroeList = objStoreRepo.GetAllCompanyStore();
        string storelist = "";
        foreach (CompanyStore objStr in objStroeList)
        {
            if (storelist == "")
            {
                storelist = objStr.CompanyID.ToString();
            }
            else
            {
                storelist = storelist + "," + objStr.CompanyID.ToString();
            }
        }

        //get company
        CompanyRepository objRepo = new CompanyRepository();
        List<Company> objList = new List<Company>();
        if (this.CompanyStoreId != 0)
            objList = objRepo.GetAllCompany();
        else
            objList = objRepo.GetCompanyNotCompanyStore(storelist);
        Common.BindDDL(ddlCompany, objList, "CompanyName", "CompanyID", "-select company-");

    }

    /// <summary>
    /// Function to get values when user comes in edit mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DisplayData(object sender, EventArgs e)
    {

        if (this.CompanyStoreId != 0)
        {
            CompanyStore objCompanyEmployee = new CompanyStoreRepository().GetById(this.CompanyStoreId);
            if (objCompanyEmployee == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(objCompanyEmployee.StoreStausMessage))
            {
                trStatusMessage.Attributes.Add("style", "display:block");
                txtAddress.Value = objCompanyEmployee.StoreStausMessage.ToString().Replace(Environment.NewLine, "\n");
            }
            else
            {
                trStatusMessage.Attributes.Add("style", "display:none");
                txtAddress.Value = string.Empty;
            }

            ddlCompany.Items.FindByValue(objCompanyEmployee.CompanyID.ToString()).Selected = true;
            ddlCompany.Enabled = false;
            ddlStoreStatus.Items.FindByValue(objCompanyEmployee.StoreStatusID.ToString()).Selected = true;
        }
    }
    #endregion


    #region Button Click events
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.CompanyStoreId == 0 && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }
            else if (this.CompanyStoreId > 0 && !base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            CompanyStore objCompanyStore = new CompanyStore();
            CompanyStoreRepository objStoreRepository = new CompanyStoreRepository();
            if (this.CompanyStoreId != 0)
            {
                objCompanyStore = objStoreRepository.GetById(this.CompanyStoreId);
                
            }
            objCompanyStore.StoreStatusID = Convert.ToInt64(ddlStoreStatus.SelectedItem.Value);
            objCompanyStore.CompanyID = Convert.ToInt64(ddlCompany.SelectedItem.Value);
            objCompanyStore.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objCompanyStore.CretedDate = System.DateTime.Now;
            objCompanyStore.StoreStausMessage = txtAddress.Value;
            if (this.CompanyStoreId == 0)
            {
                objStoreRepository.Insert(objCompanyStore);
            }
            objStoreRepository.SubmitChanges();
            this.CompanyStoreId = objCompanyStore.StoreID;

            Response.Redirect("SplashImage.aspx?Id=" + this.CompanyStoreId);
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);

        }

    }
    #endregion

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
}

