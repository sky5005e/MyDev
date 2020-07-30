using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Text;
using System.IO;

public partial class HeadsetRepairCenter_HeadsetQuoteStatus : System.Web.UI.Page
{
    HeadsetRepairCenterRepository objHeadsetRepair = new HeadsetRepairCenterRepository();
    NotesHistoryRepository objGenralNote = new NotesHistoryRepository();
    UserInformationRepository objUserInformationRepository = new UserInformationRepository();

    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            MasterPageFile = "~/MasterPage.master";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["UserId"]) && !string.IsNullOrEmpty(Request.QueryString["HeadsetRepairID"]) && !string.IsNullOrEmpty(Request.QueryString["Status"]) && !string.IsNullOrEmpty(Request.QueryString["rb"]))
            {
                HeadsetRepairCenterMaster objheadset = objHeadsetRepair.GetHeadsetRepairCenterById(Convert.ToInt64(Request.QueryString["HeadsetRepairID"]));
                long UserID = Convert.ToInt64(Request.QueryString["UserId"]);
                long IncentexUserID = Convert.ToInt64(Request.QueryString["rb"]);

                //HeadsetRepair Note History
                NoteDetail objNotedetails = new NoteDetail();
                objNotedetails.NoteFor = "HeadsetRepairCenter";
                objNotedetails.ForeignKey = objheadset.HeadsetRepairID;
                objNotedetails.SpecificNoteFor = "IEActivity";         //Note by CE or CA
                objNotedetails.CreateDate = DateTime.Now;
                objNotedetails.UpdateDate = DateTime.Now;
                objNotedetails.CreatedBy = UserID;   //CE or CA ID
                objNotedetails.UpdatedBy = UserID;  //CE or CA ID



                if (Request.QueryString["Status"] == "Approve")
                {
                    dvSuccessfullyApproved.Visible = true;
                    dvcancel.Visible = false;

                    objheadset.IsCustomerApprovedQuote = true;
                    objheadset.UpdatedBy = UserID;
                    objheadset.UpdatedDate = System.DateTime.Now;
                    objHeadsetRepair.SubmitChanges();

                    //Headset Repair Note History
                    objNotedetails.Notecontents = "Headset Repair Quote Approve";
                    objGenralNote.Insert(objNotedetails);
                    objGenralNote.SubmitChanges();
                }
                else if (Request.QueryString["Status"] == "Cancel")
                {
                    dvSuccessfullyApproved.Visible = false;
                    dvcancel.Visible = true;

                    objheadset.IsCustomerApprovedQuote = false;
                    objheadset.UpdatedBy = UserID;
                    objheadset.UpdatedDate = System.DateTime.Now;
                    objHeadsetRepair.SubmitChanges();

                    //Headset Repair Note History
                    objNotedetails.Notecontents = "Headset Repair Quote Cancel";
                    objGenralNote.Insert(objNotedetails);
                    objGenralNote.SubmitChanges();
                }

                List<IEListResultsCustom> objList = objHeadsetRepair.GetHeadsetRepairCenterManageEmail(objNotedetails.ForeignKey.Value);
                if (objList != null && objList.Count > 0)
                {
                    for (int i = 0; i < objList.Count; i++) //objList[i].Email
                    {
                        this.SendNotes(objNotedetails, objList[i].Email, objList[i].UserInfoID.ToString());
                    }
                }

                UserInformation objUserInformation = objUserInformationRepository.GetById(IncentexUserID);
                this.SendNotes(objNotedetails, objUserInformation.Email, objUserInformation.UserInfoID.ToString());
            }
        }
    }

    private void SendNotes(NoteDetail objNotedetails, string sentTo, string UserID)
    {
        try
        {
            UserInformation objUserInformation = objUserInformationRepository.GetById(objNotedetails.UpdatedBy);

            string sFrmadd = "support@world-link.us.com";
            String sToadd = sentTo;
            String sSubject = "Incentex Message - Headset Repair Number " + objNotedetails.ForeignKey;
            string sFrmname = objUserInformation.FirstName + " " + objUserInformation.LastName;


            String smtphost = Application["SMTPHOST"].ToString();
            Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            String smtpUserID = Application["SMTPUSERID"].ToString();
            String smtppassword = Application["SMTPPASSWORD"].ToString();

            String eMailTemplate = String.Empty;
            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/DocumentStorageCenter.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();


            string body = null;
            body = objNotedetails.Notecontents + "<br/><br/>";

            StringBuilder MessageBody = new StringBuilder(eMailTemplate);
            MessageBody.Replace("{MessageBody}", body);
            MessageBody.Replace("{Sender}", "Best Regards,<br/>" + sFrmname);

            //if (new ManageEmailRepository().CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderNotes)) == true)
            //{
            String sReplyToadd = CommonMails.ReplyToHeadserRepairCenter;//"ordernotes@world-link.us.com";
            String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+hn" + objNotedetails.ForeignKey + "un" + UserID + "snf" + "CEActivity" + "rb" + objNotedetails.CreatedBy + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
            new CommonMails().SendMailWithReplyTo(objUserInformation.UserInfoID, "HeadsetRepairCenter", sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true, objNotedetails.ForeignKey);
            //}
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}
