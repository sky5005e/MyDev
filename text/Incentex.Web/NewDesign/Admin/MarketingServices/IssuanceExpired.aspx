<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="IssuanceExpired.aspx.cs" Inherits="Admin_IssuanceExpired" Title="incentex | Issuance Expired" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $(window).ValidationUI();
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(GetTriggeredElement);

            $("#<%= btnSearchIssuanceData.ClientID %>").click(function() {
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
            <asp:PostBackTrigger ControlID="btnSearchIssuanceData" />
        </Triggers>
        <ContentTemplate>
            <section id="container" class="cf GSE-page">
                <div class="narrowcolumn alignleft">                    
                    <div class="filter-block cf">
                        <div class="title-txt">
                            <span>Search</span><a href="#" title="Help video" onclick="GetHelpVideo('Marketing Services','Issuance Expired')">Help video</a></div>
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
                                <button id="btnSearchIssuanceData" runat="server" class="blue-btn" onserverclick="btnSearchIssuanceData_Click">Search</button>
                            </li>
                        </ul>
                        </div>
                    </div>
                </div>
                <div class="widecolumn alignright">
                    <div class="filter-content" id="GseManagement">
                        <div class="filter-headbar cf">
                            <span class="headbar-title">Issuance Expired</span> <em id="totalcount_em" runat="server" visible="false"></em>
                            <div class="filter-search">
                                <asp:TextBox ID="txtSearchGrid" runat="server" CssClass="input-field-small default_title_text"
                                    ToolTip="Search Results..." placeholder="Search Results..." MaxLength="100"></asp:TextBox>
                                <asp:LinkButton ID="btnSearchGrid" runat="server" CssClass="go-btn" OnClick="btnSearchGrid_Click"
                                    Enabled="false"> GO</asp:LinkButton></div>
                        </div>
                        <div class="gse-tablebox cf">
                            <asp:GridView ID="gvIssuanceExpired" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowCommand="gvIssuanceExpired_RowCommand" CssClass="table-grid">
                                <Columns>
                                    <asp:TemplateField SortExpression="ExpirationDate">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnExpirationDate" runat="server" CommandArgument="ExpirationDate" CommandName="Sort"><span style="text-align:center" >Expiration Date</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                      
                                            <asp:Label ID="lblExpirationDate" runat="server" Text='<%# Eval("ExpirationDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col1" Width="115px" />
                                        <ItemStyle CssClass="col1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="FirstName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnEmployeeName" runat="server" CommandArgument="FirstName"
                                                CommandName="Sort"><span >Employee Name</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEmployeeName" runat="server" CommandArgument='<%# Eval("UserInfoID") + "-" + Eval("CompanyID") %>' CommandName="FirstName" Text='<%# Eval("FirstName") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col2" Width="200px" />
                                        <ItemStyle CssClass="col2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="IssuanceProgramName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnIssuanceName" runat="server" CommandArgument="IssuanceProgramName"
                                                CommandName="Sort"><span>Issuance Name</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:HiddenField ID="hfUserInfoId" runat="server" Value='<%# Eval("UserInfoId")%>' />
                                            <asp:HiddenField ID="hfUIPolicyID" runat="server" Value='<%# Eval("UniformIssuancePolicyID")%>' />
                                            <asp:LinkButton ID="lnkIssuanceName" runat="server" CommandArgument='<%# Eval("UserInfoId") + "-" + Eval("CompanyId") %>' CommandName="IssuanceProgramName" Text='<%# Eval("IssuanceProgramName") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col3" />
                                        <ItemStyle CssClass="col3" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table class="table-grid">
        	                            <tr>
          	                                <th class="col1">Expiration Date</th>
                                            <th class="col2">Employee Name</th>
                                            <th class="col3">Issuance Name</th>
                                        </tr>
                                        <tr>
              	                            <td colspan="3" style="text-align:center;vertical-align:middle;">Records not found</td>
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
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);">Help Video</a>
                <a class="close-btn" href="javascript:void(0);">Close</a>
                <div class="warranty-content">
                    <span id="spancIP">                        
                        <h1>Under construction...</h1>
                    </span>
                </div>                
            </div>
        </div>
    </div>
</asp:Content>
