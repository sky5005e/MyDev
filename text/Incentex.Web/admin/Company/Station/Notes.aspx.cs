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
using System.Xml.Linq;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using System.Collections.Generic;


public partial class admin_Company_Station_Notes : PageBase
{

    Int64 CompanyId
    {
        get
        {
            if (ViewState["CompanyId"] == null)
            {
                ViewState["CompanyId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyId"]);
        }
        set
        {
            ViewState["CompanyId"] = value;
        }
    }

    Int64 StationId
    {
        get
        {
            if (ViewState["StationId"] == null)
            {
                ViewState["StationId"] = 0;
            }
            return Convert.ToInt64(ViewState["StationId"]);
        }
        set
        {
            ViewState["StationId"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Additional Information";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to company listing</span>";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Company/ViewCompany.aspx";

            if (Request.QueryString.Count > 0)
            {
                try
                {
                    this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                    this.StationId = Convert.ToInt64(Request.QueryString.Get("SubId"));



                }
                catch (Exception ex)
                {
                    Response.Redirect("~/admin/Company/Station/ViewStation.aspx?Id=" + this.CompanyId);
                }

                if (this.StationId == 0)
                {
                    Response.Redirect("~/admin/Company/Station/MainStationInfo.aspx?Id=" + this.CompanyId, true);
                }

                manuControl.PopulateMenu(3, 5, this.CompanyId, this.StationId, true);

                DisplayNotes();
            }
            else
            {
                Response.Redirect("~/admin/Company/Station/ViewStation.aspx");
            }

        }
    }

    void DisplayNotes()
    {
        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        List<NoteDetail> objList = objRepo.GetByForeignKeyId(this.StationId, DAEnums.NoteForType.Station);

        txtNoteHistory.Text = "";
        foreach (NoteDetail obj in objList)
        {
            txtNoteHistory.Text += "Created Date : " + Convert.ToDateTime(obj.CreateDate).ToString("dd-MMM-yyyy") + "\n";
            txtNoteHistory.Text += "Created Time : " + Convert.ToDateTime(obj.CreateDate).ToString("hh:mm tt") + "\n";

            UserInformationRepository objUserRepo = new UserInformationRepository();
            UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);

            if(objUser != null)
            {
                txtNoteHistory.Text += "Created By : " + objUser.FirstName + " " + objUser.LastName + "\n";
            }


            txtNoteHistory.Text += "Note : " + obj.Notecontents + "\n";
            txtNoteHistory.Text += "---------------------------------------------------------------------------\n";

        }

    }



    protected void lnkAddNote_Click(object sender, EventArgs e)
    {
        modalAddnotes.Show();

    }

    protected void lnkSaveNote_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";

        if (string.IsNullOrEmpty(txtNote.Text))
        {
            return;
        }

        try
        {


            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            NoteDetail obj = new NoteDetail()
            {
                ForeignKey = this.StationId,
                Notecontents = txtNote.Text
                ,
                NoteFor = DAEnums.GetNoteForTypeName(DAEnums.NoteForType.Station)
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
            txtNote.Text = "";
            DisplayNotes();

        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in saving record ...";
            ErrHandler.WriteError(ex);
        }

    }


}//class
