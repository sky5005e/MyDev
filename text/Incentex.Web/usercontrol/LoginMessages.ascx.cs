/// <summary>
/// Module Name : Login Message System
/// Description : This user control is for display popup to ca/ce on each page for accept terms&condition if available
///               this popup is not display if condition is not available or already accept by user.
///               Insert terms & conditions on user note history when approve or decline.
/// Created : Mayur on 6-dec-2011
/// </summary>

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using AjaxControlToolkit;
using Incentex.DAL;
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;
using System.Web.UI.WebControls;

public partial class UserControl_LoginMessages : System.Web.UI.UserControl
{
    #region Data Members
    LookupRepository obj = new LookupRepository();
    CompanyStoreRepository objStoreRepo = new CompanyStoreRepository();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (IncentexGlobal.CurrentMember != null && (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.ThirdPartySupplierEmployee)))
            {
                if (obj.GetById(objStoreRepo.GetCompanyStoreStatusByCompanyId((long)IncentexGlobal.CurrentMember.CompanyId)).sLookupName == "Open" && obj.GetById((long)IncentexGlobal.CurrentMember.WLSStatusId).sLookupName == "Active")
                {
                    if (objStoreRepo.GetByCompanyId((long)IncentexGlobal.CurrentMember.CompanyId).IsAccessSystemActive == true && objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).AcceptTerms == false)
                    {
                        LoadTNC();
                        if (rptTNC.Items.Count > 0)
                        {
                            mpeAccepTerms.X = 300;
                            mpeAccepTerms.Y = 50;
                            mpeAccepTerms.Show();

                            //visible false video training system popup
                            ((Panel)Page.Master.FindControl("pnlLoadVideoUserControl")).Visible = false;
                        }
                    }
                }
            }
        }
    }
    
    protected override void OnInit(EventArgs e)
    {
        Page.Init += delegate(object sender, EventArgs e_Init)
        {
            if (ToolkitScriptManager.GetCurrent(Page) == null && ScriptManager.GetCurrent(Page) == null)
            {
                ToolkitScriptManager sMgr = new ToolkitScriptManager();
                phScriptManager.Controls.AddAt(0, sMgr);
            }
        };
        base.OnInit(e);
    }

    protected void lnkbtnAccept_Click(object sender, EventArgs e)
    {
        CompanyEmployee objCompanyEmployee = new CompanyEmployee();
        CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
        objCompanyEmployee = objEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        objCompanyEmployee.AcceptTerms = true;
        objEmpRepo.SubmitChanges();

        AddUserNote("Accept");
    }

    protected void lnkbtnDecline_Click(object sender, EventArgs e)
    {
        if (IncentexGlobal.CurrentMember != null)
        {
            AddUserNote("Cancel");
            //Update Value in User Activity
            UdpateUserLoginAndActivity();
            //End
        }
        Session.Remove("CurrentMember");
        Session.Remove("IsIEFromStore");
        IncentexGlobal.CurrentMember = null;
        IncentexGlobal.IsIEFromStore = false;
        IncentexGlobal.IsIEFromStoreTestMode = false;
        Session.RemoveAll();
        Response.Redirect("~/login.aspx");
    }
    #endregion

    #region Misc Functions

    /// <summary>
    /// Loads the Terms & Conditions based on workgroup ID and company ID
    /// </summary>
    private void LoadTNC()
    {
        try
        {
            //get workgroup by userinformationid
            CompanyStoreDocumentRepository objRep = new CompanyStoreDocumentRepository();
            CompanyEmployee objCmpnyInfo = new CompanyEmployee();

            objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

            //Get news by workgroup and companyid
            rptTNC.DataSource = objRep.GetAllTNCsByUser(objCmpnyInfo.WorkgroupID, (long)IncentexGlobal.CurrentMember.CompanyId);
            rptTNC.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    /// <summary>
    /// Udpates the user login and activity.
    /// </summary>
    public void UdpateUserLoginAndActivity()
    {
        UserActivityRepository objUAct = new UserActivityRepository();
        UserLoginActivity objUser = new UserLoginActivity();
        objUser = objUAct.GetByUserId(IncentexGlobal.CurrentMember.UserInfoID);
        if (objUser != null)
        {
            //User Has logged in once

            objUser.LogOutTime = System.DateTime.Now.TimeOfDay.ToString();
            objUser.LoginStatus = false;
            objUAct.SubmitChanges();
        }
    }

    /// <summary>
    /// Adds the terms & conditions to user note section.
    /// </summary>
    public void AddUserNote(string action)
    {
        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        CompanyStoreDocumentRepository objRep = new CompanyStoreDocumentRepository();
        CompanyEmployee objCmpnyInfo = new CompanyEmployee();

        objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        List<CompanyStoreDocumentRepository.TNCExtension> objTNCExtension= objRep.GetAllTNCsByUser(objCmpnyInfo.WorkgroupID, (long)IncentexGlobal.CurrentMember.CompanyId);
        
        StringBuilder strb=new StringBuilder();
        if (action=="Accept")
            strb.Append("Terms & Conditions Accepted ." + Environment.NewLine);
        else
            strb.Append("Terms & Conditions Declined." + Environment.NewLine);

        for (int i = 0; i < objTNCExtension.Count; i++)
		{
		    strb.Append( (i+1) + ". " + objTNCExtension[i].TNCHeader + Environment.NewLine);
            strb.Append( objTNCExtension[i].TNCContent);
            if (i != objTNCExtension.Count - 1)
                strb.Append(Environment.NewLine + Environment.NewLine);
        }

        NoteDetail obj = new NoteDetail()
        {
            ForeignKey = objCmpnyInfo.CompanyEmployeeID,
            Notecontents = strb.ToString()
            ,
            NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.CompanyEmployee)
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
    }
    #endregion
}
