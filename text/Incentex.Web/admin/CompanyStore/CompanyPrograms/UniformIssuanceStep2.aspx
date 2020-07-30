<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="UniformIssuanceStep2.aspx.cs" Inherits="admin_CompanyStore_CompanyPrograms_UniformIssuanceStep1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        // change value in custom dropdown
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }

        // select all checkboxes
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

    <script language="javascript" type="text/javascript">
        $().ready(function() {
            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                //alert(objValMsg);

                $("#aspnetForm").validate({
                    onsubmit: false,
                    rules: {
                    ctl00$ContentPlaceHolder1$txtNewGroup: { required: true,},
                        ctl00$ContentPlaceHolder1$ddlAssociationPolicy: { NotequalTo: "0" }
                       , ctl00$ContentPlaceHolder1$ddlItems: { NotequalTo: "0" }
                        
                    , ctl00$ContentPlaceHolder1$ddlIssuance: { NotequalTo: "0" }
                    }
                     , messages:
                    {
                        ctl00$ContentPlaceHolder1$txtNewGroup: {
                            required: replaceMessageString(objValMsg, "Required", "Group Name")},
                        ctl00$ContentPlaceHolder1$ddlAssociationPolicy: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "Issuance association ploicy") }
                       , ctl00$ContentPlaceHolder1$ddlItems: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "master item") }
                        
                        , ctl00$ContentPlaceHolder1$ddlIssuance: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "Issuance") }

}//messages
                    }); //validate
                }); //get

                $("#<%=lnkAddItem.ClientID %>").click(function() {
                    return $('#aspnetForm').valid();
                });
               
            });          //ready



    </script>

    <script language="javascript" type="text/javascript">

        // check if value is numeric in textbox
        //        var lblmessage = document.getElementById("ctl00_ContentPlaceHolder1_lblMsg");
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
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            <br />
        </div>
        <asp:Panel runat="server" ID="pnlUniIssuancePolicy">
            <div class="uniform_pad uniform_pad_w">
                <h4>
                    Please select the item(s) that you will be setting a reminder for and also the number
                    of pieces that will be issued:</h4>
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
                <div class="black_middle form_table">
                    <div class="add_item_left alignleft">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="custom-sel">
                                <asp:DropDownList ID="ddlAssociationPolicy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAssociationPolicy_SelectedIndexChanged">
                                </asp:DropDownList>
                            </span>
                        </div>
                        <div class="form_bot_co" runat="server" id="divNewGroupAll" visible="false">
                            <span>&nbsp;</span></div>
                        <asp:Panel runat="server" ID="pnlNewGroup" Visible="false">
                            <div runat="server" id="divNewGroup">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span id="span2" runat="server" class="input_label">Group Name</span>
                                    <asp:TextBox ID="txtNewGroup" runat="server" CssClass="w_label" OnTextChanged="txtNewGroup_TextChanged"
                                        AutoPostBack="true"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form_bot_co" visible="false" runat="server" id="divbamountbottom">
                            <span>&nbsp;</span></div>
                        <div class="form_bot_co" runat="server" id="divNote" visible="false">
                            <span>&nbsp;</span></div>
                        <div runat="server" id="divPolicyNote" visible="false">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span id="span1" runat="server" class="input_label">Policy Note : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                <asp:TextBox ID="txtAssociationPolicyNote" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form_bot_co" runat="server" id="divPolicyNoteBudgetamt" visible="false">
                            <span>&nbsp;</span></div>
                        <asp:Panel runat="server" ID="divBudgetamt" Visible="false">
                            <div runat="server" id="ddivamount">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span id="spanBAmount" runat="server" class="input_label">Budget Amount($):</span>
                                    <asp:TextBox ID="txtBudgetAmt" onchange="CheckNum(this.id)" MaxLength="9" runat="server"
                                        CssClass="w_label"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form_bot_co" runat="server" id="divIssuanceBudget" visible="false">
                            <span>&nbsp;</span></div>
                        <!-- Issuance dropdown-->
                        <div id="divIssuanceAssociation" class="form_bot_co" runat="server">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <asp:UpdatePanel ID="upItems" runat="server">
                                <ContentTemplate>
                                    <span class="custom-sel">
                                        <asp:DropDownList ID="ddlIssuance" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIssuance_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </span>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <asp:UpdatePanel ID="upIssuance" runat="server">
                                <ContentTemplate>
                                    <span class="custom-sel">
                                        <asp:DropDownList ID="ddlItems" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlItems_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </span>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label">Employee Type :</span>
                            <label class="dropimg_width275">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlEmployeeType" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </label>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label">Climate :</span>
                            <label class="dropimg_width275">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlClimate" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </label>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                        <asp:Panel ID="pnlRank" runat="server">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Department :</span>
                                <label class="dropimg_width275">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlDepartment" runat="server" onchange="pageLoad(this,value);">
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </asp:Panel>
                    </div>
                    <div class="alignright agent_name">
                        <div class="agent_img">
                            <asp:UpdatePanel ID="upImage" runat="server">
                                <ContentTemplate>
                                    <div class="upload_photo gallery" runat="server">
                                        <img runat="server" id="imgPhoto" height="150" width="150" alt="Item Image" />
                                        <a id="prettyphotoDiv" href="~/UploadedImages/ProductImages/ProductDefault.jpg" rel='prettyPhoto[i]'
                                            runat="server">
                                            <img id="imgSplashImage" runat="server" src="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                                height="198" width="145" runat="server" alt='Loading' />
                                        </a>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="alignnone">
                    </div>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
            </div>
        </asp:Panel>
        <div class="spacer25">
        </div>
        <div class="botbtn centeralign">
            <asp:LinkButton ID="lnkAddItem" runat="server" CssClass="grey2_btn" OnClick="lnkAddItem_Click"><span>Add Master Item</span></asp:LinkButton>&nbsp;
        </div>
        <div class="spacer25">
        </div>
        <div>
            <h4>
                List of all items</h4>
            <asp:GridView ID="gv" runat="server" OnDataBinding="gv_DataBinding" AutoGenerateColumns="False"
                HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                RowStyle-CssClass="ord_content" OnDataBound="gv_DataBound" OnRowCommand="gv_RowCommand"
                ShowFooter="True" OnRowEditing="gv_RowEditing" OnRowCancelingEdit="gv_RowCancelingEdit"
                OnRowUpdating="gv_RowUpdating" DataKeyNames="UniformIssuancePolicyItemID">
                <RowStyle CssClass="ord_content"></RowStyle>
                <Columns>
                    <asp:TemplateField SortExpression="MasterStyleName">
                        <HeaderTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtnMasterStyleName" runat="server" CommandArgument="MasterStyleName"
                                    CommandName="Sort">Master #</asp:LinkButton>
                            </span>
                            <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span class="first">
                                <%# Eval("ItemName")%>
                            </span>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                            <asp:HiddenField runat="server" ID="lblRankId" Value='<%# Eval("RankId") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkAssociationIssuanceType" runat="server" CommandName="Sort"><span>Association Type</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblsLookupName" runat="server" Text='<% # (Convert.ToString(Eval("sLookupName")).ToString().Length > 30) ? Eval("sLookupName").ToString().Substring(0,30)+"..." :  Convert.ToString(Eval("sLookupName")+ "&nbsp;") %>'
                                ToolTip='<%# Eval("sLookupName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkPolicyNote" runat="server" CommandName="Sort"><span>Policy Note</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPolicyNote" runat="server" Text='<% # (Convert.ToString(Eval("AssociationIssuancePolicyNote")).ToString().Length > 10) ? Eval("AssociationIssuancePolicyNote").ToString().Substring(0,10)+"..." :  Convert.ToString(Eval("AssociationIssuancePolicyNote")+ "&nbsp;") %>'
                                ToolTip='<%# Eval("AssociationIssuancePolicyNote") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkIssuance" runat="server" CommandName="Sort"><span>Issuance</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblIssuance" runat="server" Text='<% # Eval("Issuance") + "&nbsp;" %>'
                                ToolTip='<%# Eval("Issuance") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="7%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkBudgetAmount" runat="server" CommandName="Sort"><span>Budget Amt</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblBudgetAmt" runat="server" Text='<% # Eval("AssociationbudgetAmt") + "&nbsp;" %>'
                                ToolTip='<%# Eval("AssociationbudgetAmt") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="9%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkEmployeeType" runat="server" CommandName="Sort"><span>Employee Type</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblEmployeeType" runat="server" Text='<% # (Convert.ToString(Eval("EmployeeType")).ToString().Length > 8) ? Eval("EmployeeType").ToString().Substring(0,8)+"..." :  Convert.ToString(Eval("EmployeeType")+ "&nbsp;") %>'
                                ToolTip='<%# Eval("EmployeeType") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEmployeeType" Style="background-color: #303030; border: medium none;
                                color: #ffffff; width: 120px; padding: 2px" BackColor="#303030" runat="server"
                                OnSelectedIndexChanged="ddlEmployeeType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hdnEmployeeType" runat="server" Value='<%# Eval("EmployeeType") %>' />
                        </EditItemTemplate>
                        <ItemStyle CssClass="g_box" Width="9%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkClimateType" runat="server" CommandName="Sort"><span>Climate Type</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblClimateType" runat="server" Text='<% # (Convert.ToString(Eval("WeatherType")).ToString().Length > 8) ? Eval("WeatherType").ToString().Substring(0,8)+"..." :  Convert.ToString(Eval("WeatherType")+ "&nbsp;") %>'
                                ToolTip='<%# Eval("WeatherType") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlWeatherType" Style="background-color: #303030; border: medium none;
                                color: #ffffff; width: 130px; padding: 2px" BackColor="#303030" runat="server"
                                OnSelectedIndexChanged="ddlWeatherType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hdnWeatherType" runat="server" Value='<%# Eval("WeatherType") %>' />
                        </EditItemTemplate>
                        <ItemStyle CssClass="b_box" Width="9%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkGroupName" runat="server" CommandName="Sort"><span>Group Name</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblNEWGROUP" runat="server" Text='<% # (Convert.ToString(Eval("NEWGROUP")).ToString().Length > 8) ? Eval("NEWGROUP").ToString().Substring(0,8)+"..." :  Convert.ToString(Eval("NEWGROUP")+ "&nbsp;") %>'
                                ToolTip='<%# Eval("NEWGROUP") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="9%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span class="white_co">Edit</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                    Text="Edit"></asp:LinkButton>
                            </span>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <span>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                                    Text="Update"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text="Cancel"></asp:LinkButton>
                            </span>
                        </EditItemTemplate>
                        <ItemStyle CssClass="b_box" HorizontalAlign="Center" Width="14%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span class="white_co">Delete</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span style="height: 26px">&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="imgDelete"
                                runat="server" ImageUrl="~/Images/close.png" CommandName="DeleteRec" CommandArgument='<%# Eval("UniformIssuancePolicyItemID") %>'
                                OnClientClick="javascript:return confirm('Are you sure, you want to delete this record ?')" /></span>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" HorizontalAlign="Center" Width="3%" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="ord_header"></HeaderStyle>
            </asp:GridView>
            <div class="spacer25">
            </div>
        </div>
        <div class="bot_alert next">
            <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click">Next Step 4 </asp:LinkButton>
        </div>
        <div class="bot_alert prev">
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnAssociation" />
    <asp:HiddenField runat="server" ID="hdnAssociationName" />
    <asp:HiddenField runat="server" ID="hdnPaymentOption" />
</asp:Content>
