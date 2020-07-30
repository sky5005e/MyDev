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
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using System.IO;
using System.Text;

public partial class admin_ServiceTicketCenter_AssignSubOwners : PageBase
{
    #region Page Properties

    ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
    //ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization

    Int64 ServiceTicketID
    {
        get
        {
            if (ViewState["ServiceTicketID"] == null)
            {
                ViewState["ServiceTicketID"] = 0;
            }
            return Convert.ToInt64(ViewState["ServiceTicketID"]);
        }
        set
        {
            ViewState["ServiceTicketID"] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Support Ticket Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Support Ticket Sub Owners";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = !String.IsNullOrEmpty(Convert.ToString(Request.UrlReferrer)) ? Convert.ToString(Request.UrlReferrer) : "~/Admin/index.aspx";

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])))
            {
                ServiceTicketID = Convert.ToInt64(Request.QueryString["id"]);
                vw_ServiceTicket objServiceTicket = new ServiceTicketRepository().GetFirstByID(ServiceTicketID);

                if (objServiceTicket != null)
                {
                    lblTicketNumber.Text = Convert.ToString(objServiceTicket.ServiceTicketID);
                    lblTicketName.Text = Convert.ToString(objServiceTicket.ServiceTicketName);
                    lblTicketStatus.Text = Convert.ToString(objServiceTicket.TicketStatus);
                    DisplaySubOwners();
                }
            }
        }
        lblMsg.Text = "";
    }

    #endregion

    #region Page Control Events

    protected void dtlSubOwners_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox chk = (CheckBox)e.Item.FindControl("chkSubOwners");
            HtmlGenericControl dvChk = (HtmlGenericControl)e.Item.FindControl("menuspan");
            HiddenField hdnSubOwnerID = (HiddenField)e.Item.FindControl("hdnSubOwnerID");
            Label lblSubOwners = (Label)e.Item.FindControl("lblSubOwners");
            chk.Attributes.Add("onclick", "javascript:HideTodo('" + chk.ClientID + "', '" + lblSubOwners.Text + "', '" + hdnSubOwnerID.Value + "');");
            if (chk.Checked)
            {
                dvChk.Attributes.Add("class", "custom-checkbox_checked");
            }
        }
    }

    protected void chkSubOwners_CheckedChanged(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        CheckBox chkSubOwner = (CheckBox)sender;

        //long NoteID = 0;
        //String Message = String.Empty;
        if (chkSubOwner.Checked)
        {
            ServiceTicketSubOwnerDetail objSubOwnerDetail = new ServiceTicketSubOwnerDetail();
            objSubOwnerDetail.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objSubOwnerDetail.CreatedDate = DateTime.Now;
            objSubOwnerDetail.ServiceTicketID = this.ServiceTicketID;
            objSubOwnerDetail.SubOwnerID = Convert.ToInt64(hdnTempID.Value);
            objSerTicRep.Insert(objSubOwnerDetail);

            ServiceTicketNoteRecipientsDetail objSTNRD = objSerTicRep.GetNoteRecipient(this.ServiceTicketID, Convert.ToInt64(hdnTempID.Value));
            objSTNRD.SubscriptionFlag = true;
            objSTNRD.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objSTNRD.UpdatedDate = DateTime.Now;

            objSerTicRep.SubmitChanges();
            LogAction("Assigned sub owner : " + hdnSubOwner.Value + "."); //NoteID = 
            //Message = "You are assigned as a \"Sub Owner\" to this ticket.";
        }
        else
        {
            ServiceTicketNoteRecipientsDetail objSTNRD = objSerTicRep.GetNoteRecipient(this.ServiceTicketID, Convert.ToInt64(hdnTempID.Value));
            objSTNRD.SubscriptionFlag = false;
            objSTNRD.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objSTNRD.UpdatedDate = DateTime.Now;
            objSerTicRep.SubmitChanges();

            objSerTicRep.DeleteSubOwnerTicketIDNUserID(this.ServiceTicketID, Convert.ToInt64(hdnTempID.Value), IncentexGlobal.CurrentMember.UserInfoID);
            LogAction("Removed sub owner : " + hdnSubOwner.Value + "."); //NoteID = 
            //Message = "You are no longer a \"Sub Owner\" to this ticket.";
        }

        //ServiceTicket objServiceTicket = objSerTicRep.GetById(this.ServiceTicketID);

        #region For Sending Emails
        //Message = "You have assigned " + hdnSubOwner.Value + " as a \"Sub Owner\" to this ticket.";

        //StreamReader _StreamReader;
        //_StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/ReceiptForTicket.htm"));
        //String eMailTemplate = String.Empty;
        //eMailTemplate = _StreamReader.ReadToEnd();
        //_StreamReader.Close();
        //_StreamReader.Dispose();

        //Common objcommon = new Common();

        #region Send Alert To Sub Owner

        //UserInformationRepository objUserRepo = new UserInformationRepository();
        //UserInformation objSubOwner = objUserRepo.GetById(Convert.ToInt64(hdnTempID.Value));

        //MessageBody = new StringBuilder(eMailTemplate);
        //MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
        //MessageBody.Replace("{FullName}", objSubOwner.FirstName + " " + objSubOwner.LastName);
        //MessageBody.Replace("{TicketNo}", Convert.ToString(objServiceTicket.ServiceTicketID));
        //MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
        //MessageBody.Replace("{Note}", Message);
        ////Email Management
        //if (objManageEmailRepo.CheckEmailAuthentication(objSubOwner.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
        //{
        //    objcommon.SendServiceTicketReciept(objServiceTicket.ServiceTicketID, NoteID, Convert.ToString(objSubOwner.UserInfoID), 2, objSubOwner.Email, MessageBody.ToString(), Convert.ToString(Application["SMTPHOST"]), Convert.ToString(Application["SMTPPORT"]), Convert.ToString(objServiceTicket.ServiceTicketName));
        //}
        #endregion
        #endregion

        lblMsg.Text = "Subowner information saved successfully.";
        DisplaySubOwners();
    }

    #endregion

    #region Page Methods

    private void DisplaySubOwners()
    {
        if (this.ServiceTicketID != 0)
        {
            vw_ServiceTicket objSerTic = objSerTicRep.GetFirstByID(this.ServiceTicketID);

            if (objSerTic != null)
            {
                lblOwnerName.Text = objSerTic.ServiceTicketOwnerID != null ? objSerTic.FirstName + " " + objSerTic.LastName : "N/A";

                h6CASupps.InnerText = objSerTic.SupplierID == null ? h6CASupps.InnerText : "Supplier Employees";

                //Get subowners
                List<GetServiceTicketSubOwnerDetailsResult> objSubOwnerList = new List<GetServiceTicketSubOwnerDetailsResult>() { };
                objSubOwnerList = objSerTicRep.GetSubOwnersByTicketID(Convert.ToInt32(this.ServiceTicketID)).Where(le => le.IsOwner == 0).OrderBy(le => le.FirstName).ToList();

                dtlIESubOwners.DataSource = objSubOwnerList.Where(le => le.Usertype == 1 || le.Usertype == 2).ToList();
                dtlIESubOwners.DataBind();

                dtlCASubOwners.DataSource = objSubOwnerList.Where(le => objSerTic.SupplierID == null ? le.Usertype == 3 : (le.Usertype == 5 || le.Usertype == 6)).ToList();
                dtlCASubOwners.DataBind();
            }
        }
    }

    private Int64 LogAction(String Content)
    {
        String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);

        NoteDetail objComNot = new NoteDetail();
        NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();

        objComNot.Notecontents = Content;
        objComNot.NoteFor = strNoteFor;
        objComNot.SpecificNoteFor = "IEActivity";
        objComNot.ForeignKey = this.ServiceTicketID;
        objComNot.CreateDate = System.DateTime.Now;
        objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objComNot.UpdateDate = System.DateTime.Now;
        objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

        objCompNoteHistRepos.Insert(objComNot);
        objCompNoteHistRepos.SubmitChanges();
        return objComNot.NoteID;
    }

    #endregion
}
