<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CustomDropDown.ascx.cs" Inherits="CustomDropDown" %>

<%--<script src="../../JS/JQuery/jquery-1.3.2.min.js" type="text/javascript"></script>--%>

<script type="text/javascript" language="javascript">
    $().ready(function() {
        $("select[id$='ddlDropDown']").change(function() {
            if ($(this).val() == $("input[type=hidden][id$='hdnValueToAddNewOption']").val()) {
                CheckCustomTextBox(this);
            }
        });

        $("input[id$='txtTextBox']").blur(function() {
            CheckCustomTextBox(this);
            $(this).siblings(".btnHiddenClick").click();
        });
    });
</script>

<asp:DropDownList ID="ddlDropDown" runat="server" OnSelectedIndexChanged="ddlDropDown_SelectedIndexChanged"
    AutoPostBack="true" onchange="ShowDefaultLoader();">
</asp:DropDownList>
<asp:Button ID="btnButton" CssClass="btnHiddenClick" runat="server" 
    OnClick="btnButton_Click" style="display:none;" />
<asp:TextBox ID="txtTextBox" runat="server" Visible="false" onblur="ShowDefaultLoader();" CssClass="txtCustom"></asp:TextBox>
<asp:HiddenField ID="hdnValueToAddNewOption" runat="server" />
<asp:HiddenField ID="hdnParentSpanClassToRemove" runat="server" />
<asp:HiddenField ID="hdnIsEditing" runat="server" Value="false" />
<asp:HiddenField ID="hdnModule" runat="server" Value="false" />
    <%--<asp:RequiredFieldValidator ID="rqBasicAssetType" runat="server" ControlToValidate="ddlDropDown"
                                        Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" ValidationGroup="SaveBasic"
                                        ErrorMessage="*"></asp:RequiredFieldValidator>--%>
    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTextBox"
                                        Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" ValidationGroup="SaveBasic"
                                        ErrorMessage="*"></asp:RequiredFieldValidator>--%>