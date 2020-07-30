<%@ Page Title="incentex | Pending Users" Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true" CodeFile="ViewPendingUsers.aspx.cs" Inherits="MyAccount_ViewPendingUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        $().ready(function() {
            $('input').iCheck({
                checkboxClass: 'icheckbox_flat'
                //radioClass: 'iradio_flat'
            });
            $(window).ValidationUI();
        });
        // Select All checkbox
        function changeAll() {
            var parent = $("#<%= gvPendingUsers.ClientID %>").attr("id");
            var checkBoxes = $("#" + parent + " :input");
            for (var i = 0; i < checkBoxes.length; i++)
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].id.indexOf("chkSelectUser") >= 0)
                $("#" + checkBoxes[i].id).iCheck('check');
        }

//            $('.table-headbtn a').click(function() {
//                var _title = $(this).attr("title");
//                var strfor = _title;
//                if (_title == "Approve Selected") {
//                    _title = 'To approve the order(s) you selected, please click "yes" to confirm.';
//                }
//                else {
//                    _title = 'Are you sure you want to ' + _title + '?';
//                }
//                DisplayConfirmation(_title, strfor);
//            });
        

//        function DisplayConfirmation(_msg,strfor) {
//            $("#Confirmation-popup").css('top', '0');
//            $(".fade-layer").show();
//            $("#dvConfirmationMsg").show();
//            $("#pdMsg").html(_msg);
//            $("#ctl00_ContentPlaceHolder1_hdnMainStatus").val(strfor);
//        }

//        function CloseConfirmation() {
//            $("#Confirmation-popup").css('top', '-9999px');
//            $(".fade-layer").hide();
//            $("#dvConfirmationMsg").hide();
//            $("#pdMsg").html('');
//            $("#ctl00_ContentPlaceHolder1_hdnMainStatus").val('');
//        }
        //End
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section id="container" class="cf pendingOrder-page">
	<div class="pending-order-block">
  	<h5 class="filter-headbar cf"><span class="headbar-title">Pending Employees</span> <span class="watch-video-org">
  	<a href="javascript: void(0);" class="video-btn popup-openlink" title="Watch Help video" onclick="GetHelpVideo('Pending Employees','Pending Employees')"></a></span></h5>
		<div class="table-headbtn cf">
			<a  href="javascript:;" class="table-btn alignleft" title="Select All" onclick="changeAll();">Select All</a>
			<a  href="javascript:;" class="table-btn alignleft" title="Approve All" >Approve All</a>
			<a href="javascript:;" class="table-btn del-button alignleft" title="Approve All With Same Password" >Approve All With Same Password</a>
			<asp:Label ID="lblPendingCount" runat="server" class="pending-ordr"></asp:Label>
		</div>
  	
       <asp:GridView ID="gvPendingUsers" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowDataBound="gvPendingUsers_RowDataBound" OnRowCommand="gvPendingUsers_RowCommand" CssClass="table-grid">
                    <Columns>
                       <asp:TemplateField Visible="false" HeaderText="Id">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblRegistraionID" Text='<%# Eval("iRegistraionID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField>
                            <HeaderTemplate>&nbsp;
                            </HeaderTemplate>
                            <ItemTemplate>
                            <label class="label_checkbox"><asp:CheckBox CssClass="icheckbox_flat" ID="chkSelectUser" runat="server" />&nbsp; </label>                               
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField SortExpression="CompanyName">
                            <HeaderTemplate >
                                 <asp:LinkButton ID="lnkCompanyName" runat="server" CommandArgument="CompanyName"
                                    CommandName="Sorting">Company Name</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompanyName" Text='<%# Eval("CompanyName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField SortExpression="EmployeeName">
                            <HeaderTemplate>
                               <asp:LinkButton ID="lnkEmployeeName" runat="server" CommandArgument="EmployeeName"
                                    CommandName="Sorting">Employee Name</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblEmployeeName" Text='<%#Eval("EmployeeName")%>'/>
                            </ItemTemplate>
                             </asp:TemplateField>
                       <asp:TemplateField SortExpression="WorkGroup">
                            <HeaderTemplate>
                               <asp:LinkButton ID="lnkWorkGroup" runat="server" CommandArgument="WorkGroup"
                                    CommandName="Sorting">WorkGroup</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWorkGroup" Text='<%#Eval("WorkGroup")%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField SortExpression="BaseStationName">
                            <HeaderTemplate>
                               <asp:LinkButton ID="lnkBaseStation" runat="server" CommandArgument="BaseStationName"
                                    CommandName="Sorting">Station</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblBaseStationName" Text='<%# Convert.ToString(Eval("BaseStation")).Substring(0, 3)%>' />
                            </ItemTemplate>
                             <HeaderStyle CssClass="textcenter" />
                            <ItemStyle CssClass="textcenter" />
                        </asp:TemplateField>                      
                       <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Actions</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                            <div class="pending-btn">
                            <a href="javascript:ShowUserBasicInfo('<%# Eval("iRegistraionID") %>');" class="gray-button-view royaledit-btn" title="EDIT">View Profile</a>
                            </div>
                            </ItemTemplate>
                            <HeaderStyle CssClass="last last-action textcenter" />
                            <ItemStyle CssClass="last last-action textcenter" />
                        </asp:TemplateField>
                    </Columns>
               </asp:GridView>
        <div id="pagingtable"  runat="server" class="store-header cf">
                    <asp:LinkButton ID="lnkViewAllTop" runat="server" OnClick="lnkViewAll_Click" class="view-link">VIEW ALL</asp:LinkButton>
                    <%--START Top paging--%>
                    <div class="pagination alignright">
                        <asp:LinkButton ID="lnkbtnPrevious" runat="server" OnClick="lnkbtnPrevious_Click"
                            class="left-arrow"></asp:LinkButton>
                        <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>' OnClientClick="SetPageIndexActive(this);"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:LinkButton ID="lnkbtnNext" runat="server" OnClick="lnkbtnNext_Click" class="right-arrow"></asp:LinkButton>
                    </div>
                    <%--END Top paging--%>
                </div>
  </div>
   
    <%--Start Popup Help Video  --%>
    <div class="popup-outer" id="video-block">
        <div class="popupInner">
            <div class="video-block">
                <a href="javascript:;"  onclick="CloseVideo();" class="hide-popup">Close</a>
                 <div class="video-player">
                     <iframe id="iframeVideo" runat="server" width="900" height="517" frameborder="0">
                        </iframe>
                </div>
                </div>
        </div>
    </div>
    <%--END Popup Help Video  --%>
    
    <%--  Hidden Field--%>
</section>
</asp:Content>

