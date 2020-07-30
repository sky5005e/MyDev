<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintPreShippingLabel.aspx.cs" Inherits="ProductReturnManagement_PrintPreShippingLabel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">




<html><head><title>
View/Print Label</title></head>
<style>
    .small_text {font-size: 80%;}
    .large_text {font-size: 115%;}
</style>
 <script language="javascript" type="text/javascript">
     window.onload = function() {
         window.print();
     };
     function printDiv() {
         var prtContent = document.getElementById('printableArea');
         var WinPrint = window.open('', '', 'left=0,top=0,width=1,height=1,toolbar=0,scrollbars=0,status=0');
         WinPrint.document.write(prtContent.innerHTML);
         WinPrint.document.close();
         WinPrint.focus();
         WinPrint.print();
         WinPrint.close();
     }

    </script>
<body bgcolor="#FFFFFF">
 <div id="printableArea" style="width:650; border:0;">
<table border="0" cellpadding="0" cellspacing="0" width="600"><tr>
<td height="120px" align="left" valign="top" colspan="4">
<b class="large_text">INCENTEX REPAIR PROCESSING CENTER</b>
&nbsp;<br />
<p>For proper processing please include this repair order ticket in the
package you are sending back to us for repair.</p>
 </td></tr>
 <%--<tr style="height:25px;"><td valign="top">Company : <asp:Label id="lblcompany" runat="server" /></td>
      <td valign="top">Repair Number : <asp:Label id="lblrepairnumber" runat="server" /></td>
 </tr>
 <tr style="height:25px;"> <td valign="top">Contact : <asp:Label id="lblcontact" runat="server" /></td>
      <td valign="top">Submit Date : <asp:Label id="lblsubmitdate" runat="server" /></td>
 </tr>
 <tr style="height:25px;"> <td valign="top">Repair Quantity : <asp:Label id="lblrepairquantity" runat="server" /></td>
      <td valign="top">Request Quote : <asp:Label id="lblrequestquote" runat="server" /></td>
 </tr>
 <tr style="height:25px;"> <td valign="top">Headset Brand : <asp:Label id="lblheadsetbrand" runat="server" /></td>
 </tr>--%>
 </table>
 <br /><br />
<table border="0" cellpadding="0" cellspacing="0" width="600">
<tr>
<td class="small_text" align="left" valign="top">
&nbsp;&nbsp;&nbsp;
<a name="foldHere">FOLD HERE</a></td>
</tr>
<tr>
<td align="left" valign="top"><hr>
</td>
</tr>
</table>

<table>
<tr>
<td height="10">&nbsp;
</td>
</tr>
</table>

<table border="0" cellpadding="0" cellspacing="0" width="650" ><tr>
<td align="left" valign="top">

<img src="../UPSShippingReferences/ShippingLabelImage/HeadsetRepairShippingLabel.JPEG" height="392" width="651" alt="Shipping Label"/>
</td>
</tr></table>
</div>
</body>
</html>

