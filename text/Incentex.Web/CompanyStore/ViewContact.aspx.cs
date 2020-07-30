using System;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class CompanyStore_ViewContact : PageBase
{
    CompanyStoreDocumentRepository objRep = new CompanyStoreDocumentRepository();

    Int64 CompanyID;
    Int64 UserInfoID;
    
    #region Page Load Event

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Redirect user to login page if session gets expires
            CheckLogin();

            //Assign Page Header and return URL 
            ((Label)Master.FindControl("lblPageHeading")).Text = "Contact Us";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            if (IncentexGlobal.IsIEFromStore)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            }

            //Retrieve UserID and CompanyID
            //UserInfoID = Int64.Parse(Request.QueryString["u"]);
            //CompanyID = Int64.Parse(Request.QueryString["c"]);
            UserInfoID = Int64.Parse(Session["UserInfoID"].ToString());
            CompanyID = Int64.Parse(Session["CompanyID"].ToString());

            //Load Contacts
            LoadContacts();
        }
    }

    #endregion

    #region Misc Functions
    
    /// <summary>
    /// Created on: 21-Dec-2010 Shehzad
    /// Loads all Contacts for the Store and Workgroup
    /// </summary>
    private void LoadContacts()
    {
        try
        {
            //get workgroup by userinformationid
            CompanyEmployee objCmpnyInfo = new CompanyEmployee();
            CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
            objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

            //Bind Repeater
            rptContacts.DataSource = objRep.GetAllContactsByWorkgroupID(objCmpnyInfo.WorkgroupID, CompanyID);
            rptContacts.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    /// <summary>
    /// Created Date: 21-Dec-2010 by Shehzad
    /// Sets the image for the each Contact in Repeater
    /// </summary>
    /// <param name="value">Gets the sender</param>
    /// <returns></returns>
    protected string SetImages(object value)
    {
        string strImage;
        if (value != null) //Set Path + Image name
        {
            strImage = "../UploadedImages/CompanyStoreDocuments/" + value.ToString();
            return strImage;
        }

        //Set the default photo if value is null in database
        strImage = "../UploadedImages/employeePhoto/employee-photo.gif";
        return strImage;
    }
 
    #endregion

}
