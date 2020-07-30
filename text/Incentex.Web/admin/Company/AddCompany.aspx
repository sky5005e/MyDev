<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="AddCompany.aspx.cs" Inherits="admin_Company_AddCompany" Title="Company>>Add Company" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="manuControl" runat="server" />

    <script type="text/javascript" language="javascript">
        $(function() {
            scrolltextarea(".scrollme2", "#Scrolltop2", "#ScrollBottom2");

        });
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        //For Company
                        ctl00$ContentPlaceHolder1$txtCompanyName: { required: true },
                        ctl00$ContentPlaceHolder1$txtAddress: { required: true },
                        ctl00$ContentPlaceHolder1$txtdFirstName: { required: true },
                        ctl00$ContentPlaceHolder1$txtdLastName: { required: true },

                        //sATRT  country state and city validation
                        ctl00$ContentPlaceHolder1$ddldCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddldState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddldCity: { NotequalTo: "0" },
                        //
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "0" },


                        //Accounting
                        ctl00$ContentPlaceHolder1$txtdEmail: { required: true, email: true },

                        //ctl00$ContentPlaceHolder1$txtdTelephone: { required: true },
                        ctl00$ContentPlaceHolder1$txtdTelephone: { required: true },
                        ctl00$ContentPlaceHolder1$ddlPaymentTerms: { NotequalTo: "0" },
                        //Company
                        ctl00$ContentPlaceHolder1$txtEmail: { required: true, email: true },
                        //ctl00$ContentPlaceHolder1$txtTelephone: { required: true}
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: true },
                        ctl00$ContentPlaceHolder1$txtWebSite: { url: true }

                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtCompanyName: {
                            required: replaceMessageString(objValMsg, "Required", "company name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        },
                        ctl00$ContentPlaceHolder1$txtAddress: {
                            required: replaceMessageString(objValMsg, "Required", "Address")
                        },
                        ctl00$ContentPlaceHolder1$txtdFirstName: {
                            required: replaceMessageString(objValMsg, "Required", "first name")

                        },
                        ctl00$ContentPlaceHolder1$txtdLastName: {
                            required: replaceMessageString(objValMsg, "Required", "last name")

                        },


                        ctl00$ContentPlaceHolder1$txtEmail: {
                            required: replaceMessageString(objValMsg, "Required", "email"),
                            email: replaceMessageString(objValMsg, "Valid", "email address")
                        },
                        ctl00$ContentPlaceHolder1$txtdEmail: {
                            required: replaceMessageString(objValMsg, "Required", "email"),
                            email: replaceMessageString(objValMsg, "Valid", "email address")
                        },

                        //Satrt Country Sate AND cITY
                        ctl00$ContentPlaceHolder1$ddlCountry: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country")
                        },
                        ctl00$ContentPlaceHolder1$ddldCountry: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country")
                        },



                        ctl00$ContentPlaceHolder1$ddlState: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state")
                        },
                        ctl00$ContentPlaceHolder1$ddldState: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state")
                        },

                        ctl00$ContentPlaceHolder1$ddlCity: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city")
                        },

                        ctl00$ContentPlaceHolder1$ddldCity: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city")
                        },
                        ctl00$ContentPlaceHolder1$ddlPaymentTerms: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "Payment terms")
                        },


                        //End
                        //Telephone
                        ctl00$ContentPlaceHolder1$txtTelephone: {
                            required: replaceMessageString(objValMsg, "Required", "telephone")
                            //number: replaceMessageString(objValMsg, "Number", "")
                        },
                        ctl00$ContentPlaceHolder1$txtdTelephone: {
                            required: replaceMessageString(objValMsg, "Required", "telephone number")
                            // number: replaceMessageString(objValMsg, "Number", "")
                        },
                        ctl00$ContentPlaceHolder1$txtWebSite: {
                            url: "Please enter valid url"
                        }

                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtAddress")
                            error.insertAfter("#dvtxtAddress");
                        else
                            error.insertAfter(element);
                    }
                });
                $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                    return $('#aspnetForm').valid();
                });
            });
        });
   
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <div class="form_pad">
        <div>
            <h4>
                Customer Headquarters Information</h4>
            <table class="form_table">
                <tr>
                    <td rowspan="3">
                        <div style="text-align: center; color: Red; font-size: larger;">
                            <asp:Label ID="lblmsg" runat="server">
                            </asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
            <table class="form_table" style="vertical-align: top">
                <tr>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Company Name</span>
                                            <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="100" TabIndex="0" CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <asp:UpdatePanel ID="upnlCountry" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCountry" runat="server" onchange="pageLoad(this,value);"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"
                                                            TabIndex="3">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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
                                            <asp:TextBox ID="txtZip" runat="server" CssClass="w_label" MaxLength="100" TabIndex="6"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box employeeedit_text clearfix">
                                            <span class="input_label alignleft">Address</span>
                                            <div class="textarea_box alignright">
                                                <div class="scrollbar">
                                                    <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                                    </a>
                                                </div>
                                                <textarea id="txtAddress" rows="3" runat="server" class="scrollme2" tabindex="1"></textarea>
                                            </div>
                                            <div id="dvtxtAddress">
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
                                        <asp:UpdatePanel ID="upnlState" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">State/Province</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlState" runat="server" onchange="pageLoad(this,value);" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlState_SelectedIndexChanged" TabIndex="4">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div>
                                                        <asp:UpdateProgress ID="UpdateProgress4" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upnlCountry">
                                                            <ProgressTemplate>
                                                                <img src="~/Images/ajaxbtn.gif" class="progress_img" /></ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Web-site</span>
                                            <asp:TextBox ID="txtWebSite" runat="server" CssClass="w_label" MaxLength="100" TabIndex="7"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd_r">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Telephone</span>
                                            <asp:TextBox ID="txtTelephone" CssClass="w_label" runat="server" MaxLength="100"
                                                TabIndex="2"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <asp:UpdatePanel ID="upnlCity" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCity" runat="server" onchange="pageLoad(this,value);" AutoPostBack="True"
                                                            TabIndex="5">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div>
                                                        <asp:UpdateProgress ID="UpdateProgress6" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upnlCountry">
                                                            <ProgressTemplate>
                                                                <img src="~/Images/ajaxbtn.gif" class="progress_img" /></ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider">
        </div>
        <h4>
            Accounting Department Information</h4>
        <div>
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">First Name</span>
                                            <asp:TextBox ID="txtdFirstName" runat="server" CssClass="w_label" MaxLength="100"
                                                TabIndex="8"></asp:TextBox>
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
                                        <div class="form_box employeeedit_text clearfix">
                                            <span class="input_label alignleft">Address</span>
                                            <div class="textarea_box alignright">
                                                <div class="scrollbar">
                                                    <a href="#scroll" id="Scrolltop2" class="scrolltop"></a><a href="#scroll" id="ScrollBottom2"
                                                        class="scrollbottom"></a>
                                                </div>
                                                <textarea name="" id="txtdAddress" rows="3" runat="server" class="scrollme2" tabindex="11"></textarea>
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
                                        <asp:UpdatePanel ID="upnldCountry" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddldCountry" runat="server" onchange="pageLoad(this,value);"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddldCountry_SelectedIndexChanged"
                                                            TabIndex="14">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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
                                            <asp:TextBox ID="txtdZip" runat="server" CssClass="w_label" MaxLength="100" TabIndex="17"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Last Name</span>
                                            <asp:TextBox ID="txtdLastName" runat="server" MaxLength="100" CssClass="w_label"
                                                TabIndex="9"></asp:TextBox>
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
                                            <asp:TextBox ID="txtdMobile" runat="server" MaxLength="100" CssClass="w_label" TabIndex="12"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <asp:UpdatePanel ID="upnldState" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">State/Province</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddldState" runat="server" onchange="pageLoad(this,value);"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddldState_SelectedIndexChanged" TabIndex="15">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div>
                                                        <asp:UpdateProgress ID="UpdateProgress3" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upnlCountry">
                                                            <ProgressTemplate>
                                                                <img src="~/Images/ajaxbtn.gif" class="progress_img" /></ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Extension</span>
                                            <asp:TextBox ID="txtdExtension" runat="server" CssClass="w_label" MaxLength="100"
                                                TabIndex="18"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd_r">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Title</span>
                                            <asp:TextBox ID="txtdTitle" runat="server" CssClass="w_label" MaxLength="50" TabIndex="10"></asp:TextBox>
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
                                            <asp:TextBox ID="txtdTelphone" runat="server" MaxLength="100" CssClass="w_label"
                                                TabIndex="13"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <asp:UpdatePanel ID="upnldcity" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddldCity" runat="server" onchange="pageLoad(this,value);" AutoPostBack="True"
                                                            TabIndex="16">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div>
                                                        <asp:UpdateProgress ID="UpdateProgress5" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upnlCountry">
                                                            <ProgressTemplate>
                                                                <img src="~/Images/ajaxbtn.gif" class="progress_img" /></ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Fax</span>
                                            <asp:TextBox ID="txtdFax" runat="server" MaxLength="100" CssClass="w_label" TabIndex="19"></asp:TextBox>
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
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Email</span>
                                <asp:TextBox ID="txtdEmail" runat="server" CssClass="w_label" MaxLength="100" TabIndex="20"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Payment Terms</span> <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlPaymentTerms" runat="server" onchange="pageLoad(this,value);"
                                                AutoPostBack="true" TabIndex="21">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        <div class="form_table">
            <div class="form_top_co">
                <span>&nbsp;</span></div>
            <div class="form_box taxt_area clearfix" style="height: 180px">
                <span class="input_label alignleft" style="height: 178px">Notes/History</span>
                <div class="textarea_box alignright">
                    <div class="scrollbar" style="height: 182px">
                        <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                            class="scrollbottom"></a>
                    </div>
                    <asp:TextBox ID="txtNoteHistory" runat="server" TabIndex="30" TextMode="MultiLine"
                        CssClass="scrollme1" Height="178px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="form_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div class="alignnone spacer15">
        </div>
        <div class="rightalign gallery">
            <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
            <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="grey2_btn alignright" OnClick="lnkAddNew_Click"><span>+ Add Note</span></asp:LinkButton>
            <at:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
                DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="closepopup">
            </at:ModalPopupExtender>
        </div>
        <asp:Panel ID="pnlNotes" runat="server" Style="display: none;">
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
                            <div class="pp_content" style="height: 228px; display: block;">
                                <div class="pp_loaderIcon" style="display: none;">
                                </div>
                                <div class="pp_fade" style="display: block;">
                                    <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                    <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                            style="visibility: visible;">previous</a>
                                    </div>
                                    <div id="pp_full_res">
                                        <div class="pp_inline clearfix">
                                            <div class="form_popup_box">
                                                <div class="label_bar">
                                                    <span>Add Notes / History
                                                        <br />
                                                        <br />
                                                        <asp:TextBox Height="120" Width="350" TextMode="MultiLine" ID="txtNote" runat="server"></asp:TextBox></span>
                                                </div>
                                                <div>
                                                    <asp:LinkButton ID="lnkButton" class="grey2_btn" runat="server" OnClick="btnSubmit_Click"><span>Save Notes</span></asp:LinkButton>
                                                    <%-- <asp:Button ID="btnSubmit" class="grey2_btn" Text="Add Note" runat="server"  OnClick="btnSubmit_Click"  />--%>
                                                </div>
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
        <table>
            <tr>
                <td>
                    <div>
                        <div class="botbtn centeralign">
                            <input type="checkbox" id="chkCentralized" runat="server" tabindex="40" visible="false" />
                            <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Save Information"
                                OnClick="lnkBtnSaveInfo_Click" TabIndex="22"><span>Save Information</span></asp:LinkButton>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
