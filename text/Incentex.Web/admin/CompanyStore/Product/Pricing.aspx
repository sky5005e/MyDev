<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="Pricing.aspx.cs" Inherits="admin_ProductStoreManagement_StorePricing"
    Title="Product>> Pricing" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 36px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }

        function setvalues(ctl, cla) {
            $('.' + cla).val(ctl.value);
        }
    </script>

    <script type="text/javascript">
        // Let's use a lowercase function name to keep with JavaScript conventions
        function selectAll(invoker) {
            // Since ASP.NET checkboxes are really HTML input elements
            //  let's get all the inputs
            var inputElements = document.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                // Filter through the input types looking for checkboxes
                if (myElement.type === "checkbox") {
                    myElement.checked = invoker.checked;
                }
            }
        } 
    </script>

    <script type="text/javascript" language="javascript">

        $().ready(function() {
            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$lblStyle: { required: true },
                        ctl00$ContentPlaceHolder1$ddlMasterItemNo: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlItemNumber: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtPriceStartDate: {
                            //required: true,
                            validdate: true,
                            CompareTo: '{"controltovalidate":"#ctl00_ContentPlaceHolder1_txtPricingEndDate","operator":"<="}'
                        },
                        ctl00$ContentPlaceHolder1$txtPricingEndDate: {
                            //required: true,
                            validdate: true,
                            CompareTo: '{"controltovalidate":"#ctl00_ContentPlaceHolder1_txtPriceStartDate","operator":">="}'
                        }

                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$lblStyle: {
                            required: replaceMessageString(objValMsg, "Required", "style")

                        },
                        ctl00$ContentPlaceHolder1$ddlMasterItemNo: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "master item number")
                        },

                        ctl00$ContentPlaceHolder1$ddlItemNumber: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "item number")
                        },
                        ctl00$ContentPlaceHolder1$txtPriceStartDate: {
                            //required: "<br/>Please select Ads start Date",
                            validdate: "<br />Invalid date format.",
                            CompareTo: "<br />Start date should be less than end date"
                        },
                        ctl00$ContentPlaceHolder1$txtPricingEndDate: {
                            //required: "<br/>Please select Ads End Date",
                            validdate: "<br />Invalid date format.",
                            CompareTo: "<br />End date should be greater than start date"
                        }
                    }

                });
            });

            $("#<%=lnkAddItem.ClientID %>").click(function() {

                return $('#aspnetForm').valid();
            });


        });
        $(function() {

            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();

                $(function() {

                    $(".datepicker1").datepicker({
                        buttonText: 'DatePicker',
                        showOn: 'button',
                        buttonImage: '../../../images/calender-icon.jpg',
                        buttonImageOnly: true
                    });
                });

            }
        }

    </script>

    <script type="text/javascript">
        function validate(validator, arg) {
            var textBox1 = document.getElementById('<%=txtLevelOne.ClientID %>');
            var textBox2 = document.getElementById('<%=txtLeveltwo.ClientID %>');
            var textBox3 = document.getElementById('<%=txtLevelthree.ClientID %>');
            var textBox4 = document.getElementById('<%=txtLevelfour.ClientID %>');
            if (textBox1.value > textBox3.value) {
                arg.IsValid = true; //Valid Value  
            }
            else if (textBox2.value > textBox3.value) {
                arg.IsValid = true; //Valid Value  
            }
            else
                arg.IsValid = false; //Invalid Value   
        }   
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <mb:MenuUserControl ID="manuControl" runat="server" />
    <div class="form_pad">
        <div>
            <asp:UpdatePanel runat="server" ID="upAdd">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlAddItem">
                        <table class="dropdown_pad form_table">
                            <tr>
                                <td colspan="2">
                                    <div style="text-align: center; color: Red; font-size: larger;">
                                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box taxt_area clearfix">
                                                    <span class="input_label alignleft" style="width: 39%!important;">Product Description</span>
                                                    <div class="textarea_box alignright" style="width: 54%;">
                                                        <div class="scrollbar">
                                                            <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                                            </a></label>
                                                        </div>
                                                        <asp:TextBox ID="txtPrdDescription" runat="server" TextMode="MultiLine" ReadOnly="true"
                                                            CssClass="scrollme1" Height="65px"></asp:TextBox>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co" style="width: 400px">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box shipmax_in">
                                                    <span class="input_label">Master Item Number</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlMasterItemNo" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);"
                                                            OnSelectedIndexChanged="ddlMasterItemNo_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co" style="width: 400px">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Style </span><span class="w_label">
                                                        <asp:Label runat="server" ID="lblStyle"></asp:Label></span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Item color</span> <span class="w_label">
                                                        <asp:Label runat="server" ID="lblItemColor"></asp:Label></span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="upnlItemNumber" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Item Number</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlItemNo" AutoPostBack="true" runat="server" onchange="pageLoad(this,value);"
                                                            OnSelectedIndexChanged="ddlItemNo_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Product Cost($)</span>
                                                    <asp:TextBox ID="txtProductCost" runat="server" MaxLength="6" CssClass="w_label"></asp:TextBox>
                                                    <at:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers" ValidChars="." FilterInterval="0" TargetControlID="txtProductCost">
                                                    </at:FilteredTextBoxExtender>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Size</span> <span class="w_label">
                                                        <asp:Label runat="server" ID="lblSize"></asp:Label></span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Level One (L1)$</span>
                                                    <asp:TextBox ID="txtLevelOne" runat="server" MaxLength="6" CssClass="w_label"></asp:TextBox>
                                                    <%--<at:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txtLevelOne" Mask="999.99"
                                                        InputDirection="RightToLeft" AcceptNegative="Left" DisplayMoney="Left" AutoComplete="false"
                                                        MaskType="Number" runat="server">
                                                    </at:MaskedEditExtender>--%>
                                                    <at:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers" ValidChars="." FilterInterval="0" TargetControlID="txtLevelOne">
                                                    </at:FilteredTextBoxExtender>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Level Two (L2)$</span>
                                                    <asp:TextBox ID="txtLeveltwo" runat="server" MaxLength="6" CssClass="w_label"></asp:TextBox>
                                                    <%--<at:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtLeveltwo" Mask="999.99"
                                                        InputDirection="RightToLeft" AcceptNegative="Left" DisplayMoney="Left" AutoComplete="false"
                                                        MaskType="Number" runat="server">
                                                    </at:MaskedEditExtender>--%>
                                                    <at:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers" ValidChars="." FilterInterval="0" TargetControlID="txtLeveltwo">
                                                    </at:FilteredTextBoxExtender>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Level Three (L3)$</span>
                                                    <asp:TextBox ID="txtLevelthree" runat="server" MaxLength="6" CssClass="w_label"></asp:TextBox>
                                                    <%--<at:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtLevelthree" Mask="?????"
                                                        InputDirection="RightToLeft" AcceptNegative="Left" DisplayMoney="Left" AutoComplete="false"
                                                        MaskType="Alphanumeric" runat="server">
                                                    </at:MaskedEditExtender>--%>
                                                    <at:FilteredTextBoxExtender ID="txtLevelthree_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterType="Custom, Numbers" ValidChars="." FilterInterval="0"
                                                        TargetControlID="txtLevelthree">
                                                    </at:FilteredTextBoxExtender>
                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="L3 Price should not be greater than L2 or L3 prices!"
                                                        ValidationGroup="grp1" ClientValidationFunction="validate"></asp:CustomValidator>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpPnlLevel4" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Level Four (L4)$</span>
                                                    <asp:TextBox ID="txtLevelfour" runat="server" MaxLength="6" CssClass="w_label"></asp:TextBox>
                                                    <at:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers" ValidChars="." FilterInterval="0" TargetControlID="txtLevelfour">
                                                    </at:FilteredTextBoxExtender>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpPnlClosePrice" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Close Out Price$</span>
                                                    <asp:TextBox ID="txtCloseOutPrice" runat="server" MaxLength="6" CssClass="w_label"></asp:TextBox>
                                                    <at:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers" ValidChars="." FilterInterval="0" TargetControlID="txtCloseOutPrice">
                                                    </at:FilteredTextBoxExtender>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="upnlPricingStatus" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box shipmax_in">
                                                    <span class="input_label">Level 3 Pricing Status</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlLevelThreePricStatus" runat="server" onchange="pageLoad(this,value);">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td class="formtd">
                                    <div class="calender_l">
                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box shipmax_in">
                                                    <span class="input_label cal_widper">Level 3 Pricing Start Date</span>
                                                    <asp:TextBox ID="txtPriceStartDate" runat="server" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div class="calender_l">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box shipmax_in">
                                                    <span class="input_label cal_widper">Level 3 Pricing End Date</span>
                                                    <asp:TextBox ID="txtPricingEndDate" runat="server" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="upnlOrderrule" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box shipmax_in">
                                                    <span class="input_label">Level 3 Ordering Rule</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlLevethreeOrderRule" AutoPostBack="true" runat="server" onchange="pageLoad(this,value);">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="upnlSaleFlag" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box shipmax_in">
                                                    <span class="input_label">Level 3 On Sale Flag</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlLevelthreeSaleFlag" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="upnlPreSeason" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box shipmax_in">
                                                    <span class="input_label">Level 3 Pre-Season Purchase</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlLevel3PreSeasonPurchase" AutoPostBack="true" runat="server"
                                                            onchange="pageLoad(this,value);">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box shipmax_in">
                                                    <span class="input_label">Price for</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlPricefor" AutoPostBack="true" runat="server" onchange="pageLoad(this,value);">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="spacer10" colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <div class="botbtn centeralign">
                                                <asp:LinkButton ID="lnkAddItem" CausesValidation="true" ValidationGroup="grp1" class="grey2_btn"
                                                    runat="server" ToolTip="Add Item" OnClick="lnkAddItem_Click"><span>Save Information</span></asp:LinkButton>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="divider">
        </div>
        <div>
            <h4>
                List of Item Details</h4>
            <asp:UpdatePanel runat="server" ID="upnlGrdCompany">
                <ContentTemplate>
                    <div>
                        <div>
                            <div style="text-align: center; color: Red; font-size: larger;">
                                <asp:Label ID="lblmsg" runat="server">
                                </asp:Label>
                            </div>
                            <asp:GridView ID="gvPricing" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="gvPricing_RowCommand"
                                OnRowDataBound="gvPricing_RowDataBound" OnRowEditing="gvPricing_RowEditing">
                                <Columns>
                                    <asp:TemplateField HeaderText="Check">
                                        <HeaderTemplate>
                                            <span>
                                                <asp:CheckBox ID="cbSelectAll" runat="server" OnClick="selectAll(this)" />&nbsp;</span>
                                            <div class="corner">
                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <span class="first">
                                                <asp:CheckBox ID="CheckBox1" runat="server" />&nbsp;</span>
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" CssClass="b_box centeralign" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="False" HeaderText="Id">
                                        <HeaderTemplate>
                                            <span>ProductItemPricingID</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblProductItemPricingID" Text='<%# Eval("ProductItemPricingID") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="False" HeaderText="Id">
                                        <HeaderTemplate>
                                            <span>ProductItemID</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblProductItemID" Text='<%# Eval("ProductItemID") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="MasterStyleName">
                                        <HeaderTemplate>
                                            <span>
                                                <asp:LinkButton ID="lnkbtnMasterStyleName" runat="server" CommandArgument="MasterStyleName"
                                                    CommandName="Sort">Master #</asp:LinkButton>
                                            </span>
                                            <asp:PlaceHolder ID="placeholderMasterStyleName" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span>
                                                <asp:LinkButton ID="hypStyle" CommandName="Edit" runat="server" ToolTip='<%# Eval("MasterStyleName")%>'><%# (Eval("MasterStyleName").ToString().Length > 18) ? Eval("MasterStyleName").ToString().Substring(0, 18) + "..." : Eval("MasterStyleName")%></asp:LinkButton>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="24%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ItemNumber">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                                CommandName="Sort"> <span >Item #</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderItemNumber" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="16%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ItemColor">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnItemColor" runat="server" CommandArgument="ItemColor" CommandName="Sort"><span>Color</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderItemColor" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblItemColor" ToolTip='<%# Eval("ItemColor") %>' Text='<%# (Eval("ItemColor").ToString().Length > 5) ? Eval("ItemColor").ToString().Substring(0, 5) + "..." : Eval("ItemColor") + "&nbsp;"%>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="9%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ItemSize">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnItemSize" runat="server" CommandArgument="ItemSize" CommandName="Sort"><span>Size</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderItemSize" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblItemSize" Text='<%# Eval("ItemSize") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ProductCost">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnProductCost" runat="server" CommandArgument="ProductCost"
                                                CommandName="Sort"><span >Product Cost</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderProductCost" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                                    text-align: center; color: #ffffff; width: 50px; padding: 2px" onchange="CheckNum(this.id)"
                                                    MaxLength="10" BackColor="#303030" ID="txtProductCost" Text='<%# Eval("ProductCost") %>'></asp:TextBox>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="12%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Level1">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnLevel1" runat="server" CommandArgument="Level1" CommandName="Sort"><span >L1</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderLevel1" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none; 
                                                    text-align: center; color: #ffffff; width: 50px; padding: 2px" CssClass="l1" Ondblclick="setvalues(this,'l1')" onchange="CheckNum(this.id)"
                                                    MaxLength="10" BackColor="#303030" ID="txtLevel1grid" Text='<%# Eval("Level1") %>'></asp:TextBox>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Level2">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnLevel2" runat="server" CommandArgument="Level2" CommandName="Sort"><span >L2</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderLevel2" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:TextBox runat="server" CssClass="l2" Ondblclick="setvalues(this,'l2')" Style="background-color: #303030; border: medium none;
                                                    text-align: center; color: #ffffff; width: 50px; padding: 2px" onchange="CheckNum(this.id)"
                                                    MaxLength="10" BackColor="#303030" ID="txtLevel2grid" Text='<%# Eval("Level2") %>'></asp:TextBox>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Level3">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnLevel3" runat="server" CommandArgument="Level3" CommandName="Sort"><span >L3</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderLevel3" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:TextBox runat="server" CssClass="l3" Ondblclick="setvalues(this,'l3')" Style="background-color: #303030; border: medium none;
                                                    text-align: center; color: #ffffff; width: 50px; padding: 2px" onchange="CheckNum(this.id)"
                                                    MaxLength="10" BackColor="#303030" ID="txtLevel3grid" Text='<%# Eval("Level3") %>'></asp:TextBox>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Level4">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnLevel4" runat="server" CommandArgument="Level4" CommandName="Sort"><span >L4</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderLevel4" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:TextBox runat="server" CssClass="l4" Ondblclick="setvalues(this,'l4')" Style="background-color: #303030; border: medium none;
                                                    text-align: center; color: #ffffff; width: 50px; padding: 2px" onchange="CheckNum(this.id)"
                                                    MaxLength="10" BackColor="#303030" ID="txtLevel4grid" Text='<%# Eval("Level4") %>'></asp:TextBox>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CloseOut">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCloseOut" runat="server" CommandArgument="CloseOut" CommandName="Sort"><span >Close Out</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderCloseOut" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                                    text-align: center; color: #ffffff; width: 50px; padding: 2px" onchange="CheckNum(this.id)"
                                                    MaxLength="10" BackColor="#303030" ID="txtCloseOutPricegrid" Text='<%# Eval("CloseOutPrice") %>'></asp:TextBox>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Delete</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:ImageButton ID="lnkbtndelete" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                                    CommandArgument='<%# Eval("ProductItemPricingID") %>' runat="server" ImageUrl="~/Images/close.png" /></span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="3%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div>
                            <div class="companylist_botbtn alignleft">
                                <asp:LinkButton ID="btnSaveStatus" runat="server" TabIndex="0" CssClass="grey_btn"
                                    OnClick="btnSaveStatus_Click"><span>Save</span>
                                </asp:LinkButton>
                            </div>
                            <div id="pagingtable" runat="server" class="alignright pagging">
                                <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                                </asp:LinkButton>
                                <span>
                                    <asp:DataList ID="DataList2" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" OnItemCommand="DataList2_ItemCommand" OnItemDataBound="DataList2_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:DataList></span>
                                <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
