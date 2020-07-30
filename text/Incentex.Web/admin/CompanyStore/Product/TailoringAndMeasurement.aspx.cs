//Page Modification done by mayur on 1st-Jun-2012
//Making tailoring option by store product ID wise instead of product item ID wise
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_ProductManagement_TailoringAndMeasurement : PageBase
{
    #region Data Members

    Int64 StorProductID
    {
        get
        {
            if (ViewState["StorProductID"] == null)
            {
                ViewState["StorProductID"] = 0;
            }
            return Convert.ToInt64(ViewState["StorProductID"]);
        }
        set
        {
            ViewState["StorProductID"] = value;
        }
    }

    Int64 MasterStyleID
    {
        get
        {
            if (ViewState["MasterStyleID"] == null)
            {
                ViewState["MasterStyleID"] = 0;
            }
            return Convert.ToInt64(ViewState["MasterStyleID"]);
        }
        set
        {
            ViewState["MasterStyleID"] = value;
        }
    }

    LookupRepository objLookupRepos = new LookupRepository();
    Common objcommon = new Common();
    TailoringMeasurementsRepository objTailoringMeasurementsRepo = new TailoringMeasurementsRepository();
    ProductItemDetailsRepository objProductItemDetailsRepository = new ProductItemDetailsRepository();

    /// <summary>
    /// To Display WorkgroupName
    /// </summary>
    String WorkGroupNameToDisplay
    {
        get
        {
            if (Session["WorkGroupNameToDisplay"] != null && Session["WorkGroupNameToDisplay"].ToString().Length > 0)
                ViewState["WorkGroupNameToDisplay"] = " - " + Session["WorkGroupNameToDisplay"].ToString();
            else
                ViewState["WorkGroupNameToDisplay"] = "";

            return ViewState["WorkGroupNameToDisplay"].ToString();
        }
    }

    #endregion

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        Int32 ManageID = (Int32)Incentex.DAL.Common.DAEnums.ManageID.CompanyProduct;
        Session["ManageID"] = ManageID;
        lnkTailorChart.Attributes.Add("onclick", "return validate()");

        if (!IsPostBack)
        {
            base.MenuItem = "Store Management Console";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            Int64 StoreID = 0;
            if (Request.QueryString["Id"] != null)
                StoreID = Convert.ToInt64(Request.QueryString["Id"].ToString());

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Tailoring & Measurements" + WorkGroupNameToDisplay;
            ((HtmlGenericControl)manuControl.FindControl("dvSubMenu")).Visible = false;
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.ProductReturnURL;

            if (Request.QueryString["SubId"] != null && Request.QueryString["SubId"] != "" && Request.QueryString["SubId"] != "0")
            {
                StorProductID = Convert.ToInt32(Request.QueryString["SubId"]);
                ProductItem objProductItem = objProductItemDetailsRepository.GetByProductId(Convert.ToInt32(StorProductID));
                MasterStyleID = objProductItem.MasterStyleID;
                manuControl.PopulateMenu(4, 0, StorProductID, StoreID, false);
                FillMasterItem();
                bindGridViewTailoringChart();
            }
            else
            {
                Response.Redirect("General.aspx?SubId=" + Request.QueryString["SubId"] + "&Id=" + Request.QueryString["Id"]);
            }
        }
    }

    protected void lnkTailorChart_Click(object sender, EventArgs e)
    {
        try
        {
            
            ProductItemTailoringMeasurement objTailoringMesurements = new ProductItemTailoringMeasurement();
            objTailoringMesurements.StoreProductID = StorProductID;
            objTailoringMesurements.MasterStyleId = MasterStyleID;

            //Tailoring
            if (fpGuideLineUpload.Value != "")
            {
                String sFileNameGuidline = System.DateTime.Now.Ticks + "_" + fpGuideLineUpload.Value;
                objTailoringMesurements.TailoringGuidelines = sFileNameGuidline;
                Request.Files[0].SaveAs(Server.MapPath("~/UploadedImages/TailoringMeasurement/") + sFileNameGuidline);
            }
            else
            {
                objTailoringMesurements.TailoringGuidelines = "";
            }

            //Measurements
            if (fpMeasurementsUpload.Value != "")
            {
                String sFileNameMeasurement = System.DateTime.Now.Ticks + "_" + fpMeasurementsUpload.Value;
                objTailoringMesurements.TailoringMeasurementChart = sFileNameMeasurement;
                Request.Files[1].SaveAs(Server.MapPath("~/UploadedImages/TailoringMeasurement/") + sFileNameMeasurement);
            }
            else
            {
                objTailoringMesurements.TailoringMeasurementChart = "";
            }


            List<ProductItemTailoringMeasurement> objTailoringExists = new List<ProductItemTailoringMeasurement>();
            objTailoringExists = objTailoringMeasurementsRepo.GetTailoringMeasurementByStoreProductIDAndMasterItemID(MasterStyleID, StorProductID);
            if (objTailoringExists.Count > 0)
            {
                bindGridViewTailoringChart();
                lblMessageGuide.Text = "Already exist Tailoring Guidelines & Measurement Charts for this master item!";
                return;
            }
            else
            {
                objTailoringMeasurementsRepo.Insert(objTailoringMesurements);
                objTailoringMeasurementsRepo.SubmitChanges();
            }

            bindGridViewTailoringChart();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvTailMeasurementChart_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "view")
        {
            String file = e.CommandArgument.ToString();
            HiddenField lnkFileName;
            GridViewRow row;
            row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            lnkFileName = (HiddenField)gvTailMeasurementChart.Rows[row.RowIndex].FindControl("hdnTailoringGuide");
            String filePath = IncentexGlobal.StoreProductTailoringMeasurement;
            String strFullPath = filePath + lnkFileName.Value;
            DownloadFile(strFullPath);
        }
        else if (e.CommandName == "view1")
        {
            String file = e.CommandArgument.ToString();
            HiddenField lnkFileName;
            GridViewRow row;
            row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            lnkFileName = (HiddenField)gvTailMeasurementChart.Rows[row.RowIndex].FindControl("hdnMeasuremntChart");
            String filePath = IncentexGlobal.StoreProductTailoringMeasurement;
            String strFullPath = filePath + lnkFileName.Value;
            DownloadFile(strFullPath);
        }
        else if (e.CommandName == "del")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            ImageButton lbtnDelete;
            Label lblProductItemTailoringMeasurementsID;
            HiddenField hdnGildline;
            HiddenField hdnMeasurement;
            GridViewRow row;
            row = (GridViewRow)((ImageButton)e.CommandSource).Parent.Parent;
            lbtnDelete = (ImageButton)(gvTailMeasurementChart.Rows[row.RowIndex].FindControl("lnkbtndelete"));
            lblProductItemTailoringMeasurementsID = (Label)(gvTailMeasurementChart.Rows[row.RowIndex].FindControl("lblProductItemTailoringMeasurementID"));
            hdnGildline = (HiddenField)(gvTailMeasurementChart.Rows[row.RowIndex].FindControl("hdnTailoringGuide"));
            hdnMeasurement = (HiddenField)(gvTailMeasurementChart.Rows[row.RowIndex].FindControl("hdnMeasuremntChart"));
            objTailoringMeasurementsRepo.DeleteProductItemTailoringMeasuremts(Convert.ToInt64(lblProductItemTailoringMeasurementsID.Text), IncentexGlobal.CurrentMember.UserInfoID);
            objTailoringMeasurementsRepo.SubmitChanges();

            //if (hdnGildline.Value != null)
            //    objcommon.DeleteImageFromFolder(hdnGildline.Value, IncentexGlobal.StoreProductTailoringMeasurement);

            //if (hdnMeasurement.Value != null)
            //    objcommon.DeleteImageFromFolder(hdnMeasurement.Value, IncentexGlobal.StoreProductTailoringMeasurement);

            bindGridViewTailoringChart();
        }
    }

    #endregion

    #region Methods

    public void bindGridViewTailoringChart()
    {
        try
        {
            if (StorProductID != 0 && MasterStyleID != 0)
            {
                List<TailoringMeasurementsRepository.ProductItemTailoringMeasurementResult> oad = new TailoringMeasurementsRepository().TailoringMeasurementsChart(StorProductID, MasterStyleID);

                gvTailMeasurementChart.DataSource = oad;
                gvTailMeasurementChart.DataBind();

                if (gvTailMeasurementChart.Rows.Count > 0)
                    lblMessageGuide.Text = "";
                else
                    lblMessageGuide.Text = "No Record found!";
            }
            else
                Response.Redirect("TailoringAndMeasurement.aspx?SubId=" + Request.QueryString["SubId"] + "&Id=" + Request.QueryString["Id"]);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void DownloadFile(String filepath)
    {
        System.IO.Stream iStream = null;
        String type = "";
        String ext = Path.GetExtension(filepath);

        // Buffer to read 10K bytes in chunk:
        Byte[] buffer = new Byte[10000];
        
        // Total bytes to read:
        Int64 dataToRead;

        // Identify the file name.
        String filename = System.IO.Path.GetFileName(filepath);

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
                }
            }

            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.ContentType = type;
            Response.WriteFile(filepath);
            Response.End();
        }
        catch (Exception ex)
        {
            // Trap the error, if any.
            Response.Write("Error : " + ex.Message);
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


    public void FillMasterItem()
    {
        try
        {
            String strStatus = "MasterItemNumber";
            ddlMasterItem.DataSource = objLookupRepos.GetByLookup(strStatus);
            ddlMasterItem.DataValueField = "iLookupID";
            ddlMasterItem.DataTextField = "sLookupName";
            ddlMasterItem.DataBind();
            ddlMasterItem.Items.Insert(0, new ListItem("-select-", "0"));
            ddlMasterItem.SelectedValue = Convert.ToString(MasterStyleID);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}