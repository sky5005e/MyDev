<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MyIssuancePolicy.aspx.cs" Inherits="MyAccount_MyIssuancePolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad" style="width:auto; overflow:auto;">
        <div class="shipping_method_btn">
            <asp:Repeater ID="Rep" runat="server" OnDataBinding="Rep_DataBinding"  OnItemDataBound="Rep_ItemDataBound"
                OnItemCommand="Rep_ItemCommand" >
                <ItemTemplate>
                    <div class="alignleft">
                        <div>
                        <div id="imgColor" runat="server" class="issuance_pkg_block"></div>
                        <asp:HiddenField ID="hdnBeforeAfter" runat="server" Value='<%# Eval("ShowBeforeAfter") %>' />
                        <asp:HiddenField ID="hdnWorkgroupID"  runat="server" Value='<%# Eval("WorkgroupID") %>' />
                        <asp:HiddenField ID="hdnPolicydate" runat="server" Value='<%# Eval("PolicyDate") %>' />
                        </div>
                        <asp:LinkButton ID="lnkPackage2" runat="server" CommandArgument='<%#Eval("UniformIssuancePolicyID")%>'
                            CommandName="Detail" Text="Month Issuance<br /> Package"> </asp:LinkButton>
                            
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
