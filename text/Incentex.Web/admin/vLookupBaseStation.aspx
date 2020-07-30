<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="vLookupBaseStation.aspx.cs" Inherits="admin_vLookupBaseStation" Title="World-Link System - Dropdown Listing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link media="screen" rel="stylesheet" href="../CSS/colorbox.css" />
    <style type="text/css">
        select
        {
            width: 100%;
        }
        .fileinput
        {
            float: left;
            margin-right: 10px;
            width: 145px;
        }
        .fileinput input
        {
            width: 130px;
            padding: 8px 0px;
            color: #fff;
        }
        .custom-checkbox input, .custom-checkbox_checked input
        {
            width: 20px;
            height: 20px;
            margin-left: -8px;
        }
         .grey2_btn
        {
            background: url("../images/bot-grey-btn.png") no-repeat scroll 0 -35px transparent;
            color: #FFFFFF !important;
            display: inline-block;
            font-size: 15px !important;
            margin-right: 33px;
            padding-left: 7px;
            text-decoration: none;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        var formats = 'jpg|gif|png';
        $().ready(function() {

            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtPriorityName: {
                            required: true
                            // remote: "checkexistence.aspx?action=lookupnameexistence&button=<%=btnSubmit.Text%>"
                        },
                        ctl00$ContentPlaceHolder1$flFile:
                        {
                            //required: true,
                            accept: formats
                        }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtPriorityName: {
                            required: replaceMessageString(objValMsg, "Required", "icon name")
                            //remote : "Record already exist."
                        },
                        ctl00$ContentPlaceHolder1$flFile: {
                            //required: "<br/>Please select file to upload.", 
                            accept: replaceMessageString(objValMsg, "ImageType", "jpg,gif,png")
                        }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$flFile")
                            error.insertAfter("#dvIconName");
                        else
                            error.insertAfter(element);
                    }

                });

            });
        });

    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div>
            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="grey2_btn alignright" OnClick="lnkAddNew_Click"><span>+ Add</span></asp:LinkButton>
            <br />
            <br />
            <br />
            <asp:DataList ID="dtLstLookup" runat="server" RepeatDirection="Vertical" RepeatColumns="2"
                OnItemCommand="dtLstLookup_ItemCommand" OnItemDataBound="dtLstLookup_ItemDataBound">
                <ItemTemplate>
                    <table class="form_table">
                        <tr>
                            <td class="formtd">
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box clearfix dropdown_search">
                                        <span class="alignleft status_detail">
                                            <asp:HiddenField ID="hdnLookupIcon" Value='<%# DataBinder.Eval(Container.DataItem, "sBaseStationIcon")%>'
                                                runat="server" />
                                            <img id="imgLookupIcon" runat="server" alt='Loading' />
                                            <asp:Literal ID="txtLookupName" runat="server" Text='<%# Eval("sBaseStation")%>'></asp:Literal>
                                            <%--<asp:TextBox ID="txtLookupName" runat="server" Text='<%# Eval("sLookupName")%>' MaxLength="100" ></asp:TextBox>--%>
                                        </span><span class="alignright">
                                            <asp:LinkButton ID="lnkLookupID" CommandName="deletevalue" CommandArgument='<%# Eval("iBaseStationId")%>'
                                                runat="server" OnClientClick="return confirm('Are you sure you want to delete record?');"><img src="../images/close-btn.png" alt=""  /></asp:LinkButton>
                                        </span><span class="alignright">
                                            <asp:LinkButton ID="lnkEdit" CommandName="editvalue" CommandArgument='<%# Eval("iBaseStationId")%>'
                                                runat="server"><img src="../images/edit-icon.png" alt="" /></asp:LinkButton>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div>
            <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
            
            <at:ModalPopupExtender ID="modal" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
                DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="cboxClose">
            </at:ModalPopupExtender>
            <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
                <div id="cboxWrapper" style="display: block; width: 458px; height: 709px; position: fixed;
                    left: 35%; top: 2%;">
                    <div style="">
                        <div id="cboxTopLeft" style="float: left;">
                        </div>
                        <div id="cboxTopCenter" style="float: left; width: 408px;">
                        </div>
                        <div id="cboxTopRight" style="float: left;">
                        </div>
                    </div>
                    <div style="clear: left;">
                        <div id="cboxMiddleLeft" style="float: left; height: 663px;">
                        </div>
                        <div id="cboxContent" style="float: left; width: 408px; display: block; height: 663px;">
                            <div id="cboxLoadedContent" style="display: block;">
                                <div style="padding: 10px;">
                                    <div class="label_bar">
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                    </div>
                                    <div class="weather_form">
                                        <div class="weatherlabel_pad">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="custom-sel">
                                                    <asp:DropDownList ID="ddlCountry" onchange="pageLoad(this,value);" runat="server">
                                                    </asp:DropDownList>
                                                    <span class="slc"><span class="src">Country Name</span></span></span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                        <div class="weatherlabel_pad">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Icon Name</span>
                                                <asp:TextBox ID="txtPriorityName" CssClass="w_label" runat="server" />
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                        <div class="weatherlabel_filepad">
                                            <div class="fileinput">
                                                <div>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Icon</span>
                                                        <img id="imgEdit" runat="server" alt="load" />
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </div>
                                            </div>
                                            <div class="choose-btn">
                                                <input type="file" id="flFile" class="file" runat="server" />
                                                <input type="button" value="Choose File" class="button" />
                                            </div>
                                            <div style="clear: both;" id="dvIconName">
                                            </div>
                                        </div>
                                        <div class="weatherlabel_pad">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Latitude</span>
                                                <asp:TextBox ID="txtLatitude" CssClass="w_label" runat="server" />
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                        <div class="weatherlabel_pad">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Longitude</span>
                                                <asp:TextBox ID="txtLongitude" CssClass="w_label" runat="server" />
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </div>
                                    <div class="weatherDetails true">
                                        <table>
                                            <tr class="header">
                                                <td>
                                                    <div class="alignleft h-weather">
                                                        Weather</div>
                                                    <div class="alignleft h-link">
                                                        Links</div>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gv" ShowHeader="false" runat="server" AutoGenerateColumns="false"
                                            GridLines="None" CssClass="weather_box">
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblId" runat="server" Text='<%#Eval("iLookupID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <img src="../images/<%#Eval("sLookupIcon")%>" alt="" />
                                                        <span>
                                                            <%#Eval("sLookupName") %>
                                                        </span>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="alignleft d-weather" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <span runat="server" id="dvChk" class="custom-checkbox centeralign">
                                                            <asp:CheckBox ID="chk" runat="server" />
                                                        </span>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="alignright d-link" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="spacer10" style="clear: both;">
                                    </div>
                                    <div class="centeralign">
                                        <asp:Button ID="btnSubmit" Width="80px" runat="server" OnClick="btnSubmit_Click"
                                            Text="" />
                                    </div>
                                </div>
                                <div id="cboxLoadingOverlay" style="height: 663px; display: none;" class="">
                                </div>
                                <div id="cboxLoadingGraphic" style="height: 663px; display: none;" class="">
                                </div>
                                <div id="cboxTitle" style="display: block;" class="">
                                </div>
                                <div id="cboxClose" style="" class="">
                                    close</div>
                            </div>
                        </div>
                        <div id="cboxMiddleRight" style="float: left; height: 663px;">
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
        </div>
    </div>
</asp:Content>
