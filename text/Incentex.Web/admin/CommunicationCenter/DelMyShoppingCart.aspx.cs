using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CommunicationCenter_DelMyShoppingCart : System.Web.UI.Page
{
    #region Properties
    MyShoppingCartRepository objMSCRepo = new MyShoppingCartRepository();
    #endregion 
    #region Event's
    protected void Page_Load(object sender, EventArgs e)
    {
        ((HtmlControl)Master.FindControl("divheder")).Visible = false;
        Int64 UserinfoID = 0;
        if (Request.QueryString["DateTimeSpan"] != null)
        {
            DateTime nowtime = DateTime.Now;
            DateTime dtofLink = Convert.ToDateTime(Request.QueryString["DateTimeSpan"]);
            dtofLink = dtofLink.AddHours(24);
            if (nowtime < dtofLink)
            {
                UserinfoID = Convert.ToInt32(Request.QueryString["UserinfoID"]);
                UserInformation objUser = new UserInformation();
                UserInformationRepository objUserRepository = new UserInformationRepository();
                Response.Redirect("MyAccount/OrderManagement/MyPendingOrders.aspx");
            }
            else
                lblMessage.Text = "Time is expire. Please login to World Link";
        }
        else if (Request.QueryString["UserinfoID"] != null)
        {
            String MyShoppingCartID = Request.QueryString["Id"].ToString();
            UserinfoID = Convert.ToInt32(Request.QueryString["UserinfoID"]);
            int value = 1;
            String[] arrayList = MyShoppingCartID.Split(',').ToArray();
            for (int i = 0; i < arrayList.Length; i++)
            {
                if (arrayList[i].ToString() != "")
                {
                    Int32 shoppingID = Convert.ToInt32(arrayList[i].ToString());
                    value = objMSCRepo.RemoveProductsFromCart(shoppingID, UserinfoID);
                }
            }
            if (value == 0)
                lblMessage.Text = "Your Shopping cart items are deleted successfully.";
            else
                lblMessage.Text = "There is no items to delete.";
        }
    }
    #endregion
    #region Method
    #endregion
   
}
