<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OrderShipmentDetails.aspx.cs" Inherits="OrderManagement_OrderShipmentDetails" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Src="~/usercontrol/UPSPackageTracking.ascx" TagName="UPSPackageTracking"
    TagPrefix="ups" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:Panel runat="server" ID="pnlUPS">
    </asp:Panel>
    <style type="text/css">
        .width300
        {
            font-size: small;
            width: 300px !important;
        }
        .padding0
        {
            padding-bottom: 0px !important;
        }
    </style>
    <div class="black_round_box">
        <div class="black2_round_top">
            <span></span>
        </div>
        <div class="black2_round_middle">
            <div class="form_pad">
                <div class="pro_search_pad" style="width: 900px;">
                    <mb:MenuUserControl ID="menucontrol" runat="server" />
                    <div>
                        <div class="black_middle order_detail_pad">
                            <table class="order_detail" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 50%">
                                        <table>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label>
                                                        Order Number :
                                                    </label>
                                                    <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label>
                                                        Reference Name :
                                                    </label>
                                                    <asp:Label ID="lblOrderBy" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 50%">
                                        <table>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
                                                        Ordered Date :
                                                    </label>
                                                    <asp:Label ID="lblOrderedDate" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
                                                        Order Status :
                                                    </label>
                                                    <asp:Label runat="server" ID="lblOrderStatus"></asp:Label>
                                                </td>
                                            </tr>
                                            <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                              {%>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
                                                        Payment Method :
                                                    </label>
                                                    <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <%} %>
                                            <tr id="trCreditType" runat="server">
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
                                                        Credit Type :
                                                    </label>
                                                    <asp:Label ID="lblCreditType" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="black_bot_co">
                            <span>&nbsp;</span></div>
                        <div class="spacer15">
                        </div>
                        <div class="black_top_co">
                            <span>&nbsp;</span></div>
                        <div class="black_middle order_detail_pad">
                            <div class="clearfix billing_head">
                                <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                  {%>
                                <div class="alignleft">
                                    <span>Bill To: </span>
                                </div>
                                <%}
                                  else
                                  {%>
                                <div class="alignleft">
                                    <span>&nbsp;</span>
                                </div>
                                <%} %>
                                <div class="alignright">
                                    <span style="padding-left: 29px!important;">Ship To:</span>
                                </div>
                            </div>
                            <div>
                                <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                  {%>
                                <div class="alignleft" style="width: 49%;">
                                    <div class="tab_content_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="tab_content">
                                        <table class="order_detail" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBName" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBCompany" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBAddress1" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBAddress2" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBCity" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBCountry" runat="server" /></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="tab_content_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                                <%}
                                  else
                                  {%>
                                <div class="alignleft" style="width: 49%;">
                                    &nbsp;
                                </div>
                                <%} %>
                                <div class="alignright" style="width: 49%;">
                                    <div class="tab_content_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="tab_content">
                                        <table class="order_detail" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSName" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSCompany" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSAddress" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSAddress2" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSStreet" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSCity" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSCountry" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="tab_content_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                                <div class="alignnone">
                                </div>
                            </div>
                        </div>
                        <div class="black_bot_co">
                            <span>&nbsp;</span></div>
                        <div class="spacer15">
                        </div>
                        <div>
                            <div style="text-align: center">
                                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                            </div>
                            <asp:GridView ID="gvShippedOrderDetail" runat="server" AutoGenerateColumns="false"
                                HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                RowStyle-CssClass="ord_content" OnRowDataBound="gvShippedOrderDetail_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Shipment</span>
                                            <div class="corner">
                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" CssClass="first" ID="lblShipment" Text='<%#Convert.ToInt32(Container.DataItemIndex + 1)%>'></asp:Label>
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Shipping Date</span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblShipingDate" Text='<%# Eval("ShipingDate","{0:d}") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtShipDate" runat="server" Text='<%# Eval("ShipingDate","{0:d}") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Boxes</span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblNoOfBoxes" runat="server" Text='<%#Eval("NoOfBoxes")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Carrier</span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblSelectedService" runat="server" Style="border-right: 0px;"></asp:Label>
                                            <asp:HiddenField runat="server" ID="hdnShipperServiceID" Value='<%# Eval("ShipperServiceID") %>' />
                                            <asp:HiddenField runat="server" ID="hdnShipperService" Value='<%# Eval("ShipperService") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>                                            
                                            <asp:DropDownList ID="ddlShipperService" runat="server">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Tracking Number</span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdnTrackingNo" Value='<%# Eval("TrackingNo") %>' />
                                            <div style="border-color: #1C1C1C #1F1F1F; border-style: solid; border-width: 1px;
                                                display: block; padding: 0 8px;">
                                                <asp:GridView ID="gvTrackingNo" runat="server" AutoGenerateColumns="false" GridLines="None"
                                                    Width="100%" ShowHeader="false" OnRowCommand="gvTrackingNo_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lbltrackingNo" Text='<%#Eval("trackingnuber")%>'
                                                                    CommandName="TrackingNumber" CommandArgument='<%#Eval("trackingnuber")%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="centeralign padding0" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtTrackingNumber" runat="server" Text='<%# Eval("TrackingNo") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="30%" />
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Packing Slip</span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:HyperLink ID="hypShippment" CommandArgument='<%# Eval("PackageId") %>' CommandName="Go"
                                                    ToolTip="View Shipment Details" runat="server">
                                                    <asp:Image ID="img" runat="server" ImageUrl="~/Images/shipment06.png" Height="24px"
                                                        Width="24px" />
                                                </asp:HyperLink>
                                            </span>
                                            <asp:HiddenField ID="hdnShippment" runat="server" Value='<%# Eval("PackageId") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
