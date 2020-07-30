<%@ Page Title="Company Details" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="MainCompanyPartners.aspx.cs" Inherits="admin_ArtWorkDecoratingLibrary_MainCompanyPartners" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calendar-small.png',
                buttonImageOnly: true
            });
        });
    </script>

    <script type="text/javascript">
        $(window).load(function() {
            $("#dvLoader").hide();
        });
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleContainerContent").hide();

        });   
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div class="form_table" style="width: 920px;">
            <div>
                <%--First Div--%>
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
                <div class="black_middle order_detail_pad">
                    <table>
                        <tr>
                            <td style="width: 50%">
                                <table>
                                    <tr>
                                        <td style="font-size: small;">
                                            <label>
                                                Company Name :
                                            </label>
                                            <asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small;">
                                            <label>
                                                Owners Name :
                                            </label>
                                            <asp:TextBox ID="txtOwnerName" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small;">
                                            <label>
                                                Address 1 :
                                            </label>
                                            <asp:TextBox ID="txtAddress1" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small;">
                                            <label>
                                                Address 2 :
                                            </label>
                                            <asp:TextBox ID="txtAddress2" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small;">
                                            <label>
                                                City :
                                            </label>
                                            <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small;">
                                            <label>
                                                State :
                                            </label>
                                            <asp:TextBox ID="txtState" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small;">
                                            <label>
                                                Zip :
                                            </label>
                                            <asp:TextBox ID="txtZip" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small;">
                                            <label>
                                                Telephone :
                                            </label>
                                            <asp:TextBox ID="txtTelephone" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small;">
                                            <label>
                                                Fax :
                                            </label>
                                            <asp:TextBox ID="txtFax" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 50%">
                                <table class="checktable_supplier true">
                                    <tr>
                                        <td style="font-size: small;">
                                            Checkmark Below Services
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DataList ID="dtDecoratingServices" runat="server">
                                                <ItemTemplate>
                                                    <span class="custom-checkbox alignleft" id="DcrtServicespan" runat="server">
                                                        <asp:CheckBox ID="chkDcrtServiceName" runat="server" /></span>
                                                    <label style="font-size: small;">
                                                        <asp:Label ID="lblDcrtServiceName" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                                    <asp:HiddenField ID="hdnDcrtServiceID" runat="server" Value='<%#Eval("iLookupID")%>' />
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                                <div class="centeralign">
                                <asp:LinkButton ID="lnkBtnSaveServices"  runat="server" CssClass="grey2_btn alignright" OnClick="lnkBtnSaveServices_Click"><span>Save</span></asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
            </div>
        </div>
        <div class="spacer15">
        </div>
        <div class="collapsibleContainer" title="Company Contacts :" align="left">
            <%--  <div class="form_table">
                <asp:GridView ID="gvEmpDetails" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" AllowSorting="true">
                    <Columns>
                        <asp:TemplateField Visible="false" HeaderText="Id">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblID" Text='<%# Eval("UserInfoID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Check">
                            <HeaderTemplate>
                                <span>&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this);"
                                    runat="server" /></span>
                                <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="first">&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkDelete" runat="server" />
                                </span>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Middle" CssClass="b_box" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkFirstName" runat="server" CommandArgument="FirstName" CommandName="Sorting"><span >Name</span></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span>
                                    <asp:HyperLink ID="lnkEditSupp" runat="server" NavigateUrl='<%# "~/admin/Supplier/MainCompanyContact.aspx?Id=" + Eval("SupplierID").ToString()%>'
                                        Text='<%#Eval("FirstName") + " " + Eval("LastName") %>'>  </asp:HyperLink>
                                </span>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkSupportingRole" runat="server" CommandArgument="SupportingRole"
                                    CommandName="Sorting"><span>Supporting Role</span></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSupportingRole" Text='<%# Eval("SupportingRole")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkEmailAddress" runat="server" CommandArgument="EmailAddress"
                                    CommandName="Sorting"><span>EmailAddress</span></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblEmailAddress" Text='<%# Eval("EmailAddress")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkStatus" runat="server" CommandArgument="Status" CommandName="Sorting"><span class="white_co">Status</span></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("Status")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>--%>
        </div>
        <div class="spacer15">
        </div>
        <div class="collapsibleContainer" title="Company Notes :" align="left">
            <div class="form_table">
                <div>
                    <div class="form_table">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box taxt_area clearfix" style="height: 100px">
                            <span class="input_label alignleft" style="height: 100px">Notes</span>
                            <div class="textarea_box alignright">
                                <div class="scrollbar" style="height: 103px">
                                    <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                    </a>
                                </div>
                                <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                    Height="100px" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                    <div class="alignnone spacer15">
                    </div>
                    <div class="rightalign gallery" id="div1" runat="server">
                        <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                        <asp:LinkButton ID="lnkAddNew" CommandName="AddNotes" runat="server" CssClass="grey2_btn alignright"><span>+Add Notes</span></asp:LinkButton>
                        <at:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkAddNew" BackgroundCssClass="modalBackground"
                            DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="A5">
                        </at:ModalPopupExtender>
                    </div>
                    <div>
                        <asp:Panel ID="pnlNotes" runat="server" Style="display: none;">
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
                                                    <div id="Div2">
                                                        <div class="pp_inline clearfix">
                                                            <div class="form_popup_box" style="padding-top: 15px;">
                                                                <div class="label_bar">
                                                                    <span>Notes :
                                                                        <br />
                                                                        <br />
                                                                        <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtCompanyNote"
                                                                            runat="server"></asp:TextBox></span>
                                                                </div>
                                                                <div>
                                                                    <asp:LinkButton ID="lnkNoteHis" runat="server" class="grey2_btn" OnClientClick="return CheckNoteHistory()"
                                                                        OnClick="lnkNoteHis_Click"><span>Save Notes</span></asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="pp_details clearfix" style="width: 371px;">
                                                        <a href="#" id="A5" runat="server" class="pp_close">Close</a>
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
                    <br />
                    <br />
                    <br />
                    <%--End Note History--%>
                </div>
            </div>
        </div>
        <div class="spacer15">
        </div>
        <div class="collapsibleContainer" title="Pricing Agreements :" align="left">
            <div class="form_table">
                <asp:GridView ID="GridViewPricing" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" AllowSorting="true">
                    <Columns>
                        <asp:TemplateField Visible="false" HeaderText="Id">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblID" Text='<%# Eval("DecoratingPartnersID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Check">
                            <HeaderTemplate>
                                <span>&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this);"
                                    runat="server" /></span>
                                <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="first">&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkDelete" runat="server" />
                                </span>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Middle" CssClass="b_box" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span >Customer</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span>
                                    <asp:HyperLink ID="lnkEditSupp" runat="server" 
                                        Text='<%#Eval("CustomerName") %>'>  </asp:HyperLink>
                                </span>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Date & Time</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblDateTime" Text='<%# Eval("CreatedDate")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Good Until</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblGoodUntil" Text='<%# Eval("Good_Until")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span class="white_co">File</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblFile" Text='<%# Eval("File")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="rightalign gallery" id="div3" runat="server">
                <asp:LinkButton ID="lnkDummyAddPricing" class="grey2_btn alignright" runat="server"
                    Style="display: none"></asp:LinkButton>
                <asp:LinkButton ID="lnkAddPricing" CommandName="AddPricing" runat="server" CssClass="grey2_btn alignright"><span>+Add price list</span></asp:LinkButton>
                <at:ModalPopupExtender ID="modalAddPricing" TargetControlID="lnkAddPricing" BackgroundCssClass="modalBackground"
                    DropShadow="true" runat="server" PopupControlID="pnlPricing" CancelControlID="cls">
                </at:ModalPopupExtender>
            </div>
            <div>
                <asp:Panel ID="pnlPricing" runat="server" Style="display: none;">
                    <div class="pp_pic_holder facebook" style="display: block; width: 411px;">
                        <div class="pp_top" style="">
                            <div class="pp_left">
                            </div>
                            <div class="pp_middle">
                            </div>
                            <div class="pp_right">
                            </div>
                        </div>
                        <div class="pp_content_container">
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
                                            <div id="Div4">
                                                <div class="pp_inline clearfix">
                                                    <div class="form_popup_box" style="padding-top: 15px;">
                                                        <div class="centeralign">
                                                            <table>
                                                                <tr class="label_bar" style="height:25px;">
                                                                    <td>
                                                                        <span>Customer Name:</span>
                                                                    </td>
                                                                    <td>
                                                                        <span>
                                                                            <asp:TextBox class="popup_input" ID="txtCustomerName" runat="server"></asp:TextBox></span>
                                                                    </td>
                                                                </tr>
                                                                <tr class="label_bar" style="height:25px;">
                                                                    <td>
                                                                        <span>Good Until : </span>
                                                                    </td>
                                                                    <td>
                                                                        <span>
                                                                            <asp:TextBox class="popup_input" ID="txtGoodUntil" runat="server"></asp:TextBox></span>
                                                                    </td>
                                                                </tr>
                                                                <tr class="label_bar" style="height:25px;">
                                                                    <td>
                                                                        <span>Date:</span>
                                                                    </td>
                                                                    <td>
                                                                        <span>
                                                                            <asp:TextBox ID="txtDate" Style="border: medium none; width: 80px; padding: 2px"
                                                                                runat="server" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                                                                        </span>
                                                                    </td>
                                                                </tr>
                                                                <tr class="label_bar" style="height:25px;">
                                                                    <td>
                                                                        <span>File :</span>
                                                                    </td>
                                                                    <td>
                                                                        <span>
                                                                            <asp:FileUpload ID="fileUp" runat="server" /></span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <div>
                                                                <asp:LinkButton ID="lnkSavePricing" runat="server" CommandName="SAVEPricing" class="grey2_btn"
                                                                    OnClick="lnkSavePricing_Click"><span>Save </span></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="pp_details clearfix" style="width: 371px;">
                                                <a href="#" id="cls" runat="server" class="pp_close">Close</a>
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
        </div>
    </div>
</asp:Content>
