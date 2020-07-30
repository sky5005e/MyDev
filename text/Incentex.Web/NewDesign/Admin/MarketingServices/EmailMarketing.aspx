<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="EmailMarketing.aspx.cs" Inherits="Admin_EmailMarketing" Title="incentex | Email Marketing" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../../StaticContents/js/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $(window).ValidationUI();
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(GetTriggeredElement);

            $("#<%= btnSearchAbdnCartDate.ClientID %>, #<%= btnEmailHistory.ClientID %>, #<%= btnSearchEmailHistory.ClientID %>").click(function() {
                $(".progress-layer").show();
            });

            $("#<%= btnSendTemplate.ClientID %>").click(function() {
                if (ValidateSendEmail() == true) {
                    $(".progress-layer").show();
                }
                else return false;
            });

            if ($("#<%= ddlDate.ClientID %>").val() == 4) {
                $(".spanDate").css("display", "block");
            }

            if ($("#<%= ddlSentDateH.ClientID %>").val() == 4) {
                $(".spanDateSent").css("display", "block");
            }

            if ($("#<%= ddlViewDateH.ClientID %>").val() == 4) {
                $(".spanDateView").css("display", "block");
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

            $("#<%= ddlSentDateH.ClientID %>").change(function() {
                if ($(this).val() == 4) {
                    $(".spanDateSent").css("display", "block");
                }
                else {
                    $(".spanDateSent").css("display", "none");
                    $("#<%= txtFromDateSentH.ClientID %>").val("");
                    $("#<%= txtToDateSentH.ClientID %>").val("");
                }
            });

            $("#<%= ddlViewDateH.ClientID %>").change(function() {
                if ($(this).val() == 4) {
                    $(".spanDateView").css("display", "block");
                }
                else {
                    $(".spanDateView").css("display", "none");
                    $("#<%= txtFromDateViewH.ClientID %>").val("");
                    $("#<%= txtToDateViewH.ClientID %>").val("");
                }
            });

            if ($("#<%= rbSendIndividually.ClientID %>").attr("checked") == undefined) {
                $(".li-customer-email").css("display", "none");
                $(".li-comp-work").css("display", "block");
            }
            else {
                $(".li-customer-email").css("display", "block");
                $(".li-comp-work").css("display", "none");
            }

            if ($("#<%= rbSendImmediately.ClientID %>").attr("checked") == undefined) {
                $(".send-later").css("display", "block");
            }
            else {
                $(".send-later").css("display", "none");
            }

            $("#<%= txtDateTime.ClientID %>").datetimepicker({
                changemonth: true,
                changeyear: true
            });
            $("#<%= rbSendIndividually.ClientID %>").on('ifChecked', function(event) {
                var thisName = $('input:radio').attr('name');
                if ($('input:radio[name="' + thisName + '"]').is(':checked') == true) {
                    $(".li-customer-email").css("display", "block");
                    $(".li-comp-work").css("display", "none");
                }
                else {
                    $(".li-customer-email").css("display", "none");
                    $(".li-comp-work").css("display", "block");
                }
            });
            $("#<%= rbSendInMask.ClientID %>").on('ifChecked', function(event) {
                var thisName = $('input:radio').attr('name');
                if ($('input:radio[name="' + thisName + '"]').is(':checked') == true) {
                    $(".li-customer-email").css("display", "none");
                    $(".li-comp-work").css("display", "block");
                }
                else {
                    $(".li-customer-email").css("display", "block");
                    $(".li-comp-work").css("display", "none");

                }
            });

            $("#<%= rbSendImmediately.ClientID %>").on('ifChecked', function(event) {
                var thisName = $('input:radio').attr('name');
                if ($('input:radio[name="' + thisName + '"]').is(':checked') == true) {
                    $(".send-later").css("display", "none");
                }
                else {
                    $(".send-later").css("display", "block");
                }
            });

            $("#<%= rbSendLater.ClientID %>").on('ifChecked', function(event) {
                var thisName = $('input:radio').attr('name');
                if ($('input:radio[name="' + thisName + '"]').is(':checked') == true) {
                    $(".send-later").css("display", "block");
                }
                else {
                    $(".send-later").css("display", "none");
                }
            });
        });

        function ClearValidation() {
            $("#<%= txtCustomerEmailID.ClientID %>").removeClass("ErrorField");
            $("#<%= ddlCompanySE.ClientID %>").parent().removeClass("ErrorField");
            $("#<%= ddlWorkgroupSE.ClientID %>").parent().removeClass("ErrorField");
            $("#<%= txtDateTime.ClientID %>").removeClass("ErrorField");
            $(".li-email-text").removeClass("ErrorField");
        }
        function ValidateSendEmail() {
            ClearValidation();
            var isValid = true;

            if ($(".cke_contents").find("textarea").val() != undefined) {
                if ($(".cke_contents").find("textarea").val() == '') {
                    $(".li-email-text").addClass("ErrorField");
                    isValid = false;
                }
            }
            else if ($(".cke_contents").find("iframe").contents().find("body").html() != undefined) {
                if ($(".cke_contents").find("iframe").contents().find("body").html() == "<p><br></p>") {
                    $(".li-email-text").addClass("ErrorField");
                    isValid = false;
                }
            }
                        
            if ($("#<%= rbSendIndividually.ClientID %>").attr("checked") == "checked") {
                if ($("#<%= txtCustomerEmailID.ClientID %>").val() == "") {
                    $("#<%= txtCustomerEmailID.ClientID %>").addClass("ErrorField");
                    isValid = false;
                }
                else {
                    var x = $("#<%= txtCustomerEmailID.ClientID %>").val();
                    var atpos = x.indexOf("@");
                    var dotpos = x.lastIndexOf(".");
                    if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {
                        $("#<%= txtCustomerEmailID.ClientID %>").addClass("ErrorField");
                        isValid = false;
                    }
                }
            }
            if ($("#<%= rbSendInMask.ClientID %>").attr("checked") == "checked") {
                if ($("#<%= ddlCompanySE.ClientID %>").val() == "0") {
                    $("#<%= ddlCompanySE.ClientID %>").parent().addClass("ErrorField");
                    isValid = false;
                }
                if ($("#<%= ddlWorkgroupSE.ClientID %>").val() == "0") {
                    $("#<%= ddlWorkgroupSE.ClientID %>").parent().addClass("ErrorField");
                    isValid = false;
                }
            }
            if ($("#<%= rbSendLater.ClientID %>").attr("checked") == "checked") {
                if ($("#<%= txtDateTime.ClientID %>").val() == "") {
                    $("#<%= txtDateTime.ClientID %>").addClass("ErrorField");
                    isValid = false;
                }
            }

            return isValid;
        }

        function CheckForMaskOption() {
            if ($("#<%= rbSendIndividually.ClientID %>").attr("checked") == "checked") {
                $(".li-customer-email").css("display", "block");
                $(".li-comp-work").css("display", "none");
            }
            else if ($("#<%= rbSendInMask.ClientID %>").attr("checked") == "checked") {
                $(".li-customer-email").css("display", "none");
                $(".li-comp-work").css("display", "block");
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
                        <button id="btnSendEmail" runat="server" class="blue-btn" onserverclick="btnSendEmail_click">Send Email</button>
                        <button id="btnEmailHistory" runat="server" class="blue-btn" onserverclick="btnEmailHistory_click">History</button>
                        <div class="title-txt">
                            <span>Search</span><a href="#" title="Help video" onclick="GetHelpVideo('Marketing Services','Email Marketing')">Help video</a></div>
                        <div class="filter-form cf" runat="server" id="divEmailFilter">
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
                                        <asp:DropDownList ID="ddlCompany" runat="server" class="default"
                                            onselectedindexchanged="ddlCompany_SelectedIndexChanged" AutoPostBack="true">
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
                                        <asp:DropDownList ID="ddlModule" runat="server" class="default">
                                        </asp:DropDownList>
                                    </span>
                                </li>
                                <li>
                                    <span class="select-drop filter-drop">
                                        <asp:DropDownList ID="ddlEmailTitle" runat="server" class="default">
                                        </asp:DropDownList>
                                    </span>
                                </li>
                                <li>
                                    <button id="btnSearchAbdnCartDate" runat="server" class="blue-btn" onserverclick="btnSearchAbdnCartDate_Click">Search</button>
                                </li>
                            </ul>
                        </div>
                        <div class="filter-form cf" runat="server" id="divHistoryFilter" style="display:none;">
                            <ul class="cf">
                                <li>
                                    <span class="select-drop filter-drop">
                                        <asp:DropDownList ID="ddlCompanyH" runat="server" class="default">
                                        </asp:DropDownList>
                                    </span>
                                </li>
                                <li>
                                    <span class="select-drop filter-drop">
                                        <asp:DropDownList ID="ddlUserH" runat="server" class="default">
                                        </asp:DropDownList>
                                    </span>
                                </li>
                                <li>
                                    <span class="select-drop filter-drop">
                                        <asp:DropDownList ID="ddlSentDateH" runat="server" class="default">
                                            <asp:ListItem Text="- Sent Date -" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Today" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Yesterday" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Last 7 days" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Custom" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </span>
                                </li>
                                <li>
                                    <span class="spanDateSent" style="display:none;">
                                        <asp:TextBox ID="txtFromDateSentH" runat="server" CssClass="input-field-all setDatePicker" placeholder="From Date"
                                            ToolTip="From Date" tabindex="1"></asp:TextBox>
                                    </span>
                                </li>
                                <li>
                                    <span class="spanDateSent" style="display:none;">
                                        <asp:TextBox ID="txtToDateSentH" runat="server" CssClass="input-field-all setDatePicker" placeholder="To Date"
                                            ToolTip="To Date" tabindex="1"></asp:TextBox>
                                    </span>
                                </li>
                                <li>
                                    <span class="select-drop filter-drop">
                                        <asp:DropDownList ID="ddlViewDateH" runat="server" class="default">
                                            <asp:ListItem Text="- View Date -" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Today" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Yesterday" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Last 7 days" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Custom" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </span>
                                </li>
                                <li>
                                    <span class="spanDateView" style="display:none;">
                                        <asp:TextBox ID="txtFromDateViewH" runat="server" CssClass="input-field-all setDatePicker" placeholder="From Date"
                                            ToolTip="From Date" tabindex="1"></asp:TextBox>
                                    </span>
                                </li>
                                <li>
                                    <span class="spanDateView" style="display:none;">
                                        <asp:TextBox ID="txtToDateViewH" runat="server" CssClass="input-field-all setDatePicker" placeholder="To Date"
                                            ToolTip="To Date" tabindex="1"></asp:TextBox>
                                    </span>
                                </li>
                                <li>
                                    <button id="btnSearchEmailHistory" runat="server" class="blue-btn" onserverclick="btnSearchEmailHistory_Click">Search</button>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="widecolumn alignright">
                    <div class="filter-content" id="GseManagement">
                        <div class="filter-headbar cf">
                            <span class="headbar-title">Email Marketing</span> <em id="totalcount_em" runat="server" visible="false"></em>
                            <div class="filter-search">
                                <asp:TextBox ID="txtSearchGrid" runat="server" CssClass="input-field-small default_title_text"
                                    ToolTip="Search Results..." placeholder="Search Results..." MaxLength="100"></asp:TextBox>
                                <asp:LinkButton ID="btnSearchGrid" runat="server" CssClass="go-btn" OnClick="btnSearchGrid_Click"
                                    Enabled="false"> GO</asp:LinkButton></div>
                        </div>
                        <div class="gse-tablebox cf">
                            <asp:GridView ID="gvEmailMarketing" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowCommand="gvEmailMarketing_RowCommand" CssClass="table-grid">
                                <Columns>                                    
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
                                    <asp:TemplateField SortExpression="FirstName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnFirstName" runat="server" CommandArgument="FirstName" CommandName="Sort"><span >User</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFirstName" runat="server"  Text='<%# Eval("FirstName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col2" />
                                        <ItemStyle CssClass="col2" />
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
                                    <asp:TemplateField SortExpression="Workgroup">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnWorkgroup" runat="server" CommandArgument="Workgroup" CommandName="Sort"><span >Workgroup</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label ID="lblWorkgroup" runat="server" Text='<%# Eval("Workgroup") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4" />
                                        <ItemStyle CssClass="col4" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ModuleName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnModuleName" runat="server" CommandArgument="ModuleName" CommandName="Sort"><span >Module</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblModuleName" runat="server"  Text='<%# Eval("ModuleName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col5" />
                                        <ItemStyle CssClass="col5" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Title">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnTitle" runat="server" CommandArgument="Title" CommandName="Sort"><span >Title</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTitle" runat="server"  Text='<%# Eval("Title") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col6" />
                                        <ItemStyle CssClass="col6" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table class="table-grid">
        	                            <tr>
          	                            <th class="col1">Date</th>
                                        <th class="col2">User</th>
                                        <th class="col3">Company</th>
                                        <th class="col4">Workgroup</th>
                                        <th class="col5">Module</th>
                                        <th class="col6">Title</th>
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
                    <!-- History Grid  -->
                    <div class="gse-tablebox cf">
                            <asp:GridView ID="gvEmailHistory" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowCommand="gvEmailHistory_RowCommand" CssClass="table-grid">
                                <Columns>                                    
                                    <asp:TemplateField SortExpression="CompanyName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCompanyName" runat="server" CommandArgument="CompanyName" CommandName="Sort"><span>Company</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompanyName" runat="server"  Text='<%# Eval("CompanyName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col2" />
                                        <ItemStyle CssClass="col2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SentTo">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnSentTo" runat="server" CommandArgument="SentTo" CommandName="Sort"><span style="text-align:center" >User</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                      
                                            <asp:Label ID="lblSentTo" runat="server" Text='<%# Eval("SentTo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col1" />
                                        <ItemStyle CssClass="col1" />
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField SortExpression="SentDate">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnSentDate" runat="server" CommandArgument="SentDate"
                                                CommandName="Sort"><span>Sent Date</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblSentDate" Text='<%# Eval("SentDate") == null ? "" : Eval("SentDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col2"  />
                                        <ItemStyle CssClass="col2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SentTime">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnSentTime" runat="server" CommandArgument="SentTime"
                                                CommandName="Sort"><span>Sent Time</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label runat="server" ID="lblSentTime" Text='<%# Eval("SentDate") == null ? "" : Eval("SentDate", "{0:HH:mm}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col3"  />
                                        <ItemStyle CssClass="col3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ViewedDate">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnViewedDate" runat="server" CommandArgument="ViewedDate" CommandName="Sort"><span >Viewed Date</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                          
                                               <asp:Label ID="lblViewedDate" runat="server" Text='<%# Eval("ViewedDate") == null ? "" : Eval("ViewedDate", "{0:MM/dd/yyyy}")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4" />
                                        <ItemStyle CssClass="col4" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ViewedTime">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnViewedTime" runat="server" CommandArgument="ViewedTime" CommandName="Sort"><span >Viewed Time</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>                          
                                               <asp:Label ID="lblViewedTime" runat="server" Text='<%# Eval("ViewedDate") == null ? "" : Eval("ViewedDate", "{0:HH:mm}")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col5" />
                                        <ItemStyle CssClass="col5" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table class="table-grid">
        	                            <tr>
          	                            <th class="col1">Company</th>
                                        <th class="col2">User</th>
                                        <th class="col3">Sent Date</th>
                                        <th class="col4">Sent Time</th>
                                        <th class="col5">Viewed Date</th>
                                        <th class="col6">Viewed Time</th>
                                      </tr>
                                      <tr>
          	                            <td colspan="5" style="text-align:center;vertical-align:middle;">Records not found</td>
                                      </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    <div id="pagingtableH" runat="server" class="store-footer cf" visible="false">
                        <a href="javascript:;" class="store-title">BACK TO TOP</a>
                        <asp:LinkButton ID="lnkViewAllBottomH" runat="server" OnClick="lnkViewAllH_Click" CssClass="pagination alignright view-link postback cf"> VIEW ALL </asp:LinkButton>
                        <div class="pagination alignright cf">
                            <span>
                                <asp:LinkButton ID="lnkPreviousH" CssClass="left-arrow alignleft postback" runat="server"
                                    OnClick="lnkPreviousH_Click" ToolTip="Previous"> </asp:LinkButton>
                            </span>
                            <asp:DataList ID="dtlPagingH" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" OnItemCommand="dtlPagingH_ItemCommand">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPaging" runat="server" CommandArgument='<%# Eval("Index") %>'
                                        CommandName="ChangePage" Text='<%# Eval("Index") %>' CssClass="postback"> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList>
                            <asp:LinkButton ID="lnkNextH" CssClass="right-arrow alignright postback" runat="server"
                                OnClick="lnkNextH_Click" ToolTip="Next"> </asp:LinkButton>
                        </div>
                    </div>
                    <!-- History Grid END -->
                    </div>
                </div>
                
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="email-marketing-popup" class="popup-outer popupouter-center">
        <div class="popupInner">
            <div class="specs-popup">
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Marketing Services','Send Email')">Help Video</a>
                <a class="close-btn" href="javascript:void(0);">Close</a>
                <div class="warranty-content">
                    <h2><asp:Label ID="lblHisotryTitle" runat="server">Send Email</asp:Label></h2>
                    <div class="clear"></div>
                    <div class="table-outer">
                        <ul class="cf">                            
                            <li class="li-email-text">
                                <CKEditor:CKEditorControl ID="TxtEmailText" BasePath="../../../JS/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                            </li>
                            <li>
                                <ul>
                                    <li><asp:RadioButton ID="rbSendIndividually" Text="Send Individually" runat="server" GroupName="MaskOption" Checked="true" /></li>
                                    <li><asp:RadioButton ID="rbSendInMask" Text="Send in Mask" runat="server" GroupName="MaskOption" /></li>                                    
                                </ul>
                            </li>
                            <li class="li-comp-work" style="display:none;">
                                  <span class="select-drop filter-drop">
                                        <asp:DropDownList ID="ddlCompanySE" runat="server" class="default"
                                            OnSelectedIndexChanged="ddlCompanySE_SelectedIndexChanged" AutoPostBack="true" >
                                        </asp:DropDownList>
                                  </span>
                                  <span class="select-drop filter-drop">
                                        <asp:DropDownList ID="ddlWorkgroupSE" runat="server" class="default">
                                        </asp:DropDownList>
                                  </span>
                            </li>
                            <li class="li-customer-email">
                                <asp:TextBox ID="txtCustomerEmailID" runat="server" CssClass="input-field-all" placeholder="Customer Email Id"
                                    ToolTip="Customer Email Id"></asp:TextBox>
                            </li>
                            <li>
                                <ul>
                                    <li><asp:RadioButton ID="rbSendImmediately" Text="Send Immediately" runat="server" GroupName="SendingOption" Checked="true" /></li>
                                    <li><asp:RadioButton ID="rbSendLater" Text="Send Later" runat="server" GroupName="SendingOption" /></li>                                    
                                </ul>
                            </li>
                             <li class="send-later" style="display:none;">
                                <asp:TextBox ID="txtDateTime" runat="server" CssClass="input-field-all" placeholder="Date/Time"
                                    ToolTip="Date/Time"></asp:TextBox>
                            </li>
                            <li>
                                <%--<button id="btnSendTemplate" runat="server" class="blue-btn submit" validationgroup="BasicTabValidate" call="BasicTabValidate" onserverclick="btnSendTemplate_click">Send</button>--%>
                                <asp:Button ID="btnSendTemplate" runat="server" class="blue-btn submit" Text="Send" OnClick="btnSendTemplate_click"></asp:Button>
                            </li>
                        </ul>                            
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
