<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewStation.aspx.cs" Inherits="admin_Company_Station_ViewStation" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
//    function SelectAllCheckboxes(spanChk) {
//        var IsChecked = spanChk.checked;
//        var Chk = spanChk;
//        Parent = Chk.form.elements;
//        for (i = 0; i < Parent.length; i++) {
//            if (Parent[i].type == "checkbox" && Parent[i].id != Chk.id) {
//                if (Parent[i].checked != IsChecked)
//                    Parent[i].click();
//            }
//        }
//    }
//    function SelectAllCheckboxesSpecific(spanChk) {
//        var IsChecked = spanChk.checked;
//        var Chk = spanChk;
//        Parent = document.getElementById('ctl00_ContentPlaceHolder1_gv');
//        var items = Parent.getElementsByTagName('input');
//        for (i = 0; i < items.length; i++) {
//            if (items[i].id != Chk && items[i].type == "checkbox") {
//                if (items[i].checked != IsChecked) {
//                    items[i].click();
//                }
//            }
//        }
//    }

//    function SelectAllCheckboxesMoreSpecific(spanChk) {
//        var IsChecked = spanChk.checked;
//        var Chk = spanChk;
//        Parent = document.getElementById('ctl00_ContentPlaceHolder1_gvIncentexEmployee');
//        for (i = 0; i < Parent.rows.length; i++) {
//            var tr = Parent.rows[i];
//            //var td = tr.childNodes[0];			   		   
//            var td = tr.firstChild;
//            var item = td.firstChild;
//            if (item.id != Chk && item.type == "checkbox") {
//                if (item.checked != IsChecked) {
//                    item.click();
//                }
//            }
//        }
//    }

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
    <mb:MenuUserControl ID="manuControl" runat="server" />
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
                                    <span class="custom-checkbox gridheader">
                                        <asp:CheckBox ID="chkAll" runat="server" />
                                        &nbsp; </span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                 <HeaderStyle CssClass="centeralign" />
                                <ItemTemplate>
                                   <span class="first custom-checkbox gridcontent"><asp:CheckBox ID="chkDelete" runat="server" /></span>
                                   <div class="corner">
                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                        </div>
                                </ItemTemplate>
                                <ItemStyle Width="5%" CssClass="b_box centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <%--<span>Station Code</span>--%>
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
                                    <%--<span class="white_co">Country</span>--%>
                                    <asp:LinkButton ID="lnkCompany" runat="server" CommandArgument="sCountryName" CommandName="Sorting"><span class="white_co">Country</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCountry" Text='<% # (Convert.ToString(Eval("sCountryName")).Length > 15) ? Eval("sCountryName").ToString().Substring(0,15)+"..." : Convert.ToString(Eval("sCountryName"))+ "&nbsp;"  %>'
                                        ToolTip='<% #Eval("sCountryName")  %>'></asp:Label>
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
                            <asp:TemplateField SortExpression="Active">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnActive" runat="server" CommandArgument="Active" CommandName="Sort"><span>Station Status</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderActive" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnLookupIcon" runat="server" />
                                    <asp:HiddenField ID="hdnStatusID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Active")%>' />
                                    <%--<asp:Label ID="lblStatus" runat="server" Text="" class="btn_space"><img id="imgLookupIcon" style="height:20px;width:20px"  runat="server" alt='Loading' /></asp:Label>--%>
                                    <asp:LinkButton ID="lblStatus" runat="server" Text="" CommandName="StatusVhange"
                                        class="btn_space">
                                        <span class="btn_space">
                                            <img id="imgLookupIcon" style="height: 20px; width: 20px" runat="server" alt='Loading' /></span></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField >
                                    <HeaderTemplate>
                                        <span class="white_co">Action</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkEdit1" runat="server" Text="Edit" ></asp:HyperLink>
                                    </ItemTemplate>
                                 </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <div class="companylist_botbtn alignleft">
                        <asp:HyperLink ID="lnkAddStation" runat="server" class="grey_btn" NavigateUrl="~/admin/Company/Station/MainStationInfo.aspx">
						                <span>Add Station</span>
                        </asp:HyperLink>
                        <asp:LinkButton ID="lnkDelete" runat="server" class="grey_btn" OnClick="lnkDelete_Click"
                            OnClientClick="return confirm('Are you sure, you want to delete selected records ?')">
						                <span>Delete</span>
                        </asp:LinkButton>
                    </div>
                    <%--<div class="alignright pagging">
					       <a href="#" class="prevb">Prev</a><a href="#">1</a><a 
href="#">2</a><a href="#">3</a><a href="#">4</a><a href="#">5</a><a href="#"    class="nextb">Next</a>
					        </div>--%>
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
