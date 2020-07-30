<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewCamp.aspx.cs" Inherits="admin_CommunicationCenter_ViewCamp" Title="Communications Center" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript" language="javascript">

        function DeleteConfirmation() {
         var ans =confirm("Are you sure, you want to delete selected Campaign?");
            if (ans)
            {
              
                return true;
             }
            else
            {
                
                return false;
                
            }
              
        }
    </script>

    <div style="text-align: center">
        <asp:Label ID="lblMsgGrid" runat="server" CssClass="errormessage"></asp:Label>
    </div>
    <%--<table>
        <tr>
            <td>--%>
    <div class="form_pad">
        <div>
            <div style="text-align: center; color: Red; font-size: larger;">
                <asp:GridView ID="gvViewCamp" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                    CssClass="orderreturn_box" GridLines="None" OnRowCommand="gvViewCamp_RowCommand"
                    OnRowDataBound="gvViewCamp_RowDataBound" RowStyle-CssClass="ord_content">
                    <Columns>
                        <asp:TemplateField Visible="False" HeaderText="Id">
                            <HeaderTemplate>
                                <span>CampaignID</span>
                                 
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCampaignID" Text='<%# Eval("CampaignID") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="2%" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False" HeaderText="IsComplete">
                            <HeaderTemplate>
                                <span>IsComplete</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblIsComplete" Text='<%# Eval("IsComplete") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="2%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="CDate">
                            <HeaderTemplate>
                                <span class="centeralign">Send Date</span>
                                <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCampDate" CssClass="first" runat="server" Text='<%#Eval("CDate") %>'></asp:Label>
                                 <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                            </ItemTemplate>
                            <ItemStyle Width="10%" CssClass="b_box centeralign" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Name">
                            <HeaderTemplate>
                                <span class="centeralign">
                                    <asp:LinkButton ID="lnkbtnName" runat="server" CommandName="Sort">Campaign Name</asp:LinkButton>
                                </span>
                                <asp:PlaceHolder ID="placeholderName" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="LblCampaignName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box centeralign" Width="20%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="TotalMail">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnTotalMail" runat="server" CommandName="Sort"><span class="centeralign">Number of Emails Sent</span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderTotalMail" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTotalMail" runat="server" Text='<%# Eval("TotalMail")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box centeralign" Width="20%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span class="centeralign">Detail</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="centeralign">
                                    <asp:HyperLink ID="lnkedit" CommandArgument='<%# Eval("CampaignID") %>' CommandName="detailCamp"
                                        runat="server" NavigateUrl='<%# "~/admin/CommunicationCenter/CampDetail.aspx?Id=" + Eval("CampaignID").ToString() + "&compID=" + Eval("CompanyID").ToString()%>'>Detail</asp:HyperLink> 
                                        
                                       <asp:HyperLink ID="lnkBtnEdit" CommandArgument='<%# Eval("CampaignID") %>' CommandName="editCamp"
                                        runat="server" NavigateUrl='<%# "~/admin/CommunicationCenter/CreateCampaign.aspx?campID=" + Eval("CampaignID").ToString()%>'>Edit</asp:HyperLink> 
                                </span>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box centeralign" Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span class="centeralign">Delete</span>
                                <div class="corner">
                                <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                            </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="btn_space">
                                    <asp:ImageButton ID="lnkbtndelete" runat="server" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                        CommandArgument='<%# Eval("CampaignID") %>' ImageUrl="~/Images/close.png" /></span>
                                         <div class="corner">
                                <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                            </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box centeralign" Width="8%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Label ID="lblmsg" runat="server">
                </asp:Label>
            </div>
        </div>
    </div>
    <%--</td>
        </tr>
    </table>--%>
</asp:Content>
