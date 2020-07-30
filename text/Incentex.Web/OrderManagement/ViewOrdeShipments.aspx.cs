using System;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class OrderManagement_ViewOrdeShipments : PageBase
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
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfo();
    CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfo();
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
    Order objOrder = new Order();

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

            ((Label)Master.FindControl("lblPageHeading")).Text = "Shipping Details";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/OrderManagement/OrderDetailView.aspx";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.OrderReturnURL;
            if (Request.QueryString["id"] != null)
            {
                this.OrderID = Convert.ToInt64(Request.QueryString["id"]);
            }
            //Bind Menu
            menucontrol.PopulateMenu(1, 0, this.OrderID, 0, false);

            //End

            BindOrderStatus();
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            {
                newshipment.Visible = false;
            }
            else
            {
                newshipment.Visible = true;
            }

        }
    }
    private void BindOrderStatus()
    {
        try
        {
            //Added on 24 Mar 11
            OrderConfirmationRepository objOrderConfir = new OrderConfirmationRepository();
            Order objOrder = objOrderConfir.GetByOrderID(this.OrderID);
            //lblShipping.Text = Convert.ToDecimal(objOrder.SalesTax).ToString();
            //lblSalesTax.Text = Convert.ToDecimal(objOrder.ShippingAmount).ToString();
            //lblOrderTotal.Text = Convert.ToString(Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.SalesTax) + Convert.ToDecimal(objOrder.ShippingAmount));
            lblOrderedDate.Text = Convert.ToDateTime(objOrder.OrderDate).ToString("MM/dd/yyyy");
            lblOrderBy.Text = objOrder.ReferenceName.ToString();

            if (objOrder != null)
                lblOrderNo.Text = objOrder.OrderNumber;


            if (objOrder.PaymentOption != null)
            {
                INC_Lookup objLookup = new LookupRepository().GetById((long)objOrder.PaymentOption);
                lblPaymentMethod.Text = objLookup.sLookupName;
            }
            else
            {
                lblPaymentMethod.Text = "Paid By Corporate";
            }
            //End Added
            lblOrderStatus.Text = objOrder.OrderStatus.ToString();

            //Start Update Nagmani Change 31-May-2011

            //objBillingInfo = objCmpEmpContRep.GetBillingDetailById((long)objOrder.UserId);
            //BindBillingAddress();

            // objShippingInfo = objCmpEmpContRep.GetShippingDetailById((long)objOrder.UserId, objOrder.ShippingInfromationid);
            // BindShippingAddress();

            if (objOrder.OrderFor == "IssuanceCart")
            {
                //Comapny Pays
                objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((long)objOrder.UserId, objOrder.OrderID);
                BindBillingAddress();

                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((long)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                BindShippingAddress();

            }
            else
            {
                objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((long)objOrder.UserId, objOrder.OrderID);
                BindBillingAddress();

                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((long)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                BindShippingAddress();

            }

            //End Nagmani Change 31-May-2011


            //Added on 18 Arp By Ankit
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
            {
                trCreditType.Visible = false;
            }


        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void BindBillingAddress()
    {
        lblBAddress1.Text = objBillingInfo.Address;
        lblBAddress2.Text = objBillingInfo.Address2;
        if (objBillingInfo.CityID != null && objBillingInfo.StateID != null)
            lblBCity.Text = objCity.GetById((long)objBillingInfo.CityID).sCityName + "," + objState.GetById((long)objBillingInfo.StateID).sStatename + " " + objBillingInfo.ZipCode;
        lblBCompany.Text = objBillingInfo.CompanyName;
        if (objBillingInfo.CountryID != null)
            lblBCountry.Text = objCountry.GetById((long)objBillingInfo.CountryID).sCountryName;
        //lblBEmail.Text = objBillingInfo.Email;

        if (!String.IsNullOrEmpty(objBillingInfo.BillingCO))
            lblBName.Text = objBillingInfo.BillingCO + " " + objBillingInfo.Manager;
        else
            lblBName.Text = objBillingInfo.Name;
        //lblBPhone.Text = objBillingInfo.Telephone;

    }
    protected void BindShippingAddress()
    {
        lblSAddress.Text = objShippingInfo.Address;
        lblSAddress2.Text = objShippingInfo.Address2;
        lblSStreet.Text = objShippingInfo.Street;
        if (objShippingInfo.CityID != null && objShippingInfo.StateID != null)
            lblSCity.Text = objCity.GetById((long)objShippingInfo.CityID).sCityName + "," + objState.GetById((long)objShippingInfo.StateID).sStatename + " " + objShippingInfo.ZipCode;
        lblSCompany.Text = objShippingInfo.CompanyName;
        if (objShippingInfo.CountryID != null)
            lblSCountry.Text = objCountry.GetById((long)objShippingInfo.CountryID).sCountryName;
        lblSName.Text = objShippingInfo.Name + " " + objShippingInfo.Fax;
    }
    protected void pastShipment_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/OrderManagement/OrderShipmentDetails.aspx?id=" + this.OrderID);
    }
    protected void newshipment_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/OrderManagement/OrderShippingDetails.aspx?id=" + this.OrderID);
    }
}
