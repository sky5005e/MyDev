<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="SystemAccess.aspx.cs" Inherits="Admin_SystemAccess" Title="incentex | System Access" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $(window).ValidationUI();
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(GetTriggeredElement);

            $("#<%= btnSearchAccessData.ClientID %>").click(function() {
                $(".progress-layer").show();
            });

            if ($("#<%= ddlDate.ClientID %>").val() == 4) {
                $(".spanDate").css("display", "block");
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
        });

        function BindSystemDataGrid(obj) {
            var userid = $(obj).prev().val();
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "SAService.asmx/GetSystemData",
                contentType: "application/json; charset=utf-8",
                data: '{ "UserInfoID" : "' + userid + '" }',
                async: false,
                success: function(data) {

                },
                error: function(err, msg) {
                    alert("Error : " + msg);
                }
            });
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
            <asp:PostBackTrigger ControlID="btnSearchAccessData" />
        </Triggers>
        <ContentTemplate>
            <section id="container" class="cf GSE-page">
                <div class="narrowcolumn alignleft">                    
                    <div class="filter-block cf">
                        <div class="title-txt">
                            <span>Search</span><a href="#" title="Help video" onclick="GetHelpVideo('Marketing Services','System Access')">Help video</a></div>
                        <div class="filter-form cf">
                        <ul class="cf">
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlDate" runat="server" class="default">
                                        <asp:ListItem Text="- Date -" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Today" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Yesterday" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Last 7 days" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Custom" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <span class="spanDate" style="display:none;">
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="input-field-all setDatePicker" placeholder="From Date"
                                        ToolTip="From Date" tabindex="1"></asp:TextBox>
                                </span>
                            </li>
                            <li>
                                <span class="spanDate" style="display:none;">
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="input-field-all setDatePicker" placeholder="To Date"
                                        ToolTip="To Date" tabindex="1"></asp:TextBox>
                                </span>
                            </li>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlUser" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlCompany" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlWorkgroup" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlStation" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <button id="btnSearchAccessData" runat="server" class="blue-btn" onserverclick="btnSearchAccessData_Click">Search</button>
                            </li>
                        </ul>
                        </div>
                    </div>
                </div>
                <div class="widecolumn alignright">
                    <div class="filter-content" id="GseManagement">
                        <div class="filter-headbar cf">
                            <span class="headbar-title">System Access</span> <em id="totalcount_em" runat="server" visible="false"></em>
                            <div class="filter-search">
                                <asp:TextBox ID="txtSearchGrid" runat="server" CssClass="input-field-small default_title_text"
                                    ToolTip="Search Results..." placeholder="Search Results..." MaxLength="100"></asp:TextBox>
                                <asp:LinkButton ID="btnSearchGrid" runat="server" CssClass="go-btn" OnClick="btnSearchGrid_Click"
                                    Enabled="false"> GO</asp:LinkButton></div>
                        </div>
                        <div class="gse-tablebox cf">
                            <asp:GridView ID="gvSystemAccess" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowCommand="gvSystemAccess_RowCommand" OnRowDataBound="gvSystemAccess_RowDataBound" CssClass="table-grid">
                                <Columns>
                                    <asp:TemplateField SortExpression="CompanyName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCompanyName" runat="server" CommandArgument="CompanyName" CommandName="Sort"><span style="text-align:center" >Company</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                      
                                            <asp:Label ID="lblCompanyName" runat="server" Text='<%# Eval("CompanyName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col1" />
                                        <ItemStyle CssClass="col1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="FirstName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnUserName" runat="server" CommandArgument="FirstName" CommandName="Sort"><span style="text-align:center" >User Name</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                      
                                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col1" />
                                        <ItemStyle CssClass="col1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="NoOfAccess">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnNoOfAccesses" runat="server" CommandArgument="NoOfAccess"
                                                CommandName="Sort"><span ># of Accesses</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNoOfAccess" runat="server"  Text='<%# Eval("NoOfAccess")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col2 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <ItemStyle CssClass="col2 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="OrderPlaced">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnOrderPlaced" runat="server" CommandArgument="OrderPlaced"
                                                CommandName="Sort"><span>Order Placed</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblOrderPlaced" Text='<%# Eval("OrderPlaced") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col3 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <ItemStyle CssClass="col3 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblSystemData" runat="server" Text="Details"></asp:Label>
                                            <%--<asp:LinkButton ID="lnkbtnSystemData" runat="server" CommandArgument="Status" CommandName="Sort"><span >Details</span></asp:LinkButton>--%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfUserInfoId" runat="server" Value='<%# Eval("UserInfoId")%>' />
                                            <asp:LinkButton ID="lnkSystemData" runat="server" CommandArgument='<%# Eval("UserInfoId") + "," + Eval("FirstName") %>' CommandName="ShowSystemDataDetails">Details</asp:LinkButton>
                                            <%--<asp:Label ID="lblSystemData" Text="System Data" runat="server" onclick="BindSystemDataGrid(this);"></asp:Label>--%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4 text-center" />
                                        <ItemStyle CssClass="col4 text-center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table class="table-grid">
        	                            <tr>
          	                            <th class="col1">User Name</th>
                                        <th class="col2"># of Accesses</th>
                                        <th class="col3">Order Placed</th>
                                        <th class="col4">Details</th>
                                      </tr>
                                      <tr>
          	                            <td colspan="4" style="text-align:center;vertical-align:middle;">Records not found</td>
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
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Marketing Services','History')">Help Video</a>
                <a class="close-btn" href="javascript:void(0);">Close</a>
                <div class="warranty-content">
                    <h2><asp:Label ID="lblHisotryTitle" runat="server"></asp:Label></h2>
                    <div class="clear"></div>
                    <div class="table-outer">
                    <asp:GridView ID="gvSystemData" runat="server" AutoGenerateColumns="false" GridLines="None"
                        Visible="true" CssClass="table-grid" OnRowCommand="gvSystemData_RowCommand">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <%--<asp:LinkButton ID="lbSessionHeader" runat="server" CommandArgument="Session" CommandName="Sort">Session</asp:LinkButton>--%>
                                    <asp:Label ID="lblSessionHeader" runat="server">Session</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSession" runat="server" Text='<%# Eval("Session") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col1 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                                <ItemStyle CssClass="col1 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbLoginDateHeader" runat="server" CommandArgument="LoginDate"
                                        CommandName="Sort">Login Date</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLoginDate" runat="server" Text='<%# Eval("LoginDate","{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col2 postback" />
                                <ItemStyle CssClass="col2" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbLoginTimeHeader" runat="server" CommandArgument="LoginTime"
                                        CommandName="Sort">Login Time</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLoginTime" runat="server" Text='<%# Eval("LoginDate","{0:HH:mm}") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col3 postback" />
                                <ItemStyle CssClass="col3" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbSessionLengthHeader" runat="server" CommandArgument="SessionLength"
                                        CommandName="Sort">Session Length</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLoginTime" runat="server" Text='<%# Eval("SessionLength") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col4 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                                <ItemStyle CssClass="col4 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbBrowsersHeader" runat="server" CommandArgument="BrowserName"
                                        CommandName="Sort">Browsers</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBrowserName" runat="server" Text='<%# Eval("BrowserName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col5" />
                                <ItemStyle CssClass="col5 postback" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbOSHeader" runat="server" CommandArgument="OS" CommandName="Sort">OS</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOS" runat="server" Text='<%# Eval("OS") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col6" />
                                <ItemStyle CssClass="col6 postback" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbResolutionHeader" runat="server" CommandArgument="Resolution"
                                        CommandName="Sort">Resolution</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblResolution" runat="server" Text='<%# Eval("Resolution") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col7" />
                                <ItemStyle CssClass="col7 postback" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <table class="table-grid">
                                <tr>
                                    <th class="col1">
                                        Session
                                    </th>
                                    <th class="col2">
                                        Login Date
                                    </th>
                                    <th class="col3">
                                        Login Time
                                    </th>
                                    <th class="col4">
                                        Session Length
                                    </th>
                                    <th class="col5">
                                        Browsers
                                    </th>
                                    <th class="col6">
                                        OS
                                    </th>
                                    <th class="col7">
                                        Resolution
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align: center; vertical-align: middle;">
                                        Records not found
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    </div>
                </div>
                <div id="pagingTableSD" runat="server" class="store-footer cf" visible="false">
                    <a href="javascript:;" class="store-title">BACK TO TOP</a>
                    
                    <asp:LinkButton ID="lnkViewAllSD" runat="server" OnClick="lnkViewAllSD_Click" CssClass="pagination alignright view-link postback cf"> VIEW ALL </asp:LinkButton>
                    <div class="pagination alignright cf">
                        <span>
                            <asp:LinkButton ID="lnkPreviousSD" CssClass="left-arrow alignleft postback" runat="server"
                                OnClick="lnkPreviousSD_Click" ToolTip="Previous"> </asp:LinkButton>
                        </span>
                        <asp:DataList ID="dtlPagingSD" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" OnItemCommand="dtlPagingSD_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPaging" runat="server" CommandArgument='<%# Eval("Index") %>'
                                    CommandName="ChangePage" Text='<%# Eval("Index") %>' CssClass="postback"> </asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:LinkButton ID="lnkNextSD" CssClass="right-arrow alignright postback" runat="server"
                            OnClick="lnkNextSD_Click" ToolTip="Next"> </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
