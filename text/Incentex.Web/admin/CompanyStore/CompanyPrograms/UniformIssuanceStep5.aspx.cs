/* Project Name : Incentex 
 * Module Name : Uniform Isuance Program 
 * Description : This page is for add\edit of step5 of Uniform Isuance Program
 * ----------------------------------------------------------------------------------------- 
 * DATE | ID/ISSUE| AUTHOR | REMARKS 
 * ----------------------------------------------------------------------------------------- 
 * 23-Oct-2010 | 1 | Amit Trivedi | Design and Coding
 * ----------------------------------------------------------------------------------------- */

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
using System.Xml.Linq;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using System.Collections.Generic;
using System.IO;


public partial class admin_CompanyStore_CompanyPrograms_UniformIssuanceStep1 : PageBase
{
    #region Properties
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
    String PaymentType
    {
        get
        {
            if (ViewState["PaymentType"] == null)
            {
                ViewState["PaymentType"] = null;
            }
            return Convert.ToString(ViewState["PaymentType"]);
        }
        set
        {
            ViewState["PaymentType"] = value;
        }
    }
    Int64 UniformIssuancePolicyID
    {
        get
        {
            if (ViewState["UniformIssuancePolicyID"] == null)
            {
                ViewState["UniformIssuancePolicyID"] = 0;
            }
            return Convert.ToInt64(ViewState["UniformIssuancePolicyID"]);
        }
        set
        {
            ViewState["UniformIssuancePolicyID"] = value;
        }
    }
    UniformIssuancePolicyRepository objUniformIssuancePolicyRepository = new UniformIssuancePolicyRepository();
    UniformIssuancePolicyItemRepository objUniformIssuancePolicyItemRepository = new UniformIssuancePolicyItemRepository();
    LookupRepository objLookupRepository = new LookupRepository(); 
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Uniform Issuance Policy - Step 5";
            this.PaymentType = Convert.ToString(Request.QueryString.Get("PaymentType"));
            this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));
            this.UniformIssuancePolicyID = Convert.ToInt64(Request.QueryString.Get("SubId"));
            menuControl.PopulateMenu(4, 1, this.CompanyStoreId, this.UniformIssuancePolicyID, true);

            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/CompanyPrograms/UniformIssuanceStep4.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType;
            DisplayData();
            lstItems.DataBind();
            dtBillingAddress.DataBind();
            dtShipping.DataBind();
        }
    }
    #region Methods

    /// <summary>
    /// Display existing data
    /// </summary>
    void DisplayData()
    {
        UniformIssuancePolicy objUniformIssuancePolicy = objUniformIssuancePolicyRepository.GetById(this.UniformIssuancePolicyID);


        //department and workgroup

        if (objUniformIssuancePolicy.DepartmentID != null && objUniformIssuancePolicy.DepartmentID > 0)
            lblDepartment.Text = objLookupRepository.GetById(Convert.ToInt64(objUniformIssuancePolicy.DepartmentID)).sLookupName;
        lblWorkGroup.Text = objLookupRepository.GetById(objUniformIssuancePolicy.WorkgroupID).sLookupName;
        
        //issuance time rules
        if (objUniformIssuancePolicy.NumberOfMonths != null)
        {
            lblIssuancePeriod.Text = objUniformIssuancePolicy.NumberOfMonths + " Months";
        }
        else
        {
            lblIssuancePeriod.Text = Convert.ToDateTime(objUniformIssuancePolicy.EligibleDate.ToString()).ToShortDateString();
        }

        if (objUniformIssuancePolicy.CreditExpireNumberOfMonths != null)
        {
            lblExpiresAfter.Text = objUniformIssuancePolicy.CreditExpireNumberOfMonths + " Months";
        }
        else
        {
            lblExpiresAfter.Text = Convert.ToDateTime(objUniformIssuancePolicy.CreditExpireDate).ToShortDateString();
        }


        if((bool)objUniformIssuancePolicy.IsDateOfHiredTicked == true)
        {
            chkDateOfHire.Checked = true;
            spChkDateOfHire.Attributes.Remove("class");
            spChkDateOfHire.Attributes.Add("class", "custom-checkbox_checked true");
        }

        //reminder

        lblReminder1.Text = objUniformIssuancePolicy.FirstReminderAlarm + " Months Before";
        lblReminder2.Text = objUniformIssuancePolicy.SecondReminderAlarm + " Months Before";

        //if(objUniformIssuancePolicy.SecondReminderAlarm == 1)
        //{
        //    lblReminder2.Text = "Yes";
        //}
        //else
        //{
        //    lblReminder2.Text = "No";
        //}

        lblReminder3.Text = objUniformIssuancePolicy.ThirdReminderAlarm + " Months Before";
        lblExpirationReminder.Text = objUniformIssuancePolicy.ExpirationReminder + " Months After";
    }

    #endregion
    #region Events
    /// <summary>
    /// redirect to previous page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        // redirect to previous step
        Response.Redirect("UniformIssuanceStep4.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType);

    }
    /// <summary>
    /// get datasource for item list and set to datalist
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstItems_DataBinding(object sender, EventArgs e)
    {
        try
        {
            UniformIssuancePolicyItemRepository obj = new UniformIssuancePolicyItemRepository();
            //List<UniformIssuancePolicyItemRepository.UniformIssuancePolicyItemResult> objUniformIssuancePolicyItemList = objUniformIssuancePolicyItemRepository.GetByUniformIssuancePolicyID(this.UniformIssuancePolicyID);
            List<SelectUniformIssuancePolicyProgramResult> objUniformIssuancePolicyItemList = new List<SelectUniformIssuancePolicyProgramResult>();
            objUniformIssuancePolicyItemList = obj.GetCreditProgram(this.UniformIssuancePolicyID);
            if (objUniformIssuancePolicyItemList.Count > 0)
            {
                lstItems.DataSource = objUniformIssuancePolicyItemList;
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    /// <summary>
    /// bind item name and image in grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstItems_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem  )
        {
            HiddenField hdnUniformIssuancePolicyItemID = e.Item.FindControl("hdnUniformIssuancePolicyItemID") as HiddenField;
            HiddenField hdnMasterItemId = e.Item.FindControl("hdnMasterItemId") as HiddenField;
            HiddenField hdnStoreProductid = e.Item.FindControl("hdnStoreProductid") as HiddenField;
            Image imgPhoto = e.Item.FindControl("imgPhoto") as Image;
            HiddenField hdnPaymentOptionid = e.Item.FindControl("hdhPaymentOption") as HiddenField;
            Label lblPaymentName = e.Item.FindControl("lblPaymentOption") as Label;
            if (hdnPaymentOptionid.Value == "1")
            {
                lblPaymentName.Text = "CompanyPays";
            }
            else
            {
                lblPaymentName.Text = "EmployeePays";
            }
            Label lblItem = e.Item.FindControl("lblItem") as Label;
            //Label lblIssuance = e.Item.FindControl("lblIssuance") as Label;

            INC_Lookup objLookup = objLookupRepository.GetById( Convert.ToInt64(hdnMasterItemId.Value));

            if(objLookup != null)
            {
                lblItem.Text = objLookup.sLookupName;
            }

            //display image
            StoreProductImageRepository objStoreProductImageRepository = new StoreProductImageRepository();
            //Start update Nagmani 16-Feb-2012
            //List<StoreProductImage> objList = objStoreProductImageRepository.GetStoreProductImage(this.CompanyStoreId, Convert.ToInt64(hdnMasterItemId.Value));
            List<StoreProductImage> objList = objStoreProductImageRepository.GetStoreProductImageNew(this.CompanyStoreId, Convert.ToInt64(hdnMasterItemId.Value), Convert.ToInt64(hdnStoreProductid.Value));
            //End
           // HtmlAnchor lnkItemImg = e.Item.FindControl("lnkItemImg") as HtmlAnchor;

            if (objList.Count > 0)
            {
                if (!string.IsNullOrEmpty(objList[0].ProductImage))
                {
                  //  string imgPath = "~/UploadedImages/ProductImages/" + objList[0].ProductImage;
                    string imgPath = "~/UploadedImages/ProductImages/Thumbs/" + objList[0].ProductImage;
                    if (File.Exists(Server.MapPath(imgPath)))
                    {
                        imgPhoto.ImageUrl = imgPath;
                        imgPhoto.Visible = true;
                        //lnkItemImg.HRef = imgPath;
                    }
                    
                }
            }
        }
    }
    /// <summary>
    /// Redirect to first step
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkEditDetails_Click(object sender, EventArgs e)
    {
        Response.Redirect("UniformIssuanceStep1.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType,false );
        
    }
    protected void lnkProcess_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewIssuancePrograms.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID, false);
    }
    #endregion
    protected void dtBillingAddress_DataBinding(object sender, EventArgs e)
    {
        try
        {
            UniformIssuancePolicyItemRepository obj = new UniformIssuancePolicyItemRepository();

            List<SelectBillingAddressResult> objUniformIssuancePolicyItemList = new List<SelectBillingAddressResult>();
            objUniformIssuancePolicyItemList = obj.GetBillingAddress(this.UniformIssuancePolicyID);
            if (objUniformIssuancePolicyItemList.Count > 0)
            {
               
                pnlCompanyPays.Visible = true;
                dtBillingAddress.DataSource = objUniformIssuancePolicyItemList;
            }
            else
            {
                pnlCompanyPays.Visible = false;
                
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void dtShipping_DataBinding(object sender, EventArgs e)
    {
        try
        {
            UniformIssuancePolicy objUniformIssuancePolicy = objUniformIssuancePolicyRepository.GetById(this.UniformIssuancePolicyID);
            if (objUniformIssuancePolicy.ShowPayment.ToString() == "Y")
            {
                UniformIssuancePolicyItemRepository obj = new UniformIssuancePolicyItemRepository();

                List<SelectShippingAddressResult> objUniformIssuancePolicyItemList = new List<SelectShippingAddressResult>();
                objUniformIssuancePolicyItemList = obj.GetShippingAddress(this.UniformIssuancePolicyID);
                if (objUniformIssuancePolicyItemList.Count > 0)
                {
                    Panel1.Visible = true;
                    dtShipping.DataSource = objUniformIssuancePolicyItemList;
                }
                else
                {
                    Panel1.Visible = false;
                }
            }
            else
            {
                Panel1.Visible = false;
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}
