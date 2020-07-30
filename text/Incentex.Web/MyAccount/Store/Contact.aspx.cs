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
using Incentex.DAL;
using System.Collections.Generic;
using Incentex.DAL.SqlRepository;
using Incentex.DA;

public partial class admin_CompanyStore_Contact : PageBase
{
    #region Local Property

    DataSet ds;
    CountryDA objCountry = new CountryDA();
    StateDA objState = new StateDA();
    CityDA objCity = new CityDA();
    CompanyStoreDocumentRepository objRep = new CompanyStoreDocumentRepository();
    List<StoreDocument> objStoreDocumentList = new List<StoreDocument>();
    PagedDataSource pds = new PagedDataSource();
    StoreDocument objStoreDocument = new StoreDocument();
    Common objcommon = new Common();

    Int64 CompanyStoreId
    {
        get
        {
            if (ViewState["CompanyStoreId"] == null)
            {
                ViewState["CompanyStoreId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {
            ViewState["CompanyStoreId"] = value;
        }
    }
    Int64 CompanyStoreDocumentId
    {
        get
        {
            if (ViewState["CompanyStoreDocumentId"] == null)
            {
                ViewState["CompanyStoreDocumentId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreDocumentId"]);
        }
        set
        {
            ViewState["CompanyStoreDocumentId"] = value;
        }
    }
    #endregion

    #region Page Load Event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Redirect user to login page if session gets expires
            CheckLogin();
            //Assign Page Header and return URL 
            ((Label)Master.FindControl("lblPageHeading")).Text = "Contact";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/UserManagement.aspx";
            getcontactbystore();

        }
    }
    #endregion

    #region Miscellaneous functions
    public void getcontactbystore()
    {
        DataView myDataView = new DataView();
        this.CompanyStoreId = new CompanyStoreRepository().GetByCompanyId((long)IncentexGlobal.CurrentMember.CompanyId).StoreID;
        hdnWorkgroup.Value = new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).WorkgroupID.ToString();
        lblWG.Text = new LookupRepository().GetById(new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).WorkgroupID).sLookupName;
        List<CompanyStoreDocumentRepository.ContactExtension> objExtList = objRep.GetAllContactsByWorkgroupIDForCA(Convert.ToInt64(hdnWorkgroup.Value),this.CompanyStoreId).ToList();
        if (objExtList.Count == 0)
        {
            pagingtable.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
        }


        DataTable dataTable = ListToDataTable(objExtList);
        myDataView = dataTable.DefaultView;
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
        dtlNews.DataSource = pds;
        dtlNews.DataBind();
        doPaging();
        //objStoreDocumentList = objRep.GetAllGuideLineManuals(this.CompanyStoreId);

    }

    private void ClearControls()
    {
        FillCountry();
        //hdnWorkgroup.Value = new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).WorkgroupID.ToString();
        hdnDepartment.Value = "0";
        //hdnWorkgroup.Value = "0";
        hdPriPhoto.Value = "";
        txtFirstName.Text = string.Empty;
        txtFirstName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtTitle.Text = string.Empty;
        txtCompanyName.Text = string.Empty;
        txtAddress1.Text = string.Empty;
        txtAddress2.Text = string.Empty;
        ddlCountry.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
        ddlCity.SelectedIndex = 0;
        txtZip.Text = string.Empty;
        txtTelephone.Text = string.Empty;
        txtFax.Text = string.Empty;
        txtMobile.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtDepartment.Text = string.Empty;
        txtRoles.Value = string.Empty;

    }

    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null);
            }
            dt.Rows.Add(row);
        }
        return dt;
    }
    #endregion

    #region GridView Events
    protected void dtlNews_RowCommand(object sender, GridViewCommandEventArgs e)
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

        if (e.CommandName == "deletedocument")
        {
            GridViewRow row;
            row = (GridViewRow)((ImageButton)e.CommandSource).Parent.Parent;
            objRep.DeleteCompanyStoreDocument(e.CommandArgument.ToString());
            lblmsgList.Text = "Record deleted successfully";
        }

        if (e.CommandName == "EditContact")
        {
            FillCountry();
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            objStoreDocument = objRep.GetById(Convert.ToInt64(e.CommandArgument.ToString()));
            this.CompanyStoreDocumentId = objStoreDocument.StoreDocumentID;
            hdnWorkgroup.Value = objStoreDocument.WorkgroupID.ToString();
            hdnDepartment.Value = objStoreDocument.DepartmentID.ToString();
            txtFirstName.Text = objStoreDocument.ContactFirstName;
            txtLastName.Text = objStoreDocument.ContactLastName;
            hdPriPhoto.Value = objStoreDocument.ContactUploadImage;
            txtTitle.Text = objStoreDocument.ContactTitle;
            txtCompanyName.Text = objStoreDocument.ContactCompanyName;
            txtAddress1.Text = objStoreDocument.ContactAddress1;
            txtAddress2.Text = objStoreDocument.ContactAddress2;

            ddlCountry.SelectedValue = objStoreDocument.ContactCountryID.ToString();
            ddlCountry_SelectedIndexChanged(sender, e);

            ddlState.SelectedValue = objStoreDocument.ContactStateID.ToString();
            ddlState_SelectedIndexChanged(sender, e);

            ddlCity.Items.FindByValue(objStoreDocument.ContactCityID.ToString()).Selected = true;
            txtZip.Text = objStoreDocument.ContactZip;
            txtTelephone.Text = objStoreDocument.ContactTelephone;
            txtFax.Text = objStoreDocument.ContactFax;
            txtMobile.Text = objStoreDocument.ContactMobile;
            txtEmail.Text = objStoreDocument.ContactEmail;
            txtDepartment.Text = objStoreDocument.ContactDepartment;
            txtRoles.Value = objStoreDocument.ContactUserRoleDescription;
            //e.CommandArgument.ToString();

        }


        getcontactbystore();

    }
    protected void dtlNews_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            switch (this.ViewState["SortExp"].ToString())
            {
                case "Workgroup":
                    PlaceHolder placeholderWorkgroup = (PlaceHolder)e.Row.FindControl("placeholderWorkgroup");
                    break;
                case "Department":
                    PlaceHolder placeholderDepartment = (PlaceHolder)e.Row.FindControl("placeholderDepartment");
                    break;
                case "FirstName":
                    PlaceHolder placeholderFirstName = (PlaceHolder)e.Row.FindControl("placeholderFirstName");
                    break;
                case "LastName":
                    PlaceHolder placeholderLastName = (PlaceHolder)e.Row.FindControl("placeholderLastName");
                    break;

            }

        }
    }
    #endregion

    #region Button Click events
    protected void lnkBtnUploadContact_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.CompanyStoreDocumentId != 0)
            {
                objStoreDocument = objRep.GetById(this.CompanyStoreDocumentId);
            }
            objStoreDocument.WorkgroupID = Convert.ToInt64(hdnWorkgroup.Value);
            objStoreDocument.DepartmentID = Convert.ToInt64(hdnDepartment.Value);
            objStoreDocument.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objStoreDocument.CretedDate = System.DateTime.Now;
            objStoreDocument.DocuemntFor = Incentex.DAL.Common.DAEnums.CompanyStoreDocumentFor.Contact.ToString();
            objStoreDocument.ContactFirstName = txtFirstName.Text;
            objStoreDocument.ContactLastName = txtLastName.Text;
            objStoreDocument.ContactTitle = txtTitle.Text;
            objStoreDocument.ContactCompanyName = txtCompanyName.Text;
            objStoreDocument.ContactAddress1 = txtAddress1.Text;
            objStoreDocument.ContactAddress2 = txtAddress2.Text;
            objStoreDocument.ContactCountryID = Convert.ToInt64(ddlCountry.SelectedItem.Value);
            objStoreDocument.ContactStateID = Convert.ToInt64(ddlState.SelectedItem.Value);
            objStoreDocument.ContactCityID = Convert.ToInt64(ddlCity.SelectedItem.Value);
            objStoreDocument.ContactZip = txtZip.Text;
            objStoreDocument.ContactTelephone = txtTelephone.Text;
            objStoreDocument.ContactFax = txtFax.Text;
            objStoreDocument.ContactMobile = txtMobile.Text;
            objStoreDocument.ContactEmail = txtEmail.Text;
            objStoreDocument.ContactDepartment = txtDepartment.Text;
            objStoreDocument.ContactUserRoleDescription = txtRoles.Value;
            if (hdPriPhoto.Value != "")
                objStoreDocument.ContactUploadImage = hdPriPhoto.Value;
            else
                objStoreDocument.ContactUploadImage = null;
            objStoreDocument.StoreId = this.CompanyStoreId;
            if (this.CompanyStoreDocumentId == 0)
            {
                objRep.Insert(objStoreDocument);
            }
            objRep.SubmitChanges();
            if (this.CompanyStoreDocumentId == 0)
                lblmsgList.Text = "Record added successfully";
            else
                lblmsgList.Text = "Record updated successfully";
            ClearControls();
            getcontactbystore();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void lnkAddContact_Click(object sender, EventArgs e)
    {
        this.CompanyStoreDocumentId = 0;
        lblmsgList.Text = string.Empty;
        ClearControls();
    }
    #endregion

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            getcontactbystore();
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
    public int PagerSize
    {
        get
        {
            if (this.ViewState["PagerSize"] == null)
                return Convert.ToInt32(Application["GRIDPAGERSIZE"]);
            else
                return Convert.ToInt16(this.ViewState["PagerSize"].ToString());
        }
        set
        {
            this.ViewState["PagerSize"] = value;
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
                return PagerSize;
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
                ToPg = ToPg + PagerSize;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - PagerSize;

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
        getcontactbystore();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        getcontactbystore();
    }
    #endregion

    #region City,State,Coutry
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlState.SelectedIndex <= 0)
            {
                ddlCity.Enabled = false;
                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-select city-", "0"));
            }
            else
            {
                ddlCity.Enabled = true;
                ds = objCity.GetCityByStateID(Convert.ToInt32(ddlState.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlCity.DataSource = ds;
                        ddlCity.DataValueField = "iCityID";
                        ddlCity.DataTextField = "sCityName";
                        ddlCity.DataBind();
                        ddlCity.Items.Insert(0, new ListItem("-select city-", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // messagae = ex.Message;
        }
        finally
        {
            //ds.Dispose();
            //objCity = null;
        }
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCountry.SelectedIndex <= 0)
            {
                ddlState.Enabled = false;
                ddlState.Items.Clear();
                ddlState.Items.Add(new ListItem("-select state-", "0"));

                ddlCity.Enabled = false;
                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-select city-", "0"));
            }
            else
            {
                ddlState.Enabled = true;
                ds = objState.GetStateByCountryID(Convert.ToInt32(ddlCountry.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlState.DataSource = ds;
                        ddlState.DataValueField = "iStateID";
                        ddlState.DataTextField = "sStateName";
                        ddlState.DataBind();
                        ddlState.Items.Insert(0, new ListItem("-select state-", "0"));

                        ddlCity.Items.Clear();
                        ddlCity.Items.Add(new ListItem("-select city-", "0"));
                    }
                }

            }

        }
        catch (Exception ex)
        {
            //messagae = ex.Message;
        }
        finally
        {
            //ds.Dispose();
            //objState = null;
        }
    }
    public void FillCountry()
    {
        try
        {
            ds = objCountry.GetCountry();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCountry.DataSource = ds;
                ddlCountry.DataTextField = "sCountryName";
                ddlCountry.DataValueField = "iCountryID";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("-select country-", "0"));
                ddlCountry.SelectedValue = ddlCountry.Items.FindByText("United States").Value;



                ddlState.Enabled = true;
                ds = objState.GetStateByCountryID(Convert.ToInt32(ddlCountry.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlState.DataSource = ds;
                        ddlState.DataValueField = "iStateID";
                        ddlState.DataTextField = "sStateName";
                        ddlState.DataBind();
                        ddlState.Items.Insert(0, new ListItem("-select state-", "0"));

                        ddlCity.Items.Clear();
                        ddlCity.Items.Add(new ListItem("-select city-", "0"));
                    }
                }

            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            ds.Dispose();
            objCountry = null;
        }
    }
    #endregion


}
