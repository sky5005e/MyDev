<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MyUserInformation.aspx.cs" Inherits="MyAccount_MySettings_MyUserInformation"
    Title="User Information Section" %>

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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script type="text/javascript" language="javascript">
    $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
            $().ready(function() {
                $("#aspnetForm").validate({
                    rules: {                       
                        ctl00$ContentPlaceHolder1$TxtFirstName: { required: true },
                        ctl00$ContentPlaceHolder1$TxtLastName: { required: true },
                        ctl00$ContentPlaceHolder1$TxtAddress: { required: true },
                        ctl00$ContentPlaceHolder1$DrpCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$DrpState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$DrpCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$TxtZip: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$TxtEmail: { required: true, email: true },
                        ctl00$ContentPlaceHolder1$TxtTelephone: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$TxtMobile: { alphanumeric: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$TxtFirstName: { required: replaceMessageString(objValMsg, "Required", "first name") },
                        ctl00$ContentPlaceHolder1$TxtLastName: { required: replaceMessageString(objValMsg, "Required", "last name") },
                        ctl00$ContentPlaceHolder1$TxtAddress: { required: replaceMessageString(objValMsg, "Required", "Address1") },
                        ctl00$ContentPlaceHolder1$DrpCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") },
                        ctl00$ContentPlaceHolder1$DrpState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") },
                        ctl00$ContentPlaceHolder1$DrpCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") },
                        ctl00$ContentPlaceHolder1$TxtZip: { required: replaceMessageString(objValMsg, "Required", "Zipcode"), alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$TxtEmail: { required: replaceMessageString(objValMsg, "Required", "email"), email: replaceMessageString(objValMsg, "Valid", "email address") },
                        ctl00$ContentPlaceHolder1$TxtTelephone: { required: replaceMessageString(objValMsg, "Required", "telephone number"), alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$TxtMobile: { alphanumeric: replaceMessageString(objValMsg, "Number", "") }
                    }
                });                
            });
                              
        $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
//                if ($("#<%=DrpCity.ClientID %> option:selected").attr('value') == "-Other-") {                    
//                    $("#ctl00_ContentPlaceHolder1_txtCity").rules("add", {
//                            required: true,
//                            messages: {
//                                    required: replaceMessageString(objValMsg, "Required", "city name")
//                            }
//                    });
//                    }
//                else {                    
//                     $("#ctl00_ContentPlaceHolder1_txtCity").rules("remove");
//                }
            
                return $('#aspnetForm').valid();
            });  
            
        }); 
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) 
            {
                assigndesign();
            }
    </script>

    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <div class="centeralign">
            <div class="centeralign">
                 <table class="form_table centeralign" width="100%" cellpadding="0" cellspacing="0">
                    <tr>  
                    <td width="3%"></td>                 
                        <td class="formtd" width="50%">
                            <table>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Company</span>
                                                <asp:TextBox ID="TxtCompanyName" TabIndex="1" runat="server" CssClass="w_label"></asp:TextBox>
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
                                                <span class="input_label">Last Name</span>
                                                <asp:TextBox ID="TxtLastName" TabIndex="3" runat="server" CssClass="w_label"></asp:TextBox>
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
                                                <span class="input_label">Address2</span>
                                                <asp:TextBox ID="TxtAddress2" runat="server" TabIndex="5" CssClass="w_label"></asp:TextBox>
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
                                                <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="DrpState" runat="server" TabIndex="7" onchange="pageLoad(this,value);"
                                                        AutoPostBack="True" OnSelectedIndexChanged="DrpState_SelectedIndexChanged">
                                                        <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
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
                                                <span class="input_label">Zip</span>
                                                <asp:TextBox ID="TxtZip" runat="server" TabIndex="10" CssClass="w_label"></asp:TextBox>
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
                                                <span class="input_label">Mobile</span>
                                                <asp:TextBox ID="TxtMobile" runat="server" TabIndex="12" CssClass="w_label"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="formtd" width="47%">
                            <table>
                                 <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">First Name</span>
                                                <asp:TextBox ID="TxtFirstName" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
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
                                                <span class="input_label">Address1</span>
                                                <asp:TextBox ID="TxtAddress" runat="server" TabIndex="4" CssClass="w_label"></asp:TextBox>
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
                                                <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="DrpCountry" TabIndex="6" runat="server" onchange="pageLoad(this,value);"
                                                        AutoPostBack="True" OnSelectedIndexChanged="DrpCountry_SelectedIndexChanged">
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
                                                <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="DrpCity" runat="server" TabIndex="8" onchange="pageLoad(this,value);"
                                                        AutoPostBack="true" OnSelectedIndexChanged="DrpCity_SelectedIndexChanged">
                                                        <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="PnlCityOther" runat="server" visible="false">
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">City Name</span>
                                                <asp:TextBox ID="txtCity" runat="server" TabIndex="9" CssClass="w_label"></asp:TextBox>
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
                                                <span class="input_label">Telephone</span>
                                                <asp:TextBox ID="TxtTelephone" runat="server" TabIndex="11" CssClass="w_label"></asp:TextBox>
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
                                                <span class="input_label">Company Email</span>
                                                <asp:TextBox ID="TxtEmail" runat="server" TabIndex="14" CssClass="w_label"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>                        
                    </tr>
                </table>
            </div>
        </div>
        <div class="spacer20">
        </div>
        <div class="centeralign">
            <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" TabIndex="15"
                ToolTip="Save Basic Information" OnClick="lnkBtnSaveInfo_Click"><span>Save</span></asp:LinkButton>
        </div>
    </div>  
</asp:Content>

