using System;
using System.Web.UI;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL;
using System.Web;

/// <summary>
/// Summary description for PageBase
/// </summary>
public class PageBase : Page
{
    System.Diagnostics.Stopwatch mysw;
    public PageBase()
    {
        //
        // TODO: Add constructor logic here
        mysw = new System.Diagnostics.Stopwatch();
        //
    }

    protected override void OnLoad(EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack)
        {
            //SetIsErrorOccur(false);
            // This is only for mouse flow.
            //Page.ClientScript.RegisterClientScriptInclude("MouseFlowPathScript", ResolveUrl("~/js/MouseFlow.js"));
        }
        base.OnLoad(e);
    }
    public String MenuItem
    {
        get
        {
            return Convert.ToString(ViewState["MenuItem"]);
        }

        set
        {
            ViewState["MenuItem"] = value;
        }
    }

    public Int64? ParentMenuID
    {
        get
        {
            return Convert.ToInt64(ViewState["ParentMenuID"]);
        }

        set
        {
            ViewState["ParentMenuID"] = value;
        }
    }
    /// <summary>
    /// This will true when there will some error occur in our system
    /// </summary>
    public static Boolean IsError
    {

        get
        {   
            return Convert.ToBoolean(HttpContext.Current.Session["IsError"]);
        }
        set
        {
            HttpContext.Current.Session["IsError"] = value;
        }

    }
    /// <summary>
    /// This will set true to IsError properity when exception occur in WL site.
    /// </summary>
    /// <param name="IsException"></param>
    public static void SetIsErrorOccur(Boolean IsException)
    {
        if (IsException)
            IsError = true;
        else
            IsError = false;
    }
    protected void SetTopLinkScript(String id)
    {
        //1: WHAT's NEW -"whats-link" 
        //2: ADMIN -"admin-link" 
        //3: ORDER -"shop-link"
        //4: RESOURCES-"video-link" 
        //5: BLOG-"blog-link" 
        //6: CONTACT-"contact-link" 
        this.ClientScript.RegisterClientScriptBlock(this.GetType(),Guid.NewGuid().ToString(),"$(document).ready(function() { $('.containerNavigation ul li#" + id + "').addClass('active'); });",true);
    }
    public Boolean CanAdd
    {
        get
        {
            return Convert.ToBoolean(ViewState["CanAdd"]);
        }

        set
        {
            ViewState["CanAdd"] = value;
        }
    }

    public Boolean CanView
    {
        get
        {
            return Convert.ToBoolean(ViewState["CanView"]);
        }

        set
        {
            ViewState["CanView"] = value;
        }
    }

    public Boolean CanEdit
    {
        get
        {
            return Convert.ToBoolean(ViewState["CanEdit"]);
        }

        set
        {
            ViewState["CanEdit"] = value;
        }
    }

    public Boolean CanDelete
    {
        get
        {
            return Convert.ToBoolean(ViewState["CanDelete"]);
        }

        set
        {
            ViewState["CanDelete"] = value;
        }
    }

    /// <summary>
    /// Check is CurrentMemeber Session which has been assigned is null or not.
    /// If it is null then re-direct it to the login page.
    /// </summary>
    protected void CheckLogin()
    {
        if (IncentexGlobal.CurrentMember == null)
        {
            Response.Redirect("~/login.aspx");
        }
    }

    protected void CheckAccessRights(Int64? UserInfoID, String MenuItem, Int64? ParentMenuID)
    {
        if (!String.IsNullOrEmpty(MenuItem) && ParentMenuID != null)
        {
            AccessRightRepository objRightRepo = new AccessRightRepository();
            List<GetAccessRightsMenuWiseResult> lstRights = objRightRepo.CheckAcessRight(UserInfoID, MenuItem, Convert.ToInt64(ParentMenuID));

            if (lstRights != null && lstRights.Count > 0)
                SetAccessRights(lstRights[0].CanAdd, lstRights[0].CanDelete, lstRights[0].CanEdit, lstRights[0].CanView);
        }
    }

    protected void SetAccessRights(Boolean add, Boolean delete, Boolean edit, Boolean view)
    {
        this.CanAdd = add;
        this.CanDelete = delete;
        this.CanEdit = edit;
        this.CanView = view;
    }

    protected void RedirectToUnauthorised()
    {
        IncentexGlobal.UnAuthorisedURL = Request.UrlReferrer.ToString();
        Response.Redirect("~/Admin/UnAuthorised.aspx");
    }

    protected void StartStopwatch()
    {
        mysw.Reset();
        mysw.Start();
    }

    protected void EndStopwatch(String EventName)
    {
        EndStopwatch(EventName, string.Empty);
    }

    protected void EndStopwatch(String EventName, String ModuleName)
    {
        mysw.Stop();
        PageLoadRepository PLRepo = new PageLoadRepository();

        System.Web.HttpBrowserCapabilities browser = Request.Browser;
        String PlatForm = GetPlatForm();
        PLRepo.AddPageLoadDetails(Common.UserID, Common.CompID, Request.Url.AbsolutePath, EventName, mysw.Elapsed.Milliseconds, ModuleName, string.Empty, browser.Browser, PlatForm);
    }

    protected void EndStopwatch(String EventName, String ModuleName, String SubMenu)
    {
        mysw.Stop();
        System.Web.HttpBrowserCapabilities browser = Request.Browser;

        PageLoadRepository PLRepo = new PageLoadRepository();
        String PlatForm = GetPlatForm();
        PLRepo.AddPageLoadDetails(Common.UserID, Common.CompID, Request.Url.AbsolutePath, EventName, mysw.Elapsed.Milliseconds, ModuleName, SubMenu, browser.Browser, PlatForm);
    }

    private String GetPlatForm()
    {
        var UsrAgent = Request.UserAgent.ToLower();
        if (UsrAgent.Contains("iphone")) return "Iphone";
        else if (UsrAgent.Contains("ipad")) return "Ipad";
        else if (UsrAgent.Contains("ipod")) return "Ipod";
        else if (UsrAgent.Contains("windows")) return "Desktop";
        else return string.Empty;
    }
}