<%@ Page Title="Vendor Management" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="VendorManagement.aspx.cs" Inherits="AssetManagement_VendorManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
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
    <style type="text/css">
        .textarea_box textarea
        {
            height: 178px;
        }
        .textarea_box
        {
            height: 178px;
        }
        .textarea_box .scrollbar
        {
            height: 185px;
        }
    </style>

    <script type="text/javascript" language="javascript">
         $().ready(function() {
             $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                 objValMsg = $.xml2json(xml); $("#aspnetForm").validate(
                {
                    rules:
                    {                        
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtVendorName: { required: true },       
                        ctl00$ContentPlaceHolder1$txtEmail: { email: true }
                    },
                    messages:
                    {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "Company") }, 
                        ctl00$ContentPlaceHolder1$txtVendorName: { required: replaceMessageString(objValMsg, "Required", "Vendor Name") },     
                        ctl00$ContentPlaceHolder1$txtEmail: {email: replaceMessageString(objValMsg, "Email", "")}
                    }
                });
             });
             $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
            
                 return $('#aspnetForm').valid();
             });
         });
    </script>

    <div class="alignnone">
        &nbsp;</div>
    <div class="form_pad">
        <div class="centeralign">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="form_table">
            <table class="dropdown_pad">
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
                            <span class="input_label" style="width: 36%">Vendor Company </span>&nbsp;
                            <asp:TextBox ID="txtVendorName" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
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
                            <span class="input_label" style="width: 36%">Owner&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </span>&nbsp;<asp:TextBox ID="txtContact" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
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
                            <span class="input_label" style="width: 36%">Address1 </span>&nbsp;
                            <asp:TextBox ID="txtAddress1" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
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
                            <span class="input_label" style="width: 36%">Address2 </span>&nbsp;
                            <asp:TextBox ID="txtAddress2" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
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
                            <span class="input_label" style="width: 36%">Country</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlCountry" runat="server" onchange="pageLoad(this,value);"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
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
                            <span class="input_label" style="width: 36%">State</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlState" runat="server" onchange="pageLoad(this,value);" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
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
                            <span class="input_label" style="width: 36%">City</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlCity" runat="server" onchange="pageLoad(this,value);">
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
                            <span class="input_label" style="width: 36%">Zip </span>&nbsp;
                            <asp:TextBox ID="txtZip" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
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
                            <span class="input_label" style="width: 36%">Telephone</span>
                            <asp:TextBox ID="txtTelephone" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
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
                            <span class="input_label" style="width: 36%">Fax </span>&nbsp;
                            <asp:TextBox ID="txtFax" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 36%">Email </span>&nbsp;
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
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
                            <span class="input_label" style="width: 36%">Base Station</span>
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
                <%--<tr>
                <td class="centeralign">                
                        <asp:LinkButton ID="btnAddEmployee" CssClass="grey_btn" runat="server" 
                           OnClick="btnAddEmployee_Click"><span>Add Employee</span></asp:LinkButton>
                           &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="btnViewEmployee" CssClass="grey_btn" runat="server" 
                            OnClick="btnViewEmployee_Click"><span>View Employee</span></asp:LinkButton>
                   
                </td>
                </tr>--%>
                <tr>
                    <td class="spacer25">
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Save Basic Information"
                            OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <%--<table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td width="20%">
                    </td>
                    <td width="20%">
                        <div class="btn_width_small">
                            <asp:LinkButton ID="LinkButton2" class="gredient_btnMainPage" title="Flag Asset"
                                runat="server">
                <img src="../admin/Incentex_Used_Icons/SupportLocations.png" alt="World-Link System Control" />
                <span>               
                    Support Locations                
                </span>
                            </asp:LinkButton>
                        </div>
                    </td>
                    <td width="20%">
                        <div class="btn_width_small">
                            <asp:LinkButton ID="lnkFlagAsset" class="gredient_btnMainPage" title="Flag Asset"
                                runat="server">
                <img src="../admin/Incentex_Used_Icons/EvaluationReport.png" alt="World-Link System Control" />
                <span>               
                    Evaluation Reports                
                </span>
                            </asp:LinkButton>
                        </div>
                    </td>
                    <td width="20%">
                        <div class="btn_width_small">
                            <asp:LinkButton ID="LinkButton1" class="gredient_btnMainPage" title="Flag Asset"
                                runat="server">
                <img src="../admin/Incentex_Used_Icons/SystemAccess.png" alt="World-Link System Control" />
                <span>               
                    System Access                
                </span>
                            </asp:LinkButton>
                        </div>
                    </td>
                    <td width="20%">
                    </td>
                </tr>
            </table>--%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
