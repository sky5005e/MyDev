using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Collections.Generic;
using System.Text;
using AjaxControlToolkit;
using Incentex.BE;
using Incentex.DA;
using commonlib.Common;

public partial class ProductReturnManagement_ProductItemsReturnPrint : PageBase
{
    #region Data Members
    Int64 OrderID
    {
        get
        {
            if (ViewState["OrderID"] == null)
            {
                ViewState["OrderID"] = 0;
            }
            return Convert.ToInt64(ViewState["OrderID"]);
        }
        set
        {
            ViewState["OrderID"] = value;
        }
    }
    string OrderStatus
    {
        get
        {
            if (ViewState["OrderStatus"] == null)
            {
                ViewState["OrderStatus"] = null;
            }
            return Convert.ToString(ViewState["OrderStatus"]);
        }
        set
        {
            ViewState["OrderStatus"] = value;
        }
    }
    OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
    OrderDocumentRepository objOrderDocRepos = new OrderDocumentRepository();
    ProductReturnRepository objProductReturnRepos = new ProductReturnRepository();
    Order objOrder = new Order();
    ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
    LookupRepository objLookRep = new LookupRepository();
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfo();
    CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfo();
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    #endregion

    #region Page Event's
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Order Detail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "   ";
            ((LinkButton)Master.FindControl("btnLogout")).Visible = false;
            ((HtmlImage)Master.FindControl("imgHomeBtn")).Visible = false;
            ((HtmlImage)Master.FindControl("imgSupportTickets")).Visible = false;
            ((HtmlImage)Master.FindControl("imgOpenServiceTicket")).Visible = false;

            if (Request.QueryString["Id"] != null)
            {
                this.OrderID = Convert.ToInt64(Request.QueryString["Id"]);
                setOrderStatus();
            }

            BindAddress();
            BindOrderStatus();
            BindParentRepeaterSupplierAddress();
        }
    }
    #endregion

    #region Grid Events
    protected void gvMainOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    #endregion

    #region General Methods
    private void setOrderStatus()
    {
        Order o = new OrderConfirmationRepository().GetByOrderID(this.OrderID);
        this.OrderStatus = o.OrderStatus;
    }
    private void BindAddress()
    {
        try
        {
            List<SelectOrderAddressResult> obj = new List<SelectOrderAddressResult>();
            obj = OrderRepos.GetOrderAddress(Convert.ToInt32(this.OrderID));
            if (obj.Count > 0)
            {
                lblOrderNo.Text = obj[0].OrderNumber;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    private void BindOrderStatus()
    {
        try
        {
           
            OrderConfirmationRepository objOrderConfir = new OrderConfirmationRepository();
            Order objOrder = objOrderConfir.GetByOrderID(this.OrderID);
           
            lblOrderedDate.Text = Convert.ToDateTime(objOrder.OrderDate).ToString("MM/dd/yyyy");
            lblOrderBy.Text = objOrder.ReferenceName.ToString();
            
            if (objOrder.PaymentOption != null)
            {
                INC_Lookup objLookup = new LookupRepository().GetById((long)objOrder.PaymentOption);
                lblPaymentMethod.Text = objLookup.sLookupName + (!string.IsNullOrEmpty(objOrder.PaymentOptionCode) ? " : " + objOrder.PaymentOptionCode : "");
            }
            else
            {
                lblPaymentMethod.Text = "Paid By Corporate";
            }
          
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


            lblOrderStatus.Text = objOrder.OrderStatus.ToString();


            LookupRepository objLookRep = new LookupRepository();
            string strPayment = "StatusOptionOne";
        

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


        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void BindBillingAddress()
    {
        if (objBillingInfo != null)
        {
            lblBAddress.Text = objBillingInfo.Address;
            lblBCity.Text = objCity.GetById((long)objBillingInfo.CityID).sCityName + "," + objState.GetById((long)objBillingInfo.StateID).sStatename + " " + objBillingInfo.ZipCode;
            lblBCompany.Text = objBillingInfo.CompanyName;
            lblBPhoneNumber.Text = objBillingInfo.Telephone;
            lblBEmailAddress.Text = objBillingInfo.Email;
            lblBCountry.Text = objCountry.GetById((long)objBillingInfo.CountryID).sCountryName;
           
            lblBName.Text = objBillingInfo.BillingCO + " " + objBillingInfo.Manager;
            
        }

    }
    protected void BindShippingAddress()
    {
        if (objShippingInfo != null)
        {
            lblSAddress.Text = objShippingInfo.Address;
            lblSAddress2.Text = objShippingInfo.Address2;
            lblSStreet.Text = objShippingInfo.Street;
            lblSCity.Text = objCity.GetById((long)objShippingInfo.CityID).sCityName + "," + objState.GetById((long)objShippingInfo.StateID).sStatename + " " + objShippingInfo.ZipCode;
            lblSCompany.Text = objShippingInfo.CompanyName;
            lblSCountry.Text = objCountry.GetById((long)objShippingInfo.CountryID).sCountryName;
            lblSName.Text = objShippingInfo.Name + " " + objShippingInfo.Fax;
            lblSPhoneNumber.Text = objShippingInfo.Telephone;
            lblSEmailAddress.Text = objShippingInfo.Email;
        }
    }
    private void BindParentRepeaterSupplierAddress()
    {
        try
        {
            List<GetReturnOrderDetailsResult> objList = new List<GetReturnOrderDetailsResult>();
            List<object> objMainRecord = new List<object>();

            objList = objProductReturnRepos.GetReturnOrderDetails(Convert.ToInt32(this.OrderID));
            
            gvMainOrderDetail.DataSource = objList;
            gvMainOrderDetail.DataBind();

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion
}
