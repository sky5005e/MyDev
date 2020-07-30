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

public partial class AssetManagement_VendorNotes : PageBase
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
    Int64 EquipmentVendorID
    {
        get
        {
            if (ViewState["EquipmentVendorID"] == null)
            {
                ViewState["EquipmentVendorID"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentVendorID"]);
        }
        set
        {
            ViewState["EquipmentVendorID"] = value;
        }
    }
   

    Common objcomm = new Common();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Company";
            base.ParentMenuID = 50;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            Common.MenuModuleId = Incentex.DAL.Common.DAEnums.ManageID.EquipmentVendor;
            if (Request.QueryString.Count > 0)
            {


                this.EquipmentVendorID = Convert.ToInt64(Request.QueryString.Get("Id"));
               

                ((Label)Master.FindControl("lblPageHeading")).Text = "Company Notes";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/BasicVendorInformation.aspx?Id=" + this.EquipmentVendorID;


                // menuControl.PopulateMenu(3, 0, this.SupplierID, this.SupplierEmployeeID, true);
                menuControl.PopulateMenu(1, 0, this.EquipmentVendorID, 0, false);

            }
            //else
            //{
            //    Response.Redirect("VendorList.aspx");
            //}

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
                ForeignKey = this.EquipmentVendorID,
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
                SpecificNoteFor = "Vendor"
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

        if (this.EquipmentVendorID != 0)
        {
            DisplayNotes();
        }
    }

    void DisplayNotes()
    {
        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        List<NoteDetail> objList = objRepo.GetNotesForCACEId(this.EquipmentVendorID, Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement, "Vendor");
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
            Response.Redirect("VendorAttachments.aspx?Id=" + this.EquipmentVendorID);
        }
        catch (Exception ex)
        {

        }
    }
}
