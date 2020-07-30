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
using System.Collections.Generic;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.IO;
public partial class AssetManagement_Reports_PartsPurchasedPage : PageBase
{
    #region Data Members
    AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();
    public Int64 VEUserInfoID
    {
        get
        {
            if (this.ViewState["VEUserInfoID"] == null)
                return 0;
            else
                return Convert.ToInt64(this.ViewState["VEUserInfoID"].ToString());
        }
        set
        {
            this.ViewState["VEUserInfoID"] = value;
        }
    }
    public Int64 JobCodeID
    {
        get
        {
            if (this.ViewState["JobCodeID"] == null)
                return 0;
            else
                return Convert.ToInt64(this.ViewState["JobCodeID"].ToString());
        }
        set
        {
            this.ViewState["JobCodeID"] = value;
        }
    }
    Int64 CompanyID
    {
        get
        {
            if (ViewState["CompanyID"] == null)
            {
                ViewState["CompanyID"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyID"]);
        }
        set
        {
            ViewState["CompanyID"] = value;
        }
    }
    Int64 BaseStationID
    {
        get
        {
            if (ViewState["BaseStationID"] == null)
            {
                ViewState["BaseStationID"] = 0;
            }
            return Convert.ToInt64(ViewState["BaseStationID"]);
        }
        set
        {
            ViewState["BaseStationID"] = value;
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
    String FDate
    {
        get
        {
            if (ViewState["FDate"] == null)
            {
                ViewState["FDate"] = "1/1/2012";
            }
            return Convert.ToString(ViewState["FDate"]);
        }
        set
        {
            ViewState["FDate"] = value;
        }
    }
    String TDate
    {
        get
        {
            if (ViewState["TDate"] == null)
            {
                ViewState["TDate"] = DateTime.Now.ToString();
            }
            return Convert.ToString(ViewState["TDate"]);
        }
        set
        {
            ViewState["TDate"] = value;
        }
    }
    Int64 EquiTypeIDView
    {
        get
        {
            if (ViewState["EquiTypeIDView"] == null)
            {
                ViewState["EquiTypeIDView"] = 0;
            }
            return Convert.ToInt64(ViewState["EquiTypeIDView"]);
        }
        set
        {
            ViewState["EquiTypeIDView"] = value;
        }
    }
    #endregion
    #region Event Handlers
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
        if (!IsPostBack)
        {
            CheckLogin();
            ((Label)Master.FindControl("lblPageHeading")).Text = "Parts Purchased Detail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/Reports/PartsPurchasedReport.aspx";
            //-----

            if (Request.QueryString.Count > 0)
            {

                if (string.IsNullOrEmpty(Request.QueryString["EquipmentTypeID"]) == false && Request.QueryString["EquipmentTypeID"] != "0")
                {
                    EquipmentTypeID = Convert.ToInt64(Request.QueryString["EquipmentTypeID"]);
                }
                if (string.IsNullOrEmpty(Request.QueryString["BaseStationID"]) == false && Request.QueryString["BaseStationID"] != "0")
                {
                    BaseStationID = Convert.ToInt64(Request.QueryString["BaseStationID"]);
                }
                if (string.IsNullOrEmpty(Request.QueryString["FDate"]) == false)
                {
                    FDate = Convert.ToString(Request.QueryString["FDate"]);
                }
                if (string.IsNullOrEmpty(Request.QueryString["TDate"]) == false)
                {
                    TDate = Convert.ToString(Request.QueryString["TDate"]);
                }
                if (string.IsNullOrEmpty(Request.QueryString["EquiTypeIDView"]) == false && Request.QueryString["EquiTypeIDView"] != "0")
                {
                    EquiTypeIDView = Convert.ToInt64(Request.QueryString["EquiTypeIDView"]);
                }
               
            }

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                CompanyID = Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId);
            }
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                VEUserInfoID = Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID);
            }

            //-----
            BindData();
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/Reports/PartsPurchasedReport.aspx?EquipmentTypeID=" + this.EquiTypeIDView + "&BaseStationID=" + this.BaseStationID + "&FromDate=" + Convert.ToString(Convert.ToDateTime(this.FDate).ToString("MM/dd/yyyy")) + "&ToDate=" + Convert.ToString(Convert.ToDateTime(this.TDate).ToString("MM/dd/yyyy"));
        }
    }
    protected void rptProductCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Int64 JobCode = Convert.ToInt64(((HiddenField)e.Item.FindControl("hdnJobCodeID")).Value);
            List<AssetReportRepository.PartsPurchaseDetailResult> objPartPurchaseDetail = new AssetReportRepository().GetPartsPurchaseDetail(Convert.ToDateTime(this.FDate), Convert.ToDateTime(this.TDate), this.CompanyID, this.EquipmentTypeID, this.BaseStationID, JobCode);
            if (objPartPurchaseDetail.Count > 0)
            {
                ((GridView)e.Item.FindControl("gvEquipment")).DataSource = objPartPurchaseDetail;
                ((GridView)e.Item.FindControl("gvEquipment")).DataBind();
            }
            else
            {
                ((Label)e.Item.FindControl("lblMsg")).Text = "No Data Found.";
            }
        }
    }
    protected void gvEquipment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "PDF")
            {
                string val = Convert.ToString(e.CommandArgument);
                string sFilePath = Server.MapPath("../../UploadedImages/EquipmentInvoice/");
                string strFullPath = sFilePath + val;
                DownloadFile(strFullPath);               
            }
        }
        catch (Exception)
        {}
       

    }
    protected void gvEquipment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //Change PDF & XL Icon Conditionally
                string FileName = string.Empty;
                string FileExt = string.Empty;
                ImageButton lnkbtnPDF = (ImageButton)e.Row.FindControl("lnkbtnPDF");
                HiddenField hfPDF = (HiddenField)e.Row.FindControl("hfPDF");
                string val = Convert.ToString(hfPDF.Value);
                if (string.IsNullOrEmpty(val) == false)
                {
                    lnkbtnPDF.Enabled = true;
                    FileName = val;
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
        }
        catch (Exception)
        { }

    }      
   
    #endregion
    #region Methods
   
    public void BindData()
    {
        try
        {
            List<AssetReportRepository.GetJobCodelResult> objJobCode = new AssetReportRepository().GetJobCodeList(Convert.ToDateTime(this.FDate), Convert.ToDateTime(this.TDate), this.CompanyID, this.EquipmentTypeID, this.BaseStationID);
            rptProductCategory.DataSource = objJobCode;
            rptProductCategory.DataBind();
        }
        catch { }
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
    #endregion
}
