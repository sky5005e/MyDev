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

public partial class admin_IncentexEmployee_EmployeeNotes : PageBase
{
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

    Common objcomm = new Common();

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

                ((Label)Master.FindControl("lblPageHeading")).Text = "Employee Notes";
                //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to  Manage Incentex Employee</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/IncentexEmployee/ViewIncentexEmployee.aspx";
                
                menucontrol.PopulateMenu(5, 0, this.IncentexEmployeeID, 0,false);
            }
            else
            {
                Response.Redirect("ViewIncentexEmployee.aspx");
            }

            DisplayData(sender,e);
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
                ForeignKey = this.IncentexEmployeeID,
                Notecontents = txtNote.Text
                ,
                NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.IncentexEmployee)
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
        modalAddnotes.Show();
    }

    void DisplayData(object sender, EventArgs e)
    {

        if (this.IncentexEmployeeID != 0)
        {
            DisplayNotes();
        }
    }

    void DisplayNotes()
    {
        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        List<NoteDetail> objList = objRepo.GetByForeignKeyId(this.IncentexEmployeeID, Incentex.DAL.Common.DAEnums.NoteForType.IncentexEmployee);
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
            Response.Redirect("ShoppingSetting.aspx?id=" + this.IncentexEmployeeID);
        }
        catch (Exception ex)
        {

        }
    }
}
