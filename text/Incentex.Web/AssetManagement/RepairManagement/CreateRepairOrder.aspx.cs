using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using commonlib.Common;
using System.Data;
using System.IO;
using System.Text;
using System.Configuration;
using System.Net;
using System.Reflection;
public partial class AssetManagement_RepairManagement_CreateRepairOrder : PageBase
{
    #region DataMembers
    AssetVendorRepository objVendorRepo = new AssetVendorRepository();
    AssetRepairMgtRepository objAssetRepairRepository = new AssetRepairMgtRepository();
    EquipmentRepairOrder objEqp = new EquipmentRepairOrder();
    #endregion
    #region Events
    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                MasterPageFile = "~/admin/MasterPageAdmin.master";
            }
            else
            {
                MasterPageFile = "~/MasterPage.master";
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                base.MenuItem = "Repair Order Management";
                base.ParentMenuID = 46;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView || !base.CanAdd)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Create Repair Order";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/RepairManagement/RepairManagementIndex.aspx";
                BindValues();
                BindVendor();
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    ddlCompany.SelectedValue = Convert.ToString(IncentexGlobal.CurrentMember.CompanyId);
                    ddlCompany.Enabled = false;
                }
                else
                {
                    ddlCompany.Items.Insert(0, new ListItem("-Select-", "0"));
                    ddlCompany.Enabled = true;
                }
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Bind Equipment Type
        List<AssetRepairMgtRepository.GetEquipmentTypeResult> objList = new List<AssetRepairMgtRepository.GetEquipmentTypeResult>();
        objList = objAssetRepairRepository.GetEquipmentTypebyCompany(Convert.ToInt64(ddlCompany.SelectedValue));
        Common.BindDDL(ddlEquipmentType, objList, "EquipmentTypeName", "EquipmentTypeID", "-Select-");

        //Bind Equipment ID
        List<AssetRepairMgtRepository.GetEquipmentIDResult> objListID = new List<AssetRepairMgtRepository.GetEquipmentIDResult>();
        objListID = objAssetRepairRepository.GetEquipmentIDbyEquipmentType(Convert.ToInt64(ddlEquipmentType.SelectedValue), Convert.ToInt64(ddlCompany.SelectedValue));
        Common.BindDDL(ddlEquipmentId, objListID, "EquipmentID", "EquipmentMasterID", "-Select-");
    }
    protected void ddlEquipmentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Bind Equipment ID
        List<AssetRepairMgtRepository.GetEquipmentIDResult> objList = new List<AssetRepairMgtRepository.GetEquipmentIDResult>();
        objList = objAssetRepairRepository.GetEquipmentIDbyEquipmentType(Convert.ToInt64(ddlEquipmentType.SelectedValue), Convert.ToInt64(ddlCompany.SelectedValue));
        Common.BindDDL(ddlEquipmentId, objList, "EquipmentID", "EquipmentMasterID", "-Select-");
    }
    protected void ddlEquipmentId_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlEquipmentStatus.SelectedValue = objAssetRepairRepository.GetEquipmentStatusbyEquipmentID(Convert.ToInt64(ddlEquipmentId.SelectedValue));
        }
        catch (Exception)
        {}
       
    }
    protected void ddlNotifyVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlNotifyVendor.SelectedValue == "True")
        {
            trVendorStation.Visible = true;
            //get Base Stations
            dtVBaseStation.DataSource = objAssetRepairRepository.GetBaseStationFromVendorEmployeeBaseSattion();
            dtVBaseStation.DataBind();
        }
        else
        {
            trVendorStation.Visible = false;
            dtVBaseStation.DataSource = null;
            dtVBaseStation.DataBind();
        }
    }
    protected void ddlNotifyOurInternalCompanyPersonal_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlNotifyOurInternalCompanyPersonal.SelectedValue == "True")
        {
            trCustomerStation.Visible = true;
            //get Base Stations
            dtCBaseStation.DataSource = objAssetRepairRepository.GetBaseStationFromCustomerEmployeeBaseSattion();
            dtCBaseStation.DataBind();
        }
        else
        {
            trCustomerStation.Visible = false;
            dtCBaseStation.DataSource = null;
            dtCBaseStation.DataBind();
        }
    }
    protected void lnkBtnSubmitRepairOrder_Click(object sender, EventArgs e)
    { 
        try
        {
            objEqp.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            objEqp.VendorID = Convert.ToInt64(ddlVendor.SelectedValue);
            objEqp.EquipmentMasterID = Convert.ToInt64(ddlEquipmentId.SelectedValue);
            objEqp.RepairReason = Convert.ToInt64(ddlReasonforRepair.SelectedValue);
            objEqp.RequestedServiceDate = Convert.ToDateTime(txtReturntoServiceDate.Text);
            objEqp.RepairStatusID = objAssetRepairRepository.GetRepairOrderStatus("Submitted to Vendor"); //Get Sumitted to Vendor Status
            // Notify 
            if (ddlNotifyVendor.SelectedValue == "True")
                objEqp.IsVendorNotify = true;
            else
                objEqp.IsVendorNotify = false;

            // Notify Company
            if (ddlNotifyOurInternalCompanyPersonal.SelectedValue == "True")
                objEqp.IsCmpPersonalNotify = true;
            else
                objEqp.IsCmpPersonalNotify = false;

            objEqp.ProblemDescription = txtDescriptionofProblem.Text;

            if (objEqp.RepairOrderID == 0)
            {
                // for Created Date and By User name
                objEqp.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objEqp.CreatedDate = DateTime.Now;
                Int64? AutoRepairNo = objAssetRepairRepository.GetMaxRepairNo();
                if (AutoRepairNo != null)
                    objEqp.AutoRepairNumber = Convert.ToInt64(AutoRepairNo + 1);
                else
                    objEqp.AutoRepairNumber = 2221;                
                objAssetRepairRepository.Insert(objEqp);
                lblMsg.Text = "Records Inserted Successfully...";
            }
            else
            {
                //For Update Date
                objEqp.UpdatedDate = DateTime.Now;
                objEqp.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                lblMsg.Text = "Records Updated Successfully...";
            }
            objAssetRepairRepository.SubmitChanges();
            //Update Equipment Status
            if (ddlEquipmentStatus.SelectedValue != "0")
            objAssetRepairRepository.UpdateEquipmentStatus(Convert.ToInt64(ddlEquipmentId.SelectedValue),Convert.ToInt64(ddlEquipmentStatus.SelectedValue));

            #region Send Mail's
            if (ddlNotifyVendor.SelectedValue == "True")
                SendMailToVendorUserListbyBaseStation();
            if (ddlNotifyOurInternalCompanyPersonal.SelectedValue == "True")
                SendMailToCompanyUserListbyBaseStation();
            #endregion 
            Response.Redirect("ViewOpenRepairOrders.aspx");
        }
        catch (System.Threading.ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion
    #region Methods
    private void SendMailToVendorUserListbyBaseStation()
    {
        List<String> chkedIDList = new List<String>();
        foreach (DataListItem dt in dtVBaseStation.Items)
        {
            if (((CheckBox)dt.FindControl("chkVBaseStation")).Checked == true)
            {
                HiddenField hdnBaseStation = (HiddenField)dt.FindControl("hdnVBaseStation");
                chkedIDList.Add(hdnBaseStation.Value);
            }
        }

        List<AssetRepairMgtRepository.GetVenderUserListInfo> distinctUser = objAssetRepairRepository.GetVendorUserListbyBaseStationID(chkedIDList).ToList();
        IEqualityComparer<AssetRepairMgtRepository.GetVenderUserListInfo> customComparer = new PropertyComparer<AssetRepairMgtRepository.GetVenderUserListInfo>("UserInfoID");
        IEnumerable<AssetRepairMgtRepository.GetVenderUserListInfo> list = distinctUser.Distinct(customComparer);
        foreach (var user in list)
        {
            if (objVendorRepo.CheckEmailAuthentication(Convert.ToInt64(user.UserInfoID), 8) == true)// 8 For Repair Order Mails
            {
           
            String subjectForVendor = ddlCompany.SelectedItem.Text + " - " + ddlEquipmentId.SelectedItem.Text;
            String MessageBody = "This email is to notify you that " + ddlCompany.SelectedItem.Text + " has requested for <br/>"
                                  + "you to look at there asset " + ddlEquipmentId.SelectedItem.Text + " which is a " + ddlEquipmentType.SelectedItem.Text + ". They are <br/>" +
                                 "requesting you to look at the following: <br/><br/>" +
                                 "Description of Service:<br/><br/>" + txtDescriptionofProblem.Text;
           
            sendVerificationEmail(user.LoginEmail, user.FullName, subjectForVendor, MessageBody, Convert.ToInt64(user.UserInfoID));
            }
        }
    }
    private void SendMailToCompanyUserListbyBaseStation()
    {
        List<String> chkedIDList = new List<String>();
        foreach (DataListItem dt in dtCBaseStation.Items)
        {
            if (((CheckBox)dt.FindControl("chkCBaseStation")).Checked == true)
            {
                HiddenField hdnBaseStation = (HiddenField)dt.FindControl("hdnCBaseStation");
                chkedIDList.Add(hdnBaseStation.Value);
            }
        }

        List<AssetRepairMgtRepository.GetVenderUserListInfo> distinctUser= objAssetRepairRepository.GetCustomerUserListbyBaseStationID(chkedIDList).ToList();

        // To get Distinct value from Generic List using LINQ
        // Create an Equality Comprarer Intance
        IEqualityComparer<AssetRepairMgtRepository.GetVenderUserListInfo> customComparer = new PropertyComparer<AssetRepairMgtRepository.GetVenderUserListInfo>("UserInfoID");
        IEnumerable<AssetRepairMgtRepository.GetVenderUserListInfo> list = distinctUser.Distinct(customComparer);
        foreach (var user in list)
        {
            if (objVendorRepo.CheckEmailAuthentication(Convert.ToInt64(user.UserInfoID), 8) == true)// 8 For Repair Order Mails
            {
                String subjectForCustomer = ddlEquipmentType.SelectedItem.Text + " - " + ddlEquipmentId.SelectedItem.Text;
                String MsgBody = "This email is to inform you about the following status for this asset <br/>" +
                                     "and let you know that it have been (Taken Out Of Service)." +
                                     "Description of Problem:<br/><br/>" + txtDescriptionofProblem.Text + "<br/><br/>" +
                                     "We have notified our local services vendor and they are addressing the request.";
               
                sendVerificationEmail(user.LoginEmail, user.FullName, subjectForCustomer, MsgBody, Convert.ToInt64(user.UserInfoID));
            }
        }

    }
    private void sendVerificationEmail(String UserEmail, String UserName, String subjectText, String Body,Int64 userInfoID)
    {
        try
        {
            string sFrmadd = IncentexGlobal.CurrentMember.LoginEmail;
            string sToadd = UserEmail.Trim();
            string sSubject = subjectText;
            string sFrmname = IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName;
            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();
            String eMailTemplate = String.Empty;
            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/SimpleMailFormat.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();
            StringBuilder MessageBody = new StringBuilder(eMailTemplate);
            MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
            MessageBody.Replace("{MessageBody}", Body.ToString());
            //Live server Message
            new CommonMails().SendMail(userInfoID, null, sFrmadd, sToadd, sSubject, MessageBody.ToString(), Common.DisplyName, Common.SMTPHost, Common.SMTPPort, false, true);
            //Text Message
            String TxtBody = "Out for Service - (" + ddlEquipmentType.SelectedItem.Text + ") - (" + ddlEquipmentId.SelectedItem.Text + ")";
            new AssetMgtRepository().TextMsg(userInfoID, TxtBody);            
        }
        catch (Exception ex)
        {
        }

    }
     private void BindValues()
    {
        //get company
        CompanyRepository objCompanyRepo = new CompanyRepository();
        List<Company> objCompanyList = new List<Company>();
        objCompanyList = objCompanyRepo.GetAllCompany();
        Common.BindDDL(ddlCompany, objCompanyList, "CompanyName", "CompanyID", "-Select-");
        //get Equipment Type
        List<AssetRepairMgtRepository.GetEquipmentTypeResult> objType = new List<AssetRepairMgtRepository.GetEquipmentTypeResult>();
        objType = objAssetRepairRepository.GetEquipmentTypebyCompany(Convert.ToInt64(ddlCompany.SelectedValue));
        Common.BindDDL(ddlEquipmentType, objType, "EquipmentTypeName", "EquipmentTypeID", "-Select-");
      
       // List<EquipmentLookup> objStatus = objAssetRepairRepository.GetRepairStatusInfo(0, "RepairStatus");
       // Common.BindDDL(ddlRepairStatus, objStatus, "sLookupName", "iLookupID", "-Select-");

        //get EquipmentID
        List<AssetRepairMgtRepository.GetEquipmentIDResult> objID = new List<AssetRepairMgtRepository.GetEquipmentIDResult>();
        objID = objAssetRepairRepository.GetEquipmentIDbyEquipmentType(Convert.ToInt64(ddlEquipmentType.SelectedValue), Convert.ToInt64(ddlCompany.SelectedValue));
        Common.BindDDL(ddlEquipmentId, objID, "EquipmentID", "EquipmentMasterID", "-Select-");
        //get Reason for Repairs
        List<EquipmentLookup> objReasonList = objAssetRepairRepository.GetRepairReasonInfo(0, "RepairReason");
        Common.BindDDL(ddlReasonforRepair, objReasonList, "sLookupName", "iLookupID", "-Select-");
       

         //Get Equipment Status
        LookupRepository objLookupRepository = new LookupRepository();
        List<INC_Lookup> objStatus = new List<INC_Lookup>();
        objStatus = objLookupRepository.GetByLookup("EquipmentStatus");
        objStatus = objStatus.OrderBy(p => p.sLookupName).ToList();
        Common.BindDDL(ddlEquipmentStatus, objStatus, "sLookupName", "iLookupID", "-Select Status-");

    }
    private void BindVendor()
    {
        AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
        List<EquipmentVendorMaster> objList = new List<EquipmentVendorMaster>();
        objList = objAssetVendorRepository.GetAllEquipmentVendor().OrderBy(v => v.EquipmentVendorName).ToList();
        Common.BindDDL(ddlVendor, objList, "EquipmentVendorName", "EquipmentVendorID", "-Select-");
    }
    
    //private void ResetControls()
    //{
    //    ddlCompany.SelectedIndex = 0;
    //    ddlEquipmentType.SelectedIndex = 0;
    //    ddlEquipmentStatus.SelectedIndex = 0;
    //    ddlEquipmentId.SelectedIndex = 0;
    //    ddlVendor.SelectedIndex = 0;
    //    ddlReasonforRepair.SelectedIndex = 0;
    //    txtReturntoServiceDate.Text = "";
    //    ddlNotifyVendor.SelectedIndex = 0;
    //    ddlNotifyOurInternalCompanyPersonal.SelectedIndex = 0;
    //    txtDescriptionofProblem.Text = "";
    //}
    #endregion
      
   
    #region For Property Comparer Class
    public class PropertyComparer<T> : IEqualityComparer<T>
    {
        private PropertyInfo _PropertyInfo;

        /// <summary>
        /// Creates a new instance of PropertyComparer.
        /// </summary>
        /// <param name="propertyName">The name of the property on type T 
        /// to perform the comparison on.</param>
        public PropertyComparer(string propertyName)
        {
            //store a reference to the property info object for use during the comparison
            _PropertyInfo = typeof(T).GetProperty(propertyName,
        BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            if (_PropertyInfo == null)
            {
                throw new ArgumentException(string.Format("{0} is not a property of type {1}.", propertyName, typeof(T)));
            }
        }

        #region IEqualityComparer<T> Members

        public bool Equals(T x, T y)
        {
            //get the current value of the comparison property of x and of y
            object xValue = _PropertyInfo.GetValue(x, null);
            object yValue = _PropertyInfo.GetValue(y, null);

            //if the xValue is null then we consider them equal if and only if yValue is null
            if (xValue == null)
                return yValue == null;

            //use the default comparer for whatever type the comparison property is.
            return xValue.Equals(yValue);
        }

        public int GetHashCode(T obj)
        {
            //get the value of the comparison property out of obj
            object propertyValue = _PropertyInfo.GetValue(obj, null);

            if (propertyValue == null)
                return 0;

            else
                return propertyValue.GetHashCode();
        }

        #endregion
    }
    #endregion
}
    
