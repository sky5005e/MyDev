<%@ Page Title="Store View Management" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="StoreView.aspx.cs" Inherits="admin_CompanyStore_StoreView" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <div class="alignleft">
            <p class="upload_photo gallery">
                <a id="prettyphotoDiv" rel='prettyPhoto[a]' href="~/Images/StoreCategoryLocations.png"
                    runat="server">
                    <img id="imgSplashImage" alt="" style="width: 250px; height: 300px;" runat="server"
                        src="~/Images/StoreCategoryLocations.png" />
                </a>
            </p>
        </div> 
        <div class="form_table select_box_pad" style="width:390px;">
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div>
                <span style="color: #72757C; font-size: 15px; color: #B0B0B0;">Store :</span>
                <asp:Label ID="lblStore" runat="server" Style="color: #72757C; font-size: 15px;" />
            </div>
            <table>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;" title="Left first location from top">Location
                                    A</span> <span class="custom-sel label-sel">
                                        <asp:DropDownList ID="ddlLocationA" onchange="pageLoad(this,value);" runat="server">
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
                                <span class="input_label" style="width: 30%;" title="Left second location from top">
                                    Location B</span> <span class="custom-sel label-sel">
                                        <asp:DropDownList ID="ddlLocationB" onchange="pageLoad(this,value);" runat="server">
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
                                <span class="input_label" style="width: 30%;" title="Right second location from top">
                                    Location C</span> <span class="custom-sel label-sel">
                                        <asp:DropDownList ID="ddlLocationC" onchange="pageLoad(this,value);" runat="server">
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
                                <span class="input_label" style="width: 30%;" title="Right third location from top">
                                    Location D</span> <span class="custom-sel label-sel">
                                        <asp:DropDownList ID="ddlLocationD" onchange="pageLoad(this,value);" runat="server">
                                        </asp:DropDownList>
                                    </span>
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
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Save Information"
                            OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
