<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ChangeStatus.aspx.cs" Inherits="UserStatusChange_ChangeStatus" Title="Change Status"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register TagPrefix="ucMultidrp" TagName="MultiSelectDropBox" Src="~/usercontrol/MultiselectDropbox.ascx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
        // select checkboxes in grid
        function SelectAllCheckboxesSpecific(spanChk) {
            var IsChecked = spanChk.checked;
            var Chk = spanChk;
            Parent = document.getElementById('ctl00_ContentPlaceHolder1_gvUniIssuancePolicy');
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

    <style type="text/css">
        div.DT
        {
            background-color: #303030;
        }
        div.DVMain
        {
            display: none;
            overflow: auto;
            width: 100%;
            height: 200px;
            background-color: #A4A4A4;
            position: absolute;
            top: 33px;
            z-index: 3;
        }
        #DVLI
        {
            background-color: #A4A4A4;
        }
        #DVLI:hover
        {
            background-color: #D4D0C8;
        }
        .LI
        {
            background-color: transparent;
            text-decoration: none;
            color: #000;
        }
        .LI:Hover
        {
            background-color: #D4D0C8;
            text-decoration: none;
        }
        input.MSTBM
        {
            height: 24px;
            color: #F4F4F4;
            margin-top: 5px;
        }
        input.MSTBM span
        {
            display: none;
            position: absolute;
            margin-top: 5px;
            top: 20px;
            left: -10px;
            width: 175px;
            padding: 5px;
            z-index: 2;
            background: #000;
            color: #fff;
            margin: 0 0 0 3px;
            padding: 0 0 0 5px;
            -moz-border-radius: 5px; /* this works only in camino/firefox */
            -webkit-border-radius: 5px; /* this is just for Safari */
        }
        input.MSTBM:hover span
        {
            display: block;
        }
        label.LIL
        {
            font-size: 11px;
            height: 24px;
            line-height: 22px;
            color: Black;
            padding: 0 0 0 5px;
        }
    </style>
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml); $("#aspnetForm").validate(
                {
                    rules:
                    {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlEmployee: { NotequalTo: "0" }
                    },
                    messages:
                    {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Company") },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Workgroup") },
                        ctl00$ContentPlaceHolder1$ddlEmployee: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Employee") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlEmployee")
                            error.insertAfter("#dvEmployee");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCompany")
                            error.insertAfter("#dvCompany");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlEmployeeType")
                            error.insertAfter("#dvEmployeeType");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlNewWorkgroup")
                            error.insertAfter("#dvNewWorkgroup");
                        else
                            error.insertAfter(element);
                    }
                });
            });
            $("#<%=lnkBtnNext1.ClientID %>").click(function() {
                var ddlEmployeeType = document.getElementById('ctl00_ContentPlaceHolder1_ddlEmployeeType');
                if (ddlEmployeeType != null) {
                    $("#ctl00_ContentPlaceHolder1_ddlEmployeeType").rules("remove");
                }
                var ddlNewWorkgroup = document.getElementById('ctl00_ContentPlaceHolder1_ddlNewWorkgroup');
                if (ddlNewWorkgroup != null) {
                    $("#ctl00_ContentPlaceHolder1_ddlNewWorkgroup").rules("remove");
                }

                return $('#aspnetForm').valid();
            });

            $("#<%=lnkBtnNext2.ClientID %>").click(function() {

                $("#ctl00_ContentPlaceHolder1_ddlEmployeeType").rules("add", {
                    NotequalTo: "0",
                    messages: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Employee Type")
                    }
                });

                $("#ctl00_ContentPlaceHolder1_ddlNewWorkgroup").rules("add", {
                    NotequalTo: "0",
                    messages: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Workgroup")
                    }
                });

                return $('#aspnetForm').valid();
            });
        });         
        
    </script>
    <script type="text/javascript" language="javascript">

     function SHMulSel(ControlClientID, e) {

         var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'tbm');
         var divisionMain = window.document.getElementById(ControlClientID + '_' + 'divMain');
         var displayStatus = divisionMain.style.display;
         if (displayStatus == 'block') {
             divisionMain.style.display = 'none';
         }
         else {
             divisionMain.style.display = 'block';
         }
         var evt = (window.event == null) ? e : window.event;
         evt.cancelBubble = true;
     }

     function DisplayTitle(control) {
         control.title = control.value;
     }

     function SCIT(chkbox, ControlClientID) {


         var labelCollection = window.document.getElementsByTagName('label');

         var hSelectedItemsValueList = document.getElementById(ControlClientID + '_' + 'hsiv');
         var textBoxCurrentValue = new String();
         var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'tbm');
         var selectedLabelValue;

         textBoxCurrentValue = textBoxMain.value;
         var pElement = chkbox.parentElement == null ? chkbox.parentNode : chkbox.parentElement;
         var labelElement = navigator.appName == "Microsoft Internet Explorer" ? pElement.childNodes[2] : pElement.childNodes[3];
         labelValue = labelElement.innerText == null ? labelElement.textContent : labelElement.innerText;

         if (chkbox.checked) {
             textBoxCurrentValue = labelValue + ', ';
             if (hSelectedItemsValueList.value == '') {
                 hSelectedItemsValueList.value = chkbox.value + ',';
             }
             else {
                 hSelectedItemsValueList.value = hSelectedItemsValueList.value + chkbox.value + ',';
             }
             if (textBoxMain.value == '-Select-') {
                 textBoxMain.value = 'Selected';
             }
             // Set the select companies in the hidden field
             $("#ctl00_ContentPlaceHolder1_hdnListofEmployeeUniformID").val(hSelectedItemsValueList.value);

         }
         else {
             textBoxCurrentValue = textBoxCurrentValue.replace(labelValue + ',', "");
             hSelectedItemsValueList.value = hSelectedItemsValueList.value.replace(chkbox.value + ',', "");

             if (hSelectedItemsValueList.value == '') {
                 textBoxMain.value = '-Select-';
             }
             // Set the select companies in the hidden field
             $("#ctl00_ContentPlaceHolder1_hdnListofEmployeeUniformID").val(hSelectedItemsValueList.value);

         }
     }

    </script>
    <div style="text-align: center">
        <div class="spacer20">
        </div>
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
    </div>
    <div class="form_pad">
        <div class="form_table" id="dvFirst" runat="server">
            <table class="dropdown_pad ">
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">Company</span> <span class="custom-sel label-sel-small"
                                style="width: 50%;">
                                <asp:DropDownList ID="ddlCompany" runat="server" onchange="pageLoad(this,value);"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </span>
                            <div id="dvCompany">
                            </div>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">Work Group</span> <span class="custom-sel label-sel-small"
                                style="width: 50%;">
                                <asp:DropDownList ID="ddlWorkgroup" runat="server" onchange="pageLoad(this,value);"
                                    TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlWorkgroup_SelectedIndexChanged">
                                </asp:DropDownList>
                            </span>
                            <div id="dvWorkgroup">
                            </div>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">Employee</span> <span class="custom-sel label-sel-small"
                                style="width: 50%;">
                                <asp:DropDownList ID="ddlEmployee" runat="server" onchange="pageLoad(this,value);"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </span>
                            <div id="dvEmployee">
                            </div>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="spacer10">
                    </td>
                </tr>
                <tr>
                    <td class="rightalign">
                        <asp:LinkButton ID="lnkBtnNext1" class="grey2_btn" runat="server" ToolTip="Next"
                            OnClick="lnkBtnNext1_Click"><span>Next</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <%--Second Div--%>
        <div class="form_table" id="dvSecond" runat="server" visible="false">
            <table class="dropdown_pad ">
                <tr>
                    <td colspan="2">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">Employee</span>
                            <asp:TextBox ID="txtEmployee" runat="server" CssClass="w_label" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">New Work Group</span> <span class="custom-sel label-sel-small"
                                style="width: 50%;">
                                <asp:DropDownList ID="ddlNewWorkgroup" runat="server" onchange="pageLoad(this,value);"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </span>
                            <div id="dvNewWorkgroup">
                            </div>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">New Employee Uniform</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <ucMultidrp:MultiSelectDropBox ID="drpMulti" runat="server" />
                                </span>
                                <div id="dvdrpMulti">
                                </div>
                            </label>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">New Rank/Title</span> <span class="custom-sel label-sel-small"
                                style="width: 50%;">
                                <asp:DropDownList ID="ddlEmployeeType" runat="server" onchange="pageLoad(this,value);"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </span>
                            <div id="dvEmployeeType">
                            </div>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">Issuance Policy Status</span> <span
                                class="custom-sel label-sel-small" style="width: 50%;">
                                <asp:DropDownList ID="ddlIssuancePolicy" runat="server" onchange="pageLoad(this,value);">
                                    <asp:ListItem Value="C" Selected="True">Complete</asp:ListItem>
                                    <asp:ListItem Value="P">Partial</asp:ListItem>
                                </asp:DropDownList>
                            </span>
                            <div id="dvIssuancePolicy">
                            </div>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">Employee Status</span> <span class="custom-sel label-sel-small"
                                style="width: 50%;">
                                <asp:DropDownList ID="ddlEmployeeStatus" runat="server" onchange="pageLoad(this,value);"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </span>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="spacer10" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="leftalign">
                            <asp:LinkButton ID="lnkBtnBack1" class="grey2_btn" runat="server" ToolTip="Next"
                                OnClick="lnkBtnBack1_Click"><span>Back</span></asp:LinkButton>
                        </div>
                    </td>
                    <td>
                        <div class="rightalign">
                            <asp:LinkButton ID="lnkBtnNext2" class="grey2_btn" runat="server" ToolTip="Next"
                                OnClick="lnkBtnNext2_Click"><span>Next</span></asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%--Third Div--%>
        <div class="form_table" id="dvThird" runat="server" visible="false">
            <table class="dropdown_pad ">
                <tr>
                    <td colspan="2">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">Employee</span>
                            <asp:TextBox ID="txtConfEmployee" runat="server" CssClass="w_label" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">Workgroup</span>
                            <asp:TextBox ID="txtConfWorkgroup" runat="server" CssClass="w_label" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">New Employee Uniform</span>
                            <asp:TextBox ID="txtEmployeeUniform" runat="server" CssClass="w_label" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">Rank/Title</span>
                            <asp:TextBox ID="txtConfEmployeeType" runat="server" CssClass="w_label" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">Issuance Policy Status</span>
                            <asp:TextBox ID="txtConfIssuancePolicy" runat="server" CssClass="w_label" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 40%">Employee Status</span>
                            <asp:TextBox ID="txtConfEmployeeStatus" runat="server" CssClass="w_label" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                    <td class="spacer10" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td class="leftalign">
                        <asp:LinkButton ID="lnkBtnBack2" class="grey2_btn" runat="server" ToolTip="Next"
                            OnClick="lnkBtnBack2_Click"><span>Back</span></asp:LinkButton>
                    </td>
                    <td class="rightalign">
                        <asp:LinkButton ID="lnkProcess" class="grey2_btn" runat="server" ToolTip="Next" OnClick="lnkProcess_Click"><span>Process Change</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <%--New publish policies --%>
        <div class="form_table" id="dvUniformIssuancePolicies" runat="server" visible="false">
            <div style="text-align: center" id="dvPolMsg" runat="server">
                <div class="spacer20">
                </div>
                <asp:Label ID="lblmsgPolicy" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            New Issuance policies are
            <div>
                <table>
                    <tr>
                        <td class="formtd">
                            <asp:GridView ID="gvUniIssuancePolicy" runat="server" AutoGenerateColumns="false"
                                HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                RowStyle-CssClass="ord_content" OnRowDataBound="gvUniIssuancePolicy_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Check">
                                        <HeaderTemplate>
                                            <span>
                                                <asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this);"
                                                    runat="server" />
                                                &nbsp;</span>
                                            <div class="corner">
                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <span class="first">
                                                <asp:CheckBox ID="chkchild" runat="server" />&nbsp; </span>
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
                                            <span>Program Name</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblIssuanceProgramName" Text='<%#Eval("IssuanceProgramName")%>' />
                                            <%--  <span>
                                               <asp:LinkButton ID="lnkIssuanceProgramName" runat="server" Text='<%#Eval("IssuanceProgramName")%>'
                                                    CommandName="EditRec" CommandArgument='<%#Eval("UniformIssuancePolicyID")%>'></asp:LinkButton>
                                                     </span>--%>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="21%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Group Name</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblGroupName" runat="server" Text='<%#Convert.ToString(Eval("GroupName")).Length==0 ? "No Group Name" : Eval("GroupName")%>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Order Status</span>
                                            <div class="corner">
                                                <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderStatus" runat="server"/>
                                            <div class="corner">
                                                <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Policy Status</span>
                                            <div class="corner">
                                                <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnPolicyExpiryDate" runat="server" Value='<%#Eval("CreditExpireDate")%>' />
                                            <asp:Label ID="lblPolicyStatus" runat="server" />
                                            <div class="corner">
                                                <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="15%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="spacer10">
                            </div>
                            <div class="alignright" id="dvButtonControler" runat="server">
                                <asp:LinkButton ID="lnkActivate" runat="server" class="grey_btn" OnClick="lnkActivate_Click"><span>Re-Activate</span></asp:LinkButton>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    <asp:HiddenField ID="hdnListofEmployeeUniformID" runat="server" />
    </div>
</asp:Content>
