/// <summary>
/// Module Name : Additional Station
/// Description : Used to add aditional station for that user only.
///               We can add workgroup and for that workgroup employee also for this station.
/// Created : Mayur on 31-dec-2011
/// </summary>

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Company_Employee_AdditionalStation : PageBase
{
    #region Property
    Int64 CompanyId
    {
        get
        {
            if (ViewState["CompanyId"] == null)
            {
                ViewState["CompanyId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyId"]);
        }
        set
        {
            ViewState["CompanyId"] = value;
        }
    }
    Int64 EmployeeId
    {
        get
        {
            if (ViewState["EmployeeId"] == null)
            {
                ViewState["EmployeeId"] = 0;
            }
            return Convert.ToInt64(ViewState["EmployeeId"]);
        }
        set
        {
            ViewState["EmployeeId"] = value;
        }
    }
    Int64 UserInfoId
    {
        get
        {
            if (ViewState["UserInfoId"] == null)
            {
                ViewState["UserInfoId"] = 0;
            }
            return Convert.ToInt64(ViewState["UserInfoId"]);
        }
        set
        {
            ViewState["UserInfoId"] = value;
        }
    }
    long CompanyContactInfoID
    {
        get
        {
            if (ViewState["CompanyContactInfoID"] == null)
            {
                ViewState["CompanyContactInfoID"] = "0";
            }
            return Convert.ToInt64(ViewState["CompanyContactInfoID"]);
        }
        set
        {
            ViewState["CompanyContactInfoID"] = value;
        }
    }
    Int64 iCityID
    {
        get
        {
            if (ViewState["iCityID"] == null)
            {
                ViewState["iCityID"] = 0;
            }
            return Convert.ToInt64(ViewState["iCityID"]);
        }
        set
        {
            ViewState["iCityID"] = value;
        }
    }
    PagedDataSource pds = new PagedDataSource();
    BaseStationRepository objBaseStationRepository = new BaseStationRepository();
    CompanyEmployeeContactInfoRepository objCompanyEmployeeContactInfoRepository = new CompanyEmployeeContactInfoRepository();
    Common objcomm = new Common();
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
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Employees";
            base.ParentMenuID = 11;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            if (Request.QueryString.Count > 0)
            {
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.EmployeeId = Convert.ToInt64(Request.QueryString.Get("SubId"));

                if (this.EmployeeId == 0)
                {
                    Response.Redirect("~/admin/Company/Employee/ViewEmployee.aspx?Id=" + this.CompanyId);
                }

                menucontrol.PopulateMenu(2, 7, this.CompanyId, this.EmployeeId, true);

                this.UserInfoId = new CompanyEmployeeRepository().GetById(this.EmployeeId).UserInfoID;

                ((Label)Master.FindControl("lblPageHeading")).Text = "Additional Stations Managed";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Company/Employee/ViewEmployee.aspx?id=" + this.CompanyId;

                FillBasedStation();
                BindShippingCountryDDL(sender, e);
                BindWorkgroupDDL();
                BindGrid();
            }
            else
            {
                Response.Redirect("~/admin/Company/Employee/ViewEmployee.aspx");
            }
        }
    }

    protected void btnAddStation_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            lblMsg.Text = string.Empty;
            lblmsgList.Text = string.Empty;
            if (ddlBasestation.SelectedValue=="0")
            {
                lblMsg.Text = "Please select station...";
                BindGrid();
                return;
            }
            if (objCompanyEmployeeContactInfoRepository.CheckForShippingStationAvailabelByUserInfoID(this.UserInfoId, ddlBasestation.SelectedValue) == true)
            {
                lblMsg.Text = "Record already exists...";
                BindGrid();
                return;
            }
            CompanyEmployeeContactInfo objCompanyEmployeeContactInfo = new CompanyEmployeeContactInfo();

            INC_BasedStation objBasedStation = objBaseStationRepository.GetById(Convert.ToInt64(ddlBasestation.SelectedValue));
            objCompanyEmployeeContactInfo.CompanyName = new CompanyRepository().GetById(this.CompanyId).CompanyName;
            objCompanyEmployeeContactInfo.UserInfoID = this.UserInfoId;
            objCompanyEmployeeContactInfo.Station = ddlBasestation.SelectedValue;
            objCompanyEmployeeContactInfo.Name = null;//manager firstname
            objCompanyEmployeeContactInfo.Fax = null;//manager lastname
            objCompanyEmployeeContactInfo.Email = null;//manager emailid
            objCompanyEmployeeContactInfo.Address = null;
            objCompanyEmployeeContactInfo.Title = objBasedStation.sBaseStation;//AddressName
            objCompanyEmployeeContactInfo.CountryID = objBasedStation.iCountryID;
            objCompanyEmployeeContactInfo.StateID = null;
            objCompanyEmployeeContactInfo.CityID = null;
            objCompanyEmployeeContactInfo.ZipCode = null;
            objCompanyEmployeeContactInfo.Telephone = null;
            objCompanyEmployeeContactInfo.ContactInfoType = Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString();
            objCompanyEmployeeContactInfo.OrderType = "AdditionalStation";

            objCompanyEmployeeContactInfoRepository.Insert(objCompanyEmployeeContactInfo);
            objCompanyEmployeeContactInfoRepository.SubmitChanges();
            lblMsg.Text = "Record inserted successfully ...";
            BindGrid();

        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnEditShippingAddress_Click(object sender, EventArgs e)
    {
        //Start Add City when Other Selection form city dropdownlist"
        if (PnlCityOther.Visible == true)
        {
            CityRepository objCityRep = new CityRepository();
            INC_City objCity = new INC_City();
            objCity.iCountryID = Convert.ToInt64(DrpShipingCoutry.SelectedItem.Value);
            objCity.iStateID = Convert.ToInt64(DrpShipingState.SelectedItem.Value);
            if (this.iCityID != 0)
            {
                objCity = objCityRep.GetById(this.iCityID);
            }
            objCity.sCityName = txtCity.Text;
            objCityRep.Insert(objCity);
            objCityRep.SubmitChanges();
            this.iCityID = Convert.ToInt32(objCity.iCityID);
            Session["this.iCityID"] = this.iCityID;
            txtCity.Text = string.Empty;
            PnlCityOther.Visible = false;
        }
        //End


        lblMsg.Text = "";
        lblmsgList.Text = "";
        lblPopupMessage.Text = "";
        CompanyEmployeeContactInfo objShipInfo = new CompanyEmployeeContactInfo();

        try
        {
            objShipInfo = objCompanyEmployeeContactInfoRepository.GetShippingDetailsByContactInfoID(this.CompanyContactInfoID);

            objShipInfo.CompanyName = TxtShippingCompany.Text;
            objShipInfo.Name = TxtShippingFirstName.Text;
            objShipInfo.Title = TxtShipingTitle.Text;
            objShipInfo.CountryID = Common.GetLongNull(DrpShipingCoutry.SelectedValue, true);
            objShipInfo.StateID = Common.GetLongNull(DrpShipingState.SelectedValue, true);
            if (DrpShippingCity.SelectedValue == "-Other-")
            {
                objShipInfo.CityID = Convert.ToInt64(Session["this.iCityID"]);
            }
            else
            {
                objShipInfo.CityID = Int64.Parse(DrpShippingCity.SelectedItem.Value);
            }
            objShipInfo.ZipCode = TxtShippingZip.Text;
            objShipInfo.Fax = TxtShipingLastName.Text;
            objShipInfo.Address = TxtShipingAddress.Text;

            objCompanyEmployeeContactInfoRepository.SubmitChanges();
            this.CompanyContactInfoID = 0;
            BindGrid();
            ClearShippingFields(sender, e);
            lblmsgList.Text = "Record updated successfully...";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

            if (ex.ToString().Contains("Cannot insert duplicate key"))
                lblPopupMessage.Text = "Title already exists please enter another Title could not save shipping address.";
            else
                lblPopupMessage.Text = "Error in saving shipping address ...";
            modalShippingAddress.Show();
        }
    }

    protected void lnkbtnAddWorkgroup_Click(object sender, EventArgs e)
    {
        try
        {
            //check for workgroup already exist for this station or not
            if (objCompanyEmployeeContactInfoRepository.CheckForStationWorkgroupAvailabelByContactInfoID(this.CompanyContactInfoID, Convert.ToInt64(ddlWorkgroup.SelectedValue)) == true)
            {
                lblPopupMessage.Text = "This Workgroup is already exists for this station...";
                BindGrid();
                modalShippingAddress.Show();
                return;
            }
            CompanyEmployeeStationWorkgroup objCompanyEmployeeStationWorkgroup = new CompanyEmployeeStationWorkgroup();
            objCompanyEmployeeStationWorkgroup.CompanyContactInfoID = this.CompanyContactInfoID;
            objCompanyEmployeeStationWorkgroup.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            objCompanyEmployeeStationWorkgroup.TotalEmployee = Convert.ToInt64(txtTotalEmployee.Text);
            objCompanyEmployeeContactInfoRepository.Insert(objCompanyEmployeeStationWorkgroup);
            objCompanyEmployeeContactInfoRepository.SubmitChanges();
            BindGrid();
            ClearWorkgroupFields();
            lblmsgList.Text = "Record inserted successfully...";
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblPopupMessage.Text = objcomm.Callvalidationmessage(Server.MapPath("../../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }
    }

    protected void DrpShipingCoutry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(DrpShipingCoutry.SelectedValue));

        DrpShipingState.Items.Clear();
        DrpShipingState.ClearSelection();
        DrpShippingCity.Items.Clear();
        DrpShippingCity.Items.Insert(0, new ListItem("-select city-", "0"));
        Common.BindDDL(DrpShipingState, objStateList, "sStatename", "iStateID", "-select state-");
        if (DrpShipingCoutry.SelectedValue == "0" && DrpShipingState.SelectedValue == "0")
        {
            DrpShippingCity.Items.Remove(new ListItem("-Other-", "-Other-"));
        }
        try
        {
            if ((DropDownList)sender == DrpShipingCoutry)
            {
                modalShippingAddress.Show();
            }
        }
        catch { }
    }

    protected void DrpShipingState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(DrpShipingState.SelectedValue));
        DrpShippingCity.Items.Clear();
        Common.BindDDL(DrpShippingCity, objCityList, "sCityName", "iCityID", "-select city-");
        if (DrpShipingState.SelectedIndex > 0)
        {
            DrpShippingCity.Items.Insert(1, "-Other-");
        }
        modalShippingAddress.Show();
    }
    
    protected void DrpShippingCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DrpShippingCity.SelectedValue == "-Other-")
        {
            PnlCityOther.Visible = true;
        }
        else
        {
            PnlCityOther.Visible = false;
        }
        modalShippingAddress.Show();
    }
    
    #region Datalist Events
    protected void dtlUserStations_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            switch (this.ViewState["SortExp"].ToString())
            {
                case "Station":
                    PlaceHolder placeholderStation = (PlaceHolder)e.Row.FindControl("placeholderStation");
                    break;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((HiddenField)e.Row.FindControl("hdnIsAdditionalStationActive")).Value == "False")
            {
                ((HtmlImage)e.Row.FindControl("imbLink")).Src = "~/Images/grd_inactive.png";
                ((HtmlImage)e.Row.FindControl("imbLink")).Height = 18;
                ((HtmlImage)e.Row.FindControl("imbLink")).Width = 18;
            }

            List<CompanyEmployeeContactInfoRepository.StationAdditionalWorkgroup> objCompanyEmployeeStationWorkgroup = objCompanyEmployeeContactInfoRepository.GetAdditionalWorkgroupByCompanyContactInfoID(Convert.ToInt64(((HiddenField)e.Row.FindControl("hdnCompanyContactInfoID")).Value));
            GridView grdWorkgroup = ((GridView)e.Row.FindControl("dtlUserWorkgroup"));
            DataTable dataTable = ListToDataTable(objCompanyEmployeeStationWorkgroup);
            grdWorkgroup.DataSource = dataTable.DefaultView;
            grdWorkgroup.DataBind();
            grdWorkgroup.Visible = true;
        }
    }
    protected void dtlUserStations_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblMsg.Text = string.Empty;
        lblmsgList.Text = string.Empty;
        Int64 CompanyContactInfoID = 0;
        CompanyEmployeeContactInfo objShipInfo = null;
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
            BindGrid();
        }

        if (e.CommandName == "StationAddress")
        {
            tblWorkgroup.Visible = false;
            lnkbtnAddWorkgroup.Visible = false;
            tblAddress.Visible = true;
            lnkbtnEditShippingAddress.Visible = true;
            
            PnlCityOther.Visible = false;
            //display billing record to edit
            CompanyContactInfoID = Convert.ToInt64(e.CommandArgument);
            objShipInfo = objCompanyEmployeeContactInfoRepository.GetShippingDetailsByContactInfoID(CompanyContactInfoID);

            if (objShipInfo != null)
            {
                this.CompanyContactInfoID = CompanyContactInfoID;
                TxtShippingCompany.Text = objShipInfo.CompanyName;
                //Name is First Name Now for the shipping
                TxtShippingFirstName.Text = objShipInfo.Name;
                TxtShipingTitle.Text = objShipInfo.Title;
                DrpShipingCoutry.SelectedValue = Common.GetLongString(objShipInfo.CountryID);
                DrpShipingCoutry_SelectedIndexChanged(sender, e);

                DrpShipingState.SelectedValue = Common.GetLongString(objShipInfo.StateID);
                DrpShipingState_SelectedIndexChanged(sender, e);

                DrpShippingCity.SelectedValue = Common.GetLongString(objShipInfo.CityID);

                TxtShippingZip.Text = objShipInfo.ZipCode;
                //Fax is Last name now for the shipping
                TxtShipingLastName.Text = objShipInfo.Fax;
                TxtShipingAddress.Text = objShipInfo.Address;
                TxtShippingCompany.Focus();
            }
            
            modalShippingAddress.Show();

            BindGrid();
        }
        if (e.CommandName == "Workgroup")
        {
            BindGrid();

            //GridViewRow row;
            //row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            //GridView grdWorkgroup = ((GridView)dtlUserStations.Rows[row.RowIndex].FindControl("dtlUserWorkgroup"));
            //List<CompanyEmployeeContactInfo> objCompanyEmployeeContactInfoList = objCompanyEmployeeContactInfoRepository.GetAditionalStationShippingDetailsByUserInfoID(this.UserInfoId);
            //DataTable dataTable = ListToDataTable(objCompanyEmployeeContactInfoList);
            //grdWorkgroup.DataSource = dataTable.DefaultView;
            //grdWorkgroup.DataBind();
            //grdWorkgroup.Visible = true;

            this.CompanyContactInfoID = Convert.ToInt64(e.CommandArgument);
            tblWorkgroup.Visible = true;
            lnkbtnAddWorkgroup.Visible = true;
            tblAddress.Visible = false;
            lnkbtnEditShippingAddress.Visible = false;
            modalShippingAddress.Show();
        }
        if (e.CommandName == "DeleteStation")
        {
            try
            {
                if (!base.CanDelete)
                {
                    base.RedirectToUnauthorised();
                }

                objShipInfo = objCompanyEmployeeContactInfoRepository.GetShippingDetailsByContactInfoID(Convert.ToInt64(e.CommandArgument));
                objCompanyEmployeeContactInfoRepository.Delete(objShipInfo);
                objCompanyEmployeeContactInfoRepository.SubmitChanges();
                lblmsgList.Text = "Record deleted successfully ...";
            }
            catch (Exception ex)
            {
                if (((System.Data.SqlClient.SqlException)(ex)).Number == 547)
                {
                    lblmsgList.Text = "Unable to delete Shipping information as it is being used in orders!!";
                }
                else
                {
                    ErrHandler.WriteError(ex);
                    lblmsgList.Text = "Error in deleting record ...";
                }
            }
            BindGrid();
        }
        if (e.CommandName == "Link")
        {
            objShipInfo = objCompanyEmployeeContactInfoRepository.GetShippingDetailsByContactInfoID(Convert.ToInt64(e.CommandArgument));
            objShipInfo.IsAdditionalStationActive = !objShipInfo.IsAdditionalStationActive;
            objCompanyEmployeeContactInfoRepository.SubmitChanges();
            lblmsgList.Text = "Record updated successfully ...";
            BindGrid();
        }
    }
    protected void dtlUserWorkgroup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteWorkgroup")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            CompanyEmployeeStationWorkgroup CompanyEmployeeStationWorkgroup = objCompanyEmployeeContactInfoRepository.GetCompanyEmployeeStationWorkgroupByID(Convert.ToInt64(e.CommandArgument));
            objCompanyEmployeeContactInfoRepository.Delete(CompanyEmployeeStationWorkgroup);
            objCompanyEmployeeContactInfoRepository.SubmitChanges();
            lblmsgList.Text = "Record deleted successfully ...";
        }
        BindGrid();
    }
    #endregion
    #endregion

    #region Methods
    /// <summary>
    /// Fills the based station dropdown.
    /// </summary>
    public void FillBasedStation()
    {
        BaseStationRepository objBaseStationRepository = new BaseStationRepository();
        List<INC_BasedStation> objBasedStation = new BaseStationRepository().GetAllBaseStation().OrderBy(b => b.sBaseStation).ToList();
        DataTable dtBaseStation = ListToDataTable(objBasedStation);
        if (dtBaseStation.Rows.Count > 0)
        {
            ddlBasestation.DataSource = dtBaseStation;
            ddlBasestation.DataValueField = "iBaseStationId";
            ddlBasestation.DataTextField = "sBaseStation";
            ddlBasestation.DataBind();
            ddlBasestation.Items.Insert(0, new ListItem("-select basestation-", "0"));
        }
    }

    /// <summary>
    /// Binds the shipping country dropdownlist.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    void BindShippingCountryDDL(object sender, EventArgs e)
    {
        List<INC_Country> objShippingCountryList = new CountryRepository().GetAll();
        Common.BindDDL(DrpShipingCoutry, objShippingCountryList, "sCountryName", "iCountryID", "-select country-");
        DrpShipingCoutry.SelectedValue = DrpShipingCoutry.Items.FindByText("United States").Value;
        DrpShipingCoutry_SelectedIndexChanged(sender, e);
    }

    /// <summary>
    /// Binds the workgroup dropdownlist.
    /// </summary>
    void BindWorkgroupDDL()
    {
        CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetById(this.EmployeeId);
        string strStatus = "Workgroup";
        ddlWorkgroup.DataSource = new LookupRepository().GetByLookupWorkgroupName(strStatus, Convert.ToInt32(objCompanyEmployee.WorkgroupID));
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
    }

    /// <summary>
    /// Binds the additional detail.
    /// </summary>
    /// <param name="obj">The obj.</param>
    protected void BindDetail(List<CompanyEmployeeContactInfo> obj)
    {
        try
        {
            UserInformation objUI = new UserInformationRepository().GetById(this.UserInfoId);
            lblStationManager.Text = objUI.FirstName + " " + objUI.LastName;
            Int64 TotalEmployee = 0;
            int TotalWorkgroup = 0;
            foreach (CompanyEmployeeContactInfo info in obj)
            {
                List<CompanyEmployeeContactInfoRepository.StationAdditionalWorkgroup> objWorkgroup = objCompanyEmployeeContactInfoRepository.GetAdditionalWorkgroupByCompanyContactInfoID(info.CompanyContactInfoID);
                foreach (CompanyEmployeeContactInfoRepository.StationAdditionalWorkgroup workgroup in objWorkgroup)
                {
                    TotalWorkgroup++;
                    TotalEmployee += workgroup.TotalEmployee;
                }
            }
            lblTotalEmployee.Text = TotalEmployee.ToString();
            lblTotalWorkgroup.Text = TotalWorkgroup.ToString();
        }
        catch { }
    }
    
    public void BindGrid()
    {
        DataView myDataView = new DataView();
        List<CompanyEmployeeContactInfo> objCompanyEmployeeContactInfoList = objCompanyEmployeeContactInfoRepository.GetAditionalStationShippingDetailsByUserInfoID(this.UserInfoId);

        //bind station detail
        BindDetail(objCompanyEmployeeContactInfoList);

        if (objCompanyEmployeeContactInfoList.Count == 0)
        {
            pagingtable.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
        }

        DataTable dataTable = ListToDataTable(objCompanyEmployeeContactInfoList);
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
        dtlUserStations.DataSource = pds;
        dtlUserStations.DataBind();
        doPaging();
    }
    
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
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

    /// <summary>
    /// Clears the shipping popup fields.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    void ClearShippingFields(object sender, EventArgs e)
    {

        TxtShippingCompany.Text = "";
        TxtShippingFirstName.Text = "";
        TxtShipingTitle.Text = "";
        //DrpShipingState.SelectedValue = "0";
        DrpShippingCity.SelectedValue = "0";
        TxtShippingZip.Text = "";
        //Fax is Last Name now
        TxtShipingLastName.Text = "";
        TxtShipingAddress.Text = "";
        DrpShipingCoutry.SelectedValue = DrpShipingCoutry.Items.FindByText("United States").Value;
        DrpShipingCoutry_SelectedIndexChanged(sender, e);
    }

    /// <summary>
    /// Clears the workgroup fields.
    /// </summary>
    void ClearWorkgroupFields()
    {
        ddlWorkgroup.SelectedIndex = 0;
        txtTotalEmployee.Text = string.Empty;
    }
    #endregion

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGrid();
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
            {
                ToPg = pds.PageCount;
            }

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
        BindGrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindGrid();
    }
    #endregion
}
