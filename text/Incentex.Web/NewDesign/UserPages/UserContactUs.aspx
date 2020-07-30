<%@ Page Title="" Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true" CodeFile="UserContactUs.aspx.cs" Inherits="UserPages_UserContactUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" >
    $(document).ready(function() {
        $('body').removeClass('userLogin');
        $('body').addClass('afterLogin');
        $('body').addClass('contact-page');
        $('.welcome-txt').hide();
        $('.contact-block').show();
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>

