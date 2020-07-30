<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewEmployee.aspx.cs" Inherits="admin_Company_Employee_ViewEmployee"
    Title="View Company Employee" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdateProgress ID="upPro" runat="server" DisplayAfter="100">
        <ProgressTemplate>
            <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
            </div>
            <div class="updateProgressDiv">
                <img alt="Loading" src="../../../Images/ajax-loader-large.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <script type="text/javascript">
        // Let's use a lowercase function name to keep with JavaScript conventions
//        function selectAll(invoker) {
//            // Since ASP.NET checkboxes are really HTML input elements
//            //  let's get all the inputs
//            var inputElements = document.getElementsByTagName('input');
//            for (var i = 0; i < inputElements.length; i++) {
//                var myElement = inputElements[i];
//                // Filter through the input types looking for checkboxes
//                if (myElement.type === "checkbox") {
//                    myElement.checked = invoker.checked;
//                }
//            }
//        }

        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }

        function openPriModal() {
            $('#dvUpload').modal({ position: [110, ] });
        }
    </script>

    <mb:MenuUserControl ID="menucontrol" runat="server" />
    <div class="form_pad">
        <asp:UpdatePanel ID="upPanel" runat="server">
            <ContentTemplate>
                <div class="form_pad" style="padding-top: 10px !important;">
                    <div>
                        <table class="form_table">
                            <tr>
                                <td class="formtd">
                                    &nbsp;
                                </td>
                                <td class="formtd">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Login Email</span>
                                            <asp:TextBox ID="txtEmailAddress" runat="server">
                                            </asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                                <td class="formtd_r">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="botbtn centeralign">
                        <asp:LinkButton CssClass="grey2_btn" ID="btnSearch" runat="server" TabIndex="7" OnClick="btnSearch_Click"><span>Search</span></asp:LinkButton>
                        <asp:LinkButton CssClass="grey2_btn" ID="btnClear" runat="server" OnClick="btnClear_Click"><span>Clear</span></asp:LinkButton>
                    </div>
                    <br />
                    <br />
                    <div class="botbtn centeralign">
                        <a class="grey2_btn" title="Upload Photo"><span id="aUpload" onclick="openPriModal()">
                            Upload Employee</span></a>
                    </div>
                    <div class="form_pad">
                        <div style="text-align: center">
                            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                        </div>
                        <asp:GridView ID="gvEmployee" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnPageIndexChanging="gvEmployee_PageIndexChanging"
                            OnRowCommand="gvEmployee_RowCommand" OnRowDataBound="gvEmployee_RowDataBound">
                            <Columns>
                                <asp:TemplateField Visible="false" HeaderText="Id">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCompanyEmployeeID" Text='<%# Eval("CompanyEmployeeID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Check">
                                    <HeaderTemplate>
                                        <span class="custom-checkbox gridheader">
                                            <asp:CheckBox ID="cbSelectAll" runat="server"  />&nbsp;</span>
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="centeralign" />
                                    <ItemTemplate>
                                        <span class="first custom-checkbox gridcontent">
                                            <asp:CheckBox ID="chkemployee" runat="server" />&nbsp; </span>
                                        <div class="corner">
                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" CssClass="b_box centeralign" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="FullName">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnCusName" runat="server" CommandArgument="FullName" CommandName="Sort"><span>Employee Name</span></asp:LinkButton>
                                        <%--   <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>--%>
                                        <asp:PlaceHolder ID="placeholderEmployeeName" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%--  <span class="first">
                                    <asp:CheckBox ID="chkemployee" runat="server" />--%>
                                        <asp:LinkButton ID="lnkbtnfullname" CommandName="vieweditces" CommandArgument='<%# Eval("CompanyEmployeeID") %>'
                                            runat="server" ToolTip='<%# Eval("CompanyEmployeeID") %>'>
                                    <span><%# Eval("FullName") %></span>
                                        </asp:LinkButton>
                                        <%--</span><div class="corner">
                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                        </div>--%>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Country">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnCountry" runat="server" CommandArgument="Country" CommandName="Sort"><span class="white_co">Country</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderCountry" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCountry" Text='<%# Eval("Country") + "&nbsp;" %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="State">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnState" runat="server" CommandArgument="State" CommandName="Sort">
                                            <span>State</span>
                                        </asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderState" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblStationManager" Text='<%# Eval("State") + "&nbsp;" %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Telephone">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtncntctnum" runat="server" CommandArgument="Telephone" CommandName="Sort">
                                            <span class="white_co">Contact no.</span>
                                        </asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholdercontactnumber" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblStationAdmin" Text='<%# Eval("Telephone") + "&nbsp;" %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="LoginEmail">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnLoginEmail" runat="server" CommandArgument="LoginEmail"
                                            CommandName="Sort">
                                <span>Login Email</span>
                                        </asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderLoginEmail" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblLoginEmail" Text='<%# Eval("LoginEmail") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="WLSStatusId">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnProductStatus" runat="server" CommandArgument="WLSStatusId"
                                            CommandName="Sort"><span>Status</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderProductStatus" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnLookupIcon" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "IconPath")%>' />
                                        <asp:HiddenField ID="hdnStatusID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "WLSStatusId")%>' />
                                        <%--<asp:Label ID="lblStatus" runat="server" Text="" class="btn_space"><img id="imgLookupIcon" style="height:20px;width:20px"  runat="server" alt='Loading' /></asp:Label>--%>
                                        <asp:LinkButton ID="lblStatus" runat="server" Text="" CommandName="StatusVhange"
                                            class="btn_space">
                                            <span class="btn_space">
                                                <img id="imgLookupIcon" style="height: 20px; width: 20px" runat="server" alt='Loading' /></span></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box centeralign" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div>
                            <div>
                                <div class="companylist_botbtn alignleft">
                                    <asp:LinkButton ID="lnkBtnAddEmployee" OnClick="lnkBtnAddEmployee_Click" runat="server"
                                        class="grey_btn"><span>Add Employee</span></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnDelete" OnClick="lnkBtnDelete_Click" OnClientClick="return DeleteConfirmation();"
                                        runat="server" class="grey_btn"><span>Delete</span></asp:LinkButton>
                                </div>
                                <div class="alignright pagging" id="dvPaging" runat="server">
                                    <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                                    </asp:LinkButton>
                                    <span>
                                        <asp:DataList ID="dtlViewEmployee" runat="server" CellPadding="1" CellSpacing="1"
                                            OnItemCommand="dtlViewEmployee_ItemCommand" OnItemDataBound="dtlViewEmployee_ItemDataBound"
                                            RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </span>
                                    <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="cc" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div id="dvUpload" style="display: none;">
        <form id="frmUploadPri" method="post" action="../../../controller/uploadcompanyemployee.aspx"
        enctype="multipart/form-data">
        <div class="tbl_row">
            <strong>Upload User Data </strong>
        </div>
        <div class="cl spacer">
        </div>
        <div class="cl spacer">
        </div>
        <div class="tbl_row">
            <div class="note" style="width: 100%;">
                <img src="../../../Images/lightbulb.gif" alt="note:" />&nbsp;Please upload user
                sheet. You can upload "xls" and "xlsx" files.</div>
        </div>
        <div class="cl spacer">
        </div>
        <div class="tbl_row">
            <input type="file" size="45" name="flPriPhoto" id="flPriPhoto" onchange="checkValid();" />
        </div>
        <div class="spacer">
        </div>
        <div class="tbl_row">
            <div class="note" style="width: 100%;">
                <img src="../../../Images/lightbulb.gif" alt="note:" />&nbsp;<b>Following are required
                    fields to add data successfully..</b> FirstName,LastName,MiddleName,Email,LoginEmail,Password,HirerdDate,<br />
                EmployeeID,StoreActivatedDate,StoreActivatedBy,StratingCreditAmount
                <br />
                <b>AND You must have to add manually</b>
                <br />
                isCompanyAdmin,Usertype,CountryId,StateId,CityId,WLSStatusId,DepartmentID,WorkgroupID,<br />
                RegionID,GenderID,BaseStation,CompanyId,CreatedDate <b>in the excel sheet you are uploading!!</b>
            </div>
        </div>
        <%-- <div style="font-size: small; color: Black;">
            Maximum file size is <strong>1 MB.</strong>
        </div>--%>
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
                <img src="../../../Images/ajaxbtn.gif" alt="Please wait..." />&nbsp;Uploading...
            </div>
        </div>

        <script language="javascript" type="text/javascript">
            //setting allowed image file types
            var hash = { 'xls': 1, 'xlsx': 1 };

            function checkValid() {

                $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                    objValMsg = $.xml2json(xml);

                    if ($.trim($("#flPriPhoto").val()) == "") {

                        $('#dvEditStatus').html(replaceMessageString(objValMsg, "FileRequired", ""));
                        //$("#err").show();
                        return false;
                    }
                    if (!hash[get_extension($("#flPriPhoto").val())]) {
                        $('#dvEditStatus').html(replaceMessageString(objValMsg, "ImageType", "xls,xlsx"));
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

                $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                    objValMsg = $.xml2json(xml);

                    $('#frmUploadPri').ajaxSubmit({
                        data: {
                            mode: 'priphoto', Pg: "SignUp"
                        },
                        beforeSubmit: function(a, f, o) {

                            if ($.trim($("#flPriPhoto").val()) == "") {
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "FileRequired", ""));
                                return false;
                            }
                            if (!hash[get_extension($("#flPriPhoto").val())]) {
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "ImageType", "xls,xlsx"));
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
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "ImageType", "ImageType", "xls,xlsx"));
                            }
                            else if (retval == "false") {
                                btnUpload.show();
                                progstatus.hide();
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "ProcessError", ""));
                            }
                            else {

                                $('#dvEditStatus').html('');
                                $('#aDeletePri').show();
                                //Close the modal popup.
                                $.modal.close();

                                //refresh page
                                location.reload();



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
