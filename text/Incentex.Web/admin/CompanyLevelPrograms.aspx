<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CompanyLevelPrograms.aspx.cs" Inherits="admin_CompanyLevelPrograms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <h4>
            Anniversary Credit Program</h4>
        <h4>
            <asp:Label ID="NoProgram" runat="server"></asp:Label>
        </h4>
        <asp:LinkButton ID="lnkBtnGetInformation" CssClass="grey_btn" runat="server" ToolTip="Save Information"
            OnClick="lnkBtnGetInformation_Click"><span>Get Users</span></asp:LinkButton>
        <div class="divider">
        </div>
        <h4>
            Uniform Issuance Program</h4>
        <div class="spacer20">
        </div>
        <div id="issuance">
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <table class="max_label_width">
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <label class="dropimg_width386">
                                                <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlEmployeePolicyStatus" onchange="pageLoad(this,value);" runat="server">
                                                    </asp:DropDownList>
                                                </span>
                                                <div id="divProductStatus">
                                                </div>
                                            </label>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="bot_alert next">
            <asp:LinkButton ID="lnkNext" runat="server" title="Next Step 4" OnClick="lnkNext_Click">Next Step 4</asp:LinkButton>
        </div>
        <div class="bot_alert prev">
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        </div>
    </div>
</asp:Content>
