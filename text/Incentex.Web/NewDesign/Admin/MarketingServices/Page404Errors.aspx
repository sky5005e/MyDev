<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="Page404Errors.aspx.cs" Inherits="Admin_Page404Errors" Title="incentex | 404 Errors" %>

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
                            <span>Search</span><a href="#" onclick="GetHelpVideo('Marketing Services','404 Errors')" title="Help video">Help video</a></div>
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
                                    <asp:DropDownList ID="ddlModuleName" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <button id="btnSearchErrorDate" runat="server" class="blue-btn" onserverclick="btnSearchErrorDate_Click">Search</button>
                            </li>
                        </ul>
                        </div>
                    </div>
                </div>
                <div class="widecolumn alignright">
                    <div class="filter-content" id="GseManagement">
                        <div class="filter-headbar cf">
                            <span class="headbar-title">404 Errors</span> <em id="totalcount_em" runat="server" visible="false"></em>
                            <div class="filter-search">
                                <asp:TextBox ID="txtSearchGrid" runat="server" CssClass="input-field-small default_title_text"
                                    ToolTip="Search Results..." placeholder="Search Results..." MaxLength="100"></asp:TextBox>
                                <asp:LinkButton ID="btnSearchGrid" runat="server" CssClass="go-btn" OnClick="btnSearchGrid_Click"
                                    Enabled="false"> GO</asp:LinkButton></div>
                        </div>
                        <div class="gse-tablebox cf">
                            <asp:GridView ID="gv404Errors" runat="server" AutoGenerateColumns="false" 
                                GridLines="None" OnRowCommand="gv404Errors_RowCommand" 
                                CssClass="table-grid error-table" onrowdatabound="gv404Errors_RowDataBound">
                                <Columns>
                                    <asp:TemplateField SortExpression="CompanyName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCompany" runat="server" CommandArgument="CompanyName"
                                                CommandName="Sort"><span>Company</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblCompany" Text='<%# Eval("CompanyName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col1"  />
                                        <ItemStyle CssClass="col1"  />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CustomerName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCustomerName" runat="server" CommandArgument="CustomerName"
                                                CommandName="Sort"><span>Customer Name</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblCustomerName" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col2" />
                                        <ItemStyle CssClass="col2" />
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField SortExpression="ErrorDate">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnDate" runat="server" CommandArgument="ErrorDate" CommandName="Sort"><span style="text-align:center" >Date</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                      
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("ErrorDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col3" />
                                        <ItemStyle CssClass="col3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ErrorTime">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnErrorTime" runat="server" CommandArgument="ErrorTime"
                                                CommandName="Sort"><span >Time</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblErrorTime" runat="server"  Text='<%# Eval("ErrorDate", "{0:HH:mm}")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4" />
                                        <ItemStyle CssClass="col4" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ErrorPageName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnErrorPageName" runat="server" CommandArgument="ErrorPageName"
                                                CommandName="Sort"><span >Page Name</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPageWithError" runat="server"  Text='<%# Eval("ErrorPageName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col5" />
                                        <ItemStyle CssClass="col5" />
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnNotes" runat="server" CommandArgument="Notes" CommandName="Sort"><span>Details</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfUserInfoId" runat="server" Value='<%# Eval("ErrorID")%>' />
                                            <asp:LinkButton ID="lnkNotes" runat="server" CommandArgument='<%# Eval("ErrorID") %>' CommandName="ShowNotes">Details</asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col6" />
                                        <ItemStyle CssClass="col6" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table class="table-grid">
        	                            <tr>
          	                            <th class="col1">Company</th>
                                        <th class="col2">Customer Name</th>
                                        <th class="col3">Date</th>                                        
                                        <th class="col4">Time</th>
                                        <th class="col5">Page Name</th>
                                        <th class="col6">Details</th>
                                      </tr>
                                      <tr>
          	                            <td colspan="5" style="text-align:center;vertical-align:middle;">Records not found</td>
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
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Marketing Services','Notes')">Help Video</a>
                <a class="close-btn" href="javascript:void(0);">Close</a>
                <div class="warranty-content">
                    <h2>
                        Details</h2>
                    <div>
                        <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="false" 
                                GridLines="None" 
                                CssClass="table-grid">
                                <Columns>
                                    <asp:TemplateField SortExpression="Resolution">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnResolution" runat="server" CommandArgument="Resolution"
                                                CommandName="Sort"><span>Resolution</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblResolution" Text='<%# Eval("Resolution") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col1"  />
                                        <ItemStyle CssClass="col1"  />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="OS">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnOS" runat="server" CommandArgument="OS"
                                                CommandName="Sort"><span>OS</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblOS" Text='<%# Eval("OS") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col2" />
                                        <ItemStyle CssClass="col2" />
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField SortExpression="Browser">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnBrowser" runat="server" CommandArgument="Browser" CommandName="Sort"><span style="text-align:center" >Browser</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                      
                                            <asp:Label ID="lblBrowser" runat="server" Text='<%# Eval("Browser") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col3" />
                                        <ItemStyle CssClass="col3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Notes">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnNotes" runat="server" CommandArgument="Notes" CommandName="Sort"><span style="text-align:center" >Notes</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                      
                                            <asp:Label ID="lblNotes" runat="server" Text='<%# Eval("Notes") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4" />
                                        <ItemStyle CssClass="col4" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table class="table-grid">
        	                            <tr>
          	                            <th class="col1">Resolution</th>
                                        <th class="col2">OS</th>
                                        <th class="col3">Browser</th>
                                        <th class="col4">Notes</th>
                                      </tr>
                                      <tr>
          	                            <td colspan="4" style="text-align:center;vertical-align:middle;">Records not found</td>
                                      </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        <%--<asp:Repeater ID="rptNotes" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                            
                            <div class="boxscroll">
                            <div class="viewnote-listbox">
                                <ul class="viewnote-list cf">
                                    <li class="customer-chat">
                                        <h5 class="cf">
                                            <span><%# Eval("FirstName") %></span><em><%# Eval("ErrorDate","{0:ddd, MMM dd, yyyy, hh:mm tt}") %></em></h5>
                                        <p>
                                            <%# Eval("Notes") %>
                                        </p>
                                    </li>
                                </ul>
                              </div>
                             </div>
                            </ItemTemplate>
                        </asp:Repeater>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
