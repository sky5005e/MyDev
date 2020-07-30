/*
 * I have used Repeater for the Multiple Radiobutton as there is custom layout to use Radiobutton.
 * Kindly take a note of it and you can change it to Radiobutton if you have alternate solution to bind wrap the label and span with each radiobutton
 */

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
using System.Collections.Generic;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using ASP;

using System.Globalization;


public partial class Admin_CreateCampaign : PageBase
{

    #region Page Variables and Properties

    enum GridID
    {
        Invoice = 1,
        Warranty = 2,
        Claim = 3
    }

    Int64 EquipmentMasterID
    {
        get
        {
            return Convert.ToInt64(ViewState["EquipmentMasterID"]);
        }
        set
        {
            ViewState["EquipmentMasterID"] = value;
        }
    }

    Int64 CompanyID
    {
        get
        {
            return Convert.ToInt64(ViewState["CompanyID"]);
        }
        set
        {
            ViewState["CompanyID"] = value;
        }
    }

    Int64 EquipmentTypeID
    {
        get
        {
            return Convert.ToInt64(ViewState["EquipmentTypeID"]);
        }
        set
        {
            ViewState["EquipmentTypeID"] = value;
        }
    }

    Boolean IsFlagAsset
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsFlagAsset"]);
        }
        set
        {
            ViewState["IsFlagAsset"] = value;
        }
    }

    Decimal InvoiceTotalAmount
    {
        get
        {
            return Convert.ToDecimal(ViewState["InvoiceTotalAmount"]);
        }
        set
        {
            ViewState["InvoiceTotalAmount"] = value;
        }
    }

    public String InvoiceSortExp
    {
        get
        {
            return Convert.ToString(Session["InvoiceSortExp"]);
        }
        set
        {
            Session["InvoiceSortExp"] = value;
        }
    }

    public String InvoiceSortOrder
    {
        get
        {
            return Convert.ToString(Session["InvoiceSortOrder"]);
        }
        set
        {
            Session["InvoiceSortOrder"] = value;
        }
    }

    public String InvoicePagingStatus
    {
        get
        {
            if (Session["InvoicePagingStatus"] == null)
                return "VIEW ALL";
            return Convert.ToString(Session["InvoicePagingStatus"]);
        }
        set
        {
            Session["InvoicePagingStatus"] = value;
        }
    }

    public String WarrantyPagingStatus
    {
        get
        {
            if (Session["WarrantyPagingStatus"] == null)
                return "VIEW ALL";
            return Convert.ToString(Session["WarrantyPagingStatus"]);
        }
        set
        {
            Session["WarrantyPagingStatus"] = value;
        }
    }

    public String ClaimPagingStatus
    {
        get
        {
            if (Session["ClaimPagingStatus"] == null)
                return "VIEW ALL";
            return Convert.ToString(Session["ClaimPagingStatus"]);
        }
        set
        {
            Session["ClaimPagingStatus"] = value;
        }
    }

    string CurrModule = "Asset Management";
    string CurrSubMenu = "Assets";

    #endregion

    #region Event Handlers

    protected void lnkbtnDownload_Click(object sender, EventArgs e)
    {
        LinkButton lnkbtn = (LinkButton)sender;
        String filepath = Server.MapPath("~/" + lnkbtn.CommandArgument);
        String strFileName = "MyFile";
        if (!String.IsNullOrEmpty(lnkbtn.CommandName))
            strFileName = lnkbtn.CommandName;
        Common.DownloadFile(filepath, strFileName);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {                        

        }       
    }

    /// <summary>
    /// Handles the Click event when the Tab is click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>        

    protected void lbHelpVideo_Click(object sender, EventArgs e)
    {
        string strVideoURL = Common.GetMyHelpVideo("Help Video", "Asset Management", "Add New Field");
        if (strVideoURL != "")
        {
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Convert.ToString(Guid.NewGuid()), "ShowVideo('" + strVideoURL + "');", true);
        }
    }

    #endregion

    #region Methods
   

    /// <summary>
    /// Bind dropdown and add onchange attribute
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ddl"></param>
    /// <param name="list"></param>
    /// <param name="DataTextField"></param>
    /// <param name="DataValueField"></param>
    /// <param name="FirstListItem"></param>    

    private void BindBasicDropDown()
    {
        try
        {
            LookupRepository objLookupRepository = new LookupRepository();
            List<INC_Lookup> objList = new List<INC_Lookup>();

            //Bind Equipment Types

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Function to remove the Active class from each of the Tab
    /// </summary>    
    #endregion

    #region Basic Tab

    protected void btnBasicSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    

    

    

    

    
}

