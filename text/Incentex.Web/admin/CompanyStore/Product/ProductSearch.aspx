<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ProductSearch.aspx.cs" Inherits="admin_CompanyStore_Product_ProductSearch"
    Title="Product Search" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">

        $().ready(function() {
            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtPrice: { number: true },
                          ctl00$ContentPlaceHolder1$ddlReportViewType: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtPrice: {
                            number: replaceMessageString(objValMsg, "Number", "Price")
                        },
                        ctl00$ContentPlaceHolder1$ddlReportViewType: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "report view type") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlReportViewType")
                            error.insertAfter("#dvReportView");
                        else
                            error.insertAfter(element);
                    }

                });

            });


            $("#<%=lnkSubmitRequest.ClientID %>").click(function() {
                $('#dvLoader').show();
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }

            });
        });

        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div class="form_table">
            <table class="dropdown_pad">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Company Store</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlCompanyStore" TabIndex="0" onchange="pageLoad(this,value);"
                                            runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCompanyStore_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                     <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Report View Type #</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlReportViewType" TabIndex="2" onchange="pageLoad(this,value);"
                                            runat="server">
                                            <asp:ListItem Value="0">- Select -</asp:ListItem>
                                            <asp:ListItem Value="Vendor Setup View">Vendor Setup View</asp:ListItem>
                                            <asp:ListItem Value="Pricing Setup Report">Pricing Setup Report</asp:ListItem>
                                            <asp:ListItem Value="Back Order Settings Report">Back Order Settings Report</asp:ListItem>
                                            <asp:ListItem Value="Inventory Management Report">Inventory Management Report</asp:ListItem>
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvReportView">
                                    </div>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Master Item #</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlMasterItemNo" TabIndex="1" onchange="pageLoad(this,value);"
                                            runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMasterItemNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
           
                <tr runat="server" id="trMasterItemNumberKeyword" visible="false">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Item Number keyword</span>
                                <asp:TextBox ID="txtMasterItemNumberKeyword" runat="server" TabIndex="2" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trItemNumber" visible="false">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Item Number</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlItemNo" TabIndex="3" onchange="pageLoad(this,value);" runat="server"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label" style="width: 32%">Workgroup</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlWorkgroup" runat="server" TabIndex="4" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvWorkgroup">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Keyword</span>
                                <asp:TextBox ID="txtKeyword" runat="server" TabIndex="5" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Price</span>
                                <asp:TextBox ID="txtPrice" runat="server" TabIndex="6" MaxLength="100" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton ID="lnkSubmitRequest" class="gredient_btn" TabIndex="3" runat="server"
                            OnClick="lnkSubmitRequest_Click"><span><strong>Search Now</strong></span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
