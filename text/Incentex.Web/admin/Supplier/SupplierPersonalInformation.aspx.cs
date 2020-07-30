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
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using Incentex.BE;
using Incentex.DA;

public partial class admin_Supplier_SupplierPersonalInformation : PageBase
{
    #region Properties

    SupplierEmployeeRepository objRepo = new SupplierEmployeeRepository();
    SupplierEmployee obj = new SupplierEmployee();

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

    Int64 UserInfoID
    {
        get
        {
            if (ViewState["UserInfoID"] == null)
            {
                ViewState["UserInfoID"] = 0;
            }
            return Convert.ToInt64(ViewState["UserInfoID"]);
        }
        set
        {
            ViewState["UserInfoID"] = value;
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
                if (this.SupplierEmployeeID == 0)
                {
                    Response.Redirect("~/admin/Supplier/BasicSupplierInformation.aspx?Id=" + this.SupplierEmployeeID);
                }
                ((Label)Master.FindControl("lblPageHeading")).Text = "Personal Information";
                //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to  Manage Incentex Employee</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Supplier/ViewAddSupplierEmployee.aspx?Id=" + this.SupplierID;

                menucontrol.PopulateMenu(3, 4, this.SupplierID, this.SupplierEmployeeID, true);
            }
            else
            {
                Response.Redirect("ViewSupplierEmployee.aspx");
            }

            DisplayData();
        }
    }

    void DisplayData()
    {
        if (this.SupplierEmployeeID != 0)
        {
            objRepo = new SupplierEmployeeRepository();
            obj = objRepo.GetById(this.SupplierEmployeeID);

            if (obj != null)
            {
                txtCompanyComputer.Text = obj.CompanyComputerName;
                txtComputerBrand.Text = obj.ComputerBrand;
                txtSerialNumberComputer.Text = obj.ComputerSerialNo;
                txtDateIssuedComputer.Text = obj.ComputerDateOfIssue!=null? Convert.ToDateTime(obj.ComputerDateOfIssue).ToShortDateString():"";
                txtMobileNumber.Text = obj.MobileNo;
                txtMobilePhone.Text = obj.MobilePhone;
                txtSerialNumberMobile.Text = obj.MobileSerialNo;
                            
                txtDateIssuedMobile.Text = obj.MobileDateOfIssue!=null? Convert.ToDateTime(obj.MobileDateOfIssue).ToShortDateString():"";
                txtBirthDay.Text =obj.Birthdate!=null? Convert.ToDateTime(obj.Birthdate).ToShortDateString():"";
                txtSpouseName.Text = obj.SpouseName;
                txtChildrenName.Text = obj.ChildrensName;
            }
        }
    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            objRepo = new SupplierEmployeeRepository();
            obj = objRepo.GetById(this.SupplierEmployeeID);
            obj.CompanyComputerName = txtCompanyComputer.Text;
            obj.ComputerBrand = txtComputerBrand.Text;
            obj.ComputerSerialNo = txtSerialNumberComputer.Text;
            obj.ComputerDateOfIssue = Convert.ToDateTime(txtDateIssuedComputer.Text);
            obj.MobileNo = txtMobileNumber.Text;
            obj.MobilePhone = txtMobilePhone.Text;
            obj.MobileSerialNo = txtSerialNumberMobile.Text;
            obj.MobileDateOfIssue = Convert.ToDateTime(txtDateIssuedMobile.Text);
            obj.Birthdate = Convert.ToDateTime(txtBirthDay.Text);
            obj.SpouseName = txtSpouseName.Text;
            obj.ChildrensName = txtChildrenName.Text;
            objRepo.SubmitChanges();

            Response.Redirect("SupplierEmployeeNotes.aspx?id=" + Convert.ToInt32(this.SupplierID.ToString()) + "&SubId=" + Convert.ToInt32(this.SupplierEmployeeID.ToString()));
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
