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
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;


public partial class admin_IncentexEmployee_PaymentInfo : PageBase
{

    #region Properties

    Int64 IncentexEmployeeID
    {
        get
        {
            if (ViewState["IncentexEmployeeID"] == null)
            {
                ViewState["IncentexEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["IncentexEmployeeID"]);
        }
        set
        {
            ViewState["IncentexEmployeeID"] = value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Incentex Employee";
            base.ParentMenuID = 10;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Payment Information";
            // ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to Employee listing</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/IncentexEmployee/ViewIncentexEmployee.aspx";

            if (Request.QueryString.Count > 0)
            {
                this.IncentexEmployeeID = Convert.ToInt64(Request.QueryString.Get("Id"));
                if (this.IncentexEmployeeID == 0)
                {
                    Response.Redirect("~/admin/IncentexEmployee/BasicInformation.aspx?Id=" + this.IncentexEmployeeID);
                }

                manuControl.PopulateMenu(2, 0, this.IncentexEmployeeID, 0, false);

            }
            else
            {
                Response.Redirect("ViewIncentexEmployee.aspx");
            }
            lstEmployeeBenefits.DataBind();
            lstPayPeriods.DataBind();

            DisplayData();

        }
    }

    void DisplayData()
    {
        IncentexEmployeeRepository objRepo = new IncentexEmployeeRepository();
        IncentexEmployee obj = objRepo.GetById(this.IncentexEmployeeID);

        if (obj != null)
        {

            txtHourlyRate.Text = obj.HourlyRate.ToString();
            txtWeeklySalary.Text = obj.WeeklySalary.ToString();
            txtMonthlySalary.Text = obj.MonthlySalary.ToString();
            txtCommissionRate.Text = obj.CommisionRate.ToString();
            txtCommissionDetails.Text = obj.Commisiondetail;
            txtDateHired.Text = Common.GetDateString(obj.DateOfHire);

            string EmployeeBenefits = obj.EmployeeBenefit;
            string PayPeriods = obj.PayPeriodDetail;

            DisplayInList(EmployeeBenefits, lstEmployeeBenefits);
            DisplayInList(PayPeriods, lstPayPeriods);

        }
    }


    void DisplayInList(string Ids, DataList dl)
    {

        if (!string.IsNullOrEmpty(Ids))
        {
            string[] idSplit = Ids.Split(',');

            foreach (DataListItem li in dl.Items)
            {
                CheckBox chk = li.FindControl("chk") as CheckBox;
                HiddenField hdnId = li.FindControl("hdnId") as HiddenField;
                HtmlGenericControl spChk = li.FindControl("spChk") as HtmlGenericControl;

                foreach (string id in idSplit)
                {
                    if (id.Equals(hdnId.Value))
                    {
                        chk.Checked = true;
                        spChk.Attributes.Add("class", "custom-checkbox_checked alignleft");
                    }
                }
            }
        }

    }


    /// <summary>
    ///  Bind EmployeeBenefits List
    ///  Amit 20Sep2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstEmployeeBenefits_DataBinding(object sender, EventArgs e)
    {
        LookupRepository objRepo = new LookupRepository();
        List<INC_Lookup> objList = objRepo.GetByLookupCode(DAEnums.LookupCodeType.IncentexEmployeeBenefits);
        lstEmployeeBenefits.DataSource = objList;
    }


    /// <summary>
    ///  Bind PayPeriods List
    ///  Amit 20Sep2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstPayPeriods_DataBinding(object sender, EventArgs e)
    {
        LookupRepository objRepo = new LookupRepository();
        List<INC_Lookup> objList = objRepo.GetByLookupCode(DAEnums.LookupCodeType.IncentexEmployeePayPeriods);
        lstPayPeriods.DataSource = objList;
    }


    /// <summary>
    /// Update Data
    /// Amit 20Sep2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            IncentexEmployeeRepository objRepo = new IncentexEmployeeRepository();

            IncentexEmployee obj = objRepo.GetById(this.IncentexEmployeeID);

            obj.HourlyRate = Common.GetDecimal(txtHourlyRate.Text);
            obj.WeeklySalary = Common.GetDecimal(txtWeeklySalary.Text);
            obj.MonthlySalary = Common.GetDecimal(txtMonthlySalary.Text);
            obj.CommisionRate = Common.GetDecimal(txtCommissionRate.Text);
            obj.Commisiondetail = txtCommissionDetails.Text;
            obj.DateOfHire = Common.GetDate(txtDateHired);

            obj.EmployeeBenefit = GetCommaSeparatedId(lstEmployeeBenefits);
            obj.PayPeriodDetail = GetCommaSeparatedId(lstPayPeriods);

            objRepo.SubmitChanges();

            lblMsg.Text = "Record Saved Successfully ...";
            Response.Redirect("MenuDataAccess.aspx?Id=" + this.IncentexEmployeeID, false);
            DisplayData();
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in saving record ...";
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Get Comma Separated list of ids
    /// </summary>
    /// <param name="lst"></param>
    /// <returns></returns>
    string GetCommaSeparatedId(DataList lst)
    {
        string Ids = "";

        foreach (DataListItem li in lst.Items)
        {
            CheckBox chk = li.FindControl("chk") as CheckBox;
            HiddenField hdnId = li.FindControl("hdnId") as HiddenField;

            if (chk.Checked)
            {
                Ids += hdnId.Value + ",";
            }
        }
        return Ids;
    }

}
