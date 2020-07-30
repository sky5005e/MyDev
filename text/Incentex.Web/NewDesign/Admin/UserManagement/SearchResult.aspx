<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="SearchResult.aspx.cs" Inherits="NewDesign_Admin_UserManagement_SearchResult"
    Title="Search Result" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
   $().ready(function() {

     $(".tabs li").on(window['eventVar'], function() {   
        var divshowID =  $(this).attr("tab-id");
            $(".employee-form").removeClass("active")
            $(".employee-form").hide();
            $(".message-container").hide();
            $("." + divshowID).addClass("active").show();  
                  
       // $.changeTab(this, !$(this).hasClass("checkchanges"));
    });
});
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="admin-user-manage">
        <div class="admin-tab-navbox">
            <div class="wrapper">
                <ul class="admin-tab-nav cf">
                    <li><a href="#" title="Dashboard">Dashboard</a></li>
                    <li><a href="#" title="User Management">User Management</a></li>
                    <li><a href="#" title="Order Management">Order Management</a></li>
                    <li><a href="#" title="My Orders">My Orders</a></li>
                </ul>
            </div>
        </div>
        <div class="title-typ2-box">
            <div class="wrapper title-typ2">
                User Detail</div>
        </div>
        <div id="employee-Details-Block" class="wrapper">
            <div class="employee-form-block">
                <div class="employess-content">
                    <div class="order-links ">
                        <ul class="tabs tabnav cf">
                            <li class="active checkchanges tab-basic" tab-id="basic"><a href="javascript:;" title="Basic">
                                <em></em>Basic</a></li>
                            <li class="checkchanges tab-menu-access" tab-id="menuaccess"><a href="javascript:;"
                                title="Menu Access"><em></em>Menu Access</a></li>
                            <li class="checkchanges tab-settings" tab-id="adminsettings"><a href="javascript:;"
                                title="Admin Settings"><em></em>Settings</a></li>
                            <li class="checkchanges tab-reports" tab-id="reports"><a href="javascript:;" title="Reports">
                                <em></em>Reports</a></li>
                            <li class="checkchanges tab-history last" tab-id="history"><a href="javascript:;"
                                title="History"><em></em>History</a></li>
                        </ul>
                    </div>
                    <div class="employee-form current-tab basic">
                        <div>
                            <div class="basic-form tab-content cf">
                                <ul class="left-form cf">
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">First Name</span>
                                            <input type="text" value="First Name" name="" id="" class="input-field-all first-field checkvalidation">
                                        </label>
                                    </li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Last Name</span>
                                            <input type="text" value="First Name" name="" id="" class="input-field-all first-field checkvalidation">
                                        </label>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Preferred Email</span>
                                            <input type="text" value="Preferred Email" name="" id="" class="input-field-all checkvalidation">
                                        </label>
                                    </li>
                                    <li class="alignright">
                                        <label>
                                            <a href="javascript:;" class="generate-btn" title="generate">generate</a><span class="lbl-txt">Password</span>
                                            <input type="password" value="" name="" id="" class="input-field-all checkvalidation">
                                        </label>
                                    </li>
                                    <li class="clear">&nbsp</li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Gender</span></label><span class="select-drop medium-drop">
                                                <select class="default">
                                                    <option value="0" selected="selected">-Gender-</option>
                                                    <option value="1">Male</option>
                                                    <option value="2">Female</option>
                                                </select>
                                            </span></li>
                                </ul>
                                <ul class="left-form cf">
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Employee ID</span>
                                            <input type="text" value="Preferred Email" name="" id="" class="input-field-all first-field">
                                        </label>
                                    </li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Workgroup</span>
                                        </label>
                                        <label>
                                            <span class="select-drop medium-drop">
                                                <select class="default">
                                                    <option value="0" selected="selected">-Workgroup-</option>
                                                </select>
                                            </span>
                                        </label>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Position</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <select class="default">
                                                <option value="0" selected="selected">-Position-</option>
                                            </select>
                                        </span></li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Date of Hire</span>
                                        </label>
                                        <div class="date-field">
                                            <input type="text" value="" name="" id="" class="input-field-all first-field checkvalidation setDatePicker">
                                        </div>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Station</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <select class="default">
                                                <option value="0" selected="selected">-Station-</option>
                                            </select>
                                        </span></li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Issuance Policy</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <select class="default">
                                                <option value="0" selected="selected">-Issuance Policy-</option>
                                            </select>
                                        </span></li>
                                </ul>
                                <ul class="left-form cf">
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">System Access</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <select class="default">
                                                <option value="0" selected="selected">-System Access-</option>
                                            </select>
                                        </span></li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">System Status</span>
                                        </label>
                                        <span class="select-drop medium-drop">
                                            <select class="default">
                                                <option value="0" selected="selected">-System Status-</option>
                                            </select>
                                        </span></li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Request Date</span>
                                            <input type="text" value="" name="" id="" class="input-field-all">
                                        </label>
                                    </li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt">Activation Date</span>
                                            <input type="text" value="" name="" id="" class="input-field-all">
                                        </label>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Activated By</span>
                                            <input type="text" value="" name="" id="" class="input-field-all">
                                        </label>
                                    </li>
                                </ul>
                            </div>
                            <div class="asset-btn-block assetbtn-blockbar cf">
                                <a id="aBasicCancel" class="small-gray-btn" href="javascript:;" title="Cancel changes and Close">
                                    <span>CANCEL</span> </a><a id="" href="javascript:;" class="small-blue-btn submit"><span>
                                        SAVE</span></a> <a id="" href="javascript:;" class="small-gray-btn submit"><span>CLOSE</span></a>
                                <a id="" href="javascript:;" class="small-blue-btn submit"><span>SEND LOGIN</span></a>
                            </div>
                        </div>
                    </div>
                    <div class="employee-form current-tab menuaccess" style="display: none;">
                        <div>
                            <div class="employee-payment cf">
                                <ul class="emp-left tabs">
                                    <li tab-id="paymentoptions" class="active"><a title="Payment Options" href="javascript:;">
                                        Payment Options</a></li>
                                    <li tab-id="productcategories" class=""><a title="Product Categories" href="javascript:;">
                                        Product Categories</a></li>
                                    <li tab-id="shippingoptions" class=""><a title="Shipping Options" href="javascript:;">
                                        Shipping Options</a></li>
                                    <li tab-id="manageemails" class=""><a title="Manage Emails" href="javascript:;">Manage
                                        Emails</a></li>
                                </ul>
                                <div class="emp-right current-tab paymentoptions active" style="display: block;">
                                    <div class="emp-title">
                                        Payment Options</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <li class="lable-block">
                                                <label class="label_checkbox">
                                                </label>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox" />
                                                    Cost-Center Code
                                                </label>
                                            </li>
                                            <li class="lable-block">
                                                <label class="label_checkbox">
                                                </label>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox" />
                                                    Credit Card
                                                </label>
                                            </li>
                                            <li class="lable-block">
                                                <label class="label_checkbox">
                                                </label>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox" />
                                                    Employee Payroll Deduct
                                                </label>
                                            </li>
                                            <li class="lable-block">
                                                <label class="label_checkbox">
                                                </label>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox" />
                                                    GL-Code
                                                </label>
                                            </li>
                                            <li class="lable-block">
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                    MOAS Issuance Policy Purchases
                                                </label>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                    MOAS Storefront Purchases
                                                </label>
                                                <div style="display: none;" class="" id="">
                                                    <div class="emp-sub-title parentdiv">
                                                        MOAS Approvers <span class="down-arrow">&nbsp;</span></div>
                                                    <ul style="display: none;" class="check-header childdiv cf">
                                                        <div style="" class="input-textarea">
                                                            <table cellspacing="0" border="0" style="border-collapse: collapse;" id="">
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Ana Garcia
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Kim Archambeau
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Bill Brooks
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Matthew Kucinski
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Carol Chibbaro
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Michael Zacharias
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    David Jurich
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Rich Dancaster
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Dinah Liautaud
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Rory Douglass
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Gregory Manny
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Sean Adams
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Incentex Admin
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Sean Adams
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Incentex Incentex
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Sean Adams
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <li>
                                                                                <label class="label_checkbox approver">
                                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                                    Kim Archambeau
                                                                                </label>
                                                                            </li>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </ul>
                                                </div>
                                            </li>
                                            <li class="lable-block">
                                                <label class="label_checkbox">
                                                </label>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                    Purchase Order
                                                </label>
                                            </li>
                                            <li class="lable-block">
                                                <label class="label_checkbox">
                                                </label>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                    Replacement Uniforms
                                                </label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div style="display: none;" class="emp-right current-tab productcategories">
                                    <div class="emp-title">
                                        Product Categories</div>
                                    <div class="MenuAccessScrollbar">
                                        <div>
                                            <input type="hidden" value="2" id="" name="">
                                            <div class="emp-sub-title parentdiv">
                                                Aviation Supplies <span class="down-arrow">&nbsp;</span></div>
                                            <ul style="display: block;" class="check-header childdiv cf">
                                                <table cellspacing="0" border="0" style="border-collapse: collapse;" id="">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Cabin Cleaning Supplies
                                                                    </label>
                                                                </li>
                                                            </td>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Office Supplies
                                                                    </label>
                                                                </li>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        First Aid Supplies
                                                                    </label>
                                                                </li>
                                                            </td>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Ramp Supplies
                                                                    </label>
                                                                </li>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        GSE Equipment
                                                                    </label>
                                                                </li>
                                                            </td>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Security Supplies
                                                                    </label>
                                                                </li>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        In-Flight Supplies</label>
                                                                </li>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </ul>
                                        </div>
                                        <div>
                                            <input type="hidden" value="1" id="" name="">
                                            <div class="emp-sub-title parentdiv">
                                                Employee Uniforms <span class="down-arrow">&nbsp;</span></div>
                                            <ul style="display: block;" class="check-header childdiv cf">
                                                <table cellspacing="0" border="0" style="border-collapse: collapse;" id="">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Bus Drivers
                                                                    </label>
                                                                </li>
                                                            </td>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Inspection Group
                                                                    </label>
                                                                </li>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Cabin Cleaning Crew
                                                                    </label>
                                                                </li>
                                                            </td>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Maintenance Crew
                                                                    </label>
                                                                </li>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Cargo Agents
                                                                    </label>
                                                                </li>
                                                            </td>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Pax Services
                                                                    </label>
                                                                </li>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Company Pilots
                                                                    </label>
                                                                </li>
                                                            </td>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Ramp Agents
                                                                    </label>
                                                                </li>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Customer Service Agents
                                                                    </label>
                                                                </li>
                                                            </td>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Security Agents
                                                                    </label>
                                                                </li>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Deicers
                                                                    </label>
                                                                </li>
                                                            </td>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Stores
                                                                    </label>
                                                                </li>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Dispatchers
                                                                    </label>
                                                                </li>
                                                            </td>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Supervisor Group
                                                                    </label>
                                                                </li>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Flight Attendants
                                                                    </label>
                                                                </li>
                                                            </td>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Tech Ops
                                                                    </label>
                                                                </li>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <li>
                                                                    <label class="label_checkbox">
                                                                        <input type="checkbox" name="checkbox" id="checkbox">
                                                                        Fueling Agents
                                                                    </label>
                                                                </li>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <div style="display: none;" class="emp-right current-tab shippingoptions">
                                    <div class="emp-title">
                                        Shipping Options</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <li>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                </label>
                                            </li>
                                            <li>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                </label>
                                            </li>
                                            <li>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                </label>
                                            </li>
                                            <li>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                </label>
                                            </li>
                                            <li>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                </label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div style="display: none;" class="emp-right current-tab manageemails">
                                    <div class="emp-title">
                                        Manage Emails</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <li>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                    Approved Users
                                                </label>
                                            </li>
                                            <li>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                    Order Confirmations
                                                </label>
                                            </li>
                                            <li>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                    Order Notes
                                                </label>
                                            </li>
                                            <li>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                    Return Confirmations
                                                </label>
                                            </li>
                                            <li>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                    Return Notes
                                                </label>
                                            </li>
                                            <li>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                    Return Notifications - Accounting Related
                                                </label>
                                            </li>
                                            <li>
                                                <label class="label_checkbox">
                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                    Support Tickets
                                                </label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="asset-btn-block assetbtn-blockbar cf">
                                <a href="javascript:;" class="small-gray-btn" id=""><span>Cancel</span> </a><a href=""
                                    class="small-blue-btn" tabindex="23" id=""><span>SAVE</span></a> <a href="" class="small-gray-btn"
                                        title="Close" tabindex="24" id=""><span>Close</span></a>
                            </div>
                        </div>
                    </div>
                    <div class="employee-form current-tab adminsettings" style="display: none;">
                        <div id="">
                            <div class="employee-payment cf">
                                <ul class="emp-left tabs">
                                    <li tab-id="stationmanagement" class="checkchanges tab-section active"><a title="Station Management"
                                        href="javascript:;">Station Management</a> </li>
                                    <li tab-id="workgroupmanagement" class="checkchanges tab-section"><a title="Workgroup Management"
                                        href="javascript:;">Workgroup Management</a> </li>
                                    <li tab-id="privileges" class="checkchanges tab-section"><a title="Privileges" href="javascript:;">
                                        Privileges</a> </li>
                                </ul>
                                <div class="emp-right current-tab stationmanagement">
                                    <div class="emp-title">
                                        Station Management</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <table cellspacing="0" border="0" style="border-collapse: collapse;" id="">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <li>
                                                                <label class="label_checkbox">
                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                    ABQ - Albuquerque, NM
                                                                </label>
                                                            </li>
                                                        </td>
                                                        <td>
                                                            <li>
                                                                <label class="label_checkbox">
                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                    LGB - Long Beach, CA
                                                                </label>
                                                            </li>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <li>
                                                                <label class="label_checkbox">
                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                    ACY - Atlantic City, NJ
                                                                </label>
                                                            </li>
                                                        </td>
                                                        <td>
                                                            <li>
                                                                <label class="label_checkbox">
                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                    LIM - Lima, Peru
                                                                </label>
                                                            </li>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ul>
                                    </div>
                                </div>
                                <div class="emp-right current-tab workgroupmanagement" style="display: none;">
                                    <div class="emp-title">
                                        Workgroup Management</div>
                                    <div id="workgroupmanagementcontainer" class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                        </ul>
                                    </div>
                                </div>
                                <div style="display: none;" class="emp-right current-tab privileges">
                                    <div class="emp-title">
                                        Privileges</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="asset-btn-block assetbtn-blockbar cf">
                                <a href="javascript:;" class="small-gray-btn" id=""><span>Cancel</span> </a><a href=""
                                    class="small-blue-btn postback" tabindex="23" id=""><span>SAVE</span></a> <a href=""
                                        class="small-gray-btn postback" title="Close" id=""><span>Close</span></a>
                            </div>
                        </div>
                    </div>
                    <div class="employee-form current-tab reports" style="display: none;">
                        <div id="">
                            <div class="employee-payment cf">
                                <ul class="emp-left tabs">
                                    <li tab-id="mainreports" class="checkchanges tab-section active"><a title="Reports"
                                        href="javascript:;">Reports</a> </li>
                                    <li tab-id="subreports" class="checkchanges tab-section"><a title="Sub Reports" href="javascript:;">
                                        Sub Reports</a> </li>
                                    <li tab-id="workgroups" class="checkchanges tab-section"><a title="Workgroups" href="javascript:;">
                                        Workgroups</a> </li>
                                    <li tab-id="stations" class="checkchanges tab-section"><a title="Stations" href="javascript:;">
                                        Stations</a> </li>
                                    <li tab-id="pricelevels" class="checkchanges tab-section"><a title="Price Levels"
                                        href="javascript:;">Price Levels</a> </li>
                                </ul>
                                <div class="emp-right current-tab mainreports active" style="display: block;">
                                    <div class="emp-title">
                                        Reports</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                            <table cellspacing="0" border="0" style="border-collapse: collapse;" id="ctl00_ContentPlaceHolder1_dlReports">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <li>
                                                                <label class="label_checkbox">
                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                    Anniversary Credits
                                                                </label>
                                                            </li>
                                                        </td>
                                                        <td>
                                                            <li>
                                                                <input type="checkbox" name="checkbox" id="checkbox">
                                                                Product Planning </label> </li>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <li>
                                                                <label class="label_checkbox">
                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                    Employee Information
                                                                </label>
                                                            </li>
                                                        </td>
                                                        <td>
                                                            <li>
                                                                <label class="label_checkbox">
                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                    Service Level Scorecard
                                                                </label>
                                                            </li>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <li>
                                                                <label class="label_checkbox">
                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                    Order Management
                                                                </label>
                                                            </li>
                                                        </td>
                                                        <td>
                                                            <li>
                                                                <label class="label_checkbox">
                                                                    <input type="checkbox" name="checkbox" id="checkbox">
                                                                    Spend Summary
                                                                </label>
                                                            </li>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ul>
                                    </div>
                                </div>
                                <div style="display: none;" class="emp-right current-tab subreports">
                                    <div class="emp-title">
                                        Sub Reports</div>
                                    <div id="subreportscontainer" class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                        </ul>
                                    </div>
                                </div>
                                <div style="display: none;" class="emp-right current-tab workgroups">
                                    <div class="emp-title">
                                        Workgroups</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                        </ul>
                                    </div>
                                </div>
                                <div style="display: none;" class="emp-right current-tab stations">
                                    <div class="emp-title">
                                        Stations</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                        </ul>
                                    </div>
                                </div>
                                <div style="display: none;" class="emp-right current-tab pricelevels">
                                    <div class="emp-title">
                                        Price Levels</div>
                                    <div class="MenuAccessScrollbar">
                                        <ul class="check-header cf">
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="asset-btn-block assetbtn-blockbar cf">
                                <a href="javascript:;" class="small-gray-btn" id=""><span>Cancel</span> </a><a href=""
                                    class="small-blue-btn postback" tabindex="23" id=""><span>SAVE</span></a> <a href=""
                                        class="small-gray-btn postback" title="Close" id=""><span>Close</span></a>
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
    <div class="footer-box wrapper">
        Copyright © 2014 Incentex. All Rights Reserved.</div>
</asp:Content>
