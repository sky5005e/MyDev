using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using System.Data;
using Incentex.DAL;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;


public partial class admin_ArtWorkDecoratingLibrary_AddDecoratingSpecifications : PageBase
{
    #region Properties
    LookupRepository objLookRep = new LookupRepository();
    SupplierRepository objSupplier = new SupplierRepository();
    PurchaseOrderManagmentRepository objPurchaseRep = new PurchaseOrderManagmentRepository();
    DecoratingSpecsRepository objDecoratingRepo = new DecoratingSpecsRepository();
    ArtWorkRepository objArtworkRepo = new ArtWorkRepository();
    String SelectedArtworkID
    {
        get
        {
            if (ViewState["SelectedArtworkID"] == null)
                ViewState["SelectedArtworkID"] = String.Empty;
            return ViewState["SelectedArtworkID"].ToString();
        }
        set
        {
            ViewState["SelectedArtworkID"] = value;
        }
    }
    Int64 DecoratingSpecsID
    {
        get
        {
            if (ViewState["DecoratingSpecsID"] == null)
                ViewState["DecoratingSpecsID"] = String.Empty;
            return Convert.ToInt64(ViewState["DecoratingSpecsID"]);
        }
        set
        {
            ViewState["DecoratingSpecsID"] = value;
        }
    }
    Int64 MasterStyleID
    {
        get
        {
            if (ViewState["MasterStyleID"] == null)
                ViewState["MasterStyleID"] = 0;
            return Convert.ToInt64(ViewState["MasterStyleID"]);
        }
        set
        {
            ViewState["MasterStyleID"] = value;
        }
    }
    /// <summary>
    /// Create imprint list 
    /// </summary>
    List<ImprintList> list
    {
        get
        {
            if (this.ViewState["list"] == null)
            {
                this.ViewState["list"] = new List<ImprintList>();
            }
            return (List<ImprintList>)(this.ViewState["list"]);
        }
        set
        {
            this.ViewState["list"] = value;
        }
    }
    String CmdString
    {
        get
        {
            if (ViewState["CmdString"] == null)
                ViewState["CmdString"] = String.Empty;
            return ViewState["CmdString"].ToString();
        }
        set
        {
            ViewState["CmdString"] = value;
        }
    }
    /// <summary>
    /// Set unique number
    /// </summary>
    String uniqueNumber = String.Empty;
    #endregion

    #region Page Event's
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Artwork Library";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Add Decorating Specifications";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/ArtWorkDecoratingLibrary/ArtworkIndex.aspx";
            tblgrdViewImprint.Visible = false;
            this.BindDropdown();

            if (!String.IsNullOrEmpty(Request.QueryString["DecoId"]))
                this.DecoratingSpecsID = Convert.ToInt64(Request.QueryString["DecoId"]);

        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCompany.SelectedValue != "0")
            {
                // get all file name in this string builder;
                StringBuilder Filename = new StringBuilder();
                #region Saving Attachments
                if (Request.Files.Count > 0)
                {
                    HttpFileCollection Attachments = Request.Files;
                    for (int i = 0; i < Attachments.Count; i++)
                    {
                        HttpPostedFile Attachment = Attachments[i];
                        if (Attachment.ContentLength > 0 && !String.IsNullOrEmpty(Attachment.FileName))
                        {
                            String SavedFileName = "Artwork_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Attachment.FileName;
                            SavedFileName = Common.MakeValidFileName(SavedFileName);
                            String filePath = Server.MapPath("../../UploadedImages/DecoratingFiles/") + SavedFileName;
                            Attachment.SaveAs(filePath);

                            if (Filename.ToString() == "")
                            {
                                Filename.Append(SavedFileName);
                            }
                            else
                            {
                                Filename.Append("," + SavedFileName);
                            }
                        }
                    }
                }
                #endregion

                DecoratingSpecificationsMaster objDecSpecMaster = new DecoratingSpecificationsMaster();
                objDecSpecMaster.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
                objDecSpecMaster.ProductType = Convert.ToInt64(ddlProductType.SelectedValue);
                objDecSpecMaster.JobName = Convert.ToString(txtJobName.Text);
                objDecSpecMaster.DecoratingMethod = Convert.ToInt64(ddlDecoratingMethod.SelectedValue);
                objDecSpecMaster.MasterItemNumber = Convert.ToInt64(MasterStyleID);
                objDecSpecMaster.DecoratingReferenceTag = Convert.ToString(lblDecoratingReferenceTag.Text);
                // to save file as per ext. 
                String[] fileext = Filename.ToString().Split(',');
                foreach (var f in fileext)
                {
                    if (f.Contains(".jpg") || f.Contains(".jpeg") || f.Contains(".eps") || f.Contains(".ai")
                        || f.Contains(".dst") || f.Contains(".emb"))
                        objDecSpecMaster.FinalProductImage = f.ToString();
                    else if (f.Contains(".pdf") || f.Contains(".doc") || f.Contains(".txt"))
                        objDecSpecMaster.FinalProof = f.ToString();
                }

                objDecSpecMaster.CreatedDate = DateTime.Now;
                objDecSpecMaster.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objDecoratingRepo.Insert(objDecSpecMaster);
                objDecoratingRepo.SubmitChanges();
                // get DecoratingSpecificationsID
                Int64 DecoratingSpecificationsID = objDecSpecMaster.DecoratingSpecificationsID;

                foreach (var item in list)
                {
                    DecoratingSpecificationsDetail objDecSpecDetails = new DecoratingSpecificationsDetail();

                    objDecSpecDetails.DecoratingSpecificationsID = DecoratingSpecificationsID;
                    objDecSpecDetails.ArtworkDecoratingID = Convert.ToString(item.SelectedArtworkID);
                    objDecSpecDetails.MasterItemNumber = Convert.ToInt64(MasterStyleID);
                    objDecSpecDetails.DecSpecLocationsID = Convert.ToInt64(item.ddlLocSelectedValue);
                    objDecSpecDetails.DecSpecDecoratorID = Convert.ToInt64(item.ddlDecoratorSelectedValue);
                    objDecSpecDetails.ScreenRangeSXL = Convert.ToString(item.ScreenNoSXL);
                    objDecSpecDetails.DesignNumber2XL3XL = Convert.ToString(item.ScreenNo2XL);
                    objDecSpecDetails.DesignNumber5XL7XL = Convert.ToString(item.ScreenNo5XL);
                    objDecSpecDetails.LoctionProofFile = Convert.ToString(item.FileLocationProof);
                    objDecoratingRepo.Insert(objDecSpecDetails);
                }
                objDecoratingRepo.SubmitChanges();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCompany.SelectedValue != "0")
        {
            String Comp = ddlCompany.SelectedItem.Text.Substring(0, 3).ToUpper();
        // to check unique number
        checkAgain:
            uniqueNumber = Comp + "-" + GenerateRandomNumber().ToString();
            if (objDecoratingRepo.HasRandomDecoratingNumber(uniqueNumber))//If true then generate new Random Number
            {
                goto checkAgain;
            }

            lblDecoratingReferenceTag.Text = uniqueNumber.ToString();
        }

        #region Master Item number
        List<StoreProductCustom> objlst = objPurchaseRep.GetAllMasterItemByCompanyID(Convert.ToInt64(ddlCompany.SelectedValue));

        // To get Distinct value from Generic List using LINQ
        // Create an Equality Comprarer Intance
        IEqualityComparer<StoreProductCustom> customComparer = new Common.PropertyComparer<StoreProductCustom>("storeproductid");
        IEnumerable<StoreProductCustom> distinctList = objlst.Distinct(customComparer);
        if (distinctList.Count() > 0)
        {
            ddlMasterItemNumber.DataSource = distinctList;
            ddlMasterItemNumber.DataValueField = "storeproductid";
            ddlMasterItemNumber.DataTextField = "summary";
            ddlMasterItemNumber.DataBind();
        }
        ddlMasterItemNumber.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion
    }

    public void ddlMasterItemNumber_SelectedIndexchanged(object sender, EventArgs e)
    {
        string ImagePath = objPurchaseRep.GetProductImages(Convert.ToInt64(ddlMasterItemNumber.SelectedValue));
        imgmasteritem.Src = "~/UploadedImages/ProductImages/Thumbs/" + ImagePath;
        MasterStyleID = objPurchaseRep.GetMasterStyleID(Convert.ToInt64(ddlMasterItemNumber.SelectedValue));
    }
    public void ddlImprintLocations_SelectedIndexchanged(object sender, EventArgs e)
    {
        list = null;
        Int32 num = Convert.ToInt32(ddlImprintLocations.SelectedValue);
        grdViewImprint.DataSource = GetImprintList(num);
        grdViewImprint.DataBind();
        tblgrdViewImprint.Visible = true;

    }
    protected void btnFileProofSave_Click(object sender, EventArgs e)
    {
        String FilenameLocProof = String.Empty;
        if (fileUploadLocProof.HasFile)
        {
            FilenameLocProof = "FileProof_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fileUploadLocProof.FileName;
            String filePath = Server.MapPath("../../UploadedImages/DecoratingFiles/") + FilenameLocProof;
            fileUploadLocProof.SaveAs(filePath);
        }
        foreach (var item in list)
        {
            list.Where(i => i.ID == Convert.ToInt64(CmdString)).ToList().ForEach(u => u.FileLocationProof = Convert.ToString(FilenameLocProof));
        }
        BindGridViewList();
    }
    protected void btnSCDSave_Click(object sender, EventArgs e)
    {
        foreach (var item in list)
        {
            list.Where(i => i.ID == Convert.ToInt64(CmdString)).ToList().ForEach(u => u.ddlDecoratorSelectedValue = Convert.ToInt32(ddlDecorator.SelectedValue));
            list.Where(i => i.ID == Convert.ToInt64(CmdString)).ToList().ForEach(u => u.ScreenNoSXL = Convert.ToString(txtSXL.Text));
            list.Where(i => i.ID == Convert.ToInt64(CmdString)).ToList().ForEach(u => u.ScreenNo2XL = Convert.ToString(txt2XL.Text));
            list.Where(i => i.ID == Convert.ToInt64(CmdString)).ToList().ForEach(u => u.ScreenNo5XL = Convert.ToString(txt5XL.Text));
        }
        BindGridViewList();
    }
    protected void btnSaveArtworkID_Click(object sender, EventArgs e)
    {

        foreach (GridViewRow gvItem in grdViewArtworkDetails.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvItem.FindControl("chkSelect");
            Label lblProductItemID = (Label)gvItem.FindControl("lblArtworkDecoratingID");
            if (chkSelect != null)
            {
                if (chkSelect.Checked)
                {
                    if (String.IsNullOrEmpty(SelectedArtworkID))
                        SelectedArtworkID = lblProductItemID.Text;
                    else
                        SelectedArtworkID += "," + lblProductItemID.Text;

                    chkSelect.Checked = false;
                }
            }
        }
        foreach (var item in list)
        {
            list.Where(i => i.ID == Convert.ToInt64(CmdString)).ToList().ForEach(c => c.SelectedArtworkID = SelectedArtworkID);
        }

        BindGridViewList();
    }

    protected void grdViewImprint_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "FileLocationProof")
        {
            CmdString = e.CommandArgument.ToString();
            modalFileProofLoc.Show();
        }
        if (e.CommandName == "SCD")
        {
            CmdString = e.CommandArgument.ToString();
            BindDecoratorddl();
            modalImprint.Show();
        }
        if (e.CommandName == "Artwork")
        {
            SelectedArtworkID = null;
            CmdString = e.CommandArgument.ToString();
            list.Where(i => i.ID == Convert.ToInt64(CmdString)).ToList().ForEach(c => c.ddlLocSelectedValue = Convert.ToInt32(hdnddlChanges.Value));
            List<ArtworkDecoratingLibrary> listitem = objArtworkRepo.GetArtworkLibrarybyCompanyID(Convert.ToInt64(ddlCompany.SelectedValue));
            grdViewArtworkDetails.DataSource = listitem;
            grdViewArtworkDetails.DataBind();
            modalpnlselectartwork.Show();
        }
    }
    protected void nestedGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Download")
        {
            string ThumbName = string.Empty;
            String Filename = e.CommandArgument.ToString();
            ThumbName = Server.MapPath("~/UploadedImages/Artwork/Thumbs/") + Filename;
            if (!String.IsNullOrEmpty(Filename))
                DownloadFile(ThumbName, Filename);
        }
    }

    protected void nestedGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnFile = (HiddenField)e.Row.FindControl("hdnFile");

            // To display file type icon
            HtmlImage imgFiletype = (HtmlImage)e.Row.FindControl("imgFiletype");
            string ext = Path.GetExtension(hdnFile.Value);
            imgFiletype.Src = "~/Images/FileType/" + ext.Replace(".", "") + ".png";
        }
    }
    protected void grdViewImprint_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblArtworkIDs = (Label)e.Row.FindControl("lblArtworkIDs");
            Label lblLocationNo = (Label)e.Row.FindControl("lblLocationNo");
            GridView gridViewNested = (GridView)e.Row.FindControl("nestedGridView");

            if (!string.IsNullOrEmpty(lblArtworkIDs.Text))
            {
                String itemID = list.Where(i => i.ID == Convert.ToInt64(lblLocationNo.Text)).SingleOrDefault().SelectedArtworkID;
                List<ArtworkDecoratingLibrary> listArt = objArtworkRepo.GetArtworkLibrarybyArtDecoratingID(itemID);
                gridViewNested.DataSource = listArt;
                gridViewNested.DataBind();
            }


            DropDownList ddlId = (DropDownList)e.Row.FindControl("ddlId");
            HiddenField hdnddlLocValue = (HiddenField)e.Row.FindControl("hdnddlLocValue");

            BindLocationsddl(ddlId);
            if (!String.IsNullOrEmpty(hdnddlLocValue.Value))
                ddlId.SelectedValue = hdnddlLocValue.Value;
        }
    }
    #endregion

    #region Page Method
    private void DisplayData()
    {
        DecoratingSpecificationsMaster objlist = objDecoratingRepo.GetDecoratingDetailsByID(this.DecoratingSpecsID);
        if (objlist != null)
        {

        }
    }

    private void BindGridViewList()
    {
        grdViewImprint.DataSource = list;
        grdViewImprint.DataBind();
        tblgrdViewImprint.Visible = true;
    }
    private void BindLocationsddl(DropDownList ddlLoc)
    {
        if (ddlLoc != null)
        {
            try
            {
                ddlLoc.DataSource = objLookRep.GetByLookup("ImprintLocations");
                ddlLoc.DataValueField = "iLookupID";
                ddlLoc.DataTextField = "sLookupName";
                ddlLoc.DataBind();
                ddlLoc.Items.Insert(0, new ListItem("-Select-", "0"));
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
        }
    }
    private void BindDecoratorddl()
    {
        try
        {
            ddlDecorator.DataSource = objLookRep.GetByLookup("Decorator");
            ddlDecorator.DataValueField = "iLookupID";
            ddlDecorator.DataTextField = "sLookupName";
            ddlDecorator.DataBind();
            ddlDecorator.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    /// <summary>
    /// To generate Random of 5 Digit.
    /// </summary>
    /// <returns></returns>
    private Int64 GenerateRandomNumber()
    {
        Random random = new Random();
        return random.Next(10000, 99999);
    }
    private List<ImprintList> GetImprintList(Int32 number)
    {

        for (int i = 1; i <= number; i++)
        {
            ImprintList obj = new ImprintList();
            obj.ID = i;
            obj.LocationNo = "Location :" + i;
            obj.DropDownlist = "ddl" + i.ToString();
            obj.SelectArtwork = "art" + i.ToString();
            obj.ListIcons = "Downloads";
            obj.LocationProof = "locP" + i.ToString();
            obj.SCD = "SCD";
            list.Add(obj);
        }
        return list;

    }
    protected void DownloadFile(string filepath, string displayFileName)
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

                    case ".pdf":
                        type = "Application/pdf";
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
                    case ".bmp":
                        type = "image/bmp";
                        break;
                    case ".ai":
                        type = "application/illustrator";
                        break;
                    case ".epf":
                        type = "application/postscript";
                        break;
                    //left:--rm

                }
            }


            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + displayFileName + ext + "\"");
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
    private void BindDropdown()
    {
        ddlMasterItemNumber.Items.Insert(0, new ListItem("-Select-", "0"));

        CompanyRepository objRepo = new CompanyRepository();
        List<Company> objList = new List<Company>();
        objList = objRepo.GetAllCompany().OrderBy(c => c.CompanyName).ToList();
        Common.BindDDL(ddlCompany, objList, "CompanyName", "CompanyID", "-Select-");

        #region ddlProductType
        ddlProductType.DataSource = objLookRep.GetByLookup("ProductType");
        ddlProductType.DataValueField = "iLookupID";
        ddlProductType.DataTextField = "sLookupName";
        ddlProductType.DataBind();
        ddlProductType.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion

        #region ddlDecoratingMethod
        ddlDecoratingMethod.DataSource = objLookRep.GetByLookup("DecoratingMethod");
        ddlDecoratingMethod.DataValueField = "iLookupID";
        ddlDecoratingMethod.DataTextField = "sLookupName";
        ddlDecoratingMethod.DataBind();
        ddlDecoratingMethod.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion

    }
    #endregion
    [Serializable]
    public class ImprintList
    {
        public Int64 ID { get; set; }
        public Int32 ddlLocSelectedValue { get; set; }
        public String FileLocationProof { get; set; }
        public Int32 ddlDecoratorSelectedValue { get; set; }
        public String ScreenNoSXL { get; set; }
        public String ScreenNo2XL { get; set; }
        public String ScreenNo5XL { get; set; }
        public String SelectedArtworkID { get; set; }
        public String LocationNo { get; set; }
        public String DropDownlist { get; set; }
        public String SelectArtwork { get; set; }
        public String ListIcons { get; set; }
        public String LocationProof { get; set; }
        public String SCD { get; set; }

    }
}
