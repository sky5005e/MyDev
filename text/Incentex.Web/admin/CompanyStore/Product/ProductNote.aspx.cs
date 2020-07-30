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
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.BE;
using Incentex.DA;
using commonlib;

public partial class admin_ProductManagement_ProductNote :PageBase
{
    Common objcomm = new Common();
    Int64 iStoreId
    {
        get
        {
            if (ViewState["iStoreID"] == null)
            {
                ViewState["iStoreID"] = 0;
            }
            return Convert.ToInt64(ViewState["iStoreID"]);
        }
        set
        {
            ViewState["iStoreID"] = value;
        }
    }
    Int64 ProductId
    {
        get
        {
            if (ViewState["ProductId"] == null)
            {
                ViewState["ProductId"] = 0;
            }
            return Convert.ToInt64(ViewState["ProductId"]);
        }
        set
        {
            ViewState["ProductId"] = value;
        }
    }

    /// <summary>
    /// To Display WorkgroupName
    /// </summary>
    String WorkGroupNameToDisplay
    {
        get
        {
            if (Session["WorkGroupNameToDisplay"] != null && Session["WorkGroupNameToDisplay"].ToString().Length > 0)
                ViewState["WorkGroupNameToDisplay"] = " - " + Session["WorkGroupNameToDisplay"].ToString();
            else
                ViewState["WorkGroupNameToDisplay"] = "";

            return ViewState["WorkGroupNameToDisplay"].ToString();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int ManageID = (int)Incentex.DAL.Common.DAEnums.ManageID.CompanyProduct;
        Session["ManageID"] = ManageID;
        if (!IsPostBack)
        {
            base.MenuItem = "Store Management Console";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            if (Request.QueryString["Id"] != null)
                iStoreId = Convert.ToInt64(Request.QueryString["Id"].ToString());

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Notes" + WorkGroupNameToDisplay;
            ((HtmlGenericControl)menuControl.FindControl("dvSubMenu")).Visible = false;
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.ProductReturnURL;

            if (Request.QueryString["SubId"] != null && Request.QueryString["SubId"] != "0" && Request.QueryString["SubId"] != "")
            {
                this.ProductId = Convert.ToInt64(Request.QueryString.Get("SubId"));
                menuControl.PopulateMenu(6, 0, this.ProductId, iStoreId, false);
                DisplayNotes();
            }
            else
            {
                Response.Redirect("General.aspx?SubId=" + Request.QueryString["SubId"] + "&Id=" + Request.QueryString["Id"]);
            }
        }
    }
    
    private void DisplayNotes()
    {
        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        List<NoteDetail> objList = objRepo.GetByForeignKeyId(this.ProductId, Incentex.DAL.Common.DAEnums.NoteForType.StoreProduct);
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
    
    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        modalAddnotes.Show();
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
                ForeignKey = this.ProductId,
                Notecontents = txtNote.Text
                ,
                NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.StoreProduct)
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
}
