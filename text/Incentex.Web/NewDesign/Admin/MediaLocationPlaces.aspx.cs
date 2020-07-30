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
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.IO;
using System.Collections.Generic;

public partial class NewDesign_Admin_MediaLocationPlaces : System.Web.UI.Page
{
    #region Properties

    String SiteURL
    {
        get
        {
            return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["siteurl"].ToString());
        }
    }


    public Int32 NoOfRecordsToDisplay
    {
        get
        {
            if (Convert.ToString(this.ViewState["NoOfRecordsToDisplay"]) == "")
                return Convert.ToInt32(Application["NEWPAGESIZE"]);
            else
                return Convert.ToInt32(this.ViewState["NoOfRecordsToDisplay"]);
        }
        set
        {
            this.ViewState["NoOfRecordsToDisplay"] = value;
        }
    }

    public Int32 PageIndex
    {
        get
        {
            if (Convert.ToString(this.ViewState["PageIndex"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["PageIndex"]);
        }
        set
        {
            this.ViewState["PageIndex"] = value;
        }
    }

    public String SortColumn
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortColumn"]) == String.Empty)
                return "EmployeeName";
            else
                return Convert.ToString(this.ViewState["SortColumn"]);
        }
        set
        {
            this.ViewState["SortColumn"] = value;
        }
    }

    public String SortDirection
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortDirection"]) == String.Empty)
                return "ASC";
            else
                return Convert.ToString(this.ViewState["SortDirection"]);
        }
        set
        {
            this.ViewState["SortDirection"] = value;
        }
    }

    public Int32 TotalPages
    {
        get
        {
            if (Convert.ToString(this.ViewState["TotalPages"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["TotalPages"]);
        }
        set
        {
            this.ViewState["TotalPages"] = value;
        }
    }

    public Int32 NoOfPagesToDisplay
    {
        get
        {
            if (Convert.ToString(this.ViewState["NoOfPagesToDisplay"]) == "")
                return Convert.ToInt32(Application["NEWPAGERSIZE"]);
            else
                return Convert.ToInt32(this.ViewState["NoOfPagesToDisplay"]);
        }
        set
        {
            this.ViewState["NoOfPagesToDisplay"] = value;
        }
    }

    public Int32 FromPage
    {
        get
        {
            if (Convert.ToString(this.ViewState["FromPage"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["FromPage"]);
        }
        set
        {
            this.ViewState["FromPage"] = value;
        }
    }

    public Int32 ToPage
    {
        get
        {
            if (Convert.ToString(this.ViewState["ToPage"]) == "")
                return this.NoOfPagesToDisplay;
            else
                return Convert.ToInt32(this.ViewState["ToPage"]);
        }
        set
        {
            this.ViewState["ToPage"] = value;
        }
    }


    public String Placement
    {
        get
        {
            if (Convert.ToString(this.ViewState["Placement"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["Placement"]);
        }
        set
        {
            this.ViewState["Placement"] = value;
        }
    }

    public String ModuleName
    {
        get
        {
            if (Convert.ToString(this.ViewState["ModuleName"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["ModuleName"]);
        }
        set
        {
            this.ViewState["ModuleName"] = value;
        }
    }




    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) && IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            {
                Response.Redirect("unauthorised.aspx");
                
            }

            if (string.IsNullOrEmpty(Request.QueryString["id"]) || string.IsNullOrEmpty(Request.QueryString["type"]))
                Response.Redirect("medialocation.aspx");

            this.PageIndex = 1;
            BindGrid(false);
            GetPageTitle();
         
        }
    }

    protected void gvPlaceName_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Sort")
            {
                if (this.SortColumn == null)
                {
                    this.SortColumn = Convert.ToString(e.CommandArgument);
                    this.SortDirection = "ASC";
                }
                else
                {
                    if (this.SortColumn == Convert.ToString(e.CommandArgument))
                    {
                        if (this.SortDirection == "ASC")
                            this.SortDirection = "DESC";
                        else
                            this.SortDirection = "ASC";
                    }
                    else
                    {
                        this.SortDirection = "ASC";
                        this.SortColumn = Convert.ToString(e.CommandArgument);
                    }
                }

                BindGrid(true);
            }

            if (e.CommandName == "EditRecord")
            {
                MediaRepository ObjMediaRepostiroty = new MediaRepository();
                ltPopupTitle.Text = "Edit Place";
                string strarr = e.CommandArgument.ToString();
                hfModuleId.Value = strarr;
                //hfModuleType.Value = strarr[1];

                if (Request.QueryString["type"] == "Help Video")
                {
                    MediaHelpVidSub ObjHelpSub = new MediaHelpVidSub();
                    ObjHelpSub = ObjMediaRepostiroty.GetHelpSubById(Convert.ToInt64(strarr));
                    txtModuleName.Text = ObjHelpSub.ModuleName;
                    ddlPlaceType.Text = ObjHelpSub.PageType;
                    txtUrl.Text =ObjHelpSub.ThumbnailURL;
                    if (!string.IsNullOrEmpty(ObjHelpSub.ThumbnailImage))
                    {

                        ShowImage.HRef = "~/newdesign/StaticContents/img/ThumbImages/" + ObjHelpSub.ThumbnailImage;
                        ShowImage.Visible = true;
                    }
                    else
                    {
                        ShowImage.Visible = false;
                    }

                        

                    OpenPopup("add-module-Block", "employess-content");
                }
                else if (Request.QueryString["type"] == "Document Link")
                {
                    MediadocLinkSub ObjDocSub = new MediadocLinkSub();

                    ObjDocSub = ObjMediaRepostiroty.GetLinkSubById(Convert.ToInt64(strarr));
                    txtModuleName.Text = ObjDocSub.ModuleName;
                    ddlPlaceType.Text = ObjDocSub.PageType;
                    txtUrl.Text = ObjDocSub.ThumbnailURL;

                    if (!string.IsNullOrEmpty(ObjDocSub.ThumbnailImage))
                    {
                        ShowImage.HRef = "~/newdesign/StaticContents/img/ThumbImages/" + ObjDocSub.ThumbnailImage;
                        ShowImage.Visible = true;
                    }
                    else
                    {
                        ShowImage.Visible = false;
                    }

                   
                    OpenPopup("add-module-Block", "employess-content");
                }
                else if (Request.QueryString["type"] == "Login Pop-up")
                {
                    //ShowAlertMessage("Can not edit this record.");

                    MediaLoginpopup objLogin = new MediaLoginpopup();

                    objLogin = ObjMediaRepostiroty.GetLoginPopupbyid(Convert.ToInt64(strarr));
                    txtModuleName.Text = objLogin.ModuleName;
                    ddlPlaceType.Text = objLogin.PageType;
                    txtUrl.Text = objLogin.ThumbnailURL;


                    if (!string.IsNullOrEmpty(objLogin.ThumbnailImage))
                    {
                        ShowImage.HRef = "~/newdesign/StaticContents/img/ThumbImages/" + objLogin.ThumbnailImage;
                        ShowImage.Visible = true;
                    }
                    else
                    {
                        ShowImage.Visible = false;
                    }
                    
                    OpenPopup("add-module-Block", "employess-content");
                }
            }
            


        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }


    #region Paging Events


    protected void dtlPaging_ItemCommand(Object sender, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ChangePage")
            {
                this.PageIndex = Convert.ToInt32(e.CommandArgument);
                BindGrid(false);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dtlPaging_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            LinkButton lnkPaging = (LinkButton)e.Item.FindControl("lnkPaging");

            if (lnkPaging.Text == Convert.ToString(this.PageIndex))
            {
                lnkPaging.Enabled = false;
                lnkPaging.Font.Bold = true;
                lnkPaging.Attributes.Add("style", "cursor:pointer;");
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkPrevious_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex -= 1;
            BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkNext_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex += 1;
            BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkViewAll_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex = 1;
            this.NoOfRecordsToDisplay = 0;
            BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Methods

    private void ShowAlertMessage(string message)
    {
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "GeneralAlertMsg('" + message + "');", true);
    }


    public string GetMediaImage(string filename)
    {
        //mm
        string img = "";
        if(!string.IsNullOrEmpty(filename))
                    img = "<a class='ajax'  href='../staticcontents/img/ThumbImages/"+ filename +"'> <img src='../StaticContents/img/search.png' width='30px' height='30px' /></a>";
        
        return img;
    }

    private void OpenPopup(string mainid, string contentid)
    {
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowPopUp('" + mainid + "');", true);
    }




   
    public void ResizeImage(string path, string newfilepath, string fileName, string newfilename, int width, int height)
    {
        //fileName = "indexpage.png";
        int NewWidth = width;
        int NewHeight = height;
        // FileStream fs = File.Open(, FileMode.Open);

        StreamReader sr = new StreamReader(path + fileName);
        System.Drawing.Bitmap bmpOut = new System.Drawing.Bitmap(NewWidth, NewHeight);
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmpOut);
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.FillRectangle(System.Drawing.Brushes.White, 0, 0, NewWidth, NewHeight);
        g.DrawImage(new System.Drawing.Bitmap(sr.BaseStream), 0, 0, NewWidth, NewHeight);
        MemoryStream stream = new MemoryStream();
        bmpOut.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        String saveImagePath = newfilepath + newfilename;
        bmpOut.Save(saveImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        g.Dispose();
        bmpOut.Dispose();
        sr.Dispose();
    }

    //private void ShowAlertMessage(string message)
    //{
    //    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "GeneralAlertMsg('" + message + "');", true);
    //}

    //private void OpenPopup(string mainid, string contentid)
    //{
    //    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowPopUp('" + mainid + "','" + contentid + "');", true);
    //}

    private void GeneratePaging()
    {
        try
        {
            Int32 tCurrentPg = this.PageIndex;
            Int32 tToPg = this.ToPage;
            Int32 tFrmPg = this.FromPage;

            if (tCurrentPg > tToPg)
            {
                tFrmPg = tToPg + 1;
                tToPg += this.NoOfPagesToDisplay;
            }

            if (tCurrentPg < tFrmPg)
            {
                tToPg = tFrmPg - 1;
                tFrmPg -= this.NoOfPagesToDisplay;
            }

            if (this.TotalPages - tToPg == 1 && (tToPg + 1) - tFrmPg <= (NoOfPagesToDisplay - 1))
            {
                tToPg = tToPg + 1;
            }

            if (tToPg > this.TotalPages)
                tToPg = this.TotalPages;

            this.ToPage = tToPg;
            this.FromPage = tFrmPg;

            DataTable dt = new DataTable();
            dt.Columns.Add("Index");

            for (Int32 i = tFrmPg; i <= tToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Index"] = i;
                dt.Rows.Add(dr);
            }

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void BindGrid(Boolean isForSorting)
    {
        try
        {
            MediaRepository MediaRepo = new MediaRepository();
            List<GetMediaLocationPagesResult> LstMediaLocationPlaces = MediaRepo.GetMediaLocationPages(Convert.ToInt64(Request.QueryString["id"]), this.ModuleName,Request.QueryString["type"].ToString(),this.NoOfRecordsToDisplay, this.PageIndex, this.SortColumn, this.SortDirection);
            
            if (LstMediaLocationPlaces != null && LstMediaLocationPlaces.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(LstMediaLocationPlaces[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;
            }

            gvPlaceName.DataSource = LstMediaLocationPlaces;
            gvPlaceName.DataBind();

            if (!isForSorting)
            {
                //this.FromPage = TotalPages;
                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 1;
                lnkNext.Enabled = this.PageIndex < this.TotalPages;
            }

            if (gvPlaceName.Rows.Count > 0)
            {
                totalcount_em.InnerText = LstMediaLocationPlaces[0].TotalRecords + " results";
                pagingtable.Visible = true;
            }
            else
            {
                totalcount_em.InnerText = "no results";
                pagingtable.Visible = false;
            }

            totalcount_em.Visible = true;


        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }


    public void GetPageTitle()
    {
        MediaRepository ObjMediaRepostiroty = new MediaRepository();
        if (Request.QueryString["type"].ToString() == "Document Link")
        {
            MediaDocLinkParent ObjDoc = new MediaDocLinkParent();
            ObjDoc = ObjMediaRepostiroty.GetModuleNameForDocById(Convert.ToInt64(Request.QueryString["id"]));
            ltPlaces.Text ="Places For " + ObjDoc.ModuleName;

        }
        else if (Request.QueryString["type"].ToString() == "Help Video")
        {
            MediaHelpVidPrnt ObjHelp = new MediaHelpVidPrnt();
            ObjHelp = ObjMediaRepostiroty.GetModuleNameForVideoById(Convert.ToInt64(Request.QueryString["id"]));
            ltPlaces.Text ="Places For " + ObjHelp.ModuleName;
        }
        else if (Request.QueryString["type"].ToString() == "Login Pop-up")
        {
            ltPlaces.Text ="Places For Login Pop-up";
        }

    }


    #endregion

    protected void btnSearchMedia_Click(object sender, EventArgs e)
    {
        NoOfPagesToDisplay = Convert.ToInt32(Application["NEWPAGERSIZE"]);
        NoOfRecordsToDisplay = Convert.ToInt32(Application["NEWPAGESIZE"]);
        TotalPages = 1;
        FromPage = 1;
        ToPage = NoOfPagesToDisplay;

        //this.Placement = ddlSearchPlacement.SelectedValue == "0" ? string.Empty : ddlSearchPlacement.SelectedItem.Text;
        this.ModuleName = txtSearchModuleName.Text.Trim() == "" ? string.Empty : txtSearchModuleName.Text;
        this.PageIndex = 1;
        BindGrid(false);

    }
    protected void lbAddPopupAddModule_Click(object sender, EventArgs e)
    {
        MediaRepository ObjRepos = new MediaRepository();
        
        switch (Request.QueryString["type"].ToString())
        {
            case "Help Video":
                if (ObjRepos.IsPlaceAlredyEdisted(Convert.ToInt64(hfModuleId.Value), Convert.ToInt64(Request.QueryString["id"]), txtModuleName.Text, "Help Video"))
                {
                    ShowAlertMessage("Place name is already existed");
                    OpenPopup("add-module-Block", "employess-content");
                    return;
                }
                MediaHelpVidSub objHelpsubVid;
          
                if (hfModuleId.Value == "0")
                {
                    objHelpsubVid = new MediaHelpVidSub();
                }
                else
                {
                    objHelpsubVid = ObjRepos.GetHelpSubById(Convert.ToInt64(hfModuleId.Value));
                }
                objHelpsubVid.ModuleName = txtModuleName.Text;
                objHelpsubVid.ThumbnailURL = txtUrl.Text;
                objHelpsubVid.MediaHelpVidPrntID = Convert.ToInt64(Request.QueryString["id"]);
               

                objHelpsubVid.PageType = ddlPlaceType.SelectedValue=="0"?"":ddlPlaceType.Text;
                
                if (fuscreenimage.HasFile == true)
                {
                   string filename=System.DateTime.Now.Ticks.ToString() + "_" + fuscreenimage.FileName;
                   string thumbimage = "600x800_" + filename;
                   fuscreenimage.SaveAs(Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/") + filename);
                   ResizeImage(Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/"), Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/"), filename, thumbimage, 680, 800);
                   objHelpsubVid.ThumbnailImage = thumbimage;

                }
                if (hfModuleId.Value == "0")
                    ObjRepos.Insert(objHelpsubVid);
                    
                ObjRepos.SubmitChanges();
              
                ShowAlertMessage("Record has been updated successfully.");
                BindGrid(false);


                break;
           case "Document Link":
                if (ObjRepos.IsPlaceAlredyEdisted(Convert.ToInt64(hfModuleId.Value), Convert.ToInt64(Request.QueryString["id"]), txtModuleName.Text, "Document Link"))
                {
                    ShowAlertMessage("Place name is already existed");
                    OpenPopup("add-module-Block", "employess-content");
                    return;
                }

                MediadocLinkSub objDocsub;
                if (hfModuleId.Value == "0")
                {
                    objDocsub = new MediadocLinkSub();
                }
                else
                {
                    objDocsub = ObjRepos.GetLinkSubById(Convert.ToInt64(hfModuleId.Value));
                }
                objDocsub.ModuleName = txtModuleName.Text;
                objDocsub.ThumbnailURL = txtUrl.Text;
                objDocsub.PageType = ddlPlaceType.SelectedValue == "0" ? "" : ddlPlaceType.Text; 
                objDocsub.MediaDocumentLinkId = Convert.ToInt64(Request.QueryString["id"]);


                if (fuscreenimage.HasFile == true)
                {
                    string filename = System.DateTime.Now.Ticks.ToString() + "_" + fuscreenimage.FileName;
                    string thumbimage = "427x570_" + filename;
                    fuscreenimage.SaveAs(Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/") + filename);
                    ResizeImage(Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/"), Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/"), filename, thumbimage, 680, 800);
                    objDocsub.ThumbnailImage = thumbimage;
                }
                if (hfModuleId.Value == "0")
                    ObjRepos.Insert(objDocsub);
                ObjRepos.SubmitChanges();
              
                ShowAlertMessage("Record has been updated successfully.");
                BindGrid(false);
                break;

           case "Login Pop-up":
                if (ObjRepos.IsPlaceAlredyEdisted(Convert.ToInt64(hfModuleId.Value), Convert.ToInt64(Request.QueryString["id"]), txtModuleName.Text, "Login Pop-up"))
                {
                    ShowAlertMessage("Place name is already existed");
                    OpenPopup("add-module-Block", "employess-content");
                    return;
                }
                MediaLoginpopup ObjLoginPopup;
                if (hfModuleId.Value == "0")
                {
                    ObjLoginPopup = new MediaLoginpopup();
                }
                else
                {
                    ObjLoginPopup = ObjRepos.GetLoginPopupbyid(Convert.ToInt64(hfModuleId.Value));
                }
                ObjLoginPopup.ModuleName = txtModuleName.Text;
                ObjLoginPopup.ThumbnailURL = txtUrl.Text;
                ObjLoginPopup.PageType = ddlPlaceType.SelectedValue == "0" ? "" : ddlPlaceType.Text; 

                if (fuscreenimage.HasFile == true)
                {
                    string filename = System.DateTime.Now.Ticks.ToString() + "_" + fuscreenimage.FileName;
                    string thumbimage = "427x570_" + filename;
                    fuscreenimage.SaveAs(Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/") + filename);
                    ResizeImage(Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/"), Server.MapPath("~/NewDesign/staticcontents/img/ThumbImages/"), filename, thumbimage, 680, 800);
                    ObjLoginPopup.ThumbnailImage = thumbimage;
                }
                if (hfModuleId.Value == "0")
                    ObjRepos.Insert(ObjLoginPopup);
                ObjRepos.SubmitChanges();
               
                ShowAlertMessage("Record has been updated successfully.");
                BindGrid(false);
                break;
        }

    }
}
