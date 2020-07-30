<%@ Page Title="Document Storage Center" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="ListDocumentStoregeCenter.aspx.cs" Inherits="admin_DocumentStoregeCentre_ListDocumentStoregeCenter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link media="screen" rel="stylesheet" href="../../CSS/colorbox.css" />
    <style type="text/css">
        .custom-checkbox input, .custom-checkbox_checked input
        {
            width: 20px;
            height: 20px;
            margin-left: -8px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function() {
            var winH = $(window).height();
            var winW = $(window).width();
            $('#cboxWrapper').css({
                'position': 'fixed',
                'top': parseInt((winH / 2) - (330 / 2), 10),
                'left': parseInt((winW / 2) - (458 / 2), 10)
            });

            $('#cboxpasswordWrapper').css({
                'position': 'fixed',
                'top': parseInt((winH / 2) - (270 / 2), 10),
                'left': parseInt((winW / 2) - (458 / 2), 10)
            });
        });


        function setpassword(password) {
            var pass = password;
            $("#<%= password.ClientID %>").val(pass);
            $("#ctl00_ContentPlaceHolder1_lblError").text("");
            $("#ctl00_ContentPlaceHolder1_txtpassword").val("");
        }

        function checkpassword() {
            var pass = $("#ctl00_ContentPlaceHolder1_txtpassword").val();
            var hdnpass = $('#<%= password.ClientID %>').val();

            if (pass == hdnpass) {
                return true;
            }
            else {
                $("#ctl00_ContentPlaceHolder1_lblError").text("Password is incorrect.");
                return false;
            }
        }

        function Loadloder() {
            if (confirm('Are you sure, you want to delete selected item?')) {
                $('#dvLoader').show();
                return true;
            }
            else {
                return false;
            }
        }
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
    <div class="form_pad" id="dvList" runat="server">
        <div style="text-align: center; color: Red; font-size: larger;">
            <asp:Label ID="lblmsg" runat="server">
            </asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="grdView_RowDataBound"
            OnRowCommand="grdView_RowCommand">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblArtworkID" Text='<%# Eval("DocumentStorageID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="FileName">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkFileName" runat="server" CommandArgument="FileName" CommandName="Sort"><span>File Name</span></asp:LinkButton>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="first">
                            <asp:Label runat="server" ID="lblFileName" Text='<%# Eval("FileName") %> ' />
                        </div>
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="UplodedDate">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkUplodedDate" runat="server" CommandArgument="UplodedDate"
                            CommandName="Sort"><span>Date Uploaded</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblUplodedDate" Text='<%# Eval("UplodedDate") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="UplodedBy">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkUplodedBy" runat="server" CommandArgument="UplodedBy" CommandName="Sort"><span>Uploaded By</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblUplodedBy" Text='<%# Eval("UplodedBy") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="FileSize">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkFileSize" runat="server" CommandArgument="FileSize" CommandName="Sort"><span>File Size</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblFileSize" Text='<%# Eval("FileSize") + "&nbsp;" %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="LastViewBy">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkLastViewBy" runat="server" CommandArgument="LastViewBy" CommandName="Sort"><span>Last View By</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblLastViewBy" Text='<%# Eval("LastViewBy") + "&nbsp;" %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span class="white_co">Email</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:LinkButton ID="lnkSendEmail" runat="server" CommandArgument='<%# Eval("DocumentStorageID") %>'
                                CssClass="btn_space" ToolTip="Send an  E-mail" CommandName="SendEmail"><img height="24" width="24" src="../../Images/shipment06.png" alt="X" /></asp:LinkButton>
                        </span>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box centeralign" Width="3%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span class="white_co">View File</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:HiddenField ID="hdnfilename" runat="server" Value='<%# Eval("FileName") %>' />
                            <asp:HiddenField ID="hdnOriginalFileName" runat="server" Value='<%# Eval("OriginalFileName") %>' />
                            <asp:HiddenField ID="hdnextension" runat="server" Value='<%# Eval("extension") %>' />
                            <asp:LinkButton ID="lnkviewdocument" runat="server" CommandArgument='<%# Eval("DocumentStorageID") %>'
                                CssClass="btn_space" ToolTip="view file" CommandName="ViewDocument">
                                <img alt="X" id="viewimg" runat="server" />
                            </asp:LinkButton>&nbsp; </span>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box centeralign" />
                    <HeaderStyle CssClass="centeralign" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>Delete</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:LinkButton ID="lnkbtndelete" CommandName="DeleteDocument" OnClientClick="return Loadloder();"
                                CommandArgument='<%# Eval("DocumentStorageID") %>' runat="server" CssClass="btn_space">
                                <img alt="X" id="delete" src="~/Images/close.png" runat="server" /></asp:LinkButton>
                        </span>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box centeralign" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div id="pagingtable" runat="server" class="alignright pagging">
            <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
            </asp:LinkButton>
            <span>
                <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                    RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                            CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:DataList></span>
            <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
        </div>
    </div>
    <div class="form_pad" id="dvfolder" runat="server">
        <asp:HiddenField ID="password" runat="server" />
        <div class="spacer10">
        </div>
        <div>
            <asp:DataList ID="dtDocumentFolder" runat="server" RepeatDirection="Horizontal" RepeatColumns="3"
                OnItemCommand="dtDocumentFolder_ItemCommand" Width="100%">
                <ItemTemplate>
                    <div style="padding-bottom: 65px; width: 100%">
                        <asp:LinkButton ID="lnkfolder" CommandName="SearchDocument" CommandArgument='<%# Eval("iLookupID")%>'
                            runat="server" OnClientClick='<%# "setpassword(\"" + Eval("Val1") + "\");" %>'>
                            <img style="float: left;" id="imgBtnPageURL" runat="server" src="~/admin/Incentex_Used_Icons/Incentex-DocumentStoregeFolder.png" />
                            <span style="color: White; text-decoration: none; margin: 14px 0px 0px 5px; float: left;
                                width: 200px;">
                                <%# Eval("sLookupName")%></span>
                            <asp:HiddenField ID="hdfpassword" runat="server" Value='<%# Eval("Val1") %>' />
                        </asp:LinkButton>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </div>
    <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
    <at:ModalPopupExtender ID="modal" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
        DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="cboxClose">
    </at:ModalPopupExtender>
    <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
        <div id="cboxWrapper" style="display: block; width: 458px; height: 330px;">
            <div style="">
                <div id="cboxTopLeft" style="float: left;">
                </div>
                <div id="cboxTopCenter" style="float: left; width: 408px;">
                </div>
                <div id="cboxTopRight" style="float: left;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxMiddleLeft" style="float: left; height: 268px;">
                </div>
                <div id="cboxContent" style="float: left; width: 408px; display: block; height: 268px;">
                    <div id="cboxLoadedContent" style="display: block;">
                        <div style="padding: 10px;">
                            <div class="weatherDetails true" style="height: auto;">
                                <asp:HiddenField ID="hdnOrderID" runat="server" />
                            </div>
                            <div class="spacer10" style="clear: both;">
                            </div>
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <div class="noteIncentexThumb1" style="width: 100%; font-size: 12px;">
                                        <img src="../../Images/lightbulb.gif" alt="note:" />&nbsp; Press "Enter" to Insert
                                        more email address.with comma..
                                        <br />
                                        <br />
                                        <b>For Example:</b>
                                        <br />
                                        knelson@incentex.com,ken@incentex.com
                                    </div>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                            <div class="form_table" style="margin-top: 15px;">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box employeeedit_text clearfix">
                                    <span class="input_label alignleft">Email To</span>
                                    <div class="textarea_box alignright" style="height: 70px;">
                                        <div class="scrollbar" style="height: 77px;">
                                            <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                            </a>
                                        </div>
                                        <textarea id="txtEmailTo" style="height: 72px;" rows="3" runat="server" class="scrollme2"
                                            tabindex="1"></textarea>
                                    </div>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                            <div class="centeralign" style="margin-top: 15px;">
                                <asp:LinkButton ID="btnSubmit" CssClass="grey2_btn" runat="server" OnClick="btnSubmit_Click">
                                <span>Send Mail</span>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div id="cboxClose" style="" class="">
                            close</div>
                    </div>
                </div>
                <div id="cboxMiddleRight" style="float: left; height: 268px;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxBottomLeft" style="float: left;">
                </div>
                <div id="cboxBottomCenter" style="float: left; width: 408px;">
                </div>
                <div id="cboxBottomRight" style="float: left;">
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lnkDummyAddNewPass" class="grey2_btn alignright" runat="server"
        Style="display: none"></asp:LinkButton>
    <at:ModalPopupExtender ID="passwordmodal" TargetControlID="lnkDummyAddNewPass" BackgroundCssClass="modalBackground"
        DropShadow="true" runat="server" PopupControlID="pnlPassword" CancelControlID="cboxpasswordClose">
    </at:ModalPopupExtender>
    <asp:Panel ID="pnlPassword" runat="server" Style="display: none;">
        <div id="cboxpasswordWrapper" style="display: block; width: 458px; height: 270px;">
            <div style="">
                <div id="cboxTopLeft" style="float: left;">
                </div>
                <div id="cboxTopCenter" style="float: left; width: 408px;">
                </div>
                <div id="cboxTopRight" style="float: left;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxMiddleLeft" style="float: left; height: 208px;">
                </div>
                <div id="cboxContent" style="float: left; width: 408px; display: block; height: 208px;">
                    <div id="cboxLoadedContent" style="display: block;">
                        <div style="padding: 10px;">
                            <div class="weatherDetails true" style="height: auto;">
                                <asp:Label ID="lblError" runat="server" CssClass="errormessage"></asp:Label>
                            </div>
                            <div class="spacer10" style="clear: both;">
                            </div>
                            <div class="form_table" style="margin-top: 15px;">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 21%">Password</span>
                                    <asp:TextBox ID="txtpassword" TextMode="Password" runat="server" CssClass="w_label"></asp:TextBox>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                            <div class="centeralign" style="margin-top: 15px;">
                                <asp:LinkButton ID="btnSubmitpassword" CssClass="grey2_btn" runat="server" OnClientClick="return checkpassword();"
                                    OnClick="btnSubmitpass_Click">
                                <span>Submit</span>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div id="cboxpasswordClose" style="" class="">
                            close</div>
                    </div>
                </div>
                <div id="cboxMiddleRight" style="float: left; height: 208px;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxBottomLeft" style="float: left;">
                </div>
                <div id="cboxBottomCenter" style="float: left; width: 408px;">
                </div>
                <div id="cboxBottomRight" style="float: left;">
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
