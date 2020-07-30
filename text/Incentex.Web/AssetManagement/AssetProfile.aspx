<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AssetProfile.aspx.cs" Inherits="AssetManagement_AssetProfile" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <style type="text/css">
        .subownername
        {
            color: #72757C;
        }
        .form_popup_box span.error
        {
            color: #FF0000;
            display: block;
            font-style: italic;
            padding: 0px 0 0 0px;
            text-align: left;
        }
        .textarea_box textarea
        {
            height: 198px;
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
       var formats = 'jpg|gif|png|swf|pdf|doc';
        $().ready(function() {

            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {                        
                       
                     ctl00$ContentPlaceHolder1$txtEquipmentId: { required: true },
                      ctl00$ContentPlaceHolder1$flFile:
                     {   
                         accept: formats
                     },       
                      ctl00$ContentPlaceHolder1$txtPICCEmail: { email: true },
                       ctl00$ContentPlaceHolder1$txtInsAgentEmail: { email: true },        
                     ctl00$ContentPlaceHolder1$txtRegCmpCntEmail: { email: true }, 
                     ctl00$ContentPlaceHolder1$txtInsDate: { required: true }
                    },
                    messages: {                        
                       
                         ctl00$ContentPlaceHolder1$txtEquipmentId: { required: replaceMessageString(objValMsg, "Required", "Equipment ID") },
                          ctl00$ContentPlaceHolder1$flFile: {                            
                            accept: "<br/>" + replaceMessageString(objValMsg, "ImageType", "jpg,gif,png,pdf,txt,doc")
                        },     
                         ctl00$ContentPlaceHolder1$txtPICCEmail: {
                            email: replaceMessageString(objValMsg, "Email", "")
                        },
                        ctl00$ContentPlaceHolder1$txtInsAgentEmail: {
                            email: replaceMessageString(objValMsg, "Email", "")
                        }, 
                        ctl00$ContentPlaceHolder1$txtRegCmpCntEmail: {
                            email: replaceMessageString(objValMsg, "Email", "")
                        },                  
                        ctl00$ContentPlaceHolder1$txtInsDate: { required: replaceMessageString(objValMsg, "Required", "date") }                         
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtInsDate")
                            error.insertAfter("#dvDate");
                        else
                            error.insertAfter(element);
                    }
                });
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkBtnSaveInfo").click(function() {      
                      
                return $('#aspnetForm').valid();
            });
            
              $("#ctl00_ContentPlaceHolder1_btnSubmit").click(function() {      
                      
                return $('#aspnetForm').valid();
            });
        });
                
                
                
                
        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'Date',
                showOn: 'button',
                //dateFormat: 'dd/mm/yy',
                buttonImage: '../CSS/images/calendar_small.png',
                buttonImageOnly: true
            });
        });
        
         function DeleteConfirmation() {
         var ans =confirm("Are you sure, you want to delete selected Record?");
            if (ans)
            {              
                return true;
             }
            else
            {                
                return false;               
            }              
        }
        
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div>
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 100%">
                    <%--<div class="user_manage_btn btn_width_small">
                        <asp:LinkButton ID="lnkInsurance" class="review" title="Insurance" runat="server"
                            Width="200px">                
               <strong>             
                   Insurance               
                 </strong>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lnkRegistration" class="review" title="Registration" runat="server"
                            Width="200px">
                <span >
                <strong>
                    Registration
                </strong>
                </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lnkInspections" class="review" title="Inspections" runat="server"
                            Width="200px">
                <span >
                <strong>
                    Inspections
                </strong>
                </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lnkAssetStatus" class="review" title="Asset Status" runat="server"
                            Width="200px">
                <span >
                <strong>
                    Asset Status
                </strong>
                </span>
                        </asp:LinkButton>
                    </div>--%>
                    <ul class="clearfix nav">
                        <li runat="server" id="liInsurance"><a href="#">Insurance</a></li>
                        <li class="green_check" runat="server" id="liRegistration"><a href="#">Registration</a></li>
                        <li class="green_check" runat="server" id="liInspections"><a href="#">Inspections</a></li>
                        <li class="green_check last" runat="server" id="liAssetStatus"><a href="#">Asset Status</a></li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
    <div class="black_middle order_detail_pad" style="margin-left: 50px; margin-right: 50px;">
        <table class="order_detail" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="font-size: 15px; width: 70%">
                    <label>
                        Asset Type :
                    </label>
                    <asp:Label ID="lblAssetType" runat="server"></asp:Label>
                </td>
                <td style="font-size: 15px; width: 30%">
                    <label>
                        Asset ID :
                    </label>
                    <asp:Label ID="lblAssetID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 15px">
                    <label>
                        Brand :
                    </label>
                    <asp:Label ID="lblBrand" runat="server"></asp:Label>
                </td>
                <td style="font-size: 15px">
                    <label>
                        Serial No. :
                    </label>
                    <asp:Label ID="lblSerialNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 15px">
                    <label>
                        Year :
                    </label>
                    <asp:Label ID="lblYear" runat="server"></asp:Label>
                </td>
                <td style="font-size: 15px">
                    <label>
                        Fuel :
                    </label>
                    <asp:Label ID="lblFuel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 15px">
                    <label>
                        Base Station :
                    </label>
                    <asp:Label ID="lblBaseStation" runat="server" />
                </td>
                <td style="font-size: 15px">
                    <label>
                        Hours :
                    </label>
                    <asp:Label ID="lblHours" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 15px">
                    <label>
                        Status :
                    </label>
                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                </td>
                <td style="font-size: 15px">
                    <label>
                        Last Inspection on :
                    </label>
                    <asp:Label ID="lblLastInspection" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td style="width: 97%">
                    <at:ModalPopupExtender ID="ModalEdit" TargetControlID="lnkEditAsset" BackgroundCssClass="modalBackground"
                        DropShadow="true" runat="server" PopupControlID="pnlEditAsset" CancelControlID="PopupClose">
                    </at:ModalPopupExtender>
                    <asp:Panel ID="pnlEditAsset" runat="server" Style="display: none;">
                        <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                            left: 35%; top: 30%;">
                            <div class="pp_top" style="">
                                <div class="pp_left">
                                </div>
                                <div class="pp_middle">
                                </div>
                                <div class="pp_right">
                                </div>
                            </div>
                            <div class="pp_content_container" style="">
                                <div class="pp_left" style="">
                                    <div class="pp_right" style="">
                                        <div class="pp_content" style="height: 400px; display: block;">
                                            <div class="pp_loaderIcon" style="display: none;">
                                            </div>
                                            <div class="pp_fade" style="display: block;">
                                                <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                <div class="pp_hoverContainer" style="height: 100px; width: 500px; display: none;">
                                                    <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                        style="visibility: visible;">previous</a>
                                                </div>
                                                <div id="Div1">
                                                    <div class="pp_inline clearfix">
                                                        <div class="form_popup_box">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Equipment Type</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:DropDownList ID="ddlEquipmentType" Width="170px" runat="server" onchange="pageLoad(this,value);">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Brand</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:DropDownList ID="ddlBrand" runat="server" Width="170px" onchange="pageLoad(this,value);">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Serial Number</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtSerialNo" Width="170px" runat="server" MaxLength="10" CssClass="w_label"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Year</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtModelYear" Width="170px" runat="server" MaxLength="4" CssClass="w_label"></asp:TextBox>
                                                                            <at:FilteredTextBoxExtender ID="flttxtModelYear" runat="server" TargetControlID="txtModelYear"
                                                                                FilterType="Numbers">
                                                                            </at:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Fuel Type</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:DropDownList ID="ddlFuelType" runat="server" Width="170px" onchange="pageLoad(this,value);">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Equipment ID</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtEquipmentId" Width="170px" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Base Station</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:DropDownList ID="ddlBaseStation" runat="server" Width="170px" onchange="pageLoad(this,value);">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Hours</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtHours" Width="170px" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label" style="vertical-align: bottom">Last Inspection</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <span>
                                                                                <asp:TextBox ID="txtLastInspection" Width="140px" runat="server" CssClass="w_label datepicker min_w"
                                                                                    TabIndex="1">
                                                                                </asp:TextBox></span>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Status</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:DropDownList ID="ddlStatus" Width="170px" runat="server" CssClass="w_label">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="centeralign" colspan="2" style="padding-top: 10px">
                                                                        <asp:LinkButton ID="lnkBtnSaveInfo" runat="server" class="grey2_btn" OnClick="lnkBtnSaveInfo_Click"
                                                                            ToolTip="Save Basic Information"><span>Save Information</span></asp:LinkButton>
                                                                        <%--<a href="#" class="grey_btn" title="Save Information"><span>Save Information</span></a>--%>
                                                                        <div class="pp_details clearfix">
                                                                            <a id="PopupClose" runat="server" class="pp_close" href="#">Close</a>
                                                                            <p class="pp_description" style="display: none;">
                                                                            </p>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="pp_bottom" style="">
                                <div class="pp_left" style="">
                                </div>
                                <div class="pp_middle" style="">
                                </div>
                                <div class="pp_right" style="">
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </td>
                <td align="right" style="width: 3%">
                    <asp:LinkButton ID="lnkEditAsset" runat="server" CssClass="subownername"><span>Edit</span></asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <div class="spacer25">
    </div>
    <div class="order_detail_pad" style="margin-left: 40px; margin-right: 8px;">
        <table width="100%">
            <tr>
                <td>
                    <div class="form_table">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="custom-sel">
                                <asp:DropDownList ID="ddlShowMe" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlShowMe_SelectedIndexChanged"
                                    onchange="pageLoad(this,value);">
                                </asp:DropDownList>
                            </span>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </td>
                <td width="30%">
                </td>
                <td width="30%">
                    <div class="btn_width_small">
                        <asp:LinkButton ID="lnkFlagAsset" class="gredient_btnMainPage" title="Flag Asset"
                            runat="server" OnClick="lnkFlagAsset_Click">
                <img src="../admin/Incentex_Used_Icons/FlagAsset.png" alt="World-Link System Control" />
                <span>               
                    Flag Asset                
                </span>
                        </asp:LinkButton>
                    </div>
                </td>
            </tr>
            <%-- <tr>
                <td width="40%">
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box">
                        <span class="input_label"></span>
                        <label class="dropimg_width">
                            <span class="custom-sel">
                               
                            </span>
                        </label>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </td>
                
            </tr>--%>
        </table>
    </div>
    <div class="spacer25">
    </div>
    <div runat="server" id="dvInsuranceHide">
        <div style="width: 861px; margin: 0 auto;">
            <div class="header_bg" runat="server" id="dvInsuranceRecords">
                <div class="header_bgr" id="divheder" runat="server">
                    <span class="title alignleft" runat="server" id="spantitle" style="width: 36%;">
                        <asp:Label ID="lblHeading" runat="server" Text="Insurance Records"></asp:Label>
                    </span>
                    <%--<a href="~/login.aspx"
                                        class="grey_btn alignright" title="Return to Login Page"><span>Return to Login Page</span></a>--%>
                    <div class="alignnone">
                        &nbsp;</div>
                </div>
            </div>
            <div class="alignnone">
                &nbsp;</div>
        </div>
        <div class="spacer25">
        </div>
        <div runat="server" id="dvInsurance" class="black_middle order_detail_pad" style="margin-left: 50px;
            margin-right: 50px">
            <table class="order_detail" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="font-size: 15px; width: 70%">
                        <label>
                            Insurance Company :
                        </label>
                        <asp:Label ID="lblInsuranceCompany" runat="server"></asp:Label>
                    </td>
                    <td style="font-size: 15px; width: 30%">
                        <label>
                            Expires :
                        </label>
                        <asp:Label ID="lblExpires" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 15px">
                        <label>
                            Policy No :
                        </label>
                        <asp:Label ID="lblPolicyNumber" runat="server"></asp:Label>
                    </td>
                    <td style="font-size: 15px">
                        <label>
                            Asset Vin No :
                        </label>
                        <asp:Label ID="lblAssetVinNumber" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 15px">
                        <label>
                            Asset Plate No :
                        </label>
                        <asp:Label ID="lblAssetPlateNumber" runat="server" />
                    </td>
                    <td style="font-size: 15px">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="width: 97%">
                        <at:ModalPopupExtender ID="ModalEditInsuranceCompany" TargetControlID="lnkEditInsuranceCompany"
                            BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlEditInsuranceCompany"
                            CancelControlID="PopupCloseInsuranceCompany">
                        </at:ModalPopupExtender>
                        <asp:Panel ID="pnlEditInsuranceCompany" runat="server" Style="display: none;">
                            <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                left: 35%; top: 30%;">
                                <div class="pp_top" style="">
                                    <div class="pp_left">
                                    </div>
                                    <div class="pp_middle">
                                    </div>
                                    <div class="pp_right">
                                    </div>
                                </div>
                                <div class="pp_content_container" style="">
                                    <div class="pp_left" style="">
                                        <div class="pp_right" style="">
                                            <div class="pp_content" style="height: 280px; display: block;">
                                                <div class="pp_loaderIcon" style="display: none;">
                                                </div>
                                                <div class="pp_fade" style="display: block;">
                                                    <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                    <div class="pp_hoverContainer" style="height: 80px; width: 500px; display: none;">
                                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                            style="visibility: visible;">previous</a>
                                                    </div>
                                                    <div id="Div4">
                                                        <div class="pp_inline clearfix">
                                                            <div class="form_popup_box">
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:Label ID="Label1" runat="server" CssClass="errormessage"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Insurance Company</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtInsuranceCompany" Width="170px" runat="server" MaxLength="100"
                                                                                    CssClass="w_label"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Policy Number</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtPolicynumber" Width="170px" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Asset Vin Number</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtAssetVinNumber" Width="170px" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Asset Plate Number</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtAssetPlateNumber" Width="170px" runat="server" MaxLength="20"
                                                                                    CssClass="w_label"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <%-- <tr>
                                                                    <td>
                                                                        <span class="input_label" style="vertical-align: bottom">Expires</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar" >
                                                                            <span>
                                                                                <asp:TextBox ID="txtExpires"  Width="140px" runat="server" CssClass="w_label datepicker min_w"
                                                                                    TabIndex="1">
                                                                                </asp:TextBox></span>
                                                                        </div>
                                                                    </td>
                                                                </tr>--%>
                                                                    <tr>
                                                                        <td class="centeralign" colspan="2" style="padding-top: 20px">
                                                                            <asp:LinkButton ID="lnkBtnSaveInsuranceCompany" runat="server" class="grey2_btn"
                                                                                ToolTip="Save Basic Information" OnClick="lnkBtnSaveInsuranceCompany_Click"><span>Save Information</span></asp:LinkButton>
                                                                            <%--<a href="#" class="grey_btn" title="Save Information"><span>Save Information</span></a>--%>
                                                                            <div class="pp_details clearfix">
                                                                                <a id="PopupCloseInsuranceCompany" runat="server" class="pp_close" href="#">Close</a>
                                                                                <p class="pp_description" style="display: none;">
                                                                                </p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="pp_bottom" style="">
                                    <div class="pp_left" style="">
                                    </div>
                                    <div class="pp_middle" style="">
                                    </div>
                                    <div class="pp_right" style="">
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </td>
                    <td style="width: 3%" align="right">
                        <asp:LinkButton ID="lnkEditInsuranceCompany" runat="server" CssClass="subownername"><span>Edit</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer25">
        </div>
        <div runat="server" id="dvInspectionHeader" style="margin-left: 50px; margin-right: 50px;
            margin-bottom: 10px">
            <table class="order_detail" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="font-size: 15px; width: 68%">
                        <label>
                            Our Internal Company Contact:
                        </label>
                    </td>
                    <td style="font-size: 15px; width: 32%">
                        <label>
                            Our Insurance Agent:
                        </label>
                    </td>
                </tr>
            </table>
        </div>
        <div runat="server" id="dvInspection" class="black_middle order_detail_pad" style="margin-left: 50px;
            margin-right: 50px;">
            <table class="order_detail" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="font-size: 15px; width: 70%">
                        <label>
                            Name :
                        </label>
                        <asp:Label ID="lblICContactName" runat="server"></asp:Label>
                    </td>
                    <td style="font-size: 15px; width: 30%">
                        <label>
                            Company :
                        </label>
                        <asp:Label ID="lblInsAgentCompany" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 15px">
                        <label>
                            Telephone :
                        </label>
                        <asp:Label ID="lblICContactTelephone" runat="server"></asp:Label>
                    </td>
                    <td style="font-size: 15px">
                        <label>
                            Name :
                        </label>
                        <asp:Label ID="lblInsAgentName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 15px">
                        <label>
                            Email :
                        </label>
                        <asp:Label ID="lblICContactEmail" runat="server"></asp:Label>
                    </td>
                    <td style="font-size: 15px">
                        <label>
                            Telephone :
                        </label>
                        <asp:Label ID="lblInsAgentTelephone" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 15px">
                        &nbsp;
                    </td>
                    <td style="font-size: 15px">
                        <label>
                            Email :
                        </label>
                        <asp:Label ID="lblInsAgentEmail" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="width: 97%">
                        <at:ModalPopupExtender ID="ModalEditInternalContacts" TargetControlID="lnkEditInternalContacts"
                            BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlEditInternalContacts"
                            CancelControlID="PopupCloseEditInternalContacts">
                        </at:ModalPopupExtender>
                        <asp:Panel ID="pnlEditInternalContacts" runat="server" Style="display: none;">
                            <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                left: 35%; top: 30%;">
                                <div class="pp_top" style="">
                                    <div class="pp_left">
                                    </div>
                                    <div class="pp_middle">
                                    </div>
                                    <div class="pp_right">
                                    </div>
                                </div>
                                <div class="pp_content_container" style="">
                                    <div class="pp_left" style="">
                                        <div class="pp_right" style="">
                                            <div class="pp_content" style="height: 350px; display: block;">
                                                <div class="pp_loaderIcon" style="display: none;">
                                                </div>
                                                <div class="pp_fade" style="display: block;">
                                                    <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                    <div class="pp_hoverContainer" style="height: 100px; width: 400px; display: none;">
                                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                            style="visibility: visible;">previous</a>
                                                    </div>
                                                    <div id="Div5">
                                                        <div class="pp_inline clearfix">
                                                            <div class="form_popup_box">
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:Label ID="Label2" runat="server" CssClass="errormessage"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <strong>Our Internal Company Contact</strong>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Name</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtPICCName" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Telephone</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtPICCTelephone" Width="170px" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
                                                                                <at:FilteredTextBoxExtender ID="ftxtPICCTelephone" TargetControlID="txtPICCTelephone"
                                                                                    runat="server" FilterType="Custom,Numbers" ValidChars="-">
                                                                                </at:FilteredTextBoxExtender>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Email</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtPICCEmail" Width="170px" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <strong>Our Insurance Agent </strong>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Company</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtInsAgentCompany" Width="170px" runat="server" MaxLength="100"
                                                                                    CssClass="w_label"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Name</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtInsAgentName" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label" style="vertical-align: bottom">Telephone</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar" style="vertical-align: top">
                                                                                <span>
                                                                                    <asp:TextBox ID="txtInsAgentTelephone" Width="170px" runat="server" MaxLength="20"
                                                                                        CssClass="w_label"></asp:TextBox>
                                                                                    <at:FilteredTextBoxExtender ID="ftxtInsAgentTelephone" TargetControlID="txtInsAgentTelephone"
                                                                                        runat="server" FilterType="Custom,Numbers" ValidChars="-">
                                                                                    </at:FilteredTextBoxExtender>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label" style="vertical-align: bottom">Email</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar" style="vertical-align: top">
                                                                                <span>
                                                                                    <asp:TextBox ID="txtInsAgentEmail" Width="170px" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="centeralign" colspan="2" style="padding-top: 20px">
                                                                            <asp:LinkButton ID="lnkBtnSaveInternalContacts" runat="server" class="grey2_btn"
                                                                                ToolTip="Save Basic Information" OnClick="lnkBtnSaveInternalContacts_Click"><span>Save Information</span></asp:LinkButton>
                                                                            <%--<a href="#" class="grey_btn" title="Save Information"><span>Save Information</span></a>--%>
                                                                            <div class="pp_details clearfix">
                                                                                <a id="PopupCloseEditInternalContacts" runat="server" class="pp_close" href="#">Close</a>
                                                                                <p class="pp_description" style="display: none;">
                                                                                </p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="pp_bottom" style="">
                                    <div class="pp_left" style="">
                                    </div>
                                    <div class="pp_middle" style="">
                                    </div>
                                    <div class="pp_right" style="">
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </td>
                    <td style="width: 3%" align="right">
                        <asp:LinkButton ID="lnkEditInternalContacts" runat="server" CssClass="subownername"><span>Edit</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer25">
        </div>
        <div runat="server" id="dvManageInsurance" class="order_detail_pad" style="margin-left: 40px;
            margin-right: 8px;">
            <div style="text-align: center;">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="width: 70%">
                            <div class="btn_width_small">
                                <asp:LinkButton ID="lnkInsurancePolicy" class="gredient_btnMainPage" title="Insurance Policy"
                                    runat="server" OnClick="lnkInsurancePolicy_Click">
                <img src="../admin/Incentex_Used_Icons/InsurancePolicy.png" alt="World-Link System Control" />
                <span>               
                    Insurance Policy                
                </span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnkInsurancePolicyDummy" runat="server" Style="display: none;"></asp:LinkButton>
                            </div>
                            <div>
                                <at:ModalPopupExtender ID="ModalInsurancePolicy" TargetControlID="lnkInsurancePolicyDummy"
                                    BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlInsurancePolicy"
                                    CancelControlID="closepopupInsurancePolicy">
                                </at:ModalPopupExtender>
                                <asp:Panel ID="pnlInsurancePolicy" runat="server" Style="display: none;">
                                    <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                        left: 35%; top: 30%;">
                                        <div class="pp_top" style="">
                                            <div class="pp_left">
                                            </div>
                                            <div class="pp_middle">
                                            </div>
                                            <div class="pp_right">
                                            </div>
                                        </div>
                                        <div class="pp_content_container" style="">
                                            <div class="pp_left" style="">
                                                <div class="pp_right" style="">
                                                    <div class="pp_content" style="height: 228px; display: block;">
                                                        <div class="pp_loaderIcon" style="display: none;">
                                                        </div>
                                                        <div class="pp_fade" style="display: block;">
                                                            <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                            <div class="pp_hoverContainer" style="height: 92px; width: 370px; display: none;">
                                                                <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                                    style="visibility: visible;">previous</a>
                                                            </div>
                                                            <div id="Div12">
                                                                <div class="pp_inline clearfix">
                                                                    <div class="form_popup_box">
                                                                        <div class="label_bar">
                                                                            <asp:Label ID="Label5" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                                        </div>
                                                                        <table cellpadding="5" cellspacing="5">
                                                                            <tr>
                                                                                <td>
                                                                                    <span class="input_label">Send Mail To:<br />
                                                                                        Comma(,)Separated</span>
                                                                                </td>
                                                                                <td>
                                                                                    <div class="label_bar">
                                                                                        <asp:TextBox ID="txtSendMailTo" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <span class="input_label">Message:</span>
                                                                                </td>
                                                                                <td>
                                                                                    <div class="label_bar">
                                                                                        <asp:TextBox ID="txtMessage" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <div class="label_bar btn_padinn" style="margin-left: 10px;">
                                                                                        <asp:LinkButton ID="btnSendInsurancePolicy" CssClass="grey2_btn" runat="server" OnClick="btnSendInsurancePolicy_Click"><span>Send Mail</span></asp:LinkButton>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="pp_details clearfix" style="width: 371px;">
                                                                <a href="#" id="closepopupInsurancePolicy" runat="server" class="pp_close">Close</a>
                                                                <p class="pp_description" style="display: none;">
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="pp_bottom" style="">
                                            <div class="pp_left" style="">
                                            </div>
                                            <div class="pp_middle" style="">
                                            </div>
                                            <div class="pp_right" style="">
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </td>
                        <td style="width: 30%">
                            <div class="btn_width_small">
                                <asp:LinkButton ID="lnkAddEditInsurancePolicy" class="gredient_btnMainPage" title="Add/Edit Insurance Policy"
                                    runat="server">
                <img src="../admin/Incentex_Used_Icons/AddEdit Insurance Policy.png" alt="World-Link System Control" />
                <span>               
                    Add Edit Insurance Policy                
                </span>
                                </asp:LinkButton>
                                <at:ModalPopupExtender ID="modal" TargetControlID="lnkAddEditInsurancePolicy" BackgroundCssClass="modalBackground"
                                    DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="closepopup">
                                </at:ModalPopupExtender>
                                <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
                                    <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                        left: 35%; top: 30%;">
                                        <div class="pp_top" style="">
                                            <div class="pp_left">
                                            </div>
                                            <div class="pp_middle">
                                            </div>
                                            <div class="pp_right">
                                            </div>
                                        </div>
                                        <div class="pp_content_container" style="">
                                            <div class="pp_left" style="">
                                                <div class="pp_right" style="">
                                                    <div class="pp_content" style="height: 228px; display: block;">
                                                        <div class="pp_loaderIcon" style="display: none;">
                                                        </div>
                                                        <div class="pp_fade" style="display: block;">
                                                            <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                            <div class="pp_hoverContainer" style="height: 92px; width: 370px; display: none;">
                                                                <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                                    style="visibility: visible;">previous</a>
                                                            </div>
                                                            <div id="pp_full_res">
                                                                <div class="pp_inline clearfix">
                                                                    <div class="form_popup_box">
                                                                        <div class="label_bar">
                                                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                                        </div>
                                                                        <table cellpadding="5" cellspacing="5">
                                                                            <tr>
                                                                                <td>
                                                                                    <label>
                                                                                        Date :</label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtInsDate" runat="server" CssClass="w_label datepicker min_w"></asp:TextBox>
                                                                                    <div id="dvDate">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                    <div id="dvInsurancePolicyHide" runat="server">
                                                                                        <asp:Label ID="lblInsurancePolicyName" runat="server"></asp:Label>
                                                                                        <asp:Button ID="btnDeleteInsurancePolicy" Text="Delete" runat="server" OnClick="btnDeleteInsurancePolicy_Click"
                                                                                            CausesValidation="false" />
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <label>
                                                                                        Policy :</label>
                                                                                </td>
                                                                                <td>
                                                                                    <input type="file" id="flFile" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <div class="label_bar btn_padinn" style="margin-left: 10px;">
                                                                                        <asp:LinkButton ID="btnSubmit" CssClass="grey2_btn" runat="server" OnClick="btnSubmit_Click"><span>ADD</span></asp:LinkButton>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="pp_details clearfix" style="width: 371px;">
                                                                <a href="#" id="closepopup" runat="server" class="pp_close">Close</a>
                                                                <p class="pp_description" style="display: none;">
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="pp_bottom" style="">
                                            <div class="pp_left" style="">
                                            </div>
                                            <div class="pp_middle" style="">
                                            </div>
                                            <div class="pp_right" style="">
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%--Maintenance Cost Start--%>
    <div runat="server" id="dvMaintenanceCostHide" style="margin-left: 50px; margin-right: 50px;">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <asp:LinkButton ID="lnkPostInvoice" runat="server" CssClass="grey2_btn" OnClick="lnkPostInvoice_Click"><span>Post Invoice</span></asp:LinkButton>
                    <asp:LinkButton ID="lnkPostInvoiceDummy" runat="server" CssClass="grey2_btn" Style="display: none;"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="spacer20">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <div>
                            <div style="text-align: center; color: Red; font-size: larger;">
                                <asp:Label ID="lblMsgGrid" runat="server">
                                </asp:Label>
                            </div>
                            <div>
                                <asp:GridView ID="gvEquipment" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvEquipment_RowDataBound"
                                    OnRowCommand="gvEquipment_RowCommand" ShowFooter="True">
                                    <Columns>
                                        <asp:TemplateField Visible="False" HeaderText="EquipmentMaintenanceCostID">
                                            <HeaderTemplate>
                                                <span>EquipmentMaintenanceCostID</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEquipmentID" Text='<%# Eval("EquipmentMaintenanceCostID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False" HeaderText="EquipmentMasterID">
                                            <HeaderTemplate>
                                                <span>EquipmentMasterID</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEquipmentMasterID" Text='<%# Eval("EquipmentMasterID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtVendor" runat="server" CommandArgument="Vendor" CommandName="Sort"><span>Vendor</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderVendor" runat="server"></asp:PlaceHolder>
                                                <div class="corner">
                                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl centeralign"></span>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfVendor" runat="server" Value='<%# Eval("Vendor")%>' />
                                                <%--<asp:Label runat="server" ID="lblVendor" Text='<%# "&nbsp;" + Eval("Vendor") %>'></asp:Label>--%>
                                                <asp:LinkButton ID="lnkVendor" CommandArgument='<%# Eval("EquipmentMaintenanceCostID") %>'
                                                    CommandName="Vendor" runat="server">
                                         <span class="first"><%# "&nbsp;" + Eval("EquipmentVendorName")%></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="DateofService">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnDateofService" runat="server" CommandArgument="DateofService"
                                                    CommandName="Sort"><span >Date of Service</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderDateofService" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfDateofService" runat="server" Value='<%# Eval("DateofService")%>' />
                                                <asp:Label runat="server" ID="lblDateofService" Text='<%# "&nbsp;" + Eval("DateofService", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Invoice">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnInvoice" runat="server" CommandArgument="Invoice" CommandName="Sort"><span >Invoice #</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderInvoice" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfInvoice" runat="server" Value='<%# Eval("Invoice")%>' />
                                                <asp:Label runat="server" ID="lblInvoice" Text='<%# "&nbsp;" + Eval("Invoice")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="10%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField SortExpression="Description">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnDescription" runat="server" CommandArgument="Status" CommandName="Sort"><span >Description</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderDescription" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfDescription" runat="server" Value='<%# Eval("Description")%>' />
                                                <asp:Label runat="server" ID="lblDescription" ToolTip='<%# Eval("Description")%>'
                                                    Text='<%# Convert.ToString(Eval("Description")).Length > 20 ? Convert.ToString(Eval("Description")).Substring(0, 20).Trim() + "..." : "&nbsp;" + Convert.ToString(Eval("Description")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="20%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField SortExpression="JobCodeName">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnJobCodeName" runat="server" CommandArgument="JobCodeName"
                                                    CommandName="Sort"><span >Job Code</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderJobCodeName" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfJobCodeName" runat="server" Value='<%# Eval("JobCodeName")%>' />
                                                <asp:Label runat="server" ID="lblJobCodeName" Text='<%# Eval("JobCodeName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="LaborAmount">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnLaborAmount" runat="server" CommandArgument="LaborAmount" CommandName="Sort"><span >Labor Amt</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderLaborAmount" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfLaborAmount" runat="server" Value='<%# Eval("LaborAmount")%>' />
                                                <asp:Label runat="server" ID="lblLaborAmount" Text='<%# "&nbsp;" +  Eval("LaborAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box rightalign" Width="10%" />                                           
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="PartsAmount">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnPartsAmount" runat="server" CommandArgument="PartsAmount" CommandName="Sort"><span >Parts Amt</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderPartsAmount" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfPartsAmount" runat="server" Value='<%# Eval("PartsAmount")%>' />
                                                <asp:Label runat="server" ID="lblPartsAmount" Text='<%# "&nbsp;" +  Eval("PartsAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box rightalign" Width="10%" />                                           
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Amount">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnAmount" runat="server" CommandArgument="Amount" CommandName="Sort"><span >Total Invoice Amt</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderAmount" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfAmount" runat="server" Value='<%# Eval("Amount")%>' />
                                                <asp:Label runat="server" ID="lblAmount" Text='<%# Eval("Amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box rightalign" Width="15%" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblShow" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Total: $"></asp:Label>
                                                <asp:Label ID="lblTotalAmount" runat="server" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <span class="centeralign">File</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span class="btn_space">
                                                    <asp:ImageButton ID="lnkbtnPDF" runat="server" CommandName="PDF" CommandArgument='<%# Eval("EquipmentMaintenanceCostID") %>' /></span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <span class="centeralign">Delete</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span class="btn_space">
                                                    <asp:ImageButton ID="lnkbtndelete" runat="server" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                                        CommandArgument='<%# Eval("EquipmentMaintenanceCostID") %>' ImageUrl="~/Images/close.png" /></span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div>
                            <div class="companylist_botbtn alignleft">
                            </div>
                            <div id="pagingtable" runat="server" class="alignright pagging">
                                <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                                </asp:LinkButton>
                                <span>
                                    <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:DataList></span>
                                <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td style="width: 97%">
                    <at:ModalPopupExtender ID="ModalPostInvoice" TargetControlID="lnkPostInvoiceDummy"
                        BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlPostInvoice"
                        CancelControlID="PopupClosePostInvoice">
                    </at:ModalPopupExtender>
                    <asp:Panel ID="pnlPostInvoice" runat="server" Style="display: none;">
                        <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                            left: 35%; top: 30%;">
                            <div class="pp_top" style="">
                                <div class="pp_left">
                                </div>
                                <div class="pp_middle">
                                </div>
                                <div class="pp_right">
                                </div>
                            </div>
                            <div class="pp_content_container" style="">
                                <div class="pp_left" style="">
                                    <div class="pp_right" style="">
                                        <div class="pp_content" style="height: 550px; display: block;">
                                            <div class="pp_loaderIcon" style="display: none;">
                                            </div>
                                            <div class="pp_fade" style="display: block;">
                                                <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                <div class="pp_hoverContainer" style="height: 350px; width: 350px; display: none;">
                                                    <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                        style="visibility: visible;">previous</a>
                                                </div>
                                                <div id="Div6">
                                                    <div class="pp_inline clearfix">
                                                        <div class="form_popup_box">
                                                            <table>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="Label3" runat="server" CssClass="errormessage"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Vendor</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:DropDownList ID="ddlVendor" Width="170px" runat="server" CssClass="w_label">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Date of Service</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtDateofService" Width="140px" runat="server" CssClass="w_label datepicker min_w"
                                                                                TabIndex="1">
                                                                            </asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Invoice #</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtInvoice" Width="170px" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Description</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtDescription" Width="170px" runat="server" MaxLength="100" CssClass="w_label"
                                                                                TextMode="MultiLine"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Total Invoice Amt</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtAmount" Width="170px" runat="server" MaxLength="18" CssClass="w_label"></asp:TextBox>
                                                                            <at:FilteredTextBoxExtender ID="ftbAmount" TargetControlID="txtAmount" runat="server"
                                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                                            </at:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Labor Amount</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtLaborAmount" Width="170px" runat="server" MaxLength="18" CssClass="w_label"></asp:TextBox>
                                                                            <at:FilteredTextBoxExtender ID="fltLaborAmount" TargetControlID="txtLaborAmount" runat="server"
                                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                                            </at:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                  <tr>
                                                                    <td>
                                                                        <span class="input_label">Parts Amount</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtPartsAmount" Width="170px" runat="server" MaxLength="18" CssClass="w_label"></asp:TextBox>
                                                                            <at:FilteredTextBoxExtender ID="fltPartsAmount" TargetControlID="txtPartsAmount" runat="server"
                                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                                            </at:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Job Code</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:DropDownList ID="ddlJobCode" runat="server" Width="170px" OnSelectedIndexChanged="ddlJobCode_SelectedIndexChanged"
                                                                                AutoPostBack="true" onchange="pageLoad(this,value);">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Job Sub Code</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="checklist" style="height: 80px; border: 1px solid Grey; overflow: auto">
                                                                            <asp:CheckBoxList ID="cblJobSubCode" runat="server" DataTextField="JobCodeName" DataValueField="JobCodeID">
                                                                            </asp:CheckBoxList>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Document</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <input type="file" id="InvoiceDoc" runat="server" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="centeralign" colspan="2" style="padding-top: 20px">
                                                                        <asp:LinkButton ID="lnkSaveInvoice" runat="server" class="grey2_btn" OnClick="lnkSaveInvoice_Click"
                                                                            ToolTip="Save Basic Information"><span>Save Information</span></asp:LinkButton>
                                                                        <%--<a href="#" class="grey_btn" title="Save Information"><span>Save Information</span></a>--%>
                                                                        <div class="pp_details clearfix">
                                                                            <a id="PopupClosePostInvoice" runat="server" class="pp_close" href="#">Close</a>
                                                                            <p class="pp_description" style="display: none;">
                                                                            </p>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="pp_bottom" style="">
                                <div class="pp_left" style="">
                                </div>
                                <div class="pp_middle" style="">
                                </div>
                                <div class="pp_right" style="">
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <%--Maintenance Cost End--%>
    <%-- Registration Start--%>
    <div runat="server" id="dvRegistration">
        <div class="black_middle order_detail_pad" style="margin-left: 50px; margin-right: 50px">
            <table class="order_detail" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="font-size: 15px; width: 70%">
                        <label>
                            Asset Type :
                        </label>
                        <asp:Label ID="lblAssetTypeReg" runat="server"></asp:Label>
                    </td>
                    <td style="font-size: 15px; width: 30%">
                        <label>
                            Date Registered :
                        </label>
                        <asp:Label ID="lblDateRegistered" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 15px">
                        <label>
                            Asset Vin Number :
                        </label>
                        <asp:Label ID="lblAssetVinNumberReg" runat="server"></asp:Label>
                    </td>
                    <td style="font-size: 15px">
                        <label>
                            Date Expires :
                        </label>
                        <asp:Label ID="lblRegDateExpires" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 15px">
                        <label>
                            Asset Plate Number :
                        </label>
                        <asp:Label ID="lblAssetPlateNumberReg" runat="server" />
                    </td>
                    <td style="font-size: 15px">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="width: 97%">
                        <at:ModalPopupExtender ID="ModalEditRegistration" TargetControlID="lnkEditRegistration"
                            BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlEditRegistration"
                            CancelControlID="PopupCloseRegistration">
                        </at:ModalPopupExtender>
                        <asp:Panel ID="pnlEditRegistration" runat="server" Style="display: none;">
                            <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                left: 35%; top: 30%;">
                                <div class="pp_top" style="">
                                    <div class="pp_left">
                                    </div>
                                    <div class="pp_middle">
                                    </div>
                                    <div class="pp_right">
                                    </div>
                                </div>
                                <div class="pp_content_container" style="">
                                    <div class="pp_left" style="">
                                        <div class="pp_right" style="">
                                            <div class="pp_content" style="height: 250px; display: block;">
                                                <div class="pp_loaderIcon" style="display: none;">
                                                </div>
                                                <div class="pp_fade" style="display: block;">
                                                    <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                    <div class="pp_hoverContainer" style="height: 50px; width: 500px; display: none;">
                                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                            style="visibility: visible;">previous</a>
                                                    </div>
                                                    <div id="Div7">
                                                        <div class="pp_inline clearfix">
                                                            <div class="form_popup_box">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label" style="vertical-align: bottom">Date Registered</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <span>
                                                                                    <asp:TextBox ID="txtDateRegistered" Width="140px" runat="server" CssClass="w_label datepicker min_w"
                                                                                        TabIndex="1">
                                                                                    </asp:TextBox></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label" style="vertical-align: bottom">Date Expires</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <span>
                                                                                    <asp:TextBox ID="txtRegDateExpires" Width="140px" runat="server" CssClass="w_label datepicker min_w"
                                                                                        TabIndex="1">
                                                                                    </asp:TextBox></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="centeralign" colspan="2" style="padding-top: 20px">
                                                                            <asp:LinkButton ID="lnkBtnSaveRegistration" runat="server" class="grey2_btn" ToolTip="Save Basic Information"
                                                                                OnClick="lnkBtnSaveRegistration_Click"><span>Save Information</span></asp:LinkButton>
                                                                            <%--<a href="#" class="grey_btn" title="Save Information"><span>Save Information</span></a>--%>
                                                                            <div class="pp_details clearfix">
                                                                                <a id="PopupCloseRegistration" runat="server" class="pp_close" href="#">Close</a>
                                                                                <p class="pp_description" style="display: none;">
                                                                                </p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="pp_bottom" style="">
                                    <div class="pp_left" style="">
                                    </div>
                                    <div class="pp_middle" style="">
                                    </div>
                                    <div class="pp_right" style="">
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </td>
                    <td style="width: 3%" align="right">
                        <asp:LinkButton ID="lnkEditRegistration" runat="server" CssClass="subownername"><span>Edit</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer25">
        </div>
        <div runat="server" id="dvRegHeader" style="margin-left: 50px; margin-right: 50px;
            margin-bottom: 10px">
            <table class="order_detail" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="font-size: 15px; width: 68%">
                        <label>
                            Internal Company Contact:
                        </label>
                    </td>
                    <td style="font-size: 15px; width: 32%">
                        <label>
                            Registration Payment Information:
                        </label>
                    </td>
                </tr>
            </table>
        </div>
        <div runat="server" id="dvRegCmpContact" class="black_middle order_detail_pad" style="margin-left: 50px;
            margin-right: 50px;">
            <table class="order_detail" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="font-size: 15px; width: 70%">
                        <label>
                            Name :
                        </label>
                        <asp:Label ID="lblRegCmpCntName" runat="server"></asp:Label>
                    </td>
                    <td style="font-size: 15px; width: 30%">
                        <label>
                            Payment By :
                        </label>
                        <asp:Label ID="lblRegPaymentBy" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 15px">
                        <label>
                            Telephone :
                        </label>
                        <asp:Label ID="lblRegCmpCntTelephone" runat="server"></asp:Label>
                    </td>
                    <td style="font-size: 15px">
                        <label>
                            Online Payment :
                        </label>
                        <%-- <asp:Label ID="lblRegOnlinePayment" runat="server"></asp:Label>--%>
                        <asp:HyperLink ID="hlnkRegOnlinePayment" runat="server"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 15px">
                        <label>
                            Email :
                        </label>
                        <asp:Label ID="lblRegCmpCntEmail" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="width: 97%">
                        <at:ModalPopupExtender ID="ModalRegInternalContacts" TargetControlID="lnkEditRegInternalContacts"
                            BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlEditRegInternalContacts"
                            CancelControlID="PopupCloseRegInternalContacts">
                        </at:ModalPopupExtender>
                        <asp:Panel ID="pnlEditRegInternalContacts" runat="server" Style="display: none;">
                            <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                left: 35%; top: 30%;">
                                <div class="pp_top" style="">
                                    <div class="pp_left">
                                    </div>
                                    <div class="pp_middle">
                                    </div>
                                    <div class="pp_right">
                                    </div>
                                </div>
                                <div class="pp_content_container" style="">
                                    <div class="pp_left" style="">
                                        <div class="pp_right" style="">
                                            <div class="pp_content" style="height: 350px; display: block;">
                                                <div class="pp_loaderIcon" style="display: none;">
                                                </div>
                                                <div class="pp_fade" style="display: block;">
                                                    <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                    <div class="pp_hoverContainer" style="height: 100px; width: 400px; display: none;">
                                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                            style="visibility: visible;">previous</a>
                                                    </div>
                                                    <div id="Div13">
                                                        <div class="pp_inline clearfix">
                                                            <div class="form_popup_box">
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:Label ID="Label6" runat="server" CssClass="errormessage"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <strong>Internal Company Contact</strong>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Name</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtRegCmpCntName" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Telephone</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtRegCmpCntTelephone" Width="170px" runat="server" MaxLength="20"
                                                                                    CssClass="w_label"></asp:TextBox>
                                                                                <at:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtRegCmpCntTelephone"
                                                                                    runat="server" FilterType="Custom,Numbers" ValidChars="-">
                                                                                </at:FilteredTextBoxExtender>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Email</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtRegCmpCntEmail" Width="170px" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <strong>Registration Payment Information </strong>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Payment Mode</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <%-- <asp:TextBox ID="TextBox1" Width="170px" runat="server" MaxLength="100"
                                                                                    CssClass="w_label"></asp:TextBox>--%>
                                                                                <asp:RadioButton ID="rdbPaymentModeOnline" runat="server" Checked="true" GroupName="PaymentMode" />
                                                                                <span class="input_label">Pay Online</span>
                                                                                <asp:RadioButton ID="rdbPaymentModeCheck" runat="server" GroupName="PaymentMode" />
                                                                                <span class="input_label">Pay by Cheque</span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Online Payment URl</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtRegOnlinePaymentURL" Width="170px" runat="server" MaxLength="100"
                                                                                    CssClass="w_label"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="centeralign" colspan="2" style="padding-top: 20px">
                                                                            <asp:LinkButton ID="lnkBtnSaveRegInternalContacts" runat="server" class="grey2_btn"
                                                                                ToolTip="Save Basic Information" OnClick="lnkBtnSaveRegInternalContacts_Click"><span>Save Information</span></asp:LinkButton>
                                                                            <%--<a href="#" class="grey_btn" title="Save Information"><span>Save Information</span></a>--%>
                                                                            <div class="pp_details clearfix">
                                                                                <a id="PopupCloseRegInternalContacts" runat="server" class="pp_close" href="#">Close</a>
                                                                                <p class="pp_description" style="display: none;">
                                                                                </p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="pp_bottom" style="">
                                    <div class="pp_left" style="">
                                    </div>
                                    <div class="pp_middle" style="">
                                    </div>
                                    <div class="pp_right" style="">
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </td>
                    <td style="width: 3%" align="right">
                        <asp:LinkButton ID="lnkEditRegInternalContacts" runat="server" CssClass="subownername"><span>Edit</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer25">
        </div>
        <div runat="server" id="dvManageRegistration" class="order_detail_pad" style="margin-left: 40px;
            margin-right: 8px;">
            <div>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="2">
                            <div style="text-align: center; height: 10px">
                                <asp:Label ID="lblErrorRegistration" runat="server" CssClass="errormessage"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 70%">
                            <div class="btn_width_small">
                                <asp:LinkButton ID="lnkRegistrationPolicy" class="gredient_btnMainPage" title="Registration Policy"
                                    runat="server" OnClick="lnkRegistrationPolicy_Click">
                <img src="../admin/Incentex_Used_Icons/InsurancePolicy.png" alt="World-Link System Control" />
                <span>               
                    Registration                
                </span>
                                </asp:LinkButton>
                            </div>
                        </td>
                        <td style="width: 30%">
                            <div class="btn_width_small">
                                <asp:LinkButton ID="lnkAddEditRegistrationPolicy" class="gredient_btnMainPage" title="Add/Edit Registration Policy"
                                    runat="server">
                <img src="../admin/Incentex_Used_Icons/AddEdit Insurance Policy.png" alt="World-Link System Control" />
                <span>               
                    Add Edit Registration                
                </span>
                                </asp:LinkButton>
                                <at:ModalPopupExtender ID="ModalRegistration" TargetControlID="lnkAddEditRegistrationPolicy"
                                    BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlRegistration"
                                    CancelControlID="closepopupRegistration">
                                </at:ModalPopupExtender>
                                <asp:Panel ID="pnlRegistration" runat="server" Style="display: none;">
                                    <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                        left: 35%; top: 30%;">
                                        <div class="pp_top" style="">
                                            <div class="pp_left">
                                            </div>
                                            <div class="pp_middle">
                                            </div>
                                            <div class="pp_right">
                                            </div>
                                        </div>
                                        <div class="pp_content_container" style="">
                                            <div class="pp_left" style="">
                                                <div class="pp_right" style="">
                                                    <div class="pp_content" style="height: 170px; display: block;">
                                                        <div class="pp_loaderIcon" style="display: none;">
                                                        </div>
                                                        <div class="pp_fade" style="display: block;">
                                                            <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                            <div class="pp_hoverContainer" style="height: 50px; width: 370px; display: none;">
                                                                <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                                    style="visibility: visible;">previous</a>
                                                            </div>
                                                            <div id="Div8">
                                                                <div class="pp_inline clearfix">
                                                                    <div class="form_popup_box">
                                                                        <div class="label_bar">
                                                                            <asp:Label ID="lblErrorRegDoc" runat="server" CssClass="errormessage"></asp:Label>
                                                                        </div>
                                                                        <table cellpadding="5" cellspacing="5">
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <div id="dvRegDocHide" runat="server">
                                                                                        <asp:Label ID="lblRegDocName" runat="server"></asp:Label>
                                                                                        <asp:Button ID="btnDeleteRegDoc" Text="Delete" runat="server" OnClick="btnDeleteRegDoc_Click" />
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <label>
                                                                                        Registration Document :</label>
                                                                                </td>
                                                                                <td>
                                                                                    <input type="file" id="flRegDoc" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <div class="label_bar btn_padinn" style="margin-left: 10px;">
                                                                                        <asp:LinkButton ID="lnkSaveRegDoc" CssClass="grey2_btn" runat="server" OnClick="lnkSaveRegDoc_Click"><span>ADD</span></asp:LinkButton>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="pp_details clearfix" style="width: 371px;">
                                                                <a href="#" id="closepopupRegistration" runat="server" class="pp_close">Close</a>
                                                                <p class="pp_description" style="display: none;">
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="pp_bottom" style="">
                                            <div class="pp_left" style="">
                                            </div>
                                            <div class="pp_middle" style="">
                                            </div>
                                            <div class="pp_right" style="">
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="spacer25">
        </div>
    </div>
    <%-- Registration End--%>
    <%--Weekly Maintenence Start--%>
    <div runat="server" id="dvWeeklyMaintenanceHide" style="margin-left: 50px; margin-right: 50px;">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <asp:LinkButton ID="lnkPostEvent" runat="server" CssClass="grey2_btn"><span>Post Event</span></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="spacer20">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <div>
                            <div style="text-align: center; color: Red; font-size: larger;">
                                <asp:Label ID="lblPostEvent" runat="server">
                                </asp:Label>
                            </div>
                            <div>
                                <asp:GridView ID="gvPostEvent" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvPostEvent_RowDataBound"
                                    OnRowCommand="gvPostEvent_RowCommand" ShowFooter="True">
                                    <Columns>
                                        <asp:TemplateField Visible="False" HeaderText="EquipmentWeeklyMaintinanceID">
                                            <HeaderTemplate>
                                                <span>EquipmentWeeklyMaintinanceID</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEquipmentWeeklyMaintinanceID" Text='<%# Eval("EquipmentWeeklyMaintinanceID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False" HeaderText="EquipmentMasterID">
                                            <HeaderTemplate>
                                                <span>EquipmentMasterID</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEquipmentMstrID" Text='<%# Eval("EquipmentMasterID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="WeekOfDate">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnWeekOfDate" runat="server" CommandArgument="WeekOfDate"
                                                    CommandName="Sort"><span >Week Of</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderWeekOfDate" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfWeekOfDate" runat="server" Value='<%# Eval("WeekOfDate")%>' />
                                                <asp:LinkButton ID="lnkWeekOfDate" CommandArgument='<%# Eval("EquipmentWeeklyMaintinanceID") %>'
                                                    CommandName="WeekOfDate" runat="server">
                                         <span class="first"><%# "&nbsp;" + Eval("WeekOfDate", "{0:MM/dd/yyyy}") %></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Hours">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnHours" runat="server" CommandArgument="Hours" CommandName="Sort"><span >Hours</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderHours" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfHours" runat="server" Value='<%# Eval("Hours")%>' />
                                                <asp:Label runat="server" ID="lblHours" Text='<%# "&nbsp;" + Eval("Hours") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Oil">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnOil" runat="server" CommandArgument="Oil" CommandName="Sort"><span >Oil</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderOil" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfOil" runat="server" Value='<%# Eval("Oil")%>' />
                                                <asp:Label runat="server" ID="lblOil" Text='<%# "&nbsp;" + Eval("Oil")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="AntiFreeze">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnAntiFreeze" runat="server" CommandArgument="Status" CommandName="Sort"><span >Antifreeze</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderAntiFreeze" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfAntiFreeze" runat="server" Value='<%# Eval("AntiFreeze")%>' />
                                                <asp:Label runat="server" ID="lblAntiFreeze" Text='<%# "&nbsp;" + Eval("AntiFreeze") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ServiceBrake">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnServiceBrake" runat="server" CommandArgument="ServiceBrake"
                                                    CommandName="Sort"><span >Service Brakes</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderServiceBrake" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfServiceBrake" runat="server" Value='<%# Eval("ServiceBrake")%>' />
                                                <%--<asp:Label runat="server" ID="lblServiceBrake" Text='<%# "&nbsp;" + Eval("ServiceBrake") %>'></asp:Label>--%>
                                                <span class="btn_space">
                                                    <asp:Image runat="server" ID="imgServiceBrake" />
                                                </span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ParkBrake">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnParkBrake" runat="server" CommandArgument="ParkBrake" CommandName="Sort"><span >Parking Brake</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderParkBrake" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfParkBrake" runat="server" Value='<%# Eval("ParkBrake")%>' />
                                                <%--<asp:Label runat="server" ID="lblParkBrake" Text='<%# "&nbsp;" + Eval("ParkBrake") %>'></asp:Label>--%>
                                                <span class="btn_space">
                                                    <asp:Image runat="server" ID="imgParkBrake" />
                                                </span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box centeralign" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Lights">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnLights" runat="server" CommandArgument="Lights" CommandName="Sort"><span >Lights</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderLights" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfLights" runat="server" Value='<%# Eval("Lights")%>' />
                                                <%--<asp:Label runat="server" ID="lblLights" Text='<%# "&nbsp;" + Eval("Lights")%>'></asp:Label>--%>
                                                <span class="btn_space">
                                                    <asp:Image runat="server" ID="imgLights" />
                                                </span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box centeralign" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ATF">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnATF" runat="server" CommandArgument="Status" CommandName="Sort"><span >ATF</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderATF" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfATF" runat="server" Value='<%# Eval("ATF")%>' />
                                                <%--<asp:Label runat="server" ID="lblATF" Text='<%# "&nbsp;" + Eval("ATF") %>'></asp:Label>--%>
                                                <span class="btn_space">
                                                    <asp:Image runat="server" ID="imgATF" />
                                                </span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Tires">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnTires" runat="server" CommandArgument="Status" CommandName="Sort"><span >Tires</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderTires" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfTires" runat="server" Value='<%# Eval("Tires")%>' />
                                                <%--<asp:Label runat="server" ID="lblTires" Text='<%# "&nbsp;" + Eval("Tires") %>'></asp:Label>--%>
                                                <span class="btn_space">
                                                    <asp:Image runat="server" ID="imgTires" />
                                                </span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box centeralign" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <span class="centeralign">Delete</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span class="btn_space">
                                                    <asp:ImageButton ID="lnkbtndelete" runat="server" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                                        CommandArgument='<%# Eval("EquipmentWeeklyMaintinanceID") %>' ImageUrl="~/Images/close.png" /></span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <%--<div>
                            <div class="companylist_botbtn alignleft">
                            </div>
                            <div id="pagingtable" runat="server" class="alignright pagging">
                                <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                                </asp:LinkButton>
                                <span>
                                    <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:DataList></span>
                                <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                            </div>
                        </div>--%>
                    </div>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td style="width: 97%">
                    <at:ModalPopupExtender ID="ModalWeeklyMaintinance" TargetControlID="lnkPostEvent"
                        BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlWeeklyMaintinance"
                        CancelControlID="PopupCloseWeeklyMaintinance">
                    </at:ModalPopupExtender>
                    <asp:Panel ID="pnlWeeklyMaintinance" runat="server" Style="display: none;">
                        <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                            left: 35%; top: 30%;">
                            <div class="pp_top" style="">
                                <div class="pp_left">
                                </div>
                                <div class="pp_middle">
                                </div>
                                <div class="pp_right">
                                </div>
                            </div>
                            <div class="pp_content_container" style="">
                                <div class="pp_left" style="">
                                    <div class="pp_right" style="">
                                        <div class="pp_content" style="height: 430px; display: block;">
                                            <div class="pp_loaderIcon" style="display: none;">
                                            </div>
                                            <div class="pp_fade" style="display: block;">
                                                <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                <div class="pp_hoverContainer" style="height: 230px; width: 350px; display: none;">
                                                    <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                        style="visibility: visible;">previous</a>
                                                </div>
                                                <div id="Div9">
                                                    <div class="pp_inline clearfix">
                                                        <div class="form_popup_box">
                                                            <table>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="Label4" runat="server" CssClass="errormessage"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Week of</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtWeekOfDate" Width="140px" runat="server" CssClass="w_label datepicker min_w"
                                                                                TabIndex="1">
                                                                            </asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Hours</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtEventHours" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                                                                            <at:FilteredTextBoxExtender ID="ftbEventHours" TargetControlID="txtEventHours" runat="server"
                                                                                FilterType="Numbers">
                                                                            </at:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Oil</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtOil" Width="170px" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Antifreeze</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtAntiFreeze" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Service Brake</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <%--<asp:TextBox ID="txtServiceBrake" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>--%>
                                                                            <asp:RadioButton ID="rdbServiceBrakeT" runat="server" Checked="true" GroupName="ServiceBrake" />
                                                                            <span class="input_label">True</span>
                                                                            <asp:RadioButton ID="rdbServiceBrakeF" runat="server" GroupName="ServiceBrake" />
                                                                            <span class="input_label">False</span>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Park Brake</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <%--<asp:TextBox ID="txtParkBrake" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>--%>
                                                                            <asp:RadioButton ID="rdbParkBrakeT" runat="server" Checked="true" GroupName="ParkBrake" />
                                                                            <span class="input_label">True</span>
                                                                            <asp:RadioButton ID="rdbParkBrakeF" runat="server" GroupName="ParkBrake" />
                                                                            <span class="input_label">False</span>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Lights</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <%--<asp:TextBox ID="txtLights" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>--%>
                                                                            <asp:RadioButton ID="rdbLightsT" runat="server" Checked="true" GroupName="rdbLights" />
                                                                            <span class="input_label">True</span>
                                                                            <asp:RadioButton ID="rdbLightsF" runat="server" GroupName="rdbLights" />
                                                                            <span class="input_label">False</span>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">ATF</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <%--<asp:TextBox ID="txtATF" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>--%>
                                                                            <asp:RadioButton ID="rdbATFT" runat="server" Checked="true" GroupName="ATF" />
                                                                            <span class="input_label">True</span>
                                                                            <asp:RadioButton ID="rdbATFF" runat="server" GroupName="ATF" />
                                                                            <span class="input_label">False</span>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Tires</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <%--<asp:TextBox ID="txtTires" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>--%>
                                                                            <asp:RadioButton ID="rdbTiresT" runat="server" Checked="true" GroupName="Tires" />
                                                                            <span class="input_label">True</span>
                                                                            <asp:RadioButton ID="rdbTiresF" runat="server" GroupName="Tires" />
                                                                            <span class="input_label">False</span>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="centeralign" colspan="2" style="padding-top: 20px">
                                                                        <asp:LinkButton ID="lnkSaveWeeklyMaintinance" runat="server" class="grey2_btn" OnClick="lnkSaveWeeklyMaintinance_Click"
                                                                            ToolTip="Save Basic Information"><span>Save Information</span></asp:LinkButton>
                                                                        <%--<a href="#" class="grey_btn" title="Save Information"><span>Save Information</span></a>--%>
                                                                        <div class="pp_details clearfix">
                                                                            <a id="PopupCloseWeeklyMaintinance" runat="server" class="pp_close" href="#">Close</a>
                                                                            <p class="pp_description" style="display: none;">
                                                                            </p>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="pp_bottom" style="">
                                <div class="pp_left" style="">
                                </div>
                                <div class="pp_middle" style="">
                                </div>
                                <div class="pp_right" style="">
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <%--Weekly Maintenance Stop--%>
    <%--Equipment Image Start--%>
    <div runat="server" id="dvEquipmentImageHide" style="margin-left: 50px; margin-right: 50px;">
        <div>
            <table width="100%">
                <tr>
                    <td align="center" style="width: 100%">
                        <div style="text-align: center">
                            <asp:RadioButton ID="rdbAssetImage" runat="server" Text="Asset Images" GroupName="ImageType"
                                Checked="true" AutoPostBack="true" Font-Size="15px" ForeColor="Gray"></asp:RadioButton>
                            &nbsp;
                            <asp:RadioButton ID="rdbIncidentRepair" runat="server" Text="Incident/Repair Images"
                                GroupName="ImageType" AutoPostBack="true" Font-Size="15px" ForeColor="Gray">
                            </asp:RadioButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="spacer25">
                        </div>
                        <asp:LinkButton ID="lnkAddEquipmentImage" runat="server" CssClass="grey2_btn" OnClick="lnkAddEquipmentImage_Click"><span>Add Image</span></asp:LinkButton>
                        <asp:LinkButton ID="lnkAddEquipmentImageDummy" runat="server" CssClass="grey2_btn"
                            Style="display: none;"></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <div style="text-align: center">
                <asp:Label ID="lblErrorImage" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <table width="100%">
                <tr>
                    <td style="width: 97%">
                        <at:ModalPopupExtender ID="ModalEquipmentImage" TargetControlID="lnkAddEquipmentImageDummy"
                            BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlEquipmentImage"
                            CancelControlID="PopupCloseEquipmentImage">
                        </at:ModalPopupExtender>
                        <asp:Panel ID="pnlEquipmentImage" runat="server" Style="display: none;">
                            <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                left: 35%; top: 30%;">
                                <div class="pp_top" style="">
                                    <div class="pp_left">
                                    </div>
                                    <div class="pp_middle">
                                    </div>
                                    <div class="pp_right">
                                    </div>
                                </div>
                                <div class="pp_content_container" style="">
                                    <div class="pp_left" style="">
                                        <div class="pp_right" style="">
                                            <div class="pp_content" style="height: 200px; display: block;">
                                                <div class="pp_loaderIcon" style="display: none;">
                                                </div>
                                                <div class="pp_fade" style="display: block;">
                                                    <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                    <div class="pp_hoverContainer" style="height: 100px; width: 350px; display: none;">
                                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                            style="visibility: visible;">previous</a>
                                                    </div>
                                                    <div id="Div10">
                                                        <div class="pp_inline clearfix">
                                                            <div class="form_popup_box">
                                                                <table cellpadding="5" cellspacing="5">
                                                                    <tr>
                                                                        <td>
                                                                            <label>
                                                                                Date :</label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtImageDate" runat="server" CssClass="w_label datepicker min_w"
                                                                                TabIndex="1"></asp:TextBox>
                                                                            <div id="Div11">
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <label>
                                                                                Name :</label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtImageName" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <label>
                                                                                Image :</label>
                                                                        </td>
                                                                        <td>
                                                                            <input type="file" id="flImage" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <div class="label_bar btn_padinn" style="margin-left: 10px;">
                                                                                <asp:LinkButton ID="btnSaveImage" CssClass="grey2_btn" runat="server" OnClick="btnSaveImage_Click"><span>ADD</span></asp:LinkButton>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="pp_details clearfix" style="width: 371px;">
                                                        <a href="#" id="PopupCloseEquipmentImage" runat="server" class="pp_close">Close</a>
                                                        <p class="pp_description" style="display: none;">
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="pp_bottom" style="">
                                    <div class="pp_left" style="">
                                    </div>
                                    <div class="pp_middle" style="">
                                    </div>
                                    <div class="pp_right" style="">
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <div class="splash_img_pad">
                <asp:DataList ID="dtSplash" runat="server" RepeatDirection="Vertical" RepeatColumns="5"
                    RepeatLayout="Flow" OnItemDataBound="dtSplash_ItemDataBound" OnItemCommand="dtSplash_ItemCommand">
                    <ItemTemplate>
                        <div class="alignleft item">
                            <div>
                                <p class="upload_photo gallery">
                                    <%--  <span class="lt_co"></span><span class="rt_co"></span><span class="lb_co"></span>
                            <span class="rb_co"></span>--%>
                                    <a id="prettyphotoDiv" rel="prettyPhoto[p]" runat="server">
                                        <img id="imgSplashImage" height="182" width="172" runat="server" alt='Loading' />
                                    </a>
                                </p>
                            </div>
                            <div>
                                <p class="upload_photo gallery">
                                    <table cellpadding="0" cellspacing="0" width="100%" frame="box" style="border-color: Black">
                                        <tr>
                                            <td style="width: 33%">
                                                <asp:Label runat="server" ID="lblImageName" Text='<%# "&nbsp;" + Eval("ImageName") %>'></asp:Label>
                                            </td>
                                            <td style="width: 33%">
                                                <asp:Label runat="server" ID="lblImageDate" Text='<%# "&nbsp;" + Eval("ImageDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                            </td>
                                            <td style="width: 33%" align="right">
                                                <div class="alignright item">
                                                    <asp:LinkButton ID="lnkBtnDeleteDoc" CommandArgument='<%# Eval("EquipmentImageID") %>'
                                                        CommandName="DeleteSplashImage" runat="server" ToolTip="Upload Image" OnClientClick="return DeleteConfirmation();"><img src="../Images/delete_products_icon.png" alt="Delete Image" /></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnImageFileName" runat="server" Value='<%# Eval("ImageFileName") %>' />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </p>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <div class="alignnone">
                </div>
            </div>
        </div>
        <div class="spacer25">
        </div>
    </div>
    <%--Equipment Image Stop--%>
    <%--Asset & Parts Specifications Start--%>
    <div runat="server" id="dvFieldPartSpecification" style="margin-left: 50px; margin-right: 50px;">
        <div style="text-align: center">
            <asp:Label ID="lblFieldError" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="form_table">
            <asp:DataList ID="dtlField" runat="server" RepeatDirection="Vertical" RepeatColumns="1"
                RepeatLayout="Flow" OnItemDataBound="dtlField_ItemDataBound">
                <ItemTemplate>
                    <div style="margin-bottom: -10px;">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 38%">
                                <%# Eval("FieldMasterName") %></span>
                            <asp:HiddenField ID="hdnFieldMasterID" runat="server" Value='<%# Eval("FieldMasterID") %>' />
                            <asp:TextBox ID="txtFieldDesc" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <div class="spacer25">
            </div>
            <div class="aligncenter ">
                <asp:LinkButton ID="lnkbtnSaveField" class="grey2_btn" runat="server" OnClick="lnkbtnSaveField_Click"><span>Save</span></asp:LinkButton>
            </div>
        </div>
        <div class="spacer25">
        </div>
    </div>
    <%--Asset & Parts Specifications Stop--%>
    <%--Parts History Start--%>
    <div runat="server" id="dvPartsHistory" visible="false" style="width: 861px; margin: 0 auto;">
        <asp:GridView ID="gvPartsHistory" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblBillingPartsOrderedID" Text='<%# Eval("BillingPartsOrderedID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="PartNumber">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnPartNumber" runat="server" CommandArgument="PartNumber"
                            CommandName="Sort"><span>Part Number</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPartNumber" Text='<%# Eval("PartNumber") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="15%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="CreatedDate">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnDateUsed" runat="server" CommandArgument="DateUsed" CommandName="Sort"><span>Date Used</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDateUsed" Text='<%# Eval("CreatedDate","{0:MM/dd/yyyy}") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Quantity">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnQuantity" runat="server" CommandArgument="Quantity" CommandName="Sort"><span>Qty</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblQuantity" Text='<%# Eval("Quantity") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="5%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle CssClass="centeralign" />
                    <HeaderTemplate>
                        <span>Description</span>
                    </HeaderTemplate>
                    <HeaderStyle />
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDescriptions" Text='<%# Eval("SummaryDescriptions") %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box leftalign" Width="70%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="spacer25">
        </div>
    </div>
    <%--Parts History End--%>
    <%--Repair Order Start--%>
    <div runat="server" id="dvRepairOrder" visible="false" style="width: 861px; margin: 0 auto;">
        <asp:GridView ID="gvRepairOrder" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
            <Columns>
                <asp:TemplateField Visible="false" HeaderText="EquipmentMasterID">                    
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblRepairOrderID" Text='<%# Eval("RepairOrderID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
               
                <asp:TemplateField HeaderText="EquipmentMasterID">
                    <HeaderTemplate>
                        <span>Repair Number</span>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span class="first">
                            <asp:HyperLink ID="lnkRepairOrderID" CommandArgument='<%# Eval("RepairOrderID") %>'
                                runat="server" NavigateUrl='<%# "~/AssetManagement/RepairManagement/RepairProfile.aspx?Page=AssetProfile&RepairOrderId=" + Eval("RepairOrderID").ToString() + "&EquipmentMasterID=" + Eval("EquipmentMasterID").ToString()%>'><%#"RN" + Eval("AutoRepairNumber")%></asp:HyperLink>
                        </span>
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="15%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="EquipmentID">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnEquipmentID" runat="server" CommandArgument="AssetID" CommandName="Sort"><span >Asset ID</span></asp:LinkButton>
                        <asp:PlaceHolder ID="placeholderEquipmentID" runat="server"></asp:PlaceHolder>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblEquipmentID" Text='<%#Eval("EquipmentID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="EquiType">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnEquiType" runat="server" CommandArgument="AssetType" CommandName="Sort"><span >Asset Type</span></asp:LinkButton>
                        <asp:PlaceHolder ID="placeholderEquiType" runat="server"></asp:PlaceHolder>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HiddenField ID="hfEquiType" runat="server" Value='<%# Eval("EquipmetType")%>' />
                        <asp:Label runat="server" ID="lblEquiType" Text='<%# Eval("EquipmetType")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="15%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Base Station">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnBaseStation" runat="server" CommandArgument="BaseStation"
                            CommandName="Sort"><span >Base Station</span></asp:LinkButton>
                        <asp:PlaceHolder ID="placeholderBaseStation" runat="server"></asp:PlaceHolder>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HiddenField ID="hfBaseStation" runat="server" Value='<%# Eval("baseStation")%>' />
                        <asp:Label runat="server" ID="lblBaseStation" Text='<%# Eval("baseStation")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="15%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Status">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnStatus" runat="server" CommandArgument="Status" CommandName="Sort"><span >Status</span></asp:LinkButton>
                        <asp:PlaceHolder ID="placeholderStatus" runat="server"></asp:PlaceHolder>
                        <div class="corner">
                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HiddenField ID="hfStatus" runat="server" Value='<%# Eval("RepairStatus")%>' />
                        <asp:Label runat="server" ID="lblStatus" Text='<%#"&nbsp;" + Eval("RepairStatus")%>'></asp:Label>
                        <div class="corner">
                            <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="15%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="spacer25">
        </div>
    </div>
    <%--Parts History End--%>
    <div runat="server" id="Div3" style="margin-left: 50px; margin-right: 50px;">
        <table class="order_detail" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td colspan="3">
                    <div class="form_table">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box taxt_area clearfix" style="height: 220px">
                            <span class="input_label alignleft" style="height: 218px">
                                <br />
                                Notes/History : </span>
                            <div class="textarea_box alignright">
                                <div class="scrollbar" style="height: 218px">
                                    <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                    </a>
                                </div>
                                <asp:TextBox ID="txtNotesHistory" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                    ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                    <div class="alignnone spacer15">
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 50%">
                    <span id="chkSpan" class="custom-checkbox alignright" runat="server">
                        <asp:CheckBox ID="chkHideEdited" runat="server" AutoPostBack="true" OnCheckedChanged="chkHideEdited_CheckedChanged" />
                    </span>
                </td>
                <td class="chkPostedNote" style="width: 20%; font-size: 12px">
                    &nbsp;Hide Edited Notes
                </td>
                <td style="width: 30%">
                    <div class="rightalign gallery">
                        <asp:LinkButton ID="lnkDummyAddHisotry" class="grey2_btn alignright" runat="server"
                            Style="display: none"></asp:LinkButton>
                        <asp:LinkButton ID="lnkAddHisotry" CommandName="AddNotes" runat="server" CssClass="grey2_btn alignright"><span>+ Add Note</span></asp:LinkButton>
                        <at:ModalPopupExtender ID="ModalPopupExtender1" TargetControlID="lnkAddHisotry" BackgroundCssClass="modalBackground"
                            DropShadow="true" runat="server" PopupControlID="pnlOrderHistory" CancelControlID="closepopuphistory">
                        </at:ModalPopupExtender>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Panel ID="pnlOrderHistory" runat="server" Style="display: none;">
                        <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                            left: 35%; top: 30%;">
                            <div class="pp_top" style="">
                                <div class="pp_left">
                                </div>
                                <div class="pp_middle">
                                </div>
                                <div class="pp_right">
                                </div>
                            </div>
                            <div class="pp_content_container" style="">
                                <div class="pp_left" style="">
                                    <div class="pp_right" style="">
                                        <div class="pp_content" style="height: 228px; display: block;">
                                            <div class="pp_loaderIcon" style="display: none;">
                                            </div>
                                            <div class="pp_fade" style="display: block;">
                                                <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                                    <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                        style="visibility: visible;">previous</a>
                                                </div>
                                                <div id="Div2">
                                                    <div class="pp_inline clearfix">
                                                        <div class="form_popup_box">
                                                            <div class="label_bar">
                                                                <span>Notes/History :
                                                                    <br />
                                                                    <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtOrderNotesHistory"
                                                                        runat="server"></asp:TextBox></span>
                                                            </div>
                                                            <div>
                                                                <asp:LinkButton ID="lnkButtonSaveHistory" CommandName="SAVECACE" class="grey2_btn"
                                                                    runat="server" OnClick="lnkButtonSaveHistory_Click"><span>Save Notes</span></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="pp_details clearfix" style="width: 371px;">
                                                    <a href="#" id="closepopuphistory" runat="server" class="pp_close">Close</a>
                                                    <p class="pp_description" style="display: none;">
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="pp_bottom" style="">
                                <div class="pp_left" style="">
                                </div>
                                <div class="pp_middle" style="">
                                </div>
                                <div class="pp_right" style="">
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
