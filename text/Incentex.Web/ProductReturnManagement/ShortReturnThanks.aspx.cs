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
using System.Text;
using commonlib.Common;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Collections.Generic;
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL.Common;

public partial class ProductReturnManagement_ShortReturnThanks : PageBase
{
    #region Properties 
    /// <summary>
    /// to check whether short return processed or not
    /// </summary>
    Boolean IsProcessing
    {
        get
        {
            if (ViewState["IsProcessing"] == null)
            {
                ViewState["IsProcessing"] = false;
            }
            return (bool)ViewState["IsProcessing"];
        }
        set
        {
            ViewState["IsProcessing"] = value;
        }
    }
    #endregion 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Shorts Return System";

            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            if (Request.QueryString["Req"] != null && Request.QueryString["Req"] == "processing")
            {
                this.IsProcessing = true;
            }
            SetVisibleContent();
            GeneratedPrintContent();
            setInnerHtml();
            
        }
    }
    private void SetVisibleContent()
    {
        if (IsProcessing)
        {
            dvPrint.Visible = true;
            dvemail.Visible = false;
            dvthankyou.Visible = false;
        }
        else
        {
            dvPrint.Visible = false;
            dvemail.Visible = true;
            dvthankyou.Visible = true;
        }
    }
    /// <summary>
    /// To set Inner Content in div printableArea
    /// </summary>
    private void setInnerHtml()
    {
        String print = "printDiv('printableArea');";
        lblprint.Text = messageBody.ToString();
        btnPrint.Attributes.Add("onclick", print);
    }

    #region Print
    String messageBody = String.Empty;
    private void GeneratedPrintContent()
    {
        IncentexBEDataContext db = new IncentexBEDataContext();
        var ListData = db.ReturnShortProducts.Where(s => s.UserInfoID == IncentexGlobal.CurrentMember.UserInfoID).ToList();
        foreach (var items in ListData)
        {
            try
            {
                EmailTemplateBE objEmailBE = new EmailTemplateBE();
                EmailTemplateDA objEmailDA = new EmailTemplateDA();
                RegistrationBE objRegistrationBE = new RegistrationBE();
                RegistrationDA objRegistrationDA = new RegistrationDA();
                DataSet dsEmailTemplate;
                Common objcommm = new Common();
                //Get Email Content
                objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
                objEmailBE.STemplateName = "Short Return";
                dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
                UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
                UserInformation objUsrInfo = new UserInformation();
                if (dsEmailTemplate != null)
                {
                    string sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                    objUsrInfo = objUsrInfoRepo.GetById(IncentexGlobal.CurrentMember.UserInfoID);
                    string sToadd = "";
                    Int64 sToUserInfoID = 0;
                    if (objUsrInfo != null)
                    {
                        sToadd = objUsrInfo.LoginEmail;
                        sToUserInfoID = objUsrInfo.UserInfoID;
                    }
                    
                    string sSubject = "Shorts Exchange" + items.OrderId.ToString();
                    string sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                    StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                    messagebody.Replace("{Customername}", objUsrInfo.FirstName + " " + objUsrInfo.LastName);
                    messagebody.Replace("{RA}", "RA" + items.OrderId.ToString());
                    messagebody.Replace("{fullname}", objUsrInfo.FirstName + " " + objUsrInfo.LastName);
                    messagebody.Replace("{ReferenceName}", items.OrderId.ToString());
                    // For Quantity
                    messagebody.Replace("{ReturnQuantity}", items.ReturnQty.ToString());
                    messagebody.Replace("{SentOn}", System.DateTime.Now.ToString());
                    messagebody.Replace("{Status}", "Shorts Exchange");
                    messagebody.Replace("{storename}", "Shorts Return System");
                    //Start WTDC Shipp To Address

                    ManagedShipAddressRepository objManageRepos = new ManagedShipAddressRepository();
                    MangedShipAddress objManage = new MangedShipAddress();
                    objManage = objManageRepos.GetAllRecord();
                    if (objManage != null)
                    {
                        messagebody.Replace("{CompanyName}", objManage.CompanyName);
                        messagebody.Replace("{Title}", objManage.Title);
                        messagebody.Replace("{Address}", objManage.Address);
                        messagebody.Replace("{CityStateZip}", new CityRepository().GetById(Convert.ToInt64(objManage.CityId)).sCityName + "  " + new StateRepository().GetById(Convert.ToInt64(objManage.StateId)).sStatename + "  " + objManage.Zipcode);
                        messagebody.Replace("{Tel}", objManage.Telephone);
                    }

                    //End
                    // Start shipping Address

                    if (!String.IsNullOrEmpty(items.FName))
                        messagebody.Replace("{Customername}", items.FName + " " + items.LName);
                    if (!String.IsNullOrEmpty(items.companyName))
                        messagebody.Replace("{CustomerAddress}", items.companyName);
                    if (!String.IsNullOrEmpty(items.City))
                        messagebody.Replace("{CustomerCity}", items.City.ToString());
                    if (!String.IsNullOrEmpty(items.State))
                        messagebody.Replace("{CustomerState}", items.State.ToString());
                    if (!String.IsNullOrEmpty(items.Country))
                        messagebody.Replace("{CustomerCountry}", items.Country.ToString());
                    if (!String.IsNullOrEmpty(items.Phone))
                        messagebody.Replace("{Tel1}", items.Phone.ToString());
                    if (!String.IsNullOrEmpty(items.Email))
                        messagebody.Replace("{Email}", items.Email.ToString());
                    if (!String.IsNullOrEmpty(items.ZipCode))
                        messagebody.Replace("{Zip}", items.ZipCode.ToString());

                    messagebody.Replace("{Customerstorename}", "Shorts Return System");
                    // END
                    #region start
                    string innermessage = "";

                    // For INC_3032 Items
                    if (items.ItemINC3032Qnty !=0)
                    {

                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='20%' style='font-weight: bold;'>";
                        innermessage = innermessage + "INC-3032-00";// 
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + items.ItemINC3032Qnty.ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='20%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + items.ItemINC3032Size.ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='35%' style='font-weight: bold;text-align:left;'>";
                        innermessage = innermessage + items.RemarksINC3032.ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                    }
                    // For INC_3034 Items
                    if (items.ItemINC3034Qnty != 0)
                    {

                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='20%' style='font-weight: bold;'>";
                        innermessage = innermessage + "INC-3034-00";// 
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + items.ItemINC3034Qnty.ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='20%' style='font-weight: bold;text-align:center;'>";
                        innermessage = innermessage + items.ItemINC3034Size.ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='35%' style='font-weight: bold;text-align:left;'>";
                        innermessage = innermessage + items.RemarksINC3034.ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                    }
                    messagebody.Replace("{innermessage}", innermessage);


                    #endregion
                    messagebody.Replace("{reasonforreturn}", "Shorts Return");//+ lblShortReturnDescriptions.Text); 
                    messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                    // Set messageBody for Admin
                    messageBody = messagebody.ToString();
                    // Set print Content
                    //lblprint.Text = messagebody.ToString();
                    //
                    //string smtphost = Application["SMTPHOST"].ToString();
                    //int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                    //string smtpUserID = Application["SMTPUSERID"].ToString();
                    //string smtppassword = Application["SMTPPASSWORD"].ToString();


                    ////Email Management
                    //if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ReturnConfirmations)) == true)
                    //{
                    //    //Live server Message
                    //    General.SendMail(sFrmadd, sToadd, sSubject, messagebody.ToString(), smtphost, smtpport, true, false, sFrmname);
                    //}

                }

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
        }
    }

#endregion 
}
