using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Globalization;
using CurrencyConverter;

/// <summary>
/// BasePage for the common funtionality in all 
/// the web pages of the site.
/// </summary>
public class BasePage : Page
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public BasePage()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// The name of the culture selection dropdown list in the common header.
    /// </summary>
    public const string LanguageCurrencyDropDownName = "ctl00$drpCurrency$ddlLanguage";


    /// <summary>
    /// The name of the PostBack event target field in a posted form.  You can use this to see which
    /// control triggered a PostBack:  Request.Form[PostBackEventTarget] .
    /// </summary>
    public const string PostBackEventTarget = "__EVENTTARGET";

    /// <SUMMARY>
    /// Overriding the InitializeCulture method to set the user selected
    /// option in the current thread. Note that this method is called much
    /// earlier in the Page lifecycle and we don't have access to any controls
    /// in this stage, so have to use Form collection.
    /// </SUMMARY>
    protected override void InitializeCulture()
    {
        ///<remarks><REMARKS>
        ///Check if PostBack occured. Cannot use IsPostBack in this method
        ///as this property is not set yet.
        ///</remarks>
        IncentexGlobal.ConvertionRate = 1.0;
        if (Request[PostBackEventTarget] != null)
        {
            string controlID = Request[PostBackEventTarget];

            //ctl00_ContentPlaceHolder1_ddlCurrencyTo
            if (controlID.Equals(LanguageCurrencyDropDownName))
            {
                string selectedValue =
                       Request.Form[Request[PostBackEventTarget]].ToString();

                /*switch (selectedValue)
                {
                    case "0": SetCulture("en-GB");
                        break;
                    case "1": SetCulture("da-DK");
                        break;
                    case "2": SetCulture("ur-PK");
                        break;
                    default:
                        SetCulture("en-GB");
                        break;
                }*/
                SetSession(selectedValue);

            }
        }
        base.InitializeCulture();
    }


    /// <Summary>
    /// Sets the current UICulture and CurrentCulture based on
    /// the arguments
    /// </Summary>
    /// <PARAM name="name"></PARAM>
    /// <PARAM name="locale"></PARAM>
    protected void SetCulture(string culture)
    {

        //Use this
        this.UICulture = culture;
        this.Culture = culture;
        //OR This
        if (culture != "Auto")
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
        }
        /*
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(name);
        Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
        ///<remarks>
        ///Saving the current thread's culture set by the User in the Session
        ///so that it can be used across the pages in the current application.
        ///</remarks>*/
        Session["MyUICulture"] = Thread.CurrentThread.CurrentUICulture;
        Session["MyCulture"] = Thread.CurrentThread.CurrentCulture;
    }

    public void TestSetSession(string currencyto)
    {
        Double currency;
        if (IncentexGlobal.CurrencyFrom == null)
        {
            IncentexGlobal.CurrencyFrom = "USD";
            CurrencyConverter.CurrencyConvertor myref = new CurrencyConverter.CurrencyConvertor();
            IncentexGlobal.ConvertionRate = currency = myref.ConversionRate(converttocurrency(IncentexGlobal.CurrencyFrom), converttocurrency(currencyto));

        }
        else
        {
            CurrencyConverter.CurrencyConvertor myref = new CurrencyConverter.CurrencyConvertor();
            if (IncentexGlobal.CurrencyTo != null)
            {
                IncentexGlobal.CurrencyFrom = IncentexGlobal.CurrencyTo;
            }
            IncentexGlobal.CurrencyTo = currencyto;
            IncentexGlobal.ConvertionRate = currency = myref.ConversionRate(converttocurrency(IncentexGlobal.CurrencyFrom), converttocurrency(currencyto));
        }



        //   Response.Write(IncentexGlobal.CurrencyFrom + " To " + IncentexGlobal.CurrencyTo + "at" + IncentexGlobal.ConvertionRate.ToString());
    }

    public void SetSession(string currencyto)
    {
        IncentexGlobal.Currency = GetCurrency(currencyto);
        Double currency;
        if (IncentexGlobal.CurrencyFrom == null)
        {
            IncentexGlobal.CurrencyFrom = "USD";

        }
        else
        {
            CurrencyConverter.CurrencyConvertor myref = new CurrencyConverter.CurrencyConvertor();
            IncentexGlobal.CurrencyTo = currencyto;
            IncentexGlobal.ConvertionRate = currency = myref.ConversionRate(converttocurrency(IncentexGlobal.CurrencyFrom), converttocurrency(currencyto));
            if (IncentexGlobal.ConvertionRate == 0)
            {
                IncentexGlobal.ConvertionRate = 1;
            }
        }



        //   Response.Write(IncentexGlobal.CurrencyFrom + " To " + IncentexGlobal.CurrencyTo + "at" + IncentexGlobal.ConvertionRate.ToString());
    }

    public string GetCurrency(string currency)
    {
        string c;
        switch (currency)
        {
            case "USD":
                c = "$";
                break;
            case "EUR":
                c = "€";
                break;
            case "CAD":
                c = "C$";
                break;
            default:
                c = "$";
                break;

        }
        return c;



    }

    public Currency converttocurrency(string value)
    {
        Currency a = new Currency();

        switch (value)
        {

            case "CAD":
                a = Currency.CAD;
                break;
            case "EUR":
                a = Currency.EUR;
                break;
            case "USD":
                a = Currency.USD;
                break;

        }
        return a;
    }


}
