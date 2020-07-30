<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="AbandonedCarts.aspx.cs" Inherits="Admin_AbandonedCarts" Title="incentex | Abandoned Carts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $(window).ValidationUI();
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(GetTriggeredElement);

            $("#<%= btnSearchAbdnCartDate.ClientID %>, #<%= btnSend.ClientID %>").click(function() {
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
        
        function CheckAllCheckbox(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=gvAbandonedCart.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length - 1; i++) {
                var currclass = GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("div")[0].className;
                if (Checkbox.checked) {
                    GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("div")[0].className = "icheckbox_flat";
                }
                else {
                    GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("div")[0].className = "icheckbox_flat checked";
                }
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[1].checked = !Checkbox.checked;
            }
        }

        function CheckForAllCheckBox(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=gvAbandonedCart.ClientID %>");
            var IsAllChecked = true;
            var UnCheckedTot = 0;
            for (i = 1; i < GridVwHeaderChckbox.rows.length - 1; i++) {
                if (GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("div")[0].className == "icheckbox_flat" || GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("div")[0].className == "icheckbox_flat hover") {
                    IsAllChecked = false;
                    UnCheckedTot++;
                }
            }
            
            if (!IsAllChecked && UnCheckedTot == 1 && !Checkbox.checked) {
                IsAllChecked = true;
            }
            else if (IsAllChecked && Checkbox.checked) {
                IsAllChecked = false;
            }
            
            if (!IsAllChecked) {
                GridVwHeaderChckbox.rows[0].cells[0].getElementsByTagName("INPUT")[0].checked = "";
                GridVwHeaderChckbox.rows[0].cells[0].getElementsByTagName("div")[0].className = "icheckbox_flat";
            }
            else {
                GridVwHeaderChckbox.rows[0].cells[0].getElementsByTagName("INPUT")[0].checked = "checked";
                GridVwHeaderChckbox.rows[0].cells[0].getElementsByTagName("div")[0].className = "icheckbox_flat checked";
            }
        }
                
        function GetTriggeredElement() {
            $(".default").uniform();
           // ScrollToTag("container", false);
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
    <asp:UpdatePanel ID="UPGeneral" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearchAbdnCartDate" />
        </Triggers>
        <ContentTemplate>
            <section id="container" class="cf GSE-page">
                <div class="narrowcolumn alignleft">                    
                    <div class="filter-block cf">
                        <div class="title-txt">
                            <span>Search</span><a href="#" title="Help video" onclick="GetHelpVideo('Marketing Services','Abandoned Carts')">Help video</a></div>
                        <div class="filter-form cf">
                        <ul class="cf">                            
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlCompany" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlCustomerName" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>
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
                                    <asp:DropDownList ID="ddlLocation" runat="server" class="default">
                                        <asp:ListItem Value="0">- Location -</asp:ListItem>
                                        <asp:ListItem Value="1">Store Front</asp:ListItem>
                                        <asp:ListItem Value="2">Issuance</asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </li>                            
                            <li>
                                <button id="btnSearchAbdnCartDate" runat="server" class="blue-btn" onserverclick="btnSearchAbdnCartDate_Click">Search</button>
                            </li>
                        </ul>
                        </div>
                    </div>
                </div>
                <div class="widecolumn alignright">
                    <div class="filter-content" id="GseManagement">
                        <div class="filter-headbar cf">
                            <span class="headbar-title">Abandoned Carts</span> <em id="totalcount_em" runat="server" visible="false"></em>
                            <div class="filter-search">
                                <asp:TextBox ID="txtSearchGrid" runat="server" CssClass="input-field-small default_title_text"
                                    ToolTip="Search Results..." placeholder="Search Results..." MaxLength="100"></asp:TextBox>
                                <asp:LinkButton ID="btnSearchGrid" runat="server" CssClass="go-btn" OnClick="btnSearchGrid_Click"
                                    Enabled="false"> GO</asp:LinkButton></div>
                        </div>
                        <div class="gse-tablebox cf">
                            <asp:GridView ID="gvAbandonedCart" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowCommand="gvAbandonedCart_RowCommand" CssClass="table-grid">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkhSelectAll" runat="server" onclick="CheckAllCheckbox(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnEmail" runat="server" Value='<%# Eval("Email") %>' />
                                            <asp:HiddenField ID="hdnUserInfoID" runat="server" Value='<%# Eval("UserInfoID") %>' />
                                            <asp:HiddenField ID="hdnMyShoppingCartID" runat="server" Value='<%# Eval("MyShoppingCartID") %>' />
                                            <asp:CheckBox ID="chkSelect" runat="server" onclick="CheckForAllCheckBox(this);" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col1" />
                                        <ItemStyle CssClass="col1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CompanyName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCompany" runat="server" CommandArgument="CompanyName"
                                                CommandName="Sort"><span>Company</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblCompany" Text='<%# Eval("CompanyName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col3"  />
                                        <ItemStyle CssClass="col3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CustomerName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCustomerName" runat="server" CommandArgument="CustomerName" CommandName="Sort"><span >Customer Name</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFirstName" runat="server"  Text='<%# Eval("FirstName") %>'></asp:Label>
                                            <%--<asp:LinkButton ID="lnkFirstName" runat="server" CommandArgument='<%# Eval("MyShoppingCartID") %>' CommandName="CustomerName" Text='<%# Eval("FirstName") %>'></asp:LinkButton>--%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4" />
                                        <ItemStyle CssClass="col4" />
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField SortExpression="CreatedDate">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCreatedDate" runat="server" CommandArgument="CreatedDate" CommandName="Sort"><span style="text-align:center" >Date</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                      
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("CreatedDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col1" />
                                        <ItemStyle CssClass="col1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CreatedTime">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCreatedTime" runat="server" CommandArgument="CreatedTime"
                                                CommandName="Sort"><span >Time</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreatedTime" runat="server"  Text='<%# Eval("CreatedDate", "{0:HH:mm}")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col2" />
                                        <ItemStyle CssClass="col2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Location">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnLocation" runat="server" CommandArgument="Location" CommandName="Sort"><span >Location</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4" />
                                        <ItemStyle CssClass="col4" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="UnitPrice">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnUnitPrice" runat="server" CommandArgument="UnitPrice" CommandName="Sort"><span >Price</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Eval("UnitPrice") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4" />
                                        <ItemStyle CssClass="col4" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table class="table-grid">
        	                            <tr>
          	                            <th class="col1">Date</th>
                                        <th class="col2">Time</th>
                                        <th class="col3">Company</th>
                                        <th class="col4">Location</th>
                                        <th class="col5">Customer Name</th>
                                      </tr>
                                      <tr>
          	                            <td colspan="5" style="text-align:center;vertical-align:middle;">Records not found</td>
                                      </tr>
                                    </table>
                                </EmptyDataTemplate>
                                
                            </asp:GridView>
                            
                            <a id="btnSend" runat="server" class="small-blue-btn submit alignright mrg-5" visible="false" onserverclick="btnSend_click"><span>Send Email</span></a>
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
                    <h2>
                        Under Development...</h2>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
