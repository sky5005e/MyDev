/* Project Name : Incentex 
 * Module Name : Uniform Isuance Program 
 * Description : This page is for add\edit of step3 of Uniform Isuance Program
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


public partial class admin_CompanyStore_CompanyPrograms_UniformIssuanceStep1 : PageBase
{
    #region Properties

    Int64 CompanyStoreId
    {
        get
        {
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {
            ViewState["CompanyStoreId"] = value;
        }
    }

    Int64 UniformIssuancePolicyID
    {
        get
        {
            return Convert.ToInt64(ViewState["UniformIssuancePolicyID"]);
        }
        set
        {
            ViewState["UniformIssuancePolicyID"] = value;
        }
    }

    String PaymentType
    {
        get
        {
            return Convert.ToString(ViewState["PaymentType"]);
        }
        set
        {
            ViewState["PaymentType"] = value;
        }
    }
    UniformIssuancePolicyRepository objUniformIssuancePolicyRepository = new UniformIssuancePolicyRepository(); 

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Uniform Issuance Policy - Step 3";
            this.PaymentType = Convert.ToString(Request.QueryString.Get("PaymentType"));
            this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));
            this.UniformIssuancePolicyID = Convert.ToInt64(Request.QueryString.Get("SubId"));
            menuControl.PopulateMenu(4, 1, this.CompanyStoreId, this.UniformIssuancePolicyID, true);
            if (PaymentType == "CompanyPays" || PaymentType == "MOAS")
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/CompanyPrograms/UniformIssuanceStep2.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType;
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/CompanyPrograms/UniformIssuanceStep2.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID;
            BindData();
            DisplayData();
        }

    }

    #region Methods

    /// <summary>
    /// bind months
    /// </summary>
    void BindData()
    {
        //bind months
        ddlMonths.Items.Insert(0,new ListItem("Number of Months","0") );

        for (int i = 1; i <= 81;i++)
        {
            ddlMonths.Items.Insert(i,new ListItem(i.ToString(),i.ToString()));
        }
        ddlMonths.Attributes.Add("onchange", "pageLoad(this,value);");
    }

    /// <summary>
    /// display data if record exists
    /// </summary>
    void DisplayData()
    {
        UniformIssuancePolicy objUniformIssuancePolicy = objUniformIssuancePolicyRepository.GetById(this.UniformIssuancePolicyID);

        if (objUniformIssuancePolicy != null)
        {
            txtEligibleDate.Text = Common.GetDateString(objUniformIssuancePolicy.EligibleDate);
            ddlMonths.SelectedValue = objUniformIssuancePolicy.NumberOfMonths.ToString();
            if (objUniformIssuancePolicy.IsDateOfHiredTicked != null && Convert.ToBoolean(objUniformIssuancePolicy.IsDateOfHiredTicked))
            {
                chkDateOfHire.Checked = Convert.ToBoolean(objUniformIssuancePolicy.IsDateOfHiredTicked);
                spChkDateOfHire.Attributes.Remove("class");
                spChkDateOfHire.Attributes.Add("class", "alignright custom-checkbox_checked");
            }
        }
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
        Response.Redirect("UniformIssuanceStep2.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType);
    }

    /// <summary>
    /// Save record and redirect to next page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        //save record and redirect to next page

        lblMsg.Text = "";
        
        try
        {

            if (chkDateOfHire.Checked)
            {
                if (ddlMonths.SelectedValue == "0")
                {
                    lblMsg.Text = "Please Select months ";
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtEligibleDate.Text))
                {
                    lblMsg.Text = "Please Select date";
                    return;
                }
            }
            UniformIssuancePolicy objUniformIssuancePolicy = objUniformIssuancePolicyRepository.GetById(this.UniformIssuancePolicyID);
            if (objUniformIssuancePolicy != null)
            {
                if (chkDateOfHire.Checked)
                {
                    objUniformIssuancePolicy.NumberOfMonths = Convert.ToInt32(ddlMonths.SelectedValue);
                    objUniformIssuancePolicy.EligibleDate = null;
                }
                else
                {
                    objUniformIssuancePolicy.EligibleDate = Common.GetDate(txtEligibleDate);
                    objUniformIssuancePolicy.NumberOfMonths = null;
                }
                objUniformIssuancePolicy.IsDateOfHiredTicked = chkDateOfHire.Checked;
                objUniformIssuancePolicyRepository.SubmitChanges();
                lblMsg.Text = "Record Saved Successfully.";
                Response.Redirect("UniformIssuanceStep4.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType, true);
            }
            else
                lblMsg.Text = "No record found";
        }
        catch(Exception ex)
        {
            lblMsg.Text = "Error in saving record.";
            ErrHandler.WriteError(ex);
        }

    }

    #endregion

}
