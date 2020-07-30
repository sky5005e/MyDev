<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="UpdateAllStoreStatus.aspx.cs" Inherits="admin_UpdateAllStoreStatus"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <style type="text/css">
        .textarea_box textarea
        {
            height: 178px;
        }
        .textarea_box
        {
            height: 178px;
        }
        .textarea_box .scrollbar
        {
            height: 185px;
        }
    </style>
    <script type="text/javascript" language="javascript">
          $(function() {
            scrolltextarea(".scrollme2", "#Scrolltop2", "#ScrollBottom2");

        });

        $().ready(function() {
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        
                        ctl00$ContentPlaceHolder1$ddlStoreStatus: { NotequalTo: "0" }
                    },
                    messages: {
                        
                        ctl00$ContentPlaceHolder1$ddlStoreStatus: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "store status") }

                    }
                });
            });

            $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });
        });

    </script>
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
                                <span class="custom-sel ">
                                    <asp:DropDownList ID="ddlStoreStatus" onchange="pageLoad(this,value);" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlStoreStatus_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </span>
                            </div>
                            
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trStatusMessage" runat="server" style="display: none">
                    <td width="500px">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box employeeedit_text clearfix">
                                <span class="input_label alignleft">Message</span>
                                <div class="textarea_box alignright">
                                    <div class="scrollbar">
                                        <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                        </a>
                                    </div>
                                    <textarea id="txtAddress" rows="10" runat="server" class="scrollme2" tabindex="1"></textarea>
                                </div>
                                <div id="dvtxtAddress">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="spacer10">
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Save Basic Information"
                            OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
                        
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
