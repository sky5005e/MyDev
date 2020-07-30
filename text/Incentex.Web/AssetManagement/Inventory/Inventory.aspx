<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="Inventory.aspx.cs" Inherits="AssetManagement_Inventory_Inventory" Title="Inventory" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        } 
         $(function() {
            $(".datepicker").datepicker({
                buttonText: 'Date',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });      
    </script>
   

    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div style="text-align: center">
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
    </div>
    <div class="form_pad">
        <div class="form_table">       
            <table class="dropdown_pad ">               
                 <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 36%">Current Inventory</span>&nbsp;
                            <asp:TextBox ID="txtCurrentInventory" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
                               <at:FilteredTextBoxExtender ID="ftbCurrentInventory" TargetControlID="txtCurrentInventory" runat="server"
                               FilterType="Custom,Numbers" ValidChars="."> </at:FilteredTextBoxExtender>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 36%">Re-Order Level</span>&nbsp;
                            <asp:TextBox ID="txtReOrderLevel" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>                  
                 <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 36%">Supplier Inventory:</span>&nbsp;
                            <asp:TextBox ID="txtSupplierInventory" runat="server" MaxLength="8" CssClass="w_label"></asp:TextBox>
                             <at:FilteredTextBoxExtender ID="ftbSupplierInventory" TargetControlID="txtSupplierInventory" runat="server"
                               FilterType="Custom,Numbers" ValidChars="."> </at:FilteredTextBoxExtender>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 36%">On-Order with Supplier</span>&nbsp;
                            <asp:TextBox ID="txtOnOrderSupplier" runat="server" MaxLength="8" CssClass="w_label"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                        <td class="form_table" style="padding-top: 2px">
                            <div class="calender_l" style="width: 450px">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box supplier_annual_date">
                                    <span class="input_label">Expected Delivery Date</span>
                                    <asp:TextBox ID="txtExpDeliveryDate" runat="server" CssClass="w_label datepicker min_w"
                                        TabIndex="1"></asp:TextBox>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                <tr>
                    <td class="spacer10">
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnNext" class="grey2_btn" runat="server" OnClick="lnkBtnNext_Click"><span>Next</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

