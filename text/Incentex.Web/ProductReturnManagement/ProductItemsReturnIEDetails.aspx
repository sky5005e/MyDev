<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ProductItemsReturnIEDetails.aspx.cs" Inherits="ProductReturnManagement_ProductItemsReturnIEDetails"
    Title="Return Product >> For Incentex Employee" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <style type="text/css">
        .textarea_box textarea
        {
            height: 96px;
        }
        .textarea_box
        {
            height: 96px;
        }
        .textarea_box .scrollbar
        {
            height: 96px;
        }
        .fontsizesmall
        {
            font-size: small;
        }
        .noteIncentex
        {
            background-color: #E1E0E0;
            color: black;
            font-family: "Trebuchet MS" ,tahoma,arial,verdana;
            font-size: 0.8em;
            padding-bottom: 5px;
            padding-top: 7px;
        }
        .shipmentclass
        {
            background: none repeat scroll 0 0 #101010;
            color: #72757C;
            display: block;
            border: solid 1px #1c1c1c;
            padding: 0px 8px;
            border-left: none;
            border-right: solid 1px #1f1f1f;
        }
        .rightalign
        {
            font-size: small;
            color: #72757C;
            text-align: right;
        }
        .width300
        {
            font-size: small;
            width: 300px !important;
        }
    </style>

    <script type="text/javascript">

        // check if value is numeric in textbox
        function CheckNum(id) {
            var txt = document.getElementById(id);
            if (!IsNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.focus();
                txt.value = "";
            }


            var OrderQtyField = id.toString().substring(0, id.toString().indexOf("txtReceivedQty", 0) - 1);
            OrderQtyField = OrderQtyField + '_lblReturnQty';

            if (parseInt(txt.value) > parseInt(document.getElementById(OrderQtyField).innerHTML)) {
                alert("You can not enter Received qty more than actual qty " + document.getElementById(OrderQtyField).innerHTML + ".");
                txt.value = "";
                txt.focus();
                return;
            }

        }

        function IsNumeric(sText) {
            var ValidChars = "0123456789.";
            //var ValidChars = "0123456789";
            var IsNumber = true;
            var Char;


            for (i = 0; i < sText.length && IsNumber == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    IsNumber = false;
                }
            }
            return IsNumber;
        }

        function ValidateItemGrid(id) {
            var count = 2;
            var AtleastOne = false;
            for (var i = 0; i < $('#ctl00_ContentPlaceHolder1_gvProductReturn tr').length - 1; i++) {

                var Field = "ctl00_ContentPlaceHolder1_gvProductReturn_ctl";

                var ReceivedQtyField;
                if (count.toString().length == 1)
                    ReceivedQtyField = Field + '0' + count + '_txtReceivedQty';
                else
                    ReceivedQtyField = Field + count + '_txtReceivedQty';

                var ReturnStatusField;
                if (count.toString().length == 1)
                    ReturnStatusField = Field + '0' + count + '_ddlReturnStatus';
                else
                    ReturnStatusField = Field + count + '_ddlReturnStatus';

                if (document.getElementById(ReturnStatusField).value == 0 && document.getElementById(ReceivedQtyField).value != "") {
                    document.getElementById(ReturnStatusField).focus();
                    alert("Please Select the Return status for the item.");
                    return false;
                }
                if (document.getElementById(ReturnStatusField).value > 0 && document.getElementById(ReceivedQtyField).value == "") {
                    document.getElementById(ReceivedQtyField).focus();
                    alert("Please enter Received Qty for the item.");
                    return false;
                }
                if (document.getElementById(ReturnStatusField).value > 0 && document.getElementById(ReceivedQtyField).value != "")
                    AtleastOne = true;
                count = count + 1;
            }
            if (AtleastOne)
                return true;
            else {
                alert("Please enter data for at least one row.");
                return false;
            }
        }
        //End
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript" language="javascript">

        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();

            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({

            });


        });
    });
   
    </script>

    <div class="black_round_box">
        <div class="black2_round_top">
            <span></span>
        </div>
        <div class="black2_round_middle">
            <div class="form_pad">
                <div class="pro_search_pad" style="width: 940px;">
                    <mb:MenuUserControl ID="menucontrol" runat="server" />
                    <div>
                        <div style="text-align: center; color: Red; font-size: larger;">
                            <asp:Label ID="lblmsg" runat="server">
                            </asp:Label>
                        </div>
                        <div class="black_top_co">
                            <span>&nbsp;</span></div>
                        <div class="black_middle order_detail_pad">
                           <%-- <div class="black_top_co">
                                <span>&nbsp;</span></div>--%>
                           <%-- <div class="black_middle order_detail_pad">--%>
                                <table class="order_detail" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 50%">
                                            <table>
                                                <tr>
                                                    <td style="font-size: small;">
                                                        <label>
                                                            RA# :
                                                        </label>
                                                        <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: small;">
                                                        <label>
                                                            Customer Name :
                                                        </label>
                                                        <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 50%">
                                            <table class="order_detail">
                                                <tr>
                                                    <td style="font-size: small;">
                                                        <label style="padding-left: 28px!important;">
                                                            Submit Date :
                                                        </label>
                                                        <asp:Label runat="server" ID="lblSubmitDate"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: small;">
                                                        <label style="padding-left: 28px!important;">
                                                            Processed Date :
                                                        </label>
                                                        <asp:Label ID="lblProcessDate" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: small;">
                                                        <label style="padding-left: 28px!important;">
                                                            Status :
                                                        </label>
                                                        <asp:Label runat="server" ID="lblStatus"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </td>
                                    </tr>
                                </table>
                         
                            <div class="spacer15">
                            </div>
                            
                            <div class="spacer15">
                            </div>
                        </div>
                        <div class="black_bot_co">
                            <span>&nbsp;</span></div>
                        <div class="spacer15">
                        </div>
                        <table width="100%">
                            <tr>
                                <td style="width: 80%; vertical-align: middle;">
                                    <div>
                                        <div class="alignleft" style="width: 100%;">
                                            <span>Product Return Summary:</span></div>
                                    </div>
                                    <div class="spacer20">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvProductReturn" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvProductReturn_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField SortExpression="ReturnQty">
                                                <HeaderTemplate>
                                                    <span>Qty Returned</span>
                                                    <div class="corner">
                                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                    </div>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnProductReturnId" runat="server" Value='<%# Eval("ProductReturnId") %>' />
                                                    <asp:Label runat="server" ID="lblReturnQty" Text='<%# Eval("ReturnQty") %>' />
                                                     <div class="corner">
                                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ItemNumber">
                                                <HeaderTemplate>
                                                    <span>Item #</span>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ProductDescrption">
                                                <HeaderTemplate>
                                                    <span>Description</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductionDescription" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box" Width="30%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ReceivedQty">
                                                <HeaderTemplate>
                                                    <span>Received Qty</span>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <span class="btn_space">
                                                        <asp:TextBox ID="txtReceivedQty" runat="server" onchange="CheckNum(this.id)" Style="background-color: #303030;
                                                            border: medium none; color: #FFFFFF; padding: 2px; width: 100%; text-align: center;"></asp:TextBox>
                                                    </span>
                                                    <asp:HiddenField ID="hdnshippid" runat="server" Value='<%# Eval("ShippID") %>' />
                                                    <asp:HiddenField ID="hdnitemNo" runat="server" Value='<%# Eval("ItemNumber") %>' />
                                                    <asp:HiddenField ID="hdnShoppingCartId" runat="server" Value='<%# Eval("MyShoppingCartId") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box centeralign" Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ReasonCode">
                                                <HeaderTemplate>
                                                    <span>Reason Code</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReason" runat="server" Text='<%# Eval("Reason") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ReturnStatus">
                                                <HeaderTemplate>
                                                    <span>Status</span>
                                                     <div class="corner" >
                                                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                                        </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span style="height: 26px;">
                                                        <asp:DropDownList runat="server" ID="ddlReturnStatus" runat="server" Style="margin-top: 3px;
                                                            background-color: #303030; border: #303030; width: 100%; color: #72757C;">
                                                        </asp:DropDownList>
                                                    </span>
                                                     <div class="corner" >
                                                        <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box" Width="20%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <div class="spacer20">
                        </div>
                        <table>
                            <tr>
                                <td colspan="3">
                                    <div class="botbtn centeralign">
                                        <asp:LinkButton ID="lnkSaveOrderDetails" CommandName="OrderDetails" class="grey2_btn saveShipDetails"
                                            OnClientClick="return ValidateItemGrid(this.id);" runat="server" OnClick="lnkSaveOrderDetails_Click"><span>Return Processed</span></asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="spacer20">
                        </div>
                        <div>
                            <div class="form_table" id="dvNotesHistory" runat="server">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box taxt_area clearfix" style="height: 100px">
                                    <span class="input_label alignleft" style="height: 100px">Notes/History :</span>
                                    <div class="textarea_box alignright">
                                        <div class="scrollbar" style="height: 103px">
                                            <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                                class="scrollbottom"></a>
                                        </div>
                                        <asp:TextBox ID="txtOrderNotesForCECA" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                            Height="100px" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                            <div class="alignnone spacer15">
                            </div>
                            <div class="rightalign gallery" id="divAddNots" runat="server">
                                <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                                <asp:LinkButton ID="lnkAddNew" CommandName="AddNotes" runat="server" CssClass="grey2_btn alignright"><span>+ Add Note</span></asp:LinkButton>
                                <at:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkAddNew" BackgroundCssClass="modalBackground"
                                    DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="closepopup">
                                </at:ModalPopupExtender>
                            </div>
                            <div>
                                <asp:Panel ID="pnlNotes" runat="server">
                                    <div class="pp_pic_holder facebook" style="display: block; width: 411px;">
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
                        <%--NewLy Added--%>
                        <div class="spacer20">
                        </div>
                        <div>
                            <div class="form_table" id="dvInternalNotes" runat="server">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box taxt_area clearfix" style="height: 100px">
                                    <span class="input_label alignleft" style="height: 100px">Incentex Internal Notes Only</span>
                                    <div class="textarea_box alignright">
                                        <div class="scrollbar" style="height: 103px">
                                            <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                            </a>
                                        </div>
                                        <asp:TextBox ID="txtOrderNotesForIE" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                            Height="100px" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                            <div class="alignnone spacer15">
                            </div>
                            <div class="rightalign gallery" id="div1" runat="server">
                                <asp:LinkButton ID="lnkDummyAddNewIE" class="grey2_btn alignright" runat="server"
                                    Style="display: none"></asp:LinkButton>
                                <asp:LinkButton ID="lnkAddNewIE" CommandName="AddNotes" runat="server" CssClass="grey2_btn alignright"><span>+ Add Note</span></asp:LinkButton>
                                <at:ModalPopupExtender ID="modalAddnotesIE" TargetControlID="lnkAddNewIE" BackgroundCssClass="modalBackground"
                                    DropShadow="true" runat="server" PopupControlID="pnlNotesIE" CancelControlID="A5">
                                </at:ModalPopupExtender>
                            </div>
                            <div>
                                <asp:Panel ID="pnlNotesIE" runat="server">
                                    <div class="pp_pic_holder facebook" style="display: block; width: 411px;">
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
                                                    <div class="pp_content" style="height: 45px; display: block;">
                                                        <div class="pp_fade" style="display: block;">
                                                            <span class="noteIncentex" style="width: 80%; font-size: 12px; background-color: inherit;
                                                                color: Black; font-weight: bold;">
                                                                <img src="../Images/errorpage.png" height="25px" width="25px" alt="note:" />&nbsp;&nbsp;
                                                                You are about to post an Incentex Internal Note.
                                                                <br />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Customer will not be
                                                                able to view this note. </span>
                                                            <div class="pp_details clearfix" style="width: 371px;">
                                                                <a href="#" id="A5" runat="server" class="pp_close">Close</a>
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
                                                            <div id="Div2">
                                                                <div class="pp_inline clearfix">
                                                                    <div class="form_popup_box" style="padding-top: 15px;">
                                                                        <div class="label_bar">
                                                                            <span>Incentex Employee Notes/History :
                                                                                <br />
                                                                                <br />
                                                                                <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtNoteIE" runat="server"></asp:TextBox></span>
                                                                        </div>
                                                                        <div>
                                                                            <asp:LinkButton ID="lnkNoteHisForIE" runat="server" CommandName="SAVECACE" class="grey2_btn"
                                                                                OnClientClick="return CheckNoteHistory()" OnClick="lnkNoteHisForIE_Click"><span>Save Notes</span></asp:LinkButton>
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
                        <%--END--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="black2_round_bottom">
            <span></span>
        </div>
        <div class="alignnone">
            &nbsp;</div>
    </div>
    <%--Modal Popup Panels--%>
</asp:Content>
