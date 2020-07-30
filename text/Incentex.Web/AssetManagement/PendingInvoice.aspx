<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="PendingInvoice.aspx.cs" Inherits="AssetManagement_PendingInvoice" Title="Pending Invoice" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>

    <script type="text/javascript" language="javascript">
//        function ApproveConfirmation() {
//            if (confirm("Do you want to Approve these Invoices?") == true)
//                return true;
//            else
//                return false;
//        }
        
         function DeleteConfirmation() {
            if (confirm("Are you sure, you want to Delete selected records ?") == true)
                return true;
            else
                return false;
        }

        function SelectAllCheckboxesSpecific(spanChk) {
            var IsChecked = spanChk.checked;
            var Chk = spanChk;
            Parent = document.getElementById('ctl00_ContentPlaceHolder1_gvEquipment');
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != Chk && items[i].type == "checkbox") {
                    if (items[i].checked != IsChecked) {
                        items[i].click();
                    }
                }
            }
        }
    </script>

    <asp:UpdatePanel runat="server" ID="upnlCompanyStore">
        <Triggers>
            <asp:PostBackTrigger ControlID="gvEquipment" />
        </Triggers>
        <ContentTemplate>
            <div class="form_pad">
                <div>
                  
                    <div style="text-align: center; color: Red; font-size: larger;">
                        <asp:Label ID="lblmsg" runat="server">
                        </asp:Label>
                    </div>
                    <asp:GridView ID="gvEquipment" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="gvEquipment_RowCommand"
                        OnRowDataBound="gvEquipment_RowDataBound">
                        <Columns>
                           
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span>&nbsp;&nbsp;<asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this);"
                                        runat="server" />
                                        &nbsp; </span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="first">&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                    </span>
                                </ItemTemplate>
                                 <ItemStyle CssClass="b_box"  VerticalAlign="Middle"  Width="5%" />
                            </asp:TemplateField>
                             <asp:TemplateField Visible="False" >                               
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEquipmentMaintenanceCostID" Text='<%# Eval("EquipmentMaintenanceCostID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>                             
                            <asp:TemplateField SortExpression="EquipmentVendorName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnEquipmentVendorName" runat="server" CommandArgument="EquipmentVendorName" CommandName="Sort"><span >Company</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderEquipmentVendorName" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfEquipmentVendorName" runat="server" Value='<%# Eval("EquipmentVendorName")%>' />
                                    <asp:Label runat="server" ID="lblEquipmentVendorName" Text='<%# "&nbsp;" + Eval("EquipmentVendorName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="25%" />
                            </asp:TemplateField> 
                             <asp:TemplateField SortExpression="AssetType">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnAssetType" runat="server" CommandArgument="AssetType" CommandName="Sort"><span >Asset Type</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderAssetType" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfAssetType" runat="server" Value='<%# Eval("AssetType")%>' />
                                    <asp:Label runat="server" ID="lblAssetType" Text='<%# "&nbsp;" + Eval("AssetType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="15%" />
                            </asp:TemplateField> 
                             <asp:TemplateField SortExpression="EquipmentID">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnEquipmentID" runat="server" CommandArgument="EquipmentID" CommandName="Sort"><span >Asset ID</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderEquipmentID" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfEquipmentID" runat="server" Value='<%# Eval("EquipmentID")%>' />
                                    <asp:Label runat="server" ID="lblEquipmentID" Text='<%# "&nbsp;" + Eval("EquipmentID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="20%" />
                            </asp:TemplateField>                          
                            <asp:TemplateField SortExpression="Invoice">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnInvoice" runat="server" CommandArgument="Invoice" CommandName="Sort"><span >Invoice Number</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderInvoice" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfInvoice" runat="server" Value='<%# Eval("Invoice")%>' />
                                    <asp:Label runat="server" ID="lblInvoice" Text='<%# "&nbsp;" + Eval("Invoice") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="15%" />
                            </asp:TemplateField> 
                             <asp:TemplateField SortExpression="Amount">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnAmount" runat="server" CommandArgument="Amount" CommandName="Sort"><span >Invoice Amount</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderAmount" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfAmount" runat="server" Value='<%# Eval("Amount")%>' />
                                    <asp:Label runat="server" ID="lblAmount" Text='<%# "&nbsp;" + Eval("Amount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="15%" />
                            </asp:TemplateField> 
                            
                             
                               <asp:TemplateField>
                                            <HeaderTemplate>
                                                <span class="centeralign">File</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span class="btn_space">
                                                    <asp:ImageButton ID="lnkbtnPDF" runat="server" CommandName="PDF" CommandArgument='<%# Eval("EquipmentMaintenanceCostID") %>' /></span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                        </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div runat="server" id="dvBottom">
                    <div class="companylist_botbtn alignleft">
                        <asp:LinkButton ID="btnApprove" CssClass="grey_btn" runat="server" TabIndex="0" 
                            OnClick="btnApprove_Click"><span>Approve</span></asp:LinkButton>
            <asp:LinkButton ID="btnApproveDummy" CssClass="grey_btn" runat="server" TabIndex="0" Style="display: none;"></asp:LinkButton>
                    <at:ModalPopupExtender ID="modal" TargetControlID="btnApproveDummy" BackgroundCssClass="modalBackground"
                                    DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="closepopup">
                                </at:ModalPopupExtender>
                                <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
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
                                                    <div class="pp_content" style="height: 150px; display: block;">
                                                        <div class="pp_loaderIcon" style="display: none;">
                                                        </div>
                                                        <div class="pp_fade" style="display: block;">
                                                            <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                            <div class="pp_hoverContainer" style="height: 50px; width: 370px; display: none;">
                                                                <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                                    style="visibility: visible;">previous</a>
                                                            </div>
                                                            <div id="pp_full_res">
                                                                <div class="pp_inline clearfix">
                                                                    <div class="form_popup_box">
                                                                       
                                                                        <table cellpadding="5" cellspacing="5">
                                                                           
                                                                            <tr>
                                                                           
                                                                            <td>
                                                                                <asp:Label ID="lblInvoiceMail" runat="server"></asp:Label> 
                                                                             </td>
                                                                            </tr>
                                                                           
                                                                            <tr>
                                                                                <td >
                                                                                    <div class="label_bar btn_padinn" style="margin-right: 3px;">
                                                                                       <br /> <asp:LinkButton ID="btnYes" CssClass="grey2_btn" runat="server" OnClick="btnYes_Click"><span>Yes</span></asp:LinkButton>
                                                                                         <asp:LinkButton ID="btnNo" CssClass="grey2_btn" runat="server" OnClick="btnNo_Click"><span>No</span></asp:LinkButton>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                            </div>
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
                     <div class="companylist_botbtn">
                        <asp:LinkButton ID="btnDelete" CssClass="grey_btn" runat="server" TabIndex="0" OnClientClick="return DeleteConfirmation();"
                            OnClick="btnDelete_Click"><span>Delete</span></asp:LinkButton>
                    </div>
                    <div id="pagingtable" runat="server" class="alignright pagging">
                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                        </asp:LinkButton>
                        <span>
                            <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList></span>
                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
            <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

