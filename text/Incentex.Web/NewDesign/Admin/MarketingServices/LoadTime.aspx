<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="LoadTime.aspx.cs" Inherits="Admin_LoadTime" Title="incentex | Load Time" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $(window).ValidationUI();
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(GetTriggeredElement);

            $("#<%= btnSearchErrorDate.ClientID %>").click(function() {
                $(".progress-layer").show();
            });

            if ($("#<%= ddlDate.ClientID %>").val() == 4) {
                $(".spanDate").css("display", "block");
            }

            if ($("#<%= ddlLoadTime.ClientID %>").val() == "Custom") {
                $(".spanLoadTime").css("display", "block");
            }

            $("#<%= ddlDate.ClientID %>").change(function() {
                if ($(this).val() == 4) {
                    $(".spanDate").css("display", "block");
                }
                else {
                    $(".spanDate").css("display", "none");
                    $("#<%= txtFromDate.ClientID %>").val("");
                    $("#<%= txtToDate.ClientID %>").val("");
                }
            });

            $("#<%= ddlLoadTime.ClientID %>").change(function() {
                if ($(this).val() == "Custom") {
                    $(".spanLoadTime").css("display", "block");
                }
                else {
                    $(".spanLoadTime").css("display", "none");
                    $("#<%= txtFormLoadTime.ClientID %>").val("");
                    $("#<%= txtToLoadTime.ClientID %>").val("");
                }
            });
        });

        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function GetTriggeredElement() {
            $(".default").uniform();
            //ScrollToTag("container", false);
            BindPopUpCloseEvents();
        }

        function ShowPopUp(MainDivTargetID, PopUpDivTargetID) {
            $("#" + MainDivTargetID).css('top', '0');
            $("#" + PopUpDivTargetID).show();
            $(".fade-layer").show();
            SetPopUpAtTop();
        }

        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="sc" runat="server">
    </asp:ScriptManager>
    <input type="hidden" value="admin-link" id="hdnActiveLink" />
    <asp:HiddenField ID="hdnBasicUserInfoID" runat="server" />
    <asp:UpdatePanel ID="UPGeneral" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearchErrorDate" />
        </Triggers>
        <ContentTemplate>
            <section id="container" class="cf GSE-page">
                <div class="narrowcolumn alignleft">                    
                    <div class="filter-block cf">
                        <div class="title-txt">
                            <span>Search</span><a href="#" onclick="GetHelpVideo('Marketing Services','Load Time')" title="Help video">Help video</a></div>
                        <div class="filter-form cf">
                        <ul class="cf">                            
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlCompany" runat="server" class="default " 
                                    onselectedindexchanged="ddlCompany_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ErrorMessage="Please select Company" Display="Dynamic" InitialValue="0" CssClass="error"
                                            ControlToValidate="ddlCompany" ValidationGroup="BasicTabValidate" SetFocusOnError="True"> </asp:RequiredFieldValidator>   
                                </span>
                            </li>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlWorkgroup" runat="server" class="default "
                                    onselectedindexchanged="ddlWorkgroup_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvWorkgroup" runat="server" ErrorMessage="Please select Workgroup" Display="Dynamic" InitialValue="0" CssClass="error"
                                            ControlToValidate="ddlWorkgroup" ValidationGroup="BasicTabValidate" SetFocusOnError="True"> </asp:RequiredFieldValidator>   
                                </span>
                            </li>                            
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlUser" runat="server" class="default ">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvUser" runat="server" ErrorMessage="Please select User" Display="Dynamic" InitialValue="0" CssClass="error"
                                            ControlToValidate="ddlUser" ValidationGroup="BasicTabValidate" SetFocusOnError="True"> </asp:RequiredFieldValidator>   
                                </span>
                            </li>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlModuleName" runat="server" class="default"
                                    onselectedindexchanged="ddlModuleName_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlSubMenu" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <%--<li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlPageLoading" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>--%>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlEventName" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlBrowser" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlDevice" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlDate" runat="server" class="default ">
                                        <asp:ListItem Text="- Date -" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Today" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Yesterday" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Last 7 days" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Custom" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvDate" runat="server" ErrorMessage="Please select Date" Display="Dynamic" InitialValue="0" CssClass="error"
                                            ControlToValidate="ddlDate" ValidationGroup="BasicTabValidate" SetFocusOnError="True"> </asp:RequiredFieldValidator>   
                                </span>
                            </li>
                            <li>
                                <span class="spanDate" style="display:none;">
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="input-field-all setDatePicker" placeholder="From Date"
                                        ToolTip="From Date"></asp:TextBox>
                                </span>
                            </li>
                            <li>
                                <span class="spanDate" style="display:none;">
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="input-field-all setDatePicker" placeholder="To Date"
                                        ToolTip="To Date"></asp:TextBox>
                                </span>
                            </li>                            
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlLoadTime" runat="server" class="default">
                                        <asp:ListItem Value="0">- Load Time -</asp:ListItem>
                                        <asp:ListItem Value="10 secs">10 secs</asp:ListItem>
                                        <asp:ListItem Value="<30 secs"><30 secs</asp:ListItem>
                                        <asp:ListItem Value="30-60 secs">30-60 secs</asp:ListItem>
                                        <asp:ListItem Value="1-3 mins">1-3 mins</asp:ListItem>
                                        <asp:ListItem Value="Custom">Custom</asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <span class="spanLoadTime" style="display:none;">
                                    <asp:TextBox ID="txtFormLoadTime" runat="server" CssClass="input-field-all" placeholder="From Time (in ms)"
                                        ToolTip="From Time (in sec)" onkeydown="return isNumber(event);"></asp:TextBox>
                                </span>
                            </li>
                            <li>
                                <span class="spanLoadTime" style="display:none;">
                                    <asp:TextBox ID="txtToLoadTime" runat="server" CssClass="input-field-all" placeholder="To Time (in ms)"
                                        ToolTip="To Time (in sec)" onkeydown="return isNumber(event);"></asp:TextBox>
                                </span>
                            </li>                                                                                    
                            <li>
                                <button id="btnSearchErrorDate" runat="server" class="blue-btn submit" onserverclick="btnSearchErrorDate_Click" ValidationGroup="BasicTabValidate" call="BasicTabValidate">Search</button>
                                <%--<asp:Button ID="btnSearchErrorDate" runat="server" class="blue-btn submit" ValidationGroup="BasicTabValidate" call="BasicTabValidate" Text="Search" OnClick="btnSearchErrorDate_Click"></asp:Button>--%>
                                
                            </li>                            
                        </ul>
                        </div>
                    </div>
                </div>
                <div class="widecolumn alignright">
                    <div class="filter-content" id="GseManagement">
                        <div class="filter-headbar cf">
                            <span class="headbar-title">Load Time</span> <em id="totalcount_em" runat="server" visible="false"></em>
                            <div class="filter-search">
                                <asp:TextBox ID="txtSearchGrid" runat="server" CssClass="input-field-small default_title_text"
                                    ToolTip="Search Results..." placeholder="Search Results..." MaxLength="100"></asp:TextBox>
                                <asp:LinkButton ID="btnSearchGrid" runat="server" CssClass="go-btn" OnClick="btnSearchGrid_Click"
                                    Enabled="false"> GO</asp:LinkButton></div>
                        </div>
                        <div class="gse-tablebox cf">
                            <asp:GridView ID="gvLoadTime" runat="server" AutoGenerateColumns="false" 
                                GridLines="None" OnRowCommand="gvLoadTime_RowCommand" 
                                CssClass="table-grid load-time-table" onrowdatabound="gvLoadTime_RowDataBound">
                                <Columns>      
                                    <asp:TemplateField SortExpression="CompanyName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCompanyName" runat="server" CommandArgument="CompanyName" CommandName="Sort"><span >Company</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCompanyName" Text='<%# Eval("CompanyName") %>'></asp:Label>
                                            <%--<asp:Label ID="lblSystemData" Text="System Data" runat="server" onclick="BindSystemDataGrid(this);"></asp:Label>--%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col3" />
                                        <ItemStyle CssClass="col3" />
                                    </asp:TemplateField>                              
                                    <asp:TemplateField SortExpression="FirstName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnFirstName" runat="server" CommandArgument="FirstName" CommandName="Sort"><span>User</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFirstName" Text='<%# Eval("FirstName") %>'></asp:Label>
                                            <%--<asp:Label ID="lblSystemData" Text="System Data" runat="server" onclick="BindSystemDataGrid(this);"></asp:Label>--%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4" />
                                        <ItemStyle CssClass="col4" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Workgroup">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnWorkgroup" runat="server" CommandArgument="Workgroup" CommandName="Sort"><span>Workgroup</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblWorkgroup" Text='<%# Eval("Workgroup") %>'></asp:Label>
                                            <%--<asp:Label ID="lblSystemData" Text="System Data" runat="server" onclick="BindSystemDataGrid(this);"></asp:Label>--%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4" />
                                        <ItemStyle CssClass="col4" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ModuleName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnModuleName" runat="server" CommandArgument="ModuleName"
                                                CommandName="Sort"><span>Module</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblModuleName" Text='<%# Eval("ModuleName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col1" />
                                        <ItemStyle CssClass="col1" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField SortExpression="SubMenu">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnSubMenu" runat="server" CommandArgument="SubMenu"
                                                CommandName="Sort"><span>Sub Menu</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblSubMenu" Text='<%# Eval("SubMenu") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col2"  />
                                        <ItemStyle CssClass="col2"  />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Details">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDetails" runat="server"  Text="Details"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnDetails" runat="server" CommandName="Details" 
                                                Text="Details" CommandArgument='<%# Eval("UserInfoID") + "#" + Eval("ModuleName") + "#" + Eval("SubMenu") + "#" + Eval("FirstName") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col6 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <ItemStyle CssClass="col6 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table class="table-grid">
        	                            <tr>
          	                            <th class="col1">Company</th>
                                        <th class="col2">User</th>
                                        <th class="col3">Workgroup</th>
                                        <th class="col4">Module</th>
                                        <th class="col5">Submenu</th>
                                        <th class="col6">Details</th>
                                      </tr>
                                      <tr>
          	                            <td colspan="6" style="text-align:center;vertical-align:middle;">Records not found</td>
                                      </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                        <div id="pagingtable" runat="server" class="store-footer cf" visible="false">
                    <a href="javascript:;" class="store-title">BACK TO TOP</a>
                    <asp:LinkButton ID="lnkViewAllBottom" runat="server" OnClick="lnkViewAll_Click" CssClass="pagination alignright view-link postback cf"> VIEW ALL </asp:LinkButton>
                    <div class="pagination alignright cf">
                        <span>
                            <asp:LinkButton ID="lnkPrevious" CssClass="left-arrow alignleft postback" runat="server"
                                OnClick="lnkPrevious_Click" ToolTip="Previous"> </asp:LinkButton>
                        </span>
                        <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPaging" runat="server" CommandArgument='<%# Eval("Index") %>'
                                    CommandName="ChangePage" Text='<%# Eval("Index") %>' CssClass="postback"> </asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:LinkButton ID="lnkNext" CssClass="right-arrow alignright postback" runat="server"
                            OnClick="lnkNext_Click" ToolTip="Next"> </asp:LinkButton>
                    </div>
                </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="system-data-popup" class="popup-outer popupouter-center">
        <div class="popupInner">
            <div class="specs-popup">
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Marketing Services','Load Time Details')">Help Video</a>
                <a class="close-btn" href="javascript:void(0);">Close</a>
                <div class="warranty-content">
                    <h2><asp:Label ID="lblHisotryTitle" runat="server">Details</asp:Label></h2>
                    <div class="clear"></div>
                    <div class="table-outer">
                    <asp:GridView ID="gvLTSubDetails" runat="server" AutoGenerateColumns="false" GridLines="None"
                        Visible="true" CssClass="table-grid" OnRowCommand="gvLTSubDetails_RowCommand" onrowdatabound="gvLTSubDetails_RowDataBound">
                        <Columns>
                            <asp:TemplateField SortExpression="CreatedDate">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbDate" runat="server" CommandArgument="CreatedDate"
                                        CommandName="Sort">Date</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col1" />
                                <ItemStyle CssClass="col1" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="PageName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbPageName" runat="server" CommandArgument="PageName"
                                        CommandName="Sort">Page Loading</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPageName" runat="server" Text='<%# Eval("PageName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col2 " />
                                <ItemStyle CssClass="col2" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="PageEvent">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbPageEvent" runat="server" CommandArgument="PageEvent"
                                        CommandName="Sort">Page Event</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPageEvent" runat="server" Text='<%# Eval("PageEvent") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col3" />
                                <ItemStyle CssClass="col3" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="LoadTime">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbLoadTime" runat="server" CommandArgument="LoadTime"
                                        CommandName="Sort">Load Time</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLoadTime" runat="server" Text='<%# Eval("LoadTime") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col4 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                                <ItemStyle CssClass="col4 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="BrowserName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbBrowserName" runat="server" CommandArgument="BrowserName"
                                        CommandName="Sort">Browsers</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBrowserName" runat="server" Text='<%# Eval("BrowserName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col5" />
                                <ItemStyle CssClass="col5" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="DeviceName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbDeviceName" runat="server" CommandArgument="DeviceName"
                                        CommandName="Sort">Device</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDeviceName" runat="server" Text='<%# Eval("DeviceName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col7" />
                                <ItemStyle CssClass="col7" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <table class="table-grid">
                                <tr>
                                    <th class="col1">
                                        Date
                                    </th>
                                    <th class="col2">
                                        Page Loading
                                    </th>
                                    <th class="col3">
                                        Page Event
                                    </th>
                                    <th class="col4">
                                        Load Time
                                    </th>
                                    <th class="col5">
                                        Browsers
                                    </th>
                                    <th class="col6">
                                        Device
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="6" style="text-align: center; vertical-align: middle;">
                                        Records not found
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    </div>
                </div>
                <div id="pagingTableDtl" runat="server" class="store-footer cf" visible="false">
                    <a href="javascript:;" class="store-title">BACK TO TOP</a>
                    
                    <asp:LinkButton ID="lnkViewAllDtl" runat="server" OnClick="lnkViewAllDtl_Click" CssClass="pagination alignright view-link postback cf"> VIEW ALL </asp:LinkButton>
                    <div class="pagination alignright cf">
                        <span>
                            <asp:LinkButton ID="lnkPreviousDtl" CssClass="left-arrow alignleft postback" runat="server"
                                OnClick="lnkPreviousDtl_Click" ToolTip="Previous"></asp:LinkButton>
                        </span>
                        <asp:DataList ID="dtlPagingDtl" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" OnItemCommand="dtlPagingDtl_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPaging" runat="server" CommandArgument='<%# Eval("Index") %>'
                                    CommandName="ChangePage" Text='<%# Eval("Index") %>' CssClass="postback"> </asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:LinkButton ID="lnkNextDtl" CssClass="right-arrow alignright postback" runat="server"
                            OnClick="lnkNextDtl_Click" ToolTip="Next"> </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
