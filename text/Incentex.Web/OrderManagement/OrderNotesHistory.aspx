<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OrderNotesHistory.aspx.cs" Inherits="OrderManagement_OrderNotesHistory" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>

    <script type="text/javascript" language="javascript">
        function buttonclick()
        {
         jConfirm('Are you sure you want to delete this item?', 'Delete Confirmation',
            function(retval)
            {
            }
         );
        
        }
        function CheckNoteHistory()
        {
          if(document.getElementById('ctl00_ContentPlaceHolder1_txtNote').value != "")
           {
               /* if(confirm('YOU ARE ABOUT TO SEND A NOTE TO A CUSTOMER'))
                {
                    return true;
                }
                else
                {
                    return false;
                }*/
                return true;
           }
           else
           {
               alert('Please enter notes/history');
               return false;
           }
         }
         
        function CheckNoteHistoryForIE()
        {
          if(document.getElementById('ctl00_ContentPlaceHolder1_txtNoteForIE').value != "")
           {
                return true;
           }
           else
           {
               alert('Please enter notes/history');
               return false;
           }
         }
        
        $(function() {
            scrolltextarea(".scrollme1", "#Scrolltop1", "#ScrollBottom1");
        });

        
            
    </script>

    <style type="text/css">
        .textarea_box textarea
        {
            height: 196px;
        }
        .textarea_box
        {
            height: 196px;
        }
        .textarea_box .scrollbar
        {
            height: 196px;
        }
        .width300
        {
            font-size: small;
            width: 300px !important;
        }
    </style>
    <div class="black_round_box">
        <div class="black2_round_top">
            <span></span>
        </div>
        <div class="black2_round_middle">
            <div class="form_pad">
                <div class="pro_search_pad" style="width: 900px;">
                    <mb:MenuUserControl ID="menucontrol" runat="server" />
                    <div>
                        <%-- <div class="black_top_co">
                            <span>&nbsp;</span></div>--%>
                        <div class="black_middle order_detail_pad">
                            <table class="order_detail" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 50%">
                                        <table>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label>
                                                        Order Number :
                                                    </label>
                                                    <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label>
                                                        Reference Name :
                                                    </label>
                                                    <asp:Label ID="lblOrderBy" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 50%">
                                        <table>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 29px!important;">
                                                        Ordered Date :
                                                    </label>
                                                    <asp:Label ID="lblOrderedDate" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 29px!important;">
                                                        Order Status :
                                                    </label>
                                                    <asp:Label runat="server" ID="lblOrderStatus"></asp:Label>
                                                </td>
                                            </tr>
                                            <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                              {%>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 29px!important;">
                                                        Payment Method :
                                                    </label>
                                                    <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <%} %>
                                            <tr id="trCreditType" runat="server">
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 29px!important;">
                                                        Credit Type :
                                                    </label>
                                                    <asp:Label ID="lblCreditType" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="black_bot_co">
                            <span>&nbsp;</span></div>
                        <div class="spacer15">
                        </div>
                        <div class="black_top_co">
                            <span>&nbsp;</span></div>
                        <div class="black_middle order_detail_pad">
                            <div class="clearfix billing_head">
                                <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                  {%>
                                <div class="alignleft">
                                    <span>Bill To: </span>
                                </div>
                                <%}
                                  else
                                  {%>
                                <div class="alignleft">
                                    <span>&nbsp;</span>
                                </div>
                                <%} %>
                                <div class="alignright">
                                    <span style="padding-left: 29px!important;">Ship To:</span>
                                </div>
                            </div>
                            <div>
                                <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                  {%>
                                <div class="alignleft" style="width: 49%;">
                                    <div class="tab_content_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="tab_content">
                                        <table class="order_detail" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBName" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBCompany" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBAddress1" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBAddress2" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBCity" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBCountry" runat="server" /></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="tab_content_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                                <%}
                                  else
                                  {%>
                                <div class="alignleft" style="width: 49%;">
                                    &nbsp;
                                </div>
                                <%} %>
                                <div class="alignright" style="width: 49%;">
                                    <div class="tab_content_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="tab_content">
                                        <table class="order_detail" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSName" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSCompany" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSAddress" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSAddress2" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSStreet" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSCity" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSCountry" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="tab_content_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                                <div class="alignnone">
                                </div>
                            </div>
                        </div>
                        <div class="black_bot_co">
                            <span>&nbsp;</span></div>
                        <div class="spacer15">
                        </div>
                    </div>
                    <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                      {%>
                    <div class="clearfix billing_head">
                        Notes/History :
                    </div>
                    <div>
                        <div class="form_table">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box taxt_area clearfix" style="height: 200px">
                                <span class="input_label alignleft" style="height: 200px">Notes/History :</span>
                                <div class="textarea_box alignright">
                                    <div class="scrollbar" style="height: 200px">
                                        <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                            class="scrollbottom"></a>
                                    </div>
                                    <asp:TextBox ID="txtOrderNotesForCECA" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                        Height="197px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                        <div class="alignnone spacer15">
                        </div>
                        <div class="rightalign gallery">
                            <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                            <asp:LinkButton ID="lnkAddNew" CommandName="AddNotes" runat="server" OnClick="lnkAddNew_Click"
                                CssClass="grey2_btn alignright"><span>+ Add Note</span></asp:LinkButton>
                            <at:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkAddNew" BackgroundCssClass="modalBackground"
                                DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="closepopup">
                            </at:ModalPopupExtender>
                        </div>
                        <div>
                            <asp:Panel ID="pnlNotes" runat="server" Style="display: none;">
                                <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                    left: 35%; top: 30%;">
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
                                                <div class="pp_content" style="height: 30px; display: block;">
                                                    <div class="pp_fade" style="display: block;">
                                                        <span class="noteIncentex" style="width: 80%; font-size: 12px; background-color: inherit;
                                                            color: Black; font-weight: bold;">
                                                            <img src="../Images/errorpage.png" height="25px" width="25px" alt="note:" />&nbsp;&nbsp;YOU
                                                            ARE ABOUT TO SEND A NOTE TO A CUSTOMER </span>
                                                        <div class="pp_details clearfix" style="width: 371px;">
                                                            <a href="#" id="closepopup" runat="server" class="pp_close">Close</a>
                                                            <p class="pp_description" style="display: none;">
                                                            </p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="pp_content_container" style="">
                                        <div class="pp_left" style="">
                                            <div class="pp_right" style="">
                                                <div class="pp_content" style="height: 228px; display: block;">
                                                    <div class="pp_loaderIcon" style="display: none;">
                                                    </div>
                                                    <div class="pp_fade" style="display: block;">
                                                        <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                        <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                                            <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                                style="visibility: visible;">previous</a>
                                                        </div>
                                                        <div id="pp_full_res">
                                                            <div class="pp_inline clearfix">
                                                                <div class="form_popup_box" style="padding-top: 15px;">
                                                                    <div class="label_bar">
                                                                        <span>Notes/History :
                                                                            <br />
                                                                            <br />
                                                                            <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtNote" runat="server"></asp:TextBox></span>
                                                                    </div>
                                                                    <div>
                                                                        <asp:LinkButton ID="lnkButton" runat="server" CommandName="SAVECACE" class="grey2_btn"
                                                                            OnClientClick="return CheckNoteHistory()" OnClick="lnkButton_Click"><span>Save Notes</span></asp:LinkButton>
                                                                    </div>
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
                        </div>
                        <br />
                        <br />
                        <br />
                        <%--End Note History--%>
                    </div>
                    <%--NOTES FOR INCENTEX EMPLOYEES--%>
                    <%if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
                      {%>
                    <div class="clearfix billing_head">
                        Notes/History for Incentex Employee :
                    </div>
                    <div>
                        <div class="form_table">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box taxt_area clearfix" style="height: 200px">
                                <span class="input_label alignleft" style="height: 200px">Internal Notes Only for IE
                                    :</span>
                                <div class="textarea_box alignright">
                                    <div class="scrollbar" style="height: 200px">
                                        <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                        </a>
                                    </div>
                                    <asp:TextBox ID="txtOrderNotesForIE" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                        Height="197px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                        <div class="alignnone spacer15">
                        </div>
                        <div class="rightalign gallery">
                            <asp:LinkButton ID="lnkDummyAddNewIE" class="grey2_btn alignright" runat="server"
                                Style="display: none"></asp:LinkButton>
                            <asp:LinkButton ID="lnkAddNewIE" CommandName="AddNotes" runat="server" OnClick="lnkAddNew_Click"
                                CssClass="grey2_btn alignright"><span>+ Add Note</span></asp:LinkButton>
                            <at:ModalPopupExtender ID="modalAddnotesForIE" TargetControlID="lnkAddNewIE" BackgroundCssClass="modalBackground"
                                DropShadow="true" runat="server" PopupControlID="pnlNotesForIE" CancelControlID="closepopupIE">
                            </at:ModalPopupExtender>
                        </div>
                        <div>
                            <asp:Panel ID="pnlNotesForIE" runat="server" Style="display: none;">
                                <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                    left: 35%; top: 30%;">
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
                                                <div class="pp_content" style="height: 30px; display: block;">
                                                    <div class="pp_fade" style="display: block;">
                                                        <span class="noteIncentex" style="width: 80%; font-size: 12px; background-color: inherit;
                                                            color: Black; font-weight: bold;">
                                                            <img src="../Images/errorpage.png" height="25px" width="25px" alt="note:" />&nbsp;&nbsp;SEND
                                                            NOTES TO IE ONLY</span>
                                                        <div class="pp_details clearfix" style="width: 371px;">
                                                            <a href="#" id="closepopupIE" runat="server" class="pp_close">Close</a>
                                                            <p class="pp_description" style="display: none;">
                                                            </p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="pp_content_container" style="">
                                        <div class="pp_left" style="">
                                            <div class="pp_right" style="">
                                                <div class="pp_content" style="height: 228px; display: block;">
                                                    <div class="pp_loaderIcon" style="display: none;">
                                                    </div>
                                                    <div class="pp_fade" style="display: block;">
                                                        <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                        <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                                            <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                                style="visibility: visible;">previous</a>
                                                        </div>
                                                        <div id="Div1">
                                                            <div class="pp_inline clearfix">
                                                                <div class="form_popup_box" style="padding-top: 15px;">
                                                                    <div class="label_bar">
                                                                        <span>Notes/History :
                                                                            <br />
                                                                            <br />
                                                                            <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtNoteForIE" runat="server"></asp:TextBox></span>
                                                                    </div>
                                                                    <div>
                                                                        <asp:LinkButton ID="linkbtnNotesForIE" runat="server" CommandName="SAVEIEs" class="grey2_btn"
                                                                            OnClientClick="return CheckNoteHistoryForIE()" OnClick="linkbtnNotesForIE_Click"><span>Save Notes</span></asp:LinkButton>
                                                                    </div>
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
                        </div>
                        <br />
                        <br />
                        <br />
                        <%--End Note History--%>
                    </div>
                    <%}
                      }
                      else
                      { %>
                    <div style="text-align: center">
                        <asp:Label ID="lblIsEmployeeMsg" runat="server" Text="You have no access for this tab."
                            CssClass="errormessage"></asp:Label>
                    </div>
                    <%} %>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
