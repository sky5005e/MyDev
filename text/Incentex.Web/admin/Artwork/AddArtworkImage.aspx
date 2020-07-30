<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="AddArtworkImage.aspx.cs" Inherits="admin_Artwork_AddArtworkImage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
    $().ready(function()
    {
     $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlStoreStatus: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$fpUpload: { required: true, accept: formats }
                        
                        
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "customer") },
                        ctl00$ContentPlaceHolder1$ddlStoreStatus: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "file category") },
                        ctl00$ContentPlaceHolder1$fpUpload: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCompany")
                            error.insertAfter("#dvCompany");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlStoreStatus")
                            error.insertAfter("#dvFC");
                       else if (element.attr("name") == "ctl00$ContentPlaceHolder1$fpUpload")
                            error.insertAfter("#dvthumb");
                       else if (element.attr("name") == "ctl00$ContentPlaceHolder1$fpUploadLargeImage")
                            error.insertAfter("#dvLarge");
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

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad select_box_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="form_table">
            <table>
                <tr>
                    <td>
                        <h4 style="margin-bottom: 0px;">
                            Associate with customer
                        </h4>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <asp:UpdatePanel ID="upCompany" runat="server">
                                <ContentTemplate>
                                    <div class="form_box">
                                        <span class="custom-sel">
                                            <asp:DropDownList ID="ddlCompany" onchange="pageLoad(this,value);" runat="server"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                        <div id="dvCompany">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <h4 style="margin-bottom: 0px;">
                            Associate with file category
                        </h4>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <asp:UpdatePanel ID="upStoreStatus" runat="server">
                                <ContentTemplate>
                                    <div class="form_box">
                                        <span class="custom-sel ">
                                            <asp:DropDownList ID="ddlStoreStatus" onchange="pageLoad(this,value);" runat="server"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                        <div id="dvFC">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <h4 style="margin-bottom: 0px;">
                            Attach thumbnail image 
                        </h4>
                    </td>
                </tr>
                <tr id="tdUpload" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <input id="fpUpload" type="file" runat="server" />
                                <br />
                                <br />
                                <div class="noteIncentexthumb" style="width: 100%; font-size: 12px;">
                                    <img src="../../Images/lightbulb.gif" style="z-index: -1" alt="note:" />&nbsp;Supported
                                    file formats are
                                    <br />
                                    <b>.jpg | .pdf | .gif | .eps | .ai | .DST and .EMB</b></div>
                                    <br />
                                <div class="noteIncentexThumb1" style="width: 100%; font-size: 12px;">
                                    <img src="../../Images/lightbulb.gif" alt="note:" />&nbsp;Supported file resolution
                                    is <br /> <b>145 X 198(Width X Height)</b></div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                        <div id="dvthumb">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <h4 style="margin-bottom: 0px;">
                            Attach full size image
                        </h4>
                    </td>
                </tr>
                <tr id="tdUpload1" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <input id="fpUploadLargeImage" type="file" runat="server" />
                                <br />
                                <br />
                                <div class="noteIncentexlarge" style="width: 100%; font-size: 12px;">
                                    <img src="../../Images/lightbulb.gif" style="z-index: -1" alt="note:" />&nbsp;Supported
                                    file formats are
                                    <b>.jpg | .gif </b></div>
                                <br />
                                <div class="noteIncentexlarge1" style="width: 100%; font-size: 12px;">
                                    <img src="../../Images/lightbulb.gif" alt="note:" />&nbsp;Supported file resolution
                                    is <br /><b>400 X 600(Width X Height)</b></div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                        <div id="dvLarge">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Upload files"
                            OnClick="lnkBtnSaveInfo_Click"><span>Upload Files</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
