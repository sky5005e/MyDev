<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminPage.aspx.cs" Inherits="NewDesign_HTML_AdminPage"  MasterPageFile="~/NewDesign/FrontMasterPage.master" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

 <script type="text/javascript">
   $().ready(function() {

     $(".tabs li").on(window['eventVar'], function() {   
        var divshowID =  $(this).attr("tab-id");
            $(".employee-form").removeClass("active")
            $(".employee-form").hide();
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
    
    <div class="emp-manage-container">
          <div class="wrapper">
            <div class="emp-manage-inner cf">
                <div class="title">Employee Management <span class="results">5471 Results</span></div>
                <div class="filter-search">
                    <input type="text" placeholder="Search Results..." class="input-box" title="Search Results..." id="" maxlength="100" name="">
                    <a class="go-btn" id="">GO</a>
                </div>
                
               <div class="select-typ1-outer">
                    <div class="select-typ1">
			            <select>
                           <option value="0" selected="selected">search criteria</option>
                        </select>
                    </div>    
                </div>
               
               <div class="add-empbtn-box">  
                <a class="newemp-btn" title="Add New Employee">Add New Employee</a>                  
               </div>                           
            </div>
          </div>
    </div>
    
    <div id="employee-Details-Block" class="wrapper">
        <div class="employee-form-block">
            <div class="employess-content">
            <div class="cart-category">
                <ul class="tabs tabnav cf">
                    <li class="first active" tab-id="manualentry"><a href="javascript:;" id="lnkManualEntry"
                        title="Manual Entry"><em></em>Manual Entry</a></li>
                    <li class="last" tab-id="uploadexcel"><a href="javascript:;" id="lnkUploadExcelFile" title="Upload Excel File">
                        <em></em>Upload Excel File</a></li>
                </ul>
            </div>
                <div class="employee-form current-tab manualentry tabcon">
        <div>
            <div class="basic-form cf">
                <ul class="left-form cf">
                    <li class="alignleft">
                        <label>
                            <span class="lbl-txt">First Name</span>
                            <input type="text" value="First Name" name="" id="Text1" class="input-field-all first-field checkvalidation">                                           
                        </label>
                    </li>
                    <li class="alignright">
                        <label>
                            <span class="lbl-txt">Last Name</span>
                            <input type="text" value="Last Name" name="" id="Text2" class="input-field-all checkvalidation">     
                        </label>
                    </li>
                    <li class="alignleft">
                        <label>
                            <span class="lbl-txt">Preferred Email</span>
                            <input type="text" value="Preferred Email" name="" id="Text3" class="input-field-all checkvalidation">  
                        </label>
                    </li>
                    <li class="alignright">
                        <label>
                            <a href="javascript:;" class="generate-btn" title="generate">generate</a><span class="lbl-txt">Password</span>
                             <input type="password" value="" name="" id="Password1" class="input-field-all checkvalidation"> 
                        </label>
                    </li>
                    <li class="clear">&nbsp;</li>
                    <li class="alignleft">
                        <label>
                            <span class="lbl-txt">Gender</span>
                        </label>
                        <span class="select-drop medium-drop">
                             <select class="default">
                               <option value="0" selected="selected">Gender</option>
                               <option value="1">Male</option>
                               <option value="2">Female</option>
                            </select>
                        </span>
                     </li>
                </ul>
                <ul class="left-form cf">
                    <li class="alignleft">
                        <label>
                            <span class="lbl-txt">Employee ID</span>
                            <input type="text" value="Employee ID" name="" id="Text4" class="input-field-all first-field checkvalidation"> 
                        </label>
                    </li>
                    <li class="alignright">
                        <label>
                            <span class="lbl-txt">Workgroup</span>
                        </label>
                        <span class="select-drop medium-drop">
                            <select class="default">
                               <option value="0" selected="selected">Customer Service Agents</option>
                               <option value="1">Customer Service Agents</option>
                            </select>
                        </span>
                    </li>
                    <li class="alignleft">
                        <label>
                            <span class="lbl-txt">Position</span>
                        </label>
                        <span class="select-drop medium-drop">
                           <select class="default">
                               <option value="0" selected="selected">- Position -</option>
                            </select>
                        </span></li>
                    <li class="alignright">
                        <label>
                            <span class="lbl-txt">Date of Hire</span>
                        </label>
                        <div class="date-field">
                            <input type="text" value="" name="" id="Text5" class="input-field-all first-field checkvalidation"> 
                        </div>
                    </li>
                    <li class="alignleft">
                        <label>
                            <span class="lbl-txt">Station</span>
                        </label>
                        <span class="select-drop medium-drop">
                           <select class="default">
                               <option value="0" selected="selected">- Station -</option>
                               <option value="1">- Station -</option>
                            </select>
                        </span>
                    </li>
                    <li class="alignright">
                        <label>
                            <span class="lbl-txt">Issuance Policy</span>
                        </label>
                        <span class="select-drop medium-drop">
                            <select class="default">
                               <option value="0" selected="selected">-Issuance -</option>
                            </select>
                        </span>
                    </li>
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
                               <option value="0" selected="selected">-System Status -</option>
                            </select>
                        </span></li>
                </ul>
            </div>
            <div class="textcenter">
                To send login email please go to Basic Tab in User Profile
            </div>
            <div class="emp-btn-block">
                <a id="A1" href="javascript:;" tabindex="20" class="gray-btn">Cancel</a>
                <a id="A2" href="javascript:;" tabindex="20" class="blue-btn submit">Add Employee</a>
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
                        <input type="file"  name="" id="File1" class="checkvalidation">
                    </div>
                </div>
                <div class="emp-btn-block">
                      <a href="javascript:;" class="blue-btn" title="Download Template">Download Template</a>
                      <a href="" class="blue-btn submit" id="">Upload</a>
                      
                </div>
            </div>
        </div>
    </div>
        </div>
        </div>
    </div>
</div>
 <div class="footer-box wrapper">Copyright © 2014 Incentex. All Rights Reserved.</div>
</asp:Content>

