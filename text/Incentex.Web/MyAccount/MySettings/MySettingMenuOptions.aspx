<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MySettingMenuOptions.aspx.cs" Inherits="MyAccount_MySettings_MySettingMenuOptions" Title="MySetting Menu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <%--<div class="btn_width worldlink_btn">
            <div align="center" >
                <asp:DataList ID="dtIndex" runat="server" RepeatDirection="Vertical"  
                    RepeatColumns="2" onitemcommand="dtIndex_ItemCommand" Width="50%">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkMenuAccess" CommandName="Redirect" CommandArgument='<%# Eval("sLookupName")%>' ToolTip='<%# Eval("sLookupName")%>' CssClass="gredient_btn" runat="server"><span><strong><%# Eval("sLookupName")%></strong></span></asp:LinkButton>
                        <asp:HiddenField ID="hdnMenuAccess" runat="server" Value='<%# Eval("iLookupID") %>' />
                    </ItemTemplate>
                   </asp:DataList>
                </div>
        </div>--%>
        
        <div >
            <div>
            <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
            <td>
                    
             <asp:LinkButton ID="lnkUserInformation" class="gredient_btnMainPage" title="User Information"
                            runat="server" onclick="lnkUserInformation_Click" >
                <img src="../../admin/Incentex_Used_Icons/user_information.png"    alt="World-Link System Control" />
                <span style="width:200px;">               
                   User Information             
                </span>
                        </asp:LinkButton>
            </td>
            <td>
             <asp:LinkButton ID="lnkShippingInformation" class="gredient_btnMainPage" title="Shipping Information"
                            runat="server" onclick="lnkShippingInformation_Click" >
                <img src="../../admin/Incentex_Used_Icons/shipping_information.png"  alt="World-Link System Control" />
                <span style="width:200px;">               
                   Shipping Information             
                </span>
                        </asp:LinkButton>
            </td>
            <td>
           
             <asp:LinkButton ID="lnkBillingInformation" class="gredient_btnMainPage" title="Billing Information"
                            runat="server" onclick="lnkFlaggedAssets_Click" >
                <img src="../../admin/Incentex_Used_Icons/billing_inforamtion.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                   Billing Information             
                </span>
                        </asp:LinkButton>
            </td>
            </tr>
             <tr>
            <td>
              <asp:LinkButton ID="lnkPasswordChange" class="gredient_btnMainPage" title="Password Change"
                            runat="server" onclick="lnkPasswordChange_Click" >
                <img src="../../admin/Incentex_Used_Icons/password_change.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Password Change            
                </span>
                        </asp:LinkButton>
           
            </td>
            <td>
             
         
            </td>
            <td>
              
         
            </td>
            </tr>
            
            
              
            </table>
             
             
             </div>    
        </div>
    </div>
</asp:Content>
