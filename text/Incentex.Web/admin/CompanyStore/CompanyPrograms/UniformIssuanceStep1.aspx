<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="UniformIssuanceStep1.aspx.cs" Inherits="admin_CompanyStore_CompanyPrograms_UniformIssuanceStep1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />

    <script language="javascript" type="text/javascript">
        function pageLoad(sender, args) {
                assigndesign();
        }
        $().ready(function() {
            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {

                objValMsg = $.xml2json(xml);
                //alert(objValMsg);

                $("#aspnetForm").validate({
                    rules:
                    {
                        ctl00$ContentPlaceHolder1$txtProgramName: { required: true }
                        , ctl00$ContentPlaceHolder1$txtCorporateAmount: { required: true }
                        , ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: "0" }
                        , ctl00$ContentPlaceHolder1$ddlGender: { NotequalTo: "0" }

                        , ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "0" }
                        , ctl00$ContentPlaceHolder1$ddlCorporateDiscount: { NotequalTo: "0" }
                        , ctl00$ContentPlaceHolder1$ddlPriceLevel: { NotequalTo: "0" }
                        , ctl00$ContentPlaceHolder1$ddlBeforeAfter: { NotequalTo: "0" }
                    }
                     , messages:
                    {
                        ctl00$ContentPlaceHolder1$txtProgramName: { required: replaceMessageString(objValMsg, "Required", "program name") }
                        , ctl00$ContentPlaceHolder1$txtCorporateAmount: { required: replaceMessageString(objValMsg, "Required", "Corporate Amount") }
                        , ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department") }
                        , ctl00$ContentPlaceHolder1$ddlGender: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Gender") }
                        , ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "workgroup") }
                        , ctl00$ContentPlaceHolder1$ddlCorporateDiscount: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Corporate status") }
                         , ctl00$ContentPlaceHolder1$ddlPriceLevel: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "Pricing Level ") }
                         , ctl00$ContentPlaceHolder1$ddlBeforeAfter: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "Show Before/After") }

                    }, //messages


                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlGender")
                            error.insertAfter("#dvGender");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlDepartment")
                            error.insertAfter("#dvDepartment");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCorporateDiscount")
                            error.insertAfter("#dvCorporateDiscount");
                        else
                            error.insertAfter(element);
                    }

                }); //validate

            }); //get

            $("#<%=lnkNext.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });

            /*End*/

            $(function() {
                $(".datepicker").datepicker({
                    buttonText: 'Date',
                    showOn: 'button',
                    buttonImage: '../../../images/calender-icon.jpg',
                    buttonImageOnly: true
                });
            });


        });                     //ready



    </script>
 
    <script type="text/javascript" language="javascript">
        // check if value is numeric in textbox
        function CheckNum(id) {
            var txt = document.getElementById(id);
            var div = document.getElementById('dvCorporateAmount');
            if (!IsNumeric(txt.value)) {
                div.innerHTML = 'Please enter numeric value';
                txt.focus();
            }
            else {
                div.innerHTML = '';
            }

        }

        function IsNumeric(sText) {
            var ValidChars = "0123456789.";
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
        $(document).ready(function(){
            var Ctrl = $("#<%= chkApplyStationLevelApproverFlow.ClientID %>");
            ShowMOASUsers(Ctrl,"moasusersdiv");
            if(!$(Ctrl).is(":checked"))
            {
               $(Ctrl).next().toggleClass("checkbox-off");
               $(Ctrl).next().toggleClass("checkbox-on");
            }
        });
        function ShowMOASUsers(Ctrl,DivClassName)
        {
            $("." + DivClassName).toggle(!$(Ctrl).is(":checked"));
        }
    </script>

    <script language="javascript" type="text/javascript">
        function VisiblePriority(name) {
            var parent = getParentByTagName(name, "td");
            var textBoxes = parent.getElementsByTagName("input");
            for (var i = 0; i < textBoxes.length; i++) {
                if (textBoxes[i].type == "text") {
                    if (name.checked) {
                        var Mainparent = getParentByTagName(name, "table");
                        var checkBoxes = Mainparent.getElementsByTagName("input");
                        var countCheckedCheckBoxes = 0;
                        for (var j = 0; j < checkBoxes.length; j++) {
                            if (checkBoxes[j].type == "checkbox" && checkBoxes[j].checked == true) {
                                countCheckedCheckBoxes = countCheckedCheckBoxes + 1;
                            }
                        }
                        $(textBoxes[i]).show();
                        $(textBoxes[i]).val(countCheckedCheckBoxes);
                    }
                    else {
                        $(textBoxes[i]).hide();
                        $(textBoxes[i]).val('');
                    }
                }
            }
        }
        function getParentByTagName(obj, tag) {
            var obj_parent = obj.parentNode;
            if (!obj_parent) return false;
            if (obj_parent.tagName.toLowerCase() == tag) return obj_parent;
            else return getParentByTagName(obj_parent, tag);
        }

        function isNumberkey(evt) {
            var charcode = (evt.which) ? evt.which : event.keycode
            if (charcode > 31 && (charcode < 48 || charcode > 57))
                return false;

            return true;
        }
        function CheckMOASIsSelect() {
            if(!$("#<%= chkApplyStationLevelApproverFlow.ClientID %>").is(":checked"))
            {
                var MOASval = [];
                var returnVal = true;
                var dtdAdmin = document.getElementById("ctl00_ContentPlaceHolder1_dtCompanyAdmin");
                if (dtdAdmin != null) {
                    $('#ctl00_ContentPlaceHolder1_dtCompanyAdmin :checkbox:checked').each(function(i) {
                        MOASval[i] = $(this).val();
                    });

                    if (MOASval.length <= 0) {
                        alert("Please select at least one MOAS user.");
                        returnVal = false;
                    }
                    $(".allowValue").each(function(i) {
                        if ($(this).css('display') != 'none' && $(this).val() == '') {
                            alert("Please enter priority.");
                            $(this).focus();
                            returnVal = false;
                        }
                    });
                    if (returnVal == false) {
                        $("#ctl00$ContentPlaceHolder1$hdnreturnval").value = false;

                        return false;
                    }
                    else {
                        $("#ctl00$ContentPlaceHolder1$hdnreturnval").value = true;

                        return true;
                    }
                }
            }
            else
                return true;
        }
    </script>

    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            <br />
        </div>
        <h4 style="text-align: center;">
            In this step please select the department and work group you are planning for.</h4>
        <div class="uniform_pad">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="black_middle form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <span class="input_label">Program Name :</span>
                    <asp:TextBox ID="txtProgramName" runat="server" CssClass="w_label"></asp:TextBox>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="black_middle form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <span class="input_label">Gender :</span>
                    <label class="dropimg_width230">
                        <span class="label-sel-small custom-sel">
                            <asp:DropDownList ID="ddlGender" runat="server" onchange="pageLoad(this,value);">
                            </asp:DropDownList>
                        </span>
                        <div id="dvGender">
                        </div>
                    </label>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <!-- Purchase Required   -->
            <div class="black_middle form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <span class="input_label">Complete Purchase Required :</span>
                            <label class="dropimg_width272">
                                <span class="label-sel-small custom-sel">
                                    <asp:DropDownList ID="ddlPurchaseRequired" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="Div3">
                                </div>
                            </label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="black_middle form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <asp:UpdatePanel ID="upnlPricing" runat="server">
                        <ContentTemplate>
                            <span class="input_label">Pricing Level :</span>
                            <label class="dropimg_width272">
                                <span class="label-sel-small custom-sel">
                                    <asp:DropDownList ID="ddlPriceLevel" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="black_middle form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <span class="input_label">Show Price :</span>
                            <label class="dropimg_width272">
                                <span class="label-sel-small custom-sel">
                                    <asp:DropDownList ID="ddlShowPrice" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="black_middle form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                        <ContentTemplate>
                            <span class="input_label">Price for</span>
                            <label class="dropimg_width272">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlPricefor" AutoPostBack="true" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <!--department START -->
            <div class="black_middle form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <span class="input_label">Department :</span>
                    <label class="dropimg_width230">
                        <span class="custom-sel label-sel-small">
                            <asp:DropDownList ID="ddlDepartment" runat="server" onchange="pageLoad(this,value);">
                            </asp:DropDownList>
                        </span>
                        <div id="dvDepartment">
                        </div>
                    </label>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <!-- workgroup -->
            <div class="black_middle form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <span class="input_label">Workgroup :</span>
                    <label class="dropimg_width230">
                        <span class="custom-sel label-sel-small">
                            <asp:DropDownList ID="ddlWorkgroup" runat="server" onchange="pageLoad(this,value);">
                            </asp:DropDownList>
                        </span>
                        <div id="dvWorkgroup">
                        </div>
                    </label>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="black_middle form_table">
                <div class="calender_l">
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box supplier_annual_date">
                        <span class="input_label" style="width: 48%;">Policy Date</span>
                        <asp:TextBox ID="txtPolicyDate" runat="server" CssClass="w_label datepicker min_w"></asp:TextBox>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
            </div>
            <div class="black_middle form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box ">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <span class="input_label">Show Before/After</span>
                            <label class="dropimg_width272">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlBeforeAfter" AutoPostBack="true" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="black_middle form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <span class="input_label">Show Shipping Payment Address</span>
                            <label class="dropimg_width272">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlPaymentOption" AutoPostBack="true" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="black_middle form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box ">
                    <span class="input_label">Corporate Discount :</span>
                    <label class="dropimg_width230">
                        <span class="custom-sel label-sel-small">
                            <asp:DropDownList ID="ddlCorporateDiscount" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);"
                                OnSelectedIndexChanged="ddlCorporateDiscount_SelectedIndexChanged">
                                <asp:ListItem Value="0" Text="-Select-" />
                                <asp:ListItem Value="True" Text="Active" />
                                <asp:ListItem Value="False" Text="Inactive" />
                            </asp:DropDownList>
                        </span>
                        <div id="dvCorporateDiscount">
                        </div>
                    </label>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="black_middle form_table" id="dvCorporate" runat="server">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <span class="input_label">Corporate Discount Amount:</span>
                    <asp:TextBox ID="txtCorporateAmount" runat="server" CssClass="w_label" onchange="CheckNum(this.id)"></asp:TextBox>
                    <div id="dvCorporateAmount" style="color: red; display: block; font-size: 11px; margin-bottom: -5px;
                        padding-left: 55px; font-style: italic; margin-left: 31%;">
                    </div>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="black_middle form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="ddlPaymentOptionAddress" />
                        </Triggers>
                        <ContentTemplate>
                            <span class="custom-sel">
                                <asp:DropDownList ID="ddlPaymentOptionAddress" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlPaymentOptionAddress_SelectedIndexChanged">
                                </asp:DropDownList>
                            </span>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
            <div class="spacer25">
            </div>
        </div>
        <asp:Panel runat="server" ID="pnlMOAS" Style="width: 700px; margin: 0 auto;" Visible="false"
            CssClass="checktable_supplier form_table true">
            <div class="divider">
            </div>
            <h4>
                MOAS Users:</h4>
            <div>
                <span class="custom-checkbox_checked alignleft">
                    <asp:CheckBox ID="chkApplyStationLevelApproverFlow" onclick="ShowMOASUsers(this,'moasusersdiv');" runat="server" Checked="true" />
                </span>
                <span style="color:#72757C;">Apply Company/Station Level Approver</span>
            </div>
             <div class="divider">
            </div>
            <div class="moasusersdiv" style="display:none;">
                <asp:DataList RepeatColumns="4" CellSpacing="5" ID="dtCompanyAdmin" runat="server">
                    <ItemTemplate>
                        <span class="custom-checkbox alignleft" id="adminspan" runat="server">
                            <asp:CheckBox ID="chkCompanyAdmins" onclick='VisiblePriority(this);' runat="server" />
                        </span>
                        <label>
                            <asp:Label ID="lblCompanyAdmins" Text='<%# Eval("FirstName") + " " + Eval("LastName") %>'
                                runat="server"></asp:Label></label>
                        &nbsp;
                        <asp:TextBox runat="server" ID="txtApproverPriority" onkeypress="return isNumberkey(event);"
                            CssClass="allowValue" Width="12" MaxLength="1" Style="background-color: #303030;
                            float: right; display: none; padding: 0px 5px;"></asp:TextBox>
                        <asp:HiddenField ID="hdnCompanyAdmins" runat="server" Value='<%#Eval("UserInfoID")%>' />
                    </ItemTemplate>
                </asp:DataList>
            <div class="errormessage">
                Please provide priority order properly to make this functionality work correctly.
                (Avoid duplicate values)
            </div>
            </div>
        </asp:Panel>
        <div class="spacer25">
        </div>
        <div class="bot_alert next">
            <asp:LinkButton ID="lnkNext" runat="server" title="Next Step 2" OnClientClick="return CheckMOASIsSelect();"
                OnClick="lnkNext_Click">Next Step 2</asp:LinkButton>
        </div>
        <div class="bot_alert prev">
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        </div>
    </div>
    <input id="hdnPrice" type="hidden" value="0" runat="server" />
    <asp:HiddenField runat="server" ID="hdnPaymentOption" />
</asp:Content>
