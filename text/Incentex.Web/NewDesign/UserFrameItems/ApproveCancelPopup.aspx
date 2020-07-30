<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApproveCancelPopup.aspx.cs" Inherits="UserFrameItems_ApproveCancelPopup" %>

<%@ Register TagPrefix="uc" TagName="CommonHeader" Src="~/NewDesign/UserControl/NewCommonHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <uc:CommonHeader ID="ucCommonHead" runat="server" />
    <title>Incentex</title>
    
</head>
<body class="NoClass" style="background-color : Transparent;">
    <form id="formApproveCancel" runat="server">
    <div class="popup-block">
                    <h3>
                        <asp:Label ID="lblMsgTitle" runat="server" ></asp:Label></h3>
                        <div class="generalmsg-content">
                        <p id="pMsgContent">
                        </p>
                    </div>
                    <label id="lblReasonContent" runat="server" visible="false">
                        <textarea id="txtMstReasonCode" rows="7" cols="7" runat="server" placeholder="Type reason for cancelling..."
                            class="default_title_text multiline-text"></textarea>
                    </label>
                    <div class="btn-block">
                        <asp:LinkButton ID="lnkbtnCancel" runat="server" class="gray-btn" >
                            Cancel</asp:LinkButton>
                             <asp:LinkButton ID="lnkbtnSubmit" runat="server" class="blue-btn" >
                            submit</asp:LinkButton>
                    </div>
                </div>
                <span class="popup-bot">&nbsp;</span>
    </form>
</body>
</html>
