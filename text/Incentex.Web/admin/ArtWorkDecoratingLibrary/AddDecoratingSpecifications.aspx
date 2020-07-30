<%@ Page Title="Decorating Specifications" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="AddDecoratingSpecifications.aspx.cs" Inherits="admin_ArtWorkDecoratingLibrary_AddDecoratingSpecifications" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
        function changeAll(chk) {
            var parent = getParentByTagName(chk, "table");
            var checkBoxes = parent.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++)
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].id.indexOf("chkSelect") >= 0)
                checkBoxes[i].checked = chk.checked;
        }
        function getParentByTagName(obj, tag) {
            var obj_parent = obj.parentNode;
            if (!obj_parent) return false;
            if (obj_parent.tagName.toLowerCase() == tag) return obj_parent;
            else return getParentByTagName(obj_parent, tag);
        }
        function showNestedGridView(obj) {
            var nestedGridView = document.getElementById(obj);
            if (nestedGridView.style.display == "none") {
                nestedGridView.style.display = "inline";

            } else {
                nestedGridView.style.display = "none";

            }
        }
        function SetItemGridddlValue(value) {
            $("#<%=hdnddlChanges.ClientID %>").val(value);
        }
					
    </script>

    <script type="text/javascript" language="javascript">
        var formats = 'jpg|jpeg|eps|ai|DST|EMB';
        var largerformats = 'jpg|gif';
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlProductType: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlDecoratingMethod: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlMasterItemNumber: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtJobName: { required: true },
                        ctl00$ContentPlaceHolder1$fileFinalProof: { required: false, accept: 'doc|txt|pdf' },
                        ctl00$ContentPlaceHolder1$fileProductImage: { required: false, accept: formats }

                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Company") },
                        ctl00$ContentPlaceHolder1$ddlProductType: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Product type") },
                        ctl00$ContentPlaceHolder1$ddlDecoratingMethod: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Decorating Method") },
                        ctl00$ContentPlaceHolder1$ddlMasterItemNumber: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Master Item Number") },
                        ctl00$ContentPlaceHolder1$txtJobName: { required: replaceMessageString(objValMsg, "Required", "Job Name") },
                        ctl00$ContentPlaceHolder1$fileFinalProof: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." },
                        ctl00$ContentPlaceHolder1$fileProductImage: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCompany")
                            error.insertAfter("#dvCompany");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlProductType")
                            error.insertAfter("#dvProductType");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlDecoratingMethod")
                            error.insertAfter("#dvDecoratingMethod");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlMasterItemNumber")
                            error.insertAfter("#dvMasterItemNumber");
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div class="form_table">
            <div class="spacer10">
            </div>
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            <div class="spacer10">
            </div>
            <table class="product_detail" cellpadding="0" cellspacing="0">
                <tr style="height: 40px;">
                    <td style="width: 25%;">
                        <div>
                            <h4>
                                <span class="white_header" style="float: none">Master Item</span></h4>
                        </div>
                        <div class="spacer10">
                        </div>
                        <div class="alignleft">
                            <div class="agent_img">
                                <div class="alignleft item">
                                    <p class="upload_photo gallery">
                                        <a id="prettyphotoDiv" rel='prettyPhoto[a]' href="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                            runat="server">
                                            <img id="imgmasteritem" runat="server" src="~/UploadedImages/ProductImages/ProductDefault.jpg" />
                                        </a>
                                        <asp:HiddenField ID="hdnimagestatus" runat="server" Value='<%# Eval("ProductImageActive") %>' />
                                        <asp:HiddenField ID="hdndocumentname" runat="server" Value='<%# Eval("ProductImage") %>' />
                                        <asp:HiddenField ID="hdnlargerimagename" runat="server" Value='<%# Eval("LargerProductImage") %>' />
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div style="text-align: center;">
                            <asp:Label Style="line-height: 12px; width: 100%; height: auto;" ID="lblItemDescriptions"
                                runat="server"></asp:Label>
                        </div>
                    </td>
                    <td style="width: 50%;">
                        <table>
                            <tr id="trCompany" runat="server">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 30%;">Select Company</span> <span class="custom-sel label-sel">
                                                <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvCompany">
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trProductType" runat="server">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 30%;">Select Product Type</span> <span class="custom-sel label-sel">
                                                <asp:DropDownList ID="ddlProductType" runat="server" TabIndex="5" onchange="pageLoad(this,value);">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvProductType">
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trArtworkName" runat="server">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 30%;">Decorating Job Name </span>
                                            <asp:TextBox ID="txtJobName" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                                            <div id="dvJobName">
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trDecoratingMethod" runat="server">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 30%;">Decorating Method</span> <span class="custom-sel label-sel">
                                                <asp:DropDownList ID="ddlDecoratingMethod" runat="server" TabIndex="6" onchange="pageLoad(this,value);">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvDecoratingMethod">
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trMasterItemNumber" runat="server">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 30%;">Select Master Item Number</span> <span
                                                class="custom-sel label-sel">
                                                <asp:DropDownList ID="ddlMasterItemNumber" runat="server" TabIndex="6" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlMasterItemNumber_SelectedIndexchanged">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvMasterItemNumber">
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trDecoratingReferenceTag" runat="server">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 30%;">Decorating Reference Tag </span>
                                            <asp:Label ID="lblDecoratingReferenceTag" TabIndex="4" runat="server"></asp:Label>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trImprintLocations" runat="server">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 40%;">Number of Imprint Locations</span>
                                            <span class="custom-sel label-sel" style="width: 54%;">
                                                <asp:DropDownList ID="ddlImprintLocations" AutoPostBack="true" OnSelectedIndexChanged="ddlImprintLocations_SelectedIndexchanged"
                                                    runat="server">
                                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvImprintLocations">
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 25%;" rowspan="3">
                    </td>
                </tr>
            </table>
            <table id="tblgrdViewImprint" runat="server" class="product_detail" cellpadding="0"
                cellspacing="0">
                <tr style="height: 40px;">
                    <td>
                        <div>
                            <asp:GridView ID="grdViewImprint" runat="server" AutoGenerateColumns="false" CssClass="orderreturn_box"
                                GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="grdViewImprint_RowCommand"
                                OnRowDataBound="grdViewImprint_RowDataBound">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblLocationNo" Text='<%# Eval("ID") %>' />
                                            <asp:Label runat="server" ID="lblArtworkIDs" Text='<%# Eval("SelectedArtworkID") %>' />
                                            <asp:HiddenField ID="hdnddlLocValue" runat="server" Value='<%# Eval("ddlLocSelectedValue") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblLocationNm" Text='<%# Eval("LocationNo") %>' />
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <span style="height: 26px;">
                                                <asp:DropDownList ID="ddlId" Style="margin-top: 3px; background-color: #303030; border: #303030;
                                                    width: 100%; color: #72757C;" runat="server"  onchange="SetItemGridddlValue(this.value)">
                                                </asp:DropDownList>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:LinkButton runat="server" ID="slcArt" Text="Select Artwork" CommandName="Artwork"
                                                    CommandArgument='<%# Eval("ID") %>'  OnClientClick="return SetItemGridddlValue();"/>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a href="javascript:showNestedGridView('artID-<%# Eval("ID") %>');">
                                                <asp:Label runat="server" ID="lbl" Text='<%# Eval("ListIcons") %>' />
                                            </a>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:LinkButton runat="server" ID="lnkLocationProof" Text="Location Proof" CommandName="FileLocationProof" CommandArgument='<%# Eval("ID") %>'/>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:LinkButton ID="lnkbtnSCD" runat="server" CommandName="SCD" Text='<%# Eval("SCD") %> '
                                                    CommandArgument='<%# Eval("ID") %>'>
                                                </asp:LinkButton></span>
                                            <div class="corner">
                                                <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <tr>
                                                <td colspan="100%">
                                                    <div id="artID-<%# Eval("ID") %>" style="display: none; position: relative;">
                                                        <asp:GridView ID="nestedGridView" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                                            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="nestedGridView_RowDataBound"
                                                            OnRowCommand="nestedGridView_RowCommand" Style="margin-left: 250px; margin-right: 250px;
                                                            width: 40%;">
                                                            <Columns>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnFile" runat="server" Value='<%#Eval("ArtworkFilesJPG")%>' />
                                                                        <asp:HiddenField ID="hdnArtWorkId" runat="server" Value='<%#Eval("ArtworkDecoratingID")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Artwork No.</span>
                                                                        <div class="corner">
                                                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                                        </div>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblartnumber" Text='<%# Eval("ArtworkNumber") %>' />
                                                                        <div class="corner">
                                                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="g_box" Width="50%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Type</span>
                                                                        <div class="corner">
                                                                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                                                        </div>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                         <span style="height: 26px;">
                                                                            <asp:LinkButton ID="lnkDonwloadDoc" runat="server" CommandName="Download" CommandArgument='<%#Eval("ArtworkFilesJPG")%>'>
                                                                                <img id="imgFiletype" src="~/Images/download_btn.png" style="height: 20px; width: 20px;
                                                                                    margin: 3px 0px -4px 2px" runat="server" alt='no file' />
                                                                            </asp:LinkButton>
                                                                        </span>
                                                                        <div class="corner">
                                                                            <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="b_box" Width="50%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <table class="product_detail" cellpadding="0" cellspacing="0">
                <tr style="height: 40px;">
                    <td style="width: 25%;">
                    </td>
                    <td style="width: 50%;">
                        <table>
                            <tr id="trFinalProof" runat="server">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 30%;">Final Proof</span>
                                            <input type="file" id="fileFinalProof" style="margin: -24px 0px 8px 150px" runat="server"
                                                tabindex="7" />
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trFinalProductImage" runat="server">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 30%;">Final Product Image</span>
                                            <input type="file" id="fileProductImage" style="margin: -24px 0px 8px 150px" runat="server"
                                                tabindex="7" />
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="centeralign">
                                    <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Search files"
                                        OnClick="lnkBtnSaveInfo_Click"><span>Save</span></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 25%;">
                    </td>
                </tr>
            </table>
        </div>
        <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
        <at:ModalPopupExtender ID="modalImprint" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
            DropShadow="true" runat="server" PopupControlID="pnlSCD" CancelControlID="closepopup">
        </at:ModalPopupExtender>
        <asp:Panel ID="pnlSCD" runat="server" Style="display: none;">
            <div class="pp_pic_holder facebook" style="display: block; width: 411px; left: 35%;
                top: 25%; position: fixed;" runat="server" id="dvHolderFacebook">
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
                            <div class="pp_content" style="height: 355px; display: block;" runat="server" id="dvContent">
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
                                                    Select Decorator : <span>
                                                        <asp:DropDownList ID="ddlDecorator" runat="server">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="label_bar">
                                                    <asp:Label ID="lblMessage" runat="server" Font-Size="14px"></asp:Label>
                                                </div>
                                                <div class="label_bar">
                                                    Garment Range: S - XL
                                                </div>
                                                <div class="label_bar">
                                                    Screen Number : <span>
                                                        <asp:TextBox class="popup_input" ID="txtSXL" runat="server"></asp:TextBox></span>
                                                </div>
                                                <div class="label_bar">
                                                    Garment Range: 2XL - 3XL
                                                </div>
                                                <div class="label_bar">
                                                    Design Number: <span>
                                                        <asp:TextBox class="popup_input" ID="txt2XL" runat="server"></asp:TextBox></span>
                                                </div>
                                                <div class="label_bar">
                                                    Garment Range: 5XL - 7XL
                                                </div>
                                                <div class="label_bar">
                                                    Design Number: <span>
                                                        <asp:TextBox class="popup_input" ID="txt5XL" runat="server"></asp:TextBox></span>
                                                </div>
                                                <div class="centeralign">
                                                    <asp:Button ID="btnSCDSave" runat="server" OnClick="btnSCDSave_Click" Text="Save" />
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
        <at:ModalPopupExtender ID="modalpnlselectartwork" TargetControlID="lnkDummyAddNew"
            BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlselectartwork"
            CancelControlID="A1">
        </at:ModalPopupExtender>
        <asp:Panel ID="pnlselectartwork" runat="server" Style="display: none;">
            <div class="pp_pic_holder facebook" style="display: block; width: 600px; left: 25%;
                top: 25%; position: fixed;" runat="server" id="Div1">
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
                            <div class="pp_content" style="height: 355px; display: block;" runat="server" id="Div2">
                                <div class="pp_loaderIcon" style="display: none;">
                                </div>
                                <div class="pp_fade" style="display: block;">
                                    <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                    <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                            style="visibility: visible;">previous</a>
                                    </div>
                                    <div id="Div3">
                                        <div class="pp_inline clearfix">
                                            <div class="form_popup_box">
                                                <div class="form_pad">
                                                    <div class="form_table">
                                                        <asp:GridView ID="grdViewArtworkDetails" runat="server" AutoGenerateColumns="false"
                                                            CellPadding="4" ForeColor="#333333" GridLines="None">
                                                            <RowStyle BackColor="White" ForeColor="#333333" />
                                                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                                            <HeaderStyle BackColor="ButtonShadow" Font-Bold="True" ForeColor="Gray" />
                                                            <AlternatingRowStyle BackColor="White" />
                                                            <Columns>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblArtworkDecoratingID" Text='<%# Eval("ArtworkDecoratingID") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Check">
                                                                    <HeaderTemplate>
                                                                        <span style="height: 22px;">
                                                                            <asp:CheckBox ID="cbSelectAll" runat="server" OnClick="changeAll(this)" />&nbsp;</span>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle CssClass="centeralign" />
                                                                    <ItemTemplate>
                                                                        <span style="height: 22px;">
                                                                            <asp:CheckBox ID="chkSelect" runat="server" />&nbsp; </span>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="5%" CssClass="b_box centeralign" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Artwork Name</span>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblArtworkName" Text='<%# Eval("ArtworkName") %> ' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="g_box" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Artwork Number</span>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblArtworkNumber" Text='<%# Eval("ArtworkNumber") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="b_box" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <br />
                                                    <div class="centeralign">
                                                        <asp:Button ID="btnSaveArtworkID" runat="server" OnClick="btnSaveArtworkID_Click"
                                                            Text="Save" CausesValidation="false" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="pp_details clearfix" style="width: 371px;">
                                        <a href="#" id="A1" runat="server" class="pp_close">Close</a>
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
        <at:ModalPopupExtender ID="modalFileProofLoc" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
            DropShadow="true" runat="server" PopupControlID="pnlFileproofSCD" CancelControlID="closeFlpopup">
        </at:ModalPopupExtender>
        <asp:Panel ID="pnlFileproofSCD" runat="server" Style="display: none;">
            <div class="pp_pic_holder facebook" style="display: block; width: 411px; left: 35%;
                top: 25%; position: fixed;" runat="server" id="Div4">
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
                            <div class="pp_content" style="height: 155px; display: block;" runat="server" id="Div5">
                                <div class="pp_loaderIcon" style="display: none;">
                                </div>
                                <div class="pp_fade" style="display: block;">
                                    <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                    <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                            style="visibility: visible;">previous</a>
                                    </div>
                                    <div id="Div6">
                                        <div class="pp_inline clearfix">
                                            <div class="form_popup_box">
                                                <div class="centeralign">
                                                    <div class="label_bar">
                                                        Upload file for location proof
                                                    </div>
                                                    <div class="label_bar">
                                                        <asp:FileUpload ID="fileUploadLocProof" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="centeralign">
                                                    <asp:Button ID="btnFileProofSave" runat="server" OnClick="btnFileProofSave_Click"
                                                        Text="Save" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="pp_details clearfix" style="width: 371px;">
                                        <a href="#" id="closeFlpopup" runat="server" class="pp_close">Close</a>
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
    <asp:HiddenField ID="hdnddlChanges" runat="server" Value="0" />
</asp:Content>
