<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Contact.aspx.cs" Inherits="admin_CompanyStore_Contact" Title="Contact" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
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
                        <asp:MenuItem Text="News" Value="News" NavigateUrl="~/MyAccount/Store/News.aspx">
                        </asp:MenuItem>
                        <asp:MenuItem SeparatorImageUrl="~/Images/pipe.gif"></asp:MenuItem>
                        <asp:MenuItem Text="Contact" Selected="true"  Value="Contact" NavigateUrl="~/MyAccount/Store/Contact.aspx">
                        </asp:MenuItem>
                        <asp:MenuItem SeparatorImageUrl="~/Images/pipe.gif"></asp:MenuItem>
                        <asp:MenuItem Text="Guideline Manuals"  Value="GuidelineManuals" NavigateUrl="~/MyAccount/Store/GuideLineManuals.aspx">
                        </asp:MenuItem>
                    </Items>
                </asp:Menu>
            </div>
        </div>
    </div>

    <script type="text/javascript" language="javascript">
        
                     
        
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected image?") == true)
                return true;
            else
                return false;
        }


               $(function() {
                   scrolltextarea(".scrollme2", "#Scrolltop2", "#ScrollBottom2");
               });



        function pageLoad(sender, args) {
            {

                assigndesign();

                //
                $("#Workgroup").msDropDown({ mainCSS: 'dd2' });
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


                $(function() {


                    if ($('#<%=hdPriPhoto.ClientID%>').val() != "") {
                        $('#aDeletePri').show();
                        var retval = $('#<%=hdPriPhoto.ClientID%>').val();
                        $('#imgPhotoupload').attr('src', '../../controller/createthumb.aspx?_ty=user&_path=' + retval + '&_twidth=140&_theight=161');
                        $('#imgPhotoupload').attr('title', retval);
                        $('#imgPhotoupload').parent().attr('href', '../../controller/createthumb.aspx?_ty=userlarge&_path=' + retval + '&_twidth=600&_theight=600');


                        //$('#ankit').attr('href', '../../../controller/createthumb.aspx?_ty=user&_path=' + val + '&_twidth=800&_theight=800');
                    }
                    else {
                        $('#imgPhotoupload').attr('src', '../../controller/createthumb.aspx?_ty=user&_path=employee-photo.gif&_twidth=140&_theight=161');
                        $('#imgPhotoupload').parent().attr('href', '../../controller/createthumb.aspx?_ty=userlarge&_path=employee-photo.gif&_twidth=600&_theight=600');

                        $('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value', 'employee-photo.gif');


                        $('#aDeletePri').hide();
                        $('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value', '');
                    }
                });


            }
        }


        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    onsubmit: false,
                    rules: {

                        ctl00$ContentPlaceHolder1$hdnDepartment: { NotequalTo: "0" },
                       
                        ctl00$ContentPlaceHolder1$txtFirstName: { required: true },
                        ctl00$ContentPlaceHolder1$txtLastName: { required: true },
                        ctl00$ContentPlaceHolder1$txtTitle: { required: true },
                        ctl00$ContentPlaceHolder1$txtCompanyName: { required: true },
                        ctl00$ContentPlaceHolder1$txtAddress1: { required: true },
                        ctl00$ContentPlaceHolder1$txtZip: { required: true,alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: true,alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtEmail: { required: true, email: true },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtFax:{alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtMobile:{alphanumeric: true }


                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$hdnDepartment: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department") },
                       
                        ctl00$ContentPlaceHolder1$txtFirstName: { required: replaceMessageString(objValMsg, "Required", "first name") },
                        ctl00$ContentPlaceHolder1$txtLastName: { required: replaceMessageString(objValMsg, "Required", "last name") },
                        ctl00$ContentPlaceHolder1$txtTitle: { required: replaceMessageString(objValMsg, "Required", "title") },
                        ctl00$ContentPlaceHolder1$txtCompanyName: { required: replaceMessageString(objValMsg, "Required", "company name") },
                        ctl00$ContentPlaceHolder1$txtAddress1: { required: replaceMessageString(objValMsg, "Required", "address1") },
                        ctl00$ContentPlaceHolder1$txtZip: { required: replaceMessageString(objValMsg, "Required", "zip code"),alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: replaceMessageString(objValMsg, "Required", "telephone"),alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtEmail: { required: replaceMessageString(objValMsg, "Required", "email"), email: replaceMessageString(objValMsg, "Valid", "email address") },
                        ctl00$ContentPlaceHolder1$ddlCountry: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country")
                        },
                        ctl00$ContentPlaceHolder1$ddlState: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state")
                        },
                        ctl00$ContentPlaceHolder1$ddlCity: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city")
                        },
                        ctl00$ContentPlaceHolder1$txtFax:{alphanumeric: replaceMessageString(objValMsg, "Number", "")},
                        ctl00$ContentPlaceHolder1$txtMobile:{alphanumeric: replaceMessageString(objValMsg, "Number", "") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$hdnWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$hdnDepartment")
                            error.insertAfter("#dvDepartment");
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

            $("#<%=lnkBtnUploadContact.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });


            //Delete Event
            $('#aDeletePri').click(function() {

                jConfirm(replaceMessageString(objValMsg, "DeleteConfirm", "photo"), "Delete photo", function(RetVal) {
                    if (RetVal) {
                        $('#aDeletePri').hide();
                        $('#dvProgPP').show();

                        $.post("../../controller/ajaxopes.aspx", {
                            mode: 'DELPRIPHOTO',
                            imgname: $('#imgPhotoupload').attr('title')
                        },
                        function(sRet) {
                            $('#dvProgPP').hide();
                            if (sRet == 'true') {

                                $('#imgPhotoupload').attr('src', '../../controller/createthumb.aspx?_ty=user&_path=employee-photo.gif&_twidth=140&_theight=161');
                                $('#imgPhotoupload').parent().attr('href', '../../controller/createthumb.aspx?_ty=userlarge&_path=employee-photo.gif&_twidth=600&_theight=600');

                                $('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value', 'employee-photo.gif');


                                $('#aDeletePri').hide();
                                $('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value', '');

                            }
                            else {
                                $('#aDeletePri').show();
                                $.showmsg('dvActionMessagePP', 'error', replaceMessageString(objValMsg, "ProcessError", ""), true);
                            }
                        }
                    );
                    }
                });
            });
        });






        function openPriModal() {
            $('#dvUpload').modal({ position: [110, ] });
        }
    </script>

    <div class="form_pad">
        <div>
            <div class="shipping_method_pad">
                <h4>
                    List of all contact – View/Edit Contacts and Delete Contacts</h4>
                <asp:UpdatePanel ID="upPanleMain" runat="server">
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
                                                <asp:LinkButton ID="hypWorkgroup" CommandName="EditContact" CommandArgument='<%# Eval("StoreDocumentId") %>'
                                                    runat="server" ToolTip="Click here to update contact"><%# Eval("Department")%></asp:LinkButton>
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
                                    <asp:TemplateField SortExpression="FirstName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnFirstName" runat="server" CommandArgument="FirstName" CommandName="Sort"><span>First Name</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderFirstName" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFirstName" Text='<%# Eval("FirstName") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="LastName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnLastName" runat="server" CommandArgument="LastName" CommandName="Sort"><span>Last Name</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderLastName" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblLastName" Text='<%# Eval("LastName") %>' />
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
                                            <%--<asp:LinkButton ID="lnkbtndelete" CommandName="deletedocument" OnClientClick="return DeleteConfirmation();"
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
                    <%--<asp:UpdatePanel ID="upBtn" runat="server">
                        <ContentTemplate>--%>
                    <asp:LinkButton ID="lnkAddContact" Text="Add Contanct" CssClass="greysm_btn" runat="server"
                        OnClick="lnkAddContact_Click"><span>Add Contact</span></asp:LinkButton>
                    <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
                <div class="spacer25">
                </div>
                <div class="spacer25">
                </div>
                <div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                </div>
                <table>
                    <tr>
                        <td class="formtd_member">
                            <table class="select_box_pad form_table">
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
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
                                                            <option value="<%=dsDepartment.Tables[0].Rows[i]["iLookupID"]%>" title="<%=path%>"
                                                                onchange="pageLoad(this,value);">
                                                                <%=dsDepartment.Tables[0].Rows[i]["sLookupName"].ToString()%>
                                                            </option>
                                                            <%
                                                                } %>
                                                        </select>
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
                                            <asp:UpdatePanel ID="UpFN" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">First Name</span>
                                                        <asp:TextBox ID="txtFirstName" CssClass="w_label" runat="server"></asp:TextBox>
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
                                            <asp:UpdatePanel ID="upLM" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Last Name</span>
                                                        <asp:TextBox ID="txtLastName" CssClass="w_label" runat="server"></asp:TextBox>
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
                                            <asp:UpdatePanel ID="UpTitle" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Title</span>
                                                        <asp:TextBox ID="txtTitle" CssClass="w_label" runat="server"></asp:TextBox>
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
                                            <asp:UpdatePanel ID="UpNewsTitle" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box employeeedit_text clearfix">
                                                        <span class="input_label">User Role</span>
                                                        <div class="textarea_box alignright">
                                                            <div class="scrollbar">
                                                                <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                                                </a>
                                                            </div>
                                                            <textarea id="txtRoles" rows="3" runat="server" class="scrollme2"></textarea>
                                                        </div>
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
                                            <asp:UpdatePanel ID="upDepartment" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Department</span>
                                                        <asp:TextBox ID="txtDepartment" CssClass="w_label" runat="server"></asp:TextBox>
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
                                            <asp:UpdatePanel ID="upCompanyName" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Company Name</span>
                                                        <asp:TextBox ID="txtCompanyName" CssClass="w_label" runat="server"></asp:TextBox>
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
                                            <asp:UpdatePanel ID="upAddress1" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Address1</span>
                                                        <asp:TextBox ID="txtAddress1" CssClass="w_label" runat="server"></asp:TextBox>
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
                                            <asp:UpdatePanel ID="upAddress2" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Address2</span>
                                                        <asp:TextBox ID="txtAddress2" CssClass="w_label" runat="server"></asp:TextBox>
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
                                            <asp:UpdatePanel ID="upnlCountry" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlCountry" runat="server" onchange="pageLoad(this,value);"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </span>
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
                                            <asp:UpdatePanel ID="upnlSate" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlState" runat="server" onchange="pageLoad(this,value);" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                                <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                        <div>
                                                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upnlCountry">
                                                                <ProgressTemplate>
                                                                    <img src="~/Images/ajaxbtn.gif" class="progress_img" /></ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                        </div>
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
                                            <asp:UpdatePanel ID="upnlCity" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="true">
                                                                <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                        <div>
                                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upnlSate">
                                                                <ProgressTemplate>
                                                                    <img src="~/Images/ajaxbtn.gif" class="progress_img" /></ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                        </div>
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
                                            <asp:UpdatePanel ID="upZip" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Zip</span>
                                                        <asp:TextBox ID="txtZip" CssClass="w_label" runat="server"></asp:TextBox>
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
                                            <asp:UpdatePanel ID="upTelephone" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Telephone</span>
                                                        <asp:TextBox ID="txtTelephone" CssClass="w_label" runat="server"></asp:TextBox>
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
                                            <asp:UpdatePanel ID="upFax" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Fax</span>
                                                        <asp:TextBox ID="txtFax" CssClass="w_label" runat="server"></asp:TextBox>
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
                                            <asp:UpdatePanel ID="upMobile" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Mobile</span>
                                                        <asp:TextBox ID="txtMobile" CssClass="w_label" runat="server"></asp:TextBox>
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
                                            <asp:UpdatePanel ID="upEmail" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Email</span>
                                                        <asp:TextBox ID="txtEmail" CssClass="w_label" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
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
                                        <asp:LinkButton ID="lnkBtnUploadContact" class="grey2_btn" runat="server" ToolTip="Upload Contact"
                                            OnClick="lnkBtnUploadContact_Click"><span>Upload Contact</span></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="upload_img" align="center" style="padding-top: 0px;">
                            <div class="top_songs_midbg" id="dvPriPhoto">
                                <div>
                                    <span class="lt_co"></span><span class="rt_co"></span><span class="lb_co"></span>
                                    <span class="rb_co"></span>
                                    <div id="dvActionMessagePP">
                                    </div>
                                    <div id="dvPriPhotoContainer" class="upload_photo gallery" runat="server">
                                        <a href="../../UploadedImages/employeePhoto/employee-photo.gif" rel='prettyPhoto'>
                                            <img id='imgPhotoupload' src="../../UploadedImages/employeePhoto/employee-photo.gif" /></a>
                                    </div>
                                    <div class="upload_btn">
                                        <div class="noteIncentex" style="width: 100%; font-size: 12px; border: 1px;">
                                            <img src="../Images/lightbulb.gif" style="z-index: -1" alt="note:" />&nbsp;Supported
                                            file resolution is 140 X 160 (Width X Height)</div>
                                    </div>
                                    <div class="upload_btn">
                                        <a class="grey2_btn" title="Delete photo"><span id="aDeletePri">Delete photo</span>
                                        </a>
                                    </div>
                                    <div class="upload_btn">
                                        <a class="grey2_btn" title="Upload Photo"><span id="aUpload" onclick="openPriModal()">
                                            Upload Photo</span></a>
                                    </div>
                                </div>
                            </div>
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
            <input type="hidden" id="hdPriPhoto" runat="server" value="" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="cc" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div id="dvUpload" style="display: none;">
        <form id="frmUploadPri" method="post" action="../controller/UploadPhoto.aspx" enctype="multipart/form-data">
        <div class="tbl_row">
            <strong>Upload photo </strong>
        </div>
        <div class="cl spacer">
        </div>
        <div class="cl spacer">
        </div>
        <div class="tbl_row">
            <div class="note" style="width: 100%;">
                <img src="Images/lightbulb.gif" alt="note:" />&nbsp;Please upload your photo. You
                can upload jpg, gif, bmp and png image files.</div>
        </div>
        <div class="cl spacer">
        </div>
        <div class="cl spacer">
        </div>
        <div class="tbl_row">
            <input type="file" size="45" name="flPriPhoto" id="flPriPhoto" onchange="checkValid();" />
        </div>
        <div class="spacer">
        </div>
        <div style="font-size: small; color: Black;">
            Maximum file size is <strong>1 MB.</strong>
        </div>
        <div class="spacer">
        </div>
        <div class="tbl_row">
            <label id="dvEditStatus" class="size-note">
            </label>
        </div>
        <div class="cl spacer">
        </div>
        <div class="tbl_row">
            <input type="button" onclick="submitpriphoto()" id="btnUploadPri" value="Upload" />
            <div id="imgProcess" style="display: none;">
                <img src="Images/ajaxbtn.gif" alt="Please wait..." />&nbsp;Uploading...
            </div>
        </div>

        <script language="javascript" type="text/javascript">
            //setting allowed image file types
            var hash = { 'jpg': 1, 'gif': 1, 'jpeg': 1, 'png': 1, 'bmp': 1 };

            function checkValid() {

                $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                    objValMsg = $.xml2json(xml);

                    if ($.trim($("#flPriPhoto").val()) == "") {

                        $('#dvEditStatus').html(replaceMessageString(objValMsg, "FileRequired", ""));
                        //$("#err").show();
                        return false;
                    }
                    if (!hash[get_extension($("#flPriPhoto").val())]) {
                        $('#dvEditStatus').html(replaceMessageString(objValMsg, "ImageType", "jpg,gif,bmp,png"));
                        //$("#err").show();
                        return false;
                    }
                    $('#dvEditStatus').html('');
                    return true;

                });
            }

            function submitpriphoto() {

                var btnUpload = $('#btnUploadPri');
                var progstatus = $('#imgProcess');

                $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                    objValMsg = $.xml2json(xml);

                    $('#frmUploadPri').ajaxSubmit({
                        data: {
                            mode: 'storecontactphoto', Pg: "SignUp"
                        },
                        beforeSubmit: function(a, f, o) {

                            if ($.trim($("#flPriPhoto").val()) == "") {
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "FileRequired", ""));
                                return false;
                            }
                            if (!hash[get_extension($("#flPriPhoto").val())]) {
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "ImageType", "jpg,gif,bmp,png"));
                                //$("#err").show();
                                return false;
                            }

                            btnUpload.hide();
                            progstatus.show();

                        },
                        success: function(retval) {

                            if (retval == "SIZELIMIT") {
                                btnUpload.show();
                                progstatus.hide();
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "FileSize", "1MB"));
                            }
                            else if (retval == "FILETYPE") {
                                btnUpload.show();
                                progstatus.hide();
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "ImageType", "ImageType", "jpg,gif,bmp,png"));
                            }
                            else if (retval == "false") {
                                btnUpload.show();
                                progstatus.hide();
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "ProcessError", ""));
                            }
                            else {
                                //window.location.reload();
                                $('#dvEditStatus').html('');
                                $('#aDeletePri').show();

                                if ($('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value') != "") {

                                    $.post("../controller/ajaxopes.aspx", {
                                        mode: 'DELETESTOREPHOTO',
                                        imgname: $('#imgPhotoupload').attr('title')
                                    },
                                        function(sRet) {
                                        }
                                    );

                                }

                                $('#imgPhotoupload').attr('src', '../../controller/createthumb.aspx?_ty=CompanyStoreContact&_path=' + retval + '&_twidth=140&_theight=161');
                                $('#imgPhotoupload').attr('title', retval);
                                $('#imgPhotoupload').parent().attr('href', '../../controller/createthumb.aspx?_ty=CompanyStoreContact&_path=' + retval + '&_twidth=600&_theight=600');

                                $('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value', retval);
                                $.modal.close();
                            }
                        }
                    });
                });
            }
            function get_extension(n) {
                n = n.substr(n.lastIndexOf('.') + 1);
                return n.toLowerCase();
            }
        </script>

        </form>
    </div>
</asp:Content>
