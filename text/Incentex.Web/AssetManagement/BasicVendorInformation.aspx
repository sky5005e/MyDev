<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="BasicVendorInformation.aspx.cs" Inherits="AssetManagement_BasicVendorInformation"
    Title="Basic Vendor Information" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 36px;
        }
        .checklist input
        {
            float: left;
        }
        .checklist label
        {
            line-height: 10px;
            width: 100%;
            text-align: left;
            display: block;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $(function() {
            scrolltextarea(".scrollme", "#Scrolltop1", "#ScrollBottom1");

        });


        function CheckForm() {
           var CheckedIDs="";
            $(".AssociateCustomer").find("input[type=checkbox]:checked").each(function(){
                if(CheckedIDs != "")
                    CheckedIDs += ",";
                CheckedIDs += $(this).parent().siblings("input[type=hidden]").val();
            });
            $("#<%= selectedIDs.ClientID %>").val(CheckedIDs);
            return $('#aspnetForm').valid();
        }
         
     
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
        $(document).ready(function(){
           //$("#<%=dvAssociateCustomer.ClientID %>").hide();
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(GetTriggeredElement);
             $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtVendorName: { required: true, alphanumeric: true },
                         ctl00$ContentPlaceHolder1$txtAddress: { required: true },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtMobile: { alphanumeric: true },
                        ctl00$ContentPlaceHolder1$ddlBaseStation: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo:"<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "Company") },
                        ctl00$ContentPlaceHolder1$txtVendorName: {
                            required: replaceMessageString(objValMsg, "Required", "Vendor Name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        },
                        ctl00$ContentPlaceHolder1$txtAddress: { required: replaceMessageString(objValMsg, "Required", "address") },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: replaceMessageString(objValMsg, "Required", "telephone"), alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtMobile: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$ddlBaseStation: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "BaseStation") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtAddress")
                            error.insertAfter("#divtxtAddress");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlBaseStation")
                            error.insertAfter("#divBaseStation");
                        else
                            error.insertAfter(element);
                    }
                });
            });
        });
         function ShowAssociateCustomer(flag)
        { 
            if(flag.toString().toLowerCase() == "false")
            {
                $("#<%=dvAssociateCustomer.ClientID %>").show();
                $("#<%=dvIsCustomer.ClientID %>").children("span").toggleClass("checkbox-off",true).toggleClass("checkbox-on",false);
            }
            else
            {
                $("#<%=dvAssociateCustomer.ClientID %>").hide();
                $("#<%=dvIsCustomer.ClientID %>").children("span").toggleClass("checkbox-off",false).toggleClass("checkbox-on",true);
            }
        }
        function GetTriggeredElement()
        {
            $(" .iscustomer_check , .iscustomer_checked ").find("input").css({ opacity: 0.0 });
            $(".iscustomer_check , .iscustomer_checked ").append("<span> </span>");
            if($(" .iscustomer_check , .iscustomer_checked ").find("input").is(":checked"))
                $(".iscustomer_check , .iscustomer_checked ").find("span").addClass("checkbox-on");
            else
                $(".iscustomer_check , .iscustomer_checked ").find("span").addClass("checkbox-off");
            $(".iscustomer_checked input").click(function() {
               if(!$(this).is(":checked"))
                    $(this).siblings("span").toggleClass("checkbox-off",true).toggleClass("checkbox-on",false);
               else
                    $(this).siblings("span").toggleClass("checkbox-off",false).toggleClass("checkbox-on",true);
            });
            ShowAssociateCustomer($("#<%= chkIsCustomer.ClientID%>").is(":checked"));
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div>
            <asp:UpdatePanel ID="up1" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="ddlCity" />
                </Triggers>
                <ContentTemplate>
                    <table class="form_table">
                        <tr>
                            <td class="formtd">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Company Name</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </span>
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
                                                <div class="form_box employeeedit_text clearfix">
                                                    <span class="input_label alignleft">Address</span>
                                                    <div class="textarea_box alignright">
                                                        <div class="scrollbar">
                                                            <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                                                class="scrollbottom"></a>
                                                        </div>
                                                        <asp:TextBox ID="txtAddress" runat="server" CssClass="scrollme" TextMode="MultiLine"
                                                            TabIndex="6"></asp:TextBox>
                                                    </div>
                                                    <div id="divtxtAddress">
                                                    </div>
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
                                                    <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCity" runat="server" TabIndex="9" onchange="pageLoad(this,value);"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                                                            <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="PnlCityOther" runat="server" visible="false">
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">City Name</span>
                                                    <asp:TextBox ID="txtCity" runat="server" TabIndex="10" CssClass="w_label"></asp:TextBox>
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
                                                    <span class="input_label">Fax</span>
                                                    <asp:TextBox ID="txtFax" runat="server" CssClass="w_label" TabIndex="14"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="formtd">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Vendor Name</span>
                                                    <asp:TextBox ID="txtVendorName" runat="server" CssClass="w_label" TabIndex="2"></asp:TextBox>
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
                                                    <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" TabIndex="7"
                                                            OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                            <asp:ListItem Text="-select country-" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
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
                                                    <span class="input_label">Zip Code</span>
                                                    <asp:TextBox ID="txtZip" runat="server" CssClass="w_label" TabIndex="11"></asp:TextBox>
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
                                                    <span class="input_label">Mobile</span>
                                                    <asp:TextBox ID="txtMobile" runat="server" CssClass="w_label" TabIndex="15"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="formtd_r">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Contact</span>
                                                    <asp:TextBox ID="txtContact" runat="server" CssClass="w_label" TabIndex="3"></asp:TextBox>
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
                                                    <span class="input_label">State/Province</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" TabIndex="8" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                            <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
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
                                                    <span class="input_label">Telephone</span>
                                                    <asp:TextBox ID="txtTelephone" runat="server" CssClass="w_label" TabIndex="12"></asp:TextBox>
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
                                                    <span class="input_label">Is Customer</span>
                                                    <div class="iscustomer_checked" runat="server" id="dvIsCustomer" style="width: 40px;
                                                        height: 43px; margin-top: -35px">
                                                        <asp:CheckBox ID="chkIsCustomer" runat="server" Checked="true" onclick="ShowAssociateCustomer($(this).is(':checked'));" />
                                                    </div>
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="dvAssociateCustomer" runat="server" class="Associatecustomer">
            <div class="divider">
            </div>
            <h4>
                Associate Customer</h4>
            <asp:HiddenField ID="selectedIDs" runat="server" />
            <asp:HiddenField ID="OldeSelectedIDs" runat="server" />
            <div class="form_table">
                <table class="checktable_supplier true">
                    <tr>
                        <td style="width: 36%">
                            <table>
                                <tr>
                                    <td class="formtd">
                                        <table class="checktable_supplier true">
                                            <tr>
                                                <td>
                                                    <div style="height: 150px; overflow: auto">
                                                        <asp:DataList ID="dlAssociateCustomer" runat="server" CssClass="AssociateCustomer">
                                                            <ItemTemplate>
                                                                <span class="custom-checkbox alignleft" id="AssiciateCustomerSpan" runat="server">
                                                                    <asp:CheckBox ID="chkCustomer" runat="server" />
                                                                </span>
                                                                <asp:HiddenField ID="hdnEquipmentVendorID" runat="server" Value='<%#Eval("EquipmentVendorID")%>' />
                                                                <label>
                                                                    <asp:Label ID="lblEquipmentVendorName" Text='<%# Eval("EquipmentVendorName") %>'
                                                                        runat="server"></asp:Label></label>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                        <asp:Label ID="lblEmptyMessage" runat="server" ForeColor="Red"></asp:Label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 64%">
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="divider">
        </div>
        <h4>
            Base Station</h4>
        <div class="form_table">
            <table class="checktable_supplier">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box" style="width: 607px">
                                <span class="input_label">Base Station</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlBaseStation" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="divBaseStation">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd_r">
                    </td>
                </tr>
            </table>
        </div>
        <%--<div class="form_table">
           
            <table class="checktable_supplier true">
                
                <tr>
                    <td style="width: 36%">
                           
            <table>
                    <tr>
                        <td class="formtd">
                            <table class="checktable_supplier true">
                                <tr>
                                    <td>
                                     <div  style="height:150px;overflow:auto">
                                      
                                         </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
       
                    </td>
                    <td style="width: 64%">
                    </td>
                </tr>
            </table>
        </div>--%>
        <div class="botbtn centeralign">
            <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" TabIndex="22" OnClick="lnkSave_Click"
                OnClientClick="javascript: return CheckForm();">

								        <span>Save Information</span>
            </asp:LinkButton>
        </div>
    </div>
    <asp:HiddenField ID="hdnStatus" Value="0" runat="server" />
</asp:Content>
