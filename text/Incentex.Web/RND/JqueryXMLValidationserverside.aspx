<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="JqueryXMLValidationserverside.aspx.cs" Inherits="RND_testXMLValidation" Title="Untitled Page" %>

<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="commonlib.Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<%-- <form runat="server" id="aspnetform">--%>
<%Common objcomm = new Common();
    
 %>
   
    <script type="text/javascript" language="javascript">

        $().ready(function() {
            $("#aspnetForm").validate({
                rules: {
                    ctl00$ContentPlaceHolder1$txtCompanyName: { required: true, alphanumeric: true }

                },

                messages: {
                    ctl00$ContentPlaceHolder1$txtCompanyName: {
                        required: '<br/><%=objcomm.Callvalidationmessage(Server.MapPath("Commonmsg.xml"),"Required","company name")%>',
                        alphanumeric: '<br/><%=objcomm.Callvalidationmessage(Server.MapPath("Commonmsg.xml"),"Number","")%>'
                    }
                }
            });

        });

    </script>

    <table>
        <tr>
            <td>
                Company Name
                <asp:TextBox ID="txtCompanyName" MaxLength="100" runat="server"></asp:TextBox>
            </td>
            <td>
                
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <asp:Button id="btnSubmit" Text="Submit"  runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
