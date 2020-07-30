<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="AccessRights.aspx.cs" Inherits="admin_IncentexEmployee_AccessRights"
    Title="Incentex Employees - Access Rights" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_pad .divider
        {
            height: 2px;
            margin: 0px 0px 0px 0px;
        }
        .checktable_supplier td
        {
            padding: 1px 0px 1px 0px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function SetCheckCheckBox()
        {
            $(".custom-checkbox input:checked").each(function(){
                $(this).siblings("span").toggleClass("checkbox-off",true);
                $(this).siblings("span").toggleClass("checkbox-on",false);
            });
        }
        $().ready(function() {
            SetCheckCheckBox();
//            $(".chkHeader :checkbox").click(function() {
//                var chkID = $(this).attr("id").substring($(this).attr("id").indexOf("chk"), $(this).attr("id").indexOf("All"));
//                $(".chkLine :checkbox[id$='" + chkID + "']").attr("checked", $(this).is(":checked"));
//            });
//            
//            $(".chkLine :checkbox").click(function() {
//                var chkID = $(this).attr("id").substring($(this).attr("id").indexOf("chk"), $(this).attr("id").length);
//                if (!$(this).is(":checked")) {
//                    $(".chkHeader :checkbox[id$='chkViewAll']").attr("checked", false);
//                }
//                else {                
//                    var IsChecked = true;
//                    $(".chkLine :checkbox[id$='" + chkID + "']").each(function() {
//                        if (!$(this).is(":checked")) {
//                            IsChecked = false;
//                            return false;
//                        }
//                    });
//                    $(".chkHeader :checkbox[id$='" + chkID + "All']").attr("checked", IsChecked);
//                }
//            });
            
            $("table[class^='parent']").each(function() {
                $(this).find("span[id$='lblAccessMenuItem']").parent().prepend("<img src='../../Images/plus.png' id='img" + $(this).find("input[type=hidden][id$='hdnAccessMenuID']").val() + "' onclick='expand(this, \"" + $(this).find("input[type=hidden][id$='hdnAccessMenuID']").val() + "\");' border='0' alt='Expand'/>");
            });
            
            $("table[class*='child']").each(function() {
                var level = $(this).find("input[type=hidden][id$='hdnDisplayLevel']").val();
                var space = "&nbsp;&nbsp;&nbsp;";
                
                for(var i=0; i<level; i++) {
                    space+=space;
                }
                    
                $(this).find("span[id$='lblAccessMenuItem']").parent().html(space + $(this).find("span[id$='lblAccessMenuItem']").parent().html());               
                
                $(this).hide();
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkBtnSaveInfo").click(function() {
                $("#dvLoader").show();
            });
        });
        
        function expand(ele, id) {
            ele.src = '../../Images/minus.png';
            $("table[class*='child" + id + "']").each(function() {
                $(this).show();
            });
            ele.onclick = function () { collapse(ele, id); return; };            
        }

        function collapse(ele, id) {            
            ele.src = '../../Images/plus.png';            
            $("table[class*='child" + id + "']").each(function() {
                var AccessMenuID = $(this).find("input[type=hidden][id$='hdnAccessMenuID']").val();
                if ($("#img" + AccessMenuID + "").attr("id") != null && typeof($("#img" + AccessMenuID + "").attr("id")) != "undefined") {                    
                    if ($("#img" + AccessMenuID + "").attr("onclick").toString().indexOf("expand") == -1) {
                        $("#img" + AccessMenuID + "").click();
                    }
                }
                $(this).hide();
            });
            ele.onclick = function () { expand(ele, id); return; };
        }    
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <mb:MenuUserControl ID="menucontrol" runat="server" />
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="tabcontent" id="menu_access" style="width: 100%;">
        <div class="form_pad" style="width: 90%;">
            <div style="text-align: center;">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div class="form_table">
                <div>
                    <h4>
                        Incentex Employee :&ensp;<b><asp:Label ID="lblUserFullName" runat="server"></asp:Label></b></h4>
                </div>                
                <div class="divider">
                </div>
                <div>
                    <table class="checktable_supplier true">
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 40%;">
                                            <h4>
                                                <b>Menu Items</b></h4>
                                        </td>
                                        <td style="width: 15%;">
                                            <h4>
                                                <b>View</b></h4>
                                        </td>
                                        <td style="width: 15%;">
                                            <h4>
                                                <b>Add</b></h4>
                                        </td>
                                        <td style="width: 15%;">
                                            <h4>
                                                <b>Edit</b></h4>
                                        </td>
                                        <td style="width: 15%;">
                                            <h4>
                                                <b>Delete</b></h4>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <div class="divider">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <asp:DataList ID="dtlRights" runat="server" OnItemDataBound="dtlRights_ItemDataBound">
                                    <HeaderTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 40%;">
                                                    <label style="font-size: 13px;">
                                                        All</label>
                                                </td>
                                                <td style="width: 15%;">
                                                        <asp:CheckBox ID="chkViewAll" ToolTip="Right to View All" CssClass="custom-checkbox chkHeader alignleft" runat="server" />
                                                </td>
                                                <td style="width: 15%;">
                                                    <asp:CheckBox ID="chkAddAll" ToolTip="Right to Add All" CssClass="custom-checkbox chkHeader alignleft" runat="server" />
                                                </td>
                                                <td style="width: 15%;">
                                                    <asp:CheckBox ID="chkEditAll" ToolTip="Right to Edit All" CssClass="custom-checkbox chkHeader alignleft" runat="server" />
                                                </td>
                                                <td style="width: 15%;">
                                                    <asp:CheckBox ID="chkDeleteAll" ToolTip="Right to Delete All" CssClass="custom-checkbox chkHeader alignleft"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <div class="divider">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table style="width: 100%;" id="tblMenus" runat="server">
                                            <tr>
                                                <td style="width: 40%;">
                                                    <label style="font-size: 13px;">
                                                        <asp:Label ID="lblAccessMenuItem" Text='<%# Eval("AccessMenuItem") %>' ToolTip="Menu Item"
                                                            runat="server"></asp:Label></label>
                                                    <asp:HiddenField ID="hdnAccessMenuID" Value='<%# Eval("AccessMenuID") %>' runat="server">
                                                    </asp:HiddenField>
                                                    <asp:HiddenField ID="hdnAccessRightID" Value='<%# Eval("AccessRightID") %>' runat="server">
                                                    </asp:HiddenField>
                                                    <asp:HiddenField ID="hdnHasChild" Value='<%# Eval("HasChild") %>' runat="server">
                                                    </asp:HiddenField>
                                                    <asp:HiddenField ID="hdnParentMenuID" Value='<%# Eval("ParentMenuID") %>' runat="server">
                                                    </asp:HiddenField>
                                                    <asp:HiddenField ID="hdnDisplayLevel" Value='<%# Eval("DisplayLevel") %>' runat="server">
                                                    </asp:HiddenField>
                                                    <asp:HiddenField ID="hdnOrderString" Value='<%# Eval("OrderString") %>' runat="server">
                                                    </asp:HiddenField>
                                                </td>
                                                <td style="width: 15%;">
                                                    <asp:CheckBox ID="chkView" CssClass="custom-checkbox alignleft chkLine" ToolTip="Right to View" runat="server"
                                                        Visible='<%# Eval("IsViewable") %>' Checked='<%# Eval("CanView") %>' />
                                                </td>
                                                <td style="width: 15%;">
                                                    <asp:CheckBox ID="chkAdd" CssClass="custom-checkbox alignleft chkLine" ToolTip="Right to Add" runat="server"
                                                        Visible='<%# Eval("IsAddable") %>' Checked='<%# Eval("CanAdd") %>' />
                                                </td>
                                                <td style="width: 15%;">
                                                    <asp:CheckBox ID="chkEdit" CssClass="custom-checkbox alignleft chkLine" ToolTip="Right to Edit" runat="server"
                                                        Visible='<%# Eval("IsEditable") %>' Checked='<%# Eval("CanEdit") %>' />
                                                </td>
                                                <td style="width: 15%;">
                                                    <asp:CheckBox ID="chkDelete" CssClass="custom-checkbox alignleft chkLine" ToolTip="Right to Delete" runat="server"
                                                        Visible='<%# Eval("IsDeleteable") %>' Checked='<%# Eval("CanDelete") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <div class="divider">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="spacer25">
            </div>
            <div class="botbtn centeralign">
                <asp:LinkButton ID="lnkBtnSaveAccessRights" class="grey2_btn" runat="server" ToolTip="Save Access Rights"
                    OnClick="lnkBtnSaveAccessRights_Click"><span>Save Access Rights</span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
