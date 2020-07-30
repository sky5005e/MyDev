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
using System.Collections.Generic;
using Incentex.BE;
using Incentex.DA;
public partial class AssetManagement_Inventory_Attachment : PageBase
{
    #region Properties

    Int64 EquipmentInventoryID
    {
        get
        {
            if (ViewState["EquipmentInventoryID"] == null)
            {
                ViewState["EquipmentInventoryID"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentInventoryID"]);
        }
        set
        {
            ViewState["EquipmentInventoryID"] = value;
        }
    }

    AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();
    EquipmentInventoryDocument objInventoryDocument = new EquipmentInventoryDocument();
    Common objcommon = new Common();
    PagedDataSource pds = new PagedDataSource();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {   
        if (!IsPostBack)
        {
            base.MenuItem = "Station Inventory";
            base.ParentMenuID = 46;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Attachment";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/Inventory/Notes.aspx";

            Common.MenuModuleId = Incentex.DAL.Common.DAEnums.ManageID.EquipmentInventory;
            if (Request.QueryString.Count > 0)
            {
                this.EquipmentInventoryID = Convert.ToInt64(Request.QueryString.Get("Id"));
            }

            menuControl.PopulateMenu(5, 0, this.EquipmentInventoryID, 0, false);
            lblMsg.Text = "";

            bindgrid();
        }
    }
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        string sFilePath = null;
        try
        {
            if (!base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            objInventoryDocument.DocumentDescription = txtDescription.Text;
            objInventoryDocument.IsImage = false;
            objInventoryDocument.EquipmentInventoryID = this.EquipmentInventoryID;

            if (((float)Request.Files[0].ContentLength / 1048576) > 2)
            {
                lblMsg.Text = "The file you are uploading is more than 2MB.";
                return;
            }

            if (DocFile.Value != "")
            {
                objInventoryDocument.DocumentName = DocFile.Value;
                sFilePath = Server.MapPath("../../UploadedImages/EquipmentInventoryDoc/") + DocFile.Value;
                objcommon.DeleteImageFromFolder(DocFile.Value, Server.MapPath("../../UploadedImages/EquipmentInventoryDoc/"));
                Request.Files[0].SaveAs(sFilePath);
            }
            objInventoryDocument.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objInventoryDocument.UpdatedDate = DateTime.Now;
            objInventoryRepository.Insert(objInventoryDocument);

            objInventoryRepository.SubmitChanges();
            lblMsg.Text = "Record Saved Succesfully..";
            txtDescription.Text = "";
            bindgrid();
        }
        catch (Exception)
        {


        }
    }
    /// <summary>
    /// Row Command Event of Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEquipment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {


            if (e.CommandName.Equals("Sort"))
            {
                if (this.ViewState["SortExp"] == null)
                {
                    this.ViewState["SortExp"] = e.CommandArgument.ToString();
                    this.ViewState["SortOrder"] = "ASC";
                }
                else
                {
                    if (this.ViewState["SortExp"].ToString() == e.CommandArgument.ToString())
                    {
                        if (this.ViewState["SortOrder"].ToString() == "ASC")
                            this.ViewState["SortOrder"] = "DESC";
                        else
                            this.ViewState["SortOrder"] = "ASC";

                    }
                    else
                    {
                        this.ViewState["SortOrder"] = "ASC";
                        this.ViewState["SortExp"] = e.CommandArgument.ToString();
                    }
                }
            }

            if (e.CommandName == "del")
            {
                if (!base.CanDelete)
                {
                    base.RedirectToUnauthorised();
                }

                objInventoryRepository.DeleteInventoryDoc((Convert.ToInt64(e.CommandArgument)));
                lblMsg.Text = "Record Deleted";
                HiddenField hfImagePath = ((HiddenField)((ImageButton)(e.CommandSource)).Parent.Parent.FindControl("hfDocumentName"));
                objcommon.DeleteImageFromFolder(hfImagePath.Value, Server.MapPath("../../UploadedImages/EquipmentInventoryDoc/"));
            }

            bindgrid();
        }
        catch (Exception)
        {

        }
    }
    /// <summary>
    /// Row Databound event of a grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEquipment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
            {
                Image ImgSort = new Image();
                switch (this.ViewState["SortExp"].ToString())
                {
                    case "DocumentDescription":
                        PlaceHolder placeholderDescription = (PlaceHolder)e.Row.FindControl("placeholderDocumentDescription");
                        break;
                    case "DocumentName":
                        PlaceHolder placeholderDocumentName = (PlaceHolder)e.Row.FindControl("placeholderDocumentName");
                        break;
                    case "Date":
                        PlaceHolder placeholderDate = (PlaceHolder)e.Row.FindControl("placeholderDate");
                        break;

                }

            }

        }
        catch (Exception)
        {


        }
    }
    public void bindgrid()
    {
        DataView myDataView = new DataView();

        DataTable dt = new DataTable();

        try
        {

            dt = ListToDataTable(objInventoryRepository.GetInventoryAttachment(this.EquipmentInventoryID));
            if (dt.Rows.Count == 0)
            {
                dvTotalRecords.Visible = false;
                pagingtable.Visible = false;
            }
            else
            {
                dvTotalRecords.Visible = true;
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
            pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
            pds.CurrentPageIndex = CurrentPage;
            lnkbtnNext.Enabled = !pds.IsLastPage;
            lnkbtnPrevious.Enabled = !pds.IsFirstPage;

            gvEquipment.DataSource = pds;
            gvEquipment.DataBind();
            doPaging();

        }
        catch (Exception)
        {


        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        Response.Redirect("Image.aspx?Id=" + this.EquipmentInventoryID);
    }
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
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
    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindgrid();
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
        bindgrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        bindgrid();
    }
    #endregion
}
