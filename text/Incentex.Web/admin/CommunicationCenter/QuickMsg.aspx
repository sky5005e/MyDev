<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="QuickMsg.aspx.cs" Inherits="admin_CommunicationCenter_QuickMsg" ValidateRequest="false"
    Title="Communications Center" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ajax__htmleditor_editor_default .ajax__htmleditor_editor_toptoolbar div.ajax__htmleditor_toolbar_button span.ajax__htmleditor_toolbar_selectlable
        {
            color: black;
            font-family: Arial;
            font-size: 12px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        var formats = 'pdf|doc|png|jpg|gif|png';
        $().ready(function() {

            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {

                        ctl00$ContentPlaceHolder1$txtEmail: { required: true, email: true },
                        ctl00$ContentPlaceHolder1$fpAttachment: { required: false, accept: formats }

                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtEmail: {
                            required: replaceMessageString(objValMsg, "Required", "email address"),
                            email: replaceMessageString(objValMsg, "Valid", "email address")


                        },
                        ctl00$ContentPlaceHolder1$fpAttachment: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$fpAttachment")
                            error.insertAfter("#dvFileType");
                        else
                            error.insertAfter(element);
                    }
                });
                $("#<%=lnkSendMail.ClientID %>").click(function() {
                    return $('#aspnetForm').valid();
                });
                $("#<%=btnAddItem.ClientID %>").click(function() {
                    $("#ctl00_ContentPlaceHolder1_txtEmail").rules("remove");
                    if ($("#aspnetForm").valid()) {
                        return true;

                    }
                    else {
                        return false;
                    }
                });

            });
        });
    </script>

    <script language="javascript" type="text/javascript">
        function clickButton(e, buttonid) {
            var bt = document.getElementById(buttonid);
            if (typeof (bt) == 'object') {
                if (navigator.appName.indexOf("Netscape") > -1) {
                    if (e.keyCode == 13) {
                        if (bt && typeof (bt.click) == 'undefined') {
                            bt.click = addClickFunction1(bt);
                        }
                    }
                }
                if (navigator.appName.indexOf("Microsoft Internet Explorer") > -1) {
                    if (event.keyCode == 13) {
                        bt.click();
                        return false;
                    }
                }
            }
        }

        function addClickFunction1(bt) {
            debugger;
            var result = true;
            if (bt.onclick) result = bt.onclick();
            if (typeof (result) == 'undefined' || result) {
                eval(bt.href);
            }
        }
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div class="forgot_box" style="width: 94%;">
            <asp:Label ID="lblMessage" runat="server" CssClass="errormessage">
            </asp:Label>
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="black_middle">
                <div class="clearfix">
                </div>
                <div>
                    <div>
                        <CKEditor:CKEditorControl ID="TxtEmailText" BasePath="../../JS/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                    </div>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <h4 style="margin-bottom: 0px;">
                                    Email Attachment(s)
                                </h4>
                            </td>
                        </tr>
                        <tr id="tdUpload" runat="server">
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <asp:GridView ID="grv" runat="server" Width="60%" AutoGenerateColumns="False" OnRowDeleting="grv_RowDeleting"
                                            ShowHeader="False">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrackingNumber" Text='<%#Eval("imageOnly")%>' runat="server" Height="21px"
                                                            Width="274px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:CommandField ShowDeleteButton="True" />
                                            </Columns>
                                        </asp:GridView>
                                        <div class="spacer25">
                                        </div>
                                        <input id="fpAttachment" type="file" runat="server" />
                                        <asp:Button ID="btnAddItem" runat="server" OnClick="btnAddItem_Click" CssClass=" grey2_btn"
                                            Text="Add item"></asp:Button>
                                        <div id="dvFileType">
                                        </div>
                                        <br />
                                        <br />
                                        <div class="noteIncentexthumb" style="width: 100%; font-size: 12px;">
                                            <img src="../Incentex_Used_Icons/icon_pdf.png" style="z-index: -1" alt="note:" />&nbsp;Supported
                                            Supported file formats are <b>.pdf | .doc | .png | .jpeg | .jpg | .gif </b>
                                        </div>
                                        <br />
                                        <div class="noteIncentexthumb" style="width: 100%; font-size: 12px;">
                                            <img src="../Incentex_Used_Icons/icon_pdf.png" style="z-index: -1" alt="note:" />
                                            Maximum Length of Attachment(s) <b>18 MB </b>
                                        </div>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div class="form_table alignleft">
                        <div class="form_box">
                            <span class="input_label">Email Address</span>
                            <asp:TextBox ID="txtEmail" MaxLength="100" runat="server" CssClass="w_label"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form_bot_co">
                    </div>
                </div>
                <asp:LinkButton ID="lnkSendMail" class="grey2_btn alignright" runat="server" OnClick="lnkSendMailForText_Click"><span>Send Test Now</span></asp:LinkButton>
                <div class="alignnone">
                </div>
            </div>
        </div>
        <div class="spacer25">
        </div>
        <div class="bot_alert next">
            <asp:LinkButton ID="lnkNext" runat="server" title="Final Step" OnClick="lnkNext_Click">Final Step</asp:LinkButton>
        </div>
        <div class="bot_alert prev">
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
