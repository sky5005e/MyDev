<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TrackingCenter.aspx.cs" Inherits="MyAccount_TrackingCenter" Title="Tracking Center" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(window).load(function() {
            $("#dvLoader").hide();
        });
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $("#dvLoader").show();
        });
    </script>

    <div id="dvLoader">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div class="btn_width worldlink_btn">
            <div align="center">
                <asp:DataList ID="dtIndex" runat="server" RepeatDirection="Vertical" RepeatColumns="2"
                    OnItemCommand="dtIndex_ItemCommand" Width="50%">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkMenuAccess" CommandName="Redirect" CommandArgument='<%# Eval("sLookupName")%>'
                            ToolTip='<%# Eval("sLookupName")%>' CssClass="gredient_btn" runat="server"><span><strong><%# Eval("sLookupName")%></strong></span></asp:LinkButton>
                        <asp:HiddenField ID="hdnMenuAccess" runat="server" Value='<%# Eval("iLookupID") %>' />
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </div>
    </div>
</asp:Content>
