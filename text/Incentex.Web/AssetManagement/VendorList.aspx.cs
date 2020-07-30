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
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL.Common;

public partial class AssetManagement_VendorList : PageBase
{
    #region DataMembers
    PagedDataSource pds = new PagedDataSource();
    DataSet ds = new DataSet();
    AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
    Int64? CompanyID
    {
        get
        {
            if (ViewState["CompanyID"] == null)
            {
                return null;
            }
            return Convert.ToInt64(ViewState["CompanyID"]);
        }
        set
        {
            ViewState["CompanyID"] = value;
        }
    }
    Int64? EquipmentVendorID
    {
        get
        {
            if (ViewState["EquipmentVendorID"] == null)
            {
                return null;
            }
            return Convert.ToInt64(ViewState["EquipmentVendorID"]);
        }
        set
        {
            ViewState["EquipmentVendorID"] = value;
        }
    }
    bool IsEmployee
    {
        get
        {
            if (ViewState["IsEmployee"] == null)
            {
                return false;
            }
            return Convert.ToBoolean(ViewState["IsEmployee"]);
        }
        set
        {
            ViewState["IsEmployee"] = value;
        }
    }
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

                base.MenuItem = "Manage Company";
                base.ParentMenuID = 50;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Company List";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Index.aspx";
                }
                else
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/AssetMgtIndex.aspx";
                }
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || (Incentex.DAL.Common.DAEnums.GetUserTypeFor(IncentexGlobal.CurrentMember.Usertype) == "GSEAssetManagement"))
                {
                    CompanyID = Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId);
                }
                if (IncentexGlobal.GSEMgtCurrentMember != null && IncentexGlobal.GSEMgtCurrentMember.VendorID != null)
                {
                    EquipmentVendorID = Convert.ToInt64(IncentexGlobal.GSEMgtCurrentMember.VendorID);
                }
                if (IncentexGlobal.GSEMgtCurrentMember != null && (IncentexGlobal.GSEMgtCurrentMember.CurrentUserType == (long)Incentex.DAL.Common.DAEnums.UserTypes.CustomerEmployee || IncentexGlobal.GSEMgtCurrentMember.CurrentUserType == (long)Incentex.DAL.Common.DAEnums.UserTypes.VendorEmployee))
                {
                    IsEmployee = true;
                }

                bindgrid();

                if (IncentexGlobal.GSEMgtCurrentMember != null)
                {
                    lnkbtnAddCompany.Visible = false;
                    btnDelete.Visible = false;
                }

            }
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
            if (e.CommandName == "ViewEmployee")
            {
                Response.Redirect("~/AssetManagement/EmployeeList.aspx?Id=" + e.CommandArgument.ToString());
            }
            if (e.CommandName == "AddEmployee")
            {

                Response.Redirect("~/AssetManagement/BasicVendorEmpInformation.aspx?Id=" + e.CommandArgument.ToString());
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
                    case "EquipmentVendorName":
                        PlaceHolder placeholderEquipmentID = (PlaceHolder)e.Row.FindControl("placeholderEquipmentVendorName");
                        break;
                    case "Owner":
                        PlaceHolder placeholderOwner = (PlaceHolder)e.Row.FindControl("placeholderOwner");
                        break;
                    case "BaseStation":
                        PlaceHolder placeholderBaseStation = (PlaceHolder)e.Row.FindControl("placeholderBaseStation");
                        break;
                }

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                Label lblEquipmentVendorID = new Label();
                lblEquipmentVendorID = (Label)e.Row.FindControl("lblEquipmentVendorID");
                HiddenField hfBaseStationID = new HiddenField();
                hfBaseStationID = (HiddenField)e.Row.FindControl("hfBaseStationID");
                HiddenField hfCompanyID = new HiddenField();
                hfCompanyID = (HiddenField)e.Row.FindControl("hfCompanyID");
                Button lnkbtnAddEmployee = new Button();
                lnkbtnAddEmployee = (Button)e.Row.FindControl("lnkbtnAddEmployee");
                Button lnkbtnViewEmployee = new Button();
                lnkbtnViewEmployee = (Button)e.Row.FindControl("lnkbtnViewEmployee");

                HyperLink lnkedit = new HyperLink();
                lnkedit = (HyperLink)e.Row.FindControl("lnkedit");
                Label lnkVendorName = new Label();
                lnkVendorName = (Label)e.Row.FindControl("lnkVendorName");
                CheckBox chkDelete = new CheckBox();
                chkDelete = (CheckBox)e.Row.FindControl("chkDelete");
                if (IncentexGlobal.GSEMgtCurrentMember != null)
                {
                    if (IncentexGlobal.GSEMgtCurrentMember.CurrentUserType != (long)Incentex.DAL.Common.DAEnums.UserTypes.VendorSuperAdmin && IncentexGlobal.GSEMgtCurrentMember.CurrentUserType != (long)Incentex.DAL.Common.DAEnums.UserTypes.CustomerSuperAdmin)
                    {
                        lnkedit.Visible = false;
                        lnkVendorName.Visible = true;

                        if (IncentexGlobal.GSEMgtCurrentMember.CurrentUserType == (long)Incentex.DAL.Common.DAEnums.UserTypes.FederalAviationAssociation || this.IsEmployee)
                            lnkbtnAddEmployee.Visible = false;
                    }
                    if (chkDelete != null && Convert.ToInt64(lblEquipmentVendorID.Text) == (long)IncentexGlobal.GSEMgtCurrentMember.VendorID)
                    {
                        chkDelete.Visible = false;
                    }
                }

            }
        }
        catch (Exception)
        {


        }
    }


    /// <summary>
    /// Delete button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnDelete_Click(object sender, EventArgs e)
    {


        try
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            string NotDeleted = "";

            bool IsAnySelected = false;
            foreach (GridViewRow gr in gvEquipment.Rows)
            {
                CheckBox chkDelete = gr.FindControl("chkDelete") as CheckBox;
                Label lblID = gr.FindControl("lblEquipmentVendorID") as Label;


                if (chkDelete.Checked)
                {

                    objAssetVendorRepository.DeleteVendor(Convert.ToInt64(lblID.Text));
                    IsAnySelected = true;
                }
            }

            if (IsAnySelected)
            {
                if (string.IsNullOrEmpty(NotDeleted))
                {
                    lblmsg.Text = "Selected Records Deleted Successfully ...";
                }
                else
                {
                    NotDeleted = NotDeleted.Substring(0, NotDeleted.Length - 1);
                    lblmsg.Text = NotDeleted + " can not be deleted as they contains 1 or more employee.";
                }


            }
            else
            {
                lblmsg.Text = "Please Select Record to delete ...";
            }
            objAssetVendorRepository.SubmitChanges();
            bindgrid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = "Error in deleting record ...";
            ErrHandler.WriteError(ex);
        }


    }
    protected void lnkbtnAddCompany_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        Response.Redirect("BasicVendorInformation.aspx?id=0");
    }
    #endregion
    #region Methods
    public void bindgrid()
    {
        DataView myDataView = new DataView();

        DataTable dt = new DataTable();
        AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
        //UserInformation objuser = new UserInformation();
        //UserInformationRepository objuserrepos = new UserInformationRepository();
        try
        {

            dt = ListToDataTable(objAssetVendorRepository.GetCompanyList(this.EquipmentVendorID, this.CompanyID, this.IsEmployee));
            if (dt.Rows.Count == 0)
            {
                dvTotalRecords.Visible = false;
                pagingtable.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                dvTotalRecords.Visible = true;
                LblTotalRecords.Text = Convert.ToString(dt.Rows.Count);
                pagingtable.Visible = true;
                btnDelete.Visible = true;
            }

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

    /// <summary>
    /// Function which converts List to the DataTable
    /// </summary>
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

    #endregion
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
