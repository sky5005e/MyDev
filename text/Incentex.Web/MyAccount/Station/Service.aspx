<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Service.aspx.cs" Inherits="admin_Company_Station_AddService"  %>
<%--<%@ Register Src="~/admin/Company/Station/StationSubMenu.ascx" TagName="StationSubManu" TagPrefix="uc" %>--%>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript">


        $(function() {
           // scrolltextarea(".scrollme", "#Scrolltop1", "#ScrollBottom1");
            //scrolltextarea(".scrollme2", "#Scrolltop2", "#ScrollBottom2");
        //scrolltextarea(".scrollme3", "#Scrolltop3", "#ScrollBottom3");
            
         //   scrolltextarea(".scrollme4", "#Scrolltop4", "#ScrollBottom4");
          //  scrolltextarea(".scrollme5", "#Scrolltop5", "#ScrollBottom5");
            //scrolltextarea(".scrollme6", "#Scrolltop6", "#ScrollBottom6");

        });


        function CheckNum(id) {
            var txt = document.getElementById(id);
            if (!IsNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.value = "";
                txt.focus();
             
            }
            else {
                GetSum();
            }
        }

        function GetSum() {
            var add = 0;
            $(':text').each(function() {
                add += Number($(this).val());
            });
            //alert(add);
            //alert($("#spTotalEmp").html());
            $("#spTotalEmp").html(add);
        }

        $(function() {
            GetSum();
        });

        function IsNumeric(sText) {
            //var ValidChars = "0123456789.";
            var ValidChars = "0123456789";
            var IsNumber = true;
            var Char;


            for (i = 0; i < sText.length && IsNumber == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    IsNumber = false;
                }
            }
            return IsNumber;

        }


</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<uc:StationSubManu runat="server" ID="UcSubMenu" StationInfo="ManagerInfo_2" />--%>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
<uc:MenuUserControl ID="manuControl" runat="server" />
 <div class="form_pad">
       <div class="form_table addedit_pad">
        <div style="text-align:center" >
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" ></asp:Label>
                </div>
            <h4>Station Services</h4>
						<div>
						<%--	<table class="form_table">
								<tr>
									<td class="formtd_left">
										<table class="station_check_con">
											<tr>
												<td>
													<div class="station_con">
														<div class="form_top_co"><span>&nbsp;</span></div>
														<div class="form_box">
															<span class="input_label">Pax Service Agents</span>
															<input type="text"  class="w_label"/>
														</div>
														<div class="form_bot_co"><span>&nbsp;</span></div>
													</div>
													<div class="alignright station_checkbox"><input type="checkbox" /></div>
												</td>
											</tr>
											<tr>
												<td>
													<div class="station_con">
														<div class="form_top_co"><span>&nbsp;</span></div>
														<div class="form_box">
															<span class="input_label">Security Agents</span>
															<input type="text"  class="w_label"/>
														</div>
														<div class="form_bot_co"><span>&nbsp;</span></div>
													</div>
													<div class="alignright station_checkbox"><input type="checkbox" /></div>
												</td>
											</tr>
											<tr>
												<td>
													<div class="station_con">
														<div class="form_top_co"><span>&nbsp;</span></div>
														<div class="form_box">
															<span class="input_label">Fueling Agents</span>
															<input type="text"  class="w_label"/>
														</div>
														<div class="form_bot_co"><span>&nbsp;</span></div>
													</div>
													<div class="alignright station_checkbox"><input type="checkbox" /></div>
												</td>
											</tr>
											<tr>
												<td>
													<div class="station_con">
														<div class="form_top_co"><span>&nbsp;</span></div>
														<div class="form_box">
															<span class="input_label">Supervisors</span>
															<input type="text"  class="w_label"/>
														</div>
														<div class="form_bot_co"><span>&nbsp;</span></div>
													</div>
													<div class="alignright station_checkbox"><input type="checkbox" /></div>
												</td>
											</tr>
											<tr>
												<td>
													<div class="station_con">
														<div class="form_top_co"><span>&nbsp;</span></div>
														<div class="form_box">
															<span class="input_label">Cabin Cleaners</span>
															<input type="text"  class="w_label"/>
														</div>
														<div class="form_bot_co"><span>&nbsp;</span></div>
													</div>
													<div class="alignright station_checkbox"><input type="checkbox" /></div>
												</td>
											</tr>
											<tr>
												<td>
													<div class="station_con">
														<div class="form_top_co"><span>&nbsp;</span></div>
														<div class="form_box">
															<span class="input_label">Cargo Agents</span>
															<input type="text"  class="w_label"/>
														</div>
														<div class="form_bot_co"><span>&nbsp;</span></div>
													</div>
													<div class="alignright station_checkbox"><input type="checkbox" /></div>
												</td>
											</tr>
										</table>
									</td>
									<td class="formtd_right">
										<table class="station_check_con">
											<tr>
												<td>
													<div class="station_con">
														<div class="form_top_co"><span>&nbsp;</span></div>
														<div class="form_box">
															<span class="input_label">Ramp Agents</span>
															<input type="text"  class="w_label"/>
														</div>
														<div class="form_bot_co"><span>&nbsp;</span></div>
													</div>
													<div class="alignright station_checkbox"><input type="checkbox" /></div>
												</td>
											</tr>
											<tr>
												<td>
													<div class="station_con">
														<div class="form_top_co"><span>&nbsp;</span></div>
														<div class="form_box">
															<span class="input_label">Maintenance Crews</span>
															<input type="text"  class="w_label"/>
														</div>
														<div class="form_bot_co"><span>&nbsp;</span></div>
													</div>
													<div class="alignright station_checkbox"><input type="checkbox" /></div>
												</td>
											</tr>
											<tr>
												<td>
													<div class="station_con">
														<div class="form_top_co"><span>&nbsp;</span></div>
														<div class="form_box">
															<span class="input_label">Bus Drivers</span>
															<input type="text"  class="w_label"/>
														</div>
														<div class="form_bot_co"><span>&nbsp;</span></div>
													</div>
													<div class="alignright station_checkbox"><input type="checkbox" /></div>
												</td>
											</tr>
											<tr>
												<td>
													<div class="station_con">
														<div class="form_top_co"><span>&nbsp;</span></div>
														<div class="form_box">
															<span class="input_label">Dispatch Agents</span>
															<input type="text"  class="w_label"/>
														</div>
														<div class="form_bot_co"><span>&nbsp;</span></div>
													</div>
													<div class="alignright station_checkbox"><input type="checkbox" /></div>
												</td>
											</tr>
											<tr>
												<td>
													<div class="station_con">
														<div class="form_top_co"><span>&nbsp;</span></div>
														<div class="form_box">
															<span class="input_label">Deicers</span>
															<input type="text"  class="w_label"/>
														</div>
														<div class="form_bot_co"><span>&nbsp;</span></div>
													</div>
													<div class="alignright station_checkbox"><input type="checkbox" /></div>
												</td>
											</tr>
											<tr>
												<td>
													<div class="station_con">
														<div class="form_top_co"><span>&nbsp;</span></div>
														<div class="form_box">
															<span class="input_label">Cargo Customer Service</span>
															<input type="text"  class="w_label"/>
														</div>
														<div class="form_bot_co"><span>&nbsp;</span></div>
													</div>
													<div class="alignright station_checkbox"><input type="checkbox" /></div>
												</td>
											</tr>
											<tr>
												<td>Total Station Employees: 75</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>--%>
							
							
                            <asp:DataList ID="lst" runat="server" ondatabinding="lst_DataBinding"
                             RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Table"
                            > 
                            <HeaderTemplate>
                                <table class="form_table">
                                    <tr>
                                    <td>
                            </HeaderTemplate>
                                <ItemTemplate>
                                    <table class=" formtd_left" style="width:310px!important">
                                        <tr>
                                            <td class="station_check_con">
                                                 <div class="station_con">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">
                                                            <asp:Label ID="lblId" runat="server" Text=<%#Eval("iLookupID")%> Visible="false" ></asp:Label>
                                                            <%#Eval("sLookupName")%>
                                                        </span>                                                        
                                                        <asp:TextBox ID="txtVal" runat="server" onchange="CheckNum(this.id)" CssClass="w_label"></asp:TextBox>
                                                        
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </div>
                                                <div class="alignright station_checkbox" id="divChk" runat="server">
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <FooterTemplate>
                                        
                                    </td>
                                    </tr>
                                    <tr>
												<td>Total Station Employees: <span id="spTotalEmp">75</span></td>
											</tr>
                                    </table>
                                </FooterTemplate>
                            </asp:DataList>
										
							
						</div>	
						<div class="alignnone spacer25"></div>
						<div class="additional_btn">
							<ul class="clearfix">
								<li>
								    <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" 
                                        onclick="lnkSave_Click">
								        <span>Save Information</span>
								    </asp:LinkButton>
								</li>
							</ul>
						</div>  
       </div>
</div>
</asp:Content>

