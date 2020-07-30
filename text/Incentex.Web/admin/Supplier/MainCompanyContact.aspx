<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="MainCompanyContact.aspx.cs" Inherits="admin_Supplier_MainCompanyContact" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 36px;
        }
    </style>

    <script language="javascript" type="text/javascript">

        $(function() {
            scrolltextarea(".scrollme", "#Scrolltop1", "#ScrollBottom1");

        });

        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {

                objValMsg = $.xml2json(xml);
                //alert(objValMsg);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtCompanyName: { required: true, alphanumeric: true }
                        , ctl00$ContentPlaceHolder1$txtFirstName: { required: true, alphanumeric: true }
                    , ctl00$ContentPlaceHolder1$txtLastName: { required: true, alphanumeric: true }

                        , ctl00$ContentPlaceHolder1$txtSupplierSetupDate: { date: true }
                        , ctl00$ContentPlaceHolder1$txtEmail: { required: true, email: true }
                        , ctl00$ContentPlaceHolder1$txtCompanyWebsite: { url: true }
                        , ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" }
                        , ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "0" }
                        , ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "0" }
                         , ctl00$ContentPlaceHolder1$txtUserName: { required: true, email: true }
                         , ctl00$ContentPlaceHolder1$txtPassword: { required: true }
                         , ctl00$ContentPlaceHolder1$txtAddress: { required: true }
                         , ctl00$ContentPlaceHolder1$txtTelephone: { required: true, alphanumeric: true }
                         , ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: true }
                         , ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: true }
                         , ctl00$ContentPlaceHolder1$txtMobile: { alphanumeric: true }

                    }
                    , messages:
                    {
                        ctl00$ContentPlaceHolder1$txtCompanyName: {
                            required: replaceMessageString(objValMsg, "Required", "company name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        }
                       , ctl00$ContentPlaceHolder1$txtFirstName: {
                           required: replaceMessageString(objValMsg, "Required", "first name"),
                           alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                       }
                        , ctl00$ContentPlaceHolder1$txtLastName: {
                            required: replaceMessageString(objValMsg, "Required", "last name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        }
                        , ctl00$ContentPlaceHolder1$txtSupplierSetupDate: {
                            date: replaceMessageString(objValMsg, "ValidDate", "")
                        }
                        ,
                        ctl00$ContentPlaceHolder1$txtEmail: {
                            required: replaceMessageString(objValMsg, "Required", "email"),
                            email: replaceMessageString(objValMsg, "Email", "")
                        }
                        , ctl00$ContentPlaceHolder1$txtCompanyWebsite: {
                            url: replaceMessageString(objValMsg, "Website", "")
                        }
                        , ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") }
                        , ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") }
                        , ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") }
                        , ctl00$ContentPlaceHolder1$txtUserName: {
                            required: replaceMessageString(objValMsg, "Required", "user name"),

                            email: replaceMessageString(objValMsg, "Email", "")
                        }
                         , ctl00$ContentPlaceHolder1$txtPassword: { required: replaceMessageString(objValMsg, "Required", "Password")

                         }
                        , ctl00$ContentPlaceHolder1$txtAddress: { required: replaceMessageString(objValMsg, "Required", "address") }
                         , ctl00$ContentPlaceHolder1$txtTelephone:
                         {
                             required: replaceMessageString(objValMsg, "Required", "telephone"),
                             alphanumeric: replaceMessageString(objValMsg, "Number", "")
                         },
                        ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtMobile: { alphanumeric: replaceMessageString(objValMsg, "Number", "") }

                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtAddress")
                            error.insertAfter("#divtxtAddress");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtEmail")
                            error.insertAfter("#dvEmail");
                        else
                            error.insertAfter(element);
                    }

                });
            });
            
            $.checkEmailExists = function () {
                if ($("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val() == "1") {
                    $("#dvEmail").html("This user name already exists in the system.");
                    $("#dvEmail").show();
                    return false;
                }
                else if ($("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val() == "2") {
                    $("#dvEmail").html("A registration request with this user name is already pending for approval.");
                    $("#dvEmail").show();
                    return false;
                }
                else if ($("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val() == "0") {
                    $("#dvEmail").hide();
                    return true;
                }                
            }
            
            $("#ctl00_ContentPlaceHolder1_txtUserName").blur(function() {                
                $.ajax({
                    type: "POST",
                    url: "MainCompanyContact.aspx/CheckDuplicateEmail",
                    data: "{'email':'" + $(this).val() + "', 'userInfoID':'" + $("#ctl00_ContentPlaceHolder1_hdnUserInfoID").val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(msg) {
                        $("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val(msg.d);                            
                        $.checkEmailExists();
                    }
                });
            });         

            $("#<%=lnkSave.ClientID %>").click(function() {
                return $('#aspnetForm').valid() && $.checkEmailExists();
            });
            
            $(window).scroll(function () {              
                $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());

        });     //ready

        function pageLoad(sender, args) {
            assigndesign();
        }

        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'Date',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
        
    </script>

    <style type="text/css">
        .form_table .calender_l .ui-datepicker-trigger
        {
            top: 0px;
        }
        .form_table .shipmax_in .max_w
        {
            width: 45%;
        }
        /*
       .form_table .input_label
        {
        	color:#81B3F8 !important;
        }*/</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
    <asp:HiddenField ID="hdnUserInfoID" runat="server" />
    <asp:HiddenField ID="hdnEmailExistsCode" runat="server" Value="0" />
    <mb:MenuUserControl ID="manuControl" runat="server" />
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <h4>
            Main Company Contact</h4>
        <div>
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <table class="form_table">
                        <tr>
                            <td class="formtd">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Company Name</span>
                                                    <%--<input type="text" class="w_label" />--%>
                                                    <%--<asp:TextBox ID="txtCompany" runat="server" CssClass="w_label" TabIndex="1"></asp:TextBox>--%>
                                                    <%-- <span class="custom-sel label-sel-small">--%>
                                                    <%--<asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" >
                                                        <asp:ListItem Text="-select company-" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="w_label" TabIndex="1"></asp:TextBox>
                                                    <%--  </span>--%>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Title</span>
                                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="w_label" TabIndex="4"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" TabIndex="7"
                                                            OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                            <asp:ListItem Text="-select country-" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Zip Code</span>
                                                    <asp:TextBox ID="txtZip" runat="server" CssClass="w_label" TabIndex="10"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Fax</span>
                                                    <asp:TextBox ID="txtFax" runat="server" CssClass="w_label" TabIndex="13"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Skype Name</span>
                                                    <asp:TextBox ID="txtSkypeName" runat="server" CssClass="w_label" TabIndex="16"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="formtd">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">First Name</span>
                                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="w_label" TabIndex="2"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Department</span>
                                                    <asp:TextBox ID="txtDepartment" runat="server" CssClass="w_label" TabIndex="5"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">State/Province</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" TabIndex="8" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                            <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Telephone</span>
                                                    <asp:TextBox ID="txtTelephone" runat="server" CssClass="w_label" TabIndex="11"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Mobile</span>
                                                    <asp:TextBox ID="txtMobile" runat="server" CssClass="w_label" TabIndex="14"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="formtd_r">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Last Name</span>
                                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="w_label" TabIndex="3"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box employeeedit_text clearfix">
                                                    <span class="input_label alignleft">Address</span>
                                                    <div class="textarea_box alignright">
                                                        <div class="scrollbar">
                                                            <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                                                class="scrollbottom"></a>
                                                        </div>
                                                        <%--<textarea name="" cols="" rows="" class="scrollme"></textarea>--%>
                                                        <asp:TextBox ID="txtAddress" runat="server" CssClass="scrollme colorblue" TextMode="MultiLine"
                                                            TabIndex="6"></asp:TextBox>
                                                    </div>
                                                    <div id="divtxtAddress">
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCity" runat="server" TabIndex="9">
                                                            <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Extension</span>
                                                    <asp:TextBox ID="txtExtension" runat="server" CssClass="w_label" TabIndex="12"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Email</span>
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="w_label" TabIndex="15"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Company Website</span>
                                                    <asp:TextBox ID="txtCompanyWebsite" runat="server" CssClass="w_label" TabIndex="17"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="divider">
        </div>
        <h4>
            Log In Info</h4>
        <div>
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div style="width: 450px">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">User Name</span>
                                            <%--<input type="text" class="w_label" />--%>
                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="width: 450px">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Password</span>
                                            <asp:TextBox ID="txtPassword" runat="server" CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider">
        </div>
        <h4>
            Supplier Classifications</h4>
        <div>
            <table>
                <tr>
                    <td>
                        <!-- list start -->
                        <%--       <asp:DataList ID="lst" runat="server" 
                        RepeatLayout="Table" 
                        RepeatColumns="3" 
                        RepeatDirection="Horizontal" ondatabinding="lst_DataBinding"  >
                      
                        <ItemTemplate>
                            <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft">
                                     
                                        <asp:CheckBox id="chk" runat="server" />
                                        </span>
                                        <label>
                                        <%#Eval("sLookupName") %>
                                        </label>
                                </td>
                            </tr>
                            </table>
                        </ItemTemplate>
                        <ItemStyle Width="30%" />
                    </asp:DataList>--%>
                        <!-- list end -->
                    </td>
                </tr>
                <tr>
                    <td class="form_table">
                        <div class="calender_l shipmax_in" style="width: 450px">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label max_w" style="width: 38%">Supplier Classifications</span>
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlSupplierClassifications" runat="server" TabIndex="9">
                                        <asp:ListItem Text="-select a classification-" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="form_table" style="padding-top: 15px">
                        <div class="calender_l" style="width: 450px">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box supplier_annual_date">
                                <span class="input_label">Supplier Setup Date</span>
                                <asp:TextBox ID="txtSupplierSetupDate" runat="server" CssClass="w_label datepicker min_w"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider">
        </div>
        <h4>
            World-Link System Privileges</h4>
        <div>
            <table>
                <tr>
                    <td class="formtd">
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft" id="spchkAccessToWorldLinkTrackingCenter"
                                        runat="server">
                                        <%--<input type="checkbox" />--%>
                                        <asp:CheckBox ID="chkAccessToWorldLinkTrackingCenter" runat="server" />
                                    </span>
                                    <label>
                                        Access to World-Link Order Management</label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft" id="spchkAccessToPurchaseOrders" runat="server">
                                        <%--<input type="checkbox" />--%>
                                        <asp:CheckBox ID="chkAccessToPurchaseOrders" runat="server" />
                                    </span>
                                    <label>
                                        Access to Edit or Make Changes to Purchase Orders</label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <%--<td>
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <span class="custom_radio alignleft">
                                        <input type="radio" name="gen" />
                                    </span>
                                    <label> Female</label>
                                    
                                    <span class="custom_radio alignleft">
                                        <input type="radio" name="gen" />
                                    </span>
                                    <label>Male</label>
                                </td>
                                
                            </tr>
                        </table>
                    </td>--%>
                </tr>
            </table>
        </div>
        <div class="alignnone spacer25">
        </div>
        <div class="additional_btn">
            <ul class="clearfix">
                <li>
                    <%--<a href="#" title="Save Information" class="grey2_btn"><span>Save Information</span></a>--%>
                    <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" OnClick="lnkSave_Click">
								        <span>Save Information</span>
                    </asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
