<%@ Page Language="C#" AutoEventWireup="true" 
MasterPageFile="~/MasterPage.master"
CodeFile="JqueryXMLValidClientside.aspx.cs" Inherits="RND_testAjaxSubmit"
    Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript" src="validationMessage.js" language="javascript"></script>
<script type="text/javascript" language="javascript">

    $().ready(function() {

        //load the xml file and return the array

        //        var xmlDocument = [create xml document];
        //            $.ajax({
        //               type: "GET",
        //                url: "Commonmsg.xml",
        //                dataType: "xml",
        //                success: xmlDocument
        //               });
        //
        //        alert(xmlDocument);


        //            $.get('Commonmsg.xml', function(xml) {
        //                json = $.xml2json(xml);
        //                alert(json.Required);
        //            });
        //


        $.get('Commonmsg.xml', function(xml) {
            json = $.xml2json(xml);
         

            $("#aspnetForm").validate({
                rules: {
                    ctl00$ContentPlaceHolder1$txtCompanyName: { required: true, alphanumeric: true}
                },

                messages: {
                    ctl00$ContentPlaceHolder1$txtCompanyName: {
                    required: '<br/>' + replaceJsonstr(json, "field", "Company name"),
                        alphanumeric: json.Number
                    }
                }
            });
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
