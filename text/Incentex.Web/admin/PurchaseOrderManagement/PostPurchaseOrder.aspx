<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="PostPurchaseOrder.aspx.cs" Inherits="admin_PurchaseOrderManagement_PostPurchaseOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px !important;
            padding-left: 36px;
        }
        .bulkorderquantity
        {
            margin: 0 auto;
            width: 35px;
            background: none repeat scroll 0 0 transparent;
            border: medium none;
            color: #F4F4F4;
            font-size: 11px;
            padding: 5px;
        }
        .orderreturn_box .ord_header th span
        {
            font-weight: bold;
        }
        .form_table input
        {
            background: none repeat scroll 0 0 transparent;
            border: medium none;
            color: #F4F4F4;
            font-size: 11px;
            height: 21px !important;
            line-height: 20px;
            vertical-align: middle;
        }
    </style>

    <script src="../../JS/JQuery/jquery.MultiFile.pack.js" type="text/javascript" language="javascript"></script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }

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
        var formats = 'pdf|jpg|jpeg|xls|xlsx|doc';
        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleInnerContainer").collapsibleInnerPanel();
            $(".collapsibleSubInnerContainer").collapsibleSubInnerPanel();
            $(".collapsibleContainerContent").hide();
            $(".collapsibleContainerInnerContent").hide();
            $(".collapsibleContainerSubInnerContent").hide();

            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlvendor: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlOrderfor: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlOverseasVendor: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlOrderSentby: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlMasterItem: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtOrderNumber: { required: true },
                        ctl00$ContentPlaceHolder1$fileupload: { accept: formats }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlvendor: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "vendor") },
                        ctl00$ContentPlaceHolder1$ddlOrderfor: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Order for") },
                        ctl00$ContentPlaceHolder1$ddlOverseasVendor: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Overseas Vendor") },
                        ctl00$ContentPlaceHolder1$ddlOrderSentby: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Order Sent by") },
                        ctl00$ContentPlaceHolder1$ddlMasterItem: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Master Item") },
                        ctl00$ContentPlaceHolder1$txtOrderNumber: { required: replaceMessageString(objValMsg, "Required", "order number") },
                        ctl00$ContentPlaceHolder1$fileupload: { accept: "File type not supported." }
                        //                        , accept: "File type not supported.", filesize: "Please select file less than 150MB."
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlvendor")
                            error.insertAfter("#dvVendor");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlOrderfor")
                            error.insertAfter("#dvOrderfor");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlOverseasVendor")
                            error.insertAfter("#dvOverseasVendor");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlOrderSentby")
                            error.insertAfter("#dvOrderSentby");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlMasterItem")
                            error.insertAfter("#dvMasterItem");
                        else
                            error.insertAfter(element);
                    }

                });
            });

            $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                if ($('#aspnetForm').valid()) {
                    $('#dvLoader').show();
                    return true;
                }
                else {
                    return false;
                }
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad form_table" style="padding-top: 0px !important;">
        <div>
            <table class="product_detail" cellpadding="0" cellspacing="0">
                <tr style="height: 40px;">
                    <td style="width: 25%;">
                        &nbsp;
                    </td>
                    <td style="width: 50%;">
                        &nbsp;
                    </td>
                    <td style="width: 25%;" rowspan="3">
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                    </td>
                    <td style="width: 75%;">
                        <asp:Label ID="lblMsg" runat="server" Font-Size="Small" Font-Italic="true" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: 25%;">
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <div>
                            <h4>
                                <span class="white_header" style="float: none">Master Item</span></h4>
                        </div>
                        <div class="spacer10">
                        </div>
                        <div class="alignleft">
                            <div class="agent_img">
                                <div class="alignleft item">
                                    <p class="upload_photo gallery">
                                        <a id="prettyphotoDiv" rel='prettyPhoto[a]' href="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                            runat="server">
                                            <img id="imgmasteritem" runat="server" src="~/UploadedImages/ProductImages/ProductDefault.jpg" />
                                        </a>
                                        <asp:HiddenField ID="hdnimagestatus" runat="server" Value='<%# Eval("ProductImageActive") %>' />
                                        <asp:HiddenField ID="hdndocumentname" runat="server" Value='<%# Eval("ProductImage") %>' />
                                        <asp:HiddenField ID="hdnlargerimagename" runat="server" Value='<%# Eval("LargerProductImage") %>' />
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div style="text-align: center;">
                            <asp:Label Style="line-height: 12px; width: 100%; height: auto;" ID="lblItemDescriptions"
                                runat="server"></asp:Label>
                        </div>
                    </td>
                    <td style="width: 50%;" class="leftalign">
                        <div>
                            <h4>
                                <span class="white_header" style="float: none">Purchase Order</span></h4>
                            <div style="width: 75%">
                                <table class="form_table">
                                    <tr id="trVendor" runat="server">
                                        <td class="form_td">
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Vendor</span><span class="custom-sel label_sel">
                                                        <asp:DropDownList ID="ddlvendor" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);"
                                                            OnSelectedIndexChanged="ddlvendor_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div id="dvVendor">
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trOrderfor" runat="server">
                                        <td class="form_td">
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Order for</span> <span class="custom-sel label_sel">
                                                        <asp:DropDownList ID="ddlOrderfor" runat="server" onchange="pageLoad(this,value);">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div id="dvOrderfor">
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trOverseasVendor" runat="server">
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Overseas Vendor</span> <span class="custom-sel label_sel">
                                                        <asp:DropDownList ID="ddlOverseasVendor" runat="server" onchange="pageLoad(this,value);">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div id="dvOverseasVendor">
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trOrderNumber" runat="server">
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box shipmax_in">
                                                    <span class="input_label">Enter Purchase Order Number</span> <span id="dvL3" runat="server"
                                                        style="color: #F4F4F4; font-size: 11px;"></span>
                                                    <asp:TextBox ID="txtOrderNumber" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trOrderSentby" runat="server">
                                        <td class="form_td">
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Purchase Order Sent by</span> <span class="custom-sel label_sel">
                                                        <asp:DropDownList ID="ddlOrderSentby" runat="server" onchange="pageLoad(this,value);">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div id="dvOrderSentby">
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trSelectMasterItem" runat="server">
                                        <td class="form_td">
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Select Master Item</span> <span class="custom-sel label_sel">
                                                        <asp:DropDownList ID="ddlMasterItem" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);"
                                                            OnSelectedIndexChanged="ddlMasterItem_SelectedIndexchanged">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div id="dvMasterItem">
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
                                                    <span class="input_label">Upload Files </span><span id="Span1" runat="server" style="color: #F4F4F4;
                                                        font-size: 11px;"></span>
                                                    <input type="file" id="fileupload" style="margin: -24px 0px 8px 212px" runat="server"
                                                        tabindex="6" class="accept-pdf|jpg|jpeg|xls|xlsx|doc" />
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                    <td style="width: 25%;" class="alignleft">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="collapsibleContainer" title="Listing of Item Sizes" align="left">
                            <div class="form_table">
                                <asp:GridView ID="gvItemDetails" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvItemDetails_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField Visible="False" HeaderText="Id">
                                            <HeaderTemplate>
                                                <span>ProductItemID</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblProductItemID" Text='<%# Eval("ProductItemID") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False" HeaderText="Id">
                                            <HeaderTemplate>
                                                <span>Product ID</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblProductId" Text='<%# Eval("ProductId") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
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
                                                    <asp:CheckBox ID="chkItem" runat="server" />&nbsp; </span>
                                                <div class="corner">
                                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" CssClass="b_box centeralign" />
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
                                                    <asp:LinkButton ID="hypStyle" CommandName="Edit" runat="server" ToolTip='<%# Eval("MasterStyleName")%>'><%# (Eval("MasterStyleName").ToString().Length > 20) ? Eval("MasterStyleName").ToString().Substring(0, 20) + "..." : Eval("MasterStyleName") + "&nbsp;"%></asp:LinkButton>
                                                </span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ProductStyle">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnStyle" runat="server" CommandArgument="ProductStyle" CommandName="Sort"><span>Style</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderStyle" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblStyle" Text='<%# Eval("ProductStyle") %>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="18%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ItemNumber">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                                    CommandName="Sort"> <span >Item #</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderItemNumber" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                                <asp:HiddenField ID="hdnItemNumber" runat="server" Value='<%# Eval("ItemNumber") %>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="17%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ItemColor">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnItemColor" runat="server" CommandArgument="ItemColor" CommandName="Sort"><span>Color</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderItemColor" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblItemColor" ToolTip='<%# Eval("ItemColor")%>' Text='<%# (Eval("ItemColor").ToString().Length > 8) ? Eval("ItemColor").ToString().Substring(0, 8) + "..." : Eval("ItemColor") + "&nbsp;"%>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ItemSize">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnItemSize" runat="server" CommandArgument="ItemSize" CommandName="Sort"><span>Size</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderItemSize" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblItemSize" Text='<%# Eval("ItemSize") %>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ItemQuantity">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnQuantity" runat="server" CommandArgument="ItemQuantity"
                                                    CommandName="Sort"><span>Quantity</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderItemQuantity" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span class="btn_space">
                                                    <asp:TextBox runat="server" ID="txtQuantity" Text='<%# Eval("ItemQuantity") %>' Style="background-color: #303030;
                                                        border: medium none; color: #FFFFFF; padding: 2px; width: 100%; text-align: center;" /></span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box centeralign" Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ItemPrice">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnPrice" runat="server" CommandArgument="ItemPrice" CommandName="Sort"><span>Price</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderPrice" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span class="btn_space">
                                                    <asp:TextBox runat="server" ID="txtPrice" Text='<%# Eval("ItemPrice") %>' Style="background-color: #303030;
                                                        border: medium none; color: #FFFFFF; padding: 2px; width: 100%; text-align: center;" /></span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box centeralign" Width="7%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="spacer10">
                        </div>
                    </td>
                    <td style="width: 25%;" class="alignleft">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div>
                            <asp:LinkButton ID="lnkBtnSaveInfo" runat="server" CssClass="grey2_btn" Style="margin-top: 15px"
                                OnClick="lnkBtnSaveInfo_Click"><span>Save</span>
                            </asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
