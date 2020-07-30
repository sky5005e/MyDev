<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewUploadDocument.aspx.cs" Inherits="admin_Company_ViewUploadDocument"
    Title="World-Link System - ViewUploadDocument" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="manuControl" runat="server" />

    <script type="text/javascript" language="javascript">
        // var formats = 'jpg|gif|png';
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {

                        ctl00$ContentPlaceHolder1$flFile:
                     {
                         required: true
                         //accept: formats
                     }
                    },
                    messages: {

                        ctl00$ContentPlaceHolder1$flFile: {
                            required: "<br/>Please select file to upload."
                            // accept: "<br/>" + replaceMessageString(objValMsg, "ImageType", "jpg,gif,png")
                        }
                    }

                });

            });
        });
        
    </script>

    <script type="text/javascript" language="javascript">
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }
    </script>

    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div>
            <table class="form_table">
                <tr>
                    <td class="formtd_right">
                        <table>
                            <tr>
                                <td class="upload_pad_station">
                                    <div class="alignleft item">
                                        <div>
                                            <asp:LinkButton ID="lnkPGUpload" class="greyicon_btn btn" runat="server" ToolTip="Save Information"
                                                OnClick="lnkPGUpload_Click">
                                                <span runat="server" class="btn_width250" id="aUpload">Program Agreement<img src="../../Images/program-agrrement-icon.png"
                                                    alt="" /></span></asp:LinkButton></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td class="upload_pad_station">
                                    <div class="alignleft item">
                                        <div>
                                            <asp:LinkButton ID="lnkGMUpload" class="greyicon_btn btn" runat="server" ToolTip="Upload File"
                                                OnClick="lnkGMUpload_Click">
                                                <span runat="server" class="btn_width250" id="Span1">Guidelines Manual<img src="../../images/upload-guideness-icon.png"
                                                    alt="" /></span></asp:LinkButton>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd_right">
                        <table>
                            <tr>
                                <td class="formtd_right">
                                    <table>
                                        <tr>
                                            <td class="upload_pad_station">
                                                <div class="alignleft item">
                                                    <div>
                                                        <asp:LinkButton ID="lnkNewFiles" class="greyicon_btn btn" runat="server" ToolTip="Upload File"
                                                            OnClick="lnkNewFiles_Click">
                                                            <span runat="server" class="btn_width250" id="Span2">News Files<img src="../../images/upload-supplier-icon.png"
                                                                alt="" /></span></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="upload_pad_station">
                        <div class="alignleft item">
                            <div>
                                <asp:LinkButton ID="lnkNDAAgreement" class="greyicon_btn btn" runat="server" ToolTip="Upload File"
                                    OnClick="lnkNDAAgreement_Click">
                                    <span runat="server" class="btn_width250" id="Span3">NDA Agreement<img src="../../images/program-agrrement-icon.png"
                                        alt="" /></span></asp:LinkButton>
                            </div>
                        </div>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td class="upload_pad_station">
                                    <div class="alignleft item">
                                        <div>
                                            <asp:LinkButton ID="lnkOtherDocument" class="greyicon_btn btn" runat="server" ToolTip="Upload File"
                                                OnClick="lnkOtherDocument_Click">
                                                <span runat="server" class="btn_width250" id="Span4">Other Documents<img src="../../images/upload-other-icon.png"
                                                    alt="" /></span></asp:LinkButton>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" align="center">
            <tr>
                <td width="20%">
                </td>
                <td width="60%" align="center">
                    <div>
                        <asp:UpdatePanel runat="server" ID="upnlGrdCompany">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="gvComDocumnet" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="header_bg">
                                </div>
                                <div class="alignnone">
                                    &nbsp;</div>
                                <div class="form_pad">
                                    <div class="user_manage_btn btn_width_small">
                                        <asp:GridView ID="gvComDocumnet" runat="server" RowStyle-CssClass="ord_content" AutoGenerateColumns="False"
                                            HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                             OnRowCommand="gvComDocumnet_RowCommand" OnRowDataBound="gvComDocumnet_RowDataBound"
                                            HorizontalAlign="Center">
                                            <RowStyle CssClass="ord_content" />
                                            <Columns>
                                                <asp:TemplateField Visible="False" HeaderText="Id">
                                                    <HeaderTemplate>
                                                        Document ID
                                                         <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                      <div class="corner">
                                                        <asp:Label runat="server" ID="lblDocumentID" Text='<%# Eval("DocumentId") %>' />
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" CssClass="g_box" />
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="sLookupName">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkbtnDocName" runat="server" CommandArgument="sLookupName" CommandName="Sort"><span >Document Name</span></asp:LinkButton>
                                                        <asp:PlaceHolder ID="placeholderDocName" runat="server"></asp:PlaceHolder>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblDocumentName"  Text='<% # (Convert.ToString(Eval("sLookupName")).Length > 35) ? Eval("sLookupName").ToString().Substring(0,35)+"..." : Convert.ToString(Eval("sLookupName"))+ "&nbsp;"  %>' ToolTip='<% #Eval("sLookupName")  %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="g_box" Width="23%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="FileName">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkbtnFileName" runat="server" CommandArgument="FileName" CommandName="Sort"><span >File Name</span></asp:LinkButton>
                                                        <asp:PlaceHolder ID="placeholderFileName" runat="server"></asp:PlaceHolder>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span>
                                                            <asp:LinkButton runat="server" ID="lnkFileName" CommandName="view" CommandArgument='<%# Eval("FileName") %>'
                                                                Text='<%# Eval("FileName") %>'></asp:LinkButton></span>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="b_box" Width="17%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                       <asp:LinkButton ID="lnkbtnDelete" runat="server" CommandArgument="Delete" CommandName="Sort"><span>Delete</span></asp:LinkButton> 
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                         <span><asp:LinkButton ID="lnkbtndelete" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                                            CommandArgument='<%# Eval("DocumentId") %>' runat="server">
                                                           
                                                                <img id="delete" src="~/Images/close.png" runat="server" /></span></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="ord_header" />
                                        </asp:GridView>
                                    </div>
                                    <div>
                                        <div class="companylist_botbtn alignleft">
                                        </div>
                                        <div id="pagingtable" runat="server" class="alignright pagging">
                                            <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                                            </asp:LinkButton>
                                            <span>
                                                <asp:DataList ID="DataList2" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="DataList2_ItemCommand"
                                                    OnItemDataBound="DataList2_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                            CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:DataList></span>
                                            <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td width="20%">
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
        <at:ModalPopupExtender ID="modal" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
            DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="closepopup">
        </at:ModalPopupExtender>
        <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
            <div class="pp_pic_holder facebook" style="display: block; width: 411px; position:fixed;left:35%;top:30%;">
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
                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                </div>
                                                <div class="label_bar" id="icondiv" runat="server" visible="false">
                                                    <label>
                                                        Icon :</label>
                                                    <img id="imgEdit" runat="server" alt="load" />
                                                </div>
                                                <div class="label_bar">
                                                    <label>
                                                        Upload File :</label>
                                                    <span>
                                                        <input type="file" id="flFile" runat="server" /></span>
                                                </div>
                                                <div class="label_bar btn_padinn">
                                                    <asp:Button ID="btnSubmit" Text="Upload" runat="server" OnClick="btnSubmit_Click" />
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
    </div>
    <asp:HiddenField ID="hdnPorgramAgrement" runat="server" />
    <asp:HiddenField ID="hdnGuidelinesManuel" runat="server" />
    <asp:HiddenField ID="hdnNewsFiles" runat="server" />
    <asp:HiddenField ID="hdnNDAAgreement" runat="server" />
    <asp:HiddenField ID="hdnOtherDocuments" runat="server" />
</asp:Content>
