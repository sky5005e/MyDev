<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MyPassworChanged.aspx.cs" Inherits="MyAccount_MySettings_MyPassworChanged" Title="Password Section" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
   <script type="text/javascript" language="javascript">
        $().ready(function() {

            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                 $("#aspnetForm").validate({
                    rules: {
                        

                        //password

                        ctl00$ContentPlaceHolder1$TxtConfirmNewPassword: { equalTo: "#<%=TxtNewPassword.ClientID%>" }
                    },
                    messages: {
                      

                        
                        ctl00$ContentPlaceHolder1$TxtConfirmNewPassword: { equalTo: replaceMessageString(objValMsg, "EqualTo", "Passeord") }

                    }
                }); // form validate
                
                 $("#<%=lnkChangePassword.ClientID %>").click(function() {
                
                    $("#ctl00_ContentPlaceHolder1_TxtOldPassword").rules("add",
                    {
                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "old password") }
                    });
                });
            });

        });         //ready


        
    </script>

    <div class="alignnone">
        &nbsp;</div>
    <div class="form_pad select_box_pad" style="width: 400px">
    <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="form_table">
            <table>
               
                <tr>
                    <td>
                        <table>
                           
                            
                            <%--<div >--%>
                            <tr id="dvMinShippingAmount"  runat="server">
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Old Password</span>
                                             <asp:TextBox ID="TxtOldPassword" runat="server" TextMode="Password" TabIndex="1"
                                                CssClass="w_label"></asp:TextBox>
                                            
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="dvShippingOfSale"  runat="server">
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">New Password</span>
                                            <asp:TextBox ID="TxtNewPassword" runat="server" TextMode="Password" TabIndex="2"
                                                CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Confirm Password</span>
                                           <asp:TextBox ID="TxtConfirmNewPassword" runat="server" TabIndex="3" TextMode="Password"
                                                CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                     
                            <%--</div>--%>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkChangePassword" class="grey2_btn" runat="server" OnClick="lnkChangePassword_Click"
                            TabIndex="4"><span>Change Password</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>



