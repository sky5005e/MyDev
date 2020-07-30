<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DropDownControl.ascx.cs"
    Inherits="DropDownControl" ClassName="DropDownControl" %>
<%@ Reference Control="~/NewDesign/UserControl/CustomDropDown.ascx" %>
<%@ Register Src="~/NewDesign/UserControl/CustomDropDown.ascx" TagName="CustomDropDown" TagPrefix="cdd" %>
<li class="alignleft selector-close">
    <div class="lbl-txt-midd"><asp:Label ID="lblText" runat="server"></asp:Label></div>
    <span id="dropSpan" runat="server" class="basic-drop">
        <cdd:CustomDropDown ID="ddlCustomDropDown" runat="server" />
    </span>
    <asp:LinkButton ID="lnkbtnRemoveClick" runat="server" CssClass="remove-field"
    OnClick="lnkRemoveControl_Click"><img
        src="../../NewDesign/StaticContents/img/remove-field-ico.png" width="26" height="26" alt="remove"></asp:LinkButton>
    <asp:HiddenField ID="hdnFieldMasterID" runat="server" />
    <asp:HiddenField ID="hdnFieldDetailID" runat="server" />
</li>
