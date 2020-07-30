using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class OrderManagement_OrderView : PageBase
{
    #region Data Members

    Decimal TotalDollarValue = 0M;
    Int64 TotalRecords = 0;
    PagedDataSource pds = new PagedDataSource();
    CompanyRepository objCompRepos = new CompanyRepository();
    CompanyEmployeeRepository objCompEmpRepos = new CompanyEmployeeRepository();
    OrderConfirmationRepository objOrdRepos = new OrderConfirmationRepository();

    Int64 SupplierId
    {
        get
        {
            return Convert.ToInt64(ViewState["SupplierId"]);
        }
        set
        {
            ViewState["SupplierId"] = value;
        }
    }

    String StoreId
    {
        get
        {
            return Convert.ToString(ViewState["StoreId"]);
        }
        set
        {
            ViewState["StoreId"] = value;
        }
    }

    String OrderNumber
    {
        get
        {
            return Convert.ToString(ViewState["OrderNumber"]);
        }
        set
        {
            ViewState["OrderNumber"] = value;
        }
    }

    String OrderStatus
    {
        get
        {
            return Convert.ToString(ViewState["OrderStatus"]);
        }
        set
        {
            ViewState["OrderStatus"] = value;
        }
    }

    String FirstName
    {
        get
        {
            return Convert.ToString(ViewState["FirstName"]);
        }
        set
        {
            ViewState["FirstName"] = value;
        }
    }

    String LastName
    {
        get
        {
            return Convert.ToString(ViewState["LastName"]);
        }
        set
        {
            ViewState["LastName"] = value;
        }
    }

    String StationCode
    {
        get
        {
            return Convert.ToString(ViewState["StationCode"]);
        }
        set
        {
            ViewState["StationCode"] = value;
        }
    }

    String EmployeeCode
    {
        get
        {
            return Convert.ToString(ViewState["EmployeeCode"]);
        }
        set
        {
            ViewState["EmployeeCode"] = value;
        }
    }

    String WorkGroupID
    {
        get
        {
            return Convert.ToString(ViewState["WorkGroupID"]);
        }
        set
        {
            ViewState["WorkGroupID"] = value;
        }
    }

    String EmailAddress
    {
        get
        {
            return Convert.ToString(ViewState["EmailAddress"]);
        }
        set
        {
            ViewState["EmailAddress"] = value;
        }
    }

    String PaymentType
    {
        get
        {
            return Convert.ToString(ViewState["PaymentType"]);
        }
        set
        {
            ViewState["PaymentType"] = value;
        }
    }

    Boolean? SAPImportStatus
    {
        get
        {
            if (ViewState["SAPImportStatus"] == null)
                return null;
            else
                return Convert.ToBoolean(ViewState["SAPImportStatus"]);
        }
        set
        {
            ViewState["SAPImportStatus"] = value;
        }
    }

    String OrderPlaced
    {
        get
        {
            return Convert.ToString(ViewState["OrderPlaced"]);
        }
        set
        {
            ViewState["OrderPlaced"] = value;
        }
    }

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

    /// <summary>
    ///  Current User WorkGroup Access
    /// </summary>
    String WorkGroupAccess
    {
        get
        {
            return Convert.ToString(ViewState["WorkGroupAccess"]);
        }
        set
        {
            ViewState["WorkGroupAccess"] = value;
        }
    }

    String DaysRange
    {
        get
        {
            return Convert.ToString(ViewState["DaysRange"]);
        }
        set
        {
            ViewState["DaysRange"] = value;
        }
    }

    Boolean CanDeleteOrders
    {
        get
        {
            return Convert.ToBoolean(ViewState["CanDeleteOrders"]);
        }
        set
        {
            ViewState["CanDeleteOrders"] = value;
        }
    }

    DateTime? ToDate
    {
        get
        {
            if (ViewState["ToDate"] == null)
                return null;
            else
                return Convert.ToDateTime(ViewState["ToDate"]);
        }
        set
        {
            ViewState["ToDate"] = value;
        }
    }

    DateTime? FromDate
    {
        get
        {
            if (ViewState["FromDate"] == null)
                return null;
            else
                return Convert.ToDateTime(ViewState["FromDate"]);
        }
        set
        {
            ViewState["FromDate"] = value;
        }
    }

    public DateTime? HireDateBefore = null;

    List<SelectCompanyOrderDetailsViewResult> objSupplierORderList = new List<SelectCompanyOrderDetailsViewResult>();
    List<SelectCompanyOrderDetailsResult> ObjCompanyOrderList = new List<SelectCompanyOrderDetailsResult>();
    List<SelectOrderBySearchCriteriaForSupplierResult> lstSupplierOrders = new List<SelectOrderBySearchCriteriaForSupplierResult>();

    #endregion

    #region Events

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

            ((Label)Master.FindControl("lblPageHeading")).Text = "Order Summary View";

            if (Request.QueryString["ChartSubReport"] != null)//This is to navigate to chart page when comming from chart
            {
                String ReturnUrl = String.Empty;

                if (Request.QueryString["ChartSubReport"] == "Order Status Snapshot")
                    ReturnUrl = "~/admin/Report/OrderManagementReport.aspx";
                else if (Request.QueryString["ChartSubReport"] == "Ship Complete Report")
                    ReturnUrl = "~/admin/Report/ServiceLevelScorecardReport.aspx";
                else
                {
                    if (Request.QueryString["page"] != null)
                        ReturnUrl = "~/MyAccount/report/SalesSummaryReport.aspx";
                    else
                        ReturnUrl = "~/admin/Report/SalesSummaryReport.aspx";
                }

                //Now combine all queryString with returnurl
                for (Int32 i = 0; i < Request.QueryString.Count; i++)
                {
                    if (!String.IsNullOrEmpty(Request.QueryString[i]))
                    {
                        if (i == 0)
                            ReturnUrl += "?SubReport";
                        else
                            ReturnUrl += "&" + Request.QueryString.AllKeys[i];

                        ReturnUrl += "=" + Convert.ToString(Request.QueryString[i]);
                    }
                }

                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = ReturnUrl;
            }
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/OrderManagement/OrderLookupSearch.aspx";

            if (!String.IsNullOrEmpty(Request.QueryString["ex"]) && Request.QueryString["ex"] == "1")
            {
                if (Session["OrdSerParams"] != null)
                {
                    String[] ordSerParams = Convert.ToString(Session["OrdSerParams"]).Split(',');
                    hdnScrollY.Value = ordSerParams[0];
                    this.CurrentPage = Convert.ToInt32(ordSerParams[1]);
                    this.ViewState["SortExp"] = !String.IsNullOrEmpty(ordSerParams[2]) ? ordSerParams[2] : null;
                    this.ViewState["SortOrder"] = !String.IsNullOrEmpty(ordSerParams[3]) ? ordSerParams[3] : null;
                    Session.Remove("OrdSerParams");
                }
            }

            if (!String.IsNullOrEmpty(Request.QueryString["StoreId"]))
            {
                this.StoreId = Convert.ToString(Request.QueryString["StoreId"]);
                List<SelectCompanyNameCompanyIDResult> objlist = new CompanyStoreRepository().GetBYStoreId(Convert.ToInt32(StoreId));
                lblCompanyName.Text = Convert.ToString(objlist[0].CompanyName);
            }

            if (!String.IsNullOrEmpty(Request.QueryString["ToDate"]))
                this.ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);

            if (!String.IsNullOrEmpty(Request.QueryString["FromDate"]))
                this.FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);

            if (!String.IsNullOrEmpty(Request.QueryString["Email"]))
                this.EmailAddress = Convert.ToString(Request.QueryString["Email"]);

            if (!String.IsNullOrEmpty(Request.QueryString["OrderNumber"]))
                this.OrderNumber = Convert.ToString(Request.QueryString["OrderNumber"]);

            if (!String.IsNullOrEmpty(Request.QueryString["OrdeStatus"]))
                this.OrderStatus = Convert.ToString(Request.QueryString["OrdeStatus"]);

            if (!String.IsNullOrEmpty(Request.QueryString["WorkGroup"]))
                this.WorkGroupID = Convert.ToString(Request.QueryString["WorkGroup"]);

            if (!String.IsNullOrEmpty(Request.QueryString["FName"]))
                this.FirstName = Convert.ToString(Request.QueryString["FName"]);

            if (!String.IsNullOrEmpty(Request.QueryString["LName"]))
                this.LastName = Convert.ToString(Request.QueryString["LName"]);

            if (!String.IsNullOrEmpty(Request.QueryString["EmployeeCode"]))
                this.EmployeeCode = Convert.ToString(Request.QueryString["EmployeeCode"]);

            if (!String.IsNullOrEmpty(Request.QueryString["SAP"]))
                this.SAPImportStatus = Convert.ToBoolean(Request.QueryString["SAP"]);

            if (!String.IsNullOrEmpty(Request.QueryString["StationCode"]))
                this.StationCode = Convert.ToString(Request.QueryString["StationCode"]);

            if (!String.IsNullOrEmpty(Request.QueryString["PaymentType"]))
                this.PaymentType = Convert.ToString(Request.QueryString["PaymentType"]);
            else
                this.PaymentType = null;

            if (!String.IsNullOrEmpty(Request.QueryString["HDB"]))
                this.HireDateBefore = Convert.ToDateTime(Request.QueryString["HDB"]);

            if (!String.IsNullOrEmpty(Request.QueryString["OP"]))
                this.OrderPlaced = Convert.ToString(Request.QueryString["OP"]);

            if (!String.IsNullOrEmpty(Request.QueryString["BSA"]))
                this.BaseStationsAccess = Convert.ToString(Request.QueryString["BSA"]);

            if (!String.IsNullOrEmpty(Request.QueryString["DaysRange"]))
                this.DaysRange = Convert.ToString(Request.QueryString["DaysRange"]);

            if (!String.IsNullOrEmpty(Request.QueryString["WorkGroupAccess"]))
                this.WorkGroupAccess = Convert.ToString(Request.QueryString["WorkGroupAccess"]);

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
            {
                BindGridViewForSupplier();
                gvOrderDetail.Columns[1].Visible = false;
            }
            else
            {
                BindGridView();

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                {
                    if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                        this.CanDeleteOrders = new PreferenceRepository().GetUserPreferenceValue(IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.Usertype, "AllowToDeleteOrders") == "Yes" ? true : false;

                    if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || this.CanDeleteOrders)
                        gvOrderDetail.Columns[8].Visible = true;

                    gvOrderDetail.Columns[1].Visible = true;
                    gvOrderDetail.Columns[6].Visible = true;
                }
                else
                {
                    gvOrderDetail.Columns[1].Visible = false;
                    gvOrderDetail.Columns[6].Visible = false;
                }
            }

            if (!String.IsNullOrEmpty(this.StoreId))
            {
                gvOrderDetail.Columns[2].Visible = false;
                tdCompany.Visible = true;
            }
            else
            {
                gvOrderDetail.Columns[2].Visible = true;
                tdCompany.Visible = false;
            }
        }
    }

    protected void gvOrderDetail_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //((HyperLink)e.Row.FindControl("hypOrderNumber")).NavigateUrl = "~/OrderManagement/OrderDetail.aspx?Id=" + ((HiddenField)e.Row.FindControl("hdnOrderNumber")).Value.ToString();

                //Add by mayur on 6-april-2012 to display order time for supplier
                Label lblOrderDate = (Label)e.Row.FindControl("lblOrderDate");
                TimeZoneInfo newYorkTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                DateTime DisplayTime = TimeZoneInfo.ConvertTime(Convert.ToDateTime(lblOrderDate.Text), newYorkTimeZone);
                if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                    lblOrderDate.Text = DisplayTime.ToString("MM/dd/yyyy HH:mm:ss");
                else
                    lblOrderDate.Text = DisplayTime.ToString("MM/dd/yyyy");
                //end mayur

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                {
                    HiddenField hdnSentToSAP = (HiddenField)e.Row.FindControl("hdnSentToSAP");
                    HiddenField hdnUpdatedBySAPDate = (HiddenField)e.Row.FindControl("hdnUpdatedBySAPDate");
                    HiddenField hdnOrderStatus = (HiddenField)e.Row.FindControl("hdnOrderStatus");
                    HiddenField hdnCanReTransmitToSAP = (HiddenField)e.Row.FindControl("hdnCanReTransmitToSAP");
                    ImageButton imgSAPStatus = (ImageButton)e.Row.FindControl("imgSAPStatus");

                    imgSAPStatus.Style.Add("cursor", "default");
                    imgSAPStatus.Enabled = false;

                    if (!String.IsNullOrEmpty(hdnUpdatedBySAPDate.Value))
                    {
                        imgSAPStatus.ImageUrl = "../Images/sap_purple.png";
                        imgSAPStatus.ToolTip = "Updated by SAP";
                    }
                    else if (Convert.ToBoolean(hdnSentToSAP.Value))
                    {
                        imgSAPStatus.ImageUrl = "../Images/sap_green.png";
                        imgSAPStatus.ToolTip = "Sent to SAP";
                    }
                    else if (hdnOrderStatus.Value.ToUpper() == "ORDER PENDING")
                    {
                        imgSAPStatus.ImageUrl = "../Images/sap_orange.png";
                        imgSAPStatus.ToolTip = "Order Pending";
                    }
                    else if (Convert.ToBoolean(hdnCanReTransmitToSAP.Value))
                    {
                        imgSAPStatus.ImageUrl = "../Images/sap_yellow.png";
                        imgSAPStatus.ToolTip = "Waiting For Retransmission";
                    }
                    else
                    {
                        imgSAPStatus.ImageUrl = "../Images/sap_red.png";
                        imgSAPStatus.ToolTip = "Failed to reach SAP";
                        imgSAPStatus.Enabled = true;
                        imgSAPStatus.Style.Add("cursor", "pointer");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvOrderDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Sort")
            {
                if (this.ViewState["SortExp"] == null)
                {
                    this.ViewState["SortExp"] = Convert.ToString(e.CommandArgument);
                    this.ViewState["SortOrder"] = "ASC";
                }
                else
                {
                    if (Convert.ToString(this.ViewState["SortExp"]) == Convert.ToString(e.CommandArgument))
                    {
                        if (Convert.ToString(this.ViewState["SortOrder"]) == "ASC")
                            this.ViewState["SortOrder"] = "DESC";
                        else
                            this.ViewState["SortOrder"] = "ASC";
                    }
                    else
                    {
                        this.ViewState["SortOrder"] = "ASC";
                        this.ViewState["SortExp"] = Convert.ToString(e.CommandArgument);
                    }
                }

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                    BindGridViewForSupplier();
                else
                    BindGridView();
            }
            else if (e.CommandName == "SendEmail")
            {
                lblmsg.Text = String.Empty;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                {
                    sendEmailToSupplier(Convert.ToInt64(e.CommandArgument));
                    lblmsg.Text = "Mail Sent successfully to the supplier(s) of the order";
                }
                else
                {
                    GetAndBindSupplier(Convert.ToInt64(e.CommandArgument));
                    modal.Show();
                }
            }
            else if (e.CommandName == "deleteorder")
            {
                if (!base.CanDelete && !this.CanDeleteOrders)
                {
                    base.RedirectToUnauthorised();
                }

                OrderConfirmationRepository objOrdRepos = new OrderConfirmationRepository();
                Order ordDetails = objOrdRepos.GetByOrderID(Convert.ToInt64(e.CommandArgument));

                #region Credits

                if (ordDetails.CreditUsed != null)
                {
                    CompanyEmployeeRepository objCmnyEmp = new CompanyEmployeeRepository();
                    CompanyEmployee cmpEmpl = objCmnyEmp.GetByUserInfoId((Int64)ordDetails.UserId);
                    if (ordDetails.CreditUsed == "Previous")
                    {
                        cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + ordDetails.CreditAmt;
                        cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + ordDetails.CreditAmt;
                        cmpEmpl.CreditAmtToExpired = cmpEmpl.CreditAmtToExpired + ordDetails.CreditAmt;
                        objCmnyEmp.SubmitChanges();
                    }
                    else if (ordDetails.CreditUsed == "Anniversary")
                    {
                        cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + ordDetails.CreditAmt;
                        cmpEmpl.CreditAmtToExpired = cmpEmpl.CreditAmtToExpired + ordDetails.CreditAmt;
                        objCmnyEmp.SubmitChanges();
                    }
                }

                #endregion

                #region Inventory

                List<Order> obj = objOrdRepos.GetShoppingCartId(ordDetails.OrderNumber);
                if (obj.Count > 0)
                {
                    String[] a;

                    a = obj[0].MyShoppingCartID.ToString().Split(',');

                    foreach (String u in a)
                    {
                        if (ordDetails.OrderFor == "ShoppingCart")
                        {
                            //Shopping cart
                            MyShoppingCartRepository objShoppingCartRepos = new MyShoppingCartRepository();
                            MyShoppinCart objShoppingcart = new MyShoppinCart();
                            ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                            ProductItem objProductItem = new ProductItem();
                            objShoppingcart = objShoppingCartRepos.GetById(Convert.ToInt32(u), (Int64)ordDetails.UserId);

                            objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(objShoppingcart.StoreProductID), Convert.ToInt64(objShoppingcart.MasterItemNo), objShoppingcart.ItemNumber);
                            //Update Inventory Here 
                            //Call here upDate Procedure
                            String strProcess = "Shopping";
                            String strMessage = objOrdRepos.IncreaseDescreaseInventory(objProductItem.ProductItemID, -Convert.ToInt32(objShoppingcart.Quantity), strProcess);

                        }
                        else
                        {
                            //Issuance
                            MyIssuanceCartRepository objIssuanceRepos = new MyIssuanceCartRepository();
                            MyIssuanceCart objIssuance = new MyIssuanceCart();
                            ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                            //End 

                            objIssuance = objIssuanceRepos.GetById(Convert.ToInt32(u), (Int64)ordDetails.UserId);
                            List<SelectProductIDResult> objList = objProItemRepos.GetProductId(objIssuance.MasterItemID, Convert.ToInt64(u), Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(ordDetails.StoreID));
                            //Update Inventory Here 
                            //Call here upDate Procedure
                            for (Int32 i = 0; i < objList.Count; i++)
                            {
                                String strProcess = "UniformIssuance";
                                String strMessage = objOrdRepos.IncreaseDescreaseInventory(Convert.ToInt64(objList[i].ProductItemID), -Convert.ToInt32(objIssuance.Qty), strProcess);
                            }
                        }
                    }
                }
                #endregion

                //Change Status here
                objOrdRepos.UpdateStatus(Convert.ToInt64(e.CommandArgument), "Deleted", IncentexGlobal.CurrentMember.UserInfoID, DateTime.Now);

                lblmsg.Text = "Order deleted successfully...";

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                    BindGridViewForSupplier();
                else
                    BindGridView();
            }
            else if (e.CommandName == "orderdetail")
            {
                //((HyperLink)e.Row.FindControl("hypOrderNumber")).NavigateUrl = "~/OrderManagement/OrderDetail.aspx?Id=" + ((HiddenField)e.Row.FindControl("hdnOrderNumber")).Value.ToString();
                Session["OrdSerParams"] = hdnScrollY.Value + "," + this.CurrentPage + "," + Convert.ToString(this.ViewState["SortExp"]) + "," + Convert.ToString(this.ViewState["SortOrder"]);
                Response.Redirect("orderdetail.aspx?id=" + e.CommandArgument.ToString(), false);
            }
            else if (e.CommandName == "Retransmit")
            {
                Boolean isSuccess = new SAPOperations().SubmitOrderToSAP(Convert.ToInt64(e.CommandArgument.ToString().Split(',')[0]));

                if (isSuccess)
                    lblmsg.Text = "Order # " + e.CommandArgument.ToString().Split(',')[1] + " : transmission to SAP succeeded...";
                else
                    lblmsg.Text = "Order # " + e.CommandArgument.ToString().Split(',')[1] + " : transmission to SAP failed...";

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                    BindGridViewForSupplier();
                else
                    BindGridView();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnSubmit_Click(Object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow grv in gvSupplier.Rows)
                if (((CheckBox)grv.FindControl("chk")).Checked)
                    sendVerificationEmailSupplier(Convert.ToInt64(hdnOrderID.Value), null, Convert.ToInt64(((Label)grv.FindControl("lblId")).Text), ((Label)grv.FindControl("lblSupName")).Text);

            lblmsg.Text = "Mail Sent Successfully to selected supplier(s)..";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            modal.Hide();
            lblmsg.Text = "Error in sending Email : " + ex.Message;
        }
    }

    #endregion

    #region Methods

    private void BindGridView()
    {
        try
        {
            IncentexGlobal.OrderReturnURL = Request.Url.ToString() + (Request.QueryString.Count > 0 ? String.IsNullOrEmpty(Request.QueryString["ex"]) ? "&ex=1" : "" : "?ex=1");

            DataTable dataTable = new DataTable();
            Int32 RecordCount = 0;

            if (DaysRange == "")
            {
                List<SelectCompanyOrderDetailsViewResult> objlist = new List<SelectCompanyOrderDetailsViewResult>();
                objlist = objOrdRepos.GetCompanyOrderDetailView(this.FromDate, this.ToDate, this.StoreId == "" ? null : this.StoreId, this.EmailAddress == "" ? null : this.EmailAddress, this.OrderNumber == "" ? null : this.OrderNumber, this.OrderStatus == "" ? null : this.OrderStatus, this.FirstName == "" ? null : this.FirstName, this.LastName == "" ? null : this.LastName, this.EmployeeCode == "" ? null : this.EmployeeCode, this.StationCode == "" ? null : this.StationCode, this.WorkGroupID == "" ? null : this.WorkGroupID, this.PaymentType == "" ? null : this.PaymentType, this.SAPImportStatus, HireDateBefore, this.OrderPlaced == "" ? null : this.OrderPlaced, this.BaseStationsAccess == "" ? null : this.BaseStationsAccess);

                if (objlist.Count > 0 && IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    //add by mayur for filter waiting for review status order not display
                    objlist = objlist.Where(o => o.OrderStatus.ToUpper() != "ORDER PENDING").OrderBy(le => le.LastName).ThenBy(le => le.FirstName).ToList();
                    //Order by added by devraj on 16th, October, 2012
                }

                //NOTE: Do filteration for workgroup access when coming from the report module
                if (!String.IsNullOrEmpty(Request.QueryString["ChartSubReport"]) && WorkGroupAccess != "")
                    objlist = objlist.Where(x => this.WorkGroupAccess.Contains(Convert.ToString(x.WorkgroupId))).ToList();
                //NOTE: Do filteration when coming from the report module

                if (objlist.Count > 0)
                {
                    RecordCount = objlist.Count;

                    TotalRecords = objlist.Where(a => a.OrderStatus.ToUpper() != "CANCELED" && a.OrderStatus.ToUpper() != "ORDER PENDING").Count();

                    if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        TotalDollarValue = Convert.ToDecimal(objlist.Where(a => a.OrderStatus.ToUpper() != "CANCELED" && a.OrderStatus.ToUpper() != "ORDER PENDING").Sum(a => (a.AmountWithOutMOAS + a.CE_MOASAmount)));
                    else
                        TotalDollarValue = Convert.ToDecimal(objlist.Where(a => a.OrderStatus.ToUpper() != "CANCELED" && a.OrderStatus.ToUpper() != "ORDER PENDING").Sum(a => (a.AmountWithOutMOAS + a.CAIE_MOASAmount)));

                    dataTable = ListToDataTable(objlist);
                }
            }
            else
            {
                List<SelectShippedCompleteOrdersResult> objlist = new List<SelectShippedCompleteOrdersResult>();
                objlist = objOrdRepos.GetShippedCompleteOrders(this.FromDate, this.ToDate, this.StoreId == "" ? null : this.StoreId, this.EmailAddress == "" ? null : this.EmailAddress, this.OrderNumber == "" ? null : this.OrderNumber, this.OrderStatus == "" ? null : this.OrderStatus, this.FirstName == "" ? null : this.FirstName, this.LastName == "" ? null : this.LastName, this.EmployeeCode == "" ? null : this.EmployeeCode, this.StationCode == "" ? null : this.StationCode, this.WorkGroupID == "" ? null : this.WorkGroupID, this.PaymentType == "" ? null : this.PaymentType, this.HireDateBefore, this.OrderPlaced == "" ? null : this.OrderPlaced);
                ReportRepository objReportRepository = new ReportRepository();

                //NOTE: Do filteration for workgroup access when coming from the report module
                if (!String.IsNullOrEmpty(Request.QueryString["ChartSubReport"]) && WorkGroupAccess != "")
                {
                    objlist = objlist.Where(x => this.WorkGroupAccess.Contains(x.WorkgroupId.ToString()) && this.BaseStationsAccess.Contains(x.BaseStation.ToString())).ToList();
                }
                //NOTE: Do filteration when coming from the report module

                if (DaysRange == "1")
                    objlist = objlist.Where(X => objReportRepository.GetDateDifference(X.OrderDate, X.ShipDate, 0, 1)).ToList();
                else if (DaysRange == "5")
                    objlist = objlist.Where(X => objReportRepository.GetDateDifference(X.OrderDate, X.ShipDate, 2, 5)).ToList();
                else if (DaysRange == "7")
                    objlist = objlist.Where(X => objReportRepository.GetDateDifference(X.OrderDate, X.ShipDate, 6, 7)).ToList();
                else if (DaysRange == "14")
                    objlist = objlist.Where(X => objReportRepository.GetDateDifference(X.OrderDate, X.ShipDate, 8, 14)).ToList();
                else
                    objlist = objlist.Where(X => objReportRepository.GetDateDifference(X.OrderDate, X.ShipDate, 14, -1)).ToList();

                if (objlist.Count > 0)
                {
                    RecordCount = objlist.Count;

                    TotalRecords = objlist.Where(a => a.OrderStatus.ToUpper() != "CANCELED" && a.OrderStatus.ToUpper() != "ORDER PENDING").Count();

                    if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        TotalDollarValue = Convert.ToDecimal(objlist.Where(a => a.OrderStatus.ToUpper() != "CANCELED" && a.OrderStatus.ToUpper() != "ORDER PENDING").Sum(a => (a.AmountWithOutMOAS + a.CE_MOASAmount)));
                    else
                        TotalDollarValue = Convert.ToDecimal(objlist.Where(a => a.OrderStatus.ToUpper() != "CANCELED" && a.OrderStatus.ToUpper() != "ORDER PENDING").Sum(a => (a.AmountWithOutMOAS + a.CAIE_MOASAmount)));

                    dataTable = ListToDataTable(objlist);
                }
            }

            if (RecordCount > 0)
            {
                DataView myDataView = new DataView();
                myDataView = dataTable.DefaultView;

                if (this.ViewState["SortExp"] != null)
                    myDataView.Sort = Convert.ToString(this.ViewState["SortExp"]) + " " + Convert.ToString(this.ViewState["SortOrder"]);

                pds.DataSource = myDataView;
                pds.AllowPaging = true;
                pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
                pds.CurrentPageIndex = this.CurrentPage;
                lnkbtnNext.Enabled = !pds.IsLastPage;
                lnkbtnPrevious.Enabled = !pds.IsFirstPage;
                gvOrderDetail.DataSource = pds;
                gvOrderDetail.DataBind();

                if (gvOrderDetail.Rows.Count == 0)
                {
                    tdReords.Visible = false;
                    pagingtable.Visible = false;
                    lblmsg.Text = "No records found for given criteria.";
                }
                else
                {
                    tdReords.Visible = true;
                    pagingtable.Visible = true;
                    doPaging();
                    CheckForDisplayOrderAmountAccess();
                }
            }
            else
            {
                gvOrderDetail.DataSource = null;
                gvOrderDetail.DataBind();
                tdReords.Visible = false;
                pagingtable.Visible = false;
                lblmsg.Text = "No records found for given criteria.";
            }

            lblRecords.Text = String.Format("Total Orders : {0} <br/> Orders Total Value : ${1}", TotalRecords.ToString(), TotalDollarValue.ToString("0,0.00"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Check for the Access Permission for the Logged In User to Display Total Order Amount 
    /// Added by Prashant - Jan 2013
    /// </summary>
    protected void CheckForDisplayOrderAmountAccess()
    {
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
        {
            CompanyEmployee objCompanyEmployee = objCompEmpRepos.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
            if (objCompanyEmployee != null && objCompanyEmployee.DisplayTotalOrderAmount != null)
            {
                if (Convert.ToBoolean(objCompanyEmployee.DisplayTotalOrderAmount))
                    lblRecords.Visible = true;
                else
                    lblRecords.Visible = false;
            }
            else
                lblRecords.Visible = false;
        }
    }

    private void BindGridViewForSupplier()
    {
        try
        {
            IncentexGlobal.OrderReturnURL = Request.Url.ToString() + (Request.QueryString.Count > 0 ? String.IsNullOrEmpty(Request.QueryString["ex"]) ? "&ex=1" : "" : "?ex=1");

            Supplier objSupplier = new SupplierRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
            this.SupplierId = objSupplier.SupplierID;
            List<SelectOrderBySearchCriteriaForSupplierResult> lstOrders = objOrdRepos.GetSupplierOrders(this.FromDate, this.ToDate, this.StoreId == "" ? null : this.StoreId, this.EmailAddress == "" ? null : this.EmailAddress, this.OrderNumber == "" ? null : this.OrderNumber, this.OrderStatus == "" ? null : this.OrderStatus, this.FirstName == "" ? null : this.FirstName, this.LastName == "" ? null : this.LastName, this.EmployeeCode == "" ? null : this.EmployeeCode, this.StationCode == "" ? null : this.StationCode, this.WorkGroupID == "" ? null : this.WorkGroupID, this.PaymentType == "" ? null : this.PaymentType, HireDateBefore, this.OrderPlaced == "" ? null : this.OrderPlaced, this.BaseStationsAccess == "" ? null : this.BaseStationsAccess, this.SupplierId);

            //NOTE: Do filteration for workgroup access when coming from the report module
            if (!String.IsNullOrEmpty(Request.QueryString["ChartSubReport"]) && WorkGroupAccess != "")
                lstOrders = lstOrders.Where(x => this.WorkGroupAccess.Contains(x.WorkgroupId.ToString())).ToList();
            //NOTE: Do filteration when coming from the report module

            if (lstOrders.Count > 0)
            {
                lstSupplierOrders = lstOrders;
                tdReords.Visible = true;

                TotalRecords = lstOrders.Where(a => a.OrderStatus.ToUpper() != "CANCELED").Count();

                TotalDollarValue = Convert.ToDecimal(lstOrders.Where(a => a.OrderStatus.ToUpper() != "CANCELED").Sum(a => (a.AmountWithOutMOAS + a.CAIE_MOASAmount)));

                CheckForDisplayOrderAmountAccess();

                //NOTE: Do filteration for workgroup access when coming from the report module
                if (!String.IsNullOrEmpty(Request.QueryString["ChartSubReport"]) && WorkGroupAccess != "")
                    lstSupplierOrders = lstSupplierOrders.Where(x => this.WorkGroupAccess.Contains(x.WorkgroupId.ToString())).ToList();
                //NOTE: Do filteration when coming from the report module

                TotalRecords = lstSupplierOrders.Where(a => a.OrderStatus.ToUpper() != "CANCELED").Count();
                TotalDollarValue = Convert.ToDecimal(lstSupplierOrders.Where(a => a.OrderStatus.ToUpper() != "CANCELED").Sum(a => a.FinalAmount));

                DataView myDataView = new DataView();
                DataTable dataTable = ListToDataTable(lstSupplierOrders);
                myDataView = dataTable.DefaultView;

                if (this.ViewState["SortExp"] != null)
                    myDataView.Sort = Convert.ToString(this.ViewState["SortExp"]) + " " + Convert.ToString(this.ViewState["SortOrder"]);

                pds.DataSource = myDataView;
                pds.AllowPaging = true;
                pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
                pds.CurrentPageIndex = this.CurrentPage;
                lnkbtnNext.Enabled = !pds.IsLastPage;
                lnkbtnPrevious.Enabled = !pds.IsFirstPage;
                gvOrderDetail.DataSource = pds;
                gvOrderDetail.DataBind();

                if (gvOrderDetail.Rows.Count == 0)
                {
                    tdReords.Visible = false;
                    pagingtable.Visible = false;
                    lblmsg.Text = "No records found for given criteria.";
                }
                else
                {
                    tdReords.Visible = true;
                    pagingtable.Visible = true;
                    gvOrderDetail.Columns[5].Visible = true;
                    doPaging();
                    CheckForDisplayOrderAmountAccess();
                }
            }
            else
            {
                gvOrderDetail.DataSource = null;
                gvOrderDetail.DataBind();
                tdReords.Visible = false;
                pagingtable.Visible = false;
                lblmsg.Text = "No records found for given criteria.";
            }

            lblRecords.Text = String.Format("Total Orders : {0} <br/> Orders Total Value : ${1}", TotalRecords.ToString(), TotalDollarValue.ToString("0,0.00"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendEmailToSupplier(Int64 orderID)
    {
        try
        {
            Order objOrder = new Order();
            OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
            List<SelectSupplierAddressBySupplierIdResult> obj = new List<SelectSupplierAddressBySupplierIdResult>();
            objOrder = OrderRepos.GetByOrderID(orderID);

            if (objOrder != null)
                obj = OrderRepos.GetSupplierAddressBySupplier(objOrder.OrderFor, objOrder.OrderID, new SupplierRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).SupplierID);

            foreach (SelectSupplierAddressBySupplierIdResult repeaterItem in obj)
                sendVerificationEmailSupplier(objOrder.OrderID, objOrder.MyShoppingCartID, repeaterItem.SupplierID, repeaterItem.Name);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendVerificationEmailSupplier(Int64 OrderID, String ShoppingCartID, Int64 supplierId, String fullName)
    {
        try
        {
            //Get supplierinfo by id
            UserInformation objSupplierUserInformation = new UserInformationRepository().GetById(new SupplierRepository().GetById(supplierId).UserInfoID);
            String supplieremail = objSupplierUserInformation.LoginEmail;
            //End

            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
            OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
            CompanyRepository objCmpRepo = new CompanyRepository();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Placed Supplier";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                Order objOrder = objRepos.GetByOrderID(OrderID);
                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                String sSubject = "Order confirmation from" + " " + objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + ": " + OrderNumber;
                String sToadd = supplieremail;

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());

                messagebody.Replace("{firstnote}", "Please review the following order below if you have any questions please post them to the notes section of this order located within the order in the system.");
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName);
                messagebody.Replace("{OrderStatus}", "OPEN");
                messagebody.Replace("{FullName}", fullName);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());
                messagebody.Replace("{PaymentType}", "");
                messagebody.Replace("{Payment Method :}", "");

                //shipping address
                messagebody.Replace("{Shipping Address}", "Ship To:");
                messagebody.Replace("{AirportView1}", "Airport :");
                messagebody.Replace("{DepartmentView1}", "Department :");
                messagebody.Replace("{NameView1}", "Name :");
                messagebody.Replace("{CompanyNameView1}", "Company Name :");
                messagebody.Replace("{AddressView1}", "Address :");
                messagebody.Replace("{CityView1}", "City :");
                messagebody.Replace("{StateView1}", "State :");
                messagebody.Replace("{ZipView1}", "Zip :");
                messagebody.Replace("{CountryView1}", "Country :");
                messagebody.Replace("{PhoneView1}", "Phone :");
                messagebody.Replace("{EmailView1}", "Email :");

                CompanyEmployeeContactInfo objShippingInfo;

                if (objOrder.OrderFor == "ShoppingCart")
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                else
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");

                CityRepository objCity = new CityRepository();
                StateRepository objState = new StateRepository();
                CountryRepository objCountry = new CountryRepository();

                messagebody.Replace("{Airport1}", objShippingInfo.Airport);

                if (objShippingInfo.DepartmentID != null)
                    messagebody.Replace("{Department1}", new LookupRepository().GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
                else
                    messagebody.Replace("{Department1}", "");

                messagebody.Replace("{Name1}", objShippingInfo.Name + " " + objShippingInfo.Fax);
                messagebody.Replace("{CompanyName1}", objShippingInfo.CompanyName);
                messagebody.Replace("{Address11}", objShippingInfo.Address + "<br/>" + objShippingInfo.Address2 + "<br/>" + objShippingInfo.Street);
                messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
                messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
                messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
                messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
                messagebody.Replace("{Email1}", objShippingInfo.Email);

                //#region start
                String innermessageSupplier = "";

                List<SelectSupplierAddressResult> objSupplier = new OrderConfirmationRepository().GetSupplierAddress(objOrder.OrderFor, objOrder.OrderID);

                #region Supplier
                //Get and Bind Supplier Details
                foreach (SelectSupplierAddressResult eachsupplier in objSupplier)
                {
                    if (supplierId == eachsupplier.SupplierID)
                    {
                        innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td colspan='9'>";
                        innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                        innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td align='center' colspan='2'  style='background-color:Gray;font-weight:bold;color:Black;'>";
                        innermessageSupplier = innermessageSupplier + eachsupplier.CompanyName;
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "</tr>";
                        /*innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td style='width:50%'>";
                        innermessageSupplier = innermessageSupplier + "<b>Vendor</b>:" + eachsupplier.CompanyName;
                        innermessageSupplier = innermessageSupplier + "<br />";
                        innermessageSupplier = innermessageSupplier + eachsupplier.CompanyAdddress;
                        innermessageSupplier = innermessageSupplier + "<br />";
                        innermessageSupplier = innermessageSupplier + eachsupplier.CityName + "," + eachsupplier.sStatename + " " + eachsupplier.Name + "</td>";
                        //innermessageSupplier = innermessageSupplier + "</tr>";
                        //innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td style='width: 50%'>";
                        innermessageSupplier = innermessageSupplier + "<b>Contact:</b>" + eachsupplier.Name;
                        innermessageSupplier = innermessageSupplier + "<br />";
                        innermessageSupplier = innermessageSupplier + "<b>Account #:</b>" + eachsupplier.BankAccountNumber;
                        innermessageSupplier = innermessageSupplier + "<br />";
                        innermessageSupplier = innermessageSupplier + "<b>Telephone:</b>" + eachsupplier.Telephone;
                        innermessageSupplier = innermessageSupplier + "<br />";
                        innermessageSupplier = innermessageSupplier + "<b>Email:</b>" + eachsupplier.ContactEmail;
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "</tr>";*/
                        innermessageSupplier = innermessageSupplier + "</table>";
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "</tr>";

                        //Header Row
                        //innermessageSupplier = innermessageSupplier + "<tr>";
                        //innermessageSupplier = innermessageSupplier + "<td colspan='7'>";
                        //innermessageSupplier = innermessageSupplier + "<hr />";
                        //innermessageSupplier = innermessageSupplier + "</td>";
                        //innermessageSupplier = innermessageSupplier + "</tr>";
                        innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td colspan='9'>";
                        innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                        innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: left;'>";
                        innermessageSupplier = innermessageSupplier + "Ordered";
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessageSupplier = innermessageSupplier + "Item#";
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                        innermessageSupplier = innermessageSupplier + "Size";
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                        innermessageSupplier = innermessageSupplier + "Color";
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "<td width='35%' style='font-weight: bold; text-align: center;'>";
                        innermessageSupplier = innermessageSupplier + "Description";
                        innermessageSupplier = innermessageSupplier + "</td>";

                        //Add mayur 20-Jan-2012
                        innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                        innermessageSupplier = innermessageSupplier + "Unit Price";
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                        innermessageSupplier = innermessageSupplier + "Extended Price";
                        innermessageSupplier = innermessageSupplier + "</td>";
                        //End

                        innermessageSupplier = innermessageSupplier + "</table>";
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "</tr>";
                        innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td colspan='9'>";
                        innermessageSupplier = innermessageSupplier + "<hr />";
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "</tr>";
                        //End

                        List<SelectOrderDetailsResult> obj1 = new List<SelectOrderDetailsResult>();
                        String[] MyShoppingcart;
                        MyShoppingcart = objOrder.MyShoppingCartID.Split(',');
                        for (Int32 i = 0; i < MyShoppingcart.Count(); i++)
                        {
                            obj1 = new OrderConfirmationRepository().GetDetailOrder(Convert.ToInt32(MyShoppingcart[i]), Convert.ToInt32(eachsupplier.SupplierID), objOrder.OrderFor);
                            if (obj1.Count > 0)
                            {

                                foreach (SelectOrderDetailsResult s in obj1)
                                {
                                    innermessageSupplier = innermessageSupplier + "<tr>";
                                    innermessageSupplier = innermessageSupplier + "<td colspan='9'>";
                                    innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                                    innermessageSupplier = innermessageSupplier + "<tr>";
                                    innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: left;'>";
                                    innermessageSupplier = innermessageSupplier + s.Quantity;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    innermessageSupplier = innermessageSupplier + "<td width='15%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + s.ItemNumber;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + s.Size;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + s.Color;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    innermessageSupplier = innermessageSupplier + "<td width='35%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + s.ProductDescrption;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    //Updated by Prashant 12th martch 2013
                                    innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + "$" + (s.MOASItemPrice != null ? Convert.ToDecimal(s.MOASItemPrice) : Convert.ToDecimal(s.Price)).ToString();
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + "$" + (s.MOASItemPrice != null ? Convert.ToDecimal(s.MOASItemPrice) : Convert.ToDecimal(s.Price)) * Convert.ToDecimal(s.Quantity);
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    //End
                                    innermessageSupplier = innermessageSupplier + "</tr>";
                                    innermessageSupplier = innermessageSupplier + "</table>";
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                    innermessageSupplier = innermessageSupplier + "</tr>";
                                }

                            }

                        }
                        innermessageSupplier = innermessageSupplier + "<tr>";
                        innermessageSupplier = innermessageSupplier + "<td colspan='9'>";
                        innermessageSupplier = innermessageSupplier + "<hr />";
                        innermessageSupplier = innermessageSupplier + "</td>";
                        innermessageSupplier = innermessageSupplier + "</tr>";
                    }

                }
                messagebody.Replace("{innermesaageforsupplier}", innermessageSupplier);

                //End
                #endregion

                messagebody.Replace(" {Order Notes:}", "Order Notes :");
                messagebody.Replace("{ShippingCostView}", "");
                messagebody.Replace("{SalesTaxView}", "");
                messagebody.Replace("{OrderTotalView}", "Order Total :");

                String n = NameBars(objOrder.OrderID);
                if (n != null)
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + n);
                else
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));

                messagebody.Replace("{ShippingCost}", "");
                messagebody.Replace("{Saletax}", "");
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal((objOrder.MOASSalesTax != null ? objOrder.MOASSalesTax : objOrder.SalesTax) + (objOrder.MOASOrderAmount != null ? objOrder.MOASOrderAmount : objOrder.OrderAmount)).ToString());
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(objSupplierUserInformation.UserInfoID, "Order Detail To Supplier", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private String NameBars(Int64 OrderId)
    {
        StringBuilder strNameBars = new StringBuilder();
        Order objOrder = new OrderConfirmationRepository().GetByOrderID(OrderId);
        if (objOrder.OrderFor == "IssuanceCart")
        {
            List<MyIssuanceCart> objIssList = new MyIssuanceCartRepository().GetIssuanceCartByOrderID(objOrder.OrderID);
            foreach (MyIssuanceCart objItem in objIssList)
            {
                if (objItem.NameToBeEngraved != null && objItem.NameToBeEngraved != String.Empty)
                {
                    if (objItem.NameToBeEngraved.Contains(','))
                    {
                        String[] EmployeeTitle = objItem.NameToBeEngraved.Split(',');
                        strNameBars.Append("Name to be Engraved on Name Bar:" + EmployeeTitle[0] + "\n");
                        strNameBars.Append("Employee Title" + EmployeeTitle[1] + "\n");
                    }
                    else
                    {
                        strNameBars.Append("Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n");
                    }
                }
            }
        }
        else
        {
            List<SelectMyShoppingCartProductResult> objMyShpList = new MyShoppingCartRepository().SelectCurrentOrderProducts(objOrder.OrderID);
            foreach (SelectMyShoppingCartProductResult objItem in objMyShpList)
            {
                if (objItem.NameToBeEngraved != null && objItem.NameToBeEngraved != String.Empty)
                {
                    if (objItem.NameToBeEngraved.Contains(','))
                    {
                        String[] EmployeeTitle = objItem.NameToBeEngraved.Split(',');
                        strNameBars.Append("Name to be Engraved on Name Bar:" + EmployeeTitle[0] + "\n");
                        strNameBars.Append("Employee Title:" + EmployeeTitle[1] + "\n");
                    }
                    else
                    {
                        strNameBars.Append("Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n");
                    }
                }
            }
        }

        return strNameBars.ToString();
    }

    private void GetAndBindSupplier(Int64 orderid)
    {
        Order objOrder = new Order();
        OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
        List<SelectSupplierAddressResult> obj = new List<SelectSupplierAddressResult>();
        List<SelectSupplierAddressBySupplierIdResult> objSupplier = new List<SelectSupplierAddressBySupplierIdResult>();
        objOrder = OrderRepos.GetByOrderID(orderid);

        if (objOrder != null)
        {
            String[] MyShoppingcart;
            MyShoppingcart = objOrder.MyShoppingCartID.Split(',');
            obj = OrderRepos.GetSupplierAddress(objOrder.OrderFor, objOrder.OrderID);
        }

        gvSupplier.DataSource = obj;
        gvSupplier.DataBind();

        hdnOrderID.Value = orderid.ToString();
    }

    #region Pagging Methods

    public Int32 CurrentPage
    {
        get
        {
            return Convert.ToInt32(this.ViewState["CurrentPage"]);
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }

    public Int32 PagerSize
    {
        get
        {
            if (this.ViewState["PagerSize"] == null)
                return Convert.ToInt32(Application["GRIDPAGERSIZE"]);
            else
                return Convert.ToInt32(this.ViewState["PagerSize"]);
        }
        set
        {
            this.ViewState["PagerSize"] = value;
        }
    }

    public Int32 FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt32(this.ViewState["FrmPg"]);
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }

    public Int32 ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return this.PagerSize;
            else
                return Convert.ToInt32(this.ViewState["ToPg"]);
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

            Int32 CurrentPg = pds.CurrentPageIndex + 1;

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

            for (Int32 i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            DataList2.DataSource = dt;
            DataList2.DataBind();

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnPrevious_Click(Object sender, EventArgs e)
    {
        this.CurrentPage -= 1;
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
            BindGridViewForSupplier();
        else
            BindGridView();
    }

    /// <summary>
    /// Called Next record on the basic of
    /// No of paging.
    /// Nagmani 08/10/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnNext_Click(Object sender, EventArgs e)
    {
        this.CurrentPage += 1;
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
            BindGridViewForSupplier();
        else
            BindGridView();
    }

    protected void DataList2_ItemCommand(Object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            this.CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                BindGridViewForSupplier();
            else
                BindGridView();
        }
    }

    /// <summary>
    /// lnkbtnPaging of paging button is enable false 
    /// Nagmani 08/10/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataList2_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(this.CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        DataTable dt = new DataTable();

        foreach (PropertyInfo info in typeof(T).GetProperties())
            dt.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);

        foreach (T t in list)
        {
            DataRow row = dt.NewRow();

            foreach (PropertyInfo info in typeof(T).GetProperties())
                row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;

            dt.Rows.Add(row);
        }

        return dt;
    }

    #endregion

    #endregion
}