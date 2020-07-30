<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewStoreProduct.aspx.cs" Inherits="admin_ProductStoreManagement_AddProductView"
    Title="Store >> Product Store List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleContainerContent").each(function() {
                var hdnIamExpanded = document.getElementById($(this).parent().attr("id").substring(0, $(this).parent().attr("id").length - 13) + "hdnIamExpanded");
                if (hdnIamExpanded.value == "true") {
                    $(this).show();
                }
                else if (hdnIamExpanded.value == "false") {
                    $(this).hide();
                }
            });
        });            
            
    </script>

    <script type="text/javascript">

        function CollapsibleContainerTitleOnClick() {
            // The item clicked is the title div... get this parent (the overall container) and toggle the content within it.
            var hdnIamExpanded = document.getElementById($(this).parent().attr("id").substring(0, $(this).parent().attr("id").length - 13) + "hdnIamExpanded");

            $(".collapsibleContainerContent", $(this).parent()).slideToggle();

            if (hdnIamExpanded.value == "true") {
                hdnIamExpanded.value = "false";
            }
            else if (hdnIamExpanded.value == "false") {
                hdnIamExpanded.value = "true";
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }
    </script>

    <script type="text/javascript" language="javascript">
     function DeleteConfirmation() {
         if (confirm("Are you sure, you want to delete selected item?") == true)
             return true;
         else
             return false;
     }

     var formats = 'xls|xlsx';
     $().ready(function() {
         $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
             objValMsg = $.xml2json(xml);
             $("#aspnetForm").validate({
                 rules: {
                     ctl00$ContentPlaceHolder1$flproductUpload: { required: true, accept: formats },
                     ctl00$ContentPlaceHolder1$flSubproductUpload: { required: true, accept: formats }
                 },
                 messages: {
                     ctl00$ContentPlaceHolder1$flproductUpload: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "Please enter .xls or .xlsx only." },
                     ctl00$ContentPlaceHolder1$flSubproductUpload: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "Please enter .xls or .xlsx only." }
                 }

             });
             $("#<%=btnProductUpload.ClientID %>").click(function() {
                 $("#ctl00_ContentPlaceHolder1_flSubproductUpload").rules("remove");
                 $("#ctl00_ContentPlaceHolder1_flproductUpload").rules("add", {
                     required: true,
                     messages: { NotequalTo: replaceMessageString(objValMsg, "Required", "files"), accept: "Please enter .xls or .xlsx only." }
                 });
                 return $('#aspnetForm').valid();
             });
             $("#<%=btnSubItemUpload.ClientID %>").click(function() {
                 $("#ctl00_ContentPlaceHolder1_flproductUpload").rules("remove");
                 $("#ctl00_ContentPlaceHolder1_flSubproductUpload").rules("add", {
                     required: true,
                     messages: { NotequalTo: replaceMessageString(objValMsg, "Required", "files"), accept: "Please enter .xls or .xlsx only." }
                 });
                 return $('#aspnetForm').valid();
             });
             $("#<%=btnDownloadExcel.ClientID %>").click(function() {
                 $("#ctl00_ContentPlaceHolder1_flproductUpload").rules("remove");
                 $("#ctl00_ContentPlaceHolder1_flSubproductUpload").rules("remove");
                 return $('#aspnetForm').valid();
             });
         });

     });  
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <%--this is for select all checkbox within grid--%>

    <script type="text/javascript">
        function changeAll(chk) {
            var parent = getParentByTagName(chk, "table");
            var checkBoxes = parent.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++)
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].id.indexOf("CheckBox1") >= 0)
                checkBoxes[i].checked = chk.checked;
        }
        function getParentByTagName(obj, tag) {
            var obj_parent = obj.parentNode;
            if (!obj_parent) return false;
            if (obj_parent.tagName.toLowerCase() == tag) return obj_parent;
            else return getParentByTagName(obj_parent, tag);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="sc1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../../Images/ajax-loader-large.gif" />
        </div>
    </div>    
    <div class="form_pad">
        <div class="alignright">
            <asp:LinkButton ID="lnkDummyPro" class="grey2_btn" runat="server" Style="display: none"></asp:LinkButton>
            <asp:LinkButton ID="lnkProductUpload" runat="server" CssClass="grey2_btn" OnClick="lnkProductUpload_Click"><span>Upload Product</span></asp:LinkButton>
            <at:ModalPopupExtender ID="modalProduct" TargetControlID="lnkDummyPro" BackgroundCssClass="modalBackground"
                DropShadow="true" runat="server" PopupControlID="pnlProduct" CancelControlID="closepro">
            </at:ModalPopupExtender>
            <asp:LinkButton ID="lnkDummySub" class="grey2_btn" runat="server" Style="display: none"></asp:LinkButton>
            <asp:LinkButton ID="lnkSubItemBulkProduct" runat="server" CssClass="grey2_btn" OnClick="lnkSubItemBulkProduct_Click"><span>Upload Subitem Product</span></asp:LinkButton>
            <at:ModalPopupExtender ID="modalSubItemProduct" TargetControlID="lnkDummySub" BackgroundCssClass="modalBackground"
                DropShadow="true" runat="server" PopupControlID="pnlSubItemProduct" CancelControlID="closesub">
            </at:ModalPopupExtender>
            <div class="spacer10">
            </div>
        </div>
        <div id="dvCompanyName" runat="server" style="text-align: left; color: #5C5B60; font-size: larger;
            font-weight: bold; padding-left: 15px;">
            Company Name :
            <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
            <div class="spacer5">
            </div>
        </div>
        <div style="text-align: center">
            <asp:Label ID="lblMsgGlobal" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <asp:Repeater ID="rptWorkgroup" runat="server" OnItemDataBound="rptWorkgroup_ItemDataBound"
            OnItemCommand="rptWorkgroup_ItemCommand">
            <ItemTemplate>
                <div style="clear: both;" runat="server" id="dvCollapsible" class="collapsibleContainer"
                    title='<%# Eval("sLookupName")%>'>
                    <div style="text-align: center">
                        <asp:Label ID="lblmsg" runat="server" CssClass="errormessage"></asp:Label>
                    </div>
                    <asp:HiddenField ID="hdnIamExpanded" runat="server" Value="false" />
                    <asp:HiddenField ID="hdnWorkgroupId" Value='<%# DataBinder.Eval(Container.DataItem, "iLookupID")%>'
                        runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnWorkgroupName" Value='<%# DataBinder.Eval(Container.DataItem, "sLookupName")%>'
                        runat="server"></asp:HiddenField>
                    <asp:GridView ID="gvStoreProduct" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" DataKeys="lblStoreProductID" RowStyle-CssClass="ord_content"
                        OnRowCommand="gvStoreProduct_RowCommand" OnRowDataBound="gvStoreProduct_RowDataBound">
                        <Columns>
                            <asp:TemplateField Visible="False" HeaderText="Id">
                                <HeaderTemplate>
                                    <span>StoreProductID</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStoreProductID" Text='<%# Eval("StoreProductID") %>' />
                                    <asp:Label runat="server" ID="lblCompanyId" Text='<%# Eval("CompanyId") %>' />
                                    <asp:Label runat="server" ID="lblStoreid" Text='<%# Eval("StoreId") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="1%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span>
                                        <asp:CheckBox ID="cbSelectAll" OnClick="changeAll(this)" runat="server" />&nbsp;</span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemTemplate>
                                    <span class="first">
                                        <asp:CheckBox ID="CheckBox1" runat="server" />&nbsp; </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="5%" CssClass="b_box centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="CategoryName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnCategoryName" runat="server" CommandArgument="CategoryName"
                                        CommandName="Sort"><span >Product Category</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderCategoryName" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="hypCategoryName" ToolTip='<%# Eval("CategoryName") %>' CommandName="Edit"
                                        runat="server"><span><%#(Eval("CategoryName").ToString().Length > 15) ? Eval("CategoryName").ToString().Substring(0, 15) + "..." : Eval("CategoryName") + "&nbsp;" %></span> </asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="14%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="WorkgroupName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnWorkgroupName" runat="server" CommandArgument="WorkgroupName"
                                        CommandName="Sort"><span>Workgroup</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderWorkgroupName" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblWorkgroup" Text='<% # (Convert.ToString(Eval("WorkgroupName")).Length > 17) ? Eval("WorkgroupName").ToString().Substring(0,17)+"..." : Convert.ToString(Eval("WorkgroupName"))+ "&nbsp;"  %>'
                                        ToolTip='<%# Eval("WorkgroupName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="16%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="MasterItemNumber">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnMasterItemNumber" runat="server" CommandArgument="MasterItemNumber"
                                        CommandName="Sort"><span>Master Item Number</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderMasterItemNumber" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMasterItemNumber" Text='<% # (Convert.ToString(Eval("MasterItemNumber")).Length > 17) ? Eval("MasterItemNumber").ToString().Substring(0,17)+"..." : Convert.ToString(Eval("MasterItemNumber"))+ "&nbsp;"  %>'
                                        ToolTip='<%# Eval("MasterItemNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="17%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="ProductDescription">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnProductDescription" runat="server" CommandArgument="ProductDescription"
                                        CommandName="Sort"><span >Product Description</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderProductDescription" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblProductDesc" runat="server" Text='<% # (Convert.ToString(Eval("ProductDescription")).ToString().Length > 25) ? Eval("ProductDescription").ToString().Substring(0,25)+"..." :  Convert.ToString(Eval("ProductDescription")+ "&nbsp;") %>'
                                        ToolTip='<%# Eval("ProductDescription") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="23%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="GarmentType">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnGarmentType" runat="server" CommandArgument="GarmentType"
                                        CommandName="Sort"><span>Garment</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderGarmentType" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblGarmentType" Text='<%# Eval("GarmentType") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="EmployeeType">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnEmployeeType" runat="server" CommandArgument="EmployeeType"
                                        CommandName="Sort"><span>Employee Type</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderEmployeeType" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEmployeeType" runat="server" Text='<% # (Convert.ToString(Eval("EmployeeType")).ToString().Length > 13) ? Eval("EmployeeType").ToString().Substring(0,13)+"..." :  Convert.ToString(Eval("EmployeeType")+ "&nbsp;") %>'
                                        ToolTip='<%# Eval("EmployeeType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="12%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="ProductStatus">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnProductStatus" runat="server" CommandArgument="ProductStatus"
                                        CommandName="Sort"><span>Status</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderProductStatus" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnLookupIcon" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "IconPath")%>' />
                                    <asp:HiddenField ID="hdnStatusID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "StatusID")%>' />
                                    <%--<asp:Label ID="lblStatus" runat="server" Text="" class="btn_space"><img id="imgLookupIcon" style="height:20px;width:20px"  runat="server" alt='Loading' /></asp:Label>--%>
                                    <asp:LinkButton ID="lblStatus" runat="server" Text="" CommandName="StatusVhange"
                                        class="btn_space">
                                        <span class="btn_space">
                                            <img id="imgLookupIcon" style="height: 20px; width: 20px" runat="server" alt='Loading' /></span></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" Width="5%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div class="spacer15">
                    </div>
                    <div class="checktable_supplier true clearfix">
                        <%--<span class="custom-checkbox alignleft" id="menuspan" runat="server">--%>
                        <asp:CheckBox ID="chkActive" Checked="false" OnCheckedChanged="chkActive_CheckedChanged"
                            AutoPostBack="true" runat="server" />
                        <%-- </span>--%>
                        <label style="color: White; font-weight: bold">
                            Show Active/InActive Item</label>
                    </div>
                    <div class="spacer15">
                    </div>
                    <div id="dvButtonControler" runat="server">
                        <div class="companylist_botbtn alignleft">
                            <asp:LinkButton ID="btnAddProduct" runat="server" ToolTip="Add product" TabIndex="0"
                                CssClass="grey_btn" CommandName="AddNew"><span>Add Product</span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" CssClass="grey_btn" runat="server" ToolTip="Delete Product information"
                                TabIndex="0" OnClientClick="return DeleteConfirmation();" OnClick="btnDelete_Click"><span>Delete</span></asp:LinkButton>
                        </div>
                        <div id="pagingtable" runat="server" class="alignright pagging">
                            <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click">
                            </asp:LinkButton>
                            <span>
                                <asp:DataList ID="DataList2" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" OnItemCommand="DataList2_ItemCommand" OnItemDataBound="DataList2_ItemDataBound">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                            CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList></span>
                            <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                        </div>
                    </div>
                    <div style="clear: both;">
                    </div>
                </div>
                <br />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <asp:Panel ID="pnlProduct" runat="server" Style="display: none;">
        <div class="pp_pic_holder facebook" style="display: block; width: 490px; position: fixed;
            left: 30%; top: 30%;">
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
                        <div class="pp_content" style="height: 228px; display: block;">
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
                                            <div class="tbl_row">
                                                <strong>Upload Product Data </strong>
                                            </div>
                                            <div class="cl spacer">
                                            </div>
                                            <div class="cl spacer">
                                            </div>
                                            <div class="tbl_row">
                                                <div class="note" style="width: 100%;">
                                                    <img src="../../../Images/lightbulb.gif" alt="note:" />&nbsp;Please upload product
                                                    sheet. You can upload ".xls" and ".xlsx" files.</div>
                                            </div>
                                            <div class="cl spacer">
                                            </div>
                                            <div class="tbl_row">
                                                <input type="file" id="flproductUpload" runat="server" />
                                            </div>
                                            <div class="spacer">
                                            </div>
                                            <div class="tbl_row">
                                                <div class="note" style="width: 100%;">
                                                    <img src="../../../Images/lightbulb.gif" alt="note:" />&nbsp;Please fill each and
                                                    every field of excel sheet rows to upload successfully in our system.
                                                </div>
                                            </div>
                                            <div class="spacer">
                                            </div>
                                            <div class="tbl_row">
                                                <asp:Label ID="lblStatus" class="size-note" runat="server">
                                                </asp:Label>
                                            </div>
                                            <div class="cl spacer">
                                            </div>
                                            <div class="tbl_row">
                                                <asp:Button ID="btnProductUpload" runat="server" OnClick="btnProductUpload_Click"
                                                    Text="Upload" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="pp_details clearfix" style="width: 371px;">
                                    <a href="#" id="closepro" runat="server" class="pp_close">Close</a>
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
    <asp:Panel ID="pnlSubItemProduct" runat="server" Style="display: none;">
        <div class="pp_pic_holder facebook" style="display: block; width: 490px; position: fixed;
            left: 30%; top: 30%;">
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
                        <div class="pp_content" style="height: 300px; display: block;">
                            <div class="pp_loaderIcon" style="display: none;">
                            </div>
                            <div class="pp_fade" style="display: block;">
                                <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                    <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                        style="visibility: visible;">previous</a>
                                </div>
                                <div id="Div1">
                                    <div class="pp_inline clearfix">
                                        <div class="form_popup_box">
                                            <div class="tbl_row">
                                                <strong>Upload Subitem Product Data </strong>
                                            </div>
                                            <div class="cl spacer">
                                            </div>
                                            <div class="cl spacer">
                                            </div>
                                            <div class="tbl_row">
                                                <div class="note" style="width: 100%;">
                                                    <img src="../../../Images/lightbulb.gif" alt="note:" />&nbsp;Please get latest Product
                                                    item ID to fill in sub item field.</div>
                                            </div>
                                            <div class="cl spacer">
                                            </div>
                                            <div class="tbl_row">
                                                workgroup
                                                <asp:DropDownList ID="ddlWorkgruop" runat="server" onchange="pageLoad(this,value);" />
                                                <asp:Button ID="btnDownloadExcel" runat="server" OnClick="btnDownloadExcel_Click"
                                                    Text="Download" />
                                            </div>
                                            <div class="tbl_row">
                                                <div class="note" style="width: 100%;">
                                                    <img src="../../../Images/lightbulb.gif" alt="note:" />&nbsp;Please upload Subitem
                                                    product sheet. You can upload ".xls" and ".xlsx" files.</div>
                                            </div>
                                            <div class="cl spacer">
                                            </div>
                                            <div class="tbl_row">
                                                <input type="file" id="flSubproductUpload" runat="server" />
                                            </div>
                                            <div class="spacer">
                                            </div>
                                            <div class="tbl_row">
                                                <div class="note" style="width: 100%;">
                                                    <img src="../../../Images/lightbulb.gif" alt="note:" />&nbsp;Please fill each and
                                                    every field of excel sheet rows to upload successfully in our system.
                                                </div>
                                            </div>
                                            <div class="spacer">
                                            </div>
                                            <div class="tbl_row">
                                                <asp:Label ID="lblsubitemmsg" class="size-note" runat="server">
                                                </asp:Label>
                                            </div>
                                            <div class="cl spacer">
                                            </div>
                                            <div class="tbl_row">
                                                <asp:Button ID="btnSubItemUpload" runat="server" OnClick="btnSubItemUpload_Click"
                                                    Text="Upload" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="pp_details clearfix" style="width: 371px;">
                                    <a href="#" id="closesub" runat="server" class="pp_close">Close</a>
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
    <asp:HiddenField ID="hdnExpnad" runat="server" Value="" />
</asp:Content>
