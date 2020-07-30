using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class OrderManagement_OrderLookupSearch : PageBase
{
    #region Data Members

    String toDate = null;
    String fromDate = null;
    String storeID = null;
    String emailAddress = null;
    String orderNumber = null;
    String orderStatus = null;
    String firstName = null;
    String lastName = null;
    String stationCode = null;
    String employeeCode = null;
    String workGroup = null;
    String paymentType = null;
    String SAPImportStatus = null;
    String hireDateBefore = null;
    String orderPlaced = null;

    /// <summary>
    ///  Current User Base Stations Access
    /// </summary>
    String BaseStationsAccess
    {
        get
        {
            return Convert.ToString(ViewState["BaseStationsAccess"]);
        }
        set
        {
            ViewState["BaseStationsAccess"] = value;
        }
    }
    BaseStationRepository objBaseRepos = new BaseStationRepository();
    LookupRepository objLookRep = new LookupRepository();
    List<INC_BasedStation> objList = new List<INC_BasedStation>();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    UserInformation objuser = new UserInformation();
    UserInformationRepository objuserrepos = new UserInformationRepository();

    #endregion

    #region Event Handlers

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Order Management System";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ddlCompanyStore.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            txtFromDate.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            txtToDate.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            ddlOrderStatus.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            txtOrderNumber.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            txtFName.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            txtLName.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            txtEmpNumber.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            txtEmail.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                ddlStationCodeCA.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            else
                ddlStationCodeIE.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            ddlPaymentOption.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            ddlSAPImportStatus.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");
            ddlOrderPlaced.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + lnkSubmitRequest.ClientID + "');");

            ((Label)Master.FindControl("lblPageHeading")).Text = "Order Lookup";

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            {
                IncentexGlobal.ManageID = 9;
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";

                CompanyEmployee objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                objuser = objuserrepos.GetById(objCmpnyInfo.UserInfoID);
                if (objCmpnyInfo != null)
                    BaseStationsAccess = objCmpnyInfo.BaseStationsAccess;

                trSAPImportStatus.Visible = false;
                trHireDateBefore.Visible = true;
            }
            else
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    trSAPImportStatus.Visible = true;
                else
                    trSAPImportStatus.Visible = false;

                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";
            }

            FillCompanyStore();
            FillOrderStaus();
            FillBaseStation();
            FillPaymentOption();
            FillWorkgroup();
            FillOrderPlaced();
            lblmsg.Text = "Please select one or more search criteria and run the report.";
        }
    }

    protected void lnkSubmitRequest_Click(Object sender, EventArgs e)
    {
        if (ddlCompanyStore.SelectedIndex > 0)
            storeID = ddlCompanyStore.SelectedValue;

        if (txtToDate.Text != "")
            toDate = txtToDate.Text.Trim();

        if (txtFromDate.Text != "")
            fromDate = txtFromDate.Text.Trim();

        if (txtEmail.Text != "")
            emailAddress = txtEmail.Text.Trim();

        if (txtOrderNumber.Text != "")
            orderNumber = txtOrderNumber.Text.Trim();

        if (ddlOrderStatus.SelectedIndex > 0)
            orderStatus = ddlOrderStatus.SelectedItem.Text;

        if (txtFName.Text != "")
            firstName = txtFName.Text.Trim();

        if (txtLName.Text != "")
            lastName = txtLName.Text.Trim();

        if (txtEmpNumber.Text != "")
            employeeCode = txtEmpNumber.Text.Trim();

        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
        {
            if (ddlStationCodeCA.SelectedIndex > 0)
                stationCode = ddlStationCodeCA.SelectedValue;
        }
        else
        {
            if (ddlStationCodeIE.SelectedIndex > 0)
                stationCode = ddlStationCodeIE.SelectedValue;
        }

        if (ddlWorkgroup.SelectedValue != "0")
            workGroup = ddlWorkgroup.SelectedValue;

        if (ddlPaymentOption.SelectedIndex > 0)
            paymentType = ddlPaymentOption.SelectedValue;

        if (txtHireDateBefore.Text != "")
            hireDateBefore = txtHireDateBefore.Text.Trim();

        if (ddlSAPImportStatus.SelectedIndex > 0)
            SAPImportStatus = Convert.ToBoolean(Convert.ToByte(ddlSAPImportStatus.SelectedValue)).ToString();

        if (ddlOrderPlaced.SelectedIndex > 0)
        {
            if (ddlOrderPlaced.SelectedItem.Text == "Today")
                orderPlaced = "0";
            else if (ddlOrderPlaced.SelectedItem.Text == "Yesterday")
                orderPlaced = "-1";
            else if (ddlOrderPlaced.SelectedItem.Text == "Last 3 Days")
                orderPlaced = "-2";
            else if (ddlOrderPlaced.SelectedItem.Text == "Last 7 Days")
                orderPlaced = "-6";
            else if (ddlOrderPlaced.SelectedItem.Text == "Last 30 Days")
                orderPlaced = "-29";
        }

        Response.Redirect("OrderView.aspx?StoreId=" + storeID + "&ToDate=" + toDate + "&FromDate=" + fromDate + "&Email=" + emailAddress + "&OrderNumber=" + orderNumber + "&OrdeStatus=" + orderStatus + "&WorkGroup=" + workGroup + "&FName=" + firstName + "&LName=" + lastName + "&EmployeeCode=" + employeeCode + "&StationCode=" + stationCode + "&PaymentType=" + paymentType + "&HDB=" + hireDateBefore + "&OP=" + orderPlaced + "&BSA=" + BaseStationsAccess + "&SAP=" + SAPImportStatus);
    }

    #endregion

    #region Methods

    public void FillPaymentOption()
    {
        try
        {
            String strPayment = "WLS Payment Option";
            ddlPaymentOption.DataSource = objLookRep.GetByLookup(strPayment);
            ddlPaymentOption.DataTextField = "sLookupName";
            ddlPaymentOption.DataValueField = "iLookupID";
            ddlPaymentOption.DataBind();
            ddlPaymentOption.Items.Insert(0, new ListItem("-Select-", "0"));

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillWorkgroup()
    {
        try
        {
            List<INC_Lookup> objINC_LookupList = new List<INC_Lookup>();
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            {
                CompanyEmployee objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

                List<AdditionalWorkgroup> additionaWorkgroupList = new AdditionalWorkgroupRepository().GetManageDetailName(Convert.ToInt32(objCmpnyInfo.CompanyEmployeeID), Convert.ToInt32(IncentexGlobal.CurrentMember.CompanyId));
                List<Int64> workgroupList = new List<Int64>();
                workgroupList.Add((Int64)objCmpnyInfo.ManagementControlForWorkgroup);
                for (Int32 i = 0; i < additionaWorkgroupList.Count; i++)
                    workgroupList.Add((Int64)additionaWorkgroupList[i].WorkgroupID);

                objINC_LookupList = objLookRep.GetByLookupWorkgroupList("Workgroup", workgroupList);
            }
            else
                objINC_LookupList = objLookRep.GetByLookup("Workgroup");

            //Fill the related workgroup to dropdown
            ddlWorkgroup.DataSource = objINC_LookupList;
            ddlWorkgroup.DataTextField = "sLookupName";
            ddlWorkgroup.DataValueField = "iLookupID";
            ddlWorkgroup.DataBind();
            ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillOrderStaus()
    {
        try
        {
            List<INC_Lookup> objINCLookupList = objLookRep.GetByLookup("StatusOptionOne");
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                objINCLookupList = objINCLookupList.Where(o => o.sLookupName != "ORDER PENDING").ToList();// change condition by mayur for not display waiting for review status

            ddlOrderStatus.DataSource = objINCLookupList;
            ddlOrderStatus.DataValueField = "iLookupID";
            ddlOrderStatus.DataTextField = "sLookupName";
            ddlOrderStatus.DataBind();
            ddlOrderStatus.Items.Insert(0, new ListItem("-select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillBaseStation()
    {
        try
        {
            objList = objBaseRepos.GetAllBaseStation();
            if (objList.Count > 0)
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    // For CA this drop down will always show on top of this pages and hide for IE
                    ddlStationCodeCA.DataSource = objList.OrderBy(q => q.sBaseStation).ToList();
                    ddlStationCodeCA.DataValueField = "iBaseStationId";
                    ddlStationCodeCA.DataTextField = "sBaseStation";
                    ddlStationCodeCA.DataBind();
                    ddlStationCodeCA.Items.Insert(0, new ListItem("-select-", "0"));
                }
                else
                {
                    // For IE this drop down will always show on Previous position and hide for CA
                    ddlStationCodeIE.DataSource = objList.OrderBy(q => q.sBaseStation).ToList();
                    ddlStationCodeIE.DataValueField = "iBaseStationId";
                    ddlStationCodeIE.DataTextField = "sBaseStation";
                    ddlStationCodeIE.DataBind();
                    ddlStationCodeIE.Items.Insert(0, new ListItem("-select-", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillCompanyStore()
    {
        try
        {
            OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();
            ddlCompanyStore.DataSource = objOrderConfirmationRepository.GetCompanyStoreName().OrderBy(le => le.CompanyName);
            ddlCompanyStore.DataValueField = "StoreID";
            ddlCompanyStore.DataTextField = "CompanyName";
            ddlCompanyStore.DataBind();
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            {
                ddlCompanyStore.SelectedValue = Convert.ToString(new CompanyStoreRepository().GetByCompanyId(Convert.ToInt64(objuser.CompanyId)).StoreID);
                ddlCompanyStore.Enabled = false;
                trCompanyStore.Visible = false;
                trStationCodeCA.Visible = true;
                trStationCodeIE.Visible = false;
            }
            else
            {
                ddlCompanyStore.Items.Insert(0, new ListItem("-Select-", "0"));
                ddlCompanyStore.Enabled = true;
                trCompanyStore.Visible = true;
                trStationCodeCA.Visible = false;
                trStationCodeIE.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public void FillOrderPlaced()
    {
        try
        {
            LookupRepository objLookRep = new LookupRepository();
            ddlOrderPlaced.DataSource = objLookRep.GetByLookup("OrderPlaced");
            ddlOrderPlaced.DataTextField = "sLookupName";
            ddlOrderPlaced.DataValueField = "iLookupID";
            ddlOrderPlaced.DataBind();
            ddlOrderPlaced.Items.Insert(0, new ListItem("-Select-", "0"));

            //ddlOrderPlaced.SelectedValue = ddlOrderPlaced.Items.FindByText("Today").Value;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}