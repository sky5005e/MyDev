using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Configuration;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Data.SqlClient;
using System.Globalization;



public partial class MyAccount_OrderManagement_PendingOrders : PageBase
{

    #region Data Members
    AnniversaryProgramRepository.CompanyAnniversarySortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = AnniversaryProgramRepository.CompanyAnniversarySortExpType.OrderID;
            }
            return (AnniversaryProgramRepository.CompanyAnniversarySortExpType)ViewState["SortExp"];
        }
        set
        {
            ViewState["SortExp"] = value;
        }
    }

    Incentex.DAL.Common.DAEnums.SortOrderType SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"] = Incentex.DAL.Common.DAEnums.SortOrderType.Desc;
            }
            return (Incentex.DAL.Common.DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }
    PagedDataSource pds = new PagedDataSource();
    CompanyEmployeeContactInfo objShippingInfo;


    /// <summary>
    /// Set true when user click from his/her email button view pending shopping cart.
    /// </summary>
    Boolean IsFormEmail
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsFormEmail"]);
        }
        set
        {
            ViewState["IsFormEmail"] = value;
        }
    }
    Int64 UserInfoID
    {
        get
        {
            return Convert.ToInt64(ViewState["UserInfoID"]);
        }
        set
        {
            ViewState["UserInfoID"] = value;
        }
    }

    /// <summary>
    /// Set this value when view all is clicked.
    /// </summary>
    private Boolean IsViewAllTotal
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsViewAllTotal"]);
        }
        set
        {
            ViewState["IsViewAllTotal"] = value;
        }
    }

    /// <summary>
    /// Set Shipping Info ID
    /// </summary>
    private Int64 ShippingInfoID
    {
        get
        {
            return Convert.ToInt64(ViewState["ShippingInfoID"]);
        }
        set
        {
            ViewState["ShippingInfoID"] = value;
        }
    }

    #endregion

    #region Page Event's
  
    protected void Page_Load(object sender, EventArgs e)
    {
        // Top - Link 
        base.SetTopLinkScript("admin-link");
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["UserInfoID"]) && !String.IsNullOrEmpty(Request.QueryString["IsFormEmail"]))
            {
                this.IsFormEmail = Convert.ToBoolean(Request.QueryString["IsFormEmail"].ToString());
                this.UserInfoID = Convert.ToInt64(Request.QueryString["UserInfoID"].ToString());
            }
            else
            {
                this.IsFormEmail = false;
                this.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
            }
            BindPendingOrdersGrid();
        }
    }
    
    protected void gvPendingOrders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblUserName = (Label)e.Row.FindControl("lblUserName");
            HiddenField hdnUser = (HiddenField)e.Row.FindControl("hdnUser");
            if (lblUserName != null && !String.IsNullOrEmpty(hdnUser.Value))
                lblUserName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Convert.ToString(hdnUser.Value).ToLower());

            Int64 _OrderID = Convert.ToInt64(((Label)e.Row.FindControl("lblOrderID")).Text);
            String _OrderStatus = Convert.ToString(((Label)e.Row.FindControl("lblOrderStatus")).Text);
            String _OrderFor = Convert.ToString(((Label)e.Row.FindControl("lblOrderFor")).Text);
            GridView gvOrderDetails = (GridView)e.Row.FindControl("gvOrderDetails");
            List<GetMyCartItemDetailsByOrderIDResult> ListCart = new MyShoppingCartRepository().GetMyCartItemDetailsByOrderID(_OrderID, _OrderFor, _OrderStatus);
            gvOrderDetails.DataSource = ListCart;
            gvOrderDetails.DataBind();

        }
    }

    protected void gvPendingOrders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Sorting":
                    if (this.SortExp.ToString() == e.CommandArgument.ToString())
                    {
                        if (this.SortOrder == Incentex.DAL.Common.DAEnums.SortOrderType.Asc)
                            this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Desc;
                        else
                            this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
                    }
                    else
                    {
                        this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
                        this.SortExp = (AnniversaryProgramRepository.CompanyAnniversarySortExpType)Enum.Parse(typeof(AnniversaryProgramRepository.CompanyAnniversarySortExpType), e.CommandArgument.ToString());
                    }
                    BindPendingOrdersGrid();
                    break;
                case "Approve":
                    // For Save and Approve changes
                    GridView _gvOrderDetails = (GridView)((LinkButton)e.CommandSource).FindControl("gvOrderDetails");
                    SaveGridEditChanges(_gvOrderDetails, Convert.ToString(e.CommandArgument));
                    new OrderApproval().ApproveOrder(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);
                    // new OrderApproval().GenerateEmail(Convert.ToInt64(e.CommandArgument));
                    BindPendingOrdersGrid();
                    break;

                case "EditShippingInfo":

                    Int64 orderID = Convert.ToInt64(e.CommandArgument);
                    Order objOrderEdit = new OrderConfirmationRepository().GetByOrderID(orderID);

                    if (objOrderEdit.OrderFor == "IssuanceCart")
                    {
                        objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrderEdit.UserId, objOrderEdit.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                    }
                    else
                    {
                        objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrderEdit.UserId, objOrderEdit.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                    }
                    SetShippingInfo(objShippingInfo);
                    ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "DisplayShippingInfo();", true);
                    // Grid should be expanded.
                    ClientScript.RegisterStartupScript(this.GetType(), "call showgrid", "showOrderDetailsGV('Order-" + orderID + "')", true);
                    break;

                case "SaveChanges":
                    GridView _gvOrderDetailsRead = (GridView)((LinkButton)e.CommandSource).FindControl("gvOrderDetails");
                    SaveGridEditChanges(_gvOrderDetailsRead, Convert.ToString(e.CommandArgument));
                    BindPendingOrdersGrid();
                    break;
                //case "Close":
                //    break;
                //case "Cancel":
                //    break;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// gvOrderDetails RowCommand 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvOrderDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
       
        if (e.CommandName == "DeleteOrderItem")
        {
          
            Int64 cartID = 0;
            Int64 orderID = 0;
            String order_CartID = Convert.ToString(e.CommandArgument);
            try
            {
                String[] arOrdID = order_CartID.Split(',').ToArray();
                if (arOrdID.Length > 1)
                {
                    orderID = Convert.ToInt64(arOrdID[0]);
                    cartID = Convert.ToInt64(arOrdID[1]);


                    new OrderConfirmationRepository().DeleteItemFromMOASOrder(orderID, cartID, IncentexGlobal.CurrentMember.UserInfoID);
                    // Bind grid
                    BindPendingOrdersGrid();
                }
                // Grid should be expanded.
                ClientScript.RegisterStartupScript(this.GetType(), "call showgrid", "showOrderDetailsGV('Order-" + orderID + "')", true);

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
           
        }
        
    }

    protected void ddlCountry_SelectedIndexChanged(Object sender, EventArgs e)
    {
        BindState(Convert.ToInt64(ddlCountry.SelectedValue));
        ddlState_SelectedIndexChanged(sender, e);
        // Grid should be expanded.
        ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "DisplayShippingInfo();", true);

    }

    protected void ddlState_SelectedIndexChanged(Object sender, EventArgs e)
    {
        BindCity(Convert.ToInt64(ddlState.SelectedValue));
        // Grid should be expanded.
        ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "DisplayShippingInfo();", true);
    }

    protected void lnkbtnYes_Click(object sender, EventArgs e)
    {
        try
        {
            String msgContent = Convert.ToString(hdnMainStatus.Value);
            switch (msgContent)
            {
                case "Approve Selected":
                    ApproveSelected();
                    break;
                case "Approve All":
                    ApproveALL();
                    break;
                case "Cancel All":
                    // Show Reason for cancelling.
                    ClientScript.RegisterStartupScript(this.GetType(), "show_reason_popup", "ShowReasonForCancelPopup('')", true);
                    break;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }

    protected void lnkCancelOrder_Click(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(hdnCancelOrderID.Value) && !String.IsNullOrEmpty(hdnMainStatus.Value) && hdnMainStatus.Value == "Cancel All" && !String.IsNullOrEmpty(txtReasonCode.Value))
            {
                CancelALL(Convert.ToString(txtReasonCode.Value));
            }
            else if (!String.IsNullOrEmpty(hdnCancelOrderID.Value) && !String.IsNullOrEmpty(txtReasonCode.Value))
            {
                CancelOrder(Convert.ToInt64(hdnCancelOrderID.Value), Convert.ToString(txtReasonCode.Value));
               // Response.Redirect("~/MyAccount/OrderManagement/PendingOrders.aspx?action=cancel",false);
            }
            else
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "GeneralAlertMsg('Please enter reason code!! ');", true);

            BindPendingOrdersGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkSaveShippingAddress_Click(object sender, EventArgs e)
    {
        CompanyEmployeeContactInfoRepository objCmpEmpShipping = new CompanyEmployeeContactInfoRepository();
        objShippingInfo = objCmpEmpShipping.GetShippingDetailsByID((Convert.ToInt64(ShippingInfoID)));
        if (objShippingInfo != null)
        {
            //Edit Shipping details here
            objShippingInfo.ZipCode = txtZipCode.Text.Trim();
            objShippingInfo.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFirstName.Text.Trim().ToLower());
            objShippingInfo.Fax = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtLastName.Text.Trim().ToLower());
            objShippingInfo.CompanyName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtCompany.Text.Trim().ToLower());
            objShippingInfo.Address = txtAddress1.Text.Trim();
            objShippingInfo.Address2 = txtSuiteApt.Text.Trim();
            objShippingInfo.Street = txtSuiteApt.Text.Trim();
            objShippingInfo.Email = txtEmailAddress.Text.Trim();
            objShippingInfo.Telephone = txtPhoneNumber.Text.Trim();
            objShippingInfo.CountryID = Convert.ToInt64(ddlCountry.SelectedItem.Value.ToString());
            objShippingInfo.StateID = Convert.ToInt64(ddlState.SelectedItem.Value.ToString());
            objShippingInfo.CityID = Convert.ToInt64(ddlCity.SelectedItem.Value);
            objCmpEmpShipping.SubmitChanges();
        }
        this.ShippingInfoID = 0;
    }
    #endregion 

    #region Page Method's

    private void SaveGridEditChanges(GridView gridOrderDetailsRead, String commandArg)
    {
        
        String CartID_Qty = String.Empty;
        Decimal _totalAmount = 0M;
        foreach (GridViewRow mGrid in gridOrderDetailsRead.Rows)
        {
            TextBox txtqty = (TextBox)mGrid.FindControl("txtqty");
            Label lblMyCartID = (Label)mGrid.FindControl("lblMyCartID");
            String _unitCost = ((Label)mGrid.FindControl("lblUnitCost")).Text;
            _unitCost = _unitCost.Replace("$", "");
            String _extendedPrice = ((Label)mGrid.FindControl("lblExtendedPrice")).Text;
            _extendedPrice = _extendedPrice.Replace("$", "");
            Decimal _newAmount = 0M;
            if (!String.IsNullOrEmpty(txtqty.Text))
            {
                if (!String.IsNullOrEmpty(CartID_Qty))
                    CartID_Qty += ","+lblMyCartID.Text + "_" + txtqty.Text;
                else
                    CartID_Qty = lblMyCartID.Text + "_" + txtqty.Text;
                // Multiply Amount * quantity when quantity changed
                _newAmount = Convert.ToDecimal(_unitCost) * Convert.ToDecimal(txtqty.Text);

            }
            else
                _newAmount = Convert.ToDecimal(_extendedPrice);
            // Total Amount
            _totalAmount += _newAmount;
        }
        if (!String.IsNullOrEmpty(CartID_Qty))
        {
            EditItemFromMOASOrderResult result = new OrderConfirmationRepository().EditItemFromMOASOrder(Convert.ToInt64(commandArg), CartID_Qty, IncentexGlobal.CurrentMember.UserInfoID, _totalAmount);
        }
                    
    }

    private void ApproveALL()
    {
        try
        {
            foreach (GridViewRow mGrid in gvPendingOrders.Rows)
            {
                Label lblOrderID = (Label)mGrid.FindControl("lblOrderID");
                new OrderApproval().ApproveOrder(Convert.ToInt64(lblOrderID.Text), IncentexGlobal.CurrentMember.UserInfoID);
            }
            BindPendingOrdersGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void ApproveSelected()
    {
        try
        {
            Boolean IsAnyRowChecked = false;
            foreach (GridViewRow mGrid in gvPendingOrders.Rows)
            {
                CheckBox chkSelectOrder = (CheckBox)mGrid.FindControl("chkSelectOrder");
                Label lblOrderID = (Label)mGrid.FindControl("lblOrderID");
                if (chkSelectOrder.Checked && !String.IsNullOrEmpty(lblOrderID.Text))
                {
                    IsAnyRowChecked = true;
                    new OrderApproval().ApproveOrder(Convert.ToInt64(lblOrderID.Text), IncentexGlobal.CurrentMember.UserInfoID);
                }
            }
            if (!IsAnyRowChecked)
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "GeneralAlertMsg('Please select order to approve!! ');", true);

            BindPendingOrdersGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void CancelALL(String ReasonMsg)
    {
        try
        {
            foreach (GridViewRow mGrid in gvPendingOrders.Rows)
            {
                Label lblOrderID = (Label)mGrid.FindControl("lblOrderID");
                CancelOrder(Convert.ToInt64(lblOrderID.Text), ReasonMsg);
            }
            BindPendingOrdersGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SetShippingInfo(CompanyEmployeeContactInfo objAddressInfo)
    {
        this.ShippingInfoID = objAddressInfo.CompanyContactInfoID;
        //Bind Country
        CountryRepository objRepo = new CountryRepository();
        List<INC_Country> objList = objRepo.GetAll();
        ddlCountry.DataSource = objList.OrderBy(o => o.sCountryName).ToList();
        ddlCountry.DataTextField = "sCountryName";
        ddlCountry.DataValueField = "iCountryID";
        ddlCountry.DataBind();
        ddlCountry.SelectedValue = Convert.ToString(objAddressInfo.CountryID);

        //Bind State
        BindState(Convert.ToInt64(objAddressInfo.CountryID));
        ddlState.SelectedValue = Convert.ToString(objAddressInfo.StateID);

        //bind City
        BindCity(Convert.ToInt64(objAddressInfo.StateID));
        ddlCity.SelectedValue = Convert.ToString(objAddressInfo.CityID);

        txtZipCode.Text = objAddressInfo.ZipCode;
        txtFirstName.Text = objAddressInfo.Name;
        txtLastName.Text = objAddressInfo.Fax;
        txtCompany.Text = objAddressInfo.CompanyName;
        txtAddress1.Text = objAddressInfo.Address;
        txtSuiteApt.Text = objAddressInfo.Address2;
        txtEmailAddress.Text = objAddressInfo.Email;
        txtPhoneNumber.Text = objAddressInfo.Telephone;
    }

    /// <summary>
    /// Binds the pending orders grid.
    /// Bind Paging datasource and set paging property
    /// </summary>
    private void BindPendingOrdersGrid()
    {
      
        List<GetUserPendingOrdersToApproveResult> objExtList = new List<GetUserPendingOrdersToApproveResult>();
        objExtList = new OrderConfirmationRepository().GetMyPendingOrders(this.UserInfoID, this.SortExp, this.SortOrder);
        if (objExtList.Count == 0)
            pagingtable.Visible = false;
        else
            pagingtable.Visible = true;
             
        pds.DataSource = objExtList;
        pds.AllowPaging = true;
        if (IsViewAllTotal)
        {
            CurrentPage = 0;
            pds.PageSize = objExtList.Count;
        }
        else
            pds.PageSize = 15;//Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gvPendingOrders.DataSource = pds;
        gvPendingOrders.DataBind();
        doPaging();
        // Display Total Count 
        lblPendingCount.Text = String.Format("Total Pending Orders : {0}", Convert.ToString(objExtList.Count));
    }
    /// <summary>
    /// 
    /// </summary>
    private void CancelOrder(Int64 orderID, String reasonMsg)
    {
        OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
        OrderMOASSystemRepository objOrderMOASSystemRepository = new OrderMOASSystemRepository();
        Order objCancelOrder = objRepos.GetByOrderID(orderID);
        UserInformation objCancelUser = new UserInformationRepository().GetById(Convert.ToInt64(objCancelOrder.UserId));

        objCancelOrder.OrderStatus = "Canceled";
        objCancelOrder.UpdatedDate = DateTime.Now;
        objRepos.SubmitChanges();

        //start changes added by mayur for maintain history of manager cancel order on 6-feb-2012
        OrderMOASSystem objOrderMOASSystem = objOrderMOASSystemRepository.GetByOrderIDAndManagerUserInfoID(objCancelOrder.OrderID, this.UserInfoID);
        objOrderMOASSystem.Status = "Canceled";
        objOrderMOASSystem.DateAffected = DateTime.Now;
        objOrderMOASSystemRepository.SubmitChanges();
        //end changes added by mayur for maintain history of manager cancel order on 6-feb-2012

        new OrderApproval().AddNoteHistory("Canceled", objCancelOrder.OrderID, IncentexGlobal.CurrentMember.UserInfoID, reasonMsg);

        #region Give back the credit amount he has used for this order
        if (objCancelOrder.CreditUsed != null)
        {
            CompanyEmployeeRepository objCmnyEmp = new CompanyEmployeeRepository();
            CompanyEmployee cmpEmpl = objCmnyEmp.GetByUserInfoId((Int64)objCancelOrder.UserId);
            if (objCancelOrder.CreditUsed == "Previous")
            {
                cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + objCancelOrder.CreditAmt;
                objCmnyEmp.SubmitChanges();
            }
            else if (objCancelOrder.CreditUsed == "Anniversary")
            {
                cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + objCancelOrder.CreditAmt;
                objCmnyEmp.SubmitChanges();
            }
        }
        #endregion

        #region update inventory and transfer order to shopping cart
        List<Order> obj = objRepos.GetShoppingCartId(objCancelOrder.OrderNumber);
        if (obj.Count > 0)
        {
            String[] a;

            a = obj[0].MyShoppingCartID.ToString().Split(',');

            foreach (String u in a)
            {
                if (objCancelOrder.OrderFor == "ShoppingCart")
                {
                    //Shopping cart
                    MyShoppinCart objShoppingcart = new MyShoppinCart();
                    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
                    ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                    ProductItem objProductItem = new ProductItem();
                    objShoppingcart = objShoppingCartRepository.GetById(Convert.ToInt32(u), (Int64)objCancelOrder.UserId);

                    objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(objShoppingcart.StoreProductID), Convert.ToInt64(objShoppingcart.MasterItemNo), objShoppingcart.ItemNumber);
                    //Update Inventory Here 
                    //Call here upDate Procedure
                    String strProcess = "Shopping";
                    String strMessage = objRepos.IncreaseDescreaseInventory(objProductItem.ProductItemID, -Convert.ToInt32(objShoppingcart.Quantity), strProcess);

                }
                else
                {
                    //Issuance
                    MyIssuanceCartRepository objIssuanceRepos = new MyIssuanceCartRepository();
                    MyIssuanceCart objIssuance = new MyIssuanceCart();
                    ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                    //End 

                    objIssuance = objIssuanceRepos.GetById(Convert.ToInt32(u), (Int64)objCancelOrder.UserId);
                    List<SelectProductIDResult> objList = new List<SelectProductIDResult>();
                    CompanyEmployeeRepository objcmpemprepo = new CompanyEmployeeRepository();
                    AnniversaryProgramRepository objacprepo = new AnniversaryProgramRepository();
                    Int64 storeid = objacprepo.GetEmpStoreId((Int64)objCancelOrder.UserId, objcmpemprepo.GetByUserInfoId((Int64)objCancelOrder.UserId).WorkgroupID).StoreID;
                    objList = objProItemRepos.GetProductId(objIssuance.MasterItemID, Convert.ToInt64(u), Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(storeid));
                    //Update Inventory Here 
                    //Call here upDate Procedure
                    for (Int32 i = 0; i < objList.Count; i++)
                    {
                        String strProcess = "UniformIssuance";
                        String strMessage = objRepos.IncreaseDescreaseInventory(Convert.ToInt64(objList[i].ProductItemID), -Convert.ToInt32(objIssuance.Qty), strProcess);
                        //String strMessage = objRepos.UpdateInventory(Convert.ToInt64(u), Convert.ToInt64(objList[i].ProductItemID), strProcess);
                    }
                }
            }
        }
        #endregion

        // sendVerificationEmail("Canceled", objCancelOrder.OrderID, objCancelUser.LoginEmail, objCancelUser.FirstName, objCancelUser.UserInfoID, objCancelOrder); //send email to CE 

        if (!objCancelOrder.ReferenceName.ToLower().Contains("test") && !IncentexGlobal.IsIEFromStoreTestMode)
        {
            //sendIEEmail("Canceled", objCancelOrder.OrderID);//send email to IE
        }
        else
        {
            //sendTestOrderEmailNotification("Canceled", objCancelOrder.OrderID);
        }

    }

    private void BindState(Int64 countryID)
    {
        try
        {
            StateRepository objStateRepo = new StateRepository();
            ddlState.DataSource = objStateRepo.GetByCountryId(countryID).OrderBy(le => le.sStatename).ToList();
            ddlState.DataTextField = "sStateName";
            ddlState.DataValueField = "iStateId";
            ddlState.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void BindCity(Int64 stateID)
    {
        try
        {
            CityRepository objCityRepo = new CityRepository();
            ddlCity.DataSource = objCityRepo.GetByStateId(stateID).OrderBy(le => le.sCityName).ToList();
            ddlCity.DataTextField = "sCityName";
            ddlCity.DataValueField = "iCityID";
            ddlCity.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion 

    #region Paging


    protected void dtlPaging_ItemCommand(Object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "lnkbtnPaging")
            {
                this.IsViewAllTotal = false;
                CurrentPage = Convert.ToInt16(e.CommandArgument);
                BindPendingOrdersGrid();
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
            LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");

            if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
            {
                lnkbtnPage.Enabled = false;
                lnkbtnPage.Font.Bold = true;
                lnkbtnPage.CssClass = String.Empty;
                lnkbtnPage.Font.Size = new FontUnit(12);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public Int32 CurrentPage
    {
        get
        {
            return Convert.ToInt32(ViewState["CurrentPage"]);
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }

    public Int32 FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt32(ViewState["FrmPg"]);
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
                return 3;
            else
                return Convert.ToInt32(ViewState["ToPg"]);
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
                ToPg = ToPg + 3;
            }
            else if (!IsViewAllTotal && CurrentPg == ToPg)// this will set if when user will click page index
            {
                ToPg = 3;
            }

            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - 3;
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
            // Bottom
            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();
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
            this.IsViewAllTotal = true;
            BindPendingOrdersGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnPrevious_Click(Object sender, EventArgs e)
    {
        try
        {
            this.IsViewAllTotal = false;
            CurrentPage -= 1;
            BindPendingOrdersGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnNext_Click(Object sender, EventArgs e)
    {
        try
        {
            this.IsViewAllTotal = false;
            CurrentPage += 1;
            BindPendingOrdersGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}
