using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Report_OrderManagementReport : PageBase
{
    #region Data Members
    ReportRepository objReportRepository = new ReportRepository();
    LookupRepository objLookupRepos = new LookupRepository();
    PagedDataSource pds = new PagedDataSource();
    ReportAccessRightsRepository objReportAccessRightsRepository = new ReportAccessRightsRepository();
    String SubReport
    {
        get
        {
            if (ViewState["SubReport"] == null)
            {
                ViewState["SubReport"] = null;
            }
            return Convert.ToString(ViewState["SubReport"]);
        }
        set
        {
            ViewState["SubReport"] = value;
        }
    }
    String WorkGroupIDs
    {
        get
        {
            if (ViewState["WorkGroupIDs"] == null)
            {
                ViewState["WorkGroupIDs"] = null;
            }
            return Convert.ToString(ViewState["WorkGroupIDs"]);
        }
        set
        {
            ViewState["WorkGroupIDs"] = value;
        }
    }
    String BaseStationIDs
    {
        get
        {
            if (ViewState["BaseStationIDs"] == null)
            {
                ViewState["BaseStationIDs"] = null;
            }
            return Convert.ToString(ViewState["BaseStationIDs"]);
        }
        set
        {
            ViewState["BaseStationIDs"] = value;
        }
    }
    String PriceLevelIDs
    {
        get
        {
            if (ViewState["PriceLevelIDs"] == null)
            {
                ViewState["PriceLevelIDs"] = null;
            }
            return Convert.ToString(ViewState["PriceLevelIDs"]);
        }
        set
        {
            ViewState["PriceLevelIDs"] = value;
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
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

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Order Management Dashboard";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Report/ReportDashBoard.aspx";
            IncentexGlobal.ManageID = 9;
            FillCompanyStore();
            FillBasedStation();
            FillWorkgroup();
            FillGender();
            FillPeriod();
            ddlPeriod.Items.Insert(5, new ListItem("Current Year", System.Data.Linq.SqlClient.SqlMethods.DateDiffDay(Convert.ToDateTime("01/01/" + DateTime.Now.Year), System.DateTime.Now).ToString()));//Add new item for current year

            //this is for setting search criteria
            if (Request.QueryString["FromDate"] != null)
                txtFromDate.Text = Request.QueryString["FromDate"];
            if (Request.QueryString["ToDate"] != null)
                txtToDate.Text = Request.QueryString["ToDate"];
            if (Request.QueryString["StoreID"] != null)
                ddlCompanyStore.SelectedValue = Request.QueryString["StoreID"];
            if (string.IsNullOrEmpty(Request.QueryString["Period"]))
            {
                trFromDate.Visible = true;
                trToDate.Visible = true;
                ddlPeriod.SelectedValue = "99999";
            }
            else
            {
                ddlPeriod.SelectedValue = Request.QueryString["Period"];
                if (Convert.ToInt64(ddlPeriod.SelectedValue) < 367)
                {
                    txtFromDate.Text = DateTime.Now.AddDays(-Convert.ToInt64(ddlPeriod.SelectedValue)).ToString("MM/dd/yyyy");
                    txtToDate.Text = string.Empty;
                }
                else
                {
                    txtFromDate.Text = "01/01/" + ddlPeriod.SelectedValue;
                    txtToDate.Text = "12/31/" + ddlPeriod.SelectedValue;
                }
            }
            if (!string.IsNullOrEmpty(Request.QueryString["SubReport"]) && Request.QueryString["SubReport"] == "Employee Payroll Deduct")
            {
                if (!string.IsNullOrEmpty(Request.QueryString["WorkGroup"]))
                    ddlWorkgroup.SelectedValue = Request.QueryString["WorkGroup"];
                if (!string.IsNullOrEmpty(Request.QueryString["StationCode"]))
                    ddlBasestation.SelectedValue = Request.QueryString["StationCode"];
            }
            GenerateChart();
        }
    }
    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        if (ddlPeriod.SelectedValue == "99999")//This is for date range
        {
            trFromDate.Visible = true;
            trToDate.Visible = true;
        }
        else
        {
            if (Convert.ToInt64(ddlPeriod.SelectedValue) < 367)
                txtFromDate.Text = DateTime.Now.AddDays(-Convert.ToInt64(ddlPeriod.SelectedValue)).ToString("MM/dd/yyyy");
            else
            {
                txtFromDate.Text = "01/01/" + ddlPeriod.SelectedValue;
                txtToDate.Text = "12/31/" + ddlPeriod.SelectedValue;
            }
            trFromDate.Visible = false;
            trToDate.Visible = false;
        }
        GenerateChart();
    }
    protected void lnkSubmitRequest_Click(object sender, EventArgs e)
    {
        GenerateChart();
    }
    protected void chrtOrderAtAGlance_Click(object sender, ImageMapEventArgs e)
    {
        Response.Redirect("~/OrderManagement/OrderView.aspx?ChartSubReport=" + SubReport + "&StoreId=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedItem.Value : "") + "&ToDate=" + txtToDate.Text.Trim() + "&FromDate=" + txtFromDate.Text.Trim() + "&Email=&OrderNumber=&OrdeStatus=" + e.PostBackValue + "&WorkGroup=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : "") + "&FName=&LName=&EmployeeCode=&StationCode=" + (ddlBasestation.SelectedValue != "0" ? ddlBasestation.SelectedValue : "") + "&PaymentType=&WorkGroupAccess=" + this.WorkGroupIDs + "&BSA=" + this.BaseStationIDs);
    }
    protected void chkEDPReceived_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkEDPChecked = (CheckBox)sender;
        HiddenField hdnOrderID = (HiddenField)chkEDPChecked.Parent.Parent.Parent.FindControl("hdnOrderID");
        if (hdnOrderID != null)
        {
            OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();
            Order objOrder = objOrderConfirmationRepository.GetByOrderID(Convert.ToInt64(hdnOrderID.Value));
            objOrder.IsEDPReceived = chkEDPChecked.Checked ? true : false;
            objOrderConfirmationRepository.SubmitChanges();
        }
        GenerateChart();
    }
    protected void gvEmployeePayrollDeduct_RowCommand(object sender, GridViewCommandEventArgs e)
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
            GenerateChart();
        }
        if (e.CommandName.Equals("OrderDetail"))
        {
            IncentexGlobal.OrderReturnURL = Request.Url.ToString() + "&WorkGroup=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : "") + "&StationCode=" + (ddlBasestation.SelectedValue != "0" ? ddlBasestation.SelectedValue : "");
            Response.Redirect("~/OrderManagement/OrderDetail.aspx?Id=" + e.CommandArgument);
        }
    }
    protected void gvEmployeePayrollDeduct_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkEDPReceived = (CheckBox)e.Row.FindControl("chkEDPReceived");
            HtmlControl spnEDPReceived = (HtmlControl)e.Row.FindControl("spnEDPReceived");
            HiddenField hdnEDPReceived = (HiddenField)e.Row.FindControl("hdnEDPReceived");
            if (hdnEDPReceived != null && !string.IsNullOrEmpty(hdnEDPReceived.Value))
            {
                if (Convert.ToBoolean(hdnEDPReceived.Value) == true)
                {
                    chkEDPReceived.Checked = true;
                    spnEDPReceived.Attributes.Add("class", "custom-checkbox_checked");
                }
                else
                {
                    chkEDPReceived.Checked = false;
                    spnEDPReceived.Attributes.Add("class", "custom-checkbox");
                }
            }
        }
    }
    protected void chkReference_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkReference = (CheckBox)sender;
        HiddenField hdnOrderID = (HiddenField)chkReference.Parent.Parent.Parent.Parent.Parent.Parent.FindControl("hdnOrderID");
        HiddenField hdnMyShoppingCartID = (HiddenField)chkReference.Parent.Parent.FindControl("hdnMyShoppingCartID");
        if (hdnOrderID != null)
        {
            OrderDropShipReportRepository objOrderDropShipReportRepository = new OrderDropShipReportRepository();
            //If false then delete the record and if true the add new record
            if (chkReference.Checked)
            {
                OrderDropShipReport objOrderDropShipReport = new OrderDropShipReport();
                objOrderDropShipReport.OrderID = Convert.ToInt64(hdnOrderID.Value);
                objOrderDropShipReport.MyShoppingCartID = Convert.ToInt64(hdnMyShoppingCartID.Value);
                objOrderDropShipReport.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                objOrderDropShipReport.Status = true;
                objOrderDropShipReport.UpdatedDate = DateTime.Now;
                objOrderDropShipReportRepository.Insert(objOrderDropShipReport);
                objOrderDropShipReportRepository.SubmitChanges();
            }
            else
            {
                List<OrderDropShipReport> objOrderDropShipReport = objOrderDropShipReportRepository.GetByOrderIDAndMyShoppingCartID(Convert.ToInt64(hdnOrderID.Value), Convert.ToInt64(hdnMyShoppingCartID.Value));
                objOrderDropShipReportRepository.DeleteAll(objOrderDropShipReport);
                objOrderDropShipReportRepository.SubmitChanges();
            }
        }
        GenerateChart();
    }
    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        //lnkPrint.Attributes.Add("onclick", "PrintGridView();");

        DateTime? FromDate = null;
        DateTime? ToDate = null;
        Int64? StoreID = null;
        Int64? WorkgroupID = null;
        Int64? GenderID = null;
        Int64? BaseStationID = null;
        Int64? UserInfoID = null;

        if (txtFromDate.Text != "")
            FromDate = Convert.ToDateTime(txtFromDate.Text.Trim());
        if (txtToDate.Text != "")
            ToDate = Convert.ToDateTime(txtToDate.Text.Trim());
        if (ddlCompanyStore.SelectedIndex > 0)
            StoreID = Convert.ToInt64(ddlCompanyStore.SelectedValue);
        if (ddlWorkgroup.SelectedValue != "0")
            WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
        if (ddlBasestation.SelectedValue != "0")
            BaseStationID = Convert.ToInt64(ddlBasestation.SelectedValue);
        if (ddlGender.SelectedValue != "0")
            GenderID = Convert.ToInt64(ddlGender.SelectedValue);
        if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;

        List<SelectOrderDropShipSupplierReportResult> objResult = objReportRepository.GetOrderDropShipSupplierReport(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
        StringBuilder objHtml = new StringBuilder();
        if (objResult != null && objResult.Count > 0)
        {
            //Getting Order Detail only 
            var orderResult = (from result in objResult
                               select new
                               {
                                   OrderID = result.OrderID,
                                   OrderNumber = result.OrderNumber,
                                   CompanyName = result.CompanyName,
                                   OrderDate = result.OrderDate,
                                   Name = result.Name,
                                   BBillingCo = result.BBillingCo,
                                   BManager = result.BManager,
                                   BAddress = result.BAddress,
                                   BCityID = result.BCityID,
                                   BCompanyName = result.BCompanyName,
                                   BCountryID = result.BCountryID,
                                   BEmail = result.BEmail,
                                   BFax = result.BFax,
                                   BMobile = result.BMobile,
                                   BName = result.BName,
                                   BStateID = result.BStateID,
                                   BTelephone = result.BTelephone,
                                   BZipCode = result.BZipCode,
                                   SBillingCo = result.SBillingCo,
                                   SManager = result.SManager,
                                   SAddress = result.SAddress,
                                   SCityID = result.SCityID,
                                   SCompanyName = result.SCompanyName,
                                   SCountryID = result.SCountryID,
                                   SEmail = result.SEmail,
                                   SFax = result.SFax,
                                   SMobile = result.SMobile,
                                   SName = result.SName,
                                   SStateID = result.SStateID,
                                   STelephone = result.STelephone,
                                   SZipCode = result.SZipCode,
                               }).Distinct().ToList();


            objHtml.Append("<html><head>");
            objHtml.Append("<title>Print Preview</title>");
            objHtml.Append("</head><body>");
            objHtml.Append("<div style='width:920px;margin:0 auto;'><h4>Summary Drop-Ship Report</h4>");

            foreach (var Item in orderResult)
            {
                //This is for generate general detail
                objHtml.Append("<table><tr>");
                objHtml.Append("<td width='50%'>");
                objHtml.Append("<table><tr><td>Company: " + Item.CompanyName + "</td></tr><tr><td>Order Number: " + Item.OrderNumber + "</td></tr></table>");
                objHtml.Append("</td>");
                objHtml.Append("<td width='50%'>");
                objHtml.Append("<table><tr><td>Ordered By: " + Item.Name + "</td></tr><tr><td>Order Date: " + Item.OrderDate.Value.ToShortDateString() + "</td></tr></table>");
                objHtml.Append("</td>");
                objHtml.Append("</tr></table>");

                //This is for generate Billing And Shipping detail
                objHtml.Append("<table><tr>");
                objHtml.Append("<td width='50%'><strong>Bill To:</strong>");
                objHtml.Append("<table><tr><td>" + Item.BBillingCo + " " + Item.BManager + "</td></tr><tr><td>" + Item.BCompanyName + "</td></tr><tr><td>" + Item.BAddress + "</td></tr><tr><td>" + new CityRepository().GetById((long)Item.BCityID).sCityName + "," + new StateRepository().GetById((long)Item.BStateID).sStatename + " " + Item.BZipCode + "</td></tr><tr><td>" + new CountryRepository().GetById((long)Item.BCountryID).sCountryName + "</td></tr><tr><td>" + Item.BEmail + "</td></tr><tr><td>" + Item.BTelephone + "</td></tr></table>");
                objHtml.Append("</td>");
                objHtml.Append("<td width='50%'><strong>Ship To:</strong>");
                objHtml.Append("<table><tr><td>" + Item.SName + " " + Item.SFax + "</td></tr><tr><td>" + Item.SCompanyName + "</td></tr><tr><td>" + Item.SAddress + "</td></tr><tr><td>" + new CityRepository().GetById((long)Item.SCityID).sCityName + "," + new StateRepository().GetById((long)Item.SStateID).sStatename + " " + Item.SZipCode + "</td></tr><tr><td>" + new CountryRepository().GetById((long)Item.SCountryID).sCountryName + "</td></tr><tr><td>" + Item.SEmail + "</td></tr><tr><td>" + Item.STelephone + "</td></tr></table>");
                objHtml.Append("</td>");
                objHtml.Append("</tr></table>");

                //Get item detail based on order id
                var orderItemResult = (from result in objResult
                                       where result.OrderID == Item.OrderID && result.MyShoppingCartID != null
                                       select new
                                       {
                                           ItemNumber = result.ItemNumber,
                                           Quantity = result.Quantity,
                                           Description = result.ProductDescrption,
                                           UnitPrice = result.UnitPrice,
                                           Status = result.Status != null ? result.Status : false,
                                           MyShoppingCartID = result.MyShoppingCartID
                                       }).ToList();

                objHtml.Append("<table><tr><td class='alignleft'><strong>Drop-Ship Order Items:</strong></td></tr><tr><td><table cellspacing='0' border='0'>");
                objHtml.Append("<tr><th>Item #</th><th>QTY</th><th>Description</th><th>Retail</th><th>Status</th></tr>");

                foreach (var childitem in orderItemResult)
                {
                    objHtml.Append("<tr><td>" + childitem.ItemNumber + "</td><td style='width:10%;' align='center'>" + childitem.Quantity + "</td><td style='width:55%;'>" + childitem.Description + "</td><td align='center' style='width:10%;'>" + childitem.UnitPrice + "</td><td align='center' style='width:5%;'>" + childitem.Status + "</td></tr>");
                }
                objHtml.Append("</table></td></tr></table>");
                objHtml.Append("<hr/>");
            }
            objHtml.Append("</div></body>");
            objHtml.Append("</html>");
        }

        System.Web.UI.Page pg = new System.Web.UI.Page();
        pg.EnableEventValidation = false;
        HtmlForm frm = new HtmlForm();
        pg.Controls.Add(frm);
        frm.Attributes.Add("runat", "server");
        HtmlGenericControl dv = new HtmlGenericControl();
        dv.InnerHtml = objHtml.ToString();
        frm.Controls.Add(dv);
        pg.DesignerInitialize();
        System.IO.StringWriter stringWrite = new System.IO.StringWriter(objHtml);
        System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
        pg.RenderControl(htmlWrite);

        string strHTML = stringWrite.ToString();
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Write(strHTML);
        HttpContext.Current.Response.Write("<script>window.print();</script>");
        HttpContext.Current.Response.End();

        //ClientScript.RegisterStartupScript(this.GetType(), "PrentDiv", "PrintPage('" + objHtml.ToString() + "');", true);

        //string printScript = "function PrintGridView() { var printWindow = window.open('print.htm','PrintWindow','letf=0,top=0,width=150,height=300,toolbar=1,scrollbars=1,status=1');printWindow.document.write(" + objHtml.ToString() + ");printWindow.document.close();printWindow.focus();printWindow.print();printWindow.close();}";
        //ClientScript.RegisterClientScriptBlock(this.GetType(), "PrintGridView", "<script type=text/javascript>function PrintGridView() { var printWindow = window.open('print.htm','PrintWindow','letf=0,top=0,width=150,height=300,toolbar=1,scrollbars=1,status=1');printWindow.document.write(" + objHtml.ToString() + ");printWindow.document.close();printWindow.focus();printWindow.print();printWindow.close();}</script>");
        //ClientScript.RegisterClientScriptBlock(this.GetType(), "PrintGridView", printScript, true);
    }
    #endregion

    #region Methods
    private void FillCompanyStore()
    {
        try
        {
            List<SelectCompanyStoreNameResult> objCompanyList = new List<SelectCompanyStoreNameResult>();
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
                objCompanyList = new OrderConfirmationRepository().GetCompanyStoreName();
            else
                objCompanyList = objReportAccessRightsRepository.GetStoreByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 2221);

            if (objCompanyList != null)
            {
                ddlCompanyStore.DataSource = objCompanyList.OrderBy(x => x.CompanyName);
                ddlCompanyStore.DataValueField = "StoreID";
                ddlCompanyStore.DataTextField = "CompanyName";
                ddlCompanyStore.DataBind();
            }
            ddlCompanyStore.Items.Insert(0, new ListItem("-Select Store-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillBasedStation()
    {
        List<INC_BasedStation> basestationList = new List<INC_BasedStation>();
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            basestationList = new BaseStationRepository().GetAllBaseStation().ToList();
        else
            basestationList = objReportAccessRightsRepository.GetBaseStationByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 2221);

        if (basestationList != null)
        {
            ddlBasestation.DataSource = basestationList.OrderBy(x => x.sBaseStation);
            ddlBasestation.DataValueField = "iBaseStationId";
            ddlBasestation.DataTextField = "sBaseStation";
            ddlBasestation.DataBind();
        }
        ddlBasestation.Items.Insert(0, new ListItem("-Select Basestation-", "0"));
    }

    private void FillWorkgroup()
    {
        List<INC_Lookup> WorkgroupList = new List<INC_Lookup>();
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            WorkgroupList = objLookupRepos.GetByLookup("Workgroup").ToList();
        else
            WorkgroupList = objReportAccessRightsRepository.GetWorkgroupByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 2221);

        if (WorkgroupList != null)
        {
            ddlWorkgroup.DataSource = WorkgroupList.OrderBy(x => x.sLookupName);
            ddlWorkgroup.DataValueField = "iLookupID";
            ddlWorkgroup.DataTextField = "sLookupName";
            ddlWorkgroup.DataBind();
        }
        ddlWorkgroup.Items.Insert(0, new ListItem("-Select Workgroup-", "0"));
    }

    private void FillGender()
    {
        ddlGender.DataSource = objLookupRepos.GetByLookup("Gender").OrderBy(x => x.sLookupName);
        ddlGender.DataValueField = "iLookupID";
        ddlGender.DataTextField = "sLookupName";
        ddlGender.DataBind();
        ddlGender.Items.Insert(0, new ListItem("-Select Gender-", "0"));
    }
    private void FillPeriod()
    {
        ddlPeriod.ClearSelection();
        ddlPeriod.Items.Clear();
        ddlPeriod.DataSource = Common.BindPeriodDropDownItems();
        ddlPeriod.DataValueField = "Value";
        ddlPeriod.DataTextField = "Text";
        ddlPeriod.DataBind();
    }
    protected void GenerateChart()
    {
        DateTime? FromDate = null;
        DateTime? ToDate = null;
        Int64? StoreID = null;
        Int64? WorkgroupID = null;
        Int64? GenderID = null;
        Int64? BaseStationID = null;
        Int64? UserInfoID = null;

        if (txtFromDate.Text != "")
            FromDate = Convert.ToDateTime(txtFromDate.Text.Trim());
        if (txtToDate.Text != "")
            ToDate = Convert.ToDateTime(txtToDate.Text.Trim());
        if (ddlCompanyStore.SelectedIndex > 0)
            StoreID = Convert.ToInt64(ddlCompanyStore.SelectedValue);
        if (ddlWorkgroup.SelectedValue != "0")
            WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
        if (ddlBasestation.SelectedValue != "0")
            BaseStationID = Convert.ToInt64(ddlBasestation.SelectedValue);
        if (ddlGender.SelectedValue != "0")
            GenderID = Convert.ToInt64(ddlGender.SelectedValue);
        if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;

        if (Request.QueryString["SubReport"] != null)
        {
            SubReport = Request.QueryString["SubReport"].ToString();
        }
        if (SubReport != null)
        {
            if (SubReport == "Order Status Snapshot")
            {
                List<ReportRepository.GetOrderAtAGlanceWiseResult> objResult = objReportRepository.GetOrderAtAGlanceWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    objResult = objResult.Where(x => x.Status != "Canceled" && x.Status != "Deleted").ToList();
                    chrtOrderAtAGlance.DataSource = objResult;
                    chrtOrderAtAGlance.DataBind();
                    lblDisplayText.Text = "Total Order : " + objResult.Sum(x => x.Count);
                }
                dvOrderAtAGlance.Visible = true;
            }
            else if (SubReport == "Employee Payroll Deduct")
            {
                List<ReportRepository.GetEmployeePayrollDeductWiseResult> objResult = objReportRepository.GetEmployeePayrollDeductWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);

                if (objResult != null && objResult.Count > 0)
                {
                    DataView myDataView = new DataView();
                    DataTable dataTable = ListToDataTable(objResult);
                    myDataView = dataTable.DefaultView;
                    if (this.ViewState["SortExp"] != null)
                    {
                        myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
                    }
                    pds.DataSource = myDataView;
                    pds.AllowPaging = true;
                    pds.PageSize = 1000; //As per Ken told Convert.ToInt32(Application["GRIDPAGING"]);
                    pds.CurrentPageIndex = CurrentPage;
                    lnkbtnNext.Enabled = !pds.IsLastPage;
                    lnkbtnPrevious.Enabled = !pds.IsFirstPage;
                    gvEmployeePayrollDeduct.DataSource = pds;
                    gvEmployeePayrollDeduct.DataBind();

                    pagingtable.Visible = true;
                    lblRecordCounter.Text = objResult.Count.ToString();
                    doPaging();
                }
                else
                {
                    pagingtable.Visible = false;
                }
                dvEmployeePayrollDeduct.Visible = true;
            }
            else if (SubReport == "Summary Drop-Ship Report")
            {
                List<SelectOrderDropShipSupplierReportResult> objResult = objReportRepository.GetOrderDropShipSupplierReport(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);

                if (objResult != null && objResult.Count > 0)
                {
                    //Getting Order Detail only 
                    var orderResult = (from result in objResult
                                       select new
                                       {
                                           OrderID = result.OrderID,
                                           OrderNumber = result.OrderNumber,
                                           CompanyName = result.CompanyName,
                                           OrderDate = result.OrderDate,
                                           Name = result.Name
                                       }).Distinct().ToList();

                    rptDropShipSummary.DataSource = orderResult;
                    rptDropShipSummary.DataBind();

                    lblDropShipRecordCounter.Text = orderResult.Count.ToString();

                    foreach (RepeaterItem repeaterItem in rptDropShipSummary.Items)
                    {
                        HiddenField hdnOrderID = (HiddenField)repeaterItem.FindControl("hdnOrderID");

                        //Get item detail based on order id
                        var orderItemResult = (from result in objResult
                                               where result.OrderID == Convert.ToInt64(hdnOrderID.Value) && result.MyShoppingCartID != null
                                               select new
                                               {
                                                   ItemNumber = result.ItemNumber,
                                                   Quantity = result.Quantity,
                                                   Description = result.ProductDescrption,
                                                   UnitPrice = result.UnitPrice,
                                                   Status = result.Status != null ? result.Status : false,
                                                   MyShoppingCartID = result.MyShoppingCartID
                                               }).ToList();

                        GridView grdItemDetail = ((GridView)(repeaterItem.FindControl("grdItemDetail")));
                        grdItemDetail.DataSource = orderItemResult;
                        grdItemDetail.DataBind();

                        foreach (GridViewRow item in grdItemDetail.Rows)
                        {
                            CheckBox chkReference = (CheckBox)item.FindControl("chkReference");
                            HtmlControl spnReference = (HtmlControl)item.FindControl("spnReference");
                            HiddenField hdnReference = (HiddenField)item.FindControl("hdnReference");
                            if (hdnReference != null && !string.IsNullOrEmpty(hdnReference.Value))
                            {
                                if (Convert.ToBoolean(hdnReference.Value) == true)
                                {
                                    chkReference.Checked = true;
                                    spnReference.Attributes.Add("class", "custom-checkbox_checked");
                                }
                                else
                                {
                                    chkReference.Checked = false;
                                    spnReference.Attributes.Add("class", "custom-checkbox");
                                }
                            }
                        }

                        //Get reference detail based on order id
                        var orderReferenceResult = (from result in objResult
                                                    where result.OrderID == Convert.ToInt64(hdnOrderID.Value) && result.DropShipReportID != null
                                                    select new
                                                    {
                                                        IEName = result.IEName,
                                                        UpdatedDate = result.UpdatedDate
                                                    }).ToList();

                        GridView grdReferenceDetail = ((GridView)(repeaterItem.FindControl("grdReferenceDetail")));
                        grdReferenceDetail.DataSource = orderReferenceResult;
                        grdReferenceDetail.DataBind();
                    }
                }
                dvSummaryDropShipReport.Visible = true;
            }
            else if (SubReport == "Back Order Report")
            {
                List<SelectBackOrderReportResult> objResult = objReportRepository.GetBackOrderReport(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);

                if (objResult != null && objResult.Count > 0)
                {
                    //Getting Order Detail only 
                    var orderResult = (from result in objResult
                                       select new
                                       {
                                           OrderID = result.OrderID,
                                           OrderNumber = result.OrderNumber,
                                           CompanyName = result.CompanyName,
                                           OrderDate = result.OrderDate,
                                           Name = result.Name
                                       }).Distinct().ToList();

                    rpBackOrderReport.DataSource = orderResult;
                    rpBackOrderReport.DataBind();

                    lblBackOrderCount.Text = orderResult.Count.ToString();

                    foreach (RepeaterItem repeaterItem in rpBackOrderReport.Items)
                    {
                        HiddenField hdnOrderID = (HiddenField)repeaterItem.FindControl("hdnOrderID");

                        //Get item detail based on order id
                        var orderItemResult = (from result in objResult
                                               where result.OrderID == Convert.ToInt64(hdnOrderID.Value) && result.MyShoppingCartID != null
                                               select new
                                               {
                                                   ItemNumber = result.ItemNumber,
                                                   QuantityOrdered = result.QuantityOrdered,
                                                   Description = result.ProductDescrption,
                                                   UnitPrice = result.UnitPrice,
                                                   Status = result.Status != null ? result.Status : false,
                                                   MyShoppingCartID = result.MyShoppingCartID,
                                                   BackOrderedUntil = result.BackOrderedUntil,
                                                   IsShippedComplete = (((result.QuantityOrdered - (result.QuantityShipped != null ? result.QuantityShipped : 0)) > 0) ? false : true),
                                                   Inventory = result.Inventory,
                                                   QuantityShipped = result.QuantityShipped,
                                                   BackOrderQuantity = (result.QuantityOrdered - (result.QuantityShipped != null ? result.QuantityShipped : 0))
                                               }).OrderBy(x => x.QuantityShipped >= x.QuantityOrdered).ThenBy(x => x.QuantityShipped != 0).ThenBy(x => x.Inventory).ToList();
                        var backorderresult = orderItemResult.Where(o => o.Inventory != null && o.Inventory <= 0 && !o.IsShippedComplete).ToList();
                        GridView grdItemDetail = ((GridView)(repeaterItem.FindControl("grdItemDetail")));
                        grdItemDetail.DataSource = backorderresult;
                        grdItemDetail.DataBind();
                       
                    }
                }
                dvBackOrder.Visible = true;
            }
        }
    }

    #region Pagging Methods
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

            DataList2.DataSource = dt;
            DataList2.DataBind();

        }
        catch (Exception)
        { }

    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        GenerateChart();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        GenerateChart();
    }
    protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            GenerateChart();
        }
    }

    protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
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

    #endregion
}
