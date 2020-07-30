<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewAnniversaryCreditPrograms.aspx.cs" Inherits="MyAccount_CompanyProgram_ViewAnniversaryCreditPrograms"
    Title="Company Anniversary Program" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="srcmgr" runat="server">
    </asp:ScriptManager>
    <div class="form_pad" style="padding-top: 20px !important;">
        <div class="order_label" style="margin-top: 0px;">
            <asp:Label ID="lblWorkgroup" runat="server">
            </asp:Label></div>
        <asp:UpdatePanel ID="upPanel" runat="server">
            <ContentTemplate>
                <div style="text-align: right; color: #5C5B60; font-size: larger; font-weight: bold;"
                    id="divStats" runat="server">
                    Total Record Count(s):
                    <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                </div>
                <div class="spacer10">
                </div>
                <div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                </div>
                <asp:GridView ID="gvAnniversaryPro" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnPageIndexChanging="gvAnniversaryPro_PageIndexChanging"
                    OnRowCommand="gvAnniversaryPro_RowCommand" OnRowDataBound="gvAnniversaryPro_RowDataBound"
                    ShowFooter="true">
                    <Columns>
                        <asp:TemplateField Visible="false" HeaderText="Id">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompanyEmployeeIDView" Text='<%# Eval("CompanyEmployeeID") %>' />
                                <asp:Label runat="server" ID="lblCompanyEmployeeUserInfoID" Text='<%# Eval("UserInfoID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="EmployeeID">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblEmployeeID" runat="server" CommandArgument="EmployeeID" CommandName="Sort"><span>Employee #</span></asp:LinkButton>
                                <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" CssClass="first" ID="lblCompanyEmployeeID" Text='<%# Eval("EmployeeID") %>' />
                                <div class="corner">
                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="HirerdDate">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnHireDate" runat="server" CommandArgument="HirerdDate" CommandName="Sort"><span>Hired Date</span></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnStartingCredit" runat="server" />
                                <asp:HiddenField ID="hdnAnniversary" runat="server" />
                                <asp:Label runat="server" ID="lblHiredDate" Text='<%# Convert.ToDateTime(Eval("HirerdDate")).ToShortDateString() %> ' />
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="FullName">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnCusName" runat="server" CommandArgument="FullName" CommandName="Sort"><span>Name</span></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lnkbtnfullname" runat="server" Text='<%# Eval("FullName") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="StartingCreditAmount">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnStartingCredit" runat="server" CommandArgument="StartingCreditAmount"
                                    CommandName="Sort">
                                <span>Starting Credit</span>
                                </asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderStaringCredit" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span>
                                    <asp:TextBox runat="server" ID="lblStaringCredit" Width="40px" Style="background-color: #303030;
                                        border: medium none; color: #ffffff; width: 70px; padding: 2px" />
                                </span>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="AnniversaryCreditAmount">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnAnniversaryCreditAmount" runat="server" CommandArgument="AnniversaryCreditAmount"
                                    CommandName="Sort">
                                <span>Anniversary Credit</span>
                                </asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderAnniversaryCreditAmount" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span>
                                    <asp:TextBox runat="server" ID="lblAnniversaryCreditAmount" Width="50px" Style="background-color: #303030;
                                        border: medium none; color: #ffffff; width: 70px; padding: 2px" />&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkSaveAnnive" CommandName="saveAnniversary" CommandArgument='<%# Eval("CompanyEmployeeID") %>'
                                        runat="server" OnClientClick="return confirm('Are you sure you want to save record?');"
                                        ToolTip="Update Anniversary credits of this user!"><img style="height:25px;width:25px;" src="../../images/save-information-icon.png" alt=""  /></asp:LinkButton>
                                </span>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" />
                        </asp:TemplateField>
                        <%--Need to uncomment this and also Join with the base station table in the SP
                        commented because there are no base station and in the sp there is join with base station table--%>
                        <%--<asp:TemplateField SortExpression="sBaseStation">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnBaseStation" runat="server" CommandArgument="sBaseStation"
                                    CommandName="Sort"><span class="white_co">Base Station</span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderCountry" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblBaseStation" Text='<%# Eval("sBaseStation") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField SortExpression="CreditBalance">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnCreditBalance" runat="server" CommandArgument="CreditBalance"
                                    CommandName="Sort">
                                <span>Credit Balance</span>
                                </asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderCreditBalance" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCreditBalance" />
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Status">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnStatus" runat="server" CommandArgument="Status" CommandName="Sort">
                                    <span>Status</span>
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkStatus" runat="server" Text="" class="btn_space" CommandName="ChangeStatus">
                                    <span class="btn_space">
                                        <img id="imgLookupIcon" style="height: 20px; width: 20px" runat="server" alt='Loading' /></span></asp:LinkButton>
                                <asp:HiddenField runat="server" ID="lblIconPath" Value='<%# Eval("IconPath") %>' />
                                <asp:HiddenField ID="hdnStatusID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "WLSStatusId")%>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box centeralign" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Status">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnEmployeeProfile" runat="server" CommandArgument="Status"
                                    CommandName="Sort">
                        <span class="white_co">Employee Profile</span>
                         <div class="corner"><span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span></div>
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="btn_box centeralign">
                                    <asp:LinkButton ID="lnkBtn" runat="server" CssClass="greysm_btn" CommandArgument='<%# Eval("CompanyEmployeeID") %>'
                                        CommandName="ViewProfile">
                                    <span style="color:White;">
                                    View Profile
                                    </span>
                                    </asp:LinkButton>
                                    <div class="corner">
                                        <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                    </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle CssClass="first" ForeColor="#72757C" Font-Bold="true" />
                </asp:GridView>
                <div class="alignright pagging">
                    <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                    </asp:LinkButton>
                    <span>
                        <asp:DataList ID="lstPaging" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="lstPaging_ItemCommand"
                            OnItemDataBound="lstPaging_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList></span>
                    <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="companylist_botbtn">
            <asp:LinkButton ID="lnkSave" runat="server" class="grey_btn" OnClick="lnkSave_Click"><span>Save Credit</span></asp:LinkButton>
        </div>
        <asp:HiddenField ID="hdnTotalValue" runat="server" />
    </div>
</asp:Content>
