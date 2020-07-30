<%@ Page Title="Access System" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="AccessSystem.aspx.cs" Inherits="admin_CompanyStore_AccessSystem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_table span.error
        {
            padding-left: 36px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected tearms & conditions?") == true)
                return true;
            else
                return false;
        }


        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    onsubmit: false,
                    rules: {

                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtHeader: { required: true },
                        ctl00$ContentPlaceHolder1$txtContent: { required: true }

                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department") },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "workgroup") },
                        ctl00$ContentPlaceHolder1$txtHeader: { required: replaceMessageString(objValMsg, "Required", "Header") },
                        ctl00$ContentPlaceHolder1$txtContent: { required: replaceMessageString(objValMsg, "Required", "Content") }

                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlDepartment")
                            error.insertAfter("#dvDepartment");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtHeader")
                            error.insertAfter("#divHeader");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtContent")
                            error.insertAfter("#divContent");

                        else
                            error.insertAfter(element);
                    }


                });
                /*End Change Event of all the dropdowns*/

                $(function() {
                    scrolltextarea(".scrollme2", "#Scrolltop2", "#ScrollBottom2");
                    scrolltextarea(".scrollme1", "#Scrolltop1", "#ScrollBottom1");

                });


            });

            $("#<%=lnkBtnUploadTNC.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });
        });

        //Function that gets fired when edit it clicked..
        function pageLoad(sender, args) {
            assigndesign();

        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <div>
            <div class="shipping_method_pad">
                <div class="checktable_supplier true clearfix" style="margin-bottom: 20px;">
                    <span class="custom-checkbox alignleft" id="AccessSystemspan" runat="server">
                        <asp:CheckBox AutoPostBack="true" runat="server" ID="chkAccessSystem" OnCheckedChanged="chkAccessSystem_CheckedChanged" />
                    </span>
                    <label>
                        Access System&nbsp;(on/off).</label>
                </div>
                <%--Start display list of condition on grid with paging--%>
                <h4>
                    List of all Terms & Conditions – View/Edit and Delete</h4>
                <asp:UpdatePanel ID="upPanleMain" runat="server">
                    <ContentTemplate>
                        <div style="text-align: center">
                            <asp:Label ID="lblmsg" runat="server" CssClass="errormessage"></asp:Label>
                        </div>
                        <div>
                            <asp:GridView ID="dtlTNC" runat="server" HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box"
                                GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                                OnRowDataBound="dtlTNC_RowDataBound" OnRowCommand="dtlTNC_RowCommand">
                                <Columns>
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
                                            <span class="first">
                                                <asp:LinkButton ID="hypWorkgroup" CommandName="EditTNC" CommandArgument='<%# Eval("StoreDocumentId") %>'
                                                    runat="server" ToolTip="Click here to update TNC"><%# Eval("Department")%></asp:LinkButton>
                                            </span>
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
                                            <asp:Label runat="server" ID="lblWorkgroup" Text='<%# Eval("Workgroup") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="TNCHeader">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnTNCHeader" runat="server" CommandArgument="TNCHeader" CommandName="Sort"><span>Header</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderTNCHeader" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTNCHeader" Text='<% # (Eval("TNCHeader").ToString().Length > 20) ? Eval("TNCHeader").ToString().Substring(0,20)+"..." : Eval("TNCHeader")  %>'
                                                ToolTip='<% #Eval("TNCHeader")  %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="TNCContent">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnTNCContent" runat="server" CommandArgument="TNCContent"
                                                CommandName="Sort"><span>Content</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderTNCContent" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTNCContent" Text='<% # (Eval("TNCContent").ToString().Length > 20) ? Eval("TNCContent").ToString().Substring(0,20)+"..." : Eval("TNCContent")  %>'
                                                ToolTip='<% #Eval("TNCContent")  %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Delete</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:ImageButton ID="lnkbtndelete" CommandName="deletedocument" OnClientClick="return DeleteConfirmation();"
                                                    CommandArgument='<%# Eval("StoreDocumentId") %>' runat="server" ImageUrl="~/Images/close.png" /></span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="20%" />
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
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--End display list of condition on grid with paging--%>
                <div class="spacer10">
                </div>
                <div class="btn_space">
                    <asp:LinkButton ID="lnkAddTNC" Text="Add Terms & Conditions" CssClass="greysm_btn"
                        runat="server" OnClick="lnkAddTNC_Click"><span>Add Terms & Conditions</span></asp:LinkButton>
                </div>
                <div class="spacer25">
                </div>
                <div class="spacer25">
                </div>
                <%--Start table for add/edit condition based on workgroup and department--%>
                <table class="select_box_pad form_table" style="width : 420px;">
                    <tr>
                        <td>
                            <div>
                             <asp:UpdatePanel ID="upDepartment" runat="server">
                                    <ContentTemplate>
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
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>
                             <asp:UpdatePanel ID="upWorkgroup" runat="server">
                                    <ContentTemplate>
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
                                    </ContentTemplate></asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>
                                <asp:UpdatePanel ID="UpQuestion" runat="server">
                                    <ContentTemplate>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box employeeedit_text clearfix">
                                            <span class="input_label">Header</span>
                                            <div class="textarea_box alignright">
                                                <div class="scrollbar">
                                                    <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                                    </a>
                                                </div>
                                                <textarea id="txtHeader" rows="3" runat="server" class="scrollme2"></textarea>
                                            </div>
                                            <%--<asp:TextBox ID="txtQuestion" TextMode="MultiLine" CssClass="w_label" runat="server"></asp:TextBox>--%>
                                            <div id="divHeader">
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>
                                <asp:UpdatePanel ID="UpContent" runat="server">
                                    <ContentTemplate>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box employeeedit_text clearfix"">
                                            <span class="input_label">Content</span>
                                            <div class="textarea_box alignright">
                                                <div class="scrollbar">
                                                    <a href="#scroll" id="A3" class="scrolltop"></a><a href="#scroll" id="A4" class="scrollbottom">
                                                    </a>
                                                </div>
                                                <textarea id="txtContent" rows="3" runat="server" class="scrollme1"></textarea>
                                            </div>
                                            <div id="divContent">
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="spacer10">
                        </td>
                    </tr>
                    <tr>
                        <td class="centeralign">
                            <asp:LinkButton ID="lnkBtnUploadTNC" class="grey2_btn" runat="server" OnClick="lnkBtnUploadTNC_Click"><span>Upload Terms & Conditions</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <%--End table for add/edit condition based on workgroup and department--%>
            </div>
        </div>
    </div>

</asp:Content>
