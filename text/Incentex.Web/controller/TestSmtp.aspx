<%@ Page Language="C#" AutoEventWireup="true" Inherits="controller_TestSmtp" CodeFile="~/controller/TestSmtp.aspx.cs"
    Title="test" Async="true" %>

<%@ Reference Control="~/NewDesign/UserControl/CustomDropDown.ascx" %>
<%@ Register Src="~/NewDesign/UserControl/MultiSelectDropDown.ascx" TagName="MultiSelectDropDown"
    TagPrefix="msd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <script src="../JS/jquery.js" type="text/javascript"></script>

    <script src="../NewDesign/StaticContents/js/placeholders.js" type="text/javascript"
        language="javascript"></script>

    <script src="../JS/JQuery/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
//        $().ready(function() {
//            var s = $("#theTD").text().split(' ');
//            var animationcanvas = $('<div />');
//            for (i = 0; i< s.length; i++){
//                animationcanvas.append('<div style="float:left" class="dvAnimate">' + s[i] + '&nbsp;</div>');                
//            }
//            
//            $("#theTD").html(animationcanvas);
//            
//            setInterval(function() { $(".dvAnimate").each(function(index) {                
//                $(this).fadeOut("fast").fadeIn(50*index)
//                .animate({ fontSize: "24px" }, 50*index ).delay(4000).animate({ fontSize: "16px" }, 50*index );
//                var randomColor = Math.floor(Math.random()*16777215).toString(16);
//    	        $(this).css("color", "#" + randomColor);    	        
//            }); }, 15000);

//            $("#btnCallService").click(function(){            
//                $.ajax({
//                    type: "POST",
//                    url: '../TestService.asmx/HelloWorld',                
//                    contentType: "application/json; charset=utf-8",
//                    dataType: "json",
//                    success: function (data) {
//                        $("#btnCallService").val(data);
//                    },
//                    error: function (data, errorThrown) {
//                        alert("Fail");
//                        alert(errorThrown);
//                    }
//                });
//            });
//          
//            $(".chkItem input:checkbox").change(function(){            
//                $("#hdnCurrent").val($(this).attr("id") + "|" + $(this).attr("checked"));
//                $("#hdnButton").click();
//            });
//        });
    </script>

    <script type="text/javascript">
        $().ready(function() {
            //$("#show-news").html($("#news-container").find("li").slice(0, 2));
        });
    </script>

    <script language="JavaScript" type="text/javascript">
        function startclock(){
	        var thetime=new Date();
	        var nhours=thetime.getHours();
	        var nmins=thetime.getMinutes();
	        var nsecn=thetime.getSeconds();
	        
	        var dd = "AM";            
            if (nhours >= 12) {             
                dd = "PM";
                nhours -=12;
            }

	        if (nhours==0)
   	            nhours=12;

	        if (nsecn<10)
 	            nsecn = "0" + nsecn;
	        if (nmins<10)
	            nmins = "0" + nmins;
	        if (nhours<10)
	            nhours = "0" + nhours
	            
	        document.ciClockform.gmtClock.value = nhours + ":" + nmins + ":" + nsecn + " " + dd;	        
	        
	        setTimeout('startclock()',1000);
        }
        
        function SetServerTimeToTextBox(ddate, tzone){
	        var thetime=new Date(ddate);
	        var nmonth = thetime.getMonth() + 1;
	        var ndate = thetime.getDate();
	        var nyear = thetime.getFullYear();
	        var nhours=thetime.getHours();
	        var nmins=thetime.getMinutes();
	        var nsecn=thetime.getSeconds();
	        
	        var dd = "AM";            
            if (nhours >= 12) {             
                dd = "PM";
                nhours -=12;
            }

	        if (nhours==0)
   	            nhours=12;

	        if (nsecn<10)
 	            nsecn = "0" + nsecn;
	        if (nmins<10)
	            nmins = "0" + nmins;
	        if (nhours<10)
	            nhours = "0" + nhours
	            
	        return nmonth + "/" + ndate + "/" + nyear + " " + nhours + ":" + nmins + ":" + nsecn + " " + dd + " (" + tzone + ")";
        }
        
        $().ready(function() {
            //alert($("#txtServerTime").val());
            //alert($("#txtServerTime").val());
            var MyDate = new Date('<%= DateTime.Now.ToString() %>');
            var timzone = "<%= System.TimeZone.CurrentTimeZone.StandardName %>";
            $("#txtServerTime").val(SetServerTimeToTextBox(MyDate, timzone));
            //alert("<%# DateTime.Now %>");
            
            $("#txtZipCode").change(function() {
                if (isValidPostalCode($("#txtZipCode").val(), $("#txtCountryCode").val()))
                    $("#lblClientValidationForZipCode").html("Valid Zip Code from Client!!!");
                else
                    $("#lblClientValidationForZipCode").html("In-valid Zip Code from Client!!!");
            });
            
            if (isValidPostalCode($("#txtZipCode").val(), $("#txtCountryCode").val()))
                $("#lblClientValidationForZipCode").html("Valid Zip Code from Client!!!");
            else
                $("#lblClientValidationForZipCode").html("In-valid Zip Code from Client!!!");
        });
        
        function isValidPostalCode(postalCode, countryCode) {
            switch (countryCode) {
                case "US":
                    postalCodeRegex = /^([0-9]{5})(?:[-\s]*([0-9]{4}))?$/;
                    break;
                case "CA":
                    postalCodeRegex = /^([ABCEGHJKLMNPRSTVXYabceghjklmnprstvxy][0-9][ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvwxyz])\s*([0-9][ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvwxyz][0-9])$/;
                    break;
                default:
                    postalCodeRegex = /^(?:[A-Z0-9]+([- ]?[A-Z0-9]+)*)?$/;
            }
            return postalCodeRegex.test(postalCode);
        }
    </script>

    <title>Test Page</title>
</head>
<body>
    <div style="display: none;">
        <form action="" name="ciClockform" style="margin: 0px; padding: 0px;">
        <p class="fixGMTTxt" style="width: 125px; height: 18px; padding: 0px 2px 2px 10px;
            float: left;">
            Time
            <input type="text" name="gmtClock" style="border-left: 1px solid rgb(204, 204, 204);
                border-right: 1px solid rgb(204, 204, 204); width: 75px; height: 13px;" class="ciSerFixtdatebg" />

            <script language="JavaScript" type="text/javascript">
                startclock();
            </script>

        </p>
        </form>
    </div>
    <div>
        <form id="form1" runat="server">
        <div style="display: none;">
            <label>
                Multi Select 1</label>
            <msd:MultiSelectDropDown ID="msdTest1" runat="server" />
        </div>
        <div style="display: none;">
            <label>
                Multi Select 2</label>
            <msd:MultiSelectDropDown ID="msdTest2" runat="server" />
        </div>
        <div id="dvTest" runat="server" visible="true">
            <br />
            <asp:Button ID="btnErrorCheck" runat="server" Text="Test Error" OnClick="btnErrorCheck_Click"
                Visible="false" />
            <br />
            <asp:Button ID="hdnButton" runat="server" Style="display: none;" OnClick="hdnButton_Click" />
            <asp:HiddenField ID="hdnCurrent" runat="server" />
            <div>
                <asp:Panel ID="pnlCustomDropDownPanel" runat="server">
                    <asp:Label ID="lblCustomDropDown" runat="server" Text="Custom Drop Down"></asp:Label>&ensp;&ensp;&ensp;
                </asp:Panel>
            </div>
            <div style="width: 100%;">
                <table style="width: 100%;">
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            Zip-Code USA :
                            <asp:TextBox ID="txtZipCodeUS" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                        <td colspan="2">
                            Province Canada :
                            <asp:TextBox ID="txtProvinceCA" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            County Name US :
                            <asp:Label ID="lblCountyNameUS" runat="server"></asp:Label>
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                        <td colspan="2">
                            Province Abbreviation :
                            <asp:Label ID="lblAbbreviationCA" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            Tax Rate USA :
                            <asp:Label ID="lblTaxRateUS" runat="server"></asp:Label>
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                        <td colspan="2">
                            GST Canada :
                            <asp:Label ID="lblGSTCA" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;&nbsp;
                        </td>
                        <td colspan="2">
                            PST Canada :
                            <asp:Label ID="lblPSTCA" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;&nbsp;
                        </td>
                        <td colspan="2">
                            Total Tax Canada :
                            <asp:Label ID="lblTotalCA" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnGetStrikeIronResponseUS" runat="server" OnClick="btnGetStrikeIronResponseUS_Click"
                                Text="Get Strike Iron Response for USA" />
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                        <td colspan="2">
                            <asp:Button ID="btnGetStrikeIronResponseCA" runat="server" OnClick="btnGetStrikeIronResponseCA_Click"
                                Text="Get Strike Iron Response for Canada" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <label>
                                Enter Path for <b>Order</b> Update Request XML :
                            </label>
                            <asp:TextBox ID="txtOrderUpdateXMLPath" runat="server" />
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                        <td colspan="2">
                            <label>
                                Enter Path for <b>Shipment</b> Update Request XML :
                            </label>
                            <asp:TextBox ID="txtShipmentUpdateXMLPath" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnUpdateSalesOrderFromXML" runat="server" Text="Update Sales Order From XML"
                                OnClick="btnUpdateSalesOrderFromXML_Click" />
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                        <td colspan="2">
                            <asp:Button ID="btnUpdateShipmentFromXML" runat="server" Text="Update Sales Order Shipment From XML"
                                OnClick="btnUpdateShipmentFromXML_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Item Number:
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemNumber" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Item Description:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Current Item Number
                        </td>
                        <td>
                            <asp:TextBox ID="txtCurrItemNum" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Item Description:
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemDesc" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Previous Item Number
                        </td>
                        <td>
                            <asp:TextBox ID="txtPreviousItemNum" runat="server"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%;">
                            <asp:Button ID="Button1" runat="server" Text="New" OnClick="Button1_Click" />
                        </td>
                        <td style="width: 20%;">
                            <asp:Button ID="Button2" runat="server" Text="Test Regex" OnClick="Button2_Click" />
                        </td>
                        <td style="width: 20%;">
                            <asp:Button ID="Button3" runat="server" Text="Read IMAP" OnClick="Button3_Click" />
                        </td>
                        <td style="width: 20%;">
                            <asp:Button ID="Button4" runat="server" Text="Test" OnClick="Button4_Click" />
                        </td>
                        <td style="width: 20%;">
                            <asp:Button ID="Button5" runat="server" Text="Read POP" OnClick="Button5_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%;">
                            <asp:Label ID="lblOrderNumberToUpdate" runat="server" Text="Order Number"></asp:Label>
                        </td>
                        <td colspan="2" style="width: 40%;">
                            <asp:TextBox ID="txtOrderNumberToUpdate" runat="server"></asp:TextBox>
                        </td>
                        <td colspan="2" style="width: 40%;">
                            <asp:Button ID="btnLoadOrderToUpdate" runat="server" Text="Load Order" OnClick="btnLoadOrderToUpdate_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:GridView ID="gvOrderItems" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvOrderItems_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemNumber" runat="server" Text='<%# Eval("ItemNumber") %>'></asp:Label>
                                            <asp:HiddenField ID="hdnWorkGroupID" runat="server" Value='<%# Eval("WorkGroupID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:TextBox>
                                            <asp:HiddenField ID="hdnDescription" runat="server" Value='<%# Eval("ProductDescrption") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDeleteTxn" Text="Delete" runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Button ID="btnUpdateFromSAP" runat="server" Text="Update from SAP" OnClick="btnUpdateShipment_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div style="border: solid 1px Black; padding: 5px;">
                                <b>Order Response</b>
                                <asp:Literal ID="ltrOrderResponse" runat="server"></asp:Literal>
                            </div>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td colspan="5">
                            <input type="button" id="btnCallService" name="btnCallService" value="call service" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td colspan="5" id="theTD">
                            Hi, this is a demo for animation using jQuery fadeOut, fadeIn, delay, setInterval
                            and css. Enloy...!!!
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td colspan="5">
                            <asp:GridView ID="gvTemp" runat="server" Visible="false" OnRowDataBound="gvTemp_RowDataBound"
                                OnRowDeleting="gvTemp_RowDeleting" AutoGenerateDeleteButton="true">
                                <Columns>
                                    <asp:BoundField DataField="ID" />
                                    <asp:TemplateField Visible="true">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnGridID" runat="server" Value='<%# Eval("ID") %>' />
                                            <asp:Repeater ID="repeater1" runat="server" OnItemCommand="repeater1_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnRepID" runat="server" Value='<%# Eval("ID") %>' />
                                                    <asp:RadioButtonList ID="radiobuttonlist1" runat="server">
                                                        <asp:ListItem Text="1"></asp:ListItem>
                                                        <asp:ListItem Text="2"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Button ID="btnButton" runat="server" Text="Footer Button" CommandName="button1" />
                                                    <asp:Button ID="btnButton2" runat="server" Text="Footer Button" CommandName="button2" />
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBoxList ID="chkList" runat="server">
                                            </asp:CheckBoxList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <%--<fjx:FileUploader ID="FileUploader1" MaxFileSize="5MB" MaxNumberFiles="10" MaxFileQueueSize="20MB"
            runat="server">
        </fjx:FileUploader>--%>
                <asp:HiddenField ID="popport" runat="server" Value="995" />
                <asp:HiddenField ID="imapport" runat="server" Value="143" />
                <asp:HiddenField ID="from" runat="server" Value="support@world-link.us.com" />
                <asp:HiddenField ID="to" runat="server" Value="devraj.gadhavi@indianic.com" />
                <asp:HiddenField ID="ssl" runat="server" Value="true" />
                <asp:HiddenField ID="host" runat="server" Value="pop.gmail.com" />
                <asp:HiddenField ID="username" runat="server" Value="incentextest10@gmail.com" />
                <asp:HiddenField ID="password" runat="server" Value="test10incentex" />
                <%--<asp:HiddenField ID="host" runat="server" Value="localhost" />
        <asp:HiddenField ID="username" runat="server" Value="birdiethis1985" />
        <asp:HiddenField ID="password" runat="server" Value="admin1985@" /> --%>
            </div>
            <div style="border: solid 1px Black; padding: 5px;">
                <b>Insert Response</b>
                <asp:Literal ID="ltrInsertResponse" runat="server"></asp:Literal>
                <br />
                <br />
                <b>Update Response</b>
                <asp:Literal ID="ltrUpdateResponse" runat="server"></asp:Literal>
            </div>
            <div>
                <asp:Button ID="btnTestProcedure" runat="server" OnClick="btnTestProcedure_Click"
                    Text="Test Procedure" />
            </div>
            <br />
            <br />
            <br />
            <br />
            <div style="float: left">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblOrderNumberToTransmit" runat="server" Text="Enter Order Number To Transmit To SAP : " />
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderNumberToTransmit" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnTransmitToSAP" runat="server" Text="Transmit To SAP" OnClick="btnTransmitToSAP_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="float: left">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblServerTime" runat="server" Text="Server Time :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServerTime" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="dvTodaysEmail" runat="server" visible="false">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblSendEmail" runat="server">Send Today's Emails</asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="btnSendEmail" runat="server" Text="Send Today's Emails" OnClick="btnSendEmail_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        <div id="show-news" style="height: 520px;">
            Hi!
        </div>
        <div id="news-container" style="display: none;">
            <ul id="inner">
                <li>
                    <div>
                        News 1
                    </div>
                </li>
                <li>
                    <div>
                        News 2
                    </div>
                </li>
                <li>
                    <div>
                        News 3
                    </div>
                </li>
                <li>
                    <div>
                        News 4
                    </div>
                </li>
                <li>
                    <div>
                        News 5
                    </div>
                </li>
                <li>
                    <div>
                        News 6
                    </div>
                </li>
                <li>
                    <div>
                        News 7
                    </div>
                </li>
                <li>
                    <div>
                        News 8
                    </div>
                </li>
            </ul>
        </div>
        </form>
    </div>

    <script type="text/javascript">
         (function() {
             var mf = document.createElement("script"); mf.type = "text/javascript"; mf.async = true;
             mf.src = "//cdn.mouseflow.com/projects/6400d321-64cf-4fcb-a60d-a9e5ecdd0ee2.js";
             document.getElementsByTagName("head")[0].appendChild(mf);
         })();
    </script>

    <script type="text/javascript">    
        var title = document.getElementsByTagName("title")[0].innerHTML;
        var cid = "<%=Common.CompID %>";
        var uid = "<%= Common.UserID %>";
        var er = "<%= PageBase.IsError %>";
        var path = "/" + title + "/cid_" + cid + "&uid_" + uid + "&er_" + er + "";
        var mouseflowPath = path;     
    </script>

</body>
</html>
