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
<td align="left" valign="top">

<ol class="small_text"> <li><b>Print the label:</b> &nbsp;
Select Print from the File menu in this browser window to print the label below.<br><br><li><b>
Fold the printed label at the dotted line.</b> &nbsp;
Place the label in a UPS Shipping Pouch. If you do not have a pouch, affix the folded label using clear plastic shipping tape over the entire label.<br><br><li><b>GETTING YOUR SHIPMENT TO UPS<br>
Customers without a Daily Pickup</b><ul><li>Take this package to any location of The UPS Store?, UPS Drop Box, UPS Customer Center, UPS Alliances (Office Depot? or Staples?) or Authorized Shipping Outlet near you or visit <a href="http://www.ups.com/content/us/en/index.jsx">www.ups.com/content/us/en/index.jsx</a> and select Drop Off.<li>
Air shipments (including Worldwide Express and Expedited) can be picked up or dropped off. To schedule a pickup, or to find a drop-off location, select the Pickup or Drop-off icon from the UPS tool bar.  </ul> <br> 
<b>Customers with a Daily Pickup</b><ul><li>
Your driver will pickup your shipment(s) as usual. </ul>
</ol></td></tr></table>
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

<img src="../UPSShippingReferences/ShippingLabelImage/ShippingLabel.JPEG" height="392" width="651" alt="Shipping Label"/>
</td>
</tr></table>
</div>
</body>
</html>

