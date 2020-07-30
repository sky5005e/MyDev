<%@ Page Title="Store >> Video Podcast" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VideoPodcast.aspx.cs" Inherits="CompanyStore_VideoPodcast" %>
 <%@ Register Src="~/usercontrol/TrainingVideo.ascx" TagName="VideoUserControl" TagPrefix="vti"%>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div class="btn_width worldlink_btn">
            <div>
                <asp:DataList ID="dtVideoList" runat="server" RepeatDirection="Vertical" RepeatColumns="3"
                    OnItemCommand="dtVideoList_ItemCommand">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdnVideoTitle" Value='<%# Eval("VideoTitle") %>'/>
                        <asp:LinkButton runat="server" CssClass="gredient_btn" ID="lnkbtnVideoTitle" CommandArgument='<%# Eval("VideoName") %>' CommandName="Play"><span><%# Eval("VideoTitle") %></span></asp:LinkButton>
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </div>
    </div>
</asp:Content>

