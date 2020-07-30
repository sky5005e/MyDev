<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TextBoxControl.ascx.cs" Inherits="TextBoxControl" 
ClassName="TextBoxControl" %>

<li class='alignleft'>
        <label>
            <div class="lbl-txt-midd"><asp:Label ID="lblText" runat="server" EnableViewState="true" ></asp:Label></div>
            <asp:TextBox ID="txtSpecification" runat="server" CssClass="input-field-all" TabIndex="2"></asp:TextBox>
            <asp:HiddenField ID="hdnFieldMasterID" runat="server" />
            <asp:HiddenField ID="hdnFieldDetailID" runat="server" />
        </label>
        <asp:LinkButton ID="lnkbtnRemoveClick" runat="server" CssClass="remove-field" OnClick="lnkRemoveControl_Click"><img
        src="../../NewDesign/StaticContents/img/remove-field-ico.png" width="26" height="26" alt="remove"></asp:LinkButton>
</li>