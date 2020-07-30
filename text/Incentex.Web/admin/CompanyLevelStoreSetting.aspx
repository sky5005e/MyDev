<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CompanyLevelStoreSetting.aspx.cs" Inherits="admin_CompanyLevelStoreSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();

            }
        }
    </script>

    <div class="form_pad">
        <h4 style="text-align: center;">
            In this step please select the store front setting for the department and work group
            you are planning for.</h4>
        <div class="divider">
        </div>
        <h4>
            User Store Options</h4>
        <div class="form_table" id="userstoreoption">
            <table>
                <tr>
                    <td class="formtd">
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <asp:DataList ID="dtUserStoreFront" runat="server">
                                        <ItemTemplate>
                                            <span class="custom-checkbox alignleft" id="storespan" runat="server">
                                                <asp:CheckBox ID="chkUserStoreFront" runat="server" />
                                            </span>
                                            <label>
                                                <asp:Label ID="lblUserStoreFront" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                            <asp:HiddenField ID="hdnStoreFront" runat="server" Value='<%#Eval("iLookupID")%>' />
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider">
        </div>
        <h4>
            World-Link System Payment Options</h4>
        <div class="form_table clearfix">
            <div class="formtd alignleft">
                <table class="checktable_supplier true">
                    <tr>
                        <td>
                            <asp:DataList ID="dtPaymentOptions" runat="server">
                                <ItemTemplate>
                                    <span class="custom-checkbox alignleft" id="paymentspan" runat="server">
                                        <asp:CheckBox ID="chkPaymentOptions" runat="server" />
                                    </span>
                                    <label>
                                        <asp:Label ID="lblPaymentOptions" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                    <asp:HiddenField ID="hdnPaymentOption" runat="server" Value='<%#Eval("iLookupID")%>' />
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                </table>
                <div class="spacer15">
                </div>
                <h4>
                    Required Checkout Information</h4>
                <div class="spacer15">
                </div>
                <table class="checktable_supplier true">
                    <tr>
                        <td>
                            <asp:DataList ID="dtCheckOutInfo" runat="server">
                                <ItemTemplate>
                                    <span class="custom-checkbox alignleft" id="checkoutspan" runat="server">
                                        <asp:CheckBox ID="chkCheckOutInfo" runat="server" /></span>
                                    <label>
                                        <asp:Label ID="lblCheckOutInfo" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                    <asp:HiddenField ID="hdnChecoutInfo" runat="server" Value='<%#Eval("iLookupID")%>' />
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="formtd alignleft" style="visibility: hidden;">
                <table class="checktable_supplier true">
                    <tr>
                        <td>
                            <span class="custom-checkbox alignleft" id="spanthirdpartybilling" runat="server">
                                <asp:CheckBox ID="chkThirdPartyBilling" runat="server" /></span><label>Third Party Vendor
                                    Billing</label>
                        </td>
                    </tr>
                </table>
                <div class="spacer10">
                    &nbsp;</div>
                <div id="dvThirdParty" runat="server" style="display: none;">
                    <table class="form_table">
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Company</span>
                                        <asp:TextBox ID="txtVendorCompany" CssClass="w_label" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Contact</span>
                                        <asp:TextBox ID="txtVendorContact" CssClass="w_label" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Address</span>
                                        <asp:TextBox ID="txtVendorAddress" CssClass="w_label" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">City</span>
                                        <asp:TextBox ID="txtVendorCity" CssClass="w_label" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">State/Province</span>
                                        <asp:TextBox ID="txtVendorState" CssClass="w_label" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Zip</span>
                                        <asp:TextBox ID="txtVendorZip" CssClass="w_label" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Tel</span>
                                        <asp:TextBox ID="txtVendorTelephone" CssClass="w_label" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Email</span>
                                        <asp:TextBox ID="txtVendorEmail" CssClass="w_label" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="spacer25">
        </div>
        <div class="bot_alert next">
            <asp:LinkButton ID="lnkNext" runat="server" title="Next Step 4" OnClick="lnkNext_Click">Next Step 4</asp:LinkButton>
        </div>
        <div class="bot_alert prev">
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        </div>
    </div>
</asp:Content>
