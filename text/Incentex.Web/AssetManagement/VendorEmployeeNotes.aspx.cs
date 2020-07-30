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

public partial class AssetManagement_VendorEmployeeNotes : PageBase
{
    #region Properties
    Int64 NoteId
    {
        get
        {
            if (ViewState["NoteId"] == null)
            {
                ViewState["NoteId"] = 0;
            }
            return Convert.ToInt64(ViewState["NoteId"]);
        }
        set
        {
            ViewState["NoteId"] = value;
        }
    }
    Int64 VendorID
    {
        get
        {
            if (ViewState["VendorID"] == null)
            {
                ViewState["VendorID"] = 0;
            }
            return Convert.ToInt64(ViewState["VendorID"]);
        }
        set
        {
            ViewState["VendorID"] = value;
        }
    }
    Int64 VendorEmployeeID
    {
        get
        {
            if (ViewState["VendorEmployeeID"] == null)
            {
                ViewState["VendorEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["VendorEmployeeID"]);
        }
        set
        {
            ViewState["VendorEmployeeID"] = value;
        }
    }

    Common objcomm = new Common();
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.MenuModuleId = Incentex.DAL.Common.DAEnums.ManageID.EquipmentVendorEmployee;
            if (Request.QueryString.Count > 0)
            {
                base.MenuItem = "Manage Employee";
                base.ParentMenuID = 50;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                this.VendorID = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.VendorEmployeeID = Convert.ToInt64(Request.QueryString.Get("SubId"));

                ((Label)Master.FindControl("lblPageHeading")).Text = "Vendor Employee Notes";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/MenuAccess.aspx?Id=" + this.VendorID + "&SubId=" + this.VendorID; 


                // menuControl.PopulateMenu(3, 0, this.SupplierID, this.SupplierEmployeeID, true);
                menuControl.PopulateMenu(3, 0, this.VendorEmployeeID, this.VendorID, false);

            }
            else
            {
                Response.Redirect("EmployeeList.aspx");
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
                ForeignKey = this.VendorEmployeeID,
                Notecontents = txtNote.Text
                ,
                NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement)
                ,
                CreatedBy = IncentexGlobal.CurrentMember.UserInfoID
                ,
                CreateDate = DateTime.Now
                ,
                UpdateDate = DateTime.Now
                ,
                UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID
                ,
                SpecificNoteFor = "Vendor Employee"
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
        modalAddnotes.Show();
    }

    void DisplayData(object sender, EventArgs e)
    {

        if (this.VendorEmployeeID != 0)
        {
            DisplayNotes();
        }
    }

    void DisplayNotes()
    {
        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        List<NoteDetail> objList = objRepo.GetNotesForCACEId(this.VendorEmployeeID, Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement,"Vendor Employee");
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
            Response.Redirect("VendorEmployeeAttachments.aspx?SubId=" + this.VendorEmployeeID + "&Id=" + this.VendorID);
        }
        catch (Exception ex)
        {

        }
    }
}
