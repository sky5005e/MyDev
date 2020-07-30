<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="IssuancePackagePreorder.aspx.cs" Inherits="MyAccount_IssuancePackagePreorder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }

        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
    </script>

    <script type="text/javascript" language="javascript">
        // check if value is numeric in textbox
        function CheckNum(id) {
            var txt = document.getElementById(id);
            if (!IsNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.value = "";
                txt.focus();

            }
        }

        function IsNumeric(sText) {
            var ValidChars = "0123456789.";
            //var ValidChars = "0123456789";
            var IsNumber = true;
            var Char;


            for (i = 0; i < sText.length && IsNumber == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    IsNumber = false;
                }
            }
            return IsNumber;

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div id="dvNoRecord" runat="server" visible="false">
            <asp:Label ID="lblNoRecord" runat="server" Text="No Records Found" Font-Italic="true"
                ForeColor="Red"></asp:Label>
        </div>
        <div id="divDateEligible" runat="server" visible="false" class="centeralign">
            <asp:Label ID="lblDateEligible" runat="server" Font-Italic="true" ForeColor="Red"></asp:Label>
        </div>
        <div class="centeralign">
            <asp:Label ID="lblSingleMsg" runat="server" Font-Italic="true" ForeColor="Red"></asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <div id="divDate" runat="server" class="headersmall_bgr title">
            <table width="100%">
                <tr>
                    <td width="20%" style="font-weight: bold; color: #878282; font-size: 13px;">
                        Eligable Date :
                        <asp:Label ID="lblEligable" runat="server"></asp:Label>
                    </td>
                    <td width="15%">
                    </td>
                    <td width="15%">
                    </td>
                    <td width="15%">
                    </td>
                    <td width="17%">
                    </td>
                    <td width="18%" style="font-weight: bold; color: #878282; font-size: 13px">
                        Expires Date :
                        <asp:Label ID="lblExpire" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer10">
        </div>
        <div id="dvSingle" runat="server" class="headersmall_bg">
            <div class="headersmall_bgr title">
                <asp:Panel ID="pnlSingleHeader" runat="server">
                    <asp:Label ID="lblSingleItemAssociation" ForeColor="#878282" runat="server"></asp:Label>
                </asp:Panel>
                <at:CollapsiblePanelExtender ID="cpeSingle" runat="server" TargetControlID="pnlSingle"
                    CollapseControlID="pnlSingleHeader" ExpandControlID="pnlSingleHeader" CollapsedSize="0">
                </at:CollapsiblePanelExtender>
            </div>
        </div>
        <asp:Panel ID="pnlSingle" runat="server">
            <div class="tab_header" runat="server">
                <div class="tab_header_r" runat="server">
                    <table runat="server">
                        <tr>
                            <td style="width: 24%;">
                            </td>
                            <td style="width: 10%; color: #878282;">
                                Issuance
                            </td>
                            <td style="width: 9%; color: #878282;">
                                Purchased
                            </td>
                            <td style="width: 14%; color: #878282;">
                                Balance Left
                            </td>
                            <td style="width: 9%; color: #878282;" runat="server" id="tdSinglePrice">
                                Price
                            </td>
                            <td style="width: 14%; color: #878282;">
                                Quantity
                            </td>
                            <td style="width: 15%; color: #878282;">
                                Select Size
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:Repeater ID="rptSingleAssociation" runat="server" OnItemDataBound="rptSingleAssociation_ItemDataBound"
                OnItemCommand="rptSingleAssociation_ItemCommand">
                <ItemTemplate>
                    <div>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 24%; color: #878282;">
                                    <asp:Label ID="lblItemNumber" runat="server"></asp:Label>
                                    &nbsp;
                                    <div class="alignleft">
                                        <div class="agent_img">
                                            <p class="upload_photo gallery">
                                                <asp:HiddenField ID="hdnLookupIcon" runat="server" />
                                                <asp:HiddenField ID="hdnProductImage" runat="server" />
                                                <asp:HiddenField ID="hdnlargerimagename" runat="server" />
                                                <asp:HiddenField ID="hdnEmployeetype" Value='<%# Eval("EmployeeTypeid") %>' runat="server" />
                                                <asp:HiddenField ID="hdnWeathertype" Value='<%# Eval("WeatherTypeid") %>' runat="server" />
                                                <asp:HiddenField ID="hdnStoreProductid" Value='<%# Eval("StoreProductid") %>' runat="server" />
                                                <a id="prettyphotoDiv" href="~/UploadedImages/ProductImages/ProductDefault.jpg" rel='prettyPhoto'
                                                    runat="server">
                                                    <img id="imgSplashImage" src="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                                        height="198" width="145" runat="server" alt='Loading' />
                                                </a>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="aligncenter">
                                        <div class="leftalign" style="padding-left: 20px;">
                                            <asp:HyperLink ID="lnkSizingChart" Target="_blank" CssClass="btn_pdf2" Visible="false"
                                                runat="server"><span>Sizing Chart</span></asp:HyperLink>
                                        </div>
                                    </div>
                                    <div class="alignnone spacer20">
                                    </div>
                                    <div class="botbtn centeralign">
                                        <asp:Panel ID="pnlTailoring" runat="server">
                                            <div class="cart_header alignleft">
                                                <div class="left_co">
                                                </div>
                                                <div class="right_co">
                                                </div>
                                                <div class="tailoring_detail_pad">
                                                    <img src="../Images/tailoring-icon.gif" alt="" /><span style="color: #878282;">Tailoring
                                                        Details</span>
                                                    <br />
                                                    <span style="color: #878282;">Length:
                                                        <%--  <asp:Label runat="server" Width="25px" ID="lblTailoringLength" Text=' <%# DataBinder.Eval(Container, "DataItem.TailoringLength")%>'></asp:Label>--%>
                                                        <asp:TextBox runat="server" Width="25px" ID="txtSingleTailoringlenght"></asp:TextBox>
                                                    </span>
                                                    <br />
                                                    Run Charge :
                                                    <asp:Label ID="lblRunCharge" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </td>
                                <td style="width: 76%;">
                                    <table class="shoppingcart_box">
                                        <tr>
                                            <td style="padding: 0px;">
                                                <div class="cart_header">
                                                    <div class="left_co">
                                                    </div>
                                                    <div class="right_co">
                                                    </div>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr>
                                                            <%--<td style="width: 6%;color:#878282;">
                                                                
                                                            </td>--%>
                                                            <td style="width: 15%; color: #878282;">
                                                                <asp:Label ID="lblIssuance" runat="server"></asp:Label>
                                                            </td>
                                                            <td style="width: 9%; color: #878282;">
                                                                <asp:Label ID="lblPurchase" runat="server"></asp:Label>
                                                            </td>
                                                            <td style="width: 14%; color: #878282; text-align: center;">
                                                                <asp:Label ID="lblBalance" runat="server"></asp:Label>
                                                            </td>
                                                            <td style="width: 9%; color: #878282;">
                                                                <asp:Label ID="lblPrice" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdnItemNumber" runat="server" />
                                                            </td>
                                                            <td style="width: 14%;">
                                                                <div class="qty_selbox" style="padding-top: 4px;">
                                                                    <asp:TextBox ID="txtQty" runat="server" onchange="CheckNum(this.id)" MaxLength="5"
                                                                        Width="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                            <td style="width: 19%;">
                                                                <div class="form_top_co">
                                                                    <span>&nbsp;</span>
                                                                </div>
                                                                <div class="form_box">
                                                                    <span class="custom-sel custom_sel_size" style="width: 100%;">
                                                                        <asp:DropDownList ID="ddlSize" runat="server" OnSelectedIndexChanged="ddlSize_SelectedIndexChanged"
                                                                            AutoPostBack="true" onchange="pageLoad(this,value);">
                                                                        </asp:DropDownList>
                                                                    </span>
                                                                </div>
                                                                <div class="form_bot_co">
                                                                    <span>&nbsp;</span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="spacer10">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 0px;">
                                                <asp:Panel ID="pnlNameBarsForSingleAssociation" runat="server" Visible="false">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div id="Div1" class="alignleft" runat="server">
                                                                    Name to be Engraved:
                                                                    <asp:Label ID="lblNameTobeEngravedForSingleAssociation" runat="server"></asp:Label>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="Div2" class="alignleft" runat="server">
                                                                    Employee Title:
                                                                    <asp:Label ID="lblEmplTitleForSingleAssociation" runat="server"></asp:Label>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
                <SeparatorTemplate>
                    <div class="spacer10">
                    </div>
                </SeparatorTemplate>
            </asp:Repeater>
        </asp:Panel>
        <div class="alignnone spacer20">
        </div>
        <div class="centeralign">
            <asp:Label ID="lblGroupMsg" runat="server" Font-Italic="true" ForeColor="Red"></asp:Label>
        </div>
        <div id="dvGroup" runat="server" class="headersmall_bg">
            <div class="headersmall_bgr title">
                <asp:Panel ID="pnlGroupHeader" runat="server">
                    <asp:Label ID="lblGroupItemAssociation" ForeColor="#878282" runat="server"></asp:Label>
                </asp:Panel>
                <at:CollapsiblePanelExtender ID="cpeGroup" runat="server" TargetControlID="pnlGroup"
                    CollapseControlID="pnlGroupHeader" ExpandControlID="pnlGroupHeader" CollapsedSize="0">
                </at:CollapsiblePanelExtender>
            </div>
        </div>
        <div class="alignnone spacer20">
        </div>
        <asp:Panel ID="pnlGroup" runat="server">
            <asp:Repeater ID="rptGroupAssociation" runat="server" OnItemCommand="rptGroupAssociation_ItemCommand"
                OnItemDataBound="rptGroupAssociation_ItemDataBound">
                <ItemTemplate>
                    <div id="dvGroupParent" runat="server" class="headersmall_bg">
                        <div class="headersmall_bgr title">
                            <asp:Panel ID="pnlGroupHeaderParent" runat="server">
                                <asp:Label ID="lblGroupItemAssociationParent" ForeColor="#878282" runat="server"></asp:Label>
                            </asp:Panel>
                        </div>
                    </div>
                    <div id="Div3" class="tab_header" runat="server">
                        <div id="Div4" class="tab_header_r" runat="server">
                            <table id="Table1" runat="server">
                                <tr>
                                    <td style="width: 24%;" class="alignleft">
                                    </td>
                                    <td style="width: 10%; color: #878282;">
                                        Issuance
                                    </td>
                                    <td style="width: 9%; color: #878282;">
                                        Purchased
                                    </td>
                                    <td style="width: 14%; color: #878282;">
                                        Balance Left
                                    </td>
                                    <td style="width: 9%; color: #878282;" runat="server">
                                        <asp:Label ID="tdGroupPrice" Text="Price" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 14%; color: #878282;">
                                        Quantity
                                    </td>
                                    <td style="width: 15%; color: #878282;">
                                        Select Size
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="tab_header">
                        <div class="tab_header_r">
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td style="width: 28%;">
                                    </td>
                                    <td style="width: 15%;" class="alignleft">
                                        <asp:HiddenField ID="hdnNEWGROUP" runat="server" Value='<%# Eval("NEWGROUP") %>' />
                                    </td>
                                    <td style="width: 10%; color: #878282;">
                                        <asp:Label ID="lblGroupIssuance" Text='<%# Eval("Issuance") %>' runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 9%; color: #878282;">
                                        <asp:Label ID="lblPurchaseGroupBalance" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 14%; color: #878282;">
                                        <asp:Label ID="lblGroupBalanceLeft" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 9%; color: #878282;">
                                    </td>
                                    <td style="width: 14%; color: #878282;">
                                    </td>
                                    <td style="width: 19%; color: #878282;">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div>
                        <asp:Repeater ID="rptChildGroupAssociation" runat="server" OnItemDataBound="rptChildGroupAssociation_ItemDataBound">
                            <ItemTemplate>
                                <div>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 24%; color: #878282;">
                                                <asp:HiddenField ID="hdnChildGroupMasterItemId" Value='<%#Eval("MasterItemId")%>'
                                                    runat="server" />
                                                <asp:Label ID="lblItemNumber" runat="server"></asp:Label>
                                                &nbsp;&nbsp;
                                                <div class="alignleft">
                                                    <div class="agent_img">
                                                        <p class="upload_photo gallery">
                                                            <asp:HiddenField ID="hdnLookupIcon" runat="server" />
                                                            <asp:HiddenField ID="hdnProductImage" runat="server" />
                                                            <asp:HiddenField ID="hdnlargerimagename" runat="server" />
                                                            <asp:HiddenField ID="hdnEmployeetype" Value='<%# Eval("EmployeeTypeid") %>' runat="server" />
                                                            <asp:HiddenField ID="hdnWeathertype" Value='<%# Eval("WeatherTypeid") %>' runat="server" />
                                                            <asp:HiddenField ID="hdnStoreProductid" Value='<%# Eval("StoreProductid") %>' runat="server" />
                                                            <a id="prettyphotoDiv" href="~/UploadedImages/employeePhoto/employee-photo.gif" rel='prettyPhoto'
                                                                runat="server">
                                                                <img id="imgSplashImage" src="~/UploadedImages/employeePhoto/employee-photo.gif"
                                                                    height="198" width="145" runat="server" alt='Loading' />
                                                            </a>
                                                        </p>
                                                    </div>
                                                </div>
                                                <div class="aligncenter">
                                                    <div class="leftalign" style="padding-left: 20px;">
                                                        <asp:HyperLink ID="lnkGroupSizingChart" Target="_blank" CssClass="btn_pdf2" Visible="true"
                                                            runat="server"><span>Sizing Chart</span></asp:HyperLink>
                                                    </div>
                                                </div>
                                                <div class="alignnone spacer20">
                                                </div>
                                                <div class="botbtn centeralign">
                                                    <asp:Panel ID="pnlTailoringGroup" runat="server">
                                                        <div class="cart_header alignleft">
                                                            <div class="left_co">
                                                            </div>
                                                            <div class="right_co">
                                                            </div>
                                                            <div class="tailoring_detail_pad">
                                                                <img src="../Images/tailoring-icon.gif" alt="" /><span style="color: #878282;">Tailoring
                                                                    Details</span>
                                                                <br />
                                                                <span style="color: #878282;">Length:
                                                                    <%--  <asp:Label runat="server" Width="25px" ID="lblTailoringLength" Text=' <%# DataBinder.Eval(Container, "DataItem.TailoringLength")%>'></asp:Label>--%>
                                                                    <asp:TextBox runat="server" Width="25px" ID="txtGroupTailoringlenght"></asp:TextBox>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                                <div class="aligncenter">
                                                    <%--Pilot Shirt--%>
                                                </div>
                                            </td>
                                            <td style="width: 76%;">
                                                <table class="shoppingcart_box">
                                                    <tr>
                                                        <td style="padding: 0px;">
                                                            <div class="cart_header">
                                                                <div class="left_co">
                                                                </div>
                                                                <div class="right_co">
                                                                </div>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td style="width: 20%;">
                                                                        </td>
                                                                        <td style="width: 10%;">
                                                                        </td>
                                                                        <td style="width: 14%;">
                                                                        </td>
                                                                        <td style="width: 9%; color: #878282;">
                                                                            <asp:Label ID="lblGroupPrice" runat="server"></asp:Label>
                                                                            <asp:HiddenField ID="hdnGroupItemNumber" runat="server" />
                                                                        </td>
                                                                        <td style="width: 13%;">
                                                                            <div class="qty_selbox" style="padding-top: 4px;">
                                                                                <asp:TextBox ID="txtQty" onchange="CheckNum(this.id)" MaxLength="5" runat="server"
                                                                                    Width="25px"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                        <td style="width: 20%;">
                                                                            <div class="form_top_co">
                                                                                <span>&nbsp;</span>
                                                                            </div>
                                                                            <div class="form_box">
                                                                                <span class="custom-sel custom_sel_size" style="width: 100%;">
                                                                                    <asp:DropDownList ID="ddlSize" runat="server" OnSelectedIndexChanged="ddlSize1_SelectedIndexChanged"
                                                                                        AutoPostBack="true" onchange="pageLoad(this,value);">
                                                                                    </asp:DropDownList>
                                                                                </span>
                                                                            </div>
                                                                            <div class="form_bot_co">
                                                                                <span>&nbsp;</span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="spacer10">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 0px;">
                                                            <asp:Panel ID="pnlNameBarsForGroupAssociation" runat="server" Visible="false">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <div id="Div1" class="alignleft" runat="server">
                                                                                Name to be Engraved:
                                                                                <asp:Label ID="lblNameTobeEngravedForGroupAssociation" runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div id="Div2" class="alignleft" runat="server">
                                                                                Employee Title:
                                                                                <asp:Label ID="lblEmplTitleForGroupAssociation" runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </ItemTemplate>
                <SeparatorTemplate>
                    <div class="spacer10">
                    </div>
                </SeparatorTemplate>
            </asp:Repeater>
        </asp:Panel>
        <div class="alignnone spacer20">
        </div>
        <div class="centeralign">
            <asp:Label ID="lblGroupBudgetMsg" runat="server" Font-Italic="true" ForeColor="Red"></asp:Label>
        </div>
        <div id="dvGroupBudget" runat="server" class="headersmall_bg">
            <div class="headersmall_bgr title">
                <asp:Panel ID="pnlGroupBudgetHeader" runat="server">
                    <asp:Label ID="lblGroupItemAssociationBudget" ForeColor="#878282" runat="server"></asp:Label>
                </asp:Panel>
                <at:CollapsiblePanelExtender ID="cpeGroupBudget" runat="server" TargetControlID="pnlGroupBudget"
                    CollapseControlID="pnlGroupBudgetHeader" ExpandControlID="pnlGroupBudgetHeader"
                    CollapsedSize="0">
                </at:CollapsiblePanelExtender>
            </div>
        </div>
        <div class="alignnone spacer20">
        </div>
        <asp:Panel ID="pnlGroupBudget" runat="server">
            <asp:Repeater ID="rptGroupBudgetAssociation" runat="server" OnItemDataBound="rptGroupBudgetAssociation_ItemDataBound">
                <ItemTemplate>
                    <div id="dvGroupParentBudget" runat="server" class="headersmall_bg">
                        <div class="headersmall_bgr title">
                            <asp:Panel ID="pnlGroupBudgetHeaderParent" runat="server">
                                <asp:Label ID="lblGroupItemAssociationBudgetParent" ForeColor="#878282" runat="server"></asp:Label>
                            </asp:Panel>
                        </div>
                    </div>
                    <div id="Div5" class="tab_header" runat="server">
                        <div id="Div6" class="tab_header_r" runat="server">
                            <table id="Table2" runat="server">
                                <tr>
                                    <td style="width: 22%;" class="alignleft">
                                    </td>
                                    <td style="width: 2%; color: #878282;" runat="server">
                                        <asp:Label ID="tdIssuance" Text="Issuance" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td style="width: 2%; color: #878282;" runat="server">
                                        <asp:Label ID="Td1" Text="Purchased" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td style="width: 2%; color: #878282;" runat="server">
                                        <asp:Label ID="Td2" Text=" Balance Left" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td style="width: 11%; color: #878282;">
                                        Budget Amt
                                    </td>
                                    <td id="Td3" style="width: 11%; color: #878282;">
                                        Budget Used
                                    </td>
                                    <td style="width: 11%; color: #878282;">
                                        Budget Left
                                    </td>
                                    <td style="width: 10%; color: #878282;">
                                        <asp:Label ID="tdBudgetPrice" Text="Price" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 10%; color: #878282;">
                                        Quantity
                                    </td>
                                    <td style="width: 19%; color: #878282;">
                                        Select Size
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="tab_header">
                        <div class="tab_header_r">
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td style="width: 22%;" class="alignleft">
                                        <asp:HiddenField ID="hdnNEWGROUP" runat="server" Value='<%# Eval("NEWGROUP") %>' />
                                    </td>
                                    <td id="Td4" style="width: 2%; color: #878282;" runat="server" visible="false">
                                        <asp:Label ID="lblGroupBudgetIssuance" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 20%; color: #878282;">
                                        <asp:Label ID="lblPurchaseGroupBudget" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td style="width: 2%; color: #878282;">
                                        <asp:Label ID="lblGroupBudgetBalanceLeft" Visible="false" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 11%; color: #878282;">
                                        <asp:Label ID="lblGroupBudgetAmount" Text='<%# Eval("AssociationbudgetAmt") %>' runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 11%; color: #878282;">
                                        <asp:Label ID="lblBudgetUsed" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 11%; color: #878282;">
                                        <asp:Label ID="lblGroupBudgetAmountLeft" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 10%;">
                                    </td>
                                    <td style="width: 10%;">
                                    </td>
                                    <td style="width: 19%;">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div>
                        <asp:Repeater ID="rptChildGroupBudgetAssociation" runat="server" OnItemDataBound="rptChildGroupBudgetAssociation_ItemDataBound">
                            <ItemTemplate>
                                <div>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 22%; color: #878282;">
                                                <asp:HiddenField ID="hdnChildGroupBudgetAssociationMasterItemId" Value='<%#Eval("MasterItemId")%>'
                                                    runat="server" />
                                                <asp:Label ID="lblItemNumber" runat="server"></asp:Label>
                                                &nbsp;
                                                <div class="alignleft">
                                                    <div class="agent_img">
                                                        <p class="upload_photo gallery">
                                                            <asp:HiddenField ID="hdnLookupIcon" runat="server" />
                                                            <asp:HiddenField ID="hdnProductImage" runat="server" />
                                                            <asp:HiddenField ID="hdnlargerimagename" runat="server" />
                                                            <asp:HiddenField ID="hdnEmployeetype" Value='<%# Eval("EmployeeTypeid") %>' runat="server" />
                                                            <asp:HiddenField ID="hdnWeathertype" Value='<%# Eval("WeatherTypeid") %>' runat="server" />
                                                            <asp:HiddenField ID="hdnStoreProductid" Value='<%# Eval("StoreProductid") %>' runat="server" />
                                                            <%--<asp:HiddenField ID="hdnMasterItemID" Value='<%#Eval("MasterItemId")%>' runat="server" />--%>
                                                            <a id="prettyphotoDiv" href="~/UploadedImages/employeePhoto/employee-photo.gif" rel='prettyPhoto'
                                                                runat="server">
                                                                <img id="imgSplashImage" src="~/UploadedImages/employeePhoto/employee-photo.gif"
                                                                    height="198" width="145" runat="server" alt='Loading' />
                                                            </a>
                                                        </p>
                                                    </div>
                                                </div>
                                                <div class="aligncenter">
                                                    <div class="leftalign" style="padding-left: 20px;">
                                                        <asp:HyperLink ID="lnkGroupBudgetSizingChart" Target="_blank" CssClass="btn_pdf2" Visible="true"
                                                            runat="server"><span>Sizing Chart</span></asp:HyperLink>
                                                    </div>
                                                </div>
                                                <div class="alignnone spacer20">
                                                </div>
                                                <div class="botbtn centeralign">
                                                    <asp:Panel ID="pnlTailoringGroupBudget" runat="server">
                                                        <div class="cart_header alignleft">
                                                            <div class="left_co">
                                                            </div>
                                                            <div class="right_co">
                                                            </div>
                                                            <div class="tailoring_detail_pad">
                                                                <img src="../Images/tailoring-icon.gif" alt="" /><span style="color: #878282;">Tailoring
                                                                    Details</span>
                                                                <br />
                                                                <span style="color: #878282;">Length:
                                                                    <asp:TextBox runat="server" Width="25px" ID="txtBudgetTailoringlenght"></asp:TextBox>
                                                                    <%--  <asp:Label runat="server" Width="25px" ID="lblTailoringLength" Text=' <%# DataBinder.Eval(Container, "DataItem.TailoringLength")%>'></asp:Label>--%>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                                <div class="aligncenter">
                                                    <%--Pilot Shirt--%>
                                                </div>
                                            </td>
                                            <td style="width: 78%;">
                                                <table class="shoppingcart_box">
                                                    <tr>
                                                        <td style="padding: 0px;">
                                                            <div class="cart_header">
                                                                <div class="left_co">
                                                                </div>
                                                                <div class="right_co">
                                                                </div>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td id="Td5" style="width: 2%;" visible="false" runat="server">
                                                                        </td>
                                                                        <td id="Td6" style="width: 2%;" visible="false" runat="server">
                                                                        </td>
                                                                        <td id="Td7" style="width: 2%;" visible="false" runat="server">
                                                                        </td>
                                                                        <td style="width: 11%;">
                                                                        </td>
                                                                        <td style="width: 11%;">
                                                                        </td>
                                                                        <td style="width: 11%;">
                                                                        </td>
                                                                        <td style="width: 10%; color: #878282;">
                                                                            <asp:Label ID="lblGroupBudgetPrice" runat="server"></asp:Label>
                                                                            <asp:HiddenField ID="hdnBudgetItemNumber" runat="server" />
                                                                        </td>
                                                                        <td style="width: 10%;">
                                                                            <div class="qty_selbox" style="padding-top: 4px;">
                                                                                <asp:TextBox ID="txtQty" onchange="CheckNum(this.id)" MaxLength="5" runat="server"
                                                                                    Width="25px"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                        <td style="width: 19%;">
                                                                            <div class="form_top_co">
                                                                                <span>&nbsp;</span>
                                                                            </div>
                                                                            <div class="form_box">
                                                                                <span class="custom-sel custom_sel_size" style="width: 100%;">
                                                                                    <asp:DropDownList ID="ddlSize" OnSelectedIndexChanged="ddlSize2_SelectedIndexChanged"
                                                                                        AutoPostBack="true" runat="server" onchange="pageLoad(this,value);">
                                                                                    </asp:DropDownList>
                                                                                </span>
                                                                            </div>
                                                                            <div class="form_bot_co">
                                                                                <span>&nbsp;</span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="spacer10">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 0px;">
                                                            <asp:Panel ID="pnlNameBarsForBudgetAssociation" runat="server" Visible="false">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <div id="Div1" class="alignleft" runat="server">
                                                                                Name to be Engraved:
                                                                                <asp:Label ID="lblNameTobeEngravedForBudgetAssociation" runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div id="Div2" class="alignleft" runat="server">
                                                                                Employee Title:
                                                                                <asp:Label ID="lblEmplTitleForBudgetAssociation" runat="server"></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </ItemTemplate>
                <SeparatorTemplate>
                    <div class="spacer10">
                    </div>
                </SeparatorTemplate>
            </asp:Repeater>
        </asp:Panel>
        <div class="alignnone spacer20">
        </div>
        <div id="dvShipandCheckout" runat="server">
            <table class="form_table">
                <tr style="width: 40%;" runat="server" visible="false">
                    <td class="formtd">
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label max_w">Please ship on</span>
                                <asp:TextBox ID="txtShippingDate" runat="server" CssClass="w_label datepicker min_w"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="formtd" colspan="2">
                        <div class="spacer20">
                        </div>
                        <div class="botbtn centeralign">
                            <asp:LinkButton ID="btnCheckOut" runat="server" CssClass="grey2_btn" ToolTip="Proceed to Checkout"
                                OnClick="btnCheckOut_Click"><span>Check Out</span></asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
