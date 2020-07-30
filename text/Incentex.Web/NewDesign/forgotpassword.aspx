<%@ Page Language="C#" MasterPageFile="~/NewDesign/LoginMaster.master" AutoEventWireup="true"
    CodeFile="forgotpassword.aspx.cs" Inherits="forgotpassword" Title="World-Link System -Forgotpassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript" language="javascript">
         $(document).ready(function () {
             $(window).ValidationUI({ApplyClassToParentLabel:true});
         });
        function ShowMsgPopup() {
            $("#register-block").css('top', '0');
            $("#register-block .register-block").show();
        }
        
    </script>

    <div class="login-form forgot-form">
        <div class="forgot-title">
            FORGOT YOUR PASSWORD?</div>
        <p class="forgot-txt">
            Enter your email address below and a new password will be sent to you shortly.</p>
            <asp:Panel ID="pnForgotPswd" runat="server" DefaultButton="lnkSendPassword">
        <label class="login-field">
            <span class="login-ico"></span>
            <asp:TextBox ID="txtEmail" runat="server" MaxLength="300" TabIndex="1" class="input-txt default_title_text checkvalidation"
                placeholder="Email"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmail"
                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Save"
                ErrorMessage="*"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="rvEmail" Display="Dynamic" CssClass="error" runat="server"
                ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                ValidationGroup="Save" SetFocusOnError="True" ErrorMessage="*"></asp:RegularExpressionValidator>
        </label>
        <asp:LinkButton ID="lnkSendPassword" class="blue-home-btn" runat="server" OnClick="lnkSendPassword_Click" ValidationGroup="Save"
                            call="Save"><span>Send Password</span></asp:LinkButton>
        </asp:Panel>
        <div class="or-txt forgot">
            OR</div>
        <a href="login.aspx" class="blue-home-btn"><span>Back to Login</span></a>
    </div>
    <div class="popup-outer" id="register-block">
        <div class="popupInner">
            <div class="register-block">
                <a href="#" class="close-btn">Close</a>
                <div class="register-content">
                    <p class="reg-txt">
                        Thank you for your request. Please check your email in-box for your password now.</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
