<%@ Page Title="incentex | Login" Language="C#" MasterPageFile="~/NewDesign/LoginMaster.master"
    AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .error_field
        {
            border: 4px solid #FF0000;
            padding: 10px 11px 6px;
            width: 250px;
        }
    </style>
    <!-- For Validations-->

    <script type="text/javascript">
        function ShowLoginPopup(ControlID, fullname) {
            $("#" + ControlID).css('top', '0');
            $("#sucess-contents").show();
            $("#register-contents").hide();
            $( "#<%=newMemberName.ClientID %>").html(fullname);           
            SetPopUpAtTop();
        }

        $(document).ready(function() {
            $('input').iCheck({
                checkboxClass: 'icheckbox_flat'
                //radioClass: 'iradio_flat'
            });
            // Allow to change month and year
            $(".setDatePicker").datepicker({
                changeMonth: true,
                changeYear: true
            });

            // Window form validations
            $(window).ValidationUI();

            $("#<%= btnLogin.ClientID %>").click(function() {
                $("#<%= hdnOS.ClientID %>").val(navigator.userAgent);
                $("#<%= hdnResolution.ClientID %>").val(screen.width + "x" + screen.height);
            });
        });

        function redirectHttpToHttps() {
            if (document.URL.indexOf("https") < 0 && document.URL.indexOf("world-link.us.com") > 0) {
                window.location = document.URL.replace("http", "https");
            }
        }
        redirectHttpToHttps();
        function OpenVideo() {
            $("#ctl00_ctl00_ContentPlaceHolderMaster_ContentPlaceHolder1_iframeVideo").attr("src", "http://vimeo.com/user19227861/review/72414822/f9ce776c88");
        }
        function CloseVideo() {
            $("#ctl00_ctl00_ContentPlaceHolderMaster_ContentPlaceHolder1_iframeVideo").attr("src", '');
        }
    </script>

    <script type="text/javascript">
     var siteurl = '<%=ConfigurationSettings.AppSettings["NewDesignSiteurl"].ToString()%>';
      
    function GetHelpVideo(ModuleName,PageOrPupupName) {
    $.ajax({
        type: "POST",
        url: siteurl + "UserPages/WSUser.asmx/GetHelpVideoOrDoc",
        data: "{'Type':'Login Pop-up', 'ModuleName':'" + ModuleName + "','PageOrPopupTitle':'" + PageOrPupupName + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(msg) {
      
                        if(msg.d!="")
                        {
                        ShowHelpVideo(msg.d);
                        }
        },
        error: function(x, e) {
        }
     });
    }
    
    function ShowHelpVideo(VidUrl) {
         
            $("#help-video-block").css('top', '0');
            $(".fade-layer").show();
            $("#help-video-block").show();
            var videoURL =VidUrl;
            $("#HelpVideoIframe").attr("src", videoURL);
        }



  function CloseHelpVideo() {
           
            $("#help-video-block").hide();
            $("#HelpVideoIframe").attr("src", '');
        }
    </script>

    <script type="text/javascript">
  $(function() {
    var opt = null;
    var list = $( "#<%=hdnCompanyList.ClientID %>").val();
    opt = list.split(",").slice(0,-1);
    $( "#<%=txtCompany.ClientID %>" ).autocomplete({
      source: opt
    });
  });
    </script>

    <asp:HiddenField runat="server" ID="hdnUrlReferrer" />
    <asp:HiddenField runat="server" ID="hdnOS" />
    <asp:HiddenField runat="server" ID="hdnResolution" />
    <asp:HiddenField runat="server" ID="hdnCompanyList" />
    <div class="login-form">
        <label class="login-field">
            <span class="login-ico"></span>
            <asp:TextBox ID="txtEmail" runat="server" MaxLength="300" TabIndex="1" class="input-txt default_title_text"
                placeholder="Email"></asp:TextBox>
        </label>
        <label class="password-field">
            <span class="pass-ico"></span>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" TabIndex="2" class="input-txt default_title_text"
                placeholder="Password" title="Password"></asp:TextBox>
            <a href="NewDesign/forgotpassword.aspx" class="forgot-btn" title="forgot">Forgot</a></label>
        <asp:LinkButton ID="btnLogin" class="login-home-btn" runat="server" OnClick="btnLogin_Click"
            TabIndex="3"><span>Login</span></asp:LinkButton>
        <div class="check-box-block cf">
            <span class="error-msg fail-msg">
                <asp:Label ID="lblMessage" runat="server"></asp:Label></span>
            <label class="label_checkbox cf">
                <asp:CheckBox ID="chkRememberMe" runat="server" TabIndex="4" class="icheckbox_flat" />
                Remember me
            </label>
        </div>
        <div class="or-txt">
            OR</div>
        <a class="blue-home-btn popup-openlink" id="popup-register" href="javascript:void(0);">
            <span>Become a Member</span></a>
    </div>
    <!-- end footer -->
    <div class="popup-outer" id="video-block">
        <div class="popupInner">
            <div class="video-block">
                <a href="javascript:;" onclick="CloseVideo();" class="hide-popup">Close</a>
                <div class="video-player">
                    <iframe id="iframeVideo" runat="server" width="900" height="590" src="http://vimeo.com/user19227861/review/72414822/f9ce776c88"
                        frameborder="0"></iframe>
                </div>
            </div>
        </div>
    </div>
    <div class="popup-outer" id="term-block">
        <div class="popupInner">
            <div class="term-block">
                <a href="javascript:;" class="close-btn">Close</a>
                <div class="term-content">
                    <h2>
                        Terms of Service</h2>
                    <ol>
                        <li><span>Terms</span>
                            <p>
                                By accessing this web site, you are agreeing to be bound by these web site Terms
                                and Conditions of Use, all applicable laws and regulations, and agree that you are
                                responsible for compliance with any applicable local laws. If you do not agree with
                                any of these terms, you are prohibited from using or accessing this site. The materials
                                contained in this web site are protected by applicable copyright and trade mark
                                law.</p>
                        </li>
                        <li><span>Use License</span>
                            <ul>
                                <li>Permission is granted to temporarily download one copy of the materials (information
                                    or software) on Incentex's web site for personal, non-commercial transitory viewing
                                    only. This is the grant of a license, not a transfer of title, and under this license
                                    you may not:
                                    <ul>
                                        <li>modify or copy the materials;</li>
                                        <li>use the materials for any commercial purpose, or for any public display (commercial
                                            or non-commercial);</li>
                                        <li>attempt to decompile or reverse engineer any software contained on Incentex's web
                                            site;</li>
                                        <li>remove any copyright or other proprietary notations from the materials; or</li>
                                        <li>transfer the materials to another person or "mirror" the materials on any other
                                            server.</li>
                                    </ul>
                                </li>
                                <li>This license shall automatically terminate if you violate any of these restrictions
                                    and may be terminated by Incentex at any time. Upon terminating your viewing of
                                    these materials or upon the termination of this license, you must destroy any downloaded
                                    materials in your possession whether in electronic or printed format. </li>
                            </ul>
                        </li>
                    </ol>
                    <div class="term-btn-block">
                        <a href="javascript:;" class="gray-btn cancel-btn" title="Decline"><span>Decline</span></a>
                        <a href="javascript:;" class="blue-btn" title="Accept"><span>Accept</span></a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="popup-outer" id="register-block">
        <div class="popupInner">
            <a href="javascript:;" class="close-btn">Close</a> <a href="javascript: void(0);"
                class="help-video-btn popup-openlink" title="Help video" onclick="GetHelpVideo('Login Pop-up','Become a Member')">
                Help video</a>
            <div class="register-block" id="register-contents">
                <div class="register-content">
                    <div class="register-header cf">
                        <h2>
                            Become a Member</h2>
                    </div>
                    <p class="reg-txt">
                        Thank you for your interest in becoming a member of the Incentex global purchasing
                        network. Please take a minute and ﬁll out the information requested below.</p>
                    <div class="cf">
                        <ul class="left-form">
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">First Name</span>
                                    <asp:TextBox ID="txtFirstName" runat="server" class="input-field first-field checkvalidation"
                                        TabIndex="5"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Save"
                                        ErrorMessage="Please enter first name."></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            <li class="alignright">
                                <label>
                                    <span class="lbl-txt">Last Name</span>
                                    <asp:TextBox ID="txtLastName" runat="server" class="input-field first-field checkvalidation"
                                        TabIndex="6"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Save"
                                        ErrorMessage="Please enter last name."></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">Company</span>
                                    <asp:TextBox ID="txtCompany" runat="server" class="input-field checkvalidation" TabIndex="7"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="txtCompany"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Save"
                                        ErrorMessage="Please enter company name."></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            <li class="alignright">
                                <label>
                                    <span class="lbl-txt">Employee ID</span>
                                    <asp:TextBox ID="txtEmployeeID" runat="server" class="input-field" TabIndex="8"></asp:TextBox></label></li>
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">Station</span> <span class="select-drop medium-drop">
                                        <asp:DropDownList ID="ddlBaseStation" runat="server" class="checkvalidation default"
                                            TabIndex="9">
                                            <asp:ListItem Value="0" Text="- Station -"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvBaseStation" runat="server" ControlToValidate="ddlBaseStation"
                                            Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" ValidationGroup="Save"
                                            ErrorMessage="Please select station."></asp:RequiredFieldValidator>
                                    </span>
                                </label>
                            </li>
                            <li class="alignright">
                                <label>
                                    <span class="lbl-txt">Workgroup</span> <span class="select-drop medium-drop">
                                        <asp:DropDownList ID="ddlWorkgroup" runat="server" TabIndex="10" class="checkvalidation default">
                                            <asp:ListItem Value="0" Text="- Workgroup -"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvWorkGroup" runat="server" ControlToValidate="ddlWorkgroup"
                                            Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" ValidationGroup="Save"
                                            ErrorMessage="Please select workgroup."></asp:RequiredFieldValidator>
                                    </span>
                                </label>
                            </li>
                            <li class="alignleft">
                                <label>
                                    <%--Rename from "Employee Title" to "Position" as per Ken Request--%>
                                    <span class="lbl-txt">Position</span> <span class="select-drop medium-drop">
                                        <asp:DropDownList ID="ddlEmployeeTitle" runat="server" TabIndex="11" class="checkvalidation default">
                                            <asp:ListItem Value="0" Text="- Position -"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvPosition" runat="server" ControlToValidate="ddlEmployeeTitle"
                                            Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" ValidationGroup="Save"
                                            ErrorMessage="Please select position."></asp:RequiredFieldValidator>
                                    </span>
                                </label>
                            </li>
                            <li class="alignright">
                                <label>
                                    <span class="lbl-txt">Date of Hire</span>
                                    <asp:TextBox ID="txtDateOfHire" runat="server" class="input-field setDatePicker checkvalidation"
                                        TabIndex="11"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDateOfHire" runat="server" ControlToValidate="txtDateOfHire"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Save"
                                        ErrorMessage="Please enter date of hire."></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">Gender</span></label>
                                <span class="select-drop medium-drop">
                                    <asp:DropDownList ID="ddlGender" runat="server" class="checkvalidation default" TabIndex="12">
                                        <asp:ListItem Value="0" Text="- Gender -"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlGender"
                                        Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" ValidationGroup="Save"
                                        ErrorMessage="Please select gender."></asp:RequiredFieldValidator>
                                </span></li>
                            <li class="alignright">
                                <label>
                                    <span class="lbl-txt">Preferred Email</span>
                                    <asp:TextBox ID="txtEmailAddress" runat="server" class="input-field checkvalidation"
                                        TabIndex="13"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" ControlToValidate="txtEmailAddress"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Save"
                                        ErrorMessage="Please enter preferred email."></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revEmailAddress" Display="Dynamic" CssClass="error"
                                        runat="server" ControlToValidate="txtEmailAddress" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="Save" SetFocusOnError="True" ErrorMessage="Please enter valid preferred email."></asp:RegularExpressionValidator>
                                </label>
                            </li>
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt" style="line-height: 25px">Create Password</span>
                                    <asp:TextBox ID="txtPwdRegistration" runat="server" class="input-field" TabIndex="14"></asp:TextBox></label></li>
                        </ul>
                    </div>
                    <div class="reg-btn-block cf">
                        <asp:LinkButton ID="lnkSubmitRegistration" class="blue-home-btn sucess-link popup-openlink submit"
                            runat="server" OnClick="lnkSubmitRegistration_Click" TabIndex="15" ValidationGroup="Save"
                            call="Save"><span>Submit</span></asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="sucess-block" id="sucess-contents">
                <h2>
                    Submission Accepted</h2>
                <p>
                    <asp:Label ID="newMemberName" runat="server"></asp:Label>, your company has received
                    your membership request.</p>
                <br />
                <br />
                <p>
                    Once your information has been reviewed and approved by your company you will receive
                    an email with your login information. Please note that Incentex cannot approve your
                    membership access; it can only be approved by your company.</p>
                <div class="center-txt">
                    Thank you,<br />
                    <br />
                    Incentex Customer Care</div>
            </div>
        </div>
    </div>
</asp:Content>
