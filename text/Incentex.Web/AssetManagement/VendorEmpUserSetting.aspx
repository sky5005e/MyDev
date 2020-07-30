<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="VendorEmpUserSetting.aspx.cs" Inherits="AssetManagement_VendorEmpUserSetting" Title="User Setting" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl" TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script type="text/javascript">
        function SelectAll(id) {
            var TargetBaseControl = document.getElementById('<%= this.dvSubReport.ClientID %>');
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
    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <mb:MenuUserControl ID="menucontrol" runat="server" />
    <div class="tabcontent" id="menudata">
    <div class="alignnone">
            &nbsp;</div>
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>       
        
        <div id="dvSubReport" runat="server" >
                <h4>
                   GSE Asset Management User Setting :
                </h4>
                <asp:GridView ID="dtlMenus" runat="server" ShowHeader="false" AutoGenerateColumns="false"
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
                                                <asp:HiddenField ID="hdnMenuAccess" runat="server" Value='<%#Eval("UserSettingID") %>' />
                                                <span>
                                                    <%#Eval("UserSettingName")%>
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
                                <div class="wheather_check " runat="server" id="dvChkSubReport">
                                    <asp:CheckBox ID="chkdtlMenus" runat="server" />
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="rightalign" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        <div class="alignnone spacer25">
            </div>
        <div class="botbtn centeralign">
            <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" TabIndex="19" OnClick="lnkSave_Click">
								        <span>Save Information</span>
            </asp:LinkButton>
        </div>
    </div>
    
    <%--New Menu--%>
    
    
    </div>
</asp:Content>

