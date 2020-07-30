<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Priority.aspx.cs" Inherits="DropdownMenu_Priority" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script src="../JS/jQuery/jquery-1.3.2.min.js" type="text/javascript" language="javascript"></script>

    <script src="../JS/JQuery/jquery.validate.js" type="text/javascript" language="javascript"></script>

</head>
<body>
    <form id="form1" runat="server">

    <script type="text/javascript" language="javascript">


        var formats = 'jpg|gif|png';

        $().ready(function() {

        $("#form1").validate({
                rules: {
                txtPriorityName: 
                    {
                         required: true,
                         alphanumeric: true 
                     },
                     flFile: 
                     {
                         required: true,
                         accept: formats 
                     }

                },
                messages: 
                {
                    txtPriorityName: { required: "<br />Please enter video title." },
                    flFile: { required: "<br />Please select file to upload.", accept: "<br />File type not supported." }
                }
            });
        });

    </script>

    <div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblPrioryName" runat="server" Text="Priority"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPriorityName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPriorityIcon" runat="server" Text="Priority Icon"></asp:Label>
                </td>
                <td>
                    <%--<asp:FileUpload ID="fpUp" runat="server" />--%>
                    <input type="file" id="flFile" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
