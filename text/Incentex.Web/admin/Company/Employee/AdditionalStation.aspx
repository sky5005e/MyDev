<%@ Page Title="Company Employee >> Additional Stations Managed" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="AdditionalStation.aspx.cs" Inherits="admin_Company_Employee_AdditionalStation" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link media="screen" rel="stylesheet" href="../../../CSS/colorbox.css" />
    <style type="text/css">
        .headernote
        {
            color: #909090;
            display: block;
            text-align: center;
        }
        .custom-checkbox input, .custom-checkbox_checked input
        {
            width: 20px;
            height: 20px;
            margin-left: -10px;
        }
        .stationinfo .firsttd
        {
            padding-bottom: 15px;
        }
        .stationinfo .label
        {
            color: #F4F4F4 !important;
        }
        .stationinfo .content
        {
            color: #909090;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript" language="javascript">

        $().ready(function() {

            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlBasestation: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlBasestation: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Station") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlBasestation")
                            error.insertAfter("#dvStation");
                        else
                            error.insertAfter(element);
                    }
                });

            });

            $("#ctl00_ContentPlaceHolder1_btnAddStation").click(function() {
                return $("#aspnetForm").valid();
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menucontrol" runat="server" />
    <div class="form_pad">
        <%--start div for adding new station for this user--%>
        <div>
            <span class="headernote">Please Select from the choices below additional stations which
                are managed by this employee.</span>
            <div class="spacer25">
            </div>
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div class="spacer10">
            </div>
            <table class="select_box_pad form_table" style="width: 330px;">
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <label class="dropimg_width">
                                <span class="input_label">Select Station</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlBasestation" onchange="pageLoad(this,value);" runat="server">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvStation">
                                </div>
                            </label>
                            <div id="dvStation">
                            </div>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
            </table>
            <div class="botbtn centeralign">
                <asp:LinkButton CssClass="grey2_btn" ID="btnAddStation" runat="server" OnClick="btnAddStation_Click"><span>+ Add Station</span></asp:LinkButton>
            </div>
        </div>
        <%--end div for adding new station for this user--%>
        <div class="spacer25">
        </div>
        <asp:UpdatePanel ID="upPanel" runat="server">
            <ContentTemplate>
                <div class="shipping_method_pad">
                    <h4>
                        Station Info:</h4>
                    <div>
                        <div class="black_top_co">
                            <span>&nbsp;</span></div>
                        <div class="black_middle">
                            <table class="stationinfo" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="firsttd">
                                        <span class="label">Station Manager</span> :
                                        <asp:Label CssClass="content" runat="server" ID="lblStationManager"></asp:Label>
                                    </td>
                                    <td class="firsttd">
                                        <span class="label">Employees</span> :
                                        <asp:Label CssClass="content" runat="server" ID="lblTotalEmployee"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="label">Number of Workgroups</span> :
                                        <asp:Label CssClass="content" runat="server" ID="lblTotalWorkgroup"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="black_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </div>
                <%--start listing of station for this user with pagging--%>
                <div class="shipping_method_pad">
                    <h4>
                        Station Listings:</h4>
                    <div style="text-align: center">
                        <asp:Label ID="lblmsgList" runat="server" CssClass="errormessage"></asp:Label>
                    </div>
                    <asp:GridView ID="dtlUserStations" runat="server" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                        OnRowDataBound="dtlUserStations_RowDataBound" OnRowCommand="dtlUserStations_RowCommand">
                        <Columns>
                            <asp:TemplateField SortExpression="Tit">
                                <HeaderTemplate>
                                    <span>
                                        <asp:LinkButton ID="lnkbtnHeaderStation" runat="server" CommandArgument="Station"
                                            CommandName="Sort">Station Listings</asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderStation" runat="server"></asp:PlaceHolder>
                                    </span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="first">
                                        <asp:HiddenField runat="server" ID="hdnCompanyContactInfoID" Value='<%# Eval("CompanyContactInfoID") %>' />
                                        <%# Eval("Title") %>
                                    </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="40%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Station Address</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnStationAddress" runat="server" CommandArgument='<%# Eval("CompanyContactInfoID") %>'
                                        CommandName="StationAddress">
                                        <span class="btn_space">
                                                 <img alt="Station Address" src="~/admin/Incentex_Used_Icons/ic-address.png" width="24" height="24" runat="server" /></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemStyle CssClass="b_box centeralign" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Additional Workgroup</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnWorkgroup" runat="server" CommandArgument='<%# Eval("CompanyContactInfoID") %>'
                                        CommandName="Workgroup">
                                        <span class="btn_space">
                                                 <img alt="Additional Workgroup" src="~/admin/Incentex_Used_Icons/ic-additionaworkgroup.png" width="24" height="24" runat="server" /></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemStyle CssClass="g_box centeralign" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Link</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnIsAdditionalStationActive" Value='<%# Eval("IsAdditionalStationActive") %>' />
                                    <asp:LinkButton ID="lnkbtnLink" ToolTip="Click to change status." runat="server"
                                        CommandArgument='<%# Eval("CompanyContactInfoID") %>' CommandName="Link">
                                        <span class="btn_space">
                                            <img id="imbLink" src="~/Images/grd_active.png" width="24" height="24" runat="server" /></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemStyle CssClass="g_box centeralign" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Delete</span>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtndelete" runat="server" CommandArgument='<%# Eval("CompanyContactInfoID") %>'
                                        CommandName="DeleteStation" OnClientClick="return confirm('Are you sure, you want to delete selected item?');">
                                        <span class="btn_space">
                                            <img id="imgDelete" alt="Delete" src="~/Images/close.png" width="24" height="24"
                                                runat="server" /></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box centeralign" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    </td></tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td colspan="4">
                                            <asp:GridView ID="dtlUserWorkgroup" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                                CssClass="orderreturn_box" OnRowCommand="dtlUserWorkgroup_RowCommand" GridLines="None"
                                                RowStyle-CssClass="ord_content">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Workgroup </span>
                                                            <div class="corner">
                                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span class="first">
                                                                <%# Eval("WorkgroupName") %></span>
                                                            <div class="corner">
                                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                            </div>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box" Width="70%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Employees </span>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span>
                                                                <%# Eval("TotalEmployee") %></span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box" Width="30%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Delete</span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtndelete" runat="server" CommandArgument='<%# Eval("StationWorkgroupID") %>'
                                                                CommandName="DeleteWorkgroup" OnClientClick="return confirm('Are you sure, you want to delete selected item?');">
                                                                <span class="btn_space">
                                                                    <img id="imgDelete" alt="Delete" src="~/Images/close.png" width="24" height="24"
                                                                        runat="server" /></span>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="10%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div>
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
                </div>
                <%--end listing of station for this user with pagging--%>
                <div>
                    <asp:LinkButton ID="lnkDummyEditAddress" class="grey2_btn alignright" runat="server"
                        Style="display: none"></asp:LinkButton>
                    <at:ModalPopupExtender ID="modalShippingAddress" TargetControlID="lnkDummyEditAddress"
                        BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlShippingAddress"
                        CancelControlID="cboxClose">
                    </at:ModalPopupExtender>
                    <%--Start oppup panel for shipping detail and workgrop detail--%>
                    <asp:Panel ID="pnlShippingAddress" runat="server" Style="display: none;">
                        <div id="cboxWrapper" style="display: block; width: 758px; height: 409px; position: fixed;
                            left: 35%; top: 30%;">
                            <div style="">
                                <div id="cboxTopLeft" style="float: left;">
                                </div>
                                <div id="cboxTopCenter" style="float: left; width: 708px;">
                                </div>
                                <div id="cboxTopRight" style="float: left;">
                                </div>
                            </div>
                            <div style="clear: left;">
                                <div id="cboxMiddleLeft" style="float: left; height: 363px;">
                                </div>
                                <div id="cboxContent" style="float: left; width: 708px; display: block; height: 363px;">
                                    <div id="cboxLoadedContent" style="display: block;">
                                        <div style="padding: 10px;">
                                            <div style="text-align: center;">
                                                <asp:Label ID="lblPopupMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                            </div>
                                            <div class="weather_form" style="width: auto; margin: 0 auto;">
                                                <%--start table for shipping detail--%>
                                                <table class="form_table" runat="server" id="tblAddress">
                                                    <tr>
                                                        <td class="formtd">
                                                            <div class="weatherlabel_pad">
                                                                <div class="form_top_co">
                                                                    <span>&nbsp;</span></div>
                                                                <div class="form_box">
                                                                    <span class="input_label">Company</span>
                                                                    <asp:TextBox ID="TxtShippingCompany" runat="server" CssClass="w_label" TabIndex="1"></asp:TextBox>
                                                                </div>
                                                                <div class="form_bot_co">
                                                                    <span>&nbsp;</span></div>
                                                            </div>
                                                            <div class="weatherlabel_pad">
                                                                <div class="form_top_co">
                                                                    <span>&nbsp;</span></div>
                                                                <div class="form_box">
                                                                    <span class="input_label">Last Name</span>
                                                                    <asp:TextBox ID="TxtShipingLastName" runat="server" CssClass="w_label" TabIndex="3"></asp:TextBox>
                                                                </div>
                                                                <div class="form_bot_co">
                                                                    <span>&nbsp;</span></div>
                                                            </div>
                                                            <div class="weatherlabel_pad">
                                                                <div>
                                                                    <div class="form_top_co">
                                                                        <span>&nbsp;</span></div>
                                                                    <div class="form_box">
                                                                        <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                                            <asp:DropDownList ID="DrpShipingCoutry" runat="server" CssClass="w_label" OnSelectedIndexChanged="DrpShipingCoutry_SelectedIndexChanged"
                                                                                AutoPostBack="true" TabIndex="5">
                                                                            </asp:DropDownList>
                                                                        </span>
                                                                    </div>
                                                                    <div class="form_bot_co">
                                                                        <span>&nbsp;</span></div>
                                                                </div>
                                                            </div>
                                                            <div class="weatherlabel_pad">
                                                                <div>
                                                                    <div class="form_top_co">
                                                                        <span>&nbsp;</span></div>
                                                                    <div class="form_box">
                                                                        <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                                            <asp:DropDownList ID="DrpShippingCity" OnSelectedIndexChanged="DrpShippingCity_SelectedIndexChanged"
                                                                                AutoPostBack="true" runat="server" TabIndex="7">
                                                                            </asp:DropDownList>
                                                                        </span>
                                                                    </div>
                                                                    <div class="form_bot_co">
                                                                        <span>&nbsp;</span></div>
                                                                </div>
                                                            </div>
                                                            <div class="weatherlabel_pad">
                                                                <div class="form_top_co">
                                                                    <span>&nbsp;</span></div>
                                                                <div class="form_box">
                                                                    <span class="input_label">Address Name</span>
                                                                    <asp:TextBox ID="TxtShipingTitle" runat="server" CssClass="w_label" TabIndex="9"></asp:TextBox>
                                                                </div>
                                                                <div class="form_bot_co">
                                                                    <span>&nbsp;</span></div>
                                                            </div>
                                                        </td>
                                                        <td class="formtd">
                                                            <div class="weatherlabel_pad">
                                                                <div class="form_top_co">
                                                                    <span>&nbsp;</span></div>
                                                                <div class="form_box">
                                                                    <span class="input_label">First Name</span>
                                                                    <asp:TextBox ID="TxtShippingFirstName" runat="server" CssClass="w_label" TabIndex="2"></asp:TextBox>
                                                                </div>
                                                                <div class="form_bot_co">
                                                                    <span>&nbsp;</span></div>
                                                            </div>
                                                            <div class="weatherlabel_pad">
                                                                <div class="form_top_co">
                                                                    <span>&nbsp;</span></div>
                                                                <div class="form_box">
                                                                    <span class="input_label">Address</span>
                                                                    <asp:TextBox ID="TxtShipingAddress" runat="server" CssClass="w_label" TabIndex="4"></asp:TextBox>
                                                                </div>
                                                                <div class="form_bot_co">
                                                                    <span>&nbsp;</span></div>
                                                            </div>
                                                            <div class="weatherlabel_pad">
                                                                <div>
                                                                    <div class="form_top_co">
                                                                        <span>&nbsp;</span></div>
                                                                    <div class="form_box">
                                                                        <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                                                            <asp:DropDownList ID="DrpShipingState" runat="server" OnSelectedIndexChanged="DrpShipingState_SelectedIndexChanged"
                                                                                AutoPostBack="true" TabIndex="6">
                                                                            </asp:DropDownList>
                                                                        </span>
                                                                    </div>
                                                                    <div class="form_bot_co">
                                                                        <span>&nbsp;</span></div>
                                                                </div>
                                                            </div>
                                                            <div class="weatherlabel_pad" id="PnlCityOther" runat="server" visible="false">
                                                                <div>
                                                                    <div class="form_top_co">
                                                                        <span>&nbsp;</span></div>
                                                                    <div class="form_box">
                                                                        <span class="input_label">City Name</span>
                                                                        <asp:TextBox ID="txtCity" runat="server" CssClass="w_label" TabIndex="8"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form_bot_co">
                                                                        <span>&nbsp;</span></div>
                                                                </div>
                                                            </div>
                                                            <div class="weatherlabel_pad">
                                                                <div class="form_top_co">
                                                                    <span>&nbsp;</span></div>
                                                                <div class="form_box">
                                                                    <span class="input_label">Zip</span>
                                                                    <asp:TextBox ID="TxtShippingZip" runat="server" CssClass="w_label" TabIndex="8"></asp:TextBox>
                                                                </div>
                                                                <div class="form_bot_co">
                                                                    <span>&nbsp;</span></div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%--end table for shipping detail--%>
                                                <%--start table for workgrop detail--%>
                                                <table runat="server" id="tblWorkgroup" style="width: 300px; margin: 30px auto 30px;"
                                                    visible="false">
                                                    <tr>
                                                        <td>
                                                            <div class="weatherlabel_pad">
                                                                <div>
                                                                    <div class="form_top_co">
                                                                        <span>&nbsp;</span></div>
                                                                    <div class="form_box">
                                                                        <span class="input_label">Workgroup</span> <span class="custom-sel label-sel-small">
                                                                            <asp:DropDownList ID="ddlWorkgroup" runat="server" CssClass="w_label" onchange="pageLoad(this,value);">
                                                                            </asp:DropDownList>
                                                                        </span>
                                                                    </div>
                                                                    <div class="form_bot_co">
                                                                        <span>&nbsp;</span></div>
                                                                </div>
                                                            </div>
                                                            <div class="weatherlabel_pad">
                                                                <div class="form_top_co">
                                                                    <span>&nbsp;</span></div>
                                                                <div class="form_box">
                                                                    <span class="input_label">Employee</span>
                                                                    <asp:TextBox ID="txtTotalEmployee" runat="server" CssClass="w_label"></asp:TextBox>
                                                                </div>
                                                                <div class="form_bot_co">
                                                                    <span>&nbsp;</span></div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%--end table for workgroup detail--%>
                                            </div>
                                            <div class="centeralign">
                                                <asp:LinkButton ID="lnkbtnEditShippingAddress" CssClass="grey2_btn" runat="server"
                                                    OnClick="lnkbtnEditShippingAddress_Click"><span>Update Station</span></asp:LinkButton>
                                                <asp:LinkButton ID="lnkbtnAddWorkgroup" CssClass="grey2_btn" runat="server" OnClick="lnkbtnAddWorkgroup_Click"
                                                    Visible="false"><span>Add Workgroup</span></asp:LinkButton>
                                            </div>
                                        </div>
                                        <div id="cboxLoadingOverlay" style="height: 363px; display: none;" class="">
                                        </div>
                                        <div id="cboxLoadingGraphic" style="height: 363px; display: none;" class="">
                                        </div>
                                        <div id="cboxTitle" style="display: block;" class="">
                                        </div>
                                        <div id="cboxClose" style="" class="">
                                            close</div>
                                    </div>
                                </div>
                                <div id="cboxMiddleRight" style="float: left; height: 363px;">
                                </div>
                            </div>
                            <div style="clear: left;">
                                <div id="cboxBottomLeft" style="float: left;">
                                </div>
                                <div id="cboxBottomCenter" style="float: left; width: 708px;">
                                </div>
                                <div id="cboxBottomRight" style="float: left;">
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <%--End oppup panel for shipping detail and workgrop detail--%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
