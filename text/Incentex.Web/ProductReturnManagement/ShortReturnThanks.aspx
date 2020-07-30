<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ShortReturnThanks.aspx.cs" Inherits="ProductReturnManagement_ShortReturnThanks"
    Title="Thanks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function printDiv(divName) {
            var prtContent = document.getElementById(divName);
            var WinPrint = window.open('', '', 'left=0,top=0,width=1,height=1,toolbar=0,scrollbars=0,status=0');
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad" style="padding-left: 30px; padding-right: 30px;">
        <div id="dvthankyou" runat="server">
            <span style="font-size: large; text-align: right; color: Gray; font-family: Trebuchet MS,tahoma,arial,Times New Roman;
                font-weight: bold;">Thank you for submitting your product return request. Please
                review the steps below for the next steps in this process.</span></div>
        <div class="spacer15">
        </div>
        <div id="dvemail" runat="server">
            <label style="color: Gray; font-family: Trebuchet MS,tahoma,arial,Times New Roman;
                font-size: large; font-weight: bold;">
                We have emailed you a product return authorization form. Please print and include
                this form with your shorts and mail to the address listed on the return authorization
                form. This is required to properly process your request.
            </label>
        </div>
        <div id="dvPrint" runat="server">
            <label style="color: Gray; font-family: Trebuchet MS,tahoma,arial,Times New Roman;
                font-size: large; font-weight: bold;">
                Shorts Product already exchanged!. Please print and include this form with your
                shorts and mail to the address listed on the return authorization form. This is
                required to properly process your request.
            </label>
            <div class="spacer15">
            </div>
        </div>
        <div class="spacer15">
        </div>
        <div class="aligncenter">
            <asp:LinkButton ID="btnPrint" runat="server" class="gredient_btn ">
                                    <span><strong>Print email copied</strong></span></asp:LinkButton>
        </div>
    </div>
    <div id="printableArea" style="display:none; visibility:hidden;">
        <asp:Label ID="lblprint" runat="server"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
