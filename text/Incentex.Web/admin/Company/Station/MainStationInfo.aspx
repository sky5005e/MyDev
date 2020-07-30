<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="MainStationInfo.aspx.cs" Inherits="admin_Company_Station_AddStation" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="uc" %>
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
    </style>

    <script language="javascript" type="text/javascript">


        $(function() {
            scrolltextarea(".scrollme", "#Scrolltop1", "#ScrollBottom1");
        });

        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'Date',
                showOn: 'button',
                buttonImage: '../../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });

        $().ready(function() {
            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {

                objValMsg = $.xml2json(xml);
                //alert(objValMsg);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtCode: { required: true, alphanumeric: true }
                    , ctl00$ContentPlaceHolder1$txtAirport: { required: true, alphanumeric: true }
                    , ctl00$ContentPlaceHolder1$txtAddress: { required: true }
                    , ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" }
                    , ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "0" }
                    , ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "0" }
                    , ctl00$ContentPlaceHolder1$txtTel: { required: true, alphanumeric: true }
                    , ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: "0" }
                    , ctl00$ContentPlaceHolder1$txtStationOperationingSince: { date: true }
                    , ctl00$ContentPlaceHolder1$txtSetupDate: { date: true }
                    , ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: true }
                    , ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: true }
                    , ctl00$ContentPlaceHolder1$txt3OperatingSince: { date: true }
                        //, ctl00$ContentPlaceHolder1$txtCostCenter: {number:true}
                        //, ctl00$ContentPlaceHolder1$txtStationId: { required: true }
                        //, ctl00$ContentPlaceHolder1$txtStationNumber: { required: true }
                    }
                    , messages:
                    {
                        ctl00$ContentPlaceHolder1$txtCode: {
                            required: replaceMessageString(objValMsg, "Required", "station code"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        }
                        , ctl00$ContentPlaceHolder1$txtAirport: {
                            required: replaceMessageString(objValMsg, "Required", "airport name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        }
                        , ctl00$ContentPlaceHolder1$txtAddress: {
                            required: replaceMessageString(objValMsg, "Required", "address")
                        }
                        , ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") }
                        , ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") }
                        , ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") }
                        , ctl00$ContentPlaceHolder1$txtTel: {
                            required: replaceMessageString(objValMsg, "Required", "tel")
                            , alphanumeric: replaceMessageString(objValMsg, "Number", "")
                        }
                        , ctl00$ContentPlaceHolder1$txtStationOperationingSince: {
                            date: replaceMessageString(objValMsg, "ValidDate", "")
                        }
                        , ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", " Status") }
                        , ctl00$ContentPlaceHolder1$txtSetupDate: {
                            date: replaceMessageString(objValMsg, "ValidDate", "")
                        },
                        ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txt3OperatingSince: { date: replaceMessageString(objValMsg, "ValidDate", "") }
                        //, ctl00$ContentPlaceHolder1$txtCostCenter: { number: replaceMessageString(objValMsg, "Number", "") }
                        //                        , ctl00$ContentPlaceHolder1$txtStationId: {
                        //                        required: replaceMessageString(objValMsg, "Required", "station id")
                        //                        }
                        //                          , ctl00$ContentPlaceHolder1$txtStationNumber: {
                        //                              required: replaceMessageString(objValMsg, "Required", "station number")
                        //                          }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlStatus")
                            error.insertAfter("#divStatus");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtStationOperationingSince")
                            error.insertAfter("#dvOpSince");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtSetupDate")
                            error.insertAfter("#dvStationStratup");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txt3OperatingSince")
                            error.insertAfter("#dvOpeatingSince");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtAddress")
                            error.insertAfter("#dvtxtAddress");
                        else
                            error.insertAfter(element);
                    }
                });
            });

            $("#<%=lnkSave.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });

        });

        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></at:ToolkitScriptManager>
--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <uc:MenuUserControl ID="manuControl" runat="server" />
    <div class="form_pad">
        <div class="form_table addedit_pad">
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <h4>
                Main Station Information</h4>
            <div class="form_label">
                Company Name:
                <%--<span>Spirit Airlines</span>--%>
                <asp:Label ID="lblCompany" runat="server"></asp:Label>
            </div>
            <div>
                <table>
                    <tr>
                        <td class="formtd_left">
                            <table>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Airport Name</span>
                                                <asp:TextBox ID="txtAirport" runat="server"></asp:TextBox>
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
                                            <div class="form_box clearfix text_applicant">
                                                <span class="input_label alignleft">Address</span>
                                                <div class="textarea_box alignright">
                                                    <div class="scrollbar">
                                                        <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                                            class="scrollbottom"></a>
                                                    </div>
                                                    <%--<textarea name="" cols="" rows="" class="scrollme"></textarea>--%>
                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="scrollme" TextMode="MultiLine"
                                                        Rows="2"></asp:TextBox>
                                                </div>
                                                <div id="dvtxtAddress">
                                                </div>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upCountry" runat="server">
                                            <ContentTemplate>
                                                <div>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Country</span>
                                                        <%--<input type="text" class="w_label" />--%>
                                                        <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                                <asp:ListItem Text="-select country-" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upCountry">
                                                            <ProgressTemplate>
                                                                <asp:Image ID="Image3" ImageUrl="~/Images/ajaxbtn.gif" runat="server" CssClass="progress_img" /></ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upState" runat="server">
                                            <ContentTemplate>
                                                <div>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">State/Province</span>
                                                        <%--<input type="text" class="w_label" />--%>
                                                        <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                                <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upState">
                                                            <ProgressTemplate>
                                                                <asp:Image ID="Image2" ImageUrl="~/Images/ajaxbtn.gif" runat="server" CssClass="progress_img" /></ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upCity" runat="server">
                                            <ContentTemplate>
                                                <div>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlCity" runat="server">
                                                                <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                        <asp:UpdateProgress ID="UpdateProgress3" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upCity">
                                                            <ProgressTemplate>
                                                                <asp:Image ID="Image1" ImageUrl="~/Images/ajaxbtn.gif" runat="server" CssClass="progress_img" /></ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Zip</span>
                                                <asp:TextBox ID="txtZip" runat="server" CssClass="w_label" MaxLength="100" TabIndex="6"></asp:TextBox>
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
                                                <span class="input_label">Tel</span>
                                                <asp:TextBox ID="txtTel" runat="server"></asp:TextBox>
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
                                                <asp:TextBox ID="txtFax" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="formtd_right">
                            <table>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Station Code</span>
                                                <asp:TextBox ID="txtCode" runat="server" CssClass="w_label"></asp:TextBox>
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
                                                <span class="input_label">Operationing Since</span> <span class="outer_dateimg">
                                                    <asp:TextBox ID="txtStationOperationingSince" runat="server" CssClass="datepicker w_label"></asp:TextBox>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                            <div id="dvOpSince">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Station Setup Date</span> <span class="outer_dateimg">
                                                    <asp:TextBox ID="txtSetupDate" runat="server" CssClass="datepicker w_label"></asp:TextBox>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                            <div id="dvStationStratup">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Station Cost Center</span>
                                                <asp:TextBox ID="txtCostCenter" runat="server"></asp:TextBox>
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
                                                <span class="input_label">Station Number</span>
                                                <asp:TextBox ID="txtStationNumber" runat="server"></asp:TextBox>
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
                                                <span class="input_label">Station ID</span>
                                                <asp:TextBox ID="txtStationId" runat="server"></asp:TextBox>
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
                                                <span class="input_label">Status</span>
                                                <label class="dropimg_width150">
                                                    <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList runat="server" ID="ddlStatus" onchange="pageLoad(this,value);">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div id="divStatus">
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
            <h4>
                3rd Party Vendor Information</h4>
            <div>
                <table>
                    <tr>
                        <td class="formtd_left">
                            <table>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Company Name</span>
                                                <asp:TextBox ID="txt3CompanyName" runat="server"></asp:TextBox>
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
                                                <span class="input_label">Corporate Contact</span>
                                                <asp:TextBox ID="txt3CorporateContact" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="formtd_right">
                            <table>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Operating Since</span> <span class="outer_dateimg">
                                                    <%--<input type="text"  class="datepicker w_label"/>--%>
                                                    <asp:TextBox ID="txt3OperatingSince" runat="server" CssClass="datepicker w_label"></asp:TextBox>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                            <div id="dvOpeatingSince">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Contract Term</span>
                                                <asp:TextBox ID="txt3ContractTerm" runat="server"></asp:TextBox>
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
            <div class="alignnone spacer25">
            </div>
            <div class="additional_btn">
                <ul class="clearfix">
                    <li>
                        <%--<a href="#" title="Save Information" class="grey2_btn"><span>Save Information</span></a>--%>
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" OnClick="lnkSave_Click">
								        <span>Save Information</span>
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
