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


public partial class AssetFrameItems_ViewFlaggedNotes : System.Web.UI.Page
{

    Int64 EquipmentMasterID
    {
        get
        {
            return Convert.ToInt64(ViewState["EquipmentMasterID"]);
        }
        set
        {
            ViewState["EquipmentMasterID"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["eid"]))
            {
                EquipmentMasterID = Convert.ToInt64(Request.QueryString["eid"]);
                BindBasicTabNotes();
            }
        }
    }

    private void BindBasicTabNotes()
    {
        try
        {
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();
            string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.FlagAssets);
            var objNoteDetailResult = objCompNoteHistRepos.GetFlaggedNotes(Convert.ToInt64(this.EquipmentMasterID), strNoteFor);
            if (objNoteDetailResult != null && objNoteDetailResult.Count > 0)
                nobasicnotes_li.Visible = false;
            else
                nobasicnotes_li.Visible = true;
            rpBasicViewNote.DataSource = objNoteDetailResult;
            rpBasicViewNote.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnAssetsNote_Click(object sender, EventArgs e)
    {
        try
        {
            String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.FlagAssets);
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();
            NoteDetail objComNot = new NoteDetail();
            objComNot.Notecontents = txtFlaggedNoteDetails.Value.Trim();
            objComNot.NoteFor = strNoteFor;
            objComNot.SpecificNoteFor = "Flagged";
            objComNot.ForeignKey = Convert.ToInt64(this.EquipmentMasterID);
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objCompNoteHistRepos.Insert(objComNot);
            objCompNoteHistRepos.SubmitChanges();
            txtFlaggedNoteDetails.Value = "";
            BindBasicTabNotes();
            this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "window.parent.TriggerClickEvent('close-btn',true); window.parent.GeneralAlertMsg('Note added successfully.');  window.parent.BindNotificationItems();  window.parent.ShowLoader(false);", true);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnAssetsUnflagged_Click(object sender, EventArgs e)
    {
        try
        {

            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            EquipmentMaster objEquipmentMaster = new EquipmentMaster();
            if (this.EquipmentMasterID > 0)
            {
                //" + System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/AddNewAsset.aspx?eqpID=" + "
                objAssetMgtRepository.UnflaggedAssets(this.EquipmentMasterID, IncentexGlobal.CurrentMember.UserInfoID, Incentex.DAL.Common.DAEnums.NoteForType.FlagAssets);
                this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "window.parent.TriggerClickEvent('close-btn',true); window.parent.GeneralAlertMsg('Note has been unflagged successfully.','');  window.parent.BindNotificationItems();  window.parent.ShowLoader(false);", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnViewAsset_Click(object sender, EventArgs e)
    {
        this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "RedirectURL('" + System.Configuration.ConfigurationManager.AppSettings["siteurl"] + "NewDesign/Admin/AddNewAsset.aspx?eqpID=" + EquipmentMasterID + "');", true);
    }

}
