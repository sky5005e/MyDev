using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class ProductReturnManagement_ProductReturn : PageBase
{
    OrderConfirmationRepository OrdConRepository = new OrderConfirmationRepository();
    ShipOrderRepository OrdShippOrder = new ShipOrderRepository();
    LookupRepository objLookRep = new LookupRepository();
    ProductReturnRepository objPrdReturnRepos = new ProductReturnRepository();
    UserInformationRepository objUserInfoRepos = new UserInformationRepository();

    Int64 OrderID
    {
        get
        {   
            return Convert.ToInt64(ViewState["OrderID"]);
        }
        set
        {
            ViewState["OrderID"] = value;
        }
    }

    Int64 ProductReturnId
    {
        get
        {   
            return Convert.ToInt64(ViewState["ProductReturnId"]);
        }
        set
        {
            ViewState["ProductReturnId"] = value;
        }
    }

    protected void Page_Load(Object sender, EventArgs e)
    {
        //Check Here for CA/CE to Inserted Return Quantity
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
        {
            lnkBtnSaveInfo.Visible = true;
        }
        else
        {
            lnkBtnSaveInfo.Visible = false;
        }

        if (!IsPostBack)
        {
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Return";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/ProductReturnManagement/ProductReturnView.aspx";

            BindOrderClosed();

            if (Request.QueryString["OrderID"] != null)
            {
                PnlgvReturnProductUpdate.Visible = true;
                upnlOrderClosed.Visible = false;
                pnlReturnStatus.Visible = true;
                PnlgvOrderRetun.Visible = false;
                tblProductDescription.Visible = false;
                BidGridViewProductReturn();
            }
            else
            {
                upnlOrderClosed.Visible = true;
                tblProductDescription.Visible = true;
            }
        }
    }

    /// <summary>
    /// Bind Order # dropdownlist 
    /// which order no has status is Closed"
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void BindOrderClosed()
    {   
        List<GetOrdersToBeReturnedByUserInfoIDResult> objOrd = new List<GetOrdersToBeReturnedByUserInfoIDResult>();
        objOrd = OrdConRepository.GetOrderNo(IncentexGlobal.CurrentMember.UserInfoID);
        if (objOrd.Count > 0)
        {
            ddlOrderClosed.DataSource = objOrd;
            ddlOrderClosed.DataTextField = "OrderNumber";
            ddlOrderClosed.DataValueField = "OrderID";
            ddlOrderClosed.DataBind();
            ddlOrderClosed.Items.Insert(0, new ListItem("-select Order-", "0"));
        }
    }

    /// <summary>
    /// BindGriedViewOrder Selection of Dropdownlist()
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BidGridView()
    {
        List<ShipingOrder> objList = new List<ShipingOrder>();

        if (ddlOrderClosed.SelectedIndex > 0)
        {
            OrderID = Convert.ToInt64(ddlOrderClosed.SelectedValue);
        }
        else if (Request.QueryString["OrderID"] != null)
        {
            OrderID = Convert.ToInt64(Request.QueryString["OrderID"].ToString());
        }
        else
        {
            OrderID = 0;
        }

        //Check here the OrderId Return Status
        objList = OrdShippOrder.CheckReturnStatus(OrderID);

        if (objList.Count == 0)
        {
            lblmsg.Text = "This Order Number Has Not Shipped Yet";

            pnlReturnStatus.Visible = false;
        }
        else if (objList.Count > 0)
        {
            if (objList[0].ReturnStatus != null)
            {
                lblmsg.Text = "This Order Number is Already Return";
                pnlReturnStatus.Visible = false;

            }
            else
            {
                pnlReturnStatus.Visible = true;
                PnlgvReturnProductUpdate.Visible = true;
                lblmsg.Text = "";
            }
        }

        List<ShipingOrder> objList1 = new List<ShipingOrder>();
        objList1 = OrdShippOrder.GetAllShippOrder(OrderID);
        gvOrderReturn.DataSource = objList1;
        gvOrderReturn.DataBind();

        if (objList1.Count == 0)
        {
            pnlReturnStatus.Visible = false;

        }
        else
        {
            pnlReturnStatus.Visible = true;

        }
    }

    protected void lnkBtnSaveInfo_Click(Object sender, EventArgs e)
    {
        foreach (GridViewRow t in gvOrderReturn.Rows)
        {
            HiddenField hdnTrackingNo = (HiddenField)t.FindControl("hdnTrackingNumber");
            HiddenField hdnOrderID = (HiddenField)t.FindControl("hdnOrderID");
            HiddenField hdnShippID = (HiddenField)t.FindControl("hdnshippid");
            HiddenField hdnPackageID = (HiddenField)t.FindControl("hdnPackageID");
            HiddenField hdnMyShoppingCartid = (HiddenField)t.FindControl("hdnShoppingCartId");
            HiddenField hdnItemNumber = (HiddenField)t.FindControl("hdnitemNo");
            TextBox txtReturnQty = (TextBox)t.FindControl("txtReturnQty");
            DropDownList ddlRequest = (DropDownList)t.FindControl("ddlRequesting");
            HiddenField hdnShipDate = (HiddenField)t.FindControl("hdnShipDate");
            Label lblItemReceived = (Label)t.FindControl("lblReceivedQty");

            ReturnProduct objReturnProduct = new ReturnProduct();
            objReturnProduct.OrderId = Convert.ToInt64(hdnOrderID.Value);
            objReturnProduct.ReturnQty = Convert.ToInt64(txtReturnQty.Text);
            objReturnProduct.SubmitDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            objReturnProduct.ShippId = Convert.ToInt64(hdnShippID.Value);
            objReturnProduct.Requesting = Convert.ToInt64(ddlRequest.SelectedValue);
            objReturnProduct.Reason = txtPrdDescription.Text;
            objReturnProduct.ItemNumber = hdnItemNumber.Value;
            objReturnProduct.MyShoppingCartID = Convert.ToInt64(hdnMyShoppingCartid.Value);
            objReturnProduct.TrackingNumber = hdnTrackingNo.Value;
            objReturnProduct.PackageID = hdnPackageID.Value;
            objReturnProduct.Status = "Received";
            objReturnProduct.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;

            if (ddlRequest.SelectedItem.Text == "Repair")
            {
                if (!(String.IsNullOrEmpty(txtReturnQty.Text)))
                {
                    //Check Here Return Quantity Should Not be Greater Than item Received
                    if ((Convert.ToInt32(txtReturnQty.Text)) <= (Convert.ToInt32(lblItemReceived.Text)))
                    {
                        objPrdReturnRepos.Insert(objReturnProduct);
                        objPrdReturnRepos.SubmitChanges();

                        //Update ReturnStatus in shipporder table
                        OrdShippOrder.UpDateShipOrderReturnStatus(Convert.ToInt32(hdnShippID.Value), ddlRequest.SelectedItem.Text);
                        BidGridView();
                    }
                    else
                    {
                        lblmsg.Text = "Return Quantity Should Not Be Greater Than Received Quantity";
                    }
                }
            }
            else
            {
                //Check Here the ship date Should Not be Greater than 25 days for Exchange and Return
                DateTime dtShipDate = Convert.ToDateTime(hdnShipDate.Value);
                if (dtShipDate.Day <= 25)
                {
                    if (!(String.IsNullOrEmpty(txtReturnQty.Text)))
                    {
                        //Check Here Return Quantity Should Not be Greater Than item Received
                        if ((Convert.ToInt32(txtReturnQty.Text)) <= (Convert.ToInt32(lblItemReceived.Text)))
                        {
                            objPrdReturnRepos.Insert(objReturnProduct);
                            objPrdReturnRepos.SubmitChanges();

                            //Update ReturnStatus in shipporder table
                            OrdShippOrder.UpDateShipOrderReturnStatus(Convert.ToInt32(hdnShippID.Value), ddlRequest.SelectedItem.Text);
                            BidGridView();
                            if ((Convert.ToInt32(txtReturnQty.Text)) == (Convert.ToInt32(lblItemReceived.Text)))
                            {
                                txtReturnQty.Text = "";
                                txtReturnQty.Enabled = false;
                                ddlRequest.Enabled = false;
                            }
                            else
                            {
                                txtReturnQty.Text = "";
                                ddlRequest.SelectedIndex = 0;
                                ddlRequest.Enabled = true;
                            }
                        }
                        else
                        {
                            lblmsg.Text = "Return Quantity Should Not Be Greater Than Received Quantity";
                        }
                    }
                }
                else
                {
                    lblmsg.Text = "Ship Date is Greater Than 25 Days.So No Product Can Be Return And Exchanged or Refunded";
                }
            }
        }

        foreach (GridViewRow t in gvReturnProductUpdate.Rows)
        {
            ReturnProduct objReturnProduct = new ReturnProduct();
            TextBox txtReturnQty = (TextBox)t.FindControl("txtReturnQty");
            HiddenField hdnShippID = (HiddenField)t.FindControl("hdnshippid");
            TextBox txtReason = (TextBox)t.FindControl("txtReason");
            Label lblItemReceived = (Label)t.FindControl("lblReceivedQty");
            HiddenField hdnProductReturnId = (HiddenField)t.FindControl("hdnProductReturnId");
            DropDownList ddlRequest = (DropDownList)t.FindControl("ddlRequesting");
            ProductReturnId = Convert.ToInt64(hdnProductReturnId.Value);
            objReturnProduct.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

            if (this.ProductReturnId != 0)
            {
                objReturnProduct = objPrdReturnRepos.GetById(this.ProductReturnId);
            }

            objReturnProduct.ReturnQty = Convert.ToInt64(txtReturnQty.Text);
            objReturnProduct.Reason = txtReason.Text;
            objReturnProduct.Requesting = Convert.ToInt64(ddlRequest.SelectedValue);

            if (this.ProductReturnId != 0)
            {
                if (!(String.IsNullOrEmpty(txtReturnQty.Text)))
                {
                    //Check Here Return Quantity Should Not be Greater Than item Received
                    if ((Convert.ToInt32(txtReturnQty.Text)) <= (Convert.ToInt32(lblItemReceived.Text)))
                    {
                        objPrdReturnRepos.SubmitChanges();
                        this.ProductReturnId = objReturnProduct.ProductReturnId;
                        //Update ReturnStatus in shipporder table
                        OrdShippOrder.UpDateShipOrderReturnStatus(Convert.ToInt32(hdnShippID.Value), ddlRequest.SelectedItem.Text);
                        BidGridViewProductReturn();
                        lblmsg.Text = "Record Updated Successfully!";
                    }
                    else
                    {
                        lblmsg.Text = "Return Quantity Should Not Be Greater Than Received Quantity";
                    }
                }
            }
        }
    }

    protected void ddlOrderClosed_SelectedIndexChanged(Object sender, EventArgs e)
    {
        PnlgvReturnProductUpdate.Visible = false;
        PnlgvOrderRetun.Visible = true;
        BidGridView();
    }

    protected void gvOrderReturn_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            List<ShippedReturnProductDescriptionResult> objNew = new List<ShippedReturnProductDescriptionResult>();
            HiddenField hdnOrderID = (HiddenField)e.Row.FindControl("hdnOrderID");
            HiddenField hdnitemNo = (HiddenField)e.Row.FindControl("hdnitemNo");
            HiddenField hdnShoppingCartId = (HiddenField)e.Row.FindControl("hdnShoppingCartId");
            Label lblColor = (Label)e.Row.FindControl("lblColor");
            Label lblSize = (Label)e.Row.FindControl("lblSize");
            Label lblProductDescription = (Label)e.Row.FindControl("lblProductionDescription");
            DropDownList ddlRequest = (DropDownList)e.Row.FindControl("ddlRequesting");
            objNew = OrdShippOrder.GetDesCription(Convert.ToInt32(hdnOrderID.Value), Convert.ToInt32(hdnShoppingCartId.Value), hdnitemNo.Value);

            if (objNew.Count > 0)
            {
                lblColor.Text = objNew[0].Color;
                lblSize.Text = objNew[0].Size;
                lblProductDescription.Text = objNew[0].ProductDescrption;
            }

            String strRequest = "Request";
            ddlRequest.DataSource = objLookRep.GetByLookup(strRequest);
            ddlRequest.DataValueField = "iLookupID";
            ddlRequest.DataTextField = "sLookupName";
            ddlRequest.DataBind();
            ddlRequest.Items.Insert(0, new ListItem("-Select-", "0"));
        }
    }

    /// <summary>
    ///  BidGridViewProductReturn()
    ///  This gridview is bind from table Product Return
    /// </summary>
    private void BidGridViewProductReturn()
    {
        List<ReturnProduct> objList = new List<ReturnProduct>();
        if (Request.QueryString["OrderID"] != null)
        {
            OrderID = Convert.ToInt64(Request.QueryString["OrderID"].ToString());
        }
        else
        {
            OrderID = 0;
        }
        objList = objPrdReturnRepos.GetByOrderID(OrderID);
        gvReturnProductUpdate.DataSource = objList;
        gvReturnProductUpdate.DataBind();
    }

    /// <summary>
    /// Bind Color, Description,Size, Received Quantity And ShipDate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvReturnProductUpdate_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            List<ShippedReturnProductDescriptionResult> objNew = new List<ShippedReturnProductDescriptionResult>();
            List<ShipingOrder> objShipp = new List<ShipingOrder>();
            HiddenField hdnRequesting = (HiddenField)e.Row.FindControl("hdnRequesting");
            HiddenField hdnOrderID = (HiddenField)e.Row.FindControl("hdnOrderID");
            HiddenField hdnShipDate = (HiddenField)e.Row.FindControl("hdnShipDate");
            HiddenField hdnitemNo = (HiddenField)e.Row.FindControl("hdnitemNo");
            HiddenField hdniShippid = (HiddenField)e.Row.FindControl("hdnshippid");
            HiddenField hdnShoppingCartId = (HiddenField)e.Row.FindControl("hdnShoppingCartId");
            Label lblColor = (Label)e.Row.FindControl("lblColor");
            Label lblSize = (Label)e.Row.FindControl("lblSize");
            Label lblReceivedQty = (Label)e.Row.FindControl("lblReceivedQty");
            Label lblProductDescription = (Label)e.Row.FindControl("lblProductionDescription");
            DropDownList ddlRequest = (DropDownList)e.Row.FindControl("ddlRequesting");
            objNew = OrdShippOrder.GetDesCription(Convert.ToInt32(hdnOrderID.Value), Convert.ToInt32(hdnShoppingCartId.Value), hdnitemNo.Value);

            if (objNew.Count > 0)
            {
                lblColor.Text = objNew[0].Color;
                lblSize.Text = objNew[0].Size;
                lblProductDescription.Text = objNew[0].ProductDescrption;
            }

            objShipp = OrdShippOrder.GetShippAll(Convert.ToInt64(hdniShippid.Value));

            if (objShipp.Count > 0)
            {
                lblReceivedQty.Text = objShipp[0].QuantityReceived.ToString();
                hdnShipDate.Value = objShipp[0].ShipingDate.ToString();
                String strRequest = "Request";
                ddlRequest.DataSource = objLookRep.GetByLookup(strRequest);
                ddlRequest.DataValueField = "iLookupID";
                ddlRequest.DataTextField = "sLookupName";
                ddlRequest.DataBind();
                ddlRequest.Items.Insert(0, new ListItem("-Select-", "0"));
                ddlRequest.Items.FindByValue(hdnRequesting.Value).Selected = true;
            }
        }
    }
}