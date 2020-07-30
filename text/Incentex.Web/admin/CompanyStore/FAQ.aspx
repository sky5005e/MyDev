<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="FAQ.aspx.cs" Inherits="admin_CompanyStore_FAQ" Title="FAQ" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .form_table span.error
        {
            padding-left: 36px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected image?") == true)
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
                        ctl00$ContentPlaceHolder1$txtQuestion: { required: true },
                        ctl00$ContentPlaceHolder1$txtAnswer: { required: true }

                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department") },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "workgroup") },
                        ctl00$ContentPlaceHolder1$txtQuestion: { required: replaceMessageString(objValMsg, "Required", "Question") },
                        ctl00$ContentPlaceHolder1$txtAnswer: { required: replaceMessageString(objValMsg, "Required", "Answer") }

                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlDepartment")
                            error.insertAfter("#dvDepartment");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtQuestion")
                            error.insertAfter("#divQuestion");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtAnswer")
                            error.insertAfter("#divAnswer");

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

            $("#<%=lnkBtnUploadFaq.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });
        });


        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });

        //Function that gets fired when edit it clicked..
        function pageLoad(sender, args) {
            assigndesign();

        }

     
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <div>
            <div class="shipping_method_pad">
                <h4>
                    List of all FAQ – View/Edit FAQ and Delete FAQ</h4>
                <asp:UpdatePanel ID="upPanleMain" runat="server">
                    <ContentTemplate>
                        <div style="text-align: center">
                            <asp:Label ID="lblmsg" runat="server" CssClass="errormessage"></asp:Label>
                        </div>
                        <div>
                            <asp:GridView ID="dtlFaq" runat="server" HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box"
                                GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                                OnRowDataBound="dtlFaq_RowDataBound" OnRowCommand="dtlFaq_RowCommand">
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
                                            <span class="first">
                                                <asp:LinkButton ID="hypWorkgroup" CommandName="EditFAQ" CommandArgument='<%# Eval("StoreDocumentId") %>'
                                                    runat="server" ToolTip="Click here to update FAQ"><%# Eval("Department")%></asp:LinkButton>
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
                                    <asp:TemplateField SortExpression="FaqQuestion">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnFaqQuestion" runat="server" CommandArgument="FaqQuestion"
                                                CommandName="Sort"><span>Question</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderFaqQuestion" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFaqQuestion" Text='<% # (Eval("FaqQuestion").ToString().Length > 20) ? Eval("FaqQuestion").ToString().Substring(0,20)+"..." : Eval("FaqQuestion")  %>'
                                                ToolTip='<% #Eval("FaqQuestion")  %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="FaqAnswer">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnFaqAnswer" runat="server" CommandArgument="FaqAnswer" CommandName="Sort"><span>Answer</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderFaqAnswer" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFaqAnswer" Text='<% # (Eval("FaqAnswer").ToString().Length > 20) ? Eval("FaqAnswer").ToString().Substring(0,20)+"..." : Eval("FaqAnswer")  %>'
                                                ToolTip='<% #Eval("FaqAnswer")  %>' />
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
                                            <%--  <asp:LinkButton ID="lnkbtndelete" CommandName="deletedocument" OnClientClick="return DeleteConfirmation();"
                                                CommandArgument='<%# Eval("StoreDocumentId") %>' Text="X" runat="server">
                                                <span>
                                                    <img id="delete" src="~/Images/close.png" runat="server" /></span></asp:LinkButton>--%>
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
                <div class="spacer10">
                </div>
                <div class="btn_space">
                    <%--   <asp:UpdatePanel ID="upBtn" runat="server">
                        <ContentTemplate>--%>
                    <asp:LinkButton ID="lnkAddFaq" Text="Add Faq" CssClass="greysm_btn" runat="server"
                        OnClick="lnkAddFaq_Click"><span>Add FAQ</span></asp:LinkButton>
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
                <div class="spacer25">
                </div>
                <div class="spacer25">
                </div>
                <table class="select_box_pad form_table" style="width: 420px;">
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
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>
                                <asp:UpdatePanel ID="UpFaqQuestion" runat="server">
                                    <ContentTemplate>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box employeeedit_text clearfix">
                                            <span class="input_label">Question</span>
                                            <div class="textarea_box alignright">
                                                <div class="scrollbar">
                                                    <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                                    </a>
                                                </div>
                                                <textarea id="txtQuestion" rows="3" runat="server" class="scrollme2"></textarea>
                                            </div>
                                            <%--<asp:TextBox ID="txtQuestion" TextMode="MultiLine" CssClass="w_label" runat="server"></asp:TextBox>--%>
                                            <div id="divQuestion">
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
                                <asp:UpdatePanel ID="UpAnswer" runat="server">
                                    <ContentTemplate>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box employeeedit_text clearfix"">
                                            <span class="input_label">Answer</span>
                                            <div class="textarea_box alignright">
                                                <div class="scrollbar">
                                                    <a href="#scroll" id="A3" class="scrolltop"></a><a href="#scroll" id="A4" class="scrollbottom">
                                                    </a>
                                                </div>
                                                <textarea id="txtAnswer" rows="3" runat="server" class="scrollme1"></textarea>
                                            </div>
                                            <%--<asp:TextBox ID="txtAnswer" CssClass="w_label" runat="server"></asp:TextBox>--%>
                                            <div id="divAnswer">
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
                            <asp:LinkButton ID="lnkBtnUploadFaq" class="grey2_btn" runat="server" ToolTip="Upload Image"
                                OnClick="lnkBtnUploadFaq_Click"><span>Upload FAQ</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
