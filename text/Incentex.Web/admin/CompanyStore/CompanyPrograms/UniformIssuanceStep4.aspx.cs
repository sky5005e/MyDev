/* Project Name : Incentex 
 * Module Name : Uniform Isuance Program 
 * Description : This page is for add\edit of step4 of Uniform Isuance Program
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


public partial class admin_CompanyStore_CompanyPrograms_UniformIssuanceStep1 : PageBase
{

    #region Properties
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

    #endregion
    
    protected void Page_Load(object sender, EventArgs e)
    {

        CheckLogin();

        if (!IsPostBack)
        {


            ((Label)Master.FindControl("lblPageHeading")).Text = "Uniform Issuance Policy - Step 4";
            this.PaymentType = Convert.ToString(Request.QueryString.Get("PaymentType"));
            this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));
            this.UniformIssuancePolicyID = Convert.ToInt64(Request.QueryString.Get("SubId"));
            menuControl.PopulateMenu(4, 1, this.CompanyStoreId, this.UniformIssuancePolicyID, true);


            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/CompanyPrograms/UniformIssuanceStep3.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType;
            BindDropDowns();
            BindData();
            DisplayData();
        }
    }


    #region Methods

    public void BindDropDowns()
    {
        LookupRepository objLookRep = new LookupRepository();
        ddlStatus.DataSource = objLookRep.GetByLookup("status");
        ddlStatus.DataValueField = "iLookupID";
        ddlStatus.DataTextField = "sLookupName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    /// <summary>
    /// Bind Master data to dropdown 
    /// </summary>
    void BindData()
    {
        // CreditExpireNumberOfMonth and first reminder,   //third reminder
        ddlCreditExpireNumberOfMonth.Items.Insert(0,new ListItem("Number of Months","0"));
        Common.AddOnChangeAttribute(ddlCreditExpireNumberOfMonth);

        ddlReminder1.Items.Insert(0, new ListItem("1st Reminder", "0"));
        Common.AddOnChangeAttribute(ddlReminder1);

        ddlReminder2.Items.Insert(0, new ListItem("2nd Reminder", "0"));
        Common.AddOnChangeAttribute(ddlReminder2);

        ddlReminder3.Items.Insert(0, new ListItem("3st Reminder", "0"));
        Common.AddOnChangeAttribute(ddlReminder3);

        ddlExpirationReminder.Items.Insert(0, new ListItem("Expiration Reminder", "0"));
        Common.AddOnChangeAttribute(ddlExpirationReminder);


        for (int i = 1; i <= 24;i++)
        {
            ddlCreditExpireNumberOfMonth.Items.Insert(i, new ListItem(i.ToString(),i.ToString()));
            if (i <= 12)
            {
                ddlReminder1.Items.Insert(i, new ListItem(i.ToString(), i.ToString()));
                ddlReminder2.Items.Insert(i, new ListItem(i.ToString(), i.ToString()));
                ddlReminder3.Items.Insert(i, new ListItem(i.ToString(), i.ToString()));
                ddlExpirationReminder.Items.Insert(i, new ListItem(i.ToString(), i.ToString()));
            }
        }


        //bind Company in Active Rule
        ddlEmployeeActiveRule.Items.Insert(0, new ListItem("Select", "0"));
        ddlEmployeeActiveRule.Items.Insert(1, new ListItem(DAEnums.GetEmployeeActveRuleName(DAEnums.EmployeeActveRule.ProgramResume), Convert.ToString((int)DAEnums.EmployeeActveRule.ProgramResume)));
        ddlEmployeeActiveRule.Items.Insert(2, new ListItem(DAEnums.GetEmployeeActveRuleName(DAEnums.EmployeeActveRule.ProgramStop), Convert.ToString((int)DAEnums.EmployeeActveRule.ProgramStop)));
        Common.AddOnChangeAttribute(ddlEmployeeActiveRule);

     

        // second reminder

        //ddlReminder2.Items.Insert(0, new ListItem("2nd Reminder", "-1"));

        //ddlReminder2.Items.Insert(1, new ListItem("Yes", "1"));
        //ddlReminder2.Items.Insert(2, new ListItem("No", "0"));

        //Common.AddOnChangeAttribute(ddlReminder2);

    
    }

    /// <summary>
    /// display existing data
    /// </summary>
    void DisplayData()
    {
        
        UniformIssuancePolicy objUniformIssuancePolicy = objUniformIssuancePolicyRepository.GetById(this.UniformIssuancePolicyID);

        if (objUniformIssuancePolicy != null)
        {
            ddlCreditExpireNumberOfMonth.SelectedValue = Common.GetIntString(objUniformIssuancePolicy.CreditExpireNumberOfMonths);
            ddlEmployeeActiveRule.SelectedValue = Common.GetIntString(objUniformIssuancePolicy.EmployeeActiveRule);
            ddlReminder1.SelectedValue = Common.GetIntString(objUniformIssuancePolicy.FirstReminderAlarm);
            ddlReminder2.SelectedValue = Common.GetIntString(objUniformIssuancePolicy.SecondReminderAlarm);
            ddlReminder3.SelectedValue = Common.GetIntString(objUniformIssuancePolicy.ThirdReminderAlarm);
            ddlExpirationReminder.SelectedValue = Common.GetIntString(objUniformIssuancePolicy.ExpirationReminder);
            txtExpireDate.Text = Common.GetDateString(objUniformIssuancePolicy.CreditExpireDate);
               //status
            ddlStatus.SelectedValue = objUniformIssuancePolicy.Status.ToString();

            if (objUniformIssuancePolicy.IsDateOfHiredTicked == true)
            {
                divExpireDate.Visible = false;
                divExpireMonth.Visible = true;
            }
            else
            {
                divExpireDate.Visible = true;
                divExpireMonth.Visible = false;
            }

        }

    }

    #endregion

    #region Events

    /// <summary>
    /// save data and redirect to next page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        
        /*
        if (ddlCreditExpireNumberOfMonth.SelectedValue != "0")
        {
            if (txtExpireDate.Text != "")
            {
                lblMsg.Text = "Please select either number of months or expire date";
                return;
            }
        }


        if(ddlCreditExpireNumberOfMonth.SelectedValue == "0" && txtExpireDate.Text == "")
        {
            lblMsg.Text = "Please select either number of months or expire date";
            return;
        }
         */

        if (divExpireDate.Visible == false)
        {
            if (ddlCreditExpireNumberOfMonth.SelectedValue == "0")
            {
                lblMsg.Text = "Please select number of months ";
                return;
            }
        }
        else
        {
            if (txtExpireDate.Text == "")
            {
                lblMsg.Text = "Please select expire date ";
                return;
            }
        }

        try
        {
            UniformIssuancePolicy objUniformIssuancePolicy = objUniformIssuancePolicyRepository.GetById(this.UniformIssuancePolicyID);

            objUniformIssuancePolicy.CreditExpireNumberOfMonths = Convert.ToInt32(ddlCreditExpireNumberOfMonth.SelectedValue);
            objUniformIssuancePolicy.EmployeeActiveRule = Convert.ToInt16(ddlEmployeeActiveRule.SelectedValue);
            objUniformIssuancePolicy.FirstReminderAlarm = Convert.ToInt32(ddlReminder1.SelectedValue);
            objUniformIssuancePolicy.SecondReminderAlarm = Convert.ToInt32(ddlReminder2.SelectedValue);
            objUniformIssuancePolicy.ThirdReminderAlarm = Convert.ToInt32(ddlReminder3.SelectedValue);
            objUniformIssuancePolicy.ExpirationReminder = Convert.ToInt32(ddlExpirationReminder.SelectedValue);

            if (divExpireDate.Visible == false)
            {
                objUniformIssuancePolicy.CreditExpireNumberOfMonths = Convert.ToInt32(ddlCreditExpireNumberOfMonth.SelectedValue);
                objUniformIssuancePolicy.CreditExpireDate = null;
            }
            else
            {
                objUniformIssuancePolicy.CreditExpireNumberOfMonths = null;
                objUniformIssuancePolicy.CreditExpireDate = Common.GetDate(txtExpireDate);
            }

            objUniformIssuancePolicy.Status = Convert.ToInt64(ddlStatus.SelectedValue);

            objUniformIssuancePolicyRepository.SubmitChanges();

            Response.Redirect("UniformIssuanceStep5.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType);
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch(Exception ex)
        {
            lblMsg.Text = "Error in saving record";
            ErrHandler.WriteError(ex);
        }
   
    }

    /// <summary>
    /// redirect to previous page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        Response.Redirect("UniformIssuanceStep3.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType);

    }
    
    #endregion



}//class
