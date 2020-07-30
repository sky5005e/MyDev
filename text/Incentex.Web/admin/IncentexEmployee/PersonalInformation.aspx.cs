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

public partial class admin_IncentexEmployee_PersonalInformation : PageBase
{
    #region Properties

    IncentexEmployeeRepository objRepo = new IncentexEmployeeRepository();
    IncentexEmployee obj = new IncentexEmployee();

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

            if (Request.QueryString.Count > 0)
            {
                this.IncentexEmployeeID = Convert.ToInt64(Request.QueryString.Get("Id"));
                if (this.IncentexEmployeeID == 0)
                {
                    Response.Redirect("~/admin/IncentexEmployee/BasicInformation.aspx?Id=" + this.IncentexEmployeeID);
                }
                ((Label)Master.FindControl("lblPageHeading")).Text = "Personal Information";
                //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to  Manage Incentex Employee</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/IncentexEmployee/ViewIncentexEmployee.aspx";
                
                menucontrol.PopulateMenu(4, 0, this.IncentexEmployeeID, 0,false);
            }
            else
            {
                Response.Redirect("ViewIncentexEmployee.aspx");
            }
            
            DisplayData();
        }
    }

    void DisplayData()
    {
        if (this.IncentexEmployeeID != 0)
        {
            objRepo = new IncentexEmployeeRepository();
            obj = objRepo.GetById(this.IncentexEmployeeID);

            if (obj != null)
            {
                txtCompanyComputer.Text = obj.CompanyComputerName;
                txtComputerBrand.Text = obj.ComputerBrand;
                txtSerialNumberComputer.Text = obj.ComputerSerialNo;
                txtDateIssuedComputer.Text = Convert.ToDateTime(obj.ComputerDateOfIssue).ToShortDateString();
                txtMobileNumber.Text = obj.MobileNo;
                txtMobilePhone.Text = obj.MobilePhone;
                txtSerialNumberMobile.Text = obj.MobileSerialNo;
                txtDateIssuedMobile.Text = Convert.ToDateTime(obj.MobileDateOfIssue).ToShortDateString();
                txtBirthDay.Text = Convert.ToDateTime(obj.Birthdate).ToShortDateString(); 
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

            objRepo = new IncentexEmployeeRepository();
            obj = objRepo.GetById(this.IncentexEmployeeID);
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
            obj.ChildrensName =txtChildrenName.Text;
            objRepo.SubmitChanges();

            Response.Redirect("EmployeeNotes.aspx?id=" + this.IncentexEmployeeID);
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