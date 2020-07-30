using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class OrderManagement_OrderShippingDetails : PageBase
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

            menucontrol.PopulateMenu(1, 0, this.OrderID, 0, true);

            BindOrder();
            BindSuppliers();
        }
    }

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

    private void BindSuppliers()
    {
        try
        {
            OrderConfirmationRepository objOrderRepo = new OrderConfirmationRepository();
            List<GetSuppliersForOrderResult> lstSuppliers = objOrderRepo.GetSuppliersByOrderID(this.OrderID, IncentexGlobal.CurrentMember.UserInfoID);

            if (lstSuppliers.Count > 0)
            {
                parentRepeater.DataSource = lstSuppliers;
                parentRepeater.DataBind();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void parentRepeater_ItemCommand(Object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "AddNotes")
            {
                DropDownList ddlShipper = (DropDownList)e.Item.FindControl("ddlShipper");
                LookupRepository objLookRep = new LookupRepository();
                String strPayment = "Shipping Type";

                ddlShipper.DataSource = objLookRep.GetShipperByShipperType(strPayment);
                ddlShipper.DataValueField = "iLookupID";
                ddlShipper.DataTextField = "sLookupName";
                ddlShipper.DataBind();
                ddlShipper.Items.Insert(0, new ListItem("-Select-", "0"));

                ModalPopupExtender modalAddnotes = (ModalPopupExtender)e.Item.FindControl("modalAddnotes");
                modalAddnotes.Show();
            }
            else if (e.CommandName == "SAVECACE")
            {
                Int64 newshipperid = 0;
                ModalPopupExtender modalAddnotes = (ModalPopupExtender)e.Item.FindControl("modalAddnotes");
                TextBox txtShipDate = ((TextBox)e.Item.FindControl("txtShipDate"));
                DropDownList ddlShipper = ((DropDownList)e.Item.FindControl("ddlShipper"));
                TextBox txtNoOfBoxes = ((TextBox)e.Item.FindControl("txtNoOfBoxes"));
                GridView gvShippedOrderDetail = ((GridView)e.Item.FindControl("gvShippedOrderDetail"));

                //logic for inserting the values into the lookup table
                if (((DropDownList)e.Item.FindControl("ddlShipper")).SelectedItem.Text == "Other")
                {
                    if (((TextBox)e.Item.FindControl("txtOtherShipper")).Text != null)
                    {
                        INC_Lookup objNewshipper = new INC_Lookup();
                        LookupRepository objRep = new LookupRepository();
                        objNewshipper.iLookupCode = "Shipping Type";
                        objNewshipper.sLookupName = ((TextBox)e.Item.FindControl("txtOtherShipper")).Text;
                        objRep.Insert(objNewshipper);
                        objRep.SubmitChanges();
                        newshipperid = objNewshipper.iLookupID;
                    }
                }

                String alltrackingnumbers = String.Empty;
                String packageId = "Shipment_" + System.DateTime.Now.Ticks;

                //Update the values for each record existed on the page
                for (Int32 i = 0; i <= 6; i++)
                {
                    String singlevalue = ((TextBox)e.Item.FindControl("txtTrackingNumber_" + (i + 1))).Text;
                    if (!String.IsNullOrEmpty(singlevalue))
                    {
                        if (alltrackingnumbers == String.Empty)
                            alltrackingnumbers = singlevalue;
                        else
                            alltrackingnumbers = alltrackingnumbers + "," + singlevalue;
                    }
                }

                ShipOrderRepository objShipOrderRepo = new ShipOrderRepository();

                foreach (GridViewRow r in gvShippedOrderDetail.Rows)
                {
                    ShipingOrder objShipOrder = objShipOrderRepo.GetById(Convert.ToInt32(((Label)r.FindControl("hdnShipID")).Text));

                    if (!(String.IsNullOrEmpty(txtShipDate.Text)))
                        objShipOrder.ShipingDate = Convert.ToDateTime(txtShipDate.Text);

                    if (ddlShipper.SelectedIndex > 0)
                    {
                        if (ddlShipper.SelectedItem.Text != "Other")
                        {
                            objShipOrder.ShipperService = Convert.ToInt64(ddlShipper.SelectedValue);
                        }
                        else
                        {
                            if (newshipperid != 0)
                                objShipOrder.ShipperService = newshipperid;
                            else
                                objShipOrder.ShipperService = Convert.ToInt64(ddlShipper.SelectedValue);
                        }
                    }
                    else
                        objShipOrder.ShipperService = null;

                    if (txtNoOfBoxes.Text != String.Empty)
                        objShipOrder.NoOfBoxes = Convert.ToInt32(txtNoOfBoxes.Text);
                    else
                        objShipOrder.NoOfBoxes = null;

                    if (alltrackingnumbers != String.Empty)
                        objShipOrder.TrackingNo = alltrackingnumbers;
                    else
                        objShipOrder.TrackingNo = null;

                    objShipOrder.IsShipped = true;
                    objShipOrder.PackageId = packageId;
                }

                objShipOrderRepo.SubmitChanges();

                modalAddnotes.Hide();
                BindSuppliers();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void parentRepeater_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                if (this.OrderStatus.ToUpper() == "CANCELED")
                {
                    ((LinkButton)e.Item.FindControl("lnkSaveOrderDetails")).Visible = false;
                }

                HiddenField hdnSuppleriD = (HiddenField)e.Item.FindControl("hdnSupplierId");
                List<GetOrderShippingDetailsResult> lstShippingDetails = new OrderConfirmationRepository().GetOrderShippingDetails(this.OrderID, IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(hdnSuppleriD.Value)).Where(le => le.ShipQuantity > 0).ToList();

                GridView gvShippedOrderDetail = (GridView)e.Item.FindControl("gvShippedOrderDetail");

                gvShippedOrderDetail.DataSource = lstShippingDetails;
                gvShippedOrderDetail.DataBind();

                if (lstShippingDetails.Count == 0)
                {
                    ((LinkButton)e.Item.FindControl("lnkAddNew")).Visible = false;
                    ((Label)e.Item.FindControl("lblMsg")).Text = "No records found";
                    ((Label)e.Item.FindControl("lblMsg")).Visible = true;
                }
                else
                {
                    ((LinkButton)e.Item.FindControl("lnkAddNew")).Visible = true;
                    ((Label)e.Item.FindControl("lblMsg")).Text = String.Empty;
                    ((Label)e.Item.FindControl("lblMsg")).Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlShipper_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlShipper = (DropDownList)sender;
            HtmlControl otherShipper = (HtmlControl)ddlShipper.Parent.FindControl("otherShipper");

            if (ddlShipper.Items.Count > 0)
            {
                if (ddlShipper.SelectedIndex != 0)
                {
                    if (ddlShipper.SelectedItem.Text == "Other")
                        otherShipper.Visible = true;
                    else
                        otherShipper.Visible = false;

                    ModalPopupExtender modalAddnotes = (ModalPopupExtender)ddlShipper.Parent.FindControl("modalAddnotes");
                    modalAddnotes.Show();
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}