<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="UniformIssuanceStep4.aspx.cs" Inherits="admin_CompanyStore_CompanyPrograms_UniformIssuanceStep1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">

        // change value in custom dropdown
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
        
        // For Status field 
//        $(function() {
//            var $single = $('#ProductStatus'), singleValues = $single.val(), singleClasses = $single.find('option:selected').attr('class');
//            $("#ctl00_ContentPlaceHolder1_hdnStatus").val(singleValues);
//        });
        // set date image and select date
        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'Date',
                showOn: 'button',
                buttonImage: '../../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });

        //validations

        $().ready(function() {
            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {

                objValMsg = $.xml2json(xml);
                //alert(objValMsg);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtExpireDate: { date: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtExpireDate: { date: replaceMessageString(objValMsg, "ValidDate", "") }
                    }


                }); //validate
            }); //get


            $("#<%=lnkNext.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            }); //click

        });   //ready
        
        
     
    </script>

    <style type="text/css">
        .form_table .calender_l
        {
            position: relative;
            width: 91%;
        }
        
 
    </style>
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
        <div class="uniform_pad">
            <h4>
                How long after the employee meets there issuance date will there "issuance amount"
                expire?</h4>
            <div id="divExpireMonth" runat="server">
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
                <div class="black_middle form_table">
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box">
                        <span class="custom-sel">
                            <asp:DropDownList ID="ddlCreditExpireNumberOfMonth" runat="server">
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="spacer20">
            </div>
            <h4 style="display:none">
                Or
            </h4>
            <div id="divExpireDate" runat="server">
                <div class="tab_content_top_co">
                    <span>&nbsp;</span></div>
                <div class="tab_content form_table">
                    <div class="calender_l">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box supplier_annual_date">
                            <span class="input_label">Expire Date</span>
                            <asp:TextBox ID="txtExpireDate" runat="server" CssClass="w_label datepicker min_w"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </div>
                <div class="tab_content_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="spacer20">
            </div>
            <h4>
                If the employee is furloughed will the system put the calculation of his issuance
                time on hold, and once they are hired back it will resume the calculation of time
                from the last purchase date to the furloughed date, then start calculation from
                the re-hired date until it adds up to a total of the time you selected in Step 3.</h4>
            <div>
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
                <div class="black_middle form_table">
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box">
                        <span class="custom-sel">
                            <asp:DropDownList ID="ddlEmployeeActiveRule" runat="server">
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="spacer20">
            </div>
            <h4>
                Do you want to set a reminder schedule to inform the employee that there issuance
                date is approaching, if so select the number of months before that we will send
                an automatic email notice.</h4>
            <div>
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
                <div class="black_middle form_table">
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box">
                        <span class="custom-sel">
                            <asp:DropDownList ID="ddlReminder1" runat="server">
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="black_middle form_table">
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box">
                        <span class="custom-sel">
                            <asp:DropDownList ID="ddlReminder2" runat="server">
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="black_middle form_table">
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box">
                        <span class="custom-sel">
                            <asp:DropDownList ID="ddlReminder3" runat="server">
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="black_middle form_table">
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box">
                        <span class="custom-sel">
                            <asp:DropDownList ID="ddlExpirationReminder" runat="server">
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="spacer20">
            </div>
            <h4>
                Status
            </h4>
            <div>
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
                <div class="black_middle form_table">
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box">
                    <span class="input_label">Status :</span>
                    <label class="dropimg_width230">
                        <span class="custom-sel label-sel-small">
                            <asp:DropDownList ID="ddlStatus" runat="server" onchange="pageLoad(this,value);">
                            </asp:DropDownList>
                        </span>
                    </label>
                </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
            </div>
        </div>
        <div class="spacer25">
        </div>
        <div class="bot_alert next">
            <%--<a href="UniformIssuanceStep5.aspx"" title="Next Step 5">Next Step 5</a>--%>
            <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click">Next Step 6</asp:LinkButton>
        </div>
        <div class="bot_alert prev">
            <%--<a href="UniformIssuanceStep3.aspx" title="Prev Step 2">Prev Step 3</a>--%>
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        </div>
    </div>
</asp:Content>
