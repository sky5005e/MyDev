<%@ Page Title="Store >> Contact Us" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="ViewContact.aspx.cs" Inherits="admin_CompanyStore_ViewContact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad pro_search_pad">
        <asp:Repeater ID="rptContacts" runat="server">
            <ItemTemplate>
                <div class="black_top_co">
                    <span>&nbsp;</span>
                </div>
                <div class="black_middle order_detail_pad clearfix">
                    <div class="alignleft" style="width: 30%;">
                        <div id="dvPriPhoto">
                            <div>
                                <span class="lt_co"></span><span class="rt_co"></span><span class="lb_co"></span>
                                <span class="rb_co"></span>
                                <div id="dvActionMessagePP">
                                </div>
                                <div id="dvPriPhotoContainer" class="upload_photo gallery">
                                    <a href='<%# SetImages(Eval("Image")) %>' rel='prettyPhoto'>
                                        <img id="imgPhotoupload" src='<%# SetImages(Eval("Image")) %>' alt="Contact" width='140' height='161' />
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="alignleft" style="width: 70%;">
                        <h4>
                            Supporting Role</h4>
                        <p>
                            <asp:Label ID="lblContactUserRoleDescription" runat="server" Text='<%# Eval("UserRoleDescription") %>'></asp:Label>
                        </p>
                        <div class="spacer20">
                        </div>
                        <h4>
                            Reach Us</h4>
                        <p>
                            <asp:Label ID="lblCompanyName" runat="server" Text='<%# Eval("CompanyName") %>'></asp:Label><br />
                            <asp:Label ID="lblContactAddress1" runat="server" Text='<%# Eval("Address1") %>'></asp:Label><br />
                            <asp:Label ID="lblContactAddress2" runat="server" Text='<%# Eval("Address2") %>'></asp:Label><br />
                            <asp:Label ID="lblContactCity" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                            <asp:Label ID="lblContactState" runat="server" Text='<%# Eval("State") %>'></asp:Label><br />
                            <asp:Label ID="lblContactZip" runat="server" Text='<%# Eval("ZipCode") %>'></asp:Label>&nbsp;<asp:Label
                                ID="lblContactCountry" runat="server" Text='<%# Eval("Country") %>'></asp:Label><br />
                        </p>
                        <div class="spacer10">
                        </div>
                        <p>
                            <strong>Phone:</strong>&nbsp;<asp:Label ID="lblContactPhone" runat="server" Text='<%# Eval("Telephone") %>'></asp:Label></p>
                        <div class="spacer10">
                        </div>
                        <p>
                            <strong>Mobile:</strong>&nbsp;<asp:Label ID="lblContactMobile" runat="server" Text='<%# Eval("Mobile") %>'></asp:Label></p>
                        <div class="spacer10">
                        </div>
                        <p>
                            <strong>Fax:</strong>&nbsp;<asp:Label ID="lblContactFax" runat="server" Text='<%# Eval("Fax") %>'></asp:Label></p>
                        <div class="spacer10">
                        </div>
                        <p>
                            <strong>Email:</strong>&nbsp;<asp:Label ID="lblContactEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label></p>
                    </div>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
