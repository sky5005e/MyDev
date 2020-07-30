<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BasicInfo.aspx.cs" Inherits="UserFrameItems_BasicInfo" %>

<%@ Register TagPrefix="uc" TagName="CommonHeader" Src="~/NewDesign/UserControl/NewCommonHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <uc:CommonHeader ID="ucCommonHead" runat="server" />
    <title>Incentex</title>

    <script type="text/javascript">
        $(document).ready(function() {
            $('input').iCheck({
                checkboxClass: 'icheckbox_flat',
                radioClass: 'iradio_flat'
            });
            $("#txtHireDate").datepicker({
                changeMonth: true,
                changeYear: true
            });
        });
        function CloseUserInfoPopup(result) {
            if (result) {
                var popid = window.parent.document.getElementById('ctl00_pnlUserInfo');
                var fade = window.parent.document.getElementById('fade-layer');
                popid.style.display = 'none'; //or
                popid.style.visibility = 'hidden';
                fade.style.display = 'none'; //or
                fade.style.visibility = 'hidden';
            }
            else {
                window.parent.GeneralAlertMsg('oops error occur while processing your request');
            }
        }
    </script>

</head>
<body class="NoClass">
    <form id="form1" runat="server">
    <div class="register-block" id="registerblock" runat="server">
        <div class="register-content">
            <p class="regtext">
                In this section you can change any of the information listed below about this user.
                Once you make the changes please save your changes. 
             </p>
            <div class="cf">
                <ul class="left-form">
                    <li class="alignleft">
                        <label>
                            <span class="lbl-txt">First Name</span>
                            <asp:TextBox ID="txtFirstName" runat="server" class="input-field first-field" TabIndex="1"></asp:TextBox>
                        </label>
                    </li>
                    <li class="alignright">
                        <label>
                            <span class="lbl-txt">Last Name</span>
                            <asp:TextBox ID="txtLastName" runat="server" class="input-field first-field" TabIndex="2"></asp:TextBox>
                        </label>
                    </li>
                    <li class="alignleft">
                        <label>
                            <span class="lbl-txt">Company</span>
                            <asp:TextBox ID="txtCompany" runat="server" class="input-field" ReadOnly="true" TabIndex="3"></asp:TextBox>
                        </label>
                    </li>
                    <li class="alignright">
                        <label>
                            <span class="lbl-txt">Employee ID</span>
                            <asp:TextBox ID="txtEmployeeID" runat="server" class="input-field" TabIndex="4"></asp:TextBox></label></li>
                    <li class="alignleft">
                        <label>
                            <span class="lbl-txt">Base Station</span> <span class="select-drop medium-drop">
                                <asp:DropDownList ID="ddlBaseStation" runat="server" class="checkvalidation default"
                                    TabIndex="5">
                                    <asp:ListItem Value="0" Text="-select-"></asp:ListItem>
                                </asp:DropDownList>
                            </span>
                        </label>
                    </li>
                    <li class="alignright">
                        <label>
                            <span class="lbl-txt">Workgroup</span> <span class="select-drop medium-drop">
                                <asp:DropDownList ID="ddlWorkGroup" runat="server" class="checkvalidation default"
                                    TabIndex="6">
                                    <asp:ListItem Value="0" Text="-select-"></asp:ListItem>
                                </asp:DropDownList>
                            </span>
                        </label>
                    </li>
                    <li class="alignleft">
                        <label>
                            <span class="lbl-txt">Position</span> <span class="select-drop medium-drop">
                                <asp:DropDownList ID="ddlEmployeeTitle" runat="server" class="checkvalidation default"
                                    TabIndex="7">
                                    <asp:ListItem Value="0" Text="-select-"></asp:ListItem>
                                </asp:DropDownList>
                            </span>
                        </label>
                    </li>
                    <li class="alignright">
                        <label>
                            <span class="lbl-txt">Date of Hire</span>
                            <asp:TextBox ID="txtHireDate" runat="server" class="input-field checkvalidation"
                                TabIndex="8"></asp:TextBox>
                        </label>
                    </li>
                    <li class="alignleft">
                        <label>
                            <span class="lbl-txt">Gender</span></label>
                        <span class="select-drop medium-drop">
                            <asp:DropDownList ID="ddlGender" runat="server" class="checkvalidation default" TabIndex="9">
                                <asp:ListItem Value="0" Text="-select-"></asp:ListItem>
                            </asp:DropDownList>
                        </span></li>
                    <li class="alignright">
                        <label>
                            <span class="lbl-txt">Email Address</span>
                            <asp:TextBox ID="txtEmailAddress" runat="server" class="input-field checkvalidation"
                                TabIndex="10"></asp:TextBox>
                        </label>
                    </li>
                    <li class="alignleft">
                        <label>
                            <span class="lbl-txt">Password</span>
                            <asp:TextBox ID="txtPwdRegistration" runat="server" class="input-field" TabIndex="11"></asp:TextBox></label></li>
                    <li class="alignright">
                        <label>
                            <span class="lbl-txt" style="width: 150px">Third Party Supplier</span>
                        </label>    
                            <span class="select-drop medium-drop">
                                <asp:DropDownList ID="ddlCompany" runat="server" class="checkvalidation default" TabIndex="9">
                                    <asp:ListItem Value="0" Text="-select-"></asp:ListItem>
                                </asp:DropDownList>
                            </span>
                    </li>
                </ul>
            </div>
            <div class="reg-btn-block cf">
                <asp:LinkButton ID="lnkSubmitRegistration" class="blue-home-btn sucess-link popup-openlink submit"
                    runat="server" TabIndex="12" OnClick="lnkSaveRegistration_Click"><span>Save</span></asp:LinkButton>
                <asp:LinkButton ID="lnkSaveAndApprove" class="blue-home-btn sucess-link popup-openlink submit"
                    runat="server" TabIndex="13" OnClick="lnkSaveAndApprove_Click"><span>Save & Approve</span></asp:LinkButton>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
