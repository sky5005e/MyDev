<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="IssuancePackage.aspx.cs" Inherits="MyAccount_IssuancePackage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        $(function() {
            //  scrolltextarea(".scrollme", "#Scrolltop1", "#ScrollBottom1");
            //  scrolltextarea(".scrollme2", "#Scrolltop2", "#ScrollBottom2");
            // scrolltextarea(".scrollme3", "#Scrolltop3", "#ScrollBottom3");
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad pro_search_pad">
    <!-- Product List -->
    
       
    
        
        
        
        <asp:Repeater ID="rep1" runat="server"
                OnDataBinding="rep1_DataBinding" OnItemCommand="rep1_ItemCommand" >
                
            <HeaderTemplate>
                <!--List Header -->
                <div class="tab_header">
                    <div class="tab_header_r">
                        <table>
                            <tr>
                                <td style="width: 40%;" class="alignleft">
                                    Uniform Image
                                </td>
                                <td align="center" style="width: 20%;">
                                    Issuance
                                </td>
                                <td style="width: 20%;">
                                    Balance Left
                                </td>
                                <td style="width: 30%;">
                                    Package Value
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </HeaderTemplate>
        
            <ItemTemplate>
                <div>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 24%;">
                                <div class="alignleft">
                                    <div class="agent_img">
                                        <span class="tl_co"></span><span class="tr_co"></span><span class="bl_co"></span>
                                        <span class="br_co"></span><a href="#" title="Pilot Shirt">
                                            <%--<img src="../images/pro-issu-img.jpg" alt="Pilot Shirt" /></a>--%>
                                            <asp:Image ID="imgPhoto" runat="server" />
                                            
                                            </div>
                                            
                                </div>
                            </td>
                            <td style="width: 76%;">
                                <table class="shoppingcart_box">
                                    <tr>
                                        <td style="padding: 0px;">
                                            <div class="cart_header">
                                                <div class="left_co">
                                                </div>
                                                <div class="right_co">
                                                </div>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 40%;">
                                                            <%#Eval("Issuance")%>
                                                        </td>
                                                        <td style="width: 15%;">
                                                         <%# Eval("Issuance")%>
                                                        </td>
                                                        <td style="width: 45%;">
                                                            <%# Eval("Issuance")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="spacer10">
                </div>
            </ItemTemplate>
            
            <FooterTemplate>
                <div class="alignright">
                    <table class="shoppingcart_box">
                        <tr class="total_count">
                            <td class="rightalign">
                                Total Package Value:
                            </td>
                            <td>
                                $235.00
                            </td>
                        </tr>
                    </table>
                </div>
            </FooterTemplate>
            
        </asp:Repeater>
        
        <div class="alignnone spacer10">
        </div>
        <p>
            You are eligible to order these uniform pieces on 5/12/2010, if it is before that
            time we offer the ability for your to pre-order this item and will ship once you
            reach your ability date.</p>
        <div class="shopping_cart_btn alignright">
            <a href="#" class="grey2_btn" title="Approve Order"><span>Order Now</span></a>
            <%--<a href="#" class="grey2_btn" title="Edit Order"><span>Pre-Order Package</span></a>
            --%>
            <asp:LinkButton ID="lnkPreOrder" runat="server" CssClass="grey2_btn" OnClick="lnkPreOrder_Click">
            <span>Pre-Order Package</span>
            </asp:LinkButton>
        </div>
    </div>
</asp:Content>
