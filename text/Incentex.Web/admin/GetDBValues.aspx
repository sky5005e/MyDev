<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="GetDBValues.aspx.cs" Inherits="admin_GetDBValues" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>
    <asp:ScriptManager ID="scr" runat="server"></asp:ScriptManager>
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
        <td style="height:50px;">
        </td>
        </tr>
        <tr>
            <td style="width: 20%">
               <u> User Type :</u>
                <br />
                <br />
                <asp:GridView ID="grvUserType" runat="server" AutoGenerateColumns="false" Width="">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                UserType Id
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Id" runat="server" Text='<%#Eval("UserTypeID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                User Type Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Name" runat="server" Text='<%#Eval("UserType1")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td style="width: 20%">
               <u>  Company Name :</u>
                <br />
                <br />
                <asp:GridView ID="grvCompany" runat="server" AutoGenerateColumns="false" Width="">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Company Id
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Id" runat="server" Text='<%#Eval("CompanyID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Company Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Name" runat="server" Text='<%#Eval("CompanyName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td style="width: 20%">
                <u> WLS Status :</u>
                <br />
                <br />
                <asp:GridView ID="grvStatus" runat="server" AutoGenerateColumns="false" Width="">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                WLSStatus Id
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%#Eval("iLookupID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                WLS Status Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDName" runat="server" Text='<%#Eval("sLookupName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="5" style="height: 50px;">
            </td>
        </tr>
        <tr>
            <td style="width: 20%">
                <u> Department :</u>
                <br />
                <br />
                <asp:GridView ID="grvDept" runat="server" AutoGenerateColumns="false" Width="">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                DepartmentId
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDeptID" runat="server" Text='<%#Eval("iLookupID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                DepartmentName
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDeptName" runat="server" Text='<%#Eval("sLookupName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td style="width: 20%">
                <u> Workgroup:</u>
                <br />
                <br />
                <asp:GridView ID="grvWrokgroup" runat="server" AutoGenerateColumns="false" Width="">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Workgroup Id
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Id" runat="server" Text='<%#Eval("iLookupID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Workgroup
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Name" runat="server" Text='<%#Eval("sLookupName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td style="widows: 20%">
                <u> Region:</u>
                <br />
                <br />
                <asp:GridView ID="grvRegion" runat="server" AutoGenerateColumns="false" Width="">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                RegionId
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="ID" runat="server" Text='<%#Eval("iLookupID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Region Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Name" runat="server" Text='<%#Eval("sLookupName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="5" style="height: 50px;">
            </td>
        </tr>
        <tr>
            <td style="width: 20%">
                <u> Gender:</u>
                <br />
                <br />
                <asp:GridView ID="grvGender" runat="server" AutoGenerateColumns="false" Width="">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                GenderId
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblGenderId" runat="server" Text='<%#Eval("iLookupID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                GenderName
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblGenderName" runat="server" Text='<%#Eval("sLookupName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td style="widows: 10%">
                &nbsp;
            </td>
            <td style="widows: 20%" colspan="3">
                <u> Base station:</u>
                <br />
                <br />
                <asp:GridView ID="grvBaseStation" runat="server" AutoGenerateColumns="false" Width="">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                BaseStation Id
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Id" runat="server" Text='<%#Eval("iBaseStationId")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                BaseStation Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Name" runat="server" Text='<%#Eval("sBaseStation")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Country
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Country" runat="server" Text='<%#Eval("sCountryName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="5" style="height: 50px;">
            </td>
        </tr>
        
        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>
                <tr>
                    <td colspan="5">
                        <div class="form_table">
                            <div>
                                <table>
                                    <tr>
                                        <td class="formtd">
                                            <table>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCountry" TabIndex="5" runat="server" onchange="pageLoad(this,value);"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlState" runat="server" TabIndex="6" onchange="pageLoad(this,value);"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                            <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                        <tr>
                                            <td>
                                                <div>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlCity" runat="server" TabIndex="7" AutoPostBack="true">
                                                                <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </div>
                                            </td>
                                        </tr>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </ContentTemplate>
        </asp:UpdatePanel>
    </table>
</asp:Content>
