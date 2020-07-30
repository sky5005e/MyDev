using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class OrderManagement_OrderShipmentDetails : PageBase
{
    #region Page Properties & Fields

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

    public class TrackingNumber
    {
        public String trackingnuber { get; set; }
        public Int64 suppliernumber { get; set; }
        public Int64 ordernumber { get; set; }
    }

    #endregion

    #region Page Events

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

            ((Label)Master.FindControl("lblPageHeading")).Text = "Shipping Details";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.OrderReturnURL;

            if (Request.QueryString["id"] != null)
                this.OrderID = Convert.ToInt64(Request.QueryString["id"]);

            menucontrol.PopulateMenu(1, 1, this.OrderID, 0, true);

            BindOrder();
            BindShipment();
        }
    }

    #endregion

    #region Page Methods

    private void BindOrder()
    {
        try
        {
            OrderConfirmationRepository objOrderConfir = new OrderConfirmationRepository();
            GetOrderBillNShipDetailByOrderIDResult objOrder = objOrderConfir.GetOrderBillingNShippingDetail(this.OrderID);

            if (objOrder != null)
            {
                lblOrderedDate.Text = Convert.ToDateTime(objOrder.OrderDate).ToString("MM/dd/yyyy");
                lblOrderBy.Text = Convert.ToString(objOrder.ReferenceName);
                lblOrderNo.Text = Convert.ToString(objOrder.OrderNumber);

                if (!String.IsNullOrEmpty(objOrder.PaymentOption))
                    lblPaymentMethod.Text = objOrder.PaymentOption;
                else
                    lblPaymentMethod.Text = "Paid By Corporate";

                lblOrderStatus.Text = objOrder.OrderStatus.ToString();

                BindAddresses(objOrder);

                if (objOrder.CreditUsed == "Previous")
                {
                    lblCreditType.Text = "Starting Credits";
                    trCreditType.Visible = true;
                }
                else if (objOrder.CreditUsed == "Anniversary")
                {
                    lblCreditType.Text = "Anniversary Credits";
                    trCreditType.Visible = true;
                }
                else
                    trCreditType.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindAddresses(GetOrderBillNShipDetailByOrderIDResult objOrder)
    {
        lblBAddress1.Text = objOrder.BAddress1;
        lblBAddress2.Text = objOrder.BAddress2;
        lblBCity.Text = objOrder.BCity + "," + objOrder.BState + " " + objOrder.BZipCode;
        lblBCompany.Text = objOrder.BCompany;
        lblBCountry.Text = objOrder.BCountry;
        lblBName.Text = objOrder.BFirstName + " " + objOrder.BLastName;

        lblSAddress.Text = objOrder.SAddress1;
        lblSAddress2.Text = objOrder.SAddress2;
        lblSStreet.Text = objOrder.SStreet;
        lblSCity.Text = objOrder.SCity + "," + objOrder.SState + " " + objOrder.SZipCode;
        lblSCompany.Text = objOrder.SCompany;
        lblSCountry.Text = objOrder.SCountry;
        lblSName.Text = objOrder.SFirstName + " " + objOrder.SLastName;
    }

    private void BindShipment()
    {
        try
        {
            List<GetOrderPastShipmentDetailsResult> lstShipments = new ShipOrderRepository().GetOrderPastShipments(this.OrderID, IncentexGlobal.CurrentMember.UserInfoID);

            gvShippedOrderDetail.DataSource = lstShipments;
            gvShippedOrderDetail.DataBind();

            if (lstShipments.Count == 0)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "No records found";
            }
            else
            {
                lblMsg.Visible = false;
                lblMsg.Text = String.Empty;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Control Methods

    protected void gvShippedOrderDetail_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnTrackingNo = (HiddenField)e.Row.FindControl("hdnTrackingNo");
            HiddenField hdnShippment = (HiddenField)e.Row.FindControl("hdnShippment");
            HiddenField hdnShipperService = (HiddenField)e.Row.FindControl("hdnShipperService");

            HyperLink hypShippment = (HyperLink)e.Row.FindControl("hypShippment");
            Label lblSelectedService = (Label)e.Row.FindControl("lblSelectedService");

            GridView gvTrackingNo = (GridView)e.Row.FindControl("gvTrackingNo");

            if (!String.IsNullOrEmpty(hdnTrackingNo.Value))
            {
                List<TrackingNumber> lstTrackNew = new List<TrackingNumber>();
                String[] TrackNo = Convert.ToString(hdnTrackingNo.Value).Split(',');

                foreach (String str in TrackNo)
                {
                    TrackingNumber objNew = new TrackingNumber();
                    objNew.trackingnuber = str;
                    objNew.ordernumber = this.OrderID;
                    lstTrackNew.Add(objNew);
                }

                gvTrackingNo.DataSource = lstTrackNew;
                gvTrackingNo.DataBind();
            }

            if (!String.IsNullOrEmpty(hdnShipperService.Value))
                lblSelectedService.Text = hdnShipperService.Value;
            else
                lblSelectedService.Text = "Not Found";

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            {
                hypShippment.Visible = false;
            }
            else
            {
                hypShippment.Visible = true;
                hypShippment.NavigateUrl = "~/OrderManagement/OrderQtyShipped.aspx?ShippID=" + Convert.ToString(hdnShippment.Value) + "&Id=" + this.OrderID;
            }
        }
    }

    protected void gvTrackingNo_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "TrackingNumber")
        {
            GridViewRow parentRow = (GridViewRow)((LinkButton)(e.CommandSource)).Parent.Parent.Parent.Parent.Parent.Parent;

            if (parentRow != null)
            {
                Label lblSelectedService = (Label)parentRow.FindControl("lblSelectedService");
                if (lblSelectedService.Text.Contains("UPS"))
                {
                    usercontrol_UPSPackageTracking ups = LoadControl("~/usercontrol/UPSPackageTracking.ascx") as usercontrol_UPSPackageTracking;
                    ups.TrackingNumber = e.CommandArgument.ToString();
                    pnlUPS.Controls.Add(ups);
                }
            }
        }
    }

    #endregion
}