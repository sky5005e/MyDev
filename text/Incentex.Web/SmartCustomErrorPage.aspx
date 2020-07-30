<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SmartCustomErrorPage.aspx.cs" Inherits="SmartCustomErrorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Smart Error Page!</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>A Smart Error Page!</h1>
        <p>
            I am a smart(er) error page. Since I am reached in the <code>Error</code> event via a
            <code>Server.Transfer()</code>, I can obtain information about the exception via
            <code>Server.GetLastError()</code>. The message shown below depends on the type of exception
            that was thrown. My functionality could be extended to show more (or less) details about the exception
            based on the user who reached this page...
        </p>
        <p>
            <asp:Label runat="server" ID="ErrorDetails"></asp:Label>
        </p>
    </div> 
    </form>
</body>
</html>
