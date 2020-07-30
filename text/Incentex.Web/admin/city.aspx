<%@ Page Language="C#" Title="World-Link System - Manage City" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="city.aspx.cs" Inherits="admin_city" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        var formats = 'jpg|gif|png';
        $().ready(function() {

            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtCity: {
                            required: true
                            // remote: "checkexistence.aspx?action=lookupnameexistence&button=<%=btnSubmit.Text%>"
                        }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtCity: {
                            required: "<br/>" + replaceMessageString(objValMsg, "Required", "city")
                            //remote : "Record already exist."
                        }
                    }

                });
            
        });
    });
    
    function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }     
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }   
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdatePanel ID="upPanel" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubmit" />
            <asp:PostBackTrigger ControlID="lnkAddNew" />
        </Triggers>
        <ContentTemplate>
            <div class="form_pad">
                <div class="centeralign">
                    <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
                <div class="not_yet_pad">
                    <table>
                        <tr>
                            <td class="formtd_member">
                                <table class="form_table">
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:UpdatePanel ID="upnlCountry" runat="server">
                                                    <ContentTemplate>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="input_label">Country</span> <span class="custom-sel label-sel">
                                                                <asp:DropDownList ID="ddlCountry" runat="server" onchange="pageLoad(this,value);"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
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
                                                <asp:UpdatePanel ID="upnlSate" runat="server">
                                                    <ContentTemplate>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="input_label">State</span> <span class="custom-sel label-sel">
                                                                <asp:DropDownList ID="ddlState" runat="server" onchange="pageLoad(this,value);" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                                    <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </span>
                                                            <div>
                                                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upnlCountry">
                                                                    <ProgressTemplate>
                                                                        <img src="Images/ajaxbtn.gif" class="progress_img" /></ProgressTemplate>
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
                                    <tr visible="false" id="trCity" runat="server">
                                        <td>
                                            <div>
                                                <asp:UpdatePanel ID="upnlCity" runat="server">
                                                    <ContentTemplate>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="input_label">City</span> <span class="custom-sel label-sel">
                                                                <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="true">
                                                                    <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </span>
                                                            <div>
                                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upnlSate">
                                                                    <ProgressTemplate>
                                                                        <img src="Images/ajaxbtn.gif" class="progress_img" /></ProgressTemplate>
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
                                            <asp:GridView ID="grvCity" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="grvCity_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCityId" runat="server" Text='<%# Eval("iCityID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnCityName" runat="server" CommandArgument="sCityName" CommandName="Sort"><span>City Name</span></asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCityName" runat="server" Text='<%# Eval("sCityName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                      <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnEdit" runat="server" CommandArgument="EditCity" CommandName="Sort"><span>Edit</span></asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span>
                                                                <asp:LinkButton ID="lnkbtnedit" CommandName="editCity" CommandArgument='<%# Eval("iCityID") %>' runat="server">
                                                                    <img id="edit" src="~/Images/edit-icon.png" height="25" width="25" runat="server" /></asp:LinkButton></span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnDelete" runat="server" CommandArgument="Delete" CommandName="Sort"><span>Delete</span></asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span>
                                                                <asp:LinkButton ID="lnkbtndelete" CommandName="delCity" OnClientClick="return DeleteConfirmation();"
                                                                    CommandArgument='<%# Eval("iCityID") %>' runat="server">
                                                                    <img id="delete" src="~/Images/close.png" runat="server" /></asp:LinkButton></span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <div>
                                                <div>
                                                    <div class="alignright pagging" id="dvPaging" runat="server">
                                                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                                                        </asp:LinkButton>
                                                        <span>
                                                            <asp:DataList ID="dtlViewEmployee" runat="server" CellPadding="1" CellSpacing="1"
                                                                OnItemCommand="dtlViewEmployee_ItemCommand" OnItemDataBound="dtlViewEmployee_ItemDataBound"
                                                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                        </span>
                                                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                        <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="grey2_btn alignright" OnClick="lnkAddNew_Click"><span>+ Add</span></asp:LinkButton>
                        <at:ModalPopupExtender ID="modal" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
                            DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="closepopup">
                        </at:ModalPopupExtender>
                        <asp:Panel ID="pnlPriority" runat="server" >
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
                                                                <div class="label_bar">
                                                                    <label>
                                                                        City :</label>
                                                                    <span>
                                                                        <asp:TextBox class="popup_input" ID="txtCity" runat="server"></asp:TextBox></span>
                                                                </div>
                                                                <div class="label_bar btn_padinn">
                                                                    <asp:Button ID="btnSubmit" Text="Add City" runat="server" OnClick="btnSubmit_Click" />
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
                    <input type="hidden" id="hfCityId" value="" runat="server" />
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
