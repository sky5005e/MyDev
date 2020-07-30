<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ArtworkPreview.aspx.cs" Inherits="admin_Artwork_ArtworkPreview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">

        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete this art ?") == true)
                return true;
            else
                return false;
        }
        function DisplayImage(id, src) {
            if (id != '0')
                window.location = "ViewLargerImage.aspx?id=" + id + "&src=" + src;
        }
       
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <asp:Label ID="lblMsg" runat="server" Visible="false" Text="No Records Found" CssClass="errormessage"></asp:Label>
        <!-- Artwork List -->
        <asp:DataList ID="dtArtList" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
            RepeatColumns="5" CellPadding="0" CellSpacing="0" OnItemDataBound="dtArtList_ItemDataBound"
            OnItemCommand="dtArtList_ItemCommand">
            <ItemTemplate>
                <div class="clearfix">
                    <div class="agent_img alignleft">
                        <span class="tl_co"></span><span class="tr_co"></span>
                        <div id="dvPriPhotoContainer " class="upload_photo alignright gallery">
                            <img id="img" ondblclick="DisplayImage(this.title,this.alt)" runat="server" src="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                style="border-width: 5px; height: 198px; width: 140px" title='<%#Eval("ArtWorkId")%>' />
                        </div>
                    </div>
                </div>
                <div class="uniform_price clearfix" style="width: 125px;">
                    <span class="bl_co"></span><span class="br_co"></span>
                    <table>
                        <tr>
                            <td>
                                <div style="text-align: left; color: #72757C">
                                    <asp:Label ID="lblDisplay" runat="server" />
                                    <span> Type: <img id="imgFiletype" src="~/Images/download_btn.png" style="height: 20px; width: 20px; margin:3px 0px -4px 2px"
                                        runat="server" alt='Loading' /></span>
                                </div>
                </td>
                <asp:HiddenField ID="hdnArtworkFor" runat="server" Value='<%#Eval("ArtworkFor")%>' />
                <asp:HiddenField ID="hdnArtworkName" runat="server" Value='<%#Eval("ArtworkName")%>' />
                </tr> </table> </div>
                <div class="uniform_price clearfix" style="width: 125px;">
                    <span class="bl_co"></span><span class="br_co"></span>
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkDownload" runat="server" Text="" CommandName="download" class="btn_space">
                                    <span class="btn_space">
                                        <img id="img2" src="~/Images/download_btn.png" style="height: 20px; width: 20px"
                                            runat="server" alt='Loading' /></span></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkemail" runat="server" Text="" CommandName="email" class="btn_space"
                                    CommandArgument='<%# Eval("ArtWorkId") %>'>
                                    <span class="btn_space">
                                        <img id="img1" src="~/Images/shipment06.png" style="height: 20px; width: 20px" runat="server"
                                            alt='Loading' /></span></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lblDelete" OnClientClick="return DeleteConfirmation();" runat="server"
                                    Text="" CommandName="del" CommandArgument='<%# Eval("ArtWorkId") %>' class="btn_space">
                                    <span class="btn_space">
                                        <img id="imgdelete" src="~/Images/close.png" style="height: 20px; width: 20px" runat="server"
                                            alt='Loading' /></span></asp:LinkButton>
                            </td>
                            <asp:HiddenField ID="hdnFile" runat="server" Value='<%#Eval("ArtFileName")%>' />
                            <asp:HiddenField ID="hdnArtWorkId" runat="server" Value='<%#Eval("ArtWorkId")%>' />
                        </tr>
                    </table>
                </div>
                <div class="spacer10">
                </div>
            </ItemTemplate>
            <ItemStyle Width="165" />
        </asp:DataList>
        <!-- Artwork List End -->
    </div>
</asp:Content>
