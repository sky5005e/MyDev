using System;
using System.Configuration;
using System.Data;
using System.Text;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class forgotpassword : System.Web.UI.Page
{
    #region Data Members

    EmailTemplateBE objEmailBE = new EmailTemplateBE();
    EmailTemplateDA objEmailDA = new EmailTemplateDA();
    DataSet dsEmailTemplate;
    Common objcommm = new Common();

    #endregion

    #region Event Handlers

    protected void Page_Load(Object sender, EventArgs e)
    {

    }

    protected void lnkSendPassword_Click(Object sender, EventArgs e)
    {
        try
        {
            UserInformationRepository objUserRepo = new UserInformationRepository();
            GetUserInfoForForgetPasswordByEmailResult objUserInformation = objUserRepo.GetUserInfoForForgetPasswordByEmail(txtEmail.Text.Trim());

            if (objUserInformation != null)
            {
                String newPassword = RandomString(8);

                UserInformation objUser = objUserRepo.GetById(objUserInformation.UserInfoID);
                if (objUser != null)
                {
                    objUser.Password = newPassword;
                    objUserRepo.SubmitChanges();

                    objUserInformation.Password = newPassword;
                    sendVerificationEmail(objUserInformation);
                    this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowMsgPopup();", true);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Methods

    private void sendVerificationEmail(GetUserInfoForForgetPasswordByEmailResult objUserInformation)
    {
        try
        {
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "forgot password";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                String sFrmadd = Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"]);
                String sToadd = Convert.ToString(objUserInformation.LoginEmail);
                String sSubject = Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sSubject"]);
                String sFrmname = Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sFromName"]);

                StringBuilder messagebody = new StringBuilder(Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"])); // From Template table
                messagebody.Replace("{fullname}", Convert.ToString(objUserInformation.FirstName) + " " + Convert.ToString(objUserInformation.LastName));
                messagebody.Replace("{email}", Convert.ToString(objUserInformation.LoginEmail));
                messagebody.Replace("{password}", Convert.ToString(objUserInformation.Password));
                messagebody.Replace("{siteurl}", Convert.ToString(ConfigurationSettings.AppSettings["siteurl"]));
                messagebody.Replace("{CompanyName}", Convert.ToString(objUserInformation.CompanyName));

                String smtphost = Convert.ToString(Application["SMTPHOST"]);
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Convert.ToString(Application["SMTPUSERID"]);
                String smtppassword = Convert.ToString(Application["SMTPPASSWORD"]);

                new CommonMails().SendMail(objUserInformation.UserInfoID, "Forgot Password", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Methods

    private String RandomString(Int32 len)
    {
        Random r = new Random();
        String str = "ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghijklmnpqrstuvwxyz123456789";
        StringBuilder sb = new StringBuilder();

        while ((len--) > 0)
            sb.Append(str[(Int32)(r.NextDouble() * str.Length)]);

        return sb.ToString();
    }

    #endregion
}