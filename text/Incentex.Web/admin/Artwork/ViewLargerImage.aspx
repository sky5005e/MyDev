<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewLargerImage.aspx.cs" Inherits="admin_Artwork_ViewLargerImage"  %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script  language="javascript" type="text/javascript"> 
    
    function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete this art ?") == true)
                return true;
            else
                return false;
        }
    </script>
    <div id="content" class="">
        <div class="black_round_box">
            <div class="black2_round_top">
                <span></span>
            </div>
            <div class="black2_round_middle">
                <div class="clearfix">
                </div>
                <div class="front_page_pad" align="center" >
                    <div class="splash_img_pad">
                        <asp:Label ID="lblMsg" runat="server" Visible="false" Text="No Records Found" CssClass="errormessage"></asp:Label>
                        <div class="clearfix">
                            <div>
                                <span class="tl_co"></span><span class="tr_co"></span>
                                <div id="dvPriPhotoContainer " class="upload_photo  gallery">
                                    <img id="imgProduct" runat="server" src="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                        style="border-width: 5px;" />
                                </div>
                            </div>
                        </div>
                        <div class="uniform_price  clearfix" style="width: 120px !important;">
                            <span class="bl_co"></span><span class="br_co"></span>
                            <table>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkDownload" runat="server" Text="" CommandName="download" 
                                            class="btn_space" onclick="lnkDownload_Click">
                                            <span class="btn_space">
                                                <img id="img2" src="~/Images/download_btn.png" style="height: 20px; width: 20px"
                                                    runat="server" alt='Loading' /></span></asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkemail" runat="server" Text="" CommandName="email" 
                                            class="btn_space" onclick="lnkemail_Click">
                                            <span class="btn_space">
                                                <img id="img1" src="~/Images/e-mail_btn.png" style="height: 20px; width: 20px" runat="server"
                                                    alt='Loading' /></span></asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lblDelete" OnClientClick="return DeleteConfirmation();" runat="server"
                                            Text="" CommandName="del" class="btn_space" onclick="lblDelete_Click">
                                            <span class="btn_space">
                                                <img id="imgdelete" src="~/Images/close_btn.png" style="height: 20px; width: 20px"
                                                    runat="server" alt='Loading' /></span></asp:LinkButton>
                                    </td>
                                   
                                </tr>
                            </table>
                        </div>
                        <div class="spacer10">
                        </div>
                    </div>
                </div>
                <div class="black2_round_bottom">
                    <span></span>
                </div>
                <div class="alignnone">
                    &nbsp;</div>
            </div>
        </div>
    </div>
</asp:Content>
