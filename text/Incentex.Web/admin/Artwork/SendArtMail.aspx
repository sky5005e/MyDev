<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="SendArtMail.aspx.cs" Inherits="admin_Artwork_SendArtMail" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
     $().ready(function()
    {
     $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtEmailTo: { required: true },
                        ctl00$ContentPlaceHolder1$txtSubject: { required: true },
                        ctl00$ContentPlaceHolder1$txtAddress: { required: true }
                        
                        
                        
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtEmailTo: { required: replaceMessageString(objValMsg, "Required", "Email To") },
                        ctl00$ContentPlaceHolder1$txtSubject: { required: replaceMessageString(objValMsg, "Required", "subject") },
                        ctl00$ContentPlaceHolder1$txtAddress: { required: replaceMessageString(objValMsg, "Required", "message")}
                        
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtEmailTo")
                            error.insertAfter("#dvEmail");
                     else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtSubject")
                            error.insertAfter("#dvSubject");
                       else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtAddress")
                            error.insertAfter("#dvMessage");
                        else
                            error.insertAfter(element);
                    }

                });
         });
         
          $("#<%=lnkBtnSendNow.ClientID %>").click(function() {
                return $('#aspnetForm').valid();

            });
    });
    </script>
    
    <script type="text/javascript" language="javascript">
     
        $(function() {
            scrolltextarea(".scrollme1", "#Scrolltop1", "#ScrollBottom1");
        });
    </script>

    <asp:ScriptManager ID="srcMgr" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="upPro" runat="server" DisplayAfter="100">
        <ProgressTemplate>
            <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
            </div>
            <div class="updateProgressDiv">
                <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upPanel" runat="server">
        <ContentTemplate>
            <div class="form_pad select_box_pad">
                <div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                </div>
                <div class="form_table">
                    <table>
                    <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <div class="noteIncentexThumb1" style="width: 100%; font-size: 12px;">
                                            <img src="../../Images/lightbulb.gif" alt="note:" />&nbsp; Press "Enter" to Insert
                                            more email address.without comma..
                                            <br />
                                            <br />
                                            <b>For Example:</b>
                                            <br />
                                            knelson@incentex.com
                                            <br />
                                            ken@incentex.com
                                        </div>
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
                                        <span class="input_label alignleft">Email To</span>
                                        <div class="textarea_box alignright" style="height:70px;">
                                            <div class="scrollbar" style="height:77px;">
                                                <a href="#scroll" id="A3" class="scrolltop"></a><a href="#scroll" id="A4" class="scrollbottom">
                                                </a>
                                            </div>
                                            <textarea id="txtEmailTo" style="height:72px;" rows="3" runat="server" class="scrollme2" tabindex="1"></textarea>
                                        </div>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                                <div id="dvEmail">
                                </div>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 20% !important;">Subject</span>
                                        <asp:TextBox ID="txtSubject" TabIndex="1" runat="server" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                                <div id="dvSubject">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box employeeedit_text clearfix">
                                        <span class="input_label alignleft">Message</span>
                                        <div class="textarea_box alignright" style="height:70px;">
                                            <div class="scrollbar" style="height:77px;">
                                                <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                                </a>
                                            </div>
                                            <textarea id="txtAddress" style="height:72px;" rows="3" runat="server" class="scrollme2" tabindex="2"></textarea>
                                        </div>
                                        <div id="dvtxtAddress">
                                        </div>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                                <div id="dvMessage">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="centeralign">
                                <asp:LinkButton ID="lnkBtnSendNow" class="grey2_btn" runat="server" ToolTip="Send Now"
                                  OnClick="lnkBtnSendNow_Click"><span>Send Now</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
