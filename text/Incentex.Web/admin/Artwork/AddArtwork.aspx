<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="AddArtwork.aspx.cs" Inherits="admin_Artwork_AddArtwork" %>

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
        var formats = 'jpg|gif|pdf|eps|ai|DST|EMB';
        var largerformats = 'jpg|gif';
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlArtworkFor: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtArtworkName: { required: true },
                        ctl00$ContentPlaceHolder1$txtSpecialNotes: { required: true }

                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Company") },
                        ctl00$ContentPlaceHolder1$ddlArtworkFor: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Artwork For") },
                        ctl00$ContentPlaceHolder1$txtArtworkName: { required: replaceMessageString(objValMsg, "Required", "Artwork Name") },
                        ctl00$ContentPlaceHolder1$txtSpecialNotes: { required: replaceMessageString(objValMsg, "Required", "Special Notes") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCompany")
                            error.insertAfter("#dvCompany");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlArtworkFor")
                            error.insertAfter("#dvArtworkFor");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtArtworkName")
                            error.insertAfter("#dvtxtArtworkName");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtSpecialNotes")
                            error.insertAfter("#dvtxtSpecialNotes");
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
                                    <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1">
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
                                <span class="input_label" style="width: 30%;">Artwork Name </span>
                                <asp:TextBox ID="txtArtworkName" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                                <div id="dvtxtArtworkName">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trArtworkFor" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Artwork For</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlArtworkFor" runat="server" TabIndex="3" AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvArtworkFor">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trArtworkDesignNumber" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%;">Artwork Design Number </span>
                                <asp:Label ID="lblArtworkDesignNumber" TabIndex="4" runat="server" CssClass="w_label"></asp:Label>
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
                </tr>
                <tr id="tdUpload" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%;">Upload Files </span>
                                <input type="file" id="fileupload" style="margin: -24px 0px 8px 150px" runat="server"
                                    tabindex="6" class="multi max-5 accept-EPS|AI|EMB|DST|jpg|pdf|png" />
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
