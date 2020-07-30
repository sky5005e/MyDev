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
using System.IO;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL;

public partial class NewDesign_Admin_MediaLocation : System.Web.UI.Page
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
                //return 18;

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

            FillDropDowns();
            this.PageIndex = 1;
            BindGrid(false);
        }
    }

    protected void gvModuleName_RowCommand(Object sender, GridViewCommandEventArgs e)
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
                ltPopupTitle.Text = "Edit Module";
                string[] strarr = e.CommandArgument.ToString().Split('_');
                hfModuleId.Value = strarr[0];
                hfModuleType.Value = strarr[1];

                if (strarr[1] == "Help Video")
                {
                    MediaHelpVidPrnt ObjHelp = new MediaHelpVidPrnt();
                    ObjHelp = ObjMediaRepostiroty.GetModuleNameForVideoById(Convert.ToInt64(strarr[0]));
                    txtModuleName.Text = ObjHelp.ModuleName;
                    ddlPlacement.SelectedValue = ddlPlacement.Items.FindByText("Help Video").Value;
                    OpenPopup("add-module-Block", "employess-content");


                }
                else if (strarr[1] == "Document Link")
                {
                    MediaDocLinkParent ObjDoc = new MediaDocLinkParent();
                    ObjDoc = ObjMediaRepostiroty.GetModuleNameForDocById(Convert.ToInt64(strarr[0]));
                    txtModuleName.Text = ObjDoc.ModuleName;
                    ddlPlacement.SelectedValue = ddlPlacement.Items.FindByText("Document Link").Value;
                    OpenPopup("add-module-Block", "employess-content");
                }
                else if (strarr[1] == "Login Pop-up")
                {
                    ShowAlertMessage("Can not edit this record.");
                }
            }
            if (e.CommandName == "ShowPlace")
            {
                string[] strarr = e.CommandArgument.ToString().Split('_');
                hfModuleId.Value = strarr[0];
                hfModuleType.Value = strarr[1];
                Response.Redirect("MediaLocationPlaces.aspx?id=" + strarr[0] + "&type=" + strarr[1],false);
                //Server.Execute("MediaLocationPlaces.aspx?id=" + strarr[0] + "&type=" + strarr[1]);
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

    private void OpenPopup(string mainid, string contentid)
    {
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowPopUp('" + mainid + "');", true);
    }


    private void FillDropDowns()
    {
        MediaRepository OBJ = new MediaRepository();
        List<MediaPlaceMent> lstplacement = OBJ.GetPlacement();
        BindDropDown(ddlSearchPlacement, lstplacement, "PlacementName", "MediaPlaceMentId", "Placement", "0");
        BindDropDown(ddlPlacement, lstplacement.Where(x => x.PlacementName != "Login Pop-up"), "PlacementName", "MediaPlaceMentId", "Placement", "0");
    }

    private void BindDropDown(DropDownList ddlDropDown, Object lstValues, String textField, String valueField, String selectLabel, String selectedValue)
    {
        try
        {
            ddlDropDown.DataSource = lstValues;
            ddlDropDown.DataTextField = textField;
            ddlDropDown.DataValueField = valueField;
            ddlDropDown.DataBind();

            ddlDropDown.Items.Insert(0, new ListItem("- " + selectLabel + " -", "0"));
            ddlDropDown.SelectedValue = ddlDropDown.Items.FindByValue(selectedValue) != null ? selectedValue : "0";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
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

            if (this.TotalPages - tToPg == 1 && (tToPg + 1) - tFrmPg <= (NoOfPagesToDisplay-1))
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
            List<GetMediaLocationModulesResult> LstMediaLocation = MediaRepo.GetMediaLocationModules(this.Placement, this.ModuleName, this.NoOfRecordsToDisplay, this.PageIndex, this.SortColumn, this.SortDirection);

            if (LstMediaLocation != null && LstMediaLocation.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(LstMediaLocation[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;
            }

            gvModuleName.DataSource = LstMediaLocation;
            gvModuleName.DataBind();

            if (!isForSorting)
            {
                //this.FromPage = TotalPages;
                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 1;
                lnkNext.Enabled = this.PageIndex < this.TotalPages;
            }

            if (gvModuleName.Rows.Count > 0)
            {
                totalcount_em.InnerText = LstMediaLocation[0].TotalRecords + " results";
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


    #endregion

    protected void btnSearchMedia_Click(object sender, EventArgs e)
    {
        NoOfPagesToDisplay = Convert.ToInt32(Application["NEWPAGERSIZE"]);
        NoOfRecordsToDisplay = Convert.ToInt32(Application["NEWPAGESIZE"]);
        TotalPages = 1;
        FromPage = 1;
        ToPage = NoOfPagesToDisplay;


        this.Placement = ddlSearchPlacement.SelectedValue == "0" ? string.Empty : ddlSearchPlacement.SelectedItem.Text;
        this.ModuleName = txtSearchModuleName.Text.Trim() == "" ? string.Empty : txtSearchModuleName.Text;
        this.PageIndex = 1;
        BindGrid(false);

    }
    protected void lbAddPopupAddModule_Click(object sender, EventArgs e)
    {
        MediaRepository ObjRepos = new MediaRepository();
        switch (ddlPlacement.SelectedItem.Text)
        {
            case "Help Video":
                MediaHelpVidPrnt objHelpVid;
                if (ObjRepos.IsModuleAlreadyExist(Convert.ToInt64(hfModuleId.Value), txtModuleName.Text, "Help Video"))
                {
                    ShowAlertMessage("Module name already existed.");
                    OpenPopup("add-module-Block", "employess-content");
                    return;
                }
                if (hfModuleId.Value == "0")
                {
                    objHelpVid = new MediaHelpVidPrnt();
                    objHelpVid.CreatedId = IncentexGlobal.CurrentMember.UserInfoID;
                    objHelpVid.CreatedDate = System.DateTime.Now.Date;

                }
                else
                {
                    objHelpVid = ObjRepos.GetModuleNameForVideoById(Convert.ToInt64(hfModuleId.Value));
                }

                objHelpVid.ModuleName = txtModuleName.Text;

                if (hfModuleId.Value == "0")
                    ObjRepos.Insert(objHelpVid);
                ObjRepos.SubmitChanges();
                BindGrid(false);
                ShowAlertMessage("Record has been updated successfully.");


                break;
            case "Document Link":
                MediaDocLinkParent objDocLink;
                if (ObjRepos.IsModuleAlreadyExist(Convert.ToInt64(hfModuleId.Value), txtModuleName.Text, "Document Link"))
                {
                    ShowAlertMessage("Module name already existed.");
                    OpenPopup("add-module-Block", "employess-content");
                    return;
                }
                if (hfModuleId.Value == "0")
                {
                    objDocLink = new MediaDocLinkParent();
                    objDocLink.CreatedId = IncentexGlobal.CurrentMember.UserInfoID;
                    objDocLink.CreatedDate = System.DateTime.Now.Date;
                }
                else
                {
                    objDocLink = ObjRepos.GetModuleNameForDocById(Convert.ToInt64(hfModuleId.Value));
                }

                objDocLink.ModuleName = txtModuleName.Text;
                if (hfModuleId.Value == "0")
                    ObjRepos.Insert(objDocLink);
                ObjRepos.SubmitChanges();
                BindGrid(false);
                ShowAlertMessage("Record has been updated successfully.");
                break;
        }

    }
}
