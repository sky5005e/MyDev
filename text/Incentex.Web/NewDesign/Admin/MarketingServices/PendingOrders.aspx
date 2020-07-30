<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="PendingOrders.aspx.cs" Inherits="Admin_PendingOrders" Title="incentex | Pending Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $(window).ValidationUI();
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(GetTriggeredElement);

            $("#<%= btnSearchPendingOrders.ClientID %>, #<%= btnSend.ClientID %>").click(function() {
                $(".progress-layer").show();
            });            
        });

        function CheckAllCheckbox(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=gvPendingOrders.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
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
            var GridVwHeaderChckbox = document.getElementById("<%=gvPendingOrders.ClientID %>");
            var IsAllChecked = true;
            var UnCheckedTot = 0;
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
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
            ScrollToTag("container", false);
            BindPopUpCloseEvents();
        }

        function ShowPopUp(MainDivTargetID, PopUpDivTargetID) {
            $("#" + MainDivTargetID).css('top', '0');
            $("#" + PopUpDivTargetID).show();
            $(".fade-layer").show();
            popupCenter();
        }

        function popupCenter() {
            $('.popupInner').each(function() {
                $(this).css('margin-top', ($(this).parent('.popup-outer').height() - $(this).height()) / 2)
            });
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
            <asp:PostBackTrigger ControlID="btnSearchPendingOrders" />
        </Triggers>
        <ContentTemplate>
            <section id="container" class="cf GSE-page">
                <div class="narrowcolumn alignleft">                    
                    <div class="filter-block cf">
                        <div class="title-txt">
                            <span>Search</span><a href="#" title="Help video" onclick="GetHelpVideo('Marketing Services','Pending Orders')">Help video</a></div>
                        <div class="filter-form cf">
                        <ul class="cf">
                            <li>
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlCompany" runat="server" class="default" 
                                    onselectedindexchanged="ddlCompany_SelectedIndexChanged" AutoPostBack="true">
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
                                <span class="select-drop filter-drop">
                                    <asp:DropDownList ID="ddlWorkgroup" runat="server" class="default">
                                    </asp:DropDownList>
                                </span>
                            </li>
                            <li>
                                <asp:TextBox ID="txtNameOfCustomer" runat="server" CssClass="input-field-all" placeholder="Name of Customer"
                                    ToolTip="Name of Customer"></asp:TextBox>
                            </li>                            
                            <li>
                                <asp:TextBox ID="txtNameOfApprover" runat="server" CssClass="input-field-all" placeholder="Name of Approver"
                                    ToolTip="Name of Approver" ></asp:TextBox>
                            </li>
                            <li>
                                <button id="btnSearchPendingOrders" runat="server" class="blue-btn" onserverclick="btnSearchPendingOrders_Click">Search</button>
                            </li>
                        </ul>
                        </div>
                    </div>
                </div>
                <div class="widecolumn alignright">
                    <div class="filter-content" id="GseManagement">
                        <div class="filter-headbar cf">
                            <span class="headbar-title">Pending Orders</span> <em id="totalcount_em" runat="server" visible="false"></em>
                            <div class="filter-search">
                                <asp:TextBox ID="txtSearchGrid" runat="server" CssClass="input-field-small default_title_text"
                                    ToolTip="Search Results..." placeholder="Search Results..." MaxLength="100"></asp:TextBox>
                                <asp:LinkButton ID="btnSearchGrid" runat="server" CssClass="go-btn" OnClick="btnSearchGrid_Click"
                                    Enabled="false"> GO</asp:LinkButton></div>
                        </div>
                        <div class="gse-tablebox cf">
                            <asp:GridView ID="gvPendingOrders" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowCommand="gvPendingOrders_RowCommand" OnRowDataBound="gvPendingOrders_RowDataBound" CssClass="table-grid">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkhSelectAll" runat="server" onclick="CheckAllCheckbox(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnApproverID" runat="server" Value='<%# Eval("ApproverID") %>' />
                                            <asp:HiddenField ID="hdnEmail" runat="server" Value='<%# Eval("ApproverEmailID") %>' />
                                            <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%# Eval("OrderID") %>' />
                                            <asp:HiddenField ID="hdnApproverDetails" runat="server" Value='<%# Eval("ApproverDetails") %>' />
                                            <asp:HiddenField ID="hdnOrderApproverLevel" runat="server" Value='<%# Eval("OrderApproverLevel") %>' />
                                            <asp:CheckBox ID="chkSelect" runat="server" onclick="CheckForAllCheckBox(this);" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col1" />
                                        <ItemStyle CssClass="col1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CompanyName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCompanyName" runat="server" CommandArgument="CompanyName" CommandName="Sort"><span style="text-align:center" >Company</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                      
                                            <asp:Label ID="lblCompanyName" runat="server" Text='<%# Eval("CompanyName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col2" />
                                        <ItemStyle CssClass="col2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Station">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnStation" runat="server" CommandArgument="Station"
                                                CommandName="Sort"><span >Station</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblStation" runat="server"  Text='<%# Eval("Station")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col3" />
                                        <ItemStyle CssClass="col3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Workgroup">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnWorkgroup" runat="server" CommandArgument="Workgroup"
                                                CommandName="Sort"><span>Workgroup</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblWorkgroup" Text='<%# Eval("Workgroup") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4"  />
                                        <ItemStyle CssClass="col4" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CustomerName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnNameOfPendingUser" runat="server" CommandArgument="CustomerName" CommandName="Sort"><span >Customer w/ Pending Order</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCustomerName" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col5" />
                                        <ItemStyle CssClass="col5" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ApproverName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnNameOfApprover" runat="server" CommandArgument="ApproverName" CommandName="Sort"><span >Name of Approver</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblNameOfApprover" Text='<%# Eval("ApproverName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col6" />
                                        <ItemStyle CssClass="col6" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="OrderDate">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnOrderDate" runat="server" CommandArgument="OrderDate" CommandName="Sort"><span >Order Date</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblOrderDate" Text='<%# Eval("OrderDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col7" />
                                        <ItemStyle CssClass="col7" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table class="table-grid">
        	                            <tr>
          	                            <th class="col1">Company</th>
                                        <th class="col2">Station</th>
                                        <th class="col3">Workgroup</th>
                                        <th class="col4">Customer w/ Pending Order</th>                                        
                                        <th class="col5">Name of Approver</th>
                                        <th class="col6">Order Date</th>
                                      </tr>
                                      <tr>
          	                            <td colspan="6" style="text-align:center;vertical-align:middle;">Records not found</td>
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
                        System Data</h2>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
