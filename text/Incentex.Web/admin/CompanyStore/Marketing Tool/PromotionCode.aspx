<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="PromotionCode.aspx.cs" Inherits="admin_CompanyStore_Marketing_Tool_PromotionCode" Title="Promotion Code" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

  <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    
    
     <script type="text/javascript" language="javascript">
         $().ready(function() {
             $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                 objValMsg = $.xml2json(xml); $("#aspnetForm").validate(
                {
                    rules:
                    {   
                        ctl00$ContentPlaceHolder1$txtPromotionCode: { required: true },
                        ctl00$ContentPlaceHolder1$txtStartDate: { required: true },
                        ctl00$ContentPlaceHolder1$txtEndDate: { required: true }
                    },
                    messages:
                    {  
                        ctl00$ContentPlaceHolder1$txtPromotionCode: { required: replaceMessageString(objValMsg, "Required", "Discount") },
                        ctl00$ContentPlaceHolder1$txtStartDate: { required: replaceMessageString(objValMsg, "Required", "Start Date") },
                        ctl00$ContentPlaceHolder1$txtEndDate: { required: replaceMessageString(objValMsg, "Required", "End Date") }
                    }
                });
             });
             $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                 return $('#aspnetForm').valid();
             });
         });
                $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
            
        });
    </script>


        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
         <div class="form_pad">      
        <div class="form_table">
            <table class="dropdown_pad ">
              <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Workgroup</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlWorkgroup" runat="server" onchange="pageLoad(this,value)" 
                                    AutoPostBack="true" onselectedindexchanged="ddlWorkgroup_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>                   
                </tr>
                 <%--<tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Promotion for Selected Item</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlPromotionFor" runat="server" onchange="pageLoad(this,value)">
                                    <asp:ListItem Value="0" Text="No" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>                   
                </tr>--%>
                 <tr runat="server" id="trItem">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Item</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlItem" runat="server" onchange="pageLoad(this,value)">
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
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 38%">Apply Discount To</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlPriceLevel" runat="server" onchange="pageLoad(this,value)">
                                    </asp:DropDownList>
                                </span>                              
                            </label>
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
                            <span class="input_label" style="width: 38%">Type of Promotion</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlPromotionType" runat="server" onchange="pageLoad(this,value)">
                                    <asp:ListItem Value="0" Text="Free Shipping" Selected="True"></asp:ListItem> 
                                    </asp:DropDownList>
                                </span>                              
                            </label>
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
                            <span class="input_label" style="width: 38%">Promotion Code</span>
                            <asp:TextBox ID="txtPromotionCode" runat="server" MaxLength="5" CssClass="w_label"></asp:TextBox>                            
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
               
                
                 <tr>
               
                    <td id="Td1"  runat="server">
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%;">Start Date</span>
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="cal_w datepicker1"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                   
                </tr>
                <tr>
               
                    <td id="Td2"  runat="server">
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%;">End Date</span>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="cal_w datepicker1"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                   
                </tr>
              
                <tr>
                    <td class="spacer10">
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" 
                            ToolTip="Save Basic Information" onclick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
                        <%--<a href="#" class="grey_btn" title="Save Information"><span>Save Information</span></a>--%>
                    </td>
                </tr>
            </table>
        </div>
       
    </div>
</asp:Content>

