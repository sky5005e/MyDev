<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewStation.aspx.cs" Inherits="admin_Company_Station_ViewStation" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
    function SelectAllCheckboxes(spanChk) {
        var IsChecked = spanChk.checked;
        var Chk = spanChk;
        Parent = Chk.form.elements;
        for (i = 0; i < Parent.length; i++) {
            if (Parent[i].type == "checkbox" && Parent[i].id != Chk.id) {
                if (Parent[i].checked != IsChecked)
                    Parent[i].click();
            }
        }
    }
    function SelectAllCheckboxesSpecific(spanChk) {
        var IsChecked = spanChk.checked;
        var Chk = spanChk;
        Parent = document.getElementById('ctl00_ContentPlaceHolder1_gv');
        var items = Parent.getElementsByTagName('input');
        for (i = 0; i < items.length; i++) {
            if (items[i].id != Chk && items[i].type == "checkbox") {
                if (items[i].checked != IsChecked) {
                    items[i].click();
                }
            }
        }
    }

    function SelectAllCheckboxesMoreSpecific(spanChk) {
        var IsChecked = spanChk.checked;
        var Chk = spanChk;
        Parent = document.getElementById('ctl00_ContentPlaceHolder1_gvIncentexEmployee');
        for (i = 0; i < Parent.rows.length; i++) {
            var tr = Parent.rows[i];
            //var td = tr.childNodes[0];			   		   
            var td = tr.firstChild;
            var item = td.firstChild;
            if (item.id != Chk && item.type == "checkbox") {
                if (item.checked != IsChecked) {
                    item.click();
                }
            }
        }
    }

/*
    function HighlightRow(chkB) {
        var IsChecked = chkB.checked;
        if (IsChecked) {
            chkB.parentElement.parentElement.style.backgroundColor = '#228b22';  // grdEmployees.SelectedItemStyle.BackColor
            chkB.parentElement.parentElement.style.color = 'white'; // grdEmployees.SelectedItemStyle.ForeColor
        }
        else {
            chkB.parentElement.parentElement.style.backgroundColor = 'white'; //grdEmployees.ItemStyle.BackColor
            chkB.parentElement.parentElement.style.color = 'black'; //grdEmployees.ItemStyle.ForeColor
        }
    }
    */
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <%--<mb:MenuUserControl ID="manuControl" runat="server" />--%>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div class="form_pad">
                <div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                    <br />
                </div>
                <div>
                    <asp:GridView ID="gv" runat="server" OnDataBinding="gv_DataBinding" AutoGenerateColumns="false"
                        HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                        RowStyle-CssClass="ord_content" OnRowDataBound="gv_RowDataBound" OnDataBound="gv_DataBound"
                        OnRowCommand="gv_RowCommand">
                        <Columns>
                            <asp:TemplateField Visible="false" HeaderText="Id">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblID" Text='<%# Eval("StationID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span>&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this);"
                                        runat="server" />
                                        &nbsp; </span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="first">&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkDelete" runat="server" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkStationCode" runat="server" CommandArgument="StationCode"
                                        CommandName="Sorting"><span >Station Code</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkEdit" runat="server">
                                        <asp:Label runat="server" ID="lblStationName" Text='<%# Eval("StationCode") %>' /></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkCompany" runat="server" CommandArgument="sCountryName" CommandName="Sorting"><span class="white_co">Country</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCountry" Text='<%# Eval("sCountryName") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Station Manager</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStationManager" />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span class="white_co">Station Admin</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStationAdmin" />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <div class="companylist_botbtn alignleft">
                        <asp:HyperLink ID="lnkAddStation" runat="server" class="grey_btn" NavigateUrl="~/MyAccount/Station/MainStationInfo.aspx">
						                <span>Add Station</span>
                        </asp:HyperLink>
                        <asp:LinkButton ID="lnkDelete" runat="server" class="grey_btn" OnClick="lnkDelete_Click"
                            OnClientClick="return confirm('Are you sure, you want to delete selected records ?')">
						                <span>Delete</span>
                        </asp:LinkButton>
                    </div>
                    <div class="alignright pagging">
                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                        </asp:LinkButton>
                        <span>
                            <asp:DataList ID="lstPaging" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="lstPaging_ItemCommand"
                                OnItemDataBound="lstPaging_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList></span>
                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
