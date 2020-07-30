<%@ Page Title="Employee Training Center" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="EmployeeTrainingCenter.aspx.cs" Inherits="admin_EmployeeTrainingCenter_EmployeeTrainingCenter" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <div class="form_pad" style="padding-left: 180px;">
        <div class="spacer10">
        </div>
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" Style="padding-left: 35px;"></asp:Label>
        <div class="spacer10">
        </div>
        <div id="dvDocument" runat="server">
            <asp:HyperLink ID="lnkAddEmployeeTraining" runat="server" title="Add Employee Training" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/Incentex-AddEmployeeTraininig.png" /> 
                <span>
                   Add Employee Training
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkSearchEmployeeTraining" runat="server" title="Search Employee Training" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/Incentex-SearchEmployeeTraininig.png" /> 
                <span>
                   Search Employee Training
                </span>
            </asp:HyperLink>
        </div>
        <div>
           <asp:HyperLink ID="lnkAddEmployeeTrainingType" runat="server" title="Add Employee Training Type" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/Incentex-AddEmployeeTraininig.png" /> 
                <span>
                   Add Employee Training Type  
                </span>
            </asp:HyperLink>
        </div>
    </div>
</asp:Content>

