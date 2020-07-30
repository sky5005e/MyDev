<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ServiceTicketDetail.aspx.cs" Inherits="admin_ServiceTicketCenter_ServiceTicketDetail"
    Title="Support Ticket Detail" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
        .form_table .calender_l .ui-datepicker-trigger
        {
            top: -2px;
        }
        .form_table input
        {
            font-size: 15px;
            height: 16px;
            color: #72757C;
        }
        select
        {
            width: 100%;
        }
        .fileinput
        {
            float: left;
            margin-right: 10px;
            width: 145px;
        }
        .fileinput input
        {
            width: 130px;
            padding: 8px 0px;
            color: #fff;
        }
        .custom-checkbox input, .custom-checkbox_checked input
        {
            width: 20px;
            height: 20px;
            margin-left: -8px;
        }
        .approval_manage .form
        {
            background: url(   "../../Images/approvalicon.png" ) no-repeat scroll left 18px transparent;
            padding-left: 100px;
            width: 50%;
        }
        .subownername
        {
            color: #72757C;
        }
        .order_detail td
        {
            font-size: 15px;
            color: #72757c;
            line-height: 20px;
            padding-bottom: 0px !important;
            text-align: left !important;
        }
        .order_detail label
        {
            color: #B0B0B0;
            padding-right: 6px;
            padding-left: 20px;
        }
        .st_detail td
        {
            font-size: 15px;
            color: #72757c;
        }
        .st_detail label
        {
            color: #B0B0B0;
        }
        .custom-sel select option
        {
            font-size: 13px;
            padding: 5px 3px;
            color: #000;
            background: #a4a4a4;
            line-height: 22px;
        }
        .custom-sel span.slc span.src
        {
            float: left;
            line-height: 28px;
            color: #B0B0B0;
            padding: 0 15px 0 0;
            display: block;
            height: 28px;
            font-size: 14px;
            background: url(../images/sel-bg.gif) right -28px no-repeat;
            _visibility: hidden;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
    </script>

    <script type="text/javascript" language="javascript">
        function HideTodo(id, lnk, ownid, detailid) {
            if (document.getElementById(id).checked == true) {
                document.getElementById('ctl00_ContentPlaceHolder1_hdnTempID').value = ownid;
            }
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_hdnTempID').value = detailid;
            }
        }

        function UpdateToDoStatus(todoid) {
            document.getElementById('ctl00_ContentPlaceHolder1_hdnTempTodoID').value = todoid;
        }
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $('#dvLoader').hide();
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtTodo: { required: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtTodo: { required: replaceMessageString(objValMsg, "Required", "to-do") }
                    },
                    errorPlacement: function(error, element) {
                        error.insertAfter(element);
                    }
                });
            });

            $("#ctl00_ContentPlaceHolder1_btnAddToDo").click(function() {
                if ($("#aspnetForm").valid()) {
                    $('#dvLoader').show();
                    return true;
                }
                else {
                    return false;
                }
            });

            $("#ctl00_ContentPlaceHolder1_btnAddItem").click(function() {
                var fileinput = document.getElementById('ctl00_ContentPlaceHolder1_fpAttachment');
                if (!fileinput.files[0]) {
                    alert("Please select file.");
                    return false;
                }
                return true;
            });

            $('#ctl00_ContentPlaceHolder1_txtNote').live('keydown', function(e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode == 9) {
                    e.preventDefault();
                    $('#ctl00_ContentPlaceHolder1_lnkButton').focus();
                }
            });

            $('#ctl00_ContentPlaceHolder1_txtNoteIE').live('keydown', function(e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode == 9) {
                    e.preventDefault();
                    $('#ctl00_ContentPlaceHolder1_lnkNoteHisForIE').focus();
                }
            });

            var prev_val2;

            $('#ctl00_ContentPlaceHolder1_ddlServiceTicketStatus').focus(function() {
                prev_val2 = $(this).val();
            }).change(function() {
                $(this).blur() // Firefox fix as suggested by AgDude
                var success = confirm('Are you sure, you want to change status to ' + $('#ctl00_ContentPlaceHolder1_ddlServiceTicketStatus :selected').text() + '?');
                if (success) {
                    //alert('changed');
                    // Other changed code would be here...
                }
                else {
                    $(this).val(prev_val2);
                    //alert('unchanged');
                    return false;
                }
            });

            var prev_val3;

            $('#ctl00_ContentPlaceHolder1_ddlTicketOwner').focus(function() {
                prev_val3 = $(this).val();
            }).change(function() {
                $(this).blur() // Firefox fix as suggested by AgDude
                var success = confirm('Are you sure, you want to change ticket owner to ' + $('#ctl00_ContentPlaceHolder1_ddlTicketOwner :selected').text() + '?');
                if (success) {
                    //alert('changed');
                    // Other changed code would be here...
                }
                else {
                    $(this).val(prev_val3);
                    //alert('unchanged');
                    return false;
                }
            });
            
            $("#ctl00_ContentPlaceHolder1_txtDatePromised").click(function() {
                $(this).select();
            });
            
            $("#ctl00_ContentPlaceHolder1_txtDatePromised").keypress(function(e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode == 46) {                    
                    return true;
                }
                else {
                    return false;
                }
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkButton").click(function() {
                $('#dvLoader').show();
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkButton").click(function() {
                $('#dvLoader').show();
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkNoteHisForIE").click(function() {
                $('#dvLoader').show();
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkNoteSupp").click(function() {
                $('#dvLoader').show();
            });
            
            $(window).scroll(function () {              
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
        });
    </script>

    <script type="text/javascript" language="javascript">        
        $(function() {
            scrolltextarea(".scrollme1", "#ScrollTop1", "#ScrollBottom1");
        });
    </script>

    <script type="text/javascript" language="javascript">
        $(function() {
            $(".datepicker1").datepicker({                
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true,
                minDate: new Date()
            });
        });
    </script>

    <script type="text/javascript">
        $(window).load(function() {
            $("#dvLoader").hide();
        });
    </script>

    <link media="screen" rel="stylesheet" href="../../CSS/colorbox.css" />
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div class="pro_search_pad" style="width: 800px;">
            <div style="text-align: center;">
                <asp:UpdatePanel ID="upMsg" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="spacer10">
            </div>
            <div id="mainDIV">
                <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
                <div>
                    <div class="black_top_co">
                        <span>&nbsp;</span></div>
                    <div class="black_middle order_detail_pad">
                        <table class="order_detail" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 50%">
                                    <table>
                                        <tr>
                                            <td>
                                                <label>
                                                    Ticket Number :
                                                </label>
                                                <asp:Label ID="lblServiceTicketNumber" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="vertical-align: middle;">
                                                    <label style="float: left;">
                                                        Ticket Name :
                                                    </label>
                                                </div>
                                                <div id="txtName">
                                                    <div class="form_table select_box_pad" style="width: 40%;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <div class="form_top_co">
                                                                        <span>&nbsp;</span></div>
                                                                    <div class="form_box" style="height: 15px;">
                                                                        <asp:TextBox ID="txtServiceTicketName" runat="server" Style="line-height: 13px; vertical-align: middle;"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form_bot_co">
                                                                        <span>&nbsp;</span></div>
                                                                </td>
                                                                <td valign="middle" style="vertical-align: middle;">
                                                                    <asp:LinkButton ID="imgBtnChangeTicketName" runat="server" CssClass="alignright"
                                                                        ToolTip="Change Ticket Name" OnClick="imgBtnChangeTicketName_Click"><img src="../../Images/green.png" alt="Apply" />
                                                                    </asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Ticket Owner :
                                                </label>
                                                <asp:Label ID="lblServiceTicketOwner" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label style="float: left;">
                                                    Date Needed :
                                                </label>
                                                <div class="form_table select_box_pad" style="width: 145px;">
                                                    <div class="calender_l">
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <asp:TextBox ID="txtDatePromised" runat="server" Style="width: 80px;" CssClass="datepicker1"
                                                                OnTextChanged="txtDatePromised_TextChanged" AutoPostBack="true" AutoCompleteType="None"></asp:TextBox>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 50%">
                                    <table>
                                        <tr>
                                            <td>
                                                <label style="padding-left: 28px!important;" id="lblCustSupp" runat="server">
                                                    Customer :
                                                </label>
                                                <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trContact" runat="server">
                                            <td>
                                                <label style="padding-left: 28px!important;">
                                                    Contact :
                                                </label>
                                                <asp:Label runat="server" ID="lblContact"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trCustomerEmail" runat="server">
                                            <td>
                                                <label style="padding-left: 28px!important;" id="lblCustSuppEmail" runat="server">
                                                    Customer Email :
                                                </label>
                                                <asp:Label ID="lblCustomerEmail" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trTelephone" runat="server">
                                            <td>
                                                <label style="padding-left: 28px!important;">
                                                    Telephone :
                                                </label>
                                                <asp:Label ID="lblTelephone" runat="server"></asp:Label>
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
                </div>
            </div>
        </div>
        <div class="pro_search_pad" style="width: 800px;">
            <table class="form_table st_detail">
                <tr>
                    <td valign="middle" style="vertical-align: middle; width: 50%;">
                        <div>
                            <label style="color: #B0B0B0;">
                                Start Date :
                            </label>
                            <asp:Label ID="lblStartDate" runat="server" Style="color: #72757C;"></asp:Label>
                        </div>
                    </td>
                    <td style="width: 50%">
                        <asp:UpdatePanel ID="upTicketOwner" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="ddlTicketOwner" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="custom-sel">
                                        <asp:DropDownList ID="ddlTicketOwner" TabIndex="7" onchange="pageLoad(this,value);"
                                            runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTicketOwner_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" style="vertical-align: middle; width: 50%;">
                        <div>
                            <label style="color: #B0B0B0;">
                                Close Date :
                            </label>
                            <asp:Label ID="lblEndDate" runat="server" Style="color: #72757C;"></asp:Label>
                        </div>
                    </td>
                    <td style="width: 50%">
                        <asp:UpdatePanel ID="upServiceTicketStatus" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="ddlServiceTicketStatus" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="custom-sel">
                                        <asp:DropDownList ID="ddlServiceTicketStatus" TabIndex="7" onchange="pageLoad(this,value);"
                                            runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlServiceTicketStatus_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" style="vertical-align: middle; width: 50%;">
                        <table>
                            <tr>
                                <td style="width: 10%;">
                                    <label style="color: #B0B0B0;">
                                        Flag :
                                    </label>
                                </td>
                                <td style="width: 90%;">
                                    <asp:LinkButton ID="lnkFlag" runat="server" OnClick="lnkFlag_Click">
                                        <img alt="Flag" src="" runat="server" id="imgFlag" /></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%">
                        <asp:UpdatePanel ID="upTypeOfRequest" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="ddlTypeOfRequest" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="custom-sel">
                                        <asp:DropDownList ID="ddlTypeOfRequest" TabIndex="7" onchange="pageLoad(this,value);"
                                            runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTypeOfRequest_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 800px; margin: 0 auto;">
            <dl>
                <dd style="float: left;">
                    <a href='<%= Convert.ToString("AssignSubOwners.aspx?id=") + Convert.ToString(Request.QueryString["id"]) %>'
                        title="Assign Sub Owners">
                        <img src="../../Images/approvalicon.png" alt="Assign Sub Owners" /></a>
                </dd>
                <dd style="float: left; margin-left: 20px; width: 85%;">
                    <div  >
                        <h4 style="float: left; margin-right: 7px; color: #B0B0B0;">
                            Owner :</h4>
                        <asp:LinkButton ID="lnkOwnerToDo" runat="server" CssClass="subownername" Style="float: left;
                            font-size: 15px;" OnClick="lnkOwnerToDo_Click" />
                    </div>
                    <div style="float: right; margin-left: 20px; width: 57%;">
                        <h4 style="float: left; margin-right: 7px; color: #B0B0B0;">
                            Reason :</h4>
                        <asp:Label ID="lblServiceTicketReason" runat="server" style="float: left; color: #72757C; font-size: 15px;"></asp:Label>
                    </div>
                    <div style="clear: both;">
                        <h4 style="margin-bottom: 0px; color: #B0B0B0;">
                            Sub Owners :</h4>
                        <table>
                            <tr>
                                <td style="width: 50%;">
                                    <div class="checktable_supplier true">
                                        <h4 style="color: #B0B0B0;">
                                            Incentex Employees</h4>
                                        <asp:DataList ID="dtlIESubOwners" runat="server" OnItemCommand="dtlSubOwners_ItemCommand"
                                            RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <ItemTemplate>
                                                <asp:LinkButton CssClass="subownername" ID="lnkSubOwnerToDo" runat="server" CommandName="ToDo"
                                                    CommandArgument='<%# Eval("SubOwnerID") %>' Text='<%# Convert.ToString(Eval("FirstName")) + " " + Convert.ToString(Eval("LastName")) %>'
                                                    Style="font-size: 15px; margin-right: 50px;" />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </div>
                                </td>
                                <td style="width: 50%;">
                                    <div class="checktable_supplier true">
                                        <h4 style="color: #B0B0B0;" id="h4CASupp" runat="server">
                                            Company Admins</h4>
                                        <asp:DataList ID="dtlCASubOwners" runat="server" OnItemCommand="dtlSubOwners_ItemCommand"
                                            RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <ItemTemplate>
                                                <asp:LinkButton CssClass="subownername" ID="lnkSubOwnerToDo" runat="server" CommandName="ToDo"
                                                    CommandArgument='<%# Eval("SubOwnerID") %>' Text='<%# Convert.ToString(Eval("FirstName")) + " " + Convert.ToString(Eval("LastName")) %>'
                                                    Style="font-size: 15px; margin-right: 50px;" />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </dd>
            </dl>
        </div>
    </div>
    <div class="alignnone spacer25">
    </div>
    <div class="pro_search_pad" style="width: 800px; clear: both;" id="dvCustomerNote"
        runat="server">
        <div class="form_table">
            <div class="form_top_co">
                <span>&nbsp;</span></div>
            <div class="form_box taxt_area clearfix" style="height: 180px;">
                <span class="input_label alignleft" style="height: 130px; font-size: 13px; width: 20%!important;
                    padding-top: 50px;">Notes/History :</span>
                <div class="textarea_box alignright" style="width: 78%;">
                    <div class="scrollbar" style="height: 183px">
                        <a href="#scroll" id="Scrolltop" class="scrolltop"></a><a href="#scroll" id="ScrollBottom"
                            class="scrollbottom"></a>
                    </div>
                    <asp:TextBox ID="txtNotesForCECA" runat="server" TextMode="MultiLine" CssClass="scrollme"
                        Height="180px" Style="font-size: 13px; color: #B0B0B0;" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="form_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div class="alignnone spacer15">
        </div>
        <div class="rightalign gallery" id="divAddNotes" runat="server">
            <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
            <asp:LinkButton ID="lnkAddNew" CommandName="AddNotes" runat="server" CssClass="alignright">
                <img alt="+ Add Customer View Note" src="../../Images/new_red_btn.png" id="imgAddNoteCustSupp"
                    runat="server" /></asp:LinkButton>
            <asp:HiddenField ID="hdnAddCustNoteClicked" runat="server" Value="0" />
            <at:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkAddNew" BackgroundCssClass="modalBackground"
                DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="closepopup">
            </at:ModalPopupExtender>
        </div>
        <div>
            <asp:Panel ID="pnlNotes" runat="server" Style="display: none;">
                <div class="pp_pic_holder facebook" id="dvCustNotefacebook" runat="server" style="display: block;
                    width: 411px; height: 530px; position: fixed; left: 35%; top: 2%;">
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
                                            color: Black; font-weight: bold;" id="spanNoteCustSupp" runat="server">
                                            <img src="../../Images/errorpage.png" height="25px" width="25px" alt="note:" />&nbsp;&nbsp;YOU
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
                                <div class="pp_content" id="dvCustNotepp_Content" runat="server" style="height: 520px;
                                    display: block;">
                                    <div class="pp_loaderIcon" style="display: none;">
                                    </div>
                                    <div class="pp_fade" style="display: block;">
                                        <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                        <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                            <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                style="visibility: visible;">previous</a>
                                        </div>
                                        <div class="pp_full_res">
                                            <div class="pp_inline clearfix">
                                                <div class="form_popup_box" style="padding-top: 15px;">
                                                    <div class="label_bar">
                                                        <span>Notes/History :
                                                            <br />
                                                            <br />
                                                            <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtNote" runat="server"></asp:TextBox></span>
                                                    </div>
                                                    <div style="text-align: center;">
                                                        <span style="width: 100%; font-size: 13px; background-color: inherit; color: Black;
                                                            font-weight: bold;">Send email notification to:</span>
                                                    </div>
                                                    <div>
                                                        <div id="dvCustomer" runat="server">
                                                            <span style="width: 80%; font-size: 12px; background-color: inherit; color: Black;
                                                                font-weight: bold;" id="spanCustSupp" runat="server">Customer</span>
                                                            <br />
                                                            <br />
                                                            <div>
                                                                <table class="true">
                                                                    <tr>
                                                                        <td style="float: left; margin-right: 10px;">
                                                                            <span class="custom-checkbox alignleft" id="spanCustomer" runat="server">
                                                                                <asp:CheckBox ID="chkCustomer" runat="server" />
                                                                            </span>
                                                                        </td>
                                                                        <td style="float: left; margin-top: 3px;">
                                                                            <asp:Label ID="lblSendEmailToCustomer" runat="server" Style="font-size: 12px; background-color: inherit;
                                                                                color: Black; text-align: left;"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:HiddenField ID="hdnCompanyID" runat="server" />
                                                                <asp:HiddenField ID="hdnCustomerID" runat="server" />
                                                                <asp:HiddenField ID="hdnCustomerEmail" runat="server" />
                                                                <asp:HiddenField ID="hdnSupplierID" runat="server" />
                                                                <asp:HiddenField ID="hdnSupplierEmail" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div id="dvCustAdmins" runat="server">
                                                            <br />
                                                            <span style="width: 80%; font-size: 12px; background-color: inherit; color: Black;
                                                                font-weight: bold;" id="spanCustAdminsSuppEmp" runat="server">Company Admins</span>
                                                            <br />
                                                            <br />
                                                            <div style="overflow: auto; height: 75px;">
                                                                <asp:DataList ID="dtlCustNoteCAs" runat="server" CssClass="true" RepeatDirection="Horizontal"
                                                                    RepeatColumns="2" RepeatLayout="Table" OnItemDataBound="dtlCustNoteCAs_ItemDataBound">
                                                                    <ItemTemplate>
                                                                        <table>
                                                                            <tr>
                                                                                <td style="float: left; margin-right: 10px;">
                                                                                    <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                                                                        <asp:CheckBox ID="chkCustNoteCAs" runat="server" />
                                                                                    </span>
                                                                                </td>
                                                                                <td style="float: left; margin-top: 3px;">
                                                                                    <asp:Label ID="lblUserNameCA" runat="server" Text='<%# Convert.ToString(Eval("FirstName")) + " " + Convert.ToString(Eval("LastName")) %>'
                                                                                        Style="font-size: 12px; background-color: inherit; color: Black; text-align: left;"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:HiddenField ID="hdnCustNoteCARecipientID" runat="server" Value='<%# Eval("RecipientsDetailID") %>' />
                                                                        <asp:HiddenField ID="hdnCustNoteCAUserID" runat="server" Value='<%# Eval("UserInfoID") %>' />
                                                                        <asp:HiddenField ID="hdnCustNoteCAEmail" runat="server" Value='<%# Eval("Email") %>' />
                                                                        <asp:HiddenField ID="hdnCustNoteCAFlag" runat="server" Value='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("SubscriptionFlag"))) ? Eval("SubscriptionFlag") : "false" %>' />
                                                                    </ItemTemplate>
                                                                </asp:DataList>
                                                            </div>
                                                        </div>
                                                        <div>
                                                            <br />
                                                            <span style="width: 80%; font-size: 12px; background-color: inherit; color: Black;
                                                                font-weight: bold;">Incentex Employees</span>
                                                            <br />
                                                            <br />
                                                            <div style="overflow: auto; height: 75px;">
                                                                <asp:DataList ID="dtlCustNoteIEs" runat="server" CssClass="true" RepeatDirection="Horizontal"
                                                                    RepeatColumns="2" RepeatLayout="Table" OnItemDataBound="dtlCustNoteIEs_ItemDataBound">
                                                                    <ItemTemplate>
                                                                        <table>
                                                                            <tr>
                                                                                <td style="float: left; margin-right: 10px;">
                                                                                    <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                                                                        <asp:CheckBox ID="chkCustNoteIEs" runat="server" />
                                                                                    </span>
                                                                                </td>
                                                                                <td style="float: left; margin-top: 3px;">
                                                                                    <asp:Label ID="lblUserNameIE" runat="server" Text='<%# Convert.ToString(Eval("FirstName")) + " " + Convert.ToString(Eval("LastName")) %>'
                                                                                        Style="font-size: 12px; background-color: inherit; color: Black; text-align: left;"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:HiddenField ID="hdnCustNoteIERecipientID" runat="server" Value='<%# Eval("RecipientsDetailID") %>' />
                                                                        <asp:HiddenField ID="hdnCustNoteIEUserID" runat="server" Value='<%# Eval("UserInfoID") %>' />
                                                                        <asp:HiddenField ID="hdnCustNoteIEEmail" runat="server" Value='<%# Eval("Email") %>' />
                                                                        <asp:HiddenField ID="hdnCustNoteIEFlag" runat="server" Value='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("SubscriptionFlag"))) ? Eval("SubscriptionFlag") : "false" %>' />
                                                                    </ItemTemplate>
                                                                </asp:DataList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div>
                                                        <asp:LinkButton ID="lnkButton" runat="server" class="grey2_btn alignright" OnClick="lnkButton_Click">
                                                        <span>Save Note</span></asp:LinkButton>
                                                    </div>
                                                    <div>
                                                        <asp:LinkButton ID="lnkSaveCustNoteRecipients" runat="server" class="grey2_btn alignleft"
                                                            OnClick="lnkSaveRecipients_Click"><span>Save Recipients</span></asp:LinkButton>
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
    <div class="pro_search_pad" style="width: 800px; clear: both;">
        <div class="form_table">
            <div class="form_top_co">
                <span>&nbsp;</span></div>
            <div class="form_box taxt_area clearfix" style="height: 280px;">
                <span class="input_label" style="height: 180px; font-size: 12px; width: 20%!important;
                    padding-top: 100px;" id="spanNoteLabel" runat="server">Incentex Internal Notes Only</span>
                <div class="textarea_box alignright" style="width: 78%;">
                    <div class="scrollbar" style="height: 283px">
                        <a href="#scroll" id="ScrollTop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                            class="scrollbottom"></a>
                    </div>
                    <asp:UpdatePanel ID="upNotesForIE" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="txtDatePromised" />
                            <asp:PostBackTrigger ControlID="chkPostedNotes" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:TextBox ID="txtNotesForIE" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                Height="280px" Style="font-size: 13px; color: #B0B0B0;" ReadOnly="true"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="form_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div class="alignnone spacer15">
        </div>
        <div class="rightalign gallery" id="divAddInternalNotes" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkAttachments" runat="server" CssClass="grey2_btn alignleft">
                            <span id="atSpan" runat="server">Attachments</span></asp:LinkButton>
                        <asp:LinkButton ID="lnkDummyAttachments" class="grey2_btn alignleft" runat="server"
                            Style="display: none"></asp:LinkButton>
                        <at:ModalPopupExtender ID="modalAttachments" TargetControlID="lnkAttachments" BackgroundCssClass="modalBackground"
                            DropShadow="true" runat="server" PopupControlID="pnlAttachments" CancelControlID="closeattchement">
                        </at:ModalPopupExtender>
                    </td>
                    <td>
                        <table class="true" id="tblPostedNotesOnly" runat="server">
                            <tr>
                                <td>
                                    <span id="spanPostedNotes" runat="server" class="custom-checkbox alignleft">
                                        <asp:CheckBox ID="chkPostedNotes" runat="server" OnCheckedChanged="chkPostedNotes_CheckedChanged"
                                            AutoPostBack="true" />
                                    </span>
                                </td>
                                <td style="vertical-align: middle;">
                                    Show Only Posted Notes
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkDummyAddNewIE" class="grey2_btn alignright" runat="server"
                            Style="display: none"></asp:LinkButton>
                        <asp:LinkButton ID="lnkAddNewIE" runat="server" CssClass="grey2_btn alignright">
                            <span id="spanAddNote" runat="server">+ Add Internal Company Note</span></asp:LinkButton>
                        <at:ModalPopupExtender ID="modalAddnotesIE" TargetControlID="lnkAddNewIE" BackgroundCssClass="modalBackground"
                            DropShadow="true" runat="server" PopupControlID="pnlNotesIE" CancelControlID="closeIEPopup">
                        </at:ModalPopupExtender>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:Panel ID="pnlNotesIE" runat="server" Style="display: none;">
                <div class="pp_pic_holder facebook" style="display: block; width: 411px; height: 365px;
                    position: fixed; left: 35%; top: 15%;">
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
                                            color: Black; font-weight: bold;" id="spanIENote" runat="server">
                                            <img src="../../Images/errorpage.png" height="25px" width="25px" alt="note:" />&nbsp;&nbsp;
                                            You are about to post an Incentex Internal Note.
                                            <br />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Customer will not be
                                            able to view this note. </span>
                                        <div class="pp_details clearfix" style="width: 371px;">
                                            <a href="#" id="closeIEPopup" runat="server" class="pp_close">Close</a>
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
                                <div class="pp_content" style="height: 365px; display: block;">
                                    <div class="pp_loaderIcon" style="display: none;">
                                    </div>
                                    <div class="pp_fade" style="display: block;">
                                        <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                        <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                            <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                style="visibility: visible;">previous</a>
                                        </div>
                                        <div class="pp_full_res">
                                            <div class="pp_inline clearfix">
                                                <div class="form_popup_box" style="padding-top: 15px;">
                                                    <div class="label_bar">
                                                        <span>Incentex Employee Notes/History :
                                                            <br />
                                                            <br />
                                                            <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtNoteIE" runat="server"></asp:TextBox></span>
                                                    </div>
                                                    <div style="text-align: center;">
                                                        <span style="width: 100%; font-size: 13px; background-color: inherit; color: Black;
                                                            font-weight: bold;">Send email notification to:</span>
                                                    </div>
                                                    <div>
                                                        <br />
                                                        <span style="width: 80%; font-size: 12px; background-color: inherit; color: Black;
                                                            font-weight: bold;">Incentex Employees</span>
                                                        <br />
                                                        <br />
                                                        <div style="overflow: auto; height: 75px;">
                                                            <asp:DataList ID="dtlIENoteIEs" runat="server" CssClass="true" RepeatDirection="Horizontal"
                                                                RepeatColumns="2" RepeatLayout="Table" OnItemDataBound="dtlIENoteIEs_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <td style="float: left; margin-right: 10px;">
                                                                                <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                                                                    <asp:CheckBox ID="chkIENoteIEs" runat="server" />
                                                                                </span>
                                                                            </td>
                                                                            <td style="float: left; margin-top: 3px;">
                                                                                <asp:Label ID="lblUserNameIE" runat="server" Text='<%# Convert.ToString(Eval("FirstName")) + " " + Convert.ToString(Eval("LastName")) %>'
                                                                                    Style="font-size: 12px; background-color: inherit; color: Black; text-align: left;"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:HiddenField ID="hdnIENoteIERecipientID" runat="server" Value='<%# Eval("RecipientsDetailID") %>' />
                                                                    <asp:HiddenField ID="hdnIENoteIEUserID" runat="server" Value='<%# Eval("UserInfoID") %>' />
                                                                    <asp:HiddenField ID="hdnIENoteIEEmail" runat="server" Value='<%# Eval("Email") %>' />
                                                                    <asp:HiddenField ID="hdnIENoteIEFlag" runat="server" Value='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("SubscriptionFlag"))) ? Eval("SubscriptionFlag") : "false" %>' />
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div>
                                                        <asp:LinkButton ID="lnkNoteHisForIE" runat="server" CommandName="SAVECACE" class="grey2_btn alignright"
                                                            OnClick="lnkNoteHisForIE_Click"><span>Save Notes</span></asp:LinkButton>
                                                    </div>
                                                    <div>
                                                        <asp:LinkButton ID="lnkSaveIntNoteRecipients" runat="server" class="grey2_btn alignleft"
                                                            OnClick="lnkSaveRecipients_Click"><span>Save Recipients</span></asp:LinkButton>
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
        <div>
            <asp:Panel ID="pnlAttachments" runat="server" Style="display: none;">
                <div class="cboxWrapper" style="display: block; width: 558px; height: 549px; position: fixed;
                    left: 30%; top: 8%;">
                    <div style="">
                        <div class="cboxTopLeft" style="float: left;">
                        </div>
                        <div class="cboxTopCenter" style="float: left; width: 508px;">
                        </div>
                        <div class="cboxTopRight" style="float: left;">
                        </div>
                    </div>
                    <div style="clear: left;">
                        <div class="cboxMiddleLeft" style="float: left; height: 483px;">
                        </div>
                        <div class="cboxContent" style="float: left; width: 508px; display: block; height: 483px;">
                            <div class="cboxLoadedContent" style="display: block; overflow: visible;">
                                <div style="padding: 25px 10px 10px 10px;">
                                    <div style="height: 383px; overflow: auto;">
                                        <div style="text-align: center; color: Red; font-size: 14px;">
                                            <asp:Label ID="lblAttachmentMsg" runat="server">
                                            </asp:Label>
                                        </div>
                                        <asp:GridView ID="grvAttachment" runat="server" Width="100%" HeaderStyle-CssClass="ord_header"
                                            CssClass="orderreturn_box" GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                                            OnRowCommand="grvAttachment_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>File Name </span>
                                                        <div class="corner">
                                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span class="first">
                                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="" CommandName="download"><%#Eval("OnlyFileName")%></asp:LinkButton>
                                                        </span>
                                                        <div class="corner">
                                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="g_box" Width="85%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>Delete</span>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkbtndelete" CommandName="DeleteAttachment" OnClientClick="return confirm('Are you sure, you want to delete attachment?');"
                                                            CommandArgument='<%#Eval("OnlyFileName")%>' runat="server">
                                                            <span class="btn_space">
                                                                <img alt="X" id="delete" src="~/Images/close.png" runat="server" /></span></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="spacer10">
                                    </div>
                                    <div class="form_top_co" style="clear: both;">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <input id="fpAttachment" name="fpAttachment" type="file" runat="server" style="float: left;" />
                                        <span style="float: left;">
                                            <asp:LinkButton ID="btnAddItem" Text="Add File" CssClass="greysm_btn" runat="server"
                                                OnClick="btnAddItem_Click"><span>Add File</span></asp:LinkButton>
                                        </span>
                                        <br />
                                        <div id="dvAttachment">
                                        </div>
                                        <br />
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                                <div class="cboxLoadingOverlay" style="height: 483px; display: none;" id="attachementcboxLoadingOverlay">
                                </div>
                                <div class="cboxLoadingGraphic" style="height: 483px; display: none;" id="attachementcboxLoadingGraphic">
                                </div>
                                <div class="cboxTitle" style="display: block;" id="attachementtitle">
                                </div>
                            </div>
                            <div class="cboxClose" style="" id="closeattchement">
                                close</div>
                        </div>
                        <div class="cboxMiddleRight" style="float: left; height: 483px;">
                        </div>
                    </div>
                    <div style="clear: left;">
                        <div class="cboxBottomLeft" style="float: left;">
                        </div>
                        <div class="cboxBottomCenter" style="float: left; width: 508px;">
                        </div>
                        <div class="cboxBottomRight" style="float: left;">
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div>
            <asp:LinkButton ID="lnkDummyTodo" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
            <at:ModalPopupExtender ID="modalTodo" BehaviorID="BehaviorTodo" TargetControlID="lnkDummyTodo"
                BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlTodo"
                CancelControlID="closeToDo">
            </at:ModalPopupExtender>
            <asp:Panel ID="pnlTodo" runat="server" Style="display: none;">
                <div class="cboxWrapper" style="display: block; width: 708px; height: 649px; position: fixed;
                    left: 25%; top: 5%;">
                    <div style="">
                        <div class="cboxTopLeft" style="float: left;">
                        </div>
                        <div class="cboxTopCenter" style="float: left; width: 658px;">
                        </div>
                        <div class="cboxTopRight" style="float: left;">
                        </div>
                    </div>
                    <div style="clear: left;">
                        <div class="cboxMiddleLeft" style="float: left; height: 533px;">
                        </div>
                        <div class="cboxContent" style="float: left; width: 658px; display: block; height: 533px;">
                            <div class="cboxLoadedContent" style="display: block; overflow: visible;">
                                <div style="padding: 25px 10px 10px 10px;">
                                    <div style="text-align: center; color: Red; font-size: 14px;">
                                        <asp:Label ID="lblToDoMsg" runat="server">
                                        </asp:Label>
                                    </div>
                                    <div style="height: 333px; overflow: auto;">
                                        <asp:DataList ID="dlTodo" runat="server" OnItemCommand="dlTodo_ItemCommand" CssClass="orderreturn_box"
                                            HeaderStyle-CssClass="ord_header" ShowHeader="true" RowStyle-CssClass="ord_content"
                                            GridLines="None" OnItemDataBound="dlTodo_ItemDataBound">
                                            <HeaderTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="width: 10%;">
                                                            <span>Done?</span>
                                                            <div class="corner">
                                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                            </div>
                                                        </td>
                                                        <td style="width: 65%;">
                                                            <span>To-Do</span>
                                                        </td>
                                                        <td style="width: 15%;">
                                                            <span>Due Date</span>
                                                        </td>
                                                        <td style="width: 10%;">
                                                            <span>Delete</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnToDoID" runat="server" Value='<%# Eval("ToDoID") %>' />
                                                <table class="true">
                                                    <tr>
                                                        <td class="b_box centeralign" style="width: 10%; border: solid 1px #1c1c1c; color: #ADADAD;
                                                            background: black;">
                                                            <span class="custom-checkbox centeralign" id="menuspan" runat="server">
                                                                <asp:CheckBox ID="chkTodoDone" runat="server" Checked='<%#Convert.ToBoolean(Eval("Done"))%>'
                                                                    OnCheckedChanged="chkTodoDone_CheckedChanged" AutoPostBack="true" />
                                                            </span>
                                                        </td>
                                                        <td class="g_box" style="width: 65%; border: solid 1px #1c1c1c; padding-left: 5px;
                                                            color: #72757C; background: #101010">
                                                            <asp:Label ID="lblToDo" runat="server" Text='<%# Convert.ToString(Eval("ToDo")).Length > 75 ? Convert.ToString(Eval("ToDo")).Substring(0, 75).Trim() + "..." : Convert.ToString(Eval("ToDo"))%>'
                                                                ToolTip='<%#Eval("ToDo")%>' />
                                                        </td>
                                                        <td class="b_box centeralign" style="width: 15%; border: solid 1px #1c1c1c; color: #ADADAD;
                                                            background: black;">
                                                            <lable>
                                                        <span>
                                                            <%# Eval("DueDate") != null ? Convert.ToDateTime(Eval("DueDate")).ToString("MM/dd/yyyy") : "" %></span></lable>
                                                        </td>
                                                        <td class="g_box centeralign" style="width: 10%; border: solid 1px #1c1c1c; color: #72757C;
                                                            background: #101010">
                                                            <asp:LinkButton ID="lnkbtndelete" CommandName="DeleteTodo" OnClientClick="return confirm('Are you sure, you want to delete To-do?');"
                                                                CommandArgument='<%#Eval("ToDoID")%>' runat="server">
                                                                <span class="btn_space">
                                                                    <img alt="X" id="delete" src="~/Images/close.png" runat="server" /></span></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </div>
                                    <div class="spacer10">
                                    </div>
                                    <div class="weather_form" style="width: 100%; padding: 0px 0px 0px 0px; margin: 0px auto 10px;">
                                        <div class="weatherlabel_pad">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 8%">To-do</span>
                                                <asp:TextBox ID="txtTodo" TabIndex="1" runat="server" MaxLength="2500" CssClass="w_label"
                                                    Style="width: 88%"></asp:TextBox>
                                                <div id="dvTodo">
                                                </div>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </div>
                                    <div class="weather_form select_box_pad form_table" style="width: 100%; padding: 0px 0px 0px 0px;
                                        margin: 0px auto 10px;">
                                        <div class="calender_l">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 9%;">Due Date</span>
                                                <asp:TextBox ID="txtDueDate" TabIndex="2" runat="server" CssClass="datepicker1 w_label"
                                                    Style="width: 87%"></asp:TextBox>
                                                <div id="dvDueDate">
                                                </div>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </div>
                                    <div class="centeralign" style="width: 100%;">
                                        <span class="btn_space">
                                            <asp:LinkButton ID="btnAddToDo" Text="Add To-do" CssClass="greysm_btn" runat="server"
                                                OnClick="btnAddToDo_Click"><span>Add To-do</span></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div class="cboxLoadingOverlay" style="height: 533px; display: none;">
                                </div>
                                <div class="cboxLoadingGraphic" style="height: 533px; display: none;">
                                </div>
                                <div class="cboxTitle" style="display: block;">
                                </div>
                            </div>
                            <div class="cboxClose" style="" id="closeToDo">
                                close</div>
                        </div>
                        <div class="cboxMiddleRight" style="float: left; height: 533px;">
                        </div>
                    </div>
                    <div style="clear: left;">
                        <div class="cboxBottomLeft" style="float: left;">
                        </div>
                        <div class="cboxBottomCenter" style="float: left; width: 658px;">
                        </div>
                        <div class="cboxBottomRight" style="float: left;">
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
    <div>
        <asp:Panel ID="pnlNoteSupp" runat="server" Style="display: none;">
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
                                            <div class="form_popup_box">
                                                <div class="label_bar">
                                                    <span>Notes/History :
                                                        <br />
                                                        <br />
                                                        <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtNoteSupp" runat="server"></asp:TextBox></span>
                                                </div>
                                                <div>
                                                    <asp:LinkButton ID="lnkNoteSupp" class="grey2_btn alignright" runat="server" OnClick="lnkNoteSupp_Click"><span>Save Notes</span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="pp_details clearfix" style="width: 371px;">
                                        <a href="#" id="closeSuppPopup" runat="server" class="pp_close">Close</a>
                                        <p class="pp_description" style="display: none;">
                                        </p>
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
    <asp:HiddenField ID="hdnTempID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnTempTodoID" runat="server" Value="0" />

    <script type="text/javascript" language="javascript">
        $(function() {
            scrolltextarea(".scrollme", "#Scrolltop", "#ScrollBottom");
        });
    </script>

</asp:Content>
