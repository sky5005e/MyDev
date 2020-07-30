<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="GuideLineManuals.aspx.cs" Inherits="admin_CompanyStore_GuideLineManuals"
    Title="GuideLine Manuals" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />

    <script type="text/javascript" language="javascript">

        function pageLoad(sender, args) {
            assigndesign();

        }
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected image?") == true)
                return true;
            else
                return false;
        }

        var formats = 'doc|txt|pdf|docx';

        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {

                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$fpUpload: { required: true, accept: formats },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department") },
                        ctl00$ContentPlaceHolder1$fpUpload: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported." },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "workgroup") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlDepartment")
                            error.insertAfter("#dvDepartment");
                        else
                            error.insertAfter(element);
                    }


                });

                /*End Change Event of all the dropdowns*/

                $("#<%=lnkBtnUploadWorkgroup.ClientID %>").click(function() {
                    return $('#aspnetForm').valid();

                });


            });
        });

    </script>

    <div class="form_pad">
        <div>
            <table class="select_box_pad form_table" style="width : 420px;">
                <h4>
                    Upload Guidelines Manual
                </h4>
                <tr>
                    <td>
                        <div>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label">Department :</span>
                            <label class="dropimg_width230">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlDepartment" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvDepartment">
                                </div>
                            </label>
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
                            <span class="input_label">Workgroup :</span>
                            <label class="dropimg_width230">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlWorkgroup" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvWorkgroup">
                                </div>
                            </label>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                    </td>
                </tr>
                <tr id="tdUpload" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <input id="fpUpload" type="file" runat="server" />
                                <br />
                                <br />
                                <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                                    <img src="../../Images/lightbulb.gif" alt="note:" />&nbsp;Supported file formats
                                    are <b>.doc|.txt|.pdf|.docx</b></div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnUploadWorkgroup" class="grey2_btn" runat="server" ToolTip="Upload Image"
                            OnClick="lnkBtnUploadWorkgroup_Click"><span>Upload</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <div class="shipping_method_pad">
                <h4>
                    List Of Guidelines Manuals</h4>
               <%-- <asp:UpdatePanel ID="upPanleMain" runat="server">
                    <ContentTemplate>--%>
                        <div style="text-align: center">
                            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                        </div>
                        <div>
                            <asp:GridView ID="dtlManuals" runat="server" HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box"
                                GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                                OnRowDataBound="dtlManuals_RowDataBound" OnRowCommand="dtlManuals_RowCommand">
                                <Columns>
                                    <asp:TemplateField Visible="False" HeaderText="Id">
                                        <HeaderTemplate>
                                            <span>Company Store ID</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCompanyID" Text='<%# Eval("StoreDocumentId") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Department">
                                        <HeaderTemplate>
                                            <span>
                                                <asp:LinkButton ID="lnkbtnDepartment" runat="server" CommandArgument="Department"
                                                    CommandName="Sort">Department</asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderDepartment" runat="server"></asp:PlaceHolder>
                                            </span>
                                            <div class="corner">
                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            
                                                <asp:Label CssClass="first" ID="lblDept" runat="server" Text='<%# Eval("Department")%>'></asp:Label>
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Workgroup">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnWorkgroup" runat="server" CommandArgument="Workgroup" CommandName="Sort"><span>Workgroup</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderWorkgroup" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCountry" Text='<%# Eval("Workgroup") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="DocumentName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnFileName" runat="server" CommandArgument="DocumentName"
                                                CommandName="Sort"><span>File Name</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderFileName" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfHiddenFile" runat="server" Value='<%# Eval("DocumentName") %>' />
                                            <span><asp:LinkButton ID="lblFileName" runat="server" CommandArgument='<%# Eval("DocumentName") %>' CommandName="viewguidelinemanual" Text='<%# Eval("DocumentName") %>'></asp:LinkButton> </span>
                                            <%--<asp:Label runat="server" ID="lblFileName" Text='<%# Eval("DocumentName") %>' />--%>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Delete</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtndelete" CommandName="deletedocument" OnClientClick="return DeleteConfirmation();"
                                                CommandArgument='<%# Eval("StoreDocumentId") %>' Text="X" runat="server">
                                                <span>
                                                    <img id="delete" src="~/Images/close.png" runat="server" /></span></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="20%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
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
                    <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
        </div>
        </div>
</asp:Content>
