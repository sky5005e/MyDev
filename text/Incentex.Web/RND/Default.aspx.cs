using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CurrencyConverter;
using Incentex.DA;
using System.Collections.Generic;
using Incentex.BE;


public partial class _Default : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Double a = 50.0;
        //Response.Write(a + "<br/>"); 
        //Response.Write(IncentexGlobal.CurrencyFrom + "<br/>");
        //Response.Write(IncentexGlobal.ConvertionRate + "<br/>");
        //Response.Write(IncentexGlobal.CurrencyTo+ "<br/>");
        //Response.Write(IncentexGlobal.Currency + " " + a * IncentexGlobal.ConvertionRate);

        userinfo u = new userinfo();
        List<UserBE> r = u.GetAllUsers("products");
        grdView.DataSource = r;
        grdView.DataBind();


    }
    //Not In use
    private void BindControls()
    {
        Array itemValue = Enum.GetValues(typeof(Currency));
        Array itemNames = Enum.GetNames(typeof(Currency));
        // this is test
        //itemValue = Enum.GetValues(typeof(Currency));
        //itemNames = Enum.GetNames(typeof(Currency));


        for (int i = 0; i < itemNames.Length - 1; i++)
        {
            if (itemNames.GetValue(i).ToString() == "USD" || itemNames.GetValue(i).ToString() == "EUR" || itemNames.GetValue(i).ToString() == "CAD")
            {
                ListItem item = new ListItem();
                if (itemNames.GetValue(i).ToString() == "USD")
                {
                    item = new ListItem("US Dollar", itemValue.GetValue(i).ToString());
                }
                else if (itemNames.GetValue(i).ToString() == "EUR")
                {
                    item = new ListItem("EU euro", itemValue.GetValue(i).ToString());

                }
                else
                {
                    item = new ListItem("Canadian dollar", itemValue.GetValue(i).ToString());
                }
                //ddlCurrencyFrom.Items.Add(item);
                //ddlCurrencyTo.Items.Add(item);
            }
            /*
            Response.Write("case '" + itemNames.GetValue(i).ToString() + "': <br/>");
            Response.Write("a= Currency." + itemNames.GetValue(i).ToString() + ";");
            Response.Write("<br/>break;<br/>");*/


        }
    }
    //Not In Use
    protected void btnChange_Click(object sender, EventArgs e)
    {
        Double currency;
        CurrencyConverter.CurrencyConvertor myref = new CurrencyConverter.CurrencyConvertor();
        //myref.ConversionRate(Convert.ToDecimal(ddlCurrencyFrom.SelectedItem.Value), ddlCurrencyTo.SelectedItem.Value);
        //double conversionRate = myref.ConversionRate((CurrencyConvertor.Currency)Enum.Parse(typeof(CurrencyConvertor.Currency), "INR"), (CurrencyConvertor.Currency)Enum.Parse(typeof(CurrencyConvertor.Currency), "USD"));
        currency = myref.ConversionRate(converttocurrency(ddlCurrencyFrom.SelectedItem.Value), converttocurrency(ddlCurrencyTo.SelectedItem.Value));
        lblConvertionRate.Text = "1 " + ddlCurrencyFrom.SelectedItem.Text + "= " + "<b>" + currency.ToString() + "</b>" + " " + ddlCurrencyTo.SelectedItem.Text;
        //double t = myref.ConversionRate(Currency.INR, Currency.USD);
    }
    //Not in Use
    public Currency converttocurrency(string value)
    {
        Currency a = new Currency();

        switch (value)
        {
            case "AFA":
                a = Currency.AFA;
                break;
            case "ALL":
                a = Currency.ALL;
                break;
            case "DZD":
                a = Currency.DZD;
                break;
            case "ARS":
                a = Currency.ARS;
                break;
            case "AWG":
                a = Currency.AWG;
                break;
            case "AUD":
                a = Currency.AUD;
                break;
            case "BSD":
                a = Currency.BSD;
                break;
            case "BHD":
                a = Currency.BHD;
                break;
            case "BDT":
                a = Currency.BDT;
                break;
            case "BBD":
                a = Currency.BBD;
                break;
            case "BZD":
                a = Currency.BZD;
                break;
            case "BMD":
                a = Currency.BMD;
                break;
            case "BTN":
                a = Currency.BTN;
                break;
            case "BOB":
                a = Currency.BOB;
                break;
            case "BWP":
                a = Currency.BWP;
                break;
            case "BRL":
                a = Currency.BRL;
                break;
            case "GBP":
                a = Currency.GBP;
                break;
            case "BND":
                a = Currency.BND;
                break;
            case "BIF":
                a = Currency.BIF;
                break;
            case "XOF":
                a = Currency.XOF;
                break;
            case "XAF":
                a = Currency.XAF;
                break;
            case "KHR":
                a = Currency.KHR;
                break;
            case "CAD":
                a = Currency.CAD;
                break;
            case "CVE":
                a = Currency.CVE;
                break;
            case "KYD":
                a = Currency.KYD;
                break;
            case "CLP":
                a = Currency.CLP;
                break;
            case "CNY":
                a = Currency.CNY;
                break;
            case "COP":
                a = Currency.COP;
                break;
            case "KMF":
                a = Currency.KMF;
                break;
            case "CRC":
                a = Currency.CRC;
                break;
            case "HRK":
                a = Currency.HRK;
                break;
            case "CUP":
                a = Currency.CUP;
                break;
            case "CYP":
                a = Currency.CYP;
                break;
            case "CZK":
                a = Currency.CZK;
                break;
            case "DKK":
                a = Currency.DKK;
                break;
            case "DJF":
                a = Currency.DJF;
                break;
            case "DOP":
                a = Currency.DOP;
                break;
            case "XCD":
                a = Currency.XCD;
                break;
            case "EGP":
                a = Currency.EGP;
                break;
            case "SVC":
                a = Currency.SVC;
                break;
            case "EEK":
                a = Currency.EEK;
                break;
            case "ETB":
                a = Currency.ETB;
                break;
            case "EUR":
                a = Currency.EUR;
                break;
            case "FKP":
                a = Currency.FKP;
                break;
            case "GMD":
                a = Currency.GMD;
                break;
            case "GHC":
                a = Currency.GHC;
                break;
            case "GIP":
                a = Currency.GIP;
                break;
            case "XAU":
                a = Currency.XAU;
                break;
            case "GTQ":
                a = Currency.GTQ;
                break;
            case "GNF":
                a = Currency.GNF;
                break;
            case "GYD":
                a = Currency.GYD;
                break;
            case "HTG":
                a = Currency.HTG;
                break;
            case "HNL":
                a = Currency.HNL;
                break;
            case "HKD":
                a = Currency.HKD;
                break;
            case "HUF":
                a = Currency.HUF;
                break;
            case "ISK":
                a = Currency.ISK;
                break;
            case "INR":
                a = Currency.INR;
                break;
            case "IDR":
                a = Currency.IDR;
                break;
            case "IQD":
                a = Currency.IQD;
                break;
            case "ILS":
                a = Currency.ILS;
                break;
            case "JMD":
                a = Currency.JMD;
                break;
            case "JPY":
                a = Currency.JPY;
                break;
            case "JOD":
                a = Currency.JOD;
                break;
            case "KZT":
                a = Currency.KZT;
                break;
            case "KES":
                a = Currency.KES;
                break;
            case "KRW":
                a = Currency.KRW;
                break;
            case "KWD":
                a = Currency.KWD;
                break;
            case "LAK":
                a = Currency.LAK;
                break;
            case "LVL":
                a = Currency.LVL;
                break;
            case "LBP":
                a = Currency.LBP;
                break;
            case "LSL":
                a = Currency.LSL;
                break;
            case "LRD":
                a = Currency.LRD;
                break;
            case "LYD":
                a = Currency.LYD;
                break;
            case "LTL":
                a = Currency.LTL;
                break;
            case "MOP":
                a = Currency.MOP;
                break;
            case "MKD":
                a = Currency.MKD;
                break;
            case "MGF":
                a = Currency.MGF;
                break;
            case "MWK":
                a = Currency.MWK;
                break;
            case "MYR":
                a = Currency.MYR;
                break;
            case "MVR":
                a = Currency.MVR;
                break;
            case "MTL":
                a = Currency.MTL;
                break;
            case "MRO":
                a = Currency.MRO;
                break;
            case "MUR":
                a = Currency.MUR;
                break;
            case "MXN":
                a = Currency.MXN;
                break;
            case "MDL":
                a = Currency.MDL;
                break;
            case "MNT":
                a = Currency.MNT;
                break;
            case "MAD":
                a = Currency.MAD;
                break;
            case "MZM":
                a = Currency.MZM;
                break;
            case "MMK":
                a = Currency.MMK;
                break;
            case "NAD":
                a = Currency.NAD;
                break;
            case "NPR":
                a = Currency.NPR;
                break;
            case "ANG":
                a = Currency.ANG;
                break;
            case "NZD":
                a = Currency.NZD;
                break;
            case "NIO":
                a = Currency.NIO;
                break;
            case "NGN":
                a = Currency.NGN;
                break;
            case "KPW":
                a = Currency.KPW;
                break;
            case "NOK":
                a = Currency.NOK;
                break;
            case "OMR":
                a = Currency.OMR;
                break;
            case "XPF":
                a = Currency.XPF;
                break;
            case "PKR":
                a = Currency.PKR;
                break;
            case "XPD":
                a = Currency.XPD;
                break;
            case "PAB":
                a = Currency.PAB;
                break;
            case "PGK":
                a = Currency.PGK;
                break;
            case "PYG":
                a = Currency.PYG;
                break;
            case "PEN":
                a = Currency.PEN;
                break;
            case "PHP":
                a = Currency.PHP;
                break;
            case "XPT":
                a = Currency.XPT;
                break;
            case "PLN":
                a = Currency.PLN;
                break;
            case "QAR":
                a = Currency.QAR;
                break;
            case "ROL":
                a = Currency.ROL;
                break;
            case "RUB":
                a = Currency.RUB;
                break;
            case "WST":
                a = Currency.WST;
                break;
            case "STD":
                a = Currency.STD;
                break;
            case "SAR":
                a = Currency.SAR;
                break;
            case "SCR":
                a = Currency.SCR;
                break;
            case "SLL":
                a = Currency.SLL;
                break;
            case "XAG":
                a = Currency.XAG;
                break;
            case "SGD":
                a = Currency.SGD;
                break;
            case "SKK":
                a = Currency.SKK;
                break;
            case "SIT":
                a = Currency.SIT;
                break;
            case "SBD":
                a = Currency.SBD;
                break;
            case "SOS":
                a = Currency.SOS;
                break;
            case "ZAR":
                a = Currency.ZAR;
                break;
            case "LKR":
                a = Currency.LKR;
                break;
            case "SHP":
                a = Currency.SHP;
                break;
            case "SDD":
                a = Currency.SDD;
                break;
            case "SRG":
                a = Currency.SRG;
                break;
            case "SZL":
                a = Currency.SZL;
                break;
            case "SEK":
                a = Currency.SEK;
                break;
            case "CHF":
                a = Currency.CHF;
                break;
            case "SYP":
                a = Currency.SYP;
                break;
            case "TWD":
                a = Currency.TWD;
                break;
            case "TZS":
                a = Currency.TZS;
                break;
            case "THB":
                a = Currency.THB;
                break;
            case "TOP":
                a = Currency.TOP;
                break;
            case "TTD":
                a = Currency.TTD;
                break;
            case "TND":
                a = Currency.TND;
                break;
            case "TRL":
                a = Currency.TRL;
                break;
            case "USD":
                a = Currency.USD;
                break;
            case "AED":
                a = Currency.AED;
                break;
            case "UGX":
                a = Currency.UGX;
                break;
            case "UAH":
                a = Currency.UAH;
                break;
            case "UYU":
                a = Currency.UYU;
                break;
            case "VUV":
                a = Currency.VUV;
                break;
            case "VEB":
                a = Currency.VEB;
                break;
            case "VND":
                a = Currency.VND;
                break;
            case "YER":
                a = Currency.YER;
                break;
            case "YUM":
                a = Currency.YUM;
                break;
            case "ZMK":
                a = Currency.ZMK;
                break;
            case "ZWD":
                a = Currency.ZWD;
                break;
        }
        return a;
    }

    /*AFA-Afghanistan Afghani
    ALL-Albanian Lek
    DZD-Algerian Dinar
    ARS-Argentine Peso
    AWG-Aruba Florin
    AUD-Australian Dollar
    BSD-Bahamian Dollar
    BHD-Bahraini Dinar
    BDT-Bangladesh Taka
    BBD-Barbados Dollar
    BZD-Belize Dollar
    BMD-Bermuda Dollar
    BTN-Bhutan Ngultrum
    BOB-Bolivian Boliviano
    BWP-Botswana Pula
    BRL-Brazilian Real
    GBP-British Pound
    BND-Brunei Dollar
    BIF-Burundi Franc
    XOF-CFA Franc (BCEAO)
    XAF-CFA Franc (BEAC)
    KHR-Cambodia Riel
    CAD-Canadian Dollar
    CVE-Cape Verde Escudo
    KYD-Cayman Islands Dollar
    CLP-Chilean Peso
    CNY-Chinese Yuan
    COP-Colombian Peso
    KMF-Comoros Franc
    CRC-Costa Rica Colon
    HRK-Croatian Kuna
    CUP-Cuban Peso
    CYP-Cyprus Pound
    CZK-Czech Koruna
    DKK-Danish Krone
    DJF-Dijibouti Franc
    DOP-Dominican Peso
    XCD-East Caribbean Dollar
    EGP-Egyptian Pound
    SVC-El Salvador Colon
    EEK-Estonian Kroon
    ETB-Ethiopian Birr
    EUR-Euro
    FKP-Falkland Islands Pound
    GMD-Gambian Dalasi
    GHC-Ghanian Cedi
    GIP-Gibraltar Pound
    XAU-Gold Ounces
    GTQ-Guatemala Quetzal
    GNF-Guinea Franc
    GYD-Guyana Dollar
    HTG-Haiti Gourde
    HNL-Honduras Lempira
    HKD-Hong Kong Dollar
    HUF-Hungarian Forint
    ISK-Iceland Krona
    INR-Indian Rupee
    IDR-Indonesian Rupiah
    IQD-Iraqi Dinar
    ILS-Israeli Shekel
    JMD-Jamaican Dollar
    JPY-Japanese Yen
    JOD-Jordanian Dinar
    KZT-Kazakhstan Tenge
    KES-Kenyan Shilling
    KRW-Korean Won
    KWD-Kuwaiti Dinar
    LAK-Lao Kip
    LVL-Latvian Lat
    LBP-Lebanese Pound
    LSL-Lesotho Loti
    LRD-Liberian Dollar
    LYD-Libyan Dinar
    LTL-Lithuanian Lita
    MOP-Macau Pataca
    MKD-Macedonian Denar
    MGF-Malagasy Franc
    MWK-Malawi Kwacha
    MYR-Malaysian Ringgit
    MVR-Maldives Rufiyaa
    MTL-Maltese Lira
    MRO-Mauritania Ougulya
    MUR-Mauritius Rupee
    MXN-Mexican Peso
    MDL-Moldovan Leu
    MNT-Mongolian Tugrik
    MAD-Moroccan Dirham
    MZM-Mozambique Metical
    MMK-Myanmar Kyat
    NAD-Namibian Dollar
    NPR-Nepalese Rupee
    ANG-Neth Antilles Guilder
    NZD-New Zealand Dollar
    NIO-Nicaragua Cordoba
    NGN-Nigerian Naira
    KPW-North Korean Won
    NOK-Norwegian Krone
    OMR-Omani Rial
    XPF-Pacific Franc
    PKR-Pakistani Rupee
    XPD-Palladium Ounces
    PAB-Panama Balboa
    PGK-Papua New Guinea Kina
    PYG-Paraguayan Guarani
    PEN-Peruvian Nuevo Sol
    PHP-Philippine Peso
    XPT-Platinum Ounces
    PLN-Polish Zloty
    QAR-Qatar Rial
    ROL-Romanian Leu
    RUB-Russian Rouble
    WST-Samoa Tala
    STD-Sao Tome Dobra
    SAR-Saudi Arabian Riyal
    SCR-Seychelles Rupee
    SLL-Sierra Leone Leone
    XAG-Silver Ounces
    SGD-Singapore Dollar
    SKK-Slovak Koruna
    SIT-Slovenian Tolar
    SBD-Solomon Islands Dollar
    SOS-Somali Shilling
    ZAR-South African Rand
    LKR-Sri Lanka Rupee
    SHP-St Helena Pound
    SDD-Sudanese Dinar
    SRG-Surinam Guilder
    SZL-Swaziland Lilageni
    SEK-Swedish Krona
    TRY-Turkey Lira
    CHF-Swiss Franc
    SYP-Syrian Pound
    TWD-Taiwan Dollar
    TZS-Tanzanian Shilling
    THB-Thai Baht
    TOP-Tonga Pa'anga
    TTD-Trinidad&amp;Tobago Dollar
    TND-Tunisian Dinar
    TRL-Turkish Lira
    USD-U.S. Dollar
    AED-UAE Dirham
    UGX-Ugandan Shilling
    UAH-Ukraine Hryvnia
    UYU-Uruguayan New Peso
    VUV-Vanuatu Vatu
    VEB-Venezuelan Bolivar
    VND-Vietnam Dong
    YER-Yemen Riyal
    YUM-Yugoslav Dinar
    ZMK-Zambian Kwacha
    ZWD-Zimbabwe Dollar*/

    protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Double a = 50.0;
            //Response.Write(a + "<br/>");
            //Response.Write(IncentexGlobal.CurrencyFrom + "<br/>");
            //Response.Write(IncentexGlobal.ConvertionRate + "<br/>");
            //Response.Write(IncentexGlobal.CurrencyTo+ "<br/>");
            //Response.Write(IncentexGlobal.Currency + " " + a * IncentexGlobal.ConvertionRate);

            double prize = Convert.ToDouble(((Label)e.Row.FindControl("lblProdPrize")).Text);
            ((Label)e.Row.FindControl("lblProdPrize")).Text = IncentexGlobal.Currency + (prize * IncentexGlobal.ConvertionRate).ToString();

        }
    }
}
