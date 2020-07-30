<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="ItemInfo.aspx.cs" Inherits="AssetManagement_Inventory_ItemInfo" Title="ItemInfo" %>
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
        $().ready(function() {
             $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                 objValMsg = $.xml2json(xml); $("#aspnetForm").validate(
                {
                   rules:
                    {                        
                        ctl00$ContentPlaceHolder1$txtPartNumber: { required: true },
                         ctl00$ContentPlaceHolder1$txtMFGPartNumber: { required: true }
                    },
                    messages:
                    {
                        ctl00$ContentPlaceHolder1$txtPartNumber: { required: replaceMessageString(objValMsg, "Required", "Part Number") },
                        ctl00$ContentPlaceHolder1$txtMFGPartNumber: { required: replaceMessageString(objValMsg, "Required", "MFG Part Number") }
                    }
                });
             });
             $("#<%=lnkBtnNext.ClientID %>").click(function() {
           
                 return $('#aspnetForm').valid();
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
        <asp:UpdatePanel ID="up1" runat="server">               
                <ContentTemplate>
            <table class="dropdown_pad ">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Category</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" 
                                    onselectedindexchanged="ddlCategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </span>
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
                                <span class="input_label">Sub Category</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlSubCategory" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>   
                </tr>
                 <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 36%">Part Number</span>&nbsp;
                            <asp:TextBox ID="txtPartNumber" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
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
                            <span class="input_label" style="width: 36%">Manufacture's Part Number</span>&nbsp;
                            <asp:TextBox ID="txtMFGPartNumber" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                  <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box employeeedit_text clearfix">
                                                    <span class="input_label alignleft">Description</span>
                                                    <div class="textarea_box alignright">
                                                        <div class="scrollbar">
                                                            <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                                                class="scrollbottom"></a>
                                                        </div>
                                                        <asp:TextBox ID="txtProductDescription" runat="server" CssClass="scrollme" TextMode="MultiLine"
                                                         MaxLength="20"   ></asp:TextBox>
                                                    </div>
                                                    <div id="divtxtAddress">
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                 <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 36%">Product Cost</span>&nbsp;
                            <asp:TextBox ID="txtProductCost" runat="server" MaxLength="8" CssClass="w_label"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
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
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

