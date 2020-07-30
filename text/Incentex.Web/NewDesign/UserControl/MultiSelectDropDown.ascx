<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MultiSelectDropDown.ascx.cs"
    Inherits="MultiSelectDropDown" %>
<div class="multi-select-ctrl">
    <asp:Label ID="lblMultiSelect" runat="server" CssClass="multi-select-container input-field-all medium" ></asp:Label>
    <div class="multi-select-content content">
        <asp:Repeater ID="repMultiSelect" runat="server">
            <ItemTemplate>
                <div>
                    <asp:CheckBox ID="chkMultiSelect" CssClass='<%# "multi-select-check-" + this.DataIndexField %>'
                        runat="server" Checked='<%# Convert.ToBoolean(Eval(this.DataCheckField)) %>'
                        Text='<%# Eval(this.DataTextField) %>' />
                    <asp:HiddenField ID="hdnMultiSelectID" runat="server" Value='<%# Eval(this.DataValueField) %>' />
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
