<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="SessionRecordings.aspx.cs" Inherits="Admin_SessionRecordings" Title="incentex | Session Recordings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $(window).ValidationUI();
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(GetTriggeredElement);

            $("#<%= btnSearchSessionRecording.ClientID %>").click(function() {
                if (Page_ClientValidate("BasicTabValidate")) {
                    $(".progress-layer").show();
                }
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

        function PlaySession(siteid) {
            window.open(siteid, 'playvideo', 'width=420,height=500 ,scrollbars=yes');
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
            <asp:PostBackTrigger ControlID="btnSearchSessionRecording" />
        </Triggers>
        <ContentTemplate>
            <section id="container" class="cf GSE-page">
                <div class="narrowcolumn alignleft">                    
                    <div class="filter-block cf">
                        <div class="title-txt">
                            <span>Search</span><a href="#" onclick="GetHelpVideo('Marketing Services','Session Recordings')" title="Help video">Help video</a></div>
                        <div class="filter-form cf">
                        <ul class="cf">                                                        
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlCompany" runat="server" class="default checkvalidation" >
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlUser" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <%--<li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlWorkgroup" runat="server" class="default checkvalidation">
                                    </asp:DropDownList>
                                </span>
                            </li>                            
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlStation" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>--%>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlDate" runat="server" class="default checkvalidation">
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
                                <asp:Button ID="btnSearchSessionRecording" runat="server" class="blue-btn submit" Text="Search" OnClick="btnSearchSessionRecording_Click" ValidationGroup="BasicTabValidate" call="BasicTabValidate"></asp:Button>
                            </li>                            
                        </ul>
                        </div>
                    </div>
                </div>
                <div class="widecolumn alignright">
                    <div class="filter-content" id="GseManagement">
                        <div class="filter-headbar cf">
                            <span class="headbar-title">Session Recordings</span> <em id="totalcount_em" runat="server" visible="false"></em>
                            <div class="filter-search">
                                <asp:TextBox ID="txtSearchGrid" runat="server" CssClass="input-field-small default_title_text"
                                    ToolTip="Search Results..." placeholder="Search Results..." MaxLength="100"></asp:TextBox>
                                <asp:LinkButton ID="btnSearchGrid" runat="server" CssClass="go-btn" OnClick="btnSearchGrid_Click"
                                    Enabled="false"> GO</asp:LinkButton></div>
                        </div>
                        <div class="gse-tablebox cf">
                            <asp:GridView ID="gvSessionRecoding" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowCommand="gvSessionRecoding_RowCommand" CssClass="table-grid">
                                <Columns>                                    
                                    <asp:TemplateField SortExpression="CountryCode">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCountryCode" runat="server" CommandArgument="CountryCode"
                                                CommandName="Sort"><span>Country</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Image ID="img" ImageUrl='<%# Eval("ImageSrc") %>' border="0" title='<%# Eval("CountryCode")%>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col1" />
                                        <ItemStyle CssClass="col1" />
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField SortExpression="CompanyName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCompanyName" runat="server" CommandArgument="CompanyName"
                                                CommandName="Sort"><span>Company</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblCompanyName" Text='<%# Eval("CompanyName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col2"  />
                                        <ItemStyle CssClass="col2"  />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="UserName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnUserName" runat="server" CommandArgument="UserName" CommandName="Sort"><span >User</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblUserName" Text='<%# Eval("UserName") %>'></asp:Label>
                                            <%--<asp:Label ID="lblSystemData" Text="System Data" runat="server" onclick="BindSystemDataGrid(this);"></asp:Label>--%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col3" />
                                        <ItemStyle CssClass="col3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="VisitLength">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnVisitLength" runat="server" CommandArgument="VisitLength" CommandName="Sort"><span >Duration</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblVisitLength" Text='<%# Eval("VisitLength") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4" />
                                        <ItemStyle CssClass="col4" />
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPlay" Text="Play" runat="server"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <a id="lnkPlayUrl" runat="server" onclick="javascript:PlaySession(this.title);" title='<%# Eval("PopUrl")%>'>
                                                <asp:Image ID="imgPlay" ImageUrl="~/admin/Incentex_Used_Icons/play_16x16.png" Width="16"
                                                    Height="16" border="0" title="Play" runat="server" />
                                                </a>
                                            </span>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col7" />
                                        <ItemStyle CssClass="col7" />                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label runat="server" ID="lnkbtnDetails" Text="Details"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDetails" runat="server" CommandName="Details" CommandArgument='<%# Eval("Id") %>'>Details</asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4" />
                                        <ItemStyle CssClass="col4" />
                                    </asp:TemplateField>  
                                </Columns>
                                <EmptyDataTemplate>
                                    <table class="table-grid">
        	                            <tr>
          	                            <th class="col1">Courtry</th>
                                        <th class="col2">Company</th>
                                        <th class="col3">User</th>
                                        <th class="col4">Duration</th>
                                        <th class="col5">Play</th>
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
    <div id="session-details-popup" class="popup-outer popupouter-center">
        <div class="popupInner">
            <div class="specs-popup">
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Marketing Services','Session Details')">Help Video</a>
                <a class="close-btn" href="javascript:void(0);">Close</a>
                <div class="warranty-content">
                    <h2><asp:Label ID="lblHisotryTitle" runat="server"></asp:Label></h2>
                    <div class="clear"></div>
                    <div class="table-outer">
                    <asp:GridView ID="gvSessionDetails" runat="server" AutoGenerateColumns="false" GridLines="None"
                        Visible="true" CssClass="table-grid">
                        <Columns>
                            <asp:TemplateField SortExpression="Browser">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnBrowser" runat="server" CommandArgument="Browser" CommandName="Sort"><span style="text-align:center" >Browser</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>                      
                                    <asp:Label ID="lblBrowser" runat="server" Text='<%# Eval("Browser") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col5" />
                                <ItemStyle CssClass="col5" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="OS">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnOS" runat="server" CommandArgument="OS"
                                        CommandName="Sort"><span >OS</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOS" runat="server"  Text='<%# Eval("OS")%>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col6" />
                                <ItemStyle CssClass="col6" />
                            </asp:TemplateField>                     
                        </Columns>
                        <EmptyDataTemplate>
                            <table class="table-grid">
                                <tr>
                                    <th class="col1">
                                        Browser
                                    </th>
                                    <th class="col2">
                                        OS
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center; vertical-align: middle;">
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
                    
                    <asp:LinkButton ID="lnkViewAllSD" runat="server" CssClass="pagination alignright view-link postback cf"> VIEW ALL </asp:LinkButton>
                    <div class="pagination alignright cf">
                        <span>
                            <asp:LinkButton ID="lnkPreviousSD" CssClass="left-arrow alignleft postback" runat="server"
                                ToolTip="Previous"> </asp:LinkButton>
                        </span>
                        <asp:DataList ID="dtlPagingSD" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPaging" runat="server" CommandArgument='<%# Eval("Index") %>'
                                    CommandName="ChangePage" Text='<%# Eval("Index") %>' CssClass="postback"> </asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:LinkButton ID="lnkNextSD" CssClass="right-arrow alignright postback" runat="server"
                            ToolTip="Next"> </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
