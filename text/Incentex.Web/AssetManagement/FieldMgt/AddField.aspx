<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AddField.aspx.cs" Inherits="AssetManagement_FieldMgt_AddField" Title="Add Field" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    
     <script type="text/javascript" language="javascript">
         $().ready(function() {
             $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                 objValMsg = $.xml2json(xml); $("#aspnetForm").validate(
                {
                    rules:
                    {             
                     ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                     ctl00$ContentPlaceHolder1$ddlEquipmentType: { NotequalTo: "0" },           
                        ctl00$ContentPlaceHolder1$txtField: { required: true }
                    },
                    messages:
                    {
                    ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "Company") },
                    ctl00$ContentPlaceHolder1$ddlEquipmentType: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "Equipment Type") },
                        ctl00$ContentPlaceHolder1$txtField: { required: replaceMessageString(objValMsg, "Required", "Field") }
                    }
                });
             });
             $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                 return $('#aspnetForm').valid();
             });
         });
    </script>


        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
         <div class="form_pad">      
        <div class="form_table">
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
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 38%">Equipment Type</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlEquipmentType" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);">
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
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 38%">Field</span>
                            <asp:TextBox ID="txtField" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
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
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" 
                            ToolTip="Save Basic Information" onclick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
                        <%--<a href="#" class="grey_btn" title="Save Information"><span>Save Information</span></a>--%>
                    </td>
                </tr>
            </table>
        </div>
       
    </div>
</asp:Content>

