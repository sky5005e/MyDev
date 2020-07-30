<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HeadsetQuoteStatus.aspx.cs" Inherits="HeadsetRepairCenter_HeadsetQuoteStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%-- Start Headset Repair Quote successfully approved part--%>
    <div id="dvSuccessfullyApproved" runat="server" visible="false">
        <div class="centeralign" style="padding-top:75px;padding-bottom:50px;">
            <img src="../Images/manager_approved_officially.png" alt="Approved" />
            <div class="spacer20">
                &nbsp;</div>
            <h4>
                Headset Repair Quote has been Approved successfully.
            </h4>
        </div>
    </div>
    <%-- End order successfully approved part--%>
   
    <%-- Start Headset Repair Quote successfully cancel  part--%>
    <div id="dvcancel" runat="server" visible="false">
        <div class="centeralign" style="padding-top:75px;padding-bottom:50px;">
            <img src="../Images/manager_canceled_officially.png" alt="Cancel" />
            <div class="spacer20">
                &nbsp;</div>
            <h4>
                Headset Repair Quote has been Cancel successfully.
            </h4>
        </div>
    </div>
    <%-- End order successfully approved part--%>
</asp:Content>


