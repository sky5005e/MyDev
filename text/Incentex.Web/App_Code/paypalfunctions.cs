using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;

/// <summary>
/// Summary description for NVPAPICaller
/// </summary>
public class NVPAPICaller
{
    //private static readonly ILog log = LogManager.GetLogger(typeof(NVPAPICaller));

    private string pendpointurl = "https://payflowpro.paypal.com";

    private const string VENDOR = "VENDOR";
    private const string PARTNER = "PARTNER";
    private const string PWD = "PWD";

    private string APIUser = "knelson";
    //Fill in the APIPassword variable yourself, the wizard will not do this automatically
    private string APIPassword = "";
    private string APIVendor = "knelson";
    private string APIPartner = "paypal";
    //Flag that determines the Payflow environment (live or pilot)
    private string Env = "pilot";
	private string BNCode = "PF-CCWizard";

    //HttpWebRequest Timeout specified in milliseconds 
    private const int Timeout = 90000;
    private static readonly string[] SECURED_NVPS = new string[] { VENDOR, PARTNER, PWD };


    /// <summary>
    /// Sets the Payflow Credentials
    /// </summary>
    /// <param name="Userid"></param>
    /// <param name="Pwd"></param>
    /// <param name="Signature"></param>
    /// <returns></returns>
    public void SetCredentials(string User, string Pwd, string Vendor, string Partner)
    {
        APIUser = User;
        APIPassword = Pwd;
        APIVendor = Vendor;
		APIPartner = Partner;
    }

    /// <summary>
    /// ShortcutExpressCheckout: The shortcut implementation of SetExpressCheckout
    /// </summary>
    /// <param name="amt"></param>
    /// <param ref name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool ShortcutExpressCheckout(string amt, ref string token, ref string retMsg)
    {
		string host = "www.paypal.com";
		if (Env == "pilot")
		{
			pendpointurl = "https://pilot-payflowpro.paypal.com";
			host = "www.sandbox.paypal.com";
		}
		
		string returnURL = "http://ind115.cfmdeveloper.com/default.aspx";
        string cancelURL = "http://ind115.cfmdeveloper.com/default.aspx";

        NVPCodec encoder = new NVPCodec();

		encoder["TENDER"] = "P";
        encoder["ACTION"] = "S";
		if ("Authorization" == "Sale")
		{
			encoder["TRXTYPE"] = "A";
		}
		else /* sale */
		{
			encoder["TRXTYPE"] = "S";
		}
        encoder["RETURNURL"] = returnURL;
        encoder["CANCELURL"] = cancelURL;
        encoder["AMT"] = amt;
        encoder["CURRENCY"] = "USD";

		// unique request ID
		System.Guid uid = System.Guid.NewGuid();

        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp,uid.ToString());

        NVPCodec decoder = new NVPCodec();
        decoder.Decode(pStresponsenvp);

        string strAck = decoder["RESULT"].ToLower();
        if (strAck != null && strAck == "0")
        {
            token = decoder["TOKEN"];

            string ECURL = "https://" + host + "/cgi-bin/webscr?cmd=_express-checkout&" + "token=" + token;

            retMsg = ECURL;
            return true;
        }
        else
        {
            retMsg = "ErrorCode=" + strAck + "&" +
                "Desc=" + decoder["RESPMSG"];

            return false;
        }
    }

    /// <summary>
    /// MarkExpressCheckout: The method that calls SetExpressCheckout, invoked from the 
    /// Billing Page EC placement
    /// </summary>
    /// <param name="amt"></param>
    /// <param ref name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool MarkExpressCheckout(string amt, 
                        string shipToName, string shipToStreet, string shipToStreet2,
                        string shipToCity, string shipToState, string shipToZip, 
                        string shipToCountryCode, ref string token, ref string retMsg)
    {

		string host = "www.paypal.com";
		if (Env == "pilot")
		{
			pendpointurl = "https://pilot-payflowpro.paypal.com";
			host = "www.sandbox.paypal.com";
		}

		string returnURL = "http://ind115.cfmdeveloper.com/default.aspx";
        string cancelURL = "http://ind115.cfmdeveloper.com/default.aspx";

        NVPCodec encoder = new NVPCodec();
		encoder["TENDER"] = "P";
		encoder["ACTION"] = "S";
		if ("Authorization" == "Sale")
		{
			encoder["TRXTYPE"] = "A";
		}
		else /* sale */
		{
			encoder["TRXTYPE"] = "S";
		}
        encoder["RETURNURL"] = returnURL;
        encoder["CANCELURL"] = cancelURL;
        encoder["AMT"] = amt;
        encoder["CURRENCY"]  = "USD";

        //Optional Shipping Address entered on the merchant site
        encoder["SHIPTOSTREET"]     = shipToStreet;
        encoder["SHIPTOSTREET2"]    = shipToStreet2;
        encoder["SHIPTOCITY"]       = shipToCity;
        encoder["SHIPTOSTATE"]      = shipToState;
        encoder["SHIPTOZIP"]        = shipToZip;
        encoder["SHIPTOCOUNTRY"]	= shipToCountryCode;
        encoder["ADDROVERRIDE"]		= "1";
	
		// unique request ID
		System.Guid uid = System.Guid.NewGuid();

        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp,uid.ToString());

        NVPCodec decoder = new NVPCodec();
        decoder.Decode(pStresponsenvp);

        string strAck = decoder["RESULT"].ToLower();
        if (strAck != null && strAck == "0")
        {
            token = decoder["TOKEN"];

            string ECURL = "https://" + host + "/cgi-bin/webscr?cmd=_express-checkout&" + "token=" + token;

            retMsg = ECURL;
            return true;
        }
        else
        {
			retMsg = "ErrorCode=" + strAck + "&" +
				"Desc=" + decoder["RESPMSG"];

			return false;
        }
    }



    /// <summary>
    /// GetShippingDetails: The method that calls GetExpressCheckoutDetails
    /// </summary>
    /// <param name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool GetShippingDetails(string token, ref string PayerId, ref string ShippingAddress, ref string retMsg)
    {
		if (Env == "pilot")
		{
			pendpointurl = "https://pilot-payflowpro.paypal.com";
		}

        NVPCodec encoder = new NVPCodec();
        encoder["TOKEN"] = token;
		encoder["TENDER"] = "P";
		encoder["ACTION"] = "G";
		if ("Authorization" == "Sale")
		{
			encoder["TRXTYPE"] = "A";
		}
		else /* sale */
		{
			encoder["TRXTYPE"] = "S";
		}

		// unique request ID
		System.Guid uid = System.Guid.NewGuid();

        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall( pStrrequestforNvp,uid.ToString() );

        NVPCodec decoder = new NVPCodec();
        decoder.Decode( pStresponsenvp );

        string strAck = decoder["RESULT"].ToLower();
        if (strAck != null && strAck == "0")
        {
            ShippingAddress = "<table><tr>";
            ShippingAddress += "<td colspan='2'> Shipping Address</td></tr>";
            ShippingAddress += "<td> Street1 </td><td>" + decoder["SHIPTOSTREET"] + "</td></tr>";
            ShippingAddress += "<td> Street2 </td><td>" + decoder["SHIPTOSTREET2"] + "</td></tr>";
            ShippingAddress += "<td> City </td><td>" + decoder["SHIPTOCITY"] + "</td></tr>";
            ShippingAddress += "<td> State </td><td>" + decoder["SHIPTOSTATE"] + "</td></tr>";
            ShippingAddress += "<td> Zip </td><td>" + decoder["SHIPTOZIP"] + "</td>";
            ShippingAddress += "</tr>";

            return true;
        }
        else
        {
			retMsg = "ErrorCode=" + strAck + "&" +
				"Desc=" + decoder["RESPMSG"];

			return false;
        }
    }


    /// <summary>
    /// ConfirmPayment: The method that calls DoExpressCheckoutPayment, invoked from the 
    /// Billing Page EC placement
    /// </summary>
    /// <param name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool ConfirmPayment(string finalPaymentAmount, string token, string PayerId, ref NVPCodec decoder, ref string retMsg )
    {
		if (Env == "pilot")
		{
			pendpointurl = "https://pilot-payflowpro.paypal.com";
		}

        NVPCodec encoder = new NVPCodec();

        encoder["TOKEN"] = token;
		encoder["TENDER"] = "P";
		encoder["ACTION"] = "D";
		if ("Authorization" == "Sale")
		{
			encoder["TRXTYPE"] = "A";
		}
		else /* sale */
		{
			encoder["TRXTYPE"] = "S";
		}
        encoder["PAYERID"] = PayerId;
        encoder["AMT"] = finalPaymentAmount;

		// unique request ID
		string unique_id ="1";

        //if (Session["unique_id"] == null)
        //{
        //    System.Guid uid = System.Guid.NewGuid();
        //    unique_id = uid.ToString();
        //    Session["unique_id"] = unique_id;
        //}
        //else
        //{
        //    unique_id = (string)Session["unique_id"];
        //}

        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp,unique_id);

        decoder = new NVPCodec();
        decoder.Decode(pStresponsenvp);

        string strAck = decoder["RESULT"].ToLower();
        if (strAck != null && strAck == "0")
        {
            return true;
        }
        else
        {
			retMsg = "ErrorCode=" + strAck + "&" +
				"Desc=" + decoder["RESPMSG"];

			return false;
        }
    }


	/// <summary>
	/// DirectPayment: The method for credit card payment
	/// </summary>
	/// Note:
	///	There are other optional inputs for credit card processing that are not presented here.
	///		For a complete list of inputs available, please see the documentation here for US and UK:
    ///		https://cms.paypal.com/cms_content/US/en_US/files/developer/PP_PayflowPro_Guide.pdf
    ///		https://cms.paypal.com/cms_content/GB/en_GB/files/developer/PP_WebsitePaymentsPro_IntegrationGuide_UK.pdf
	/// <param name="amt"></param>
	/// <param ref name="token"></param>
	/// <param ref name="retMsg"></param>
	/// <returns></returns>
	public bool DirectPayment(string paymentType, string paymentAmount, string creditCardType, string creditCardNumber, string expDate, string cvv2, string firstName, string lastName, string street, string city, string state, string zip, string countryCode, string currencyCode, string orderdescription, ref NVPCodec decoder, ref string retMsg)
	{
		if (Env == "pilot")
		{
			pendpointurl = "https://pilot-payflowpro.paypal.com";
		}

		NVPCodec encoder = new NVPCodec();
		encoder["TENDER"] = "C";
		if ("Authorization" == "Sale")
		{
			encoder["TRXTYPE"] = "A";
		}
		else /* sale */
		{
			encoder["TRXTYPE"] = "S";
		}
		encoder["ACCT"]				= creditCardNumber;
		encoder["CVV2"]				= cvv2;
		encoder["EXPDATE"]			= expDate;
		encoder["ACCTTYPE"]			= creditCardType;
		encoder["AMT"]				= paymentAmount;
		encoder["CURRENCY"]			= currencyCode;
		encoder["FIRSTNAME"]		= firstName;
		encoder["LASTNAME"]			= lastName;
		encoder["STREET"]			= street;
		encoder["CITY"]				= city;
		encoder["STATE"]			= state;
		encoder["ZIP"]				= zip;
		encoder["COUNTRY"]			= countryCode;
		// unique request ID
		System.Guid uid = System.Guid.NewGuid();
		encoder["INVNUM"]			= uid.ToString();
		encoder["ORDERDESC"]		= orderdescription;
		encoder["VERBOSITY"]		= "MEDIUM";

		string pStrrequestforNvp = encoder.Encode();
		string pStresponsenvp = HttpCall(pStrrequestforNvp,uid.ToString());

		decoder = new NVPCodec();
		decoder.Decode(pStresponsenvp);

		string strAck = decoder["RESULT"].ToLower();
		if (strAck != null && strAck == "0")
		{
			return true;
		}
		else
		{
			retMsg = "ErrorCode=" + strAck + "&" +
				"Desc=" + decoder["RESPMSG"];

			return false;
		}
	}


    /// <summary>
    /// HttpCall: The main method that is used for all API calls
    /// </summary>
    /// <param name="NvpRequest"></param>
    /// <returns></returns>
    public string HttpCall(string NvpRequest,string unique_id) //CallNvpServer
    {
        string url = pendpointurl;

        //To Add the credentials from the profile
        string strPost = NvpRequest + "&" + buildCredentialsNVPString();
		strPost = strPost + "&BUTTONSOURCE=" + BNCode;

        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
        objRequest.Timeout = Timeout;
        objRequest.Method = "POST";
        objRequest.ContentLength = strPost.Length;
		objRequest.ContentType = "text/namevalue";
		objRequest.Headers.Add("X-VPS-CLIENT-TIMEOUT","45");
		objRequest.Headers.Add("X-VPS-REQUEST-ID",unique_id);
        if (Env == "pilot")
		{
			objRequest.Headers.Add("Host","pilot-payflowpro.paypal.com");
		}
		else
		{
			objRequest.Headers.Add("Host","payflowpro.paypal.com");
		}

        try
        {
            using (StreamWriter myWriter = new StreamWriter(objRequest.GetRequestStream()))
            {
                myWriter.Write(strPost);
            }
        }
        catch (Exception e)
        {
            /*
            if (log.IsFatalEnabled)
            {
                log.Fatal(e.Message, this);
            }*/
        }

        //Retrieve the Response returned from the NVP API call to PayPal
        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        string result;
        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        {
            result = sr.ReadToEnd();
        }

        //Logging the response of the transaction
        /* if (log.IsInfoEnabled)
         {
             log.Info("Result :" +
                       " Elapsed Time : " + (DateTime.Now - startDate).Milliseconds + " ms" +
                      result);
         }
         */
        return result;
    }

    /// <summary>
    /// Credentials added to the NVP string
    /// </summary>
    /// <param name="profile"></param>
    /// <returns></returns>
    private string buildCredentialsNVPString()
    {
        NVPCodec codec = new NVPCodec();

        if (!IsEmpty(APIUser))
            codec["USER"] = APIUser;

        if (!IsEmpty(APIPassword))
            codec[PWD] = APIPassword;

        if (!IsEmpty(APIVendor))
            codec[VENDOR] = APIVendor;

		if (!IsEmpty(APIPartner))
			codec[PARTNER] = APIPartner;

        return codec.Encode();
    }

    /// <summary>
    /// Returns if a string is empty or null
    /// </summary>
    /// <param name="s">the string</param>
    /// <returns>true if the string is not null and is not empty or just whitespace</returns>
    public static bool IsEmpty(string s)
    {
        return s == null || s.Trim() == string.Empty;
    }
}


public sealed class NVPCodec : NameValueCollection
{
    private const string AMPERSAND = "&";
    private const string EQUALS = "=";
    private static readonly char[] AMPERSAND_CHAR_ARRAY = AMPERSAND.ToCharArray();
    private static readonly char[] EQUALS_CHAR_ARRAY = EQUALS.ToCharArray();

    /// <summary>
    /// Returns the built NVP string of all name/value pairs in the Hashtable
    /// </summary>
    /// <returns></returns>
    public string Encode()
    {
        StringBuilder sb = new StringBuilder();
        bool firstPair = true;
        foreach (string kv in AllKeys)
        {
            string name = kv;
            string val = this[kv];
            if (!firstPair)
            {
                sb.Append(AMPERSAND);
            }
            sb.Append(name).Append(EQUALS).Append(val);
            firstPair = false;
        }
        return sb.ToString();
    }

    /// <summary>
    /// Decoding the string
    /// </summary>
    /// <param name="nvpstring"></param>
    public void Decode(string nvpstring)
    {
        Clear();
        foreach (string nvp in nvpstring.Split(AMPERSAND_CHAR_ARRAY))
        {
            string[] tokens = nvp.Split(EQUALS_CHAR_ARRAY);
            if (tokens.Length >= 2)
            {
                string name = tokens[0];
                string val = tokens[1];
                Add(name, val);
            }
        }
    }

    private static string UrlDecode(string s) { return HttpUtility.UrlDecode(s); }
    private static string UrlEncode(string s) { return HttpUtility.UrlEncode(s); }

    #region Array methods
    public void Add(string name, string val, int index)
    {
        this.Add(GetArrayName(index, name), val);
    }

    public void Remove(string arrayName, int index)
    {
        this.Remove(GetArrayName(index, arrayName));
    }

    /// <summary>
    /// 
    /// </summary>
    public string this[string name, int index]
    {
        get
        {
            return this[GetArrayName(index, name)];
        }
        set
        {
            this[GetArrayName(index, name)] = name;
        }
    }

    private static string GetArrayName(int index, string name)
    {
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException("index", "index can not be negative : " + index);
        }
        return name + index;
    }
    #endregion
}