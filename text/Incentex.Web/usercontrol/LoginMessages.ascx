<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginMessages.ascx.cs"
    Inherits="UserControl_LoginMessages" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
 <script language="javascript" type="text/javascript">
     //check for each condition is check by user.
     function checkAcceptTerms() {
         var val = [];
         var valcheck = [];
         $('.checktable_supplier :checkbox').each(function(i) {
             val[i] = $(this).val();
         });
         $('.checktable_supplier :checkbox:checked').each(function(i) {
             valcheck[i] = $(this).val();
         });
         if (val.length!=valcheck.length) {
             alert("Please select all Terms & Conditions.");
             return false;
         }
         return true;
     }       
</script>
<asp:LinkButton ID="lnkDummy" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
<at:ModalPopupExtender ID="mpeAccepTerms" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground"
    DropShadow="true" runat="server" PopupControlID="pnlLoginMessages">
</at:ModalPopupExtender>
<asp:PlaceHolder runat="server" ID="phScriptManager"></asp:PlaceHolder>
<asp:Panel ID="pnlLoginMessages" runat="server" Style="display: none;left:30%;top:30%;" >
    <div class="pp_pic_holder facebook" style="display: block; width: 711px;">
        <div class="pp_top" style="">
            <div class="pp_left">
            </div>
            <div class="pp_middle">
            </div>
            <div class="pp_right">
            </div>
        </div>
        <div class="pp_content_container" style="">
            <div class="pp_left" style="">
                <div class="pp_right" style="">
                    <div class="pp_content" style="height:auto; display: block;">
                        <div class="pp_fade" style="display: block;">
                            <div id="pp_full_res">
                                <div class="pp_inline clearfix">
                                        <h4 style="margin-bottom:15px;">Terms & Conditions</h4>
                                        <div style="margin-bottom:20px;overflow:auto;height:400px;" class="checktable_supplier true">
                                        <asp:Repeater ID="rptTNC" runat="server">
                                            <ItemTemplate>
                                                <div>
                                                    <div>
                                                        <span class="custom-checkbox alignleft" id="AcceptTermsspan" runat="server">
                                                            <asp:CheckBox runat="server" ID="chkAcceptTerms" />
                                                        </span>
                                                        <strong>
                                                            <%# Eval("TNCHeader")%></strong>
                                                    </div>
                                                    <div style="margin-left:32px;margin-top:5px;">
                                                            <%# Eval("TNCContent")%>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                            <SeparatorTemplate>
                                                <div class="spacer20">
                                                </div>
                                            </SeparatorTemplate>
                                        </asp:Repeater>
                                        </div>
                                        <div class="centeralign">
                                            <asp:LinkButton ID="lnkbtnAccept" OnClientClick="return checkAcceptTerms();" OnClick="lnkbtnAccept_Click" class="grey2_btn" runat="server"><span>Accept</span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkbtnDecline" class="grey2_btn" OnClick="lnkbtnDecline_Click"  runat="server"><span>Decline</span></asp:LinkButton>
                                        </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="pp_bottom" style="">
            <div class="pp_left" style="">
            </div>
            <div class="pp_middle" style="">
            </div>
            <div class="pp_right" style="">
            </div>
        </div>
    </div>
</asp:Panel>
