<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CreateCampaign.aspx.cs" Inherits="admin_CommunicationCenter_CreateCampaign"
    Title="Communications Center" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register TagPrefix="ucMultidrp" TagName="MultiSelectDropBox" Src="~/usercontrol/MultiselectDropbox.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">

        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtCampaignName: { required: true },
                        ctl00$ContentPlaceHolder1$txtSubject: { required: true },
                        ctl00$ContentPlaceHolder1$txtFromName: { required: true },
                        ctl00$ContentPlaceHolder1$txtFromEmail: { required: true, email: true }

                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtCampaignName: { required: replaceMessageString(objValMsg, "Required", "Campaign Name") },
                        ctl00$ContentPlaceHolder1$txtSubject: { required: replaceMessageString(objValMsg, "Required", "Email Subject") },
                        ctl00$ContentPlaceHolder1$txtFromName: { required: replaceMessageString(objValMsg, "Required", "Name of employee") },
                        ctl00$ContentPlaceHolder1$txtFromEmail: { required: replaceMessageString(objValMsg, "Required", "Email of employee"),
                            email: replaceMessageString(objValMsg, "Valid", "email address")
                        }
                    },
                    errorPlacement: function(error, element) {
                        error.insertAfter(element);
                    }
                });
            });
            ////in case remove comment end
            //set link
            $("#<%=lnkNext.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });

        });

    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
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
                $("#ctl00_ContentPlaceHolder1_hdnListofExcludedCompanies").val(hSelectedItemsValueList.value);

            }
            else {
                textBoxCurrentValue = textBoxCurrentValue.replace(labelValue + ',', "");
                hSelectedItemsValueList.value = hSelectedItemsValueList.value.replace(chkbox.value + ',', "");

                if (hSelectedItemsValueList.value == '') {
                    textBoxMain.value = '-Select-';
                }
                // Set the select companies in the hidden field
                $("#ctl00_ContentPlaceHolder1_hdnListofExcludedCompanies").val(hSelectedItemsValueList.value);

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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div class="form_table">
            <table class="dropdown_pad ">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Campaign Name</span>
                                <asp:TextBox ID="txtCampaignName" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
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
                                <span class="input_label" style="width: 32%">Email Subject</span>
                                <asp:TextBox ID="txtSubject" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
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
                                <span class="input_label" style="width: 32%">Sender’s Name</span>
                                <asp:TextBox ID="txtFromName" runat="server" MaxLength="50" CssClass="w_label" Style="width: 52%;"></asp:TextBox>
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
                                <span class="input_label" style="width: 32%">Reply To Address</span>
                                <asp:TextBox ID="txtFromEmail" runat="server" MaxLength="50" CssClass="w_label" Style="width: 52%;"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trcompany" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Company</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);"
                                            OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvCompany">
                                    </div>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trExcludeCompanies" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Exclude Companies</span>
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
                        </div>
                    </td>
                </tr>
                <tr id="trdepartment" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Department</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlDepartment" runat="server" AutoPostBack="false" onchange="pageLoad(this,value);">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvDepartment">
                                    </div>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trworkgroup" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">WorkGroup</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlWorkgroup" runat="server" AutoPostBack="false" onchange="pageLoad(this,value);">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvWorkgroup">
                                    </div>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="tremployeetype" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Employee Type</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlEmployeeType" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);"
                                        OnSelectedIndexChanged="ddlEmployeeType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvEmployeeType">
                                    </div>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="tremployee" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Employee</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlEmployee" runat="server" AutoPostBack="false" onchange="pageLoad(this,value);">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="DivEmpList">
                                    </div>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trgender" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Gender</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlGender" runat="server" AutoPostBack="false" onchange="pageLoad(this,value);">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvGender">
                                    </div>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trcountry" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Country</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlCountry" runat="server" onchange="pageLoad(this,value);">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="Div1">
                                    </div>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trbaseStation" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Base Station</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlBaseStation" runat="server" AutoPostBack="false" onchange="pageLoad(this,value);">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvhdnBaseStation">
                                    </div>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer25">
        </div>
        <div class="bot_alert next">
            <asp:LinkButton ID="lnkNext" runat="server" title="Next Step 2" OnClick="lnkNext_Click1">Next Step 2</asp:LinkButton>
        </div>
        <div class="bot_alert prev">
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        </div>
    </div>
    <asp:HiddenField ID="hdnListofExcludedCompanies" runat="server" />
</asp:Content>
