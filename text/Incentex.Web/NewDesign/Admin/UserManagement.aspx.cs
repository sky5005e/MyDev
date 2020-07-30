using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class Admin_UserManagement : PageBase
{
    #region Properties

    public String FirstName
    {
        get
        {
            if (Convert.ToString(this.ViewState["FirstName"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["FirstName"]);
        }
        set
        {
            this.ViewState["FirstName"] = value;
        }
    }

    public String LastName
    {
        get
        {
            if (Convert.ToString(this.ViewState["LastName"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["LastName"]);
        }
        set
        {
            this.ViewState["LastName"] = value;
        }
    }

    public Int64? WorkGroupID
    {
        get
        {
            if (this.ViewState["WorkGroupID"] == null || Convert.ToString(this.ViewState["WorkGroupID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["WorkGroupID"]);
        }
        set
        {
            this.ViewState["WorkGroupID"] = value;
        }
    }

    public Int64? BaseStationID
    {
        get
        {
            if (this.ViewState["BaseStationID"] == null || Convert.ToString(this.ViewState["BaseStationID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["BaseStationID"]);
        }
        set
        {
            this.ViewState["BaseStationID"] = value;
        }
    }

    public Int64? SytemStatusID
    {
        get
        {
            if (this.ViewState["SytemStatusID"] == null || Convert.ToString(this.ViewState["SytemStatusID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["SytemStatusID"]);
        }
        set
        {
            this.ViewState["SytemStatusID"] = value;
        }
    }

    public Int64? SytemAccessID
    {
        get
        {
            if (this.ViewState["SytemAccessID"] == null || Convert.ToString(this.ViewState["SytemAccessID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["SytemAccessID"]);
        }
        set
        {
            this.ViewState["SytemAccessID"] = value;
        }
    }

    public String KeyWord
    {
        get
        {
            if (Convert.ToString(this.ViewState["KeyWord"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["KeyWord"]);
        }
        set
        {
            this.ViewState["KeyWord"] = value;
        }
    }

    public Int64? CompanyID
    {
        get
        {
            if (this.ViewState["CompanyID"] == null || Convert.ToString(this.ViewState["CompanyID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["CompanyID"]);
        }
        set
        {
            this.ViewState["CompanyID"] = value;
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

    public Int64 ActiveID
    {
        get
        {
            return Convert.ToInt64(this.ViewState["ActiveID"]);
        }
        set
        {
            this.ViewState["ActiveID"] = value;
        }
    }

    public Int64 InActiveID
    {
        get
        {
            return Convert.ToInt64(this.ViewState["InActiveID"]);
        }
        set
        {
            this.ViewState["InActiveID"] = value;
        }
    }

    #endregion

    #region Fields

    private List<GetUserCategoryAccessResult> lstUserCategories = new List<GetUserCategoryAccessResult>();

    private List<GetUserStationsResult> lstUserStations = new List<GetUserStationsResult>();

    private List<GetUserStationWorkGroupResult> lstUserStationWorkGroups = new List<GetUserStationWorkGroupResult>();

    private List<GetUserStationPrivilegesResult> lstUserStationPrivileges = new List<GetUserStationPrivilegesResult>();

    private List<GetUserReportsResult> lstUserReports = new List<GetUserReportsResult>();

    private List<GetUserSubReportsResult> lstUserSubReports = new List<GetUserSubReportsResult>();

    private List<GetUserReportWorkGroupsResult> lstUserReportWorkGroups = new List<GetUserReportWorkGroupsResult>();

    private List<GetUserReportStationsResult> lstUserReportStations = new List<GetUserReportStationsResult>();

    private List<GetUserReportPriceLevelsResult> lstUserReportPriceLevels = new List<GetUserReportPriceLevelsResult>();

    #endregion

    #region Custom Classes

    #endregion

    #region Page Events

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                this.CompanyID = IncentexGlobal.CurrentMember.CompanyId;

            FillDropDowns();
            revAddPopupEmail.ValidationExpression = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
            revBasicEmail.ValidationExpression = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
        }
    }

    #endregion

    #region Control Events

    protected void btnSearchEmployee_Click(Object sender, EventArgs e)
    {
        try
        {
            this.FirstName = txtSearchFirstName.Text.Trim();
            this.LastName = txtSearchLastName.Text.Trim();
            this.WorkGroupID = Convert.ToInt64(ddlSearchWorkGroup.SelectedValue);
            this.BaseStationID = Convert.ToInt64(ddlSearchBaseStation.SelectedValue);
            this.SytemStatusID = Convert.ToInt64(ddlSearchSystemStatus.SelectedValue);
            this.SytemAccessID = Convert.ToInt64(ddlSearchSystemAccess.SelectedValue);
            this.KeyWord = String.Empty;
            this.NoOfRecordsToDisplay = Convert.ToInt32(Application["NEWPAGESIZE"]);
            this.PageIndex = 1;
            this.FromPage = 1;
            this.ToPage = this.NoOfPagesToDisplay;

            btnSearchGrid.Enabled = true;
            btnSearchGrid.CssClass += " postback";
            txtSearchGrid.Text = String.Empty;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                this.CompanyID = Convert.ToInt64(ddlSearchCompany.SelectedValue);

            BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnSearchGrid_Click(Object sender, EventArgs e)
    {
        try
        {
            this.KeyWord = txtSearchGrid.Text.Trim();
            BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void chkStatus_CheckedChanged(Object sender, EventArgs e)
    {
        try
        {
            CheckBox chkSender = (CheckBox)sender;
            Boolean IsStatusUpdated = false;

            foreach (GridViewRow employeeRow in gvEmployees.Rows)
            {
                CheckBox chkStatus = (CheckBox)employeeRow.FindControl("chkStatus");

                if (chkSender.ClientID == chkStatus.ClientID)
                {
                    HiddenField hdnUserInfoID = (HiddenField)employeeRow.FindControl("hdnUserInfoID");

                    if (hdnUserInfoID != null && Convert.ToInt64(hdnUserInfoID.Value) > 0)
                    {
                        UserInformationRepository objUserRepo = new UserInformationRepository();
                        UserInformation objUser = objUserRepo.GetById(Convert.ToInt64(hdnUserInfoID.Value));
                        if (objUser != null)
                        {
                            NoteDetail objNote = new NoteDetail();
                            objNote.CreateDate = DateTime.Now;
                            objNote.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                            objNote.ForeignKey = objUser.UserInfoID;

                            if (objUser.WLSStatusId == this.ActiveID)
                            {
                                objUser.WLSStatusId = this.InActiveID;
                                objNote.Notecontents = "Changed user status to : In-Active.";
                            }
                            else
                            {
                                objUser.WLSStatusId = this.ActiveID;
                                objNote.Notecontents = "Changed user status to : Active.";
                            }

                            objNote.NoteFor = "UserManagement";
                            objNote.SpecificNoteFor = "IEActivity";

                            objUserRepo.Insert(objNote);
                            objUserRepo.SubmitChanges();
                            IsStatusUpdated = true;
                            break;
                        }
                    }
                }
            }

            if (IsStatusUpdated)
                BindGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvEmployees_RowCommand(Object sender, GridViewCommandEventArgs e)
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
            else if (e.CommandName == "Detail")
            {
                hdnBasicUserInfoID.Value = e.CommandArgument.ToString();
                SetEmployeeDetails(Convert.ToInt64(hdnBasicUserInfoID.Value));
                this.ClientScript.RegisterStartupScript(this.GetType(), new Guid().ToString(), "EmployeeDetailsPopup('basic');", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void repPaymentOptions_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnPaymentOptionID = (HiddenField)e.Item.FindControl("hdnPaymentOptionID");
                CheckBox chkPaymentOption = (CheckBox)e.Item.FindControl("chkPaymentOption");
                Literal ltrlPaymentOption = (Literal)e.Item.FindControl("ltrlPaymentOption");

                GetUserPaymentOptionsResult objPaymentOption = (GetUserPaymentOptionsResult)e.Item.DataItem;

                if (objPaymentOption != null && chkPaymentOption != null && hdnPaymentOptionID != null && ltrlPaymentOption != null)
                {
                    chkPaymentOption.Checked = Convert.ToBoolean(objPaymentOption.IsActive);
                    ltrlPaymentOption.Text = Convert.ToString(objPaymentOption.PaymentOption);

                    //This is only for MOAS Payment Option
                    if (!String.IsNullOrEmpty(hdnPaymentOptionID.Value) && Convert.ToInt64(hdnPaymentOptionID.Value) == Convert.ToInt32(Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS))
                    {
                        CheckBox chkMOASIssuancePurchase = (CheckBox)e.Item.FindControl("chkMOASIssuancePurchase");
                        Literal ltrlMOASIssuancePurchase = (Literal)e.Item.FindControl("ltrlMOASIssuancePurchase");

                        DataList dlMOASApprovers = (DataList)e.Item.FindControl("dlMOASApprovers");
                        Panel pnlMOASApprovers = (Panel)e.Item.FindControl("pnlMOASApprovers");

                        ltrlPaymentOption.Text = "MOAS Storefront Purchases";

                        ltrlMOASIssuancePurchase.Text = "MOAS Issuance Policy Purchases";
                        ltrlMOASIssuancePurchase.Visible = true;

                        chkPaymentOption.CssClass += " chkMOAS";
                        chkPaymentOption.Checked = Convert.ToBoolean(objPaymentOption.IsForStorePurchase);

                        chkMOASIssuancePurchase.CssClass += " chkMOAS";
                        chkMOASIssuancePurchase.Checked = Convert.ToBoolean(objPaymentOption.IsForIssuancePurchase);
                        chkMOASIssuancePurchase.Visible = true;

                        pnlMOASApprovers.Visible = true;
                        pnlMOASApprovers.CssClass = "pnlMOASApprovers";

                        if (!chkPaymentOption.Checked && !chkMOASIssuancePurchase.Checked)
                            pnlMOASApprovers.Attributes.Add("style", "display:none;");

                        List<GetUserMOASApproversResult> lstMOASApprovers = new CompanyEmployeeRepository().GetUserMOASApprovers(Convert.ToInt64(hdnBasicUserInfoID.Value));

                        lstMOASApprovers = lstMOASApprovers.Select(le =>
                            new GetUserMOASApproversResult
                            {
                                ApproverID = le.ApproverID,
                                ApproverLevel = le.ApproverLevel,
                                FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(le.FirstName.Trim().ToLower()),
                                IsActive = le.IsActive,
                                LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(le.LastName.Trim().ToLower())
                            }).OrderBy(le => le.FirstName).ThenBy(le => le.LastName).ToList();

                        dlMOASApprovers.DataSource = lstMOASApprovers;
                        dlMOASApprovers.DataBind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void repCategories_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataList dlSubCategories = (DataList)e.Item.FindControl("dlSubCategories");
                HiddenField hdnCategoryID = (HiddenField)e.Item.FindControl("hdnCategoryID");
                dlSubCategories.DataSource = lstUserCategories.Where(le => le.CategoryID == Convert.ToInt64(hdnCategoryID.Value)).OrderBy(le => le.SubCategoryName).ToList();
                dlSubCategories.DataBind();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dlStationWorkGroups_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserStationsResult objRowData = (GetUserStationsResult)e.Item.DataItem;

                MultiSelectDropDown msdStationWorkGroups = (MultiSelectDropDown)e.Item.FindControl("msdStationWorkGroups");

                if (msdStationWorkGroups != null)
                {
                    msdStationWorkGroups.DataSource = this.lstUserStationWorkGroups.Where(le => le.StationID == objRowData.StationID).ToList();
                    msdStationWorkGroups.DataValueField = "WorkGroupID";
                    msdStationWorkGroups.DataTextField = "WorkGroup";
                    msdStationWorkGroups.DataCheckField = "IsActive";
                    msdStationWorkGroups.DataSelectText = this.lstUserStationWorkGroups.Where(le => le.StationID == objRowData.StationID && Convert.ToBoolean(le.IsActive)).ToList().Count > 0 ? "- Selected - " : "- Workgroup -";
                    msdStationWorkGroups.ToolTip = "- Workgroup -";
                    msdStationWorkGroups.DataIndexField = e.Item.ItemIndex.ToString();
                    msdStationWorkGroups.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dlStationPrivileges_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserStationsResult objRowData = (GetUserStationsResult)e.Item.DataItem;

                MultiSelectDropDown msdStationPrivileges = (MultiSelectDropDown)e.Item.FindControl("msdStationPrivileges");

                if (msdStationPrivileges != null)
                {
                    msdStationPrivileges.DataSource = this.lstUserStationPrivileges.Where(le => le.StationID == objRowData.StationID).ToList();
                    msdStationPrivileges.DataValueField = "PrivilegeID";
                    msdStationPrivileges.DataTextField = "PrivilegeName";
                    msdStationPrivileges.DataCheckField = "IsActive";
                    msdStationPrivileges.DataSelectText = this.lstUserStationPrivileges.Where(le => le.StationID == objRowData.StationID && Convert.ToBoolean(le.IsActive)).ToList().Count > 0 ? "- Selected - " : "- Privilege -";
                    msdStationPrivileges.ToolTip = "- Privilege -";
                    msdStationPrivileges.DataIndexField = e.Item.ItemIndex.ToString();
                    msdStationPrivileges.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dlSubReports_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserReportsResult objRowData = (GetUserReportsResult)e.Item.DataItem;

                MultiSelectDropDown msdSubReports = (MultiSelectDropDown)e.Item.FindControl("msdSubReports");

                if (msdSubReports != null)
                {
                    msdSubReports.DataSource = this.lstUserSubReports.Where(le => le.UserReportID == objRowData.UserReportID).ToList();
                    msdSubReports.DataValueField = "SubReportID";
                    msdSubReports.DataTextField = "SubReport";
                    msdSubReports.DataCheckField = "IsActive";
                    msdSubReports.DataSelectText = this.lstUserSubReports.Where(le => le.UserReportID == objRowData.UserReportID && Convert.ToBoolean(le.IsActive)).ToList().Count > 0 ? "- Selected - " : "- Sub Report -";
                    msdSubReports.ToolTip = "- Sub Report -";
                    msdSubReports.DataIndexField = e.Item.ItemIndex.ToString();
                    msdSubReports.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dlReportWorkGroups_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserReportsResult objRowData = (GetUserReportsResult)e.Item.DataItem;

                MultiSelectDropDown msdReportWorkGroups = (MultiSelectDropDown)e.Item.FindControl("msdReportWorkGroups");

                if (msdReportWorkGroups != null)
                {
                    msdReportWorkGroups.DataSource = this.lstUserReportWorkGroups.Where(le => le.UserReportID == objRowData.UserReportID).ToList();
                    msdReportWorkGroups.DataValueField = "WorkGroupID";
                    msdReportWorkGroups.DataTextField = "WorkGroup";
                    msdReportWorkGroups.DataCheckField = "IsActive";
                    msdReportWorkGroups.DataSelectText = this.lstUserReportWorkGroups.Where(le => le.UserReportID == objRowData.UserReportID && Convert.ToBoolean(le.IsActive)).ToList().Count > 0 ? "- Selected - " : "- Workgroup -";
                    msdReportWorkGroups.ToolTip = "- Workgroup -";
                    msdReportWorkGroups.DataIndexField = e.Item.ItemIndex.ToString();
                    msdReportWorkGroups.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dlReportStations_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserReportsResult objRowData = (GetUserReportsResult)e.Item.DataItem;

                MultiSelectDropDown msdReportStations = (MultiSelectDropDown)e.Item.FindControl("msdReportStations");

                if (msdReportStations != null)
                {
                    msdReportStations.DataSource = this.lstUserReportStations.Where(le => le.UserReportID == objRowData.UserReportID).ToList();
                    msdReportStations.DataValueField = "StationID";
                    msdReportStations.DataTextField = "Station";
                    msdReportStations.DataCheckField = "IsActive";
                    msdReportStations.DataSelectText = this.lstUserReportStations.Where(le => le.UserReportID == objRowData.UserReportID && Convert.ToBoolean(le.IsActive)).ToList().Count > 0 ? "- Selected - " : "- Station -";
                    msdReportStations.ToolTip = "- Station -";
                    msdReportStations.DataIndexField = e.Item.ItemIndex.ToString();
                    msdReportStations.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dlReportPriceLevels_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserReportsResult objRowData = (GetUserReportsResult)e.Item.DataItem;

                MultiSelectDropDown msdReportPriceLevels = (MultiSelectDropDown)e.Item.FindControl("msdReportPriceLevels");

                if (msdReportPriceLevels != null)
                {
                    msdReportPriceLevels.DataSource = this.lstUserReportPriceLevels.Where(le => le.UserReportID == objRowData.UserReportID).ToList();
                    msdReportPriceLevels.DataValueField = "PriceLevelID";
                    msdReportPriceLevels.DataTextField = "PriceLevel";
                    msdReportPriceLevels.DataCheckField = "IsActive";
                    msdReportPriceLevels.DataSelectText = this.lstUserReportPriceLevels.Where(le => le.UserReportID == objRowData.UserReportID && Convert.ToBoolean(le.IsActive)).ToList().Count > 0 ? "- Selected - " : "- Price Level -";
                    msdReportPriceLevels.ToolTip = "- Price Level -";
                    msdReportPriceLevels.DataIndexField = e.Item.ItemIndex.ToString();
                    msdReportPriceLevels.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lbAddPopupAddEmployee_Click(Object sender, EventArgs e)
    {
        try
        {
            Page.Validate("AddEmployee");
            if (Page.IsValid)
            {
                UserInformation objUserInfo = new UserInformation();
                UserInformationRepository objUserRepo = new UserInformationRepository();
                CompanyEmployee objCompanyEmployee = new CompanyEmployee();
                CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();

                if (!objUserRepo.CheckEmailExistence(txtAddPopupEmail.Text.Trim(), 0))
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "AddEmployeePopup();GeneralAlertMsg('This email already exists in the system.', true);", true);
                    return;
                }

                if (!String.IsNullOrEmpty(txtAddPopupEmployeeID.Text.Trim()))
                {
                    if (!objEmpRepo.CheckEmployeeIDExistence(txtAddPopupEmployeeID.Text.Trim(), Convert.ToInt64(this.CompanyID), 0))
                    {
                        this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "AddEmployeePopup();GeneralAlertMsg('This employee id already exists in the system.', true);", true);
                        return;
                    }
                    else if (!new RegistrationRepository().CheckEmployeeIDExistence(txtAddPopupEmployeeID.Text.Trim(), Convert.ToInt64(this.CompanyID), 0))
                    {
                        this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "AddEmployeePopup();GeneralAlertMsg('This employee id already exists in registration requests.', true);", true);
                        return;
                    }
                }

                objUserInfo.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtAddPopupFirstName.Text.Trim().ToLower());
                objUserInfo.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtAddPopupLastName.Text.Trim().ToLower());
                objUserInfo.WLSStatusId = Convert.ToInt32(ddlAddPopupSystemStatus.SelectedValue);
                objUserInfo.Email = txtAddPopupEmail.Text.Trim();
                objUserInfo.LoginEmail = txtAddPopupEmail.Text.Trim();
                objUserInfo.Password = txtAddPopupPassword.Text.Trim();
                objUserInfo.CreatedDate = DateTime.Now;
                objUserInfo.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objUserInfo.CompanyId = this.CompanyID;
                objUserInfo.Usertype = Convert.ToInt64(ddlAddPopupSystemAccess.SelectedValue);

                objUserInfo.ClimateSettingId = null;

                objUserRepo.Insert(objUserInfo);
                objUserRepo.SubmitChanges();

                objCompanyEmployee.UserInfoID = objUserInfo.UserInfoID;
                objCompanyEmployee.CompanyEmail = txtAddPopupEmail.Text.Trim();

                if (ddlAddPopupSystemAccess.SelectedItem.Text == Incentex.DA.DAEnums.CompanyEmployeeTypes.Admin.ToString())
                {
                    objCompanyEmployee.isCompanyAdmin = true;
                    objCompanyEmployee.IsMOASApprover = false;
                    objCompanyEmployee.ManagementControlForWorkgroup = Convert.ToInt64(ddlAddPopupWorkGroup.SelectedValue);
                    objCompanyEmployee.ManagementControlForRegion = null;
                    objCompanyEmployee.ManagementControlForStaionlocation = Convert.ToInt64(ddlAddPopupBaseStation.SelectedValue);
                }
                else
                {
                    objCompanyEmployee.isCompanyAdmin = false;
                    objCompanyEmployee.IsMOASApprover = false;
                    objCompanyEmployee.ManagementControlForWorkgroup = null;
                    objCompanyEmployee.ManagementControlForDepartment = null;
                    objCompanyEmployee.ManagementControlForRegion = null;
                    objCompanyEmployee.ManagementControlForStaionlocation = null;
                }

                if (!String.IsNullOrEmpty(txtAddPopupDateOfHire.Text.Trim()))
                    objCompanyEmployee.HirerdDate = Convert.ToDateTime(txtAddPopupDateOfHire.Text.Trim());

                if (!String.IsNullOrEmpty(txtAddPopupEmployeeID.Text.Trim()))
                    objCompanyEmployee.EmployeeID = txtAddPopupEmployeeID.Text.Trim();

                if (ddlAddPopupPosition.SelectedIndex != 0)
                    objCompanyEmployee.EmployeeTitleId = Convert.ToInt64(ddlAddPopupPosition.SelectedItem.Value);

                if (ddlAddPopupBaseStation.SelectedIndex > 0)
                    objCompanyEmployee.BaseStation = Convert.ToInt64(ddlAddPopupBaseStation.SelectedValue);

                if (ddlAddPopupGender.SelectedIndex > 0)
                    objCompanyEmployee.GenderID = Convert.ToInt64(ddlAddPopupGender.SelectedValue);

                if (ddlAddPopupWorkGroup.SelectedIndex > 0)
                    objCompanyEmployee.WorkgroupID = Convert.ToInt64(ddlAddPopupWorkGroup.SelectedValue);

                objCompanyEmployee.StoreActivatedBy = IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName;
                objCompanyEmployee.StoreActivatedDate = DateTime.Now;
                objCompanyEmployee.DateRequestSubmitted = DateTime.Now.ToString("MM/dd/yyyy");

                if (ddlAddPopupSystemStatus.SelectedItem.Text == "Active")
                    objCompanyEmployee.LastActiveDate = DateTime.Now;
                else
                    objCompanyEmployee.LastInActiveDate = DateTime.Now;

                if (ddlAddPopupIssuancePolicy.SelectedIndex > 0)
                    objCompanyEmployee.EmpIssuancePolicyStatus = Convert.ToInt64(ddlAddPopupIssuancePolicy.SelectedValue);

                objCompanyEmployee.RegionID = null;
                objCompanyEmployee.DepartmentID = null;
                objCompanyEmployee.UploadImage = null;
                objCompanyEmployee.IsMOASStationLevelApprover = null;

                objEmpRepo.Insert(objCompanyEmployee);

                objEmpRepo.SubmitChanges();

                ClearAddEmployeePopup();

                this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "GeneralAlertMsg('New employee added successfully.')", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "AddEmployeePopup();GeneralAlertMsg('An error occured : " + ex.Message + "', true)", true);
        }
    }

    protected void lbDownloadExcel_Click(Object sender, EventArgs e)
    {
        try
        {
            String excelTemplatePath = Common.UserManagementTemplatePath + "Bulk_Upload_Users_Template.xlsx";

            FileInfo excelTemplate = new FileInfo(excelTemplatePath);
            if (excelTemplate.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + excelTemplate.Name);
                Response.AddHeader("Content-Length", Convert.ToString(excelTemplate.Length));
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(excelTemplate.FullName);
                Response.End();
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "GeneralAlertMsg('Template does not exist.', true);AddEmployeePopup();$(document).ready(function(){$('#lnkUploadExcelFile').parent().click();});", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lbUploadExcel_Click(Object sender, EventArgs e)
    {
        try
        {
            if (fuUsers.HasFile)
            {
                String savedFileName = "UserManagement_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fuUsers.FileName;
                savedFileName = Common.MakeValidFileName(savedFileName);
                String filePath = Common.UserManagementTempPath + savedFileName;
                fuUsers.PostedFile.SaveAs(filePath);

                BulkUploadUser objBulkUploadUser = new BulkUploadUser();

                String message = objBulkUploadUser.SaveUsers(filePath, Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));

                if (message == String.Empty)
                    this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "GeneralAlertMsg('New employees inserted successfully.')", true);
                else
                    this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "GeneralAlertMsg('" + message.Replace("'", "&quot;").Replace("\r\n", "") + "', true);AddEmployeePopup();$(document).ready(function(){$('#lnkUploadExcelFile').parent().click();});", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnClearEmployeeDetails_Click(Object sender, EventArgs e)
    {
        try
        {
            ClearEmployeeDetails();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lbBasicSave_Click(Object sender, EventArgs e)
    {
        try
        {
            if (SaveBasic())
            {
                SetEmployeeDetails(Convert.ToInt64(hdnBasicUserInfoID.Value));
                this.ClientScript.RegisterStartupScript(this.GetType(), new Guid().ToString(), "EmployeeDetailsPopup('menuaccess', true);", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "EmployeeDetailsPopup('basic');GeneralAlertMsg('An error occured : " + ex.Message + "', true)", true);
        }
    }

    protected void lbBasicClose_Click(Object sender, EventArgs e)
    {
        try
        {
            if (SaveBasic())
                ClearEmployeeDetails();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lbBasicSendEmail_Click(Object sender, EventArgs e)
    {
        try
        {
            Page.Validate("Basic");
            if (Page.IsValid)
            {
                String emailTemplate = String.Empty;
                String emailSubject = "World-Link System Login Information";

                StreamReader streamReader;
                streamReader = File.OpenText(Server.MapPath("~/emailtemplate/LoginInformation.htm"));
                emailTemplate = streamReader.ReadToEnd();
                streamReader.Close();
                streamReader.Dispose();

                StringBuilder emailBody = new StringBuilder(emailTemplate);

                emailBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                emailBody.Replace("{fullname}", txtBasicFirstName.Text.Trim() + " " + txtBasicLastName.Text.Trim());
                emailBody.Replace("{email}", txtBasicEmail.Text.Trim());
                emailBody.Replace("{password}", txtBasicPassword.Text.Trim());

                new CommonMails().SendMail(Convert.ToInt64(this.hdnBasicUserInfoID.Value), "Login Information from User Management", Common.EmailFrom, txtBasicEmail.Text.Trim(), emailSubject, emailBody.ToString(), Common.DisplyName, Common.SMTPHost, Common.SMTPPort, Common.UserName, Common.Password, Common.SSL, true, null);
                this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "EmployeeDetailsPopup('basic');GeneralAlertMsg('Email with login information sent successfully.', true)", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "EmployeeDetailsPopup('basic');GeneralAlertMsg('An error occured : " + ex.Message + "', true)", true);
        }
    }

    protected void lbMenuAccessSave_Click(Object sender, EventArgs e)
    {
        try
        {
            if (SaveMenuAccess())
            {
                hdnCurrentSection.Value = "stationmanagement";
                SetEmployeeDetails(Convert.ToInt64(hdnBasicUserInfoID.Value), "adminsettings", Convert.ToString(hdnCurrentSection.Value));
                this.ClientScript.RegisterStartupScript(this.GetType(), new Guid().ToString(), "EmployeeDetailsPopup('adminsettings', 'stationmanagement');", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "EmployeeDetailsPopup('menuaccess');GeneralAlertMsg('An error occured : " + ex.Message + "', true)", true);
        }
    }

    protected void lbMenuAccessClose_Click(Object sender, EventArgs e)
    {
        try
        {
            if (SaveMenuAccess())
                ClearEmployeeDetails();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lbAdminSettingsSave_Click(Object sender, EventArgs e)
    {
        try
        {
            if (SaveAdminSettings())
            {
                if (hdnCurrentSection.Value == "stationmanagement")
                    hdnCurrentSection.Value = "workgroupmanagement";
                else if (hdnCurrentSection.Value == "workgroupmanagement")
                    hdnCurrentSection.Value = "privileges";
                else if (hdnCurrentSection.Value == "privileges")
                    hdnCurrentSection.Value = String.Empty;

                if (hdnCurrentSection.Value != String.Empty)
                {
                    SetEmployeeDetails(Convert.ToInt64(hdnBasicUserInfoID.Value), "adminsettings", Convert.ToString(hdnCurrentSection.Value));
                    this.ClientScript.RegisterStartupScript(this.GetType(), new Guid().ToString(), "EmployeeDetailsPopup('adminsettings', '" + hdnCurrentSection.Value + "');", true);
                }
                else
                    this.ClientScript.RegisterStartupScript(this.GetType(), new Guid().ToString(), "EmployeeDetailsPopup('reports');", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "EmployeeDetailsPopup('adminsettings', '" + hdnCurrentSection.Value + "');GeneralAlertMsg('An error occured : " + ex.Message + "', true)", true);
        }
    }

    protected void lbAdminSettingsClose_Click(Object sender, EventArgs e)
    {
        try
        {
            if (SaveAdminSettings())
                ClearEmployeeDetails();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lbReportsSave_Click(Object sender, EventArgs e)
    {
        try
        {
            if (SaveReports())
            {
                if (hdnCurrentSection.Value == "mainreports")
                    hdnCurrentSection.Value = "subreports";
                else if (hdnCurrentSection.Value == "subreports")
                    hdnCurrentSection.Value = "workgroups";
                else if (hdnCurrentSection.Value == "workgroups")
                    hdnCurrentSection.Value = "stations";
                else if (hdnCurrentSection.Value == "stations")
                    hdnCurrentSection.Value = "pricelevels";
                else if (hdnCurrentSection.Value == "pricelevels")
                    hdnCurrentSection.Value = String.Empty;

                if (hdnCurrentSection.Value != String.Empty)
                {
                    SetEmployeeDetails(Convert.ToInt64(hdnBasicUserInfoID.Value), "reports", Convert.ToString(hdnCurrentSection.Value));
                    this.ClientScript.RegisterStartupScript(this.GetType(), new Guid().ToString(), "EmployeeDetailsPopup('reports', '" + hdnCurrentSection.Value + "');", true);
                }
                else
                    this.ClientScript.RegisterStartupScript(this.GetType(), new Guid().ToString(), "EmployeeDetailsPopup('history');", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lbReportsClose_Click(Object sender, EventArgs e)
    {
        try
        {
            if (SaveReports())
                ClearEmployeeDetails();
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

    #endregion

    #region Methods

    private void FillDropDowns()
    {
        try
        {
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
            {
                CompanyRepository objCompanyRepo = new CompanyRepository();
                List<Company> lstCompanies = objCompanyRepo.GetAllCompany();
                BindDropDown(ddlSearchCompany, lstCompanies, "CompanyName", "CompanyId", "Company", "0");
                liCompany.Visible = true;
            }

            LookupRepository objLookupRepo = new LookupRepository();
            List<INC_Lookup> lstGender = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Gender).OrderBy(le => le.sLookupName).ToList();
            BindDropDown(ddlAddPopupGender, lstGender, "sLookupName", "iLookupID", "Gender", "0");
            BindDropDown(ddlBasicGender, lstGender, "sLookupName", "iLookupID", "Gender", "0");

            List<INC_Lookup> lstWorkGroups = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Workgroup).OrderBy(le => le.sLookupName).ToList();
            BindDropDown(ddlSearchWorkGroup, lstWorkGroups, "sLookupName", "iLookupID", "Workgroup", "0");
            BindDropDown(ddlAddPopupWorkGroup, lstWorkGroups, "sLookupName", "iLookupID", "Workgroup", "0");
            BindDropDown(ddlBasicWorkGroup, lstWorkGroups, "sLookupName", "iLookupID", "Workgroup", "0");

            BaseStationRepository objBaseStationRepo = new BaseStationRepository();
            List<INC_BasedStation> lstBaseStations = objBaseStationRepo.GetAllBaseStation().OrderBy(le => le.sBaseStation).ToList();
            BindDropDown(ddlSearchBaseStation, lstBaseStations, "sBaseStation", "iBaseStationId", "Station", "0");
            BindDropDown(ddlAddPopupBaseStation, lstBaseStations, "sBaseStation", "iBaseStationId", "Station", "0");
            BindDropDown(ddlBasicBaseStation, lstBaseStations, "sBaseStation", "iBaseStationId", "Station", "0");

            List<INC_Lookup> lstEmployeeTitles = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.EmployeeTitle).OrderBy(le => le.sLookupName).ToList();
            BindDropDown(ddlAddPopupPosition, lstEmployeeTitles, "sLookupName", "iLookupID", "Position", "0");
            BindDropDown(ddlBasicPosition, lstEmployeeTitles, "sLookupName", "iLookupID", "Position", "0");

            Dictionary<Int32, String> dctEmployeeTypes = Common.GetEnumForBind(typeof(Incentex.DA.DAEnums.CompanyEmployeeTypes));
            BindDropDown(ddlSearchSystemAccess, dctEmployeeTypes, "value", "key", "System Access", "0");
            BindDropDown(ddlAddPopupSystemAccess, dctEmployeeTypes, "value", "key", "System Access", "0");
            BindDropDown(ddlBasicSystemAccess, dctEmployeeTypes, "value", "key", "System Access", "0");
            ddlAddPopupSystemAccess.SelectedValue = Convert.ToString(Convert.ToInt32(Incentex.DA.DAEnums.CompanyEmployeeTypes.Employee));

            List<INC_Lookup> lstStatuses = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Status).OrderBy(le => le.sLookupName).ToList();
            BindDropDown(ddlSearchSystemStatus, lstStatuses, "sLookupName", "iLookupID", "System Status", "0");
            BindDropDown(ddlAddPopupSystemStatus, lstStatuses, "sLookupName", "iLookupID", "System Status", "0");
            BindDropDown(ddlBasicSystemStatus, lstStatuses, "sLookupName", "iLookupID", "System Status", "0");
            BindDropDown(ddlAddPopupIssuancePolicy, lstStatuses, "sLookupName", "iLookupID", "Issuance Policy", "0");
            BindDropDown(ddlBasicIssuancePolicy, lstStatuses, "sLookupName", "iLookupID", "Issuance Policy", "0");
            ddlAddPopupSystemStatus.SelectedValue = Convert.ToString(lstStatuses.FirstOrDefault(le => le.sLookupName == "InActive").iLookupID);

            this.ActiveID = lstStatuses.FirstOrDefault(le => le.sLookupName == "Active").iLookupID;
            this.InActiveID = lstStatuses.FirstOrDefault(le => le.sLookupName == "InActive").iLookupID;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
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

    private void BindGrid(Boolean isForSorting)
    {
        try
        {
            CompanyEmployeeRepository objCERepo = new CompanyEmployeeRepository();
            List<GetEmployeesBySearchCriteriaResult> lstEmployees = objCERepo.GetEmployeesBySearchCriteria(this.FirstName, this.LastName, this.WorkGroupID, this.BaseStationID, this.SytemStatusID, this.SytemAccessID, null, this.CompanyID, this.KeyWord, this.NoOfRecordsToDisplay, this.PageIndex, this.SortColumn, this.SortDirection);

            if (lstEmployees != null && lstEmployees.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lstEmployees[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;

                lstEmployees = lstEmployees.Select(le =>
                new GetEmployeesBySearchCriteriaResult
                {
                    BaseStation = !String.IsNullOrEmpty(le.BaseStation) ? le.BaseStation.Trim().ToUpper() : String.Empty,
                    BaseStationID = le.BaseStationID,
                    CompanyEmployeeID = le.CompanyEmployeeID,
                    EmployeeID = le.EmployeeID,
                    EmployeeStatus = le.EmployeeStatus,
                    FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(le.FirstName.Trim()),
                    LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(le.LastName.Trim()),
                    RowNum = le.RowNum,
                    TotalRecords = le.TotalRecords,
                    UserInfoID = le.UserInfoID,
                    WLSStatusID = le.WLSStatusID,
                    WorkGroup = !String.IsNullOrEmpty(le.WorkGroup) ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(le.WorkGroup.Trim().ToLower()) : String.Empty,
                    WorkGroupID = le.WorkGroupID
                }).ToList();
            }

            gvEmployees.DataSource = lstEmployees;
            gvEmployees.DataBind();

            if (!isForSorting)
            {
                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 1;
                lnkNext.Enabled = this.PageIndex < this.TotalPages;
            }

            if (gvEmployees.Rows.Count > 0)
            {
                totalcount_em.InnerText = lstEmployees[0].TotalRecords + " results";
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

    private void ClearAddEmployeePopup()
    {
        try
        {
            ddlAddPopupBaseStation.SelectedIndex = 0;
            ddlAddPopupGender.SelectedIndex = 0;
            ddlAddPopupIssuancePolicy.SelectedIndex = 0;
            ddlAddPopupPosition.SelectedIndex = 0;
            ddlAddPopupSystemAccess.SelectedIndex = 0;
            ddlAddPopupSystemStatus.SelectedIndex = 0;
            ddlAddPopupWorkGroup.SelectedIndex = 0;

            txtAddPopupDateOfHire.Text = String.Empty;
            txtAddPopupEmail.Text = String.Empty;
            txtAddPopupEmployeeID.Text = String.Empty;
            txtAddPopupFirstName.Text = String.Empty;
            txtAddPopupLastName.Text = String.Empty;
            txtAddPopupPassword.Text = String.Empty;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void ClearEmployeeDetails()
    {
        try
        {
            ddlBasicBaseStation.SelectedIndex = 0;
            ddlBasicGender.SelectedIndex = 0;
            ddlBasicIssuancePolicy.SelectedIndex = 0;
            ddlBasicPosition.SelectedIndex = 0;
            ddlBasicSystemAccess.SelectedIndex = 0;
            ddlBasicSystemStatus.SelectedIndex = 0;
            ddlBasicWorkGroup.SelectedIndex = 0;

            hdnBasicUserInfoID.Value = String.Empty;
            hdnCurrentSection.Value = "stationmanagement";

            txtBasicActivationDate.Text = String.Empty;
            txtBasicActivatedBy.Text = String.Empty;
            txtBasicDateOfHire.Text = String.Empty;
            txtBasicEmail.Text = String.Empty;
            txtBasicEmployeeID.Text = String.Empty;
            txtBasicFirstName.Text = String.Empty;
            txtBasicLastName.Text = String.Empty;
            txtBasicPassword.Text = String.Empty;
            txtBasicRequestDate.Text = String.Empty;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SetEmployeeDetails(Int64 userInfoID)
    {
        try
        {
            SetEmployeeDetails(userInfoID, String.Empty, String.Empty);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SetEmployeeDetails(Int64 userInfoID, String tabName, String sectionName)
    {
        try
        {
            GetUserDetailsByUserInfoIDResult objUser = new UserInformationRepository().GetUserDetailsByUserInfoID(userInfoID);
            if (objUser != null)
            {
                Boolean isAdmin = false;
                isAdmin = objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin);

                dvAdminSettingsForAdmins.Visible = isAdmin;
                dvAdminSettingsForEmployee.Visible = !isAdmin;
                dvReportsForAdmins.Visible = isAdmin;
                dvReportsForEmployee.Visible = !isAdmin;

                #region General Info

                ddlBasicBaseStation.SelectedValue = objUser.BaseStationID != null ? Convert.ToString(objUser.BaseStationID) : "0";
                ddlBasicGender.SelectedValue = objUser.GenderID != null ? Convert.ToString(objUser.GenderID) : "0";
                ddlBasicIssuancePolicy.SelectedValue = objUser.EmpIssuancePolicyStatus != null ? Convert.ToString(objUser.EmpIssuancePolicyStatus) : "0";
                ddlBasicPosition.SelectedValue = objUser.EmployeeTitleId != null ? Convert.ToString(objUser.EmployeeTitleId) : "0";
                ddlBasicSystemAccess.SelectedValue = Convert.ToString(objUser.Usertype);
                ddlBasicSystemStatus.SelectedValue = Convert.ToString(objUser.WLSStatusId);
                ddlBasicWorkGroup.SelectedValue = objUser.WorkGroupID != null ? Convert.ToString(objUser.WorkGroupID) : "0";

                txtBasicActivatedBy.Text = Convert.ToString(objUser.StoreActivatedBy);
                txtBasicActivatedBy.Attributes.Add("readonly", "readonly");
                txtBasicActivationDate.Text = objUser.StoreActivatedDate != null ? Convert.ToDateTime(objUser.StoreActivatedDate).ToString("MM/dd/yyyy") : "";
                txtBasicActivationDate.Attributes.Add("readonly", "readonly");
                txtBasicDateOfHire.Text = objUser.HireDate != null ? Convert.ToDateTime(objUser.HireDate).ToString("MM/dd/yyyy") : "";
                txtBasicEmail.Text = Convert.ToString(objUser.EmailAddress);
                txtBasicEmployeeID.Text = Convert.ToString(objUser.EmployeeID);
                txtBasicFirstName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Convert.ToString(objUser.FirstName));
                txtBasicLastName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Convert.ToString(objUser.LastName));
                txtBasicPassword.Text = Convert.ToString(objUser.UserPassword);
                txtBasicRequestDate.Text = objUser.DateRequestSubmitted != null ? Convert.ToDateTime(objUser.DateRequestSubmitted).ToString("MM/dd/yyyy") : "";
                txtBasicRequestDate.Attributes.Add("readonly", "readonly");

                #endregion

                CompanyEmployeeRepository objCompEmpRepo = new CompanyEmployeeRepository();

                #region Menu Access

                if (String.IsNullOrEmpty(tabName) || tabName == "menuaccess")
                {
                    List<GetUserPaymentOptionsResult> lstPaymentOptions = objCompEmpRepo.GetUserPaymentOptions(objUser.UserInfoID).OrderBy(le => le.PaymentOption).ToList();
                    repPaymentOptions.DataSource = lstPaymentOptions;
                    repPaymentOptions.DataBind();

                    List<GetUserShippingMethodsResult> lstShippingMethods = objCompEmpRepo.GetUserShippingMethods(objUser.UserInfoID).OrderBy(le => le.sLookupName).ToList();
                    repShippingMethods.DataSource = lstShippingMethods;
                    repShippingMethods.DataBind();

                    lstUserCategories = objCompEmpRepo.GetUserCategoryAccess(objUser.UserInfoID).OrderBy(le => le.CategoryName).ToList();
                    repCategories.DataSource = lstUserCategories.Select(le => new { CategoryID = le.CategoryID, CategoryName = le.CategoryName }).Distinct().OrderBy(le => le.CategoryName).ToList();
                    repCategories.DataBind();

                    List<GetUserManageEmailSettingsResult> lstManageEmails = objCompEmpRepo.GetUserManageEmailSettings(objUser.UserInfoID).Where(le => le.IsForAdmin != null && (le.IsForAdmin == false && objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))).OrderBy(le => le.ManageEmailName).ToList();
                    repManageEmails.DataSource = lstManageEmails;
                    repManageEmails.DataBind();
                }

                #endregion

                if (isAdmin)
                {
                    #region Admin Settings

                    if (String.IsNullOrEmpty(tabName) || tabName == "adminsettings")
                    {
                        this.lstUserStations = objCompEmpRepo.GetUserStations(userInfoID);
                        dlStations.DataSource = this.lstUserStations;
                        dlStations.DataBind();

                        this.lstUserStationWorkGroups = objCompEmpRepo.GetUserStationWorkGroup(userInfoID);
                        dlStationWorkGroups.DataSource = this.lstUserStations.Where(le => Convert.ToBoolean(le.IsActive)).ToList();
                        dlStationWorkGroups.DataBind();

                        this.lstUserStationPrivileges = objCompEmpRepo.GetUserStationPrivileges(userInfoID);
                        dlStationPrivileges.DataSource = this.lstUserStations.Where(le => Convert.ToBoolean(le.IsActive)).ToList();
                        dlStationPrivileges.DataBind();
                    }

                    #endregion

                    #region Reports

                    if (String.IsNullOrEmpty(tabName) || tabName == "reports")
                    {
                        this.lstUserReports = objCompEmpRepo.GetUserReports(userInfoID);
                        dlReports.DataSource = this.lstUserReports;
                        dlReports.DataBind();

                        this.lstUserSubReports = objCompEmpRepo.GetUserSubReports(userInfoID);
                        dlSubReports.DataSource = this.lstUserReports.Where(le => Convert.ToBoolean(le.IsActive)).ToList();
                        dlSubReports.DataBind();

                        this.lstUserReportWorkGroups = objCompEmpRepo.GetUserReportWorkGroups(userInfoID);
                        dlReportWorkGroups.DataSource = this.lstUserReports.Where(le => Convert.ToBoolean(le.IsActive)).ToList();
                        dlReportWorkGroups.DataBind();

                        this.lstUserReportStations = objCompEmpRepo.GetUserReportStations(userInfoID);
                        dlReportStations.DataSource = this.lstUserReports.Where(le => Convert.ToBoolean(le.IsActive)).ToList();
                        dlReportStations.DataBind();

                        this.lstUserReportPriceLevels = objCompEmpRepo.GetUserReportPriceLevels(userInfoID);
                        dlReportPriceLevels.DataSource = this.lstUserReports.Where(le => Convert.ToBoolean(le.IsActive)).ToList();
                        dlReportPriceLevels.DataBind();
                    }

                    #endregion
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private Boolean SaveBasic()
    {
        try
        {
            Page.Validate("Basic");
            if (Page.IsValid)
            {
                Int64 userInfoID = Convert.ToInt64(hdnBasicUserInfoID.Value);

                UserInformationRepository objUserRepo = new UserInformationRepository();
                CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();

                UserInformation objUserInfo = objUserRepo.GetById(userInfoID);
                CompanyEmployee objCompanyEmployee = objEmpRepo.GetByUserInfoId(objUserInfo.UserInfoID);

                if (!objUserRepo.CheckEmailExistence(txtBasicEmail.Text.Trim(), userInfoID))
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "EmployeeDetailsPopup('basic');GeneralAlertMsg('This email already exists in the system.', true);", true);
                    return false;
                }

                if (!String.IsNullOrEmpty(txtBasicEmployeeID.Text.Trim()))
                {
                    if (!objEmpRepo.CheckEmployeeIDExistence(txtBasicEmployeeID.Text.Trim(), Convert.ToInt64(this.CompanyID), userInfoID))
                    {
                        this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "EmployeeDetailsPopup('basic');GeneralAlertMsg('This employee id already exists in the system.', true);", true);
                        return false;
                    }
                    else if (!new RegistrationRepository().CheckEmployeeIDExistence(txtBasicEmployeeID.Text.Trim(), Convert.ToInt64(this.CompanyID), userInfoID))
                    {
                        this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "EmployeeDetailsPopup('basic');GeneralAlertMsg('This employee id already exists in registration requests.', true);", true);
                        return false;
                    }
                }

                objUserInfo.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objUserInfo.CreatedDate = DateTime.Now;
                objUserInfo.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtBasicFirstName.Text.Trim().ToLower());
                objUserInfo.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtBasicLastName.Text.Trim().ToLower());
                objUserInfo.WLSStatusId = Convert.ToInt32(ddlBasicSystemStatus.SelectedValue);
                objUserInfo.Email = txtBasicEmail.Text.Trim();
                objUserInfo.LoginEmail = txtBasicEmail.Text.Trim();
                objUserInfo.Password = txtBasicPassword.Text.Trim();
                objUserInfo.UpdatedDate = DateTime.Now;
                objUserInfo.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objUserInfo.CompanyId = this.CompanyID;
                objUserInfo.Usertype = Convert.ToInt64(ddlBasicSystemAccess.SelectedValue);

                objUserInfo.ClimateSettingId = null;
                objUserRepo.SubmitChanges();

                objCompanyEmployee.CompanyEmail = txtBasicEmail.Text.Trim();

                if (ddlBasicSystemAccess.SelectedItem.Text == Incentex.DA.DAEnums.CompanyEmployeeTypes.Admin.ToString())
                {
                    objCompanyEmployee.isCompanyAdmin = true;
                    objCompanyEmployee.IsMOASApprover = false;
                    objCompanyEmployee.ManagementControlForWorkgroup = Convert.ToInt64(ddlBasicWorkGroup.SelectedValue);
                    objCompanyEmployee.ManagementControlForRegion = null;
                    objCompanyEmployee.ManagementControlForStaionlocation = Convert.ToInt64(ddlBasicBaseStation.SelectedValue);
                }
                else
                {
                    objCompanyEmployee.isCompanyAdmin = false;
                    objCompanyEmployee.IsMOASApprover = false;
                    objCompanyEmployee.ManagementControlForWorkgroup = null;
                    objCompanyEmployee.ManagementControlForDepartment = null;
                    objCompanyEmployee.ManagementControlForRegion = null;
                    objCompanyEmployee.ManagementControlForStaionlocation = null;
                }

                if (!String.IsNullOrEmpty(txtBasicDateOfHire.Text.Trim()))
                    objCompanyEmployee.HirerdDate = Convert.ToDateTime(txtBasicDateOfHire.Text.Trim());

                objCompanyEmployee.EmployeeID = txtBasicEmployeeID.Text.Trim();

                if (ddlBasicPosition.SelectedIndex != 0)
                    objCompanyEmployee.EmployeeTitleId = Convert.ToInt64(ddlBasicPosition.SelectedItem.Value);

                objCompanyEmployee.BaseStation = Convert.ToInt64(ddlBasicBaseStation.SelectedValue);
                objCompanyEmployee.GenderID = Convert.ToInt64(ddlBasicGender.SelectedValue);
                objCompanyEmployee.WorkgroupID = Convert.ToInt64(ddlBasicWorkGroup.SelectedValue);

                if (ddlBasicSystemStatus.SelectedItem.Text == "Active")
                    objCompanyEmployee.LastActiveDate = DateTime.Now;
                else
                    objCompanyEmployee.LastInActiveDate = DateTime.Now;

                objCompanyEmployee.EmpIssuancePolicyStatus = Convert.ToInt64(ddlBasicIssuancePolicy.SelectedValue);

                objCompanyEmployee.RegionID = null;
                objCompanyEmployee.DepartmentID = null;
                objCompanyEmployee.UploadImage = null;
                objCompanyEmployee.IsMOASStationLevelApprover = null;

                objEmpRepo.SubmitChanges();
                return true;
            }
            else
                return false;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "EmployeeDetailsPopup('basic');GeneralAlertMsg('An error occured : " + ex.Message + "', true)", true);
            return false;
        }
    }

    private Boolean SaveMenuAccess()
    {
        try
        {
            Int64 userInfoID = Convert.ToInt64(hdnBasicUserInfoID.Value);

            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
            CompanyEmployee objCompanyEmployee = objEmpRepo.GetByUserInfoId(userInfoID);

            #region Payment Options

            //Stuff for MOAS payment options
            UserPaymentOption objUserMOASPaymentOption = new UserPaymentOption();
            UserMOASPaymentOptionFor objUserMOASPaymentOptionFor = objEmpRepo.SetUserMOASPaymentOptionFor(userInfoID);

            if (objUserMOASPaymentOptionFor != null)
                objEmpRepo.Delete(objUserMOASPaymentOptionFor);

            objUserMOASPaymentOptionFor = new UserMOASPaymentOptionFor();

            List<UserPaymentOption> lstUserPaymentOptions = objEmpRepo.SetUserPaymentOptions(userInfoID);
            objEmpRepo.DeleteAll(lstUserPaymentOptions);

            foreach (RepeaterItem item in repPaymentOptions.Items)
            {
                HiddenField hdnPaymentOptionID = (HiddenField)item.FindControl("hdnPaymentOptionID");

                if (hdnPaymentOptionID != null && !String.IsNullOrEmpty(hdnPaymentOptionID.Value))
                {
                    CheckBox chkPaymentOption = (CheckBox)item.FindControl("chkPaymentOption");

                    if (Convert.ToInt64(hdnPaymentOptionID.Value) != Convert.ToInt32(Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS))
                    {
                        if (chkPaymentOption != null && chkPaymentOption.Checked)
                        {
                            UserPaymentOption objUserPaymentOption = new UserPaymentOption();
                            objUserPaymentOption.UserInfoID = userInfoID;
                            objUserPaymentOption.CompanyID = Convert.ToInt64(this.CompanyID);
                            objUserPaymentOption.PaymentOptionID = Convert.ToInt64(hdnPaymentOptionID.Value);
                            objEmpRepo.Insert(objUserPaymentOption);
                        }
                    }
                    else
                    {
                        //Removing MOAS Approvers
                        List<UserMOASApprover> lstMOASApprovers = objEmpRepo.SetUserMOASApprovers(userInfoID);
                        objEmpRepo.DeleteAll(lstMOASApprovers);

                        CheckBox chkMOASIssuancePurchase = (CheckBox)item.FindControl("chkMOASIssuancePurchase");
                        DataList dlMOASApprovers = (DataList)item.FindControl("dlMOASApprovers");

                        if ((chkPaymentOption != null && chkPaymentOption.Checked) || (chkMOASIssuancePurchase != null && chkMOASIssuancePurchase.Checked))
                        {
                            objUserMOASPaymentOption.UserInfoID = userInfoID;
                            objUserMOASPaymentOption.CompanyID = Convert.ToInt64(this.CompanyID);
                            objUserMOASPaymentOption.PaymentOptionID = Convert.ToInt64(hdnPaymentOptionID.Value);
                            objEmpRepo.Insert(objUserMOASPaymentOption);

                            objUserMOASPaymentOptionFor.IsForIssuancePurchase = chkMOASIssuancePurchase.Checked;
                            objUserMOASPaymentOptionFor.IsForStorePurchase = chkPaymentOption.Checked;
                            objUserMOASPaymentOptionFor.UserInfoID = userInfoID;
                        }

                        foreach (DataListItem approverItem in dlMOASApprovers.Items)
                        {
                            CheckBox chkApprover = (CheckBox)approverItem.FindControl("chkApprover");

                            if (chkApprover != null && chkApprover.Checked)
                            {
                                HiddenField hdnApproverID = (HiddenField)approverItem.FindControl("hdnApproverID");
                                TextBox txtApproverLevel = (TextBox)approverItem.FindControl("txtApproverLevel");

                                UserMOASApprover objUserMOASApprover = new UserMOASApprover();
                                objUserMOASApprover.UserInfoID = userInfoID;
                                objUserMOASApprover.CompanyID = Convert.ToInt64(this.CompanyID);
                                objUserMOASApprover.ApproverID = Convert.ToInt64(hdnApproverID.Value);
                                objUserMOASApprover.ApproverLevel = Convert.ToInt16(txtApproverLevel.Text.Trim());
                                objEmpRepo.Insert(objUserMOASApprover);
                            }
                        }
                    }
                }
            }

            objEmpRepo.SubmitChanges();

            if (objUserMOASPaymentOption != null && objUserMOASPaymentOption.UserPaymentOptionID > 0)
            {
                objUserMOASPaymentOptionFor.UserMOASPaymentOptionID = objUserMOASPaymentOption.UserPaymentOptionID;
                objEmpRepo.Insert(objUserMOASPaymentOptionFor);
            }

            #endregion

            #region Product Categories

            List<UserCategoryAccess> lstCategoryAccess = objEmpRepo.SetUserCategoryAccess(userInfoID);
            objEmpRepo.DeleteAll(lstCategoryAccess);

            foreach (RepeaterItem repItem in repCategories.Items)
            {
                if (repItem.ItemType == ListItemType.Item || repItem.ItemType == ListItemType.AlternatingItem)
                {
                    DataList dlSubCategories = (DataList)repItem.FindControl("dlSubCategories");
                    HiddenField hdnCategoryID = (HiddenField)repItem.FindControl("hdnCategoryID");

                    foreach (DataListItem dlItem in dlSubCategories.Items)
                    {
                        HiddenField hdnSubCategoryID = (HiddenField)dlItem.FindControl("hdnSubCategoryID");
                        CheckBox chkSubCategory = (CheckBox)dlItem.FindControl("chkSubCategory");

                        if (chkSubCategory.Checked)
                        {
                            UserCategoryAccess objCategoryAccess = new UserCategoryAccess();
                            objCategoryAccess.CategoryID = Convert.ToInt64(hdnCategoryID.Value);
                            objCategoryAccess.SubCategoryID = Convert.ToInt64(hdnSubCategoryID.Value);
                            objCategoryAccess.UserInfoID = userInfoID;
                            objEmpRepo.Insert(objCategoryAccess);
                        }
                    }
                }
            }

            #endregion

            #region Shipping Methods

            StringBuilder sbShippingMethods = new StringBuilder();

            foreach (RepeaterItem item in repShippingMethods.Items)
            {
                CheckBox chkShippingMethod = (CheckBox)item.FindControl("chkShippingMethod");

                if (chkShippingMethod != null && chkShippingMethod.Checked)
                {
                    HiddenField hdnShippingMethodID = (HiddenField)item.FindControl("hdnShippingMethodID");

                    if (hdnShippingMethodID != null && !String.IsNullOrEmpty(hdnShippingMethodID.Value))
                    {
                        sbShippingMethods.Append((sbShippingMethods.Length > 0 ? "," : "") + hdnShippingMethodID.Value);
                    }
                }
            }

            objCompanyEmployee.ShippingMethods = sbShippingMethods.ToString();

            #endregion

            objEmpRepo.SubmitChanges();

            #region Manage Emails

            CompanyEmpManageEmailRepository objCompanyEmpManageEmailRepo = new CompanyEmpManageEmailRepository();
            List<CompanyEmpManageEmail> lstExistingManageEmailSettings = objCompanyEmpManageEmailRepo.GetEmailRightsByUserInfoID(objCompanyEmployee.UserInfoID);

            objCompanyEmpManageEmailRepo.DeleteAll(lstExistingManageEmailSettings);
            objCompanyEmpManageEmailRepo.SubmitChanges();

            foreach (RepeaterItem item in repManageEmails.Items)
            {
                CheckBox chkManageEmail = (CheckBox)item.FindControl("chkManageEmail");

                if (chkManageEmail != null && chkManageEmail.Checked)
                {
                    HiddenField hdnManageEmailID = (HiddenField)item.FindControl("hdnManageEmailID");

                    if (hdnManageEmailID != null && !String.IsNullOrEmpty(hdnManageEmailID.Value))
                    {
                        CompanyEmpManageEmail objCompanyEmpManageEmail = new CompanyEmpManageEmail();
                        objCompanyEmpManageEmail.UserInfoID = objCompanyEmployee.UserInfoID;
                        objCompanyEmpManageEmail.ManageEmailID = Convert.ToInt64(hdnManageEmailID.Value);

                        objCompanyEmpManageEmailRepo.Insert(objCompanyEmpManageEmail);
                    }
                }
            }

            objCompanyEmpManageEmailRepo.SubmitChanges();

            #endregion

            return true;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "EmployeeDetailsPopup('menuaccess');GeneralAlertMsg('An error occured : " + ex.Message + "', true)", true);
            return false;
        }
    }

    private Boolean SaveAdminSettings()
    {
        try
        {
            Int64 userInfoID = Convert.ToInt64(hdnBasicUserInfoID.Value);
            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();

            #region Station Management

            if (!String.IsNullOrEmpty(hdnCurrentSection.Value) && hdnCurrentSection.Value == "stationmanagement")
            {
                List<UserStationWorkGroup> lstUserStationWorkGroups = objEmpRepo.SetUserStationWorkGroup(userInfoID);
                objEmpRepo.DeleteAll(lstUserStationWorkGroups);

                List<UserStationPrivilege> lstUserStationPrivileges = objEmpRepo.SetUserStationPrivilege(userInfoID);
                objEmpRepo.DeleteAll(lstUserStationPrivileges);
                objEmpRepo.SubmitChanges();

                List<UserStation> lstOldStations = objEmpRepo.SetUserStations(userInfoID);
                objEmpRepo.DeleteAll(lstOldStations);

                List<UserStation> lstNewStations = new List<UserStation>();

                foreach (DataListItem dlItem in dlStations.Items)
                {
                    CheckBox chkStation = (CheckBox)dlItem.FindControl("chkStation");

                    if (chkStation != null && chkStation.Checked)
                    {
                        HiddenField hdnStationID = (HiddenField)dlItem.FindControl("hdnStationID");

                        if (hdnStationID != null && !String.IsNullOrEmpty(hdnStationID.Value) && Convert.ToInt64(hdnStationID.Value) > 0)
                        {
                            UserStation objUserStation = new UserStation();
                            objUserStation.UserInfoID = userInfoID;
                            objUserStation.StationID = Convert.ToInt64(hdnStationID.Value);
                            objEmpRepo.Insert(objUserStation);
                            lstNewStations.Add(objUserStation);
                        }
                    }
                }

                objEmpRepo.SubmitChanges();

                foreach (UserStation objNewUserStation in lstNewStations)
                {
                    UserStation objOldStation = lstOldStations.FirstOrDefault(le => le.StationID == objNewUserStation.StationID);

                    if (objOldStation != null)
                    {
                        foreach (UserStationWorkGroup objUserStationWorkGroup in lstUserStationWorkGroups.Where(le => le.UserStationID == objOldStation.ID))
                        {
                            UserStationWorkGroup objNewUserStationWorkGroup = new UserStationWorkGroup();
                            objNewUserStationWorkGroup.UserInfoID = userInfoID;
                            objNewUserStationWorkGroup.UserStationID = objNewUserStation.ID;
                            objNewUserStationWorkGroup.WorkGroupID = objUserStationWorkGroup.WorkGroupID;
                            objEmpRepo.Insert(objNewUserStationWorkGroup);
                        }

                        foreach (UserStationPrivilege objUserStationPrivilege in lstUserStationPrivileges.Where(le => le.UserStationID == objOldStation.ID))
                        {
                            UserStationPrivilege objNewUserStationPrivilege = new UserStationPrivilege();
                            objNewUserStationPrivilege.UserInfoID = userInfoID;
                            objNewUserStationPrivilege.UserStationID = objNewUserStation.ID;
                            objNewUserStationPrivilege.PrivilegeID = objUserStationPrivilege.PrivilegeID;
                            objEmpRepo.Insert(objNewUserStationPrivilege);
                        }
                    }
                }

                objEmpRepo.SubmitChanges();
            }

            #endregion

            #region Workgroup Management

            if (!String.IsNullOrEmpty(hdnCurrentSection.Value) && hdnCurrentSection.Value == "workgroupmanagement")
            {
                List<UserStationWorkGroup> lstUserStationWorkGroups = objEmpRepo.SetUserStationWorkGroup(userInfoID);
                objEmpRepo.DeleteAll(lstUserStationWorkGroups);
                objEmpRepo.SubmitChanges();

                foreach (DataListItem dlItem in dlStationWorkGroups.Items)
                {
                    HiddenField hdnUserStationID = (HiddenField)dlItem.FindControl("hdnUserStationID");

                    if (hdnUserStationID != null && !String.IsNullOrEmpty(hdnUserStationID.Value) && Convert.ToInt64(hdnUserStationID.Value) > 0)
                    {
                        MultiSelectDropDown msdStationWorkGroups = (MultiSelectDropDown)dlItem.FindControl("msdStationWorkGroups");

                        if (msdStationWorkGroups != null)
                        {
                            foreach (RepeaterItem repItem in msdStationWorkGroups.Items)
                            {
                                CheckBox chkMultiSelect = (CheckBox)repItem.FindControl("chkMultiSelect");

                                if (chkMultiSelect != null && chkMultiSelect.Checked)
                                {
                                    HiddenField hdnMultiSelectID = (HiddenField)repItem.FindControl("hdnMultiSelectID");

                                    if (hdnMultiSelectID != null && !String.IsNullOrEmpty(hdnMultiSelectID.Value) && Convert.ToInt64(hdnMultiSelectID.Value) > 0)
                                    {
                                        UserStationWorkGroup objUserStationWorkGroup = new UserStationWorkGroup();
                                        objUserStationWorkGroup.UserInfoID = userInfoID;
                                        objUserStationWorkGroup.UserStationID = Convert.ToInt64(hdnUserStationID.Value);
                                        objUserStationWorkGroup.WorkGroupID = Convert.ToInt64(hdnMultiSelectID.Value);
                                        objEmpRepo.Insert(objUserStationWorkGroup);

                                        if (msdStationWorkGroups.DataSelectText != "- Selected - ")
                                            msdStationWorkGroups.DataSelectText = "- Selected - ";
                                    }
                                }
                            }
                        }
                    }
                }

                objEmpRepo.SubmitChanges();
            }

            #endregion

            #region Privileges

            if (!String.IsNullOrEmpty(hdnCurrentSection.Value) && hdnCurrentSection.Value == "privileges")
            {
                List<UserStationPrivilege> lstUserStationPrivileges = objEmpRepo.SetUserStationPrivilege(userInfoID);
                objEmpRepo.DeleteAll(lstUserStationPrivileges);
                objEmpRepo.SubmitChanges();

                foreach (DataListItem dlItem in dlStationPrivileges.Items)
                {
                    HiddenField hdnUserStationID = (HiddenField)dlItem.FindControl("hdnUserStationID");

                    if (hdnUserStationID != null && !String.IsNullOrEmpty(hdnUserStationID.Value) && Convert.ToInt64(hdnUserStationID.Value) > 0)
                    {
                        MultiSelectDropDown msdStationPrivileges = (MultiSelectDropDown)dlItem.FindControl("msdStationPrivileges");

                        if (msdStationPrivileges != null)
                        {
                            foreach (RepeaterItem repItem in msdStationPrivileges.Items)
                            {
                                CheckBox chkMultiSelect = (CheckBox)repItem.FindControl("chkMultiSelect");

                                if (chkMultiSelect != null && chkMultiSelect.Checked)
                                {
                                    HiddenField hdnMultiSelectID = (HiddenField)repItem.FindControl("hdnMultiSelectID");

                                    if (hdnMultiSelectID != null && !String.IsNullOrEmpty(hdnMultiSelectID.Value) && Convert.ToInt64(hdnMultiSelectID.Value) > 0)
                                    {
                                        UserStationPrivilege objUserStationPrivilege = new UserStationPrivilege();
                                        objUserStationPrivilege.UserInfoID = userInfoID;
                                        objUserStationPrivilege.UserStationID = Convert.ToInt64(hdnUserStationID.Value);
                                        objUserStationPrivilege.PrivilegeID = Convert.ToInt64(hdnMultiSelectID.Value);
                                        objEmpRepo.Insert(objUserStationPrivilege);

                                        if (msdStationPrivileges.DataSelectText != "- Selected - ")
                                            msdStationPrivileges.DataSelectText = "- Selected - ";
                                    }
                                }
                            }
                        }
                    }
                }

                objEmpRepo.SubmitChanges();
            }

            #endregion

            return true;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "EmployeeDetailsPopup('adminsettings', '" + hdnCurrentSection.Value + "');GeneralAlertMsg('An error occured : " + ex.Message + "', true)", true);
            return false;
        }
    }

    private Boolean SaveReports()
    {
        try
        {
            Int64 userInfoID = Convert.ToInt64(hdnBasicUserInfoID.Value);
            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();

            #region Main Reports

            if (!String.IsNullOrEmpty(hdnCurrentSection.Value) && hdnCurrentSection.Value == "mainreports")
            {
                List<UserReportPriceLevel> lstUserReportPriceLevels = objEmpRepo.SetUserReportPriceLevels(userInfoID);
                objEmpRepo.DeleteAll(lstUserReportPriceLevels);

                List<UserReportStation> lstUserReportStations = objEmpRepo.SetUserReportStations(userInfoID);
                objEmpRepo.DeleteAll(lstUserReportStations);

                List<UserReportWorkGroup> lstUserReportWorkGroups = objEmpRepo.SetUserReportWorkGroups(userInfoID);
                objEmpRepo.DeleteAll(lstUserReportWorkGroups);

                List<UserSubReport> lstUserSubReports = objEmpRepo.SetUserSubReports(userInfoID);
                objEmpRepo.DeleteAll(lstUserSubReports);
                objEmpRepo.SubmitChanges();

                List<UserReport> lstOldReports = objEmpRepo.SetUserReports(userInfoID);
                objEmpRepo.DeleteAll(lstOldReports);

                List<UserReport> lstNewReports = new List<UserReport>();

                foreach (DataListItem dlItem in dlReports.Items)
                {
                    CheckBox chkReport = (CheckBox)dlItem.FindControl("chkReport");

                    if (chkReport != null && chkReport.Checked)
                    {
                        HiddenField hdnReportID = (HiddenField)dlItem.FindControl("hdnReportID");

                        if (hdnReportID != null && !String.IsNullOrEmpty(hdnReportID.Value) && Convert.ToInt64(hdnReportID.Value) > 0)
                        {
                            UserReport objUserReport = new UserReport();
                            objUserReport.UserInfoID = userInfoID;
                            objUserReport.ReportID = Convert.ToInt64(hdnReportID.Value);
                            objEmpRepo.Insert(objUserReport);
                            lstNewReports.Add(objUserReport);
                        }
                    }
                }

                objEmpRepo.SubmitChanges();

                foreach (UserReport objNewUserReport in lstNewReports)
                {
                    UserReport objOldReport = lstOldReports.FirstOrDefault(le => le.ReportID == objNewUserReport.ReportID);

                    if (objOldReport != null)
                    {
                        foreach (UserSubReport objUserSubReport in lstUserSubReports.Where(le => le.UserReportID == objOldReport.ID))
                        {
                            UserSubReport objNewUserSubReport = new UserSubReport();
                            objNewUserSubReport.UserInfoID = userInfoID;
                            objNewUserSubReport.UserReportID = objNewUserReport.ID;
                            objNewUserSubReport.SubReportID = objUserSubReport.SubReportID;
                            objEmpRepo.Insert(objNewUserSubReport);
                        }

                        foreach (UserReportWorkGroup objUserReportWorkGroup in lstUserReportWorkGroups.Where(le => le.UserReportID == objOldReport.ID))
                        {
                            UserReportWorkGroup objNewUserReportWorkGroup = new UserReportWorkGroup();
                            objNewUserReportWorkGroup.UserInfoID = userInfoID;
                            objNewUserReportWorkGroup.UserReportID = objNewUserReport.ID;
                            objNewUserReportWorkGroup.WorkGroupID = objUserReportWorkGroup.WorkGroupID;
                            objEmpRepo.Insert(objNewUserReportWorkGroup);
                        }

                        foreach (UserReportStation objUserReportStation in lstUserReportStations.Where(le => le.UserReportID == objOldReport.ID))
                        {
                            UserReportStation objNewUserReportStation = new UserReportStation();
                            objNewUserReportStation.UserInfoID = userInfoID;
                            objNewUserReportStation.UserReportID = objNewUserReport.ID;
                            objNewUserReportStation.StationID = objUserReportStation.StationID;
                            objEmpRepo.Insert(objNewUserReportStation);
                        }

                        foreach (UserReportPriceLevel objUserReportPriceLevel in lstUserReportPriceLevels.Where(le => le.UserReportID == objOldReport.ID))
                        {
                            UserReportPriceLevel objNewUserReportPriceLevel = new UserReportPriceLevel();
                            objNewUserReportPriceLevel.UserInfoID = userInfoID;
                            objNewUserReportPriceLevel.UserReportID = objNewUserReport.ID;
                            objNewUserReportPriceLevel.PriceLevelID = objUserReportPriceLevel.PriceLevelID;
                            objEmpRepo.Insert(objNewUserReportPriceLevel);
                        }
                    }
                }

                objEmpRepo.SubmitChanges();
            }

            #endregion

            #region Sub Reports

            if (!String.IsNullOrEmpty(hdnCurrentSection.Value) && hdnCurrentSection.Value == "subreports")
            {
                List<UserSubReport> lstUserSubReports = objEmpRepo.SetUserSubReports(userInfoID);
                objEmpRepo.DeleteAll(lstUserSubReports);
                objEmpRepo.SubmitChanges();

                foreach (DataListItem dlItem in dlSubReports.Items)
                {
                    HiddenField hdnUserReportID = (HiddenField)dlItem.FindControl("hdnUserReportID");

                    if (hdnUserReportID != null && !String.IsNullOrEmpty(hdnUserReportID.Value) && Convert.ToInt64(hdnUserReportID.Value) > 0)
                    {
                        MultiSelectDropDown msdSubReports = (MultiSelectDropDown)dlItem.FindControl("msdSubReports");

                        if (msdSubReports != null)
                        {
                            foreach (RepeaterItem repItem in msdSubReports.Items)
                            {
                                CheckBox chkMultiSelect = (CheckBox)repItem.FindControl("chkMultiSelect");

                                if (chkMultiSelect != null && chkMultiSelect.Checked)
                                {
                                    HiddenField hdnMultiSelectID = (HiddenField)repItem.FindControl("hdnMultiSelectID");

                                    if (hdnMultiSelectID != null && !String.IsNullOrEmpty(hdnMultiSelectID.Value) && Convert.ToInt64(hdnMultiSelectID.Value) > 0)
                                    {
                                        UserSubReport objUserSubReport = new UserSubReport();
                                        objUserSubReport.UserInfoID = userInfoID;
                                        objUserSubReport.UserReportID = Convert.ToInt64(hdnUserReportID.Value);
                                        objUserSubReport.SubReportID = Convert.ToInt64(hdnMultiSelectID.Value);
                                        objEmpRepo.Insert(objUserSubReport);

                                        if (msdSubReports.DataSelectText != "- Selected - ")
                                            msdSubReports.DataSelectText = "- Selected - ";
                                    }
                                }
                            }
                        }
                    }
                }

                objEmpRepo.SubmitChanges();
            }

            #endregion

            #region Report Workgroups

            if (!String.IsNullOrEmpty(hdnCurrentSection.Value) && hdnCurrentSection.Value == "workgroups")
            {
                List<UserReportWorkGroup> lstUserReportWorkGroups = objEmpRepo.SetUserReportWorkGroups(userInfoID);
                objEmpRepo.DeleteAll(lstUserReportWorkGroups);
                objEmpRepo.SubmitChanges();

                foreach (DataListItem dlItem in dlReportWorkGroups.Items)
                {
                    HiddenField hdnUserReportID = (HiddenField)dlItem.FindControl("hdnUserReportID");

                    if (hdnUserReportID != null && !String.IsNullOrEmpty(hdnUserReportID.Value) && Convert.ToInt64(hdnUserReportID.Value) > 0)
                    {
                        MultiSelectDropDown msdReportWorkGroups = (MultiSelectDropDown)dlItem.FindControl("msdReportWorkGroups");

                        if (msdReportWorkGroups != null)
                        {
                            foreach (RepeaterItem repItem in msdReportWorkGroups.Items)
                            {
                                CheckBox chkMultiSelect = (CheckBox)repItem.FindControl("chkMultiSelect");

                                if (chkMultiSelect != null && chkMultiSelect.Checked)
                                {
                                    HiddenField hdnMultiSelectID = (HiddenField)repItem.FindControl("hdnMultiSelectID");

                                    if (hdnMultiSelectID != null && !String.IsNullOrEmpty(hdnMultiSelectID.Value) && Convert.ToInt64(hdnMultiSelectID.Value) > 0)
                                    {
                                        UserReportWorkGroup objUserReportWorkGroup = new UserReportWorkGroup();
                                        objUserReportWorkGroup.UserInfoID = userInfoID;
                                        objUserReportWorkGroup.UserReportID = Convert.ToInt64(hdnUserReportID.Value);
                                        objUserReportWorkGroup.WorkGroupID = Convert.ToInt64(hdnMultiSelectID.Value);
                                        objEmpRepo.Insert(objUserReportWorkGroup);

                                        if (msdReportWorkGroups.DataSelectText != "- Selected - ")
                                            msdReportWorkGroups.DataSelectText = "- Selected - ";
                                    }
                                }
                            }
                        }
                    }
                }

                objEmpRepo.SubmitChanges();
            }

            #endregion

            #region Report Stations

            if (!String.IsNullOrEmpty(hdnCurrentSection.Value) && hdnCurrentSection.Value == "stations")
            {
                List<UserReportStation> lstUserReportStations = objEmpRepo.SetUserReportStations(userInfoID);
                objEmpRepo.DeleteAll(lstUserReportStations);
                objEmpRepo.SubmitChanges();

                foreach (DataListItem dlItem in dlReportStations.Items)
                {
                    HiddenField hdnUserReportID = (HiddenField)dlItem.FindControl("hdnUserReportID");

                    if (hdnUserReportID != null && !String.IsNullOrEmpty(hdnUserReportID.Value) && Convert.ToInt64(hdnUserReportID.Value) > 0)
                    {
                        MultiSelectDropDown msdReportStations = (MultiSelectDropDown)dlItem.FindControl("msdReportStations");

                        if (msdReportStations != null)
                        {
                            foreach (RepeaterItem repItem in msdReportStations.Items)
                            {
                                CheckBox chkMultiSelect = (CheckBox)repItem.FindControl("chkMultiSelect");

                                if (chkMultiSelect != null && chkMultiSelect.Checked)
                                {
                                    HiddenField hdnMultiSelectID = (HiddenField)repItem.FindControl("hdnMultiSelectID");

                                    if (hdnMultiSelectID != null && !String.IsNullOrEmpty(hdnMultiSelectID.Value) && Convert.ToInt64(hdnMultiSelectID.Value) > 0)
                                    {
                                        UserReportStation objUserReportStation = new UserReportStation();
                                        objUserReportStation.UserInfoID = userInfoID;
                                        objUserReportStation.UserReportID = Convert.ToInt64(hdnUserReportID.Value);
                                        objUserReportStation.StationID = Convert.ToInt64(hdnMultiSelectID.Value);
                                        objEmpRepo.Insert(objUserReportStation);

                                        if (msdReportStations.DataSelectText != "- Selected - ")
                                            msdReportStations.DataSelectText = "- Selected - ";
                                    }
                                }
                            }
                        }
                    }
                }

                objEmpRepo.SubmitChanges();
            }

            #endregion

            #region Report Price Levels

            if (!String.IsNullOrEmpty(hdnCurrentSection.Value) && hdnCurrentSection.Value == "pricelevels")
            {
                List<UserReportPriceLevel> lstUserReportPriceLevels = objEmpRepo.SetUserReportPriceLevels(userInfoID);
                objEmpRepo.DeleteAll(lstUserReportPriceLevels);
                objEmpRepo.SubmitChanges();

                foreach (DataListItem dlItem in dlReportPriceLevels.Items)
                {
                    HiddenField hdnUserReportID = (HiddenField)dlItem.FindControl("hdnUserReportID");

                    if (hdnUserReportID != null && !String.IsNullOrEmpty(hdnUserReportID.Value) && Convert.ToInt64(hdnUserReportID.Value) > 0)
                    {
                        MultiSelectDropDown msdReportPriceLevels = (MultiSelectDropDown)dlItem.FindControl("msdReportPriceLevels");

                        if (msdReportPriceLevels != null)
                        {
                            foreach (RepeaterItem repItem in msdReportPriceLevels.Items)
                            {
                                CheckBox chkMultiSelect = (CheckBox)repItem.FindControl("chkMultiSelect");

                                if (chkMultiSelect != null && chkMultiSelect.Checked)
                                {
                                    HiddenField hdnMultiSelectID = (HiddenField)repItem.FindControl("hdnMultiSelectID");

                                    if (hdnMultiSelectID != null && !String.IsNullOrEmpty(hdnMultiSelectID.Value) && Convert.ToInt64(hdnMultiSelectID.Value) > 0)
                                    {
                                        UserReportPriceLevel objUserReportPriceLevel = new UserReportPriceLevel();
                                        objUserReportPriceLevel.UserInfoID = userInfoID;
                                        objUserReportPriceLevel.UserReportID = Convert.ToInt64(hdnUserReportID.Value);
                                        objUserReportPriceLevel.PriceLevelID = Convert.ToInt64(hdnMultiSelectID.Value);
                                        objEmpRepo.Insert(objUserReportPriceLevel);

                                        if (msdReportPriceLevels.DataSelectText != "- Selected - ")
                                            msdReportPriceLevels.DataSelectText = "- Selected - ";
                                    }
                                }
                            }
                        }
                    }
                }

                objEmpRepo.SubmitChanges();
            }

            #endregion

            return true;
        }
        catch (Exception ex)
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "EmployeeDetailsPopup('reports');GeneralAlertMsg('An error occured : " + ex.Message + "', true)", true);
            ErrHandler.WriteError(ex);
            return false;
        }
    }

    #endregion
}