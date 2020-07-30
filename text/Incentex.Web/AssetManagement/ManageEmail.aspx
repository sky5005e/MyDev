<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="ManageEmail.aspx.cs" Inherits="AssetManagement_ManageEmail" Title="Manage Email & Dropdown" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <style type="text/css">
      .ui-widget input, .ui-widget select, .ui-widget textarea, .ui-widget button
        {
        	font-size:11px;
        }
 </style>
 <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
         $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleContainerContent").hide();
        });
    
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
          <div class="collapsibleContainer" title="Email Management">
                        <div style="text-align: center">
        <div>
            <table>
                    <tr>
                        <td class="formtd">
                           <%-- <table class="checktable_supplier true">
                                <tr>
                                    <td>
                                        <asp:DataList ID="dtManageEmail" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="ManageEmailspan" runat="server">
                                                    <asp:CheckBox ID="chkManageEmail" runat="server" />
                                                </span>
                                                <label>
                                                    <asp:Label ID="lblManageEmail" Text='<%# Eval("EquipmentEmailName") %>' runat="server"></asp:Label></label>
                                                <asp:HiddenField ID="hdnManageEmail" runat="server" Value='<%#Eval("EquipmentEmailID")%>' />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>--%>
                            
                            <div id="dvSubReport" runat="server" >               
                <asp:GridView ID="dtManageEmail" runat="server" ShowHeader="false" AutoGenerateColumns="false"
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
                                                <asp:HiddenField ID="hdnManageEmail" runat="server" Value='<%#Eval("EquipmentEmailID") %>' />
                                                <span>
                                                    <%#Eval("EquipmentEmailName")%>
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
                                <div class="wheather_check " runat="server" id="ManageEmailspan">
                                    <asp:CheckBox ID="chkManageEmail" runat="server" />
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="rightalign" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
                        </td>
                    </tr>
                    <tr><td>
                    <div class="alignnone spacer25"></div>
                    </td></tr>
                    <tr><td>
                     <div class="botbtn centeralign">
            <asp:LinkButton ID="lnkSaveEmail" runat="server" CssClass="grey2_btn" TabIndex="19" OnClick="lnkSaveEmail_Click">
								        <span>Save</span>
            </asp:LinkButton>
        </div>
                    </td></tr>
                </table>
        </div>
       </div>
       </div>
       <div class="spacer25"> </div>
         <div class="collapsibleContainer" title="Dropdown Management">
                        <div style="text-align: center">
        <div>
            <table>
                    <tr>
                        <td class="formtd">
                            <%--<table class="checktable_supplier true">
                                <tr>
                                    <td>
                                        <asp:DataList ID="dtManageDropdown" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="ManageDropdownspan" runat="server">
                                                    <asp:CheckBox ID="chkManageDropdown" runat="server" />
                                                </span>
                                                <label>
                                                    <asp:Label ID="lblManageDropdown" Text='<%# Eval("EquipmentDropdownName") %>' runat="server"></asp:Label></label>
                                                <asp:HiddenField ID="hdnManageDropdown" runat="server" Value='<%#Eval("EquipmentDropdownID")%>' />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>--%>   
                            <div id="Div1" runat="server" >               
                <asp:GridView ID="dtManageDropdown" runat="server" ShowHeader="false" AutoGenerateColumns="false"
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
                                                <asp:HiddenField ID="hdnManageDropdown" runat="server" Value='<%#Eval("EquipmentDropdownID") %>' />
                                                <span>
                                                    <%#Eval("EquipmentDropdownName")%>
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
                                <div class="wheather_check " runat="server" id="ManageDropdownspan">
                                    <asp:CheckBox ID="chkManageDropdown" runat="server" />
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="rightalign" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>                         
                        </td>
                    </tr>
                    <tr><td>
                    <div class="alignnone spacer25"></div>
                    </td></tr>
                    <tr><td>
                     <div class="botbtn centeralign">
            <asp:LinkButton ID="lnkSaveDropDown" runat="server" CssClass="grey2_btn" TabIndex="19" OnClick="lnkSaveDropDown_Click">
								        <span>Save</span>
            </asp:LinkButton>
        </div>
                    </td></tr>
                </table>
        </div>
        </div>
        </div>
        <div class="spacer25"> </div>
        <div class="botbtn centeralign">
            <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" TabIndex="19" OnClick="lnkSave_Click">
								        <span>Next</span>
            </asp:LinkButton>
        </div>
    </div>
    </div>
</asp:Content>

