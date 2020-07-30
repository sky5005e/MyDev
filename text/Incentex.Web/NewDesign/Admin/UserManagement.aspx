<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="UserManagement.aspx.cs" Inherits="Admin_UserManagement" Title="incentex | User Management" %>

<%@ Reference Control="~/NewDesign/UserControl/MultiSelectDropDown.ascx" %>
<%@ Register Src="~/NewDesign/UserControl/MultiSelectDropDown.ascx" TagName="MultiSelectDropDown"
    TagPrefix="msd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $(window).ValidationUI();

            $(".postback").click(function() {
                $(".progress-layer").show();
            });

            $("#addNewEmployee").click(function() {
                AddEmployeePopup();
            });

            $("#aCloseAddEmployee, #lbAddPopupCancel").click(function() {
                $("#add-Employee-Block").hide();
                $(".fade-layer").hide();
            });

            $("#aCloseEmployeeDetails, #aBasicCancel").click(function() {
                $(".progress-layer").show();
                Page_ClientValidate("clearValidate");
                $("#<%= btnClearEmployeeDetails.ClientID %>").click();
            });

            $("#aMenuAccessCancel").click(function() {
                $.changeTab($("li[tab-id='basic']"));
            });

            $("#aAdminSettingsCancel").click(function() {
                $.changeTab($("li[tab-id='menuaccess']"));
            });
            
            $("#aReportsCancel").click(function() {
                $.changeTab($("li[tab-id='adminsettings']"));
            });

            $("#aAddPopupGeneratePassword").click(function() {
                $("#<%= txtAddPopupPassword.ClientID %>").val($.generatePassword(8));
            });

            $("#aBasicGeneratePassword").click(function() {
                $("#<%= txtBasicPassword.ClientID %>").val($.generatePassword(8));
            });

            $("#<%= lbAddPopupAddEmployee.ClientID %>").click(function() {
                var PageIsValid = Page_ClientValidate("AddEmployee");
                if (PageIsValid) {
                    $(".progress-layer").show();
                }
                return PageIsValid;
            });

            $("#<%= lbUploadExcel.ClientID %>").click(function() {
                var PageIsValid = Page_ClientValidate("UploadUsers");
                if (PageIsValid) {
                    $(".progress-layer").show();
                }
                return PageIsValid;
            });

            $("#<%= lbBasicSave.ClientID %>, #<%= lbBasicClose.ClientID %>, <%= lbBasicSendEmail.ClientID %>").click(function() {
                var PageIsValid = Page_ClientValidate("Basic");
                if (PageIsValid) {
                    $(".progress-layer").show();
                }
                return PageIsValid;
            });

            $("#<%= lbMenuAccessSave.ClientID %>, #<%= lbMenuAccessClose.ClientID %>").click(function() {
                var isOkay = true;
                
                if($(".chkMOAS input:checked").length > 0 && $(".approver input[type=checkbox]:checked").length == 0) {
                    GeneralAlertMsg('Please select at least one MOAS approver.', true);
                    isOkay = false;
                }
                else {
                    $(".approver input[type=checkbox]:checked").each(function() {
                        if ($(this).parents(".approver").siblings(".priority").val() == "" && isOkay) {
                            GeneralAlertMsg('Please provide priority for MOAS approver(s).', true);
                            isOkay = false;
                        }
                    });
                    
                    if (isOkay) {
                        var lstPriority = $(".priority").filter(function() {
                            var txtPriority = $(this);
                            if (txtPriority.val() != "") {
                                return txtPriority;
                            }
                        });
                        
                        lstPriority.each(function() {
                            if (isOkay) {
                                var txtPriority1 = $(this);                                
                                lstPriority.each(function() {
                                    if (isOkay) {
                                        var txtPriority2 = $(this);
                                        if (txtPriority1.attr("id") != txtPriority2.attr("id") && txtPriority1.val() == txtPriority2.val()) {
                                            GeneralAlertMsg('Same priority can not be assigned to more than one MOAS approver(s).', true);
                                            isOkay = false;
                                            return;
                                        }
                                    }
                                });
                            }
                            else
                                return;
                        });
                    }
                }
                
                if (isOkay)
                    $(".progress-layer").show();
                    
                return isOkay;
            });
            
            $(".multi-select-container").click(function(e) {
                $(this).siblings(".multi-select-content").addClass("clicked");
                $(".multi-select-container").removeClass("highlight");
                $(this).addClass("highlight");
                $(".multi-select-content").not(".clicked").hide();
                
//                var containerParentBottom = $("#workgroupmanagementcontainer").offset().top + $("#workgroupmanagementcontainer").height();
//                var containerBottom = $(this).offset().top + $(this).height();
//                var contentHeight = $(this).siblings(".multi-select-content").height();
//                
//                alert(containerParentBottom - containerBottom);
//                
//                if ((containerParentBottom - containerBottom) > contentHeight) {                
//		            $(this).siblings(".multi-select-content").slideToggle({direction:"down"}, 20);
//		            alert(1);
//		          }
//		          else {
//		            alert(2);
//		            $(this).siblings(".multi-select-content").slideToggle({direction:"up"}, 20);
//		          }

                $(this).siblings(".multi-select-content").toggle();
		        
		        $(this).siblings(".multi-select-content").removeClass("clicked");
		        e.stopPropagation();
	        });
        	
	        $(".multi-select-content").click(function(e) {	            
	            e.stopPropagation();
	        });
	        
	        $("span[class*='multi-select-check-']").find("input[type=checkbox]").click(function() {
                var clickedCheckbox = $(this);
                clickedCheckbox.attr('checked', !clickedCheckbox.attr('checked'));
                
                var multiSelectContentDiv = clickedCheckbox.parent().parent().parent().parent();
                var labelSpan = multiSelectContentDiv.siblings("span");
                
	            if (multiSelectContentDiv.find("input[type=checkbox]:checked").length > 0) {
	                labelSpan.text("- Selected -");
	            }
	            else {
	                labelSpan.text(labelSpan.attr('title'));            
	            }
	        });
        	
	        $(document).click(function() {
	            $(".multi-select-container").removeClass("highlight");
                $(".multi-select-content").hide();
	        });
	        
	        $.updatePriority = function(ele) {
	            var lstPriority = $(".priority").filter(function() {
                    var txtPriority = $(this);
                    if (txtPriority.val() != "") {
                        return txtPriority;
                    }
                });
                
                var priorityCount = lstPriority.length;
                
                if ($(ele).is(":checked")) {
                    priorityCount += 1;
                    $(ele).parents(".approver").siblings(".priority").val(priorityCount);
                }
                else {
                    var clearedPriority = parseInt($(ele).parents(".approver").siblings(".priority").val(), 10);
                    $(ele).parents(".approver").siblings(".priority").val("");
                    
                    lstPriority.each(function() {
                        var txtPriority = $(this);
                        var currentPriority = parseInt($(txtPriority).val(), 10);
                        if (currentPriority > clearedPriority) {
                            $(txtPriority).val(currentPriority - 1);
                        }
                    });
                }
	        };
        });
        
        function AddEmployeePopup() {
            $("#add-Employee-Block").css("top", "0").show();
            $(".fade-layer").show();
            SetPopUpAtTop();
        }
        
        function EmployeeDetailsPopup(tabID, sectionID) {            
            $("#employee-Details-Block").css("top", "0").show();
            $(".fade-layer").show();            
            SetPopUpAtTop();
            $.changeTab($("li[tab-id=" + tabID + "]"));            
            
            if ((tabID == "adminsettings" || tabID == "reports") && sectionID.length > 0) {
                $.changeTab($("li[tab-id=" + sectionID + "]"));                
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="umScriptManager" runat="server">
    </asp:ScriptManager>
    <input type="hidden" value="admin-link" id="hdnActiveLink" />
    <asp:HiddenField ID="hdnBasicUserInfoID" runat="server" />
    <asp:HiddenField ID="hdnCurrentSection" runat="server" />
    <asp:Button ID="btnClearEmployeeDetails" runat="server" OnClick="btnClearEmployeeDetails_Click"
        Text="Clear Employee Details" Style="display: none;" ValidationGroup="clearValidate" />
    <% if (!Request.IsLocal)
       { %>
    <section id="container" class="cf filter-page">
    <% }
       else
       { %>
    <div id="container" class="cf filter-page">
        <%} %>
        <div class="narrowcolumn alignleft">
            <a id="addNewEmployee" href="javascript:;" class="blue-btn add-new">Add New Employee</a>
            <div class="filter-block cf">
                <div class="title-txt">
                    <span>&nbsp;&nbsp;</span><a href="javascript:;" title="Help Video" onclick="GetHelpVideo('User Management','Employee Management')">Help
                        Video</a></div>
                <div class="filter-form cf">
                    <ul class="cf">
                        <li>
                            <label class="select-txt filter-text">
                                Select one or more search criteria and run the report.
                            </label>
                        </li>
                        <li id="liCompany" runat="server" visible="false"><span class="select-drop filter-drop">
                            <asp:DropDownList ID="ddlSearchCompany" runat="server" CssClass="default" TabIndex="1">
                            </asp:DropDownList>
                        </span></li>
                        <li>
                            <asp:TextBox ID="txtSearchFirstName" runat="server" CssClass="input-field-all first-field default_title_text"
                                placeholder="First Name" ToolTip="First Name" TabIndex="2" MaxLength="200">
                            </asp:TextBox>
                        </li>
                        <li>
                            <asp:TextBox ID="txtSearchLastName" runat="server" CssClass="input-field-all first-field default_title_text"
                                placeholder="Last Name" ToolTip="Last Name" TabIndex="3" MaxLength="200">
                            </asp:TextBox>
                        </li>
                        <li><span class="select-drop filter-drop">
                            <asp:DropDownList ID="ddlSearchWorkGroup" runat="server" CssClass="default" TabIndex="4">
                            </asp:DropDownList>
                        </span></li>
                        <li><span class="select-drop filter-drop">
                            <asp:DropDownList ID="ddlSearchBaseStation" runat="server" CssClass="default" TabIndex="5">
                            </asp:DropDownList>
                        </span></li>
                        <li><span class="select-drop filter-drop">
                            <asp:DropDownList ID="ddlSearchSystemStatus" runat="server" CssClass="default" TabIndex="6">
                            </asp:DropDownList>
                        </span></li>
                        <li><span class="select-drop filter-drop">
                            <asp:DropDownList ID="ddlSearchSystemAccess" runat="server" CssClass="default" TabIndex="7">
                            </asp:DropDownList>
                        </span></li>
                        <%--<li>
                            <asp:TextBox ID="txtSearchEmail" runat="server" CssClass="input-field-all default_title_text"
                                placeholder="Email" ToolTip="Email" TabIndex="5">
                            </asp:TextBox>
                        </li>--%>
                        <li>
                            <asp:Button ID="btnSearchEmployee" runat="server" OnClick="btnSearchEmployee_Click"
                                CssClass="blue-btn add-new postback" Text="Search for Employee" TabIndex="7" />
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="widecolumn alignright">
            <div class="filter-content">
                <div class="filter-header cf">
                    <span class="title-txt">Employee Management</span> <em id="totalcount_em" runat="server"
                        visible="false"></em>
                    <div class="filter-search">
                        <asp:TextBox ID="txtSearchGrid" runat="server" CssClass="input-field-small default_title_text"
                            ToolTip="Search Results..." placeholder="Search Results..." MaxLength="100"></asp:TextBox>
                        <asp:LinkButton ID="btnSearchGrid" runat="server" CssClass="go-btn" OnClick="btnSearchGrid_Click"
                            Enabled="false">
                            GO</asp:LinkButton>
                    </div>
                </div>
                <div class="filter-tableblock cf">
                    <asp:GridView ID="gvEmployees" runat="server" AutoGenerateColumns="false" GridLines="None"
                        Visible="true" OnRowCommand="gvEmployees_RowCommand" CssClass="table-grid">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbEmployeeNameHeader" runat="server" CommandArgument="EmployeeName"
                                        CommandName="Sort" CssClass="postback">Employee Name</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span>
                                        <asp:LinkButton ID="lbEmployeeName" runat="server" CommandName="Detail" CommandArgument='<%# Eval("UserInfoID") %>'
                                            Text='<%# Eval("FirstName") + " " + Eval("LastName")  %>' CssClass="postback"></asp:LinkButton></span>
                                    <asp:HiddenField ID="hdnUserInfoID" runat="server" Value='<%# Eval("UserInfoID") %>' />
                                    <asp:HiddenField ID="hdnEmployeeID" runat="server" Value='<%# Eval("CompanyEmployeeID") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col1" />
                                <ItemStyle CssClass="col1" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbWorkgroupHeader" runat="server" CommandArgument="Workgroup"
                                        CommandName="Sort" CssClass="postback">Workgroup</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkgroup" runat="server" Text='<%# Eval("Workgroup") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col2" />
                                <ItemStyle CssClass="col2" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbBaseStationHeader" runat="server" CommandArgument="BaseStation"
                                        CommandName="Sort" CssClass="postback">Station</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBaseStation" runat="server" Text='<%# Eval("BaseStation") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col3" />
                                <ItemStyle CssClass="col3" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbEmployeeStatusHeader" runat="server" CommandArgument="EmployeeStatus"
                                        CommandName="Sort">Status</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="apple_check">
                                        <label class="apple_checkbox">
                                            <asp:CheckBox ID="chkStatus" runat="server" Checked='<%# Convert.ToBoolean(Eval("EmployeeStatus")) %>'
                                                AutoPostBack="true" OnCheckedChanged="chkStatus_CheckedChanged" CssClass="postback"></asp:CheckBox>&nbsp;</label></div>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col4" />
                                <ItemStyle CssClass="col4" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="pagingtable" runat="server" class="store-footer cf" visible="false">
                    <a href="javascript:;" class="store-title">BACK TO TOP</a>
                    <asp:LinkButton ID="lnkViewAllBottom" runat="server" OnClick="lnkViewAll_Click" CssClass="pagination alignright view-link postback cf">
                        VIEW ALL
                    </asp:LinkButton>
                    <div class="pagination alignright cf">
                        <span>
                            <asp:LinkButton ID="lnkPrevious" CssClass="left-arrow alignleft postback" runat="server"
                                OnClick="lnkPrevious_Click" ToolTip="Previous">
                            </asp:LinkButton>
                        </span>
                        <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPaging" runat="server" CommandArgument='<%# Eval("Index") %>'
                                    CommandName="ChangePage" Text='<%# Eval("Index") %>' CssClass="postback">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:LinkButton ID="lnkNext" CssClass="right-arrow alignright postback" runat="server"
                            OnClick="lnkNext_Click" ToolTip="Next">
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <% if (Request.IsLocal)
           { %>
    </div>
    <% }
           else
           { %>
    </section>
    <%} %>
    <%-- Start Add Employee Popup --%>
    <div class="popup-outer" id="add-Employee-Block" style="display: none;">
        <div class="popupInner">
            <div class="employee-pop-block">
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('User Management','Add New Employee')">
                    Help Video</a><a href="javascript:;" class="close-btn" id="aCloseAddEmployee">Close</a>
                <div class="employess-content emp-height">
                    <ul class="cart-category tabs tabnav cf">
                        <li class="active" tab-id="manualentry"><a href="javascript:;" id="lnkManualEntry"
                            title="Manual Entry"><em></em>Manual Entry</a></li>
                        <li tab-id="uploadexcel"><a href="javascript:;" id="lnkUploadExcelFile" title="Upload Excel File">
                            <em></em>Upload Excel File</a></li>
                    </ul>
                    <div class="employee-form current-tab manualentry tabcon">
                        <div>
                            <div class="basic-form cf">
                                <ul class="left-form cf">
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">First Name</span>
                                            <asp:TextBox ID="txtAddPopupFirstName" runat="server" TabIndex="7" CssClass="input-field-all first-field checkvalidation"
                                                MaxLength="200"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvAddPopupFirstName" runat="server" ControlToValidate="txtAddPopupFirstName"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please enter first name.">
                                            </asp:RequiredFieldValidator>
                                        </label>
                                    </li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Last Name</span>
                                            <asp:TextBox ID="txtAddPopupLastName" runat="server" TabIndex="8" CssClass="input-field-all first-field checkvalidation"
                                                MaxLength="200"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvAddPopupLastName" runat="server" ControlToValidate="txtAddPopupLastName"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please enter last name.">
                                            </asp:RequiredFieldValidator>
                                        </label>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Preferred Email</span>
                                            <asp:TextBox ID="txtAddPopupEmail" runat="server" TabIndex="9" CssClass="input-field-all checkvalidation"
                                                MaxLength="200"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvAddPopupEmail" runat="server" ControlToValidate="txtAddPopupEmail"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please enter preferred email.">
                                            </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revAddPopupEmail" runat="server" ControlToValidate="txtAddPopupEmail"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please enter valid email." ValidationExpression="">
                                            </asp:RegularExpressionValidator>
                                        </label>
                                    </li>
                                    <li class="alignright">
                                        <label>
                                            <a href="javascript:;" id="aAddPopupGeneratePassword" tabindex="11" class="generate-btn"
                                                title="generate">generate</a><span class="lbl-txt">Password</span>
                                            <asp:TextBox ID="txtAddPopupPassword" runat="server" TabIndex="10" CssClass="input-field-all checkvalidation"
                                                MaxLength="200"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvAddPopupPassword" runat="server" ControlToValidate="txtAddPopupPassword"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please enter password.">
                                            </asp:RequiredFieldValidator>
                                        </label>
                                    </li>
                                    <li class="clear">&nbsp;</li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Gender</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <asp:DropDownList ID="ddlAddPopupGender" runat="server" CssClass="default checkvalidation"
                                                TabIndex="11">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="rfvAddPopupGender" runat="server" ControlToValidate="ddlAddPopupGender"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please select gender." InitialValue="0">
                                            </asp:RequiredFieldValidator>--%>
                                        </span></li>
                                </ul>
                                <ul class="left-form cf">
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Employee ID</span>
                                            <asp:TextBox ID="txtAddPopupEmployeeID" runat="server" TabIndex="12" CssClass="input-field-all first-field"
                                                MaxLength="100"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="rfvAddPopupEmployeeID" runat="server" ControlToValidate="txtAddPopupEmployeeID"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please enter employee id.">
                                            </asp:RequiredFieldValidator>--%>
                                        </label>
                                    </li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Workgroup</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <asp:DropDownList ID="ddlAddPopupWorkGroup" runat="server" CssClass="default checkvalidation"
                                                TabIndex="13">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="rfvAddPopupWorkGroup" runat="server" ControlToValidate="ddlAddPopupWorkGroup"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please select workgroup." InitialValue="0">
                                            </asp:RequiredFieldValidator>--%>
                                        </span></li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Position</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <asp:DropDownList ID="ddlAddPopupPosition" runat="server" CssClass="default checkvalidation"
                                                TabIndex="14">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="rfvAddPopupPosition" runat="server" ControlToValidate="ddlAddPopupPosition"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please select position." InitialValue="0">
                                            </asp:RequiredFieldValidator>--%>
                                        </span></li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Date of Hire</span>
                                        </label>
                                        <div class="date-field">
                                            <asp:TextBox ID="txtAddPopupDateOfHire" runat="server" class="input-field-all first-field checkvalidation setDatePicker"
                                                TabIndex="15" MaxLength="10"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="rfvAddPopupDateOfHire" runat="server" ControlToValidate="txtAddPopupDateOfHire"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please enter date of hire.">
                                            </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Station</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <asp:DropDownList ID="ddlAddPopupBaseStation" runat="server" CssClass="default checkvalidation"
                                                TabIndex="16">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="rfvAddPopupBaseStation" runat="server" ControlToValidate="ddlAddPopupBaseStation"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please select station." InitialValue="0">
                                            </asp:RequiredFieldValidator>--%>
                                        </span></li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Issuance Policy</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <asp:DropDownList ID="ddlAddPopupIssuancePolicy" runat="server" CssClass="default checkvalidation"
                                                TabIndex="17">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="rfvAddPopupIssuancePolicy" runat="server" ControlToValidate="ddlAddPopupIssuancePolicy"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please select issuance policy status." InitialValue="0">
                                            </asp:RequiredFieldValidator>--%>
                                        </span></li>
                                </ul>
                                <ul class="left-form cf">
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">System Access</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <asp:DropDownList ID="ddlAddPopupSystemAccess" runat="server" CssClass="default checkvalidation"
                                                TabIndex="18">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvAddPopupSystemAccess" runat="server" ControlToValidate="ddlAddPopupSystemAccess"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please select system access." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span></li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">System Status</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <asp:DropDownList ID="ddlAddPopupSystemStatus" runat="server" CssClass="default checkvalidation"
                                                TabIndex="19">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvAddPopupSystemStatus" runat="server" ControlToValidate="ddlAddPopupSystemStatus"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddEmployee"
                                                ErrorMessage="Please select system status." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span></li>
                                </ul>
                            </div>
                            <div class="textcenter">
                                To send login email please go to Basic Tab in User Profile
                            </div>
                            <div class="emp-btn-block">
                                <a id="lbAddPopupCancel" href="javascript:;" tabindex="20" class="gray-btn">Cancel</a>
                                <asp:LinkButton ID="lbAddPopupAddEmployee" runat="server" TabIndex="21" CssClass="blue-btn submit"
                                    ValidationGroup="AddEmployee" call="AddEmployee" OnClick="lbAddPopupAddEmployee_Click">
                                        Add Employee
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="employee-form current-tab uploadexcel tabcon" style="display: none;">
                        <div>
                            <div class="upload-form">
                                <div class="upload-content">
                                    <div class="upload-txt">
                                        Drag or paste your Excel file here, or <a href="#" title="browse">browse</a> for
                                        a file to upload.</div>
                                    <div class="upload-file">
                                        <asp:FileUpload ID="fuUsers" runat="server" CssClass="checkvalidation" />
                                        <asp:RequiredFieldValidator ID="rqBulkAssetFile" runat="server" ControlToValidate="fuUsers"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="UploadUsers"
                                            ErrorMessage="Please upload excel file with user data."></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="emp-btn-block">
                                    <asp:LinkButton ID="lbDownloadExcel" runat="server" CssClass="blue-btn" Text="Download Template"
                                        OnClick="lbDownloadExcel_Click"></asp:LinkButton>
                                    <asp:LinkButton ID="lbUploadExcel" runat="server" CssClass="blue-btn submit" Text="Upload"
                                        ValidationGroup="UploadUsers" call="UploadUsers" OnClick="lbUploadExcel_Click"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- End Add Employee Popup --%>
    <%-- Start View Employee Popup --%>
    <div class="popup-outer" id="employee-Details-Block" style="display: none;">
        <div class="popupInner">
            <div class="message-popup employee-pop-block">
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('User Management','Employee Details')">
                    Help Video</a><a href="javascript:;" class="close-btn" id="aCloseEmployeeDetails">Close</a>
                <div class="message-content employess-content emp-height">
                    <ul class="order-links tabs tabnav cf">
                        <li class="active checkchanges" tab-id="basic"><a href="javascript:;" title="Basic">
                            <em></em>Basic</a></li>
                        <li class="checkchanges" tab-id="menuaccess"><a href="javascript:;" title="Menu Access">
                            <em></em>Menu Access</a></li>
                        <li class="checkchanges" tab-id="adminsettings"><a href="javascript:;" title="Admin Settings">
                            <em></em>Settings</a></li>
                        <li class="checkchanges" tab-id="reports"><a href="javascript:;" title="Reports"><em>
                        </em>Reports</a></li>
                        <li class="checkchanges last" tab-id="history"><a href="javascript:;" title="History">
                            <em></em>History</a></li>
                    </ul>
                    <div class="employee-form current-tab basic">
                        <div>
                            <div class="basic-form tab-content cf">
                                <ul class="left-form cf">
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">First Name</span>
                                            <asp:TextBox ID="txtBasicFirstName" runat="server" TabIndex="7" CssClass="input-field-all first-field checkvalidation"
                                                MaxLength="200"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvBasicFirstName" runat="server" ControlToValidate="txtBasicFirstName"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                ErrorMessage="Please enter first name.">
                                            </asp:RequiredFieldValidator>
                                        </label>
                                    </li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Last Name</span>
                                            <asp:TextBox ID="txtBasicLastName" runat="server" TabIndex="8" CssClass="input-field-all first-field checkvalidation"
                                                MaxLength="200"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvBasicLastName" runat="server" ControlToValidate="txtBasicLastName"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                ErrorMessage="Please enter first name.">
                                            </asp:RequiredFieldValidator>
                                        </label>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Preferred Email</span>
                                            <asp:TextBox ID="txtBasicEmail" runat="server" TabIndex="9" CssClass="input-field-all checkvalidation"
                                                MaxLength="200"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvBasicEmail" runat="server" ControlToValidate="txtBasicEmail"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                ErrorMessage="Please enter preferred email.">
                                            </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revBasicEmail" runat="server" ControlToValidate="txtBasicEmail"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                ErrorMessage="Please enter valid email." ValidationExpression="">
                                            </asp:RegularExpressionValidator>
                                        </label>
                                    </li>
                                    <li class="alignright">
                                        <label>
                                            <a href="javascript:;" id="aBasicGeneratePassword" tabindex="11" class="generate-btn"
                                                title="generate">generate</a><span class="lbl-txt">Password</span>
                                            <asp:TextBox ID="txtBasicPassword" runat="server" TabIndex="10" CssClass="input-field-all checkvalidation"
                                                MaxLength="200"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvBasicPassWord" runat="server" ControlToValidate="txtBasicPassword"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                ErrorMessage="Please enter password.">
                                            </asp:RequiredFieldValidator>
                                        </label>
                                    </li>
                                    <li class="clear">&nbsp</li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Gender</span></label><span class="select-drop medium-drop">
                                                <asp:DropDownList ID="ddlBasicGender" runat="server" CssClass="default checkvalidation"
                                                    TabIndex="11">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvBasicGender" runat="server" ControlToValidate="ddlBasicGender"
                                                    Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                    ErrorMessage="Please select gender." InitialValue="0">
                                                </asp:RequiredFieldValidator>
                                            </span></li>
                                </ul>
                                <ul class="left-form cf">
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Employee ID</span>
                                            <asp:TextBox ID="txtBasicEmployeeID" runat="server" TabIndex="12" CssClass="input-field-all first-field"
                                                MaxLength="100"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="rfvBasicEmployeeID" runat="server" ControlToValidate="txtBasicEmployeeID"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                ErrorMessage="Please enter employee id.">
                                            </asp:RequiredFieldValidator>--%>
                                        </label>
                                    </li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Workgroup</span>
                                        </label>
                                        <label>
                                            <span class="select-drop medium-drop">
                                                <asp:DropDownList ID="ddlBasicWorkGroup" runat="server" CssClass="default checkvalidation"
                                                    TabIndex="13">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvBasicWorkGroup" runat="server" ControlToValidate="ddlBasicWorkGroup"
                                                    Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                    ErrorMessage="Please select workgroup." InitialValue="0">
                                                </asp:RequiredFieldValidator>
                                            </span>
                                        </label>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Position</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <asp:DropDownList ID="ddlBasicPosition" runat="server" CssClass="default checkvalidation"
                                                TabIndex="14">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="rfvBasicPosition" runat="server" ControlToValidate="ddlBasicPosition"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                ErrorMessage="Please select position." InitialValue="0">
                                            </asp:RequiredFieldValidator>--%>
                                        </span></li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Date of Hire</span>
                                        </label>
                                        <div class="date-field">
                                            <asp:TextBox ID="txtBasicDateOfHire" runat="server" class="input-field-all first-field checkvalidation setDatePicker"
                                                TabIndex="15" MaxLength="10"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvBasicDateOfHire" runat="server" ControlToValidate="txtBasicDateOfHire"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                ErrorMessage="Please enter date of hire.">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Station</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <asp:DropDownList ID="ddlBasicBaseStation" runat="server" CssClass="default checkvalidation"
                                                TabIndex="16">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvBasicBaseStation" runat="server" ControlToValidate="ddlBasicBaseStation"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                ErrorMessage="Please select station." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span></li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Issuance Policy</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <asp:DropDownList ID="ddlBasicIssuancePolicy" runat="server" CssClass="default checkvalidation"
                                                TabIndex="17">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvBasicIssuancePolicy" runat="server" ControlToValidate="ddlBasicIssuancePolicy"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                ErrorMessage="Please select issuance policy status." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span></li>
                                </ul>
                                <ul class="left-form cf">
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">System Access</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <asp:DropDownList ID="ddlBasicSystemAccess" runat="server" CssClass="default checkvalidation"
                                                TabIndex="18">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvBasicSystemAccess" runat="server" ControlToValidate="ddlBasicSystemAccess"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                ErrorMessage="Please select system access." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span></li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">System Status</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <asp:DropDownList ID="ddlBasicSystemStatus" runat="server" CssClass="default checkvalidation"
                                                TabIndex="19">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvBasicSystemStatus" runat="server" ControlToValidate="ddlBasicSystemStatus"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Basic"
                                                ErrorMessage="Please select system status." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span></li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Request Date</span>
                                            <asp:TextBox ID="txtBasicRequestDate" runat="server" TabIndex="20" CssClass="input-field-all"></asp:TextBox>
                                        </label>
                                    </li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Activation Date</span>
                                            <asp:TextBox ID="txtBasicActivationDate" runat="server" TabIndex="21" CssClass="input-field-all"></asp:TextBox>
                                        </label>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Activated By</span>
                                            <asp:TextBox ID="txtBasicActivatedBy" runat="server" TabIndex="22" CssClass="input-field-all full-width"></asp:TextBox>
                                        </label>
                                    </li>
                                </ul>
                            </div>
                            <div class="asset-btn-block assetbtn-blockbar cf">
                                <a id="aBasicCancel" class="small-gray-btn" href="javascript:;" title="Cancel changes and Close">
                                    <span>CANCEL</span> </a>
                                <asp:LinkButton ID="lbBasicSave" CssClass="small-blue-btn submit" ToolTip="Save and Move to Next Tab"
                                    ValidationGroup="Basic" call="Basic" runat="server" OnClick="lbBasicSave_Click">
                                    <span>SAVE</span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="lbBasicClose" runat="server" CssClass="small-gray-btn submit"
                                    ValidationGroup="Basic" call="Basic" ToolTip="Save changes and Close" OnClick="lbBasicClose_Click">
                                    <span>CLOSE</span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="lbBasicSendEmail" CssClass="small-blue-btn submit" ToolTip="Send Email with Login Information"
                                    ValidationGroup="Basic" call="Basic" runat="server" OnClick="lbBasicSendEmail_Click">
                                    <span>SEND LOGIN</span>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="employee-form current-tab menuaccess" style="display: none;">
                        <div>
                            <div class="employee-payment cf">
                                <ul class="emp-left tabs">
                                    <li class="active" tab-id="paymentoptions"><a href="javascript:;" title="Payment Options">
                                        Payment Options</a></li>
                                    <li tab-id="productcategories"><a href="javascript:;" title="Product Categories">Product
                                        Categories</a></li>
                                    <li tab-id="shippingoptions"><a href="javascript:;" title="Shipping Options">Shipping
                                        Options</a></li>
                                    <li tab-id="manageemails"><a href="javascript:;" title="Manage Emails">Manage Emails</a></li>
                                </ul>
                                <div class="emp-right current-tab paymentoptions">
                                    <div class="emp-title">
                                        Payment Options</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <asp:Repeater ID="repPaymentOptions" runat="server" OnItemDataBound="repPaymentOptions_ItemDataBound">
                                                <ItemTemplate>
                                                    <li class="lable-block">
                                                        <label class="label_checkbox">
                                                            <asp:CheckBox ID="chkMOASIssuancePurchase" runat="server" CssClass="icheckbox_flat"
                                                                Visible="false" /><asp:Literal Visible="false" runat="server" ID="ltrlMOASIssuancePurchase"></asp:Literal>
                                                        </label>
                                                        <label class="label_checkbox">
                                                            <asp:CheckBox ID="chkPaymentOption" runat="server" CssClass="icheckbox_flat" /><asp:Literal
                                                                runat="server" ID="ltrlPaymentOption"></asp:Literal>
                                                            <asp:HiddenField ID="hdnPaymentOptionID" runat="server" Value='<%# Eval("PaymentOptionID") %>' />
                                                        </label>
                                                        <asp:Panel ID="pnlMOASApprovers" runat="server" Visible="false">
                                                            <div class="emp-sub-title parentdiv">
                                                                MOAS Approvers <span class="down-arrow">&nbsp;</span></div>
                                                            <ul class="check-header childdiv cf" style="display: none;">
                                                                <div class="input-textarea" style="">
                                                                    <asp:DataList ID="dlMOASApprovers" runat="server" RepeatColumns="2" RepeatDirection="Vertical">
                                                                        <ItemTemplate>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <asp:CheckBox ID="chkApprover" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsActive")) %>'
                                                                                        CssClass="icheckbox_flat" /><%# Eval("FirstName") + " " + Eval("LastName") %>
                                                                                    <asp:HiddenField ID="hdnApproverID" runat="server" Value='<%# Eval("ApproverID") %>' />
                                                                                </label>
                                                                                <asp:TextBox ID="txtApproverLevel" Width="12" MaxLength="2" CssClass="input-field-small priority onlynumber alignright"
                                                                                    runat="server" Text='<%# Eval("ApproverLevel") %>'></asp:TextBox>
                                                                            </li>
                                                                        </ItemTemplate>
                                                                    </asp:DataList>
                                                                </div>
                                                            </ul>
                                                        </asp:Panel>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>
                                <div class="emp-right current-tab productcategories" style="display: none;">
                                    <div class="emp-title">
                                        Product Categories</div>
                                    <div class="MenuAccessScrollbar">
                                        <asp:Repeater ID="repCategories" runat="server" OnItemDataBound="repCategories_ItemDataBound">
                                            <ItemTemplate>
                                                <div>
                                                    <asp:HiddenField ID="hdnCategoryID" runat="server" Value='<%# Eval("CategoryID") %>' />
                                                    <div class="emp-sub-title parentdiv">
                                                        <%# Eval("CategoryName") %>
                                                        <span class="down-arrow">&nbsp;</span></div>
                                                    <ul class="check-header childdiv cf" style="display: none;">
                                                        <asp:DataList ID="dlSubCategories" runat="server" RepeatColumns="2" RepeatDirection="Vertical">
                                                            <ItemTemplate>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <asp:CheckBox ID="chkSubCategory" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsActive")) %>'
                                                                            CssClass="icheckbox_flat" /><%# Eval("SubCategoryName")%>
                                                                        <asp:HiddenField ID="hdnSubCategoryID" runat="server" Value='<%# Eval("SubCategoryID") %>' />
                                                                    </label>
                                                                </li>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </ul>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                                <div class="emp-right current-tab shippingoptions" style="display: none;">
                                    <div class="emp-title">
                                        Shipping Options</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <asp:Repeater ID="repShippingMethods" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <label class="label_checkbox">
                                                            <asp:CheckBox ID="chkShippingMethod" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsActive")) %>'
                                                                CssClass="icheckbox_flat" /><%# Eval("sLookupName")%>
                                                            <asp:HiddenField ID="hdnShippingMethodID" runat="server" Value='<%# Eval("iLookupID") %>' />
                                                        </label>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>
                                <div class="emp-right current-tab manageemails" style="display: none;">
                                    <div class="emp-title">
                                        Manage Emails</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <asp:Repeater ID="repManageEmails" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <label class="label_checkbox">
                                                            <asp:CheckBox ID="chkManageEmail" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsActive")) %>'
                                                                CssClass="icheckbox_flat" /><%# Eval("ManageEmailName") %>
                                                            <asp:HiddenField ID="hdnManageEmailID" runat="server" Value='<%# Eval("ManageEmailID") %>' />
                                                        </label>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="asset-btn-block assetbtn-blockbar cf">
                                <a id="aMenuAccessCancel" class="small-gray-btn" href="javascript:;"><span>Cancel</span>
                                </a>
                                <asp:LinkButton ID="lbMenuAccessSave" runat="server" TabIndex="23" OnClick="lbMenuAccessSave_Click"
                                    CssClass="small-blue-btn"><span>SAVE</span></asp:LinkButton>
                                <asp:LinkButton ID="lbMenuAccessClose" runat="server" TabIndex="24" OnClick="lbMenuAccessClose_Click"
                                    CssClass="small-gray-btn" ToolTip="Close"><span>Close</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="employee-form current-tab adminsettings" style="display: none;">
                        <div id="dvAdminSettingsForEmployee" runat="server">
                            <label class="tab-center">
                                This tab is disabled for employees other than the Company Admins.
                            </label>
                        </div>
                        <div id="dvAdminSettingsForAdmins" runat="server">
                            <div class="employee-payment cf">
                                <ul class="emp-left tabs">
                                    <li class="active checkchanges tab-section" tab-id="stationmanagement"><a href="javascript:;"
                                        title="Station Management">Station Management</a> </li>
                                    <li class="checkchanges tab-section" tab-id="workgroupmanagement"><a href="javascript:;"
                                        title="Workgroup Management">Workgroup Management</a> </li>
                                    <li class="checkchanges tab-section" tab-id="privileges"><a href="javascript:;" title="Privileges">
                                        Privileges</a> </li>
                                </ul>
                                <div class="emp-right current-tab stationmanagement">
                                    <div class="emp-title">
                                        Station Management</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <asp:DataList ID="dlStations" runat="server" RepeatColumns="2" RepeatDirection="Vertical">
                                                <ItemTemplate>
                                                    <li>
                                                        <label class="label_checkbox">
                                                            <asp:CheckBox ID="chkStation" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsActive")) %>'
                                                                CssClass="icheckbox_flat" /><%# Eval("Station") %>
                                                            <asp:HiddenField ID="hdnStationID" runat="server" Value='<%# Eval("StationID") %>' />
                                                        </label>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ul>
                                    </div>
                                </div>
                                <div class="emp-right current-tab workgroupmanagement" style="display: none;">
                                    <div class="emp-title">
                                        Workgroup Management</div>
                                    <div class="MenuAccessScrollbar" id="workgroupmanagementcontainer">
                                        <ul class="check-header cf">
                                            <asp:DataList ID="dlStationWorkGroups" runat="server" RepeatColumns="2" RepeatDirection="Vertical"
                                                OnItemDataBound="dlStationWorkGroups_ItemDataBound">
                                                <ItemTemplate>
                                                    <li class="dmsmainlist-title">
                                                        <label class="lbl-txt">
                                                            <%# Eval("Station") %></label>
                                                        <asp:HiddenField ID="hdnUserStationID" runat="server" Value='<%# Eval("UserStationID") %>' />
                                                        <msd:MultiSelectDropDown ID="msdStationWorkGroups" runat="server" />
                                                    </li>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ul>
                                    </div>
                                </div>
                                <div class="emp-right current-tab privileges" style="display: none;">
                                    <div class="emp-title">
                                        Privileges</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <asp:DataList ID="dlStationPrivileges" runat="server" RepeatColumns="2" RepeatDirection="Vertical"
                                                OnItemDataBound="dlStationPrivileges_ItemDataBound">
                                                <ItemTemplate>
                                                    <li class="dmsmainlist-title">
                                                        <label class="lbl-txt">
                                                            <%# Eval("Station") %></label>
                                                        <asp:HiddenField ID="hdnUserStationID" runat="server" Value='<%# Eval("UserStationID") %>' />
                                                        <msd:MultiSelectDropDown ID="msdStationPrivileges" runat="server" />
                                                    </li>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="asset-btn-block assetbtn-blockbar cf">
                                <a id="aAdminSettingsCancel" class="small-gray-btn" href="javascript:;"><span>Cancel</span>
                                </a>
                                <asp:LinkButton ID="lbAdminSettingsSave" runat="server" TabIndex="23" OnClick="lbAdminSettingsSave_Click"
                                    CssClass="small-blue-btn postback"><span>SAVE</span></asp:LinkButton>
                                <asp:LinkButton ID="lbAdminSettingsClose" runat="server" TabIndex="24" OnClick="lbAdminSettingsClose_Click"
                                    CssClass="small-gray-btn postback" ToolTip="Close"><span>Close</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="employee-form current-tab reports" style="display: none;">
                        <div id="dvReportsForEmployee" runat="server">
                            <label class="tab-center">
                                This tab is disabled for employees other than the Company Admins.
                            </label>
                        </div>
                        <div id="dvReportsForAdmins" runat="server">
                            <div class="employee-payment cf">
                                <ul class="emp-left tabs">
                                    <li class="active checkchanges tab-section" tab-id="mainreports"><a href="javascript:;"
                                        title="Reports">Reports</a> </li>
                                    <li class="checkchanges tab-section" tab-id="subreports"><a href="javascript:;" title="Sub Reports">
                                        Sub Reports</a> </li>
                                    <%--<li class="checkchanges tab-section" tab-id="stores"><a href="javascript:;" title="Stores">
                                        Stores</a> </li>--%>
                                    <li class="checkchanges tab-section" tab-id="workgroups"><a href="javascript:;" title="Workgroups">
                                        Workgroups</a> </li>
                                    <li class="checkchanges tab-section" tab-id="stations"><a href="javascript:;" title="Stations">
                                        Stations</a> </li>
                                    <li class="checkchanges tab-section" tab-id="pricelevels"><a href="javascript:;"
                                        title="Price Levels">Price Levels</a> </li>
                                </ul>
                                <div class="emp-right current-tab mainreports">
                                    <div class="emp-title">
                                        Reports</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <asp:DataList ID="dlReports" runat="server" RepeatColumns="2" RepeatDirection="Vertical">
                                                <ItemTemplate>
                                                    <li>
                                                        <label class="label_checkbox">
                                                            <asp:CheckBox ID="chkReport" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsActive")) %>'
                                                                CssClass="icheckbox_flat" /><%# Eval("Report") %>
                                                            <asp:HiddenField ID="hdnReportID" runat="server" Value='<%# Eval("ReportID") %>' />
                                                        </label>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ul>
                                    </div>
                                </div>
                                <div class="emp-right current-tab subreports" style="display: none;">
                                    <div class="emp-title">
                                        Sub Reports</div>
                                    <div class="MenuAccessScrollbar" id="subreportscontainer">
                                        <ul class="check-header cf">
                                            <asp:DataList ID="dlSubReports" runat="server" RepeatColumns="2" RepeatDirection="Vertical"
                                                OnItemDataBound="dlSubReports_ItemDataBound">
                                                <ItemTemplate>
                                                    <li class="dmsmainlist-title">
                                                        <label class="lbl-txt">
                                                            <%# Eval("Report")%></label>
                                                        <asp:HiddenField ID="hdnUserReportID" runat="server" Value='<%# Eval("UserReportID") %>' />
                                                        <msd:MultiSelectDropDown ID="msdSubReports" runat="server" />
                                                    </li>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ul>
                                    </div>
                                </div>
                                <div class="emp-right current-tab workgroups" style="display: none;">
                                    <div class="emp-title">
                                        Workgroups</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <asp:DataList ID="dlReportWorkGroups" runat="server" RepeatColumns="2" RepeatDirection="Vertical"
                                                OnItemDataBound="dlReportWorkGroups_ItemDataBound">
                                                <ItemTemplate>
                                                    <li class="dmsmainlist-title">
                                                        <label class="lbl-txt">
                                                            <%# Eval("Report") %></label>
                                                        <asp:HiddenField ID="hdnUserReportID" runat="server" Value='<%# Eval("UserReportID") %>' />
                                                        <msd:MultiSelectDropDown ID="msdReportWorkGroups" runat="server" />
                                                    </li>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ul>
                                    </div>
                                </div>
                                <div class="emp-right current-tab stations" style="display: none;">
                                    <div class="emp-title">
                                        Stations</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <asp:DataList ID="dlReportStations" runat="server" RepeatColumns="2" RepeatDirection="Vertical"
                                                OnItemDataBound="dlReportStations_ItemDataBound">
                                                <ItemTemplate>
                                                    <li class="dmsmainlist-title">
                                                        <label class="lbl-txt">
                                                            <%# Eval("Report") %></label>
                                                        <asp:HiddenField ID="hdnUserReportID" runat="server" Value='<%# Eval("UserReportID") %>' />
                                                        <msd:MultiSelectDropDown ID="msdReportStations" runat="server" />
                                                    </li>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ul>
                                    </div>
                                </div>
                                <div class="emp-right current-tab pricelevels" style="display: none;">
                                    <div class="emp-title">
                                        Price Levels</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <asp:DataList ID="dlReportPriceLevels" runat="server" RepeatColumns="2" RepeatDirection="Vertical"
                                                OnItemDataBound="dlReportPriceLevels_ItemDataBound">
                                                <ItemTemplate>
                                                    <li class="dmsmainlist-title">
                                                        <label class="lbl-txt">
                                                            <%# Eval("Report") %></label>
                                                        <asp:HiddenField ID="hdnUserReportID" runat="server" Value='<%# Eval("UserReportID") %>' />
                                                        <msd:MultiSelectDropDown ID="msdReportPriceLevels" runat="server" />
                                                    </li>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="asset-btn-block assetbtn-blockbar cf">
                                <a id="aReportsCancel" class="small-gray-btn" href="javascript:;"><span>Cancel</span>
                                </a>
                                <asp:LinkButton ID="lbReportsSave" runat="server" TabIndex="23" OnClick="lbReportsSave_Click"
                                    CssClass="small-blue-btn postback"><span>SAVE</span></asp:LinkButton>
                                <asp:LinkButton ID="lbReportsClose" runat="server" TabIndex="24" OnClick="lbReportsClose_Click"
                                    CssClass="small-gray-btn postback" ToolTip="Close"><span>Close</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="message-container current-tab history" style="display: none;">
                        <span class="top-bg">&nbsp;</span>
                        <div id="boxscroll">
                            <div class="pop-message-listing">
                                <ul class="cf">
                                    <li class="customer-chat">
                                        <h5 class="cf">
                                            <span>Status Edited by Jane Doe</span><em>Thu, Jan 21, 2013, 8:11 am</em></h5>
                                        <p>
                                            Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus
                                            mus. Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum.
                                            Natoque penatibus et magnis dis parturient montes,
                                        </p>
                                    </li>
                                    <li class="customer-chat">
                                        <h5 class="cf">
                                            <span>Status Edited by Jane Doe</span><em>Thu, Jan 21, 2013, 8:11 am</em></h5>
                                        <p>
                                            Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus
                                            mus. Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum.
                                            Natoque penatibus et magnis dis parturient montes,
                                        </p>
                                    </li>
                                    <li class="customer-chat">
                                        <h5 class="cf">
                                            <span>Status Edited by Jane Doe</span><em>Thu, Jan 21, 2013, 8:11 am</em></h5>
                                        <p>
                                            Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus
                                            mus. Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum.
                                            Natoque penatibus et magnis dis parturient montes,
                                        </p>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="pop-message-post">
                            <label>
                                <span>Add a Note:</span><input type="text" class="default_title_text input-popup"
                                    placeholder="Typed text" title="Typed text"></label>
                            <button class="blue-btn">
                                Send</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- End View Employee Popup --%>
</asp:Content>
