<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AccessReports.aspx.cs" Inherits="AssetManagement_AccessReports" Title="Access Reports" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <style type="text/css">
        .form_box .custom-sel span.error
        {
            padding: 24px 0;
        }
    </style>
 <script language="javascript" type="text/javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>
 <script type="text/javascript" language="javascript">
    $().ready(function() {
        $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
            objValMsg = $.xml2json(xml);
            $("#aspnetForm").validate({
                rules: {
                    ctl00$ContentPlaceHolder1$ddlReportType: { NotequalTo: "0" }
                },
                messages: {
                    ctl00$ContentPlaceHolder1$ddlReportType: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "report type") }
                },
                errorPlacement: function(error, element) {
                    error.insertAfter(element);
                }
            });

        });
        $("#ctl00_ContentPlaceHolder1_lnkbtnSave").click(function() {
            return $('#aspnetForm').valid();
        });

    });
    </script>
    <script type="text/javascript">
        function SelectAll(id) {
            var TargetBaseControl = document.getElementById('<%= this.gvBaseStation.ClientID %>');
            var TargetChildControl = "ChkBaseStation";
           
            //Checked/Unchecked all the checkBoxes in side the GridView.
            var inputElements = TargetBaseControl.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                // Filter through the input types looking for checkboxes
                if (myElement.type == "checkbox" && myElement.id.indexOf(TargetChildControl, 0) >= 0) {
                    $(myElement).parent().removeClass('wheather_checked');
                    $(myElement).parent().removeClass('wheather_check');
                    myElement.checked = id.checked;
                    if (id.checked) {
                        $(myElement).parent().addClass('wheather_checked');
                    }
                    else {
                        $(myElement).parent().addClass('wheather_check');
                    }
                }
            }
        }
    </script>	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menucontrol" runat="server" />
    <div class="form_pad" id="dvCompanyAdmin" runat="server">
    <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>  
        <div class="form_table" style="width: 600px; margin: 0 auto;"> 
            <div style="width: 70%;margin:0 auto;">               
                <div>
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box">
                        <span class="input_label" style="width: 30%;">Select Report Type :</span>
                        <label class="dropimg_width212">
                            <span class="custom-sel label-sel">
                                <asp:DropDownList ID="ddlReportType" onchange="pageLoad(this,value);" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </span>
                        </label>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
            </div>
            <div class="spacer25">
            </div>           
            <div id="dvEquipmentType" runat="server" visible="false">
                <h4>
                    Select Equipment Type :
                </h4>
                <asp:GridView ID="gvEquipmentType" runat="server" ShowHeader="false" AutoGenerateColumns="false"
                    GridLines="None" CssClass="weather_box">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div>
                                    <div>
                                        <div class="bl_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="bl_middle_bo">
                                            <div style="height: 36px">
                                                <asp:HiddenField ID="hdnEquipmentTypeID" runat="server" Value='<%#Eval("iLookupID") %>' />
                                                <span>
                                                    <%#Eval("sLookupName") %>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="bl_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="weather_left_box" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div class="wheather_check " runat="server" id="dvChkEquipmentType">
                                    <asp:CheckBox ID="chkEquipmentType" runat="server" />
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="rightalign" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
           
            <div class="alignnone spacer25">
            </div>
            <div id="dvBaseStation" runat="server" visible="false">
                <h4 style="float:left;padding-top:20px;">
                    Select Base Station :
                </h4>
                <div class="weather_box">
                    <span class="wheather_check " style="float: right; margin-right: 7px; margin-bottom: 10px;" id="dvChkBaseStationAll">
                        <asp:CheckBox ID="ChkBaseStationAll" runat="server" OnClick="javascript:SelectAll(this);" />
                    </span>
                </div>
                <asp:GridView ID="gvBaseStation" runat="server" ShowHeader="false" AutoGenerateColumns="false"
                    GridLines="None" CssClass="weather_box" style="clear:both;">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div>
                                    <div>
                                        <div class="bl_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="bl_middle_bo">
                                            <div style="height: 36px">
                                                <asp:HiddenField ID="hdnBaseStationID" runat="server" Value='<%#Eval("iBaseStationId") %>' />
                                                <span>
                                                    <%#Eval("sBaseStation")%>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="bl_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="weather_left_box" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div class="wheather_check " runat="server" id="dvChkBaseStation">
                                    <asp:CheckBox ID="ChkBaseStation" runat="server" />
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="rightalign" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div style="text-align:center;">
                <asp:LinkButton ID="lnkbtnSave" runat="server" CssClass="grey2_btn"  OnClick="lnkbtnSave_Click">
								        <span>Save Information</span>
                </asp:LinkButton>
            </div>
        </div>
    </div>
   
</asp:Content>

