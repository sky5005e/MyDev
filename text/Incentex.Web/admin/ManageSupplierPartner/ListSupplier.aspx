<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ListSupplier.aspx.cs" Inherits="admin_ManageSupplierPartner_ListSupplier"
    Title="Supplier List" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div class="btn_width worldlink_btn">
            <div>
                <asp:DataList ID="dtSupIndex" runat="server" RepeatDirection="Vertical" RepeatColumns="3">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnMenuAccess" runat="server" Value='<%# Eval("SupplierPartnerID") %>' />
                        <a href="javascript:createWindow('<%#Eval("URL")%>','name','features')" class="gredient_btn1"
                            title='<%# GetTooltip((Eval("LoginName")),(Eval("Password")))%>'><span>
                                <img src="../Incentex_Used_Icons/Incentex-managesupplierpartner.png" alt="Manage Supplier Partner" />
                                <%# Eval("Name")%></span> </a>
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </div>
    </div>

    <script type="text/javascript">                 
        function createWindow(cUrl,cName,cFeatures) {               
                    window.open("http://"+cUrl);
        }
    </script>

</asp:Content>
