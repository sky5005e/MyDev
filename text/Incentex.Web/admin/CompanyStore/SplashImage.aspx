<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="SplashImage.aspx.cs" Inherits="admin_CompanyStore_SplashImage" Title="Splash Image" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" src="../../JS/jquery.prettyPhoto.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();

        }
        
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected image?") == true)
                return true;
            else
                return false;
        }

        var formats = 'jpg|gif|png|swf';

        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    //                    onsubmit: false,
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$fpUpload: { required: true, accept: formats },
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "workgroup") },
                        ctl00$ContentPlaceHolder1$fpUpload: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." },
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlDepartment")
                            error.insertAfter("#dvDepartment");
                        else
                            error.insertAfter(element);
                    }

                });


            });

            $("#<%=lnkBtnUploadWorkgroup.ClientID %>").click(function() {
                return $('#aspnetForm').valid();

            });
        });
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div>
        <table class="select_box_pad form_table" style="width: 420px;">
            <tr>
                <td class="spacer10">
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label">Department :</span>
                            <label class="dropimg_width230">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlDepartment" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvDepartment">
                                </div>
                            </label>
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
                            <span class="input_label">Workgroup :</span>
                            <label class="dropimg_width230">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlWorkgroup" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvWorkgroup">
                                </div>
                            </label>
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
                        <div class="form_box centeralign">
                            <asp:LinkButton ID="lnkBtnGetReocords" class="grey2_btn" runat="server" ToolTip="Get Splash Image by Workgroup"
                                OnClick="lnkBtnGetReocords_Click"><span>Get Splash Images</span></asp:LinkButton>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </td>
            </tr>
            <tr id="trNote" runat="server" visible="false">
                <td>
                    <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                        <img src="../../Images/lightbulb.gif" alt="note:" />&nbsp;You can upload 5 images
                        per workgroup.Please delete at least one image below to upload new.</div>
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
                            <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                                <img src="../../Images/lightbulb.gif" style="z-index: -1" alt="note:" />&nbsp;Supported
                                file formats are <b>.jpg|.gif|.png|.swf</b></div>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="centeralign">
                    <asp:LinkButton ID="lnkBtnUploadWorkgroup" class="grey2_btn" runat="server" ToolTip="Upload Image"
                        OnClick="lnkBtnUploadWorkgroup_Click"><span>Upload</span></asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <div class="alignnone">
        &nbsp;</div>
    <div class="front_page_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="splash_img_pad">
            <asp:DataList ID="dtSplash" runat="server" RepeatDirection="Vertical" RepeatColumns="5"
                RepeatLayout="Flow" OnItemDataBound="dtSplash_ItemDataBound" OnItemCommand="dtSplash_ItemCommand">
                <ItemTemplate>
                    <div class="alignleft item">
                        <p class="upload_photo gallery">
                            <a id="prettyphotoDiv" rel="prettyPhoto[p]" runat="server">
                                <img id="imgSplashImage" height="182" width="172" runat="server" alt='Loading' />
                            </a>
                        </p>
                        <div>
                            <asp:LinkButton ID="lnkBtnDeleteDoc" CommandArgument='<%# Eval("StoreDocumentID") %>'
                                CommandName="DeleteSplashImage" class="greyicon_btn btn" runat="server" ToolTip="Upload Image"
                                OnClientClick="return DeleteConfirmation();"><span class="btn_width180">Delete</span></asp:LinkButton>
                            <asp:HiddenField ID="hdndocumentname" runat="server" Value='<%# Eval("DocumentName") %>' />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <div class="alignnone">
            </div>
        </div>
    </div>
</asp:Content>
