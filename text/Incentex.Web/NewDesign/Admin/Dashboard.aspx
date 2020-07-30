<%@ Page Language="C#" MasterPageFile="~/NewDesign/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="NewDesign_Admin_Dashboard" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderAdmin" Runat="Server">
 <div class="content">
      <div class="page-title">
        <h2 style="text-transform:uppercase">Dashboard</h2>
      </div>
      <div class="container-body">
        <div id="Dashboard"> 
          
          <!--Start Dashboard Charts-->
          
          <div class="charts">
            <div class="chart-one"><img src="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"]%>StaticContents/Admin/assets/img/chart1.jpg" width="500" height="866" alt="Chart1" /></div>
            <div class="chart-two"><img src="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"]%>StaticContents/Admin/assets/img/chart2.jpg" width="500" height="866" alt="Chart2" /></div>
            <div class="chart-three"><img src="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"]%>StaticContents/Admin/assets/img/chart3.jpg" width="500" height="866" alt="Chart3" /></div>
          </div>
          
          <!--End Dashboard Charts--> 
          
        </div>
      </div>
    </div>
</asp:Content>

