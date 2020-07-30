<%@ Page Title="" Language="C#" MasterPageFile="~/NewDesign/LoginMaster.master" AutoEventWireup="true" CodeFile="Loginfailed.aspx.cs" Inherits="Loginfailed" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            $(window).ValidationUI({ ApplyClassToParentLabel: true });
        });
        function ShowMsgPopup() {
            $("#register-block").css('top', '0');
            $("#register-block .register-block").show();
        }
        
    </script>

    <div class="login-form forgot-form">
        <div class="forgot-title">
            Login Failed</div>
        <p class="forgot-txt">
            The login email or password you have entered is not correct.
        </p>
        <p class="forgot-txt">
            Please try again !!
        </p>
        <div class="or-txt">
            OR</div>
        <p class="forgot-txt">
            Contact <a href="mailto:support@world-link.us.com" title="support@world-link.us.com">
                support@world-link.us.com</a>
        </p>
        <br />
        <a href="<%=System.Configuration.ConfigurationManager.AppSettings["siteurl"]%>login.aspx" class="blue-home-btn"><span>Back to Login</span></a>
        <%-- <br />
            You have been deactivated from the system
            <br />
            If you have any questions, contact <a href="mailto:support@world-link.us.com" title="support@world-link.us.com">
                support@world-link.us.com</a>
            <br />
            <asp:Label ID="lblClosedMessage" runat="server" Text="Your Company store is Closed from the system"></asp:Label>
            <br />
            If you have any questions, contact <a href="mailto:support@world-link.us.com" title="support@world-link.us.com">
                support@world-link.us.com</a>--%>
    </div>
</asp:Content>

