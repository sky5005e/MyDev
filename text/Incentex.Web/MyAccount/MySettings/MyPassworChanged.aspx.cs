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
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_MySettings_MyPassworChanged : PageBase
{
    UserInformationRepository objUsrInfoRep = new UserInformationRepository();
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        ((Label)Master.FindControl("lblPageHeading")).Text = "Password Sections";
        if (IncentexGlobal.IsIEFromStore)
        {
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
        }
        else
        {
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/MySettings/MySettingMenuOptions.aspx";
        }
        TxtOldPassword.Focus();
    }
    protected void lnkChangePassword_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        //VALIDATIONS 
        if (string.IsNullOrEmpty(TxtOldPassword.Text))
        {
            lblMsg.Text = "Please enter old password";
            return;
        }
        if (!string.IsNullOrEmpty(TxtNewPassword.Text))
        {

            if (IncentexGlobal.CurrentMember.Password != TxtOldPassword.Text.Trim())
            {
                lblMsg.Text = "Please enter your correct Old Password !!";
                return;
            }
            else
            {
                lblMsg.Text = string.Empty;
            }
        }

        try
        {
            #region User Information

            //update User information
            UserInformation objUserInfo = objUsrInfoRep.GetById(IncentexGlobal.CurrentMember.UserInfoID);
            if (TxtNewPassword.Text != string.Empty || TxtConfirmNewPassword.Text != string.Empty)
            {
                objUserInfo.Password = TxtNewPassword.Text;
                objUsrInfoRep.SubmitChanges();
                IncentexGlobal.CurrentMember.Password = objUserInfo.Password;
                lblMsg.Text = "Password changed successfully";

            }
            else
            {
                lblMsg.Text = "Please enter old password or new password or confirm new password!!";
                return;
            }


            #endregion
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

            if (ex.ToString().Contains("Cannot insert duplicate key"))
                lblMsg.Text = "Record already exists.";
            else
                lblMsg.Text = "Error in saving record ...";

        }
    }
}
