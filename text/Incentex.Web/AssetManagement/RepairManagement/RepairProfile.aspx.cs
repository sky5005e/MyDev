using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Configuration;
using System.Data;
using System.Collections;
using commonlib.Common;
public partial class AssetManagement_RepairManagement_RepairProfile : PageBase
{
    #region DataMembers

    UserInformation objuser = new UserInformation();
    UserInformationRepository objuserrepos = new UserInformationRepository();
    EquipmentRepairOrder objEquipment = new EquipmentRepairOrder();
    EquipmentImage objEquipmentImage = new EquipmentImage();
    List<EquipmentImage> objEquipmentImageList = new List<EquipmentImage>();
    AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
    AssetRepairMgtRepository objAssetRepairRepository = new AssetRepairMgtRepository();
    AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
    AssetFieldRepository objFieldRepository = new AssetFieldRepository();
    EquipmentMaintenanceCostDetail objEquipMaintenanceCostDtl = new EquipmentMaintenanceCostDetail();
    Int64 CompanyID = 0;
    Common objcommon = new Common();
    PagedDataSource pds = new PagedDataSource();
    DataSet ds = new DataSet();
    String Page = null;
    Int64 RepairOrderID
    {
        get
        {
            if (ViewState["RepairOrderID"] == null)
            {
                ViewState["RepairOrderID"] = 0;
            }
            return Convert.ToInt64(ViewState["RepairOrderID"]);
        }
        set
        {
            ViewState["RepairOrderID"] = value;
        }
    }
    Int64 EquipmentMasterID
    {
        get
        {
            if (ViewState["EquipmentMasterID"] == null)
            {
                ViewState["EquipmentMasterID"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentMasterID"]);
        }
        set
        {
            ViewState["EquipmentMasterID"] = value;
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
    Double TotalHours
    {
        get
        {
            if (ViewState["TotolHours"] == null)
            {
                ViewState["TotolHours"] = 0;
            }
            return Convert.ToDouble(ViewState["TotolHours"]);
        }
        set
        {
            ViewState["TotolHours"] = value;
        }
    }
    String IsBillingComplete
    {
        get
        {
            if (ViewState["IsBillingComplete"] == null)
            {
                ViewState["IsBillingComplete"] = "false";
            }
            return ViewState["IsBillingComplete"].ToString();
        }
        set
        {
            ViewState["IsBillingComplete"] = value;
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
    Decimal totalAmount = 0M;
    #endregion

    #region Events
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

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Repair Order Profile";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";

                

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {

                    CompanyID = Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId);
                }
                else
                {
                    CompanyID = 0;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["RepairOrderId"]))
                {
                    RepairOrderID = Convert.ToInt64(Request.QueryString["RepairOrderId"]);
                    EquipmentMasterID = Convert.ToInt64(objAssetRepairRepository.GetEquipMasterIDFrmRepairOrderID(RepairOrderID));
                    Page = Convert.ToString(Request.QueryString["Page"]);
                    IsBillingComplete = Convert.ToString(Request.QueryString["IsBillingComplete"]);
                }
                if (Page == "AssetProfile")
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/AssetProfile.aspx?Page=RepairProfile&Id=" + EquipmentMasterID;
                else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/RepairManagement/ViewOpenRepairOrders.aspx?IsBillingComplete=" + IsBillingComplete;
                bindGridview();
                bindDropDowns();
                DisplayData();
                //bindAssetPartGrid();
                //BindPopupValues();
                //DisplayPopupData();
                ResetControls();
               
                if (Page == "CheckOnSiteInventory")
                {
                    hdnExpand.Value = "OP";
                    ddlShowMe.SelectedIndex = 6;
                    dvRepairReason.Visible = false;
                    dvPartsAsset.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvNotesHistory.Visible = false;
                    dvShowRepairHours.Visible = false;
                    dvOrderParts.Visible = true;
                    dvCloseBilling.Visible = false;
                    dvCustInfo.Visible = false;
                    dvPartsHistory.Visible = false;
                }
            }

            //---Hide Add Note Button According to Vendor Employee Email Rights
            lnkAddHisotry.Visible = true;
            if (Convert.ToString(Session["Usr"]) == Convert.ToString(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                if (objAssetVendorRepository.CheckEmailAuthentication(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), Convert.ToInt64(ddlShowMe.SelectedValue)) != true)
                {
                    lnkAddHisotry.Visible = false;
                }
                else
                {
                    lnkAddHisotry.Visible = true;
                }
            }


            DisplayNotes();
            DisplayNotifications();
            getImages();
           
        }
        catch (Exception)
        {


        }
    }


    //--Main Section
    protected void ddlShowMe_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


            if (ddlShowMe.SelectedValue != "0")// for test
            {

                if (ddlShowMe.SelectedValue == "1")
                {
                    dvPartsHistory.Visible = true;
                    dvEquipmentImageHide.Visible = false;
                    dvOrderParts.Visible = false;                    
                    dvNotesHistory.Visible = false;
                    dvShowRepairHours.Visible = false;
                    dvCustInfo.Visible = false;
                    dvPartsAsset.Visible = false;
                    dvRepairReason.Visible = false;
                    dvCloseBilling.Visible = false;                    
                    
                }
                else if (ddlShowMe.SelectedValue == "2")// For Asset & Parts Specification
                {
                    dvPartsHistory.Visible = false;
                    dvEquipmentImageHide.Visible = true;
                    dvOrderParts.Visible = false;
                    dvNotesHistory.Visible = false;
                    dvShowRepairHours.Visible = false;
                    dvCustInfo.Visible = false;
                    dvPartsAsset.Visible = false;
                    dvRepairReason.Visible = false;
                    dvCloseBilling.Visible = false;
                }
                else if (ddlShowMe.SelectedValue == "3")// For Equipment Images
                {
                    dvPartsHistory.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvOrderParts.Visible = true;
                    dvNotesHistory.Visible = false;
                    dvShowRepairHours.Visible = false;
                    dvCustInfo.Visible = false;
                    dvPartsAsset.Visible = false;
                    dvRepairReason.Visible = false;
                    dvCloseBilling.Visible = false; 
                }
                else if (ddlShowMe.SelectedValue == "4")
                {
                    dvPartsHistory.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvOrderParts.Visible = false;
                    dvNotesHistory.Visible = true;
                    dvShowRepairHours.Visible = false;
                    dvCustInfo.Visible = false;
                    dvPartsAsset.Visible = false;
                    dvRepairReason.Visible = false;
                    dvCloseBilling.Visible = false;  
                }
                else if (ddlShowMe.SelectedValue == "5")
                {
                    dvPartsHistory.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvOrderParts.Visible = false;
                    dvNotesHistory.Visible = false;
                    dvShowRepairHours.Visible = true;
                    dvCustInfo.Visible = false;
                    dvPartsAsset.Visible = false;
                    dvRepairReason.Visible = false;
                    dvCloseBilling.Visible = false;  
                }
                else if (ddlShowMe.SelectedValue == "6")
                {
                    dvPartsHistory.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvOrderParts.Visible = false;
                    dvNotesHistory.Visible = false;
                    dvShowRepairHours.Visible = false;
                    dvCustInfo.Visible = true;
                    dvPartsAsset.Visible = false;
                    dvRepairReason.Visible = false;
                    dvCloseBilling.Visible = false;  
                }
                else if (ddlShowMe.SelectedValue == "7")
                {
                    dvPartsHistory.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvOrderParts.Visible = false;
                    dvNotesHistory.Visible = false;
                    dvShowRepairHours.Visible = false;
                    dvCustInfo.Visible = false;
                    dvPartsAsset.Visible = true;
                    dvRepairReason.Visible = false;
                    dvCloseBilling.Visible = false;  
                }
                else if (ddlShowMe.SelectedValue == "8")
                {
                    dvPartsHistory.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvOrderParts.Visible = false;
                    dvNotesHistory.Visible = false;
                    dvShowRepairHours.Visible = false;
                    dvCustInfo.Visible = false;
                    dvPartsAsset.Visible = false;
                    dvRepairReason.Visible = true;
                    dvCloseBilling.Visible = false;  
                }
                else if (ddlShowMe.SelectedValue == "9")
                {
                    dvPartsHistory.Visible = false;
                    dvEquipmentImageHide.Visible = false;
                    dvOrderParts.Visible = false;
                    dvNotesHistory.Visible = false;
                    dvShowRepairHours.Visible = false;
                    dvCustInfo.Visible = false;
                    dvPartsAsset.Visible = false;
                    dvRepairReason.Visible = false;
                    dvCloseBilling.Visible = true;  
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
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        objEquipment = objAssetRepairRepository.GetRepairOrderByID(this.RepairOrderID);
        if (objEquipment != null)
        {
            objEquipment.RepairStatusID = Convert.ToInt64(ddlRepairOrderStatus.SelectedValue);
            objEquipment.VendorRepairID = txtVendorRepairID.Text;
            objAssetRepairRepository.SubmitChanges();
        }
        DisplayData();
    }
    //protected void gvPostJobBilling_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    LblTotalHours.Text = "";
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        Label lblHoursWorked = (Label)e.Row.FindControl("lblHoursWorked");
    //        TotalHours += Convert.ToDouble(lblHoursWorked.Text);
    //        LblTotalHours.Text = Convert.ToString(TotalHours);
    //    }

    //}

    //--Equipment Images
    protected void btnSaveImage_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        string sFilePath = null;

        try
        {

            if (((float)Request.Files[0].ContentLength / 1048576) > 2)
            {
                lblErrorImage.Text = "The file you are uploading is more than 2MB.";
                ModalEquipmentImage.Show();
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
            objEquipmentImage.EquipmentMasterID = this.EquipmentMasterID;
            objEquipmentImage.CreatedDate = System.DateTime.Now;
            objEquipmentImage.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objAssetMgtRepository.Insert(objEquipmentImage);
            objAssetMgtRepository.SubmitChanges();
            getImages();
        }
        catch (Exception)
        {

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
    #region DataList Events
    protected void gvEquipment_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "del")
        {
            Int64 val = Convert.ToInt64(e.CommandArgument);
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            {
                objAssetMgtRepository.DeleteMaintinanceCost(val);
                lblMaintenance.Text = "Record deleted successfully";
                bindPostInvoicegrid();
            }
            else
            {
                lblMaintenance.Text = "Sorry You are not Authorized to Delete this Record";
            }
        }
        if (e.CommandName == "Vendor")
        {
           // ResetPopup();
            Int64 val = Convert.ToInt64(e.CommandArgument);
            EquipmentMaintenanceCostID = Convert.ToInt64(e.CommandArgument);
            objEquipMaintenanceCostDtl = objAssetMgtRepository.GetByEquipMaintenanceCostId(val);
            if (Convert.ToString(objEquipMaintenanceCostDtl.Vendor) != string.Empty)
            {
                ddlInvoiceVendor.SelectedValue = Convert.ToString(objEquipMaintenanceCostDtl.Vendor);
            }
            if (Convert.ToString(objEquipMaintenanceCostDtl.DateofService) != string.Empty)
            {
                txtDateofService.Text = Convert.ToString(Convert.ToDateTime(objEquipMaintenanceCostDtl.DateofService).ToString("MM/dd/yyyy"));
            }
            txtInvoice.Text = objEquipMaintenanceCostDtl.Invoice;
            txtDescription.Text = objEquipMaintenanceCostDtl.Description;
            txtAmount.Text = Convert.ToString(objEquipMaintenanceCostDtl.Amount);
            if (objEquipMaintenanceCostDtl.JobCode != null)
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

                string sFilePath = Server.MapPath("../../UploadedImages/EquipmentInvoice/");
                //DownloadFile(sFilePath, objEquipmentMaster.InsurancePolicy);

                string strFullPath = sFilePath + objEquipMaintenanceCostDtl.DocumentPath;
                DownloadFile(strFullPath);
            }
            else
            {
                lblMaintenance.Text = "No Document Found..";
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
                else if (FileExt == "xl" || FileExt == "xls")
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
    protected void ddlJobCode_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        lblMaintenance.Text = "";
      
        DataTable dt = new DataTable();
        try
        {

            if (ddlJobCode.SelectedIndex != 0)
            {
                dt = Common.ListToDataTable(objAssetMgtRepository.GetAllSubJobCodeDetail(Convert.ToInt64(ddlJobCode.SelectedValue)));
                if (dt.Rows.Count != 0)
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
            objAssetMgtRepository.DeleteEquipmentImage(Convert.ToInt64(e.CommandArgument));
            objcommon.DeleteImageFromFolder(((HiddenField)e.Item.FindControl("hdnImageFileName")).Value, Server.MapPath("../../UploadedImages/EquipmentImage/"));
        }
        getImages();

    }

    #endregion

    //--Service Repair Notes
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
            objComNot.ForeignKey = Convert.ToInt64(this.RepairOrderID);
            objComNot.SpecificNoteFor = "Service Repair Notes";
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

            }

            DisplayNotes();

        }

        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

        }
    }

    //--My Job Billing
    //My Repair Hours
    protected void lnkBtnSavePostHours_Click(object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        hdnExpand.Value = "RH";
        EquipmentBillingRepairHour objEquipBilling = new EquipmentBillingRepairHour();

        objEquipBilling.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
        objEquipBilling.CreatedDate = DateTime.Now;
        objEquipBilling.HoursWorked = Convert.ToDouble(txtHoursWorked.Text);
        objEquipBilling.SummaryWorkPerformed = txtSummaryWorkPerformed.Text;
        objEquipBilling.RepairOrderID = this.RepairOrderID;
        if (this.CompanyID != 0)
            objEquipBilling.CompanyID = this.CompanyID;
        else
            objEquipBilling.CompanyID = null;

        objAssetRepairRepository.Insert(objEquipBilling);
        objAssetRepairRepository.SubmitChanges();
        bindGridview();
        ResetPopupFields();
    }
    //Check on Site
    protected void lnkCheckOnSiteStock_Click(object sender, EventArgs e)
    {
        Response.Redirect("CheckOnSiteInventory.aspx?id=" + this.RepairOrderID);
    }
    //Add Parts
    protected void lnkAddPartOrderInfo_Click(object sender, EventArgs e)
    {
        hdnExpand.Value = "OP";
        EquipmentBillingPartsOrdered objBillingParts = new EquipmentBillingPartsOrdered();

        objBillingParts.RepairOrderID = this.RepairOrderID;
        objBillingParts.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
        objBillingParts.CreatedDate = DateTime.Now;
        objBillingParts.PartNumber = txtPartNumber.Text;
        objBillingParts.SummaryDescriptions = txtPartDescription.Text;
        if (string.IsNullOrEmpty(txtOrderQuantity.Text)==false)
        {
            objBillingParts.Quantity = Convert.ToInt64(txtOrderQuantity.Text); 
        }       
        if (ddlVendor.SelectedValue!="0")
        {
            objBillingParts.VendorID = Convert.ToInt64(ddlVendor.SelectedValue);
        }        
        objAssetRepairRepository.Insert(objBillingParts);
        objAssetRepairRepository.SubmitChanges();

        // display reocrds
        bindGridview();
        ResetPopupFields();
    }
    //Close Billing
    protected void lnkBtnCompleteBilling_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            hdnExpand.Value = "CB";
            //Update IsBillingComplete and Repair Status
            EquipmentRepairOrder objRepairOrder = objAssetRepairRepository.GetRepairOrderByID(this.RepairOrderID);
            objRepairOrder.IsBillingComplete = true;
            objRepairOrder.RepairStatusID = objAssetRepairRepository.GetRepairOrderStatus("Closed");
            objRepairOrder.BillingFinishedOn = DateTime.Now;
            objAssetRepairRepository.SubmitChanges();            
            
            Response.Redirect("ViewOpenRepairOrders.aspx");
        }
        catch (Exception)
        { }
    }

    protected void lnkReadyForBilling_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            MailNotifyAccountingService();
            EquipmentRepairOrder objReadyForBilling = objAssetRepairRepository.GetRepairOrderByID(this.RepairOrderID);
            objReadyForBilling.ReadyForBillingOn = DateTime.Now;
            objAssetRepairRepository.SubmitChanges();
            lblMsg.Text = "Mail sent";
            DisplayNotifications();
        }
        catch (Exception)
        { }
    }

    protected void lnkAssetBack_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            //Mail Notification to those vendor employees who have same base station as that Asset.
            MailNotifyAssetBack();
            //Update Asset Status to Active
            Int64 EquipStatus = objAssetRepairRepository.GetEquipmentStatus();
            objAssetRepairRepository.UpdateEquipmentStatus(EquipmentMasterID, EquipStatus);
            //Notification Textbox
            EquipmentRepairOrder objAssetBack = objAssetRepairRepository.GetRepairOrderByID(this.RepairOrderID);
            objAssetBack.NotifyAssetBackOn = DateTime.Now;
            objAssetRepairRepository.SubmitChanges();
            lblMsg.Text = "Mail sent";
            DisplayNotifications();
        }
        catch (Exception)
        { }
    }
    //--Maintenance Related Cost
    protected void lnkSaveInvoice_Click(object sender, EventArgs e)
    {
        string sFilePath = null;
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            if (EquipmentMaintenanceCostID != 0)
            {
                objEquipMaintenanceCostDtl = objAssetMgtRepository.GetByEquipMaintenanceCostId(this.EquipmentMaintenanceCostID);
            }

            objEquipMaintenanceCostDtl.EquipmentMasterID = this.EquipmentMasterID;
            if (ddlInvoiceVendor.SelectedValue != "0")
            {
                objEquipMaintenanceCostDtl.Vendor = Convert.ToInt64(ddlInvoiceVendor.SelectedValue);
            }
            if (txtDateofService.Text != "")
            {
                objEquipMaintenanceCostDtl.DateofService = Convert.ToDateTime(txtDateofService.Text);
            }
            objEquipMaintenanceCostDtl.Invoice = txtInvoice.Text.Trim();
            objEquipMaintenanceCostDtl.Description = txtDescription.Text;

            objEquipMaintenanceCostDtl.Amount = txtAmount.Text != "" ? Convert.ToDecimal(txtAmount.Text) : 0;

            objEquipMaintenanceCostDtl.JobCode = Convert.ToInt64(ddlJobCode.SelectedValue);
            objEquipMaintenanceCostDtl.IsRepaired = true;
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
                lblMaintenance.Text = "The file you are uploading is more than 2MB.";
                ModalPostInvoice.Show();
                return;
            }

            if (InvoiceDoc.Value != "")
            {
                objEquipMaintenanceCostDtl.DocumentPath = InvoiceDoc.Value;
                sFilePath = Server.MapPath("../../UploadedImages/EquipmentInvoice/") + InvoiceDoc.Value;
                objcommon.DeleteImageFromFolder(InvoiceDoc.Value, Server.MapPath("../../UploadedImages/EquipmentInvoice/"));
                Request.Files[0].SaveAs(sFilePath);
            }
            if (EquipmentMaintenanceCostID == 0)
            {
                objAssetMgtRepository.Insert(objEquipMaintenanceCostDtl);

            }

            objAssetMgtRepository.SubmitChanges();
            bindPostInvoicegrid();
            //EditNotes("Maintenance Related Cost");
            EquipmentMaintenanceCostID = 0;
            //ResetPopup();
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

        //ResetPopup();
        ModalPostInvoice.Show();
    }

    private void MailNotifyAssetBack()
    {
        try
        {
            List<EquipmentVendorEmployee> lstRecipients = new List<EquipmentVendorEmployee>();
            lstRecipients = objAssetRepairRepository.GetVendorEmployeeByBaseStation(this.EquipmentMasterID);
            foreach (var user in lstRecipients)
            {
                if (objAssetVendorRepository.CheckEmailAuthentication(Convert.ToInt64(user.UserInfoID), 8) == true)// 8 For Repair Order Mails
                {

                    List<AssetRepairMgtRepository.RepairOrderList> objRepairOrderMail = new List<AssetRepairMgtRepository.RepairOrderList>();
                    objRepairOrderMail = objAssetRepairRepository.GetRepairOrderListBy(0, 0, this.RepairOrderID, 0, 0, "0", "0", null);

                    String subject = "Asset Back in Service - RN" + objRepairOrderMail[0].AutoRepairNumber;
                    String ReasonForRepair = Convert.ToString(objRepairOrderMail[0].RepairReason);
                    String ProblemDesc = Convert.ToString(objRepairOrderMail[0].ProblemDescription);

                    String MsgBody = "Asset of Repair #:RN" + objRepairOrderMail[0].AutoRepairNumber + " is back in service";                 

                    UserInformationRepository objUserInforepo = new UserInformationRepository();
                    UserInformation objUserInfo = objUserInforepo.GetById(user.UserInfoID);

                    String FullName = objUserInfo.FirstName + " " + objUserInfo.LastName;
                    sendVerificationEmail(objUserInfo.LoginEmail, FullName, subject, MsgBody, Convert.ToInt64(user.UserInfoID));
                }
            }
        }
        catch (Exception)
        { }
    }
    
    //private void MailNotifyVendorService()
    //{
    //    try
    //    {
    //         List<GetGSEUsersResult> lstRecipients = new List<GetGSEUsersResult>();
    //            lstRecipients = new AssetMgtRepository().GetGSEUsers().ToList();
    //            foreach (var user in lstRecipients)
    //            {
    //                if (objAssetVendorRepository.CheckEmailAuthentication(Convert.ToInt64(user.UserInfoID), 8) == true)// 8 For Repair Order Mails
    //                {

    //                    List<AssetRepairMgtRepository.RepairOrderList> objRepairOrderMail = new List<AssetRepairMgtRepository.RepairOrderList>();
    //                    objRepairOrderMail = objAssetRepairRepository.GetRepairOrderListBy(0, 0, this.RepairOrderID, 0, 0, "0", "0",null);

    //                    String subject = "Billing Summary - RN" + objRepairOrderMail[0].AutoRepairNumber;
    //                    String ReasonForRepair = Convert.ToString(objRepairOrderMail[0].RepairReason);
    //                    String ProblemDesc = Convert.ToString(objRepairOrderMail[0].ProblemDescription);

    //                    String MsgBody = "Repair #:RN" + objRepairOrderMail[0].AutoRepairNumber + "<br/>Reason for Repair: " + ReasonForRepair +
    //                                       "<br/> Discription of Issue:" + ProblemDesc;


    //                    //Repair Hours
    //                    List<AssetRepairMgtRepository.GetEquipmentBillingRepairHourInfo> objHrs = objAssetRepairRepository.GetEquipmentBillingRepairHourDetails(this.RepairOrderID);
    //                    if (objHrs.Count != 0)
    //                    {
    //                        string RepHrs = "<U>Hours(Date)</U><br/>";
    //                        foreach (AssetRepairMgtRepository.GetEquipmentBillingRepairHourInfo Hrs in objHrs)
    //                        {
    //                            RepHrs += Convert.ToString(Hrs.HoursWorked) + " (" + Convert.ToDateTime(Hrs.CreatedDate).ToString("MM/dd/yyyy") + ")<br/>";
    //                        }
    //                        MsgBody += "<br/> <B>List of Labor Hours:</B><br/>" + RepHrs;
    //                    }


    //                    //Parts Ordered
    //                    List<AssetRepairMgtRepository.GetEquipmentBillingPartsOrderedInfo> objPartOrder = objAssetRepairRepository.GetEquipmentBillingPartsOrderedDetails(this.RepairOrderID);
    //                    if (objPartOrder.Count != 0)
    //                    {
    //                        string RepParts = "<U>Part Number(Quantity)</U><br/>";
    //                        foreach (AssetRepairMgtRepository.GetEquipmentBillingPartsOrderedInfo PartOrder in objPartOrder)
    //                        {
    //                            RepParts += Convert.ToString(PartOrder.PartNumber) + " (" + Convert.ToString(PartOrder.Quantity) + ")<br/>";
    //                        }
    //                        MsgBody += "<br/> <B>List of Parts:</B><br/>" + RepParts;
    //                    }

    //                    String FullName = user.FirstName + " " + user.LastName;
    //                    sendVerificationEmail(user.LoginEmail, FullName, subject, MsgBody, Convert.ToInt64(user.UserInfoID));
    //                }
    //            }
    //    }
    //    catch (Exception)
    //    { }
    //}

    private void MailNotifyAccountingService()
    {
        try
        {
            List<GetGSEUsersResult> lstRecipients = new List<GetGSEUsersResult>();
                lstRecipients = new AssetMgtRepository().GetGSEUsers().ToList();
                foreach (var user in lstRecipients)
                {
                    if (objAssetVendorRepository.CheckEmailAuthentication(Convert.ToInt64(user.UserInfoID), 9) == true)// 9 For Accounting Emails
                    {
                        List<AssetRepairMgtRepository.RepairOrderList> objRepairOrderMail = new List<AssetRepairMgtRepository.RepairOrderList>();
                        objRepairOrderMail = objAssetRepairRepository.GetRepairOrderListBy(0, 0, this.RepairOrderID, 0, 0, "0", "0",null);

                        string CustomerName = objAssetRepairRepository.DisplayCustomerInfo(this.RepairOrderID).Name;

                        String subject = CustomerName + "-" + objRepairOrderMail[0].EquipmentID;
                        String FullName = user.FirstName + " " + user.LastName;
                        String MsgBody = "Service is complete on asset- " + objRepairOrderMail[0].EquipmentID + " you can bill.";

                        sendVerificationEmail(user.LoginEmail, FullName, subject, MsgBody, Convert.ToInt64(user.UserInfoID));
                    }
                }
        }
        catch (Exception)
        { }
    }

    protected void gvPartsOrdered_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "del")
        {
            Int64 val = Convert.ToInt64(e.CommandArgument);
           
                objAssetRepairRepository.DeletePartsOrdered(val);
                lblPartsOrderedMsg.Text = "Record deleted successfully";
                bindGridview();
           
        }
    }
    
    /// <summary>
    /// Datalst Item data bound command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dtlField_ItemDataBound(object sender, DataListItemEventArgs e)
    {

    }

   
    #endregion

    #region Methods

    void DisplayData()
    {

        AssetVendorRepository objAssetvendorRepository = new AssetVendorRepository();
        try
        {
            DataTable dt = Common.ListToDataTable(objAssetRepairRepository.GetRepairOrderListBy(0, 0, this.RepairOrderID, 0, 0, "0", "0",null));
            if (dt.Rows.Count != 0)
            {

                //--Main Start--
               // dvRepairReason.Visible = true;
                lblAssetType.Text = Convert.ToString(dt.Rows[0]["EquipmetType"]);
                lblProblemDesc.Text = Convert.ToString(dt.Rows[0]["ProblemDescription"]);
                lblAssetID.Text = Convert.ToString(dt.Rows[0]["EquipmentID"]);
                lblRepairNumber.Text = "RN" + Convert.ToString(dt.Rows[0]["AutoRepairNumber"]);                
                lblStatus.Text = Convert.ToString(dt.Rows[0]["RepairStatus"]);
                lblBrand.Text = Convert.ToString(dt.Rows[0]["Brand"]);
                lblYear.Text = Convert.ToString(dt.Rows[0]["ModalYear"]);
                lblFuel.Text = Convert.ToString(dt.Rows[0]["Fuel"]);
                lblSerialNo.Text = Convert.ToString(dt.Rows[0]["SerialNo"]);
                lblReasonRepair.Text = Convert.ToString(dt.Rows[0]["RepairReason"]);
                lblVendorRepairID.Text = Convert.ToString(dt.Rows[0]["VendorRepairID"]);
                EquipmentTypeID = Convert.ToInt64(dt.Rows[0]["EquipmentTypeID"]);
                FieldCompany = Convert.ToInt64(dt.Rows[0]["CompanyID"]);
                if (Convert.ToString(dt.Rows[0]["CreatedDate"]) != string.Empty)
                {
                    lblRequestedDate.Text = Convert.ToString(Convert.ToDateTime(dt.Rows[0]["CreatedDate"]).ToString("MM/dd/yyyy"));
                }
                if (Convert.ToString(dt.Rows[0]["ReturnedDate"]) != string.Empty)
                {
                    lblReturnedDate.Text = Convert.ToString(Convert.ToDateTime(dt.Rows[0]["ReturnedDate"]).ToString("MM/dd/yyyy"));
                }
                txtVendorRepairID.Text = Convert.ToString(dt.Rows[0]["VendorRepairID"]);
                ddlRepairOrderStatus.SelectedValue = Convert.ToString(dt.Rows[0]["RepairStatusID"]);
                // -- Main End--
                
                bindFinalBillingGridview();
                // Bind Parts History
                bindPartsHistoryGridView();
                DisplayCustomerInformation();
                bindPostInvoicegrid();
                getFields();
                getFieldDesc();
                
            }

        }
        catch (Exception)
        {


        }

    }

    /// <summary>
    /// Add File for Insurance Policy
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //void DisplayPopupData()
    //{
    //    try
    //    {
    //        objEquipment = objAssetRepairRepository.GetDetailFromCampID(this.RepairOrderID);
    //        if (objEquipment != null)
    //        {
    //            --Main Start--
    //            ddlEquipmentType.SelectedValue = Convert.ToString(objEquipment.EquipmentTypeID);
    //            ddlBrand.SelectedValue = Convert.ToString(objEquipment.BrandID);
    //            txtModelYear.Text = Convert.ToString(objEquipment.ModelYear);
    //            txtSerialNo.Text = Convert.ToString(objEquipment.SerialNumber);
    //            ddlFuelType.SelectedValue = Convert.ToString(objEquipment.FuelTypeID);
    //            ddlBaseStation.SelectedValue = Convert.ToString(objEquipment.BaseStationID);
    //            ddlStatus.SelectedValue = Convert.ToString(objEquipment.EquipmentStatus);
    //            txtSerialNo.Text = objEquipment.SerialNumber;
    //            --Main End--

    //        }
    //    }
    //    catch (Exception)
    //    {


    //    }
    //}
    private Dictionary<Int32, String> GetDropDownsValues()
    {
        // Create and return new Hashtable.
        Dictionary<Int32, String> dictioanytable = new Dictionary<int, string>();

        dictioanytable.Add(1, "Asset Parts History");
        dictioanytable.Add(2, "Equipment Images");
        dictioanytable.Add(3, "Mechanic's Parts Ordered");
        dictioanytable.Add(4, "Mechanic's Service Repair Notes");
        dictioanytable.Add(5, "Mechanic's Work Hours");
        dictioanytable.Add(6, "Our Customers Contact Information");
        dictioanytable.Add(7, "Parts & Asset Specifications");
        dictioanytable.Add(8, "Problem Description");
        dictioanytable.Add(9, "Transfer to Accounting for Billing");
       
        return dictioanytable;

    }
    private void bindDropDowns()
    {
        if (GetDropDownsValues().Count > 0)
        {
            ddlShowMe.DataSource = GetDropDownsValues();
            ddlShowMe.DataTextField = "Value";
            ddlShowMe.DataValueField = "Key";
            ddlShowMe.DataBind();
        }

        //get Vendors
        AssetVendorRepository objVendorRep = new AssetVendorRepository();
        List<EquipmentVendorMaster> objVendorList = new List<EquipmentVendorMaster>();
        objVendorList = objVendorRep.GetAllEquipmentVendor();
        Common.BindDDL(ddlVendor, objVendorList, "EquipmentVendorName", "EquipmentVendorID", "-Select Vendor-");

        //get Repair Status
        List<EquipmentLookup> objStatus = objAssetRepairRepository.GetRepairStatusInfo(0, "RepairStatus");
        Common.BindDDL(ddlRepairOrderStatus, objStatus, "sLookupName", "iLookupID", "-Select-");

        //Bind Dropdown Job Code        
        List<EquipmentJobCodeLookup> objEquipmentJobCodeLookupList = new List<EquipmentJobCodeLookup>();
        objEquipmentJobCodeLookupList = objAssetMgtRepository.GetAllJobCode();
        Common.BindDDL(ddlJobCode, objEquipmentJobCodeLookupList, "JobCodeName", "JobCodeID", "--Select--");

        //get Vendor List
        List<GetEquipVendorDetailResult> objGetEquipVendorDetailResult = new List<GetEquipVendorDetailResult>();
        objGetEquipVendorDetailResult = objAssetVendorRepository.GetVendorDetailBySP(this.CompanyID);
        Common.BindDDL(ddlInvoiceVendor, objGetEquipVendorDetailResult, "EquipmentVendorName", "EquipmentVendorID", "-Select Vendor-");

    }
    //private void bindAssetPartGrid()
    //{
    //    try
    //    {
    //         DataTable dt = Common.ListToDataTable(objAssetRepairRepository.GetRepairOrderListBy(0, 0, this.RepairOrderID, 0, 0, "0", "0",null));
    //         if (dt.Rows.Count != 0)
    //         {
    //             gvEquipment.DataSource = dt;
    //             gvEquipment.DataBind();
    //         }
    //    }
    //    catch (Exception)
    //    {
    //    }
    //}
    private void bindGridview()
    {
        //Parts Ordered Grid
        List<AssetRepairMgtRepository.GetEquipmentBillingPartsOrderedInfo> objListpartorder = objAssetRepairRepository.GetEquipmentBillingPartsOrderedDetails(this.RepairOrderID);
        gvPartsOrdered.DataSource = objListpartorder;
        gvPartsOrdered.DataBind();
        //Repair Hour Grid
        List<AssetRepairMgtRepository.GetEquipmentBillingRepairHourInfo> objList = objAssetRepairRepository.GetEquipmentBillingRepairHourDetails(this.RepairOrderID);
        if (objList.Count!=0)
        {
            dvTotalRecords.Visible = true;
            LblTotalHours.Text = Convert.ToString(objList.Sum(x => x.HoursWorked));
            gvPostJobBilling.DataSource = objList;
            gvPostJobBilling.DataBind();
        }
        bindFinalBillingGridview();
        bindPartsHistoryGridView();
    }
    private void bindFinalBillingGridview()
    {
        //Parts Ordered Grid
        List<AssetRepairMgtRepository.GetEquipmentBillingPartsOrderedInfo> objListpartorder = objAssetRepairRepository.GetEquipmentBillingPartsOrderedDetails(this.RepairOrderID);
        if (objListpartorder.Count!=0)
        {
            dvPartsUsed.Visible = true;
            gvFBPartsOrdered.DataSource = objListpartorder;
            gvFBPartsOrdered.DataBind();
        }
        else
            dvPartsUsed.Visible = false;
       
        //Repair Hour Grid
        List<AssetRepairMgtRepository.GetEquipmentBillingRepairHourInfo> objList = objAssetRepairRepository.GetEquipmentBillingRepairHourDetails(this.RepairOrderID);
        if (objList.Count != 0)
        {
            dvRepairHours.Visible = true;
            gvFBPostJobBilling.DataSource = objList;
            gvFBPostJobBilling.DataBind();
        }
        else
            dvRepairHours.Visible = false;
       
    }
    private void bindPartsHistoryGridView()
    {
        //Parts Ordered Grid
        List<AssetRepairMgtRepository.GetEquipmentBillingPartsOrderedInfo> objListPartsHistory = objAssetRepairRepository.GetEquipmentBillingPartsOrderedDetails(this.RepairOrderID);
        if (objListPartsHistory.Count != 0)
        {
            gvPartsHistory.DataSource = objListPartsHistory;
            gvPartsHistory.DataBind();
        }        

    }
    //public void BindPopupValues()
    //{
    //    try
    //    {
    //        LookupRepository objLookupRepository = new LookupRepository();
    //        List<INC_Lookup> objList = new List<INC_Lookup>();
    //        String LookUpCode = string.Empty;
    //        //get Equipment Type
    //        LookUpCode = "EquipmentType";
    //        objList = objLookupRepository.GetByLookup(LookUpCode);
    //        Common.BindDDL(ddlEquipmentType, objList, "sLookupName", "iLookupID", "-Select-");
    //        //get Brand
    //        LookUpCode = "Brand";
    //        objList = objLookupRepository.GetByLookup(LookUpCode);
    //        Common.BindDDL(ddlBrand, objList, "sLookupName", "iLookupID", "-Select-");
    //        //get Fuel Type
    //        LookUpCode = "FuelType";
    //        objList = objLookupRepository.GetByLookup(LookUpCode);
    //        Common.BindDDL(ddlFuelType, objList, "sLookupName", "iLookupID", "-Select-");
    //        //get Status
    //        LookUpCode = "EquipmentStatus";
    //        objList = objLookupRepository.GetByLookup(LookUpCode);
    //        Common.BindDDL(ddlStatus, objList, "sLookupName", "iLookupID", "-Select-");
    //        //get Base Stations
    //        CompanyRepository objRepo = new CompanyRepository();
    //        List<INC_BasedStation> objBaseStationList = new List<INC_BasedStation>();
    //        objBaseStationList = objRepo.GetAllBaseStationResult();
    //        Common.BindDDL(ddlBaseStation, objBaseStationList, "sBaseStation", "iBaseStationId", "-Select-");
        


    //    }
    //    catch (Exception)
    //    {


    //    }
    //}

    private void ResetControls()
    {
        ddlShowMe.SelectedIndex = 7;
        dvPartsHistory.Visible = false;
        dvEquipmentImageHide.Visible = false;
        dvOrderParts.Visible = false;
        dvNotesHistory.Visible = false;
        dvShowRepairHours.Visible = false;
        dvCustInfo.Visible = false;
        dvPartsAsset.Visible = false;
        dvRepairReason.Visible = true;
        dvCloseBilling.Visible = false;  
    }

   
    public void DisplayNotes()
    {
        try
        {
            txtNotesHistory.Text = "";
            txtMecanicNotes.Text = "";
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            //Check for vendor employee
            List<NoteDetail> objList = objRepo.GetByForeignKeyId(Convert.ToInt64(this.RepairOrderID), Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement);

            foreach (NoteDetail obj in objList)
            {
                if (obj.SpecificNoteFor == "Service Repair Notes")
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
                    txtMecanicNotes.Text = txtNotesHistory.Text;//Entry in Mecanics Notes
                }

            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    public void DisplayNotifications()
    {
        try
        {
            EquipmentRepairOrder objRepairNotification = objAssetRepairRepository.GetRepairOrderByID(this.RepairOrderID);
            txtAssetBack.Text = objRepairNotification.NotifyAssetBackOn!=null?"Notified On " + Convert.ToString(objRepairNotification.NotifyAssetBackOn):" ";
            txtReadyForBilling.Text = objRepairNotification.ReadyForBillingOn != null ? "Notified On " + Convert.ToString(objRepairNotification.ReadyForBillingOn) : " ";
            txtBillingFinished.Text = objRepairNotification.BillingFinishedOn != null ? "Notified On " + Convert.ToString(objRepairNotification.BillingFinishedOn) : " ";
        }
        catch (Exception)
        {}
    }

    public void getImages()
    {

        try
        {
            objEquipmentImageList = objAssetMgtRepository.GetEquipmentImagesById(this.EquipmentMasterID, rdbAssetImage.Checked);
            dtSplash.DataSource = objEquipmentImageList;
            dtSplash.DataBind();
        }
        catch (Exception)
        {
        }

    }

    private void DisplayCustomerInformation()
    {
        try
        {
            GetEquipRepairCustomerResult objCust = objAssetRepairRepository.DisplayCustomerInfo(this.RepairOrderID);
            lblName.Text = objCust.Name;
            lblAddress.Text = objCust.Adr;
            lblTelephone.Text = objCust.Telephone;
            lblMobile.Text = objCust.Mobile;
            lblEmail.Text = objCust.Email;
        }
        catch (Exception)
        { }
    }
    public void getFields()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = Common.ListToDataTable(objFieldRepository.GetFieldsForAssetProfile(this.FieldCompany, this.EquipmentTypeID));
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
            dt = Common.ListToDataTable(objFieldRepository.GetFieldDetailDesc(this.FieldCompany, this.EquipmentMasterID));
            Int64 dtlFieldMasterID = 0;
            Int64 dtFieldMasterID = 0;

            foreach (DataRow dr in dt.Rows)
            {
                dtFieldMasterID = Convert.ToInt64(dr[2]);
                foreach (DataListItem item in dtlField.Items)
                {
                    HiddenField hdnFieldMasterID = (HiddenField)item.FindControl("hdnFieldMasterID");
                    dtlFieldMasterID = Convert.ToInt64(hdnFieldMasterID.Value);
                    if (dtFieldMasterID == dtlFieldMasterID)
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
    private void ResetPopupFields()
    {
        txtDatejobbilling.Text = "";
        txtHoursWorked.Text = "";
        txtSummaryWorkPerformed.Text = "";
        txtOrderQuantity.Text = "";
        txtPartNumber.Text = "";
        txtPartDescription.Text = "";
        ddlVendor.SelectedIndex = 0;
        //-----Post Invoice Start
        ddlVendor.SelectedIndex = 0;
        ddlVendor.Enabled = true;
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
        {
            EquipmentVendorEmployee objEquipmentVendorEmployee = new EquipmentVendorEmployee();
            objEquipmentVendorEmployee = objAssetVendorRepository.GetVendorEmpByUserInfoID(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
            string VendorId = Convert.ToString(objEquipmentVendorEmployee.VendorID);
            ddlInvoiceVendor.SelectedValue = VendorId;
            ddlInvoiceVendor.Enabled = false;
        }
        txtDateofService.Text = "";
        txtInvoice.Text = "";
        txtDescription.Text = "";
        txtAmount.Text = "";
        ddlJobCode.SelectedIndex = 0;
        BindChecklist(Convert.ToInt64(ddlJobCode.SelectedValue));
        //-----Post Invoice End
    }
    private void sendVerificationEmail(String UserEmail, String UserName, String subjectText, String Body, Int64 userInfoID)
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
            //General.SendMail(sFrmadd, sToadd, sSubject, MessageBody.ToString(), smtphost, smtpport, true, false, sFrmname);
            String TxtBody = "Returned to Service - (" + lblAssetType.Text + ")-(" + lblAssetID.Text + ")";
            new AssetMgtRepository().TextMsg(userInfoID, TxtBody);
            //Local testing email settings
            if (HttpContext.Current.Request.IsLocal)
                General.SendMail(sFrmadd, "incentextest6@gmail.com", sSubject, MessageBody.ToString(), "smtp.gmail.com", 587, "incentextest6@gmail.com", "test6incentex", sFrmname, true, true);

        }
        catch (Exception ex)
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
                 dt = Common.ListToDataTable(objAssetRepairRepository.GetEquipMaintenanceForRepair(Convert.ToInt64(EquipmentMasterID)));
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
    public void BindChecklist(Int64 JobCode)
    {
        DataTable dt = new DataTable();
        dt = Common.ListToDataTable(objAssetMgtRepository.GetAllSubJobCodeDetail(JobCode));

        cblJobSubCode.DataSource = dt;
        cblJobSubCode.DataBind();

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
    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
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

        }
        catch (Exception)
        { }

    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
    }
    #endregion
    #endregion

}