using System;
using System.Web;
using System.Xml;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class My_Cart_CoupaCheckOut : System.Web.UI.Page
{
    public String url
    {
        get
        {
            return Convert.ToString(ViewState["url"]);
        }
        set
        {
            ViewState["url"] = value;
        }
    }

    public String xmlTextArea
    {
        get
        {
            return Convert.ToString(ViewState["xmlTextArea"]);
        }
        set
        {
            ViewState["xmlTextArea"] = value;
        }
    }

    String ViewCoupaID
    {
        get
        {
            return Convert.ToString(ViewState["ViewCoupaID"]);
        }
        set
        {
            ViewState["ViewCoupaID"] = value;
        }
    }

    /// <summary>
    /// Here we will set Session CoupaID 
    /// </summary>
    String CoupaID
    {
        get
        {
            return Convert.ToString(HttpContext.Current.Session["CoupaID"]);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewCoupaID = CoupaID;
        XmlDocument xdoc = new XmlDocument();
        CoupaPunchOutDetail objcp = new CoupaPunchOutDetail();
        objcp = new UserInformationRepository().GetCoupaPunchOutDetailbyID(Convert.ToInt64(ViewCoupaID));
        if (objcp != null)
        {
            string xmlouter = Convert.ToString(objcp.CoupaPunchOutDetails);
            url = Convert.ToString(objcp.PunchOutURL);
            xdoc.LoadXml(xmlouter);
        }
        xmlTextArea = xdoc.OuterXml;
        Session.RemoveAll();
    }
}