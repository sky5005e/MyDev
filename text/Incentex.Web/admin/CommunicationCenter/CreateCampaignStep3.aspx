<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CreateCampaignStep3.aspx.cs" Inherits="admin_CommunicationCenter_CreateCampaignStep3"
    Title="Communications Center" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link media="screen" rel="stylesheet" href="../../CSS/colorbox.css" />
    <style type="text/css">
        .custom-checkbox input, .custom-checkbox_checked input
        {
            width: 20px;
            height: 20px;
            margin-left: -8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad" style="padding-left: 200px">
        <div class="spacer25">
        </div>
        <div class="bot_alert next">
            <asp:LinkButton ID="lnkNext" runat="server" title="Final Step" OnClick="lnkNext_Click">Step 4</asp:LinkButton>
        </div>
        <div class="bot_alert prev">
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        </div>
        <div>
            <table>
                <tr>
                    <td class="btn_width worldlink_btn">
                            <asp:LinkButton ID="lnkUploadStyleSheet" class="gredient_btn1" runat="server" ToolTip="Upload Templates"
                                OnClick="lnkUploadStyleSheet_Click">
                            <img src="../Incentex_Used_Icons/manage-template.png" alt="Manage Templates" />
                            <span>Manage Templates</span></asp:LinkButton>
                    </td>
                    <td class="btn_width worldlink_btn">
                            <asp:LinkButton ID="LinkButton3" class="gredient_btn1" runat="server" ToolTip="Send Quick Message"
                                OnClick="LnkQuickMsg_Click">
                            <img src="../Incentex_Used_Icons/create-email-message.png" alt="Create Email Message" />
                            <span>Create Email Message</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
