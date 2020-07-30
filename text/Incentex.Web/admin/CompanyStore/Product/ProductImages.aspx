<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ProductImages.aspx.cs" Inherits="admin_ProductManagement_ProductImages"
    Title="Product >> Images" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" src="../../../JS/jquery.prettyPhoto.js" type="text/javascript"></script>
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdateProgress runat="server" ID="uprogressPGrid" DisplayAfter="1">
        <ProgressTemplate>
            <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
            </div>
            <div class="updateProgressDiv">
                    <img alt="Loading" src="../../../Images/ajax-loader-large.gif"/>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <mb:MenuUserControl ID="menuControl" runat="server" />

    <script type="text/javascript" language="javascript">
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected image?") == true)
                return true;
            else
                return false;
        }

        var formats = 'jpg|gif|png|swf';
        $().ready(function() {
            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {


                    ctl00$ContentPlaceHolder1$ddlMasterItemNo: { NotequalTo: "0" },
                    ctl00$ContentPlaceHolder1$fpUpload: { required: true, accept: formats },
                    ctl00$ContentPlaceHolder1$fpUploadLargerImage : {required: true, accept: formats}

                    },
                    messages: {


                        ctl00$ContentPlaceHolder1$ddlMasterItemNo: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "master item number")
                        },
                          ctl00$ContentPlaceHolder1$fpUpload: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." },
                          ctl00$ContentPlaceHolder1$fpUploadLargerImage : { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." }

                    }

                });
                $("#<%=lnkBtnUploadWorkgroup.ClientID %>").click(function() {

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

    <div class="front_page_pad">
        <div>
            <table class="dropdown_pad form_table">
                <tr>
                    <td colspan="2" class="formtd">
                        <div style="text-align: center; color: Red; font-size: small;">
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
            <table class="dropdown_pad form_table">
                <tr>
                    <td runat="server" id="StyleNo" class="formtd">
                        <div>
                            <asp:UpdatePanel ID="upnlItemStyle" runat="server">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Style</span> <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlItemStyle" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);">
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
                            <asp:UpdatePanel ID="upnlMasteritemNo" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lnkBtnUploadWorkgroup" />
                                    <asp:PostBackTrigger ControlID="ddlMasterItemNo" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Master Item Number</span> <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlMasterItemNo" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);"
                                                OnSelectedIndexChanged="ddlMasterItemNo_SelectedIndexChanged">
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
                    <td class="spacer10">
                    </td>
                </tr>
                <tr id="tdUpload" runat="server">
                    <td>
                        <asp:UpdatePanel ID="upUpload" runat="server">
                            <ContentTemplate>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <input id="fpUpload" type="file" runat="server" />
                                        <br />
                                        <br />
                                        <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                                            <img src="../../../Images/lightbulb.gif" alt="note:" />&nbsp;Supported file formats
                                            are <b>.jpg|.gif|.png|.swf</b></div>
                                        <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                                            <img src="../../../Images/lightbulb.gif" alt="note:" />&nbsp;Supported file resolution
                                            is <b>145 X 198(Width X Height)</b></div>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr id="trLargerImage" runat="server">
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <input id="fpUploadLargerImage" type="file" runat="server" />
                                        <br />
                                        <br />
                                        <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                                            <img src="../../../Images/lightbulb.gif" alt="note:" />&nbsp;Supported file formats
                                            are <b>.jpg|.gif|.png|.swf</b></div>
                                        <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                                            <img src="../../../Images/lightbulb.gif" alt="note:" />&nbsp;Supported file resolution
                                            is <b>600 X 400(Width X Height)</b></div>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkBtnUploadWorkgroup" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkBtnUploadWorkgroup" class="grey2_btn" runat="server" ToolTip="Upload Image"
                                    OnClick="lnkBtnUploadWorkgroup_Click"><span>Upload</span></asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <div class="alignnone">
            &nbsp;</div>
        <asp:UpdatePanel ID="upImages" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="dtProductImages" />
            </Triggers>
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td width="10%">
                        </td>
                        <td width="88%">
                            <div class="front_page_pad">
                                <div class="splash_img_pad">
                                    <asp:DataList ID="dtProductImages" runat="server" RepeatDirection="Vertical" RepeatColumns="4"
                                        RepeatLayout="Flow" OnItemDataBound="dtProductImages_ItemDataBound" OnItemCommand="dtProductImages_ItemCommand">
                                        <ItemTemplate>
                                            <div class="alignleft item">
                                                <p class="upload_photo gallery">
                                                    <%--  <span class="lt_co"></span><span class="rt_co"></span><span class="lb_co"></span>
                            <span class="rb_co"></span>--%><a id="prettyphotoDiv" rel='prettyPhoto[a]' runat="server">
                                                            <img id="imgSplashImage" runat="server" alt='Loading' />
                                                           </a>
                                                </p>
                                                <div>
                                                    <asp:RadioButton runat="server" AutoPostBack="true" ID="rbImages" Text="Select as Product Image"
                                                        OnCheckedChanged="rbImages_CheckedChanged" />
                                                    <asp:HiddenField ID="hdnImageId" runat="server" Value='<%# Eval("StoreProductImageId") %>' />
                                                    <asp:HiddenField ID="hdnProductImageActive" runat="server" Value='<%# Eval("ProductImageActive") %>' />
                                                </div>
                                                <div>
                                                    <asp:LinkButton ID="lnkBtnDeleteDoc" CommandArgument='<%# Eval("StoreProductImageId") %>'
                                                        CommandName="DeleteSplashImage" class="greyicon_btn btn" runat="server" ToolTip="Upload Image"
                                                        OnClientClick="return DeleteConfirmation();"><span class="btn_width180">Delete</span></asp:LinkButton>
                                                    <asp:HiddenField ID="hdndocumentname" runat="server" Value='<%# Eval("ProductImage") %>' />
                                                    <asp:HiddenField ID="hdnlargerimagename" runat="server" Value='<%# Eval("LargerProductImage") %>' />
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <div class="alignnone">
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td width="2%">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
