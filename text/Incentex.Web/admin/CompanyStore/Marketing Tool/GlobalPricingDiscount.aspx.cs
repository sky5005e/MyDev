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
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Collections.Generic;
using Incentex.DA;
using System.IO;
using System.Text;
using commonlib.Common;
public partial class admin_CompanyStore_Marketing_Tool_GlobalPricingDiscount : PageBase
{
    #region Local Property
    Int64 CompanyStoreId
    {
        get
        {
            if (ViewState["CompanyStoreId"] == null)
            {
                ViewState["CompanyStoreId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {
            ViewState["CompanyStoreId"] = value;
        }
    }
    String StoreName
    {
        get
        {
            if (ViewState["StoreName"] == null)
            {
                ViewState["StoreName"] = "";
            }
            return Convert.ToString(ViewState["StoreName"]);
        }
        set
        {
            ViewState["StoreName"] = value;
        }
    }
    GlobalPricingDiscount objGlobalPricingDiscount = new GlobalPricingDiscount();
    MarketingToolRepository objMarketingToolRepository = new MarketingToolRepository();
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            CheckLogin();
            if (!IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));
                    ((Label)Master.FindControl("lblPageHeading")).Text = "Global Pricing Discount";
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/Marketing Tool/MarketingTool.aspx?id=" + this.CompanyStoreId;
                  
                    BindValues();
                    GetStoreName();
                }
                
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    public void BindValues()
    {
        try
        {
            LookupRepository objLookRep = new LookupRepository();
            // For Workgroup
            ddlWorkgroup.DataSource = objLookRep.GetByLookup("Workgroup");
            ddlWorkgroup.DataValueField = "iLookupID";
            ddlWorkgroup.DataTextField = "sLookupName";
            ddlWorkgroup.DataBind();
            ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));

            // For price level
            ddlPriceLevel.DataSource = objLookRep.GetByLookup("PriceLevel");
            ddlPriceLevel.DataValueField = "iLookupID";
            ddlPriceLevel.DataTextField = "sLookupName";
            ddlPriceLevel.DataBind();
            ddlPriceLevel.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlPriceLevel.Items.RemoveAt(5);

        }
        catch (Exception)
        {


        }
    }
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {            
            objGlobalPricingDiscount.StoreID = this.CompanyStoreId;
            if (ddlWorkgroup.SelectedIndex!=0)
            objGlobalPricingDiscount.WorkgroupID =Convert.ToInt64(ddlWorkgroup.SelectedValue);

            objGlobalPricingDiscount.Discount = Convert.ToInt64 (txtDiscount.Text.Trim());
            if (ddlPriceLevel.SelectedIndex != 0)
                objGlobalPricingDiscount.PriceLevel = Convert.ToInt64(ddlPriceLevel.SelectedValue);

            objGlobalPricingDiscount.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
            objGlobalPricingDiscount.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
            objGlobalPricingDiscount.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objGlobalPricingDiscount.CreatedDate = DateTime.Now;
            if (Convert.ToDateTime(txtEndDate.Text.Trim())>=Convert.ToDateTime(txtStartDate.Text.Trim()))
            {
                objMarketingToolRepository.Insert(objGlobalPricingDiscount);
                objMarketingToolRepository.SubmitChanges();
                sendVerificationEmail(IncentexGlobal.CurrentMember.LoginEmail, IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName, IncentexGlobal.CurrentMember.UserInfoID);
                RestControls();
                lblMsg.Text = "Record Saved Successfully";
            }
            else
            {
                lblMsg.Text = "Start Date Should be lesser or equal to End Date";
            }
            
        }
        catch (Exception)
        {
            lblMsg.Text = "Error Saving";
        }
    }
    private void RestControls()
    {
        ddlWorkgroup.SelectedIndex = 0;
        txtDiscount.Text = "";
        ddlPriceLevel.SelectedIndex = 0;
        txtStartDate.Text = "";
        txtEndDate.Text = "";
    }
    private void sendVerificationEmail(String UserEmail, String UserName, Int64 userInfoID)
    {
        try
        {
            string sFrmadd = IncentexGlobal.CurrentMember.LoginEmail;
            string sToadd = UserEmail.Trim();
            string sFrmname = IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName;
            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();
            string sSubject = "Global Pricing Discount";
            string sBody = "Discount of " + txtDiscount.Text + "% is given on Store " + this.StoreName + " From " + txtStartDate.Text + " To " + txtStartDate.Text;

            String eMailTemplate = String.Empty;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/SimpleMailFormat.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            StringBuilder MessageBody = new StringBuilder(eMailTemplate);
            MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
            MessageBody.Replace("{MessageBody}", sBody.ToString());
            //Live server Message
            new CommonMails().SendMail(userInfoID, null, sFrmadd, sToadd, sSubject, MessageBody.ToString(), Common.DisplyName, Common.SMTPHost, Common.SMTPPort, false, true);
            //Local testing email settings
            if (HttpContext.Current.Request.IsLocal)
                General.SendMail(sFrmadd, "incentextest6@gmail.com", sSubject, MessageBody.ToString(), "smtp.gmail.com", 587, "incentextest6@gmail.com", "test6incentex", sFrmname, true, true);

        }
        catch (Exception ex)
        {
        }

    }
    private void GetStoreName()
    {
        try
        {
            CompanyStoreRepository objStoreName = new CompanyStoreRepository();
            List<SelectCompanyNameCompanyIDResult> objStore = new List<SelectCompanyNameCompanyIDResult>();
            objStore = objStoreName.GetBYStoreId(Convert.ToInt32(this.CompanyStoreId));
            this.StoreName = objStore.FirstOrDefault().CompanyName;
        }
        catch (Exception)
        {}
    }
}
