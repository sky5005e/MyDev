<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewIssuancePrograms.aspx.cs" Inherits="admin_CompanyStore_CompanyPrograms_ViewIssuancePrograms" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        // change value in custom dropdown
        function pageLoad(sender, args) {
            assigndesign();
        }
        function SetTextboxEnable(txtid, chkid) {
            var preval = document.getElementById('ctl00_ContentPlaceHolder1_hdngrpValue');
            var txt = document.getElementById(txtid);
            var chkbx = document.getElementById(chkid);
            if (chkbx.checked == true) {
                preval.value = txt.value;
                txt.value = "";
                txt.focus();
                txt.disabled = false;
            }
            else {
                txt.value = preval.value;
                preval = "";
                txt.disabled = true;
            }
        }
        // select checkboxes in grid
        function SelectAllCheckboxesSpecific(spanChk) {
            var IsChecked = spanChk.checked;
            var Chk = spanChk;
            Parent = document.getElementById('ctl00_ContentPlaceHolder1_gv');
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != Chk && items[i].type == "checkbox") {
                    if (items[i].checked != IsChecked) {
                        items[i].click();
                    }
                }
            }
        }     
     
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div class="form_pad">
                <div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                    <br />
                </div>
                <div>
                    <!-- Grid Start -->
                    <asp:GridView ID="gv" runat="server" OnDataBinding="gv_DataBinding" AutoGenerateColumns="false"
                        HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                        RowStyle-CssClass="ord_content" OnRowDataBound="gv_RowDataBound" OnDataBound="gv_DataBound"
                        OnRowCommand="gv_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span>
                                        <asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this);"
                                            runat="server" /> &nbsp;</span>
                                         <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemTemplate>
                                    <span class="first">
                                        <asp:CheckBox ID="chkDelete" runat="server" />&nbsp; </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="5%" CssClass="b_box centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblID" Text='<%#Eval("UniformIssuancePolicyID")%>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="1%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkProgramName" runat="server" CommandArgument="IssuanceProgramName"
                                        CommandName="Sorting"><span >Program Name</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span>
                                        <asp:LinkButton ID="lnkIssuanceProgramName" runat="server" Text='<%#Eval("IssuanceProgramName")%>'
                                            CommandName="EditRec" CommandArgument='<%#Eval("UniformIssuancePolicyID")%>'></asp:LinkButton>
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="21%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkGroupName" runat="server" CommandArgument="GroupName" CommandName="Sorting"><span>Group Name</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="btn_space">
                                        <asp:TextBox ID="txtDprt" runat="server" Text='<%#Eval("GroupName").ToString().Length==0 ? "No Group Name" : Eval("GroupName")%>' Enabled="false"
                                        Style="background-color: #303030; border: medium none; color: #ADADAD; padding: 2px; width: 100%; text-align: center;"></asp:TextBox>
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkWorkGroup" runat="server" CommandArgument="WorkGroup" CommandName="Sorting"><span class="white_co">WorkGroup</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblWorkGroup" Text='<%#Eval("WorkGroup")%>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Eligible ( Date or Months ) </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEligibleDate" Text='<% # (Convert.ToString(Eval("EligibleDateDisp")).Length > 35) ? Eval("EligibleDateDisp").ToString().Substring(0,35)+"..." : Convert.ToString(Eval("EligibleDateDisp"))+ "&nbsp;"  %>'
                                        ToolTip='<% #Eval("EligibleDateDisp")  %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="18%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span class="white_co">Expires In ( Date or Months ) </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblExpiresAfter" Text='<% # (Convert.ToString(Eval("CreditExpireDateDisp")).Length > 35) ? Eval("CreditExpireDateDisp").ToString().Substring(0,35)+"..." : Convert.ToString(Eval("CreditExpireDateDisp"))+ "&nbsp;"  %>'
                                        ToolTip='<% #Eval("CreditExpireDateDisp")  %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkStatus" runat="server" CommandArgument="Status" CommandName="Sorting"><span >Status</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="btn_space" style="text-align: center;">
                                        <asp:ImageButton ID="imgbtnStatus" runat="server" Style="height: 20px; width: 20px;"
                                            CommandName="ChangeStatus" CommandArgument='<%#Eval("UniformIssuancePolicyID")%>' />
                                        <asp:HiddenField ID="hdnStatus" runat="server" Value='<%#Eval("Status")%>' />
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="5%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <!-- Grid End -->
                </div>
                <div>
                    <div class="companylist_botbtn alignleft">
                        <asp:LinkButton ID="btnAddUniformIssuanceProg" runat="server" TabIndex="0" CssClass="grey_btn"
                            OnClick="btnAddUniformIssuanceProg_Click"><span>Add Uniform Issuance Program</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lnkDelete" runat="server" class="grey_btn" OnClick="lnkDelete_Click"
                            OnClientClick="return confirm('Are you sure , you want to delete selected record ?')">
						                <span>Delete</span>
                        </asp:LinkButton>
                         <asp:LinkButton ID="LnkSaveGroupName" runat="server" class="grey_btn" OnClick="LnkSaveGroupName_Click">
						                <span>Save GroupName</span>
                        </asp:LinkButton>
                    </div>
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
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdngrpValue" runat="server" />
</asp:Content>
