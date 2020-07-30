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

public partial class admin_CompanyStore_StoreShipping : PageBase
{

    #region Local Property
    LookupDA sEU = new LookupDA();
    LookupBE sEUBE = new LookupBE();
    CompanyStore objCompanyStore = new CompanyStore();
    CompanyStoreRepository objStoreRepository = new CompanyStoreRepository();
    string selectedservices = "";

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
                //Assign Page Header and return URL 
                this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));

                if (this.CompanyStoreId == 0)
                {
                    Response.Redirect("~/admin/CompanyStore/GeneralStoreSetup.aspx?Id=" + this.CompanyStoreId);
                }

                if (this.CompanyStoreId > 0 && !base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                } 

                ((Label)Master.FindControl("lblPageHeading")).Text = "Store Shipping";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/CompanyStore/ViewCompanyStore.aspx";
                Session["ManageID"] = 5;

                menuControl.PopulateMenu(2, 0, this.CompanyStoreId, 0, false);
                BindShippingDetails();
                Common.BindActiveInactiveDDL(ddlFreeStatus);

                //Function gets called when user comes in edit mode
                DisplayData(sender, e);
            }
        }
    }
    #endregion

    #region Miscellaneous functions
    /// <summary>
    /// Function to get values when user comes in edit mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DisplayData(object sender, EventArgs e)
    {
        CheckBox chk;
        HiddenField lblId;
        if (this.CompanyStoreId != 0)
        {
            objCompanyStore = objStoreRepository.GetById(this.CompanyStoreId);
            if (objCompanyStore == null)
            {
                return;
            }
            //if (objCompanyStore.ShippingTypeID != null)
                //ddlShipper.Items.FindByValue(objCompanyStore.ShippingTypeID.ToString()).Selected = true;
            if (Convert.ToBoolean(objCompanyStore.IsSaleShipping))
            {
                chkShippingPercenteageOfSale.Checked = true;
                spnShip.Attributes.Add("class", "custom-checkbox_checked");
                txtMinShipping.Text = objCompanyStore.MinimumShippingAmount.ToString();
                txtShippingOfSale.Text = objCompanyStore.ShippiingPercentOfSale.ToString();
                dvShippingOfSale.Attributes.Add("style", "display:marker");
                dvMinShippingAmount.Attributes.Add("style", "display:marker");
            }
            //Make selected items

            if (objCompanyStore.ShippingServices != null)
            {
                string[] Ids = objCompanyStore.ShippingServices.Split(',');
                /*foreach (DataListItem dt in dtlShippingType.Items)
                {
                    chk = dt.FindControl("chkdtlMenus") as CheckBox;
                    lblId = dt.FindControl("hdnServices") as HiddenField;
                    HtmlGenericControl dvChk = dt.FindControl("menuspan") as HtmlGenericControl;

                    foreach (string i in Ids)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }
                    }
                }*/
            }

            txtTotalSaleAbove.Text = objCompanyStore.totalsaleabove;

            if (objCompanyStore.shippingprogramfor == "BothUnTicked")
            {
                chkAdmin.Checked = false;
                chkEmployee.Checked = false;
            }
            if (objCompanyStore.shippingprogramfor == "Both")
            {
                chkAdmin.Checked = true;
                chkEmployee.Checked = true;
                Span1.Attributes.Add("class", "custom-checkbox_checked");
                Span2.Attributes.Add("class", "custom-checkbox_checked");
            }
            if (objCompanyStore.shippingprogramfor == "Admin")
            {
                chkAdmin.Checked = true;
                Span2.Attributes.Add("class", "custom-checkbox_checked");
            }
            if (objCompanyStore.shippingprogramfor == "Employee")
            {
                chkEmployee.Checked = true;
                Span1.Attributes.Add("class", "custom-checkbox_checked");
            }

            if (objCompanyStore.isFreeShippingActive != null)
            {
                if ((bool)objCompanyStore.isFreeShippingActive)
                {
                    ddlFreeStatus.Items.FindByText("Active").Selected = true;
                    txtProgStartDate.Text = Convert.ToDateTime(objCompanyStore.ShippingProgramStartDate).ToString("MM/dd/yyyy");
                    txtProgEndDate.Text = Convert.ToDateTime(objCompanyStore.ShippingProgramEndDate).ToString("MM/dd/yyyy");
                }
                else
                {
                    ddlFreeStatus.Items.FindByText("InActive").Selected = true;
                    txtProgStartDate.Text = null;
                    txtProgEndDate.Text = null;
                }
            }


        }
    }
    /// <summary>
    /// Bind Shipping type and shipping services
    /// </summary>
    public void BindShippingDetails()
    {

        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "Shipping Type";
        DataSet dsEU = sEU.LookUp(sEUBE);
        //ddlShipper.DataTextField = "sLookupName";
        //ddlShipper.DataValueField = "iLookupID";
        //ddlShipper.DataSource = dsEU;
        //ddlShipper.DataBind();
        //ddlShipper.Items.Insert(0, new ListItem("Choose Shipping Type", "0"));

        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "Shipping Services";
        DataSet dsS = sEU.LookUp(sEUBE);
        //dtlShippingType.DataSource = dsS;
        //dtlShippingType.DataBind();
    }

    #endregion

    #region Button Click events
    protected void lnkBtnSaveInformation_Click(object sender, EventArgs e)
    {

        try
        {
            /* foreach (DataListItem dt in dtlShippingType.Items)
            {
                if (((CheckBox)dt.FindControl("chkdtlMenus")).Checked == true)
                {
                    if (selectedservices == "")
                    {
                        selectedservices = ((HiddenField)dt.FindControl("hdnServices")).Value;

                    }
                    else
                    {
                        selectedservices = selectedservices + "," + ((HiddenField)dt.FindControl("hdnServices")).Value;

                    }
                }


            }*/
            //getbyid
            objCompanyStore = objStoreRepository.GetById(this.CompanyStoreId);

            //update values
            //objCompanyStore.ShippingTypeID = Convert.ToInt64(ddlShipper.SelectedItem.Value);
            objCompanyStore.ShippingServices = selectedservices;
            if (chkShippingPercenteageOfSale.Checked)
            {
                objCompanyStore.IsSaleShipping = chkShippingPercenteageOfSale.Checked;
                objCompanyStore.MinimumShippingAmount = txtMinShipping.Text.Trim()!=string.Empty?Convert.ToDecimal(txtMinShipping.Text.Trim()):0;
                objCompanyStore.ShippiingPercentOfSale = txtShippingOfSale.Text.Trim()!=string.Empty?Convert.ToDecimal(txtShippingOfSale.Text.Trim()):0;
            }
            else
            {
                objCompanyStore.IsSaleShipping = false;
                objCompanyStore.MinimumShippingAmount = null;
                objCompanyStore.ShippiingPercentOfSale = null;
            }
            objCompanyStore.totalsaleabove = txtTotalSaleAbove.Text.Trim()!=string.Empty?txtTotalSaleAbove.Text.Trim():"0";
           
            string shippingprg = string.Empty;
            if (chkAdmin.Checked == false && chkEmployee.Checked == false)
            {
                objCompanyStore.shippingprogramfor = "BothUnTicked";
            }

            if (chkAdmin.Checked == true && chkEmployee.Checked == true)
            {
                objCompanyStore.shippingprogramfor = "Both";
            }
            if (chkAdmin.Checked == false && chkEmployee.Checked == true)
            {
                objCompanyStore.shippingprogramfor = "Employee";
            }
            if (chkAdmin.Checked == true && chkEmployee.Checked == false)
            {
                objCompanyStore.shippingprogramfor = "Admin";
            }
            if (ddlFreeStatus.SelectedItem.Text == "Active")
            {
                objCompanyStore.isFreeShippingActive = true;
                objCompanyStore.ShippingProgramStartDate = Convert.ToDateTime(txtProgStartDate.Text);
                objCompanyStore.ShippingProgramEndDate = Convert.ToDateTime(txtProgEndDate.Text);

            }
            else
            {
                objCompanyStore.isFreeShippingActive = false;
                objCompanyStore.ShippingProgramStartDate = null;
                objCompanyStore.ShippingProgramEndDate = null;
            }

            objStoreRepository.SubmitChanges();

            //redirecting after record saves
            Response.Redirect("GuideLineManuals.aspx?Id=" + this.CompanyStoreId);
        }
        catch { }
    }
    #endregion
  
}
