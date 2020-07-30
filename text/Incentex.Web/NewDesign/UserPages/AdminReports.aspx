<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="AdminReports.aspx.cs" Inherits="UserPages_AdminReports" Title="incentex | Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
    $(document).ready(function(){
        SetDatePicker();
        $(window).ValidationUI();
//        $(".submit").click(function() {
//         if (Page_ClientValidate("GenerateReport"))
//            ShowDefaultLoader();
//    });
         $("#<%= btnGenerateReport.ClientID %>").click(function() {
                 if (Page_ClientValidate("GenerateReport"))
                    ShowDefaultLoader();
         });
    });
    
    function SetDatePicker() {
        $(".setDatePicker").datepicker({
            changeMonth: true,
            changeYear: true
        });
    }
    $("#<%= btnGenerateReport.ClientID %>").click(function() {
         if (Page_ClientValidate("GenerateReport"))
            ShowDefaultLoader();
    });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<input type="hidden" value="admin-link" id="hdnActiveLink" />
    <section id="container" class="cf filter-page">
	<div class="narrowcolumn alignleft">
  	<div class="filter-block cf program-report">
    	<div class="title-txt"><span>Filter</span><a href="javascript: void(0);" title="Help video">Help video</a></div>
      <div class="filter-form cf">
        	<ul class="cf">
          	<li><label class="select-txt">Select one or more search criteria and run the report.</label></li>
            <li><span class="select-drop filter-drop">
                <asp:DropDownList ID="ddlReport" runat="server" class="default checkvalidation">
                                    </asp:DropDownList></span>
                <asp:RequiredFieldValidator ID="rfvReport" runat="server" ControlToValidate="ddlReport"
                                                Display="Dynamic" CssClass="error" ValidationGroup="GenerateReport"
                                                ErrorMessage="Please select report." InitialValue="0">
                 </asp:RequiredFieldValidator>                    
            </li>
            <li><span class="select-drop filter-drop">
                <asp:DropDownList ID="ddlPeriod" runat="server" class="default checkvalidation"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" onchange="ShowDefaultLoader();">
                                    </asp:DropDownList></span>
                <asp:RequiredFieldValidator ID="rfvPeriod" runat="server" ControlToValidate="ddlPeriod"
                                                Display="Dynamic" CssClass="error" ValidationGroup="GenerateReport"
                                                ErrorMessage="Please select date range." InitialValue="0">
                 </asp:RequiredFieldValidator>
            </li>
            <li id="fromDate_li" runat="server" visible="false">
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="input-field-all default_title_text setDatePicker" placeholder="From Date..."
                                    ToolTip="From Date..." tabindex="2" ></asp:TextBox>
            </li>
            <li id="toDate_li" runat="server"  visible="false"><asp:TextBox ID="txtToDate" runat="server" CssClass="input-field-all default_title_text setDatePicker" placeholder="To Date..."
                                    ToolTip="To Date..." tabindex="3" ></asp:TextBox></li>
            <li><span class="select-drop filter-drop">
               <asp:DropDownList ID="ddlBaseStation" runat="server" class="default">
                                    </asp:DropDownList></span></li>
             <li><span class="select-drop filter-drop">
               <asp:DropDownList ID="ddlWorkgroup" runat="server" class="default">
                                    </asp:DropDownList></span></li>
              <li>
                <button id="btnGenerateReport" runat="server" class="blue-btn submit" onserverclick="btnGenerateReport_Click" validationgroup="GenerateReport" call="GenerateReport"
                >
                    Generate</button></li>
              </ul>
          
      </div>
    </div>
  </div>
  <div class="widecolumn alignright">
  	<div class="filter-content">
    	<div class="filter-header cf">
      		<span class="title-txt" id="Report_title_span" runat="server">Reports</span>
          
      </div>
      <div class="program-report">
      	    <asp:Chart ID="PieChart" runat="server" BackColor="Transparent" Height="484" 
                    Width="726" TextAntiAliasingQuality="Normal">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" ToolTip="#VALX : #VAL{C}" CustomProperties="DrawingStyle=Pie,
                            PieDrawingStyle=Concave,PieLabelStyle=Outside,PieLineColor=Gray,LabelsRadialLineSize=1, LabelsHorizontalLineSize=0,
                            CollectedThreshold=2,CollectedLabel=Other (#PERCENT),CollectedSliceExploded=True,CollectedColor=Green,CollectedToolTip=Other : #VAL{C}"
                            IsValueShownAsLabel="True" LabelForeColor="0,0,0" Label="#VALX (#PERCENT)" Font="14px">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent" Area3DStyle-Enable3D="false"
                            BackImageTransparentColor="Transparent" BackImageWrapMode="TileFlipX">
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
           <span class="none-txt" id="nodata_msg" runat="server" visible="false">[No data found]</span>
      </div>
      <div style="float: right; position: relative; top: -30px; right: 10px;" id="dvTotalSpend" runat="server" visible="false">
            Total Spend :
            <asp:Label runat="server" ID="lblTotalSpend"></asp:Label>
      </div>
    </div>
  </div>
</section>
</asp:Content>
