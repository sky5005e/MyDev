<%@ Page Title="System Setup Code" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="SystemSetupCode.aspx.cs" Inherits="MyAccount_SystemSetupCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <h3 style="color: #72757C; text-align: left;">
            <asp:Label Text="" ID="lblCompanyName" runat="server"></asp:Label></h3>
        <div class="divider">
        </div>
        <div class="form_table clearfix">
            <div class="formtd alignleft">
                <h4>
                    Departments</h4>
                <table class="checktable_supplier">
                    <tr>
                        <td>
                            <asp:DataList ID="dtlDepartments" runat="server">
                                <ItemTemplate>
                                    <span>
                                        <asp:Label ID="lblDepartmentID" Text='<%# Eval("DepartmentID") %>' runat="server"></asp:Label>
                                    </span>
                                    <label>
                                        <asp:Label ID="lblDepartment" Text='<%# Eval("Department") %>' runat="server"></asp:Label></label>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="formtd alignleft">
                <h4>
                    Workgroups</h4>
                <table class="checktable_supplier">
                    <tr>
                        <td>
                            <asp:DataList ID="dtlWorkgroups" runat="server">
                                <ItemTemplate>
                                    <span>
                                        <asp:Label ID="lblWorkgroupID" Text='<%# Eval("WorkGroupID") %>' runat="server"></asp:Label>
                                    </span>
                                    <label>
                                        <asp:Label ID="lblWorkgroup" Text='<%# Eval("WorkGroup") %>' runat="server"></asp:Label></label>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="divider">
        </div>
        <div class="checktable_supplier">
            <h4>
                Example: 3rd Party Supplier Code ( Company Code-Department Code-Workgroup Code-3rd
                Party Supplier Code )</h4>
            <label>
                <span>Company Name: </span>
            </label>
            <span id="spnCompanyName2" runat="server"></span>
            <br />
            <label>
                <span>Department: </span>
            </label>
            <span>Customer Service</span><br />
            <label>
                <span>Workgroup: </span>
            </label>
            <span>Flight Attendant Group</span><br />
            <label>
                <span>Company Access Code: </span>
            </label>
            <span><span id="spnCompanyCode2" runat="server" style="margin-right: 0px;"></span>-74-51</span>
        </div>        
    </div>
</asp:Content>
