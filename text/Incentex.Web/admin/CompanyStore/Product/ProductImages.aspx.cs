using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_ProductManagement_ProductImages : PageBase
{
    #region Data Members

    Int64 iStoreID
    {
        get
        {
            if (ViewState["iStoreID"] == null)
            {
                ViewState["iStoreID"] = 0;
            }
            return Convert.ToInt64(ViewState["iStoreID"]);
        }
        set
        {
            ViewState["iStoreID"] = value;
        }
    }

    Int64 StoreProductID
    {
        get
        {
            if (ViewState["StoreProductID"] == null)
            {
                ViewState["StoreProductID"] = 0;
            }
            return Convert.ToInt64(ViewState["StoreProductID"]);
        }
        set
        {
            ViewState["StoreProductID"] = value;
        }
    }

    Int32 MasterItemNo
    {
        get
        {
            if (ViewState["MasterItemNo"] == null)
            {
                ViewState["MasterItemNo"] = 0;
            }
            return Convert.ToInt32(ViewState["MasterItemNo"]);
        }
        set
        {
            ViewState["MasterItemNo"] = value;
        }
    }

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

    Boolean bolflag = false;
    StoreProductImage objStoreProdImage = new StoreProductImage();
    ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
    List<StoreProductImage> objStoreProductImage = new List<StoreProductImage>();
    StoreProductImageRepository objRepos = new StoreProductImageRepository();
    Common objcommon = new Common();
    String sFilePath, sFilePathLarge;
    String sFileName, sFileNameLarge;
    LookupRepository objLookRep = new LookupRepository();

    #endregion

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        Int32 ManageID = (Int32)Incentex.DAL.Common.DAEnums.ManageID.CompanyProduct;
        Session["ManageID"] = ManageID;

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

            if (Request.QueryString["Id"] != null)
                iStoreID = Convert.ToInt64(Request.QueryString["Id"].ToString());

            if (Request.QueryString["SubId"] != null && Request.QueryString["SubId"] != "0" && Request.QueryString["SubId"] != "")
            {
                StoreProductID = Convert.ToInt64(Request.QueryString["SubId"].ToString());
                menuControl.PopulateMenu(5, 0, StoreProductID, iStoreID, false);
            }
            else
                Response.Redirect("General.aspx?SubId=" + Request.QueryString["SubId"] + "&Id=" + Request.QueryString["Id"]);

            if (Request.QueryString["MasterItemNo"] != null)
                MasterItemNo = Convert.ToInt32(Request.QueryString["MasterItemNo"].ToString());
            else
            {
                ProductItem objprodItem = new ProductItem();
                objprodItem = objRepository.GetStoreProductMasterItemNo(Convert.ToInt32(StoreProductID));
                MasterItemNo = Convert.ToInt32(objprodItem.MasterStyleID);
            }

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Images" + WorkGroupNameToDisplay;
            ((HtmlGenericControl)menuControl.FindControl("dvSubMenu")).Visible = false;
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.ProductReturnURL;

            StyleNo.Visible = false;
            FillMasterItemNumber();
            if (MasterItemNo != 0)
            {
                ddlMasterItemNo.SelectedValue = MasterItemNo.ToString();
                ddlMasterItemNo.Enabled = false;
            }

            bindListView();
        }
    }

    /// <summary>
    /// Delete the Images path in database and from images in folder
    /// Upload Images/Product Images
    /// Bind the Listview
    /// Nagmani Kumar 16/10/2010
    /// </summary>
    protected void dtProductImages_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteSplashImage")
            {
                if (!base.CanDelete)
                {
                    base.RedirectToUnauthorised();
                }

                objRepos.Delete(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);
                //Deleting Thumb Image
                //objcommon.DeleteImageFromFolder(((HiddenField)e.Item.FindControl("hdndocumentname")).Value, IncentexGlobal.StoreProductThumbImagepath);
                //Deleting Larger Image
                //objcommon.DeleteImageFromFolder(((HiddenField)e.Item.FindControl("hdnlargerimagename")).Value, IncentexGlobal.StoreProductImagepath);
            }

            bindListView();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Show Images in List view data is bound from the folder 
    /// Upload Images/Product Images
    /// Bind the Listview
    /// Nagmani Kumar 16/10/2010
    /// </summary>
    protected void dtProductImages_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((HtmlImage)e.Item.FindControl("imgSplashImage")).Src = "../../../UploadedImages/ProductImages/Thumbs/" + ((HiddenField)e.Item.FindControl("hdndocumentname")).Value;
                ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "../../../UploadedImages/ProductImages/" + ((HiddenField)e.Item.FindControl("hdnlargerimagename")).Value;

                String strProdactive = ((HiddenField)e.Item.FindControl("hdnProductImageActive")).Value;
                RadioButton rbImage = (RadioButton)e.Item.FindControl("rbImages");

                if (strProdactive == "1")
                    rbImage.Checked = true;
                else
                    rbImage.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlMasterItemNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            objStoreProductImage = objRepos.GetStoreProductImagesdByMasterID(Convert.ToInt64(ddlMasterItemNo.SelectedValue), StoreProductID);

            if (objStoreProductImage.Count == 5)
            {
                lnkBtnUploadWorkgroup.Visible = false;
                tdUpload.Visible = false;
            }
            else
            {
                lnkBtnUploadWorkgroup.Visible = true;
                tdUpload.Visible = true;
            }

            dtProductImages.DataSource = objStoreProductImage;
            dtProductImages.DataBind();

            lblMessage.Text = "";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void rbImages_CheckedChanged(object sender, EventArgs e)
    {
        RadioButton RBtn = (RadioButton)sender;
        DataListItem item = (DataListItem)RBtn.Parent;
        LinkButton lnkImageId = new LinkButton();

        RBtn = (RadioButton)item.FindControl("rbImages");
        if (RBtn.Checked == true)
        {
            Int32 activated = 0;
            Int32 status = 1;
            Int32 masteritenno = Convert.ToInt32(ddlMasterItemNo.SelectedValue);
            objRepos.UpdateBeforeActive(activated, status, StoreProductID);
            String primaryimageid = ((HiddenField)item.FindControl("hdnImageId")).Value;
            Int32 active = 1;
            objRepos.Update(primaryimageid, active);
            bindListView();

            if (ddlMasterItemNo.SelectedIndex > 0)
                ddlMasterItemNo_SelectedIndexChanged(sender, e);
        }
    }

    /// <summary>
    /// Save the Images path in database and save images in folder
    /// Upload Images/Product Images
    /// the Images after the Upload the Images
    /// Bind the Listview
    /// Nagmani Kumar 16/10/2010
    /// </summary>
    protected void lnkBtnUploadWorkgroup_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            StoreProductRepository objSPRep = new StoreProductRepository();
            StoreProduct obj = new StoreProduct();
            List<SelectProductImageMasterItemStyleDuplicateResult> objImageMaster = new List<SelectProductImageMasterItemStyleDuplicateResult>();
            obj = objSPRep.GetById(Convert.ToInt32(StoreProductID));
            Int32 storeid = Convert.ToInt32(obj.StoreId);
            String modeAdd = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.Add);
            objImageMaster = objRepos.CheckDuplicateMasterItem(Convert.ToInt32(StoreProductID), Convert.ToInt32(ddlMasterItemNo.SelectedValue), 0, storeid, modeAdd);

            if (objImageMaster[0].IsDuplicate == 0)
            {
                Int32 intMasterItemNo = 0;
                if (objImageMaster[0].MasterItemNo != null)
                {
                    intMasterItemNo = Convert.ToInt32(objImageMaster[0].MasterItemNo);
                }

                if (objImageMaster[0].MasterItemNo == null || intMasterItemNo == Convert.ToInt32(ddlMasterItemNo.SelectedValue))
                {
                    sFileName = "Thumb_" + System.DateTime.Now.Ticks + "_" + fpUpload.Value;
                    sFileName = Common.MakeValidFileName(sFileName);
                    sFilePath = Server.MapPath("../../../UploadedImages/ProductImages/Thumbs/") + sFileName;
                    Request.Files[0].SaveAs(sFilePath);

                    sFileNameLarge = "Large_" + System.DateTime.Now.Ticks + "_" + fpUploadLargerImage.Value;
                    sFileNameLarge = Common.MakeValidFileName(sFileNameLarge);
                    sFilePathLarge = Server.MapPath("../../../UploadedImages/ProductImages/") + sFileNameLarge;
                    Request.Files[1].SaveAs(sFilePathLarge);

                    //Assign Thumb Image value
                    if (fpUpload.Value != null)
                        objStoreProdImage.ProductImage = sFileName;
                    else
                        objStoreProdImage.ProductImage = null;

                    //Assign Larger Image
                    if (fpUploadLargerImage.Value != null)
                        objStoreProdImage.LargerProductImage = sFileNameLarge;
                    else
                        objStoreProdImage.LargerProductImage = null;
                    //Select storeid from storeproduct table

                    objStoreProdImage.StoreID = obj.StoreId;
                    //
                    objStoreProdImage.StyleId = Convert.ToInt64(ddlMasterItemNo.SelectedValue);
                    objStoreProdImage.StoreProductID = StoreProductID;
                    objRepos.Insert(objStoreProdImage);
                    objRepos.SubmitChanges();

                    //
                    bindListView();
                    bolflag = true;
                    lblMessage.Text = "";
                }
                else
                {
                    bindListView();
                    bolflag = false;
                    SetFocus(ddlMasterItemNo);
                    lblMessage.Text = "Master item number will be same for one product!";
                    return;
                }
            }
            else
            {
                bolflag = false;
                SetFocus(ddlMasterItemNo);
                return;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Methods
    /// <summary>
    /// bindListView()
    /// Bind the Listview
    /// Nagmani Kumar 16/10/2010
    /// </summary>
    public void bindListView()
    {
        try
        {
            if (ddlMasterItemNo.SelectedIndex > 0)
                objStoreProductImage = objRepos.GetStoreProductImages(Convert.ToInt32(StoreProductID), MasterItemNo);
            else
                objStoreProductImage = objRepos.GetStoreProductImages(Convert.ToInt32(StoreProductID), MasterItemNo);

            if (objStoreProductImage.Count == 5)
            {
                lnkBtnUploadWorkgroup.Visible = false;
                tdUpload.Visible = false;
            }
            else
            {
                lnkBtnUploadWorkgroup.Visible = true;
                tdUpload.Visible = true;
            }

            dtProductImages.DataSource = objStoreProductImage;
            dtProductImages.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    ///Fill the Style Number dropdownlist
    ///from lookup table
    ///FillStyleNumber()
    /// Nagmani 11/10/2010
    /// </summary>
    public void FillStyleNumber()
    {
        try
        {
            String strStatus = "StyleNo";
            ddlItemStyle.DataSource = objLookRep.GetByLookup(strStatus);
            ddlItemStyle.DataValueField = "iLookupID";
            ddlItemStyle.DataTextField = "sLookupName";
            ddlItemStyle.DataBind();
            ddlItemStyle.Items.Insert(0, new ListItem("-select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    ///Fill the Master Item Number dropdownlist
    ///from lookup table
    ///FillMasterItemNumber()
    /// Nagmani 11/10/2010
    /// </summary>
    public void FillMasterItemNumber()
    {
        try
        {
            String strStatus = "MasterItemNumber";
            ddlMasterItemNo.DataSource = objLookRep.GetByLookup(strStatus);
            ddlMasterItemNo.DataValueField = "iLookupID";
            ddlMasterItemNo.DataTextField = "sLookupName";
            ddlMasterItemNo.DataBind();
            ddlMasterItemNo.Items.Insert(0, new ListItem("-select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}