<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AddIncentexEmployee.aspx.cs" Inherits="admin_UserManagement_AddIncentexEmployee" Title="World-Link System-Add Incentex Employee" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl" TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="src" runat="server"></asp:ScriptManager>
<mb:MenuUserControl ID="manuControl" runat="server" />
<div class="class="form_pad"">
 <asp:MultiView
        id="mvIncentexEmployee"
        ActiveViewIndex="0"
        Runat="server">
        <asp:View ID="viewContactInformation" runat="server">
           <h4>Contact Information</h4>
					<div>
						<table class="form_table">
							<tr>
								<td class="formtd">
									<table>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box">
														<span class="input_label">Company Name</span>
													    <asp:TextBox ID="txtComName" class="w_label" runat="server" ></asp:TextBox>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box">
														<span class="input_label">Title</span>
														<asp:TextBox ID="txtTitle" class="w_label" runat="server" ></asp:TextBox>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<div>
                                        <asp:UpdatePanel ID="upnlCountry" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Country</span> <span class="custom-sel label-sel">
                                                        <asp:DropDownList ID="ddlCountry" runat="server" onchange="pageLoad(this,value);" AutoPostBack="True" >
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
											</td>
										</tr>
										<tr>
											<td>
												 <asp:UpdatePanel ID="upnlSate" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">State</span> <span class="custom-sel label-sel">
                                                        <asp:DropDownList ID="ddlState" runat="server" onchange="pageLoad(this,value);" AutoPostBack="True" >
                                                            <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div>
                                                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upnlCountry">
                                                            <ProgressTemplate>
                                                                <img src="Images/ajaxbtn.gif" class="progress_img" /></ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
											</td>
										</tr>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box">
														<span class="input_label">Fax</span>
														<input type="text" class="w_label"/>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box">
														<span class="input_label">Skype Name</span>
														<input type="text" class="w_label"/>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
									</table>
								</td>
								<td class="formtd">
									<table>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box">
														<span class="input_label">First Name</span>
														<input type="text" class="w_label"/>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box">
														<span class="input_label">Department</span>
														<input type="text" class="w_label"/>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box">
														<span class="input_label">State/Province</span>
														<input type="text" class="w_label"/>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box">
														<span class="input_label">Telephone</span>
														<input type="text" class="w_label"/>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box">
														<span class="input_label">Mobile</span>
														<input type="text" class="w_label"/>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
									</table>
								</td>
								<td class="formtd_r">
									<table>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box">
														<span class="input_label">Last Name</span>
														<input type="text" class="w_label"/>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box employeeedit_text clearfix">
														<span class="input_label alignleft">Address</span>
														<div class="textarea_box alignright">
															<div class="scrollbar"><A href="#scroll" id="Scrolltop1" class="scrolltop"></A><A href="#scroll" id="ScrollBottom1" class="scrollbottom"></A></div>
															<textarea name="" cols="" rows="" class="scrollme"></textarea>
														</div>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box">
														<span class="input_label">Zip Code</span>
														<input type="text" class="w_label"/>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box">
														<span class="input_label">Extension</span>
														<input type="text" class="w_label"/>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
										<tr>
											<td>
												<div>
													<div class="form_top_co"><span>&nbsp;</span></div>
													<div class="form_box">
														<span class="input_label">Email</span>
														<input type="text" class="w_label"/>
													</div>
													<div class="form_bot_co"><span>&nbsp;</span></div>
												</div>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</div>
					<div class="divider"></div>
					<h4>Employee Type</h4>
					<div class="form_table">
						<table class="checktable_supplier true">
							<tr>
								<td class="formtd"><span class="custom-checkbox alignleft"><input type="checkbox" /></span><label>Direct Company Employee</label></td>
								<td><span class="custom-checkbox alignleft"><input type="checkbox" /></span><label>Independent Contractor</label></td>
								<td class="formtd_r"></td>
							</tr>
						</table>
					</div> 
        </asp:View>        
        <asp:View ID="View2" runat="server">
            <br />This is the second view
            <br />This is the second view
            <br />This is the second view
            <br />This is the second view
        </asp:View>        
        <asp:View ID="View3" runat="server">
            <br />This is the third view
            <br />This is the third view
            <br />This is the third view
            <br />This is the third view
        </asp:View>        
    </asp:MultiView>
</div>
</asp:Content>


