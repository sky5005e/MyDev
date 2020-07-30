<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OrderInvoicePayment.aspx.cs" Inherits="OrderManagement_OrderInvoicePayment" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <style type="text/css">
        .bigwidthlabel
        {
            width: 135px !important;
        }
        orderreturn_box .ord_content a.close
        {
            background: url(../images/close.png) no-repeat top;
        }
        .width300
        {
            font-size: small;
            width: 300px !important;
        }
    </style>

    <script type="text/javascript">
        
        var formats = 'pdf';

        $().ready(function() {
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) 
            {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                rules: {
                        ctl00$ContentPlaceHolder1$txtInvoiceNumber:{required : true},
                        ctl00$ContentPlaceHolder1$fpSupplierPurchadOrderUpload: { required: true, accept: formats }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtInvoiceNumber:{required: replaceMessageString(objValMsg, "Required", "invoice number")},
                        ctl00$ContentPlaceHolder1$fpSupplierPurchadOrderUpload: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." }
                    },
                    onsubmit: false
                });
            });
             $("#<%=lnkbtnDocument.ClientID %>").click(function() {
                return $('#aspnetForm').valid();

            });
         });
       
    </script>

    <%--   <asp:UpdatePanel ID="uMain" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkbtnDocument" />
        </Triggers>
        <ContentTemplate>--%>
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
                                                    <label style="padding-left: 28px!important;">
                                                        Ordered Date :
                                                    </label>
                                                    <asp:Label ID="lblOrderedDate" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
                                                        Order Status :
                                                    </label>
                                                    <asp:Label runat="server" ID="lblOrderStatus"></asp:Label>
                                                </td>
                                            </tr>
                                            <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                              {%>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
                                                        Payment Method :
                                                    </label>
                                                    <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <%} %>
                                            <tr id="trCreditType" runat="server">
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
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
                    <table width="100%">
                        <tr>
                            <td colspan="3">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 80%; vertical-align: middle;" colspan="2">
                                            <div class="clearfix billing_head">
                                                <div class="alignleft" style="width: 100%;">
                                                    <span>Invoice & Document Details :</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="alignnone spacer15">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div>
                                                <asp:GridView ID="gvSupplierPurchaseOrder" runat="server" AutoGenerateColumns="false"
                                                    HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                                    OnRowCommand="gvSupplierPurchaseOrder_RowCommand" RowStyle-CssClass="ord_content"
                                                    OnRowDataBound="gvSupplierPurchaseOrder_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField Visible="False" HeaderText="Id">
                                                            <HeaderTemplate>
                                                                <span>OrderDocumentID</span>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblSupplierPurchaseOrderId" Text='<%# Eval("OrderDocumentID") %>' />
                                                                <asp:HiddenField ID="hdnPSupplierId" runat="server" Value='<%# Eval("SupplierId") %>' />
                                                                <asp:HiddenField runat="server" ID="hdnPOrderId" Value='<%# Eval("OrderId") %>' />
                                                                <asp:HiddenField ID="hdnMyShoppingCartId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "MyShoppingCartID")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="1%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <span>Invoice Number</span>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInvoice" runat="server" Text='<% #Eval("InvoiceNumber") == null ? "---" : Eval("InvoiceNumber") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="b_box" Width="5%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="FileName">
                                                            <HeaderTemplate>
                                                                <span>File Name</span>
                                                                <%--<asp:LinkButton ID="lnkbtnFileName" runat="server" CommandArgument="FileName" CommandName="Sort"><span >FileName</span></asp:LinkButton>--%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lnkFileName" runat="server" Text='<% # (Convert.ToString(Eval("FileName")).Length > 30) ? Eval("FileName").ToString().Substring(0,30)+"..."  : Convert.ToString(Eval("FileName"))+ "&nbsp;"  %>'
                                                                    ToolTip='<% #Eval("FileName")  %>'></asp:Label>
                                                                <asp:HiddenField ID="hdnFileNamePurchase" Value='<%# Eval("FileName") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="b_box" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblUserName" Text='<%# Eval("UploadedBy") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="b_box" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <span>Icon</span>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <span class="btn_space">
                                                                    <asp:LinkButton runat="server" ID="lnkuIcon" CommandName="viewPurchaseOrder" CommandArgument='<%# Eval("FileName") %>'
                                                                        CssClass="pdf2"></asp:LinkButton>
                                                                </span>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="centeralign" />
                                                            <ItemStyle Width="3%" CssClass="b_box centeralign" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <span>Delete</span>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="centeralign" />
                                                            <ItemTemplate>
                                                                <span class="btn_space">
                                                                    <asp:LinkButton ID="lnkbtndeletePurchaseOrder" runat="server" Text="" CommandName="deleteinvoicedocument"
                                                                        CommandArgument='<%# Eval("OrderDocumentID") %>' CssClass="close" OnClientClick="javascript:return confirm('Are you sure you want to delete invoice?');">
                                                                    </asp:LinkButton>
                                                                </span>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="gallery centeralign">
                                                <asp:LinkButton ID="lnkbtnOrderPaper1" class="grey2_btn centeralign" runat="server"
                                                    Style="display: none"></asp:LinkButton>
                                                <asp:LinkButton ID="lnkbtnOrderPaper" CommandName="AddNotes" runat="server" CssClass="grey2_btn centeralign"><span>+ Upload Documents</span></asp:LinkButton>
                                                <at:ModalPopupExtender ID="ModalPopupExtenderOrder" TargetControlID="lnkbtnOrderPaper"
                                                    BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlNotesOrder"
                                                    CancelControlID="A3">
                                                </at:ModalPopupExtender>
                                            </div>
                                            <div>
                                                <asp:Panel ID="pnlNotesOrder" runat="server" Style="display: none;" >
                                                    <div class="pp_pic_holder facebook" style="display: block; width: 450px; position:fixed;left:35%;top:30%;">
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
                                                                    <div class="pp_content" style="height: 300px; display: block;">
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
                                                                                    <div class="form_popup_box">
                                                                                        <div class="label_bar" style="padding-left: 9px;">
                                                                                            <span class="input_label">Invoice Number : </span>
                                                                                            <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="popup_input"></asp:TextBox>
                                                                                        </div>
                                                                                        <div class="label_bar" style="padding-left: 9px;">
                                                                                            <span>Select File Type:</span>
                                                                                            <asp:DropDownList runat="server" ID="ddlFileType" AutoPostBack="true" OnSelectedIndexChanged="ddlFileType_Changed">
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                        <div class="label_bar" style="padding-left: 9px;" id="filetypeDiv" runat="server">
                                                                                            <span>Select Vendor: &nbsp;&nbsp;&nbsp;</span>
                                                                                            <asp:DropDownList runat="server" ID="ddlVendor">
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                        <div class="form_box" style="background-color: #E1E0E0; padding-left: 9px;">
                                                                                            <span class="input_label">Upload Document : </span>
                                                                                            <input id="fpSupplierPurchadOrderUpload" type="file" runat="server" style="border: medium none;
                                                                                                color: Black; padding: 2px" />
                                                                                            <div class="spacer15">
                                                                                            </div>
                                                                                            <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                                                                                                <img src="../Images/lightbulb.gif" alt="note:" />&nbsp;Supported file formats are
                                                                                                <b>.pdf</b></div>
                                                                                        </div>
                                                                                        <div align="center">
                                                                                            <asp:LinkButton ID="lnkbtnDocument" CommandName="SAVEDOCUMENT" class="grey2_btn"
                                                                                                runat="server" OnClick="lnkbtnDocument_Click"><span>Save</span></asp:LinkButton>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="pp_details clearfix" style="width: 371px;">
                                                                                <a href="#" id="A3" runat="server" class="pp_close">Close</a>
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
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
