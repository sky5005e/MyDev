<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CompanyLevelFinalStep.aspx.cs" Inherits="admin_CompanyLevelFinalStep" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <h4 style="text-align: center;">
            Please click Final SAVE button below.</h4>
        <h4 style="text-align: center;">
            <blink>
            You are about to save 
            <br />
            1.) MENU ACCESS <br />
            2.)STORE FRONT SETTING <br /> 
            3.)ANNIVERSARY CREDITS <br />
            4.)UNIFORM ISSUANCE POLICY <br />values for the 
            "<asp:Label ID="noofusers" runat="server"></asp:Label>" Company Employee
            <br />
            Workgroup : <asp:Label ID="lblworkgroup" runat="server"></asp:Label>
            </blink>
        </h4>
        <div class="uniform_pad">
            <div>
            </div>
        </div>
        <div class="spacer25">
        </div>
        <div class="bot_alert next">
            <asp:LinkButton ID="lnkBtnSave" runat="server" title="Save Information" OnClick="lnkBtnSave_Click">Final SAVE</asp:LinkButton>
        </div>
        <div class="bot_alert prev">
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        </div>
    </div>
</asp:Content>
