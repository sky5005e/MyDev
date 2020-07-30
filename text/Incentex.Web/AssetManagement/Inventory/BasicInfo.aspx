<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="BasicInfo.aspx.cs" Inherits="AssetManagement_Inventory_BasicInfo" Title="Basic Info" %>

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
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlVendor: { NotequalTo: "0" }
                    },
                    messages:
                    {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "Company") },
                        ctl00$ContentPlaceHolder1$ddlVendor: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "Vendor") }
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
                                <span class="input_label">Company</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true">
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
                                <span class="input_label">Vendor</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlVendor" runat="server" AutoPostBack="true">
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
                            <span class="input_label" style="width: 38%">Base Station</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlBaseStation" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </label>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
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
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
