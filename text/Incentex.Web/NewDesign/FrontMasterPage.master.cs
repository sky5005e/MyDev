using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class FrontMasterPage : System.Web.UI.MasterPage
{
    #region Variable's
    string webpath = ConfigurationSettings.AppSettings["siteurl"].ToString();
    #endregion

    #region Master Page Events that can be subscribed by content pages

    private static readonly Object objCommand = new Object();

    public delegate void ProductCategoryChangedEventHandler(Object sender, CommandEventArgs e);

    public event ProductCategoryChangedEventHandler ProductCategoryChanged
    {
        add { Events.AddHandler(objCommand, value); }
        remove { Events.RemoveHandler(objCommand, value); }
    }

    protected virtual void OnProductCategoryChanged(CommandEventArgs eventArgs)
    {
        ProductCategoryChangedEventHandler commandEventHandler = (ProductCategoryChangedEventHandler)Events[objCommand];
        if (commandEventHandler != null)
        {
            commandEventHandler(this, eventArgs);
        }
        ////Bubble up this event so that the base control ItemCommand event can be fired
        //base.RaiseBubbleEvent(this, eventArgs);
    }

    public String CommandName
    {
        get
        {
            return Convert.ToString(ViewState["CommandName"]);
        }
        set
        {
            ViewState["CommandName"] = value;
        }
    }

    public String CommandArgument
    {
        get
        {
            return Convert.ToString(ViewState["CommandArgument"]);
        }
        set
        {
            ViewState["CommandArgument"] = value;
        }
    }

    #endregion

    #region Page Event's

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CurrencyDropDown();

            this.OnProductCategoryChanged(new CommandEventArgs(this.CommandName, this.CommandArgument));
        }
        BindTitles();
    }

    protected void lnklogOut_Click(object sender, EventArgs e)
    {
        // for tracking center logout time in table(UserTracking)        

        if (IncentexGlobal.IsIEFromStore)
        {
            Response.Redirect("~/NewDesign/UserPages/Index.aspx?IsFromStoreFront=1", true);
        }

        if (IncentexGlobal.CurrentMember != null)
            UdpateUserLoginForTC();

        IncentexGlobal.CurrentMember = null;
        IncentexGlobal.GSEMgtCurrentMember = null;
        IncentexGlobal.IsIEFromStore = false;
        IncentexGlobal.IsIEFromStoreTestMode = false;
        Session.RemoveAll();
        Response.Redirect("~/login.aspx");
    }
    #endregion

    #region Page Method's

    private void CurrencyDropDown()
    {
        try
        {
            List<CountryCurrency> list = new CountryRepository().GetCountryCurrencyList();
            if (list.Count > 0)
            {
                ddlCurrencyTo.DataSource = list;
                ddlCurrencyTo.DataTextField = "CountryCurrencyName";
                ddlCurrencyTo.DataValueField = "CurrencyCode";
                ddlCurrencyTo.DataBind();
                ddlCurrencyTo.SelectedValue = "USD";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void BindTitles()
    {
        CountryRepository objCXRepo = new CountryRepository();

        List<CountryCurrency> listCountry = objCXRepo.GetCountryCurrencyList();
        if (ddlCurrencyFrom != null)
        {
            foreach (ListItem lifrom in ddlCurrencyFrom.Items)
            {
                lifrom.Attributes["title"] = webpath + "admin/Incentex_Used_Icons/CountryFlagsIcon/US.png"; // setting text value of item as tooltip
            }
        }

        if (ddlCurrencyTo != null)
        {
            foreach (ListItem lito in ddlCurrencyTo.Items)
            {
                lito.Attributes["title"] = webpath + "admin/Incentex_Used_Icons/CountryFlagsIcon/" + listCountry.Where(q => q.CurrencyCode == lito.Value).FirstOrDefault().CountryCurrencyFlagIcon; // setting text value of item as tooltip
            }
        }
    }

    // This is for the logout for the USERtracking system.
    public void UdpateUserLoginForTC()
    {
        UserTrackingRepo objTrackinRepo = new UserTrackingRepo();
        UserTracking objTrackinTbl = new UserTracking();
        DateTime LogoutTime = Convert.ToDateTime(System.DateTime.Now.TimeOfDay.ToString());

        objTrackinRepo.SetLogout(Convert.ToInt32(Session["trackID"]), LogoutTime);
    }

    #endregion
}