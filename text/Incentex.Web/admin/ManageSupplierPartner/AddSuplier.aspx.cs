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
using Incentex.DAL.Common;


public partial class admin_ManageSupplierPartner_AddSuplier : PageBase
{
    Int64 SupplierPartnerId
    {
        get
        {
            if (ViewState["SupplierPartnerId"] == null)
            {
                ViewState["SupplierPartnerId"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierPartnerId"]);
        }
        set
        {
            ViewState["SupplierPartnerId"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Supplier Partner";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Supllier Partner Information";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/ManageSupplierPartner/ViewSupplierPartner.aspx";
            //lblMsg.Visible = false;
            if (Request.QueryString.Count > 0)
            {
                this.SupplierPartnerId = Convert.ToInt64(Request.QueryString.Get("Id"));

                if (this.SupplierPartnerId == 0 && !base.CanAdd)
                {
                    base.RedirectToUnauthorised();
                }
                //manuControl.PopulateMenu(0, 0, this.SupplierPartnerId, 0, false);
            }
            else
            {
                Response.Redirect("~/admin/ManageSupplierPartner/ViewSupplierPartner.aspx");
            }
            DisplayData();
        }

    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        if (this.SupplierPartnerId == 0)
        {
            SupplierPartner ObjSPTbl = new SupplierPartner();
            ObjSPTbl.Name = txtSupllierName.Text.Trim();
            ObjSPTbl.URL = TxtUrl.Text.Trim();
            ObjSPTbl.LoginName = TxtLoginName.Text.Trim();
            ObjSPTbl.Password = TxtPassword.Text.Trim();
            //Status
            if (Convert.ToInt16(DdlStatus.SelectedValue) == 1)
            {
                ObjSPTbl.Status = true;
            }
            if (Convert.ToInt16(DdlStatus.SelectedValue) == 2)
            {
                ObjSPTbl.Status = false;
            }
            ObjSPTbl.CratedDate = Convert.ToDateTime(TxtCreatedDate.Text);
            SupplierPartnerRepo ObjSPRepo = new SupplierPartnerRepo();
            ObjSPRepo.Insert(ObjSPTbl);
            ObjSPRepo.SubmitChanges();
            lblMsg1.Text = "Record inserted Successfully...";
        }
        if (this.SupplierPartnerId != 0)
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            SupplierPartnerRepo objRepo = new SupplierPartnerRepo();
            SupplierPartner objSupplier = objRepo.GetById(this.SupplierPartnerId);
           
              objSupplier.Name= txtSupllierName.Text;
              objSupplier.URL = TxtUrl.Text;
              objSupplier.LoginName = TxtLoginName.Text;
              objSupplier.Password =TxtPassword.Text;
             objSupplier.CratedDate = Convert.ToDateTime(TxtCreatedDate.Text);
             bool vari = objSupplier.Status;
            if (DdlStatus.Items.FindByValue("1").Selected == true)
            {

                objSupplier.Status = true;
            }
            if (DdlStatus.Items.FindByValue("2").Selected == true)
            {
                objSupplier.Status = false; 
            }

            //SupplierPartnerRepo ObjSPRepo = new SupplierPartnerRepo();
            //ObjSPRepo.Insert(ObjSPTbl);
            objRepo.SubmitChanges();

            lblMsg1.Text = "Record Updated Successfully...";
        }
    }
    void DisplayData()
    {
        SupplierPartnerRepo objRepo = new SupplierPartnerRepo();
        SupplierPartner objSupplier = objRepo.GetById(this.SupplierPartnerId);


        if (objSupplier != null)
        {

            //display supplier info
            // ddlCompany.SelectedValue = objSupplier.CompanyId.ToString();
            txtSupllierName.Text = objSupplier.Name;
            TxtUrl.Text = objSupplier.URL;
            TxtLoginName.Text = objSupplier.LoginName;
            TxtPassword.Text=objSupplier.Password;
            TxtCreatedDate.Text   = Common.GetDateString(objSupplier.CratedDate);
            if (objSupplier.Status == true)
            {

                DdlStatus.Items.FindByValue("1").Selected = true;
            }
            if (objSupplier.Status == false)
            {
                DdlStatus.Items.FindByValue("2").Selected = true;
            }


        }

    }
}
