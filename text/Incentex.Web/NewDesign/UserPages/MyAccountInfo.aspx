<%@ Page Title="My Account Information" Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master"
    AutoEventWireup="true" CodeFile="MyAccountInfo.aspx.cs" Inherits="UserPages_MyAccountInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $().ready(function() {
            $(window).ValidationUI();
            MainCurrentDisplay();
        });
        function MainCurrentDisplay() {
            $(".leftNav").removeClass("active");
            $(".current-tab").css('display', 'none');
            var liActive = $("#ctl00_ContentPlaceHolder1_liActive").val();
            var dvActive = $("#ctl00_ContentPlaceHolder1_dvDisplay").val();
            $("#" + liActive).addClass("active");
            $("#" + dvActive).css('display', 'block');
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <section id="container" class="cf">
    <div class="narrowcolumn alignleft">
        <div class="subNav-block">
            <h2 class="title-txt">
                Account</h2>
            <ul class="cf tabs tabnav structdiff" id="subNavigation">
                <li id="liBasicInfo" runat="server" tab-id="BasicInfo" class="leftNav checkchanges"><a href="javascript:;" title="Basic Information">Basic Information</a></li>
                <li id="liMyAddressInfo" runat="server" tab-id="MyAddressInfo" class="leftNav checkchanges"><a href="javascript:;" title="My Address Book">My Address Book</a></li>
                <li id="liPasswordInfo" runat="server" tab-id="PasswordInfo" class="leftNav checkchanges"><a href="javascript:;" title="Change Password">Change Password</a></li>
            </ul>
        </div>
    </div>
    <div class="widecolumn alignright">
        <div class="account-form-block">
            <div id="dvBasicInfo" runat="server" class="tabcon current-tab account-formInner BasicInfo">
                <h2 class="title-txt">
                    Basic Information</h2>
                <div class="basic-form">
                    <div class="cf">
                        <ul class="left-form cf">
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">First Name*</span>
                                    <asp:TextBox ID="txtFirstName" runat="server" TabIndex="1" class="input-field-all first-field checkvalidation"
                                        placeholder="First"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBasicInfo"
                                        ErrorMessage="Please enter first name"></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            <li class="alignright">
                                <label>
                                    <span class="lbl-txt">Last Name*</span>
                                    <asp:TextBox ID="txtLastName" runat="server" TabIndex="2" class="input-field-all first-field checkvalidation"
                                        placeholder="Last"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqvLastName" runat="server" ControlToValidate="txtLastName"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBasicInfo"
                                        ErrorMessage="Please enter last name"></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">Company</span><asp:TextBox ID="txtCompany" runat="server" TabIndex="3"
                                        class="input-field-all checkvalidation" placeholder="Company Name"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqvCompany" runat="server" ControlToValidate="txtCompany"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBasicInfo"
                                        ErrorMessage="Please enter Company name"></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            <li class="alignright">
                                <label>
                                    <span class="lbl-txt">Employee ID</span><asp:TextBox ID="txtEmployeeID" runat="server"
                                        TabIndex="4" class="input-field-all disable-txt checkvalidation" placeholder="Employee ID"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqvEmployeeID" runat="server" ControlToValidate="txtEmployeeID"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBasicInfo"
                                        ErrorMessage="Please enter first name"></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">Base Station</span><asp:TextBox ID="txtBaseStation" runat="server"
                                        TabIndex="5" class="input-field-all checkvalidation" placeholder="Station"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqvBaseStation" runat="server" ControlToValidate="txtBaseStation"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBasicInfo"
                                        ErrorMessage="Please enter BaseStation"></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            <li class="alignright">
                                <label>
                                    <span class="lbl-txt">Workgroup</span><asp:TextBox ID="txtWorkGroup" runat="server"
                                        TabIndex="6" class="input-field-all disable-txt checkvalidation" placeholder="not provided"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqvWorkgroup" runat="server" ControlToValidate="txtWorkGroup"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBasicInfo"
                                        ErrorMessage="Please enter WorkGroup name"></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">Email*</span><asp:TextBox ID="txtEmail" runat="server" TabIndex="7"
                                        class="input-field-all full-width checkvalidation" placeholder="abc@xyz.com"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqvEmail" runat="server" ControlToValidate="txtEmail"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBasicInfo"
                                        ErrorMessage="Please enter email"></asp:RequiredFieldValidator>
                                </label>
                            </li>
                        </ul>
                        <div class="save-block">
                            <asp:LinkButton ID="lnkSaveMainAccInfo" runat="server" class="save-btn" OnClick="lnkSaveMainAccInfo_Click"
                                ValidationGroup="SaveBasicInfo" TabIndex="8">SAVE CHANGES</asp:LinkButton></div>
                    </div>
                </div>
            </div>
            <div id="dvMyAddressInfo" runat="server" class="tabcon current-tab MyAddressInfo" style="display: none;">
                <div class="account-formInner">
                    <h2 class="title-txt">
                        Add New Address</h2>
                    <div class="basic-form">
                        <asp:Panel ID="pnlNewAddress" runat="server" CssClass="cf">
                            <ul class="left-form cf">
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">First Name*</span><asp:TextBox ID="txtNewFirstName" runat="server"
                                            TabIndex="1" class="input-field-all first-field checkvalidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rvNewFirstName" runat="server" ControlToValidate="txtNewFirstName"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveNewAddressInfo"
                                            ErrorMessage="Please enter first name"></asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt">Last Name*</span><asp:TextBox ID="txtNewLastName" runat="server"
                                            TabIndex="2" class="input-field-all first-field checkvalidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rvNewLastName" runat="server" ControlToValidate="txtNewLastName"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveNewAddressInfo"
                                            ErrorMessage="Please enter last name"></asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Company</span>
                                        <asp:TextBox ID="txtNewCompanyName" runat="server" TabIndex="3" class="input-field-all checkvalidation"></asp:TextBox>
                                        <ajaxToolkit:AutoCompleteExtender ServicePath="MyAccountInfo.aspx" runat="server"
                                            ID="acNewCompany" TargetControlID="txtNewCompanyName" ServiceMethod="GetCompanyList"
                                            MinimumPrefixLength="1" CompletionInterval="1000" EnableCaching="true" CompletionSetCount="12" />
                                        <asp:RequiredFieldValidator ID="rqNewCompanyName" runat="server" ControlToValidate="txtNewCompanyName"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveNewAddressInfo"
                                            ErrorMessage="Please enter Company Name"></asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt1">Station Code*</span></label><span class="select-drop source-drop"><asp:DropDownList
                                            ID="ddlNewBaseStation" runat="server" class="default checkvalidation" TabIndex="4">
                                        </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rqNewBaseStation" runat="server" ControlToValidate="ddlNewBaseStation"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveNewAddressInfo"
                                                ErrorMessage="Please select Base Station." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span></li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Address *</span><asp:TextBox ID="txtNewAddress" runat="server"
                                            TabIndex="5" class="input-field-all full-width checkvalidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqNewAddress" runat="server" ControlToValidate="txtNewAddress"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveNewAddressInfo"
                                            ErrorMessage="Please enter Address"></asp:RequiredFieldValidator></label></li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Suite/Apt #</span><asp:TextBox ID="txtNewSuiteApt" runat="server"
                                            TabIndex="6" class="input-field-all checkvalidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqNewSuiteApt" runat="server" ControlToValidate="txtNewSuiteApt"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveNewAddressInfo"
                                            ErrorMessage="Please enter Suite Apt"></asp:RequiredFieldValidator></label></li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt">City*</span><asp:TextBox ID="txtNewCity" runat="server" TabIndex="7"
                                            class="input-field-all checkvalidation"></asp:TextBox>
                                        <ajaxToolkit:AutoCompleteExtender ServicePath="MyAccountInfo.aspx" runat="server"
                                            ID="acNewCity" TargetControlID="txtNewCity" ServiceMethod="GetCityList" CompletionInterval="1000"
                                            EnableCaching="true" CompletionSetCount="12" MinimumPrefixLength="1" ContextKey="0" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEmail"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveNewAddressInfo"
                                            ErrorMessage="Please enter email"></asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">State*</span><span class="select-drop source-drop"><asp:DropDownList
                                            ID="ddlNewState" runat="server" class="default checkvalidation" TabIndex="8" OnSelectedIndexChanged="ddlNewState_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rqddlNewState" runat="server" ControlToValidate="ddlNewState"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveNewAddressInfo"
                                                ErrorMessage="Please select state." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span>
                                    </label>
                                </li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt">Zip Code*</span><asp:TextBox ID="txtNewZipCode" runat="server"
                                            TabIndex="9" class="input-field-all checkvalidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqNewZipCode" runat="server" ControlToValidate="txtNewZipCode"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveNewAddressInfo"
                                            ErrorMessage="Please enter Zip Code"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revNewZipCode" runat="server" ControlToValidate="txtNewZipCode"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveNewAddressInfo"
                                            ErrorMessage="Please enter valid zip code.">
                                        </asp:RegularExpressionValidator>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Country*</span></label><span class="select-drop country-drop"><asp:DropDownList
                                            ID="ddlNewAddressCountry" runat="server" class="default checkvalidation" TabIndex="10" OnSelectedIndexChanged="ddlNewAddressCountry_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rqNewAddressCountry" runat="server" ControlToValidate="ddlNewAddressCountry"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveNewAddressInfo"
                                                ErrorMessage="Please select Address Country." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span></li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt2">Save to Address Book As*</span><asp:TextBox ID="txtNewAddressSave"
                                            runat="server" TabIndex="11" class="input-field-all full-width checkvalidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqNewAddressSave" runat="server" ControlToValidate="txtNewAddressSave"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveNewAddressInfo"
                                            ErrorMessage="Please enter Address Save as"></asp:RequiredFieldValidator>
                                    </label>
                                </li>
                            </ul>
                            <div class="save-block">
                                <asp:LinkButton ID="lnkNewAddressAdd" runat="server" class="save-btn submit" OnClick="lnkNewAddressAdd_Click"
                                    TabIndex="12" ValidationGroup="SaveNewAddressInfo" call="SaveNewAddressInfo">ADD TO MY ADDRESS BOOK</asp:LinkButton></div>
                        </asp:Panel>
                    </div>
                </div>
                <div class="account-formInner">
                    <h2 class="title-txt">
                        My Address Book</h2>
                    <div class="basic-form">
                        <asp:Panel ID="pnlMyAddress" runat="server" CssClass="cf">
                            <ul class="left-form cf">
                                <li class="alignleft border-data">
                                    <label>
                                        <span class="lbl-txt1">Address Name</span></label><span class="select-drop country-drop"><asp:DropDownList
                                            ID="ddlMyAddressBookName" runat="server" class="default" TabIndex="13" OnSelectedIndexChanged="ddlMyAddressBookName_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                        </span></li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">First Name*</span><asp:TextBox ID="txtMyAddressFirstName" runat="server"
                                            TabIndex="14" class="input-field-all first-field checkvalidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqMyAddressFirstName" runat="server" ControlToValidate="txtMyAddressFirstName"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveMyAddressInfo"
                                            ErrorMessage="Please enter first name"></asp:RequiredFieldValidator></label></li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt">Last Name*</span><asp:TextBox ID="txtMyAddressLastName" runat="server"
                                            TabIndex="15" class="input-field-all first-field checkvalidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqMyAddressLastName" runat="server" ControlToValidate="txtMyAddressLastName"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveMyAddressInfo"
                                            ErrorMessage="Please enter last name"></asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Company</span>
                                        <asp:TextBox ID="txtMyAddressCompanyName" runat="server" TabIndex="16" class="input-field-all checkvalidation"></asp:TextBox>
                                        <ajaxToolkit:AutoCompleteExtender ServicePath="MyAccountInfo.aspx" runat="server"
                                            ID="acExistingCompanyName" TargetControlID="txtMyAddressCompanyName" ServiceMethod="GetCompanyList"
                                            MinimumPrefixLength="1" CompletionInterval="1000" EnableCaching="true" CompletionSetCount="12" />
                                        <asp:RequiredFieldValidator ID="rqMyAddressCompanyName" runat="server" ControlToValidate="txtMyAddressCompanyName"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveMyAddressInfo"
                                            ErrorMessage="Please enter company"></asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt1">Station Code*</span></label><span class="select-drop source-drop"><asp:DropDownList
                                            ID="ddlMyAddressBaseStation" runat="server" class="default checkvalidation" TabIndex="17">
                                        </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rqMyAddressBaseStation" runat="server" ControlToValidate="ddlMyAddressBaseStation"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveMyAddressInfo"
                                                ErrorMessage="Please select station code" InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span></li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Address *</span><asp:TextBox ID="txtMyAddressAdress" runat="server"
                                            TabIndex="19" class="input-field-all full-width checkvalidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rq" runat="server" ControlToValidate="txtMyAddressAdress"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveMyAddressInfo"
                                            ErrorMessage="Please enter address"></asp:RequiredFieldValidator></label></li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Suite/Apt #</span><asp:TextBox ID="txtMyAddressSuiteApt" runat="server"
                                            TabIndex="20" class="input-field-all checkvalidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqMyAddressSuiteApt" runat="server" ControlToValidate="txtMyAddressSuiteApt"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveMyAddressInfo"
                                            ErrorMessage="Please enter suite "></asp:RequiredFieldValidator></label></li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt">City*</span><asp:TextBox ID="txtMyAddressCity" runat="server"
                                            TabIndex="21" class="input-field-all checkvalidation"></asp:TextBox>
                                        <ajaxToolkit:AutoCompleteExtender ServicePath="MyAccountInfo.aspx" runat="server"
                                            ID="acMyAddressCity" TargetControlID="txtMyAddressCity" ServiceMethod="GetCityList"
                                            CompletionInterval="1000" EnableCaching="true" CompletionSetCount="12" MinimumPrefixLength="1"
                                            ContextKey="0" />
                                        <asp:RequiredFieldValidator ID="rqMyAddressCity" runat="server" ControlToValidate="txtMyAddressCity"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveMyAddressInfo"
                                            ErrorMessage="Please enter city"></asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">State*</span><span class="select-drop source-drop"><asp:DropDownList
                                            ID="ddlMyAddressState" runat="server" class="default" TabIndex="22" OnSelectedIndexChanged="ddlMyAddressState_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rqMyAddressState" runat="server" ControlToValidate="ddlMyAddressState"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveMyAddressInfo"
                                                ErrorMessage="Please select state." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span>
                                    </label>
                                </li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt">Zip Code*</span><asp:TextBox ID="txtMyAddressZipCode" runat="server"
                                            TabIndex="23" class="input-field-all checkvalidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqtxtMyAddressZipCode" runat="server" ControlToValidate="txtMyAddressZipCode"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveMyAddressInfo"
                                            ErrorMessage="Please enter zip code"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revMyAddressZipCode" runat="server" ControlToValidate="txtMyAddressZipCode"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveMyAddressInfo"
                                            ErrorMessage="Please enter valid zip code.">
                                        </asp:RegularExpressionValidator></label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Country*</span></label><span class="select-drop country-drop"><asp:DropDownList
                                            ID="ddlMyAddressCountry" runat="server" class="default checkvalidation" TabIndex="24" OnSelectedIndexChanged="ddlMyAddressCountry_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rqMyAddressCountry" runat="server" ControlToValidate="ddlMyAddressCountry"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveMyAddressInfo"
                                                ErrorMessage="Please select country." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span></li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt2">Save to Address Book As*</span><asp:TextBox ID="txtMyAddressBook"
                                            runat="server" TabIndex="25" class="input-field-all full-width checkvalidation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqMyAddressBook" runat="server" ControlToValidate="txtMyAddressBook"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveMyAddressInfo"
                                            ErrorMessage="Please enter save address as.">
                                        </asp:RequiredFieldValidator></label></li>
                            </ul>
                            <div class="two-btn-block">
                                <asp:LinkButton ID="lnkDeleteAddress" runat="server" class="infodel-btn" CausesValidation="false" OnClick="lnkDeleteAddress_Click">DELETE</asp:LinkButton><asp:LinkButton
                                    ID="lnkSaveNewMyAddress" runat="server" class="save-btn submit" OnClick="lnkSaveNewMyAddress_Click"
                                    ValidationGroup="SaveMyAddressInfo" call="SaveMyAddressInfo">SAVE CHANGES</asp:LinkButton></div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <div id="dvPasswordInfo" runat="server" class="tabcon current-tab account-formInner PasswordInfo" style="display: none;">
                <h2 class="title-txt">
                    Change Password</h2>
                <div class="basic-form" method="post">
                    <div class="cf">
                        <ul class="left-form cf">
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt1">Old Password*</span><asp:TextBox ID="txtOldPassword" runat="server"
                                        TabIndex="1" class="input-field-all full-width checkvalidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqvOldPassword" runat="server" ControlToValidate="txtOldPassword"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavenewPasswordInfo"
                                        ErrorMessage="Please enter Old Password"></asp:RequiredFieldValidator></label></li>
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt1">New Password*</span><asp:TextBox ID="txtNewPassword" runat="server"
                                        TabIndex="2" class="input-field-all full-width checkvalidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqvNewPassword" runat="server" ControlToValidate="txtNewPassword"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavenewPasswordInfo"
                                        ErrorMessage="Please enter New Password"></asp:RequiredFieldValidator></label></li>
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt2">Confirm New Password*</span><asp:TextBox ID="txtNewConfirmPassword"
                                        runat="server" TabIndex="3" class="input-field-all full-width checkvalidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqvConfirmPassword" runat="server" ControlToValidate="txtNewConfirmPassword"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavenewPasswordInfo"
                                        ErrorMessage="Please enter Confirm New Password"></asp:RequiredFieldValidator></label></li>
                        </ul>
                        <div class="save-block">
                            <asp:LinkButton ID="lnkSavePassword" runat="server" class="save-btn submit" OnClick="lnkSavePassword_Click"
                                ValidationGroup="SavenewPasswordInfo" call="SavenewPasswordInfo">SAVE NEW PASSWORD</asp:LinkButton></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </section>
     <input type="hidden" value="admin-link" id="hdnActiveLink" />
     <asp:HiddenField ID="liActive" runat="server" />
     <asp:HiddenField ID="dvDisplay" runat="server"/>
</asp:Content>
