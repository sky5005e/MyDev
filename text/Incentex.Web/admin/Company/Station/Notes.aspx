<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="Notes.aspx.cs" Inherits="admin_Company_Station_Notes"  %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script language="javascript" type="text/javascript">


        $(function() {
           // scrolltextarea(".scrollme", "#Scrolltop1", "#ScrollBottom1");
            //scrolltextarea(".scrollme2", "#Scrolltop2", "#ScrollBottom2");
        //scrolltextarea(".scrollme3", "#Scrolltop3", "#ScrollBottom3");
            
         //   scrolltextarea(".scrollme4", "#Scrolltop4", "#ScrollBottom4");
          //  scrolltextarea(".scrollme5", "#Scrolltop5", "#ScrollBottom5");
            scrolltextarea(".scrollme6", "#Scrolltop6", "#ScrollBottom6");

        });

        $().ready(function() {

        var objValMsg;

        $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
            objValMsg = $.xml2json(xml);
            $("#aspnetForm").validate({
                rules: {}//rules
            , messages: {}//messages
            , onsubmit: false
            }); //validate
        });

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
        
        });

</script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ajax:ToolkitScriptManager ID="sc1"	runat="server"></ajax:ToolkitScriptManager>
<uc:MenuUserControl ID="manuControl" runat="server" />
    <div class="form_pad">
        <div class="form_table addedit_pad">
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            
            <div>
							<div class="form_top_co"><span>&nbsp;</span></div>
							<div class="form_box taxt_area clearfix" style="height:180px">
								<span class="input_label alignleft" style="height:178px">Notes/History</span>
								<div class="textarea_box alignright">
									<div class="scrollbar" style="height:182px">
									    <A href="#scroll" id="Scrolltop6" class="scrolltop"></A>
									    <A href="#scroll" id="ScrollBottom6" class="scrollbottom"></A></div>
									<%--<textarea name="" cols="" rows="" class="scrollme6">Under the last seasonal weather Icon please create a notes/history section.</textarea>--%>
									<asp:TextBox ID="txtNoteHistory" runat="server" TextMode="MultiLine" 
									    CssClass="scrollme6"
									    ReadOnly="true"
									    Rows="12"
									     Height="178px"
									></asp:TextBox>
								</div>
							</div>
							<div class="form_bot_co"><span>&nbsp;</span></div>
						</div>
						<div class="alignnone spacer15"></div>
						<div class="rightalign gallery">
						 <asp:LinkButton ID="lnkDummyAddNote" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                   
								    <asp:LinkButton ID="lnkAddNote" runat="server" CssClass="grey2_btn alignright" 
                                        onclick="lnkAddNote_Click" >
								    <span>+ Add Note</span>
								    </asp:LinkButton>
								     <ajax:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkDummyAddNote" BackgroundCssClass="modalBackground"
                                        DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="closepopup"
                                        >
                                     </ajax:ModalPopupExtender>
<%--						<a href="#inline_popup1" class="grey2_btn" rel="prettyPhoto[inline]"><span>+ Add Note</span></a>
--%>						</div>
						<!-- Pop up -->
       
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
                                                                <asp:TextBox Height="120" Width="350" TextMode="MultiLine" 
                                                                    ID="txtNote" 
                                                                    runat="server"
                                                                   
                                                                    ></asp:TextBox></span>
                                                        </div>
                                                       
                                                      
                                                        <div class="additional_btn">
            <ul class="clearfix" >
                <li>
                    <%--<a href="#" title="Save Information" class="grey2_btn"><span>Save Information</span></a>--%>
                    <asp:LinkButton ID="lnkSaveNote" runat="server" CssClass="grey2_btn" OnClick="lnkSaveNote_Click" >
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
    
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

