<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CompanyPrograms.aspx.cs" Inherits="admin_CompanyStore_CompanyPrograms"
    Title="Company Programs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <div>
            <div class="centeralign btn_space">
                
                <asp:LinkButton ID="lnkBtnAnniversaryCreditProgram" runat="server" CssClass="gredient_btn"
                    OnClick="lnkBtnAnniversaryCreditProgram_Click"><span><strong>Anniversary Credit Program</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkBtnIssuancePolicy" runat="server" CssClass="gredient_btn"
                    OnClick="lnkBtnIssuancePolicy_Click"><span><strong>Uniform Issuance Policy</strong></span></asp:LinkButton>
                <%--<a href="CompanyPrograms/ViewAnniversaryPrograms.aspx" class="gredient_btn" title="Anniversary Credit Program"></a>--%>
                <%--<a href="CompanyPrograms/ViewIssuancePrograms.aspx" title="Uniform Issuance Policy" class="gredient_btn"><label><span></span></label></a>--%>
            </div>
        </div>
    </div>
</asp:Content>
