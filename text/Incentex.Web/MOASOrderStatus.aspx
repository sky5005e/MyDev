<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MOASOrderStatus.aspx.cs" Inherits="MOASOrderStatus" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        //this is for check that reason for canceling the order is enter
        function validateform() {
            if ($("#ctl00_ContentPlaceHolder1_txtCancelReason").val() == '') {
                alert("Please enter reasion for Cancelling order.");
                return false;
            }
            return true;
        }
    </script>
    
    <%-- Start order already approved part--%>
    <div id="dvOrderAlreadyApproved" runat="server" visible="false">
        <div class="centeralign" style="padding-top:75px;padding-bottom:50px;">
            <img src="Images/manager_approved_already.png" alt="Manager Approved Already" />
            <div class="spacer20">&nbsp;</div>
            <h4>
                This order has already been approved.
            </h4>
        </div>
    </div>
    <%-- End order already approved part--%>
    
    <%-- Start order already canceled part--%>
    <div id="dvOrderAlreadyCanceled" runat="server" visible="false">
        <div class="centeralign" style="padding-top:75px;padding-bottom:50px;">
            <img src="Images/manager_canceled_officially.png" alt="Manager Cancelled Already" />
            <div class="spacer20">&nbsp;</div>
            <h4>
                This order has already been cancelled.
            </h4>
        </div>
    </div>
    <%-- End order already canceled part--%>
    
    <%-- Start order successfully approved part--%>
    <div id="dvOrderSuccessfullyApproved" runat="server" visible="false">
        <div class="centeralign" style="padding-top:75px;padding-bottom:50px;">
            <img src="Images/manager_approved_officially.png" alt="Manager Approved Officially" />
            <div class="spacer20">
                &nbsp;</div>
            <h4>
                Order has been Approved successfully.
            </h4>
        </div>
    </div>
    <%-- End order successfully approved part--%>
    
    <%-- Start order successfully cancel part--%>
    <div id="dvOrderSuccessfullyCancel" runat="server" visible="false">
        <div class="centeralign" style="padding-top:75px;padding-bottom:50px;">
            <img src="Images/manager_canceled_officially.png" alt="Manager Cancelled Officially" />
            <div class="spacer20">
                &nbsp;</div>
            <h4>
                Order has been cancelled successfully.
            </h4>
        </div>
    </div>
    <%-- End order successfully cancel part--%>
    
    <%-- Start order cancel reason part--%>
    <div id="dvOrderCancel" runat="server" visible="false" class="black_round_middle">
        <div class="black_round_box">
            <div class="black2_round_top">
                <span></span>
            </div>
            <div class="black2_round_middle">
                <div class="form_pad pro_search_pad">
                    <div class="black_top_co">
                        <span>&nbsp;</span></div>
                    <div class="black_middle order_detail_pad">
                        <div class="form_table">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box taxt_area clearfix" style="height: 100px">
                                            <span class="input_label alignleft" style="height: 98px">Reason for Cancelling: </span>
                                            <div class="textarea_box alignright">
                                                <div class="scrollbar" style="height: 99px">
                                                    <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                                    </a>
                                                </div>
                                                <asp:TextBox ID="txtCancelReason" runat="server" TextMode="MultiLine" CssClass="scrollme1"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="botbtn centeralign">
                                            <asp:LinkButton ID="lnkCancelOrder" OnClientClick="return validateform();" class="grey2_btn" runat="server" 
                                                onclick="lnkCancelOrder_Click"><span>Cancel Now</span></asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="black_bot_co">
                        <span>&nbsp;</span></div>
                </div>
            </div>
            <div class="black2_round_bottom">
                <span></span>
            </div>
        </div>
    </div>
    <%-- End order cancel reason part--%>
    
</asp:Content>
