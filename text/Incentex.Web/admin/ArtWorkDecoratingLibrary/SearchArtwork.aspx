<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="SearchArtwork.aspx.cs" Inherits="admin_ArtWorkDecoratingLibrary_SearchArtwork" Title="Artwork" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <style type="text/css">
        .custom-sel select 
        {
            position: relative;
            margin-bottom: -8px;
        }
        .radiobutton input
        {
        	margin-right:5px;
        }
        .radiobutton label
        {
        	margin-right:20px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <div class="form_pad select_box_pad" style="width: 390px;">
        <div class="form_table">
            <table>
                
                <tr id="trCompany" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Company Name</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlCompany" onchange="pageLoad(this,value);" runat="server"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvCompany">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                
                <tr id="trFileName" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">File Name</span>
                                <asp:TextBox ID="txtFileName" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSearchInfo" class="grey2_btn" runat="server" ToolTip="Search files"
                            OnClick="lnkBtnSearchInfo_Click"><span>Search</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
