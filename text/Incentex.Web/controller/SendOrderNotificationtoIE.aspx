<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendOrderNotificationtoIE.aspx.cs" Inherits="controller_SendOrderNotificationtoIE" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Order Number:- <asp:TextBox ID="txtOrderNumber" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rete" runat="server" ControlToValidate="txtOrderNumber" ErrorMessage="Please enter Order Number"
            Display="Dynamic" ValidationGroup="SendNotification"></asp:RequiredFieldValidator>
        <asp:Button ID="btnSendNotification"  runat="server" Text="Send Notification to IEs" OnClick="BtnSendNotification_Click" ValidationGroup="SendNotification" />
        <asp:Label ID="lblCount" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
