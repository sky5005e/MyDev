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

public partial class admin_Supplier_SupplierPaymentInfo : PageBase
{
    #region Properties

    Int64 SupplierEmployeeID
    {
        get
        {
            if (ViewState["SupplierEmployeeID"] == null)
            {
                ViewState["SupplierEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierEmployeeID"]);
        }
        set
        {
            ViewState["SupplierEmployeeID"] = value;
        }
    }
    Int64 SupplierID
    {
        get
        {
            if (ViewState["SupplierID"] == null)
            {
                ViewState["SupplierID"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierID"]);
        }
        set
        {
            ViewState["SupplierID"] = value;
        }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Employee";
            base.ParentMenuID = 18;

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
                if (Request.QueryString["SubId"] != null)
                {
                    this.SupplierEmployeeID = Convert.ToInt64(Request.QueryString.Get("SubId"));
                }
                if (Request.QueryString["id"] != "0")
                {
                    this.SupplierID = Convert.ToInt64(Request.QueryString.Get("id"));
                    // txtCompany.Text = Convert.ToString(objSupplierRepos.GetById(this.SupplierID).CompanyName);
                }
                ((Label)Master.FindControl("lblPageHeading")).Text = "Payment Information";
                // ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to Employee listing</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Supplier/ViewAddSupplierEmployee.aspx?id=" + this.SupplierID;

                manuControl.PopulateMenu(3, 2, this.SupplierID, this.SupplierEmployeeID, true);

            }
            else
            {
                Response.Redirect("ViewSupplier.aspx");
            }
            lstEmployeeBenefits.DataBind();
            lstPayPeriods.DataBind();

            DisplayData();

        }
    }
    void DisplayData()
    {
        SupplierEmployeeRepository objRepo = new SupplierEmployeeRepository();
        SupplierEmployee obj = objRepo.GetById(this.SupplierEmployeeID);

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
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstEmployeeBenefits_DataBinding(object sender, EventArgs e)
    {
        LookupRepository objRepo = new LookupRepository();
        List<INC_Lookup> objList = objRepo.GetByLookupCode(DAEnums.LookupCodeType.SupplierEmployeeBenefits);
        lstEmployeeBenefits.DataSource = objList;
    }

    /// <summary>
    ///  Bind PayPeriods List
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstPayPeriods_DataBinding(object sender, EventArgs e)
    {
        LookupRepository objRepo = new LookupRepository();
        List<INC_Lookup> objList = objRepo.GetByLookupCode(DAEnums.LookupCodeType.SupplierEmployeePayPeriods);
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

            SupplierEmployeeRepository objRepo = new SupplierEmployeeRepository();

            SupplierEmployee obj = objRepo.GetById(this.SupplierEmployeeID);

            obj.HourlyRate = Common.GetDecimal(txtHourlyRate.Text);
            obj.WeeklySalary = Common.GetDecimal(txtWeeklySalary.Text);
            obj.MonthlySalary = Common.GetDecimal(txtMonthlySalary.Text);
            obj.CommisionRate = Common.GetDecimal(txtCommissionRate.Text);
            obj.Commisiondetail = txtCommissionDetails.Text;
            obj.DateOfHire = Common.GetDate(txtDateHired);

            obj.EmployeeBenefit = GetCommaSeparatedId(lstEmployeeBenefits);
            obj.PayPeriodDetail = GetCommaSeparatedId(lstPayPeriods);

            objRepo.SubmitChanges();

            lblMsg.Text = "Recoed Saved Successfully ...";
            Response.Redirect("SupplierMenuDataAccess.aspx?id=" + Convert.ToInt32(this.SupplierID.ToString()) + "&SubId=" + Convert.ToInt32(this.SupplierEmployeeID.ToString()));
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
