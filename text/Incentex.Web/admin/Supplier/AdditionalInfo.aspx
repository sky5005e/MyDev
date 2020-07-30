<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdditionalInfo.aspx.cs" Inherits="admin_Supplier_AdditionalInfo" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl" TagPrefix="mb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script language="javascript" type="text/javascript">
    // modal popup for Not active feature
        function FeatureNotActive() {
            jAlert("Currently this feature is not active..", "Feature Not Active", function(RetVal) {
            if (RetVal) {
                }
            });
        }
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="sc1" runat="server"></asp:ScriptManager>
            <mb:MenuUserControl ID="manuControl" runat="server" />


    <div class="form_pad">
        <div class="header_bg">
            <div class="header_bgr title">
                Additional Supplier Information</div>
        </div>
        <div class="supp_hist_btn clearfix btn_width">
            <div class="alignleft rightalign supp_width">
                <a class="gredient_btn" href="#" onclick="FeatureNotActive();" title="Purchase Order History">
                            <label>
                                <span>Purchase Order History</span></label></a> <a href="#" 
                                onclick="FeatureNotActive();" 
                                class="gredient_btn"
                                    title="Supplier Performance Scorcard">
                                    <label>
                                        <span>Supplier Performance Scorcard</span></label></a>
            </div>
            <div class="alignright supp_width">
                 <a href="#" class="gredient_btn"  onclick="FeatureNotActive();" 
                            title="Open Purchase Orders">
                            <label>
                                <span>Open Purchase Orders</span></label></a> <a href="#" 
                                onclick="FeatureNotActive();" 
                                class="gredient_btn" title="Spend History">
                                    <label>
                                        <span>Spend History</span></label></a>
            </div>
        </div>
       <%-- <div class="additional_btn">
            <ul class="clearfix">
                <li>
                    <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn">
								        <span>Save Information</span>
                    </asp:LinkButton>
                </li>
            </ul>
        </div>--%>
    </div>
</asp:Content>


