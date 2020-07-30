<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="FactoryOrPersonalInfo.aspx.cs" Inherits="admin_Supplier_FactoryOrPersonalInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl" TagPrefix="mb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <script language="javascript" type="text/javascript">

        $(function() {
             scrolltextarea(".scrollme3", "#Scrolltop3", "#ScrollBottom3");

         });


         $().ready(function() {
             $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {

                 objValMsg = $.xml2json(xml);
                 //alert(objValMsg);

                 $("#aspnetForm").validate({
                     rules: {
                         ctl00$ContentPlaceHolder1$txtAnnualPriceOfferReviewDate: { date: true }
                     , ctl00$ContentPlaceHolder1$txtBirthday: { date: true }
                     , ctl00$ContentPlaceHolder1$fluDocument: { required: true }

                    }//rules
                      , messages:
                    {
                        ctl00$ContentPlaceHolder1$txtFirstName: {
                            date: replaceMessageString(objValMsg, "ValidDate", "")
                        }
                        , ctl00$ContentPlaceHolder1$txtBirthday: {
                            date: replaceMessageString(objValMsg, "ValidDate", "")
                        }

                    }//messages
                     , onsubmit: false
                }); //validate
                     
                 });//get

                 $("#<%=lnkSave.ClientID %>").click(function() {
                     //remove required rule for document
                     $("#ctl00_ContentPlaceHolder1_fluDocument").rules("remove");
                     $("#ctl00_ContentPlaceHolder1_fluMasterPrice").rules("remove");
                     $("#ctl00_ContentPlaceHolder1_txtNote").rules("remove");

                     return $('#aspnetForm').valid();
                 }); //click



                 //btnSubmit click
                 $("#ctl00_ContentPlaceHolder1_btnSubmit").click(function() {

                     $("#ctl00_ContentPlaceHolder1_fluDocument").rules("add",
                            { required: true,
                                messages: { required: replaceMessageString(objValMsg, "Required", "file") }
                            });

                     if ($('#ctl00_ContentPlaceHolder1_fluDocument').valid()) {
                         return true;
                     }
                     else {
                         return false;
                     }

                 }); //click


                 $("#ctl00_ContentPlaceHolder1_btnUploadMasterPrice").click(function() {

                     $("#ctl00_ContentPlaceHolder1_fluMasterPrice").rules("add",
                            { required: true,
                                messages: { required: replaceMessageString(objValMsg, "Required", "file") }
                            });

                     if ($('#ctl00_ContentPlaceHolder1_fluMasterPrice').valid()) {
                         return true;
                     }
                     else {
                         return false;
                     }
                 }); //click


                 $("#ctl00_ContentPlaceHolder1_lnkSaveNote").click(function() {

                     $("#ctl00_ContentPlaceHolder1_txtNote").rules("add",
                            { required: true,
                                messages: { required: replaceMessageString(objValMsg, "Required", "Notes") }
                            });

                     if ($('#ctl00_ContentPlaceHolder1_txtNote').valid()) {
                         return true;
                     }
                     else {
                         return false;
                     }
                 }); //click


             });                    //ready
                    

         $(function() {
             $(".datepicker").datepicker({
                 buttonText: 'Date',
                 showOn: 'button',
                 buttonImage: '../../images/calender-icon.jpg',
                 buttonImageOnly: true
             });
         });
         
           </script>
           
           <style type="text/css"> 
            .form_table input.w_label {
                    margin-right:-5px;
                    width:37%;
            }
           </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<asp:ScriptManager id="sc1" runat="server"></asp:ScriptManager>--%>
<ajax:ToolkitScriptManager ID="sc1" runat="server"></ajax:ToolkitScriptManager>
        <mb:MenuUserControl ID="manuControl" runat="server" />

    <div class="form_pad">
        <div style="text-align:center" >
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" ></asp:Label>
                </div>
   
					<h4>Factory Information</h4>
					<div>
						<table class="form_table">
							<tr>
								<td class="formtd">
									<div class="shipmax_in btn_leval_pad">
										<div class="form_top_co"><span>&nbsp;</span></div>
										<div class="form_box">
											<span class="input_label max_w">Hours of Operation</span>
											<asp:TextBox ID="txtHoursOfOperation" runat="server" CssClass="w_label min_w"></asp:TextBox>
											<%--<input type="text" class="w_label min_w"/>--%>
											
										</div>
										<div class="form_bot_co"><span>&nbsp;</span></div>
									</div>
								</td>
							<%--	<td class="formtd">
									<div class="btn_leval_pad">
										<div class="form_top_co"><span>&nbsp;</span></div>
										<div class="form_box">
											<span class="custom-sel">
												<select>
													<option>Select</option>
												</select>
											</span>
										</div>
										<div class="form_bot_co"><span>&nbsp;</span></div>
									</div>
								</td>--%>
								<td>
									<%--<a href="#" class="greyicon_btn btn" title="List Vacations & Supplier Closings"><span class="list_btn">List Vacations & Supplier Closings<img src="../../images/upload-product-icon.png" alt=""/></span></a>--%>
								
								</td>
							</tr>
							<tr>
							    <td>
							        	<asp:LinkButton ID="lnkVacations" runat="server" CssClass="greyicon_btn btn" 
                                        onclick="lnkVacations_Click">
									    <span class="list_btn">List Vacations & Supplier Closings<img src="../../images/upload-product-icon.png" alt=""/></span>    
									</asp:LinkButton>
							    </td>
							</tr>
							<tr>
							
							    <td colspan="2" align="center">
							   
							        <asp:GridView ID="gvDocumnetVacations" runat="server" 
                                        RowStyle-CssClass="ord_content" AutoGenerateColumns="false"
                                                    HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                                    
                                        ondatabinding="gvDocumnetVacations_DataBinding" onrowcommand="gvDocumnetVacations_RowCommand" 
                                        Width="50%"
                                                     >
                                                    
                                                    <RowStyle CssClass="ord_content"></RowStyle>
                                                    <Columns>
                                                        <asp:TemplateField Visible="False" HeaderText="Id">
                                                            <HeaderTemplate>
                                                                <span>Document ID</span>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblDocumentID" Text='<%# Eval("DocumentId") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    
                                                        <asp:TemplateField SortExpression="FileName">
                                                            <HeaderTemplate>
                                                            <span >File Name</span>
                                                                <%--<asp:LinkButton ID="lnkbtnFileName" runat="server" CommandArgument="FileName" CommandName="Sort"><span >File Name</span></asp:LinkButton>--%>
                                                             
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                           <span>    <%-- <asp:LinkButton runat="server" ID="lnkFileName" CommandName="view" CommandArgument='<%# Eval("FileName") %>'
                                                                    Text='<%# Eval("FileName") %>'></asp:LinkButton>--%>
                                                                    <asp:HyperLink ID="lnkDoc" runat="server" Text=<%#Eval("OriginalFileName")%>
                                                                        NavigateUrl=<%# "~/UploadedImages/SupplierDocuments/" + Eval("FileName")%>
                                                                        onclick="window.open(this.href);return false;"
                                                                      
                                                                    > </asp:HyperLink></span>
                                                                  <%--  <a href="about:blank" onclick="window.open(this.href);return false;" >Open</a>--%>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="b_box" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <span>Delete</span>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                           <asp:HiddenField ID="hdnFileName" runat="server" Value=<%#Eval("FileName")%> />
                                                                <%--<asp:LinkButton ID="lnkbtndelete" 
                                                                CommandName="del" OnClientClick="return confirm('Are you sure, you want to delete selected records ?');"
                                                                    CommandArgument=<%#Eval("DocumentId")%>  runat="server">
                                                                    <span><asp:Image id="delete"  ImageUrl="~/Images/close.png" runat="server" /></span></asp:LinkButton>--%>
                                                                  
                                                                  <span style="height:26px">  <asp:ImageButton ID="lnkbtndelete" runat="server" ImageUrl="~/Images/close.png" 
                                                                    CommandName="del" OnClientClick="return confirm('Are you sure, you want to delete selected records ?');"
                                                                     CommandArgument=<%#Eval("DocumentId")%>
                                                                    /></span>
                                                            </ItemTemplate>
                                                            <ItemStyle  CssClass="g_box" Width="10%"  />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                
							    </td>
							</tr>
						</table>
					</div>
					<div class="divider"></div>
					<h4>Supplier's Price Offers List</h4>
					<div>
						<table class="form_table">
							<tr>
								<td class="formtd" style=>
								<%--<a href="#" class="greyicon_btn btn" title="Upload Master Price Offer List"><span class="btn_width285">Upload Master Price Offer List<img src="../../images/upload-supplier-icon.png" alt=""/></span></a>--%>
								
								<asp:LinkButton ID="lnkMasterPrice" runat="server" CssClass="greyicon_btn btn" 
                                        onclick="lnkMasterPrice_Click">
								    <span class="btn_width285">Upload Master Price Offer List<img src="../../images/upload-supplier-icon.png" alt=""/></span>
								</asp:LinkButton>
								
								</td>
								<td >
									
								</td>
								
							</tr>
							<tr>
							    <td colspan="2">
							        <!-- document grid -->
							        
							        <asp:GridView ID="gvDocumnetMasterPrice" runat="server" 
                                        RowStyle-CssClass="ord_content" AutoGenerateColumns="false"
                                                    HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                                    
                                      
                                        Width="50%" ondatabinding="gvDocumnetMasterPrice_DataBinding" onrowcommand="gvDocumnetMasterPrice_RowCommand"
                                                     >
                                                    
                                                    <RowStyle CssClass="ord_content"></RowStyle>
                                                    <Columns>
                                                        <asp:TemplateField Visible="False" HeaderText="Id">
                                                            <HeaderTemplate>
                                                                <span>Document ID</span>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblDocumentID" Text='<%# Eval("DocumentId") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    
                                                        <asp:TemplateField SortExpression="FileName">
                                                            <HeaderTemplate>
                                                            <span >File Name</span>
                                                                <%--<asp:LinkButton ID="lnkbtnFileName" runat="server" CommandArgument="FileName" CommandName="Sort"><span >File Name</span></asp:LinkButton>--%>
                                                             
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                            <span>
                                                               <%-- <asp:LinkButton runat="server" ID="lnkFileName" CommandName="view" CommandArgument='<%# Eval("FileName") %>'
                                                                    Text='<%# Eval("FileName") %>'></asp:LinkButton>--%>
                                                                    <asp:HyperLink ID="lnkDoc" runat="server" Text=<%#Eval("OriginalFileName")%>
                                                                        NavigateUrl=<%# "~/UploadedImages/SupplierDocuments/" + Eval("FileName")%>
                                                                        onclick="window.open(this.href);return false;"
                                                                        
                                                                    > </asp:HyperLink>
                                                                  <%--  <a href="about:blank" onclick="window.open(this.href);return false;" >Open</a>--%>
                                                                  </span>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="b_box" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <span>Delete</span>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                             <asp:HiddenField ID="hdnFileName" runat="server" Value=<%#Eval("FileName")%> />
                                                              <%--<asp:LinkButton ID="lnkbtndelete" CommandName="del" OnClientClick="return confirm('Are you sure, you want to delete selected records ?');"
                                                                    CommandArgument=<%#Eval("DocumentId")%>  runat="server">
                                                                   <span > <asp:Image id="delete"  ImageUrl="~/Images/close.png" runat="server" /> </span></asp:LinkButton>--%>
                                                                          <span style="height:26px;vertical-align:middle">  <asp:ImageButton ID="lnkbtndelete" runat="server" ImageUrl="~/Images/close.png" 
                                                                    CommandName="del" OnClientClick="return confirm('Are you sure, you want to delete selected records ?');"
                                                                    CommandArgument=<%#Eval("DocumentId")%>
                                                                    /> </span>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="g_box" Width="10%" />
                                                            
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
							        
							        <!-- document grid  end-->
							        
							        
							    </td>
							</tr>
							<tr>
							    <td class="btn_leval_pad" colspan="2">
							    <div class="calender_l" style="width:450px">
										<div class="form_top_co"><span>&nbsp;</span></div>
										<div class="form_box supplier_annual_date">
											<span class="input_label">Supplier's Annual Price Offer Review Date</span>
											<asp:TextBox ID="txtAnnualPriceOfferReviewDate" runat="server" CssClass="w_label datepicker"></asp:TextBox>
										</div>
										<div class="form_bot_co"><span>&nbsp;</span></div>
									</div>
							    </td>
							    
							</tr>
						</table>
					</div>
					<div class="divider"></div>
					<h4>Account Numbers</h4>
					<div>
						<table class="form_table">
							<tr>
								<td class="formtd">
									<div class="shipmax_in">
										<div class="form_top_co"><span>&nbsp;</span></div>
										<div class="form_box">
											<span class="input_label max_w">Supplier Account Number</span>
											<asp:TextBox ID="txtSupplierAccountNumber" runat="server" CssClass="w_label min_w"></asp:TextBox>
										</div>
										<div class="form_bot_co"><span>&nbsp;</span></div>
									</div>
								</td>
								<td class="formtd">
									<div class="shipmax_in">
										<div class="form_top_co"><span>&nbsp;</span></div>
										<div class="form_box">
											<span class="input_label max_w">Our Account Number</span>
											<asp:TextBox ID="txtOurAccountNumber" runat="server" CssClass="w_label min_w"></asp:TextBox>
										</div>
										<div class="form_bot_co"><span>&nbsp;</span></div>
									</div>
								</td>
								<td class="formtd_r"></td>
							</tr>
						</table>
					</div>
					<div class="divider"></div>
					<h4>Personal Information</h4>
					<div>
						<table class="form_table">
							<tr>
								<td class="formtd">
								
								 <div class="calender_l" >
										<div class="form_top_co"><span>&nbsp;</span></div>
										<div class="form_box supplier_annual_date">
											<span class="input_label">Birthday</span>
											<asp:TextBox ID="txtBirthday" runat="server" CssClass="w_label datepicker min_w"></asp:TextBox>
										</div>
										<div class="form_bot_co"><span>&nbsp;</span></div>
									</div>
								
									
								</td>
								<td class="formtd">
									<div>
										<div class="form_top_co"><span>&nbsp;</span></div>
										<div class="form_box">
											<span class="input_label">Spouses Name</span>
											
											<asp:TextBox ID="txtSpousesName" runat="server" CssClass="w_label"></asp:TextBox>
											
										</div>
										<div class="form_bot_co"><span>&nbsp;</span></div>
									</div>
								</td>
								<td class="formtd_r">
									<div>
										<div class="form_top_co"><span>&nbsp;</span></div>
										<div class="form_box">
											<span class="input_label">Childrens Name</span>
											<asp:TextBox ID="txtChildrensName" runat="server" CssClass="w_label"></asp:TextBox>
										</div>
										<div class="form_bot_co"><span>&nbsp;</span></div>
									</div>
								</td>
							</tr>
						</table>
					</div>
    <div class="divider"></div>
					<div>
						<table class="form_table">
							<tr>
								<td>
									<div>
										<div class="form_top_co"><span>&nbsp;</span></div>
										<div class="form_box taxt_area clearfix" style="height:180px">
								<span class="input_label alignleft" style="height:178px">Notes/History</span>
								<div class="textarea_box alignright">
									<div class="scrollbar" style="height:182px">
									    <A href="#scroll" id="Scrolltop3" class="scrolltop"></A>
									    <A href="#scroll" id="ScrollBottom3" class="scrollbottom"></A></div>
									<%--<textarea name="" cols="" rows="" class="scrollme6">Under the last seasonal weather Icon please create a notes/history section.</textarea>--%>
									<asp:TextBox ID="txtNoteHistory" runat="server" TextMode="MultiLine" 
									    CssClass="scrollme3"
									    ReadOnly="true"
									    Rows="12"
									     Height="178px"
									></asp:TextBox>
								</div>
							</div>
										<div class="form_bot_co"><span>&nbsp;</span></div>
									</div>
								</td>
							</tr>
							<tr>
								<td class="gallery" colspan="3">
								
								    <%--<a href="#inline_popup1"  class="grey2_btn alignright" ><span>+ Add Note</span></a>--%>
								     <asp:LinkButton ID="lnkDummyAddNote" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                   
								    <asp:LinkButton ID="lnkAddNote" runat="server" CssClass="grey2_btn alignright" 
                                        onclick="lnkAddNote_Click" >
								    <span>+ Add Note</span>
								    </asp:LinkButton>
								     <ajax:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkDummyAddNote" BackgroundCssClass="modalBackground"
                                        DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="closepopup"
                                        >
                                     </ajax:ModalPopupExtender>
                                     
                                     
								    </td>
							</tr>
						
        
						<%--	<div id="inline_popup1" style="display:none;">
								<div class="addnote_box clearfix">
									<h3 class="alignleft title_popup" >Jacob Martin</h3>
									<div class="alignright date_box">
										<span>December 07, 2010, 1:10 PM</span>
									</div>
								</div>
								<p class="popup_content">Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Cras metus. In gravida. Nulla vel justo in magna adipiscing vulputate. Fusce nunc tortor, facilisis nec, posuere sit amet, venenatis et, tortor. Proin turpis. Maecenas quis dolor lobortis nulla iaculis tempor. Nulla feugiat, dui sed sagittis dapibus, lacus augue imperdiet enim, id varius velit eros in dolor. Integer felis quam, imperdiet cursus, laoreet non, consequat a, enim.</p>
								<div class="rightalign bottombtn_popup"><a href="#" class="pop_close popupbtn"><span>Save Notes</span></a></div>
							</div>--%>
						</table>
					</div>
        <div class="additional_btn">
            <ul class="clearfix">
                <li>
                    <%--<a href="#" title="Save Information" class="grey2_btn"><span>Save Information</span></a>--%>
                    <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" 
                        onclick="lnkSave_Click">
								        <span>Save Information</span>
                    </asp:LinkButton>
                </li>
            </ul>
        </div>
        
        	
    </div>

<!-- Note Pop up -->
       
        <asp:Panel ID="pnlNotes" runat="server" Style="display: none;">
                    <div class="pp_pic_holder facebook" style="display: block; width: 411px; position:fixed;left:35%;top:30%;">
                        <div class="pp_top" style="">
                            <div class="pp_left">
                            </div>
                            <div class="pp_middle">
                            </div>
                            <div class="pp_right">
                            </div>
                        </div>
                        <div class="pp_content_container" style="">
                            <div class="pp_left" style="">
                                <div class="pp_right" style="">
                                    <div class="pp_content" style="height: 240px; display: block;">
                                       
                                        <div class="pp_fade" style="display: block;">
                                          
                                            <div id="pp_full_res">
                                                <div class="pp_inline clearfix">
                                                    <div class="form_popup_box">
                                                        <div class="label_bar">
                                                            <span>
                                                                  Add Notes / Hisory <br/><br/>
                                                                <asp:TextBox Height="120" Width="350" TextMode="MultiLine" ID="txtNote" runat="server" ></asp:TextBox></span>
                                                        </div>
                                                        <div class="additional_btn">
                                                            <ul class="clearfix">
                                                                <li>
                                                                    <%--<a href="#" title="Save Information" class="grey2_btn"><span>Save Information</span></a>--%>
                                                                    <asp:LinkButton ID="lnkSaveNote" runat="server" CssClass="grey2_btn" OnClick="lnkSaveNote_Click">
								                                      <span>Save Notes</span>
                                                                    </asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </div>
        
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="pp_details clearfix" style="width: 371px;">
                                                <a href="#" id="closepopup" runat="server" class="pp_close">Close</a>
                                                <p class="pp_description" style="display: none;">
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="pp_bottom" style="">
                            <div class="pp_left" style="">
                            </div>
                            <div class="pp_middle" style="">
                            </div>
                            <div class="pp_right" style="">
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                
             
                
          <!-- Pop up end -->
                   
          
          <!-- Vacations and Supplier closing popup -->
              
        
          <asp:LinkButton ID="lnkDummyAddNewVacations" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
<ajax:ModalPopupExtender ID="modalVacations" TargetControlID="lnkDummyAddNewVacations" BackgroundCssClass="modalBackground"
  DropShadow="true" runat="server" PopupControlID="pnlDocumentVacations" CancelControlID="closepopupVacations">
 </ajax:ModalPopupExtender>
 <asp:Panel ID="pnlDocumentVacations" runat="server" Style="display: none;">
                        <div class="pp_pic_holder facebook" style="display: block; width: 411px; position:fixed;left:35%;top:30%;">
                            <div class="pp_top" style="">
                                <div class="pp_left">
                                </div>
                                <div class="pp_middle">
                                </div>
                                <div class="pp_right">
                                </div>
                            </div>
                            <div class="pp_content_container" style="">
                                <div class="pp_left" style="">
                                    <div class="pp_right" style="">
                                        <div class="pp_content" style="height: 128px; display: block;">
                                            <div class="pp_loaderIcon" style="display: none;">
                                            </div>
                                            <div class="pp_fade" style="display: block;">
                                                <div id="Div1">
                                                    <div class="pp_inline clearfix">
                                                      
                                                        <div class="form_popup_box">
                                                            <div class="label_bar">
                                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                            </div>
                                                           
                                                            <div class="label_bar">
                                                                <label>
                                                                    Upload File :</label>
                                                                <span>
                                                                
                                                                    <asp:FileUpload ID="fluDocument" runat="server" />
                                                                    </span>
                                                            </div>
                                                            <div class="label_bar btn_padinn">
                                                            
                                                                <asp:Button ID="btnSubmit" Text="Upload" runat="server" OnClick="btnSubmit_Click" />
                                                                
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="pp_details clearfix" style="width: 371px;">
                                                    <a href="#" id="closepopupVacations" runat="server" class="pp_close">Close</a>
                                                    <p class="pp_description" style="display: none;">
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="pp_bottom" style="">
                                <div class="pp_left" style="">
                                </div>
                                <div class="pp_middle" style="">
                                </div>
                                <div class="pp_right" style="">
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
          
          <!-- Vacations and Supplier closing popup end -->
          
          
<!-- Upload Master Price Offer List -->
        
          <asp:LinkButton ID="lnkDummyAddNewMasterPrice" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
          
<ajax:ModalPopupExtender ID="modalMasterPrice" TargetControlID="lnkDummyAddNewMasterPrice" BackgroundCssClass="modalBackground"
  DropShadow="true" runat="server" PopupControlID="pnlDocumentMasterPrice" CancelControlID="closepopupMasterPrice">
 </ajax:ModalPopupExtender>
 <asp:Panel ID="pnlDocumentMasterPrice" runat="server" Style="display: none;">
                        <div class="pp_pic_holder facebook" style="display: block; width: 411px;position:fixed;left:35%;top:30%;">
                            <div class="pp_top" style="">
                                <div class="pp_left">
                                </div>
                                <div class="pp_middle">
                                </div>
                                <div class="pp_right">
                                </div>
                            </div>
                            <div class="pp_content_container" style="">
                                <div class="pp_left" style="">
                                    <div class="pp_right" style="">
                                        <div class="pp_content" style="height: 128px; display: block;">
                                            <div class="pp_loaderIcon" style="display: none;">
                                            </div>
                                            <div class="pp_fade" style="display: block;">
                                                <div id="Div2">
                                                    <div class="pp_inline clearfix">
                                                      
                                                        <div class="form_popup_box">
                                                            <div class="label_bar">
                                                                <asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                            </div>
                                                           
                                                            <div class="label_bar">
                                                                <label>
                                                                    Upload File :</label>
                                                                <span>
                                                                
                                                                    <asp:FileUpload ID="fluMasterPrice" runat="server" />
                                                                    </span>
                                                            </div>
                                                            <div class="label_bar btn_padinn">
                                                            
                                                                <asp:Button ID="btnUploadMasterPrice" Text="Upload" runat="server" onclick="btnUploadMasterPrice_Click" 
                                                                     />
                                                                
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="pp_details clearfix" style="width: 371px;">
                                                    <a href="#" id="closepopupMasterPrice" runat="server" class="pp_close">Close</a>
                                                    <p class="pp_description" style="display: none;">
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="pp_bottom" style="">
                                <div class="pp_left" style="">
                                </div>
                                <div class="pp_middle" style="">
                                </div>
                                <div class="pp_right" style="">
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

<!-- Upload Master Price Offer List end -->
    

</asp:Content>


