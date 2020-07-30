<%@ Page Title="Document Storage Center" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="AddDocumentStorageType.aspx.cs" Inherits="admin_DocumentStorageCentre_AddDocumentStorageType" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../../JS/JQuery/jquery.MultiFile.pack.js" type="text/javascript" language="javascript"></script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: { ctl00$ContentPlaceHolder1$txtDocumentType: { required: true },
                        ctl00$ContentPlaceHolder1$txtpassword: { minlength: 4 }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtDocumentType: {required: replaceMessageString(objValMsg, "Required", "Document Storage Type")},
                        ctl00$ContentPlaceHolder1$txtpassword: { minlength: replaceMessageString(objValMsg, "MinLength", "four") }
                    }
                });
            });

            $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                if ($('#aspnetForm').valid()) {
                    $('#dvLoader').show();
                    return true;
                }
                else {
                    return false;
                }
            });
        });

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
    <div class="form_pad select_box_pad" style="width: 400px;">
        <div class="form_table">
            <div class="spacer10">
            </div>
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            <div class="spacer10">
            </div>
            <table>
                <tr id="trDocumentType" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%;">Document Storage Type </span>
                                <asp:TextBox ID="txtDocumentType" TabIndex="1" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trpassword" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%;">Password </span>
                                <asp:TextBox ID="txtpassword" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSaveInfo" TabIndex="3" class="grey2_btn" runat="server" ToolTip="Save Document Storage Type"
                            OnClick="lnkBtnSaveInfo_Click"><span>Save</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div class="form_table">
            <div class="spacer10">
            </div>
            <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="grdView_RowCommand"
                OnRowEditing="grdView_RowEditing">
                <Columns>
                    <asp:TemplateField SortExpression="iLookupCode">
                        <HeaderTemplate>
                            <span>Employee Training Type</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="first">
                                <%--<asp:Label runat="server" ID="lblTrainingType" Text='<%# Eval("sLookupName") %> ' />--%>
                                <asp:HiddenField ID="hdnlookupname" runat="server" Value='<%# Eval("sLookupName") %>' />
                                <asp:HiddenField ID="hdnpassword" runat="server" Value='<%# Eval("Val1") %>' />
                                <asp:HiddenField ID="hdncreateBy" runat="server" Value='<%# Eval("sLookupIcon") %>' />
                                <asp:LinkButton ID="lnkbtnedit" CommandName="edit" CommandArgument='<%# Eval("iLookupID") %>'
                                    runat="server">
                                       <span><%# Eval("sLookupName")%></span>
                                </asp:LinkButton>
                                <div class="corner">
                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                </div>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span>Delete</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtndelete" CommandName="DeleteTraining" OnClientClick="return Loadloder();"
                                    CommandArgument='<%# Eval("iLookupID") %>' runat="server" class="btn_space">
                                    <img alt="X" id="delete" src="~/Images/close.png" runat="server" /></asp:LinkButton>
                            </span>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box centeralign" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
