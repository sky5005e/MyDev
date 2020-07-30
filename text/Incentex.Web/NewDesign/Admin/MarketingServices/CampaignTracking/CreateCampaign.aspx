<%@ Page Title="incentex | Create Campaign" Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master"
    AutoEventWireup="true" EnableEventValidation="false" CodeFile="CreateCampaign.aspx.cs"
    Inherits="Admin_CreateCampaign" EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        var form_original_data;
        $(document).ready(function() {
            $(window).ValidationUI();

            form_original_data = $("#" + $(".MainDiv").val()).find("input, textarea, select").serialize();
        });           
       
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <input type="hidden" value="admin-link" id="hdnActiveLink" />
    <!-- value='#' is kept intentionally, please do not remove that-->
    <input type="hidden" value="#" id="hdnOpenDivs" runat="server" />
    <!-- value='#' is kept intentionally, please do not remove that-->
    <input type="hidden" value="#" id="hdnCloseDivs" runat="server" />
    <div class="filter-content">
        
        <div id="dvCampTrack" class="account-form-block ParentTab" data-defaultbutton="<%= btnBasicSave.ClientID %>">
        <div class="filter-headbar cf">
            <span class="headbar-title mainAssetTitle" id="title_span" runat="server">Create Campaign</span>
            <a href="javascript:void(0);" onclick="GetHelpVideo('Asset Management','Add New Asset');"
                title="Help Video" class="help-videolink" onclick="ClearFileUploadControl();">Help
                Video</a>
        </div>
            <div class="account-formInner">
                <div class="basic-form" method="post">
                    <div class="cf">
                        <ul class="left-form cf">
                            <li class="alignleft" tabindex="1">
                                <span class="lbl-txt1">Campaign Name</span> 
                                <span class="select-drop basic-drop">
                                    <asp:TextBox ID="txtCampaignName" runat="server" CssClass="input-field-all checkvalidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCampaignName" runat="server" ControlToValidate="txtCampaignName"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="BasicTabValidate"
                                        ErrorMessage="Please Enter Campaign Name"></asp:RequiredFieldValidator>
                                </span>
                            </li>
                            <li class="alignright" tabindex="1">
                                <span class="lbl-txt1">Email Subject</span> 
                                <span class="select-drop basic-drop">
                                    <asp:TextBox ID="txtEmailSubject" runat="server" CssClass="input-field-all checkvalidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvEmailSubject" runat="server" ControlToValidate="txtEmailSubject"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="BasicTabValidate"
                                        ErrorMessage="Please Enter Email Subject"></asp:RequiredFieldValidator>
                                </span>
                            </li>
                            <li class="alignleft" tabindex="1">
                                <span class="lbl-txt1">Sender's Name</span> 
                                <span class="select-drop basic-drop">
                                    <asp:TextBox ID="txtSenderName" runat="server" CssClass="input-field-all checkvalidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSenderName" runat="server" ControlToValidate="txtSenderName"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="BasicTabValidate"
                                        ErrorMessage="Please Enter Sender's Name"></asp:RequiredFieldValidator>
                                </span>
                            </li>
                            <li class="alignright" tabindex="1">
                                <span class="lbl-txt1">Reply to Address</span> 
                                <span class="select-drop basic-drop">
                                    <asp:TextBox ID="txtReplyToAddress" runat="server" CssClass="input-field-all checkvalidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvReplyToAddress" runat="server" ControlToValidate="txtReplyToAddress"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="BasicTabValidate"
                                        ErrorMessage="Please Enter Reply to Address"></asp:RequiredFieldValidator>
                                </span>
                            </li>
                            <li class="alignleft" tabindex="6">
                                <span class="lbl-txt1">Company</span> 
                                <span class="select-drop basic-drop">
                                    <asp:DropDownList ID="ddlCompany" runat="server" class="checkvalidation default">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="ddlCompany"
                                        Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" ValidationGroup="BasicTabValidate"
                                        ErrorMessage="Please select Company"></asp:RequiredFieldValidator>
                                </span>
                            </li>
                            <li class="alignright" tabindex="6">
                                <span class="lbl-txt1">Exclude Companies</span> 
                                <span class="select-drop basic-drop">
                                    <asp:DropDownList ID="ddlExcludeCompanies" runat="server" class="checkvalidation default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li class="alignleft" tabindex="6">
                                <span class="lbl-txt1">Department</span> 
                                <span class="select-drop basic-drop">
                                    <asp:DropDownList ID="ddlDepartment" runat="server" class="checkvalidation default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li class="alignright" tabindex="6">
                                <span class="lbl-txt1">Workgroup</span> 
                                <span class="select-drop basic-drop">
                                    <asp:DropDownList ID="ddlWorkgroup" runat="server" class="checkvalidation default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li class="alignleft" tabindex="6">
                                <span class="lbl-txt1">Employee Type</span> 
                                <span class="select-drop basic-drop">
                                    <asp:DropDownList ID="ddlEmployeeType" runat="server" class="checkvalidation default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                             <li class="alignright" tabindex="6">
                                <span class="lbl-txt1">Employee</span> 
                                <span class="select-drop basic-drop">
                                    <asp:DropDownList ID="ddlEmployee" runat="server" class="checkvalidation default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li class="alignleft" tabindex="6">
                                <span class="lbl-txt1">Gender</span> 
                                <span class="select-drop basic-drop">
                                    <asp:DropDownList ID="ddlGender" runat="server" class="checkvalidation default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li class="alignright" tabindex="6">
                                <span class="lbl-txt1">Contry</span> 
                                <span class="select-drop basic-drop">
                                    <asp:DropDownList ID="ddlCountry" runat="server" class="checkvalidation default">
                                    </asp:DropDownList>
                                </span>                                
                            </li>
                            <li class="alignleft" tabindex="6">
                                <span class="lbl-txt1">Base Station</span> 
                                <span class="select-drop basic-drop">
                                    <asp:DropDownList ID="ddlBaseStation" runat="server" class="checkvalidation default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li class="cf">&nbsp;</li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="asset-btn-block assetbtn-blockbar cf">
                <asp:LinkButton ID="btnBasicSave" CssClass="small-blue-btn submit" runat="server"
                    OnClick="btnBasicSave_Click" ValidationGroup="BasicTabValidate" call="BasicTabValidate"><span>SAVE</span></asp:LinkButton>
                <asp:LinkButton ID="lnkbtnBasicClose" runat="server" CssClass="small-gray-btn" ToolTip="Close"><span>Close</span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
