<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ServiceTicketNotesReplyFromEmail.aspx.cs"
    Inherits="controller_ServiceTicketNotesReplyFromEmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reply to Email</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="cleanmsg" Value="true" runat="server" />
        <asp:HiddenField ID="deletemsg" Value="true" runat="server" />
        <%--<asp:HiddenField ID="host" Value="127.0.0.1" runat="server" />
        <asp:HiddenField ID="port" Value="143" runat="server" />
        <asp:HiddenField ID="username" Value="replyto@world-link.us.com" runat="server" />
        <asp:HiddenField ID="password" Value="7940bhoh" runat="server" />
        <asp:HiddenField ID="ssl" Value="false" runat="server" />
        <asp:HiddenField ID="criteria" Value="unseen" runat="server" />
        <asp:HiddenField ID="folder" Value="INBOX" runat="server" />
        <asp:TextBox ID="txtCharSet" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />--%>
    </div>
    </form>
</body>
</html>
