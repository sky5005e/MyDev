using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class OrderManagement_OrderQtyShipped : PageBase
{
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

    String PackageID
    {
        get
        {
            return Convert.ToString(ViewState["PackageID"]);
        }
        set
        {
            ViewState["PackageID"] = value;
        }
    }

    public class TrackingNumber
    {
        public String trackingnuber { get; set; }
        public String PackageId { get; set; }
    }

    private static List<TrackingNumber> ListValuesTracking
    {
        get
        {
            if ((HttpContext.Current.Session["ListValuesTracking"]) == null)
                return new List<TrackingNumber>();
            else
                return (List<TrackingNumber>)HttpContext.Current.Session["ListValuesTracking"];
        }
        set
        {
            HttpContext.Current.Session["ListValuesTracking"] = value;
        }
    }

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

            ((Label)Master.FindControl("lblPageHeading")).Text = "Order Quantity Shipped Detail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.OrderReturnURL;

            this.PackageID = Convert.ToString(Request.QueryString["ShippID"]);
            this.OrderID = Convert.ToInt64(Request.QueryString["id"]);

            bindGridView();

            menucontrol.PopulateMenu(1, 1, this.OrderID, 0, true);
            setOrderStatus();
        }
    }

    private void setOrderStatus()
    {
        Order o = new OrderConfirmationRepository().GetByOrderID(this.OrderID);
        this.OrderStatus = o.OrderStatus;
        if (this.OrderStatus.ToUpper() == "CANCELED")
        {
            lnkSaveOrderDetails.Visible = false;
            trNumber.Visible = false;
        }
    }

    public void bindGridView()
    {
        try
        {
            List<GetShippedOrderQuantityResult> lstShippedOrderQuantity = new ShipOrderRepository().GetShippeQuantityForPackingSlip(this.OrderID, this.PackageID, IncentexGlobal.CurrentMember.UserInfoID);
            if (lstShippedOrderQuantity.Count > 0)
            {
                gvShippedOrderDetail.DataSource = lstShippedOrderQuantity;
                gvShippedOrderDetail.DataBind();
            }

            SetValues(Convert.ToDateTime(lstShippedOrderQuantity[0].ShipingDate), Convert.ToInt64(lstShippedOrderQuantity[0].ShipperServiceID), lstShippedOrderQuantity[0].TrackingNo, lstShippedOrderQuantity[0].ShippingOrderStatus, lstShippedOrderQuantity[0].NoOfBoxes);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public void SetValues(DateTime ShipDate, Int64 ShipperId, String TrackingNumber, String Status2, Int32? Boxes)
    {
        String strPayment = "Shipping Type";
        ddlShipper.DataSource = new LookupRepository().GetShipperByShipperType(strPayment);
        ddlShipper.DataValueField = "iLookupID";
        ddlShipper.DataTextField = "sLookupName";
        ddlShipper.DataBind();
        ddlShipper.Items.Insert(0, new ListItem("-Select-", "0"));

        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
        {
            drpStatusTwo.DataSource = new LookupRepository().GetByLookup("StatusOptionTwo");
            drpStatusTwo.DataValueField = "iLookupID";
            drpStatusTwo.DataTextField = "sLookupName";
            drpStatusTwo.DataBind();
            drpStatusTwo.Items.Insert(0, new ListItem("-Select-", "0"));

            if (!String.IsNullOrEmpty(Status2) && drpStatusTwo.Items.FindByText(Status2) != null)
                drpStatusTwo.Items.FindByText(Status2).Selected = true;
        }

        if (TrackingNumber != null)
        {
            String[] listTrackingNumbers = TrackingNumber.Split(',');
            List<TrackingNumber> objTempList = new List<TrackingNumber>();

            foreach (String s in listTrackingNumbers)
            {
                TrackingNumber item = new TrackingNumber();
                item.PackageId = this.PackageID;
                item.trackingnuber = s;
                objTempList.Add(item);
            }

            ListValuesTracking = objTempList;
            Session["ListVal"] = ListValuesTracking;
            grvTrackingNumber.DataSource = ListValuesTracking;
            grvTrackingNumber.DataBind();
        }

        txtShipDate.Text = ShipDate.ToString("MM/dd/yyyy");
        ddlShipper.Items.FindByValue(ShipperId.ToString()).Selected = true;
        txtBoxes.Text = Convert.ToString(Boxes);
    }

    private void bindtrackingnumbergrid()
    {
        grvTrackingNumber.DataSource = ListValuesTracking;
        grvTrackingNumber.DataBind();
    }

    protected void ddlShipper_SelectedIndexChange(Object sender, EventArgs e)
    {
        if (ddlShipper.SelectedIndex != 0)
        {
            if (ddlShipper.SelectedItem.Text == "Other")
                otherShipper.Visible = true;
            else
                otherShipper.Visible = false;
        }
    }

    protected void grvTrackingNumber_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "deletetrackingnumber")
        {
            ListValuesTracking.RemoveAll(delegate(TrackingNumber t) { return t.trackingnuber == e.CommandArgument.ToString(); });
            bindtrackingnumbergrid();
        }
    }

    protected void lnkSaveOrderDetails_Click(Object sender, EventArgs e)
    {
        Boolean isError = false;
        try
        {
            Int64 newshipperid = 0;
            if (ddlShipper.SelectedItem.Text == "Other")
            {
                if (txtOtherShipper.Text != null)
                {
                    INC_Lookup objNewshipper = new INC_Lookup();
                    LookupRepository objLookUpRep = new LookupRepository();
                    objNewshipper.iLookupCode = "Shipping Type";
                    objNewshipper.sLookupName = txtOtherShipper.Text;
                    objLookUpRep.Insert(objNewshipper);
                    objLookUpRep.SubmitChanges();
                    newshipperid = objNewshipper.iLookupID;
                }
            }

            List<ShipingOrder> objShippingOrderList = new List<ShipingOrder>();
            ShipOrderRepository objRep = new ShipOrderRepository();
            objShippingOrderList = objRep.GetByPackageId(this.PackageID);

            String tranum = String.Empty;

            if (ListValuesTracking.Count > 0)
            {
                foreach (TrackingNumber tn in ListValuesTracking)
                {
                    if (tranum == String.Empty)
                        tranum = tn.trackingnuber;
                    else
                        tranum = tranum + "," + tn.trackingnuber;
                }                
            }
            else
            {
                lblmsg.Text = "Please enter atleast one tracking number";
                return;
            }

            foreach (ShipingOrder objShiporder in objShippingOrderList)
            {
                if (tranum.Length > 0)
                    objShiporder.TrackingNo = tranum;
                else
                    objShiporder.TrackingNo = null;

                objShiporder.ShipingDate = Convert.ToDateTime(txtShipDate.Text);

                if (ddlShipper.SelectedIndex > 0)
                {
                    if (ddlShipper.SelectedItem.Text != "Other")
                        objShiporder.ShipperService = Convert.ToInt64(ddlShipper.SelectedItem.Value);
                    else
                    {
                        if (newshipperid != 0)
                            objShiporder.ShipperService = newshipperid;
                        else
                            objShiporder.ShipperService = Convert.ToInt64(ddlShipper.SelectedItem.Value);
                    }
                }

                objShiporder.NoOfBoxes = Convert.ToInt32(txtBoxes.Text);

                if (drpStatusTwo.SelectedIndex > 0)
                    objShiporder.ShippingOrderStatus = drpStatusTwo.SelectedItem.Text;
                else
                    objShiporder.ShippingOrderStatus = null;                
            }

            objRep.SubmitChanges();

            bindGridView();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            isError = true;
        }

        if (!isError)
            Response.Redirect("~/OrderManagement/OrderShipmentDetails.aspx?Id=" + this.OrderID + "&SubId=0");
    }

    protected void btnTrackingNumber_Click(Object sender, EventArgs e)
    {
        try
        {
            List<TrackingNumber> obj = new List<TrackingNumber>();
            TrackingNumber objnumber = new TrackingNumber();
            objnumber.trackingnuber = txtTrackingNo.Text;
            objnumber.PackageId = this.PackageID;

            if (Session["ListVal"] != null)
            {
                List<TrackingNumber> objtemp = new List<TrackingNumber>();
                objtemp = ListValuesTracking;
                objtemp.Add(objnumber);
                obj = objtemp;
                Session["ListVal"] = obj;
            }
            else
            {
                obj.Add(objnumber);
                Session["ListVal"] = obj;
            }

            ListValuesTracking = obj;
            bindtrackingnumbergrid();
            txtTrackingNo.Text = String.Empty;
        }
        catch
        {
            Session["ListVal"] = null;
            ListValuesTracking = null;
        }
    }
}