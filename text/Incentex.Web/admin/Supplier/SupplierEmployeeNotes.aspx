<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="SupplierEmployeeNotes.aspx.cs" Inherits="admin_Supplier_SupplierEmployeeNotes" Title="Supplier Employee Notes" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--Added manually to increase text area size--%>
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

    <script type="text/javascript" language="javascript">
        /*Function to get X and Y co-ordinates of a browser*/
        $().ready(function() {

            posY = getScreenCenterY();
            posX = getScreenCenterX();

            $("#ctl00_ContentPlaceHolder1_hfY").val(posY);
            $("#ctl00_ContentPlaceHolder1_hfX").val(posX);

            function getScreenCenterY() {
                var y = 0;
                y = getScrollOffset() + (getInnerHeight() / 2);
                return (y);
            }

            function getScreenCenterX() {
                return (document.body.clientWidth / 2);
            }

            function getInnerHeight() {
                var y;
                if (self.innerHeight) // all except Explorer
                {
                    y = self.innerHeight;
                }
                else if (document.documentElement &&
            document.documentElement.clientHeight)
                // Explorer 6 Strict Mode
                {
                    y = document.documentElement.clientHeight;
                }
                else if (document.body) // other Explorers
                {
                    y = document.body.clientHeight;
                }
                return (y);
            }

            function getScrollOffset() {
                var y;
                if (self.pageYOffset) // all except Explorer
                {
                    y = self.pageYOffset;
                }
                else if (document.documentElement &&
            document.documentElement.scrollTop)
                // Explorer 6 Strict
                {
                    y = document.documentElement.scrollTop;
                }
                else if (document.body) // all other Explorers
                {
                    y = document.body.scrollTop;
                }
                return (y);
            }


            $(function() {
                scrolltextarea(".scrollme1", "#Scrolltop1", "#ScrollBottom1");
            });
        });
        /*End*/
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menucontrol" runat="server" />
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
 <%--       <div>
            <div class="form_top_co">
                <span>&nbsp;</span></div>
            <div class="form_box taxt_area clearfix">
                <span class="input_label alignleft">Notes/History</span>
                <div class="textarea_box alignright">
                    <div class="scrollbar" style="height: 182px">
                        <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                            class="scrollbottom"></a>
                    </div>
                    <asp:TextBox ID="txtNoteHistory" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                        ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="form_bot_co">
                <span>&nbsp;</span></div>
        </div>--%>
         <div class="form_table">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box taxt_area clearfix" style="height: 182px">
                            <span class="input_label alignleft" style="height: 180px;vertical-align:middle">Notes/History : </span>
                            <div class="textarea_box alignright">
                                <div class="scrollbar" style="height: 181px">
                                    <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                    </a>
                                </div>
                                <asp:TextBox ID="txtNoteHistory" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                    ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                    <div class="alignnone spacer15">
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
        <asp:Panel ID="pnlNotes" runat="server" style="display:none;">
            <div class="pp_pic_holder facebook" style="display: block; width: 411px;">
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
                                                    <span>Add Notes / Hisory
                                                        <br />
                                                        <br />
                                                        <span>
                                                            <asp:TextBox Height="120" Width="350" TextMode="MultiLine" ID="txtNote" runat="server"></asp:TextBox></span>
                                                </div>
                                                <div>
                                                    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="grey2_btn" OnClick="btnSubmit_Click">
								                                      <span>Save Notes</span>
                                                    </asp:LinkButton>
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
        <div class="alignnone spacer25">
        </div>
        <div class="botbtn centeralign">
            <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" TabIndex="19" OnClick="lnkSave_Click">
								        <span>Save Information</span>
            </asp:LinkButton>
        </div>
    </div>
    <input type="hidden" id="hfX" value="" runat="server" />
    <input type="hidden" id="hfY" value="" runat="server" />
</asp:Content>

