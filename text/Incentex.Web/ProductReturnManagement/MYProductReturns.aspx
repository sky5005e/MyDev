<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MYProductReturns.aspx.cs" Inherits="ProductReturnManagement_MYProductReturns"
    Title="My Product Returns" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div class="form_pad">
            <div class="btn_width worldlink_btn">
                <table>
                    <tr>
                        <td>
                            <div align="center">
                                <a href="OrderProductReturns.aspx" title="Return Product Now" class="gredient_btn"><span>
                                    <strong>Return Product Now</strong></span></a>
                            </div>
                        </td>
                        <td>
                            <a href="ProductReturnView.aspx" title="Open Product Returns" class="gredient_btn"><span>
                                <strong>Open Product Returns </strong></span></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                         <%--Hide Shorts return system as per Ken--%>
                            <div align="center" id="dvShortReturn" runat="server" style="display:none; visibility:hidden;">
                                <asp:LinkButton ID="btnShortReturnSystem" runat="server" 
                                    class="gredient_btn " onclick="btnShortReturnSystem_Click">
                                    <span><strong>Shorts Return System</strong></span></asp:LinkButton>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
