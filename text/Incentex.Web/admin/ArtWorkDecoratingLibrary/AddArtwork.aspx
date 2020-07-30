<%@ Page Title="Add Artwork" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="AddArtwork.aspx.cs" Inherits="admin_ArtWorkDecoratingLibrary_AddArtwork" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
    </script>

    <script type="text/javascript" language="javascript">
        var formats = 'jpg|gif|pdf|eps|ai|DST|EMB';
        var largerformats = 'jpg|gif';
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtArtworkName: { required: true },
                        ctl00$ContentPlaceHolder1$txtSAPReferenceTag: { required: true },
                        ctl00$ContentPlaceHolder1$ddlArtworkCreatedBy: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlGarmentSizeApply: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$fileuploadJPG: { required: false, accept: 'jpg|JPEG' },
                        ctl00$ContentPlaceHolder1$fileuploadPDF: { required: false, accept: 'pdf' },
                        ctl00$ContentPlaceHolder1$fileuploadEMB: { required: false, accept: 'EMB' },
                        ctl00$ContentPlaceHolder1$fileuploadDST: { required: false, accept: 'DST' },
                        ctl00$ContentPlaceHolder1$fileuploadEPS: { required: false, accept: 'EPS' }
                        //ctl00$ContentPlaceHolder1$txtSpecialNotes: { required: true }


                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Company") },
                        ctl00$ContentPlaceHolder1$txtArtworkName: { required: replaceMessageString(objValMsg, "Required", "Artwork Name") },
                        ctl00$ContentPlaceHolder1$txtSAPReferenceTag: { required: replaceMessageString(objValMsg, "Required", "SAP Reference Tag") },
                        ctl00$ContentPlaceHolder1$ddlArtworkCreatedBy: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Artwork Created By") },
                        ctl00$ContentPlaceHolder1$ddlGarmentSizeApply: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Garment Size") },
                        ctl00$ContentPlaceHolder1$fileuploadJPG: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." },
                        ctl00$ContentPlaceHolder1$fileuploadPDF: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." },
                        ctl00$ContentPlaceHolder1$fileuploadEMB: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." },
                        ctl00$ContentPlaceHolder1$fileuploadDST: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." },
                        ctl00$ContentPlaceHolder1$fileuploadEPS: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." }

                        // ctl00$ContentPlaceHolder1$txtSpecialNotes: { required: replaceMessageString(objValMsg, "Required", "Special Notes") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCompany")
                            error.insertAfter("#dvCompany");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtArtworkName")
                            error.insertAfter("#dvtxtArtworkName");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlArtworkCreatedBy")
                            error.insertAfter("#dvArtworkCreatedBy");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlGarmentSizeApply")
                            error.insertAfter("#dvGarmentSizeApply");
                        else
                            error.insertAfter(element);
                    }

                });
            });

            $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                return $('#aspnetForm').valid();

            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad select_box_pad" style="width: 400px;">
        <div class="form_table">
            <div class="spacer10">
            </div>
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            <div class="spacer10">
            </div>
            <table>
                <tr id="trCompany" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Company</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvCompany">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trArtworkName" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Artwork File Name </span>
                                <asp:TextBox ID="txtArtworkName" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                                <div id="dvtxtArtworkName">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trArtworkNumber" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Artwork Number </span>
                                <asp:Label ID="lblArtworkNumber" TabIndex="3" runat="server" CssClass="w_label"></asp:Label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trSAPReferenceTag" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">SAP Reference Tag </span>
                                <asp:TextBox ID="txtSAPReferenceTag" TabIndex="4" runat="server" CssClass="w_label"></asp:TextBox>
                                <div id="dvSAPReferenceTag">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trArtworkCreatedBy" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Artwork Created By</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlArtworkCreatedBy" runat="server" TabIndex="5" onchange="pageLoad(this,value);" >
                                    </asp:DropDownList>
                                </span>
                                <div id="dvArtworkCreatedBy">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trGarmentSizeApply" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Garment Size Apply</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlGarmentSizeApply" runat="server" TabIndex="6" onchange="pageLoad(this,value);" >
                                    </asp:DropDownList>
                                </span>
                                <div id="dvGarmentSizeApply">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <%--   <tr >
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box taxt_area clearfix">
                                <span class="input_label alignleft" style="width: 32%!important;">Special Notes</span>
                                <div class="textarea_box alignright" style="width: 65%;">
                                    <div class="scrollbar">
                                        <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                            class="scrollbottom"></a>
                                    </div>
                                    <asp:TextBox ID="txtSpecialNotes" runat="server" TabIndex="5" TextMode="MultiLine"
                                        CssClass="scrollme1" Height="70px">
                                    </asp:TextBox>
                                </div>
                                <div id="dvtxtSpecialNotes">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>--%>
                <tr id="trUploadJPG" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%;">Upload Files .JPG</span>
                                <input type="file" id="fileuploadJPG" style="margin: -24px 0px 8px 150px" runat="server"
                                    tabindex="7" />
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trUploadPDF" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%;">Upload Files .PDF</span>
                                <input type="file" id="fileuploadPDF" style="margin: -24px 0px 8px 150px" runat="server"
                                    tabindex="7" />
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trUploadEMB" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%;">Upload Files .EMB</span>
                                <input type="file" id="fileuploadEMB" style="margin: -24px 0px 8px 150px" runat="server"
                                    tabindex="7" />
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trUploadDST" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%;">Upload Files .DST</span>
                                <input type="file" id="fileuploadDST" style="margin: -24px 0px 8px 150px" runat="server"
                                    tabindex="7" />
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trUploadEPS" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%;">Upload Files .EPS</span>
                                <input type="file" id="fileuploadEPS" style="margin: -24px 0px 8px 150px" runat="server"
                                    tabindex="7" />
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Search files"
                            OnClick="lnkBtnSaveInfo_Click"><span>Save</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
