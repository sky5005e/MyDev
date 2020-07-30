using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Incentex.DAL;

/// <summary>
/// Summary description for IncentexGlobal
/// </summary>
public class IncentexGlobal
{
    public IncentexGlobal()
    {
        // TODO: Add constructor logic here
        //
    }

    public static UserInformation CurrentMember
    {
        get
        {
            if ((HttpContext.Current.Session["CurrentUser"]) == null)
                return null;
            else
                return (UserInformation)HttpContext.Current.Session["CurrentUser"];
        }
        set
        {
            HttpContext.Current.Session["CurrentUser"] = value;
        }
    }

    /// <summary>
    /// ManageID Set the Session when 
    /// Click on Mange Company
    /// Click on Mange Supplier
    /// Click on Manage Incentex Employee
    /// </summary>
    public static int ManageID
    {
        get
        {
            if ((HttpContext.Current.Session["ManageID"]) == null)
                return 0;
            else
                return Convert.ToInt32(HttpContext.Current.Session["ManageID"].ToString());
        }
        set
        {
            HttpContext.Current.Session["ManageID"] = value;
        }
    }

    public static List<INC_MenuPrivilege> IncentexEmployeeMenuRights
    {
        get
        {
            if ((HttpContext.Current.Session["MenuAccess"]) == null)
                return null;
            else
                return ((List<INC_MenuPrivilege>)HttpContext.Current.Session["MenuAccess"]);
        }
        set
        {
            HttpContext.Current.Session["MenuAccess"] = value;
        }
    }

    public static List<INC_DataPrivilege> IncentexEmployeeDataRestriction
    {
        get
        {
            if ((HttpContext.Current.Session["DataRestriction"]) == null)
                return null;
            else
                return ((List<INC_DataPrivilege>)HttpContext.Current.Session["DataRestriction"]);
        }
        set
        {
            HttpContext.Current.Session["MenuAccess"] = value;
        }
    }

    public static string UserType
    {
        get;
        set;
    }
    public static string CurrencyFrom
    {
        get;
        set;
    }
    public static string CurrencyTo
    {
        get;
        set;
    }

    public static double ConvertionRate
    {
        get;
        set;
    }
    public static string Currency
    {
        get;
        set;
    }

    public static string dropdownIconPath
    {
        get
        {
            return HttpContext.Current.Server.MapPath("../admin/Incentex_Used_Icons/");
        }
    }
    public static string documentpath
    {
        get
        {
            return HttpContext.Current.Server.MapPath("../../UploadedImages/CompanyDocument/");
        }
    }

    public static string companystoredocuments
    {
        get
        {
            return HttpContext.Current.Server.MapPath("../../UploadedImages/CompanyStoreDocuments/");
        }
    }

    public static string StoreProductImagepath
    {
        get
        {
            return HttpContext.Current.Server.MapPath("../../../UploadedImages/ProductImages/");
        }
    }

    public static string StoreProductThumbImagepath
    {
        get
        {
            return HttpContext.Current.Server.MapPath("../../../UploadedImages/ProductImages/Thumbs/");
        }
    }

    public static string StoreProductTailoringMeasurement
    {
        get
        {
            return HttpContext.Current.Server.MapPath("../../../UploadedImages/TailoringMeasurement/");
        }
    }

    public static string OrderPurchaseInvoice
    {
        get
        {
            return HttpContext.Current.Server.MapPath("../UploadedImages/TailoringMeasurement/");
        }
    }

    public static bool IsIEFromStore
    {
        get
        {
            if ((HttpContext.Current.Session["IsIEFromStore"]) == null)
                return false;
            else
                return Convert.ToBoolean(HttpContext.Current.Session["IsIEFromStore"].ToString());
        }
        set
        {
            HttpContext.Current.Session["IsIEFromStore"] = value;
        }
    }

    public static bool IsIEFromStoreTestMode
    {
        get
        {
            if ((HttpContext.Current.Session["IsIEFromStoreTestMode"]) == null)
                return false;
            else
                return Convert.ToBoolean(HttpContext.Current.Session["IsIEFromStoreTestMode"].ToString());
        }
        set
        {
            HttpContext.Current.Session["IsIEFromStoreTestMode"] = value;
        }
    }

    public static bool IsPlaceExchangeOrder
    {
        get
        {
            if ((HttpContext.Current.Session["IsPlaceExchangeOrder"]) == null)
                return false;
            else
                return Convert.ToBoolean(HttpContext.Current.Session["IsPlaceExchangeOrder"]);
        }
        set
        {
            HttpContext.Current.Session["IsPlaceExchangeOrder"] = value;
        }
    }

    public static UserInformation AdminUser
    {
        get
        {
            if ((HttpContext.Current.Session["AdminUser"]) == null)
                return null;
            else
                return (UserInformation)HttpContext.Current.Session["AdminUser"];
        }
        set
        {
            HttpContext.Current.Session["AdminUser"] = value;
        }
    }

    public static string IndexPageLink
    {
        get
        {
            if ((HttpContext.Current.Session["IndexPageLink"]) == null)
                return null;
            else
                return Convert.ToString(HttpContext.Current.Session["IndexPageLink"].ToString());
        }
        set
        {
            HttpContext.Current.Session["IndexPageLink"] = value;
        }
    }

    public static string Paypal_HOST
    {
        get
        {
            return ConfigurationManager.AppSettings["Paypal_HOST"];
        }
    }

    public static string Paypal_HOST_API
    {
        get
        {
            return ConfigurationManager.AppSettings["Paypal_HOST_API"];
        }
    }

    public static PaymentInfo PaymentDetails
    {
        get
        {
            if ((HttpContext.Current.Session["PaymentDetails"]) == null)
                return null;
            else
                return (PaymentInfo)HttpContext.Current.Session["PaymentDetails"];
        }
        set
        {
            HttpContext.Current.Session["PaymentDetails"] = value;
        }
    }

    public static string OrderReturnURL
    {
        get
        {
            if ((HttpContext.Current.Session["OrderReturnURL"]) == null)
                return null;
            else
                return Convert.ToString(HttpContext.Current.Session["OrderReturnURL"].ToString());
        }
        set
        {
            HttpContext.Current.Session["OrderReturnURL"] = value;
        }
    }

    public static string ProductReturnURL
    {
        get
        {
            if ((HttpContext.Current.Session["ProductReturnURL"]) == null)
                return null;
            else
                return Convert.ToString(HttpContext.Current.Session["ProductReturnURL"].ToString());
        }
        set
        {
            HttpContext.Current.Session["ProductReturnURL"] = value;
        }
    }

    public static string SearchTicketURL
    {
        get { return Convert.ToString(HttpContext.Current.Session["SearchTicketURL"]); }
        set { HttpContext.Current.Session["SearchTicketURL"] = value; }
    }

    public static string UnAuthorisedURL
    {
        get
        {
            return Convert.ToString(HttpContext.Current.Session["UnAuthorisedURL"]);
        }
        set
        {
            HttpContext.Current.Session["UnAuthorisedURL"] = value;
        }
    }

    public static string SearchPageReturnURL
    {
        get
        {
            if ((HttpContext.Current.Session["SearchPageReturnURL"]) == null)
                return null;
            else
                return Convert.ToString(HttpContext.Current.Session["SearchPageReturnURL"].ToString());
        }
        set
        {
            HttpContext.Current.Session["SearchPageReturnURL"] = value;
        }
    }

    public static string SearchPageReturnURLFrontSide
    {
        get
        {
            if ((HttpContext.Current.Session["SearchPageReturnURLFrontSide"]) == null)
                return null;
            else
                return Convert.ToString(HttpContext.Current.Session["SearchPageReturnURLFrontSide"].ToString());
        }
        set
        {
            HttpContext.Current.Session["SearchPageReturnURLFrontSide"] = value;
        }
    }

    public static string ArtImagrReturnURL
    {
        get
        {
            if ((HttpContext.Current.Session["ArtImagrReturnURL"]) == null)
                return null;
            else
                return Convert.ToString(HttpContext.Current.Session["ArtImagrReturnURL"].ToString());
        }
        set
        {
            HttpContext.Current.Session["ArtImagrReturnURL"] = value;
        }
    }

    /// <summary>
    /// Added by prashant on 29th March 2013 
    /// </summary>
    public static List<GSEUserDetails> GSEMgtUserDetails
    {
        get
        {
            if ((HttpContext.Current.Session["GSEUserDetails"]) == null)
                return null;
            else
                return ((List<GSEUserDetails>)HttpContext.Current.Session["GSEUserDetails"]);
        }
        set
        {
            HttpContext.Current.Session["GSEUserDetails"] = value;
        }
    }

    public static GSEUserDetails GSEMgtCurrentMember
    {
        get
        {
            if ((HttpContext.Current.Session["GSEMgtCurrentMember"]) == null)
                return null;
            else
                return ((GSEUserDetails)HttpContext.Current.Session["GSEMgtCurrentMember"]);
        }
        set
        {
            HttpContext.Current.Session["GSEMgtCurrentMember"] = value;
        }
    }
}