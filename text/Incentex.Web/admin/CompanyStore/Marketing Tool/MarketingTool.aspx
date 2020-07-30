<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="MarketingTool.aspx.cs" Inherits="admin_CompanyStore_Marketing_Tool_MarketingTool" Title="Marketing Tools" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <div class="btn_width worldlink_btn">
                 <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td style="width:20%"></td>
                <td style="width:30%">
                                
                 <asp:LinkButton ID="lnkGlobalPricingDiscount" class="gredient_btn" title="Global Pricing Discount"
                            runat="server"  onclick="lnkGlobalPricingDiscount_Click" >
              <%--  <img src="../../../admin/Incentex_Used_Icons/addinventory.png" alt="World-Link System Control" />--%>
                <span style="width:200px;">               
                  Global Pricing Discount           
                </span>
                        </asp:LinkButton> 
                
                </td>
               
                <td style="width:50%">
                      <asp:LinkButton ID="lnkPromotionCode" class="gredient_btn" title="Promotion Code"
                            runat="server"  onclick="lnkPromotionCode_Click" >
               <%-- <img src="../../../admin/Incentex_Used_Icons/searchinventory.png" alt="World-Link System Control" />--%>
                <span style="width:200px;">               
                  Promotion Code           
                </span>
                        </asp:LinkButton> 
                </td>
                </tr>
                </table>        
               
                        
           
        </div>
    </div>
     
</asp:Content>

