<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="PastDownloads.aspx.cs" Inherits="admin_Supplier_PastDownloads" Title="World-Link System - Past Downloads" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function HeaderClick(CheckBox) {
            //Get target base & child control.            
            var TargetBaseControl = document.getElementById(CheckBox.id.substring(0, CheckBox.id.length - 18));
            var TargetChildControl = "chkBxSelect";

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                Inputs[n].checked = CheckBox.checked;                
            }
        }

        function ChildClick(CheckBox, HCheckBox) {
            //get target control.            
            var flag = true;
            
            if (CheckBox.checked == true) {
                var TargetBaseControl = document.getElementById(CheckBox.id.substring(0, CheckBox.id.length - 18));
                var TargetChildControl = "chkBxSelect";

                //Get all the control of the type INPUT in the base control.
                var Inputs = TargetBaseControl.getElementsByTagName("input");
                
                //Checked/Unchecked all the checkBoxes in side the GridView.            
                for (var n = 0; n < Inputs.length; ++n) {            
                    if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                        if (Inputs[n].checked == false) {
                            flag = false;
                            break;
                        }
                    } 
                }
            }
            else {
                flag = false;
            }
            //Change state of the header CheckBox.
            var HeaderCheckBox = document.getElementById(HCheckBox);
            HeaderCheckBox.checked = flag;
        }
        
        function CompareDates(fromDate, toDate, separator) {
            var fromDateArr = Array();
            var toDateArr = Array();

            fromDateArr = fromDate.split(separator);
            toDateArr = toDate.split(separator);
            
            var fromMt = fromDateArr[0];
            var fromDt = fromDateArr[1];
            var fromYr = fromDateArr[2];

            var toMt = toDateArr[0];
            var toDt = toDateArr[1];            
            var toYr = toDateArr[2];

            if (fromYr > toYr) {
                return 0;
            }
            else if (fromYr == toYr && fromMt > toMt) {
                return 0;
            }
            else if (fromYr == toYr && fromMt == toMt && fromDt > toDt) {
                return 0;
            }
            else
                return 1;
        }
    </script>

    <script type="text/javascript" language="javascript">
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });

        $(function() {
            $("#<%=lnkBtnSearchNow.ClientID %>").click(function() {
                if (!CompareDates($("#ctl00_ContentPlaceHolder1_txtFromDate").val(), $("#ctl00_ContentPlaceHolder1_txtToDate").val(), "/")) {
                    ShowMessage('1');
                    return false;
                }
            });

            $("#ctl00_ContentPlaceHolder1_txtFromDate").change(function() {
                if (!CompareDates($("#ctl00_ContentPlaceHolder1_txtFromDate").val(), $("#ctl00_ContentPlaceHolder1_txtToDate").val(), "/")) {
                    ShowMessage('1');
                }
                else {
                    ShowMessage('0');
                }
            });

            $("#ctl00_ContentPlaceHolder1_txtToDate").change(function() {
                if (!CompareDates($("#ctl00_ContentPlaceHolder1_txtFromDate").val(), $("#ctl00_ContentPlaceHolder1_txtToDate").val(), "/")) {
                    ShowMessage('1');
                }
                else {
                    ShowMessage('0');
                }
            });
            
            $(window).scroll(function () {
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
        });

        function ShowMessage(show) {
            if (show == '1') {
                document.getElementById("spanFromDate").style.display = '';
                document.getElementById("spanToDate").style.display = '';
            }
            else {
                document.getElementById("spanFromDate").style.display = 'none';
                document.getElementById("spanToDate").style.display = 'none';
            }
        }
    </script>

    <style type="text/css">
        .synclabel
        {
            font-size: 15px;
            color: #72757c;
        }
        .tdmiddle
        {
            vertical-align: middle;
            width: 45%;
        }
    </style>
    <style type="text/css">
        .form_box span.error
        {
            margin-left: 0%;
        }
        .select_box_pad
        {
            width: 380px;
        }
    </style>
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
        <div class="form_table">
            <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
            <table class="select_box_pad">
                <tr>
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">From Date</span>
                                <asp:TextBox ID="txtFromDate" runat="server" TabIndex="0" CssClass="datepicker1 w_label"></asp:TextBox>
                                <span class="error" id="spanFromDate" style="display: none;">From date should be less
                                    than to date.</span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">To Date</span>
                                <asp:TextBox ID="txtToDate" runat="server" TabIndex="1" CssClass="datepicker1 w_label"></asp:TextBox>
                                <span class="error" id="spanToDate" style="display: none;">To date should be greater
                                    than from date.</span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block" colspan="2">
                        <asp:LinkButton ID="lnkBtnSearchNow" TabIndex="2" class="gredient_btn" runat="server"
                            ToolTip="Search Now" OnClick="lnkBtnSearchNow_Click"><span>Search Now</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div style="text-align: center">
            <asp:Label ID="lblMsgList" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="100%">
                        <div style="text-align: right; color: #5C5B60; font-size: larger; font-weight: bold;
                            padding-left: 15px; display: none;" id="dvTotal" runat="server">
                            Total Records :
                            <asp:Label ID="lblRecords" runat="server"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td colspan="2">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div>
                            <asp:GridView ID="gvDownloadedOrders" runat="server" HeaderStyle-CssClass="ord_header"
                                CssClass="orderreturn_box true records" GridLines="None" AutoGenerateColumns="false"
                                RowStyle-CssClass="ord_content" OnRowDataBound="gvDownloadedOrders_RowDataBound"
                                OnRowCommand="gvDownloadedOrders_RowCommand" OnRowCreated="gvDownloadedOrders_RowCreated">
                                <Columns>
                                    <asp:TemplateField HeaderText="Check">
                                        <HeaderTemplate>
                                            <span class="chkClass123xyz">
                                                <asp:CheckBox ID="chkBxHeader" onclick="javascript:HeaderClick(this);" runat="server" />&nbsp;</span>
                                            <div class="corner">
                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <span class="first centeralign chkClass123xyz">
                                                <asp:CheckBox ID="chkBxSelect" runat="server" />&nbsp;</span>
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                            <asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("ID") %>' />
                                            <asp:HiddenField runat="server" ID="hdnOriginalFile" Value='<%# Eval("OriginalFile") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" CssClass="b_box centeralign" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="DisplayName">
                                        <HeaderTemplate>
                                            <span>
                                                <asp:LinkButton ID="lnkBtnHeaderDisplayName" runat="server" CommandArgument="DisplayName"
                                                    CommandName="Sort">File Name</asp:LinkButton>
                                                <asp:PlaceHolder ID="placeHolderDisplayName" runat="server"></asp:PlaceHolder>
                                            </span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblDisplayName" ToolTip='<%# Convert.ToString(Eval("DisplayName")) %>'
                                                Text='<%# Convert.ToString(Eval("DisplayName")).Length > 50 ? Convert.ToString(Eval("DisplayName")).Substring(0, 50).Trim() + "..." : Convert.ToString(Eval("DisplayName")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="35%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="DownloadedBy">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkBtnHeaderDownloadedBy" runat="server" CommandArgument="DownloadedBy"
                                                CommandName="Sort"><span>Downloaded By</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeHolderDownloadedBy" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblDownloadedBy" Text='<%# Convert.ToString(Eval("DownloadedBy")).Length > 30 ? Convert.ToString(Eval("DownloadedBy")).Substring(0, 30).Trim() + "..." : Convert.ToString(Eval("DownloadedBy")) %>'
                                                ToolTip='<%# Eval("DownloadedBy")  %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="DownloadedDate">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkBtnHeaderDownloadedDate" runat="server" CommandArgument="DownloadedDate"
                                                CommandName="Sort"><span>Downloaded Date & Time</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeHolderDownloadedDate" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span runat="server" id="lblDownloadedDate">
                                                <%# Convert.ToDateTime(Eval("DownloadedDate")).ToString("MM/dd/yyyy h:mm:ss tt") %></span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Download</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:LinkButton ID="lnkDownload" runat="server" CommandArgument='<%# Convert.ToString(Eval("DisplayName") + "|" + Eval("OriginalFile")) %>'
                                                    CommandName="Download" ToolTip="Download" Style="margin-left: 1px;">
                                                            <img alt="D" src="../../Images/download_btn.png" style="margin-right: 2px;"
                                                                id="imgDownload" /></asp:LinkButton></span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="orderreturn_box">
                                <div class="companylist_botbtn alignleft">
                                    <asp:LinkButton ID="lnkDelete" runat="server" class="grey_btn" OnClick="lnkDelete_Click"
                                        OnClientClick="return confirm('Are you sure, you want to delete selected record(s)?')">
						                <span>Delete</span>
                                    </asp:LinkButton>
                                </div>
                                <div id="pagingTable" runat="server" class="alignright pagging">
                                    <asp:LinkButton ID="lnkBtnPrevious" class="prevb" runat="server" OnClick="lnkBtnPrevious_Click"> 
                                    </asp:LinkButton>
                                    <span>
                                        <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                    CommandName="lnkBtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:DataList></span>
                                    <asp:LinkButton ID="lnkBtnNext" class="nextb" runat="server" OnClick="lnkBtnNext_Click"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
