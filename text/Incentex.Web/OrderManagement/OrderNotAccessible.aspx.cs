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
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;

public partial class OrderManagement_OrderNotAccessible : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Access Denied for accessing order detail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.OrderReturnURL;
            GetUsersAccessingTheOrders();
        }
    
    }
    public void GetUsersAccessingTheOrders()
    {
        Int64 OrderId = Convert.ToInt64(Request.QueryString["OrderId"].ToString());
        OrderDetailHistoryRepository objHis = new OrderDetailHistoryRepository();
        List<Incentex.DAL.SqlRepository.OrderDetailHistoryRepository.ListUsers> objUserList = objHis.GetAllUsersAccessingTheOrders(OrderId);
        if (objUserList.Count > 0)
        {
            lblUserName.Text = objUserList[0].username.ToString();
        }
    }
}
