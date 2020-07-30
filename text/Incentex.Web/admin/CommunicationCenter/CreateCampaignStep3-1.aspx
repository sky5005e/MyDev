<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CreateCampaignStep3-1.aspx.cs" Inherits="admin_CommunicationCenter_CreateCampaignStep3_1"
    Title="CampaignStep3-1 Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link media="screen" rel="stylesheet" href="../../CSS/colorbox.css" />
    <style type="text/css">
        .custom-checkbox input, .custom-checkbox_checked input
        {
            width: 20px;
            height: 20px;
            margin-left: -8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        // Let's use a lowercase function name to keep with JavaScript conventions
        function selectAll(invoker) {
            // Since ASP.NET checkboxes are really HTML input elements
            //  let's get all the inputs
            var inputElements = document.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                // Filter through the input types looking for checkboxes
                if (myElement.type === "checkbox") {
                    myElement.checked = invoker.checked;
                }
            }
        }
        function DeleteConfirmation() {
            var ans = confirm("Are you sure, you want to delete selected Campaign?");
            if (ans) {

                return true;
            }
            else {

                return false;

            }

        }
    </script>

    <script type="text/javascript" language="javascript">
        var formats = 'css|html';
        $().ready(function() {

            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {

                        ctl00$ContentPlaceHolder1$flFile: { required: true, accept: formats },
                        ctl00$ContentPlaceHolder1$txtTempName: { required: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$flFile: { required: "Please select file to upload.",
                            accept: replaceMessageString(objValMsg, "ImageType", "css,html")
                        },
                        ctl00$ContentPlaceHolder1$txtTempName: { required: replaceMessageString(objValMsg, "Required", "Template name") }
                    }

                });
                /*End Change Event of all the dropdowns*/
                $("#<%=btnSubmit.ClientID %>").click(function() {
                    return $('#aspnetForm').valid();
                });
            });
        });
        
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad" style="padding-left: 170px; padding-right:130px;">
        <div class="spacer25">
        </div>
         <div>
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="errormessage"></asp:Label>
        </div>
         <div class="spacer10">
        </div>
        <div class="bot_alert next">
            <asp:LinkButton ID="lnkNext" runat="server" title="Final Step" OnClick="lnkNext_Click">Step 5</asp:LinkButton>
        </div>
        <div class="bot_alert prev">
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        </div>
        
        <div>
            <table>
                <tr>
                    <td class="btn_width worldlink_btn">
                        <asp:LinkButton ID="lnkViewTemplates" class="gredient_btn1" runat="server" ToolTip="View Templates"
                            OnClick="lnkViewTemplates_Click">
                            <img src="../Incentex_Used_Icons/Veiw-template.png" alt="View Templates" />
                            <span>View Templates</span></asp:LinkButton>
                    </td>
                    <td class="btn_width worldlink_btn">
                        <asp:LinkButton ID="lnkpopupDummy2" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                        <asp:LinkButton ID="lnkUploadTemplates" class="gredient_btn1" runat="server" ToolTip="Upload Templates"
                            OnClick="lnkUploadTemplates_Click">
                            <img src="../Incentex_Used_Icons/upload_template.png" alt="Upload Templates" />
                            <span>Upload Templates</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div id="dvTemp" runat="server">
            <div style="text-align: center">
                <asp:Label ID="lblMsgGrid" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div class="spacer25">
            </div>
            <asp:GridView ID="gvTemplatesList" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" 
                OnRowDataBound="gvTemplatesList_RowDataBound" 
                onrowcommand="gvTemplatesList_RowCommand">
                <Columns>
                    <asp:TemplateField Visible="False" HeaderText="Id">
                        <HeaderTemplate>
                            <span>TempID</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblProductItemID" Text='<%# Eval("TempID") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="2%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Check">
                        <HeaderTemplate>
                            <asp:Label ID="lblselect" runat="server" Text="Select" />
                                <%--<span>
                                <asp:CheckBox ID="cbSelectAll" runat="server" OnClick="selectAll(this)" />&nbsp; 
                                </span>
                                --%>                            
                            <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </HeaderTemplate>
                        <HeaderStyle CssClass="centeralign" />
                        <ItemTemplate>
                            <asp:RadioButton CssClass="first btn_space" ID="chkTemp" runat="server" AutoPostBack="true"
                                GroupName='<%# Eval("TempID") %>' OnCheckedChanged="chkTemp_CheckedChanged" style="display:block;"/>
                            <asp:HiddenField ID="hdnTempvalue" runat="server" Value='<%# Eval("Tempfile") %>' />
                            <%--<asp:CheckBox ID="CheckBox1" runat="server" />&nbsp;--%>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="5%" CssClass="b_box centeralign" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="DateAdded">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnDateAdded" runat="server" CommandArgument="DateAdded" CommandName="Sort"><span>Date</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderDateAdded" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDate" Text='<%# Eval("CreatedDate")%>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="23%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="TempName">
                        <HeaderTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtnTempName" runat="server" CommandArgument="TempName" CommandName="Sort">Template Name</asp:LinkButton>
                            </span>
                            <asp:PlaceHolder ID="placeholderTempName" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblTempName" Text='<%# Eval("TempName")%>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ViewTemp">
                        <HeaderTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtnViewTemp" runat="server" CommandArgument="ViewTemp" CommandName="Sort">Details</asp:LinkButton>
                            </span>
                            <asp:PlaceHolder ID="placeholderViewTemp" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span>
                                <asp:HiddenField ID="hdnTempID" runat="server" Value='<%#Eval("TempID")%>' />
                                <asp:LinkButton ID="hypViewTemp" CommandName="open" runat="server" ToolTip="view templates" CommandArgument='<%# Eval("TempID") %>'>View Templates</asp:LinkButton>
                            </span>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span>Delete</span>
                            <div class="corner">
                                <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span class="btn_space">
                                <asp:LinkButton ID="lnkbtndelete" runat="server" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                    CommandArgument='<%# Eval("TempID") %>'>
                                    <asp:Image ID="img" ImageUrl="~/Images/close.png" runat="server" />
                                    </asp:LinkButton></span>
                            <div class="corner">
                                <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box centeralign" Width="10%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <at:ModalPopupExtender ID="modal" TargetControlID="lnkpopupDummy2" BackgroundCssClass="modalBackground"
            DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="cboxClose">
        </at:ModalPopupExtender>
        <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
            <div id="cboxWrapper" style="display: block; width: 458px; height: 300px; position: fixed;
                left: 35%; top: 30%;">
                <div style="">
                    <div id="cboxTopLeft" style="float: left;">
                    </div>
                    <div id="cboxTopCenter" style="float: left; width: 408px;">
                    </div>
                    <div id="cboxTopRight" style="float: left;">
                    </div>
                </div>
                <div style="clear: left;">
                    <div id="cboxMiddleLeft" style="float: left; height: 250px;">
                    </div>
                    <div id="cboxContent" style="float: left; width: 408px; display: block; height: 250px;">
                        <div id="cboxLoadedContent" style="display: block;">
                            <div style="padding: 10px;">
                                <div class="weatherDetails true" style="height: auto;">
                                    <div class="form_popup_box">
                                        <div class="label_bar">
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                        </div>
                                        <div class="label_bar">
                                            <label style="width: 100px;">
                                                File name:</label>
                                            <span>
                                                <input type="file" id="flFile" runat="server" /></span>
                                        </div>
                                        <div class="label_bar btn_padinn">
                                        </div>
                                        <div class="noteIncentexthumb centeralign" style="width: 100%; font-size: 12px;">
                                            <img src="../../Images/lightbulb.gif" style="z-index: -1" alt="note:" />&nbsp;Supported
                                            file formats are <b>.CSS | .HTML </b>
                                        </div>
                                        <div class="label_bar btn_padinn">
                                        </div>
                                        <div class="label_bar">
                                            <label style="width: 100px;">
                                                Template Name :</label>
                                            <span>
                                                <asp:TextBox Style="height: 23px;" ID="txtTempName" runat="server"></asp:TextBox>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="spacer5" style="clear: both;">
                                </div>
                                <div class="centeralign">
                                    <asp:Button ID="btnSubmit" CssClass="gredient_btn" Width="200px" Text="Add To Database"
                                        runat="server" OnClick="btnSubmit_Click" />
                                </div>
                            </div>
                            <div id="cboxLoadingOverlay" style="height: 250px; display: none;" class="">
                            </div>
                            <div id="cboxLoadingGraphic" style="height: 250px; display: none;" class="">
                            </div>
                            <div id="cboxTitle" style="display: block;" class="">
                            </div>
                            <div id="cboxClose" style="" class="">
                                close
                            </div>
                        </div>
                    </div>
                    <div id="cboxMiddleRight" style="float: left; height: 250px;">
                    </div>
                </div>
                <div style="clear: left;">
                    <div id="cboxBottomLeft" style="float: left;">
                    </div>
                    <div id="cboxBottomCenter" style="float: left; width: 408px;">
                    </div>
                    <div id="cboxBottomRight" style="float: left;">
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
