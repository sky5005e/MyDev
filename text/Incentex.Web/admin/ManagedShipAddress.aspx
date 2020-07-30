<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="ManagedShipAddress.aspx.cs" Inherits="admin_ManagedShipAddress" Title="Manage Ship To" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <style type="text/css">
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 36px;
        }
    </style>
    <script language="javascript" type="text/javascript">

        $(function() {
             scrolltextarea(".scrollme", "#Scrolltop1", "#ScrollBottom1");
  
        });

        $().ready(function() {
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {

                objValMsg = $.xml2json(xml);
                //alert(objValMsg);

                $("#aspnetForm").validate({
                rules: {
                ctl00$ContentPlaceHolder1$txtCompanyName: { required: true, alphanumeric: true }
                        ,ctl00$ContentPlaceHolder1$txtFirstName: { required: true, alphanumeric: true }
                    , ctl00$ContentPlaceHolder1$txtLastName: { required: true, alphanumeric: true }
                  
                        
                        , ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" }
                        , ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "0" }
                        , ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "0" }
                         , ctl00$ContentPlaceHolder1$txtAddress: { required: true }
                         , ctl00$ContentPlaceHolder1$txtTelephone: { required: true,alphanumeric: true }
                         , ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: true }
                        
                         
                    }
                    , messages:
                    {
                     ctl00$ContentPlaceHolder1$txtCompanyName: {
                            required: replaceMessageString(objValMsg, "Required", "company name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        }
                       , ctl00$ContentPlaceHolder1$txtFirstName: {
                            required: replaceMessageString(objValMsg, "Required", "first name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        }
                        , ctl00$ContentPlaceHolder1$txtLastName: {
                            required: replaceMessageString(objValMsg, "Required", "last name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        }
                        
                        , ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") }
                        , ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") }
                        , ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") }
                        
                        , ctl00$ContentPlaceHolder1$txtAddress: { required: replaceMessageString(objValMsg, "Required", "address") }
                         ,ctl00$ContentPlaceHolder1$txtTelephone: 
                         { 
                            required: replaceMessageString(objValMsg, "Required", "telephone"),
                            alphanumeric: replaceMessageString(objValMsg, "Number", "")
                         },
                         ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: replaceMessageString(objValMsg, "Number", "") }
                         

                    },
                    errorPlacement: function(error, element) {
                    if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtAddress")
                        error.insertAfter("#divtxtAddress");
                        else
                            error.insertAfter(element);
                    }

                });
            });

            $("#<%=lnkSave.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            }); //click

        });     //ready
    
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="sc1" runat="server"></asp:ScriptManager>

    <div class="form_pad">
      <div style="text-align:center" >
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" ></asp:Label>
                </div>
        <h4>
           Manage Ship To</h4>
        <div>
        
        <asp:UpdatePanel ID="up1" runat="server">   
            <ContentTemplate>
            
        
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Company Name</span>
                                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="w_label" TabIndex="1"></asp:TextBox>
                                            </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Title</span>
                                            <asp:TextBox ID="txtTitle" runat="server" CssClass="w_label" TabIndex="4"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                           
                            
                             <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">City</span>
                                             <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlCity" runat="server"  TabIndex="9" 
                                                      >
                                                        <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    
                                              
                                                </span>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Zip Code</span>
                                            <asp:TextBox ID="txtZip" runat="server" CssClass="w_label" TabIndex="10"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
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
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">First Name</span>
                                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="w_label" TabIndex="2"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                           <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Country</span>
                                            <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" 
                                                TabIndex="7" onselectedindexchanged="ddlCountry_SelectedIndexChanged" 
                                                      >
                                                        <asp:ListItem Text="-select country-" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    
                                              
                                                </span>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            
                         
                             <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box employeeedit_text clearfix">
                                            <span class="input_label alignleft">Address</span>
                                            <div class="textarea_box alignright">
                                                <div class="scrollbar">
                                                    <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                                        class="scrollbottom"></a>
                                                </div>
                                                <%--<textarea name="" cols="" rows="" class="scrollme"></textarea>--%>
                                                <asp:TextBox ID="txtAddress" runat="server" CssClass="scrollme colorblue" 
                                                    TextMode="MultiLine" TabIndex="6"></asp:TextBox>
                                            </div>
                                            <div id="divtxtAddress"></div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            
                            
                            
                             <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Extension</span>
                                            <asp:TextBox ID="txtExtension" runat="server" CssClass="w_label" TabIndex="12"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
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
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Last Name</span>
                                            <asp:TextBox ID="txtLastName" runat="server" CssClass="w_label" TabIndex="3"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                         
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">State/Province</span>
                                             <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" 
                                                TabIndex="8" onselectedindexchanged="ddlState_SelectedIndexChanged" 
                                                      >
                                                        <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    
                                              
                                                </span>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                          <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Telephone</span>
                                            <asp:TextBox ID="txtTelephone" runat="server" CssClass="w_label" TabIndex="11"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                           
                           
                        </table>
                    </td>
                </tr>
            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
        <div class="divider">
        </div>
        
        <div class="alignnone spacer25"></div>
						<div class="additional_btn">
							<ul class="clearfix">
								<li>
								    <%--<a href="#" title="Save Information" class="grey2_btn"><span>Save Information</span></a>--%>
								    <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" onclick="lnkSave_Click" 
                                    >
								        <span>Save Information</span>
								    </asp:LinkButton>
								</li>
							</ul>
						</div>
    </div>
   
</asp:Content>
