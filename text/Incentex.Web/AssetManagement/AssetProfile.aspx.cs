using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class AssetManagement_AssetProfile : PageBase
{
    #region DataMembers
    CompanyEmployee objCmpnyInfo = new CompanyEmployee();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    UserInformation objuser = new UserInformation();
    UserInformationRepository objuserrepos = new UserInformationRepository();
    CompanyStoreRepository objCompStoreRepos = new CompanyStoreRepository();
    CompanyStore objcomstore = new CompanyStore();
    EquipmentMaster objEquipmentMaster = new EquipmentMaster();
    EquipmentMaintenanceCostDetail objEquipMaintenanceCostDtl = new EquipmentMaintenanceCostDetail();
    EquipmentWeeklyMaintenance objEquipmentWeeklyMaintenance = new EquipmentWeeklyMaintenance();
    EquipmentImage objEquipmentImage = new EquipmentImage();
    List<EquipmentImage> objEquipmentImageList = new List<EquipmentImage>();
    AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
    AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
    AssetFieldRepository objFieldRepository = new AssetFieldRepository();
    EquipmentFieldMaster objFieldMaster = new EquipmentFieldMaster();
    EquipmentFieldDetail objFieldDetail = new EquipmentFieldDetail();
    AssetRepairMgtRepository objAssetRepairRepository = new AssetRepairMgtRepository();
    Int64 CompanyID = 0;
    Decimal totalAmount = 0M;
    //Int64 WorkgroupID = 0;
    Common objcommon = new Common();
    PagedDataSource pds = new PagedDataSource();
    DataSet ds = new DataSet();
    Int64 EquipmentMasterId
    {
        get
        {
            if (ViewState["EquipmentMasterId"] == null)
            {
                ViewState["EquipmentMasterId"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentMasterId"]);
        }
        set
        {
            ViewState["EquipmentMasterId"] = value;
        }
    }
    private String EquipmentID
    {
        get
        {
            if (Convert.ToString(this.ViewState["EquipmentID"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["EquipmentID"]);
        }
        set
        {
            this.ViewState["EquipmentID"] = value;
        }
    }
    Int64 EquipmentMaintenanceCostID
    {
        get
        {
            if (ViewState["EquipmentMaintenanceCostID"] == null)
            {
                ViewState["EquipmentMaintenanceCostID"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentMaintenanceCostID"]);
        }
        set
        {
            ViewState["EquipmentMaintenanceCostID"] = value;
        }
    }
    Int64 EquipmentWeeklyMaintinanceID
    {
        get
        {
            if (ViewState["EquipmentWeeklyMaintinanceID"] == null)
            {
                ViewState["EquipmentWeeklyMaintinanceID"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentWeeklyMaintinanceID"]);
        }
        set
        {
            ViewState["EquipmentWeeklyMaintinanceID"] = value;
        }
    }
    Int64 EquipmentTypeID
    {
        get
        {
            if (ViewState["EquipmentTypeID"] == null)
            {
                ViewState["EquipmentTypeID"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentTypeID"]);
        }
        set
        {
            ViewState["EquipmentTypeID"] = value;
        }
    }
    Int64 FieldCompany
    {
        get
        {
            if (ViewState["FieldCompany"] == null)
            {
                ViewState["FieldCompany"] = 0;
            }
            return Convert.ToInt64(ViewState["FieldCompany"]);
        }
        set
        {
            ViewState["FieldCompany"] = value;
        }
    }    
    string FromPage
    {
        get
        {
            if (ViewState["FromPage"] == null)
            {
                ViewState["FromPage"] = 0;
            }
            return Convert.ToString(ViewState["FromPage"]);
        }
        set
        {
            ViewState["FromPage"] = value;
        }
    }
    string EqTypeID=null;
    string EqID = null;
    string BSID = null;
    string EqStatus=null;
    string GSEDepID=null;
    string CID = null;
    string RPID = null;
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

                base.MenuItem = "Add GSE Equipment";
                base.ParentMenuID = 46;

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
                    this.EquipmentMasterId = Convert.ToInt64(Request.QueryString.Get("ID"));
                    this.FromPage = Convert.ToString(Request.QueryString.Get("Page"));

                    EqTypeID = Convert.ToString(Session["EqTypeID"]);
                    EqID =Convert.ToString(Session["EqID"]);
                    BSID =Convert.ToString(Session["BSID"]);
                    EqStatus =Convert.ToString(Session["EqStatus"]);
                    GSEDepID =Convert.ToString(Session["GSEDepID"]);
                    CID =Convert.ToString(Session["CID"]);
                    RPID = Convert.ToString(Request.QueryString.Get("RPID"));
                }
                
                ((Label)Master.FindControl("lblPageHeading")).Text = "Asset Profile";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    
                    //objcomstore = objCompStoreRepos.GetByCompanyId(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
                    CompanyID = Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId);
                    
                }
                else
                {
                    CompanyID = 0;
                    //WorkgroupID = 0;
                }
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    lnkPostInvoice.Visible = true;
                }
                else
                {
                    lnkPostInvoice.Visible = false;
                }
               
                bindPostInvoicegrid();
                bindPostEventgrid();
                getImages();
                BindPageDropdowns();
                DisplayData();
                BindPopupValues();
                DisplayPopupData();
                getFields();
                getFieldDesc();
                bindPartsHistoryGridView();
                bindRepairOrderGridView();
                ResetControls();
                lblMsg.Text = "";

                ShowUploadedFile();

                if (FromPage == "FlaggedAssets")
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/FlaggedAssets.aspx";
                }
                else if (FromPage == "RepairProfile")
                {
                    GetRepairOrderSelection();
                }
                else
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/ManageEquipment.aspx?EquipmentTypeID=" + EqTypeID + "&EquipmentID=" + EqID + "&BaseStationID=" + BSID + "&EquipmentStatus=" + EqStatus + "&CompanyID=" + CID + "&GSEDepartmentID=" + GSEDepID;
                }
            }
            
            //---Hide Add Note Button According to Vendor Employee Email Rights
            lnkAddHisotry.Visible = true;
            if (Convert.ToString(Session["Usr"]) == Convert.ToString(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                if (objAssetVendorRepository.CheckEmailAuthentication(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID),Convert.ToInt64(ddlShowMe.SelectedValue)) != true)
                {
                    lnkAddHisotry.Visible = false;
                }
                else
                {
                    lnkAddHisotry.Visible = true;
                }
            }

            getImages();
            DisplayNotes();            
        }
        catch (Exception)
        {


        }
    }
    
    
    protected void lnkFlagAsset_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
        EquipmentMaster objEquipmentMaster = new EquipmentMaster();
        try
        {


            objEquipmentMaster = objAssetMgtRepository.GetById(EquipmentMasterId);
            if (objEquipmentMaster.Flagged == false)
            {
                lnkFlagAsset.ForeColor = System.Drawing.Color.Red;
                objEquipmentMaster.Flagged = true;
            }
            else
            {
                lnkFlagAsset.ForeColor = System.Drawing.Color.Empty;
                objEquipmentMaster.Flagged = false;
            }

            objAssetMgtRepository.SubmitChanges();
        }
        catch (Exception)
        {


        }
    }   
    //--Main Section
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            EquipmentMaster objEquipmentMaster = new EquipmentMaster();

            objEquipmentMaster = objAssetMgtRepository.GetById(this.EquipmentMasterId);
            objEquipmentMaster.EquipmentID = txtEquipmentId.Text.Trim();
            objEquipmentMaster.EquipmentTypeID = Convert.ToInt64(ddlEquipmentType.SelectedValue);
            objEquipmentMaster.BaseStationID= Convert.ToInt64(ddlBaseStation.SelectedValue);
            objEquipmentMaster.BrandID = Convert.ToInt64(ddlBrand.SelectedValue);
            objEquipmentMaster.SerialNumber = txtSerialNo.Text.Trim();
            objEquipmentMaster.ModelYear = txtModelYear.Text != "" ? Convert.ToInt32(txtModelYear.Text.Trim()) : 0;
            objEquipmentMaster.FuelTypeID = Convert.ToInt64(ddlFuelType.SelectedValue);
            objEquipmentMaster.Hours = txtHours.Text;
            if (txtLastInspection.Text != "")
            {
                objEquipmentMaster.LastInspectionDate = Convert.ToDateTime(txtLastInspection.Text);
            }
            objEquipmentMaster.Status = Convert.ToInt64(ddlStatus.SelectedValue);
            objEquipmentMaster.UpdatedDate = DateTime.Now;
            objEquipmentMaster.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objAssetMgtRepository.SubmitChanges();
          
            DisplayData();
            DisplayPopupData();
            ColorChange();
            lblMsg.Text = "";
        }
        catch (Exception)
        {


        }
    }
    //--Insurance---
    protected void lnkBtnSaveInsuranceCompany_Click(object sender, EventArgs e)
    {
        try
        {

            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            EquipmentMaster objEquipmentMaster = new EquipmentMaster();

            objEquipmentMaster = objAssetMgtRepository.GetById(this.EquipmentMasterId);
            objEquipmentMaster.InsuranceCompany = txtInsuranceCompany.Text.Trim();
            objEquipmentMaster.PolicyNumber = txtPolicynumber.Text.Trim();
            objEquipmentMaster.VinNumber = txtAssetVinNumber.Text.Trim();
            objEquipmentMaster.PlateNumber = txtAssetPlateNumber.Text.Trim();           
                       
            objEquipmentMaster.UpdatedDate = DateTime.Now;
            objEquipmentMaster.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objAssetMgtRepository.SubmitChanges();
            EditNotes("Insurance Records");

            DisplayData();
            DisplayPopupData();
            lblMsg.Text = "";
        }
        catch (Exception)
        {


        }
    }
    protected void lnkBtnSaveInternalContacts_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            EquipmentMaster objEquipmentMaster = new EquipmentMaster();

            objEquipmentMaster = objAssetMgtRepository.GetById(this.EquipmentMasterId);
            objEquipmentMaster.InternalCompContactName = txtPICCName.Text.Trim();
            objEquipmentMaster.InternalCompContactTelephone = txtPICCTelephone.Text.Trim();
            objEquipmentMaster.InternalCompContactEmail = txtPICCEmail.Text.Trim();
            objEquipmentMaster.InsuranceAgentCompany = txtInsAgentCompany.Text.Trim();
            objEquipmentMaster.InsuranceAgentName = txtInsAgentName.Text.Trim();
            objEquipmentMaster.InsuranceAgentTelephone = txtInsAgentTelephone.Text.Trim();
            objEquipmentMaster.InsuranceAgentEmail = txtInsAgentEmail.Text.Trim();
            objEquipmentMaster.UpdatedDate = DateTime.Now;
            objEquipmentMaster.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objAssetMgtRepository.SubmitChanges();
            EditNotes("Insurance Records");

            DisplayData();
            DisplayPopupData();
            lblMsg.Text = "";
        }
        catch (Exception)
        {


        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        string sFilePath = null;
        //AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
        //EquipmentMaster objEquipmentMaster = new EquipmentMaster();

        try
        {


            objEquipmentMaster = objAssetMgtRepository.GetById(this.EquipmentMasterId);

            if (((float)Request.Files[0].ContentLength / 1048576) > 2)
            {
                lblMessage.Text = "The file you are uploading is more than 2MB.";
                modal.Show();
                return;
            }

            if (flFile.Value != "")
            {
                objEquipmentMaster.InsurancePolicy = flFile.Value;
                sFilePath = Server.MapPath("../UploadedImages/InsurancePolicy/") + flFile.Value;
                objcommon.DeleteImageFromFolder(flFile.Value, Server.MapPath("../UploadedImages/InsurancePolicy/"));
                Request.Files[0].SaveAs(sFilePath);
            }
            string ChkDate = txtInsDate.Text;
            DateTime start = Convert.ToDateTime(txtInsDate.Text.Trim(), System.Globalization.CultureInfo.CurrentCulture);
            objEquipmentMaster.ExpireOn = start;

            //string sFileNameLarge = "Large_" + System.DateTime.Now.Ticks + "_" + flFile.Value;
            //sFileNameLarge = Server.MapPath("../UploadedImages/ProductImages/Large") + sFileNameLarge;
            //Request.Files[0].SaveAs(sFilePath);

            objAssetMgtRepository.SubmitChanges();
            ColorChange();
            DisplayData();
            EditNotes("Insurance Records");
            ShowUploadedFile();
            lblMsg.Text = "";
        }
        catch (Exception)
        {

        }

    }
    protected void lnkInsurancePolicy_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            objEquipmentMaster = objAssetMgtRepository.GetById(EquipmentMasterId);
            if (objEquipmentMaster.InsurancePolicy != null)
            {
                ModalInsurancePolicy.Show();
                //string sFilePath = Server.MapPath("../UploadedImages/InsurancePolicy/");
                //string strFullPath = sFilePath + objEquipmentMaster.InsurancePolicy;
                //DownloadFile(strFullPath);
            }
            else
            {
                lblMsg.Text = "No Document Found to Mail..";
            }


        }
        catch (Exception)
        {


        }
    }
    protected void btnSendInsurancePolicy_Click(object sender, EventArgs e)
    {
        try
        {
            objEquipmentMaster = objAssetMgtRepository.GetById(EquipmentMasterId);
            if (objEquipmentMaster.InsurancePolicy != null)
            {

                string sFilePath = Server.MapPath("../UploadedImages/InsurancePolicy/");
                string strFullPath = sFilePath + objEquipmentMaster.InsurancePolicy;
                string sFileName = objEquipmentMaster.InsurancePolicy;
                string sFrmadd = IncentexGlobal.CurrentMember.Email;
                string Toadd = txtSendMailTo.Text.Trim();
                string sToadd = null;
                string sFrmname = "Incentex";
                string sSubject = "Insurance Policy";
                string smtphost = Application["SMTPHOST"].ToString();
                int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                string smtpUserID = Application["SMTPUSERID"].ToString();
                string smtppassword = Application["SMTPPASSWORD"].ToString();
                string body = txtMessage.Text;
                string strEmailTemp = null;
                bool result = false;

                //
                string[] parts = Toadd.Split(',');
                foreach (string part in parts)
                {
                    sToadd = part;
                    result = new CommonMails().SendMailWithAttachment(0, null, sFrmadd, sToadd, sSubject, body, sFrmname, false, true, strFullPath, smtphost, smtpport, smtpUserID, "smtppassword", sFileName, true);
                    //result = SendMailWithMulipAttachment(sFrmadd, sToadd, sSubject, body, sFrmname, false, true, smtphost, smtpport, smtpUserID, smtppassword, true, strFullPath, sFileName);
                    /////----Insert Notes Start

                    string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement);
                    NoteDetail objComNot = new NoteDetail();
                    NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();
                    objComNot.Notecontents = sFileName;
                    objComNot.NoteFor = strNoteFor;
                    objComNot.ForeignKey = Convert.ToInt64(EquipmentMasterId);
                    objComNot.ReceivedBy = sToadd;
                    objComNot.CreateDate = System.DateTime.Now;
                    objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                    objComNot.UpdateDate = System.DateTime.Now;
                    objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                    objCompNoteHistRepos.Insert(objComNot);
                    objCompNoteHistRepos.SubmitChanges();

                    DisplayNotes();
                    /////----Insert Note End

                }
                lblMsg.Text = "Mail Sent";

            }
            else
            {
                lblMsg.Text = "No Document Found to Mail..";
            }
            txtSendMailTo.Text = "";
            txtMessage.Text = "";

        }
        catch (Exception)
        {


        }
    }
    //button to Delete Document
    protected void btnDeleteInsurancePolicy_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            objEquipmentMaster = objAssetMgtRepository.GetById(EquipmentMasterId);
            objcommon.DeleteImageFromFolder(objEquipmentMaster.InsurancePolicy, Server.MapPath("../UploadedImages/InsurancePolicy/"));
            objEquipmentMaster.InsurancePolicy = null;
            objAssetMgtRepository.SubmitChanges();
            ShowUploadedFile();
        }
        catch (Exception)
        {


        }
    }
   //--Maintenance Related Cost
    protected void lnkSaveInvoice_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        string sFilePath = null;
        try
        {
            if (EquipmentMaintenanceCostID != 0)
            {
                objEquipMaintenanceCostDtl = objAssetMgtRepository.GetByEquipMaintenanceCostId(this.EquipmentMaintenanceCostID);
            }

            objEquipMaintenanceCostDtl.EquipmentMasterID = this.EquipmentMasterId;
            if (ddlVendor.SelectedValue != "0")
            {
                objEquipMaintenanceCostDtl.Vendor = Convert.ToInt64(ddlVendor.SelectedValue);
            }
            if (txtDateofService.Text != "")
            {
                objEquipMaintenanceCostDtl.DateofService = Convert.ToDateTime(txtDateofService.Text);
            }
            objEquipMaintenanceCostDtl.Invoice = txtInvoice.Text.Trim();
            objEquipMaintenanceCostDtl.Description = txtDescription.Text;

            objEquipMaintenanceCostDtl.Amount = txtAmount.Text != "" ? Convert.ToDecimal(txtAmount.Text) : 0;
            objEquipMaintenanceCostDtl.LaborAmount = txtLaborAmount.Text != "" ? Convert.ToDecimal(txtLaborAmount.Text) : 0;
            objEquipMaintenanceCostDtl.PartsAmount = txtPartsAmount.Text != "" ? Convert.ToDecimal(txtPartsAmount.Text) : 0;

            objEquipMaintenanceCostDtl.JobCode = Convert.ToInt64(ddlJobCode.SelectedValue);
            //CheckBoxList---------
            string JobSubCOde = string.Empty;
            foreach (ListItem item in cblJobSubCode.Items)
            {
                if (item.Selected)
                {

                    if (JobSubCOde == string.Empty)
                    {
                        JobSubCOde = item.Value;
                    }
                    else
                    {
                        JobSubCOde = JobSubCOde + "," + item.Value;
                    }
                }
            }

            //---------------------
            objEquipMaintenanceCostDtl.JobSubCode = JobSubCOde;
            if (((float)Request.Files[0].ContentLength / 1048576) > 2)
            {
                lblMessage.Text = "The file you are uploading is more than 2MB.";
                modal.Show();
                return;
            }

            if (InvoiceDoc.Value != "")
            {
                objEquipMaintenanceCostDtl.DocumentPath = InvoiceDoc.Value;
                sFilePath = Server.MapPath("../UploadedImages/EquipmentInvoice/") + InvoiceDoc.Value;
                objcommon.DeleteImageFromFolder(InvoiceDoc.Value, Server.MapPath("../UploadedImages/EquipmentInvoice/"));
                Request.Files[0].SaveAs(sFilePath);
            }
            if (EquipmentMaintenanceCostID == 0)
            {
                objAssetMgtRepository.Insert(objEquipMaintenanceCostDtl);

            }

            objAssetMgtRepository.SubmitChanges();
            bindPostInvoicegrid();
            EditNotes("Maintenance Related Cost");
            EquipmentMaintenanceCostID = 0;
            ResetPopup();
        }
        catch (Exception)
        {


        }
    }
    protected void lnkPostInvoice_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        ResetPopup();
        ModalPostInvoice.Show();
    }
    //--Registration
    protected void lnkBtnSaveRegistration_Click(object sender, EventArgs e)
    {
        try
        {

            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            objEquipmentMaster = objAssetMgtRepository.GetById(this.EquipmentMasterId);
            if (txtDateRegistered.Text != "")
            {
                objEquipmentMaster.RegistrationDate = Convert.ToDateTime(txtDateRegistered.Text);
            }
            else
            {
                objEquipmentMaster.RegistrationDate = null;
            }
            if (txtRegDateExpires.Text != "")
            {
                objEquipmentMaster.RegistrationExpiryDate = Convert.ToDateTime(txtRegDateExpires.Text);
            }
            else
            {
                objEquipmentMaster.RegistrationExpiryDate = null;
            }


            objEquipmentMaster.UpdatedDate = DateTime.Now;
            objEquipmentMaster.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objAssetMgtRepository.SubmitChanges();

            EditNotes("Registration Record");
            DisplayData();
            DisplayPopupData();
            ColorChange();
            lblMsg.Text = "";
        }
        catch (Exception)
        {


        }
    }
    protected void lnkBtnSaveRegInternalContacts_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }
            
            objEquipmentMaster = objAssetMgtRepository.GetById(this.EquipmentMasterId);
            objEquipmentMaster.RegCompanyContactName = txtRegCmpCntName.Text.Trim();
            objEquipmentMaster.RegCompanyContactTelephone = txtRegCmpCntTelephone.Text.Trim();
            objEquipmentMaster.RegCompanyContactEmail = txtRegCmpCntEmail.Text.Trim();
            objEquipmentMaster.RegPaymentMode = rdbPaymentModeOnline.Checked;
            objEquipmentMaster.RegOnlinePaymentURL = txtRegOnlinePaymentURL.Text.Trim();
           
            objEquipmentMaster.UpdatedDate = DateTime.Now;
            objEquipmentMaster.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objAssetMgtRepository.SubmitChanges();
            EditNotes("Registration Record");

            DisplayData();
            DisplayPopupData();
            lblMsg.Text = "";
        }
        catch (Exception)
        {


        }
    }
    protected void lnkSaveRegDoc_Click(object sender, EventArgs e)
    {
        lblErrorRegistration.Text = "";
        string sFilePath = null;
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            objEquipmentMaster = objAssetMgtRepository.GetById(this.EquipmentMasterId);

            if (((float)Request.Files[0].ContentLength / 1048576) > 2)
            {
                lblErrorRegDoc.Text = "The file you are uploading is more than 2MB.";
                ModalRegistration.Show();
                return;
            }

            if (flRegDoc.Value != "")
            {
                objEquipmentMaster.RegistrationDoc = flRegDoc.Value;
                sFilePath = Server.MapPath("../UploadedImages/RegistrationDocument/") + flRegDoc.Value;
                objcommon.DeleteImageFromFolder(flRegDoc.Value, Server.MapPath("../UploadedImages/RegistrationDocument/"));
                Request.Files[0].SaveAs(sFilePath);
            }


            objAssetMgtRepository.SubmitChanges();
            DisplayData();
            EditNotes("Registration Record");
            ShowUploadedFile();
        }
        catch (Exception)
        {

        }

    }
    protected void lnkRegistrationPolicy_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            objEquipmentMaster = objAssetMgtRepository.GetById(EquipmentMasterId);
            if (objEquipmentMaster.RegistrationDoc != null)
            {

                string sFilePath = Server.MapPath("../UploadedImages/RegistrationDocument/");
                //DownloadFile(sFilePath, objEquipmentMaster.InsurancePolicy);

                string strFullPath = sFilePath + objEquipmentMaster.RegistrationDoc;
                DownloadFile(strFullPath);
            }
            else
            {
                lblErrorRegistration.Text = "No Document Found..";
            }

        }
        catch (Exception)
        {


        }
    }
    //button to Delete Document
    protected void btnDeleteRegDoc_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            objEquipmentMaster = objAssetMgtRepository.GetById(EquipmentMasterId);
            objcommon.DeleteImageFromFolder(objEquipmentMaster.RegistrationDoc, Server.MapPath("../UploadedImages/RegistrationDocument/"));
            objEquipmentMaster.RegistrationDoc = null;
            objAssetMgtRepository.SubmitChanges();
            ShowUploadedFile();
        }
        catch (Exception)
        {


        }
    }
   //--Weekly Maintenance
    protected void lnkSaveWeeklyMaintinance_Click(object sender, EventArgs e)
    {

        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            if (EquipmentWeeklyMaintinanceID != 0)
            {
                objEquipmentWeeklyMaintenance = objAssetMgtRepository.GetByEquipWeeklyMaintenanceId(this.EquipmentWeeklyMaintinanceID);
            }

            objEquipmentWeeklyMaintenance.EquipmentMasterID = this.EquipmentMasterId;

            if (txtWeekOfDate.Text != "")
            {
                objEquipmentWeeklyMaintenance.WeekOfDate = Convert.ToDateTime(txtWeekOfDate.Text);
            }
            objEquipmentWeeklyMaintenance.Hours = txtEventHours.Text != "" ? Convert.ToInt64(txtEventHours.Text.Trim()) : 0;
            objEquipmentWeeklyMaintenance.Oil = txtOil.Text.Trim();
            objEquipmentWeeklyMaintenance.AntiFreeze = txtAntiFreeze.Text.Trim();
            objEquipmentWeeklyMaintenance.ServiceBrake = rdbServiceBrakeT.Checked;
            objEquipmentWeeklyMaintenance.ParkBrake = rdbParkBrakeT.Checked;
            objEquipmentWeeklyMaintenance.Lights = rdbLightsT.Checked;
            objEquipmentWeeklyMaintenance.ATF = rdbATFT.Checked;
            objEquipmentWeeklyMaintenance.Tires = rdbTiresT.Checked;

            if (EquipmentWeeklyMaintinanceID == 0)
            {
                objAssetMgtRepository.Insert(objEquipmentWeeklyMaintenance);

            }
            lblPostEvent.Text = "";
            objAssetMgtRepository.SubmitChanges();
            bindPostEventgrid();
            EquipmentWeeklyMaintinanceID = 0;
            ResetPopup();
            EditNotes("Weekly Maintenance Record");
        }
        catch (Exception)
        {


        }
    }
    //--Equipment Images
    protected void btnSaveImage_Click(object sender, EventArgs e)
    {
        string sFilePath = null;

        try
        {
            if (!base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            if (((float)Request.Files[0].ContentLength / 1048576) > 2)
            {
                lblErrorImage.Text = "The file you are uploading is more than 2MB.";
                modal.Show();
                return;
            }

            if (flImage.Value != "")
            {
                objEquipmentImage.ImageFileName = flImage.Value;
                sFilePath = Server.MapPath("../UploadedImages/EquipmentImage/") + flImage.Value;
                objcommon.DeleteImageFromFolder(flImage.Value, Server.MapPath("../UploadedImages/EquipmentImage/"));
                Request.Files[0].SaveAs(sFilePath);
            }

            if (txtImageDate.Text != "")
            {
                objEquipmentImage.ImageDate = Convert.ToDateTime(txtImageDate.Text.Trim());
            }
            else
            {
                objEquipmentImage.ImageDate = null;
            }

            objEquipmentImage.ImageName = txtImageName.Text.Trim();
            objEquipmentImage.IsAssetImage = rdbAssetImage.Checked;
            objEquipmentImage.EquipmentMasterID = this.EquipmentMasterId;
            objEquipmentImage.CreatedDate = System.DateTime.Now;
            objEquipmentImage.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objAssetMgtRepository.Insert(objEquipmentImage);
            objAssetMgtRepository.SubmitChanges();
            getImages();
            EditNotes("Images");
        }
        catch (Exception)
        {

        }

    }
    //--Asset & Parts Specifications
    protected void lnkbtnSaveField_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            if (dtlField.Items.Count != 0)
            {

                foreach (DataListItem item in dtlField.Items)
                {
                    HiddenField hdnFieldMasterID = (HiddenField)item.FindControl("hdnFieldMasterID");
                    //HiddenField lblFieldName = (HiddenField)item.FindControl("lblFieldName");
                    TextBox txtFieldDesc = (TextBox)item.FindControl("txtFieldDesc");
                    EquipmentFieldDetail objFieldDetail = new EquipmentFieldDetail();
                    DataTable dt = new DataTable();
                    Int64 FieldMasterID = 0;
                    Boolean ToUpdate = false;
                    dt = ListToDataTable(objFieldRepository.GetFieldDetailDesc(this.FieldCompany, this.EquipmentMasterId));
                    foreach (DataRow dr in dt.Rows)
                    {
                        FieldMasterID = Convert.ToInt64(dr[2]);
                        if (FieldMasterID == Convert.ToInt64(hdnFieldMasterID.Value))
                        {
                            ToUpdate = true;
                        }
                    }
                    if (ToUpdate == true)
                    {
                        objFieldDetail = objFieldRepository.GetFieldDetailById(Convert.ToInt64(hdnFieldMasterID.Value), this.EquipmentMasterId);
                    }
                    objFieldDetail.CompanyID = this.FieldCompany;
                    objFieldDetail.Description = txtFieldDesc.Text;
                    objFieldDetail.EquipmentMasterID = this.EquipmentMasterId;
                    objFieldDetail.FieldMasterID = Convert.ToInt64(hdnFieldMasterID.Value);
                    if (ToUpdate == false)
                    {
                        objFieldRepository.Insert(objFieldDetail);
                    }
                    objFieldRepository.SubmitChanges();
                }
                EditNotes("Asset & Parts Specifications");
                lblFieldError.Text = "Record Saved Successfully";
            }

        }
        catch (Exception)
        {


        }
    }
   
   
   
    protected void lnkButtonSaveHistory_Click(object sender, EventArgs e)
    {
        try
        {
            string strNoteHistory = "";
            string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement);

            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();


            objComNot.Notecontents = txtOrderNotesHistory.Text;
            objComNot.NoteFor = strNoteFor;
            objComNot.ForeignKey = Convert.ToInt64(EquipmentMasterId);
            objComNot.SpecificNoteFor = ddlShowMe.SelectedValue;
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objComNot.UpdateDate = System.DateTime.Now;
            objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            if (!(string.IsNullOrEmpty(txtOrderNotesHistory.Text)))
            {
                strNoteHistory = txtOrderNotesHistory.Text;
                objCompNoteHistRepos.Insert(objComNot);
                objCompNoteHistRepos.SubmitChanges();
                int NoteId = (int)(objComNot.NoteID);
                txtOrderNotesHistory.Text = "";
                SendEmailNotes(strNoteHistory, Convert.ToInt64(ddlShowMe.SelectedValue));
            }
           
            DisplayNotes();
           
        }

        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

        }
    }
    
    protected void lnkAddEquipmentImage_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        txtImageDate.Text = "";
        txtImageName.Text = "";
        ModalEquipmentImage.Show();
    }
    
    protected void ddlShowMe_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblMsg.Text = "";
            lblMsgGrid.Text = "";
            lblErrorRegistration.Text = "";
            lblPostEvent.Text = "";
            lblFieldError.Text = "";
            if (ddlShowMe.SelectedValue != "0")
            {
                if (ddlShowMe.SelectedValue == "1")
                {
                    dvInsuranceHide.Visible = true;
                    dvMaintenanceCostHide.Visible = false;
                    dvRegistration.Visible = false;
                    dvWeeklyMaintenanceHide.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvFieldPartSpecification.Visible = false;
                    dvPartsHistory.Visible = false;
                    dvRepairOrder.Visible = false;
                }
                else if (ddlShowMe.SelectedValue == "3")
                {
                    dvInsuranceHide.Visible = false;
                    dvMaintenanceCostHide.Visible = true;
                    dvRegistration.Visible = false;
                    dvWeeklyMaintenanceHide.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvFieldPartSpecification.Visible = false;
                    dvPartsHistory.Visible = false;
                    dvRepairOrder.Visible = false;
                }
                else if (ddlShowMe.SelectedValue == "4")
                {
                    dvInsuranceHide.Visible = false;
                    dvMaintenanceCostHide.Visible = false;
                    dvRegistration.Visible = true;
                    dvWeeklyMaintenanceHide.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvFieldPartSpecification.Visible = false;
                    dvPartsHistory.Visible = false;
                    dvRepairOrder.Visible = false;
                }
                else if (ddlShowMe.SelectedValue == "5")
                {
                    dvInsuranceHide.Visible = false;
                    dvMaintenanceCostHide.Visible = false;
                    dvRegistration.Visible = false;
                    dvWeeklyMaintenanceHide.Visible = true;
                    dvEquipmentImageHide.Visible = false;
                    dvFieldPartSpecification.Visible = false;
                    dvPartsHistory.Visible = false;
                    dvRepairOrder.Visible = false;
                }
                else if (ddlShowMe.SelectedValue == "6")
                {
                    dvInsuranceHide.Visible = false;
                    dvMaintenanceCostHide.Visible = false;
                    dvRegistration.Visible = false;
                    dvWeeklyMaintenanceHide.Visible = false;
                    dvEquipmentImageHide.Visible = true;
                    dvFieldPartSpecification.Visible = false;
                    dvPartsHistory.Visible = false;
                    dvRepairOrder.Visible = false;
                }
                else if (ddlShowMe.SelectedValue == "7")
                {
                    dvInsuranceHide.Visible = false;
                    dvMaintenanceCostHide.Visible = false;
                    dvRegistration.Visible = false;
                    dvWeeklyMaintenanceHide.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvFieldPartSpecification.Visible = true;
                    dvPartsHistory.Visible = false;
                    dvRepairOrder.Visible = false;
                }
                else if (ddlShowMe.SelectedValue == "8")
                {
                    dvInsuranceHide.Visible = false;
                    dvMaintenanceCostHide.Visible = false;
                    dvRegistration.Visible = false;
                    dvWeeklyMaintenanceHide.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvFieldPartSpecification.Visible = false;
                    dvPartsHistory.Visible = true;
                    dvRepairOrder.Visible = false;
                }
                else if (ddlShowMe.SelectedValue == "9")
                {
                    dvInsuranceHide.Visible = false;
                    dvMaintenanceCostHide.Visible = false;
                    dvRegistration.Visible = false;
                    dvWeeklyMaintenanceHide.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvFieldPartSpecification.Visible = false;
                    dvPartsHistory.Visible = false;
                    dvRepairOrder.Visible = true;
                }
            }
            else
            {
                ResetControls();

            }

        }
        catch (Exception)
        {


        }
    }
    protected void ddlJobCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        lblMsgGrid.Text = "";
        lblErrorRegistration.Text = "";
        lblPostEvent.Text = "";
        DataTable dt = new DataTable();
        try
        {
           
            if (ddlJobCode.SelectedIndex != 0)
            {
                dt =  ListToDataTable(objAssetMgtRepository.GetAllSubJobCodeDetail(Convert.ToInt64(ddlJobCode.SelectedValue)));
                if (dt.Rows.Count!=0)
                {
                    cblJobSubCode.DataSource = dt;
                    cblJobSubCode.DataBind();
                }
            }
            else
            {
                ResetControls();

            }
            ModalPostInvoice.Show();
        }
        catch (Exception)
        {


        }
    }
    protected void chkHideEdited_CheckedChanged(object sender, EventArgs e)
    {
        if (chkHideEdited.Checked)
        {
            chkSpan.Attributes.Add("class", "custom-checkbox_checked alignright");
            DisplayCreatedNotes();
        }
        else
        {
            chkSpan.Attributes.Add("class", "custom-checkbox alignright");
        }
    }
  
    protected void gvEquipment_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "del")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            Int64 val = Convert.ToInt64(e.CommandArgument);
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            {
                objAssetMgtRepository.DeleteMaintinanceCost(val);
                lblMsgGrid.Text = "Record deleted successfully";
                bindPostInvoicegrid();
            }
            else
            {
                lblMsgGrid.Text = "Sorry You are not Authorized to Delete this Record";
            }
        }
        if (e.CommandName == "Vendor")
        {
            ResetPopup();
            Int64 val = Convert.ToInt64(e.CommandArgument);
            EquipmentMaintenanceCostID = Convert.ToInt64(e.CommandArgument);
           objEquipMaintenanceCostDtl= objAssetMgtRepository.GetByEquipMaintenanceCostId(val);
           if (Convert.ToString(objEquipMaintenanceCostDtl.Vendor)!=string.Empty)
           {
               ddlVendor.SelectedValue = Convert.ToString(objEquipMaintenanceCostDtl.Vendor);
           }          
           if (Convert.ToString(objEquipMaintenanceCostDtl.DateofService)!=string.Empty)
           {
               txtDateofService.Text = Convert.ToString(Convert.ToDateTime(objEquipMaintenanceCostDtl.DateofService).ToString("MM/dd/yyyy"));    
           }           
           txtInvoice.Text = objEquipMaintenanceCostDtl.Invoice;
           txtDescription.Text = objEquipMaintenanceCostDtl.Description;
           txtAmount.Text = Convert.ToString(objEquipMaintenanceCostDtl.Amount);
           txtLaborAmount.Text = Convert.ToString(objEquipMaintenanceCostDtl.LaborAmount);
           txtPartsAmount.Text = Convert.ToString(objEquipMaintenanceCostDtl.PartsAmount);
           if (objEquipMaintenanceCostDtl.JobCode!=null)
           {
               ddlJobCode.SelectedValue = Convert.ToString(objEquipMaintenanceCostDtl.JobCode);
               
               BindChecklist(Convert.ToInt64(ddlJobCode.SelectedValue));
              
               string[] JobSubCode = objEquipMaintenanceCostDtl.JobSubCode.Split(',');
               foreach (ListItem item in cblJobSubCode.Items)
               {
                   foreach (string i in JobSubCode)
                   {
                       if (i.Equals(item.Value))
                       {
                           item.Selected = true;
                       }
                   }
               }

           }         
            ModalPostInvoice.Show();
            
        }
        if (e.CommandName == "PDF")
        {
            Int64 val = Convert.ToInt64(e.CommandArgument);
            objEquipMaintenanceCostDtl = objAssetMgtRepository.GetByEquipMaintenanceCostId(val);
            if (objEquipMaintenanceCostDtl.DocumentPath != null)
            {

                string sFilePath = Server.MapPath("../UploadedImages/EquipmentInvoice/");
                //DownloadFile(sFilePath, objEquipmentMaster.InsurancePolicy);

                string strFullPath = sFilePath + objEquipMaintenanceCostDtl.DocumentPath;
                DownloadFile(strFullPath);
            }
            else
            {
                lblMsgGrid.Text = "No Document Found..";
            }
        }

    }

    protected void gvEquipment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblAmount = (Label)e.Row.FindControl("lblAmount");
            Decimal Amount = Decimal.Parse(lblAmount.Text);
            totalAmount += Amount;

            //Change PDF & XL Icon Conditionally
            string FileName = string.Empty;
            string FileExt = string.Empty;
            ImageButton lnkbtnPDF = (ImageButton)e.Row.FindControl("lnkbtnPDF");
            Label lblEquipmentMaintinanceID = (Label)e.Row.FindControl("lblEquipmentID");
            Int64 val = lblEquipmentMaintinanceID.Text != null ? Convert.ToInt64(lblEquipmentMaintinanceID.Text) : 0;
            objEquipMaintenanceCostDtl = objAssetMgtRepository.GetByEquipMaintenanceCostId(val);
            if (objEquipMaintenanceCostDtl.DocumentPath != null)
            {
                lnkbtnPDF.Enabled = true;
                FileName = objEquipMaintenanceCostDtl.DocumentPath;
                string[] parts = FileName.Split('.');
                FileExt = parts[1];
                if (FileExt == "pdf")
                {                    
                    lnkbtnPDF.ImageUrl = string.Format("~/Images/pdf.png");                   
                }
                else if (FileExt == "xl"||FileExt == "xls")
                {
                    lnkbtnPDF.ImageUrl = string.Format("~/Images/excel_small.png");
                }
                else
                {
                    lnkbtnPDF.ImageUrl = string.Format("~/Images/Document.png");
                }

            }
            else
            {
                lnkbtnPDF.ImageUrl = string.Format("~/Images/spacer.gif");
                lnkbtnPDF.Enabled = false;
            }
           
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblTotalAmount = (Label)e.Row.FindControl("lblTotalAmount");
            lblTotalAmount.Text = totalAmount.ToString();           
        }
    }

    protected void gvPostEvent_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "del")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            Int64 val = Convert.ToInt64(e.CommandArgument);
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            {
                objAssetMgtRepository.DeleteWeeklyMaintenance(val);
                lblPostEvent.Text = "Record deleted successfully";
                bindPostEventgrid();
            }
            else
            {
                lblPostEvent.Text = "Sorry You are not Authorized to Delete this Record";
            }
        }
        if (e.CommandName == "WeekOfDate")
        {
            lblPostEvent.Text = "";
            Int64 val = Convert.ToInt64(e.CommandArgument);
            EquipmentWeeklyMaintinanceID = Convert.ToInt64(e.CommandArgument);
            objEquipmentWeeklyMaintenance = objAssetMgtRepository.GetByEquipWeeklyMaintenanceId(val);

            if (Convert.ToString(objEquipmentWeeklyMaintenance.WeekOfDate) != string.Empty)
            {
                txtWeekOfDate.Text = Convert.ToString(Convert.ToDateTime(objEquipmentWeeklyMaintenance.WeekOfDate).ToString("MM/dd/yyyy"));
            }
            txtEventHours.Text = Convert.ToString(objEquipmentWeeklyMaintenance.Hours);
            txtOil.Text = objEquipmentWeeklyMaintenance.Oil;
            txtAntiFreeze.Text = objEquipmentWeeklyMaintenance.AntiFreeze;
            rdbServiceBrakeT.Checked = Convert.ToBoolean(objEquipmentWeeklyMaintenance.ServiceBrake);
            rdbParkBrakeT.Checked = Convert.ToBoolean(objEquipmentWeeklyMaintenance.ParkBrake);
            rdbLightsT.Checked = Convert.ToBoolean(objEquipmentWeeklyMaintenance.Lights);
            rdbServiceBrakeT.Checked = Convert.ToBoolean(objEquipmentWeeklyMaintenance.ATF);
            rdbTiresT.Checked = Convert.ToBoolean(objEquipmentWeeklyMaintenance.Tires);
            ModalWeeklyMaintinance.Show();

        }
       
    }

    protected void gvPostEvent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Image imgServiceBrake = (Image)e.Row.FindControl("imgServiceBrake");
                Image imgParkBrake = (Image)e.Row.FindControl("imgParkBrake");
                Image imgLights = (Image)e.Row.FindControl("imgLights");
                Image imgATF = (Image)e.Row.FindControl("imgATF");
                Image imgTires = (Image)e.Row.FindControl("imgTires");
                HiddenField hfParkBrake = (HiddenField)e.Row.FindControl("hfParkBrake");
                Label lblEquipmentWeeklyMaintinanceID = (Label)e.Row.FindControl("lblEquipmentWeeklyMaintinanceID");
                Int64 val = lblEquipmentWeeklyMaintinanceID.Text != null ? Convert.ToInt64(lblEquipmentWeeklyMaintinanceID.Text) : 0;
                objEquipmentWeeklyMaintenance = objAssetMgtRepository.GetByEquipWeeklyMaintenanceId(val);

                if (objEquipmentWeeklyMaintenance.ServiceBrake == true)
                {
                    imgServiceBrake.ImageUrl = string.Format("~/Images/GreenCircle.png");
                }
                else
                {
                    imgServiceBrake.ImageUrl = string.Format("~/Images/RedCircle.png");
                }

                if (objEquipmentWeeklyMaintenance.ParkBrake == true)
                {
                    imgParkBrake.ImageUrl = string.Format("~/Images/GreenCircle.png");
                }
                else
                {
                    imgParkBrake.ImageUrl = string.Format("~/Images/RedCircle.png");
                }

                if (objEquipmentWeeklyMaintenance.Lights == true)
                {
                    imgLights.ImageUrl = string.Format("~/Images/GreenCircle.png");
                }
                else
                {
                    imgLights.ImageUrl = string.Format("~/Images/RedCircle.png");
                }

                if (objEquipmentWeeklyMaintenance.ATF == true)
                {
                    imgATF.ImageUrl = string.Format("~/Images/GreenCircle.png");
                }
                else
                {
                    imgATF.ImageUrl = string.Format("~/Images/RedCircle.png");
                }

                if (objEquipmentWeeklyMaintenance.Tires == true)
                {
                    imgTires.ImageUrl = string.Format("~/Images/GreenCircle.png");
                }
                else
                {
                    imgTires.ImageUrl = string.Format("~/Images/RedCircle.png");
                }





            }
        }
        catch (Exception)
        {


        }
    }

    #region DataList Events

    /// <summary>
    /// Datalst Item data bound command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dtSplash_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ((HtmlImage)e.Item.FindControl("imgSplashImage")).Src = "~/UploadedImages/EquipmentImage/" + ((HiddenField)e.Item.FindControl("hdnImageFileName")).Value;
            ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/EquipmentImage/" + ((HiddenField)e.Item.FindControl("hdnImageFileName")).Value;
            //((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "../../controller/createthumb.aspx?_ty=CompanyStoreDocument&_path=" + ((HiddenField)e.Item.FindControl("hdndocumentname")).Value + "&_twidth=600&_theight=600";

            //"~/UploadedImages/CompanyStoreDocuments/" + ((HiddenField)e.Item.FindControl("hdndocumentname")).Value;

        }
    }

    /// <summary>
    /// Datalst Itemcommand event
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void dtSplash_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "DeleteSplashImage")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            objAssetMgtRepository.DeleteEquipmentImage(Convert.ToInt64(e.CommandArgument));
            objcommon.DeleteImageFromFolder(((HiddenField)e.Item.FindControl("hdnImageFileName")).Value, Server.MapPath("../UploadedImages/EquipmentImage/"));
        }
        getImages();

    }

    /// <summary>
    /// Datalst Item data bound command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dtlField_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //DataRow drItem = ((DataRowView)e.Row.DataItem).Row;
            
            TextBox txtStratingCreditAmount = e.Item.FindControl("txtFieldDesc") as TextBox;


        }
    }
    #endregion
  
    #endregion

    #region Methods

    void DisplayData()
    {
        DataTable dt = new DataTable();
        AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
        try
        {

        dt = ListToDataTable(objAssetMgtRepository.GetEquipmentsDetail(null, null, null, null,Convert.ToString(EquipmentMasterId), null,null,CompanyID,0));//0 for VEUserInfoID
        if (dt.Rows.Count != 0)
        {
            this.EquipmentID = Convert.ToString(dt.Rows[0]["EquipmentID"]);
            this.EquipmentTypeID = Convert.ToInt64(dt.Rows[0]["EquipmentTypeID"]);
            this.FieldCompany = Convert.ToInt64(dt.Rows[0]["CompanyID"]);
            //--Main Start--
            lblAssetType.Text = Convert.ToString(dt.Rows[0]["EquiType"]);            
            lblBrand.Text = Convert.ToString(dt.Rows[0]["Brand"]);
            lblSerialNo.Text = Convert.ToString(dt.Rows[0]["SerialNumber"]);
            lblYear.Text = Convert.ToString(dt.Rows[0]["Year"]);
            lblBaseStation.Text = Convert.ToString(dt.Rows[0]["BaseStation"]);
            lblAssetID.Text = Convert.ToString(dt.Rows[0]["EquipmentID"]);
            lblFuel.Text = Convert.ToString(dt.Rows[0]["Fuel"]);
            lblHours.Text = Convert.ToString(dt.Rows[0]["Hours"]);
            if (Convert.ToString(dt.Rows[0]["LastInspectionDate"])!=string.Empty)
            {
                lblLastInspection.Text = Convert.ToString(Convert.ToDateTime(dt.Rows[0]["LastInspectionDate"]).ToString("MM/dd/yyyy"));
            }
           
           //--Main End--
            //--Insurance Start--
            lblInsuranceCompany.Text = Convert.ToString(dt.Rows[0]["InsuranceCompany"]);
            lblPolicyNumber.Text = Convert.ToString(dt.Rows[0]["PolicyNumber"]);
            if (Convert.ToString(dt.Rows[0]["ExpireOn"]) != string.Empty)
            {
                lblExpires.Text = Convert.ToString(Convert.ToDateTime(dt.Rows[0]["ExpireOn"]).ToString("MM/dd/yyyy"));
            }           
            lblStatus.Text = Convert.ToString(dt.Rows[0]["Status"]);
            lblAssetVinNumber.Text = Convert.ToString(dt.Rows[0]["VinNumber"]);
            lblAssetPlateNumber.Text = Convert.ToString(dt.Rows[0]["PlateNumber"]);
            //--Insurance End--

            //--Company Contact Start
            lblICContactName.Text = Convert.ToString(dt.Rows[0]["InternalCompContactName"]);
            lblICContactTelephone.Text = Convert.ToString(dt.Rows[0]["InternalCompContactTelephone"]);
            lblICContactEmail.Text = Convert.ToString(dt.Rows[0]["InternalCompContactEmail"]);
            lblInsAgentCompany.Text = Convert.ToString(dt.Rows[0]["InsuranceAgentCompany"]);
            lblInsAgentName.Text = Convert.ToString(dt.Rows[0]["InsuranceAgentName"]);
            lblInsAgentTelephone.Text = Convert.ToString(dt.Rows[0]["InsuranceAgentTelephone"]);
            lblInsAgentEmail.Text = Convert.ToString(dt.Rows[0]["InsuranceAgentEmail"]);
            //--Company Contact End

            //--Registration Start
            lblAssetTypeReg.Text = Convert.ToString(dt.Rows[0]["EquiType"]);
            lblAssetVinNumberReg.Text = Convert.ToString(dt.Rows[0]["VinNumber"]);
            lblAssetPlateNumberReg.Text = Convert.ToString(dt.Rows[0]["PlateNumber"]);
            if (Convert.ToString(dt.Rows[0]["RegistrationDate"]) != string.Empty)
            {
                lblDateRegistered.Text = Convert.ToString(Convert.ToDateTime(dt.Rows[0]["RegistrationDate"]).ToString("MM/dd/yyyy"));
            }
            else
            {
                lblDateRegistered.Text = "";
            }

            if (Convert.ToString(dt.Rows[0]["RegistrationExpiryDate"]) != string.Empty)
            {
                lblRegDateExpires.Text = Convert.ToString(Convert.ToDateTime(dt.Rows[0]["RegistrationExpiryDate"]).ToString("MM/dd/yyyy"));
            }
            else
            {
                lblRegDateExpires.Text = "";
            }
            lblRegCmpCntName.Text = Convert.ToString(dt.Rows[0]["RegCompanyContactName"]);
            lblRegCmpCntTelephone.Text = Convert.ToString(dt.Rows[0]["RegCompanyContactTelephone"]);
            lblRegCmpCntEmail.Text = Convert.ToString(dt.Rows[0]["RegCompanyContactEmail"]);
           // lblRegPaymentBy.Text = Convert.ToString(dt.Rows[0]["RegPaymentMode"]);
            if (Convert.ToBoolean(dt.Rows[0]["RegPaymentMode"]))
            {
                lblRegPaymentBy.Text = "Pay Online";
            }
            else
            {
                lblRegPaymentBy.Text = "Pay by Cheque";
            }
            hlnkRegOnlinePayment.Text = Convert.ToString(dt.Rows[0]["RegOnlinePaymentURL"]);
            hlnkRegOnlinePayment.Target = "_blank";
            hlnkRegOnlinePayment.NavigateUrl ="http://" + Convert.ToString(dt.Rows[0]["RegOnlinePaymentURL"]);
            //--Registration End
            ;
        }
     
        ColorChange();
        DisplayNotes();
        }
        catch (Exception)
        {

           
        }

    }
    public void BindPageDropdowns()
    {
       
        if (Convert.ToString(Session["Usr"]) == Convert.ToString(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
        {
            List<GetEquipDropdownbyUserInfoIDResult> objlstByUserInfo = new List<GetEquipDropdownbyUserInfoIDResult>();
            objlstByUserInfo = objAssetVendorRepository.GetDropDownsByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);
            Common.BindDDL(ddlShowMe, objlstByUserInfo, "EquipmentDropDownName", "EquipmentDropDownID", "--Select--");
        }
        else
        {
            List<EquipmentDropDown> objddlList = new List<EquipmentDropDown>();
            objddlList = objAssetVendorRepository.GetDropDownControl();
            Common.BindDDL(ddlShowMe, objddlList, "EquipmentDropDownName", "EquipmentDropDownID", "--Select--");
        }

        //Bind Dropdown Job Code        
        List<EquipmentJobCodeLookup> objEquipmentJobCodeLookupList = new List<EquipmentJobCodeLookup>();
        objEquipmentJobCodeLookupList = objAssetMgtRepository.GetAllJobCode();
        Common.BindDDL(ddlJobCode, objEquipmentJobCodeLookupList, "JobCodeName", "JobCodeID", "--Select--");
        
    }
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        try
        {

       
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            //table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            dt.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }
        return dt;
        }
        catch (Exception)
        {

            throw;
        }
    }
    /// <summary>
    /// Add File for Insurance Policy
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void DisplayPopupData()
    {
        try
        {

       
        AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
        EquipmentMaster objEquipmentMaster = objAssetMgtRepository.GetById(EquipmentMasterId);
        if (objEquipmentMaster != null)
        {
            //--Main Start--
            ddlEquipmentType.SelectedValue = Convert.ToString(objEquipmentMaster.EquipmentTypeID);
            ddlBrand.SelectedValue = Convert.ToString(objEquipmentMaster.BrandID);
            txtModelYear.Text = Convert.ToString(objEquipmentMaster.ModelYear);
            txtSerialNo.Text = Convert.ToString(objEquipmentMaster.SerialNumber);
            ddlFuelType.SelectedValue = Convert.ToString(objEquipmentMaster.FuelTypeID);
            txtEquipmentId.Text = objEquipmentMaster.EquipmentID;
            ddlBaseStation.SelectedValue = Convert.ToString(objEquipmentMaster.BaseStationID);
            txtHours.Text = Convert.ToString(objEquipmentMaster.Hours);
            if (Convert.ToString(objEquipmentMaster.LastInspectionDate)!=string.Empty)
            {
                txtLastInspection.Text = Convert.ToString(Convert.ToDateTime(objEquipmentMaster.LastInspectionDate).ToString("MM/dd/yyyy"));
            }
            
            //--Main End--
            //--Insurance Start--
            txtInsuranceCompany.Text = Convert.ToString(objEquipmentMaster.InsuranceCompany);
            txtPolicynumber.Text = Convert.ToString(objEquipmentMaster.PolicyNumber);
            txtAssetVinNumber.Text = Convert.ToString(objEquipmentMaster.VinNumber);
            txtAssetPlateNumber.Text = Convert.ToString(objEquipmentMaster.PlateNumber);
            ddlStatus.SelectedValue = Convert.ToString(objEquipmentMaster.Status);
           
            //--Insurance End--
            //--Internal Contact Start--
            txtPICCName.Text = Convert.ToString(objEquipmentMaster.InternalCompContactName);
            txtPICCTelephone.Text = Convert.ToString(objEquipmentMaster.InternalCompContactTelephone);
            txtPICCEmail.Text = Convert.ToString(objEquipmentMaster.InternalCompContactEmail);
            txtInsAgentCompany.Text = Convert.ToString(objEquipmentMaster.InsuranceAgentCompany);
            txtInsAgentName.Text = Convert.ToString(objEquipmentMaster.InsuranceAgentName);
            txtInsAgentTelephone.Text = Convert.ToString(objEquipmentMaster.InsuranceAgentTelephone);
            txtInsAgentEmail.Text = Convert.ToString(objEquipmentMaster.InsuranceAgentEmail);
            //--Internal Contact End--
            //--Registration Start
            if (Convert.ToString(objEquipmentMaster.RegistrationDate) != string.Empty)
            {
                txtDateRegistered.Text = Convert.ToString(Convert.ToDateTime(objEquipmentMaster.RegistrationDate).ToString("MM/dd/yyyy"));
            }
            else
            {
                txtDateRegistered.Text = "";
            }
            if (Convert.ToString(objEquipmentMaster.RegistrationExpiryDate) != string.Empty)
            {
                txtRegDateExpires.Text = Convert.ToString(Convert.ToDateTime(objEquipmentMaster.RegistrationExpiryDate).ToString("MM/dd/yyyy"));
            }
            else
            {
                txtRegDateExpires.Text = "";
            }
            txtRegCmpCntName.Text = Convert.ToString(objEquipmentMaster.RegCompanyContactName);
            txtRegCmpCntTelephone.Text = Convert.ToString(objEquipmentMaster.RegCompanyContactTelephone);
            txtRegCmpCntEmail.Text = Convert.ToString(objEquipmentMaster.RegCompanyContactEmail);
            txtRegOnlinePaymentURL.Text = Convert.ToString(objEquipmentMaster.RegOnlinePaymentURL);
            if (objEquipmentMaster.RegPaymentMode)
            {
                rdbPaymentModeOnline.Checked = true;
            }
            else
            {
                rdbPaymentModeCheck.Checked = true;
            }
            
            //--Registration End
        }
        }
        catch (Exception)
        {

          
        }
    }
    public void ShowUploadedFile()
    {
        try
        {
             EquipmentMaster objEquipmentMaster = objAssetMgtRepository.GetById(EquipmentMasterId);
             if (objEquipmentMaster != null)
             {
                 //Insurance
                 if (Convert.ToString(objEquipmentMaster.ExpireOn)!=string.Empty)
                 {
                     txtInsDate.Text = Convert.ToDateTime(objEquipmentMaster.ExpireOn).ToString("MM/dd/yyyy");
                 }
                 if (objEquipmentMaster.InsurancePolicy != null)
                 {
                     dvInsurancePolicyHide.Visible = true;
                     lblInsurancePolicyName.Text = Convert.ToString(objEquipmentMaster.InsurancePolicy);
                 }
                 else
                 {
                     dvInsurancePolicyHide.Visible = false;
                 }           
                 //Registration
                 if (objEquipmentMaster.RegistrationDoc != null)
                 {
                     dvRegDocHide.Visible = true;
                     lblRegDocName.Text = Convert.ToString(objEquipmentMaster.RegistrationDoc);
                 }
                 else
                 {
                     dvRegDocHide.Visible = false;
                 }                 
             }
            
        }
        catch (Exception)
        {
            
           
        }
    }
    public void BindPopupValues()
    {
        try
        {
            LookupRepository objLookupRepository = new LookupRepository();
            List<INC_Lookup> objList = new List<INC_Lookup>();
            String LookUpCode = string.Empty;
            //get Equipment Type
            LookUpCode = "EquipmentType";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            Common.BindDDL(ddlEquipmentType, objList, "sLookupName", "iLookupID", "-Select Equipment Type-");
            //get Brand
            LookUpCode = "Brand";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            Common.BindDDL(ddlBrand, objList, "sLookupName", "iLookupID", "-Select Brand-");
            //get Fuel Type
            LookUpCode = "FuelType";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            Common.BindDDL(ddlFuelType, objList, "sLookupName", "iLookupID", "-Select Fuel Type-");
            //get Status
            LookUpCode = "EquipmentStatus";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            Common.BindDDL(ddlStatus, objList, "sLookupName", "iLookupID", "-Select Status-");
            //get Base Stations
            CompanyRepository objRepo = new CompanyRepository();
            List<INC_BasedStation> objBaseStationList = new List<INC_BasedStation>();
            objBaseStationList = objRepo.GetAllBaseStationResult();
            Common.BindDDL(ddlBaseStation, objBaseStationList, "sBaseStation", "iBaseStationId", "-Select BaseSation-");
            //get Vendor List
            List<GetEquipVendorDetailResult> objGetEquipVendorDetailResult = new List<GetEquipVendorDetailResult>();
            objGetEquipVendorDetailResult = objAssetVendorRepository.GetVendorDetailBySP(this.CompanyID);
            Common.BindDDL(ddlVendor, objGetEquipVendorDetailResult, "EquipmentVendorName", "EquipmentVendorID", "-Select Vendor-");
           
           
        }
        catch (Exception)
        {


        }
    }
  
    private void ResetControls()
    {
        dvInsuranceHide.Visible = false;
        dvMaintenanceCostHide.Visible = false;
        dvRegistration.Visible = false;
        dvWeeklyMaintenanceHide.Visible = false;
        dvEquipmentImageHide.Visible = false;
        dvFieldPartSpecification.Visible = false;
        dvPartsHistory.Visible = false;
        dvRepairOrder.Visible = false;
        //dvInsuranceRecords.Visible = false;
        //dvInsurance.Visible = false;
        //dvInspectionHeader.Visible = false;
        //dvInspection.Visible = false;
        //dvManageInsurance.Visible = false;
    }
  
    private void ColorChange()
    {
        AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
        EquipmentMaster objEquipmentMaster = new EquipmentMaster();
        try
        {

       
        objEquipmentMaster = objAssetMgtRepository.GetById(EquipmentMasterId);
        DateTime ExpireOn = Convert.ToDateTime(objEquipmentMaster.ExpireOn, System.Globalization.CultureInfo.CurrentCulture);
        TimeSpan DateDiff = Convert.ToDateTime(objEquipmentMaster.ExpireOn).Subtract(DateTime.Now);

        //Flagged Start
        if (objEquipmentMaster.Flagged == false)
        {
            lnkFlagAsset.ForeColor = System.Drawing.Color.Empty;
        }
        else
        {
            lnkFlagAsset.ForeColor = System.Drawing.Color.Red;
        }
        //Flagged End
            //Insurance Start
        if (objEquipmentMaster.ExpireOn != null)
        {
            if (DateDiff.Days < 0)
            {
                liInsurance.Attributes["Class"] = "red_check";
                //NotesOnColorChange(Convert.ToString(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.InsuranceRecords),true);
            }
            else
            {
                if (DateDiff.Days <= 30)
                {
                    liInsurance.Attributes["Class"] = "yellow_check";
                    //NotesOnColorChange(Convert.ToString(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.InsuranceRecords),false);
                }
                else
                {
                    liInsurance.Attributes["Class"] = "green_check";
                }
            }
        }
        else
        {
            liInsurance.Attributes["Class"] = "grey_check";
        }
            //Insurance End
            //Registration Start
        DateDiff = Convert.ToDateTime(objEquipmentMaster.RegistrationExpiryDate).Subtract(DateTime.Now);
        if (objEquipmentMaster.RegistrationExpiryDate != null)
        {
            if (DateDiff.Days < 0)
            {
                liRegistration.Attributes["Class"] = "red_check";
                //NotesOnColorChange(Convert.ToString(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.Registration),true);
            }
            else
            {
                if (DateDiff.Days <= 30)
                {
                    liRegistration.Attributes["Class"] = "yellow_check";
                   // NotesOnColorChange(Convert.ToString(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.Registration),false);
                }
                else
                {
                    liRegistration.Attributes["Class"] = "green_check";
                }
            }
        }
        else
        {
            liRegistration.Attributes["Class"] = "grey_check";
        }
            //Registration End
            //Asset Status Start
        if (objEquipmentMaster.Status != 0)//If Status is Active than green else red
        {
            if (objEquipmentMaster.Status == 1710) //Active-1710(live)  1144(local)    
            {
                liAssetStatus.Attributes["Class"] = "green_check";
            }
            else
            {
                liAssetStatus.Attributes["Class"] = "red_check"; 
            }
        }
        else
        {
            liAssetStatus.Attributes["Class"] = "grey_check";
        }
            //Asset Status End

        }
        catch (Exception)
        {
        }
    }
    protected void DownloadFile(string filepath)
    {
        System.IO.Stream iStream = null;
        string type = "";
        string ext = Path.GetExtension(filepath);


        // Buffer to read 10K bytes in chunk:
        byte[] buffer = new Byte[10000];

        // Length of the file:
        int length;

        // Total bytes to read:
        long dataToRead;

        // Identify the file name.
        string filename = System.IO.Path.GetFileName(filepath);

        try
        {
            // Open the file.
            iStream = new System.IO.FileStream(filepath, System.IO.FileMode.Open,
            System.IO.FileAccess.Read, System.IO.FileShare.Read);


            // Total bytes to read:
            dataToRead = iStream.Length;
            if (ext != null)
            {
                switch (ext.ToLower())
                {
                    case ".htm":
                    case ".html":
                        type = "text/HTML";
                        break;
                    case ".txt":
                        type = "text/plain";
                        break;
                    case ".pdf":
                        type = "Application/pdf";
                        break;
                    case ".mp3":
                        type = "audio/mpeg";
                        break;
                    case ".wmi":
                        type = "audio/basic";
                        break;
                    case ".dat":
                    case ".mpeg":
                    case "mpg":
                        type = "video/mpeg";
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".gif":
                    case ".tiff":
                        type = "image/gif";
                        break;
                    case ".png":
                        type = "image/png";
                        break;
                    case ".doc":
                    case ".rtf":
                        type = "Application/msword";
                        break;
                    case ".wav":
                        type = "audio/x-wav";
                        break;
                    case "bmp":
                        type = "image/bmp";
                        break;
                    case "flv":
                        type = "video/x-flv";
                        break;
                    case ".docx":
                        type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case "ppt":
                        type = "application/vnd.ms-powerpoint";
                        break;
                    case "pptx":
                        type = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        break;
                    case "avi":
                        type = "video/x-msvideo";
                        break;
                    case "wmv":
                        type = "video/x-ms-wmv";
                        break;
                    case "wma":
                        type = "/audio/x-ms-wma";
                        break;

                    //left:--rm
                }
            }


            //Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + displayFileName + "\"");
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.ContentType = type;
            Response.WriteFile(filepath);
            Response.End();


        }
        catch (Exception ex)
        {
          
        }
        finally
        {
            if (iStream != null)
            {
                //Close the file.
                iStream.Close();
            }
        }
    }

    public void DisplayNotes()
    {
        try
        {
            txtNotesHistory.Text = "";
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            //Check for vendor employee
            List<NoteDetail> objList = objRepo.GetByForeignKeyId(Convert.ToInt64(EquipmentMasterId), Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement);
            if (Convert.ToString(Session["Usr"]) != Convert.ToString(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                foreach (NoteDetail obj in objList)
                {
                    if (obj.Notecontents == "Insurance Records")
                    {
                        txtNotesHistory.Text += "Edited : " + obj.Notecontents + "\n";
                        txtNotesHistory.Text += "Edited Date : " + Convert.ToDateTime(obj.CreateDate).ToString("dd-MMM-yyyy") + "\n";
                        txtNotesHistory.Text += "Edited Time : " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n";
                        UserInformationRepository objUserRepo = new UserInformationRepository();
                        UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);

                        if (objUser != null)
                        {
                            txtNotesHistory.Text += "Edited By : " + objUser.FirstName + " " + objUser.LastName + "\n";
                        }
                        txtNotesHistory.Text += "---------------------------------------------";
                        txtNotesHistory.Text += "\n";
                    }
                    else
                    {
                        txtNotesHistory.Text += "Note : " + obj.Notecontents + "\n";
                        txtNotesHistory.Text += "Created Date : " + Convert.ToDateTime(obj.CreateDate).ToString("dd-MMM-yyyy") + "\n";
                        txtNotesHistory.Text += "Created Time : " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n";
                        UserInformationRepository objUserRepo = new UserInformationRepository();
                        UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);

                        if (objUser != null)
                        {
                            txtNotesHistory.Text += "Created By : " + objUser.FirstName + " " + objUser.LastName + "\n";
                        }
                        if (obj.ReceivedBy != null)
                        {
                            txtNotesHistory.Text += "Received By : " + obj.ReceivedBy + "\n";
                        }
                        txtNotesHistory.Text += "---------------------------------------------";
                        txtNotesHistory.Text += "\n";

                    }

                }
            }
            else
            {
               
                    foreach (NoteDetail obj in objList)
                    {
                        if ( obj.SpecificNoteFor!=null)
                        {                           
                       
                        if (objAssetVendorRepository.CheckEmailAuthentication(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID),Convert.ToInt64( obj.SpecificNoteFor)) == true)
                        {
                        txtNotesHistory.Text += "Note : " + obj.Notecontents + "\n";
                        txtNotesHistory.Text += "Created Date : " + Convert.ToDateTime(obj.CreateDate).ToString("dd-MMM-yyyy") + "\n";
                        txtNotesHistory.Text += "Created Time : " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n";
                        UserInformationRepository objUserRepo = new UserInformationRepository();
                        UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);

                        if (objUser != null)
                        {
                            txtNotesHistory.Text += "Created By : " + objUser.FirstName + " " + objUser.LastName + "\n";
                        }
                        if (obj.ReceivedBy != null)
                        {
                            txtNotesHistory.Text += "Received By : " + obj.ReceivedBy + "\n";
                        }
                        txtNotesHistory.Text += "---------------------------------------------";
                        txtNotesHistory.Text += "\n";
                        }

                    }
                }
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public void DisplayCreatedNotes()
    {
        try
        {
            txtNotesHistory.Text = "";
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            List<NoteDetail> objList = objRepo.GetByForeignKeyId(Convert.ToInt64(EquipmentMasterId), Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement);
            foreach (NoteDetail obj in objList)
            {
                if (obj.Notecontents != "Insurance Records")
                {
                    txtNotesHistory.Text += "Note : " + obj.Notecontents + "\n";
                    txtNotesHistory.Text += "Created Date : " + Convert.ToDateTime(obj.CreateDate).ToString("dd-MMM-yyyy") + "\n";
                    txtNotesHistory.Text += "Created Time : " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n";
                    UserInformationRepository objUserRepo = new UserInformationRepository();
                    UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);

                    if (objUser != null)
                    {
                        txtNotesHistory.Text += "Created By : " + objUser.FirstName + " " + objUser.LastName + "\n";
                    }
                    if (obj.ReceivedBy != null)
                    {
                        txtNotesHistory.Text += "Received By : " + obj.ReceivedBy + "\n";
                    }
                    txtNotesHistory.Text += "---------------------------------------------";
                    txtNotesHistory.Text += "\n";
                }           


            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public void EditNotes(string NoteContent)
    {
        try
        {
            //string strNoteHistory = "";


            //NoteHistory for Supplier ORder
            string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement);

            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();


            objComNot.Notecontents = NoteContent;
            objComNot.NoteFor = strNoteFor;
            objComNot.ForeignKey = Convert.ToInt64(EquipmentMasterId);
            // objComNot.SpecificNoteFor = Convert.ToString(this.OrderID);
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objComNot.UpdateDate = System.DateTime.Now;
            objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
           // strNoteHistory = txtOrderNotesHistory.Text;
            objCompNoteHistRepos.Insert(objComNot);
            objCompNoteHistRepos.SubmitChanges();
            int NoteId = (int)(objComNot.NoteID);
            txtOrderNotesHistory.Text = "";
            DisplayNotes();

            //SendEmailNotes(NoteContent, EmailFor);

        }
        catch (Exception)
        {

        }
    }

    public void NotesOnColorChange(string NoteFor,bool IsExpired)
    {
        try
        {
            string NoteContent = string.Empty;
            Int64 SpecificNoteFor=0;
            string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement);

            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();
            if (NoteFor==Convert.ToString(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.InsuranceRecords))
            {
                SpecificNoteFor = Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.InsuranceRecords);
                if (IsExpired)
                {
                     objComNot.Notecontents = "Your Insurance on asset ("+ Convert.ToString(this.EquipmentID) +") is about to expire in 30 days.";
                }
                else
                {
                    objComNot.Notecontents = "Your Insurance on asset (" + Convert.ToString(this.EquipmentID) + ") has expired.";
                }
            }
            else
            {
                SpecificNoteFor = Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.Registration);
                if (IsExpired)
                {
                    objComNot.Notecontents = "Your Registration on asset (" + Convert.ToString(this.EquipmentID) + ") is about to expire in 30 days.";
                }
                else
                {
                    objComNot.Notecontents = "Your Registration on asset (" + Convert.ToString(this.EquipmentID) + ") has expired.";
                }
            }
            NoteContent = objComNot.Notecontents;
            objComNot.NoteFor = strNoteFor;
            objComNot.ForeignKey = Convert.ToInt64(EquipmentMasterId);
            objComNot.SpecificNoteFor = Convert.ToString(SpecificNoteFor);
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objComNot.UpdateDate = System.DateTime.Now;
            objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            // strNoteHistory = txtOrderNotesHistory.Text;
            objCompNoteHistRepos.Insert(objComNot);
            objCompNoteHistRepos.SubmitChanges();
            int NoteId = (int)(objComNot.NoteID);           
            DisplayNotes();
            SendEmailOnColorChange(NoteContent,SpecificNoteFor);

        }
        catch (Exception)
        {

        }
    }

    public void bindPostInvoicegrid()
    {
        DataView myDataView = new DataView();
        DataTable dt = new DataTable();
        try
        {
            bool IsAuthentic = objAssetVendorRepository.IsAuthentic(IncentexGlobal.CurrentMember.UserInfoID);
            if (IsAuthentic)
            {

                dt = ListToDataTable(objAssetMgtRepository.GetEquipMaintenanceCostById(Convert.ToInt64(EquipmentMasterId)));
                if (dt.Rows.Count == 0)
                {
                    pagingtable.Visible = false;

                }
                else
                {
                    pagingtable.Visible = true;

                }
                //gvEquipment.DataSource = dt;
                //gvEquipment.DataBind();

                myDataView = dt.DefaultView;
                if (this.ViewState["SortExp"] != null)
                {
                    myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
                }
                pds.DataSource = myDataView;
                pds.AllowPaging = true;

                //pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
                pds.PageSize = 25;   //Page Count 25           
                pds.CurrentPageIndex = CurrentPage;
                lnkbtnNext.Enabled = !pds.IsLastPage;
                lnkbtnPrevious.Enabled = !pds.IsFirstPage;

                gvEquipment.DataSource = pds;
                gvEquipment.DataBind();
                doPaging();
            }
            else
                pagingtable.Visible = false;
        }
        catch (Exception)
        {

        }
    }

    public void bindPostEventgrid()
    {
        DataView myDataView = new DataView();
        DataTable dt = new DataTable();
        try
        {
            dt = ListToDataTable(objAssetMgtRepository.GetEquipWeeklyMaintenance(Convert.ToInt64(EquipmentMasterId)));           
            myDataView = dt.DefaultView;
            if (this.ViewState["SortExp"] != null)
            {
                myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
            }
            pds.DataSource = myDataView;           

            gvPostEvent.DataSource = pds;
            gvPostEvent.DataBind();           
        }
        catch (Exception)
        {

        }
    }
    public void ResetPopup()
    {
        //-----Post Invoice Start
        ddlVendor.SelectedIndex = 0;
        ddlVendor.Enabled = true;
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
        {
            EquipmentVendorEmployee objEquipmentVendorEmployee = new EquipmentVendorEmployee();
            objEquipmentVendorEmployee = objAssetVendorRepository.GetVendorEmpByUserInfoID(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
            string VendorId = Convert.ToString(objEquipmentVendorEmployee.VendorID);
            ddlVendor.SelectedValue = VendorId;
            ddlVendor.Enabled = false;
        }
        txtDateofService.Text = "";
        txtInvoice.Text = "";
        txtDescription.Text = "";
        txtAmount.Text = "";
        txtPartsAmount.Text = "";
        txtLaborAmount.Text="";
        ddlJobCode.SelectedIndex = 0;
        //foreach (ListItem item in cblJobSubCode.Items)
        //{
        //        item.Selected = false;           
        //}
        BindChecklist(Convert.ToInt64(ddlJobCode.SelectedValue));
        //-----Post Invoice End

        //-----Post Event Start
        txtWeekOfDate.Text = "";
        txtEventHours.Text = "";
        txtOil.Text = "";
        txtAntiFreeze.Text = "";
        rdbServiceBrakeT.Checked = true;
        rdbParkBrakeT.Text = "";
        rdbLightsT.Text = "";
        rdbATFT.Text = "";
        rdbTiresT.Text = "";
        //-----Post Event End
    }

    /// <summary>
    /// Get Splash Images by specific workgroup
    /// </summary>
    public void getImages()
    {

        try
        {
            objEquipmentImageList = objAssetMgtRepository.GetEquipmentImagesById(this.EquipmentMasterId,rdbAssetImage.Checked);
            dtSplash.DataSource = objEquipmentImageList;
            dtSplash.DataBind();
        }
        catch (Exception)
        {
        }
       
    }
    public void getFields()
    {
        try
        {
        DataTable dt = new DataTable();
        dt = ListToDataTable(objFieldRepository.GetFieldsForAssetProfile(this.FieldCompany, this.EquipmentTypeID));
        dtlField.DataSource = dt;
        dtlField.DataBind();
        }
        catch (Exception)
        {
            
        }
    }
    public void getFieldDesc()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ListToDataTable(objFieldRepository.GetFieldDetailDesc(this.FieldCompany, this.EquipmentMasterId));
            Int64 dtlFieldMasterID = 0;
            Int64 dtFieldMasterID = 0;

            foreach (DataRow dr in dt.Rows)
            {
                dtFieldMasterID = Convert.ToInt64(dr[2]);
                foreach (DataListItem item in dtlField.Items)
                {
                    HiddenField hdnFieldMasterID = (HiddenField)item.FindControl("hdnFieldMasterID");
                    dtlFieldMasterID = Convert.ToInt64(hdnFieldMasterID.Value);
                    if (dtFieldMasterID==dtlFieldMasterID)
                    {
                         TextBox txtFieldDesc = (TextBox)item.FindControl("txtFieldDesc");
                         txtFieldDesc.Text = Convert.ToString(dr[3]);
                    }
                }
            }
            
        }
        catch (Exception)
        {
            
           
        }
    }
    private void bindPartsHistoryGridView()
    {
        //Parts Ordered Grid
        List<AssetRepairMgtRepository.GetEquipmentBillingPartsOrderedInfo> objListPartsHistoryAdd = new List<AssetRepairMgtRepository.GetEquipmentBillingPartsOrderedInfo>();
        List<EquipmentRepairOrder> objRepair = objAssetMgtRepository.GetRepairOrderByID(this.EquipmentMasterId);
        foreach (EquipmentRepairOrder item in objRepair)
        {
            List<AssetRepairMgtRepository.GetEquipmentBillingPartsOrderedInfo> objListPartsHistory = objAssetRepairRepository.GetEquipmentBillingPartsOrderedDetails(item.RepairOrderID);
            if (objListPartsHistory.Count != 0)
            {
                objListPartsHistoryAdd.AddRange(objListPartsHistory);                               
            }
        }
        gvPartsHistory.DataSource = objListPartsHistoryAdd; 
        gvPartsHistory.DataBind();
    }
    private void bindRepairOrderGridView()
    {
        DataView myDataView = new DataView();

        DataTable dt = new DataTable();

        UserInformation objuser = new UserInformation();
        UserInformationRepository objuserrepos = new UserInformationRepository();
        try
        {

            dt = ListToDataTable(objAssetRepairRepository.GetRepairOrderByEquipmentMasterID(this.EquipmentMasterId));
           
            myDataView = dt.DefaultView;
            if (this.ViewState["SortExp"] != null)
            {
                myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
            }
            pds.DataSource = myDataView;  
            gvRepairOrder.DataSource = pds;
            gvRepairOrder.DataBind();
            doPaging();

        }
        catch (Exception)
        {


        }
    }
    private void GetRepairOrderSelection()
    {
        ddlShowMe.SelectedIndex = 9;
        dvInsuranceHide.Visible = false;
        dvMaintenanceCostHide.Visible = false;
        dvRegistration.Visible = false;
        dvWeeklyMaintenanceHide.Visible = false;
        dvEquipmentImageHide.Visible = false;
        dvFieldPartSpecification.Visible = false;
        dvPartsHistory.Visible = false;
        dvRepairOrder.Visible = true;
    }
    /// <summary>
    /// Send Mail
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    //public bool SendMailWithMulipAttachment(string psFromAddress, string psToAddress, string psSubject, string psMessageBody, string Fromname
    //   , bool SSL, bool HTMLBody, string Host, Int32 Port, string uName, string Pwd, bool IsForLive, string strFullPath, string sFileName)
    //{

    //    bool Result = false;
    //    try
    //    {
    //        System.Net.Mail.MailMessage oMailMessage = new System.Net.Mail.MailMessage();
    //        SmtpClient smtpmail = new SmtpClient();
    //        if (!IsForLive)
    //        {
    //            NetworkCredential objNet = new NetworkCredential(uName, Pwd);
    //            smtpmail.Credentials = objNet;
    //        }
    //        smtpmail.Host = Host;
    //        smtpmail.Port = Port;

          
    //        MailAddress FromAddress = default(MailAddress);
    //        Attachment objAttach = new Attachment(strFullPath);
    //                System.Net.Mime.ContentDisposition obj = objAttach.ContentDisposition;
    //                obj.FileName = sFileName;
    //                oMailMessage.Attachments.Add(objAttach);             


    //        if (string.IsNullOrEmpty(Fromname))
    //        {
    //            FromAddress = new MailAddress(psFromAddress);
    //        }
    //        else
    //        {
    //            FromAddress = new MailAddress(psFromAddress, Fromname);
    //        }


    //        oMailMessage.To.Add(psToAddress);

    //        oMailMessage.From = FromAddress;
    //        oMailMessage.Subject = psSubject;
    //        oMailMessage.IsBodyHtml = HTMLBody;
    //        oMailMessage.Body = psMessageBody;
    //        oMailMessage.Priority = System.Net.Mail.MailPriority.High;

    //        smtpmail.EnableSsl = SSL;

    //        smtpmail.Send(oMailMessage);

    //        Result = true;

    //    }
    //    catch (Exception ex)
    //    {

    //        ex = null;

    //        Result = false;
    //    }


    //    return Result;
    //}

    public void BindChecklist(Int64 JobCode)
    {
        DataTable dt = new DataTable();
        dt = ListToDataTable(objAssetMgtRepository.GetAllSubJobCodeDetail(JobCode));
      
            cblJobSubCode.DataSource = dt;
            cblJobSubCode.DataBind();
       
    }
    //Email For - Selected Index of Show Me Dropdown
    private void SendEmailNotes(string strNoteHistory,Int64 EmailFor)
    {
        if (EmailFor > 0)
        {

            String eMailTemplate = String.Empty;
            String sSubject = " GSE Asset Mgt - " + this.EquipmentID;

            // String sReplyToadd = Common.ReplyToGSE;
            String sReplyToadd = "replytoGSE@world-link.us.com";

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/GSEAssetMgtNote.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();


            List<GetGSEUsersResult> lstRecipients = new List<GetGSEUsersResult>();
            lstRecipients = new AssetMgtRepository().GetGSEUsers().ToList();
            foreach (GetGSEUsersResult recipient in lstRecipients)
            {
                //Email Management
                if (objAssetVendorRepository.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), EmailFor) == true)
                //if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                {
                    String UserName = Convert.ToString(recipient.LastName) + " " + Convert.ToString(recipient.FirstName);
                    StringBuilder MessageBody = new StringBuilder(eMailTemplate);
                    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                    MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
                    MessageBody.Replace("{EquipmentID}", Convert.ToString(this.EquipmentID));
                    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                    MessageBody.Replace("{Note}", "<br/>" + strNoteHistory);
                    MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);

                    //using (MailMessage objEmail = new MailMessage())
                    //{
                    //    objEmail.Body = MessageBody.ToString();
                    //    objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                    //    objEmail.IsBodyHtml = true;
                    //    //objEmail.ReplyTo = new MailAddress(sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(this.EquipmentMasterId) + "un" + Convert.ToString(recipient.UserInfoID) + "nt1en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@')));
                    //    objEmail.ReplyTo = new MailAddress(sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(this.EquipmentMasterId) + "un" + Convert.ToString(recipient.UserInfoID) + "nt" + Convert.ToString(EmailFor) + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@')));
                    //    objEmail.Subject = sSubject;
                    //    objEmail.To.Add(new MailAddress(Convert.ToString(recipient.LoginEmail)));

                    //    SmtpClient objSmtp = new SmtpClient();

                    //    objSmtp.EnableSsl = Common.SSL;
                    //    objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
                    //    objSmtp.Host = Common.SMTPHost;
                    //    objSmtp.Port = Common.SMTPPort;

                    //    objSmtp.Send(objEmail);
                    //}
                    String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(this.EquipmentMasterId) + "un" + Convert.ToString(recipient.UserInfoID) + "nt" + Convert.ToString(EmailFor) + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                    new CommonMails().SendMailWithReplyTo(Convert.ToInt64(recipient.UserInfoID), "GSE Asset Mgt", Common.EmailFrom, Convert.ToString(recipient.LoginEmail), sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
                }
            }
        }
    }
    //Email on Color Change Insurance & Registration

    private void SendEmailOnColorChange(string strNoteHistory, Int64 EmailFor)
    {       

            String eMailTemplate = String.Empty;
            String sSubject = string.Empty;
            if (EmailFor==Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.InsuranceRecords))
            {
                 sSubject = " Insurance Alert - " + this.EquipmentID;
            }
            else
            {
                 sSubject = " Registration Alert - " + this.EquipmentID;
            }
           

            // String sReplyToadd = Common.ReplyToGSE;
            String sReplyToadd = "replytoGSE@world-link.us.com";

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/GSEAssetMgtNote.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();


            List<GetGSEUsersResult> lstRecipients = new List<GetGSEUsersResult>();
            lstRecipients = new AssetMgtRepository().GetGSEUsers().ToList();
            foreach (GetGSEUsersResult recipient in lstRecipients)
            {
                //Email Management
                if (objAssetVendorRepository.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), EmailFor) == true)
                //if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                {
                    String UserName = Convert.ToString(recipient.LastName) + " " + Convert.ToString(recipient.FirstName);
                    StringBuilder MessageBody = new StringBuilder(eMailTemplate);
                    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                    MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
                    MessageBody.Replace("{EquipmentID}", Convert.ToString(this.EquipmentID));
                    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                    MessageBody.Replace("{Note}", "<br/>" + strNoteHistory);
                    MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);

                    //using (MailMessage objEmail = new MailMessage())
                    //{
                    //    objEmail.Body = MessageBody.ToString();
                    //    objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                    //    objEmail.IsBodyHtml = true;
                    //    //objEmail.ReplyTo = new MailAddress(sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(this.EquipmentMasterId) + "un" + Convert.ToString(recipient.UserInfoID) + "nt1en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@')));
                    //    objEmail.ReplyTo = new MailAddress(sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(this.EquipmentMasterId) + "un" + Convert.ToString(recipient.UserInfoID) + "nt" + Convert.ToString(EmailFor) + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@')));
                    //    objEmail.Subject = sSubject;
                    //    objEmail.To.Add(new MailAddress(Convert.ToString(recipient.LoginEmail)));

                    //    SmtpClient objSmtp = new SmtpClient();

                    //    objSmtp.EnableSsl = Common.SSL;
                    //    objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
                    //    objSmtp.Host = Common.SMTPHost;
                    //    objSmtp.Port = Common.SMTPPort;

                    //    objSmtp.Send(objEmail);
                    //}
                    String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(this.EquipmentMasterId) + "un" + Convert.ToString(recipient.UserInfoID) + "nt" + Convert.ToString(EmailFor) + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                    new CommonMails().SendMailWithReplyTo(Convert.ToInt64(recipient.UserInfoID), "GSE Asset Mgt", Common.EmailFrom, Convert.ToString(recipient.LoginEmail), sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
                }
            }
       
    }
    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindPostInvoicegrid();
        }


    }
    protected void dtlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
    public int CurrentPage
    {
        get
        {
            if (this.ViewState["CurrentPage"] == null)
                return 0;
            else
                return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }

    public int FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt16(this.ViewState["FrmPg"].ToString());
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }
    public int ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return 5;
            else
                return Convert.ToInt16(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }
    private void doPaging()
    {
        try
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            int CurrentPg = pds.CurrentPageIndex + 1;

            if (CurrentPg > ToPg)
            {
                FrmPg = ToPg + 1;
                ToPg = ToPg + 5;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - 5;

            }

            if (pds.PageCount < ToPg)
                ToPg = pds.PageCount;
          
            for (int i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();

        }
        catch (Exception)
        { }

    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        bindPostInvoicegrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        bindPostInvoicegrid();
    }
    #endregion
    #endregion 
}