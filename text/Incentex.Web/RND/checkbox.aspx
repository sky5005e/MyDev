<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="checkbox.aspx.cs" Inherits="RND_checkbox" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script>
$(function()
	{
		scrolltextarea(".scrollme", "#Scrolltop1", "#ScrollBottom1");
		scrolltextarea(".scrollme2", "#Scrolltop2", "#ScrollBottom2");
		scrolltextarea(".scrollme3", "#Scrolltop3", "#ScrollBottom3");
		scrolltextarea(".scrollme4", "#Scrolltop4", "#ScrollBottom4");
		scrolltextarea(".scrollme5", "#Scrolltop5", "#ScrollBottom5");
	});
</script>

<div class="form_table">
						<table class="checktable_supplier true">
						<%--<tr>
						<td>
						<td class="formtd">
							<%--	 <span class="custom-checkbox alignleft"></span> --%>
								<%--<asp:CheckBoxList ID="chkpayments" RepeatDirection="ver`" runat="server"  class="custom-checkbox alignleft">
                                    <asp:ListItem Value="1">2% 10 Days - Prompt Pay Program</asp:ListItem>
                                    <asp:ListItem Value="2">Net 60 days</asp:ListItem>
                                    <asp:ListItem Value="3">Net 30 days</asp:ListItem>
                             	 </asp:CheckBoxList>--%>
							
								<%--<label>2% 10 Days - Prompt Pay Program</label>--%>
								</td>
						</td>
						</tr>--%>
							<tr>
							
							
								<td class="formtd">
								<span class="custom-checkbox alignleft">
								<input type="checkbox" />
								</span>
								<label>2% 10 Days - Prompt Pay Program</label>
								</td>
								<td class="formtd">
								<span class="custom-checkbox alignleft">
								<input type="checkbox" />
								</span>
								<label>Net 30 Days</label>
								</td>
								<td class="formtd_r">
								<span class="custom-checkbox alignleft">
								<input type="checkbox" />
								</span>
								<label>Net 45 Days</label>
								</td>
							</tr>
							<tr>
								<td class="formtd"><span class="custom-checkbox alignleft"><input type="checkbox" /></span><label>Net 60 Days</label></td>
								<td class="formtd"><span class="custom-checkbox alignleft"><input type="checkbox" /></span><label>Wire Transfer</label></td>
								<td class="formtd_r"><span class="custom-checkbox alignleft"><input type="checkbox" /></span><label>Purchase Card</label></td>
							</tr>
						</table>
					</div>

</asp:Content>


