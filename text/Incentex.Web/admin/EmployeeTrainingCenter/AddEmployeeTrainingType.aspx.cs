using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;

public partial class admin_EmployeeTrainingCenter_AddEmployeeTrainingType : PageBase
{
    #region Data Member's
    EmployeeTrainingCenterRepository objEmployeeTrainingRepo = new EmployeeTrainingCenterRepository();
    LookupRepository objLookRep = new LookupRepository();
    long iLookUpID
    {
        get
        {
            if (ViewState["iLookUpID"] != null)
                return Convert.ToInt64(ViewState["iLookUpID"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["iLookUpID"] = value;
        }
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Employee Training Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            BindData();

            ((Label)Master.FindControl("lblPageHeading")).Text = "Add Employee Training Type";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/EmployeeTrainingCenter/EmployeeTrainingCenter.aspx";
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd && !base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        if (objEmployeeTrainingRepo.CheckEmployeeTrainingExistence(txtEmployeeTrainingType.Text.Trim(), iLookUpID) == 0)
        {
            INC_Lookup objdocumenttype = new INC_Lookup();

            objdocumenttype.iLookupCode = "EmployeeTrainingType";
            objdocumenttype.sLookupName = txtEmployeeTrainingType.Text.Trim();
            objdocumenttype.Val1 = txtpassword.Text.Trim();

            if (iLookUpID == 0)
            {
                //If password is set than CreateBy set 
                if (!string.IsNullOrEmpty(txtpassword.Text.Trim()))
                    objdocumenttype.sLookupIcon = IncentexGlobal.CurrentMember.UserInfoID.ToString();

                objEmployeeTrainingRepo.Insert(objdocumenttype);
                lblMsg.Text = "Records sucessfully added";
            }
            else
            {
                //Add FirstTime password in edit mode
                if (trpassword.Visible && !string.IsNullOrEmpty(txtpassword.Text.Trim()))
                    objdocumenttype.sLookupIcon = IncentexGlobal.CurrentMember.UserInfoID.ToString();


                objLookRep.UpdateLooupById(iLookUpID, txtEmployeeTrainingType.Text.Trim(), txtpassword.Text.Trim(), objdocumenttype.sLookupIcon);
                objLookRep.SubmitChanges();
                lblMsg.Text = "Records sucessfully Updated";
                iLookUpID = 0;
                trpassword.Visible = true;
            }

            objEmployeeTrainingRepo.SubmitChanges();
            txtEmployeeTrainingType.Text = string.Empty;
            txtpassword.Text = string.Empty;
            BindData();
        }
        else
        {
            lblMsg.Text = "Training Video Type is already exists.";
        }
    }

    protected void grdView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteTraining")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            LookupRepository objLookRep = new LookupRepository();
            objLookRep.DeleteLooupById(Convert.ToInt64(e.CommandArgument), "EmployeeTrainingType");
            lblMsg.Text = "Record deleted successfully";
            BindData();
        }
        else if (e.CommandName == "edit")
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            iLookUpID = Convert.ToInt64(e.CommandArgument);
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            HiddenField hdnlookupname = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnlookupname"));
            HiddenField hdnpassword = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdnpassword"));
            HiddenField hdncreateBy = (HiddenField)(((System.Web.UI.WebControls.TableRow)(row)).FindControl("hdncreateBy"));

            txtEmployeeTrainingType.Text = hdnlookupname.Value;
            txtpassword.Text = hdnpassword.Value;

            //Password display only superadmin and user who create this
            if ((IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin)) &&
                (!string.IsNullOrEmpty(hdncreateBy.Value) ? Convert.ToInt64(hdncreateBy.Value) != IncentexGlobal.CurrentMember.UserInfoID : false))
            {
                trpassword.Visible = false;
            }
            else
            {
                trpassword.Visible = true;
            }
        }
    }

    protected void grdView_RowEditing(object sender, GridViewEditEventArgs e)
    { }

    private void BindData()
    {

        grdView.DataSource = objLookRep.GetByLookup("EmployeeTrainingType");
        grdView.DataBind();
    }
    #endregion
}
