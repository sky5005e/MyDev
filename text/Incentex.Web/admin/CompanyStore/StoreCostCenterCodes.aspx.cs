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
using System.Collections.Generic;

public partial class StoreCostCenterCodes : PageBase
{
    DataSet dsCostCenterCode;
    Common objcommon = new Common();
    StoreCostCenterCodeRepository objstorecostcentercode = new StoreCostCenterCodeRepository();
    string strMessgae = null;


    long CostCenterCode
    {
        get
        {
            if (ViewState["CostCenterCode"] == null)
                return 0;
            else
                return Convert.ToInt64(ViewState["CostCenterCode"]);
        }
        set
        {
            ViewState["CostCenterCode"] = value;
        }
    }

    long StoreID
    {
        get
        {
            if (ViewState["StoreID"] == null)
                return 0;
            else
                return Convert.ToInt64(ViewState["StoreID"]);
        }
        set
        {
            ViewState["StoreID"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Store Cost-Center Code";
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
                this.StoreID = Convert.ToInt64(Request.QueryString["id"].ToString());
                menuControl.PopulateMenu(0, 0, this.StoreID, 0, false);

                BindDatlist();

                ((Label)Master.FindControl("lblPageHeading")).Text = "General Store Information";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/CompanyStore/ViewCompanyStore.aspx";
            }
        }
    }

    /// <summary>
    /// BindDatlist()
    /// This method is used to bind the datalist
    /// from the INC_MenuMaster page.
    /// </summary>
    public void BindDatlist()
    {
        try
        {
            List<StoreCostCenterCode> lststorecode = objstorecostcentercode.GetAllQuery(this.StoreID);
            if (lststorecode.Count > 0)
            {
                dtstorecode.DataSource = lststorecode;
                dtstorecode.DataBind();
            }
            else
            {
                dtstorecode.DataSource = null;
                dtstorecode.DataBind();
            }
        }
        catch (Exception ex)
        {
            strMessgae = ex.Message.ToString();
        }
    }
    protected void dtstorecode_ItemDataBound(object sender, DataListItemEventArgs e)
    {


    }
    protected void dtstorecode_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "deletevalue")
        {
            try
            {
                if (!base.CanDelete)
                {
                    base.RedirectToUnauthorised();
                }

                long CostCenterCode = Convert.ToInt64(e.CommandArgument.ToString());
                if (objstorecostcentercode.DeleteCostCenterCode(CostCenterCode))
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = "Unable to delete record as this record exists in other detail table";
                    return;
                }
                else
                {
                    lblErrorMessage.Visible = false;
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "@@@@MyPopUpScript", strMaessage, true);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        else if (e.CommandName == "editvalue")
        {
            try
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                long CostCenterCode = Convert.ToInt64(e.CommandArgument.ToString());
                StoreCostCenterCode objstorecentercode = objstorecostcentercode.GetById(CostCenterCode);

                if (objstorecentercode != null)
                {
                    btnSubmit.Text = "Edit";
                    this.CostCenterCode = objstorecentercode.CostCenterCodeID;
                    txtCode.Text = objstorecentercode.Code;
                    txtCode.Focus();
                }

                modal.Show();
            }
            catch (Exception ex)
            {
            }
        }

        BindDatlist();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        StoreCostCenterCode objcostcode = new StoreCostCenterCode();

        if (btnSubmit.Text == "Add")
        {
            //Check dulication here when add new record.
            if (objstorecostcentercode.CheckIfExist(this.CostCenterCode, this.StoreID, txtCode.Text) != null)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
                //lblMessage.Text = "Record saved successfully!";
            }
            else
            {
                objcostcode.StoreID = this.StoreID;
                objcostcode.Code = txtCode.Text;
                objstorecostcentercode.Insert(objcostcode);
                objstorecostcentercode.SubmitChanges();
            }
        }
        else
        {
            //Check dulication here in edit mode
            if (objstorecostcentercode.CheckIfExist(this.CostCenterCode, this.StoreID, txtCode.Text) != null)
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }
            else
            {
                objstorecostcentercode.UpdateCostCenterCode(this.CostCenterCode, StoreID, txtCode.Text.Trim());
                lblMessage.Text = "Record updated successfully!";
            }
        }

        BindDatlist();
    }
    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        btnSubmit.Text = "Add";
        txtCode.Text = string.Empty;

        lblMessage.Text = string.Empty;
        modal.Controls.Clear();
        Page.SetFocus(txtCode);
        modal.Show();
    }
}

