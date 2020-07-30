<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JSON.aspx.cs" Inherits="RND_JSON" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DropDownList ID="ddlCountry" Width="180px" runat="server">
            <asp:ListItem Text="All" Value="0"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div style="width: 200px;" class="fl">
        <div id="dvState">
            <select id="ddlState" style="width: 180px;">
            </select>
        </div>
    </div>
    <div style="width: 200px;" class="fl">
        <select id="ddlCity" style="width: 180px;">
        </select>
    </div>
    </form>
</body>
</html>
