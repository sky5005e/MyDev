<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="TailoringAndMeasurement.aspx.cs" Inherits="admin_ProductManagement_TailoringAndMeasurement"
    Title="Product >> Tailoring&Measurements" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <script type="text/javascript" language="javascript">
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }
    
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
            var formats = 'doc|txt|pdf|docx';

            $().ready(function() {
                $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                    objValMsg = $.xml2json(xml);
                    $("#aspnetForm").validate({
                        rules: {

                            ctl00$ContentPlaceHolder1$fpGuideLineUpload: { accept: formats },
                            ctl00$ContentPlaceHolder1$fpMeasurementsUpload: { accept: formats }
                        },
                        messages: {
                            ctl00$ContentPlaceHolder1$fpGuideLineUpload: { accept: "File type not supported." },
                            ctl00$ContentPlaceHolder1$fpMeasurementsUpload: { accept: "File type not supported." }
                        }
                    });

                    $("#<%=lnkTailorChart.ClientID %>").click(function() {
                        return $('#aspnetForm').valid();
                    });
                });
            });
        }
    </script>

    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=fpMeasurementsUpload.ClientID%>").value == "" && document.getElementById("<%=fpGuideLineUpload.ClientID %>").value == "") {

                var abv = document.getElementById("<%=lblGuidemessage.ClientID%>");
                abv.value = "Please select at least one type of file for upload";
                alert(abv.value);
                if (document.getElementById("<%=fpMeasurementsUpload.ClientID%>").value == "") {
                    document.getElementById("<%=fpMeasurementsUpload.ClientID%>").focus();
                    return false;
                }
                else if (document.getElementById("<%=fpGuideLineUpload.ClientID%>").value == "") {
                    document.getElementById("<%=fpGuideLineUpload.ClientID%>").focus();
                    return false;
                }
                else {

                }
            }
        }
    </script>

    <mb:MenuUserControl ID="manuControl" runat="server" />
    <div class="form_pad">
        <div>
            <table class="dropdown_pad form_table">
                <tr>
                    <td colspan="2">
                        <div style="text-align: center; color: Red; font-size: larger;">
                            <asp:Label ID="lblGuidemessage" runat="server"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co" style="width: 400px">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Master Item Number</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlMasterItem" runat="server" Enabled="false" AutoPostBack="true"
                                        onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co" style="width: 400px">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Tailoring Guidelines</span>
                                <input id="fpGuideLineUpload" type="file" tabindex="7" runat="server" />
                                <br />
                                <br />
                                <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                                    <img src="../../../Images/lightbulb.gif" alt="note:" />&nbsp;Supported file formats
                                    are <b>.doc|.txt|.pdf|.docx</b></div>
                            </div>
                            <div class="form_bot_co" id="divmessage" runat="server">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co" style="width: 400px">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Measurement Charts</span>
                                <input type="file" id="fpMeasurementsUpload" tabindex="8" runat="server" />
                                <br />
                                <br />
                                <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                                    <img src="../../../Images/lightbulb.gif" alt="note:" />&nbsp;Supported file formats
                                    are <b>.doc|.txt|.pdf|.docx</b></div>
                            </div>
                            <div class="form_bot_co" id="divMMessge" runat="server">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="spacer10" colspan="2" class="formtd">
                    </td>
                </tr>
                <tr>
                    <td class="formtd" colspan="2">
                        <div class="botbtn centeralign">
                            <asp:LinkButton ID="lnkTailorChart" class="grey2_btn" runat="server" TabIndex="9"
                                ToolTip="Save tailoring &amp; Measurements" OnClick="lnkTailorChart_Click" OnClientClick="return validate()"><span>Save Chart & Guide</span></asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <h4>
                List of Tailoring GuideLines</h4>
            <div>
                <div style="text-align: center; color: Red; font-size: larger;">
                    <asp:Label ID="lblMessageGuide" runat="server">
                    </asp:Label>
                </div>
                <asp:GridView ID="gvTailMeasurementChart" runat="server" AutoGenerateColumns="false"
                    HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                    RowStyle-CssClass="ord_content" OnRowCommand="gvTailMeasurementChart_RowCommand">
                    <Columns>
                        <asp:TemplateField Visible="False" HeaderText="Id">
                            <HeaderTemplate>
                                <span>ProductItemTailoringMeasurementID</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblProductItemTailoringMeasurementID" Text='<%# Eval("ProductItemTailoringMeasurementID") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="1%" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False" HeaderText="Id">
                            <HeaderTemplate>
                                <span>MasterItemID</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblMasterItemID" Text='<%# Eval("MasterStyleID") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="2%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span >Tailoring Guide</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span>
                                    <asp:LinkButton runat="server" ID="lnkTailoringGuide" CommandName="view" CommandArgument='<%# Eval("TailoringGuidelines") %>'
                                        Text='<% # (Convert.ToString(Eval("TailoringGuidelines")).Length > 5) ? Eval("TailoringGuidelines").ToString().Substring(0,5)+"..."  : Convert.ToString(Eval("TailoringGuidelines"))+ "&nbsp;"  %>'
                                        ToolTip='<% #Eval("TailoringGuidelines")  %>'></asp:LinkButton></span>
                                <asp:HiddenField ID="hdnTailoringGuide" Value='<%# Eval("TailoringGuidelines") %>'
                                    runat="server" />
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" Width="25%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span >Measurement Chart</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnMeasuremntChart" Value='<%# Eval("TailoringMeasurementChart") %>'
                                    runat="server" />
                                <span>
                                    <asp:LinkButton runat="server" ID="lnkTailoringMeasurementChart" CommandName="view1"
                                        CommandArgument='<%# Eval("TailoringMeasurementChart") %>' Text='<% # (Convert.ToString(Eval("TailoringMeasurementChart")).Length > 5) ? Eval("TailoringMeasurementChart").ToString().Substring(0,5)+"..." : Convert.ToString(Eval("TailoringMeasurementChart"))+ "&nbsp;"  %>'
                                        ToolTip='<% #Eval("TailoringMeasurementChart")  %>'></asp:LinkButton></span>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" Width="35%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Id">
                            <HeaderTemplate>
                                <span>MasterItem Name</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblMasterItemName" Text='<%# Eval("MasterStyleName") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="30%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Delete</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="btn_space">
                                    <asp:ImageButton ID="lnkbtndelete" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                        CommandArgument='<%# Eval("ProductItemTailoringMeasurementID") %>' runat="server"
                                        ImageUrl="~/Images/close.png" /></span>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box centeralign" Width="5%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
