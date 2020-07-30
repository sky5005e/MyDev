<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="SearchArtworkLibrary.aspx.cs" Inherits="admin_Artwork_SearchArtworkLibrary" %>

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
    <div class="form_pad">
        <div class="form_table">
            <table class="dropdown_pad ">
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 38%">Select Company</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlCompany" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </label>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 38%">Artwork Name</span>
                            <asp:TextBox ID="txtArtworkName" runat="server"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 38%">Artwork For</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlArtworkFor" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </label>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 38%">Artwork Design Number</span>
                            <asp:TextBox ID="txtArtworkDesignNumber" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                    <td class="spacer10">
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSearch" class="grey2_btn" runat="server" ToolTip="Search"
                            OnClick="lnkBtnSearch_Click"><span>Search</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
