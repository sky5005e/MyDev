<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AdditionalInfo.aspx.cs" Inherits="admin_Company_Station_AddAdditionalInfo" %>

<%--<%@ Register Src="~/admin/Company/Station/StationSubMenu.ascx" TagName="StationSubManu" TagPrefix="uc" %>--%>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript">


        $().ready(function() {
        
                

      


           

        });          //ready

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        
<%--<uc:StationSubManu runat="server" ID="UcSubMenu" StationInfo="AdditionalInfo_5" />

--%>
<ajax:ToolkitScriptManager ID="sc1"	runat="server"></ajax:ToolkitScriptManager>
<uc:MenuUserControl ID="manuControl" runat="server" />

	
				
 <div class="form_pad">
       <div class="form_table addedit_pad">
        <div style="text-align:center" >
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" ></asp:Label>
                </div>
           <%-- <h4>Additional Station Information</h4>
						<div>
							<div class="additional_btn">
								<ul>
									<li><a href="#" class="grey2_btn" title="Station Employees"><span>Station Employees</span></a></li>
									<li><a href="#" class="grey2_btn" title="Sales History"><span>Sales History</span></a></li>
									<li><a href="#" class="grey2_btn" title="Returns/Exchanges"><span>Returns/Exchanges</span></a></li>
									<li><a href="#" class="grey2_btn" title="Manpak History"><span>Manpak History</span></a></li>
									<li><a href="#" class="grey2_btn" title="Customers"><span>Customers<div class="divider"></div>
						                </span></a></li>
									<li><a href="#" class="grey2_btn" title="Product Suggestions"><span>Product Suggestions</span></a></li>
									<li><a href="#" class="grey2_btn" title="Upload Documents"><span>Upload Documents</span></a></li>
								</ul>
							</div>
						</div>
						<div class="alignnone">&nbsp;</div>--%>
						<div class="wether_header">
							<p>Seasonal Weather Conditions</p>
							<span>Please place a checkmark next to the weather conditions your station will experiance over a 12 month period?</span>
						</div>
						<div>
							
							
							<asp:GridView ID="gv" runat="server" ShowHeader="false"
							AutoGenerateColumns="false" ondatabinding="gv_DataBinding" GridLines="None"
							CssClass="weather_box"
							>
							    <Columns>
							    <asp:TemplateField Visible="false" >
							        <ItemTemplate>
							        <asp:Label ID="lblId" runat="server" Text=<%#Eval("iLookupID")%> ></asp:Label>
							        </ItemTemplate>
							    </asp:TemplateField>
							        <asp:TemplateField>
							            <ItemTemplate>
							                <div>
											<div>
												<div class="bl_top_co"><span>&nbsp;</span></div>
												<div class="bl_middle_bo">
													<div><img src="../../images/<%#Eval("sLookupIcon")%>" alt=""/>
													    <span>
													    <%#Eval("sLookupName") %>
													    </span></div>
												</div>
												<div class="bl_bot_co"><span>&nbsp;</span></div>
											</div>
										</div>
										
							            </ItemTemplate>
							            <AlternatingItemTemplate>
							                
							            <div>
											<div>
												<div class="form_top_co"><span>&nbsp;</span></div>
												<div class="form_box">
													<div><img src="../../images/<%#Eval("sLookupIcon")%>" alt=""/><span><%#Eval("sLookupName") %></span></div>
												</div>
												<div class="form_bot_co"><span>&nbsp;</span></div>
											</div>
										</div>
							            </AlternatingItemTemplate>
							            <ItemStyle CssClass="weather_left_box" />
							        </asp:TemplateField>
							        <asp:TemplateField>
							            <ItemTemplate>
							              
							                <div class="wheather_check " runat="server" id="dvChk">
							                    <asp:CheckBox ID="chk" runat="server"  />
							                  
							                </div>
							            </ItemTemplate>
							            <ItemStyle CssClass="rightalign" />
							        </asp:TemplateField>
							    </Columns>
							</asp:GridView>
							
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

