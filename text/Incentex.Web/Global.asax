<%@ Application Language="C#" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Incentex.DAL" %>
<%@ Import Namespace="Incentex.DAL.SqlRepository" %>

<script RunAt="server">
    
    void Application_BeginRequest(Object sender, EventArgs e)
    {
       if ((Request.Url.AbsoluteUri.Contains(".aspx")))
        {
            if ((Request.Url.AbsoluteUri.Contains("OrderDetail.aspx")))
            {

                OrderDetailHistoryRepository objHis = new OrderDetailHistoryRepository();
                OrderDetailPageHistory objPageHistory = objHis.GetByOrderId(Convert.ToInt64(Request.QueryString["Id"]));
                if (objPageHistory == null)
                {
                    OrderDetailPageHistory objPageHistory1 = new OrderDetailPageHistory();
                    objPageHistory1.OrderId = Convert.ToInt64(Request.QueryString["Id"]);
                    objHis.Insert(objPageHistory1);
                    objHis.SubmitChanges();

                }
            }
            else
            {
                OrderDetailHistoryRepository objHis = new OrderDetailHistoryRepository();
                if (Request.UrlReferrer != null)
                {
                    if (Request.UrlReferrer.Query != "")
                    {
                        if ((Request.UrlReferrer.AbsoluteUri.Contains("OrderDetail.aspx")) || ((Request.UrlReferrer.AbsoluteUri.Contains("OrderDetailForPrint.aspx"))) || ((Request.UrlReferrer.AbsoluteUri.Contains("OrderNotesHistory.aspx"))) || ((Request.UrlReferrer.AbsoluteUri.Contains("OrderInvoicePayment.aspx"))) || ((Request.UrlReferrer.AbsoluteUri.Contains("ViewOrdeShipments.aspx"))) || ((Request.UrlReferrer.AbsoluteUri.Contains("OrderShipmentDetails.aspx"))) || ((Request.UrlReferrer.AbsoluteUri.Contains("OrderShippingDetails.aspx"))) || ((Request.UrlReferrer.AbsoluteUri.Contains("OrderItemEditQty.aspx"))))
                        {

                            if ((!(Request.Url.AbsoluteUri.Contains("OrderDetailForPrint.aspx"))) && (!(Request.Url.AbsoluteUri.Contains("OrderNotesHistory.aspx"))) && (!(Request.Url.AbsoluteUri.Contains("OrderInvoicePayment.aspx"))) && (!(Request.Url.AbsoluteUri.Contains("ViewOrdeShipments.aspx"))) && (!(Request.Url.AbsoluteUri.Contains("OrderShippingDetails.aspx"))) && (!(Request.Url.AbsoluteUri.Contains("OrderShipmentDetails.aspx"))) && (!(Request.Url.AbsoluteUri.Contains("OrderItemEditQty.aspx"))))
                            {
                                 //&& !(Request.Url.AbsoluteUri.Contains("OrderNotesHistory.aspx")) && !(Request.Url.AbsoluteUri.Contains("OrderQtyShipped.aspx")) && !(Request.Url.AbsoluteUri.Contains("OrderInvoicePayment.aspx"))
                                string[] a = new string[10];
                                //If page is "shipment details page
                                if (Request.UrlReferrer.Query.Contains("&"))
                                {

                                    string[] b = Request.UrlReferrer.Query.Split('&');
                                    a = b[0].Split('=');
                                    
                                }
                                //else other pages are there.
                                else
                                {
                                     a = Request.UrlReferrer.Query.Split('=');
                                }
                                OrderDetailPageHistory objPageHistory = objHis.GetByOrderId(Convert.ToInt64(a[1]));
                                if (objPageHistory != null)
                                {
                                    OrderDetailPageHistory objPageHistory1 = new OrderDetailPageHistory();
                                    objPageHistory.UserId = null;
                                    objPageHistory.OutDateTime = System.DateTime.Now;
                                    objHis.SubmitChanges();

                                }
                            }
                        }

                    }
                }
            }

        }

    }
    System.Data.DataSet dsSetting = new System.Data.DataSet();
    AppSettingDA objSettingDA = new AppSettingDA();
    AppSettingBE objSettingBE = new AppSettingBE();
    void Application_Start(object sender, EventArgs e)
    {
        try
        {
            objSettingBE.SOperation = "SELECTALL";
            dsSetting = objSettingDA.AppSetting(objSettingBE);
            if (dsSetting != null)
            {

                for (int i = 0; i <= (dsSetting.Tables[0].Rows.Count - 1); i++)
                {
                    //Application.Lock();
                    Application[Convert.ToString(dsSetting.Tables[0].Rows[i]["sSettingName"])] = Convert.ToString(dsSetting.Tables[0].Rows[i]["sSettingValue"]);
                    Application.UnLock();
                }
            }
        }
        catch (Exception ex)
        {
            ex = null;
        }
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }



    void Session_Start(object sender, EventArgs e)
    {
        
        // Code that runs when a new session is started
        //Nagmani 30-Dec-2011
       
        Session["ParentGridViewIndex"] = -1;
        Session["SubmitDate"] = "";
        
        //End
    }

    void Session_End(object sender, EventArgs e)
    {
       /* OrderDetailHistoryRepository objHis = new OrderDetailHistoryRepository();
        System.Collections.Generic.List<OrderDetailPageHistory> objPageHistory = objHis.GetAllOrders();
        
        foreach (OrderDetailPageHistory eachhisotry in objPageHistory)
        {
            eachhisotry.UserId = null;
            eachhisotry.OutDateTime = System.DateTime.Now;
            objHis.SubmitChanges();
        }*/
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        
    }

    public void Application_Error(object sender, EventArgs e)
    {
        //Server.Transfer(HttpContext.Current.Request.ApplicationPath + "/SmartCustomErrorPage.aspx?");
    }

    public override string GetVaryByCustomString(HttpContext context, string custom)
    {
        if (custom == "Browser")
        {
            return context.Request.Browser.Browser;
        }
        return String.Empty;
    }
      
</script>

