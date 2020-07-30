<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="BankOrGeneralQuality.aspx.cs" Inherits="admin_Supplier_BankOrGeneralQuality" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 36px;
        }
    </style>

    <script language="javascript" type="text/javascript">

        $(function() {
             scrolltextarea(".scrollme2", "#Scrolltop2", "#ScrollBottom2");

         });

         function pageLoad(sender, args) {
             {
                 assigndesign();
             }
         }
         $().ready(function() {
             $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {

                 objValMsg = $.xml2json(xml);
                 //alert(objValMsg);

                 $("#aspnetForm").validate({
                     rules: {
                                ctl00$ContentPlaceHolder1$txtBankName: { required: true, alphanumeric: true }
                                , ctl00$ContentPlaceHolder1$txtBankContactPerson: { required: true, alphanumeric: true }
                                , ctl00$ContentPlaceHolder1$txtEmail: { email: true }
                                , ctl00$ContentPlaceHolder1$txtEmaiABA: { email: true }
                                , ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" }
                                , ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "0" }
                                , ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "0" }
                                , ctl00$ContentPlaceHolder1$fluDocument: { required: true }
                                , ctl00$ContentPlaceHolder1$txtTelephone: { alphanumeric: true }
                                , ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: true }
                                , ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: true }
                                , ctl00$ContentPlaceHolder1$txtMobile: { alphanumeric: true }
                                
                     }
                , messages:
                    {
                        ctl00$ContentPlaceHolder1$txtBankName: {
                            required: replaceMessageString(objValMsg, "Required", "bank name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        }
                        , ctl00$ContentPlaceHolder1$txtBankContactPerson: {
                            required: replaceMessageString(objValMsg, "Required", "contact person"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        }
                        , ctl00$ContentPlaceHolder1$txtEmail: {
                            email: replaceMessageString(objValMsg, "Email", "")
                        }
                            , ctl00$ContentPlaceHolder1$txtEmaiABA: {
                                email: replaceMessageString(objValMsg, "Email", "")
                            }
                        , ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") }
                        , ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") }
                        , ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") }
                        , ctl00$ContentPlaceHolder1$fluDocument: { required: replaceMessageString(objValMsg, "Required", "file") }
                        ,ctl00$ContentPlaceHolder1$txtTelephone: 
                        { 
                            alphanumeric: replaceMessageString(objValMsg, "Number", "")
                        },
                        ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtMobile: { alphanumeric: replaceMessageString(objValMsg, "Number", "") }


                    }//messages
                    , onsubmit: false
                    }); //validate
                    
             });



             $("#<%=lnkSave.ClientID %>").click(function() {
                $("ctl00$ContentPlaceHolder1$fluDocument").rules("remove");
                 return $('#aspnetForm').valid();
             }); //click

             $("#ctl00_ContentPlaceHolder1_btnSubmit").click(function() {
                
                 return $('#aspnetForm').valid();
             }); //click
             

        });                //ready

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="sc1" runat="server"></asp:ScriptManager>--%>
    <ajax:ToolkitScriptManager ID="sc1" runat="server">
    </ajax:ToolkitScriptManager>
    <mb:MenuUserControl ID="manuControl" runat="server" />
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <h4>
            Bank Information</h4>
        <div>
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <table class="form_table">
                        <tr>
                            <td class="formtd">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Bank Name</span>
                                                    <asp:TextBox ID="txtBankName" runat="server" CssClass="w_label" TabIndex="1"></asp:TextBox>
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
                                                    <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"
                                                            TabIndex="4">
                                                            <asp:ListItem Text="-select country-" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
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
                                                    <span class="input_label">Zip</span>
                                                    <asp:TextBox ID="txtZip" runat="server" CssClass="w_label" TabIndex="7"></asp:TextBox>
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
                                                    <span class="input_label">Telephone</span>
                                                    <asp:TextBox ID="txtTelephone" runat="server" CssClass="w_label" TabIndex="10"></asp:TextBox>
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
                                                    <span class="input_label">Account Name</span>
                                                    <asp:TextBox ID="txtAccountName" runat="server" CssClass="w_label" TabIndex="13"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="formtd">
                                <table>
                                    <tr>
                                        <td>
                                            <div class="shipmax_in">
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label ">Bank Contact </span>
                                                    <asp:TextBox ID="txtBankContactPerson" runat="server" CssClass="w_label" TabIndex="2"></asp:TextBox>
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
                                                    <span class="input_label">State/Province</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" TabIndex="5" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                            <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
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
                                                    <span class="input_label">Fax</span>
                                                    <asp:TextBox ID="txtFax" runat="server" CssClass="w_label" TabIndex="8"></asp:TextBox>
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
                                                    <span class="input_label">Mobile</span>
                                                    <asp:TextBox ID="txtMobile" runat="server" CssClass="w_label" TabIndex="11"></asp:TextBox>
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
                                                    <span class="input_label">Routing Number</span>
                                                    <asp:TextBox ID="txtRoutingNumber" runat="server" CssClass="w_label" TabIndex="14"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="formtd_r">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box employeeedit_text clearfix">
                                                    <span class="input_label alignleft">Address</span>
                                                    <div class="textarea_box alignright">
                                                        <div class="scrollbar">
                                                            <a href="#scroll" id="Scrolltop2" class="scrolltop"></a><a href="#scroll" id="ScrollBottom2"
                                                                class="scrollbottom"></a>
                                                        </div>
                                                        <asp:TextBox ID="txtAddress" runat="server" CssClass="scrollme2" TextMode="MultiLine"
                                                            Rows="3" TabIndex="3"></asp:TextBox>
                                                        <%--<textarea name="" cols="" rows="" class="scrollme2"></textarea>--%>
                                                    </div>
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
                                                    <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCity" runat="server" TabIndex="6">
                                                            <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
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
                                                    <span class="input_label">Email</span>
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="w_label" TabIndex="9"></asp:TextBox>
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
                                                    <span class="input_label">Account Number</span>
                                                    <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="w_label" TabIndex="12"></asp:TextBox>
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
                                                    <span class="input_label">EmaiABA/SWIFTl</span>
                                                    <asp:TextBox ID="txtEmaiABA" runat="server" CssClass="w_label" TabIndex="15"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="divider">
        </div>
        <h4>
            General Quality System Compliances</h4>
        <div>
            <table>
                <tr>
                    <td>
                        <!-- list start -->
                        <asp:DataList ID="lst" runat="server" RepeatLayout="Table" RepeatColumns="3" RepeatDirection="Horizontal"
                            OnDataBinding="lst_DataBinding">
                            <ItemTemplate>
                                <table class="checktable_supplier true">
                                    <tr>
                                        <td>
                                            <span id="spchk" runat="server" class="custom-checkbox alignleft">
                                                <%--<input type="checkbox" />--%>
                                                <asp:CheckBox ID="chk" runat="server" />
                                            </span>
                                            <label>
                                                <%#Eval("sLookupName") %>
                                            </label>
                                            <asp:Label ID="lblId" runat="server" Visible="false" Text='<%#Eval("iLookupID")%>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <ItemStyle Width="30%" />
                        </asp:DataList>
                        <!-- list end -->
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <%--<a href="#" class="greyicon_btn btn" title="Upload Quality Certificates">
											        <span class="btn_width285">Upload Quality Certificates<img src="../../images/upload-quality-icon.png" alt=""/></span>
											     </a>--%>
                                    <asp:LinkButton ID="lnkUploadQualityCertificates" runat="server" CssClass="greyicon_btn btn"
                                        OnClick="lnkUploadQualityCertificates_Click">
											            <span class="btn_width285">Upload Quality Certificates<img src="../../images/upload-quality-icon.png" alt=""/></span>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvDocumnet" runat="server" RowStyle-CssClass="ord_content" AutoGenerateColumns="false"
                                        HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                         OnDataBinding="gvDocumnet_DataBinding" OnRowCommand="gvDocumnet_RowCommand"
                                        OnRowDataBound="gvDocumnet_RowDataBound" Width="50%">
                                        <RowStyle CssClass="ord_content"></RowStyle>
                                        <Columns>
                                            <asp:TemplateField Visible="False" HeaderText="Id">
                                                <HeaderTemplate>
                                                    <span>Document ID</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDocumentID" Text='<%# Eval("DocumentId") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="FileName">
                                                <HeaderTemplate>
                                                    <span>File Name</span>
                                                    <%--<asp:LinkButton ID="lnkbtnFileName" runat="server" CommandArgument="FileName" CommandName="Sort"><span >File Name</span></asp:LinkButton>--%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%-- <asp:LinkButton runat="server" ID="lnkFileName" CommandName="view" CommandArgument='<%# Eval("FileName") %>'
                                                                    Text='<%# Eval("FileName") %>'></asp:LinkButton>--%>
                                                    <span>
                                                        <asp:HyperLink ID="lnkDoc" runat="server" Text='<%#Eval("OriginalFileName")%>' NavigateUrl='<%# "~/UploadedImages/SupplierDocuments/" + Eval("FileName")%>'
                                                            onclick="window.open(this.href);return false;"> </asp:HyperLink>
                                                    </span>
                                                    <%--  <a href="about:blank" onclick="window.open(this.href);return false;" >Open</a>--%>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span>Delete</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnFileName" runat="server" Value='<%#Eval("FileName")%>' />
                                                    <asp:LinkButton ID="lnkbtndelete" CommandName="del" OnClientClick="return confirm('Are you sure, you want to delete selected records ?');"
                                                        CommandArgument='<%#Eval("DocumentId")%>' runat="server">
                                                        <span>
                                                            <img id="delete" src="~/Images/close.png" runat="server" /></span>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box" Width="10%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider">
        </div>
        <h4>
            Type of Supplier</h4>
        <div>
            <table>
                <tr>
                    <td class="form_table">
                        <div class="calender_l shipmax_in" style="width: 450px">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label ">Type of Supplier</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlSupplierType" runat="server">
                                        <asp:ListItem Text="-select type-" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <!-- list start -->
                        <%-- <asp:DataList ID="lstSupplierType" runat="server" RepeatLayout="Table" RepeatColumns="3"
                            RepeatDirection="Horizontal" OnDataBinding="lstSupplierType_DataBinding">
                            <ItemTemplate>
                                <table class="checktable_supplier true">
                                    <tr>
                                        <td>
                                            <span class="custom-checkbox alignleft">
                                                <asp:CheckBox ID="chk" runat="server" />
                                            </span>
                                            <label>
                                                <%#Eval("sLookupName") %>
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <ItemStyle Width="30%" />
                        </asp:DataList>--%>
                        <!-- list end -->
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider">
        </div>
        <div class="additional_btn">
            <ul class="clearfix">
                <li>
                    <%--<a href="#" title="Save Information" class="grey2_btn"><span>Save Information</span></a>--%>
                    <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" OnClick="lnkSave_Click">
								        <span>Save Information</span>
                    </asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>
    <!-- Pop up  -->
    <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
    <ajax:ModalPopupExtender ID="modal" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
        DropShadow="true" runat="server" PopupControlID="pnlDocument" CancelControlID="closepopup">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlDocument" runat="server" Style="display: none;">
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
                        <div class="pp_content" style="height: 128px; display: block;">
                            <div class="pp_loaderIcon" style="display: none;">
                            </div>
                            <div class="pp_fade" style="display: block;">
                                <div id="pp_full_res">
                                    <div class="pp_inline clearfix">
                                        <div class="form_popup_box">
                                            <div class="label_bar">
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                            </div>
                                            <div class="label_bar">
                                                <label>
                                                    Upload File :</label>
                                                <span>
                                                    <asp:FileUpload ID="fluDocument" runat="server" />
                                                </span>
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
    <!-- Pop up end -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
