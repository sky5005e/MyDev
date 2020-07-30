<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="News.aspx.cs" Inherits="admin_CompanyStore_News" Title="News" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .form_table span.error
        {
            padding-left: 36px;
        }
    </style>
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="spacer2">
    </div>
    <div class="header_bg">
        <div class="header_bgr clearfix">
            <div class="basic_link">
                <asp:Menu ID="MainMenu" runat="server" Orientation="Horizontal">
                    <StaticSelectedStyle CssClass="current" />
                    <DynamicSelectedStyle CssClass="current" />
                    <Items>
                        <asp:MenuItem Text="News" Value="News" Selected="true" NavigateUrl="~/MyAccount/Store/News.aspx">
                        </asp:MenuItem>
                        <asp:MenuItem SeparatorImageUrl="~/Images/pipe.gif"></asp:MenuItem>
                        <asp:MenuItem Text="Contact" Value="Contact" NavigateUrl="~/MyAccount/Store/Contact.aspx">
                        </asp:MenuItem>
                        <asp:MenuItem SeparatorImageUrl="~/Images/pipe.gif"></asp:MenuItem>
                        <asp:MenuItem Text="Guideline Manuals" Value="GuidelineManuals" NavigateUrl="~/MyAccount/Store/GuideLineManuals.aspx">
                        </asp:MenuItem>
                    </Items>
                </asp:Menu>
            </div>
        </div>
    </div>

    <script type="text/javascript" language="javascript">
   
    </script>

    <script type="text/javascript" language="javascript">

        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected news?") == true)
                return true;
            else
                return false;
        }

        function ClearControl() {
           
        }

        //        $(function() {
        //            scrolltextarea(".scrollme2", "#Scrolltop2", "#ScrollBottom2");


        //        });
        var formats = 'pdf';
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    onsubmit: false,
                    rules: {

                        ctl00$ContentPlaceHolder1$hdnDepartment: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtNewsTitle: { required: true },
                        ctl00$ContentPlaceHolder1$txtPostDate: { required: true },
                        ctl00$ContentPlaceHolder1$txtExpirationDate: { required: true },
                        ctl00$ContentPlaceHolder1$fpUpload: { /*required: true,*/accept: formats }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$hdnDepartment: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department") },
                        ctl00$ContentPlaceHolder1$txtNewsTitle: { required: replaceMessageString(objValMsg, "Required", "news title") },
                        ctl00$ContentPlaceHolder1$txtPostDate: { required: replaceMessageString(objValMsg, "Required", "news post date") },
                        ctl00$ContentPlaceHolder1$txtExpirationDate: { required: replaceMessageString(objValMsg, "Required", "news expiration date") },
                        ctl00$ContentPlaceHolder1$fpUpload: { /*required: replaceMessageString(objValMsg, "Required", "files"),*/accept: "File type not supported." }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$hdnDepartment")
                            error.insertAfter("#dvDepartment");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtNewsTitle")
                            error.insertAfter("#dvNews");
                        else
                            error.insertAfter(element);
                    }


                });


                //Set Dropdowns in Edit Mode-- Start*/
                $('#Department option').each(function(i) {
                    if ($("#ctl00_ContentPlaceHolder1_hdnDepartment").val() == $(this).val()) {
                        $(this).attr("selected", "selected");
                        $("#Department").msDropDown({ mainCSS: 'dd2' });
                    }

                });

                


                /*End*/

                /*Change Event of all the dropdowns*/

                

                $('#Department').change(function() {

                    $("#ctl00_ContentPlaceHolder1_hdnDepartment").val($(this).val());
                    return $('#ctl00_ContentPlaceHolder1_hdnDepartment').valid();
                });


                /*End Change Event of all the dropdowns*/


            });

            $("#<%=lnkBtnUploadNews.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });
        });


        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
               // dateFormat: 'dd-mm-yy',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });

        //Function that gets fired when edit it clicked..

        function pageLoad(sender, args) {
            {

                
                $("#Department").msDropDown({ mainCSS: 'dd2' });

                $('#Department option').each(function(i) {
                    if ($("#ctl00_ContentPlaceHolder1_hdnDepartment").val() == $(this).val()) {
                        $(this).attr("selected", "selected");
                        $("#Department").msDropDown({ mainCSS: 'dd2' });
                    }
                });



                $('#Department').change(function() {

                    $("#ctl00_ContentPlaceHolder1_hdnDepartment").val($(this).val());
                    return $('#ctl00_ContentPlaceHolder1_hdnDepartment').valid();
                });

                $(function() {
                    $(".datepicker").datepicker({
                        buttonText: 'DatePicker',
                        showOn: 'button',
                        buttonImage: '../../images/calender-icon.jpg',
                        buttonImageOnly: true
                    });
                });


            }
        }

     
    </script>

    <div class="form_pad">
        <div>
            <div class="shipping_method_pad">
                <h4>
                    List of all News – View/Edit News and Delete News</h4>
                <asp:UpdatePanel ID="upPanleMain" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="dtlNews" />
                    </Triggers>
                    <ContentTemplate>
                        <div style="text-align: center">
                            <asp:Label ID="lblmsgList" runat="server" CssClass="errormessage"></asp:Label>
                        </div>
                        <div>
                            <asp:GridView ID="dtlNews" runat="server" HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box"
                                GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                                OnRowDataBound="dtlNews_RowDataBound" OnRowCommand="dtlNews_RowCommand">
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
                                                <asp:LinkButton ID="hypWorkgroup" CommandName="EditNews" CommandArgument='<%# Eval("StoreDocumentId") %>'
                                                    runat="server" ToolTip="Click here to edit news"><%# Eval("Department")%></asp:LinkButton>
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
                                    <asp:TemplateField SortExpression="UploadNews">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnUploadNews" runat="server" CommandArgument="NewsTitleDes"
                                                CommandName="Sort"><span>News Title</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderUploadNews" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnNewsTitleDes" runat="server" Value='<%# Eval("NewsTitleDes") %>' />
                                            <asp:Label runat="server" ID="lblNewsTitleDes" Text='<% # (Convert.ToString(Eval("NewsTitleDes")).Length > 15) ? Eval("NewsTitleDes").ToString().Substring(0,15)+"..." : Convert.ToString(Eval("NewsTitleDes")) %>'
                                                ToolTip='<%# Eval("NewsTitleDes") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="NewsTitle">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnNewsTitle" runat="server" CommandArgument="NewsTitle" CommandName="Sort"><span>Upload News</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderNewsTitle" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span>
                                                <asp:HiddenField ID="hfHiddenFile" runat="server" Value='<%# Eval("NewsTitle") %>' />
                                                <asp:LinkButton ID="lblNewsTitle" runat="server" CommandArgument='<%# Eval("NewsTitle") %>'
                                                    CommandName="viewnewsdoc" Text='<%# Eval("NewsTitle") %>'></asp:LinkButton>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="NewsPostDate">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnNewsPostDate" runat="server" CommandArgument="NewsPostDate"
                                                CommandName="Sort"><span>News Date</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderNewsPostDate" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblNewsPostDate" Text='<%# Eval("NewsPostDate") %>' />
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
                                            <%-- <asp:LinkButton ID="lnkbtndelete" CommandName="deletedocument" OnClientClick="return DeleteConfirmation();"
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
                    <asp:LinkButton ID="lnkAddNews" Text="Add News" CssClass="greysm_btn" runat="server"
                        OnClick="lnkAddNews_Click"><span>Add News</span></asp:LinkButton>
                </div>
                <div class="spacer25">
                </div>
                <div class="spacer25">
                </div>
                <div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                </div>
                <table class="select_box_pad form_table">
                    <tr>
                        <td>
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <asp:UpdatePanel ID="upnlCountry" runat="server">
                                        <ContentTemplate>
                                            <label class="dropimg_width300">
                                                <span class="form_box status_select label-sel-small">
                                                    <select id="Department" name="Department">
                                                        <option value="0">-select department-</option>
                                                        <%
                                                            
                                                            LookupDA sDepartment = new LookupDA();
                                                            LookupBE sDepartmentBE = new LookupBE();
                                                            sDepartmentBE.SOperation = "selectall";
                                                            sDepartmentBE.iLookupCode = "Department";
                                                            DataSet dsDepartment = sDepartment.LookUp(sDepartmentBE);
                                                            for (int i = 0; i < dsDepartment.Tables[0].Rows.Count; i++)
                                                            {
                                                                string path = "../../admin/Incentex_Used_Icons/" + dsDepartment.Tables[0].Rows[i]["sLookupIcon"].ToString();%>
                                                        <option value="<%=dsDepartment.Tables[0].Rows[i]["iLookupID"]%>" onchange="pageLoad(this,value);"
                                                            title="<%=path%>">
                                                            <%=dsDepartment.Tables[0].Rows[i]["sLookupName"].ToString()%>
                                                        </option>
                                                        <%
                                                            } %>
                                                    </select>
                                                </span>
                                                <div id="dvDepartment">
                                                </div>
                                            </label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
                                <div class="form_box employeeedit_text clearfix">
                                    <span class="input_label max_w">Workgroup</span>
                                    <div class="textarea_box alignright">
                                        <asp:Label ID="lblWG" runat="server"></asp:Label>
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
                                <asp:UpdatePanel ID="UpNewsTitle" runat="server">
                                    <ContentTemplate>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box employeeedit_text clearfix">
                                            <span class="input_label max_w">News Title</span>
                                            <div class="textarea_box alignright">
                                                <div class="scrollbar">
                                                    <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                                    </a>
                                                </div>
                                                <textarea id="txtNewsTitle" rows="3" runat="server" class="scrollme2"></textarea>
                                            </div>
                                            <div id="dvNews">
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr id="tdUpload" runat="server">
                        <td>
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box employeeedit_text clearfix">
                                    <span class="input_label max_w">UploadNews</span>
                                    <input id="fpUpload" type="file" runat="server" />
                                    <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                                        <img src="../../Images/lightbulb.gif" alt="note:" />&nbsp;Supported file formats
                                        are <b>.pdf</b></div>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upPostDate" runat="server">
                                <ContentTemplate>
                                    <div class="calender_l">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label max_w">News Post Date</span>
                                            <asp:TextBox ID="txtPostDate" CssClass="datepicker w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upExpiration" runat="server">
                                <ContentTemplate>
                                    <div class="calender_l">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box shipmax_in">
                                            <span class="input_label max_w">New Expiration Date</span>
                                            <asp:TextBox ID="txtExpirationDate" CssClass="cal_w datepicker min_w" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="spacer10">
                        </td>
                    </tr>
                    <tr>
                        <td class="centeralign">
                            <asp:LinkButton ID="lnkBtnUploadNews" class="grey2_btn" runat="server" ToolTip="Upload Image"
                                OnClick="lnkBtnUploadNews_Click"><span>Upload News</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="upPrimary" runat="server">
        <ContentTemplate>
            <input id="hdnWorkgroup" type="hidden" value="0" runat="server" />
            <input id="hdnDepartment" type="hidden" value="0" runat="server" />
            <input id="hdnEditNewsDocumentName" runat="server" value="0" type="hidden" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
