using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Supplier_ShoppingSetting : PageBase
{
    Int64 SupplierID
    {
        get
        {
            if (ViewState["SupplierID"] == null)
            {
                ViewState["SupplierID"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplieID"]);
        }
        set
        {
            ViewState["SupplierID"] = value;
        }
    }
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
    Int64 UserinfoID
    {
        get
        {
            if (ViewState["UserinfoID"] == null)
            {
                ViewState["UserinfoID"] = 0;
            }
            return Convert.ToInt64(ViewState["UserinfoID"]);
        }
        set
        {
            ViewState["UserinfoID"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
                    SupplierEmployee objSupplierEmployee = new SupplierEmployeeRepository().GetById(this.SupplierEmployeeID);
                    this.UserinfoID = Convert.ToInt64(objSupplierEmployee.UserInfoID);

                    if (this.SupplierEmployeeID == 0)
                    {
                        Response.Redirect("~/admin/Supplier/BasicInformation.aspx?Id=" + this.SupplierEmployeeID);
                    }

                    ((Label)Master.FindControl("lblPageHeading")).Text = "Storefront Settings";
                    //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to  Manage Supplier Employee</span>";
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Supplier/ViewAddSupplierEmployee.aspx?Id=" + this.SupplierID;

                    menucontrol.PopulateMenu(3, 6, this.SupplierID, this.SupplierEmployeeID, true);
                    BindData();
                    DisplayData();
                }
                else
                {
                    Response.Redirect("ViewSupplierEmployee.aspx");
                }


            }
        }
        catch (Exception)
        {


        }
    }
    private void BindData()
    {
        try
        {

            //Email Management System Start
            ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();
            dtManageEmail.DataSource = objManageEmailRepo.GetEmailControl();
            dtManageEmail.DataBind();

            //Email Management System End

        }
        catch (Exception)
        {


        }
    }
    private void DisplayData()
    {
        try
        {
            CheckBox chk;
            HiddenField lblId;
            //Manage Email Start
            List<SupplierEmpManageEmail> lstSupplierEmpManageEmail = new SupplierEmpManageEmailRepository().GetEmailRightsByUserInfoID(this.UserinfoID);

            foreach (DataListItem dtM in dtManageEmail.Items)
            {
                chk = dtM.FindControl("chkManageEmail") as CheckBox;
                lblId = dtM.FindControl("hdnManageEmail") as HiddenField;
                HtmlGenericControl dvChk = dtM.FindControl("ManageEmailspan") as HtmlGenericControl;

                foreach (SupplierEmpManageEmail objMenu in lstSupplierEmpManageEmail)
                {

                    if (objMenu.ManageEmailID.ToString().Equals(lblId.Value))
                    {
                        chk.Checked = true;
                        dvChk.Attributes.Add("class", "custom-checkbox_checked");
                        break;
                    }

                }

            }
            //Manage Email End
        }
        catch (Exception)
        {


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

            //Manage Email Start
            //Delete from SupplierEmpManageEmail
            SupplierEmpManageEmailRepository objSupplierEmpManageEmailRepodel = new SupplierEmpManageEmailRepository();
            SupplierEmpManageEmail objSupplierEmpManageEmaildel = new SupplierEmpManageEmail();

            List<SupplierEmpManageEmail> lst = objSupplierEmpManageEmailRepodel.GetEmailRightsByUserInfoID(this.UserinfoID);

            foreach (SupplierEmpManageEmail l in lst)
            {
                objSupplierEmpManageEmailRepodel.Delete(l);
                objSupplierEmpManageEmailRepodel.SubmitChanges();
            }
            //Insert in SupplierEmpManageEmail
            foreach (DataListItem dt in dtManageEmail.Items)
            {
                if (((CheckBox)dt.FindControl("chkManageEmail")).Checked == true)
                {
                    SupplierEmpManageEmailRepository objSupplierEmpManageEmailRepins = new SupplierEmpManageEmailRepository();
                    SupplierEmpManageEmail objSupplierEmpManageEmailins = new SupplierEmpManageEmail();
                    objSupplierEmpManageEmailins.UserInfoID = this.UserinfoID;
                    objSupplierEmpManageEmailins.ManageEmailID = Convert.ToInt64(((HiddenField)dt.FindControl("hdnManageEmail")).Value);

                    objSupplierEmpManageEmailRepins.Insert(objSupplierEmpManageEmailins);
                    objSupplierEmpManageEmailRepins.SubmitChanges();
                }

            }

            //Manage Email End
            Response.Redirect("ViewSupplier.aspx?id=" + Convert.ToInt32(this.SupplierID.ToString()) + "&SubId=" + Convert.ToInt32(this.SupplierEmployeeID.ToString()));
        }
        catch (Exception)
        {


        }
    }
}
