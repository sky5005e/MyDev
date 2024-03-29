﻿using System;
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

public partial class AssetManagement_VendorEmployeeAttachments : PageBase
{
    #region Properties
    
    Int64 VendorID
    {
        get
        {
            if (ViewState["VendorID"] == null)
            {
                ViewState["VendorID"] = 0;
            }
            return Convert.ToInt64(ViewState["VendorID"]);
        }
        set
        {
            ViewState["VendorID"] = value;
        }
    }
    Int64 VendorEmployeeID
    {
        get
        {
            if (ViewState["VendorEmployeeID"] == null)
            {
                ViewState["VendorEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["VendorEmployeeID"]);
        }
        set
        {
            ViewState["VendorEmployeeID"] = value;
        }
    }
    AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
    EquipmentVendorDocument objEquipmentVendorDocument = new EquipmentVendorDocument();
    Common objcommon = new Common();
    PagedDataSource pds = new PagedDataSource();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.MenuModuleId = Incentex.DAL.Common.DAEnums.ManageID.EquipmentVendorEmployee;
            if (Request.QueryString.Count > 0)
            {
                base.MenuItem = "Manage Employee";
                base.ParentMenuID = 50;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                this.VendorID = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.VendorEmployeeID = Convert.ToInt64(Request.QueryString.Get("SubId"));

                ((Label)Master.FindControl("lblPageHeading")).Text = "Vendor Employee Attachments";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/VendorEmployeeNotes.aspx?Id=" + this.VendorID + "&SubId=" + this.VendorEmployeeID; 


                // menuControl.PopulateMenu(3, 0, this.SupplierID, this.SupplierEmployeeID, true);
                menuControl.PopulateMenu(4, 0, this.VendorEmployeeID, this.VendorID, false);

            }
            else
            {
                Response.Redirect("EmployeeList.aspx");
            }

            bindgrid();
        }
    }
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        string sFilePath = null;
        
        try
        {
            objEquipmentVendorDocument.DocumentDescription = txtDescription.Text;
            objEquipmentVendorDocument.DocumentFor = "VendorEmployee";
            objEquipmentVendorDocument.VendorEmployeeID = this.VendorEmployeeID;

            if (((float)Request.Files[0].ContentLength / 1048576) > 2)
            {
                lblMsg.Text = "The file you are uploading is more than 2MB.";
               
                return;
            }

            if (InvoiceDoc.Value != "")
            {
                objEquipmentVendorDocument.FileName = InvoiceDoc.Value;
                sFilePath = Server.MapPath("../UploadedImages/EquipmentVendEmpDoc/") + InvoiceDoc.Value;
                objcommon.DeleteImageFromFolder(InvoiceDoc.Value, Server.MapPath("../UploadedImages/EquipmentVendEmpDoc/"));
                Request.Files[0].SaveAs(sFilePath);
            }
            objEquipmentVendorDocument.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objEquipmentVendorDocument.CreatedDate = DateTime.Now;
            objAssetVendorRepository.Insert(objEquipmentVendorDocument);

            objAssetVendorRepository.SubmitChanges();
            lblMsg.Text = "Record Saved Succesfully..";
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

                objAssetVendorRepository.DeleteVendorDoc(Convert.ToInt64(e.CommandArgument));
                lblMsg.Text = "Record Deleted";
                //objcommon.DeleteImageFromFolder(((HiddenField)e.Item.FindControl("hdnImageFileName")).Value, Server.MapPath("../UploadedImages/EquipmentVendEmpDoc/"));
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
                    case "Description":
                        PlaceHolder placeholderDescription = (PlaceHolder)e.Row.FindControl("placeholderDescription");
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

            dt = ListToDataTable(objAssetVendorRepository.GetVendorEmpDoc(this.VendorEmployeeID));
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
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        Response.Redirect("AccessReports.aspx?SubId=" + this.VendorEmployeeID + "&Id=" + this.VendorID);
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
