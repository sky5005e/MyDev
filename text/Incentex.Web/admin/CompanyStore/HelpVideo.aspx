<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="HelpVideo.aspx.cs" Inherits="admin_CompanyStore_HelpVideo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtVideoName: { required: true },
                        ctl00$ContentPlaceHolder1$txtVideoPath: { required: true },
                        ctl00$ContentPlaceHolder1$ddlVideoFor: { NotequalTo: "0" }

                    },
                    messages: {

                        ctl00$ContentPlaceHolder1$txtVideoName: {
                            required:  replaceMessageString(objValMsg, "Required", "video Name")

                        },
                        ctl00$ContentPlaceHolder1$txtVideoPath: {
                            required: replaceMessageString(objValMsg, "Required", "Video Path")

                        },
                        ctl00$ContentPlaceHolder1$ddlVideoFor: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "video for")
                        }

                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtVideoName")
                            error.insertAfter("#divtxtVideoName");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtVideoPath")
                            error.insertAfter("#divtxtVideoPath");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlVideoFor")
                            error.insertAfter("#divddlVideoFor");
                        else
                            error.insertAfter(element);
                    }


                });

                $("#<%=lnkSubmit.ClientID %>").click(function() {

                    return $('#aspnetForm').valid();
                });
            });
        });

    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
        function PlayVideo(siteURL) {
            window.open(siteURL, 'playvideo', 'width=650,height=500 ,scrollbars=yes');
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div class="form_table">
            <table class="dropdown_pad ">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Video Name</span>
                                <asp:TextBox ID="txtVideoName" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
                                <div id="divtxtVideoName">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="VideoPath" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Video Path/Url</span>
                                <asp:TextBox ID="txtVideoPath" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                                <div id="divtxtVideoPath">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trVideoFor" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Video For</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlVideoFor" runat="server" AutoPostBack="false" onchange="pageLoad(this,value);">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="divddlVideoFor">
                                    </div>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trSubmit" runat="server">
                    <td>
                        <div class="botbtn aligncenter">
                            <asp:LinkButton ID="lnkSubmit" OnClick="lnkSubmit_Click" runat="server" class="grey2_btn"><span>Save</span></asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="dvVideoTemp" runat="server">
            <div style="text-align: center">
                <asp:Label ID="lblMsgGrid" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div class="spacer25">
            </div>
         <asp:GridView ID="gvVideoTemplatesList" runat="server" AutoGenerateColumns="false"
                HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                RowStyle-CssClass="ord_content" OnRowDataBound="gvVideoTemplatesList_RowDataBound"
                OnRowCommand="gvVideoTemplatesList_RowCommand">
                <Columns>
                    <asp:TemplateField Visible="False" HeaderText="Id">
                        <HeaderTemplate>
                            <span>TempID</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblItemID" Text='<%# Eval("HelpVideoID") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="2%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Check">
                        <HeaderTemplate>
                            <asp:Label ID="lblselect" runat="server" Text="Video Path" />                       
                            <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </HeaderTemplate>
                        <HeaderStyle CssClass="centeralign" />
                        <ItemTemplate>
                        <span>
                              <a id="hypVidepPath" runat="server" onclick="javascript:PlayVideo(this.title);"
                                title='<%# Eval("VideoPath")%>'><%# Eval("VideoPath")%></a></span>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="30%" CssClass="g_box leftalign" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="TempName">
                        <HeaderTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtnTempName" runat="server" CommandArgument="TempName" CommandName="Sort">Template Name</asp:LinkButton>
                            </span>
                            <asp:PlaceHolder ID="placeholderTempName" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                             <span>
                                <asp:LinkButton ID="lnkEdit" CommandName="updateData" runat="server" ToolTip="Edit" CommandArgument='<%# Eval("HelpVideoID") %>'><%#Eval("VideoName")%></asp:LinkButton>
                            </span>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="DateAdded">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnDateAdded" runat="server" CommandArgument="DateAdded" CommandName="Sort"><span>Date</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderDateAdded" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDate" Text='<%# Convert.ToDateTime(Eval("CreatedDate")).ToShortDateString()%>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span>Delete</span>
                            <div class="corner">
                                <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span class="btn_space">
                                <asp:LinkButton ID="lnkbtndelete" runat="server" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                    CommandArgument='<%# Eval("HelpVideoID") %>'>
                                    <asp:Image ID="img" ImageUrl="~/Images/close.png" runat="server" />
                                </asp:LinkButton></span>
                            <div class="corner">
                                <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box centeralign" Width="10%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
