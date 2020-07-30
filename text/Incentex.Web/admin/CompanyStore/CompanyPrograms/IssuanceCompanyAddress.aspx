<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="IssuanceCompanyAddress.aspx.cs" Inherits="admin_CompanyStore_CompanyPrograms_IssuanceCompanyAddress" Title="Shipping Billing Address" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript" language="javascript">
        // change value in custom dropdown
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }

             
     
    </script>
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
   <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            <br />
        </div>
      
        <div class="spacer25">
        </div>
        <div class="pro_search_pad">
          
           <div style="text-align: left">
            <asp:Label ID="lblFor" runat="server" CssClass="input_label"></asp:Label>
            <br />
        </div>
          
          
          <div class="spacer25">
        </div>
          
                <div class="header_bg" style="text-align: left;">
                    <div class="header_bgr title_small">
                        Billing Information</div>
                </div>
                <div class="spacer10">
                </div>
                <div>
                    <table class="form_table">
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Company</span>
                                        <asp:TextBox ID="TxtBillingCompanyName" runat="server" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">First Name</span>
                                        <asp:TextBox ID="TxtFirstName" runat="server" CssClass="w_label"></asp:TextBox>
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
                                        <span class="input_label">Last Name</span>
                                        <asp:TextBox ID="TxtLastName" runat="server" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                            <td>
                            </td>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Address1</span>
                                        <asp:TextBox ID="TxtBillingAddress1" runat="server" CssClass="w_label"></asp:TextBox>
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
                                        <span class="input_label">Address2</span>
                                        <asp:TextBox ID="TxtBillingAddress2" runat="server" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upBillingCountry" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="DrpBillingCountry" runat="server" AutoPostBack="true" CssClass="w_label"
                                                        OnSelectedIndexChanged="DrpBillingCountry_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upBillingState" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="DrpBillingState" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DrpBillingState_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upCity" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="DrpBillingCity" runat="server">
                                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Zip</span>
                                        <asp:TextBox ID="TxtBillingZip" runat="server" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Telephone</span>
                                                <asp:TextBox ID="TxtPhone" CssClass="w_label" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Mobile</span>
                                                <asp:TextBox ID="TxtMobile" CssClass="w_label" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Email</span>
                                                <asp:TextBox ID="TxtEmail" CssClass="w_label" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="spacer10">
                </div>
                <div class="header_bg" style="text-align: left;">
                    <div class="header_bgr title_small">
                        Shipping Information</div>
                </div>
                <div class="spacer10">
                </div>
                <div>
                    <table class="form_table">
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Company</span>
                                        <asp:TextBox ID="txtSCompany" runat="server" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">First Name</span>
                                        <asp:TextBox ID="txtSFName" runat="server" CssClass="w_label"></asp:TextBox>
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
                                        <span class="input_label">Last Name</span>
                                        <asp:TextBox ID="txtSLName" runat="server" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                            <td>
                            </td>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Address1</span>
                                        <asp:TextBox ID="txtSAddress1" runat="server" CssClass="w_label"></asp:TextBox>
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
                                        <span class="input_label">Address2</span>
                                        <asp:TextBox ID="txtSAddress2" runat="server" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlSCountry" runat="server" AutoPostBack="true" CssClass="w_label"
                                                        OnSelectedIndexChanged="ddlSCountry_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlSState" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSState_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlSCity" runat="server">
                                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Zip</span>
                                        <asp:TextBox ID="txtSZip" runat="server" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Telephone</span>
                                                <asp:TextBox ID="txtSTelephone" CssClass="w_label" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Mobile</span>
                                                <asp:TextBox ID="txtSMobile" CssClass="w_label" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Email</span>
                                                <asp:TextBox ID="txtSEmail" CssClass="w_label" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
          
            <div class="spacer25">
            </div>
            <div class="spacer25">
            </div>
            <!-- button -->
            <div class="botbtn centeralign">
                <asp:LinkButton ID="lnkAddItem" runat="server" CssClass="grey2_btn" OnClick="lnkAddItem_Click"><span>Add Address</span></asp:LinkButton>&nbsp;
            </div>
        </div>
       
        <div class="bot_alert next">
            <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click">Next Step 3 </asp:LinkButton>
        </div>
        <div class="bot_alert prev">
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        </div>
    </div>
  
</asp:Content>