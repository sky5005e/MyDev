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
using System.Collections.Generic;
using Incentex.BE;
using Incentex.DA;

public partial class admin_Supplier_SupplierEmployeeNotes : PageBase
{
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
    Common objcomm = new Common();
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
                    Response.Redirect("~/admin/Supplier/BasicInformation.aspx?Id=" + this.SupplierEmployeeID);
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Supplier Employee Notes";
                //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to  Manage Incentex Employee</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Supplier/ViewAddSupplierEmployee.aspx?Id=" + this.SupplierID;

                menucontrol.PopulateMenu(3, 5, this.SupplierID, this.SupplierEmployeeID, true);
            }
            else
            {
                Response.Redirect("ViewSupplierEmployee.aspx");
            }

            DisplayData(sender, e);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtNote.Text))
        {
            return;
        }
        try
        {
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            NoteDetail obj = new NoteDetail()
            {
                ForeignKey = this.SupplierEmployeeID,
                Notecontents = txtNote.Text
                ,
                NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.SupplierEmployee)
                ,
                CreatedBy = IncentexGlobal.CurrentMember.UserInfoID
                ,
                CreateDate = DateTime.Now
                ,
                UpdateDate = DateTime.Now
                ,
                UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID
            };

            objRepo.Insert(obj);
            objRepo.SubmitChanges();
            DisplayNotes();
            txtNote.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }
    }
    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        modalAddnotes.X = Convert.ToInt32(Convert.ToDecimal(hfX.Value)) - 200;
        modalAddnotes.Y = Convert.ToInt32(Convert.ToDecimal(hfY.Value)) - 120;
        modalAddnotes.Show();
    }
    void DisplayData(object sender, EventArgs e)
    {

        if (this.SupplierEmployeeID != 0)
        {
            DisplayNotes();
        }
    }

    void DisplayNotes()
    {
        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        List<NoteDetail> objList = objRepo.GetByForeignKeyId(this.SupplierEmployeeID, Incentex.DAL.Common.DAEnums.NoteForType.SupplierEmployee);
        txtNoteHistory.Text = string.Empty;
        foreach (NoteDetail obj in objList)
        {
            txtNoteHistory.Text += "Created Date : " + Convert.ToDateTime(obj.CreateDate).ToString("dd-MMM-yyyy") + "\n";
            txtNoteHistory.Text += "Created Time : " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n";

            UserInformationRepository objUserRepo = new UserInformationRepository();
            UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);

            if (objUser != null)
            {
                txtNoteHistory.Text += "Created By : " + objUser.FirstName + " " + objUser.LastName + "\n";
            }


            txtNoteHistory.Text += "Note : " + obj.Notecontents + "\n";
            txtNoteHistory.Text += "---------------------------------------------------------------------------\n";


        }

    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("ShoppingSetting.aspx?id=" + Convert.ToInt32(this.SupplierID.ToString()) + "&SubId=" + Convert.ToInt32(this.SupplierEmployeeID.ToString()));
        }
        catch (Exception ex)
        {

        }
    }
}
