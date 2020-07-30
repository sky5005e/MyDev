<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="AddDocumentStoregeCentre.aspx.cs" Inherits="admin_DocumentStoregeCentre_AddDocumentStoregeCentre"
    Title="Document Storage Center" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../../JS/JQuery/jquery.MultiFile.pack.js" type="text/javascript" language="javascript"></script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        var formats = 'pdf|jpg|jpeg|xls|xlsx|doc';
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddldocumentType: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtFileName: { required: true },
                        ctl00$ContentPlaceHolder1$txtSearchKeyword: { required: true },
                        ctl00$ContentPlaceHolder1$fileupload: { required: true, accept: formats }
                        //                        , filesize: "150" 
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddldocumentType: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "document") },
                        ctl00$ContentPlaceHolder1$txtFileName: { required: replaceMessageString(objValMsg, "Required", "File Name") },
                        ctl00$ContentPlaceHolder1$txtSearchKeyword: { required: replaceMessageString(objValMsg, "Required", "Search Keyword") },
                        ctl00$ContentPlaceHolder1$fileupload: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." }
                        //                        , accept: "File type not supported.", filesize: "Please select file less than 150MB."
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddldocumentType")
                            error.insertAfter("#dvdocumenttype");
                        else
                            error.insertAfter(element);
                    }

                });
            });

            $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                if ($('#aspnetForm').valid()) {
                    $('#dvLoader').show();
                    return true;
                }
                else {
                    return false;
                }
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad select_box_pad" style="width: 400px;">
        <div class="form_table">
            <div class="spacer10">
            </div>
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            <div class="spacer10">
            </div>
            <table>
                <tr id="trDocumentType" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Document Type</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddldocumentType" onchange="pageLoad(this,value);" runat="server"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvdocumenttype">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trFileName" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">File Name </span>
                                <asp:TextBox ID="txtFileName" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trSearchKeyword" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Search Words </span>
                                <asp:TextBox ID="txtSearchKeyword" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="tdUpload" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Upload Files </span>
                                <input type="file" id="fileupload" style="margin: -24px 0px 8px 150px" runat="server"
                                    tabindex="6" class="accept-pdf|jpg|jpeg|xls|xlsx|doc" />
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Save Document"
                            OnClick="lnkBtnSaveInfo_Click"><span>Save</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
