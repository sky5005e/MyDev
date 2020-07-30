<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true" CodeFile="SearchUser.aspx.cs" Inherits="NewDesign_Admin_UserManagement_SearchUser" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

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
          <a class="go-btn" id="">GO</a> </div>
        <div class="select-typ1-outer">
          <div class="select-typ1">
            <select>
              <option value="0" selected="selected">search criteria</option>
            </select>
          </div>
        </div>
        <div class="add-empbtn-box"> <a class="newemp-btn2" title="Add New Employee">Add New Employee</a> </div>
      </div>
    </div>
  </div>
  <div class="filter-container-box">
	<div class="wrapper">
            <div class="filter-block cf">
                               <!--<div class="title-txt">
                    <span>&nbsp;&nbsp;</span><a href="javascript:;" title="Help Video" onclick="">Help
                        Video</a></div>-->
                <div class="filter-form cf">
                    <ul class="cf">                        
                        <li>
                        	<input type="text" placeholder="First Name" class="input-field-all first-field default_title_text" title="First Name">
                        </li>
                        <li>
                        	 <input type="text" placeholder="Last Name" class="input-field-all first-field default_title_text" title="Last Name">
                        </li>
                        <li>
                        	<span class="select-drop filter-drop">
                            <select class="default">
                            	<option value="0" selected="selected">- Workgroup -</option>
                                <option value="361">Bus Operators</option>
                                <option value="370">Cabin Cleaning Group</option>
                                <option value="366">Cargo Agents</option>
                                <option value="367">Cargo Customer Service Agents</option>
                            </select>
                        	</span>
                        </li>
                        <li><span class="select-drop filter-drop">
                            <select class="default">
                                <option value="0" selected="selected">- Station -</option>
                                <option value="164">ABQ - Albuquerque, NM</option>
                                <option value="10">ACY - Atlantic City, NJ</option>
                                <option value="130">ADS - Addison, TX</option>
                                <option value="59">ANC - Anchorage, AK</option>
                                <option value="11">ATL - Atlanta, GA</option>
                            </select>
                        </span></li>
                        <li><span class="select-drop filter-drop">
                            <select class="default">
                                <option value="0" selected="selected">- System Status -</option>
                                <option value="135">Active</option>
                                <option value="136">InActive</option>
                            </select>
                        </span></li>
                        <li><span class="select-drop filter-drop">
                            <select class="default">
                                <option value="0" selected="selected">- System Access -</option>
                                <option value="3">Admin</option>
                                <option value="4">Employee</option>
                                <option value="7">XSE</option>
                            </select>
                        </span></li>
                        
                        <li>
                        	<input type="submit" class="blue-btn add-new postback" value="Search for Employee" name="">

                        </li>
                    </ul>
                </div>
            </div>
            </div>
        </div>
  
  <div id="container" class="cf filter-page">
    <div class="filter-tableblock cf">
      <div>
        <table cellspacing="0" border="0" style="border-collapse:collapse;" id="" class="table-grid">
          <tbody>
            <tr>
              <th scope="col" class="col1"> <a href="#" class="postback" id="">Employee Name</a> </th>
              <th scope="col" class="col2"> <a href="#" class="postback" id="">Workgroup</a> </th>
              <th scope="col" class="col3"> <a href="" class="postback" id="">Station</a> </th>
              <th scope="col" class="col4"> <a href="" id="">Status</a> </th>
            </tr>
            <tr>
              <td class="col1"><span> <a href="#" class="postback" id="">A C</a></span></td>
              <td class="col2"></td>
              <td class="col3"></td>
              <td class="col4"><div class="apple_check">
                  <label class="apple_checkbox">
                  <span class="postback">
                  <div class="icheckbox_flat" style="position: relative;">
                    <input type="checkbox" id="">
                  </div>
                  </span>&nbsp;
                  </label>
                </div></td>
            </tr>
            <tr>
              <td class="col1"><span> <a href="#" class="postback" id="">A C</a></span></td>
              <td class="col2"></td>
              <td class="col3"></td>
              <td class="col4"><div class="apple_check">
                  <label class="apple_checkbox">
                  <span class="postback">
                  <div class="icheckbox_flat checked" style="position: relative;">
                    <input type="checkbox" id="">
                  </div>
                  </span>&nbsp;
                  </label>
                </div></td>
            </tr>
            <tr>
              <td class="col1"><span> <a href="#" class="postback" id="">Aaron Brown</a></span></td>
              <td class="col2"><span id="">Flight Attendant Group</span></td>
              <td class="col3"><span id="">FLL</span></td>
              <td class="col4"><div class="apple_check">
                  <label class="apple_checkbox">
                  <span class="postback">
                  <div class="icheckbox_flat" style="position: relative;">
                    <input type="checkbox" id="">
                  </div>
                  </span>&nbsp;
                  </label>
                </div></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div class="store-footer cf" id="ctl00_ContentPlaceHolder1_pagingtable"> <a class="store-title" href="javascript:;">BACK TO TOP</a> <a href="" class="pagination alignright view-link postback cf" id=""> VIEW ALL </a>
      <div class="pagination alignright cf"> <span> <a class="left-arrow alignleft postback" title="Previous" disabled="disabled" id=""></a> </span> <span id=""><span> <a style="font-weight:bold;cursor:pointer;" class="postback" disabled="disabled" id="">1</a> </span><span> <a href="" class="postback" id="">2</a> </span><span> <a href="" class="postback" id="">3</a> </span><span> <a href="" class="postback" id="">4</a> </span><span> <a href="" class="postback" id="">5</a> </span></span> <a href="" class="right-arrow alignright postback" title="Next" id=""></a> </div>
    </div>
  </div>
</div>
<div class="footer-box wrapper">Copyright © 2014 Incentex. All Rights Reserved.</div>

</asp:Content>

